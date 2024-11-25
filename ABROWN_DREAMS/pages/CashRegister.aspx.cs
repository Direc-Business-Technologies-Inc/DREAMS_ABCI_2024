using ABROWN_DREAMS.Services;
using ABROWN_DREAMS.wcf;
using DirecLayer;
using Sap.Data.Hana;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABROWN_DREAMS
{
	public partial class CashRegister : Page
	{
		DirecWebService ws = new DirecWebService();
		DirecService wcf = new DirecService();
		SAPHanaAccess hana = new SAPHanaAccess();
		DataTable dtPayments = new DataTable();

		protected void Page_Load(object sender, EventArgs e)
		{
			//Page.Form.Attributes.Add("enctype", "multipart/form-data");
			//Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);

			Session["ReportType"] = "";

			if (Session["UserID"] == null)
			{
				alertMsg("Session expired!", "error");
				Response.Redirect("~/pages/Login.aspx");
			}


			if (!IsPostBack)
			{
				LoadBuyers();
				LoadCreditMethod();
				LoadCardBrandAccount();

				tDocDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
				tSurChargeDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
				ViewState["dtPayments"] = null;
				if ((DataTable)ViewState["dtPayments"] == null)
				{ LoadGridView("dtPayments"); }
				else { LoadData(gvPayments, "dtPayments"); }

				//show buyer selection
				ScriptManager.RegisterStartupScript(this, this.GetType(), "close", "showBuyer();", true);
				pnlBuyerInfo.Visible = false;
				//loadReceiptNo();

				ViewState["chkTagMISC"] = 0;
				ViewState["chkTagDP"] = 0;
				ViewState["ctrFordivAdvancePrincipal"] = 0;
				ViewState["MaxLBTerm"] = 0;
				ViewState["MaxLBTermDate"] = "";
				ViewState["MinLBDate"] = "";
				ViewState["MinLBTerm"] = "";


				//2023-06-07 : GET LAST TERM
				ViewState["MaxTermExcess"] = 0;
				ViewState["MaxTypeExcess"] = 0;
				ViewState["TotalPayment"] = 0;

				txtSearchBuyer.Focus();

				lblApplyToPrincipal.Text = "NO";
				lblApplyToPrincipal.ForeColor = System.Drawing.Color.Black;
				divAdvancePrincipal.Visible = false;


				cancelCWTs();

				//2023-09-19 : CANCEL ALL AR INVOICES WITH SURCHARGE TRANSACTIONTYPE AND CANCELLED INCOMING PAYMENTS
				cancelSurchageInvoices();

				//2023-10-11 : CANCEL ALL AR INVOICES WITH INTEREST TRANSACTIONTYPE AND CANCELLED INCOMING PAYMENTS
				cancelInterestInvoices();

				//2023-10-09 : CREATE JOURNAL ENTRY TO OFFSET ALL BALANCE OF AR DOWNPAYMENTS WITH CANCELLED INCOMING PAYMENTS
				cancelDFC();

				//2023-10-17 : CREATE JOURNAL ENTRY TO CANCEL JE1 -- PARTIAL PAYMET RELATED JE POSTING
				cancelJEForPartialPayments("JE5", "1RDL");
				//2023-10-17 : CREATE JOURNAL ENTRY TO CANCEL JE3 -- PARTIAL PAYMET RELATED JE POSTING
				cancelJEForPartialPayments("JE6", "1MSC");

				//2023-10-19 : CANCEL ALL AR INVOICES WITH MISCELLANEOUS TRANSACTIONTYPE AND CANCELLED INCOMING PAYMENTS
				cancelMiscellaneousInvoices();

				//2023-11-10 : CREATE JOURNAL ENTRY TO CANCEL NLTS FOR LOI -- TCP
				cancelNLTS("RNLT", "NLTS");

				//2023-11-10 : CREATE JOURNAL ENTRY TO CANCEL MLOI FOR LOI -- MISC
				cancelNLTS("RMLO", "MLOI");


			}

			//if (IsPostBack)
			//{
			//    if (Request.Params.Get("__EVENTTARGET") != null && Request.Params.Get("__EVENTTARGET").EndsWith("chkSel"))
			//    {
			//        var test1 = "";
			//        // postback from the checkbox, do nothing
			//        ScriptManager.RegisterStartupScript(this, this.GetType(), "openpage", "console.log('test');", true);
			//    }
			//    //else
			//    //{
			//    //    var test1 = "";
			//    //    // postback from the button, open the new window
			//    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "openpage", "window.open('ReportViewer.aspx');", true);
			//    //}   
			//}


			dtPayments.Columns.AddRange(new DataColumn[3] {
						new DataColumn("LineNum"),
						new DataColumn("Type"),
						new DataColumn("Amount")
				});
		}

		void clearReceiptNo()
		{
			txtOR.Text = string.Empty;
			txtPR.Text = string.Empty;
			txtAR.Text = string.Empty;

			tORDate.Text = string.Empty;
			txtPRDate.Text = string.Empty;
			txtARDate.Text = string.Empty;
		}

		void loadReceiptNo(string Type)
		{
			try
			{
				DataTable dt = ws.GetReceiptNumber(txtProj.Text, Type).Tables[0];

				if (dt.Rows.Count > 0)
				{
					string generatedNo = DataAccess.GetData(dt, 0, "Number", "0").ToString();

					if (Type == "OR")
					{
						txtOR.Text = generatedNo;
						txtPR.Text = string.Empty;
						txtAR.Text = string.Empty;
					}
					else if (Type == "PR")
					{
						txtPR.Text = generatedNo;
						txtOR.Text = string.Empty;
						txtAR.Text = string.Empty;
					}
					else
					{
						txtAR.Text = generatedNo;
						txtOR.Text = string.Empty;
						txtPR.Text = string.Empty;
					}
				}

				tORDate.Text = tDocDate.Value;
				txtPRDate.Text = tDocDate.Value;
				txtARDate.Text = tDocDate.Value;
			}
			catch (Exception ex)
			{
				alertMsg(ex.Message, "warning");
			}
		}

		void LoadHouseList()
		{
			gvHouseLot.DataSource = ws.GetBuyersProject(lblID.Text);
			gvHouseLot.DataBind();

			gvDuePayments.Visible = false;
			gvDownPayments.Visible = false;

			gvDuePayments.DataSource = null;
			gvDuePayments.DataBind();
			gvDownPayments.DataSource = null;
			gvDownPayments.DataBind();

			//check if house/lot == 1 row then load due payments
			if (gvHouseLot.Rows.Count == 1)
			{
				Label lblDocEntry = (Label)gvHouseLot.Rows[0].FindControl("lblDocEntry");
				Label lblProject = (Label)gvHouseLot.Rows[0].FindControl("lblProject");
				Label lblProjCode = (Label)gvHouseLot.Rows[0].FindControl("lblProjCode");
				Label lblBlock = (Label)gvHouseLot.Rows[0].FindControl("lblBlock");
				Label lblLot = (Label)gvHouseLot.Rows[0].FindControl("lblLot");
				Label lblModel = (Label)gvHouseLot.Rows[0].FindControl("lblModel");
				Label lblStatus = (Label)gvHouseLot.Rows[0].FindControl("lblStatus");
				Label lblPhase = (Label)gvHouseLot.Rows[0].FindControl("lblPhase");
				Label lblFinscheme = (Label)gvHouseLot.Rows[0].FindControl("lblFinscheme");
				Label lblFinschemeCode = (Label)gvHouseLot.Rows[0].FindControl("lblFinschemeCode");
				Label lblBlock1 = (Label)gvHouseLot.Rows[0].FindControl("lblBlock1");
				Label lblLot1 = (Label)gvHouseLot.Rows[0].FindControl("lblLot1");
				Label lblModel1 = (Label)gvHouseLot.Rows[0].FindControl("lblModel1");
				Label lblApproved = (Label)gvHouseLot.Rows[0].FindControl("lblApproved");
				ViewState["Status"] = lblStatus.Text;
				ViewState["BuyerStatus"] = lblApproved.Text == "" || lblApproved.Text == "?" ? "N" : lblApproved.Text;
				ViewState["SQDocEntry"] = int.Parse(lblDocEntry.Text);
				ViewState["FinCode"] = lblFinschemeCode.Text;

				LoadPaymentHistory(int.Parse(lblDocEntry.Text));


				LoadDemandLetters();

				//// #### GENERATE DOWNPAYMENT BREAKDOWN HERE

				//** highlight row 
				gvHouseLot.Rows[0].BackColor = System.Drawing.ColorTranslator.FromHtml("#A1DCF2");
				//** hide reservation gv
				gvDuePayments.Visible = false;

				//GET QUOTATION DATA FROM SQL[OQUT]
				DataTable dtQuotation = ws.GetQuotationData(int.Parse(lblDocEntry.Text)).Tables[0];
				if (DataAccess.Exist(dtQuotation))
				{
					ReloadData(dtQuotation);
				}

				gvDownPayments.Visible = true;
				gvDownPayments.DataSource = ws.GetPaymentBreakdown(int.Parse(lblDocEntry.Text));
				gvDownPayments.DataBind();
				//gvDuePayments.DataSource = ws.GetDuePayments(lblID.Text, lblProjCode.Text, lblBlock1.Text, lblLot1.Text);
				if (gvDownPayments.Rows.Count == 0)
				{
					pnlPayment.Enabled = false;
				}
				else
				{
					pnlPayment.Enabled = true;
					ViewState["DPEntry"] = int.Parse(gvDownPayments.Rows[0].Cells[0].Text);
				}

				NewComputeTotal(gvPayments);
				tDocDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
				tSurChargeDate.Text = DateTime.Today.ToString("yyyy-MM-dd");

			}
		}

		void LoadPaymentHistory(int DocEntry)
		{

			//2023-07-02 : CHANGE SOURCE
			//gvHistory.DataSource = ws.GetPaymentHistory(DocEntry);
			gvHistory.DataSource = ws.GetPaymentHistoryNew(DocEntry);


			gvHistory.DataBind();
			HideColumnInHistory();

			DataTable dt = new DataTable();
			foreach (GridViewRow rows in gvHistory.Rows)
			{
				if (rows.Cells[7].Text == "Yes")
				{
					rows.BackColor = System.Drawing.ColorTranslator.FromHtml("#808080");

					//--- DISABLED PRINT BUTTONS ---//
					//((LinkButton)rows.Cells[9].FindControl("btnPrint")).Attributes.Add("disabled", "disabled");
					//((LinkButton)rows.Cells[9].FindControl("btnPrint")).Enabled = false;
					//((LinkButton)rows.Cells[10].FindControl("btnPrintAR")).Attributes.Add("disabled", "disabled");
					//((LinkButton)rows.Cells[10].FindControl("btnPrintAR")).Enabled = false;
					//((LinkButton)rows.Cells[11].FindControl("btnPrintPR")).Attributes.Add("disabled", "disabled");
					//((LinkButton)rows.Cells[11].FindControl("btnPrintPR")).Enabled = false;
				}

			}

		}
		void LoadAccounts()
		{
			if (gvAccounts.Rows.Count == 0)
			{
				ViewState["GLAccounts"] = ws.GetGLAccounts();
				gvAccounts.DataSource = (DataSet)ViewState["GLAccounts"];
				gvAccounts.DataBind();
			}
		}

		void LoadDemandLetters()
		{
			//LOAD DATE OF DEMAND LETTERS
			string qry = $@"SELECT * FROM ""QUT9"" WHERE ""DocEntry"" = '{ViewState["SQDocEntry"].ToString()}'";
			DataTable dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
			if (dt.Rows.Count > 0)
			{
				txtDemand1.Value = DataAccess.GetData(dt, 0, "Level1Date", "").ToString() == "" ? null : Convert.ToDateTime(DataAccess.GetData(dt, 0, "Level1Date", "")).ToString("yyyy-MM-dd");
				txtDemand2.Value = DataAccess.GetData(dt, 0, "Level2Date", "").ToString() == "" ? null : Convert.ToDateTime(DataAccess.GetData(dt, 0, "Level2Date", "")).ToString("yyyy-MM-dd");
				txtDemand3.Value = DataAccess.GetData(dt, 0, "Level3Date", "").ToString() == "" ? null : Convert.ToDateTime(DataAccess.GetData(dt, 0, "Level3Date", "")).ToString("yyyy-MM-dd");
				txtDemand4.Value = DataAccess.GetData(dt, 0, "Level4Date", "").ToString() == "" ? null : Convert.ToDateTime(DataAccess.GetData(dt, 0, "Level4Date", "")).ToString("yyyy-MM-dd");
				txtDemand5.Value = DataAccess.GetData(dt, 0, "Level5Date", "").ToString() == "" ? null : Convert.ToDateTime(DataAccess.GetData(dt, 0, "Level5Date", "")).ToString("yyyy-MM-dd");
			}
		}

		void LoadBuyers()
		{
			if (gvBuyers.Rows.Count == 0)
			{
				ViewState["Buyers"] = ws.BuyersSelection();
				gvBuyers.DataSource = (DataSet)ViewState["Buyers"];
				gvBuyers.DataBind();
			}
		}


		void LoadCreditMethod()
		{
			gvPymtMethod.DataSource = ws.GetCreditPaymentMethod();
			gvPymtMethod.DataBind();
		}

		void LoadCardBrandAccount()
		{
			gvCardBrandAccount.DataSource = ws.GetCardBrandAccount();
			gvCardBrandAccount.DataBind();
		}

		void ReloadData(DataTable d)
		{
			//CHECKING OF LOI
			DataTable dtLTS = new DataTable();
			//dtLTS = hana.GetDataDS($@"SELECT ""U_LTSIssueDate"" FROM ""OPRJ"" WHERE ""PrjCode"" = '{d.Rows[0]["ProjCode"]}'", hana.GetConnection("SAPHana")).Tables[0];
			//DateTime LTS = Convert.ToDateTime(DataAccess.GetData(dtLTS, 0, "U_LTSIssueDate", DateTime.MinValue.ToString()));
			//DateTime DocDate = Convert.ToDateTime(d.Rows[0]["DocDate"]);

			lblLoi.Text = d.Rows[0]["LOI"].ToString();
			lblLTSNo.Text = d.Rows[0]["LTSNo"].ToString();

			//Session["QuotationID"] = d.Rows[0]["DocEntry"].ToString();
			//** customer details **
			txtFName.Text = d.Rows[0]["FirstName"].ToString();
			txtLName.Text = d.Rows[0]["LastName"].ToString();
			txtMName.Text = d.Rows[0]["MiddleName"].ToString();
			txtDocNum.Text = d.Rows[0]["DocNum"].ToString();
			txtProj.Text = d.Rows[0]["ProjCode"].ToString();
			txtBlock.Text = d.Rows[0]["Block"].ToString();
			txtLot.Text = d.Rows[0]["Lot"].ToString();
			txtModel.Text = string.IsNullOrWhiteSpace(d.Rows[0]["Model"].ToString()) ? "N/A" : d.Rows[0]["Model"].ToString();
			txtPhase.Text = d.Rows[0]["Phase"].ToString();
			lblTCPFinScheme.Text = d.Rows[0]["FinancingScheme"].ToString();

			//2023-05-31 : REQUESTED BY MS KATE : ADD MISC FINANCING SCHEME
			lblMiscFinScheme.Text = d.Rows[0]["MiscFinancingScheme"].ToString();


			txtBookStatus.Text = d.Rows[0]["BookStatus"].ToString();

			lblNETTCP.Text = SystemClass.ToCurrency(d.Rows[0]["NetTCP_new"].ToString()); //Das FROM OQUT Ito kinukuha
			lblSalesAgent.Text = d.Rows[0]["EmployeeName"].ToString();

			//2023-06-01 : ADDED BROKER NAME IN CASH REGISTER
			lblCoBorrower.Text = d.Rows[0]["Comaker1"].ToString();


			//2023-07-06 : ADD TAX CLASSIFICATION
			lblTaxClassification.Text = d.Rows[0]["TaxClassification"].ToString();


			//2023-06-21 : CHANGE SOURCE OF FETCHING TO STORED PROC
			//DataTable _dtTotalPaid1 = hana.GetData($@" CALL USRSP_DBTI_IC_Ledger_SAO ({ViewState["SQDocEntry"]}) ", hana.GetConnection("SAPHana"));
			DataTable _dtTotalPaid1 = hana.GetData($@"CALL USRSP_DBTI_IC_Ledger_SAO ({ViewState["SQDocEntry"]}, '{Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd")}')", hana.GetConnection("SAPHana"));

			double _TCPPayment = Convert.ToDouble(DataHelper.DataTableRet(_dtTotalPaid1, 0, "NEG_PRINCIPAL", "0"));
			double _TCPPaymentPercent = Convert.ToDouble(DataHelper.DataTableRet(_dtTotalPaid1, 0, "TotalTCP", "0"));
			//lblTotalPayment.Text = SystemClass.ToCurrency(d.Rows[0]["TotalPayment"].ToString());
			//lblPercentPaid.Text = SystemClass.ToCurrency(d.Rows[0]["PercentPayment"].ToString()) + "%";
			lblTotalPayment.Text = SystemClass.ToCurrency(_TCPPayment.ToString());
			lblPercentPaid.Text = SystemClass.ToCurrency(_TCPPaymentPercent.ToString()) + "%";

			//2023-06-08 : PULL TOTAL MISC FEE FROM OQUT
			//lblTotalMiscFee.Text = SystemClass.ToCurrency(d.Rows[0]["TotalMiscFee"].ToString());
			lblTotalMiscFee.Text = SystemClass.ToCurrency(d.Rows[0]["AddMiscFees"].ToString());

			//2023-08-29 : CHANGE SOURCE OF MISCELLANEOUS PAID (TO STORED PROC)
			//lblTotalMiscPaid.Text = SystemClass.ToCurrency(d.Rows[0]["MiscPayment"].ToString());
			DataTable _dtTotalPaidMisc1 = hana.GetData($@" CALL USRSP_DBTI_MISC_Ledger_SAO ({ViewState["SQDocEntry"]}) ", hana.GetConnection("SAPHana"));
			double _MiscPayment = Convert.ToDouble(DataHelper.DataTableRet(_dtTotalPaidMisc1, 0, "NEG_PRINCIPAL", "0"));
			lblTotalMiscPaid.Text = SystemClass.ToCurrency(_MiscPayment.ToString());




			ViewState["ProjCode"] = d.Rows[0]["ProjCode"].ToString();
			ViewState["FirstName"] = d.Rows[0]["FirstName"].ToString();
			ViewState["LastName"] = d.Rows[0]["LastName"].ToString();
			ViewState["Block"] = d.Rows[0]["Block"].ToString();
			ViewState["Lot"] = d.Rows[0]["Lot"].ToString();
			ViewState["Model"] = d.Rows[0]["Model"].ToString();
			ViewState["Size"] = d.Rows[0]["Size"].ToString();
			ViewState["Feature"] = d.Rows[0]["Feature"].ToString();
			ViewState["SalesType"] = d.Rows[0]["SalesType"].ToString();
			ViewState["AddonCardCode"] = d.Rows[0]["CardCode"].ToString();
			ViewState["Discount"] = d.Rows[0]["TotalDisc"].ToString();
			ViewState["RsvDate"] = d.Rows[0]["DocDate"].ToString();
			ViewState["LotArea"] = d.Rows[0]["LotArea"].ToString();
			ViewState["ReservationFee"] = d.Rows[0]["ReservationFee"].ToString();
			ViewState["ItemCode"] = d.Rows[0]["ItemCode"].ToString();
			ViewState["ItemCodeOC"] = d.Rows[0]["ItemCodeOC"].ToString();
			ViewState["DPAmount"] = d.Rows[0]["DPAmount"].ToString();
			ViewState["LAmount"] = d.Rows[0]["LAmount"].ToString();
			ViewState["FinancingScheme"] = d.Rows[0]["FinancingScheme"].ToString();
			//ViewState["DAS"] = d.Rows[0]["Das"].ToString();test

			DataTable dtThreshold = new DataTable();
			dtThreshold = hana.GetDataDS($@"SELECT ""U_ThresholdAmount"" FROM ""@PRODUCTTYPE"" WHERE ""Code"" = '{d.Rows[0]["HouseStatus"].ToString()}'", hana.GetConnection("SAPHana")).Tables[0];
			string threshold = (string)DataAccess.GetData(dtThreshold, 0, "U_ThresholdAmt", "0");
			ViewState["threshold"] = threshold;

			ViewState["DAS"] = d.Rows[0]["oDas"].ToString();

			double netDas = Convert.ToDouble(d.Rows[0]["DAS"]);
			if (d.Rows[0]["Allowed"].ToString() == "Approved")
			{
				netDas = netDas - Convert.ToDouble(d.Rows[0]["AddtlCharges"]);
			}

			ViewState["NetDas"] = netDas.ToString();
			ViewState["PromoDisc"] = d.Rows[0]["PromoDisc"].ToString();
			ViewState["Misc"] = d.Rows[0]["Misc"].ToString();
			ViewState["VAT"] = d.Rows[0]["Vat"].ToString();
			ViewState["AddtlCharges"] = d.Rows[0]["AddtlCharges"].ToString();




		}

		void LoadGridView(string viewState)
		{
			DataTable dt = new DataTable();
			if (viewState == "dtPayments")
			{
				dt.Columns.Add("LineNum", typeof(int));
				dt.Columns.Add("Type", typeof(string));
				dt.Columns.Add("Amount", typeof(double));
				//2

				//check details
				dt.Columns.Add("CheckNo", typeof(string));
				dt.Columns.Add("BankCode", typeof(string));
				dt.Columns.Add("Bank", typeof(string));
				dt.Columns.Add("Branch", typeof(string));
				dt.Columns.Add("DueDate", typeof(string));
				dt.Columns.Add("AccountNum", typeof(string));
				//8

				//credit details
				dt.Columns.Add("CreditCard", typeof(string));
				dt.Columns.Add("CreditAcctCode", typeof(string));
				dt.Columns.Add("CreditAcct", typeof(string));
				dt.Columns.Add("CreditCardNumber", typeof(string));
				dt.Columns.Add("ValidUntil", typeof(string));
				dt.Columns.Add("IdNum", typeof(string));
				dt.Columns.Add("TelNum", typeof(string));
				//15

				dt.Columns.Add("PymtTypeCode", typeof(string));
				dt.Columns.Add("PymtType", typeof(string));
				dt.Columns.Add("NumOfPymts", typeof(string));
				dt.Columns.Add("VoucherNum", typeof(string));
				dt.Columns.Add("Id", typeof(string));
				dt.Columns.Add("OR", typeof(string));
				//21

				//CHECK DEPOSIT DETAILS
				dt.Columns.Add("DepositedBankID", typeof(string));
				dt.Columns.Add("DepositedBank", typeof(string));
				dt.Columns.Add("DepositedBranch", typeof(string));
				dt.Columns.Add("CheckAccount", typeof(string));
				//25

				//INTERBANK
				dt.Columns.Add("InterBankDate", typeof(string));
				dt.Columns.Add("InterBankBank", typeof(string));
				dt.Columns.Add("InterBankGLAcc", typeof(string));
				dt.Columns.Add("InterBankAccNo", typeof(string));
				dt.Columns.Add("InterBankAmount", typeof(double));
				//30

				//OTHERS
				dt.Columns.Add("OthersModeOfPaymentCode", typeof(string));
				dt.Columns.Add("OthersModeOfPayment", typeof(string));
				dt.Columns.Add("OthersAmount", typeof(string));
				dt.Columns.Add("OthersReferenceNo", typeof(string));
				dt.Columns.Add("OthersGLAccountCode", typeof(string));
				//35

				dt.Columns.Add("OthersGLAccountName", typeof(string));
				dt.Columns.Add("OthersPaymentDate", typeof(string));
				//37

				dt.Columns.Add("CheckPDCId", typeof(string));
				dt.Columns.Add("CreditCardBrandAccountCode", typeof(string));
				dt.Columns.Add("CreditCardBrandAccountName", typeof(string));
				//40

				dt.Columns.Add("CashDiscountTaggingTerm", typeof(string));
				dt.Columns.Add("CashDiscountTaggingType", typeof(string));





				ViewState[viewState] = dt;
				if (viewState == "gvPayments")
				{ LoadData(gvPayments, viewState); }
			}
		}

















		void LoadData(GridView gv, string viewState)
		{
			gv.DataSource = (DataTable)ViewState[viewState];
			gv.DataBind();
		}

		#region TEXT SEARCH
		[WebMethod]
		public static List<string> TextSearch(string search)
		{
			List<string> searchresult = new List<string>();
			using (HanaConnection con = new HanaConnection(DataAccess.con("Addon")))
			{
				using (HanaCommand cmd = new HanaCommand())
				{
					cmd.CommandText = "SearchBuyer";
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Connection = con;
					con.Open();
					cmd.Parameters.AddWithValue("Search", search);
					HanaDataReader dr = cmd.ExecuteReader();
					while (dr.Read())
					{
						searchresult.Add(dr["LastName"].ToString());
					}
					con.Close();
					return searchresult;
				}
			}
		}
		#endregion

		protected void gvPayments_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			int index = Convert.ToInt32(e.CommandArgument);
			ViewState["paymentindex"] = index;
			ViewState["linenum"] = gvPayments.Rows[index].Cells[0].Text;
			string linenum = gvPayments.Rows[index].Cells[0].Text;
			string type = gvPayments.Rows[index].Cells[1].Text;

			if (e.CommandName.Equals("Del"))
			{
				confirmation("Are you sure you want to remove selected payment?", "removepayment");
			}
			else if (e.CommandName.Equals("Edt"))
			{
				if (type == "Cash")
				{
					txtCashAmount.Text = double.Parse(gvPayments.Rows[index].Cells[2].Text).ToString();
					bAddCash.Text = "Update";
					ScriptManager.RegisterStartupScript(this, this.GetType(), "showCash", "showCash();", true);
				}
				else if (type == "Interbank")
				{
					txtInterBranchDate.Value = gvPayments.Rows[index].Cells[26].Text;
					txtInterBankGLAcc.Value = gvPayments.Rows[index].Cells[28].Text;
					txtInterBankBank.Value = gvPayments.Rows[index].Cells[27].Text;
					txtInterAccounts.Value = gvPayments.Rows[index].Cells[29].Text;
					txtInterAmount.Text = double.Parse(gvPayments.Rows[index].Cells[2].Text).ToString();

					btnAddInter.Text = "Update Interbranch";
					ScriptManager.RegisterStartupScript(this, this.GetType(), "Interbranch", "showInter();", true);
				}
				else if (type.Contains("Others"))
				{
					txtOthersPaymentDate.Value = gvPayments.Rows[index].Cells[37].Text;
					txtOthersModeOfPaymentCode.Value = gvPayments.Rows[index].Cells[31].Text;
					txtOthersModeOfPayment.Value = gvPayments.Rows[index].Cells[32].Text;
					txtOthersAmount.Text = double.Parse(gvPayments.Rows[index].Cells[33].Text).ToString();
					txtOthersReferenceNo.Value = gvPayments.Rows[index].Cells[34].Text;
					txtOthersGLAccountCode.Value = gvPayments.Rows[index].Cells[35].Text;
					txtOthersGLAccountName.Value = gvPayments.Rows[index].Cells[36].Text;

					btnAddOthers.Text = "Update Payment";

					ScriptManager.RegisterStartupScript(this, this.GetType(), "showOthers", "showOthers();", true);
				}
				else if (type == "Credit")
				{
					//load check details
					string creditamt = gvPayments.Rows[index].Cells[2].Text;
					string creditcard = gvPayments.Rows[index].Cells[9].Text;
					string crediacctcode = gvPayments.Rows[index].Cells[10].Text;
					string crediacct = gvPayments.Rows[index].Cells[11].Text;
					string creditcardnum = gvPayments.Rows[index].Cells[12].Text;
					//string validuntil = gvPayments.Rows[index].Cells[13].Text.Replace("/1/", "/");
					string validuntil = $"{gvPayments.Rows[index].Cells[13].Text.Substring(0, 3)}{gvPayments.Rows[index].Cells[13].Text.Substring(8, 2)}";
					string idnum = gvPayments.Rows[index].Cells[14].Text;
					string telnum = gvPayments.Rows[index].Cells[15].Text;
					string pymttypecode = gvPayments.Rows[index].Cells[16].Text;
					string pymttype = gvPayments.Rows[index].Cells[17].Text;
					string numofpymts = gvPayments.Rows[index].Cells[18].Text;
					string vouchernum = gvPayments.Rows[index].Cells[19].Text;

					string CardBrandAccountCode = gvPayments.Rows[index].Cells[39].Text;
					string CardBrandAccountName = gvPayments.Rows[index].Cells[40].Text;

					if (idnum == "&nbsp;")
						idnum = string.Empty;
					if (telnum == "&nbsp;")
						telnum = string.Empty;


					txtCreditAmount.Text = double.Parse(creditamt).ToString();
					txtCreditCard.Value = creditcard;
					txtCreditAccountCode.Value = crediacctcode;
					txtCreditAccount.Value = crediacct;
					txtCreditCardNum.Value = creditcardnum;
					txtValidUntil.Text = validuntil;
					txtIDNum.Value = idnum;
					txtTelNo.Value = telnum;
					txtCreditMethodCode.Value = pymttypecode;
					txtCreditMethod.Value = pymttype;
					txtNoOfPayments.Value = numofpymts;
					txtVoucherNum.Value = vouchernum;
					txtCardBrandAccountCode.Value = CardBrandAccountCode;
					txtCardBrandAccountName.Value = CardBrandAccountName;


					btnAddCredit.Text = "Update Credit";

					ScriptManager.RegisterStartupScript(this, this.GetType(), "showCreditPayment", "showCreditPayment();", true);
				}
				else if (!string.IsNullOrWhiteSpace(gvPayments.Rows[index].Cells[3].Text))
				{
					//load check details
					string checkamt = gvPayments.Rows[index].Cells[2].Text;
					string checkno = gvPayments.Rows[index].Cells[3].Text;
					string bankcode = gvPayments.Rows[index].Cells[4].Text;
					string bank = gvPayments.Rows[index].Cells[5].Text;
					string branch = Server.HtmlDecode(gvPayments.Rows[index].Cells[6].Text);
					string duedate = gvPayments.Rows[index].Cells[7].Text;
					string acctnum = Server.HtmlDecode(gvPayments.Rows[index].Cells[8].Text);

					string depBankID = Server.HtmlDecode(gvPayments.Rows[index].Cells[22].Text);
					string depBank = Server.HtmlDecode(gvPayments.Rows[index].Cells[23].Text);
					string depBranch = Server.HtmlDecode(gvPayments.Rows[index].Cells[24].Text);
					string depCheckAcc = Server.HtmlDecode(gvPayments.Rows[index].Cells[25].Text);

					txtCheckAmount.Text = double.Parse(checkamt).ToString();
					txtCheckNo.Value = checkno;
					txtBankCode.Value = bankcode;
					txtCheckBank.Value = bank;
					txtCheckBranch.Value = branch;
					txtCheckDate.Text = duedate;
					txtAccount.Value = acctnum;

					txtDepositBankID.Value = (depBankID == "0" ? "" : depBankID);
					txtDepositBank.Value = (depBank == "0" ? "" : depBank);
					txtDepositedBranch.Value = (depBranch == "0" ? "" : depBranch);
					txtCheckAccount.Value = depCheckAcc;


					btnAddCheck.Text = "Update Check";

					ScriptManager.RegisterStartupScript(this, this.GetType(), "showCheck", "showCheck();", true);
				}

			}
		}


		void JEForNoLTSNumber(CashRegisterService cashRegister, string projectCode, string SapCardCode, double payment, int SODocEntry, string FinancingScheme
			, string TaxClassification, string Block, string Lot, string ARAccount, string APOthersAccount, string JEType, string DocNum, string Vatable, string PostingDate,
			  string RecieptNo, string TransCode, string Account3, double Amount3, out bool isSuccess, out string Message)
		{
			isSuccess = true;
			Message = "";
			int JournalEntryNo = 0;
			//JOURNAL ENTRY WHEN PROJECT HAS NO LTS NO.
			DataTable dtLTSNo = hana.GetData($@"SELECT ""LTSNo"" FROM OQUT WHERE ""DocEntry"" = {ViewState["SQDocEntry"]}", hana.GetConnection("SAOHana"));
			if (dtLTSNo.Rows.Count > 0)
			{
				string LTSNo = DataAccess.GetData(dtLTSNo, 0, "LTSNo", "").ToString();

				if (string.IsNullOrWhiteSpace(LTSNo))
				{
					if (isSuccess = cashRegister.CreateJournalEntry(null, projectCode, SapCardCode, payment, SODocEntry, FinancingScheme,
													TaxClassification, Block, Lot, ARAccount, APOthersAccount, Account3, JEType, DocNum, "", "", Vatable,
														PostingDate, TransCode, out JournalEntryNo, out Message, RecieptNo, 0, null, Amount3))
					{

					}
				}
			}
		}


		bool checkLTS()
		{
			//JOURNAL ENTRY WHEN PROJECT HAS NO LTS NO.
			//DataTable dtLTSNo = hana.GetData($@"SELECT ""U_LTSno"" FROM ""OPRJ"" WHERE ""PrjCode"" = '{PrjCode}'", hana.GetConnection("SAPHana"));
			DataTable dtLTSNo = hana.GetData($@"SELECT ""LTSNo"" FROM OQUT WHERE ""DocEntry"" = {ViewState["SQDocEntry"]} ", hana.GetConnection("SAOHana"));
			if (dtLTSNo.Rows.Count > 0)
			{
				string LTSNo = DataAccess.GetData(dtLTSNo, 0, "LTSNo", "").ToString();

				if (!string.IsNullOrWhiteSpace(LTSNo))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}


		protected void btnYes_Click(object sender, EventArgs e)
		{
			try
			{
				tORDate.Text = tDocDate.Value;
				txtPRDate.Text = tDocDate.Value;
				txtARDate.Text = tDocDate.Value;


				ViewState["ORNo"] = txtOR.Text;
				ViewState["ARNo"] = txtAR.Text;
				ViewState["PRNo"] = txtPR.Text;


				foreach (GridViewRow row1 in gvPayments.Rows)
				{
					dtPayments.Rows.Add(
						row1.Cells[0].Text,
						row1.Cells[1].Text,
						Convert.ToDouble(row1.Cells[2].Text)
						);
				}

				var test124124141 = dtPayments;

				//cancel trans
				if ((string)ViewState["ConfirmType"] == "removebuyer")
				{
					//close modal
					closeconfirm();
					pnlBuyerInfo.Visible = false;
					ScriptManager.RegisterStartupScript(this, this.GetType(), "show", "showBuyer();", true);
				}
				else if ((string)ViewState["ConfirmType"] == "removepayment")
				{
					//** delete selected payments
					((DataTable)ViewState["dtPayments"]).Rows.RemoveAt(int.Parse((ViewState["paymentindex"]).ToString()));
					LoadData(gvPayments, "dtPayments");
					NewComputeTotal(gvPayments);

					//** close confirm
					closeconfirm();
				}
				else if ((string)ViewState["ConfirmType"] == "ApplyToPrincipal")
				{
					loadApplyToPrincipal(1);
					closeconfirm();
				}
				else if ((string)ViewState["ConfirmType"] == "finish")
				{
					//2024-08-17: ADD BLOCKING WHEN NO SCHEDULE SELECTED FOR PAYMENT
					int ctrSelectedSched = 0;
					foreach (GridViewRow row in gvDownPayments.Rows)
					{
						//DISABLE CHECKBOX WHEN PAID
						string LineStatus = row.Cells[16].Text;
						CheckBox chkSel = (CheckBox)gvDownPayments.Rows[row.RowIndex].FindControl("chkSel");

						if (chkSel.Checked)
							if (LineStatus == "O")
							{
								if (row.Cells[1].Enabled && chkSel.Checked)
								{
									ctrSelectedSched++;
								}
							}
					}

					if (ctrSelectedSched > 0)
					{



						//2023-08-02 : CHECK IF TCP OR MISC PAYMENT
						//CHECK IF PAYMENT IS TCP
						string paymentTag = "MISC";
						if (Convert.ToInt16(ViewState["chkTagDP"]) > 0)
						{
							paymentTag = "TCP";
						}


						//2023-07-11 : BLOCK PAYMENT WHEN PAYMENT DATE IS GREATER THAN EXISTING PAYMENT DATE
						DataTable dtIncomingPaymentsBlock1 = new DataTable();

						//2023-08-02 : ADDED CONDITION -- GET INCOMING PAYMENTS BASED ON THEIR PAYMENT TYPE (TCP OR MISC)
						//string qryIncomingPaymentsBlock1 = $@"select ""DocEntry"", ""DocDate"" from ORCT where ""U_BlockNo"" = '{ViewState["Block"].ToString()}' and 
						//                                        ""U_LotNo"" = '{ViewState["Lot"].ToString()}' AND ""PrjCode"" = '{ViewState["ProjCode"].ToString()}' AND ""Canceled"" = 'N' 
						//                            AND ""DocDate"" > '{Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd")}'";
						//dtIncomingPaymentsBlock1 = hana.GetData(qryIncomingPaymentsBlock1, hana.GetConnection("SAPHana"));
						string qryIncomingPaymentsBlock1 = $@"select ""DocEntry"", ""DocDate"" from ORCT where ""U_BlockNo"" = '{ViewState["Block"].ToString()}' and 
                                                            ""U_LotNo"" = '{ViewState["Lot"].ToString()}' AND ""PrjCode"" = '{ViewState["ProjCode"].ToString()}' AND ""Canceled"" = 'N' 
                                                AND ""DocDate"" > '{Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd")}' AND  IFNULL(""U_PaymentType"",'') = '{paymentTag}'  AND
	                                                    IFNULL(""U_Restructured"",'N') = 'N'"; //2024-07-09: ADDED CONDITION
						dtIncomingPaymentsBlock1 = hana.GetData(qryIncomingPaymentsBlock1, hana.GetConnection("SAPHana"));

						//2023-07-24 : EXCLUDE BLOCKING WHEN MISCELLANEOUS POSTING
						int counter1 = 0;

						//2023-09-19 : REMOVE CONDITION SINCE MISCELLANEOUS IS ALSO AFFECTED ON INCOMING PAYMENT DOCDATE
						//CHECK IF PAYMENT IS DP
						//if (Convert.ToInt16(ViewState["chkTagDP"]) > 0)
						//{
						if (dtIncomingPaymentsBlock1.Rows.Count > 0)
						{
							counter1++;
						}
						// }

						//2023-07-24 : CHANGED CONDITION WITH CONSIDERATION IF MISCELLANEOUS OR NOT
						//if (dtIncomingPaymentsBlock1.Rows.Count == 0)
						if (counter1 == 0)
						{
							//if (int.Parse(ViewState["MaxLBTerm"].ToString()) > 0)
							//{
							//GET DATA FROM ADDON QUOTATION
							DataTable generalData = new DataTable();
							generalData = ws.GetGeneralData(Convert.ToInt32(ViewState["SQDocEntry"]), ConfigurationManager.AppSettings["HANADatabase"].ToString()).Tables[0];
							if (generalData.Rows.Count == 0)
							{
								return;
							}
							DataTable itemDetails = new DataTable();
							itemDetails = ws.GetItemDetails(ViewState["ProjCode"].ToString(),
															 ViewState["Block"].ToString(),
															ViewState["Lot"].ToString(),
															ConfigurationManager.AppSettings["HANADatabase"]).Tables[0];
							string houseItem = DataHelper.DataTableRet(itemDetails, 0, "House", "");

							string ProdType = DataHelper.DataTableRet(generalData, 0, "ProductType", "");

							if (string.IsNullOrEmpty(houseItem) && ProdType.ToUpper() != "LOT ONLY")
							{
								alertMsg("No BOM setup for this House and Lot. Please contact administrator.", "info");
							}
							else
							{

								//POSTING PERIOD CHECKING
								string qryPostingPeriodCheck = $@"SELECT 
	                                                        *
                                                        FROM
	                                                        OFPR
                                                        WHERE
	                                                        '{Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd")}' >= ""F_RefDate"" AND
                                                            '{Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd")}' <= ""T_RefDate""";
								DataTable dtPostingPeriodCheck = hana.GetData(qryPostingPeriodCheck, hana.GetConnection("SAPHana"));

								//BLOCK IF POSTING DATE DOESN'T EXIST IN POSTING PERIODS
								if (DataAccess.Exist(dtPostingPeriodCheck))
								{

									//2023-10-03 : BLOCK WHEN CHECK DUE DATE DEVIATES INSIDE PERMISSIBLE RANGE OF POSTING PERIOD SETUP
									string qryCheckDueDateChecking = $@"SELECT A.""DocDate"", B.""DueDate"" FROM ORCT A INNER JOIN 
                                                                    RCT1 B ON A.""DocEntry"" = B.""DocNum"" INNER JOIN OFPR C 
                                                                    ON '{Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd")}' >= C.""F_RefDate"" AND 
                                                                    '{Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd")}' <= C.""T_RefDate""
                                                                    WHERE A.""U_Project"" = '{ViewState["ProjCode"].ToString()}' AND A.""U_BlockNo"" = '{ViewState["Block"].ToString()}' AND
                                                                    A.""U_LotNo"" = '{ViewState["Lot"].ToString()}' AND ('{Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd")}' < C.""F_DueDate"" OR '{Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd")}' > C.""T_DueDate"") AND
	                                                                IFNULL(A.""U_Restructured"",'N') = 'N'
                                                                    ";
									DataTable dtCheckDueDateChecking = hana.GetData(qryCheckDueDateChecking, hana.GetConnection("SAPHana"));
									if (dtCheckDueDateChecking.Rows.Count <= 0)
									{


										//2023-10-03 : BLOCK WHEN BANK TRANSFER DATE DEVIATES INSIDE PERMISSIBLE RANGE OF POSTING PERIOD SETUP
										string qryBankTransferDateChecking = $@"SELECT A.""DocDate"", A.""TrsfrDate"" FROM ORCT A INNER JOIN OFPR B
                                                                        ON '{Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd")}' >= B.""F_RefDate"" AND 
                                                                        '{Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd")}' <= B.""T_RefDate""
                                                                        WHERE A.""U_Project"" = '{ViewState["ProjCode"].ToString()}' AND A.""U_BlockNo"" = '{ViewState["Block"].ToString()}' AND
                                                                        A.""U_LotNo"" = '{ViewState["Lot"].ToString()}' AND (A.""TrsfrDate"" < B.""F_DueDate"" OR A.""TrsfrDate"" > B.""T_DueDate"" AND
	                                                                    IFNULL(A.""U_Restructured"",'N') = 'N')
                                                                    ";
										DataTable dtBankTransferDateChecking = hana.GetData(qryBankTransferDateChecking, hana.GetConnection("SAPHana"));
										if (dtBankTransferDateChecking.Rows.Count <= 0)
										{


											string receiptNo = string.IsNullOrWhiteSpace(txtOR.Text) ? (string.IsNullOrWhiteSpace(txtAR.Text) ? txtPR.Text : txtAR.Text) : txtOR.Text;
											//string qryExistingOR = $@"select COUNT(""DocEntry"") ""DocEntry"" From ORCT A where ""U_Project"" = '{ViewState["ProjCode"].ToString()}' and 
											//                        (A.""U_ORNo"" = '{receiptNo}' OR A.""U_ARNo"" = '{receiptNo}' OR A.""U_PRNo"" = '{receiptNo}')	";
											//DataTable dtExistingOR = hana.GetData(qryExistingOR, hana.GetConnection("SAPHana"));
											DataTable dtExistingOR = ws.CheckORPerLocation(ViewState["ProjCode"].ToString(), receiptNo).Tables[0];

											int countOR = Convert.ToInt32(DataHelper.DataTableRet(dtExistingOR, 0, "DocEntry", "0"));

											if (!string.IsNullOrWhiteSpace(receiptNo))
											{

												if (countOR == 0)
												{

													closeconfirm();
													//if (ViewState["BuyerStatus"].ToString() == "Y")
													//{
													string errorMsg = "";
													DataTable dtLTSNo = hana.GetData($@"SELECT ""LTSNo"" FROM OQUT WHERE ""DocEntry"" = {ViewState["SQDocEntry"]}", hana.GetConnection("SAOHana"));
													string LTSNo = DataHelper.DataTableRet(dtLTSNo, 0, "LTSNo", "");
													int tag = 0;
													string LOI = string.IsNullOrWhiteSpace(LTSNo) ? "Yes" : "No";

													//Check if LTS No exists
													if (!string.IsNullOrWhiteSpace(LTSNo))
													{
														//2023-09-28 : CONDITION TO REMOVE BLOCKING IF OR/AR FOR MISC PAYMENT WHEN LOI TO LTS WHEN THE CHECKBOX IS CHECKED
														if (!chkRemoveBlocking.Checked)
														{
															if (Convert.ToInt16(ViewState["chkTagMISC"]) > 0)
															{
																if (string.IsNullOrWhiteSpace(txtAR.Text))
																{
																	txtAR.Focus();
																	tag += 1;
																	errorMsg = "Please provide AR No.";
																}
															}
															else if (Convert.ToInt16(ViewState["chkTagDP"]) > 0)
															{
																if (string.IsNullOrWhiteSpace(txtOR.Text))
																{
																	txtOR.Focus();
																	tag += 1;
																	errorMsg = "Please provide OR No.";
																}
															}
														}
													}
													//if LTS No does NOT exists
													else
													{
														if (string.IsNullOrEmpty(txtPR.Text))
														{
															txtPR.Focus();
															tag += 1;
															errorMsg = "Please provide PR Number.";
														}
													}

													#region old requirement for LTSNo
													//if (!string.IsNullOrWhiteSpace(LTSNo) && Convert.ToInt16(ViewState["chkTag-MISC"]) == 0)
													//{
													//    if (string.IsNullOrEmpty(txtOR.Text) && string.IsNullOrWhiteSpace(txtAR.Text))
													//    {
													//        tORDate.Focus();
													//        tag += 1;
													//        errorMsg = "Please provide OR/AR No.";
													//    }
													//    else
													//    {
													//        if (!string.IsNullOrWhiteSpace(txtOR.Text) && string.IsNullOrWhiteSpace(tORDate.Text))
													//        {
													//            tORDate.Focus();
													//            tag += 1;
													//            errorMsg = "Please provide OR Date No.";
													//        }
													//        if (!string.IsNullOrWhiteSpace(txtAR.Text) && string.IsNullOrWhiteSpace(txtARDate.Text))
													//        {
													//            txtARDate.Focus();
													//            tag += 1;
													//            errorMsg = "Please provide AR Date No.";
													//        }
													//    }
													//}
													//else
													//{
													//    if (Convert.ToInt16(ViewState["chkTagMISC"]) == 0)
													//    {

													//        //if (!string.IsNullOrEmpty(txtAR.Text) || !string.IsNullOrEmpty(txtPR.Text))
													//        if (!string.IsNullOrEmpty(txtPR.Text))
													//        {
													//            if (!string.IsNullOrEmpty(txtPRDate.Text))
													//            {
													//            }
													//            else
													//            {
													//                txtPRDate.Focus();
													//                tag += 1;
													//                errorMsg = "Please provide PR Date.";
													//            }

													//        }
													//        else
													//        {
													//            txtPR.Focus();
													//            tag += 1;
													//            errorMsg = "Please provide PR Number.";
													//        }
													//    }
													//    else
													//    {
													//        //if (!string.IsNullOrEmpty(txtAR.Text) || !string.IsNullOrEmpty(txtPR.Text))
													//        if (!string.IsNullOrEmpty(txtAR.Text))
													//        {
													//            if (!string.IsNullOrEmpty(txtARDate.Text))
													//            {
													//            }
													//            else
													//            {
													//                txtARDate.Focus();
													//                tag += 1;
													//                errorMsg = "Please provide AR Date.";
													//            }

													//        }
													//        else
													//        {
													//            txtAR.Focus();
													//            tag += 1;
													//            errorMsg = "Please provide AR Number.";
													//        }
													//    }
													//}
													#endregion



													if (tag > 0)
													{
														ScriptManager.RegisterStartupScript(this, this.GetType(), "confirmation", "hideConfirm();", true);
														alertMsg(errorMsg, "info");
													}
													else
													{


														double blockingTotalToBePayment = 0;
														double blockingTotalCurrentPayment = 0;

														//double getAllPaidPrincipal = 0;
														foreach (GridViewRow row0 in gvDownPayments.Rows)
														{
															blockingTotalToBePayment += Convert.ToDouble(row0.Cells[13].Text);
															//if (row0.Cells[16].Text == "C")
															//{ 
															//    getAllPaidPrincipal += Convert.ToDouble(row0.Cells[6].Text);
															//}
														}
														foreach (GridViewRow row1 in gvPayments.Rows)
														{
															blockingTotalCurrentPayment += Convert.ToDouble(row1.Cells[2].Text);
														}



														//Block adding of payment when total Payment (header) is GREATER THAN total amount for payment (rows)
														if (((blockingTotalCurrentPayment + Convert.ToDouble(lblTotalPayment.Text))) <= Convert.ToDouble(lblNETTCP.Text) &&
															Math.Round(blockingTotalCurrentPayment, 2) > Math.Round(blockingTotalToBePayment, 2)

															)
														{
															ScriptManager.RegisterStartupScript(this, this.GetType(), "confirmation", "hideConfirm();", true);
															alertMsg("Total payment supplied is greater than to be paid amount. Please try again.", "info");
														}
														//(((blockingTotalCurrentPayment + Convert.ToDouble(lblTotalPayment.Text)) Convert.ToDouble(lblNETTCP.Text)) * 100) > 100)
														else
														{

															//check if payment exist
															if (gvPayments.Rows.Count == 0)
															{
																ScriptManager.RegisterStartupScript(this, this.GetType(), "confirmation", "hideConfirm();", true);
																alertMsg("Please add payments", "info");
															}
															else
															{
																CashRegisterService cashRegister = new CashRegisterService();

																//STRING FOR ENTRIES FOR INCOMING PAYMENT
																string DPEntries = "";
																string InvoiceEntries = "";




																bool isSuccess = false;
																string SapCardCode = "";
																int DocEntry = 0;
																string Message = "";
																int JournalEntryNo;

																//for standalone invoice
																string paymentType = "";
																int paymentTerm = 0;

																string CreditableWithholdingTaxAccount = ConfigSettings.CreditableWithholdingTaxAccount;
																string ARAccount = ConfigSettings.ARClearingAccount;
																string APOthersAccount = ConfigSettings.APOthersAccount;
																string ARClearingAccount = ConfigSettings.ARClearingAccount;
																string DepositFromCustomers = ConfigSettings.ClearingAccount;
																string RetitlingPayableLotAccount = ConfigSettings.RetitlingPayableLotAccount;
																string IPSControlAccount = ConfigSettings.IPSControlAccount;
																string ExcessControlAccount = ConfigSettings.ExcessControlAccount;
																string ContractReceivablesDeferredAccount = ConfigSettings.ContractReceivablesDeferredAccount;


																//2024-09-23 : ADDED NEW ACCOUNTS FOR POSTING 
																string OutputTaxSurcharge = ConfigSettings.OutputTaxSurcharge;
																string OutputTaxInterest = ConfigSettings.OutputTaxInterest;
																string InterestVATAccount = ConfigSettings.InterestVATAccount;
																string SuchargeVATAccount = ConfigSettings.SuchargeVATAccount;

																DataTable dtSODocEntrty = null;
																int SODocEntry = 0;
																int SOEntry = 0;



																string Vatable = DataHelper.DataTableRet(generalData, 0, "Vatable", "");
																double NetTCP = Convert.ToDouble(DataHelper.DataTableRet(generalData, 0, "NetTCP", ""));
																string TaxClassification = DataHelper.DataTableRet(generalData, 0, "TaxClassification", "");
																double LoanableBalance = Convert.ToDouble(DataHelper.DataTableRet(generalData, 0, "TCPLoanableBalance", ""));

																int DPTerms = Convert.ToInt32(DataHelper.DataTableRet(generalData, 0, "DPTerms", "0"));
																int MiscTerms = Convert.ToInt32(DataHelper.DataTableRet(generalData, 0, "MiscDPTerms", "0")) + Convert.ToInt32(DataHelper.DataTableRet(generalData, 0, "MiscLBTerms", "0"));


																//GET DATA FROM SESSIONS
																string ProjectCode = ViewState["ProjCode"].ToString();
																string Block = ViewState["Block"].ToString();
																string Lot = ViewState["Lot"].ToString();
																string CardCode = ViewState["AddonCardCode"].ToString();
																string FinancingScheme = ViewState["FinancingScheme"].ToString();
																string HouseModel = ViewState["Model"].ToString();
																double ReservationFree = double.Parse(ViewState["ReservationFee"].ToString());
																double DPAmount = double.Parse(ViewState["DPAmount"].ToString());
																double LBAmount = double.Parse(ViewState["LAmount"].ToString());

																//GET PAYMENT SCHEME
																//DataTable dtPaymentScheme = hana.GetData($@"SELECT ""U_PmtSchemeType"" FROM ""@FINANCINGSCHEME"" WHERE ""Code"" = '{FinancingScheme}'", hana.GetConnection("SAPHana"));
																//string PaymentScheme = DataAccess.GetData(dtPaymentScheme, 0, "U_PmtSchemeType", "0").ToString();
																//string PaymentScheme = DataHelper.DataTableRet(generalData, 0, "PaymentScheme", "");
																string PaymentScheme = "Installment";



																string qryForUpdates = "";

																int SapEntry = 0;
																double CashDiscount = 0;


																int checkTagging = 0;

																int TagFirstDP = 0;
																int Tag25TCP27 = 0;

																double ExclusionAmountForBooking = 0;






																string qryLocation = $@"SELECT B.""Name"" FROM OPRJ A INNER JOIN ""@LOCATION"" B ON A.""U_Location"" = B.""Code"" WHERE A.""PrjCode"" = '{ProjectCode}'";
																DataTable dtGetLocation = hana.GetData(qryLocation, hana.GetConnection("SAPHana"));
																string Location = DataAccess.GetData(dtGetLocation, 0, "Name", "").ToString();





																//INITIALIZATION OF TABLE FOR INTEREST PAYMENT
																DataTable dtInterestPayments = new DataTable();
																dtInterestPayments.Columns.AddRange(new DataColumn[2]
																{
																new DataColumn("DocEntry"),
																new DataColumn("InterestAmount")
																});







																foreach (GridViewRow _row in gvDownPayments.Rows)
																{


																	CheckBox chkSel = (CheckBox)gvDownPayments.Rows[_row.RowIndex].FindControl("chkSel");

																	//2024-11-04: JMC: ADDED CONDITION -- OPEN ROWS ONLY
																	//if (chkSel.Checked  )
																	if (chkSel.Checked && _row.Cells[16].Text == "O")
																	{
																		checkTagging++;
																		string paymentdue_type = _row.Cells[3].Text;
																		string LineStatus = _row.Cells[16].Text;

																		string amountpaid = _row.Cells[12].Text == "" || _row.Cells[12].Text == null || _row.Cells[12].Text == "&nbsp;" ? "0" : _row.Cells[12].Text;
																		string payment = _row.Cells[13].Text;
																		double balance = Convert.ToDouble(_row.Cells[14].Text);
																		CashDiscount += Convert.ToDouble(_row.Cells[10].Text);
																		DocEntry = int.Parse(_row.Cells[0].Text);


																		//GET INTEREST, SURCHARGE, AND IPS AMOUNT FOR EXCLUSION ON POSTING OF AR RESERVE INVOICE (BOOKING 25%)
																		if (_row.Cells[16].Text == "O")
																		{
																			ExclusionAmountForBooking += Convert.ToDouble(_row.Cells[7].Text) + Convert.ToDouble(_row.Cells[8].Text) + Convert.ToDouble(_row.Cells[9].Text);
																		}


																		//if (double.Parse(payment) == 0)
																		//{
																		//    break;
																		//}

																		// CURRENT PAYMENT == 0 AND (principal + interest + penalty + ips != amount Paid)

																		if (double.Parse(payment) == 0 &&
																			(double.Parse((double.Parse(_row.Cells[6].Text) + double.Parse(_row.Cells[7].Text) +
																			double.Parse(_row.Cells[8].Text) + double.Parse(_row.Cells[9].Text)).ToString("0.00")) != double.Parse(amountpaid)
																			) &&
																			LineStatus == "O"
																			)
																		{

																			double test123 = (double.Parse(_row.Cells[6].Text) + double.Parse(_row.Cells[7].Text) + double.Parse(_row.Cells[8].Text) + double.Parse(_row.Cells[9].Text));
																			double test456 = double.Parse(amountpaid);
																			break;
																		}
																		paymentType = _row.Cells[3].Text;

																		//check if fullpayment per doc | if yes the post to sap | else just update ornumber
																		int Terms = int.Parse(_row.Cells[2].Text);
																		paymentTerm = int.Parse(_row.Cells[2].Text);


																		if (paymentTerm == 7 && paymentdue_type == "LB")
																		{
																			string test1 = "";
																		}


																		int DPEntry = 0;
																		double SurCharges = double.Parse(_row.Cells[8].Text);
																		double Interest = double.Parse(_row.Cells[7].Text);
																		double IPS = double.Parse(_row.Cells[9].Text);


																		int SQEntry = 0;


																		DataTable dtTotalPaid0 = ws.GetTotalPaymentsForTheYear(Convert.ToInt32(ViewState["SQDocEntry"])).Tables[0];


																		//double TotalAmoundPaid0 = Convert.ToDouble(DataHelper.DataTableRet(dtTotalPaid0, 0, "TotalAmountPaid", "0")) + Convert.ToDouble(lblAmountDue.Text);

																		//2023-07-06 : ADD TCP CURRENT PAYMENT ONLY INSTEAD OF TOTAL OF ALL CURRENT PAYMENT
																		//double TotalAmoundPaid0 = Convert.ToDouble(lblTotalPayment.Text) + Convert.ToDouble(lblAmountDue.Text);
																		var test12421312321312 = ViewState["TCPTotalPayment"];
																		double TotalAmoundPaid0 = Convert.ToDouble(lblTotalPayment.Text) + Convert.ToDouble(ViewState["TCPTotalPayment"].ToString());


																		double psNetTCP = Convert.ToDouble(DataHelper.DataTableRet(generalData, 0, "NetTCP", "0"));



																		DateTime psDocDate = Convert.ToDateTime(DataHelper.DataTableRet(generalData, 0, "DocDate", "1999-01-01"));
																		DateTime psLTSIssueDate = Convert.ToDateTime(DataHelper.DataTableRet(generalData, 0, "LTSIssueDate", psDocDate.ToString()));

																		//2023-07-04 : GET LTS FOR CHECKING
																		string LTSNoCheck = DataHelper.DataTableRet(generalData, 0, "LTSNo", "");
																		string OriginalLTSNoCheck = DataHelper.DataTableRet(generalData, 0, "OriginalLTSNo", "");



																		//2023-07-04: CONDITION IS FOR LOI ONLY
																		//IF WITH LTSNO FROM THE START
																		if (!string.IsNullOrWhiteSpace(LTSNoCheck) && !string.IsNullOrWhiteSpace(OriginalLTSNoCheck))
																		{
																			psLTSIssueDate = psDocDate;
																		}
																		//IF LOI TO LTS
																		else if (!string.IsNullOrWhiteSpace(LTSNoCheck) && string.IsNullOrWhiteSpace(OriginalLTSNoCheck))
																		{
																			psLTSIssueDate = Convert.ToDateTime(DataHelper.DataTableRet(generalData, 0, "LTSIssueDate", psDocDate.ToString()));
																		}
																		else
																		{
																			psLTSIssueDate = psDocDate;
																		}

																		var ARReserveInvoiceCondition0 = ConfigSettings.ARReserveInvoiceCondition;



																		//DETERMINE PAYMENT SCHEME;
																		//Condition is: If (TotalPayment / NetTCP) is greater than or equal 25%
																		// and Date is lessthan or equal End of year, get Engaged in Business
																		// else get Not engaged in business
																		if (
																			//((TotalAmoundPaid0 / psNetTCP) >= (Convert.ToDouble(ARReserveInvoiceCondition0))) &&
																			((TotalAmoundPaid0 / double.Parse(lblNETTCP.Text)) >= (Convert.ToDouble(ARReserveInvoiceCondition0))) &&
																			(Convert.ToDateTime(tDocDate.Value) <= (Convert.ToDateTime(psLTSIssueDate.Year + ConfigSettings.TaxEndOfYear_Month + ConfigSettings.TaxEndOfYear_Day)))
																			)
																		{
																			PaymentScheme = ConfigSettings.PaymentScheme1; //DEFERRED
																		}
																		else
																		{
																			PaymentScheme = ConfigSettings.PaymentScheme2; //INSTALLMENT
																		}


																		//2023-08-04 : CHECK IF EXISTING 
																		// Check if BP is exist in SAP
																		DataTable dtBPForInvoicePosting = new DataTable();
																		dtBPForInvoicePosting = hana.GetData($@"SELECT TOP 1 ""DocEntry"",""CardCode"",""CardName"",""CardFName""  FROM ""OCRD"" WHERE ""U_DreamsCustCode"" = '{lblID.Text}'", hana.GetConnection("SAPHana"));

																		if (DataAccess.Exist(dtBPForInvoicePosting))
																		{
																			SapCardCode = DataAccess.GetData(dtBPForInvoicePosting, 0, "CardCode", "").ToString();
																		}

																		DataTable dtCheckARResInvoice = new DataTable();
																		dtCheckARResInvoice = ws.IsReserveInvoiceExists(ProjectCode,
																										Block,
																										Lot,
																										SapCardCode).Tables[0];
																		if (DataHelper.DataTableExist(dtCheckARResInvoice))
																		{
																			PaymentScheme = DataHelper.DataTableRet(dtCheckARResInvoice, 0, "U_PaymentType", "0");
																		}
																		//SapCardCode = "";


																		//GET QUT1 ID
																		DataTable dtGetQUT1ID = new DataTable();
																		dtGetQUT1ID = hana.GetData($@" select ""Id"" from qut1 where ""DocEntry"" = {DocEntry} AND ""Terms"" = {Terms} AND 
                                                                        ""PaymentType"" = '{paymentdue_type}' AND ""LineStatus"" <> 'R'", hana.GetConnection("SAOHana"));
																		string QUT1ID = DataHelper.DataTableRet(dtGetQUT1ID, 0, "Id", "0");
























																		//######################
																		//RESERVATION PAYMENT   
																		//###################### 
																		if (paymentdue_type == "RES")
																		{
																			if (isSuccess = cashRegister.CreateReservation(DocEntry,
																												Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																												txtOR.Text,
																												balance,
																												double.Parse(payment) + double.Parse(amountpaid),
																												txtDocNum.Text,
																												"QUOTATION",
																												DPTerms,
																												MiscTerms,
																												out SapEntry,
																												out DPEntry,
																												out SapCardCode,
																												out Message))
																			{
																				//Do not update when due date is already paid 
																				if (_row.BackColor != System.Drawing.ColorTranslator.FromHtml("#28B463"))
																				{

																					double qut13Amount = GetQUT13Amount(double.Parse(payment), double.Parse(_row.Cells[6].Text), double.Parse(_row.Cells[7].Text), double.Parse(_row.Cells[8].Text), double.Parse(_row.Cells[9].Text), double.Parse(_row.Cells[12].Text), 0, 0);




																					//update forwarded status and sap entry 
																					qryForUpdates += $@" UPDATE ""OQUT"" SET ""SAPDocEntry"" = '{SapEntry}' WHERE ""DocEntry"" = '{DocEntry}'; ";
																					qryForUpdates += $@" UPDATE ""QUT1"" SET ""SapDocEntry"" = '{DPEntry}', ""AmountPaid"" = '{double.Parse(payment) + double.Parse(amountpaid)}'{(double.Parse(_row.Cells[14].Text) == 0 ? @",""LineStatus"" = 'C'" : "")} 
                                                                        WHERE ""DocEntry"" = '{DocEntry}' AND ""Terms"" = '{Terms}' AND 
                                                                        ""PaymentType"" = '{paymentdue_type}' AND ""LineStatus"" <> 'R';";
																					qryForUpdates += $@" INSERT INTO QUT13 VALUES ({DocEntry}, {QUT1ID}, '{receiptNo}', 0, '{paymentdue_type}', {Terms}, {qut13Amount}, 0, 0, '{Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd")}', 
                                                                                   '{Location}', {Session["UserID"].ToString()}, '{DateTime.Now.ToString("yyyyMMdd")}'); ";

																					DataTable dt = ws.IsPaymentForSpecificTransactionExists(DPEntry, 203).Tables[0];
																					if (double.Parse(DataAccess.GetData(dt, 0, "DocTotal", "0").ToString()) >= (double.Parse(_row.Cells[6].Text) + double.Parse(_row.Cells[7].Text) + double.Parse(_row.Cells[8].Text) + double.Parse(_row.Cells[9].Text)))
																					{
																						DPEntry = 0;
																					}


																					DPEntries = $"{DPEntries}{DPEntry};";


																					//UPDATE BATCH IN SAP : U_LOT STATUS
																					//if (isSuccess = cashRegister.UpdateBatchDetails(null, ws.GetGeneralData(DocEntry).Tables[0], out Message))
																					if (isSuccess = cashRegister.UpdateBatchDetails(null, ProjectCode, Block, Lot, "SOLD", out Message))
																					{

																					}
																					else
																					{
																						alertMsg(Message, "warning");
																						break;
																					}

																					JEForNoLTSNumber(cashRegister, ProjectCode, SapCardCode, Convert.ToDouble(payment), SODocEntry, FinancingScheme, TaxClassification,
																						 Block, Lot, DepositFromCustomers, APOthersAccount, "WithoutLTS", txtDocNum.Text, Vatable, Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																						 receiptNo, "NLTS", "", 0, out isSuccess, out Message);
																				}
																			}
																			else
																			{
																				alertMsg(Message, "warning");
																				break;
																			}

																		}





















																		//######################
																		//MISCELLAENOUS PAYMENT   
																		//###################### 
																		else if (paymentdue_type == "MISC")
																		{

																			if (chkSel.Checked && LineStatus == "O")
																			{
																				checkTagging++;



																				if (paymentTerm == 1)
																				{
																					int test = 0;
																				}
																				int AREntry = 0;



																				//string ControlAccount = "";

																				//DataTable dtTotalPaid1 = hana.GetData($@"SELECT SUM(IFNULL(""AmountPaid"",0)) ""TotalAmountPaid"" from QUT1 where ""DocEntry"" = {DocEntry} AND ""PaymentType"" <> 'MISC'", hana.GetConnection("SAOHana"));
																				//double TotalAmoundPaid1 = Convert.ToDouble(DataHelper.DataTableRet(dtTotalPaid1, 0, "TotalAmountPaid", "0"));
																				//double netTCP = Convert.ToDouble(DataHelper.DataTableRet(generalData, 0, "NetTCP", "0"));

																				//var ARReserveInvoiceCondition1 = ConfigSettings.ARReserveInvoiceCondition"].ToString();
																				////CHECK IF TOTAL PAID AMOUNT IS LESS THAN OR EQUAL 25%
																				//if (TotalAmoundPaid1 < (netTCP * Convert.ToDouble(ARReserveInvoiceCondition1)))
																				//{
																				//    ControlAccount = ARAccount;
																				//}
																				//else
																				//{
																				//    ControlAccount = ContractReceivablesDeferredAccount;
																				//}


																				//CHECK IF SAPCARDCODE EXISTS
																				if (string.IsNullOrWhiteSpace(SapCardCode))
																				{
																					// Check if BP is exist in SAP
																					DataTable dt = new DataTable();
																					dt = hana.GetData($@"SELECT TOP 1 ""DocEntry"",""CardCode"",""CardName"",""CardFName""  FROM ""OCRD"" WHERE ""U_DreamsCustCode"" = '{lblID.Text}'", hana.GetConnection("SAPHana"));

																					if (DataAccess.Exist(dt))
																					{
																						SapCardCode = DataAccess.GetData(dt, 0, "CardCode", "").ToString();
																					}
																				}


																				//GET SAP DOCENTRY FROM ADDON DB IF IT ALREADY HAS ONE 
																				if (SapEntry == 0)
																				{
																					SQEntry = Convert.ToInt32(DataHelper.DataTableRet(generalData, 0, "SAPDocEntry", "0"));
																				}
																				else
																				{
																					SQEntry = SapEntry;
																				}


																				//GET SO #
																				DataTable dtSONum = ws.IsSalesOrderExists(ProjectCode,
																												   Block,
																												   Lot,
																												   SapCardCode,
																												   txtDocNum.Text).Tables[0];

																				if (!DataAccess.Exist(dtSONum))
																				{
																					//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
																					//SALES ORDER POSTING
																					//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
																					if (isSuccess = cashRegister.CreateSalesOrder(null,
																																SapCardCode,
																																Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																																ProjectCode,
																																Block,
																																Lot,
																																SurCharges,
																																Interest,
																																SQEntry,
																																"DP",
																																txtDocNum.Text,
																																LOI,
																																Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																																DocEntry,
																																out SOEntry,
																																out Message))
																					{


																					}
																				}
																				else
																				{
																					SOEntry = Convert.ToInt32(DataHelper.DataTableRet(dtSONum, 0, "DocEntry", "0"));
																				}


																				if (isSuccess = cashRegister.CreateARInvoice(null,
																															SapCardCode,
																															Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																															//double.Parse(payment),
																															double.Parse(_row.Cells[5].Text),
																															paymentdue_type,
																															"QryGroup1",
																															Terms.ToString(),
																															ProjectCode,
																															Block,
																															Lot,
																															(SOEntry == 0 ? SODocEntry : SOEntry),
																															"N",
																															Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																															0,
																															out AREntry,
																															out Message))
																				{

																					//Do not update when due date is already paid
																					if (_row.BackColor != System.Drawing.ColorTranslator.FromHtml("#28B463"))
																					{
																						//hana.Execute($@"UPDATE ""QUT1"" SET ""SapDocEntry"" = '{AREntry}', ""AmountPaid"" = '{double.Parse(payment) + double.Parse(amountpaid)}'{(double.Parse(_row.Cells[10].Text) == 0 ? @",""LineStatus"" = 'C'" : "")} WHERE ""DocEntry"" = '{DocEntry}' AND ""Terms"" = '{Terms}' AND ""PaymentType"" = '{paymentdue_type}';", hana.GetConnection("SAOHana"));
																						qryForUpdates += $@" UPDATE ""QUT1"" SET ""SapDocEntry"" = '{AREntry}', ""AmountPaid"" = '{double.Parse(payment) + double.Parse(amountpaid)}'{(Math.Round(double.Parse(_row.Cells[14].Text), 2) == 0.00 ? @",""LineStatus"" = 'C'" : "")} WHERE ""DocEntry"" = '{DocEntry}' AND ""Terms"" = '{Terms}' AND ""PaymentType"" = '{paymentdue_type}'  AND ""LineStatus"" <> 'R';";
																						qryForUpdates += $@" INSERT INTO QUT13 VALUES ({DocEntry}, {QUT1ID}, '{receiptNo}', 0, '{paymentdue_type}', {Terms}, {double.Parse(payment)}, 0, 0, '{Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd")}', 
                                                                                 '{Location}',{Session["UserID"].ToString()}, '{DateTime.Now.ToString("yyyyMMdd")}'); ";
																						DataTable dt = ws.IsPaymentForSpecificTransactionExists(AREntry, 13).Tables[0];
																						if (double.Parse(DataAccess.GetData(dt, 0, "DocTotal", "0").ToString()) >= (double.Parse(_row.Cells[6].Text) + double.Parse(_row.Cells[7].Text) + double.Parse(_row.Cells[8].Text) + double.Parse(_row.Cells[9].Text)))
																						{
																							AREntry = 0;
																						}

																						InvoiceEntries = $"{InvoiceEntries}{AREntry};";


																						//2023-11-10 : REVERSAL OF JOURNAL ENTRY OF CANCELLED NLTS
																						//cancelNLTSReversal(txtDocNum.Text, Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"), "XRML");

																						JEForNoLTSNumber(cashRegister, ProjectCode, SapCardCode, Convert.ToDouble(payment), SODocEntry, FinancingScheme, TaxClassification,
																						//2023-08-15 : CHANGED 1ST ACCOUNT CODE FROM DEPOSITFROMCUSTOMERS TO RetitlingPayableLotAccount
																						//Block, Lot, DepositFromCustomers, APOthersAccount, "WithoutLTSMisc", txtDocNum.Text, Vatable, Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																						Block, Lot, RetitlingPayableLotAccount, APOthersAccount, "WithoutLTSMisc", txtDocNum.Text, Vatable, Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																						//2023-08-15 : UPDATED TRANSCODE FROM NLTS TO MLOI
																						//receiptNo, "NLTS", out isSuccess, out Message);
																						receiptNo, "MLOI", "", 0, out isSuccess, out Message);

																					}
																				}
																			}


																		}


























































































																		//######################
																		//DOWNPAYMENT / LOANABLE AMORT
																		//######################

																		else
																		{

																			if (paymentType == "LB" && paymentTerm == 7)
																			{
																				string test = "";
																			}

																			//CHECK IF SAPCARDCODE EXISTS
																			if (string.IsNullOrWhiteSpace(SapCardCode))
																			{
																				// Check if BP is exist in SAP
																				DataTable dt = new DataTable();
																				dt = hana.GetData($@"SELECT TOP 1 ""DocEntry"",""CardCode"",""CardName"",""CardFName""  FROM ""OCRD"" WHERE ""U_DreamsCustCode"" = '{lblID.Text}'", hana.GetConnection("SAPHana"));

																				if (DataAccess.Exist(dt))
																				{
																					SapCardCode = DataAccess.GetData(dt, 0, "CardCode", "").ToString();
																				}
																			}




																			//CHECK IF PAYMENT IS NOT MISCELLANEOUS
																			if (Convert.ToInt16(ViewState["chkTagMISC"]) == 0)
																			{


																				string sundryPayment = "N";
																				string qrySundry = $@"select top 1 ""Terms"",""PaymentType"" from qut1 
                                                                                            where ""DocEntry"" = {ViewState["SQDocEntry"]} and 
                                                                                            ""PaymentType"" IN ('DP','LB') and ""PaymentAmount"" > 0 order by ""Id"" desc";
																				DataTable dtSundry = hana.GetData(qrySundry, hana.GetConnection("SAOHana"));
																				int sundryLastTerm = Convert.ToInt16(DataHelper.DataTableRet(dtSundry, 0, "Terms", ""));
																				string sundryLastType = DataHelper.DataTableRet(dtSundry, 0, "PaymentType", "");


																				double SurchargeForExcess = 0;
																				double InterestForExcess = 0;

																				//loop to get interest and surcharge
																				foreach (GridViewRow rowExcess in gvDownPayments.Rows)
																				{
																					CheckBox chkFForExcess = (CheckBox)gvDownPayments.Rows[rowExcess.RowIndex].FindControl("chkSel");
																					if (chkFForExcess.Checked)
																					{
																						if (rowExcess.Cells[16].Text == "O")
																						{
																							//2023-09-01 : REMOVED CONDITION
																							//if (rowExcess.Cells[2].Text == sundryLastTerm.ToString())
																							//{
																							SurchargeForExcess += Convert.ToDouble(rowExcess.Cells[8].Text);
																							InterestForExcess += Convert.ToDouble(rowExcess.Cells[7].Text);
																							//}
																						}
																					}
																				}


																				if (paymentTerm == 10 && paymentdue_type == "DP")
																				{
																					var test12938129381 = "";
																				}



																				if (paymentTerm == sundryLastTerm && paymentdue_type == sundryLastType &&
																					Math.Round((((blockingTotalCurrentPayment - (Math.Round(SurchargeForExcess + InterestForExcess, 2))) + Convert.ToDouble(lblTotalPayment.Text))), 2) > Convert.ToDouble(lblNETTCP.Text)
																					)
																				{
																					sundryPayment = "Y";
																					int sundryDocEntry = 0;
																					double sundryAmount = ((blockingTotalCurrentPayment + Convert.ToDouble(lblTotalPayment.Text)) - Convert.ToDouble(lblNETTCP.Text)) - (SurchargeForExcess + InterestForExcess);


																					//CREATE AR INVOICE FOR SUNDRY PAYMENTS
																					if (isSuccess = cashRegister.CreateARInvoiceStandalone(null,
																									  SapCardCode, //2
																									  Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																									  sundryAmount, //4
																									  "LB",
																									  "0", //6
																									  ProjectCode,
																									  Block, //8
																									  Lot,
																									  HouseModel.ToString(), //10
																									  FinancingScheme,
																									  ProdType, //12
																									  ReservationFree,
																									  DPAmount, //14
																									  LBAmount,
																									  "Sundry", //16
																									  txtDocNum.Text,
																									   ConfigSettings.ExcessControlAccount, //18
																									  Vatable,
																									  sundryAmount, //20
																									  "",
																									  "N",
																									  Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																									  out sundryDocEntry,
																									  out Message)) //22
																					{
																						InvoiceEntries = $"{InvoiceEntries}{sundryDocEntry};";
																					}


																				}
















																				SapCardCode = "";
																				if (paymentType == "LB")
																				{
																					ViewState["payment"] = payment;
																					ViewState["amountpaid"] = amountpaid;
																				}

																				try
																				{

																					DataTable dt = new DataTable();

																					//GET CARDCODE FROM SAP
																					dt = hana.GetData($@"SELECT TOP 1 ""CardCode"",""CardName"",""CardFName"" FROM ""OCRD"" WHERE ""U_DreamsCustCode"" = '{CardCode}'", hana.GetConnection("SAPHana"));
																					SapCardCode = DataHelper.DataTableRet(dt, 0, "CardCode", "");



																					//GET SAP DOCENTRY FROM ADDON DB IF IT ALREADY HAS ONE 
																					if (SapEntry == 0)
																					{
																						SQEntry = Convert.ToInt32(DataHelper.DataTableRet(generalData, 0, "SAPDocEntry", "0"));
																					}
																					else
																					{
																						SQEntry = SapEntry;
																					}




																					//CHECK IF QUOTATION DATA EXISTS IN ADDON DATABASE
																					if (!DataHelper.DataTableExist(generalData))
																					{
																						Message = "Error : No Data Found upon generataing GetGeneralData SP";
																						break;
																					}


																				}
																				catch (Exception ex)
																				{
																					Message = $"Error : GetGeneralData - {ex.Message}";
																					break;
																				}



																				int SurchargeAREntry = 0;
																				int InterestAREntry = 0;
																				int IPSEntry = 0;







																				//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
																				//SALES ORDER POSTING
																				//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
																				if (isSuccess = cashRegister.CreateSalesOrder(null,
																															SapCardCode,
																															Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																															ProjectCode,
																															Block,
																															Lot,
																															SurCharges,
																															Interest,
																															SQEntry,
																															paymentdue_type,
																															txtDocNum.Text,
																															LOI,
																															Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																															DocEntry,
																															out SOEntry,
																															out Message))
																				{


																					SODocEntry = SOEntry;

																					DataTable dtTotalPaid1 = hana.GetData($@"SELECT SUM(IFNULL(""AmountPaid"",0)) ""TotalAmountPaid"" from QUT1 where ""DocEntry"" = {DocEntry} AND ""PaymentType"" <> 'MISC' AND IFNULL(""Cancelled"",'N') <> 'Y'", hana.GetConnection("SAOHana"));
																					double TotalAmoundPaid1 = Convert.ToDouble(DataHelper.DataTableRet(dtTotalPaid1, 0, "TotalAmountPaid", "0"));
																					if (Convert.ToInt32(ViewState["chkTagMISC"]) <= 0)
																					{
																						TotalAmoundPaid1 += Convert.ToDouble(lblAmountDue.Text);
																					}
																					double netTCP = Convert.ToDouble(DataHelper.DataTableRet(generalData, 0, "NetTCP", "0"));
																					var ARReserveInvoiceCondition1 = ConfigSettings.ARReserveInvoiceCondition;



																					DataTable dtLatestLTSNo = hana.GetData($@"SELECT ""U_LTSNo"" FROM OBTN 
                                                           where ""U_BlockNo"" = '{Block}' and ""U_LotNo"" = '{Lot}' and ""U_Project"" = '{ProjectCode}'", hana.GetConnection("SAPHana"));
																					string LatestLTSNo = DataHelper.DataTableRet(dtLatestLTSNo, 0, "U_LTSNo", "");


























																					//######################## NEW PROCESS OF POSTING ########################
																					//######################## NEW PROCESS OF POSTING ########################
																					//######################## NEW PROCESS OF POSTING ########################

																					//@@@@@@@@@@@@@@@@@@ Check if AR Reserve Invoice does not Exist
																					DataTable _ARResExisting = ws.IsReserveInvoiceExists(ProjectCode,
																					Block,
																					Lot,
																					SapCardCode).Tables[0];













																					////GET TOTAL PAYMENTS FROM QUT1 EXCEPT MISC
																					//DataTable _dtTotalPaid1_ = hana.GetData($@"SELECT SUM(IFNULL(""AmountPaid"",0)) ""TotalAmountPaid"" from 
																					//                                    QUT1 where ""DocEntry"" = {DocEntry} AND ""PaymentType"" <> 'MISC' AND 
																					//                                    IFNULL(""Cancelled"",'N') <> 'Y'", hana.GetConnection("SAOHana"));
																					//double _TotalAmoundPaid1_ = Convert.ToDouble(DataHelper.DataTableRet(_dtTotalPaid1_, 0, "TotalAmountPaid", "0"));



																					//if (Convert.ToInt32(ViewState["chkTagMISC"]) <= 0)
																					//{
																					//    _TotalAmoundPaid1_ += Convert.ToDouble(lblAmountDue.Text);
																					//}



																					////GET NET TCP FROM GENERALDATA STORED PROC
																					//double _netTCP_ = Convert.ToDouble(DataHelper.DataTableRet(generalData, 0, "NetTCP", "0"));

																					////GET 0.25 FROM CONFIG
																					//var _ARReserveInvoiceCondition1_ = ConfigSettings.ARReserveInvoiceCondition"].ToString();


																					//double _DPAmount = 0;

																					//if ((_TotalAmoundPaid1_ >= (_netTCP_ * Convert.ToDouble(_ARReserveInvoiceCondition1_))))
																					//{
																					//    _DPAmount = double.Parse(_row.Cells[13].Text);
																					//}
																					//else
																					//{
																					//    _DPAmount = double.Parse(_row.Cells[6].Text);
																					//}



																					DataTable _dtTotalPaid1 = hana.GetData($@"SELECT 
	                                                                                                 SUM(IFNULL(""AmountPaid"",0)) ""TotalAmountPaid"" ,
	                                                                                                 SUM(CASE 
		                                                                                                WHEN ((IFNULL(""Penalty"",0) + IFNULL(""InterestAmount"",0) + IFNULL(""IPS"",0)) - IFNULL(""AmountPaid"",0)) < 0
			                                                                                                THEN   (IFNULL(""Penalty"",0) + IFNULL(""InterestAmount"",0) + IFNULL(""IPS"",0))
		                                                                                                ELSE
			                                                                                                CASE 
				                                                                                                WHEN IFNULL(""AmountPaid"",0) = 0
					                                                                                                THEN 0
				                                                                                                ELSE 
					                                                                                                IFNULL(""AmountPaid"",0)
			                                                                                                END
			
	                                                                                                END) ""ExcludedAmount""     
                                                                                                from 
                                                                                                    QUT1 
                                                                                                where 
                                                                                                    ""DocEntry"" = {DocEntry} AND 
                                                                                                    ""PaymentType"" <> 'MISC' AND 
                                                                                                    ""LineStatus"" <> 'R' AND 
                                                                                                    IFNULL(""Cancelled"",'N') <> 'Y'", hana.GetConnection("SAOHana"));



																					double _TotalAmoundPaid1 = Convert.ToDouble(DataHelper.DataTableRet(_dtTotalPaid1, 0, "TotalAmountPaid", "0"));
																					double _ExcludedAmount = Convert.ToDouble(DataHelper.DataTableRet(_dtTotalPaid1, 0, "ExcludedAmount", "0"));


																					if ((double.Parse(_row.Cells[7].Text) +
																						double.Parse(_row.Cells[8].Text) + double.Parse(_row.Cells[9].Text)) -
																						(double.Parse(_row.Cells[12].Text) + double.Parse(_row.Cells[13].Text)) < 0)
																					{
																						if (_row.Cells[15].Text == "O")
																						{
																							_ExcludedAmount += (double.Parse(_row.Cells[7].Text) + double.Parse(_row.Cells[8].Text) + double.Parse(_row.Cells[9].Text));
																						}
																					}
																					else
																					{
																						if ((double.Parse(_row.Cells[12].Text) + double.Parse(_row.Cells[13].Text)) == 0)
																						{

																						}
																						else
																						{
																							_ExcludedAmount += (double.Parse(_row.Cells[12].Text) + double.Parse(_row.Cells[13].Text));
																						}
																					}







																					_TotalAmoundPaid1 += Convert.ToDouble(lblAmountDue.Text);

																					double _TotalAmountPaidLessExcludedAmount = 0;


																					if (_ExcludedAmount >= _TotalAmoundPaid1)
																					{
																						_TotalAmountPaidLessExcludedAmount = 0;
																					}
																					else
																					{
																						_TotalAmountPaidLessExcludedAmount = _TotalAmoundPaid1 - _ExcludedAmount;
																					}

																					double _netTCP = Convert.ToDouble(DataHelper.DataTableRet(generalData, 0, "NetTCP", "0"));
																					var _ARReserveInvoiceCondition1 = ConfigSettings.ARReserveInvoiceCondition;



																					DataTable _dtLatestLTSNo = hana.GetData($@"SELECT ""U_LTSNo"" FROM OBTN 
                                                           where ""U_BlockNo"" = '{Block}' and ""U_LotNo"" = '{Lot}' and ""U_Project"" = '{ProjectCode}'", hana.GetConnection("SAPHana"));
																					string _LatestLTSNo = DataHelper.DataTableRet(_dtLatestLTSNo, 0, "U_LTSNo", "");

																					double _DPAmount = 0;





																					////CHECK IF TOTAL PAID AMOUNT IS GREATER THAN OR EQUAL 25%
																					//if ((_TotalAmountPaidLessExcludedAmount >= (_netTCP * Convert.ToDouble(_ARReserveInvoiceCondition1))) && !string.IsNullOrWhiteSpace(_LatestLTSNo))
																					//{
																					//    _DPAmount = double.Parse(_row.Cells[13].Text);
																					//}
																					//else
																					//{
																					_DPAmount = double.Parse(_row.Cells[6].Text);
																					//}





																					string partialPaymentTag = "";
																					if ((double.Parse(payment) + double.Parse(amountpaid)) < (double.Parse(_row.Cells[6].Text) + SurCharges + Interest + IPS))
																					{
																						partialPaymentTag = "Y";
																					}


																					//@@@@@@@@@@@@@@@  Check if AR Reserve Invoice does not Exist
																					if (!DataHelper.DataTableExist(_ARResExisting))
																					{
																						if ((double.Parse(payment) + double.Parse(amountpaid)) > (SurCharges + Interest + IPS))
																						{

																							qryForUpdates += $@" UPDATE ""QUT1"" SET ""SapDocEntry"" = '{DPEntry}', 
                                                                                    ""AmountPaid"" = '{double.Parse(payment) + double.Parse(amountpaid)}'{(double.Parse(_row.Cells[14].Text) == 0 ? @",""LineStatus"" = 'C'" : "")} 
                                                                                    WHERE ""DocEntry"" = '{DocEntry}' AND ""Terms"" = '{Terms}' AND 
                                                                                    ""PaymentType"" = '{paymentdue_type}'  AND ""LineStatus"" <> 'R'; ";



																							//CHECK IF THIS WILL BE THE FIRST PAYMENT AFTER RESTRUCTURING AND IS A PARTIAL PAYMENT
																							string qryFromRestructuring = $@"SELECT ""DocEntry"" FROM ODPI WHERE ""U_PaymentOrder"" = '0' AND ""U_BlockNo"" = '{Block}' AND ""U_LotNo"" = '{Lot}'
                                                                                 AND ""Project"" = '{ProjectCode}'  AND IFNULL(""U_ContractStatus"",'Open') IN ('Open', 'Closed')";
																							DataTable dtFromRestructuring = hana.GetData(qryFromRestructuring, hana.GetConnection("SAPHana"));

																							if (dtFromRestructuring.Rows.Count > 0)
																							{
																								if (LineStatus == "O" && double.Parse(amountpaid) > 0)
																								{
																									_DPAmount = double.Parse(payment);
																								}
																							}


																							//POST AR DOWNPAYMENT INVOICE TO SAP
																							if (isSuccess = cashRegister.CreateDownPayment(null,
																								SapCardCode,
																								Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																								//totalPayment,
																								//double.Parse(_row.Cells[5].Text),
																								_DPAmount,
																								paymentdue_type,
																								Terms.ToString(),
																								ProjectCode,
																								Block,
																								Lot,
																								SOEntry,
																								// payment amount - InterestAmount + penalty + IPS
																								double.Parse(_row.Cells[5].Text) - (double.Parse(_row.Cells[7].Text) + double.Parse(_row.Cells[8].Text) + double.Parse(_row.Cells[9].Text)), //TOTAL NG BABAYARAN
																								double.Parse(payment) + double.Parse(amountpaid),
																								"",
																								DocEntry,
																								Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																								out DPEntry,
																								out Message))
																							{
																								ViewState["Terms"] = Terms.ToString();
																								ViewState["PaymentType"] = paymentdue_type;

																								//@@@@@@@@@@@@@@@  DO NOT UPDATE WHEN ALREADY PAID
																								if (_row.BackColor != System.Drawing.ColorTranslator.FromHtml("#28B463"))
																								{
																									//CHECK IF THIS WILL BE THE FIRST PAYMENT AFTER RESTRUCTURING AND IS A PARTIAL PAYMENT
																									string qryGetDPDocNum = $@"SELECT ""DocNum"" FROM ODPI WHERE ""DocEntry"" = '{DPEntry}'";
																									DataTable dtGetDPDocNum = hana.GetData(qryGetDPDocNum, hana.GetConnection("SAPHana"));
																									string DPDocNum = "";
																									if (dtGetDPDocNum.Rows.Count > 0)
																									{
																										DPDocNum = DataHelper.DataTableRet(dtGetDPDocNum, 0, "DocNum", "0");
																									}

																									//2023-10-10 : REVERSAL OF JOURNAL ENTRY OF CANCELLED DFCs
																									cancelDFCReversal(DPDocNum, Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"));

																									//2023-11-10 : REVERSAL OF JOURNAL ENTRY OF CANCELLED NLTS
																									//cancelNLTSReversal(DPDocNum, Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"), "XRNL");

																									double qut13Amount = GetQUT13Amount(double.Parse(payment), double.Parse(_row.Cells[6].Text), double.Parse(_row.Cells[7].Text), double.Parse(_row.Cells[8].Text), double.Parse(_row.Cells[9].Text), double.Parse(_row.Cells[12].Text), double.Parse(_row.Cells[8].Text), 0);


																									qryForUpdates += $@" UPDATE ""QUT1"" SET ""SapDocEntry"" = '{DPEntry}', 
                                                                                    ""AmountPaid"" = '{double.Parse(payment) + double.Parse(amountpaid)}'{(double.Parse(_row.Cells[14].Text) == 0 ? @",""LineStatus"" = 'C'" : "")} 
                                                                                    WHERE ""DocEntry"" = '{DocEntry}' AND ""Terms"" = '{Terms}' AND 
                                                                                    ""PaymentType"" = '{paymentdue_type}'  AND ""LineStatus"" <> 'R'; ";

																									//2023-10-11 : IF QUT13AMOUNT IS 0, THEN SKIP INSERTING TO QUT13
																									if (qut13Amount > 0)
																									{
																										qryForUpdates += $@" INSERT INTO QUT13 VALUES ({DocEntry}, {QUT1ID}, '{receiptNo}', 0, '{paymentdue_type}', {Terms}, {qut13Amount}, 0, 0, '{Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd")}', 
                                                                                                     '{Location}', {Session["UserID"].ToString()}, '{DateTime.Now.ToString("yyyyMMdd")}'); ";
																									}

																									DataTable dt = ws.IsPaymentForSpecificTransactionExists(DPEntry, 203).Tables[0];
																									if (double.Parse(DataAccess.GetData(dt, 0, "DocTotal", "0").ToString()) >= (double.Parse(_row.Cells[6].Text) + double.Parse(_row.Cells[7].Text) + double.Parse(_row.Cells[8].Text) + double.Parse(_row.Cells[9].Text))
																											&& balance <= 0)
																									{
																										DPEntry = 0;
																									}


																									DPEntries = $"{DPEntries}{DPEntry};";

																									//2024-08-17: changed payment from Payment in Rows only To Total Payment 
																									//JEForNoLTSNumber(cashRegister, ProjectCode, SapCardCode, payment, SODocEntry, FinancingScheme, TaxClassification,

																									//2024-08-19: revert back changes of amount 
																									//JEForNoLTSNumber(cashRegister, ProjectCode, SapCardCode, blockingTotalCurrentPayment.ToString(), SODocEntry, FinancingScheme, TaxClassification,
																									//JEForNoLTSNumber(cashRegister, ProjectCode, SapCardCode, payment, SODocEntry, FinancingScheme, TaxClassification,
																									//  Block, Lot, DepositFromCustomers, APOthersAccount, "WithoutLTS", txtDocNum.Text, Vatable, Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																									//    receiptNo, "NLTS", out isSuccess, out Message);

																									//2024-09-23: AMOUNT SHOULD ONLY BE PRINCIPAL 
																									//JEForNoLTSNumber(cashRegister, ProjectCode, SapCardCode, Math.Abs((double.Parse(_row.Cells[5].Text) + double.Parse(_row.Cells[12].Text)) - (double.Parse(_row.Cells[7].Text) + double.Parse(_row.Cells[8].Text) + double.Parse(_row.Cells[9].Text))), SODocEntry, FinancingScheme, TaxClassification,
																									//  Block, Lot, DepositFromCustomers, APOthersAccount, "WithoutLTS", txtDocNum.Text, Vatable, Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																									//    receiptNo, "NLTS", "", 0, out isSuccess, out Message);

																									//2024-10-03 : USE DP AMOUNT EVERYTIME FOR NLTS JE POSTING | ONLY POST WHEN DP WAS ONLY POSTED NOW 
																									//if (!Message.Contains("Existing"))
																									//{
																									//	JEForNoLTSNumber(cashRegister, ProjectCode, SapCardCode, _DPAmount, SODocEntry, FinancingScheme, TaxClassification,
																									//	 Block, Lot, DepositFromCustomers, APOthersAccount, "WithoutLTS", txtDocNum.Text, Vatable, Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																									//	   receiptNo, "NLTS", "", 0, out isSuccess, out Message);
																									//}

																									//2024-10-11: RAHA : POSTING OF PAID PRINCIPAL ONLY
																									//if (!Message.Contains("Existing"))
																									//{
																									//	JEForNoLTSNumber(cashRegister, ProjectCode, SapCardCode, Math.Abs((double.Parse(amountpaid)) - (double.Parse(_row.Cells[7].Text) + double.Parse(_row.Cells[8].Text) + double.Parse(_row.Cells[9].Text))), SODocEntry, FinancingScheme, TaxClassification,
																									//	 Block, Lot, DepositFromCustomers, APOthersAccount, "WithoutLTS", txtDocNum.Text, Vatable, Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																									//	   receiptNo, "NLTS", "", 0, out isSuccess, out Message);
																									//}

																									//2024-10-14: RAHA : POSTING OF PAID CURRENT PAYMENT ONLY 
																									//if (!Message.Contains("Existing"))
																									//{
																									//	JEForNoLTSNumber(cashRegister, ProjectCode, SapCardCode, Math.Abs(blockingTotalCurrentPayment - (double.Parse(_row.Cells[7].Text) + double.Parse(_row.Cells[8].Text) + double.Parse(_row.Cells[9].Text))), SODocEntry, FinancingScheme, TaxClassification,
																									//	 Block, Lot, DepositFromCustomers, APOthersAccount, "WithoutLTS", txtDocNum.Text, Vatable, Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																									//	   receiptNo, "NLTS", "", 0, out isSuccess, out Message);
																									//}

																									//2024-10-18: JMC: GET AMOUNT POSTED TO QUT13 ONLY 
																									//if (!Message.Contains("Existing"))
																									//{
																									//	JEForNoLTSNumber(cashRegister, ProjectCode, SapCardCode, qut13Amount, SODocEntry, FinancingScheme, TaxClassification,
																									//	 Block, Lot, DepositFromCustomers, APOthersAccount, "WithoutLTS", txtDocNum.Text, Vatable, Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																									//	   receiptNo, "NLTS", "", 0, out isSuccess, out Message);
																									//}

																									//2024-10-19: JMC: NEW FORMULA (PLEASE REFER ON THE EXCEL FILE)
																									if (!Message.Contains("Existing"))
																									{
																										double _nonTCPPayments = (double.Parse(_row.Cells[7].Text) + double.Parse(_row.Cells[8].Text) + double.Parse(_row.Cells[9].Text));

																										JEForNoLTSNumber(cashRegister, ProjectCode, SapCardCode, (double.Parse(_row.Cells[13].Text) - _nonTCPPayments), SODocEntry, FinancingScheme, TaxClassification,
																										 Block, Lot, DepositFromCustomers, APOthersAccount, "WithoutLTS", txtDocNum.Text, Vatable, Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																										   receiptNo, "NLTS", "", 0, out isSuccess, out Message);
																									}

																								}





























																								#region POSTING OF JOURNAL ENTRIES





																								//@@@@@@@@@@@@@@@  GET AMOUNT FOR FIRST DP AND RESERVATION AMOUNT FOR JE POSTINGS : "FirstDP" JE
																								double resAmt = 0;
																								double FirstDP = 0;





																								////2023-08-15 : PROCEED IF RES AND 1ST DP ARE NOT PAID
																								//string stringLOIToLTS = $@"select ""DocEntry"" from QUT1 where ""DocEntry"" = {DocEntry} and ""PaymentType"" in ('RES','DP') and ""Terms"" = 1 and ""LineStatus"" = 'O'";
																								//DataTable dtLOIToLTS = hana.GetData(stringLOIToLTS, hana.GetConnection("SAOHana"));


																								////2023-08-15 : PROCEED IF CONTRACT IS FROM LOI TO LTS
																								////2023-08-15 : CHECK IF RES AND 1ST DP EXISTS
																								//if (DataAccess.Exist(dtLOIToLTS) && (string.IsNullOrWhiteSpace(LTSNoCheck) && !string.IsNullOrWhiteSpace(OriginalLTSNoCheck)))
																								//{

																								//FOR RESERVATION
																								foreach (GridViewRow __row in gvDownPayments.Rows)
																								{

																									double Amount1 = __row.Cells[12].Text.Contains("nbsp") ? 0 : Convert.ToDouble(__row.Cells[12].Text);
																									double Amount2 = __row.Cells[13].Text.Contains("nbsp") ? 0 : Convert.ToDouble(__row.Cells[13].Text);


																									// ETO YUNG OLD SETUP
																									//if (__row.Cells[3].Text == "RES")
																									//{
																									//    resAmt = Amount1 + Amount2;
																									//} 
																									//if (__row.Cells[3].Text == "DP" && __row.Cells[2].Text == "1")
																									//{
																									//    FirstDP = Amount1 + Amount2;
																									//    break;
																									//}

																									//NEW SETUP
																									if (__row.Cells[3].Text == "RES")
																									{
																										resAmt = Amount1 + Amount2;
																										break;
																									}
																								}

																								//FOR 1ST DP
																								foreach (GridViewRow __row in gvDownPayments.Rows)
																								{
																									CheckBox _chkSel = (CheckBox)gvDownPayments.Rows[__row.RowIndex].FindControl("chkSel");
																									if (_chkSel.Checked)
																									{
																										if (__row.Cells[16].Text == "O")
																										{
																											if (__row.Cells[3].Text != "RES" && __row.Cells[3].Text != "MISC")
																											{
																												double Amount1 = __row.Cells[12].Text.Contains("nbsp") ? 0 : Convert.ToDouble(__row.Cells[12].Text);
																												double Amount2 = __row.Cells[13].Text.Contains("nbsp") ? 0 : Convert.ToDouble(__row.Cells[13].Text);

																												double FirstDPPrincipal = __row.Cells[6].Text.Contains("nbsp") ? 0 : Convert.ToDouble(__row.Cells[6].Text);

																												double FirstDPInterest = __row.Cells[7].Text.Contains("nbsp") ? 0 : Convert.ToDouble(__row.Cells[7].Text);
																												double FirstDPPenalty = __row.Cells[8].Text.Contains("nbsp") ? 0 : Convert.ToDouble(__row.Cells[8].Text);
																												double FirstDPIPS = __row.Cells[9].Text.Contains("nbsp") ? 0 : Convert.ToDouble(__row.Cells[9].Text);

																												double FirstDPTotalExclusion = FirstDPInterest + FirstDPPenalty + FirstDPIPS;
																												double FirstDPTotalPayment = (Amount2) - FirstDPTotalExclusion;

																												if (Math.Round(FirstDPPrincipal, 2) <= Math.Round(FirstDPTotalPayment, 2))
																												{
																													FirstDP += FirstDPTotalPayment;
																												}
																												else
																												{
																													FirstDP += FirstDPTotalPayment;
																													break;

																												}
																											}
																										}
																									}
																								}

																								//@@@@@@@@@@@@@@@ ADD JE ONLY IF FIRST DP
																								//2024-02-23 : MS KATE -- LOOK FOR JOURNAL ENTRIES WITH REF3 = 0 AND U_CANCEL <> 'Y'
																								//DataTable dtFirstDP = hana.GetData($@"SELECT ""TransId"" FROM OJDT WHERE ""Ref3"" = '0' AND ""U_BlockNo"" = '{Block}' AND ""U_LotNo"" = '{Lot}'
																								//                                AND ""Project"" = '{ProjectCode}'", hana.GetConnection("SAPHana"));
																								DataTable dtFirstDP = hana.GetData($@"SELECT ""TransId"" FROM OJDT WHERE ""Ref3"" = '0' AND ""U_BlockNo"" = '{Block}' AND ""U_LotNo"" = '{Lot}'
                                                                                                                            AND ""Project"" = '{ProjectCode}' AND IFNULL(""U_Cancel"",'N') = 'N';", hana.GetConnection("SAPHana"));


																								if (dtFirstDP.Rows.Count == 0)
																								{
																									////First Equity Payment
																									//if (paymentType == "DP" && paymentTerm == 1)
																									//{
																									//CHECK IF BUYER CLASSIFICATION = ENGAGED IN BUSINESS
																									if (TaxClassification.ToUpper() == ConfigSettings.TaxClassification1.ToUpper())
																									{

																										//GET SO DOCENTRY
																										dtSODocEntrty = hana.GetData($@"SELECT ""DocEntry"" from ORDR where ""U_BlockNo"" = '{Block}' AND ""U_LotNo"" = '{Lot}' AND ""Project"" = '{ProjectCode}' AND
                                                                                                                        ""DocStatus"" = 'O' AND IFNULL(""U_ContractStatus"",'Open') IN ('Open', 'Closed') ", hana.GetConnection("SAPHana"));
																										SODocEntry = int.Parse(DataHelper.DataTableRet(dtSODocEntrty, 0, "DocEntry", "0"));

																										double wthRate = 0;

																										//2023-08-29 : CHANGED USED NET TCP FOR CONDITIONS
																										//string qryWthRate = $@"SELECT IFNULL(""Rate"",0) ""Rate"" FROM OWHT WHERE ""U_ATC"" = '{ConfigSettings.ATCCode}' AND ""U_SellingPriceFrom"" <= {netTCP} AND ""U_SellingPriceTo"" >= {netTCP}";
																										string qryWthRate = $@"SELECT IFNULL(""Rate"",0) ""Rate"" FROM OWHT WHERE ""U_ATC"" = '{ConfigSettings.ATCCode}' AND ""U_SellingPriceFrom"" <= {double.Parse(lblNETTCP.Text)} AND ""U_SellingPriceTo"" >= {double.Parse(lblNETTCP.Text)}";

																										DataTable dtWthRate = hana.GetData(qryWthRate, hana.GetConnection("SAPHana"));
																										if (dtWthRate.Rows.Count > 0)
																										{
																											wthRate = (double.Parse(DataAccess.GetData(dtWthRate, 0, "Rate", "0").ToString())) / 100;
																										}



																										//POST ON JOURNAL ENTRY 
																										double amount = 0;

																										//2024-04-11 : ADDED CONDITION FOR DEFERRED AND INSTALLMENT FOR CWT RELATED JE POSTING
																										//if (Vatable.ToUpper() == "Y")
																										//{
																										//	amount = ((resAmt + FirstDP) / 1.12) * wthRate;
																										//}
																										//else
																										//{
																										//	amount = ((resAmt + FirstDP)) * wthRate;
																										//}
																										double cwtAmount = 0;
																										if (PaymentScheme == "Deferred")
																										{
																											//TOTAL PAYMENT  / NET TCP IS LESS THAN 25%
																											if ((blockingTotalCurrentPayment / Convert.ToDouble(lblNETTCP.Text)) >= Convert.ToDouble(_ARReserveInvoiceCondition1))
																											{
																												cwtAmount = Math.Abs(Convert.ToDouble(lblNETTCP.Text));
																											}
																										}
																										else
																										{
																											cwtAmount = resAmt + FirstDP;
																										}

																										if (Vatable.ToUpper() == "Y")
																										{
																											amount = (cwtAmount / 1.12) * wthRate;
																										}
																										else
																										{
																											amount = cwtAmount * wthRate;
																										}




																										//CHECK IF QUOTATION HAS LTSNO
																										if (checkLTS())
																										{
																											if (!string.IsNullOrWhiteSpace(OriginalLTSNoCheck))
																											{

																												if (isSuccess = cashRegister.CreateJournalEntry(null, ProjectCode, SapCardCode, amount, SODocEntry, FinancingScheme, TaxClassification,
																																  Block, Lot, CreditableWithholdingTaxAccount, "", "", "FirstDP", txtDocNum.Text, Terms.ToString(), paymentdue_type, Vatable, Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"), "NLFD",
																																  out JournalEntryNo, out Message, receiptNo))
																												{
																													TagFirstDP = 1;
																												}
																												else
																												{
																													alertMsg(Message, "warning");
																													break;
																												}
																											}
																										}

																									}

																									//}
																								}
																								//}
























																								////@@@@@@@@@@@@@@@  ADD JE ON EVERY SUCCEEDING PAYMENTS BEFORE 25% : "25TCP27" JE
																								///

																								//DataTable dtPaymentsBefore25 = hana.GetData($@"SELECT ""TransId"" FROM OJDT WHERE ""U_PaymentOrder"" = '{Terms.ToString()}' AND 
																								//                                ""U_PaymentType"" = '{paymentdue_type}' AND 
																								//                                ""U_BlockNo"" = '{Block}' AND ""U_LotNo"" = '{Lot}'
																								//                                AND ""Project"" = '{ProjectCode}'", hana.GetConnection("SAPHana"));

																								//if (dtPaymentsBefore25.Rows.Count == 0)


																								if (TagFirstDP == 0)
																								{

																									//NOT FIRST DP
																									//if (paymentType == "DP" && paymentTerm == 1)
																									//if (dtFirstDP.Rows.Count == 0)
																									//{

																									//}
																									//else
																									//{

																									//GET TOTAL PAYMENT
																									double __TotalPayment = 0;
																									foreach (GridViewRow __row in gvPayments.Rows)
																									{
																										__TotalPayment += Convert.ToDouble(__row.Cells[2].Text);
																									}


																									//2023-04-13 : DHEZA REQUEST: ADD BLOCKING: DO NOT POST THIS JE IF THE PREVIOUS PAYMENTS + CURRENT PAYMENT IS NOT EQUAL 25%
																									double CheckIf25Percent = (Convert.ToDouble(lblTotalPayment.Text) + __TotalPayment);



																									//2023-05-05 : CHECK IF DEFERRED OR INSTALLMENT
																									// IF DEFERRED = POST 25TCP27 WHEN % TOTAL TCP PAID IS LOWER THAN 25%
																									// IF INSTALLMENT = POST 25TCP27 EVERY TIME, REGARDLESS OF % TOTAL TCP PAID
																									int TagPosting = 0;

																									if (PaymentScheme == "Deferred")
																									{
																										//TOTAL PAYMENT  / NET TCP IS LESS THAN 25%
																										if ((CheckIf25Percent / double.Parse(lblNETTCP.Text)) < Convert.ToDouble(_ARReserveInvoiceCondition1))
																										{
																											TagPosting++;
																										}
																									}
																									else
																									{
																										TagPosting++;
																									}

																									double amount = 0;

																									//CHECK IF BUYER CLASSIFICATION = ENGAGED IN BUSINESS
																									if (Tag25TCP27 == 0)
																									{
																										if (TagPosting > 0)
																										{
																											if (TaxClassification.ToUpper() == ConfigSettings.TaxClassification1.ToUpper())
																											{


																												#region COMPUTATION OF AMOUNT FOR 25TCP27

																												//GET WITHHOLDING TAX RATE
																												double wthRate = 0;

																												//2023-08-29 : CHANGED NET TCP USED FOR CONDITIONS
																												//string qryWthRate = $@"SELECT IFNULL(""Rate"",0) ""Rate"" FROM OWHT WHERE ""U_ATC"" = '{ConfigSettings.ATCCode}' AND ""U_SellingPriceFrom"" <= {netTCP} AND ""U_SellingPriceTo"" >= {netTCP}";
																												string qryWthRate = $@"SELECT IFNULL(""Rate"",0) ""Rate"" FROM OWHT WHERE ""U_ATC"" = '{ConfigSettings.ATCCode}' AND ""U_SellingPriceFrom"" <= {double.Parse(lblNETTCP.Text)} AND ""U_SellingPriceTo"" >= {double.Parse(lblNETTCP.Text)}";

																												DataTable dtWthRate = hana.GetData(qryWthRate, hana.GetConnection("SAPHana"));
																												if (dtWthRate.Rows.Count > 0)
																												{
																													wthRate = (double.Parse(DataAccess.GetData(dtWthRate, 0, "Rate", "0").ToString())) / 100;
																												}


																												double Surcharge27 = 0;
																												double Interest27 = 0;
																												double IPS27 = 0;

																												//loop to get interest and surcharge
																												foreach (GridViewRow row25TCP27 in gvDownPayments.Rows)
																												{
																													CheckBox chkSel27 = (CheckBox)gvDownPayments.Rows[row25TCP27.RowIndex].FindControl("chkSel");
																													if (chkSel27.Checked)
																													{
																														if (row25TCP27.Cells[16].Text == "O")
																														{
																															Surcharge27 += Convert.ToDouble(row25TCP27.Cells[7].Text);
																															Interest27 += Convert.ToDouble(row25TCP27.Cells[8].Text);
																															IPS27 += Convert.ToDouble(row25TCP27.Cells[9].Text);
																														}
																													}
																												}

																												double totalExclusion27 = Surcharge27 + Interest27 + IPS27;

																												//POST ON JOUR  NAL ENTRY 
																												if (Vatable.ToUpper() == "Y")
																												{
																													//amount = (double.Parse(payment) / 1.12) * wthRate;
																													amount = ((__TotalPayment - totalExclusion27) / 1.12) * wthRate;
																												}
																												else
																												{
																													//amount = (double.Parse(payment)) * wthRate;
																													amount = (__TotalPayment - totalExclusion27) * wthRate;
																												}


																												//                                      string qryGetSurchargePayments = $@"SELECT IFNULL(SUM(A.""DocTotal""),0) ""DocTotal"" FROM OINV A WHERE A.""Project"" = '{txtProj.Text}' AND
																												//A.""U_BlockNo"" = '{txtBlock.Text}' AND A.""U_LotNo"" = '{txtLot.Text}' AND A.""CANCELED"" <> 'Y'  AND 
																												//                        A.""DocStatus"" = 'C' AND A.""U_TransactionType"" IN ('RE - SUR','INT INC', 'IP&S')  AND IFNULL(A.""U_ContractStatus"",'Open') IN ('Open', 'Closed') ";
																												//                                      DataTable dtGetSurchargePayments = hana.GetData(qryGetSurchargePayments, hana.GetConnection("SAPHana"));
																												//                                      double GetSurchargePayments = double.Parse(DataAccess.GetData(dtGetSurchargePayments, 0, "DocTotal", "0").ToString());

																											}
																											#endregion


																											//CHECK IF QUOTATION HAS LTSNO : POSTING OF 25TCP27
																											if (checkLTS())
																											{
																												if (isSuccess = cashRegister.CreateJournalEntry(null, ProjectCode, SapCardCode, amount, SODocEntry, FinancingScheme, TaxClassification,
																																	//2023-07-25 : CHANGED TRANSCODE FROM "QNLT" TO "DEBW"
																																	//Block, Lot, CreditableWithholdingTaxAccount, "", "", "25TCP27", txtDocNum.Text, Terms.ToString(), paymentdue_type, Vatable, Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"), "QNLT",
																																	//2023-08-02 : CHANGED TRANSCODE FROM "DEBW" TO "B25T"
																																	//Block, Lot, CreditableWithholdingTaxAccount, "", "", "25TCP27", txtDocNum.Text, Terms.ToString(), paymentdue_type, Vatable, Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"), "DEBW",
																																	Block, Lot, CreditableWithholdingTaxAccount, "", "", "25TCP27", txtDocNum.Text, Terms.ToString(), paymentdue_type, Vatable, Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"), "B25T",
																																	out JournalEntryNo, out Message, receiptNo))
																												{
																													Tag25TCP27 = 1;
																												}
																												else
																												{
																													alertMsg(Message, "warning");
																													break;
																												}
																											}
																										}
																										//}
																									}










																								}

																								#endregion





																							}
																						}

																						//GET CURRENT PAYMENT
																						double currentPayment0 = Convert.ToDouble(_row.Cells[13].Text);




																						if (paymentType == "LB" && paymentTerm == 1)
																						{
																							string test = "";
																						}




																						if (LineStatus == "O")
																						{
																							if (SurCharges > 0)
																							{

																								string qryGetWaived = $@"SELECT IFNULL(COUNT(""DocEntry""),0) ""DocEntry"" FROM OINV WHERE ""U_BlockNo"" = '{Block}' AND ""U_LotNo"" = '{Lot}' AND ""U_Project"" = '{ProjectCode}' AND 
                                                                                    ""U_Type"" = '{paymentType}' AND ""U_PaymentOrder"" = '{paymentTerm}' AND ""CANCELED"" = 'N' AND ""U_TransactionType"" = 'RE - SUR' AND
                                                                                        ""U_Waive"" = 'Y'  AND IFNULL(""U_ContractStatus"",'Open') IN ('Open', 'Closed') ";
																								DataTable dtGetWaived = hana.GetData(qryGetWaived, hana.GetConnection("SAPHana"));
																								string GetWaived = "0";
																								if (dtGetWaived.Rows.Count > 0)
																								{
																									GetWaived = DataAccess.GetData(dtGetWaived, 0, "DocEntry", "0").ToString();
																								}

																								//CHECK IF WAIVED SURCHARGE
																								if (GetWaived == "0")
																								{
																									string qrySurcharge = $@"SELECT SUM(""PaidToDate"") ""PaidToDate"" FROM OINV WHERE ""U_Type"" = '{paymentType}' AND ""U_PaymentOrder"" = '{paymentTerm}' 
                                                                              AND ""Project"" = '{ProjectCode}' AND ""U_BlockNo"" = '{Block}' AND ""U_LotNo"" = '{Lot}' AND 
                                                                              ""CardCode"" = '{SapCardCode}'  AND ""CANCELED"" = 'N' AND ""U_TransactionType"" = 'RE - SUR'  AND IFNULL(""U_ContractStatus"",'Open') IN ('Open', 'Closed')";
																									DataTable dtSurcharge = hana.GetData(qrySurcharge, hana.GetConnection("SAPHana"));
																									double ARResSurcharge = 0;

																									if (dtSurcharge.Rows.Count > 0)
																									{
																										ARResSurcharge = Convert.ToDouble(DataAccess.GetData(dtSurcharge, 0, "PaidToDate", "0").ToString());
																									}

																									double SurChargesCurrentPayment = currentPayment0;
																									if (SurChargesCurrentPayment >= (SurCharges - ARResSurcharge))
																									{
																										SurChargesCurrentPayment = SurCharges - ARResSurcharge;
																									}
																									//if (SurChargesCurrentPayment > 0)
																									//{


																									//WAIVED SURCHARGE -- CHECK TAGGING IN "MISC" COLUMN IF IT IS WAIVED
																									string qryWaiveTag = $@"SELECT ""Misc"",""Penalty"" FROM QUT1 WHERE ""DocEntry"" = {DocEntry} AND ""Terms"" = '{paymentTerm}' AND ""PaymentType"" = '{paymentType}'";
																									DataTable dtWaiveTag = hana.GetData(qryWaiveTag, hana.GetConnection("SAOHana"));
																									int waiveTag = Convert.ToInt32(DataAccess.GetData(dtWaiveTag, 0, "Misc", "0").ToString());
																									string waive = "N";
																									if (waiveTag > 0)
																									{
																										waive = "Y";
																									}

																									//CREATE AR INVOICE FOR SURCHARGE POSTING
																									if (isSuccess = cashRegister.CreateARInvoiceStandalone(null,
																										SapCardCode, //2
																										Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																										//SurChargesCurrentPayment, //4
																										SurCharges, //4
																										paymentType,
																										paymentTerm.ToString(), //6
																									   ProjectCode,
																										Block, //8
																										Lot,
																										HouseModel.ToString(), //10
																										FinancingScheme,
																										ProdType, //12
																										ReservationFree,
																										DPAmount, //14
																										LBAmount,
																										"Surcharge", //16
																										txtDocNum.Text,
																										 ConfigSettings.IPSControlAccount, //18
																										Vatable,
																										SurCharges, //20
																										partialPaymentTag,
																										waive, //22
																									  Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																										out SurchargeAREntry,
																										out Message)) //24

																									{


																										if (ARResSurcharge >= double.Parse(_row.Cells[8].Text))
																										{
																											//SurchargeAREntry = 0;
																										}
																										else
																										{
																											currentPayment0 = currentPayment0 - SurChargesCurrentPayment;
																										}


																										qryForUpdates += $@" UPDATE ""QUT1"" SET ""Penalty"" = '{SurCharges}', ""AmountPaid"" = '{double.Parse(payment) + double.Parse(amountpaid)}' WHERE ""DocEntry"" = '{DocEntry}' AND ""Terms"" = '{Terms}' AND ""PaymentType"" = '{paymentdue_type}'  AND ""LineStatus"" <> 'R'; ";
																										InvoiceEntries = $"{InvoiceEntries}{SurchargeAREntry};";

																										qryForUpdates += $@" INSERT INTO QUT13 VALUES ({DocEntry}, {QUT1ID}, '{receiptNo}', 0, '{paymentdue_type}', {Terms}, 0, 0, {SurCharges}, '{Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd")}', 
                                                                                              '{Location}', {Session["UserID"].ToString()}, '{DateTime.Now.ToString("yyyyMMdd")}'); ";



																										//2024-09-23: SURCHARGE POSTING FOR JE WHEN NO LTS 
																										if (string.IsNullOrWhiteSpace(LTSNo))
																										{
																											//GET AR INVOICE'S JE -- Output Tax 
																											string qrySurchargeJEOutPutTax = $@" SELECT B.""Credit""
                                                                                                                                            FROM OJDT A INNER JOIN JDT1 B ON A.""TransId"" = B.""TransId""
                                                                                                                                                WHERE A.""CreatedBy"" = {SurchargeAREntry} and A.""TransType"" = 13 AND B.""Account"" = '{OutputTaxSurcharge}'";
																											DataTable dtSurchargeJEOutPutTax = hana.GetData(qrySurchargeJEOutPutTax, hana.GetConnection("SAPHana"));
																											double SurchargeJEOutPutTax = Convert.ToDouble(DataAccess.GetData(dtSurchargeJEOutPutTax, 0, "Credit", "0").ToString());

																											//GET AR INVOICE'S JE -- Surcharge/Interest Amount 
																											string qrySurchargeJESurcharge = $@" SELECT B.""Credit""
                                                                                                                                            FROM OJDT A INNER JOIN JDT1 B ON A.""TransId"" = B.""TransId""
                                                                                                                                                WHERE A.""CreatedBy"" = {SurchargeAREntry} and A.""TransType"" = 13 AND B.""Account"" = '{SuchargeVATAccount}'";
																											DataTable dtSurchargeJESurcharge = hana.GetData(qrySurchargeJESurcharge, hana.GetConnection("SAPHana"));
																											double SurchargeJESurcharge = Convert.ToDouble(DataAccess.GetData(dtSurchargeJESurcharge, 0, "Credit", "0").ToString());

																											JEForNoLTSNumber(cashRegister, ProjectCode, SapCardCode, SurchargeJEOutPutTax, SODocEntry, FinancingScheme, TaxClassification,
																											Block, Lot, OutputTaxSurcharge, SuchargeVATAccount, "NLSC", txtDocNum.Text, Vatable, Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																											receiptNo, "NLSC", APOthersAccount, SurchargeJESurcharge, out isSuccess, out Message);
																										}


																									}
																									//}
																								}
																							}
																						}

																						if (paymentTerm == 8 && paymentdue_type == "LB")
																						{
																							string test1 = "";
																						}
																						if (Interest > 0)
																						{

																							string qryARResInterest = $@"SELECT SUM(""PaidToDate"") ""PaidToDate"" FROM OINV WHERE ""U_Type"" = '{paymentType}' AND ""U_PaymentOrder"" = '{paymentTerm}' 
                                                                              AND ""Project"" = '{ProjectCode}' AND ""U_BlockNo"" = '{Block}' AND ""U_LotNo"" = '{Lot}' AND 
                                                                              ""CardCode"" = '{SapCardCode}'  AND ""CANCELED"" = 'N' AND ""U_TransactionType"" = 'INT INC'  AND IFNULL(""U_ContractStatus"",'Open') IN ('Open', 'Closed')";
																							DataTable dtARResInterest = hana.GetData(qryARResInterest, hana.GetConnection("SAPHana"));
																							double ARResInterest = 0;

																							if (dtARResInterest.Rows.Count > 0)
																							{
																								ARResInterest = Convert.ToDouble(DataAccess.GetData(dtARResInterest, 0, "PaidToDate", "0").ToString());
																							}


																							double InterestCurrentPayment = currentPayment0;
																							if (InterestCurrentPayment >= (Interest - ARResInterest))
																							{
																								InterestCurrentPayment = Interest - ARResInterest;
																							}

																							if (InterestCurrentPayment > 0)
																							{

																								//Interest Payment
																								//totalPayment = (totalPayment >= Interest) ? totalPayment - Interest : totalPayment;
																								//if (isSuccess = cashRegister.CreateARInvoice(null,
																								//                                                SapCardCode,
																								//                                                Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																								//                                                totalPayment,
																								//                                                paymentdue_type,
																								//                                                "QryGroup5",
																								//                                                Terms.ToString(),
																								//                                                ProjectCode,
																								//                                                Block,
																								//                                                Lot,
																								//                                                SOEntry,
																								//                                                out InterestAREntry,
																								//                                                out Message))
																								if (isSuccess = cashRegister.CreateARInvoiceStandalone(null,
																								SapCardCode, //2
																								Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																								InterestCurrentPayment, //4
																								paymentType,
																								paymentTerm.ToString(), //6
																							   ProjectCode,
																								Block, //8
																								Lot,
																								HouseModel.ToString(),
																								FinancingScheme,
																								ProdType,
																								ReservationFree,
																								DPAmount,
																								LBAmount,
																								"Interest",
																								txtDocNum.Text,
																								 ConfigSettings.IPSControlAccount,
																								Vatable,
																								Interest,
																								partialPaymentTag,
																								"N",
																									  Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																								out InterestAREntry, //10
																								out Message))
																								{

																									if (ARResInterest >= double.Parse(_row.Cells[7].Text))
																									{
																										InterestAREntry = 0;
																									}
																									else
																									{
																										currentPayment0 = currentPayment0 - InterestCurrentPayment;
																									}

																									qryForUpdates += $@" UPDATE ""QUT1"" SET ""InterestAmount"" = '{Interest}',""AmountPaid"" = '{double.Parse(payment) + double.Parse(amountpaid)}' WHERE ""DocEntry"" = '{DocEntry}' AND ""Terms"" = '{Terms}' AND ""PaymentType"" = '{paymentdue_type}'  AND ""LineStatus"" <> 'R'; ";
																									InvoiceEntries = $"{InvoiceEntries}{InterestAREntry};";


																									qryForUpdates += $@" INSERT INTO QUT13 VALUES ({DocEntry}, {QUT1ID}, '{receiptNo}', 0, '{paymentdue_type}', {Terms}, 0, {InterestCurrentPayment}, 0, '{Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd")}', 
                                                                                    '{Location}',{Session["UserID"].ToString()}, '{DateTime.Now.ToString("yyyyMMdd")}'); ";


																									//2024-09-23: INTEREST POSTING FOR JE WHEN NO LTS 
																									if (string.IsNullOrWhiteSpace(LTSNo))
																									{
																										//GET AR INVOICE'S JE -- Output Tax 
																										string qryInterestJEOutPutTax = $@" SELECT B.""Credit""
                                                                                                                                            FROM OJDT A INNER JOIN JDT1 B ON A.""TransId"" = B.""TransId""
                                                                                                                                                WHERE A.""CreatedBy"" = {InterestAREntry} and A.""TransType"" = 13 AND B.""Account"" = '{OutputTaxInterest}'";
																										DataTable dtInerestJEOutPutTax = hana.GetData(qryInterestJEOutPutTax, hana.GetConnection("SAPHana"));
																										double InterestJEOutPutTax = Convert.ToDouble(DataAccess.GetData(dtInerestJEOutPutTax, 0, "Credit", "0").ToString());

																										//GET AR INVOICE'S JE -- INTEREST/Interest Amount 
																										string qryInterestJESurcharge = $@" SELECT B.""Credit""
                                                                                                                                            FROM OJDT A INNER JOIN JDT1 B ON A.""TransId"" = B.""TransId""
                                                                                                                                                WHERE A.""CreatedBy"" = {InterestAREntry} and A.""TransType"" = 13 AND B.""Account"" = '{InterestVATAccount}'";
																										DataTable dtInterestJESurcharge = hana.GetData(qryInterestJESurcharge, hana.GetConnection("SAPHana"));
																										double InterestJESurcharge = Convert.ToDouble(DataAccess.GetData(dtInterestJESurcharge, 0, "Credit", "0").ToString());

																										JEForNoLTSNumber(cashRegister, ProjectCode, SapCardCode, InterestJEOutPutTax, SODocEntry, FinancingScheme, TaxClassification,
																										Block, Lot, OutputTaxInterest, InterestVATAccount, "NLIT", txtDocNum.Text, Vatable, Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																										receiptNo, "NLIT", APOthersAccount, InterestJESurcharge, out isSuccess, out Message);
																									}




																								}
																							}
																						}


																						if (IPS > 0)
																						{

																							string qryIPS = $@"SELECT SUM(""PaidToDate"") ""PaidToDate"" FROM OINV WHERE ""U_Type"" = '{paymentType}' AND ""U_PaymentOrder"" = '{paymentTerm}' 
                                                                              AND ""Project"" = '{ProjectCode}' AND ""U_BlockNo"" = '{Block}' AND ""U_LotNo"" = '{Lot}' AND 
                                                                              ""CardCode"" = '{SapCardCode}'  AND ""CANCELED"" = 'N' AND ""U_TransactionType"" = 'IP&S'  AND IFNULL(""U_ContractStatus"",'Open') IN ('Open', 'Closed')";
																							DataTable dtIPS = hana.GetData(qryIPS, hana.GetConnection("SAPHana"));
																							double ARResIPS = 0;


																							if (dtIPS.Rows.Count > 0)
																							{

																								ARResIPS = Convert.ToDouble(DataAccess.GetData(dtIPS, 0, "PaidToDate", "0").ToString());
																							}


																							double IPSCurrentPayment = currentPayment0;
																							if (IPSCurrentPayment >= (IPS - ARResIPS))
																							{
																								IPSCurrentPayment = IPS - ARResIPS;
																							}
																							if (IPSCurrentPayment > 0)
																							{

																								if (isSuccess = cashRegister.CreateARInvoiceStandalone(null,
																									   SapCardCode, //2 
																									   Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																									   IPSCurrentPayment, //4
																									   paymentType,
																									   paymentTerm.ToString(), //6
																									  ProjectCode,
																									   Block, //8
																									   Lot,
																									   HouseModel.ToString(), //10
																									   FinancingScheme,
																									   ProdType, //12
																									   ReservationFree,
																									   DPAmount, //14
																									   LBAmount,
																									   "IPS", //16
																									   txtDocNum.Text,
																										ConfigSettings.IPSControlAccount, //18
																									   Vatable,
																									   IPS,
																									   partialPaymentTag,
																									   "N",
																									   Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																									   out IPSEntry,
																									   out Message)) //20
																								{
																									//hana.Execute($@"UPDATE ""QUT1"" SET ""Penalty"" = '{SurCharges}' WHERE ""DocEntry"" = '{DocEntry}' AND ""Terms"" = '{Terms}' AND ""PaymentType"" = '{paymentdue_type}';", hana.GetConnection("SAOHana"));

																									//DataTable dt = ws.IsPaymentForSpecificTransactionExists(IPSEntry, 13).Tables[0];
																									//if (double.Parse(DataAccess.GetData(dt, 0, "DocTotal", "0").ToString()) >= (double.Parse(_row.Cells[6].Text) + double.Parse(_row.Cells[7].Text) + double.Parse(_row.Cells[8].Text) + double.Parse(_row.Cells[9].Text)))
																									//{
																									//    SurchargeAREntry = 0;
																									//}


																									if (ARResIPS >= double.Parse(_row.Cells[9].Text))
																									{
																										IPSEntry = 0;
																									}



																									//else
																									//{
																									qryForUpdates += $@" UPDATE ""QUT1"" SET ""IPS"" = '{IPS}',""AmountPaid"" = '{double.Parse(payment) + double.Parse(amountpaid)}' WHERE ""DocEntry"" = '{DocEntry}' AND ""Terms"" = '{Terms}' AND ""PaymentType"" = '{paymentdue_type}'  AND ""LineStatus"" <> 'R'; ";
																									//}

																									InvoiceEntries = $"{InvoiceEntries}{IPSEntry};";
																								}
																							}
																						}


																						//InvoiceEntries = "";
																						//DPEntries = "";

																					}

























																					//@@@@@@@@@@@@@@@ IF AR RESERVE INVOICE ALREADY EXISTS
																					else
																					{


																						if (paymentType == "LB" && paymentTerm == 13)
																						{
																							string test = "";
																						}



																						////2024-11-12 : JMC : ADDING OF QUT13 WHEN AR RES INVOICE EXISTS
																						//double qut13Amount = GetQUT13Amount(double.Parse(payment), double.Parse(_row.Cells[6].Text), double.Parse(_row.Cells[7].Text), double.Parse(_row.Cells[8].Text), double.Parse(_row.Cells[9].Text), double.Parse(_row.Cells[12].Text), double.Parse(_row.Cells[8].Text), 0);


																						//qryForUpdates += $@" UPDATE ""QUT1"" SET ""SapDocEntry"" = '{DPEntry}', 
																						//		""AmountPaid"" = '{double.Parse(payment) + double.Parse(amountpaid)}'{(double.Parse(_row.Cells[14].Text) == 0 ? @",""LineStatus"" = 'C'" : "")} 
																						//		WHERE ""DocEntry"" = '{DocEntry}' AND ""Terms"" = '{Terms}' AND 
																						//		""PaymentType"" = '{paymentdue_type}'  AND ""LineStatus"" <> 'R'; ";

																						////2023-10-11 : IF QUT13AMOUNT IS 0, THEN SKIP INSERTING TO QUT13
																						//if (qut13Amount > 0)
																						//{
																						//	qryForUpdates += $@" INSERT INTO QUT13 VALUES ({DocEntry}, {QUT1ID}, '{receiptNo}', 0, '{paymentdue_type}', {Terms}, {qut13Amount}, 0, 0, '{Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd")}', 
																						//                                                                               '{Location}', {Session["UserID"].ToString()}, '{DateTime.Now.ToString("yyyyMMdd")}'); ";
																						//}



																						string ARReserveEntry = DataAccess.GetData(_ARResExisting, 0, "DocEntry", "0").ToString();

																						//AR RESERVCE INVOICE
																						int ARResctr = 0;

																						//check if AR Res Invoice # is alraedy in the string
																						foreach (var docEntry in InvoiceEntries.Split(';'))
																						{
																							if (docEntry == ARReserveEntry)
																							{
																								ARResctr++;
																							}
																						}

																						if (ARResctr == 0)
																						{
																							if ((double.Parse(payment) + double.Parse(amountpaid)) > (SurCharges + Interest + IPS))
																							{
																								if (LineStatus != "C")
																								{
																									InvoiceEntries = $"{InvoiceEntries}{ARReserveEntry};";
																								}
																							}
																						}


																						if (LineStatus != "C")
																						{
																							ViewState["Terms"] = Terms.ToString();
																							ViewState["PaymentType"] = paymentdue_type;


																							//GET CURRENT PAYMENT
																							double currentPayment0 = Convert.ToDouble(_row.Cells[13].Text);


																							//2023-10-16 : GET PAYMENTS FOR CONDITION IN QUT13 FUNCTION
																							double InterestCurrentPaymentForQut13 = 0;
																							double SurchargeCurrentPaymentForQut13 = 0;

																							if (SurCharges > 0)
																							{


																								string qryGetWaived = $@"SELECT IFNULL(COUNT(""DocEntry""),0) ""DocEntry"" FROM OINV WHERE ""U_BlockNo"" = '{Block}' AND ""U_LotNo"" = '{Lot}' AND ""U_Project"" = '{ProjectCode}' AND 
                                                                                    ""U_Type"" = '{paymentType}' AND ""U_PaymentOrder"" = '{paymentTerm}' AND ""CANCELED"" = 'N' AND ""U_TransactionType"" = 'RE - SUR' AND
                                                                                        ""U_Waive"" = 'Y'  AND IFNULL(""U_ContractStatus"",'Open') IN ('Open', 'Closed') ";
																								DataTable dtGetWaived = hana.GetData(qryGetWaived, hana.GetConnection("SAPHana"));
																								string GetWaived = "0";
																								if (dtGetWaived.Rows.Count > 0)
																								{
																									GetWaived = DataAccess.GetData(dtGetWaived, 0, "DocEntry", "0").ToString();
																								}

																								//CHECK IF WAIVED SURCHARGE
																								if (GetWaived == "0")
																								{


																									string qrySurcharge = $@"SELECT SUM(""PaidToDate"") ""PaidToDate"" FROM OINV WHERE ""U_Type"" = '{paymentType}' AND ""U_PaymentOrder"" = '{paymentTerm}' 
                                                                              AND ""Project"" = '{ProjectCode}' AND ""U_BlockNo"" = '{Block}' AND ""U_LotNo"" = '{Lot}' AND 
                                                                              ""CardCode"" = '{SapCardCode}'  AND ""CANCELED"" = 'N' AND ""U_TransactionType"" = 'RE - SUR'  AND IFNULL(""U_ContractStatus"",'Open') IN ('Open', 'Closed')";
																									DataTable dtSurcharge = hana.GetData(qrySurcharge, hana.GetConnection("SAPHana"));
																									double ARResSurcharge = 0;

																									if (dtSurcharge.Rows.Count > 0)
																									{
																										ARResSurcharge = Convert.ToDouble(DataAccess.GetData(dtSurcharge, 0, "PaidToDate", "0").ToString());
																									}

																									double SurChargeCurrentPayment = currentPayment0;
																									if (SurChargeCurrentPayment >= (SurCharges - ARResSurcharge))
																									{
																										SurChargeCurrentPayment = SurCharges - ARResSurcharge;
																									}
																									if (SurChargeCurrentPayment > 0)
																									{

																										//WAIVED SURCHARGE -- CHECK TAGGING IN "MISC" COLUMN IF IT IS WAIVED
																										string qryWaiveTag = $@"SELECT ""Misc"",""Penalty"" FROM QUT1 WHERE ""DocEntry"" = {DocEntry} AND ""Terms"" = '{paymentTerm}' AND ""PaymentType"" = '{paymentType}'";
																										DataTable dtWaiveTag = hana.GetData(qryWaiveTag, hana.GetConnection("SAOHana"));
																										int waiveTag = Convert.ToInt32(DataAccess.GetData(dtWaiveTag, 0, "Misc", "0").ToString());
																										string waive = "N";
																										if (waiveTag > 0)
																										{
																											waive = "Y";
																										}


																										//SurCharges Payment
																										if (isSuccess = cashRegister.CreateARInvoiceStandalone(null,
																																				SapCardCode, //2
																																				Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																																				SurChargeCurrentPayment, //4 
																																				paymentType,
																																				paymentTerm.ToString(), //6
																																			   ProjectCode,
																																				Block, //8
																																				Lot,
																																				HouseModel.ToString(),
																																				FinancingScheme,
																																				ProdType,
																																				ReservationFree,
																																				DPAmount,
																																				LBAmount,
																																				"Surcharge",
																																				txtDocNum.Text,
																																				 ConfigSettings.IPSControlAccount,
																																				Vatable,
																																				SurCharges,
																																				partialPaymentTag,
																																				waive,
																									  Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																																				out SurchargeAREntry, //10
																																				out Message))
																										{
																											//DataTable dt = ws.IsPaymentForSpecificTransactionExists(SurchargeAREntry, 13).Tables[0];

																											//if (double.Parse(DataAccess.GetData(dt, 0, "DocTotal", "0").ToString()) >= (double.Parse(_row.Cells[6].Text) + double.Parse(_row.Cells[7].Text) + double.Parse(_row.Cells[8].Text) + double.Parse(_row.Cells[9].Text)))
																											//{
																											// SurchargeAREntry = 0;
																											//}


																											if (ARResSurcharge >= double.Parse(_row.Cells[8].Text))
																											{
																												SurchargeAREntry = 0;
																											}
																											else
																											{
																												currentPayment0 = currentPayment0 - SurChargeCurrentPayment;
																											}

																											//else
																											//{
																											//hana.Execute($@"UPDATE ""QUT1"" SET ""Penalty"" = '{SurCharges}' WHERE ""DocEntry"" = '{DocEntry}' AND ""Terms"" = '{paymentTerm}' AND ""PaymentType"" = '{paymentType}';", hana.GetConnection("SAOHana"));
																											qryForUpdates += $@" UPDATE ""QUT1"" SET ""Penalty"" = '{SurCharges}' ,""AmountPaid"" = '{double.Parse(payment) + double.Parse(amountpaid)}' WHERE ""DocEntry"" = '{DocEntry}' AND ""Terms"" = '{paymentTerm}' AND ""PaymentType"" = '{paymentType}'  AND ""LineStatus"" <> 'R'; ";
																											//}
																											InvoiceEntries = $"{InvoiceEntries}{SurchargeAREntry};";

																											qryForUpdates += $@" INSERT INTO QUT13 VALUES ({DocEntry}, {QUT1ID}, '{receiptNo}', 0, '{paymentdue_type}', {Terms}, 0, 0, {SurChargeCurrentPayment}, '{Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd")}', 
                                                                                              '{Location}', {Session["UserID"].ToString()}, '{DateTime.Now.ToString("yyyyMMdd")}'); ";
																											SurchargeCurrentPaymentForQut13 = double.Parse(payment) + double.Parse(amountpaid);


																										}
																									}
																								}
																							}

																							if (paymentTerm == 8 && paymentdue_type == "LB")
																							{
																								string test1 = "";
																							}
																							if (Interest > 0)
																							{


																								string qryInterest = $@"SELECT SUM(""PaidToDate"") ""PaidToDate"" FROM OINV WHERE ""U_Type"" = '{paymentType}' AND ""U_PaymentOrder"" = '{paymentTerm}' 
                                                                              AND ""Project"" = '{ProjectCode}' AND ""U_BlockNo"" = '{Block}' AND ""U_LotNo"" = '{Lot}' AND 
                                                                              ""CardCode"" = '{SapCardCode}'  AND ""CANCELED"" = 'N' AND ""U_TransactionType"" = 'INT INC'  AND IFNULL(""U_ContractStatus"",'Open') IN ('Open', 'Closed')";
																								DataTable dtInterest = hana.GetData(qryInterest, hana.GetConnection("SAPHana"));
																								double ARResInterest = 0;

																								if (dtInterest.Rows.Count > 0)
																								{
																									ARResInterest = Convert.ToDouble(DataAccess.GetData(dtInterest, 0, "PaidToDate", "0").ToString());
																								}


																								double InterestCurrentPayment = currentPayment0;
																								if (InterestCurrentPayment >= (Interest - ARResInterest))
																								{
																									InterestCurrentPayment = Interest - ARResInterest;
																								}

																								if (InterestCurrentPayment > 0)
																								{

																									if (isSuccess = cashRegister.CreateARInvoiceStandalone(null,
																																				  SapCardCode, //2
																																					Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																																					InterestCurrentPayment, //4
																																					paymentType,
																																					paymentTerm.ToString(), //6
																																					ProjectCode,
																																					Block, //8
																																					Lot,
																																					HouseModel,
																																					FinancingScheme,
																																					ProdType,
																																					ReservationFree,
																																					DPAmount,
																																					LBAmount,
																																					"Interest",
																																					txtDocNum.Text,
																																					 ConfigSettings.IPSControlAccount,
																																					Vatable,
																																					Interest,
																																					partialPaymentTag,
																																					"N",
																									  Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																																					out InterestAREntry, //10
																																					out Message))
																									{
																										if (ARResInterest >= double.Parse(_row.Cells[7].Text))
																										{
																											InterestAREntry = 0;
																										}
																										else
																										{
																											currentPayment0 = currentPayment0 - InterestCurrentPayment;
																										}

																										dtInterestPayments.Rows.Add(InterestAREntry, InterestCurrentPayment);
																										ViewState["InterestAmount"] = dtInterestPayments;


																										//hana.Execute($@"UPDATE ""QUT1"" SET ""InterestAmount"" = '{Interest}' WHERE ""DocEntry"" = '{DocEntry}' AND ""Terms"" = '{paymentTerm}' AND ""PaymentType"" = '{paymentType}';", hana.GetConnection("SAOHana"));
																										qryForUpdates += $@" UPDATE ""QUT1"" SET ""InterestAmount"" = '{Interest}',""AmountPaid"" = '{double.Parse(payment) + double.Parse(amountpaid)}' WHERE ""DocEntry"" = '{DocEntry}' AND ""Terms"" = '{paymentTerm}' AND ""PaymentType"" = '{paymentType}'  AND ""LineStatus"" <> 'R'; ";
																										InvoiceEntries = $"{InvoiceEntries}{InterestAREntry};";

																										qryForUpdates += $@" INSERT INTO QUT13 VALUES ({DocEntry}, {QUT1ID}, '{receiptNo}', 0, '{paymentdue_type}', {Terms}, 0, {InterestCurrentPayment}, 0, '{Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd")}', 
                                                                                    '{Location}',{Session["UserID"].ToString()}, '{DateTime.Now.ToString("yyyyMMdd")}'); ";
																										InterestCurrentPaymentForQut13 = InterestCurrentPayment;

																									}
																								}
																							}



																							if (IPS > 0)
																							{



																								string qryIPS = $@"SELECT SUM(""PaidToDate"") ""PaidToDate"" FROM OINV WHERE ""U_Type"" = '{paymentType}' AND ""U_PaymentOrder"" = '{paymentTerm}' 
                                                                              AND ""Project"" = '{ProjectCode}' AND ""U_BlockNo"" = '{Block}' AND ""U_LotNo"" = '{Lot}' AND 
                                                                              ""CardCode"" = '{SapCardCode}'  AND ""CANCELED"" = 'N' AND ""U_TransactionType"" = 'IP&S'  AND IFNULL(""U_ContractStatus"",'Open') IN ('Open', 'Closed')";
																								DataTable dtIPS = hana.GetData(qryIPS, hana.GetConnection("SAPHana"));
																								double ARResIPS = 0;



																								if (dtIPS.Rows.Count > 0)
																								{

																									ARResIPS = Convert.ToDouble(DataAccess.GetData(dtIPS, 0, "PaidToDate", "0").ToString());
																								}

																								double IPSCurrentPayment = currentPayment0;
																								if (IPSCurrentPayment >= (IPS - ARResIPS))
																								{
																									IPSCurrentPayment = IPS - ARResIPS;
																								}
																								if (IPSCurrentPayment > 0)
																								{
																									if (isSuccess = cashRegister.CreateARInvoiceStandalone(null,
																										   SapCardCode, //2
																										   Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																										   IPSCurrentPayment, //4
																										   paymentType,
																										   paymentTerm.ToString(), //6
																										  ProjectCode,
																										   Block, //8
																										   Lot,
																										   HouseModel.ToString(), //10
																										   FinancingScheme,
																										   ProdType, //12
																										   ReservationFree,
																										   DPAmount, //14
																										   LBAmount,
																										   "IPS", //16
																										   txtDocNum.Text,
																											ConfigSettings.IPSControlAccount, //18
																										   Vatable,
																										   IPS,
																										   partialPaymentTag,
																										   "N",
																									  Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																										   out IPSEntry,
																										   out Message)) //20
																									{
																										//hana.Execute($@"UPDATE ""QUT1"" SET ""Penalty"" = '{SurCharges}' WHERE ""DocEntry"" = '{DocEntry}' AND ""Terms"" = '{Terms}' AND ""PaymentType"" = '{paymentdue_type}';", hana.GetConnection("SAOHana"));

																										//DataTable dt = ws.IsPaymentForSpecificTransactionExists(IPSEntry, 13).Tables[0];
																										//if (double.Parse(DataAccess.GetData(dt, 0, "DocTotal", "0").ToString()) >= (double.Parse(_row.Cells[6].Text) + double.Parse(_row.Cells[7].Text) + double.Parse(_row.Cells[8].Text) + double.Parse(_row.Cells[9].Text)))
																										//{
																										//    SurchargeAREntry = 0;
																										//}

																										if (ARResIPS >= double.Parse(_row.Cells[9].Text))
																										{
																											IPSEntry = 0;
																										}



																										//else
																										//{
																										qryForUpdates += $@" UPDATE ""QUT1"" SET ""IPS"" = '{IPS}',""AmountPaid"" = '{double.Parse(payment) + double.Parse(amountpaid)}' WHERE ""DocEntry"" = '{DocEntry}' AND ""Terms"" = '{Terms}' AND ""PaymentType"" = '{paymentdue_type}'  AND ""LineStatus"" <> 'R'; ";
																										//}
																										InvoiceEntries = $"{InvoiceEntries}{IPSEntry};";

																									}
																								}
																							}




																							double amountPaid0 = Convert.ToDouble((_row.Cells[12].Text == "&nbsp;" ? "0" : _row.Cells[12].Text));
																							double ARReserveInvoiceAmount = 0;


																							if (amountPaid0 < ((Interest + SurCharges + IPS)))
																							{
																								ARReserveInvoiceAmount = (currentPayment0 + amountPaid0) - (Interest + SurCharges + IPS);
																							}
																							else if (amountPaid0 >= ((Interest + SurCharges + IPS)))
																							{
																								ARReserveInvoiceAmount = currentPayment0;
																							}


																							if (!string.IsNullOrWhiteSpace(InvoiceEntries.Replace("0;", "")))
																							{
																								qryForUpdates += $@" UPDATE ""QUT1"" SET ""SapDocEntry"" = '{ARReserveEntry}',
                                                                ""AmountPaid"" = '{double.Parse(payment) + double.Parse(amountpaid)}'{(double.Parse(_row.Cells[14].Text) == 0 ? @",""LineStatus"" = 'C'" : "")} 
                                                                WHERE ""DocEntry"" = '{DocEntry}' AND ""Terms"" = '{Terms}' AND ""PaymentType"" = '{paymentdue_type}'  
                                                                AND ""LineStatus"" <> 'R'; ";

																								double qut13Amount = GetQUT13Amount(double.Parse(payment), double.Parse(_row.Cells[6].Text), double.Parse(_row.Cells[7].Text), double.Parse(_row.Cells[8].Text),
																									double.Parse(_row.Cells[9].Text), double.Parse(_row.Cells[12].Text), SurchargeCurrentPaymentForQut13, InterestCurrentPaymentForQut13);

																								//2023-10-11 : IF QUT13AMOUNT IS 0, THEN SKIP INSERTING TO QUT13
																								if (qut13Amount > 0)
																								{
																									qryForUpdates += $@" INSERT INTO QUT13 VALUES ({DocEntry}, {QUT1ID}, '{receiptNo}', 0, '{paymentdue_type}', {Terms}, {qut13Amount}, 0, 0, '{Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd")}', 
                                                                                                '{Location}', {Session["UserID"].ToString()}, '{DateTime.Now.ToString("yyyyMMdd")}'); ";
																								}
																							}
																						}
																					}






























































																				}
																				else
																				{
																					alertMsg(Message, "warning");
																					break;
																				}

																			}


																		}






																	}
																}





















																//@@@@@@@@@@@@@@@@@@@@@@@@       POSTING OF INCOMING PAYMENT  @@@@@@@@@@@@@@@@@@@@@@@@
																//@@@@@@@@@@@@@@@@@@@@@@@@       POSTING OF INCOMING PAYMENT  @@@@@@@@@@@@@@@@@@@@@@@@
																//@@@@@@@@@@@@@@@@@@@@@@@@       POSTING OF INCOMING PAYMENT  @@@@@@@@@@@@@@@@@@@@@@@@
																var incomingPaymentStartsHere = "";
																int PaymentEntry = 0;
																if (isSuccess)
																{

																	int pass = 0;

																	//AR DP INVOICE
																	foreach (var docEntry in DPEntries.Split(';'))
																	{
																		if (docEntry != "")
																		{
																			if (docEntry != "0")
																			{
																				pass++;
																			}
																		}
																	}
																	//AR INVOICE
																	foreach (var docEntry in InvoiceEntries.Split(';'))
																	{
																		if (docEntry != "")
																		{
																			if (docEntry != "0")
																			{
																				pass++;
																			}
																		}
																	}


																	//CHECK IF PAYMENTS ARE ALREADY CREATED BUT 
																	if (pass > 0)
																	{
																		if (isSuccess = SAPPaymentPosting(cashRegister,
																								SapCardCode, //2
																								DocEntry,
																								DPEntries, //4
																								InvoiceEntries,
																								CashDiscount, //6
																								paymentType,
																								paymentTerm, //8
																								0,
																								txtDocNum.Text, //10
																								0,
																								0, //12
																								Block,
																								Lot, //14   
																								ProjectCode,
																								"N", //16
																								(Convert.ToInt32(ViewState["chkTagMISC"]) >= 1 ? "MISC" : "TCP"),
																								(DataTable)ViewState["InterestAmount"],
																								out PaymentEntry,
																								out Message))
																		{


																			qryForUpdates += $@" UPDATE QUT13 SET ""SAPDocEntry"" = {PaymentEntry} WHERE ""DocEntry"" = {DocEntry} AND ""ReceiptNo"" = '{receiptNo}' AND ""Location"" = '{Location}'; ";


																			//UPDATE AMOUNT IN PDC
																			foreach (GridViewRow row in gvPayments.Rows)
																			{
																				if (row.Cells[1].Text == "Check")
																				{
																					double tempAmount = 0;
																					double checkAmount = Convert.ToDouble(row.Cells[2].Text);
																					string PDCId = row.Cells[38].Text;

																					//GET AMOUNT FROM PDC
																					string qryGetAmountFromPDC = $@"select ""CheckSum"" from qut8 where ""Id"" = {PDCId}";
																					DataTable dtGetAmountFromPDC = hana.GetData(qryGetAmountFromPDC, hana.GetConnection("SAOHana"));
																					double PDCAmount = Convert.ToDouble(DataHelper.DataTableRet(dtGetAmountFromPDC, 0, "CheckSum", "0"));

																					tempAmount = PDCAmount - checkAmount;
																					if (tempAmount < 0)
																					{
																						tempAmount = 0;
																					}


																					string qryUpdatePDCAmount = $@"UPDATE QUT8 SET ""CheckSum"" = {tempAmount} WHERE ""Id"" = {PDCId}";
																					if (isSuccess = hana.Execute(qryUpdatePDCAmount, hana.GetConnection("SAOHana")))
																					{

																					}
																					else
																					{
																						alertMsg("Transactions posted but updating PDC Amount failed. Please contact adminsitrator.", "warning");
																					}
																				}
																			}






																			// @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ //
																			// @@@@@@@@@@@@@@@@@@@@@@@@@@@ JOURNAL ENTRY FOR RES/DP/LB @@@@@@@@@@@@@@@@@@@@@@@@@@@ //
																			// @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ //

																			var test12412411 = Convert.ToInt32(ViewState["chkTagMISC"]);
																			if (Convert.ToInt32(ViewState["chkTagMISC"]) < 1)
																			{

																				//2023-12-04 : EXCLUDE POSTIUNG OF JE2 WHEN AR RESERVE INVOICE IS ALREADY POSTED 
																				DataTable dtCheckARResInvoice2 = ws.IsReserveInvoiceExists(ProjectCode,
																																			Block,
																																			Lot,
																																			SapCardCode).Tables[0];
																				if (!DataHelper.DataTableExist(dtCheckARResInvoice2))
																				{
																					//JOURNAL ENTRY 2 FOR AR 
																					//2023-10-20 : ADD FILTERING; EXCLUDE IF CANCEL TAG = 'Y'
																					//DataTable _dtARPartialPaidJE2 = hana.GetData($@"SELECT A.""TransId"", A.""LocTotal"" FROM	OJDT A WHERE A.""Project"" = '{ProjectCode}' AND
																					//        A.""U_BlockNo"" = '{Block}' AND	A.""U_LotNo"" = '{Lot}' AND A.""Ref3"" = 'JE1' AND
																					//        IFNULL(A.""U_PaymentOrder"",'') = ''", hana.GetConnection("SAPHana"));
																					DataTable _dtARPartialPaidJE2 = hana.GetData($@"SELECT 
                                                                                                                                A.""TransId"", 
                                                                                                                                A.""LocTotal"" 
                                                                                                                            FROM	
                                                                                                                                OJDT  A INNER JOIN
	                                                                                                                            ORCT B ON A.""U_ORNo"" = (CASE WHEN IFNULL(B.""U_ORNo"",'') <> '' THEN B.""U_ORNo"" 
	    							                                                                                                                           WHEN IFNULL(B.""U_ARNo"",'') <> '' THEN B.""U_ARNo"" 
	    							                                                                                                                      ELSE B.""U_PRNo"" END)   AND 
                                                                                                                                          B.""Canceled"" <> 'Y' 
                                                                                                                                WHERE 
                                                                                                                                    A.""Project"" = '{ProjectCode}' AND
                                                                                                                                    A.""U_BlockNo"" = '{Block}' AND	A.""U_LotNo"" = '{Lot}' AND A.""Ref3"" = 'JE1' AND
                                                                                                                                    IFNULL(A.""U_PaymentOrder"",'') = '' AND IFNULL(A.""U_CancelTag"",'') <> 'Y'", hana.GetConnection("SAPHana"));
																					if (_dtARPartialPaidJE2.Rows.Count > 0)
																					{
																						double _ARAmountJE2 = Convert.ToDouble(DataHelper.DataTableRet(_dtARPartialPaidJE2, 0, "LocTotal", "0"));
																						string _TransId = DataHelper.DataTableRet(_dtARPartialPaidJE2, 0, "TransId", "0").ToString();

																						//Reversal of JE1 Receivable Entries
																						//2024-10-03: CHANGE FROM DFC TO APOTHERS
																						//cashRegister.CreateJournalEntry(null, ProjectCode, SapCardCode, _ARAmountJE2, SODocEntry, FinancingScheme, TaxClassification,
																						//		   Block, Lot, DepositFromCustomers, ARClearingAccount, "", "ARDPJE2", txtDocNum.Text, "", "", Vatable, Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"), "RVRS",
																						//		   out JournalEntryNo, out Message, receiptNo, 0, null, 0, _TransId);
																						cashRegister.CreateJournalEntry(null, ProjectCode, SapCardCode, _ARAmountJE2, SODocEntry, FinancingScheme, TaxClassification,
																								   Block, Lot, DepositFromCustomers, ARClearingAccount, "", "ARDPJE2", txtDocNum.Text, "", "", Vatable, Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"), "RVRS",
																								   out JournalEntryNo, out Message, receiptNo, 0, null, 0, _TransId);

																						//Update JE1 UDF to for offset
																						hana.Execute($@"UPDATE OJDT SET ""U_PaymentOrder"" = '{JournalEntryNo}' WHERE ""TransId"" = '{_TransId}'", hana.GetConnection("SAPHana"));
																					}
																				}






																				//2023-11-21 : EXCLUDE POSTIUNG OF JE1 WHEN AR RESERVE INVOICE IS ALREADY POSTED 
																				DataTable dtCheckARResInvoice = ws.IsReserveInvoiceExists(ProjectCode,
																																			Block,
																																			Lot,
																																			SapCardCode).Tables[0];
																				if (!DataHelper.DataTableExist(dtCheckARResInvoice))
																				{
																					//JOURNAL ENTRY 1 FOR AR   
																					DataTable _dtARPartialPaidJE1 = hana.GetData($@"select (a.""DocTotal"" - a.""PaidToDate"") ""Amount"" from ODPI a where a.""Project"" = '{ProjectCode}' and
                                                                                    a.""U_BlockNo"" = '{Block}' and a.""U_LotNo"" = '{Lot}' and	a.""DocTotal"" > a.""PaidToDate"" and 
                                                                                    a.""CANCELED"" <> 'Y' and a.""DocStatus"" = 'O' AND	a.""U_Type"" IN ('RES','DP','LB')  AND IFNULL(""U_ContractStatus"",'Open') IN ('Open', 'Closed')", hana.GetConnection("SAPHana"));

																					if (_dtARPartialPaidJE1.Rows.Count > 0)
																					{

																						double _ARAmountJE1 = Convert.ToDouble(DataHelper.DataTableRet(_dtARPartialPaidJE1, 0, "Amount", "0"));

																						//Reversal of Receivable Entries
																						//2024-10-03: CHANGE FROM DFC TO APOTHERS
																						//cashRegister.CreateJournalEntry(null, ProjectCode, SapCardCode, _ARAmountJE1, SODocEntry, FinancingScheme, TaxClassification,
																						//		   Block, Lot, DepositFromCustomers, ARClearingAccount, "", "ARDPJE1", txtDocNum.Text, "", "", Vatable, Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"), "1RDL",
																						//		   out JournalEntryNo, out Message, receiptNo);

																						//2024-10-08 : JMC : UPDATED ACCOUNT CODES 
																						//cashRegister.CreateJournalEntry(null, ProjectCode, SapCardCode, _ARAmountJE1, SODocEntry, FinancingScheme, TaxClassification,
																						//		   Block, Lot, APOthersAccount, ARClearingAccount, "", "ARDPJE1", txtDocNum.Text, "", "", Vatable, Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"), "1RDL",
																						//		   out JournalEntryNo, out Message, receiptNo);

																						//2024-10-14 : JMC : NEW ACCOUNT CODES (FROM AP & AR CLEARING TO AR CLEARING & DFC)
																						//cashRegister.CreateJournalEntry(null, ProjectCode, SapCardCode, _ARAmountJE1, SODocEntry, FinancingScheme, TaxClassification,
																						//		   Block, Lot, ARClearingAccount, DepositFromCustomers, "", "ARDPJE1", txtDocNum.Text, "", "", Vatable, Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"), "1RDL",
																						//		   out JournalEntryNo, out Message, receiptNo);

																						//2024-10-18 : JMC : DEBIT DFC - CREDIT AR CLEARING SABI NI MS KATE DUN SA ABROWN LARK GC 
																						cashRegister.CreateJournalEntry(null, ProjectCode, SapCardCode, _ARAmountJE1, SODocEntry, FinancingScheme, TaxClassification,
																								   Block, Lot, DepositFromCustomers, ARClearingAccount, "", "ARDPJE1", txtDocNum.Text, "", "", Vatable, Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"), "1RDL",
																								   out JournalEntryNo, out Message, receiptNo);
																					}
																				}


																			}



																			else
																			{


																				// @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ //
																				// @@@@@@@@@@@@@@@@@@@@@@@@@@@@@ JOURNAL ENTRY FOR MISC @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ //
																				// @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ //

																				//JOURNAL ENTRY 2 FOR AR 
																				DataTable _dtARPartialPaidJE4 = hana.GetData($@"SELECT 
                                                                                                                                A.""TransId"", 
                                                                                                                                A.""LocTotal"" 
                                                                                                                            FROM	
                                                                                                                                OJDT A INNER JOIN
	                                                                                                                            ORCT B ON A.""U_ORNo"" = (CASE WHEN IFNULL(B.""U_ORNo"",'') <> '' THEN B.""U_ORNo"" 
	    							                                                                                                                             WHEN IFNULL(B.""U_ARNo"",'') <> '' THEN B.""U_ARNo"" 
	    							                                                                                                                            ELSE B.""U_PRNo"" END)   AND B.""Canceled"" <> 'Y'
                                                                                                                            WHERE 
                                                                                                                                A.""Project"" = '{ProjectCode}' AND
                                                                                                                                A.""U_BlockNo"" = '{Block}' AND	
                                                                                                                                A.""U_LotNo"" = '{Lot}' AND 
                                                                                                                                A.""Ref3"" = 'JE3' AND
                                                                                                                            IFNULL(A.""U_PaymentOrder"",'') = ''", hana.GetConnection("SAPHana"));

																				if (_dtARPartialPaidJE4.Rows.Count > 0)
																				{
																					double _ARAmountJE4 = Convert.ToDouble(DataHelper.DataTableRet(_dtARPartialPaidJE4, 0, "LocTotal", "0"));
																					string _TransId = DataHelper.DataTableRet(_dtARPartialPaidJE4, 0, "TransId", "0").ToString();

																					//Reversal of JE1 Receivable Entries
																					cashRegister.CreateJournalEntry(null, ProjectCode, SapCardCode, _ARAmountJE4, SODocEntry, FinancingScheme, TaxClassification,
																							   Block, Lot, IPSControlAccount, RetitlingPayableLotAccount, "", "ARINVJE4", txtDocNum.Text, "", "", Vatable, Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"), "RVMS",
																							   out JournalEntryNo, out Message, receiptNo);

																					//Update JE1 UDF to for offset
																					hana.Execute($@"UPDATE OJDT SET ""U_PaymentOrder"" = '{JournalEntryNo}' WHERE ""TransId"" = '{_TransId}'", hana.GetConnection("SAPHana"));
																				}





																				//JOURNAL ENTRY 1 FOR AR   
																				DataTable _dtARPartialPaidJE3 = hana.GetData($@"select (a.""DocTotal"" - a.""PaidToDate"") ""Amount"" from OINV a where a.""Project"" = '{ProjectCode}' and
                                                                                    a.""U_BlockNo"" = '{Block}' and a.""U_LotNo"" = '{Lot}' and	a.""DocTotal"" > a.""PaidToDate"" and
                                                                                    a.""CANCELED"" <> 'Y' and a.""DocStatus"" = 'O' AND	a.""U_Type"" IN ('MISC')  AND IFNULL(""U_ContractStatus"",'Open') IN ('Open', 'Closed')", hana.GetConnection("SAPHana"));


																				if (_dtARPartialPaidJE3.Rows.Count > 0)
																				{
																					double _ARAmountJE3 = Convert.ToDouble(DataHelper.DataTableRet(_dtARPartialPaidJE3, 0, "Amount", "0"));

																					//Reversal of Receivable Entries
																					cashRegister.CreateJournalEntry(null, ProjectCode, SapCardCode, _ARAmountJE3, SODocEntry, FinancingScheme, TaxClassification,
																							   Block, Lot, RetitlingPayableLotAccount, IPSControlAccount, "", "ARINVJE3", txtDocNum.Text, "", "", Vatable, Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"), "1MSC",
																							   out JournalEntryNo, out Message, receiptNo);
																				}
																			}






																			string receiptType = "OR";
																			//UPDATE AUTOKEY FOR OR #
																			if (!string.IsNullOrWhiteSpace(txtOR.Text))
																			{
																				receiptType = "OR";
																			}
																			//UPDATE AUTOKEY FOR PR #
																			else if (!string.IsNullOrWhiteSpace(txtPR.Text))
																			{
																				receiptType = "PR";
																			}
																			//UPDATE AUTOKEY FOR AR #
																			else if (!string.IsNullOrWhiteSpace(txtAR.Text))
																			{
																				receiptType = "AR";
																			}


																			//2023-08-02 : ADD CONDITION WHEN RECEIPT NUMBER IS LESS THAN THE EXISTING RECEIPT NUMBER
																			int receiptNumber = 0;
																			string qryGetORNumber = $@"SELECT	A.""U_Number"" FROM ""@RCPTNUMBERING"" A INNER JOIN ""OPRJ"" B ON A.""U_Location"" = B.""U_Location"" 
                                                                                               WHERE B.""PrjCode"" = '{txtProj.Text}' AND A.""U_RType"" = '{receiptType}' AND ""U_Water"" <> 'Y'  ";
																			DataTable dtGetORNumber = hana.GetData(qryGetORNumber, hana.GetConnection("SAPHana"));
																			if (dtGetORNumber != null)
																			{
																				receiptNumber = Convert.ToInt32(DataHelper.DataTableRet(dtGetORNumber, 0, "U_Number", "0"));
																			}

																			if (int.Parse(receiptNo.Remove(0, 3)) > receiptNumber)
																			{
																				//hana.Execute($@"UPDATE A SET A.""U_Number"" = LPAD(A.""U_Number"" + 1, 5, 00000)  FROM ""@RCPTNUMBERING"" A INNER JOIN ""OPRJ"" B 
																				//                                ON A.""U_Location"" = B.""U_Location"" WHERE  B.""PrjCode"" = '{txtProj.Text}' AND A.""U_RType"" = '{receiptType}'  ", hana.GetConnection("SAPHana"));
																				hana.Execute($@"UPDATE A SET A.""U_Number"" = LPAD('{receiptNo.Remove(0, 3)}', 6, 000000)  FROM ""@RCPTNUMBERING"" A INNER JOIN ""OPRJ"" B 
                                                                        ON A.""U_Location"" = B.""U_Location"" WHERE  B.""PrjCode"" = '{txtProj.Text}' AND A.""U_RType"" = '{receiptType}' AND ""U_Water"" <> 'Y' ", hana.GetConnection("SAPHana"));

																			}




																		}
																		else
																		{
																			alertMsg(Message, "warning");
																		}
																	}


																}

																//@@@@@@@@@@@@@@@@@@@@@@@@       POSTING OF INCOMING PAYMENT  @@@@@@@@@@@@@@@@@@@@@@@@
																//@@@@@@@@@@@@@@@@@@@@@@@@       POSTING OF INCOMING PAYMENT  @@@@@@@@@@@@@@@@@@@@@@@@
																//@@@@@@@@@@@@@@@@@@@@@@@@       POSTING OF INCOMING PAYMENT  @@@@@@@@@@@@@@@@@@@@@@@@


















































																if (isSuccess)
																{


																	//CHECK IF PAYMENT IS MISCELLANEOUS
																	if (Convert.ToInt16(ViewState["chkTagMISC"]) == 0)
																	{

																		//############## CREATION OF AR RESERVE INVOICE ##############
																		//############## CREATION OF AR RESERVE INVOICE ##############
																		//############## CREATION OF AR RESERVE INVOICE ##############


																		//DataTable _dtTotalPaid1 = hana.GetData($@"SELECT 
																		//                              SUM(IFNULL(""AmountPaid"",0)) ""TotalAmountPaid"" ,
																		//                              SUM(CASE 
																		//                              WHEN ((IFNULL(""Penalty"",0) + IFNULL(""InterestAmount"",0) + IFNULL(""IPS"",0)) - IFNULL(""AmountPaid"",0)) < 0
																		//                               THEN   (IFNULL(""Penalty"",0) + IFNULL(""InterestAmount"",0) + IFNULL(""IPS"",0))
																		//                              ELSE
																		//                               CASE 
																		//                                WHEN IFNULL(""AmountPaid"",0) = 0
																		//                                 THEN 0
																		//                                ELSE 
																		//                                 IFNULL(""AmountPaid"",0)
																		//                               END

																		//                             END) ""ExcludedAmount""        
																		//                            from 
																		//                                QUT1 
																		//                            where 
																		//                                ""DocEntry"" = {DocEntry} AND 
																		//                                ""PaymentType"" <> 'MISC' AND 
																		//                                ""LineStatus"" <> 'R' AND 
																		//                                IFNULL(""Cancelled"",'N') <> 'Y'", hana.GetConnection("SAOHana"));

																		//double _TotalAmoundPaid1 = Convert.ToDouble(DataHelper.DataTableRet(_dtTotalPaid1, 0, "TotalAmountPaid", "0"));
																		//_TotalAmoundPaid1 += Convert.ToDouble(lblAmountDue.Text);

																		//double _ExcludedAmount = Convert.ToDouble(DataHelper.DataTableRet(_dtTotalPaid1, 0, "ExcludedAmount", "0"));
																		//_ExcludedAmount += ExclusionAmountForBooking;


																		//double _TotalAmountPaidLessExcludedAmount = 0;


																		//if (_ExcludedAmount >= _TotalAmoundPaid1)
																		//{
																		//    _TotalAmountPaidLessExcludedAmount = 0;
																		//}
																		//else
																		//{
																		//    _TotalAmountPaidLessExcludedAmount = _TotalAmoundPaid1 - _ExcludedAmount;
																		//}

																		//2023-08-29 : CHANGED FETCHING FROM QUERY TO STORED PROC
																		//DataTable _dtTotalPaid1 = hana.GetData($@" CALL USRSP_DBTI_IC_Ledger_SAO ({DocEntry}) ", hana.GetConnection("SAPHana"));
																		DataTable _dtTotalPaid1 = hana.GetData($@"CALL USRSP_DBTI_IC_Ledger_SAO ({DocEntry}, '{Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd")}')", hana.GetConnection("SAPHana"));
																		double _TCPPayment = Convert.ToDouble(DataHelper.DataTableRet(_dtTotalPaid1, 0, "NEG_PRINCIPAL", "0"));
																		double _TCPPaymentPercent = Convert.ToDouble(DataHelper.DataTableRet(_dtTotalPaid1, 0, "TotalTCP", "0"));



																		double _netTCP = Convert.ToDouble(DataHelper.DataTableRet(generalData, 0, "NetTCP", "0"));
																		var _ARReserveInvoiceCondition1 = ConfigSettings.ARReserveInvoiceCondition;



																		DataTable _dtLatestLTSNo = hana.GetData($@"SELECT ""U_LTSNo"" FROM OBTN 
                                                           where ""U_BlockNo"" = '{Block}' and ""U_LotNo"" = '{Lot}' and ""U_Project"" = '{ProjectCode}'", hana.GetConnection("SAPHana"));
																		string _LatestLTSNo = DataHelper.DataTableRet(_dtLatestLTSNo, 0, "U_LTSNo", "");
																		int _ARReserveEntry = 0;



																		//if ((double.Parse(lblTotalPayment.Text) >= (_netTCP * Convert.ToDouble(_ARReserveInvoiceCondition1))) && !string.IsNullOrWhiteSpace(_LatestLTSNo))
																		//CHECK IF TOTAL PAID AMOUNT IS GREATER THAN OR EQUAL 25%
																		//if ((_TCPPayment >= (double.Parse(lblNETTCP.Text) * Convert.ToDouble(_ARReserveInvoiceCondition1))) && !string.IsNullOrWhiteSpace(_LatestLTSNo))

																		//2023-10-05 : ADD CURRENT PAYMENT
																		double currentPayment = Convert.ToDouble(ViewState["TCPTotalPayment"]);
																		if ((Math.Round(_TCPPayment, 2) >= (Math.Round(double.Parse(lblNETTCP.Text), 2) * Convert.ToDouble(_ARReserveInvoiceCondition1))) && !string.IsNullOrWhiteSpace(_LatestLTSNo))
																		{


																			//  string qryGetSurchargePayments = $@"SELECT IFNULL(SUM(A.""DocTotal""),0) ""DocTotal"" FROM OINV A WHERE A.""Project"" = '{txtProj.Text}' AND
																			//A.""U_BlockNo"" = '{txtBlock.Text}' AND A.""U_LotNo"" = '{txtLot.Text}' AND A.""CANCELED"" <> 'Y'  AND 
																			// A.""DocStatus"" = 'C' AND A.""U_TransactionType"" IN ('RE - SUR','INT INC', 'IP&S')  AND IFNULL(""U_ContractStatus"",'Open') IN ('Open', 'Closed')";
																			//  DataTable dtGetSurchargePayments = hana.GetData(qryGetSurchargePayments, hana.GetConnection("SAPHana"));
																			//  double GetSurchargePayments = double.Parse(DataAccess.GetData(dtGetSurchargePayments, 0, "DocTotal", "0").ToString());





																			double GetSurchargePayments = 0;
																			//2023-08-29 : CHANGED FETCHING FROM QUERY TO STORED PROC 
																			//foreach (GridViewRow row in gvDownPayments.Rows)
																			//{
																			//    string LineStatus = row.Cells[16].Text;
																			//    if (LineStatus == "O")
																			//    {
																			//        CheckBox chkSel = (CheckBox)gvDownPayments.Rows[row.RowIndex].FindControl("chkSel");

																			//        //string qryGetSurchargePayments = $@"SELECT IFNULL(SUM(A.""DocTotal""),0) ""DocTotal"" FROM OINV A WHERE A.""Project"" = '{txtProj.Text}' AND
																			//        //A.""U_BlockNo"" = '{txtBlock.Text}' AND A.""U_LotNo"" = '{txtLot.Text}' AND 
																			//        //A.""U_TransactionType"" IN ('RE - SUR','INT INC', 'IP&S') AND A.""U_Type"" = '{row.Cells[3].Text}' AND 
																			//        //A.""U_PaymentOrder"" = '{row.Cells[2].Text}' AND	A.""CANCELED"" <> 'Y' AND A.""DocStatus"" = 'C'  AND IFNULL(A.""U_ContractStatus"",'Open') IN ('Open', 'Closed')";
																			//        string qryGetSurchargePayments = $@"CALL USRSP_DBTI_IC_Ledger_SAO ({DocEntry}, '{Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd")}')";

																			//        DataTable dtGetSurchargePayments = hana.GetData(qryGetSurchargePayments, hana.GetConnection("SAPHana"));
																			//        GetSurchargePayments += double.Parse(DataAccess.GetData(dtGetSurchargePayments, 0, "DocTotal", "0").ToString());

																			//    }
																			//}


																			string qryGetSurchargePayments = $@"SELECT SUM(ifnull(T1.""SumApplied"",0)) SUM  
                                                                                                    FROM ""ORCT""  T0  INNER JOIN ""RCT2""  T1 ON T0.""DocEntry"" = T1.""DocNum"" inner join	
                                                                                                    ""OINV"" b on T1.""DocEntry"" = b.""DocEntry"" and T1.""InvType"" = 13 AND 
                                                                                                    b.""isIns"" = 'N' AND b.""U_TransactionType"" IN ('RE - SUR','INT INC', 'IP&S')  AND
                                                                                                    b.""CANCELED"" = 'N' AND b.""DocStatus"" = 'C'  AND 
                                                                                                    IFNULL(b.""U_ContractStatus"",'Open') IN ('Open', 'Closed') 
                                                                                                    WHERE IFNULL(t0.""U_ORNo"",'') = '{receiptNo}'   
                                                                                                  ";
																			DataTable dtGetSurchargePayments = hana.GetData(qryGetSurchargePayments, hana.GetConnection("SAPHana"));
																			GetSurchargePayments = double.Parse(DataAccess.GetData(dtGetSurchargePayments, 0, "SUM", "0").ToString());



																			//double currentPayment = 0;
																			//foreach (GridViewRow row in gvPayments.Rows)
																			//{
																			//    currentPayment += double.Parse(row.Cells[2].Text);
																			//}



																			var test1123131 = ViewState["Terms"].ToString();
																			var test112313145345345 = ViewState["PaymentType"].ToString();

																			double DiscountAmount = double.Parse(DataHelper.DataTableRet(generalData, 0, "DiscountAmount", "0"));
																			//string Vatable = DataHelper.DataTableRet(generalData, 0, "Vatable", "No");

																			if (isSuccess = cashRegister.CreateReserveInvoice(null,
																										SapCardCode, //2
																										Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"),
																										ProjectCode, //4
																										Block,
																										Lot, //6
																										0,
																										0, //8
																										SODocEntry,
																										double.Parse(lblNETTCP.Text), //10
																										FinancingScheme,
																										TaxClassification, //12
																										PaymentScheme,
																										//_TotalAmountPaidLessExcludedAmount, //14 
																										double.Parse(lblTotalPayment.Text), //14 
																																			//blockingTotalCurrentPayment - GetSurchargePayments,  //REMOVED 20230426
																										blockingTotalCurrentPayment - GetSurchargePayments, //2023-05-23 : BINALIK YUNG MINUS SURCHARGE PAYMENTS BUT WITH CORRECT QUERY
																										0, //16
																										System.Drawing.ColorTranslator.FromHtml("#ffffff"),
																										txtDocNum.Text, //18
																										Vatable,
																										ViewState["Terms"].ToString(), //20
																										ViewState["PaymentType"].ToString(),
																										Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"), //22
																										DiscountAmount,
																										(blockingTotalCurrentPayment) - GetSurchargePayments, //24

																										//2023-07-31: ADD RECEIPT NUMBER
																										receiptNo,


																										//2023-08-07 : ADD TAGGING OF RESTRUCTURING TYPE
																										0,
																										out _ARReserveEntry,
																										out Message)) //26
																			{
																				DataTable dtRestructureAction = hana.GetData($@"select ""DocEntry"" from oinv where ""DocEntry"" = {_ARReserveEntry} and ""DocTotal"" <= ""PaidToDate""  AND IFNULL(""U_ContractStatus"",'Open') IN ('Open', 'Closed')", hana.GetConnection("SAPHana"));
																				if (dtRestructureAction.Rows.Count > 0)
																				{
																					_ARReserveEntry = 0;
																				}

																				//2023-08-14 : ADDED CONDITION FOR EXISTING AR RESERVE INVOICE TO PREVENT FROM UPDATING BOOKDATE
																				if (!Message.Contains("Existing"))
																				{
																					//qryForUpdates += $@" UPDATE ""QUT1"" SET ""SapDocEntry"" = '{_ARReserveEntry}', ""AmountPaid"" = '{double.Parse(payment) + double.Parse(amountpaid)}'{(double.Parse(_row.Cells[14].Text) == 0 ? @",""LineStatus"" = 'C'" : "")} WHERE ""DocEntry"" = '{DocEntry}' AND ""Terms"" = '{Terms}' AND ""PaymentType"" = '{paymentdue_type}'  AND ""LineStatus"" <> 'R';";
																					qryForUpdates += $@" UPDATE ""OQUT"" SET ""BookStatus"" = 'Y', ""BookDate"" = '{Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd")}' WHERE ""DocEntry"" = '{DocEntry}' ; ";

																				}



																				string oSAPDB = ConfigurationManager.AppSettings["HANADatabase"];
																				//2023-11-14 : TRIGGER TO POST COMMISSION JOURNAL ENTRY REVERSAL WHEN 25% IS OBTAINED
																				//2023-11-14 : ADDED NEW CONDITION -- POST JE ONLY IF ALL COMMISSIONS ARE ALREADY POSTED
																				DataTable dtCountCommission = ws.GetCommissionPerDocEntry(oSAPDB, DocEntry.ToString()).Tables["GetCommissionPerDocEntry"];
																				if (dtCountCommission != null)
																				{
																					//2024-01-29 : CHECK IF NO ROWS FOUND IN TABLE DTCOUNTCOMMISSION
																					if (dtCountCommission.Rows.Count > 0)
																					{
																						//2023-11-13 : IF NO FOR RELEASE ANYMORE, POST JE 
																						if (dtCountCommission.Rows.Count == 0)
																						{

																							//2023-11-13 : CHECK IF JE ALREADY EXISTS
																							string qryCheckIfJEExists = $@"select * from ojdt where ""TransCode"" = 'ComJ' and ""Ref3"" = '{txtDocNum.Text}'";
																							DataTable dtCheckIfJEExists = hana.GetData(qryCheckIfJEExists, hana.GetConnection("SAPHana"));
																							if (dtCheckIfJEExists != null)
																							{

																								//2023-11-13 : CHECK IF JE ALREADY EXISTS
																								if (dtCheckIfJEExists.Rows.Count == 0)
																								{
																									string qryAmount = $@"SELECT
                                                                                                                    SUM(B.""LineTotal"") ""DocTotal""
                                                                                                                  FROM 
                                                                                                                    OPCH A INNER JOIN	
	                                                                                                                PCH1 B ON A.""DocEntry"" = B.""DocEntry""
                                                                                                                  WHERE 
                                                                                                                    ""U_DreamsQuotationNo"" = '{txtDocNum.Text}' AND 
                                                                                                                  ""U_TransactionType"" = 'COMMI' AND ""CANCELED"" = 'N'";
																									DataTable dtTotalAmount = hana.GetData(qryAmount, hana.GetConnection("SAPHana"));
																									double AmountTotal = Convert.ToDouble(DataAccess.GetData(dtTotalAmount, 0, "DocTotal", "").ToString());

																									string qryGetDateAPInvoice = $@"SELECT TOP 1 ""DocDate"" FROM OPCH WHERE ""U_DreamsQuotationNo"" = '{txtDocNum.Text}' AND 
                                                                                                                            ""U_TransactionType"" = 'COMMI' AND ""CANCELED"" = 'N' ORDER BY ""DocEntry"" DESC ";
																									DataTable dtGetDateAPInvoice = hana.GetData(qryGetDateAPInvoice, hana.GetConnection("SAPHana"));
																									string GetDateAPInvoice = DataAccess.GetData(dtGetDateAPInvoice, 0, "DocDate", "").ToString();


																									string CommissionExpenseAccount = ConfigSettings.CommissionExpenseAccount;
																									string PrepaidCommissionAccount = ConfigSettings.PrepaidCommissionAccount;

																									//2023-11-14 : CREATE AUTOMATIC JOURNAL ENTRY  
																									if (cashRegister.CreateJournalEntry(null, ProjectCode, SapCardCode, AmountTotal, SODocEntry, FinancingScheme,
																																	TaxClassification, Block, Lot, CommissionExpenseAccount, PrepaidCommissionAccount, txtDocNum.Text,
																																	"Commission", "", "", "", "Y", Convert.ToDateTime(GetDateAPInvoice).ToString("yyyyMMdd"), "ComJ", out JournalEntryNo, out Message, ""))
																									{

																										string qryUpdate = $@" UPDATE OPCH SET ""U_JENo"" = {JournalEntryNo} WHERE ""U_DreamsQuotationNo"" = '{txtDocNum.Text}' AND ""U_TransactionType"" = 'COMMI' 
                                                                                                                       AND ""CANCELED"" = 'N'";
																										hana.Execute(qryUpdate, hana.GetConnection("SAPHana"));

																										alertMsg($@"Transaction posted successfully!", "success");
																									}
																									else
																									{
																										alertMsg($@"Journal Entry posting for Commission failed (Document: {DocEntry}). -- " + Message, "warning");
																									}

																								}
																							}
																						}
																					}
																				}

																			}

																			else
																			{
																				alertMsg(Message, "warning");
																			}

																		}




																		double _wthRate = 0;
																		//string _qryWthRate = $@"SELECT IFNULL(""Rate"",0) ""Rate"" FROM OWHT WHERE ""U_ATC"" = '{ConfigSettings.ATCCode"]}' AND 
																		//     ""U_SellingPriceFrom"" <= {_netTCP} AND ""U_SellingPriceTo"" >= {_netTCP}";  //2023-05-02 BINAGO TO KASI MALI YUNG NET TCPs

																		string _qryWthRate = $@"SELECT IFNULL(""Rate"",0) ""Rate"" FROM OWHT WHERE ""U_ATC"" = '{ConfigSettings.ATCCode}' AND 
                                                             ""U_SellingPriceFrom"" <= {double.Parse(lblNETTCP.Text)} AND ""U_SellingPriceTo"" >= {double.Parse(lblNETTCP.Text)}";
																		DataTable _dtWthRate = hana.GetData(_qryWthRate, hana.GetConnection("SAPHana"));
																		if (_dtWthRate.Rows.Count > 0)
																		{
																			_wthRate = (double.Parse(DataAccess.GetData(_dtWthRate, 0, "Rate", "0").ToString())) / 100;
																		}


																		//POST 25TCP22 IF FULLY PAID
																		//if (_TCPPayment >= _netTCP) //2023-05-02 BINAGO TO KASI MALI YUNG NET TCPs
																		//2023-10-27 : ADDED CURRENT PAYMENT IN THE CONDITION
																		//if ((_TCPPayment + currentPayment) >= double.Parse(lblNETTCP.Text))
																		if ((_TCPPayment + currentPayment) >= double.Parse(lblNETTCP.Text))
																		//if (double.Parse(lblTotalPayment.Text) >= _netTCP)
																		{
																			qryForUpdates += $@" UPDATE ""OQUT"" SET ""DocStatus"" = 'C' WHERE ""DocEntry"" = '{DocEntry}' ; ";

																			//2023-07-14 : ADD CONDITION -- FOR INSTALLMENT ONLY
																			if (PaymentScheme == "Installment")
																			{
																				//FULL PAYMENT
																				if (TaxClassification.ToUpper() == ConfigSettings.TaxClassification2.ToUpper())
																				{
																					double Amount = 0;
																					if (Vatable.ToUpper() == "Y")
																					{
																						Amount = (double.Parse(lblNETTCP.Text) / 1.12) * _wthRate;
																					}
																					else
																					{
																						Amount = (double.Parse(lblNETTCP.Text)) * _wthRate;
																					}



																					//Full payment - recognition of withholding taxes
																					if (checkLTS())
																					{
																						//2023-05-03 : CHANGED CARDCODE TO SAP CARDCODE FOR CORRECT REMARKS
																						//cashRegister.CreateJournalEntry(null, ProjectCode, CardCode, Amount, SODocEntry, FinancingScheme, TaxClassification,
																						//   Block, Lot, CreditableWithholdingTaxAccount, "", "", "25TCP22", txtDocNum.Text, "", "", Vatable, Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"), "FPWT",
																						//    out JournalEntryNo, out Message, receiptNo);

																						cashRegister.CreateJournalEntry(null, ProjectCode, SapCardCode, Amount, SODocEntry, FinancingScheme, TaxClassification,
																							   Block, Lot, CreditableWithholdingTaxAccount, "", "", "25TCP22", txtDocNum.Text, ViewState["Terms"].ToString(), ViewState["PaymentType"].ToString(), Vatable, Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd"), "FPWT",
																							   out JournalEntryNo, out Message, receiptNo);
																					}
																				}
																			}
																		}

																	}

																}

















																if (isSuccess)
																{



																	//UPDATE ALL DATA FOR PAYMENTS 

																	if (!string.IsNullOrWhiteSpace(qryForUpdates))
																	{
																		foreach (var qryForUpdatesRow in qryForUpdates.Split(';'))
																		{
																			if (!string.IsNullOrWhiteSpace(qryForUpdatesRow))
																			{

																				if (isSuccess = hana.Execute(qryForUpdatesRow, hana.GetConnection("SAOHana")))
																				{

																				}
																				else
																				{
																					alertMsg("Transactions posted but updating status of addon transactions failed. Please contact adminsitrator.", "warning");
																				}
																			}
																		}
																	}







																	if (isSuccess)
																	{




																		//##################################################################################################################
																		//RESTRUCTURING FOR ADVANCE PAYMENTS -- INTERSEST RECALCULATION
																		//JOSES: 2023-02-22
																		//##################################################################################################################

																		//check if selected option for apply to principal is yes
																		if (lblApplyToPrincipal.Text == "YES")
																		{
																			string QuotationDocEntry = "0";
																			string DPDocEntry = "0";
																			string MiscEntry = "0";
																			Restructuring(PaymentEntry, out Message, out QuotationDocEntry, out DPDocEntry, out MiscEntry);
																			//alertMsg(Message, "warning");
																		}







																		gvPayments.DataSource = null;
																		gvPayments.DataBind();


																		gvDuePayments.DataSource = null;
																		gvDuePayments.DataBind();

																		string ORNo = txtOR.Text;
																		string ARNo = txtAR.Text;
																		string PRNo = txtPR.Text;

																		clearReceiptNo();
																		//loadReceiptNo(5);
																		//loadReceiptNo(5);
																		//loadReceiptNo(5);

																		txtComment.Text = string.Empty;

																		NewComputeTotal(gvPayments);
																		((DataTable)ViewState["dtPayments"]).Rows.Clear();

																		LoadHouseList();
																		ViewState["chkTagMISC"] = 0;
																		ViewState["chkTagDP"] = 0;



																		//CLEARING DATA AFTER ADVANCE PAYMENTS
																		lblApplyToPrincipal.Text = "NO";
																		lblApplyToPrincipal.ForeColor = System.Drawing.Color.Black;
																		divAdvancePaymentscheduleAccordion.Visible = false;

																		ViewState["MaxLBTerm"] = 0;
																		ViewState["MaxLBTermDate"] = "";

																		//2023-10-09 : RESET EXCESS TERMS
																		ViewState["MaxTermExcess"] = 0;
																		ViewState["MaxTypeExcess"] = 0;

																		//2024-04-05 : CHANGED FROM RECEIPT NUMBER TO PAYMENT ENTRY -- MOVED TO HEADER
																		Session["PrintDocEntry"] = PaymentEntry.ToString();

																		if (!string.IsNullOrWhiteSpace(ViewState["ORNo"].ToString()))
																		{
																			//Session["PrintDocEntry"] = PaymentEntry.ToString();
																			//2024-04-05 : CHANGED FROM RECEIPT NUMBER TO PAYMENT ENTRY -- MOVED TO HEADER
																			//Session["PrintDocEntry"] = ViewState["ORNo"].ToString();

																			ViewState["Receipt"] = ViewState["ORNo"].ToString();
																			Session["Title"] = "Cash Register - Payment ";
																			Session["ReportName"] = ConfigSettings.ORForm;
																			Session["ReportPath"] = ConfigurationManager.AppSettings["ReportPathForms"].ToString();
																			Session["ReportType"] = "Receipt";

																			//2023-05-08 : PINA ADD NI MS KATE
																			Session["Location"] = Location;
																			Session["RptConn"] = "SAP";
																			ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openpage", "window.open('ReportViewer.aspx');", true);
																		}
																		else if (!string.IsNullOrWhiteSpace(ViewState["ARNo"].ToString()))
																		{

																			//Session["PrintDocEntry"] = PaymentEntry.ToString();
																			//2024-04-05 : CHANGED FROM RECEIPT NUMBER TO PAYMENT ENTRY -- MOVED TO HEADER
																			//Session["PrintDocEntry"] = ViewState["ARNo"].ToString(); 

																			ViewState["Receipt"] = ViewState["ARNo"].ToString();
																			Session["Title"] = "Cash Register - Payment ";
																			Session["ReportName"] = ConfigSettings.ARForm;
																			Session["ReportPath"] = ConfigurationManager.AppSettings["ReportPathForms"].ToString();
																			Session["ReportType"] = "Receipt";

																			//2023-05-08 : PINA ADD NI MS KATE
																			Session["Location"] = Location;
																			Session["RptConn"] = "SAP";
																			ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openpage", "window.open('ReportViewer.aspx');", true);
																		}
																		else if (!string.IsNullOrWhiteSpace(ViewState["PRNo"].ToString()))
																		{
																			//Session["PrintDocEntry"] = PaymentEntry.ToString();
																			//2024-04-05 : CHANGED FROM RECEIPT NUMBER TO PAYMENT ENTRY -- MOVED TO HEADER
																			//Session["PrintDocEntry"] = ViewState["PRNo"].ToString();

																			ViewState["Receipt"] = ViewState["PRNo"].ToString();
																			Session["Title"] = "Cash Register - Payment ";
																			Session["ReportName"] = ConfigSettings.PRForm;
																			Session["ReportPath"] = ConfigurationManager.AppSettings["ReportPathForms"].ToString();
																			Session["ReportType"] = "Receipt";

																			//2023-05-08 : PINA ADD NI MS KATE
																			Session["Location"] = Location;
																			Session["RptConn"] = "SAP";
																			ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openpage", "window.open('ReportViewer.aspx');", true);
																		}




																		alertMsg($@"Operation completed successfully ({ViewState["Receipt"].ToString()})", "success");
																	}

																	else
																	{
																		restorePayments();
																		alertMsg("No selected transactions.", "warning");
																	}
																}
																else
																{
																	restorePayments();
																	alertMsg(Message, "warning");
																}



															}

														}

													}
													//}
													//else
													//{
													//    alertMsg("Buyer is not approved", "warning");
													//}
												}
												else
												{
													ScriptManager.RegisterStartupScript(this, this.GetType(), "confirmation", "hideConfirm();", true);
													alertMsg("OR/AR/PR No. already exists. Please try again.", "info");
												}
											}
											else
											{
												ScriptManager.RegisterStartupScript(this, this.GetType(), "confirmation", "hideConfirm();", true);
												alertMsg("No receipt number provided. Please try again.", "info");
											}
										}
										else
										{
											alertMsg("Bank Transfer date deviates from permissible range. Please contact administrator. Kindly check bank transfer date. (Adjust Due Date accordingly in Posting Date module)", "info");
										}
									}
									else
									{
										//alertMsg("Date deviates from permissible range. Please contact administrator. Kindly check due date.", "info");
										alertMsg("Check due date deviates from permissible range. Please contact administrator. Kindly check due date. (Adjust Due Date accordingly in Posting Date module)", "info");
									}
								}
								else
								{
									alertMsg("Date deviates from permissible range. Please contact administrator. Kindly check posting date in Posting Period module.", "info");
								}
							}
						}
						else
						{
							alertMsg("A payment with a date later than the current payment date has been detected. Please retry with the appropriate payment date.", "info");
						}
						//}
						//else
						//{

						//}
					}
					else
					{
						alertMsg("No schedule selected for payment.", "info");
					}
				}
			}
			catch (Exception ex)
			{
				restorePayments();
				alertMsg(ex.Message, "error");
			}
		}


		void restorePayments()
		{
			//RESTORE PAYMENTS
			foreach (DataRow row in dtPayments.Rows)
			{
				foreach (GridViewRow row1 in gvPayments.Rows)
				{
					if (row1.Cells[0].Text == row["LineNum"].ToString() &&
						row1.Cells[1].Text == row["Type"].ToString())
					{
						row1.Cells[2].Text = row["Amount"].ToString();
					}
				}
			}
		}


		bool SAPPaymentPosting(CashRegisterService cashRegister,
			string SapCardCode, //2
			int DocEntry,
			string DPEntries, //4
			string InvoiceEntries,
			double CashDiscount, //6
			string paymentType,
			int paymentTerm, //8
			double IPAmount,
			string DocNum, //10
			int DPTaggingPayment,
			double ARReserveInvoiceAmount, //12
			string block,
			string lot,
			string Project,
			string SundryPayment,
			string PaymentType,
			DataTable dtInterestAmount,
			out int PaymentEntry,
			out string Message)
		{









			bool isSuccess = true;
			string AccountNo = "";
			double CashAmount = 0;
			double CheckAmount = 0;
			double CreditAmount = 0;
			double TransferAmount = 0;
			double _OthersAmount = 0;



			//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
			// INCOMING PAYMENT
			//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

			if (cashRegister.CreateIncomingPayment(SapCardCode,
									   Convert.ToDateTime(tDocDate.Value).ToString("yyyy-MM-dd"), //2
									   txtOR.Text,
									   txtComment.Text, //4
									   DocEntry,
									   DPEntries, //6
									   InvoiceEntries,
									   gvPayments, //8
									   tORDate.Text,
									   txtPR.Text, //10
									   txtPRDate.Text,
									   txtAR.Text, //12
									   txtARDate.Text,
									   0, //14
									   CashDiscount,
									   IPAmount, //16
									   DocNum,
									   paymentTerm, //18
									   paymentType,
									   DPTaggingPayment, //20
									   ARReserveInvoiceAmount,
									   Session["UserName"].ToString(), //22
									   block,
									   lot, //24
									   Project,
									   SundryPayment, //26
									   "N",
									   0, //28
									   PaymentType,
									   Convert.ToDateTime(tDocDate.Value).ToString("yyyy-MM-dd"), //30
									   "",
									   txtCardBrandAccountCode.Value, //32
									   dtInterestAmount,
									   0, //34
									   0,

									   //2023-09-21 : ADD SURCHARGE DATE ON POSTING
									   tSurChargeDate.Text,

									   //2023-06-13 : ADD CONTRACTSTATUS -- CONSIDERING ADVANCE PAYMENT / RESTRUCTURING
									   "Open",

									   out PaymentEntry, //36
									   out Message,
									   out AccountNo, //38
									   out CashAmount,
									   out CheckAmount, //40
									   out CreditAmount,
									   out TransferAmount, //42
									   out _OthersAmount
									   ))
			{

				foreach (GridViewRow row in gvPayments.Rows)
				{
					string type = row.Cells[1].Text;
					double amount = double.Parse(row.Cells[2].Text);
					string checkno = row.Cells[3].Text;
					string bank = row.Cells[4].Text;
					string bankname = row.Cells[5].Text;
					string branch = row.Cells[6].Text;
					string checkdate = row.Cells[7].Text;
					string account = row.Cells[8].Text;

					//credit
					string CreditCard = row.Cells[9].Text;
					string CreditAcctCode = row.Cells[10].Text;
					string CreditAcct = row.Cells[11].Text;
					string CreditCardNumber = row.Cells[12].Text;
					string ValidUntil = row.Cells[13].Text;
					string IdNum = row.Cells[14].Text.Trim();
					string TelNum = row.Cells[15].Text.Trim();
					string PymtTypeCode = row.Cells[16].Text;
					string PymtType = row.Cells[17].Text;
					int NumOfPymts = Convert.ToInt32(row.Cells[18].Text == "&nbsp;" ? "0" : row.Cells[18].Text);
					string VoucherNum = row.Cells[19].Text;
					string CardBrandAccount = row.Cells[39].Text;

					//interbranch
					string InterBankDate = row.Cells[26].Text;
					string InterBankBank = row.Cells[27].Text;
					string InterBankGLAcc = row.Cells[28].Text;
					string InterBankAccNo = row.Cells[29].Text;
					string InterBankAmount = row.Cells[30].Text;

					//others 
					string OthersModeOfPaymentCode = row.Cells[31].Text;
					string OthersModeOfPayment = row.Cells[32].Text;
					double OthersAmount = Convert.ToDouble(row.Cells[33].Text);
					string OthersReferenceNo = row.Cells[34].Text;
					string OthersGLAccountCode = row.Cells[35].Text;
					string OthersGLAccountName = row.Cells[36].Text;
					string OthersPaymentDate = row.Cells[37].Text;



					int Id = ws.UpdatePaymentQuotation(DocEntry, paymentType, paymentTerm);

					// ### Verify the payment type and insert to addon db ### //
					try
					{
						#region Insert Into Addon Database


						string ORDate = string.Empty;
						string ARDate = string.Empty;
						string PRDate = string.Empty;


						if (!string.IsNullOrWhiteSpace(tORDate.Text))
						{
							ORDate = tORDate.Text;
						}
						if (!string.IsNullOrWhiteSpace(txtARDate.Text))
						{
							ARDate = txtARDate.Text;
						}
						if (!string.IsNullOrWhiteSpace(txtPRDate.Text))
						{
							PRDate = txtPRDate.Text;
						}

						if (type == "Cash")
						{

							if (CashAmount > 0)
							{

								if (!ws.PostQuotation(
									Id,
									 type,
									 DocEntry,
									 PaymentEntry,
									 //amount,
									 CashAmount,
									 txtOR.Text,
									 txtComment.Text,
									 string.Empty, //8
									"0",
									string.Empty, //10
									string.Empty,
									string.Empty,//12
									string.Empty,
									string.Empty, //14
									string.Empty,
									string.Empty, //16
									string.Empty,
									string.Empty, //18
									string.Empty,
									string.Empty, //20
									string.Empty,
									string.Empty, //22
									0,
									string.Empty, //24
									string.Empty,
									AccountNo, //26
									ORDate,
									txtAR.Text,
									ARDate,
									txtPR.Text,
									PRDate,

									string.Empty,
									string.Empty,
									0,
									string.Empty,
									string.Empty,
									string.Empty,
									string.Empty,
									tDocDate.Value
									 ))
								{
									isSuccess = false;
								}
							}
						}
						else if (type == "Interbank")
						{

							if (TransferAmount > 0)
							{
								if (!ws.PostQuotation(
							   Id,
							   type, //2
							   DocEntry,
							   PaymentEntry, //4
											 //amount,
							   TransferAmount,
							   txtOR.Text, //6
							   txtComment.Text,
							   InterBankDate, //8
							   InterBankGLAcc,
							   string.Empty, //10
							   InterBankBank,
							   string.Empty, //12
							   string.Empty,
							   string.Empty, //14
							   string.Empty,
							   string.Empty, //16
							   string.Empty,
							   string.Empty, //18
							   string.Empty,
							   string.Empty, //20
							   string.Empty,
							   string.Empty, //22
							   0,
							   string.Empty, //24
							   string.Empty,
							   AccountNo, //26
							   ORDate,
							   txtAR.Text,
							   ARDate,
							   txtPR.Text,
							   PRDate,
							   string.Empty,
									string.Empty,
									0,
									string.Empty,
									string.Empty,
									string.Empty,
									string.Empty,
								tDocDate.Value
							   ))
								{
									isSuccess = false;
								}
							}
						}
						else if (type == "Check")
						{
							if (CheckAmount > 0)
							{

								if (!ws.PostQuotation(
									Id,
									type, //2
									DocEntry,
									PaymentEntry, //4
									amount,
									//CheckAmount, //PINACOMMENT NI DHEZA KASI MALI ITO, HINDI DAPAT TOTAL NG ALL PAYMENTS ANG MASE-SAVE SA DREAMS DATABASE
									txtOR.Text, //6
									txtComment.Text,
									checkdate, //8
									checkno,
									bank, //10
									bankname,
									branch, //12
									account,
									string.Empty, //14
								   string.Empty,
								   string.Empty, //16
								   string.Empty,
								   string.Empty, //18
								   string.Empty,
								   string.Empty, //20
								   string.Empty,
								   string.Empty, //22
								   0,
								   string.Empty, //24
								   string.Empty,
								   AccountNo, //26
									ORDate,
									txtAR.Text,
									ARDate,
									txtPR.Text,
									PRDate,

									string.Empty,
									string.Empty,
									0,
									string.Empty,
									string.Empty,
									string.Empty,
									string.Empty,
									tDocDate.Value
									))
								{
									isSuccess = false;
								}
							}
						}
						else if (type == "Credit")
						{
							if (CreditAmount > 0)
							{

								if (!ws.PostQuotation(
									Id,
									type, //2
									DocEntry,
									PaymentEntry, //4
												  //amount,
									CreditAmount,
									txtOR.Text, //6
									txtComment.Text,
									string.Empty, //8
									"0",
									string.Empty, //10
									CardBrandAccount,
									string.Empty,//12
									string.Empty,
									CreditCard, //14
									CreditAcctCode,
									CreditAcct, //16
									CreditCardNumber,
									ValidUntil, //18
									IdNum,
									TelNum, //20
									PymtTypeCode,
									PymtType, //22
									NumOfPymts,
									VoucherNum, //24
									"S",
									AccountNo, //26 
									ORDate,
									txtAR.Text,
									ARDate,
									txtPR.Text,
									PRDate,

									string.Empty,
									string.Empty,
									0,
									string.Empty,
									string.Empty,
									string.Empty,
									string.Empty,
									tDocDate.Value
									))
								{
									isSuccess = false;
								}
							}
						}
						else if (type.Contains("Others"))
						{
							if (_OthersAmount > 0)
							{

								if (!ws.PostQuotation(
								 Id,
								 type, //2
								 DocEntry,
								 PaymentEntry, //4
											   //amount,
								 _OthersAmount,
								 txtOR.Text, //6
								 txtComment.Text,
								 OthersPaymentDate, //8
								 "0",

								 string.Empty, //10
								 string.Empty,
								 string.Empty, //12
								 string.Empty,

								 string.Empty, //14
								 string.Empty,
								 string.Empty, //16
								 string.Empty,
								 string.Empty, //18
								 string.Empty,
								 string.Empty, //20
								 string.Empty,
								 string.Empty, //22
								 0,
								 string.Empty, //24
								 string.Empty,
								 AccountNo, //26
								 ORDate,
								 txtAR.Text,
								 ARDate,
								 txtPR.Text,
								 PRDate,

								 OthersModeOfPaymentCode,
								 OthersModeOfPayment,
								 OthersAmount,
								 OthersReferenceNo,
								 OthersGLAccountCode,
								 OthersGLAccountName,
								 OthersPaymentDate,
								 tDocDate.Value
								 ))
								{
									isSuccess = false;
								}
							}
						}










						#endregion


					}
					catch (Exception ex)
					{
						isSuccess = false;
						break;
					}


				}




			}
			else
			{
				isSuccess = false;
				alertMsg(Message, "warning");
			}

			return isSuccess;

		}


		protected void txtCheckAmount_TextChanged(object sender, EventArgs e)
		{
		}

		protected void branch_ServerClick(object sender, EventArgs e)
		{
			LoadBranch(txtBankCode.Value);

			ScriptManager.RegisterStartupScript(this, this.GetType(), "", "showBranch()", true);
		}
		void LoadBanks(string type)
		{
			if (type == "HouseBanks")
			{
				gvBanks.DataSource = ws.GetHouseBanks();
				gvBanks.DataBind();
			}
			else
			{
				gvBanks.DataSource = ws.GetBanks();
				gvBanks.DataBind();
			}
		}
		void LoadBranch(string BankCode)
		{
			gvBranch.DataSource = ws.GetBranch(BankCode);
			gvBranch.DataBind();
		}
		protected void btnAddCheck_Click(object sender, EventArgs e)
		{
			try
			{
				if (txtCheckDate.Text == "")
				{
					alertMsg("Please choose check date first!", "warning");
					return;
				}

				if (ViewState["ProjCode"] == null)
				{
					alertMsg("Please select project first!", "warning");
					return;
				}
				//POSTING PERIOD CHECKING
				string qryPostingPeriodCheck = $@"SELECT 
	                                                *
                                                FROM
	                                                OFPR
                                                WHERE
	                                                '{Convert.ToDateTime(txtCheckDate.Text).ToString("yyyyMMdd")}' >= ""F_RefDate"" AND
                                                    '{Convert.ToDateTime(txtCheckDate.Text).ToString("yyyyMMdd")}' <= ""T_RefDate""";
				DataTable dtPostingPeriodCheck = hana.GetData(qryPostingPeriodCheck, hana.GetConnection("SAPHana"));

				//BLOCK IF POSTING DATE DOESN'T EXIST IN POSTING PERIODS
				if (DataAccess.Exist(dtPostingPeriodCheck))
				{

					if (ViewState["dtPayments"] == null)
					{
						alertMsg("Session for the selected transaction is not found. Please select another contract or reload the page.", "warning");
					}
					else
					{

						//** check adding blockings **//
						if (string.IsNullOrEmpty(txtCheckAmount.Text) ||
							string.IsNullOrEmpty(txtCheckNo.Value) ||
							string.IsNullOrEmpty(txtCheckBank.Value) ||
							string.IsNullOrEmpty(txtCheckDate.Text)
						 )
						{
							alertMsg("Please complete all required fields", "info");
						}
						else
						{
							//2023-05-03 : CHECK IF CHECK NUMBER ALREADY EXISTS IN PAYMENTS ADDED
							int ctr = 0;
							foreach (GridViewRow row in gvPayments.Rows)
							{
								if (row.Cells[3].Text == txtCheckNo.Value)
								{
									ctr++;
								}
							}


							//2023-05-03 : CHECK IF CHECK NUMBER ALREADY EXISTS IN PAYMENTS ADDED
							if (ctr <= 0)
							{
								if (btnAddCheck.Text == "Add Check")
								{

									int checkNum = int.Parse(txtCheckNo.Value);

									//2023-10-05 : ADJUSTED BLOCKING QUERY (CONSIDER BP AND BANK CODE)
									//DataTable Checkdt = hana.GetData($@"SELECT * FROM ""QUT8"" WHERE ""CheckNum"" = '{checkNum}'", hana.GetConnection("SAOHana"));

									string qryCheck = $@"SELECT * FROM ""QUT8"" A WHERE A.""DocEntry"" IN ( SELECT x.""DocEntry"" 
                                                         FROM OQUT X INNER JOIN OCRD Z ON X.""CardCode"" = Z.""CardCode"" AND 
                                                         z.""CardCode"" = '{lblID.Text}') AND A.""CheckNum"" = '{checkNum}'
                                                         AND A.""BankCode""  = '{txtBankID.Value}' ";
									DataTable Checkdt = hana.GetData(qryCheck, hana.GetConnection("SAOHana"));

									if (Convert.ToDateTime(txtCheckDate.Text) > DateTime.Now)
									{
										if (Checkdt.Rows.Count > 0)
										{
											alertMsg("Check No. already exists (PDC Saving)", "info");
										}
										else
										{

											ConfirmPDC("Check date is later than today. This will be saved as PDC");
											btnConfimPDC.Focus();
										}
									}
									else
									{
										//if (Checkdt.Rows.Count > 0)
										//{
										//    alertMsg("Check No. already exists", "error");
										//}
										//else
										//{
										//add check
										DataTable dt = new DataTable();
										dt = (DataTable)ViewState["dtPayments"];

										DataRow dr = dt.NewRow();
										dr[0] = Convert.ToInt32(dt.Rows.Count + 1);
										dr[1] = "Check";
										dr[2] = SystemClass.ToCurrency(txtCheckAmount.Text);
										dr[3] = txtCheckNo.Value;
										dr[4] = txtBankID.Value;
										dr[5] = txtCheckBank.Value;
										dr[6] = txtCheckBranch.Value;
										dr[7] = txtCheckDate.Text;
										dr[8] = txtAccount.Value;
										dr[22] = txtDepositBankID.Value;
										dr[23] = txtDepositBank.Value;
										dr[24] = txtDepositedBranch.Value;
										dr[25] = txtCheckAccount.Value;

										dt.Rows.Add(dr);
										ViewState["dtPayments"] = dt;
										LoadData(gvPayments, "dtPayments");


										LoadData(gvPayments, "dtPayments");
										NewComputeTotal(gvPayments);
										ClearCheck();
										ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "closeCheck();", true);


										//}



									}


								}
								else if (btnAddCheck.Text == "Use PDC")
								{
									//add check
									DataTable dt = new DataTable();
									dt = (DataTable)ViewState["dtPayments"];

									DataRow dr = dt.NewRow();
									dr[0] = Convert.ToInt32(dt.Rows.Count + 1);
									dr[1] = "Check";
									dr[2] = SystemClass.ToCurrency(txtCheckAmount.Text);
									dr[3] = txtCheckNo.Value;
									dr[4] = txtBankID.Value;
									dr[5] = txtCheckBank.Value;
									dr[6] = txtCheckBranch.Value;
									dr[7] = txtCheckDate.Text;
									dr[8] = txtAccount.Value;
									dr[22] = txtDepositBankID.Value;
									dr[23] = txtDepositBank.Value;
									dr[24] = txtDepositedBranch.Value;
									dr[25] = txtCheckAccount.Value;
									dr[38] = ViewState["PDCId"];


									dt.Rows.Add(dr);
									ViewState["dtPayments"] = dt;
									LoadData(gvPayments, "dtPayments");


									LoadData(gvPayments, "dtPayments");
									NewComputeTotal(gvPayments);
									btnAddCheck.Text = "Add Check";
									ClearCheck();
									ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "closeCheck();", true);
								}
								else
								{
									//update check
									foreach (DataRow dr in ((DataTable)ViewState["dtPayments"]).Rows)
									{
										if (dr[0].ToString() == ViewState["linenum"].ToString())
										{
											//dr[1] = $"{txtBankCode.Value}-{txtCheckNo.Value}";
											dr[1] = "Check";
											dr[2] = SystemClass.ToCurrency(txtCheckAmount.Text);
											dr[3] = txtCheckNo.Value;
											dr[4] = txtBankID.Value;
											dr[5] = txtCheckBank.Value;
											dr[6] = txtCheckBranch.Value;
											dr[7] = txtCheckDate.Text;
											dr[8] = txtAccount.Value;
											dr[22] = txtDepositBankID.Value;
											dr[23] = txtDepositBank.Value;
											dr[24] = txtDepositedBranch.Value;
											dr[25] = txtCheckAccount.Value;
											break;
										}
									}

									btnAddCheck.Text = "Add Check";


									LoadData(gvPayments, "dtPayments");
									NewComputeTotal(gvPayments);
									ClearCheck();
									ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "closeCheck();", true);

								}
							}
							else
							{
								alertMsg("Check number is already used. Please select/use other check.", "info");
							}
						}

					}

					loadApplyToPrincipal(0);
				}
				else
				{
					alertMsg("Date deviates from permissible range. Please contact administrator. (Posting Period)", "info");
				}
			}
			catch (Exception ex)
			{
				alertMsg(ex.Message, "error");
			}
		}

		void ConfirmPDC(string body)
		{


			lblConfirmPDC.Text = body;
			//Show Modal
			ScriptManager.RegisterStartupScript(this, this.GetType(), "ConfirmPDC", "showConfirmPDC();", true);

		}

		protected void btnConfirmPDC_Click(object sender, EventArgs e)
		{
			try
			{
				//if (ViewState["SQDocEntry"] == null)
				//{
				//    alertMsg("Please re-select contract!", "warning");
				//}
				//else
				//{


				if (
					  (Convert.ToDateTime(Convert.ToDateTime(txtCheckDate.Text).ToString("yyyy-MM-dd")) >
				Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) && string.IsNullOrEmpty(txtARPDCNo.Value))
					)
				{
					alertMsg("Please complete all required fields", "warning");
					divARPDCNo.Visible = true;
					ScriptManager.RegisterStartupScript(this, this.GetType(), "ConfirmPDC", "hideConfirmPDC();", true);
				}
				else
				{

					var oDocEntry = ViewState["SQDocEntry"];
					if (oDocEntry == null)
					{
						oDocEntry = "0";
					}
					int DocEntry = int.Parse(oDocEntry.ToString());
					int SapDocEntry = 0;
					int CheckNumPDC = int.Parse(txtCheckNo.Value);
					string CheckDatePDC = Convert.ToDateTime(txtCheckDate.Text).ToString("yyyy-MM-dd");
					double CheckSumPDC = double.Parse(txtCheckAmount.Text);
					string BankCodePDC = txtBankID.Value;
					string BankPDC = txtCheckBank.Value;
					string BranchPDC = txtCheckBranch.Value;
					string AccountPDC = txtAccount.Value;
					string CreateDate = DateTime.Now.ToString("yyyy-MM-dd");
					string DepositBankCode = txtDepositBankID.Value;
					string DepositBank = txtDepositBank.Value;
					string DepositBranch = txtDepositedBranch.Value;
					string ARPDCNo = txtARPDCNo.Value;

					ScriptManager.RegisterStartupScript(this, this.GetType(), "ConfirmPDC", "hideConfirmPDC();", true);

					if (ws.AddPDC(DocEntry,
								SapDocEntry,
								CheckSumPDC,
								CheckDatePDC,
								CheckNumPDC,
								BankCodePDC,
								BankPDC,
								BranchPDC,
								AccountPDC,
								string.Empty,
								string.Empty,
								string.Empty,
								CreateDate,
								DepositBankCode,
								DepositBank,
								DepositBranch,
								ARPDCNo,
								Session["UserID"].ToString(),
								Session["UserFullName"].ToString()
								))
					{
						ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Post-dated check added successfully!);", true);

						//txtAlert.Text = "Post - dated check Added Sucessfuly";
						//ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "showAlertSuccess();", true);
					}
					LoadData(gvPayments, "dtPayments");
					//NewComputeTotal(gvPayments);

					//CLEAR FIELDS
					txtCheckNo.Value = string.Empty;
					ClearHiddenPDCFields();
					txtCheckNo.Focus();

					//ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "closeCheck();", true);
				}





				//}
			}
			catch (Exception ex)
			{
				alertMsg(ex.Message, "warning");
			}
		}
		//NOT USED Hidden Function // - EPI
		protected void btnAddPDC_Click(object sender, EventArgs e)
		{
			//try
			//{
			//    //** check adding blockings **//
			//    if (string.IsNullOrEmpty(txtCheckAmountPDC.Text) ||
			//        string.IsNullOrEmpty(txtCheckNoPDC.Value) ||
			//        string.IsNullOrEmpty(txtCheckBankPDC.Value) ||
			//        string.IsNullOrEmpty(txtCheckDatePDC.Text))
			//    {
			//        alertMsg("Please complete all fields", "warning");
			//    }
			//    else
			//    {
			//        if (btnAddPDC.Text == "Add PDC")
			//        {
			//            int DocEntry = 0;
			//            int SapDocEntry = 0;
			//            int CheckNumPDC = int.Parse(txtCheckNoPDC.Value);
			//            string CheckDatePDC = Convert.ToDateTime(txtCheckDatePDC.Text).ToString("yyyy-MM-dd");
			//            double CheckSumPDC = double.Parse(txtCheckAmountPDC.Text);
			//            string BankCodePDC = txtBankIdPDC.Value;
			//            string BankPDC = txtCheckBankPDC.Value;
			//            string BranchPDC = txtCheckBranchPDC.Value;
			//            string AccountPDC = txtAccountPDC.Value;
			//            string CreateDate = DateTime.Now.ToString("yyyy-MM-dd");
			//            string DepositBankCode = txtDepositBankIdPDC.Value;
			//            string DepositBank = txtDepositBankPDC.Value;
			//            string DepositBranch = txtDepositedBranchPDC.Value;
			//            string ARPDCNo = txtARPDCNo.Value;


			//            //add PDC
			//            if (ws.AddPDC(DocEntry,
			//                SapDocEntry,
			//                CheckSumPDC,
			//                CheckDatePDC,
			//                CheckNumPDC,
			//                BankCodePDC,
			//                BankPDC,
			//                BranchPDC,
			//                AccountPDC,
			//                string.Empty,
			//                string.Empty,
			//                string.Empty,
			//                CreateDate,
			//                DepositBankCode,
			//                DepositBank,
			//                DepositBranch,
			//                ARPDCNo,
			//                Session["UserID"].ToString(),
			//                Session["UserFullName"].ToString()
			//                ))
			//            {
			//                alertMsg("Post-dated check Added Sucessfuly ", "success");
			//                //ScriptManager.RegisterStartupScript(this, this.GetType(), "check", "showCheck();", true);
			//            }
			//        }

			//        else
			//        {
			//            //update PDC check
			//            foreach (DataRow dr in ((DataTable)ViewState["dtPayments"]).Rows)
			//            {
			//                if (dr[0].ToString() == ViewState["linenum"].ToString())
			//                {
			//                    //dr[1] = $"{txtBankCode.Value}-{txtCheckNo.Value}";
			//                    //dr[1] = "Check";
			//                    //dr[2] = SystemClass.ToCurrency(txtCheckAmount.Text);
			//                    //dr[3] = txtCheckNo.Value;
			//                    //dr[4] = txtBankID.Value;
			//                    //dr[5] = txtCheckBank.Value;
			//                    //dr[6] = txtCheckBranch.Value;
			//                    //dr[7] = txtCheckDate.Text;
			//                    //dr[8] = txtAccount.Value;
			//                    //dr[22] = txtDepositBankID.Value;
			//                    //dr[23] = txtDepositBank.Value;
			//                    //dr[24] = txtDepositedBranch.Value;
			//                    //dr[25] = txtCheckAccount.Value;
			//                    //break;
			//                }
			//            }
			//        }
			//        ClearCheck();
			//    }
			//}
			//catch (Exception ex)
			//{
			//    alertMsg(ex.Message, "error");
			//}
		}

		// Hidden Function // - EPI
		protected void btnCheckPDC_Click(object sender, EventArgs e)
		{
			//btnAddPDC.Text = "Add PDC";
			btnAddCheck.Text = "";
			ScriptManager.RegisterStartupScript(this, this.GetType(), "showAddPDC", "showAddPDC()", true);
		}
		// Hidden Function // - EPI
		protected void btnHideAddPDC_Click(object sender, EventArgs e)
		{
			btnAddCheck.Text = "Add Check";
			//btnAddPDC.Text = "";
			ScriptManager.RegisterStartupScript(this, this.GetType(), "closeAddPDC", "closeAddPDC()", true);
		}
		protected void btnListPDC_Click(object sender, EventArgs e)
		{
			LoadPDCList();
			ScriptManager.RegisterStartupScript(this, this.GetType(), "showListPDC", "showListPDC()", true);
		}
		protected void gvSelectPDC_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			gvSelectPDC.PageIndex = e.NewPageIndex;
			LoadPDCList();
			//gvSelectPDC.DataBind();
		}



		void LoadPDCList()
		{
			//2023-10-05 : CHANGE QUERY - SHOULD BE NOT JOINED WITH DOCENTRY SINCE PDS CAN ALSO BE SHOWN ON DIFFERENT CONTRACTS WITH SAME BP
			//string qry = $@"SELECT 
			//                    TO_VARCHAR(TO_DATE(A.""CreateDate""), 'MM/DD/YYYY') ""PostingDate"",  * 
			//                FROM 
			//                    QUT8 A INNER JOIN 
			//                    OQUT B ON A.""DocEntry"" = B.""DocEntry"" 
			//                WHERE 
			//                    B.""CardCode"" = '{lblID.Text}' AND 
			//                    A.""CheckSum"" > 0";
			string qry = $@" SELECT 
	                            TO_VARCHAR(TO_DATE(A.""CreateDate""), 'MM/DD/YYYY') ""PostingDate"",  * 
                            FROM 
	                            ""QUT8"" A  
                            WHERE 
	                            A.""DocEntry"" IN ( SELECT 
							                            x.""DocEntry"" 
					                              FROM 
					  	                            OQUT X INNER JOIN 
					  	                            OCRD Z ON X.""CardCode"" = Z.""CardCode"" AND 
					  	                            z.""CardCode"" = '{lblID.Text}') ";
			DataTable dt = hana.GetData(qry, hana.GetConnection("SAOHana"));

			gvSelectPDC.DataSource = dt;
			gvSelectPDC.DataBind();
			ViewState["PDCList"] = dt;

			if (dt.Rows.Count > 0)
			{
				btnDeletePDC.Visible = true;
			}
			else
			{
				btnDeletePDC.Visible = false;
			}
		}





		protected void btnSelectPDC_Click(object sender, EventArgs e)
		{
			try
			{
				if (ViewState["SQDocEntry"] == null)
				{
					alertMsg("Please select transaction first!", "warning");
					return;
				}
				ClearCheck();

				LinkButton GetID = (LinkButton)sender;
				string Id = GetID.CommandArgument;
				string DocEntry = ViewState["SQDocEntry"].ToString();

				//DataTable dt = hana.GetData($@"SELECT ""CheckNum"", ""DueDate"", ""CheckSum"", ""Bank"", ""BankCode"", ""Branch"", ""AccountNumber"", 
				//            ""DepositBankCode"", ""DepositBank"", ""DepositBranch""  FROM ""QUT8"" WHERE ""Id"" = '{Id}' AND 
				//            ""DocEntry"" = '{DocEntry}'", hana.GetConnection("SAOHana"));
				DataTable dt = hana.GetData($@"SELECT ""Id"",""CheckNum"", ""DueDate"", ""CheckSum"", ""Bank"", ""BankCode"", ""Branch"", ""AccountNumber"", ifnull(""ARPDCNo"",'') ""ARPDCNo""
                            FROM ""QUT8"" WHERE ""Id"" = '{Id}' 
                            -- AND ""DocEntry"" = '{DocEntry}'", hana.GetConnection("SAOHana"));

				if (dt.Rows.Count > 0)
				{
					txtCheckNo.Value = DataAccess.GetData(dt, 0, "CheckNum", "").ToString();
					txtCheckAmount.Text = DataAccess.GetData(dt, 0, "CheckSum", "").ToString();
					txtCheckBank.Value = DataAccess.GetData(dt, 0, "Bank", "").ToString();
					txtBankCode.Value = DataAccess.GetData(dt, 0, "BankCode", "").ToString();
					txtBankID.Value = DataAccess.GetData(dt, 0, "BankCode", "").ToString();
					txtCheckBranch.Value = DataAccess.GetData(dt, 0, "Branch", "").ToString();
					txtCheckDate.Text = Convert.ToDateTime((string)DataAccess.GetData(dt, 0, "DueDate", "")).ToString("yyyy-MM-dd");
					//txtAccount.Value = DataAccess.GetData(dt, 0, "AccountNumber", "").ToString();
					//txtDepositBankID.Value = DataAccess.GetData(dt, 0, "DepositBankCode", "").ToString();
					//txtDepositBank.Value = DataAccess.GetData(dt, 0, "DepositBank", "").ToString();
					//txtCheckAccount.Value = string.Empty;
					//txtDepositedBranch.Value = DataAccess.GetData(dt, 0, "DepositBranch", "").ToString();
					divARPDCNo.Visible = true;
					txtARPDCNo.Value = DataAccess.GetData(dt, 0, "ARPDCNo", "").ToString();
					ViewState["PDCId"] = DataAccess.GetData(dt, 0, "Id", "0").ToString();
				}

				btnAddCheck.Text = "Use PDC";

				ScriptManager.RegisterStartupScript(this, this.GetType(), "hideListPDC", "hideListPDC()", true);
			}
			catch (Exception ex)
			{
				alertMsg(ex.Message, "warning");
			}
		}
		protected void gvSelectPDC_RowCommand(object sender, EventArgs e)
		{

		}
		protected void btnAddOthers_Click(object sender, EventArgs e)
		{
			try
			{
				if (txtOthersPaymentDate.Value == "")
				{
					alertMsg("Please choose payment date first!", "warning");
					return;
				}

				if (ViewState["ProjCode"] == null)
				{
					alertMsg("Please select project first!", "warning");
					return;
				}
				//POSTING PERIOD CHECKING
				string qryPostingPeriodCheck = $@"SELECT 
	                                                *
                                                FROM
	                                                OFPR
                                                WHERE
	                                                '{Convert.ToDateTime(txtOthersPaymentDate.Value).ToString("yyyyMMdd")}' >= ""F_RefDate"" AND
                                                    '{Convert.ToDateTime(txtOthersPaymentDate.Value).ToString("yyyyMMdd")}' <= ""T_RefDate""";
				DataTable dtPostingPeriodCheck = hana.GetData(qryPostingPeriodCheck, hana.GetConnection("SAPHana"));

				//BLOCK IF POSTING DATE DOESN'T EXIST IN POSTING PERIODS
				if (DataAccess.Exist(dtPostingPeriodCheck))
				{


					//** check adding blockings **//
					if (string.IsNullOrEmpty(txtOthersPaymentDate.Value) ||
					string.IsNullOrEmpty(txtOthersModeOfPayment.Value) ||
					string.IsNullOrEmpty(txtOthersAmount.Text) ||
					string.IsNullOrEmpty(txtOthersReferenceNo.Value))
					{
						alertMsg("Please complete all fields", "warning");
					}
					else
					{
						if (btnAddOthers.Text == "Add Payment")
						{
							//add check
							DataTable dt = new DataTable();
							dt = (DataTable)ViewState["dtPayments"];

							DataRow dr = dt.NewRow();
							dr[0] = Convert.ToInt32(dt.Rows.Count + 1);
							dr[1] = "Others - " + txtOthersModeOfPayment.Value;
							dr[2] = SystemClass.ToCurrency(txtOthersAmount.Text);

							dr[31] = txtOthersModeOfPaymentCode.Value;
							dr[32] = txtOthersModeOfPayment.Value;
							dr[33] = txtOthersAmount.Text;
							dr[34] = txtOthersReferenceNo.Value;
							dr[35] = txtOthersGLAccountCode.Value;
							dr[36] = txtOthersGLAccountName.Value;
							dr[37] = txtOthersPaymentDate.Value;


							dt.Rows.Add(dr);
							ViewState["dtPayments"] = dt;
							LoadData(gvPayments, "dtPayments");
						}
						else
						{
							//update check
							foreach (DataRow dr in ((DataTable)ViewState["dtPayments"]).Rows)
							{
								if (dr[0].ToString() == ViewState["linenum"].ToString())
								{
									dr[1] = "Others - " + txtOthersModeOfPayment.Value;
									dr[2] = SystemClass.ToCurrency(txtOthersAmount.Text);

									dr[31] = txtOthersModeOfPaymentCode.Value;
									dr[32] = txtOthersModeOfPayment.Value;
									dr[33] = txtOthersAmount.Text;
									dr[34] = txtOthersReferenceNo.Value;
									dr[35] = txtOthersGLAccountCode.Value;
									dr[36] = txtOthersGLAccountName.Value;
									dr[37] = txtOthersPaymentDate.Value;

									break;
								}
							}

							btnAddOthers.Text = "Add Payment";
						}

						LoadData(gvPayments, "dtPayments");
						NewComputeTotal(gvPayments);
						loadApplyToPrincipal(0);


						ClearOthers();
						ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "hideOthers();", true);
					}
				}
				else
				{
					alertMsg("Date deviates from permissible range. Please contact administrator. (Posting Period)", "info");
				}




			}
			catch (Exception ex)
			{
				alertMsg(ex.Message, "error");
			}
		}


		void ClearCheck()
		{
			txtCheckNo.Value = string.Empty;
			txtCheckAmount.Text = string.Empty;
			txtCheckBank.Value = string.Empty;
			txtBankCode.Value = string.Empty;
			txtCheckBranch.Value = string.Empty;
			txtCheckDate.Text = string.Empty;
			txtAccount.Value = string.Empty;
			txtDepositBankID.Value = string.Empty;
			txtDepositBank.Value = string.Empty;
			txtCheckAccount.Value = string.Empty;
			txtDepositedBranch.Value = string.Empty;
			txtARPDCNo.Value = string.Empty;

			// Hidden Modal fields ///
			//txtCheckNoPDC.Value = string.Empty;
			//txtCheckAmountPDC.Text = string.Empty;
			//txtCheckBankPDC.Value = string.Empty;
			//txtBankCodePDC.Value = string.Empty;
			//txtCheckBranchPDC.Value = string.Empty;
			//txtCheckDatePDC.Text = string.Empty;
			//txtAccountPDC.Value = string.Empty;
			//txtDepositBankIdPDC.Value = string.Empty;
			//txtDepositBankPDC.Value = string.Empty;
			//txtCheckAccountPDC.Value = string.Empty;
			//txtDepositedBranchPDC.Value = string.Empty;

		}

		void ClearHiddenPDCFields()
		{
			// Hidden Modal fields ///
			//txtCheckNoPDC.Value = string.Empty;
			//txtCheckAmountPDC.Text = string.Empty;
			//txtCheckBankPDC.Value = string.Empty;
			//txtBankCodePDC.Value = string.Empty;
			//txtCheckBranchPDC.Value = string.Empty;
			//txtCheckDatePDC.Text = string.Empty;
			//txtAccountPDC.Value = string.Empty;
			//txtDepositBankIdPDC.Value = string.Empty;
			//txtDepositBankPDC.Value = string.Empty;
			//txtCheckAccountPDC.Value = string.Empty;
			//txtDepositedBranchPDC.Value = string.Empty;

		}

		void ClearCredit()
		{
			txtCreditCard.Value = string.Empty;
			txtCreditCardNum.Value = string.Empty;
			txtCreditCardCode.Value = string.Empty;
			txtCreditAccount.Value = string.Empty;
			txtCreditAccountCode.Value = string.Empty;
			txtCreditAmount.Text = string.Empty;
			txtValidUntil.Text = string.Empty;
			txtCreditMethod.Value = string.Empty;
			txtCreditMethodCode.Value = string.Empty;
			txtNoOfPayments.Value = "1";
			txtIDNum.Value = string.Empty;
			txtTelNo.Value = string.Empty;
			txtVoucherNum.Value = string.Empty;
		}

		void ClearInterbranch()
		{
			txtInterBranchDate.Value = string.Empty;
			txtInterBankGLAcc.Value = string.Empty;
			txtInterBankBank.Value = string.Empty;
			txtInterAccounts.Value = string.Empty;
			txtInterAmount.Text = string.Empty;
		}

		void ClearOthers()
		{
			txtOthersAmount.Text = string.Empty;
			txtOthersModeOfPayment.Value = string.Empty;
			txtOthersPaymentDate.Value = string.Empty;
			txtOthersReferenceNo.Value = string.Empty;

		}

		protected void bAddCash_Click(object sender, EventArgs e)
		{
			if (Session["UserID"] == null)
			{
				alertMsg("Session expired!", "error");
				Response.Redirect("~/pages/Login.aspx");
			}

			if (!string.IsNullOrEmpty(txtCashAmount.Text))
			{
				if (bAddCash.Text == "Add")
				{
					//new cash payment
					DataTable dt = new DataTable();
					dt = (DataTable)ViewState["dtPayments"];

					DataRow dr = dt.NewRow();
					dr[0] = Convert.ToInt32(dt.Rows.Count + 1);
					dr[1] = "Cash";
					dr[2] = SystemClass.ToCurrency(txtCashAmount.Text);

					dt.Rows.Add(dr);
					ViewState["dtPayments"] = dt;
				}
				else
				{
					//update
					foreach (DataRow row in ((DataTable)ViewState["dtPayments"]).Rows)
					{
						if (row[0].ToString() == (string)ViewState["linenum"])
						{
							row[2] = double.Parse(txtCashAmount.Text);
						}
					}
				}

				LoadData(gvPayments, "dtPayments");
				NewComputeTotal(gvPayments);
				loadApplyToPrincipal(0);


				//close modal
				ScriptManager.RegisterStartupScript(this, this.GetType(), "", "closeCash();", true);
			}
			else
			{
				alertMsg("Please enter amount", "warning");
			}
		}

		#region old Compute Total
		//void ComputeTotal(GridView gv)
		//{
		//    double payment = 0;
		//    double total = 0;
		//    double totalamtpd = 0;
		//    double postedamt = 0;
		//    double principal = 0;
		//    double loanable = 0;
		//    double dpamount = 0;
		//    double duepayments = 0;
		//    //** CLEAR PAYMENT **//
		//    foreach (GridViewRow row in gvDownPayments.Rows)
		//    {
		//        row.Cells[9].Text = "0.00";
		//    }

		//    foreach (GridViewRow row in gv.Rows)
		//    {
		//        payment += double.Parse(row.Cells[2].Text);
		//    }
		//    lblAmountDue.Text = SystemClass.ToCurrency(payment.ToString());

		//    double totalamount = 0;
		//    //** RESERVATION **//
		//    if ((string)ViewState["Status"] == "CASHIER")
		//    {
		//        foreach (GridViewRow row in gvDuePayments.Rows)
		//        {
		//            totalamount = double.Parse(row.Cells[6].Text);
		//            duepayments = double.Parse(row.Cells[6].Text);
		//            row.Cells[7].Text = SystemClass.ToCurrency(payment.ToString());
		//            row.Cells[8].Text = SystemClass.ToCurrency((totalamount - payment).ToString());
		//        }

		//        lblDue.Text = SystemClass.ToCurrency(duepayments.ToString());
		//        lblAmount.Text = SystemClass.ToCurrency(totalamount.ToString());
		//        txtBalance.Text = SystemClass.ToCurrency((totalamount - payment).ToString());
		//    }
		//    //** DOWNPAYMENT **//
		//    else if ((string)ViewState["Status"] == ConfigSettings.Forwarded"].ToString())
		//    {
		//        //** CLEAR DATA **//
		//        foreach (GridViewRow row in gvDownPayments.Rows)
		//        {
		//            //clear balance
		//            if (double.Parse(row.Cells[7].Text) == 0)
		//            {
		//                row.Cells[7].Text = "0.00";
		//                row.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
		//            }
		//        }

		//        //** compute penalty & loanable **//
		//        double running_bal = 0;
		//        double total_penalties = 0;


		//        double dppenalty = double.Parse(ConfigSettings.DPPenalty"].ToString());
		//        double mapenalty = double.Parse(ConfigSettings.MAPenalty"].ToString());

		//        foreach (GridViewRow row in gvDownPayments.Rows)
		//        {
		//            if (Convert.ToDateTime(row.Cells[3].Text) < DateTime.Now)
		//            {
		//                // ** comp of interest and penalty for Inhouse Only **//
		//                if ((string)ViewState["FinCode"] == "I")
		//                //|| row.Cells[2].Text == "DP"
		//                {
		//                    running_bal += double.Parse(row.Cells[4].Text);
		//                    double penalty = 0;
		//                    //penalty = double.Parse(row.Cells[7].Text);
		//                    if (int.Parse(row.Cells[12].Text) != 1 &&
		//                        double.Parse(row.Cells[7].Text) == 0)
		//                    {

		//                        //if (row.Cells[2].Text != "MA")
		//                        //{
		//                        penalty = row.Cells[2].Text != "MA" ? (running_bal * dppenalty) : (running_bal * mapenalty);
		//                        row.Cells[7].Text = SystemClass.ToCurrency(penalty.ToString());

		//                        //}
		//                        //else
		//                        //{
		//                        //    penalty = (running_bal * mapenalty);
		//                        //    row.Cells[7].Text = SystemClass.ToCurrency(penalty.ToString());
		//                        //}
		//                    }
		//                    else
		//                    {
		//                        //row.Cells[7].Text = SystemClass.ToCurrency((double.Parse(row.Cells[7].Text) - double.Parse(row.Cells[8].Text)).ToString());
		//                        //row.Cells[8].Text = SystemClass.ToCurrency((double.Parse(row.Cells[8].Text) - penalty).ToString());
		//                    }

		//                    //** if due row color = red
		//                    row.Cells[3].ForeColor = System.Drawing.ColorTranslator.FromHtml("#a94442");
		//                    row.Cells[3].Font.Bold = true;
		//                }
		//            }
		//            else
		//            {
		//                row.Cells[7].Text = "0.00";
		//                row.Cells[3].ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
		//            }
		//            //get principal
		//            principal += double.Parse(row.Cells[5].Text);

		//            if (row.Cells[2].Text == "MA")
		//                loanable += double.Parse(row.Cells[5].Text);
		//            else if (row.Cells[2].Text == "DP")
		//                dpamount += double.Parse(row.Cells[5].Text);
		//        }
		//        //total_penalties = running_bal * 0.05;

		//        Session["TotalLoanable"] = loanable;
		//        Session["TotalDP"] = dpamount;
		//        //** end of penalty computation **//

		//        foreach (GridViewRow row in gvDownPayments.Rows)
		//        {

		//            //** total amount + penalty **//
		//            totalamount = double.Parse(SystemClass.ToCurrency(row.Cells[4].Text)) + double.Parse(SystemClass.ToCurrency(row.Cells[7].Text));
		//            postedamt = double.Parse(SystemClass.ToCurrency(row.Cells[8].Text));


		//            if (payment > double.Parse(SystemClass.ToCurrency((totalamount - postedamt).ToString())))
		//            {
		//                //row.Cells[6].Text = SystemClass.ToCurrency(totalamount.ToString());
		//                row.Cells[9].Text = SystemClass.ToCurrency((totalamount - postedamt).ToString());
		//                row.Cells[10].Text = "0.00";
		//                payment -= double.Parse(SystemClass.ToCurrency((totalamount - postedamt).ToString()));//totalamount;
		//                row.BackColor = System.Drawing.ColorTranslator.FromHtml("#f0fff0");
		//            }
		//            else
		//            {
		//                row.Cells[9].Text = SystemClass.ToCurrency(payment.ToString());
		//                row.Cells[10].Text = SystemClass.ToCurrency((totalamount - payment).ToString());

		//                if (double.Parse(SystemClass.ToCurrency(payment.ToString())) > 0)
		//                {
		//                    row.BackColor = System.Drawing.ColorTranslator.FromHtml("#f0fff0");
		//                }
		//                break;
		//            }
		//        }
		//        //compute 
		//        foreach (GridViewRow row in gvDownPayments.Rows)
		//        {
		//            total += double.Parse(row.Cells[4].Text) + double.Parse(row.Cells[7].Text);
		//            totalamount = double.Parse(SystemClass.ToCurrency(row.Cells[4].Text)) + double.Parse(SystemClass.ToCurrency(row.Cells[7].Text));
		//            postedamt = double.Parse(SystemClass.ToCurrency(row.Cells[8].Text));
		//            totalamtpd += double.Parse(SystemClass.ToCurrency(row.Cells[9].Text));

		//            double amtpaid = double.Parse(SystemClass.ToCurrency(row.Cells[9].Text));
		//            row.Cells[10].Text = SystemClass.ToCurrency(((totalamount - postedamt) - amtpaid).ToString());
		//            //check if fully paid
		//            if (double.Parse(SystemClass.ToCurrency((totalamount - postedamt).ToString())) == amtpaid)
		//            {
		//                row.BackColor = System.Drawing.ColorTranslator.FromHtml("#bdffbd");
		//                //row.Cells[6].ForeColor = System.Drawing.ColorTranslator.FromHtml("#5cb85c");
		//            }
		//            else
		//            {
		//                if (amtpaid != 0)
		//                {
		//                    //row.Cells[6].ForeColor = System.Drawing.ColorTranslator.FromHtml("#a94442");
		//                    row.BackColor = System.Drawing.ColorTranslator.FromHtml("#f4a0a0");
		//                }
		//            }
		//        }
		//        //** compute due payments **//

		//        foreach (GridViewRow row in gvDownPayments.Rows)
		//        {
		//            if (Convert.ToDateTime(row.Cells[3].Text) <= DateTime.Now)
		//                duepayments += double.Parse(row.Cells[4].Text) + double.Parse(row.Cells[7].Text);

		//            total_penalties += double.Parse(row.Cells[7].Text);
		//        }
		//        lblDue.Text = SystemClass.ToCurrency(duepayments.ToString());
		//        lblAmount.Text = SystemClass.ToCurrency(total.ToString());
		//        txtBalance.Text = SystemClass.ToCurrency((total - payment).ToString());// SystemClass.ToCurrency(principal.ToString()); //SystemClass.ToCurrency((total - payment).ToString());
		//    }


		//}

		#endregion


		void NewComputeTotal(GridView gv)
		{
			try
			{

				double payment = 0;
				double balance = 0;
				double totalamtpd = 0;
				double postedamt = 0;
				double principal = 0;
				double loanable = 0;
				double dpamount = 0;
				double duepayments = 0;
				double TotalField = 0;
				double cashDiscount = 0;


				//######################################
				//####### START CLEAR FIELDS ###########
				//######################################

				//** CLEAR PAYMENT **//
				foreach (GridViewRow row in gvDownPayments.Rows)
				{


					//ADDED CURRENT PAYMENT
					row.Cells[13].Text = "0.00";
					//DISABLE CHECKBOX WHEN PAID
					string LineStatus = row.Cells[16].Text;
					//2024-10-05 : ADDED "A" FOR ADVANCED PAYMENT STATUS
					//if (LineStatus == "C" || LineStatus == "R")
					//2024-10-08 : REVERT BACK CHANGES
					//if (LineStatus == "C" || LineStatus == "R" || LineStatus == "A")
					if (LineStatus == "C" || LineStatus == "R")
					{
						row.Cells[1].Enabled = false;
					}
					else
					{
						row.Cells[1].Enabled = true;
					}



					//2023-07-06 : MOVED HERE TO REMOVE EXCESS FOREACH
					//clear PENALTY
					if (double.Parse(row.Cells[8].Text) == 0)
					{
						row.Cells[8].Text = "0.00";
					}
					row.Cells[14].Text = "0.00";
					row.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");


				}

				//PAYMENTS CURRENTLY ADDED
				foreach (GridViewRow row in gv.Rows)
				{
					payment += double.Parse(row.Cells[2].Text);
				}
				lblAmountDue.Text = SystemClass.ToCurrency(payment.ToString());




				double totalamount = 0;



				//######################################
				//######### END CLEAR FIELDS ###########
				//######################################













				//######################################
				//####### START COMPUTE SURCHAGE #######
				//######################################


				#region SURCHARGE

				double running_bal = 0;
				double total_penalties = 0;
				if (ViewState["SQDocEntry"] != null)
				{
					//GET ACCOUNT TYPE
					string qry = $@"SELECT ""AcctType"" FROM OQUT WHERE ""DocEntry"" = '{ViewState["SQDocEntry"].ToString()}'";
					DataTable dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
					string AcctType = DataAccess.GetData(dt, 0, "AcctType", "0").ToString();

					//GET SURCHARGE FROM SAP
					qry = $@"SELECT ""U_Surcharge"" FROM ""@PRODUCTTYPE"" WHERE UPPER(""Code"") = UPPER('{AcctType}')";
					dt = hana.GetData(qry, hana.GetConnection("SAPHana"));

					//DOWNPAYMENT PENALTY RATE
					double dppenalty = double.Parse(DataAccess.GetData(dt, 0, "U_Surcharge", "0").ToString());
					double mapenalty = double.Parse(DataAccess.GetData(dt, 0, "U_Surcharge", "0").ToString());

					//double dppenalty = double.Parse(ConfigSettings.DPPenalty"].ToString());
					//double mapenalty = double.Parse(ConfigSettings.MAPenalty"].ToString());



					foreach (GridViewRow row in gvDownPayments.Rows)
					{



						string LineStatus = row.Cells[16].Text;
						if (LineStatus == "O")
						{
							CheckBox chkSel = (CheckBox)gvDownPayments.Rows[row.RowIndex].FindControl("chkSel");


							if (chkSel.Checked)
							{

								if (row.Cells[3].Text == "DP" && row.Cells[2].Text == "1")
								{
									var test = "";
								}




								if (row.Cells[3].Text == "LB" && row.Cells[2].Text == "11")
								{
									var test = "";
								}




								//WAIVED SURCHARGE -- CHECK TAGGING IN "MISC" COLUMN IF IT IS WAIVED
								string qry1 = $@"SELECT ""Misc"",""Penalty"" FROM QUT1 WHERE ""DocEntry"" = {row.Cells[0].Text} AND ""Terms"" = '{row.Cells[2].Text}' AND ""PaymentType"" = '{row.Cells[3].Text}'";
								dt = hana.GetData(qry1, hana.GetConnection("SAOHana"));
								int waiveTag = Convert.ToInt32(DataAccess.GetData(dt, 0, "Misc", "0").ToString());
								if (waiveTag == 1)
								{
									row.Cells[8].Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "Penalty", "0").ToString());
									row.Cells[4].ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
								}
								else
								{


									//IF DUE DATE IS PAST DUE1
									if (Convert.ToDateTime(row.Cells[4].Text) <= Convert.ToDateTime(tSurChargeDate.Text))
									{










										if (!(Convert.ToDateTime(tSurChargeDate.Text).Month - Convert.ToDateTime(row.Cells[4].Text).Month >= 1))
										{
											//IF NOT MISCELLANEOUS 
											if (row.Cells[3].Text != "MISC" && row.Cells[3].Text != "RES")
											{
												//GET RUNNING BALANCE (SUM OF PAYMENTAMOUNT)
												if (double.Parse(row.Cells[6].Text) + double.Parse(row.Cells[7].Text) + double.Parse(row.Cells[9].Text) > 0)
												//if (double.Parse(row.Cells[14].Text) > 0)
												{


													string qryGetSurchargePayments = $@"SELECT IFNULL(SUM(A.""DocTotal""),0) ""DocTotal"" FROM OINV A WHERE A.""Project"" = '{txtProj.Text}' AND
								                                                  A.""U_BlockNo"" = '{txtBlock.Text}' AND A.""U_LotNo"" = '{txtLot.Text}' AND 
                                                                                  A.""U_TransactionType"" = 'RE - SUR' AND A.""U_Type"" = '{row.Cells[3].Text}' AND 
								                                                  A.""U_PaymentOrder"" = '{row.Cells[2].Text}' AND	A.""CANCELED"" = 'N' AND A.""DocStatus"" = 'C'  AND IFNULL(A.""U_ContractStatus"",'Open') IN ('Open', 'Closed')";
													DataTable dtGetSurchargePayments = hana.GetData(qryGetSurchargePayments, hana.GetConnection("SAPHana"));
													double GetSurchargePayments = double.Parse(DataAccess.GetData(dtGetSurchargePayments, 0, "DocTotal", "0").ToString());

													//if (GetSurchargePayments != 0)
													if (double.Parse(row.Cells[12].Text) != 0)
													{
														double interest = double.Parse(row.Cells[7].Text);
														double ips = double.Parse(row.Cells[9].Text);

														if (((interest + ips + GetSurchargePayments) - double.Parse(row.Cells[12].Text) < 0))
														{
															running_bal = double.Parse(row.Cells[6].Text) + ((interest + ips + GetSurchargePayments) - double.Parse(row.Cells[12].Text));
														}
														else
														{
															//running_bal = double.Parse(row.Cells[6].Text);
															//COMMENTED 2022_09_05
															running_bal = double.Parse(row.Cells[6].Text) + ((interest + ips + GetSurchargePayments) - double.Parse(row.Cells[12].Text));
														}

													}
													else
													{
														////GET TOTAL PAID PRINCIPAL
														//string qryGetPrincipalPayments = $@"SELECT IFNULL(SUM(A.""DocTotal""),0) ""DocTotal"" FROM ODPI A WHERE A.""Project"" = '{txtProj.Text}' AND
														//      A.""U_BlockNo"" = '{txtBlock.Text}' AND A.""U_LotNo"" = '{txtLot.Text}' AND 
														//      A.""U_PaymentOrder"" = '{row.Cells[2].Text}' AND	A.""CANCELED"" <> 'Y'  AND A.""DocStatus"" = 'C'";
														//DataTable dtGetPrincipalPayments = hana.GetData(qryGetPrincipalPayments, hana.GetConnection("SAPHana"));
														//double GetPrincipalPayments = double.Parse(DataAccess.GetData(dtGetPrincipalPayments, 0, "DocTotal", "0").ToString());

														//running_bal = double.Parse(row.Cells[6].Text) - GetPrincipalPayments;
														double interest = double.Parse(row.Cells[7].Text);
														running_bal = double.Parse(row.Cells[6].Text) + interest;
													}



													//(PRINCIPAL + INTEREST  + IP&S) - AmountPaid
													//running_bal = (double.Parse(row.Cells[6].Text) + double.Parse(row.Cells[7].Text) + double.Parse(row.Cells[9].Text)) - (double.Parse(row.Cells[12].Text));
												}


												if (chkSel.Checked)
												{



													double penalty = 0;

													//Get Day, Month, and Year of the month
													int year = DateTime.Now.Year;
													int month = DateTime.Now.Month;
													//int monthsdue = Convert.ToDateTime(tSurChargeDate.Text).Month - Convert.ToDateTime(row.Cells[3].Text).Month;
													//int daysdue = Convert.ToDateTime(tSurChargeDate.Text).Day - Convert.ToDateTime(row.Cells[3].Text).Day;
													double monthsdue = 0;
													double daysdue = (Convert.ToDateTime(tSurChargeDate.Text) - Convert.ToDateTime(row.Cells[4].Text)).TotalDays;


													while (daysdue > 30)
													{
														monthsdue += 1;
														daysdue -= 30;
													}
													monthsdue = monthsdue < 1 ? 0 : monthsdue;
													daysdue = daysdue < 1 ? 0 : daysdue;
													int days = 30;
													//int days = DateTime.DaysInMonth(year, month);

													//COMPUTE FOR PENALTY
													//penalty = row.Cells[2].Text != "LB" ? ((running_bal / days) * (dppenalty / 100)) : ((running_bal / days) * (mapenalty / 100));
													DataTable dtSurcharge = ws.ComputeSurcharge(running_bal, dppenalty, monthsdue, daysdue).Tables[0];
													penalty = Convert.ToDouble(DataHelper.DataTableRet(dtSurcharge, 0, "Penalty", "0.00"));
													//penalty = ((running_bal * Math.Pow(1 + (dppenalty / 100), monthsdue)) * (1 + (dppenalty * daysdue / 3000))) - running_bal;

													//IF NOT RESERVATION
													if (row.Cells[2].Text != "RES" && row.Cells[2].Text != "RES")
													{


														//WAIVED SURCHARGE -- CHECK TAGGING IN "MISC" COLUMN IF IT IS WAIVED
														qry1 = $@"SELECT ""Misc"",""Penalty"" FROM QUT1 WHERE ""DocEntry"" = {row.Cells[0].Text} AND ""Terms"" = '{row.Cells[2].Text}' AND ""PaymentType"" = '{row.Cells[3].Text}'";
														dt = hana.GetData(qry1, hana.GetConnection("SAOHana"));
														waiveTag = Convert.ToInt32(DataAccess.GetData(dt, 0, "Misc", "0").ToString());
														if (waiveTag == 0)
														{
															row.Cells[8].Text = SystemClass.ToCurrency(penalty.ToString());
														}
														else
														{
															row.Cells[8].Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "Penalty", "0").ToString());
														}
													}

													//** if due, row color = red
													row.Cells[4].ForeColor = System.Drawing.ColorTranslator.FromHtml("#a94442");
													row.Cells[4].Font.Bold = true;
												}
											}
											//IF MISCELLANEOUS
											else
											{
												row.Cells[8].Text = "0.00";
												row.Cells[4].ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
											}
										}















										else
										{
											//IF NOT MISCELLANEOUS 
											if (row.Cells[3].Text != "MISC" && row.Cells[2].Text != "RES")
											{
												//GET RUNNING BALANCE (SUM OF PAYMENTAMOUNT)
												if (double.Parse(row.Cells[6].Text) + double.Parse(row.Cells[7].Text) + double.Parse(row.Cells[9].Text) > 0)
												//if (double.Parse(row.Cells[14].Text) > 0)
												{



													//GET TOTAL PAID SURCHARGE
													string qryGetSurchargePayments = $@"SELECT IFNULL(SUM(A.""DocTotal""),0) ""DocTotal"" FROM OINV A WHERE A.""Project"" = '{txtProj.Text}' AND
								                                                  A.""U_BlockNo"" = '{txtBlock.Text}' AND A.""U_LotNo"" = '{txtLot.Text}' AND 
                                                                                  A.""U_TransactionType"" = 'RE - SUR' AND A.""U_Type"" = '{row.Cells[3].Text}' AND 
								                                                  A.""U_PaymentOrder"" = '{row.Cells[2].Text}' AND	A.""CANCELED"" = 'N'  AND A.""DocStatus"" = 'C'  AND IFNULL(A.""U_ContractStatus"",'Open') IN ('Open', 'Closed')";
													DataTable dtGetSurchargePayments = hana.GetData(qryGetSurchargePayments, hana.GetConnection("SAPHana"));
													double GetSurchargePayments = double.Parse(DataAccess.GetData(dtGetSurchargePayments, 0, "DocTotal", "0").ToString());

													//if (GetSurchargePayments != 0)
													if (double.Parse(row.Cells[12].Text) != 0)
													{
														double interest = double.Parse(row.Cells[7].Text);
														double ips = double.Parse(row.Cells[9].Text);

														if (((interest + ips + GetSurchargePayments) - double.Parse(row.Cells[12].Text) < 0))
														{
															running_bal = double.Parse(row.Cells[6].Text) + ((interest + ips + GetSurchargePayments) - double.Parse(row.Cells[12].Text));
														}
														else
														{
															//running_bal = double.Parse(row.Cells[6].Text);
															//COMMENTED 2022_09_05
															running_bal = double.Parse(row.Cells[6].Text) + ((interest + ips + GetSurchargePayments) - double.Parse(row.Cells[12].Text));
														}

													}
													else
													{
														////GET TOTAL PAID PRINCIPAL
														//string qryGetPrincipalPayments = $@"SELECT IFNULL(SUM(A.""DocTotal""),0) ""DocTotal"" FROM ODPI A WHERE A.""Project"" = '{txtProj.Text}' AND
														//      A.""U_BlockNo"" = '{txtBlock.Text}' AND A.""U_LotNo"" = '{txtLot.Text}' AND 
														//      A.""U_PaymentOrder"" = '{row.Cells[2].Text}' AND	A.""CANCELED"" <> 'Y'  AND A.""DocStatus"" = 'C'";
														//DataTable dtGetPrincipalPayments = hana.GetData(qryGetPrincipalPayments, hana.GetConnection("SAPHana"));
														//double GetPrincipalPayments = double.Parse(DataAccess.GetData(dtGetPrincipalPayments, 0, "DocTotal", "0").ToString());

														//running_bal = double.Parse(row.Cells[6].Text) - GetPrincipalPayments;
														double interest = double.Parse(row.Cells[7].Text);
														running_bal = double.Parse(row.Cells[6].Text) + interest;
													}






													//(PRINCIPAL + INTEREST  + IP&S) - AmountPaid
													//running_bal = (double.Parse(row.Cells[6].Text) + double.Parse(row.Cells[7].Text) + double.Parse(row.Cells[9].Text)) - (double.Parse(row.Cells[12].Text));
												}

												double penalty = 0;


												if (chkSel.Checked)
												{

													//Get Day, Month, and Year of the month
													int year = DateTime.Now.Year;
													int month = DateTime.Now.Month;
													//int monthsdue = Convert.ToDateTime(tSurChargeDate.Text).Month - Convert.ToDateTime(row.Cells[3].Text).Month;
													//int daysdue = Convert.ToDateTime(tSurChargeDate.Text).Day - Convert.ToDateTime(row.Cells[3].Text).Day;
													double monthsdue = 0;
													double daysdue = (Convert.ToDateTime(tSurChargeDate.Text) - Convert.ToDateTime(row.Cells[4].Text)).TotalDays;


													while (daysdue > 30)
													{
														monthsdue += 1;
														daysdue -= 30;
													}

													monthsdue = monthsdue < 1 ? 0 : monthsdue;
													daysdue = daysdue < 1 ? 0 : daysdue;
													int days = 30;
													//int days = DateTime.DaysInMonth(year, month);

													//COMPUTE FOR PENALTY
													//penalty = row.Cells[2].Text != "LB" ? ((running_bal / days) * (dppenalty / 100)) : ((running_bal / days) * (mapenalty / 100));
													DataTable dtSurcharge = ws.ComputeSurcharge(running_bal, dppenalty, monthsdue, daysdue).Tables[0];
													penalty = Convert.ToDouble(DataHelper.DataTableRet(dtSurcharge, 0, "Penalty", "0.00"));
													//penalty = ((running_bal * Math.Pow(1 + (dppenalty / 100), monthsdue)) * (1 + (dppenalty * daysdue / 3000))) - running_bal;

													//IF NOT RESERVATION
													if (row.Cells[3].Text != "RES" && row.Cells[2].Text != "RES")
													{
														//WAIVED SURCHARGE -- CHECK TAGGING IN "MISC" COLUMN IF IT IS WAIVED
														qry1 = $@"SELECT ""Misc"",""Penalty"" FROM QUT1 WHERE ""DocEntry"" = {row.Cells[0].Text} AND ""Terms"" = '{row.Cells[2].Text}' AND ""PaymentType"" = '{row.Cells[3].Text}'";
														dt = hana.GetData(qry1, hana.GetConnection("SAOHana"));
														waiveTag = Convert.ToInt32(DataAccess.GetData(dt, 0, "Misc", "0").ToString());
														if (waiveTag == 0)
														{
															row.Cells[8].Text = SystemClass.ToCurrency(penalty.ToString());
														}
														else
														{
															row.Cells[8].Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "Penalty", "0").ToString());
														}
													}

													//** if due, row color = red
													row.Cells[4].ForeColor = System.Drawing.ColorTranslator.FromHtml("#a94442");
													row.Cells[4].Font.Bold = true;
												}
											}
											//IF MISCELLANEOUS
											else
											{
												row.Cells[8].Text = "0.00";
												row.Cells[4].ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
											}
											//}
										}


									}
									else
									{
										row.Cells[8].Text = "0.00";
										row.Cells[4].ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
									}
								}
							}
							else
							{
								if (row.Cells[2].Text == "2" && row.Cells[3].Text == "LB")
								{
									var test = "";
								}
								row.Cells[7].Text = SystemClass.ToCurrency(row.Cells[17].Text);
							}



						}





						//COMPUTE PRINCIPAL
						principal += double.Parse(row.Cells[6].Text);

						if (row.Cells[3].Text == "LB")
							loanable += double.Parse(row.Cells[6].Text);
						else if (row.Cells[3].Text == "DP")
							dpamount += double.Parse(row.Cells[6].Text);
					}

					//Session["TotalLoanable"] = loanable;
					//Session["TotalDP"] = dpamount;


					//######################################
					//######## END COMPUTE SURCHAGE ########
					//######################################


					#endregion















					//######################################
					//##### COMPUTE PAYMENT AND BALANCE ####
					//###################################### 
					foreach (GridViewRow row in gvDownPayments.Rows)
					{


						// CLEAR j IF ADVANCE PAYMENT
						//2023-07-05 : ONLY RUN WHEN VISIBLE 
						if (divAdvancePrincipal.Visible)
						{
							if (row.Cells[2].Text == "2" && row.Cells[3].Text == "LB")
							{
								var test = "";
							}
							//INTEREST 
							//2023-08-02 : REMOVED CONDITION OF DATES
							//if (lblApplyToPrincipal.Text == "YES" && Convert.ToDateTime(row.Cells[4].Text) >= Convert.ToDateTime(tDocDate.Value))
							if (lblApplyToPrincipal.Text == "YES")
							{
								//UPDATE INTEREST TO 0 
								row.Cells[7].Text = "0.00";

								//UPDATE AMOUNT DUE TO PRINCIPAL + INTEREST
								row.Cells[5].Text = SystemClass.ToCurrency(row.Cells[6].Text);

							}
							else
							{

								row.Cells[7].Text = SystemClass.ToCurrency(row.Cells[17].Text);
								row.Cells[5].Text = SystemClass.ToCurrency((double.Parse(row.Cells[6].Text) + double.Parse(row.Cells[7].Text)).ToString());
							}
						}






						if (row.Cells[3].Text == "LB" && row.Cells[2].Text == "2")
						{
							var test = "";
						}


						//** Cash Discount **//
						cashDiscount += Convert.ToDouble(row.Cells[10].Text);


						//** Amount Due + Penalty + IP&S **//
						double AmountDue = double.Parse(row.Cells[5].Text);
						double Principal = double.Parse(row.Cells[6].Text);
						double SurCharges = 0;
						double IPS = 0;
						double CashDiscountPerRow = double.Parse(row.Cells[10].Text);

						SurCharges = double.Parse(row.Cells[8].Text);
						IPS = double.Parse(row.Cells[9].Text);


						string LineStatus = row.Cells[16].Text;

						if (lblApplyToPrincipal.Text == "NO")
						{
							totalamount = AmountDue + SurCharges + IPS;
						}
						else
						{
							totalamount = Principal + SurCharges + IPS;
						}

						//** AmountPaid ** //
						postedamt = double.Parse(SystemClass.ToCurrency(row.Cells[12].Text));

						if (LineStatus == "O")
						{
							if (payment > 0)
							{

								//IF CURRENT PAYMENT IS GREATER THAN (TotalAmount - AmountPaid)
								if (payment > double.Parse(SystemClass.ToCurrency((totalamount - postedamt).ToString())))
								{
									CheckBox chkSel = (CheckBox)gvDownPayments.Rows[row.RowIndex].FindControl("chkSel");
									if (chkSel.Checked)
									{

										string qryGetSurchargePayments = $@"SELECT IFNULL(SUM(A.""DocTotal""),0) ""DocTotal"" FROM OINV A WHERE A.""Project"" = '{txtProj.Text}' AND
                                                              A.""U_BlockNo"" = '{txtBlock.Text}' AND A.""U_LotNo"" = '{txtLot.Text}' AND 
                                                                                      A.""U_TransactionType"" = 'RE - SUR' AND A.""U_Type"" = '{row.Cells[3].Text}' AND 
                                                              A.""U_PaymentOrder"" = '{row.Cells[2].Text}' AND	A.""CANCELED"" = 'N' AND A.""DocStatus"" = 'C' AND
                                                              A.""U_Waive"" = 'N'  AND IFNULL(A.""U_ContractStatus"",'Open') IN ('Open', 'Closed') ";
										DataTable dtGetSurchargePayments = hana.GetData(qryGetSurchargePayments, hana.GetConnection("SAPHana"));
										double GetSurchargePayments = double.Parse(DataAccess.GetData(dtGetSurchargePayments, 0, "DocTotal", "0").ToString());
										//double GetSurchargePayments = 0;

										//Current Payment
										if (LineStatus == "O")
										{


											row.Cells[13].Text = SystemClass.ToCurrency((payment).ToString());


											row.Cells[13].Text = SystemClass.ToCurrency(((totalamount - postedamt) + GetSurchargePayments).ToString());
										}
										else
										{
											row.Cells[13].Text = "0.00";
										}

										//Balance
										row.Cells[14].Text = "0.00";

										if (LineStatus == "O")
										{
											payment -= Math.Round(double.Parse(SystemClass.ToCurrency(((totalamount + GetSurchargePayments) - postedamt).ToString())), 2);//totalamount;
										}
										row.BackColor = System.Drawing.ColorTranslator.FromHtml("#f0fff0");
									}
								}
								else
								{

									CheckBox chkSel = (CheckBox)gvDownPayments.Rows[row.RowIndex].FindControl("chkSel");
									if (chkSel.Checked)
									{
										//IF PARTIAL PAYMENT

										//Current Payment
										//row.Cells[13].Text = SystemClass.ToCurrency((payment + CashDiscountPerRow).ToString());

										string qryGetSurchargePayments = $@"SELECT IFNULL(SUM(A.""DocTotal""),0) ""DocTotal"" FROM OINV A WHERE A.""Project"" = '{txtProj.Text}' AND
                                                          A.""U_BlockNo"" = '{txtBlock.Text}' AND A.""U_LotNo"" = '{txtLot.Text}' AND 
                                                                                  A.""U_TransactionType"" = 'RE - SUR' AND  A.""U_Type"" = '{row.Cells[3].Text}' AND 
                                                          A.""U_PaymentOrder"" = '{row.Cells[2].Text}' AND	A.""CANCELED"" = 'N' AND A.""DocStatus"" = 'C' AND
                                                              A.""U_Waive"" = 'N' AND IFNULL(A.""U_ContractStatus"",'Open') IN ('Open', 'Closed')";
										DataTable dtGetSurchargePayments = hana.GetData(qryGetSurchargePayments, hana.GetConnection("SAPHana"));
										double GetSurchargePayments = double.Parse(DataAccess.GetData(dtGetSurchargePayments, 0, "DocTotal", "0").ToString());
										//double GetSurchargePayments = 0;

										row.Cells[13].Text = SystemClass.ToCurrency((payment + GetSurchargePayments).ToString());


										//Balance
										row.Cells[14].Text = SystemClass.ToCurrency(((totalamount - payment) + GetSurchargePayments).ToString());


										if (double.Parse(SystemClass.ToCurrency((payment + CashDiscountPerRow).ToString())) > 0)
										{
											//HONEYDEW
											row.BackColor = System.Drawing.ColorTranslator.FromHtml("#f0fff0");
											break;
										}
									}
								}
							}
							else
							{
								row.Cells[13].Text = Math.Round(payment).ToString();
							}
						}


					}
					//######################################
					//## END COMPUTE PAYMENT AND BALANCE ###
					//###################################### 





					//COMPUTE BALANCE 
					foreach (GridViewRow row in gvDownPayments.Rows)
					{

						if (row.Cells[2].Text == "7" && row.Cells[3].Text == "LB")
						{
							var test = "";
						}

						CheckBox chkSel = (CheckBox)gvDownPayments.Rows[row.RowIndex].FindControl("chkSel");
						if (chkSel.Checked)
						{

							//total += double.Parse(row.Cells[4].Text) + double.Parse(row.Cells[7].Text);

							//AMOUNT DUE+ PENALTY + IP&S
							totalamount = double.Parse(SystemClass.ToCurrency(row.Cells[5].Text)) + double.Parse(SystemClass.ToCurrency(row.Cells[8].Text)) + double.Parse(SystemClass.ToCurrency(row.Cells[9].Text));

							//AMOUNT PAID
							postedamt = double.Parse(SystemClass.ToCurrency(row.Cells[12].Text));

							//CURRENT PAYMENT (For Total Payment field)
							totalamtpd += double.Parse(SystemClass.ToCurrency(row.Cells[13].Text));

							//CURRENT PAYMENT
							double amtpaid = double.Parse(SystemClass.ToCurrency(row.Cells[13].Text));

							//INTEREST
							double interest = double.Parse(SystemClass.ToCurrency(row.Cells[7].Text));

							//PRINCIPAL
							double principalAmt = double.Parse(SystemClass.ToCurrency(row.Cells[6].Text));

							//BALANCE = TOTAL FOR PAYMENT - AMOUNT PAID - CURRENT PAYMENT
							string LineStatus = row.Cells[16].Text;


							string qryGetSurchargePayments = $@"SELECT IFNULL(SUM(A.""DocTotal""),0) ""DocTotal"" FROM OINV A WHERE A.""Project"" = '{txtProj.Text}' AND
								                                                  A.""U_BlockNo"" = '{txtBlock.Text}' AND A.""U_LotNo"" = '{txtLot.Text}' AND 
                                                                                  A.""U_TransactionType"" = 'RE - SUR' AND  A.""U_Type"" = '{row.Cells[3].Text}' AND 
								                                                  A.""U_PaymentOrder"" = '{row.Cells[2].Text}' AND	A.""CANCELED"" = 'N'  AND A.""DocStatus"" = 'C' AND
                                                                                  A.""U_Waive"" = 'N'  AND IFNULL(A.""U_ContractStatus"",'Open') IN ('Open', 'Closed')";
							DataTable dtGetSurchargePayments = hana.GetData(qryGetSurchargePayments, hana.GetConnection("SAPHana"));
							double GetSurchargePayments = double.Parse(DataAccess.GetData(dtGetSurchargePayments, 0, "DocTotal", "0").ToString());




							if (row.Cells[3].Text == "DP" && row.Cells[2].Text == "2")
							{
								var test = "";
							}


							if (LineStatus == "O")
							{
								if (lblApplyToPrincipal.Text == "NO")
								{
									row.Cells[14].Text = SystemClass.ToCurrency(((((totalamount - postedamt) - amtpaid) + GetSurchargePayments)).ToString());
								}
								else
								{
									//2023-06-13 : Consider if balance is already negative
									if (((((principalAmt - postedamt) - amtpaid) + GetSurchargePayments) - interest) < 0)
									{
										row.Cells[14].Text = "0.00";
									}
									else
									{
										row.Cells[14].Text = SystemClass.ToCurrency(((((principalAmt - postedamt) - amtpaid) + GetSurchargePayments) - interest).ToString());
									}
								}
							}
							else
							{
								row.Cells[14].Text = "0.00";
							}




							//check if fully paid
							if (((Math.Round((totalamount - postedamt) + GetSurchargePayments, 2) == Math.Round(amtpaid, 2))))
							{
								row.BackColor = System.Drawing.ColorTranslator.FromHtml("#bdffbd");
								//row.Cells[6].ForeColor = System.Drawing.ColorTranslator.FromHtml("#5cb85c");
							}
							else
							{
								if (amtpaid != 0)
								{
									//row.Cells[6].ForeColor = System.Drawing.ColorTranslator.FromHtml("#a94442");
									row.BackColor = System.Drawing.ColorTranslator.FromHtml("#f4a0a0");
								}
							}
							balance += double.Parse(row.Cells[14].Text);
							TotalField += double.Parse(row.Cells[5].Text);
						}
					}




					double tempTCPTotalPayment = 0;

					//** COMPUTE DUE PAYMENTS AND PENALTIES **//  
					foreach (GridViewRow row in gvDownPayments.Rows)
					{
						DateTime DueDate = Convert.ToDateTime(row.Cells[4].Text);
						//double PaymentAmount = double.Parse(row.Cells[4].Text);
						double Balance = double.Parse(row.Cells[14].Text);
						double Penalty = double.Parse(row.Cells[8].Text);
						double IPS = double.Parse(row.Cells[9].Text);


						if (DueDate <= DateTime.Now && Balance >= 0)
						{
							//duepayments += PaymentAmount + Penalty + IP&S;
							//duepayments += Balance + Penalty + IPS;
							duepayments += Balance;

							total_penalties += Penalty;
						}




						//2023-07-06 : GET TCP PAYMENTS ONLY
						string LineStatus = row.Cells[16].Text;
						string Line = row.Cells[2].Text;
						string Type = row.Cells[3].Text;
						double Interest = Math.Round(double.Parse(row.Cells[7].Text), 2);
						double SurCharges = Math.Round(double.Parse(row.Cells[8].Text), 2);
						double IPS1 = Math.Round(double.Parse(row.Cells[9].Text), 2);
						double Principal = Math.Round(double.Parse(row.Cells[6].Text), 2);
						double rowPreviousPayment = double.Parse(row.Cells[12].Text);
						double rowCurrentPayment = double.Parse(row.Cells[13].Text);

						//2023-10-17 : EXCLUDED PREVIOUS PAYMENTS
						//double totalPayment = Math.Round(rowPreviousPayment + rowCurrentPayment);
						double totalPayment = Math.Round(rowCurrentPayment, 2);

						if (Line == "1" && Type == "DP")
						{
							var test = 1;
						}

						if (LineStatus == "O") //check if status is open
						{
							if (Type != "MISC") //check if not miscellaneous payment
							{
								CheckBox chkSel = (CheckBox)gvDownPayments.Rows[row.RowIndex].FindControl("chkSel");
								if (chkSel.Checked) //check if selected
								{
									//2023-10-17 : COMMENTED, SUBTRACT NALANG ANG PENALTY PLUS SURCHARGE SA CURRENT PAYMENT LAGI
									//if (totalPayment == (Interest + SurCharges + IPS1 + Principal)) //if Payment = (Interest + Surcharge  + IPS + Principal) 
									//{
									//    tempTCPTotalPayment += Principal;
									//}
									//////2023-10-05 : ADD CONDITION - IF TOTALPAYMENT > (INTEREST + SURCHARGE + IPS1 + PRINCIPAL)
									////else if (totalPayment > (Interest + SurCharges + IPS1 + Principal))
									////{

									////}
									//else if (totalPayment < (Interest + SurCharges + IPS1 + Principal))
									//{
									//    if (totalPayment < (Interest + SurCharges + IPS1))
									//    {
									//        tempTCPTotalPayment += Principal - (Interest + SurCharges + IPS1);
									//    }
									//    else
									//    {
									//        tempTCPTotalPayment += totalPayment;
									//    }
									//}

									double tempValue = Math.Round(totalPayment, 2) - (Math.Round(Interest, 2) + Math.Round(SurCharges, 2) + Math.Round(IPS1, 2));
									if (tempValue > 0)
									{
										tempTCPTotalPayment += Math.Round(tempValue, 2);
									}

								}
							}
						}

					}

					ViewState["TCPTotalPayment"] = tempTCPTotalPayment;
					var statetest = ViewState["TCPTotalPayment"];


					ViewState["MaxLBTerm"] = 0;
					ViewState["MaxLBTermDate"] = "";
					ViewState["MinLBDate"] = "";

					//2023-06-07 : GET MAX TERM FOUND
					//ViewState["MaxTermExcess"] = 0;
					//ViewState["MaxTypeExcess"] = "";
					//ViewState["TotalPayment"] = 0;

					//#################################################################################################
					//######### GREEN OUT ALL PAID ROWS   ||| CHECKING OF WHICH IS THE HIGHEST TERM CHECKED ###########
					//#################################################################################################
					foreach (GridViewRow row in gvDownPayments.Rows)
					{


						CheckBox chkSel = (CheckBox)gvDownPayments.Rows[row.RowIndex].FindControl("chkSel");

						//GREEN OUT ALL PAID ROWS
						string LineStatus = row.Cells[16].Text;
						if (LineStatus == "C" || LineStatus == "R")
						{

							row.BackColor = System.Drawing.ColorTranslator.FromHtml("#28B463");
							chkSel.Checked = true;

							if (row.Cells[3].Text == "MISC")
							{
								row.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffff00");
							}
						}
						//2024-10-05 : ADD NEW COLOR FOR ADVANCE PAYMENTS
						//else if (LineStatus == "A")
						//{
						//	row.BackColor = System.Drawing.ColorTranslator.FromHtml("#01c1cd");
						//	chkSel.Checked = true;
						//}



						// CHECKING OF WHICH IS THE HIGHEST TERM CHECKED
						int term = Convert.ToInt32(row.Cells[2].Text);
						string dueDate = row.Cells[4].Text;
						string paymentType = row.Cells[3].Text;
						int MaxTerm = Convert.ToInt32(ViewState["MaxLBTerm"]);
						int MinTerm = 0;

						//2023-06-07 : GET MAX TERM FOUND
						int MaxTermExcess = Convert.ToInt32(ViewState["MaxTermExcess"]);

						//CHECK IF IT IS CHECKED 
						if (chkSel.Checked)
						{

							//CHECK IF LINESTATUS IS OPEN
							if (LineStatus == "O" && paymentType == "LB")
							{
								//IF 0, GET MAX TERM
								if (MaxTerm == 0)
								{
									ViewState["MaxLBTerm"] = term;
									ViewState["MaxLBTermDate"] = dueDate;
								}
								// IF MORE THAN 0, CHECK IF CURRENT CONSIDERED MAX TERM IS LESS THAN THE SELECTED TERM; IF YES, GET MAX TERM
								else
								{
									if (MaxTerm < term)
									{
										ViewState["MaxLBTerm"] = term;
										ViewState["MaxLBTermDate"] = dueDate;
									}
								}


								//04-24-2023 :  GET MINIMUM DUE DATE AND DUEDATE > POSTING DATE
								if (string.IsNullOrWhiteSpace(ViewState["MinLBDate"].ToString()))
								{
									//if (Convert.ToDateTime(dueDate) <= Convert.ToDateTime(tDocDate.Value))
									//{
									ViewState["MinLBDate"] = dueDate;
									ViewState["MinLBTerm"] = term;

									//}
									//else
									//{
									//    //IF NO DATES SUPPOSEDLY PAID PRIOR, GET FIRST DATE FOR ADVANCE PAYMENT
									//    ViewState["MinLBDate"] = dueDate;
									//}
								}
							}

							if (term == 9 && LineStatus == "O" && paymentType == "LB")
							{
								var test123123123 = "";
							}

							//2023-06-07 : GET LAST TERM
							if (LineStatus == "O" && paymentType != "MISC" && paymentType != "RES")
							{
								if (MaxTermExcess == 0)
								{
									ViewState["MaxTermExcess"] = term;
									ViewState["MaxTypeExcess"] = paymentType;
								}
								else
								{
									if (MaxTermExcess < term)
									{
										ViewState["MaxTermExcess"] = term;
										ViewState["MaxTypeExcess"] = paymentType;
									}
								}
							}



						}






					}








					//AMOUNT DUE
					lblDue.Text = SystemClass.ToCurrency(((duepayments - cashDiscount) < 0 ? 0 : (duepayments - cashDiscount)).ToString());
					//TOTAL
					lblAmount.Text = SystemClass.ToCurrency(TotalField.ToString());
					//BALANCE
					txtBalance.Text = SystemClass.ToCurrency(balance.ToString());
					// SystemClass.ToCurrency(principal.ToString()); //SystemClass.ToCurrency((total - payment).ToString());





				}
			}
			catch (Exception ex)
			{
				alertMsg(ex.Message, "error");
			}
		}



		protected void gvBranch_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			LoadBranch(txtBankCode.Value);
			gvBranch.PageIndex = e.NewPageIndex;
			gvBranch.DataBind();
		}

		protected void gvBanks_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			gvBanks.PageIndex = e.NewPageIndex;
			LoadBanks(ViewState["BankType"].ToString());
		}

		protected void gvBanks_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			//if (e.CommandName.Equals("Sel"))
			//{
			//    int row = int.Parse(e.CommandArgument.ToString());
			//    string BankCode = gvBanks.Rows[row].Cells[0].Text;
			//    string BankName = gvBanks.Rows[row].Cells[1].Text;

			//    txtBankCode.Value = BankCode;
			//    txtBank.Value = BankName;


			//}
			//else
			//{
			//    int row = int.Parse(e.CommandArgument.ToString());
			//    string BankName = gvBanks.Rows[row].Cells[1].Text;

			//    Text1.Value = BankName;
			//}
			//ScriptManager.RegisterStartupScript(this, this.GetType(), "", "hideBank()", true);
		}

		protected void gvBranch_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			if (e.CommandName.Equals("Sel"))
			{
				int row = int.Parse(e.CommandArgument.ToString());
				string Account = gvBranch.Rows[row].Cells[0].Text;
				string CheckAccount = gvBranch.Rows[row].Cells[1].Text;
				string Branch = gvBranch.Rows[row].Cells[2].Text;

				txtAccount.Value = Account;
				txtCheckAccount.Value = CheckAccount;

				txtDepositedBranch.Value = Branch;
				ScriptManager.RegisterStartupScript(this, this.GetType(), "", "hideBranch()", true);
			}
		}

		protected void gvBuyers_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			gvBuyers.DataSource = (DataSet)ViewState["Buyers"];
			gvBuyers.PageIndex = e.NewPageIndex;
			gvBuyers.DataBind();
		}

		protected void gvBuyers_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			if (e.CommandName.Equals("Sel"))
			{
				int row = int.Parse(e.CommandArgument.ToString());

				lblID.Text = gvBuyers.Rows[row].Cells[0].Text;
				lblName.Text = gvBuyers.Rows[row].Cells[4].Text;

				txtFName.Text = gvBuyers.Rows[row].Cells[1].Text;
				txtLName.Text = gvBuyers.Rows[row].Cells[2].Text;
				txtMName.Text = gvBuyers.Rows[row].Cells[3].Text;
				//txtBDate.Text = gvBuyers.Rows[row].Cells[5].Text;
				lblTIN.Text = gvBuyers.Rows[row].Cells[6].Text;

				LoadHouseList();
				HideColumnInHistory();

				((DataTable)ViewState["dtPayments"]).Rows.Clear();
				gvPayments.DataSource = ((DataTable)ViewState["dtPayments"]);
				gvPayments.DataBind();


				pnlBuyerInfo.Visible = true;
				ViewState["chkTagMISC"] = 0;
				ViewState["chkTagDP"] = 0;
				ScriptManager.RegisterStartupScript(this, this.GetType(), "close", "closeBuyer();", true);
			}
		}

		protected void bSearchBuyer_Click(object sender, EventArgs e)
		{
			ViewState["Buyers"] = ws.SearchBuyersSelection(txtSearchBuyer.Value);
			gvBuyers.DataSource = (DataSet)ViewState["Buyers"];
			gvBuyers.DataBind();
		}
		protected void removebuyer_ServerClick(object sender, EventArgs e)
		{
			confirmation("Are you sure you want to remove selected buyer?Unsaved data will be lost.", "removebuyer");
		}
		void confirmation(string body, string type)
		{
			ViewState["ConfirmType"] = type;
			lblConfirmationInfo.Text = body;
			btnYes.Focus();
			//Show Modal
			ScriptManager.RegisterStartupScript(this, this.GetType(), "confirmation", "showConfirmation();", true);
		}
		void closeconfirm()
		{
			ScriptManager.RegisterStartupScript(this, this.GetType(), "closeConfirmation", "closeConfirmation();", true);
		}

		protected void removebuyer_Click(object sender, EventArgs e)
		{
			confirmation("Are you sure you want to remove selected buyer?Unsaved data will be lost.", "removebuyer");
		}
		void alertMsg(string Message, string type)
		{
			lblMessageAlert.Text = Message;
			if (type == "success")
				alertIcon.ImageUrl = "~/assets/img/success.png";
			else if (type == "warning")
				alertIcon.ImageUrl = "~/assets/img/warning.png";
			else if (type == "error")
				alertIcon.ImageUrl = "~/assets/img/error.png";
			else if (type == "info")
				alertIcon.ImageUrl = "~/assets/img/info.png";

			ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showAlert();", true);
		}
		protected void gvHouseLot_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			try
			{
				if (e.CommandName.Equals("Sel"))
				{
					int row = int.Parse(e.CommandArgument.ToString());

					Label lblDocNum = (Label)gvHouseLot.Rows[row].FindControl("lblDocNum");
					Label lblDocEntry = (Label)gvHouseLot.Rows[row].FindControl("lblDocEntry");
					Label lblProject = (Label)gvHouseLot.Rows[row].FindControl("lblProject");
					Label lblBlock = (Label)gvHouseLot.Rows[row].FindControl("lblBlock");
					Label lblLot = (Label)gvHouseLot.Rows[row].FindControl("lblLot");
					Label lblModel = (Label)gvHouseLot.Rows[row].FindControl("lblModel");
					Label lblStatus = (Label)gvHouseLot.Rows[row].FindControl("lblStatus");
					Label lblPhase = (Label)gvHouseLot.Rows[row].FindControl("lblPhase");
					Label lblFinscheme = (Label)gvHouseLot.Rows[row].FindControl("lblFinscheme");
					Label lblFinschemeCode = (Label)gvHouseLot.Rows[row].FindControl("lblFinschemeCode");
					Label lblBlock1 = (Label)gvHouseLot.Rows[row].FindControl("lblBlock1");
					Label lblLot1 = (Label)gvHouseLot.Rows[row].FindControl("lblLot1");
					Label lblModel1 = (Label)gvHouseLot.Rows[row].FindControl("lblModel1");
					Label lblProjCode = (Label)gvHouseLot.Rows[row].FindControl("lblProjCode");
					Label lblApproved = (Label)gvHouseLot.Rows[0].FindControl("lblApproved");

					//2023-05-31 : REQUESTED BY MS KATE : ADD MISC FINANCING SCHEME
					Label lblMiscFinSchemeCode = (Label)gvHouseLot.Rows[row].FindControl("lblMiscFinSchemeCode");

					//load data 
					txtProj.Text = lblProjCode.Text;
					txtDocNum.Text = lblDocNum.Text;
					txtBlock.Text = lblBlock1.Text;
					txtLot.Text = lblLot1.Text;
					txtModel.Text = lblModel1.Text;
					txtPhase.Text = lblPhase.Text;

					lblTCPFinScheme.Text = lblFinscheme.Text;

					//2023-05-31 : REQUESTED BY MS KATE : ADD MISC FINANCING SCHEME
					lblMiscFinScheme.Text = lblMiscFinSchemeCode.Text;




					ViewState["Status"] = lblStatus.Text;
					ViewState["FinancingScheme"] = lblFinschemeCode.Text;
					ViewState["Status"] = lblStatus.Text;
					ViewState["SQDocEntry"] = int.Parse(lblDocEntry.Text);
					ViewState["BuyerStatus"] = lblApproved.Text == "" || lblApproved.Text == "?" ? "N" : lblApproved.Text;

					LoadDemandLetters();


					//GET QUOTATION DATA FROM SQL[OQUT]
					DataTable dtQuotation = ws.GetQuotationData(int.Parse(lblDocEntry.Text)).Tables[0];
					if (DataAccess.Exist(dtQuotation))
					{
						ReloadData(dtQuotation);
					}



					//2023-07-02 : CHANGE SOURCE
					//gvHistory.DataSource = ws.GetPaymentHistory(int.Parse(lblDocEntry.Text));
					gvHistory.DataSource = ws.GetPaymentHistoryNew(int.Parse(lblDocEntry.Text));

					gvHistory.DataBind();
					HideColumnInHistory();

					gvDuePayments.Visible = false;
					gvDownPayments.Visible = true;
					gvDownPayments.DataSource = ws.GetPaymentBreakdown(int.Parse(lblDocEntry.Text));
					gvDownPayments.DataBind();

					if (gvDownPayments.Rows.Count == 0)
					{
						pnlPayment.Enabled = false;
					}
					else
					{
						pnlPayment.Enabled = true;
						ViewState["DPEntry"] = int.Parse(gvDownPayments.Rows[0].Cells[0].Text);
					}

					NewComputeTotal(gvPayments);

					//** highlight selected row **//
					foreach (GridViewRow selectedrow in gvHouseLot.Rows)
					{
						if (selectedrow.RowIndex == row)
						{ selectedrow.BackColor = System.Drawing.ColorTranslator.FromHtml("#A1DCF2"); }
						else
						{ selectedrow.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF"); }
					}
					tDocDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
					tSurChargeDate.Text = DateTime.Today.ToString("yyyy-MM-dd");

					gvPayments.DataSource = null;
					gvPayments.DataBind();

					ViewState["chkTagMISC"] = 0;
					ViewState["chkTagDP"] = 0;
					ViewState["ctrFordivAdvancePrincipal"] = 0;
					gvDownPayment.DataSource = null;
					gvDownPayment.DataBind();
					gvAmortization.DataSource = null;
					gvAmortization.DataBind();
					divAdvancePaymentscheduleAccordion.Visible = false;

				}
			}
			catch (Exception ex)
			{
				alertMsg(ex.Message, "error");
			}
		}


		void HideColumnInHistory()
		{
			foreach (GridViewRow rows in gvHistory.Rows)
			{
				if (rows.Cells[5].Text.Contains("PR"))
				{
					((LinkButton)rows.Cells[9].FindControl("btnPrintPR")).Visible = true;
					((LinkButton)rows.Cells[9].FindControl("btnPrint")).Visible = false;
					((LinkButton)rows.Cells[9].FindControl("btnPrintAR")).Visible = false;
				}
				else if (rows.Cells[5].Text.Contains("AR"))
				{
					((LinkButton)rows.Cells[9].FindControl("btnPrintAR")).Visible = true;
					((LinkButton)rows.Cells[9].FindControl("btnPrint")).Visible = false;
					((LinkButton)rows.Cells[9].FindControl("btnPrintPR")).Visible = false;
				}
				else if (rows.Cells[5].Text.Contains("OR"))
				{
					((LinkButton)rows.Cells[9].FindControl("btnPrint")).Visible = true;
					((LinkButton)rows.Cells[9].FindControl("btnPrintPR")).Visible = false;
					((LinkButton)rows.Cells[9].FindControl("btnPrintAR")).Visible = false;
				}
				else
				{
					((LinkButton)rows.Cells[9].FindControl("btnPrint")).Visible = false;
					((LinkButton)rows.Cells[9].FindControl("btnPrintPR")).Visible = false;
					((LinkButton)rows.Cells[9].FindControl("btnPrintAR")).Visible = false;
				}

			}
		}




		protected void btnFind_Click(object sender, EventArgs e)
		{
			//show buyer selection
			ScriptManager.RegisterStartupScript(this, this.GetType(), "close", "showBuyer();", true);
			pnlBuyerInfo.Visible = false;
		}

		protected void gvDuePayments_RowCommand(object sender, GridViewCommandEventArgs e)
		{

		}

		protected void gvDuePayments_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{

		}

		protected void btnFinish_Click(object sender, EventArgs e)
		{

			try
			{


				//2023-06-07 : ADD CONFIRMATION MESSAGE FOR EXCESS PAYMENT POSTING TO AR INVOICE STANDALONE
				string msg = $@"Are you sure you want to process payment?";


				string qrySundry = $@"select top 1 ""Terms"",""PaymentType"" from qut1 
                                                            where ""DocEntry"" = {ViewState["SQDocEntry"]} and ""PaymentType"" IN ('DP','LB') and ""PaymentAmount"" > 0 order by ""Id"" desc";
				DataTable dtSundry = hana.GetData(qrySundry, hana.GetConnection("SAOHana"));
				int sundryLastTerm = Convert.ToInt16(DataHelper.DataTableRet(dtSundry, 0, "Terms", ""));
				string sundryLastType = DataHelper.DataTableRet(dtSundry, 0, "PaymentType", "");


				var test1 = ViewState["MaxTermExcess"];
				var test2 = ViewState["MaxTypeExcess"];
				var test3 = ViewState["TotalPayment"];


				//2023-09-01 : GET SURCHARGE AND INTEREST, THEN SUBTRACT TO CURRENT TOTAL PAYMENT

				double SurchargeForExcess = 0;
				double InterestForExcess = 0;
				//loop to get interest and surcharge
				foreach (GridViewRow rowExcess in gvDownPayments.Rows)
				{
					CheckBox chkFForExcess = (CheckBox)gvDownPayments.Rows[rowExcess.RowIndex].FindControl("chkSel");
					if (chkFForExcess.Checked)
					{
						if (rowExcess.Cells[16].Text == "O")
						{
							//2023-09-01 : REMOVED CONDITION
							//if (rowExcess.Cells[2].Text == sundryLastTerm.ToString())
							//{
							SurchargeForExcess += Convert.ToDouble(rowExcess.Cells[8].Text);
							InterestForExcess += Convert.ToDouble(rowExcess.Cells[7].Text);
							//}
						}
					}
				}



				if (Convert.ToInt32(ViewState["MaxTermExcess"]) == sundryLastTerm && ViewState["MaxTypeExcess"].ToString() == sundryLastType &&
						//2023-09-01 : SUBTRACT SURCHARGE + INTEREST SA CURRENT PAYMETN    
						//((Convert.ToDouble(lblAmountDue.Text) + Convert.ToDouble(lblTotalPayment.Text))) > Convert.ToDouble(lblNETTCP.Text))
						(((Convert.ToDouble(lblAmountDue.Text) - (SurchargeForExcess + InterestForExcess)) + Convert.ToDouble(lblTotalPayment.Text))) > Convert.ToDouble(lblNETTCP.Text))
				{
					msg += $@" An EXCESS PAYMENT will be posted. ";
				}






				//string qry = $@"Select a.""DocEntry"" from oqut a inner join ocrd b on a.""CardCode"" = b.""CardCode"" 
				//        where b.""LicTradNum"" =  '{lblTIN.Text}' and IFNULL(a.""SAPDocEntry"",'') <> ''";
				//DataTable dt = hana.GetData(qry, hana.GetConnection("SAPHana"));

				//if (dt.Rows.Count == 0)
				//{
				//    confirmation("Buyer already exists. Buyer details will be updated.", "finish");
				//}
				//else
				//{



				//2023-07-05 : CHECK IF MISCELLANEOUS
				if (Convert.ToInt16(ViewState["chkTagDP"]) > 0)
				{

					//2023-07-05 : GET LTS FOR CHECKING
					DataTable generalData = ws.GetGeneralData(Convert.ToInt32(ViewState["SQDocEntry"]), ConfigurationManager.AppSettings["HANADatabase"].ToString()).Tables[0];
					DateTime psDocDate = Convert.ToDateTime(DataHelper.DataTableRet(generalData, 0, "DocDate", "1999-01-01"));


					//2023-07-06 : ADD TCP CURRENT PAYMENT ONLY INSTEAD OF TOTAL OF ALL CURRENT PAYMENT
					//double TotalAmoundPaid0 = Convert.ToDouble(lblTotalPayment.Text) + Convert.ToDouble(lblAmountDue.Text);
					//double totalCurrentPayment = 0;
					//foreach (GridViewRow row1 in gvPayments.Rows)
					//{
					//    totalCurrentPayment += Convert.ToDouble(row1.Cells[2].Text);
					//}

					double testCurrentPayment = Convert.ToDouble(ViewState["TCPTotalPayment"]);
					double TotalAmoundPaid0 = Convert.ToDouble(lblTotalPayment.Text) + testCurrentPayment;

					var ARReserveInvoiceCondition0 = ConfigSettings.ARReserveInvoiceCondition;
					DateTime psLTSIssueDate = Convert.ToDateTime(DataHelper.DataTableRet(generalData, 0, "LTSIssueDate", psDocDate.ToString()));

					string LTSNoCheck = DataHelper.DataTableRet(generalData, 0, "LTSNo", "");
					string OriginalLTSNoCheck = DataHelper.DataTableRet(generalData, 0, "OriginalLTSNo", "");

					//2023-07-05: CONDITION IS FOR LOI ONLY
					if (!string.IsNullOrWhiteSpace(LTSNoCheck) && !string.IsNullOrWhiteSpace(OriginalLTSNoCheck))
					{
						psLTSIssueDate = psDocDate;
					}
					else if (!string.IsNullOrWhiteSpace(LTSNoCheck) && string.IsNullOrWhiteSpace(OriginalLTSNoCheck))
					{
						psLTSIssueDate = Convert.ToDateTime(DataHelper.DataTableRet(generalData, 0, "LTSIssueDate", psDocDate.ToString()));
					}
					else
					{
						psLTSIssueDate = psDocDate;
					}

					//2023-07-14 : GET BOOKSTATUS TO ADD ON CHECKING
					string BookStatus = DataHelper.DataTableRet(generalData, 0, "BookStatus", "");




					if (
						((TotalAmoundPaid0 / double.Parse(lblNETTCP.Text)) >= (Convert.ToDouble(ARReserveInvoiceCondition0))) &&
						//2023-07-14 : ADDED CONDITION -- ONLY ADDS THE MESSAGE WHEN NOT YET BOOKED
						BookStatus == "N"
						)
					{
						//2023-08-14 : ADDED CONDITION  -- EXCLUDE ON LOI
						if (!string.IsNullOrWhiteSpace(LTSNoCheck))
						{

							//lblTaxClassification.Text = ConfigSettings.PaymentScheme1;
							//if (double.Parse(lblPercentPaid.Text.Replace("%", "")) <= 25)
							//{

							if ((Convert.ToDateTime(tDocDate.Value) <= (Convert.ToDateTime(psLTSIssueDate.Year + ConfigSettings.TaxEndOfYear_Month + ConfigSettings.TaxEndOfYear_Day))))
							{
								msg += $@" (Payment scheme to be used is {ConfigSettings.PaymentScheme1.ToUpper()}.)";
							}
							else
							{
								msg += $@" (Payment scheme to be used is {ConfigSettings.PaymentScheme2.ToUpper()}.)";
								//lblTaxClassification.Text = ConfigSettings.PaymentScheme2;
							}
							//alertMsg($@"Payment scheme to be used is {lblTaxClassification.Text}.", "info");
							//}
						}
					}
				}






				////2024-10-07: FOR TESTING PURPOSES ONLY  
				//DataTable generalData1 = new DataTable();
				//generalData1 = ws.GetGeneralData(Convert.ToInt32(ViewState["SQDocEntry"]), ConfigurationManager.AppSettings["HANADatabase"].ToString()).Tables[0];

				//DataTable _dtTotalPaid1 = hana.GetData($@"CALL USRSP_DBTI_IC_Ledger_SAO ({ViewState["SQDocEntry"]}, '{Convert.ToDateTime(tDocDate.Value).ToString("yyyyMMdd")}')", hana.GetConnection("SAPHana"));
				//double _TCPPayment = Convert.ToDouble(DataHelper.DataTableRet(_dtTotalPaid1, 0, "NEG_PRINCIPAL", "0"));
				//double _TCPPaymentPercent = Convert.ToDouble(DataHelper.DataTableRet(_dtTotalPaid1, 0, "TotalTCP", "0"));

				//double _netTCP = Convert.ToDouble(DataHelper.DataTableRet(generalData1, 0, "NetTCP", "0"));
				//var _ARReserveInvoiceCondition1 = ConfigSettings.ARReserveInvoiceCondition;
				//DataTable _dtLatestLTSNo = hana.GetData($@"SELECT ""U_LTSNo"" FROM OBTN 
				//                                                       where ""U_BlockNo"" = '{txtBlock.Text}' and ""U_LotNo"" = '{txtLot.Text}' and ""U_Project"" = '{txtProj.Text}'", hana.GetConnection("SAPHana"));
				//string _LatestLTSNo = DataHelper.DataTableRet(_dtLatestLTSNo, 0, "U_LTSNo", "");
				//int _ARReserveEntry = 0;

				//double currentPayment = Convert.ToDouble(ViewState["TCPTotalPayment"]);

				//msg += $@" (Formula: (TCPPayment + CurrentPayment) >= (NetTCP * 0.25%) && If LTS Only) : /n
				//				TCPPayment = {Math.Round(_TCPPayment, 2)}; CurrentPayment = {Math.Round(currentPayment, 2)}; NetTCP = {Math.Round(double.Parse(lblNETTCP.Text), 2)}; ";









				confirmation(msg, "finish");
				//}
				//int tag = 0;
				////if (!string.IsNullOrWhiteSpace(txtAR.Text) && string.IsNullOrWhiteSpace(txtARDate.Text))
				////{
				////    tag++;
				////}
				//if (!string.IsNullOrWhiteSpace(txtPR.Text) && string.IsNullOrWhiteSpace(txtPRDate.Text))
				//{
				//    tag++;
				//}
				//if (tag == 0)
				//{
				//}
				//else
				//{
				//    alertMsg("C", "warning");
				//}
			}
			catch (Exception ex)
			{
				alertMsg(ex.Message, "info");
			}

		}

		protected void btnClose_ServerClick(object sender, EventArgs e)
		{
			Response.Redirect("~/pages/Dashboard.aspx");
		}

		protected void btnSearchAccounts_Click(object sender, EventArgs e)
		{
			ViewState["GLAccounts"] = ws.SearchGLAccounts(txtSearchAccounts.Value);
			gvAccounts.DataSource = (DataSet)ViewState["GLAccounts"];
			gvAccounts.DataBind();
		}

		protected void gvAccounts_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			//int index = Convert.ToInt32(e.CommandArgument);
			//if (e.CommandName.Equals("Sel"))
			//{ 
			//    txtCreditAccountCode.Value = gvAccounts.Rows[index].Cells[0].Text;
			//    txtCreditAccount.Value = gvAccounts.Rows[index].Cells[1].Text;
			//}
			//ScriptManager.RegisterStartupScript(this, this.GetType(), "close", "closeAccounts();", true);
		}

		protected void gvAccounts_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			gvAccounts.DataSource = (DataSet)ViewState["GLAccounts"];
			gvAccounts.PageIndex = e.NewPageIndex;
			gvAccounts.DataBind();
		}

		protected void gvCreditCard_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			int index = Convert.ToInt32(e.CommandArgument);
			if (e.CommandName.Equals("Sel"))
			{
				txtCreditCardCode.Value = gvCreditCard.Rows[index].Cells[0].Text;
				txtCreditCard.Value = gvCreditCard.Rows[index].Cells[1].Text;
				txtCreditAccountCode.Value = gvCreditCard.Rows[index].Cells[3].Text;
				txtCreditAccount.Value = gvCreditCard.Rows[index].Cells[4].Text;
			}
		}

		protected void gvPymtMethod_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			int index = Convert.ToInt32(e.CommandArgument);
			if (e.CommandName.Equals("Sel"))
			{
				txtCreditMethodCode.Value = gvPymtMethod.Rows[index].Cells[0].Text;
				txtCreditMethod.Value = gvPymtMethod.Rows[index].Cells[1].Text;
			}
			ScriptManager.RegisterStartupScript(this, this.GetType(), "close", "closePymtMethod();", true);
		}

		protected void btnShowCredit_ServerClick(object sender, EventArgs e)
		{
			gvCreditCard.DataSource = ws.GetCreditCards();
			gvCreditCard.DataBind();
			ScriptManager.RegisterStartupScript(this, this.GetType(), "showCredit", "showCredit();", true);
		}

		protected void btnAccounts_ServerClick(object sender, EventArgs e)
		{
			LoadAccounts();
			ScriptManager.RegisterStartupScript(this, this.GetType(), "showAccounts", "showAccounts();", true);
		}

		protected void btnAddCredit_Click(object sender, EventArgs e)
		{
			try
			{
				if (ViewState["ProjCode"] == null)
				{
					alertMsg("Please select project first!", "warning");
					return;
				}
				//** check adding blockings **//
				if (string.IsNullOrEmpty(txtCreditCard.Value) ||
					string.IsNullOrEmpty(txtCreditCardNum.Value) ||
					string.IsNullOrEmpty(txtCreditAccount.Value) ||
					string.IsNullOrEmpty(txtCreditAmount.Text) ||
					string.IsNullOrEmpty(txtCreditMethod.Value) ||
					string.IsNullOrEmpty(txtNoOfPayments.Value) ||
					string.IsNullOrEmpty(txtCardBrandAccountName.Value)

					//||                    string.IsNullOrEmpty(txtVoucherNum.Value)
					)
				{
					alertMsg("Please fill up required fields", "warning");
				}
				else
				{
					//check if expiry format is correct mm/yy
					if (!Regex.IsMatch(txtValidUntil.Text, @"(0[1-9]|1[0-2])\/[0-9]{2}"))
					{
						alertMsg("Please check expiry date format", "warning");
					}
					else
					{
						if (btnAddCredit.Text == "Add Credit")
						{
							//add check
							DataTable dt = new DataTable();
							dt = (DataTable)ViewState["dtPayments"];

							DataRow dr = dt.NewRow();
							dr[0] = Convert.ToInt32(dt.Rows.Count + 1);
							dr[1] = "Credit";
							dr[2] = SystemClass.ToCurrency(txtCreditAmount.Text);
							//creditm
							dr[9] = txtCreditCard.Value;
							dr[10] = txtCreditAccountCode.Value;
							dr[11] = txtCreditAccount.Value;
							dr[12] = txtCreditCardNum.Value;
							dr[13] = txtValidUntil.Text.Substring(0, 3) + $"01/{ConfigSettings.CreditValidDatePrefix}" + txtValidUntil.Text.Substring(3, 2);
							dr[14] = txtIDNum.Value;
							dr[15] = txtTelNo.Value;
							dr[16] = txtCreditMethodCode.Value;
							dr[17] = txtCreditMethod.Value;
							dr[18] = txtNoOfPayments.Value;
							dr[19] = txtVoucherNum.Value;
							dr[39] = txtCardBrandAccountCode.Value;
							dr[40] = txtCardBrandAccountName.Value;

							dt.Rows.Add(dr);
							ViewState["dtPayments"] = dt;
							LoadData(gvPayments, "dtPayments");
						}
						else
						{
							//update check
							foreach (DataRow dr in ((DataTable)ViewState["dtPayments"]).Rows)
							{
								if (dr[0].ToString() == ViewState["linenum"].ToString())
								{
									dr[2] = SystemClass.ToCurrency(txtCreditAmount.Text);
									//credit
									dr[9] = txtCreditCard.Value;
									dr[10] = txtCreditAccountCode.Value;
									dr[11] = txtCreditAccount.Value;
									dr[12] = txtCreditCardNum.Value;
									dr[13] = txtValidUntil.Text.Substring(0, 3) + $"01/{ConfigSettings.CreditValidDatePrefix}" + txtValidUntil.Text.Substring(3, 2);
									dr[14] = txtIDNum.Value;
									dr[15] = txtTelNo.Value;
									dr[16] = txtCreditMethodCode.Value;
									dr[17] = txtCreditMethod.Value;
									dr[18] = txtNoOfPayments.Value;
									dr[19] = txtVoucherNum.Value;
									dr[39] = txtCardBrandAccountCode.Value;
									dr[40] = txtCardBrandAccountName.Value;
									break;
								}
							}

							btnAddCredit.Text = "Add Credit";
						}

						LoadData(gvPayments, "dtPayments");
						NewComputeTotal(gvPayments);
						loadApplyToPrincipal(0);

						ClearCheck();
						ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "closeCreditPayment();", true);
					}
				}

			}
			catch (Exception ex)
			{
				alertMsg(ex.Message, "error");
			}
		}

		protected void btnCash_Click(object sender, EventArgs e)
		{
			if (ViewState["ProjCode"] == null)
			{
				alertMsg("Please select project first!", "warning");
				return;
			}
			bAddCash.Text = "Add";
			txtCashAmount.Text = "0";
			txtCashAmount.Focus();
			ScriptManager.RegisterStartupScript(this, this.GetType(), "cash", "showCash();", true);
		}

		protected void btnCheck_Click(object sender, EventArgs e)
		{
			if (ViewState["ProjCode"] == null)
			{
				alertMsg("Please select project first!", "warning");
				return;
			}
			ClearCheck();
			btnAddCheck.Text = "Add Check";
			txtCheckNo.Focus();
			ScriptManager.RegisterStartupScript(this, this.GetType(), "check", "showCheck();", true);
		}

		protected void btnCredit_Click(object sender, EventArgs e)
		{
			if (ViewState["ProjCode"] == null)
			{
				alertMsg("Please select project first!", "warning");
				return;
			}
			ClearCredit();
			btnAddCredit.Text = "Add Credit";

			//btnShowCredit.Focus();

			DataTable dt = new DataTable();
			dt = ws.GetCreditCards().Tables[0];



			txtCreditCardCode.Value = DataHelper.DataTableRet(dt, 0, "CreditCard", "");
			txtCreditCard.Value = DataHelper.DataTableRet(dt, 0, "CardName", "");
			txtCreditAccountCode.Value = DataHelper.DataTableRet(dt, 0, "AcctCode", "");
			txtCreditAccount.Value = DataHelper.DataTableRet(dt, 0, "AcctCode", "");


			txtCreditMethodCode.Value = "1";
			txtCreditMethod.Value = "CC";

			txtCardBrandAccountCode.Value = "";
			txtCardBrandAccountName.Value = "";

			txtCreditCardNum.Focus();
			ScriptManager.RegisterStartupScript(this, this.GetType(), "credit", "showCreditPayment();", true);
		}

		protected void gvHistory_RowCommand(object sender, GridViewCommandEventArgs e)
		{

		}

		protected void btnPrint_Click(object sender, EventArgs e)
		{
			LinkButton btn = (LinkButton)sender;
			int index = int.Parse(btn.CommandArgument);

			//2024-04-05 : CHANGED FROM RECEIPT NUMBER TO PAYMENT ENTRY 
			//Session["PrintDocEntry"] = gvHistory.Rows[index].Cells[5].Text;
			Session["PrintDocEntry"] = gvHistory.Rows[index].Cells[12].Text;

			Session["Title"] = "Cash Register - Payment ";
			Session["ReportName"] = ConfigSettings.ORForm;
			Session["ReportPath"] = ConfigurationManager.AppSettings["ReportPathForms"].ToString();

			//2023-05-08 : PINA ADD NI MS KATE
			string qryLocation = $@"SELECT B.""Name"" FROM OPRJ A INNER JOIN ""@LOCATION"" B ON A.""U_Location"" = B.""Code"" 
                                    WHERE A.""PrjCode"" = '{ViewState["ProjCode"].ToString()}'";
			DataTable dtGetLocation = hana.GetData(qryLocation, hana.GetConnection("SAPHana"));
			string Location = DataAccess.GetData(dtGetLocation, 0, "Name", "").ToString();

			Session["ReportType"] = "Receipt";
			Session["Location"] = Location;


			//if (int.Parse(gvHistory.Rows[index].Cells[0].Text) != 0)
			Session["RptConn"] = "SAP";
			//open new tab
			//Response.Redirect("~/pages/ReportViewer.aspx");
			//ScriptManager.RegisterStartupScript(this, this.GetType(), "openpage", "window.open('ReportViewer.aspx','_blank');", true);
			ScriptManager.RegisterStartupScript(this, this.GetType(), "openpage", "window.open('ReportViewer.aspx');", true);

		}

		protected void btnPymtHistory_Click(object sender, EventArgs e)
		{
			ScriptManager.RegisterStartupScript(this, this.GetType(), "showHistory", "showHistory();", true);
		}

		protected void btnBLedger_Click(object sender, EventArgs e)
		{
			DataTable dt = hana.GetData("SELECT Name,RptName,RptPath FROM ORPT Where RptGroup = 'BL'", hana.GetConnection("SAOHana"));
			if (dt.Rows.Count > 0)
			{
				Session["ReportType"] = "BL";
				Session["BPCode"] = lblID.Text;
				Session["BlockLot"] = $"B{txtBlock.Text} L{txtLot.Text}";
				Session["ReportName"] = dt.Rows[0][1].ToString();
				Session["ReportPath"] = dt.Rows[0][2].ToString();

				//open new tab
				Response.Redirect("~/pages/ReportViewer.aspx");
			}
		}

		protected void btnshowbank_Click(object sender, EventArgs e)
		{
			ScriptManager.RegisterStartupScript(this, this.GetType(), "show", "showBank();", true);
		}

		protected void btnInterbranch_Click(object sender, EventArgs e)
		{
			if (ViewState["ProjCode"] == null)
			{
				alertMsg("Please select project first!", "warning");
				return;
			}
			ClearInterbranch();
			btnAddInter.Text = "Add Interbranch";
			txtInterBranchDate.Focus();
			ScriptManager.RegisterStartupScript(this, this.GetType(), "Interbranch", "showInter();", true);
		}

		protected void btnInterBank_Click(object sender, EventArgs e)
		{
			LoadInterBanks();
			ScriptManager.RegisterStartupScript(this, this.GetType(), "showInterBank", "showInterBank();", true);
		}
		protected void txtInterAmount_TextChanged(object sender, EventArgs e)
		{
			//txtInterAmount.Text = SystemClass.ToCurrency(txtInterAmount.Text);
		}

		protected void btnAddInter_Click(object sender, EventArgs e)
		{
			try
			{
				if (txtInterBranchDate.Value == "")
				{
					alertMsg("Please choose deposit date first!", "warning");
					return;
				}


				if (ViewState["ProjCode"] == null)
				{
					alertMsg("Please select project first!", "warning");
					return;
				}

				string qryPostingPeriodCheck = $@"SELECT 
	                                                *
                                                FROM
	                                                OFPR
                                                WHERE
	                                                '{Convert.ToDateTime(txtInterBranchDate.Value).ToString("yyyyMMdd")}' >= ""F_RefDate"" AND
                                                    '{Convert.ToDateTime(txtInterBranchDate.Value).ToString("yyyyMMdd")}' <= ""T_RefDate""";
				DataTable dtPostingPeriodCheck = hana.GetData(qryPostingPeriodCheck, hana.GetConnection("SAPHana"));
				//BLOCK IF POSTING DATE DOESN'T EXIST IN POSTING PERIODS
				if (DataAccess.Exist(dtPostingPeriodCheck))
				{

					//** check adding blockings **//
					if (string.IsNullOrEmpty(txtInterBranchDate.Value) ||
					string.IsNullOrEmpty(txtInterBankBank.Value) ||
					string.IsNullOrEmpty(txtInterAccounts.Value) ||
					string.IsNullOrEmpty(txtInterAmount.Text))
					{
						alertMsg("Please complete all fields", "warning");
					}
					else
					{
						if (btnAddInter.Text == "Add Interbranch")
						{
							//add check
							DataTable dt = new DataTable();
							dt = (DataTable)ViewState["dtPayments"];

							DataRow dr = dt.NewRow();
							dr[0] = Convert.ToInt32(dt.Rows.Count + 1);
							dr[1] = "Interbank";
							dr[2] = SystemClass.ToCurrency(txtInterAmount.Text);
							dr[26] = txtInterBranchDate.Value;
							dr[27] = txtInterBankBank.Value;
							dr[28] = txtInterBankGLAcc.Value;
							dr[29] = txtInterAccounts.Value;

							dt.Rows.Add(dr);
							ViewState["dtPayments"] = dt;
							LoadData(gvPayments, "dtPayments");
						}
						else
						{
							//update check
							foreach (DataRow dr in ((DataTable)ViewState["dtPayments"]).Rows)
							{
								if (dr[0].ToString() == ViewState["linenum"].ToString())
								{
									dr[1] = "Interbank";
									dr[2] = SystemClass.ToCurrency(txtInterAmount.Text);
									dr[26] = txtInterBranchDate.Value;
									dr[27] = txtInterBankBank.Value;
									dr[28] = txtInterBankGLAcc.Value;
									dr[29] = txtInterAccounts.Value;
									break;
								}
							}

							btnAddInter.Text = "Add Interbranch";
						}

						LoadData(gvPayments, "dtPayments");
						NewComputeTotal(gvPayments);
						loadApplyToPrincipal(0);


						ClearInterbranch();
						ScriptManager.RegisterStartupScript(this, this.GetType(), "showInterBank", "hideInter();", true);
					}


				}
				else
				{
					alertMsg("Date deviates from permissible range. Please contact administrator. (Posting Period)", "info");
				}

			}
			catch (Exception ex)
			{
				alertMsg(ex.Message, "error");
			}

		}
		void LoadInterBanks()
		{
			gvInterBanks.DataSource = ws.GetInterBranch();
			gvInterBanks.DataBind();
		}
		protected void gvInterBanks_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			//if (e.CommandName.Equals("Sel"))
			//{
			//    int row = int.Parse(e.CommandArgument.ToString());
			//    string BankCode = gvBanks.Rows[row].Cells[0].Text;
			//    string BankName = gvBanks.Rows[row].Cells[1].Text;

			//    txtBankCode.Value = BankCode;
			//    txtCheckBank.Value = BankName;


			//}
			//else
			//{
			//    int row = int.Parse(e.CommandArgument.ToString());
			//    string BankName = gvBanks.Rows[row].Cells[1].Text;

			//    txtCheckBank.Value = BankName;
			//}
			//ScriptManager.RegisterStartupScript(this, this.GetType(), "", "hideBank()", true);
		}
		protected void gvInterBanks_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			LoadInterBanks();
			gvInterBanks.PageIndex = e.NewPageIndex;
			gvInterBanks.DataBind();
		}

		protected void btnSelectBank_Click(object sender, EventArgs e)
		{
			LinkButton GetID = (LinkButton)sender;
			string Code = GetID.CommandArgument;

			DataTable dt = hana.GetData($@"SELECT ""BankName"" FROM ""ODSC"" WHERE ""BankCode"" = '{Code}'", hana.GetConnection("SAPHana"));
			if (ViewState["BankType"].ToString() == "HouseBanks")
			{
				//if (btnAddCheck.Text == "Add Check")
				//{
				dt = hana.GetData($@"SELECT ""BankName"" FROM ""ODSC"" WHERE ""BankCode"" = '{Code}'", hana.GetConnection("SAPHana"));
				txtDepositBank.Value = DataAccess.GetData(dt, 0, "BankName", "").ToString();
				txtDepositBankID.Value = Code;
				//}

				//// Hidden Function //
				//else if (btnAddPDC.Text == "Add PDC")
				//{
				//    dt = hana.GetData($@"SELECT ""BankName"" FROM ""ODSC"" WHERE ""BankCode"" = '{Code}'", hana.GetConnection("SAPHana"));
				//    txtDepositBankPDC.Value = DataAccess.GetData(dt, 0, "BankName", "").ToString();
				//    txtDepositBankIdPDC.Value = Code;
				//}
			}
			else
			{

				//if (btnAddCheck.Text == "Add Check")
				//{
				txtBankID.Value = Code;
				txtCheckBank.Value = DataAccess.GetData(dt, 0, "BankName", "").ToString();
				//}
				//// Hidden Function //
				//else if (btnAddPDC.Text == "Add PDC")
				//{
				//    txtBankIdPDC.Value = Code;
				//    txtCheckBankPDC.Value = DataAccess.GetData(dt, 0, "BankName", "").ToString();
				//}


			}
			txtDepositBank.Focus();
			ScriptManager.RegisterStartupScript(this, this.GetType(), "", "hideBank()", true);
		}

		protected void btnCheckHouseBanks_Click(object sender, EventArgs e)
		{
			ViewState["BankType"] = "HouseBanks";
			LoadBanks("HouseBanks");
			ScriptManager.RegisterStartupScript(this, this.GetType(), "", "showBank()", true);
		}

		protected void btnCheckBank_Click(object sender, EventArgs e)
		{
			ViewState["BankType"] = "Banks";
			LoadBanks("Banks");
			txtCheckBankSearch1.Focus();
			ScriptManager.RegisterStartupScript(this, this.GetType(), "", "showBank()", true);


		}

		protected void btnDepositAccount_ServerClick(object sender, EventArgs e)
		{
			//if (btnAddCheck.Text == "Add Check")
			//{
			loadDepositAccount(txtDepositBankID.Value);
			//}
			// Hidden Function //
			//else if (btnAddPDC.Text == "Add PDC")
			//{
			//    loadDepositAccount(txtDepositBankIdPDC.Value);
			//}
			ScriptManager.RegisterStartupScript(this, this.GetType(), "", "showDepositAccounts()", true);
		}

		void loadDepositAccount(string BankCode)
		{
			string qry = $@"SELECT * FROM ""DSC1"" WHERE ""BankCode"" = '{BankCode}'";
			DataTable dt = hana.GetData(qry, hana.GetConnection("SAPHana"));

			gvDepositAccounts.DataSource = dt;
			gvDepositAccounts.DataBind();
		}

		protected void gvDepositAccounts_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			gvDepositAccounts.PageIndex = e.NewPageIndex;
			loadDepositAccount(txtDepositBank.Value);
		}

		protected void btnSelectAccount_Click(object sender, EventArgs e)
		{
			LinkButton GetID = (LinkButton)sender;
			string Account = GetID.CommandArgument;

			//if (btnAddCheck.Text == "Add Check")
			//{
			txtAccount.Value = Account;
			string qry = $@"SELECT ""GLAccount"",""Branch"" FROM ""DSC1"" WHERE ""BankCode"" = '{txtDepositBankID.Value}' AND ""Account"" = '{Account}'  ";
			DataTable dt = hana.GetData(qry, hana.GetConnection("SAPHana"));

			txtCheckAccount.Value = (string)DataAccess.GetData(dt, 0, "GLAccount", "");
			txtDepositedBranch.Value = (string)DataAccess.GetData(dt, 0, "Branch", "");

			//}
			// Hidden Function //
			//else if (btnAddPDC.Text == "Add PDC")
			//{
			//    txtAccountPDC.Value = Account;
			//    string qry = $@"SELECT ""GLAccount"",""Branch"" FROM ""DSC1"" WHERE ""BankCode"" = '{txtDepositBankIdPDC.Value}' AND ""Account"" = '{Account}'  ";
			//    DataTable dt = hana.GetData(qry, hana.GetConnection("SAPHana"));

			//    txtCheckAccountPDC.Value = (string)DataAccess.GetData(dt, 0, "GLAccount", "");
			//    txtDepositedBranchPDC.Value = (string)DataAccess.GetData(dt, 0, "Branch", "");
			//}

			ScriptManager.RegisterStartupScript(this, this.GetType(), "", "hideDepositAccounts()", true);
		}

		protected void btnSelectCreditGLAccounts_Click(object sender, EventArgs e)
		{
			LinkButton GetID = (LinkButton)sender;
			string Account = GetID.CommandArgument;

			txtCreditAccount.Value = Account;
			ScriptManager.RegisterStartupScript(this, this.GetType(), "close", "closeAccounts();", true);

		}

		protected void btnSelectCreditCard_Click(object sender, EventArgs e)
		{
			LinkButton GetID = (LinkButton)sender;
			string CreditCard = GetID.CommandArgument;

			string qry = $@"SELECT ""AcctCode"",""CardName"" FROM ""OCRC"" WHERE ""CreditCard"" = '{CreditCard}' ";
			DataTable dt = hana.GetData(qry, hana.GetConnection("SAPHana"));

			txtCreditCard.Value = (string)DataAccess.GetData(dt, 0, "CardName", "");
			txtCreditAccount.Value = (string)DataAccess.GetData(dt, 0, "AcctCode", "");

			txtCreditMethodCode.Value = "1";
			txtCreditMethod.Value = "CC";

			ScriptManager.RegisterStartupScript(this, this.GetType(), "close", "closeCredit();", true);
		}

		protected void btnSelectInterBanks_Click(object sender, EventArgs e)
		{
			LinkButton GetID = (LinkButton)sender;
			string BankCode = GetID.CommandArgument;

			string qry = $@"SELECT ""Account"",""GLAccount""  FROM ""DSC1"" Where ""BankCode"" = '{BankCode}' AND IFNULL(""UsrNumber4"",'') = 'Yes'";

			txtInterBankBank.Value = BankCode;

			DataTable dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
			txtInterAccounts.Value = (string)DataAccess.GetData(dt, 0, "Account", "");
			txtInterBankGLAcc.Value = (string)DataAccess.GetData(dt, 0, "GLAccount", "");

			ScriptManager.RegisterStartupScript(this, this.GetType(), "", "hideInterBank()", true);
		}

		protected void btnOthers_ServerClick(object sender, EventArgs e)
		{
			if (ViewState["ProjCode"] == null)
			{
				alertMsg("Please select project first!", "warning");
				return;
			}
			ClearOthers();
			btnAddOthers.Text = "Add Payment";
			txtOthersPaymentDate.Focus();
			ScriptManager.RegisterStartupScript(this, this.GetType(), "showOthers", "showOthers();", true);
		}

		protected void btnPrintAR_Click(object sender, EventArgs e)
		{
			LinkButton btn = (LinkButton)sender;
			int index = int.Parse(btn.CommandArgument);

			//2024-04-05 : CHANGED FROM RECEIPT NUMBER TO PAYMENT ENTRY 
			//Session["PrintDocEntry"] = gvHistory.Rows[index].Cells[5].Text;
			Session["PrintDocEntry"] = gvHistory.Rows[index].Cells[12].Text;


			Session["Title"] = "Cash Register - Payment ";
			Session["ReportName"] = ConfigSettings.ARForm;
			Session["ReportPath"] = ConfigurationManager.AppSettings["ReportPathForms"].ToString();

			//2023-05-08 : PINA ADD NI MS KATE
			string qryLocation = $@"SELECT B.""Name"" FROM OPRJ A INNER JOIN ""@LOCATION"" B ON A.""U_Location"" = B.""Code"" 
                                    WHERE A.""PrjCode"" = '{ViewState["ProjCode"].ToString()}'";
			DataTable dtGetLocation = hana.GetData(qryLocation, hana.GetConnection("SAPHana"));
			string Location = DataAccess.GetData(dtGetLocation, 0, "Name", "").ToString();

			Session["ReportType"] = "Receipt";
			Session["Location"] = Location;


			//if (int.Parse(gvHistory.Rows[index].Cells[0].Text) != 0)
			Session["RptConn"] = "SAP";
			//open new tab
			//Response.Redirect("~/pages/ReportViewer.aspx");
			ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openpage", "window.open('ReportViewer.aspx');", true);
		}

		protected void btnPrintPR_Click(object sender, EventArgs e)
		{
			LinkButton btn = (LinkButton)sender;
			int index = int.Parse(btn.CommandArgument);

			//2024-04-05 : CHANGED FROM RECEIPT NUMBER TO PAYMENT ENTRY 
			//Session["PrintDocEntry"] = gvHistory.Rows[index].Cells[5].Text;
			Session["PrintDocEntry"] = gvHistory.Rows[index].Cells[12].Text;

			Session["Title"] = "Cash Register - Payment ";
			Session["ReportName"] = ConfigSettings.PRForm;
			Session["ReportPath"] = ConfigurationManager.AppSettings["ReportPathForms"].ToString();

			//2023-05-08 : PINA ADD NI MS KATE
			string qryLocation = $@"SELECT B.""Name"" FROM OPRJ A INNER JOIN ""@LOCATION"" B ON A.""U_Location"" = B.""Code"" 
                                    WHERE A.""PrjCode"" = '{ViewState["ProjCode"].ToString()}'";
			DataTable dtGetLocation = hana.GetData(qryLocation, hana.GetConnection("SAPHana"));
			string Location = DataAccess.GetData(dtGetLocation, 0, "Name", "").ToString();

			Session["ReportType"] = "Receipt";
			Session["Location"] = Location;


			//if (int.Parse(gvHistory.Rows[index].Cells[0].Text) != 0)
			Session["RptConn"] = "SAP";
			//open new tab
			//Response.Redirect("~/pages/ReportViewer.aspx");
			ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openpage", "window.open('ReportViewer.aspx');", true);
		}

		protected void btnPrintSurcharge_Click(object sender, EventArgs e)
		{
			//REPORT
			Session["PrintDocEntry"] = ViewState["SQDocEntry"].ToString();
			Session["ReportPath"] = ConfigurationManager.AppSettings["ReportPathForms"].ToString();
			Session["ReportName"] = ConfigSettings.SurchargeForm;
			Session["RptConn"] = "SAP";

			//open new tab
			ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openpage", "window.open('ReportViewer.aspx');", true);
		}
		//COMPUTE SURCHARGE WHEN SURCHARGE DATE IS CHANGED
		protected void tSurChargeDate_TextChanged(object sender, EventArgs e)
		{
			//NewComputeTotal(gvPayments);
		}

		protected void btnSurchargeDate_Click(object sender, EventArgs e)
		{
			NewComputeTotal(gvPayments);
			txtOR.Focus();
		}

		void LoadOthersPaymentMean()
		{
			gvOthersPaymentMean.DataSource = ws.GetOthersPaymentMean();
			gvOthersPaymentMean.DataBind();
		}


		protected void btnOthersModeOfPayment_ServerClick(object sender, EventArgs e)
		{
			LoadOthersPaymentMean();
			ScriptManager.RegisterStartupScript(this, this.GetType(), "showOthersPaymentMean", "showOthersPaymentMean();", true);
		}

		protected void gvOthersPaymentMean_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			gvOthersPaymentMean.PageIndex = e.NewPageIndex;
			LoadOthersPaymentMean();
			gvOthersPaymentMean.DataBind();
		}

		protected void btnSelectOthersPaymentMean_Click(object sender, EventArgs e)
		{
			LinkButton GetID = (LinkButton)sender;
			string glCode = GetID.CommandArgument;

			DataTable dt = hana.GetData($@"SELECT * FROM ""@OTHERPAYMENTMEANS"" WHERE ""Code"" = '{glCode}'", hana.GetConnection("SAPHana"));

			txtOthersModeOfPaymentCode.Value = glCode;
			txtOthersModeOfPayment.Value = (string)DataAccess.GetData(dt, 0, "Name", ""); ;
			txtOthersGLAccountCode.Value = (string)DataAccess.GetData(dt, 0, "U_GLAccountCode", "");
			txtOthersGLAccountName.Value = (string)DataAccess.GetData(dt, 0, "U_GLAccountName", "");

			ScriptManager.RegisterStartupScript(this, this.GetType(), "", "hideOthersPaymentMean()", true);
		}

		protected void btnOR_Click(object sender, EventArgs e)
		{
			loadReceiptNo("OR");
			btnFinish.Focus();
		}

		protected void btnPR_Click(object sender, EventArgs e)
		{
			loadReceiptNo("PR");
			btnFinish.Focus();
		}

		protected void btnAR_Click(object sender, EventArgs e)
		{
			loadReceiptNo("AR");
			btnFinish.Focus();
		}

		protected void btnDemandLetter_Click(object sender, EventArgs e)
		{
			ScriptManager.RegisterStartupScript(this, this.GetType(), "showPrint", "showPrint();", true);
		}

		protected void btnDemand1_Click(object sender, EventArgs e)
		{
			try
			{
				//SAVING/UPDATING OF DATE
				string qry = $@"SELECT * FROM ""QUT9"" WHERE ""DocEntry"" = '{ViewState["SQDocEntry"].ToString()}'";
				DataTable dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
				if (dt.Rows.Count <= 0)
				{
					qry = $@"INSERT INTO ""QUT9"" (""DocEntry"",""Level1Date"") VALUES ('{ViewState["SQDocEntry"].ToString()}','{Convert.ToDateTime(txtDemand1.Value).ToString("yyyy-MM-dd")}')";
					hana.Execute(qry, hana.GetConnection("SAOHana"));
				}
				else
				{
					qry = $@"UPDATE QUT9 SET ""Level1Date"" = '{Convert.ToDateTime(txtDemand1.Value).ToString("yyyy-MM-dd")}' Where ""DocEntry"" = '{ViewState["SQDocEntry"].ToString()}'";
					hana.Execute(qry, hana.GetConnection("SAOHana"));
				}

				LoadDemandLetters();

				//REPORT
				Session["PrintDocEntry"] = ViewState["SQDocEntry"].ToString();
				Session["ReportPath"] = ConfigurationManager.AppSettings["ReportPathForms"].ToString();
				Session["ReportName"] = ConfigSettings.DemandLetterForm1;
				Session["RptConn"] = "SAP";
				//open new tab
				ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openpage", "window.open('ReportViewer.aspx');", true);
			}
			catch (Exception ex)
			{
				alertMsg(ex.Message, "error");
			}

		}

		protected void btnDemand2_Click(object sender, EventArgs e)
		{
			try
			{
				//SAVING/UPDATING OF DATE
				string qry = $@"SELECT * FROM ""QUT9"" WHERE ""DocEntry"" = '{ViewState["SQDocEntry"].ToString()}'";
				DataTable dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
				if (dt.Rows.Count <= 0)
				{
					qry = $@"INSERT INTO ""QUT9"" (""DocEntry"",""Level2Date"") VALUES ('{ViewState["SQDocEntry"].ToString()}','{Convert.ToDateTime(txtDemand2.Value).ToString("yyyy-MM-dd")}')";
					hana.Execute(qry, hana.GetConnection("SAOHana"));
				}
				else
				{
					qry = $@"UPDATE QUT9 SET ""Level2Date"" = '{Convert.ToDateTime(txtDemand2.Value).ToString("yyyy-MM-dd")}' Where ""DocEntry"" = '{ViewState["SQDocEntry"].ToString()}'";
					hana.Execute(qry, hana.GetConnection("SAOHana"));
				}

				LoadDemandLetters();

				//REPORT
				Session["PrintDocEntry"] = ViewState["SQDocEntry"].ToString();
				Session["ReportPath"] = ConfigurationManager.AppSettings["ReportPathForms"].ToString();
				Session["ReportName"] = ConfigSettings.DemandLetterForm2;
				Session["RptConn"] = "SAP";

				//open new tab
				ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openpage", "window.open('ReportViewer.aspx');", true);
			}
			catch (Exception ex)
			{
				alertMsg(ex.Message, "error");
			}

		}

		protected void btnDemand3_Click(object sender, EventArgs e)
		{
			try
			{
				//SAVING/UPDATING OF DATE
				string qry = $@"SELECT * FROM ""QUT9"" WHERE ""DocEntry"" = '{ViewState["SQDocEntry"].ToString()}'";
				DataTable dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
				if (dt.Rows.Count <= 0)
				{
					qry = $@"INSERT INTO ""QUT9"" (""DocEntry"",""Level3Date"") VALUES ('{ViewState["SQDocEntry"].ToString()}','{Convert.ToDateTime(txtDemand3.Value).ToString("yyyy-MM-dd")}')";
					hana.Execute(qry, hana.GetConnection("SAOHana"));
				}
				else
				{
					qry = $@"UPDATE QUT9 SET ""Level3Date"" = '{Convert.ToDateTime(txtDemand3.Value).ToString("yyyy-MM-dd")}' Where ""DocEntry"" = '{ViewState["SQDocEntry"].ToString()}'";
					hana.Execute(qry, hana.GetConnection("SAOHana"));
				}

				LoadDemandLetters();

				//REPORT
				Session["PrintDocEntry"] = ViewState["SQDocEntry"].ToString();
				Session["ReportPath"] = ConfigurationManager.AppSettings["ReportPathForms"].ToString();
				Session["ReportName"] = ConfigSettings.DemandLetterForm3;
				Session["RptConn"] = "SAP";

				//open new tab
				ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openpage", "window.open('ReportViewer.aspx');", true);
			}
			catch (Exception ex)
			{
				alertMsg(ex.Message, "error");
			}
		}

		protected void btnDemand4_Click(object sender, EventArgs e)
		{
			try
			{
				//SAVING/UPDATING OF DATE
				string qry = $@"SELECT * FROM ""QUT9"" WHERE ""DocEntry"" = '{ViewState["SQDocEntry"].ToString()}'";
				DataTable dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
				if (dt.Rows.Count <= 0)
				{
					qry = $@"INSERT INTO ""QUT9"" (""DocEntry"",""Level4Date"") VALUES ('{ViewState["SQDocEntry"].ToString()}','{Convert.ToDateTime(txtDemand4.Value).ToString("yyyy-MM-dd")}')";
					hana.Execute(qry, hana.GetConnection("SAOHana"));
				}
				else
				{
					qry = $@"UPDATE QUT9 SET ""Level4Date"" = '{Convert.ToDateTime(txtDemand4.Value).ToString("yyyy-MM-dd")}' Where ""DocEntry"" = '{ViewState["SQDocEntry"].ToString()}'";
					hana.Execute(qry, hana.GetConnection("SAOHana"));
				}

				LoadDemandLetters();

				//REPORT
				Session["PrintDocEntry"] = ViewState["SQDocEntry"].ToString();
				Session["ReportPath"] = ConfigurationManager.AppSettings["ReportPathForms"].ToString();
				Session["ReportName"] = ConfigSettings.DemandLetterForm4;
				Session["RptConn"] = "SAP";

				//open new tab
				ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openpage", "window.open('ReportViewer.aspx');", true);
			}
			catch (Exception ex)
			{
				alertMsg(ex.Message, "error");
			}
		}

		protected void btnDemand5_Click(object sender, EventArgs e)
		{
			try
			{
				//SAVING/UPDATING OF DATE
				string qry = $@"SELECT * FROM ""QUT9"" WHERE ""DocEntry"" = '{ViewState["SQDocEntry"].ToString()}'";
				DataTable dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
				if (dt.Rows.Count <= 0)
				{
					qry = $@"INSERT INTO ""QUT9"" (""DocEntry"",""Level5Date"") VALUES ('{ViewState["SQDocEntry"].ToString()}','{Convert.ToDateTime(txtDemand5.Value).ToString("yyyy-MM-dd")}')";
					hana.Execute(qry, hana.GetConnection("SAOHana"));
				}
				else
				{
					qry = $@"UPDATE QUT9 SET ""Level5Date"" = '{Convert.ToDateTime(txtDemand5.Value).ToString("yyyy-MM-dd")}' Where ""DocEntry"" = '{ViewState["SQDocEntry"].ToString()}'";
					hana.Execute(qry, hana.GetConnection("SAOHana"));
				}

				LoadDemandLetters();

				//REPORT
				Session["PrintDocEntry"] = ViewState["SQDocEntry"].ToString();
				Session["ReportPath"] = ConfigurationManager.AppSettings["ReportPathForms"].ToString();
				Session["ReportName"] = ConfigSettings.DemandLetterForm5;
				Session["RptConn"] = "SAP";

				//open new tab
				ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openpage", "window.open('ReportViewer.aspx');", true);
			}
			catch (Exception ex)
			{
				alertMsg(ex.Message, "error");
			}
		}

		protected void btnCheckBankSearch_Click(object sender, EventArgs e)
		{
			if (ViewState["BankType"].ToString() == "Banks")
			{
				LoadBankSearch("Banks");
			}
			else
			{
				LoadBankSearch("HouseBanks");
			}
		}

		void LoadBankSearch(string type)
		{
			if (type == "HouseBanks")
			{
				if (String.IsNullOrEmpty(txtCheckBankSearch1.Text))
				{
					gvBanks.DataSource = ws.GetHouseBanks();
				}
				else
				{
					gvBanks.DataSource = ws.GetHouseBanksSearch(txtCheckBankSearch1.Text);
				}
				gvBanks.DataBind();
			}
			else
			{
				if (String.IsNullOrEmpty(txtCheckBankSearch1.Text))
				{
					gvBanks.DataSource = ws.GetBanks();
				}
				else
				{
					gvBanks.DataSource = ws.GetBanksSearch(txtCheckBankSearch1.Text);
				}
				gvBanks.DataBind();
			}
		}

		protected void btnSearchInter_Click(object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty(txtSearchInter.Value))
			{
				LoadInterBanks();
			}
			else
			{
				gvInterBanks.DataSource = ws.GetInterBranchSearch(txtSearchInter.Value);
			}
			gvInterBanks.DataBind();
		}


		protected void btnPDCPrint_Click(object sender, EventArgs e)
		{
			LinkButton btn = (LinkButton)sender;
			int index = int.Parse(btn.CommandArgument);
			DataTable dtPDCList = (DataTable)ViewState["PDCList"];


			//Session["PrintDocEntry"] = ViewState["PDCList"].Rows[index].Cells[6].Text;  
			Session["PrintDocEntry"] = dtPDCList.Rows[index]["ARPDCNo"];
			Session["Title"] = "Cash Register - Payment ";
			Session["ReportName"] = ConfigSettings.PDCForm;
			Session["ReportPath"] = ConfigurationManager.AppSettings["ReportPathForms"].ToString();
			Session["ReportType"] = "";

			//if (int.Parse(gvSelectPDC.Rows[index].Cells[0].Text) != 0)

			//2023-07-06 : UPDATE TO SAP CONNECTION
			//Session["RptConn"] = "Addon";
			Session["RptConn"] = "SAP";


			//open new tab
			//Response.Redirect("~/pages/ReportViewer.aspx");
			ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openpage", "window.open('ReportViewer.aspx');", true);
		}

		protected void btnSearchHistory_Click(object sender, EventArgs e)
		{
			var data = ws.GetPaymentHistorySearch(int.Parse(ViewState["SQDocEntry"].ToString()), txtSearchHistory.Value);
			if (data != null)
			{
				gvHistory.DataSource = data;
				gvHistory.DataBind();
				HideColumnInHistory();
			}

		}

		protected void gvDownPayments_RowCommand(object sender, GridViewCommandEventArgs e)
		{

			//GridViewRow row = (GridViewRow)(((CheckBox)e.CommandSource).NamingContainer);
			//int index = row.RowIndex;
			//CheckBox chk = (CheckBox)gvDownPayments.Rows[index].FindControl("chkSel");

			//if (e.CommandName == "chk")
			//{
			//    string paymentType = gvDownPayments.Rows[index].Cells[3].Text;
			//    if (paymentType == "MISC")
			//    {
			//        chkTagMISC++;
			//    }
			//    else
			//    {
			//        chkTagDP++;
			//    }


			//    if ((chkTagDP >= 0 && chkTagMISC == 0) || (chkTagMISC >= 0 && chkTagDP == 0))
			//    {
			//        chk.Checked = true;
			//    }
			//    else
			//    {
			//        chk.Checked = false;
			//        alertMsg("Please select only ", "warning");
			//    }



			//}


		}

		protected void chkSel_CheckedChanged(object sender, EventArgs e)
		{

			CheckBox cb = (CheckBox)sender;

			GridViewRow row = cb.NamingContainer as GridViewRow;
			int index = row.RowIndex;
			string paymentType = gvDownPayments.Rows[index].Cells[3].Text;
			string term = gvDownPayments.Rows[index].Cells[2].Text;
			double cashDiscAmount = Convert.ToDouble(gvDownPayments.Rows[index].Cells[10].Text);

			divAdvancePrincipal.Visible = false;



			//FOR COMPUTATION
			if (cb.Checked)
			{

				//if (!divAdvancePrincipal.Visible)
				//{
				//    lblApplyToPrincipal.Text = "NO";
				//}




				if (paymentType == "MISC")
				{
					ViewState["chkTagMISC"] = Convert.ToInt32(ViewState["chkTagMISC"].ToString()) + 1;
				}
				else
				{
					ViewState["chkTagDP"] = Convert.ToInt32(ViewState["chkTagDP"].ToString()) + 1;



					ViewState["AdvancePrincipal"] = 1;
					CashDiscountAddPayment();

					//ADVANCE PAYMENT SETUP - 2022-09-10
					if ((Convert.ToDateTime(gvDownPayments.Rows[index].Cells[4].Text) > Convert.ToDateTime(tDocDate.Value)) && paymentType == "LB")
					{
						ViewState["ctrFordivAdvancePrincipal"] = Convert.ToInt32(ViewState["ctrFordivAdvancePrincipal"]) + 1;
						//divAdvancePrincipal.Visible = true;
					}

				}

				var test1 = ViewState["chkTagDP"];
				var test2 = ViewState["chkTagMISC"];


				if (
					((Convert.ToInt32(ViewState["chkTagDP"]) >= Convert.ToInt32(ViewState["chkTagMISC"])) && Convert.ToInt32(ViewState["chkTagMISC"]) > 0) ||
					((Convert.ToInt32(ViewState["chkTagMISC"]) >= Convert.ToInt32(ViewState["chkTagDP"])) && Convert.ToInt32(ViewState["chkTagDP"]) > 0)
				   )
				{
					cb.Checked = false;
					ViewState["ctrFordivAdvancePrincipal"] = Convert.ToInt32(ViewState["ctrFordivAdvancePrincipal"]) - 1;

					//divAdvancePrincipal.Visible = false;
					lblApplyToPrincipal.Text = "NO";

					alertMsg("Payment for different type of transactions is not allowed.", "warning");

					if (paymentType == "MISC")
					{
						ViewState["chkTagMISC"] = 0;
					}
					else
					{
						ViewState["AdvancePrincipal"] = 0;
						ViewState["chkTagDP"] = 0;
					}


				}
				else
				{

					//2023-07-05 : COMMENTED REMOVED THIS CONDITION SINCE IT IS ALREADY FINALIZED ON CODES BELOW
					//divAdvancePrincipal.Visible = true;
					cb.Checked = true;
				}
			}
			else
			{
				if (paymentType == "MISC")
				{
					ViewState["chkTagMISC"] = Convert.ToInt32(ViewState["chkTagMISC"].ToString()) - 1;
				}
				else
				{
					CashDiscountRemovePayment(term, paymentType, cashDiscAmount);

					ViewState["chkTagDP"] = Convert.ToInt32(ViewState["chkTagDP"].ToString()) - 1;
				}
				ViewState["ctrFordivAdvancePrincipal"] = Convert.ToInt32(ViewState["ctrFordivAdvancePrincipal"]) - 1;
			}




			//2023-06-06 : REMOVED THIS CONDITION 
			//var param = ViewState["ctrFordivAdvancePrincipal"];
			//if (Convert.ToInt32(ViewState["ctrFordivAdvancePrincipal"]) > 0)
			//{
			//    divAdvancePrincipal.Visible = true;
			//}
			//else
			//{
			//    divAdvancePrincipal.Visible = false;
			//}

			NewComputeTotal(gvPayments);

			//2023-06-06 : ADDED THIS CONDITION 
			if (ViewState["MinLBDate"].ToString() == "")
			{
				divAdvancePrincipal.Visible = false;
			}
			else
			{
				divAdvancePrincipal.Visible = true;
			}

		}





		void CashDiscountAddPayment()
		{
			//CHECK CASH DISCOUNT
			foreach (GridViewRow row in gvDownPayments.Rows)
			{
				string LineStatus = row.Cells[16].Text;
				CheckBox chkSel = (CheckBox)gvDownPayments.Rows[row.RowIndex].FindControl("chkSel");

				if (LineStatus != "C")
				{
					if (row.Cells[11].Text != "&nbsp;")
					{
						DateTime cashDiscDate = Convert.ToDateTime(row.Cells[11].Text);

						if (cashDiscDate <= Convert.ToDateTime(tDocDate.Value))
						{
							//NO CASH DISCOUNT
							row.Cells[10].Text = "0.00";
						}
						else
						{
							if (chkSel.Checked)
							{
								DataTable dtCash = new DataTable();
								dtCash = (DataTable)ViewState["dtPayments"];

								DataRow dr = dtCash.NewRow();
								dr[0] = Convert.ToInt32(dtCash.Rows.Count + 1);
								dr[1] = "Cash";
								dr[2] = SystemClass.ToCurrency(row.Cells[10].Text);
								dr[41] = SystemClass.ToCurrency(row.Cells[2].Text);
								dr[42] = row.Cells[3].Text;

								dtCash.Rows.Add(dr);
								ViewState["dtPayments"] = dtCash;
								LoadData(gvPayments, "dtPayments");
							}
						}
					}
				}
			}

		}


		void CashDiscountRemovePayment(string term, string type, double cashDiscountAmount)
		{
			DataTable dt = (DataTable)ViewState["dtPayments"];
			//** delete selected payments
			foreach (DataRow dr in dt.Rows)
			{
				if (dr["CashDiscountTaggingTerm"].ToString() == term && dr["CashDiscountTaggingType"].ToString() == type && Convert.ToDouble(dr["Amount"].ToString()) == cashDiscountAmount)
				{
					dr.Delete();
				}
			}
			LoadData(gvPayments, "dtPayments");

		}


		protected void btnCheckDate_Click(object sender, EventArgs e)
		{
			try
			{
				if (Convert.ToDateTime(Convert.ToDateTime(txtCheckDate.Text).ToString("yyyy-MM-dd")) >
			  Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
				{
					divARPDCNo.Visible = true;
					txtARPDCNo.Focus();
				}
				else
				{
					divARPDCNo.Visible = false;
					txtCheckAmount.Focus();
				}
			}
			catch (Exception ex)
			{

			}
		}

		protected void gvCardBrandAccount_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			int index = Convert.ToInt32(e.CommandArgument);
			if (e.CommandName.Equals("Sel"))
			{
				txtCardBrandAccountCode.Value = gvCardBrandAccount.Rows[index].Cells[0].Text;
				txtCardBrandAccountName.Value = gvCardBrandAccount.Rows[index].Cells[1].Text;
			}
			ScriptManager.RegisterStartupScript(this, this.GetType(), "close", "closeCardBrandAccount();", true);
		}

		protected void btnDeletePDC_Click(object sender, EventArgs e)
		{
			try
			{


				string DocEntry = ViewState["SQDocEntry"].ToString();
				int tag = 0; //TAGGING FOR PDC REMOVAL
				int tag2 = 0; //TAGGING IF ANY CHECKBOX IS TICKED
				string errMsg = "";

				foreach (GridViewRow rows in gvSelectPDC.Rows)
				{
					CheckBox chkSel = (CheckBox)gvSelectPDC.Rows[rows.RowIndex].FindControl("chkSelPDC");
					double CheckSum = Convert.ToDouble(rows.Cells[1].Text);
					double CheckSumOrig = Convert.ToDouble(rows.Cells[11].Text);

					if (chkSel.Checked)
					{
						tag2++;

						if (Math.Round(CheckSum, 2) == Math.Round(CheckSumOrig, 2))
						{
							//2023-05-03 COMMENTED : INSTEAD OF UPDATING AMOUNT TO 0, REMOVE FROM THE TABLE ROW INSTEAD
							//string qry = $@" UPDATE ""QUT8"" SET ""CheckSum"" = 0 WHERE ""Id"" = {rows.Cells[10].Text}";

							string qry = $@" DELETE FROM ""QUT8"" WHERE ""Id"" = {rows.Cells[10].Text}";


							if (!hana.Execute(qry, hana.GetConnection("SAOHana")))
							{
								tag++;
							}
						}
						else
						{
							tag++;
							errMsg = "Cannot delete used PDC. Please contact Administrator.";
						}
					}
				}

				LoadPDCList();

				//2023-05-03 : CHECK IF ANY CHECKBOX IS TICKED
				if (tag2 > 0)
				{

					if (tag == 0)
					{

						alertMsg("PDC(s) successfully removed.", "success");
					}
					else
					{
						if (string.IsNullOrWhiteSpace(errMsg))
						{
							errMsg = "Operation failed. Please contact administrator.";
						}
						alertMsg(errMsg, "info");
					}
				}
				else
				{
					alertMsg("No PDC selected for deletion.", "info");
				}

			}
			catch (Exception ex)
			{
				alertMsg(ex.Message, "warning");
			}
		}

		protected void btnApplyToPrincipal_Click(object sender, EventArgs e)
		{
			confirmation("Are you sure you want to process Advance Payment - Apply to Principal?", "ApplyToPrincipal");
		}



		private void loadApplyToPrincipal(int ifFromButton)
		{
			try
			{
				if (lblApplyToPrincipal.Text == "NO" && divAdvancePrincipal.Visible == true)
				{

					DataTable dt = dtComputeSheet("GetGeneralData");
					int interestRate = int.Parse(DataAccess.GetData(dt, 0, "InterestRate", "0").ToString());
					string LoanType = DataAccess.GetData(dt, 0, "LoanType", "").ToString();
					string ProductType = DataHelper.DataTableRet(dt, 0, "ProductType", "");
					string RetitlingType = DataAccess.GetData(dt, 0, "RetitlingType", "").ToString();
					int LTerms = int.Parse(DataAccess.GetData(dt, 0, "LTerms", "1").ToString());
					int DPTerms = int.Parse(DataAccess.GetData(dt, 0, "DPTerms", "0").ToString());
					int MiscDPTerms = int.Parse(DataAccess.GetData(dt, 0, "MiscDPTerms", "0").ToString());
					double MiscDPAmount = double.Parse(DataAccess.GetData(dt, 0, "MiscDPAmount", "0").ToString());
					double MiscDPMonthly = double.Parse(DataAccess.GetData(dt, 0, "MiscFeesMonthly", "0").ToString());
					int MiscLBTerms = int.Parse(DataAccess.GetData(dt, 0, "MiscLBTerms", "0").ToString());
					double MiscLBAmount = double.Parse(DataAccess.GetData(dt, 0, "MiscLBAmount", "0").ToString());
					double MiscLBMonthly = double.Parse(DataAccess.GetData(dt, 0, "MiscLBMonthly", "0").ToString());
					string MiscDueDate = DataAccess.GetData(dt, 0, "MiscDueDate", "1999-12-31").ToString();
					double LBMonthly = double.Parse(DataAccess.GetData(dt, 0, "MonthlyLB", "0").ToString());
					//string DPDueDate = DataAccess.GetData(dt, 0, "DPDueDate", "1999-12-31").ToString();
					//lblDPDueDate.Value = DPDueDate;

					loadSchedules(interestRate, LoanType,
								  ProductType, RetitlingType,
								  LTerms, DPTerms,
								  MiscDPTerms, MiscDPAmount,
								  MiscDPMonthly, MiscLBTerms,
								  MiscLBAmount, MiscLBMonthly,
								  MiscDueDate, LBMonthly
								  );

					if (lblApplyToPrincipal.Text == "YES")
					{
						if (ifFromButton == 1)
						{
							gvDownPayment.DataSource = null;
							gvDownPayment.DataBind();
							gvAmortization.DataSource = null;
							gvAmortization.DataBind();

							divAdvancePaymentscheduleAccordion.Visible = false;


							lblApplyToPrincipal.Text = "NO";
							lblApplyToPrincipal.ForeColor = System.Drawing.Color.Black;
						}

					}
					else
					{
						if (ifFromButton == 1)
						{
							divAdvancePaymentscheduleAccordion.Visible = true;
							lblApplyToPrincipal.Text = "YES";
							lblApplyToPrincipal.ForeColor = System.Drawing.Color.Red;


						}
					}

				}
				else
				{
					gvDownPayment.DataSource = null;
					gvDownPayment.DataBind();
					gvAmortization.DataSource = null;
					gvAmortization.DataBind();

					divAdvancePaymentscheduleAccordion.Visible = false;


					lblApplyToPrincipal.Text = "NO";
					lblApplyToPrincipal.ForeColor = System.Drawing.Color.Black;
				}

				NewComputeTotal(gvPayments);

			}
			catch (Exception ex)
			{
				alertMsg(ex.Message, "info");
			}
		}


		protected double GetQUT13Amount(double currentPayment, double principal, double interest, double surcharge, double IPS, double previousPayment,
										//2023-10-16 : add amounts for additional checking
										double surchargePosted, double interestPosted
			)
		{
			double qut13Amount = 0;

			#region oldComputation
			////if current payment is greater than or equal the sumation of principal, interst, and surcharge
			//if (currentPayment >= (principal + interest + surcharge + IPS))
			//{
			//    qut13Amount = principal;
			//}

			////if current payment is less than the sumation of principal, interst, and surcharge AND
			////summation of principal, interest, and surcharge minus previous payment is less than or equal principal AND
			////no previous partial payment   
			//else if ((currentPayment < (principal + interest + surcharge + IPS)) &&
			//            previousPayment != 0
			//            )
			//{
			//    if (((principal + interest + surcharge + IPS) - previousPayment) <= principal)
			//    {
			//        //2023-10-06 : ADD CURRENTPAYMENT TO PREVIOUS PAYMENT
			//        //qut13Amount = (principal + interest + surcharge + IPS) - (previousPayment);
			//        qut13Amount = (principal + interest + surcharge + IPS) - (previousPayment + currentPayment);
			//    }
			//    //2023-10-06 : ADD CONDITION IF ALL PAYMENTS IS GREATER THAN OR EQUAL OF BALACE
			//    if ((previousPayment + currentPayment) >= (principal + interest + surcharge + IPS))
			//    {
			//        qut13Amount = currentPayment;
			//    }
			//}

			////if current payment is less than the summation of principal, interest, and surcharge minus to the previous payment 
			//else if (currentPayment < ((principal + interest + surcharge + IPS) - previousPayment))
			//{
			//    qut13Amount = currentPayment - ((interest + surcharge + IPS));
			//}
			//else
			//{
			//    qut13Amount = 0;
			//}
			#endregion

			if (currentPayment >= (Math.Round((principal + interest + surcharge + IPS), 2)))
			{
				qut13Amount = principal;
			}
			else if (currentPayment < (Math.Round((principal + interest + surcharge + IPS), 2)))
			{
				if (Math.Round((interest + surcharge + IPS), 2) >= Math.Round((previousPayment + currentPayment), 2))
				{
					qut13Amount = 0;
				}
				else if (previousPayment == 0)
				{
					if (currentPayment < (Math.Round(principal + interest + surcharge + IPS, 2)))
					{
						//2024-10-22 : JMC : ADDED CONDITION 
						//IF CURRENT PAYMENT IS LESS THAN OR EQUAL NON-TCP PAYMENT  
						if (currentPayment <= Math.Round((interest + surcharge + IPS), 2))
						{
							qut13Amount = Math.Round((((Math.Round((interest + surcharge + IPS), 2))) - currentPayment), 2);
						}
						//IF CURRENT PAYMENT IS LESS THAN OR EQUAL NON-TCP PAYMENT IS GREATER
						else if (currentPayment <= (Math.Round((principal + interest + surcharge + IPS), 2)))
						{
							qut13Amount = Math.Round(currentPayment - Math.Round((interest + surcharge), 2), 2);
						}
						else
						{
							var test = currentPayment;
							var test2 = Math.Round((principal + interest + surcharge + IPS), 2);
							//qut13Amount = currentPayment;

							//2023-12-11 : ADJUST SAVING OF AMOUNT TO QUT13 (raised by Ms. Kate)
							//qut13Amount = ((Math.Round(principal + interest + surcharge + IPS, 2))) - currentPayment;
							qut13Amount = test;
						}


					}
					else
					{
						qut13Amount = principal;
					}
				}
				else
				{
					if ((Math.Round((principal + interest + surcharge + IPS), 2)) <= principal)
					{
						qut13Amount = currentPayment;
					}
					else
					{
						if (((Math.Round((principal + interest + surcharge + IPS), 2)) - (Math.Round((previousPayment + currentPayment), 2))) > principal)
						{
							qut13Amount = Math.Round(currentPayment - ((Math.Round(principal + interest + surcharge + IPS, 2))), 2);
						}
						else
						{
							//2023-10-16 : add amounts for additional checking
							double tempAmount = 0;

							//2023-10-16 : if surcharge+interest posted is higher than the currentpayment
							tempAmount = Math.Round(currentPayment - Math.Round((surchargePosted + interestPosted), 2), 2);
							if (tempAmount < 0)
							{
								qut13Amount = 0;
							}
							else
							{
								qut13Amount = tempAmount;
							}
							//qut13Amount = currentPayment;
						}
					}
				}
			}



			return qut13Amount;
		}




		void cancelCWTs()
		{
			try
			{

				CashRegisterService cashRegister = new CashRegisterService();
				int JournalEntryNo = 0;
				string Message = "";

				#region COMMENTED 
				//2023-07-25 : ADDITIONAL JEs TO BE CANCELLEDD
				//string qry = $@"SELECT	
				//                    B.""TransId"", 
				//                    A.""U_ORNo"", 
				//                    B.""Ref3"", 
				//                    B.""RefDate"",
				//                    B.""Memo"", 
				//                    B.""Project"",
				//                    B.""U_BlockNo"", 
				//                    B.""U_LotNo"", 
				//                    A.""CardCode"", B.""Ref2""
				//                FROM 
				//                    ORCT A INNER JOIN 
				//                    OJDT B ON IFNULL(A.""U_ORNo"",IFNULL(A.""U_ARNo"", A.""U_PRNo"")) = B.""U_ORNo"" 
				//                WHERE
				//                    A.""Canceled"" = 'Y' AND 
				//                    IFNULL(A.""U_PaymentType"",'') <> '' AND 
				//                    IFNULL(A.""U_RestructCode"",'') = '' AND 
				//                    IFNULL(B.""U_CancelTag"",'') = '' AND 
				//                    IFNULL(B.""Ref3"",'') IN ('0', '27', '28', '22') AND 
				//                    IFNULL(B.""U_ORNo"",'') <> ''  AND
				//                    IFNULL(A.""U_ContractStatus"",'Open') IN ('Open','Closed')
				//                order by 
				//                    A.""DocEntry"" desc";
				#endregion

				//2023-07-31 : ADDED MORE JEs
				//2023-08-02 : ADDED 30
				//2023-11-30 : ADDED CANCELLATION DATE
				string qry = $@"SELECT	
                                    B.""TransId"", 
                                    A.""U_ORNo"", 
                                    B.""Ref3"", 
                                    B.""RefDate"",
                                    B.""Memo"", 
                                    B.""Project"",
                                    B.""U_BlockNo"", 
                                    B.""U_LotNo"", 
                                    A.""CardCode"", B.""Ref2"",
                                    IFNULL(B.""Ref3"",'') ""Ref3"",
                                    B.""SysTotal"",
                                    A.""CancelDate""  
                                FROM 
                                    ORCT A INNER JOIN 
                                    OJDT B ON IFNULL(A.""U_ORNo"",IFNULL(A.""U_ARNo"", A.""U_PRNo"")) = B.""U_ORNo"" 
                                WHERE
                                    A.""Canceled"" = 'Y' AND 
                                    IFNULL(A.""U_PaymentType"",'') <> '' AND 
                                    IFNULL(A.""U_RestructCode"",'') = '' AND 
                                    IFNULL(B.""U_CancelTag"",'N') = 'N' AND 
                                    IFNULL(B.""Ref3"",'') IN ('0', '27', '28', '22', '11', '13','14','3','4','9', '29','30') AND 
                                    IFNULL(B.""U_ORNo"",'') <> ''  AND
                                    IFNULL(A.""U_ContractStatus"",'Open') IN ('Open','Closed')
                                order by 
                                    A.""DocEntry"" desc";
				DataTable dt = hana.GetData(qry, hana.GetConnection("SAPHana"));


				if (dt.Rows.Count > 0)
				{
					foreach (DataRow row in dt.Rows)
					{
						string Memo = row["Memo"].ToString();

						//2023-11-30 : CHANGED FROM REFDATE TO CANCELLATION DATE
						//string RefDate = row["RefDate"].ToString();
						string RefDate = row["CancelDate"].ToString();


						string Project = row["Project"].ToString();
						string Block = row["U_BlockNo"].ToString();
						string Lot = row["U_LotNo"].ToString();
						string ORNo = row["U_ORNo"].ToString();
						string DocNum = row["Ref2"].ToString();
						string SapCardCode = row["CardCode"].ToString();

						//2023-07-25 : MAKE TRANSCODE DYNAMIC
						string Ref3 = row["Ref3"].ToString();

						string Account1 = "";
						double Debit = double.Parse(row["SysTotal"].ToString());

						string TransID = row["TransId"].ToString();

						//2023-08-01 : UPDATED FROM 2 COLUMNS TO ALL
						//qry = $@" SELECT x.""Debit"", x.""Account"" FROM JDT1 x WHERE x.""TransId"" = {TransID} ";
						qry = $@" SELECT x.* FROM JDT1 x WHERE x.""TransId"" = {TransID} ";
						DataTable dtRows = hana.GetData(qry, hana.GetConnection("SAPHana"));

						////2023-08-01
						//foreach (DataRow row1 in dtRows.Rows)
						//{
						//    Debit = double.Parse(row1["Debit"].ToString());
						//    if (Debit > 0)
						//    {
						//        Account1 = row1["Account"].ToString();
						//        break;
						//    }
						//}




						//2023-07-25 : MAKE TRANSCODE DYNAMIC
						string TransCode = "";
						if (Ref3 == "0" || Ref3 == "27" || Ref3 == "28" || Ref3 == "22" || Ref3 == "14" || Ref3 == "30")
						{
							TransCode = "CNCW";
						}
						//2023-08-02 : ADDED 29 
						else if (Ref3 == "11" || Ref3 == "4" || Ref3 == "29")
						{
							TransCode = "CNSL";
						}
						else
						{
							TransCode = "CNCO";
						}


						//if (!string.IsNullOrWhiteSpace(Account1))
						//{
						if (cashRegister.CreateJournalEntry(null, Project, SapCardCode, Debit, 0, "",
														Ref3, //2023-08-01 : REUSED VARIABLE FOR TAGGING OF CONNECTED JE 

													   Block, Lot, Account1, "", "", "25TCP99", DocNum, "0", "", "",

													   //2023-07-25 : DYNAMIC TRANSCODE
													   //Convert.ToDateTime(RefDate).ToString("yyyyMMdd"), "CNCW", out JournalEntryNo, out Message, ORNo))

													   //2023-08-01 : PASS DATATABLE FOR CANCELLATION
													   //Convert.ToDateTime(RefDate).ToString("yyyyMMdd"), TransCode, out JournalEntryNo, out Message, ORNo))
													   Convert.ToDateTime(RefDate).ToString("yyyyMMdd"), TransCode, out JournalEntryNo, out Message, ORNo, 0, dtRows, 0, TransID))
						{
							hana.Execute($@"UPDATE ""OJDT"" SET ""U_CancelTag"" = 'Y' WHERE ""TransId"" = '{TransID}'", hana.GetConnection("SAPHana"));
						}
						else
						{
							alertMsg("An error occurred while posting Journal Entries . Please contact administrator.", "warning");
						}
						//}
					}
				}
			}
			catch (Exception ex)
			{
				alertMsg($@"Syncing problem. Please contact adminsitrator. -- ({ex.Message})", "warning");
			}
		}










		void loadSchedules(int interestRate, string LoanType,
						   string ProductType, string RetitlingType,
						   int LTerms, int DPTerms,
						   int MiscDPTerms, double MiscDPAmount,
						   double MiscDPMonthly, int MiscLBTerms,
						   double MiscLBAmount, double MiscLBMonthly,
						   string MiscDueDate, double LBMonthly
						   )
		{
			//###########################################
			//################ SCHEDULES ################ 
			//###########################################


			//MA SCHEDULE   
			string qry = $@"SELECT * FROM ""@LOANTYPE"" WHERE ""Code"" = '{LoanType}'";
			DataTable dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
			string oneLinertag = DataAccess.GetData(dt, 0, "U_OneLiner", "N").ToString();

			int addLBTermsToDP = 1;
			int addDPTermsToLB = 0;

			if (oneLinertag != "Y")
			{
				addLBTermsToDP = LTerms;
			}

			if (double.Parse(lblTCPMonthly2n.Value) > 0)
			{
				//divMonthlyDP.Visible = true;
				ViewState["gvDownPayment"] = ws.PaymentBreakdown(DPTerms,
										Convert.ToDateTime(lblDPDueDate.Value),
										double.Parse(lblTCPMonthly2n.Value),
										0,
										double.Parse(lblBalance2n.Value),
										"DP",
										interestRate,
										double.Parse(lblMiscFees2n.Value),
										0,
										addLBTermsToDP
										).Tables["PaymentBreakdown"];
				DataTable ekek = (DataTable)ViewState["gvDownPayment"];
				LoadData(gvDownPayment, "gvDownPayment");

				//divMonthlyDP.Visible = true;
			}
			else
			{
				divMonthlyDP.Visible = false;
				ViewState["gvDownPayment"] = null;
			}























			if (ProductType.ToUpper() == "LOT ONLY" && RetitlingType == "BUYERS")
			{
				//FOR SINGLE MISCELLANEOUS

				//MISC SCHEDULE
				ViewState["gvMiscellaneous"] = ws.PaymentBreakdown(1,
											   Convert.ToDateTime(MiscDueDate),
											   MiscDPMonthly,
											   0,
											   double.Parse(lblMiscFees2n.Value),
											   "MISC",
											   interestRate,
											   double.Parse(lblMiscFees2n.Value),
											   0,
											   addLBTermsToDP
											   ).Tables["PaymentBreakdown"];
				DataTable ekek2 = (DataTable)ViewState["gvMiscellaneous"];
				LoadData(gvMiscellaneous, "gvMiscellaneous");
			}
			else
			{
				//FOR MULTIPLE MISC

				if (MiscDPTerms > 0)
				{
					//divMonthlyDPMisc.Visible = true;


					//MISC DPSCHEDULE
					ViewState["gvMiscellaneous"] = ws.PaymentBreakdown(Convert.ToInt32(MiscDPTerms),
											   Convert.ToDateTime(MiscDueDate),
											   MiscDPMonthly,
											   0,
											   MiscDPAmount,
											   "MISC",
											   interestRate,
											   MiscDPAmount,
											   0,
											   addLBTermsToDP
											   ).Tables["PaymentBreakdown"];
					DataTable ekek23 = (DataTable)ViewState["gvMiscellaneous"];
					LoadData(gvMiscellaneous, "gvMiscellaneous");
				}
				else
				{
					ViewState["gvMiscellaneous"] = null;
					gvMiscellaneous.Visible = false;
				}


				if (MiscLBTerms > 0)
				{
					divMonthlyAmortMisc.Visible = true;
					// MISC LB SCHEDULE
					string MiscLBDate = Convert.ToDateTime(MiscDueDate).AddMonths(MiscDPTerms).ToString("MM-dd-yyyy");

					if (MiscLBTerms > 1)
					{


						ViewState["gvMiscellaneousAmort"] = ws.PaymentBreakdown(MiscLBTerms,
												   Convert.ToDateTime(MiscLBDate),
												   MiscLBMonthly,
												   0,
												   MiscLBAmount,
												   "MISC",
												   interestRate,
												   MiscLBAmount,
												   0,
												   0,
												   Convert.ToInt32(MiscDPTerms)).Tables["PaymentBreakdown"];
						DataTable ekek3 = (DataTable)ViewState["gvMiscellaneousAmort"];
						LoadData(gvMiscellaneousAmort, "gvMiscellaneousAmort");
					}
					else
					{
						ViewState["gvMiscellaneousAmort"] = ws.PaymentBreakdownBankPagibig(Convert.ToDateTime(MiscLBDate).AddMonths(1),
																 MiscLBAmount,
																 0,
																 MiscDPTerms).Tables[0];
						DataTable ekek3 = (DataTable)ViewState["gvMiscellaneousAmort"];
						LoadData(gvMiscellaneousAmort, "gvMiscellaneousAmort");


					}
				}
				else
				{
					ViewState["gvMiscellaneousAmort"] = null;
					divMonthlyAmortMisc.Visible = false;
				}

			}

















			DataTable dtIPS = (DataTable)ViewState["gvDownPayment"];

			//2023-06-23 : COMMENTED FOR CONSIDERATION OF 0 DPTERMS
			//double IPS = Convert.ToDouble(DataAccess.GetData(dtIPS, 0, "NewestIPSAmount", "0").ToString());
			//double LatestTerms = Convert.ToDouble(DataAccess.GetData(dtIPS, 0, "LatestTerms", "0").ToString());
			double IPS = 0;
			double LatestTerms = 0;
			if (dtIPS != null)
			{
				IPS = Convert.ToDouble(DataAccess.GetData(dtIPS, 0, "NewestIPSAmount", "0").ToString());
				LatestTerms = Convert.ToDouble(DataAccess.GetData(dtIPS, 0, "LatestTerms", "0").ToString());
			}

			if (oneLinertag == "Y")
			{
				if (Convert.ToDateTime(lblDueDate2n.Value) > DateTime.Now)
				{
					if (IPS == 0)
					{
						IPS = 0;
					}
				}


				ViewState["gvAmortization"] = ws.PaymentBreakdownBankPagibig(Convert.ToDateTime(lblDueDate2n.Value).AddMonths(1),
					Convert.ToDouble(lblLoanableBalance6nNew.Text), IPS).Tables[0];
			}
			else
			{
				if (LTerms > 0)
				{

					if (oneLinertag != "Y")
					{
						if (IPS > 0)
						{

							addDPTermsToLB = Convert.ToInt32((DPTerms + addLBTermsToDP) - LatestTerms);
							if (addDPTermsToLB == 0)
							{
								addDPTermsToLB = DPTerms;
							}
						}
					}


					//2023-08-01 : CHANGED TO MAXIMUM/LATEST DATE CHECKED
					//2023-08-02 : COMMENTED BACK; GET MINUMUM TERM +1 INSTEAD
					//var test = ViewState["MaxLBTermDate"].ToString();
					//string LBDateForSchedule = string.IsNullOrWhiteSpace(ViewState["MaxLBTermDate"].ToString()) ? (string.IsNullOrWhiteSpace(tDocDate.Value) ? "2022-01-01" : tDocDate.Value) : ViewState["MaxLBTermDate"].ToString();


					//2023-08-01 : CHANGED TO MAXIMUM/LATEST DATE CHECKED
					//2023-08-02 : COMMENTED BACK; GET MINUMUM TERM +1 INSTEAD
					var test = ViewState["MinLBDate"].ToString();
					string LBDateForSchedule = string.IsNullOrWhiteSpace(ViewState["MinLBDate"].ToString()) ? (string.IsNullOrWhiteSpace(tDocDate.Value) ? "2022-01-01" : tDocDate.Value) : ViewState["MinLBDate"].ToString();



					int AmortMonth = Convert.ToDateTime(LBDateForSchedule).Month;
					int AmortDay = Convert.ToDateTime(LBDateForSchedule).Day;
					int AmortYear = Convert.ToDateTime(LBDateForSchedule).Year;
					string AmortDate = "";

					if (AmortMonth == 2 && DateTime.DaysInMonth(AmortYear, AmortMonth) < AmortDay)
					{
						AmortDate = $@"{AmortYear}/{AmortMonth}/{AmortDay}";
					}
					else
					{
						AmortDate = $@"{AmortYear}/{AmortMonth}/{AmortDay}";
					}

					//ViewState["gvAmortization"] = ws.PaymentBreakdown(int.Parse(txtLTerms.Text), Convert.ToDateTime(lblDueDate3.Text), double.Parse(lblMiscMonthly.Text), 0, double.Parse(txtLoanableBalance2.Text), "LB", Convert.ToDouble(Session["IRate"]), double.Parse(lblAddMiscFees.Text)).Tables[0];
					ViewState["gvAmortization"] = ws.PaymentBreakdown(
																	LTerms,
																	//2023-08-02 : FROM LEAST MONTH ONLY TO : ADDED BACK MONTHS + 1
																	//Convert.ToDateTime(AmortDate),

																	//2024-08-20 : REVERT BACK | REMOVE ADD MONTHS
																	Convert.ToDateTime(AmortDate),

																	double.Parse(lblMonthly2nNew.Text),
																	double.Parse(interestRate.ToString()),
																	double.Parse(lblLoanableBalance6nNew.Text),
																	"LB",
																	interestRate,
																	double.Parse(lblAddMiscCharges2n.Value),
																	0,
																	addDPTermsToLB,
																	//2024-10-05 : CHANGE FROM MAX TERM TO MOST MINIMUM LB TERM
																	//Convert.ToInt32(ViewState["MaxLBTerm"]),
																	//2024-10-08 :TO REMOVE UPDATES SINCE CRF IS STILL FOR SIGNING
																	//Convert.ToInt32(ViewState["MinLBTerm"]),
																	Convert.ToInt32(ViewState["MaxLBTerm"]),
																	Convert.ToDateTime(AmortDate),
																	LBMonthly).Tables[0];
					DataTable ekek2 = (DataTable)ViewState["gvAmortization"];
					lblMonthly2nNew.Text = SystemClass.ToCurrency(LBMonthly.ToString());
				}
			}


			LoadData(gvAmortization, "gvAmortization");


			// THIS IS JUST TO HIDE OTHER TABLES EXCEPT AMORT SCHEDULE
			divMonthlyAmortMisc.Visible = false;
		}







		private DataTable dtComputeSheet(string dtType)
		{

			//GET DATA FROM SESSIONS
			string ProjectCode = ViewState["ProjCode"].ToString();
			string Block = ViewState["Block"].ToString();
			string Lot = ViewState["Lot"].ToString();
			string CardCode = ViewState["AddonCardCode"].ToString();
			string FinancingScheme = ViewState["FinancingScheme"].ToString();
			string HouseModel = ViewState["Model"].ToString();
			double ReservationFee = double.Parse(ViewState["ReservationFee"].ToString());

			//GET DATA FROM ADDON QUOTATION
			DataTable generalData = new DataTable();
			generalData = ws.GetGeneralData(Convert.ToInt32(ViewState["SQDocEntry"]), ConfigurationManager.AppSettings["HANADatabase"].ToString()).Tables[0];
			string ResDate = DataHelper.DataTableRet(generalData, 0, "DocDate", "");
			string ProductType = DataHelper.DataTableRet(generalData, 0, "ProductType", "");
			string Vatable = DataHelper.DataTableRet(generalData, 0, "Vatable", "");
			string EmpName = DataHelper.DataTableRet(generalData, 0, "EmpName", "");
			string AdjLot = DataHelper.DataTableRet(generalData, 0, "SoldWithAdjacentLot", "No").ToString();
			string AdjLotNo = DataAccess.GetData(generalData, 0, "AdjacentLotQuotationNo", "0").ToString();
			string LTSNo = DataAccess.GetData(generalData, 0, "LTSNo", "").ToString();
			int interestRate = int.Parse(DataAccess.GetData(generalData, 0, "InterestRate", "0").ToString());
			string LoanType = DataAccess.GetData(generalData, 0, "LoanType", "").ToString();
			string RetitlingType = DataAccess.GetData(generalData, 0, "RetitlingType", "").ToString();
			int LTerms = int.Parse(DataAccess.GetData(generalData, 0, "LTerms", "1").ToString());
			int oDAS = int.Parse(DataAccess.GetData(generalData, 0, "OTcp", "0").ToString());
			double DPPercent = double.Parse(DataAccess.GetData(generalData, 0, "DPPercent", "0").ToString());
			double DiscountAmount = double.Parse(DataAccess.GetData(generalData, 0, "DiscountAmount", "0").ToString());
			int DPTerms = int.Parse(DataAccess.GetData(generalData, 0, "DPTerms", "0").ToString());
			double DPAmount = double.Parse(DataAccess.GetData(generalData, 0, "DPAmount", "0").ToString());
			int DiscPercent = int.Parse(DataAccess.GetData(generalData, 0, "DiscPercent", "0").ToString());
			string SoldWithAdjacentLot = DataAccess.GetData(generalData, 0, "SoldWithAdjacentLot", "No").ToString();
			string LotArea = DataAccess.GetData(generalData, 0, "LotArea", "0").ToString();
			string FloorArea = DataAccess.GetData(generalData, 0, "FloorArea", "0").ToString();
			string HouseStatus = DataAccess.GetData(generalData, 0, "HouseStatus", "").ToString();
			string Phase = DataAccess.GetData(generalData, 0, "Phase", "").ToString();
			string Size = DataAccess.GetData(generalData, 0, "Size", "").ToString();
			string Bank = DataAccess.GetData(generalData, 0, "Bank", "").ToString();
			string EmpCode = DataHelper.DataTableRet(generalData, 0, "EmpCode", "");
			string EmpPosition = DataHelper.DataTableRet(generalData, 0, "POSCode", "");
			double TCPBalanceOnEquity = double.Parse(DataAccess.GetData(generalData, 0, "TCPBalanceOnEquity", "0").ToString());
			double CompTotal = double.Parse(DataAccess.GetData(generalData, 0, "CSTotal", "0").ToString());
			double TCPDownpayment = double.Parse(DataAccess.GetData(generalData, 0, "TCPDownpayment", "0").ToString());

			// ADDED FIELDS 2023-03-08
			string MiscDueDate = DataAccess.GetData(generalData, 0, "MiscDueDate", "1999-12-31").ToString();
			string MiscFinancingScheme = DataAccess.GetData(generalData, 0, "MiscFinancingScheme", "").ToString();
			int MiscDPTerms = int.Parse(DataAccess.GetData(generalData, 0, "MiscDPTerms", "1").ToString());
			string Comaker = DataAccess.GetData(generalData, 0, "Comaker", "").ToString();

			decimal MiscDPAmount = decimal.Parse(DataAccess.GetData(generalData, 0, "MiscDPAmount", "0").ToString());
			double MiscLBAmount = double.Parse(DataAccess.GetData(generalData, 0, "MiscLBAmount", "0").ToString());
			int MiscLBTerms = int.Parse(DataAccess.GetData(generalData, 0, "MiscLBTerms", "1").ToString());
			double MiscLBMonthly = double.Parse(DataAccess.GetData(generalData, 0, "MiscLBMonthly", "0").ToString());

			//GET BATCH NUMBER
			string qryBatch = $@"SELECT ""DistNumber"",  ""U_LTSNo"" FROM OBTN WHERE ""U_BlockNo"" = '{Block}' AND ""U_LotNo"" = '{Lot}' AND ""U_Project"" = '{ProjectCode}'";
			DataTable dtBatch = hana.GetData(qryBatch, hana.GetConnection("SAPHana"));
			string batch = DataAccess.GetData(dtBatch, 0, "DistNumber", "").ToString();
			string SAPLTSNo = DataAccess.GetData(dtBatch, 0, "U_LTSNo", "").ToString();
			string LOI = string.IsNullOrWhiteSpace(SAPLTSNo) ? "Yes" : "No";

			if (MiscLBTerms == 0)
			{
				MiscLBTerms = 1;
			}
			if (LTerms == 0)
			{
				LTerms = 1;
			}
			if (MiscDPTerms == 0)
			{
				MiscDPTerms = 1;
			}

			if (interestRate == 0)
			{
				interestRate = 1;
			}

			//GET TOTAL PAYMENTS
			double totalCurrentPayment = 0;
			foreach (GridViewRow row1 in gvPayments.Rows)
			{
				totalCurrentPayment += Convert.ToDouble(row1.Cells[2].Text);
			}


			//COMPUTE FOR UPDATED FIGURES
			DataTable dt = new DataTable();
			dt = ws.GetCompSheet(
								  0, FinancingScheme,  //2
								  ProductType, oDAS,  //4
								  0, DPPercent,   //6
								  ReservationFee, DiscountAmount, //8
								  "", DPTerms, //10
								  LTerms, "Y", //12

								  //PARAMETERS FOR COMPSHEET 
								  DPAmount, DiscPercent, //14
								  0, "", //16
								  null, ProductType, //18
								  ProjectCode, 0, //20
								  tDocDate.Value, Convert.ToDateTime(tDocDate.Value).Day.ToString(), //22

								  tDocDate.Value, 0, //24
								  interestRate, RetitlingType, //26
								  SoldWithAdjacentLot, MiscDPTerms,
								  MiscDPAmount, MiscLBTerms,
								  Convert.ToDouble(lblTotalPayment.Text) + totalCurrentPayment,
								  "YES", "YES" //ddlUpdateAmortBalance.Text, ddlRetainMonthlyAmort.Text
								  ).Tables[0];

			if (DataAccess.Exist(dt) == true)
			{
				//TOTAL DOWNPAYMENT AMOUNT
				lblDownPaymentn.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "initDP", "0"));

				//DP DUE DATE
				var date1 = (string)DataAccess.GetData(dt, 0, "initDueDate1", "");
				if (date1 != "-") { date1 = Convert.ToDateTime((string)DataAccess.GetData(dt, 0, "initDueDate1", "")).ToString("yyyy-MM-dd"); }
				lblDPDueDate.Value = date1;

				//VAT
				lblVATn.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "FinalVAT", "0"));

				lblTCPMonthly2n.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "DPMonthly", "0")).ToString();

				//Balance on Equity  ess.GetData(dt, 0, "PDBalance", "0"));
				lblBalance2n.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "PDBalance", "0")).ToString();

				//BASIS OF MISCELLANEOUS FEES
				if (ProductType.ToUpper() == "LOT ONLY" && RetitlingType.ToUpper() == "BUYERS")
				{
					string qry = $@"SELECT IFNULL(""U_BuyerRetitlingFee"",0)  + IFNULL(""U_BuyerRetitlingBond"",0) ""MiscFee""
                                                FROM ""@PRODUCTTYPE"" WHERE UPPER(""Code"") = '{ProductType.ToUpper()}'";
					DataTable dtProd = hana.GetData(qry, hana.GetConnection("SAPHana"));
					if (dtProd.Rows.Count > 0)
					{
						double miscmonthly = double.Parse(DataAccess.GetData(dtProd, 0, "MiscFee", "0").ToString());
						lblMiscMonthly2n.Value = SystemClass.ToCurrency(miscmonthly.ToString()).ToString();
						lblMiscFees2n.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dtProd, 0, "MiscFee", "0")).ToString();
						lblAddMiscFees2n.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dtProd, 0, "MiscFee", "0")).ToString();
						txtMiscFees.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dtProd, 0, "MiscFee", "0")).ToString();
						txtMiscMonthly.Value = SystemClass.ToCurrency(miscmonthly.ToString()).ToString();
					}

				}
				else
				{
					lblMiscMonthly2n.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "MiscMonthly", "0")).ToString();
					lblMiscFees2n.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "CSAddMisc", "0")).ToString();
					lblAddMiscFees2n.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "CSAddMisc", "0")).ToString();
					txtMiscFees.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "CSAddMisc", "0")).ToString();
					txtMiscMonthly.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "PDMiscFee", "0")).ToString();
				}


				//LB DUE DATE
				//TCP Breakdown : Due Date 2
				var date2 = (string)DataAccess.GetData(dt, 0, "initDueDate2", "");
				if (date2 != "-")
				{
					date2 = Convert.ToDateTime((string)DataAccess.GetData(dt, 0, "initDueDate2", "")).ToString("yyyy-MM-dd");
				}
				lblDueDate2n.Value = date2;

				//LOANABLE AMOUNT
				lblLoanableBalance6nNew.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "TCPLoanable", "0")).ToString();


				tPDBalance.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "PDBalance", "0")).ToString();


				//LOANABLE MONTHLY 
				lblMonthly2nNew.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "monthly2", "0"));

				//MISCELLANEOUS AMOUNT
				lblAddMiscCharges2n.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "AddMiscCharges", "0"));

				//MISC DUE DATE
				//TCP Breakdown : Due Date 3
				var date3 = (string)DataAccess.GetData(dt, 0, "initDueDate3", "");
				if (date3 != "-")
				{
					date3 = Convert.ToDateTime((string)DataAccess.GetData(dt, 0, "initDueDate3", "").ToString().Trim()).ToString("yyyy-MM-dd");
				}
				lblDueDate3n.Value = date3;
			}

			if (dtType == "GetGeneralData")
			{
				return generalData;
			}


			return dt;

		}


















		private string Restructuring(int PaymentEntry, out string errorMsg,
									out string QuotationDocEntry, out string DPDocEntry,
									out string MiscEntry)
		{

			SapHanaLayer company = new SapHanaLayer();
			bool isSuccess = true;
			string Message = string.Empty;



			//GET DATA FROM SESSIONS
			string ProjectCode = ViewState["ProjCode"].ToString();
			string Block = ViewState["Block"].ToString();
			string Lot = ViewState["Lot"].ToString();
			string CardCode = ViewState["AddonCardCode"].ToString();
			string FinancingScheme = ViewState["FinancingScheme"].ToString();
			string HouseModel = ViewState["Model"].ToString();
			double ReservationFee = double.Parse(ViewState["ReservationFee"].ToString());

			//GET DATA FROM ADDON QUOTATION
			DataTable generalData = new DataTable();
			generalData = ws.GetGeneralData(Convert.ToInt32(ViewState["SQDocEntry"]), ConfigurationManager.AppSettings["HANADatabase"].ToString()).Tables[0];
			string ResDate = DataHelper.DataTableRet(generalData, 0, "DocDate", "");
			string ProductType = DataHelper.DataTableRet(generalData, 0, "ProductType", "");
			string Vatable = DataHelper.DataTableRet(generalData, 0, "Vatable", "");
			string EmpName = DataHelper.DataTableRet(generalData, 0, "EmpName", "");
			string AdjLot = DataHelper.DataTableRet(generalData, 0, "SoldWithAdjacentLot", "No").ToString();
			string AdjLotNo = DataAccess.GetData(generalData, 0, "AdjacentLotQuotationNo", "0").ToString();
			string LTSNo = DataAccess.GetData(generalData, 0, "LTSNo", "").ToString();
			int interestRate = int.Parse(DataAccess.GetData(generalData, 0, "InterestRate", "0").ToString());
			string LoanType = DataAccess.GetData(generalData, 0, "LoanType", "").ToString();
			string RetitlingType = DataAccess.GetData(generalData, 0, "RetitlingType", "").ToString();
			int LTerms = int.Parse(DataAccess.GetData(generalData, 0, "LTerms", "1").ToString());
			double oDAS = int.Parse(DataAccess.GetData(generalData, 0, "OTcp", "0").ToString());
			double DPPercent = double.Parse(DataAccess.GetData(generalData, 0, "DPPercent", "0").ToString());
			double DiscountAmount = double.Parse(DataAccess.GetData(generalData, 0, "DiscountAmount", "0").ToString());
			int DPTerms = int.Parse(DataAccess.GetData(generalData, 0, "DPTerms", "0").ToString());
			double DPAmount = double.Parse(DataAccess.GetData(generalData, 0, "DPAmount", "0").ToString());
			double DiscPercent = int.Parse(DataAccess.GetData(generalData, 0, "DiscPercent", "0").ToString());
			string SoldWithAdjacentLot = DataAccess.GetData(generalData, 0, "SoldWithAdjacentLot", "No").ToString();
			string LotArea = DataAccess.GetData(generalData, 0, "LotArea", "0").ToString();
			string FloorArea = DataAccess.GetData(generalData, 0, "FloorArea", "0").ToString();
			string HouseStatus = DataAccess.GetData(generalData, 0, "HouseStatus", "").ToString();
			string Phase = DataAccess.GetData(generalData, 0, "Phase", "").ToString();
			string Size = DataAccess.GetData(generalData, 0, "Size", "").ToString();
			string Bank = DataAccess.GetData(generalData, 0, "Bank", "").ToString();
			string EmpCode = DataHelper.DataTableRet(generalData, 0, "EmpCode", "");
			string EmpPosition = DataHelper.DataTableRet(generalData, 0, "POSCode", "");
			double TCPBalanceOnEquity = double.Parse(DataAccess.GetData(generalData, 0, "TCPBalanceOnEquity", "0").ToString());
			double CompTotal = double.Parse(DataAccess.GetData(generalData, 0, "CSTotal", "0").ToString());
			double TCPDownpayment = double.Parse(DataAccess.GetData(generalData, 0, "TCPDownpayment", "0").ToString());
			double MiscFeesMonthly = double.Parse(DataAccess.GetData(generalData, 0, "MiscFeesMonthly", "0").ToString());
			string DPDueDate = DataAccess.GetData(generalData, 0, "DPDueDate", "1999-12-31").ToString();

			//2024-07-30 : ADJUSTED BY JOSES : MALI YUNG 
			//double DPMonthly = double.Parse(DataAccess.GetData(generalData, 0, "DPMonthly", "0").ToString());
			double DPMonthly = double.Parse(DataAccess.GetData(generalData, 0, "TCPMonthly", "0").ToString());


			double TCPLoanableBalance = double.Parse(DataAccess.GetData(generalData, 0, "TCPLoanableBalance", "0").ToString());
			double PDLoanableBalance = double.Parse(DataAccess.GetData(generalData, 0, "PDLoanableBalance", "0").ToString());


			// ADDED FIELDS 2023-03-08
			string MiscDueDate = DataAccess.GetData(generalData, 0, "MiscDueDate", "1999-12-31").ToString();
			string MiscFinancingScheme = DataAccess.GetData(generalData, 0, "MiscFinancingScheme", "").ToString();
			int MiscDPTerms = int.Parse(DataAccess.GetData(generalData, 0, "MiscDPTerms", "1").ToString());
			string Comaker = DataAccess.GetData(generalData, 0, "Comaker", "").ToString();

			double MiscDPAmount = double.Parse(DataAccess.GetData(generalData, 0, "MiscDPAmount", "0").ToString());
			double MiscLBAmount = double.Parse(DataAccess.GetData(generalData, 0, "MiscLBAmount", "0").ToString());
			int MiscLBTerms = int.Parse(DataAccess.GetData(generalData, 0, "MiscDPTerms", "1").ToString());
			double MiscLBMonthly = double.Parse(DataAccess.GetData(generalData, 0, "MiscLBMonthly", "0").ToString());
			double MiscDPMonthly = double.Parse(DataAccess.GetData(generalData, 0, "MiscFeesMonthly", "0").ToString());

			int MiscTerms = Convert.ToInt32(DataHelper.DataTableRet(generalData, 0, "MiscDPTerms", "0")) + Convert.ToInt32(DataHelper.DataTableRet(generalData, 0, "MiscLBTerms", "0"));


			//GET BATCH NUMBER
			string qryBatch = $@"SELECT ""DistNumber"",  ""U_LTSNo"" FROM OBTN WHERE ""U_BlockNo"" = '{Block}' AND ""U_LotNo"" = '{Lot}' AND ""U_Project"" = '{ProjectCode}'";
			DataTable dtBatch = hana.GetData(qryBatch, hana.GetConnection("SAPHana"));
			string batch = DataAccess.GetData(dtBatch, 0, "DistNumber", "").ToString();
			string SAPLTSNo = DataAccess.GetData(dtBatch, 0, "U_LTSNo", "").ToString();
			string LOI = string.IsNullOrWhiteSpace(SAPLTSNo) ? "Yes" : "No";



			//GET TOTAL PAYMENTS
			double totalCurrentPayment = 0;
			foreach (GridViewRow row1 in gvPayments.Rows)
			{
				totalCurrentPayment += Convert.ToDouble(row1.Cells[2].Text);
			}



			DataTable dtCheckedLBForAdvancedPayments = new DataTable();
			dtCheckedLBForAdvancedPayments.Columns.AddRange(new DataColumn[2] {
						new DataColumn("Term"),
						new DataColumn("AmountPaid")
				});


			foreach (GridViewRow row in gvDownPayments.Rows)
			{
				//LINE STATUS IF OPEN
				if (row.Cells[16].Text == "O")
				{
					if (row.Cells[3].Text == "LB")
					{
						//CHECKBOX IF CHECKED
						CheckBox chkSel = (CheckBox)gvDownPayments.Rows[row.RowIndex].FindControl("chkSel");
						if (chkSel.Checked)
						{
							dtCheckedLBForAdvancedPayments.Rows.Add(
											   row.Cells[2].Text,
											   row.Cells[13].Text
											   );
						}
					}
				}
			}


			#region COMMENTED FUNCTION BEFORE PROCEEDING WITH RESTRUCTURING

			////COMPUTE FOR UPDATED FIGURES
			//DataTable dt = new DataTable();
			//dt = ws.GetCompSheet(
			//                      0, FinancingScheme,  //2
			//                      ProductType, oDAS,  //4
			//                      0, DPPercent,   //6
			//                      ReservationFee, DiscountAmount, //8
			//                      "", DPTerms, //10
			//                      LTerms, "Y", //12

			//                      //PARAMETERS FOR COMPSHEET 
			//                      DPAmount, DiscPercent, //14
			//                      0, "", //16
			//                      null, ProductType, //18
			//                      ProjectCode, 0, //20
			//                      Convert.ToDateTime(ResDate).ToString("yyyy-MM-dd"), Convert.ToDateTime(ResDate).Day.ToString(), //22

			//                      Convert.ToDateTime(DPDueDate).ToString("yyyy-MM-dd"), 0, //24
			//                      interestRate, RetitlingType, //26
			//                      SoldWithAdjacentLot, DPTerms,
			//                      0, 0,
			//                      Convert.ToDouble(lblTotalPayment.Text) + totalCurrentPayment,
			//                      "YES", "YES" //ddlUpdateAmortBalance.Text, ddlRetainMonthlyAmort.Text
			//                      ).Tables[0];

			//if (DataAccess.Exist(dt) == true)
			//{
			//    //TOTAL DOWNPAYMENT AMOUNT
			//    lblDownPaymentn.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "initDP", "0"));

			//    //DP DUE DATE
			//    var date1 = Convert.ToDateTime(DataAccess.GetData(dt, 0, "initDueDate1", "")).ToString("yyyy-MM-dd");
			//    if (date1 != "-") { date1 = Convert.ToDateTime((string)DataAccess.GetData(dt, 0, "initDueDate1", "")).ToString("yyyy-MM-dd"); }
			//    lblDPDueDate.Value = date1;
			//    dtpDueDate.Value = Convert.ToDateTime((string)DataAccess.GetData(dt, 0, "dtpDueDate", "")).ToString("yyyy-MM-dd");


			//    //VAT
			//    lblVATn.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "FinalVAT", "0"));

			//    lblTCPMonthly2n.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "DPMonthly", "0")).ToString();


			//    //Balance on Equity  ess.GetData(dt, 0, "PDBalance", "0"));
			//    lblBalance2n.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "PDBalance", "0")).ToString();

			//    //BASIS OF MISCELLANEOUS FEES
			//    if (ProductType.ToUpper() == "LOT ONLY" && RetitlingType.ToUpper() == "BUYERS")
			//    {
			//        string qry = $@"SELECT IFNULL(""U_BuyerRetitlingFee"",0)  + IFNULL(""U_BuyerRetitlingBond"",0) ""MiscFee""
			//                                    FROM ""@PRODUCTTYPE"" WHERE UPPER(""Code"") = '{ProductType.ToUpper()}'";
			//        DataTable dtProd = hana.GetData(qry, hana.GetConnection("SAPHana"));
			//        if (dtProd.Rows.Count > 0)
			//        {
			//            double miscmonthly = double.Parse(DataAccess.GetData(dtProd, 0, "MiscFee", "0").ToString());
			//            lblMiscMonthly2n.Value = SystemClass.ToCurrency(miscmonthly.ToString()).ToString();
			//            lblMiscFees2n.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dtProd, 0, "MiscFee", "0")).ToString();
			//            lblAddMiscFees2n.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dtProd, 0, "MiscFee", "0")).ToString();
			//            txtMiscFees.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dtProd, 0, "MiscFee", "0")).ToString();
			//            txtMiscMonthly.Value = SystemClass.ToCurrency(miscmonthly.ToString()).ToString();
			//        }
			//    }
			//    else
			//    {
			//        lblMiscMonthly2n.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "MiscMonthly", "0")).ToString();
			//        lblMiscFees2n.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "CSAddMisc", "0")).ToString();
			//        lblAddMiscFees2n.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "CSAddMisc", "0")).ToString();
			//        txtMiscFees.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "CSAddMisc", "0")).ToString();
			//        txtMiscMonthly.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "PDMiscFee", "0")).ToString();
			//    }


			//    //LB DUE DATE
			//    //TCP Breakdown : Due Date 2
			//    var date2 = (string)DataAccess.GetData(dt, 0, "initDueDate2", "");
			//    if (date2 != "-")
			//    {
			//        date2 = Convert.ToDateTime((string)DataAccess.GetData(dt, 0, "initDueDate2", "")).ToString("yyyy-MM-dd");
			//    }
			//    lblDueDate2n.Value = date2;

			//    //LOANABLE AMOUNT
			//    lblLoanableBalance6n.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "TCPLoanable", "0")).ToString();


			//    tPDBalance.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "PDBalance", "0")).ToString();


			//    //LOANABLE MONTHLY 
			//    lblMonthly2n.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "monthly2", "0"));

			//    //MISCELLANEOUS AMOUNT
			//    lblAddMiscCharges2n.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "AddMiscCharges", "0"));

			//    //MISC DUE DATE
			//    //TCP Breakdown : Due Date 3
			//    var date3 = (string)DataAccess.GetData(dt, 0, "initDueDate3", "");
			//    if (date3 != "-")
			//    {
			//        date3 = Convert.ToDateTime((string)DataAccess.GetData(dt, 0, "initDueDate3", "").ToString().Trim()).ToString("yyyy-MM-dd");
			//    }
			//    lblDueDate3n.Value = date3;
			//}





			//loadSchedules(interestRate, LoanType,
			//              ProductType, RetitlingType,
			//              LTerms, DPTerms,
			//              MiscDPTerms, MiscDPAmount,
			//              MiscDPMonthly, MiscLBTerms,
			//              MiscLBAmount, MiscLBMonthly,
			//              MiscDueDate, double.Parse(lblMonthly2n.Value));

			#endregion



			var test1 = ViewState["SQDocEntry"];
			var test2 = ViewState["DocNum"];
			var test3 = Session["UserID"];
			var test4 = Session["UserName"];

			//// CANCEL AND REPOST ALL SAP TRANSACTIONS
			//if (wcf.CancelTransactionsNew(

			//                        //OLD FIELDS
			//                        ViewState["SQDocEntry"].ToString(), company, //2
			//                        Convert.ToDateTime(ResDate).ToString("yyyy-MM-dd"), ProjectCode, //4   
			//                        Block, Lot, //6
			//                        TCPDownpayment, HouseModel, //8
			//                        FinancingScheme, ReservationFee, //10
			//                        ProductType, CardCode, //12

			//                        //NEW FIELDS
			//                        HouseModel, FinancingScheme, //14
			//                        ProductType, ReservationFee, //16
			//                        TCPDownpayment, Convert.ToDouble(lblLoanableBalance6nNew.Text), //18
			//                        lblID.Text, Convert.ToDouble(lblMiscFees2n.Value), //20 
			//                        Convert.ToDouble(lblTotalPayment.Text) + totalCurrentPayment, txtDocNum.Text, //22
			//                        Block, Lot, //24
			//                        TCPDownpayment, Convert.ToDouble(lblLoanableBalance6nNew.Text), //26 
			//                        oDAS, Vatable, //28
			//                        DiscountAmount, Convert.ToDouble(lblMiscFees2n.Value), //30
			//                        batch, EmpName, // 
			//                        AdjLot, AdjLotNo, //34
			//                        ProjectCode, MiscFeesMonthly, //36
			//                        DPTerms, LOI, //38
			//                        Session["UserName"].ToString(), "NEW", //40
			//                        Convert.ToDouble(lblTotalMiscPaid.Text), oDAS, //42
			//                        Convert.ToDateTime(tDocDate.Value).ToString("yyyy-MM-dd"), Session["UserID"].ToString(), //44
			//                        "YES", LTSNo, //46
			//                        1,  //advance paymenmt tagging
			//                        MiscTerms,
			//                        Convert.ToDouble(lblNETTCP.Text), "Advance",//2024-06-13 KUNG ADVANCE PAYMENT OR RESTRUCTURING  //ORIGINAL NET DAS
			//                        out errorMsg, out QuotationDocEntry, //48
			//                        out DPDocEntry, out MiscEntry //50
			//                        ))
			//{


			// UPDATING DATA IN ADDON DATABASE  

			//QuotationDocEntry = "0";
			//DPDocEntry = "0";
			//MiscEntry = "0";

			var test5 = ViewState["SQDocEntry"];
			var test6 = ViewState["txtDiscPercent"];
			var test7 = ViewState["gvDownPayment"];
			var test8 = ViewState["gvDownPayment"];
			var test9 = ViewState["gvAmortization"];

			var gvDownPayment = ViewState["gvDownPayment"];
			var gvAmortization = ViewState["gvAmortization"];



			if (!ws.Restructure(
					   (int)ViewState["SQDocEntry"],
					   DataAccess.GetSession(lblID.Text, ""),  //2
					   Convert.ToDateTime(tDocDate.Value),
					   "O", //4
					   ProjectCode,

					   Block, //6
					   Lot,
					   HouseModel,  //8
					   FinancingScheme,
					   LotArea, //10

					   (FloorArea == "-" ? "0" : FloorArea),
					   HouseStatus, //12
					   Phase,
					   Size, //14 
					   ProductType,

					   LoanType, //16
					   Bank,
					   oDAS, //18
					   ReservationFee,
					   DPPercent, //20

					   DPAmount,
					   DPTerms, //22
					   Convert.ToDouble(DiscPercent),
					   DiscountAmount, //24
					   LTerms,

					   Convert.ToDouble(interestRate), //26 
					   Convert.ToDateTime(lblDueDate2n.Value),
					   0, //GDI //28
					   oDAS,
					   double.Parse(lblAddMiscCharges2n.Value),  //30

					   //double.Parse(lblDASAmtn.Text),

					   //2023-06-26 : CHANGED FROM oDAS to Convert.ToDouble(lblNETTCP.Text)
					   //oDAS,
					   Convert.ToDouble(lblNETTCP.Text),

					   double.Parse(lblVATn.Value),  //32
					   Convert.ToDouble(lblNETTCP.Text),
					   TCPDownpayment, //34
					   double.Parse(lblTCPMonthly2n.Value),

					   Convert.ToDateTime(lblDPDueDate.Value), //36
					   double.Parse(lblAmountDue.Text),
					   (int)Session["UserID"], //38
					   EmpCode,
					   EmpName, //40

					   EmpPosition,
					   (DataTable)ViewState["gvDownPayment"],  //42
					   (DataTable)ViewState["gvAmortization"],
					   txtDocNum.Text, //44

					   double.Parse(txtMiscMonthly.Value),

					   DPMonthly, //46
					   double.Parse(lblMiscMonthly2n.Value),
					   double.Parse(lblAddMiscFees2n.Value), //48
					   double.Parse(tPDBalance.Value),
					   TCPBalanceOnEquity, //50

					   double.Parse(txtMiscFees.Value),
					   double.Parse(lblLoanableBalance6nNew.Text), //52
					   TCPLoanableBalance,
					   "",  //54
					   RetitlingType,

					   CompTotal, //56 
					   DPTerms,
					   Convert.ToDateTime(lblDPDueDate.Value), //58
					   Convert.ToDateTime(lblDPDueDate.Value),
					   oDAS, //60 

					   TCPDownpayment,
					   LTerms, //62
					   Convert.ToDateTime(lblDueDate2n.Value),
					   Convert.ToDateTime(lblDueDate3n.Value),//64
					   Convert.ToDouble(lblTotalPayment.Text) + totalCurrentPayment,

					   FinancingScheme,//66
					   ProductType,
					   Lot, //68
					   HouseModel,
					   ProjectCode, //70

					   "AdvancePayment",
					   Convert.ToDateTime(tDocDate.Value), //72
					   "",
					   "", //74
					   "",

					   EmpCode, //76
					   EmpName,
					   EmpPosition, //78
					   "NEW",
					   0, //80

					   Vatable,
					   Convert.ToDouble(lblMonthly2nNew.Text), //82

					   Convert.ToDateTime(tDocDate.Value),
					   "", //84
					   Convert.ToDouble(lblTotalMiscPaid.Text),
					   lblID.Text, //86
					   RetitlingType,

					   "YES", //88

					   //NEW FIELDS 2023-03-07
					   Comaker,
					   Convert.ToDateTime(MiscDueDate), //90
					   MiscFinancingScheme,
					   MiscDPTerms, //92
					   MiscDPTerms,
					   (DataTable)ViewState["gvMiscellaneous"], //94
					   (DataTable)ViewState["gvMiscellaneousAmort"],

					   MiscDPAmount, //96
					   MiscLBTerms,
					   MiscLBAmount, //98
					   MiscLBMonthly,

					   //2023-06-27 : CHECKING OF CHANGED SALES AGENT
					   0,

					   //2024-10-05 : ADDITIONAL PARAMETERS: MIN LB TERM AND MIN DUEDATE FOR ADDING OF ADVANCE TO PRINCIPAL PAYMENT
					   ViewState["MinLBTerm"].ToString(),
					   ViewState["MinLBDate"].ToString(),
					   totalCurrentPayment,
					   PaymentEntry,

					   //dtCheckedLBForAdvancedPayments,
					   out errorMsg // 100
				))
			{
				alertMsg("Error restructuring the contract. Please contact administrator. (1)", "error");
			}
			else
			{
				if (!isSuccess)
				{
					alertMsg("Error restructuring the contract. Please contact administrator. (2)", "error");
					//ROLLBACK DITO
				}
				else
				{


					//GET ALL PAID LBs FOR TAGGING THEM WITH "ADVANCE PAYMENTS"
					string qryGetPreviousPayments = $@"SELECT * FROM QUT1 WHERE 
                                                           ""DocEntry"" = {ViewState["SQDocEntry"].ToString()}  AND 
                                                           ""PaymentType"" = 'LB' AND 
                                                           ""LineStatus"" = 'R' and ""AmountPaid"" > 0 ORDER BY ""Id""";
					DataTable dtGetPreviousPayments = hana.GetData(qryGetPreviousPayments, hana.GetConnection("SAOHana"));


					//LOOP DATATABLE TO CHECK IF THE CURRENT SELECTED 
					foreach (DataRow rowPrevPayments in dtGetPreviousPayments.Rows)
					{
						//CHECK IF THE TERM EXISTS, CHECKED AS FOR PAYMENT OF ADVANCED PAYMENTS
						double amountPaid = 0;
						foreach (DataRow row in dtCheckedLBForAdvancedPayments.Rows)
						{
							int rowGetPreviousPaymentsTERMS = int.Parse(row["Term"].ToString());
							int rowCheckedLBForAdvancedPaymentsTERMS = int.Parse(rowPrevPayments["Terms"].ToString());
							if (rowGetPreviousPaymentsTERMS == rowCheckedLBForAdvancedPaymentsTERMS)
							{
								//UPDATE QUT1 ROW WHEN THE ROW IS THE CURRENT PAYMENT
								amountPaid = Convert.ToDouble(row["AmountPaid"].ToString());
								string qryUpdateAllPreviouslyPaidLB = $@"UPDATE QUT1 SET ""AdvancePayment"" = 'Y', ""Principal"" = {amountPaid}, ""InterestAmount"" = 0, ""PaymentAmount"" = {amountPaid} Where 
                                                                            ""DocEntry"" = {(int)ViewState["SQDocEntry"]} AND 
                                                                            ""LineStatus"" = 'R' AND 
                                                                            ""PaymentType"" = 'LB' AND
                                                                            ""AmountPaid"" > 0 AND 
                                                                            ""Terms"" = '{rowCheckedLBForAdvancedPaymentsTERMS}'    ";
								hana.Execute(qryUpdateAllPreviouslyPaidLB, hana.GetConnection("SAOHana"));
							}
							else
							{
								//UPDATE QUT1 ROW WHEN THE ROW IS A PREIVOUS PAYMENT
								string qryUpdateAllPreviouslyPaidLB = $@"UPDATE QUT1 SET ""PrevPaymentsAdvPayments"" = 'Y' 
                                                                 Where ""DocEntry"" = {(int)ViewState["SQDocEntry"]} AND ""LineStatus"" = 'R' AND ""PaymentType"" = 'LB' AND 
                                                                       IFNULL(""AdvancePayment"",'') <> 'Y' AND ""AmountPaid"" > 0 AND ""Terms"" = '{rowCheckedLBForAdvancedPaymentsTERMS}' ";
								hana.Execute(qryUpdateAllPreviouslyPaidLB, hana.GetConnection("SAOHana"));
							}
						}
					}





				}
			}

			//}

			DPDocEntry = "0";
			QuotationDocEntry = "0";
			MiscEntry = "0";

			return DPDocEntry;
			return MiscEntry;
			return QuotationDocEntry;
			return errorMsg;


		}

		protected void gvMiscellaneousAmort_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			gvMiscellaneousAmort.PageIndex = e.NewPageIndex;
			LoadData(gvMiscellaneousAmort, "gvMiscellaneousAmort");
		}

		protected void gvMiscellaneous_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			gvMiscellaneous.PageIndex = e.NewPageIndex;
			LoadData(gvMiscellaneous, "gvMiscellaneous");
		}

		protected void gvAmortization_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			gvAmortization.PageIndex = e.NewPageIndex;
			LoadData(gvAmortization, "gvAmortization");
		}

		protected void gvDownPayments_RowCommand1(object sender, GridViewCommandEventArgs e)
		{

		}

		protected void gvDownPayment_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			gvDownPayment.PageIndex = e.NewPageIndex;
			LoadData(gvDownPayment, "gvDownPayment");
		}

























































































		//#############################################################################################################################
		//#############################################################################################################################
		//##################################################-- CANCELLATIONS --########################################################
		//#############################################################################################################################
		//#############################################################################################################################



		private void cancelSurchageInvoices()
		{
			//2023-09-19 : CANCEL ALL AR INVOICES WITH SURCHARGE TRANSACTIONTYPE AND CANCELLED INCOMING PAYMENTS
			SapHanaLayer company = new SapHanaLayer();

			//2023-10-11 : UPDATED QUERY FOR EFFICIENCY
			//string qry = $@" SELECT	C.""DocEntry"" FROM ORCT A INNER JOIN	RCT2 B ON A.""DocEntry"" = B.""DocNum"" AND B.""InvType"" = 13 
			//                 AND A.""Canceled"" = 'Y' INNER JOIN OINV C ON B.""DocEntry"" = C.""DocEntry"" AND C.""U_TransactionType"" = 'RE - SUR' 
			//                 AND C.""DocStatus"" = 'O' AND C.""CANCELED"" NOT IN ('Y','C')";
			string qry = $@" SELECT
                                A.""DocEntry"",A.""DocNum"" 
                             FROM OINV A WHERE 
                             (SELECT
                                    COUNT(x.""DocEntry"")
                              FROM
                                    ORCT x INNER JOIN
		                            RCT2 y ON x.""DocEntry"" = y.""DocNum"" AND y.""InvType"" = 13 AND x.""Canceled"" = 'Y' AND y.""DocEntry"" = A.""DocEntry""
	                         ) > 0 AND
	                         A.""PaidToDate"" = 0 AND
	                         A.""U_TransactionType"" = 'RE - SUR' AND 	
	                         A.""DocStatus"" = 'O' AND 
	                         A.""CANCELED"" NOT IN ('Y','C')
                             ";
			DataTable dt = hana.GetData(qry, hana.GetConnection("SAPHana"));

			//declare variable for error
			int errorTag = 0;

			if (dt.Rows.Count > 0)
			{
				foreach (DataRow row in dt.Rows)
				{
					string InvoiceDocEntry = row["DocEntry"].ToString();

					//2024-04-05 : CHANGES FOR CANCELLATION OF AR INVOICES -- DOCUMENT DATE RELATED CHANGES 
					string qryIncomingPayments = $@"SELECT x.""CancelDate"" FROM ORCT x INNER JOIN RCT2 y ON x.""DocEntry"" = y.""DocNum"" AND y.""InvType"" = 13 AND x.""Canceled"" = 'Y' AND y.""DocEntry"" = {InvoiceDocEntry}";
					DataTable dtIncomingPayments = hana.GetData(qryIncomingPayments, hana.GetConnection("SAPHana"));
					string CancelDate = DataAccess.GetData(dtIncomingPayments, 0, "CancelDate", "").ToString();

					string json = $"{{\"Document\":{{\"DocEntry\":{InvoiceDocEntry},\"DocDate\":\"{Convert.ToDateTime(CancelDate).ToString("yyyyMMdd")}\"}}}}";
					StringBuilder header = new StringBuilder(json);

					//if (!company.POST($"InvoicesService_Cancel2", new StringBuilder()))
					if (!company.POST($"InvoicesService_Cancel2", header))
					{
						errorTag = 1;
					}
					else
					{
						hana.Execute($@"UPDATE ""OINV"" SET ""U_Remarks"" = 'CANCELLED BY DREAMS -- SURCHARGE : {System.DateTime.Now}' WHERE ""DocEntry"" = '{InvoiceDocEntry}';", hana.GetConnection("SAPHana"));
					}
				}
			}


			if (errorTag > 0)
			{
				alertMsg("An error occurred while cancelling an open AR Invoice (Surcharge) with cancelled Incoming Payment. Please contact administrator.", "info");
			}
		}



		private void cancelInterestInvoices()
		{
			//2023-10-11 : CANCEL ALL AR INVOICES WITH INTEREST TRANSACTIONTYPE AND CANCELLED INCOMING PAYMENTS
			SapHanaLayer company = new SapHanaLayer();
			string qry = $@" SELECT
                                A.""DocEntry"",A.""DocNum"" 
                             FROM OINV A WHERE
                             (SELECT
                                    COUNT(x.""DocEntry"")
                              FROM
                                    ORCT x INNER JOIN
		                            RCT2 y ON x.""DocEntry"" = y.""DocNum"" AND y.""InvType"" = 13 AND x.""Canceled"" = 'Y' AND y.""DocEntry"" = A.""DocEntry""
	                         ) > 0 AND
	                         A.""PaidToDate"" = 0 AND
	                         A.""U_TransactionType"" = 'INT INC' AND 	
	                         A.""DocStatus"" = 'O' AND 
	                         A.""CANCELED"" NOT IN ('Y','C')
                             ";
			DataTable dt = hana.GetData(qry, hana.GetConnection("SAPHana"));

			//declare variable for error
			int errorTag = 0;

			if (dt.Rows.Count > 0)
			{
				foreach (DataRow row in dt.Rows)
				{
					string InvoiceDocEntry = row["DocEntry"].ToString();

					//2024-04-05 : CHANGES FOR CANCELLATION OF AR INVOICES -- DOCUMENT DATE RELATED CHANGES 
					string qryIncomingPayments = $@"SELECT x.""CancelDate"" FROM ORCT x INNER JOIN RCT2 y ON x.""DocEntry"" = y.""DocNum"" AND y.""InvType"" = 13 AND x.""Canceled"" = 'Y' AND y.""DocEntry"" = {InvoiceDocEntry}";
					DataTable dtIncomingPayments = hana.GetData(qryIncomingPayments, hana.GetConnection("SAPHana"));
					string CancelDate = DataAccess.GetData(dtIncomingPayments, 0, "CancelDate", "").ToString();

					string json = $"{{\"Document\":{{\"DocEntry\":{InvoiceDocEntry},\"DocDate\":\"{Convert.ToDateTime(CancelDate).ToString("yyyyMMdd")}\"}}}}";
					StringBuilder header = new StringBuilder(json);

					//if (!company.POST($"Invoices({InvoiceDocEntry})/Cancel", new StringBuilder()))
					if (!company.POST($"InvoicesService_Cancel2", header))
					{
						errorTag = 1;
					}
					else
					{
						hana.Execute($@"UPDATE ""OINV"" SET ""U_Remarks"" = 'CANCELLED BY DREAMS -- INTEREST : {System.DateTime.Now}' WHERE ""DocEntry"" = '{InvoiceDocEntry}';", hana.GetConnection("SAPHana"));
					}
				}
			}


			if (errorTag > 0)
			{
				alertMsg("An error occurred while cancelling an open AR Invoice (Interest) with cancelled Incoming Payment. Please contact administrator.", "info");
			}
		}







		private void cancelDFC()
		{
			try
			{

				//2023-10-09 : CREATE JOURNAL ENTRY TO OFFSET ALL BALANCE OF AR DOWNPAYMENTS WITH CANCELLED INCOMING PAYMENTS
				SapHanaLayer company = new SapHanaLayer();
				CashRegisterService cashRegister = new CashRegisterService();

				//2023-10-10 : GET ALL DOWNPAYMENT INVOICE WITH CANCELLED INCOMING PAYMENTS
				string qryDPI = $@" SELECT 
                                C.*, A.""CancelDate"" ""IncomingPaymentCancellationDate"",
                                IFNULL((SELECT DISTINCT x.""BaseEntry"" FROM DPI1 x WHERE x.""DocEntry"" = C.""DocEntry"" AND x.""BaseType"" = 17),0) ""SOBaseEntry""
                             FROM 
                                ORCT A INNER JOIN 
                                RCT2 B ON A.""DocEntry"" = B.""DocNum"" AND B.""InvType"" = 203 INNER JOIN 
                                ODPI C ON B.""DocEntry"" = C.""DocEntry"" AND C.""PaidToDate"" = 0 AND C.""DocStatus"" = 'O' AND  
                                        (SELECT 
                                            COUNT(IFNULL(q.""DocEntry"",0)) 
                                        FROM 
                                            DPI1 q 
                                        WHERE 
                                            C.""DocEntry"" = q.""DocEntry"" AND 
                                            IFNULL(q.""TargetType"",0) = -1) > 0 
                             WHERE 
                                A.""Canceled"" = 'Y'";
				DataTable dtDPI = hana.GetData(qryDPI, hana.GetConnection("SAPHana"));

				//declare variable for error
				int errorTag = 0;
				string tempMessage = "";

				if (dtDPI.Rows.Count > 0)
				{
					foreach (DataRow row in dtDPI.Rows)
					{
						string DPInvoiceDocNum = row["DocNum"].ToString();
						string test = row["IncomingPaymentCancellationDate"].ToString();
						string CancelledIncomingPaymentDocDate = Convert.ToDateTime(row["IncomingPaymentCancellationDate"].ToString()).ToString("yyyyMMdd");
						int SOBaseEntry = Convert.ToInt32(row["SOBaseEntry"].ToString());



						//2023-10-10 : CHECK IF JE ALREADY EXISTS
						string qryCheckExitingJE = $@"SELECT * FROM OJDT WHERE ""Ref2"" = '{DPInvoiceDocNum}'";
						DataTable dtCheckExitingJE = hana.GetData(qryCheckExitingJE, hana.GetConnection("SAPHana"));
						if (dtCheckExitingJE.Rows.Count == 0)
						{


							//2023-10-10 : GET ALL JOURNAL ENTRIES OF FETCHED AR DOWNPAYMENT INVOICE FOR REFERENCE OF ACCOUNTS AND AMOUNTS
							string qryGetJEofDP = $@"SELECT A.""TransId"",A.""U_Project"", A.""Project"", A.""U_BlockNo"", A.""U_LotNo"",A.""Ref2"" ""DocNum"",
                                            A.""RefDate"" ""PostingDate"",""Ref2"", 
                                            (SELECT DISTINCT x.""ShortName"" FROM JDT1 x WHERE x.""TransId"" = A.""TransId"" AND ifnull(x.""Debit"",0) > 0) ""ShortName"",
                                            (SELECT DISTINCT x.""Account"" FROM JDT1 x WHERE x.""TransId"" = A.""TransId"" AND ifnull(x.""Debit"",0) > 0) ""ARAccount"",
                                            (SELECT SUM(IFNULL(x.""Debit"",0)) FROM JDT1 x WHERE x.""TransId"" = A.""TransId"" AND ifnull(x.""Debit"",0) > 0) ""Debit"",
                                            (SELECT DISTINCT x.""Account"" FROM JDT1 x 
                                                    WHERE x.""TransId"" = A.""TransId"" AND ifnull(x.""Debit"",0) = 0 AND IFNULL(x.""VatGroup"",'') <> '') ""OutputTaxAccount"",
                                            (SELECT SUM(IFNULL(x.""Credit"",0)) FROM JDT1 x 
                                            WHERE x.""TransId"" = A.""TransId"" AND ifnull(x.""Debit"",0) = 0 AND IFNULL(x.""VatGroup"",'') <> '') ""OutputTaxAmount"",
                                            (SELECT DISTINCT x.""Account"" FROM JDT1 x 
                                            WHERE x.""TransId"" = A.""TransId"" AND ifnull(x.""Debit"",0) = 0 AND IFNULL(x.""VatGroup"",'') = '') ""DFCAccount"",
                                            (SELECT SUM(IFNULL(x.""Credit"",0)) FROM JDT1 x 
                                            WHERE x.""TransId"" = A.""TransId"" AND ifnull(x.""Debit"",0) = 0 AND IFNULL(x.""VatGroup"",'') = '') ""Credit""
                                            FROM OJDT A WHERE A.""TransType"" = 203 AND A.""BaseRef"" = {DPInvoiceDocNum}   
                                            ";

							DataTable dtGetJEofDP = hana.GetData(qryGetJEofDP, hana.GetConnection("SAPHana"));
							if (dtGetJEofDP.Rows.Count > 0)
							{


								//2023-10-10 : GET ALL DATA FROM PREVIOUS JOURNAL ENTRIES 
								string ProjectCode = DataAccess.GetData(dtGetJEofDP, 0, "Project", "").ToString();
								string SAPCardCode = DataAccess.GetData(dtGetJEofDP, 0, "ShortName", "").ToString();
								double Debit = Convert.ToDouble(DataAccess.GetData(dtGetJEofDP, 0, "Debit", "0").ToString());
								string Block = DataAccess.GetData(dtGetJEofDP, 0, "U_BlockNo", "").ToString();
								string Lot = DataAccess.GetData(dtGetJEofDP, 0, "U_LotNo", "").ToString();
								string ARAccount = DataAccess.GetData(dtGetJEofDP, 0, "ARAccount", "").ToString();
								string DFCAccount = DataAccess.GetData(dtGetJEofDP, 0, "DFCAccount", "").ToString();
								string JEType = "RVDP";
								string TransCode = "RVDP";
								double Credit = Convert.ToDouble(DataAccess.GetData(dtGetJEofDP, 0, "Credit", "0").ToString());
								string OutputTaxAccount = DataAccess.GetData(dtGetJEofDP, 0, "OutputTaxAccount", "").ToString();
								double OutputTaxAmount = Convert.ToDouble(DataAccess.GetData(dtGetJEofDP, 0, "OutputTaxAmount", "").ToString());

								int tempJENo = 0;

								//2023-10-10 : GET ALL JOURNAL ENTRIES OF FETCHED AR DOWNPAYMENT INVOICE FOR REFERENCE OF ACCOUNTS AND AMOUNTS
								if (cashRegister.CreateJournalEntry(company, ProjectCode, SAPCardCode, Convert.ToDouble(Debit), SOBaseEntry, "",
									"", Block, Lot, ARAccount, OutputTaxAccount, DFCAccount, JEType, DPInvoiceDocNum, "", "", "", CancelledIncomingPaymentDocDate, TransCode,
									out tempJENo, out tempMessage, "", OutputTaxAmount, null, Credit))
								{
								}
								else
								{
									errorTag++;
								}
							}
						}
					}
				}


				if (errorTag > 0)
				{
					alertMsg($@"An error occurred while creating entries for cancelled Downpayment Invoices({tempMessage}). Please contact administrator.", "info");
				}
			}
			catch (Exception ex)
			{
				alertMsg($@"{ex.Message} -- (cancelDFC Journal Entry posting).", "info");
			}
		}




		private void cancelDFCReversal(string DocNum, string IncomingPaymentDate)
		{
			try
			{

				//2023-10-10 : REVERSAL OF JOURNAL ENTRY OF CANCELLED DFCs
				SapHanaLayer company = new SapHanaLayer();
				CashRegisterService cashRegister = new CashRegisterService();
				//2023-10-10 : CHECK IF JE ALREADY EXISTS
				string qryCheckExitingJE = $@"SELECT * FROM OJDT WHERE ""Ref2"" = '{DocNum}'"; //SI DOCNUM AY SI TRANSID NG JE NA IREREVERSE
				DataTable dtCheckExitingJE = hana.GetData(qryCheckExitingJE, hana.GetConnection("SAPHana"));


				//declare variable for error
				int errorTag = 0;
				string tempMessage = "";

				if (dtCheckExitingJE.Rows.Count > 0)
				{

					//2023-10-10 : CHECK IF REVERSAL ALREADY EXISTS 
					string qryCheckIfExistingJEReversalExists = $@"SELECT * FROM OJDT WHERE ""Ref2"" = '{DocNum}' AND ""TransCode"" = 'XRVD'";
					DataTable dtCheckIfExistingJEReversalExists = hana.GetData(qryCheckIfExistingJEReversalExists, hana.GetConnection("SAPHana"));
					if (dtCheckIfExistingJEReversalExists.Rows.Count == 0)
					{
						int JEEntry = Convert.ToInt32(DataAccess.GetData(dtCheckExitingJE, 0, "TransId", "0").ToString());

						//2023-10-10 : GET ALL JOURNAL ENTRIES OF FETCHED AR DOWNPAYMENT INVOICE FOR REFERENCE OF ACCOUNTS AND AMOUNTSe
						string qryGetJEofDP = $@"SELECT A.""TransId"",A.""U_Project"",A.""Project"", A.""U_BlockNo"", A.""U_LotNo"",A.""Ref2"" ""DocNum"",A.""RefDate"" ""PostingDate"",""Ref2"",  
	                                            (SELECT DISTINCT x.""ShortName"" FROM JDT1 x WHERE x.""TransId"" = A.""TransId"" AND ifnull(x.""Credit"",0) > 0) ""ShortName"",
                                                (SELECT DISTINCT x.""Account"" FROM JDT1 x WHERE x.""TransId"" = A.""TransId"" AND ifnull(x.""Credit"",0) > 0) ""ARAccount"",
                                                (SELECT SUM(IFNULL(x.""Debit"",0)) FROM JDT1 x WHERE x.""TransId"" = A.""TransId"" AND ifnull(x.""Debit"",0) > 0) ""Debit"", 
                                                (SELECT DISTINCT x.""Account"" FROM JDT1 x WHERE x.""TransId"" = A.""TransId"" AND ifnull(x.""Debit"",0) > 0) ""DFCAccount"",
                                                (SELECT SUM(IFNULL(x.""Credit"",0)) FROM JDT1 x WHERE x.""TransId"" = A.""TransId"" AND ifnull(x.""Credit"",0) > 0) ""Credit""
                                            FROM OJDT A WHERE A.""TransType"" = 30 AND A.""TransId"" = {JEEntry}   
                                            ";

						DataTable dtGetJEofDP = hana.GetData(qryGetJEofDP, hana.GetConnection("SAPHana"));
						if (dtGetJEofDP.Rows.Count > 0)
						{
							//2023-10-10 : GET ALL DATA FROM PREVIOUS JOURNAL ENTRIES 
							string ProjectCode = DataAccess.GetData(dtGetJEofDP, 0, "Project", "").ToString();
							string SAPCardCode = DataAccess.GetData(dtGetJEofDP, 0, "ShortName", "").ToString();
							double Debit = Convert.ToDouble(DataAccess.GetData(dtGetJEofDP, 0, "Debit", "0").ToString());
							string Block = DataAccess.GetData(dtGetJEofDP, 0, "U_BlockNo", "").ToString();
							string Lot = DataAccess.GetData(dtGetJEofDP, 0, "U_LotNo", "").ToString();
							string ARAccount = DataAccess.GetData(dtGetJEofDP, 0, "ARAccount", "").ToString();
							string DFCAccount = DataAccess.GetData(dtGetJEofDP, 0, "DFCAccount", "").ToString();
							string JEType = "XRVD";
							string TransCode = "XRVD";
							double Credit = Convert.ToDouble(DataAccess.GetData(dtGetJEofDP, 0, "Credit", "0").ToString());
							int tempJENo = 0;

							//2023-10-10 : GET ALL JOURNAL ENTRIES OF FETCHED AR DOWNPAYMENT INVOICE FOR REFERENCE OF ACCOUNTS AND AMOUNTS
							if (cashRegister.CreateJournalEntry(company, ProjectCode, SAPCardCode, Convert.ToDouble(Debit), 0, "",
								"", Block, Lot, ARAccount, DFCAccount, "", JEType, DocNum.ToString(), "", "", "", IncomingPaymentDate, TransCode,
								out tempJENo, out tempMessage, "", Credit))
							{

							}
							else
							{
								errorTag++;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				alertMsg($@"{ex.Message} -- (Reversal of cancelDFC Journal Entry posting).", "info");
			}
		}









		private void cancelJEForPartialPayments(string JECode, string TransCode)
		{
			try
			{
				//TCP
				//2023-10-17 : CREATE JOURNAL ENTRY FOR REVERSAL OF JE1 WHEN INCOMING PAYMENT IS CANCELLED
				SapHanaLayer company = new SapHanaLayer();
				CashRegisterService cashRegister = new CashRegisterService();



				//2023-10-17 : GET ALL JEs WITH CANCELLED INCOMING PAYMENTS -- CONNECTED VIA RECEIPT NUMBER
				string qryJE = $@"SELECT
		                            A.""TransId"",
		                            A.""Project"",
		                            A.""U_BlockNo"", 
		                            A.""U_LotNo"",
		                            A.""Ref2"" ""DocNum"",
		                            A.""Ref1"",
		                            A.""U_ORNo"",
		                            B.""CancelDate"",
		                            (SELECT DISTINCT x.""Account"" FROM JDT1 x WHERE x.""TransId"" = A.""TransId"" AND ifnull(x.""Debit"",0) > 0) ""DebitAccount"",
	                                (SELECT SUM(IFNULL(x.""Debit"",0)) FROM JDT1 x WHERE x.""TransId"" = A.""TransId"" AND ifnull(x.""Debit"",0) > 0) ""Debit"", 
	                                (SELECT DISTINCT x.""ShortName"" FROM JDT1 x WHERE x.""TransId"" = A.""TransId"" AND ifnull(x.""Debit"",0) = 0) ""ShortName"",
	                                (SELECT DISTINCT x.""Account"" FROM JDT1 x WHERE x.""TransId"" = A.""TransId"" AND ifnull(x.""Debit"",0) = 0 ) ""CreditAccount"",
	                                (SELECT SUM(IFNULL(x.""Credit"",0)) FROM JDT1 x WHERE x.""TransId"" = A.""TransId"" AND ifnull(x.""Debit"",0) = 0) ""Credit""
	                            FROM
		                            OJDT A INNER JOIN
		                            ORCT B ON A.""U_ORNo"" = (CASE WHEN IFNULL(B.""U_ORNo"",'') <> '' THEN B.""U_ORNo"" 
                                              WHEN IFNULL(B.""U_ARNo"",'') <> '' THEN B.""U_ARNo"" ELSE B.""U_PRNo"" END) AND 
			                                  B.""Canceled"" = 'Y' AND
			                                  A.""TransCode"" = '{TransCode}' AND
                                              IFNULL(A.""U_CancelTag"",'') <> 'Y' AND 
                                              A.""Ref3"" NOT IN ('JE2','JE4')

                                WHERE
		                            (SELECT 
			                             COUNT(x.""TransId"") 
		                             from 
			                             OJDT x 
		                             WHERE 
			                             x.""U_ORNo"" = A.""U_ORNo"" AND
			                             IFNULL(x.""TransCode"",'') = '{JECode}') = 0 AND
	                                (SELECT 
	                                     COUNT(x.""TransId"") 
	                                 from 
	                                     OJDT x 
	                                 WHERE  
	     	                             x.""U_JE1TransID"" = A.""TransId"" AND
	                                     IFNULL(x.""TransCode"",'') IN ('JE2','JE4')) = 0
	                            ORDER BY
		                            B.""DocEntry""	 

                                ";
				DataTable dtJE = hana.GetData(qryJE, hana.GetConnection("SAPHana"));

				//declare variable for error
				int errorTag = 0;
				string tempMessage = "";

				if (dtJE.Rows.Count > 0)
				{
					foreach (DataRow row in dtJE.Rows)
					{
						string DocNum = row["DocNum"].ToString();
						string CancelledIncomingPaymentDocDate = Convert.ToDateTime(row["CancelDate"].ToString()).ToString("yyyyMMdd");
						string ORNo = row["U_ORNo"].ToString();
						string TransId = row["TransId"].ToString();


						//2023-10-17 : GET ALL DATA FROM PREVIOUS JOURNAL ENTRIES 
						string ProjectCode = DataAccess.GetData(dtJE, 0, "Project", "").ToString();
						string Block = DataAccess.GetData(dtJE, 0, "U_BlockNo", "").ToString();
						string Lot = DataAccess.GetData(dtJE, 0, "U_LotNo", "").ToString();
						string SAPCardCode = DataAccess.GetData(dtJE, 0, "ShortName", "").ToString();

						string DebitAccount = DataAccess.GetData(dtJE, 0, "DebitAccount", "").ToString();
						double Debit = Convert.ToDouble(DataAccess.GetData(dtJE, 0, "Debit", "0").ToString());
						string CreditAccount = DataAccess.GetData(dtJE, 0, "CreditAccount", "").ToString();
						double Credit = Convert.ToDouble(DataAccess.GetData(dtJE, 0, "Credit", "0").ToString());


						int tempJENo = 0;

						//2023-10-10 : GET ALL JOURNAL ENTRIES OF FETCHED AR DOWNPAYMENT INVOICE FOR REFERENCE OF ACCOUNTS AND AMOUNTS
						if (cashRegister.CreateJournalEntry(company, ProjectCode, //2
															SAPCardCode, Convert.ToDouble(Debit), //4
															0, "", //6
															"", Block, //8
															Lot, CreditAccount, //10 
															DebitAccount, "", //12
															JECode, DocNum, //14
															"", "", //16
															"", CancelledIncomingPaymentDocDate, //18
															JECode, out tempJENo, //20
															out tempMessage, ORNo))
						{
							//2023-10-20 : UPDATE JE WITH CANCELTAG
							string qryUpdateJEWithCancelTag = $@"UPDATE OJDT SET ""U_CancelTag"" = 'Y' Where ""TransId"" = {TransId}";
							hana.Execute(qryUpdateJEWithCancelTag, hana.GetConnection("SAPHana"));
						}
						else
						{
							errorTag++;
						}
					}
				}


				if (errorTag > 0)
				{
					alertMsg($@"An error occurred while creating entries for cancelled Partial Payments - TCP: {JECode} ({tempMessage}). Please contact administrator.", "info");
				}
			}
			catch (Exception ex)
			{
				alertMsg($@"{ex.Message} -- ({JECode} Journal Entry posting).", "info");
			}
		}






		private void cancelMiscellaneousInvoices()
		{
			//2023-10-19 : CANCEL ALL AR INVOICES WITH MISCELLANEOUS TRANSACTIONTYPE AND CANCELLED INCOMING PAYMENTS
			SapHanaLayer company = new SapHanaLayer();

			string qry = $@" SELECT
                                A.""DocEntry"",A.""DocNum"" 
                             FROM OINV A WHERE 
                             (SELECT
                                    COUNT(x.""DocEntry"")
                              FROM
                                    ORCT x INNER JOIN
		                            RCT2 y ON x.""DocEntry"" = y.""DocNum"" AND y.""InvType"" = 13 AND x.""Canceled"" = 'Y' AND y.""DocEntry"" = A.""DocEntry""
	                         ) > 0 AND
	                         A.""PaidToDate"" = 0 AND
	                         A.""U_Type"" = 'MISC' AND 	
	                         A.""DocStatus"" = 'O' AND 
	                         A.""CANCELED"" NOT IN ('Y','C')
                             ";
			DataTable dt = hana.GetData(qry, hana.GetConnection("SAPHana"));

			//declare variable for error
			int errorTag = 0;

			if (dt.Rows.Count > 0)
			{
				foreach (DataRow row in dt.Rows)
				{
					string InvoiceDocEntry = row["DocEntry"].ToString();
					if (!company.POST($"Invoices({InvoiceDocEntry})/Cancel", new StringBuilder()))
					{
						errorTag = 1;
					}
					else
					{
						hana.Execute($@"UPDATE ""OINV"" SET ""U_Remarks"" = 'CANCELLED BY DREAMS -- MISCELLANEOUS : {System.DateTime.Now}' WHERE ""DocEntry"" = '{InvoiceDocEntry}';", hana.GetConnection("SAPHana"));
					}
				}
			}


			if (errorTag > 0)
			{
				alertMsg("An error occurred while cancelling an open AR Invoice (Miscellaneous) with cancelled Incoming Payment. Please contact administrator.", "info");
			}
		}





		private void cancelNLTS(string JECode, string TransCode)
		{
			try
			{
				//TCP
				//2023-11-10 : CREATE JOURNAL ENTRY FOR REVERSAL OF NLTS WHEN 
				SapHanaLayer company = new SapHanaLayer();
				CashRegisterService cashRegister = new CashRegisterService();



				//2023-11-10 : GET ALL JEs WITH CANCELLED INCOMING PAYMENTS -- CONNECTED VIA RECEIPT NUMBER
				string qryJE = $@"SELECT
		                            A.""TransId"",
		                            A.""Project"",
		                            A.""U_BlockNo"", 
		                            A.""U_LotNo"",
		                            A.""Ref2"" ""DocNum"",
		                            A.""Ref1"",
		                            A.""U_ORNo"",
		                            B.""CancelDate"",
		                            (SELECT DISTINCT x.""Account"" FROM JDT1 x WHERE x.""TransId"" = A.""TransId"" AND ifnull(x.""Debit"",0) > 0) ""DebitAccount"",
	                                (SELECT SUM(IFNULL(x.""Debit"",0)) FROM JDT1 x WHERE x.""TransId"" = A.""TransId"" AND ifnull(x.""Debit"",0) > 0) ""Debit"", 
	                                (SELECT DISTINCT x.""ShortName"" FROM JDT1 x WHERE x.""TransId"" = A.""TransId"" AND ifnull(x.""Debit"",0) = 0) ""ShortName"",
	                                (SELECT DISTINCT x.""Account"" FROM JDT1 x WHERE x.""TransId"" = A.""TransId"" AND ifnull(x.""Debit"",0) = 0 ) ""CreditAccount"",
	                                (SELECT SUM(IFNULL(x.""Credit"",0)) FROM JDT1 x WHERE x.""TransId"" = A.""TransId"" AND ifnull(x.""Debit"",0) = 0) ""Credit"",
									A.""U_CardCode""
	                            FROM
		                            OJDT A INNER JOIN
		                            ORCT B ON A.""U_ORNo"" = (CASE WHEN IFNULL(B.""U_ORNo"",'') <> '' THEN B.""U_ORNo"" 
                                              WHEN IFNULL(B.""U_ARNo"",'') <> '' THEN B.""U_ARNo"" ELSE B.""U_PRNo"" END) AND 
			                                  B.""Canceled"" = 'Y' AND
			                                  A.""TransCode"" = '{TransCode}' AND
                                              IFNULL(A.""U_CancelTag"",'') <> 'Y'  
                                WHERE
		                            (SELECT 
			                             COUNT(x.""TransId"") 
		                             from 
			                             OJDT x 
		                             WHERE 
			                             x.""U_ORNo"" = A.""U_ORNo"" AND
			                             IFNULL(x.""TransCode"",'') = '{JECode}') = 0  
	                            ORDER BY
		                            B.""DocEntry""	 

                                ";
				DataTable dtJE = hana.GetData(qryJE, hana.GetConnection("SAPHana"));

				//declare variable for error
				int errorTag = 0;
				string tempMessage = "";

				if (dtJE.Rows.Count > 0)
				{
					foreach (DataRow row in dtJE.Rows)
					{
						string DocNum = row["DocNum"].ToString();
						string CancelledIncomingPaymentDocDate = Convert.ToDateTime(row["CancelDate"].ToString()).ToString("yyyyMMdd");
						string ORNo = row["U_ORNo"].ToString();
						string TransId = row["TransId"].ToString();


						//2023-10-17 : GET ALL DATA FROM PREVIOUS JOURNAL ENTRIES 
						string ProjectCode = DataAccess.GetData(dtJE, 0, "Project", "").ToString();
						string Block = DataAccess.GetData(dtJE, 0, "U_BlockNo", "").ToString();
						string Lot = DataAccess.GetData(dtJE, 0, "U_LotNo", "").ToString();
						//2024-11-20 : GET U_CARDCODE INSTEAD OF JE ROWS
						//string SAPCardCode = DataAccess.GetData(dtJE, 0, "ShortName", "").ToString();
						string SAPCardCode = DataAccess.GetData(dtJE, 0, "U_CardCode", "").ToString();

						//2024-11-20 : GET SHORTNAME 
						string ShortName = DataAccess.GetData(dtJE, 0, "ShortName", "").ToString();


						string DebitAccount = DataAccess.GetData(dtJE, 0, "DebitAccount", "").ToString();
						double Debit = Convert.ToDouble(DataAccess.GetData(dtJE, 0, "Debit", "0").ToString());
						string CreditAccount = DataAccess.GetData(dtJE, 0, "CreditAccount", "").ToString();
						double Credit = Convert.ToDouble(DataAccess.GetData(dtJE, 0, "Credit", "0").ToString());


						int tempJENo = 0;

						if (cashRegister.CreateJournalEntry(company, ProjectCode, //2
															SAPCardCode, Convert.ToDouble(Debit), //4
															0, "", //6
															"", Block, //8
															Lot, CreditAccount, //10 
															DebitAccount, ShortName, //12
															JECode, DocNum, //14
															"", "", //16
															"", CancelledIncomingPaymentDocDate, //18
															JECode, out tempJENo, //20
															out tempMessage, ORNo))
						{
						}
						else
						{
							errorTag++;
						}
					}
				}


				if (errorTag > 0)
				{
					alertMsg($@"An error occurred while creating entries for cancelled Payments : {JECode} ({tempMessage}). Please contact administrator.", "info");
				}
			}
			catch (Exception ex)
			{
				alertMsg($@"{ex.Message} -- ({JECode} Journal Entry posting).", "info");
			}
		}




		//2023-11-14 : NOT TO BE USED ANYMORE BECAUSE THEY ARE REDUNDANT
		private void cancelNLTSReversal(string DocNum, string IncomingPaymentDate, string Type)
		{
			try
			{

				//2023-11-10 : REVERSAL OF JOURNAL ENTRY OF CANCELLED DFCs -- TCP / MISC
				SapHanaLayer company = new SapHanaLayer();
				CashRegisterService cashRegister = new CashRegisterService();
				//2023-11-10 : CHECK IF JE ALREADY EXISTS
				string qryCheckExitingJE = $@"SELECT * FROM OJDT WHERE ""Ref2"" = '{DocNum}'";
				DataTable dtCheckExitingJE = hana.GetData(qryCheckExitingJE, hana.GetConnection("SAPHana"));


				//declare variable for error
				int errorTag = 0;
				string tempMessage = "";

				if (dtCheckExitingJE.Rows.Count > 0)
				{

					//2023-11-10 : CHECK IF REVERSAL ALREADY EXISTS 
					string qryCheckIfExistingJEReversalExists = $@"SELECT * FROM OJDT WHERE ""Ref2"" = '{DocNum}' AND ""TransCode"" = '{Type}'";
					DataTable dtCheckIfExistingJEReversalExists = hana.GetData(qryCheckIfExistingJEReversalExists, hana.GetConnection("SAPHana"));
					if (dtCheckIfExistingJEReversalExists.Rows.Count == 0)
					{
						int JEEntry = Convert.ToInt32(DataAccess.GetData(dtCheckExitingJE, 0, "TransId", "0").ToString());

						//2023-11-10 : GET ALL JOURNAL ENTRIES OF FETCHED AR DOWNPAYMENT INVOICE FOR REFERENCE OF ACCOUNTS AND AMOUNTSe
						string qryGetJEofDP = $@"SELECT A.""TransId"",A.""U_Project"",A.""Project"", A.""U_BlockNo"", A.""U_LotNo"",A.""Ref2"" ""DocNum"",A.""RefDate"" ""PostingDate"",""Ref2"",  
	                                            (SELECT DISTINCT x.""ShortName"" FROM JDT1 x WHERE x.""TransId"" = A.""TransId"" AND ifnull(x.""Credit"",0) > 0) ""ShortName"",
                                                (SELECT DISTINCT x.""Account"" FROM JDT1 x WHERE x.""TransId"" = A.""TransId"" AND ifnull(x.""Credit"",0) > 0) ""DebitAccount"",
                                                (SELECT SUM(IFNULL(x.""Debit"",0)) FROM JDT1 x WHERE x.""TransId"" = A.""TransId"" AND ifnull(x.""Debit"",0) > 0) ""Debit"", 
                                                (SELECT DISTINCT x.""Account"" FROM JDT1 x WHERE x.""TransId"" = A.""TransId"" AND ifnull(x.""Debit"",0) > 0) ""CreditAccount"",
                                                (SELECT SUM(IFNULL(x.""Credit"",0)) FROM JDT1 x WHERE x.""TransId"" = A.""TransId"" AND ifnull(x.""Credit"",0) > 0) ""Credit""
                                            FROM OJDT A WHERE A.""TransType"" = 30 AND A.""TransId"" = {JEEntry}   
                                            ";

						DataTable dtGetJEofDP = hana.GetData(qryGetJEofDP, hana.GetConnection("SAPHana"));
						if (dtGetJEofDP.Rows.Count > 0)
						{
							//2023-10-10 : GET ALL DATA FROM PREVIOUS JOURNAL ENTRIES 
							string ProjectCode = DataAccess.GetData(dtGetJEofDP, 0, "Project", "").ToString();
							string SAPCardCode = DataAccess.GetData(dtGetJEofDP, 0, "ShortName", "").ToString();
							double Debit = Convert.ToDouble(DataAccess.GetData(dtGetJEofDP, 0, "Debit", "0").ToString());
							string Block = DataAccess.GetData(dtGetJEofDP, 0, "U_BlockNo", "").ToString();
							string Lot = DataAccess.GetData(dtGetJEofDP, 0, "U_LotNo", "").ToString();
							string DebitAccount = DataAccess.GetData(dtGetJEofDP, 0, "DebitAccount", "").ToString();
							string CreditAccount = DataAccess.GetData(dtGetJEofDP, 0, "CreditAccount", "").ToString();
							string JEType = Type;
							string TransCode = Type;
							double Credit = Convert.ToDouble(DataAccess.GetData(dtGetJEofDP, 0, "Credit", "0").ToString());
							int tempJENo = 0;

							//2023-10-10 : GET ALL JOURNAL ENTRIES OF FETCHED AR DOWNPAYMENT INVOICE FOR REFERENCE OF ACCOUNTS AND AMOUNTS
							if (cashRegister.CreateJournalEntry(company, ProjectCode,
																SAPCardCode, Convert.ToDouble(Debit), 0, "",
																"", Block,
																Lot, CreditAccount,
																DebitAccount, "",
																JEType, DocNum.ToString(),
																"", "",
																"", IncomingPaymentDate,
																TransCode, out tempJENo,
																out tempMessage, "",
																Credit))
							{

							}
							else
							{
								errorTag++;
							}

							if (errorTag > 0)
							{
								alertMsg($@"An error occurred while creating entries for Reversal of cancelNLTS Journal Entry : {DocNum} ({tempMessage}). Please contact administrator.", "info");
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				alertMsg($@"{ex.Message} -- (Reversal of cancelNLTS Journal Entry posting).", "info");
			}
		}












	}
}

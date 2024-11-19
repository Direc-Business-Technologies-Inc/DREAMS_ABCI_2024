using DirecLayer;
using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABROWN_DREAMS
{
    public partial class Quotation : Page
    {
        DirecWebService ws = new DirecWebService();
        SAPHanaAccess hana = new SAPHanaAccess();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserID"] == null)
            {
                alertMsg("Session expired!", "error");
                Response.Redirect("~/pages/Login.aspx");
            }

            Session["ReportType"] = "";

            if (!IsPostBack)
            {
                ViewState["DocNum"] = "";
                hAddCharges.Visible = false;
                btnEditProfile.Visible = false;
                ViewState["UpdatedDPDueDate"] = 0;
                lblFinish.InnerText = "Finish";
                DateTime oDate = DateTime.Now;
                tDocDate.Text = oDate.ToString("yyyy-MM-dd");
                tMiscDocDate.Text = oDate.ToString("yyyy-MM-dd");
                txtDPDueDate.Text = Convert.ToDateTime(tDocDate.Text).AddMonths(1).ToString("yyyy-MM-dd");
                //dtpDueDate.Text = oDate.ToString("yyyy-MM-dd");
                lblMiscDueDate.Text = Convert.ToDateTime(tDocDate.Text).AddMonths(1).ToString("yyyy-MM-dd");

                //VIEWSTATES FOR UPDATING ENABLED FIELDS
                ViewState["UpdateResFee"] = 0;
                ViewState["UpdateMiscFee"] = 0;


                DataTable taxclass = new DataTable();
                taxclass.Columns.AddRange(new DataColumn[2]
                        {
                        new DataColumn("Code"),
                        new DataColumn("Name")
                        });
                ViewState["TaxClass"] = taxclass;

                DataTable coowner = new DataTable();
                coowner.Columns.AddRange(new DataColumn[2]
                        {
                        new DataColumn("Code"),
                        new DataColumn("Name")
                        });
                ViewState["CoOwner"] = coowner;
                gvCoOwner.DataSource = coowner;
                gvCoOwner.DataBind();

                DataTable others = new DataTable();
                others.Columns.AddRange(new DataColumn[6]
                        {
                        new DataColumn("Row"),
                        new DataColumn("Name"),
                        new DataColumn("LastName"),
                        new DataColumn("FirstName"),
                        new DataColumn("MiddleName"),
                        new DataColumn("Relationship")
                        });
                ViewState["Others"] = others;
                gvOthers.DataSource = others;
                gvOthers.DataBind();




                ws.InitializeSPA((int)Session["UserID"]);

                RefreshSalesList();
                LoadFromFindBtn();
                //deleteSampleQuotation();
                loadInitialSalesAgent();


                // ONLY SIR REP CAN U[DATE LB TERMS NAD INTEREST RATE
                DataTable dtaccess = (DataTable)Session["UserAccess"];
                var access = dtaccess.Select($"CodeEncrypt= 'CEO'");

                if (access.Any())
                {
                    txtLTerms.Enabled = true;
                    txtFactorRate1.Enabled = true;
                }
                else
                {
                    txtLTerms.Enabled = false;
                    txtFactorRate1.Enabled = false;
                }

                //--COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                txtAdjLot.Value = "No";

                CustomValidator14.Enabled = true;

                ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "MsgBuyers_Show();", true);
            }


        }

        void loadInitialSalesAgent()
        {
            string qry = $@"select A.""Username"",C.""Id"",C.""TransID"" from OUSR A INNER JOIN BRK1 B ON A.""Username"" = B.""SAPCardCode"" 
                                     INNER JOIN OSLA C ON C.""Id"" = B.""Id"" where A.""ID"" = {Session["UserID"]}";
            DataTable dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
            if (DataAccess.GetData(dt, 0, "Username", "0").ToString().Contains("SP-"))
            {
                qry = $@"SELECT *,
	                            (SELECT
		                            CASE WHEN IFNULL(x.""Partnership"",'') = '' THEN x.""FirstName"" || ' ' || x.""MiddleName"" || ' ' || x.""LastName""
		                            ELSE x.""Partnership""
		                            END
	                            FROM
		                            OBRK x
	                            WHERE
		                            x.""BrokerId"" = A.""CreateBrokerID"") ""Name"" 
                        FROM OSLA A WHERE A.""TransID"" = '{DataAccess.GetData(dt, 0, "TransID", "0").ToString()}'";
                dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
                //lblEmployeeID.Text = DataAccess.GetData(dt, 0, "TransID", "0").ToString();
                lblEmployeeID.Text = DataAccess.GetData(dt, 0, "Id", "0").ToString();
                lblEmployeeName.Text = DataAccess.GetData(dt, 0, "SalesPerson", "").ToString();
                lblEmployeePosition.Text = DataAccess.GetData(dt, 0, "Position", "").ToString();
                lblEmployeeBrokerID.Text = DataAccess.GetData(dt, 0, "BrokerId", "").ToString();


                //2023-11-07 : ADDED BROKER NAME
                getBrokerName(lblEmployeeID.Text);
            }
        }

        void getBrokerName(string Id)
        {
            string qry = $@"SELECT *,
	                            (SELECT
		                            CASE WHEN IFNULL(x.""Partnership"",'') = '' THEN x.""FirstName"" || ' ' || x.""MiddleName"" || ' ' || x.""LastName""
		                            ELSE x.""Partnership""
		                            END
	                            FROM
		                            OBRK x
	                            WHERE
		                            x.""BrokerId"" = A.""CreateBrokerID"") ""Name"" 
                        FROM OSLA A WHERE UPPER(A.""Id"") = UPPER('{Id}')";
            DataTable dt = hana.GetData(qry, hana.GetConnection("SAOHana"));

            //2023-11-07 : ADDED BROKER NAME
            lblEmployeeBrokerName.Text = DataAccess.GetData(dt, 0, "Name", "").ToString();
        }

        //void deleteSampleQuotation()
        //{
        //    hana.Execute($@"DELETE FROM OQUT WHERE ""DocEntry"" = 0;", hana.GetConnection("SAOHana"));
        //}

        void loadCoMaker()
        {
            string qrytemp = $@"select IFNULL(""LastName"",'') || ', ' || IFNULL(""FirstName"",'') || ' ' || IFNULL(""MiddleName"",'') ""Name""  from ""temp_CRD5"" where ""UserID"" = '{Session["UserID"].ToString()}' and ""CoBorrower"" = true";
            string qry = $@"select IFNULL(""LastName"",'') || ', ' || IFNULL(""FirstName"",'') || ' ' || IFNULL(""MiddleName"",'') ""Name""   from crd5 where ""CoBorrower"" = true and ""CardCode"" = '{txtCardCode.Value}' union all ";
            DataTable dt = String.IsNullOrEmpty(txtCardCode.Value) ? hana.GetData(qrytemp, hana.GetConnection("SAOHana")) : hana.GetData(qry + qrytemp, hana.GetConnection("SAOHana"));
            gvCoMaker.DataSource = dt;
            gvCoMaker.DataBind();
        }


        //STANDALONE LOCAL PROCESSES//


        void RefreshSalesList()
        {
            ////Session["gvPosList"] = hana.GetData(@"SELECT ""Code"",""Name"" FROM ""OLST"" WHERE ""GrpCode"" = 'POS'", hana.GetConnection("SAOHana"));
            //Session["gvPosList"] = hana.GetData(@"SELECT 'Agent' ""Code"",'Agent' ""Name"" FROM DUMMY", hana.GetConnection("SAOHana"));
            //LoadData(gvPosList, "gvPosList");
        }
        void LoadFromFindBtn()
        {
            DataTable dt = new DataTable();

            // ONLY SIR REP CAN U[DATE LB TERMS NAD INTEREST RATE
            DataTable dtaccess = (DataTable)Session["UserAccess"];
            var access = dtaccess.Select($"CodeEncrypt= 'CEO'");

            if (access.Any())
            {
                dt = hana.GetData($"CALL sp_GetQuotation (1);", hana.GetConnection("SAOHana"));
            }
            else
            {
                dt = hana.GetData($"CALL sp_GetQuotation ({Session["UserID"]});", hana.GetConnection("SAOHana"));
            }


            //dt = hana.GetData($"CALL sp_GetQuotation ({Session["UserID"]});", hana.GetConnection("SAOHana"));
            if (DataAccess.Exist(dt) == true)
            {
                ViewState["dtBuyers"] = dt;
                LoadData(gvBuyers, "dtBuyers");
            }
        }
        void LoadData(GridView gv, string viewState)
        {
            try
            {
                gv.DataSource = (DataTable)ViewState[viewState];
                gv.DataBind();
            }
            catch (Exception ex) { }
        }
        void NextTab(string tab)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "NextTab", $"NextTab('{tab}');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "MsgBuyers_Hide();", true);
        }
        void PrevTab(string tab)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "QuotationSummary", $"PrevTab('QuotationSummary');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "PaymentTerms", $"PrevTab('PaymentTerms');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "HouseDetails", $"PrevTab('HouseDetails');", true);
        }
        void ClearAll(string oClearType)
        {
            switch (oClearType)
            {
                case "Project":
                    hPrjCode.Value = String.Empty;
                    tProjName.Value = String.Empty;
                    hBlockWidth.Value = String.Empty;
                    hBlockHeight.Value = String.Empty;
                    tPhase.Value = String.Empty;
                    tBlock.Value = String.Empty;
                    break;
                case "Block":
                    tBlock.Value = String.Empty;
                    break;
            }

            tFinancing.Value = String.Empty;
            tPhase.Value = String.Empty;
            txtLotClassification.Value = String.Empty;
            txtProductType.Value = String.Empty;

            clearFigureTextBoxes();

            hLotWidth.Value = String.Empty;
            hLotHeight.Value = String.Empty;
            tLot.Value = String.Empty;
            hLotWidth.Value = String.Empty;
            hLotHeight.Value = String.Empty;
            tLotArea.Value = String.Empty;
            tFloorArea.Value = String.Empty;
            tRetType.Value = String.Empty;
            txtLotNo.Value = String.Empty;
            tModel.Value = String.Empty;

            //<% --COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
            //txtAdjLot.Value = String.Empty;

            tResrvFee.Text = "0.00";
            tTerms.Text = "0";
            ViewState["IsGetHouse"] = false;
            ViewState["IsGetSize"] = false;
            ViewState["IsGetFeat"] = false;
            tHouseStatus.Value = String.Empty;
            tSize.Value = String.Empty;
            tFeature.Value = String.Empty;
            txtIncentiveOption.Value = String.Empty;
            txtIncentiveOption2.Value = String.Empty;

            txtFinancingMisc.Value = String.Empty;
            lblMiscFinancingScheme.Text = String.Empty;


            clearFields();
        }
        int CalculateYourAge(DateTime Dob)
        {
            DateTime Now = DateTime.Now;
            int Years = new DateTime(DateTime.Now.Subtract(Dob).Ticks).Year - 1;
            DateTime PastYearDate = Dob.AddYears(Years);
            int Months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (PastYearDate.AddMonths(i) == Now)
                {
                    Months = i;
                    break;
                }
                else if (PastYearDate.AddMonths(i) >= Now)
                {
                    Months = i - 1;
                    break;
                }
            }
            int Days = Now.Subtract(PastYearDate.AddMonths(Months)).Days;
            int Hours = Now.Subtract(PastYearDate).Hours;
            int Minutes = Now.Subtract(PastYearDate).Minutes;
            int Seconds = Now.Subtract(PastYearDate).Seconds;
            return Years;
        }
        void Clear()
        {
            lblID.Text = string.Empty;
            lblDocNum.Text = string.Empty;
            lblName.Text = string.Empty;
            lblFirstName.Text = string.Empty;
            lblLastName.Text = string.Empty;
            lblMiddleName.Text = string.Empty;
            lblComaker.Text = string.Empty;
            lblNatureofEmployment.Text = string.Empty;

            //2023-05-05 : REQUESTED BY DHEZA
            lblTaxClassification.Text = string.Empty;

            lblTypeofID.Text = string.Empty;
            lblIDNo.Text = string.Empty;
            lblBirthday.Text = string.Empty;
            tFinancing.Value = string.Empty;
            tAccountType.Value = string.Empty;
            lblBusinessType.Text = string.Empty;

            ddlBusinessType.SelectedValue = "Individual";

            ViewState["DocNum"] = "";

            ClearAll("Project");
            PrevTab("HouseDetails");
            ScriptManager.RegisterStartupScript(this, GetType(), "clear", "ResetTextBox()", true);
            RefreshSalesList();
            clearFields();
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

            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "showAlert();", true);
        }
        public double Ceiling(double value, double significance)
        {
            if ((value % significance) != 0)
            {
                return ((int)(value / significance) * significance) + significance;
            }

            return Convert.ToDouble(value);
        }
        void clearFields()
        {
            lblDASAmt.Text = "0.00";
            lblPromoDisc.Text = "0.00";
            lblDiscount.Text = "0.00";
            lblNetDAS.Text = "0.00";
            lblNetDAS2.Text = "0.00";

            lblMiscFinancingScheme.Text = "";
            lblMiscFees.Text = "0.00";
            lblMiscDPAmount.Text = "0.00";
            lblMiscDPTerms.Text = "0.00";
            lblMiscDPMonthly.Text = "0.00";
            lblMiscLBAmount.Text = "0.00";
            lblMiscLBTerms.Text = "0";
            lblMiscLBMonthly.Text = "0.00";

            lblAddMiscCharges.Text = "0.00";
            lblAddMiscCharges2.Text = "0.00";
            lblAddMiscFees.Text = "0.00";


            lblCompTotal.Text = "0.00";
            lblVAT.Text = "0.00";
            lblTCPFinScheme.Text = "";
            lblMiscFees.Text = "0.00";
            lblMiscDPTerms.Text = "0";
            lblNetTCP.Text = "0.00";
            lblTCPMonthly.Text = "0.00";
            lblLoanableBalance.Text = "0.00";
            lblReserveFee.Text = "0.00";
            lblReserveFee2.Text = "0.00";
            lblBalance.Text = "0.00";
            lblDPTerms.Text = "0";
            lblDownPayment.Text = "0.00";
            lblDPMonthly.Text = "0.00";
            lblDPDueDate.Text = "-";
            lblAddCharges.Text = "0.00";
            lblLBDueDate.Text = "-";
            lblAmountDue.Text = "0.00";
            lblLBMonthly.Text = "0.00";


        }
        protected void TextPercentage(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            double val = 0;
            val = SystemClass.TextIsZero(txt.Text);

            if (val > 100)
            { val = 100; }
            else if (val < 0)
            { val = 0; }

            txt.Text = val.ToString();
            Compute();

        }
        protected void TextAmount(object sender, EventArgs e)
        {
            ViewState["UpdateResFee"] = 1;


            TextBox txt = (TextBox)sender;
            double val = 0;
            val = SystemClass.TextIsZero(txt.Text);

            txt.Text = SystemClass.ToCurrency(val.ToString());

            ReservationUpdate();
            Compute();
        }


        void ClearBP()
        {
            txtLastName.Value = "";
            txtFirstName.Value = "";
            txtMiddleName.Value = "";
            txtComaker.Value = "";
            txtBirthday.Value = "";
            ViewState["Broker"] = "";
            ViewState["SalesManager"] = "";
            tNatureofEmp.Disabled = true;
            tNatureofEmp.Value = "";
            tTypeOfId.Disabled = true;
            tTypeOfId.Value = "";
            tIDNo.Value = "";
            tTIN1.Text = "";
            //tSalesType.Value = "";
            txtCardCode.Value = "";
        }
        void BoolBP(bool val)
        {
            txtLastName.Disabled = val;
            txtFirstName.Disabled = val;
            txtComaker.Disabled = val;
            txtMiddleName.Disabled = val;
            tTIN1.ReadOnly = val;
            tspecFName.Disabled = val;
            tspecMName.Disabled = val;
            tspecLName.Disabled = val;
            tspecRelationship.Disabled = val;
            txtCompanyName.Disabled = val;
            txtTinNumber.Disabled = val;
            txtBirthday.Disabled = val;
            ddlBusinessType.Enabled = !val;
            ddTaxClass.Enabled = !val;
            tIDNo.Disabled = true;

        }
        void LoadEmpList()
        {
            txtSearchEmpList.Value = "";
            string brokerID = ViewState["BrokerId"] == null ? "" : ViewState["BrokerId"].ToString();
            if (lblID.Text != "Sample Quotation")
            {
                ViewState["gvEmpList"] = ws.GetSalesEmployees(DateTime.Now, brokerID).Tables["GetSalesEmployees"];
                LoadData(gvEmpList, "gvEmpList");
            }

        }


        void Compute(int ChangeRetitlingTypeTag = 0)
        {
            //2023-10-19 : ADDED "ChangeRetitlingTypeTag" PARAMETER FOR RETITLING TYPE; DEFAULT MISC AMOUNT SHOULD BE 12,500
            if (Session["UserID"] == null)
            {
                alertMsg("Session expired!", "error");
                Response.Redirect("~/pages/Login.aspx");
            }

            try
            {
                DataTable dt = new DataTable();
                DateTime dtAge = DateTime.Now;
                int Age;

                if (!string.IsNullOrEmpty(lblBirthday.Text))
                { dtAge = DateTime.Parse(lblBirthday.Text); }

                Age = CalculateYourAge(dtAge);


                double ODAS = double.Parse(string.IsNullOrWhiteSpace(tODas.Value) ? "0" : tODas.Value);
                double PromoDisc = double.Parse(string.IsNullOrWhiteSpace(tPromoDisc.Text) ? "0" : tPromoDisc.Text);
                double DPPercent = double.Parse(string.IsNullOrWhiteSpace(tDPPercent.Text) ? "0" : tDPPercent.Text);
                double ResrvFee = double.Parse(string.IsNullOrWhiteSpace(tResrvFee.Text) ? "0" : tResrvFee.Text);
                double dpamount = double.Parse(string.IsNullOrWhiteSpace(tDPAmount.Text) ? "0" : tDPAmount.Text);
                double discountAmount = double.Parse(string.IsNullOrWhiteSpace(tSpotDPDiscAmt.Text) ? "0" : tSpotDPDiscAmt.Text);
                double discPrcent = double.Parse(string.IsNullOrWhiteSpace(txtDiscPercent.Text) ? "0" : txtDiscPercent.Text);
                //double AddChargeAmt = double.Parse(string.IsNullOrWhiteSpace(txtAddChargeAmount.Text) ? "0" : txtAddChargeAmount.Text);
                string housestat = txtProductType.Value;
                string adjacent = txtAdjLot.Value;
                int updatedDPDueDate = int.Parse(string.IsNullOrWhiteSpace(ViewState["UpdatedDPDueDate"].ToString()) ? "0" : ViewState["UpdatedDPDueDate"].ToString());
                int DPTerms = int.Parse(string.IsNullOrWhiteSpace(txtDPTerms.Text) ? "0" : txtDPTerms.Text);
                int LTerms = int.Parse(string.IsNullOrWhiteSpace(txtLTerms.Text) ? "0" : txtLTerms.Text);
                int MiscDPTerms = int.Parse(string.IsNullOrWhiteSpace(lblMiscDPTerms.Text) ? "0" : lblMiscDPTerms.Text);
                string RetType = tRetType.Value;
                int MiscLBTerm = int.Parse(string.IsNullOrWhiteSpace(lblMiscLBTerms.Text) ? "1" : lblMiscLBTerms.Text);

                if (LTerms == 0)
                {
                    LTerms = 1;
                }

                if (MiscLBTerm == 0)
                {
                    MiscLBTerm = 1;
                }

                if (MiscDPTerms == 0)
                {
                    MiscDPTerms = 1;
                }

                double AddChargeAmt = double.Parse(string.IsNullOrWhiteSpace(txtMiscFees.Text) ? "0" : txtMiscFees.Text);


                //string dtpDueDate1 = string.IsNullOrWhiteSpace(dtpDueDate.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : dtpDueDate.Text;
                string dtpDueDate1 = string.IsNullOrWhiteSpace(txtDPDueDate.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : txtDPDueDate.Text;
                double factorRate = double.Parse(string.IsNullOrWhiteSpace(txtFactorRate1.Text) ? "0" : txtFactorRate1.Text);
                //if (factorRate == 0)
                //{
                //    factorRate = 1;
                //}

                dt = ws.GetCompSheet(
                                     Age, tFinancing.Value,  //2
                                     housestat, ODAS,  //4
                                     PromoDisc, DPPercent,   //6
                                     ResrvFee, discountAmount,  //8
                                     ddlDiscBased.Text, DPTerms, //10
                                     LTerms, "Y", //12

                                     //PARAMETERS FOR COMPSHEET 
                                     dpamount, discPrcent, //14
                                     AddChargeAmt, ddlAllowed.Text, //16
                                     tSalesType.Value, txtProductType.Value, //18
                                     hPrjCode.Value, updatedDPDueDate, //20
                                     tDocDate.Text, Convert.ToDateTime(dtpDueDate1).Day.ToString(), //22

                                     txtDPDueDate.Text, Convert.ToInt32(ViewState["UpdatedDPDueDate"]), //24
                                     factorRate, RetType, //26
                                     txtAdjLot.Value, MiscDPTerms,
                                     decimal.Parse(lblMiscDPAmount.Text), MiscLBTerm
                                     ).Tables[0];

                if (DataAccess.Exist(dt) == true)
                {
                    tODas.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "ODas", "0"));
                    lblDASAmt.Text = tODas.Value;   //DAS Amount
                    lblPromoDisc.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "PromoDisc", "0"));  //Less Promo: Discount 

                    //tSpotDPDiscAmt.Text = string.IsNullOrWhiteSpace(tSpotDPDiscAmt.Text) ? "0" : tSpotDPDiscAmt.Text;
                    //tSpotDPDiscAmt.Text = SystemClass.ToCurrency(tSpotDPDiscAmt.Text);

                    //tDPPercent.Text = SystemClass.ToCurrency(SystemClass.TextIsZero((string)DataAccess.GetData(dt, 0, "DPPercent", "0")).ToString());

                    //tResrvFee.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "ResrvFee", "0"));
                    //txtDPTerms.Text = (string)DataAccess.GetData(dt, 0, "DPTerms", "0");
                    //ViewState["ActualDPTerms"] = int.Parse((string)DataAccess.GetData(dt, 0, "ActualDPTerms", "0"));
                    //if ((string)DataAccess.GetData(dt, 0, "LTerms", "0") != "0")
                    //{
                    //    txtLTerms.Text = (string)DataAccess.GetData(dt, 0, "LTerms", "0");
                    //}
                    //tRequiredMonthly.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "GDI", "0"));
                    //tDPAmount.Text = string.IsNullOrWhiteSpace(tDPAmount.Text) ? "0" : tDPAmount.Text;
                    //txtDiscPercent.Text = string.IsNullOrWhiteSpace(txtDiscPercent.Text) ? "0" : txtDiscPercent.Text;

                    //tInterestRate.Text = (string)DataAccess.GetData(dt, 0, "InterestRate", "0");
                    //tLPercent2.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "LPercent", "0"));
                    //tMonthlyAmort2.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "MonthlyAmort", "0"));
                    //tLMaturityAge.Text = (string)DataAccess.GetData(dt, 0, "LMaturityAge", "0");

                    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                    // ALLOCATING ALL THE RESULTS TO THEIR RESPECTIVE FIELDS
                    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ 
                    //computationSheet();

                    //DISCOUNT 
                    lblDiscount.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "Discount", "0"));
                    //NET DAS = Sum of DAS Amount, LessPromo Discount, and Discount
                    lblNetDAS.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "NetDAS", "0"));
                    //NET DAS second part
                    lblNetDAS2.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "NetDAS2", "0"));
                    //Add: Misc Charges
                    lblAddMiscCharges.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "AddMiscCharges", "0"));

                    //FACTOR RATE
                    //txtFactorRate.Value = (string)DataAccess.GetData(dt, 0, "FactorRate", "0");

                    //VAT
                    lblVAT.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "FinalVAT", "0"));
                    //NET TCP = Sum of netDas, AddMiscCharges, and vAT
                    lblNetTCP.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "NewNETTCP", "0"));

                    //TCP Breakdown: Down Payment
                    //lblDownPayment.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "PDBalance", "0"));

                    //TCP Breakdown: Misc Charges
                    lblAddMiscCharges2.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "AddMiscCharges", "0"));
                    // TCP Breakdown : MONTHLY
                    lblDPMonthly.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "initMonthly1", "0"));

                    //TCP Breakdown : DUE DATE 1 - DP DUE DATE
                    //int dtpDueDateVisible = Convert.ToInt32(DataAccess.GetData(dt, 0, "dtpDueDateVisible", "0"));
                    //if (dtpDueDateVisible == 0)
                    //{ dtpDueDate.Visible = false; }
                    //else { dtpDueDate.Visible = true; }

                    var date1 = (string)DataAccess.GetData(dt, 0, "initDueDate1", "");
                    if (date1 != "-") { date1 = Convert.ToDateTime((string)DataAccess.GetData(dt, 0, "initDueDate1", "")).ToString("MM-dd-yyyy"); }
                    lblDPDueDate.Text = date1;
                    //dtpDueDate.Text = Convert.ToDateTime((string)DataAccess.GetData(dt, 0, "dtpDueDate", "")).ToString("yyyy-MM-dd");
                    txtDPDueDate.Text = Convert.ToDateTime(DataAccess.GetData(dt, 0, "initDueDate1", "")).ToString("yyyy-MM-dd");
                    ViewState["UpdatedDPDueDate"] = 0;


                    //TCP Breakdown : Additional Charges
                    lblAddCharges.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "NewAdditionalCharges", "0"));

                    //TCP Breakdown : Due Date 2 - LB DUE DATE
                    var date2 = (string)DataAccess.GetData(dt, 0, "initDueDate2", "");
                    if (date2 != "-") { date2 = Convert.ToDateTime((string)DataAccess.GetData(dt, 0, "initDueDate2", "")).ToString("MM-dd-yyyy"); }
                    lblLBDueDate.Text = date2;

                    //TCP Breakdown : Loanable Amount
                    lblAmountDue.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "initFinance", "0"));

                    //TCP Breakdown : LB Monthly 2  
                    lblLBMonthly.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "monthly2", "0"));

                    //TCP Breakdown : Due Date 3 - MISC DUE DATE
                    var date3 = (string)DataAccess.GetData(dt, 0, "initDueDate3", "");
                    if (date3 != "-") { date3 = Convert.ToDateTime((string)DataAccess.GetData(dt, 0, "initDueDate3", "")).ToString("MM-dd-yyyy"); }
                    //lblDueDate3.Text = date3;

                    //Additional Chargese
                    txtAddChargeAmount.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "AddAdditionalCharge", "0"));

                    //Terms 
                    lblDPTerms.Text = txtDPTerms.Text;


                    //Balance on Equity
                    tPDBalance.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "PDBalance", "0"));
                    lblBalance.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "PDBalance", "0")).ToString();
                    //Miscellaneous Fees Breakdown
                    //string qry = $@"SELECT IFNULL(""U_BRBond"",0) Bond, IFNULL(""U_BRAdminFee"",0) AdminFee, ""U_Misc"" FROM ""@PRODUCTTYPE"" WHERE ""Code"" = '{txtProductType.Value}'";
                    //DataTable dtThes = hana.GetData(qry, hana.GetConnection("SAPHana"));

                    //MISC DUE DATE
                    lblMiscDueDate.Text = tMiscDocDate.Text;


                    //BASIS OF MISCELLANEOUS FEES
                    if (txtProductType.Value.ToUpper() == "LOT ONLY" && tRetType.Value.ToUpper() == "BUYERS")
                    {
                        //2023-10-19 : REMOVE STATIC AMOUNT FOR MISCFEE WHEN RETITLING TYPE IS BUYERS
                        //2023-10-19 : ADDED CONDITION 
                        // THIS IS TO CATER TANAY'S 15,000 AMOUNT INSTEAD OF 12,500
                        if (ChangeRetitlingTypeTag == 1)
                        {
                            string qry = $@"SELECT IFNULL(""U_BuyerRetitlingFee"",0)  + IFNULL(""U_BuyerRetitlingBond"",0) ""MiscFee""
                                                    FROM ""@PRODUCTTYPE"" WHERE UPPER(""Code"") = '{txtProductType.Value.ToUpper()}'";
                            DataTable dtProd = hana.GetData(qry, hana.GetConnection("SAPHana"));
                            if (dtProd.Rows.Count > 0)
                            {
                                lblMiscFees.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dtProd, 0, "MiscFee", "0")).ToString();
                                txtMiscFees.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dtProd, 0, "MiscFee", "0")).ToString();
                                lblAddMiscFees.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dtProd, 0, "MiscFee", "0")).ToString();
                                double miscmonthly = double.Parse(DataAccess.GetData(dtProd, 0, "MiscFee", "0").ToString());
                                lblMiscDPMonthly.Text = SystemClass.ToCurrency(miscmonthly.ToString()).ToString();
                                txtMiscMonthly.Text = SystemClass.ToCurrency(miscmonthly.ToString()).ToString();
                                lblMiscDPAmount.Text = SystemClass.ToCurrency(miscmonthly.ToString()).ToString();
                            }
                        }
                        else
                        {
                            //2023-10-19 - NEW WAY TO GET MISC FEES WHEN RETITLING TYPE = 'BUYERS'
                            {
                                txtMiscMonthly.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "CSAddMisc", "0")).ToString();
                                lblAddMiscFees.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "CSAddMisc", "0")).ToString();
                                //WHOLE MISC AMOUNT
                                lblMiscFees.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "CSAddMisc", "0")).ToString();


                                // MISC DP AMOUNT
                                lblMiscDPMonthly.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "CSAddMisc", "0")).ToString();
                                txtMiscFees.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "CSAddMisc", "0")).ToString();
                                lblMiscDPAmount.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "CSAddMisc", "0")).ToString();

                            }
                        }


                        //2023-10-19 : TERMS ARE ORIGINALLY LIKE THIS BEFORE CATERING TANAY'S REQUEST
                        {
                            lblMiscDPTerms.Text = "1";
                            txtMiscTerms.Text = "1";

                            lblMiscLBAmount.Text = "0.00";
                            lblMiscLBTerms.Text = "0";
                            lblMiscLBMonthly.Text = "0.00";
                        }
                    }
                    else
                    {
                        txtMiscMonthly.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "PDMiscFee", "0")).ToString();
                        lblAddMiscFees.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "CSAddMisc", "0")).ToString();
                        //WHOLE MISC AMOUNT
                        lblMiscFees.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "CSAddMisc", "0")).ToString();

                        // MISC DP AMOUNT
                        lblMiscDPMonthly.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "MiscDPMonthly", "0")).ToString();
                        txtMiscFees.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "CSAddMisc", "0")).ToString();
                        lblMiscDPTerms.Text = txtMiscTerms.Text;

                        //MISC LB AMOUNT
                        if (int.Parse(lblMiscLBTerms.Text) > 0)
                        {
                            lblMiscLBAmount.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "MiscLoanable", "0")).ToString();
                            lblMiscLBMonthly.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "MiscLBMonthly", "0")).ToString();
                        }
                        else
                        {
                            lblMiscLBAmount.Text = "0.00";
                            lblMiscLBMonthly.Text = "0.00";
                        }

                    }




                    //Monthly
                    txtDPMonthly.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "DPMonthly", "0")).ToString();
                    lblTCPMonthly.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "DPMonthly", "0")).ToString();
                    //Loanable Balance
                    txtLoanableBalance.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "PDLoanable", "0")).ToString();
                    lblLoanableBalance.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "TCPLoanable", "0")).ToString();
                    //Add Misc Fees
                    //Computation Sheet Total
                    lblCompTotal.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "CSTotal", "0")).ToString();
                    //Financial Scheme on Computation
                    lblTCPFinScheme.Text = tFinancing.Value;
                }
            }
            catch (Exception ex)
            { alertMsg(ex.Message, "error"); }
        }
        void AutoCompute()
        {
            // BUYER EVALUATION //
            tPBasicSalary.Text = SystemClass.ToCurrency(tPBasicSalary.Text);
            tPAllowances.Text = SystemClass.ToCurrency(tPAllowances.Text);
            tPCommissions.Text = SystemClass.ToCurrency(tPCommissions.Text);
            tPRentalIncome.Text = SystemClass.ToCurrency(tPRentalIncome.Text);
            tPRetainer.Text = SystemClass.ToCurrency(tPRetainer.Text);
            tPOthers.Text = SystemClass.ToCurrency(tPOthers.Text);
            tPTotal.Value = SystemClass.ToCurrency((double.Parse(tPBasicSalary.Text) + double.Parse(tPAllowances.Text)
                + double.Parse(tPCommissions.Text) + double.Parse(tPRentalIncome.Text)
                + double.Parse(tPRetainer.Text) + double.Parse(tPOthers.Text)).ToString());
            tSBasicSalary.Text = SystemClass.ToCurrency(tSBasicSalary.Text);
            tSAllowances.Text = SystemClass.ToCurrency(tSAllowances.Text);
            tSCommissions.Text = SystemClass.ToCurrency(tSCommissions.Text);
            tSRentalIncome.Text = SystemClass.ToCurrency(tSRentalIncome.Text);
            tSRetainer.Text = SystemClass.ToCurrency(tSRetainer.Text);
            tSOthers.Text = SystemClass.ToCurrency(tSOthers.Text);
            tSTotal.Value = SystemClass.ToCurrency((double.Parse(tSBasicSalary.Text) + double.Parse(tSAllowances.Text)
                + double.Parse(tSCommissions.Text) + double.Parse(tSRentalIncome.Text)
                + double.Parse(tSRetainer.Text) + double.Parse(tSOthers.Text)).ToString());
            tMITotal.Value = SystemClass.ToCurrency((double.Parse(tPTotal.Value) + double.Parse(tSTotal.Value)).ToString());
            tFood.Text = SystemClass.ToCurrency(tFood.Text);
            tLightWater.Text = SystemClass.ToCurrency(tLightWater.Text);
            tTelephoneBill.Text = SystemClass.ToCurrency(tTelephoneBill.Text);
            tTransportation.Text = SystemClass.ToCurrency(tTransportation.Text);
            tRent.Text = SystemClass.ToCurrency(tRent.Text);
            tEducation.Text = SystemClass.ToCurrency(tEducation.Text);
            tLoanAmort.Text = SystemClass.ToCurrency(tLoanAmort.Text);
            tMEOthers.Text = SystemClass.ToCurrency(tMEOthers.Text);
            tMETotal.Value = SystemClass.ToCurrency((double.Parse(tFood.Text) + double.Parse(tLightWater.Text)
                + double.Parse(tTelephoneBill.Text) + double.Parse(tTransportation.Text) + double.Parse(tRent.Text)
                + double.Parse(tEducation.Text) + double.Parse(tLoanAmort.Text) + double.Parse(tMEOthers.Text)).ToString());
            // Monthly Net Income
            tMNetIncome.Value = SystemClass.ToCurrency((double.Parse(tMITotal.Value) - double.Parse(tMETotal.Value)).ToString());
            // Gross Disposable Income
            tRequiredMonthly.Text = SystemClass.ToCurrency((double.Parse(tMonthlyAmort2.Text) / 0.35).ToString());
            // Monthly Amortization 
            tMonthlyAmort2.Text = SystemClass.ToCurrency(lblLBMonthly.Text);
            // Remarks
            lblRemarks.InnerText = Convert.ToDouble(tMNetIncome.Value) < Convert.ToDouble(tRequiredMonthly.Text) ? "FAILED" : "PASSED";
            lblRemarks.Attributes["style"] = Convert.ToDouble(tMNetIncome.Value) < Convert.ToDouble(tRequiredMonthly.Text) ? "color: red; margin-top: 0px; margin-bottom: 0px;" : "color: green; margin-top: 0px; margin-bottom: 0px;";
        }











































































        // CONTROLLER FUNCTIONS //

        protected void bNatureofEmp_ServerClick(object sender, EventArgs e)
        {
            Control btn = (Control)sender;
            string id = btn.ID;
            string txt = "Choose ";
            string GrpCode = "";

            if (id == "bNatureofEmp")
            { txt += "Nature of Employment"; GrpCode = "NE"; }
            else if (id == "bTypeofID")
            { txt += "Type of ID"; GrpCode = "ID"; }
            else if (id.Contains("bFinancing"))
            { txt += "Financing Scheme"; GrpCode = "FS"; }
            else if (id == "bAccountType")
            { txt += "Account Type"; GrpCode = "AT"; }
            else if (id == "bSalesType")
            { txt += "Sales Type"; GrpCode = "ST"; }
            else if (id == "bPosList")
            { txt += "Employees"; }

            ChooseText.InnerText = txt;

            string Query = "";
            string Con = "";

            if (id == "bSalesType")
            {
                Query = $@"SELECT ""FldValue"" ""Code"", ""Descr"" ""Name"" FROM ""UFD1"" WHERE ""TableID"" = 'OITT' AND ""FieldID"" = 3";
                Con = "SAPHana";
            }
            else if (id == "bPosList")
            {
                GridViewRow row = (GridViewRow)btn.NamingContainer;
                ViewState["index"] = row.RowIndex;

                Query = $@"CALL sp_GetListOfEmployees '{ConfigurationManager.AppSettings["HANADatabase"]}'";
                Con = "SAOHana";
            }
            else if (id.Contains("bFinancing"))
            {
                //Query = $@"SELECT ""Code"", ""Name"" FROM ""@FINANCINGSCHEME""";
                Query = $@"SELECT ""U_Code"" ""Code"",""U_Name"" ""Name"" FROM ""@FSC1"" WHERE ""Code"" = '{hPrjCode.Value}'";
                Con = "SAPHana";
            }
            else
            {
                Query = $@"SELECT ""Code"", ""Name"" FROM ""OLST"" WHERE ""GrpCode"" = '{GrpCode}'";
                Con = "SAOHana";
            }

            ViewState["gvList"] = hana.GetData(Query, hana.GetConnection(Con));
            LoadData(gvList, "gvList");

            ViewState["btnID"] = id;

        }
        protected void bSelect_Click(object sender, EventArgs e)
        {
            try
            {


                LinkButton GetID = (LinkButton)sender;
                string Code = GetID.CommandArgument;

                string GrpCode = (string)ViewState["btnID"];

                if (GrpCode == "bNatureofEmp")
                {
                    ViewState["tNatureofEmp"] = Code;

                    if (Code == "OTH")
                    { tNatureofEmp.Disabled = false; tNatureofEmp.Value = ""; }
                    else { tNatureofEmp.Disabled = true; tNatureofEmp.Value = ws.GetOLSTName(Code); }

                    //tTypeOfId.Disabled = true;
                    //tIDNo.Value = "";
                    //tIDNo.Disabled = false;
                    //tTypeOfId.Value = "";
                }
                else if (GrpCode == "bTypeofID")
                {
                    ViewState["tTypeOfId"] = Code;

                    if (Code == "OTH")
                    { tTypeOfId.Disabled = false; tTypeOfId.Value = ""; }
                    else { tTypeOfId.Disabled = true; tTypeOfId.Value = ws.GetOLSTName(Code); }

                    tIDNo.Value = "";
                    tIDNo.Disabled = false;
                }
                else if (GrpCode == "bFinancing")
                {
                    tFinancing.Value = ws.GetOLSTName(Code);

                    //// AUTOMATE SELECTED FINANCING SCHEME TO MISC FINSCHEME WHEN IT'S STILL BLANK
                    //if (string.IsNullOrWhiteSpace(txtFinancingMisc.Value))
                    //{
                    //    txtFinancingMisc.Value = tFinancing.Value;
                    //}

                    ViewState["tFinancing"] = Code;
                    //bGenerate_ServerClick(sender, e);
                    DataTable dt = ws.GetHouseDetails(hPrjCode.Value, tBlock.Value, tLot.Value, tFinancing.Value).Tables["GetHouseDetails"];
                    if (DataAccess.Exist(dt) == true)
                    {
                        GetHouseDetails(dt);
                    }

                    if (Code.ToUpper() == "SPOTCASH")
                    {
                        txtLoanType.Value = "SPOTCASH";
                        hideTermDiv("SPOTCASH");
                    }
                    Compute();

                }
                else if (GrpCode == "bFinancingMisc")
                {
                    txtFinancingMisc.Value = ws.GetOLSTName(Code);
                    lblMiscFinancingScheme.Text = txtFinancingMisc.Value;

                    ViewState["tFinancingMisc"] = Code;

                    DataTable dt = ws.GetHouseDetails(hPrjCode.Value, tBlock.Value, tLot.Value, txtFinancingMisc.Value).Tables["GetHouseDetails"];
                    if (DataAccess.Exist(dt) == true)
                    {
                        //GetHouseDetails(dt); 
                        GetMiscDetails(dt);
                    }


                    Compute();

                }
                else if (GrpCode == "bAccountType")
                {
                    tAccountType.Value = ws.GetOLSTName(Code);

                    ViewState["tAccountType"] = Code;
                }
                else if (GrpCode == "bSalesType")
                {
                    tSalesType.Value = ws.GetUFD1Name("OITT", Code, 3);

                    ViewState["tSalesType"] = Code;
                }
                else if (GrpCode == "bSize")
                {
                    tSize.Value = ws.GetUFD1Name("OBTN", Code, 23);

                    ViewState["tSize"] = Code;
                }
                else if (GrpCode == "bFeature")
                {
                    //tFeature.Value = (string)DataAccess.GetData(hana.GetData($@"SELECT ""Name"" FROM ""@HOUSE_FEAT"" WHERE ""U_Active"" = 'Y' AND ""Code"" = '{Code}'", hana.GetConnection("SAPHana")), 0, "Name", "");
                    tFeature.Value = "";

                    ViewState["tFeature"] = Code;
                }

                ScriptManager.RegisterStartupScript(this, GetType(), "MsgQuote", "MsgQuotation_Hide();", true);
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "info");
            }
        }
        protected void bSearch_ServerClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Value))
            {
                ViewState["dtBuyers"] = hana.GetData($"CALL sp_Search (2,'{txtSearch.Value}','{Session["UserID"]}');", hana.GetConnection("SAOHana"));
                LoadData(gvBuyers, "dtBuyers");
            }
            else
            { LoadFromFindBtn(); }
        }
        protected void bSelectBuyer_Click(object sender, EventArgs e)
        {
            try
            {

                //2023-03-23 : HIDE INCENTIVE OPTION IF SAMPLE QUOTATION
                divIncentive.Visible = true;
                divIncentive2.Visible = true;

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "clientCreateAmortClick();", true);
                LinkButton GetID = (LinkButton)sender;
                int DocEntry = int.Parse(GetID.CommandArgument);
                lblDocEntry.Text = DocEntry.ToString();
                ViewState["QuoteDocEntry"] = DocEntry;
                lblFinish.InnerText = "Update";
                DataTable dt = new DataTable();
                dt = ws.GetQuotationByID(DocEntry).Tables["GetQuotationByID"];
                if (DataAccess.Exist(dt) == true)
                {
                    ViewState["DocNum"] = (string)DataAccess.GetData(dt, 0, "DocNum", "");

                    lblID.Text = (string)DataAccess.GetData(dt, 0, "CardCode", "");
                    lblDocNum.Text = (string)DataAccess.GetData(dt, 0, "DocNum", "");

                    lblBusinessType.Text = (string)DataAccess.GetData(dt, 0, "BusinessType", "Individual");

                    loadDivisionsForNames(lblBusinessType.Text);

                    lblLastName.Text = (string)DataAccess.GetData(dt, 0, "LastName", "");
                    lblFirstName.Text = (string)DataAccess.GetData(dt, 0, "FirstName", "");
                    lblMiddleName.Text = (string)DataAccess.GetData(dt, 0, "MiddleName", "");
                    lblCompanyName.Text = (string)DataAccess.GetData(dt, 0, "CompanyName", "");
                    lblComaker.Text = (string)DataAccess.GetData(dt, 0, "Comaker", "");
                    lblName.Text = (string)DataAccess.GetData(dt, 0, "FullName", "");

                    txtComaker.Value = (string)DataAccess.GetData(dt, 0, "Comaker", "");
                    lblBirthday.Text = (string)DataAccess.GetData(dt, 0, "BirthDay", "");

                    string qry = $@"select ""Name"" from olst where ""GrpCode"" = 'NE' AND ""Code"" = '{DataAccess.GetData(dt, 0, "NatureEmp", "")}'";
                    DataTable dtX = hana.GetData(qry, hana.GetConnection("SAOHana"));
                    lblNatureofEmployment.Text = (string)DataAccess.GetData(dtX, 0, "Name", "");

                    //2023-05-05 : REQUESTED BY DHEZA
                    lblTaxClassification.Text = (string)DataAccess.GetData(dt, 0, "TaxClassification", "");

                    qry = $@"select ""Name"" from olst where ""GrpCode"" = 'ID' AND ""Code"" = '{DataAccess.GetData(dt, 0, "IDType", "")}'";
                    dtX = hana.GetData(qry, hana.GetConnection("SAOHana"));
                    lblTypeofID.Text = (string)DataAccess.GetData(dtX, 0, "Name", "");
                    lblIDNo.Text = (string)DataAccess.GetData(dt, 0, "IDNo", "");

                    lblTIN.Text = (string)DataAccess.GetData(dt, 0, "TIN", "");


                    tDocDate.Text = Convert.ToDateTime(DataAccess.GetData(dt, 0, "DocDate", "")).ToString("yyyy-MM-dd");
                    txtDPDueDate.Text = Convert.ToDateTime(DataAccess.GetData(dt, 0, "DPDueDate", "")).ToString("yyyy-MM-dd");

                    hPrjCode.Value = (string)DataAccess.GetData(dt, 0, "ProjCode", "");

                    if (hPrjCode.Value != "")
                    {
                        DataTable dt1 = ws.GetProjectDetails(hPrjCode.Value).Tables["ProjectDetails"];
                        tProjName.Value = (string)DataAccess.GetData(dt1, 0, "PrjName", "");

                        hBlockWidth.Value = (string)DataAccess.GetData(dt1, 0, "ImgWidth", "0");
                        hBlockHeight.Value = (string)DataAccess.GetData(dt1, 0, "ImgHeight", "0");

                    }

                    DataTable dtVal = ws.GetProjectDetails(hPrjCode.Value).Tables["ProjectDetails"];
                    tProjName.Value = (string)DataAccess.GetData(dtVal, 0, "PrjName", "");

                    tBlock.Value = (string)DataAccess.GetData(dt, 0, "Block", "");
                    tLot.Value = (string)DataAccess.GetData(dt, 0, "Lot", "");

                    tModel.Value = (string)DataAccess.GetData(dt, 0, "Model", "");
                    tFinancing.Value = (string)DataAccess.GetData(dt, 0, "FinancingScheme", "");
                    tLotArea.Value = (string)DataAccess.GetData(dt, 0, "LotArea", "");
                    tFloorArea.Value = (string)DataAccess.GetData(dt, 0, "FloorArea", "");

                    tHouseStatus.Value = (string)DataAccess.GetData(dt, 0, "HouseStatus", "");

                    tPhase.Value = SystemClass.NoDecimal((string)DataAccess.GetData(dt, 0, "Phase", ""));

                    txtLotClassification.Value = (string)DataAccess.GetData(dt, 0, "Size", "");
                    txtProductType.Value = (string)DataAccess.GetData(dt, 0, "AcctType", "");
                    txtLoanType.Value = (string)DataAccess.GetData(dt, 0, "LoanType", "");
                    tBank.Value = (string)DataAccess.GetData(dt, 0, "Bank", "");

                    //PAYMENT TERMS
                    tODas.Value = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "OTcp", "0").ToString());
                    tResrvFee.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "ResrvFee", "0").ToString());
                    tDPPercent.Text = SystemClass.ToDecimal(DataAccess.GetData(dt, 0, "DPPercent", "0").ToString());
                    tDPAmount.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "DPAmount", "0").ToString());

                    txtDPTerms.Text = SystemClass.NoDecimal(DataAccess.GetData(dt, 0, "DPTerms", "0").ToString());
                    lblDPTerms.Text = txtDPTerms.Text;

                    txtMiscTerms.Text = SystemClass.NoDecimal(DataAccess.GetData(dt, 0, "MiscDPTerms", "1").ToString());
                    lblMiscDPTerms.Text = txtMiscTerms.Text;

                    txtDiscPercent.Text = SystemClass.ToDecimal(DataAccess.GetData(dt, 0, "DiscPercent", "0").ToString());
                    tSpotDPDiscAmt.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "DiscAmount", "0").ToString());
                    txtLTerms.Text = SystemClass.NoDecimal(DataAccess.GetData(dt, 0, "LTerms", "0").ToString());
                    lblLBTerms.Text = txtLTerms.Text;
                    txtFactorRate1.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "InterestRate", "0").ToString());

                    lblLBDueDate.Text = Convert.ToDateTime(DataAccess.GetData(dt, 0, "LDueDate", "")).ToString("yyyy-MM-dd");
                    txtGDI.Value = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "Gdi", "").ToString());

                    //NEW FIELDS
                    tRetType.Value = DataAccess.GetData(dt, 0, "RetitlingType", "").ToString();
                    tRemarks.Value = DataAccess.GetData(dt, 0, "Remarks", "").ToString();
                    tPDBalance.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "DPBalanceOnEquity", "0").ToString());
                    txtDPMonthly.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "MonthlyDP", "0").ToString()); //check
                    txtMiscMonthly.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "PDMonthly", "0").ToString());
                    txtLoanableBalance.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "PDLoanableBalance", "0").ToString());
                    lblAddMiscFees.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "AddMiscFees", "0").ToString());
                    lblCompTotal.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "CSTotal", "0").ToString());
                    lblReserveFee.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "ResrvFee", "0").ToString());
                    lblReserveFee2.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "ResrvFee", "0").ToString());
                    lblBalance.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "TCPBalanceOnEquity", "0").ToString());
                    lblTCPMonthly.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "TCPMonthly", "0").ToString());
                    lblLoanableBalance.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "TCPLoanableBalance", "0").ToString());

                    lblMiscFees.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "AddMiscFees", "0").ToString());
                    lblMiscDPMonthly.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "MiscFeesMonthly", "0").ToString());
                    //txtMiscFees.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "ResrvFee", "0").ToString());
                    txtMiscFees.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "AddMiscFees", "0").ToString());

                    //--COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                    //txtAdjLot.Value = DataAccess.GetData(dt, 0, "SoldWithAdjacentLot", "").ToString();

                    txtLotNo.Value = SystemClass.NoDecimal(DataAccess.GetData(dt, 0, "AdjacentLotQuotationNo", "0").ToString());
                    txtIncentiveOption.Value = DataAccess.GetData(dt, 0, "Incentive", "No").ToString() == "Y" ? "Yes" : DataAccess.GetData(dt, 0, "Incentive", "No").ToString();
                    txtIncentiveOption2.Value = DataAccess.GetData(dt, 0, "Incentive", "No").ToString() == "Y" ? "Yes" : DataAccess.GetData(dt, 0, "Incentive", "No").ToString();

                    bLotNo.Disabled = txtAdjLot.Value == "No" || txtAdjLot.Value == "" ? true : false;

                    if (txtProductType.Value.ToUpper() == "HOUSE AND LOT")
                    {
                        tRetType.Value = "ABCI";
                        btnRetType.Disabled = true;
                    }
                    else
                    {
                        btnRetType.Disabled = false;
                    }

                    //Disable if RetType = ABCI
                    btnRetType.Disabled = tRetType.Value == "ABCI" ? true : false;

                    //ADDED FIELDS 2023-03-08
                    tMiscDocDate.Text = Convert.ToDateTime(DataAccess.GetData(dt, 0, "MiscDueDate", tDocDate.Text)).ToString("yyyy-MM-dd");
                    txtFinancingMisc.Value = DataAccess.GetData(dt, 0, "MiscFinancingScheme", "").ToString();
                    lblMiscFinancingScheme.Text = txtFinancingMisc.Value;


                    //COMPUTATION IN LEFT SIDE 
                    lblDASAmt.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "OTcp", "0").ToString());
                    lblDiscount.Text = SystemClass.ToCurrency((Convert.ToDouble(DataAccess.GetData(dt, 0, "DiscAmount", "0").ToString()) * -1).ToString());
                    lblNetDAS.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "Das", "0").ToString());
                    tNetDas.Value = lblNetDAS.Text;

                    lblNetDAS2.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "Das", "").ToString());
                    lblVAT.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "Vat", "").ToString());
                    lblNetTCP.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "NetTcp", "").ToString());

                    lblAddMiscCharges.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "OMisc", "").ToString());

                    lblTCPFinScheme.Text = tFinancing.Value;

                    lblDownPayment.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "TCPDownpayment", "").ToString());
                    lblAddMiscCharges2.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "OMisc", "").ToString());
                    lblDPMonthly.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "MonthlyDP", "").ToString());

                    lblDPDueDate.Text = Convert.ToDateTime(DataAccess.GetData(dt, 0, "DPDueDate", "")).ToString("yyyy-MM-dd");

                    lblAmountDue.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "LAmount", "").ToString());
                    lblLBMonthly.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "MonthlyLB", "").ToString());

                    tBlock2.Value = tBlock.Value;
                    tLot2.Value = tLot.Value;

                    txtLotNo.Value = (string)DataAccess.GetData(dt, 0, "AdjacentLotQuotationNo", "0");

                    //--COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                    //txtAdjLot.Value = (string)DataAccess.GetData(dt, 0, "SoldWithAdjacentLot", "");

                    lblMiscDPAmount.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "MiscDPAmount", "0").ToString());
                    lblMiscLBAmount.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "MiscLBAmount", "0").ToString());
                    lblMiscLBTerms.Text = DataAccess.GetData(dt, 0, "MiscLBTerms", "0").ToString();
                    lblMiscLBMonthly.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "MiscLBMonthly", "0").ToString());


                    hideTermDiv(txtLoanType.Value);
                    loadSchedules();



                    dt = hana.GetData($@"SELECT ""POSCode"", ""EmpCode"", ""EmpName"", ""POSCode"" FROM ""QUT5"" WHERE ""DocEntry"" = {DocEntry}", hana.GetConnection("SAOHana"));
                    lblEmployeeID.Text = DataAccess.GetData(dt, 0, "EmpCode", "").ToString();
                    lblEmployeeName.Text = DataAccess.GetData(dt, 0, "EmpName", "").ToString();
                    lblEmployeePosition.Text = DataAccess.GetData(dt, 0, "POSCode", "").ToString();


                    //2023-11-07 : ADDED BROKER NAME
                    getBrokerName(lblEmployeeID.Text);

                    if (txtLoanType.Value != "BANK")
                    {
                        divBank.Visible = false;
                        divBank2.Visible = false;
                        tBank.Value = " ";
                        tBank2.Value = " ";
                    }
                    else
                    {
                        divBank.Visible = true;
                        divBank2.Visible = true;
                    }





                    //Get data for Co-owner
                    DataTable dtCoOwner = hana.GetData($@"SELECT ""CardCode"" ""Code"", ""Name"" FROM ""QUT10"" WHERE ""DocEntry"" = {DocEntry}", hana.GetConnection("SAOHana"));
                    gvCoOwner.DataSource = dtCoOwner;
                    gvCoOwner.DataBind();
                    ViewState["CoOwner"] = dtCoOwner;


                    //if (gvPosList.Rows.Count > 0)
                    //{
                    //    foreach (GridViewRow row in gvPosList.Rows)
                    //    {
                    //        foreach (DataRow dr in dt.Rows)
                    //        {
                    //            if (row.Cells[0].Text == dr["POSCode"].ToString())
                    //            {
                    //                Label lblName = (Label)row.FindControl("lblSalesName");
                    //                Label lblCode = (Label)row.FindControl("lblSalesCode");

                    //                lblName.Text = dr["EmpName"].ToString();
                    //                lblCode.Text = dr["EmpCode"].ToString();
                    //                break;
                    //            }
                    //        }
                    //    }
                    //}

                    ScriptManager.RegisterStartupScript(this, GetType(), "MsgBuyers_Hide", "MsgBuyers_Hide();", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "MsgBuyers_Hide", "MsgBuyers_Hide();", true);
                alertMsg(ex.Message, "error");
            }
        }
        protected void bSelectProject_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            ClearAll("Project");
            hPrjCode.Value = GetID.CommandArgument;
            DataTable dt = new DataTable();
            dt = ws.GetProjectDetails(hPrjCode.Value).Tables["ProjectDetails"];


            //2023-06-01 : GET PROJECT NAME FROM SAP INSTEAD
            DataTable dtSAP = new DataTable();
            dtSAP = ws.GetProjectDetailsSAP(hPrjCode.Value).Tables["ProjectDetailsSAP"];
            tProjName.Value = (string)DataAccess.GetData(dtSAP, 0, "PrjName", "");



            hBlockWidth.Value = (string)DataAccess.GetData(dt, 0, "ImgWidth", "0");
            hBlockHeight.Value = (string)DataAccess.GetData(dt, 0, "ImgHeight", "0");
            tPhase.Value = (string)DataAccess.GetData(hana.GetData($@"SELECT ""U_Phase"" FROM ""OPRJ"" WHERE ""PrjCode"" = '{hPrjCode.Value}'", hana.GetConnection("SAPHana")), 0, "U_prPhase", "");

            var dtt = ws.GetHouseModelNew(hPrjCode.Value).Tables["GetHouseModelNew"];
            tModel.Value = (string)DataAccess.GetData(dtt, 0, "Code", "");



            //string qryLotWithHouseOption = $@"SELECT ""U_LotWithHouseOption"" FROM OPRJ WHERE ""PrjCode"" = '{hPrjCode.Value}' ";
            //DataTable dtLotWithHouseOption = hana.GetData(qryLotWithHouseOption, hana.GetConnection("SAPHana"));
            //if ((string)DataAccess.GetData(dtLotWithHouseOption, 0, "U_LotWithHouseOption", "Yes") == "Yes")
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "enableModelBtn", "enableModelBtn();", true);
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "disableModelBtn", "disableModelBtn();", true);
            //}

            ScriptManager.RegisterStartupScript(this, GetType(), "ProjHide", "MsgProjList_Hide();", true);
        }
        protected void tNextHouseDetails_Click(object sender, EventArgs e)
        {

            //ScriptManager.RegisterStartupScript(this, GetType(), "tNextTab", "tNextTab();", true);
            ViewState["DiscPercent"] = txtDiscPercent.Text;
            ViewState["DPPercent"] = tDPPercent.Text;
            try
            {
                //2023-06-01 : ADD BLOCKING FOR DP DUE DATE AND DOC DATE
                int counter = 0;
                if (Convert.ToDateTime(txtDPDueDate.Text) < Convert.ToDateTime(tDocDate.Text))
                {
                    //CHECKED IF DPDUEDATE IS HIGHER THAN DOCUMENT DATE
                    counter++;
                }
                if (Convert.ToDateTime(tMiscDocDate.Text) < Convert.ToDateTime(tDocDate.Text))
                {
                    //CHECKED IF DPDUEDATE IS HIGHER THAN DOCUMENT DATE
                    counter++;
                }


                if (counter == 0)
                {


                    //05-05-2023 : BLOCK IF INCENTIVE DOES NOT EXIST IN SAP
                    int tagIncentiveBlocking = 0;
                    string qryIncentiveBlocking = $@"SELECT A.""Code"", CAST(IFNULL(B.""U_EffDF"",'1999-01-01') AS TIMESTAMP),	CAST(IFNULL(B.""U_EffDT"",'2100-12-31') AS TIMESTAMP)
                                                 FROM ""@COMMINCSCHEME"" A INNER JOIN ""@INCENTIVE"" B ON A.""Code"" = B.""Code""  
                                                 WHERE  A.""U_Project"" = '{hPrjCode.Value}' AND UPPER(A.""U_Type"") = UPPER('{txtProductType.Value}') AND
                                                 '{Convert.ToDateTime(tDocDate.Text).ToString("yyyy-MM-dd")}' >= CAST(IFNULL(B.""U_EffDF"",'1999-01-01') AS TIMESTAMP)  AND 
                                                 '{Convert.ToDateTime(tDocDate.Text).ToString("yyyy-MM-dd")}' <= CAST(IFNULL(B.""U_EffDT"",'2100-12-31') AS TIMESTAMP)";
                    DataTable dtIncentiveBlocking = hana.GetData(qryIncentiveBlocking, hana.GetConnection("SAPHana"));

                    //2023-09-06 : ADJUST BLOCKING--EXCLUDE SAMPLE COMPUTATION
                    if (lblID.Text != "Sample Quotation")
                    {
                        if (txtIncentiveOption.Value == "Yes")
                        {
                            if (dtIncentiveBlocking.Rows.Count <= 0)
                            {
                                tagIncentiveBlocking++;
                            }
                        }
                    }

                    if (tagIncentiveBlocking == 0)
                    {
                        //2023-09-06 : ADJUST BLOCKING--EXCLUDE SAMPLE COMPUTATION
                        int tagCommissionBlocking = 0;

                        //05-05-2023 : BLOCK IF COMMISION SCHEME FOR THE PROJECT DOES NOT EXIST
                        string qryCommissionScheme = $@" SELECT A.""Code"" ""CommissionCode"", A.""U_Type"" ""Type"", A.""U_Project"" ""ProjCode"",
                                                IFNULL(B.""U_CollectedTCP"",0) ""CollectedTCP"", IFNULL(B.""U_Release"",0) ""Release"",	
                                                IFNULL(B.""U_CommissionRelease"",0) ""CommissionRelease"",
                                                IFNULL(A.""U_Commission"",0) ""CommissionPercent"" 
                                                FROM ""@COMMINCSCHEME"" A INNER JOIN	""@COMMISSION"" B ON A.""Code"" = B.""Code""  
                                                 WHERE  A.""U_Project"" = '{hPrjCode.Value}' AND UPPER(A.""U_Type"") = UPPER('{txtProductType.Value}')";
                        DataTable dtCommissionScheme = hana.GetData(qryCommissionScheme, hana.GetConnection("SAPHana"));

                        //2023-09-06 : ADJUST BLOCKING--EXCLUDE SAMPLE COMPUTATION
                        if (lblID.Text != "Sample Quotation")
                        {
                            if (dtCommissionScheme.Rows.Count <= 0)
                            {
                                tagCommissionBlocking++;
                            }
                        }


                        //2023-09-06 : ADJUST BLOCKING--EXCLUDE SAMPLE COMPUTATION
                        //if (dtCommissionScheme.Rows.Count > 0)
                        if (tagCommissionBlocking == 0)
                        {

                            //2023-09-06 : ADJUST BLOCKING--EXCLUDE SAMPLE COMPUTATION
                            int tagCommissionBlocking2 = 0;

                            //05-05-2023 : CHECK IF THERE'S A 0 RELEASE
                            string qryCommissionScheme0 = $@" SELECT A.*
                                                     FROM ""@COMMINCSCHEME"" A INNER JOIN	""@COMMISSION"" B ON A.""Code"" = B.""Code""  
                                                     WHERE  A.""U_Project"" = '{hPrjCode.Value}' AND UPPER(A.""U_Type"") = UPPER('{txtProductType.Value}') AND
                                                      IFNULL(B.""U_Release"",0) <= 0 ";
                            DataTable dtCommissionScheme0 = hana.GetData(qryCommissionScheme0, hana.GetConnection("SAPHana"));

                            //2023-09-06 : ADJUST BLOCKING--EXCLUDE SAMPLE COMPUTATION
                            if (lblID.Text != "Sample Quotation")
                            {
                                if (dtCommissionScheme0.Rows.Count > 0)
                                {
                                    tagCommissionBlocking2++;
                                }
                            }


                            //2023-09-06 : ADJUST BLOCKING--EXCLUDE SAMPLE COMPUTATION
                            //if (dtCommissionScheme0.Rows.Count == 0)
                            if (tagCommissionBlocking2 == 0)
                            {

                                //04-24-2023 : CHECK IF FINANCING SCHEME LB TERM IS 0
                                string qryGetFinancingSchemeLBTerm = $@"SELECT * FROM ""@FSC1"" WHERE ""Code"" = '{hPrjCode.Value}' AND ""U_Code"" = '{tFinancing.Value}'  ";
                                DataTable dtGetFinancingSchemeLBTerm = hana.GetData(qryGetFinancingSchemeLBTerm, hana.GetConnection("SAPHana"));
                                double BalTerms = double.Parse(DataAccess.GetData(dtGetFinancingSchemeLBTerm, 0, "U_BalTerms", "0").ToString());
                                int blockTag = 0;
                                int requiredTerm = 0;

                                //04-24-2023 BLOCKING IF LOAN TYPE IS BANK OR HDMF, BALANCE TERMS SHOULD ALWAYS BE 1 
                                if (txtLoanType.Value.Contains("BANK") || txtLoanType.Value.Contains("HDMF"))
                                {
                                    if (BalTerms != 1)
                                    {
                                        blockTag++;
                                        requiredTerm = 1;
                                    }
                                }

                                //04-24-2023 BLOCKING IF LOAN TYPE IS BANK OR HDMF  , BALANCE TERMS SHOULD ALWAYS BE 1 
                                if (txtLoanType.Value.Contains("SPOTCASH"))
                                {
                                    if (BalTerms != 0)
                                    {
                                        blockTag++;
                                        requiredTerm = 0;
                                    }
                                }

                                if (blockTag == 0)
                                {



                                    double result = 0;
                                    bool success = double.TryParse(tFloorArea.Value, out result);


                                    //04-20-2023 CHECK IF PRODUCT TYPE IS HOUSE AND LOT AND NO HOUSE MODEL
                                    double result2 = 0;
                                    if (txtProductType.Value.ToUpper() == "LOT ONLY")
                                    {
                                        string model = string.IsNullOrWhiteSpace(tModel.Value) ? "-" : tModel.Value;
                                        if (model != "-")
                                        {
                                            result2 = 1;
                                        }
                                    }


                                    //04-20-2023 BLOCK IF PRODTYPE = HOUSE AND LOT AND NO HOUSE MODEL IN BATCH DETAILS
                                    if (result2 == 0)
                                    {
                                        if (txtProductType.Value.ToUpper() == "HOUSE AND LOT" && result <= 0)
                                        {
                                            alertMsg("The selected house and lot has no floor area. Please contact administrator.", "info");
                                        }
                                        else
                                        {

                                            //--- Block Loanable amount > Max loanable amount ---//
                                            double loanable = Convert.ToDouble(lblLoanableBalance.Text);
                                            //if (loanable > 6000000 && (lblComaker.Text).ToUpper() == "N/A")
                                            //{
                                            //    alertMsg("Loanable amount is more than 6M. Co-maker is required.", "warning");
                                            //}
                                            //else
                                            //{
                                            if (lblEmployeeID.Text != "")
                                            {
                                                string ret = "";
                                                if (Session["UserID"] == null)
                                                {
                                                    alertMsg("Session expired!", "error");
                                                    Response.Redirect("~/pages/Login.aspx");
                                                }
                                                nxtHouseDetails();
                                            }
                                            else
                                            {
                                                if (lblID.Text == "Sample Quotation")
                                                {
                                                    nxtHouseDetails();

                                                }
                                                else
                                                {
                                                    alertMsg("Sales Agent is required to proceed.", "warning");
                                                }
                                            }
                                            //}


                                            if (lblID.Text != "Sample Quotation")
                                            {
                                                bPrint.Visible = false;
                                                bFinish.Visible = true;
                                            }
                                            else
                                            {
                                                bPrint.Visible = true;
                                                bFinish.Visible = false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        alertMsg("Lot only product type should not have house model. Please contact administrator.", "info");
                                    }

                                }
                                else
                                {
                                    alertMsg($@"TCP LB Term for {txtLoanType.Value} Loan should be {requiredTerm}. Kindly review the financing scheme selected.", "info");
                                }
                            }
                            else
                            {
                                alertMsg($@"0 release found in Commission Scheme for Project {hPrjCode.Value} and {txtProductType.Value} type. Please contact administrator.", "info");
                            }
                        }
                        else
                        {
                            alertMsg($@"No Commission Scheme found for Project {hPrjCode.Value} and {txtProductType.Value} type. Please contact Adminsitrator.", "info");
                        }
                    }
                    else
                    {
                        alertMsg($@"No incentive maintenance found for Document Date: {Convert.ToDateTime(tDocDate.Text).ToString("MMMM dd, yyyy")}. Please contact Administrator.", "info");
                    }
                }
                else
                {
                    tDocDate.Focus();
                    alertMsg($@"DP/MISC Due date should not be earlier than Document Date.", "info");
                }
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }
        }


        void nxtHouseDetails()
        {

            //#############################################
            //FOR RELOADING OF MISCELLANEOUS AMOUNTS AND TERMS AND SCHEDULE
            //#############################################
            lblMiscFinancingScheme.Text = txtFinancingMisc.Value;




            tDocDate2.Value = tDocDate.Text;
            tProjName2.Value = tProjName.Value;
            tModel2.Value = tModel.Value;
            tFinancing2.Value = tFinancing.Value;
            txtLotArea2.Value = tLotArea.Value;
            txtFloorArea2.Value = tFloorArea.Value;
            tHouseStatus2.Value = tHouseStatus.Value;
            txtPhase2.Value = tPhase.Value;
            txtLotClass2.Value = txtLotClassification.Value;
            txtProductType2.Value = txtProductType.Value;
            txtLoanType2.Value = txtLoanType.Value;
            tBank2.Value = tBank.Value;

            //CHECK WHEN UPDATING RESERVATION FEE
            ReservationUpdate();
            //CHECK WHEN UPDATING MISCELLANEOUS FEE
            MiscUpdate();

            //SCHEDULE
            loadSchedules();

            NextTab("PaymentTerms");

            if (lblID.Text == "Sample Quotation")
            { bFinish.Visible = false; }
            else
            { bFinish.Visible = true; }

        }

        void loadSchedules()
        {
            //###########################################
            //################ SCHEDULES ################ 
            //###########################################


            double interestRate = string.IsNullOrWhiteSpace(txtFactorRate1.Text) ? 0 : Convert.ToDouble(txtFactorRate1.Text);


            //DP 
            ViewState["gvDownPayment"] = ws.PaymentBreakdown(Convert.ToInt32(txtDPTerms.Text),
                                                           Convert.ToDateTime(lblDPDueDate.Text),
                                                           double.Parse(lblTCPMonthly.Text),
                                                           0,
                                                           double.Parse(lblBalance.Text),
                                                           "DP",
                                                           interestRate,
                                                           double.Parse(lblMiscFees.Text),
                                                           0,
                                                           0,
                                                           0,
                                                           Convert.ToDateTime(lblDPDueDate.Text)).Tables["PaymentBreakdown"];
            DataTable ekek = (DataTable)ViewState["gvDownPayment"];
            LoadData(gvDownPayment, "gvDownPayment");



















            if (txtProductType.Value.ToUpper() == "LOT ONLY" && tRetType.Value.ToUpper() == "BUYERS")
            {
                //FOR SINGLE MISC

                ViewState["gvMiscellaneous"] = ws.PaymentBreakdown(1,
                                                               Convert.ToDateTime(lblMiscDueDate.Text),
                                                               double.Parse(lblMiscDPMonthly.Text),
                                                               0,
                                                               double.Parse(lblMiscFees.Text),
                                                               "MISC",
                                                               interestRate,
                                                               double.Parse(lblMiscFees.Text),
                                                               0,
                                                               0).Tables["PaymentBreakdown"];
                DataTable ekek2 = (DataTable)ViewState["gvMiscellaneous"];
                LoadData(gvMiscellaneous, "gvMiscellaneous");

                ViewState["gvMiscellaneousAmort"] = null;
                divMonthlyAmortMisc.Visible = false;
            }
            else
            {




















                //FOR MULTIPLE MISC

                if (Convert.ToInt32(lblMiscDPTerms.Text) > 0)
                {
                    divMonthlyDPMisc.Visible = true;

                    // MISC DP SCHEDULE
                    ViewState["gvMiscellaneous"] = ws.PaymentBreakdown(Convert.ToInt32(lblMiscDPTerms.Text),
                                                                   Convert.ToDateTime(lblMiscDueDate.Text),
                                                                   double.Parse(lblMiscDPMonthly.Text),
                                                                   0,
                                                                   double.Parse(lblMiscDPAmount.Text),
                                                                   "MISC",
                                                                   interestRate,
                                                                   double.Parse(lblMiscDPAmount.Text),
                                                                   0,
                                                                   0,
                                                                   0,
                                                                   Convert.ToDateTime(lblMiscDueDate.Text),
                                                                   double.Parse(lblMiscDPMonthly.Text)
).Tables["PaymentBreakdown"];
                    DataTable ekek2 = (DataTable)ViewState["gvMiscellaneous"];
                    LoadData(gvMiscellaneous, "gvMiscellaneous");

                }
                else
                {
                    ViewState["gvMiscellaneous"] = null;
                    gvMiscellaneous.Visible = false;
                    //lblMiscDPAmount.Text = "0.00";
                    //lblMiscDPMonthly.Text = "0.00";
                }






                //2023-08-18 : CHANGED BLOCKING FROM TERM TO AMOUNT
                //if (Convert.ToInt32(lblMiscLBTerms.Text) > 0)
                if (Convert.ToDouble(lblMiscLBAmount.Text) > 0)
                {
                    divMonthlyAmortMisc.Visible = true;

                    // MISC LB SCHEDULE
                    string MiscLBDate = Convert.ToDateTime(lblMiscDueDate.Text).AddMonths(int.Parse(lblMiscDPTerms.Text)).ToString("MM-dd-yyyy");

                    if (Convert.ToInt32(lblMiscLBTerms.Text) > 1)
                    {
                        ViewState["gvMiscellaneousAmort"] = ws.PaymentBreakdown(Convert.ToInt32(lblMiscLBTerms.Text),
                                                       Convert.ToDateTime(MiscLBDate),
                                                       double.Parse(lblMiscLBMonthly.Text),
                                                       0,
                                                       double.Parse(lblMiscLBAmount.Text),
                                                       "MISC",
                                                       interestRate,
                                                       double.Parse(lblMiscLBAmount.Text),
                                                       0,
                                                       0,
                                                       Convert.ToInt32(lblMiscDPTerms.Text)).Tables["PaymentBreakdown"];
                        DataTable ekek3 = (DataTable)ViewState["gvMiscellaneousAmort"];
                        LoadData(gvMiscellaneousAmort, "gvMiscellaneousAmort");
                    }
                    else
                    {
                        ViewState["gvMiscellaneousAmort"] = ws.PaymentBreakdownBankPagibig(Convert.ToDateTime(MiscLBDate),
                                                                                         Convert.ToDouble(lblMiscLBAmount.Text),
                                                                                         0,
                                                                                         Convert.ToInt32(lblMiscDPTerms.Text)).Tables[0];
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












            //MA SCHEDULE   

            //2023-08-18 : CHANGED BLOCKING FROM TERM TO AMOUNT
            //if (int.Parse(lblLBTerms.Text) > 0)
            if (double.Parse(lblLoanableBalance.Text) > 0)
            {

                string qry = $@"SELECT * FROM ""@LOANTYPE"" WHERE ""Code"" = '{txtLoanType.Value}'";
                DataTable dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
                string oneLinertag = DataAccess.GetData(dt, 0, "U_OneLiner", "N").ToString();



                if (oneLinertag == "Y")
                {
                    //ViewState["gvAmortization"] = ws.PaymentBreakdownBankPagibig(Convert.ToDateTime(lblDueDate2.Text).AddMonths(1), Convert.ToDouble(lblAmountDue.Text)).Tables[0];
                    ViewState["gvAmortization"] = ws.PaymentBreakdownBankPagibig(Convert.ToDateTime(lblLBDueDate.Text).AddMonths(1),
                                                                                Convert.ToDouble(lblLoanableBalance.Text),
                                                                                0).Tables[0];
                }
                else
                {
                    //ViewState["gvAmortization"] = ws.PaymentBreakdown(int.Parse(txtLTerms.Text), Convert.ToDateTime(lblDueDate2.Text).AddMonths(1),
                    //            double.Parse(lblMonthly2.Text), double.Parse(txtFactorRate1.Text), double.Parse(lblAmountDue.Text), "LB", interestRate, double.Parse(lblAddMiscCharges2.Text)).Tables[0];
                    ViewState["gvAmortization"] = ws.PaymentBreakdown(int.Parse(txtLTerms.Text),
                                                                     Convert.ToDateTime(lblLBDueDate.Text).AddMonths(1), //2
                                                                     double.Parse(lblLBMonthly.Text),
                                                                     double.Parse(txtFactorRate1.Text), //4
                                                                     double.Parse(lblLoanableBalance.Text),
                                                                     "LB", //6
                                                                     interestRate,
                                                                     double.Parse(lblMiscFees.Text),
                                                                     0,
                                                                     0,
                                                                     0,
                                                                     //2023-09-15 : CHANFED TO LBLDPDUEDATE TO GET ORIGINAL DATE
                                                                     Convert.ToDateTime(lblDPDueDate.Text)).Tables[0]; //8
                }
                LoadData(gvAmortization, "gvAmortization");
                divMonthlyAmort.Visible = true;
            }
            else
            {
                divMonthlyAmort.Visible = false;
                ViewState["gvAmortization"] = null;
                DataTable ekek2 = (DataTable)ViewState["gvAmortization"];
            }

        }




        protected void bLot_ServerClick(object sender, EventArgs e)
        {
            //ClearAll("Lot");c
            //DataTable dt = new DataTable();
            //dt = hana.GetData($@"SELECT ""ImgWidth"",""ImgHeight"" FROM ""PRJ1"" WHERE ""PrjCode"" = '{hPrjCode.Value}' AND ""Block"" = '{tBlock.Value}'", hana.GetConnection("SAOHana"));
            //hLotWidth.Value = (string)DataAccess.GetData(dt, 0, "ImgWidth", "0");
            //hLotHeight.Value = (string)DataAccess.GetData(dt, 0, "ImgHeight", "0");
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "drawLot", "drawLotMap();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ProjHide", "MsgBlockLotList_Hide();", true);
            ViewState["dtDocumentStatus"] = ws.GetDocumentStatus().Tables["DocumentStatus"];
            LoadData(gvBlockColor, "dtDocumentStatus");
            //ScriptManager.RegisterStartupScript(this, GetType(), "BlockShow", "MsgLotList_Show();", true);



            string qryHouseModel = $@"SELECT ""U_HouseModel"",""U_HouseModelName"" FROM OBTN WHERE ""U_BlockNo"" = '{tBlock.Value}' AND 
                        ""U_LotNo"" = '{tLot.Value}' AND ""U_Project"" = '{hPrjCode.Value}'";
            DataTable dt = hana.GetData(qryHouseModel, hana.GetConnection("SAPHana"));
            tModel.Value = (string)DataAccess.GetData(dt, 0, "U_HouseModel", "");


            //string qryLotWithHouseOption = $@"SELECT ""U_LotWithHouseOption"" FROM OPRJ WHERE ""PrjCode"" = '{hPrjCode.Value}' ";
            //DataTable dtLotWithHouseOption = hana.GetData(qryLotWithHouseOption, hana.GetConnection("SAPHana"));
            //if ((string)DataAccess.GetData(dtLotWithHouseOption, 0, "U_LotWithHouseOption", "") == "Yes")
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "enableModelBtn", "enableModelBtn();", true);
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "disableModelBtn", "disableModelBtn();", true);
            //}


        }
        protected void bGenerate_ServerClick(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();

                string qryStatusLot = $@"SELECT ""U_LotStatus"" FROM OBTN WHERE ""U_BlockNo"" = '{tBlock.Value}' AND 
                        ""U_LotNo"" = '{tLot.Value}' AND ""U_Project"" = '{hPrjCode.Value}'";
                dt = hana.GetData(qryStatusLot, hana.GetConnection("SAPHana"));
                string LotStatus = (string)DataAccess.GetData(dt, 0, "U_LotStatus", "");


                if (LotStatus == "S01")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Gen", "MsgLotList_Hide();", true);
                    dt = ws.GetHouseDetails(hPrjCode.Value, tBlock.Value, tLot.Value, tFinancing.Value).Tables["GetHouseDetails"];
                    if (DataAccess.Exist(dt) == true)
                    {
                        GetHouseDetails(dt);
                    }
                    else
                    {
                        ViewState["IsGetHouse"] = true;
                        ViewState["IsGetSize"] = true;
                        ViewState["IsGetFeat"] = true;
                    }

                    if (lblID.Text != "Sample Quotation")
                    {
                        bPrint.Visible = false;
                        bFinish.Visible = true;
                        ws.SetColorByProj(hPrjCode.Value, tBlock.Value, tLot.Value, "GETDATE()");
                    }
                    else
                    {
                        bPrint.Visible = true;
                        bFinish.Visible = false;
                    }
                }
                else
                {
                    tLot.Value = "";
                    alertMsg("Lot is not available.", "info");
                    ScriptManager.RegisterStartupScript(this, GetType(), "HideLot", "MsgLotList_Hide();", true);
                }

            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }
        }


        void GetHouseDetails(DataTable dt)
        {


            //GENERATE AUTOMATICALLY 
            tLotArea.Value = (string)DataAccess.GetData(dt, 0, "U_LotArea", "");
            tFloorArea.Value = (string)DataAccess.GetData(dt, 0, "U_FloorArea", "");
            tHouseStatus.Value = (string)DataAccess.GetData(dt, 0, "U_ProductStatus", "");
            tPhase.Value = (string)DataAccess.GetData(dt, 0, "U_Phase", "");
            txtLotClassification.Value = (string)DataAccess.GetData(dt, 0, "U_LotClass", "");
            txtProductType.Value = (string)DataAccess.GetData(dt, 0, "U_ProductType", "");

            var Test12312313 = ViewState["UpdateResFee"];

            //SET RETITLING TYPE TO ABCI IF PRODUCT TYPE IS HOUSE AND LOT
            if (txtProductType.Value.ToUpper() == "HOUSE AND LOT")
            {
                tRetType.Value = "ABCI";
                btnRetType.Disabled = true;

                if (Convert.ToInt32(ViewState["UpdateResFee"]) == 0)
                {
                    tResrvFee.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "U_ResFee", "0").ToString());
                }
            }
            //else if (txtProductType.Value != "House and Lot" && tRetType.Value == "ABCI")
            //{   
            //    btnRetType.Disabled = false;
            //    tResrvFee.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "U_ResFee", "0").ToString());
            //}
            else
            {
                btnRetType.Disabled = false;
                if (Convert.ToInt32(ViewState["UpdateResFee"]) == 0)
                {
                    tResrvFee.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "U_ResFeeLot", "0").ToString());
                }
            }

            double LandPrice = Convert.ToDouble(DataAccess.GetData(dt, 0, "LandPrice", "0").ToString());
            double HousePrice = Convert.ToDouble(DataAccess.GetData(dt, 0, "HousePrice", "0").ToString());
            double MinLot = Convert.ToDouble(DataAccess.GetData(dt, 0, "MinLot", "0").ToString());

            //COMPUTE TCP
            //if (MinLot == Convert.ToDouble(tLotArea.Value))
            //{
            //    tODas.Value = SystemClass.ToCurrency(HousePrice.ToString());
            //}
            //else
            //{
            tODas.Value = SystemClass.ToCurrency((LandPrice + HousePrice).ToString());
            //}

            txtDPTerms.Text = (string)DataAccess.GetData(dt, 0, "U_DPTerms", "");

            tDPPercent.Text = SystemClass.NoDecimal(DataAccess.GetData(dt, 0, "U_Equity", "").ToString());
            ViewState["DPPercent"] = tDPPercent.Text;

            txtDiscPercent.Text = SystemClass.NoDecimal(DataAccess.GetData(dt, 0, "U_Discount", "").ToString());
            ViewState["DiscPercent"] = tDPPercent.Text;

            tSpotDPDiscAmt.Text = SystemClass.ToCurrency(SystemClass.GetDiscountAmount(tODas.Value, txtDiscPercent.Text).ToString()).ToString();

            //Get VAT
            double vat = 0;
            double grossTCP = (LandPrice + HousePrice) - Convert.ToDouble(tSpotDPDiscAmt.Text);
            string qry = $@"SELECT IFNULL(""U_ThresholdAmount"",0) Threshold FROM ""@PRODUCTTYPE"" WHERE UPPER(""Code"") = UPPER('{txtProductType.Value}')";
            DataTable dtThes = hana.GetData(qry, hana.GetConnection("SAPHana"));
            if (dtThes.Rows.Count > 0)
            {
                double threshold = Convert.ToDouble(DataAccess.GetData(dtThes, 0, "Threshold", ""));
                if (threshold <= grossTCP)
                {
                    vat = grossTCP * 0.12;
                }
            }

            lblReserveFee.Text = tResrvFee.Text;
            lblReserveFee2.Text = tResrvFee.Text;

            tNetDas.Value = SystemClass.ToCurrency((Convert.ToDouble(tODas.Value) - Convert.ToDouble(tSpotDPDiscAmt.Text)).ToString());
            tDPAmount.Text = SystemClass.ToCurrency(SystemClass.GetDiscountAmount(tNetDas.Value, tDPPercent.Text).ToString());

            //double x = SystemClass.GetDiscountAmount(tNetDas.Value, tDPPercent.Text);
            //tDPAmount.Text = SystemClass.ToCurrency((x % 1000 >= 500 ? x + 1000 - x % 1000 : x - x % 1000).ToString());


            lblDownPayment.Text = tDPAmount.Text;

            if (tFinancing.Value == "Spot Cash")
            {
                txtLoanType.Value = "SPOTCASH";
                hideTermDiv(txtLoanType.Value);

                divBank.Visible = false;
                divBank2.Visible = false;

            }

            txtLTerms.Text = (string)DataAccess.GetData(dt, 0, "U_BalTerms", "0");
            lblLBTerms.Text = txtLTerms.Text;

            txtFactorRate1.Text = (string)DataAccess.GetData(dt, 0, "U_Interest", "0");

            //GetMiscDetails(dt);

        }

        void GetMiscDetails(DataTable dt)
        {
            //GET ALL MISCELLANEOUS DETAILS FOR MISCELLANEOUS FEE BREAKDOWN (DOWNPAYMENT AND LOANABLE BALACE)
            txtMiscTerms.Text = (string)DataAccess.GetData(dt, 0, "U_DPTerms", "0");
            lblMiscDPTerms.Text = (string)DataAccess.GetData(dt, 0, "U_DPTerms", "1");
            string MiscDPPercentage = DataAccess.GetData(dt, 0, "U_Equity", "0").ToString();

            //if (Convert.ToInt32(ViewState["UpdateMiscFee"]) == 0)
            //{
            lblMiscDPAmount.Text = SystemClass.ToCurrency(SystemClass.GetDiscountAmount(lblMiscFees.Text, MiscDPPercentage).ToString()).ToString();
            //}

            lblMiscLBTerms.Text = (string)DataAccess.GetData(dt, 0, "U_BalTerms", "1");
            //txtFactorRate1.Text = (string)DataAccess.GetData(dt, 0, "U_Interest", "0");
        }



        protected void bLotList_ServerClick(object sender, EventArgs e)
        {
            if (tProjName.Value != "")
            {
                ClearAll("Block");
                ScriptManager.RegisterStartupScript(this, GetType(), "ProjHide", "MsgProjList_Hide();", true);
                //ScriptManager.RegisterStartupScript(this, GetType(), "Block", "drawBlockMap();", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "BlockShow", "MsgBlockLotList_Show();", true);
            }
            else { alertMsg("Please select project first!", "warning"); }



        }
        protected void gvBlockColor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBlockColor.PageIndex = e.NewPageIndex;
            LoadData(gvBlockColor, "dtDocumentStatus");
            bLot_ServerClick(sender, e);
        }
        protected void bModel_ServerClick(object sender, EventArgs e)
        {
            if (hPrjCode.Value != "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "MsgHouseModel", "MsgHouseModel_Show();", true);
                gvHouseModelList.DataSource = ws.GetHouseModelNew(hPrjCode.Value).Tables["GetHouseModelNew"];
                gvHouseModelList.DataBind();

                if (!string.IsNullOrWhiteSpace(tFinancing.Value))
                {
                    Compute();
                }
            }
            else { alertMsg("Please select project first!", "warning"); }
        }
        protected void bPicPreview_ServerClick(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string Code = GetID.CommandArgument;
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgHouseList", "MsgHouseList_Hide();", true);
            gvPicPreview.DataSource = ws.GetHousePicture(Code).Tables["GetHousePicture"];
            gvPicPreview.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgPicPreview", "MsgPicPreview_Show();", true);
        }
        protected void bChooseHouse_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string Code = GetID.CommandArgument;
            DataTable dt = new DataTable();
            dt = ws.GetHousePicture(Code).Tables["GetHousePicture"];
            ViewState["HouseStatusCode"] = (string)DataAccess.GetData(dt, 0, "Code", "");
            tModel.Value = (string)DataAccess.GetData(dt, 0, "Name", "");


            if (Convert.ToInt32(ViewState["UpdateResFee"]) == 0)
            {
                tResrvFee.Text = SystemClass.ToNumeric((string)DataAccess.GetData(dt, 0, "U_ResFee", "0"));
            }


            ScriptManager.RegisterStartupScript(this, GetType(), "MsgHouseList", "MsgHouseList_Hide();", true);
        }
        protected void bFinish_Click(object sender, EventArgs e)
        {
            bFinish.Enabled = false;
            try
            {

                if (lblID.Text != "Sample Quotation")
                {


                    //BLOCK IF NO STANDARD COST FACTOR RATE FOUND IN PROJECTS (SAP)
                    DataTable dtStandardCost = hana.GetData($@"SELECT ""U_Standardcost"" FROM ""OPRJ"" WHERE ""PrjCode"" = '{hPrjCode.Value}'", hana.GetConnection("SAPHana"));
                    double StandardCostFactorRate = Convert.ToDouble(DataAccess.GetData(dtStandardCost, 0, "U_Standardcost", "0").ToString());

                    if (StandardCostFactorRate > 0)
                    {

                        //CHECK IF THE QUOTATION NEEDS TO BE WAIVED (BASED ON THRESHOLD)
                        string qry = $@"SELECT * FROM ""@EMAIL"" WHERE ""Code"" = 'QTNWVE'";
                        DataTable dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
                        if (Convert.ToDouble(lblLoanableBalance.Text) >= Convert.ToDouble(DataAccess.GetData(dt, 0, "U_ApprovalAmt", "").ToString()) && ((lblComaker.Text).ToUpper() == "N/A" || String.IsNullOrEmpty(lblComaker.Text)))
                        {
                            lblConfirmationInfo.Text = $@"Loanable amount is more than {DataAccess.GetData(dt, 0, "U_ApprovalAmt", "").ToString()}. Co-maker is required.";
                            ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "showMsgConfirm();", true);
                        }
                        else
                        {
                            // IF RESERVATION HAS ALREADY BEEN PAID (ALREADY SOLD TO OTHER CUSTOMER/CONTRACT)
                            string qryExistingBlockAndLot = $@"SELECT A.""DocEntry"" FROM OQUT A LEFT JOIN QUT1 B ON A.""DocEntry"" = B.""DocEntry"" WHERE A.""ProjCode"" = '{hPrjCode.Value}' 
                                    AND A.""Block"" = '{tBlock.Value}' AND	A.""Lot"" = '{tLot.Value}' AND	B.""PaymentType"" = 'RES' AND	IFNULL(B.""AmountPaid"",0) >= B.""PaymentAmount""
                                    AND B.""LineStatus"" <> 'R' AND A.""DocStatus"" NOT IN ('R','F')";

                            DataTable dtExistingBlockAndLot = hana.GetDataDS(qryExistingBlockAndLot, hana.GetConnection("SAOHana")).Tables[0];

                            if (dtExistingBlockAndLot.Rows.Count == 0)
                            {
                                //BLOCK IF QUOTATION WITH THE SAME BLOCK AND LOT ALREADY EXISTS UNDER THIS CUSTOMER
                                string qryExistingBPBlockANdLot = $@"SELECT A.""DocEntry"" FROM OQUT A WHERE A.""ProjCode"" =    '{hPrjCode.Value}' AND 
                                                A.""Block"" = '{tBlock.Value}' AND	A.""Lot"" = '{tLot.Value}' AND A.""CardCode"" = '{lblID.Text}' AND A.""DocStatus"" NOT IN ('F','R')";

                                DataTable dtExistingBPBlockANdLot = hana.GetDataDS(qryExistingBPBlockANdLot, hana.GetConnection("SAOHana")).Tables[0];

                                if (!string.IsNullOrEmpty(ViewState["DocNum"].ToString()))
                                {
                                    SaveQuotation();
                                }
                                else
                                {
                                    if (dtExistingBPBlockANdLot.Rows.Count == 0)
                                    {
                                        SaveQuotation();
                                    }
                                    else
                                    {
                                        alertMsg("Quotation with the same block & lot already exists under this customer. Please contact administrator.", "info");
                                    }

                                }
                            }
                            else
                            {
                                alertMsg("Selected house & lot is already sold. Please contact administrator.", "info");
                            }

                        }
                    }
                    else
                    {
                        alertMsg($@"No Standard Cost Factor Rate found for the project {tProjName2.Value}. Please contact adminsitrator.", "info");
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                bFinish.Enabled = true;
                alertMsg(ex.Message, "error");
            }
            bFinish.Enabled = true;

        }
        protected void Text_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;

            SystemClass.ToDecimal(txt.Text);

            double t1 = 0, t2 = 0;
            if (txt.ID == "tPBasicSalary" || txt.ID == "tSBasicSalary")
            {
                t1 = SystemClass.TextIsZero(tPBasicSalary.Text);
                t2 = SystemClass.TextIsZero(tSBasicSalary.Text);

                tTBasicSalary.Value = SystemClass.ToCurrency((t1 + t2).ToString());
            }
            else if (txt.ID == "tPAllowances" || txt.ID == "tSAllowances")
            {
                t1 = SystemClass.TextIsZero(tPAllowances.Text);
                t2 = SystemClass.TextIsZero(tSAllowances.Text);

                tTAllowances.Value = SystemClass.ToCurrency((t1 + t2).ToString());
            }
            else if (txt.ID == "tPCommissions" || txt.ID == "tSCommissions")
            {
                t1 = SystemClass.TextIsZero(tPCommissions.Text);
                t2 = SystemClass.TextIsZero(tSCommissions.Text);

                tTCommissions.Value = SystemClass.ToCurrency((t1 + t2).ToString());
            }
            else if (txt.ID == "tPRentalIncome" || txt.ID == "tSRentalIncome")
            {
                t1 = SystemClass.TextIsZero(tPRentalIncome.Text);
                t2 = SystemClass.TextIsZero(tSRentalIncome.Text);

                tTRentalIncome.Value = SystemClass.ToCurrency((t1 + t2).ToString());
            }
            else if (txt.ID == "tPRetainer" || txt.ID == "tSRetainer")
            {
                t1 = SystemClass.TextIsZero(tPRetainer.Text);
                t2 = SystemClass.TextIsZero(tSRetainer.Text);

                tTRetainer.Value = SystemClass.ToCurrency((t1 + t2).ToString());
            }
            else if (txt.ID == "tPOthers" || txt.ID == "tSOthers")
            {
                t1 = SystemClass.TextIsZero(tPOthers.Text);
                t2 = SystemClass.TextIsZero(tSOthers.Text);

                tTOthers.Value = SystemClass.ToCurrency((t1 + t2).ToString());
            }

            AutoCompute();
        }
        protected void tNextHouseSummary_Click(object sender, EventArgs e)
        {

            //THIS IS NOT USED ANYMORE

            string oHouseStatusCode = DataAccess.GetViewState((string)ViewState["HouseStatusCode"], "");
            string otSize = DataAccess.GetViewState((string)ViewState["tSize"], "");
            string otFeature = DataAccess.GetViewState((string)ViewState["tFeature"], "");
            string oHouseStatus = DataAccess.GetViewState((string)ViewState["HouseStatus"], "");
            string otFinancing = DataAccess.GetViewState((string)ViewState["tFinancing"], "");
            string otAccountType = DataAccess.GetViewState((string)ViewState["tAccountType"], "");
            string otSalesType = tSalesType.Value;
            double oLotArea = 0;
            double oFloorArea = 0;

            if (ddlAllowed.Text == "Approved")
            {
                tTerms.Text = string.IsNullOrWhiteSpace(tTerms.Text) ? "1" : tTerms.Text;
            }

            if (tLotArea.Value != "")
            { oLotArea = double.Parse(tLotArea.Value); }

            if (tFloorArea.Value != "")
            { oFloorArea = double.Parse(tFloorArea.Value); }

            DateTime oDPDueDate = DateTime.Now;
            DateTime ACDueDate = DateTime.Now;
            DateTime oLDueDate = DateTime.Now;
            DateTime selDueDate = DateTime.Now;


            if (lblLBDueDate.Text != "-")
            {
                ACDueDate = Convert.ToDateTime(lblLBDueDate.Text);
            }
            else { ACDueDate = Convert.ToDateTime("2000-01-01"); }

            //oLDueDate = oDPDueDate.AddMonths(int.Parse(ddlDPTerms.SelectedValue) + int.Parse(ConfigSettings.LTermsBuffer"].ToString()));
            oLDueDate = Convert.ToDateTime(lblLBDueDate.Text);

            if (txtDPDueDate.Text != "")
            { selDueDate = Convert.ToDateTime(txtDPDueDate.Text); }


            string ret = ws.Quotation(3, 0,  //2
                                      DataAccess.GetViewState(lblID.Text, ""), Convert.ToDateTime(tDocDate.Text),  //4
                                      "O", hPrjCode.Value,  //6
                                      tPhase.Value, tBlock.Value, //8 
                                      tLot.Value, oHouseStatusCode,  //10
                                      otSize, otFeature,  //12
                                      oHouseStatus, oLotArea, //14
                                      oFloorArea, otFinancing,  //16
                                      otAccountType, otSalesType, //18
                                      (string)ViewState["ItemCode"], (string)ViewState["ItemCodeOC"], //20
                                      Convert.ToDouble(lblDASAmt.Text), Convert.ToDouble(lblAddMiscCharges.Text), //22  
                                      Convert.ToDouble(lblVAT.Text), Convert.ToDouble(lblNetTCP.Text),  //24
                                      Convert.ToDouble(lblNetTCP.Text), Convert.ToDouble(lblPromoDisc.Text) * -1, //26
                                      double.Parse(txtDiscPercent.Text), double.Parse(lblDiscount.Text), //28    DISCOUNT PERCENT AND DISCOUNT AMOUNT
                                      double.Parse(tSpotDPDiscAmt.Text), //30
                                      Convert.ToDouble(lblDiscount.Text), Convert.ToDouble(lblDASAmt.Text),  //34
                                      Convert.ToDouble(lblNetDAS.Text), Convert.ToDouble(lblAddMiscCharges.Text),  //36
                                      Convert.ToDouble(lblVAT.Text), Convert.ToDouble(lblNetTCP.Text), //38
                                      double.Parse(tDPPercent.Text), double.Parse(tDPAmount.Text), //40 
                                      double.Parse(tResrvFee.Text),  //42
                                      Convert.ToDouble(lblDownPayment.Text), int.Parse(txtDPTerms.Text), //44 
                                      oDPDueDate, Convert.ToDouble(lblDPMonthly.Text),  //46
                                                                                        //int.Parse(tLMaturityAge.Text), double.Parse(tLPercent2.Value), //48 
                                      0, 0, //48 
                                      Convert.ToDouble(lblAmountDue.Text), int.Parse(txtLTerms.Text), //50
                                                                                                      //oLDueDate, double.Parse(tInterestRate.Text),  //52
                                      oLDueDate, 0,  //52
                                      double.Parse(tMonthlyAmort2.Text), 0,  //54
                                      0, 0,  //56
                                      0, 0,  //58
                                      0, 0,  //60
                                      0, 0,  //62
                                      0, 0,  //64
                                      0, 0,  //66
                                      0, 0,  //68
                                      0, 0,  //70
                                      0, 0,  //72
                                      0, 0,  //74
                                      0, (int)Session["UserID"], //76
                                      "", "",  //78
                                      0, "",  //80
                                      7, null,  //82
                                      null, null,  //84
                                                   //gvPosList, double.Parse(lblNetDAS2.Text),  //86
                                      null, double.Parse(lblNetDAS2.Text),  //86
                                      double.Parse(lblAddCharges.Text), ACDueDate, //88
                                      double.Parse(lblLBMonthly.Text), selDueDate); //90

            if (ret == "Operation completed successfully.")
            {
                tMonthlyAmort2.Text = lblLBMonthly.Text;
                AutoCompute();

                if (lblFinish.InnerText == "Update")
                {
                    tTBasicSalary.Value = SystemClass.ToCurrency((double.Parse(tPBasicSalary.Text) + double.Parse(tSBasicSalary.Text)).ToString());
                    tTAllowances.Value = SystemClass.ToCurrency((double.Parse(tPAllowances.Text) + double.Parse(tSAllowances.Text)).ToString());
                    tTCommissions.Value = SystemClass.ToCurrency((double.Parse(tPCommissions.Text) + double.Parse(tSCommissions.Text)).ToString());
                    tTRentalIncome.Value = SystemClass.ToCurrency((double.Parse(tPRentalIncome.Text) + double.Parse(tSRentalIncome.Text)).ToString());
                    tTRetainer.Value = SystemClass.ToCurrency((double.Parse(tPRetainer.Text) + double.Parse(tSRetainer.Text)).ToString());
                    tTOthers.Value = SystemClass.ToCurrency((double.Parse(tPOthers.Text) + double.Parse(tSOthers.Text)).ToString());
                }

                if (otFinancing == "SC")
                { NextTab("QuickEval"); NextTab("QuotationSummary"); }
                else
                { NextTab("QuickEval"); }

            }
            else
            { alertMsg(ret, "error"); }
        }
        protected void bSize_ServerClick(object sender, EventArgs e)
        {
            if (tModel.Value != "")
            {
                Control btn = (Control)sender;
                string id = btn.ID;
                string txt = "Choose ";
                string oQuery = "";

                if (id == "bSize")
                { txt += "Size of the House"; oQuery = $@"CALL sp_GetHouseSize ('{hana.GetConnection("SAOHana")}','{(string)ViewState["HouseStatusCode"] }','{hPrjCode.Value}');"; }
                else if (id == "bFeature") // && (bool)ViewState["IsGetFeat"]  == true
                { txt += "Feature"; oQuery = $@"CALL sp_GetFeature '({ConfigurationManager.AppSettings["HANADatabase"]}','{(string)ViewState["HouseStatusCode"] }','{hPrjCode.Value}');"; }
                ChooseText.InnerText = txt;

                if (oQuery != "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "MsgQuotation", "MsgQuotation_Show();", true);
                    gvList.DataSource = hana.GetData(oQuery, hana.GetConnection("SAOHana"));
                    gvList.DataBind();
                    ViewState["btnID"] = id;
                }
            }
            else { alertMsg("Please select house model first!", "warning"); }
        }
        protected void tDiscount_TextChanged(object sender, EventArgs e)
        {
            TextBox tbox = (TextBox)sender;
            if (string.IsNullOrEmpty(tbox.Text))
            { tbox.Text = "0"; }

            double oMax = double.Parse((string)ViewState["MaxDiscount"]);
            double oMin = double.Parse((string)ViewState["MinDiscount"]);
            double oVal = double.Parse(tbox.Text);

            if (oVal == 0)
            { tbox.Text = "0.00"; }
            else if (oVal > oMax)
            { tbox.Text = oMax.ToString(); }
            else if (oVal < oMin)
            { tbox.Text = oMin.ToString(); }

            tbox.Focus();
        }
        protected void tNumeric_TextChange(object sender, EventArgs e)
        {
            TextBox tbox = (TextBox)sender;
            if (string.IsNullOrEmpty(tbox.Text))
            { tbox.Text = "0"; }
            tbox.Text = SystemClass.ToNumeric(tbox.Text);
        }
        protected void Terms_SelectedIndexChanged(object sender, EventArgs e)
        {
            tTerms.Text = "0";
            Compute();
        }
        protected void PrevQuoatationSummary_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", $"PrevTab('PaymentTerms');", true);
            gvDownPayment.DataSource = null;
            gvDownPayment.DataBind();

            gvAmortization.DataSource = null;
            gvAmortization.DataBind();

            gvAddCharges.DataSource = null;
            gvAddCharges.DataBind();
        }
        protected void gvDownPayment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDownPayment.PageIndex = e.NewPageIndex;
            LoadData(gvDownPayment, "gvDownPayment");
        }
        protected void gvAmortization_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAmortization.PageIndex = e.NewPageIndex;
            LoadData(gvAmortization, "gvAmortization");
        }
        protected void tSample_Click(object sender, EventArgs e)
        {
            //if (lblID.Text == "Sample Quotation")
            //{
            //    btnSelectAgent.Enabled = false;
            //}
            //else
            //{
            //    btnSelectAgent.Enabled = true;
            //}

            //2023-03-23 : HIDE INCENTIVE OPTION IF SAMPLE QUOTATION
            divIncentive.Visible = false;
            divIncentive2.Visible = false;
            txtIncentiveOption.Value = "No";
            txtIncentiveOption2.Value = "No";


            Clear();
            clearFields();
            ClearAll("Project");
            clearPage();

            lblID.Text = "Sample Quotation";
            lblDocNum.Text = "Sample Quotation";
            lblFinish.InnerText = "Finish";

            //tabSalesPerson.Visible = false;
            DateTime date;
            string tDate = "";
            if (!string.IsNullOrEmpty(txtBirthday.Value))
            {
                date = DateTime.Parse(txtBirthday.Value);
                tDate = date.ToString("dd-MMM-yyyy");
            }

            //2023-04-18 : CHANGED - REQUEST BY DHEZA https://direcbti.monday.com/boards/4119829782/views/93297238/pulses/4318010193
            //lblBusinessType.Text = ddlBusinessType.Text;
            lblBusinessType.Text = "";


            lblLastName.Text = txtLastName.Value;
            lblFirstName.Text = txtFirstName.Value;
            lblMiddleName.Text = txtMiddleName.Value;
            lblBirthday.Text = tDate;
            lblNatureofEmployment.Text = tNatureofEmp.Value;

            //2023-05-05 : REQUESTED BY DHEZA
            lblTaxClassification.Text = ddlBusinessType.Text;

            lblTypeofID.Text = tTypeOfId.Value;
            lblIDNo.Text = tIDNo.Value;
            string MidName = txtMiddleName.Value;

            if (!string.IsNullOrEmpty(MidName))
            { MidName = txtMiddleName.Value[0].ToString(); }
            lblName.Text = $"{txtLastName.Value}, {txtFirstName.Value} {MidName}";

            if (lblName.Text == ",  ")
            { lblName.Text = ""; }

            ClearBP();
            ScriptManager.RegisterStartupScript(this, GetType(), "NewQuotationHide", "MsgNewQuotation_Hide();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "MsgSampleActual_Hide();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "MsgBuyers_Hide();", true);
        }
        protected void bProjName_ServerClick(object sender, EventArgs e)
        {
            ViewState["dtProjectList"] = ws.GetProjects().Tables["Projects"];
            LoadData(gvProjectList, "dtProjectList");

            if (lblID.Text != "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "ProjShow", "MsgProjList_Show();", true);
            }
            else { alertMsg("Please choose buyers first!", "warning"); }

        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            LoadFromFindBtn();
            ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "MsgBuyers_Show();", true);
        }
        protected void btnCardCode_ServerClick(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = hana.GetData("CALL sp_GetBP;", hana.GetConnection("SAOHana"));
            ViewState["gvBPList"] = dt;
            LoadData(gvBPList, "gvBPList");
            txtLastName.Disabled = false;
            txtFirstName.Disabled = false;
            txtMiddleName.Disabled = false;

            //if (lblID.Text == "Sample Quotation")
            //{ lblFinish.InnerText = "Finish"; }
            //else
            //{ lblFinish.InnerText = "Update"; }

            ViewState["BPSelection"] = "BUYER";
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgBPList_Show", "MsgBPList_Show();", true);
        }
        protected void btnBAAdd_ServerClick(object sender, EventArgs e)
        {

            //2023-03-23 : SHOW INCENTIVE OPTION IF NOT SAMPLE QUOTATION
            divIncentive.Visible = true;
            divIncentive2.Visible = true;

            Clear();
            clearFields();
            ClearAll("Project");
            clearPage();
            gvCoOwner.DataSource = null;
            gvCoOwner.DataBind();
            loadDivisionsForNames(ddlBusinessType.Text);

            ddlBusinessType.Items.FindByValue("Co-ownership").Enabled = false;
            ddlBusinessType.Items.FindByValue("Guardianship").Enabled = false;
            ddlBusinessType.Items.FindByValue("Trusteeship").Enabled = false;
            ddlBusinessType.Items.FindByValue("Others").Enabled = false;

            ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "MsgSampleActual_Hide();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "NewQuotationShow", "MsgNewQuotation_Show();", true);
        }

        void loadDivisionsForNames(string type)
        {
            DataTable taxclass = new DataTable();
            taxclass = (DataTable)ViewState["TaxClass"];
            taxclass.Rows.Clear();
            taxclass.Rows.Add("", "");
            RequiredFieldValidator3.Enabled = true;

            if (type == "Individual")
            {
                divSpec.Visible = false;
                //divcoowner.Visible = false;
                divothers.Visible = false;

                specHeader.Visible = false;

                divIndividual.Visible = true;
                trFirstName.Visible = true;
                trLastName.Visible = true;
                trMiddleName.Visible = true;

                trBirthday.Visible = true;
                trIdType.Visible = true;
                trIdNumber.Visible = true;
                trNatureEmployment.Visible = true;
                trTin.Visible = true;

                trCompanyName.Visible = false;
                divCorp.Visible = false;
                txtCompanyName.Value = " ";
                txtTinNumber.Value = " ";
                tTIN1.Text = "";

                txtBirthday.Value = null;

                cvCoOwner.Enabled = false;

                taxclass.Rows.Add("Engaged in Business", "Engaged in Business");
                taxclass.Rows.Add("Not engaged in Business", "Not engaged in Business");
                ddTaxClass.DataSource = taxclass;
                ddTaxClass.DataBind();
            }
            else if (type == "Guardianship" || type == "Trusteeship")
            {
                divSpec.Visible = true;
                //divcoowner.Visible = false;
                divothers.Visible = false;

                specHeader.Visible = true;
                specHeader.InnerText = type + " Details";

                divIndividual.Visible = true;
                trFirstName.Visible = true;
                trLastName.Visible = true;
                trMiddleName.Visible = true;

                trBirthday.Visible = true;
                trIdType.Visible = true;
                trIdNumber.Visible = true;
                trNatureEmployment.Visible = true;
                trTin.Visible = true;

                trCompanyName.Visible = false;
                divCorp.Visible = false;
                txtCompanyName.Value = " ";
                txtTinNumber.Value = " ";
                tTIN1.Text = "";

                cvCoOwner.Enabled = false;

                taxclass.Rows.Add("Engaged in Business", "Engaged in Business");
                taxclass.Rows.Add("Not engaged in Business", "Not engaged in Business");
                ddTaxClass.DataSource = taxclass;
                ddTaxClass.DataBind();


                //2023-06-20 : DISABLE TIN REQUIREMENT WHEN GUARDIANSHIP = GUARDEE
                if (type == "Guardianship" && tspecRelationship.Value.ToUpper() == "GUARDEE")
                {
                    RequiredFieldValidator3.Enabled = false;
                }
            }
            else if (type == "Co-ownership" || type == "Others")
            {
                divSpec.Visible = false;
                if (type == "Co-ownership")
                {
                    //divcoowner.Visible = true;
                    divothers.Visible = false;

                    //cvCoOwner.Enabled = true;
                    cvCoOwner.Enabled = false;

                    specHeader.Visible = false;
                }
                else
                {
                    //divcoowner.Visible = false;
                    divothers.Visible = true;

                    cvCoOwner.Enabled = false;
                    specHeader.Visible = true;
                    specHeader.InnerText = type + " Details";
                }



                divIndividual.Visible = true;
                trFirstName.Visible = true;
                trLastName.Visible = true;
                trMiddleName.Visible = true;

                trBirthday.Visible = true;
                trIdType.Visible = true;
                trIdNumber.Visible = true;
                trNatureEmployment.Visible = true;
                trTin.Visible = true;

                trCompanyName.Visible = false;
                divCorp.Visible = false;
                txtCompanyName.Value = " ";
                txtTinNumber.Value = " ";
                tTIN1.Text = "";

                taxclass.Rows.Add("Engaged in Business", "Engaged in Business");
                taxclass.Rows.Add("Not engaged in Business", "Not engaged in Business");
                ddTaxClass.DataSource = taxclass;
                ddTaxClass.DataBind();
            }
            else
            {
                divSpec.Visible = false;
                //divcoowner.Visible = false;
                divothers.Visible = false;

                specHeader.Visible = false;

                divIndividual.Visible = false;
                trFirstName.Visible = false;
                trLastName.Visible = false;
                trMiddleName.Visible = false;

                trBirthday.Visible = false;
                trIdType.Visible = false;
                trIdNumber.Visible = false;
                trNatureEmployment.Visible = false;
                trTin.Visible = true;

                trCompanyName.Visible = true;
                divCorp.Visible = true;
                txtComaker.Value = " ";
                txtLastName.Value = " ";
                txtFirstName.Value = " ";
                txtMiddleName.Value = " ";
                tNatureofEmp.Value = " ";
                tTypeOfId.Value = " ";
                txtBirthday.Value = "1999-01-01";
                tIDNo.Value = " ";
                txtTinNumber.Value = "";
                tTIN1.Text = " ";

                cvCoOwner.Enabled = false;

                ddTaxClass.Visible = true;

                taxclass.Rows.Clear();
                //taxclass.Rows.Add("Corporation", "Corporation");
                taxclass.Rows.Add("Engaged in Business", "Engaged in Business");
                taxclass.Rows.Add("Not engaged in Business", "Not engaged in Business");

                ddTaxClass.DataSource = taxclass;
                ddTaxClass.DataBind();
            }

            taxClassChanged();
        }

        void clearPage()
        {
            tDocDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            tProjName.Value = "";
            tBlock.Value = "";
            tLot.Value = "";
            tModel.Value = "";
            tFinancing.Value = "";
            tLotArea.Value = "";
            tFloorArea.Value = "";
            tHouseStatus.Value = "";
            tPhase.Value = "";
            txtLotClassification.Value = "";
            txtProductType.Value = "";
            txtLoanType.Value = "";
            tBank.Value = "";

            clearFigureTextBoxes();


            lblEmployeeID.Text = "";
            lblEmployeeName.Text = "";
            lblEmployeePosition.Text = "";
            lblEmployeeBrokerID.Text = "";

            gvCoOwner.DataSource = null;
            gvCoOwner.DataBind();

            tMiscDocDate.Text = Convert.ToDateTime(tDocDate.Text).AddMonths(1).ToString("yyyy-MM-dd");

        }


        void clearFigureTextBoxes()
        {
            tODas.Value = "";
            tNetDas.Value = "";
            txtDPMonthly.Text = "";
            txtMiscFees.Text = "";
            txtMiscTerms.Text = "";
            txtMiscMonthly.Text = "";
            tRemarks.Value = String.Empty;
            tResrvFee.Text = "";
            tPDBalance.Text = "";
            tDPPercent.Text = "";
            tDPAmount.Text = "";
            txtDPTerms.Text = "";
            txtDiscPercent.Text = "";
            tSpotDPDiscAmt.Text = "";
            txtLTerms.Text = "0";
            lblLBTerms.Text = txtLTerms.Text;

            txtFactorRate1.Text = "0";
            //dtpDueDate.Text = null;
            txtGDI.Value = "0";
            txtLoanType.Value = String.Empty;
            tBank.Value = String.Empty;

            tDocDate2.Value = DateTime.Now.ToString("yyyy-MM-dd");
            tProjName2.Value = "";
            tBlock2.Value = "";
            tLot2.Value = "";
            tModel2.Value = "";
            tFinancing2.Value = "";
            txtLotArea2.Value = "";
            txtFloorArea2.Value = "";
            tHouseStatus2.Value = "";
            txtPhase2.Value = "";
            txtLotClassification.Value = "";
            txtProductType2.Value = "";
            txtLoanType2.Value = "";
            tBank2.Value = "";
            txtLoanableBalance.Text = "";
            txtIncentiveOption2.Value = "";
        }

        protected void gvBPList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBPList.PageIndex = e.NewPageIndex;
            LoadData(gvBPList, "gvBPList");
        }
        protected void bSelectBP_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton GetID = (LinkButton)sender;
                string Code = GetID.CommandArgument;
                DataTable dt = new DataTable();
                dt = hana.GetData($@"CALL sp_GetBPbyCardCode ('{Code}')", hana.GetConnection("SAOHana"));

                //2023-06-20 : CLEAR VALUES OF GUARDIANSHIP/TRUSTEESHIP ON INITIAL DATA
                clearSpecNames();

                string test = ViewState["BPSelection"].ToString();

                //FOR SELECTION OF PRINCIPAL BUYER
                if (ViewState["BPSelection"].ToString() == "BUYER")
                {

                    txtCardCode.Value = Code;
                    BoolBP(true);


                    if (DataAccess.Exist(dt) == true)
                    {

                        if ((string)DataAccess.GetData(dt, 0, "BusinessType", "Individual") != "Individual" && (string)DataAccess.GetData(dt, 0, "BusinessType", "Individual") != "Corporation")
                        {
                            ddlBusinessType.Items.FindByValue("Co-ownership").Enabled = true;
                            ddlBusinessType.Items.FindByValue("Guardianship").Enabled = true;
                            ddlBusinessType.Items.FindByValue("Trusteeship").Enabled = true;
                            ddlBusinessType.Items.FindByValue("Others").Enabled = true;
                        }

                        ddlBusinessType.SelectedValue = (string)DataAccess.GetData(dt, 0, "BusinessType", "Individual");

                        loadDivisionsForNames(ddlBusinessType.SelectedValue);
                        string taxclass = (string)DataAccess.GetData(dt, 0, "TaxClassification", "");
                        if (taxclass.ToUpper() == ConfigSettings.TaxClassification1.ToUpper()
                                || taxclass.ToUpper() == ConfigSettings.TaxClassification2.ToUpper()
                                || taxclass.ToUpper() == "CORPORATION")
                        {
                            if (taxclass.ToUpper() == "ENGAGED IN BUSINESS")
                            {
                                taxclass = "Engaged in Business";
                            }
                            else if (taxclass.ToUpper() == "NOT ENGAGED IN BUSINESS")
                            {
                                taxclass = "Not engaged in Business";
                            }
                            else
                            {
                                taxclass = "Engaged in Business";
                            }
                            ddTaxClass.SelectedValue = taxclass;
                        }

                        txtLastName.Value = (string)DataAccess.GetData(dt, 0, "LastName", "");
                        txtFirstName.Value = (string)DataAccess.GetData(dt, 0, "FirstName", "");
                        txtMiddleName.Value = (string)DataAccess.GetData(dt, 0, "MiddleName", "");
                        //if (ddlBusinessType.SelectedValue == "Corporation")
                        //{
                        //    txtComakerCorp.Value = (string)DataAccess.GetData(dt, 0, "Comaker", "N/A");
                        //}
                        //else
                        //{
                        txtComaker.Value = (string)DataAccess.GetData(dt, 0, "Comaker", "N/A");
                        //}

                        txtCompanyName.Value = (string)DataAccess.GetData(dt, 0, "CompanyName", "");

                        string bdate = "";
                        DateTime date;

                        bdate = (string)DataAccess.GetData(dt, 0, "BirthDay", "");
                        if (!string.IsNullOrEmpty(bdate))
                        {
                            date = DateTime.Parse(bdate);
                            bdate = date.ToString("yyyy-MM-dd");
                        }
                        txtBirthday.Value = bdate;
                        //ViewState["Broker"]  = (string)DataAccess.GetData(dt, 0, "Broker", "");
                        //ViewState["SalesManager"]  = (string)DataAccess.GetData(dt, 0, "SalesManager", "");


                        string tCode;
                        tCode = (string)DataAccess.GetData(dt, 0, "NatureEmp", "");

                        if (ws.OLSTExist(tCode) == true)
                        {
                            ViewState["tNatureofEmp"] = tCode;
                            tNatureofEmp.Value = ws.GetOLSTName(tCode);
                        }
                        else
                        {
                            if (taxclass.ToUpper() == "ENGAGED IN BUSINESS")
                            {
                                ViewState["tNatureofEmp"] = "NE2";
                                tNatureofEmp.Value = ws.GetOLSTName("NE2");
                            }
                            else
                            {
                                ViewState["tNatureofEmp"] = "OTH";
                                tNatureofEmp.Value = ws.GetOLSTName(tCode);
                            }
                        }

                        tCode = (string)DataAccess.GetData(dt, 0, "IDType", "");

                        if (ws.OLSTExist(tCode) == true)
                        { ViewState["tTypeOfId"] = tCode; tTypeOfId.Value = ws.GetOLSTName(tCode); }
                        else { ViewState["tTypeOfId"] = "OTH"; tTypeOfId.Value = tCode; }

                        tIDNo.Value = (string)DataAccess.GetData(dt, 0, "IDNo", "");

                        txtTinNumber.Value = (string)DataAccess.GetData(dt, 0, "TIN", "");
                        tTIN1.Text = (string)DataAccess.GetData(dt, 0, "TIN", "");

                        loadCoMaker();

                        if (ddlBusinessType.SelectedValue != "Individual" && ddlBusinessType.SelectedValue != "Corporation")
                        {
                            string qryGuardianship = $@"SELECT A.""BusinessType"", B.""FirstName"", B.""MiddleName"", B.""LastName"", B.""EmailAddress"", B.""CellNo"", C.""SpecialBuyerRole"" 
                                                        FROM OCRD A LEFT JOIN CRD7 C ON A.""CardCode"" = C.""CardCode"" LEFT JOIN CRD1 B ON B.""CardCode"" = C.""SpecialBuyerLink""
                                                        WHERE A.""CardCode"" = '{Code}' AND B.""CardType"" = 'Buyer'";
                            DataTable dtGuardianship = hana.GetData(qryGuardianship, hana.GetConnection("SAOHana"));
                            if (dtGuardianship.Rows.Count > 0)
                            {
                                //FOR CO-OWNER / GUARDIANSHIP
                                if (ddlBusinessType.SelectedValue != "Co-ownership" && ddlBusinessType.SelectedValue != "Others")
                                {
                                    //FOR GUARDISAN AND TRUSTEESHIP
                                    tspecFName.Value = (string)DataAccess.GetData(dtGuardianship, 0, "FirstName", "");
                                    tspecMName.Value = (string)DataAccess.GetData(dtGuardianship, 0, "MiddleName", "");
                                    tspecLName.Value = (string)DataAccess.GetData(dtGuardianship, 0, "LastName", "");
                                    tspecRelationship.Value = (string)DataAccess.GetData(dtGuardianship, 0, "SpecialBuyerRole", "");
                                }
                                else
                                {
                                    //FOR CO-OWNER AND OTHERS
                                    loadOthers();
                                }
                            }
                        }

                    }
                }


                //FOR SELECTION OF CO-OWNER BUYER
                else
                {
                    int tag = 0;
                    DataTable dtCoOwner = new DataTable();
                    dtCoOwner = (DataTable)ViewState["CoOwner"];

                    if (dtCoOwner.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtCoOwner.Rows)
                        {
                            if (row["Code"].ToString() == Code)
                            {
                                tag = 1;
                            }
                        }
                    }


                    if (tag == 0)
                    {

                        string LName = (string)DataAccess.GetData(dt, 0, "LastName", "");
                        string FName = (string)DataAccess.GetData(dt, 0, "FirstName", "");
                        string MName = (string)DataAccess.GetData(dt, 0, "MiddleName", "");
                        string CompanyName = (string)DataAccess.GetData(dt, 0, "CompanyName", "");
                        string Name = string.IsNullOrWhiteSpace(CompanyName) ? FName + ' ' + MName + ' ' + LName : CompanyName;

                        dtCoOwner.Rows.Add(Code
                                    , Name);

                        ViewState["CoOwner"] = dtCoOwner;
                        gvCoOwner.DataSource = dtCoOwner;
                        gvCoOwner.DataBind();
                    }
                    else
                    {
                        alertMsg("Customer already exists. Please try again.", "warning");
                    }

                }


                //2023-06-20 : DISABLE TIN REQUIREMENT WHEN GUARDIANSHIP = GUARDEE
                if (ddlBusinessType.SelectedValue == "Guardianship" && tspecRelationship.Value.ToUpper() == "GUARDEE")
                {
                    RequiredFieldValidator3.Enabled = false;
                }

                ScriptManager.RegisterStartupScript(this, GetType(), "MsgBPList_Hide", "MsgBPList_Hide();", true);

            }
            catch (Exception ex)
            {

                alertMsg(ex.Message, "error");
            }

        }

        void clearSpecNames()
        {
            //2023-06-20 : CLEAR VALUES OF GUARDIANSHIP/TRUSTEESHIP ON INITIAL DATA

            //CLEAR VALUES : FOR GUARDISAN AND TRUSTEESHIP
            tspecFName.Value = string.Empty;
            tspecMName.Value = string.Empty;
            tspecLName.Value = string.Empty;
            tspecRelationship.Value = string.Empty;
        }

        protected void btnNextQuick_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] == null)
                {
                    alertMsg("Session expired!", "error");
                    Response.Redirect("~/pages/Login.aspx");
                }
                if ((int)Session["UserID"] == 2)
                {
                    gvAmortization.Visible = false;
                }
                else { gvAmortization.Visible = true; }


                string oCode = (string)ViewState["tFinancing"];

                tDocDate2.Value = tDocDate.Text;
                tProjName2.Value = tProjName.Value;
                tBlock2.Value = tBlock.Value;
                tLot2.Value = tLot.Value;
                tModel2.Value = tModel.Value;
                //tPhase2.Value = tPhase.Value;
                //tSize2.Value = tSize.Value;
                //tFeature2.Value = tFeature.Value;
                //tLotArea2.Value = tLotArea.Value;
                //tFloorArea2.Value = tFloorArea.Value;
                tHouseStatus2.Value = tHouseStatus.Value;
                tFinancing2.Value = tFinancing.Value;
                //tAccountType2.Value = tAccountType.Value;
                //tSalesType2.Value = tSalesType.Value;
                //tTcp2.Value = lblNetTCP.Text;
                //tTotalDisc2.Text = lblDiscount.Text;
                //tNetDisc2.Text = lblNetDAS.Text;
                //tResrvFee2.Value = tResrvFee.Text;
                //tNetDP2.Value = lblDownPayment.Text;
                //tDPPercent2.Text = SystemClass.ToDecimal(tDPPercent.Text);
                //tDPAmount2.Value = SystemClass.ToCurrency((Convert.ToDecimal(lblDownPayment.Text) + Convert.ToDecimal(tResrvFee.Text)).ToString());
                //tLAmount2.Value = lblAmountDue.Text;

                DateTime dueDate;
                if (lblLBDueDate.Text == "-")
                {
                    if (tFinancing.Value == "Spot Cash")
                    {
                        dueDate = Convert.ToDateTime(tDocDate.Text);
                    }
                    else { dueDate = Convert.ToDateTime(lblDPDueDate.Text); }
                }
                else { dueDate = Convert.ToDateTime(lblLBDueDate.Text); }

                if (ddlAllowed.Text == "Approved" && double.Parse(tTerms.Text) == 0)
                {
                    alertMsg("No Additional Charges Term supplied.", "error");
                }
                else
                {

                    if (ddlAllowed.Text == "Approved")
                    {
                        hAddCharges.Visible = true;

                        double AdditionalCharges;
                        AdditionalCharges = double.Parse(lblTCPMonthly.Text);

                        ViewState["gvAddCharges"] = ws.PaymentBreakdown(int.Parse(tTerms.Text),
                                                                      dueDate,
                                                                      AdditionalCharges,
                                                                      0,
                                                                      Convert.ToDouble(lblBalance.Text),
                                                                      "AC",
                                                                      0,
                                                                      double.Parse(lblMiscFees.Text),
                                                                      0, 0).Tables["PaymentBreakdown"];
                        LoadData(gvAddCharges, "gvAddCharges");
                    }

                    DataTable dt = new DataTable();
                    dt = hana.GetData($@"SELECT ""Code"", ""Name"" FROM ""OLST"" WHERE ""GrpCode"" = 'FS' AND ""Name"" = '{tFinancing.Value}'", hana.GetConnection("SAOHana"));
                    string oFSCode = (string)DataAccess.GetData(dt, 0, "Code", "0");
                    dt = hana.GetData($@"SELECT ""U_InterestRate"" FROM ""@FACTOR_RATE"" WHERE ""U_Terms"" = '{txtLTerms.Text}' AND ""U_FinancingScheme"" = '{oFSCode}'", hana.GetConnection("SAPHana"));
                    ViewState["IRate"] = (string)DataAccess.GetData(dt, 0, "U_InterestRate", "0");

                    ViewState["IRate"] = string.IsNullOrWhiteSpace(Convert.ToString(ViewState["IRate"])) ? "0" : ViewState["IRate"];

                    //DP
                    //ViewState["gvDownPayment"]  = ws.PaymentBreakdown((int)ViewState["ActualDPTerms"], dueDate, double.Parse(lblMonthly1.Text), 0, double.Parse(lblDownPayment.Text), "DP", Convert.ToDouble(ViewState["IRate"]), double.Parse(lblAddMiscCharges2.Text)).Tables["PaymentBreakdown"];
                    //LoadData(gvDownPayment, "gvDownPayment");

                    ViewState["gvDownPayment"] = ws.PaymentBreakdown((int)ViewState["ActualDPTerms"], dueDate, double.Parse(lblTCPMonthly.Text), 0, double.Parse(lblBalance.Text), "DP",
                                        Convert.ToDouble(ViewState["IRate"]), double.Parse(lblMiscFees.Text), 0, 0).Tables["PaymentBreakdown"];
                    LoadData(gvDownPayment, "gvDownPayment");

                    //MA
                    if (oCode == "B" || oCode == "P" || oCode == "SC")
                    {
                        ViewState["gvAmortization"] = ws.PaymentBreakdownBankPagibig(Convert.ToDateTime(lblLBDueDate.Text), Convert.ToDouble(lblAmountDue.Text), 0).Tables[0];
                        LoadData(gvAmortization, "gvAmortization");
                    }
                    else
                    {
                        //ViewState["gvAmortization"] = ws.PaymentBreakdown(int.Parse(txtLTerms.Text), Convert.ToDateTime(lblDueDate3.Text), double.Parse(lblMonthly2.Text), double.Parse(tInterestRate.Text), double.Parse(lblAmountDue.Text), "LB", Convert.ToDouble(ViewState["IRate"])).Tables[0];
                        ViewState["gvAmortization"] = ws.PaymentBreakdown(int.Parse(txtLTerms.Text), Convert.ToDateTime(lblLBDueDate.Text), double.Parse(lblLBMonthly.Text), 0,
                             double.Parse(lblAmountDue.Text), "LB", Convert.ToDouble(ViewState["IRate"]), double.Parse(lblAddMiscCharges2.Text), 0, 0).Tables[0];
                        //ViewState["gvAmortization"] = ws.PaymentBreakdown(int.Parse(txtLTerms.Text), Convert.ToDateTime(lblDueDate3.Text), double.Parse(lblMiscMonthly.Text), 0, double.Parse(txtLoanableBalance2.Text), "LB", Convert.ToDouble(ViewState["IRate"]), double.Parse(lblAddMiscFees.Text)).Tables[0];
                        LoadData(gvAmortization, "gvAmortization");
                    }

                    if (lblID.Text == "Sample Quotation")
                    { bFinish.Visible = false; }
                    else
                    { bFinish.Visible = true; }

                    NextTab("QuotationSummary");


                }
            }
            catch (Exception ex)
            { alertMsg(ex.Message, "error"); }
        }
        protected void gvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvList.PageIndex = e.NewPageIndex;
            LoadData(gvList, "gvList");
        }
        protected void btnSelectEmpList_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string Code = GetID.CommandArgument;

            string qry = $@"SELECT * FROM OSLA A WHERE A.""TransID"" = '{Code}'";
            DataTable dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
            //lblEmployeeID.Text = Code;
            lblEmployeeID.Text = DataAccess.GetData(dt, 0, "Id", "0").ToString();
            lblEmployeeName.Text = DataAccess.GetData(dt, 0, "SalesPerson", "").ToString();
            lblEmployeePosition.Text = DataAccess.GetData(dt, 0, "Position", "").ToString();
            lblEmployeeBrokerID.Text = DataAccess.GetData(dt, 0, "BrokerId", "").ToString();

            //2023-11-07 : ADDED BROKER NAME
            getBrokerName(lblEmployeeID.Text);

            qry = $@"SELECT * FROM BRK2 A WHERE A.""SalesPersonId"" = '{Code}' AND ";
            dt = hana.GetData(qry, hana.GetConnection("SAOHana"));



            //Label lblName = (Label)gvPosList.Rows[index].FindControl("lblSalesName");
            //Label lblCode = (Label)gvPosList.Rows[index].FindControl("lblSalesCode");

            //lblName.Text = (string)DataAccess.GetData(hana.GetData($@"SELECT ""LastName"" || ', ' || ""FirstName"" ""CardName"" FROM ""OHEM"" WHERE ""EmpID"" = '{Code}'", hana.GetConnection("SAPHana")), 0, "CardName", "");
            ////lblName.Text = (string)DataAccess.GetData(ws.Select($"SELECT CardName FROM OCRD WHERE CardCode = '{Code}'", "CardName", "SAP").Tables["CardName"], 0, "CardName", "");

            //lblCode.Text = Code;
            //txtSearchEmpList.Value = "";

            ScriptManager.RegisterStartupScript(this, GetType(), "MsgEmpListHide", "MsgEmpList_Hide();", true);
        }
        protected void gvEmpList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvEmpList.PageIndex = e.NewPageIndex;
            LoadData(gvEmpList, "gvEmpList");
        }
        protected void btnSearchEmpList_ServerClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearchEmpList.Value))
            {
                //ViewState["gvEmpList"] = hana.GetData($@"SELECT ""EmpID"" ""CardCode"", ""LastName"" || ', ' || ""FirstName"" ""CardName"" FROM ""OHEM"" WHERE (""LastName"" || ', ' || ""FirstName"") LIKE '%{txtSearchEmpList.Value}%'", hana.GetConnection("SAPHana"));
                //ViewState["gvEmpList"] = ws.Select($"SELECT CardCode,CardName FROM OCRD WHERE CardName LIKE '%{txtSearchEmpList.Value}%'", "EmployeeList", "SAP").Tables["EmployeeList"];
                //LoadData(gvEmpList, "gvEmpList");

                string brokerID = ViewState["BrokerId"] == null ? "" : ViewState["BrokerId"].ToString();
                ViewState["gvEmpList"] = ws.GetSalesEmployeesSearch(DateTime.Now, brokerID, txtSearchEmpList.Value).Tables["GetSalesEmployeesSearch"];
                LoadData(gvEmpList, "gvEmpList");

            }
            else
            { LoadEmpList(); }
        }
        protected void bPosList_ServerClick(object sender, EventArgs e)
        {
            Control btn = (Control)sender;

            GridViewRow row = (GridViewRow)btn.NamingContainer;
            ViewState["index"] = row.RowIndex;
            LoadEmpList();
        }
        protected void bPrint_Click(object sender, EventArgs e)
        {
            try
            {

                int check = 0;
                DataTable dtAdjacentLotPrice1 = ws.GetHouseDetails(hPrjCode.Value, tBlock.Value, tLot.Value, tFinancing.Value).Tables["GetHouseDetails"];
                double HousePrice1 = Convert.ToDouble(DataAccess.GetData(dtAdjacentLotPrice1, 0, "HousePrice", "0").ToString());

                //##########    BLOCKING WHEN THE SELECTED BLOCK AND LOT IS "HOUSE AND LOT" AND DOES NOT HAVE HOUSE PRICE ###########//
                if (txtProductType.Value.ToUpper() == "HOUSE AND LOT" && HousePrice1 <= 0)
                {
                    check = 1;
                }

                if (check == 0)
                {

                    //deleteSampleQuotation();

                    string flrArea = "0";
                    if (!string.IsNullOrEmpty(tFloorArea.Value))
                    {
                        if (tFloorArea.Value != "-")
                        {
                            flrArea = tFloorArea.Value;
                        }
                    }


                    var test1 = ViewState["QuoteDocEntry"];
                    var test2 = ViewState["DPPercent"].ToString();
                    var test3 = ViewState["DiscPercent"].ToString();
                    var test4 = Session["UserID"].ToString();
                    var test5 = ViewState["DocNum"].ToString();

                    string qry = $@"SELECT ""U_LTSNo"" FROM OBTN WHERE ""U_Project"" = '{hPrjCode.Value}' AND ""U_BlockNo"" = '{tBlock.Value}' AND ""U_LotNo"" = '{tLot.Value}'";
                    DataTable dtLTS = hana.GetDataDS(qry, hana.GetConnection("SAPHana")).Tables[0];
                    string LTSNo = DataAccess.GetData(dtLTS, 0, "U_LTSNo", "").ToString();


                    string qryPaymentScheme = $@"SELECT IFNULL(""U_PmtSchemeType"",'') ""U_PmtSchemeType"" FROM ""@FINANCINGSCHEME"" WHERE ""Code"" = '{tFinancing.Value}'";
                    DataTable dtPaymentScheme = hana.GetDataDS(qryPaymentScheme, hana.GetConnection("SAPHana")).Tables[0];
                    string PaymentScheme = DataAccess.GetData(dtPaymentScheme, 0, "U_PmtSchemeType", "").ToString();



                    //CHECK IF VATABLE OR NOT
                    DataTable dtVATThreshold = hana.GetData($@"SELECT ""U_ThresholdAmount"" FROM ""@PRODUCTTYPE"" WHERE UPPER(""Code"") = UPPER('{txtProductType.Value}')", hana.GetConnection("SAPHana"));
                    double vatThreshold = Convert.ToDouble(DataHelper.DataTableRet(dtVATThreshold, 0, "U_ThresholdAmount", "0"));
                    double adjacentLotPrice = 0;
                    string vatable = "";
                    if (txtAdjLot.Value == "Yes")
                    {
                        DataTable dtAdjacentLotPrice = ws.GetHouseDetails(hPrjCode.Value, tBlock.Value, txtLotNo.Value, tFinancing.Value).Tables["GetHouseDetails"];
                        double LandPrice = Convert.ToDouble(DataAccess.GetData(dtAdjacentLotPrice, 0, "LandPrice", "0").ToString());
                        double HousePrice = Convert.ToDouble(DataAccess.GetData(dtAdjacentLotPrice, 0, "HousePrice", "0").ToString());
                        adjacentLotPrice = LandPrice + HousePrice;
                    }
                    double TotalPrice = Convert.ToDouble(lblNetDAS.Text) + adjacentLotPrice;
                    vatable = (TotalPrice > vatThreshold ? "Y" : "N");


                    //if (string.IsNullOrWhiteSpace(tDPPercent.Text))
                    //{
                    //    tDPPercent.Text = "0";
                    //}

                    //if (string.IsNullOrWhiteSpace(txtDiscPercent.Text))
                    //{
                    //    txtDiscPercent.Text = "0";
                    //}



                    var ticks = DateTime.Now.Ticks;
                    var guid = Guid.NewGuid().ToString();
                    var uniqueSessionId = ticks.ToString() + guid.ToString();





                    int id = 0;
                    //CHECK COUNT OF DATA IN OQUU
                    DataTable dtSampleQuotationCount = hana.GetData($@"SELECT COUNT(""DocEntry"") ""CountOQUU"" FROM ""OQUU""", hana.GetConnection("SAOHana"));
                    int SampleQuotationCount = Convert.ToInt32(DataHelper.DataTableRet(dtSampleQuotationCount, 0, "CountOQUU", "0"));

                    string ret = ws.SampleQuotation(
                                       4,
                                       SampleQuotationCount + 1,  //2
                                       DataAccess.GetViewState(lblID.Text, ""),
                                       Convert.ToDateTime(tDocDate.Text), //4
                                       "O",

                                       hPrjCode.Value, //6
                                       tBlock.Value,
                                       tLot.Value, //8
                                       tModel.Value,
                                       tFinancing.Value, //10
                                       tLotArea.Value,
                                       flrArea, //12
                                       tHouseStatus.Value,
                                       tPhase.Value, //14
                                       txtLotClassification.Value,
                                       txtProductType.Value, //16
                                       txtLoanType.Value,
                                       tBank.Value, //18


                                       double.Parse(lblDASAmt.Text),
                                       double.Parse(tResrvFee.Text), //20
                                       double.Parse(ViewState["DPPercent"].ToString()),
                                       double.Parse(tDPAmount.Text), //22
                                       int.Parse(txtDPTerms.Text),

                                       double.Parse(ViewState["DiscPercent"].ToString()), //24
                                       double.Parse(tSpotDPDiscAmt.Text),
                                       int.Parse(txtLTerms.Text), //26 
                                       double.Parse(txtFactorRate1.Text),
                                       Convert.ToDateTime(lblLBDueDate.Text),  //28
                                       double.Parse(txtGDI.Value),

                                       double.Parse(lblDASAmt.Text),  //30
                                       double.Parse(lblAddMiscCharges.Text),
                                       double.Parse(lblNetDAS.Text),  //32
                                       double.Parse(lblVAT.Text),
                                       double.Parse(lblNetTCP.Text),//34
                                       double.Parse(lblDownPayment.Text),
                                       //double.Parse(lblMonthly1.Text),    //36
                                       double.Parse(lblTCPMonthly.Text),    //36
                                       Convert.ToDateTime(lblDPDueDate.Text),
                                       double.Parse(lblAmountDue.Text), //38
                                       (int)Session["UserID"],


                                       lblEmployeeID.Text,    //40
                                       lblEmployeeName.Text,
                                       lblEmployeePosition.Text,  //42
                                       (DataTable)ViewState["gvDownPayment"],
                                       (DataTable)ViewState["gvAmortization"], //44
                                       uniqueSessionId,
                                       double.Parse(txtMiscMonthly.Text), //46

                                       double.Parse(lblTCPMonthly.Text),
                                       double.Parse(lblMiscDPMonthly.Text), //48
                                       double.Parse(lblAddMiscFees.Text),
                                       double.Parse(tPDBalance.Text), //50
                                       double.Parse(lblBalance.Text),
                                       double.Parse(txtMiscFees.Text), //52
                                       double.Parse(txtLoanableBalance.Text),
                                       double.Parse(lblLoanableBalance.Text), //54
                                       tRemarks.Value,
                                       tRetType.Value, //56
                                       double.Parse(lblCompTotal.Text),

                                       txtAdjLot.Value, //58
                                       txtLotNo.Value,

                                       //RELEATED TO MISCELLANEOUS ADJUSTMENTS
                                       Convert.ToDateTime(lblMiscDueDate.Text),
                                       txtFinancingMisc.Value,
                                       txtMiscTerms.Text,
                                       (DataTable)ViewState["gvMiscellaneous"],
                                       double.Parse(lblMiscDPAmount.Text),
                                       int.Parse(lblMiscLBTerms.Text),
                                       double.Parse(lblMiscLBAmount.Text),
                                       double.Parse(lblMiscLBMonthly.Text),
                                       (DataTable)ViewState["gvMiscellaneousAmort"],

                                       //ddTaxClass.SelectedValue.ToString(),
                                       string.Empty, //60
                                       LTSNo,
                                       PaymentScheme, //62
                                       vatable,
                                       Convert.ToDouble(lblLBMonthly.Text), //64
                                       string.IsNullOrEmpty(txtIncentiveOption.Value) ? "N" : (txtIncentiveOption.Value == "Yes" ? "Y" : "N"),
                                       (DataTable)ViewState["CoOwner"], //44
                                       lblComaker.Text
                                       );

                    if (ret == "Operation completed successfully.")
                    {
                        Session["PrintDocEntry"] = uniqueSessionId;
                        Session["Title"] = "Sample Quotation";
                        Session["ReportName"] = ConfigSettings.QuotationForm;
                        Session["ReportPath"] = ConfigurationManager.AppSettings["ReportPathForms"].ToString();
                        Session["RptConn"] = "SAP";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openpage", "window.open('ReportViewer.aspx');", true);
                        txtDiscPercent.Text = ViewState["DiscPercent"].ToString();
                        tDPPercent.Text = ViewState["DPPercent"].ToString();

                        ViewState["UpdateResFee"] = 0;

                    }
                    else
                    {
                        alertMsg("Error with printing. Please contact administrator.", "error");
                    }
                }
                else
                {
                    alertMsg("No House Price provided in SAP. Please contact administrator.", "info");
                }

            }
            catch
            (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }


            //open new tab 
        }
        protected void bSearchBuyer_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearchbuyer.Value))
            {
                ViewState["dtBuyers"] = hana.GetData($"CALL sp_Search (4,'{txtSearchbuyer.Value}','')", hana.GetConnection("SAOHana"));
                LoadData(gvBPList, "dtBuyers");
            }
            else
            {
                DataTable dt = new DataTable();
                dt = hana.GetData("CALL sp_GetBP;", hana.GetConnection("SAOHana"));
                ViewState["gvBPList"] = dt;
                LoadData(gvBPList, "gvBPList");
            }
        }
        protected void btnPrevTab_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "NextEnable", "NextEnable();", true);
        }
        protected void tDPAmount_TextChanged(object sender, EventArgs e)
        {
            Compute();
        }
        protected void ddlDPDay_TextChanged(object sender, EventArgs e)
        {
            Compute();
        }
        protected void gvAddCharges_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAddCharges.PageIndex = e.NewPageIndex;
            LoadData(gvAddCharges, "gvAddCharges");
        }
        protected void tTerms_TextChanged(object sender, EventArgs e)
        {
            Compute();
        }
        protected void dtpDueDate_TextChanged(object sender, EventArgs e)
        {
            //dueDateChanges();
        }
        protected void bCreateNew_ServerClick(object sender, EventArgs e)
        {
            //btnEditProfile.Visible = true;
            ViewState["QuoteDocEntry"] = 0;
            BoolBP(false);

            string UserName = Session["UserName"].ToString();

            if (UserName.Contains("SP-"))
            {
                btnBAAdd.Visible = false;
            }
            else
            {
                btnBAAdd.Visible = true;
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "MsgBuyers_Hide();", true);
            //ScriptManager.RegisterStartupScript(this, GetType(), "NewQuotationShow", "MsgNewQuotation_Show();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgSampleActual", "MsgSampleActual_Show();", true);
        }
        protected void bCreateNewCoborrower_ServerClick(object sender, EventArgs e)
        {
            ClearCoBorrower();
            //btnEditProfile.Visible = true;
            //ViewState["QuoteDocEntry"]  = 0;
            BoolBP(false);
            ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "MsgBuyers_Hide();", true);
            //ScriptManager.RegisterStartupScript(this, GetType(), "NewQuotationShow", "MsgNewQuotation_Show();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgSampleActual", "MsgCoBorrower_Show();", true);
        }








        protected void bCreateNewCoOwner_ServerClick(object sender, EventArgs e)
        {
            //ClearCoOwner();
            ////btnEditProfile.Visible = true;
            ////ViewState["QuoteDocEntry"]  = 0;
            //BoolBP(false);
            //string InnerText = "";
            //if (ddlBusinessType.SelectedValue == "Co-ownership")
            //{
            //    InnerText = "Co-Owner";
            //}
            //else if (ddlBusinessType.SelectedValue == "Others")
            //{
            //    InnerText = "Others";
            //}
            //coothers.InnerText = InnerText;
            //ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "MsgBuyers_Hide();", true);
            ////ScriptManager.RegisterStartupScript(this, GetType(), "NewQuotationShow", "MsgNewQuotation_Show();", true);
            //ScriptManager.RegisterStartupScript(this, GetType(), "MsgCoOwnermodal", "MsgCoOwnermodal_Show();", true);
            DataTable dt = new DataTable();
            dt = hana.GetData("CALL sp_GetBP;", hana.GetConnection("SAOHana"));
            ViewState["gvBPList"] = dt;
            LoadData(gvBPList, "gvBPList");

            ViewState["BPSelection"] = "CO-OWNER";
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgBPList_Show", "MsgBPList_Show();", true);
        }











        protected void gvProjectList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProjectList.PageIndex = e.NewPageIndex;
            LoadData(gvProjectList, "dtProjectList");
        }
        protected void gvBuyers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBuyers.PageIndex = e.NewPageIndex;
            LoadData(gvBuyers, "dtBuyers");
        }

        protected void btnAcceptNewBuyer_ServerClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {

                //2023-05-31 : REMOVE BLOCKING OF TIN WHEN NOT INDIVIDUAL NOR CORPORATION
                int counter = 0;

                //Check if TIN is in correct format(###-###-###-###)
                bool isOK = Regex.IsMatch((string.IsNullOrWhiteSpace(tTIN1.Text) ? txtTinNumber.Value : tTIN1.Text), @"[0-9][0-9][0-9]\-[0-9][0-9][0-9]\-[0-9][0-9][0-9]\-[0-9][0-9][0-9]");


                //2023-05-31 : REMOVE BLOCKING OF TIN WHEN NOT INDIVIDUAL NOR CORPORATION
                if (!isOK)
                {
                    if (ddlBusinessType.SelectedValue != "Corporation" && ddlBusinessType.SelectedValue != "Individual")
                    {
                        counter = 0;
                    }
                    else
                    {
                        counter++;
                    }
                }


                if (counter == 0)
                {
                    bool isBDateValid = true;
                    try { DateTime bdate = Convert.ToDateTime(txtBirthday.Value); } catch (Exception ex) { isBDateValid = false; }

                    //CHECK IF TIN ALREADY EXISTS
                    string tin = ddlBusinessType.SelectedValue != "Corporation" ? tTIN1.Text : txtTinNumber.Value;
                    string qry1 = $@"SELECT ""CardCode"" FROM ""CRD1"" where ""TIN"" = '{tin}'";
                    DataTable dt1 = hana.GetData(qry1, hana.GetConnection("SAOHana"));
                    //if (dt1.Rows.Count > 0 && String.IsNullOrEmpty(txtCardCode.Value))
                    //{
                    //    alertMsg("Cannot add buyer. TIN already exists!", "warning");
                    //}
                    if (txtLastName.Value == "" || txtFirstName.Value == "" || tIDNo.Value == "" || txtBirthday.Value == "")
                    {
                        alertMsg("Please fillup fields last name, first name, birthday and ID No.", "warning");
                    }
                    else if (!isBDateValid)
                    {
                        alertMsg("Birthday is not valid.", "warning");
                    }
                    //else if (tNatureofEmp.Value == "" && ddlBusinessType.SelectedValue != "Corporation")
                    //{
                    //    alertMsg("Please select nature of employment.", "warning");
                    //}
                    else if (tTypeOfId.Value == "" && !divCorp.Visible)
                    {
                        alertMsg("Please select type of ID.", "warning");
                    }
                    else
                    {
                        ViewState["DocNum"] = "";
                        if (string.IsNullOrEmpty(txtCardCode.Value))
                        {
                            lblID.Text = "New Buyer";
                        }
                        else { lblID.Text = txtCardCode.Value; }

                        lblDocNum.Text = "New Quotation";


                        ///IF EXIST MUST CHECK THE QUERY FOR IFExistBP 
                        DataTable dt = new DataTable();
                        dt = ws.IFExistBP(txtLastName.Value, txtFirstName.Value, txtMiddleName.Value, txtCompanyName.Value, tIDNo.Value).Tables["IFExistBP"];

                        if (DataAccess.Exist(dt) == true)
                        { lblID.Text = (string)DataAccess.GetData(dt, 0, "CardCode", ""); }
                        else
                        { lblID.Text = "New Buyer"; }

                        lblBusinessType.Text = ddlBusinessType.Text;
                        lblTIN.Text = txtTinNumber.Value;

                        //lblComaker.Text = lblBusinessType.Text == "Corporation" ? txtComakerCorp.Value : txtComaker.Value;
                        lblComaker.Text = txtComaker.Value;
                        lblLastName.Text = txtLastName.Value;
                        lblFirstName.Text = txtFirstName.Value;
                        lblMiddleName.Text = txtMiddleName.Value;
                        lblCompanyName.Text = txtCompanyName.Value;
                        lblTIN.Text = tin;



                        if ((string)ViewState["tNatureofEmp"] == "OTH")
                        { ViewState["tNatureofEmp"] = tNatureofEmp.Value; }

                        lblBirthday.Text = txtBirthday.Value;
                        lblNatureofEmployment.Text = tNatureofEmp.Value;

                        //2023-05-05 : REQUESTED BY DHEZA
                        lblTaxClassification.Text = ddTaxClass.Text;

                        lblTypeofID.Text = tTypeOfId.Value;
                        lblIDNo.Text = tIDNo.Value;
                        string MidName = txtMiddleName.Value;

                        if (!string.IsNullOrEmpty(MidName))
                        { MidName = txtMiddleName.Value[0].ToString(); }

                        if (ddlBusinessType.Text != "Corporation")
                        {
                            lblName.Text = $"{txtLastName.Value}, {txtFirstName.Value} {MidName}";
                        }
                        else
                        {
                            lblName.Text = txtCompanyName.Value;
                        }

                        //DataTable dtt = new DataTable();
                        //var query = $@"SELECT T1.""TransID"",T1.""SalesPerson"",T1.""Position"" FROM OCRD T0 INNER JOIN OSLA T1 ON T0.""SalesAgent"" = CAST(T1.""TransID"" AS VARCHAR(100)) WHERE T0.""CardCode"" = '{lblID.Text}'";
                        //dtt = hana.GetData(query, hana.GetConnection("SAOHana"));
                        //lblEmployeeID.Text = (string)DataAccess.GetData(dtt, 0, "TransID", "");
                        //lblEmployeeName.Text = (string)DataAccess.GetData(dtt, 0, "SalesPerson", "");
                        //lblEmployeePosition.Text = (string)DataAccess.GetData(dtt, 0, "Position", "");

                        //string userposition = Session["BrkPosition"].ToString();
                        //string userid = Session["UserID"].ToString();
                        loadInitialSalesAgent();

                        ClearBP();
                        ScriptManager.RegisterStartupScript(this, GetType(), "NewQuotationHide", "MsgNewQuotation_Hide();", true);
                    }
                }
                else
                {
                    alertMsg("Invalid TIN format. Must be 000-000-000-000", "info");
                }
            }

        }

        protected void bSelectHouseModel_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string Code = GetID.CommandArgument;

            tModel.Value = Code;
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgHouseModel", "MsgHouseModel_Hide();", true);

        }

        protected void gvHouseModelList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvHouseModelList.PageIndex = e.NewPageIndex;
            gvHouseModelList.DataSource = ws.GetHouseModelNew(hPrjCode.Value).Tables["GetHouseModelNew"];
            gvHouseModelList.DataBind();
        }


        void loadPaymentTermsData()
        {
            DataTable dt = new DataTable();
            string qry = $@"SELECT ""Code"", ""Name"" FROM ""OLST"" WHERE ""GrpCode"" = 'FS' AND ""Name"" = '{tFinancing.Value}'";
            dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
            string oFSCode = (string)DataAccess.GetData(dt, 0, "Code", "0");
        }

        protected void txtLTerms_TextChanged(object sender, EventArgs e)
        {
            Compute();
        }

        protected void btnLoanType_ServerClick(object sender, EventArgs e)
        {
            string qry = $@"SELECT ""Code"", ""Name"" FROM ""@LOANTYPE"" WHERE ""U_Active"" = 'Y'  {(lblBusinessType.Text == "Corporation" ? @" AND ""Code"" <> 'HDMF'" : "")} ";
            gvLoanType.DataSource = hana.GetData(qry, hana.GetConnection("SAPHana"));
            gvLoanType.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgLoanType", "MsgLoanType_Show();", true);
        }

        protected void gvLoanType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLoanType.PageIndex = e.NewPageIndex;
            string qry = @"SELECT ""Code"", ""Name"" FROM ""@LOANTYPE"" WHERE ""U_Active"" = 'Y'";
            gvLoanType.DataSource = hana.GetData(qry, hana.GetConnection("SAPHana"));
            gvLoanType.DataBind();
        }

        protected void bLoanType_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string Code = GetID.CommandArgument;
            txtLoanType.Value = Code;
            tBank.Value = "";
            hideTermDiv(Code);

            if (txtLoanType.Value != "BANK")
            {
                divBank.Visible = false;
                divBank2.Visible = false;
            }
            else
            {
                divBank.Visible = true;
                divBank2.Visible = true;
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "MsgLoanType", "MsgLoanType_Hide();", true);
        }

        void hideTermDiv(string Code)
        {
            string qry = $@"SELECT * FROM ""@LOANTYPE"" WHERE ""Code"" = '{Code}'";
            DataTable dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
            string oneLinertag = DataAccess.GetData(dt, 0, "U_OneLiner", "N").ToString();

            if (oneLinertag == "Y")
            {
                terms.Visible = false;
                interestRate.Visible = false;
                //dueDate.Visible = false;
                //divMonthly2.Visible = false;
                //dtpDueDate.Visible = false;
                //RequiredFieldValidator19.Enabled = false;
                //RequiredFieldValidator20.Enabled = false;
                txtLTerms.Text = "0";
                lblLBTerms.Text = txtLTerms.Text;

                txtFactorRate1.Text = "0";

                //RequiredFieldValidator21.Enabled = false;
                //RequiredFieldValidator22.Enabled = false;
            }
            else
            {
                terms.Visible = true;
                interestRate.Visible = true;
                //dueDate.Visible = true;
                //divMonthly2.Visible = true;
                //dtpDueDate.Visible = true;
                //RequiredFieldValidator19.Enabled = true;
                //RequiredFieldValidator20.Enabled = true;
                //RequiredFieldValidator21.Enabled = true;
                //RequiredFieldValidator22.Enabled = true;
            }
        }

        protected void btnBank_ServerClick(object sender, EventArgs e)
        {
            string qry = $@"SELECT DISTINCT B.""U_BankCode"" ""Code"",B.""U_BankName"" ""Name"" from ""@ACCREDBANKSH"" A LEFT JOIN ""@ACCREDBANKSR"" B ON A.""Code"" = B.""Code"" AND A.""Code"" = '{hPrjCode.Value}'
                            WHERE IFNULL(""U_ValidFr"",CURRENT_DATE) <= CURRENT_DATE AND IFNULL(""U_ValidTo"",CURRENT_DATE)>= CURRENT_DATE AND IFNULL(B.""Code"",'') <> '' ";
            gvBanks.DataSource = hana.GetData(qry, hana.GetConnection("SAPHana"));
            gvBanks.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgAccreditedBanks", "MsgAccreditedBanks_Show();", true);
        }

        protected void btnRetType_ServerClick(object sender, EventArgs e)
        {
            string qry = $@"Select 'ABCI' as ""Name"" from ""DUMMY"" union all select 'BUYERS' as ""Name"" from ""DUMMY""";
            gvRetType.DataSource = hana.GetData(qry, hana.GetConnection("SAPHana"));
            gvRetType.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgRetitlingType", "MsgRetitlingType_Show();", true);
        }
        protected void btnAdjLot_ServerClick(object sender, EventArgs e)
        {
            string qry = $@"Select 'Yes' as ""Name"" from ""DUMMY"" union all select 'No' as ""Name"" from ""DUMMY""";
            gvAdjLot.DataSource = hana.GetData(qry, hana.GetConnection("SAOHana"));
            gvAdjLot.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgAdjLot", "MsgAdjLot_Show();", true);
        }
        protected void bLotNo_ServerClick(object sender, EventArgs e)
        {
            loadAdjacentLots();
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgLotNo", "MsgLotNo_Show();", true);
        }

        void loadAdjacentLots()
        {
            string qry = $@"Select ""U_LotNo"" ""Lot"", ""U_BlockNo"" ""BlocK"" from ""OBTN"" where ""U_Project"" = '{hPrjCode.Value}' and ""U_BlockNo"" = '{tBlock.Value}' order by length(""U_LotNo""), ""U_LotNo"" ";
            gvLotNo.DataSource = hana.GetData(qry, hana.GetConnection("SAPHana"));
            gvLotNo.DataBind();
        }


        protected void gvBanks_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBanks.PageIndex = e.NewPageIndex;
            string qry = @"SELECT ""Code"", ""Name"" FROM ""@ACCREDITEDBANKSH""";
            gvBanks.DataSource = hana.GetData(qry, hana.GetConnection("SAPHana"));
            gvBanks.DataBind();
        }

        protected void bBanks_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string Code = GetID.CommandArgument;
            tBank.Value = Code;

            ScriptManager.RegisterStartupScript(this, GetType(), "MsgAccreditedBanks", "MsgAccreditedBanks_Hide();", true);
        }

        protected void bRetitlingType_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string Name = GetID.CommandArgument;
            tRetType.Value = Name;
            DataTable dt = ws.GetHouseDetails(hPrjCode.Value, tBlock.Value, tLot.Value, tFinancing.Value).Tables["GetHouseDetails"];
            if (DataAccess.Exist(dt) == true)
            {
                GetHouseDetails(dt);
            }

            //2023-05-05: TRIGGER WHEN RETITLING TYPE IS CHANGED
            txtMiscFees.Text = "0";
            MiscUpdate();

            //2023-10-19 : Change position ; moved after MIscUpdate();
            Compute(1);

            ScriptManager.RegisterStartupScript(this, GetType(), "MsgRetitlingType", "MsgRetitlingType_Hide();", true);
        }
        protected void bAdjLot_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string Name = GetID.CommandArgument;

            //--COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
            //txtAdjLot.Value = Name;

            //RequiredFieldValidator25.Enabled = Name == "No" ? false : true;
            bLotNo.Disabled = Name == "No" ? true : false;
            txtLotNo.Value = Name == "No" ? "" : txtLotNo.Value;
            Compute();
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgAdjLot", "MsgAdjLot_Hide();", true);
        }

        protected void bLotNo_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string Name = GetID.CommandArgument;
            txtLotNo.Value = Name;
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgLotNo", "MsgLotNo_Hide();", true);
        }

        protected void btnSelectAgent_Click(object sender, EventArgs e)
        {
            if (lblID.Text != "Sample Quotation")
            {
                LoadEmpList();
                ScriptManager.RegisterStartupScript(this, GetType(), "MsgEmpListShow", "MsgEmpList_Show();", true);
            }
            else
            {
                alertMsg("No sales agent needed for Sample Quotation.", "info");
            }
        }

        protected void gvSharingDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void ddlBusinessType_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadDivisionsForNames(ddlBusinessType.Text);
        }

        protected void tDocDate_TextChanged(object sender, EventArgs e)
        {

            // MISC DUE DATE
            lblDPDueDate.Text = Convert.ToDateTime(tDocDate.Text).AddMonths(1).ToString("yyyy-MM-dd");
            txtDPDueDate.Text = Convert.ToDateTime(tDocDate.Text).AddMonths(1).ToString("yyyy-MM-dd");
            tMiscDocDate.Text = tDocDate.Text;

            // DP DUE DATE
            lblLBDueDate.Text = Convert.ToDateTime(lblDPDueDate.Text).AddMonths(1).ToString("yyyy-MM-dd");

        }

        protected void btnCoMakerSearch_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtCoMakerSearch.Value))
            {
                string qrytemp = $@"select IFNULL(""LastName"",'') || ', ' || IFNULL(""FirstName"",'') || ' ' || IFNULL(""MiddleName"",'') ""Name""  from ""temp_CRD5"" 
                                    where ""UserID"" = '{Session["UserID"].ToString()}'
                                    and ""CoBorrower"" = true
                                    and (LOWER(""LastName"") like '%{txtCoMakerSearch.Value.ToLower()}%' 
                                     or LOWER(""FirstName"") like '%{txtCoMakerSearch.Value.ToLower()}%'
                                     or LOWER(""MiddleName"") like '%{txtCoMakerSearch.Value.ToLower()}%')";
                string qry = $@"select IFNULL(""LastName"",'') || ', ' || IFNULL(""FirstName"",'') || ' ' || IFNULL(""MiddleName"",'') ""Name""  from crd5 
                        where ""CoBorrower"" = true 
                        and ""CardCode"" = '{txtCardCode.Value}'
                        and (LOWER(""LastName"") like '%{txtCoMakerSearch.Value.ToLower()}%' 
                             or LOWER(""FirstName"") like '%{txtCoMakerSearch.Value.ToLower()}%'
                             or LOWER(""MiddleName"") like '%{txtCoMakerSearch.Value.ToLower()}%') union all ";
                DataTable dt = String.IsNullOrEmpty(txtCardCode.Value) ? hana.GetData(qrytemp, hana.GetConnection("SAOHana")) : hana.GetData(qry + qrytemp, hana.GetConnection("SAOHana"));
                gvCoMaker.DataSource = dt;
                gvCoMaker.DataBind();
            }
            else
            {
                loadCoMaker();
            }
        }

        protected void gvCoMaker_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCoMaker.PageIndex = e.NewPageIndex;
            loadCoMaker();
        }

        protected void gvCoOwner_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCoOwner.PageIndex = e.NewPageIndex;
            loadCoOwner1();
        }

        protected void gvOthers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvOthers.PageIndex = e.NewPageIndex;
            loadOthers();
        }

        protected void bSelectCoMaker_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string Name = GetID.CommandArgument;
            txtComaker.Value = Name;
            lblComaker.Text = Name;
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgCoMaker_Hide", "MsgCoMaker_Hide();", true);
        }

        protected void bSelectCoOwner_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string Name = GetID.CommandArgument;
            txtCoowner.Value = Name;
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgCoOwner_Hide", "MsgCoOwner_Hide();", true);
        }

        protected void bSelectOthers_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string Name = GetID.CommandArgument;
            txtOthers.Value = Name;
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgOthers_Hide", "MsgOthers_Hide();", true);
        }

        protected void btnCoMaker_ServerClick(object sender, EventArgs e)
        {
            loadCoMaker();
        }

        protected void btnCoOwner_ServerClick(object sender, EventArgs e)
        {
            //loadCoOwner();
        }

        void loadCoOwner1()
        {
            DataTable dt = (DataTable)ViewState["CoOwner"];
            gvCoOwner.DataSource = dt;
            gvCoOwner.DataBind();
        }

        void loadOthers()
        {
            string datatable = "";
            if (ddlBusinessType.SelectedValue == "Others")
            {
                datatable = "Others";

                //string qrytemp = $@"select ""CardCode"" ""Row"", IFNULL(""LastName"",'') || ', ' || IFNULL(""FirstName"",'') || ' ' || IFNULL(""MiddleName"",'') ""Name"", ""LastName"", ""FirstName"", ""MiddleName"", ""Relationship"" from ""CRD7"" where ""CardCode"" = '{txtCardCode.Value}'";
                string qrytemp = $@"Select 
	                                A.""SpecialBuyerLink"" as ""Row"",
                                    IFNULL(B.""LastName"",'') || ', ' || IFNULL(B.""FirstName"",'') || ' ' || IFNULL(B.""MiddleName"",'') ""Name"",
	                                B.""LastName"",
	                                B.""FirstName"",
	                                B.""MiddleName"",
	                                A.""BuyerType"" as ""SpecifiedBusinessType""
                                    from 
	                                    ""CRD7"" A INNER JOIN 
	                                    CRD1 B ON A.""SpecialBuyerLink"" = B.""CardCode""
                                    where 
	                                A.""CardCode"" = '{txtCardCode.Value}' AND 
	                                B.""CardType"" = 'Buyer' ";
                //string qry = $@"select ""LastName"" || ', ' || ""FirstName"" || ' ' || ""MiddleName"" ""Name""  from crd5 where ""CoBorrower"" = 'true' and ""CardCode"" = '{txtCardCode.Value}' union all ";
                DataTable dt = hana.GetData(qrytemp, hana.GetConnection("SAOHana"));
                DataTable dt1 = null;

                int count = 1;
                if (!string.IsNullOrWhiteSpace(datatable))
                {
                    dt1 = (DataTable)ViewState[datatable];
                    foreach (DataRow row in dt1.Rows)
                    {
                        row["Row"] = count.ToString();
                        count++;
                    }
                }

                foreach (DataRow row in dt.Rows)
                {
                    row["Row"] = count.ToString();
                    count++;
                }


                dt.Merge(dt1);

                gvOthers.DataSource = dt;
                gvOthers.DataBind();

            }
        }

        protected void gvLotNo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLotNo.PageIndex = e.NewPageIndex;
            loadAdjacentLots();
        }

        protected void btnAddCoborrower_ServerClick(object sender, EventArgs e)
        {
            int count = GetCountID("ID");
            Control btn = (Control)sender;
            string id = btn.ID;
            int UserID = (int)Session["UserID"];
            DataTable dt = new DataTable();

            if (id == "btnAddCoborrower")
            {
                //ADD
                if (ws.insert_temp__crd5(UserID,   //1
                                            count,  //2
                                            false,  //3
                                            true,  //4
                                            mtxtRelationship.Text, //5
                                            mtxtLName.Text,  //6
                                            mtxtFName.Text, //7
                                            mtxtMName.Text, //8
                                            "",  //9
                                            "", //10
                                            "", //11
                                            "", //12
                                            "",  //13
                                            "", //14
                                            "", //15
                                            "",  //16
                                            "",  //17
                                            "", //18
                                            "", //19
                                            "", //20
                                            "PH", //21 
                                            "PH", //22
                                            "", //23
                                            0, //24
                                            "",  //25
                                            "",  //26
                                            "",  //27
                                            0, //28
                                            "", //29
                                            "", //30
                                            "ES1",  //31
                                            "NE1", //32
                                            "Single",
                                            "") == true)   //33 

                {


                    dt = ws.select_temp_crd5(UserID).Tables["select_temp_crd5"];
                    ClearCoBorrower();
                    loadCoMaker();
                    ScriptManager.RegisterStartupScript(this, GetType(), "MsgCoMaker_Hide", "MsgCoBorrower_Hide();", true);
                }
            }
            else
            {
                //UPDATE
                string query = $@"UPDATE ""temp_CRD5"" SET
                                    ""Relationship"" = '{mtxtRelationship.Text}',
                                    ""LastName"" = '{mtxtLName.Text}',
                                    ""FirstName"" = '{mtxtFName.Text}',
                                    ""MiddleName"" = '{mtxtMName.Text}'
                                WHERE ""UserID"" = '{UserID}' AND ""ID"" = '{mtxtRow.Text}'";
                if (hana.Execute(query, hana.GetConnection("SAOHana")) == true)
                {
                    dt = ws.select_temp_crd5(UserID).Tables["select_temp_crd5"];
                    ClearCoBorrower();
                    loadCoMaker();
                    ScriptManager.RegisterStartupScript(this, GetType(), "MsgCoMaker_Hide", "MsgCoBorrower_Hide();", true);
                }
            }


        }









        protected void btnAddCoOwner_ServerClick(object sender, EventArgs e)
        {
            int count = 0;
            //if (ddlBusinessType.SelectedValue == "Co-ownership")
            //{
            //    datatable = "CoOwner";
            //    //foreach (DataRow row in gvCoOwner.Rows)
            //    //{
            //    //    if (row.RowState.ToString() != "Deleted")
            //    count++;
            //    //}
            //}
            //else if (ddlBusinessType.SelectedValue == "Others")
            //{
            //    datatable = "Others";
            //    foreach (DataRow row in gvOthers.Rows)
            //    {
            //        if (row.RowState.ToString() != "Deleted")
            //            count++;
            //    }
            //}

            Control btn = (Control)sender;
            string id = btn.ID;
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["CoOwner"];

            dt.Rows.Add(
                        count + 1
                        , $@"{tOwnerLName.Text}, {tOwnerFName.Text} {tOwnerMName.Text}"
                        , tOwnerLName.Text
                        , tOwnerFName.Text
                        , tOwnerMName.Text
                        , tOwnerRelationship.Text);

            ViewState["CoOwner"] = dt;

            ClearCoOwner();
            loadCoOwner1();
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgCoOwner_Hide", "MsgCoOwnermodal_Hide();", true);
        }
















        int GetCountID(string Type)
        {
            int ret = 0;
            try
            {
                DataTable dt = new DataTable();
                string param = "";

                if (Type == "LineNum")
                { param = $@"CRD2"" WHERE ""ID"" = {(int)ViewState["SPACoBorrowerCount"]} AND "; }
                else if (Type == "ID")
                { param = $@"CRD5"" WHERE "; }

                param = param + $@"""UserID"" = {(int)Session["UserID"]}";

                dt = hana.GetData($@"SELECT MAX(IFNULL(""{Type}"",0)) + 1 ""{Type}"" FROM ""temp_{param}", hana.GetConnection("SAOHana"));
                ret = int.Parse((string)DataAccess.GetData(dt, 0, Type, "0"));
            }
            catch (Exception ex)
            {
                if (Type == "LineNum")
                { ret = 0; }
                else if (Type == "ID")
                { ret = 1; }
            }
            return ret;
        }

        void ClearCoBorrower()
        {
            mtxtRow.Text = string.Empty;
            mtxtLName.Text = string.Empty;
            mtxtFName.Text = string.Empty;
            mtxtMName.Text = string.Empty;
            mtxtRelationship.Text = string.Empty;
        }

        void ClearCoOwner()
        {
            tCoOwnerRow.Text = string.Empty;
            tOwnerLName.Text = string.Empty;
            tOwnerFName.Text = string.Empty;
            tOwnerMName.Text = string.Empty;
            tOwnerRelationship.Text = string.Empty;
        }

        protected void CustomValidator14_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (ddlBusinessType.Text != "Guardianship" && ddlBusinessType.Text != "Trusteeship")
            {
                //Check if TIN is in correct format(###-###-###-###)
                bool isOK = Regex.IsMatch(tTIN1.Text, @"[0-9][0-9][0-9]\-[0-9][0-9][0-9]\-[0-9][0-9][0-9]\-[0-9][0-9][0-9]");
                if (!isOK)
                {
                    RequiredFieldValidator3.Visible = false;
                    args.IsValid = false;
                }
                else
                {
                    RequiredFieldValidator3.Visible = true;
                    args.IsValid = true;
                }
            }
        }

        protected void cvCoOwner_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (gvCoOwner.Rows.Count <= 0)
            {
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }
        }


        protected void bDeleteCoMaker_Click1(object sender, EventArgs e)
        {
            try
            {
                LinkButton GetID = (LinkButton)sender;
                string Name = GetID.CommandArgument;


                string qry = "";
                qry = $@"DELETE FROM ""temp_CRD5"" WHERE ""UserID"" = 
                    {Session["UserID"]} AND (IFNULL(""LastName"",'') || ', ' || IFNULL(""FirstName"",'') || ' ' || IFNULL(""MiddleName"",'') ) = '{Name}'";
                hana.Execute(qry, hana.GetConnection("SAOHana"));

                loadCoMaker();
                alertMsg("Co-borrower removed.", "info");
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "warning");
            }

        }

        protected void bDeleteOwner_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton GetID = (LinkButton)sender;
                string Code = GetID.CommandArgument;


                DataTable dt = new DataTable();
                DataTable dtNew1 = new DataTable();
                dt = (DataTable)ViewState["CoOwner"];
                dtNew1 = (DataTable)ViewState["CoOwner"];

                foreach (DataRow row in dt.Rows)
                {
                    if (row["Code"].ToString() == Code)
                    {
                        row.Delete();
                        break;
                    }

                    if (dtNew1.Rows.Count <= 0)
                    {
                        break;
                    }
                }

                ViewState["CoOwner"] = dtNew1;
                gvCoOwner.DataSource = dtNew1;
                gvCoOwner.DataBind();

                alertMsg("Co-owner removed.", "info");
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "warning");
            }
        }

        protected void btnIncentiveOption_ServerClick(object sender, EventArgs e)
        {
            string qry = $@"Select 'Yes' as ""Name"" from ""DUMMY"" union all select 'No' as ""Name"" from ""DUMMY""";
            gvIncentiveOption.DataSource = hana.GetData(qry, hana.GetConnection("SAOHana"));
            gvIncentiveOption.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgIncentive", "MsgIncentive_Show();", true);
        }

        protected void bIncentive_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string Name = GetID.CommandArgument;
            txtIncentiveOption.Value = Name;
            txtIncentiveOption2.Value = Name;
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgIncentive", "MsgIncentive_Hide();", true);
        }

        void SaveQuotation()
        {

            // 04-20-2023 : BLOCK WHEN NO RESERVATION FEE FOUND
            if (double.Parse(lblReserveFee.Text) > 0)
            {


                //04-20-2023 CHECK IF PRODUCT TYPE IS HOUSE AND LOT AND NO HOUSE MODEL
                double result2 = 0;
                if (txtProductType.Value.ToUpper() == "LOT ONLY")
                {
                    string model = string.IsNullOrWhiteSpace(tModel.Value) ? "-" : tModel.Value;
                    if (model != "-")
                    {
                        result2 = 1;
                    }
                }

                //04-20-2023 BLOCK IF PRODTYPE = HOUSE AND LOT AND NO HOUSE MODEL IN BATCH DETAILS
                if (result2 == 0)
                {

                    // 05-13-2023 CHECK IF NO SHARING DETAILS FOUND
                    DataTable dtIncentiveAgents = ws.GetIncentiveAgents(lblEmployeeID.Text, hPrjCode.Value, txtProductType.Value).Tables[0];
                    if (dtIncentiveAgents.Rows.Count > 0)
                    {
                        // 04-20-2023 : BLOCK POSTING WHEN PROJECT AND PRODUCT TYPE NOT FOUND ON COMMISSION/INCENTIVE SCHEME IN SAP 
                        string qryCommissionScheme = $@"SELECT * FROM ""@COMMINCSCHEME"" WHERE ""U_Project"" = '{hPrjCode.Value}' AND 
                                                    UPPER(""U_Type"") = UPPER('{txtProductType.Value}') ";
                        DataTable dtCommissionScheme = hana.GetData(qryCommissionScheme, hana.GetConnection("SAPHana"));
                        if (dtCommissionScheme.Rows.Count > 0)
                        {
                            int check = 0;
                            DataTable dtAdjacentLotPrice1 = ws.GetHouseDetails(hPrjCode.Value, tBlock.Value, tLot.Value, tFinancing.Value).Tables["GetHouseDetails"];
                            double HousePrice1 = Convert.ToDouble(DataAccess.GetData(dtAdjacentLotPrice1, 0, "HousePrice", "0").ToString());

                            //##########    BLOCKING WHEN THE SELECTED BLOCK AND LOT IS "HOUSE AND LOT" AND DOES NOT HAVE HOUSE PRICE ###########//
                            if (txtProductType.Value.ToUpper() == "HOUSE AND LOT" && HousePrice1 <= 0)
                            {
                                check = 1;
                            }

                            if (check == 0)
                            {

                                int UserID = (int)Session["UserID"];
                                if (lblID.Text == "New Buyer")
                                {
                                    lblID.Text = ws.LeadBusinessPartner(lblLastName.Text, lblFirstName.Text, lblMiddleName.Text, lblCompanyName.Text, DateTime.Parse(lblBirthday.Text),
                                      (string)ViewState["tNatureofEmp"], (string)ViewState["tTypeOfId"], lblIDNo.Text, (int)Session["UserID"], lblBusinessType.Text, lblTIN.Text, "", ddTaxClass.SelectedValue.ToString());

                                    DataTable dt = ws.select_temp_Listcrd5(UserID).Tables["select_temp_Listcrd5"];
                                    int i = 0;
                                    string ID = ws.GetAutoKey(1, "B");
                                    bool rets = false;
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        int LineID = int.Parse(dr["ID"].ToString());
                                        bool SPA1 = false;
                                        bool CoBorrower1 = false;
                                        if (dr["SPA"].ToString() == "1")
                                        {
                                            SPA1 = true;
                                            CoBorrower1 = false;
                                        }
                                        else if (dr["CoBorrower"].ToString() == "1")
                                        {
                                            SPA1 = false;
                                            CoBorrower1 = true;
                                        }
                                        rets = ws.BPSPACoBorrower(lblID.Text, ID, LineID, SPA1, CoBorrower1, dr["Relationship"].ToString(), dr["LastName"].ToString(), dr["FirstName"].ToString(), dr["MiddleName"].ToString(), dr["Gender"].ToString()
                                                        , dr["Citizenship"].ToString(), Convert.ToDateTime(dr["BirthDate"].ToString() == "" ? "0001-01-01" : dr["BirthDate"].ToString()).ToString("yyyy-MM-dd"), dr["BirthPlace"].ToString(), dr["HomeTelNo"].ToString(), dr["CellNo"].ToString(), dr["Email"].ToString(), dr["FB"].ToString(), dr["TIN"].ToString(), dr["SSSNo"].ToString(), dr["GSISNo"].ToString()
                                                        , dr["PagIbigNo"].ToString(), dr["PresentAddress"].ToString(), dr["PermanentAddress"].ToString(), dr["HomeOwnership"].ToString(), dr["YearsOfStay"].ToString(), dr["EmpBusinessName"].ToString(), dr["EmpBusinessAddress"].ToString()
                                                        , dr["Position"].ToString(), dr["YearsOfService"].ToString(), dr["OfficeTelNo"].ToString(), dr["FaxNo"].ToString()
                                                        , dr["EmploymentStatus"].ToString(), dr["NatureOfEmp"].ToString(), dr["CivilStatus"].ToString(), dr["SPAFormDocument"].ToString());
                                    }
                                    if (rets && (lblBusinessType.Text == "Guardianship" || lblBusinessType.Text == "Trusteeship"))
                                    {
                                        hana.Execute($@"DELETE FROM CRD7 WHERE LCASE(""CardCode"") = '{lblID.Text.ToLower()}'; ", hana.GetConnection("SAOHana"));

                                        string qryCRD7 = $@"INSERT INTO CRD7
                                    (
                                    ""CardCode"",
                                    ""BuyerType"",
                                    ""FirstName"",
                                    ""MiddleName"",
                                    ""LastName"",
                                    ""Relationship"",
                                    ""CreateDate"")
                                    VALUES(
                                    '{lblID.Text}'
                                    , '{lblBusinessType.Text}'
                                    , '{tspecFName.Value}'
                                    , '{tspecMName.Value}'
                                    , '{tspecLName.Value}'
                                    , '{tspecRelationship.Value}'
                                    , '{DateTime.Now.ToString("yyyy-MM-dd")}'
                                    )";

                                        rets = hana.Execute(qryCRD7, hana.GetConnection("SAOHana"));
                                    }
                                    //else if (rets && lblBusinessType.Text == "Co-ownership")
                                    //{
                                    //    DataTable dtcoowner = (DataTable)ViewState["CoOwner"];
                                    //    foreach (DataRow row in dtcoowner.Rows)
                                    //    {
                                    //        if (row.RowState.ToString() != "Deleted")
                                    //        {
                                    //            string qryCRD7 = $@"INSERT INTO CRD7
                                    //                    (
                                    //                    ""CardCode"",
                                    //                    ""BuyerType"",
                                    //                    ""FirstName"",
                                    //                    ""MiddleName"",
                                    //                    ""LastName"",
                                    //                    ""Relationship"",
                                    //                    ""CreateDate"")
                                    //                    VALUES(
                                    //                    '{lblID.Text}'
                                    //                    , '{lblBusinessType.Text}'
                                    //                    , '{row["FirstName"].ToString()}'
                                    //                    , '{row["LastName"].ToString()}'
                                    //                    , '{row["MiddleName"].ToString()}'
                                    //                    , '{row["Relationship"].ToString()}'
                                    //                    , '{DateTime.Now.ToString("yyyy-MM-dd")}'
                                    //                    );";

                                    //            rets = hana.Execute(qryCRD7, hana.GetConnection("SAOHana"));
                                    //        }
                                    //    }
                                    //}
                                    ws.InitializeSPA(UserID);
                                }



                                // 05-13-2023 : BLOCK WHEN BUYER IS NOT APPROVED
                                string qryApprovedBuyer = $@"SELECT ""CardCode"" FROM ""OCRD"" WHERE ""CardCode"" = '{lblID.Text}' 
                                                            AND IFNULL(""Approved"", '')  = 'Y'";
                                DataTable dtApprovedBuyer = hana.GetData(qryApprovedBuyer, hana.GetConnection("SAOHana"));


                                if (dtApprovedBuyer.Rows.Count > 0)
                                {


                                    string flrArea = "0";
                                    if (!string.IsNullOrEmpty(tFloorArea.Value))
                                    {
                                        if (tFloorArea.Value != "-")
                                        {
                                            flrArea = tFloorArea.Value;
                                        }
                                    }

                                    string qry = $@"SELECT ""U_LTSNo"" FROM OBTN WHERE ""U_Project"" = '{hPrjCode.Value}' AND ""U_BlockNo"" = '{tBlock.Value}' AND ""U_LotNo"" = '{tLot.Value}'";
                                    DataTable dtLTS = hana.GetDataDS(qry, hana.GetConnection("SAPHana")).Tables[0];
                                    string LTSNo = DataAccess.GetData(dtLTS, 0, "U_LTSNo", "").ToString();

                                    string qryPaymentScheme = $@"SELECT IFNULL(""U_PmtSchemeType"",'') ""U_PmtSchemeType"" FROM ""@FINANCINGSCHEME"" WHERE ""Code"" = '{tFinancing.Value}'";
                                    DataTable dtPaymentScheme = hana.GetDataDS(qryPaymentScheme, hana.GetConnection("SAPHana")).Tables[0];
                                    string PaymentScheme = DataAccess.GetData(dtPaymentScheme, 0, "U_PmtSchemeType", "").ToString();

                                    //CHECK IF VATABLE OR NOT
                                    DataTable dtVATThreshold = hana.GetData($@"SELECT ""U_ThresholdAmount"" FROM ""@PRODUCTTYPE"" WHERE UPPER(""Code"") = UPPER('{txtProductType.Value}')", hana.GetConnection("SAPHana"));
                                    double vatThreshold = Convert.ToDouble(DataHelper.DataTableRet(dtVATThreshold, 0, "U_ThresholdAmount", "0"));
                                    double adjacentLotPrice = 0;
                                    string vatable = "";
                                    if (txtAdjLot.Value == "Yes")
                                    {
                                        DataTable dtAdjacentLotPrice = ws.GetHouseDetails(hPrjCode.Value, tBlock.Value, txtLotNo.Value, tFinancing.Value).Tables["GetHouseDetails"];
                                        double LandPrice = Convert.ToDouble(DataAccess.GetData(dtAdjacentLotPrice, 0, "LandPrice", "0").ToString());
                                        double HousePrice = Convert.ToDouble(DataAccess.GetData(dtAdjacentLotPrice, 0, "HousePrice", "0").ToString());
                                        adjacentLotPrice = LandPrice + HousePrice;
                                    }
                                    double TotalPrice = Convert.ToDouble(lblNetDAS.Text) + adjacentLotPrice;
                                    vatable = (TotalPrice > vatThreshold ? "Y" : "N");


                                    string ret = ws.QuotationNew(
                                                       4,
                                                       (int)ViewState["QuoteDocEntry"],  //2
                                                       DataAccess.GetViewState(lblID.Text, ""),
                                                       Convert.ToDateTime(tDocDate.Text), //4
                                                       "O",

                                                       hPrjCode.Value, //6
                                                       tBlock.Value,
                                                       tLot.Value, //8
                                                       tModel.Value,
                                                       tFinancing.Value, //10
                                                       tLotArea.Value,
                                                       flrArea, //12
                                                       tHouseStatus.Value,
                                                       tPhase.Value, //14
                                                       txtLotClassification.Value,
                                                       txtProductType.Value, //16
                                                       txtLoanType.Value,
                                                       tBank.Value, //18

                                                       double.Parse(lblDASAmt.Text),
                                                       double.Parse(tResrvFee.Text), //20
                                                       double.Parse(tDPPercent.Text),
                                                       double.Parse(tDPAmount.Text), //22
                                                       int.Parse(txtDPTerms.Text),

                                                       double.Parse(txtDiscPercent.Text), //24
                                                       double.Parse(tSpotDPDiscAmt.Text),
                                                       int.Parse(txtLTerms.Text), //26 
                                                       double.Parse(txtFactorRate1.Text),
                                                       Convert.ToDateTime(lblLBDueDate.Text),  //28
                                                       double.Parse(txtGDI.Value),

                                                       double.Parse(lblDASAmt.Text),  //30
                                                       double.Parse(lblAddMiscCharges.Text),
                                                       double.Parse(lblNetDAS.Text),  //32
                                                       double.Parse(lblVAT.Text),
                                                       double.Parse(lblNetTCP.Text),//34
                                                       double.Parse(lblDownPayment.Text),
                                                       //double.Parse(lblMonthly1.Text),    //36
                                                       double.Parse(lblTCPMonthly.Text),    //36
                                                       Convert.ToDateTime(lblDPDueDate.Text),
                                                       double.Parse(lblAmountDue.Text), //38
                                                       (int)Session["UserID"],


                                                       lblEmployeeID.Text,    //40
                                                       lblEmployeeName.Text,
                                                       lblEmployeePosition.Text,  //42
                                                       (DataTable)ViewState["gvDownPayment"],
                                                       (DataTable)ViewState["gvAmortization"], //44
                                                       ViewState["DocNum"].ToString(),
                                                       double.Parse(txtMiscMonthly.Text), //46

                                                       double.Parse(lblTCPMonthly.Text),
                                                       double.Parse(lblMiscDPMonthly.Text), //48
                                                       double.Parse(lblAddMiscFees.Text),
                                                       double.Parse(tPDBalance.Text), //50
                                                       double.Parse(lblBalance.Text),
                                                       double.Parse(txtMiscFees.Text), //52
                                                       double.Parse(txtLoanableBalance.Text),
                                                       double.Parse(lblLoanableBalance.Text), //54
                                                       tRemarks.Value,
                                                       tRetType.Value, //56
                                                       double.Parse(lblCompTotal.Text),

                                                       txtAdjLot.Value, //58
                                                       txtLotNo.Value,

                                                       //RELEATED TO MISCELLANEOUS ADJUSTMENTS
                                                       Convert.ToDateTime(lblMiscDueDate.Text),
                                                       txtFinancingMisc.Value,
                                                       txtMiscTerms.Text,
                                                       (DataTable)ViewState["gvMiscellaneous"],
                                                       double.Parse(lblMiscDPAmount.Text),
                                                       int.Parse(lblMiscLBTerms.Text),
                                                       double.Parse(lblMiscLBAmount.Text),
                                                       double.Parse(lblMiscLBMonthly.Text),
                                                       (DataTable)ViewState["gvMiscellaneousAmort"],

                                                       //ddTaxClass.SelectedValue.ToString(),
                                                       string.Empty, //60
                                                       LTSNo,
                                                       PaymentScheme, //62
                                                       vatable,
                                                       Convert.ToDouble(lblLBMonthly.Text), //64
                                                       string.IsNullOrEmpty(txtIncentiveOption.Value) ? "N" : (txtIncentiveOption.Value == "Yes" ? "Y" : "N"),
                                                       (DataTable)ViewState["CoOwner"], //44
                                                       lblComaker.Text
                                                       );


                                    if (ret != "Operation completed successfully.")
                                    { ws.SQDeleteLeads(lblID.Text); alertMsg(ret, "error"); }
                                    else
                                    {
                                        lblFinish.InnerText = "Finish";
                                        alertMsg(ret, "success");
                                        Clear();
                                        ClearAll("Project");
                                        clearPage();
                                        PrevTab("HouseDetails");
                                        ViewState["UpdateResFee"] = 0;
                                        ScriptManager.RegisterStartupScript(this, GetType(), "clear", "ResetTextBox()", true);

                                    }
                                }
                                else
                                {
                                    alertMsg("Buyer is not approved. Please contact administrator.", "info");
                                }
                            }
                            else
                            {
                                alertMsg("No House Price provided in SAP. Please contact administrator.", "info");
                            }
                        }
                        else
                        {
                            alertMsg("No Commission/Incentive scheme found. Please contact administrator.", "info");
                        }

                    }
                    else
                    {
                        alertMsg("No sharing details found for the selected agent. Please contact administrator.", "info");
                    }

                }
                else
                {
                    alertMsg("Lot only product type should not have house model. Please contact administrator.", "info");
                }




            }
            else
            {
                alertMsg("No Reservation Fee found. Please contact administrator.", "info");
            }

        }










        protected void btnWaive_Click(object sender, EventArgs e)
        {
            try
            {
                //Email notification for waive approval
                int randomvalue;
                using (RNGCryptoServiceProvider rg = new RNGCryptoServiceProvider())
                {
                    byte[] rno = new byte[5];
                    rg.GetBytes(rno);
                    randomvalue = BitConverter.ToInt32(rno, 0);
                }
                var ticks = DateTime.Now.Ticks.ToString();
                string otp = Math.Abs(randomvalue).ToString().Substring(0, 5);
                //Random rand = new Random(100);                
                //string emailaddress = "direc.olegario@gmail.com";
                string qry = $@"SELECT * FROM ""@EMAIL"" WHERE ""Code"" = 'QTNWVE'";
                DataTable dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
                ws.sendEmail(otp,
                                        //ConfigSettings.EmailSubjectApprovalOnGoing"].ToString() + DateTime.Now.ToString("yyyy-MM-dd"),
                                        DataAccess.GetData(dt, 0, "U_Subject", "").ToString(),
                                        DataAccess.GetData(dt, 0, "U_OTPRecipient", "").ToString(),
                                        //ConfigSettings.EmailBodySubjectApprovalOnGoing"].ToString(),
                                        DataAccess.GetData(dt, 0, "U_Body", "").ToString(),
                                        "",
                                        "",
                                        "OTP",
                                        DataAccess.GetData(dt, 0, "Code", "").ToString());

                hana.Execute($@"UPDATE ""@EMAIL"" SET ""U_AlertExp"" = '{otp}' WHERE ""Code"" = 'QTNWVE';", hana.GetConnection("SAPHana"));
                ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "showmodalOTP();", true);
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }

        }

        protected void btnConfirmOTP_Click(object sender, EventArgs e)
        {
            string qry = $@"SELECT * FROM ""@EMAIL"" WHERE ""Code"" = 'QTNWVE'";
            DataTable dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
            string otp = DataAccess.GetData(dt, 0, "U_AlertExp", "").ToString();
            if (txtOTP.Text == DataAccess.GetData(dt, 0, "U_AlertExp", "").ToString())
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "hideMsgConfirm();", true);


                // IF RESERVATION HAS ALREADY BEEN PAID
                string qryExistingBlockAndLot = $@"SELECT A.""DocEntry"" FROM OQUT A LEFT JOIN QUT1 B ON A.""DocEntry"" = B.""DocEntry"" WHERE A.""ProjCode"" = '{hPrjCode.Value}' 
                                    AND A.""Block"" = '{tBlock.Value}' AND	A.""Lot"" = '{tLot.Value}' AND	B.""PaymentType"" = 'RES' AND	IFNULL(B.""AmountPaid"",0) >= B.""PaymentAmount""
                                    AND B.""LineStatus"" <> 'R'";

                DataTable dtExistingBlockAndLot = hana.GetDataDS(qryExistingBlockAndLot, hana.GetConnection("SAOHana")).Tables[0];

                if (dtExistingBlockAndLot.Rows.Count == 0)
                {

                    string qryExistingBPBlockANdLot = $@"SELECT A.""DocEntry"" FROM OQUT A WHERE A.""ProjCode"" =    '{hPrjCode.Value}' AND 
                                                A.""Block"" = '{tBlock.Value}' AND	A.""Lot"" = '{tLot.Value}' AND A.""CardCode"" = '{lblID.Text}' AND A.""DocStatus"" NOT IN ('F','R')";

                    DataTable dtExistingBPBlockANdLot = hana.GetDataDS(qryExistingBPBlockANdLot, hana.GetConnection("SAOHana")).Tables[0];

                    if (!string.IsNullOrEmpty(ViewState["DocNum"].ToString()))
                    {
                        SaveQuotation();
                    }
                    else
                    {
                        if (dtExistingBPBlockANdLot.Rows.Count == 0)
                        {
                            SaveQuotation();
                        }
                        else
                        {
                            alertMsg("Quotation with the same block & lot already exists under this customer. Please contact administrator.", "warning");
                        }

                    }
                }
                else
                {
                    alertMsg("Selected house & lot is already sold. Please contact administrator.", "warning");
                }
            }
            else
            {
                alertMsg("The OTP you entered is incorrect.", "warning");
            }
        }

        protected void ddTaxClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            taxClassChanged();
        }

        void taxClassChanged()
        {
            //2023-05-16 : REMOVE CONDITION AS REQUESTED BY DHEZA -- CANNOT PROCEED ON CREATION OF QUOTATION
            //if (ddTaxClass.SelectedValue != "Not engaged in Business")
            //{
            //    divEmp.Visible = true;
            //    RequiredFieldValidator13.Enabled = true;
            //}
            //else
            //{
            //    divEmp.Visible = false;
            //    RequiredFieldValidator13.Enabled = false;
            //}
        }

        protected void tMiscDocDate_TextChanged(object sender, EventArgs e)
        {

        }

        protected void gvMiscellaneous_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMiscellaneous.PageIndex = e.NewPageIndex;
            LoadData(gvMiscellaneous, "gvMiscellaneous");
        }

        protected void gvMiscellaneousAmort_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMiscellaneousAmort.PageIndex = e.NewPageIndex;
            LoadData(gvMiscellaneousAmort, "gvMiscellaneousAmort");
        }

        protected void btnLoadResFee_ServerClick(object sender, EventArgs e)
        {
            ReservationUpdate();
        }

        private void ReservationUpdate()
        {
            if (Convert.ToInt32(ViewState["UpdateResFee"]) == 1)
            {
                if (double.Parse(string.IsNullOrWhiteSpace(tResrvFee.Text) ? "0" : tResrvFee.Text) == 0)
                {
                    ViewState["UpdateResFee"] = 0;
                }
            }
            else
            {
                if (double.Parse(string.IsNullOrWhiteSpace(tResrvFee.Text) ? "0" : tResrvFee.Text) > 0)
                {
                    ViewState["UpdateResFee"] = 1;
                }
                else
                {
                    ViewState["UpdateResFee"] = 0;
                }
            }


            tResrvFee.Text = SystemClass.ToCurrency(tResrvFee.Text);
            DataTable dt = ws.GetHouseDetails(hPrjCode.Value, tBlock.Value, tLot.Value, tFinancing.Value).Tables["GetHouseDetails"];
            if (DataAccess.Exist(dt) == true)
            {
                GetHouseDetails(dt);
            }
            Compute();

        }

        protected void btnLoadMiscFee_ServerClick(object sender, EventArgs e)
        {
            MiscUpdate();
        }


        private void MiscUpdate()
        {
            //if (Convert.ToInt32(ViewState["UpdateMiscFee"]) == 1)
            //{
            //    //WHEN UPDATING THE FIELD
            //    //CHECK IF RESERVATION FEE IS 0, REMOVE UPDATERESERVATION TAGGING TO 0
            //    if (double.Parse(txtMiscFees.Text) == 0)
            //    {
            //        ViewState["UpdateMiscFee"] = 0;
            //    }
            //}
            //else
            //{
            //    //WHEN THE FIELD IS NOT UPDATED
            //    if (double.Parse(txtMiscFees.Text) > 0)
            //    {
            //        ViewState["UpdateMiscFee"] = 1;
            //    }
            //    else
            //    {
            //        ViewState["UpdateMiscFee"] = 0;
            //    }

            //} 

            Compute();
            //GET MISCELLANEOUS DATA REQUIRED
            DataTable dt = ws.GetHouseDetails(hPrjCode.Value, tBlock.Value, tLot.Value, txtFinancingMisc.Value).Tables["GetHouseDetails"];
            if (DataAccess.Exist(dt) == true)
            {
                GetMiscDetails(dt);
            }
            Compute();
        }

        protected void txtDPDueDate_TextChanged(object sender, EventArgs e)
        {
            // Get the entered date string
            string dateString = txtDPDueDate.Text;

            // Try to parse the date string into a DateTime object
            DateTime date;
            if (DateTime.TryParse(dateString, out date))
            {
                // The entered text is a valid date, do something with it
            }
            else
            {
                // The entered text is not a valid date, ignore it
            }

        }

        protected void CustomValidator14_ServerValidate1(object source, ServerValidateEventArgs args)
        {
            if (ddlBusinessType.Text != "Guardianship" && ddlBusinessType.Text != "Trusteeship")
            {
                //Check if TIN is in correct format(###-###-###-###)
                bool isOK = Regex.IsMatch(tTIN1.Text, @"[0-9][0-9][0-9]\-[0-9][0-9][0-9]\-[0-9][0-9][0-9]\-[0-9][0-9][0-9]");
                if (!isOK)
                {
                    RequiredFieldValidator3.Visible = false;
                    args.IsValid = false;
                }
                else
                {
                    RequiredFieldValidator3.Visible = true;
                    args.IsValid = true;
                }
            }
        }

        void confirmation(string body, string type)
        {
            ViewState["ConfirmType"] = type;
            lblConfirmationInfo.Text = body;
            btnYes.Focus();
            //Show Modal
            ScriptManager.RegisterStartupScript(this, this.GetType(), "confirmation", "showConfirmation();", true);
        }

        protected void btnYes_Click(object sender, EventArgs e)
        {

        }

        protected void btnSearchProject_Click(object sender, EventArgs e)
        {
            //2023-06-05 : ADD SEARCH FUNCTION FOR PROJECT
            if (!string.IsNullOrEmpty(txtSearchProject.Value))
            {
                ViewState["dtProjectList"] = ws.GetProjectsSearch(txtSearchProject.Value).Tables["Projects"];
                LoadData(gvProjectList, "dtProjectList");
            }
            else
            {
                DataTable dt = new DataTable();
                dt = ws.GetProjects().Tables["Projects"];
                ViewState["dtProjectList"] = dt;
                LoadData(gvProjectList, "dtProjectList");
            }
        }
    }
}
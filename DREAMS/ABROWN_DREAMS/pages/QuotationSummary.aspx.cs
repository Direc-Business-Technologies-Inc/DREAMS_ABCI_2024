using System;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABROWN_DREAMS
{
    public partial class QuotationSummary : Page
    {
        DirecWebService ws = new DirecWebService();
        DirecService wcf = new DirecService();
        SAPHanaAccess hana = new SAPHanaAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["ReportType"] = "";
                Session["dtPayments"] = null;
                LoadQuotationList();
                LoadBanks();
                LoadCreditMethod();

                if ((DataTable)Session["dtPayments"] == null)
                { LoadGridView("dtPayments"); }
                else { LoadData(gvPayments, "dtPayments"); }

                if ((DataTable)Session["dtChecks"] == null)
                { LoadGridView("dtChecks"); }
                else { LoadData(gvChecks, "dtChecks"); }

                //btnSchedule.Disabled = true;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "showQuotation();", true);
            }
        }

        void LoadReports()
        {
            gvReports.DataSource = ws.GetReportPerGroup("OQUT", lblLoi.Text);
            gvReports.DataBind();

        }


        void LoadData(GridView gv, string session)
        {
            gv.DataSource = (DataTable)Session[session];
            gv.DataBind();
        }
        void LoadBanks()
        {
            gvBanks.DataSource = ws.GetBanks();
            gvBanks.DataBind();
        }
        void LoadAccounts()
        {
            if (gvAccounts.Rows.Count == 0)
            {
                Session["GLAccounts"] = ws.GetGLAccounts();
                gvAccounts.DataSource = (DataSet)Session["GLAccounts"];
                gvAccounts.DataBind();
            }
        }
        void LoadBranch(string BankCode)
        {
            gvBranch.DataSource = ws.GetBranch(BankCode);
            gvBranch.DataBind();
        }
        void LoadQuotationList()
        {
            Session["QuotationList"] = ws.GetQuotationList();
            gvQuotationList.DataSource = (DataSet)Session["QuotationList"];
            gvQuotationList.DataBind();

        }


        void LoadCreditMethod()
        {
            gvPymtMethod.DataSource = ws.GetCreditPaymentMethod();
            gvPymtMethod.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (gvPayments.Rows.Count > 0)
            {
                if (double.Parse(lblAmountDue.Text) > double.Parse(lblAmount.Text))
                {
                    alertMsg("Unable to process over payment", "warning");
                }
                else
                {
                    confirmation("Are you sure you want to save payments?", "payment");
                }
            }
            else
            {
                alertMsg("Please enter payments", "warning");
            }
        }
        protected void btnCancel_ServerClick(object sender, EventArgs e)
        {
            confirmation("Are you sure want to cancel?Unsaved data will be lost.", "cancel");
            //Clear();
        }
        protected void bEmploymentStatus_ServerClick(object sender, EventArgs e)
        {

        }
        protected void gvDownPayment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDownPayment.DataSource = ws.GetPaymentSchedule(int.Parse(Session["QuotationID"].ToString()), "DP");
            gvDownPayment.PageIndex = e.NewPageIndex;
            gvDownPayment.DataBind();
        }

        protected void gvAmortization_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gvAmortization.DataSource = ws.GetPaymentSchedule(int.Parse(Session["QuotationID"].ToString()), "MA");
            gvAmortization.DataSource = ws.GetPaymentSchedule(Convert.ToInt32(Session["DocEntry"]), "LB");
            gvAmortization.PageIndex = e.NewPageIndex;
            gvAmortization.DataBind();
        }

        void getBrokerName(string Id)
        {
            //2023-11-07 : ADDED BROKER NAME
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
            lblBrokerName.Text = DataAccess.GetData(dt, 0, "Name", "").ToString();
        }



        void ReloadData(DataTable d)
        {
            //CHECKING OF LOI
            DataTable dtLTS = new DataTable();
            //dtLTS = hana.GetDataDS($@"SELECT ""U_LTSIssueDate"" FROM ""OPRJ"" WHERE ""PrjCode"" = '{d.Rows[0]["ProjCode"].ToString()}'", hana.GetConnection("SAPHana")).Tables[0];
            //DateTime LTS = Convert.ToDateTime(DataAccess.GetData(dtLTS, 0, "U_LTSIssueDate", DateTime.MinValue.ToString()));
            //DateTime DocDate = Convert.ToDateTime(d.Rows[0]["DocDate"]);

            //if (DocDate > LTS)
            //{
            //    lblLoi.Text = "YES";
            //}
            //else
            //{
            //    lblLoi.Text = "NO";
            //}




            Session["CardCode"] = d.Rows[0]["CardCode"].ToString();
            lblQuotationNum.Text = d.Rows[0]["DocNum"].ToString();
            lblDocEntry.Text = d.Rows[0]["DocEntry"].ToString();
            Session["QuotationID"] = d.Rows[0]["DocEntry"].ToString();
            //** customer details **
            lblName.Text = d.Rows[0]["Name"].ToString();
            lblID.Text = d.Rows[0]["CardCode"].ToString();

            //2023-05-16 : REQUESTED TO BE ADDED BY DHEZA
            lblDocDate.Text = Convert.ToDateTime(d.Rows[0]["DocDate"]).ToString("MMM dd, yyyy");
            lblSalesAgent.Text = d.Rows[0]["EmpName"].ToString();

            //2023-11-07 : ADD BROKER NAME
            getBrokerName(d.Rows[0]["EmpCode"].ToString());
            lblBrokerBusinessName.Text = d.Rows[0]["BrokerBusinessName"].ToString();

            lblModel.Text = d.Rows[0]["Model"].ToString();
            txtFName.Text = d.Rows[0]["FirstName"].ToString();
            txtLName.Text = d.Rows[0]["LastName"].ToString();
            //** project details **
            txtProjId.Text = d.Rows[0]["ProjCode"].ToString();
            txtBlock.Text = d.Rows[0]["Block"].ToString();
            txtLot.Text = d.Rows[0]["Lot"].ToString();
            txtLotArea.Text = d.Rows[0]["LotArea"].ToString();
            txtFloorArea.Text = d.Rows[0]["FloorArea"].ToString();
            txtFinScheme.Text = d.Rows[0]["FinancingScheme"].ToString();
            //txtAcctType.Text = d.Rows[0]["AcctType"].ToString();
            //txtSalesType.Text = d.Rows[0]["SalesType"].ToString();
            txtBookStatus.Text = d.Rows[0]["BookStatus"].ToString();

            //** price details **
            lblAmount.Text = SystemClass.ToCurrency(d.Rows[0]["ReservationFee"].ToString());
            reservationbalance.Text = SystemClass.ToCurrency(d.Rows[0]["ReservationBalance"].ToString());
            Session["reservation"] = d.Rows[0]["ReservationFee"].ToString();
            txtDAS.Text = SystemClass.ToCurrency(d.Rows[0]["DAS"].ToString());
            //txtMisc.Text = SystemClass.ToCurrency(d.Rows[0]["Misc"].ToString());
            txtMisc.Text = SystemClass.ToCurrency(d.Rows[0]["AddMiscFees"].ToString());
            //txtVat.Text = SystemClass.ToCurrency(d.Rows[0]["Vat"].ToString());
            //txtNetTCP.Text = SystemClass.ToCurrency(d.Rows[0]["NetTCP"].ToString());
            txtNetTCP.Text = SystemClass.ToCurrency(d.Rows[0]["NetTCP_new"].ToString());
            txtDiscount.Text = SystemClass.ToCurrency(d.Rows[0]["DiscAmount"].ToString());
            txtDiscountPercent.Text = Math.Round(Convert.ToDouble(d.Rows[0]["DiscPercent"])).ToString() + "%";
            //txtDiscountPercent.Text = Math.Round(Convert.ToDouble(d.Rows[0]["SpotDPPercent"])).ToString()+ "%";

            //**** Downpayment
            txtDPPercent.Text = d.Rows[0]["DPPercent"].ToString() + "%";
            txtDPAmount.Text = SystemClass.ToCurrency(d.Rows[0]["TCPDownpayment"].ToString());
            //txtFrstDP.Text = SystemClass.ToCurrency(d.Rows[0]["FirstDP"].ToString());
            txtRsvFee.Text = SystemClass.ToCurrency(d.Rows[0]["ReservationFee"].ToString());
            txtNetDP.Text = SystemClass.ToCurrency(Convert.ToString(Convert.ToDouble(d.Rows[0]["TCPDownpayment"]) - Convert.ToDouble(d.Rows[0]["ReservationFee"])));
            //txtNetDP.Text = SystemClass.ToCurrency(d.Rows[0]["NetDP"].ToString());
            txtDPTerms.Text = d.Rows[0]["DPTerms"].ToString();
            txtDPDueDate.Text = Convert.ToDateTime(d.Rows[0]["DPDueDate"].ToString()).ToString("dd-MMM-yyyy");
            txtMonthlyDP.Text = SystemClass.ToCurrency(d.Rows[0]["TCPMonthly"].ToString());
            //**** Loanable
            txtLPercent.Text = d.Rows[0]["LPercent"].ToString() + "%";
            //txtLAmount.Text = SystemClass.ToCurrency(d.Rows[0]["LAmount"].ToString());
            txtLAmount.Text = SystemClass.ToCurrency(d.Rows[0]["TCPLoanableBalance"].ToString());

            if (d.Rows[0]["LTerms"].ToString() == "0")
            {
                if (txtFinScheme.Text.ToUpper() == "SPOTCASH")
                {
                    txtLTerms.Text = "0";
                }
                else
                {
                    txtLTerms.Text = "1";
                }
                txtMonthlyAmort.Text = txtLAmount.Text;
            }
            else
            {
                txtLTerms.Text = d.Rows[0]["LTerms"].ToString();
                txtMonthlyAmort.Text = SystemClass.ToCurrency(d.Rows[0]["MonthlyLB"].ToString());
                //txtMonthlyAmort.Text = SystemClass.ToCurrency(d.Rows[0]["LAmount"].ToString());
            }
            txtRate.Text = d.Rows[0]["InterestRate"].ToString();





            lblLoanType.Text = d.Rows[0]["LoanType"].ToString();
            if (lblLoanType.Text.ToUpper() == "BANK")
            {
                trBank.Visible = true;
            }
            else
            {
                trBank.Visible = false;
            }
            lblBank.Text = d.Rows[0]["Bank"].ToString();



            lblRetitlingType.Text = d.Rows[0]["RetitlingType"].ToString();

            //2023-05-05 : REQUESTED BY DHEZA
            lblTaxClassification.Text = d.Rows[0]["TaxClassification"].ToString();

            lblAdjacentLot.Text = d.Rows[0]["AdjacentLotQuotationNo"].ToString() == "0" ? "N/A" : d.Rows[0]["AdjacentLotQuotationNo"].ToString();

            lblLoi.Text = d.Rows[0]["LOI"].ToString();
            lblLTSNo.Text = d.Rows[0]["LTSNo"].ToString();

            lblCoBorrower.Text = d.Rows[0]["Comaker1"].ToString();


            //2023 -- MISCELLANEOUS TABLE
            lblMiscFinancingScheme.Text = d.Rows[0]["MiscFinancingScheme"].ToString();
            lblMiscDueDate.Text = Convert.ToDateTime(d.Rows[0]["MiscDueDate"]).ToString("dd-MMM-yyyy");
            lblMiscFees.Text = SystemClass.ToCurrency(d.Rows[0]["AddMiscFees"].ToString());
            lblMiscDPAmount.Text = SystemClass.ToCurrency(d.Rows[0]["MiscDPAmount"].ToString());
            lblMiscDPMonthly.Text = SystemClass.ToCurrency(d.Rows[0]["MiscFeesMonthly"].ToString());
            lblMiscDPTerms.Text = d.Rows[0]["MiscDPTerms"].ToString();
            lblMiscLBAmount.Text = SystemClass.ToCurrency(d.Rows[0]["MiscLBAmount"].ToString());
            lblMiscLBMonthly.Text = SystemClass.ToCurrency(d.Rows[0]["MiscLBMonthly"].ToString());
            lblMiscLBTerms.Text = d.Rows[0]["MiscLBTerms"].ToString();



            //** additional details **//
            Session["Model"] = d.Rows[0]["Model"].ToString();
            Session["Size"] = d.Rows[0]["Size"].ToString();
            Session["Feature"] = d.Rows[0]["Feature"].ToString();
            Session["Model"] = d.Rows[0]["Model"].ToString();
            Session["SalesType"] = d.Rows[0]["SalesType"].ToString();
            Session["AddonCardCode"] = d.Rows[0]["CardCode"].ToString();
            Session["Discount"] = d.Rows[0]["TotalDisc"].ToString();
            Session["RsvDate"] = d.Rows[0]["DocDate"].ToString();
        }

        void Clear()
        {
            //2023-05-16 : REQUESTED TO BE ADDED BY DHEZA
            lblDocDate.Text = string.Empty;
            lblSalesAgent.Text = string.Empty;

            lblName.Text = string.Empty;
            lblID.Text = string.Empty;
            lblQuotationNum.Text = string.Empty;
            lblAmountDue.Text = "0.00";
            //** customer details **
            txtFName.Text = string.Empty;
            txtLName.Text = string.Empty;
            //** project details **
            txtProjId.Text = string.Empty;
            txtBlock.Text = string.Empty;
            txtLot.Text = string.Empty;
            txtLotArea.Text = string.Empty;
            txtFloorArea.Text = string.Empty;
            txtFinScheme.Text = string.Empty;
            //txtSalesType.Text = string.Empty;
            //txtAcctType.Text = string.Empty;
            txtBookStatus.Text = string.Empty;
            //** payment details **
            reservationbalance.Text = "0.00";
            txtDAS.Text = "0.00";
            txtMisc.Text = "0.00";
            //txtVat.Text = "0.00";
            txtNetTCP.Text = "0.00";
            txtDiscount.Text = "0.00";
            txtDiscountPercent.Text = string.Empty;
            //**** Downpayment
            txtDPPercent.Text = string.Empty;
            txtDPAmount.Text = "0.00";
            //txtFrstDP.Text = "0.00";
            txtRsvFee.Text = "0.00";
            txtNetDP.Text = "0.00";
            txtDPTerms.Text = string.Empty;
            txtDPDueDate.Text = string.Empty;
            txtMonthlyDP.Text = "0.00";
            //**** Loanable
            txtLPercent.Text = string.Empty;
            txtLAmount.Text = "0.00";
            txtLTerms.Text = "0.00";
            txtRate.Text = "0.00";
            txtMonthlyAmort.Text = "0.00";
            txtCashAmount.Text = string.Empty;
            txtCheckAmount.Text = "0.00";
            txtCheckTotal.Text = "0.00";
            txtTotalAmount.Text = "0.00";
            lblAmount.Text = "0.00";
            lblAmountDue.Text = "0.00";
            reservationbalance.Text = "0.00";

            gvPayments.DataSource = null;
            gvPayments.DataBind();

            ((DataTable)Session["dtPayments"]).Rows.Clear();

            //** clear check dt **//
            gvChecks.DataSource = null;
            gvChecks.DataBind();
        }
        void progressBar(double value)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "progress", $"progressBar('{value}');", true);
        }
        protected void gvBanks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Sel"))
            {
                int row = int.Parse(e.CommandArgument.ToString());
                string BankCode = gvBanks.Rows[row].Cells[0].Text;
                string BankName = gvBanks.Rows[row].Cells[1].Text;

                txtBankCode.Value = BankCode;
                txtBank.Value = BankName;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "hideBank()", true);
            }
        }
        protected void gvBanks_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadBanks();
            gvBanks.PageIndex = e.NewPageIndex;
            gvBanks.DataBind();
        }
        protected void gvBranch_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Sel"))
            {
                int row = int.Parse(e.CommandArgument.ToString());
                string Account = gvBranch.Rows[row].Cells[0].Text;
                string Branch = gvBranch.Rows[row].Cells[1].Text;
                txtAccount.Value = Account;
                txtBranch.Value = Branch;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "hideBranch()", true);
            }
        }

        protected void gvBranch_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadBranch(txtBankCode.Value);
            gvBranch.PageIndex = e.NewPageIndex;
            gvBranch.DataBind();
        }

        protected void branch_ServerClick(object sender, EventArgs e)
        {
            LoadBranch(txtBankCode.Value);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "showBranch()", true);
        }

        protected void txtCashAmount_TextChanged(object sender, EventArgs e)
        {
            //reservationpayment.Text = GetTotal().ToString("#,#00.00");
            //Session["Payment"] = GetTotal().ToString("#,#00.00");
            ComputeTotalPayment();
        }

        void ComputeTotalPayment()
        {
            string cashTotal = "0";
            if (string.IsNullOrEmpty(txtCashAmount.Text))
            {
                cashTotal = "0";
            }
            else
            {
                cashTotal = txtCashAmount.Text;
            }
            string pymt = SystemClass.ToCurrency((double.Parse(cashTotal) + double.Parse(txtCheckTotal.Text)).ToString());

            txtTotalAmount.Text = pymt;
            Session["Payment"] = pymt;
        }

        protected void txtCheckAmount_TextChanged(object sender, EventArgs e)
        {
            Session["Payment"] = GetTotal().ToString("#,#00.00");
        }
        double GetTotal()
        {
            double check = 0;
            double cash = 0;
            if (!string.IsNullOrEmpty(txtCheckAmount.Text))
            {
                check = double.Parse(txtCheckAmount.Text);
            }

            if (!string.IsNullOrEmpty(txtCashAmount.Text))
            {
                cash = double.Parse(txtCashAmount.Text);
            }

            return check + cash;
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

        protected void btnYes_Click(object sender, EventArgs e)
        {
            //cancel trans
            if ((string)Session["ConfirmType"] == "cancel")
            {
                //clear data
                Response.Redirect("~/pages/Reservation.aspx");
            }
            else if ((string)Session["ConfirmType"] == "payment")
            {
                try
                {
                    int DocEntry = (int)Session["DocEntry"];
                    bool isError = false;
                    foreach (GridViewRow row in gvPayments.Rows)
                    {
                        string type = row.Cells[1].Text;
                        double amount = double.Parse(row.Cells[2].Text);
                        //string checkno = row.Cells[3].Text;
                        string bank = row.Cells[3].Text;
                        string bankname = row.Cells[4].Text;
                        string branch = row.Cells[5].Text;
                        string checkdate = row.Cells[6].Text;
                        string checkno = row.Cells[7].Text;
                        string account = row.Cells[8].Text;
                        //credit
                        string CreditCard = row.Cells[9].Text;
                        string CreditAcctCode = row.Cells[10].Text;
                        string CreditAcct = row.Cells[11].Text;
                        string CreditCardNumber = row.Cells[12].Text;
                        string ValidUntil = row.Cells[13].Text;
                        string IdNum = row.Cells[14].Text;
                        string TelNum = row.Cells[15].Text;
                        string PymtTypeCode = row.Cells[16].Text;
                        string PymtType = row.Cells[17].Text;
                        string NumOfPymts = row.Cells[18].Text;
                        string VoucherNum = row.Cells[19].Text;
                        int Id = int.Parse(row.Cells[20].Text);


                        if (IdNum == "&nbsp;")
                            IdNum = string.Empty;
                        if (TelNum == "&nbsp;")
                            TelNum = string.Empty;

                        if (type == "Cash")
                        {

                            //if (!ws.AddReservationPayments(
                            //     Id,
                            //     type,
                            //     DocEntry,
                            //     amount
                            //     ))
                            //{
                            //    isError = true;
                            //}
                        }
                        else if (type == "Check")
                        {

                            //if (!ws.AddReservationPayments(
                            //    Id,
                            //    type,
                            //    DocEntry,
                            //    amount,
                            //    checkdate,
                            //    checkno,
                            //    bank,
                            //    bankname,
                            //    branch,
                            //    account
                            //    ))
                            //{
                            //    isError = true;
                            //}
                        }
                        else if (type == "Credit")
                        {
                            //if (!ws.AddReservationPayments(
                            //    Id,
                            //    type,
                            //    DocEntry,
                            //    amount,
                            //    string.Empty,
                            //    string.Empty,
                            //    string.Empty,
                            //    string.Empty,
                            //    string.Empty,
                            //    string.Empty,
                            //    CreditCard,
                            //    CreditAcctCode,
                            //    CreditAcct,
                            //    CreditCardNumber,
                            //    ValidUntil,
                            //    IdNum,
                            //    TelNum,
                            //    PymtTypeCode,
                            //    PymtType,
                            //    NumOfPymts,
                            //    VoucherNum,
                            //    "S"
                            //    ))
                            //{
                            //    isError = true;
                            //}
                        }
                    }
                    closeconfirm();

                    if (isError == true)
                    {
                        alertMsg("Error in saving payments", "error");
                    }
                    else
                    {
                        if (double.Parse(reservationbalance.Text) == 0)
                        {
                            //add status to quotation
                            //ws.Execute($"UPDATE OQUT SET ForwardedStatus = 'RSV' Where DocEntry = '{DocEntry}'", "Addon");
                            LoadQuotationList();
                            alertMsg("Operation completed successfully", "success");

                            Session["dtPayments"] = ws.GetReservationPayments(int.Parse(lblDocEntry.Text)).Tables[0];
                            gvPayments.DataSource = (DataTable)Session["dtPayments"];
                            gvPayments.DataBind();

                            ComputeTotal(gvPayments);
                            //clear data
                            Clear();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "showQuotation();", true);
                        }
                        else
                        {
                            alertMsg("Operation completed successfully", "success");

                            //GET QUOTATION DATA FROM SQL[OQUT]
                            DataTable dtQuotation = ws.GetQuotationData(DocEntry).Tables["QuotationData"];
                            Clear();

                            Session["dtPayments"] = ws.GetReservationPayments(int.Parse(lblDocEntry.Text)).Tables[0];
                            gvPayments.DataSource = (DataTable)Session["dtPayments"];
                            gvPayments.DataBind();

                            if (DataAccess.Exist(dtQuotation))
                            {
                                ReloadData(dtQuotation);
                            }
                            ComputeTotal(gvPayments);
                        }
                    }
                }
                catch (Exception ex)
                {
                    alertMsg(ex.Message, "error");
                }
            }
            else if ((string)Session["ConfirmType"] == "removecheck")
            {
                //** delete selected check
                ((DataTable)Session["dtChecks"]).Rows.RemoveAt(int.Parse((Session["checkindex"]).ToString()));

                LoadData(gvChecks, "dtChecks");

                //compute total
                double checkTotal = 0;
                foreach (GridViewRow row in gvChecks.Rows)
                {
                    checkTotal += double.Parse(row.Cells[2].Text);
                }
                txtCheckTotal.Text = SystemClass.ToCurrency(checkTotal.ToString());

                ComputeTotalPayment();

                //** close confirm
                closeconfirm();
            }
            else if ((string)Session["ConfirmType"] == "removepayment")
            {
                //** delete selected check
                ((DataTable)Session["dtPayments"]).Rows.RemoveAt(int.Parse((Session["paymentindex"]).ToString()));
                LoadData(gvPayments, "dtPayments");
                ComputeTotal(gvPayments);

                //delete if exist in db
                ws.RemovePayments(int.Parse(Session["pymtid"].ToString()), Session["type"].ToString());

                //** close confirm
                closeconfirm();
            }
            else if ((string)Session["ConfirmType"] == "find")
            {
                closeconfirm();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "showQuotation();", true);
            }
            else if ((string)Session["ConfirmType"] == "forward")
            {
                closeconfirm();
                try
                {
                    hana.Execute($@"UPDATE ""OQUT"" SET ""ForwardedStatus"" = 'CASHIER' WHERE ""DocEntry"" = '{lblDocEntry.Text}'", hana.GetConnection("SAOHana"));
                    alertMsg("Successfully forwarded to cashier", "success");

                    LoadQuotationList();
                    btnForward.Visible = false;

                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "", "showQuotation();", true);
                }
                catch (Exception ex)
                {
                    alertMsg(ex.Message, "error");
                }

            }
        }

        protected void gvQuotationList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvQuotationList.DataSource = (DataSet)Session["QuotationList"];
            gvQuotationList.PageIndex = e.NewPageIndex;
            gvQuotationList.DataBind();
        }

        protected void gvQuotationList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Sel"))
                {
                    int row = int.Parse(e.CommandArgument.ToString());
                    int DocEntry = int.Parse(gvQuotationList.Rows[row].Cells[0].Text);
                    string Status = gvQuotationList.Rows[row].Cells[13].Text.Trim();

                    if (!string.IsNullOrEmpty(Status))
                    {
                        btnForward.Visible = false;
                    }
                    else
                    {
                        btnForward.Visible = true;
                    }

                    Session["DocEntry"] = DocEntry;

                    //GET QUOTATION DATA FROM SQL [OQUT]
                    DataTable dtQuotation = ws.GetQuotationData(DocEntry).Tables[0];
                    if (DataAccess.Exist(dtQuotation))
                    {
                        ReloadData(dtQuotation);


                        //** load monthly amortization **//
                        gvAmortization.DataSource = ws.GetPaymentSchedule(DocEntry, "LB");
                        gvAmortization.DataBind();

                        if (gvAmortization.Rows.Count == 0)
                        { dAmort.Visible = false; }
                        else { dAmort.Visible = true; }




                        //** load monthly dp **//
                        gvDownPayment.DataSource = ws.GetPaymentSchedule(DocEntry, "DP");
                        gvDownPayment.DataBind();

                        if (gvDownPayment.Rows.Count == 0)
                        { dDown.Visible = false; }
                        else { dDown.Visible = true; }


                        //** load misc dp schedule **//
                        gvMiscellaneousDP.DataSource = ws.GetPaymentSchedule(DocEntry, "MISC", "DP");
                        gvMiscellaneousDP.DataBind();

                        if (gvMiscellaneousDP.Rows.Count == 0)
                        { dMiscDP.Visible = false; }
                        else { dMiscDP.Visible = true; }

                        //** load misc lb schedule **//
                        gvMiscellaneousAmort.DataSource = ws.GetPaymentSchedule(DocEntry, "MISC", "LB");
                        gvMiscellaneousAmort.DataBind();

                        if (gvMiscellaneousAmort.Rows.Count == 0)
                        { divMonthlyAmortMisc.Visible = false; }
                        else { divMonthlyAmortMisc.Visible = true; }



                        //** load existing payments **//
                        Session["dtPayments"] = ws.GetReservationPayments(int.Parse(lblDocEntry.Text)).Tables[0];
                        gvPayments.DataSource = (DataTable)Session["dtPayments"];
                        gvPayments.DataBind();

                        ComputeTotal(gvPayments);
                        ComputeTotalPayment();

                        divMonthlyDP.Visible = true;
                        divMonthlyAmort.Visible = true;
                    }
                    //** highlight selected row **//
                    foreach (GridViewRow selectedrow in gvQuotationList.Rows)
                    {
                        if (gvQuotationList.Rows[selectedrow.RowIndex].Cells[0].Text == DocEntry.ToString())
                        { selectedrow.BackColor = System.Drawing.ColorTranslator.FromHtml("#A1DCF2"); }
                        else
                        { selectedrow.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF"); }
                    }

                    loadCoOwner();

                    //btnSchedule.Disabled = false;

                    //close modal
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "closeQuotation();", true);

                    LoadReports();


                    //2023-05-05 : REQUESTED BY DHEZA
                    loadCommissionScheme();
                    loadIncentiveScheme();
                    loadSharingDetails();

                    //HIDE DAS REPORT WHEN QUOTATION IS NOT FULLY PAID
                    foreach (GridViewRow row1 in gvReports.Rows)
                    {
                        if (row1.Cells[1].Text == "Deeds of Absolute Sale")
                        {
                            DataTable dt = new DataTable();
                            dt = hana.GetDataDS($@"SELECT ""DocEntry"" FROM QUT1 WHERE ""DocEntry"" = {Session["DocEntry"]} AND ""LineStatus"" = 'O'", hana.GetConnection("SAOHana")).Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                if (lblLoi.Text == "NO")
                                {
                                    gvReports.DataSource = hana.GetDataDS($@"SELECT * FROM ""ORPT"" Where ""RptGroup"" = 'OQUT' AND ""Id"" NOT IN (5,2) ", hana.GetConnection("SAOHana")).Tables[0];
                                }
                                else
                                {
                                    gvReports.DataSource = hana.GetDataDS($@"SELECT * FROM ""ORPT"" Where ""RptGroup"" = 'OQUT' AND ""Id"" NOT IN (5) ", hana.GetConnection("SAOHana")).Tables[0];
                                }
                                gvReports.DataBind();
                            }

                        }
                    }


                }
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }
        }

        protected void bSearch_ServerClick(object sender, EventArgs e)
        {
            if (chkPerProject.Checked)
            {
                Session["QuotationList"] = ws.SearchQuotationListPerProject(txtSearch.Value);
                gvQuotationList.DataSource = (DataSet)Session["QuotationList"];
                gvQuotationList.DataBind();
            }
            if (chkPerProjectName.Checked)
            {
                Session["QuotationList"] = ws.SearchQuotationListPerProjectName(txtSearch.Value);
                gvQuotationList.DataSource = (DataSet)Session["QuotationList"];
                gvQuotationList.DataBind();
            }

            if (!chkPerProject.Checked && !chkPerProjectName.Checked)
            {
                Session["QuotationList"] = ws.SearchQuotationList(txtSearch.Value);
                gvQuotationList.DataSource = (DataSet)Session["QuotationList"];
                gvQuotationList.DataBind();
            }
        }

        protected void btnPayment_ServerClick(object sender, EventArgs e)
        {
            if (gvPayments.Rows.Count > 0)
            {
                if (double.Parse(lblAmountDue.Text) > double.Parse(lblAmount.Text))
                {
                    alertMsg("Unable to process over payment", "warning");
                }
                else
                {
                    confirmation("Are you sure you want to save payments?", "payment");
                }
            }
            else
            {
                alertMsg("Please add payments", "warning");
            }
        }
        protected void btnAddCheck_Click(object sender, EventArgs e)
        {
            //** check adding blockings **//
            if (string.IsNullOrEmpty(txtCheckAmount.Text) ||
                string.IsNullOrEmpty(txtCheckNo.Value) ||
                string.IsNullOrEmpty(txtBank.Value) ||
                string.IsNullOrEmpty(txtCheckDate.Text))
            {
                alertMsg("Please complete all fields", "warning");
            }
            else
            {
                if (btnAddCheck.Text == "Add Check")
                {
                    //add check
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["dtPayments"];

                    DataRow dr = dt.NewRow();
                    dr[0] = Convert.ToInt32(dt.Rows.Count + 1);
                    dr[1] = "Check";
                    dr[2] = SystemClass.ToCurrency(txtCheckAmount.Text);
                    //check
                    dr[3] = txtCheckNo.Value;
                    dr[4] = txtBankCode.Value;
                    dr[5] = txtBank.Value;
                    dr[6] = txtBranch.Value;
                    dr[7] = txtCheckDate.Text;
                    dr[8] = txtAccount.Value;

                    dt.Rows.Add(dr);
                    Session["dtPayments"] = dt;
                    LoadData(gvPayments, "dtPayments");
                }
                else
                {
                    //update check
                    foreach (DataRow dr in ((DataTable)Session["dtPayments"]).Rows)
                    {
                        if (dr[0].ToString() == Session["linenum"].ToString())
                        {
                            dr[2] = SystemClass.ToCurrency(txtCheckAmount.Text);
                            dr[3] = txtCheckNo.Value;
                            dr[4] = txtBankCode.Value;
                            dr[5] = txtBank.Value;
                            dr[6] = txtBranch.Value;
                            dr[7] = txtCheckDate.Text;
                            dr[8] = txtAccount.Value;
                            break;
                        }
                    }

                    btnAddCheck.Text = "Add Check";
                }

                LoadData(gvPayments, "dtPayments");
                ComputeTotal(gvPayments);
                ClearCheck();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "closeCheck();", true);
            }
        }
        void ClearCheck()
        {
            txtCheckNo.Value = string.Empty;
            txtCheckAmount.Text = string.Empty;
            txtBank.Value = string.Empty;
            txtBankCode.Value = string.Empty;
            txtBranch.Value = string.Empty;
            txtCheckDate.Text = string.Empty;
            txtAccount.Value = string.Empty;
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
        protected void gvChecks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            Session["checkindex"] = index;
            string linenum = gvChecks.Rows[index].Cells[0].Text;

            if (e.CommandName.Equals("Del"))
            {
                confirmation("Are you sure you want to remove selected check?", "removecheck");
            }
        }
        void LoadGridView(string session)
        {
            DataTable dt = new DataTable();

            if (session == "dtPayments")
            {
                dt.Columns.Add("LineNum", typeof(int));
                dt.Columns.Add("Type", typeof(string));
                dt.Columns.Add("Amount", typeof(double));
                //check details
                dt.Columns.Add("CheckNo", typeof(string));
                dt.Columns.Add("BankCode", typeof(string));
                dt.Columns.Add("Bank", typeof(string));
                dt.Columns.Add("Branch", typeof(string));
                dt.Columns.Add("DueDate", typeof(string));
                dt.Columns.Add("AccountNum", typeof(string));
                //credit details
                dt.Columns.Add("CreditCard", typeof(string));
                dt.Columns.Add("CreditAcctCode", typeof(string));
                dt.Columns.Add("CreditAcct", typeof(string));
                dt.Columns.Add("CreditCardNumber", typeof(string));
                dt.Columns.Add("ValidUntil", typeof(string));
                dt.Columns.Add("IdNum", typeof(string));
                dt.Columns.Add("TelNum", typeof(string));
                dt.Columns.Add("PymtTypeCode", typeof(string));
                dt.Columns.Add("PymtType", typeof(string));
                dt.Columns.Add("NumOfPymts", typeof(string));
                dt.Columns.Add("VoucherNum", typeof(string));
                dt.Columns.Add("Id", typeof(string));
                dt.Columns.Add("OR", typeof(string));

                Session[session] = dt;

                if (session == "gvPayments")
                { LoadData(gvPayments, session); }
            }
        }
        void confirmation(string body, string type)
        {
            Session["ConfirmType"] = type;
            lblConfirmationInfo.Text = body;
            //Show Modal
            ScriptManager.RegisterStartupScript(this, this.GetType(), "confirmation", "showConfirmation();", true);
        }
        void closeconfirm()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "close", "closeConfirmation();", true);
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            confirmation("Are you sure you want to change selected quotation?Unsaved data will be lost.", "find");
        }

        protected void removebuyer_Click(object sender, EventArgs e)
        {

        }

        protected void gvHouseLot_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvPayments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            Session["paymentindex"] = index;
            Session["linenum"] = gvPayments.Rows[index].Cells[0].Text;
            Session["type"] = gvPayments.Rows[index].Cells[1].Text;
            Session["pymtid"] = gvPayments.Rows[index].Cells[20].Text;
            string linenum = gvPayments.Rows[index].Cells[0].Text;
            string type = gvPayments.Rows[index].Cells[1].Text;
            string ornumber = gvPayments.Rows[index].Cells[21].Text.Trim();

            if (e.CommandName.Equals("Del"))
            {
                if (string.IsNullOrEmpty(ornumber))
                {
                    confirmation("Are you sure you want to remove selected payment?", "removepayment");
                }
                else
                {
                    alertMsg("Unable to remove payments with OR", "warning");
                }
            }
            else if (e.CommandName.Equals("Edt"))
            {
                if (type == "Cash")
                {
                    if (string.IsNullOrEmpty(ornumber))
                    {
                        txtCashTotal.Text = double.Parse(gvPayments.Rows[index].Cells[2].Text).ToString();
                        bAddCash.Text = "Update";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "showCash", "showCash();", true);

                    }
                    else
                    {
                        alertMsg("Unable to edit payments with OR", "warning");
                    }
                }
                else if (type == "Check")
                {
                    //load check details
                    string checkamt = gvPayments.Rows[index].Cells[2].Text;
                    string checkno = gvPayments.Rows[index].Cells[3].Text;
                    string bankcode = gvPayments.Rows[index].Cells[4].Text;
                    string bank = gvPayments.Rows[index].Cells[5].Text;
                    string branch = gvPayments.Rows[index].Cells[6].Text;
                    string duedate = gvPayments.Rows[index].Cells[7].Text;
                    string acctnum = gvPayments.Rows[index].Cells[8].Text;

                    txtCheckAmount.Text = double.Parse(checkamt).ToString();
                    txtCheckNo.Value = checkno;
                    txtBankCode.Value = bankcode;
                    txtBank.Value = bank;
                    txtBranch.Value = branch;
                    txtCheckDate.Text = duedate;
                    txtAccount.Value = acctnum;

                    btnAddCheck.Text = "Update Check";

                    if (!string.IsNullOrEmpty(ornumber))
                    {
                        btnAddCheck.Enabled = false;
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showCheck", "showCheck();", true);
                }
                else if (type == "Credit")
                {
                    //load check details
                    string creditamt = gvPayments.Rows[index].Cells[2].Text;
                    string creditcard = gvPayments.Rows[index].Cells[9].Text;
                    string crediacctcode = gvPayments.Rows[index].Cells[10].Text;
                    string crediacct = gvPayments.Rows[index].Cells[11].Text;
                    string creditcardnum = gvPayments.Rows[index].Cells[12].Text;
                    string validuntil = gvPayments.Rows[index].Cells[13].Text.Replace("/1/", "/");
                    string idnum = gvPayments.Rows[index].Cells[14].Text;
                    string telnum = gvPayments.Rows[index].Cells[15].Text;
                    string pymttypecode = gvPayments.Rows[index].Cells[16].Text;
                    string pymttype = gvPayments.Rows[index].Cells[17].Text;
                    string numofpymts = gvPayments.Rows[index].Cells[18].Text;
                    string vouchernum = gvPayments.Rows[index].Cells[19].Text;

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

                    btnAddCredit.Text = "Update Credit";

                    if (!string.IsNullOrEmpty(ornumber))
                    {
                        btnAddCredit.Enabled = false;
                    }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showCreditPayment", "showCreditPayment();", true);
                }
            }
        }
        protected void bAddCash_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCashTotal.Text))
            {
                if (bAddCash.Text == "Add")
                {
                    //new cash payment
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["dtPayments"];

                    DataRow dr = dt.NewRow();
                    dr[0] = Convert.ToInt32(dt.Rows.Count + 1);
                    dr[1] = "Cash";
                    dr[2] = SystemClass.ToCurrency(txtCashTotal.Text);

                    dt.Rows.Add(dr);
                    Session["dtPayments"] = dt;
                }
                else
                {
                    //update
                    foreach (DataRow row in ((DataTable)Session["dtPayments"]).Rows)
                    {
                        if (row[0].ToString() == (string)Session["linenum"])
                        {
                            row[2] = double.Parse(txtCashTotal.Text);
                        }
                    }
                }

                LoadData(gvPayments, "dtPayments");
                ComputeTotal(gvPayments);
                //close modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "closeCash();", true);
            }
            else
            {
                alertMsg("Please enter amount", "warning");
            }
        }
        void ComputeTotal(GridView gv)
        {
            //compute total
            double total = 0;
            double balance = 0;
            foreach (GridViewRow row in gv.Rows)
            {
                total += double.Parse(row.Cells[2].Text);
            }

            lblAmountDue.Text = SystemClass.ToCurrency(total.ToString());
            //compute remaining balance
            balance = double.Parse(lblAmount.Text) - double.Parse(lblAmountDue.Text);
            reservationbalance.Text = SystemClass.ToCurrency(balance.ToString());
        }
        protected void btnClose_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/pages/Dashboard.aspx");
        }
        protected void btnAddCredit_Click(object sender, EventArgs e)
        {
            //** check adding blockings **//
            if (string.IsNullOrEmpty(txtCreditCard.Value) ||
                string.IsNullOrEmpty(txtCreditCardNum.Value) ||
                string.IsNullOrEmpty(txtCreditAccount.Value) ||
                string.IsNullOrEmpty(txtCreditAmount.Text) ||
                string.IsNullOrEmpty(txtCreditMethod.Value) ||
                string.IsNullOrEmpty(txtNoOfPayments.Value) ||
                string.IsNullOrEmpty(txtVoucherNum.Value))
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
                        dt = (DataTable)Session["dtPayments"];

                        DataRow dr = dt.NewRow();
                        dr[0] = Convert.ToInt32(dt.Rows.Count + 1);
                        dr[1] = "Credit";
                        dr[2] = SystemClass.ToCurrency(txtCreditAmount.Text);
                        //credit
                        dr[9] = txtCreditCard.Value;
                        dr[10] = txtCreditAccountCode.Value;
                        dr[11] = txtCreditAccount.Value;
                        dr[12] = txtCreditCardNum.Value;
                        dr[13] = txtValidUntil.Text.Substring(0, 2) + "/1" + txtValidUntil.Text.Substring(2, 3);
                        dr[14] = txtIDNum.Value;
                        dr[15] = txtTelNo.Value;
                        dr[16] = txtCreditMethodCode.Value;
                        dr[17] = txtCreditMethod.Value;
                        dr[18] = txtNoOfPayments.Value;
                        dr[19] = txtVoucherNum.Value;

                        dt.Rows.Add(dr);
                        Session["dtPayments"] = dt;
                        LoadData(gvPayments, "dtPayments");
                    }
                    else
                    {
                        //update check
                        foreach (DataRow dr in ((DataTable)Session["dtPayments"]).Rows)
                        {
                            if (dr[0].ToString() == Session["linenum"].ToString())
                            {
                                dr[2] = SystemClass.ToCurrency(txtCreditAmount.Text);
                                //credit
                                dr[9] = txtCreditCard.Value;
                                dr[10] = txtCreditAccountCode.Value;
                                dr[11] = txtCreditAccount.Value;
                                dr[12] = txtCreditCardNum.Value;
                                dr[13] = txtValidUntil.Text.Substring(0, 2) + "/1" + txtValidUntil.Text.Substring(2, 3);
                                dr[14] = txtIDNum.Value;
                                dr[15] = txtTelNo.Value;
                                dr[16] = txtCreditMethodCode.Value;
                                dr[17] = txtCreditMethod.Value;
                                dr[18] = txtNoOfPayments.Value;
                                dr[19] = txtVoucherNum.Value;
                                break;
                            }
                        }
                        btnAddCredit.Text = "Add Credit";
                    }

                    LoadData(gvPayments, "dtPayments");
                    ComputeTotal(gvPayments);
                    ClearCheck();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "closeCreditPayment();", true);
                }
            }
        }

        protected void gvAccounts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName.Equals("Sel"))
            {
                txtCreditAccountCode.Value = gvAccounts.Rows[index].Cells[0].Text;
                txtCreditAccount.Value = gvAccounts.Rows[index].Cells[1].Text;
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "close", "closeAccounts();", true);
        }
        protected void gvAccounts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAccounts.DataSource = (DataSet)Session["GLAccounts"];
            gvAccounts.PageIndex = e.NewPageIndex;
            gvAccounts.DataBind();
        }

        protected void btnSearchAccounts_Click(object sender, EventArgs e)
        {
            Session["GLAccounts"] = ws.SearchGLAccounts(txtSearchAccounts.Value);
            gvAccounts.DataSource = (DataSet)Session["GLAccounts"];
            gvAccounts.DataBind();
        }

        protected void btnAccounts_ServerClick(object sender, EventArgs e)
        {
            LoadAccounts();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showAccounts", "showAccounts();", true);
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "close", "closeCredit();", true);
        }

        protected void btnShowCredit_ServerClick(object sender, EventArgs e)
        {
            gvCreditCard.DataSource = ws.GetCreditCards();
            gvCreditCard.DataBind();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showCredit", "showCredit();", true);
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

        protected void btnCash_ServerClick(object sender, EventArgs e)
        {
            txtCashAmount.Text = "0";
            bAddCash.Text = "Add";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cash", "showCash();", true);
        }

        protected void btnCheck_ServerClick(object sender, EventArgs e)
        {
            ClearCheck();
            btnAddCheck.Text = "Add Check";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "check", "showCheck();", true);
        }

        protected void btnCredit_ServerClick(object sender, EventArgs e)
        {
            ClearCredit();
            btnAddCredit.Text = "Add Credit";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "credit", "showCreditPayment();", true);
        }

        protected void gvPayments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Checking the RowType of the Row  
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string ornumber = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "ORNumber"));

                if (!string.IsNullOrEmpty(ornumber))
                {
                    e.Row.Attributes["style"] = "background-color: #96ff9d";
                }
                else
                {
                    e.Row.Attributes["style"] = "background-color: #ffffff";
                }
            }
        }

        protected void gvReports_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            //if (e.CommandName.Equals("Print"))
            //{

            //if (gvReports.Rows[index].Cells[2].Text.Contains("Reservation Agreement") ||
            //    gvReports.Rows[index].Cells[2].Text.Contains("Amortization Schedule") ||
            //    gvReports.Rows[index].Cells[2].Text.Contains("Letter of Intent")
            //   )
            //{
            Session["RptConn"] = "SAP";
            //}
            //else
            //{
            //    Session["RptConn"] = "Addon";
            //}

            if (gvReports.Rows[index].Cells[5].Text == "1")
            {
                Session["PrintDocEntry"] = lblID.Text;

                //2023-06-05 : Add Project ; additional parameter on form
                //Session["reportParameter"] =  txtProjID.Text;
                //2023-06-06 : Change from Project to DocEntry instead
                Session["reportParameter"] = lblDocEntry.Text;
                Session["ReportType"] = "Buyer";
            }
            else
            {
                Session["PrintDocEntry"] = lblDocEntry.Text;
                Session["ReportType"] = "";
            }


            Session["ReportName"] = gvReports.Rows[index].Cells[2].Text + ".rpt";
            Session["ReportPath"] = gvReports.Rows[index].Cells[3].Text;
            //Session["ReportPath"] = ConfigSettings.ReportPathLocal"];


            //open new tab
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openpage", "window.open('ReportViewer.aspx');", true);
            //}
        }

        protected void gvReports_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Checking the RowType of the Row  
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    Image img = e.Row.FindControl("isPrinted") as Image;

            //    //Check if Block has image
            //    if (!string.IsNullOrEmpty(e.Row.Cells[4].Text.Trim()))
            //    {
            //        img.ImageUrl = "~/assets/img/checked.png";
            //    }
            //    else
            //    {
            //        img.ImageUrl = "~/assets/img/cancel.png";
            //    }
            //}
        }

        protected void btnForward_ServerClick(object sender, EventArgs e)
        {
            confirmation("Are you sure you want to forward to cashier?", "forward");
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int index = int.Parse(btn.CommandArgument);


            Session["PrintDocEntry"] = lblDocEntry.Text;
            Session["Title"] = gvReports.Rows[index].Cells[1].Text;
            Session["ReportName"] = gvReports.Rows[index].Cells[2].Text + ".rpt";
            Session["ReportPath"] = gvReports.Rows[index].Cells[3].Text;
            int objcode = int.Parse(gvReports.Rows[index].Cells[5].Text);

            //report type 1=report 0=forms
            if (objcode == 1)
            {
                Session["ReportType"] = "Buyer";
                Session["PrintDocEntry"] = lblDocEntry.Text;
                Session["reportParameter"] = lblID.Text;
            }

            //open new tab
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openpage", "window.open('ReportViewer.aspx');", true);

            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "report", "window.open('../pages/ReportViewer.aspx', '_blank');", true);
        }

        protected void gvQuotationList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Checking the RowType of the Row  
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string status = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "ForwardedStatus"));

                if (!string.IsNullOrWhiteSpace(status))
                {
                    e.Row.Attributes["style"] = "background-color: #adf6dc";
                }
                else
                {
                    e.Row.Attributes["style"] = "background-color: #ffffff";
                }
            }
        }

        protected void gvAdditionalCharges_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMiscellaneousDP.DataSource = ws.GetPaymentSchedule(int.Parse(Session["QuotationID"].ToString()), "MISC", "DP");
            gvMiscellaneousDP.PageIndex = e.NewPageIndex;
            gvMiscellaneousDP.DataBind();
        }

        protected void chkPerProject_CheckedChanged(object sender, EventArgs e)
        {

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


        protected void gvCoOwner_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {


            gvCoOwner.PageIndex = e.NewPageIndex;
            loadCoOwner();
        }

        void loadCoOwner()
        {

            //Get data for Co-owner
            DataTable dtCoOwner = hana.GetData($@"SELECT ""CardCode"" ""Code"", ""Name"" FROM ""QUT10"" WHERE ""DocEntry"" = { lblDocEntry.Text }", hana.GetConnection("SAOHana"));
            gvCoOwner.DataSource = dtCoOwner;
            gvCoOwner.DataBind();
        }





        protected void gvMiscellaneousAmort_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMiscellaneousAmort.DataSource = ws.GetPaymentSchedule(int.Parse(Session["QuotationID"].ToString()), "MISC", "LB");
            gvMiscellaneousAmort.PageIndex = e.NewPageIndex;
            gvMiscellaneousAmort.DataBind();
        }


        void loadCommissionScheme()
        {

            //2023-05-05 : REQUESTED BY DHEZA
            //GET COMMISSION SCHEME
            DataTable dtCommissionScheme = hana.GetData($@"SELECT * FROM ""QUT14"" WHERE ""DocEntry"" = { lblDocEntry.Text }", hana.GetConnection("SAOHana"));
            gvCommissionScheme.DataSource = dtCommissionScheme;
            gvCommissionScheme.DataBind();
        }



        protected void gvCommissionScheme_PageIndexChanging1(object sender, GridViewPageEventArgs e)
        {
            //2023-05-05 : REQUESTED BY DHEZA
            //GET SHARING DETAILS
            DataTable dtCommissionScheme = hana.GetData($@"SELECT * FROM ""QUT14"" WHERE ""DocEntry"" = { lblDocEntry.Text }", hana.GetConnection("SAOHana"));
            gvCommissionScheme.DataSource = dtCommissionScheme;
            gvCommissionScheme.PageIndex = e.NewPageIndex;
            gvCommissionScheme.DataBind();
        }


        void loadIncentiveScheme()
        {

            //2023-05-05 : REQUESTED BY DHEZA
            //GET INCENTIVE SCHEME
            DataTable dtIncentiveScheme = hana.GetData($@"SELECT * FROM ""QUT15"" WHERE ""DocEntry"" = { lblDocEntry.Text }", hana.GetConnection("SAOHana"));
            gvIncentiveScheme.DataSource = dtIncentiveScheme;
            gvIncentiveScheme.DataBind();
        }

        protected void gvIncentiveScheme_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //2023-05-05 : REQUESTED BY DHEZA
            //GET INCENTIVE SCHEME
            DataTable dtIncentiveScheme = hana.GetData($@"SELECT * FROM ""QUT15"" WHERE ""DocEntry"" = { lblDocEntry.Text }", hana.GetConnection("SAOHana"));
            gvIncentiveScheme.DataSource = dtIncentiveScheme;
            gvIncentiveScheme.PageIndex = e.NewPageIndex;
            gvIncentiveScheme.DataBind();
        }


        void loadSharingDetails()
        {

            //2023-05-05 : REQUESTED BY DHEZA
            //GET SHARING DETAILS
            DataTable dtSharingDetails = hana.GetData($@"SELECT * FROM ""QUT11"" WHERE ""DocEntry"" = { lblDocEntry.Text }", hana.GetConnection("SAOHana"));
            gvSharingDetails.DataSource = dtSharingDetails;
            gvSharingDetails.DataBind();
        }


        protected void gvSharingDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }
    }
}
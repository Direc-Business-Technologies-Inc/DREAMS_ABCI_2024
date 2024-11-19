using ABROWN_DREAMS.Services;
using ABROWN_DREAMS.wcf;
using DataCipher;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABROWN_DREAMS
{
    public partial class Restructuring : Page
    {
        DirecWebService ws = new DirecWebService();
        SAPHanaAccess hana = new SAPHanaAccess();
        SapHanaLayer company = new SapHanaLayer();
        DirecService wcf = new DirecService();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

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

                    //dtpDueDate.Text = oDate.ToString("yyyy-MM-dd");
                    txtDPDueDate.Text = Convert.ToDateTime(tDocDate.Text).AddMonths(1).ToString("yyyy-MM-dd");

                    lblOldMiscDueDate.Text = oDate.ToString("yyyy-MM-dd");
                    lblNewMiscDueDate.Text = oDate.ToString("yyyy-MM-dd");

                    DataTable coowner = new DataTable();
                    coowner.Columns.AddRange(new DataColumn[2]
                            {
                        new DataColumn("Code"),
                        new DataColumn("Name")
                            });
                    ViewState["CoOwner"] = coowner;
                    gvCoOwner.DataSource = coowner;
                    gvCoOwner.DataBind();


                    //LoadOwners();
                    RefreshSalesList();
                    LoadFromFindBtn();
                    LoadOwners();
                    LoadRestructuringTypes();
                    deleteSampleQuotation();


                    //VIEWSTATES FOR UPDATING ENABLED FIELDS
                    ViewState["UpdateResFee"] = 0;
                    ViewState["UpdateMiscFee"] = 0;

                    //DISABLE BLOCKING WHEN USER LOGGED IN IS CASHIER

                    DataTable dtUserAccess1 = (DataTable)Session["UserAccess"];
                    var CC = dtUserAccess1.Select($"CodeEncrypt= 'CC'");
                    string CCValue = "N";
                    if (CC.Any())
                    {
                        CCValue = "Y";
                    }

                    if (CCValue == "Y")
                    {
                        RequiredFieldValidator18.Enabled = false;
                        RequiredFieldValidator18.Visible = false;
                        //RequiredFieldValidator3.Enabled = false;
                        //RequiredFieldValidator3.Visible = false;
                        requestLetter.Visible = false;
                        requestLetterDate.Visible = false;

                    }


                    ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "MsgBuyers_Show();", true);
                }

                if (Session["UserID"] == null)
                {
                    alertMsg("Session expired!", "error");
                    Response.Redirect("~/pages/Login.aspx");
                }
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "info");
            }
        }





        //STANDALONE LOCAL PROCESSES//

        void deleteSampleQuotation()
        {
            hana.Execute($@"DELETE FROM OQUT WHERE ""DocEntry"" = 0;", hana.GetConnection("SAOHana"));
        }


        void CheckUserType()
        {
            DataTable dt = new DataTable();
            dt = hana.GetData($@"SELECT * FROM ""USR2"" WHERE ""UserID"" = {Session["UserID"]}", hana.GetConnection("SAOHana"));
            if (DataAccess.Exist(dt) == true)
            {
                for (int i = 0; i <= dt.Rows.Count; i++)
                {
                    string Mode = Cryption.Decrypt((string)DataAccess.GetData(dt, 0, "CodeEncrypt", ""));
                    int cnt = Mode.Length - Session["UserID"].ToString().Length;
                    Mode = Mode.Substring(Session["UserID"].ToString().Length, cnt);

                    //Commented to allow changing of Financing Scheme
                    //if (!Mode.ToLower().Contains("cc") && Session["UserID"].ToString() != "1")
                    //{
                    //    bFinancing.Visible = false;
                    //    break;
                    //}
                    //else
                    //{
                    //    bFinancing.Visible = true;
                    //}
                }

            }

        }


        void loadRestructuringDocReq()
        {

            try
            {
                DataTable dt = new DataTable();
                dt = hana.GetData($@"select * from ""@DOCUMENTS"" where ""U_Type"" = 'Restructuring'", hana.GetConnection("SAPHana"));
                if (DataAccess.Exist(dt) == true)
                {
                    gvRestructuringDocumentRequirement.DataSource = dt;
                    gvRestructuringDocumentRequirement.DataBind();


                    if (!string.IsNullOrWhiteSpace(lblDocEntry.Text))
                    {
                        foreach (GridViewRow row1 in gvRestructuringDocumentRequirement.Rows)
                        {
                            string DocId = gvRestructuringDocumentRequirement.Rows[row1.RowIndex].Cells[0].Text;

                            string qry = $@"select * from RDOC where ""DocID"" = '{DocId}' AND ""DocEntry"" = '{lblDocEntry.Text}' AND IFNULL(""FileName"",'') <> ''";
                            DataTable dt1 = hana.GetData(qry, hana.GetConnection("SAOHana"));

                            if (dt1.Rows.Count > 0)
                            {
                                TextBox txtExpiration = (TextBox)gvRestructuringDocumentRequirement.Rows[row1.RowIndex].FindControl("lblExpirationDate");
                                LinkButton btnPreview = (LinkButton)gvRestructuringDocumentRequirement.Rows[row1.RowIndex].FindControl("btnPreview1");
                                LinkButton btnDelete = (LinkButton)gvRestructuringDocumentRequirement.Rows[row1.RowIndex].FindControl("btnRemove1");
                                Label lblFileName = (Label)gvRestructuringDocumentRequirement.Rows[row1.RowIndex].FindControl("lblFileName1");

                                txtExpiration.Text = Convert.ToDateTime(DataAccess.GetData(dt1, 0, "ExpirationDate", "")).ToString("yyyy-MM-dd");
                                lblFileName.Text = (string)DataAccess.GetData(dt1, 0, "FileName", "");
                                visibleDocumentButtons(true, btnPreview, btnDelete);
                            }
                        }
                    }



                }
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "info");
            }

        }



        void RefreshSalesList()
        {
            ////Session["gvPosList"] = hana.GetData(@"SELECT ""Code"",""Name"" FROM ""OLST"" WHERE ""GrpCode"" = 'POS'", hana.GetConnection("SAOHana"));
            //Session["gvPosList"] = hana.GetData(@"SELECT 'Agent' ""Code"",'Agent' ""Name"" FROM DUMMY", hana.GetConnection("SAOHana"));
            //LoadData(gvPosList, "gvPosList");
        }
        void LoadFromFindBtn()
        {
            DataTable dt = new DataTable();
            dt = hana.GetData($"CALL sp_GetQuotationRestructure ({Session["UserID"]});", hana.GetConnection("SAOHana"));
            if (DataAccess.Exist(dt) == true)
            {
                ViewState["dtBuyers"] = dt;
                LoadData(gvBuyers, "dtBuyers");
            }
        }

        void LoadRestructuringTypes()
        {
            try
            {

                DataTable dt = new DataTable();
                DataTable dtUserAccess1 = (DataTable)Session["UserAccess"];
                var CC = dtUserAccess1.Select($"CodeEncrypt= 'CC'");
                string CCValue = "N";

                if (CC.Any())
                {
                    CCValue = "Y";
                }



                dt = ws.LoadRestructuringTypes(CCValue).Tables["LoadRestructuringTypes"];
                if (DataAccess.Exist(dt) == true)
                {
                    ddlRestructureType.DataSource = dt;
                    ddlRestructureType.DataBind();
                }
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "info");
            }
        }


        void LoadData(GridView gv, string viewstate)
        {
            try
            {
                gv.DataSource = (DataTable)ViewState[viewstate];
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
        void ClearHouseDetails(string oClearType)
        {
            switch (oClearType)
            {
                case "Project":
                    //hPrjCode.Value = String.Empty;
                    tProjCode.Value = String.Empty;
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
            txtFinancingMisc.Value = String.Empty;

            tPhase.Value = String.Empty;
            txtLotClassification.Value = String.Empty;
            txtProductType.Value = String.Empty;


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
            txtAdjLot.Value = String.Empty;

            tResrvFee.Text = "0.00";
            tTerms.Text = "0";
            ViewState["IsGetHouse"] = false;
            ViewState["IsGetSize"] = false;
            ViewState["IsGetFeat"] = false;
            tHouseStatus.Value = String.Empty;
            tSize.Value = String.Empty;
            tFeature.Value = String.Empty;
            //tDocDate.Text = String.Empty;

            ClearFigureTextBoxes();
            //clearComputationTabs();
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
        void ClearProfileTab()
        {
            lblID.Text = String.Empty;
            lblName.Text = String.Empty;
            lblFirstName.Text = String.Empty;
            lblLastName.Text = String.Empty;
            lblMiddleName.Text = String.Empty;

            //2023-05-24 : ADD LOADING OF CO-BORROWER
            txtComaker1.Value = String.Empty;

            lblBusinessType.Text = String.Empty;
            lblNatureofEmployment.Text = String.Empty;
            lblTypeofID.Text = String.Empty;
            lblIDNo.Text = String.Empty;
            lblBirthday.Text = String.Empty;
            tFinancing.Value = String.Empty;
            txtFinancingMisc.Value = String.Empty;

            tAccountType.Value = String.Empty;
            lblTaxClassification.Text = String.Empty;
            lblTIN.Text = String.Empty;
            lblDocNum.Text = String.Empty;

            ddlBusinessType.SelectedValue = "Individual";
            lblEmployeeID.Text = "";
            lblEmployeeName.Text = "";
            lblEmployeePosition.Text = "";
            lblEmployeeBrokerID.Text = "";




            ViewState["DocNum"] = "";

            ClearHouseDetails("Project");
            PrevTab("HouseDetails");
            ScriptManager.RegisterStartupScript(this, GetType(), "clear", "ResetTextBox()", true);
            RefreshSalesList();
            clearComputationTabs();
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
        void clearComputationTabs()
        {
            //########################################################################
            //############## SECOND TAB - OLD COMPUTATIONS  ##########################
            //########################################################################

            lblOldDASAmt.Text = "0.00";
            lblOldDiscount.Text = "0.00";
            lblOldNetTCP.Text = "0.00";

            lblOldAddMiscFees.Text = "0.00";
            lblOldCompTotal.Text = "0.00";
            lblOldReserveFee.Text = "0.00";

            lblOldTCPFinScheme.Text = "";
            lblOldDownPayment.Text = "0.00";
            lblOldReserveFee2.Text = "0.00";
            lblOldBalance.Text = "0.00";
            lblOldDPTerms.Text = "";
            lblOldDPMonthly.Text = "0.00";
            lblOldLoanableBalance.Text = "0.00";

            lblOldMiscFees.Text = "0.00";
            lblOldMiscDPTerms.Text = "0";
            lblOldMiscDPMonthly.Text = "0.00";

            //HIDDEN FIELDS
            lblOldNetDAS.Text = "0.00";
            //tNetDas.Value = SystemClass.ToCurrency((grossTCP + vat).ToString());
            lblOldNetDAS2.Text = "0.00";
            lblOldVAT.Text = "0.00";
            lblOldAddMiscCharges.Text = "0.00";
            //lblOldAddMiscCharges2.Text = "0.00";
            lblOldDPMonthlyHidden.Text = "0.00";
            lblOldDPDueDate.Text = "0.00";
            lblOldLBAmount.Text = "0.00";


            //########################################################################
            //############## THIRD TAB - NEW COMPUTATIONS  ###########################
            //########################################################################

            lblNewDasAmt.Text = "0.00";
            lblNewDiscount.Text = "0.00";
            lblNewNetTCP.Text = "0.00";

            lblNewAddMiscFees.Text = "0.00";
            lblNewNetMiscFee.Text = "0.00";

            lblNewReserveFee.Text = "0.00";

            lblNewTCPFinScheme.Text = "";
            lblNewDownPayment.Text = "0.00";
            lblNewReserveFee2.Text = "0.00";
            lblNewBalance.Text = "0.00";
            lblNewDPTerms.Text = "";
            lblNewDPMonthly.Text = "0.00";
            lblNewLoanableBalance.Text = "0.00";

            lblNewMiscFees.Text = "0.00";
            lblNewmiscFeeOriginal.Text = "0.00";
            lblNewMiscDPTerms.Text = "0";
            lblNewMiscDPMonthly.Text = "0.00";

            lblTotalPaidAmount.Text = "0.00";
            lblTotalPaidAmountMisc.Text = "0.00";
            lblNewMiscFeePaid.Text = "0.00";
            lblNewMiscFeePaid2.Text = "0.00";

            txtPenaltyAmount.Text = "0.00";
            //lblNetPaidAmount.Text = "0.00";

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
            TextBox txt = (TextBox)sender;
            double val = 0;
            val = SystemClass.TextIsZero(txt.Text);

            txt.Text = SystemClass.ToCurrency(val.ToString());
            DataTable dt = ws.GetHouseDetails(tProjCode.Value, tBlock.Value, tLot.Value, txtFinancingMisc.Value).Tables["GetHouseDetails"];
            if (DataAccess.Exist(dt) == true)
            {
                //GetHouseDetails(dt, 1);
                GetMiscDetails(dt);
            }
            Compute();
        }


        void ClearBP()
        {
            txtLastName.Value = "";
            txtFirstName.Value = "";
            txtMiddleName.Value = "";
            txtBirthday.Value = "";
            ViewState["Broker"] = "";
            ViewState["SalesManager"] = "";
            tNatureofEmp.Disabled = true;
            tNatureofEmp.Value = "";
            tTypeOfId.Disabled = true;
            tTypeOfId.Value = "";
            tIDNo.Value = "";
            //tSalesType.Value = "";
            txtCardCode.Value = "";
        }
        void BoolBP(bool val)
        {
            txtLastName.Disabled = val;
            txtFirstName.Disabled = val;
            txtMiddleName.Disabled = val;
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
        //public void dueDateChanges()
        //{
        //    ViewState["UpdatedDPDueDate"] = 1;
        //    DateTime date1 = Convert.ToDateTime(dtpDueDate.Text);
        //    string day1 = date1.Day.ToString();
        //    ddlDPDay.SelectedValue = day1.ToString();
        //    Compute();
        //}
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



                // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                // @@@@@@@@@@@@@@@@@@@@@@@@@@ 10-27-2022 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                // @@@@@@@@@@@@@@@@@@@@@@@@@@ ADDITIONAL CONDITIONS FOR RESTRUCTURING @@@@@@@@@@@
                // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

                //visibility for "Retain Monthly Amortization"
                // 2023-03-15 : ALWAYS HIDE RETAIN MONTHLY AMORT OPTION; DEFAULT NO.
                //if (double.Parse(txtFactorRate1.Text) > 0)
                //{
                //    divRetainMonthlyAmort.Visible = true;
                //    if (ddlRetainMonthlyAmort.SelectedValue == "NO")
                //    {
                //        ddlRetainMonthlyAmort.SelectedValue = "NO";
                //    }
                //    else
                //    {
                //        ddlRetainMonthlyAmort.SelectedValue = "YES";
                //    }
                //}
                //else
                //{
                //    divRetainMonthlyAmort.Visible = false;
                //    ddlRetainMonthlyAmort.SelectedValue = "NO";
                //}


                // REMOVE TOTAL PAYMENT FOR COMPUTATION OF TOTAL NET WHEN UPDATE AMORT BALANCE IS YES
                double totalPayment = 0;
                if (ddlUpdateAmortBalance.Text == "YES")
                {
                    totalPayment = Convert.ToDouble(lblTotalPaidAmount.Text);

                    // ALWAYS REMOVE RESERVATION FEE FOR RESTRUCTURING
                    tResrvFee.Text = "0.00";
                    lblNewReserveFee.Text = "0.00";
                    lblNewReserveFee2.Text = "0.00";


                    lblNewLessTCPPayment.Text = lblTotalPaidAmount.Text;
                    lblNewLessMiscPayment.Text = lblTotalPaidAmountMisc.Text;
                    divTCPPaymentDisplay.Visible = true;
                }
                else
                {
                    tResrvFee.Text = hReservationFee.Value;
                    lblNewReserveFee.Text = hReservationFee.Value;
                    lblNewReserveFee2.Text = hReservationFee.Value;
                    divTCPPaymentDisplay.Visible = false;


                    lblNewLessTCPPayment.Text = "0.00";
                    lblNewLessMiscPayment.Text = "0.00";
                }
                // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@




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
                double discountAmount = double.Parse(string.IsNullOrWhiteSpace(txtDiscountAmount.Text) ? "0" : txtDiscountAmount.Text);
                double discPrcent = double.Parse(string.IsNullOrWhiteSpace(txtDiscPercent.Text) ? "0" : txtDiscPercent.Text);
                double AddChargeAmt = double.Parse(string.IsNullOrWhiteSpace(txtMiscFees.Text) ? "0" : txtMiscFees.Text);
                string housestat = txtProductType.Value;
                int updatedDPDueDate = int.Parse(string.IsNullOrWhiteSpace(ViewState["UpdatedDPDueDate"].ToString()) ? "0" : ViewState["UpdatedDPDueDate"].ToString());
                int LTerms = int.Parse(string.IsNullOrWhiteSpace(txtLTerms.Text) ? "0" : txtLTerms.Text);
                string RetType = tRetType.Value;

                int MiscDPTerms = int.Parse(string.IsNullOrWhiteSpace(lblNewMiscDPTerms.Text) ? "0" : lblNewMiscDPTerms.Text);
                //2023-05-17 : NO MORE UPDATING TO 1 WHEN TERM IS 0
                //int MiscLBTerm = int.Parse(string.IsNullOrWhiteSpace(lblNewMiscLBTerms.Text) ? "1" : lblNewMiscLBTerms.Text);
                int MiscLBTerm = int.Parse(string.IsNullOrWhiteSpace(lblNewMiscLBTerms.Text) ? "0" : lblNewMiscLBTerms.Text);


                if (LTerms == 0)
                {
                    LTerms = 1;
                }

                //2023-05-17 : NO MORE UPDATING TO 1 WHEN TERM IS 0
                //if (MiscLBTerm == 0)
                //{
                //    MiscLBTerm = 1;
                //}
                //if (MiscDPTerms == 0)
                //{
                //    MiscDPTerms = 1;
                //}

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
                                     ResrvFee, discountAmount, //8
                                     ddlDiscBased.Text, int.Parse(string.IsNullOrWhiteSpace(txtDPTerms.Text) ? "1" : txtDPTerms.Text), //10
                                     LTerms, "Y", //12

                                     //PARAMETERS FOR COMPSHEET  
                                     dpamount, discPrcent, //14
                                     AddChargeAmt, ddlAllowed.Text, //16
                                     tSalesType.Value, txtProductType.Value, //18
                                     tProjCode.Value, updatedDPDueDate, //20
                                     tDocDate.Text, Convert.ToDateTime(dtpDueDate1).Day.ToString(), //22

                                     txtDPDueDate.Text, Convert.ToInt32(ViewState["UpdatedDPDueDate"]), //24
                                     factorRate, RetType, //26
                                     txtAdjLot.Value, MiscDPTerms,
                                     decimal.Parse(lblNewMiscDPAmount.Text), MiscLBTerm,
                                     Convert.ToDouble(lblTotalPaidAmount.Text),
                                     ddlUpdateAmortBalance.Text, ddlRetainMonthlyAmort.Text,
                                     Convert.ToDouble(lblTotalPaidAmountMisc.Text)
                                     ).Tables[0];

                if (DataAccess.Exist(dt) == true)
                {
                    ViewState["OriginalNETDAS"] = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "OriginalNetDAS", "0"));
                    tODas.Value = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "ODas", "0"));
                    lblNewDasAmt.Text = tODas.Value;   //DAS Amount
                    lblPromoDiscn.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "PromoDisc", "0"));  //Less Promo: Discount 


                    //DISCOUNT 
                    lblNewDiscount.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "Discount", "0"));
                    //NET DAS = Sum of DAS Amount, LessPromo Discount, and Discount
                    lblNewNetDAS.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "NetDAS", "0"));
                    //NET DAS second part
                    //lblNetDAS2n.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "NetDAS2", "0"));
                    //Add: Misc Charges
                    //lblAddMiscChargesn.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "AddMiscCharges", "0"));

                    //FACTOR RATE
                    //txtFactorRate.Value = (string)DataAccess.GetData(dt, 0, "FactorRate", "0");

                    //VAT
                    lblNewVAT.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "FinalVAT", "0"));
                    //NET TCP = Sum of netDas, AddMiscCharges, and vAT
                    lblNewNetTCP.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "NetDAS", "0"));
                    tNetDas.Value = lblNewNetTCP.Text;



                    //lblNetTCPn.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "NewNETTCP", "0"));

                    //TCP Breakdown: Down Payment
                    lblNewDownPayment.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "initDP", "0"));
                    tDPAmount.Text = lblNewDownPayment.Text;

                    ////TCP Breakdown: Misc Charges
                    //lblNewMiscFees.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "CSAddMisc", "0"));

                    // TCP Breakdown : DP MONTHLY
                    lblNewDPMonthly.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "initMonthly1", "0"));


                    //TCP Breakdown : DP DUE DATE 1
                    //int dtpDueDateVisible = Convert.ToInt32(DataAccess.GetData(dt, 0, "dtpDueDateVisible", "0"));
                    //if (dtpDueDateVisible == 0)
                    //{ dtpDueDate.Visible = false; }
                    //else { dtpDueDate.Visible = true; }
                    var date1 = (string)DataAccess.GetData(dt, 0, "initDueDate1", "");
                    if (date1 != "-") { date1 = Convert.ToDateTime((string)DataAccess.GetData(dt, 0, "initDueDate1", "")).ToString("yyyy-MM-dd"); }
                    lblNewDPDueDate.Text = date1;
                    txtDPDueDate.Text = Convert.ToDateTime(DataAccess.GetData(dt, 0, "initDueDate1", "")).ToString("yyyy-MM-dd");
                    //dtpDueDate.Text = Convert.ToDateTime((string)DataAccess.GetData(dt, 0, "dtpDueDate", "")).ToString("yyyy-MM-dd");
                    ViewState["UpdatedDPDueDate"] = 0;



                    //TCP Breakdown : Additional Charges
                    //lblAddChargesn.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "NewAdditionalCharges", "0"));

                    //TCP Breakdown : LB Due Date Due Date 2
                    var date2 = (string)DataAccess.GetData(dt, 0, "initDueDate2", "");
                    if (date2 != "-")
                    {
                        date2 = Convert.ToDateTime((string)DataAccess.GetData(dt, 0, "initDueDate2", "")).ToString("yyyy-MM-dd");
                    }
                    lblDueDate2n.Text = date2;

                    if (double.Parse(lblNewLoanableBalance.Text) > 0)
                    {
                        lblNewLBDueDate.Text = date2;
                    }


                    //TCP Breakdown : Loanable Amount
                    lblNewLoanableBalance.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "initFinance", "0"));
                    txtLoanableBalance.Text = lblNewLoanableBalance.Text;

                    //LB TERM TEXTBOX TO LABEL
                    lblNewLBTerms.Text = txtLTerms.Text;

                    //TCP Breakdown : LB Monthly 2  
                    if (double.Parse(lblNewLBTerms.Text) > 0)
                    {
                        lblNewLBMonthly.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "monthly2", "0"));
                    }
                    else
                    {
                        lblNewLBMonthly.Text = "0.00";
                    }


                    //TCP Breakdown : LB Due Date 3
                    var date3 = (string)DataAccess.GetData(dt, 0, "initDueDate3", "");
                    if (date3 != "-")
                    {
                        date3 = Convert.ToDateTime((string)DataAccess.GetData(dt, 0, "initDueDate3", "").ToString().Trim()).ToString("yyyy-MM-dd");
                    }
                    //lblNewLBDueDate.Text = date3;

                    //Additional Chargese
                    txtAddChargeAmount.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "AddAdditionalCharge", "0"));

                    //Terms
                    //txtMiscTerms.Text = txtDPTerms.Text;
                    if (double.Parse(lblNewBalance.Text) > 0)
                    {
                        lblNewDPTerms.Text = txtDPTerms.Text;
                    }
                    else
                    {
                        lblNewDPTerms.Text = "0";
                        txtDPTerms.Text = "0";
                    }





                    //Balance on Equity  ess.GetData(dt, 0, "PDBalance", "0"));
                    lblNewBalance.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "PDBalance", "0")).ToString();
                    tPDBalance.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "PDBalance", "0")).ToString();













                    //MISC DUE DATE
                    lblNewMiscDueDate.Text = tMiscDocDate.Text;

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
                                double miscmonthly = double.Parse(DataAccess.GetData(dtProd, 0, "MiscFee", "0").ToString());
                                lblNewMiscDPMonthly.Text = SystemClass.ToCurrency(miscmonthly.ToString()).ToString();
                                lblNewMiscDPTerms.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dtProd, 0, "MiscFee", "0")).ToString();
                                txtMiscFees.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dtProd, 0, "MiscFee", "0")).ToString();
                                lblNewAddMiscFees.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dtProd, 0, "MiscFee", "0")).ToString();
                                txtMiscMonthly.Text = SystemClass.ToCurrency(miscmonthly.ToString()).ToString();

                                //2023-06-27 : TRIGGER FOR 12,500 MISC FEE
                                lblNewmiscFeeOriginal.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dtProd, 0, "MiscFee", "0")).ToString();
                                lblNewMiscFees.Text = SystemClass.ToCurrency((double.Parse((string)DataAccess.GetData(dtProd, 0, "MiscFee", "0")) - double.Parse(lblTotalPaidAmountMisc.Text)).ToString()).ToString();
                                lblNewMiscDPAmount.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dtProd, 0, "MiscFee", "0")).ToString();

                                //2023-06-29: ADDED FIELDS FOR FLAT AMOUNT
                                lblNewMiscBalanceOnEquity.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dtProd, 0, "MiscFee", "0")).ToString();

                            }
                        }
                        else
                        {
                            //2023-10-19 - NEW WAY TO GET MISC FEES WHEN RETITLING TYPE = 'BUYERS'
                            {
                                string miscmonthly = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "CSAddMisc", "0")).ToString();
                                lblNewMiscDPMonthly.Text = miscmonthly;
                                lblNewMiscDPTerms.Text = miscmonthly;
                                txtMiscFees.Text = miscmonthly;
                                lblNewAddMiscFees.Text = miscmonthly;
                                txtMiscMonthly.Text = miscmonthly;

                                // MISC DP AMOUNT
                                lblNewmiscFeeOriginal.Text = miscmonthly;
                                lblNewMiscFees.Text = miscmonthly;
                                lblNewMiscDPAmount.Text = miscmonthly;

                                //2023-06-29: ADDED FIELDS FOR FLAT AMOUNT
                                lblNewMiscBalanceOnEquity.Text = SystemClass.ToCurrency((double.Parse(miscmonthly) - double.Parse(lblTotalPaidAmountMisc.Text)).ToString()).ToString();

                            }
                        }


                        //2023-10-19 : TERMS ARE ORIGINALLY LIKE THIS BEFORE CATERING TANAY'S REQUEST
                        {
                            lblNewMiscDPTerms.Text = "1";
                            txtMiscTerms.Text = "1";

                            lblNewMiscLBAmount.Text = "0.00";
                            lblNewMiscLBTerms.Text = "0";
                            lblNewMiscLBMonthly.Text = "0.00";
                        }
                    }
                    else
                    {
                        txtMiscMonthly.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "PDMiscFee", "0")).ToString();
                        lblNewAddMiscFees.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "CSAddMisc", "0")).ToString();
                        lblNewmiscFeeOriginal.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "CSAddMisc", "0")).ToString();
                        lblNewMiscFees.Text = SystemClass.ToCurrency((double.Parse((string)DataAccess.GetData(dt, 0, "CSAddMisc", "0")) - double.Parse(lblTotalPaidAmountMisc.Text)).ToString()).ToString();
                        lblNewMiscDPTerms.Text = txtMiscTerms.Text;
                        lblNewMiscDPMonthly.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "MiscDPMonthly", "0").ToString()).ToString();

                        txtMiscFees.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "CSAddMisc", "0")).ToString();

                        //MISC LB AMOUNT
                        if (double.Parse(lblNewMiscLBTerms.Text) > 0)
                        {
                            lblNewMiscLBAmount.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "MiscLoanable", "0")).ToString();

                            //2023-05-17 : REMOVED DIVISION FROM TERM SINCE IT'S ALREADY IN THE QUERY
                            //lblNewMiscLBMonthly.Text = SystemClass.ToCurrency((double.Parse((string)DataAccess.GetData(dt, 0, "MiscLBMonthly", "0")) / double.Parse(lblNewMiscLBTerms.Text)).ToString()).ToString();
                            lblNewMiscLBMonthly.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "MiscLBMonthly", "0"));
                        }
                        else
                        {
                            lblNewMiscLBAmount.Text = "0.00";
                            lblNewMiscLBMonthly.Text = "0.00";
                        }
                    }







                    //Monthly
                    // CHANGES FOR NEW MISC AMORTIZATION (2023-03-14)
                    //txtDPMonthly.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "DPMonthly", "0")).ToString();
                    //lblNewDPMonthly.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "DPMonthly", "0")).ToString();
                    txtDPMonthly.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "TCPMonthly", "0")).ToString();
                    lblNewDPMonthly.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "TCPMonthly", "0")).ToString();

                    //lblMiscMonthly2n.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "MiscMonthly", "0")).ToString();
                    //txtMiscMonthly.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "PDMiscFee", "0")).ToString();

                    //Loanable Balance
                    //txtLoanableBalance.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "PDLoanable", "0")).ToString();
                    txtLoanableBalance.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "TCPLoanable", "0")).ToString();
                    lblNewLoanableBalance.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "TCPLoanable", "0")).ToString();

                    //Add Misc Fees
                    //lblAddMiscFees2n.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "CSAddMisc", "0")).ToString();


                    //Computation Sheet Total
                    //lblNewCompTotal.Text = SystemClass.ToCurrency((string)DataAccess.GetData(dt, 0, "CSTotal", "0")).ToString();
                    double NetMiscFee = double.Parse(lblNewAddMiscFees.Text) - double.Parse(lblNewLessMiscPayment.Text);
                    lblNewNetMiscFee.Text = SystemClass.ToCurrency(Convert.ToString(NetMiscFee > 0 ? NetMiscFee : 0).ToString());

                    //Financial Scheme on Computation
                    lblNewTCPFinScheme.Text = tFinancing.Value;


                    ViewState["tDPPercent"] = tDPPercent.Text;
                    ViewState["txtDiscPercent"] = txtDiscPercent.Text;

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
            tMonthlyAmort2.Text = SystemClass.ToCurrency(lblOldLBMonthly.Text);
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
                Query = $@"SELECT ""U_Code"" ""Code"",""U_Name"" ""Name"" FROM ""@FSC1"" WHERE ""Code"" = '{tProjCode.Value}'";
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
            LinkButton GetID = (LinkButton)sender;
            string Code = GetID.CommandArgument;

            string GrpCode = (string)ViewState["btnID"];

            if (GrpCode == "bNatureofEmp")
            {
                ViewState["tNatureofEmp"] = Code;

                if (Code == "OTH")
                { tNatureofEmp.Disabled = false; tNatureofEmp.Value = ""; }
                else { tNatureofEmp.Disabled = true; tNatureofEmp.Value = ws.GetOLSTName(Code); }

                tTypeOfId.Disabled = true;
                tIDNo.Value = "";
                tIDNo.Disabled = false;
                tTypeOfId.Value = "";
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
                ViewState["tFinancing"] = Code;
                //bGenerate_ServerClick(sender, e);
                DataTable dt = ws.GetHouseDetails(tProjCode.Value, tBlock.Value, tLot.Value, tFinancing.Value).Tables["GetHouseDetails"];
                if (DataAccess.Exist(dt) == true)
                {
                    GetHouseDetails(dt);
                }
                Compute();
            }
            else if (GrpCode == "bFinancingMisc")
            {
                txtFinancingMisc.Value = ws.GetOLSTName(Code);
                lblNewMiscFinancingScheme.Text = txtFinancingMisc.Value;

                ViewState["tFinancingMisc"] = Code;

                DataTable dt = ws.GetHouseDetails(tProjCode.Value, tBlock.Value, tLot.Value, txtFinancingMisc.Value).Tables["GetHouseDetails"];
                if (DataAccess.Exist(dt) == true)
                {
                    //GetHouseDetails(dt, 1);
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
        protected void bSearch_ServerClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Value))
            {
                ViewState["dtBuyers"] = hana.GetData($"CALL sp_Search ('3','{txtSearch.Value}','{Session["UserID"]}');", hana.GetConnection("SAOHana"));
                LoadData(gvBuyers, "dtBuyers");
            }
            else
            { LoadFromFindBtn(); }
        }
        protected void bSelectBuyer_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton GetID = (LinkButton)sender;
                int DocEntry = int.Parse(GetID.CommandArgument);
                lblDocEntry.Text = DocEntry.ToString();
                ViewState["QuoteDocEntry"] = DocEntry;


                DataTable dtUserAccess1 = (DataTable)Session["UserAccess"];
                var RES = dtUserAccess1.Select($"CodeEncrypt= 'RES'");

                string qryRestructuringType = $@"select IFNULL(""RestructuringType"",'') ""RestructuringType"" From oqut where ""DocEntry"" = '{DocEntry}'";
                DataTable dtRestructuringType = hana.GetData(qryRestructuringType, hana.GetConnection("SAOHana"));

                if (!RES.Any() && (int)Session["UserID"] != 1)
                {
                    //if (DataAccess.GetData(dtRestructuringType, 0, "RestructuringType", "").ToString() != "")
                    //{
                    LoadQuotationDetails(DocEntry);
                    //}
                    //else
                    //{
                    //    ScriptManager.RegisterStartupScript(this, GetType(), "MsgBuyers_Hide", "MsgBuyers_Hide();", true);
                    //    alertMsg("You are not allowed to restructure multiple times. Please contact administrator.", "warning");
                    //}
                }
                else
                {
                    LoadQuotationDetails(DocEntry);
                }




            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "MsgBuyers_Hide", "MsgBuyers_Hide();", true);
                alertMsg(ex.Message, "error");
            }
        }


        void LoadQuotationDetails(int DocEntry)
        {
            try
            {


                lblFinish.InnerText = "Update";

                DataTable dt = new DataTable();
                dt = ws.GetQuotationByID(DocEntry).Tables["GetQuotationByID"];
                if (DataAccess.Exist(dt) == true)
                {
                    ////CheckBox DOCUMENT
                    //DataTable dtDoc = new DataTable();
                    //string docqry = $@"select ""DocName"" from qdoc where ""CardCode"" = '{DataAccess.GetData(dt, 0, "CardCode", "")}' AND ""DocId"" = '29' AND ""DocEntry"" = '{DocEntry}'";
                    //dtDoc = hana.GetData(docqry, hana.GetConnection("SAOHana"));
                    //if (DataAccess.Exist(dtDoc) != true && (string)DataAccess.GetData(dt, 0, "BusinessType", "Individual") == "Corporation")
                    //{
                    //    ScriptManager.RegisterStartupScript(this, GetType(), "MsgBuyers_Hide", "MsgBuyers_Hide();", true);
                    //    alertMsg("This Buyer is missing Board Resolution document requirement", "error");
                    //    ClearProfileTab();
                    //    ClearHouseDetails("Project");
                    //    clearComputationTabs();
                    //    clearPage();
                    //    clearRestructuringDocuments();
                    //    PrevTab("HouseDetails");
                    //    ScriptManager.RegisterStartupScript(this, GetType(), "clear", "ResetTextBox()", true);

                    //}
                    //else
                    //{









                    //####################################################
                    //############## FIRST TAB PROFILE  ##################
                    //####################################################

                    ViewState["DocNum"] = (string)DataAccess.GetData(dt, 0, "DocNum", "");
                    lblName.Text = (string)DataAccess.GetData(dt, 0, "FullName", "");
                    lblID.Text = (string)DataAccess.GetData(dt, 0, "CardCode", "");
                    lblDocNum.Text = (string)DataAccess.GetData(dt, 0, "DocNum", "");

                    lblLastName.Text = (string)DataAccess.GetData(dt, 0, "LastName", "");
                    lblBusinessType.Text = (string)DataAccess.GetData(dt, 0, "BusinessType", "Individual");
                    lblFirstName.Text = (string)DataAccess.GetData(dt, 0, "FirstName", "");
                    lblMiddleName.Text = (string)DataAccess.GetData(dt, 0, "MiddleName", "");

                    //2023-05-24 : ADD LOADING OF CO-BORROWER
                    txtComaker1.Value = (string)DataAccess.GetData(dt, 0, "Comaker", "");

                    lblCompanyName.Text = (string)DataAccess.GetData(dt, 0, "CompanyName", "");
                    lblBirthday.Text = (string)DataAccess.GetData(dt, 0, "BirthDay", "");
                    string qry = $@"select ""Name"" from olst where ""GrpCode"" = 'NE' AND ""Code"" = '{DataAccess.GetData(dt, 0, "NatureEmp", "")}'";
                    DataTable dtX = hana.GetData(qry, hana.GetConnection("SAOHana"));
                    lblNatureofEmployment.Text = (string)DataAccess.GetData(dtX, 0, "Name", "");
                    qry = $@"select ""Name"" from olst where ""GrpCode"" = 'ID' AND ""Code"" = '{DataAccess.GetData(dt, 0, "IDType", "")}'";
                    dtX = hana.GetData(qry, hana.GetConnection("SAOHana"));
                    lblTypeofID.Text = (string)DataAccess.GetData(dtX, 0, "Name", "");
                    lblIDNo.Text = (string)DataAccess.GetData(dt, 0, "IDNo", "");
                    lblTaxClassification.Text = (string)DataAccess.GetData(dt, 0, "TaxClassification", "");
                    lblTIN.Text = (string)DataAccess.GetData(dt, 0, "TIN", "");

                    loadDivisionsForNames(lblBusinessType.Text);

                    DataTable dtEmp = hana.GetData($@"SELECT ""POSCode"", ""EmpCode"", ""EmpName"", ""POSCode"" FROM ""QUT5"" WHERE ""DocEntry"" = {DocEntry}", hana.GetConnection("SAOHana"));
                    lblEmployeeID.Text = DataAccess.GetData(dtEmp, 0, "EmpCode", "").ToString();
                    lblEmployeeName.Text = DataAccess.GetData(dtEmp, 0, "EmpName", "").ToString();
                    lblEmployeePosition.Text = DataAccess.GetData(dtEmp, 0, "POSCode", "").ToString();

                    lblOldLBDueDate.Text = Convert.ToDateTime(DataAccess.GetData(dt, 0, "LDueDate", "")).ToString("yyyy-MM-dd");
                    lblNewLBDueDate.Text = Convert.ToDateTime(DataAccess.GetData(dt, 0, "LDueDate", "")).ToString("yyyy-MM-dd");

                    hSalesAgentID.Value = DataAccess.GetData(dtEmp, 0, "EmpCode", "").ToString();












                    //########################################################################
                    //############## SECOND TAB - OLD COMPUTATIONS  ##########################
                    //########################################################################

                    lblOldDASAmt.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "OTcp", "0").ToString());
                    lblOldDiscount.Text = SystemClass.ToCurrency((Convert.ToDouble(DataAccess.GetData(dt, 0, "DiscAmount", "0").ToString()) * -1).ToString());
                    lblOldNetTCP.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "Das", "").ToString());

                    lblOldAddMiscFees.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "AddMiscFees", "0").ToString());
                    lblOldCompTotal.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "CSTotal", "0").ToString());
                    lblOldReserveFee.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "ResrvFee", "0").ToString()); ;

                    lblOldTCPFinScheme.Text = (string)DataAccess.GetData(dt, 0, "FinancingScheme", "");
                    lblOldDownPayment.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "TCPDownpayment", "").ToString());
                    lblOldReserveFee2.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "ResrvFee", "0").ToString());
                    lblOldBalance.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "TCPBalanceOnEquity", "0").ToString());
                    lblOldDPTerms.Text = SystemClass.NoDecimal(DataAccess.GetData(dt, 0, "DPTerms", "0").ToString());
                    lblOldDPMonthly.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "TCPMonthly", "0").ToString());
                    lblOldLoanableBalance.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "TCPLoanableBalance", "0").ToString());


                    lblOldMiscDueDate.Text = Convert.ToDateTime(DataAccess.GetData(dt, 0, "MiscDueDate", "1999-12-31")).ToString("yyyy-MM-dd");
                    lblOldMiscFees.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "AddMiscFees", "0").ToString());
                    lblOldMiscDPTerms.Text = SystemClass.NoDecimal(DataAccess.GetData(dt, 0, "MiscDPTerms", "0").ToString());
                    lblOldMiscDPMonthly.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "MiscFeesMonthly", "0").ToString());


                    //HIDDEN FIELDS
                    lblOldNetDAS.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "Das", "0").ToString());
                    //tNetDas.Value = SystemClass.ToCurrency((grossTCP + vat).ToString());
                    lblOldNetDAS2.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "Das", "").ToString());
                    lblOldVAT.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "Vat", "").ToString());
                    lblOldAddMiscCharges.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "OMisc", "").ToString());
                    //lblOldAddMiscCharges2.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "OMisc", "").ToString());
                    lblOldDPMonthlyHidden.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "MonthlyDP", "").ToString());
                    lblOldDPDueDate.Text = Convert.ToDateTime(DataAccess.GetData(dt, 0, "DPDueDate", "")).ToString("yyyy-MM-dd");
                    lblOldLBAmount.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "LAmount", "").ToString());
                    lblOldLBTerms.Text = SystemClass.NoDecimal(DataAccess.GetData(dt, 0, "LTerms", "0").ToString());
                    lblOldLBMonthly.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "MonthlyLB", "").ToString());


                    lblOldMiscDPAmount.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "MiscDPAmount", "0").ToString());
                    lblOldMiscLBAmount.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "MiscLBAmount", "0").ToString());
                    lbloldMiscLBTerms.Text = DataAccess.GetData(dt, 0, "MiscLBTerms", "0").ToString();
                    lblOldMiscLBMonthly.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "MiscLBMonthly", "0").ToString());








                    //########################################################################
                    //############## THIRD TAB - NEW COMPUTATIONS  ###########################
                    //########################################################################

                    lblNewDasAmt.Text = lblOldDASAmt.Text;
                    lblNewDiscount.Text = lblOldDiscount.Text;
                    lblNewNetTCP.Text = lblOldNetTCP.Text;

                    lblNewAddMiscFees.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "AddMiscFees", "0").ToString());

                    lblNewReserveFee.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "ResrvFee", "0").ToString()); ;

                    lblNewTCPFinScheme.Text = (string)DataAccess.GetData(dt, 0, "FinancingScheme", "");
                    lblNewDownPayment.Text = lblOldDownPayment.Text;
                    lblNewReserveFee2.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "ResrvFee", "0").ToString()); ;
                    lblNewBalance.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "TCPBalanceOnEquity", "0").ToString());
                    lblNewDPTerms.Text = SystemClass.NoDecimal(DataAccess.GetData(dt, 0, "DPTerms", "0").ToString());
                    lblNewDPMonthly.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "TCPMonthly", "0").ToString());
                    lblNewLoanableBalance.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "TCPLoanableBalance", "0").ToString());

                    lblNewMiscDueDate.Text = Convert.ToDateTime(DataAccess.GetData(dt, 0, "MiscDueDate", "1999-12-31")).ToString("yyyy-MM-dd");
                    lblNewMiscFees.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "AddMiscFees", "0").ToString());
                    lblNewmiscFeeOriginal.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "AddMiscFees", "0").ToString());
                    lblNewMiscDPTerms.Text = SystemClass.NoDecimal(DataAccess.GetData(dt, 0, "MiscDPTerms", "0").ToString());
                    lblNewMiscDPMonthly.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "MiscFeesMonthly", "0").ToString());

                    //GET ALL PAYMENTS  
                    //string SurchargePropertyFieldName = ConfigSettings.SurchargeProperty"].ToString();
                    //DataTable itemDetails = ws.GetARInvoiceDetails(SurchargePropertyFieldName).Tables[0];


                    //####### 2022-09-15 

                    //string surchageItem = DataAccess.GetData(dt, 0, "ItemCode", "").ToString();
                    //string qryPenalty = $@" select SUM(A.""DocTotal"") ""DocTotal"" From oinv a inner join inv1 b on a.""DocEntry"" = b.""DocEntry"" 
                    //                            where b.""ItemCode"" = '{surchageItem}' and a.""U_BlockNo"" = '{(string)DataAccess.GetData(dt, 0, "Block", "")}' and 
                    //                            a.""U_LotNo"" = '{(string)DataAccess.GetData(dt, 0, "Lot", "")}' and  a.""Project"" = '{(string)DataAccess.GetData(dt, 0, "ProjCode", "")}' 
                    //                            and a.""CANCELED"" = 'N' and  a.""isIns"" <> 'Y' AND  IFNULL(a.""U_DreamsQuotationNo"",'') = '{ViewState["DocNum"].ToString()}'";
                    //DataTable dtPenalties = hana.GetData(qryPenalty, hana.GetConnection("SAPHana"));
                    //double Totalsurcharge = Convert.ToDouble(DataAccess.GetData(dtPenalties, 0, "DocTotal", "0").ToString());


                    //DataTable dtPayments = hana.GetData($@"SELECT SUM(""AmountPaid"") ""TotalPaid"" FROM QUT1 WHERE 
                    //                                ""DocEntry"" = {lblDocEntry.Text} AND IFNULL(""Cancelled"",'N') = 'N' AND ""PaymentType"" <> 'MISC' ", hana.GetConnection("SAOHana"));

                    //double Totalpayments = Convert.ToDouble(DataAccess.GetData(dtPayments, 0, "TotalPaid", "0").ToString());


                    DataTable dtPayments = ws.GetQuotationData(DocEntry).Tables["QuotationData"];

                    double Totalpayments = Convert.ToDouble(DataAccess.GetData(dtPayments, 0, "TotalPayment", "0").ToString());
                    lblTotalPaidAmount.Text = SystemClass.ToCurrency((Totalpayments).ToString());
                    lblNewTotalTCPPaymentDisplay.Text = SystemClass.ToCurrency((Totalpayments).ToString());

                    if (ddlUpdateAmortBalance.Text == "YES")
                    {
                        divTCPPaymentDisplay.Visible = true;
                    }
                    else
                    {
                        divTCPPaymentDisplay.Visible = false;
                    }
                    //DataTable dtPaymentsMisc = hana.GetData($@"SELECT SUM(""AmountPaid"") ""TotalPaid"" FROM QUT1 WHERE 
                    //                                ""DocEntry"" = {lblDocEntry.Text} AND IFNULL(""Cancelled"",'N') = 'N'  AND ""PaymentType"" = 'MISC' ", hana.GetConnection("SAOHana"));

                    double TotalpaymentsMisc = Convert.ToDouble(DataAccess.GetData(dtPayments, 0, "MiscPayment", "0").ToString());
                    lblTotalPaidAmountMisc.Text = SystemClass.ToCurrency((TotalpaymentsMisc).ToString());
                    lblNewMiscFeePaid.Text = lblTotalPaidAmountMisc.Text;
                    lblNewMiscFeePaid2.Text = lblTotalPaidAmountMisc.Text;

                    //lblTotalMISCPaymentDisplay.Text = SystemClass.ToCurrency((TotalpaymentsMisc).ToString());
                    txtPenaltyAmount.Text = "0.00";


                    if (ddlUpdateAmortBalance.Text == "YES")
                    {
                        lblNewLessTCPPayment.Text = SystemClass.ToCurrency((Totalpayments * -1).ToString());
                        //lblNewLessMiscPayment.Text = SystemClass.ToCurrency((TotalpaymentsMisc * -1 ).ToString());
                        lblNewLessMiscPayment.Text = SystemClass.ToCurrency((TotalpaymentsMisc).ToString());

                    }
                    else
                    {
                        lblNewLessTCPPayment.Text = "0.00";
                        lblNewLessMiscPayment.Text = "0.00";
                    }

                    double NetMiscFee = double.Parse(lblNewAddMiscFees.Text) - double.Parse(lblNewLessMiscPayment.Text);
                    lblNewNetMiscFee.Text = SystemClass.ToCurrency(Convert.ToString(NetMiscFee > 0 ? NetMiscFee : 0).ToString());

                    computeNetTotalPaid();

                    //HIDDEN FIELDS
                    lblNewNetDAS.Text = lblOldNetDAS.Text;
                    //lblNetDAS2n.Text = lblOldNetDAS2.Text;


                    //COMPUTATION IN LEFT SIDE <-- NEW COMPUTATION

                    lblNewVAT.Text = lblOldVAT.Text;
                    //lblAddMiscChargesn.Text = lblOldAddMiscCharges.Text;
                    //lblAddMiscCharges2n.Text = lblOldAddMiscCharges2.Text;
                    lblNewDPMonthly.Text = lblOldDPMonthly.Text;
                    lblNewDPDueDate.Text = lblOldDPDueDate.Text;
                    lblNewLoanableBalance.Text = lblOldLoanableBalance.Text;


                    //lblFileName.Text = (string)DataAccess.GetData(dt, 0, "LetterReqDocument", "");
                    lblDueDate2n.Text = Convert.ToDateTime(DataAccess.GetData(dt, 0, "LDueDate", "")).ToString("yyyy-MM-dd");
                    lblNewLBTerms.Text = SystemClass.NoDecimal(DataAccess.GetData(dt, 0, "LTerms", "0").ToString());
                    txtLTerms.Text = lblNewLBTerms.Text;

                    lblNewLBMonthly.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "MonthlyLB", "").ToString());



                    lblNewMiscDPAmount.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "MiscDPAmount", "0").ToString());
                    lblNewMiscLBAmount.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "MiscLBAmount", "0").ToString());
                    lblNewMiscLBTerms.Text = DataAccess.GetData(dt, 0, "MiscLBTerms", "0").ToString();
                    lblNewMiscLBMonthly.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "MiscLBMonthly", "0").ToString());


























                    //############################################################
                    //############## FIELDS IN THE MAIN PANE TAB #################
                    //############################################################

                    tDocDate.Text = Convert.ToDateTime(DataAccess.GetData(dt, 0, "DocDate", "")).ToString("yyyy-MM-dd");
                    tProjCode.Value = (string)DataAccess.GetData(dt, 0, "ProjCode", "");

                    DataTable dt1 = ws.GetProjectDetails(tProjCode.Value).Tables["ProjectDetails"];



                    if (tProjCode.Value != "")
                    {
                        hBlockWidth.Value = (string)DataAccess.GetData(dt1, 0, "ImgWidth", "0");
                        hBlockHeight.Value = (string)DataAccess.GetData(dt1, 0, "ImgHeight", "0");
                    }
                    //DataTable dtVal = ws.GetProjectDetails(hPrjCode.Value).Tables["ProjectDetails"];

                    //2023-06-08 : GET PROJECT NAME FROM SAP
                    DataTable dtPrjName = ws.GetProjectDetailsSAP(tProjCode.Value).Tables["ProjectDetailsSAP"];
                    tProjName.Value = (string)DataAccess.GetData(dtPrjName, 0, "PrjName", "");

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
                    hideTermDiv(txtLoanType.Value);

                    tBank.Value = (string)DataAccess.GetData(dt, 0, "Bank", "");
                    tRetType.Value = DataAccess.GetData(dt, 0, "RetitlingType", "").ToString();
                    txtAdjLot.Value = DataAccess.GetData(dt, 0, "SoldWithAdjacentLot", "No").ToString();
                    txtLotNo.Value = SystemClass.NoDecimal(DataAccess.GetData(dt, 0, "AdjacentLotQuotationNo", "0").ToString());


                    tMiscDocDate.Text = Convert.ToDateTime(DataAccess.GetData(dt, 0, "MiscDueDate", tDocDate.Text)).ToString("yyyy-MM-dd");
                    txtFinancingMisc.Value = (string)DataAccess.GetData(dt, 0, "MiscFinancingScheme", "");
                    lblNewMiscFinancingScheme.Text = txtFinancingMisc.Value;







                    //###############################################################
                    //############## PAYMENT DETAILS IN MAIN PANE TAB #################
                    //###############################################################

                    tODas.Value = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "OTcp", "0").ToString());
                    txtDiscPercent.Text = SystemClass.ToDecimal(DataAccess.GetData(dt, 0, "DiscPercent", "0").ToString());
                    ViewState["txtDiscPercent"] = txtDiscPercent.Text;
                    txtDiscountAmount.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "DiscAmount", "0").ToString());
                    tNetDas.Value = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "Das", "0").ToString());

                    tDPPercent.Text = SystemClass.ToDecimal(DataAccess.GetData(dt, 0, "DPPercent", "0").ToString());
                    tDPAmount.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "DPAmount", "0").ToString());
                    tResrvFee.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "ResrvFee", "0").ToString());
                    hReservationFee.Value = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "ResrvFee", "0").ToString());
                    tPDBalance.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "DPBalanceOnEquity", "0").ToString());
                    txtDPTerms.Text = SystemClass.NoDecimal(DataAccess.GetData(dt, 0, "DPTerms", "0").ToString());
                    ViewState["tDPPercent"] = tDPPercent.Text;
                    //txtDPMonthly.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "MonthlyDP", "0").ToString()); //check
                    txtDPMonthly.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "TCPMonthly", "0").ToString()); //check

                    txtMiscFees.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "AddMiscFees", "0").ToString());
                    txtMiscTerms.Text = SystemClass.NoDecimal(DataAccess.GetData(dt, 0, "MiscDPTerms", "1").ToString());
                    txtMiscMonthly.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "PDMonthly", "0").ToString());

                    txtLoanableBalance.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "PDLoanableBalance", "0").ToString());
                    txtLTerms.Text = SystemClass.NoDecimal(DataAccess.GetData(dt, 0, "LTerms", "0").ToString());
                    txtFactorRate1.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "InterestRate", "0").ToString());



                    //HIDDEN FIELDS
                    hPrjCode.Value = (string)DataAccess.GetData(dt, 0, "ProjCode", "");
                    hHouseModel.Value = (string)DataAccess.GetData(dt, 0, "Model", "");
                    hLoanType.Value = (string)DataAccess.GetData(dt, 0, "LoanType", "");
                    hProductType.Value = (string)DataAccess.GetData(dt, 0, "AcctType", "");
                    hBlock.Value = (string)DataAccess.GetData(dt, 0, "Block", "");
                    hLot.Value = (string)DataAccess.GetData(dt, 0, "Lot", "");
                    hDPTerms.Value = SystemClass.NoDecimal(DataAccess.GetData(dt, 0, "DPTerms", "0").ToString());
                    hLBTerms.Value = SystemClass.NoDecimal(DataAccess.GetData(dt, 0, "LTerms", "0").ToString());
                    hLBDueDate.Value = Convert.ToDateTime(DataAccess.GetData(dt, 0, "LDueDate", "")).ToString("yyyy-MM-dd");
                    hCardCode.Value = (string)DataAccess.GetData(dt, 0, "CardCode", "");
                    hDPDueDate.Value = lblOldDPDueDate.Text;
                    hRetitlingType.Value = DataAccess.GetData(dt, 0, "RetitlingType", "").ToString();

                    ////Get VAT
                    //double vat = 0;
                    //double grossTCP = Convert.ToDouble(tODas.Value) - Convert.ToDouble(DataAccess.GetData(dt, 0, "LTerms", "0"));
                    //qry = $@"SELECT IFNULL(""U_ThresholdAmount"",0) Threshold FROM ""@PRODUCTTYPE"" WHERE ""Code"" = '{txtProductType.Value}'";
                    //DataTable dtThes = hana.GetData(qry, hana.GetConnection("SAPHana"));
                    //if (dtThes.Rows.Count > 0)
                    //{
                    //    double threshold = Convert.ToDouble(DataAccess.GetData(dtThes, 0, "Threshold", ""));
                    //    if (threshold <= grossTCP)
                    //    {
                    //        vat = grossTCP * 0.12;
                    //    }
                    //}


                    //txtGDI.Value = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "Gdi", "").ToString());




                    //############################################################
                    //############## BELOW THE TABS PANE #########################
                    //############################################################

                    //NEW FIELDS 





                    //COMPUTATION IN LEFT SIDE 





                    tBlock2.Value = tBlock.Value;
                    tLot2.Value = tLot.Value;


                    //GET HOUSE DETAILS FROM SAP
                    DataTable dtMiscDetails = ws.GetHouseDetails(tProjCode.Value, tBlock.Value, tLot.Value, txtFinancingMisc.Value).Tables["GetHouseDetails"];
                    if (DataAccess.Exist(dtMiscDetails) == true)
                    {
                        //GetHouseDetails(dt, 1);
                        GetMiscDetails(dtMiscDetails);
                    }

                    //COMPUTATION (CONSIDERING PAYMENTS)
                    Compute();

                    //GET HOUSE DETAILS FROM SAP
                    dtMiscDetails = ws.GetHouseDetails(tProjCode.Value, tBlock.Value, tLot.Value, txtFinancingMisc.Value).Tables["GetHouseDetails"];
                    if (DataAccess.Exist(dtMiscDetails) == true)
                    {
                        //GetHouseDetails(dt, 1);
                        GetMiscDetails(dtMiscDetails);
                    }

                    //COMPUTATION (CONSIDERING PAYMENTS)
                    Compute();



                    loadSchedules();

                    loadCoOwner();



                    CheckUserType();
                    loadRestructuringDocReq();

                    ScriptManager.RegisterStartupScript(this, GetType(), "MsgBuyers_Hide", "MsgBuyers_Hide();", true);
                    //}
                }
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "info");
            }
        }

        void computeNetTotalPaid()
        {
            double netPaid = Convert.ToDouble(lblTotalPaidAmount.Text) - Convert.ToDouble(txtPenaltyAmount.Text);
            double penaltyAmt = Convert.ToDouble(txtPenaltyAmount.Text);
            if (netPaid < 0)
            {
                netPaid = 0;
            }
            netPaid = SystemClass.TextIsZero(netPaid.ToString());
            penaltyAmt = SystemClass.TextIsZero(penaltyAmt.ToString());
            //lblNetPaidAmount.Text = SystemClass.ToCurrency(netPaid.ToString()).ToString();
            txtPenaltyAmount.Text = SystemClass.ToCurrency(penaltyAmt.ToString()).ToString();
        }


        protected void bSelectProject_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            ClearHouseDetails("Project");
            tProjCode.Value = GetID.CommandArgument;
            DataTable dt = new DataTable();
            dt = ws.GetProjectDetails(GetID.CommandArgument).Tables["ProjectDetails"];


            //2023-06-01 : GET PROJECT NAME FROM SAP INSTEAD
            DataTable dtSAP = new DataTable();
            dtSAP = ws.GetProjectDetailsSAP(tProjCode.Value).Tables["ProjectDetailsSAP"];
            tProjName.Value = (string)DataAccess.GetData(dtSAP, 0, "PrjName", "");


            hBlockWidth.Value = (string)DataAccess.GetData(dt, 0, "ImgWidth", "0");
            hBlockHeight.Value = (string)DataAccess.GetData(dt, 0, "ImgHeight", "0");
            tPhase.Value = (string)DataAccess.GetData(hana.GetData($@"SELECT ""U_Phase"" FROM ""OPRJ"" WHERE ""PrjCode"" = '{ GetID.CommandArgument}'", hana.GetConnection("SAPHana")), 0, "U_prPhase", "");

            var dtt = ws.GetHouseModelNew(GetID.CommandArgument).Tables["GetHouseModelNew"];
            tModel.Value = (string)DataAccess.GetData(dtt, 0, "Code", "");
            ////bBlockList_ServerClick(sender, e);
            ScriptManager.RegisterStartupScript(this, GetType(), "ProjHide", "MsgProjList_Hide();", true);
        }
        protected void tNextHouseDetails_Click(object sender, EventArgs e)
        {


            try
            {

                ////CHECK IF NO LETTER OF REQUEST
                //if (!string.IsNullOrWhiteSpace(lblFileName.Text))
                //{

                ViewState["DiscPercent"] = txtDiscPercent.Text;
                ViewState["DPPercent"] = tDPPercent.Text;

                DataTable dtUserAccess1 = (DataTable)Session["UserAccess"];
                var CC = dtUserAccess1.Select($"CodeEncrypt= 'CC'");
                string CCValue = "N";
                if (CC.Any())
                {
                    CCValue = "Y";
                }







                if (ddlRestructureType.Text != "-")
                {

                    if (lblEmployeeID.Text != "")
                    {
                        if (string.IsNullOrWhiteSpace(txtRequestLetterApprovalDate.Value) && CCValue == "N")
                        {
                            alertMsg("Please provide Request Letter Approval Date.", "warning");
                            txtRequestLetterApprovalDate.Focus();
                        }
                        else
                        {
                            RequiredFieldValidator18.Enabled = false;
                            RequiredFieldValidator18.Visible = false;

                            if (string.IsNullOrWhiteSpace(lblFileName.Text) && CCValue == "N")
                            {
                                alertMsg("Please provide Approved Request Letter.", "warning");
                            }
                            else
                            {
                                string ret = "";
                                if (Session["UserID"] == null)
                                {
                                    alertMsg("Session expired!", "error");
                                    Response.Redirect("~/pages/Login.aspx");
                                }
                                else
                                {

                                    //DataTable dt = ws.GetHouseDetails(tProjCode.Value, tBlock.Value, tLot.Value, tFinancing.Value).Tables["GetHouseDetails"];
                                    //if (DataAccess.Exist(dt) == true)
                                    //{
                                    //    GetHouseDetails(dt);
                                    //}
                                    //Compute();

                                    nxtHouseDetails();
                                    //Compute();
                                    ScriptManager.RegisterStartupScript(this, GetType(), "tNextTab", "tNextTab();", true);
                                }
                            }
                        }

                    }
                    else
                    {
                        alertMsg("Sales Agent is required to proceed.", "warning");
                        btnSelectAgent_Click(sender, e);
                    }
                }
                else
                {
                    alertMsg("Please provide restructuring type.", "warning");
                    ddlRestructureType.Focus();
                }
                //}
                //else
                //{
                //    alertMsg("No Letter of Request attached. Please provide Letter of Request to proceed.", "warning");
                //}
            }
            catch (Exception ex)
            { alertMsg(ex.Message, "error"); }
        }


        void nxtHouseDetails()
        {

            tDocDate2.Value = tDocDate.Text;
            tProjName2.Value = tProjName.Value;
            tModel2.Value = tModel.Value;
            tFinancing2.Value = tFinancing.Value;
            txtLotArea2.Value = tLotArea.Value;
            txtFloorArea2.Value = tFloorArea.Value;
            tHouseStatus2.Value = txtProductType.Value;
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


            //MA SCHEDULE   
            string qry = $@"SELECT * FROM ""@LOANTYPE"" WHERE ""Code"" = '{txtLoanType.Value}'";
            DataTable dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
            string oneLinertag = DataAccess.GetData(dt, 0, "U_OneLiner", "N").ToString();

            int addLBTermsToDP = 1;
            int addDPTermsToLB = 0;


            //DP SCHEDULE
            if (oneLinertag != "Y")
            {
                addLBTermsToDP = int.Parse(txtLTerms.Text);
            }

            if (double.Parse(txtDPTerms.Text) > 0)
            {
                ViewState["gvDownPayment"] = ws.PaymentBreakdown(Convert.ToInt32(string.IsNullOrWhiteSpace(txtDPTerms.Text) ? "1" : txtDPTerms.Text),
                                          Convert.ToDateTime(lblNewDPDueDate.Text),
                                          double.Parse(lblNewDPMonthly.Text),
                                          0,
                                          double.Parse(lblNewBalance.Text),
                                          "DP",
                                          interestRate,
                                          double.Parse(lblNewMiscFees.Text),
                                          double.Parse(txtPenaltyAmount.Text),
                                          addLBTermsToDP
                                          ).Tables["PaymentBreakdown"];
                divMonthlyDP.Visible = true;
            }
            else
            {
                divMonthlyDP.Visible = false;
            }
            DataTable ekek = (DataTable)ViewState["gvDownPayment"];
            LoadData(gvDownPayment, "gvDownPayment");





















            if (txtProductType.Value.ToUpper() == "LOT ONLY" && tRetType.Value == "BUYERS")
            {
                //FOR SINGLE MISCELLANEOUS

                //MISC SCHEDULE
                ViewState["gvMiscellaneous"] = ws.PaymentBreakdown(1,
                                               Convert.ToDateTime(lblNewMiscDueDate.Text),
                                               double.Parse(lblNewMiscDPMonthly.Text),
                                               0,
                                               double.Parse(lblNewMiscFees.Text),
                                               "MISC",
                                               interestRate,
                                               double.Parse(lblNewMiscFees.Text),
                                               0,
                                               addLBTermsToDP
                                               ).Tables["PaymentBreakdown"];
                DataTable ekek2 = (DataTable)ViewState["gvMiscellaneous"];
                LoadData(gvMiscellaneous, "gvMiscellaneous");


            }
            else
            {
                //FOR MULTIPLE MISC


                if (string.IsNullOrWhiteSpace(lblNewMiscDPTerms.Text))
                {
                    lblNewMiscDPTerms.Text = "0";
                }

                if (Convert.ToInt32(lblNewMiscDPTerms.Text) > 0)
                {
                    divMonthlyDPMisc.Visible = true;

                    //MISC SCHEDULE
                    ViewState["gvMiscellaneous"] = ws.PaymentBreakdown(Convert.ToInt32(lblNewMiscDPTerms.Text),
                                               Convert.ToDateTime(lblNewMiscDueDate.Text),
                                               double.Parse(lblNewMiscDPMonthly.Text),
                                               0,
                                               //double.Parse(lblNewMiscDPAmount.Text),
                                               //2023-06-07 : CHANGE TO BALANCE ON EQUITY
                                               double.Parse(lblNewMiscBalanceOnEquity.Text),
                                               "MISC",
                                               interestRate,
                                               //double.Parse(lblNewMiscDPAmount.Text),
                                               //2023-06-07 : CHANGE TO BALANCE ON EQUITY
                                               double.Parse(lblNewMiscBalanceOnEquity.Text),
                                               0,
                                               addLBTermsToDP
                                               ).Tables["PaymentBreakdown"];
                    DataTable ekek2 = (DataTable)ViewState["gvMiscellaneous"];
                    LoadData(gvMiscellaneous, "gvMiscellaneous");
                    divMonthlyDPMisc.Visible = true;
                }
                else
                {
                    ViewState["gvMiscellaneous"] = null;
                    divMonthlyDPMisc.Visible = false;
                }



                if (Convert.ToInt32(lblNewMiscLBTerms.Text) > 0)
                {
                    divMonthlyAmortMisc.Visible = true;

                    // MISC LB SCHEDULE
                    string MiscLBDate = Convert.ToDateTime(lblNewMiscDueDate.Text).AddMonths(int.Parse(lblNewMiscDPTerms.Text)).ToString("MM-dd-yyyy");


                    if (Convert.ToInt32(lblNewMiscLBTerms.Text) > 1)
                    {

                        ViewState["gvMiscellaneousAmort"] = ws.PaymentBreakdown(Convert.ToInt32(lblNewMiscLBTerms.Text),
                                                   Convert.ToDateTime(MiscLBDate),
                                                   double.Parse(lblNewMiscLBMonthly.Text),
                                                   0,
                                                   double.Parse(lblNewMiscLBAmount.Text),
                                                   "MISC",
                                                   interestRate,
                                                   double.Parse(lblNewMiscLBAmount.Text),
                                                   0,
                                                   0,
                                                   Convert.ToInt32(lblNewMiscDPTerms.Text)).Tables["PaymentBreakdown"];
                        DataTable ekek3 = (DataTable)ViewState["gvMiscellaneousAmort"];
                        LoadData(gvMiscellaneousAmort, "gvMiscellaneousAmort");
                    }
                    else
                    {
                        ViewState["gvMiscellaneousAmort"] = ws.PaymentBreakdownBankPagibig(Convert.ToDateTime(MiscLBDate).AddMonths(1),
                                                                                       Convert.ToDouble(lblNewMiscLBAmount.Text),
                                                                                       0,
                                                                                       Convert.ToInt32(lblNewMiscDPTerms.Text)).Tables[0];
                        DataTable ekek3 = (DataTable)ViewState["gvMiscellaneousAmort"];
                        LoadData(gvMiscellaneousAmort, "gvMiscellaneousAmort");

                    }

                    divMonthlyAmortMisc.Visible = true;
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
                if (Convert.ToDateTime(lblDueDate2n.Text) > DateTime.Now)
                {
                    if (IPS == 0)
                    {
                        IPS = Convert.ToDouble(txtPenaltyAmount.Text);
                    }
                }

                //ViewState["gvAmortization"] = ws.PaymentBreakdownBankPagibig(Convert.ToDateTime(lblDueDate2.Text).AddMonths(1), Convert.ToDouble(lblAmountDue.Text)).Tables[0];


                ViewState["gvAmortization"] = ws.PaymentBreakdownBankPagibig(Convert.ToDateTime(lblDueDate2n.Text).AddMonths(1),
                    Convert.ToDouble(lblNewLoanableBalance.Text), IPS).Tables[0];
                DataTable ekek3 = (DataTable)ViewState["gvAmortization"];

                divMonthlyAmort.Visible = true;
            }
            else
            {
                if (int.Parse(txtLTerms.Text) > 0)
                {

                    divMonthlyAmort.Visible = true;


                    if (oneLinertag != "Y")
                    {
                        if (IPS > 0)
                        {

                            addDPTermsToLB = Convert.ToInt32((int.Parse(txtDPTerms.Text) + addLBTermsToDP) - LatestTerms);
                            if (addDPTermsToLB == 0)
                            {
                                addDPTermsToLB = int.Parse(txtDPTerms.Text);
                            }
                        }
                    }




                    if (ddlRetainMonthlyAmort.Text == "YES" && ddlUpdateAmortBalance.Text == "YES")
                    {
                        int AmortMonth = Convert.ToDateTime(string.IsNullOrWhiteSpace(txtRestructuringDate.Value) ? "2022-01-01" : txtRestructuringDate.Value).Month;
                        int AmortDay = Convert.ToDateTime(tDocDate.Text).Day;
                        int AmortYear = Convert.ToDateTime(tDocDate.Text).Year;
                        string AmortDate = "";

                        if (AmortMonth == 2 && DateTime.DaysInMonth(AmortYear, AmortMonth) < AmortDay)
                        {
                            AmortDate = $@"{AmortYear}/{AmortMonth}/{AmortDay}";
                        }
                        else
                        {
                            AmortDate = $@"{AmortYear}/{AmortMonth}/{AmortDay}";
                        }

                        //ViewState["gvAmortization"] = ws.PaymentBreakdown(int.Parse(txtLTerms.Text), Convert.ToDateTime(lblDueDate3.Text), double.Parse(lblMiscMonthly.Text), 0, double.Parse(txtLoanableBalance2.Text), "LB", Convert.ToDouble(ViewState["IRate"]), double.Parse(lblAddMiscFees.Text)).Tables[0];
                        ViewState["gvAmortization"] = ws.PaymentBreakdown(
                                                                        int.Parse(txtLTerms.Text),
                                                                        Convert.ToDateTime(AmortDate).AddMonths(1),
                                                                        double.Parse(lblNewLBMonthly.Text),
                                                                        double.Parse(txtFactorRate1.Text),
                                                                        double.Parse(lblNewLoanableBalance.Text),
                                                                        "LB",
                                                                        interestRate,
                                                                        double.Parse(lblNewMiscFees.Text),
                                                                        double.Parse(txtPenaltyAmount.Text),
                                                                        addDPTermsToLB,
                                                                        0,
                                                                        Convert.ToDateTime(AmortDate)).Tables[0];
                        DataTable ekek2 = (DataTable)ViewState["gvAmortization"];
                    }
                    else
                    {
                        //ViewState["gvAmortization"] = ws.PaymentBreakdown(int.Parse(txtLTerms.Text), Convert.ToDateTime(lblDueDate3.Text), double.Parse(lblMiscMonthly.Text), 0, double.Parse(txtLoanableBalance2.Text), "LB", Convert.ToDouble(ViewState["IRate"]), double.Parse(lblAddMiscFees.Text)).Tables[0];
                        ViewState["gvAmortization"] = ws.PaymentBreakdown(
                                                                        int.Parse(txtLTerms.Text),
                                                                        Convert.ToDateTime(lblDueDate2n.Text).AddMonths(1),
                                                                        double.Parse(lblNewLBMonthly.Text),
                                                                        double.Parse(txtFactorRate1.Text),
                                                                        double.Parse(lblNewLoanableBalance.Text),
                                                                        "LB",
                                                                        interestRate,
                                                                        double.Parse(lblNewMiscFees.Text),
                                                                        double.Parse(txtPenaltyAmount.Text),
                                                                        addDPTermsToLB,
                                                                        0,
                                                                        Convert.ToDateTime(lblDueDate2n.Text)).Tables[0];
                        DataTable ekek2 = (DataTable)ViewState["gvAmortization"];
                    }


                }
                else
                {
                    divMonthlyAmort.Visible = false;
                    ViewState["gvAmortization"] = null;
                    DataTable ekek2 = (DataTable)ViewState["gvAmortization"];
                }


            }
            LoadData(gvAmortization, "gvAmortization");


        }






















        protected void bLot_ServerClick(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ProjHide", "MsgBlockLotList_Hide();", true);
            ViewState["dtDocumentStatus"] = ws.GetDocumentStatus().Tables["DocumentStatus"];
            LoadData(gvBlockColor, "dtDocumentStatus");


            string qryHouseModel = $@"SELECT ""U_HouseModel"",""U_HouseModelName"" FROM OBTN WHERE ""U_BlockNo"" = '{tBlock.Value}' AND 
                        ""U_LotNo"" = '{tLot.Value}' AND ""U_Project"" = '{tProjCode.Value}'";
            DataTable dt = hana.GetData(qryHouseModel, hana.GetConnection("SAPHana"));
            tModel.Value = (string)DataAccess.GetData(dt, 0, "U_HouseModel", "");
        }

        protected void bGenerate_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Gen", "MsgLotList_Hide();", true);
                DataTable dt = new DataTable();
                dt = ws.GetHouseDetails(tProjCode.Value, tBlock.Value, tLot.Value, tFinancing.Value).Tables["GetHouseDetails"];
                if (DataAccess.Exist(dt) == true)
                {
                    //GENERATE AUTOMATICALLY 
                    tLotArea.Value = (string)DataAccess.GetData(dt, 0, "U_LotArea", "");
                    tFloorArea.Value = (string)DataAccess.GetData(dt, 0, "U_FloorArea", "");
                    tHouseStatus.Value = (string)DataAccess.GetData(dt, 0, "U_ProductStatus", "");
                    tPhase.Value = (string)DataAccess.GetData(dt, 0, "U_Phase", "");
                    txtLotClassification.Value = (string)DataAccess.GetData(dt, 0, "U_LotClass", "");
                    txtProductType.Value = (string)DataAccess.GetData(dt, 0, "U_ProductType", "");

                    //SET RETITLING TYPE TO ABCI IF PRODUCT TYPE IS HOUSE AND LOT
                    if (txtProductType.Value.ToUpper() == "HOUSE AND LOT")
                    {
                        tRetType.Value = "ABCI";
                        btnRetType.Disabled = true;
                        tResrvFee.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "U_ResFee", "0").ToString());
                    }
                    else
                    {
                        btnRetType.Disabled = false;
                        tResrvFee.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "U_ResFeeLot", "0").ToString());
                    }

                    double LandPrice = Convert.ToDouble(DataAccess.GetData(dt, 0, "LandPrice", "0").ToString());
                    double HousePrice = Convert.ToDouble(DataAccess.GetData(dt, 0, "HousePrice", "0").ToString());
                    double MinLot = Convert.ToDouble(DataAccess.GetData(dt, 0, "MinLot", "0").ToString());

                    //COMPUTE TCP
                    if (MinLot == Convert.ToDouble(tLotArea.Value))
                    {
                        tODas.Value = SystemClass.ToCurrency(HousePrice.ToString());
                    }
                    else
                    {
                        tODas.Value = SystemClass.ToCurrency((LandPrice + HousePrice).ToString());
                    }

                    //tResrvFee.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "U_ResFee", "0").ToString());
                    txtDPTerms.Text = (string)DataAccess.GetData(dt, 0, "U_DPTerms", "");
                    tDPPercent.Text = SystemClass.NoDecimal(DataAccess.GetData(dt, 0, "U_Equity", "").ToString());
                    txtDiscPercent.Text = SystemClass.NoDecimal(DataAccess.GetData(dt, 0, "U_Discount", "").ToString());

                    txtDiscountAmount.Text = SystemClass.ToCurrency(SystemClass.GetDiscountAmount(tODas.Value, txtDiscPercent.Text).ToString()).ToString();

                    //Get VAT
                    double vat = 0;
                    double grossTCP = (LandPrice + HousePrice) - Convert.ToDouble(txtDiscountAmount.Text);
                    string qry = $@"SELECT IFNULL(""U_ThresholdAmount"",0) Threshold FROM ""@PRODUCTTYPE"" WHERE UPPER(""Code"") = '{txtProductType.Value.ToUpper()}'";
                    DataTable dtThes = hana.GetData(qry, hana.GetConnection("SAPHana"));
                    if (dtThes.Rows.Count > 0)
                    {
                        double threshold = Convert.ToDouble(DataAccess.GetData(dtThes, 0, "Threshold", ""));
                        if (threshold <= grossTCP)
                        {
                            vat = grossTCP * 0.12;
                        }
                    }

                    //tNetDas.Value = SystemClass.ToCurrency((grossTCP + vat).ToString());
                    tNetDas.Value = SystemClass.ToCurrency((Convert.ToDouble(tODas.Value) - Convert.ToDouble(txtDiscountAmount.Text)).ToString());
                    //tDPAmount.Text = SystemClass.ToCurrency(SystemClass.GetDiscountAmount(tODas.Value, tDPPercent.Text).ToString()).ToString();
                    tDPAmount.Text = SystemClass.ToCurrency(SystemClass.GetDiscountAmount(tNetDas.Value, tDPPercent.Text).ToString()).ToString();


                }
                else
                {
                    ViewState["IsGetHouse"] = true;
                    ViewState["IsGetSize"] = true;
                    ViewState["IsGetFeat"] = true;
                }

                if (lblID.Text != "Sample Quotation")
                {
                    //bPrint.Visible = false;
                    bFinish.Visible = true;
                    ws.SetColorByProj(tProjCode.Value, tBlock.Value, tLot.Value, "GETDATE()");
                }
                else
                {
                    //bPrint.Visible = true;
                    bFinish.Visible = false;
                }
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }
        }











        protected void bLotList_ServerClick(object sender, EventArgs e)
        {

            if (tProjName.Value != "")
            {
                ClearHouseDetails("Block");
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
            //if (tProjCode.Value != "")
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "MsgHouseModel", "MsgHouseModel_Show();", true);
            //    gvHouseModelList.DataSource = ws.GetHouseModelNew(tProjCode.Value).Tables["GetHouseModelNew"];
            //    gvHouseModelList.DataBind();

            //}
            //else { alertMsg("Please select project first!", "warning"); }

            try
            {
                DataTable dt = ws.GetHouseDetails(tProjCode.Value, tBlock.Value, tLot.Value, tFinancing.Value).Tables["GetHouseDetails"];
                tLotArea.Value = (string)DataAccess.GetData(dt, 0, "U_LotArea", "");
                tFloorArea.Value = (string)DataAccess.GetData(dt, 0, "U_FloorArea", "");
                tHouseStatus.Value = (string)DataAccess.GetData(dt, 0, "U_ProductStatus", "");
                tPhase.Value = (string)DataAccess.GetData(dt, 0, "U_Phase", "");
                txtLotClassification.Value = (string)DataAccess.GetData(dt, 0, "U_LotClass", "");
                txtProductType.Value = (string)DataAccess.GetData(dt, 0, "U_ProductType", "");

                tModel.Value = (string)DataAccess.GetData(dt, 0, "U_HouseModel", "");
                Compute();
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "warning");
            }

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

            //tResrvFee.Text = SystemClass.ToNumeric((string)DataAccess.GetData(dt, 0, "U_ResFee", "0"));
            if (Convert.ToInt32(ViewState["UpdateResFee"]) == 0)
            {
                tResrvFee.Text = SystemClass.ToNumeric((string)DataAccess.GetData(dt, 0, "U_ResFee", "0"));
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "MsgHouseList", "MsgHouseList_Hide();", true);
        }


        bool patchSAPDocument(SapHanaLayer company, string module, int entry, string column, string value, out string errorDesc)
        {
            return company.PATCH($@"{module}({entry})", $@"{{""{column}"": ""{value}""}}", out errorDesc);
        }


        bool cancelSAPTransactions(SapHanaLayer company, string module, int entry, string table, string field)
        {
            string qry = $@"SELECT ""DocEntry"" FROM {table} WHERE ""DocEntry"" = {entry} AND ""{field}"" <> 'Y'";
            DataTable dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
            if (dt.Rows.Count > 0)
            {
                return company.POST($"{module}({entry})/Cancel", new StringBuilder());
            }
            else
            {
                return true;
            }
        }


        bool updateSalesAgent(SapHanaLayer company, string DocNum, string salesAgent, out string errorDesc)
        {
            bool finalReturn = true;
            errorDesc = "";

            try
            {
                string qry = $@"SELECT  DISTINCT IFNULL(B.""TrgetEntry"",0) ""TrgetEntry"" , A.""DocEntry"" FROM OQUT A INNER JOIN QUT1 B ON A.""DocEntry"" = B.""DocEntry"" WHERE
                            A.""U_DreamsQuotationNo"" = '{DocNum}' AND IFNULL(A.""U_IssueType"",'') <> 'CANCELED'";
                DataTable dt = hana.GetData(qry, hana.GetConnection("SAPHana"));

                //QUOTATIONS
                int DocEntry = Convert.ToInt32(DataAccess.GetData(dt, 0, "DocEntry", "0"));
                if (dt.Rows.Count > 0)
                {
                    finalReturn = patchSAPDocument(company, "Quotations", DocEntry, "U_SalesAgent", salesAgent, out errorDesc);
                }

                //ORDERS
                if (finalReturn)
                {
                    DocEntry = Convert.ToInt32(DataAccess.GetData(dt, 0, "TrgetEntry", "0"));
                    if (DocEntry != 0)
                    {
                        finalReturn = patchSAPDocument(company, "Orders", DocEntry, "U_SalesAgent", salesAgent, out errorDesc);
                    }
                }

                //DOWNPAYMENT INVOICES
                if (finalReturn)
                {
                    qry = $@"SELECT DISTINCT A.""DocEntry"",A.""DocNum"" FROM ODPI A LEFT JOIN DPI1 B ON A.""DocEntry"" = B.""DocEntry"" 
                                WHERE ""U_DreamsQuotationNo"" = '{DocNum}' AND B.""TargetType"" <> 14";
                    dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
                    foreach (DataRow dr in dt.Rows)
                    {
                        DocEntry = Convert.ToInt32(dr["DocEntry"]);
                        finalReturn = patchSAPDocument(company, "DownPayments", DocEntry, "U_SalesAgent", salesAgent, out errorDesc);
                    }

                }

                //AR RESERVE INVOICES
                if (finalReturn)
                {
                    qry = $@"SELECT DISTINCT A.""DocEntry"",A.""DocNum"" FROM OINV A LEFT JOIN INV1 B ON A.""DocEntry"" = B.""DocEntry"" 
                                WHERE ""U_DreamsQuotationNo"" = '{DocNum}' AND A.""CANCELED"" = 'N' AND A.""isIns"" = 'Y'";
                    dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
                    foreach (DataRow dr in dt.Rows)
                    {
                        DocEntry = Convert.ToInt32(dr["DocEntry"]);
                        finalReturn = patchSAPDocument(company, "Invoices", DocEntry, "U_SalesAgent", salesAgent, out errorDesc);
                    }
                }

            }
            catch (Exception ex)
            {
                errorDesc = ex.Message;
            }



            return finalReturn;
        }

        bool updateCommission(SapHanaLayer company, string DocNum, string Project, string Block, string Lot, out string errorDesc)
        {
            bool finalReturn = true;
            errorDesc = "";

            try
            {
                string qry = $@"SELECT ""U_TransactionType"",""DocEntry"" FROM OPCH WHERE ""U_DreamsQuotationNo"" = '{DocNum}' and ""U_BlockNo"" = '{Block}' and 
                            ""U_LotNo"" = '{Lot}' and ""CANCELED"" = 'N' and ""U_TransactionType"" = 'COMMI'";
                DataTable dt = hana.GetData(qry, hana.GetConnection("SAPHana"));

                //QUOTATIONS
                int DocEntry = Convert.ToInt32(DataAccess.GetData(dt, 0, "DocEntry", "0"));
                if (dt.Rows.Count > 0)
                {
                    finalReturn = patchSAPDocument(company, "PurchaseInvoices", DocEntry, "U_ContractStatus", "Restructured", out errorDesc);
                }
            }
            catch (Exception ex)
            {
                errorDesc = ex.Message;
            }


            return finalReturn;
        }





        protected void bFinish_Click(object sender, EventArgs e)
        {
            confirmation("Are you sure you want to proceed restructuring?", "finish");

        }









        void confirmation(string body, string type)
        {
            ViewState["ConfirmType"] = type;
            lblConfirmationInfo.Text = body;
            btnYes.Focus();
            //Show Modal
            ScriptManager.RegisterStartupScript(this, this.GetType(), "confirmation", "showConfirmation();", true);
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


            if (lblOldLBDueDate.Text != "-")
            {
                ACDueDate = Convert.ToDateTime(lblOldLBDueDate.Text);
            }
            else { ACDueDate = Convert.ToDateTime("2000-01-01"); }

            //oLDueDate = oDPDueDate.AddMonths(int.Parse(ddlDPTerms.SelectedValue) + int.Parse(ConfigSettings.LTermsBuffer"].ToString()));
            oLDueDate = Convert.ToDateTime(lblOldLBDueDate.Text);


            //if (dtpDueDate.Text != "")
            //{ selDueDate = Convert.ToDateTime(dtpDueDate.Text); }

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
                                      Convert.ToDouble(lblOldDASAmt.Text), Convert.ToDouble(lblOldAddMiscCharges.Text), //22  
                                      Convert.ToDouble(lblOldVAT.Text), Convert.ToDouble(lblOldNetTCP.Text),  //24
                                      Convert.ToDouble(lblOldNetTCP.Text), Convert.ToDouble(lblPromoDisc.Text) * -1, //26
                                      double.Parse(txtDiscPercent.Text), double.Parse(lblOldDiscount.Text), //28    DISCOUNT PERCENT AND DISCOUNT AMOUNT
                                      double.Parse(txtDiscountAmount.Text), //30
                                      Convert.ToDouble(lblOldNetTCP.Text), Convert.ToDouble(lblOldDASAmt.Text),  //34
                                      Convert.ToDouble(lblOldNetDAS2.Text), Convert.ToDouble(lblOldAddMiscCharges.Text),  //36
                                      Convert.ToDouble(lblOldVAT.Text), Convert.ToDouble(lblOldNetTCP.Text), //38
                                      double.Parse(tDPPercent.Text), double.Parse(tDPAmount.Text), //40 
                                      double.Parse(tResrvFee.Text),  //42
                                      Convert.ToDouble(lblOldDownPayment.Text), int.Parse(txtDPTerms.Text), //44 
                                      oDPDueDate, Convert.ToDouble(lblOldDPMonthlyHidden.Text),  //46
                                                                                                 //int.Parse(tLMaturityAge.Text), double.Parse(tLPercent2.Value), //48 
                                      0, 0, //48 
                                      Convert.ToDouble(lblOldLBAmount.Text), int.Parse(txtLTerms.Text), //50
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
                                      null, double.Parse(lblOldNetDAS2.Text),  //86
                                      double.Parse(lblOldAddCharges.Text), ACDueDate, //88
                                      double.Parse(lblOldLBMonthly.Text), selDueDate); //90

            if (ret == "Operation completed successfully.")
            {
                tMonthlyAmort2.Text = lblOldLBMonthly.Text;
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
                { txt += "Size of the House"; oQuery = $@"CALL sp_GetHouseSize ('{hana.GetConnection("SAOHana")}','{(string)ViewState["HouseStatusCode"]}','{tProjCode.Value}');"; }
                else if (id == "bFeature") // && (bool)ViewState["IsGetFeat"] == true
                { txt += "Feature"; oQuery = $@"CALL sp_GetFeature '({ConfigurationManager.AppSettings["HANADatabase"]}','{(string)ViewState["HouseStatusCode"]}','{tProjCode.Value}');"; }
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
            loadSchedules();
            LoadData(gvDownPayment, "gvDownPayment");
        }
        protected void gvAmortization_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAmortization.PageIndex = e.NewPageIndex;
            loadSchedules();
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



            ClearProfileTab();
            clearComputationTabs();
            ClearHouseDetails("Project");
            clearPage();

            lblID.Text = "Sample Quotation";
            lblFinish.InnerText = "Finish";

            DateTime date;
            string tDate = "";
            if (!string.IsNullOrEmpty(txtBirthday.Value))
            {
                date = DateTime.Parse(txtBirthday.Value);
                tDate = date.ToString("dd-MMM-yyyy");
            }

            lblBusinessType.Text = ddlBusinessType.Text;
            lblLastName.Text = txtLastName.Value;
            lblFirstName.Text = txtFirstName.Value;
            lblMiddleName.Text = txtMiddleName.Value;
            lblBirthday.Text = tDate;
            lblNatureofEmployment.Text = tNatureofEmp.Value;
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
            ClearProfileTab();
            clearComputationTabs();
            ClearHouseDetails("Project");
            clearPage();

            loadDivisionsForNames(ddlBusinessType.Text);

            ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "MsgSampleActual_Hide();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "NewQuotationShow", "MsgNewQuotation_Show();", true);
        }

        void loadDivisionsForNames(string type)
        {
            if (type != "Corporation")
            {
                divIndividual.Visible = true;
                trFirstName.Visible = true;
                trLastName.Visible = true;
                trMiddleName.Visible = true;

                trBirthday.Visible = true;
                trIdType.Visible = true;
                trIdNumber.Visible = true;
                trNatureEmployment.Visible = true;
                //trTin.Visible = false;

                trCompanyName.Visible = false;
                divCorp.Visible = false;
                txtCompanyName.Value = " ";
                //txtTinNumber.Value = " ";
            }
            else
            {
                divIndividual.Visible = false;
                trFirstName.Visible = false;
                trLastName.Visible = false;
                trMiddleName.Visible = false;

                trBirthday.Visible = false;
                trIdType.Visible = false;
                trIdNumber.Visible = false;
                trNatureEmployment.Visible = false;
                //trTin.Visible = true;

                trCompanyName.Visible = true;
                divCorp.Visible = true;
                txtLastName.Value = " ";
                txtFirstName.Value = " ";
                txtMiddleName.Value = " ";
                tNatureofEmp.Value = " ";
                tTypeOfId.Value = " ";
                tIDNo.Value = " ";
            }
        }

        void clearPage()
        {
            tDocDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            tProjName.Value = "";
            tBlock.Value = "";
            tLot.Value = "";
            tModel.Value = "";
            tFinancing.Value = "";
            txtFinancingMisc.Value = "";

            tLotArea.Value = "";
            tFloorArea.Value = "";
            tHouseStatus.Value = "";
            tPhase.Value = "";
            txtLotClassification.Value = "";
            txtProductType.Value = "";
            txtLoanType.Value = "";
            tBank.Value = "";
        }


        void ClearFigureTextBoxes()
        {

            tODas.Value = "";
            tResrvFee.Text = "";
            tDPPercent.Text = "";
            tDPAmount.Text = "";
            txtDPTerms.Text = "";
            txtDiscPercent.Text = "";
            txtDiscountAmount.Text = "";
            txtLTerms.Text = "0";
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

            tNetDas.Value = String.Empty;
            tResrvFee.Text = "";
            tPDBalance.Text = "";
            txtDPMonthly.Text = "";
            txtMiscFees.Text = "";
            txtMiscTerms.Text = "";
            txtMiscMonthly.Text = "";

        }


        void clearRestructuringDocuments()
        {
            txtRestructuringDate.Value = String.Empty;
            ddlRestructureType.SelectedIndex = 0;
            txtOtherRestructuringType.Text = "";
            lblFileName.Text = "";
            txtRequestLetterApprovalDate.Value = String.Empty;


        }



        protected void gvBPList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBPList.PageIndex = e.NewPageIndex;
            LoadData(gvBPList, "gvBPList");
        }
        protected void bSelectBP_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string Code = GetID.CommandArgument;
            DataTable dt = new DataTable();
            dt = hana.GetData($@"CALL sp_GetBPbyCardCode ('{Code}')", hana.GetConnection("SAOHana"));

            txtCardCode.Value = Code;
            BoolBP(true);


            string test = ViewState["BPSelection"].ToString();



            if (DataAccess.Exist(dt) == true)
            {

                if (ViewState["BPSelection"].ToString() == "BUYER")
                {

                    ddlBusinessType.SelectedValue = (string)DataAccess.GetData(dt, 0, "BusinessType", "Individual");

                    txtLastName.Value = (string)DataAccess.GetData(dt, 0, "LastName", "");
                    txtFirstName.Value = (string)DataAccess.GetData(dt, 0, "FirstName", "");
                    txtMiddleName.Value = (string)DataAccess.GetData(dt, 0, "MiddleName", "");
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
                    //ViewState["Broker"] = (string)DataAccess.GetData(dt, 0, "Broker", "");
                    //ViewState["SalesManager"] = (string)DataAccess.GetData(dt, 0, "SalesManager", "");

                    string tCode;
                    tCode = (string)DataAccess.GetData(dt, 0, "NatureEmp", "");

                    if (ws.OLSTExist(tCode) == true)
                    { ViewState["tNatureofEmp"] = tCode; tNatureofEmp.Value = ws.GetOLSTName(tCode); }
                    else { ViewState["tNatureofEmp"] = "OTH"; tNatureofEmp.Value = tCode; }

                    tCode = (string)DataAccess.GetData(dt, 0, "IDType", "");

                    if (ws.OLSTExist(tCode) == true)
                    { ViewState["tTypeOfId"] = tCode; tTypeOfId.Value = ws.GetOLSTName(tCode); }
                    else { ViewState["tTypeOfId"] = "OTH"; tTypeOfId.Value = tCode; }

                    tIDNo.Value = (string)DataAccess.GetData(dt, 0, "IDNo", "");
                }
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

            }
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgBPList_Hide", "MsgBPList_Hide();", true);
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
                if (lblOldLBDueDate.Text == "-")
                {
                    if (tFinancing.Value == "Spot Cash")
                    {
                        dueDate = Convert.ToDateTime(tDocDate.Text);
                    }
                    else { dueDate = Convert.ToDateTime(lblOldDPDueDate.Text); }
                }
                else { dueDate = Convert.ToDateTime(lblOldLBDueDate.Text); }

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
                        AdditionalCharges = double.Parse(lblOldAddCharges.Text) / double.Parse(tTerms.Text);

                        ViewState["gvAddCharges"] = ws.PaymentBreakdown(int.Parse(tTerms.Text), dueDate, AdditionalCharges, 0, Convert.ToDouble(lblOldAddCharges.Text), "AC", 0,
                            double.Parse(lblOldDPMonthly.Text), 0, 0).Tables["PaymentBreakdown"];
                        LoadData(gvAddCharges, "gvAddCharges");
                    }

                    DataTable dt = new DataTable();
                    dt = hana.GetData($@"SELECT ""Code"", ""Name"" FROM ""OLST"" WHERE ""GrpCode"" = 'FS' AND ""Name"" = '{tFinancing.Value}'", hana.GetConnection("SAOHana"));
                    string oFSCode = (string)DataAccess.GetData(dt, 0, "Code", "0");
                    dt = hana.GetData($@"SELECT ""U_InterestRate"" FROM ""@FACTOR_RATE"" WHERE ""U_Terms"" = '{txtLTerms.Text}' AND ""U_FinancingScheme"" = '{oFSCode}'", hana.GetConnection("SAPHana"));
                    ViewState["IRate"] = (string)DataAccess.GetData(dt, 0, "U_InterestRate", "0");

                    ViewState["IRate"] = string.IsNullOrWhiteSpace(Convert.ToString(ViewState["IRate"])) ? "0" : ViewState["IRate"];

                    //ViewState["gvDownPayment"] = ws.PaymentBreakdown((int)Session["ActualDPTerms"], dueDate, double.Parse(lblOldDPMonthly2.Text), 0, double.Parse(lblOldDownPayment.Text),
                    //    "DP", Convert.ToDouble(ViewState["IRate"]), double.Parse(lblOldAddMiscCharges2.Text), 0, 0).Tables["PaymentBreakdown"];
                    //LoadData(gvDownPayment, "gvDownPayment");

                    if (oCode == "B" || oCode == "P" || oCode == "SC")
                    {
                        //ViewState["gvAmortization"] = ws.PaymentBreakdownBankPagibig(Convert.ToDateTime(lblDueDate3.Text), Convert.ToDouble(lblOldLBAmount.Text), 0).Tables[0];
                        LoadData(gvAmortization, "gvAmortization");
                    }
                    else
                    {
                        //ViewState["gvAmortization"] = ws.PaymentBreakdown(int.Parse(txtLTerms.Text), Convert.ToDateTime(lblDueDate3.Text), double.Parse(lblMonthly2.Text), double.Parse(tInterestRate.Text), double.Parse(lblAmountDue.Text), "LB", Convert.ToDouble(ViewState["IRate"])).Tables[0];
                        //ViewState["gvAmortization"] = ws.PaymentBreakdown(int.Parse(txtLTerms.Text), Convert.ToDateTime(lblDueDate3.Text), double.Parse(lblNewDasAmt.Text), 0,
                        //    double.Parse(lblOldLBAmount.Text), "LB", Convert.ToDouble(ViewState["IRate"]), double.Parse(lblOldAddMiscCharges2.Text), 0, 0).Tables[0];
                        //LoadData(gvAmortization, "gvAmortization");
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
            lblEmployeeID.Text = DataAccess.GetData(dt, 0, "Id", "0").ToString(); ;
            lblEmployeeName.Text = DataAccess.GetData(dt, 0, "SalesPerson", "").ToString();
            lblEmployeePosition.Text = DataAccess.GetData(dt, 0, "Position", "").ToString();
            lblEmployeeBrokerID.Text = DataAccess.GetData(dt, 0, "BrokerId", "").ToString();


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
                //ViewState["gvEmpList"] = hana.GetData($@"SELECT ""EmpID"" ""CardCode"", ""LastName"" || ', ' || ""FirstName"" ""CardName"" FROM ""OHEM"" WHERE (UPPER(""LastName"") || ', ' || UPPER(""FirstName"")) LIKE UPPER('%{txtSearchEmpList.Value}%')", hana.GetConnection("SAPHana"));
                //ViewState["gvEmpList"] = ws.Select($"SELECT CardCode,CardName FROM OCRD WHERE CardName LIKE '%{txtSearchEmpList.Value}%'", "EmployeeList", "SAP").Tables["EmployeeList"];

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



                //LinkButton btn = (LinkButton)sender;
                //int index = int.Parse(btn.CommandArgument);

                deleteSampleQuotation();

                string flrArea = "0";
                if (!string.IsNullOrEmpty(tFloorArea.Value))
                {
                    flrArea = tFloorArea.Value;
                }

                string qry = $@"SELECT ""U_LTSNo"" FROM OBTN WHERE ""U_Project"" = '{hPrjCode.Value}' AND ""U_BlockNo"" = '{tBlock.Value}' AND ""U_LotNo"" = '{tLot.Value}'";
                DataTable dtLTS = hana.GetDataDS(qry, hana.GetConnection("SAPHana")).Tables[0];
                string LTSNo = DataAccess.GetData(dtLTS, 0, "U_LTSNo", "").ToString();

                string qryPaymentScheme = $@"SELECT IFNULL(""U_PmtSchemeType"",'') ""U_PmtSchemeType"" FROM ""@FINANCINGSCHEME"" WHERE ""Code"" = '{tFinancing.Value}'";
                DataTable dtPaymentScheme = hana.GetDataDS(qryPaymentScheme, hana.GetConnection("SAPHana")).Tables[0];
                string PaymentScheme = DataAccess.GetData(dtPaymentScheme, 0, "U_PmtSchemeType", "").ToString();


                //CHECK IF VATABLE OR NOT
                DataTable dtVATThreshold = hana.GetData($@"SELECT ""U_ThresholdAmount"" FROM ""@PRODUCTTYPE"" WHERE UPPER(""Code"") = '{txtProductType.Value.ToUpper()}'", hana.GetConnection("SAPHana"));
                double vatThreshold = Convert.ToDouble(DataAccess.GetData(dtLTS, 0, "U_ThresholdAmount", "").ToString());
                double adjacentLotPrice = 0;
                string vatable = "";
                if (txtAdjLot.Value == "Yes")
                {
                    DataTable dtAdjacentLotPrice = ws.GetHouseDetails(tProjCode.Value, tBlock.Value, txtLotNo.Value, tFinancing.Value).Tables["GetHouseDetails"];
                    double LandPrice = Convert.ToDouble(DataAccess.GetData(dtAdjacentLotPrice, 0, "LandPrice", "0").ToString());
                    double HousePrice = Convert.ToDouble(DataAccess.GetData(dtAdjacentLotPrice, 0, "HousePrice", "0").ToString());
                    adjacentLotPrice = LandPrice + HousePrice;
                }
                double TotalPrice = Convert.ToDouble(lblOldNetDAS.Text) + adjacentLotPrice;
                vatable = (TotalPrice > vatThreshold ? "Y" : "N");



                var test1 = ViewState["QuoteDocEntry"];
                var test2 = ViewState["DPPercent"].ToString();
                var test3 = ViewState["DiscPercent"].ToString();
                var test4 = Session["UserID"].ToString();
                var test5 = ViewState["DocNum"].ToString();


                //string ret = ws.SampleQuotation(
                //                    4,
                //                    (int)ViewState["QuoteDocEntry"],  //2
                //                    DataAccess.GetSession(lblID.Text, ""),
                //                    Convert.ToDateTime(tDocDate.Text), //4
                //                    "O",

                //                     tProjCode.Value, //6
                //                    tBlock.Value,
                //                    tLot.Value, //8
                //                    tModel.Value,
                //                    tFinancing.Value, //10
                //                    tLotArea.Value,
                //                    flrArea, //12
                //                    tHouseStatus.Value,
                //                    tPhase.Value, //14
                //                    txtLotClassification.Value,
                //                    txtProductType.Value, //16
                //                    txtLoanType.Value,
                //                    tBank.Value, //18

                //                    double.Parse(lblNewDasAmt.Text),
                //                    double.Parse(tResrvFee.Text), //20
                //                    double.Parse(ViewState["DPPercent"].ToString()),
                //                    double.Parse(tDPAmount.Text), //22
                //                    int.Parse(txtDPTerms.Text),

                //                    double.Parse(ViewState["DiscPercent"].ToString()), //24
                //                    double.Parse(txtDiscountAmount.Text),
                //                    int.Parse(txtLTerms.Text), //26 
                //                    double.Parse(txtFactorRate1.Text),
                //                    Convert.ToDateTime(lblDueDate2n.Text),  //28
                //                    0,

                //                    double.Parse(lblNewDasAmt.Text),  //30
                //                    double.Parse(lblNewAddMiscFees.Text),
                //                    double.Parse(lblNewNetDAS.Text), //32
                //                    double.Parse(lblNewVAT.Text),
                //                    double.Parse(lblNewNetTCP.Text), //34
                //                    double.Parse(lblNewDownPayment.Text),
                //                    double.Parse(lblNewDPMonthly.Text),   //36
                //                    Convert.ToDateTime(lblNewDPDueDate.Text),
                //                    double.Parse(lblNewLoanableBalance.Text),   //38
                //                    (int)Session["UserID"],

                //                    double.Parse(txtMiscMonthly.Text),
                //                    double.Parse(lblNewDPMonthly.Text),
                //                    double.Parse(lblNewMiscDPMonthly.Text),
                //                    double.Parse(lblNewAddMiscFees.Text),
                //                    double.Parse(tPDBalance.Text),
                //                    double.Parse(lblNewBalance.Text),
                //                    double.Parse(txtMiscFees.Text),
                //                    double.Parse(txtLoanableBalance.Text),
                //                    double.Parse(lblOldLoanableBalance.Text),
                //                    "",
                //                    tRetType.Value,
                //                    double.Parse(lblNewCompTotal.Text),
                //                    txtAdjLot.Value,
                //                    txtLotNo.Value,
                //                    //ddTaxClass.SelectedValue.ToString(),
                //                    ViewState["DocNum"].ToString(),
                //                    LTSNo,
                //                    PaymentScheme,
                //                    vatable,
                //                    double.Parse(lblNewLBMonthly.Text),
                //                    "N",
                //                    txtComaker.Value
                //                    );


                //if (ret == "Operation completed successfully.")
                //{
                //    Session["PrintDocEntry"] = "0";
                //    Session["Title"] = "Sample Quotation";
                //    Session["ReportName"] = ConfigSettings.RestructuringForm"];
                //    Session["ReportPath"] = ConfigSettings.ReportPathForms"];
                //    Session["RptConn"] = "Addon";
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openpage", "window.open('ReportViewer.aspx');", true);
                //    txtDiscPercent.Text = ViewState["DiscPercent"].ToString();
                //    tDPPercent.Text = ViewState["DPPercent"].ToString();
                //}
                //else
                //{
                //    alertMsg("Error with printing. Please contact administrator.", "error");
                //}





            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }


        }




        protected void bSearchBuyer_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearchbuyer.Value))
            {
                ViewState["dtBuyers"] = hana.GetData($"CALL sp_Search (1,'{txtSearchbuyer.Value}','')", hana.GetConnection("SAOHana"));
                LoadData(gvBPList, "dtBuyers");
            }
            else
            { LoadFromFindBtn(); }
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
            LoadData(gvDownPayment, "gvAddCharges");
        }
        protected void gvDownPayment_PageIndexChanging1(object sender, GridViewPageEventArgs e)
        {

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
            ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "MsgBuyers_Hide();", true);
            //ScriptManager.RegisterStartupScript(this, GetType(), "NewQuotationShow", "MsgNewQuotation_Show();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgSampleActual", "MsgSampleActual_Show();", true);
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
            bool isBDateValid = true;
            try { DateTime bdate = Convert.ToDateTime(txtBirthday.Value); } catch (Exception ex) { isBDateValid = false; }


            if (txtLastName.Value == "" || txtFirstName.Value == "" || tIDNo.Value == "" || txtBirthday.Value == "")
            { alertMsg("Please fillup fields last name, first name, birthday and ID No.", "warning"); }
            else if (!isBDateValid) { alertMsg("Birthday is not valid.", "warning"); }
            else if (tNatureofEmp.Value == "") { alertMsg("Please select nature of employment.", "warning"); }
            else if (tTypeOfId.Value == "") { alertMsg("Please select type of ID.", "warning"); }
            else
            {
                if (string.IsNullOrEmpty(txtCardCode.Value))
                { lblID.Text = "New Buyer"; }
                else { lblID.Text = txtCardCode.Value; }

                ///IF EXIST MUST CHECK THE QUERY FOR IFExistBP 
                DataTable dt = new DataTable();
                dt = ws.IFExistBP(txtLastName.Value, txtFirstName.Value, txtMiddleName.Value, txtCompanyName.Value, tIDNo.Value).Tables["IFExistBP"];

                if (DataAccess.Exist(dt) == true)
                { lblID.Text = (string)DataAccess.GetData(dt, 0, "CardCode", ""); }
                else
                { lblID.Text = "New Buyer"; }

                lblComaker.Text = txtComaker.Value;
                lblLastName.Text = txtLastName.Value;
                lblFirstName.Text = txtFirstName.Value;
                lblMiddleName.Text = txtMiddleName.Value;
                lblCompanyName.Text = txtCompanyName.Value;

                if ((string)ViewState["tNatureofEmp"] == "OTH")
                { ViewState["tNatureofEmp"] = tNatureofEmp.Value; }

                lblBirthday.Text = txtBirthday.Value;
                lblNatureofEmployment.Text = tNatureofEmp.Value;
                lblTypeofID.Text = tTypeOfId.Value;
                lblIDNo.Text = tIDNo.Value;
                string MidName = txtMiddleName.Value;

                if (!string.IsNullOrEmpty(MidName))
                { MidName = txtMiddleName.Value[0].ToString(); }

                if (ddlBusinessType.Text == "Individual")
                {
                    lblName.Text = $"{txtLastName.Value}, {txtFirstName.Value} {MidName}";
                }
                else
                {
                    lblName.Text = txtCompanyName.Value;
                }
                DataTable dtt = new DataTable();
                var query = $@"SELECT T1.""TransID"",T1.""SalesPerson"",T1.""Position"" FROM OCRD T0 INNER JOIN OSLA T1 ON T0.""SalesAgent"" = CAST(T1.""TransID"" AS VARCHAR(100)) WHERE T0.""CardCode"" = '{lblID.Text}'";
                dtt = hana.GetData(query, hana.GetConnection("SAOHana"));
                lblEmployeeID.Text = (string)DataAccess.GetData(dtt, 0, "TransID", "");
                lblEmployeeName.Text = (string)DataAccess.GetData(dtt, 0, "SalesPerson", "");
                lblEmployeePosition.Text = (string)DataAccess.GetData(dtt, 0, "Position", "");

                ClearBP();
                ScriptManager.RegisterStartupScript(this, GetType(), "NewQuotationHide", "MsgNewQuotation_Hide();", true);
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
            gvHouseModelList.DataSource = ws.GetHouseModelNew(tProjCode.Value).Tables["GetHouseModelNew"];
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
            string qry = @"SELECT ""Code"", ""Name"" FROM ""@LOANTYPE"" WHERE ""U_Active"" = 'Y'";
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
                dueDate.Visible = false;
                //divMonthly2.Visible = false;
                txtLTerms.Text = "0";
                txtFactorRate1.Text = "0";
            }
            else
            {
                terms.Visible = true;
                interestRate.Visible = true;
                dueDate.Visible = true;
                //divMonthly2.Visible = false;
            }
        }

        protected void btnBank_ServerClick(object sender, EventArgs e)
        {
            string qry = $@"SELECT DISTINCT B.""U_BankCode"" ""Code"",B.""U_BankName"" ""Name"" from ""@ACCREDBANKSH"" A LEFT JOIN ""@ACCREDBANKSR"" B ON A.""Code"" = B.""Code"" AND A.""Code"" = '{tProjCode.Value}'
                            WHERE IFNULL(""U_ValidFr"",CURRENT_DATE) <= CURRENT_DATE AND IFNULL(""U_ValidTo"",CURRENT_DATE)>= CURRENT_DATE AND IFNULL(B.""Code"",'') <> '' ";
            gvBanks.DataSource = hana.GetData(qry, hana.GetConnection("SAPHana"));
            gvBanks.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgAccreditedBanks", "MsgAccreditedBanks_Show();", true);
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

        void visibleDocumentButtons(bool visible, LinkButton prev, LinkButton del)
        {
            prev.Visible = visible;
            del.Visible = visible;
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {

            if (FileUpload.HasFile) //If the used Uploaded a file  
            {
                //string code = Environment.MachineName;
                //2023-06-01 : NAMING OF UPLOADED FILES FOR 
                var ticks = DateTime.Now.Ticks;
                var guid = Guid.NewGuid().ToString();
                var uniqueSessionId = ticks.ToString() + guid.ToString();
                string[] agentCode = uniqueSessionId.Split('-');
                ViewState["Guid"] = agentCode.Last();


                string code = ViewState["Guid"].ToString();
                string uniqueCode = code;


                //Get FileName and Extension seperately
                string fileNameOnly = Path.GetFileNameWithoutExtension(FileUpload.FileName);
                string extension = Path.GetExtension(FileUpload.FileName);




                //string FileName = fileNameOnly + "_" + code + extension; //Name of the file is stored in local Variable   
                string FileName = fileNameOnly + "_" + code + extension; //File Name of buyer  

                lblFileName.Text = FileName;
                FileUpload.PostedFile.SaveAs(Server.MapPath("~/RESTRUCTURING/") + FileName); //File is saved in the Physical folder  

                visibleDocumentButtons(true, btnPreview, btnRemove);
                DataTable dt = ws.GetHouseDetails(tProjCode.Value, tBlock.Value, tLot.Value, tFinancing.Value).Tables["GetHouseDetails"];
                if (DataAccess.Exist(dt) == true)
                {
                    GetHouseDetails(dt);
                }
                Compute();
            }
            else
            {
                alertMsg("Please choose file!", "warning");
            }
        }


        void GetHouseDetails(DataTable dt)
        {
            //if (MiscFinScheme == 1)
            //{
            //    txtMiscTerms.Text = (string)DataAccess.GetData(dt, 0, "U_DPTerms", "1");
            //}
            //else
            //{
            //GENERATE AUTOMATICALLY 
            tLotArea.Value = (string)DataAccess.GetData(dt, 0, "U_LotArea", "");
            tFloorArea.Value = (string)DataAccess.GetData(dt, 0, "U_FloorArea", "");
            tHouseStatus.Value = (string)DataAccess.GetData(dt, 0, "U_ProductStatus", "");
            tPhase.Value = (string)DataAccess.GetData(dt, 0, "U_Phase", "");
            txtLotClassification.Value = (string)DataAccess.GetData(dt, 0, "U_LotClass", "");
            txtProductType.Value = (string)DataAccess.GetData(dt, 0, "U_ProductType", "");

            //SET RETITLING TYPE TO ABCI IF PRODUCT TYPE IS HOUSE AND LOT
            if (txtProductType.Value.ToUpper() == "HOUSE AND LOT")
            {
                tRetType.Value = "ABCI";
                btnRetType.Disabled = true;

                //tResrvFee.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "U_ResFee", "0").ToString());
                if (Convert.ToInt32(ViewState["UpdateResFee"]) == 0)
                {
                    tResrvFee.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "U_ResFee", "0").ToString());
                }
            }
            //COMMENTED 2021-12-27 
            //else if (txtProductType.Value.ToUpper() != "HOUSE AND LOT" && tRetType.Value == "ABCI")
            //{
            //    btnRetType.Disabled = false;
            //    tResrvFee.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "U_ResFee", "0").ToString());
            //}
            else
            {
                btnRetType.Disabled = false;
                //tResrvFee.Text = SystemClass.ToCurrency(DataAccess.GetData(dt, 0, "U_ResFeeLot", "0").ToString());

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
            txtDiscPercent.Text = SystemClass.NoDecimal(DataAccess.GetData(dt, 0, "U_Discount", "").ToString());

            txtDiscountAmount.Text = SystemClass.ToCurrency(SystemClass.GetDiscountAmount(tODas.Value, txtDiscPercent.Text).ToString()).ToString();

            //Get VAT
            double vat = 0;
            double grossTCP = (LandPrice + HousePrice) - Convert.ToDouble(txtDiscountAmount.Text);
            string qry = $@"SELECT IFNULL(""U_ThresholdAmount"",0) Threshold FROM ""@PRODUCTTYPE"" WHERE UPPER(""Code"") = '{txtProductType.Value.ToUpper()}'";
            DataTable dtThes = hana.GetData(qry, hana.GetConnection("SAPHana"));
            if (dtThes.Rows.Count > 0)
            {
                double threshold = Convert.ToDouble(DataAccess.GetData(dtThes, 0, "Threshold", ""));
                if (threshold <= grossTCP)
                {
                    vat = grossTCP * 0.12;
                }
            }
            lblNewReserveFee2.Text = tResrvFee.Text;
            lblNewReserveFee.Text = tResrvFee.Text;
            tNetDas.Value = SystemClass.ToCurrency((Convert.ToDouble(tODas.Value) - Convert.ToDouble(txtDiscountAmount.Text)).ToString());
            tDPAmount.Text = SystemClass.ToCurrency(SystemClass.GetDiscountAmount(tNetDas.Value, tDPPercent.Text).ToString()).ToString();
            lblNewDownPayment.Text = tDPAmount.Text;


            if (tFinancing.Value == "Spot Cash")
            {
                txtLoanType.Value = "SPOTCASH";
                hideTermDiv(txtLoanType.Value);

                divBank.Visible = false;
                divBank2.Visible = false;

            }

            txtLTerms.Text = (string)DataAccess.GetData(dt, 0, "U_BalTerms", "0");
            lblNewLBTerms.Text = txtLTerms.Text;

            txtFactorRate1.Text = (string)DataAccess.GetData(dt, 0, "U_Interest", "0");
            //}
        }


        void GetMiscDetails(DataTable dt)
        {

            string MiscDPPercentage = DataAccess.GetData(dt, 0, "U_Equity", "0").ToString();

            lblNewMiscDPAmount.Text = SystemClass.ToCurrency(SystemClass.GetDiscountAmount(lblNewmiscFeeOriginal.Text, MiscDPPercentage).ToString()).ToString();

            //2023-05-17 : ADDITIONAL CONDITION FOR COMPUTATION OF MISCPAYMENT 
            if (double.Parse(lblNewMiscDPAmount.Text) > double.Parse(lblNewMiscFeePaid.Text))
            {

                lblNewMiscBalanceOnEquity.Text = SystemClass.ToCurrency((double.Parse(lblNewMiscDPAmount.Text) - double.Parse(lblNewMiscFeePaid.Text)).ToString());

                //GET ALL MISCELLANEOUS DETAILS FOR MISCELLANEOUS FEE BREAKDOWN (DOWNPAYMENT AND LOANABLE BALACE)
                txtMiscTerms.Text = (string)DataAccess.GetData(dt, 0, "U_DPTerms", "0");
                lblNewMiscDPTerms.Text = (string)DataAccess.GetData(dt, 0, "U_DPTerms", "1");
            }
            else
            {
                lblNewMiscBalanceOnEquity.Text = "0.00";
                txtMiscTerms.Text = "0";
                lblNewMiscDPTerms.Text = "0";
            }

            lblNewMiscLBTerms.Text = (string)DataAccess.GetData(dt, 0, "U_BalTerms", "0");
            //txtFactorRate1.Text = (string)DataAccess.GetData(dt, 0, "U_Interest", "0");
        }


        protected void btnPreview_Click(object sender, EventArgs e)
        {
            //string Filepath = Server.MapPath("~/RESTRUCTURING/" + lblFileName.Text);
            ////lblFileName.Text = Filepath;
            ////System.Diagnostics.Process.Start(Filepath);
            //var host = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.LocalPath, "");
            //ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + host + "/RESTRUCTURING/" + lblFileName.Text + "');", true);

            if (File.Exists(Server.MapPath("~/RESTRUCTURING/") + lblFileName.Text))
            {
                string Filepath = Server.MapPath("~/RESTRUCTURING/" + lblFileName.Text);
                //System.Diagnostics.Process.Start(Filepath);
                var host = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.LocalPath, "");
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + host + "/RESTRUCTURING/" + lblFileName.Text + "');", true);
            }

        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            if (File.Exists(Server.MapPath("~/RESTRUCTURING/") + lblFileName.Text))
            {
                if (lblFinish.InnerText == "Update")
                {
                    string CardCode = (string)ViewState["CardCode"];
                    hana.Execute($@"UPDATE ""OCRD"" SET ""ApprovedDocument"" = '' WHERE ""CardCode"" = '{CardCode}'", hana.GetConnection("SAOHana"));
                }
                // If file found, delete it    
                File.Delete(Server.MapPath("~/RESTRUCTURING/") + lblFileName.Text);
                lblFileName.Text = "";
                visibleDocumentButtons(false, btnPreview, btnRemove);
            }
        }

        protected void txtPenaltyAmount_TextChanged(object sender, EventArgs e)
        {
            computeNetTotalPaid();
        }

        protected void btnSearchOwner_Click(object sender, EventArgs e)
        {
            ViewState["Ownership"] = ws.OwnerListSearch(txtSearchOwner.Value);
            gvOwnership.DataSource = (DataSet)ViewState["Ownership"];
            gvOwnership.DataBind();
        }

        protected void gvOwnership_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Sel"))
            {
                int row = int.Parse(e.CommandArgument.ToString());
                lblID.Text = gvOwnership.Rows[row].Cells[0].Text;
                lblName.Text = gvOwnership.Rows[row].Cells[4].Text;


                loadNewProfile();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "close", "closeOwner();", true);
            }
        }


        void loadNewProfile()
        {
            string qry = $@"select b.""LastName"",b.""FirstName"",b.""MiddleName"",a.""BusinessType"",a.""Comaker"",b.""CompanyName"",b.""BirthDay"", b.""NatureEmp"",
                         a.""IDType"",a.""IDNo"",a.""TaxClassification"", b.""TIN""	  from ocrd a inner join crd1 b on a.""CardCode"" = b.""CardCode"" 
                         where a.""CardCode"" = '{lblID.Text}' and b.""CardType"" = 'Buyer'";
            DataTable dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
            if (dt.Rows.Count > 0)
            {
                lblLastName.Text = DataAccess.GetData(dt, 0, "LastName", "").ToString();
                lblFirstName.Text = DataAccess.GetData(dt, 0, "FirstName", "").ToString();
                lblMiddleName.Text = DataAccess.GetData(dt, 0, "MiddleName", "").ToString();
                lblBusinessType.Text = DataAccess.GetData(dt, 0, "BusinessType", "").ToString();

                lblComaker.Text = DataAccess.GetData(dt, 0, "Comaker", "").ToString();
                lblCompanyName.Text = DataAccess.GetData(dt, 0, "CompanyName", "").ToString();
                lblBirthday.Text = DataAccess.GetData(dt, 0, "BirthDay", "").ToString();

                lblNatureofEmployment.Text = DataAccess.GetData(dt, 0, "NatureEmp", "").ToString();

                string qry1 = $@"select ""Name"" from olst where ""GrpCode"" = 'ID' AND ""Code"" = '{DataAccess.GetData(dt, 0, "IDType", "")}'";
                DataTable dtX = hana.GetData(qry1, hana.GetConnection("SAOHana"));
                lblTypeofID.Text = (string)DataAccess.GetData(dtX, 0, "Name", "");

                lblIDNo.Text = DataAccess.GetData(dt, 0, "IDNo", "").ToString();
                lblTaxClassification.Text = DataAccess.GetData(dt, 0, "TaxClassification", "").ToString();
                lblTIN.Text = DataAccess.GetData(dt, 0, "TIN", "").ToString();
            }
        }


        protected void gvOwnership_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvOwnership.DataSource = (DataSet)ViewState["Ownership"];
            gvOwnership.PageIndex = e.NewPageIndex;
            gvOwnership.DataBind();
        }
        void LoadOwners()
        {
            if (gvOwnership.Rows.Count == 0)
            {
                ViewState["Ownership"] = ws.OwnerList();
                gvOwnership.DataSource = (DataSet)ViewState["Ownership"];
                gvOwnership.DataBind();
            }
        }

        protected void btnAdjLot_ServerClick(object sender, EventArgs e)
        {
            string qry = $@"Select 'Yes' as ""Name"" from ""DUMMY"" union all select 'No' as ""Name"" from ""DUMMY""";
            gvAdjLot.DataSource = hana.GetData(qry, hana.GetConnection("SAOHana"));
            gvAdjLot.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgAdjLot", "MsgAdjLot_Show();", true);
        }














        //protected void btnRetType_ServerClick(object sender, EventArgs e)
        //{
        //    string qry = $@"Select 'ABCI' as ""Name"" from ""DUMMY"" union all select 'BUYERS' as ""Name"" from ""DUMMY""";
        //    gvRetType.DataSource = hana.GetData(qry, hana.GetConnection("SAPHana"));
        //    gvRetType.DataBind();
        //    ScriptManager.RegisterStartupScript(this, GetType(), "MsgRetitlingType", "MsgRetitlingType_Show();", true);
        //}












        #region computationSheet old

        //void computationSheet()
        //{
        //    //joses
        //    if (Session["UserID"] == null)
        //    {
        //        alertMsg("Session expired!", "error");
        //        Response.Redirect("~/pages/Login.aspx");
        //    }
        //    try
        //    {
        //        //DISCOUNT 
        //        double initDisc = 0;
        //        if (Convert.ToDecimal(txtDiscountAmount.Text) == 0 && ddlDiscBased.Text == "Spot DP") //if discount amount = 0 and discount base is "spot dp"
        //        {
        //            if (Convert.ToDecimal(tDPAmount.Text) == 0) //if no downpayment amount
        //            {
        //                initDisc = (Convert.ToDouble(lblDASAmt.Text) * (Convert.ToDouble(tDPPercent.Text) / 100)) * (Convert.ToDouble(txtDiscPercent.Text) / 100); //(Das Amount * dp %) * disc %
        //            }
        //            else { initDisc = Convert.ToDouble(tDPAmount.Text) * (Convert.ToDouble(txtDiscPercent.Text) / 100); } //dp amt * disc %
        //        }
        //        else
        //        {
        //            if (Convert.ToDecimal(txtDiscountAmount.Text) != 0 && ddlDiscBased.Text == "Spot DP") // if disc amt <> 0 and disc base = "spot dp"
        //            {
        //                initDisc = Convert.ToDouble(tDPAmount.Text) * Convert.ToDouble(txtDiscPercent.Text); //dp amt * disc %
        //            }
        //            else
        //            {
        //                if (Convert.ToDouble(txtDiscountAmount.Text) == 0 && ddlDiscBased.Text == "Spot Cash") //if disc amt =    0 and disc base is "spot cash"
        //                {
        //                    initDisc = Convert.ToDouble(lblDASAmt.Text) * (Convert.ToDouble(txtDiscPercent.Text) / 100); //das amt * disc %
        //                }
        //                else { initDisc = Convert.ToDouble(txtDiscountAmount.Text); } //disc amount only 
        //            }
        //        }
        //        lblDiscount.Text = SystemClass.ToCurrency((initDisc * -1).ToString());

        //        //NET DAS = Sum of DAS Amount, LessPromo Discount, and Discount
        //        lblNetDAS.Text = SystemClass.ToCurrency((Convert.ToDouble(lblDASAmt.Text) + Convert.ToDouble(lblPromoDisc.Text) + Convert.ToDouble(lblDiscount.Text)).ToString()); //NET DAS

        //        //NET DAS second part
        //        txtAddChargeAmount.Text = string.IsNullOrWhiteSpace(txtAddChargeAmount.Text) ? "0" : txtAddChargeAmount.Text;
        //        if (ddlAllowed.Text == "Approved")
        //        { lblNetDAS2.Text = SystemClass.ToCurrency((Convert.ToDouble(lblNetDAS.Text) - Convert.ToDouble(txtAddChargeAmount.Text)).ToString()); }
        //        else { lblNetDAS2.Text = SystemClass.ToCurrency(Convert.ToDouble(lblNetDAS.Text).ToString()); }

        //        //CHECK FOR FINANCING SCHEME CODE
        //        DataTable dt = new DataTable();
        //        dt = ws.Select($"SELECT Code, Name FROM OLST WHERE GrpCode = 'FS' AND Name = '{tFinancing.Value}'", "OLST", "Addon").Tables["OLST"];
        //        string oFSCode = (string)DataAccess.GetData(dt, 0, "Code", "0");

        //        //

        //        if (tSalesType.Value == "Inclusive")
        //        {
        //            double miscPercent = 0;
        //            //CHECK IF PERCENTAGE EXISTS IN MODEL
        //            dt = ws.Select($"select Distinct U_MiscRate From [@HOUSE_MOD] WHERE Code = '{tModel.Value}'", "HOUSE_MOD", "SAP").Tables["HOUSE_MOD"];
        //            double modelPercent = Convert.ToDouble(string.IsNullOrEmpty((string)DataAccess.GetData(dt, 0, "U_MiscRate", "0")) ? "0" : (string)DataAccess.GetData(dt, 0, "U_MiscRate", "0"));
        //            if (modelPercent != 0)
        //            { miscPercent = modelPercent; }
        //            else
        //            {
        //                //CHECK IF PERCENTAGE EXISTS IN PROJECT
        //                dt = ws.Select($"SELECT distinct  U_soMiscRate FROM OPRJ WHERE PrjCode = '{hPrjCode.Value}'", "OPRJ", "SAP").Tables["OPRJ"];
        //                double ProjMiscRate = Convert.ToDouble(string.IsNullOrEmpty((string)DataAccess.GetData(dt, 0, "U_soMiscRate", "0")) ? "0" : (string)DataAccess.GetData(dt, 0, "U_soMiscRate", "0"));
        //                if (ProjMiscRate != 0)
        //                { miscPercent = ProjMiscRate; }
        //                else
        //                {
        //                    //LOOK UP ON FINANCING SCHEME 
        //                    dt = ws.Select($"select DISTINCT  A.U_MiscCharge from [@FSCHEME] A WHERE A.Code =  '{oFSCode}'", "FactorRate", "SAP").Tables["FactorRate"];
        //                    double oMisccCharge = Convert.ToDouble(string.IsNullOrEmpty((string)DataAccess.GetData(dt, 0, "U_MiscCharge", "0")) ? "0" : (string)DataAccess.GetData(dt, 0, "U_MiscCharge", "0"));
        //                    if (oMisccCharge != 0)
        //                    { miscPercent = oMisccCharge; }
        //                }
        //            }
        //            lblAddMiscCharges.Text = SystemClass.ToCurrency(((miscPercent / 100) * Convert.ToDouble(lblNetDAS2.Text)).ToString());
        //            txtAddChargeAmount.Text = lblAddMiscCharges.Text;
        //        }
        //        else
        //        {
        //            lblAddMiscCharges.Text = "0.00";
        //            txtAddChargeAmount.Text = "0.00";
        //        }

        //        //FACTOR RATE
        //        DataTable dt1 = new DataTable();
        //        dt1 = ws.Select($"SELECT U_FactorRate FROM [@FACTOR_RATE] WHERE U_Terms = '{ddlLTerms.SelectedValue.ToString()}' AND U_FinancingScheme = '{oFSCode}'", "FactorRate", "SAP").Tables["FactorRate"];
        //        string uFactorRate = (string)DataAccess.GetData(dt1, 0, "U_FactorRate", "0");
        //        txtFactorRate.Value = uFactorRate;

        //        //VAT
        //        double initVat = 0;
        //        DataTable dt2 = new DataTable();
        //        dt2 = ws.Select($"SELECT U_ThresholdAmt FROM [@HOUSE_PROP] WHERE Name = '{tHouseStatus.Value}'", "HOUSE_PROP", "SAP").Tables["HOUSE_PROP"];
        //        string threshold = (string)DataAccess.GetData(dt2, 0, "U_ThresholdAmt", "0");
        //        if (Convert.ToDouble(lblNetDAS2.Text) >= Convert.ToDouble(threshold))
        //        {
        //            initVat = Convert.ToDouble(lblNetDAS2.Text) * 0.12;
        //        }
        //        else
        //        {
        //            if (Convert.ToDouble(txtAddChargeAmount.Text) > 100000 && ddlAllowed.SelectedValue == "Approved")
        //            {
        //                initVat = (Convert.ToDouble(txtAddChargeAmount.Text) - 100000) * 0.12;
        //            }
        //            else { initVat = 0; }
        //        }
        //        lblVAT.Text = SystemClass.ToCurrency(initVat.ToString());

        //        //NET TCP = Sum of netDas, AddMiscCharges, and vAT
        //        lblNetTCP.Text = SystemClass.ToCurrency((Convert.ToDouble(lblNetDAS2.Text) + Convert.ToDouble(lblAddMiscCharges.Text) + Convert.ToDouble(lblVAT.Text)).ToString());





        //        //TCP Breakdown: Down Payment
        //        double initDP = 0;
        //        if (tFinancing.Value == "Spot Cash") //if financiing scheme is Spot Cash
        //        {
        //            initDP = 0; // no amount
        //        }
        //        else
        //        {
        //            if (tDPAmount.Text == "0") //if downpayment amount 
        //            {
        //                if (tFinancing.Value == "Spot Cash") // if financing scheme is Spot Cash
        //                {
        //                    initDP = Convert.ToDouble(lblNetTCP.Text) - Convert.ToDouble(tResrvFee.Text); //total net tcp - reservation fee
        //                }
        //                else
        //                {
        //                    initDP = (Convert.ToDouble(lblNetTCP.Text) * (Convert.ToDouble(tDPPercent.Text) / 100)) - Convert.ToDouble(tResrvFee.Text); // (total net tcp  
        //                }
        //            }
        //            else { initDP = Convert.ToDouble(tDPAmount.Text); } // get DP Amount
        //        }
        //        lblDownPayment.Text = SystemClass.ToCurrency(initDP.ToString());

        //        // TCP Breakdown : MONTHLY
        //        double initMonthly1 = 0;
        //        if (tFinancing.Value == "Spot Cash") //if financiing scheme is Spot Cash
        //        {
        //            initMonthly1 = Convert.ToDouble(lblDownPayment.Text); //get downpayment amount
        //        }
        //        else { initMonthly1 = Convert.ToDouble(lblDownPayment.Text) / Convert.ToDouble(ddlDPTerms.SelectedItem.Text); } //downpatment amt / dp terms (months)
        //        lblMonthly1.Text = SystemClass.ToCurrency(initMonthly1.ToString());

        //        //TCP Breakdown : DUE DATE 1
        //        if (Convert.ToInt32(ViewState["UpdatedDPDueDate"]) == 0)
        //        {

        //            string initDueDate1 = "-";
        //            if (tFinancing.Value == "Spot Cash") //if financiing scheme is Spot Cash
        //            {
        //                lblDueDate1.Text = "-"; //no due date
        //                dtpDueDate.Visible = false;
        //            }
        //            else
        //            {
        //                if (tDocDate.Text != "" && ddlDPDay.SelectedValue != "") //if transDate and Due Date is not 0
        //                {
        //                    DateTime tranDate1 = Convert.ToDateTime(tDocDate.Text); //convert transdate form text to doc date
        //                    if (Convert.ToInt16(tranDate1.Day.ToString()) > Convert.ToInt16(ddlDPDay.SelectedValue)) //if transaction date day is greatr than due date
        //                    {
        //                        initDueDate1 = Convert.ToString(tranDate1.AddDays(7)); //get transaction date plus 7 days
        //                    }
        //                    else
        //                    {
        //                        if (tranDate1.Month.ToString() == "2" && ddlDPDay.SelectedValue.ToString() == "30")
        //                        {
        //                            initDueDate1 = tranDate1.AddMonths(1).AddDays(-1).ToString();
        //                        }
        //                        else
        //                        {
        //                            //make date by concating trans date month + due date day + trans date year
        //                            initDueDate1 = tranDate1.Month.ToString() + "/" + ddlDPDay.SelectedValue.ToString() + "/" + tranDate1.Year.ToString();
        //                        }
        //                    }
        //                }

        //                if (!initDueDate1.Contains("-"))
        //                {
        //                    lblDueDate1.Text = Convert.ToDateTime(initDueDate1).ToString("MM-dd-yyyy");
        //                }

        //                dtpDueDate.Visible = true;
        //                dtpDueDate.Text = Convert.ToDateTime(initDueDate1).ToString("yyyy-MM-dd");
        //            }
        //        }
        //        else
        //        {
        //            lblDueDate1.Text = dtpDueDate.Text;
        //        }
        //        ViewState["UpdatedDPDueDate"] = 0;

        //        //TCP Breakdown : Additional Charges
        //        if (ddlAllowed.Text == "Approved") //if allowed
        //        {
        //            lblAddCharges.Text = txtAddChargeAmount.Text; //get data of total charge amount
        //        }
        //        else { lblAddCharges.Text = "0.00"; } //get 0

        //        //TCP Breakdown : Due Date 2
        //        string initDueDate2 = "-";
        //        if (Convert.ToDouble(lblAddCharges.Text) == 0)
        //        {
        //            lblDueDate2.Text = "-";
        //        }
        //        else
        //        {
        //            if (tFinancing.Value == "Spot Cash") //if financiing scheme is Spot Cash
        //            {
        //                DateTime tranDate2 = Convert.ToDateTime(tDocDate.Text); //convert transdate form text to doc date
        //                if (Convert.ToInt16(tranDate2.Day.ToString()) > Convert.ToInt16(ddlDPDay.SelectedValue)) //if transaction date day is greatr than due date
        //                {
        //                    initDueDate2 = Convert.ToString(tranDate2.AddDays(7)); //get transaction date plus 7 days
        //                }
        //                else { initDueDate2 = tranDate2.Month.ToString() + "/" + ddlDPDay.SelectedValue.ToString() + "/" + tranDate2.Year.ToString(); } //make date by concating trans date month + due date day + trans date year
        //            }

        //            else
        //            {
        //                initDueDate2 = lblDueDate1.Text; //get due date 1
        //            }

        //            if (!initDueDate2.Equals("-"))
        //            {
        //                lblDueDate2.Text = Convert.ToDateTime(initDueDate2).ToString("MM-dd-yyyy");
        //            }
        //        }

        //        //TCP Breakdown : Loanable Amount
        //        double initFinance = 0;
        //        if (tFinancing.Value == "Spot Cash") //if financiing scheme is Spot Cash
        //        {
        //            initFinance = Convert.ToDouble(lblNetTCP.Text) - Convert.ToDouble(tResrvFee.Text); //gset net tcp - reservation fee
        //        }
        //        else { initFinance = Convert.ToDouble(lblNetTCP.Text) - Convert.ToDouble(lblDownPayment.Text) - Convert.ToDouble(tResrvFee.Text); } //get net tcp - dp amount - reservation fee
        //        lblAmountDue.Text = SystemClass.ToCurrency(initFinance.ToString());

        //        //TCP Breakdown : Monthly 2 
        //        txtFactorRate.Value = string.IsNullOrWhiteSpace(txtFactorRate.Value) ? "0" : txtFactorRate.Value;
        //        lblMonthly2.Text = SystemClass.ToCurrency((Convert.ToDouble(lblAmountDue.Text) * Convert.ToDouble(txtFactorRate.Value)).ToString());

        //        //TCP Breakdown : Due Date 3
        //        string initDueDate3 = "-";
        //        DateTime tranDate3 = Convert.ToDateTime(tDocDate.Text); //convert transdate form text to doc date
        //        if (tFinancing.Value == "Spot Cash") //if financiing scheme is Spot Cash
        //        {
        //            if (Convert.ToInt16(tranDate3.Day.ToString()) > Convert.ToInt16(ddlDPDay.SelectedValue)) //if transaction date day is greatr than due date
        //            {
        //                initDueDate3 = Convert.ToString(tranDate3.AddDays(7)); //get transaction date plus 7 days
        //            }
        //            else { initDueDate3 = tranDate3.Month.ToString() + "/" + ddlDPDay.SelectedValue.ToString() + "/" + tranDate3.Year.ToString(); } //make date by concating trans date month + due date day + trans date year
        //        }
        //        else
        //        {
        //            if (tranDate3.Month.ToString() == "2" && ddlDPDay.SelectedValue.ToString() == "30")
        //            {
        //                initDueDate3 = tranDate3.AddMonths(1).AddDays(-1).ToString();
        //            }
        //            else
        //            {
        //                initDueDate3 = tranDate3.Month.ToString() + "/" + ddlDPDay.SelectedValue.ToString() + "/" + tranDate3.Year.ToString();  //make date by concating trans date month + due date day + trans date year
        //                if (!initDueDate3.Equals("-"))
        //                {
        //                    initDueDate3 = Convert.ToDateTime(initDueDate3).AddMonths(Convert.ToInt16(ddlDPTerms.SelectedValue.ToString())).ToString(); //add smonths depends 
        //                }

        //            }
        //        }

        //        DataTable dtDueDate3 = new DataTable();
        //        dtDueDate3 = ws.Select($"SELECT ISNULL(ROUND(U_GapDueDate,0),0) GapDueDate FROM [@FSCHEME] WHERE Code = '{oFSCode}'", "GapDueDate", "SAP").Tables["GapDueDate"];
        //        double GapDueDate = Convert.ToDouble((DataAccess.GetData(dtDueDate3, 0, "GapDueDate", "0")));
        //        if (GapDueDate > 0)
        //        {
        //            initDueDate3 = Convert.ToDateTime(initDueDate3).AddMonths(-1).ToString();
        //            initDueDate3 = Convert.ToDateTime(initDueDate3).AddMonths(Convert.ToInt16(GapDueDate)).ToString();
        //        }
        //        if (!initDueDate3.Contains("-"))
        //        {
        //            lblDueDate3.Text = Convert.ToDateTime(initDueDate3).ToString("MM-dd-yyyy");
        //        }


        //        //Additional Chargese
        //        double AdditionalCharge = 0;
        //        double initAddCharge = 0;
        //        initAddCharge = Convert.ToDouble(lblNetDAS.Text) - Convert.ToDouble(threshold);
        //        initAddCharge = Ceiling(initAddCharge, 1000);
        //        if (initAddCharge > 200000 || initAddCharge < 0)
        //        {
        //            AdditionalCharge = 0;
        //        }
        //        else { AdditionalCharge = initAddCharge; }
        //        txtAddChargeAmount.Text = SystemClass.ToCurrency((Math.Ceiling(AdditionalCharge) * 1.12).ToString());

        //    }
        //    catch (Exception ex)
        //    { alertMsg(ex.Message, "error"); }
        //}
        #endregion

        protected void bLotNo_ServerClick(object sender, EventArgs e)
        {
            loadAdjacentLots();
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgLotNo", "MsgLotNo_Show();", true);
        }

        void loadAdjacentLots()
        {
            string qry = $@"Select ""U_LotNo"" ""Lot"", ""U_BlockNo"" ""BlocK"" from ""OBTN"" where ""U_Project"" = '{tProjCode.Value}' and ""U_BlockNo"" = '{tBlock.Value}'";
            gvLotNo.DataSource = hana.GetData(qry, hana.GetConnection("SAPHana"));
            gvLotNo.DataBind();
        }

        protected void bAdjLot_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string Name = GetID.CommandArgument;
            txtAdjLot.Value = Name;
            RequiredFieldValidator25.Enabled = Name == "No" ? false : true;
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

        protected void gvLotNo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLotNo.PageIndex = e.NewPageIndex;
            loadAdjacentLots();
        }

        protected void btnRequirementDocumentRequirements_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgRestructuringDocReq", "MsgRestructuringDocReq_Show();", true);
        }


        protected void gvRestructuringDocumentRequirement_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRestructuringDocumentRequirement.PageIndex = e.NewPageIndex;
            loadRestructuringDocReq();
        }

        protected void gvRestructuringDocumentRequirement_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToString() == "Upload1" || e.CommandName.ToString() == "Preview1" || e.CommandName.ToString() == "Remove1")
                {
                    uploadDocRequirements(gvRestructuringDocumentRequirement, e, "FileUpload1", "lblFileName1", "btnPreview1", "btnRemove1");
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "MsgRestructuringDocReq", "MsgRestructuringDocReq_Show();", true);

            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "warning");
            }
        }



        void uploadDocRequirements(GridView gv, GridViewCommandEventArgs e, string FileUpload, string lblfilename, string preview, string remove)
        {
            try
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int index = row.RowIndex;
                FileUpload fileUpload = (FileUpload)gv.Rows[index].FindControl(FileUpload);
                Label lblFileName = (Label)gv.Rows[index].FindControl(lblfilename);
                LinkButton btnPreview = (LinkButton)gv.Rows[index].FindControl(preview);
                LinkButton btnDelete = (LinkButton)gv.Rows[index].FindControl(remove);

                TextBox ExpirationDate = (TextBox)gv.Rows[index].FindControl("lblExpirationDate");


                string docId = gv.Rows[index].Cells[0].Text;
                string docName = gv.Rows[index].Cells[1].Text;

                if (e.CommandName == "Upload1")
                {


                    if (fileUpload.HasFile) //If there's an Uploaded a file  
                    {


                        //2023-06-09 : NAMING OF UPLOADED FILES FOR 
                        var ticks = DateTime.Now.Ticks;
                        var guid = Guid.NewGuid().ToString();
                        var uniqueSessionId = ticks.ToString() + guid.ToString();
                        string[] agentCode = uniqueSessionId.Split('-');
                        ViewState["Guid"] = agentCode.Last();

                        //2023-06-09 : NAMING OF UPLOADED FILES FOR 
                        //string code = Environment.MachineName + "_" + docId;
                        string code = ViewState["Guid"].ToString();


                        //Get FileName and Extension seperately
                        string fileNameOnly = Path.GetFileNameWithoutExtension(fileUpload.FileName);
                        string extension = Path.GetExtension(fileUpload.FileName);
                        string uniqueCode = code;

                        string FileName = fileNameOnly + "_" + code + extension; //Name of the file is stored in local Variable   
                        lblFileName.Text = FileName;


                        //save/update img to database
                        if (ws.AddRestructuringDocuments(lblDocEntry.Text, docId, docName, FileName, ExpirationDate.Text, (int)Session["UserID"], DateTime.Now.ToShortDateString()))
                        {
                            fileUpload.PostedFile.SaveAs(Server.MapPath("~/RES_REQ/") + FileName); //File is saved in the Physical folder  

                            visibleDocumentButtons(true, btnPreview, btnDelete);
                            loadRestructuringDocReq();
                            //btnSave.Visible = true;
                            alertMsg("Document uploaded successfully.", "success");

                        }
                        else
                        { alertMsg("Error in uploading documents.", "error"); }


                    }
                }
                else if (e.CommandName == "Preview1")
                {
                    string Filepath = Server.MapPath("~/RES_REQ/" + lblFileName.Text);
                    //System.Diagnostics.Process.Start(Filepath);
                    var host = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.LocalPath, "");
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + host + "/RES_REQ/" + lblFileName.Text + "');", true);
                }
                else if (e.CommandName == "Remove1")
                {
                    if (File.Exists(Server.MapPath("~/RES_REQ/") + lblFileName.Text))
                    {
                        // If file found, delete it    
                        File.Delete(Server.MapPath("~/RES_REQ/") + lblFileName.Text);
                        lblFileName.Text = "";
                        visibleDocumentButtons(false, btnPreview, btnDelete);
                    }
                    else
                    {
                        lblFileName.Text = "";
                        visibleDocumentButtons(false, btnPreview, btnDelete);
                    }

                    if (!hana.Execute($@"UPDATE ""RDOC"" SET ""FileName""= NULL Where ""DocEntry"" = '{lblDocEntry.Text}'and ""DocID"" = '{docId}'", hana.GetConnection("SAOHana")))
                    {
                        alertMsg("Error in deleting attached documents", "error");
                    }
                    loadRestructuringDocReq();
                }
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }
        }

        protected void tDocDate_TextChanged(object sender, EventArgs e)
        {
            //Compute();
            //loadSchedules();
        }

        protected void btnRetType_ServerClick(object sender, EventArgs e)
        {
            string qry = $@"Select 'ABCI' as ""Name"" from ""DUMMY"" union all select 'BUYERS' as ""Name"" from ""DUMMY""";
            gvRetType.DataSource = hana.GetData(qry, hana.GetConnection("SAPHana"));
            gvRetType.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgRetitlingType", "MsgRetitlingType_Show();", true);
        }

        protected void bRetitlingType_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string Name = GetID.CommandArgument;
            tRetType.Value = Name;
            DataTable dt = ws.GetHouseDetails(tProjCode.Value, tBlock.Value, tLot.Value, tFinancing.Value).Tables["GetHouseDetails"];
            if (DataAccess.Exist(dt) == true)
            {
                GetHouseDetails(dt);

            }



            //2023-07-03 : RECALCULATION OF MISCELLANEOUS 
            txtMiscFees.Text = "0.00";
            MiscUpdate();

            //2023-10-19 : Change position ; moved after MIscUpdate();
            Compute(1);

            ScriptManager.RegisterStartupScript(this, GetType(), "MsgRetitlingType", "MsgRetitlingType_Hide();", true);
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


        protected void gvCoOwner_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCoOwner.PageIndex = e.NewPageIndex;
            loadCoOwner();
        }


        void loadCoOwner()
        {
            DataTable dtCoOwner = hana.GetData($@"SELECT ""CardCode"" ""Code"", ""Name"" FROM ""QUT10"" WHERE ""DocEntry"" = {lblDocEntry.Text}", hana.GetConnection("SAOHana"));
            //DataTable dt = (DataTable)ViewState["CoOwner"];
            ViewState["CoOwner"] = dtCoOwner;
            gvCoOwner.DataSource = dtCoOwner;
            gvCoOwner.DataBind();
        }


        protected void btnCoOwner_ServerClick(object sender, EventArgs e)
        {
            loadCoOwner();
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

        protected void bCreateNewCoOwner_ServerClick(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            dt = hana.GetData("CALL sp_GetBP;", hana.GetConnection("SAOHana"));
            ViewState["gvBPList"] = dt;
            LoadData(gvBPList, "gvBPList");

            ViewState["BPSelection"] = "CO-OWNER";
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgBPList_Show", "MsgBPList_Show();", true);
        }



        protected void btnYes_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "confirmation", "hideConfirm();", true);

            DataTable test1 = (DataTable)ViewState["gvDownPayment"];
            test1 = (DataTable)ViewState["gvAmortization"];
            string test3 = ViewState["tDPPercent"].ToString();
            string test4 = ViewState["txtDiscPercent"].ToString();
            string test5 = ViewState["QuoteDocEntry"].ToString();
            string test6 = Session["UserID"].ToString();
            string test7 = ViewState["DocNum"].ToString();
            string test8 = ViewState["QuoteDocEntry"].ToString();
            double gdi = string.IsNullOrWhiteSpace(txtGDI.Value) ? 0 : double.Parse(txtGDI.Value);
            string floorArea = string.IsNullOrWhiteSpace(tFloorArea.Value) ? "0" : tFloorArea.Value;
            double IPS = string.IsNullOrWhiteSpace(txtPenaltyAmount.Text) ? 0 : double.Parse(txtPenaltyAmount.Text);
            string OriginalNetDAS = ViewState["OriginalNETDAS"].ToString();


            DataTable dtUserAccess1 = (DataTable)Session["UserAccess"];
            var CC = dtUserAccess1.Select($"CodeEncrypt= 'CC'");
            string CCValue = "N";
            if (CC.Any())
            {
                CCValue = "Y";
                txtRequestLetterApprovalDate.Value = tDocDate.Text;
            }

            string errorDesc = "";
            try
            {
                SapHanaLayer company = new SapHanaLayer();
                if (company.Connect())
                {



                    //2023-06-27 : ADD BLOCKING WHEN AGENT SELECTED HAS NO SHARING DETAILS
                    DataTable dtSharingDetails = ws.GetIncentiveAgents(lblEmployeeID.Text, hPrjCode.Value, txtProductType.Value).Tables[0];
                    if (dtSharingDetails.Rows.Count > 0)
                    {






                        //2023-06-23 : GET RESTRUCTURING TYPE 
                        string qryRestructureAction = $@"SELECT IFNULL(""U_Type"",'UPDATE') ""Type"" FROM ""@RESTRUCTURINGTYPE"" WHERE ""Code"" = '{ddlRestructureType.Text}'";
                        DataTable dtRestructureAction = hana.GetData(qryRestructureAction, hana.GetConnection("SAPHana"));
                        string RestructureAction = DataAccess.GetData(dtRestructureAction, 0, "Type", "").ToString();

                        //2023-05-31 : BLOCK RESTRUCTURING WHEN DREAMS DOCUMENT DATE 
                        DataTable dtIncomingPaymentsBlock1 = new DataTable();
                        string qryIncomingPaymentsBlock1 = $@"select ""DocEntry"", ""DocDate"" from ORCT where ""U_BlockNo"" = '{hBlock.Value}' and ""U_LotNo"" = '{hLot.Value}' AND ""PrjCode"" = '{hPrjCode.Value}' AND ""Canceled"" = 'N' 
                                                AND ""DocDate"" > '{Convert.ToDateTime(tDocDate.Text).ToString("yyyyMMdd")}' AND IFNULL(""U_Restructured"",'N') = 'N' ";
                        dtIncomingPaymentsBlock1 = hana.GetData(qryIncomingPaymentsBlock1, hana.GetConnection("SAPHana"));

                        //2023-06-23 : CONSIDER BLOCKING IF UPDATE OR NEW ONLY
                        int ctr1 = 0;
                        if (RestructureAction == "NEW")
                        {
                            if (dtIncomingPaymentsBlock1.Rows.Count > 0)
                            {
                                ctr1++;
                            }
                        }


                        //2023-05-31 : BLOCK RESTRUCTURING WHEN DREAMS DOCUMENT DATE 
                        //if (dtIncomingPaymentsBlock1.Rows.Count == 0)

                        //2023-06-23 : CHANGED BLOCKING ; CONSIDER RESTRUCTURING TYPE
                        if (ctr1 == 0)
                        {

                            //CHECK INCOMING PAYMENT DATES 
                            DataTable dtIncomingPaymentsBlock2 = new DataTable();
                            string qryIncomingPaymentsBlock2 = $@"select ""DocEntry"" from ORCT where ""U_BlockNo"" = '{hBlock.Value}' and ""U_LotNo"" = '{hLot.Value}' AND ""PrjCode"" = '{hPrjCode.Value}' AND ""Canceled"" = 'N' 
                                                AND ""DocDate"" > '{Convert.ToDateTime(txtRestructuringDate.Value).ToString("yyyyMMdd")}'  AND IFNULL(""U_Restructured"",'N') = 'N' ";
                            dtIncomingPaymentsBlock2 = hana.GetData(qryIncomingPaymentsBlock2, hana.GetConnection("SAPHana"));

                            //2023-06-23 : CONSIDER BLOCKING IF UPDATE OR NEW ONLY
                            int ctr2 = 0;
                            if (RestructureAction == "NEW")
                            {
                                if (dtIncomingPaymentsBlock1.Rows.Count > 0)
                                {
                                    ctr2++;
                                }
                            }


                            //2023-06-23 : CHANGED BLOCKING ; CONSIDER RESTRUCTURING TYPE
                            //if (dtIncomingPaymentsBlock2.Rows.Count == 0)
                            if (ctr2 == 0)
                            {


                                //CHECK DOCUMENT
                                DataTable dtDoc = new DataTable();
                                //string docqry = $@"select ""DocName"" from qdoc where ""CardCode"" = '{lblID.Text}' AND ""DocId"" = '29' AND ""DocEntry"" = '{lblDocEntry.Text}'";
                                string docqry = $@"select ""DocName"" from RDOC where ""DocID"" = 'BR' AND ""DocEntry"" = '{lblDocEntry.Text}'";
                                dtDoc = hana.GetData(docqry, hana.GetConnection("SAOHana"));
                                if (DataAccess.Exist(dtDoc) != true && lblBusinessType.Text == "Corporation")
                                {
                                    alertMsg("This Buyer is missing Board Resolution document requirement", "error");
                                }
                                else
                                {
                                    if (!string.IsNullOrWhiteSpace(ViewState["DocNum"].ToString()))
                                    {



                                        if (ddlRestructureType.Text != "-")
                                        {
                                            if (string.IsNullOrWhiteSpace(txtRequestLetterApprovalDate.Value) && CCValue == "N")
                                            {
                                                alertMsg("Please provide Request Letter Approval Date.", "warning");
                                                txtRequestLetterApprovalDate.Focus();

                                            }
                                            else
                                            {

                                                //CHECK IF NO LETTER OF REQUEST
                                                if (string.IsNullOrWhiteSpace(lblFileName.Text) && CCValue == "N")
                                                {
                                                    alertMsg("No Letter of Request attached. Please provide Letter of Request to proceed.", "warning");

                                                }
                                                else
                                                {

                                                    if (lblEmployeeID.Text != "")
                                                    {

                                                        bool isSuccess = true;
                                                        string Message = string.Empty;

                                                        string QuotationDocEntry = "0";
                                                        string DPDocEntry = "0";
                                                        string MiscEntry = "0";




                                                        string qryBatch = $@"SELECT ""DistNumber"",  ""U_LTSNo"" FROM OBTN WHERE ""U_BlockNo"" = '{tBlock.Value}' AND ""U_LotNo"" = '{tLot.Value}' AND ""U_Project"" = '{tProjCode.Value}'";
                                                        DataTable dtBatch = hana.GetData(qryBatch, hana.GetConnection("SAPHana"));
                                                        string batch = DataAccess.GetData(dtBatch, 0, "DistNumber", "").ToString();
                                                        string LTSNo = DataAccess.GetData(dtBatch, 0, "U_LTSNo", "").ToString();
                                                        ViewState["LOI"] = string.IsNullOrWhiteSpace(LTSNo) ? "Yes" : "No";

                                                        //string qryThreshold = $@"SELECT IFNULL(x.""U_ThresholdAmount"",0) ""ThresholdAmount"" FROM ""SBOUAT_ABCI"".""@PRODUCTTYPE"" x WHERE x.""Code"" = '{txtProductType.Value}'";
                                                        //DataTable dtThreshold = hana.GetData(qryBatch, hana.GetConnection("SAPHana"));
                                                        //string threshold = DataAccess.GetData(dtBatch, 0, "ThresholdAmount", "0").ToString();

                                                        //CHECK IF VATABLE OR NOT
                                                        DataTable dtVATThreshold = hana.GetData($@"SELECT ""U_ThresholdAmount"" FROM ""@PRODUCTTYPE"" WHERE UPPER(""Code"") = '{txtProductType.Value.ToUpper()}'", hana.GetConnection("SAPHana"));
                                                        double vatThreshold = Convert.ToDouble(DataAccess.GetData(dtVATThreshold, 0, "U_ThresholdAmount", "").ToString());
                                                        double adjacentLotPrice = 0;
                                                        string vatable = "";
                                                        if (txtAdjLot.Value == "Yes")
                                                        {
                                                            DataTable dtAdjacentLotPrice = ws.GetHouseDetails(tProjCode.Value, tBlock.Value, txtLotNo.Value, tFinancing.Value).Tables["GetHouseDetails"];
                                                            double LandPrice = Convert.ToDouble(DataAccess.GetData(dtAdjacentLotPrice, 0, "LandPrice", "0").ToString());
                                                            double HousePrice = Convert.ToDouble(DataAccess.GetData(dtAdjacentLotPrice, 0, "HousePrice", "0").ToString());
                                                            adjacentLotPrice = LandPrice + HousePrice;
                                                        }

                                                        //CHECK IF OLD DAS IS VATABLE
                                                        double TotalPrice = Convert.ToDouble(lblOldNetDAS.Text) + adjacentLotPrice;
                                                        vatable = (TotalPrice > vatThreshold ? "Y" : "N");



                                                        if (RestructureAction == "NEW")
                                                        {
                                                            string TestDocNum = ViewState["DocNum"].ToString();

                                                            //################################
                                                            //SAP -- CANCELLATION OF TRANSACTIONS
                                                            //################################


                                                            string qryLTSNo = $@"SELECT ""LTSNo"" FROM OQUT WHERE ""DocNum"" = '{ViewState["DocNum"].ToString()}'";
                                                            DataTable dtLTS = hana.GetDataDS(qryLTSNo, hana.GetConnection("SAOHana")).Tables[0];
                                                            string OQUTLTSNo = DataAccess.GetData(dtLTS, 0, "LTSNo", "").ToString();

                                                            if (!wcf.CancelTransactionsNew(

                                                                                           //OLD 
                                                                                           lblDocEntry.Text, company, //2
                                                                                           tDocDate.Text, hPrjCode.Value, //4   
                                                                                           hBlock.Value, hLot.Value, //6
                                                                                           Convert.ToDouble(lblOldDownPayment.Text), hHouseModel.Value, //8
                                                                                           lblOldTCPFinScheme.Text, Convert.ToDouble(lblOldReserveFee.Text), //10

                                                                                           //NEW
                                                                                           hProductType.Value, hCardCode.Value, //12
                                                                                           tModel.Value, tFinancing.Value, //14
                                                                                           txtProductType.Value, Convert.ToDouble(tResrvFee.Text), //16
                                                                                           Convert.ToDouble(tDPAmount.Text), Convert.ToDouble(lblNewLoanableBalance.Text), //18
                                                                                           lblID.Text, Convert.ToDouble(lblNewMiscFees.Text), //20
                                                                                                                                              //Convert.ToDouble(lblNetPaidAmount.Text), ViewState["DocNum"].ToString(), //22
                                                                                           Convert.ToDouble(lblTotalPaidAmount.Text), ViewState["DocNum"].ToString(), //22
                                                                                           tBlock.Value, tLot.Value, //24
                                                                                           Convert.ToDouble(lblNewDownPayment.Text), Convert.ToDouble(lblNewLoanableBalance.Text), //26
                                                                                                                                                                                   //Conver..t.ToDouble(lblNetTCPn.Text), vatable, //28
                                                                                           Convert.ToDouble(tNetDas.Value), vatable, //28
                                                                                           Convert.ToDouble(txtDiscountAmount.Text), Convert.ToDouble(txtMiscFees.Text), //30
                                                                                           batch, lblEmployeeName.Text, //32
                                                                                           txtAdjLot.Value, txtLotNo.Value, //34
                                                                                           tProjCode.Value, Convert.ToDouble(lblOldMiscDPMonthly.Text), //36
                                                                                           Convert.ToInt32(lblNewDPTerms.Text), ViewState["LOI"].ToString(), //38
                                                                                           Session["UserName"].ToString(), RestructureAction, //40
                                                                                           Convert.ToDouble(lblTotalPaidAmountMisc.Text), Convert.ToDouble(lblOldNetTCP.Text), //42
                                                                                           txtRestructuringDate.Value, Session["UserID"].ToString(), //44
                                                                                           ddlUpdateAmortBalance.Text, OQUTLTSNo, //46
                                                                                           0,
                                                                                           Convert.ToInt32(lblNewMiscDPTerms.Text) + Convert.ToInt32(lblNewMiscLBTerms.Text),
                                                                                           Convert.ToDouble(ViewState["OriginalNETDAS"]), "Restructured",//2024-06-13 KUNG ADVANCE PAYMENT OR RESTRUCTURING 
                                                                                           out errorDesc, out QuotationDocEntry, //48
                                                                                           out DPDocEntry, out MiscEntry //50
                                                                                           )
                                                                )
                                                            {
                                                                isSuccess = false;
                                                            }
                                                            else
                                                            {
                                                                isSuccess = true;

                                                                //isSuccess = updateCommission(company, ViewState["DocNum"].ToString(), hPrjCode.Value, hBlock.Value, hLot.Value, out errorDesc);

                                                            }



                                                        }
                                                        else
                                                        {
                                                            //RESTRUCTURING -- TAG ALL AR DP INVOICE WITH "OLD"
                                                            string qryOldARDPI = $@" UPDATE ODPI  SET ""U_RestructureTag"" = 'OLD' WHERE ""U_DreamsQuotationNo"" = '{ViewState["DocNum"].ToString()}'; 
                                                                           UPDATE OINV  SET ""U_RestructureTag"" = 'OLD' WHERE ""U_DreamsQuotationNo"" = '{ViewState["DocNum"].ToString()}';  ";
                                                            hana.Execute(qryOldARDPI, hana.GetConnection("SAPHana"));
                                                        }

                                                        string qry = "";
                                                        DataTable dt;




                                                        ////RESTRUCTURING -- CHANGE OF SALES AGENT
                                                        //if (isSuccess)
                                                        //{

                                                        //}



                                                        ////2023-06-22 : SAVING OF COMMISSION SCHEME AFTER RESTRUCTURING  
                                                        //if (isSuccess)
                                                        //{
                                                        //    //05-05-2023 : BLOCK IF COMMISION SCHEME FOR THE PROJECT DOES NOT EXIST
                                                        //    string qryCommissionScheme = $@" SELECT A.""Code"" ""CommissionCode"", A.""U_Type"" ""Type"", A.""U_Project"" ""ProjCode"",
                                                        //        IFNULL(B.""U_CollectedTCP"",0) ""CollectedTCP"", IFNULL(B.""U_Release"",0) ""Release"",	
                                                        //        IFNULL(B.""U_CommissionRelease"",0) ""CommissionRelease"",
                                                        //        IFNULL(A.""U_Commission"",0) ""CommissionPercent"" 
                                                        //        FROM ""@COMMINCSCHEME"" A INNER JOIN	""@COMMISSION"" B ON A.""Code"" = B.""Code""  
                                                        //         WHERE  A.""U_Project"" = '{hPrjCode.Value}' AND UPPER(A.""U_Type"") = UPPER('{txtProductType.Value}')";
                                                        //    DataTable dtCommissionScheme = hana.GetData(qryCommissionScheme, hana.GetConnection("SAPHana"));

                                                        //    if (dtCommissionScheme != null)
                                                        //    {
                                                        //        foreach (DataRow dr in dtCommissionScheme.Rows)
                                                        //        {
                                                        //            //2023-06-22 : SAVING OF COMMISSION SCHEME TO QUT14
                                                        //            ws.sp_SaveCommissionScheme(lblDocEntry.Text, dr["CommissionCode"].ToString(),

                                                        //                                    dr["Type"].ToString(), dr["ProjCode"].ToString(),
                                                        //                                    double.Parse(dr["CollectedTCP"].ToString()), int.Parse(dr["Release"].ToString()),
                                                        //                                    double.Parse(dr["CommissionRelease"].ToString()), double.Parse(dr["CommissionPercent"].ToString()),
                                                        //                                    (int)Session["UserID"], DateTime.Now.ToString("yyyy-MM-dd")
                                                        //                                    );
                                                        //        }
                                                        //    }



                                                        //}




                                                        if (isSuccess)
                                                        {

                                                            //SAP POSTING BEFORE DELETING QUT1 ROWS
                                                            double cash = 0;
                                                            double check = 0;
                                                            double credit = 0;
                                                            double interbranch = 0;

                                                            string qryBPCode2 = $@"SELECT ""CardCode"" FROM OCRD WHERE ""CardFName"" = '{lblID.Text}'";
                                                            DataTable dtCardCode2 = hana.GetData(qryBPCode2, hana.GetConnection("SAPHana"));
                                                            //GET CASH, CHECK, CREDIT, AND INTERBRANCH/OTHERS TOTAL
                                                            dt = hana.GetData($@"SELECT SUM(""CashSum"") ""Cash"" FROM QUT2 WHERE ""DocEntry"" = {lblDocEntry.Text}", hana.GetConnection("SAOHana"));
                                                            cash = Convert.ToDouble(DataAccess.GetData(dt, 0, "Cash", "0").ToString());
                                                            dt = hana.GetData($@"SELECT SUM(""CheckSum"") ""CheckSum"" FROM QUT3 WHERE ""DocEntry"" = {lblDocEntry.Text}", hana.GetConnection("SAOHana"));
                                                            check = Convert.ToDouble(DataAccess.GetData(dt, 0, "CheckSum", "0").ToString());
                                                            dt = hana.GetData($@"SELECT SUM(""CreditSum"") ""CreditSum"" FROM QUT4 WHERE ""DocEntry"" = {lblDocEntry.Text}", hana.GetConnection("SAOHana"));
                                                            credit = Convert.ToDouble(DataAccess.GetData(dt, 0, "CreditSum", "0").ToString());
                                                            dt = hana.GetData($@"SELECT SUM(""BankSum"") ""BankSum"" FROM QUT7 WHERE ""DocEntry"" = {lblDocEntry.Text}", hana.GetConnection("SAOHana"));
                                                            interbranch = Convert.ToDouble(DataAccess.GetData(dt, 0, "BankSum", "0").ToString());



                                                            //2023-06-27 : CHECK IF SALES AGENT IS CHANGED
                                                            int checkAgent = 0;
                                                            if (hSalesAgentID.Value != lblEmployeeID.Text)
                                                            {
                                                                checkAgent = 1;
                                                            }




                                                            if (!ws.Restructure(
                                                                   (int)ViewState["QuoteDocEntry"],
                                                                   DataAccess.GetSession(lblID.Text, ""),  //2
                                                                   Convert.ToDateTime(tDocDate.Text),
                                                                   "O", //4
                                                                   tProjCode.Value,

                                                                   tBlock.Value, //6
                                                                   tLot.Value,
                                                                   tModel.Value,  //8
                                                                   tFinancing.Value,
                                                                   tLotArea.Value, //10

                                                                   (floorArea == "-" ? "0" : floorArea),
                                                                   tHouseStatus.Value, //12
                                                                   tPhase.Value,
                                                                   txtLotClassification.Value, //14 
                                                                   txtProductType.Value,

                                                                   txtLoanType.Value, //16
                                                                   tBank.Value,





                                                                   double.Parse(lblNewDasAmt.Text), //18
                                                                   double.Parse(tResrvFee.Text),
                                                                   double.Parse(ViewState["tDPPercent"].ToString()), //20

                                                                   double.Parse(lblNewDownPayment.Text),
                                                                   int.Parse(txtDPTerms.Text), //22
                                                                   double.Parse(ViewState["txtDiscPercent"].ToString()),
                                                                   double.Parse(txtDiscountAmount.Text), //24
                                                                   int.Parse(txtLTerms.Text),

                                                                   double.Parse(txtFactorRate1.Text), //26 
                                                                   Convert.ToDateTime(lblDueDate2n.Text),
                                                                   gdi, //28
                                                                   double.Parse(lblNewDasAmt.Text),
                                                                   double.Parse(lblOldAddMiscCharges.Text),  //30

                                                                   //double.Parse(lblDASAmtn.Text),
                                                                   //double.Parse(tNetDas.Value),

                                                                   //POSTING TO DAS NET OF DISCOUNT
                                                                   double.Parse(lblNewDasAmt.Text) + double.Parse(lblNewDiscount.Text), //31

                                                                   double.Parse(lblNewVAT.Text),  //32
                                                                   double.Parse(tNetDas.Value),
                                                                   double.Parse(lblOldDownPayment.Text), //34
                                                                   double.Parse(lblNewDPMonthly.Text),

                                                                   Convert.ToDateTime(lblNewDPDueDate.Text), //36
                                                                   double.Parse(lblNewLoanableBalance.Text),
                                                                   (int)Session["UserID"], //38
                                                                   lblEmployeeID.Text,
                                                                   lblEmployeeName.Text, //40

                                                                   lblEmployeePosition.Text,
                                                                   (DataTable)ViewState["gvDownPayment"],  //42
                                                                   (DataTable)ViewState["gvAmortization"],
                                                                   ViewState["DocNum"].ToString(), //44
                                                                   double.Parse(txtMiscMonthly.Text),

                                                                   //2023-06-27 : CHANGED SOURCE FOR TCPMONTHLY (DP MONTHLY)
                                                                   //double.Parse(lblNewMiscFees.Text), //46
                                                                   double.Parse(lblNewDPMonthly.Text), //46


                                                                   double.Parse(lblNewMiscDPMonthly.Text),

                                                                   double.Parse(lblNewAddMiscFees.Text), //48
                                                                   double.Parse(tPDBalance.Text),
                                                                   double.Parse(lblOldBalance.Text), //50

                                                                   double.Parse(txtMiscFees.Text),
                                                                   double.Parse(lblNewLoanableBalance.Text), //52
                                                                   double.Parse(lblNewLoanableBalance.Text),
                                                                   txtOtherRestructuringType.Text,  //54
                                                                   tRetType.Value,

                                                                   double.Parse(lblOldCompTotal.Text), //56 
                                                                   int.Parse(hDPTerms.Value),
                                                                   Convert.ToDateTime(lblOldDPDueDate.Text), //58
                                                                   Convert.ToDateTime(hDPDueDate.Value),
                                                                   double.Parse(lblOldDASAmt.Text), //60 

                                                                   double.Parse(lblOldDownPayment.Text),
                                                                   int.Parse(hLBTerms.Value), //62
                                                                   Convert.ToDateTime(hLBDueDate.Value),
                                                                   Convert.ToDateTime(lblNewLBDueDate.Text),//64
                                                                   Convert.ToDouble(lblTotalPaidAmount.Text),

                                                                   lblOldTCPFinScheme.Text,//66
                                                                   hProductType.Value,
                                                                   hLot.Value, //68
                                                                   hHouseModel.Value,
                                                                   hPrjCode.Value, //70

                                                                   ddlRestructureType.Text,
                                                                   Convert.ToDateTime(txtRequestLetterApprovalDate.Value), //72
                                                                   lblFileName.Text,
                                                                   QuotationDocEntry,
                                                                   DPDocEntry,

                                                                   lblEmployeeID.Text,
                                                                   lblEmployeeName.Text,
                                                                   lblEmployeePosition.Text,
                                                                   RestructureAction,
                                                                   IPS,

                                                                   vatable,
                                                                   Convert.ToDouble(lblNewLBMonthly.Text),

                                                                   Convert.ToDateTime(txtRestructuringDate.Value),
                                                                   MiscEntry,
                                                                   Convert.ToDouble(lblTotalPaidAmountMisc.Text),
                                                                   hCardCode.Value,
                                                                   hRetitlingType.Value,

                                                                   ddlUpdateAmortBalance.Text,

                                                                   //NEW FIELDS 2023-03-07
                                                                   lblComaker.Text,
                                                                   Convert.ToDateTime(lblNewMiscDueDate.Text),
                                                                   txtFinancingMisc.Value,
                                                                   int.Parse(lblNewMiscDPTerms.Text),
                                                                   int.Parse(lblOldMiscDPTerms.Text),
                                                                   (DataTable)ViewState["gvMiscellaneous"],
                                                                   (DataTable)ViewState["gvMiscellaneousAmort"],

                                                                   double.Parse(lblNewMiscDPAmount.Text),
                                                                   int.Parse(lblNewMiscLBTerms.Text),
                                                                   double.Parse(lblNewMiscLBAmount.Text),
                                                                   double.Parse(lblNewMiscLBMonthly.Text),

                                                                   //2023-06-27 : CHECKING IF SALES AGENT CHANGED
                                                                   checkAgent,

                                                                   out Message // 74
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


                                                                    //2023-06-26 : LINIPAT PWESTO
                                                                    //if (hSalesAgentID.Value != lblEmployeeID.Text)
                                                                    //2023-06-27 : CHANGED TO CHECKING INSTEAD
                                                                    if (checkAgent > 0)
                                                                    {

                                                                        //UPDATE ALL EXISTING AGENTS TO RESTRUCTURED 
                                                                        isSuccess = hana.Execute($@"UPDATE OPCH SET ""LineStatus"" = 'R' WHERE ""DocEntry"" = {lblDocEntry.Text}", hana.GetConnection("SAOHana"));

                                                                        //2023-06-23 : COMMENTED; MOVED TO SP_RESTRUCTURE
                                                                        //isSuccess = hana.Execute($@"UPDATE QUT11 SET ""LineStatus"" = 'R' WHERE ""DocEntry"" = {lblDocEntry.Text}", hana.GetConnection("SAOHana"));
                                                                        //isSuccess = hana.Execute($@"UPDATE QUT5 SET ""LineStatus"" = 'R' WHERE ""DocEntry"" = {lblEmployeeID.Text}", hana.GetConnection("SAOHana"));


                                                                        DataTable dtIncentiveAgents = ws.GetIncentiveAgents(lblEmployeeID.Text, tProjCode.Value, txtProductType.Value).Tables[0];

                                                                        if (dtIncentiveAgents != null)
                                                                        {

                                                                            foreach (DataRow dr in dtIncentiveAgents.Rows)
                                                                            {
                                                                                ws.sp_SaveIncentiveCommissioninformation(
                                                                                               lblDocEntry.Text,
                                                                                               dr["Position"].ToString(),
                                                                                               dr["SAPCardCode"].ToString(),
                                                                                               dr["SalesPerson"].ToString(),
                                                                                               Convert.ToInt32(Session["UserID"]),
                                                                                               dr["Status"].ToString(),
                                                                                               dr["Id"].ToString(),
                                                                                               dr["Percentage"].ToString(),
                                                                                               dr["HouseAndLotPercentage"].ToString());
                                                                            }
                                                                        }

                                                                    }




                                                                    lblFinish.InnerText = "Finish";
                                                                    alertMsg("Operation completed successfully", "success");
                                                                    ClearProfileTab();
                                                                    ClearHouseDetails("Project");
                                                                    clearComputationTabs();
                                                                    clearPage();
                                                                    clearRestructuringDocuments();
                                                                    PrevTab("HouseDetails");
                                                                    ScriptManager.RegisterStartupScript(this, GetType(), "clear", "ResetTextBox()", true);
                                                                }
                                                            }

                                                            //}
                                                            //else
                                                            //{
                                                            //    alertMsg("Updating of Quotation Status failed. Please contact administrator. (3)", "error");
                                                            //}


                                                        }
                                                        else
                                                        {
                                                            alertMsg("Restructuring failed. Please contact administrator. (" + errorDesc + ")", "warning");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        alertMsg("Sales Agent is required to proceed.", "warning");
                                                        btnSelectAgent_Click(sender, e);
                                                    }
                                                }
                                            }
                                        }



                                        else
                                        {
                                            alertMsg("Please provide restructuring type.", "info");
                                            ddlRestructureType.Focus();
                                        }

                                    }
                                    else
                                    {
                                        alertMsg("Please reload the page. (Document Number session is missing)", "info");
                                    }
                                }

                            }
                            else
                            {
                                alertMsg("Restructuring Date specified is less than payment dates posted.", "info");

                            }
                        }
                        else
                        {
                            alertMsg("Document Date specified is less than payment dates posted.", "info");

                        }
                    }
                    else
                    {
                        alertMsg("No sharing details found for the selected agent. Please contact administrator.", "info");
                    }
                }
                else
                {
                    alertMsg("DREAMS is not able to connect to SAP database. Please try again. If error persists, please contact administrator.", "info");
                }
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "warning");
            }
        }

        protected void ddlUpdateAmortBalance_SelectedIndexChanged(object sender, EventArgs e)
        {
            Compute();
            loadSchedules();
        }

        protected void ddlRetainMonthlyAmort_SelectedIndexChanged(object sender, EventArgs e)
        {
            Compute();
            loadSchedules();
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












        //################################################################################
        //################################################################################
        //2023-05-24 : ADD LOADING OF CO-BORROWER
        //################################################################################
        //################################################################################

        #region CO-BORROWER

        protected void btnCoMaker_ServerClick(object sender, EventArgs e)
        {
            loadCoMaker();
        }

        void loadCoMaker()
        {
            string qrytemp = $@"select IFNULL(""LastName"",'') || ', ' || IFNULL(""FirstName"",'') || ' ' || IFNULL(""MiddleName"",'') ""Name""  from ""temp_CRD5"" where ""UserID"" = '{Session["UserID"].ToString()}' and ""CoBorrower"" = true";
            string qry = $@"select IFNULL(""LastName"",'') || ', ' || IFNULL(""FirstName"",'') || ' ' || IFNULL(""MiddleName"",'') ""Name""   from crd5 where ""CoBorrower"" = true and ""CardCode"" = '{txtCardCode.Value}' union all ";
            DataTable dt = String.IsNullOrEmpty(txtCardCode.Value) ? hana.GetData(qrytemp, hana.GetConnection("SAOHana")) : hana.GetData(qry + qrytemp, hana.GetConnection("SAOHana"));
            gvCoMaker.DataSource = dt;
            gvCoMaker.DataBind();
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

        protected void bSelectCoMaker_Click2(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string Name = GetID.CommandArgument;
            txtComaker1.Value = Name;
            lblComaker.Text = Name;
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgCoMaker_Hide", "MsgCoMaker_Hide();", true);
        }

        protected void bDeleteCoMaker_Click(object sender, EventArgs e)
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

        protected void Button1_ServerClick(object sender, EventArgs e)
        {
            ClearCoBorrower();
            //btnEditProfile.Visible = true;
            //ViewState["QuoteDocEntry"]  = 0;
            BoolBP(false);
            ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "MsgBuyers_Hide();", true);
            //ScriptManager.RegisterStartupScript(this, GetType(), "NewQuotationShow", "MsgNewQuotation_Show();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgSampleActual", "MsgCoBorrower_Show();", true);
        }
        void ClearCoBorrower()
        {
            mtxtRow.Text = string.Empty;
            mtxtLName.Text = string.Empty;
            mtxtFName.Text = string.Empty;
            mtxtMName.Text = string.Empty;
            mtxtRelationship.Text = string.Empty;
        }


        #endregion

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
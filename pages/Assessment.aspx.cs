using DirecLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABROWN_DREAMS
{
    public partial class Assessment : System.Web.UI.Page
    {
        DirecWebService ws = new DirecWebService();
        DirecService wcf = new DirecService();
        SAPHanaAccess hana = new SAPHanaAccess();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserID"] == null)
            {
                alertMsg("Session expired!", "error");
                Response.Redirect("~/pages/Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadBuyers();
                tSurChargeDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                //show buyer selection
                ScriptManager.RegisterStartupScript(this, this.GetType(), "close", "showBuyer();", true);
            }


        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            //show buyer selection
            ScriptManager.RegisterStartupScript(this, this.GetType(), "close", "showBuyer();", true);
        }

        protected void gvHouseLot_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Sel"))
            {
                int row = int.Parse(e.CommandArgument.ToString());
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
                Label lblDocNum = (Label)gvHouseLot.Rows[row].FindControl("lblDocNum");

                //load data
                tProj.Text = lblProject.Text;
                tBlock.Text = lblBlock1.Text;
                tLot.Text = lblLot1.Text;
                tModel.Text = lblModel1.Text;
                //tPhase.Text = string.Format("{0:#,#.##}", Convert.ToDecimal(lblPhase.Text));
                tPhase.Text = lblPhase.Text;
                tFinScheme.Text = lblFinscheme.Text;
                lblQuotationNum.Text = lblDocNum.Text;
                tTcp.Text = SystemClass.ToCurrency(gvHouseLot.Rows[row].Cells[0].Text);
                Session["FinCode"] = lblFinschemeCode.Text;
                Session["Status"] = lblStatus.Text;
                Session["SQDocEntry"] = int.Parse(lblDocEntry.Text);
                Session["DPEntry"] = int.Parse(gvHouseLot.Rows[row].Cells[1].Text);
                Session["FinScheme"] = lblFinscheme.Text;
                //** DOWNPAYMENT **//
                //if ((string)Session["Status"] == ConfigSettings.Forwarded"].ToString())
                //{
                gvDownPayments.Visible = true;
                gvDownPayments.DataSource = ws.GetDownPayments(int.Parse(lblDocEntry.Text), lblFinscheme.Text);
                gvDownPayments.DataBind();
                //}

                ComputeTotal();

                //** highlight selected row **//
                foreach (GridViewRow selectedrow in gvHouseLot.Rows)
                {
                    if (selectedrow.RowIndex == row)
                    { selectedrow.BackColor = System.Drawing.ColorTranslator.FromHtml("#A1DCF2"); }
                    else
                    { selectedrow.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF"); }
                }
            }
        }

        protected void bSearchBuyer_Click(object sender, EventArgs e)
        {
            Session["Buyers"] = ws.SearchBuyersAssessment(txtSearchBuyer.Value);
            gvBuyers.DataSource = (DataSet)Session["Buyers"];
            gvBuyers.DataBind();
        }
        protected void btnClose_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/pages/Dashboard.aspx");
        }
        void LoadBuyers()
        {
            Session["Buyers"] = ws.BuyersAssessment();
            gvBuyers.DataSource = (DataSet)Session["Buyers"];
            gvBuyers.DataBind();
        }
        void ComputeTotal()
        {
            double payment = 0;
            double total = 0;
            double totalamtpd = 0;
            double postedamt = 0;
            double principal = 0;
            double loanable = 0;
            double dpamount = 0;
            double duepayments = 0;


            //if ((string)Session["Status"] == ConfigSettings.Forwarded"].ToString())
            //{



            //** CLEAR DATA **//
            foreach (GridViewRow row in gvDownPayments.Rows)
            {
                //clear balance
                if (double.Parse(row.Cells[7].Text) == 0 || row.Cells[2].Text != "DP")
                {
                    row.Cells[7].Text = "0.00";
                    row.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                }
            }








            //######################################
            //####### START COMPUTE SURCHAGE #######
            //######################################

            //** compute penalty & loanable **//
            double running_bal = 0;
            double total_penalties = 0;



            #region SURCHARGE

            string qry = $@"SELECT ""AcctType"" FROM OQUT WHERE ""DocEntry"" = '{Session["SQDocEntry"].ToString()}'";
            DataTable dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
            string AcctType = DataAccess.GetData(dt, 0, "AcctType", "0").ToString();

            qry = $@"SELECT ""U_Surcharge"" FROM ""@PRODUCTTYPE"" WHERE UPPER(""Code"") = UPPER('{AcctType}')";
            dt = hana.GetData(qry, hana.GetConnection("SAPHana"));

            double dppenalty = double.Parse(DataAccess.GetData(dt, 0, "U_Surcharge", "0").ToString());
            double mapenalty = double.Parse(DataAccess.GetData(dt, 0, "U_Surcharge", "0").ToString());

            //double dppenalty = double.Parse(ConfigSettings.DPPenalty"].ToString());
            //double mapenalty = double.Parse(ConfigSettings.MAPenalty"].ToString());




            foreach (GridViewRow row in gvDownPayments.Rows)
            {

                if (row.Cells[3].Text == "LB" && row.Cells[2].Text == "2")
                {
                    var test = "";
                }

                string LineStatus = row.Cells[13].Text;
                if (LineStatus == "O")
                {

                    //CheckBox chk = row.FindControl("chk") as CheckBox;
                    TextBox txtPenalty = (TextBox)row.FindControl("txtPenalty");

                    //IF DUE DATE IS PAST DUE
                    if (Convert.ToDateTime(row.Cells[3].Text) <= Convert.ToDateTime(tSurChargeDate.Text))
                    {
                        if (!(Convert.ToDateTime(tSurChargeDate.Text).Month - Convert.ToDateTime(row.Cells[3].Text).Month >= 1))
                        {
                            //IF NOT MISCELLANEOUS 
                            if (row.Cells[2].Text != "MISC")
                            {
                                //GET RUNNING BALANCE (SUM OF PAYMENTAMOUNT)
                                if (double.Parse(row.Cells[9].Text) > 0)
                                {

                                    // GET EXISTING SURCHARGE PAYMENTS
                                    string qryGetSurchargePayments = $@"SELECT IFNULL(SUM(A.""DocTotal""),0) ""DocTotal"" FROM OINV A WHERE A.""Project"" = '{tProj.Text}' AND
								                                                  A.""U_BlockNo"" = '{tBlock.Text}' AND A.""U_LotNo"" = '{tLot.Text}' AND 
                                                                                  A.""U_TransactionType"" = 'RE - SUR' AND A.""U_Type"" = '{row.Cells[3].Text}' AND 
								                                                  A.""U_PaymentOrder"" = '{row.Cells[1].Text}' AND	A.""CANCELED"" <> 'Y'  AND A.""DocStatus"" = 'C'";
                                    DataTable dtGetSurchargePayments = hana.GetData(qryGetSurchargePayments, hana.GetConnection("SAPHana"));
                                    double GetSurchargePayments = double.Parse(DataAccess.GetData(dtGetSurchargePayments, 0, "DocTotal", "0").ToString());

                                    double interest = double.Parse(row.Cells[6].Text);
                                    //if (GetSurchargePayments != 0)
                                    if (double.Parse(row.Cells[12].Text) != 0)
                                    {

                                        if (((interest + GetSurchargePayments) - double.Parse(row.Cells[12].Text) < 0))
                                        {
                                            running_bal = double.Parse(row.Cells[5].Text) + ((interest + GetSurchargePayments) - double.Parse(row.Cells[12].Text));
                                        }
                                        else
                                        {
                                            running_bal = double.Parse(row.Cells[5].Text) + ((interest + GetSurchargePayments) - double.Parse(row.Cells[12].Text));
                                        }

                                    }
                                    else
                                    {
                                        running_bal = double.Parse(row.Cells[5].Text) + interest;
                                    }

                                    //running_bal = double.Parse(row.Cells[4].Text);
                                }

                                double penalty = 0;

                                //Get Day, Month, and Year of the month
                                int year = DateTime.Now.Year;
                                int month = DateTime.Now.Month;

                                double monthsdue = 0;
                                double daysdue = (Convert.ToDateTime(tSurChargeDate.Text) - Convert.ToDateTime(row.Cells[3].Text)).TotalDays;


                                while (daysdue > 30)
                                {
                                    monthsdue += 1;
                                    daysdue -= 30;
                                }


                                monthsdue = monthsdue < 1 ? 0 : monthsdue;
                                daysdue = daysdue < 1 ? 0 : daysdue;
                                int days = 30;

                                DataTable dtSurcharge = ws.ComputeSurcharge(running_bal, dppenalty, monthsdue, daysdue).Tables[0];
                                penalty = Convert.ToDouble(DataHelper.DataTableRet(dtSurcharge, 0, "Penalty", "0.00"));


                                //WAIVED SURCHARGE
                                string qry1 = $@"SELECT ""Misc"",""Penalty"" FROM QUT1 WHERE ""DocEntry"" = {row.Cells[0].Text} AND ""Terms"" = '{row.Cells[1].Text}' AND ""PaymentType"" = '{row.Cells[2].Text}'";
                                dt = hana.GetData(qry1, hana.GetConnection("SAOHana"));
                                int waiveTag = Convert.ToInt32(DataAccess.GetData(dt, 0, "Misc", "0").ToString());
                                if (waiveTag == 1)
                                {
                                    penalty = double.Parse(DataAccess.GetData(dt, 0, "Penalty", "0").ToString());
                                    txtPenalty.BackColor = System.Drawing.Color.LightSkyBlue;
                                }

                                //2023-06-29: ALWAYS GENERATE PENALTY COLUMN 
                                //row.Cells[7].Text = SystemClass.ToCurrency(penalty.ToString());
                                row.Cells[7].Text = SystemClass.ToCurrency(Convert.ToDouble(DataHelper.DataTableRet(dtSurcharge, 0, "Penalty", "0.00")).ToString());

                                txtPenalty.Text = SystemClass.ToCurrency(penalty.ToString());

                                //** if due row color = red
                                row.Cells[3].ForeColor = System.Drawing.ColorTranslator.FromHtml("#a94442");
                                row.Cells[3].Font.Bold = true;
                            }
                            else
                            {
                                row.Cells[7].Text = "0.00";
                                row.Cells[3].ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                            }
                        }


                        else
                        {
                            //if (Convert.ToDateTime(tSurChargeDate.Text).Month - Convert.ToDateTime(row.Cells[3].Text).Month == 1 && Convert.ToDateTime(tSurChargeDate.Text).Day == 1)
                            //{
                            if (row.Cells[2].Text != "MISC")
                            {
                                //GET RUNNING BALANCE (SUM OF PAYMENTAMOUNT)
                                if (double.Parse(row.Cells[9].Text) > 0)
                                {
                                    // GET EXISTING SURCHARGE PAYMENTS
                                    string qryGetSurchargePayments = $@"SELECT IFNULL(SUM(A.""DocTotal""),0) ""DocTotal"" FROM OINV A WHERE A.""Project"" = '{tProj.Text}' AND
								                                                  A.""U_BlockNo"" = '{tBlock.Text}' AND A.""U_LotNo"" = '{tLot.Text}' AND 
                                                                                  A.""U_TransactionType"" = 'RE - SUR' AND A.""U_Type"" = '{row.Cells[3].Text}' AND 
								                                                  A.""U_PaymentOrder"" = '{row.Cells[1].Text}' AND	A.""CANCELED"" <> 'Y'  AND A.""DocStatus"" = 'C'";
                                    DataTable dtGetSurchargePayments = hana.GetData(qryGetSurchargePayments, hana.GetConnection("SAPHana"));
                                    double GetSurchargePayments = double.Parse(DataAccess.GetData(dtGetSurchargePayments, 0, "DocTotal", "0").ToString());

                                    double interest = double.Parse(row.Cells[6].Text);
                                    //if (GetSurchargePayments != 0)
                                    if (double.Parse(row.Cells[12].Text) != 0)
                                    {


                                        if (((interest + GetSurchargePayments) - double.Parse(row.Cells[12].Text) < 0))
                                        {
                                            running_bal = double.Parse(row.Cells[5].Text) + ((interest + GetSurchargePayments) - double.Parse(row.Cells[12].Text));
                                        }
                                        else
                                        {
                                            running_bal = double.Parse(row.Cells[5].Text) + ((interest + GetSurchargePayments) - double.Parse(row.Cells[12].Text));
                                        }

                                    }
                                    else
                                    {

                                        running_bal = double.Parse(row.Cells[5].Text) + interest;
                                    }

                                    //running_bal = double.Parse(row.Cells[4].Text);


                                }
                                double penalty = 0;

                                //Get Day, Month, and Year of the month
                                int year = DateTime.Now.Year;
                                int month = DateTime.Now.Month;
                                double monthsdue = 0;
                                double daysdue = (Convert.ToDateTime(tSurChargeDate.Text) - Convert.ToDateTime(row.Cells[3].Text)).TotalDays;


                                while (daysdue > 30)
                                {
                                    monthsdue += 1;
                                    daysdue -= 30;
                                }
                                monthsdue = monthsdue < 1 ? 0 : monthsdue;
                                daysdue = daysdue < 1 ? 0 : daysdue;
                                int days = 30;



                                DataTable dtSurcharge = ws.ComputeSurcharge(running_bal, dppenalty, monthsdue, daysdue).Tables[0];
                                penalty = Convert.ToDouble(DataHelper.DataTableRet(dtSurcharge, 0, "Penalty", "0.00"));



                                //WAIVED SURCHARGE -- CHECK TAGGING IN "MISC" COLUMN IF IT IS WAIVED
                                string qry1 = $@"SELECT ""Misc"",""Penalty"" FROM QUT1 WHERE ""DocEntry"" = {row.Cells[0].Text} AND ""Terms"" = '{row.Cells[1].Text}' AND ""PaymentType"" = '{row.Cells[2].Text}'";
                                dt = hana.GetData(qry1, hana.GetConnection("SAOHana"));
                                int waiveTag = Convert.ToInt32(DataAccess.GetData(dt, 0, "Misc", "0").ToString());
                                if (waiveTag == 1)
                                {
                                    penalty = Convert.ToDouble(DataAccess.GetData(dt, 0, "Penalty", "0").ToString());
                                    txtPenalty.BackColor = System.Drawing.Color.LightSkyBlue;
                                }

                                //2023-06-29: ALWAYS GENERATE PENALTY COLUMN 
                                //row.Cells[7].Text = SystemClass.ToCurrency(penalty.ToString());
                                row.Cells[7].Text = SystemClass.ToCurrency(Convert.ToDouble(DataHelper.DataTableRet(dtSurcharge, 0, "Penalty", "0.00")).ToString());

                                txtPenalty.Text = SystemClass.ToCurrency(penalty.ToString());

                            }
                            else
                            {
                                if (row.Cells[2].Text == "DP")
                                {
                                    txtPenalty.Text = row.Cells[7].Text;
                                }
                            }
                            row.Cells[3].ForeColor = System.Drawing.ColorTranslator.FromHtml("#a94442");
                            row.Cells[3].Font.Bold = true;
                            //}
                        }
                    }
                    else
                    {
                        row.Cells[7].Text = "0.00";
                        row.Cells[3].ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                    }











                    //get principal
                    principal += double.Parse(row.Cells[5].Text);

                    //Block editing of Surcharge Amount when Surcharge Date - Due Date is more than 5 days.
                    string role = "";

                    if (int.Parse(Session["UserID"].ToString()) == 1)
                    {
                        role = "SRCHRG";
                    }
                    else
                    {
                        DataTable dtUserAccess1 = (DataTable)Session["UserAccess"];
                        var srchrge = dtUserAccess1.Select($"CodeEncrypt= 'SRCHRG'");


                        //role = (!string.IsNullOrWhiteSpace(Session["UserAccess"].ToString()) ? "" : "");
                        role = (srchrge.Any() ? "SRCHRG" : "");
                    }

                    txtPenalty.ReadOnly = Convert.ToDateTime(row.Cells[3].Text).Date.AddDays(5) < Convert.ToDateTime(tSurChargeDate.Text).Date && role != "SRCHRG" ? true : false;

                    if (row.Cells[2].Text == "LB")
                        loanable += double.Parse(row.Cells[5].Text);
                    else if (row.Cells[2].Text == "DP")
                        dpamount += double.Parse(row.Cells[5].Text);
                }
            }

            #endregion


















            //######################################
            //##### COMPUTE PAYMENT AND BALANCE ####
            //###################################### 

            foreach (GridViewRow row in gvDownPayments.Rows)
            {

                //** total amount + penalty **//
                postedamt = double.Parse(SystemClass.ToCurrency(row.Cells[12].Text));
                //CheckBox chk = row.FindControl("chk") as CheckBox;
                //if (chk.Checked)
                total_penalties += double.Parse(row.Cells[7].Text);
            }

            //** compute due payments **//

            foreach (GridViewRow row in gvDownPayments.Rows)
            {
                if (Convert.ToDateTime(row.Cells[3].Text) <= DateTime.Now)
                    duepayments += double.Parse(row.Cells[4].Text) + double.Parse(row.Cells[7].Text);
            }

            txtTotalPenalty.Text = SystemClass.ToCurrency(total_penalties.ToString());
            //}

            foreach (GridViewRow row in gvDownPayments.Rows)
            {
                if (row.Cells[2].Text == "MISC")
                    row.Visible = false;
            }
        }


        //void GetCashDiscount()
        //{
        //    foreach (GridViewRow row in gvDownPayments.Rows)
        //    {
        //        TextBox txtCashDiscount = (TextBox)row.FindControl("txtCashDiscount");
        //        TextBox txtCashDiscountPercent = (TextBox)row.FindControl("txtCashDiscountPercent");
        //        TextBox txtCashDiscountValidDate = (TextBox)row.FindControl("txtCashDiscountValidDate");


        //    }
        //}





        protected void gvBuyers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Sel"))
            {
                int row = int.Parse(e.CommandArgument.ToString());

                lblID.Text = gvBuyers.Rows[row].Cells[0].Text;
                lblName.Text = gvBuyers.Rows[row].Cells[4].Text;
                lblFName.Text = gvBuyers.Rows[row].Cells[1].Text;
                lblLName.Text = gvBuyers.Rows[row].Cells[2].Text;
                lblMName.Text = gvBuyers.Rows[row].Cells[3].Text;

                LoadHouseList();

                ComputeTotal();

                pnlBuyerInfo.Visible = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "close", "closeBuyer();", true);
            }
        }
        void LoadHouseList()
        {
            gvHouseLot.DataSource = ws.GetBuyersProject(lblID.Text);
            gvHouseLot.DataBind();

            gvDownPayments.Visible = false;

            gvDownPayments.DataSource = null;
            gvDownPayments.DataBind();

            //check if house/lot == 1 row then load due payments
            if (gvHouseLot.Rows.Count != 0)
            {
                Label lblDocEntry = (Label)gvHouseLot.Rows[0].FindControl("lblDocEntry");
                Label lblProject = (Label)gvHouseLot.Rows[0].FindControl("lblProject");
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
                Label lblDocNum = (Label)gvHouseLot.Rows[0].FindControl("lblDocNum");


                //load data
                tProj.Text = lblProject.Text;
                tBlock.Text = lblBlock1.Text;
                tLot.Text = lblLot1.Text;
                tModel.Text = lblModel1.Text;
                //tPhase.Text = Convert.ToDecimal(lblPhase.Text);
                //tPhase.Text = string.Format("{0:#,#.##}", Convert.ToDecimal(lblPhase.Text));
                tPhase.Text = lblPhase.Text;
                tFinScheme.Text = lblFinscheme.Text;
                lblQuotationNum.Text = lblDocNum.Text;
                tTcp.Text = SystemClass.ToCurrency(gvHouseLot.Rows[0].Cells[0].Text);
                Session["FinCode"] = lblFinschemeCode.Text;
                Session["Status"] = lblStatus.Text;
                Session["SQDocEntry"] = int.Parse(lblDocEntry.Text);
                Session["DPEntry"] = int.Parse(gvHouseLot.Rows[0].Cells[1].Text);
                txtDocumentDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                //if (lblStatus.Text == ConfigSettings.Forwarded"].ToString())
                //{
                lblQuotationNum.Text = lblDocNum.Text;
                //** highlight row **//
                gvHouseLot.Rows[0].BackColor = System.Drawing.ColorTranslator.FromHtml("#A1DCF2");
                //** GET QUOTATION DATA FROM SQL[OQUT]
                DataTable dtQuotation = ws.GetQuotationData(int.Parse(lblDocEntry.Text)).Tables[0];
                if (DataAccess.Exist(dtQuotation))
                {
                    //ReloadData(dtQuotation);
                }
                gvDownPayments.Visible = true;
                gvDownPayments.DataSource = ws.GetDownPayments(int.Parse(lblDocEntry.Text), tFinScheme.Text);
                gvDownPayments.DataBind();
                //}
            }
        }

        private object FormatDate(DateTime input)
        {
            return String.Format("{0:MM/dd/yy}", input);
        }


        protected void gvBuyers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBuyers.DataSource = (DataSet)Session["Buyers"];
            gvBuyers.PageIndex = e.NewPageIndex;
            gvBuyers.DataBind();
        }

        protected void gvDownPayments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Checking the RowType of the Row  
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //int cancelPenalty = int.Parse(DataBinder.Eval(e.Row.DataItem, "CancelPenalty").ToString());
                //CheckBox chk = e.Row.FindControl("chk") as CheckBox;

                //if (cancelPenalty == 0)
                //{
                //    chk.Checked = true;
                //}
                //else
                //{
                //    chk.Checked = false;
                //}
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            confirmation("Are you sure you want to update?", "update");
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "closeConfirmation", "closeConfirmation();", true);
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
            closeconfirm();

            try
            {


                //update trans
                if ((string)Session["ConfirmType"] == "update")
                {
                    foreach (GridViewRow row in gvDownPayments.Rows)
                    {
                        int test = gvDownPayments.Rows.Count;
                        //CheckBox chk = row.FindControl("chk") as CheckBox;
                        TextBox txtPenalty = (TextBox)gvDownPayments.Rows[row.RowIndex].FindControl("txtPenalty");
                        TextBox txtCashDiscount = (TextBox)gvDownPayments.Rows[row.RowIndex].FindControl("txtCashDiscount");
                        TextBox txtCashDiscountPercent = (TextBox)gvDownPayments.Rows[row.RowIndex].FindControl("txtCashDiscountPercent");
                        TextBox txtCashDiscountValidDate = (TextBox)gvDownPayments.Rows[row.RowIndex].FindControl("txtCashDiscountValidDate");



                        string discountDate = string.IsNullOrWhiteSpace(txtCashDiscountValidDate.Text) ? "null" : "'" + txtCashDiscountValidDate.Text + "'";
                        double cashDiscount = Convert.ToDouble(string.IsNullOrWhiteSpace(txtCashDiscount.Text) ? "null" : txtCashDiscount.Text);
                        double CashDiscountPercent = Convert.ToDouble(string.IsNullOrWhiteSpace(txtCashDiscountPercent.Text) ? "null" : txtCashDiscountPercent.Text);


                        int DPEntry = (int)Session["SQDocEntry"];
                        int Terms = int.Parse(row.Cells[1].Text);
                        string Type = row.Cells[2].Text;
                        //double penalty = double.Parse(row.Cells[7].Text) + (txtPenalty.Text == "" || txtPenalty.Text == null ? 0 : double.Parse(txtPenalty.Text));


                        double penalty = Convert.ToDouble(txtPenalty.Text == "" ? "0" : txtPenalty.Text);
                        //if (chk.Checked)
                        //{
                        //if (string.IsNullOrEmpty(txtPenalty.Text))
                        //{
                        //txtPenalty.Text = "0";
                        //hana.Execute($@"UPDATE ""QUT1"" SET ""Penalty""='{double.Parse(txtPenalty.Text)}' WHERE ""DocEntry"" = '{DPEntry}' and ""Terms"" = '{Terms}' and ""PaymentType"" = '{Type}' and A.""LineStatus"" = 'O'", hana.GetConnection("SAOHana"));
                        //}

                        //}
                        //else
                        //{
                        //else
                        //{


                        //2023-06-29 : GET GENERATED PENALTY FROM GRIDVIEW
                        double PenaltyGeneratedAmount = Convert.ToDouble(row.Cells[7].Text);

                        string miscTag = "1";


                        //2023-06-29 : CHANGED CONDITION FOR UPDATING SURCHARGE
                        //if (penalty <= 0)
                        //{
                        //    miscTag = "0";
                        //}
                        if (Math.Round(penalty, 2) != Math.Round(PenaltyGeneratedAmount, 2))
                        {
                            miscTag = "1";
                        }
                        else
                        {
                            miscTag = "0";
                            penalty = 0;
                        }


                        hana.Execute($@"UPDATE 
                                            ""QUT1"" 
                                        SET 
                                            ""Penalty"" = {penalty}, 
                                            ""Misc"" = {miscTag}, 
                                            ""CashDiscount"" = {cashDiscount}, 
                                            ""CashDiscountValidDate"" = {discountDate},
                                            ""CashDiscountPercent"" = {CashDiscountPercent}
                                        WHERE 
                                            ""DocEntry"" = '{DPEntry}' and 
                                            ""Terms"" = '{Terms}' and 
                                            ""PaymentType"" = '{Type}' and 
                                            ""LineStatus"" = 'O'", hana.GetConnection("SAOHana"));
                        //}
                        //hana.Execute($@"UPDATE ""DPI1"" SET ""CancelPenalty"" = '1',""Penalty""=0 WHERE ""DocEntry"" = '{DPEntry}' and ""Terms"" = '{Terms}' and ""PaymentType"" = '{Type}' and IFNULL(""Canceled"",0) = 0", hana.GetConnection("SAOHana"));
                        //}
                    }

                    //** rebind gv **//
                    gvDownPayments.DataSource = ws.GetDownPayments(int.Parse(Session["SQDocEntry"].ToString()), tFinScheme.Text);
                    gvDownPayments.DataBind();

                    ComputeTotal();

                    alertMsg("Operation completed successfully", "success");
                }
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "warning");
            }
        }

        //COMPUTE SURCHARGE WHEN SURCHARGE DATE IS CHANGED
        protected void tSurChargeDate_TextChanged(object sender, EventArgs e)
        {
            ComputeTotal();
        }

        protected void txtCashDiscountPercent_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);

            TextBox txtCashDiscount = (TextBox)row.FindControl("txtCashDiscount");
            TextBox txtCashDiscountPercent = (TextBox)row.FindControl("txtCashDiscountPercent");

            try
            {

                double amount = Convert.ToDouble(row.Cells[4].Text);

                if (!string.IsNullOrWhiteSpace(txtCashDiscountPercent.Text))
                {
                    txtCashDiscount.Text = SystemClass.ToCurrency(Convert.ToString(amount * (Convert.ToDouble(txtCashDiscountPercent.Text) / 100)));
                }
            }
            catch (Exception ex)
            {
                //txtCashDiscountPercent.Text = "";
                alertMsg(ex.Message, "warning");
            }

        }

        protected void txtCashDiscount_TextChanged(object sender, EventArgs e)
        {

            GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);


            TextBox txtCashDiscount = (TextBox)row.FindControl("txtCashDiscount");
            TextBox txtCashDiscountPercent = (TextBox)row.FindControl("txtCashDiscountPercent");
            try
            {


                double amount = Convert.ToDouble(row.Cells[4].Text);

                if (!string.IsNullOrWhiteSpace(txtCashDiscount.Text))
                {
                    txtCashDiscountPercent.Text = SystemClass.ToCurrency(Convert.ToString((Convert.ToDouble(txtCashDiscount.Text) / amount) * 100));
                }
            }
            catch (Exception ex)
            {
                //txtCashDiscount.Text = "";
                alertMsg(ex.Message, "warning");
            }

        }

        protected void btnSurchargeDate1_Click(object sender, EventArgs e)
        {
            ComputeTotal();
        }

        //protected void btnBLedger_Click(object sender, EventArgs e)
        //{
        //    DataTable dt = hana.GetData(@"SELECT ""Name"",""RptName"",""RptPath"" FROM ""ORPT"" Where ""RptGroup"" = 'BL'", hana.GetConnection("SAOHana"));
        //    if (dt.Rows.Count > 0)
        //    {
        //        Session["ReportType"] = "BL";
        //        Session["BPCode"] = lblID.Text;
        //        Session["BlockLot"] = $"B{tBlock.Text} L{tLot.Text}";
        //        Session["ReportName"] = dt.Rows[0][1].ToString();
        //        Session["ReportPath"] = dt.Rows[0][2].ToString();

        //        //open new tab
        //        Response.Redirect("~/pages/ReportViewer.aspx");
        //    }
        //}
    }
}
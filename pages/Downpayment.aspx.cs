using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABROWN_DREAMS
{
    public partial class Downpayment : Page
    {
        DirecWebService ws = new DirecWebService();
        DirecService wcf = new DirecService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["dtDPDownpayment"] = null;
                LoadData(gvDownpayment, "dtDPDownpayment");
                DateTime oDate = DateTime.Now;
                txtDocDate.Value = oDate.ToString("yyyy-MM-dd");
                LoadBanks();
            }
        }

        void LoadData(GridView gv, string session)
        {
            gv.DataSource = (DataTable)Session[session];
            gv.DataBind();
        }

        protected void gvDownpayment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDownpayment.PageIndex = e.NewPageIndex;
            LoadData(gvDownpayment, "dtDownpayment");
        }

        protected void btnFind_ServerClick(object sender, EventArgs e)
        {
            DPList();
            ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "showDPList();", true);
        }

        void DPList()
        {
            if (gvDPList.Rows.Count <= 0)
            {
                Session["dtDPList"] = DataAccess.Select("Addon", "sp_GetDowpaymentList");
                LoadData(gvDPList, "dtDPList");
            }
        }

        protected void gvDPList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDownpayment.PageIndex = e.NewPageIndex;
            LoadData(gvDownpayment, "dtDPList");
        }

        protected void bSelectDP_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string DocEntry = GetID.CommandArgument;
            Session["DocEntry"] = DocEntry;
            Session["GetDowpaymentByDocEntry"] = DataAccess.Select("Addon", $"sp_GetDowpaymentByDocEntry {DocEntry}");
            LoadData(gvDownpayment, "GetDowpaymentByDocEntry");
            DataTable dt = new DataTable();
            dt = (DataTable)Session["GetDowpaymentByDocEntry"];

            txtCardCode.Value = (string)DataAccess.GetData(dt, 0, "CardCode", "");
            txtCardName.Value = (string)DataAccess.GetData(dt, 0, "Name", "");
            txtDocNum.Value = (string)DataAccess.GetData(dt, 0, "DocNum", "");
            ScriptManager.RegisterStartupScript(this, GetType(), "close", "hideDPList();", true);
        }

        //protected void SelectAll_CheckedChanged(object sender, EventArgs e)
        //{
        //    CheckBox ChkBoxHeader = (CheckBox)gvDownpayment.HeaderRow.FindControl("SelectAll");
        //    foreach (GridViewRow row in gvDownpayment.Rows)
        //    {
        //        CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkRow");
        //        if (ChkBoxHeader.Checked == true)
        //        { ChkBoxRows.Checked = true; }
        //        else
        //        { ChkBoxRows.Checked = false; }
        //    }
        //}

        protected void bTab_ServerClick(object sender, EventArgs e)
        {
            bTabCash.Attributes.Add("class", "btn btn-default");
            bTabCheck.Attributes.Add("class", "btn btn-default");

            TabCash.Attributes.Add("class", "tab-pane");
            TabCheck.Attributes.Add("class", "tab-pane");
            Control GetID = (Control)sender;
            switch (GetID.ID)
            {
                case "bTabCash":
                    bTabCash.Attributes.Add("class", "btn btn-success");
                    TabCash.Attributes.Add("class", "tab-pane active");
                    break;
                case "bTabCheck":
                    bTabCheck.Attributes.Add("class", "btn btn-success");
                    TabCheck.Attributes.Add("class", "tab-pane active");
                    break;
            }
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

        void confirmation(string body, string type)
        {
            Session["ConfirmType"] = type;
            lblConfirmationInfo.Text = body;
            ScriptManager.RegisterStartupScript(this, GetType(), "confirmation", "showConfirmation();", true);
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

        void LoadBanks()
        {
            gvBanks.DataSource = ws.GetBanks();
            gvBanks.DataBind();
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


        void LoadBranch(string BankCode)
        {
            gvBranch.DataSource = ws.GetBranch(BankCode);
            gvBranch.DataBind();
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
        { txtTotalAmount.Text = SystemClass.ToCurrency(GetTotal().ToString()); }

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

        protected void btnPaymentMeans_ServerClick(object sender, EventArgs e)
        {
            if (gvDownpayment.Rows.Count > 0)
            { ScriptManager.RegisterStartupScript(this, GetType(), "close", "showPaymentMeans();", true); }
            else { alertMsg("Please select transaction first.", "warning"); }
        }

        protected void bPaymntmeans_ServerClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTotalAmount.Text))
            { txtTotalAmount.Text = "0"; }
            double oPayment = double.Parse(txtTotalAmount.Text);
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[7] { new DataColumn("Terms"), new DataColumn("DueDate"), new DataColumn("MonthlyAmort"),
                                                    new DataColumn("Principal"), new DataColumn("Interest"), new DataColumn("ActualPay"), new DataColumn("Balance") });

            foreach (GridViewRow row in gvDownpayment.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int Terms = 0;
                    string DueDate;
                    double MonthlyAmort = 0;
                    string Principal = "";
                    string Interest = "";
                    double ActualPay = 0;
                    string Balance = "";

                    Terms = int.Parse(gvDownpayment.Rows[row.RowIndex].Cells[0].Text);
                    DueDate = gvDownpayment.Rows[row.RowIndex].Cells[1].Text;
                    MonthlyAmort = double.Parse(gvDownpayment.Rows[row.RowIndex].Cells[2].Text);
                    Principal = gvDownpayment.Rows[row.RowIndex].Cells[3].Text;
                    Interest = gvDownpayment.Rows[row.RowIndex].Cells[4].Text;
                    Balance = gvDownpayment.Rows[row.RowIndex].Cells[6].Text;
                    if (ActualPay == 0 && ActualPay < oPayment)
                    {
                        if (oPayment > MonthlyAmort)
                        {
                            ActualPay = MonthlyAmort;
                            oPayment = oPayment - MonthlyAmort;
                        }
                        else if (oPayment <= MonthlyAmort)
                        {
                            ActualPay = oPayment;
                            oPayment = 0;
                        }
                    }
                    else
                    { ActualPay = 0; }
                    ActualPay = Math.Round(ActualPay, 2);
                    dt.Rows.Add(Terms, DueDate, SystemClass.ToCurrency(MonthlyAmort.ToString()), Principal, Interest, SystemClass.ToCurrency(ActualPay.ToString()), Balance);
                }
            }
            gvDownpayment.DataSource = dt;
            gvDownpayment.DataBind();

            ScriptManager.RegisterStartupScript(this, GetType(), "close", "hideMsgPaymentmeans();", true);
        }

        protected void btnSave_ServerClick(object sender, EventArgs e)
        { ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "showMsgBox();", true); }

        protected void btnYes_Click(object sender, EventArgs e)
        {
            try
            {
                double ActualPay = 0;
                string Terms = "";
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[2] { new DataColumn("Terms"), new DataColumn("ActualPay") });

                foreach (GridViewRow row in gvDownpayment.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        if (Terms == "")
                        { Terms = $"({gvDownpayment.Rows[row.RowIndex].Cells[0].Text}"; }
                        else { Terms = $"{Terms},{gvDownpayment.Rows[row.RowIndex].Cells[0].Text}"; }
                        dt.Rows.Add(Terms, SystemClass.ToCurrency(gvDownpayment.Rows[row.RowIndex].Cells[5].Text));
                        ActualPay = ActualPay + double.Parse(gvDownpayment.Rows[row.RowIndex].Cells[5].Text);

                    }
                }

                Terms = $"{Terms})";
                string DocEntry = (string)Session["DocEntry"];
                string errMsg = "";
                int oDocEntry = 0;
                if (ActualPay > 0)
                {
                    int SQDonEntry = ws.GetSQDocEntry(int.Parse(DocEntry));
                    if (SQDonEntry != 0)
                    {
                        int SODocEntry = ws.GetDocEntryDPRef(SQDonEntry);
                        if (SODocEntry != 0)
                        {
                            if (wcf.CreateDPTerms(SODocEntry, DocEntry, ActualPay, out errMsg, out oDocEntry) == true)
                            { alertMsg(ws.SetDownpaymentActualPay(DocEntry, dt, oDocEntry), "success"); }
                            else { alertMsg(errMsg, "error"); }
                        }
                    }
                }
            }
            catch (Exception ex)
            { alertMsg(ex.Message, "error"); }
            ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "hideMsgBox();", true);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABROWN_DREAMS
{
    public partial class Cleanup : System.Web.UI.Page
    {
        DirecWebService ws = new DirecWebService();
        SAPHanaAccess hana = new SAPHanaAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //pnlQuotation.Visible = true;
                //pnlReservation.Visible = false;
                //pnlBuyer.Visible = false;
            }

            if (Session["UserID"] == null)
            {
                alertMsg("Session expired!", "error");
                Response.Redirect("~/pages/Login.aspx");
            }
        }

        protected void bTab_ServerClick(object sender, EventArgs e)
        {
            bQuotation.Attributes.Add("class", "btn btn-default");
            bBuyers.Attributes.Add("class", "btn btn-default");

            tabQuotation.Attributes.Add("class", "tab-pane fade in");
            tabReservation.Attributes.Add("class", "tab-pane fade in");
            tabBuyers.Attributes.Add("class", "tab-pane fade in");

            Control GetID = (Control)sender;
            switch (GetID.ID)
            {
                case "bQuotation":
                    bQuotation.Attributes.Add("class", "btn btn-success");
                    tabQuotation.Attributes.Add("class", "tab-pane fade in active");
                    break;
                case "bReservation":
                    tabReservation.Attributes.Add("class", "tab-pane fade in active");
                    break;
                case "bBuyers":
                    bBuyers.Attributes.Add("class", "btn btn-success");
                    tabBuyers.Attributes.Add("class", "tab-pane fade in active");
                    break;
            }
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                loadQuotation();
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "warning");
            }
        }

        void loadQuotation()
        {
            string dateFrom = string.IsNullOrWhiteSpace(txtDateFrom.Text) ? "2021-01-01" : txtDateFrom.Text;
            string dateTo = string.IsNullOrWhiteSpace(txtDateTo.Text) ? "2100-12-12" : txtDateTo.Text;

            gvQuotation.DataSource = ws.CleanupQuotationReservation(dateFrom, dateTo, "Q");
            gvQuotation.DataBind();
        }

        void loadBuyers()
        {
            gvBuyers.DataSource = ws.CleanBuyer();
            gvBuyers.DataBind();
        }

        protected void btnGenerate2_Click(object sender, EventArgs e)
        {
            gvReservation.DataSource = ws.CleanupQuotationReservation(txtDateFrom2.Text, txtDateTo2.Text, "R");
            gvReservation.DataBind();
        }

        protected void btnGenerate3_Click(object sender, EventArgs e)
        {

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

        protected void btnClean_Click(object sender, EventArgs e)
        {
            confirmation("Are you sure you want to archive the selected transactions?", "removequo");
        }

        protected void btnYes_Click(object sender, EventArgs e)
        {
            try
            {
                closeconfirm();
                string Message = string.Empty;

                if ((string)Session["ConfirmType"] == "removequo")
                {
                    foreach (GridViewRow row in gvQuotation.Rows)
                    {
                        CheckBox chkSel = (CheckBox)gvQuotation.Rows[row.RowIndex].FindControl("chkSel");

                        if (chkSel.Checked)
                        {
                            ws.Execute($@"UPDATE OQUT SET ""DocStatus"" = 'A' WHERE ""DocEntry"" = '{row.Cells[1].Text}'", hana.GetConnection("SAOHana"));
                        }
                    }

                    loadQuotation();
                    alertMsg("Selected documents are now archived.", "success");
                }


                else if ((string)Session["ConfirmType"] == "removebuyer")
                {
                    foreach (GridViewRow row in gvBuyers.Rows)
                    {
                        ws.Execute($@"UPDATE OCRD SET ""IsArchive"" = 'true' WHERE ""CardCode"" = '{row.Cells[0].Text}'", "Addon");
                    }


                    loadBuyers();
                    alertMsg("Selected buyers are now archived.", "success");
                }

                else if ((string)Session["ConfirmType"] == "removeres")
                {
                    foreach (GridViewRow row in gvReservation.Rows)
                    {
                        ws.Execute($"DELETE FROM OQUT Where DocEntry = '{row.Cells[1].Text}'", "Addon");
                    }
                    gvReservation.DataSource = ws.CleanupQuotationReservation(txtDateFrom2.Text, txtDateTo2.Text, "R");
                    gvReservation.DataBind();
                }

            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "warning");
            }
        }

        protected void btnCleanup3_Click(object sender, EventArgs e)
        {
            confirmation("Are you sure you want to archive the selected transactions?", "removebuyer");
        }

        protected void btnClean2_Click(object sender, EventArgs e)
        {
            confirmation("Are you sure you want to archive the selected buyers?", "removeres");
        }

        protected void chkSel_CheckedChanged(object sender, EventArgs e)
        {

        }

    }
}
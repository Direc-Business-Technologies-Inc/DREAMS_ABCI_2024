using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABROWN_DREAMS
{
    public partial class DocumentRequirements : Page
    {
        DirecWebService ws = new DirecWebService();
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["ReportType"] = "";
            if (!IsPostBack)
            {
                LoadRoles();
                LoadDocuments();
                pnlDoclist.Enabled = false;
                btnSave.Enabled = false;
                btnCancel.Enabled = false;
            }
        }

        void LoadRoles()
        {
            gvRoles.DataSource = ws.GetUserRoles();
            gvRoles.DataBind();
        }
        void LoadDocuments()
        {
            gvDocs.DataSource = ws.GetRequieredDocuments();
            gvDocs.DataBind();
            //gvDocs.HeaderRow.TableSection = TableRowSection.TableHeader;
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


        protected void gvRoles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int row = int.Parse(e.CommandArgument.ToString());
            if (e.CommandName.Equals("Sel"))
            {
                pnlDoclist.Enabled = true;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;

                lblTitle.Text = gvRoles.Rows[row].Cells[1].Text;
                Session["DocCode"] = gvRoles.Rows[row].Cells[0].Text;

                //reload gridview
                gvDocs.DataSource = ws.GetDocPerCode((string)Session["DocCode"]);
                gvDocs.DataBind();
                DocCheck();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblTitle.Text))
            {
                try
                {
                    foreach (GridViewRow row in gvDocs.Rows)
                    {
                        CheckBox chk = (row.Cells[0].FindControl("chk") as CheckBox);
                        string DocId = row.Cells[1].Text;
                        if (chk.Checked == true)
                        {
                            //update doc code
                            ws.UpdateDocumentCode(DocId, (string)Session["DocCode"]);
                        }
                        else
                        {
                            ws.UpdateDocumentCode(DocId, null);
                        }
                    }

                    //reload gridview
                    //reload gridview
                    gvDocs.DataSource = ws.GetDocPerCode((string)Session["DocCode"]);
                    gvDocs.DataBind();
                    DocCheck();

                    alertMsg("Operation completed successfully", "success");
                }
                catch (Exception ex)
                {
                    alertMsg(ex.Message, "error");
                }
            }
        }

        void DocCheck()
        {
            //**unchecked all * *//
            foreach (GridViewRow row1 in gvDocs.Rows)
            {
                CheckBox chk = (row1.Cells[0].FindControl("chk") as CheckBox);
                chk.Checked = false;
            }
            foreach (GridViewRow row1 in gvDocs.Rows)
            {
                string code = string.Format("{0}", row1.Cells[3].Text);
                CheckBox chk = (row1.Cells[0].FindControl("chk") as CheckBox);

                if (code == (string)Session["DocCode"])
                {
                    chk.Checked = true;
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            confirmation("Are you sure you want to cancel?Unsaved data will be lost.", "cancel");
        }

        protected void btnYes_Click(object sender, EventArgs e)
        {
            //cancel trans
            if ((string)Session["ConfirmType"] == "cancel")
            {
                //clear data
                Response.Redirect("~/pages/DocumentRequirements.aspx");
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
    }
}
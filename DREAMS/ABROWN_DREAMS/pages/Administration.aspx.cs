using System;
using System.Configuration;
using System.Data;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABROWN_DREAMS
{
    public partial class Administration : System.Web.UI.Page
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

            if (!IsPostBack)
            {
                pnlTerms.Visible = false;
                //DocumentStatus();
                ValidValues();
                Terms();
                DocumentsList();
                BrokerDocumentsList();
                LoadRole();
                loadBrokerRenewalDate();
                Session["RoleId"] = 0;
                btnSave.Visible = false;
            }
        }
        void DocumentStatus()
        {
            gvDocumentStatus.DataSource = ws.GetDocumentStatus();
            gvDocumentStatus.DataBind();
        }

        void loadBrokerRenewalDate()
        {
            //TO
            DataTable dt = hana.GetData($@"SELECT ""Name"" FROM ""OLST"" Where ""Code"" = 'BRK'", hana.GetConnection("SAOHana"));
            txtRenewalDate.Text = Convert.ToDateTime(DataAccess.GetData(dt, 0, "Name", "")).ToString("yyyy-MM-dd");
            //FROM
            DataTable dt1 = hana.GetData($@"SELECT ""Name"" FROM ""OLST"" Where ""Code"" = 'BRK2'", hana.GetConnection("SAOHana"));
            txtRenewalDateFrom.Text = Convert.ToDateTime(DataAccess.GetData(dt1, 0, "Name", null)).ToString("yyyy-MM-dd");
            //GRACE PERIOD
            DataTable dt3 = hana.GetData($@"SELECT ""Name"" FROM ""OLST"" Where ""Code"" = 'BRK3'", hana.GetConnection("SAOHana"));
            txtGracePeriod.Text = DataAccess.GetData(dt3, 0, "Name", "0").ToString();
            //GRACE PERIOD
            DataTable dt4 = hana.GetData($@"SELECT ""Name"" FROM ""OLST"" Where ""Code"" = 'BRK4'", hana.GetConnection("SAOHana"));
            txtMinimumDay.Text = DataAccess.GetData(dt4, 0, "Name", "0").ToString();

        }

        void ValidValues()
        {
            gvValidValues.DataSource = ws.GetValidValues();
            gvValidValues.DataBind();
        }
        void LoadRole()
        {
            gvRoles.DataSource = ws.GetRoles();
            gvRoles.DataBind();
        }

        void Terms()
        {
            DataTable dt = new DataTable();
            dt = ws.GetFinancingScheme().Tables[0];
            gvFinancingScheme.DataSource = dt;
            gvFinancingScheme.DataBind();

        }
        void DocumentsList()
        {
            gvDocuments.DataSource = ws.GetDocumentsList();
            gvDocuments.DataBind();
        }
        void BrokerDocumentsList()
        {
            gvBrokerDocuments.DataSource = ws.GetBrokerDocuments();
            gvBrokerDocuments.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool success = false;
            foreach (GridViewRow row in gvDocumentStatus.Rows)
            {
                TextBox tb = (TextBox)row.FindControl("txtColor");
                string code = row.Cells[0].Text;
                DataTable dt = hana.GetData($@"SELECT ""Code"" FROM ""ODCS"" Where ""Code"" = '{code}'", hana.GetConnection("SAOHana"));
                if (!DataAccess.Exist(dt))
                {
                    if (ws.AddColor(code, tb.Text))
                    {
                        success = true;
                    }
                    else { success = false; break; }
                }
                else
                {
                    if (ws.UpdateColor(code, tb.Text))
                    {
                        success = true;
                    }
                    else { success = false; break; }
                }
            }
            if (success)
            {
                alertMsg("Color updated successfully", "success");
            }
            else
            {
                alertMsg("Error in updating color", "error");
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

        protected void gvValidValues_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            txtCode.Text = "";
            txtName.Text = "";
            txtGroup.Text = "";
            if (e.CommandName == "Edt")
            {
                int rowIndex = int.Parse(e.CommandArgument.ToString());
                txtCode.Text = gvValidValues.Rows[rowIndex].Cells[0].Text;
                txtName.Text = gvValidValues.Rows[rowIndex].Cells[1].Text;
                txtGroup.Text = gvValidValues.Rows[rowIndex].Cells[2].Text;
                //show modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showValidValues();", true);
            }
        }
        protected void btnEdit_ServerClick(object sender, EventArgs e)
        {
            if (ws.UpdateValidValues(txtCode.Text, txtName.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeValidValues();", true);
                //success
                alertMsg("Name updated successfully", "success");
            }
            else
            {
                //failed
                alertMsg("Error in updating name", "error");
            }
            //reload list
            ValidValues();
        }
        protected void gvValidValues_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ValidValues();
            gvValidValues.PageIndex = e.NewPageIndex;
            gvValidValues.DataBind();
        }

        protected void btnAddTerms_ServerClick(object sender, EventArgs e)
        {
            if (ws.AddTerms(txtTerms.Text, (string)Session["FinancingScheme"]))
            {
                gvTerms.DataSource = ws.GetTerms((string)Session["FinancingScheme"]);
                gvTerms.DataBind();
                //pnlTerms.Visible = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeTerms();", true);
                //success
                alertMsg("New payment terms added successfully", "success");
            }
            else
            {
                //failed
                alertMsg("Error in adding payment terms", "error");
            }
        }

        protected void gvDocumentStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DocumentStatus();
            gvDocumentStatus.PageIndex = e.NewPageIndex;
            gvDocumentStatus.DataBind();
        }

        protected void btnAddDocuments_ServerClick(object sender, EventArgs e)
        {
            if (txtDocuments.Text != "")
            {
                if (ws.AddDocumentRequirements(txtDocuments.Text, "1", ddlBusinessType.SelectedValue))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "closeDoc();", true);
                    //success
                    alertMsg("New document added successfully", "success");
                }
                else
                {
                    //failed
                    alertMsg("Error in adding document", "error");
                }
                //reload
                DocumentsList();
            }
            else
            {
                alertMsg("Error in adding document: Documents field is blank", "error");
            }
        }

        //protected void gvTerms_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    Terms();
        //    gvTerms.PageIndex = e.NewPageIndex;
        //    gvTerms.DataBind();
        //}

        protected void bSelectBuyer_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string Code = GetID.CommandArgument;
            Session["FinancingScheme"] = Code;


            DataTable dt = hana.GetData($@"SELECT ""Name"" FROM ""OLST"" Where ""GrpCode"" = 'FS' And ""Code"" = '{Code}'", hana.GetConnection("SAOHana"));
            if (dt.Rows.Count > 0)
            {
                pnlTerms.Visible = true;
                lblTermsTitle.Text = dt.Rows[0][0].ToString() + " - Terms";
                gvTerms.DataSource = ws.GetTerms(Code);
                gvTerms.DataBind();
            }
        }

        protected void gvDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int i = int.Parse(e.CommandArgument.ToString());
            //txtDiaryId.Text = gvStatus.Rows[i].Cells[0].Text;
            if (e.CommandName.Equals("Del"))
            {
                Session["DocId"] = gvDocuments.Rows[i].Cells[0].Text;
                confirmation("Are you sure you want to remove selected documents?", "removedoc");
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "closeConfirmation", "closeConfirmation();", true);
        }

        protected void btnYes_Click(object sender, EventArgs e)
        {
            string Message = string.Empty;
            closeconfirm();
            if ((string)Session["ConfirmType"] == "removedoc")
            {
                if (!hana.Execute($@"DELETE FROM ""ODOC"" Where ""DocId"" = '{(string)Session["DocId"]}'", hana.GetConnection("SAOHana")))
                {
                    alertMsg("Error in removing selected documents", "error");
                }
                else
                {
                    DocumentsList();
                }
            }
            else if ((string)Session["ConfirmType"] == "removeBrokerdoc")
            {
                if (!hana.Execute($@"DELETE FROM ""OBRD"" Where ""DocId"" = '{(string)Session["DocBrokerId"]}'", hana.GetConnection("SAOHana")))
                {
                    alertMsg("Error in removing selected broker documents", "error");
                }
                else
                {
                    BrokerDocumentsList();
                }
            }
            else if ((string)Session["ConfirmType"] == "removeterms")
            {
                if (!hana.Execute($@"DELETE FROM ""OLST"" Where ""GrpCode"" = 'FS-{(string)Session["FinancingScheme"]}' AND ""Code"" = '{(string)Session["TermId"]}'", hana.GetConnection("SAOHana")))
                {
                    alertMsg("Error in removing selected documents", "error");
                }
                else
                {
                    gvTerms.DataSource = ws.GetTerms((string)Session["FinancingScheme"]);
                    gvTerms.DataBind();
                }
            }
            else if ((string)Session["ConfirmType"] == "removerole")
            {
                if (!hana.Execute($@"DELETE FROM ""ROLE"" Where ""Id"" = '{(int)Session["RoleId"]}'", hana.GetConnection("SAOHana")))
                {
                    alertMsg("Error in removing selected role", "error");
                }
                else
                {
                    LoadRole();
                }
            }

        }

        protected void gvTerms_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int i = int.Parse(e.CommandArgument.ToString());
            //txtDiaryId.Text = gvStatus.Rows[i].Cells[0].Text;
            if (e.CommandName.Equals("Del"))
            {
                Session["TermId"] = gvTerms.Rows[i].Cells[0].Text;
                confirmation("Are you sure you want to remove selected terms?", "removeterms");
            }
        }

        protected void btnAddRole_Click(object sender, EventArgs e)
        {
            btnAddUserRole.InnerText = "Add";
            txtRoleId.Text = "0";
            txtSequence.Text = string.Empty;
            txtRoleCode.Text = string.Empty;
            txtRoleName.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showRole", "showRole();", true);
        }

        protected void btnAddUserRole_ServerClick(object sender, EventArgs e)
        {
            string sequence = txtSequence.Text == "" ? "0" : txtSequence.Text;
            if (ws.AddRole((int)Session["RoleId"], int.Parse(sequence),
                txtRoleCode.Text,
                txtRoleName.Text))
            {
                LoadRole();
                alertMsg("User role added/updated successfully", "success");
            }
            else
            {
                alertMsg("Error in adding/updating role", "error");
            }
        }

        protected void gvRoles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int i = int.Parse(e.CommandArgument.ToString());
            txtRoleId.Text = gvRoles.Rows[i].Cells[0].Text;
            Session["RoleId"] = int.Parse(gvRoles.Rows[i].Cells[0].Text);
            if (e.CommandName.Equals("Edt"))
            {
                txtSequence.Text = gvRoles.Rows[i].Cells[1].Text;
                txtRoleCode.Text = gvRoles.Rows[i].Cells[2].Text;
                txtRoleName.Text = gvRoles.Rows[i].Cells[3].Text;

                btnAddUserRole.InnerText = "Update";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "showRole", "showRole()", true);
            }
            else
            {
                //** delete status **//
                confirmation("Are you sure you want to remove selected role?", "removerole");
            }
        }

        protected void gvBrokerDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int i = int.Parse(e.CommandArgument.ToString());
            if (e.CommandName.Equals("Del"))
            {
                Session["DocBrokerId"] = gvBrokerDocuments.Rows[i].Cells[0].Text;
                confirmation("Are you sure you want to remove selected documents?", "removeBrokerdoc");
            }
        }

        protected void btnAddBrokerDocuments_ServerClick(object sender, EventArgs e)
        {
            if (ws.AddBrokerDocumentRequirements(txtBrokerDocument.Text, ddlBrokerSection.SelectedItem.Text, "1"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "closeBrokerDoc();", true);
                //success
                alertMsg("New broker document added successfully", "success");
            }
            else
            {
                //failed
                alertMsg("Error in adding broker document", "error");
            }
            //reload
            BrokerDocumentsList();
        }

        protected void btnSaveRenewalDate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtRenewalDateFrom.Text) || string.IsNullOrEmpty(txtRenewalDateFrom.Text))
                {
                    alertMsg("Please don't leave the date as blank!", "warning");
                    return;
                }

                if (int.Parse(Convert.ToDateTime(txtRenewalDateFrom.Text).ToString("yyyyMMdd")) > int.Parse(Convert.ToDateTime(txtRenewalDate.Text).ToString("yyyyMMdd")))
                {
                    alertMsg("Please check the renewal date from and date to.", "warning");
                    return;
                }

                hana.Execute($@" UPDATE OLST SET ""Name"" = '{Convert.ToDateTime(txtRenewalDate.Text).ToString("yyyy-MM-dd")}' WHERE ""Code"" = 'BRK'", hana.GetConnection("SAOHana"));
                hana.Execute($@" UPDATE OLST SET ""Name"" = '{Convert.ToDateTime(txtRenewalDateFrom.Text).ToString("yyyy-MM-dd")}' WHERE ""Code"" = 'BRK2'", hana.GetConnection("SAOHana"));
                hana.Execute($@" UPDATE OLST SET ""Name"" = '{txtGracePeriod.Text}' WHERE ""Code"" = 'BRK3'", hana.GetConnection("SAOHana"));
                hana.Execute($@" UPDATE OLST SET ""Name"" = '{txtMinimumDay.Text}' WHERE ""Code"" = 'BRK4'", hana.GetConnection("SAOHana"));
                alertMsg("Broker Renewal Date successfully updated!", "success");
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }
        }

        protected void txtDocuments_TextChanged(object sender, EventArgs e)
        {
            if (txtDocuments.Text != "")
            {
                btnAddDocuments.Disabled = false;
            }
            else
            {
                btnAddDocuments.Disabled = true;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABROWN_DREAMS.pages
{
    public partial class Broker1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!this.IsPostBack)
            {
                step_1.Visible = true;
                step_2.Visible = false;
                step_3.Visible = false;
                step_4.Visible = false;
                step1.Attributes.Add("class", "btn btn-info btn-circle");
                step2.Attributes.Add("class", "btn btn-default btn-circle");
                step3.Attributes.Add("class", "btn btn-default btn-circle");
                step4.Attributes.Add("class", "btn btn-default btn-circle");

                //btnNext.Attributes.Add("AutoPostback", "true");
                btnAdd.Attributes.Add("AutoPostback", "true");

                btnAdd.Attributes.Add("onclick", "this.disabled = true;" + ClientScript.GetPostBackEventReference(btnAdd, null) + ";");

                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[6]
                {
                        new DataColumn("Id"),
                        new DataColumn("SalesPersonName"),
                        new DataColumn("PRCLicense/AccreditationNo"),
                        new DataColumn("MobileNumber"),
                        new DataColumn("EmailAddress"),
                        new DataColumn("Position"),
                });
                ViewState["SalesPerson"] = dt;
                this.BindGrid();

                DataTable dt2 = new DataTable();
                dt2.Columns.AddRange(new DataColumn[5]
                {
                        new DataColumn("Id"),
                        new DataColumn("Position"),
                        new DataColumn("SalesPersonName"),
                        new DataColumn("Percentage"),
                        new DataColumn("Amount?"),
                });
                ViewState["SharingDetails"] = dt2;
                this.BindGrid2();
            }
        }

        protected void BindGrid()
        {
            gvSalesPerson.DataSource = (DataTable)ViewState["SalesPerson"];
            gvSalesPerson.DataBind();
        }

        protected void BindGrid2()
        {
            gvShareDetails.DataSource = (DataTable)ViewState["SharingDetails"];
            gvShareDetails.DataBind();
        }

        protected void ClearSalesPersonModal()
        {
            mtxtEmail.Text = "";
            mtxtMobile.Text = "";
            mtxtPRCLicense.Text = "";
            mtxtSalesPerson.Text = "";
            mtxtPecent.Text = "";
            mtxtAmount.Text = "";
            mtxtSalesPersonShare.Text = "";
            ddPosition.SelectedIndex = 0;
            mddPositionShare.SelectedIndex = 0;
        }


        protected void btnNext_ServerClick(object sender, EventArgs e)
        {
            step_1.Visible = false;
            step_2.Visible = true;
            step_3.Visible = false;
            step_4.Visible = false;
            step1.Attributes.Add("class", "btn btn-success btn-circle");
            step2.Attributes.Add("class", "btn btn-info btn-circle");
            step3.Attributes.Add("class", "btn btn-default btn-circle");
            step4.Attributes.Add("class", "btn btn-default btn-circle");
        }

        protected void btnNextStep_Click(object sender, EventArgs e)
        {
            step_1.Visible = false;
            step_2.Visible = true;
            step_3.Visible = false;
            step_4.Visible = false;
            step1.Attributes.Add("class", "btn btn-success btn-circle");
            step2.Attributes.Add("class", "btn btn-info btn-circle");
            step3.Attributes.Add("class", "btn btn-default btn-circle");
            step4.Attributes.Add("class", "btn btn-default btn-circle");
        }

        protected void btnNext2_ServerClick(object sender, EventArgs e)
        {
            step_1.Visible = false;
            step_2.Visible = false;
            step_3.Visible = true;
            step_4.Visible = false;
            step1.Attributes.Add("class", "btn btn-success btn-circle");
            step2.Attributes.Add("class", "btn btn-success btn-circle");
            step3.Attributes.Add("class", "btn btn-info btn-circle");
            step4.Attributes.Add("class", "btn btn-default btn-circle");

        }
        protected void btnNext3_ServerClick(object sender, EventArgs e)
        {
            step_1.Visible = false;
            step_2.Visible = false;
            step_3.Visible = false;
            step_4.Visible = true;
            step1.Attributes.Add("class", "btn btn-success btn-circle");
            step2.Attributes.Add("class", "btn btn-success btn-circle");
            step3.Attributes.Add("class", "btn btn-success btn-circle");
            step4.Attributes.Add("class", "btn btn-info btn-circle");

            if (rbTypeOfBusiness.SelectedValue == "Sole Proprietor   ")
            {
                SoleProp.Visible = true;
                Partnership.Visible = false;
                Corporation.Visible = false;
            }
            else if (rbTypeOfBusiness.SelectedValue == "Partnership   ")
            {
                SoleProp.Visible = false;
                Partnership.Visible = true;
                Corporation.Visible = false;
            }
            else
            {
                SoleProp.Visible = false;
                Partnership.Visible = false;
                Corporation.Visible = true;
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

            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "ShowAlert();", true);
        }

        protected void btnPrevious_ServerClick(object sender, EventArgs e)
        {
            step_1.Visible = true;
            step_2.Visible = false;
            step_3.Visible = false;
            step_4.Visible = false;
            step1.Attributes.Add("class", "btn btn-info btn-circle");
            step2.Attributes.Add("class", "btn btn-default btn-circle");
            step3.Attributes.Add("class", "btn btn-default btn-circle");
            step4.Attributes.Add("class", "btn btn-default btn-circle");
        }

        protected void btnPrevious2_ServerClick(object sender, EventArgs e)
        {
            step_1.Visible = false;
            step_2.Visible = true;
            step_3.Visible = false;
            step_4.Visible = false;
            step1.Attributes.Add("class", "btn btn-success btn-circle");
            step2.Attributes.Add("class", "btn btn-info btn-circle");
            step3.Attributes.Add("class", "btn btn-default btn-circle");
            step4.Attributes.Add("class", "btn btn-default btn-circle");
        }

        protected void btnPrevious3_ServerClick(object sender, EventArgs e)
        {

            step_1.Visible = false;
            step_2.Visible = false;
            step_3.Visible = true;
            step_4.Visible = false;
            step1.Attributes.Add("class", "btn btn-success btn-circle");
            step2.Attributes.Add("class", "btn btn-success btn-circle");
            step3.Attributes.Add("class", "btn btn-info btn-circle");
            step4.Attributes.Add("class", "btn btn-default btn-circle");
        }

        protected void btnAddSalesPerson_ServerClick(object sender, EventArgs e)
        {

            DataTable dt = (DataTable)ViewState["SalesPerson"];
            dt.Rows.Add(
                Convert.ToInt32(dt.Rows.Count + 1),
                mtxtSalesPerson.Text.Trim(),
                mtxtPRCLicense.Text.Trim(),
                mtxtMobile.Text.Trim(),
                mtxtEmail.Text.Trim(),
                ddPosition.SelectedItem.Text);
            ViewState["SalesPerson"] = dt;
            this.BindGrid();
        }

        protected void mbtnAddShare_ServerClick(object sender, EventArgs e)
        {

        }

        protected void btnAddShareDetails_ServerClick(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                DataTable dt2 = (DataTable)ViewState["SharingDetails"];
                dt2.Rows.Add(
                     Convert.ToInt32(dt2.Rows.Count + 1),
                     mddPositionShare.SelectedItem.Text,
                     mtxtSalesPersonShare.Text.Trim(),
                     Convert.ToInt32(mtxtPecent.Text.Trim()),
                     mtxtAmount.Text.Trim());

                ViewState["SharingDetails"] = dt2;
                this.BindGrid2();
                ClearSalesPersonModal();

            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dt2 = (DataTable)ViewState["SharingDetails"];
            dt2.Rows.Add(
                 Convert.ToInt32(dt2.Rows.Count + 1),
                 mddPositionShare.SelectedItem.Text,
                 mtxtSalesPersonShare.Text.Trim(),
                 Convert.ToInt32(mtxtPecent.Text.Trim()),
                 mtxtAmount.Text.Trim());

            ViewState["SharingDetails"] = dt2;
            this.BindGrid2();
            ClearSalesPersonModal();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideAddSharingModal();", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowSharingModal();", true);
        }

        protected void btnAdd_ServerClick(object sender, EventArgs e)
        {
            DataTable dt2 = (DataTable)ViewState["SharingDetails"];
            dt2.Rows.Add(
                 Convert.ToInt32(dt2.Rows.Count + 1),
                 mddPositionShare.SelectedItem.Text,
                 mtxtSalesPersonShare.Text.Trim(),
                 Convert.ToInt32(mtxtPecent.Text.Trim()),
                 mtxtAmount.Text.Trim());

            ViewState["SharingDetails"] = dt2;
            this.BindGrid2();
            ClearSalesPersonModal();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideAddSharingModal();", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowSharingModal();", true);
        }

        protected void btnUpdate_ServerClick(object sender, EventArgs e)
        {
            alertMsg("test", "success");
        }

        protected void testbtn_ServerClick(object sender, EventArgs e)
        {

        }

        protected void btnSubmitID_ServerClick(object sender, EventArgs e)
        {
            var ticks = DateTime.Now.Ticks;
            var guid = Guid.NewGuid().ToString();
            var uniqueSessionId = ticks.ToString() + guid + txtUniqueIdSet.Text;

            txtUniqueId.Text = uniqueSessionId;
            ScriptManager.RegisterStartupScript(this, GetType(), "Success", "ShowSuccessAlert();", true);
            mtxtUniqueId.Text = uniqueSessionId;
        }

    

        //protected void btnEdit_ServerClick(object sender, EventArgs e)
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowEditSharing();", true);
        //    foreach (GridViewRow dept in gvShareDetails.Rows)
        //    {
        //        //GridViewRow row = gvShareDetails.Rows[rowIndex];

        //        //String value = row.Cells[2].Text.ToString();
        //        string percent = dept.Cells[2].Text.ToString();
        //    }


        //}
    }
}
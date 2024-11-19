using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABROWN_DREAMS.pages
{
    public partial class Forfeitures : Page
    {
        SAPHanaAccess hana = new SAPHanaAccess();
        DirecWebService ws = new DirecWebService();
        protected void Page_Load(object sender, EventArgs e)
        {
            loaddata();
        }
        void loaddata()
        {
            try
            {
                string database = ConfigurationManager.AppSettings["HANADatabase"].ToString();
                DataTable dt = ws.GetForfeitures(database).Tables["GetForfeitures"];
                gvDocList.DataSource = dt;
                gvDocList.DataBind();
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "info");
            }
        }

        protected void gvDocList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDocList.PageIndex = e.NewPageIndex;
            gvDocList.DataSource = (DataTable)ViewState["Forfeited"];
            gvDocList.DataBind();
        }

        protected void bSearchBuyer_Click(object sender, EventArgs e)
        {
            ViewState["Forfeited"] = ws.GetForfeituresSearch(ConfigurationManager.AppSettings["HANADatabase"].ToString(),
                txtSearchBuyer.Value).Tables[0];
            gvDocList.DataSource = (DataTable)ViewState["Forfeited"];
            gvDocList.DataBind();
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

    }


}
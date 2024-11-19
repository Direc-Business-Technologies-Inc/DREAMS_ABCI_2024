using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABROWN_DREAMS.pages
{
    public partial class ReportList : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string[] filePaths = Directory.GetFiles(ConfigurationManager.AppSettings["ReportPath"].ToString());
                List<ListItem> files = new List<ListItem>();
                foreach (string filePath in filePaths)
                {
                    files.Add(new ListItem(Path.GetFileName(filePath), filePath));
                }
                gvReports.DataSource = files;
                gvReports.DataBind();
            }
        }

        protected void gvReports_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName.Equals("report"))
            {
                Session["ReportName"] = gvReports.Rows[index].Cells[0].Text;
                Session["ReportPath"] = ConfigurationManager.AppSettings["ReportPath"].ToString();
                Session["ReportType"] = "RL";
                Session["RptConn"] = ConfigurationManager.AppSettings["ReportConnection"].ToString();
                //open new tab
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openpage", "window.open('ReportViewer.aspx');", true);
            }
        }


        protected void btnPrint_Click(object sender, EventArgs e)
        {
        }

        protected void gvReports_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "Test", true);

        }

        protected void gvReports_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "Test", true);
        }
    }
}
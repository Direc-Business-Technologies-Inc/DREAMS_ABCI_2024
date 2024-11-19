using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABROWN_DREAMS
{
    public partial class HouseCon : System.Web.UI.Page
    {
        DirecWebService ws = new DirecWebService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                LoadForConstruction();
            }
        }
        void LoadForConstruction()
        {
            gvForConstruction.DataSource = ws.ForHouseConstruction();
            gvForConstruction.DataBind();
        }
       
        protected void btnExport_ServerClick(object sender, EventArgs e)
        {
            Response.ClearContent();
            Response.AppendHeader("content-disposition", "attachment;filename=ForHouseConstruction.xls");
            Response.ContentType = "application/excel";

            StringWriter stringwriter = new StringWriter();
            HtmlTextWriter htmtextwriter = new HtmlTextWriter(stringwriter);

            gvForConstruction.HeaderRow.Style.Add("background-color", "#ffffff");

            foreach (TableCell tableCell in gvForConstruction.HeaderRow.Cells)
            {
                tableCell.Style["background-color"] = "#ffffff";
            }

            foreach (GridViewRow gridviewrow in gvForConstruction.Rows)
            {
                gridviewrow.BackColor = System.Drawing.Color.White;
                foreach (TableCell gridviewrowtablecell in gridviewrow.Cells)
                {
                    gridviewrowtablecell.Style["background-color"] = "#ffffff";
                }
            }
            try
            {
                gvForConstruction.RenderControl(htmtextwriter);
                Response.Write(stringwriter.ToString());
                Response.End();
            }
            catch{
            }
            
           
        }

        protected void gvForConstruction_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
    }
}
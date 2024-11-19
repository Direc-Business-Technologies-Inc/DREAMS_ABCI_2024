using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Services;
using System.Configuration;
using System.Web;

namespace ABROWN_DREAMS
{

    public partial class TESTING : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        [WebMethod]
        public static List<string> TextSearch(string search)
        {
            List<string> searchresult = new List<string>();
            using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT TOP 10 LastName,FirstName FROM CRD1 Where CardType='Buyer' AND (LastName like '%'"+ search + "'%')";
                    //cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    con.Open();
                    //cmd.Parameters.AddWithValue("@Search", search);
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        searchresult.Add(dr["LastName"].ToString());
                    }
                    con.Close();
                    return searchresult;
                }
            }
        }
        protected void crvPage_Init(object sender, EventArgs e)
        {
            LoadReport();
        }

        void LoadReport()
        {
            string SapDatabase = DataAccess.GetconDetails("SAP", "Initial Catalog");
            string DBUsername = DataAccess.GetconDetails("SAP", "User ID");
            string DBPassword = DataAccess.GetconDetails("SAP", "Password");
            string Server = DataAccess.GetconDetails("SAP", "Data Source");

            ReportDocument report = new ReportDocument();
            report.Load(HttpContext.Current.Server.MapPath(@"Reports\OO_PurchOrder.rpt"));
            report.SetParameterValue("DocEntry", "1");
            report.SetDatabaseLogon(DBUsername,DBPassword, Server, SapDatabase);
            report.PrintToPrinter(1, true, 0, 0);
            //crViewer.ReportSource = report;
            //crViewer.DataBind();

        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            //ReportDocument oDocument = new ReportDocument();
            //oDocument.Load(Server.MapPath@"/ReportPath/ReportName.rpt");
            //oDocument.SetDataSource(new DataSet()); // Added report data as dataset.

            //crViewer.ReportSource = oDocument;
            //crViewer.PrintReport();
        }
    }
} 
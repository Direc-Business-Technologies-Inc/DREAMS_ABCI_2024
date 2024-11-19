using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Sap.Data.Hana;
using System;
using System.Configuration;
using System.Web.UI;

namespace ABROWN_DREAMS
{
    public partial class ReportViewer : System.Web.UI.Page
    {
        SAPHanaAccess hana = new SAPHanaAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Page.Title))
                {
                    Page.Title = (string)Session["Title"];
                }

                if (Session["UserID"] == null)
                { Response.Redirect("~/pages/Login.aspx"); }


            }
            catch
            {


            }

        }

        protected void Page_Init(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                LoadReport();
            }
            else
            {
                ReportDocument doc = (ReportDocument)Session["ReportDocument"];
                crystal.ReportSource = doc;
                crystal.DataBind();
            }
        }

        protected void crvPage_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadReport();
            }
            else
            {
                ReportDocument doc = (ReportDocument)Session["ReportDocument"];
                crystal.ReportSource = doc;
                crystal.DataBind();
            }
        }
        void LoadReport()
        {
            try
            {
                //** SAP **
                string SapDatabase = ConfigurationManager.AppSettings["HANADatabase"];
                string DBUsername = ConfigurationManager.AppSettings["HANAUserID"];
                string DBPassword = ConfigurationManager.AppSettings["HANAPassword"];
                string Driver = ConfigurationManager.AppSettings["HANADriver"];
                string Server = ConfigurationManager.AppSettings["HANAServer"];
                //** ADD ON **
                string AODatabase = ConfigurationManager.AppSettings["SAODatabase"];
                string AODBUsername = ConfigurationManager.AppSettings["HANAUserID"];
                string AODBPassword = ConfigurationManager.AppSettings["HANAPassword"];
                string AOServer = ConfigurationManager.AppSettings["SAOServer"];

                string path = $"{(string)Session["ReportPath"]}\\{(string)Session["ReportName"]}";
                string entry = (string)Session["PrintDocEntry"];
                string reportType = (string)Session["reportType"];
                string rptParameter = (string)Session["reportParameter"];
                string Location = Session["Location"] == null ? "" : Session["Location"].ToString();

                //ReportDocument report = new ReportDocument();
                //report.Load(path);

                //if ((string)Session["ReportType"] == "BL")
                //{
                //    report.SetParameterValue(0, (string)Session["BPCode"]);
                //    report.SetParameterValue(1, (string)Session["BlockLot"]);
                //}
                //else if ((string)Session["ReportType"] == "rL")
                //{

                //}
                //else
                //{
                //    report.SetParameterValue("DocKey@", entry);
                //}

                //if (!string.IsNullOrEmpty((string)Session["RptConn"]) && Session["RptConn"].ToString() == "SAP")
                //{
                //    report.SetDatabaseLogon(DBUsername, DBPassword, Server, SapDatabase);
                //}
                //else
                //{ report.SetDatabaseLogon(AODBUsername, AODBPassword, AOServer, AODatabase); }


                ReportDocument cryRpt = new ReportDocument(), crSubreportDocument;
                TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
                TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
                ConnectionInfo crConnectionInfo = new ConnectionInfo();





                cryRpt.Load(path);
                //if (Session["ReportType"].ToString() == "RL")
                //{

                //}
                //else if (Session["ReportType"].ToString() == "Buyer")
                //{
                //    cryRpt.SetParameterValue("DocKey@", entry);
                //    /*  cryRpt.SetParameterValue("BPCode@", rptParameter);*/

                //    //2023-06-05 : Add Project ; additional parameter on form
                //    cryRpt.SetParameterValue("PRJ@", rptParameter);

                //}
                //else if (Session["ReportType"].ToString() == "Receipt")
                //{
                //    cryRpt.SetParameterValue("DocKey@", entry);
                //    cryRpt.SetParameterValue("Name@", Location);
                //}
                //else
                //{
                //    cryRpt.SetParameterValue("DocKey@", entry);
                //}

                ParameterFieldDefinitions crParameterdef = cryRpt.DataDefinition.ParameterFields;

                //#############################################################################
                //Added by Cedi 070119
                var logonProperties = new DbConnectionAttributes();

                string test = Session["RptConn"].ToString();
                if (Session["RptConn"].ToString() == "Addon")
                {
                    logonProperties.Collection.Set("Connection String", $@"DRIVER={Driver};SERVERNODE={Server}; UID={AODBUsername};PWD={AODBPassword};CS={AODatabase};");
                }
                else
                {
                    logonProperties.Collection.Set("Connection String", $@"DRIVER={Driver};SERVERNODE={Server}; UID={AODBUsername};PWD={AODBPassword};CS={SapDatabase};");
                }
                logonProperties.Collection.Set("UseDSNProperties", false);
                var connectionAttributes = new DbConnectionAttributes();
                connectionAttributes.Collection.Set("Database DLL", "crdb_odbc.dll");
                connectionAttributes.Collection.Set("QE_DatabaseName", String.Empty);
                connectionAttributes.Collection.Set("QE_DatabaseType", "ODBC (RDO)");
                connectionAttributes.Collection.Set("QE_LogonProperties", logonProperties);
                connectionAttributes.Collection.Set("QE_ServerDescription", AOServer);
                connectionAttributes.Collection.Set("QE_SQLDB", false);
                connectionAttributes.Collection.Set("SSO Enabled", false);


                //#############################################################################

                crConnectionInfo.ServerName = Server;
                if (Session["RptConn"].ToString() == "Addon")
                {
                    crConnectionInfo.DatabaseName = AODatabase;
                }
                else
                {
                    crConnectionInfo.DatabaseName = SapDatabase;
                }

                crConnectionInfo.UserID = AODBUsername;
                crConnectionInfo.Password = AODBPassword;

                crConnectionInfo.Attributes = connectionAttributes;
                crConnectionInfo.Type = ConnectionInfoType.CRQE;
                crConnectionInfo.IntegratedSecurity = false;
                //crConnectionInfo.Type = ConnectionInfoType.SQL;

                foreach (Table CrTable in cryRpt.Database.Tables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;

                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }



                Sections crSections;
                ReportObjects crReportObjects;
                SubreportObject crSubreportObject;


                Database crDatabase;
                crDatabase = cryRpt.Database;
                Tables crTables;
                crTables = crDatabase.Tables;



                // THIS STUFF HERE IS FOR REPORTS HAVING SUBREPORTS 
                // set the sections object to the current report's section 
                crSections = cryRpt.ReportDefinition.Sections;
                // loop through all the sections to find all the report objects 
                foreach (Section crSection in crSections)
                {
                    crReportObjects = crSection.ReportObjects;

                    //loop through all the report objects in there to find all subreports 
                    foreach (ReportObject crReportObject in crReportObjects)
                    {
                        if (crReportObject.Kind == ReportObjectKind.SubreportObject)
                        {
                            crSubreportObject = (SubreportObject)crReportObject;
                            //open the subreport object and logon as for the general report 
                            crSubreportDocument = crSubreportObject.OpenSubreport(crSubreportObject.SubreportName);
                            crDatabase = crSubreportDocument.Database;
                            crTables = crDatabase.Tables;
                            foreach (Table aTable in crTables)
                            {
                                crtableLogoninfo = aTable.LogOnInfo;
                                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                                aTable.ApplyLogOnInfo(crtableLogoninfo);
                            }
                        }
                    }



                }

                crystal.ReportSource = cryRpt;
                crystal.EnableDatabaseLogonPrompt = false;
                crystal.HasCrystalLogo = true;
                crystal.HasPrintButton = true;
                crystal.HasDrillUpButton = false;
                crystal.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                crystal.EnableParameterPrompt = true;
                crystal.HasRefreshButton = true;
                crystal.HasSearchButton = false;
                crystal.ShowAllPageIds = true;
                crystal.HasToggleGroupTreeButton = false;
                crystal.EnableDrillDown = false;
                crystal.SeparatePages = false;
                crystal.PrintMode = CrystalDecisions.Web.PrintMode.Pdf;
                //if (!Session["ReportName"].ToString().Contains("Demand Letter"))
                //{
                //    cryRpt.PrintToPrinter(1, false, 0, 0);
                //}
                Session["ReportDocument"] = cryRpt;
                cryRpt.Refresh();
                //if (Session["ReportType"].ToString() != "RL")
                //{
                //    cryRpt.SetParameterValue("DocKey@", entry);
                //}
                if (Session["ReportType"].ToString() == "RL")
                {

                }
                else if (Session["ReportType"].ToString() == "Buyer")
                {
                    cryRpt.SetParameterValue("DocKey@", entry);
                    //cryRpt.SetParameterValue("BPCode@", rptParameter);

                    //2023-06-05 : Add Project ; additional parameter on form
                    cryRpt.SetParameterValue("PRJ@", rptParameter);
                }
                else if (Session["ReportType"].ToString() == "Receipt")
                {
                    cryRpt.SetParameterValue("DocKey@", entry);
                    cryRpt.SetParameterValue("Name@", Location);
                }
                else
                {
                    cryRpt.SetParameterValue("DocKey@", entry);
                }
                crystal.DataBind();
                //Session["ReportType"] = "";


            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
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

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showAlert();", true);
        }

    }
}
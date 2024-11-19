using ABROWN_DREAMS;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Api_DREAMS.Controllers
{
    public class DailyReportController : ApiController
    {
        SAPHanaAccess hana = new SAPHanaAccess();

        [HttpPost]
        public string SaveToPDF()
        {
            try
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                string qry = @"SELECT ""PrjCode"",""PrjName"" from ""OPRJ""";
                DataTable dt = hana.GetData(qry, hana.GetConnection("SAOHana"));

                string Server = ConfigurationManager.AppSettings[".HANAServer"];
                string Driver = ConfigurationManager.AppSettings["HANADriver"];
                //** ADD ON **
                string AODatabase = ConfigurationManager.AppSettings["SAODatabase"];
                string AODBUsername = ConfigurationManager.AppSettings["HANAUserID"];
                string AODBPassword = ConfigurationManager.AppSettings["HANAPassword"];
                string AOServer = ConfigurationManager.AppSettings["SAOServer"];

                //HttpResponse test = System.Web.HttpContext.Current.Response;

                ReportDocument cryRpt = new ReportDocument(), crSubreportDocument;
                TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
                TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
                ConnectionInfo crConnectionInfo = new ConnectionInfo();

                foreach (DataRow row in dt.Rows)
                {
                    cryRpt.Load(HttpContext.Current.Server.MapPath("~/TESTING/ABCI - DAYEND REPORT per PROJECT.rpt"));
                    //cryRpt.Load(@"D:\Project\DREAMS\Api_DREAMS\TESTING\ABCI - DAYEND REPORT.rpt");


                    cryRpt.SetParameterValue("DocEntry", row["PrjCode"].ToString());
                    ParameterFieldDefinitions crParameterdef = cryRpt.DataDefinition.ParameterFields;

                    var logonProperties = new DbConnectionAttributes();
                    logonProperties.Collection.Set("Connection String", $@"DRIVER={Driver};SERVERNODE={Server}; UID={AODBUsername};PWD={AODBPassword};CS={AODatabase};");
                    logonProperties.Collection.Set("UseDSNProperties", false);
                    var connectionAttributes = new DbConnectionAttributes();
                    connectionAttributes.Collection.Set("Database DLL", "crdb_odbc.dll");
                    connectionAttributes.Collection.Set("QE_DatabaseName", String.Empty);
                    connectionAttributes.Collection.Set("QE_DatabaseType", "ODBC (RDO)");
                    connectionAttributes.Collection.Set("QE_LogonProperties", logonProperties);
                    connectionAttributes.Collection.Set("QE_ServerDescription", AOServer);
                    connectionAttributes.Collection.Set("QE_SQLDB", false);
                    connectionAttributes.Collection.Set("SSO Enabled", false);

                    crConnectionInfo.ServerName = Server;
                    crConnectionInfo.DatabaseName = AODatabase;
                    crConnectionInfo.UserID = AODBUsername;
                    crConnectionInfo.Password = AODBPassword;

                    crConnectionInfo.Attributes = connectionAttributes;
                    crConnectionInfo.Type = ConnectionInfoType.CRQE;
                    crConnectionInfo.IntegratedSecurity = false;

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


                    ExportOptions CrExportOptions;
                    DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                    PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
                    CrDiskFileDestinationOptions.DiskFileName = $@"D:\Testingcles\{row["PrjCode"].ToString()} - {row["PrjName"].ToString()}.pdf";
                    CrExportOptions = cryRpt.ExportOptions;
                    {
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptions;
                    }
                    cryRpt.Export();
                    cryRpt.Close();

                    FTPUploading(row["PrjCode"].ToString(), row["PrjName"].ToString());

                }

                stopWatch.Stop();
                // Get the elapsed time as a TimeSpan value.
                TimeSpan ts = stopWatch.Elapsed;

                // Format and display the TimeSpan value.
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);

                return "Success. Elapsed time: " + elapsedTime;
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

        }

        void FTPUploading(string PrjCode, string PrjName)
        {
            //FTP Server URL.
            string ftp = "ftp://192.168.10.107:21/";

            //FTP Folder name. Leave blank if you want to upload to root folder.
            string ftpFolder = "";

            //Read the FileName and convert it to Byte array.
            string fileName = Path.GetFileName($@"D:\TestingclesFTP\{PrjCode} - {PrjName}.pdf");

            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftp + ftpFolder + fileName);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential("FTPTest", "1234");

            request.Method = WebRequestMethods.Ftp.UploadFile;
            //request.Credentials = new NetworkCredential(username, password);
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = true;

            //Load the file
            FileStream stream = File.OpenRead($@"D:\Testingcles\{PrjCode} - {PrjName}.pdf");
            byte[] buffer = new byte[stream.Length];

            stream.Read(buffer, 0, buffer.Length);
            stream.Close();

            //Upload file
            Stream reqStream = request.GetRequestStream();
            reqStream.Write(buffer, 0, buffer.Length);
            reqStream.Close();

        }

    }
}
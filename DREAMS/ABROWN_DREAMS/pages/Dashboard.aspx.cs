using ABROWN_DREAMS.VIewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Drawing.Imaging;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Table = CrystalDecisions.CrystalReports.Engine.Table;
using System.Net;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace ABROWN_DREAMS
{
    public partial class Dashboard : Page
    {
        SAPHanaAccess hana = new SAPHanaAccess();
        DirecWebService ws = new DirecWebService();




        protected void Page_Load(object sender, EventArgs e)
        {
            //Uri uri = new Uri("ftp://35.187.254.188/");//the private address
            //if (uri.Scheme != Uri.UriSchemeFtp)
            //{
            //    return;
            //}
            //FtpWebRequest reqFTP;
            //reqFTP = (FtpWebRequest)WebRequest.Create(uri);
            //reqFTP.Credentials = new NetworkCredential("abrown", "abrown123");
            //reqFTP.KeepAlive = false;
            //reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            //reqFTP.UseBinary = true;
            //reqFTP.Proxy = null;
            //reqFTP.UsePassive = true;
            //FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();





            Session["ReportType"] = "";
            if (Session["UserID"] == null)
            {
                alertMsg("Session expired!", "error");
                Response.Redirect("~/pages/Login.aspx");
            }

            if (!IsPostBack)
            {
                txtProjId.Value = ConfigSettings.PRJCode;
                txtProjName.Value = ConfigSettings.PRJName;

                DataTable dtProj = new DataTable();
                dtProj = hana.GetData($@"SELECT ""PrjImage"",""ImgWidth"",""ImgHeight"" FROM ""OPRJ"" Where ""PrjCode"" = '{txtProjId.Value}' AND ""PrjImage"" IS NOT NULL", hana.GetConnection("SAOHana"));

                if (dtProj.Rows.Count > 0)
                {
                    string imgwidth = dtProj.Rows[0][1].ToString();
                    string imgheight = dtProj.Rows[0][2].ToString();
                    //load block
                    imgProjectWidth.Value = imgwidth;
                    imgProjectHeight.Value = imgheight;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "imgProject", "drawProjectMap();", true);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "loadCanvas();", true);
                //close modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "closeProjectList();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "closeProjectList();", true);

                string qry = "";
                DataTable dt = new DataTable();

                //CHECK IF BROKER
                string brkcheck = Session["BrkPosition"].ToString();

                if (!String.IsNullOrWhiteSpace(brkcheck))
                {
                    //CHECKING OF ATP EXPIRATION DATE
                    qry = $@"SELECT 
	                            ABS(DAYS_BETWEEN(CURRENT_DATE, D.""ATPDate"")) ""ATPDate"" --ATP Expiry Date
	                            ,ABS(DAYS_BETWEEN(CURRENT_DATE, D.""ValidTo"")) ""ValidTo"" --PTR Expiry Date
	                            ,ABS(DAYS_BETWEEN(CURRENT_DATE, D.""ValidTo"")) ""PRCLicenseValid"" --PRC Expiry Date
	                            ,ABS(DAYS_BETWEEN(CURRENT_DATE, D.""PassportValidTo"")) ""PassportValidTo"" --Passport Valid To
	                            ,ABS(DAYS_BETWEEN(CURRENT_DATE, D.""AIPO_ValidTo"")) ""AIPO_ValidTo"" --AIPO Valid To
	                            ,ABS(DAYS_BETWEEN(CURRENT_DATE, D.""IDExpirationDate"")) ""IDExpiratinDate""
	                            ,ABS(DAYS_BETWEEN(CURRENT_DATE, D.""IDExpirationDate2"")) ""IDExpiratinDate2""
	                            ,ABS(DAYS_BETWEEN(CURRENT_DATE, D.""IDExpirationDate3"")) ""IDExpiratinDate3""
	                            ,ABS(DAYS_BETWEEN(CURRENT_DATE, D.""IDExpirationDate4"")) ""IDExpiratinDate4""
                            FROM 
	                            BRK1 A 
                            INNER JOIN 
                                OSLA B 
                            ON 
	                            A.""Id"" = B.""Id"" 
                            LEFT JOIN 
	                            OUSR C 
                            ON 
	                            A.""SAPCardCode"" = C.""Username""
                            INNER JOIN
	                            OBRK D
                            ON
	                            D.""BrokerId"" = A.""BrokerId""
                            WHERE C.""ID"" = IFNULL('{Session["UserID"]}',0)";
                    dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
                    if ((Convert.ToInt32(DataAccess.GetData(dt, 0, "ATPDate", "30").ToString()) <= Convert.ToInt32(ConfigSettings.ATPDateExp)
                        || Convert.ToInt32(DataAccess.GetData(dt, 0, "ValidTo", "30").ToString()) <= Convert.ToInt32(ConfigSettings.ATPDateExp)
                        || Convert.ToInt32(DataAccess.GetData(dt, 0, "PRCLicenseValid", "30").ToString()) <= Convert.ToInt32(ConfigSettings.ATPDateExp)
                        || Convert.ToInt32(DataAccess.GetData(dt, 0, "PassportValidTo", "30").ToString()) <= Convert.ToInt32(ConfigSettings.ATPDateExp)
                        || Convert.ToInt32(DataAccess.GetData(dt, 0, "AIPO_ValidTo", "30").ToString()) <= Convert.ToInt32(ConfigSettings.ATPDateExp)
                        || Convert.ToInt32(DataAccess.GetData(dt, 0, "IDExpiratinDate", "30").ToString()) <= Convert.ToInt32(ConfigSettings.ATPDateExp)
                        || Convert.ToInt32(DataAccess.GetData(dt, 0, "IDExpiratinDate2", "30").ToString()) <= Convert.ToInt32(ConfigSettings.ATPDateExp)
                        || Convert.ToInt32(DataAccess.GetData(dt, 0, "IDExpiratinDate3", "30").ToString()) <= Convert.ToInt32(ConfigSettings.ATPDateExp)
                        || Convert.ToInt32(DataAccess.GetData(dt, 0, "IDExpiratinDate4", "30").ToString()) <= Convert.ToInt32(ConfigSettings.ATPDateExp))
                        && Convert.ToInt32(Session["UserID"]) != 1
                        )
                    {
                        alertMsg($"Your credential/s are nearly expired. Please update your credentials to avoid being blocked in our system", "warning");
                    }
                }
                //if (Convert.ToInt32(DataAccess.GetData(dt, 0, "ATPDate", "0").ToString()) <= Convert.ToInt32(ConfigSettings.ATPDateExp"].ToString()))
                //{
                //    alertMsg($"The ATP Date will expire on {DataAccess.GetData(dt, 0, "ATPDate", "0")} days. Please contact administrator.", "warning");
                //}

                dt = hana.GetData($@"SELECT ""Name"" FROM ""OLST"" Where ""Code"" = 'BRK'", hana.GetConnection("SAOHana"));
                DateTime brokerRenewalDate = Convert.ToDateTime(DataAccess.GetData(dt, 0, "Name", ""));
                if (DateTime.Now.ToString("yyyy-MM-dd") == brokerRenewalDate.ToString("yyyy-MM-dd"))
                {
                    alertMsg($"The date today is the set Renewal Date ({brokerRenewalDate}). Please renew broker information immediately at: {ConfigurationManager.AppSettings["BrokerPageLink"].ToString()}.", "warning");

                }

                LoadStatus();

            }


        }
        void LoadStatus()
        {
            Session["dtDocumentStatus"] = ws.GetDocumentStatus().Tables["DocumentStatus"];
            LoadData(gvBlockColor, "dtDocumentStatus");
        }
        void LoadData(GridView gv, string session)
        {
            gv.DataSource = (DataTable)Session[session];
            gv.DataBind();
        }

        protected void btnExport_ServerClick(object sender, EventArgs e)
        {

        }
        //public override void VerifyRenderingInServerForm(Control control)
        //{
        //}

        protected void btnShowProj_ServerClick(object sender, EventArgs e)
        {
            LoadSAPProjects();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "projList", "showProjList();", true);
        }
        void LoadSAPProjects()
        {
            if (gvProjectList.Rows.Count == 0)
            {
                gvProjectList.DataSource = ws.GetSAPProjects();
                gvProjectList.DataBind();
            }
        }

        protected void bSearch_Click(object sender, EventArgs e)
        {
            gvProjectList.DataSource = ws.SearchSAPProjects(txtSearch.Value);
            gvProjectList.DataBind();
        }

        protected void gvProjectList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int row = int.Parse(e.CommandArgument.ToString());
            if (e.CommandName.Equals("Sel"))
            {
                string projCode = string.IsNullOrEmpty(gvProjectList.Rows[row].Cells[0].Text) ? "PARK INFINA" : gvProjectList.Rows[row].Cells[0].Text;
                string projName = string.IsNullOrEmpty(gvProjectList.Rows[row].Cells[1].Text) ? "PARK INFINA" : gvProjectList.Rows[row].Cells[1].Text;

                txtProjId.Value = projCode;
                txtProjName.Value = projName;

                DataTable dtProj = new DataTable();
                dtProj = hana.GetData($@"SELECT ""PrjImage"",""ImgWidth"",""ImgHeight"" FROM ""OPRJ"" Where ""PrjCode"" = '{txtProjId.Value}' AND ""PrjImage"" IS NOT NULL", hana.GetConnection("SAOHana"));

                if (dtProj.Rows.Count > 0)
                {
                    string imgwidth = dtProj.Rows[0][1].ToString();
                    string imgheight = dtProj.Rows[0][2].ToString();
                    //load block
                    imgProjectWidth.Value = imgwidth;
                    imgProjectHeight.Value = imgheight;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "imgProject", "drawProjectMap();", true);
                //close modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "closeProjectList();", true);
            }
        }

        protected void btnLotPreview_ServerClick(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = hana.GetData($@"SELECT ""ImgWidth"",""ImgHeight"" FROM ""PRJ1"" WHERE ""PrjCode"" = '{txtProjId.Value}' AND ""Block"" = '{txtSelectedBlock.Value}'", hana.GetConnection("SAOHana"));
            txtLotWidthPreview.Value = (string)DataAccess.GetData(dt, 0, "ImgWidth", "0");
            txtLotHeightPreview.Value = (string)DataAccess.GetData(dt, 0, "ImgHeight", "0");

            ScriptManager.RegisterStartupScript(this, this.GetType(), "imgProject", "drawProjectMap();", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "drawLot", "drawLotPreview();", true);
            //show modal
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "show", "showLotPreview();", true);


            //DataTable dt = new DataTable();
            //dt = ws.Select($"SELECT ImgWidth,ImgHeight FROM PRJ1 WHERE PrjCode = '{hPrjCode.Value}' AND Block = '{tBlock.Value}'", "PRJ1", "Addon").Tables["PRJ1"];
            //hLotWidth.Value = (string)DataAccess.GetData(dt, 0, "ImgWidth", "0");
            //hLotHeight.Value = (string)DataAccess.GetData(dt, 0, "ImgHeight", "0");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "imgProject", "drawProjectMap();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "MsgLotList_Show", "MsgLotList_Show();", true);
            LoadStatus();

        }

        protected void gvProjectList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProjectList.DataSource = ws.GetSAPProjects();
            gvProjectList.PageIndex = e.NewPageIndex;
            gvProjectList.DataBind();
        }

        protected void bGenerate_ServerClick(object sender, EventArgs e)
        {
            try
            {

                //ScriptManager.RegisterStartupScript(this, GetType(), "Gen", "MsgLotList_Hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "imgProject", "drawProjectMap();", true);

                DataTable dt = new DataTable();
                string qryStatusLot = $@"SELECT ""U_LotStatus"" FROM OBTN WHERE ""U_BlockNo"" = '{txtSelectedBlock.Value}' AND 
                        ""U_LotNo"" = '{txtSelectedLot.Value}' AND ""U_Project"" = '{txtProjId.Value}'";
                dt = hana.GetData(qryStatusLot, hana.GetConnection("SAPHana"));
                string LotStatus = (string)DataAccess.GetData(dt, 0, "U_LotStatus", "");

                if (LotStatus == "S01")
                {
                    dt = ws.GetHouseStatus(txtProjId.Value, txtSelectedBlock.Value, txtSelectedLot.Value, "", "").Tables["GetHouseStatus"];
                    if (DataAccess.Exist(dt) == true)
                    {


                        string modelName = (string)DataAccess.GetData(dt, 0, "Code", "");
                        lblModel.Text = modelName;
                        dt = ws.GetHouseModel(txtProjId.Value, modelName).Tables["GetHouseModel"];
                        if (DataAccess.Exist(dt))
                        {
                            string imageURL = (string)DataAccess.GetData(dt, 0, "U_Picture", "");
                            imgHouse.ImageUrl = $"~/Handler/HouseModel.ashx?ID={imageURL}";

                            lblFloorArea.Text = (string)DataAccess.GetData(dt, 0, "U_HouseModel", "");
                            lblResFee.Text = (string)DataAccess.GetData(dt, 0, "ResFee", "");
                            //bPicPreview.CommandArgument = "";

                            string qry = $@"SELECT COUNT(*) ""Number of Buyers"" from OQUT A INNER JOIN CRD1 B ON A.""CardCode"" = B.""CardCode"" AND B.""CardType"" = 'Buyer' INNER JOIN QUT5 C ON A.""DocEntry"" = C.""DocEntry"" AND IFNULL(C.""EmpName"",'') <> ''
                                        WHERE A.""ProjCode"" = '{txtProjId.Value}' AND A.""Block"" = '{txtSelectedBlock.Value}' AND A.""Lot"" = '{txtSelectedLot.Value}'";
                            //dt = hana.GetData(qry, hana.GetConnection("SAOHana"));

                            DataTable dt1 = hana.GetData(qry, hana.GetConnection("SAOHana"));
                            //lblInterestedBuyer.Text = (string)DataAccess.GetData(dt, 0, "Name", " *** ");
                            lblNoOfBuyers.Text = (string)DataAccess.GetData(dt1, 0, "Number of Buyers", "");

                            ScriptManager.RegisterStartupScript(this, GetType(), "MsgHouseList", "MsgHouseList_Show();", true);
                        }
                        else
                        {
                            alertMsg($"House model does not exist to Block: {txtSelectedBlock.Value} and Lot: {txtSelectedLot.Value}", "info");
                        }
                    }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "drawProjectMap", "drawProjectMap();", true);
                    //ScriptManager.RegisterStartupScript(this, GetType(), "MsgHouseList", "MsgHouseList_Show();", true);
                    //gvHouseList.DataSource = ws.GetHouseModel(txtProjId.Value, modelName).Tables["GetHouseModel"];
                    //gvHouseList.DataBind();
                }
                else if (LotStatus == "S02")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "drawProjectMap", "drawProjectMap();", true);
                    alertMsg("Lot is not available for sale.", "info");
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "drawProjectMap", "drawProjectMap();", true);
                    alertMsg("Lot is already sold.", "info");
                    //alertMsg("Lot is not available anymore for viewing.", "info");
                }
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }
        }

        //protected void bPicPreview_Click(object sender, EventArgs e)
        //{
        //    ScriptManager.RegisterStartupScript(this, GetType(), "MsgHouseList", "MsgHouseList_Hide();", true);
        //    gvPicPreview.DataSource = ws.GetHousePicture(lblModel.Text).Tables["GetHousePicture"];
        //    gvPicPreview.DataBind();
        //    ScriptManager.RegisterStartupScript(this, GetType(), "MsgPicPreview", "MsgPicPreview_Show();", true);
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "imgProject", "drawProjectMap();", true);
        //}

        protected void bChooseHouse_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string Code = GetID.CommandArgument;
            DataTable dt = new DataTable();
            dt = ws.GetHousePicture(Code).Tables["GetHousePicture"];
            Session["HouseStatusCode"] = (string)DataAccess.GetData(dt, 0, "Code", "");
            //tModel.Value = (string)DataAccess.GetData(dt, 0, "Name", "");

            //tResrvFee.Text = SystemClass.ToNumeric((string)DataAccess.GetData(dt, 0, "U_ResFee", "0"));
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgHouseList", "MsgHouseList_Hide();", true);
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

        [WebMethod]
        public static void ReportCreation(List<ReportViewModel.ImageData1> items)
        {

            DirecWebService ws = new DirecWebService();
            try
            {

                foreach (var data in items)
                {
                    string projCode = items[0].prjCode.ToString();
                    string base64String = items[0].canvassImage.Replace("data:image/jpeg;base64,", "");
                    string tobyte = base64String.Replace('_', '/').Replace('-', '+');
                    switch (base64String.Length % 4)
                    {
                        case 2:
                            tobyte += "==";
                            break;
                        case 3:
                            tobyte += "=";
                            break;
                    }
                    byte[] img_byte = Convert.FromBase64String(tobyte);

                    if (ws.UpdateProjectCanvass("Addon", projCode, img_byte) == true)
                    {
                        //return "Block updated successfully";
                        //DeleteBlockTemp(txtProjId.Value, txtBlock.Text);
                        // alertMsg("Download Successfully", "success");
                    }
                    else
                    {
                        //return "Updating of block failed";
                        //alertMsg("Download failed", "error");
                    }
                }

                //foreach (var data in dataList)
                //{
                //    string projCode = dataList[0];


                //    string base64String = projCode.Replace("data:image/jpeg;base64,", "");
                //    string tobyte = base64String.Replace('_', '/').Replace('-', '+');
                //    byte[] img_byte = Convert.FromBase64String(tobyte);

                //    //if (ws.UpdateProjectCanvass("Addon", projCode, img_byte) == true)
                //    //{
                //    //    //return "Block updated successfully";
                //    //    //DeleteBlockTemp(txtProjId.Value, txtBlock.Text);
                //    //    //alertMsg("Download Successfully", "success");
                //    //}
                //    //else
                //    //{
                //    //    //return "Updating of block failed";
                //    //    //alertMsg("Download failed", "error");
                //    //}
                //}

            }
            catch (Exception ex)
            {

            }
            //return "post";
            //return true;
        }

        [WebMethod]
        public static void GenerateMapForm()
        {
            HttpContext.Current.Session["PrintDocEntry"] = "";
            HttpContext.Current.Session["Title"] = "";
            HttpContext.Current.Session["ReportName"] = ConfigSettings.MapGenerationForm;
            HttpContext.Current.Session["ReportPath"] = ConfigurationManager.AppSettings["ReportPath"];
            HttpContext.Current.Session["ReportType"] = "RL";

            HttpContext.Current.Session["RptConn"] = "Addon";

            //open new tab
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openpage", "window.open('ReportViewer.aspx');", true);
        }
        protected void btnGenerateRep_ServerClick(object sender, EventArgs e)
        {
            try
            {
                //HtmlTable table = ((HtmlTable)tblTest);
                //table.Rows[0].Cells[0].FindControl("ControlId");

                string sketchData = Request.Form["sketch_data"];
                //string[] split = sketchData.Split('Dbti');

                string projCode = txtProjId.Value;

                //byte[] img_byte = DataAccess.ConvertImageToByteArray(img, ImageFormat.Jpeg);
                //string bytes = Convert.FromBase64String(img_byte.Split(',')[1]);

                //System.Drawing.Image convertToImage = System.Drawing.Image.FromStream(new MemoryStream(Convert.FromBase64String(base64String)));
                //string fileName = "myImage.jpeg";
                //convertToImage.Save(Server.MapPath("~/LETTER_REQ/" + fileName));

                string base64String = sketchData.Replace("data:image/jpeg;base64,", "");
                string tobyte = base64String.Replace('_', '/').Replace('-', '+');
                switch (base64String.Length % 4)
                {
                    case 2:
                        tobyte += "==";
                        break;
                    case 3:
                        tobyte += "=";
                        break;
                }

                byte[] img_byte = Convert.FromBase64String(tobyte);

                if (ws.UpdateProjectCanvass("Addon", projCode, img_byte) == true)
                {
                    //return "Block updated successfully";
                    //DeleteBlockTemp(txtProjId.Value, txtBlock.Text);
                    alertMsg("Download Successfully", "success");
                    HttpContext.Current.Session["PrintDocEntry"] = projCode;
                    HttpContext.Current.Session["Title"] = "";
                    HttpContext.Current.Session["ReportName"] = ConfigSettings.SingleMapGenerationForm;
                    HttpContext.Current.Session["ReportPath"] = ConfigurationManager.AppSettings["ReportPath"];
                    HttpContext.Current.Session["ReportType"] = "";

                    HttpContext.Current.Session["RptConn"] = "Addon";

                    //open new tab
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openpage", "window.open('ReportViewer.aspx');", true);
                }
                else
                {
                    //return "Updating of block failed";
                    alertMsg("Download failed", "error");
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "loadCanvas();", true);
            }
            catch (Exception ex)
            {

            }

        }

        protected void btnreportGenerator_ServerClick(object sender, EventArgs e)
        {
            try
            {


            }
            catch (Exception exc)
            {

            }


        }




        void Generateion(string qry)
        {
            try
            {
                DataTable dt = hana.GetData(qry, hana.GetConnection("SAOHana"));

                string Server = ConfigurationManager.AppSettings["HANAServer"];
                string Driver = ConfigurationManager.AppSettings["HANADriver"];
                //** ADD ON **
                string AODatabase = ConfigurationManager.AppSettings["SAODatabase"];
                string AODBUsername = ConfigurationManager.AppSettings["HANAUserID"];
                string AODBPassword = ConfigurationManager.AppSettings["HANAPassword"];
                string AOServer = ConfigurationManager.AppSettings["SAOServer"];



                ReportDocument cryRpt = new ReportDocument(), crSubreportDocument;
                TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
                TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
                CrystalDecisions.Shared.ConnectionInfo crConnectionInfo = new CrystalDecisions.Shared.ConnectionInfo();


                foreach (DataRow row in dt.Rows)
                {
                    cryRpt.Load(HttpContext.Current.Server.MapPath($@"{ConfigurationManager.AppSettings["LocationMap"]}/{ ConfigSettings.SingleMapGenerationForm}"));

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


                    string FilePathForMap = $@"{ConfigurationManager.AppSettings["GenerationMap"]}\{row["PrjCode"].ToString()} - {row["PrjName"].ToString()}.pdf";

                    //GENERATION OF PDF TO THE FOLDER
                    CrDiskFileDestinationOptions.DiskFileName = HttpContext.Current.Server.MapPath(FilePathForMap);
                    CrExportOptions = cryRpt.ExportOptions;
                    {
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptions;
                    }
                    cryRpt.Export();
                    cryRpt.Close();

                    //FTPUploading(row["PrjCode"].ToString(), row["PrjName"].ToString());
                    //SFTPUploading(row["PrjCode"].ToString(), row["PrjName"].ToString());
                    //MoveFiles(row["PrjCode"].ToString(), row["PrjName"].ToString());

                    alertMsg($@"Successfully generated project map for {txtProjName.Value}.", "success");

                }

            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "warning");
            }
        }




        void FTPUploading(string PrjCode, string PrjName)
        {
            try
            {



                //FTP Server URL.
                string ftp = ConfigurationManager.AppSettings["FTP"];

                //FTP Folder name. Leave blank if you want to upload to root folder.
                string ftpFolder = "";

                //Read the FileName and convert it to Byte array.
                string fileName = Path.GetFileName($@"{ConfigurationManager.AppSettings["GenerationMap"]}\{PrjCode} - {PrjName}.pdf");

                // Get the object used to communicate with the server.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftp + ftpFolder + fileName);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["FTPUser"], ConfigurationManager.AppSettings["FTPPassword"]);

                request.Method = WebRequestMethods.Ftp.UploadFile;
                //request.Credentials = new NetworkCredential(username, password);
                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = true;



                //Load the file
                FileStream stream = File.OpenRead($@"{ConfigurationManager.AppSettings["GenerationMapRead"]}\{PrjCode} - {PrjName}.pdf");
                byte[] buffer = new byte[stream.Length];

                stream.Read(buffer, 0, buffer.Length);

                //Upload file
                Stream reqStream = request.GetRequestStream();
                reqStream.Write(buffer, 0, buffer.Length);
                stream.Close();
                reqStream.Close();
            }
            catch (Exception ex)
            {

            }
        }


        void SFTPUploading(string PrjCode, string PrjName)
        {
            try
            {
                // Enter IP here
                string host = "35.187.254.188";

                // Enter user here
                string username = "abrown";

                // Enter password here
                string password = "abrown123";


                //using (SftpClient _sftp = new SftpClient(host, username, password))
                //{
                //    foreach (var d in _sftp.ConnectionInfo.Encryptions.Where(p => p.Key != "aes128-cbc").ToList())
                //    {
                //        _sftp.ConnectionInfo.Encryptions.Remove(d.Key);
                //    }

                //    _sftp.Connect();
                //}

                //KeyboardInteractiveAuthenticationMethod keybAuth = new KeyboardInteractiveAuthenticationMethod(username);
                //keybAuth.AuthenticationPrompt += new EventHandler<AuthenticationPromptEventArgs>(HandleKeyEvent);

                //Renci.SshNet.ConnectionInfo connectionInfo = new Renci.SshNet.ConnectionInfo(host, "username", new PasswordAuthenticationMethod(username, password), keybAuth);

                var connectionInfo = new Renci.SshNet.ConnectionInfo(host, "sftp", new PasswordAuthenticationMethod(username, password));

                string fileName = $@"{ConfigurationManager.AppSettings["GenerationMap"]}\{PrjCode} - {PrjName}.pdf";

                // Upload File
                using (var sftp = new SftpClient(connectionInfo))
                {

                    sftp.Connect();
                    sftp.ChangeDirectory("/files");
                    using (var uplfileStream = System.IO.File.OpenRead(fileName))
                    {
                        sftp.UploadFile(uplfileStream, fileName, true);
                    }
                    sftp.Disconnect();
                }

            }
            catch (Exception ex)
            {

            }

        }




        void HandleKeyEvent(Object sender, Renci.SshNet.Common.AuthenticationPromptEventArgs e)
        {
            foreach (Renci.SshNet.Common.AuthenticationPrompt prompt in e.Prompts)
            {
                if (prompt.Request.IndexOf("Password:", StringComparison.InvariantCultureIgnoreCase) != -1)
                {
                    prompt.Response = "abrown123";
                }
            }
        }



        void MoveFiles(string PrjCode, string PrjName)
        {
            string SourcefileName = $@"{ConfigurationManager.AppSettings["GenerationMap"]}\{PrjCode} - {PrjName}.pdf";
            string DestinationfileName = $@"{ConfigurationManager.AppSettings["GenerationMapRead"]}\{PrjCode} - {PrjName}.pdf";

            string sourceFilePath = Server.MapPath(SourcefileName);
            string destinationFilePath = Server.MapPath(DestinationfileName);

            if (File.Exists(sourceFilePath))
            {
                File.Copy(sourceFilePath, destinationFilePath, true);
            }
        }


        protected void btnGenerateTest_ServerClick(object sender, EventArgs e)
        {
            string sketchData = Request.Form["sketch_data"];

            string projCode = txtProjId.Value;

            string base64String = sketchData.Replace("data:image/jpeg;base64,", "");
            string tobyte = base64String.Replace('_', '/').Replace('-', '+');
            switch (base64String.Length % 4)
            {
                case 2:
                    tobyte += "==";
                    break;
                case 3:
                    tobyte += "=";
                    break;
            }

            byte[] img_byte = Convert.FromBase64String(tobyte);

            if (ws.UpdateProjectCanvass("Addon", projCode, img_byte) == true)
            {
                Generateion($@"SELECT ""PrjCode"",""PrjName"" from ""OPRJ"" WHERE ""PrjCode"" = '{txtProjId.Value}'");
            }
        }


        //protected void btnGenerateTestAll_ServerClick(object sender, EventArgs e)
        //{

        //}
        //[WebMethod]
        //public static void GenerateTestAllProj(List<ReportViewModel.ImageData1> items)
        //{
        //    DirecWebService ws = new DirecWebService();
        //    Dashboard db = new Dashboard();
        //    try
        //    {
        //        foreach (var data in items)
        //        {
        //            string projCode = items[0].prjCode.ToString();
        //            string base64String = items[0].canvassImage.Replace("data:image/jpeg;base64,", "");
        //            string tobyte = base64String.Replace('_', '/').Replace('-', '+');
        //            switch (base64String.Length % 4)
        //            {
        //                case 2:
        //                    tobyte += "==";
        //                    break;
        //                case 3:
        //                    tobyte += "=";
        //                    break;
        //            }
        //            byte[] img_byte = Convert.FromBase64String(tobyte);

        //            if (ws.UpdateProjectCanvass("Addon", projCode, img_byte) == true)
        //            {
        //                //return "Block updated successfully";
        //                //DeleteBlockTemp(txtProjId.Value, txtBlock.Text);
        //                // alertMsg("Download Successfully", "success");
        //            }
        //            else
        //            {
        //                //return "Updating of block failed";
        //                //alertMsg("Download failed", "error");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //    db.Generateion($@"SELECT ""PrjCode"",""PrjName"" from ""OPRJ""");
        //}
    }
}
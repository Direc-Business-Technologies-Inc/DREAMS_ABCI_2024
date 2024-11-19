using ABROWN_DREAMS.wcf;
using DataCipher;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;

namespace ABROWN_DREAMS
{
    public partial class Login : Page
    {
        enum Noti { Success, Error };
        DirecWebService ws = new DirecWebService();
        SAPHanaAccess hana = new SAPHanaAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            DREAMSVersion.InnerText = version;


            //2023-09-06 : GET DATABASE NAME
            AddonDBName.Text = ConfigurationManager.AppSettings["SAODatabase"];
            SapDBName.Text = ConfigurationManager.AppSettings["HANADatabase"];
        }
        protected void btnSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {


                DataTable dt = new DataTable();
                dt = hana.GetData($@"SELECT ""Password"" FROM ""OUSR"" WHERE ""Username"" = '{txtUser.Value}'", hana.GetConnection("SAOHana"));

                if (DataAccess.Exist(dt) == true)
                {
                    Session["BrkStatus"] = "";
                    Session["BrkReason"] = "";
                    Session["CompName"] = "";
                    Session["ProdName"] = "";
                    Session["Brkcreds"] = "";
                    Session["UserAccess"] = null;

                    if (ws.WebLogin(txtUser.Value, txtPass.Value) == true)
                    {
                        string qry = "";

                        //CHECK IF SALES AGENT OR BROKER
                        Session["BrkPosition"] = "";
                        qry = $@"SELECT A.""Position"", A.""SalesPerson"", A.""Id""
                                FROM OSLA A
	                                Inner join BRK1 B ON A.""Id"" = B.""Id""	
                                WHERE B.""SAPCardCode"" = '{txtUser.Value}'";
                        DataTable dt3 = hana.GetData(qry, hana.GetConnection("SAOHana"));
                        Session["BrkPosition"] = DataAccess.GetData(dt3, 0, "Position", "").ToString();

                        int UserID = ws.WebUserID(txtUser.Value);
                        Session["UserName"] = txtUser.Value;
                        //CHECKING OF RENEWAL DATE

                        //CHECKING OF CREDENTIALS EXPIRATION DATE
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
                            WHERE C.""ID"" = IFNULL('{UserID.ToString()}',0)";
                        DataTable dt2 = hana.GetData(qry, hana.GetConnection("SAOHana"));
                        if (Convert.ToInt32(DataAccess.GetData(dt, 0, "ATPDate", "30").ToString()) < 0
                            || Convert.ToInt32(DataAccess.GetData(dt, 0, "ValidTo", "30").ToString()) < 0
                            || Convert.ToInt32(DataAccess.GetData(dt, 0, "PRCLicenseValid", "30").ToString()) < 0
                            || Convert.ToInt32(DataAccess.GetData(dt, 0, "PassportValidTo", "30").ToString()) < 0
                            || Convert.ToInt32(DataAccess.GetData(dt, 0, "AIPO_ValidTo", "30").ToString()) < 0
                            || Convert.ToInt32(DataAccess.GetData(dt, 0, "IDExpiratinDate", "30").ToString()) < 0
                            || Convert.ToInt32(DataAccess.GetData(dt, 0, "IDExpiratinDate2", "30").ToString()) < 0
                            || Convert.ToInt32(DataAccess.GetData(dt, 0, "IDExpiratinDate3", "30").ToString()) < 0
                            || Convert.ToInt32(DataAccess.GetData(dt, 0, "IDExpiratinDate4", "30").ToString()) < 0
                            )
                        {
                            NotiMsg($"Your credential/s are expired. Please update your credentails to be able to login again.", Noti.Error);
                        }
                        else
                        {
                            dt = hana.GetData($@"SELECT * FROM ""USR2"" WHERE ""UserID"" = {UserID}", hana.GetConnection("SAOHana"));
                            DataTable dtUserAccess = new DataTable();
                            dtUserAccess.Columns.AddRange(new DataColumn[1]
                            {
                                new DataColumn("CodeEncrypt")
                            });

                            if (DataAccess.Exist(dt) == true)
                            {
                                foreach (DataRow rows in dt.Rows)
                                {
                                    //string Mode = Cryption.Decrypt((string)DataAccess.GetData(dt, 0, "CodeEncrypt", ""));
                                    string Mode = Cryption.Decrypt(rows["CodeEncrypt"].ToString());
                                    int cnt = Mode.Length - UserID.ToString().Length;
                                    Mode = Mode.Substring(UserID.ToString().Length, cnt);
                                    dtUserAccess.Rows.Add(Mode);
                                }

                                //Session["UserAccess"] = Mode;
                                Session["UserAccess"] = dtUserAccess;
                            }

                            //if (DateTime.Now >= Convert.ToDateTime(App.AppSettings("regedit")))
                            //{
                            //    NotiMsg("Trial period expire. Please contact administrator.", Noti.Error);
                            //}
                            //else
                            //{
                            SapHanaLayer company = new SapHanaLayer();
                            bool result = company.Connect();
                            if (!result)
                            {
                                NotiMsg($"({company.ResultCode}) {company.ResultDescription}", Noti.Error);
                            }
                            else
                            {
                                //FOR CHECKING OF RENEWAL DATE 
                                if (txtUser.Value != "admin")
                                {
                                    //if (Session["UserAccess"].ToString() != "PSA")
                                    DataTable dtUserAccess1 = (DataTable)Session["UserAccess"];
                                    if (dtUserAccess1 != null)
                                    {
                                        var psa = dtUserAccess1.Select($"CodeEncrypt= 'PSA'");
                                        if (!psa.Any())
                                        {
                                            //CHECK IF THE USER IS ALREADY APPROVED
                                            dt2 = hana.GetData($@"SELECT ""ApprovalDate"" FROM ""OBRK"" Where ""SAPCardCode"" = '{txtUser.Value}'", hana.GetConnection("SAOHana"));
                                            DateTime ApprovalDate = Convert.ToDateTime(DataAccess.GetData(dt2, 0, "ApprovalDate", null));
                                            if (ApprovalDate.Year >= DateTime.Now.Year)
                                            {
                                                //FROM DATE
                                                dt = hana.GetData($@"SELECT ""Name"" FROM ""OLST"" Where ""Code"" = 'BRK2'", hana.GetConnection("SAOHana"));
                                                DateTime brokerRenewalDate = Convert.ToDateTime(DataAccess.GetData(dt, 0, "Name", ""));
                                                //TO DATE
                                                dt2 = hana.GetData($@"SELECT ""Name"" FROM ""OLST"" Where ""Code"" = 'BRK'", hana.GetConnection("SAOHana"));
                                                DateTime brokerRenewalDateExpiry = Convert.ToDateTime(DataAccess.GetData(dt2, 0, "Name", ""));
                                                //GRACE PERIOD
                                                dt2 = hana.GetData($@"SELECT ""Name"" FROM ""OLST"" Where ""Code"" = 'BRK3'", hana.GetConnection("SAOHana"));
                                                double GracePeriod = Convert.ToDouble(DataAccess.GetData(dt2, 0, "Name", ""));
                                                //MINIMUM DAYS
                                                dt2 = hana.GetData($@"SELECT ""Name"" FROM ""OLST"" Where ""Code"" = 'BRK4'", hana.GetConnection("SAOHana"));
                                                double MinimumDays = Convert.ToDouble(DataAccess.GetData(dt2, 0, "Name", ""));
                                                //INDICATOR IF FOR 2ND NOTIF OR FOR BLOCKING STAGE
                                                dt2 = hana.GetData($@"SELECT ""EmailAddress"", ""BrokerId"",""EmailNotifStage"" FROM ""OBRK"" Where ""SAPCardCode"" = '{txtUser.Value}'", hana.GetConnection("SAOHana"));






                                                //if (brokerRenewalDate.Date == DateTime.Now.Date && (Math.Abs(brokerRenewalDateExpiry.Subtract(DateTime.Now).TotalDays)) < Convert.ToInt32(App.AppSettings("ATPDateExp")))
                                                if (ApprovalDate < brokerRenewalDate.Date && (Math.Abs(brokerRenewalDate.Subtract(DateTime.Now).TotalDays) > GracePeriod) && GracePeriod > 0)
                                                {
                                                    //dt = hana.GetData($@"SELECT ""UpdateDate"", ""BrokerId"" FROM ""OBRK"" Where ""SAPCardCode"" = '{txtUser.Value}'", hana.GetConnection("SAOHana"));
                                                    //string CreateDate = Convert.ToDateTime(DataAccess.GetData(dt, 0, "UpdateDate", "2000-01-01")).ToString("yyyy-MM-dd");
                                                    //DateTime UpdateDate = Convert.ToDateTime(DataAccess.GetData(dt, 0, "UpdateDate", CreateDate));
                                                    //if (UpdateDate > brokerRenewalDate)
                                                    //{

                                                    //EMAIL NOTIF FOR EXPIRY OF GRACE PERIOD IF NOT SENT BEFORE
                                                    dt2 = hana.GetData($@"SELECT ""EmailAddress"", ""BrokerId"",""EmailNotifStage"" FROM ""OBRK"" Where ""SAPCardCode"" = '{txtUser.Value}'", hana.GetConnection("SAOHana"));
                                                    //if (DataAccess.GetData(dt2, 0, "EmailNotifStage", "").ToString() == "2")
                                                    //{
                                                    qry = $@"SELECT * FROM ""@EMAIL"" WHERE ""Code"" = 'BRKRNWLBLK'";
                                                    dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
                                                    ws.sendEmail(DataAccess.GetData(dt2, 0, "BrokerId", "").ToString(),
                                                                            DataAccess.GetData(dt, 0, "U_Subject", "").ToString(),
                                                                            DataAccess.GetData(dt2, 0, "EmailAddress", "").ToString(),
                                                                            DataAccess.GetData(dt, 0, "U_Body", "").ToString(),
                                                                            "",
                                                                            "",
                                                                            "Broker ID",
                                                                            DataAccess.GetData(dt, 0, "Code", "").ToString());
                                                    hana.Execute($@"UPDATE OBRK SET ""EmailNotifStage"" = '3', ""Status"" = 'RENEWAL' Where ""SAPCardCode"" = '{txtUser.Value}'", hana.GetConnection("SAOHana"));
                                                    //}

                                                    //ALERT MSG
                                                    NotiMsg($"Login failed: Renewal Date has passed. Please update information in Broker Page.", Noti.Error);

                                                    //}
                                                    //else
                                                    //{
                                                    //    Session["Company"] = company;
                                                    //    Session["UserID"] = UserID;
                                                    //    Session["BrokerId"] = DataAccess.GetData(dt, 0, "BrokerId", "");
                                                    //    Response.Redirect("~/pages/Dashboard.aspx");
                                                    //}
                                                }


                                                //else if (brokerRenewalDate.Date >= DateTime.Now.Date && (Math.Abs(brokerRenewalDateExpiry.Subtract(DateTime.Now).TotalDays)) < Convert.ToInt32(App.AppSettings("ATPDateExp")))
                                                else if (ApprovalDate < brokerRenewalDate.Date && (Math.Abs(brokerRenewalDate.Subtract(DateTime.Now).TotalDays) > GracePeriod))
                                                {
                                                    //dt = hana.GetData($@"SELECT ""UpdateDate"", ""BrokerId"" FROM ""OBRK"" Where ""SAPCardCode"" = '{txtUser.Value}'", hana.GetConnection("SAOHana"));
                                                    //string CreateDate = Convert.ToDateTime(DataAccess.GetData(dt, 0, "UpdateDate", "2000-01-01")).ToString("yyyy-MM-dd");
                                                    //DateTime UpdateDate = Convert.ToDateTime(DataAccess.GetData(dt, 0, "UpdateDate", CreateDate));
                                                    //if (UpdateDate > brokerRenewalDate)
                                                    //{

                                                    //EMAIL NOTIF FOR EXPIRY IF NOT SENT BEFORE
                                                    dt2 = hana.GetData($@"SELECT ""EmailAddress"", ""BrokerId"",""EmailNotifStage"" FROM ""OBRK"" Where ""SAPCardCode"" = '{txtUser.Value}'", hana.GetConnection("SAOHana"));
                                                    //if (DataAccess.GetData(dt2, 0, "EmailNotifStage", "").ToString() == "2")
                                                    //{
                                                    qry = $@"SELECT * FROM ""@EMAIL"" WHERE ""Code"" = 'BRKRNWLBLK'";
                                                    dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
                                                    ws.sendEmail(DataAccess.GetData(dt2, 0, "BrokerId", "").ToString(),
                                                                            DataAccess.GetData(dt, 0, "U_Subject", "").ToString(),
                                                                            DataAccess.GetData(dt2, 0, "EmailAddress", "").ToString(),
                                                                            DataAccess.GetData(dt, 0, "U_Body", "").ToString(),
                                                                            "",
                                                                            "",
                                                                            "Broker ID",
                                                                            DataAccess.GetData(dt, 0, "Code", "").ToString());
                                                    hana.Execute($@"UPDATE OBRK SET ""EmailNotifStage"" = '3', ""Status"" = 'RENEWAL' Where ""SAPCardCode"" = '{txtUser.Value}'", hana.GetConnection("SAOHana"));
                                                    //}

                                                    //ALERT MSG
                                                    NotiMsg($"Login failed: Renewal Date has passed. Please update information in Broker Page.", Noti.Error);

                                                    //}
                                                    //else
                                                    //{
                                                    Session["Company"] = company;
                                                    Session["UserID"] = UserID;
                                                    Session["BrokerId"] = DataAccess.GetData(dt, 0, "BrokerId", ""); 
                                                    redirectPage();
                                                    //}
                                                }





                                                else if (ApprovalDate < brokerRenewalDate.Date && (Math.Abs(brokerRenewalDate.Subtract(DateTime.Now).TotalDays) > MinimumDays) && Convert.ToDouble(DataAccess.GetData(dt2, 0, "EmailNotifStage", "0").ToString()) > 2)
                                                {
                                                    dt2 = hana.GetData($@"SELECT ""EmailAddress"", ""BrokerId"",""EmailNotifStage"" FROM ""OBRK"" Where ""SAPCardCode"" = '{txtUser.Value}'", hana.GetConnection("SAOHana"));
                                                    //SEND SECOND NOTIF IF NOT SENT BEFORE
                                                    //if (DataAccess.GetData(dt2, 0, "EmailNotifStage", "").ToString() == "1")
                                                    //{
                                                    qry = $@"SELECT * FROM ""@EMAIL"" WHERE ""Code"" = 'BRKRNWLSVN'";
                                                    dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
                                                    ws.sendEmail(DataAccess.GetData(dt2, 0, "BrokerId", "").ToString(),
                                                                            DataAccess.GetData(dt, 0, "U_Subject", "").ToString(),
                                                                            DataAccess.GetData(dt2, 0, "EmailAddress", "").ToString(),
                                                                            DataAccess.GetData(dt, 0, "U_Body", "").ToString(),
                                                                            "",
                                                                            "",
                                                                            "Broker ID",
                                                                            DataAccess.GetData(dt, 0, "Code", "").ToString());
                                                    hana.Execute($@"UPDATE OBRK SET ""EmailNotifStage"" = '2', ""Status"" = 'RENEWAL' Where ""SAPCardCode"" = '{txtUser.Value}'", hana.GetConnection("SAOHana"));
                                                    //}

                                                    Session["Company"] = company;
                                                    Session["UserID"] = UserID;

                                                    redirectPage();
                                                }




                                                //else if (brokerRenewalDateExpiry.Date < DateTime.Now.Date)
                                                else if (brokerRenewalDate.Date <= DateTime.Now.Date && (ApprovalDate < brokerRenewalDate))
                                                {
                                                    //SEND FIRST NOTIF IF NOT SENT BEFORE
                                                    dt2 = hana.GetData($@"SELECT ""EmailAddress"", ""BrokerId"",""EmailNotifStage"" FROM ""OBRK"" Where ""SAPCardCode"" = '{txtUser.Value}'", hana.GetConnection("SAOHana"));
                                                    //if (DataAccess.GetData(dt2, 0, "EmailNotifStage", "").ToString() == "" || DataAccess.GetData(dt2, 0, "EmailNotifStage", "").ToString() == "0")
                                                    //{
                                                    qry = $@"SELECT * FROM ""@EMAIL"" WHERE ""Code"" = 'BRKRNWL'";
                                                    dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
                                                    ws.sendEmail(DataAccess.GetData(dt2, 0, "BrokerId", "").ToString(),
                                                                            DataAccess.GetData(dt, 0, "U_Subject", "").ToString(),
                                                                            DataAccess.GetData(dt2, 0, "EmailAddress", "").ToString(),
                                                                            DataAccess.GetData(dt, 0, "U_Body", "").ToString(),
                                                                            "",
                                                                            "",
                                                                            "Broker ID",
                                                                            DataAccess.GetData(dt, 0, "Code", "").ToString());

                                                    hana.Execute($@"UPDATE OBRK SET ""EmailNotifStage"" = '1', ""Status"" = 'RENEWAL' Where ""SAPCardCode"" = '{txtUser.Value}'", hana.GetConnection("SAOHana"));
                                                    //}

                                                    Session["Company"] = company;
                                                    Session["UserID"] = UserID;

                                                    redirectPage();
                                                }
                                                else
                                                {
                                                    Session["Company"] = company;
                                                    Session["UserID"] = UserID;
                                                    
                                                    redirectPage();
                                                }
                                            }
                                            else
                                            {
                                                Session["Company"] = company;
                                                Session["UserID"] = UserID;

                                                redirectPage();
                                            }
                                        }
                                        else
                                        {
                                            Session["Company"] = company;
                                            Session["UserID"] = UserID;

                                            redirectPage();

                                        }
                                    }
                                    else { NotiMsg("User credentials does not have assigned roles yet. Please contact the system administrator", Noti.Error); }
                                }
                                else
                                {
                                    Session["Company"] = company;
                                    Session["UserID"] = UserID;

                                    redirectPage();
                                }
                                //}
                            }
                        }
                    }
                    else { NotiMsg("Please input the correct username/password.", Noti.Error); }
                }
                else { NotiMsg("Invalid username.", Noti.Error); }
            }
            catch (Exception ex)
            {
                NotiMsg($"Login failed: {ex.Message}", Noti.Error);
            }
        }


        void redirectPage()
        {
            //2024-07-08: CHANGE REDIRECT FROM DASHBOARD TO QUOTATION
            //Response.Redirect("~/pages/Dashboard.aspx");
            Response.Redirect("~/pages/Buyers.aspx");
        }

        void NotiMsg(string text, Noti options)
        {
            string oClass = "alert fade in alert-";
            string oTitle = "";
            switch (options)
            {
                case Noti.Success:
                    oClass += "success";
                    oTitle = "Success : ";
                    break;
                case Noti.Error:
                    oClass += "danger";
                    oTitle = "Error : ";
                    ErrorHandler.Log(Session["UserId"]?.ToString()
                    , "Login"
                    , "Medium"
                    , text
                    , "NotiMsg");
                    break;
            }
            lblHeader.InnerText = oTitle;
            lblMsg.InnerText = text;
            MsgNoti.Attributes.Add("class", oClass);
            MsgNoti.Visible = true;
        }
    }
}
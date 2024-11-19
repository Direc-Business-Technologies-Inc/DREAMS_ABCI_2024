using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABROWN_DREAMS.pages
{
    public partial class Licensing : System.Web.UI.Page
    {
        SAPHanaAccess hana = new SAPHanaAccess();
        DirecWebService ws = new DirecWebService();
        AssemblyInfo _info = new AssemblyInfo(Assembly.GetEntryAssembly());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                DataTable dtgeninfo = new DataTable();
                dtgeninfo.Columns.AddRange(new DataColumn[7]
                        {
                        new DataColumn("ProductName"),
                        new DataColumn("ProductVersion"),
                        new DataColumn("ComputerName"),
                        new DataColumn("HardwareKey"),
                        new DataColumn("CompanyName"),
                        new DataColumn("ContactPerson"),
                        new DataColumn("EmailAddress")
                        });
                ViewState["GenInfo"] = dtgeninfo;

                getGeneralInfo();

                visibleDocumentButtons(false, btnPreview, btnRemove);
            }
        }

        void getGeneralInfo()
        {
            var computerName = Dns.GetHostName();
            var hardwareKey = SystemSettings.getUniqueID("C");

            DataTable dtgeninfo = new DataTable();
            dtgeninfo = (DataTable)ViewState["GenInfo"];

            dtgeninfo.Rows.Add(
                _info.Product,
                _info.FileVersion,
                computerName,
                hardwareKey,
                _info.Company
                //"",
                //""
                );

            gvLicense.DataSource = dtgeninfo;
            gvLicense.DataBind();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {

            if (FileUpload.HasFile) //If the used Uploaded a file  
            {
                string code = Environment.MachineName;

                //Get FileName and Extension seperately
                string fileNameOnly = Path.GetFileNameWithoutExtension(FileUpload.FileName);
                string extension = Path.GetExtension(FileUpload.FileName);
                string uniqueCode = code;

                string FileName = fileNameOnly + "_" + code + extension; //Name of the file is stored in local Variable   
                lblFileName.Text = FileName;
                FileUpload.PostedFile.SaveAs(Server.MapPath("~/BUYER_AGENT/") + FileName); //File is saved in the Physical folder  

                visibleDocumentButtons(true, btnPreview, btnRemove);
            }
            if (FileUpload.HasFile) //If the used Uploaded a file  
            {
                string code = Environment.MachineName;

                //Get FileName and Extension seperately
                string fileNameOnly = Path.GetFileNameWithoutExtension(FileUpload.FileName);
                string extension = Path.GetExtension(FileUpload.FileName);
                string uniqueCode = code;

                string FileName = fileNameOnly + "_" + code + extension; //Name of the file is stored in local Variable   
                lblFileName.Text = FileName;
                FileUpload.PostedFile.SaveAs(Server.MapPath("~/BUYER_APPROVED/") + FileName); //File is saved in the Physical folder  

                visibleDocumentButtons(true, btnPreview, btnRemove);
            }
        }
        void DeleteTemporaryFIles()
        {
            string Filepath = Server.MapPath("~/TEMP_DOCS/");
            string code = $@"*{Environment.MachineName}*.*";
            string[] fileList = Directory.GetFiles(Filepath, code);
            foreach (string file in fileList)
            {
                System.Diagnostics.Debug.WriteLine(file + "will be deleted");
                File.Delete(file);
            }
        }
        protected void btnRemove_Click(object sender, EventArgs e)
        {
            if (File.Exists(Server.MapPath("~/BUYER_AGENT/") + lblFileName.Text))
            {
                // If file found, delete it    
                File.Delete(Server.MapPath("~/BUYER_AGENT/") + lblFileName.Text);
                lblFileName.Text = "";
                visibleDocumentButtons(false, btnPreview, btnRemove);
            }
            if (File.Exists(Server.MapPath("~/BUYER_APPROVED/") + lblFileName.Text))
            {
                // If file found, delete it    
                File.Delete(Server.MapPath("~/BUYER_APPROVED/") + lblFileName.Text);
                lblFileName.Text = "";
                visibleDocumentButtons(false, btnPreview, btnRemove);
            }
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            string Filepath = Server.MapPath("~/BUYER_AGENT/" + lblFileName.Text);
            //lblFileName.Text = Filepath;
            //System.Diagnostics.Process.Start(Filepath);
            var host = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.LocalPath, "");
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + host + "/BUYER_AGENT/" + lblFileName.Text + "');", true);

        }

        void visibleDocumentButtons(bool visible, LinkButton prev, LinkButton del)
        {
            prev.Visible = visible;
            del.Visible = visible;
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

        protected void btnRequestLicense_Click(object sender, EventArgs e)
        {
            try
            {
                TextBox txtContactPerson = gvLicense.Rows[0].FindControl("gvContactPerson") as TextBox;
                TextBox txtEmailAddress = gvLicense.Rows[0].FindControl("gvCompanyName") as TextBox;
                string fileLoc = Server.MapPath("~/LicenseRequest/");
                string productName = gvLicense.Rows[0].Cells[0].Text;
                string computerName = gvLicense.Rows[0].Cells[2].Text;
                string hardwareKey = gvLicense.Rows[0].Cells[3].Text;
                string companyName = gvLicense.Rows[0].Cells[4].Text;
                string contactPerson = txtContactPerson.Text;
                string emailAddress = txtEmailAddress.Text;
                string password = "";

                if (!Directory.Exists(fileLoc))
                    Directory.CreateDirectory(fileLoc);

                string file = $"{fileLoc}{computerName}" +
                    $"{DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "").Trim()}.ID";

                LicenseHelper.CreatePreRegistrationID(file,
                                                    computerName,
                                                    hardwareKey,
                                                    companyName,
                                                    contactPerson,
                                                    emailAddress,
                                                    password);

                //send thru email

                string qry = $@"SELECT * FROM ""@EMAIL"" WHERE ""Code"" = 'LCSNG'";
                DataTable dt = hana.GetData(qry, hana.GetConnection("SAPHana"));

                MailMessage mail = new MailMessage();
                //mail.To.Add("rndteam@direcbsi.com");
                mail.To.Add($"{ConfigurationManager.AppSettings["EmailTo"].ToString()}");
                mail.From = new MailAddress(emailAddress, contactPerson);
                mail.Subject = companyName + " : License Request for " + productName;
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.Body = ws.createEmailBody("", "", "", "", "", DataAccess.GetData(dt, 0, "U_Picture", "").ToString());
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;

                mail.Priority = MailPriority.High;
                SmtpClient client = new SmtpClient();
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(DataAccess.GetData(dt, 0, "U_Email", "").ToString(), DataAccess.GetData(dt, 0, "U_Password", "").ToString());
                client.Port = Convert.ToInt32(DataAccess.GetData(dt, 0, "U_Port", "0").ToString());
                client.Host = DataAccess.GetData(dt, 0, "U_Host", "").ToString();
                client.EnableSsl = true;

                client.Send(mail);

                alertMsg("Request Successful", "success");
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }
            
        }

        [WebMethod]
        public static object ReadLicense(string DateMM, string DateDD, string DateYYYY, string ComputerName, string HardwareKey, string ProductId, string NumberOfUsers)
        {
            var month = DataCipher.DataDecrypt(DateMM);
            var day = DataCipher.DataDecrypt(DateDD);
            var year = DataCipher.DataDecrypt(DateYYYY);
            var computerName = DataCipher.DataDecrypt(ComputerName);
            var hardwareKey = DataCipher.DataDecrypt(HardwareKey);
            var productId = DataCipher.DataDecrypt(ProductId);
            var noOfUsers = DataCipher.DataDecrypt(NumberOfUsers);
            //save in database

            //matching to
            var output = "Success";

            //if existing license get the number of users selected. Validate with noOfUsers if equal
            //get top noOfUsers
            //30 > 25 = retain tick ( prompt user discrepancy )

            return output;
        }
    }
}
using System;
using DataCipher;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABROWN_DREAMS
{
    public partial class Main : MasterPage
    {
        DirecWebService ws = new DirecWebService();
        SAPHanaAccess hana = new SAPHanaAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["BuyerLedger"] = 1;

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            DREAMSVersion.InnerText = version;



            if (!IsPostBack)
            {
                if (Session["UserID"] == null)
                { Response.Redirect("~/pages/Login.aspx"); }

                int UserID = (int)Session["UserId"];

                //GET SALES AGENT/BROKER NAME
                if (Session["UserName"].ToString().StartsWith("SP-"))
                {
                    DataTable dtUsername = null;
                    //if (Session["BrkPosition"].ToString() == "Sales Agent")
                    //{
                    //    dtUsername = hana.GetData($@"SELECT A.""SalesPerson""
                    //            FROM OSLA A
                    //             Inner join BRK1 B ON A.""Id"" = B.""Id""	
                    //            WHERE B.""SAPCardCode"" = '{Session["UserName"].ToString()}'", hana.GetConnection("SAOHana"));
                    //}
                    //else
                    //{
                    //    dtUsername = hana.GetData($@"SELECT LEFT(C.""FirstName"", 1) || '. ' || C.""LastName"" ""SalesPerson""
                    //            FROM OSLA A
                    //             Inner join BRK1 B ON A.""Id"" = B.""Id""
                    //             Inner join OBRK C ON B.""BrokerId"" = C.""BrokerId""
                    //            WHERE B.""SAPCardCode"" = '{Session["UserName"].ToString()}'", hana.GetConnection("SAOHana"));
                    //}
                    //string username = DataAccess.GetData(dtUsername, 0, "SalesPerson", "").ToString();
                    dtUsername = hana.GetData($@"SELECT ""CardName"" from ""OCRD""
                                WHERE ""CardCode"" = '{Session["UserName"].ToString()}'", hana.GetConnection("SAPHana"));
                    string username = DataAccess.GetData(dtUsername, 0, "CardName", "").ToString();
                    lblUsername.InnerText = username;
                }
                else
                {
                    lblUsername.InnerText = ws.WebProfile(UserID);
                }

                Session["UserFullName"] = lblUsername.InnerText;

                HeadVisible(false);

                if ((int)Session["UserID"] != 1)
                {
                    Session["ForwardedCode"] = ws.GetRoleByID(UserID);

                    //get sequence ID
                    Session["SeqId"] = ws.GetSeqId((string)Session["ForwardedCode"]);

                    DataTable dt = new DataTable();
                    dt = hana.GetDataDS($@"SELECT ""FeatEncrypt"" FROM ""USR1"" WHERE ""UserID"" = {UserID}", hana.GetConnection("SAOHana")).Tables[0];

                    foreach (DataRow dr in dt.Rows)
                    {
                        string FeatCode = Cryption.Decrypt(dr["FeatEncrypt"].ToString());
                        int cnt = FeatCode.Length - UserID.ToString().Length;
                        FeatCode = FeatCode.Substring(UserID.ToString().Length, cnt);
                        switch (FeatCode)
                        {
                            case "OPRJ":
                                aMasterData.Visible = true;
                                aProjMap.Visible = false;
                                break;
                            case "OCRD":
                                aMasterData.Visible = true;
                                aBuyersInfo.Visible = true;
                                break;
                            case "OQUT":
                                aSalesOrder.Visible = true;
                                aQuotation.Visible = true;
                                break;
                            case "OBRK":
                                aBrokerApproval.Visible = true;
                                break;
                            case "OQUS":
                                aSalesOrder.Visible = true;
                                aQuotationSummary.Visible = true;
                                break;
                            case "ODPI":
                                aPayments.Visible = true;
                                aPaymentCashier.Visible = true;
                                break;
                            case "ODCR":
                                aDocuments.Visible = true;
                                aDocRequirement.Visible = true;
                                break;
                            case "ODCS":
                                aDocuments.Visible = true;
                                aDocStatus.Visible = true;
                                break;
                            case "OAST":
                                aAMD.Visible = true;
                                aAssestment.Visible = true;
                                break;
                            case "ORST":
                                aAMD.Visible = true;
                                aRestructing.Visible = true;
                                aRestructuringHistory.Visible = true;
                                break;
                            case "OHCN":
                                aAMD.Visible = true;
                                aHouseCon.Visible = true;
                                break;
                            case "OADM":
                                aAdministration.Visible = true;
                                aAdmin.Visible = true;
                                break;
                            case "ODRQ":
                                aAdministration.Visible = true;
                                break;
                            case "OUSR":
                                aAdministration.Visible = true;
                                aUserManage.Visible = true;
                                break;
                            case "OCLN":
                                aAdministration.Visible = true;
                                aCleanup.Visible = true;
                                aChangePassword.Visible = true;
                                break;
                            case "ORPT":
                                aReport.Visible = true;
                                aReportList.Visible = true;
                                break;
                            case "OFRF":
                                aForfeiture.Visible = true;
                                aForfeit.Visible = true;
                                break;
                            case "OPRJ1":
                                aMasterData.Visible = true;
                                aProjectPerLot.Visible = true;
                                break;
                            case "RST1":
                                aAMD.Visible = true;
                                aRestructuringHistory.Visible = true;
                                break;
                            case "OINC":
                                aAMD.Visible = true;
                                aIncentive.Visible = true;
                                break;
                            case "OCMS":
                                aAMD.Visible = true;
                                aCommission.Visible = true;
                                break;
                        }
                    }


                    DataTable dtUser;
                    dtUser = hana.GetData($@"SELECT IFNULL(""Username"",'') ""Username"" FROM ""OUSR"" WHERE ""ID"" = '{Session["UserID"].ToString()}'", hana.GetConnection("SAOHana"));
                    string username = DataAccess.GetData(dtUser, 0, "Username", "").ToString();
                    if (username.StartsWith("SP-"))
                    {
                        btnShowUsers.Disabled = true;
                        aChangePassword.Visible = true;
                        txtUserName.Value = username;
                    }
                    else
                    {
                        divTxtUser.Visible = true;
                    }

                }
                else
                { Session["ForwardedCode"] = ws.GetDocStatusName(1); HeadVisible(true); }


            }
        }

        void HeadVisible(bool val)
        {
            aMasterData.Visible = val;
            //aProjMap.Visible = val;
            aBuyersInfo.Visible = val;

            aSalesOrder.Visible = val;
            aQuotation.Visible = val;
            aQuotationSummary.Visible = val;
            aBrokerApproval.Visible = val;
            aChangePassword.Visible = val;

            aPayments.Visible = val;
            aPaymentCashier.Visible = val;

            aAMD.Visible = val;
            aAssestment.Visible = val;
            aRestructing.Visible = val;

            aDocuments.Visible = val;
            aDocRequirement.Visible = val;
            aDocStatus.Visible = val;

            aForfeiture.Visible = val;

            aAdministration.Visible = val;
            aAdmin.Visible = val;
            aEmail.Visible = val;
            aApproval.Visible = val;
            aUserManage.Visible = val;
            aCleanup.Visible = val;

            aReport.Visible = val;
            aReportList.Visible = val;
        }

        protected void btnYes_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("~/pages/Login.aspx");
        }

        protected void aBuyerLedger_Click(object sender, EventArgs e)
        {
            DataTable dt = hana.GetData(@"SELECT ""Name"",""RptName"",""RptPath"" FROM ""ORPT"" Where ""RptGroup"" = 'BL'", hana.GetConnection("SAOHana"));
            if (dt.Rows.Count > 0)
            {
                Session["ReportType"] = "BL";
                //Session["BPCode"] = lblID.Text;
                //Session["BlockLot"] = $"B{txtBlock.Text} L{txtLot.Text}";
                Session["ReportName"] = dt.Rows[0][1].ToString();
                Session["ReportPath"] = dt.Rows[0][2].ToString();

                //open new tab
                Response.Redirect("~/pages/ReportViewer.aspx");
            }
        }

        protected void btnUpdate_ServerClick(object sender, EventArgs e)
        {
            string password = Cryption.Encrypt($"{txtUserName.Value}{txtPassword.Text}");
            if (ws.UpdatePassword(txtUserName.Value, password))
            {
                alertMsg($@"Password for the user ""{txtUserName.Value}"" has been updated successfully.", "success");
            }
            else
            {
                //failed
                alertMsg("Error in adding document", "error");
            }
        }



        void alertMsg(string Message, string type)
        {
            lblMessageAlert1.Text = Message;
            if (type == "success")
                alertIcon1.ImageUrl = "~/assets/img/success.png";
            else if (type == "warning")
                alertIcon1.ImageUrl = "~/assets/img/warning.png";
            else if (type == "error")
                alertIcon1.ImageUrl = "~/assets/img/error.png";
            else if (type == "info")
                alertIcon1.ImageUrl = "~/assets/img/info.png";

            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "showAlert1();", true);
        }

        protected void btnOKUpdatePassword_ServerClick(object sender, EventArgs e)
        {


            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "hideAlert1();", true);
        }

        protected void btnShowUsers_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string qry = @"SELECT ""Username"" FROM OUSR WHERE ""IsLock"" = 'false'";

                gvUsers.DataSource = hana.GetData(qry, hana.GetConnection("SAOHana"));
                gvUsers.DataBind();

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowPopup", "ShowPopup();", true);
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }
        }

        protected void gvUsers_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            string qry = @"SELECT ""ID"", ""Username"" FROM OUSR WHERE ""IsLock"" = 'false'";

            gvUsers.DataSource = hana.GetData(qry, hana.GetConnection("SAOHana"));
            gvUsers.DataBind();
            gvUsers.PageIndex = e.NewPageIndex;
        }

        //protected void gvUsers_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName == "Sel")
        //    {
        //        string username = gvUsers.Rows[row].Cells[0].Text;
        //        txtUserName.Value = username;
        //    }
        //}

        protected void btnSelectUser_Click(object sender, EventArgs e)
        {
            LinkButton GetName = (LinkButton)sender;
            string UserName = (GetName.CommandArgument).ToString();
            txtUserName.Value = UserName;
            ScriptManager.RegisterStartupScript(this, GetType(), "HideUsers", "HideUsers();", true);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABROWN_DREAMS
{
    public partial class Index : System.Web.UI.MasterPage
    {
        DirecWebService ws = new DirecWebService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            { Response.Redirect("~/pages/Login.aspx"); }
            else
            {
                if (!IsPostBack)
                {
                    int UserID = (int)Session["UserId"];
                    lblUsername.InnerText = ws.WebProfile(UserID);

                    HeadVisible(false);

                    if ((int)Session["UserID"] != 1)
                    {
                        Session["ForwardedCode"] = ws.GetRoleByID(UserID);

                        //get sequence ID
                        Session["SeqId"] = ws.GetSeqId((string)Session["ForwardedCode"]);

                        //DataTable dt = new DataTable();
                        //dt = ws.Select($"SELECT FeatEncrypt FROM USR1 WHERE UserID = {UserID}", "USR1", "Addon").Tables["USR1"];

                        //foreach (DataRow dr in dt.Rows)
                        //{
                        //    string FeatCode = Cryption.Decrypt(dr["FeatEncrypt"].ToString());
                        //    int cnt = FeatCode.Length - UserID.ToString().Length;
                        //    FeatCode = FeatCode.Substring(UserID.ToString().Length, cnt);
                        //    switch (FeatCode)
                        //    {
                        //        case "OPRJ":
                        //            aMasterData.Visible = true;
                        //            aProjMap.Visible = true;
                        //            break;
                        //        case "OCRD":
                        //            aMasterData.Visible = true;
                        //            aBuyersInfo.Visible = true;
                        //            break;
                        //        case "OQUT":
                        //            aSalesOrder.Visible = true;
                        //            aQuotation.Visible = true;
                        //            break;
                        //        case "RQUT":
                        //            aSalesOrder.Visible = true;
                        //            //aSalesRestructuring.Visible = true;
                        //            break;
                        //        case "ORSV":
                        //            aPayments.Visible = true;
                        //            //aReservePayment.Visible = true;
                        //            break;
                        //        case "ODPI":
                        //            aPayments.Visible = true;
                        //            //aDownpayment.Visible = true;
                        //            break;
                        //        case "ORCP":
                        //            aPayments.Visible = true;
                        //            //aLoanablePayment.Visible = true;
                        //            break;
                        //        case "RDPI":
                        //            aPayments.Visible = true;
                        //            break;
                        //        case "ODOC":
                        //            aDocuments.Visible = true;
                        //            aDocReqirement.Visible = true;
                        //            break;
                        //        case "OLST":
                        //            aAdministration.Visible = true;
                        //            aAdmin.Visible = true;
                        //            break;
                        //        case "ODCS":
                        //            aAdministration.Visible = true;
                        //            aBuyersDocReq.Visible = true;
                        //            break;
                        //        case "OUSR":
                        //            aAdministration.Visible = true;
                        //            aUserManage.Visible = true;
                        //            break;
                        //    }
                        //}
                    }
                    else
                    { Session["ForwardedCode"] = ws.GetDocStatusName(1); HeadVisible(true); }
                }
            }
        }
        void HeadVisible(bool val)
        {
            //aMasterData.Visible = val;
            //aProjMap.Visible = val;
            //aBuyersInfo.Visible = val;

            //aSalesOrder.Visible = val;
            //aQuotation.Visible = val;
            ////aSalesRestructuring.Visible = val;

            //aPayments.Visible = val;
            ////aReservePayment.Visible = val;
            ////aDownpayment.Visible = val;
            ////aLoanablePayment.Visible = val;

            //aDocuments.Visible = val;
            //aDocReqirement.Visible = val;
            //aDocStatus.Visible = val;

            //aAdministration.Visible = val;
            //aAdmin.Visible = val;
            //aEmail.Visible = val;
            //aApproval.Visible = val;
            //aBuyersDocReq.Visible = val;
            //aUserManage.Visible = val;
            //aCleanup.Visible = val;

            //aReport.Visible = val;
            //aReportList.Visible = val;
        }
        protected void btnYes_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("~/pages/Login.aspx");
        }
    }
}
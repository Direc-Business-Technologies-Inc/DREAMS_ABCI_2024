using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABROWN_DREAMS.pages
{
    public partial class Incentives : Page
    {
        SAPHanaAccess hana = new SAPHanaAccess();
        DirecService wcf = new DirecService();
        DirecWebService ws = new DirecWebService();


        protected void Page_Load(object sender, EventArgs e)
        {
            Page.MaintainScrollPositionOnPostBack = true;

            if (!this.IsPostBack)
            {
                loaddata();

                //2023-11-16 : RESET DOCDATE
                txtDocDate.Text = null;
            }
            if (Session["UserID"] == null)
            {
                alertMsg("Session expired!", "error");
                Response.Redirect("~/pages/Login.aspx");
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

            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "ShowAlert();", true);
        }
        void loaddata()
        {
            string oSAPDB = ConfigurationManager.AppSettings["HANADatabase"];
            //DataTable dt = hana.GetData($@" SELECT DISTINCT * FROM ( SELECT T0.""DocEntry"", T0.""DocNum"" , T0.""AcctType""
            //                                 , A1.""Position"" as ""Position"", A1.""SAPCardCode"", A1.""SalesPerson"" , S1.""U_Release"" as ""Release"", S1.""U_Amount"" as ""Amount""
            //                                 , T0.""ProjCode"", T0.""Block"", T0.""Lot""                                             
            //                                 , T0.""CardCode""
            //                                 , C1.""FirstName"" || ' ' || C1.""LastName"" as ""CardName"" 
            //                                 , 'Incentives' as ""TransType""
            //                                 , IFNULL(T0.""Incentive"",'N') ""Incentive""
            //                                FROM OQUT T0 INNER JOIN CRD1 C1 ON T0.""CardCode"" = C1.""CardCode"" AND C1.""CardType"" = 'Buyer'
            //                                /**--RESERV AND DP BREAKDOWN--**/  INNER JOIN QUT1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" AND T1.""PaymentType"" = 'RES'
            //                                /**--AGENT TAG--**/                INNER JOIN QUT5 T5 ON T0.""DocEntry"" = T5.""DocEntry""
            //                                --/**--AGENT DATA--**/             INNER JOIN OSLA A1 ON T5.""EmpCode"" = CAST(A1.""TransID"" AS NVARCHAR(30))
            //                                --/**--BROKER TAG--**/             LEFT JOIN BRK1 BR ON A1.""Id"" = BR.""Id""
            //                                --/**--BROKER INFO--**/			   LEFT JOIN OBRK BK ON BR.""BrokerId"" = BK.""BrokerId""

            //                                                                INNER JOIN (
            //                                                                    select CAST(A.""TransID"" AS NVARCHAR(30)) as ""TransID"", 'Sales Agent' as ""Position"", A.""Id"", A.""SalesPerson"", B.""SAPCardCode"" , A.""VATCode"", A.""WTaxCode"" , C.""Status""
            //                                                                    FROM OSLA A inner join BRK1 B on A.""Id"" = B.""Id"" inner join OBRK C on B.""BrokerId"" = C.""BrokerId""
            //                                                                    union all
            //                                                                    select CAST(A.""TransID"" AS NVARCHAR(30)) as ""TransID"", 'Broker' as ""Position"", A.""Id"", (CASE WHEN C.""TypeOfBusiness"" <> 'SOLE PROPRIETOR' THEN C.""Partnership"" ELSE C.""FirstName"" || ' ' || C.""MiddleName"" || ' ' || C.""LastName"" END) , C.""SAPCardCode"", J.""VATCode"", J.""WTaxCode"", C.""Status""
            //                                                                    FROM OSLA A inner join BRK1 B on A.""Id"" = B.""Id"" inner join OBRK C on B.""BrokerId"" = C.""BrokerId"" inner join BRK1 K on C.""BrokerId"" = K.""BrokerId"" AND B.""Id"" = K.""Id"" inner join OSLA J ON K.""Id"" = J.""Id""
            //                                                                    ) AS A1 ON T5.""EmpCode"" = CAST(A1.""Id"" AS NVARCHAR(30)) AND A1.""Status"" <> 'REJECTED' 
            //                                /**--SAP INCENTIVE--**/            LEFT JOIN ""{oSAPDB}"".""@INCENTIVE"" S1 ON UPPER(T0.""AcctType"") = UPPER(S1.""Code"") AND A1.""Position"" = S1.""U_Position""

            //                                WHERE
            //                                ----CONDITION IF DP NOT EXIST THEN DO NOT SHOW FROM THE LIST----
            //                                    S1.""U_Release"" <>
            //                                           CASE WHEN IFNULL((SELECT MAX(D1.""DocEntry"") FROM QUT1 D1 WHERE D1.""DocEntry"" = T0.""DocEntry"" AND D1.""LineStatus""='C' AND D1.""Terms"" = '1' AND D1.""PaymentType"" = 'DP'), 0) = 0
            //                                           THEN '2' ELSE '0' END
            //                                ----CONDITION TO GET RESERVATION FULL PAYMENT----
            //                                    AND (T1.""PaymentAmount"" - T1.""AmountPaid"") = 0
            //                                    AND IFNULL(A1.""SAPCardCode"",'') <> ''
            //                                ----CONDITION EXCLUDE POSTED-----
            //                                    ) AS T0 LEFT JOIN OPCH P0 ON T0.""DocEntry"" = P0.""DocEntry"" AND T0.""Release"" = P0.""Release"" AND T0.""Position"" = P0.""Position"" AND T0.""TransType"" = P0.""TransType"" 
            //                                    WHERE IFNULL(P0.""SAPDocEntry"",'')='' AND T0.""Incentive"" = 'Y'                                                             
            //                                    ORDER BY T0.""DocEntry"" ASC, T0.""Release"" ;                                                                                                                                               
            //", hana.GetConnection("SAOHana"));
            DataTable dt = ws.GetIncentive(oSAPDB).Tables["GetIncentive"];
            ViewState["Incentive"] = dt;
            gvDocList.DataSource = dt;
            gvDocList.DataBind();
        }

        protected void btnUploadIncent_Click(object sender, EventArgs e)
        {
            try
            {
                string errMsg = "";
                if (!string.IsNullOrWhiteSpace(txtDocDate.Text))
                {


                    int nothingChecked = 1;

                    foreach (GridViewRow row in gvDocList.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chkRow = (row.Cells[0].FindControl("cbRow") as CheckBox);
                            if (chkRow.Checked)
                            {
                                nothingChecked = 0;

                                string oDocEntry = row.Cells[1].Text;
                                string oPosition = row.Cells[4].Text;
                                string oAgent = row.Cells[6].Text;
                                string oRelease = row.Cells[7].Text;
                                string oCardCode = row.Cells[5].Text;

                                if (!String.IsNullOrEmpty(oDocEntry))
                                {
                                    //if (ddlReplenishment.Text == "Cash")
                                    //{
                                    //    oCardCode = ConfigSettings.ReplenishmentBP"].ToString();
                                    //}

                                    //// --QryGroup2-- FOR INCENTIVES
                                    string _rest = wcf.PostAP("Incentives", oDocEntry, oPosition, oRelease, oCardCode, ConfigSettings.IncentiveProperty, ddlReplenishment.Text, txtDocDate.Text);
                                    if (_rest.Contains("error"))
                                    {
                                        ////IF ERROR: CREATE FUNCTION 
                                        errMsg = $@"Error on posting for Document: {oDocEntry}  Agent: {oAgent}  Release: {oRelease} -- ({_rest})";
                                    }
                                    else
                                    {
                                        ////2023-11-16 : RESET DOCDATE
                                        //txtDocDate.Text = null;
                                    }
                                }
                            }
                        }
                    }

                    if (nothingChecked == 0)
                    {
                        if (errMsg.Contains("error"))
                        {
                            ////IF ERROR: CREATE FUNCTION
                            alertMsg(errMsg, "info");
                        }
                        else
                        {
                            alertMsg($@"Transaction posted successfully!", "success");
                            loaddata();
                        }

                    }
                    else
                    {
                        alertMsg($@"No checked transaction. Please try again", "warning");
                    }

                }
                else
                {
                    alertMsg($@"Please provide Upload Date.", "info");
                }
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "info");
            }
        }

        protected void btnDocSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txDocSearch.Value))
            {
                string oSAPDB = ConfigurationManager.AppSettings["HANADatabase"];
                //    DataTable dt = hana.GetData($@" SELECT DISTINCT * FROM ( SELECT T0.""DocEntry"", T0.""DocNum"" , T0.""AcctType""
                //                                 , A1.""Position"" as ""Position"", A1.""SAPCardCode"", A1.""SalesPerson"" , S1.""U_Release"" as ""Release"", S1.""U_Amount"" as ""Amount""
                //                                 , T0.""ProjCode"", T0.""Block"", T0.""Lot""                                             
                //                                 , T0.""CardCode""
                //                                 , C1.""FirstName"" || ' ' || C1.""LastName"" as ""CardName"" 
                //                                 , 'Incentives' as ""TransType""
                //                                FROM OQUT T0 INNER JOIN CRD1 C1 ON T0.""CardCode"" = C1.""CardCode"" AND C1.""CardType"" = 'Buyer'
                //                                /**--RESERV AND DP BREAKDOWN--**/  INNER JOIN QUT1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" AND T1.""PaymentType"" = 'RES'
                //                                /**--AGENT TAG--**/                INNER JOIN QUT5 T5 ON T0.""DocEntry"" = T5.""DocEntry""
                //                                --/**--AGENT DATA--**/             INNER JOIN OSLA A1 ON T5.""EmpCode"" = CAST(A1.""TransID"" AS NVARCHAR(30))
                //                                --/**--BROKER TAG--**/             LEFT JOIN BRK1 BR ON A1.""Id"" = BR.""Id""
                //                                --/**--BROKER INFO--**/			   LEFT JOIN OBRK BK ON BR.""BrokerId"" = BK.""BrokerId""

                //                                                                INNER JOIN (
                //                                                                    select CAST(A.""TransID"" AS NVARCHAR(30)) as ""TransID"", 'Sales Agent' as ""Position"", A.""Id"", A.""SalesPerson"", B.""SAPCardCode"" , A.""VATCode"", A.""WTaxCode"" , C.""Status""
                //                                                                    FROM OSLA A inner join BRK1 B on A.""Id"" = B.""Id"" inner join OBRK C on B.""BrokerId"" = C.""BrokerId""
                //                                                                    union all
                //                                                                    select CAST(A.""TransID"" AS NVARCHAR(30)) as ""TransID"", 'Broker' as ""Position"", A.""Id"", (CASE WHEN C.""TypeOfBusiness"" <> 'SOLE PROPRIETOR' THEN C.""Partnership"" ELSE C.""FirstName"" || ' ' || C.""MiddleName"" || ' ' || C.""LastName"" END) , C.""SAPCardCode"", J.""VATCode"", J.""WTaxCode"", C.""Status""
                //                                                                    FROM OSLA A inner join BRK1 B on A.""Id"" = B.""Id"" inner join OBRK C on B.""BrokerId"" = C.""BrokerId"" inner join BRK1 K on C.""BrokerId"" = K.""BrokerId"" AND B.""Id"" = K.""Id"" inner join OSLA J ON K.""Id"" = J.""Id""
                //                                                                    ) AS A1 ON T5.""EmpCode"" = CAST(A1.""Id"" AS NVARCHAR(30)) AND A1.""Status"" <> 'REJECTED' 
                //                                /**--SAP INCENTIVE--**/            LEFT JOIN ""{oSAPDB}"".""@INCENTIVE"" S1 ON UPPER(T0.""AcctType"") = UPPER(S1.""Code"") AND A1.""Position"" = S1.""U_Position""

                //                                WHERE
                //                                ----CONDITION IF DP NOT EXIST THEN DO NOT SHOW FROM THE LIST----
                //                                    S1.""U_Release"" <>
                //                                           CASE WHEN IFNULL((SELECT MAX(D1.""DocEntry"") FROM QUT1 D1 WHERE D1.""DocEntry"" = T0.""DocEntry"" AND D1.""LineStatus""='C' AND D1.""Terms"" = '1' AND D1.""PaymentType"" = 'DP'), 0) = 0
                //                                           THEN '2' ELSE '0' END
                //                                ----CONDITION TO GET RESERVATION FULL PAYMENT----
                //                                    AND (T1.""PaymentAmount"" - T1.""AmountPaid"") = 0
                //                                    AND IFNULL(A1.""SAPCardCode"",'') <> ''
                //                                ----NAME FILTER----
                //                                    AND (UPPER(A1.""SalesPerson"")LIKE UPPER('%{txDocSearch.Value}%') OR UPPER(C1.""FirstName"") LIKE UPPER('%{txDocSearch.Value}%') OR UPPER(C1.""LastName"") LIKE UPPER('%{txDocSearch.Value}%') OR UPPER(T0.""CardCode"") LIKE UPPER('%{txDocSearch.Value}%'))
                //                                ----CONDITION EXCLUDE POSTED-----
                //                                    ) AS T0 LEFT JOIN OPCH P0 ON T0.""DocEntry"" = P0.""DocEntry"" AND T0.""Release"" = P0.""Release"" AND T0.""Position"" = P0.""Position"" AND T0.""TransType"" = P0.""TransType"" WHERE IFNULL(P0.""SAPDocEntry"",'')=''
                //                                    ORDER BY T0.""DocEntry"" ASC, T0.""Release"" ;                                                                                                                                               
                //", hana.GetConnection("SAOHana"));
                DataTable dt = ws.GetIncentiveSearch(oSAPDB, txDocSearch.Value).Tables["GetIncentiveSearch"];
                gvDocList.DataSource = dt;
                gvDocList.DataBind();
            }
            else
            { loaddata(); }
        }
    }
}
using ABROWN_DREAMS.Services;
using DirecLayer;
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
    public partial class Commission : System.Web.UI.Page
    {
        SAPHanaAccess hana = new SAPHanaAccess();
        DirecService wcf = new DirecService();
        DirecWebService ws = new DirecWebService();

        private string oSAPDB = ConfigurationManager.AppSettings["HANADatabase"];


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
            try
            {
                //DataTable dt = hana.GetData($@"SELECT DISTINCT  * FROM (  
                //                                SELECT TOP 10 T0.""DocEntry"", T0.""DocNum"" , T0.""AcctType""
                //                                     , B2.""PositionSharedDetails"" as ""Position""
                //                                     , B2.""SalesPersonNameSharedDetails"" as ""SharedPerson""
                //                                     , S1.""U_Release"" as ""Release""    
                //                                     , C1.""FirstName"" || ' ' || C1.""LastName"" as ""CardName"" 
                //                                     , T0.""ProjCode"", T0.""Block"", T0.""Lot"", T0.""Model""
                //                                     , B2.""OslaID"" as ""Id""
                //                                     , S2.""CardCode"" as ""SAPCardCode""
                //                                     , T0.""NetTcp""
                //                                     , T0.""CardCode""
                //                                     , T0.""CommissionRemark""
                //                                     , T1.""PaidAmnt""
                //                                     , 'Commission' as ""TransType""
                //                                     , S1.""U_CollectedTCP""
                //                                     , S1.""U_CommissionRelease""
                //                                     , (T1.""PaidAmnt"" / T0.""NetTcp"") * 100 as ""CollectedPerct""                                     
                //                                     , case when(T1.""PaidAmnt"" / T0.""Das"") * 100 >= S1.""U_CollectedTCP"" Then((S1.""U_CommissionRelease"" / 100) * (T0.""Das""/1.12)) *(B2.""PercentageSharedDetails"" / 100) else 0 End as ""Amount""
                //                                FROM 
                //                                    OQUT T0 INNER JOIN CRD1 C1 ON T0.""CardCode"" = C1.""CardCode"" AND C1.""CardType"" = 'Buyer'
                //                            /**--RESERV AND DP BREAKDOWN--**/  INNER JOIN(SELECT ""DocEntry"", SUM(""AmountPaid"") as ""PaidAmnt"" FROM QUT1 WHERE ""PaymentType"" <> 'MISC' group by ""DocEntry"") AS T1 ON T0.""DocEntry"" = T1.""DocEntry""
                //                            /**--AGENT TAG--**/                INNER JOIN QUT5 T5 ON T0.""DocEntry"" = T5.""DocEntry""
                //                            /**--AGENT DATA--**/               INNER JOIN OSLA A1 ON T5.""EmpCode"" = CAST(A1.""TransID"" AS NVARCHAR(30))
                //                            /**--BROKER TAG--**/               INNER JOIN BRK1 BR ON A1.""Id"" = BR.""Id""
                //                            /**--SHARING INFO--**/			   INNER JOIN BRK2 B2 ON BR.""Id"" = B2.""SalesPersonId""

                //                            /**--SAP SCHEME--**/            INNER JOIN ""{oSAPDB}"".""@COMMISSION"" S1 ON T0.""AcctType"" = S1.""Code""
                //                            /**--SAP BP--**/                INNER JOIN ""{oSAPDB}"".""OCRD"" S2 ON B2.""OslaID"" = S2.""U_SalesAgentCode""
                //                            WHERE case when(T1.""PaidAmnt"" / T0.""NetTcp"") * 100 >= S1.""U_CollectedTCP"" Then((S1.""U_CommissionRelease"" / 100) * T0.""NetTcp"") * (B2.""PercentageSharedDetails"" / 100) else 0 End <> 0
                //                            ) AS T0 LEFT JOIN OPCH P0 ON T0.""DocEntry"" = P0.""DocEntry"" AND T0.""Release"" = P0.""Release"" AND T0.""Position"" = P0.""Position""  AND T0.""TransType"" = P0.""TransType"" WHERE IFNULL(P0.""SAPDocEntry"",'')='' 
                //                            ORDER BY T0.""DocEntry"" ASC, T0.""Release"" ;
                //", hana.GetConnection("SAOHana"));


                DataTable dt = ws.GetCommission(oSAPDB).Tables["GetCommission"];
                ViewState["Commission"] = dt;
                gvDocList.DataSource = dt;
                gvDocList.DataBind();
                loadRemarks();

            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "warning");
            }
        }


        void loadDataSearch(string Search)
        {
            try
            {
                DataTable dt = ws.GetCommissionSearch(oSAPDB, Search).Tables["GetCommissionSearch"];
                gvDocList.DataSource = dt;
                gvDocList.DataBind();
                loadRemarks();

            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "warning");
            }
        }


        void loadRemarks()
        {
            foreach (GridViewRow row in gvDocList.Rows)
            {
                string oDocEntry = row.Cells[1].Text;


                if (!String.IsNullOrEmpty(oDocEntry))
                {

                    if (oDocEntry == "1413")
                    {
                        string test = "";
                    }

                    TextBox txtCommissionRemarks = (row.Cells[17].FindControl("txtCommissionRemarks") as TextBox);

                    //string qry = $@"SELECT A.""DocEntry"",A.""U_DocName"", C.""LoanType"", D.""BusinessType"", C.""Bank"", C.""CardCode"" FROM
                    //            ""{oSAPDB}"".""@BUYERDOCREQR"" A INNER JOIN
                    //            ""{oSAPDB}"".""@BUYERDOCREQH"" B	ON A.""DocEntry"" = B.""DocEntry"" LEFT JOIN
                    //            OQUT C ON C.""LoanType"" = B.""U_LoanType"" AND C.""Bank"" = B.""U_BankCode"" INNER JOIN
                    //            OCRD D ON D.""CardCode"" = C.""CardCode"" AND CASE WHEN D.""BusinessType"" NOT IN ('Individual','Corporation') THEN 'Individual' END = B.""U_BusinessType"" LEFT JOIN
                    //            CRD1 E ON E.""CardCode"" = D.""CardCode"" AND E.""CardType"" = 'Buyer' INNER JOIN
                    //            QDOC F ON F.""DocEntry"" = C.""DocEntry"" AND F.""DocId"" = A.""LineId""
                    //            WHERE C.""DocEntry"" = {oDocEntry} AND IFNULL(F.""DocName"",'') <> '' AND IFNULL(A.""U_PaymentPhase"", 1) = 1";

                    //string qry = $@"select A.""DocEntry"",A.""CardCode"",A.""LoanType"",CASE WHEN IFNULL(A.""Bank"",'') <> 'BANK' THEN A.""LoanType"" ELSE IFNULL(A.""Bank"",'') END ""Bank"",	B.""BusinessType"" FROM
                    //                OQUT A INNER JOIN OCRD B ON A.""CardCode"" = B.""CardCode"" INNER JOIN
                    //                 CRD1 C ON B.""CardCode"" = C.""CardCode"" AND C.""CardType"" = 'Buyer' LEFT JOIN
                    //                 QDOC D ON D.""DocEntry"" = A.""DocEntry"" AND IFNULL(D.""DocName"",'') <> '' INNER JOIN 
                    //                 ""{oSAPDB}"".""@BUYERDOCREQH"" E ON A.""LoanType"" = E.""U_LoanType"" AND (CASE WHEN IFNULL(A.""Bank"",'') <> 'BANK' THEN A.""LoanType"" ELSE IFNULL(A.""Bank"",'') END) = IFNULL(E.""U_BankCode"",'')INNER JOIN

                    //                    ""{oSAPDB}"".""@BUYERDOCREQR"" F ON E.""DocEntry"" = F.""DocEntry"" AND D.""DocId"" = F.""LineId""
                    //                WHERE
                    //                 A.""DocEntry"" = {oDocEntry} AND IFNULL(D.""DocName"",'') <> '' AND IFNULL(F.""U_PaymentPhase"", 1) = 1";


                    //DataTable Reqdt = hana.GetData(qry, hana.GetConnection("SAOHana"));
                    DataTable Reqdt = ws.LoadDocumentRemarks(oSAPDB, oDocEntry).Tables["LoadDocumentRemarks"];


                    ViewState["RemarksDT"] = Reqdt;


                    if (Reqdt.Rows.Count > 0)
                    {

                        string LoanType = Reqdt.Rows[0]["LoanType"].ToString();
                        string oBusinessType = Reqdt.Rows[0]["BusinessType"].ToString();
                        oBusinessType = oBusinessType.ToLower() != "corporation" ? "Individual" : oBusinessType;

                        string Bank = Reqdt.Rows[0]["Bank"].ToString();


                        if (Bank.Contains("nbsp") || string.IsNullOrWhiteSpace(Bank))
                        {
                            if (LoanType.ToUpper() == "INHOUSE")
                            {
                                Bank = "INHOUSE";
                            }
                            else
                            {
                                if (LoanType.ToUpper() == "HDMF")
                                {
                                    Bank = "HDMF";
                                }
                                if (LoanType.ToUpper() == "SPOTCASH")
                                {
                                    Bank = "SPOTCASH";
                                }
                                if (LoanType.ToUpper() == "N/A")
                                {
                                    Bank = "N/A";
                                }
                            }
                        }
                        else
                        {
                            if (LoanType.ToUpper() == "HDMF")
                            {
                                Bank = "HDMF";
                            }
                            if (LoanType.ToUpper() == "SPOTCASH")
                            {
                                Bank = "SPOTCASH";
                            }
                            if (LoanType.ToUpper() == "N/A")
                            {
                                Bank = "N/A";
                            }
                        }



                        string tqry = $@"SELECT A.""DocEntry""   FROM ""@BUYERDOCREQH"" A INNER JOIN ""@BUYERDOCREQR"" B ON A.""DocEntry"" = B.""DocEntry"" 
                                            WHERE A.""U_LoanType"" = '{LoanType}' AND A.""U_BankCode"" = '{Bank}' AND A.""U_BusinessType"" = '{oBusinessType}' 
                                            AND IFNULL(B.""U_PaymentPhase"", 1) = 1;";

                        DataTable dtDocs = hana.GetData($@"{tqry}", hana.GetConnection("SAPHana"));



                        //if (Reqdt.Rows[0]["CardCode"].ToString() == "BP0000000157")
                        //{
                        //    string test = "0";
                        //}

                        if (Reqdt.Rows.Count >= dtDocs.Rows.Count)
                        {
                            txtCommissionRemarks.Text = "Complete Document Requirements";
                        }
                        else
                        {
                            txtCommissionRemarks.Text = "Incomplete Document Requirements";
                            row.BackColor = System.Drawing.ColorTranslator.FromHtml("#c4c2c2");
                        }

                    }
                    else
                    {
                        txtCommissionRemarks.Text = "No Document Requirements";
                        row.BackColor = System.Drawing.ColorTranslator.FromHtml("#808080");
                    }
                }
            }
        }


        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                string oSAPDB = ConfigurationManager.AppSettings["HANADatabase"];

                if (!string.IsNullOrWhiteSpace(txtDocDate.Text))
                {



                    foreach (GridViewRow row in gvDocList.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chkRow = (row.Cells[0].FindControl("cbRow") as CheckBox);
                            if (chkRow.Checked)
                            {


                                string oDocEntry = row.Cells[1].Text;
                                string oPosition = row.Cells[4].Text;
                                string oAgent = row.Cells[5].Text;
                                string oRelease = row.Cells[6].Text;


                                //FOR JOURNAL ENTRY
                                string oDocNum = row.Cells[2].Text;
                                string oProject = row.Cells[10].Text;
                                string oCardCode = row.Cells[15].Text;
                                double oAmount = Convert.ToDouble(row.Cells[7].Text);
                                int oSONo = Convert.ToInt32(row.Cells[20].Text);
                                string oFinancingScheme = row.Cells[21].Text;
                                string oTaxClassification = row.Cells[22].Text;
                                string oBlock = row.Cells[11].Text;
                                string oLot = row.Cells[12].Text;
                                string oBooked = row.Cells[23].Text;

                                //2023-11-16 : GET COLLECTEDTCP
                                string CollectedTCP = row.Cells[24].Text;


                                //2023-09-22 : GET LTS 
                                //GET DATA FROM ADDON QUOTATION
                                DataTable generalData = new DataTable();
                                generalData = ws.GetGeneralData(Convert.ToInt32(oDocEntry), ConfigurationManager.AppSettings["HANADatabase"].ToString()).Tables[0];
                                string LTSNo = DataHelper.DataTableRet(generalData, 0, "LTSNo", "");

                                if (!string.IsNullOrWhiteSpace(LTSNo))
                                {
                                    //2023-09-22 : GET CORRECT BP CARDCODE
                                    string qryLocation = $@"SELECT B.""Name"", B.""U_SortCode"" FROM OPRJ A INNER JOIN ""@LOCATION"" B ON A.""U_Location"" = B.""Code"" WHERE A.""PrjCode"" = '{oProject}'";
                                    DataTable dtGetLocation = hana.GetData(qryLocation, hana.GetConnection("SAPHana"));
                                    string Location = DataAccess.GetData(dtGetLocation, 0, "Name", "").ToString();

                                    string tempCardCode = "";
                                    string qryGetCashierPerLocation = $@"SELECT * FROM ""@CASHPAYEE"" WHERE ""Name"" = '{Location}'";
                                    DataTable dtGetGetCashierPerLocation = hana.GetData(qryGetCashierPerLocation, hana.GetConnection("SAPHana"));

                                    if (ddlReplenishment.Text == "Cash")
                                    {
                                        tempCardCode = DataAccess.GetData(dtGetGetCashierPerLocation, 0, "U_BPCodeCommission", ConfigSettings.ReplenishmentBP).ToString();
                                    }
                                    else
                                    {
                                        tempCardCode = oCardCode;
                                    }

                                    //2023-09-22 : ADD BLOCKING IF DEFAULT WTAXCODE DOESNT EXIST IN SAP BP
                                    string DefaultWTaxCode = "";
                                    DataTable dtSAPBPWtax = hana.GetData($@"SELECT ""WTCode"" FROM OCRD WHERE ""CardCode"" = '{tempCardCode}' ", hana.GetConnection("SAPHana"));
                                    if (DataAccess.Exist(dtSAPBPWtax))
                                    {
                                        foreach (DataRow items in dtSAPBPWtax.Rows)
                                        {
                                            DefaultWTaxCode = items["WTCode"].ToString();
                                            break;
                                        }
                                    }
                                    //2023-09-22 : ADD BLOCKING IF DEFAULT WTAXCODE DOESNT EXIST IN SAP BP
                                    if (!string.IsNullOrWhiteSpace(DefaultWTaxCode))
                                    {
                                        if (oDocEntry == "778")
                                        {
                                            string test = "";
                                        }

                                        if (!string.IsNullOrEmpty(oDocEntry))
                                        {

                                            //TextBox txtCommissionRemarks = (row.Cells[17].FindControl("txtCommissionRemarks") as TextBox);
                                            ///Doc Requirements passed
                                            //string qry = $@"SELECT A.""DocEntry"", A.""LoanType"", C.""CardCode"", B1.""Document"", C.""DocId"", A1.""BusinessType"" FROM OQUT A
                                            //                                            INNER JOIN OCRD A1 ON A.""CardCode"" = A1.""CardCode""
                                            //                                            INNER JOIN CRD1 B ON A1.""CardCode"" = B.""CardCode"" AND B.""CardType"" = 'Buyer'
                                            //                                            INNER JOIN ODOC B1 ON A1.""BusinessType"" = B1.""Code""
                                            //                                            INNER JOIN QDOC C ON A.""DocEntry"" = C.""DocEntry"" AND A.""CardCode"" = C.""CardCode""
                                            //                                            WHERE B1.""DocId"" = C.""DocId"" AND A.""DocEntry"" = '{oDocEntry}'";


                                            //string qry = $@"select A.""DocEntry"",A.""CardCode"",A.""LoanType"",CASE WHEN IFNULL(A.""Bank"",'') <> 'BANK' THEN A.""LoanType"" ELSE IFNULL(A.""Bank"",'') END ""Bank"",	B.""BusinessType"" FROM
                                            //    OQUT A INNER JOIN OCRD B ON A.""CardCode"" = B.""CardCode"" INNER JOIN
                                            //     CRD1 C ON B.""CardCode"" = C.""CardCode"" AND C.""CardType"" = 'Buyer' LEFT JOIN
                                            //     QDOC D ON D.""DocEntry"" = A.""DocEntry"" AND IFNULL(D.""DocName"",'') <> '' INNER JOIN 
                                            //     ""{oSAPDB}"".""@BUYERDOCREQH"" E ON A.""LoanType"" = E.""U_LoanType"" AND (CASE WHEN IFNULL(A.""Bank"",'') <> 'BANK' THEN A.""LoanType"" ELSE IFNULL(A.""Bank"",'') END) = IFNULL(E.""U_BankCode"",'')INNER JOIN

                                            //        ""{oSAPDB}"".""@BUYERDOCREQR"" F ON E.""DocEntry"" = F.""DocEntry"" AND D.""DocId"" = F.""LineId""
                                            //    WHERE
                                            //     A.""DocEntry"" = {oDocEntry} AND IFNULL(D.""DocName"",'') <> '' AND IFNULL(F.""U_PaymentPhase"", 1) = 1";

                                            //DataTable Reqdt = hana.GetData(qry, hana.GetConnection("SAOHana"));

                                            DataTable Reqdt = ws.LoadDocumentRemarks(oSAPDB, oDocEntry).Tables["LoadDocumentRemarks"];


                                            if (Reqdt.Rows.Count > 0)
                                            {

                                                string LoanType = Reqdt.Rows[0]["LoanType"].ToString();
                                                string oBusinessType = Reqdt.Rows[0]["BusinessType"].ToString();
                                                oBusinessType = oBusinessType.ToLower() != "corporation" ? "Individual" : oBusinessType;
                                                string Bank = Reqdt.Rows[0]["Bank"].ToString();



                                                if (Bank.Contains("nbsp") || string.IsNullOrWhiteSpace(Bank))
                                                {
                                                    if (LoanType.ToUpper() == "INHOUSE")
                                                    {
                                                        Bank = "INHOUSE";
                                                    }
                                                    else
                                                    {
                                                        if (LoanType.ToUpper() == "HDMF")
                                                        {
                                                            Bank = "HDMF";
                                                        }
                                                        if (LoanType.ToUpper() == "SPOTCASH")
                                                        {
                                                            Bank = "SPOTCASH";
                                                        }
                                                        if (LoanType.ToUpper() == "N/A")
                                                        {
                                                            Bank = "N/A";
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (LoanType.ToUpper() == "HDMF")
                                                    {
                                                        Bank = "HDMF";
                                                    }
                                                    if (LoanType.ToUpper() == "SPOTCASH")
                                                    {
                                                        Bank = "SPOTCASH";
                                                    }
                                                    if (LoanType.ToUpper() == "N/A")
                                                    {
                                                        Bank = "N/A";
                                                    }
                                                }








                                                //string qry1 = $@"select * from ""ODOC"" where IFNULL(""Code"",'Individual') = '{oBusinessType}'";
                                                //string qry2 = $@"select * from ""ODOC"" where IFNULL(""Code"",'Individual') = '{oBusinessType}' OR IFNULL(""Code"",'Both') = 'Both'";

                                                //string tqry = "";
                                                //tqry = (LoanType != "HDMF" && LoanType != "BANK") ? qry2 : qry1;
                                                string tqry = $@"SELECT A.""DocEntry""   FROM ""@BUYERDOCREQH"" A INNER JOIN ""@BUYERDOCREQR"" B ON A.""DocEntry"" = B.""DocEntry"" 
                                            WHERE A.""U_LoanType"" = '{LoanType}' AND A.""U_BankCode"" = '{Bank}' AND A.""U_BusinessType"" = '{oBusinessType}' 
                                            AND IFNULL(B.""U_PaymentPhase"", 1) = 1;";




                                                DataTable dtDoc = hana.GetData($@"{tqry}", hana.GetConnection("SAPHana"));
                                                //foreach (DataRow docpass in Reqdt.Rows)
                                                //{
                                                //    foreach (DataRow documents in dtDoc.Rows)
                                                //    {
                                                //        //string[] docArray;

                                                //        if (int.Parse(docpass.ItemArray[4].ToString()) != int.Parse(documents.ItemArray[0].ToString()))
                                                //        {
                                                //            //var rowAsString = string.Join();
                                                //            txtCommissionRemarks.Text = documents.ItemArray[1].ToString();
                                                //            //docArray = docArray.
                                                //            break;
                                                //        }
                                                //        //else { ItemArray.Cast<string>().ToArray()
                                                //        //    alertMsg($@"Your Doc Req is incomplete", "info");
                                                //        //}
                                                //    }

                                                //}


                                                if (Reqdt.Rows.Count == dtDoc.Rows.Count)
                                                {
                                                    if (!oCardCode.Contains("nbsp") && !string.IsNullOrEmpty(oCardCode))
                                                    {

                                                        //POSTING TO SAP AP INVOICE
                                                        string _rest = wcf.PostAP("Commission", oDocEntry, oPosition, oRelease, oCardCode,
                                                                        ConfigSettings.CommissionProperty, ddlReplenishment.Text, txtDocDate.Text);
                                                        if (_rest.Contains("error"))
                                                        {
                                                            ////IF ERROR: CREATE FUNCTION
                                                            alertMsg($@"Error on posting for Document: {oDocEntry}  Agent: {oAgent}  Release: {oRelease} -- ({_rest})", "info");
                                                        }
                                                        else
                                                        {
                                                            string CommissionExpenseAccount = ConfigSettings.CommissionExpenseAccount;
                                                            string PrepaidCommissionAccount = ConfigSettings.PrepaidCommissionAccount;
                                                            int JournalEntryNo;
                                                            string Message = "";

                                                            //IF BOOKED, CREATE AUTOMATIC JOURNAL ENTRY
                                                            if (oBooked == "Y")
                                                            {

                                                                //2023-11-13 : ADDED NEW CONDITION -- POST JE ONLY IF ALL COMMISSIONS ARE ALREADY POSTED
                                                                DataTable dtCountCommission = ws.GetCommissionPerDocEntry(oSAPDB, oDocEntry).Tables["GetCommissionPerDocEntry"];
                                                                if (dtCountCommission != null)
                                                                {
                                                                    //2023-11-13 : IF NO FOR RELEASE ANYMORE, POST JE 
                                                                    if (dtCountCommission.Rows.Count == 0)
                                                                    {

                                                                        //2023-11-13 : CHECK IF JE ALREADY EXISTS
                                                                        string qryCheckIfJEExists = $@"select * from ojdt where ""TransCode"" = 'ComJ' and ""Ref3"" = '{oDocNum}'";
                                                                        DataTable dtCheckIfJEExists = hana.GetData(qryCheckIfJEExists, hana.GetConnection("SAPHana"));
                                                                        if (dtCheckIfJEExists != null)
                                                                        {

                                                                            //2023-11-13 : CHECK IF JE ALREADY EXISTS
                                                                            if (dtCheckIfJEExists.Rows.Count == 0)
                                                                            {

                                                                                //2023-11-16 : GET COLLECTEDTCP% FOR CONDITION VALIDATION; JE WILL ONLY BE POSTED IF 25% IS POSTED FOR AP INVOICE
                                                                                if (int.Parse(CollectedTCP) >= 25)
                                                                                {

                                                                                    //2023-11-13 : CHANGE QUERY FOR GETTING AMOUNT FOR OPCH POSTING
                                                                                    ////GET TOTAL AMOUNT OF UNCREATED JEs OF COMMISSIONS
                                                                                    ////string qryAmount = $@"SELECT IFNULL(SUM(IFNULL(""BaseAmount"",0)),0) ""DocTotal"" FROM OPCH WHERE ""U_DreamsQuotationNo"" = '{oDocNum}' AND 
                                                                                    //string qryAmount = $@"SELECT
                                                                                    //                SUM(B.""LineTotal"") ""DocTotal""
                                                                                    //              FROM 
                                                                                    //                OPCH A INNER JOIN	
                                                                                    //             PCH1 B ON A.""DocEntry"" = B.""DocEntry""
                                                                                    //              WHERE 
                                                                                    //                ""U_DreamsQuotationNo"" = '{oDocNum}' AND 
                                                                                    //              ""U_TransactionType"" = 'COMMI' AND ""CANCELED"" = 'N' AND IFNULL(""U_JENo"",'') = ''";
                                                                                    //DataTable dtTotalAmount = hana.GetData(qryAmount, hana.GetConnection("SAPHana"));
                                                                                    //double AmountTotal = Convert.ToDouble(DataAccess.GetData(dtTotalAmount, 0, "DocTotal", "").ToString());

                                                                                    string qryAmount = $@"SELECT
                                                                                    SUM(B.""LineTotal"") ""DocTotal""
                                                                                  FROM 
                                                                                    OPCH A INNER JOIN	
	                                                                                PCH1 B ON A.""DocEntry"" = B.""DocEntry""
                                                                                  WHERE 
                                                                                    ""U_DreamsQuotationNo"" = '{oDocNum}' AND 
                                                                                  ""U_TransactionType"" = 'COMMI' AND ""CANCELED"" = 'N'";
                                                                                    DataTable dtTotalAmount = hana.GetData(qryAmount, hana.GetConnection("SAPHana"));
                                                                                    double AmountTotal = Convert.ToDouble(DataAccess.GetData(dtTotalAmount, 0, "DocTotal", "").ToString());



                                                                                    //CREATE AUTOMATIC JOURNAL ENTRY
                                                                                    CashRegisterService cashRegister = new CashRegisterService();
                                                                                    //if (cashRegister.CreateJournalEntry(null, oProject, oCardCode, oAmount + AmountTotal, oSONo, oFinancingScheme,
                                                                                    if (cashRegister.CreateJournalEntry(null, oProject, oCardCode, AmountTotal, oSONo, oFinancingScheme,
                                                                                                                    oTaxClassification, oBlock, oLot, CommissionExpenseAccount, PrepaidCommissionAccount, oDocNum,
                                                                                                                    "Commission", "", "", "", "Y", Convert.ToDateTime(txtDocDate.Text).ToString("yyyyMMdd"), "COMJ", out JournalEntryNo, out Message, ""))
                                                                                    {

                                                                                        string qryUpdate = $@" UPDATE OPCH SET ""U_JENo"" = {JournalEntryNo} WHERE ""U_DreamsQuotationNo"" = '{oDocNum}' AND ""U_TransactionType"" = 'COMMI' 
                                                                                        AND ""CANCELED"" = 'N'";
                                                                                        hana.Execute(qryUpdate, hana.GetConnection("SAPHana"));

                                                                                        alertMsg($@"Transaction posted successfully!", "success");
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        alertMsg($@"Commission posted but Journal Entry failed (Document: {oDocEntry}  Agent: {oAgent}  Release: {oRelease}). -- " + Message, "warning");
                                                                                    }
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                alertMsg($@"Transaction posted successfully!", "success");
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            alertMsg($@"Transaction posted successfully!", "success");
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        alertMsg($@"Transaction posted successfully!", "success");
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    alertMsg($@"Transaction posted successfully!", "success");
                                                                }

                                                                ////2023-11-16 : RESET DOCDATE
                                                                //txtDocDate.Text = null;
                                                            }
                                                            else
                                                            {
                                                                alertMsg($@"Transaction posted successfully!", "success");
                                                            }
                                                        }

                                                    }
                                                    else
                                                    {
                                                        alertMsg($@"No SAP Buyer code for this contract. Please contact administrator", "info");
                                                    }


                                                }
                                                else
                                                {
                                                    row.BackColor = System.Drawing.ColorTranslator.FromHtml("#bdffbd");
                                                    alertMsg($@"Incomplete Document Requirements!", "info");
                                                }

                                            }
                                            else
                                            {
                                                row.BackColor = System.Drawing.ColorTranslator.FromHtml("#bdffbd");
                                                alertMsg($@"No Document Requirements!", "error");

                                            }
                                            //// --QryGroup3-- FOR COMMISSION

                                        }

                                    }
                                    else
                                    {
                                        alertMsg($@"No default Withholding Tax Code found for BP: {tempCardCode}. Please contact administrator.", "info");
                                    }
                                }
                                else
                                {
                                    alertMsg($@"No LTS found for contract with document number: {oDocNum}. Please contact administrator.", "info");
                                }
                            }
                        }
                    }



                    ////2023-11-16 : RESET DOCDATE
                    //txtDocDate.Text = null;

                    loaddata();
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

        protected void bSearch_Click(object sender, EventArgs e)
        {
            loadDataSearch(txtSearch.Value);
        }
    }
}
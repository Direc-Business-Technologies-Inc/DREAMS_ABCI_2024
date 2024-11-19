using System;
using System.Configuration;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABROWN_DREAMS
{
    public partial class Documents : Page
    {
        DirecWebService ws = new DirecWebService();
        DirecService wcf = new DirecService();
        SAPHanaAccess hana = new SAPHanaAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["ReportType"] = "";
            if (Session["UserID"] == null)
            {
                alertMsg("Session expired!", "error");
                Response.Redirect("~/pages/Login.aspx");
            }

            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            if (!IsPostBack)
            {
                ViewState["expression"] = "";
                //int ID = ws.GetDocStatusID((string)Session["UserAccess"]);

                //if (ID < 5)
                //{
                //    ID++;
                //}
                //else
                //{
                //    Session["UserAccess"] = "LB";
                //}

                //string Code = ws.GetDocStatusName(ID);
                //Session["Code"] = Code;

                //Session["ForwardedCode"] = Code;
                //lblSave.InnerText = Code.ToLower();

                //if (Code == "LB")
                //    btnSave.Visible = false;
                //else
                //    btnSave.Visible = true;

                DataTable dtaccess = (DataTable)Session["UserAccess"];
                var access = dtaccess.Select($"CodeEncrypt= 'AMD'");
                if (access.Any())
                {
                    btnCancel.Visible = false;
                }


                gvDocList2.Visible = false;
                //Session["Code"] = Code;

                GridColumns("");

                //if ((string)Session["UserAccess"] == "LB")
                //{
                //    btnSave.InnerText = "Ready For Move-In";
                //}
                //ViewState["BusinessType"] = "Individual','Corporation";
                BPList();
                DeleteTemporaryFIles();
                ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "showBuyersList();", true);
            }


            if (Session["UserID"] == null)
            {
                alertMsg("Session expired!", "error");
                Response.Redirect("~/pages/Login.aspx");
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

        //void visibleUpdateButtons(LinkButton add, LinkButton update)
        //{
        //    add.Visible = false;
        //    update.Visible = true;
        //}

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

            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "showAlert();", true);
        }

        void confirmation(string body, string type)
        {
            Session["ConfirmType"] = type;
            lblConfirmationInfo.Text = body;
            ScriptManager.RegisterStartupScript(this, GetType(), "confirmation", "showConfirmation();", true);
        }

        void ContractSellCheck(string DocEntry)
        {
            //DataTable dt = hana.GetData($@"SELECT ""AmountPaid"",""PaymentAmount"" FROM ""QUT1"" WHERE ""DocEntry"" = {DocEntry} AND ""Terms"" = 1  AND ""PaymentType"" = 'DP' AND IFNULL(""AmountPaid"",0) >= ""PaymentAmount""", hana.GetConnection("SAOHana"));
            //--COMMENTED 04-04-2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
            DataTable dt = hana.GetData($@"SELECT ""AmountPaid"",""PaymentAmount"" FROM ""QUT1"" WHERE ""DocEntry"" = {DocEntry} AND ""Terms"" = 1  AND ""PaymentType"" = 'RES' AND IFNULL(""AmountPaid"",0) >= ""PaymentAmount"" AND ""LineStatus"" <> 'R'", hana.GetConnection("SAOHana"));
            if (dt.Rows.Count == 0)
            {
                btnContractSell.Visible = false;

            }
            else
            {
                if (gvDocList.Rows.Count == 0)
                {
                    btnContractSell.Visible = false;
                }
                else
                {
                    btnContractSell.Visible = true;
                }
            }

        }
        void DASCheck(string DocEntry)
        {

            //DataTable dt = hana.GetData($@"SELECT ""DocStatus"" FROM ""OQUT"" WHERE ""DocEntry"" = {DocEntry} AND ""DocStatus"" = 'C'", hana.GetConnection("SAOHana"));
            DataTable dt = hana.GetData($@"SELECT ""DocEntry"" FROM QUT1 WHERE ""DocEntry"" = {DocEntry} AND ""LineStatus"" = 'O'", hana.GetConnection("SAOHana"));
            if (dt.Rows.Count > 0)
            {
                btnDAS.Visible = false;
            }
            else
            {
                if (gvDocList.Rows.Count == 0)
                {
                    btnContractSell.Visible = false;
                }
                else
                {
                    btnDAS.Visible = true;
                }
            }
        }




        void LoadDocuments()
        {
            //string code = "Individual";
            string code = ViewState["BusinessType"].ToString();
            code = code.ToLower() != "corporation" ? "Individual" : code;
            string bank = ViewState["Bank"].ToString();
            string loanType = ViewState["LoanType"].ToString();
            //string Se1ID;
            //if ((int)Session["UserID"] == 1)
            //{
            //    Se1ID = "3";
            //}
            //else { Se1ID = ws.GetSeqId((string)Session["ForwardedCode"]); }
            //DataTable dt = hana.GetData($@"select  NULL as ""Remarks"", NULL as ""Attachment"",* from ""ODOC""", hana.GetConnection("SAOHana"));
            //gvDocList.DataSource = ws.GetDocuments(txtDocEntry.Value, txtCustCode.Value, (string)Session["SeqId"]);
            //gvDocList.DataSource = ws.GetDocuments(txtDocEntry.Value, txtCustCode.Value, Se1ID);
            //DataTable dt = hana.GetData($@"select  NULL as ""Remarks"", NULL as ""ExpirationDate"", NULL as ""IssueDate"", NULL as ""IDType"", NULL as ""ReferenceNo"", NULL as ""Attachment"",* from ""ODOC"" where IFNULL(""Code"",'Individual') = '{code}'", hana.GetConnection("SAOHana"));
            //DataTable dt1 = hana.GetData($@"select  NULL as ""Remarks"", NULL as ""ExpirationDate"", NULL as ""IssueDate"", NULL as ""IDType"", NULL as ""ReferenceNo"", NULL as ""Attachment"",* from ""ODOC"" where IFNULL(""Code"",'Individual') = '{code}' OR IFNULL(""Code"",'Both') = 'Both'", hana.GetConnection("SAOHana"));

            if (bank.Contains("nbsp") || string.IsNullOrWhiteSpace(bank))
            {
                if (loanType.ToUpper() == "INHOUSE")
                {
                    bank = "INHOUSE";
                }
                else
                {
                    if (loanType.ToUpper() == "HDMF")
                    {
                        bank = "HDMF";
                    }
                    if (loanType.ToUpper() == "SPOTCASH")
                    {
                        bank = "SPOTCASH";
                    }
                    if (loanType.ToUpper() == "N/A")
                    {
                        bank = "N/A";
                    }
                }
            }
            else
            {
                if (loanType.ToUpper() == "HDMF")
                {
                    bank = "HDMF";
                }
                if (loanType.ToUpper() == "SPOTCASH")
                {
                    bank = "SPOTCASH";
                }
                if (loanType.ToUpper() == "N/A")
                {
                    bank = "N/A";
                }
            }

            DataTable dt = hana.GetData($@"select 
                        NULL as ""Remarks"", 
                        NULL as ""ExpirationDate"", 
                        NULL as ""IssueDate"", 
                        NULL as ""IDType"",
                        NULL as ""ReferenceNo"", 
                        NULL as ""Attachment"", 
                        B.""U_DocName"" as ""Document"", 
                        ""LineId"" as ""DocId"" 
                    from 
                       ""@BUYERDOCREQH"" A INNER JOIN
                       ""@BUYERDOCREQR"" B ON A.""DocEntry"" = B.""DocEntry""
                    where 
                        IFNULL(A.""U_BusinessType"",'Individual') = '{code}' and 
                        A.""U_BankCode"" = '{bank}' AND
                       	A.""U_LoanType"" = '{loanType}'"
             , hana.GetConnection("SAPHana"));

            //DataTable dt1 = hana.GetData($@"select  NULL as ""Remarks"", NULL as ""ExpirationDate"", NULL as ""IssueDate"", NULL as ""IDType"", 
            //NULL as ""ReferenceNo"", NULL as ""Attachment"", ""U_DocName"" as ""Document"", ""LineId"" as ""DocId"" from ""@BANKREQR"" 
            //where (IFNULL(""U_BusinessType"",'Individual') = '{code}' OR IFNULL(""U_BusinessType"",'Both') = 'Both') and ""Code"" = '{bank}'", hana.GetConnection("SAPHana"));


            DataTable dt1 = hana.GetData($@"select 
                        NULL as ""Remarks"", 
                        NULL as ""ExpirationDate"", 
                        NULL as ""IssueDate"", 
                        NULL as ""IDType"",
                        NULL as ""ReferenceNo"", 
                        NULL as ""Attachment"", 
                        B.""U_DocName"" as ""Document"", 
                        ""LineId"" as ""DocId"" 
                    from 
                       ""@BUYERDOCREQH"" A INNER JOIN
                       ""@BUYERDOCREQR"" B ON A.""DocEntry"" = B.""DocEntry""
                    where 
                        IFNULL(A.""U_BusinessType"",'Individual') = '{code}' and 
                        A.""U_BankCode"" = '{bank}' AND
                       	A.""U_LoanType"" = '{loanType}'"
                 , hana.GetConnection("SAPHana"));

            //if (loanType.ToLower() == "bank")
            //{
            //    bank = "HDMF";
            //    dt = hana.GetData($@"select 
            //            NULL as ""Remarks"", 
            //            NULL as ""ExpirationDate"", 
            //            NULL as ""IssueDate"", 
            //            NULL as ""IDType"",
            //            NULL as ""ReferenceNo"", 
            //            NULL as ""Attachment"", 
            //            B.""U_DocName"" as ""Document"", 
            //            ""LineId"" as ""DocId"" 
            //        from 
            //           ""@BUYERDOCREQH"" A INNER JOIN
            //           ""@BUYERDOCREQR"" B ON A.""DocEntry"" = B.""DocEntry""
            //        where 
            //            IFNULL(A.""U_BusinessType"",'Individual') = '{code}' and 
            //            A.""U_BankCode"" = '{bank}'"
            // , hana.GetConnection("SAPHana"));

            //    dt1 = hana.GetData($@"select 
            //            NULL as ""Remarks"", 
            //            NULL as ""ExpirationDate"", 
            //            NULL as ""IssueDate"", 
            //            NULL as ""IDType"",
            //            NULL as ""ReferenceNo"", 
            //            NULL as ""Attachment"", 
            //            B.""U_DocName"" as ""Document"", 
            //            ""LineId"" as ""DocId"" 
            //        from 
            //           ""@BUYERDOCREQH"" A INNER JOIN
            //           ""@BUYERDOCREQR"" B ON A.""DocEntry"" = B.""DocEntry""
            //        where 
            //            IFNULL(A.""U_BusinessType"",'Individual') = '{code}' and 
            //            A.""U_BankCode"" = '{bank}'"
            //         , hana.GetConnection("SAPHana"));
            //}

            //REMOVE SPOUSE CONSENT IF NOT MARRIED
            if (ViewState["CivilStatus"].ToString().ToLower() != "married")
            {
                foreach (DataRow dr in dt1.Rows)
                {
                    if (dr["Document"].ToString() == "Spouse Consent")
                    {
                        dr.Delete();
                        break;

                    }
                }
                dt1.AcceptChanges();

                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["Document"].ToString() == "Spouse Consent")
                    {
                        dr.Delete();
                        break;

                    }
                }
                dt.AcceptChanges();
            }

            gvDocList.DataSource = dt;
            gvDocList.DataSource = dt1;
            gvDocList.DataBind();
            ViewState["Documents"] = dt;
            ViewState["Documents1"] = dt1;
            BindGrid();



            string docentry = Session["DocEntry"].ToString();
            dt = hana.GetData($@"SELECT A.""DocEntry"",A.""CardCode"",A.""DocId"",A.""DocName"",A.""IDType"",A.""ReferenceNo"",A.""EXPIRATIONDATE"",A.""ISSUEDATE"" 
                                    FROM ""QDOC"" A INNER JOIN ""OQUT"" B ON A.""DocEntry"" = B.""DocEntry"" AND A.""CardCode"" = B.""CardCode"" 
                                    WHERE A.""DocEntry"" = {docentry} AND A.""CardCode"" = '{txtCustCode.Value}'", hana.GetConnection("SAOHana"));
            //if (dt.Rows.Count != 0)
            //{
            //int blank = 0;
            foreach (DataRow row in dt.Rows)
            {
                //if (row["DocName"].ToString() != "")
                //{
                foreach (GridViewRow row1 in gvDocList.Rows)
                {
                    string test1 = row["DocId"].ToString();
                    string test2 = row["DocName"].ToString();

                    string Id = gvDocList.Rows[row1.RowIndex].Cells[1].Text;
                    Image docStatus = (Image)gvDocList.Rows[row1.RowIndex].FindControl("docStatus");
                    if (Id == row["DocId"].ToString() && row["DocName"].ToString() != "")
                    {
                        //gvDocList.Rows[row1.RowIndex].Cells[3].Text = row["DocName"].ToString();
                        DropDownList ID = (DropDownList)gvDocList.Rows[row1.RowIndex].FindControl("ddIDtype");
                        TextBox Ref = (TextBox)gvDocList.Rows[row1.RowIndex].FindControl("lblReferenceNo");
                        TextBox Issue = (TextBox)gvDocList.Rows[row1.RowIndex].FindControl("lblIssueDate");
                        TextBox Expiration = (TextBox)gvDocList.Rows[row1.RowIndex].FindControl("lblExpirationDate");
                        Label lblAttachment = (Label)gvDocList.Rows[row1.RowIndex].FindControl("lblAttachment");
                        lblAttachment.Text = row["DocName"].ToString();
                        lblAttachment.ToolTip = row["DocName"].ToString();
                        ID.Text = row["IDType"].ToString();
                        Ref.Text = row["ReferenceNo"].ToString();
                        Issue.Text = Convert.ToDateTime(row["ISSUEDATE"]).ToString("yyyy-MM-dd");
                        Expiration.Text = Convert.ToDateTime(row["EXPIRATIONDATE"]).ToString("yyyy-MM-dd");

                        if (row["DocName"].ToString() != "")
                        {
                            LinkButton btnPreview = (LinkButton)gvDocList.Rows[row1.RowIndex].FindControl("btnPreview");
                            LinkButton btnDelete = (LinkButton)gvDocList.Rows[row1.RowIndex].FindControl("btnRemove");
                            Label d9 = (Label)gvDocList.Rows[row1.RowIndex].FindControl("lblFileName");
                            d9.Text = row["DocName"].ToString();
                            visibleDocumentButtons(true, btnPreview, btnDelete);
                        }
                        //LinkButton btnUpdate = (LinkButton)gvDocList.Rows[row1.RowIndex].FindControl("btnUpdateDocStatus");
                        //LinkButton btnAdd = (LinkButton)gvDocList.Rows[row1.RowIndex].FindControl("btnAddDocStatus");
                        //visibleUpdateButtons(btnAdd, btnUpdate);


                        docStatus.ImageUrl = "~/assets/img/success.png";
                        break;
                    }
                    //else
                    //{
                    //    docStatus.ImageUrl = "~/assets/img/cancel.png";
                    //}
                }
                //}
                //else
                //{
                //    blank++;
                //}
            }
            //if(dt.Rows.Count == blank)
            //{
            //    btnSave.Visible = false;
            //}
            //else
            //{
            //    btnSave.Visible = true;
            //}
            //}
            //else
            //{
            //    btnSave.Visible = false;
            //}
        }
        protected Boolean IfAMD(string Code)
        {
            return Code == "AMD";
        }
        void BPList()
        {
            gvBuyers.DataSource = null;
            gvBuyers.DataBind();
            //string query = $"CALL sp_DocAMD_BPList ('{(string)Session["UserAccess"]}')";
            string query = $"CALL sp_DocAMD_BPList ('{(string)ViewState["BusinessType"]}')";
            DataTable dt = hana.GetData(query, hana.GetConnection("SAOHana"));
            gvBuyers.DataSource = dt;
            gvBuyers.DataBind();
            ViewState["dirState"] = dt;
            ViewState["sortdr"] = "Asc";
        }

        protected void gvBuyers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Select"))
            {
                try
                {
                    int row = int.Parse(e.CommandArgument.ToString());
                    Session["DocEntry"] = gvBuyers.Rows[row].Cells[0].Text;
                    txtDocEntry.Value = gvBuyers.Rows[row].Cells[0].Text;
                    txtDocNum.Value = gvBuyers.Rows[row].Cells[5].Text;
                    txtCustCode.Value = gvBuyers.Rows[row].Cells[4].Text;

                    if (gvBuyers.Rows[row].Cells[22].Text == "Individual")
                    {
                        txtLName.Value = gvBuyers.Rows[row].Cells[6].Text;
                        txtFName.Value = gvBuyers.Rows[row].Cells[7].Text;
                        txtMName.Value = gvBuyers.Rows[row].Cells[8].Text;
                        divNames.Visible = true;
                        divCompanyName.Visible = false;

                    }
                    else
                    {
                        divNames.Visible = false;
                        divCompanyName.Visible = true;
                        txtCompanyName.Value = gvBuyers.Rows[row].Cells[21].Text;
                    }


                    //** proj details **//
                    txtProj.Value = gvBuyers.Rows[row].Cells[1].Text;
                    txtPhase.Value = HttpUtility.HtmlDecode(gvBuyers.Rows[row].Cells[9].Text).Trim();
                    //txtPhase.Value = string.Format("{0:#,#.##}", Convert.ToDecimal(gvBuyers.Rows[row].Cells[7].Text));
                    txtBlock.Value = gvBuyers.Rows[row].Cells[2].Text;
                    txtLot.Value = gvBuyers.Rows[row].Cells[3].Text;

                    txtModel.Value = gvBuyers.Rows[row].Cells[10].Text;
                    txtFinScheme.Value = gvBuyers.Rows[row].Cells[11].Text;
                    Session["SQDocEntry"] = int.Parse(gvBuyers.Rows[row].Cells[13].Text);
                    ViewState["BusinessType"] = gvBuyers.Rows[row].Cells[14].Text;
                    ViewState["LoanType"] = gvBuyers.Rows[row].Cells[15].Text;
                    ViewState["CivilStatus"] = gvBuyers.Rows[row].Cells[16].Text;
                    ViewState["Bank"] = gvBuyers.Rows[row].Cells[17].Text;

                    txtDocumentDate.Value = gvBuyers.Rows[row].Cells[18].Text;
                    txtProdType.Value = gvBuyers.Rows[row].Cells[19].Text;
                    txtLoanType.Value = gvBuyers.Rows[row].Cells[15].Text;
                    txtBank.Value = gvBuyers.Rows[row].Cells[17].Text;
                    if (txtBank.Value.ToLower().Contains("nbsp"))
                    {
                        txtBank.Value = "";
                    }
                    txtAcctDetailsNetTCP.Value = SystemClass.ToCurrency(gvBuyers.Rows[row].Cells[20].Text);

                    if (txtLoanType.Value == "HDMF")
                    {
                        divBank.Visible = false;
                        btnLoanType.Disabled = false;
                    }
                    else if (txtLoanType.Value == "INHOUSE")
                    {
                        divBank.Visible = false;
                        btnLoanType.Disabled = true;
                    }
                    else if (txtLoanType.Value == "SPOTCASH")
                    {
                        divBank.Visible = false;
                        btnLoanType.Disabled = true;
                    }
                    else
                    {
                        divBank.Visible = true;
                        btnLoanType.Disabled = false;
                    }



                    if (!gvBuyers.Rows[row].Cells[23].Text.ToLower().Contains("nbsp"))
                    {
                        lblWRACode.Text = gvBuyers.Rows[row].Cells[23].Text;
                        string qry = $@"SELECT ""Name"" from ""@WRASTATUS"" where ""Code"" = '{lblWRACode.Text}'";
                        DataTable dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
                        txtWRAStatus.Value = DataAccess.GetData(dt, 0, "Name", "").ToString();

                        txtWRADate.Text = Convert.ToDateTime(gvBuyers.Rows[row].Cells[24].Text).ToString("yyyy-MM-dd");
                        txtWRARemarks.Text = gvBuyers.Rows[row].Cells[25].Text;
                    }
                    else
                    {
                        lblWRACode.Text = "";
                        txtWRAStatus.Value = "";
                        txtWRADate.Text = "";
                        txtWRARemarks.Text = "";
                    }



                    LoadDocuments();
                    ContractSellCheck(Session["DocEntry"].ToString());
                    DASCheck(Session["DocEntry"].ToString());
                    //** load summary **//
                    DataTable dtQuotation = ws.GetQuotationData(int.Parse(txtDocEntry.Value)).Tables[0];
                    ReloadData(dtQuotation);
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "hideBuyersList();", true);
                    alertMsg(ex.Message, "error");
                }



                ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "hideBuyersList();", true);
            }
            else if (e.CommandName.Equals("Sort"))
            {
                ViewState["expression"] = e.CommandArgument.ToString();
            }
        }
        void ReloadData(DataTable d)
        {
            if (d.Rows.Count > 0)
            {
                Session["CardCode"] = d.Rows[0]["CardCode"].ToString();
                lblQuotationNum.Text = d.Rows[0]["DocNum"].ToString();
                lblDocEntry.Text = d.Rows[0]["DocEntry"].ToString();
                Session["QuotationID"] = d.Rows[0]["DocEntry"].ToString();
                //** customer details **
                lblName.Text = d.Rows[0]["Name"].ToString();
                lblID.Text = d.Rows[0]["CardCode"].ToString();
                lblModel.Text = d.Rows[0]["Model"].ToString();
                lblFName.Text = d.Rows[0]["FirstName"].ToString();
                lblLName.Text = d.Rows[0]["LastName"].ToString();
                //** project details **
                txtProjId.Text = d.Rows[0]["ProjCode"].ToString();
                lblBlock.Text = d.Rows[0]["Block"].ToString();
                lblLot.Text = d.Rows[0]["Lot"].ToString();
                txtLotArea.Text = d.Rows[0]["LotArea"].ToString();
                txtFloorArea.Text = d.Rows[0]["FloorArea"].ToString();
                //lblFinScheme.Text = d.Rows[0]["FinScheme"].ToString();
                lblFinScheme.Text = d.Rows[0]["FinancingScheme"].ToString();
                //txtAcctType.Text = d.Rows[0]["AcctType"].ToString();
                //txtSalesType.Text = d.Rows[0]["SalesType"].ToString();
                //** price details **
                //lblAmount.Text = SystemClass.ToCurrency(d.Rows[0]["ReservationFee"].ToString());
                //reservationbalance.Text = SystemClass.ToCurrency(d.Rows[0]["ReservationBalance"].ToString());
                Session["reservation"] = d.Rows[0]["ReservationFee"].ToString();
                lblDAS.Text = SystemClass.ToCurrency(d.Rows[0]["DAS"].ToString());
                lblMisc.Text = SystemClass.ToCurrency(d.Rows[0]["Misc"].ToString());
                lblVAT.Text = SystemClass.ToCurrency(d.Rows[0]["Vat"].ToString());
                lblNetTCP.Text = SystemClass.ToCurrency(d.Rows[0]["NetTCP_new"].ToString());
                lblDiscountAmount.Text = SystemClass.ToCurrency(d.Rows[0]["DiscAmount"].ToString());
                lblDiscountPercent.Text = Math.Round(Convert.ToDouble(d.Rows[0]["DiscPercent"])).ToString() + "%";
                //**** Downpayment
                lblDPPercent.Text = d.Rows[0]["DPPercent"].ToString() + "%";
                lblDPAmount.Text = SystemClass.ToCurrency(d.Rows[0]["DPAmount"].ToString());
                txtFrstDP.Text = SystemClass.ToCurrency(d.Rows[0]["FirstDP"].ToString());
                lblRsvFee.Text = SystemClass.ToCurrency(d.Rows[0]["ReservationFee"].ToString());
                lblNetDP.Text = SystemClass.ToCurrency(Convert.ToString(Convert.ToDouble(d.Rows[0]["DPAmount"]) - Convert.ToDouble(d.Rows[0]["ReservationFee"])));
                lblDPTerms.Text = d.Rows[0]["DPTerms"].ToString();
                lblDPDueDate.Text = Convert.ToDateTime(d.Rows[0]["DPDueDate"].ToString()).ToString("dd-MMM-yyyy");
                lblMonthlyDP.Text = SystemClass.ToCurrency(d.Rows[0]["MonthlyDP"].ToString());
                //**** Loanable
                lblLPercent.Text = d.Rows[0]["LPercent"].ToString() + "%";
                lblLAmount.Text = SystemClass.ToCurrency(d.Rows[0]["TCPLoanableBalance"].ToString());
                lblLTerms.Text = d.Rows[0]["LTerms"].ToString();
                lblRate.Text = d.Rows[0]["InterestRate"].ToString();
                lblProdType.Text = d.Rows[0]["ProductType"].ToString();
                lblLoanType.Text = d.Rows[0]["LoanType"].ToString();
                lblBank.Text = ViewState["Bank"].ToString();


                if (d.Rows[0]["LTerms"].ToString() == "0")
                {
                    if (lblFinScheme.Text.ToUpper() == "SPOTCASH")
                    {
                        lblLTerms.Text = "0";
                    }
                    else
                    {
                        lblLTerms.Text = "1";
                    }
                    lblMonthlyAmort.Text = lblLAmount.Text;
                }
                else
                {
                    lblLTerms.Text = d.Rows[0]["LTerms"].ToString();
                    lblMonthlyAmort.Text = SystemClass.ToCurrency(d.Rows[0]["MonthlyLB"].ToString());
                    //txtMonthlyAmort.Text = SystemClass.ToCurrency(d.Rows[0]["LAmount"].ToString());
                }


                //2023 -- MISCELLANEOUS TABLE
                lblMiscFinancingScheme.Text = d.Rows[0]["MiscFinancingScheme"].ToString();
                lblMiscDueDate.Text = Convert.ToDateTime(d.Rows[0]["MiscDueDate"]).ToString("dd-MMM-yyyy");
                lblMiscFees.Text = SystemClass.ToCurrency(d.Rows[0]["AddMiscFees"].ToString());
                lblMiscDPAmount.Text = SystemClass.ToCurrency(d.Rows[0]["MiscDPAmount"].ToString());
                lblMiscDPMonthly.Text = SystemClass.ToCurrency(d.Rows[0]["MiscFeesMonthly"].ToString());
                lblMiscDPTerms.Text = d.Rows[0]["MiscDPTerms"].ToString();
                lblMiscLBAmount.Text = SystemClass.ToCurrency(d.Rows[0]["MiscLBAmount"].ToString());
                lblMiscLBMonthly.Text = SystemClass.ToCurrency(d.Rows[0]["MiscLBMonthly"].ToString());
                lblMiscLBTerms.Text = d.Rows[0]["MiscLBTerms"].ToString();


                lblRetitlingType.Text = d.Rows[0]["RetitlingType"].ToString();
                lblAdjacentLot.Value = d.Rows[0]["AdjacentLotQuotationNo"].ToString() == "0" ? "N/A" : d.Rows[0]["AdjacentLotQuotationNo"].ToString();

                lblLoi.Text = d.Rows[0]["LOI"].ToString();
                lblLTSNo.Text = d.Rows[0]["LTSNo"].ToString();

                lblCoBorrower.Text = d.Rows[0]["Comaker"].ToString();

                int DocEntry = int.Parse(lblDocEntry.Text);
                //GET QUOTATION DATA FROM SQL [OQUT]
                DataTable dtQuotation = ws.GetQuotationData(DocEntry).Tables[0];
                if (DataAccess.Exist(dtQuotation))
                {
                    //ReloadData(dtQuotation);


                    //** load monthly amortization **//
                    gvAmortization.DataSource = ws.GetPaymentSchedule(DocEntry, "LB");
                    gvAmortization.DataBind();

                    if (gvAmortization.Rows.Count == 0)
                    { dAmort.Visible = false; }
                    else { dAmort.Visible = true; }




                    //** load monthly dp **//
                    gvDownPayment.DataSource = ws.GetPaymentSchedule(DocEntry, "DP");
                    gvDownPayment.DataBind();

                    if (gvDownPayment.Rows.Count == 0)
                    { dDown.Visible = false; }
                    else { dDown.Visible = true; }


                    //** load misc dp schedule **//
                    gvMiscellaneousDP.DataSource = ws.GetPaymentSchedule(DocEntry, "MISC", "DP");
                    gvMiscellaneousDP.DataBind();

                    if (gvMiscellaneousDP.Rows.Count == 0)
                    { dMiscDP.Visible = false; }
                    else { dMiscDP.Visible = true; }

                    //** load misc lb schedule **//
                    gvMiscellaneousAmort.DataSource = ws.GetPaymentSchedule(DocEntry, "MISC", "LB");
                    gvMiscellaneousAmort.DataBind();

                    if (gvMiscellaneousAmort.Rows.Count == 0)
                    { divMonthlyAmortMisc.Visible = false; }
                    else { divMonthlyAmortMisc.Visible = true; }


                    divMonthlyDP.Visible = true;
                    divMonthlyAmort.Visible = true;
                }
            }
        }

        protected void gvBuyers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBuyers.PageIndex = e.NewPageIndex;
            if (!string.IsNullOrEmpty(txtSearch.Value))
            {
                string query = $"CALL sp_DocAMD_BPList_Search ('{(string)ViewState["BusinessType"]}','{txtSearch.Value}')";
                gvBuyers.DataSource = hana.GetData(query, hana.GetConnection("SAOHana"));
                gvBuyers.DataBind();
            }
            else
            {
                string query = $"CALL sp_DocAMD_BPList ('{(string)ViewState["BusinessType"]}')";
                gvBuyers.DataSource = hana.GetData(query, hana.GetConnection("SAOHana"));
                gvBuyers.DataBind();
            }
        }

        protected void bSelectBP_Click(object sender, EventArgs e)
        {

        }

        protected void bCustCode_ServerClick(object sender, EventArgs e)
        {
            BPList();


            ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "showBuyersList();", true);
        }

        protected void btnCancel_ServerClick(object sender, EventArgs e)
        {
            confirmation("Are you sure you want to cancel selected documents?", "cancel");
        }

        protected void btnYes_Click(object sender, EventArgs e)
        {
            closeconfirm();
            string oDocEntry = (string)Session["DocEntry"];
            string oForwarded = (string)DataAccess.GetData(hana.GetData($@"SELECT ""ForwardedStatus"" FROM ""OQUT"" WHERE ""DocEntry"" = {oDocEntry}", hana.GetConnection("SAOHana")), 0, "ForwardedStatus", "");
            string SQEntry = (string)DataAccess.GetData(hana.GetData($@"SELECT ""SAPDocEntry"" FROM ""OQUT"" WHERE ""DocEntry"" = {oDocEntry}", hana.GetConnection("SAOHana")), 0, "SAPDocEntry", "");
            if ((string)Session["ConfirmType"] == "forward")
            {
                int ID = ws.GetDocStatusID(oForwarded);
                ID++;
                string Code = ws.GetDocStatusName(ID);
                string err = "error";
                string ret = "";

                if (Code == ConfigSettings.Forwarded)
                {

                    int SODocEntry = ws.GetSODocEntry(int.Parse(SQEntry));
                    if (SODocEntry != 0)
                    {
                        //** update status only **//
                        ret = hana.ExecuteStr($@"UPDATE ""OQUT"" SET ""ForwardedStatus"" = '{Code}' WHERE ""DocEntry"" = {oDocEntry}", hana.GetConnection("SAOHana"));

                        if (ret == "Operation completed successfully.")
                        {
                            err = "success";
                            txtCustCode.Value = string.Empty;
                            txtDocNum.Value = string.Empty;
                            txtFName.Value = string.Empty;
                            txtLName.Value = string.Empty;
                            txtMName.Value = string.Empty;

                            txtProj.Value = string.Empty;
                            txtPhase.Value = string.Empty;
                            txtModel.Value = string.Empty;
                            txtBlock.Value = string.Empty;
                            txtLot.Value = string.Empty;
                            txtFinScheme.Value = string.Empty;

                            Session["DocEntry"] = null;

                            gvDocList.DataSource = null;
                            gvDocList.DataBind();
                            gvDocList2.DataSource = null;
                            gvDocList2.DataBind();
                        }
                    }
                    else
                    {
                        //** create sales order **//
                        if (wcf.CreateSO(int.Parse(oDocEntry), (int)Session["UserID"], lblBlock.Text, lblLot.Text, txtLotArea.Text, Code, out ret) == true)
                        {
                            ret = hana.ExecuteStr($@"UPDATE ""OQUT"" SET ""ForwardedStatus"" = '{Code}' WHERE ""DocEntry"" = {oDocEntry}", hana.GetConnection("SAOHana"));

                            if (ret == "Operation completed successfully.")
                            {
                                err = "success";
                                txtCustCode.Value = string.Empty;
                                txtDocNum.Value = string.Empty;
                                txtFName.Value = string.Empty;
                                txtLName.Value = string.Empty;
                                txtMName.Value = string.Empty;
                                txtProj.Value = string.Empty;
                                txtPhase.Value = string.Empty;
                                txtModel.Value = string.Empty;
                                txtBlock.Value = string.Empty;
                                txtLot.Value = string.Empty;
                                txtFinScheme.Value = string.Empty;

                                Session["DocEntry"] = null;

                                gvDocList.DataSource = null;
                                gvDocList.DataBind();
                                gvDocList2.DataSource = null;
                                gvDocList2.DataBind();
                            }
                        }
                    }
                }
                else
                {
                    ret = hana.ExecuteStr($@"UPDATE ""OQUT"" SET ""ForwardedStatus"" = '{Code}' WHERE ""DocEntry"" = {oDocEntry}", hana.GetConnection("SAOHana"));

                    //** add documents with no attachments **//
                    foreach (GridViewRow row in gvDocList.Rows)
                    {
                        DropDownList IDt = (DropDownList)row.FindControl("ddIDtype");
                        TextBox Ref = (TextBox)row.FindControl("lblReferenceNo");
                        TextBox Issue = (TextBox)row.FindControl("lblIssueDate");
                        TextBox Expiration = (TextBox)row.FindControl("lblExpirationDate");

                        string IDType = IDt.SelectedValue;
                        string ReferenceNo = Ref.Text;
                        DateTime IssueDate = Convert.ToDateTime(Convert.ToDateTime(Issue.Text).ToString("yyyy-MM-dd"));
                        DateTime ExpirationDate = Convert.ToDateTime(Convert.ToDateTime(Expiration.Text).ToString("yyyy-MM-dd"));

                        string docid = row.Cells[1].Text;

                        //if (string.IsNullOrWhiteSpace(row.Cells[3].Text))
                        //    ws.AddDocuments(txtDocEntry.Value, txtCustCode.Value, docid, null, null, (int)Session["UserID"], IDType, ReferenceNo, IssueDate, ExpirationDate);
                    }

                    if (ret == "Operation completed successfully.")
                    {
                        err = "success";
                        txtCustCode.Value = string.Empty;
                        txtDocNum.Value = string.Empty;
                        txtFName.Value = string.Empty;
                        txtLName.Value = string.Empty;
                        txtMName.Value = string.Empty;
                        txtProj.Value = string.Empty;
                        txtPhase.Value = string.Empty;
                        txtModel.Value = string.Empty;
                        txtBlock.Value = string.Empty;
                        txtLot.Value = string.Empty;
                        txtFinScheme.Value = string.Empty;
                        Session["DocEntry"] = null;

                        gvDocList.DataSource = null;
                        gvDocList.DataBind();
                        gvDocList2.DataSource = null;
                        gvDocList2.DataBind();
                    }
                }

                //** CREATE A/R Reserve Invoice **//



                alertMsg(ret, err);
            }
            else if ((string)Session["ConfirmType"] == "cancel")
            {
                int ID = ws.GetDocStatusID(oForwarded);
                ID--;
                string Code = ws.GetDocStatusName(ID);

                //**check if cancel has remarks **//
                //bool block = false;
                //int ctr = 0;
                //foreach (GridViewRow row in gvDocList.Rows)
                //{
                //    CheckBox chk = row.FindControl("chk") as CheckBox;
                //    TextBox tRemarks = row.FindControl("txtRemarks") as TextBox;
                //    if (chk.Checked)
                //    {
                //        if (string.IsNullOrWhiteSpace(tRemarks.Text))
                //        {
                //            block = true;
                //            break;
                //        }
                //        ctr++;
                //    }

                //}

                //if (ctr == 0)
                //    alertMsg("Please select document(s) to be cancelled", "warning");
                //else
                //{
                //    if (!block)
                //    {

                if (!hana.Execute($@"UPDATE ""OQUT"" SET ""ForwardedStatus"" = '{Code}' WHERE ""DocEntry"" = {oDocEntry}", hana.GetConnection("SAOHana")))
                {
                    alertMsg("Error in cancelling documents", "error");
                }
                else
                {
                    foreach (GridViewRow row in gvDocList.Rows)
                    {
                        TextBox tRemarks = row.FindControl("txtRemarks") as TextBox;
                        int DocId = int.Parse(row.Cells[1].Text);
                        if (!hana.Execute($@"UPDATE ""QDOC"" SET ""Remarks"" = '{tRemarks.Text}' WHERE ""DocEntry"" = {oDocEntry} and ""DocId"" = '{DocId}'", hana.GetConnection("SAOHana")))
                        {
                            alertMsg("Error in updating remarks for cancelled documents", "error");
                        }
                    }

                    txtCustCode.Value = string.Empty;
                    txtDocNum.Value = string.Empty;
                    txtFName.Value = string.Empty;
                    txtLName.Value = string.Empty;
                    txtMName.Value = string.Empty;
                    txtProj.Value = string.Empty;
                    txtPhase.Value = string.Empty;
                    txtModel.Value = string.Empty;
                    txtBlock.Value = string.Empty;
                    txtLot.Value = string.Empty;
                    txtFinScheme.Value = string.Empty;
                    Session["DocEntry"] = null;
                    gvDocList.DataSource = null;
                    gvDocList.DataBind();
                    gvDocList2.DataSource = null;
                    gvDocList2.DataBind();
                    alertMsg("Cancellation of documents successful", "success");
                }
                //    }
                //    else
                //    {
                //        alertMsg("Please input remarks for cancelled documents", "warning");
                //    }
                //}
            }
            else if ((string)Session["ConfirmType"] == "decline")
            {
                if (!hana.Execute($@"UPDATE ""QDOC"" SET ""DocStatus"" = 'O' WHERE ""DocEntry"" = {oDocEntry} and ""DocId"" = '{(string)Session["selecteddoc"]}'", hana.GetConnection("SAOHana")))
                {
                    alertMsg("Error in declining documents", "error");
                }

                LoadDocuments();
            }
            else if ((string)Session["ConfirmType"] == "remove")
            {
                if (!hana.Execute($@"UPDATE ""QDOC"" SET ""DocName""=NULL,""DocAttachment""=NULL Where ""DocEntry"" = '{oDocEntry}'and ""DocId"" = '{(string)Session["selecteddoc"]}'", hana.GetConnection("SAOHana")))
                {
                    alertMsg("Error in deleting attached documents", "error");
                }

                LoadDocuments();
            }
        }
        protected void btnSave_ServerClick(object sender, EventArgs e)
        {
            //if (txtCustCode.Value == "")
            //{ alertMsg("Please choose document number first.", "info"); }
            //else
            //{ confirmation($"Are you sure you want to forward documents to {lblSave.InnerText.ToLower()}", "forward"); }
        }

        protected void bSelectPreview_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string docID = GetID.CommandArgument;
            string oDocEntry = (string)Session["DocEntry"];
            string imageUrl = "~/handler/DocumentPreview.ashx?id=" + oDocEntry + "&doc=" + docID;
            imgPreview.ImageUrl = Page.ResolveUrl(imageUrl);
            //show image preview
            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "showPreview();", true);
        }

        void closeconfirm()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "closeConfirmation", "closeConfirmation();", true);
        }
        protected void btnConfirm_ServerClick(object sender, EventArgs e)
        {
            string ret = "";
            try
            {
                string oDocEntry = (string)Session["DocEntry"];
                string oDocId = (string)Session["DocId"];
                ret = ws.DeleteImage(oDocEntry, oDocId);

            }
            catch (Exception ex)
            { ret = ex.Message; }
            string type = "error";

            if (ret == "Operation completed successfully.")
            { type = "success"; }
            alertMsg(ret, type);

            if (ret == "Operation completed successfully.")
            { LoadDocuments(); }
        }

        protected void bDeleteImg_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            Session["DocId"] = GetID.CommandArgument;
            ScriptManager.RegisterStartupScript(this, GetType(), "pop", "showMsgConfirm();", true);
        }
        void GridColumns(string code)
        {
            DataTable dtaccess = (DataTable)Session["UserAccess"];
            var access = dtaccess.Select($"CodeEncrypt= 'AMD'");
            var access2 = dtaccess.Select($"CodeEncrypt= 'DP'");
            //if (code == "AMD")
            if (access.Any())
            {
                gvDocList.Columns[13].Visible = false;
                gvDocList.Columns[14].Visible = false;
            }
            //else if (code == "DP")
            else if (access2.Any())
            {
                gvDocList.Columns[6].Visible = false;
                gvDocList.Columns[7].Visible = false;
                gvDocList.Columns[10].Visible = false;
                gvDocList.Columns[11].Visible = false;
                gvDocList.Columns[14].Visible = false;

            }
            else
            {
                gvDocList.Columns[13].Visible = false;
                gvDocList.Columns[14].Visible = false;
            }
        }
        protected void gvDocList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Image img = e.Row.FindControl("docStatus") as Image;
                if (!string.IsNullOrWhiteSpace(e.Row.Cells[4].Text))
                {
                    img.ImageUrl = "~/assets/img/checked.png";
                }
                else
                {
                    img.ImageUrl = "~/assets/img/cancel.png";
                }

                TextBox txtRemarks = e.Row.FindControl("txtRemarks") as TextBox;
                //string code = (string)Session["Code"];
                //if (code == "AMD")
                DataTable dtaccess = (DataTable)Session["UserAccess"];
                var access = dtaccess.Select($"CodeEncrypt= 'AMD'");
                if (access.Any())
                {
                    txtRemarks.ReadOnly = true;
                }
                else
                {
                    txtRemarks.ReadOnly = false;
                }

                //Image img2 = e.Row.FindControl("Status") as Image;
                //if (e.Row.Cells[3].Text != "O")
                //{
                //    img2.ImageUrl = "~/assets/img/checked.png";
                //}
                //else
                //{
                //    img2.ImageUrl = "~/assets/img/cancel.png";
                //}

                //TextBox txtRemarks = e.Row.FindControl("txtRemarks") as TextBox;
                //if ((string)Session["Code"] == "DP")
                //{
                //    gvDocList.Columns[5].Visible = false;
                //    gvDocList.Columns[6].Visible = false;
                //    gvDocList.Columns[7].Visible = false;
                //    gvDocList.Columns[8].Visible = false;
                //    gvDocList.Columns[9].Visible = false;
                //}
                //else
                //{
                //    gvDocList.Columns[9].Visible = false;
                //    gvDocList.Columns[10].Visible = false;
                //    txtRemarks.ReadOnly = true;


                //    if (!string.IsNullOrWhiteSpace(txtRemarks.Text))
                //    {
                //        //e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#af2525");
                //    }
                //}
            }
        }

        protected void gvDocList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName.Equals("Sel"))
                {
                    int index = Convert.ToInt32(e.CommandArgument);

                    string docid = gvDocList.Rows[index].Cells[1].Text;
                    Session["selecteddoc"] = docid;

                    Label lblAttachment = (Label)gvDocList.Rows[index].FindControl("lblAttachment");
                    //if (!string.IsNullOrEmpty(gvDocList.Rows[index].Cells[3].Text.Replace("&nbsp;", null)))
                    if (!string.IsNullOrEmpty(lblAttachment.Text.Replace("&nbsp;", null)))
                    {
                        string oDocEntry = (string)Session["DocEntry"];
                        string imageUrl = "~/handler/DocumentPreview.ashx?id=" + oDocEntry + "&doc=" + gvDocList.Rows[index].Cells[1].Text;
                        imgPreview.ImageUrl = Page.ResolveUrl(imageUrl);
                        //***********
                        //string imageUrl = "~/Handler/BlockPreview.ashx?id=" + txtProjId.Value + "&Block=" + block;
                        //imgPreview.ImageUrl = Page.ResolveUrl(imageUrl);
                        //show image preview
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "showPreview();", true);
                    }
                }
                //else if (e.CommandName.Equals("Upload"))
                //{
                //int index = Convert.ToInt32(e.CommandArgument);

                //string docid = gvDocList.Rows[index].Cells[1].Text;
                //Session["selecteddoc"] = docid;

                //FileUpload fu = (FileUpload)gvDocList.Rows[index].FindControl("docImage");

                ////if image is empty
                //if (fu.HasFile)
                //{
                //    System.Drawing.Image img = System.Drawing.Image.FromStream(fu.PostedFile.InputStream);
                //    //Convert Img to Byte
                //    byte[] img_byte = DataAccess.ConvertImageToByteArray(img, ImageFormat.Jpeg);

                //    string code = Environment.MachineName;

                //    //Get FileName and Extension seperately
                //    string fileNameOnly = Path.GetFileNameWithoutExtension(fu.FileName);
                //    if ((fu.FileName.Length + code.Length) >= 50)
                //    {
                //        int lengthRemove = ((fu.FileName.Length + code.Length) - 50);
                //        fileNameOnly = fileNameOnly.Remove(fileNameOnly.Length - lengthRemove - 1, lengthRemove + 1);
                //    }
                //    string extension = Path.GetExtension(fu.FileName);
                //    string uniqueCode = code;
                //    string FileName = fileNameOnly + "_" + code + extension;

                //    DropDownList ID = (DropDownList)gvDocList.Rows[index].FindControl("ddIDtype");
                //    TextBox Ref = (TextBox)gvDocList.Rows[index].FindControl("lblReferenceNo");
                //    TextBox Issue = (TextBox)gvDocList.Rows[index].FindControl("lblIssueDate");
                //    TextBox Expiration = (TextBox)gvDocList.Rows[index].FindControl("lblExpirationDate");

                //    string IDType = ID.SelectedValue;
                //    string ReferenceNo = Ref.Text;
                //    DateTime IssueDate = Convert.ToDateTime(Convert.ToDateTime(Issue.Text).ToString("yyyy-MM-dd"));
                //    DateTime ExpirationDate = Convert.ToDateTime(Convert.ToDateTime(Expiration.Text).ToString("yyyy-MM-dd"));

                //    //save/update img to database
                //    if (ws.AddDocuments(txtDocEntry.Value, txtCustCode.Value, docid, FileName, img_byte, (int)Session["UserID"], IDType, ReferenceNo, IssueDate, ExpirationDate))
                //    {
                //        //btnSave.Visible = true;
                //        alertMsg("Document uploaded successfully.", "success");

                //    }
                //    else
                //    { alertMsg("Error in uploading documents.", "error"); }
                //}
                //else
                //{
                //    alertMsg("Please choose image!", "warning");
                //}
                //LoadDocuments();
                //}
                else if (e.CommandName.Equals("Del"))
                {
                    confirmation($"Are you sure you want to remove attachment from selected document?", "remove");

                }

                else if (e.CommandName.ToString() == "Upload" || e.CommandName.ToString() == "Preview" || e.CommandName.ToString() == "Remove")
                {

                    uploadDocRequirements(gvDocList, e, "FileUpload1", "lblFileName", "btnPreview", "btnRemove");
                }
            }
            catch (Exception ex)
            {

            }
        }




        void uploadDocRequirements(GridView gv, GridViewCommandEventArgs e, string FileUpload, string lblfilename, string preview, string remove)
        {
            try
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int index = row.RowIndex;
                FileUpload fileUpload = (FileUpload)gv.Rows[index].FindControl(FileUpload);
                Label lblFileName = (Label)gv.Rows[index].FindControl(lblfilename);
                LinkButton btnPreview = (LinkButton)gv.Rows[index].FindControl(preview);
                LinkButton btnDelete = (LinkButton)gv.Rows[index].FindControl(remove);

                string DocID = gvDocList.Rows[index].Cells[1].Text;
                TextBox ReferenceNo = (TextBox)gvDocList.Rows[index].FindControl("lblReferenceNo");
                TextBox IssueDate = (TextBox)gvDocList.Rows[index].FindControl("lblIssueDate");
                TextBox ExpirationDate = (TextBox)gvDocList.Rows[index].FindControl("lblExpirationDate");
                TextBox Remarks = (TextBox)gvDocList.Rows[index].FindControl("txtRemarks");



                if (e.CommandName == "Upload")
                {
                    string documentName = gv.Rows[index].Cells[0].Text;


                    if (fileUpload.HasFile) //If there's an Uploaded a file  
                    {
                        //2023-06-22 : FOR UPLOAD OF FILES WITH DOCNUM ON THE FILES
                        //string code = Environment.MachineName;
                        string code = txtDocNum.Value;

                        //Get FileName and Extension seperately
                        string fileNameOnly = Path.GetFileNameWithoutExtension(fileUpload.FileName);
                        string extension = Path.GetExtension(fileUpload.FileName);
                        string uniqueCode = code;

                        string FileName = fileNameOnly + "_" + code + extension; //Name of the file is stored in local Variable   
                        lblFileName.Text = FileName;


                        //save/update img to database
                        if (ws.AddDocuments(txtDocEntry.Value, txtCustCode.Value, DocID, FileName, (int)Session["UserID"], ReferenceNo.Text, IssueDate.Text, ExpirationDate.Text, Remarks.Text))
                        {
                            fileUpload.PostedFile.SaveAs(Server.MapPath("~/TEMP_DOCS/") + FileName); //File is saved in the Physical folder  

                            visibleDocumentButtons(true, btnPreview, btnDelete);
                            moveTemporaryFilesToPermanent(gvDocList, "lblFileName");
                            LoadDocuments();
                            //btnSave.Visible = true;
                            alertMsg("Document uploaded successfully.", "success");

                        }
                        else
                        { alertMsg("Error in uploading documents.", "error"); }


                    }
                }
                else if (e.CommandName == "Preview")
                {
                    if (File.Exists(Server.MapPath("~/TEMP_DOCS/") + lblFileName.Text))
                    {
                        string Filepath = Server.MapPath("~/TEMP_DOCS/" + lblFileName.Text);
                        //System.Diagnostics.Process.Start(Filepath);
                        var host = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.LocalPath, "");
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + host + "/TEMP_DOCS/" + lblFileName.Text + "');", true);
                    }
                    else
                    {
                        string Filepath = Server.MapPath("~/DOCUMENT_STATUS/" + lblFileName.Text);
                        //System.Diagnostics.Process.Start(Filepath);
                        var host = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.LocalPath, "");
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + host + "/DOCUMENT_STATUS/" + lblFileName.Text + "');", true);
                    }
                }
                else if (e.CommandName == "Remove")
                {
                    if (File.Exists(Server.MapPath("~/TEMP_DOCS/") + lblFileName.Text))
                    {
                        // If file found, delete it    
                        File.Delete(Server.MapPath("~/TEMP_DOCS/") + lblFileName.Text);
                        lblFileName.Text = "";
                        visibleDocumentButtons(false, btnPreview, btnDelete);
                    }
                    else
                    {
                        lblFileName.Text = "";
                        visibleDocumentButtons(false, btnPreview, btnDelete);
                    }

                    if (!hana.Execute($@"UPDATE ""QDOC"" SET ""DocName""=NULL,""DocAttachment""=NULL Where ""DocEntry"" = '{txtDocEntry.Value}'and ""DocId"" = '{DocID}'", hana.GetConnection("SAOHana")))
                    {
                        alertMsg("Error in deleting attached documents", "error");
                    }
                    LoadDocuments();
                }
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }
        }

        void moveTemporaryFilesToPermanent(GridView gv, string lblfileName)
        {

            foreach (GridViewRow row in gv.Rows)
            {
                int index = row.RowIndex;

                string FileName = ((Label)row.FindControl(lblfileName)).Text;

                if (!string.IsNullOrWhiteSpace(FileName)) //If the used Uploaded a file  
                {
                    string sourceFilePath = Server.MapPath("~/TEMP_DOCS/") + FileName;
                    string destinationFilePath = Server.MapPath("~/DOCUMENT_STATUS/") + FileName;
                    if (File.Exists(sourceFilePath))
                    {
                        File.Copy(sourceFilePath, destinationFilePath, true);
                    }
                }
            }
        }






        protected void gvDocList2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            string docid = gvDocList2.Rows[index].Cells[0].Text;
            string oDocEntry = (string)Session["DocEntry"];
            Session["selecteddoc"] = docid;

            if (e.CommandName.Equals("Sel"))
            {
                if (!string.IsNullOrEmpty(gvDocList2.Rows[index].Cells[3].Text.Replace("&nbsp;", null)))
                {
                    string imageUrl = "~/handler/DocumentPreview.ashx?id=" + oDocEntry + "&doc=" + gvDocList2.Rows[index].Cells[1].Text;
                    imgPreview.ImageUrl = Page.ResolveUrl(imageUrl);
                    //***********
                    //string imageUrl = "~/Handler/BlockPreview.ashx?id=" + txtProjId.Value + "&Block=" + block;
                    //imgPreview.ImageUrl = Page.ResolveUrl(imageUrl);
                    //show image preview
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "showPreview();", true);
                }
            }
            else if (e.CommandName.Equals("Decline"))
            {
                TextBox tRemarks = (TextBox)gvDocList2.Rows[index].FindControl("txtRemarks");
                if (!string.IsNullOrEmpty(tRemarks.Text))
                    confirmation("Are you sure you want to decline/reject selected documents?", "decline");
                else
                    alertMsg("Please input reason for declining/rejection of documents", "warning");
            }
            else if (e.CommandName.Equals("Accept"))
            {
                if (!hana.Execute($@"UPDATE ""QDOC"" SET ""DocStatus"" = 'C' WHERE ""DocEntry"" = {oDocEntry} and ""DocId"" = '{docid}'", hana.GetConnection("SAOHana")))
                {
                    alertMsg("Error in accepting documents", "error");
                }
                LoadDocuments();
            }
        }

        protected void gvDocList2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Image img = e.Row.FindControl("docStatus") as Image;
                if (e.Row.Cells[3].Text != "O")
                {
                    img.ImageUrl = "~/assets/img/checked.png";
                }
                else
                {
                    img.ImageUrl = "~/assets/img/cancel.png";
                }
            }
        }

        protected void gvBuyers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    if (e.Row.Cells[12].Text == "DP")
            //    {
            //        e.Row.Attributes["style"] = "background-color: #adf6dc";
            //    }
            //    else
            //    {
            //        e.Row.Attributes["style"] = "background-color: #ffffff";
            //    }

            //    //** check if has sales order **//
            //    int SODocEntry = ws.GetSODocEntry(int.Parse(e.Row.Cells[13].Text));
            //    if (SODocEntry != 0)
            //    {
            //        e.Row.Attributes["style"] = "background-color: #adf6dc";
            //    }
            //    else
            //    {
            //        e.Row.Attributes["style"] = "background-color: #ffffff";
            //    }
            //}
        }

        protected void gvDownPayment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            //** load monthly dp **//
            gvDownPayment.DataSource = ws.GetPaymentSchedule(int.Parse(txtDocEntry.Value), "DP");
            gvDownPayment.PageIndex = e.NewPageIndex;
            gvDownPayment.DataBind();
        }

        protected void gvAmortization_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAmortization.DataSource = ws.GetPaymentSchedule(int.Parse(txtDocEntry.Value), "MA");
            gvAmortization.PageIndex = e.NewPageIndex;
            gvAmortization.DataBind();
        }

        protected void btnContractSell_Click(object sender, EventArgs e)
        {
            Session["PrintDocEntry"] = lblDocEntry.Text;
            Session["Title"] = "Contract to Sell";
            Session["ReportName"] = ConfigSettings.DocumentRequirement;
            Session["ReportPath"] = ConfigurationManager.AppSettings["ReportPathForms"].ToString();
            Session["ReportType"] = "";
            Session["RptConn"] = "SAP";

            //open new tab
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openpage", "window.open('ReportViewer.aspx');", true);
        }

        protected void btnDAS_Click(object sender, EventArgs e)
        {
            Session["PrintDocEntry"] = lblDocEntry.Text;
            Session["Title"] = "Contract to Sell";
            Session["ReportName"] = ConfigSettings.DocumentRequirementDAS;
            Session["ReportPath"] = ConfigurationManager.AppSettings["ReportPathForms"].ToString();
            Session["ReportType"] = "";
            Session["RptConn"] = "SAP";

            //open new tab
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openpage", "window.open('ReportViewer.aspx');", true);
        }
        protected void BindGrid()
        {
            if (((string)ViewState["LoanType"] != "HDMF") && ((string)ViewState["LoanType"] != "BANK"))
            {
                gvDocList.DataSource = (DataTable)ViewState["Documents1"];
                gvDocList.DataBind();
            }
            else
            {
                gvDocList.DataSource = (DataTable)ViewState["Documents"];
                gvDocList.DataBind();
            }

        }
        protected void gvDocList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDocList.PageIndex = e.NewPageIndex;
            BindGrid();

            //gvDocList.PageIndex = e.NewPageIndex;
            //gvDocList.DataBind();
        }

        protected void bSearch_ServerClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Value))
            {
                string query = $"CALL sp_DocAMD_BPList_Search ('{(string)ViewState["BusinessType"]}','{txtSearch.Value}')";
                gvBuyers.DataSource = hana.GetData(query, hana.GetConnection("SAOHana"));
                gvBuyers.DataBind();
            }
            else
            {
                string query = $"CALL sp_DocAMD_BPList ('{(string)ViewState["BusinessType"]}')";
                gvBuyers.DataSource = hana.GetData(query, hana.GetConnection("SAOHana"));
                gvBuyers.DataBind();
            }
        }

        protected void gvDocList_PageIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvDocList_PageIndexChanging1(object sender, GridViewPageEventArgs e)
        {
            gvDocList.DataSource = ViewState["Documents"];
            gvDocList.PageIndex = e.NewPageIndex;
            gvDocList.DataBind();
            LoadDocuments();
        }
        //protected void txtBusinessType_OnSelect(object sender, EventArgs e)
        //{
        //    ViewState["BusinessType"] = txtBusinessType.SelectedItem.Value;
        //    BPList();
        //}

        protected void btnLoanType_ServerClick(object sender, EventArgs e)
        {
            //string qry = $@"SELECT ""Code"", ""Name"" FROM ""@LOANTYPE"" WHERE ""U_Active"" = 'Y' AND ""Code"" <> 'INHOUSE'  {(ViewState["BusinessType"].ToString() == "Corporation" ? @" AND ""Code"" <> 'HDMF'" : "")} ";
            string qry = $@"SELECT ""Code"", ""Name"" FROM ""@LOANTYPE"" WHERE ""U_Active"" = 'Y'  {(ViewState["BusinessType"].ToString() == "Corporation" ? @" AND ""Code"" <> 'HDMF'" : "")} ";
            gvLoanType.DataSource = hana.GetData(qry, hana.GetConnection("SAPHana"));
            gvLoanType.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgLoanType", "MsgLoanType_Show();", true);
        }

        protected void bLoanType_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string Code = GetID.CommandArgument;
            txtLoanType.Value = Code;

            if (txtLoanType.Value != "BANK")
            {
                divBank.Visible = false;
            }
            else
            {
                divBank.Visible = true;
            }

            ViewState["LoanType"] = Code;

            LoadDocuments();

            ScriptManager.RegisterStartupScript(this, GetType(), "MsgLoanType", "MsgLoanType_Hide();", true);
        }

        protected void gvLoanType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLoanType.PageIndex = e.NewPageIndex;
            //string qry = $@"SELECT ""Code"", ""Name"" FROM ""@LOANTYPE"" WHERE ""U_Active"" = 'Y' AND ""Code"" <> 'INHOUSE'  {(ViewState["BusinessType"].ToString() == "Corporation" ? @" AND ""Code"" <> 'HDMF'" : "")} ";
            string qry = $@"SELECT ""Code"", ""Name"" FROM ""@LOANTYPE"" WHERE ""U_Active"" = 'Y' {(ViewState["BusinessType"].ToString() == "Corporation" ? @" AND ""Code"" <> 'HDMF'" : "")} ";
            gvLoanType.DataSource = hana.GetData(qry, hana.GetConnection("SAPHana"));
            gvLoanType.DataBind();
        }

        protected void btnBank_ServerClick(object sender, EventArgs e)
        {
            string qry = $@"SELECT DISTINCT B.""U_BankCode"" ""Code"",B.""U_BankName"" ""Name"" from ""@ACCREDBANKSH"" A LEFT JOIN ""@ACCREDBANKSR"" B ON A.""Code"" = B.""Code"" AND A.""Code"" = '{txtProj.Value}'
                            WHERE IFNULL(""U_ValidFr"",CURRENT_DATE) <= CURRENT_DATE AND IFNULL(""U_ValidTo"",CURRENT_DATE)>= CURRENT_DATE AND IFNULL(B.""Code"",'') <> '' ";
            gvBanks.DataSource = hana.GetData(qry, hana.GetConnection("SAPHana"));
            gvBanks.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgAccreditedBanks", "MsgAccreditedBanks_Show();", true);
        }

        protected void bBanks_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string Code = GetID.CommandArgument;
            txtBank.Value = Code;

            ViewState["Bank"] = Code;

            LoadDocuments();

            ScriptManager.RegisterStartupScript(this, GetType(), "MsgAccreditedBanks", "MsgAccreditedBanks_Hide();", true);
        }

        protected void gvBanks_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBanks.PageIndex = e.NewPageIndex;
            string qry = @"SELECT ""Code"", ""Name"" FROM ""@ACCREDITEDBANKSH""";
            gvBanks.DataSource = hana.GetData(qry, hana.GetConnection("SAPHana"));
            gvBanks.DataBind();
        }

        protected void btnSaveQuotation_Click(object sender, EventArgs e)
        {
            try
            {

                int errorTag = 0;


                if (!string.IsNullOrWhiteSpace(txtWRAStatus.Value))
                {
                    if (string.IsNullOrWhiteSpace(txtWRADate.Text))
                    {
                        errorTag = 1;
                    }
                }

                if (errorTag == 0)
                {
                    string loantype = txtLoanType.Value;
                    string bank = txtBank.Value;
                    string ret = hana.ExecuteStr($@"UPDATE ""OQUT"" SET 
                                                    ""Bank"" = '{bank}', 
                                                    ""LoanType"" = '{loantype}',
                                                    ""WRAStatus"" = '{lblWRACode.Text}',
                                                    ""WRAUpdateDate"" = '{(string.IsNullOrWhiteSpace(txtWRADate.Text) ? "" : Convert.ToDateTime(txtWRADate.Text).ToString("yyyyMMdd")) }',
                                                    ""WRARemarks"" = '{txtWRARemarks.Text}'
                                                WHERE 
                                                    ""DocEntry"" = {Session["DocEntry"].ToString()}", hana.GetConnection("SAOHana"));
                    if (ret == "Operation completed successfully.")
                    {
                        alertMsg("Details saved", "success");
                    }
                    else
                    {
                        alertMsg("Error on saving of details", "warning");
                    }
                }
                else
                {
                    alertMsg("Please provide Update date for WRA.", "warning");
                    txtWRADate.Focus();
                }
            }
            catch (Exception ex)
            {

                alertMsg("Error: " + ex, "warning");
            }
        }

        protected void gvBuyers_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dtrslt = (DataTable)ViewState["dirState"];
            if (dtrslt.Rows.Count > 0)
            {
                if (Convert.ToString(ViewState["sortdr"]) == "Asc")
                {
                    dtrslt.DefaultView.Sort = e.SortExpression + " Desc";
                    ViewState["sortdr"] = "Desc";
                }
                else
                {
                    dtrslt.DefaultView.Sort = e.SortExpression + " Asc";
                    ViewState["sortdr"] = "Asc";
                }
                gvBuyers.DataSource = dtrslt;
                gvBuyers.DataBind();
            }
        }

        void AddSortImage(int columnIndex, GridViewRow headerRow)
        {
            // Create the sorting image based on the sort direction.
            Image sortImage = new Image();
            if (Convert.ToString(ViewState["sortdr"]) == "Asc")
            {
                sortImage.ImageUrl = columnIndex == 21 ? "~/assets/img/sort-name-ascending.png" : "~/assets/img/sort-ascending.png";
                sortImage.AlternateText = "Ascending Order";
            }
            else
            {
                sortImage.ImageUrl = columnIndex == 21 ? "~/assets/img/sort-name-descending.png" : "~/assets/img/sort-descending.png";
                sortImage.AlternateText = "Descending Order";
            }
            sortImage.Width = columnIndex == 21 ? 20 : 13;
            sortImage.Height = columnIndex == 21 ? 20 : 13;
            sortImage.Style.Remove("margin-left");
            sortImage.Style.Add("margin-left", "7px");

            // Add the image to the appropriate header cell.
            headerRow.Cells[columnIndex].Controls.Add(sortImage);

        }

        // This is a helper method used to determine the index of the
        // column being sorted. If no column is being sorted, -1 is returned.
        int GetSortColumnIndex()
        {

            // Iterate through the Columns collection to determine the index
            // of the column being sorted. 
            foreach (DataControlField field in gvBuyers.Columns)
            {
                //if (field.SortExpression == gvBuyers.SortExpression)
                if (field.SortExpression == Convert.ToString(ViewState["expression"]) && Convert.ToString(ViewState["expression"]) != "")
                {
                    return gvBuyers.Columns.IndexOf(field);
                }
            }

            return -1;
        }

        protected void gvBuyers_RowCreated(object sender, GridViewRowEventArgs e)
        {
            // Use the RowType property to determine whether the 
            // row being created is the header row. 
            if (e.Row.RowType == DataControlRowType.Header)
            {
                // Call the GetSortColumnIndex helper method to determine 
                // the index of the column being sorted. 
                int sortColumnIndex = GetSortColumnIndex();

                if (sortColumnIndex != -1)
                {
                    // Call the AddSortImage helper method to add
                    // a sort direction image to the appropriate
                    // column header. 
                    AddSortImage(sortColumnIndex, e.Row);
                }
            }
        }

        protected void gvWRAStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvWRAStatus.PageIndex = e.NewPageIndex;
            string qry = @"SELECT ""Code"", ""Name"" FROM ""@WRASTATUS""";
            gvWRAStatus.DataSource = hana.GetData(qry, hana.GetConnection("SAPHana"));
            gvWRAStatus.DataBind();
        }

        protected void btnWRAStatus_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string Code = GetID.CommandArgument;
            lblWRACode.Text = Code;

            string qry = $@"SELECT * from ""@WRASTATUS"" where ""Code"" = '{Code}'";
            DataTable dt = hana.GetData(qry, hana.GetConnection("SAPHana"));

            txtWRAStatus.Value = DataAccess.GetData(dt, 0, "Name", "").ToString();
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgWRAStatus", "MsgWRAStatus_Hide();", true);

        }

        protected void btnWRAStatus1_Click(object sender, EventArgs e)
        {

        }


        protected void btnWRAStatus_ServerClick(object sender, EventArgs e)
        {
            string qry = $@"SELECT * from ""@WRASTATUS"" ";
            gvWRAStatus.DataSource = hana.GetData(qry, hana.GetConnection("SAPHana"));
            gvWRAStatus.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgWRAStatus", "MsgWRAStatus_Show();", true);
        }

        protected void gvMiscellaneousDP_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMiscellaneousDP.DataSource = ws.GetPaymentSchedule(int.Parse(Session["QuotationID"].ToString()), "MISC", "DP");
            gvMiscellaneousDP.PageIndex = e.NewPageIndex;
            gvMiscellaneousDP.DataBind();
        }

        protected void gvMiscellaneousAmort_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMiscellaneousAmort.DataSource = ws.GetPaymentSchedule(int.Parse(Session["QuotationID"].ToString()), "MISC", "LB");
            gvMiscellaneousAmort.PageIndex = e.NewPageIndex;
            gvMiscellaneousAmort.DataBind();
        }

        protected void gvMiscellaneousAmort_PageIndexChanging1(object sender, GridViewPageEventArgs e)
        {

        }
    }
}
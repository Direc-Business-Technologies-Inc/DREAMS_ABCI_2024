using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABROWN_DREAMS.pages
{
    public partial class DocumentStatus : System.Web.UI.Page
    {
        DirecWebService ws = new DirecWebService();
        SAPHanaAccess hana = new SAPHanaAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //btnShowStatus.Visible = false;  
                lblDocEntry.Text = "0";
                LoadBuyers();
                LoadStatus();
                DfltTab((string)Session["ForwardedCode"]);
                //Show Modal
                Page.Form.Attributes.Add("enctype", "multipart/form-data");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "buyer", "showBuyer();", true);
                DeleteTemporaryFIles();
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
                    File.Copy(sourceFilePath, destinationFilePath, true);
                }
            }
        }
        void LoadBuyers()
        {
            if (gvBuyers.Rows.Count == 0)
            {
                Session["Buyers"] = ws.DocStatBuyerList();
                gvBuyers.DataSource = (DataSet)Session["Buyers"];
                gvBuyers.DataBind();
            }
        }

        void LoadStatus()
        {
            ddStatus.DataSource = ws.GetStatus((string)Session["ForwardedCode"]);
            ddStatus.DataBind();

            if (ddStatus.Items.Count > 0)
            {
                ddSubStatus.DataSource = ws.GetSubStatus(int.Parse(ddStatus.SelectedItem.Value));
                ddSubStatus.DataBind();
            }
        }
        void LoadStatus(string code)
        {
            ddStatus.DataSource = ws.GetStatus(code);
            ddStatus.DataBind();

            if (ddStatus.Items.Count > 0)
            {
                ddSubStatus.DataSource = ws.GetSubStatus(int.Parse(ddStatus.SelectedItem.Value));
                ddSubStatus.DataBind();
            }
        }
        void LoadDocuments()
        {
            DataTable dt = hana.GetData($@"SELECT TO_VARCHAR (TO_DATE(CURRENT_DATE), 'yyyy-MM-dd') ""inputdate"",* FROM ""@DOCUMENTS"" WHERE ""U_Scheme"" = '{ViewState["LoanType"]}'", hana.GetConnection("SAPHana"));
            gvStatus.DataSource = dt;
            gvStatus.DataBind();
            foreach (GridViewRow row in gvStatus.Rows)
            {
                TextBox d1 = (TextBox)gvStatus.Rows[row.RowIndex].FindControl("txtDate1");
                d1.Text = DateTime.Now.ToString("yyyy-MM-dd");
                TextBox d2 = (TextBox)gvStatus.Rows[row.RowIndex].FindControl("txtDate2");
                d2.Text = DateTime.Now.ToString("yyyy-MM-dd");
                TextBox d3 = (TextBox)gvStatus.Rows[row.RowIndex].FindControl("txtDate3");
                d3.Text = DateTime.Now.ToString("yyyy-MM-dd");
                TextBox d4 = (TextBox)gvStatus.Rows[row.RowIndex].FindControl("txtDate4");
                d4.Text = DateTime.Now.ToString("yyyy-MM-dd");
                TextBox d5 = (TextBox)gvStatus.Rows[row.RowIndex].FindControl("txtDate5");
                d5.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        void LoadBuyerDocuments(int DocEntry)
        {
            DataTable dt = hana.GetData($@"SELECT * FROM ""DOCSTATUS"" WHERE ""DocEntry"" = {DocEntry} ", hana.GetConnection("SAOHana"));
            foreach (DataRow row in dt.Rows)
            {
                foreach (GridViewRow row1 in gvStatus.Rows)
                {
                    string d = gvStatus.Rows[row1.RowIndex].Cells[0].Text;
                    if (d != "")
                    {
                        string ekek = row["DocCode"].ToString();
                        if (d == row["DocCode"].ToString())
                        {
                            string[] test = row["InputDate"].ToString().Split('/');
                            string month, day, year;
                            month = test[0];
                            day = test[1];
                            year = test[2];
                            gvStatus.Rows[row1.RowIndex].Cells[1].Text = Convert.ToDateTime(row["InputDate"]).ToString("yyyy-MM-dd");
                            //gvStatus.Rows[row1.RowIndex].Cells[1].Text = $"{year}-{month}-{day}";
                            TextBox d1 = (TextBox)gvStatus.Rows[row1.RowIndex].FindControl("txtDate1");
                            d1.Text = Convert.ToDateTime(row["DateRequired"]).ToString("yyyy-MM-dd");
                            TextBox d2 = (TextBox)gvStatus.Rows[row1.RowIndex].FindControl("txtDate2");
                            d2.Text = Convert.ToDateTime(row["IssueDate"]).ToString("yyyy-MM-dd");
                            TextBox d3 = (TextBox)gvStatus.Rows[row1.RowIndex].FindControl("txtDate3");
                            d3.Text = Convert.ToDateTime(row["DocumentDate"]).ToString("yyyy-MM-dd");
                            TextBox d4 = (TextBox)gvStatus.Rows[row1.RowIndex].FindControl("txtDate4");
                            d4.Text = Convert.ToDateTime(row["ReceivedDate"]).ToString("yyyy-MM-dd");
                            TextBox d5 = (TextBox)gvStatus.Rows[row1.RowIndex].FindControl("txtDate5");
                            d5.Text = Convert.ToDateTime(row["ExpiryDate"]).ToString("yyyy-MM-dd");
                            TextBox d6 = (TextBox)gvStatus.Rows[row1.RowIndex].FindControl("tStatusName");
                            d6.Text = row["Status"].ToString();
                            TextBox d7 = (TextBox)gvStatus.Rows[row1.RowIndex].FindControl("txtStandardReferenceNo");
                            d7.Text = row["ReferenceNo"].ToString();
                            TextBox d8 = (TextBox)gvStatus.Rows[row1.RowIndex].FindControl("txtStandardAttachmentComments");
                            d8.Text = row["Remarks"].ToString();
                            if (row["Attachment"].ToString() != "")
                            {
                                LinkButton btnPreview = (LinkButton)gvStatus.Rows[row1.RowIndex].FindControl("btnPreview");
                                LinkButton btnDelete = (LinkButton)gvStatus.Rows[row1.RowIndex].FindControl("btnRemove");
                                Label d9 = (Label)gvStatus.Rows[row1.RowIndex].FindControl("lblFileName");
                                d9.Text = row["Attachment"].ToString();
                                visibleDocumentButtons(true, btnPreview, btnDelete);
                            }
                            LinkButton btnUpdate = (LinkButton)gvStatus.Rows[row1.RowIndex].FindControl("btnUpdateDocStatus");
                            LinkButton btnAdd = (LinkButton)gvStatus.Rows[row1.RowIndex].FindControl("btnAddDocStatus");
                            visibleUpdateButtons(btnAdd, btnUpdate);
                        }
                    }


                }
            }
            //foreach (GridViewRow row in gvStatus.Rows)
            //{
            //    TextBox d1 = (TextBox)gvStatus.Rows[row.RowIndex].FindControl("txtDate1");
            //    d1.Text = DateTime.Now.ToString("yyyy-MM-dd");
            //    TextBox d2 = (TextBox)gvStatus.Rows[row.RowIndex].FindControl("txtDate2");
            //    d2.Text = DateTime.Now.ToString("yyyy-MM-dd");
            //    TextBox d3 = (TextBox)gvStatus.Rows[row.RowIndex].FindControl("txtDate3");
            //    d3.Text = DateTime.Now.ToString("yyyy-MM-dd");
            //    TextBox d4 = (TextBox)gvStatus.Rows[row.RowIndex].FindControl("txtDate4");
            //    d4.Text = DateTime.Now.ToString("yyyy-MM-dd");
            //    TextBox d5 = (TextBox)gvStatus.Rows[row.RowIndex].FindControl("txtDate5");
            //    d5.Text = DateTime.Now.ToString("yyyy-MM-dd");
            //}
        }
        void LoadStatusList()
        {
            //DataTable dt = hana.GetData($@"Select 'Preparation' as ""Preparation"", 'Submitted' as ""Submitted"", 'Waiting' as ""Waiting"", 'Released' as ""Released"", 'Completed' as ""Completed"" from ""DUMMY""", hana.GetConnection("SAPHana"));
            DataTable dt = hana.GetData($@"Select 'Preparation' as ""Statuses"" from ""DUMMY"" union all select 'Submitted' as ""Statuses"" from ""DUMMY"" union all Select 'Waiting' as ""Statuses"" from ""DUMMY"" union all Select 'Released' as ""Statuses"" from ""DUMMY"" union all Select 'Completed' as ""Statuses"" from ""DUMMY""", hana.GetConnection("SAPHana"));
            gvStatusList.DataSource = dt;
            gvStatusList.DataBind();
        }

        void AddDocStatus()
        {
            int DocEntry = Convert.ToInt32(ViewState["BuyerDocEntry"]);
            DateTime InputDate = DateTime.MinValue;
            string Scheme = "";
            string Document = "";
            DateTime DateRequired = DateTime.MinValue;
            DateTime IssueDate = DateTime.MinValue;
            DateTime DocumentDate = DateTime.MinValue;
            DateTime ReceivedDate = DateTime.MinValue;
            DateTime ExpiryDate = DateTime.MinValue;
            string Status = "";
            string ReferenceNo = "";
            string Remarks = "";
            string Attachment = "";
            DataSet Header = null;
            try
            {
                foreach (GridViewRow row in gvStatus.Rows)
                {
                    if (row.Cells[0].Text == ViewState["DocCode"].ToString())
                    {

                        //Label inputDate = (Label)gvStatus.Rows[row.RowIndex].FindControl("inputdate");
                        //Label scheme = (Label)gvStatus.Rows[row.RowIndex].FindControl("U_Scheme");
                        //Label document = (Label)gvStatus.Rows[row.RowIndex].FindControl("U_Document");
                        TextBox dateRequired = (TextBox)gvStatus.Rows[row.RowIndex].FindControl("txtDate1");
                        TextBox issueDate = (TextBox)gvStatus.Rows[row.RowIndex].FindControl("txtDate2");
                        TextBox documentDate = (TextBox)gvStatus.Rows[row.RowIndex].FindControl("txtDate3");
                        TextBox receivedDate = (TextBox)gvStatus.Rows[row.RowIndex].FindControl("txtDate4");
                        TextBox expiryDate = (TextBox)gvStatus.Rows[row.RowIndex].FindControl("txtDate5");
                        TextBox status = (TextBox)gvStatus.Rows[row.RowIndex].FindControl("tStatusName");
                        TextBox referenceNo = (TextBox)gvStatus.Rows[row.RowIndex].FindControl("txtStandardReferenceNo");
                        TextBox remarks = (TextBox)gvStatus.Rows[row.RowIndex].FindControl("txtStandardAttachmentComments");
                        Label attachment = (Label)gvStatus.Rows[row.RowIndex].FindControl("lblFileName");

                        string[] test = row.Cells[1].Text.Split('-');
                        string month, day, year;
                        year = test[0];
                        month = test[1];
                        day = test[2];
                        string DocCode = ViewState["DocCode"].ToString();
                        InputDate = DateTime.Parse($"{year}-{month}-{day}");
                        Scheme = row.Cells[2].Text;
                        Document = row.Cells[3].Text;
                        DateRequired = Convert.ToDateTime(dateRequired.Text);
                        IssueDate = Convert.ToDateTime(issueDate.Text);
                        DocumentDate = Convert.ToDateTime(documentDate.Text);
                        ReceivedDate = Convert.ToDateTime(receivedDate.Text);
                        ExpiryDate = Convert.ToDateTime(expiryDate.Text);
                        Status = status.Text;
                        ReferenceNo = referenceNo.Text;
                        Remarks = remarks.Text;
                        Attachment = attachment.Text;

                        Header = ws.AddDocumentStatus(DocEntry,
                                                        InputDate,
                                                        Scheme,
                                                        DocCode,
                                                        Document,
                                                        DateRequired,
                                                        IssueDate,
                                                        DocumentDate,
                                                        ReceivedDate,
                                                        ExpiryDate,
                                                        Status,
                                                        ReferenceNo,
                                                        Remarks,
                                                        Attachment
                                                        );
                        break;
                    }
                }

                if (Header == null)
                {

                    string ret = $"Failed to submit your documents! Please contact your administrator (Error: Header - {Header})";
                    alertMsg(ret, "error");
                }
                else
                {
                    alertMsg("Document Added", "success");
                    moveTemporaryFilesToPermanent(gvStatus, "lblFileName");
                    DeleteTemporaryFIles();
                }
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }
        }

        void UpdateDocStatus()
        {
            int DocEntry = Convert.ToInt32(ViewState["BuyerDocEntry"]);
            DateTime DateRequired = DateTime.MinValue;
            DateTime IssueDate = DateTime.MinValue;
            DateTime DocumentDate = DateTime.MinValue;
            DateTime ReceivedDate = DateTime.MinValue;
            DateTime ExpiryDate = DateTime.MinValue;
            string Status = "";
            string ReferenceNo = "";
            string Remarks = "";
            string Attachment = "";
            DataSet Header = null;
            try
            {
                foreach (GridViewRow row in gvStatus.Rows)
                {
                    if (row.Cells[0].Text == ViewState["DocCode"].ToString())
                    {

                        //Label inputDate = (Label)gvStatus.Rows[row.RowIndex].FindControl("inputdate");
                        //Label scheme = (Label)gvStatus.Rows[row.RowIndex].FindControl("U_Scheme");
                        //Label document = (Label)gvStatus.Rows[row.RowIndex].FindControl("U_Document");
                        TextBox dateRequired = (TextBox)gvStatus.Rows[row.RowIndex].FindControl("txtDate1");
                        TextBox issueDate = (TextBox)gvStatus.Rows[row.RowIndex].FindControl("txtDate2");
                        TextBox documentDate = (TextBox)gvStatus.Rows[row.RowIndex].FindControl("txtDate3");
                        TextBox receivedDate = (TextBox)gvStatus.Rows[row.RowIndex].FindControl("txtDate4");
                        TextBox expiryDate = (TextBox)gvStatus.Rows[row.RowIndex].FindControl("txtDate5");
                        TextBox status = (TextBox)gvStatus.Rows[row.RowIndex].FindControl("tStatusName");
                        TextBox referenceNo = (TextBox)gvStatus.Rows[row.RowIndex].FindControl("txtStandardReferenceNo");
                        TextBox remarks = (TextBox)gvStatus.Rows[row.RowIndex].FindControl("txtStandardAttachmentComments");
                        Label attachment = (Label)gvStatus.Rows[row.RowIndex].FindControl("lblFileName");

                        string DocCode = ViewState["DocCode"].ToString();
                        DateRequired = Convert.ToDateTime(dateRequired.Text);
                        IssueDate = Convert.ToDateTime(issueDate.Text);
                        DocumentDate = Convert.ToDateTime(documentDate.Text);
                        ReceivedDate = Convert.ToDateTime(receivedDate.Text);
                        ExpiryDate = Convert.ToDateTime(expiryDate.Text);
                        Status = status.Text;
                        ReferenceNo = referenceNo.Text;
                        Remarks = remarks.Text;
                        Attachment = attachment.Text;

                        Header = ws.UpdateDocumentStatus(DocEntry,
                                                        DocCode,
                                                        DateRequired,
                                                        IssueDate,
                                                        DocumentDate,
                                                        ReceivedDate,
                                                        ExpiryDate,
                                                        Status,
                                                        ReferenceNo,
                                                        Remarks,
                                                        Attachment
                                                        );
                        break;
                    }
                }
                if (Header == null)
                {

                    string ret = $"Failed to submit your documents! Please contact your administrator (Error: Header - {Header})";
                    alertMsg(ret, "error");
                }
                else
                {
                    alertMsg("Document Updated", "success");
                    moveTemporaryFilesToPermanent(gvStatus, "lblFileName");
                    DeleteTemporaryFIles();
                }

            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }
        }
        protected void gvHouseLot_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Sel"))
            {
                //btnShowStatus.Visible = true;
                int row = int.Parse(e.CommandArgument.ToString());
                int DocEntry = int.Parse(gvHouseLot.Rows[row].Cells[0].Text);
                ViewState["BuyerDocEntry"] = DocEntry;
                lblDocEntry.Text = DocEntry.ToString();
                Session["QuoteDocEntry"] = DocEntry;

                lblProj.Text = gvHouseLot.Rows[row].Cells[1].Text;
                lblModel.Text = gvHouseLot.Rows[row].Cells[2].Text;
                lblBlock.Text = gvHouseLot.Rows[row].Cells[3].Text;
                lblLot.Text = gvHouseLot.Rows[row].Cells[4].Text;

                //** Account Info **//
                lblFinScheme.Text = gvHouseLot.Rows[row].Cells[8].Text;
                //lblAcctType.Text = gvHouseLot.Rows[row].Cells[9].Text;
                //lblSalesType.Text = gvHouseLot.Rows[row].Cells[10].Text;
                lblDPTerms.Text = gvHouseLot.Rows[row].Cells[11].Text;
                lblLBTerms.Text = gvHouseLot.Rows[row].Cells[12].Text;
                lblDueDate.Text = Convert.ToDateTime(gvHouseLot.Rows[row].Cells[13].Text).ToShortDateString();

                lblFinCode.Text = gvHouseLot.Rows[row].Cells[16].Text;
                lblTCP.Text = SystemClass.ToCurrency(gvHouseLot.Rows[row].Cells[17].Text);
                //lblDiscount.Text = SystemClass.ToCurrency(gvHouseLot.Rows[row].Cells[18].Text);
                lblStage.Text = gvHouseLot.Rows[row].Cells[19].Text;
                //lblAmountPaid.Text = SystemClass.ToCurrency(gvHouseLot.Rows[row].Cells[15].Text);
                lblSapCardCode.Text = gvHouseLot.Rows[row].Cells[20].Text;
                lblFName.Text = gvHouseLot.Rows[row].Cells[21].Text;
                lblLName.Text = gvHouseLot.Rows[row].Cells[22].Text;
                lblMName.Text = gvHouseLot.Rows[row].Cells[23].Text;
                lblSQEntry.Text = gvHouseLot.Rows[row].Cells[24].Text;
                lblProdType.Text = gvHouseLot.Rows[row].Cells[25].Text;
                lblLoanType.Text = gvHouseLot.Rows[row].Cells[26].Text;
                lblBank.Text = gvHouseLot.Rows[row].Cells[27].Text;
                ViewState["LoanType"] = gvHouseLot.Rows[row].Cells[26].Text;
                LoadDocuments();
                LoadBuyerDocuments(Convert.ToInt32(ViewState["BuyerDocEntry"]));
                LoadStatus();
                LoadDiary(DocEntry);
                //** highlight selected row **//
                foreach (GridViewRow selectedrow in gvHouseLot.Rows)
                {
                    if (selectedrow.RowIndex == row)
                    { selectedrow.BackColor = System.Drawing.ColorTranslator.FromHtml("#A1DCF2"); }
                    else
                    { selectedrow.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF"); }
                }
            }
        }

        protected void bSearchBuyer_Click(object sender, EventArgs e)
        {
            Session["Buyers"] = ws.DocStatBuyerListSearch(txtSearchBuyer.Value);
            gvBuyers.DataSource = (DataSet)Session["Buyers"];
            gvBuyers.DataBind();
        }

        protected void gvBuyers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Sel"))
            {
                //** clear status **//
                //gvStatus.DataSource = null;
                //gvStatus.DataBind();
                lblDocEntry.Text = "0";

                int row = int.Parse(e.CommandArgument.ToString());
                lblID.Text = gvBuyers.Rows[row].Cells[0].Text;
                lblName.Text = gvBuyers.Rows[row].Cells[4].Text;
                lblBuyerCode.Text = gvBuyers.Rows[row].Cells[0].Text;

                //load project if data = 1
                LoadHouseList();
                if (gvHouseLot.Rows.Count != 0)
                {
                    //btnShowStatus.Visible = true;
                    //LoadBuyerDocuments(Convert.ToInt32(gvBuyers.Rows[row].Cells[5].Text));
                    LoadAccountDetails();
                    LoadDocuments();
                    LoadBuyerDocuments(Convert.ToInt32(ViewState["BuyerDocEntry"]));
                    LoadStatus();
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "close", "closeBuyer();", true);
            }
        }
        void LoadHouseList()
        {
            gvHouseLot.DataSource = ws.GetAccountProjectList(lblID.Text);
            gvHouseLot.DataBind();
        }
        void LoadAccountDetails()
        {
            int DocEntry = int.Parse(gvHouseLot.Rows[0].Cells[0].Text);
            ViewState["BuyerDocEntry"] = DocEntry;
            lblDocEntry.Text = DocEntry.ToString();
            lblProj.Text = gvHouseLot.Rows[0].Cells[1].Text;
            lblModel.Text = gvHouseLot.Rows[0].Cells[2].Text;
            lblBlock.Text = gvHouseLot.Rows[0].Cells[3].Text;
            lblLot.Text = gvHouseLot.Rows[0].Cells[4].Text;
            //** Account Info **//
            lblFinScheme.Text = gvHouseLot.Rows[0].Cells[8].Text;
            //lblAcctType.Text = gvHouseLot.Rows[0].Cells[9].Text;
            //lblSalesType.Text = gvHouseLot.Rows[0].Cells[10].Text;
            lblDPTerms.Text = gvHouseLot.Rows[0].Cells[11].Text;
            lblLBTerms.Text = gvHouseLot.Rows[0].Cells[12].Text;
            lblDueDate.Text = Convert.ToDateTime(gvHouseLot.Rows[0].Cells[13].Text).ToShortDateString();
            Session["DPEntry"] = int.Parse(gvHouseLot.Rows[0].Cells[14].Text);
            Session["Payments"] = double.Parse(gvHouseLot.Rows[0].Cells[15].Text);
            lblFinCode.Text = gvHouseLot.Rows[0].Cells[16].Text;
            lblTCP.Text = SystemClass.ToCurrency(gvHouseLot.Rows[0].Cells[17].Text);
            //lblDiscount.Text = SystemClass.ToCurrency(gvHouseLot.Rows[0].Cells[18].Text);
            lblStage.Text = gvHouseLot.Rows[0].Cells[19].Text;
            //lblAmountPaid.Text = SystemClass.ToCurrency(gvHouseLot.Rows[0].Cells[15].Text);
            lblSapCardCode.Text = gvHouseLot.Rows[0].Cells[20].Text;
            lblFName.Text = gvHouseLot.Rows[0].Cells[21].Text;
            lblLName.Text = gvHouseLot.Rows[0].Cells[22].Text;
            lblMName.Text = gvHouseLot.Rows[0].Cells[23].Text;
            lblSQEntry.Text = gvHouseLot.Rows[0].Cells[24].Text;
            lblProdType.Text = gvHouseLot.Rows[0].Cells[25].Text;
            lblLoanType.Text = gvHouseLot.Rows[0].Cells[26].Text;
            lblBank.Text = gvHouseLot.Rows[0].Cells[27].Text;
            ViewState["LoanType"] = gvHouseLot.Rows[0].Cells[26].Text;
            LoadDiary(DocEntry);

            //** highlight row **//
            gvHouseLot.Rows[0].BackColor = System.Drawing.ColorTranslator.FromHtml("#A1DCF2");
        }
        void LoadDiary(int DocEntry)
        {
            //gvStatus.DataSource = ws.GetDiary(DocEntry, (string)Session["ForwardedCode"]);
            //gvStatus.DataBind();
        }
        void LoadDiary(int DocEntry, string Code)
        {
            //gvStatus.DataSource = ws.GetDiary(DocEntry, Code);
            //gvStatus.DataBind();
        }
        protected void gvBuyers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBuyers.DataSource = (DataSet)Session["Buyers"];
            gvBuyers.PageIndex = e.NewPageIndex;
            gvBuyers.DataBind();
        }

        protected void btnYes_Click(object sender, EventArgs e)
        {
            string Message = string.Empty;
            if ((string)Session["ConfirmType"] == "remove")
            {
                //close modal
                closeconfirm();
                if (!ws.RemoveDiary(int.Parse(txtDiaryId.Text), out Message))
                {
                    alertMsg("Error in removing selected document status", "error");
                }
                else
                {
                    LoadDiary(int.Parse(lblDocEntry.Text));
                }
            }
        }

        protected void btnClose_ServerClick(object sender, EventArgs e)
        {

        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            LoadBuyers();
            //Show Modal
            ScriptManager.RegisterStartupScript(this, this.GetType(), "confirmation", "showBuyer();", true);
        }

        //protected void gvStatus_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    int i = int.Parse(e.CommandArgument.ToString());
        //    //txtDiaryId.Text = gvStatus.Rows[i].Cells[0].Text;
        //    if (e.CommandName.Equals("Edt"))
        //    {
        //        //** update status **//
        //        //txtDate.Text = Convert.ToDateTime(gvStatus.Rows[i].Cells[1].Text).ToString("yyyy-MM-dd");
        //        //ddStatus.SelectedItem.Value = gvStatus.Rows[i].Cells[2].Text;
        //        //ddSubStatus.SelectedItem.Value = gvStatus.Rows[i].Cells[4].Text;
        //        //txtStatus.Text = gvStatus.Rows[i].Cells[6].Text;
        //        //btnAddStatus.Text = "Update";

        //        //ScriptManager.RegisterStartupScript(this, this.GetType(), "status", "showStatus()", true);
        //    }
        //    else
        //    {
        //        //** delete status **//
        //        confirmation("Are you sure you want to remove selected status?", "remove");
        //    }
        //}

        protected void btnAddStatus_Click(object sender, EventArgs e)
        {
            string Message = string.Empty;
            //** add status **//
            if (!ws.AddDiary(int.Parse(txtDiaryId.Text)
                , int.Parse(lblDocEntry.Text)
                , (string)Session["ForwardedCode"]
                , int.Parse(ddStatus.SelectedItem.Value)
                , int.Parse(ddSubStatus.SelectedItem.Value)
                , txtStatus.Text
                , txtDate.Text
                , out Message))
            {
                //** error **//
                alertMsg(Message, "error");
            }
            else
            {
                //** refresh data **//
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "hideStatus();", true);
                LoadStatus();
                LoadDiary(int.Parse(lblDocEntry.Text));
                txtStatus.Text = string.Empty;
                btnAddStatus.Text = "Add";
                alertMsg("Operation completed successfully", "success");
            }
        }
        protected void ddStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddSubStatus.DataSource = ws.GetSubStatus(int.Parse(ddStatus.SelectedItem.Value));
            ddSubStatus.DataBind();
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
        void confirmation(string body, string type)
        {
            Session["ConfirmType"] = type;
            lblConfirmationInfo.Text = body;
            //Show Modal
            ScriptManager.RegisterStartupScript(this, this.GetType(), "confirmation", "showConfirmation();", true);
        }
        void closeconfirm()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "closeConfirmation", "closeConfirmation();", true);
        }

        protected void btnShowStatus_Click(object sender, EventArgs e)
        {
            //DataTable dt = hana.GetData($@"SELECT ""Code"",""U_Scheme"" FROM ""@DOCUMENTS"" ", hana.GetConnection("SAPHana"));
            //ddScheme.DataSource = dt;
            //ddScheme.DataBind();
            //DataTable dt = hana.GetData($@"SELECT ""U_Document"" FROM ""@DOCUMENTS"" ", hana.GetConnection("SAPHana"));
            //ddName.DataSource = dt;
            //ddName.DataBind();
            //gvStatus.Visible = false;
            txtStatus.Text = string.Empty;
            txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtDiaryId.Text = "0";
            //AddStatus.Visible = true;
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "status", "showStatus();", true);
        }
        void DfltTab(string Code)
        {
            //bConstruction.Attributes.Add("class", "btn btn-default");
            //bAMD.Attributes.Add("class", "btn btn-default");
            //bLDD.Attributes.Add("class", "btn btn-default");
            //bRSD.Attributes.Add("class", "btn btn-default");
            //bMoveIn.Attributes.Add("class", "btn btn-default");

            //if (Code == "HC")
            //{
            //    bConstruction.Attributes.Add("class", "btn btn-success");
            //}
            //else if (Code == "AMD")
            //{
            //    bAMD.Attributes.Add("class", "btn btn-success");
            //}
            //else if (Code == "LDD")
            //{
            //    bLDD.Attributes.Add("class", "btn btn-success");
            //}
            //else if (Code == "RSD")
            //{
            //    bRSD.Attributes.Add("class", "btn btn-success");
            //}
            //else if (Code == "MI")
            //{
            //    bMoveIn.Attributes.Add("class", "btn btn-success");
            //}
        }
        protected void bTab_ServerClick(object sender, EventArgs e)
        {
            //bConstruction.Attributes.Add("class", "btn btn-default");
            //bAMD.Attributes.Add("class", "btn btn-default");
            //bLDD.Attributes.Add("class", "btn btn-default");
            //bRSD.Attributes.Add("class", "btn btn-default");
            //bMoveIn.Attributes.Add("class", "btn btn-default");

            //Control GetID = (Control)sender;
            //switch (GetID.ID)
            //{
            //    case "bConstruction":
            //        bConstruction.Attributes.Add("class", "btn btn-success");
            //        AddStatusBtn("HC");
            //        break;
            //    case "bAMD":
            //        bAMD.Attributes.Add("class", "btn btn-success");
            //        AddStatusBtn("AMD");
            //        break;
            //    case "bLDD":
            //        bLDD.Attributes.Add("class", "btn btn-success");
            //        AddStatusBtn("LDD");
            //        break;
            //    case "bRSD":
            //        bRSD.Attributes.Add("class", "btn btn-success");
            //        AddStatusBtn("RSD");
            //        break;
            //    case "bMoveIn":
            //        bMoveIn.Attributes.Add("class", "btn btn-success");
            //        AddStatusBtn("MI");
            //        break;
            //}
        }
        void AddStatusBtn(string Tab)
        {
            if ((string)Session["ForwardedCode"] == Tab)
            {
                //btnShowStatus.Visible = true;
                //gvStatus.Columns[7].Visible = true;
                //gvStatus.Columns[8].Visible = true;
            }
            else
            {
                //btnShowStatus.Visible = false;
                //gvStatus.Columns[7].Visible = false;
                //gvStatus.Columns[8].Visible = false;
            }

            LoadStatus(Tab);
            LoadDiary(int.Parse(lblDocEntry.Text), Tab);
        }

        protected void gvStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvStatus.PageIndex = e.NewPageIndex;
            LoadDocuments();
            LoadBuyerDocuments(Convert.ToInt32(ViewState["BuyerDocEntry"]));
        }
        //void bSchemeList_ServerClick()
        //{

        //}
        protected void bStatusName_ServerClick(object sender, EventArgs e)
        {
            LinkButton GetDocCode = (LinkButton)sender;
            string Code = (GetDocCode.CommandArgument);
            ViewState["DocCode"] = Code;
            LoadStatusList();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "status", "showStatusList()", true);
        }

        protected void bSelectStatus_Click(object sender, EventArgs e)
        {
            LinkButton GetStatuses = (LinkButton)sender;
            string Statuses = (GetStatuses.CommandArgument);
            //ViewState["Statuses"] = Statuses;

            foreach (GridViewRow row in gvStatus.Rows)
            {
                if (row.Cells[0].Text == ViewState["DocCode"].ToString())
                {
                    TextBox tStatusName = (TextBox)gvStatus.Rows[row.RowIndex].FindControl("tStatusName");
                    tStatusName.Text = Statuses;
                    break;
                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "status", "hideStatusList()", true);
        }
        protected void gvStatus_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Upload" || e.CommandName.ToString() == "Preview" || e.CommandName.ToString() == "Remove")
            {
                uploadDocRequirements(gvStatus, e, "FileUpload1", "lblFileName", "btnPreview", "btnRemove");
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

                if (e.CommandName == "Upload")
                {
                    string documentName = gv.Rows[index].Cells[0].Text;


                    if (fileUpload.HasFile) //If the used Uploaded a file  
                    {
                        string code = Environment.MachineName;

                        //Get FileName and Extension seperately
                        string fileNameOnly = Path.GetFileNameWithoutExtension(fileUpload.FileName);
                        string extension = Path.GetExtension(fileUpload.FileName);
                        string uniqueCode = code;

                        string FileName = fileNameOnly + "_" + code + extension; //Name of the file is stored in local Variable   
                        lblFileName.Text = FileName;
                        fileUpload.PostedFile.SaveAs(Server.MapPath("~/TEMP_DOCS/") + FileName); //File is saved in the Physical folder  

                        visibleDocumentButtons(true, btnPreview, btnDelete);
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
                }
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }
        }
        void visibleDocumentButtons(bool visible, LinkButton prev, LinkButton del)
        {
            prev.Visible = visible;
            del.Visible = visible;
        }
        void visibleUpdateButtons(LinkButton add, LinkButton update)
        {
            add.Visible = false;
            update.Visible = true;
        }
        protected void btnAddDocStatus_Click(object sender, EventArgs e)
        {
            LinkButton GetDocCode = (LinkButton)sender;
            string Code = (GetDocCode.CommandArgument);
            ViewState["DocCode"] = Code;
            AddDocStatus();
            LoadHouseList();

            if (gvHouseLot.Rows.Count != 0)
            {
                LoadAccountDetails();
                LoadDocuments();
                LoadBuyerDocuments(Convert.ToInt32(ViewState["BuyerDocEntry"]));
                LoadStatus();
            }

        }
        protected void btnUpdateDocStatus_Click(object sender, EventArgs e)
        {
            LinkButton GetDocCode = (LinkButton)sender;
            string Code = (GetDocCode.CommandArgument);
            ViewState["DocCode"] = Code;
            UpdateDocStatus();
            LoadHouseList();

            if (gvHouseLot.Rows.Count != 0)
            {
                LoadAccountDetails();
                LoadDocuments();
                LoadBuyerDocuments(Convert.ToInt32(ViewState["BuyerDocEntry"]));
                LoadStatus();
            }

        }
    }
}
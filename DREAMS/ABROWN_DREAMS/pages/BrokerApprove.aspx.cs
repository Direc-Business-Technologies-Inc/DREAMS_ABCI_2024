using ABROWN_DREAMS.wcf;
using DataCipher;
using DirecLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.EnterpriseServices;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ABROWN_DREAMS.pages
{
    public partial class BrokerApprove : System.Web.UI.Page
    {
        DirecWebService ws = new DirecWebService();
        SAPHanaAccess hana = new SAPHanaAccess();


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] == null)
                {
                    alertMsg("Session expired!", "error");
                    Response.Redirect("~/pages/Login.aspx");
                }
                else
                {

                    Session["ReportType"] = "";
                    if (!this.IsPostBack)
                    {
                        ddContactPosition.SelectedValue = "2";

                        step_2.Visible = true;
                        step_3.Visible = false;
                        step_4.Visible = false;

                        step1.Attributes.Add("class", "btn btn-info btn-circle");
                        step2.Attributes.Add("class", "btn btn-default btn-circle");
                        step3.Attributes.Add("class", "btn btn-default btn-circle");

                        DataTable dt = new DataTable();
                        dt.Columns.AddRange(new DataColumn[16]
                        {
                        new DataColumn("Id"),
                        new DataColumn("SalesPersonName"),
                        new DataColumn("Username"),
                        new DataColumn("EmailAddress"),
                        new DataColumn("Position"),
                        new DataColumn("PRCLicense"),
                        new DataColumn("PRCLicenseExpirationDate"),
                        new DataColumn("ATPDate"),
                        new DataColumn("TIN"),
                        new DataColumn("VATCode"),
                        new DataColumn("WTaxCode"),
                        new DataColumn("MobileNumber"),
                        new DataColumn("ValidFrom"),
                        new DataColumn("ValidTo"),
                        new DataColumn("HLURBLicenseNo"),
                        new DataColumn("PTRNo")
                        });
                        ViewState["SalesPerson"] = dt;
                        BindGrid();

                        DataTable dt2 = new DataTable();
                        dt2.Columns.AddRange(new DataColumn[8]
                        {
                        new DataColumn("Id"),
                        new DataColumn("SalesPersonID"),
                        new DataColumn("Position"),
                        new DataColumn("SalesPersonName"),
                        new DataColumn("LotOnlyPercentage"),
                        new DataColumn("HouseandLotPercentage"),
                        new DataColumn("OslaID"),
                        new DataColumn("TransID")
                            //,
                            //new DataColumn("ValidFrom"),
                            //new DataColumn("ValidTo")
                        });
                        ViewState["SharingDetails"] = dt2;
                        BindGrid2("");

                        //GAB 05/13/2023 adding data to ViewState["ProjectList"]
                        DataTable dt3 = new DataTable();
                        dt3.Columns.AddRange(new DataColumn[4]
                        {
                        new DataColumn("ProjectCode"),
                        new DataColumn("ProjectName"),
                        new DataColumn("Commission"),
                        new DataColumn("SelectAll")
                        });
                        ViewState["ProjectList"] = dt3;
                        BindGrid3();

                        dt2 = new DataTable();
                        //DataTable dtcheck = (DataTable)ViewState["dtBrokerHeader"];
                        if (ViewState["dtBrokerHeader"] == null)
                        {
                            dt2.Columns.AddRange(new DataColumn[4]
                            {
                        new DataColumn("BusinessName"),
                        new DataColumn("FirstName"),
                        new DataColumn("MiddleName"),
                        new DataColumn("LastName")
                            });
                            ViewState["dtBrokerHeader"] = dt2;
                        }

                        #region Old WTax Code and VAT Code
                        //string qry = "";
                        //qry = @"SELECT ""WTCode"", ""WTName"" from OWHT WHERE ""Inactive"" = 'N' AND ""U_BrokersWtax"" = 'Y'";
                        //dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
                        //ddlWTAXCode.DataSource = dt;
                        //ddlWTAXCode.DataBind();

                        ////LOAD VAT AND WTAX CODESe
                        ////qry = @"SELECT ""Code"", ""Name"" from OVTG WHERE ""Inactive"" = 'N' AND ""Category"" = 'I'"; 
                        //qry = @"SELECT ""Code"", ""Name"" from OVTG WHERE ""Inactive"" = 'N' AND ""Category"" = 'I' AND ""ReportCode"" = 'Y'"; //20220719 ALIGNED WITH EXTERNAL BROKER -KASO
                        //dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
                        //dt.Rows.Add(
                        //    " ",
                        //    " "
                        //    );
                        //ddlVATCode2.DataSource = dt;
                        //ddlVATCode2.DataBind();
                        //ddlVATCode2.SelectedValue = "IT2";
                        //ddlVATCode.DataSource = dt;
                        //ddlVATCode.DataBind();
                        //ddlVATCode.SelectedValue = "IT2";

                        //qry = @"SELECT ""WTCode"", ""WTName"" from OWHT WHERE ""Inactive"" = 'N' AND ""U_BrokersWtax"" = 'Y'";
                        //dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
                        //dt.Rows.Add(
                        //    " ",
                        //    " "
                        //    );
                        //ddlWTAXCode2.DataSource = dt;
                        //ddlWTAXCode2.DataBind();
                        //ddlWTAXCode2.SelectedValue = "C140";
                        #endregion
                        //GAB 05/29/2023 Adding null value to allow blockers
                        string qry = @"SELECT ""Code"", ""Name"" from OVTG WHERE ""Inactive"" = 'N' AND ""Category"" = 'I' AND ""ReportCode"" = 'Y'";
                        dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
                        DataRow newRow3 = dt.NewRow();
                        newRow3["Code"] = -1;
                        newRow3["Name"] = "---Select VAT Code---";
                        dt.Rows.InsertAt(newRow3, 0);
                        ddlVATCode.DataSource = dt;
                        ddlVATCode.DataBind();
                        ddlVATCode.SelectedIndex = 0;
                        ddlVATCode2.DataSource = dt;
                        ddlVATCode2.DataBind();
                        ddlVATCode2.SelectedIndex = 0;

                        //GAB 05/29/2023 Adding null value to allow blockers
                        qry = @"SELECT ""WTCode"", ""WTName"" from OWHT WHERE ""Inactive"" = 'N' AND ""U_BrokersWtax"" = 'Y'";
                        dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
                        DataRow newRow2 = dt.NewRow();
                        newRow2["WTCode"] = -1;
                        newRow2["WTName"] = "---Select Witholding Tax Code---";
                        dt.Rows.InsertAt(newRow2, 0);
                        ddlWTAXCode.DataSource = dt;
                        ddlWTAXCode.DataBind();
                        ddlWTAXCode.SelectedIndex = 0;
                        ddlWTAXCode2.DataSource = dt;
                        ddlWTAXCode2.DataBind();
                        ddlWTAXCode2.SelectedIndex = 0;

                        //GAB 05/19/2023 LOAD LOCATION
                        qry = @"SELECT ""Code"", ""Name"" FROM ""@LOCATION"" ORDER BY ""Code"";";
                        dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
                        DataRow newRow = dt.NewRow();
                        newRow["Code"] = -1;
                        newRow["Name"] = "---Select Location---";
                        dt.Rows.InsertAt(newRow, 0);
                        ddlLocation.DataSource = dt;
                        ddlLocation.DataBind();
                        ddlLocation.SelectedIndex = 0;

                        string qry1 = "";
                        //qry1 = @"SELECT ""Code"", ""Name"" FROM OLST WHERE ""GrpCode"" = 'ID' ORDER BY ""Code""";
                        qry1 = @"SELECT ""Code"", ""Name"" FROM OLST WHERE ""GrpCode"" = 'ID' AND ""IsShow"" = True ORDER BY ""Code""";
                        dt = hana.GetData(qry1, hana.GetConnection("SAOHana"));
                        dt.Rows.Add(
                            "---Select Valid ID---",
                            "---Select Valid ID---"
                            );
                        ddValidID.DataSource = dt;
                        ddValidID.DataBind();
                        ddValidID.SelectedValue = "---Select Valid ID---";
                        ddValidID2.DataSource = dt;
                        ddValidID2.DataBind();
                        ddValidID2.SelectedValue = "---Select Valid ID---";
                        ddValidID3.DataSource = dt;
                        ddValidID3.DataBind();
                        ddValidID3.SelectedValue = "---Select Valid ID---";
                        ddValidID4.DataSource = dt;
                        ddValidID4.DataBind();
                        ddValidID4.SelectedValue = "---Select Valid ID---";
                        ViewState["ValidId"] = dt;

                        ddContactValidID.DataSource = dt;
                        ddContactValidID.DataBind();
                        ddContactValidID.SelectedValue = "---Select Valid ID---";

                        //SET Valid ID's Previous Value
                        DataTable dt4 = new DataTable();
                        dt4.Columns.AddRange(new DataColumn[2]
                        {
                        new DataColumn("Code"),
                        new DataColumn("Name")
                        });
                        for (int i = 1; i <= 4; i++)
                        {
                            dt4.Rows.Add(
                            "---Select Valid ID---",
                            "---Select Valid ID---"
                                );
                        }
                        ViewState["ValidIdPrev"] = dt4;

                        //GET IDTYPES WITH EXPIRY
                        string idqry = @"SELECT * FROM ""@IDTYPE""";
                        dt = hana.GetData(idqry, hana.GetConnection("SAPHana"));
                        dt.Rows.Add(
                        "---Select Valid ID---",
                        "---Select Valid ID---",
                        "N"
                            );
                        dt.Rows.Add(
                        "Others",
                        "Others",
                        "N"
                            );
                        ViewState["IDType"] = dt;

                        ViewState["SalesPersonId"] = "";

                        //2023-09-07 : ADDED TRANSID INITIAL VALUE
                        qry = $@"SELECT MAX(""TransID"") ""TransID"" FROM ""BRK2""";
                        DataTable getTransId = hana.GetData(qry, hana.GetConnection("SAOHana"));
                        ViewState["TransId"] = DataAccess.GetData(getTransId, 0, "TransID", "0").ToString();

                        LoadBrokerDocumentsStandard();
                        LoadBrokerDocumentsSoleProp();
                        LoadBrokerDocumentsPartnership();
                        LoadBrokerDocumentsCorporation();

                        loadBrokerList();

                        ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "ModalBrokerList_Show();", true);
                        Page.Form.Attributes.Add("enctype", "multipart/form-data");
                    }
                }
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }
        }

        void loadBrokerList()
        {
            DataTable dt = ws.GetBrokerList().Tables[0];
            ViewState["BrokerList"] = dt;
            gvBrokerList.DataSource = dt;
            gvBrokerList.DataBind();
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
                    string destinationFilePath = Server.MapPath("~/DOCUMENT_REQUIREMENTS/") + FileName;
                    File.Copy(sourceFilePath, destinationFilePath, true);
                }
            }
        }


        void LoadBrokerDocumentsStandard()
        {
            gvStandardDocumentRequirements.DataSource = ws.GetBrokerDocumentsStandard();
            gvStandardDocumentRequirements.DataBind();
        }

        void LoadBrokerDocumentsSoleProp()
        {
            gvSolePropDocumentRequirements.DataSource = ws.GetBrokerDocumentsSoleProp();
            gvSolePropDocumentRequirements.DataBind();
        }
        void LoadBrokerDocumentsPartnership()
        {
            gvPartnershipDocumentRequirements.DataSource = ws.GetBrokerDocumentsPartnership();
            gvPartnershipDocumentRequirements.DataBind();
        }
        void LoadBrokerDocumentsCorporation()
        {
            gvCorporationDocumentRequirements.DataSource = ws.GetBrokerDocumentsCorporation();
            gvCorporationDocumentRequirements.DataBind();
        }


        protected void BindGrid()
        {
            //GAB 06/08/2023 blank ATPDate and PRCLicense if it's default value
            DataTable salespersondt = (DataTable)ViewState["SalesPerson"];

            // Iterate through each row in the DataTable
            foreach (DataRow row in salespersondt.Rows)
            {
                var prclicenseexpdate = row["PRCLicenseExpirationDate"].ToString();
                if (prclicenseexpdate == "1900-01-01" || prclicenseexpdate == "1900/01/01")
                {
                    row["PRCLicenseExpirationDate"] = " ";
                }

                if (row["ATPDate"].ToString() == "1900-01-01" || row["ATPDate"].ToString() == "1900/01/01")
                {
                    row["ATPDate"] = " ";
                }
            }

            salespersondt.DefaultView.Sort = "Id ASC";
            salespersondt = salespersondt.DefaultView.ToTable();
            gvSalesPerson.DataSource = salespersondt;
            gvSalesPerson.DataBind();

            string businessname = "";
            string firstname = "";
            string middlename = "";
            string lastname = "";
            string salespersonname = "";

            if (gvSalesPerson.Rows.Count > 0)
            {
                businessname = DataAccess.GetData((DataTable)ViewState["dtBrokerHeader"], 0, "BusinessName", "0").ToString();
                firstname = DataAccess.GetData((DataTable)ViewState["dtBrokerHeader"], 0, "FirstName", "0").ToString();
                middlename = DataAccess.GetData((DataTable)ViewState["dtBrokerHeader"], 0, "MiddleName", "0").ToString();
                lastname = DataAccess.GetData((DataTable)ViewState["dtBrokerHeader"], 0, "LastName", "0").ToString();
                salespersonname = $@"{firstname} {middlename} {lastname}";
            }

            string name = "";
            //Hide Delete button on Itself on salesperson
            foreach (GridViewRow row in gvSalesPerson.Rows)
            {
                LinkButton delete = ((LinkButton)row.FindControl("btnSalesPersonDelete"));
                LinkButton delete1 = ((LinkButton)row.FindControl("btnSalesPersonHouseLotDelete"));
                name = System.Net.WebUtility.HtmlDecode(row.Cells[1].Text);

                if (name != null)
                {
                    if (name == businessname || name == salespersonname)
                    {
                        delete.Visible = false;
                        delete1.Visible = false;
                        break;
                    }
                }
            }
        }

        protected void BindGrid2(string ID)
        {
            try
            {
                DataTable dt2 = new DataTable();
                dt2 = (DataTable)ViewState["SharingDetails"];

                if (ViewState["SharingID"] != null)
                {
                    //if (dt2.Select($"SalesPersonId = '{ViewState["SharingID"]}'").Any())
                    //GAB 06/29/2023 COMMENTED REASON: NOT WORKING AS INTENDED
                    //if (dt2.Select($"SalesPersonId = '{ID}'").AsEnumerable().Where(x => x.RowState != DataRowState.Deleted ? x[1].ToString() == ViewState["SharingID"].ToString() : false).Any())

                    //Filtering Lot Only Sharing
                    if (dt2.Select($"SalesPersonId = '{ID}'").AsEnumerable().Where(x => x.RowState != DataRowState.Deleted ? x[1].ToString() == ViewState["SharingID"].ToString() && Convert.ToBoolean(ViewState["isLot"]) == true : false).Any())
                    {
                        gvShareDetails.DataSource = null;
                        gvShareDetails.DataBind();
                        var datasrc = dt2.Select($"SalesPersonId = '{ViewState["SharingID"]}'").Where(x => x[5].ToString() == "0").CopyToDataTable();
                        gvShareDetails.DataSource = datasrc;

                        //GAB 06/30/2023 COMMENTED
                        //gvShareDetails.DataSource = dt2.Select($"SalesPersonId = '{ViewState["SharingID"]}'").CopyToDataTable();
                        gvShareDetails.DataBind();
                        foreach (DataRow row in dt2.Rows)
                        {
                            if (row.RowState != DataRowState.Deleted)
                            {
                                if (row["LotOnlyPercentage"].ToString() == "" || row["LotOnlyPercentage"].ToString() == "0")
                                {
                                    row["LotOnlyPercentage"] = "0";
                                }
                                if (row["HouseandLotPercentage"].ToString() == "" || row["HouseandLotPercentage"].ToString() == "0")
                                {
                                    row["HouseandLotPercentage"] = "0";
                                }
                                if (row["SalesPersonName"].ToString() == "? ? ?")
                                {
                                    row["SalesPersonName"] = DataAccess.GetData((DataTable)ViewState["dtBrokerHeader"], 0, "BusinessName", "0").ToString();
                                    break;
                                }
                            }
                        }
                    }
                    //Filtering HouseNLot Sharing
                    else if (dt2.Select($"SalesPersonId = '{ID}'").AsEnumerable().Where(x => x.RowState != DataRowState.Deleted ? x[1].ToString() == ViewState["SharingID"].ToString() && Convert.ToBoolean(ViewState["isLot"]) == false : false).Any())
                    {
                        gvShareDetails.DataSource = null;
                        gvShareDetails.DataBind();
                        var datasrc = dt2.Select($"SalesPersonId = '{ViewState["SharingID"]}'").Where(x => x[4].ToString() == "0").CopyToDataTable();
                        gvShareDetails.DataSource = datasrc;
                        gvShareDetails.DataBind();
                        foreach (DataRow row in dt2.Rows)
                        {
                            if (row.RowState != DataRowState.Deleted)
                            {
                                if (row["LotOnlyPercentage"].ToString() == "" || row["LotOnlyPercentage"].ToString() == "0")
                                {
                                    row["LotOnlyPercentage"] = "0";
                                }
                                if (row["HouseandLotPercentage"].ToString() == "" || row["HouseandLotPercentage"].ToString() == "0")
                                {
                                    row["HouseandLotPercentage"] = "0";
                                }
                                if (row["SalesPersonName"].ToString() == "? ? ?")
                                {
                                    row["SalesPersonName"] = DataAccess.GetData((DataTable)ViewState["dtBrokerHeader"], 0, "BusinessName", "0").ToString();
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        gvShareDetails.DataSource = null;
                        gvShareDetails.DataBind();
                    }
                }
                else
                {
                    gvShareDetails.DataSource = null;
                    gvShareDetails.DataBind();
                }

            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }
        }

        //GAB 05/13/2023 Binding data to DataTable
        protected void BindGrid3()
        {
            try
            {
                DataTable dt3 = new DataTable(); //initializing dt3 as new Datatable
                string qry1 = @"SELECT ""U_Project"" ""ProjectCode"",""U_PrjName"" ""ProjectName"",""U_Commission"" ""Commission"", ""U_Project"" ""SelectAll""  FROM ""@COMMINCSCHEME"" ORDER BY ""ProjectCode"" ASC";
                dt3 = hana.GetData(qry1, hana.GetConnection("SAPHana"));

                // Bind DataTable to GridView
                gvProjectList.DataSource = dt3;
                gvProjectList.DataBind();

                // Set the PageIndex property to the new page index
                gvProjectList.PageIndex = gvProjectList.PageIndex;
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }
        }

        //GAB 05/13/2023 Binding data to DataTable
        protected void BindGridSharingDetailsFromPouchDb(List<SharingDetails> pouchdata)
        {
            // Create the new DataTable
            DataTable dt5 = new DataTable();

            // Define the columns for the DataTable
            dt5.Columns.Add("Id", typeof(int));
            dt5.Columns.Add("SalesPersonId", typeof(string));
            dt5.Columns.Add("PositionSharedDetails", typeof(string));
            dt5.Columns.Add("SalesPersonNameSharedDetails", typeof(string));
            dt5.Columns.Add("PercentageSharedDetails", typeof(double));
            dt5.Columns.Add("CreateDate", typeof(DateTime));
            dt5.Columns.Add("OslaId", typeof(string));
            dt5.Columns.Add("HouseandLotSharingDetails", typeof(double));
            dt5.Columns.Add("ProjectCode", typeof(string));
            dt5.Columns.Add("ProjectName", typeof(string));
            dt5.Columns.Add("CommissionPercentage", typeof(double));
            dt5.Columns.Add("Type", typeof(string));
            dt5.Columns.Add("_id", typeof(string));

            // Loop through the pouchdata and add each item to the DataTable
            foreach (var item in pouchdata.Where(x => x.SalesPersonId == (ViewState["SharingID"]).ToString() && x.Type == (ViewState["Type"]).ToString()))
            {
                // Create a new row for the DataTable
                DataRow newRow = dt5.NewRow();

                // Set the values for the columns in the new row
                newRow["Id"] = item.Id;
                newRow["SalesPersonId"] = item.SalesPersonId;
                newRow["PositionSharedDetails"] = item.PositionSharedDetails;
                newRow["SalesPersonNameSharedDetails"] = item.SalesPersonNameSharedDetails;
                newRow["PercentageSharedDetails"] = item.PercentageSharedDetails;
                newRow["CreateDate"] = item.CreateDate;
                newRow["OslaId"] = item.OslaId;
                newRow["HouseandLotSharingDetails"] = item.HouseandLotSharingDetails;
                newRow["ProjectCode"] = item.ProjectCode;
                newRow["ProjectName"] = item.ProjectName;
                newRow["CommissionPercentage"] = item.CommissionPercentage;
                newRow["Type"] = item.Type;
                newRow["_id"] = item._id;


                // Add the new row to the DataTable
                dt5.Rows.Add(newRow);
            }



            // Create a DataView from the original DataTable
            DataView dataView = new DataView(dt5);

            dataView.Sort = "ProjectCode ASC";

            // Select only the columns you want to display
            string[] displayColumns = { "ProjectCode", "ProjectName", "CommissionPercentage", "SalesPersonNameSharedDetails", "HouseandLotSharingDetails", "PercentageSharedDetails", "OslaId", "SalesPersonId", "PositionSharedDetails", "_id" };
            dataView = new DataView(dataView.ToTable(false, displayColumns), "", "", DataViewRowState.CurrentRows);


            // Hide the columns that should not be displayed in the DataGrid
            string type = ViewState["Type"].ToString();
            if (type == "Lot")
            {
                gvSelectedSharingDetails.Columns[3].ItemStyle.CssClass = "";
                gvSelectedSharingDetails.Columns[4].ItemStyle.CssClass = "hidden";
                gvSelectedSharingDetails.Columns[3].HeaderStyle.CssClass = "";
                gvSelectedSharingDetails.Columns[4].HeaderStyle.CssClass = "hidden";
            }
            else
            {
                gvSelectedSharingDetails.Columns[4].ItemStyle.CssClass = "";
                gvSelectedSharingDetails.Columns[3].ItemStyle.CssClass = "hidden";
                gvSelectedSharingDetails.Columns[4].HeaderStyle.CssClass = "";
                gvSelectedSharingDetails.Columns[3].HeaderStyle.CssClass = "hidden";
            }

            gvSelectedSharingDetails.Columns[7].HeaderStyle.CssClass = "hidden";
            gvSelectedSharingDetails.Columns[8].HeaderStyle.CssClass = "hidden";
            gvSelectedSharingDetails.Columns[9].HeaderStyle.CssClass = "hidden";
            gvSelectedSharingDetails.Columns[10].HeaderStyle.CssClass = "hidden";

            // Create a new DataTable with only the selected columns
            DataTable newTable = dataView.ToTable();

            // Bind the new DataTable to the DataGrid
            gvSelectedSharingDetails.DataSource = newTable;
            gvSelectedSharingDetails.DataBind();

            // Set the PageIndex property to the new page index
            gvProjectList.PageIndex = gvProjectList.PageIndex;
            //Set the CommPercent back
            //CommPercent.Text = ViewState["Commission"].ToString();
        }

        protected void btnNext_ServerClick(object sender, EventArgs e)
        {
            try
            {

                step_2.Visible = false;
                step_3.Visible = true;
                step_4.Visible = false;

                //GAB Validator for VAT and WTAX
                RequiredFieldValidatorVAT.Enabled = true;
                RequiredFieldValidatorWTax.Enabled = true;

                if (rbTypeOfBusiness.SelectedIndex == 0)
                {
                    //Hide Contact Details with validators 
                    RequiredFieldValidator46.Enabled = true;
                    RequiredFieldValidator59.Enabled = true;
                    RequiredFieldValidator54.Enabled = true;
                    RequiredFieldValidator55.Enabled = true;

                    ///COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                    //RequiredFieldValidator56.Enabled = true;
                    //RequiredFieldValidator60.Enabled = true;
                    //RequiredFieldValidator57.Enabled = true;
                    //RequiredFieldValidator58.Enabled = true;

                    RequiredFieldValidator61.Enabled = false;
                    RequiredFieldValidator63.Enabled = false;
                    RequiredFieldValidator65.Enabled = false;

                    RequiredFieldValidator62.Enabled = false;
                    RequiredFieldValidator64.Enabled = false;
                    RequiredFieldValidator66.Enabled = false;
                    //End//


                    divSoleProp.Attributes.Add("class", "row show");
                    divPartnership.Attributes.Add("class", "row hidden");
                    txtPartnership.Text = "?";
                    txtSECRegNo.Text = "?";
                    txtLastName.Text = "";
                    txtFirstName.Text = "";
                    txtMiddleName.Text = "";
                    lblPartnership.Text = "Sole Proprietor";
                    RequiredFieldValidator44.Enabled = true;
                    RequiredFieldValidator47.Enabled = true;
                    reqFirstName.Enabled = true;
                    RequiredFieldValidator2.Enabled = true;
                    RequiredFieldValidator1.Enabled = true;
                    CustomValidator10.Enabled = true;
                    RequiredFieldValidator55.Enabled = true;
                    txtNickName.CausesValidation = true;

                }
                else
                {
                    divSoleProp.Attributes.Add("class", "row hidden");
                    divPartnership.Attributes.Add("class", "row show");
                    if (rbTypeOfBusiness.SelectedIndex == 1)
                    {
                        lblPartnership.Text = "Partnership";
                        txtPartnership.Attributes.Add("placeholder", "PARTNERSHIP NAME");
                        ///COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                        //RequiredFieldValidator56.Enabled = false;
                        //RequiredFieldValidator57.Enabled = false;

                        RequiredFieldValidator46.Enabled = false;
                        RequiredFieldValidator54.Enabled = false;
                        RequiredFieldValidator55.Enabled = false;
                    }
                    else
                    {
                        lblPartnership.Text = "Corporation";
                        txtPartnership.Attributes.Add("placeholder", "CORPORATION NAME");
                        ///COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                        //RequiredFieldValidator56.Enabled = true;
                        //RequiredFieldValidator57.Enabled = true;

                        RequiredFieldValidator46.Enabled = true;
                        RequiredFieldValidator54.Enabled = true;
                        RequiredFieldValidator55.Enabled = true;

                    }
                    txtPartnership.Text = "";
                    txtSECRegNo.Text = "";
                    txtLastName.Text = "?";
                    txtFirstName.Text = "?";
                    txtMiddleName.Text = "?";
                    RequiredFieldValidator44.Enabled = false;
                    RequiredFieldValidator47.Enabled = false;
                    reqFirstName.Enabled = false;
                    RequiredFieldValidator2.Enabled = false;
                    RequiredFieldValidator1.Enabled = false;
                    CustomValidator10.Enabled = false;
                    txtNickName.CausesValidation = false;

                    //Hide Contact Details with validators 
                    RequiredFieldValidator61.Enabled = false;
                    RequiredFieldValidator63.Enabled = false;
                    RequiredFieldValidator65.Enabled = false;

                    ///COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                    //RequiredFieldValidator58.Enabled = false;

                    RequiredFieldValidator62.Enabled = false;
                    RequiredFieldValidator64.Enabled = false;
                    RequiredFieldValidator66.Enabled = false;
                    //End//



                }

                //////////////////////////////
                ////DELETE MO TO AFTER TESTING//
                //SoleProp.Visible = true;
                //Partnership.Visible = true;
                //Corporation.Visible = true;
                //////////////////////////////

                ClearBrokerDetails();
                ClearSalesPersonModal();
                ClearSharingDetailsModal();
                ViewState["SalesPerson"] = null;
                ViewState["SharingDetails"] = null;

                gvStandardDocumentRequirements.DataSource = null;
                gvStandardDocumentRequirements.DataBind();
                gvSolePropDocumentRequirements.DataSource = null;
                gvSolePropDocumentRequirements.DataBind();
                gvPartnershipDocumentRequirements.DataSource = null;
                gvPartnershipDocumentRequirements.DataBind();
                gvCorporationDocumentRequirements.DataSource = null;
                gvCorporationDocumentRequirements.DataBind();

                LoadBrokerDocumentsStandard();
                LoadBrokerDocumentsSoleProp();
                LoadBrokerDocumentsPartnership();
                LoadBrokerDocumentsCorporation();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideBusinessTypeModal();", true);
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }
        }

        protected void btnNext2_ServerClick(object sender, EventArgs e)
        {
            //if (rbTypeOfBusiness.SelectedIndex == 0 && ViewState["Status"].ToString() == "ACCEPTED")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowSupplementaryDetails();", true);
            //}
            //else 
            if (rbTypeOfBusiness.SelectedIndex == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideContactDetailsWithHide();", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowSupplementaryDetails();", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideSupplementaryDetails();", true);
            }

            if (Page.IsValid)
            {
                try
                {
                    var prcLicenseExpirationDate = Convert.ToDateTime("1900-01-01");
                    if (!string.IsNullOrEmpty(txtPRCLicenseExpirationDate.Text))
                    {
                        prcLicenseExpirationDate = Convert.ToDateTime(txtPRCLicenseExpirationDate.Text);
                    }

                    var atpExpiryDate = Convert.ToDateTime("1900-01-01");
                    if (!string.IsNullOrEmpty(txtATPExpiryDate.Text))
                    {
                        atpExpiryDate = Convert.ToDateTime(txtATPExpiryDate.Text);
                    }

                    string TINNo = txtTIN.Text;
                    string BrokerId = lblBrokerID.Text;
                    string qrytinno;
                    if (BrokerId != "")
                    {
                        qrytinno = $@"SELECT ""Tax"" FROM ""OBRK"" WHERE ""Tax"" = '{TINNo}' AND ""BrokerId"" <> '{BrokerId}'";
                    }
                    else
                    {
                        qrytinno = $@"SELECT ""Tax"" FROM ""OBRK"" WHERE ""Tax"" = '{TINNo}'";
                    }
                    DataTable tinNo = hana.GetData(qrytinno, hana.GetConnection("SAOHana"));
                    if (tinNo.Rows.Count > 0) throw new Exception("TIN Number Already Exists.");

                    DataTable dt2 = new DataTable();
                    dt2.Columns.AddRange(new DataColumn[16]
                    {
                        new DataColumn("Id"),
                        new DataColumn("SalesPersonName"),
                        new DataColumn("Username"),
                        new DataColumn("EmailAddress"),
                        new DataColumn("Position"),
                        new DataColumn("PRCLicense"),
                        new DataColumn("PRCLicenseExpirationDate"),
                        new DataColumn("ATPDate"),
                        new DataColumn("TIN"),
                        new DataColumn("VATCode"),
                        new DataColumn("WTaxCode"),
                        new DataColumn("MobileNumber"),
                        new DataColumn("ValidFrom"),
                        new DataColumn("ValidTo"),
                        new DataColumn("HLURBLicenseNo"),
                        new DataColumn("PTRNo")
                    });

                    step_2.Visible = false;
                    step_3.Visible = true;
                    step_4.Visible = false;
                    step1.Attributes.Add("class", "btn btn-default btn-circle");
                    step2.Attributes.Add("class", "btn btn-info btn-circle");
                    step3.Attributes.Add("class", "btn btn-default btn-circle");

                    string id = "";
                    var ticks = DateTime.Now.Ticks;
                    var guid = Guid.NewGuid().ToString();
                    var uniqueSessionId = ticks.ToString() + guid.ToString();
                    string[] agentCode = uniqueSessionId.Split('-');
                    id = agentCode.Last();

                    string name = lblPartnership.Text == "Sole Proprietor" ? (txtFirstName.Text.Trim() + " " + txtMiddleName.Text.Trim() + " " + txtLastName.Text.Trim()).ToUpper() : txtPartnership.Text.ToUpper();

                    string qry = $@"select A.""Id"" from OSLA A 
	                                inner join BRK1 B ON B.""Id"" = A.""Id"" 
	                                inner join OBRK C on C.""BrokerId"" = B.""BrokerId""
                                where C.""BrokerId"" = '{lblBrokerID.Text}' AND ""Position"" = 'Broker' AND REPLACE(A.""SalesPerson"",'''','') = '{name.Replace("'", "")}'";
                    DataTable dtId = hana.GetData(qry, hana.GetConnection("SAOHana"));
                    string id2 = DataAccess.GetData(dtId, 0, "Id", id).ToString();

                    DataTable dt = (DataTable)ViewState["SalesPerson"];
                    if (dt.Rows.Count == 0)
                    {
                        string post;

                        post = ws.registerSalesAgent(
                                                    id,
                                                    name.Replace("'", "''''"),
                                                    txtEmailAddress.Text.Trim(),
                                                    "Broker",
                                                    txtPRCRegis.Text.Trim().ToUpper(),
                                                    Convert.ToDateTime(txtPRCLicenseExpirationDate.Text),
                                                    Convert.ToDateTime(txtATPExpiryDate.Text),
                                                    txtTIN.Text,
                                                    ddlVATCode.SelectedValue,
                                                    ddlWTAXCode.SelectedValue,
                                                    txtBusinessPhoneNo.Text,
                                                    DateTime.Now,
                                                    txtHlurbNo.Text,
                                                    txtPTRNumber.Text,
                                                    lblBrokerID.Text
                                                    );
                        if (post == "Operation completed successfully.")
                        {
                            dt.Rows.Add(
                                //Convert.ToInt32(dt.Rows.Count + 1),
                                id,
                                (rbTypeOfBusiness.SelectedIndex == 0 ? txtFirstName.Text.Trim() + " " + txtMiddleName.Text.Trim() + " " + txtLastName.Text.Trim() : txtPartnership.Text).ToUpper(),
                                "",
                                txtEmailAddress.Text.Trim().ToUpper(),
                                "Broker",
                                txtPRCRegis.Text.Trim().ToUpper(),
                                txtPRCLicenseExpirationDate.Text,
                                txtATPExpiryDate.Text,
                                txtTIN.Text,
                                ddlVATCode.SelectedValue,
                                ddlWTAXCode.SelectedValue,
                                txtBusinessPhoneNo.Text.Trim(),
                                mtxtValidFrom.Text,
                                mtxtValidTo.Text,
                                txtHlurbNo.Text,
                                txtPTRNumber.Text
                                );
                            ViewState["SalesPerson"] = dt;
                        }
                        else
                        {
                            alertMsg($"Failed to submit your documents! Please contact your administrator(Error: Sales Agent Registration - {post})", "error");
                        }
                        BindGrid();
                    }
                    else
                    {
                        //FOR UPDATING OF OSLA
                        var rows1 = dt.Select($"Id = '{id2}' AND Position = 'Broker'");
                        //foreach (GridViewRow row in gvSalesPerson.Rows)
                        if (rows1.Length > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row[0].ToString() == rows1[0].ItemArray[0].ToString() && row.RowState.ToString() != "Deleted")
                                {
                                    qry = $@"UPDATE OSLA SET 			
                                        ""Id"" = '{rows1[0].ItemArray[0].ToString()}', 
			                            ""SalesPerson"" = '{(rbTypeOfBusiness.SelectedIndex == 0 ? txtFirstName.Text.Trim() + " " + txtMiddleName.Text.Trim() + " " + txtLastName.Text.Trim() : txtPartnership.Text).ToUpper().Replace("'", "''")}',
			                            ""EmailAddress"" = '{txtEmailAddress.Text.Trim().ToUpper()}',
			                            ""Position"" = 'Broker',  
			                            ""PRCLicense"" = '{txtPRCRegis.Text.Trim().ToUpper()}',
			                            ""PRCLicenseExpirationDate"" = '{prcLicenseExpirationDate.Date.ToString("yyyy-MM-dd")}',
			                            ""ATPDateSalesPerson"" = '{atpExpiryDate.Date.ToString("yyyy-MM-dd")}',
			                            ""TIN"" = '{txtTIN.Text}',
			                            ""VATCode"" = '{ddlVATCode.SelectedValue}',
			                            ""WTaxCode"" = '{ddlWTAXCode.SelectedValue}', 
			                            ""MobileNumber"" = '{txtBusinessPhoneNo.Text.Trim()}',
			                            ""UpdateDate"" = '{DateTime.Now.ToString("yyyy-MM-dd")}',
			                            ""HLURBLicenseNo"" = '{txtHlurbNo.Text}',
                                        ""CreateBrokerID"" = '{lblBrokerID.Text}',
			                            ""PTRNo"" = '{txtPTRNumber.Text}'
                                where ""Id"" = '{id2}'";
                                    bool ret = hana.Execute(qry, hana.GetConnection("SAOHana"));
                                    if (ret)
                                    {
                                        //dt2.Rows.Add(
                                        //    rows1[0].ItemArray[0].ToString(),
                                        //    (rbTypeOfBusiness.SelectedIndex == 0 ? txtFirstName.Text.Trim() + " " + txtMiddleName.Text.Trim() + " " + txtLastName.Text.Trim() : txtPartnership.Text).ToUpper(),
                                        //    rows1[0].ItemArray[2].ToString(),
                                        //    txtEmailAddress.Text.Trim().ToUpper(),
                                        //    "Broker",
                                        //    txtPRCRegis.Text.Trim().ToUpper(),
                                        //    txtPRCLicenseExpirationDate.Text,
                                        //    txtATPExpiryDate.Text,
                                        //    txtTIN.Text,
                                        //    //"IT1",
                                        //    //"C140",
                                        //    ddlVATCode.SelectedValue,
                                        //    ddlWTAXCode.SelectedValue,
                                        //    txtBusinessPhoneNo.Text.Trim(),
                                        //    DateTime.Now.ToString("yyyy-MM-dd"),
                                        //    DateTime.Now.AddYears(1).ToString("yyyy-MM-dd"),
                                        //    txtHlurbNo.Text,
                                        //    txtPTRNumber.Text
                                        //    );
                                        row[0] = rows1[0].ItemArray[0].ToString();
                                        row[1] = (rbTypeOfBusiness.SelectedIndex == 0 ? txtFirstName.Text.Trim() + " " + txtMiddleName.Text.Trim() + " " + txtLastName.Text.Trim() : txtPartnership.Text).ToUpper();
                                        row[2] = rows1[0].ItemArray[2].ToString();
                                        row[3] = txtEmailAddress.Text.Trim().ToUpper();
                                        row[4] = "Broker";
                                        row[5] = txtPRCRegis.Text.Trim().ToUpper();
                                        row[6] = txtPRCLicenseExpirationDate.Text;
                                        row[7] = txtATPExpiryDate.Text;
                                        row[8] = txtTIN.Text;
                                        row[8] = ddlVATCode.SelectedValue;
                                        row[9] = ddlWTAXCode.SelectedValue;
                                        row[11] = txtBusinessPhoneNo.Text.Trim();
                                        row[12] = DateTime.Now.ToString("yyyy-MM-dd");
                                        row[13] = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");
                                        row[14] = txtHlurbNo.Text;
                                        row[15] = txtPTRNumber.Text;

                                        //DataTable dtSalesPerson = (DataTable)gvSalesPerson.DataSource;
                                    }
                                    else
                                    {
                                        alertMsg("Error updating sales agent", "error");
                                    }
                                }
                            }
                        }
                        //ViewState["SalesPerson"] = dt2;
                        ViewState["SalesPerson"] = dt;

                    }
                    Session["PouchDb"] = txtNext2Hidden.Text;
                    txtNext3Hidden.Text = Session["PouchDb"].ToString();


                    //2023-07-07 : DESIGNATION AUTOMATION
                    if (string.IsNullOrWhiteSpace(txtDesignation2.Text))
                    {
                        txtDesignation.Text = txtContactPersonPosition.Text;
                        txtDesignation2.Text = txtContactPersonPosition.Text;
                    }
                    else
                    {
                        txtDesignation.Text = txtDesignation2.Text;
                    }




                }
                catch (Exception ex)
                {
                    alertMsg(ex.Message, "error");
                }
            }
        }


        protected void btnAddSalesPerson_ServerClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
                try
                {
                    if (!string.IsNullOrWhiteSpace(mtxtSalesPersonID.Text))
                    {
                        string qry = "";
                        DataTable dtAdd = null;
                        qry = $@"select B.""SAPCardCode"" ""Username"",* from osla A left join brk1 B on A.""Id"" = B.""Id"" where A.""Id"" = '{mtxtSalesPersonID.Text}'";
                        dtAdd = hana.GetData(qry, hana.GetConnection("SAOHana"));
                        string PRCLicenseExpirationDate = DataAccess.GetData(dtAdd, 0, "PRCLicenseExpirationDate", "").ToString();
                        string ATPDateSalesPerson = DataAccess.GetData(dtAdd, 0, "ATPDateSalesPerson", "").ToString();
                        PRCLicenseExpirationDate = PRCLicenseExpirationDate == "" ? PRCLicenseExpirationDate : Convert.ToDateTime(PRCLicenseExpirationDate).ToString("yyyy-MM-dd");
                        ATPDateSalesPerson = ATPDateSalesPerson == "" ? ATPDateSalesPerson : Convert.ToDateTime(ATPDateSalesPerson).ToString("yyyy-MM-dd");
                        DataTable dt = (DataTable)ViewState["SalesPerson"];
                        if (ViewState["AddOrEditSalesPerson"].ToString() == "Add")
                        {
                            if (dt.AsEnumerable().Where(x => x.RowState != DataRowState.Deleted ? x[0].ToString() == mtxtSalesPersonID.Text : false).Any())
                            {
                                alertMsg("Sales Person Already Exist!", "warning");
                            }
                            //else if ("Broker" == mtxtSalesPersonPosition.Text)
                            //{
                            //    alertMsg("Sales Person with Broker Position Already Exist in your List!", "warning");
                            //}
                            else
                            {
                                dt.Rows.Add(
                                       mtxtSalesPersonID.Text,
                                       mtxtSalesPerson.Text.Trim().ToUpper(),
                                       DataAccess.GetData(dtAdd, 0, "Username", "").ToString().Trim(),
                                       DataAccess.GetData(dtAdd, 0, "EmailAddress", "").ToString().Trim(),
                                       DataAccess.GetData(dtAdd, 0, "Position", "").ToString().Trim(),
                                       DataAccess.GetData(dtAdd, 0, "PRCLicense", "").ToString().Trim(),
                                       Convert.ToDateTime(DataAccess.GetData(dtAdd, 0, "PRCLicenseExpirationDate", "")).ToString("yyyy-MM-dd"),
                                       Convert.ToDateTime(DataAccess.GetData(dtAdd, 0, "ATPDateSalesPerson", "")).ToString("yyyy-MM-dd"),
                                       DataAccess.GetData(dtAdd, 0, "TIN", "").ToString().Trim(),
                                       DataAccess.GetData(dtAdd, 0, "VATCode", "").ToString().Trim(),
                                       DataAccess.GetData(dtAdd, 0, "WTaxCode", "").ToString().Trim(),
                                       DataAccess.GetData(dtAdd, 0, "MobileNumber", "").ToString().Trim(),
                                       Convert.ToDateTime(mtxtValidFrom.Text).ToString("yyyy-MM-dd"),
                                       Convert.ToDateTime(mtxtValidTo.Text).ToString("yyyy-MM-dd"),
                                       DataAccess.GetData(dtAdd, 0, "HLURBLicenseNo", "").ToString().Trim(),
                                       DataAccess.GetData(dtAdd, 0, "PTRNo", "").ToString().Trim());
                                ViewState["SalesPerson"] = dt;
                                BindGrid();
                                ClearSalesPersonModal();
                                alertMsg("Sales Person Added Successfully!", "info");
                            }
                        }
                        else
                        {
                            foreach (GridViewRow row in gvSalesPerson.Rows)
                            {
                                if (row.Cells[0].Text == mtxtID.Text)
                                {
                                    dt.Rows.RemoveAt(row.RowIndex);
                                    //dt.Rows.RemoveAt(gvSalesPerson.Rows.Count - 1);
                                    dt.AcceptChanges();
                                    gvSalesPerson.DataSource = dt;
                                    gvSalesPerson.DataBind();

                                    dt.Rows.Add(
                                      mtxtSalesPersonID.Text,
                                      mtxtSalesPerson.Text.Trim(),
                                      DataAccess.GetData(dtAdd, 0, "Username", "").ToString().Trim(),
                                      DataAccess.GetData(dtAdd, 0, "EmailAddress", "").ToString().Trim(),
                                      DataAccess.GetData(dtAdd, 0, "Position", "").ToString().Trim(),
                                      DataAccess.GetData(dtAdd, 0, "PRCLicense", "").ToString().Trim(),
                                      Convert.ToDateTime(DataAccess.GetData(dtAdd, 0, "PRCLicenseExpirationDate", "")).ToString("yyyy-MM-dd"),
                                      Convert.ToDateTime(DataAccess.GetData(dtAdd, 0, "ATPDateSalesPerson", "")).ToString("yyyy-MM-dd"),
                                      DataAccess.GetData(dtAdd, 0, "TIN", "").ToString().Trim(),
                                      DataAccess.GetData(dtAdd, 0, "VATCode", "").ToString().Trim(),
                                      DataAccess.GetData(dtAdd, 0, "WTaxCode", "").ToString().Trim(),
                                      DataAccess.GetData(dtAdd, 0, "MobileNumber", "").ToString().Trim(),
                                      Convert.ToDateTime(mtxtValidFrom.Text).ToString("yyyy-MM-dd"),
                                      Convert.ToDateTime(mtxtValidTo.Text).ToString("yyyy-MM-dd"),
                                      DataAccess.GetData(dtAdd, 0, "HLURBLicenseNo", "").ToString().Trim(),
                                      DataAccess.GetData(dtAdd, 0, "PTRNo", "").ToString().Trim());
                                    dt.DefaultView.Sort = "Id asc";
                                    dt = dt.DefaultView.ToTable();
                                    ViewState["SalesPerson"] = dt;
                                    gvSalesPerson.EditIndex = -1;
                                    BindGrid();
                                    ClearSalesPersonModal();
                                }

                            }
                            alertMsg("Sales Person Updated Successfully!", "info");
                        }

                        mtxtValidFrom.Text = null;
                        mtxtValidTo.Text = null;

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideAddSalesPersonModal();", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideAddSalesPersonModal();", true);
                        alertMsg("No sales person selected.", "error");
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideAddSalesPersonModal();", true);
                    alertMsg(ex.Message, "error");
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

            if (type == "warning" || type == "error")
            {
                ErrorHandler.Log(Session["UserId"]?.ToString()
                    , "BrokerApprove"
                   , "Medium"
                   , Message
                   , $"Broker Approve : Type of {type}.");
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "ShowAlert();", true);
        }

        protected void btnNext3_ServerClick1(object sender, EventArgs e)
        {
            //Sum should be 7% for Lot Only and 5% for House and Lot, else, block
            DataTable dt2 = (DataTable)ViewState["SharingDetails"];
            double lotpercentage = 0;
            double houseandlotpercentage = 0;
            DataTable dt = (DataTable)ViewState["SalesPerson"];
            bool proceed = false;

            string pouchDBString = Session["PouchDb"].ToString();
            string pouchDbData = pouchDBString;

            var pouchdata = JsonConvert.DeserializeObject<List<SharingDetails>>(pouchDbData);

            // Create the new DataTable
            DataTable dt5 = new DataTable();

            // Define the columns for the DataTable
            dt5.Columns.Add("Id", typeof(int));
            dt5.Columns.Add("SalesPersonId", typeof(string));
            dt5.Columns.Add("PositionSharedDetails", typeof(string));
            dt5.Columns.Add("SalesPersonNameSharedDetails", typeof(string));
            dt5.Columns.Add("PercentageSharedDetails", typeof(double));
            dt5.Columns.Add("CreateDate", typeof(DateTime));
            dt5.Columns.Add("OslaId", typeof(string));
            dt5.Columns.Add("HouseandLotSharingDetails", typeof(double));
            dt5.Columns.Add("ProjectCode", typeof(string));
            dt5.Columns.Add("ProjectName", typeof(string));
            dt5.Columns.Add("CommissionPercentage", typeof(double));
            dt5.Columns.Add("Type", typeof(string));
            dt5.Columns.Add("_id", typeof(string));


            //2023-07-20 : IF POUCHDATA IS NOT NULL
            if (pouchdata != null)
            {
                // Loop through the pouchdata and add each item to the DataTable
                foreach (var item in pouchdata)
                {
                    // Create a new row for the DataTable
                    DataRow newRow = dt5.NewRow();

                    // Set the values for the columns in the new row
                    newRow["Id"] = item.Id;
                    newRow["SalesPersonId"] = item.SalesPersonId;
                    newRow["PositionSharedDetails"] = item.PositionSharedDetails;
                    newRow["SalesPersonNameSharedDetails"] = item.SalesPersonNameSharedDetails;
                    newRow["PercentageSharedDetails"] = item.PercentageSharedDetails;
                    newRow["CreateDate"] = item.CreateDate;
                    newRow["OslaId"] = item.OslaId;
                    newRow["HouseandLotSharingDetails"] = item.HouseandLotSharingDetails;
                    newRow["ProjectCode"] = item.ProjectCode;
                    newRow["ProjectName"] = item.ProjectName;
                    newRow["CommissionPercentage"] = item.CommissionPercentage;
                    newRow["Type"] = item.Type;
                    newRow["_id"] = item._id;

                    // Add the new row to the DataTable
                    dt5.Rows.Add(newRow);
                }

                ViewState["SharingDetailsPouch"] = dt5;

            }


            //####################               COMMENTED AS REQUESTED BY SIR CHRISTIAN MARKETING 20220630_1053            ####################//         

            //foreach (DataRow row1 in dt.Rows)
            //{
            //    lotpercentage = 0;
            //    houseandlotpercentage = 0;
            //    ViewState["SharingID"] = row1["Id"].ToString();
            //    //foreach (DataRow row in dt2.Rows)
            //    if (dt2.Select($"SalesPersonId = '{ViewState["SharingID"]}'").Length != 0)
            //    {
            //        for (int i = (dt2.Select($"SalesPersonId = '{ViewState["SharingID"]}'").CopyToDataTable().Rows.Count) - 1; i >= 0; i--)
            //        {
            //            DataRow dr = dt2.Select($"SalesPersonId = '{ViewState["SharingID"]}'").CopyToDataTable().Rows[i];
            //            if (dr.RowState != DataRowState.Deleted)
            //            {
            //                lotpercentage += Convert.ToDouble(DBNull.Value.Equals(dr["LotOnlyPercentage"]) ? 0 : dr["LotOnlyPercentage"]);
            //                houseandlotpercentage += Convert.ToDouble(DBNull.Value.Equals(dr["HouseandLotPercentage"]) ? 0 : dr["HouseandLotPercentage"]);
            //            }
            //        }
            //        if ((lotpercentage != 7 && lotpercentage != 0) || houseandlotpercentage != 5 && houseandlotpercentage != 0)
            //        {
            //            proceed = false;
            //            break;
            //        }
            //        else
            //        {
            //            proceed = true;
            //        }
            //    }
            //    else
            //    {
            //        lotpercentage = 0;
            //        houseandlotpercentage = 0;
            //        proceed = true;
            //    }
            //}

            ////if ((lotpercentage == 7 || lotpercentage == 0) && (houseandlotpercentage == 5 || houseandlotpercentage == 0))
            //if (proceed)
            //{
            step_2.Visible = false;
            step_3.Visible = false;
            step_4.Visible = true;
            step1.Attributes.Add("class", "btn btn-default btn-circle");
            step2.Attributes.Add("class", "btn btn-default btn-circle");
            step3.Attributes.Add("class", "btn btn-info btn-circle");

            if (rbTypeOfBusiness.SelectedIndex == 0)
            {
                SoleProp.Visible = true;
                Partnership.Visible = false;
                Corporation.Visible = false;
            }
            else if (rbTypeOfBusiness.SelectedIndex == 1)
            {
                SoleProp.Visible = false;
                Partnership.Visible = true;
                Corporation.Visible = false;
            }
            else
            {
                SoleProp.Visible = false;
                Partnership.Visible = false;
                Corporation.Visible = true;
            }

            //}
            //else
            //{
            //    alertMsg("Error: Sum should be 7% for Lot Only and 5% for House and Lot", "error");
            //}
        }

        protected void btnPrevious2_ServerClick(object sender, EventArgs e)
        {
            //if (rbTypeOfBusiness.SelectedIndex == 0 && ViewState["Status"].ToString() == "ACCEPTED")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowSupplementaryDetails();", true);
            //}
            //else 
            if (rbTypeOfBusiness.SelectedIndex == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideContactDetailsWithHide();", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowSupplementaryDetails();", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideSupplementaryDetails();", true);
            }
            step_2.Visible = true;
            step_3.Visible = false;
            step_4.Visible = false;
            step1.Attributes.Add("class", "btn btn-info btn-circle");
            step2.Attributes.Add("class", "btn btn-default btn-circle");
            step3.Attributes.Add("class", "btn btn-default btn-circle");

            Session["PouchDb"] = txtNext3Hidden.Text;
            txtNext2Hidden.Text = Session["PouchDb"].ToString();

        }

        protected void btnPrevious3_ServerClick(object sender, EventArgs e)
        {
            step_2.Visible = false;
            step_3.Visible = true;
            step_4.Visible = false;
            step1.Attributes.Add("class", "btn btn-default btn-circle");
            step2.Attributes.Add("class", "btn btn-info btn-circle");
            step3.Attributes.Add("class", "btn btn-default btn-circle");
        }



        void closeAddSharingModal()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "HideAddSharingDetailsModal", "HideAddSharingDetailsModal();", true);
        }

        protected void ClearSalesPersonModal()
        {
            // modalAddRow
            mtxtID.Text = "";
            mtxtSalesPersonID.Text = "";
            mtxtSalesPerson.Text = "";
            mtxtValidFrom.Text = null;
            mtxtValidTo.Text = null;
        }

        protected void ClearRegisterSalesPerson()
        {
            mtxtRegisterSalesPersonName.Text = "";
            mtxtEmail.Text = "";
            ddPosition.SelectedIndex = 0;
            mtxtPRCLicense.Text = "";
            mtxtPRCLicenseExpirationDate.Text = null;
            mtxtATPDate.Text = null;
            mtxtTIN.Text = "";
            ddlVATCode2.SelectedIndex = -1;
            ddlWTAXCode2.SelectedIndex = -1;
            ddlWTAXCode.SelectedIndex = -1;
            mtxtMobile.Text = "";
            txtSalesPTRNo.Text = "";
            txtSalesHLURBNo.Text = "";
        }

        protected void ClearSharingDetailsModal()
        {
            //Sharing
            mddPositionShare.SelectedIndex = 0;
            mtxtSalesPersonShare.Text = "";
            mtxtPecent.Text = "";
            mtxtHouseAndLotPecent.Text = "";
            //mtxtValidFrom.Text = null;
            //mtxtValidTo.Text = null;
        }

        protected void ClearBrokerDetails()
        {
            txtATPExpiryDate.Text = null;
            txtLastName.Text = "";
            txtFirstName.Text = "";
            txtMiddleName.Text = "";
            txtNickName.Text = "";
            txtPartnership.Text = "";
            txtSECRegNo.Text = "";

            txtAddress.Text = "";
            txtCity.Text = "";
            txtZipCode.Text = "";
            txtNatureOfBusiness.Text = "";
            txtBusinessName.Text = "";
            txtBusinessAddress.Text = "";
            txtBusinessZipCode.Text = "";
            txtBusinessPhoneNo.Text = "";
            txtFaxNo.Text = "";
            txtEmailAddress.Text = "";
            ddlLocation.SelectedValue = "-1";
            dtDateOfBirth.Text = null;
            txtPlaceOfBirth.Text = "";
            txtReligion.Text = "";
            txtCitizenship.Value = "";
            txtTIN.Text = "";
            //txtSSS.Text = "";
            //txtPassport.Text = "";
            //txtPassportValid.Text = null;
            //dpIssuedBy.SelectedIndex = 1;
            //txtPlaceIssued.Text = "";

            txtPRCRegis.Text = "";
            txtPRCLicenseExpirationDate.Text = null;
            txtPTRNumber.Text = "";
            txtPTRValidFrom.Text = null;
            txtPTRValidTo.Text = null;
        }

        protected void btnUpdate_ServerClick(object sender, EventArgs e)
        {
            try
            {
                DataTable dt2 = (DataTable)ViewState["SharingDetails"];


                foreach (GridViewRow row in gvShareDetails.Rows)
                {
                    if (row.Cells[0].Text == mtxtID.Text)
                    {
                        dt2.Rows.RemoveAt(gvShareDetails.Rows.Count - 1);
                        dt2.AcceptChanges();
                        gvShareDetails.DataSource = dt2;
                        gvShareDetails.DataBind();

                        dt2.Rows.Add(
                                     mtxtID.Text,
                                     ViewState["SharingID"].ToString(),
                                     mddPositionShare.SelectedItem.Text,
                                     mtxtSalesPersonShare.Text.Trim(),
                                     Convert.ToDouble(mtxtPecent.Text.Trim()),
                                     Convert.ToDouble(mtxtHouseAndLotPecent.Text.Trim()),
                                      mtxtSalesPersonShareID.Text
                                     //,
                                     //mtxtValidFrom.Text,
                                     //mtxtValidTo.Text
                                     );

                        ViewState["SharingDetails"] = dt2;
                        gvShareDetails.EditIndex = -1;
                        BindGrid2(ViewState["SharingID"].ToString());
                        ClearSalesPersonModal();
                    }
                }



                step_2.Visible = false;
                step_3.Visible = true;
                step_4.Visible = false;

                alertMsg("Updated Sharing Details Successfully!", "Success");
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }
        }

        protected void BtnPouchDbDataViewHidden_ServerClick(object sender, EventArgs e)
        {
            try
            {
                //GAB 05/03/2023 pouchDb Data
                string pouchDbData = txtPouchDbDataViewHidden.Value;

                var pouchdata = JsonConvert.DeserializeObject<List<SharingDetails>>(pouchDbData);
                BindGridSharingDetailsFromPouchDb(pouchdata);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "ComparingTables()", "ComparingTables();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ChangeCommPercent()", "ChangeCommPercent()", true);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowErrorMessageModal(ex.Message);", true);
                ErrorHandler.Log(Session["UserId"]?.ToString()
                    , "BrokerApprove"
                   , "Medium"
                   , ex.Message
                   , $"Broker Approve : Type of error.");
            }
        }

        protected void btnBackToProjList_ServerClick(object sender, EventArgs e)
        {
            try
            {
                //GAB 05/04/2023 pouchDb Data
                string pouchDbData = txtPouchDbDataDeleteHidden.Value;

                //CommPercent.Text = ViewState["Commission"].ToString();

                var pouchdata = JsonConvert.DeserializeObject<List<SharingDetails>>(pouchDbData);
                BindGridSharingAfterDeleteDetailsFromPouchDb(pouchdata);

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "ComparingTables()", "ComparingTables();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ChangeCommPercent()", "ChangeCommPercent()", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowErrorMessageModal(ex.Message);", true);
                ErrorHandler.Log(Session["UserId"]?.ToString()
                    , "BrokerApprove"
                   , "Medium"
                   , ex.Message
                   , $"Broker Approve : Type of error.");
            }
        }

        protected void BindGridSharingAfterDeleteDetailsFromPouchDb(List<SharingDetails> pouchdata)
        {
            // Create the new DataTable
            DataTable dt5 = new DataTable();

            // Define the columns for the DataTable
            dt5.Columns.Add("Id", typeof(int));
            dt5.Columns.Add("SalesPersonId", typeof(string));
            dt5.Columns.Add("PositionSharedDetails", typeof(string));
            dt5.Columns.Add("SalesPersonNameSharedDetails", typeof(string));
            dt5.Columns.Add("PercentageSharedDetails", typeof(double));
            dt5.Columns.Add("CreateDate", typeof(DateTime));
            dt5.Columns.Add("OslaId", typeof(string));
            dt5.Columns.Add("HouseandLotSharingDetails", typeof(double));
            dt5.Columns.Add("ProjectCode", typeof(string));
            dt5.Columns.Add("ProjectName", typeof(string));
            dt5.Columns.Add("CommissionPercentage", typeof(double));
            dt5.Columns.Add("Type", typeof(string));
            dt5.Columns.Add("_id", typeof(string));

            // Loop through the pouchdata and add each item to the DataTable
            foreach (var item in pouchdata.Where(x => x.SalesPersonId == (ViewState["SharingID"]).ToString() && x.Type == (ViewState["Type"]).ToString()))
            {
                // Create a new row for the DataTable
                DataRow newRow = dt5.NewRow();

                // Set the values for the columns in the new row
                newRow["Id"] = item.Id;
                newRow["SalesPersonId"] = item.SalesPersonId;
                newRow["PositionSharedDetails"] = item.PositionSharedDetails;
                newRow["SalesPersonNameSharedDetails"] = item.SalesPersonNameSharedDetails;
                newRow["PercentageSharedDetails"] = item.PercentageSharedDetails;
                newRow["CreateDate"] = item.CreateDate;
                newRow["OslaId"] = item.OslaId;
                newRow["HouseandLotSharingDetails"] = item.HouseandLotSharingDetails;
                newRow["ProjectCode"] = item.ProjectCode;
                newRow["ProjectName"] = item.ProjectName;
                newRow["CommissionPercentage"] = item.CommissionPercentage;
                newRow["Type"] = item.Type;
                newRow["_id"] = item._id;


                // Add the new row to the DataTable
                dt5.Rows.Add(newRow);
            }

            // Create a DataView from the original DataTable
            DataView dataView = new DataView(dt5);

            dataView.Sort = "ProjectCode ASC";

            // Select only the columns you want to display
            string[] displayColumns = { "ProjectCode", "ProjectName", "CommissionPercentage", "SalesPersonNameSharedDetails", "HouseandLotSharingDetails", "PercentageSharedDetails", "OslaId", "SalesPersonId", "PositionSharedDetails", "_id" };
            dataView = new DataView(dataView.ToTable(false, displayColumns), "", "", DataViewRowState.CurrentRows);

            // Hide the columns that should not be displayed in the DataGrid
            string type = ViewState["Type"].ToString();
            if (type == "Lot")
            {
                gvSelectedSharingDetails.Columns[3].ItemStyle.CssClass = "";
                gvSelectedSharingDetails.Columns[4].ItemStyle.CssClass = "hidden";
                gvSelectedSharingDetails.Columns[3].HeaderStyle.CssClass = "";
                gvSelectedSharingDetails.Columns[4].HeaderStyle.CssClass = "hidden";
            }
            else
            {
                gvSelectedSharingDetails.Columns[4].ItemStyle.CssClass = "";
                gvSelectedSharingDetails.Columns[3].ItemStyle.CssClass = "hidden";
                gvSelectedSharingDetails.Columns[4].HeaderStyle.CssClass = "";
                gvSelectedSharingDetails.Columns[3].HeaderStyle.CssClass = "hidden";
            }

            gvSelectedSharingDetails.Columns[7].HeaderStyle.CssClass = "hidden";
            gvSelectedSharingDetails.Columns[8].HeaderStyle.CssClass = "hidden";
            gvSelectedSharingDetails.Columns[9].HeaderStyle.CssClass = "hidden";
            gvSelectedSharingDetails.Columns[10].HeaderStyle.CssClass = "hidden";

            // Create a new DataTable with only the selected columns
            DataTable newTable = dataView.ToTable();

            // Bind the new DataTable to the DataGrid
            gvSelectedSharingDetails.DataSource = newTable;
            gvSelectedSharingDetails.DataBind();

            // Set the PageIndex property to the new page index
            gvProjectList.PageIndex = gvProjectList.PageIndex;
        }

        protected void btnSubmitID_ServerClick(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }
        }

        void LoadDataheader(string BrokerID)
        {
            try
            {
                lblBrokerID.Text = BrokerID;
                // POPULAT BROKER DETAILS -- HEADER
                DataTable dt = (DataTable)ViewState["ValidId"];
                DataTable dt2 = (DataTable)ViewState["ValidIdPrev"];
                string qry = $@"SELECT * FROM ""OBRK"" WHERE ""BrokerId"" = '{BrokerID}'";
                DataTable dtBrokerHeader = hana.GetData(qry, hana.GetConnection("SAOHana"));
                ViewState["dtBrokerHeader"] = dtBrokerHeader;
                ViewState["InternalCode"] = "";
                ViewState["Status"] = DataAccess.GetData(dtBrokerHeader, 0, "Status", "").ToString();
                ViewState["StatusBRK"] = DataAccess.GetData(dtBrokerHeader, 0, "Status", "").ToString();
                ViewState["CardCode"] = DataAccess.GetData(dtBrokerHeader, 0, "SAPCardCode", "").ToString();
                btnCreateusers.InnerText = ViewState["CardCode"].ToString() == "" ? "Create Users" : "Update Users";

                //GET InternalCode IF THE BROKER HAS SapCardCode
                string sapcardcode = "";
                sapcardcode = DataAccess.GetData(dtBrokerHeader, 0, "SapCardCode", "").ToString();
                if (sapcardcode != "")
                {
                    string qry1 = $@"select ""CntctCode"" from OCPR where ""CardCode"" = '{sapcardcode}'";
                    DataTable dtintcode = hana.GetData(qry1, hana.GetConnection("SAPHana"));
                    ViewState["InternalCode"] = DataAccess.GetData(dtintcode, 0, "CntctCode", "").ToString();
                }

                if (dtBrokerHeader.Rows.Count != 0)
                {
                    if (!string.IsNullOrWhiteSpace(DataAccess.GetData(dtBrokerHeader, 0, "ApprovalDate", "").ToString()))
                    {
                        lblApprovalDate.Text = Convert.ToDateTime(DataAccess.GetData(dtBrokerHeader, 0, "ApprovalDate", "").ToString()).ToString("MMMM dd, yyyy");
                        approvalDate.Visible = true;
                    }
                    else
                    {
                        approvalDate.Visible = false;
                    }


                    ViewState["businesstype"] = "";
                    string typeOfBusiness = DataAccess.GetData(dtBrokerHeader, 0, "TypeOfBusiness", "0").ToString().Trim().ToUpper();
                    ViewState["businesstype"] = typeOfBusiness;

                    //GAB Validator for VAT and WTAX
                    RequiredFieldValidatorVAT.Enabled = true;
                    RequiredFieldValidatorWTax.Enabled = true;

                    if (typeOfBusiness == "SOLE PROPRIETOR")
                    {

                        //Hide Contact Details with validators
                        //if (ViewState["Status"].ToString() != "ACCEPTED")
                        //{
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideContactDetailsWithHide();", true);
                        //}
                        RequiredFieldValidator67.Enabled = false;
                        RequiredFieldValidator68.Enabled = false;
                        RequiredFieldValidator69.Enabled = false;
                        //RequiredFieldValidator70.Enabled = false;
                        RequiredFieldValidator71.Enabled = false;
                        RequiredFieldValidator72.Enabled = false;
                        RequiredFieldValidator76.Enabled = false;
                        //RequiredFieldValidator73.Enabled = false;
                        //RequiredFieldValidator74.Enabled = false;
                        //RequiredFieldValidator75.Enabled = false;
                        RequiredFieldValidator77.Enabled = false;
                        //End//

                        rbTypeOfBusiness.SelectedIndex = 0;
                        txtLastName.Text = DataAccess.GetData(dtBrokerHeader, 0, "LastName", "0").ToString().Trim();
                        txtMiddleName.Text = DataAccess.GetData(dtBrokerHeader, 0, "MiddleName", "0").ToString().Trim();
                        txtFirstName.Text = DataAccess.GetData(dtBrokerHeader, 0, "FirstName", "0").ToString().Trim();
                        txtNickName.Text = DataAccess.GetData(dtBrokerHeader, 0, "NickName", "0").ToString().Trim();
                        txtPartnership.Text = "?";
                        txtSECRegNo.Text = "?";
                        //txtLastName.Text = "";
                        //txtFirstName.Text = "";
                        //txtMiddleName.Text = "";
                        lblPartnership.Text = "Sole Proprietor";
                        RequiredFieldValidator44.Enabled = true;
                        RequiredFieldValidator47.Enabled = true;
                        reqFirstName.Enabled = true;
                        RequiredFieldValidator2.Enabled = true;
                        RequiredFieldValidator1.Enabled = true;
                        RequiredFieldValidator46.Enabled = true;
                        RequiredFieldValidator54.Enabled = true;

                        ///COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                        //RequiredFieldValidator60.Enabled = true;
                        //RequiredFieldValidator58.Enabled = true;
                        //RequiredFieldValidator56.Enabled = true;
                        //RequiredFieldValidator57.Enabled = true;

                        RequiredFieldValidator61.Enabled = false;
                        CustomValidator10.Enabled = true;
                        RequiredFieldValidator55.Enabled = true;
                        txtNickName.CausesValidation = true;
                        RequiredFieldValidator24.Enabled = true;
                        CustomValidator3.Enabled = true;
                        RequiredFieldValidator13.Enabled = true;
                        //RequiredFieldValidator14.Enabled = true;
                        //RequiredFieldValidator15.Enabled = true;
                        //RequiredFieldValidator17.Enabled = true;
                        //rfvPass.Enabled = true;
                        //RequiredFieldValidator41.Enabled = true;
                        //CustomValidator4.Enabled = true;
                        //RequiredFieldValidator45.Enabled = true;
                        //CustomValidator9.Enabled = true;
                        //rfvPlaceIssued.Enabled = true;
                        txtResidenceNo.CausesValidation = true;
                        txtMobileNo.CausesValidation = true;
                        txtSpouse.CausesValidation = true;

                        RequiredFieldValidator79.Enabled = false;

                        //SHOW VALID IDS
                        validids.Visible = true;

                        //ddContactPosition.SelectedValue = DataAccess.GetData(dtBrokerHeader, 0, "PositionContact", "---Select Valid ID---").ToString();


                        LoadUploadedFromDatabase(gvSolePropDocumentRequirements, "Sole Proprietor", BrokerID, "lblFileName2", "btnPreview2", "btnRemove2", "txtSolePropDocumentRequirementsComments", "chkSole", "txtSolePropDocumentRequirementsDateIssued", "rvSolePropDocumentRequirementsDateIssued", "cvSolePropDocumentRequirementsDateIssued");
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "showTaxDetails()", "showTaxDetails()", true);
                    }
                    else if (typeOfBusiness == "PARTNERSHIP")
                    {
                        rbTypeOfBusiness.SelectedIndex = 1;
                        txtLastName.Text = "?";
                        txtMiddleName.Text = "?";
                        txtFirstName.Text = "?";
                        txtNickName.Text = "?";
                        RequiredFieldValidator44.Enabled = false;
                        RequiredFieldValidator47.Enabled = false;
                        reqFirstName.Enabled = false;
                        RequiredFieldValidator2.Enabled = false;
                        RequiredFieldValidator1.Enabled = false;
                        RequiredFieldValidator46.Enabled = false;

                        ///COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                        //RequiredFieldValidator57.Enabled = false;
                        //RequiredFieldValidator60.Enabled = false;
                        //RequiredFieldValidator56.Enabled = false;
                        //RequiredFieldValidator61.Enabled = false;
                        //RequiredFieldValidator58.Enabled = false;

                        RequiredFieldValidator54.Enabled = false;
                        CustomValidator10.Enabled = false;
                        RequiredFieldValidator55.Enabled = false;
                        txtNickName.CausesValidation = false;
                        RequiredFieldValidator24.Enabled = false;
                        CustomValidator3.Enabled = false;
                        RequiredFieldValidator13.Enabled = false;
                        //RequiredFieldValidator14.Enabled = false;
                        //RequiredFieldValidator15.Enabled = false;
                        //RequiredFieldValidator17.Enabled = false;
                        //rfvPass.Enabled = false;
                        //RequiredFieldValidator41.Enabled = false;
                        //CustomValidator4.Enabled = false;
                        //RequiredFieldValidator45.Enabled = false;
                        //CustomValidator9.Enabled = false;
                        //rfvPlaceIssued.Enabled = false;
                        txtResidenceNo.CausesValidation = false;
                        txtMobileNo.CausesValidation = false;
                        txtSpouse.CausesValidation = false;

                        RequiredFieldValidator79.Enabled = true;

                        //HIDE POSITION WHEN NOT SALES PERSON
                        contactpersonposition.Visible = false;
                        ddContactPosition.SelectedValue = "2";

                        //HIDE VALID IDS
                        validids.Visible = false;

                        txtPartnership.Text = DataAccess.GetData(dtBrokerHeader, 0, "Partnership", "0").ToString().Trim();
                        txtSECRegNo.Text = DataAccess.GetData(dtBrokerHeader, 0, "SECRegNo", "0").ToString().Trim();
                        LoadUploadedFromDatabase(gvPartnershipDocumentRequirements, "Partnership", BrokerID, "lblFileName3", "btnPreview3", "btnRemove3", "txtPartnershipDocumentRequirementsComments", "chkPartner", "txtPartnershipDocumentRequirementsDateIssued", "rvPartnershipDocumentRequirementsDateIssued", "cvPartnershipDocumentRequirementsDateIssued");
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "hideTaxDetails()", "hideTaxDetails()", true);
                    }
                    else
                    {
                        rbTypeOfBusiness.SelectedIndex = 2;
                        txtLastName.Text = "?";
                        txtMiddleName.Text = "?";
                        txtFirstName.Text = "?";
                        txtNickName.Text = "?";
                        RequiredFieldValidator44.Enabled = false;
                        RequiredFieldValidator47.Enabled = false;
                        reqFirstName.Enabled = false;
                        RequiredFieldValidator2.Enabled = false;
                        RequiredFieldValidator1.Enabled = false;

                        ///COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                        //RequiredFieldValidator57.Enabled = true;
                        //RequiredFieldValidator60.Enabled = false;

                        RequiredFieldValidator61.Enabled = false;



                        ///COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                        //RequiredFieldValidator56.Enabled = true;
                        //RequiredFieldValidator58.Enabled = true;
                        RequiredFieldValidator46.Enabled = true;
                        RequiredFieldValidator54.Enabled = true;
                        CustomValidator10.Enabled = true;
                        RequiredFieldValidator55.Enabled = true;
                        txtNickName.CausesValidation = false;
                        RequiredFieldValidator24.Enabled = false;
                        CustomValidator3.Enabled = false;
                        RequiredFieldValidator13.Enabled = false;
                        //RequiredFieldValidator14.Enabled = false;
                        //RequiredFieldValidator15.Enabled = false;
                        //RequiredFieldValidator17.Enabled = false;
                        //rfvPass.Enabled = false;
                        //RequiredFieldValidator41.Enabled = false;
                        //CustomValidator4.Enabled = false;
                        //RequiredFieldValidator45.Enabled = false;
                        //CustomValidator9.Enabled = false;
                        //rfvPlaceIssued.Enabled = false;
                        txtResidenceNo.CausesValidation = false;
                        txtMobileNo.CausesValidation = false;
                        txtSpouse.CausesValidation = false;

                        RequiredFieldValidator79.Enabled = true;

                        //HIDE POSITION WHEN NOT SALES PERSON
                        contactpersonposition.Visible = false;
                        ddContactPosition.SelectedValue = "2";

                        //SHOW VALID IDS
                        validids.Visible = true;

                        txtPartnership.Text = DataAccess.GetData(dtBrokerHeader, 0, "Partnership", "0").ToString().Trim();
                        txtSECRegNo.Text = DataAccess.GetData(dtBrokerHeader, 0, "SECRegNo", "0").ToString().Trim();
                        LoadUploadedFromDatabase(gvCorporationDocumentRequirements, "Corporation", BrokerID, "lblFileName4", "btnPreview4", "btnRemove4", "txtCorporationDocumentRequirementsComments", "chkCorp", "txtCorporationDocumentRequirementsDateIssued", "rvCorporationDocumentRequirementsDateIssued", "cvCorporationDocumentRequirementsDateIssued");
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "hideTaxDetails()", "hideTaxDetails()", true);
                    }

                    txtATPExpiryDate.Text = Convert.ToDateTime(DataAccess.GetData(dtBrokerHeader, 0, "ATPDate", "1900-01-01")).ToString("yyyy-MM-dd");
                    txtAddress.Text = DataAccess.GetData(dtBrokerHeader, 0, "Address", "0").ToString();
                    txtCity.Text = DataAccess.GetData(dtBrokerHeader, 0, "City", "0").ToString();
                    txtZipCode.Text = DataAccess.GetData(dtBrokerHeader, 0, "ZipCode", "0").ToString();
                    txtNatureOfBusiness.Text = DataAccess.GetData(dtBrokerHeader, 0, "NatureOfBusiness", "0").ToString();
                    txtBusinessName.Text = DataAccess.GetData(dtBrokerHeader, 0, "BusinessName", "0").ToString();
                    txtBusinessAddress.Text = DataAccess.GetData(dtBrokerHeader, 0, "BusinessAddress", "0").ToString();
                    txtBusinessZipCode.Text = DataAccess.GetData(dtBrokerHeader, 0, "BusinessZipCode", "0").ToString();
                    txtBusinessPhoneNo.Text = DataAccess.GetData(dtBrokerHeader, 0, "BusinessPhoneNo", "0").ToString();
                    txtFaxNo.Text = DataAccess.GetData(dtBrokerHeader, 0, "FaxNo", "0").ToString();
                    txtEmailAddress.Text = DataAccess.GetData(dtBrokerHeader, 0, "EmailAddress", "0").ToString().Trim();
                    ddlLocation.SelectedValue = DataAccess.GetData(dtBrokerHeader, 0, "Location", "-1").ToString();
                    dtDateOfBirth.Text = Convert.ToDateTime(DataAccess.GetData(dtBrokerHeader, 0, "Birthday", "1900-01-01")).ToString("yyyy-MM-dd");
                    txtPlaceOfBirth.Text = DataAccess.GetData(dtBrokerHeader, 0, "PlaceOfBirth", "0").ToString();
                    txtReligion.Text = DataAccess.GetData(dtBrokerHeader, 0, "Religion", "0").ToString();
                    txtCorpReligion.Text = DataAccess.GetData(dtBrokerHeader, 0, "Religion", "0").ToString();
                    txtCitizenship.Value = DataAccess.GetData(dtBrokerHeader, 0, "Citizenship", "filipino").ToString().ToLower();
                    txtCorpCitizenship.SelectedValue = DataAccess.GetData(dtBrokerHeader, 0, "Citizenship", "filipino").ToString().ToLower();
                    txtTIN.Text = DataAccess.GetData(dtBrokerHeader, 0, "Tax", "0").ToString();
                    //txtSSS.Text = DataAccess.GetData(dtBrokerHeader, 0, "SSS", "0").ToString();
                    //txtPassport.Text = DataAccess.GetData(dtBrokerHeader, 0, "Passport", "0").ToString();
                    //txtPassportValid.Text = Convert.ToDateTime(DataAccess.GetData(dtBrokerHeader, 0, "PassportValidFrom", "0")).ToString("yyyy-MM-dd");
                    //dpIssuedBy.SelectedItem.Value = DataAccess.GetData(dtBrokerHeader, 0, "IssuedBy", "0").ToString();
                    //txtPlaceIssued.Text = DataAccess.GetData(dtBrokerHeader, 0, "PlacedIssued", "0").ToString();

                    txtRealtyName.Text = typeOfBusiness.ToUpper() == "SOLE PROPRIETOR" ? DataAccess.GetData(dtBrokerHeader, 0, "BusinessName", "0").ToString() : DataAccess.GetData(dtBrokerHeader, 0, "Partnership", "0").ToString().Trim();

                    txtPRCRegis.Text = DataAccess.GetData(dtBrokerHeader, 0, "PRCRegNum", "0").ToString();
                    txtPRCLicenseExpirationDate.Text = Convert.ToDateTime(DataAccess.GetData(dtBrokerHeader, 0, "PRCLicenseValid", "1900-01-01")).ToString("yyyy-MM-dd");
                    txtPTRNumber.Text = DataAccess.GetData(dtBrokerHeader, 0, "PTRNo", "0").ToString();
                    txtPTRValidFrom.Text = Convert.ToDateTime(DataAccess.GetData(dtBrokerHeader, 0, "ValidFrom", "1900-01-01")).ToString("yyyy-MM-dd");
                    txtPTRValidTo.Text = Convert.ToDateTime(DataAccess.GetData(dtBrokerHeader, 0, "ValidTo", "1900-01-01")).ToString("yyyy-MM-dd");
                    txtGeneralInfoComments.Text = DataAccess.GetData(dtBrokerHeader, 0, "GENERALINFOCOMMENTS", "").ToString();
                    txtAddressBusinessComments.Text = DataAccess.GetData(dtBrokerHeader, 0, "AddressBusinessComments", "").ToString();
                    txtSupplementaryDetailsComments.Text = DataAccess.GetData(dtBrokerHeader, 0, "SupplimentaryDetailsComments", "").ToString();
                    txtPRCLicenseInformationComments.Text = DataAccess.GetData(dtBrokerHeader, 0, "PRCLicenseInformationComments", "").ToString();
                    //New Fields
                    txtResidenceNo.Text = DataAccess.GetData(dtBrokerHeader, 0, "ResidenceNo", "").ToString();
                    txtMobileNo.Text = DataAccess.GetData(dtBrokerHeader, 0, "MobileNo", "").ToString();
                    ddSex.SelectedValue = DataAccess.GetData(dtBrokerHeader, 0, "Sex", "---Select Sex---").ToString().ToUpper();
                    //txtPassportValidTo.Text = Convert.ToDateTime(DataAccess.GetData(dtBrokerHeader, 0, "PassportValidTo", "1900-01-01")).ToString("yyyy-MM-dd");
                    txtSpouse.Text = DataAccess.GetData(dtBrokerHeader, 0, "Spouse", "").ToString();
                    ddCivilStatus.SelectedValue = DataAccess.GetData(dtBrokerHeader, 0, "CivilStatus", "---Select Civil Status---").ToString().ToUpper();
                    txtPRCLicenseRegistration.Text = DataAccess.GetData(dtBrokerHeader, 0, "PRCLicenseRegistration", "").ToString();
                    txtAIPOOrganization.Text = DataAccess.GetData(dtBrokerHeader, 0, "AIPO_Organization", "").ToString();
                    txtAIPOValidFrom.Text = Convert.ToDateTime(DataAccess.GetData(dtBrokerHeader, 0, "AIPO_ValidFrom", "1900-01-01")).ToString("yyyy-MM-dd");
                    txtAIPOValidFrom.Text = txtAIPOValidFrom.Text == "1900-01-01" ? null : txtAIPOValidFrom.Text;
                    txtAIPOValidTo.Text = Convert.ToDateTime(DataAccess.GetData(dtBrokerHeader, 0, "AIPO_ValidTo", "1900-01-01")).ToString("yyyy-MM-dd");
                    txtAIPOValidTo.Text = txtAIPOValidTo.Text == "1900-01-01" ? null : txtAIPOValidTo.Text;
                    txtAIPOReceiptNo.Text = DataAccess.GetData(dtBrokerHeader, 0, "AIPO_ReceiptNo", "").ToString();
                    txtDesignation.Text = DataAccess.GetData(dtBrokerHeader, 0, "Designation", "").ToString();

                    //2023-07-07 : ADDED NEW DESIGNATION FIELD FOR AUTOMATION
                    txtDesignation2.Text = DataAccess.GetData(dtBrokerHeader, 0, "Designation", "").ToString();


                    txtContactFName.Text = DataAccess.GetData(dtBrokerHeader, 0, "FirstName", "").ToString();
                    txtContactMName.Text = DataAccess.GetData(dtBrokerHeader, 0, "MiddleName", "").ToString();
                    txtContactLName.Text = DataAccess.GetData(dtBrokerHeader, 0, "LastName", "").ToString();

                    txtContactEmail.Text = DataAccess.GetData(dtBrokerHeader, 0, "EmailContact", "").ToString().Trim();
                    txtContactMobile.Text = DataAccess.GetData(dtBrokerHeader, 0, "MobileNo", "").ToString();
                    txtContactAddress.Text = DataAccess.GetData(dtBrokerHeader, 0, "AddressContact", "").ToString();
                    txtContactResidence.Text = DataAccess.GetData(dtBrokerHeader, 0, "ResidenceContact", "").ToString();
                    ddContactValidID.SelectedValue = DataAccess.GetData(dtBrokerHeader, 0, "ValidIDContactInfo", "---Select Valid ID---").ToString();
                    txtOthersContactInfo.Text = DataAccess.GetData(dtBrokerHeader, 0, "IDOthersContactInfo", "").ToString();
                    txtContactValidIDNo.Text = DataAccess.GetData(dtBrokerHeader, 0, "IDNoContactInfo", "").ToString();
                    ChangeContactValidID();
                    txtContactPlaceIssued.Text = DataAccess.GetData(dtBrokerHeader, 0, "PlacedIssued", "").ToString();

                    txtContactPersonPosition.Text = DataAccess.GetData(dtBrokerHeader, 0, "ContactPersonPosition", "").ToString();

                    string ValidID = DataAccess.GetData(dtBrokerHeader, 0, "ValidID", "---Select Valid ID---").ToString();
                    string qryID = $@"select * from OLST where ""GrpCode"" = 'ID' AND ""Code"" = '{ValidID}' AND ""IsShow"" = true";
                    DataTable dtID = hana.GetData(qryID, hana.GetConnection("SAOHana"));
                    if (dtID.Rows.Count > 0)
                    {
                        ddValidID.SelectedValue = ValidID;
                        ValidIdDropDownEdit(dt, dt2, dtBrokerHeader, 1);
                        txtIDNo.Text = DataAccess.GetData(dtBrokerHeader, 0, "IDNo", "").ToString();
                        txtIDExpirationDate.Text = Convert.ToDateTime(DataAccess.GetData(dtBrokerHeader, 0, "IDExpirationDate", DateTime.MinValue.ToString())).ToString("yyyy-MM-dd");
                    }

                    ValidID = DataAccess.GetData(dtBrokerHeader, 0, "ValidID2", "---Select Valid ID---").ToString();
                    qryID = $@"select * from OLST where ""GrpCode"" = 'ID' AND ""Code"" = '{ValidID}' AND ""IsShow"" = true";
                    dtID = hana.GetData(qryID, hana.GetConnection("SAOHana"));
                    if (dtID.Rows.Count > 0)
                    {
                        ddValidID2.SelectedValue = DataAccess.GetData(dtBrokerHeader, 0, "ValidID2", "---Select Valid ID---").ToString();
                        ValidIdDropDownEdit(dt, dt2, dtBrokerHeader, 2);
                        txtIDNo2.Text = DataAccess.GetData(dtBrokerHeader, 0, "IDNo2", "").ToString();
                        txtIDExpirationDate2.Text = Convert.ToDateTime(DataAccess.GetData(dtBrokerHeader, 0, "IDExpirationDate2", DateTime.MinValue.ToString())).ToString("yyyy-MM-dd");
                    }

                    ValidID = DataAccess.GetData(dtBrokerHeader, 0, "ValidID3", "---Select Valid ID---").ToString();
                    qryID = $@"select * from OLST where ""GrpCode"" = 'ID' AND ""Code"" = '{ValidID}' AND ""IsShow"" = true";
                    dtID = hana.GetData(qryID, hana.GetConnection("SAOHana"));
                    if (dtID.Rows.Count > 0)
                    {
                        ddValidID3.SelectedValue = DataAccess.GetData(dtBrokerHeader, 0, "ValidID3", "---Select Valid ID---").ToString();
                        ValidIdDropDownEdit(dt, dt2, dtBrokerHeader, 3);
                        txtIDNo3.Text = DataAccess.GetData(dtBrokerHeader, 0, "IDNo3", "").ToString();
                        txtIDExpirationDate3.Text = Convert.ToDateTime(DataAccess.GetData(dtBrokerHeader, 0, "IDExpirationDate3", DateTime.MinValue.ToString())).ToString("yyyy-MM-dd");
                    }

                    ValidID = DataAccess.GetData(dtBrokerHeader, 0, "ValidID4", "---Select Valid ID---").ToString();
                    qryID = $@"select * from OLST where ""GrpCode"" = 'ID' AND ""Code"" = '{ValidID}' AND ""IsShow"" = true";
                    dtID = hana.GetData(qryID, hana.GetConnection("SAOHana"));
                    if (dtID.Rows.Count > 0)
                    {
                        ddValidID4.SelectedValue = DataAccess.GetData(dtBrokerHeader, 0, "ValidID4", "---Select Valid ID---").ToString();
                        ValidIdDropDownEdit(dt, dt2, dtBrokerHeader, 4);
                        txtIDNo4.Text = DataAccess.GetData(dtBrokerHeader, 0, "IDNo4", "").ToString();
                        txtIDExpirationDate4.Text = Convert.ToDateTime(DataAccess.GetData(dtBrokerHeader, 0, "IDExpirationDate4", DateTime.MinValue.ToString())).ToString("yyyy-MM-dd");
                    }

                    txtHlurb.Text = Convert.ToDateTime(DataAccess.GetData(dtBrokerHeader, 0, "HLURBValidFrom", "1900-01-01")).ToString("yyyy-MM-dd");
                    txtHlurbNo.Text = DataAccess.GetData(dtBrokerHeader, 0, "HLURBLicenseNo", "").ToString();
                    txtAuthorizedRepresentative.Text = DataAccess.GetData(dtBrokerHeader, 0, "AuthorizedRepresentative", "").ToString();
                    txtAuthorizedRepresentative2.Text = DataAccess.GetData(dtBrokerHeader, 0, "AuthorizedRepresentative", "").ToString();
                    txtTradeName.Text = DataAccess.GetData(dtBrokerHeader, 0, "TradeName", "").ToString();
                    txtProvince.Text = DataAccess.GetData(dtBrokerHeader, 0, "Province", "").ToString();

                    txtRegistrationDate.Text = Convert.ToDateTime(DataAccess.GetData(dtBrokerHeader, 0, "PRCRegistrationDate", null)).ToString("yyyy-MM-dd");
                    txtCommitDate.Text = Convert.ToDateTime(DataAccess.GetData(dtBrokerHeader, 0, "CommitmentDate", null)).ToString("yyyy-MM-dd");
                    txtCommitAffiliationDate.Text = Convert.ToDateTime(DataAccess.GetData(dtBrokerHeader, 0, "CommitmentAffiliationDate", null)).ToString("yyyy-MM-dd");
                    txtCommitName.Text = DataAccess.GetData(dtBrokerHeader, 0, "CommitmentName", "").ToString();
                    ddlWTAXCode2.SelectedValue = DataAccess.GetData(dtBrokerHeader, 0, "WTaxCode", "I140").ToString();
                    ddlWTAXCode.SelectedValue = DataAccess.GetData(dtBrokerHeader, 0, "WTaxCode", "I140").ToString(); //20220719 ADDED -KASO
                    ddlVATCode.SelectedValue = DataAccess.GetData(dtBrokerHeader, 0, "VATCode", "IT2").ToString();
                    //string conformecheck = DataAccess.GetData(dtBrokerHeader, 0, "Conforme", "false").ToString();
                    //if (conformecheck == "true" || conformecheck == "1")
                    //{
                    //    CBconforme.Checked = true;
                    //}
                    //else if (conformecheck == "true" || conformecheck == "0")
                    //{
                    //    CBconforme.Checked = false;
                    //}
                    ConformeBlockers();
                    if (ddValidID3.SelectedValue == "---Select Valid ID---")
                    {
                        txtIDNo3.ReadOnly = false;
                        txtIDExpirationDate3.ReadOnly = false;

                        txtIDNo3.Text = "";
                        txtIDExpirationDate3.Text = DateTime.MinValue.Date.ToString();
                    }

                    if (ddValidID4.SelectedValue == "---Select Valid ID---")
                    {
                        txtIDNo4.ReadOnly = false;
                        txtIDExpirationDate4.ReadOnly = false;

                        txtIDNo4.Text = "";
                        txtIDExpirationDate4.Text = DateTime.MinValue.Date.ToString();
                    }

                    txtIDExpirationDate.Text = String.IsNullOrEmpty(txtIDExpirationDate.Text) || Convert.ToDateTime(txtIDExpirationDate.Text).Date == DateTime.MinValue.Date ? null : txtIDExpirationDate.Text;
                    //if (Convert.ToDateTime(txtIDExpirationDate2.Text).Date != DateTime.MinValue.Date)
                    if (txtIDExpirationDate2.Text != null)
                    {
                        //idvalid3.Visible = true;
                        idvalid3.Visible = false;
                        ddValidID3.SelectedValue = DataAccess.GetData(dtBrokerHeader, 0, "ValidID3", "---Select Valid ID---").ToString();
                    }
                    else
                    {
                        txtIDExpirationDate2.Text = null;
                    }
                    //if (Convert.ToDateTime(txtIDExpirationDate3.Text).Date != DateTime.MinValue.Date)
                    if (txtIDExpirationDate3.Text != null)
                    {
                        //idvalid4.Visible = true;
                        idvalid4.Visible = false;
                        ddValidID4.SelectedValue = DataAccess.GetData(dtBrokerHeader, 0, "ValidID4", "---Select Valid ID---").ToString();
                    }
                    else
                    {
                        txtIDExpirationDate3.Text = null;
                    }
                    txtIDExpirationDate4.Text = String.IsNullOrEmpty(txtIDExpirationDate4.Text) || Convert.ToDateTime(txtIDExpirationDate4.Text).Date == DateTime.MinValue.Date ? null : txtIDExpirationDate4.Text;

                    PRCOthers();


                    foreach (GridViewRow row in gvStandardDocumentRequirements.Rows)
                    {
                        if (row.Cells[0].Text == "Broker Application Form")
                        {
                            LoadStandardDocuments("BrokerApplicationForm", dtBrokerHeader, row, "Approved1", "BrokerApplicationFormIssuedDate");
                        }
                        else if (row.Cells[0].Text == "List of Accredited Real Estate Sales Person")
                        {
                            LoadStandardDocuments("ListOfAccredited", dtBrokerHeader, row, "Approved2", "ListOfAccreditedIssuedDate");
                        }
                        else if (row.Cells[0].Text == "Accreditation Agreement")
                        {
                            LoadStandardDocuments("AccreditationAgreement", dtBrokerHeader, row, "Approved3", "AccreditationAgreementIssuedDate");
                        }
                        else if (row.Cells[0].Text == "Broker Accreditation General Policies")
                        {
                            LoadStandardDocuments("BrokerAccreditationGenrealPolicies", dtBrokerHeader, row, "Approved4", "BrokerAccreditationGenrealPoliciesIssuedDate");
                        }
                    }
                    LoadUploadedFromDatabase(gvStandardDocumentRequirements, "Standard", BrokerID, "lblFileName", "btnPreview", "btnRemove", "txtStandardAttachmentComments", "chkStandard", "txtStandardAttachmentDateIssued", "rvStandardAttachmentDateIssued", "cvStandardAttachmentDateIssued");

                    //POPULATE SALES PERSON
                    //qry = $@"SELECT A.""Id"",
                    //A.""SalesPerson"" ""SalesPersonName"",
                    //B.""SAPCardCode"" ""Username"",
                    //A.""EmailAddress"",
                    //A.""Position"",
                    //A.""PRCLicense"",
                    //A.""PRCLicenseExpirationDate"", 
                    //        TO_VARCHAR (TO_DATE(A.""ATPDateSalesPerson""), 'YYYY/MM/DD') ""ATPDate"",
                    //        A.""TIN"",
                    //        A.""VATCode"",
                    //        A.""WTaxCode"", 
                    //        A.""MobileNumber"",
                    //        TO_VARCHAR (TO_DATE(B.""ValidFrom""), 'YYYY/MM/DD') ""ValidFrom"" ,
                    //        TO_VARCHAR (TO_DATE(B.""ValidTo""), 'YYYY/MM/DD') ""ValidTo"" ,
                    //        A.""HLURBLicenseNo"",A.""PTRNo""
                    //FROM ""OSLA"" A INNER JOIN 
                    //""BRK1"" B ON A.""Id"" = B.""Id""
                    //WHERE B.""BrokerId"" = '{BrokerID}'";

                    qry = $@"SELECT A.""Id"",
                                A.""SalesPerson"" ""SalesPersonName"",
                                B.""SAPCardCode"" ""Username"",
                                A.""EmailAddress"",
                                A.""Position"",
                                A.""PRCLicense"",
                                TO_VARCHAR (TO_DATE(A.""PRCLicenseExpirationDate""), 'YYYY-MM-DD') ""PRCLicenseExpirationDate"",
                                TO_VARCHAR (TO_DATE(A.""ATPDateSalesPerson""), 'YYYY-MM-DD') ""ATPDate"",
                                A.""TIN"",
                                A.""VATCode"",
                                A.""WTaxCode"", 
                                A.""MobileNumber"",
                                TO_VARCHAR (TO_DATE(B.""ValidFrom""), 'YYYY-MM-DD') ""ValidFrom"" ,
                                TO_VARCHAR (TO_DATE(B.""ValidTo""), 'YYYY-MM-DD') ""ValidTo"" ,
                                A.""HLURBLicenseNo"",A.""PTRNo""
                            FROM ""OSLA"" A INNER JOIN 
	                            ""BRK1"" B ON A.""Id"" = B.""Id""
                            WHERE B.""BrokerId"" = '{BrokerID}';";

                    ViewState["SalesPerson"] = hana.GetData(qry, hana.GetConnection("SAOHana"));
                    BindGrid();

                    //2023-08-09 : REMOVE LOADING FROM BRK2
                    //POPULATE SHARED DETAILS
                    //qry = $@"SELECT ""Id"", ""SalesPersonId"", ""PositionSharedDetails"" ""Position"", ""SalesPersonNameSharedDetails"" ""SalesPersonName"", ""PercentageSharedDetails"" ""Percentage"", ""ValidFromSharedDetails"" ""ValidFrom"", ""ValidToSharedDetails"" ""ValidTo"",""OslaID"" FROM ""BRK2"" WHERE ""BrokerId"" = '{BrokerID}'";
                    //qry = $@"SELECT ""Id"", ""SalesPersonId"", ""PositionSharedDetails"" ""Position"", ""SalesPersonNameSharedDetails"" ""SalesPersonName"", IFNULL(""PercentageSharedDetails"",'7') ""LotOnlyPercentage"", IFNULL(""HouseandLotSharingDetails"",'5') ""HouseandLotPercentage"", ""OslaID"", ""TransID"" FROM ""BRK2"" WHERE ""BrokerId"" = '{BrokerID}'";
                    //ViewState["SharingDetails"] = hana.GetData(qry, hana.GetConnection("SAOHana"));
                    //var something = ViewState["SharingDetails"];

                    //2023-08-09 : REMOVE LOADING FROM BRK2
                    //qry = $@"SELECT MAX(""TransID"") ""TransID"" FROM ""BRK2""";
                    //DataTable getTransId = hana.GetData(qry, hana.GetConnection("SAOHana"));
                    //ViewState["TransId"] = DataAccess.GetData(getTransId, 0, "TransID", "").ToString();
                    //var idunno = ViewState["TransId"];

                    //ViewState["Commission"] = CommPercent.Text;
                    var eme = ViewState["Commission"];

                    if (rbTypeOfBusiness.SelectedIndex == 0)
                    {
                        divSoleProp.Attributes.Add("class", "row show");
                        divPartnership.Attributes.Add("class", "row hidden");
                    }
                    else
                    {
                        divSoleProp.Attributes.Add("class", "row hidden");
                        divPartnership.Attributes.Add("class", "row show");
                        if (rbTypeOfBusiness.SelectedIndex == 1)
                        {
                            lblPartnership.Text = "Partnership";
                            txtPartnership.Attributes.Add("placeholder", "PARTNERSHIP NAME");
                        }
                        else
                        {
                            lblPartnership.Text = "Corporation";
                            txtPartnership.Attributes.Add("placeholder", "CORPORATION NAME");
                        }
                    }

                    if (typeOfBusiness == "SOLE PROPRIETOR" && ViewState["Status"].ToString() == "ACCEPTED")
                    {
                        txtContactEmail.Text = txtEmailAddress.Text.Trim();
                        txtContactAddress.Text = txtBusinessAddress.Text.Trim();
                    }

                    ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "ModalBrokerList_Hide();", true);

                }
                else
                {
                    alertMsg("The Broker ID does not exist in our database.", "warning");
                }

                //2023-08-09 : CHANGED FROM TEMP_BRK2 TO BRK2
                // GAB 05/11/2023 Query for initializing PouchDb data from database
                qry = $@"SELECT 
	                        ""ProjectCode"",
	                        ""ProjectName"",
	                        ""CommissionPercentage"",
	                        ""HouseandLotSharingDetails"",
	                        ""SalesPersonNameSharedDetails"",
	                        ""PercentageSharedDetails"",
	                        ""OslaID"" AS ""OslaId"",
	                        ""SalesPersonId"",
	                        ""BrokerId"",
	                        ""PositionSharedDetails"",
                            ""CreateDate"",
                            ""Id"",
                            ""Type""
                        FROM ""BRK2""
                        WHERE ""BrokerId"" = '{BrokerID}'";
                var pouchDb = hana.GetData(qry, hana.GetConnection("SAOHana"));
                string json = JsonConvert.SerializeObject(pouchDb);
                txtNext2Hidden.Text = json;
                //txtNext3Hidden.Text = json;
                ChangeCivilStatus();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "ModalBrokerList_Hide();", true);
                alertMsg(ex.Message, "error");
            }
        }

        void LoadStandardDocuments(string document, DataTable dtBrokerHeader, GridViewRow row, string approvedField, string date)
        {
            string status = ViewState["Status"].ToString();
            ((TextBox)row.FindControl("txtStandardAttachmentComments")).Text = DataAccess.GetData(dtBrokerHeader, 0, $"{document}Comments", "").ToString().Trim();
            if (!string.IsNullOrWhiteSpace(DataAccess.GetData(dtBrokerHeader, 0, document, "").ToString()))
            {
                ((Label)row.FindControl("lblFileName")).Text = DataAccess.GetData(dtBrokerHeader, 0, document, "").ToString();
                ((TextBox)row.FindControl("txtStandardAttachmentDateIssued")).Text = Convert.ToDateTime(DataAccess.GetData(dtBrokerHeader, 0, date, "1900-01-01")).ToString("yyyy-MM-dd");
                ((TextBox)row.FindControl("txtStandardAttachmentDateIssued")).Text = ((TextBox)row.FindControl("txtStandardAttachmentDateIssued")).Text == "1900-01-01" ? null : ((TextBox)row.FindControl("txtStandardAttachmentDateIssued")).Text;
                ((TextBox)row.FindControl("txtStandardAttachmentDateIssued")).ReadOnly = true;
                string test = DataAccess.GetData(dtBrokerHeader, 0, $"{document}Comments", "").ToString().Trim();

                ((TextBox)row.FindControl("txtStandardAttachmentDateIssued")).Enabled = true;
                ((RequiredFieldValidator)row.FindControl("rvStandardAttachmentDateIssued")).Enabled = true;
                ((CustomValidator)row.FindControl("cvStandardAttachmentDateIssued")).Enabled = true;
                visibleDocumentButtons(true, (LinkButton)row.FindControl("btnPreview"), (LinkButton)row.FindControl("btnRemove"));

                if (DataAccess.GetData(dtBrokerHeader, 0, approvedField, "0").ToString().Trim() == "1")
                {
                    ((CheckBox)row.FindControl("chkStandard")).Checked = true;
                }
                else
                {
                    ((CheckBox)row.FindControl("chkStandard")).Checked = false;
                }

                //if (status == "ACCEPTED")
                //{
                //    if (!string.IsNullOrWhiteSpace(DataAccess.GetData(dtBrokerHeader, 0, document, "").ToString()))
                //    {
                //        ((TextBox)row.FindControl("txtStandardAttachmentDateIssued")).Enabled = true;
                //        ((RequiredFieldValidator)row.FindControl("rvStandardAttachmentDateIssued")).Enabled = true;
                //        visibleDocumentButtons(true, (LinkButton)row.FindControl("btnPreview"), (LinkButton)row.FindControl("btnRemove"));
                //    }

                //}
            }
        }



        void LoadUploadedFromDatabase(GridView gv, string type, string BrokerID, string label, string Prev, string Del, string text, string checkbox, string date, string rv, string cv)
        {
            string status = ViewState["Status"].ToString();
            foreach (GridViewRow row in gv.Rows)
            {
                string documentName = row.Cells[0].Text;
                if (documentName != "Broker Application Form" || documentName != "List of Accredited Real Estate Sales Person" || documentName != "Accreditation Agreement" || documentName != "Broker Accreditation General Policies")
                {
                    string qry = $@"SELECT * FROM ""BRK3"" WHERE ""DocumentName"" = '{row.Cells[0].Text}' AND ""Section"" = '{type}' AND ""BrokerId"" = '{BrokerID}'";
                    DataTable dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
                    if (dt.Rows.Count > 0)
                    {
                        ((Label)row.FindControl(label)).Text = DataAccess.GetData(dt, 0, "FileName", "").ToString().Trim();
                        ((TextBox)row.FindControl(date)).Text = Convert.ToDateTime(DataAccess.GetData(dt, 0, "DateIssued", "1900-01-01")).ToString("yyyy-MM-dd");
                        ((TextBox)row.FindControl(date)).Text = Convert.ToDateTime(((TextBox)row.FindControl(date)).Text).Date == DateTime.MinValue.Date ? null : ((TextBox)row.FindControl(date)).Text;
                        ((TextBox)row.FindControl(date)).ReadOnly = true;
                        ((TextBox)row.FindControl(text)).Text = DataAccess.GetData(dt, 0, "Comments", "").ToString().Trim();
                        if (DataAccess.GetData(dt, 0, "Approved", "0").ToString().Trim() == "1")
                        {
                            ((CheckBox)row.FindControl(checkbox)).Checked = true;
                        }
                        else
                        {
                            ((CheckBox)row.FindControl(checkbox)).Checked = false;
                        }




                        //2023-07-10 : CHANGED, AS APPROVED BY SIR CHRISTIAN VIA CHAT == SHOW ALL CONTROLS
                        //if (status == "ACCEPTED" || gv.ID.ToString() == "gvStandardDocumentRequirements")
                        if (status == "ACCEPTED")
                        {
                            if (!string.IsNullOrWhiteSpace(DataAccess.GetData(dt, 0, "FileName", "").ToString().Trim()))
                            {
                                ((TextBox)row.FindControl(date)).Enabled = true;
                                ((RequiredFieldValidator)row.FindControl(rv)).Enabled = true;
                                ((CustomValidator)row.FindControl(cv)).Enabled = true;
                                visibleDocumentButtons(true, (LinkButton)row.FindControl(Prev), (LinkButton)row.FindControl(Del));
                            }
                            else
                            {
                                ((TextBox)row.FindControl(date)).Enabled = false;
                                ((RequiredFieldValidator)row.FindControl(rv)).Enabled = false;
                                ((CustomValidator)row.FindControl(cv)).Enabled = false;
                                visibleDocumentButtons(false, (LinkButton)row.FindControl(Prev), (LinkButton)row.FindControl(Del));
                            }
                        }
                        else
                        {
                            //2023-07-10 : VISIBILITY OF THE BUTTON CONTROLS
                            visibleDocumentButtons(true, (LinkButton)row.FindControl(Prev), (LinkButton)row.FindControl(Del));
                        }
                    }
                }
            }

        }




        protected void gvShareDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "View" || e.CommandName == "Delete")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                mtxtID2.Text = row.Cells[0].Text;
                ViewState["SharingID"] = row.Cells[1].Text;
                mtxtSalesPersonShareID.Text = row.Cells[1].Text;
                mddPositionShare.SelectedItem.Text = row.Cells[2].Text;
                mtxtSalesPersonShare.Text = HttpUtility.HtmlDecode(row.Cells[3].Text);
                mtxtPecent.Text = row.Cells[4].Text;
                mtxtHouseAndLotPecent.Text = row.Cells[5].Text;
                //mtxtValidFrom.Text = row.Cells[5].Text;
                //mtxtValidTo.Text = row.Cells[6].Text;
            }
        }

        protected void btnPrev_ServerClick(object sender, EventArgs e)
        {
            step_2.Visible = true;
            step_3.Visible = false;
            step_4.Visible = false;
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            GridViewRow gridViewRow = (GridViewRow)(sender as Control).Parent.Parent;
            string index = gridViewRow.Cells[6].Text;
            ViewState["SharingDetailsID"] = index;
            string rowid = gridViewRow.Cells[7].Text;
            ViewState["RowId"] = rowid;
            btnListOfSalesPersonSharingDetails.Disabled = true;
            CommPercent.Text = "0";
            if (Convert.ToBoolean(ViewState["isLot"]) == true)
            {
                sharingLotOnly.Visible = true;
                RequiredFieldValidator32.Enabled = true;
                sharingHouseAndLot.Visible = false;
                RequiredFieldValidator15.Enabled = false;
            }
            else
            {
                sharingLotOnly.Visible = false;
                RequiredFieldValidator32.Enabled = false;
                sharingHouseAndLot.Visible = true;
                RequiredFieldValidator15.Enabled = true;
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowAddSharingDetailsModal();", true);
        }

        protected void mbtnUpdate_ServerClick(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["SalesPerson"];

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideAddRowEditModal();", true);

            step_2.Visible = false;
            step_3.Visible = true;
            step_4.Visible = false;
        }

        protected void btnNewSalesPerson_ServerClick(object sender, EventArgs e)
        {
            ViewState["SalesorShare"] = "Sales";
            ViewState["AddOrEditSalesPerson"] = "Add";
            Session["AddSalesPerson"] = "SalesPerson";
            lblSaveSalesAgent.Text = "Add";
            mtxtSalesPerson.Focus();
            btnListOfSalesPerson.Disabled = false;
            btnListOfSalesPersonSharingDetails.Disabled = false;

            ClearSalesPersonModal();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowAddSalesPersonModal();", true);
        }

        protected void btnSubmitDocument_ServerClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
                confirmation("Are you sure you want to submit these documents?", "removebuyer");
        }
        //protected void btnSubmitDocument_ServerClick(object sender, EventArgs e)
        //{
        //    if (Page.IsValid)
        //        if (ViewState["Status"].ToString() == "ACCEPTED")
        //        {
        //            UpdateBrokerInformation();
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ModalApprovalType_Show();", true);
        //        }
        //}



        void UpdateBrokerInformation()
        {
            try
            {
                string ret = "Operation completed successfully.";
                string BrokerCode = ViewState["BrokerID"].ToString();

                hana.Execute($@"DELETE FROM ""BRK1"" WHERE ""BrokerId"" = '{BrokerCode}';", hana.GetConnection("SAOHana"));
                hana.Execute($@"DELETE FROM ""BRK2"" WHERE ""BrokerId"" = '{BrokerCode}';", hana.GetConnection("SAOHana"));
                hana.Execute($@"DELETE FROM ""BRK3"" WHERE ""BrokerId"" = '{BrokerCode}';", hana.GetConnection("SAOHana"));

                string function = "UPDATE";



                //2023-08-09 : COMMENTED
                ////GAB 05/15/2023 pouchDb Data
                //string pouchDbData = hiddenLabel.Value;

                //var pouchdata = JsonConvert.DeserializeObject<List<SharingDetails>>(pouchDbData);
                ////insert delete database data where BrokerID = brokerId
                //ws.DeleteSharingDetails(BrokerCode);
                //foreach (var item in pouchdata)
                //{
                //    int Id = item.Id;
                //    string salesPersonId = item.SalesPersonId;
                //    string brokerId = BrokerCode;
                //    string position = item.PositionSharedDetails;
                //    string salesName = item.SalesPersonNameSharedDetails;
                //    double lotShare = item.PercentageSharedDetails;
                //    DateTime CreateDate = item.CreateDate;
                //    string salesAgentId = item.OslaId;
                //    double hNLotShare = item.HouseandLotSharingDetails;
                //    string projCode = item.ProjectCode;
                //    string projName = item.ProjectName;
                //    double commission = item.CommissionPercentage;
                //    string type = item.Type;


                //    ws.AddSharingDetails(Id, salesPersonId, brokerId, position, salesName, lotShare, CreateDate, salesAgentId, hNLotShare, projCode, projName, commission, type);
                //}

                string TypeOfBusiness = rbTypeOfBusiness.SelectedValue.ToUpper().Trim();

                //JOSES
                string ATPDate = txtATPExpiryDate.Text;
                string partnership = (txtPartnership.Text == "?" ? "" : txtPartnership.Text).ToUpper().Trim();
                string SECRegNo = (txtSECRegNo.Text == "?" ? "" : txtSECRegNo.Text).ToUpper().Trim();

                string LastName = (rbTypeOfBusiness.SelectedIndex != 0 ? txtContactLName.Text : txtLastName.Text).ToUpper().Trim();
                string FirstName = (rbTypeOfBusiness.SelectedIndex != 0 ? txtContactFName.Text : txtFirstName.Text).ToUpper().Trim();
                string MiddleName = (rbTypeOfBusiness.SelectedIndex != 0 ? txtContactMName.Text : txtMiddleName.Text).ToUpper().Trim();
                string NickName = txtNickName.Text.ToUpper().Trim();
                string Address = txtAddress.Text.ToUpper().Trim();
                string City = txtCity.Text.ToUpper().Trim();
                int ZipCode = Convert.ToInt32(txtZipCode.Text);
                string nature = txtNatureOfBusiness.Text.ToUpper().Trim();
                string BusinessName = txtBusinessName.Text.ToUpper().Trim();
                string BusinessAdd = txtBusinessAddress.Text.ToUpper().Trim();
                int BusinessZipCode = (int)Convert.ToInt32(txtBusinessZipCode.Text);
                string MobileNum = txtBusinessPhoneNo.Text;
                string FaxNo = txtFaxNo.Text;

                //JOSES
                string emaillAdd = txtEmailAddress.Text.Trim();
                string DateOfBirth = dtDateOfBirth.Text == null || dtDateOfBirth.Text == "" ? DateTime.MinValue.ToString("yyyy-MM-dd") : dtDateOfBirth.Text;
                string PlaceOfBirth = txtPlaceOfBirth.Text.ToUpper().Trim();
                string Religion = txtReligion.Text.ToUpper().Trim();
                string citizen = txtCitizenship.Value.Trim();
                string TIN = txtTIN.Text;
                //string SSS = txtSSS.Text;

                //string Passport = txtPassport.Text;
                //string PassportValid = txtPassportValid.Text == null || txtPassportValid.Text == "" ? DateTime.MinValue.ToString("yyyy-MM-dd") : txtPassportValid.Text;
                //string IssuedBy = dpIssuedBy.SelectedItem.Text.ToUpper().Trim();
                string PlaceIssued = txtContactPlaceIssued.Text.ToUpper().Trim();
                string PRCRegis = txtPRCRegis.Text;
                string PRCLicenseValid = txtPRCLicenseExpirationDate.Text == null || txtPRCLicenseExpirationDate.Text == "" ? DateTime.MinValue.ToString("yyyy-MM-dd") : txtPRCLicenseExpirationDate.Text;
                string PTRNumber = txtPTRNumber.Text;
                string ValidFrom = txtPTRValidFrom.Text == null || txtPTRValidFrom.Text == "" ? DateTime.MinValue.ToString("yyyy-MM-dd") : txtPTRValidFrom.Text;
                string ValidTO = txtPTRValidTo.Text == null || txtPTRValidTo.Text == "" ? DateTime.MinValue.ToString("yyyy-MM-dd") : txtPTRValidTo.Text;

                string BrokerApplicationForm = "";
                string ListOfAccredited = "";
                string AccreditationAgreement = "";
                string BrokerAccreditationGenrealPolicies = "";

                //New fields
                string ResidenceNo = txtResidenceNo.Text;
                string MobileNo = txtMobileNo.Text;
                string Sex = ddSex.Text;
                //string PassportValidTo = txtPassportValidTo.Text == null || txtPassportValidTo.Text == "" ? DateTime.MinValue.ToString("yyyy-MM-dd") : txtPassportValidTo.Text;
                string Spouse = txtSpouse.Text;
                string CivilStatus = ddCivilStatus.Text;
                string PRCLicenseRegistration = txtPRCLicenseRegistration.Text;
                string AIPOOrganization = txtAIPOOrganization.Text;
                string AIPOValidFrom = txtAIPOValidFrom.Text == null || txtAIPOValidFrom.Text == "" ? DateTime.MinValue.ToString("yyyy-MM-dd") : txtAIPOValidFrom.Text;
                string AIPOValidTo = txtAIPOValidTo.Text == null || txtAIPOValidTo.Text == "" ? DateTime.MinValue.ToString("yyyy-MM-dd") : txtAIPOValidTo.Text;
                string AIPOReceiptNo = txtAIPOReceiptNo.Text;
                string Designation = txtDesignation.Text;


                string PositionContact = ddContactPosition.SelectedValue;
                string EmailContact = txtContactEmail.Text.Trim();
                string AddressContact = txtContactAddress.Text;
                string ResidenceContact = txtContactResidence.Text;

                //Valid ID
                string ValidID = ddValidID.SelectedValue;
                string IDNo = txtIDNo.Text;
                string IDOthers = txtOthers1.Text;
                string IDExpirationDate = txtIDExpirationDate.Text == null || txtIDExpirationDate.Text == "" ? DateTime.MinValue.ToString() : txtIDExpirationDate.Text;
                string ValidID2 = ddValidID2.SelectedValue;
                string IDNo2 = txtIDNo2.Text;
                string IDOthers2 = txtOthers2.Text;
                string IDExpirationDate2 = txtIDExpirationDate2.Text == null || txtIDExpirationDate2.Text == "" ? DateTime.MinValue.ToString() : txtIDExpirationDate2.Text;
                string ValidID3 = ddValidID3.SelectedValue;
                string IDNo3 = txtIDNo3.Text;
                string IDOthers3 = txtOthers3.Text;
                string IDExpirationDate3 = txtIDExpirationDate3.Text == null || txtIDExpirationDate3.Text == "" ? DateTime.MinValue.ToString() : txtIDExpirationDate3.Text;
                string ValidID4 = ddValidID4.SelectedValue;
                string IDNo4 = txtIDNo4.Text;
                string IDOthers4 = txtOthers4.Text;
                string IDExpirationDate4 = txtIDExpirationDate4.Text == null || txtIDExpirationDate4.Text == "" ? DateTime.MinValue.ToString() : txtIDExpirationDate4.Text;

                string BrokerApplicationFormIssuedDate = DateTime.MinValue.ToString("yyyy-MM-dd");
                string ListOfAccreditedIssuedDate = DateTime.MinValue.ToString("yyyy-MM-dd");
                string AccreditationAgreementIssuedDate = DateTime.MinValue.ToString("yyyy-MM-dd");
                string BrokerAccreditationGenrealPoliciesIssuedDate = DateTime.MinValue.ToString("yyyy-MM-dd");

                string ContactPersonPosition = txtContactPersonPosition.Text;
                string HLURBLicenseNo = txtHlurbNo.Text;
                string AuthorizedRepresentative = txtAuthorizedRepresentative.Text;
                string Province = txtProvince.Text;
                string TradeName = txtTradeName.Text;

                string ContactValidID = ddContactValidID.SelectedValue;
                string ContactIDOthers = txtOthersContactInfo.Text;
                string ContactIDNo = txtContactValidIDNo.Text;

                string PRCRegistrationDate = txtRegistrationDate.Text == null || txtRegistrationDate.Text == "" ? DateTime.MinValue.ToString("yyyy-MM-dd") : txtRegistrationDate.Text;
                string CommitmentDate = txtCommitDate.Text == null || txtCommitDate.Text == "" ? DateTime.MinValue.ToString("yyyy-MM-dd") : txtCommitDate.Text;
                string CommitAffiliationDate = txtCommitAffiliationDate.Text == null || txtCommitAffiliationDate.Text == "" ? DateTime.MinValue.ToString("yyyy-MM-dd") : txtCommitAffiliationDate.Text;
                string CommitmentName = txtCommitName.Text;

                bool Conforme = CBconforme.Checked;

                foreach (GridViewRow row in gvStandardDocumentRequirements.Rows)
                {
                    if (row.Cells[0].Text == "Broker Application Form")
                    {
                        BrokerApplicationForm = ((Label)row.FindControl("lblFileName")).Text;
                        BrokerApplicationFormIssuedDate = ((TextBox)row.FindControl("txtStandardAttachmentDateIssued")).Text;
                    }
                    else if (row.Cells[0].Text == "List of Accredited Real Estate Sales Person")
                    {
                        ListOfAccredited = ((Label)row.FindControl("lblFileName")).Text;
                        ListOfAccreditedIssuedDate = ((TextBox)row.FindControl("txtStandardAttachmentDateIssued")).Text;
                    }
                    else if (row.Cells[0].Text == "Accreditation Agreement")
                    {
                        AccreditationAgreement = ((Label)row.FindControl("lblFileName")).Text;
                        AccreditationAgreementIssuedDate = ((TextBox)row.FindControl("txtStandardAttachmentDateIssued")).Text;
                    }
                    else if (row.Cells[0].Text == "Broker Accreditation General Policies")
                    {
                        BrokerAccreditationGenrealPolicies = ((Label)row.FindControl("lblFileName")).Text;
                        BrokerAccreditationGenrealPoliciesIssuedDate = ((TextBox)row.FindControl("txtStandardAttachmentDateIssued")).Text;
                    }

                }

                string Header = ws.Broker(
                                              BrokerCode,
                                              TypeOfBusiness, //2
                                              ATPDate,
                                              partnership, //4
                                              SECRegNo,

                                              FirstName,    //6
                                              MiddleName,
                                              LastName, //8
                                              NickName,
                                              Address,      //10

                                              City,
                                              ZipCode,      //12
                                              nature,
                                              BusinessName, //14
                                              BusinessAdd,
                                              BusinessZipCode,  //16
                                              MobileNum,
                                              FaxNo,    //18

                                              emaillAdd,
                                              DateOfBirth,  //20
                                              PlaceOfBirth,
                                              Religion, //22
                                              citizen,
                                              TIN,  //24
                                                    //SSS,

                                              //Passport,     //26
                                              //Convert.ToDateTime(PassportValid),
                                              //IssuedBy, //28
                                              PlaceIssued,

                                              PRCRegis, //30
                                              Convert.ToDateTime(PRCLicenseValid),
                                              PTRNumber,    //32
                                              Convert.ToDateTime(ValidFrom),
                                              Convert.ToDateTime(ValidTO),  //34
                                              ViewState["Status"].ToString(),

                                              BrokerApplicationForm,    //36
                                              ListOfAccredited,
                                              AccreditationAgreement,   //38
                                              BrokerAccreditationGenrealPolicies,

                                              DateTime.Now, //40
                                              DateTime.Now,
                                              function,

                                              ResidenceNo,
                                             MobileNo,
                                             Sex,
                                             //Convert.ToDateTime(PassportValidTo),
                                             Spouse,
                                             CivilStatus,
                                             PRCLicenseRegistration,
                                             AIPOOrganization,
                                             Convert.ToDateTime(AIPOValidFrom),
                                             Convert.ToDateTime(AIPOValidTo),
                                             AIPOReceiptNo,
                                             Designation,

                                             ValidID,
                                             IDNo,
                                             IDOthers,
                                             Convert.ToDateTime(IDExpirationDate),
                                             ValidID2,
                                             IDNo2,
                                             IDOthers2,
                                             Convert.ToDateTime(IDExpirationDate2),
                                             ValidID3,
                                             IDNo3,
                                             IDOthers3,
                                             Convert.ToDateTime(IDExpirationDate3),
                                             ValidID4,
                                             IDNo4,
                                             IDOthers4,
                                             Convert.ToDateTime(IDExpirationDate4),
                                             PositionContact,
                                             EmailContact,
                                             AddressContact,
                                             ResidenceContact,
                                             "0",
                                             Convert.ToDateTime(txtHlurb.Text == null || txtHlurb.Text == "" ? DateTime.MinValue.ToString("yyyy-MM-dd") : txtHlurb.Text),

                                             BrokerApplicationFormIssuedDate,
                                             ListOfAccreditedIssuedDate,
                                             AccreditationAgreementIssuedDate,
                                             BrokerAccreditationGenrealPoliciesIssuedDate,

                                             ContactPersonPosition,
                                             HLURBLicenseNo,
                                             AuthorizedRepresentative,
                                             TradeName,
                                             Province,

                                             ContactValidID,
                                             ContactIDOthers,
                                             ContactIDNo,

                                             Convert.ToDateTime(PRCRegistrationDate),
                                             Convert.ToDateTime(CommitmentDate),
                                             CommitmentName,
                                             Convert.ToDateTime(CommitAffiliationDate),
                                             Conforme,
                                             ddlWTAXCode.SelectedValue,
                                             ddlVATCode.SelectedValue,
                                             ddlLocation.SelectedValue
                                             );

                if (Header != "Operation completed successfully.")
                {
                    ret = $"Failed to submit your documents! Please contact your administrator (Error: Header - {Header})";
                    alertMsg(ret, "error");
                }
                else
                {


                    string child1 = "";
                    //SALES PERSON
                    DataTable dtSalesPerson = (DataTable)ViewState["SalesPerson"];
                    foreach (DataRow row in dtSalesPerson.Rows)
                    {
                        if (row.RowState != DataRowState.Deleted)
                        {
                            string index = row["Id"].ToString();
                            //string SalesPersonName = row.Cells[1].Text.ToUpper().Trim();
                            //string EmailAddress = row.Cells[2].Text;
                            //string Position = row.Cells[3].Text;
                            //string PRCLicense = row.Cells[4].Text;
                            //DateTime PRCLicenseExpirationDate = Convert.ToDateTime(row.Cells[5].Text);
                            //DateTime ATPDateSalesPerson = Convert.ToDateTime(row.Cells[6].Text);
                            //string TINSalesPerson = row.Cells[7].Text;
                            //string VATCode = row.Cells[8].Text.ToUpper().Trim();
                            //string WTaxCode = row.Cells[9].Text.ToUpper().Trim();
                            //string MobileNumber = row.Cells[10].Text;
                            DateTime validFromSalesPerson = Convert.ToDateTime(row["ValidFrom"].ToString() == "" ? DateTime.MinValue.ToString("yyyy-MM-dd") : row["ValidFrom"].ToString());
                            DateTime validToSalesPerson = Convert.ToDateTime(row["ValidTo"].ToString() == "" ? DateTime.MinValue.ToString("yyyy-MM-dd") : row["ValidTo"].ToString());

                            child1 = ws.insertBrokerSalesPerson(
                                                       index,
                                                       BrokerCode,
                                                       //SalesPersonName,
                                                       //EmailAddress,
                                                       //Position,
                                                       //PRCLicense,
                                                       //PRCLicenseExpirationDate,
                                                       //ATPDateSalesPerson,
                                                       //TINSalesPerson,
                                                       //VATCode,
                                                       //WTaxCode,
                                                       //MobileNumber,
                                                       validFromSalesPerson,
                                                       validToSalesPerson,
                                                       DateTime.Now,
                                                       "ADD"
                                                       );

                            if (child1 != "Operation completed successfully.")
                            {
                                ret = $"Failed to submit your documents! Please contact your administrator (Error: Sales Person - {child1})";
                                break;
                                // ws.SQDeleteLeads(lblID.Text); alertMsg(ret, "error"); 
                            }
                            else
                            {
                                string qry = $@"UPDATE A
                                        SET  ""SAPCardCode"" = B.""CardCode""
                                        FROM ""{ConfigurationManager.AppSettings["SAODatabase"]}"".BRK1 A
                                        INNER JOIN OCRD B ON B.""U_BrokerCode"" = '{BrokerCode}'  AND B.""U_SalesAgentCode"" = '{index}'
                                        where ifnull(A.""SAPCardCode"",'') ='' AND A.""Id"" = '{index}'";
                                hana.Execute(qry, hana.GetConnection("SAPHana"));
                            }
                        }
                    }


                    if (child1 == "Operation completed successfully.")
                    {
                        string child2 = "Operation completed successfully.";

                        //#######################################################
                        //SHARING DETAILS
                        //#######################################################


                        ////2023-08-09 : COMMENTED
                        ////2023-07-10 : CHANGED SOURCE OF SHARING DETAILS
                        ////DataTable dtShareDetails = (DataTable)ViewState["SharingDetails"];
                        //DataTable dtShareDetails = (DataTable)ViewState["SharingDetailsPouch"];


                        //2023-08-09 : GET POUCHDATA
                        string pouchDbData = hiddenLabel.Value;
                        var pouchdata = JsonConvert.DeserializeObject<List<SharingDetails>>(pouchDbData);

                        //2023-07-20 : IF NULL, WAG PROCEED 
                        //if (dtShareDetails != null)
                        //2023-08-09 : CHANGED TO POUCH
                        if (pouchdata != null)
                        {
                            //2023-08-09 : CHANGED TO POUCHDB
                            foreach (var item in pouchdata)
                            {

                                //2023-08-09 : CHANGE TO SALESPERSON 
                                //foreach (DataRow row in dtShareDetails.Rows)
                                foreach (DataRow row in dtSalesPerson.Rows)
                                {
                                    //2023-08-09 : CONDITION IF AGENT EXISTS IN SALES AGENTS   ; CHANGED CONDITION 
                                    //if (row.RowState != DataRowState.Deleted)
                                    if (item.SalesPersonId == row["Id"].ToString().Trim())
                                    {

                                        //2023-08-07 : CHANGED SYNTAX TO ALIGN FOR POUCH 
                                        //int index2 = Convert.ToInt32(row["Id"]);
                                        //string SalesPersonId = row["SalesPersonID"].ToString().Trim();
                                        int index2 = item.Id;
                                        string SalesPersonId = item.SalesPersonId.ToString().Trim();

                                        //2023-07-10 : UPDATED PARAMETER NAMES
                                        //string PositionSharedDetails = row["Position"].ToString().Trim();
                                        //string SalesPersonNameSharedDetails = row["SalesPersonName"].ToString().Trim();
                                        //double PercentageSharedDetails = Convert.ToDouble(row["LotOnlyPercentage"]);
                                        //double HouseandLotSharingDetails = Convert.ToDouble(row["HouseandLotPercentage"]);
                                        //string OslaID = row["OslaID"].ToString().Trim();
                                        ////DateTime ValidFromSharedDetails = Convert.ToDateTime(row.Cells[5].Text);
                                        ////DateTime ValidToSharedDetails = Convert.ToDateTime(row.Cells[6].Text); 
                                        ///

                                        //2023-08-09 : COMMENTED 
                                        //string PositionSharedDetails = row["PositionSharedDetails"].ToString().Trim();
                                        //string SalesPersonNameSharedDetails = row["SalesPersonNameSharedDetails"].ToString().Trim();
                                        //double PercentageSharedDetails = Convert.ToDouble(row["PercentageSharedDetails"]);
                                        //double HouseandLotSharingDetails = Convert.ToDouble(row["HouseandLotSharingDetails"]);
                                        //string OslaID = row["OslaId"].ToString().Trim();

                                        ////2023-07-10 : ADDITIONAL COLUMNS
                                        //string ProjectCode = row["ProjectCode"].ToString().Trim();
                                        //string ProjectName = row["ProjectName"].ToString().Trim();
                                        //double CommissionPercentage = Convert.ToDouble(row["CommissionPercentage"]);
                                        //string Type = row["Type"].ToString().Trim();

                                        ////2023-08-08 : UPDATE FETCHING OF DATA FROM POUCH INSTEAD OF DATATABLE
                                        string PositionSharedDetails = item.PositionSharedDetails.Trim();
                                        string SalesPersonNameSharedDetails = item.SalesPersonNameSharedDetails.Trim();
                                        double PercentageSharedDetails = Convert.ToDouble(item.PercentageSharedDetails);
                                        double HouseandLotSharingDetails = Convert.ToDouble(item.HouseandLotSharingDetails);
                                        string OslaID = item.OslaId.Trim();
                                        string ProjectCode = item.ProjectCode.Trim();
                                        string ProjectName = item.ProjectName.Trim();
                                        double CommissionPercentage = Convert.ToDouble(item.CommissionPercentage);
                                        string Type = item.Type.Trim();



                                        //2023-07-10 : ADDED PARAMETERS
                                        //child2 = ws.insertBrokerSharePerson(
                                        //                            index2,
                                        //                            SalesPersonId,
                                        //                            BrokerCode,
                                        //                            PositionSharedDetails,
                                        //                            SalesPersonNameSharedDetails,
                                        //                            PercentageSharedDetails,
                                        //                            HouseandLotSharingDetails,
                                        //                            //ValidFromSharedDetails,
                                        //                            //ValidToSharedDetails,
                                        //                            DateTime.Now,
                                        //                            OslaID,
                                        //                            "ADD"
                                        //                            );
                                        child2 = ws.insertBrokerSharePerson(
                                                                    index2,
                                                                    SalesPersonId,
                                                                    BrokerCode,
                                                                    PositionSharedDetails,
                                                                    SalesPersonNameSharedDetails,
                                                                    PercentageSharedDetails,
                                                                    HouseandLotSharingDetails,
                                                                    DateTime.Now,
                                                                    OslaID,
                                                                    "ADD",

                                                                    //2023-07-10 : ADDITIONAL PARAMETERS
                                                                    ProjectCode,
                                                                    ProjectName,
                                                                    CommissionPercentage,
                                                                    Type

                                                                    );



                                        if (child2 != "Operation completed successfully.")
                                        {
                                            ret = $"Failed to submit your documents! Please contact your administrator (Error: Sharing Details - {child2})";
                                            break;
                                            // ws.SQDeleteLeads(lblID.Text); alertMsg(ret, "error"); 
                                        }
                                    }
                                }
                            }
                        }

                        if (child1 == "Operation completed successfully.")
                        {
                            string document = ws.updateBrokerApproval(
                                               ViewState["BrokerID"].ToString(),
                                               //rbApprovalType.SelectedValue.ToUpper().Trim(),
                                               "PENDING",
                                               txtGeneralInfoComments.Text,
                                               txtAddressBusinessComments.Text,
                                               txtSupplementaryDetailsComments.Text,
                                               txtPRCLicenseInformationComments.Text
                                               );
                            if (document == "Operation completed successfully.")
                            {
                                //DOCUMENT REQUIREMENTS
                                string child3 = "Operation completed successfully.";
                                string label = "";
                                string date = "";
                                string textbox = "";
                                string checkbox = "";
                                GridView gv;
                                if (rbTypeOfBusiness.SelectedIndex == 0)
                                {
                                    label = "lblFileName2";
                                    gv = gvSolePropDocumentRequirements;
                                    date = "txtSolePropDocumentRequirementsDateIssued";
                                    checkbox = "chkSole";
                                    textbox = "txtSolePropDocumentRequirementsComments";
                                }
                                else if (rbTypeOfBusiness.SelectedIndex == 1)
                                {
                                    label = "lblFileName3";
                                    gv = gvPartnershipDocumentRequirements;
                                    date = "txtPartnershipDocumentRequirementsDateIssued";
                                    checkbox = "chkPartner";
                                    textbox = "txtPartnershipDocumentRequirementsComments";
                                }
                                else
                                {
                                    label = "lblFileName4";
                                    gv = gvCorporationDocumentRequirements;
                                    date = "txtCorporationDocumentRequirementsDateIssued";
                                    checkbox = "chkCorp";
                                    textbox = "txtCorporationDocumentRequirementsComments";
                                }
                                //FIRST ONE IS FOR STANDARDS AND SECOND IS FOR BUSINESS TYPE
                                //child3 = postDocumentRequirements(gvStandardDocumentRequirements, "lblFileName", BrokerCode);
                                //child3 = postDocumentRequirements(gv, label, BrokerCode, function, date);
                                child3 = updateDocumentComments(gvStandardDocumentRequirements, "txtStandardAttachmentComments", BrokerCode, "Standard", "chkStandard", "lblFileName", "txtStandardAttachmentDateIssued");
                                child3 = updateDocumentComments(gv, textbox, BrokerCode, rbTypeOfBusiness.SelectedItem.Text.Trim(), checkbox, label, date);
                                ret = child3;
                                if (child3 == "Operation completed successfully.")
                                {
                                    ret = updateComments();
                                }
                            }
                        }
                    }

                    //FOR DOCUMENT COMMENTS AND CHECKBOX OF STANDARD DOCUMENT
                    updateStandardDocumentComments("txtStandardAttachmentComments", BrokerCode, "", "chkStandard");

                    //DONE
                    if (ret == "Operation completed successfully.")
                    {
                        Session["ConfirmType"] = "success";

                        mtxtUniqueId.Text = $"Broker updated successfully!";


                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "hideConfirm();", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "ShowSuccessAlert();", true);

                        moveTemporaryFilesToPermanent(gvStandardDocumentRequirements, "lblFileName");
                        moveTemporaryFilesToPermanent(gvSolePropDocumentRequirements, "lblFileName2");
                        moveTemporaryFilesToPermanent(gvPartnershipDocumentRequirements, "lblFileName3");
                        moveTemporaryFilesToPermanent(gvCorporationDocumentRequirements, "lblFileName4");
                        DeleteTemporaryFIles();

                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "hideConfirm();", true);
                        alertMsg(ret, "error");
                    }

                }
            }


            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "hideConfirm();", true);
                alertMsg(ex.Message, "error");
            }
        }



        void confirmation(string body, string type)
        {
            Session["ConfirmType"] = type;
            lblConfirmationInfo.Text = body;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "confirmation", "showConfirmation();", true);
        }

        protected void btnYes_Click(object sender, EventArgs e)
        {
            ////GAB 05/13/2023 pouchDb Data
            //string pouchDbData = hiddenLabel.Value;

            //var pouchdata = JsonConvert.DeserializeObject<List<SharingDetails>>(pouchDbData);
            ////insert delete database data where BrokerID = brokerId
            //ws.DeleteSharingDetails(BrokerCode.Last().ToString());
            //foreach (var item in pouchdata)
            //{
            //    int Id = item.Id;
            //    string salesPersonId = item.SalesPersonId;
            //    string brokerId = BrokerCode.Last().ToString();
            //    string position = item.PositionSharedDetails;
            //    string salesName = item.SalesPersonNameSharedDetails;
            //    int lotShare = item.PercentageSharedDetails;
            //    DateTime CreateDate = item.CreateDate;
            //    string salesAgentId = item.OslaId;
            //    int hNLotShare = item.HouseandLotSharingDetails;
            //    string projCode = item.ProjectCode;
            //    string projName = item.ProjectName;
            //    double commission = item.CommissionPercentage;
            //    string type = item.Type;


            //    ws.AddSharingDetails(Id, salesPersonId, brokerId, position, salesName, lotShare, CreateDate, salesAgentId, hNLotShare, projCode, projName, commission, type);
            //}

        }


        string postDocumentRequirements(GridView gv, string label, string BrokerCode, string Function, string date)
        {
            string ret = "Operation completed successfully.";
            foreach (GridViewRow row in gv.Rows)
            {
                string documentName = row.Cells[0].Text.Trim();
                string fileName = ((Label)row.FindControl(label)).Text;
                string docdate = ((TextBox)row.FindControl(date)).Text;
                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    string document = ws.insertBrokerDocument(
                                                            BrokerCode,
                                                            documentName,
                                                            rbTypeOfBusiness.SelectedItem.Text.Trim(),
                                                            fileName,
                                                            DateTime.Now,
                                                            Function,
                                                            docdate
                                                            );
                    if (document != "Operation completed successfully.")
                    {
                        ret = $"Failed to submit your documents! Please contact your administrator (Error: Document - {document})";
                        break;
                    }
                }
            }
            return ret;
        }


        protected void btnAddSharing_Click(object sender, EventArgs e)
        {
            try
            {
                CommPercent.Text = "0";
                //GAB 07/3/2023
                mtxtHouseAndLotPecent.Text = "";
                //GAB 5/13/2023
                string pouchDbData = txtNext3Hidden.Text;

                var pouchdata = JsonConvert.DeserializeObject<List<SharingDetails>>(pouchDbData);
                //var data = JsonConvert.DeserializeObject<SharingDetails[]>(pouchDbData); //this has problems...
                //var pouchdata = data.ToList();
                ViewState["SalesorShare"] = "Share";

                GridViewRow gridViewRow = (GridViewRow)(sender as Control).Parent.Parent;
                ViewState["SharingID"] = gridViewRow.Cells[0].Text;
                DataTable dt = (DataTable)ViewState["SharingDetails"];
                string salesPerson = gridViewRow.Cells[0].Text;

                // Filter the rows where HouseandLotPercentage is not equal to 0
                //DataRow[] filteredRows = dt.Select($"LotOnlyPercentage <> '0' AND SalesPersonID = '{salesPerson}'");
                DataRow[] filteredRows = dt.Select($"HouseandLotPercentage = '0' AND SalesPersonID = '{salesPerson}'");

                // Create a new DataTable to store the filtered rows
                DataTable filteredDataTable = dt.Clone();
                foreach (DataRow row in filteredRows)
                {
                    filteredDataTable.ImportRow(row);
                }
                //DataTable tempTB = dt.Select($"SalesPersonId = {ViewState["SharingID"]}").CopyToDataTable();
                //dt.Merge(tempTB);
                //ViewState["SharingDetails"] = dt;
                gvShareDetails.Columns[4].ItemStyle.CssClass = "";
                gvShareDetails.Columns[5].ItemStyle.CssClass = "hidden";
                gvShareDetails.Columns[4].HeaderStyle.CssClass = "";
                gvShareDetails.Columns[5].HeaderStyle.CssClass = "hidden";
                ViewState["isLot"] = true;
                ViewState["Type"] = "Lot";

                if (ViewState["Status"].ToString() != "ACCEPTED")
                {
                    gvShareDetails.Columns[8].ItemStyle.CssClass = "hidden";
                    gvShareDetails.Columns[8].HeaderStyle.CssClass = "hidden";
                }
                else
                {
                    gvShareDetails.Columns[8].ItemStyle.CssClass = "";
                    gvShareDetails.Columns[8].HeaderStyle.CssClass = "";
                }

                string type = ViewState["Type"].ToString();

                if (filteredDataTable.Rows.Count > 0)
                {
                    BindGrid2(ViewState["SharingID"].ToString());
                }
                else
                {
                    gvShareDetails.DataSource = null;
                    gvShareDetails.DataBind();
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "ShowAddSharingModal();", true);
                BindGridSharingDetailsFromPouchDb(pouchdata);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ComparingTables()", "ComparingTables();", true);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "ChangeCommPercent()", "ChangeCommPercent()", true);
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }
        }

        protected void btnNextModal_ServerClick(object sender, EventArgs e)
        {
            //btnSubmitDocument.InnerText = "Submit Document";
            //txtUniqueIdSet.Text = "";

            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowBusinessTypeModal();", true);
        }

        protected void btnAddSharingDetails_ServerClick(object sender, EventArgs e)
        {
            ViewState["SalesorShare"] = "Share";
            Session["AddSalesPerson"] = "SharingDetails";
            ViewState["SharingDetailsID"] = null;
            mtxtSalesPersonShare.Text = "";
            btnListOfSalesPersonSharingDetails.Disabled = false;

            //2023-07-28 : CCOMMENTED TO PREVENT THE TEXTBOX FROM REFRESHING
            //CommPercent.Text = "0";


            if (Convert.ToBoolean(ViewState["isLot"]) == true)
            {
                sharingLotOnly.Visible = true;
                RequiredFieldValidator32.Enabled = true;
                sharingHouseAndLot.Visible = false;
                RequiredFieldValidator15.Enabled = false;
                mtxtPecent.Text = "7";
            }
            else
            {
                sharingLotOnly.Visible = false;
                RequiredFieldValidator32.Enabled = false;
                sharingHouseAndLot.Visible = true;
                RequiredFieldValidator15.Enabled = true;
                mtxtHouseAndLotPecent.Text = "5";
            }

            //Setting CommPercent to its value
            //CommPercent.Text = ViewState["Commission"].ToString();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowAddSharingDetailsModal();", true);
        }

        protected void CommPercent_TextChanged(object sender, EventArgs e)
        {
            // Retrieve the input value using the unique id
            //string inputValue = CommPercent.Text;
            //ViewState["Commission"] = inputValue;
        }

        protected void btnSalesPersonView_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                GridViewRow row = (GridViewRow)btn.NamingContainer;


                string index = row.Cells[0].Text;
                string SalesPersonName = row.Cells[1].Text;
                //string EmailAddress = row.Cells[2].Text;
                //string Position = row.Cells[3].Text;
                //string PRCLicense = row.Cells[4].Text;
                //string PRCLicenseExpirationDate = row.Cells[5].Text;
                //string ATPDate = Convert.ToDateTime(row.Cells[6].Text).ToString("yyyy-MM-dd");
                //string TIN = row.Cells[7].Text;
                //string VATCode = row.Cells[8].Text;
                //string WTaxCode = row.Cells[9].Text;
                //string MobileNumber = row.Cells[10].Text;
                string validFrom = (row.Cells[12].Text == "&nbsp;" || String.IsNullOrWhiteSpace(row.Cells[12].Text)) ? null : Convert.ToDateTime(row.Cells[12].Text).ToString("yyyy-MM-dd");
                string validTo = (row.Cells[13].Text == "&nbsp;" || String.IsNullOrWhiteSpace(row.Cells[13].Text)) ? null : Convert.ToDateTime(row.Cells[13].Text).ToString("yyyy-MM-dd");

                mtxtID.Text = index;
                mtxtSalesPersonID.Text = index;
                mtxtSalesPerson.Text = SalesPersonName.ToUpper();

                //GAB 5/24/2023 Disabling hamburger button in viewing to avoid editing the original broker row
                LinkButton deleteButton = (LinkButton)row.FindControl("btnSalesPersonDelete");
                LinkButton deleteButton1 = (LinkButton)row.FindControl("btnSalesPersonHouseLotDelete");

                if (!deleteButton.Visible && !deleteButton1.Visible)
                {
                    btnListOfSalesPerson.Disabled = true;
                }
                else
                {
                    btnListOfSalesPerson.Disabled = false;
                }
                //mtxtEmail.Text = EmailAddress;
                //ddPosition.SelectedItem.Text = Position;
                //mtxtPRCLicense.Text = PRCLicense;
                //mtxtPRCLicenseExpirationDate.Text = PRCLicenseExpirationDate;
                //mtxtATPDate.Text = ATPDate;
                //mtxtTIN.Text = TIN;
                //mtxtVATCode.Text = VATCode;
                //mtxtWTaxCode.Text = WTaxCode;
                //mtxtMobile.Text = MobileNumber;
                mtxtValidFrom.Text = validFrom;
                mtxtValidTo.Text = validTo;

                ViewState["AddOrEditSalesPerson"] = "Update";
                lblSaveSalesAgent.Text = "Update";
                Session["AddSalesPerson"] = "SalesPerson";
                ViewState["SalesorShare"] = "Share";

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowAddSalesPersonModal();", true);
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }
        }

        protected void btnSaveSharingDetails_ServerClick(object sender, EventArgs e)
        {
            //ADD NEW SHARING DETAILS
            try
            {
                mtxtPecent.Text = mtxtPecent.Text == "" ? "0" : mtxtPecent.Text;
                mtxtHouseAndLotPecent.Text = mtxtHouseAndLotPecent.Text == "" ? "0" : mtxtHouseAndLotPecent.Text;

                DataTable dt = (DataTable)ViewState["SharingDetails"];
                DataTable dt2 = (DataTable)ViewState["SharingDetails"];
                //GAB 06/29/2023 Commented reason: needs filter by LotOnly or HouseNLot
                //var rows = dt2.Select($"SalesPersonId = '{ViewState["SharingID"]}'");

                //GAB 06/29/2023 Filtering datatable by LotOnly or HouseNLot
                bool isLot = Convert.ToBoolean(ViewState["isLot"]);
                var qry1 = dt2.Select($"SalesPersonId = '{ViewState["SharingID"]}' AND LotOnlyPercentage <> '0'");
                var qry2 = dt2.Select($"SalesPersonId = '{ViewState["SharingID"]}' AND HouseandLotPercentage <> '0'");
                var rows = isLot == true ? qry1 : qry2; //Filters dt2 using SalesPersonId

                if (rows.Any())
                {
                    //dt = dt2.Select($"SalesPersonId = '{ViewState["SharingID"]}'").CopyToDataTable(); //GAB 06/29/2023
                    dt = isLot == true ? qry1.CopyToDataTable() : qry2.CopyToDataTable();
                    if (dt.Select($"SalesPersonId = '{ViewState["SharingID"]}'").AsEnumerable().Where(x => x.RowState != DataRowState.Deleted ? x[6].ToString() == mtxtSalesPersonShareID.Text : false).Any())
                    {
                        //ClearSharingDetailsModal();
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideDetailsModal();", true);
                        //alertMsg($"Error: Sales person with same Id already exist in sharing details", "warning");

                        //DELETE ROWSTATE DELETED
                        foreach (DataRow row in dt2.Rows)
                        {
                            if (row.RowState == DataRowState.Deleted)
                            {
                                //dt2.Rows.RemoveAt(dt2.Rows.IndexOf(row));
                                dt2.Rows.Remove(row);
                                dt2.AcceptChanges();
                                gvShareDetails.DataSource = dt2;
                                gvShareDetails.DataBind();

                                ViewState["SharingDetails"] = dt2;
                                gvShareDetails.EditIndex = -1;
                                BindGrid2(ViewState["SharingID"].ToString());

                                break;
                            }
                        }

                        if (ViewState["SharingDetailsID"] != null)
                        {
                            var sharingDeets = ViewState["SharingDetails"]; //from the datatable
                            var sharingId = ViewState["SharingID"].ToString();
                            //FROM
                            //btnAddSharingHouseLot_Click //Pag nagoopen sa salesperson table from housenlot column
                            //btnAddSharing_Click //Pag nagoopen sa salesperson table from lot column
                            //gvShareDetails_RowCommand //Pag nageedit or delete ng salesAgent table
                            string sharingDeetsId = ViewState["SharingDetailsID"].ToString();
                            //FROM:
                            //btnView_Click //Pag nageedit ng SalesAgent (Nilalagay dito yung OSLA Id nung row ng SalesAgent na ineedit.)
                            //btnAddSharingDetails_ServerClick //Pagnagclick ng add row sa SalesAgent (Ginagawa nitong null para makuha as new row na idadagdag)
                            //string newsharingDeetsId = ViewState["NewSharingDetailsID"].ToString();
                            string rowId = ViewState["RowId"].ToString();
                            //FROM: btnView_Click //Nilalagay yung TransId
                            foreach (DataRow row in dt2.Rows)
                            {
                                string OslaId = row[6].ToString();
                                string SalesPersonId = row[1].ToString();
                                string LotOnly = row[4].ToString();
                                string HouseNLot = row[5].ToString();
                                string TransId = row[7].ToString();

                                if (OslaId == sharingDeetsId &&
                                    SalesPersonId == sharingId &&
                                    TransId == rowId) //Checking every row of the same sales person with the same RowId then saving the changes
                                //Check if LotOnly
                                {
                                    //    if (row[6].ToString() == ViewState["SharingDetailsID"].ToString() && row[1].ToString() == ViewState["SharingID"].ToString())
                                    //{
                                    row[0] = Convert.ToInt32(row[0].ToString());
                                    row[1] = ViewState["SharingID"].ToString();
                                    row[2] = mddPositionShare.SelectedItem.Text;
                                    row[3] = mtxtSalesPersonShare.Text.ToUpper().Trim();
                                    row[4] = Convert.ToDouble(mtxtPecent.Text.Trim());
                                    row[5] = Convert.ToDouble(mtxtHouseAndLotPecent.Text.Trim());
                                    row[6] = row[6].ToString();
                                    //row[6] = newsharingDeetsId != null ? newsharingDeetsId : sharingDeetsId; //the problem now is: if NewSharingDetails doesn't have data... Since he gets data on bSelectsalesPersons...

                                    dt2.AcceptChanges();
                                    row.SetModified();

                                    ViewState["SharingDetails"] = dt2;
                                    BindGrid2(ViewState["SharingID"].ToString());
                                    ClearSalesPersonModal();
                                }
                            }
                        }
                        else
                        {
                            alertMsg(mtxtSalesPersonShare.Text + " is already added on the list!", "error");
                        }

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideAddSharingDetailsModal();", true);
                    }
                    else
                    {
                        if (ViewState["SharingDetailsID"] == null)
                        {
                            var uniqueId = Convert.ToInt32(ViewState["TransId"].ToString()) + 1;
                            //ADD SHARING DETAILS
                            dt2.Rows.Add(
                                 Convert.ToInt32(dt.Rows.Count + 1),
                                 ViewState["SharingID"].ToString(),
                                 mddPositionShare.SelectedItem.Text,
                                 mtxtSalesPersonShare.Text.ToUpper().Trim(),
                                 Convert.ToDouble(mtxtPecent.Text.Trim()),
                                 Convert.ToDouble(mtxtHouseAndLotPecent.Text.Trim()),
                                 mtxtSalesPersonShareID.Text,
                                 uniqueId

                            //,
                            // Convert.ToDateTime(mtxtValidFrom.Text).ToString("yyyy-MM-dd"),
                            // Convert.ToDateTime(mtxtValidTo.Text).ToString("yyyy-MM-dd")
                            );
                            ViewState["TransId"] = uniqueId;
                            ViewState["SharingDetails"] = dt2;
                            BindGrid2(ViewState["SharingID"].ToString());
                        }
                        else
                        {
                            //DELETE ROWSTATE DELETED
                            foreach (DataRow row in dt2.Rows)
                            {
                                if (row.RowState == DataRowState.Deleted)
                                {
                                    dt2.Rows.RemoveAt(dt2.Rows.IndexOf(row));
                                    dt2.AcceptChanges();
                                    gvShareDetails.DataSource = dt2;
                                    gvShareDetails.DataBind();

                                    ViewState["SharingDetails"] = dt2;
                                    gvShareDetails.EditIndex = -1;
                                    BindGrid2(ViewState["SharingID"].ToString());
                                }
                            }

                            var sharingDeets = ViewState["SharingDetails"]; //from the datatable
                            string sharingId = ViewState["SharingID"].ToString();
                            //FROM
                            //btnAddSharingHouseLot_Click //Pag nagoopen sa salesperson table from housenlot column
                            //btnAddSharing_Click //Pag nagoopen sa salesperson table from lot column
                            //gvShareDetails_RowCommand //Pag nageedit or delete ng salesAgent table
                            string sharingDeetsId = ViewState["SharingDetailsID"].ToString();
                            //FROM:
                            //btnView_Click //Pag nageedit ng SalesAgent (Nilalagay dito yung OSLA Id nung row ng SalesAgent na ineedit.)
                            //btnAddSharingDetails_ServerClick //Pagnagclick ng add row sa SalesAgent (Ginagawa nitong null para makuha as new row na idadagdag)
                            //string newsharingDeetsId = ViewState["NewSharingDetailsID"].ToString();
                            string rowId = ViewState["RowId"].ToString();

                            //UPDATING SHARING DETAILS
                            foreach (GridViewRow row in gvShareDetails.Rows)
                            {
                                // Get the values from the current row's cells
                                //string rowSharingDetailsId = row.Cells[6].Text;
                                //string rowSharingId = row.Cells[1].Text;

                                string OslaId = row.Cells[6].ToString();
                                string SalesPersonId = row.Cells[1].ToString();
                                string LotOnly = row.Cells[4].ToString();
                                string HouseNLot = row.Cells[5].ToString();
                                string TransId = row.Cells[7].ToString();
                                if (OslaId == sharingDeetsId && SalesPersonId == sharingId && TransId == rowId)
                                {
                                    //dt2.Rows.RemoveAt(gvShareDetails.Rows.Count - 1);
                                    //dt2.Rows.RemoveAt(row.RowIndex);
                                    //dt2.AcceptChanges();

                                    // Remove the row from the DataTable
                                    DataRow[] matchingRows = dt2.Select($"SalesPersonId = '{sharingId}' AND OslaID = '{sharingDeetsId}'");

                                    if (matchingRows.Length > 0)
                                    {
                                        dt2.Rows.Remove(matchingRows[0]);
                                        dt2.AcceptChanges();
                                    }
                                    gvShareDetails.DataSource = dt2;
                                    gvShareDetails.DataBind();

                                    dt2.Rows.Add(
                                        mtxtID2.Text,
                                        ViewState["SharingID"].ToString(),
                                        mddPositionShare.SelectedItem.Text,
                                        mtxtSalesPersonShare.Text.ToUpper().Trim(),
                                        Convert.ToDouble(mtxtPecent.Text.Trim()),
                                        Convert.ToDouble(mtxtHouseAndLotPecent.Text.Trim()),
                                        //mtxtSalesPersonShareID.Text
                                        row.Cells[6].Text
                                        //newsharingDeetsId != null ? newsharingDeetsId : sharingDeetsId
                                        //,
                                        //Convert.ToDateTime(mtxtValidFrom.Text).ToString("yyyy-MM-dd"),
                                        //Convert.ToDateTime(mtxtValidTo.Text).ToString("yyyy-MM-dd")
                                        );

                                    ViewState["SharingDetails"] = dt2;
                                    //gvShareDetails.EditIndex = -1;    
                                    BindGrid2(ViewState["SharingID"].ToString());
                                    ClearSalesPersonModal();
                                }
                            }
                        }

                        ClearSharingDetailsModal();

                        step_2.Visible = false;
                        step_3.Visible = true;
                        step_4.Visible = false;
                        closeAddSharingModal();

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideAddSharingDetailsModal();", true);
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "uncheckAll();", true);
                }
                else
                {

                    if (ViewState["SharingDetailsID"] == null)
                    {
                        var test = ViewState["SharingID"];
                        var uniqueId = Convert.ToInt32(ViewState["TransId"].ToString()) + 1;
                        //ADD SHARING DETAILS
                        dt2.Rows.Add(
                             Convert.ToInt32(1),
                             ViewState["SharingID"].ToString(),
                             mddPositionShare.SelectedItem.Text,
                             mtxtSalesPersonShare.Text.ToUpper().Trim(),
                             Convert.ToDouble(mtxtPecent.Text.Trim()),
                             Convert.ToDouble(mtxtHouseAndLotPecent.Text.Trim()),
                             mtxtSalesPersonShareID.Text,
                             uniqueId
                        //,
                        // Convert.ToDateTime(mtxtValidFrom.Text).ToString("yyyy-MM-dd"),
                        // Convert.ToDateTime(mtxtValidTo.Text).ToString("yyyy-MM-dd")
                        );
                        ViewState["TransId"] = uniqueId;
                        ViewState["SharingDetails"] = dt2;
                        BindGrid2(ViewState["SharingID"].ToString());
                    }
                    else
                    {
                        var sharingDeets = ViewState["SharingDetails"]; //from the datatable
                        string sharingId = ViewState["SharingID"].ToString();
                        //FROM
                        //btnAddSharingHouseLot_Click //Pag nagoopen sa salesperson table from housenlot column
                        //btnAddSharing_Click //Pag nagoopen sa salesperson table from lot column
                        //gvShareDetails_RowCommand //Pag nageedit or delete ng salesAgent table
                        string sharingDeetsId = ViewState["SharingDetailsID"].ToString();
                        //FROM:
                        //btnView_Click //Pag nageedit ng SalesAgent (Nilalagay dito yung OSLA Id nung row ng SalesAgent na ineedit.)
                        //btnAddSharingDetails_ServerClick //Pagnagclick ng add row sa SalesAgent (Ginagawa nitong null para makuha as new row na idadagdag)
                        //string newsharingDeetsId = ViewState["NewSharingDetailsID"].ToString();
                        //UPDATING SHARING DETAILS
                        foreach (GridViewRow row in gvShareDetails.Rows)
                        {
                            if (row.Cells[0].Text == ViewState["SharingDetailsID"].ToString())
                            {
                                //dt2.Rows.RemoveAt(gvShareDetails.Rows.Count - 1);
                                //dt2.Rows.RemoveAt(row.RowIndex);
                                //dt2.AcceptChanges();
                                //gvShareDetails.DataSource = dt2;
                                //gvShareDetails.DataBind();

                                // Remove the row from the DataTable
                                DataRow[] matchingRows = dt2.Select($"SalesPersonId = '{sharingId}' AND OslaID = '{sharingDeetsId}'");

                                if (matchingRows.Length > 0)
                                {
                                    dt2.Rows.Remove(matchingRows[0]);
                                    dt2.AcceptChanges();
                                }
                                gvShareDetails.DataSource = dt2;
                                gvShareDetails.DataBind();

                                dt2.Rows.Add(
                                    mtxtID2.Text,
                                    ViewState["SharingID"].ToString(),
                                    mddPositionShare.SelectedItem.Text,
                                    mtxtSalesPersonShare.Text.ToUpper().Trim(),
                                    Convert.ToDouble(mtxtPecent.Text.Trim()),
                                    Convert.ToDouble(mtxtHouseAndLotPecent.Text.Trim()),
                                    //newsharingDeetsId != null ? newsharingDeetsId : sharingDeetsId
                                    row.Cells[6].Text

                                    //,
                                    //Convert.ToDateTime(mtxtValidFrom.Text).ToString("yyyy-MM-dd"),
                                    //Convert.ToDateTime(mtxtValidTo.Text).ToString("yyyy-MM-dd")
                                    );

                                ViewState["SharingDetails"] = dt2;
                                gvShareDetails.EditIndex = -1;
                                BindGrid2(ViewState["SharingID"].ToString());
                                ClearSalesPersonModal();
                            }
                        }
                    }

                    ClearSharingDetailsModal();

                    step_2.Visible = false;
                    step_3.Visible = true;
                    step_4.Visible = false;
                    closeAddSharingModal();

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideAddSharingDetailsModal();", true);
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "uncheckAll();", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideAddSharingDetailsModal();", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideAddSharingModal();", true);
                alertMsg(ex.Message, "error");
            }
        }

        protected void gvShareDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            BindGrid2(ViewState["SharingID"].ToString());
            gvShareDetails.PageIndex = e.NewPageIndex;
            gvShareDetails.DataBind();
        }

        protected void gvSalesPerson_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSalesPerson.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        //GAB 05/13/2023 Page changing
        protected void gvProjectList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProjectList.PageIndex = e.NewPageIndex;
            BindGrid3();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ComparingTables()", "ComparingTables();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ChangeCommPercent()", "ChangeCommPercent()", true);
        }

        //GAB 05/13/2023 Page changing
        protected void gvSelectedSharingDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string pouchDbData = txtNext3Hidden.Text;
            //string pouchDbData = txtPouchDbDataViewHidden.Value;
            var pouchdata = JsonConvert.DeserializeObject<List<SharingDetails>>(pouchDbData);
            gvSelectedSharingDetails.PageIndex = e.NewPageIndex;
            BindGridSharingDetailsFromPouchDb(pouchdata);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ComparingTables()", "ComparingTables();", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "ChangeCommPercent()", "ChangeCommPercent()", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "RestoreCheckedItems", "restoreCheckedItems();", true);
        }

        //###################################################### //
        //#####################  DOCUMENTS ##################### //
        //###################################################### //


        void visibleDocumentButtons(bool visible, LinkButton prev, LinkButton del)
        {
            prev.Visible = visible;
            del.Visible = visible;
        }
        private string GenerateGuid()
        {
            //GENERATED NEW GUID FOR UNIQUE ID PER CONNECTIONS/WORKSTATIONS
            string id = "";
            var ticks = DateTime.Now.Ticks;
            var guid = Guid.NewGuid().ToString();
            var uniqueSessionId = ticks.ToString() + guid.ToString();
            string[] agentCode = uniqueSessionId.Split('-');
            return agentCode.Last();
        }
        void uploadDocRequirements(GridView gv, GridViewCommandEventArgs e, string FileUpload, string lblfilename, string preview, string remove, string date, string rv, string cb, string cv)
        {
            try
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int index = row.RowIndex;

                FileUpload fileUpload = (FileUpload)gv.Rows[index].FindControl(FileUpload);
                Label lblFileName = (Label)gv.Rows[index].FindControl(lblfilename);
                TextBox txtDate = (TextBox)gv.Rows[index].FindControl(date);
                LinkButton btnPreview = (LinkButton)gv.Rows[index].FindControl(preview);
                LinkButton btnDelete = (LinkButton)gv.Rows[index].FindControl(remove);
                CheckBox checkBox = (CheckBox)gv.Rows[index].FindControl(cb);
                RequiredFieldValidator rvDate = (RequiredFieldValidator)gv.Rows[index].FindControl(rv);
                CustomValidator cvDate = (CustomValidator)gv.Rows[index].FindControl(cv);

                if (e.CommandName == "Upload")
                {
                    string documentName = gv.Rows[index].Cells[0].Text;


                    if (fileUpload.HasFile) //If the used Uploaded a file  
                    {
                        //string code = Environment.MachineName;
                        string code = GenerateGuid();

                        //Get FileName and Extension seperately
                        string fileNameOnly = Path.GetFileNameWithoutExtension(fileUpload.FileName);
                        string extension = Path.GetExtension(fileUpload.FileName);
                        string uniqueCode = code;

                        string FileName = fileNameOnly + "_" + code + extension; //Name of the file is stored in local Variable   
                        if (File.Exists(Server.MapPath("~/DOCUMENT_REQUIREMENTS/") + FileName) || File.Exists(Server.MapPath("~/TEMP_DOCS/") + FileName))
                        {
                            alertMsg("File already exists!", "error");
                        }
                        else
                        {
                            lblFileName.Text = FileName;
                            fileUpload.PostedFile.SaveAs(Server.MapPath("~/TEMP_DOCS/") + FileName); //File is saved in the Physical folder  

                            txtDate.Enabled = true;
                            txtDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                            txtDate.ReadOnly = true;
                            rvDate.Enabled = true;
                            cvDate.Enabled = true;

                            visibleDocumentButtons(true, btnPreview, btnDelete);
                        }
                    }
                }
                else if (e.CommandName == "Preview")
                {
                    if (File.Exists(Server.MapPath("~/TEMP_DOCS/") + lblFileName.Text))
                    {
                        string Filepath = Server.MapPath("~/TEMP_DOCS/" + lblFileName.Text);
                        //lblFileName.Text = Filepath;
                        //System.Diagnostics.Process.Start(Filepath);
                        var host = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.LocalPath, "");
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + host + "/TEMP_DOCS/" + lblFileName.Text + "');", true);
                    }
                    else
                    {
                        string Filepath = Server.MapPath("~/DOCUMENT_REQUIREMENTS/" + lblFileName.Text);
                        //lblFileName.Text = Filepath;
                        //System.Diagnostics.Process.Start(Filepath);
                        var host = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.LocalPath, "");
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + host + "/DOCUMENT_REQUIREMENTS/" + lblFileName.Text + "');", true);
                    }
                }
                else if (e.CommandName == "Remove")
                {
                    if (File.Exists(Server.MapPath("~/DOCUMENT_REQUIREMENTS/") + lblFileName.Text))
                    {
                        // If file found, delete it    
                        File.Delete(Server.MapPath("~/DOCUMENT_REQUIREMENTS/") + lblFileName.Text);
                        lblFileName.Text = "";
                        txtDate.Text = null;
                        txtDate.Enabled = false;
                        rvDate.Enabled = false;
                        cvDate.Enabled = false;
                    }
                    else
                    {
                        lblFileName.Text = "";
                        txtDate.Text = null;
                        txtDate.Enabled = false;
                        rvDate.Enabled = false;
                        cvDate.Enabled = false;
                    }
                    checkBox.Checked = false;
                    visibleDocumentButtons(false, btnPreview, btnDelete);
                }
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }
        }




        protected void gvStandardDocumentRequirements_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            uploadDocRequirements(gvStandardDocumentRequirements, e, "FileUpload1", "lblFileName", "btnPreview", "btnRemove", "txtStandardAttachmentDateIssued", "rvStandardAttachmentDateIssued", "chkStandard", "cvStandardAttachmentDateIssued");
        }

        protected void gvSolePropDocumentRequirements_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            uploadDocRequirements(gvSolePropDocumentRequirements, e, "FileUpload2", "lblFileName2", "btnPreview2", "btnRemove2", "txtSolePropDocumentRequirementsDateIssued", "rvSolePropDocumentRequirementsDateIssued", "chkSole", "cvSolePropDocumentRequirementsDateIssued");
        }

        protected void gvPartnershipDocumentRequirements_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            uploadDocRequirements(gvPartnershipDocumentRequirements, e, "FileUpload3", "lblFileName3", "btnPreview3", "btnRemove3", "txtPartnershipDocumentRequirementsDateIssued", "rvPartnershipDocumentRequirementsDateIssued", "chkPartner", "cvPartnershipDocumentRequirementsDateIssued");

        }
        protected void gvCorporationDocumentRequirements_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            uploadDocRequirements(gvCorporationDocumentRequirements, e, "FileUpload4", "lblFileName4", "btnPreview4", "btnRemove4", "txtCorporationDocumentRequirementsDateIssued", "rvCorporationDocumentRequirementsDateIssued", "chkCorp", "cvCorporationDocumentRequirementsDateIssued");

        }

        protected void bSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Value))
            {
                //GAB 06/22/2023 Searching when there substrings are both infront and in the end of the searched string
                string searchTerm = txtSearch.Value.ToString().Trim();
                string searchPattern = string.Join("%", searchTerm.ToCharArray().Select(c => c.ToString())); // Inserting '%' between each character
                searchPattern = $"%{searchPattern}%"; // Adding '%' at the beginning and end

                DataTable dt = hana.GetData($"CALL sp_BrokerSearch ('{searchPattern}');", hana.GetConnection("SAOHana"));
                ViewState["BrokerList"] = dt;
                gvBrokerList.DataSource = dt;
                gvBrokerList.DataBind();

                //gvBrokerList.DataSource = hana.GetData($"CALL sp_BrokerSearch ('%{txtSearch.Value}%');", hana.GetConnection("SAOHana")); ;
                //gvBrokerList.DataBind();
            }
            else
            {
                loadBrokerList();
            }
        }

        protected void bSelectBuyer_Click(object sender, EventArgs e)
        {
            //GAB 06/17/2023 Clearing input boxes when choosing another broker
            ClearBrokerDetails();

            //Clearing All of first page
            txtResidenceNo.Text = null;
            txtSpouse.Text = null;
            ddCivilStatus.SelectedValue = "---SELECT CIVIL STATUS---";
            txtHlurb.Text = null;
            txtHlurbNo.Text = null;
            ddValidID.SelectedValue = "---Select Valid ID---";
            txtIDNo.Text = null;
            ddValidID2.SelectedValue = "---Select Valid ID---";
            txtIDNo2.Text = null;
            ddlVATCode.SelectedValue = "-1";
            ddlWTAXCode.SelectedValue = "-1";
            txtAddressBusinessComments.Text = null;
            txtContactFName.Text = null;
            txtContactMName.Text = null;
            txtContactLName.Text = null;
            txtContactPersonPosition.Text = null;
            txtContactEmail.Text = null;
            txtContactMobile.Text = null;
            txtContactAddress.Text = null;
            txtCorpCitizenship.SelectedValue = "filipino";
            ddContactValidID.SelectedValue = "---Select Valid ID---";
            txtContactValidIDNo.Text = null;
            txtContactPlaceIssued.Text = null;
            txtAuthorizedRepresentative2.Text = null;
            txtPRCLicenseInformationComments.Text = null;
            txtPTRNumber.Text = null;
            txtPTRValidFrom.Text = null;
            txtPTRValidTo.Text = null;
            txtPRCRegis.Text = null;
            txtRegistrationDate.Text = null;
            txtPRCLicenseExpirationDate.Text = null;
            txtAIPOOrganization.Text = null;
            txtAIPOValidFrom.Text = null;
            txtAIPOValidTo.Text = null;
            txtAIPOReceiptNo.Text = null;


            //2023-07-10 BAKIT INALIS LAMAN NG DOCUMENTS GRIDVIEW?
            //gvStandardDocumentRequirements.DataSource = null;
            //gvStandardDocumentRequirements.DataBind();
            //gvSolePropDocumentRequirements.DataSource = null;
            //gvSolePropDocumentRequirements.DataBind();
            //gvPartnershipDocumentRequirements.DataSource = null;
            //gvPartnershipDocumentRequirements.DataBind();
            //gvCorporationDocumentRequirements.DataSource = null;
            //gvCorporationDocumentRequirements.DataBind();

            DataTable dt2 = new DataTable();
            string qry1 = "";
            //qry1 = @"SELECT ""Code"", ""Name"" FROM OLST WHERE ""GrpCode"" = 'ID' ORDER BY ""Code""";
            qry1 = @"SELECT ""Code"", ""Name"" FROM OLST WHERE ""GrpCode"" = 'ID' AND ""IsShow"" = True ORDER BY ""Code""";
            dt2 = hana.GetData(qry1, hana.GetConnection("SAOHana"));
            dt2.Rows.Add(
                "---Select Valid ID---",
                "---Select Valid ID---"
                );
            ddValidID.DataSource = dt2;
            ddValidID.DataBind();
            ddValidID.SelectedValue = "---Select Valid ID---";
            ddValidID2.DataSource = dt2;
            ddValidID2.DataBind();
            ddValidID2.SelectedValue = "---Select Valid ID---";
            ddValidID3.DataSource = dt2;
            ddValidID3.DataBind();
            ddValidID3.SelectedValue = "---Select Valid ID---";
            ddValidID4.DataSource = dt2;
            ddValidID4.DataBind();
            ddValidID4.SelectedValue = "---Select Valid ID---";
            ViewState["ValidId"] = dt2;

            ddContactValidID.DataSource = dt2;
            ddContactValidID.DataBind();
            ddContactValidID.SelectedValue = "---Select Valid ID---";

            LinkButton GetBrokerID = (LinkButton)sender;
            string BrokerID = (GetBrokerID.CommandArgument);
            ViewState["BrokerID"] = BrokerID;
            LoadDataheader(BrokerID);

            //CHECK IF SOLE PROPRIETOR
            string typeOfBusiness = ViewState["businesstype"].ToString().ToUpper();
            if (typeOfBusiness == "SOLE PROPRIETOR")
            {
                divSoleProp.Attributes.Add("class", "row show");
                divPartnership.Attributes.Add("class", "row hidden");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowSupplementaryDetails();", true);

                RequiredFieldValidator47.Enabled = true;
                RequiredFieldValidator24.Enabled = true;
                RequiredFieldValidator13.Enabled = true;
                RequiredFieldValidator44.Enabled = true;
                //Hide Contact Details with validators 
                //RequiredFieldValidator46.Enabled = true;
                //RequiredFieldValidator59.Enabled = true;
                //RequiredFieldValidator54.Enabled = true;
                //RequiredFieldValidator55.Enabled = true;

                //RequiredFieldValidator56.Enabled = true;
                //RequiredFieldValidator60.Enabled = true;
                //RequiredFieldValidator57.Enabled = true;
                //RequiredFieldValidator58.Enabled = true;
                //RequiredFieldValidator61.Enabled = true;
                //RequiredFieldValidator63.Enabled = true;
                //RequiredFieldValidator65.Enabled = true;

                //RequiredFieldValidator62.Enabled = true;
                //RequiredFieldValidator64.Enabled = true;
                //RequiredFieldValidator66.Enabled = true;
                //End//


            }
            else
            {
                divSoleProp.Attributes.Add("class", "row hidden");
                divPartnership.Attributes.Add("class", "row show");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideSupplementaryDetails();", true);



                //Hide Contact Details with validators 
                //RequiredFieldValidator46.Enabled = true;
                //RequiredFieldValidator54.Enabled = true;
                //RequiredFieldValidator55.Enabled = true;
                //RequiredFieldValidator56.Enabled = false;
                //RequiredFieldValidator57.Enabled = false;
                //RequiredFieldValidator58.Enabled = false;

                RequiredFieldValidator47.Enabled = false;
                RequiredFieldValidator24.Enabled = false;
                RequiredFieldValidator13.Enabled = false;
                RequiredFieldValidator44.Enabled = false;

                RequiredFieldValidator59.Enabled = false;

                ///COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                //RequiredFieldValidator60.Enabled = false;

                RequiredFieldValidator61.Enabled = false;
                RequiredFieldValidator63.Enabled = false;
                RequiredFieldValidator65.Enabled = false;

                RequiredFieldValidator62.Enabled = false;
                RequiredFieldValidator64.Enabled = false;
                RequiredFieldValidator66.Enabled = false;
                //End//
            }



            #region For Approval
            //CHECK STATUS
            GridViewRow row = (GridViewRow)GetBrokerID.NamingContainer;
            string status = row.Cells[2].Text;
            ViewState["Status"] = status;
            string id = Session["UserID"].ToString();


            //CHECK USERID
            DataTable dtaccess = (DataTable)Session["UserAccess"];

            if (dtaccess == null)
            {
                //alertMsg("No authorization detected. Please re-login.", "warning");
                alertMsg("Your session has expired. Please re-login.", "warning");
            }
            else
            {
                var userid = dtaccess.Select($"CodeEncrypt= 'BRK1'");

                if (status == "ACCEPTED")
                {
                    enableControls(false);
                    btnSubmitDocument.Text = "Update";
                    //btnSubmitDocument.InnerText = "Update";

                }
                else if ((status == "PENDING" && userid.Any()) || id == "1")
                {
                    enableControls(true);
                    btnSubmitDocument.Text = "Decision";
                    //btnSubmitDocument.InnerText = "Decision";
                }
                else if (status.Contains("FOR REVISION") || status.Contains("REJECTED") || status.Contains("RENEWAL"))
                {
                    enableControls(true);
                    btnSubmitDocument.Visible = false;
                }
                #endregion

                if (status != "ACCEPTED")
                {
                    //Email notification to Broker upon receipt of application
                    string emailaddress = ViewState["businesstype"].ToString() == "SOLE PROPRIETOR" ? txtEmailAddress.Text.Trim() : txtContactEmail.Text.Trim();
                    string qry = $@"SELECT * FROM ""@EMAIL"" WHERE ""Code"" = 'BRKNGNG'";
                    DataTable dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
                    ws.sendEmail(BrokerID,
                                            //ConfigSettings.EmailSubjectApprovalOnGoing"].ToString() + DateTime.Now.ToString("yyyy-MM-dd"),
                                            DataAccess.GetData(dt, 0, "U_Subject", "").ToString(),
                                            emailaddress,
                                            //ConfigSettings.EmailBodySubjectApprovalOnGoing"].ToString(),
                                            DataAccess.GetData(dt, 0, "U_Body", "").ToString(),
                                            "",
                                            "",
                                            "Broker ID",
                                            DataAccess.GetData(dt, 0, "Code", "").ToString());
                }
            }


        }


        void enableControls(bool val)
        {

            txtContactFName.ReadOnly = val;
            txtContactMName.ReadOnly = val;
            txtContactLName.ReadOnly = val;
            txtContactEmail.ReadOnly = val;
            txtContactMobile.ReadOnly = val;
            txtContactAddress.ReadOnly = val;
            txtContactResidence.ReadOnly = val;
            txtContactValidIDNo.ReadOnly = val;
            txtContactPlaceIssued.ReadOnly = val;
            txtAuthorizedRepresentative2.ReadOnly = val;

            txtATPExpiryDate.ReadOnly = val;
            txtPartnership.ReadOnly = val;
            txtSECRegNo.ReadOnly = val;
            txtLastName.ReadOnly = val;
            txtFirstName.ReadOnly = val;
            txtMiddleName.ReadOnly = val;
            txtNickName.ReadOnly = val;

            txtAddress.ReadOnly = val;
            txtCity.ReadOnly = val;
            txtZipCode.ReadOnly = val;
            txtNatureOfBusiness.ReadOnly = val;
            txtBusinessName.ReadOnly = val;
            txtBusinessAddress.ReadOnly = val;
            txtBusinessZipCode.ReadOnly = val;
            txtBusinessPhoneNo.ReadOnly = val;
            txtFaxNo.ReadOnly = val;
            txtEmailAddress.ReadOnly = val;

            txtProvince.ReadOnly = val;

            dtDateOfBirth.ReadOnly = val;
            txtPlaceOfBirth.ReadOnly = val;
            txtReligion.ReadOnly = val;
            txtCitizenship.Disabled = val;
            txtCorpReligion.ReadOnly = val;
            txtTIN.ReadOnly = val;
            //txtSSS.ReadOnly = val;
            //txtPassport.ReadOnly = val;
            //txtPassportValid.ReadOnly = val;

            txtResidenceNo.ReadOnly = val;
            txtMobileNo.ReadOnly = val;
            //txtPassportValidTo.ReadOnly = val;
            txtSpouse.ReadOnly = val;
            txtPRCLicenseRegistration.ReadOnly = val;
            txtAIPOOrganization.ReadOnly = val;
            txtAIPOValidFrom.ReadOnly = val;
            txtAIPOValidTo.ReadOnly = val;
            txtAIPOReceiptNo.ReadOnly = val;
            txtDesignation.ReadOnly = val;

            //2027-07-07 : NEW FIELD DESIGNATION
            txtDesignation2.ReadOnly = val;


            txtIDNo.ReadOnly = val;
            txtIDNo2.ReadOnly = val;
            txtIDNo3.ReadOnly = val;
            txtIDNo4.ReadOnly = val;
            txtIDExpirationDate.ReadOnly = val;
            txtIDExpirationDate2.ReadOnly = val;
            txtIDExpirationDate3.ReadOnly = val;
            txtIDExpirationDate4.ReadOnly = val;

            txtOthers1.ReadOnly = val;
            txtOthers2.ReadOnly = val;
            txtOthers3.ReadOnly = val;
            txtOthers4.ReadOnly = val;
            txtHlurb.ReadOnly = val;
            txtHlurbNo.ReadOnly = val;
            txtAuthorizedRepresentative.ReadOnly = val;
            txtTradeName.ReadOnly = val;
            //txtCommitAffiliationDate.ReadOnly = val;

            txtContactPersonPosition.ReadOnly = val;

            txtOthersContactInfo.ReadOnly = val;
            //RequiredFieldValidator14.Enabled = !val;

            txtRegistrationDate.ReadOnly = val;

            //CustomValidator10.Enabled = !val;
            //CustomValidator9.Enabled = !val;
            //CustomValidator8.Enabled = !val;
            //CustomValidator7.Enabled = !val;
            //CustomValidator5.Enabled = !val;
            //CustomValidator4.Enabled = !val;
            CustomValidator3.Enabled = !val;

            ///COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
            //CustomValidator6.Enabled = !val;
            //CustomValidator2.Enabled = !val;
            //CustomValidator1.Enabled = !val;


            if (val)
            {
                btnCreateusers.Visible = false;
                divReports.Visible = false;

                ddContactPosition.Attributes.Add("disabled", "disabled");
                ddContactValidID.Attributes.Add("disabled", "disabled");
                //dpIssuedBy.Attributes.Add("disabled", "disabled");
                ddPosition.Attributes.Add("disabled", "disabled");
                ddlVATCode2.Attributes.Add("disabled", "disabled");
                ddlWTAXCode2.Attributes.Add("disabled", "disabled");
                ddlWTAXCode.Attributes.Add("disabled", "disabled");
                ddSex.Attributes.Add("disabled", "disabled");
                ddValidID.Attributes.Add("disabled", "disabled");
                ddValidID2.Attributes.Add("disabled", "disabled");
                ddValidID3.Attributes.Add("disabled", "disabled");
                ddlVATCode.Attributes.Add("disabled", "disabled");
                ddValidID4.Attributes.Add("disabled", "disabled");
                ddCivilStatus.Attributes.Add("disabled", "disabled");
                txtCorpCitizenship.Attributes.Add("disabled", "disabled");
                ddlLocation.Attributes.Add("disabled", "disabled");
                btnAddSalesPerson.Visible = false;
                btnSaveSharingDetails.Visible = false;
                btnNewSalesPerson.Visible = false;
                btnAddSharingDetails.Visible = false;
                btnRegisterSalesAgent.Visible = false;
                btnRegisterSalesPerson2.Visible = false;
                btnListOfSalesPersonSharingDetails.Disabled = true;
                mtxtPecent.ReadOnly = true;
                mtxtHouseAndLotPecent.ReadOnly = true;

                btnAddToSharing.Visible = false;
                btnBackToProjList.Visible = false;

            }
            else
            {
                //GAB 07/06/2023 COMMENTED REASON: WRONG QUERY NOW
                //string qry = $@"SELECT ""CardCode"" FROM OCRD WHERE ""U_BrokerCode"" = '{ViewState["BrokerID"].ToString()}'";

                string qry = $@"SELECT TOP 1 *
                                FROM ""BRK1""
                                WHERE ""BrokerId"" = '{ViewState["BrokerID"].ToString()}'
	                                AND ifnull(""SAPCardCode"",'') =''";
                DataTable dt = hana.GetData(qry, hana.GetConnection("SAOHana"));

                //CREATE USERS AND BP
                if (dt.Rows.Count > 0)
                {
                    btnCreateusers.Visible = true;
                }
                else
                {
                    btnCreateusers.Visible = false;
                }

                divReports.Visible = true;

                //GAB 07/05/2023 COMMENTED REASON: OVERLAPPING ATTRIBUTES NOT WORKING PROPERLY
                //ddContactPosition.Attributes.Add("enabled", "enabled");
                //ddContactValidID.Attributes.Add("enabled", "enabled");
                ////dpIssuedBy.Attributes.Add("enabled", "enabled");
                //ddPosition.Attributes.Add("enabled", "enabled");
                //ddlVATCode2.Attributes.Add("enabled", "enabled");
                //ddlWTAXCode2.Attributes.Add("enabled", "enabled");
                //ddlWTAXCode.Attributes.Add("enabled", "enabled");
                //ddSex.Attributes.Add("enabled", "enabled");
                //ddCivilStatus.Attributes.Add("enabled", "enabled");
                //ddValidID.Attributes.Add("enabled", "enabled");
                //ddValidID2.Attributes.Add("enabled", "enabled");
                //ddValidID3.Attributes.Add("enabled", "enabled");
                //ddValidID4.Attributes.Add("enabled", "enabled");
                //ddlVATCode.Attributes.Add("enabled", "enabled");
                //txtCorpCitizenship.Attributes.Add("enabled", "enabled");
                //ddlLocation.Attributes.Add("enabled", "enabled");
                btnAddSalesPerson.Visible = true;
                btnSaveSharingDetails.Visible = true;
                btnNewSalesPerson.Visible = true;
                btnAddSharingDetails.Visible = true;
                btnRegisterSalesAgent.Visible = true;
                btnRegisterSalesPerson2.Visible = true;
                btnListOfSalesPersonSharingDetails.Disabled = false;
                mtxtPecent.ReadOnly = false;
                mtxtHouseAndLotPecent.ReadOnly = false;

                btnAddToSharing.Visible = true;
                btnBackToProjList.Visible = true;

                //GAB 07/05/2023
                ddContactPosition.Attributes.Remove("disabled");
                ddContactValidID.Attributes.Remove("enabled");
                //dpIssuedBy.Attributes.Remove("enabled");
                ddPosition.Attributes.Remove("enabled");
                ddlVATCode2.Attributes.Remove("enabled");
                ddlWTAXCode2.Attributes.Remove("enabled");
                ddlWTAXCode.Attributes.Remove("enabled");
                ddSex.Attributes.Remove("enabled");
                ddCivilStatus.Attributes.Remove("enabled");
                ddValidID.Attributes.Remove("enabled");
                ddValidID2.Attributes.Remove("enabled");
                ddValidID3.Attributes.Remove("enabled");
                ddValidID4.Attributes.Remove("enabled");
                ddlVATCode.Attributes.Remove("enabled");
                txtCorpCitizenship.Attributes.Remove("enabled");
                ddlLocation.Attributes.Remove("enabled");
            }

            //txtPlaceIssued.ReadOnly = val;
            txtPRCRegis.ReadOnly = val;
            txtPRCLicenseExpirationDate.ReadOnly = val;
            txtPTRNumber.ReadOnly = val;
            txtPTRValidFrom.ReadOnly = val;
            txtPTRValidTo.ReadOnly = val;

            mtxtSalesPerson.ReadOnly = val;
            mtxtEmail.ReadOnly = val;
            mtxtPRCLicense.ReadOnly = val;
            mtxtPRCLicenseExpirationDate.ReadOnly = val;
            mtxtATPDate.ReadOnly = val;
            mtxtTIN.ReadOnly = val;
            mtxtMobile.ReadOnly = val;



        }

        protected void btnApprovalType_ServerClick(object sender, EventArgs e)
        {


            if (Session["UserID"] == null)
            {
                alertMsg("Session expired!", "error");
                Response.Redirect("~/pages/Login.aspx");
            }
            else
            {
                string tempGenInfoComments = txtGeneralInfoComments.Text;
                string tempAddressBisComments = txtAddressBusinessComments.Text;
                string tempSuppDetailsComments = txtSupplementaryDetailsComments.Text;
                string tempPRCLicInfoComments = txtPRCLicenseInformationComments.Text;



                string ret = "Operation completed successfully.";
                string document = ws.updateBrokerApproval(
                                                               ViewState["BrokerID"].ToString(),
                                                               //rbApprovalType.SelectedValue.ToUpper().Trim(),
                                                               "",
                                                               txtGeneralInfoComments.Text,
                                                               txtAddressBusinessComments.Text,
                                                               txtSupplementaryDetailsComments.Text,
                                                               txtPRCLicenseInformationComments.Text
                                                               );
                if (document != "Operation completed successfully.")
                {
                    rollbackComments(tempGenInfoComments, tempAddressBisComments, tempSuppDetailsComments, tempPRCLicInfoComments);
                    ret = $"Failed to submit your documents! Please contact your administrator (Error: HeaderComments - {document})";
                    alertMsg(ret, "error");
                }
                else
                {
                    document = updateStandardDocumentComments("txtStandardAttachmentComments", ViewState["BrokerID"].ToString(), rbTypeOfBusiness.SelectedItem.Text.Trim(), "chkStandard");
                    document = updateDocumentComments(gvStandardDocumentRequirements, "txtStandardAttachmentComments", ViewState["BrokerID"].ToString(), "Standard", "chkStandard", "lblFileName", "txtStandardAttachmentDateIssued");

                    if (document != "Operation completed successfully.")
                    {
                        rollbackComments(tempGenInfoComments, tempAddressBisComments, tempSuppDetailsComments, tempPRCLicInfoComments);
                        ret = $"Failed to submit your documents! Please contact your administrator (Error: DocumentComments - {document})";
                        alertMsg(ret, "error");
                    }
                    else
                    {
                        string textbox = "";
                        string checkbox = "";
                        string date = "";
                        string filename = "";
                        GridView gv;
                        if (rbTypeOfBusiness.SelectedIndex == 0)
                        {
                            textbox = "txtSolePropDocumentRequirementsComments";
                            gv = gvSolePropDocumentRequirements;
                            checkbox = "chkSole";
                            date = "txtSolePropDocumentRequirementsDateIssued";
                            filename = "lblFileName2";
                        }
                        else if (rbTypeOfBusiness.SelectedIndex == 1)
                        {
                            textbox = "txtPartnershipDocumentRequirementsComments";
                            gv = gvPartnershipDocumentRequirements;
                            checkbox = "chkPartner";
                            date = "txtPartnershipDocumentRequirementsDateIssued";
                            filename = "lblFileName3";
                        }
                        else
                        {
                            textbox = "txtCorporationDocumentRequirementsComments";
                            gv = gvCorporationDocumentRequirements;
                            checkbox = "chkCorp";
                            date = "txtCorporationDocumentRequirementsDateIssued";
                            filename = "lblFileName4";
                        }

                        document = updateDocumentComments(gv, textbox, ViewState["BrokerID"].ToString(), rbTypeOfBusiness.SelectedItem.Text.Trim(), checkbox, filename, date);

                        if (document != "Operation completed successfully.")
                        {
                            rollbackComments(tempGenInfoComments, tempAddressBisComments, tempSuppDetailsComments, tempPRCLicInfoComments);
                            ret = $"Failed to submit your documents! Please contact your administrator (Error: DocumentComments - {document})";
                            alertMsg(ret, "error");
                        }
                        else
                        {
                            //CHECK USERID
                            DataTable dtaccess = (DataTable)Session["UserAccess"];
                            var userid = dtaccess.Select($"CodeEncrypt= 'BRK1'");
                            string id = Session["UserID"].ToString();
                            //CHECK STATUS
                            string status = ViewState["Status"].ToString();
                            //CHECK IF THE SELECTED VALUE IS  ACCEPTED AND IF IT'S USER ID IS SALES01(SIR BUTCH)

                            //GET STATUS FOR EMAIL NOTIFICATION                            
                            Session["BrkStatus"] = status;
                            if (status != "ACCEPTED")
                            {
                                hana.Execute($@"UPDATE OBRK SET ""Status"" = '{rbApprovalType.SelectedValue.ToUpper().Trim()}' Where ""BrokerId"" = '{ ViewState["BrokerID"].ToString()}'", hana.GetConnection("SAOHana"));

                                //GET EMAIL
                                string qry = $@"SELECT * FROM ""@EMAIL"" WHERE ""Code"" = 'BRKAPRV'";
                                DataTable dt = hana.GetData(qry, hana.GetConnection("SAPHana"));

                                string emailaddress = ViewState["businesstype"].ToString() == "SOLE PROPRIETOR" ? txtEmailAddress.Text.Trim() : txtEmailAddress.Text.Trim() + "," + txtContactEmail.Text.Trim();

                                if (rbApprovalType.SelectedValue.ToUpper().Trim() == "ACCEPTED")
                                {
                                    if (ViewState["StatusBRK"].ToString() == "RENEWAL")
                                    {
                                        DataTable dt2 = hana.GetData($@"SELECT ""Name"" FROM ""OLST"" Where ""Code"" = 'BRK'", hana.GetConnection("SAOHana"));
                                        DateTime brokerRenewalDateExpiry = Convert.ToDateTime(DataAccess.GetData(dt, 0, "Name", ""));
                                        hana.Execute($@"UPDATE OBRK SET ""Status"" = 'ACCEPTED', ""ApprovalDate"" = {brokerRenewalDateExpiry}, ""EmailNotifStage"" = '0' Where ""BrokerId"" = '{ ViewState["BrokerID"].ToString()}'", hana.GetConnection("SAOHana"));
                                    }
                                    else
                                    {
                                        hana.Execute($@"UPDATE OBRK SET ""Status"" = 'ACCEPTED', ""ApprovalDate"" = CURRENT_DATE, ""EmailNotifStage"" = '0' Where ""BrokerId"" = '{ ViewState["BrokerID"].ToString()}'", hana.GetConnection("SAOHana"));
                                    }
                                    Session["BrkStatus"] = rbApprovalType.SelectedValue.ToUpper().Trim();
                                    ws.sendEmail(
                                            ViewState["BrokerID"].ToString(),
                                            DataAccess.GetData(dt, 0, "U_Subject", "").ToString(),
                                            emailaddress,
                                            DataAccess.GetData(dt, 0, "U_Body", "").ToString(),
                                            DataAccess.GetData(dt, 0, "U_BPLink", "").ToString(),
                                            DataAccess.GetData(dt, 0, "U_BPLinkMessage", "").ToString(),
                                            "Broker ID",
                                            DataAccess.GetData(dt, 0, "Code", "").ToString());

                                    mtxtUniqueId.Text = $"Broker status updated successfully to: { rbApprovalType.SelectedValue.ToUpper().Trim()}.";



                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ModalApprovalType_Hide();", true);
                                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "ShowSuccessAlert();", true);
                                }
                                else
                                {
                                    Session["BrkStatus"] = rbApprovalType.SelectedValue.ToUpper().Trim();
                                    if (status == "FOR REVISION")
                                    {
                                        qry = $@"SELECT * FROM ""@EMAIL"" WHERE ""Code"" = 'BRKRVSN'";
                                        dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
                                        ws.sendEmail(ViewState["BrokerID"].ToString(),
                                                         DataAccess.GetData(dt, 0, "U_Subject", "").ToString(),
                                                         emailaddress,
                                                         DataAccess.GetData(dt, 0, "U_Body", "").ToString(),
                                                         DataAccess.GetData(dt, 0, "U_BPLink", "").ToString(),
                                                         DataAccess.GetData(dt, 0, "U_BPLinkMessage", "").ToString(),
                                                         "Broker ID",
                                                         DataAccess.GetData(dt, 0, "Code", "").ToString());
                                    }
                                    else
                                    {
                                        //ADD REASON IF STATUS IS REJECTED AND COMMENT IS NOT EMPTY
                                        if (!String.IsNullOrWhiteSpace(txtGeneralInfoComments.Text) && status == "REJECTED")
                                        {
                                            Session["BrkReason"] = "Reason: " + txtGeneralInfoComments.Text;
                                        }
                                        ws.sendEmail(ViewState["BrokerID"].ToString(),
                                                     DataAccess.GetData(dt, 0, "U_Subject", "").ToString(),
                                                     emailaddress,
                                                     DataAccess.GetData(dt, 0, "U_Body", "").ToString(),
                                                     DataAccess.GetData(dt, 0, "U_BPLink", "").ToString(),
                                                     DataAccess.GetData(dt, 0, "U_BPLinkMessage", "").ToString(),
                                                     "Broker ID",
                                                     DataAccess.GetData(dt, 0, "Code", "").ToString());
                                    }



                                    mtxtUniqueId.Text = $"Broker status updated to: { rbApprovalType.SelectedValue.ToUpper().Trim()}. This is sent to the applicant via email.";
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ModalApprovalType_Hide();", true);
                                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "ShowSuccessAlert();", true);

                                }
                            }

                        }
                    }

                }
            }
        }



        string updateDocumentComments(GridView gv, string textbox, string BrokerCode, string section, string checkbox, string filename, string date)
        {
            string ret = "Operation completed successfully.";
            foreach (GridViewRow row in gv.Rows)
            {
                string documentName = row.Cells[0].Text.Trim();
                string comments = ((TextBox)row.FindControl(textbox)).Text;
                string file = ((Label)row.FindControl(filename)).Text;
                string issuedate = Convert.ToDateTime(((TextBox)row.FindControl(date)).Text == "" ? null : ((TextBox)row.FindControl(date)).Text).ToString("yyyy-MM-dd");
                string document = "";

                bool chk = ((CheckBox)row.FindControl(checkbox)).Checked;
                string check;
                if (chk)
                {
                    check = "1";
                }
                else
                {
                    check = "0";
                }

                string qry = $@"SELECT * FROM ""BRK3"" WHERE ""DocumentName"" = '{documentName}' AND ""Section"" = '{section}' AND ""BrokerId"" = '{BrokerCode}'";
                DataTable dtBrk3Check = hana.GetData(qry, hana.GetConnection("SAOHana"));

                if (dtBrk3Check.Rows.Count != 0)
                {
                    document = ws.updateBrokerApprovalDocuments(
                                                    BrokerCode,
                                                     comments,
                                                     documentName,
                                                     section,
                                                     check,
                                                     file,
                                                     Convert.ToDateTime(issuedate)
                                                     );
                }
                else
                {
                    document = ws.addBrokerApprovalDocuments(
                                                    BrokerCode,
                                                     comments,
                                                     documentName,
                                                     section,
                                                     check,
                                                     file,
                                                     Convert.ToDateTime(issuedate)
                                                     );
                }

                if (document != "Operation completed successfully.")
                {
                    ret = $"Failed to submit your documents! Please contact your administrator (Error: DocumentUpdate - {document})";
                    break;
                }
            }
            return ret;
        }


        string updateStandardDocumentComments(string textbox, string BrokerCode, string section, string checkbox)
        {
            string ret = "Operation completed successfully.";
            foreach (GridViewRow row in gvStandardDocumentRequirements.Rows)
            {
                string documentName = row.Cells[0].Text.Trim();
                string comments = ((TextBox)row.FindControl(textbox)).Text;
                string date = ((TextBox)row.FindControl("txtStandardAttachmentDateIssued")).Text;
                string filename = ((Label)row.FindControl("lblFileName")).Text;

                bool chk = ((CheckBox)row.FindControl(checkbox)).Checked;
                string check;
                if (chk)
                {
                    check = "1";
                }
                else
                {
                    check = "0";
                }

                string docName = "";
                string approved = "";
                if (documentName == "Broker Application Form")
                {
                    docName = "BrokerApplicationForm";
                    approved = "Approved1";
                }
                else if (documentName == "List of Accredited Real Estate Sales Person")
                {
                    docName = "ListOfAccredited";
                    approved = "Approved2";
                }
                else if (documentName == "Accreditation Agreement")
                {
                    docName = "AccreditationAgreement";
                    approved = "Approved3";
                }
                else
                {
                    docName = "BrokerAccreditationGenrealPolicies";
                    approved = "Approved4";
                }

                bool exec = hana.Execute($@"UPDATE ""OBRK"" SET  ""{docName}IssuedDate"" = '{date}' ,""{docName}"" = '{filename}', ""UpdateDate"" = CURRENT_DATE, ""{docName}Comments"" = '{comments}', ""{approved}"" = '{check}' WHERE ""BrokerId"" = '{BrokerCode}';", hana.GetConnection("SAOHana"));

                if (!exec)
                {
                    ret = $"Failed to submit your documents! Please contact your administrator (Error: DocumentUpdate - Standard Documents Posting)";
                    break;
                }
            }
            return ret;
        }



        void rollbackComments(string generalInfo, string addressBusiness, string suppDetails, string PRClicense)
        {
            ws.updateBrokerApproval(
                                                          ViewState["BrokerID"].ToString(),
                                                          rbApprovalType.SelectedValue.ToUpper().Trim(),
                                                          generalInfo,
                                                          addressBusiness,
                                                          suppDetails,
                                                          PRClicense
                                                          );
        }

        protected void gvBrokerList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable dt = (DataTable)ViewState["BrokerList"];
            gvBrokerList.PageIndex = e.NewPageIndex;
            gvBrokerList.DataSource = dt;
            gvBrokerList.DataBind();

        }

        protected void btnRegisterSalesAgent_ServerClick(object sender, EventArgs e)
        {
            Label1.Text = "Add";
            ViewState["SalesPersonId"] = "";
            ClearRegisterSalesPerson();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowRegisterSalesAgentModal();", true);
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideAddSharingDetailsModal();", true);
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideAddSharingModal();", true);
        }
        protected void btnCloseSalesAgent_ServerClick(object sender, EventArgs e)
        {
            ClearRegisterSalesPerson();
            //GAB 07/05/2023 COMMENTED REASON: WENT TO BACKEND
            //GAB 07/02/2023
            //if (Session["AddSalesPerson"].ToString() == "SharingDetails")
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "none", "ComparingSharingDetailsTables();", true);
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "none", "ComparingSalesPersonTables();", true);
            //}

        }
        protected void btnRegisterdSalesAgent_ServerClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    var prcLicenseExpirationDate = Convert.ToDateTime("1900-01-01");
                    if (!string.IsNullOrEmpty(mtxtPRCLicenseExpirationDate.Text))
                    {
                        prcLicenseExpirationDate = Convert.ToDateTime(mtxtPRCLicenseExpirationDate.Text);
                    }

                    var atpExpiryDate = Convert.ToDateTime("1900-01-01");
                    if (!string.IsNullOrEmpty(mtxtATPDate.Text))
                    {
                        atpExpiryDate = Convert.ToDateTime(mtxtATPDate.Text);
                    }

                    string salesid = ViewState["SalesPersonId"].ToString();

                    if (String.IsNullOrWhiteSpace(salesid))
                    {
                        //Checking for Existing TIN Number
                        string TINNo = mtxtTIN.Text;
                        string qrytinno;

                        qrytinno = $@"SELECT ""TIN"" FROM ""OSLA"" WHERE ""TIN"" = '{TINNo}'";
                        DataTable tinNo = hana.GetData(qrytinno, hana.GetConnection("SAOHana"));
                        if (tinNo.Rows.Count > 0) throw new Exception("TIN Number Already Exists.");

                        ////Checking for Existing SalesPerson Name
                        //bool spExists = false;

                        //string SPName = mtxtRegisterSalesPersonName.Text.ToUpper().Trim();
                        //string qrysalesperson = $@"SELECT ""SalesPerson"" FROM ""OSLA"" WHERE ""SalesPerson"" = '{SPName}'";
                        //DataTable salesPerson = hana.GetData(qrysalesperson, hana.GetConnection("SAOHana"));

                        //if(salesPerson.Rows.Count > 0)
                        //{
                        //    spExists = true;
                        //}

                        bool hasduplicate = false;
                        //GET PRC NUMBER
                        string brkprc = mtxtPRCLicense.Text.ToString().Trim().ToLower();

                        //GET EXISTING PRC NUMBER OF OSLA
                        string qryosla = $@"SELECT ""PRCLicense"" FROM ""OSLA""";
                        DataTable dtoslaprc = hana.GetData(qryosla, hana.GetConnection("SAOHana"));

                        //CHECK IF THE PRC NUMBER IS ALREADY EXISTING
                        foreach (DataRow row in dtoslaprc.Rows)
                        {
                            if (row["PRCLicense"].ToString().ToLower() == brkprc && mtxtPRCLicense.Text != "")
                            {
                                hasduplicate = true;
                            }
                        }

                        if (hasduplicate != true)
                        {
                            var ticks = DateTime.Now.Ticks;
                            var guid = Guid.NewGuid().ToString();
                            var uniqueSessionId = ticks.ToString() + guid.ToString();
                            string[] agentCode = uniqueSessionId.Split('-');


                            string id = agentCode.Last();
                            string post;
                            post = ws.registerSalesAgent(
                                                        id,
                                                        mtxtRegisterSalesPersonName.Text.ToUpper(),
                                                        mtxtEmail.Text.Trim(),
                                                        ddPosition.SelectedItem.Text,
                                                        mtxtPRCLicense.Text,
                                                        prcLicenseExpirationDate,
                                                        atpExpiryDate,
                                                        mtxtTIN.Text,
                                                        ddlVATCode2.SelectedValue,
                                                        ddlWTAXCode2.SelectedValue,
                                                        mtxtMobile.Text,
                                                        DateTime.Now,
                                                        txtSalesHLURBNo.Text,
                                                        txtSalesPTRNo.Text,
                                                        lblBrokerID.Text
                                                        );

                            if (post == "Operation completed successfully.")
                            {

                                //REPOPULATE SALES AGENT DATA
                                //string qry = $@"SELECT A.""Id"",A.""SalesPerson"" ""SalesPersonName"", B.""SAPCardCode"" ""Username"", A.""EmailAddress"", A.""Position"", A.""PRCLicense"", A.""PRCLicenseExpirationDate"", 
                                //TO_VARCHAR (TO_DATE(A.""ATPDateSalesPerson""), 'YYYY/MM/DD') ""ATPDate"",A.""TIN"", A.""VATCode"", A.""WTaxCode"", 
                                //A.""MobileNumber"", TO_VARCHAR (TO_DATE(B.""ValidFrom""), 'YYYY/MM/DD') ""ValidFrom"" ,  TO_VARCHAR (TO_DATE(B.""ValidTo""), 'YYYY/MM/DD') ""ValidTo"" ,A.""HLURBLicenseNo"",A.""PTRNo""
                                //FROM ""OSLA"" A INNER JOIN ""BRK1"" B ON A.""Id"" = B.""Id""  WHERE B.""BrokerId"" = '{lblBrokerID.Text}'";

                                ////GAB 06/22/2023
                                //string qry = $@"SELECT A.""Id"",
                                //    A.""SalesPerson"" ""SalesPersonName"",
                                //    B.""SAPCardCode"" ""Username"",
                                //    A.""EmailAddress"",
                                //    A.""Position"",
                                //    A.""PRCLicense"",
                                //    TO_VARCHAR (TO_DATE(A.""PRCLicenseExpirationDate""), 'YYYY-MM-DD') ""PRCLicenseExpirationDate"",
                                //    TO_VARCHAR (TO_DATE(A.""ATPDateSalesPerson""), 'YYYY-MM-DD') ""ATPDate"",
                                //    A.""TIN"",
                                //    A.""VATCode"",
                                //    A.""WTaxCode"", 
                                //    A.""MobileNumber"",
                                //    TO_VARCHAR (TO_DATE(B.""ValidFrom""), 'YYYY-MM-DD') ""ValidFrom"",
                                //    TO_VARCHAR (TO_DATE(B.""ValidTo""), 'YYYY-MM-DD') ""ValidTo"",
                                //    A.""HLURBLicenseNo"",A.""PTRNo""
                                //FROM ""OSLA"" A INNER JOIN 
                                // ""BRK1"" B ON A.""Id"" = B.""Id""
                                //WHERE B.""BrokerId"" = '{lblBrokerID.Text}';";


                                //ViewState["SalesPerson"] = hana.GetData(qry, hana.GetConnection("SAOHana"));
                                //BindGrid();

                                //ADD NEW SALES AGENT TO LIST
                                DataTable dt = (DataTable)ViewState["SalesPerson"];
                                dt.Rows.Add(
                                       id,
                                       mtxtRegisterSalesPersonName.Text.ToUpper(),
                                       "",
                                       mtxtEmail.Text.Trim(),
                                       ddPosition.SelectedItem.Text,
                                       mtxtPRCLicense.Text,
                                       prcLicenseExpirationDate.ToString("yyyy-MM-dd"),
                                       atpExpiryDate.ToString("yyyy-MM-dd"),
                                       mtxtTIN.Text,
                                       ddlVATCode2.SelectedValue,
                                       ddlWTAXCode2.SelectedValue,
                                       mtxtMobile.Text,
                                       Convert.ToDateTime(DateTime.Now.Date).ToString("yyyy-MM-dd"),
                                       Convert.ToDateTime(DateTime.Now.Date.AddYears(1)).ToString("yyyy-MM-dd"),
                                       txtSalesHLURBNo.Text,
                                       txtSalesPTRNo.Text);

                                ViewState["SalesPerson"] = dt;
                                BindGrid();

                                ClearRegisterSalesPerson();

                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideRegisterSalesAgentModal();", true);
                                alertMsg("Operation completed successfully.", "success");
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideAddSalesPersonModal();", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "HideAddSalesPersonModal();", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "CloseModalsAfterUpdateSalesPerson();", true);
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideRegisterSalesAgentModal();", true);
                                alertMsg($"Failed to submit your documents! Please contact your administrator(Error: Sales Agent Registration - {post})", "error");
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideAddSalesPersonModal();", true);


                            }
                        }
                        else
                        {
                            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideRegisterSalesAgentModal();", true);
                            alertMsg($"Sales agent with same PRC number already exist", "error");
                            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideAddSalesPersonModal();", true);
                        }
                    }
                    else
                    {
                        //insert checking for existing TIN here
                        string TINNo = mtxtTIN.Text;
                        string qrytinno;

                        qrytinno = $@"SELECT ""TIN"" FROM ""OSLA"" WHERE ""TIN"" = '{TINNo}' AND ""Id"" <> '{salesid}'";
                        DataTable tinNo = hana.GetData(qrytinno, hana.GetConnection("SAOHana"));
                        if (tinNo.Rows.Count > 0) throw new Exception("TIN Number Already Exists.");

                        string qry = $@"UPDATE OSLA SET 			
			                            ""SalesPerson"" = '{mtxtRegisterSalesPersonName.Text.ToUpper()}',
			                            ""EmailAddress"" = '{mtxtEmail.Text.Trim()}',
			                            ""Position"" = '{ddPosition.SelectedItem.Text}',  
			                            ""PRCLicense"" = '{mtxtPRCLicense.Text}',
			                            ""PRCLicenseExpirationDate"" = '{mtxtPRCLicenseExpirationDate.Text}',
			                            ""ATPDateSalesPerson"" = '{mtxtATPDate.Text}',
			                            ""TIN"" = '{mtxtTIN.Text}',
			                            ""VATCode"" = '{ddlVATCode2.SelectedValue}',
			                            ""WTaxCode"" = '{ddlWTAXCode2.SelectedValue}', 
			                            ""MobileNumber"" = '{mtxtMobile.Text}',
			                            ""UpdateDate"" = '{DateTime.Now.ToString("yyyy-MM-dd")}',
			                            ""HLURBLicenseNo"" = '{txtSalesHLURBNo.Text}',
			                            ""PTRNo"" = '{txtSalesPTRNo.Text}'
                                where ""Id"" = '{salesid}'";
                        bool ret = hana.Execute(qry, hana.GetConnection("SAOHana"));

                        if (ret)
                        {
                            ClearRegisterSalesPerson();

                            //REPOPULATE SALES AGENT DATA
                            //GAB 07/02/2023 Edited query PRCLicenseExpirationDate should be converted to a date first then a string
                            qry = $@"SELECT A.""Id"",
                                    A.""SalesPerson"" ""SalesPersonName"",
                                    B.""SAPCardCode"" ""Username"", 
                                    A.""EmailAddress"",
                                    A.""Position"",
                                    A.""PRCLicense"",
                                    --A.""PRCLicenseExpirationDate"",
                                    TO_VARCHAR (TO_DATE(A.""PRCLicenseExpirationDate""), 'YYYY/MM/DD') ""PRCLicenseExpirationDate"",
                                    TO_VARCHAR (TO_DATE(A.""ATPDateSalesPerson""), 'YYYY/MM/DD') ""ATPDate"",
                                    A.""TIN"",
                                    A.""VATCode"",
                                    A.""WTaxCode"",
                                    A.""MobileNumber"",
                                    TO_VARCHAR (TO_DATE(B.""ValidFrom""), 'YYYY/MM/DD') ""ValidFrom"",
                                    TO_VARCHAR (TO_DATE(B.""ValidTo""), 'YYYY/MM/DD') ""ValidTo"",
                                    A.""HLURBLicenseNo"",A.""PTRNo""
                                    FROM ""OSLA"" A INNER JOIN 
                                    ""BRK1"" B ON A.""Id"" = B.""Id""
                                    WHERE B.""BrokerId"" = '{lblBrokerID.Text}'";
                            ViewState["SalesPerson"] = hana.GetData(qry, hana.GetConnection("SAOHana"));
                            BindGrid();

                            //REPOPULATE SALES PERSON LIST DATA
                            DataTable dt = new DataTable();
                            if (!string.IsNullOrEmpty(txtSalesSearch.Value))
                            {
                                dt = hana.GetData($"CALL sp_OSLASearch ('{txtSalesSearch.Value}','{lblBrokerID.Text}');", hana.GetConnection("SAOHana"));
                            }
                            else
                            {
                                qry = $@"SELECT * FROM ""OSLA"" A WHERE A.""CreateBrokerID"" = '{lblBrokerID.Text}' ";


                                dt = hana.GetData(qry, hana.GetConnection("SAOHana"));

                            }

                            dt.DefaultView.Sort = "Id ASC";
                            dt = dt.DefaultView.ToTable();
                            gvSalesPersons.DataSource = dt;

                            gvSalesPersons.DataBind();

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideRegisterSalesAgentModal();", true);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideListOfSalesPersons();", true);
                            alertMsg("Operation completed successfully.", "success");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideAddSalesPersonModal();", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "CloseModalsAfterUpdateSalesPerson();", true);
                        }
                        else
                        {
                            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideRegisterSalesAgentModal();", true);
                            alertMsg($"Failed to update your documents! Please contact your administrator", "error");
                            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideAddSalesPersonModal();", true);
                        }
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "HideAddSalesPersonModal();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "CloseModalsAfterUpdateSalesPerson();", true);
                }
                catch (Exception ex)
                {
                    alertMsg(ex.Message, "error");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideAddSalesPersonModal();", true);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideListOfSalesPersons();", true);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideRegisterSalesAgentModal();", true);
                }
            }
        }


        protected void btnListOfSalesPerson_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Control btn = (Control)sender;
                string id = btn.ID;
                ViewState["TriggerButton"] = id;
                //string qry = $@"SELECT * FROM ""OSLA"" A WHERE A.""CreateBrokerID"" = '{lblBrokerID.Text}'";
                string qry = $@"SELECT 
                            A.""Id"",
                            A.""SalesPerson"",
                            A.""Position"", 
                            A.""PRCLicense"", 
                            TO_VARCHAR (TO_DATE(A.""PRCLicenseExpirationDate""), 'YYYY-MM-DD') ""PRCLicenseExpirationDate"",
                            TO_VARCHAR (TO_DATE(A.""ATPDateSalesPerson""), 'YYYY-MM-DD') ""ATPDateSalesPerson""
                        FROM
                            ""OSLA"" A 
                        WHERE 
                            A.""CreateBrokerID"" = '{lblBrokerID.Text}'";


                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();
                dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
                if (ViewState["SalesorShare"].ToString() == "Share")
                {

                    dt2.Columns.AddRange(new DataColumn[6]
                        {
                        new DataColumn("Id"),
                        new DataColumn("SalesPerson"),
                        new DataColumn("Position"),
                        new DataColumn("PRCLicense"),
                        new DataColumn("PRCLicenseExpirationDate"),
                        new DataColumn("ATPDateSalesPerson")
                        });
                    DataTable dtSalesPerson = (DataTable)ViewState["SalesPerson"];
                    foreach (DataRow row in dtSalesPerson.Rows)
                    {
                        if (row.RowState != DataRowState.Deleted)
                        {
                            foreach (DataRow row1 in dt.Rows)
                            {
                                if (row1["Id"].ToString() == row["Id"].ToString())
                                {
                                    dt2.Rows.Add(row1["Id"],
                                        row1["SalesPerson"],
                                        row1["Position"],
                                        row1["PRCLicense"],
                                        Convert.ToDateTime(row1["PRCLicenseExpirationDate"]).ToString("yyyy-MM-dd"),
                                        Convert.ToDateTime(row1["ATPDateSalesPerson"]).ToString("yyyy-MM-dd")
                                        );
                                    break;
                                }
                            }
                        }
                    }
                    foreach (DataRow row in dt2.Rows)
                    {
                        var prclicenseexpdate = row["PRCLicenseExpirationDate"].ToString();
                        if (prclicenseexpdate == "1900-01-01" || prclicenseexpdate == "1900/01/01")
                        {
                            row["PRCLicenseExpirationDate"] = " ";
                        }

                        if (row["ATPDateSalesPerson"].ToString() == "1900-01-01" || row["ATPDateSalesPerson"].ToString() == "1900/01/01")
                        {
                            row["ATPDateSalesPerson"] = " ";
                        }
                    }
                    dt2.DefaultView.Sort = "Id ASC";
                    dt2 = dt2.DefaultView.ToTable();
                    gvSalesPersons.DataSource = dt2;
                }
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var prclicenseexpdate = row["PRCLicenseExpirationDate"].ToString();
                        if (prclicenseexpdate == "1900-01-01" || prclicenseexpdate == "1900/01/01")
                        {
                            row["PRCLicenseExpirationDate"] = " ";
                        }

                        if (row["ATPDateSalesPerson"].ToString() == "1900-01-01" || row["ATPDateSalesPerson"].ToString() == "1900/01/01")
                        {
                            row["ATPDateSalesPerson"] = " ";
                        }
                    }
                    dt.DefaultView.Sort = "Id ASC";
                    dt = dt.DefaultView.ToTable();
                    gvSalesPersons.DataSource = dt;
                }
                gvSalesPersons.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "RegisterSalesAgent", "ShowListOfSalesPersons();", true);

                //GAB 07/05/2023 COMMENTED REASON: WENT TO BACKEND
                //var sesh = Session["AddSalesPerson"].ToString();
                //if (Session["AddSalesPerson"].ToString() == "SharingDetails")
                //{
                //    ScriptManager.RegisterStartupScript(this, GetType(), "none", "ComparingSharingDetailsTables();", true);
                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this, GetType(), "none", "ComparingSalesPersonTables();", true);
                //}
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "RegisterSalesAgent", "HideListOfSalesPersons();", true);
                alertMsg(ex.Message, "error");
            }

        }

        protected void gvSalesPersons_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(txtSalesSearch.Value))
            {
                dt = hana.GetData($"CALL sp_OSLASearch ('{txtSalesSearch.Value}','{lblBrokerID.Text}');", hana.GetConnection("SAOHana"));
                // Create a new DataColumn with String datatype and the desired column name
                DataColumn newColumn = new DataColumn("PRCLicenseExpirationDateString", typeof(string));
                DataColumn newColumn2 = new DataColumn("ATPDateSalesPersonString", typeof(string));

                // Add the new column to the DataTable
                dt.Columns.Add(newColumn);
                dt.Columns.Add(newColumn2);

                // Iterate through the rows and convert the DateTime value to string
                foreach (DataRow dr in dt.Rows)
                {
                    // Get the original DateTime value from the row
                    object prcLicenseExpirationDateObj = dr["PRCLicenseExpirationDate"];
                    object atpdatesalespersonObj = dr["ATPDateSalesPerson"];

                    if (prcLicenseExpirationDateObj != DBNull.Value)
                    {
                        DateTime prcLicenseExpirationDate = (DateTime)prcLicenseExpirationDateObj;

                        // Convert the DateTime value to string in the desired format
                        string prcLicenseExpDateStr = prcLicenseExpirationDate == DateTime.Parse("1900-01-01")
                            ? " "
                            : prcLicenseExpirationDate.ToString("yyyy-MM-dd");

                        // Set the converted string value to the new column in the row
                        dr["PRCLicenseExpirationDateString"] = prcLicenseExpDateStr;
                    }
                    else
                    {
                        // Handle DBNull value if needed
                        dr["PRCLicenseExpirationDateString"] = "";
                    }

                    if (atpdatesalespersonObj != DBNull.Value)
                    {
                        DateTime atpdatesalesperson = (DateTime)atpdatesalespersonObj;

                        // Convert the DateTime value to string in the desired format
                        string atpdatesalespersonStr = atpdatesalesperson == DateTime.Parse("1900-01-01")
                            ? " "
                            : atpdatesalesperson.ToString("yyyy-MM-dd");

                        // Set the converted string value to the new column in the row
                        dr["ATPDateSalesPersonString"] = atpdatesalespersonStr;
                    }
                    else
                    {
                        // Handle DBNull value if needed
                        dr["ATPDateSalesPersonString"] = "";
                    }
                }

                // Remove the original DateTime column
                dt.Columns.Remove("PRCLicenseExpirationDate");
                dt.Columns.Remove("ATPDateSalesPerson");

                // Rename the new column to the original column name if desired
                newColumn.ColumnName = "PRCLicenseExpirationDate";
                newColumn2.ColumnName = "ATPDateSalesPerson";
            }
            else
            {
                //string qry = $@"SELECT * FROM ""OSLA"" A WHERE A.""CreateBrokerID"" = '{lblBrokerID.Text}'";
                ////if (id == "btnListOfSalesPerson")
                ////{

                //dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
                string qry = $@"SELECT ""Id"",
                                    ""SalesPerson"",
                                    ""Position"",
                                    ""PRCLicense"",
                                    TO_VARCHAR(TO_DATE(""PRCLicenseExpirationDate""), 'YYYY-MM-DD') ""PRCLicenseExpirationDate"",
                                    TO_VARCHAR(TO_DATE(""ATPDateSalesPerson""), 'YYYY-MM-DD') ""ATPDateSalesPerson""
                                FROM ""OSLA""
                                WHERE ""CreateBrokerID"" = '{lblBrokerID.Text}' ";
                //if (id == "btnListOfSalesPerson")
                //{
                dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
            }

            if (ViewState["SalesorShare"].ToString() == "Share")
            {
                DataTable dt2 = new DataTable();
                dt2.Columns.AddRange(new DataColumn[6]
                    {
                        new DataColumn("Id"),
                        new DataColumn("SalesPerson"),
                        new DataColumn("Position"),
                        new DataColumn("PRCLicense"),
                        new DataColumn("PRCLicenseExpirationDate"),
                        new DataColumn("ATPDateSalesPerson")
                    });
                DataTable dtSalesPerson = (DataTable)ViewState["SalesPerson"];
                foreach (DataRow row in dtSalesPerson.Rows)
                {
                    if (row.RowState != DataRowState.Deleted)
                    {
                        foreach (DataRow row1 in dt.Rows)
                        {
                            if (row1["Id"].ToString() == row["Id"].ToString())
                            {
                                dt2.Rows.Add(row1["Id"],
                                    row1["SalesPerson"],
                                    row1["Position"],
                                    row1["PRCLicense"],
                                    Convert.ToDateTime(row1["PRCLicenseExpirationDate"]).ToString("yyy-MM-dd"),
                                    Convert.ToDateTime(row1["ATPDateSalesPerson"]).ToString("yyy-MM-dd")
                                    );
                                break;
                            }
                        }
                    }
                }
                foreach (DataRow dtrows in dt2.Rows)
                {
                    var prclicenseexpdate = dtrows["PRCLicenseExpirationDate"].ToString();
                    if (prclicenseexpdate == "1900-01-01" || prclicenseexpdate == "1900/01/01")
                    {
                        dtrows["PRCLicenseExpirationDate"] = " ";
                    }

                    if (dtrows["ATPDateSalesPerson"].ToString() == "1900-01-01" || dtrows["ATPDateSalesPerson"].ToString() == "1900/01/01")
                    {
                        dtrows["ATPDateSalesPerson"] = " ";
                    }
                }
                dt2.DefaultView.Sort = "Id ASC";
                dt2 = dt2.DefaultView.ToTable();
                gvSalesPersons.DataSource = dt2;
            }
            else
            {
                foreach (DataRow dtrows in dt.Rows)
                {
                    var prclicenseexpdate = dtrows["PRCLicenseExpirationDate"].ToString();
                    if (prclicenseexpdate == "1900-01-01" || prclicenseexpdate == "1900/01/01")
                    {
                        dtrows["PRCLicenseExpirationDate"] = " ";
                    }

                    if (dtrows["ATPDateSalesPerson"].ToString() == "1900-01-01" || dtrows["ATPDateSalesPerson"].ToString() == "1900/01/01")
                    {
                        dtrows["ATPDateSalesPerson"] = " ";
                    }
                }
                dt.DefaultView.Sort = "Id ASC";
                dt = dt.DefaultView.ToTable();
                gvSalesPersons.DataSource = dt;
            }
            gvSalesPersons.PageIndex = e.NewPageIndex;
            gvSalesPersons.DataBind();
            //GAB 07/05/2023 COMMENTED REASON: WENT TO BACKEND
            //if (Session["AddSalesPerson"].ToString() == "SharingDetails")
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "none", "ComparingSharingDetailsTables();", true);
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "none", "ComparingSalesPersonTables();", true);
            //}
        }

        protected void bSelectsalesPersons_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            DataTable dt = hana.GetData($@"SELECT ""Id"",""SalesPerson"",""Position"" FROM ""OSLA"" WHERE ""Id"" = '{GetID.CommandArgument}'", hana.GetConnection("SAOHana"));
            string name = DataAccess.GetData(dt, 0, "SalesPerson", "").ToString();
            string position = DataAccess.GetData(dt, 0, "Position", "").ToString();
            string salesPersonID = DataAccess.GetData(dt, 0, "Id", "").ToString();

            //GAB 07/05/2023 Backend Table Comparison for SalesPersonTables
            if (Session["AddSalesPerson"].ToString() == "SharingDetails")
            {
                //INSERT COMPARING SHARING DETAILS TABLE TO SALESPERSONS TABLE
                bool matchFound = false;
                //insert gvShareDetails data source here
                string sharingId = ViewState["SharingID"].ToString();

                bool type = Convert.ToBoolean(ViewState["isLot"]);
                string commType = type != true ? "LotOnlyPercentage" : "HouseandLotPercentage";

                DataTable sharingDetailsTable = (DataTable)ViewState["SharingDetails"];
                DataRow[] filteredRows = sharingDetailsTable.Select($"{commType} = '0' AND SalesPersonID = '{sharingId}'");
                DataTable filteredDataTable = sharingDetailsTable.Clone();

                foreach (DataRow row in filteredRows)
                {
                    filteredDataTable.ImportRow(row);
                }

                foreach (DataRow dr in filteredDataTable.Rows)
                {
                    string sharingDetailsTableId = dr["OslaID"].ToString();
                    if (salesPersonID == sharingDetailsTableId)
                    {
                        matchFound = true;
                        break;
                    }
                }

                if (matchFound)
                {
                    alertMsg("Sales Person is already selected.", "info");
                }
                else
                {
                    //INSERT CODE HERE WHAT TO DO IF NO MATCH IS FOUND
                    if (ViewState["TriggerButton"].ToString() == "btnListOfSalesPersonSharingDetails")
                    {
                        mtxtSalesPersonShareID.Text = GetID.CommandArgument;
                        mtxtSalesPersonShare.Text = name;
                        mtxtSalesPersonPosition.Text = position;
                        mddPositionShare.SelectedValue = position;
                    }
                    else
                    {
                        mtxtSalesPersonID.Text = GetID.CommandArgument;
                        mtxtSalesPersonPosition.Text = position;
                        mtxtSalesPerson.Text = name;
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideListOfSalesPersons();", true);
                }
            }
            else
            {
                //INSERT COMPARING SALESPERSON TABLE TO SALESPERONS TABLE HERE
                bool matchFound = false;
                DataTable salesPersonTable = (DataTable)ViewState["SalesPerson"];
                foreach (DataRow dr in salesPersonTable.Rows)
                {
                    string salesPersonTableId = dr["Id"].ToString();
                    if (salesPersonID == salesPersonTableId)
                    {
                        matchFound = true;
                        break;
                    }
                }

                if (matchFound)
                {
                    alertMsg("Sales Person is already selected.", "info");
                }
                else
                {
                    //INSERT CODE HERE WHAT TO DO IF NO MATCH IS FOUND
                    if (ViewState["TriggerButton"].ToString() == "btnListOfSalesPersonSharingDetails")
                    {
                        mtxtSalesPersonShareID.Text = GetID.CommandArgument;
                        mtxtSalesPersonShare.Text = name;
                        mtxtSalesPersonPosition.Text = position;
                        mddPositionShare.SelectedValue = position;
                    }
                    else
                    {
                        mtxtSalesPersonID.Text = GetID.CommandArgument;
                        mtxtSalesPersonPosition.Text = position;
                        mtxtSalesPerson.Text = name;
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideListOfSalesPersons();", true);
                }
            }
        }

        protected void btnCreateusers_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(lblBrokerID.Text))
                {

                    string expdate = DateTime.MinValue.ToString("yyyy-MM-dd");
                    //GET PASSPORT EXPIRATION DATE
                    if (ddValidID.SelectedValue == "ID4")
                    {
                        expdate = txtIDExpirationDate.Text;
                    }
                    else if (ddValidID2.SelectedValue == "ID4")
                    {
                        expdate = txtIDExpirationDate2.Text;
                    }
                    if (ddValidID3.SelectedValue == "ID4")
                    {
                        expdate = txtIDExpirationDate3.Text;
                    }
                    if (ddValidID4.SelectedValue == "ID4")
                    {
                        expdate = txtIDExpirationDate4.Text;
                    }

                    //CREATE BUSINESS PARTNER IN SAP
                    SapHanaLayer company = new SapHanaLayer();
                    string module = "";
                    string brkcredlistbrk = $@"";
                    string brkcredlistsa = $@"";
                    string brkcredlist = $@"<table align=""center"" style=""border: 1px solid #91714a; border-collapse: collapse;"">
                                    <tr style=""font-weight:bold; border: 1px solid #91714a;""> 	
                                    <th style=""font-weight:bold; border: 1px solid #91714a;"">Sales Agent Name</th> 
                                    <th style=""font-weight:bold; border: 1px solid #91714a;"">Email</th> 
                                    <th style=""font-weight:bold; border: 1px solid #91714a;"">Username</th> 
                                    <th style=""font-weight:bold; border: 1px solid #91714a;"">Password</th> 
                                    </tr>";
                    bool hassalesagent = false;
                    bool result = company.Connect();
                    string Message = $"({company.ResultCode}) {company.ResultDescription}";

                    //IF SUCCESSFULLY CONNECTED TO SAP
                    if (result)
                    {
                        //LOOP SALES AGENT
                        DataTable dtSalesPerson = (DataTable)ViewState["SalesPerson"];


                        //GET NEEDED ACCOUNTS FOR BP
                        DataTable dtOCRG = hana.GetData($@"select ""U_ControlAcctCode"", ""U_DPClrAcctCode"" from OCRG where ""GroupCode"" = {ConfigSettings.VendorGroupCode}", hana.GetConnection("SAPHana"));
                        string ControlAccount = DataAccess.GetData(dtOCRG, 0, "U_ControlAcctCode", "").ToString();
                        string ClearingAccount = DataAccess.GetData(dtOCRG, 0, "U_DPClrAcctCode", "").ToString();
                        string InternalCode = ViewState["InternalCode"].ToString();

                        foreach (DataRow row in dtSalesPerson.Rows)
                        {
                            row["PRCLicenseExpirationDate"] = string.IsNullOrWhiteSpace(row["PRCLicenseExpirationDate"].ToString()) ? "1900-01-01" : row["PRCLicenseExpirationDate"];
                            row["ATPDate"] = string.IsNullOrWhiteSpace(row["ATPDate"].ToString()) ? "1900-01-01" : row["ATPDate"];








                            brkcredlist = brkcredlist.Replace("</table>", "");
                            if (row.RowState != DataRowState.Deleted)
                            {
                                string test = row["Id"].ToString().Trim();
                                string testbrkid = ViewState["BrokerID"].ToString();

                                string CardCode = null;
                                module = "BusinessPartners";
                                int series = int.Parse(ConfigSettings.BrokerBPSeries);


                                DataTable dt = hana.GetData($@"SELECT TOP 1 ""CardCode"",""CardName"",""U_SalesAgentCode"" FROM ""OCRD"" Where 
                                ""U_BrokerCode"" = '{ ViewState["BrokerID"].ToString()}' AND ""U_SalesAgentCode"" = '{row["Id"].ToString().Trim()}'", hana.GetConnection("SAPHana"));
                                DataTable dt2 = hana.GetData($@"select
                                                                ""LineNum""
                                                                 ,(select ""CntctCode"" from ""OCPR"" where ""CardCode"" = '{DataAccess.GetData(dt, 0, "CardCode", "").ToString().Trim()}')as ""InternalCode""
                                                             from ""CRD1"" where ""CardCode"" = '{DataAccess.GetData(dt, 0, "CardCode", "").ToString().Trim()}' and ""AdresType"" = 'S'", hana.GetConnection("SAPHana"));
                                //DataTable dt2 = hana.GetData($@"select ""LineNum"" from ""CRD1"" where ""CardCode"" = '{ViewState["CardCode"].ToString()}' and ""AdresType"" = 'S'", hana.GetConnection("SAPHana"));
                                DataTable dtBrokerDetails = hana.GetData($@"SELECT * FROM OBRK WHERE ""BrokerId"" = '{ViewState["BrokerID"]}'", hana.GetConnection("SAOHana"));



                                //UPDATE BUSINESS PARTNER INFORMATION -- VENDOR
                                if (result = DataAccess.Exist(dt))
                                {
                                    //module = $@"BusinessPartners('{ViewState["CardCode"].ToString()}')";
                                    module = $@"BusinessPartners('{DataAccess.GetData(dt, 0, "CardCode", "").ToString().Trim()}')";
                                    //Update
                                    IDictionary<string, object> BusinessPartners = new Dictionary<string, object>()
                                    {
                                        { "Series" , series },
                                        { "CardType" , "S" },
                                        { "CardName" ,  row["SalesPersonName"].ToString().ToUpper().Trim()},
                                        { "CardForeignName" ,  row["SalesPersonName"].ToString().ToUpper().Trim()},
                                        { "GroupCode" , ConfigSettings.BrokerBPGroup },

                                        { "Currency" , ConfigSettings.Currency },
                                        { "FederalTaxID" , row["TIN"].ToString().Trim()},
                                        { "Phone1" , txtBusinessPhoneNo.Text.Trim()},
                                        { "Cellular" , row["MobileNumber"].ToString().Trim()},
                                        { "Fax" , txtFaxNo.Text.Trim()},

                                        { "EmailAddress" , row["EmailAddress"].ToString().Trim()},

                                        { "DebitorAccount" , ControlAccount},
                                        { "DownPaymentClearAct" , ClearingAccount },
                                        { "DownPaymentInterimAccount" , ClearingAccount },

                                        //VAT GROUP
                                        { "VatLiable" , "vLiable" },
                                        { "VatGroup" ,  row["VATCode"].ToString().ToUpper().Trim()},

                                        //WITHHOLDING TAX GROUP
                                        { "SubjectToWithholdingTax" , "Y" },
                                        { "WTCode" ,  row["WTaxCode"].ToString().ToUpper().Trim()},

                                        { "PriceListNum" , int.Parse(ConfigSettings.BPPriceList) },

                                        { "DeferredTax" , "N" },
                                        { "PayTermsGrpCode" ,  ConfigSettings.DefaultPaymentTermForVendor},

                                        { "U_BrokerCode" , ViewState["BrokerID"].ToString()},
                                        { "U_Position" ,  row["Position"].ToString().Trim()},
                                        { "U_Broker_ATPExpDate" , Convert.ToDateTime(txtATPExpiryDate.Text).ToString("yyyy-MM-dd")},
                                        { "U_Broker_PRCExpDate" , Convert.ToDateTime(txtPRCLicenseExpirationDate.Text).ToString("yyyy-MM-dd")},
                                        { "U_PassportExpDate" , expdate},
                                        { "U_PTRValidUntil" , Convert.ToDateTime(txtPTRValidTo.Text).ToString("yyyy-MM-dd")},
                                        { "U_SA_PRCExpDate" , Convert.ToDateTime(row["PRCLicenseExpirationDate"].ToString().Trim()).ToString("yyyy-MM-dd")},
                                        { "U_SA_ATPExpDate" , Convert.ToDateTime(row["ATPDate"].ToString().Trim()).ToString("yyyy-MM-dd")},

                                        { "U_ValidID1" ,  DataAccess.GetData(dtBrokerDetails, 0, "ValidID", "").ToString().Trim() },
                                        { "U_ValidID2" ,  DataAccess.GetData(dtBrokerDetails, 0, "ValidID2", "").ToString().Trim()},
                                        { "U_ValidID3" ,  DataAccess.GetData(dtBrokerDetails, 0, "ValidID3", "").ToString().Trim()},
                                        { "U_ValidID4" ,  DataAccess.GetData(dtBrokerDetails, 0, "ValidID4", "").ToString().Trim()},

                                        { "U_ID1RefNo" ,  DataAccess.GetData(dtBrokerDetails, 0, "IDNo", "").ToString().Trim()},
                                        { "U_ID2RefNo" ,  DataAccess.GetData(dtBrokerDetails, 0, "IDNo2", "").ToString().Trim()},
                                        { "U_ID3RefNo" ,  DataAccess.GetData(dtBrokerDetails, 0, "IDNo3", "").ToString().Trim()},
                                        { "U_ID4RefNo" ,  DataAccess.GetData(dtBrokerDetails, 0, "IDNo4", "").ToString().Trim()},

                                        { "U_SalesAgentCode" ,  row["Id"].ToString().Trim()},

                                        { "BilltoDefault" ,  DataAccess.GetData(dtBrokerDetails, 0, "City", "").ToString().Trim()},

                                    };


                                    //CONTACT PERSON TAB IN SAP BUSINESS PARTNER MASTER DATA
                                    IList<IDictionary<string, object>> ContactEmployees = new List<IDictionary<string, object>>();
                                    var ContactEmp = new Dictionary<string, object>();

                                    ContactEmp.Add("Name", row["SalesPersonName"].ToString().ToUpper().Trim());
                                    ContactEmp.Add("FirstName", DataAccess.GetData(dtBrokerDetails, 0, "FirstName", "").ToString().Trim());
                                    ContactEmp.Add("MiddleName", DataAccess.GetData(dtBrokerDetails, 0, "MiddleName", "").ToString().Trim());
                                    ContactEmp.Add("LastName", DataAccess.GetData(dtBrokerDetails, 0, "LastName", "").ToString().Trim());
                                    ContactEmp.Add("Position", row["Position"].ToString().Trim());
                                    ContactEmp.Add("E_Mail", row["EmailAddress"].ToString().Trim());
                                    ContactEmp.Add("MobilePhone", row["MobileNumber"].ToString().Trim());
                                    ContactEmp.Add("Address", DataAccess.GetData(dtBrokerDetails, 0, "Address", "").ToString().Trim());
                                    ContactEmp.Add("Pager", DataAccess.GetData(dtBrokerDetails, 0, "ValidID", "").ToString().Trim());
                                    ContactEmp.Add("Remarks1", DataAccess.GetData(dtBrokerDetails, 0, "IDNo", "").ToString().Trim());
                                    ContactEmp.Add("InternalCode", DataAccess.GetData(dt2, 0, "InternalCode", "").ToString().Trim());
                                    ContactEmp.Add("CardCode", DataAccess.GetData(dt, 0, "CardCode", "").ToString().Trim());
                                    //ContactEmp.Add("CardCode", ViewState["CardCode"].ToString());

                                    ContactEmployees.Add(ContactEmp);
                                    //ADDRESS TAB IN SAP BUSINESS PARTNER MASTER DATA
                                    IList<IDictionary<string, object>> BPAddresses = new List<IDictionary<string, object>>();
                                    var Address = new Dictionary<string, object>();

                                    Address.Add("AddressName", DataAccess.GetData(dtBrokerDetails, 0, "City", "").ToString().Trim());
                                    Address.Add("BuildingFloorRoom", DataAccess.GetData(dtBrokerDetails, 0, "Address", "").ToString().Trim());
                                    Address.Add("City", DataAccess.GetData(dtBrokerDetails, 0, "City", "").ToString().Trim());
                                    Address.Add("ZipCode", DataAccess.GetData(dtBrokerDetails, 0, "BusinessZipCode", "").ToString().Trim());
                                    Address.Add("AddressType", "bo_ShipTo");
                                    //Address.Add("RowNum", "1"); 
                                    Address.Add("RowNum", DataAccess.GetData(dt2, 0, "LineNum", "").ToString().Trim());
                                    Address.Add("BPCode", DataAccess.GetData(dt, 0, "CardCode", "").ToString().Trim());
                                    //Address.Add("BPCode", ViewState["CardCode"].ToString());


                                    BPAddresses.Add(Address);


                                    StringBuilder ContactEmployeesLine = DataHelper.JsonLinesBuilder("ContactEmployees", ContactEmployees);
                                    StringBuilder BPAddressesLine = DataHelper.JsonLinesBuilder("BPAddresses", BPAddresses);
                                    StringBuilder strLines = DataHelper.JsonLinesCombine(ContactEmployeesLine, BPAddressesLine);
                                    var json = DataHelper.JsonBuilder(BusinessPartners, strLines);

                                    //var json = DataHelper.JsonBuilder(BusinessPartners, new StringBuilder());
                                    if (company.PATCH(module, json))
                                    {
                                        Message = "Business Partner successfully updated.";
                                        CardCode = dt.Rows[0][0].ToString();

                                        hana.Execute($@"UPDATE BRK1 SET ""SAPCardCode"" = '{DataAccess.GetData(dt, 0, "CardCode", "").ToString().Trim()}' WHERE ""BrokerId"" = '{ViewState["BrokerID"].ToString()}' AND ""Id"" = '{row["Id"].ToString().Trim()}'", hana.GetConnection("SAOHana"));
                                        if (row["Position"].ToString().Trim() == "Broker")
                                        {
                                            //CHECK IF BROKER ID AND AGENT CODE EXISTS IN BP MASTER CODE VENDOR
                                            string qryGetVendor = $@"SELECT * FROM OBRK WHERE ""Tax"" = '{row["TIN"].ToString().Trim()}'";
                                            DataTable dtGetVendor = hana.GetData(qryGetVendor, hana.GetConnection("SAOHana"));
                                            if (dtGetVendor.Rows.Count > 0)
                                            {
                                                hana.Execute($@"UPDATE OBRK SET ""SAPCardCode"" = '{DataAccess.GetData(dt, 0, "CardCode", "").ToString().Trim()}' WHERE ""BrokerId"" = '{ViewState["BrokerID"].ToString()}'", hana.GetConnection("SAOHana"));
                                            }
                                        }

                                    }
                                    else
                                    {
                                        Message = $"({company.ResultCode}) {company.ResultDescription}";
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ModalApprovalType_Hide();", true);
                                        alertMsg($"Error updating Business Partner. ({Message})", "error");
                                        return;
                                    }
                                }


                                //POST TO SAP NEW BUSINESS PARTNER -- VENDOR
                                else
                                {
                                    IDictionary<string, object> BusinessPartners = new Dictionary<string, object>()
                                    {
                                        { "Series" , series },
                                        { "CardType" , "S" },
                                        { "CardName" ,  row["SalesPersonName"].ToString().ToUpper().Trim()},
                                        { "CardForeignName" ,  row["SalesPersonName"].ToString().ToUpper().Trim()},
                                        { "GroupCode" , ConfigSettings.BrokerBPGroup },

                                        { "Currency" , ConfigSettings.Currency},
                                        { "FederalTaxID" , row["TIN"].ToString().Trim()},
                                        { "Phone1" , txtBusinessPhoneNo.Text.Trim()},
                                        { "Cellular" , row["MobileNumber"].ToString().Trim()},
                                        { "Fax" , txtFaxNo.Text.Trim()},

                                        { "EmailAddress" , row["EmailAddress"].ToString().Trim()},

                                        { "DebitorAccount" , ControlAccount},
                                        { "DownPaymentClearAct" , ClearingAccount },
                                        { "DownPaymentInterimAccount" , ClearingAccount },
                                        { "VatLiable" , "vLiable" },
                                        { "SubjectToWithholdingTax" , "Y" },

                                        { "VatGroup" ,  row["VATCode"].ToString().ToUpper().Trim()},
                                        { "WTCode" ,  row["WTaxCode"].ToString().ToUpper().Trim()},
                                        { "PriceListNum" , int.Parse(ConfigSettings.BPPriceList) },

                                        { "DeferredTax" , "N" },
                                        { "PayTermsGrpCode" ,  ConfigSettings.DefaultPaymentTermForVendor},

                                        { "U_BrokerCode" , ViewState["BrokerID"].ToString()},
                                        { "U_Position" ,  row["Position"].ToString().Trim()},
                                        { "U_Broker_ATPExpDate" , Convert.ToDateTime(txtATPExpiryDate.Text).ToString("yyyy-MM-dd")},
                                        { "U_Broker_PRCExpDate" , Convert.ToDateTime(txtPRCLicenseExpirationDate.Text).ToString("yyyy-MM-dd")},
                                        { "U_PassportExpDate" , expdate},
                                        { "U_PTRValidUntil" , Convert.ToDateTime(txtPTRValidTo.Text).ToString("yyyy-MM-dd")},
                                        { "U_SA_PRCExpDate" , Convert.ToDateTime(row["PRCLicenseExpirationDate"].ToString().Trim()).ToString("yyyy-MM-dd")},
                                        { "U_SA_ATPExpDate" , Convert.ToDateTime(row["ATPDate"].ToString().Trim()).ToString("yyyy-MM-dd")},

                                        { "U_ValidID1" ,  DataAccess.GetData(dtBrokerDetails, 0, "ValidID", "").ToString().Trim() },
                                        { "U_ValidID2" ,  DataAccess.GetData(dtBrokerDetails, 0, "ValidID2", "").ToString().Trim()},
                                        { "U_ValidID3" ,  DataAccess.GetData(dtBrokerDetails, 0, "ValidID3", "").ToString().Trim()},
                                        { "U_ValidID4" ,  DataAccess.GetData(dtBrokerDetails, 0, "ValidID4", "").ToString().Trim()},

                                        { "U_ID1RefNo" ,  DataAccess.GetData(dtBrokerDetails, 0, "IDNo", "").ToString().Trim()},
                                        { "U_ID2RefNo" ,  DataAccess.GetData(dtBrokerDetails, 0, "IDNo2", "").ToString().Trim()},
                                        { "U_ID3RefNo" ,  DataAccess.GetData(dtBrokerDetails, 0, "IDNo3", "").ToString().Trim()},
                                        { "U_ID4RefNo" ,  DataAccess.GetData(dtBrokerDetails, 0, "IDNo4", "").ToString().Trim()},

                                        { "U_SalesAgentCode" ,  row["Id"].ToString().Trim()},

                                        { "BilltoDefault" ,  DataAccess.GetData(dtBrokerDetails, 0, "City", "").ToString().Trim()},


                                    };

                                    //var json = DataHelper.JsonBuilder(BusinessPartners, new StringBuilder());
                                    //CONTACT PERSON TAB IN SAP BUSINESS PARTNER MASTER DATA
                                    IList<IDictionary<string, object>> ContactEmployees = new List<IDictionary<string, object>>();
                                    var ContactEmp = new Dictionary<string, object>();

                                    ContactEmp.Add("Name", row["SalesPersonName"].ToString().ToUpper().Trim());
                                    ContactEmp.Add("FirstName", DataAccess.GetData(dtBrokerDetails, 0, "FirstName", "").ToString().Trim());
                                    ContactEmp.Add("MiddleName", DataAccess.GetData(dtBrokerDetails, 0, "MiddleName", "").ToString().Trim());
                                    ContactEmp.Add("LastName", DataAccess.GetData(dtBrokerDetails, 0, "LastName", "").ToString().Trim());
                                    ContactEmp.Add("Position", row["Position"].ToString().Trim());
                                    ContactEmp.Add("E_Mail", row["EmailAddress"].ToString().Trim());
                                    ContactEmp.Add("MobilePhone", row["MobileNumber"].ToString().Trim());
                                    ContactEmp.Add("Address", DataAccess.GetData(dtBrokerDetails, 0, "Address", "").ToString().Trim());
                                    ContactEmp.Add("Pager", DataAccess.GetData(dtBrokerDetails, 0, "ValidID", "").ToString().Trim());
                                    ContactEmp.Add("Remarks1", DataAccess.GetData(dtBrokerDetails, 0, "IDNo", "").ToString().Trim());

                                    ContactEmployees.Add(ContactEmp);
                                    //ADDRESS TAB IN SAP BUSINESS PARTNER MASTER DATA
                                    IList<IDictionary<string, object>> BPAddresses = new List<IDictionary<string, object>>();
                                    var Address = new Dictionary<string, object>();

                                    Address.Add("AddressName", DataAccess.GetData(dtBrokerDetails, 0, "City", "").ToString().Trim());
                                    Address.Add("BuildingFloorRoom", DataAccess.GetData(dtBrokerDetails, 0, "Address", "").ToString().Trim());
                                    Address.Add("City", DataAccess.GetData(dtBrokerDetails, 0, "City", "").ToString().Trim());
                                    Address.Add("ZipCode", DataAccess.GetData(dtBrokerDetails, 0, "BusinessZipCode", "").ToString().Trim());

                                    BPAddresses.Add(Address);


                                    StringBuilder ContactEmployeesLine = DataHelper.JsonLinesBuilder("ContactEmployees", ContactEmployees);
                                    StringBuilder BPAddressesLine = DataHelper.JsonLinesBuilder("BPAddresses", BPAddresses);
                                    StringBuilder strLines = DataHelper.JsonLinesCombine(ContactEmployeesLine, BPAddressesLine);
                                    var json = DataHelper.JsonBuilder(BusinessPartners, strLines);

                                    if (company.POST(module, json))
                                    {
                                        Message = "Business Partner successfully created.";

                                        hana.Execute($@"UPDATE BRK1 SET ""SAPCardCode"" = '{company.ResultDescription}' WHERE ""BrokerId"" = '{ViewState["BrokerID"].ToString()}' AND ""Id"" = '{row["Id"].ToString().Trim()}'", hana.GetConnection("SAOHana"));
                                        if (row["Position"].ToString().Trim() == "Broker")
                                        {
                                            //CHECK IF BROKER ID AND AGENT CODE EXISTS IN BP MASTER CODE VENDOR
                                            string qryGetVendor = $@"SELECT * FROM OBRK WHERE ""Tax"" = '{row["TIN"].ToString().Trim()}'";
                                            DataTable dtGetVendor = hana.GetData(qryGetVendor, hana.GetConnection("SAOHana"));
                                            if (dtGetVendor.Rows.Count > 0)
                                            {
                                                hana.Execute($@"UPDATE OBRK SET ""SAPCardCode"" = '{DataAccess.GetData(dt, 0, "CardCode", "").ToString().Trim()}' 
                                                WHERE ""BrokerId"" = '{ViewState["BrokerID"].ToString()}'", hana.GetConnection("SAOHana"));
                                            }
                                        }
                                        //CREATE USER FOR DREAMS
                                        try
                                        {
                                            string a = "";
                                            int ID = Convert.ToInt32(DataAccess.GetData(hana.GetData(@"SELECT ""AutoKey"" FROM ""AKEY"" WHERE ""ObjectCode"" = 4;", hana.GetConnection("SAOHana")), 0, "AutoKey", "0").ToString());
                                            string[] name = row["SalesPersonName"].ToString().Trim().Split(' ');
                                            string fName = "", mName = "", lName = "";
                                            if (name.Count() == 1)
                                            {
                                                fName = name[0];
                                            }
                                            else if (name.Count() == 2)
                                            {
                                                fName = name[0];
                                                mName = "";
                                                lName = name[1];
                                            }
                                            else
                                            {
                                                fName = name[0];
                                                mName = name[1];
                                                lName = name[2];
                                            }
                                            ws.SetOUSR(lName.ToUpper().Trim(), fName.ToUpper().Trim(), mName.ToUpper().Trim(), row["EmailAddress"].ToString().Trim(), company.ResultDescription, Cryption.Encrypt(company.ResultDescription + "1234"), out a);
                                            if (ID != 0)
                                            {
                                                if (ws.DeleteRole(ID) == true || ws.DeleteMode(ID) == true)
                                                {
                                                    if (ws.SetRole(ID, Cryption.Encrypt($"{ID}SALES"), (int)Session["UserID"]) == true)
                                                    {
                                                        //ws.SetMode(ID, Cryption.Encrypt($"{ID}OPRJ"), 1);
                                                        //ws.SetMode(ID, Cryption.Encrypt($"{ID}OCRD"), 1);
                                                        ws.SetMode(ID, Cryption.Encrypt($"{ID}OQUT"), 1);
                                                        //ws.SetMode(ID, Cryption.Encrypt($"{ID}OQUS"), 1);
                                                        //ws.SetMode(ID, Cryption.Encrypt($"{ID}ODCR"), 1);
                                                    }
                                                }

                                                //Email notification to Broker Creation of User
                                                if (row["Position"].ToString().Trim() != "Broker")
                                                {
                                                    brkcredlistsa += $@"<tr style=""font-weight:bold; border: 1px solid #91714a;""> 	
                                                                <td style=""font-weight:bold; border: 1px solid #91714a;"">{row["SalesPersonName"].ToString().Trim()}</td>     
                                                                <td style=""font-weight:bold; border: 1px solid #91714a;"">{row["EmailAddress"].ToString().Trim()}</td>     
                                                                <td style=""font-weight:bold; border: 1px solid #91714a;"">{company.ResultDescription}</td>     
                                                                <td style=""font-weight:bold; border: 1px solid #91714a;"">1234</td>     
                                                                </tr> ";
                                                    hassalesagent = true;
                                                    Session["Brkcreds"] = $@"Username: {company.ResultDescription} <br> Password: 1234";
                                                    string emailaddress = row["EmailAddress"].ToString().Trim();
                                                    string qry1 = $@"SELECT * FROM ""@EMAIL"" WHERE ""Code"" = 'SACRDS'";
                                                    dt = hana.GetData(qry1, hana.GetConnection("SAPHana"));
                                                    ws.sendEmail(lblBrokerID.Text,
                                                                            DataAccess.GetData(dt, 0, "U_Subject", "").ToString(),
                                                                            emailaddress,
                                                                            DataAccess.GetData(dt, 0, "U_Body", "").ToString(),
                                                                            "",
                                                                            "",
                                                                            "Broker ID",
                                                                            DataAccess.GetData(dt, 0, "Code", "").ToString());
                                                }
                                                else
                                                {
                                                    brkcredlistbrk += $@"<tr style=""font-weight:bold; border: 1px solid #91714a;""> 	
                                                                <td style=""font-weight:bold; border: 1px solid #91714a;"">{row["SalesPersonName"].ToString().Trim()}</td>     
                                                                <td style=""font-weight:bold; border: 1px solid #91714a;"">{row["EmailAddress"].ToString().Trim()}</td>     
                                                                <td style=""font-weight:bold; border: 1px solid #91714a;"">{company.ResultDescription}</td>     
                                                                <td style=""font-weight:bold; border: 1px solid #91714a;"">1234</td>     
                                                                </tr> ";
                                                }

                                            }

                                            Message = $"Business Partner Master Data and DREAMS users are successfully created.";
                                            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "ShowSuccessAlert();", true);
                                            //alertMsg($"Business Partner Master Data and DREAMS users are successfully created.", "success");

                                        }
                                        catch (Exception ex) { alertMsg(ex.Message, "error"); }



                                    }
                                    else
                                    {
                                        Message = $"({company.ResultCode}) {company.ResultDescription}";
                                        alertMsg($"Error posting Business Partner. ({Message})", "error");
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ModalApprovalType_Hide();", true);
                                        return;
                                    }

                                }
                            }
                        }















                        if (Message.Contains("created") && Message.Contains("successfully"))
                        {
                            brkcredlist += $@"{brkcredlistbrk}{brkcredlistsa}</table> ";
                            Session["Brkcreds"] = brkcredlist;
                            string qry = $@"SELECT * FROM ""@EMAIL"" WHERE ""Code"" = 'BRKCRDS'";
                            DataTable dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
                            ws.sendEmail(lblBrokerID.Text,
                                                    DataAccess.GetData(dt, 0, "U_Subject", "").ToString(),
                                                    ViewState["businesstype"].ToString() == "SOLE PROPRIETOR" ? txtEmailAddress.Text.Trim() : txtContactEmail.Text.Trim(),
                                                    DataAccess.GetData(dt, 0, "U_Body", "").ToString(),
                                                    "",
                                                    "",
                                                    "Broker ID",
                                                    DataAccess.GetData(dt, 0, "Code", "").ToString());
                        }
                        mtxtUniqueId.Text = Message;
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "ShowSuccessAlert();", true);
                        //END LOOP SALES AGENT

                        //LOOP SHARING DETAILS
                        //DataTable dtSharingDetails = (DataTable)ViewState["SharingDetails"];
                        //foreach (DataRow row in dtSharingDetails.Rows)
                        //{
                        //    if (row.RowState != DataRowState.Deleted)
                        //    {
                        //        string test = row["Id"].ToString().Trim();
                        //        string test2 = row["OslaID"].ToString().Trim();

                        //        DataTable salesperson = dtSalesPerson.Select($"Id = '{row["SalesPersonId"].ToString().Trim()}'").CopyToDataTable();

                        //        string CardCode = null;
                        //        module = "BusinessPartners";
                        //        int series = int.Parse(ConfigSettings.BrokerBPSeries"].ToString());

                        //        DataTable dt = hana.GetData($@"SELECT TOP 1 ""CardCode"",""CardName"",""CardFName"" FROM ""OCRD"" Where ""U_BrokerCode"" = '{row["SalesPersonId"].ToString().Trim()}' AND ""CardFName"" = '{row["OslaID"].ToString().Trim()}'", hana.GetConnection("SAPHana"));

                        //        if (result = DataAccess.Exist(dt))
                        //        {
                        //            //return existing cardcode
                        //            Message = "Business Partner successfully created.";
                        //            CardCode = dt.Rows[0][0].ToString();
                        //        }
                        //        else
                        //        {
                        //            IDictionary<string, object> BusinessPartners = new Dictionary<string, object>()
                        //                {
                        //                    { "Series" , series },
                        //                    { "CardType" , "S" },
                        //                    { "GroupCode" , ConfigSettings.BrokerBPGroup"].ToString() },
                        //                    { "CardName" ,  row["SalesPersonName"].ToString().ToUpper().Trim()},
                        //                    { "VatGroup" ,  salesperson.Rows[8].ToString()},
                        //                    { "WTCode" ,  salesperson.Rows[9].ToString()},
                        //                    { "CardForeignName" ,  row["OslaID"].ToString().Trim()},
                        //                    { "PriceListNum" , int.Parse(ConfigSettings.BPPriceList"].ToString()) },
                        //                    { "DownPaymentClearAct" , ConfigSettings.CashAccount"].ToString() },
                        //                    { "DownPaymentInterimAccount" , ConfigSettings.CashAccount"].ToString() },
                        //                    { "SubjectToWithholdingTax" , "N" },
                        //                    { "DeferredTax" , "N" },
                        //                    { "U_Position" ,  row["Position"].ToString().Trim()},
                        //                    { "U_BrokerCode" , row["SalesPersonId"].ToString().Trim()},
                        //                    { "FederalTaxID" , salesperson.Rows[7].ToString()},
                        //                    { "U_SA_ATPExpDate" , Convert.ToDateTime(row["ATPDate"].ToString().Trim()).ToString("yyyy-MM-dd")},
                        //                    { "U_SA_PRCExpDate" , Convert.ToDateTime(row["PRCLicenseExpirationDate"].ToString().Trim()).ToString("yyyy-MM-dd")},
                        //                    { "U_PTRValidUntil" , Convert.ToDateTime(txtPTRValidTo.Text).ToString("yyyy-MM-dd")},
                        //                    { "U_PassportExpDate" , Convert.ToDateTime(txtPassportValidTo.Text).ToString("yyyy-MM-dd")},
                        //                    { "U_Broker_PRCExpDate" , Convert.ToDateTime(txtPRCLicenseExpirationDate.Text).ToString("yyyy-MM-dd")},
                        //                    { "U_Broker_ATPExpDate" , Convert.ToDateTime(txtATPExpiryDate.Text).ToString("yyyy-MM-dd")}
                        //                };

                        //            var json = DataHelper.JsonBuilder(BusinessPartners, new StringBuilder());

                        //            if (company.POST(module, json))
                        //            {
                        //                Message = "Business Partner successfully created.";

                        //                hana.Execute($@"UPDATE BRK1 SET ""SAPCardCode"" = '{company.ResultDescription}' WHERE ""BrokerId"" = '{ViewState["BrokerID"].ToString()}' AND ""Id"" = '{row["Id"].ToString().Trim()}'", hana.GetConnection("SAOHana"));
                        //                if (row["Position"].ToString().Trim() == "Broker")
                        //                {
                        //                    hana.Execute($@"UPDATE OBRK SET ""SAPCardCode"" = '{company.ResultDescription}' WHERE ""BrokerId"" = '{ViewState["BrokerID"].ToString()}'", hana.GetConnection("SAOHana"));
                        //                }

                        //            }
                        //            else
                        //            {
                        //                Message = $"({company.ResultCode}) {company.ResultDescription}";
                        //                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ModalApprovalType_Hide();", true);
                        //                alertMsg($"Error posting Business Partner. ({Message})", "error");
                        //            }

                        //        }
                        //    }
                        //}
                        //END LOOP SHARING DETAILS

                    }
                    else
                    {
                        alertMsg("DREAMS is not able to connect to SAP database. Please try again. If error persists, please contact administrator.", "info");
                    }
                }
                else
                {
                    alertMsg("No broker selected. Please reload the page.", "info");
                }
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }
        }

        protected void btnPrintApproved_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(lblBrokerID.Text))
                {

                    HtmlButton clickedButton = (HtmlButton)sender;
                    string buttonId = clickedButton.ID;

                    //REPORT
                    string qry = $@"SELECT ""Id"" from OBRK WHERE ""BrokerId"" = '{ViewState["BrokerID"].ToString()}'";
                    DataTable dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
                    Session["PrintDocEntry"] = DataAccess.GetData(dt, 0, "Id", "0");
                    Session["ReportPath"] = ConfigurationManager.AppSettings["ReportPathForms"].ToString();
                    Session["ReportType"] = "";

                    if (buttonId == "btnBrokerApplication")
                    {
                        Session["RptConn"] = "SAP";
                        Session["ReportName"] = ConfigSettings.BrokerApplication;
                    }
                    else if (buttonId == "btnAccreditationAgreement")
                    {
                        Session["RptConn"] = "SAP";
                        Session["ReportName"] = ConfigSettings.AccreditationAgreement;
                    }
                    else if (buttonId == "btnGeneralPolicies")
                    {
                        Session["RptConn"] = "SAP";
                        Session["ReportName"] = ConfigSettings.BrokerAccreditationGeneralPolicies;
                    }
                    else if (buttonId == "btnListOfAccredited")
                    {
                        Session["RptConn"] = "SAP";
                        Session["ReportName"] = ConfigSettings.ListofAccreditedSalesPersons;
                    }


                    //open new tab
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openpage", "window.open('ReportViewer.aspx');", true);

                }
                else
                {
                    alertMsg("No broker selected. Please reload the page.", "info");
                }
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }
        }

        //protected void btnSalesPersonDelete_Click(object sender, EventArgs e)
        protected void btnSalesPersonDelete_Command(object sender, CommandEventArgs e)
        {
            string itemId = e.CommandArgument.ToString();
            ViewState["SalesPersonToDelete"] = itemId;
            confirmDelete("Are you sure you want to delete this Sales Person?");
        }
        void confirmDelete(string body)
        {
            lblDeleteConfirmation.Text = body;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showConfirmDelete", "showConfirmDelete();", true);
        }
        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            try
            {
                //LinkButton btn = (LinkButton)sender;
                //GridViewRow row = (GridViewRow)btn.NamingContainer;

                //string index = row.Cells[0].Text;
                //string SalesPersonName = row.Cells[1].Text;

                string index = ViewState["SalesPersonToDelete"].ToString();
                DataTable dt = (DataTable)ViewState["SalesPerson"];
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dt.Rows[i];
                    if (dr[0].ToString() == index)
                    {
                        dr.Delete();
                        break;
                    }

                }
                dt.AcceptChanges();
                ViewState["SalesPerson"] = dt;
                BindGrid();

                DataTable dt2 = (DataTable)ViewState["SharingDetails"];
                for (int i = dt2.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dt2.Rows[i];
                    if (dr[1].ToString() == index)
                    {
                        dr.Delete();
                    }

                }
                dt2.AcceptChanges();
                ViewState["SharingDetails"] = dt2;
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowAddSalesPersonModal();", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "hideConfirmDelete();", true);
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "hideConfirmDelete();", true);
            }
        }
        //GAB 06/29/2023 REVISED
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gridViewRow = (GridViewRow)(sender as Control).Parent.Parent;
                string id = gridViewRow.Cells[7].Text;
                DataTable dt = (DataTable)ViewState["SharingDetails"];
                var shareId = ViewState["SharingID"];
                DataTable filteredDT;
                //int count = 0;
                bool type = Convert.ToBoolean(ViewState["isLot"]);
                int newId = 1;
                List<DataRow> rowsToRemove = new List<DataRow>(); ;

                if (type == true)
                {
                    filteredDT = dt.Select($"SalesPersonId = '{shareId}'").Where(x => x[5].ToString() == "0").CopyToDataTable();
                }
                else
                {
                    filteredDT = dt.Select($"SalesPersonId = '{shareId}'").Where(x => x[4].ToString() == "0").CopyToDataTable();
                }


                //Fix row number of all rows when a certain row is deleted
                //foreach (DataRow rows in dt.Select($"SalesPersonId = '{ViewState["SharingID"]}'").CopyToDataTable().Rows)
                //{
                //    if ((count + 1) <= dt.Select($"SalesPersonId = '{ViewState["SharingID"]}'").CopyToDataTable().Rows.Count)
                //    {
                //        if (rows[0].ToString() == id)
                //        {
                //            dt.Select($"SalesPersonId = '{ViewState["SharingID"]}'")[count].Delete();
                //            //rows[0] = count++;
                //            //dt.Select($"SalesPersonId = '{ViewState["SharingID"]}'").CopyToDataTable().Rows.Remove(rows);                       
                //        }
                //        if (dt.Select($"SalesPersonId = '{ViewState["SharingID"]}'").Length == 0)
                //        {
                //            break;
                //        }
                //        if (dt.Select($"SalesPersonId = '{ViewState["SharingID"]}'").Length >= count + 1)
                //        {
                //            dt.Select($"SalesPersonId = '{ViewState["SharingID"]}'")[count][0] = count + 1;
                //        }
                //        count++;
                //    }
                //}
                foreach (DataRow rows in filteredDT.Rows)
                {
                    if (rows[7].ToString() == id)
                    {
                        //filteredDT.Rows.Remove(rows);
                        foreach (DataRow row in dt.Rows)
                        {
                            if (row[7].ToString() == id)
                            {
                                rowsToRemove.Add(row);
                                //dt.Rows.Remove(row);
                                break;
                            }
                        }
                    }
                }

                foreach (DataRow row in rowsToRemove)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr[7].ToString() == id)
                        {
                            dt.Rows.Remove(dr);
                            break;
                        }
                    }
                }

                if (type == true)
                {
                    //forLotOnly
                    foreach (DataRow row in dt.Select($"SalesPersonId = '{shareId}'").Where(x => x[5].ToString() == "0"))
                    {
                        row["Id"] = newId;
                        newId++;
                    }
                }
                else
                {
                    //forHouseNLot
                    foreach (DataRow row in dt.Select($"SalesPersonId = '{shareId}'").Where(x => x[4].ToString() == "0"))
                    {
                        row["Id"] = newId;
                        newId++;
                    }
                }

                ViewState["SharingDetails"] = dt;
                //DataTable dt2 = new DataTable();
                //dt2 = (DataTable)ViewState["SharingDetails"];
                //for (int i = dt.Select($"SalesPersonId = '{ViewState["SharingID"]}'").CopyToDataTable().Rows.Count; i >= 0; i--)
                //{
                //    DataRow dr = dt.Select($"SalesPersonId = '{ViewState["SharingID"]}'").CopyToDataTable().Rows[i];
                //    if (dr[0].ToString() == id)
                //    {
                //        for (int x = i; x < (dt.Select($"SalesPersonId = '{ViewState["SharingID"]}'").CopyToDataTable().Rows.Count) - 1; x++)
                //        {
                //            DataRow dr1 = dt.Select($"SalesPersonId = '{ViewState["SharingID"]}'").CopyToDataTable().Rows[x];
                //            dr1[0] = x;
                //        }
                //        dt.Select($"SalesPersonId = '{ViewState["SharingID"]}'")[i].Delete();
                //        ViewState["SharingDetails"] = dt;
                //        break;
                //    }

                //}
                if (ViewState["SharingID"] != null)
                {
                    //if (dt.Select($"SalesPersonId = '{ViewState["SharingID"]}'").Any())
                    //{
                    //    gvShareDetails.DataSource = null;
                    //    gvShareDetails.DataBind();
                    //    gvShareDetails.DataSource = dt.Select($"SalesPersonId = '{ViewState["SharingID"]}'").CopyToDataTable();
                    //    gvShareDetails.DataBind();
                    //}
                    if (dt.Select($"SalesPersonId = '{ViewState["SharingID"]}' AND LotOnlyPercentage <> '0'").Any() && Convert.ToBoolean(ViewState["isLot"]) == true)
                    {
                        gvShareDetails.DataSource = null;
                        gvShareDetails.DataBind();
                        gvShareDetails.DataSource = dt.Select($"SalesPersonId = '{ViewState["SharingID"]}'").Where(x => x[5].ToString() == "0").CopyToDataTable();
                        gvShareDetails.DataBind();
                    }
                    else if (dt.Select($"SalesPersonId = '{ViewState["SharingID"]}' AND HouseandLotPercentage <> '0'").Any() && Convert.ToBoolean(ViewState["isLot"]) == false)
                    {
                        gvShareDetails.DataSource = null;
                        gvShareDetails.DataBind();
                        gvShareDetails.DataSource = dt.Select($"SalesPersonId = '{ViewState["SharingID"]}'").Where(x => x[4].ToString() == "0").CopyToDataTable();
                        gvShareDetails.DataBind();
                    }
                    else
                    {
                        gvShareDetails.DataSource = null;
                        gvShareDetails.DataBind();
                    }
                }
                else
                {
                    gvShareDetails.DataSource = null;
                    gvShareDetails.DataBind();
                }

                //CommPercent.Text = ViewState["Commission"].ToString();
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "ShowAddSharingModal();", true);
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }
        }
        string updateComments()
        {

            string tempGenInfoComments = txtGeneralInfoComments.Text;
            string tempAddressBisComments = txtAddressBusinessComments.Text;
            string tempSuppDetailsComments = txtSupplementaryDetailsComments.Text;
            string tempPRCLicInfoComments = txtPRCLicenseInformationComments.Text;

            string ret = "Operation completed successfully.";

            string document = updateStandardDocumentComments("txtStandardAttachmentComments", ViewState["BrokerID"].ToString(), rbTypeOfBusiness.SelectedItem.Text.Trim(), "chkStandard");
            document = updateDocumentComments(gvStandardDocumentRequirements, "txtStandardAttachmentComments", ViewState["BrokerID"].ToString(), "Standard", "chkStandard", "lblFileName", "txtStandardAttachmentDateIssued");

            if (document != "Operation completed successfully.")
            {
                rollbackComments(tempGenInfoComments, tempAddressBisComments, tempSuppDetailsComments, tempPRCLicInfoComments);
                ret = $"Failed to submit your documents! Please contact your administrator (Error: DocumentComments - {document})";
                alertMsg(ret, "error");
            }
            else
            {

                string textbox = "";
                string checkbox = "";
                string filename = "";
                string date = "";
                GridView gv;
                if (rbTypeOfBusiness.SelectedIndex == 0)
                {
                    textbox = "txtSolePropDocumentRequirementsComments";
                    gv = gvSolePropDocumentRequirements;
                    checkbox = "chkSole";
                    date = "txtSolePropDocumentRequirementsDateIssued";
                    filename = "lblFileName2";
                }
                else if (rbTypeOfBusiness.SelectedIndex == 1)
                {
                    textbox = "txtPartnershipDocumentRequirementsComments";
                    gv = gvPartnershipDocumentRequirements;
                    checkbox = "chkPartner";
                    date = "txtPartnershipDocumentRequirementsDateIssued";
                    filename = "lblFileName3";
                }
                else
                {
                    textbox = "txtCorporationDocumentRequirementsComments";
                    gv = gvCorporationDocumentRequirements;
                    checkbox = "chkCorp";
                    date = "txtCorporationDocumentRequirementsDateIssued";
                    filename = "lblFileName4";
                }

                document = updateDocumentComments(gv, textbox, ViewState["BrokerID"].ToString(), rbTypeOfBusiness.SelectedItem.Text.Trim(), checkbox, filename, date);

                if (document != "Operation completed successfully.")
                {
                    rollbackComments(tempGenInfoComments, tempAddressBisComments, tempSuppDetailsComments, tempPRCLicInfoComments);
                    ret = $"Failed to submit your documents! Please contact your administrator (Error: DocumentComments - {document})";
                    alertMsg(ret, "error");
                }
            }
            return ret;

        }

        //PTR Valid To validator
        protected void PTRValidToValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                DateTime checker = Convert.ToDateTime(args.Value);
                DateTime defaultDate = new DateTime(1900, 1, 1);
                DateTime validfrom = txtPTRValidFrom.Text == "" ? DateTime.MinValue : Convert.ToDateTime(txtPTRValidFrom.Text);
                if (checker.Date < DateTime.Now.Date.AddMonths(1) && checker.Date != defaultDate)
                {
                    ///COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                    PTRValidToValidator.Text = checker.Date >= DateTime.Now.Date ? "PTR Expiry Date should be atleast a month from today." : "PTR Expiry Date cannot be earlier than today.";
                    //RequiredFieldValidator26.Visible = false;
                    CompareValidator6.Visible = false;
                    args.IsValid = false;
                }
                //GAB 07/01/2023 Added comparison to valid from error
                else if (checker.Date.CompareTo(validfrom) < 0)
                {
                    PTRValidToValidator.Text = "PTR To cannot be earlier than PTR From.";
                    CompareValidator6.Visible = false;
                    args.IsValid = false;
                }
                else
                {
                    ///COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                    //RequiredFieldValidator26.Visible = true;
                    CompareValidator6.Visible = true;
                    args.IsValid = true;
                }
            }
            catch (Exception ex)
            {
                args.IsValid = false;
                alertMsg(ex.Message, "error");
            }
            #region OLD CODE
            //GAB 06/29/2023 Commented 
            //DateTime checker = Convert.ToDateTime(args.Value);
            //string from = Convert.ToDateTime(txtPTRValidFrom.Text).ToString();

            //if (checker.Date < DateTime.Today.Date && from != "1/1/1900 12:00:00 AM")
            //{
            //    ///COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
            //    //RequiredFieldValidator26.Visible = false;
            //    CompareValidator6.Visible = false;
            //    args.IsValid = false;
            //}
            //else
            //{
            //    ///COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
            //    //RequiredFieldValidator26.Visible = true;
            //    CompareValidator6.Visible = true;
            //    args.IsValid = true;
            //}
            #endregion
        }
        //ATPExpiryDate validator
        protected void CustomValidator2_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime checker = Convert.ToDateTime(args.Value);
            DateTime defaultDate = new DateTime(1900, 1, 1);
            //if (checker.Date < DateTime.Today.Date.AddDays(30) && checker.Date != defaultDate)
            //GAB 07/03/2023 CHANGED REASON: Grace Period uniformity with PRC and PTR (Use this according to Sir Joses)
            if (checker.Date < DateTime.Now.Date.AddMonths(1) && checker.Date != defaultDate)
            {
                ///COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                //GAB UNCOMMENTED 07/03/2023
                //RequiredFieldValidator22.Visible = false;
                CustomValidator2.Text = checker.Date >= DateTime.Now.Date ? "ATP Expiry Date should be atleast a month from today." : "ATP Expiry Date cannot be earlier than today.";
                CompareValidator1.Visible = false;
                args.IsValid = false;
            }
            else
            {
                ///COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                //RequiredFieldValidator22.Visible = true;
                CompareValidator1.Visible = true;
                args.IsValid = true;
            }
        }
        //date of birth validator
        protected void CustomValidator3_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime checker = Convert.ToDateTime(args.Value);
            if (checker.Date > DateTime.Today.Date)
            {
                RequiredFieldValidator24.Visible = true;
                CompareValidator3.Visible = true;
                args.IsValid = false;
            }
            else
            {
                RequiredFieldValidator24.Visible = true;
                CompareValidator3.Visible = true;
                args.IsValid = true;
            }
        }

        //Sales PRC/ Accreditation Expiration Date validator
        protected void CustomValidator12_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                //20220517 make condition less than or equal -KASO
                DateTime checker = Convert.ToDateTime(args.Value);
                DateTime defaultDate = new DateTime(1900, 1, 1);
                //if (checker.Date <= DateTime.Now.Date.AddMonths(1))
                //GAB 07/03/2023 CHANGED REASON: Grace Period uniformity with PRC and PTR (Use this according to Sir Joses)
                if (checker.Date < DateTime.Now.Date.AddMonths(1) && checker.Date != defaultDate)
                {
                    ///COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                    CustomValidator12.Text = checker.Date >= DateTime.Now.Date ? "PRC/ Accreditation Expiration Date should be atleast a month from today." : "PRC/ Accreditation Expiration Date cannot be earlier than today.";
                    //CustomValidator12.Visible = true;
                    //RequiredFieldValidator39.Visible = false;
                    CompareValidator12.Visible = false;
                    args.IsValid = false;
                }
                else
                {
                    ///COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                    //CustomValidator12.Visible = true;
                    //RequiredFieldValidator39.Visible = true;
                    CompareValidator12.Visible = true;
                    args.IsValid = true;
                }
            }
            catch (Exception ex)
            {
                args.IsValid = false;
                alertMsg(ex.Message, "error");
            }
        }

        //Sales ATP/ OR Date validator
        protected void CustomValidator13_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {

                //DateTime checker = Convert.ToDateTime(args.Value);
                ////if (checker.Date < DateTime.Now.Date.AddMonths(1))
                //if (checker.Date > DateTime.Now.Date)
                //{
                //    CustomValidator13.Text = checker.Date >= DateTime.Now.Date ? "ATP/ OR Date is Invalid" : "ATP/OR Issue Date cannot be later than today.";
                //    CustomValidator13.Visible = true;
                //    RequiredFieldValidator40.Visible = false;
                //    args.IsValid = false;
                //}
                //else
                //{
                //    CustomValidator13.Visible = true;
                //    RequiredFieldValidator40.Visible = true;
                //    args.IsValid = true;
                //}
                DateTime checker = Convert.ToDateTime(args.Value);
                DateTime defaultDate = new DateTime(1900, 1, 1);
                //20220517 Changed from adddays(30) to addmonth(1), make condition less than or equal and added custom validator 13 text -KASO
                //if (checker.Date <= DateTime.Today.Date.AddMonths(1))
                //GAB 07/03/2023 CHANGED REASON: Grace Period uniformity with PRC and PTR (Use this according to Sir Joses)
                if (checker.Date < DateTime.Now.Date.AddMonths(1) && checker.Date != defaultDate)

                {
                    ///COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                    CustomValidator13.Text = checker.Date >= DateTime.Now.Date ? "ATP Expiry Date should be atleast a month from today." : "ATP Expiry Date cannot be earlier than today.";
                    //RequiredFieldValidator40.Visible = false;
                    CustomValidator13.Visible = true;
                    args.IsValid = false;
                }
                else
                {
                    ///COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                    //RequiredFieldValidator40.Visible = true;
                    CustomValidator13.Visible = true;
                    args.IsValid = true;
                }
            }
            catch (Exception ex)
            {
                args.IsValid = false;
                alertMsg(ex.Message, "error");
            }
        }
        //GAB 07/01/2023 Sales Person Valid From Validator
        protected void CustomValidator4_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //GAB 07/03/2023 COMMENDTED REASON: Not being used anymore and the code is a mess
            #region Old Code
            //DateTime checker = Convert.ToDateTime(args.Value);
            ////DateTime validto = txtPassportValidTo.Text == "" ? DateTime.MinValue : Convert.ToDateTime(txtPassportValidTo.Text);
            //DateTime validto = DateTime.MinValue;
            ////if (checker < DateTime.Today)
            ////{
            ////    CustomValidator4.Text = "Passport Valid From cannot be lower than today.";
            ////    RequiredFieldValidator41.Visible = false;
            ////    args.IsValid = false;
            ////}
            ////else
            ////{
            //if (checker.Date > validto.Date)
            //{
            //    //RequiredFieldValidator41.Visible = true;
            //    //args.IsValid = true;
            //}
            //else
            //{
            //    //CustomValidator4.Text = "Passport Valid From cannot be lower than Passport Valid To.";
            //    //RequiredFieldValidator41.Visible = false;
            //    //args.IsValid = false;
            //}
            ////}
            #endregion

            DateTime checker = Convert.ToDateTime(args.Value);
            DateTime validto = mtxtValidTo.Text == "" ? DateTime.MinValue : Convert.ToDateTime(mtxtValidTo.Text);
            if (checker.Date > DateTime.Today.Date)
            {
                CustomValidator4.Text = "Valid From cannot be later than today.";
                CompareValidator9.Visible = false;
                RequiredFieldValidator29.Visible = false;
                args.IsValid = false;
            }
            else if (checker.Date == validto.Date)
            {
                RequiredFieldValidator29.Visible = false;
                CustomValidator4.Text = "Valid From cannot be the same as Valid To.";
                CompareValidator9.Visible = false;
                args.IsValid = false;
            }
            else
            {
                //RequiredFieldValidator29.Visible = true;
                //CompareValidator9.Visible = true;
                args.IsValid = true;
            }
        }
        //GAB 07/01/2023 Sales Person Valid To Validator
        protected void ValidToValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                DateTime checker = Convert.ToDateTime(args.Value);
                DateTime validfrom = mtxtValidFrom.Text == "" ? DateTime.MinValue : Convert.ToDateTime(mtxtValidFrom.Text);
                if (checker.Date < DateTime.Now.Date.AddMonths(1))
                {
                    ValidToValidator.Text = checker.Date >= DateTime.Now.Date ? "Valid To should be atleast a month from today." : "Valid to cannot be earlier than today.";
                    RequiredFieldValidator33.Visible = false;
                    CompareValidator10.Visible = false;
                    args.IsValid = false;
                }
                else if (checker.Date.CompareTo(validfrom) < 0)
                {
                    ValidToValidator.Text = "Valid To cannot be earlier than Valid From.";
                    RequiredFieldValidator33.Visible = false;
                    CompareValidator10.Visible = false;
                    args.IsValid = false;
                }
                else
                {
                    //RequiredFieldValidator33.Visible = true;
                    //CompareValidator10.Visible = true;
                    args.IsValid = true;
                }
            }
            catch (Exception ex)
            {
                args.IsValid = false;
                alertMsg(ex.Message, "error");
            }
        }

        //PRC Expiration Date validator
        protected void CustomValidator5_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime checker = Convert.ToDateTime(args.Value);
            DateTime defaultDate = new DateTime(1900, 1, 1);
            DateTime validfrom = txtRegistrationDate.Text == "" ? DateTime.MinValue : Convert.ToDateTime(txtRegistrationDate.Text);
            if (checker.Date < DateTime.Now.Date.AddMonths(1) && checker.Date != defaultDate)
            {
                //RequiredFieldValidator19.Visible = false;
                CustomValidator5.Text = checker.Date >= DateTime.Now.Date ? "Valid Until should be atleast a month from today." : "Valid Until cannot be earlier than today.";
                CompareValidator4.Visible = false;
                args.IsValid = false;
            }
            //GAB 07/01/2023 Added comparison to valid from error
            else if (checker.Date.CompareTo(validfrom) < 0)
            {
                CustomValidator5.Text = "PRC Expiration Date cannot be earlier than Registration Date.";
                CompareValidator6.Visible = false;
                args.IsValid = false;
            }
            else
            {
                //RequiredFieldValidator19.Visible = true;
                CompareValidator4.Visible = true;
                args.IsValid = true;
            }
        }
        //PTR Valid From validator
        protected void CustomValidator6_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime checker = Convert.ToDateTime(args.Value);
            DateTime validto = txtPTRValidTo.Text == "" ? DateTime.MinValue : Convert.ToDateTime(txtPTRValidTo.Text);
            if (checker.Date > DateTime.Today.Date)
            {
                ///COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                CustomValidator6.Text = "PTR Valid From cannot be later than today.";
                CompareValidator5.Visible = false;
                //RequiredFieldValidator18.Visible = false;
                args.IsValid = false;
            }
            //else
            //{
            else if (checker.Date <= validto.Date)
            {
                ///COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                //RequiredFieldValidator18.Visible = true;
                CompareValidator5.Visible = true;
                args.IsValid = true;
            }
            else
            {
                ///COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                CustomValidator6.Text = "PTR Valid From cannot be later than PTR Valid To.";
                //RequiredFieldValidator18.Visible = false;
                CompareValidator5.Visible = false;
                args.IsValid = false;
            }
            //}
        }
        //AIPO Valid To validator
        protected void CustomValidator7_ServerValidate(object source, ServerValidateEventArgs args)
        {
            double total = 0;

            try
            {
                //foreach (GridViewRow row in gvShareDetails.Rows)
                //{
                //    if (Convert.ToBoolean(ViewState["isLot"]) == true)
                //    {
                //        total += Convert.ToDouble(row.Cells[4].Text);
                //    }
                //    else
                //    {
                //        total += Convert.ToDouble(row.Cells[5].Text);
                //    }
                //    //gvShareDetails.Columns[4].ItemStyle.CssClass = "";
                //    //gvShareDetails.Columns[5].ItemStyle.CssClass = "hidden";
                //    //gvShareDetails.Columns[4].HeaderStyle.CssClass = "";
                //    //gvShareDetails.Columns[5].HeaderStyle.CssClass = "hidden";
                //    //ViewState["isLot"] = true;
                //}


                DataTable dt2 = new DataTable();
                dt2 = (DataTable)ViewState["SharingDetails"];
                var sharingIDDetails = ViewState["SharingID"];


                if ((dt2.Select($"SalesPersonId = '{ViewState["SharingID"]}'").AsEnumerable().Where(x => x.RowState != DataRowState.Deleted ? x["SalesPersonId"].ToString() == ViewState["SharingID"].ToString() : false).Any()))
                {
                    dt2 = dt2.Select($"SalesPersonId = '{ViewState["SharingID"]}'").CopyToDataTable();
                }

                if (dt2.Select($"SalesPersonId = '{ViewState["SharingID"]}'").AsEnumerable().Where(x => x.RowState != DataRowState.Deleted ? x[1].ToString() == ViewState["SharingID"].ToString() : false).Any())
                {

                    foreach (DataRow row in dt2.Rows)
                    {
                        if (row.RowState != DataRowState.Deleted)
                        {
                            var test2 = dt2;
                            if (Convert.ToBoolean(ViewState["isLot"]) == true)
                            {
                                total += Convert.ToDouble(row["LotOnlyPercentage"].ToString());
                            }
                            else
                            {
                                total += Convert.ToDouble(row["HouseandLotPercentage"].ToString());
                            }
                        }
                    }
                }






                if (total != 0)
                {
                    if (Convert.ToBoolean(ViewState["isLot"]) == true)
                    {
                        if (total != 7)
                        {
                            args.IsValid = false;
                            CustomValidator7.Text = "Sum should be 7%!";
                        }
                        else
                        {
                            args.IsValid = true;
                        }
                    }
                    else
                    {
                        if (total != 5)
                        {
                            args.IsValid = false;
                            CustomValidator7.Text = "Sum should be 5%!";
                        }
                        else
                        {
                            args.IsValid = true;
                        }
                    }
                }
                else
                {
                    args.IsValid = true;
                }
            }
            catch (Exception ex)
            {
                args.IsValid = false;
                alertMsg(ex.Message, "error");
            }

            //DateTime checker = Convert.ToDateTime(args.Value);
            //if (checker.Date < DateTime.Now.Date.AddMonths(1))
            //{
            //    CustomValidator7.Text = checker.Date >= DateTime.Now.Date ? "AIPO Valid To is Invalid" : "AIPO Valid To cannot be lower than today.";
            //    RequiredFieldValidator51.Visible = false;
            //    args.IsValid = false;
            //}
            //else
            //{
            //    RequiredFieldValidator51.Visible = true;
            //    args.IsValid = true;
            //}
        }

        //GAB 06/26/2023 AIPO Valid To Validator
        protected void AIPOValidToValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                DateTime checker = Convert.ToDateTime(args.Value);
                DateTime validfrom = txtAIPOValidFrom.Text == "" ? DateTime.MinValue : Convert.ToDateTime(txtAIPOValidFrom.Text);
                if (checker.Date.CompareTo(validfrom) < 0)
                {
                    CustomValidator8.Text = "AIPO Valid To cannot be earlier than AIPO Valid From.";
                    args.IsValid = false;
                }
                else
                {
                    args.IsValid = true;
                }
            }
            catch (Exception ex)
            {
                args.IsValid = false;
                alertMsg(ex.Message, "error");
            }
        }
        //AIPO Valid From validator
        protected void CustomValidator8_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime checker = Convert.ToDateTime(args.Value);
            DateTime validto = txtAIPOValidTo.Text == "" ? DateTime.MinValue : Convert.ToDateTime(txtAIPOValidTo.Text);
            //if (checker.Date > DateTime.Today.Date)
            //{
            //    CustomValidator8.Text = "AIPO Valid From cannot be earlier than today.";
            //    CompareValidator7.Visible = false;
            //    //RequiredFieldValidator53.Visible = false;
            //    args.IsValid = false;
            //}
            //else
            //{
            if (checker.Date.CompareTo(DateTime.Today.Date) > 0)
            {
                CustomValidator8.Text = "AIPO Valid From cannot be later than today.";
                args.IsValid = false;
            }
            else if (checker.Date <= validto.Date)
            {
                CustomValidator8.Visible = true;
                args.IsValid = true;
            }
            else
            {
                CustomValidator8.Text = "AIPO Valid From cannot be later than AIPO Valid To.";
                args.IsValid = false;
            }
            //if (checker.Date > DateTime.Now.Date)
            //{
            //    //RequiredFieldValidator53.Visible = true;
            //    args.IsValid = true;
            //}
            //else
            //{
            //    //CustomValidator8.Text = "AIPO Valid From cannot be lower than AIPO Valid To.";
            //    //RequiredFieldValidator53.Visible = false;
            //    CompareValidator7.Visible = false;
            //    args.IsValid = false;
            //}
            //}
        }

        //PRC Registration Validator
        protected void CustomValidator9_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {

                DateTime checker = Convert.ToDateTime(args.Value);
                DateTime validto = txtPRCLicenseExpirationDate.Text == "" ? DateTime.MinValue : Convert.ToDateTime(txtPRCLicenseExpirationDate.Text);
                if (checker.Date > DateTime.Today.Date)
                {
                    CustomValidator9.Text = "Registration Date cannot be later than today.";
                    //RequiredFieldValidator14.Visible = false;
                    CompareValidator21.Visible = false;
                    args.IsValid = false;
                }
                //else
                //{
                else if (checker.Date <= validto.Date)
                {
                    //RequiredFieldValidator14.Visible = true;
                    CompareValidator21.Visible = true;
                    args.IsValid = true;
                }
                else
                {
                    CustomValidator9.Text = "Registration Date cannot be later than PRC Valid Until.";
                    //RequiredFieldValidator14.Visible = false;
                    CompareValidator21.Visible = false;
                    args.IsValid = false;
                }
            }
            catch (Exception ex)
            {
                args.IsValid = false;
                alertMsg(ex.Message, "error");
            }
        }
        //Valid ID Validator
        protected void CustomValidator10_ServerValidate(object source, ServerValidateEventArgs args)
        {

            DateTime checker = Convert.ToDateTime(args.Value);
            if (checker.Date < DateTime.Now.Date.AddMonths(1))
            {
                CustomValidator10.Text = checker >= DateTime.Now ? "ID Expiration Date should be atleast a month from today." : "ID Expiration Date cannot be earlier than today.";
                CustomValidator10.Visible = true;
                RequiredFieldValidator55.Visible = false;
                CompareValidator17.Visible = false;
                args.IsValid = false;
            }
            else
            {
                CustomValidator10.Visible = true;
                RequiredFieldValidator55.Visible = true;
                CompareValidator17.Visible = true;
                args.IsValid = true;
            }

        }

        protected void CustomValidator21_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {

                DateTime checker = Convert.ToDateTime(args.Value);
                DateTime ekek = Convert.ToDateTime("01/01/" + DateTime.Now.Year.ToString());

                if (checker.Date > DateTime.Now.Date)
                {
                    CustomValidator21.Text = "Date cannot be later than today.";
                    args.IsValid = false;
                }
            }
            catch (Exception ex)
            {
                args.IsValid = false;
                alertMsg(ex.Message, "error");
            }
        }

        //TIN Format Validator
        protected void CustomValidator14_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //Check if TIN is in correct format(###-###-###-###)
            bool isOK = Regex.IsMatch(txtTIN.Text, @"[0-9][0-9][0-9]\-[0-9][0-9][0-9]\-[0-9][0-9][0-9]\-[0-9][0-9][0-9]");
            if (!isOK)
            {
                RequiredFieldValidator16.Visible = false;
                args.IsValid = false;
            }
            else
            {
                RequiredFieldValidator16.Visible = true;
                args.IsValid = true;
            }
        }
        protected void CustomValidator15_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //Check if TIN is in correct format(###-###-###-###)
            bool isOK = Regex.IsMatch(mtxtTIN.Text, @"[0-9][0-9][0-9]\-[0-9][0-9][0-9]\-[0-9][0-9][0-9]\-[0-9][0-9][0-9]");
            if (!isOK)
            {
                RequiredFieldValidator36.Visible = false;
                args.IsValid = false;
            }
            else
            {
                RequiredFieldValidator36.Visible = true;
                args.IsValid = true;
            }
        }


        //ID Expiration date 2 Validator
        protected void CustomValidator18_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime checker = Convert.ToDateTime(args.Value);
            if (checker.Date < DateTime.Now.Date.AddMonths(1))
            {
                ///COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                //CustomValidator18.Text = checker.Date >= DateTime.Now.Date ? "ID Expiration Date is Invalid." : "ID Expiration Date cannot be lower than today.";
                //CustomValidator18.Visible = true;
                //RequiredFieldValidator58.Visible = false;
                //CompareValidator18.Visible = false;
                //args.IsValid = false;
            }
            else
            {
                ///COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                //CustomValidator18.Visible = true;
                //RequiredFieldValidator58.Visible = true;
                //CompareValidator18.Visible = true;
                //args.IsValid = true;
            }
        }
        //ID Expiration date 3 Validator
        protected void CustomValidator19_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime checker = Convert.ToDateTime(args.Value);
            if (checker.Date < DateTime.Now.Date.AddMonths(1))
            {
                CustomValidator19.Text = checker.Date >= DateTime.Now.Date ? "ID Expiration Date should be atleast a month from today." : "ID Expiration Date cannot be earlier than today.";
                CustomValidator19.Visible = true;
                args.IsValid = false;
            }
            else
            {
                CustomValidator19.Visible = true;
                args.IsValid = true;
            }
        }

        //ID Expiration date 4 Validator
        protected void CustomValidator20_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime checker = Convert.ToDateTime(args.Value);
            if (checker.Date < DateTime.Now.Date.AddMonths(1))
            {
                if (txtIDExpirationDate4.Text == null)
                {
                    CustomValidator20.Text = checker.Date >= DateTime.Now.Date ? "ID Expiration Date should be atleast a month from today." : "ID Expiration Date cannot be earlier than today.";
                    CustomValidator20.Visible = true;
                    args.IsValid = false;
                }
            }
            else
            {
                CustomValidator20.Visible = true;
                args.IsValid = true;
            }
        }

        protected void cvStandardAttachmentDateIssued_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                DateTime checker = Convert.ToDateTime(args.Value);
                if (checker.Date > DateTime.Now.Date)
                {
                    args.IsValid = false;
                }
                else
                {
                    args.IsValid = true;
                }
            }
            catch (Exception ex)
            {
                args.IsValid = false;
                alertMsg(ex.Message, "error");
            }

        }

        protected void cvSolePropDocumentRequirementsDateIssued_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                DateTime checker = Convert.ToDateTime(args.Value);
                if (checker.Date > DateTime.Now.Date)
                {
                    args.IsValid = false;
                }
                else
                {
                    args.IsValid = true;
                }
            }
            catch (Exception ex)
            {
                args.IsValid = false;
                alertMsg(ex.Message, "error");
            }

        }
        protected void cvPartnershipDocumentRequirementsDateIssued_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                DateTime checker = Convert.ToDateTime(args.Value);
                if (checker.Date > DateTime.Now.Date)
                {
                    args.IsValid = false;
                }
                else
                {
                    args.IsValid = true;
                }
            }
            catch (Exception ex)
            {
                args.IsValid = false;
                alertMsg(ex.Message, "error");
            }

        }

        protected void cvCorporationDocumentRequirementsDateIssued_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                DateTime checker = Convert.ToDateTime(args.Value);
                if (checker.Date > DateTime.Now.Date)
                {
                    args.IsValid = false;
                }
                else
                {
                    args.IsValid = true;
                }
            }
            catch (Exception ex)
            {
                args.IsValid = false;
                alertMsg(ex.Message, "error");
            }

        }

        protected void dpIssuedBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string checker = dpIssuedBy.SelectedValue;
            string checker = "";
            if (checker == "")
            {
                //txtPlaceIssued.ReadOnly = true;
                //txtPlaceIssued.CausesValidation = false;
                //rfvPlaceIssued.Enabled = false;
                //txtPlaceIssued.Text = "";
            }
            else
            {
                //txtPlaceIssued.ReadOnly = false;
                //txtPlaceIssued.CausesValidation = true;
                //rfvPlaceIssued.Enabled = true;
            }
        }


        #region Valid ID
        //Disable other valid id fields when no valid id selected
        protected void ddValidID4_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["ValidId"];
            DataTable dt2 = (DataTable)ViewState["ValidIdPrev"];
            string checker = ddValidID4.SelectedValue;
            if (checker == "---Select Valid ID---")
            {
                txtIDNo4.Text = "";
                txtIDExpirationDate4.Text = null;
                txtIDNo4.ReadOnly = true;
                txtIDExpirationDate4.ReadOnly = true;
                if (dt2.Rows[3]["Code"].ToString() != "---Select Valid ID---" && dt2.Rows[3]["Code"].ToString() != "OTH" && dt2.Rows[3]["Code"].ToString() != checker)
                {
                    dt.Rows.Add(
                    dt2.Rows[3]["Code"].ToString(),
                    dt2.Rows[3]["Name"].ToString()
                    );
                }
                dt2.Rows[3]["Code"] = ddValidID4.SelectedValue;
                dt2.Rows[3]["Name"] = ddValidID4.SelectedItem;
                ChangeOthers(false, 4);
            }
            else if (checker == "OTH")
            {
                txtIDNo4.ReadOnly = false;
                txtIDExpirationDate4.ReadOnly = false;
                if (dt2.Rows[3]["Code"].ToString() != "---Select Valid ID---" && dt2.Rows[3]["Code"].ToString() != "OTH" && dt2.Rows[3]["Code"].ToString() != checker)
                {
                    dt.Rows.Add(
                    dt2.Rows[3]["Code"].ToString(),
                    dt2.Rows[3]["Name"].ToString()
                    );
                }
                dt2.Rows[3]["Code"] = ddValidID4.SelectedValue;
                dt2.Rows[3]["Name"] = ddValidID4.SelectedItem;
                ChangeOthers(true, 4);
            }
            else
            {
                txtIDNo4.ReadOnly = false;
                txtIDExpirationDate4.ReadOnly = false;
                if (dt2.Rows[3]["Code"].ToString() != "---Select Valid ID---" && dt2.Rows[3]["Code"].ToString() != "OTH" && dt2.Rows[3]["Code"].ToString() != checker)
                {
                    dt.Rows.Add(
                    dt2.Rows[3]["Code"].ToString(),
                    dt2.Rows[3]["Name"].ToString()
                    );
                }
                dt2.Rows[3]["Code"] = ddValidID4.SelectedValue;
                dt2.Rows[3]["Name"] = ddValidID4.SelectedItem;
                if (ddValidID4.SelectedValue == "ID1")
                {
                    txtIDNo4.Text = txtTIN.Text;
                }
                foreach (DataRow row in dt.Rows)
                {
                    if (row[0].ToString() == checker && row[0].ToString() != "OTH" && row[0].ToString() != "---Select Valid ID---")
                    {
                        dt.Rows.Remove(row);
                        break;
                    }
                }
                ChangeOthers(false, 4);
            }
            ChangeValidIDDd(dt, dt2, "ddValidID4");
        }
        //Show 4th Valid ID if 2nd Valid ID has valid value
        protected void ddValidID3_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["ValidId"];
            DataTable dt2 = (DataTable)ViewState["ValidIdPrev"];
            string checker = ddValidID3.SelectedValue;
            if (checker == "---Select Valid ID---")
            {
                //idvalid4.Visible = false;
                //ddValidID4.SelectedValue = "---Select Valid ID---";
                //txtIDNo4.Text = "";
                //txtIDExpirationDate4.Text = null;
                //Disable other valid id fields when no valid id selected
                txtIDNo3.ReadOnly = true;
                txtIDExpirationDate3.ReadOnly = true;
                txtIDNo3.Text = "";
                txtIDExpirationDate3.Text = null;
                if (dt2.Rows[2]["Code"].ToString() != "---Select Valid ID---" && dt2.Rows[2]["Code"].ToString() != "OTH" && dt2.Rows[2]["Code"].ToString() != checker)
                {
                    dt.Rows.Add(
                    dt2.Rows[2]["Code"].ToString(),
                    dt2.Rows[2]["Name"].ToString()
                    );
                }
                dt2.Rows[2]["Code"] = ddValidID3.SelectedValue;
                dt2.Rows[2]["Name"] = ddValidID3.SelectedItem;
                ChangeOthers(false, 3);

            }
            else if (checker == "OTH")
            {
                //idvalid4.Visible = true;
                idvalid4.Visible = false;
                txtIDNo3.ReadOnly = false;
                txtIDExpirationDate3.ReadOnly = false;
                if (dt2.Rows[2]["Code"].ToString() != "---Select Valid ID---" && dt2.Rows[2]["Code"].ToString() != "OTH" && dt2.Rows[2]["Code"].ToString() != checker)
                {
                    dt.Rows.Add(
                    dt2.Rows[2]["Code"].ToString(),
                    dt2.Rows[2]["Name"].ToString()
                    );
                }
                dt2.Rows[2]["Code"] = ddValidID3.SelectedValue;
                dt2.Rows[2]["Name"] = ddValidID3.SelectedItem;
                ChangeOthers(true, 3);
            }
            else
            {
                //idvalid4.Visible = true;
                idvalid4.Visible = false;
                txtIDNo3.ReadOnly = false;
                txtIDExpirationDate3.ReadOnly = false;
                if (dt2.Rows[2]["Code"].ToString() != "---Select Valid ID---" && dt2.Rows[2]["Code"].ToString() != "OTH" && dt2.Rows[2]["Code"].ToString() != checker)
                {
                    dt.Rows.Add(
                    dt2.Rows[2]["Code"].ToString(),
                    dt2.Rows[2]["Name"].ToString()
                    );
                }
                dt2.Rows[2]["Code"] = ddValidID3.SelectedValue;
                dt2.Rows[2]["Name"] = ddValidID3.SelectedItem;
                if (ddValidID3.SelectedValue == "ID1")
                {
                    txtIDNo3.Text = txtTIN.Text;
                }
                foreach (DataRow row in dt.Rows)
                {
                    if (row[0].ToString() == checker && row[0].ToString() != "OTH" && row[0].ToString() != "---Select Valid ID---")
                    {
                        dt.Rows.Remove(row);
                        break;
                    }
                }
                ChangeOthers(false, 3);
            }
            ChangeValidIDDd(dt, dt2, "ddValidID3");
        }

        //Show 3rd Valid ID if 2nd Valid ID has valid value
        protected void ddValidID2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["ValidId"];
            DataTable dt2 = (DataTable)ViewState["ValidIdPrev"];
            string checker = ddValidID2.SelectedValue;

            txtOthers2.Text = "";
            txtIDNo2.Text = "";
            txtIDExpirationDate2.Text = null;

            if (checker == "---Select Valid ID---")
            {
                //idvalid3.Visible = false;
                //ddValidID3.SelectedValue = "---Select Valid ID---";
                //txtIDNo3.Text = "";
                //txtIDExpirationDate3.Text = null;
                //idvalid4.Visible = false;
                //ddValidID4.SelectedValue = "---Select Valid ID---";
                //txtIDNo4.Text = "";
                //txtIDExpirationDate4.Text = null;
                if (dt2.Rows[1]["Code"].ToString() != "---Select Valid ID---" && dt2.Rows[1]["Code"].ToString() != "OTH" && dt2.Rows[1]["Code"].ToString() != checker)
                {
                    dt.Rows.Add(
                    dt2.Rows[1]["Code"].ToString(),
                    dt2.Rows[1]["Name"].ToString()
                    );
                }
                dt2.Rows[1]["Code"] = ddValidID2.SelectedValue;
                dt2.Rows[1]["Name"] = ddValidID2.SelectedItem;
                ChangeOthers(false, 2);

            }
            else if (checker == "OTH")
            {
                //idvalid3.Visible = true;
                idvalid3.Visible = false;
                if (dt2.Rows[1]["Code"].ToString() != "---Select Valid ID---" && dt2.Rows[1]["Code"].ToString() != "OTH" && dt2.Rows[1]["Code"].ToString() != checker)
                {
                    dt.Rows.Add(
                    dt2.Rows[1]["Code"].ToString(),
                    dt2.Rows[1]["Name"].ToString()
                    );
                }
                dt2.Rows[1]["Code"] = ddValidID2.SelectedValue;
                dt2.Rows[1]["Name"] = ddValidID2.SelectedItem;
                ChangeOthers(true, 2);
            }
            else
            {
                //idvalid3.Visible = true;
                idvalid3.Visible = false;
                if (dt2.Rows[1]["Code"].ToString() != "---Select Valid ID---" && dt2.Rows[1]["Code"].ToString() != "OTH" && dt2.Rows[1]["Code"].ToString() != checker)
                {
                    dt.Rows.Add(
                    dt2.Rows[1]["Code"].ToString(),
                    dt2.Rows[1]["Name"].ToString()
                    );
                }
                dt2.Rows[1]["Code"] = ddValidID2.SelectedValue;
                dt2.Rows[1]["Name"] = ddValidID2.SelectedItem;
                if (ddValidID2.SelectedValue == "ID1")
                {
                    txtIDNo2.Text = txtTIN.Text;
                }
                foreach (DataRow row in dt.Rows)
                {
                    if (row[0].ToString() == checker && row[0].ToString() != "OTH" && row[0].ToString() != "---Select Valid ID---")
                    {
                        dt.Rows.Remove(row);
                        break;
                    }
                }
                ChangeOthers(false, 2);
            }
            ChangeValidIDDd(dt, dt2, "ddValidID2");
            txtIDNo2.Focus();
        }
        protected void ddlVATCode2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlWTAXCode2.Focus();
        }
        protected void ddValidID_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["ValidId"];
            DataTable dt2 = (DataTable)ViewState["ValidIdPrev"];
            string checker = ddValidID.SelectedValue;

            txtOthers1.Text = "";
            txtIDNo.Text = "";
            txtIDExpirationDate.Text = null;

            if (checker == "---Select Valid ID---")
            {
                if (dt2.Rows[0]["Code"].ToString() != "---Select Valid ID---" && dt2.Rows[0]["Code"].ToString() != "OTH" && dt2.Rows[0]["Code"].ToString() != checker)
                {
                    dt.Rows.Add(
                    dt2.Rows[0]["Code"].ToString(),
                    dt2.Rows[0]["Name"].ToString()
                    );
                }
                dt2.Rows[0]["Code"] = ddValidID.SelectedValue;
                dt2.Rows[0]["Name"] = ddValidID.SelectedItem;
                ChangeOthers(false, 1);
            }
            else if (checker == "OTH")
            {
                if (dt2.Rows[0]["Code"].ToString() != "---Select Valid ID---" && dt2.Rows[0]["Code"].ToString() != "OTH" && dt2.Rows[0]["Code"].ToString() != checker)
                {
                    dt.Rows.Add(
                    dt2.Rows[0]["Code"].ToString(),
                    dt2.Rows[0]["Name"].ToString()
                    );
                }
                dt2.Rows[0]["Code"] = ddValidID.SelectedValue;
                dt2.Rows[0]["Name"] = ddValidID.SelectedItem;
                ChangeOthers(true, 1);
            }
            else
            {
                if (dt2.Rows[0]["Code"].ToString() != "---Select Valid ID---" && dt2.Rows[0]["Code"].ToString() != "OTH" && dt2.Rows[0]["Code"].ToString() != checker)
                {
                    dt.Rows.Add(
                    dt2.Rows[0]["Code"].ToString(),
                    dt2.Rows[0]["Name"].ToString()
                    );
                }
                dt2.Rows[0]["Code"] = ddValidID.SelectedValue;
                dt2.Rows[0]["Name"] = ddValidID.SelectedItem;
                if (ddValidID.SelectedValue == "ID1")
                {
                    txtIDNo.Text = txtTIN.Text;
                }
                foreach (DataRow row in dt.Rows)
                {
                    if (row[0].ToString() == checker && row[0].ToString() != "OTH" && row[0].ToString() != "---Select Valid ID---")
                    {
                        dt.Rows.Remove(row);
                        break;
                    }
                }
                ChangeOthers(false, 1);
            }
            ChangeValidIDDd(dt, dt2, "ddValidID");
            txtIDNo.Focus();
        }
        //Add/Remove textbox for Others Valid ID
        void ChangeOthers(bool change, int num)
        {
            if (change == true)
            {
                if (num == 1)
                {
                    others1.Visible = change;
                    //ddothers1.Style.Remove("col-md-4");
                    ddothers1.Attributes.Remove("class");
                    ddothers1.Attributes.Add("class", "col-md-2");
                }
                if (num == 2)
                {
                    others2.Visible = change;
                    //ddothers1.Style.Remove("col-md-4");
                    ddothers2.Attributes.Remove("class");
                    ddothers2.Attributes.Add("class", "col-md-2");

                }
                if (num == 3)
                {
                    others3.Visible = change;
                    //ddothers3.Style.Remove("col-md-4");
                    ddothers3.Attributes.Remove("class");
                    ddothers3.Attributes.Add("class", "col-md-2");
                    //RequiredFieldValidator61.Enabled = change;
                    RequiredFieldValidator63.Enabled = change;
                    RequiredFieldValidator65.Enabled = change;
                }
                if (num == 4)
                {
                    others4.Visible = change;
                    //ddothers4.Style.Remove("col-md-4");
                    ddothers4.Attributes.Remove("class");
                    ddothers4.Attributes.Add("class", "col-md-2");
                    //RequiredFieldValidator62.Enabled = change;
                    RequiredFieldValidator64.Enabled = change;
                    RequiredFieldValidator66.Enabled = change;
                }
            }
            else
            {
                if (num == 1)
                {
                    others1.Visible = change;
                    //ddothers1.Style.Remove("col-md-4");
                    ddothers1.Attributes.Remove("class");
                    ddothers1.Attributes.Add("class", "col-md-4");
                }
                if (num == 2)
                {
                    others2.Visible = change;
                    //ddothers1.Style.Remove("col-md-4");
                    ddothers2.Attributes.Remove("class");
                    ddothers2.Attributes.Add("class", "col-md-4");
                }
                if (num == 3)
                {
                    others3.Visible = change;
                    //ddothers3.Style.Remove("col-md-4");
                    ddothers3.Attributes.Remove("class");
                    ddothers3.Attributes.Add("class", "col-md-4");
                    //RequiredFieldValidator61.Enabled = change;
                    RequiredFieldValidator63.Enabled = change;
                    RequiredFieldValidator65.Enabled = change;
                }
                if (num == 4)
                {
                    others4.Visible = change;
                    //ddothers4.Style.Remove("col-md-4");
                    ddothers4.Attributes.Remove("class");
                    ddothers4.Attributes.Add("class", "col-md-4");
                    //RequiredFieldValidator62.Enabled = change;
                    RequiredFieldValidator64.Enabled = change;
                    RequiredFieldValidator66.Enabled = change;
                }
            }
        }





        //Change Dropdown content

        void ChangeValidIDDd(DataTable dt, DataTable dt2, string dropdown)
        {
            string businesstype = ViewState["businesstype"].ToString();
            //if (businesstype == "SOLE PROPRIETOR")
            //{
            string prev;
            ViewState["ValidIdPrev"] = dt2;
            ViewState["ValidId"] = dt;
            if (dropdown != "ddValidID")
            {
                if (ddValidID.SelectedValue == "---Select Valid ID---")
                {
                    ddValidID.DataSource = dt;
                    ddValidID.DataBind();
                    ddValidID.SelectedValue = "---Select Valid ID---";
                }
                else if (ddValidID.SelectedValue == "OTH")
                {
                    ddValidID.DataSource = dt;
                    ddValidID.DataBind();
                    ddValidID.SelectedValue = "OTH";
                }
                else
                {
                    prev = ddValidID.SelectedValue;
                    ddValidID.SelectedValue = "---Select Valid ID---";
                    ddValidID.DataSource = dt;
                    ddValidID.DataBind();
                    if (!dt.Columns["Code"].ToString().Contains(prev))
                    {
                        ddValidID.Items.Add(new ListItem(dt2.Rows[0]["Name"].ToString(), dt2.Rows[0]["Code"].ToString()));
                    }
                    ddValidID.SelectedValue = prev;
                }
            }
            if (dropdown != "ddValidID2")
            {
                if (ddValidID2.SelectedValue == "---Select Valid ID---")
                {
                    ddValidID2.DataSource = dt;
                    ddValidID2.DataBind();
                    ddValidID2.SelectedValue = "---Select Valid ID---";
                }
                else if (ddValidID2.SelectedValue == "OTH")
                {
                    ddValidID2.DataSource = dt;
                    ddValidID2.DataBind();
                    ddValidID2.SelectedValue = "OTH";
                }
                else
                {
                    prev = ddValidID2.SelectedValue;
                    ddValidID2.SelectedValue = "---Select Valid ID---";
                    ddValidID2.DataSource = dt;
                    ddValidID2.DataBind();
                    if (!dt.Columns["Code"].ToString().Contains(prev))
                    {
                        ddValidID2.Items.Add(new ListItem(dt2.Rows[1]["Name"].ToString(), dt2.Rows[1]["Code"].ToString()));
                    }
                    ddValidID2.SelectedValue = prev;
                }
            }
            if (dropdown != "ddValidID3")
            {
                if (ddValidID3.SelectedValue == "---Select Valid ID---")
                {
                    ddValidID3.DataSource = dt;
                    ddValidID3.DataBind();
                    ddValidID3.SelectedValue = "---Select Valid ID---";
                }
                else if (ddValidID3.SelectedValue == "OTH")
                {
                    ddValidID3.DataSource = dt;
                    ddValidID3.DataBind();
                    ddValidID3.SelectedValue = "OTH";
                }
                else
                {
                    prev = ddValidID3.SelectedValue;
                    ddValidID3.SelectedValue = "---Select Valid ID---";
                    ddValidID3.DataSource = dt;
                    ddValidID3.DataBind();
                    if (!dt.Columns["Code"].ToString().Contains(prev))
                    {
                        ddValidID3.Items.Add(new ListItem(dt2.Rows[2]["Name"].ToString(), dt2.Rows[2]["Code"].ToString()));
                    }
                    ddValidID3.SelectedValue = prev;
                }
            }
            if (dropdown != "ddValidID4")
            {
                if (ddValidID4.SelectedValue == "---Select Valid ID---")
                {
                    ddValidID4.DataSource = dt;
                    ddValidID4.DataBind();
                    ddValidID4.SelectedValue = "---Select Valid ID---";
                }
                else if (ddValidID4.SelectedValue == "OTH")
                {
                    ddValidID4.DataSource = dt;
                    ddValidID4.DataBind();
                    ddValidID4.SelectedValue = "OTH";
                }
                else
                {
                    prev = ddValidID4.SelectedValue;
                    ddValidID4.SelectedValue = "---Select Valid ID---";
                    ddValidID4.DataSource = dt;
                    ddValidID4.DataBind();
                    if (!dt.Columns["Code"].ToString().Contains(prev))
                    {
                        ddValidID4.Items.Add(new ListItem(dt2.Rows[3]["Name"].ToString(), dt2.Rows[3]["Code"].ToString()));
                    }
                    ddValidID4.SelectedValue = prev;
                }
            }

            //REMOVE VALID ID EXPIRY WHEN ID DOES NOT HAVE AN EXPIRY
            DataTable dt1 = (DataTable)ViewState["IDType"];
            DataRow dt3;
            string check = "";
            if (dropdown == "ddValidID")
            {
                dt3 = (DataRow)dt1.Select($"Name = '{ddValidID.SelectedItem.Text.ToUpper()}'").FirstOrDefault();
                if (dt3 != null)
                {
                    check = dt3.ItemArray[2].ToString();
                    if (check != "Y")
                    {
                        txtIDExpirationDate.Visible = false;
                        CustomValidator10.Enabled = false;
                        RequiredFieldValidator55.Enabled = false;
                        lblId1.Visible = false;
                    }
                    else
                    {
                        txtIDExpirationDate.Visible = true;
                        RequiredFieldValidator55.Enabled = true;
                        CustomValidator10.Enabled = true;
                        lblId1.Visible = true;
                    }
                }
                else
                {
                    txtIDExpirationDate.Visible = false;
                    CustomValidator10.Enabled = false;
                    RequiredFieldValidator55.Enabled = false;
                    lblId1.Visible = false;
                }
            }
            if (dropdown == "ddValidID2")
            {
                dt3 = (DataRow)dt1.Select($"Name = '{ddValidID2.SelectedItem.Text.ToUpper()}'").FirstOrDefault();
                if (dt3 != null)
                {
                    check = dt3.ItemArray[2].ToString();
                    if (check != "Y")
                    {
                        ///COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                        txtIDExpirationDate2.Visible = false;
                        //RequiredFieldValidator58.Enabled = false;
                        //CustomValidator18.Enabled = false;
                        lblId2.Visible = false;
                    }
                    else
                    {
                        ///COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                        txtIDExpirationDate2.Visible = true;
                        //RequiredFieldValidator58.Enabled = true;
                        //CustomValidator18.Enabled = true;
                        lblId2.Visible = true;
                    }
                }
                else
                {
                    ///COMMENTED 04 - 04 - 2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                    txtIDExpirationDate2.Visible = false;
                    //RequiredFieldValidator58.Enabled = false;
                    //CustomValidator18.Enabled = false;
                    lblId2.Visible = false;
                }
            }
            if (dropdown == "ddValidID3")
            {
                dt3 = (DataRow)dt1.Select($"Name = '{ddValidID3.SelectedItem.Text.ToUpper()}'").FirstOrDefault();
                if (dt3 != null)
                {
                    check = dt3.ItemArray[2].ToString();
                    if (check != "Y")
                    {
                        txtIDExpirationDate3.Visible = false;
                        RequiredFieldValidator65.Enabled = false;
                        CustomValidator19.Enabled = false;
                        lblId3.Visible = false;
                    }
                    else
                    {
                        txtIDExpirationDate3.Visible = true;
                        RequiredFieldValidator65.Enabled = true;
                        CustomValidator19.Enabled = true;
                        lblId3.Visible = true;
                    }
                }
                else
                {
                    txtIDExpirationDate3.Visible = false;
                    RequiredFieldValidator65.Enabled = false;
                    CustomValidator19.Enabled = false;
                    lblId3.Visible = false;
                }
            }
            if (dropdown == "ddValidID4")
            {
                dt3 = (DataRow)dt1.Select($"Name = '{ddValidID4.SelectedItem.Text.ToUpper()}'").FirstOrDefault();
                if (dt3 != null)
                {
                    check = dt3.ItemArray[2].ToString();
                    if (check != "Y")
                    {
                        txtIDExpirationDate4.Visible = false;
                        RequiredFieldValidator66.Enabled = false;
                        CustomValidator20.Enabled = false;
                        lblId4.Visible = false;
                    }
                    else
                    {
                        txtIDExpirationDate4.Visible = true;
                        RequiredFieldValidator66.Enabled = true;
                        CustomValidator20.Enabled = true;
                        lblId4.Visible = true;
                    }
                }
                else
                {
                    txtIDExpirationDate4.Visible = false;
                    RequiredFieldValidator66.Enabled = false;
                    CustomValidator20.Enabled = false;
                    lblId4.Visible = false;
                }
            }
            //}
        }

        void ValidIdDropDownEdit(DataTable dt, DataTable dt2, DataTable dtBrokerHeader, int num)
        {
            //Prevent other Valid ID Dropdown to choose same ID
            if (num == 1)
            {
                if (ddValidID.SelectedValue != "---Select Valid ID---")
                {
                    dt2.Rows[0]["Code"] = ddValidID.SelectedValue;
                    dt2.Rows[0]["Name"] = ddValidID.SelectedItem;
                    if (ddValidID.SelectedValue != "OTH")
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            if (row[0].ToString() == ddValidID.SelectedValue && row[0].ToString() != "OTH" && row[0].ToString() != "---Select Valid ID---")
                            {
                                dt.Rows.Remove(row);
                                break;
                            }
                        }
                    }
                    else if (ddValidID.SelectedValue == "OTH")
                    {
                        txtOthers1.Text = DataAccess.GetData(dtBrokerHeader, 0, "IDOthers", "").ToString();
                        ChangeOthers(true, 1);
                    }
                    else
                    {
                        txtOthers1.Text = DataAccess.GetData(dtBrokerHeader, 0, "IDOthers", "").ToString();
                    }
                    ChangeValidIDDd(dt, dt2, "ddValidID");
                }
                else
                {
                    txtOthers1.Text = "";
                    txtIDNo.Text = "";
                    txtIDExpirationDate.Text = null;
                }
            }
            else if (num == 2)
            {
                if (ddValidID2.SelectedValue != "---Select Valid ID---")
                {
                    dt2.Rows[1]["Code"] = ddValidID2.SelectedValue;
                    dt2.Rows[1]["Name"] = ddValidID2.SelectedItem;
                    if (ddValidID2.SelectedValue != "OTH")
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            if (row[0].ToString() == ddValidID2.SelectedValue && row[0].ToString() != "OTH" && row[0].ToString() != "---Select Valid ID---")
                            {
                                dt.Rows.Remove(row);
                                break;
                            }
                        }
                    }
                    else if (ddValidID2.SelectedValue == "OTH")
                    {
                        txtOthers2.Text = DataAccess.GetData(dtBrokerHeader, 0, "IDOthers2", "").ToString();
                        ChangeOthers(true, 2);
                    }
                    else
                    {
                        txtOthers2.Text = DataAccess.GetData(dtBrokerHeader, 0, "IDOthers2", "").ToString();
                    }
                    ChangeValidIDDd(dt, dt2, "ddValidID2");
                }
                else
                {
                    txtOthers2.Text = "";
                    txtIDNo2.Text = "";
                    txtIDExpirationDate2.Text = null;
                }
            }
            else if (num == 3)
            {
                if (ddValidID3.SelectedValue != "---Select Valid ID---")
                {
                    dt2.Rows[2]["Code"] = ddValidID3.SelectedValue;
                    dt2.Rows[2]["Name"] = ddValidID3.SelectedItem;
                    if (ddValidID3.SelectedValue != "OTH")
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            string test = row[0].ToString();
                            if (row[0].ToString() == ddValidID3.SelectedValue && row[0].ToString() != "OTH" && row[0].ToString() != "---Select Valid ID---")
                            {
                                dt.Rows.Remove(row);
                                break;
                            }
                        }
                    }
                    else if (ddValidID3.SelectedValue == "OTH")
                    {
                        txtOthers3.Text = DataAccess.GetData(dtBrokerHeader, 0, "IDOthers3", "").ToString();
                        ChangeOthers(true, 3);
                    }
                    else
                    {
                        txtOthers3.Text = DataAccess.GetData(dtBrokerHeader, 0, "IDOthers3", "").ToString();
                    }
                    ChangeValidIDDd(dt, dt2, "ddValidID3");
                }
                else
                {
                    txtOthers3.Text = "";
                    txtIDNo3.Text = "";
                    txtIDExpirationDate3.Text = null;
                }
            }
            else if (num == 4)
            {
                if (ddValidID4.SelectedValue != "---Select Valid ID---")
                {
                    dt2.Rows[3]["Code"] = ddValidID4.SelectedValue;
                    dt2.Rows[3]["Name"] = ddValidID4.SelectedItem;
                    if (ddValidID4.SelectedValue != "OTH")
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            if (row[0].ToString() == ddValidID4.SelectedValue && row[0].ToString() != "OTH" && row[0].ToString() != "---Select Valid ID---")
                            {
                                dt.Rows.Remove(row);
                                break;
                            }
                        }
                    }
                    else if (ddValidID4.SelectedValue == "OTH")
                    {
                        txtOthers4.Text = DataAccess.GetData(dtBrokerHeader, 0, "IDOthers4", "").ToString();
                        ChangeOthers(true, 4);
                    }
                    else
                    {
                        txtOthers4.Text = DataAccess.GetData(dtBrokerHeader, 0, "IDOthers4", "").ToString();
                    }
                    ChangeValidIDDd(dt, dt2, "ddValidID4");
                }
                else
                {
                    txtOthers4.Text = "";
                    txtIDNo4.Text = "";
                    txtIDExpirationDate4.Text = null;
                }
            }
        }
        #endregion

        protected void ddCivilStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeCivilStatus();

            string status = ddCivilStatus.SelectedValue.ToUpper();
            switch (status)
            {
                case "MARRIED":
                    txtSpouse.Focus();
                    break;
                case "SINGLE":
                    txtATPExpiryDate.Focus();
                    break;
                default:
                    ddCivilStatus.Focus();
                    break;
            }
        }

        void ChangeCivilStatus()
        {
            string status = ddCivilStatus.SelectedValue;
            //SPOUSE REQUIRED FIELD WHEN STATUS IS MARRIED
            //if (status == "Married")
            if (status == "MARRIED" || status == "WIDOWED")
            {
                RequiredFieldValidator50.Enabled = true;
                txtSpouse.Visible = true;
                //txtSSS.Visible = true;
                //RequiredFieldValidator17.Enabled = true;
                //txtPassport.Visible = true;
                //rfvPass.Enabled = true;
                //txtPassportValid.Visible = true;
                //RequiredFieldValidator41.Enabled = true;
                //CustomValidator4.Enabled = true;
                //txtPassportValidTo.Visible = true;
                //RequiredFieldValidator45.Enabled = true;
                //CustomValidator9.Enabled = true;
                //dpIssuedBy.Visible = true;
                //txtPlaceIssued.Visible = true;
                //rfvPlaceIssued.Visible = true;
                civilstat1.Visible = true;
                civilstat2.Visible = true;
                //civilstat3.Visible = true;
            }
            //HIDE SPOUSE, SSS NO, PASSPORT, PASSPORT VALID FROM, PASSPORT VALID TO, ISSUED BY AND PALCED ISSUED
            //WHEN STATUS IS SINGLE
            else //if (status == "Single")
            {
                txtSpouse.Visible = false;
                RequiredFieldValidator50.Enabled = false;
                //txtSSS.Visible = false;
                //RequiredFieldValidator17.Enabled = false;
                //txtPassport.Visible = false;
                //rfvPass.Enabled = false;
                //txtPassportValid.Visible = false;
                //RequiredFieldValidator41.Enabled = false;
                //CustomValidator4.Enabled = false;
                //txtPassportValidTo.Visible = false;
                //RequiredFieldValidator45.Enabled = false;
                //CustomValidator9.Enabled = false;
                //dpIssuedBy.Visible = false;
                //txtPlaceIssued.Visible = false;
                //rfvPlaceIssued.Visible = false;
                civilstat1.Visible = false;
                civilstat2.Visible = false;
                //civilstat3.Visible = false;

            }
            //else
            //{
            //    txtSpouse.Visible = true;
            //    //RequiredFieldValidator50.Enabled = false;
            //    //txtSSS.Visible = true;
            //    //RequiredFieldValidator17.Enabled = true;
            //    //txtPassport.Visible = true;
            //    //rfvPass.Enabled = true;
            //    //txtPassportValid.Visible = true;
            //    //RequiredFieldValidator41.Enabled = true;
            //    //CustomValidator4.Enabled = true;
            //    //txtPassportValidTo.Visible = true;
            //    //RequiredFieldValidator45.Enabled = true;
            //    //CustomValidator9.Enabled = true;
            //    //dpIssuedBy.Visible = true;
            //    //txtPlaceIssued.Visible = true;
            //    //rfvPlaceIssued.Visible = true;
            //    civilstat1.Visible = true;
            //    civilstat2.Visible = true;
            //    //civilstat3.Visible = true;
            //}

            //if (rbTypeOfBusiness.SelectedIndex == 0 && ViewState["Status"].ToString() == "ACCEPTED")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowSupplementaryDetails();", true);
            //}
            //else 
            if (rbTypeOfBusiness.SelectedIndex == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideContactDetailsWithHide();", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowSupplementaryDetails();", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideSupplementaryDetails();", true);
            }
        }

        protected void bSalesSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                if (!string.IsNullOrEmpty(txtSalesSearch.Value))
                {
                    dt = hana.GetData($"CALL sp_OSLASearch ('{txtSalesSearch.Value}','{lblBrokerID.Text}');", hana.GetConnection("SAOHana"));

                    // Create a new DataColumn with String datatype and the desired column name
                    DataColumn newColumn = new DataColumn("PRCLicenseExpirationDateString", typeof(string));
                    DataColumn newColumn2 = new DataColumn("ATPDateSalesPersonString", typeof(string));

                    // Add the new column to the DataTable
                    dt.Columns.Add(newColumn);
                    dt.Columns.Add(newColumn2);

                    // Iterate through the rows and convert the DateTime value to string
                    foreach (DataRow dr in dt.Rows)
                    {
                        // Get the original DateTime value from the row
                        object prcLicenseExpirationDateObj = dr["PRCLicenseExpirationDate"];
                        object atpdatesalespersonObj = dr["ATPDateSalesPerson"];

                        if (prcLicenseExpirationDateObj != DBNull.Value)
                        {
                            DateTime prcLicenseExpirationDate = (DateTime)prcLicenseExpirationDateObj;

                            // Convert the DateTime value to string in the desired format
                            string prcLicenseExpDateStr = prcLicenseExpirationDate == DateTime.Parse("1900-01-01")
                                ? " "
                                : prcLicenseExpirationDate.ToString("yyyy-MM-dd");

                            // Set the converted string value to the new column in the row
                            dr["PRCLicenseExpirationDateString"] = prcLicenseExpDateStr;
                        }
                        else
                        {
                            // Handle DBNull value if needed
                            dr["PRCLicenseExpirationDateString"] = "";
                        }

                        if (atpdatesalespersonObj != DBNull.Value)
                        {
                            DateTime atpdatesalesperson = (DateTime)atpdatesalespersonObj;

                            // Convert the DateTime value to string in the desired format
                            string atpdatesalespersonStr = atpdatesalesperson == DateTime.Parse("1900-01-01")
                                ? " "
                                : atpdatesalesperson.ToString("yyyy-MM-dd");

                            // Set the converted string value to the new column in the row
                            dr["ATPDateSalesPersonString"] = atpdatesalespersonStr;
                        }
                        else
                        {
                            // Handle DBNull value if needed
                            dr["ATPDateSalesPersonString"] = "";
                        }
                    }

                    // Remove the original DateTime column
                    dt.Columns.Remove("PRCLicenseExpirationDate");
                    dt.Columns.Remove("ATPDateSalesPerson");

                    // Rename the new column to the original column name if desired
                    newColumn.ColumnName = "PRCLicenseExpirationDate";
                    newColumn2.ColumnName = "ATPDateSalesPerson";
                }
                else
                {
                    string qry = $@"SELECT ""Id"",
                                    ""SalesPerson"",
                                    ""Position"",
                                    ""PRCLicense"",
                                    TO_VARCHAR(TO_DATE(""PRCLicenseExpirationDate""), 'YYYY-MM-DD') ""PRCLicenseExpirationDate"",
                                    TO_VARCHAR(TO_DATE(""ATPDateSalesPerson""), 'YYYY-MM-DD') ""ATPDateSalesPerson""
                                FROM ""OSLA""
                                WHERE ""CreateBrokerID"" = '{lblBrokerID.Text}' ";
                    //if (id == "btnListOfSalesPerson")
                    //{
                    dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
                    foreach (DataRow dtrows in dt.Rows)
                    {
                        var prclicenseexpdate = dtrows["PRCLicenseExpirationDate"].ToString();
                        if (prclicenseexpdate == "1900-01-01" || prclicenseexpdate == "1900/01/01")
                        {
                            dtrows["PRCLicenseExpirationDate"] = " ";
                        }

                        if (dtrows["ATPDateSalesPerson"].ToString() == "1900-01-01" || dtrows["ATPDateSalesPerson"].ToString() == "1900/01/01")
                        {
                            dtrows["ATPDateSalesPerson"] = " ";
                        }
                    }
                    //dt = hana.GetData(qry, hana.GetConnection("SAOHana"));

                }

                if (ViewState["SalesorShare"].ToString() == "Share")
                {
                    DataTable dt2 = new DataTable();
                    dt2.Columns.AddRange(new DataColumn[6]
                        {
                        new DataColumn("Id"),
                        new DataColumn("SalesPerson"),
                        new DataColumn("Position"),
                        new DataColumn("PRCLicense"),
                        new DataColumn("PRCLicenseExpirationDate"),
                        new DataColumn("ATPDateSalesPerson")
                        });
                    DataTable dtSalesPerson = (DataTable)ViewState["SalesPerson"];
                    foreach (DataRow row in dtSalesPerson.Rows)
                    {
                        if (row.RowState != DataRowState.Deleted)
                        {
                            foreach (DataRow row1 in dt.Rows)
                            {
                                if (row1["Id"].ToString() == row["Id"].ToString())
                                {
                                    dt2.Rows.Add(row1["Id"],
                                        row1["SalesPerson"],
                                        row1["Position"],
                                        row1["PRCLicense"],
                                        row1["PRCLicenseExpirationDate"],
                                        row1["ATPDateSalesPerson"]
                                        );
                                    break;
                                }
                            }
                        }
                    }
                    dt2.DefaultView.Sort = "Id ASC";
                    dt2 = dt2.DefaultView.ToTable();
                    gvSalesPersons.DataSource = dt2;
                }
                else
                {
                    dt.DefaultView.Sort = "Id ASC";
                    dt = dt.DefaultView.ToTable();
                    gvSalesPersons.DataSource = dt;
                }
                gvSalesPersons.DataBind();
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }

        }

        //AUTOMATE When Type of Business = Corporation or Partnership
        protected void txtPartnership_TextChanged(object sender, EventArgs e)
        {
            if (ViewState["businesstype"].ToString() != "SOLE PROPRIETOR")
            {
                txtBusinessName.Text = txtPartnership.Text;
                txtRealtyName.Text = txtPartnership.Text;
            }
            if (rbTypeOfBusiness.SelectedIndex == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideContactDetailsWithHide();", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowSupplementaryDetails();", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideSupplementaryDetails();", true);
            }
            txtSECRegNo.Focus();
        }

        protected void txtAddress_TextChanged(object sender, EventArgs e)
        {
            if (ViewState["businesstype"].ToString().ToUpper() != "SOLE PROPRIETOR")
            {
                txtBusinessAddress.Text = txtAddress.Text;
            }
            if (rbTypeOfBusiness.SelectedIndex == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideContactDetailsWithHide();", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowSupplementaryDetails();", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideSupplementaryDetails();", true);
            }
            txtCity.Focus();
        }

        protected void txtZipCode_TextChanged(object sender, EventArgs e)
        {
            if (ViewState["businesstype"].ToString().ToUpper() != "SOLE PROPRIETOR")
            {
                txtBusinessZipCode.Text = txtZipCode.Text;
            }
            if (rbTypeOfBusiness.SelectedIndex == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideContactDetailsWithHide();", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowSupplementaryDetails();", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideSupplementaryDetails();", true);
            }
            txtResidenceNo.Focus();
        }

        protected void btnAddSharingHouseLot_Click(object sender, EventArgs e)
        {
            try
            {
                //GAB 07/3/2023
                mtxtPecent.Text = "";
                //GAB 5/15/2023
                string pouchDbData = txtNext3Hidden.Text;

                var pouchdata = JsonConvert.DeserializeObject<List<SharingDetails>>(pouchDbData);
                ViewState["SalesorShare"] = "Share";

                GridViewRow gridViewRow = (GridViewRow)(sender as Control).Parent.Parent;
                ViewState["SharingID"] = gridViewRow.Cells[0].Text;
                DataTable dt = (DataTable)ViewState["SharingDetails"];
                string salesPerson = gridViewRow.Cells[0].Text;

                // Filter the rows where HouseandLotPercentage is not equal to 0
                //DataRow[] filteredRows = dt.Select($"HouseandLotPercentage <> '0' AND SalesPersonID = '{salesPerson}'");
                DataRow[] filteredRows = dt.Select($"LotOnlyPercentage = '0' AND SalesPersonID = '{salesPerson}'");

                // Create a new DataTable to store the filtered rows
                DataTable filteredDataTable = dt.Clone();
                foreach (DataRow row in filteredRows)
                {
                    filteredDataTable.ImportRow(row);
                }

                //DataTable tempTB = dt.Select($"SalesPersonId = {ViewState["SharingID"]}").CopyToDataTable();
                //dt.Merge(tempTB);
                //ViewState["SharingDetails"] = dt;
                gvShareDetails.Columns[5].ItemStyle.CssClass = "";
                gvShareDetails.Columns[4].ItemStyle.CssClass = "hidden";
                gvShareDetails.Columns[5].HeaderStyle.CssClass = "";
                gvShareDetails.Columns[4].HeaderStyle.CssClass = "hidden";
                ViewState["isLot"] = false;
                ViewState["Type"] = "HouseNLot";
                string type = ViewState["Type"].ToString();
                string brokerStat = ViewState["Status"].ToString();

                if (ViewState["Status"].ToString() != "ACCEPTED")
                {
                    gvShareDetails.Columns[8].ItemStyle.CssClass = "hidden";
                    gvShareDetails.Columns[8].HeaderStyle.CssClass = "hidden";
                }
                else
                {
                    gvShareDetails.Columns[8].ItemStyle.CssClass = "";
                    gvShareDetails.Columns[8].HeaderStyle.CssClass = "";
                }

                if (filteredDataTable.Rows.Count > 0)
                {
                    BindGrid2(ViewState["SharingID"].ToString());
                }
                else
                {
                    gvShareDetails.DataSource = null;
                    gvShareDetails.DataBind();
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "ShowAddSharingModal();", true);
                BindGridSharingDetailsFromPouchDb(pouchdata);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ComparingTables()", "ComparingTables();", true);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "ChangeCommPercent()", "ChangeCommPercent()", true);
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }
        }
        protected void txtAuthorizedRepresentative2_TextChanged(object sender, EventArgs e)
        {
            txtAuthorizedRepresentative.Text = txtAuthorizedRepresentative2.Text;
            if (rbTypeOfBusiness.SelectedIndex == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideContactDetailsWithHide();", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowSupplementaryDetails();", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideSupplementaryDetails();", true);
            }
        }
        protected void btnCloseSharingDetails_ServerClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideAddSharingModal();", true);
        }

        protected void txtCorpReligion_TextChanged(object sender, EventArgs e)
        {
            txtReligion.Text = txtCorpReligion.Text;
            if (ViewState["businesstype"].ToString().ToUpper() == "SOLE PROPRIETOR")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideContactDetailsWithHide();", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowSupplementaryDetails();", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideSupplementaryDetails();", true);
            }
        }

        protected void txtCorpCitizenship_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCitizenship.SelectedIndex = txtCorpCitizenship.SelectedIndex;
            if (ViewState["businesstype"].ToString().ToUpper() == "SOLE PROPRIETOR")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideContactDetailsWithHide();", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowSupplementaryDetails();", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideSupplementaryDetails();", true);
            }
        }

        protected void ddContactValidID_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeContactValidID();
            if (ViewState["businesstype"].ToString().ToUpper() == "SOLE PROPRIETOR")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideContactDetailsWithHide();", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowSupplementaryDetails();", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideSupplementaryDetails();", true);
            }
        }

        void ChangeContactValidID()
        {
            if (ddContactValidID.SelectedValue == "OTH")
            {
                OthersContactInfo.Visible = true;

                ValidIDContactInfo.Attributes.Remove("class");
                ValidIDContactInfo.Attributes.Add("class", "col-md-3");

                //RequiredFieldValidator14.Enabled = true;
            }
            else
            {
                OthersContactInfo.Visible = false;

                ValidIDContactInfo.Attributes.Remove("class");
                ValidIDContactInfo.Attributes.Add("class", "col-md-6");

                //RequiredFieldValidator14.Enabled = false;
            }
        }

        protected void bUpdatesalesPersons_Command(object sender, CommandEventArgs e)
        {
            try
            {
                string id = e.CommandArgument.ToString();

                ViewState["SalesPersonId"] = id;

                string qry = $@"SELECT * FROM ""OSLA"" WHERE ""Id"" = '{id}'";
                DataTable dtOSLA = hana.GetData(qry, hana.GetConnection("SAOHana"));

                mtxtRegisterSalesPersonName.Text = DataAccess.GetData(dtOSLA, 0, "SalesPerson", "").ToString();
                mtxtEmail.Text = DataAccess.GetData(dtOSLA, 0, "EmailAddress", "").ToString().Trim();
                ddPosition.SelectedValue = ddPosition.Items.FindByText(DataAccess.GetData(dtOSLA, 0, "Position", "Sales Agent").ToString()).Value;
                //ddPosition.SelectedValue = DataAccess.GetData(dtOSLA, 0, "Position", "Sales Agent").ToString();
                //ddlVATCode2.SelectedValue = DataAccess.GetData(dtOSLA, 0, "VATCode", "0").ToString();
                string selectedValue = DataAccess.GetData(dtOSLA, 0, "VATCode", "-1").ToString();
                ddlVATCode2.SelectedValue = ddlVATCode2.Items.FindByValue(selectedValue)?.Value ?? "0";
                ddlWTAXCode2.SelectedValue = DataAccess.GetData(dtOSLA, 0, "WTaxCode", "0").ToString();
                txtSalesHLURBNo.Text = DataAccess.GetData(dtOSLA, 0, "HLURBLicenseNo", "").ToString();
                mtxtMobile.Text = DataAccess.GetData(dtOSLA, 0, "MobileNumber", "").ToString();
                mtxtTIN.Text = DataAccess.GetData(dtOSLA, 0, "TIN", "").ToString();
                mtxtPRCLicense.Text = DataAccess.GetData(dtOSLA, 0, "PRCLicense", "").ToString();
                mtxtPRCLicenseExpirationDate.Text = Convert.ToDateTime(DataAccess.GetData(dtOSLA, 0, "PRCLicenseExpirationDate", "1900-01-01")).ToString("yyyy-MM-dd");
                mtxtATPDate.Text = Convert.ToDateTime(DataAccess.GetData(dtOSLA, 0, "ATPDateSalesPerson", "1900-01-01")).ToString("yyyy-MM-dd");
                txtSalesPTRNo.Text = DataAccess.GetData(dtOSLA, 0, "PTRNo", "").ToString();

                Label1.Text = "Update";

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowRegisterSalesAgentModal();", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideAddSharingDetailsModal();", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideAddSharingModal();", true);
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }

        }

        protected void btnSubmitDocument_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
                if (ViewState["Status"].ToString() == "ACCEPTED")
                {
                    UpdateBrokerInformation();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ModalApprovalType_Show();", true);
                }
        }

        protected void txtTIN_TextChanged(object sender, EventArgs e)
        {
            if (ddValidID.SelectedValue == "ID1")
            {
                txtIDNo.Text = txtTIN.Text;
            }
            else if (ddValidID2.SelectedValue == "ID1")
            {
                txtIDNo2.Text = txtTIN.Text;
            }
            else if (ddValidID3.SelectedValue == "ID1")
            {
                txtIDNo3.Text = txtTIN.Text;
            }
            else if (ddValidID4.SelectedValue == "ID1")
            {
                txtIDNo4.Text = txtTIN.Text;
            }
            if (ViewState["businesstype"].ToString().ToUpper() == "SOLE PROPRIETOR")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideContactDetailsWithHide();", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowSupplementaryDetails();", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideSupplementaryDetails();", true);
            }
            txtNatureOfBusiness.Focus();
        }

        protected void txtIDNo_TextChanged(object sender, EventArgs e)
        {
            if (ddValidID.SelectedValue == "ID1")
            {
                txtTIN.Text = txtIDNo.Text;
            }
            if (ViewState["businesstype"].ToString().ToUpper() == "SOLE PROPRIETOR")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideContactDetailsWithHide();", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowSupplementaryDetails();", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideSupplementaryDetails();", true);
            }

            string id = txtOthers1.Text.ToLower().Trim();
            string selected = ddValidID.SelectedValue.ToLower();
            if (id.Contains("prc") && selected == "oth")
                txtPRCRegis.Text = txtIDNo.Text;
        }

        protected void txtIDNo2_TextChanged(object sender, EventArgs e)
        {
            if (ddValidID2.SelectedValue == "ID1")
            {
                txtTIN.Text = txtIDNo2.Text;
            }
            if (ViewState["businesstype"].ToString().ToUpper() == "SOLE PROPRIETOR")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideContactDetailsWithHide();", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowSupplementaryDetails();", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideSupplementaryDetails();", true);
            }

            string id2 = txtOthers2.Text.ToLower().Trim();
            string selected2 = ddValidID2.SelectedValue.ToLower();

            if (id2.Contains("prc") && selected2 == "oth")
                txtPRCRegis.Text = txtIDNo2.Text;

            ddlVATCode.Focus();
        }

        protected void txtIDNo3_TextChanged(object sender, EventArgs e)
        {
            if (ddValidID3.SelectedValue == "ID1")
            {
                txtTIN.Text = txtIDNo3.Text;
            }
            if (ViewState["businesstype"].ToString().ToUpper() == "SOLE PROPRIETOR")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideContactDetailsWithHide();", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowSupplementaryDetails();", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideSupplementaryDetails();", true);
            }
        }

        protected void txtIDNo4_TextChanged(object sender, EventArgs e)
        {
            if (ddValidID4.SelectedValue == "ID1")
            {
                txtTIN.Text = txtIDNo4.Text;
            }
            if (ViewState["businesstype"].ToString().ToUpper() == "SOLE PROPRIETOR")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideContactDetailsWithHide();", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowSupplementaryDetails();", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideSupplementaryDetails();", true);
            }
        }

        protected void txtBusinessName_TextChanged(object sender, EventArgs e)
        {
            txtRealtyName.Text = txtBusinessName.Text;
            txtTradeName.Text = txtBusinessName.Text;
            if (ViewState["businesstype"].ToString().ToUpper() == "SOLE PROPRIETOR")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideContactDetailsWithHide();", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowSupplementaryDetails();", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideSupplementaryDetails();", true);
            }
        }
        protected void txtOthers_TextChanged(object sender, EventArgs e)
        {
            if (ViewState["businesstype"].ToString().ToUpper() == "SOLE PROPRIETOR")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideContactDetailsWithHide();", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowSupplementaryDetails();", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideSupplementaryDetails();", true);
            }

            PRCOthers();
        }

        void PRCOthers()
        {
            string id = txtOthers1.Text.ToLower().Trim();
            string id2 = txtOthers2.Text.ToLower().Trim();
            string selected = ddValidID.SelectedValue.ToLower();
            string selected2 = ddValidID2.SelectedValue.ToLower();
            if (id.Contains("prc") && selected == "oth")
            {
                txtIDNo.Attributes.Add("MaxLength", "7");
                txtIDNo.Text = txtPRCRegis.Text;
                //txtPRCRegis.Text = txtIDNo.Text;
            }
            else
            {
                txtIDNo.Attributes.Remove("MaxLength");
            }
            //else
            //    txtIDNo.Text = "";

            if (id2.Contains("prc") && selected2 == "oth")
            {
                txtIDNo2.Attributes.Add("MaxLength", "7");
                txtIDNo2.Text = txtPRCRegis.Text;
                //txtPRCRegis.Text = txtIDNo2.Text;
            }
            else
            {
                txtIDNo2.Attributes.Remove("MaxLength");
            }
        }

        protected void txtPRCRegis_TextChanged(object sender, EventArgs e)
        {
            if (ViewState["businesstype"].ToString().ToUpper() == "SOLE PROPRIETOR")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideContactDetailsWithHide();", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowSupplementaryDetails();", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideSupplementaryDetails();", true);
            }

            string id = txtOthers1.Text.ToLower().Trim();
            string id2 = txtOthers2.Text.ToLower().Trim();
            string selected = ddValidID.SelectedValue.ToLower();
            string selected2 = ddValidID2.SelectedValue.ToLower();
            if (id.Contains("prc") && selected == "oth")
                txtIDNo.Text = txtPRCRegis.Text;
            //else
            //    txtIDNo.Text = "";

            if (id2.Contains("prc") && selected2 == "oth")
                txtIDNo2.Text = txtPRCRegis.Text;
            //else
            //    txtIDNo2.Text = "";
            txtRegistrationDate.Focus();
        }
        protected void btnClose_ServerClick(object sender, EventArgs e)
        {
            //if(lblBrokerID.Text == "")
            //{
            //    Response.Redirect("~/pages/Dashboard.aspx");
            //}
        }

        protected void CustomValidator11_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (CBconforme.Checked == false)
            {
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }
        }

        void ConformeBlockers()
        {
            btnSubmitDocument.Visible = CBconforme.Checked;

        }

        protected void CBconforme_CheckedChanged(object sender, EventArgs e)
        {
            ConformeBlockers();
        }

        protected void btnCopyPrincipalAddress_Click(object sender, EventArgs e)
        {
            txtBusinessAddress.Text = (txtAddress.Text.ToUpper() == "N/A" ? "" : txtAddress.Text);
            txtBusinessZipCode.Text = (txtZipCode.Text.ToUpper() == "N/A" ? "" : txtZipCode.Text);

            //2023-11-16 : ADD CONTROL WHEN CLICKING COPYPRINCIPAL ADDRESS BUTTON
            if (ViewState["businesstype"].ToString() != "SOLE PROPRIETOR")
            {
                solefields.Attributes.Add("class", "hidden");
                solefields2.Attributes.Add("class", "hidden");
                civilstat1.Attributes.Add("class", "hidden");
            }


            txtBusinessAddress.Focus();
        }

        protected void btnSearchBroker_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "ModalBrokerList_Show();", true);
            loadBrokerList();
        }

        protected void txtContactPersonPosition_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
using ABROWN_DREAMS.wcf;
using DirecLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABROWN_DREAMS
{
    public partial class Buyers : Page
    {
        DirecWebService ws = new DirecWebService();
        SAPHanaAccess hana = new SAPHanaAccess();


        protected void Page_Load(object sender, EventArgs e)
        {
            Page.MaintainScrollPositionOnPostBack = true;

            if (Session["UserID"] == null)
            {
                alertMsg("Session expired!", "error");
                Response.Redirect("~/pages/Login.aspx");
            }

            if (Session["UserAccess"] == null)
            {
                alertMsg("Session expired!", "error");
                Response.Redirect("~/pages/Login.aspx");
            }

            if (this.IsPostBack)
            {
                ////STAY IN CURRENT TAB
                TabName.Value = Request.Form[TabName.UniqueID];
                ////ScriptManager.RegisterStartupScript(this, GetType(), "alert", $@"tabNav(event, '{TabName.Value}');", true);
                //ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);

            }
            if (!IsPostBack)
            {
                string code = Request.QueryString["code"];

                //REMOVE GUID ON LOADING
                ViewState["Guid"] = "";




                Session["CardCode"] = "";
                ViewState["BuyerLink"] = "";

                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);

                compDetails.Visible = false;
                spouseBtn.Visible = false;
                RequiredFieldValidator11.Enabled = false;
                RequiredFieldValidator9.Enabled = false;
                RequiredFieldValidator10.Enabled = false;
                contactDetails.Visible = false;
                coborrowerGrid.Visible = false;
                othersGrid.Visible = false;
                btnPrint.Visible = false;

                FirstLoad();
                divCustomerCode.Visible = false;
                visibleDocumentButtons(false, btnPreview, btnRemove);
                visibleDocumentButtons(false, btnPreview2, btnRemove2);

                DataTable dtcoowner = new DataTable();
                dtcoowner.Columns.AddRange(new DataColumn[5]
                        {
                        new DataColumn("CardCode"),
                        new DataColumn("FirstName"),
                        new DataColumn("MiddleName"),
                        new DataColumn("LastName"),
                        new DataColumn("SpecifiedBusinessType")
                        });
                ViewState["Coowner"] = dtcoowner;

                DataTable taxclass = new DataTable();
                taxclass.Columns.AddRange(new DataColumn[2]
                        {
                        new DataColumn("Code"),
                        new DataColumn("Name")
                        });
                ViewState["TaxClass"] = taxclass;
                otherFields();

                txtCertifyDate.Value = DateTime.Now.Date.ToString("yyyy-MM-dd");
                LoadBuyerDocumentsStandard();
                LoadBuyerDocumentsStandard_NotRequired();

                DeleteTemporaryFIles();

                if (!string.IsNullOrEmpty(code))
                {
                    loadbuyer(code);
                }

                //2023-06-15 : ADD VALIDATOR FOR SPOUSE TIN FORMAT--%>
                CustomValidator14.Enabled = true;
            }

            taxClassChanged();

            //Prevents Tab UI bug upon loading
            ScriptManager.RegisterStartupScript(this, GetType(), "hide", "EmpTab();", true);
            Page.Form.Attributes.Add("enctype", "multipart/form-data");

        }

        void FirstLoad()
        {
            lblSave.InnerText = "Save";
            SPAAddUpdate.InnerText = "Add";

            LoadGridView("dtCoOwner");
            LoadData(gvCoOwner, "dtCoOwner");


            LoadGridView("dtOthers");
            LoadData(gvOthers, "dtOthers");


            LoadGridView("dtDependent");
            LoadData(gvDependent, "dtDependent");
            //2023-06-18 : CHANGE TO ViewState
            LoadDataViewState(gvDependent, "dtDependent");


            LoadGridView("dtSPADependent");
            LoadData(gvSPADependent, "dtSPADependent");

            LoadGridView("dtBankAccount");
            LoadData(gvBankAccount, "dtBankAccount");
            //2023-06-18 : CHANGE TO ViewState
            LoadDataViewState(gvBankAccount, "dtBankAccount");

            LoadGridView("dtCharacterRef");
            LoadData(gvCharacterRef, "dtCharacterRef");
            //2023-06-18 : CHANGE TO ViewState
            LoadDataViewState(gvCharacterRef, "dtCharacterRef");

            //LoadGridView("dtListSPACoBorrower");
            //LoadGridView("dtSPACBDependent");

            //2023-06-18 : CHANGE TO ViewState
            LoadData(gvCharacterRef, "gvSPACoBorrower");
            LoadGridView("gvSPACoBorrower");
            LoadDataViewState(gvSPACoBorrower, "gvSPACoBorrower");


            Session["SPACoBorrowerCount"] = 1;

            if (Session["UserID"] == null)
            {
                Session["UserID"] = 0;
            }
            ws.InitializeSPA((int)Session["UserID"]);

            ClearDependent();
            ClearBankAccount();
            ClearCharacterRef();
            AutoCompute();

            //FOR SPOUSE EMPLOYMENT STATUS DROPDOWN LIST
            //<%-- 2023-06-14 : ADDED SELF EMPLOYED AS REQUESTED  --%>
            DataTable dt = hana.GetData($@"SELECT ""Code"", ""Name"" FROM ""OLST"" WHERE ""GrpCode"" = 'ES' ORDER BY ""Code""", hana.GetConnection("SAOHana"));
            dt.Rows.Add("", "");
            tSpouseEmpStatus.DataSource = dt;
            tSpouseEmpStatus.DataBind();
            tSpouseEmpStatus.SelectedValue = "";

            //FOR SPOUSE NATURE EMPLOYMENT STATUS DROPDOWN LIST
            dt = hana.GetData($@"SELECT ""Code"", ""Name"" FROM ""OLST"" WHERE ""GrpCode"" = 'NE' AND ""IsShow"" = True", hana.GetConnection("SAOHana"));
            dt.Rows.Add("", "");
            tSpouseNatureEmp.DataSource = dt;
            tSpouseNatureEmp.DataBind();
            tSpouseNatureEmp.SelectedValue = "";

            //SOURCE OF INCOME DOPDOWN LIST 08082022 -KARL
            dt = hana.GetData($@"SELECT ""Code"", ""Name"" FROM ""OLST"" WHERE ""GrpCode"" = 'SF' AND ""IsShow"" = True", hana.GetConnection("SAOHana"));
            dt.Rows.Add("", "");
            //dt.Rows.Add("---Select Source of Funds---", "---Select Source of Funds---");
            ddSourceFunds.DataSource = dt;
            ddSourceFunds.DataBind();
            ddSourceFunds.SelectedValue = "";

            //LOAD Valid ID's
            string qry1 = @"SELECT ""Code"", ""Name"" FROM OLST WHERE ""GrpCode"" = 'ID' ORDER BY ""Code""";
            dt = hana.GetData(qry1, hana.GetConnection("SAOHana"));
            dt.Rows.Add(
                "---Select Type of ID---",
                "---Select Type of ID---"
                );
            tTypeOfId.DataSource = dt;
            tTypeOfId.DataBind();
            tTypeOfId.SelectedValue = "---Select Type of ID---";
            tTypeOfId2.DataSource = dt;
            tTypeOfId2.DataBind();
            tTypeOfId2.SelectedValue = "---Select Type of ID---";
            tTypeOfId3.DataSource = dt;
            tTypeOfId3.DataBind();
            tTypeOfId3.SelectedValue = "---Select Type of ID---";
            tTypeOfId4.DataSource = dt;
            tTypeOfId4.DataBind();
            tTypeOfId4.SelectedValue = "---Select Type of ID---";
            ViewState["ValidId"] = null;
            ViewState["ValidId"] = dt;

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
                "---Select Type of ID---",
                "---Select Type of ID---"
                    );
            }
            ViewState["ValidIdPrev"] = null;
            ViewState["ValidIdPrev"] = dt4;
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

        void confirmation(string body, string type)
        {
            Session["ConfirmType"] = type;
            lblConfirmationInfo.Text = body;
            ScriptManager.RegisterStartupScript(this, GetType(), "confirmation", "showConfirmation();", true);
        }

        void LoadGridView(string session)
        {
            DataTable dt = new DataTable();
            if (session == "dtBankAccount")
            {
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add("Bank", typeof(string));
                dt.Columns.Add("Branch", typeof(string));
                dt.Columns.Add("AcctType", typeof(string));
                dt.Columns.Add("AcctNo", typeof(string));
                dt.Columns.Add("AvgDailyBal", typeof(double));
                dt.Columns.Add("PresBal", typeof(double));
            }
            else if (session == "dtCharacterRef")
            {
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Address", typeof(string));
                dt.Columns.Add("Email", typeof(string));
                dt.Columns.Add("TelNo", typeof(string));
            }
            else if (session == "dtDependent" || session == "dtSPADependent")
            {
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Age", typeof(int));
                dt.Columns.Add("Relationship", typeof(string));
            }
            else if (session == "gvSPACoBorrower")
            {

                //2023-06-18 : COMMENTED FOR NEW SPA PROCESS
                //dt.Columns.Add("ID", typeof(int));
                //dt.Columns.Add("Relationship", typeof(string));
                //dt.Columns.Add("Name", typeof(string));
                //dt.Columns.Add("Gender", typeof(string));
                //dt.Columns.Add("Email", typeof(string));
                //dt.Columns.Add("SPAFormDocument", typeof(string));

                //2023-06-18 : UPDATED FOR NEW SPA PROCESS
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add("Relationship", typeof(string));
                dt.Columns.Add("LastName", typeof(string));
                dt.Columns.Add("FirstName", typeof(string));
                dt.Columns.Add("MiddleName", typeof(string));
                dt.Columns.Add("CivilStatus", typeof(string));
                dt.Columns.Add("YearsOfStay", typeof(string));
                dt.Columns.Add("Address", typeof(string));
                dt.Columns.Add("BirthDate", typeof(string));
                dt.Columns.Add("BirthPlace", typeof(string));
                dt.Columns.Add("Gender", typeof(string));
                dt.Columns.Add("Citizenship", typeof(string));
                dt.Columns.Add("Email", typeof(string));
                dt.Columns.Add("TelNo", typeof(string));
                dt.Columns.Add("MobileNo", typeof(string));
                dt.Columns.Add("FB", typeof(string));
                dt.Columns.Add("SPAFormDocument", typeof(string));


            }
            else if (session == "dtCoOwner")
            {
                dt.Columns.Add("CardCode", typeof(string));
                //dt.Columns.Add("BuyerType", typeof(string));
                dt.Columns.Add("FirstName", typeof(string));
                dt.Columns.Add("MiddleName", typeof(string));
                dt.Columns.Add("LastName", typeof(string));
                dt.Columns.Add("SpecifiedBusinessType", typeof(string));
                //dt.Columns.Add("Relationship", typeof(string));
                //dt.Columns.Add("Email", typeof(string));
                //dt.Columns.Add("MobileNo", typeof(string));
                //dt.Columns.Add("Address", typeof(string));
                //dt.Columns.Add("Residence", typeof(string));
                //dt.Columns.Add("ValidID", typeof(string));
                //dt.Columns.Add("ValidIDNo", typeof(string));
            }
            else if (session == "dtOthers")
            {
                dt.Columns.Add("CardCode", typeof(int));
                dt.Columns.Add("BuyerType", typeof(string));
                dt.Columns.Add("FirstName", typeof(string));
                dt.Columns.Add("MiddleName", typeof(string));
                dt.Columns.Add("LastName", typeof(string));
                dt.Columns.Add("Relationship", typeof(string));
                dt.Columns.Add("Email", typeof(string));
                dt.Columns.Add("MobileNo", typeof(string));
                dt.Columns.Add("Address", typeof(string));
                dt.Columns.Add("Residence", typeof(string));
                dt.Columns.Add("ValidID", typeof(string));
                dt.Columns.Add("ValidIDNo", typeof(string));
            }
            //else if (session == "dtListSPACoBorrower")
            //{
            //    dt.Columns.Add("ID", typeof(int));
            //    dt.Columns.Add("SPA", typeof(bool));
            //    dt.Columns.Add("CoBorrower", typeof(bool));
            //    dt.Columns.Add("Relationship", typeof(string));

            //    dt.Columns.Add("LastName", typeof(string));
            //    dt.Columns.Add("FirstName", typeof(string));
            //    dt.Columns.Add("MiddleName", typeof(string));
            //    dt.Columns.Add("Gender", typeof(string));
            //    dt.Columns.Add("Citizenship", typeof(string));
            //    dt.Columns.Add("BirthDate", typeof(string));
            //    dt.Columns.Add("BirthPlace", typeof(string));
            //    dt.Columns.Add("CellNo", typeof(string));
            //    dt.Columns.Add("HomeTelNo", typeof(string));
            //    dt.Columns.Add("Email", typeof(string));
            //    dt.Columns.Add("FB", typeof(string));

            //    dt.Columns.Add("TIN", typeof(string));
            //    dt.Columns.Add("SSSNo", typeof(string));
            //    dt.Columns.Add("GSISNo", typeof(string));
            //    dt.Columns.Add("PagIbigNo", typeof(string));

            //    dt.Columns.Add("PresentAddress", typeof(string));
            //    dt.Columns.Add("PermanentAddress", typeof(string));
            //    dt.Columns.Add("Position", typeof(string));
            //    dt.Columns.Add("YearsOfService", typeof(int));
            //    dt.Columns.Add("OfficeTelNo", typeof(string));
            //    dt.Columns.Add("FaxNo", typeof(string));
            //    dt.Columns.Add("HomeOwnership", typeof(string));
            //    dt.Columns.Add("YearsOfStay", typeof(int));

            //    dt.Columns.Add("CTCNo", typeof(string));
            //    dt.Columns.Add("DateIssued", typeof(DateTime));
            //    dt.Columns.Add("EmpBusinessName", typeof(string));
            //    dt.Columns.Add("EmpBusinessAddress", typeof(string));

            //    dt.Columns.Add("EmploymentStatus", typeof(string));
            //    dt.Columns.Add("NatureOfEmp", typeof(string));
            //    dt.Columns.Add("CivilStatus", typeof(string));
            //}
            //else if (session == "dtSPACBDependent")
            //{
            //    dt.Columns.Add("ID", typeof(int));
            //    dt.Columns.Add("LineNum", typeof(int));
            //    dt.Columns.Add("Name", typeof(string));
            //    dt.Columns.Add("Age", typeof(int));
            //    dt.Columns.Add("Relationship", typeof(string));
            //}



            Session[session] = dt;

            //2023-06-18 : CHANGE TO VIEWSTATE
            ViewState[session] = dt;



            //2023-06-18 : CHANGE TO VIEWSTATE
            //if (session == "dtDependent")
            //{ LoadData(gvDependent, session); }
            //else if (session == "dtSPADependent")
            //{ LoadData(gvSPADependent, session); }
            //else if (session == "dtBankAccount")
            //{ LoadData(gvBankAccount, session); }
            //else if (session == "dtCharacterRef")
            //{ LoadData(gvCharacterRef, session); }
            //else if (session == "gvSPACoBorrower")
            //{ LoadData(gvSPACoBorrower, session); }

            if (session == "dtDependent")
            { LoadDataViewState(gvDependent, session); }
            else if (session == "dtSPADependent")
            { LoadDataViewState(gvSPADependent, session); }
            else if (session == "dtBankAccount")
            { LoadDataViewState(gvBankAccount, session); }
            else if (session == "dtCharacterRef")
            { LoadDataViewState(gvCharacterRef, session); }
            else if (session == "gvSPACoBorrower")
            { LoadDataViewState(gvSPACoBorrower, session); }




            else if (session == "dtCoOwner")
            { LoadData(gvCoOwner, session); }
            else if (session == "dtOthers")
            { LoadData(gvOthers, session); }
        }

        void LoadData(GridView gv, string session)
        {
            gv.DataSource = (DataTable)Session[session];
            gv.DataBind();
        }


        void LoadDataViewState(GridView gv, string viewstate)
        {
            gv.DataSource = (DataTable)ViewState[viewstate];
            gv.DataBind();
        }

        protected void gvDependent_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //2023-06-18 : CHANGE TO VIEWSTATE
            //LoadData(gvDependent, "dtDependent");
            LoadDataViewState(gvDependent, "dtDependent");

            gvDependent.PageIndex = e.NewPageIndex;
            gvDependent.DataBind();
        }


        protected void gvDelete_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            Control btn = (Control)sender;
            Session["Buyers_Control"] = btn.ID;
            Session["Buyers_ID"] = Convert.ToInt32(GetID.CommandArgument);
            confirmation("Are you sure you want to delete the selected dependent?", "DelDependent");
        }

        protected void btnAdd_ServerClick(object sender, EventArgs e)
        {
            //STAY IN CURRENT TAB
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);
            try
            {
                if (tDependentName.Value == "" || tDependentAge.Value == "" || tDependentRelationship.Value == "")
                { alertMsg("Please fill up all forms before adding!", "info"); }
                else
                {
                    DataTable dt = new DataTable();
                    string session = (string)Session["bDependent"];

                    if (session == "bDependent")
                    {
                        //2023-06-18 : CHANGE TO VIEWSTATE
                        //dt = (DataTable)Session["dtDependent"];
                        dt = (DataTable)ViewState["dtDependent"];

                        DataRow dr = dt.NewRow();

                        dr[0] = Convert.ToInt32(dt.Rows.Count + 1);
                        dr[1] = tDependentName.Value.Trim().ToUpper();
                        dr[2] = Convert.ToUInt64(tDependentAge.Value);
                        dr[3] = tDependentRelationship.Value;
                        dt.Rows.Add(dr);

                        //2023-06-18 : CHANGE TO VIEWSTATE
                        //Session["dtDependent"] = dt;
                        ViewState["dtDependent"] = dt;

                        //2023-06-18 : CHANGE TO VIEWSTATE
                        //LoadData(gvDependent, "dtDependent");
                        LoadDataViewState(gvDependent, "dtDependent");


                        ClearDependent();
                    }
                    else if (session == "bSPADependent")
                    {
                        //dt = ws.Select($"SELECT MAX(ISNULL(LineNum,0)) + 1 [LineNum] FROM temp_CRD2 WHERE UserID = {(int)Session["UserID"]}","MaxLineNum","Addon").Tables["MaxLineNum"];
                        int count = GetCountID("LineNum");

                        if (ws.AddSPADependent((int)Session["UserID"], (int)Session["SPACoBorrowerCount"], count, tDependentName.Value, int.Parse(tDependentAge.Value), tDependentRelationship.Value) == true)
                        {
                            dt = ws.select_temp_crd2((int)Session["UserID"], (int)Session["SPACoBorrowerCount"]).Tables["select_temp_crd2"];
                            Session["dtSPADependent"] = dt;
                            LoadData(gvSPADependent, "dtSPADependent");
                            ClearDependent();
                        }

                        //DataRow dr = dt.NewRow();

                        //dr[0] = Convert.ToInt32(dt.Rows.Count + 1);
                        //dr[1] = tDependentName.Value;
                        //dr[2] = Convert.ToUInt64(tDependentAge.Value);
                        //dr[3] = tDependentRelationship.Value;
                        //dt.Rows.Add(dr);

                        //Session["dtSPADependent"] = dt;


                    }
                    ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "MsgDependent_Hide();", true);
                }
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }
        }

        int GetCountID(string Type)
        {
            int ret = 0;
            try
            {
                DataTable dt = new DataTable();
                string param = "";

                if (Type == "LineNum")
                { param = $@"CRD2"" WHERE ""ID"" = {(int)Session["SPACoBorrowerCount"]} AND "; }
                else if (Type == "ID")
                { param = $@"CRD5"" WHERE "; }

                param = param + $@"""UserID"" = {(int)Session["UserID"]}";

                dt = hana.GetData($@"SELECT MAX(IFNULL(""{Type}"",0)) + 1 ""{Type}"" FROM ""temp_{param}", hana.GetConnection("SAOHana"));
                ret = int.Parse((string)DataAccess.GetData(dt, 0, Type, "0"));
            }
            catch (Exception ex)
            {
                if (Type == "LineNum")
                { ret = 0; }
                else if (Type == "ID")
                { ret = 1; }
            }
            return ret;
        }

        void ClearDependent()
        {
            tDependentName.Value = "";
            tDependentAge.Value = "";
            tDependentRelationship.Value = "";
        }

        void ClearBankAccount()
        {
            tBABank.Value = "";
            tBABranch.Value = "";
            tBAAcctType.Value = "";
            tBAAcctNo.Value = "";
            //tBAAvgDailyBal.Value = "";
            //tBAPresBal.Value = "";
        }

        void ClearCharacterRef()
        {
            tCRName.Value = "";
            tCRAddress.Value = "";
            tCRTelNo.Value = "";
            reftxtEmail.Value = "";
        }

        void ClearAll()
        {
            LoadGridView("dtDependent");
            LoadGridView("dtSPADependent");
            LoadGridView("dtBankAccount");
            LoadGridView("dtCharacterRef");
        }

        protected void btnCancel_ServerClick(object sender, EventArgs e)
        {
            if (Session["CardCode"] != null)
            {
                if (Session["CardCode"].ToString() != "")
                {
                    confirmation("Are you sure you want to delete the selected buyer?", "DelBuyer");
                }
                else { alertMsg("Please select buyer first!", "warning"); }
            }
            else { alertMsg("Please select buyer first!", "warning"); }
        }

        protected void btnEmployment_ServerClick(object sender, EventArgs e)
        {
            Control btn = (Control)sender;
            string id = btn.ID;
            string txt = "Choose ";
            string GrpCode = "";

            if (id == "bEmploymentStatus")
            { txt += "Employment Status"; GrpCode = "ES"; }
            else if (id == "bNatureEmployment")
            { txt += "Nature of Employment"; GrpCode = "NE"; }
            else if (id == "bCivilStatus")
            { txt += "Civil Status"; GrpCode = "CS"; }
            else if (id == "bSpouseEmpStatus")
            { txt += "Employment Status"; GrpCode = "ES"; }
            else if (id == "bSpouseNatureEmp")
            { txt += "Nature of Employment"; GrpCode = "NE"; }
            else if (id == "bCoBorrower")
            { txt += "Relationship"; GrpCode = "RE"; }
            else if (id == "bSPAEmploymentStatus")
            { txt += "Employment Status"; GrpCode = "ES"; }
            else if (id == "bSPANatureEmployment")
            { txt += "Nature of Employment"; GrpCode = "NE"; }
            else if (id == "bSPACivilStatus")
            { txt += "Civil Status"; GrpCode = "CS"; }
            else if (id == "bNatureofEmp")
            { txt += "Nature of Employment"; GrpCode = "NE"; }
            else if (id == "bTypeofID")
            { txt += "Type of ID"; GrpCode = "ID"; }
            else if (id == "bTypeofID2")
            { txt += "Type of ID 2"; GrpCode = "ID"; }
            else if (id == "bTypeofID3")
            { txt += "Type of ID 3"; GrpCode = "ID"; }
            else if (id == "bTypeofID4")
            { txt += "Type of ID 4"; GrpCode = "ID"; }
            else if (id == "bDependentRelationship")
            { txt += "Relationship"; GrpCode = "RE"; }
            else if (id == "bSalesAgent")
            { txt += "Sales Agent"; }
            else if (id == "bSpecialInstructions")
            { txt += "Special Instructions"; }

            ChooseEmployment.InnerText = txt;


            if (txt == "Choose Sales Agent")
            {
                gvEmployment.DataSource = hana.GetData($@"SELECT DISTINCT ""Id"" ""Code"", ""SalesPerson"" ""Name"" FROM ""OSLA""", hana.GetConnection("SAOHana"));
                gvEmployment.DataBind();
                employmentSearch.Visible = true;
            }
            else if (txt == "Choose Special Instructions")
            {
                gvEmployment.DataSource = hana.GetData($@"Select 'Home Address' as ""Code"", 'Home Address' as ""Name"" from ""DUMMY"" union all select 'Business Address' as ""Code"", 'Business Address' as ""Name"" from ""DUMMY"" union all Select 'OTH' as ""Code"", 'Others' as ""Name"" from ""DUMMY""", hana.GetConnection("SAOHana"));
                gvEmployment.DataBind();
                employmentSearch.Visible = false;
            }
            else
            {
                gvEmployment.DataSource = hana.GetData($@"(SELECT ""Code"", ""Name"" FROM ""OLST"" WHERE ""GrpCode"" = '{GrpCode}' AND ""Code"" <> 'OTH' ORDER BY ""Code"") UNION ALL SELECT ""Code"", ""Name"" FROM ""OLST"" WHERE ""GrpCode"" = '{GrpCode}' AND ""Code"" = 'OTH'", hana.GetConnection("SAOHana"));
                gvEmployment.DataBind();
                if (GrpCode != "RE")
                {
                    employmentSearch.Visible = true;
                }
                else
                {
                    employmentSearch.Visible = false;
                }
            }



            Session["btnID"] = id;

            //STAY IN CURRENT TAB
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);
        }

        protected void btnSelectEmployment_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string Code = GetID.CommandArgument;

            string GrpCode = (string)Session["btnID"];

            if (GrpCode == "bEmploymentStatus")
            {
                tEmpStatus.SelectedValue = ws.GetOLSTName(Code);
                Session["tEmpStatus"] = Code;
                if (Code == "OTH")
                { tEmpStatus.Enabled = true; tEmpStatus.SelectedValue = "OTH"; }
                else { tEmpStatus.Enabled = false; }
            }
            else if (GrpCode == "bNatureEmployment")
            {
                tNatureEmployment.SelectedValue = ws.GetOLSTName(Code);
                Session["tNatureEmployment"] = Code;
                if (Code == "OTH")
                { tNatureEmployment.Enabled = true; tNatureEmployment.SelectedValue = "---Select Nature of Employment---"; }
                else { tNatureEmployment.Enabled = false; }
            }
            else if (GrpCode == "bCivilStatus")
            {
                tCivilStatus.SelectedValue = ws.GetOLSTName(Code);
                Session["tCivilStatus"] = Code;
                if (Code == "OTH")
                { tCivilStatus.Enabled = true; tCivilStatus.SelectedValue = "---Select Civil Status---"; }
                else { tCivilStatus.Enabled = false; }

                if (tCivilStatus.SelectedValue == "Married" && ddlBusinessType.Text == "Individual")
                {
                    spouseBtn.Visible = true;
                    RequiredFieldValidator11.Enabled = true;
                    RequiredFieldValidator9.Enabled = true;
                    RequiredFieldValidator10.Enabled = true;
                }
                else
                {
                    spouseBtn.Visible = false;
                    RequiredFieldValidator11.Enabled = false;
                    RequiredFieldValidator9.Enabled = false;
                    RequiredFieldValidator10.Enabled = false;
                }

            }
            else if (GrpCode == "bSpouseEmpStatus")
            {
                tSpouseEmpStatus.SelectedValue = ws.GetOLSTName(Code);
                Session["tSpouseEmpStatus"] = Code;
                if (Code == "OTH")
                { tSpouseEmpStatus.Enabled = true; tSpouseEmpStatus.SelectedValue = ""; }
                else { tSpouseEmpStatus.Enabled = false; }
            }
            else if (GrpCode == "bSpouseNatureEmp")
            {
                tSpouseNatureEmp.SelectedValue = ws.GetOLSTName(Code);
                Session["tSpouseNatureEmp"] = Code;
                if (Code == "OTH")
                { tSpouseNatureEmp.Enabled = true; tSpouseNatureEmp.SelectedValue = ""; }
                else { tSpouseNatureEmp.Enabled = false; }
            }
            else if (GrpCode == "bCoBorrower")
            {
                tRelationship.Value = ws.GetOLSTName(Code);
                Session["tRelationship"] = Code;
                if (Code == "OTH")
                { tRelationship.Disabled = false; tRelationship.Value = ""; }
                else { tRelationship.Disabled = true; }
                if (tRelationship.Value == "Spouse")
                {
                    tSPAPresentAddress.Value = (tPresentAddress.Text.ToUpper() == "N/A" ? "" : $@"{tPresentAddress.Text} ")
                    + (txtPresentStreet.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentStreet.Text} ")
                    + (txtPresentSubdivision.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentSubdivision.Text} ")
                    + (txtPresentBarangay.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentBarangay.Text} ")
                    + (txtPresentCity.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentCity.Text} ")
                    + (txtPresentProvince.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentProvince.Text} ");
                }
            }
            else if (GrpCode == "bSPAEmploymentStatus")
            {
                tSPAEmploymentStatus.SelectedValue = ws.GetOLSTName(Code);
                Session["tSPAEmploymentStatus"] = Code;
                if (Code == "OTH")
                { tSPAEmploymentStatus.Enabled = true; tSPAEmploymentStatus.SelectedValue = ""; }
                else { tSPAEmploymentStatus.Enabled = false; }
            }
            else if (GrpCode == "bSPANatureEmployment")
            {
                tSPANatureEmployment.SelectedValue = ws.GetOLSTName(Code);
                Session["tSPANatureEmployment"] = Code;
                if (Code == "OTH")
                { tSPANatureEmployment.Enabled = true; tSPANatureEmployment.SelectedValue = ""; }
                else { tSPANatureEmployment.Enabled = false; }
            }
            else if (GrpCode == "bSPACivilStatus")
            {
                tSPACivilStatus.SelectedValue = ws.GetOLSTName(Code);
                Session["tSPACivilStatus"] = Code;
                if (Code == "OTH")
                { tSPACivilStatus.Enabled = true; tSPACivilStatus.SelectedValue = ""; }
                else { tSPACivilStatus.Enabled = false; }
            }
            else if (GrpCode == "bNatureofEmp")
            {
                //Session["tNatureofEmp"] = Code;
                //if (Code == "OTH")
                //{ tNatureofEmp.Disabled = false; tNatureofEmp.Value = ""; }
                //else { tNatureofEmp.Disabled = true; tNatureofEmp.Value = ws.GetOLSTName(Code); }

                //tTypeOfId.Enabled = false;
                //tIDNo.Value = "";
                //tIDNo.Disabled = true;
                //tTypeOfId.SelectedValue = "---Select Type of ID---";
            }
            else if (GrpCode == "bTypeofID")
            {

                Session["tTypeOfId"] = Code;

                if (Code == "OTH")
                { tTypeOfId.Enabled = true; tTypeOfId.SelectedValue = "---Select Type of ID---"; }
                else { tTypeOfId.Enabled = false; tTypeOfId.SelectedValue = ws.GetOLSTName(Code); }

                tIDNo.Text = "";
                tIDNo.Enabled = true;
            }
            else if (GrpCode == "bTypeofID2")
            {

                Session["tTypeOfId2"] = Code;

                if (Code == "OTH")
                { tTypeOfId2.Enabled = true; tTypeOfId2.SelectedValue = "---Select Type of ID---"; }
                else { tTypeOfId2.Enabled = false; tTypeOfId2.SelectedValue = ws.GetOLSTName(Code); }

                tIDNo2.Text = "";
                tIDNo2.Enabled = true;
            }
            else if (GrpCode == "bTypeofID3")
            {

                Session["tTypeOfId3"] = Code;

                if (Code == "OTH")
                { tTypeOfId3.Enabled = true; tTypeOfId3.SelectedValue = "---Select Type of ID---"; }
                else { tTypeOfId3.Enabled = false; tTypeOfId3.SelectedValue = ws.GetOLSTName(Code); }

                tIDNo3.Text = "";
                tIDNo3.Enabled = true;
            }
            else if (GrpCode == "bTypeofID4")
            {

                Session["tTypeOfId4"] = Code;

                if (Code == "OTH")
                { tTypeOfId4.Enabled = true; tTypeOfId4.SelectedValue = "---Select Type of ID---"; }
                else { tTypeOfId4.Enabled = false; tTypeOfId4.SelectedValue = ws.GetOLSTName(Code); }

                tIDNo4.Text = "";
                tIDNo4.Enabled = true;
            }
            else if (GrpCode == "bDependentRelationship")
            {
                tDependentRelationship.Value = ws.GetOLSTName(Code);
                Session["tDependentRelationship"] = Code;
                if (Code == "OTH")
                { tDependentRelationship.Disabled = false; tDependentRelationship.Value = ""; }
                else { tDependentRelationship.Disabled = true; }
            }
            else if (GrpCode == "bSalesAgent")
            {
                DataTable dt = hana.GetData($@"SELECT ""SalesPerson"" ""Name"" FROM ""OSLA"" WHERE ""Id"" = '{Code}'", hana.GetConnection("SAOHana"));
                txtSalesAgent.Value = DataAccess.GetData(dt, 0, "Name", "").ToString();
                Session["SalesAgent"] = Code;

            }
            else if (GrpCode == "bSpecialInstructions")
            {
                Session["SpecialInstructions"] = Code;
                if (Code == "OTH")
                { tSpecialInstructions.Disabled = false; tSpecialInstructions.Value = ""; }
                else { tSpecialInstructions.Disabled = true; tSpecialInstructions.Value = Code; }

            }

            ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "MsgEmployment_Hide();", true);

            //STAY IN CURRENT TAB
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);

        }

        protected void bDependent_ServerClick(object sender, EventArgs e)
        {
            Control btn = (Control)sender;
            Session["bDependent"] = btn.ID;

            //2023-06-17 : FIX ADDING & UPDATING OF DEPENDENTS
            btnAdd.Visible = true;
            btnUpdate.Visible = false;

            //STAY IN CURRENT TAB
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);
        }

        protected void gvBankAccount_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //2023-06-18 : CHANGE TO ViewState
            //LoadData(gvBankAccount, "dtBankAccount");
            LoadDataViewState(gvBankAccount, "dtBankAccount");

            gvBankAccount.PageIndex = e.NewPageIndex;
            gvBankAccount.DataBind();
        }

        protected void btnBAAdd_ServerClick(object sender, EventArgs e)
        {

            if (tBABank.Value == "" || tBABranch.Value == "" || tBAAcctType.Value == "" || tBAAcctNo.Value == "" /*|| tBAAvgDailyBal.Value == "" || tBAPresBal.Value == ""*/)
            { alertMsg("Please fill up all forms before adding!", "info"); }
            else
            {
                DataTable dt = new DataTable();

                //2023-06-18 : CHANGE TO ViewState
                //dt = (DataTable)Session["dtBankAccount"];
                dt = (DataTable)ViewState["dtBankAccount"];

                DataRow dr = dt.NewRow();

                dr[0] = Convert.ToInt32(dt.Rows.Count + 1);
                dr[1] = tBABank.Value.Trim().ToUpper();
                dr[2] = tBABranch.Value.Trim().ToUpper();
                dr[3] = tBAAcctType.Value;
                dr[4] = tBAAcctNo.Value;
                //dr[5] = Convert.ToDouble(tBAAvgDailyBal.Value);
                //dr[6] = Convert.ToDouble(tBAPresBal.Value);
                dt.Rows.Add(dr);

                //2023-06-18 : CHANGE TO ViewState
                //Session["dtBankAccount"] = dt; 
                //LoadData(gvBankAccount, "dtBankAccount");
                ViewState["dtBankAccount"] = dt;
                LoadDataViewState(gvBankAccount, "dtBankAccount");


                ClearBankAccount();




                ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "MsgBankAccount_Hide();", true);
            }
        }

        protected void btnBankAccount_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            Session["Buyers_ID"] = Convert.ToInt32(GetID.CommandArgument);
            confirmation("Are you sure you want to delete the selected bank account?", "DelBankAccount");
        }

        protected void btnCharacterRef_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            Session["Buyers_ID"] = Convert.ToInt32(GetID.CommandArgument);
            confirmation("Are you sure you want to delete the selected reference?", "DelCharacterRef");
        }

        protected void btnCRAdd_ServerClick(object sender, EventArgs e)
        {
            if (tCRName.Value == "" || tCRAddress.Value == "" || tCRTelNo.Value == "")
            { alertMsg("Please fill up all forms before adding!", "info"); }
            else
            {
                DataTable dt = new DataTable();

                //2023-06-18 : CHANGE TO ViewState
                //dt = (DataTable)Session["dtCharacterRef"];
                dt = (DataTable)ViewState["dtCharacterRef"];

                DataRow dr = dt.NewRow();

                dr[0] = Convert.ToInt32(dt.Rows.Count + 1);
                dr[1] = tCRName.Value.Trim().ToUpper();
                dr[2] = tCRAddress.Value.Trim().ToUpper();
                dr[3] = reftxtEmail.Value;
                dr[4] = tCRTelNo.Value;
                dt.Rows.Add(dr);

                //2023-06-18 : CHANGE TO ViewState
                //Session["dtCharacterRef"] = dt;
                //LoadData(gvCharacterRef, "dtCharacterRef");
                ViewState["dtCharacterRef"] = dt;
                LoadDataViewState(gvCharacterRef, "dtCharacterRef");


                ClearCharacterRef();


                btnCRAdd.Visible = true;
                btnRefUpdate.Visible = false;

                ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "MsgCharacterRef_Hide();", true);
            }
        }

        protected void gvCharacterRef_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //2023-06-18 : CHANGE TO ViewState
            //LoadData(gvCharacterRef, "dtCharacterRef");
            LoadDataViewState(gvCharacterRef, "dtCharacterRef");

            gvCharacterRef.PageIndex = e.NewPageIndex;
            gvCharacterRef.DataBind();
        }

        protected void gvSPADependent_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadData(gvSPADependent, "dtSPADependent");
            gvSPADependent.PageIndex = e.NewPageIndex;
            gvSPADependent.DataBind();
        }

        protected void btnSave_ServerClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {

                try
                {
                    //if ((!string.IsNullOrWhiteSpace(tLastName.Value) || !string.IsNullOrWhiteSpace(txtCompanyName.Value)) && !string.IsNullOrWhiteSpace(tTIN.Value))

                    //2023-11-08 : CHANGE APPROACH ON BLOCKINGS
                    //if (
                    //      (
                    //          (!string.IsNullOrWhiteSpace(tLastName.Text) || !string.IsNullOrWhiteSpace(txtCompanyName.Text)) &&
                    //          !string.IsNullOrWhiteSpace(tTINCorp.Text)
                    //      )

                    //      ||

                    //      (ddlBusinessType.SelectedValue == "Trusteeship" && rbGuardian.Checked) ||
                    //      (ddlBusinessType.SelectedValue == "Guardianship" && rbGuardee.Checked)
                    //  )
                    //{
                    //    confirmation("Are you sure you want to save this transaction?", "SaveUpdateBP");
                    //}
                    //else
                    //{
                    //    alertMsg("No name/TIN provided.", "warning");
                    //}

                    int blockingTag = 0;
                    //2023-11-08 : CHECK IF INDIVIDUAL HAS NO TIN   
                    if (!string.IsNullOrWhiteSpace(tLastName.Text))
                    {
                        if (string.IsNullOrWhiteSpace(tTIN.Text))
                        {
                            blockingTag++;
                        }
                    }
                    //2023-11-08 : CHECK IF CORPORATE HAS NO TIN   
                    else if (!string.IsNullOrWhiteSpace(txtCompanyName.Text))
                    {
                        if (string.IsNullOrWhiteSpace(tTINCorp.Text))
                        {
                            blockingTag++;
                        }
                    }
                    //2023-11-08 : CHECK IF SELECTED RADIOBUTTON IS PROPER
                    if (ddlBusinessType.SelectedValue == "Trusteeship")
                    {
                        if (!rbGuardian.Checked)
                        {
                            blockingTag++;
                        }
                    }
                    //2023-11-08 : CHECK IF SELECTED RADIOBUTTON IS PROPER
                    else if (ddlBusinessType.SelectedValue == "Guardianship")
                    {
                        if (!rbGuardee.Checked)
                        {
                            blockingTag++;
                        }
                    }

                    //2023-11-08 : CHECK IF BLOCKED
                    if (blockingTag == 0)
                    {
                        confirmation("Are you sure you want to save this transaction?", "SaveUpdateBP");
                    }
                    else
                    {
                        alertMsg("No name/TIN provided.", "warning");
                    }

                    //STAY IN CURRENT TAB
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);
                }
                catch (Exception ex)
                {
                    alertMsg(ex.Message, "error");
                }
            }
        }

        protected void gvBuyers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            { LoadData(gvBuyers, "dtBuyers"); }
            catch (Exception ex)
            { alertMsg(ex.Message, "error"); }

            gvBuyers.PageIndex = e.NewPageIndex;
            gvBuyers.DataBind();
        }

        protected void btnFind_ServerClick(object sender, EventArgs e)
        {
            string position = Session["BrkPosition"].ToString();
            string qry;
            int count = 0;
            if (position.ToUpper() == "SALES AGENT")
            {
                qry = $@"SELECT ""Id""
                                FROM BRK1	
                                WHERE ""SAPCardCode"" = '{Session["UserName"].ToString()}'";
                DataTable dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
                dt = hana.GetData($@"CALL sp_Buyers('{DataAccess.GetData(dt, 0, "Id", "").ToString()}')", hana.GetConnection("SAOHana"));
                Session["dtBuyers"] = dt;
            }
            else if (position.ToUpper() == "BROKER")
            {
                qry = $@"SELECT A.""Id""
                                FROM BRK1 A
                                	Inner join OBRK B ON A.""BrokerId"" = B.""BrokerId""
                                WHERE B.""SAPCardCode"" = '{Session["UserName"].ToString()}'";
                DataTable dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
                DataTable dt2 = null;
                DataTable dt3 = null;
                foreach (DataRow row in dt.Rows)
                {
                    if (count <= 0)
                    {
                        dt2 = hana.GetData($@"CALL sp_Buyers('{row[0].ToString()}')", hana.GetConnection("SAOHana"));
                    }
                    else
                    {
                        dt3 = hana.GetData($@"CALL sp_Buyers('{row[0].ToString()}')", hana.GetConnection("SAOHana"));
                        if (dt3.Rows.Count > 0)
                        {
                            dt2.Rows.Add(dt3);
                        }
                    }
                    count++;
                }
                Session["dtBuyers"] = dt2;
            }
            else
            {
                Session["dtBuyers"] = hana.GetData($@"CALL sp_Buyers('')", hana.GetConnection("SAOHana"));
            }
            try
            { LoadData(gvBuyers, "dtBuyers"); }
            catch (Exception ex)
            { alertMsg(ex.Message, "error"); }
        }

        protected void bSearch_ServerClick(object sender, EventArgs e)
        {
            string position = Session["BrkPosition"].ToString();
            string qry;
            int count = 0;
            if (position.ToUpper() == "SALES AGENT")
            {
                qry = $@"SELECT ""Id""
                                FROM BRK1	
                                WHERE ""SAPCardCode"" = '{Session["UserName"].ToString()}'";
                DataTable dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
                dt = hana.GetData($@"CALL sp_Search (1,'{txtSearch.Value}','{DataAccess.GetData(dt, 0, "Id", "").ToString()}')", hana.GetConnection("SAOHana"));
                Session["dtBuyers"] = dt;
            }
            else if (position.ToUpper() == "BROKER")
            {
                qry = $@"SELECT A.""Id""
                                FROM BRK1 A
                                	Inner join OBRK B ON A.""BrokerId"" = B.""BrokerId""
                                WHERE B.""SAPCardCode"" = '{Session["UserName"].ToString()}'";
                DataTable dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
                DataTable dt2 = null;
                DataTable dt3 = null;
                foreach (DataRow row in dt.Rows)
                {
                    if (count <= 0)
                    {
                        dt2 = hana.GetData($@"CALL sp_Search (1,'{txtSearch.Value}','{row[0].ToString()}')", hana.GetConnection("SAOHana"));
                    }
                    else
                    {
                        dt3 = hana.GetData($@"CALL sp_Search (1,'{txtSearch.Value}','{row[0].ToString()}')", hana.GetConnection("SAOHana"));
                        if (dt3.Rows.Count > 0)
                        {
                            dt2.Rows.Add(dt3);
                        }
                    }
                    count++;
                }
                Session["dtBuyers"] = dt2;
            }
            else
            {
                Session["dtBuyers"] = hana.GetData($@"CALL sp_Search (1,'{txtSearch.Value}','')", hana.GetConnection("SAOHana"));
            }
            LoadData(gvBuyers, "dtBuyers");
        }

        protected void btnSalesSearch_ServerClick(object sender, EventArgs e)
        {
            string type = ddlBusinessType.SelectedValue;
            if (ddlBusinessType.SelectedValue != "Co-ownership" && ddlBusinessType.SelectedValue != "Individual" && ddlBusinessType.SelectedValue != "Corporation" && ddlBusinessType.SelectedValue != "Others")
            {
                string trustguard = "";
                trustguard = rbGuardian.Checked ? rbGuardee.Text : rbGuardian.Text;

                string qrytrustguard = $@"Select B.""CardCode"",B.""LastName"",B.""FirstName"",B.""MiddleName"",A.""BusinessType"" as ""SpecifiedBusinessType"" from ""OCRD"" A
                            INNER JOIN CRD1 B ON A.""CardCode"" = B.""CardCode""
                            where (UPPER(B.""CardCode"") LIKE '%{txtSearchLinkBuyer.Value.ToUpper()}%' OR UPPER(B.""FirstName"") LIKE '%{txtSearchLinkBuyer.Value.ToUpper()}%'
                            OR UPPER(B.""MiddleName"") LIKE '%{txtSearchLinkBuyer.Value.ToUpper()}%' OR UPPER(B.""LastName"") LIKE '%{txtSearchLinkBuyer.Value.ToUpper()}%')
                            AND A.""BusinessType"" = '{type}' AND IFNULL(A.""Approved"", '') <> '' AND A.""CardCode"" NOT IN('{Session["CardCode"].ToString()}','{ViewState["BuyerLink"].ToString()}') AND B.""CardType"" = 'Buyer' AND IFNULL(A.""SpecialBuyerRole"",'{rbGuardian.Text}') = '{trustguard}'";

                DataTable dt = hana.GetData(qrytrustguard, hana.GetConnection("SAOHana"));
                gvCoOwner.DataSource = dt;
                gvCoOwner.Columns[4].Visible = false;
                gvCoOwner.DataBind();
            }
            else if (ddlBusinessType.SelectedValue == "Co-ownership" || ddlBusinessType.SelectedValue == "Others")
            {
                string Coowners = "";
                DataTable dt = (DataTable)Session["dtCoOwner"];
                foreach (DataRow row in dt.Rows)
                {
                    Coowners += $@",'{row["CardCode"]}'";
                }
                string qry = $@"Select B.""CardCode"",B.""LastName"",B.""FirstName"",B.""MiddleName"",A.""BusinessType"" as ""SpecifiedBusinessType"" from ""OCRD"" A
                            INNER JOIN CRD1 B ON A.""CardCode"" = B.""CardCode""
                            where (UPPER(B.""CardCode"") LIKE '%{txtSearchLinkBuyer.Value.ToUpper()}%' OR UPPER(B.""FirstName"") LIKE '%{txtSearchLinkBuyer.Value.ToUpper()}%'
                            OR UPPER(B.""MiddleName"") LIKE '%{txtSearchLinkBuyer.Value.ToUpper()}%' OR UPPER(B.""LastName"") LIKE '%{txtSearchLinkBuyer.Value.ToUpper()}%')
                            AND A.""BusinessType"" = '{type}' AND IFNULL(A.""Approved"", '') <> '' AND A.""CardCode"" NOT IN('{Session["CardCode"].ToString()}'{Coowners}) AND B.""CardType"" = 'Buyer'";
                dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
                gvCoOwner.DataSource = dt;
                gvCoOwner.Columns[4].Visible = true;
                gvCoOwner.DataBind();
            }
            //gvCoOwner.DataSource = hana.GetData($@"Select B.*,A.""SpecifiedBusinessType"" from ""OCRD"" A
            //                                    INNER JOIN CRD1 B ON A.""CardCode"" = B.""CardCode""
            //                                    WHERE (UPPER(B.""CardCode"") LIKE '%{txtSearchLinkBuyer.Value.ToUpper()}%' OR UPPER(B.""FirstName"") LIKE '%{txtSearchLinkBuyer.Value.ToUpper()}%'
            //                                    OR UPPER(B.""MiddleName"") LIKE '%{txtSearchLinkBuyer.Value.ToUpper()}%' OR UPPER(B.""LastName"") LIKE '%{txtSearchLinkBuyer.Value.ToUpper()}%')
            //                                    AND A.""BusinessType"" = '{type}' AND IFNULL(A.""Approved"", '') <> '' AND A.""CardCode"" <> '{Session["CardCode"].ToString()}' AND B.""CardType"" = 'Buyer'", hana.GetConnection("SAOHana"));
            //gvCoOwner.DataBind();
        }

        protected void bDeleteBuyer_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            Control btn = (Control)sender;
            Session["Buyers_Control"] = btn.ID;
            Session["Buyers_ID"] = GetID.CommandArgument;
            confirmation("Are you sure you want to delete the selected buyer?", "DelBuyer");
        }

        void loadDivisionsPerBusinessType()
        {
            if (ddlBusinessType.Text != "Corporation")
            {
                divIndividual.Visible = true;
                divCompanyName.Visible = false;
                txtCompanyName.Text = " ";
                RequiredFieldValidator8.Enabled = false;
            }
            else
            {
                divIndividual.Visible = false;
                divCompanyName.Visible = true;
                RequiredFieldValidator8.Enabled = true;
            }
        }

        protected void bSelectBuyer_Click(object sender, EventArgs e)
        {

            try
            {


                divCustomerCode.Visible = true;
                btnPrint.Visible = true;


                FirstLoad();
                DataTable ValidId = (DataTable)ViewState["ValidId"];
                DataTable ValidIdPrev = (DataTable)ViewState["ValidIdPrev"];
                lblSave.InnerText = "Update";
                DateTime date;
                string bdate;
                LinkButton GetID = (LinkButton)sender;
                string Code = GetID.CommandArgument;

                lblCustomerCode.Text = Code;

                Session["CardCode"] = Code;
                string tCode;
                ClearAll();
                DataTable dt = new DataTable();
                DataTable dtapprove = new DataTable();
                dtapprove = hana.GetData($@"SELECT 
		                          ""Approved""
                                 ,""ApprovedDocument""
                                 ,""Guid""
                                FROM
                                    ""OCRD""
                                WHERE
                                    ""CardCode"" = '{Code}' ", hana.GetConnection("SAOHana"));
                //AND ""IsArchive"" = FALSE;
                string approved = (string)DataAccess.GetData(dtapprove, 0, "Approved", "N");
                lblAprvAttachment.Text = (string)DataAccess.GetData(dtapprove, 0, "ApprovedDocument", ""); ;
                DataTable dtaccess = (DataTable)Session["UserAccess"];
                var access = dtaccess.Select($"CodeEncrypt= 'SALES01'");

                //GET GUID FROM OCRD
                ViewState["Guid"] = (string)DataAccess.GetData(dtapprove, 0, "Guid", "");


                //if (Session["UserName"].ToString() != "Sales01" && approved == "Y")
                if (!access.Any())
                {
                    btnApprove.Visible = false;
                    divAttch.Visible = String.IsNullOrEmpty(lblAprvAttachment.Text) ? false : true;

                    if (approved == "Y")
                    {
                        divAprv.Visible = true;
                    }
                    else
                    {
                        divAprv.Visible = false;
                    }

                }
                else
                {
                    if (approved != "Y")
                    {
                        btnApprove.Visible = true;
                        divAprv.Visible = false;
                        lblFileName.Text = (string)DataAccess.GetData(dtapprove, 0, "ApprovedDocument", "");
                    }
                    else
                    {
                        btnApprove.Visible = false;
                        divAprv.Visible = true;
                        divAttch.Visible = String.IsNullOrEmpty(lblAprvAttachment.Text) ? false : true;
                    }
                }

                btnUpdatetoSAP.Visible = false;
                // Check if BP is exist in SAP
                dt = hana.GetData($@"SELECT TOP 1 ""DocEntry"",""CardCode"",""CardName"",""CardFName"" FROM ""OCRD"" WHERE ""U_DreamsCustCode"" = '{Code}'", hana.GetConnection("SAPHana"));

                if (DataAccess.Exist(dt))
                {
                    btnUpdatetoSAP.Visible = true;
                }








                //############################# OCRD ################################
                dt = hana.GetData($"CALL sp_BPEditOCRD ('{Code}')", hana.GetConnection("SAOHana"));

                Session["SQDocEntry"] = Code;

                //btnPrint.Visible = true;

                Session["SalesAgent"] = (string)DataAccess.GetData(dt, 0, "SalesAgent", "0");
                DataTable dtSA = hana.GetData($@"SELECT ""SalesPerson"" ""Name"" FROM ""OSLA"" WHERE ""Id"" = '{Session["SalesAgent"].ToString()}'", hana.GetConnection("SAOHana"));
                txtSalesAgent.Value = DataAccess.GetData(dtSA, 0, "Name", "").ToString();

                //SalesAgentDocument
                string SADocs = (string)DataAccess.GetData(dt, 0, "SalesAgentDocument", "");
                if (SADocs == "?" || SADocs == "")
                {
                    lblFileName.Text = "";
                    visibleDocumentButtons(false, btnPreview, btnRemove);
                }
                else
                {
                    lblFileName.Text = SADocs;
                    visibleDocumentButtons(true, btnPreview, btnRemove);
                }
                //tCode = (string)DataAccess.GetData(dt, 0, "NatureEmp", "N/A");
                //Session["tNatureofEmp"] = tCode;
                //tNatureofEmp.Value = ws.GetOLSTName(tCode);




                textAuthorizedPersonAddress.Value = DataAccess.GetData(dt, 0, "AuthorizedPersonAddress", "").ToString();
                txtAuthorizedPersonStreet.Value = DataAccess.GetData(dt, 0, "AuthorizedPersonStreet", "").ToString();
                txtAuthorizedPersonSubdivision.Value = DataAccess.GetData(dt, 0, "AuthorizedPersonSubdivision", "").ToString();
                txtAuthorizedPersonBarangay.Value = DataAccess.GetData(dt, 0, "AuthorizedPersonBarangay", "").ToString();
                txtAuthorizedPersonCity.Value = DataAccess.GetData(dt, 0, "AuthorizedPersonCity", "").ToString();
                txtAuthorizedPersonProvince.Value = DataAccess.GetData(dt, 0, "AuthorizedPersonProvince", "").ToString();




                tCode = (string)DataAccess.GetData(dt, 0, "IDType", "---Select Type of ID---");
                Session["tTypeOfId"] = tCode;
                //tTypeOfId.SelectedValue = ws.GetOLSTName(tCode);
                tTypeOfId.SelectedValue = tCode;

                tIDNo.Enabled = true;
                tIDNo.Text = (string)DataAccess.GetData(dt, 0, "IDNo", "N/A");
                txtOthers1.Text = (string)DataAccess.GetData(dt, 0, "IDOthers", "N/A");




                tCode = (string)DataAccess.GetData(dt, 0, "IDType2", "---Select Type of ID---");
                Session["tTypeOfId2"] = tCode;
                //tTypeOfId2.SelectedValue = ws.GetOLSTName(tCode);
                tTypeOfId2.SelectedValue = tCode;

                tIDNo2.Enabled = true;
                tIDNo2.Text = (string)DataAccess.GetData(dt, 0, "IDNo2", "N/A");
                txtOthers2.Text = (string)DataAccess.GetData(dt, 0, "IDOthers2", "N/A");





                tCode = (string)DataAccess.GetData(dt, 0, "IDType3", "---Select Type of ID---");
                Session["tTypeOfId3"] = tCode;
                //tTypeOfId3.SelectedValue = ws.GetOLSTName(tCode);
                tTypeOfId3.SelectedValue = tCode;

                tIDNo3.Enabled = true;
                tIDNo3.Text = (string)DataAccess.GetData(dt, 0, "IDNo3", "N/A");
                txtOthers3.Text = (string)DataAccess.GetData(dt, 0, "IDOthers3", "N/A");






                tCode = (string)DataAccess.GetData(dt, 0, "IDType4", "---Select Type of ID---");
                Session["tTypeOfId4"] = tCode;
                //tTypeOfId4.SelectedValue = ws.GetOLSTName(tCode);
                tTypeOfId4.SelectedValue = tCode;

                tIDNo4.Enabled = true;
                tIDNo4.Text = (string)DataAccess.GetData(dt, 0, "IDNo4", "N/A");
                txtOthers4.Text = (string)DataAccess.GetData(dt, 0, "IDOthers4", "N/A");





                //selectIndexChange1(tTypeOfId, 1);
                //selectIndexChange1(tTypeOfId2, 2);
                //selectIndexChange1(tTypeOfId3, 3);
                //selectIndexChange1(tTypeOfId4, 4);

                ValidIdDropDownEdit(ValidId, ValidIdPrev, tIDNo.Text, tTypeOfId, 1, tIDNo, txtOthers1);
                ValidIdDropDownEdit(ValidId, ValidIdPrev, tIDNo2.Text, tTypeOfId2, 2, tIDNo2, txtOthers2);
                ValidIdDropDownEdit(ValidId, ValidIdPrev, tIDNo3.Text, tTypeOfId3, 3, tIDNo3, txtOthers3);
                ValidIdDropDownEdit(ValidId, ValidIdPrev, tIDNo4.Text, tTypeOfId4, 4, tIDNo4, txtOthers4);








                tCode = (string)DataAccess.GetData(dt, 0, "SpecialInstructions", "");
                Session["SpecialInstructions"] = tCode;
                tSpecialInstructions.Value = tCode;

                tHomeTelNo.Value = (string)DataAccess.GetData(dt, 0, "HomeTelNo", "N/A");
                tPresentAddress.Text = (string)DataAccess.GetData(dt, 0, "PresentAddress", "N/A");
                tPermanent.Text = (string)DataAccess.GetData(dt, 0, "PermanentAddress", "N/A");
                //txtComaker.Value = (string)DataAccess.GetData(dt, 0, "Comaker", "N/A");

                string HomeOwnership = (string)DataAccess.GetData(dt, 0, "HomeOwnership", "N/A");

                //tPerMonth.Disabled = true;
                //if (HomeOwnership == "Owned")
                //{ tRented_CheckedChanged(tOwned, EventArgs.Empty); }
                //else if (HomeOwnership == "Mortgaged")
                //{ tMortgaged.Checked = true; }
                //else if (HomeOwnership == "LivingwRelatives")
                //{ tLivingwRelatives.Checked = true; }
                //else
                //{
                //    tRented.Checked = true;
                //    tPerMonth.Disabled = false;
                //    tPerMonth.Value = HomeOwnership;
                //}

                //tYearsOfStay.Value = (string)DataAccess.GetData(dt, 0, "YearsStay", "");


                tCode = (string)DataAccess.GetData(dt, 0, "CivilStatus", "---Select Civil Status---");
                if (tCode != "CS1" && tCode != "CS2")
                {
                    tCode = "CS1";
                }
                if (ws.OLSTExist(tCode) == true)
                {

                    Session["tCivilStatus"] = tCode;
                    /*tCivilStatus.SelectedValue = ws.GetOLSTName(tCode);*/
                    tCivilStatus.SelectedValue = tCode;
                }
                else
                {
                    Session["tCivilStatus"] = "OTH";
                    tCivilStatus.SelectedValue = tCode;
                }


                tRemarks.Value = (string)DataAccess.GetData(dt, 0, "Remarks", "");
                ddlBusinessType.Text = (string)DataAccess.GetData(dt, 0, "BusinessType", "Individual");
                loadDivisionsForNames(ddlBusinessType.Text);
                string taxclass = (string)DataAccess.GetData(dt, 0, "TaxClassification", "");
                if (taxclass.ToLower() == ConfigSettings.TaxClassification1.ToLower() ||
                    taxclass.ToLower() == ConfigSettings.TaxClassification2.ToLower() ||
                    taxclass.ToLower() == "corporation")
                {
                    if (taxclass.ToUpper() == "ENGAGED IN BUSINESS")
                    {
                        taxclass = "Engaged in Business";
                    }
                    else if (taxclass.ToUpper() == "NOT ENGAGED IN BUSINESS")
                    {
                        taxclass = "Not engaged in Business";
                    }
                    else
                    {
                        taxclass = "Engaged in Business";
                    }
                    ddTaxClass.SelectedValue = taxclass;
                }

                taxClassChanged();

                //ddSourceFunds.SelectedValue = (string)DataAccess.GetData(dt, 0, "SourceOfFunds", "---Select Source of Funds---");
                ddSourceFunds.SelectedValue = (string)DataAccess.GetData(dt, 0, "SourceOfFunds", "");
                txtOtherSourceOfFund.Text = (string)DataAccess.GetData(dt, 0, "OtherSourceOfFund", "N/A");
                otherFields();
                txtPresPostal.Text = (string)DataAccess.GetData(dt, 0, "PresentPostalCode", "N/A");
                txtPermPostal.Text = (string)DataAccess.GetData(dt, 0, "PermanentPostalCode", "N/A");
                ddPreCountry.SelectedValue = (string)DataAccess.GetData(dt, 0, "PresentCountry", "PH");
                ddPermCountry.SelectedValue = (string)DataAccess.GetData(dt, 0, "PermanentCountry", "PH");
                txtPermYrStay.Text = (string)DataAccess.GetData(dt, 0, "PermanentYrStay", "N/A");
                txtPresYrStay.Text = (string)DataAccess.GetData(dt, 0, "PresentYrStay", "N/A");
                txtProfession.Text = (string)DataAccess.GetData(dt, 0, "Profession", "N/A");
                ddOccupation.SelectedValue = (string)DataAccess.GetData(dt, 0, "Occupation", "N/A");
                ddMonthlyIncome.SelectedValue = (string)DataAccess.GetData(dt, 0, "MonthlyIncome", "") == "0" ? "" : (string)DataAccess.GetData(dt, 0, "MonthlyIncome", "");
                txtBusinessPhoneNo.Value = (string)DataAccess.GetData(dt, 0, "BusinessPhoneNo", "N/A");
                txtCertifyCompleteName.Value = (string)DataAccess.GetData(dt, 0, "CertifyCompleteName", "N/A");
                txtPresentStreet.Text = (string)DataAccess.GetData(dt, 0, "PresentStreet", "N/A");
                txtPresentSubdivision.Text = (string)DataAccess.GetData(dt, 0, "PresentSubdivision", "N/A");
                txtPresentBarangay.Text = (string)DataAccess.GetData(dt, 0, "PresentBarangay", "N/A");
                txtPresentCity.Text = (string)DataAccess.GetData(dt, 0, "PresentCity", "N/A");
                txtPresentProvince.Text = (string)DataAccess.GetData(dt, 0, "PresentProvince", "N/A");
                txtPermanentStreet.Text = (string)DataAccess.GetData(dt, 0, "PermanentStreet", "N/A");
                txtPermanentSubdivision.Text = (string)DataAccess.GetData(dt, 0, "PermanentSubdivision", "N/A");
                txtPermanentBarangay.Text = (string)DataAccess.GetData(dt, 0, "PermanentBarangay", "N/A");
                txtPermanentCity.Text = (string)DataAccess.GetData(dt, 0, "PermanentCity", "N/A");
                txtPermanentProvince.Text = (string)DataAccess.GetData(dt, 0, "PermanentProvince", "N/A");
                txtSpecifyBusiness.Text = (string)DataAccess.GetData(dt, 0, "SpecifiedBusinessType", "N/A");
                txtReligion.Text = (string)DataAccess.GetData(dt, 0, "Religion", "N/A");
                txtSECCORIDNo.Text = (string)DataAccess.GetData(dt, 0, "SECCORIDNo", "N/A");
                string conformecheck = (string)DataAccess.GetData(dt, 0, "Conforme", "false");
                //if (conformecheck.ToLower() == "true" || conformecheck == "1")
                //{
                //    CBconforme.Checked = true;
                //}
                //else if (conformecheck.ToLower() == "true" || conformecheck == "0")
                //{
                //    CBconforme.Checked = false;
                //}
                string CertifyDate = (string)DataAccess.GetData(dt, 0, "CertifyDate", "");
                if (!string.IsNullOrEmpty(CertifyDate))
                {
                    date = DateTime.Parse(CertifyDate);
                    CertifyDate = date.ToString("yyyy-MM-dd");
                }
                txtCertifyDate.Value = CertifyDate;

                string SpecialBuyerRole = (string)DataAccess.GetData(dt, 0, "SpecialBuyerRole", "");
                if (SpecialBuyerRole != "")
                {
                    if (SpecialBuyerRole == "Guardian" || SpecialBuyerRole == "Trustor")
                    {
                        rbGuardian.Checked = true;
                    }
                    else
                    {
                        rbGuardee.Checked = true;
                    }
                }
                else
                {
                    if (ddlBusinessType.SelectedValue == "Guardianship" || ddlBusinessType.SelectedValue == "Trusteeship")
                    {
                        rbGuardian.Checked = true;
                    }
                }
                ChangeRole();
                foreach (GridViewRow row in gvStandardDocumentRequirements.Rows)
                {
                    var test1 = row.Cells[0].Text;
                    var test2 = row.Cells[1].Text;
                    var test3 = row.Cells[2].Text;

                    if (row.Cells[0].Text == "Proof of billing")
                    {
                        LoadStandardDocuments("ProofOfBillingAttachment", dt, row);
                    }
                    else if (row.Cells[0].Text == "Valid ID 1")
                    {
                        LoadStandardDocuments("ValidId1Attachment", dt, row);
                    }
                    else if (row.Cells[0].Text == "Valid ID 2")
                    {
                        LoadStandardDocuments("ValidId2Attachment", dt, row);
                    }
                    else if (row.Cells[0].Text == "Proof of Income")
                    {
                        LoadStandardDocuments("ProofOfIncomeAttachment", dt, row);
                    }
                }
                foreach (GridViewRow row in gvStandardDocumentRequirements2.Rows)
                {
                    if (row.Cells[0].Text == "Proof of billing")
                    {
                        LoadStandardDocuments2("ProofOfBillingAttachment", dt, row);
                    }
                    else if (row.Cells[0].Text == "Valid ID 1")
                    {
                        LoadStandardDocuments2("ValidId1Attachment", dt, row);
                    }
                    else if (row.Cells[0].Text == "Valid ID 2")
                    {
                        LoadStandardDocuments2("ValidId2Attachment", dt, row);
                    }
                    //2023-11-29 : ADDED PROOF OF INCOME IN CASE IT WILL BE USED AGAIN ON NEXT TABLE
                    else if (row.Cells[0].Text == "Proof of Income")
                    {
                        LoadStandardDocuments2("ProofOfIncomeAttachment", dt, row);
                    }
                }

                //############################## END OCRD ################################

                //############################# CRD1 ################################

                dt = hana.GetData($"CALL sp_BPEditCRD1 ('{Code}','Buyer')", hana.GetConnection("SAOHana"));

                tLastName.Text = (string)DataAccess.GetData(dt, 0, "LastName", "N/A") == " " ? "N/A" : (string)DataAccess.GetData(dt, 0, "LastName", "N/A");
                tFirstName.Text = (string)DataAccess.GetData(dt, 0, "FirstName", "N/A") == " " ? "N/A" : (string)DataAccess.GetData(dt, 0, "FirstName", "N/A");
                tMiddleName.Text = (string)DataAccess.GetData(dt, 0, "MiddleName", "N/A") == " " ? "N/A" : (string)DataAccess.GetData(dt, 0, "MiddleName", "N/A");
                tLastName2.Text = (string)DataAccess.GetData(dt, 0, "LastName", "N/A") == " " ? "N/A" : (string)DataAccess.GetData(dt, 0, "LastName", "N/A");
                tFirstName2.Text = (string)DataAccess.GetData(dt, 0, "FirstName", "N/A") == " " ? "N/A" : (string)DataAccess.GetData(dt, 0, "FirstName", "N/A");
                tMiddleName2.Text = (string)DataAccess.GetData(dt, 0, "MiddleName", "N/A") == " " ? "N/A" : (string)DataAccess.GetData(dt, 0, "MiddleName", "N/A");


                txtCompanyName.Text = (string)DataAccess.GetData(dt, 0, "CompanyName", "N/A");
                txtConformeCorp.Value = (string)DataAccess.GetData(dt, 0, "CompanyName", "N/A");

                loadDivisionsPerBusinessType();

                //2023-06-27 : GET FROM SPOUSE BUSINESS COUNTRY
                //ddSPOBusCountry.SelectedValue = (string)DataAccess.GetData(dt, 0, "EmpBusCountry", "PH");
                ddSPOBusCountry.SelectedValue = (string)DataAccess.GetData(dt, 0, "SpouseBusCountry", "PH");

                ddEmployCountry.SelectedValue = (string)DataAccess.GetData(dt, 0, "EmpBusCountry", "PH");

                tGender.Value = (string)DataAccess.GetData(dt, 0, "Gender", "N/A");
                tCitizenship.Value = (string)DataAccess.GetData(dt, 0, "Citizenship", "N/A");

                bdate = (string)DataAccess.GetData(dt, 0, "BirthDay", "");
                if (!string.IsNullOrEmpty(bdate))
                {
                    date = DateTime.Parse(bdate);
                    bdate = date.ToString("yyyy-MM-dd");
                }

                tBirthDate.Value = bdate;
                tBirthPlace.Value = (string)DataAccess.GetData(dt, 0, "BirthPlace", "N/A");
                tCellphoneNo.Value = (string)DataAccess.GetData(dt, 0, "CellNo", "N/A");
                tEmpEmailAddress.Value = (string)DataAccess.GetData(dt, 0, "EmailAddress", "N/A");
                tFBAccount.Value = (string)DataAccess.GetData(dt, 0, "FBAccount", "N/A");
                //tTIN.Value = (string)DataAccess.GetData(dt, 0, "TIN", "N/A");


                tTINCorp.Text = (string)DataAccess.GetData(dt, 0, "TIN", "N/A");

                tTIN.Text = (string)DataAccess.GetData(dt, 0, "TIN", "N/A");


                tSSSNo.Text = (string)DataAccess.GetData(dt, 0, "SSSNo", "N/A");
                tGSISNo.Value = (string)DataAccess.GetData(dt, 0, "GSISNo", "N/A");
                tPagibigNo.Text = (string)DataAccess.GetData(dt, 0, "PagibiNo", "N/A");
                tPosition.Value = (string)DataAccess.GetData(dt, 0, "Position", "N/A");
                tYearsofService.Value = (string)DataAccess.GetData(dt, 0, "YearsService", "0");
                tOfficeTelNo.Value = (string)DataAccess.GetData(dt, 0, "OfficeTelNo", "N/A");
                tFAXNo.Value = (string)DataAccess.GetData(dt, 0, "FaxNo", "N/A");
                tEmpBusinessName.Value = (string)DataAccess.GetData(dt, 0, "EmpBusName", "N/A");
                tEmpBusinessAddress.Value = (string)DataAccess.GetData(dt, 0, "EmpBusAdd", "N/A");

                //txtProfession.Text = (string)DataAccess.GetData(dt, 0, "SourceOfFunds", "N/A");       

                tCode = (string)DataAccess.GetData(dt, 0, "EmpStatus", "---Select Employment Status---");
                if (tCode == "ES1" || tCode == "ES2" || tCode == "ES3" || tCode == "ES4" || tCode == "ES5")
                {
                    //tEmpStatus.SelectedValue = ws.GetOLSTName(tCode);
                    tEmpStatus.SelectedValue = tCode;
                    Session["tEmpStatus"] = tCode;
                }
                else
                {
                    Session["tEmpStatus"] = "OTH";
                    tEmpStatus.SelectedValue = tCode;
                }

                taxClassChanged();

                tCode = (string)DataAccess.GetData(dt, 0, "NatureEmp", "---Select Nature of Employment---");

                if (ws.OLSTExist(tCode) == true)
                { Session["tNatureEmployment"] = tCode; /*tNatureEmployment.SelectedValue = ws.GetOLSTName(tCode);*/ tNatureEmployment.SelectedValue = tCode; }
                else { Session["tNatureEmployment"] = "---Select Nature of Employment---"; tNatureEmployment.SelectedValue = "---Select Nature of Employment---"; }

                dt = hana.GetData($"CALL sp_BPEditCRD1 ('{Code}','Spouse')", hana.GetConnection("SAOHana"));

                tSpouseLastName.Value = (string)DataAccess.GetData(dt, 0, "LastName", "N/A");
                tSpouseFirstName.Value = (string)DataAccess.GetData(dt, 0, "FirstName", "N/A");
                tSpouseMiddleName.Value = (string)DataAccess.GetData(dt, 0, "MiddleName", "N/A");
                tSpouseGender.Value = (string)DataAccess.GetData(dt, 0, "Gender", "N/A");
                tSpouseCitizenship.Value = (string)DataAccess.GetData(dt, 0, "Citizenship", "N/A");

                bdate = (string)DataAccess.GetData(dt, 0, "BirthDay", "");
                if (!string.IsNullOrEmpty(bdate))
                {
                    date = DateTime.Parse(bdate);
                    bdate = date.ToString("yyyy-MM-dd");
                }

                tSpouseBirthDate.Value = bdate;
                tSpouseBirthPlace.Value = (string)DataAccess.GetData(dt, 0, "BirthPlace", "N/A");
                tSpouseCellphoneNo.Value = (string)DataAccess.GetData(dt, 0, "CellNo", "N/A");
                tSpouseEmailAdd.Value = (string)DataAccess.GetData(dt, 0, "EmailAddress", "N/A");
                tSpouseFBAccount.Value = (string)DataAccess.GetData(dt, 0, "FBAccount", "N/A");


                //COMMENTED 06 - 13 - 2023 : CHANGE REQUEST  https://direcbti.monday.com/boards/4462005457/pulses/4576973637?asset_id=908456598 --%>
                //tSpouseTIN.Text = (string)DataAccess.GetData(dt, 0, "TIN", "N/A");
                tSpouseTIN2.Text = (string)DataAccess.GetData(dt, 0, "TIN", "N/A");


                tSpouseSSSNo.Value = (string)DataAccess.GetData(dt, 0, "SSSNo", "N/A");
                tSpouseGSISNo.Value = (string)DataAccess.GetData(dt, 0, "GSISNo", "N/A");
                tSpousePagibigNo.Value = (string)DataAccess.GetData(dt, 0, "PagibiNo", "N/A");
                tSpousePosition.Value = (string)DataAccess.GetData(dt, 0, "Position", "N/A");
                tSpouseYearsofService.Value = (string)DataAccess.GetData(dt, 0, "YearsService", "0");
                tSpouseOfficeTelNo.Value = (string)DataAccess.GetData(dt, 0, "OfficeTelNo", "N/A");
                tSpouseFAXNo.Value = (string)DataAccess.GetData(dt, 0, "FaxNo", "N/A");
                tSpouseEmpBusinessName.Value = (string)DataAccess.GetData(dt, 0, "EmpBusName", "N/A");
                tSpouseEmpBusinessAddress.Value = (string)DataAccess.GetData(dt, 0, "EmpBusAdd", "N/A");
                txtSPOAddress.Text = (string)DataAccess.GetData(dt, 0, "PresentAddress", "N/A");

                tCode = (string)DataAccess.GetData(dt, 0, "EmpStatus", "");

                if (ws.OLSTExist(tCode) == true)
                {
                    Session["tSpouseEmpStatus"] = tCode;
                    /*tSpouseEmpStatus.SelectedValue = ws.GetOLSTName(tCode);*/
                    tSpouseEmpStatus.SelectedValue = tCode;
                }
                else
                {
                    Session["tSpouseEmpStatus"] = "OTH";

                    tSpouseEmpStatus.SelectedValue = (tCode == "N/A" ? "" : tCode);
                }

                tCode = (string)DataAccess.GetData(dt, 0, "NatureEmp", "");

                if (ws.OLSTExist(tCode) == true)
                {
                    Session["tSpouseNatureEmp"] = tCode;
                    /*tSpouseNatureEmp.SelectedValue = ws.GetOLSTName(tCode);*/
                    tSpouseNatureEmp.SelectedValue = tCode;
                }
                else
                {
                    Session["tSpouseNatureEmp"] = "";

                    tSpouseNatureEmp.SelectedValue = "";
                }

                ShowSpouseBusiness();
                //############################## END CRD1 ################################

                //############################# temp_TABLE ################################

                //dt = ws.Select($"sp_EditSPACBDependent '{Code}'", "dtSPACBDependent", "Addon").Tables["dtSPACBDependent"];
                //Session["dtSPACBDependent"] = dt;

                //dt = ws.Select($"sp_EditSPACBList '{Code}'", "dtListSPACoBorrower", "Addon").Tables["dtListSPACoBorrower"];
                //Session["dtListSPACoBorrower"] = dt;

                hana.Execute($"CALL sp_EditSPACBDependent ('{Code}',{(int)Session["UserID"]})", hana.GetConnection("SAOHana"));

                //foreach (DataRow dr in ws.Select($"sp_EditSPACBList '{Code}',{(int)Session["UserID"]}", "dtListSPACoBorrower", "Addon").Tables["dtListSPACoBorrower"].Rows)
                //{
                //    DataRow dr1 = dt1.NewRow();
                //    dr1[0] = int.Parse(dr["ID"].ToString());
                //    dr1[1] = dr["Relationship"].ToString();
                //    dr1[2] = $"{dr["LastName"].ToString()}, {dr["FirstName"].ToString()} {dr["MiddleName"].ToString()[0]}";
                //    dr1[3] = dr["Gender"].ToString();
                //    dr1[4] = dr["Email"].ToString();
                //    dt1.Rows.Add(dr1);
                //}
                dt = hana.GetData($"cALL sp_EditSPACBList ('{Code}',{(int)Session["UserID"]})", hana.GetConnection("SAOHana"));

                //2023-06-18 : CHANGE TO ViewState
                //Session["gvSPACoBorrower"] = dt;
                //LoadData(gvSPACoBorrower, "gvSPACoBorrower");
                ViewState["gvSPACoBorrower"] = dt;
                LoadDataViewState(gvSPACoBorrower, "gvSPACoBorrower");

                //2023-06-18 : CHANGE TO VIEWSTATE
                //dt = (DataTable)Session["dtDependent"];
                dt = (DataTable)ViewState["dtDependent"];

                foreach (DataRow dr in hana.GetData($@"SELECT ""Name"",""Age"",""Relationship"" FROM ""CRD2"" WHERE ""DependentType"" = 'B' AND ""CardCode"" = '{Code}'", hana.GetConnection("SAOHana")).Rows)
                {
                    DataRow adr = dt.NewRow();

                    adr[0] = Convert.ToInt32(dt.Rows.Count + 1);
                    adr[1] = dr["Name"].ToString();
                    adr[2] = Convert.ToUInt64(dr["Age"]);
                    adr[3] = dr["Relationship"].ToString();
                    dt.Rows.Add(adr);
                }


                //2023-06-18 : CHANGE TO VIEWSTATE
                //Session["dtDependent"] = dt;
                //LoadData(gvDependent, "dtDependent"); 
                ViewState["dtDependent"] = dt;
                LoadDataViewState(gvDependent, "dtDependent");


                dt = (DataTable)Session["dtSPADependent"];

                foreach (DataRow dr in hana.GetData($@"SELECT ""Name"",""Age"",""Relationship"" FROM ""CRD2"" WHERE ""DependentType"" = 'S' AND ""CardCode"" = '{Code}'", hana.GetConnection("SAOHana")).Rows)
                {
                    DataRow adr = dt.NewRow();

                    adr[0] = Convert.ToInt32(dt.Rows.Count + 1);
                    adr[1] = dr["Name"].ToString();
                    adr[2] = Convert.ToUInt64(dr["Age"]);
                    adr[3] = dr["Relationship"].ToString();
                    dt.Rows.Add(adr);
                }
                Session["dtSPADependent"] = dt;
                LoadData(gvSPADependent, "dtSPADependent");


                //2023-06-18 : CHANGE TO ViewState
                //dt = (DataTable)Session["dtBankAccount"];
                dt = (DataTable)ViewState["dtBankAccount"];

                foreach (DataRow dr in hana.GetData($@"SELECT ""Bank"",""Branch"",""AcctType"",""AcctNo"",IFNULL(""AvgDailyBal"",0) ""AvgDailyBal"",IFNULL(""PresentBal"",0) ""PresentBal"" FROM ""CRD3"" WHERE ""CardCode"" = '{Code}'", hana.GetConnection("SAOHana")).Rows)
                {
                    DataRow adr = dt.NewRow();

                    adr[0] = Convert.ToInt32(dt.Rows.Count + 1);
                    adr[1] = dr["Bank"].ToString();
                    adr[2] = dr["Branch"].ToString();
                    adr[3] = dr["AcctType"].ToString();
                    adr[4] = dr["AcctNo"].ToString();
                    adr[5] = Convert.ToUInt64(dr["AvgDailyBal"]);
                    adr[6] = Convert.ToUInt64(dr["PresentBal"]);
                    dt.Rows.Add(adr);
                }

                //2023-06-18 : CHANGE TO ViewState
                //Session["dtBankAccount"] = dt;
                //LoadData(gvBankAccount, "dtBankAccount");
                ViewState["dtBankAccount"] = dt;
                LoadDataViewState(gvBankAccount, "dtBankAccount");


                //2023-06-18 : CHANGE TO ViewState
                //dt = (DataTable)Session["dtCharacterRef"];
                dt = (DataTable)ViewState["dtCharacterRef"];

                foreach (DataRow dr in hana.GetData($@"SELECT ""Name"",""Address"",""Email"",""TelNo"" FROM ""CRD4"" WHERE ""CardCode"" = '{Code}'", hana.GetConnection("SAOHana")).Rows)
                {
                    DataRow adr = dt.NewRow();

                    adr[0] = Convert.ToInt32(dt.Rows.Count + 1);
                    adr[1] = dr["Name"].ToString();
                    adr[2] = dr["Address"].ToString().Replace("amp;", "");
                    adr[3] = dr["Email"].ToString();
                    adr[4] = dr["TelNo"].ToString();
                    dt.Rows.Add(adr);
                }

                //2023-06-18 : CHANGE TO ViewState
                //Session["dtCharacterRef"] = dt;
                //LoadData(gvCharacterRef, "dtCharacterRef");
                ViewState["dtCharacterRef"] = dt;
                LoadDataViewState(gvCharacterRef, "dtCharacterRef");

                //############################## END temp_TABLE ################################

                //############################# CRD7 ################################
                dt = hana.GetData($@"SELECT * FROM CRD7 WHERE ""CardCode"" = '{Code}' AND ""BuyerType"" = '{ddlBusinessType.Text}'", hana.GetConnection("SAOHana"));
                if (dt.Rows.Count > 0)
                {
                    string qrycoowner = $@"Select A.""SpecialBuyerLink"" as ""CardCode"",B.""LastName"",B.""FirstName"",B.""MiddleName"",A.""BuyerType"" as ""SpecifiedBusinessType"" from ""CRD7"" A
                            INNER JOIN CRD1 B ON A.""SpecialBuyerLink"" = B.""CardCode""
                            where A.""CardCode"" = '{Session["CardCode"].ToString()}' AND B.""CardType"" = 'Buyer'";

                    dt = hana.GetData(qrycoowner, hana.GetConnection("SAOHana"));
                    //####GUARDIANSHIP/TRUSTEESHIP####
                    if (ddlBusinessType.Text == "Guardianship" || ddlBusinessType.Text == "Trusteeship")
                    {
                        ViewState["BuyerLink"] = (string)DataAccess.GetData(dt, 0, "SpecialBuyerLink", "");
                        txtspecbuyer.Value = ($@"{(string)DataAccess.GetData(dt, 0, "FirstName", "")} {(string)DataAccess.GetData(dt, 0, "MiddleName", "")} {(string)DataAccess.GetData(dt, 0, "LastName", "")}").Trim();
                    }
                    else if (ddlBusinessType.Text == "Co-ownership" || ddlBusinessType.Text == "Others")
                    {
                        Session["dtCoOwner"] = dt;
                        LoadData(gvcoownerlist, "dtCoOwner");
                    }
                }
                //############################## END CRD7 ################################

                DeleteTemporaryFIles();

                access = dtaccess.Select($"CodeEncrypt= 'SALES' or CodeEncrypt= 'SALES01'");
                if (!access.Any())
                {
                    btnSave.Visible = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "MsgBuyers_Hide();", true);
                }
                else
                {
                    btnSave.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "MsgBuyers_HideIT();", true);
                }

                GetBuyerAttachments(gvStandardDocumentRequirements, "FileUpload3", "lblFileName1", "btnPreview3", "btnRemove3", "RequiredFieldValidatorAttachments");

            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "warning");
            }

        }

        protected void tRented_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            string rbID = rb.ID;
            rb.Checked = true;

            if (rbID == "tRented")
            { /*tPerMonth.Disabled = false;*/ }
            else if (rbID == "tSPARented")
            { tSPAPerMonth.Disabled = false; }
            else if (rbID == "tOwned" || rbID == "tMortgaged" || rbID == "tLivingwRelatives")
            {
                //tPerMonth.Value = string.Empty;
                //tPerMonth.Disabled = true;
                tSPAPerMonth.Disabled = true;
            }
            else if (rbID == "tSPAOwned" || rbID == "tSPAMortgaged" || rbID == "tSPALivingwRelatives")
            {
                tSPAPerMonth.Value = string.Empty;
                tSPAPerMonth.Disabled = true;
                tSPAPerMonth.Disabled = true;
            }

            //STAY IN CURRENT TAB
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);
        }

        protected void btnYes_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                if ((string)Session["ConfirmType"] == "DelDependent")
                {
                    if ((string)Session["Buyers_Control"] == "gvDelete")
                    {
                        //2023-06-18 : CHANGE TO VIEWSTATE
                        //dt = (DataTable)Session["dtDependent"];
                        dt = (DataTable)ViewState["dtDependent"];

                        for (int i = dt.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = dt.Rows[i];

                            if (Convert.ToInt32(dr["ID"]) == (int)Session["Buyers_ID"])
                            { dr.Delete(); }
                        }

                        //2023-06-18 : CHANGE TO VIEWSTATE
                        //LoadData(gvDependent, "dtDependent");
                        LoadDataViewState(gvDependent, "dtDependent");
                    }
                    else if ((string)Session["Buyers_Control"] == "gvSPADelete")
                    {
                        //dt = (DataTable)Session["dtSPADependent"];

                        //for (int i = dt.Rows.Count - 1; i >= 0; i--)
                        //{
                        //    DataRow dr = dt.Rows[i];

                        //    if (Convert.ToInt32(dr["ID"]) == (int)Session["Buyers_ID"])
                        //    { dr.Delete(); }
                        //}
                        int UserID = (int)Session["UserID"];
                        int ID = (int)Session["SPACoBorrowerCount"];
                        if (ws.DeleteSPADependent(UserID, ID, (int)Session["Buyers_ID"]) == true)
                        {
                            dt = ws.select_temp_crd2(UserID, ID).Tables["select_temp_crd2"];
                            Session["dtSPADependent"] = dt;
                            LoadData(gvSPADependent, "dtSPADependent");
                        }

                    }
                    else if ((string)Session["Buyers_Control"] == "gvSPADelete")
                    {
                        dt = (DataTable)Session["dtSPADependent"];

                        for (int i = dt.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = dt.Rows[i];

                            if (Convert.ToInt32(dr["ID"]) == (int)Session["Buyers_ID"])
                            { dr.Delete(); }
                        }

                        LoadData(gvSPADependent, "dtSPADependent");
                    }
                }
                //else if ((string)Session["ConfirmType"] == "DelLink")
                //{
                //    dt = (DataTable)Session["dtDependent"];

                //    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                //    {
                //        DataRow dr = dt.Rows[i];

                //        if (Convert.ToInt32(dr["ID"]) == (int)Session["Buyers_ID"])
                //        { dr.Delete(); }
                //    }

                //    LoadData(gvDependent, "dtDependent");
                //}
                else if ((string)Session["ConfirmType"] == "CreateNew")
                { Response.Redirect(Request.RawUrl); }
                else if ((string)Session["ConfirmType"] == "DelBankAccount")
                {
                    //2023-06-18 : CHANGE TO ViewState
                    //dt = (DataTable)Session["dtBankAccount"];
                    dt = (DataTable)ViewState["dtBankAccount"];

                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = dt.Rows[i];

                        if (Convert.ToInt32(dr["ID"]) == (int)Session["Buyers_ID"])
                        { dr.Delete(); }
                    }

                    //2023-06-18 : CHANGE TO ViewState
                    //LoadData(gvBankAccount, "dtBankAccount");
                    LoadDataViewState(gvBankAccount, "dtBankAccount");
                }
                else if ((string)Session["ConfirmType"] == "DelCoOwner")
                {
                    dt = (DataTable)Session["dtCoOwner"];

                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = dt.Rows[i];

                        if (dr.RowState.ToString() != "Deleted")
                            if (dr["CardCode"].ToString() == Session["BuyersCo_ID"].ToString())
                            { dr.Delete(); }
                    }
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row.RowState == DataRowState.Deleted)
                        {
                            dt.Rows.Remove(row);
                            dt.AcceptChanges();
                            break;
                        }
                    }
                    Session["dtCoOwner"] = dt;
                    LoadData(gvcoownerlist, "dtCoOwner");
                }
                else if ((string)Session["ConfirmType"] == "DelOthers")
                {
                    dt = (DataTable)Session["dtOthers"];

                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = dt.Rows[i];

                        if (dr.RowState.ToString() != "Deleted")
                            if (dr["CardCode"].ToString() == Session["BuyersCo_ID"].ToString())
                            { dr.Delete(); }
                    }

                    LoadData(gvOthers, "dtOthers");
                }
                else if ((string)Session["ConfirmType"] == "DelCharacterRef")
                {
                    //2023-06-18 : CHANGE TO ViewState
                    //dt = (DataTable)Session["dtCharacterRef"];
                    dt = (DataTable)ViewState["dtCharacterRef"];

                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = dt.Rows[i];

                        if (Convert.ToInt32(dr["ID"]) == (int)Session["Buyers_ID"])
                        { dr.Delete(); }
                    }

                    //2023-06-18 : CHANGE TO ViewState
                    //LoadData(gvCharacterRef, "dtCharacterRef");
                    LoadDataViewState(gvCharacterRef, "dtCharacterRef");
                }
                else if ((string)Session["ConfirmType"] == "DelBuyer")
                {
                    string CardCode = (string)Session["CardCode"];


                    if (ws.BPDelete(CardCode, (int)Session["UserID"]) == true)
                    {
                        Session["CardCode"] = "";
                        LoadData(gvBuyers, "dtBuyers");
                        mtxtUniqueId.Text = $"Operation completed successfully.";
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "ShowSuccessAlert();", true);
                        //alertMsg("Operation completed successfully.", "success");
                    }
                    else { alertMsg("You are not allowed to delete this transaction.", "warning"); }
                }
                else if ((string)Session["ConfirmType"] == "AddSPACoBorrower")
                {
                    if (!string.IsNullOrEmpty(tRelationship.Value))
                    {
                        //dt = ws.Select($"SELECT UserID FROM temp_CRD5 WHERE UserID = {(int)Session["UserID"]} AND CardCode = '{(string)Session["CardCode"]}'", "temp_CRD5", "Addon").Tables["temp_CRD5"];

                        //int count = dt.Rows.Count;

                        //string date = DateTime.Now.ToShortDateString();

                        //if (tSPABirthDate.Value == "")
                        //{ tSPABirthDate.Value = date; }

                        //if (tSPADateIssued.Value == "")
                        //{ tSPADateIssued.Value = date; }
                        if (SPAAddUpdate.InnerText == "Add")
                            Session["SPACoBorrowerCount"] = GetCountID("ID");

                        int UserID = (int)Session["UserID"];
                        int ID = (int)Session["SPACoBorrowerCount"];

                        string SPAHomeOwnership = "Owned";
                        if (tSPAOwned.Checked == true)
                        { SPAHomeOwnership = "Owned"; }
                        else if (tSPAMortgaged.Checked == true)
                        { SPAHomeOwnership = "Mortgaged"; }
                        else if (tSPALivingwRelatives.Checked == true)
                        { SPAHomeOwnership = "LivingwRelatives"; }
                        else if (tSPARented.Checked == true)
                        { SPAHomeOwnership = tSPAPerMonth.Value; }

                        string bdate = "";

                        if (!string.IsNullOrEmpty(tSPABirthDate.Value))
                        { bdate = Convert.ToDateTime(tSPABirthDate.Value).ToString("yyyy-MM-dd"); }

                        UserID = (int)Session["UserID"];
                        if (ws.DeleteListSPA(UserID, ID) == true)
                        {
                            if (ws.insert_temp__crd5(UserID,   //1
                                                    ID,  //2

                                                    //2023-06-08 : REMOVE CONDITION; ALWAYS SPA
                                                    //tSPA.Checked,  //3
                                                    //tCoBorrower.Checked,  //4

                                                    true,
                                                    false,

                                                    tRelationship.Value, //5
                                                    tSPALastName.Value,  //6
                                                    tSPAFirstName.Value, //7
                                                    tSPAMiddleName.Value, //8
                                                    tSPAGender.Value,  //9
                                                    tSPACitizenship.Value, //10
                                                    bdate, //11
                                                    tSPABirthPlace.Value, //12
                                                    tSPACellphoneNo.Value,  //13
                                                    tSPAHomeTelNo.Value, //14
                                                    tSPAEmailAdd.Value, //15
                                                    tSPAFBAccount.Value,  //16
                                                    tSPATIN.Value,  //17
                                                    tSPASSSNo.Value, //18
                                                    tSPAGSISNo.Value, //19
                                                    tSPAPagibigNo.Value, //20
                                                    tSPAPresentAddress.Value, //21 
                                                                              //tSPAPermanent.Value, //22
                                                    ddPermCountry.SelectedValue,
                                                    tSPAPosition.Value, //23
                                                                        //int.Parse(tSPAYearsofService.Value), //24
                                                    0,
                                                    tSPAOfficeTelNo.Value,  //25
                                                    tSPAFAXNo.Value,  //26
                                                    SPAHomeOwnership,  //27
                                                    int.Parse(tSPAYearsOfStay.Value), //28
                                                    tSPAEmpBusinessName.Value, //29
                                                    tSPAEmpBusinessAddress.Value, //30
                                                    tSPAEmploymentStatus.SelectedValue,  //31
                                                    tSPANatureEmployment.SelectedValue, //32
                                                    tSPACivilStatus.SelectedValue,
                                                    lblSPAFileName.Text) == true)   //33 

                            {
                                //dt = ws.Select($"SELECT MAX(ISNULL(ID,0)) + 1 [ID] FROM temp_CRD5 WHERE UserID = {(int)Session["UserID"]}", "MaxID", "Addon").Tables["MaxID"];
                                //int count = GetCountID("ID");

                                if (SPAAddUpdate.InnerText != "Add")
                                    Session["SPACoBorrowerCount"] = GetCountID("ID");

                                dt = ws.select_temp_crd5(UserID).Tables["select_temp_crd5"];
                                ClearSPACoBorrower();

                                //2023-06-18 : CHANGE TO ViewState
                                //Session["gvSPACoBorrower"] = dt;
                                //LoadData(gvSPACoBorrower, "gvSPACoBorrower");
                                ViewState["gvSPACoBorrower"] = dt;
                                LoadDataViewState(gvSPACoBorrower, "gvSPACoBorrower");
                            }
                        }

                        //DataRow dr = dt.NewRow();
                        //dr[0] = ID;
                        //dr[1] = tSPA.Checked;
                        //dr[2] = tCoBorrower.Checked;
                        //dr[3] = tRelationship.Value;
                        //dr[4] = tSPALastName.Value;
                        //dr[5] = tSPAFirstName.Value;
                        //dr[6] = tSPAMiddleName.Value;
                        //dr[7] = tSPAGender.Value;
                        //dr[8] = tSPACitizenship.Value;
                        //dr[9] = Convert.ToDateTime(tSPABirthDate.Value).ToShortDateString();
                        //dr[10] = tSPABirthPlace.Value;
                        //dr[11] = tSPACellphoneNo.Value;
                        //dr[12] = tSPAHomeTelNo.Value;
                        //dr[13] = tSPAEmailAdd.Value;
                        //dr[14] = tSPAFBAccount.Value;
                        //dr[15] = tSPATIN.Value;
                        //dr[16] = tSPASSSNo.Value;
                        //dr[17] = tSPAGSISNo.Value;
                        //dr[18] = tSPAPagibigNo.Value;
                        //dr[19] = tSPAPresentAddress.Value;
                        //dr[20] = tSPAPermanent.Value;
                        //dr[21] = tSPAPosition.Value;
                        //dr[22] = int.Parse(tSPAYearsofService.Value);
                        //dr[23] = tSPAOfficeTelNo.Value;
                        //dr[24] = tSPAFAXNo.Value;
                        //dr[25] = SPAHomeOwnership;
                        //dr[26] = int.Parse(tSPAYearsOfStay.Value);
                        //dr[27] = tSPACTC.Value;
                        //dr[28] = Convert.ToDateTime(tSPADateIssued.Value);
                        //dr[29] = tSPAEmpBusinessName.Value;
                        //dr[30] = tSPAEmpBusinessAddress.Value;
                        //dr[31] = tSPAEmploymentStatus.Value;
                        //dr[32] = tSPANatureEmployment.Value;
                        //dr[33] = tSPACivilStatus.Value;
                        //dt.Rows.Add(dr);
                        //Session["dtListSPACoBorrower"] = dt;



                        //DataRow dr1 = dt.NewRow();
                        //dr1[0] = ID;
                        //dr1[1] = tRelationship.Value;
                        //dr1[2] = $"{tSPALastName.Value}, {tSPAFirstName.Value} {tSPAMiddleName.Value}";
                        //dr1[3] = tSPAGender.Value;
                        //dr1[4] = tSPAEmailAdd.Value;
                        //dt.Rows.Add(dr1);


                        //DataTable dt1 = new DataTable();
                        //dt1 = (DataTable)Session["dtSPACBDependent"];

                        //for (int i = 0; i < gvSPADependent.Rows.Count; i++)
                        //{
                        //    DataRow dr3 = dt1.NewRow();
                        //    dr3[0] = count;
                        //    dr3[1] = i;
                        //    dr3[2] = gvSPADependent.Rows[i].Cells[0].Text;
                        //    dr3[3] = int.Parse(gvSPADependent.Rows[i].Cells[1].Text);
                        //    dr3[4] = gvSPADependent.Rows[i].Cells[2].Text; ;
                        //    dt1.Rows.Add(dr3);
                        //}
                        //Session["dtSPACBDependent"] = dt1;

                        //count++;


                    }
                    else { alertMsg("Please fill up SPA &/or Co-Borrower details.", "warning"); }
                    ScriptManager.RegisterStartupScript(this, GetType(), "hide", "$('#MsgItems').modal('hide');;", true);
                }
                else if ((string)Session["ConfirmType"] == "DelSPACoBorrower")
                {
                    //2023-06-18: COMMENTED FOR NEW SPA PROCESS
                    //int ID = (int)Session["SPACB_ID"];

                    //int UserID = (int)Session["UserID"];
                    //if (ws.DeleteListSPA(UserID, ID) == true && ws.DeleteSPADependentByID(UserID, ID) == true)
                    //{
                    //    dt = ws.select_temp_crd5(UserID).Tables["select_temp_crd5"];
                    //    Session["gvSPACoBorrower"] = dt;
                    //    LoadData(gvSPACoBorrower, "gvSPACoBorrower");
                    //    ClearSPACoBorrower();
                    //    Session["SPACoBorrowerCount"] = GetCountID("ID");
                    //}

                    //2023-06-18 : CHANGE TO ViewState
                    //dt = (DataTable)Session["gvSPACoBorrower"];
                    dt = (DataTable)ViewState["gvSPACoBorrower"];

                    dt.Rows.RemoveAt(int.Parse(Session["SPACB_ID"].ToString()));
                    gvSPACoBorrower.DataSource = dt;
                    gvSPACoBorrower.DataBind();

                    //2023-06-18 : CHANGE TO ViewState
                    //Session["gvSPACoBorrower"] = dt;
                    //LoadData(gvSPACoBorrower, "gvSPACoBorrower");
                    ViewState["gvSPACoBorrower"] = dt;
                    LoadDataViewState(gvSPACoBorrower, "gvSPACoBorrower");


                    //dt = (DataTable)Session["dtListSPACoBorrower"];

                    //for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    //{
                    //    DataRow dr = dt.Rows[i];

                    //    if (Convert.ToInt32(dr["ID"]) == (int)Session["SPACB_ID"])
                    //    { dr.Delete(); }
                    //}

                    //dt = (DataTable)Session["dtSPACBDependent"];

                    //for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    //{
                    //    DataRow dr = dt.Rows[i];

                    //    if (Convert.ToInt32(dr["ID"]) == (int)Session["SPACB_ID"])
                    //    { dr.Delete(); }
                    //}

                    //dt = (DataTable)Session["gvSPACoBorrower"];

                    //for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    //{
                    //    DataRow dr = dt.Rows[i];

                    //    if (Convert.ToInt32(dr["ID"]) == (int)Session["SPACB_ID"])
                    //    { dr.Delete(); }
                    //}

                    //LoadData(gvSPACoBorrower, "gvSPACoBorrower");
                }
                else if ((string)Session["ConfirmType"] == "SaveUpdateBP")
                {

                    //string HomeOwnership = "Owned";
                    //if (tOwned.Checked == true)
                    //{ HomeOwnership = "Owned"; }
                    //else if (tMortgaged.Checked == true)
                    //{ HomeOwnership = "Mortgaged"; } 
                    //else if (tLivingwRelatives.Checked == true)
                    //{ HomeOwnership = "LivingwRelatives"; }
                    //else if (tRented.Checked == true)
                    //{ HomeOwnership = tPerMonth.Value; }

                    //string SPAHomeOwnership = "Owned";
                    //if (tOwned.Checked == true)
                    //{ SPAHomeOwnership = "Owned"; }
                    //else if (tMortgaged.Checked == true)
                    //{ SPAHomeOwnership = "Mortgaged"; }
                    //else if (tLivingwRelatives.Checked == true)
                    //{ SPAHomeOwnership = "LivingwRelatives"; }
                    //else if (tRented.Checked == true)
                    //{ SPAHomeOwnership = tSPAPerMonth.Value; }

                    //string date = DateTime.Now.ToShortDateString();

                    //if (tBirthDate.Value == "")
                    //{ tBirthDate.Value = date; }
                    //if (tDateIssued.Value == "")
                    //{ tDateIssued.Value = date; }
                    //if (tSpouseBirthDate.Value == "")
                    //{ tSpouseBirthDate.Value = date; }
                    //if (tSpouseDateIssued.Value == "")
                    //{ tSpouseDateIssued.Value = date; }
                    //if (tSPABirthDate.Value == "")
                    //{ tSPABirthDate.Value = date; }
                    //if (tSPADateIssued.Value == "")
                    //{ tSPADateIssued.Value = date; }

                    bool IsUpdate;
                    string CardCode;

                    if (lblSave.InnerText == "Save")
                    {
                        IsUpdate = false;
                        CardCode = (int.Parse(ws.GetAutoKey(1, "C")) + (int)Session["UserID"]).ToString();

                        //BLOCK ADDING WHEN TIN ALREADY EXIST
                        //string qry = $@"SELECT ""CardCode"" FROM ""CRD1"" where ""TIN"" = '{tTIN.Value}'";
                        if (!String.IsNullOrWhiteSpace(tTIN.Text))
                        {
                            string qry = $@"SELECT ""CardCode"" FROM ""CRD1"" where ""TIN"" = '{tTIN.Text}'  AND  ""CardType"" = 'Buyer'";
                            dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
                            if (dt.Rows.Count > 0)
                            {
                                alertMsg("Buyer with the same TIN already exists", "error");
                                return;
                            }
                        }
                    }
                    else
                    {
                        IsUpdate = true;
                        CardCode = (string)Session["CardCode"];
                    }

                    string oEmpStatus = tEmpStatus.SelectedValue == "---Select Employment Status---" ? "" : tEmpStatus.SelectedValue;

                    if (oEmpStatus == "OTH")
                    { oEmpStatus = tEmpStatus.SelectedValue; }

                    string oNatureEmployment = tNatureEmployment.SelectedValue == "---Select Nature of Employment---" ? "" : tNatureEmployment.SelectedValue;

                    if (oNatureEmployment == "OTH")
                    { oNatureEmployment = tNatureEmployment.SelectedValue; }

                    string oCivilStatus = tCivilStatus.SelectedValue == "---Select Civil Status---" ? "" : tCivilStatus.SelectedValue;

                    if (oCivilStatus == "OTH")
                    { oCivilStatus = tCivilStatus.SelectedValue; }

                    string oSpouseEmpStatus = (string)Session["tSpouseEmpStatus"] == null ? "" : (string)Session["tSpouseEmpStatus"];

                    if (oSpouseEmpStatus == "OTH")
                    { oSpouseEmpStatus = tSpouseEmpStatus.SelectedValue; }

                    string oSpouseNatureEmp = (string)Session["tSpouseNatureEmp"] == null ? "" : (string)Session["tSpouseNatureEmp"];

                    if (oSpouseNatureEmp == "OTH")
                    { oSpouseNatureEmp = tSpouseNatureEmp.SelectedValue; }

                    string oRelationship = (string)Session["tRelationship"] == null ? "" : (string)Session["tRelationship"];

                    if (oRelationship == "OTH")
                    { oRelationship = tRelationship.Value; }

                    string oSPAEmploymentStatus = (string)Session["tSPAEmploymentStatus"] == "---Select Employment Status---" ? "" : (string)Session["tSPAEmploymentStatus"];

                    if (oSPAEmploymentStatus == "OTH")
                    { oSPAEmploymentStatus = tSPAEmploymentStatus.SelectedValue; }

                    string oSPANatureEmployment = (string)Session["tSPANatureEmployment"] == "---Select Nature of Employment---" ? "" : (string)Session["tSPANatureEmployment"];

                    if (oSPANatureEmployment == "OTH")
                    { oSPANatureEmployment = tSPANatureEmployment.SelectedValue; }

                    string oSPACivilStatus = (string)Session["tSPACivilStatus"] == null ? "" : (string)Session["tSPACivilStatus"];

                    if (oSPACivilStatus == "OTH")
                    { oSPACivilStatus = tSPACivilStatus.SelectedValue; }


                    //if ((string)Session["tTypeOfId"] == "OTH")
                    //{
                    Session["tTypeOfId"] = tTypeOfId.SelectedValue;
                    //}
                    //if ((string)Session["tTypeOfId2"] == "OTH")
                    //{
                    Session["tTypeOfId2"] = tTypeOfId2.SelectedValue;
                    //}
                    //if ((string)Session["tTypeOfId3"] == "OTH")
                    //{
                    Session["tTypeOfId3"] = tTypeOfId3.SelectedValue;
                    //}
                    //if ((string)Session["tTypeOfId4"] == "OTH")
                    //{
                    Session["tTypeOfId4"] = tTypeOfId4.SelectedValue;
                    //}
                    if ((string)Session["SpecialInstructions"] == "OTH")
                    {
                        Session["SpecialInstructions"] = tSpecialInstructions.Value;
                    }

                    //New Fields
                    //OCRD
                    string newcardcode = ws.GetAutoKey(1, "G");
                    string PresentPostalCode = txtPresPostal.Text;
                    string PermanentPostalCode = txtPermPostal.Text;
                    string _ddPresCountry = ddPreCountry.SelectedValue.Contains("Select") ? "" : ddPreCountry.SelectedValue;
                    string _ddPermCountry = ddPermCountry.SelectedValue.Contains("Select") ? "" : ddPermCountry.SelectedValue;
                    string PermanentYrStay = txtPermYrStay.Text == "" ? "0" : txtPermYrStay.Text;
                    string Profession = txtProfession.Text;
                    string _ddSourceFunds = ddSourceFunds.SelectedValue.Contains("Select") ? "" : ddSourceFunds.SelectedValue;
                    string _ddOccupation = ddOccupation.SelectedValue.Contains("Select") ? "" : ddOccupation.SelectedValue;
                    string _ddMonthlyIncome = ddMonthlyIncome.SelectedValue.Contains("Select") || ddMonthlyIncome.SelectedValue == "" ? "" : ddMonthlyIncome.SelectedValue;
                    string PresentYrStay = txtPresYrStay.Text == "" ? "0" : txtPresYrStay.Text;
                    //string Comaker = txtComaker.Value;
                    string Comaker = "";
                    string TaxClassification = ddTaxClass.SelectedValue;
                    //CRD1
                    string _ddEmpCountry = ddEmployCountry.SelectedValue.Contains("Select") ? "" : ddEmployCountry.SelectedValue;

                    //CRD5
                    string SpouseResidentialNo = txtSPOAddress.Text;
                    //string SPAHomeTelNo = txtSPOTelNo.Text;

                    //2023-06-16: FIXED FIELD, SHOULD BE SPOUSE COUNTRY INSTEAD OF EMPLOYEE COUNTRY
                    //string _ddSPOBusCountry = ddEmployCountry.SelectedValue.Contains("Select") ? "" : ddEmployCountry.SelectedValue;
                    string _ddSPOBusCountry = ddSPOBusCountry.SelectedValue.Contains("Select") ? "" : ddSPOBusCountry.SelectedValue;


                    string CertifyCompleteName = txtCertifyCompleteName.Value;
                    string CertifyDate = txtCertifyDate.Value;

                    string AuthorizedPersonAddress = textAuthorizedPersonAddress.Value;
                    string AuthorizedPersonStreet = txtAuthorizedPersonStreet.Value;
                    string AuthorizedPersonSubdivision = txtAuthorizedPersonSubdivision.Value;
                    string AuthorizedPersonBarangay = txtAuthorizedPersonBarangay.Value;
                    string AuthorizedPersonCity = txtAuthorizedPersonCity.Value;
                    string AuthorizedPersonProvince = txtAuthorizedPersonProvince.Value;

                    string PresentStreet = txtPresentStreet.Text;
                    string PresentSubdivision = txtPresentSubdivision.Text;
                    string PresentBarangay = txtPresentBarangay.Text;
                    string PresentCity = txtPresentCity.Text;
                    string PresentProvince = txtPresentProvince.Text;
                    string PermanentStreet = txtPermanentStreet.Text;
                    string PermanentSubdivision = txtPermanentSubdivision.Text;
                    string PermanentBarangay = txtPermanentBarangay.Text;
                    string PermanentCity = txtPermanentCity.Text;
                    string PermanentProvince = txtPermanentProvince.Text;
                    string SpecifiedBusinessType = txtSpecifyBusiness.Text;
                    string SpecialBuyerLink = ViewState["BuyerLink"].ToString();
                    string SECCORIDNo = txtSECCORIDNo.Text;
                    bool Conforme = CBconforme.Checked;

                    string TINNo = ddlBusinessType.SelectedValue != "Corporation" ? tTIN.Text : tTINCorp.Text;

                    string ProofOfBillingAttachment = "";
                    string ValidId1Attachment = "";
                    string ValidId2Attachment = "";
                    string ProofOfIncomeAttachment = "";

                    string Religion = txtReligion.Text;

                    string SpecialBuyerRole = "";
                    if (ddlBusinessType.SelectedValue == "Guardianship" || ddlBusinessType.SelectedValue == "Trusteeship")
                    {
                        SpecialBuyerRole = rbGuardian.Checked ? rbGuardian.Text : rbGuardee.Text;
                    }

                    //2023-11-29 : DOCUMENTS REQUIREMENT CHANGED: MANDATORY SHOULD BE VALID ID 1 AND 2
                    //foreach (GridViewRow row in gvStandardDocumentRequirements.Rows)
                    //{
                    //    if (row.Cells[0].Text == "Proof of billing")
                    //    {
                    //        ProofOfBillingAttachment = ((Label)row.FindControl("lblFileName1")).Text;
                    //    }
                    //    else if (row.Cells[0].Text == "Valid ID 1")
                    //    {
                    //        ValidId1Attachment = ((Label)row.FindControl("lblFileName1")).Text;
                    //    }
                    //    else if (row.Cells[0].Text == "Valid ID 2")
                    //    {
                    //        ValidId2Attachment = ((Label)row.FindControl("lblFileName1")).Text;
                    //    }
                    //    else if (row.Cells[0].Text == "Proof of Income")
                    //    {
                    //        ProofOfIncomeAttachment = ((Label)row.FindControl("lblFileName1")).Text;
                    //    }
                    //}

                    //foreach (GridViewRow row in gvStandardDocumentRequirements2.Rows)
                    //{
                    //    if (row.Cells[0].Text == "Valid ID 2")
                    //    {
                    //        ValidId2Attachment = ((Label)row.FindControl("lblFileName3")).Text;
                    //    }
                    //}


                    foreach (GridViewRow row in gvStandardDocumentRequirements.Rows)
                    {

                        if (row.Cells[0].Text == "Valid ID 1")
                        {
                            ValidId1Attachment = ((Label)row.FindControl("lblFileName1")).Text;
                        }
                        else if (row.Cells[0].Text == "Valid ID 2")
                        {
                            ValidId2Attachment = ((Label)row.FindControl("lblFileName1")).Text;
                        }
                    }

                    foreach (GridViewRow row in gvStandardDocumentRequirements2.Rows)
                    {
                        if (row.Cells[0].Text == "Proof of billing")
                        {
                            ProofOfBillingAttachment = ((Label)row.FindControl("lblFileName3")).Text;
                        }
                        else if (row.Cells[0].Text == "Proof of Income")
                        {
                            ProofOfIncomeAttachment = ((Label)row.FindControl("lblFileName3")).Text;
                        }
                    }


                    string LastName = "";
                    string FirstName = "";
                    string MiddleName = "";


                    if (ddlBusinessType.Text.ToUpper() == "CORPORATION")
                    {
                        LastName = tLastName2.Text;
                        FirstName = tFirstName2.Text;
                        MiddleName = tMiddleName2.Text;
                    }
                    else
                    {
                        LastName = tLastName.Text;
                        FirstName = tFirstName.Text;
                        MiddleName = tMiddleName.Text;
                    }


                    string ret = ws.BusinessPartner(CardCode, oNatureEmployment, //2
                                                    (string)Session["tTypeOfId"] == "---Select Type of ID---" ? "" : (string)Session["tTypeOfId"], tIDNo.Text, //4
                                                    (string)Session["SalesAgent"] == null ? "" : (string)Session["SalesAgent"], LastName, //6
                                                    FirstName, MiddleName, //8
                                                    tGender.Value, tCitizenship.Value, //10
                                                    (string.IsNullOrEmpty(tBirthDate.Value) ? "1000-01-01" : tBirthDate.Value), tBirthPlace.Value, //12
                                                    tHomeTelNo.Value, tCellphoneNo.Value, //14
                                                    tEmpEmailAddress.Value, tFBAccount.Value, //16
                                                    /*tTIN.Value,*/ TINNo, tSSSNo.Text, //18
                                                    tGSISNo.Value, tPagibigNo.Text, //20
                                                    tPresentAddress.Text, tPermanent.Text, //22
                                                                                           //HomeOwnership, SystemClass.TextIsZero(tYearsOfStay.Value),
                                                    tEmpBusinessName.Value, tEmpBusinessAddress.Value, //24
                                                    tPosition.Value, SystemClass.TextIsZero(tYearsofService.Value), //26
                                                    tOfficeTelNo.Value, tFAXNo.Value, //28
                                                    oEmpStatus, oNatureEmployment, //30
                                                    oCivilStatus, tSpouseLastName.Value, //32
                                                    tSpouseFirstName.Value, tSpouseMiddleName.Value, //34
                                                    tSpouseGender.Value, tSpouseCitizenship.Value, //36
                                                    (string.IsNullOrEmpty(tSpouseBirthDate.Value) ? "1000-01-01" : tSpouseBirthDate.Value), tSpouseBirthPlace.Value, //38
                                                    tSpouseCellphoneNo.Value, tSpouseEmailAdd.Value, //40


                                                    //COMMENTED 06 - 13 - 2023 : CHANGE REQUEST  https://direcbti.monday.com/boards/4462005457/pulses/4576973637?asset_id=908456598 --%>
                                                    //tSpouseFBAccount.Value, tSpouseTIN.Value, //42
                                                    tSpouseFBAccount.Value, tSpouseTIN2.Text, //42

                                                    tSpouseSSSNo.Value, tSpouseGSISNo.Value, //44
                                                    tSpousePagibigNo.Value, tSpousePosition.Value, //46
                                                    SystemClass.TextIsZero(tSpouseYearsofService.Value), tSpouseOfficeTelNo.Value, //48
                                                    tSpouseFAXNo.Value, tSpouseEmpBusinessName.Value, //50

                                                    //2023-06-16: GET DATA DIRECTLY FROM TEXTFIELD
                                                    //tSpouseEmpBusinessAddress.Value, oSpouseEmpStatus, //52
                                                    tSpouseEmpBusinessAddress.Value, tSpouseEmpStatus.SelectedValue, //52

                                                    //2023-06-16: GET DATA DIRECTLY FROM TEXTFIELD
                                                    //oSpouseNatureEmp, tRemarks.Value, //54
                                                    tSpouseNatureEmp.SelectedValue, tRemarks.Value,//54

                                                    //2023-06-18 : CHANGE TO ViewState
                                                    //(DataTable)Session["dtDependent"], (DataTable)Session["dtBankAccount"], //56
                                                    (DataTable)ViewState["dtDependent"], (DataTable)ViewState["dtBankAccount"], //56
                                                                                                                                //(DataTable)Session["dtCharacterRef"],//57
                                                    (DataTable)ViewState["dtCharacterRef"],//57

                                                    IsUpdate, (int)Session["UserID"], //59
                                                    ddlBusinessType.Text, lblFileName.Text, //61


                                                    //NEW FIELDS
                                                    PresentPostalCode, PermanentPostalCode,  //63
                                                    _ddPresCountry, _ddPermCountry, //65
                                                    (PermanentYrStay == "N/A" ? "0" : PermanentYrStay), Profession, //67
                                                    _ddSourceFunds, _ddOccupation, //69
                                                    _ddMonthlyIncome, //70
                                                                      //_ddEmpCountry,
                                                    SpouseResidentialNo, //71
                                                                         //SPAHomeTelNo,



                                                    //2023-06-27 : CHANGED TO EMP BUSINESS COUNTRY
                                                    //_ddSPOBusCountry, (PresentYrStay == "N/A" ? "0" : PresentYrStay), //73
                                                    _ddEmpCountry, (PresentYrStay == "N/A" ? "0" : PresentYrStay), //73


                                                    txtCompanyName.Text, Comaker, //75
                                                    TaxClassification, //76
                                                    (string)Session["tTypeOfId2"] == "---Select Type of ID---" ? "" : (string)Session["tTypeOfId2"], tIDNo2.Text,  //78
                                                    (string)Session["tTypeOfId3"] == "---Select Type of ID---" ? "" : (string)Session["tTypeOfId3"], tIDNo3.Text, //80
                                                    (string)Session["tTypeOfId4"] == "---Select Type of ID---" ? "" : (string)Session["tTypeOfId4"], tIDNo4.Text, //82
                                                    (string)Session["SpecialInstructions"] == null ? "" : (string)Session["SpecialInstructions"], txtBusinessPhoneNo.Value, //84
                                                    txtOtherSourceOfFund.Text //85
                                                    , CertifyCompleteName, CertifyDate //87
                                                    , txtOthers1.Text, txtOthers2.Text //89
                                                    , txtOthers3.Text, txtOthers4.Text //91
                                                    , AuthorizedPersonAddress, AuthorizedPersonStreet //93
                                                    , AuthorizedPersonSubdivision, AuthorizedPersonBarangay //95
                                                    , AuthorizedPersonCity, AuthorizedPersonProvince //97

                                                    , PresentStreet, PresentSubdivision //99
                                                    , PresentBarangay, PresentCity //101
                                                    , PresentProvince, PermanentStreet //103
                                                    , PermanentSubdivision, PermanentBarangay //105
                                                    , PermanentCity, PermanentProvince //107
                                                    , SpecifiedBusinessType, Conforme //109
                                                    , ProofOfBillingAttachment, ValidId1Attachment //111
                                                    , ValidId2Attachment, Religion //113
                                                    , SECCORIDNo, SpecialBuyerRole //115
                                                    , ViewState["Guid"].ToString(), ProofOfIncomeAttachment //117
                                                                                                            //2023-06-18 : CHANGE TO ViewState
                                                                                                            //, (DataTable)Session["gvSPACoBorrower"]
                                                    , (DataTable)ViewState["gvSPACoBorrower"], _ddSPOBusCountry
                                        );
                    string type = "error";
                    if (ret == "Operation completed successfully.")
                    {


                        moveTemporaryFilesToPermanent(gvStandardDocumentRequirements, "lblFileName1");
                        moveTemporaryFilesToPermanent(gvStandardDocumentRequirements2, "lblFileName3");

                        //2023-06-19 :  SAVING OF SPA FILES 
                        moveTemporaryFilesToPermanentSPA(gvSPACoBorrower);

                        ////SPECIAL BUYERS SAVING/UPDATING////  
                        if (IsUpdate)
                        {
                            hana.Execute($@"DELETE FROM CRD7 WHERE LOWER(""CardCode"") = '{CardCode.ToLower()}'; ", hana.GetConnection("SAOHana"));
                            newcardcode = CardCode;
                        }
                        //ADD GUARDIANSHIP/TRUSTEESHIP
                        if (ddlBusinessType.Text == "Guardianship" || ddlBusinessType.Text == "Trusteeship")
                        {



                            string qryCRD7 = $@"INSERT INTO CRD7
                                    (
                                    ""CardCode"",
                                    ""BuyerType"",
                                   -- ""FirstName"",
                                   -- ""MiddleName"",
                                   -- ""LastName"",
                                   -- ""Relationship"",
                                   -- ""Email"",
                                   -- ""MobileNo"",
                                   -- ""Address"",
                                   -- ""Residence"",
                                   -- ""ValidID"",
                                   -- ""ValidIDNo"",
                                    ""CreateDate"",
                                    ""SpecialBuyerLink"",
                                    ""SpecialBuyerRole"")
                                    VALUES(
                                    '{newcardcode}'
                                    , '{ddlBusinessType.Text}'
                                   -- , '{txtContactFName.Text}'
                                   -- , '{txtContactMName.Text}'
                                   -- , '{txtContactLName.Text}'
                                   -- , '{txtContactPersonPosition.Text}'
                                   -- , '{txtContactEmail.Text}'
                                   -- , '{txtContactMobile.Text}'
                                   -- , '{txtContactAddress.Text}'
                                   -- , '{txtContactResidence.Text}'
                                   -- , '{ddContactValidID.SelectedValue}'
                                   -- , '{txtContactValidIDNo.Text}'
                                    , '{DateTime.Now.ToString("yyyy-MM-dd")}'
                                    , '{ViewState["BuyerLink"].ToString()}'
                                    , '{(rbGuardian.Checked ? rbGuardian.Text : rbGuardee.Text)}'
                                    )";
                            if (hana.Execute(qryCRD7, hana.GetConnection("SAOHana")) == false)
                            {
                                ret = "Error.";
                            }
                            else
                            {
                                ret = "Operation completed successfully.";
                            }
                        }
                        else if (ddlBusinessType.Text == "Co-ownership" || ddlBusinessType.Text == "Others")
                        {
                            dt = (DataTable)Session["dtCoOwner"];
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row.RowState != DataRowState.Deleted)
                                {
                                    string qryCRD7 = $@"INSERT INTO CRD7
                                    (
                                    ""CardCode"",
                                    ""BuyerType"",
                                    --""FirstName"",
                                    --""MiddleName"",
                                    --""LastName"",
                                    --""Relationship"",
                                    --""Email"",
                                    --""MobileNo"",
                                    --""Address"",
                                    --""Residence"",
                                    --""ValidID"",
                                    --""ValidIDNo"",
                                    ""CreateDate"",
                                    ""SpecialBuyerLink"")
                                    VALUES(
                                    '{newcardcode}'
                                    , '{ddlBusinessType.Text}'
                                   --, '{row["FirstName"].ToString()}'
                                    --, '{row["MiddleName"].ToString()}'
                                    --, '{row["LastName"].ToString()}'
                                    --, ''
                                    --, ''
                                    --, ''
                                    --, ''
                                    --, ''
                                    --, ''
                                    --, ''
                                    , '{DateTime.Now.ToString("yyyy-MM-dd")}'
                                    , '{row["CardCode"].ToString()}')";
                                    if (hana.Execute(qryCRD7, hana.GetConnection("SAOHana")) == false)
                                    {
                                        ret = "Error.";
                                        break;
                                    }
                                    else
                                    {
                                        ret = "Operation completed successfully.";
                                    }
                                }
                            }
                        }
                        ////END OF SPECIAL BUYERS SAVING/UPDATING////


                        DataTable dtSAPBP = new DataTable();
                        dtSAPBP = hana.GetData($@"SELECT TOP 1 ""DocEntry"",""CardCode"",""CardName"",""CardFName"" FROM ""OCRD"" WHERE ""U_DreamsCustCode"" = '{CardCode}'", hana.GetConnection("SAPHana"));
                        SapHanaLayer company = new SapHanaLayer();
                        string SAPCardCode = "";


                        if (DataAccess.Exist(dtSAPBP))
                        {
                            int bpSeries = int.Parse(ConfigSettings.BPSeries);
                            string clearingAccount = ConfigSettings.ClearingAccount;
                            int bpListNum = int.Parse(ConfigSettings.BPPriceList);
                            string PayTermsGrpCode = ConfigSettings.PayTermsGrpCode;
                            IDictionary<string, object> BusinessPartners = new Dictionary<string, object>()
                                {
                                { "CardName" ,  LastName + ", " +  FirstName},
                                { "CardForeignName" , LastName + ", " +  FirstName},
                                { "U_LName" , LastName},
                                { "U_FName" ,  FirstName},
                                { "U_MName" , MiddleName},
                                };

                            var json = DataHelper.JsonBuilder(BusinessPartners, new StringBuilder());

                            if (company.PATCH($@"BusinessPartners('{DataAccess.GetData(dtSAPBP, 0, "CardCode", "").ToString()}')", json))
                            {
                                //SapCardCode = company.ResultDescription;
                                SAPCardCode = DataAccess.GetData(dtSAPBP, 0, "CardCode", "").ToString();
                                ret = "Business Partner successfully updated.";
                            }
                            else
                            {
                                SAPCardCode = "";
                                ret = $"Error: ({company.ResultCode}) {company.ResultDescription}";
                            }
                        }




                        if (ret == "Operation completed successfully." || ret == "Business Partner successfully updated.")
                        {



                            string qry = $@"UPDATE OCRD SET ""Approved"" = 'N' WHERE ""CardCode"" = '{CardCode}'";
                            hana.Execute(qry, hana.GetConnection("SAOHana"));
                            lblSave.InnerText = "Save";
                            type = "success";
                            ClearAllData();
                            divCustomerCode.Visible = false;
                        }
                        else
                        {
                            alertMsg("Error on saving!", "error");
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "hide", "closeConfirmation();", true);
                        alertMsg(ret, type);
                    }
                    mtxtUniqueId.Text = ret;
                    DeleteTemporaryFIles();
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "ShowSuccessAlert();", true);
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "hide", "closeConfirmation();", true);
            }
            catch (Exception ex)
            { ScriptManager.RegisterStartupScript(this, GetType(), "hide", "closeConfirmation();", true); alertMsg(ex.Message, "error"); }
        }

        void ClearSPACoBorrower()
        {
            //tSPA.Checked = false;
            //tCoBorrower.Checked = false;
            tRelationship.Value = string.Empty;
            tSPALastName.Value = string.Empty;
            tSPAFirstName.Value = string.Empty;
            tSPAMiddleName.Value = string.Empty;
            tSPAGender.Value = string.Empty;
            tSPACitizenship.Value = string.Empty;
            tSPABirthDate.Value = string.Empty;
            tSPABirthPlace.Value = string.Empty;
            tSPACellphoneNo.Value = string.Empty;
            tSPAHomeTelNo.Value = string.Empty;
            tSPAEmailAdd.Value = string.Empty;
            tSPAFBAccount.Value = string.Empty;
            tSPATIN.Value = string.Empty;
            tSPASSSNo.Value = string.Empty;
            tSPAGSISNo.Value = string.Empty;
            tSPAPagibigNo.Value = string.Empty;
            tSPAPresentAddress.Value = string.Empty;
            //tSPAPermanent.Value = string.Empty;
            tSPAPosition.Value = string.Empty;
            tSPAYearsofService.Value = "0";
            tSPAOfficeTelNo.Value = string.Empty;
            tSPAFAXNo.Value = string.Empty;
            tSPAYearsOfStay.Value = "0";
            tSPAEmpBusinessName.Value = string.Empty;
            tSPAEmpBusinessAddress.Value = string.Empty;
            tSPAEmploymentStatus.SelectedValue = string.Empty;
            tSPANatureEmployment.SelectedValue = string.Empty;
            tSPACivilStatus.SelectedValue = string.Empty;
            //tRented_CheckedChanged(tOwned, EventArgs.Empty);
            LoadGridView("dtSPADependent");
            LoadData(gvSPADependent, "dtSPADependent");
            SPAAddUpdate.InnerText = "Add";
        }

        protected void Text_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;

            SystemClass.ToDecimal(txt.Text);

            double t1 = 0, t2 = 0;
            if (txt.ID == "tPBasicSalary" || txt.ID == "tSBasicSalary")
            {
                t1 = SystemClass.TextIsZero(tPBasicSalary.Text);
                t2 = SystemClass.TextIsZero(tSBasicSalary.Text);

                tTBasicSalary.Value = SystemClass.ToCurrency((t1 + t2).ToString());
            }
            else if (txt.ID == "tPAllowances" || txt.ID == "tSAllowances")
            {
                t1 = SystemClass.TextIsZero(tPAllowances.Text);
                t2 = SystemClass.TextIsZero(tSAllowances.Text);

                tTAllowances.Value = SystemClass.ToDecimal((t1 + t2).ToString());
            }
            else if (txt.ID == "tPCommissions" || txt.ID == "tSCommissions")
            {
                t1 = SystemClass.TextIsZero(tPCommissions.Text);
                t2 = SystemClass.TextIsZero(tSCommissions.Text);

                tTCommissions.Value = SystemClass.ToDecimal((t1 + t2).ToString());
            }
            else if (txt.ID == "tPRentalIncome" || txt.ID == "tSRentalIncome")
            {
                t1 = SystemClass.TextIsZero(tPRentalIncome.Text);
                t2 = SystemClass.TextIsZero(tSRentalIncome.Text);

                tTRentalIncome.Value = SystemClass.ToDecimal((t1 + t2).ToString());
            }
            else if (txt.ID == "tPRetainer" || txt.ID == "tSRetainer")
            {
                t1 = SystemClass.TextIsZero(tPRetainer.Text);
                t2 = SystemClass.TextIsZero(tSRetainer.Text);

                tTRetainer.Value = SystemClass.ToDecimal((t1 + t2).ToString());
            }
            else if (txt.ID == "tPOthers" || txt.ID == "tSOthers")
            {
                t1 = SystemClass.TextIsZero(tPOthers.Text);
                t2 = SystemClass.TextIsZero(tSOthers.Text);

                tTOthers.Value = SystemClass.ToDecimal((t1 + t2).ToString());
            }

            AutoCompute();
        }

        void AutoCompute()
        {
            tPBasicSalary.Text = SystemClass.ToDecimal(tPBasicSalary.Text);
            tPAllowances.Text = SystemClass.ToDecimal(tPAllowances.Text);
            tPCommissions.Text = SystemClass.ToDecimal(tPCommissions.Text);
            tPRentalIncome.Text = SystemClass.ToDecimal(tPRentalIncome.Text);
            tPRetainer.Text = SystemClass.ToDecimal(tPRetainer.Text);
            tPOthers.Text = SystemClass.ToDecimal(tPOthers.Text);

            tPTotal.Value = SystemClass.ToCurrency((double.Parse(tPBasicSalary.Text) + double.Parse(tPAllowances.Text)
                + double.Parse(tPCommissions.Text) + double.Parse(tPRentalIncome.Text)
                + double.Parse(tPRetainer.Text) + double.Parse(tPOthers.Text)).ToString());

            tSBasicSalary.Text = SystemClass.ToDecimal(tSBasicSalary.Text);
            tSAllowances.Text = SystemClass.ToDecimal(tSAllowances.Text);
            tSCommissions.Text = SystemClass.ToDecimal(tSCommissions.Text);
            tSRentalIncome.Text = SystemClass.ToDecimal(tSRentalIncome.Text);
            tSRetainer.Text = SystemClass.ToDecimal(tSRetainer.Text);
            tSOthers.Text = SystemClass.ToDecimal(tSOthers.Text);

            tSTotal.Value = SystemClass.ToCurrency((double.Parse(tSBasicSalary.Text) + double.Parse(tSAllowances.Text)
                + double.Parse(tSCommissions.Text) + double.Parse(tSRentalIncome.Text)
                + double.Parse(tSRetainer.Text) + double.Parse(tSOthers.Text)).ToString());

            tRequiredMonthly.Text = SystemClass.ToCurrency(tRequiredMonthly.Text);
            tMonthlyAmort.Value = SystemClass.ToCurrency(tMonthlyAmort.Value);

            tMITotal.Value = SystemClass.ToCurrency((double.Parse(tPTotal.Value) + double.Parse(tSTotal.Value)).ToString());

            tFood.Text = SystemClass.ToDecimal(tFood.Text);
            tLightWater.Text = SystemClass.ToDecimal(tLightWater.Text);
            tTelephoneBill.Text = SystemClass.ToDecimal(tTelephoneBill.Text);
            tTransportation.Text = SystemClass.ToDecimal(tTransportation.Text);
            tRent.Text = SystemClass.ToDecimal(tRent.Text);
            tEducation.Text = SystemClass.ToDecimal(tEducation.Text);
            tLoanAmort.Text = SystemClass.ToDecimal(tLoanAmort.Text);
            tMEOthers.Text = SystemClass.ToDecimal(tMEOthers.Text);

            tMETotal.Value = SystemClass.ToCurrency((double.Parse(tFood.Text) + double.Parse(tLightWater.Text)
                + double.Parse(tTelephoneBill.Text) + double.Parse(tTransportation.Text) + double.Parse(tRent.Text)
                + double.Parse(tEducation.Text) + double.Parse(tLoanAmort.Text) + double.Parse(tMEOthers.Text)).ToString());
        }

        void ClearAllData()
        {
            foreach (Control control in Page.Controls)
            {
                if (control is TextBox)
                {
                    TextBox txt = (TextBox)control;
                    txt.Text = string.Empty;
                }
            }
            ClearAll();
            ScriptManager.RegisterStartupScript(this, GetType(), "Clear", "ClearAll();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "SHowEmpTab()", "SHowEmpTab();", true);
            Session["CardCode"] = null;
            tIDNo.Enabled = false;
            Session["SPACoBorrowerCount"] = 0;
            ClearSPACoBorrower();
            tSpouseYearsofService.Value = "0";
            tYearsofService.Value = "0";
            //tYearsOfStay.Value = "0";
            FirstLoad();
        }

        protected void gvSPACoBorrower_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //2023-06-18 : CHANGE TO ViewState
            //LoadData(gvSPACoBorrower, "gvSPACoBorrower");
            LoadDataViewState(gvSPACoBorrower, "gvSPACoBorrower");
            gvSPADependent.PageIndex = e.NewPageIndex;
            gvSPADependent.DataBind();
        }

        protected void btnSPACoBorrower_ServerClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {

                //2023-06-18 : CHANGE TO ViewState
                //DataTable dt = (DataTable)Session["gvSPACoBorrower"];
                DataTable dt = (DataTable)ViewState["gvSPACoBorrower"];

                dt.Rows.Add(
                    Convert.ToInt32(dt.Rows.Count + 1),
                    tRelationship.Value.Trim().ToUpper(),
                    tSPALastName.Value.Trim().ToUpper(),
                    tSPAFirstName.Value.Trim().ToUpper(),
                    tSPAMiddleName.Value.Trim().ToUpper(),
                    tSPACivilStatus.Text.Trim().ToUpper(),
                    string.IsNullOrEmpty(tSPAYearsOfStay.Value) ? "0" : tSPAYearsOfStay.Value.Trim(),
                    string.IsNullOrEmpty(tSPAPresentAddress.Value) ? string.Empty : tSPAPresentAddress.Value.Trim().ToUpper(),
                    string.IsNullOrEmpty(tSPABirthDate.Value) ? string.Empty : tSPABirthDate.Value.Trim(),
                    string.IsNullOrEmpty(tSPABirthPlace.Value) ? string.Empty : tSPABirthPlace.Value.Trim().ToUpper(),
                    tSPAGender.Value,
                    tSPACitizenship.Value,
                    string.IsNullOrEmpty(tSPAEmailAdd.Value) ? string.Empty : tSPAEmailAdd.Value.Trim(),
                    string.IsNullOrEmpty(tSPAHomeTelNo.Value) ? string.Empty : tSPAHomeTelNo.Value.Trim(),
                    string.IsNullOrEmpty(tSPACellphoneNo.Value) ? string.Empty : tSPACellphoneNo.Value.Trim(),
                    string.IsNullOrEmpty(tSPAFBAccount.Value) ? string.Empty : tSPAFBAccount.Value.Trim(),
                    lblSPAFileName.Text.Trim()
                    );

                //2023-06-18 : CHANGE TO ViewState
                //Session["gvSPACoBorrower"] = dt;
                //gvSPACoBorrower.DataSource = (DataTable)Session["gvSPACoBorrower"];
                ViewState["gvSPACoBorrower"] = dt;
                gvSPACoBorrower.DataSource = (DataTable)ViewState["gvSPACoBorrower"];

                gvSPACoBorrower.DataBind();

                clearSPADetails();


                //Session["ExtBuyerRow"] = "1";
                //Session["ExtGv"] = "";

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "MsgSPACoBorrower_Hide();", true);

                //confirmation("Are you sure you want to add SPA / Co Borrower?", "AddSPACoBorrower");

                //2023-06-16 : COMMENTED FOR SPA CHANGES
                //confirmation("Are you sure you want to add SPA?", "AddSPACoBorrower");
            }
            //STAY IN CURRENT TAB
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);
        }




        void clearSPADetails()
        {
            txtSPAID.Value = string.Empty;
            tRelationship.Value = string.Empty;
            tSPALastName.Value = string.Empty;
            tSPAFirstName.Value = string.Empty;
            tSPAMiddleName.Value = string.Empty;
            tSPACivilStatus.Text = string.Empty;
            tSPAYearsOfStay.Value = string.Empty;
            tSPAPresentAddress.Value = string.Empty;
            tSPABirthDate.Value = string.Empty;
            tSPABirthPlace.Value = string.Empty;
            tSPAGender.SelectedIndex = 0;
            tSPACitizenship.SelectedIndex = 0;
            tSPAEmailAdd.Value = string.Empty;
            tSPAHomeTelNo.Value = string.Empty;
            tSPACellphoneNo.Value = string.Empty;
            tSPAFBAccount.Value = string.Empty;
            lblSPAFileName.Text = string.Empty;

        }

        protected void gvSPACBDelete_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            Session["SPACB_ID"] = Convert.ToInt32(GetID.CommandArgument);
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);
            confirmation("Are you sure you want to delete the selected SPA/Co-Borrower?", "DelSPACoBorrower");
        }

        protected void gvSPACBEdit_Click(object sender, EventArgs e)
        {

            //2023-06-18: ADJUSTMENTS ON NEW SPA PROCESS

            LinkButton GetID = (LinkButton)sender;
            int row = int.Parse(GetID.CommandArgument.ToString());

            txtSPAID.Value = gvSPACoBorrower.Rows[row].Cells[0].Text;

            tRelationship.Value = gvSPACoBorrower.Rows[row].Cells[1].Text;
            tSPALastName.Value = gvSPACoBorrower.Rows[row].Cells[2].Text;
            tSPAFirstName.Value = gvSPACoBorrower.Rows[row].Cells[3].Text;
            tSPAMiddleName.Value = gvSPACoBorrower.Rows[row].Cells[4].Text;
            tSPACivilStatus.Text = gvSPACoBorrower.Rows[row].Cells[5].Text;
            tSPAYearsOfStay.Value = string.IsNullOrEmpty(gvSPACoBorrower.Rows[row].Cells[6].Text) ? "0" : gvSPACoBorrower.Rows[row].Cells[6].Text; ;
            tSPAPresentAddress.Value = string.IsNullOrEmpty(gvSPACoBorrower.Rows[row].Cells[7].Text) ? string.Empty : gvSPACoBorrower.Rows[row].Cells[7].Text;
            tSPABirthDate.Value = string.IsNullOrEmpty(gvSPACoBorrower.Rows[row].Cells[8].Text) ? string.Empty : Convert.ToDateTime(gvSPACoBorrower.Rows[row].Cells[8].Text).ToString("yyyy-MM-dd");
            tSPABirthPlace.Value = string.IsNullOrEmpty(gvSPACoBorrower.Rows[row].Cells[9].Text) ? string.Empty : gvSPACoBorrower.Rows[row].Cells[9].Text;
            tSPAGender.Value = gvSPACoBorrower.Rows[row].Cells[10].Text;
            tSPACitizenship.Value = gvSPACoBorrower.Rows[row].Cells[11].Text;
            tSPAEmailAdd.Value = string.IsNullOrEmpty(gvSPACoBorrower.Rows[row].Cells[12].Text) ? string.Empty : gvSPACoBorrower.Rows[row].Cells[12].Text;
            tSPAHomeTelNo.Value = string.IsNullOrEmpty(gvSPACoBorrower.Rows[row].Cells[13].Text) ? string.Empty : gvSPACoBorrower.Rows[row].Cells[13].Text;
            tSPACellphoneNo.Value = string.IsNullOrEmpty(gvSPACoBorrower.Rows[row].Cells[14].Text) ? string.Empty : gvSPACoBorrower.Rows[row].Cells[14].Text;
            tSPAFBAccount.Value = string.IsNullOrEmpty(gvSPACoBorrower.Rows[row].Cells[15].Text) ? string.Empty : gvSPACoBorrower.Rows[row].Cells[15].Text;

            lblSPAFileName.Text = string.Empty;
            visibleDocumentButtons(false, btnSPAPreview, btnSPARemove);

            lblSPAFileName.Text = gvSPACoBorrower.Rows[row].Cells[16].Text;


            //2023-06-18 : VISIBLE FIELDS
            if (!string.IsNullOrEmpty(lblSPAFileName.Text))
            {
                visibleDocumentButtons(true, btnSPAPreview, btnSPARemove);
            }
            else
            {
                visibleDocumentButtons(false, btnSPAPreview, btnSPARemove);
            }


            spaTitle.InnerText = "Update SPA Details";

            btnSPACoBorrower.Visible = false;
            btnSPAUpdate.Visible = true;

            //2023-06-16 : CHANGE SEQUENCE OF LOADING DATA + OPENING MODAL--%>
            ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "MsgSPACoBorrower_Show();", true);

            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);



            #region oldCode
            //DataTable dt = new DataTable();
            ////if (SPAAddUpdate.InnerText == "Update")
            ////{
            ////    int UserID = (int)Session["UserID"];

            ////    string SPAHomeOwnership = "Owned";
            ////    if (tSPAOwned.Checked == true)
            ////    { SPAHomeOwnership = "Owned"; }
            ////    else if (tSPAMortgaged.Checked == true)
            ////    { SPAHomeOwnership = "Mortgaged"; }
            ////    else if (tSPALivingwRelatives.Checked == true)
            ////    { SPAHomeOwnership = "LivingwRelatives"; }
            ////    else if (tSPARented.Checked == true)
            ////    { SPAHomeOwnership = tSPAPerMonth.Value; }

            ////    string bdate = "";

            ////    if (!string.IsNullOrEmpty(tSPABirthDate.Value))
            ////    { bdate = Convert.ToDateTime(tSPABirthDate.Value).ToShortDateString(); }

            ////    if (ws.insert_temp__crd5(UserID, (int)Session["SPACoBorrowerCount"], tSPA.Checked, tCoBorrower.Checked, tRelationship.Value, tSPALastName.Value, tSPAFirstName.Value, tSPAMiddleName.Value,
            ////                            tSPAGender.Value, tSPACitizenship.Value, bdate, tSPABirthPlace.Value,
            ////                            tSPACellphoneNo.Value, tSPAHomeTelNo.Value, tSPAEmailAdd.Value, tSPAFBAccount.Value, tSPATIN.Value, tSPASSSNo.Value,
            ////                            tSPAGSISNo.Value, tSPAPagibigNo.Value, tSPAPresentAddress.Value, /*tSPAPermanent.Value*/ ddPermCountry.SelectedValue, tSPAPosition.Value, int.Parse(tSPAYearsofService.Value),
            ////                            tSPAOfficeTelNo.Value, tSPAFAXNo.Value, SPAHomeOwnership, int.Parse(tSPAYearsOfStay.Value),
            ////                            tSPAEmpBusinessName.Value, tSPAEmpBusinessAddress.Value, tSPAEmploymentStatus.SelectedValue, tSPANatureEmployment.SelectedValue, tSPACivilStatus.SelectedValue) == true)
            ////    {
            ////        dt = ws.select_temp_crd5(UserID).Tables["select_temp_crd5"];
            ////        ClearSPACoBorrower();
            ////        Session["gvSPACoBorrower"] = dt;
            ////        LoadData(gvSPACoBorrower, "gvSPACoBorrower");
            ////    }
            ////}
            //LinkButton GetID = (LinkButton)sender;
            //int ID = Convert.ToInt32(GetID.CommandArgument);
            //Session["SPACoBorrowerCount"] = ID;
            //SPAAddUpdate.InnerText = "Update";

            ////(DataTable)Session["dtListSPACoBorrower"], (DataTable)Session["dtSPACBDependent"]
            ////gvSPACoBorrower, dr.Delete(); 

            ////dt = (DataTable)Session["dtListSPACoBorrower"];

            //dt = hana.GetDataDS($@"SELECT * FROM ""temp_CRD5"" WHERE ""UserID"" = {(int)Session["UserID"]} AND ""ID"" = '{ID}'", hana.GetConnection("SAOHana")).Tables[0];

            //for (int i = dt.Rows.Count - 1; i >= 0; i--)
            //{
            //    DataRow dr = dt.Rows[i];
            //    if (Convert.ToInt32(dr["ID"]) == ID)
            //    {


            //        //2023-06-08 : REMOVE CONDITION; ALWAYS SPA 
            //        //if (Convert.ToBoolean(dr["SPA"]))
            //        //{
            //        //    tSPA.Checked = true;
            //        //}
            //        //else
            //        //{
            //        //    tSPA.Checked = false;
            //        //}
            //        //if (Convert.ToBoolean(dr["CoBorrower"]))
            //        //{
            //        //    tCoBorrower.Checked = true;
            //        //}
            //        //else
            //        //{
            //        //    tCoBorrower.Checked = false;
            //        //}
            //        //divUploadSPADocs.Visible = tSPA.Checked;
            //        divUploadSPADocs.Visible = true;


            //        tRelationship.Value = dr["Relationship"].ToString();
            //        tSPALastName.Value = dr["LastName"].ToString();
            //        tSPAFirstName.Value = dr["FirstName"].ToString();
            //        tSPAMiddleName.Value = dr["MiddleName"].ToString();
            //        tSPAGender.Value = dr["Gender"].ToString();
            //        tSPACitizenship.Value = dr["Citizenship"].ToString();

            //        string bdate = dr["BirthDate"].ToString();
            //        if (!string.IsNullOrEmpty(bdate))
            //        {
            //            DateTime oBDAY;
            //            oBDAY = DateTime.Parse(bdate);
            //            bdate = oBDAY.ToString("yyyy-MM-dd");
            //        }

            //        tSPABirthDate.Value = bdate;
            //        tSPABirthPlace.Value = dr["BirthPlace"].ToString();
            //        tSPACellphoneNo.Value = dr["CellNo"].ToString();
            //        tSPAHomeTelNo.Value = dr["HomeTelNo"].ToString();
            //        tSPAEmailAdd.Value = dr["Email"].ToString();
            //        tSPAFBAccount.Value = dr["FB"].ToString();
            //        tSPATIN.Value = dr["TIN"].ToString();
            //        tSPASSSNo.Value = dr["SSSNo"].ToString();
            //        tSPAGSISNo.Value = dr["GSISNo"].ToString();
            //        tSPAPagibigNo.Value = dr["PagIbigNo"].ToString();
            //        tSPAPresentAddress.Value = dr["PresentAddress"].ToString();
            //        //ddPermCountry.SelectedValue = dr["PermanentAddress"].ToString();
            //        //tSPAPermanent.Value = dr["PermanentAddress"].ToString();
            //        tSPAPosition.Value = dr["Position"].ToString();
            //        tSPAYearsofService.Value = Convert.ToInt32(Math.Round(decimal.Parse(dr["YearsOfService"].ToString()))).ToString();
            //        tSPAOfficeTelNo.Value = dr["OfficeTelNo"].ToString();
            //        tSPAFAXNo.Value = dr["FaxNo"].ToString();

            //        string SPAHomeOwnership = dr["HomeOwnership"].ToString();

            //        tSPAPerMonth.Disabled = true;
            //        if (SPAHomeOwnership == "Owned")
            //        { tSPAOwned.Checked = true; }
            //        else if (SPAHomeOwnership == "Mortgaged")
            //        { tSPAMortgaged.Checked = true; }
            //        else if (SPAHomeOwnership == "LivingwRelatives")
            //        { tSPALivingwRelatives.Checked = true; }
            //        else
            //        {
            //            tSPARented.Checked = true;
            //            tSPAPerMonth.Disabled = false;
            //            tSPAPerMonth.Value = SPAHomeOwnership;
            //        }

            //        tSPAYearsOfStay.Value = Math.Round(decimal.Parse(dr["YearsOfStay"].ToString())).ToString();

            //        tSPAEmpBusinessName.Value = dr["EmpBusinessName"].ToString();
            //        tSPAEmpBusinessAddress.Value = dr["EmpBusinessAddress"].ToString();
            //        tSPAEmploymentStatus.SelectedValue = dr["EmploymentStatus"].ToString();
            //        tSPANatureEmployment.SelectedValue = dr["NatureOfEmp"].ToString();
            //        tSPACivilStatus.SelectedValue = dr["CivilStatus"].ToString();
            //        lblSPAFileName.Text = dr["SPAFormDocument"].ToString();

            //        if (!String.IsNullOrEmpty(lblSPAFileName.Text))
            //        {
            //            visibleDocumentButtons(true, btnSPAPreview, btnSPARemove);
            //        }
            //        else
            //        {
            //            visibleDocumentButtons(false, btnSPAPreview, btnSPARemove);
            //        }
            //        //dr.Delete();
            //        DataTable dt1 = new DataTable();



            //        //DataTable dt2 = new DataTable();
            //        //dt1 = (DataTable)Session["dtSPACBDependent"];

            //        //for (int x = dt1.Rows.Count - 1; x >= 0; x--)
            //        //{
            //        //    DataRow dr1 = dt1.Rows[x];

            //        //    if (Convert.ToInt32(dr1["ID"]) == ID)
            //        //    {
            //        //        dt2 = (DataTable)Session["dtSPADependent"];

            //        //        DataRow dr2 = dt2.NewRow();
            //        //        dr2[0] = dr1["ID"].ToString();
            //        //        dr2[1] = dr1["Name"].ToString();
            //        //        dr2[2] = Convert.ToUInt64(dr1["Age"].ToString());
            //        //        dr2[3] = dr1["Relationship"].ToString();
            //        //        dt2.Rows.Add(dr2);

            //        //        Session["dtSPADependent"] = dt2;

            //        //        dr1.Delete();
            //        //    }
            //        //}





            //        int UserID = (int)Session["UserID"];
            //        //if (ws.DeleteListSPA(UserID, ID) == true)
            //        //{
            //        //    dt1 = hana.GetData($@"SELECT ""ID"",""Name"",""Age"",""Relationship"" FROM ""temp_CRD2"" WHERE ""UserID"" = {(int)Session["UserID"]} AND ""LineNum"" = {ID}", hana.GetConnection("SAOHana"));
            //        //    Session["dtSPADependent"] = dt1;
            //        //    LoadData(gvSPADependent, "dtSPADependent");

            //        //    dt1 = ws.select_temp_crd5(UserID).Tables["select_temp_crd5"];
            //        //    Session["gvSPACoBorrower"] = dt1;
            //        //    LoadData(gvSPACoBorrower, "gvSPACoBorrower");
            //        //}



            //        //DataTable dt3 = new DataTable();
            //        //dt3 = (DataTable)Session["gvSPACoBorrower"];

            //        //for (int y = dt3.Rows.Count - 1; y >= 0; y--)
            //        //{
            //        //    DataRow dr3 = dt3.Rows[y];

            //        //    if (Convert.ToInt32(dr3["ID"]) == ID)
            //        //    { dr3.Delete(); }
            //        //}

            //        //LoadData(gvSPACoBorrower, "gvSPACoBorrower");
            //    }
            //}

            #endregion 

        }

        protected void btnAddSalesAgent_ServerClick(object sender, EventArgs e)
        {

        }

        protected void gvEmployment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSalesSearch.Value))
            {
                gvEmployment.DataSource = hana.GetData($@"SELECT DISTINCT ""Id"" ""Code"", ""SalesPerson"" ""Name"" FROM ""OSLA"" WHERE UPPER(""SalesPerson"") LIKE '%{txtSalesSearch.Value.ToUpper()}%'", hana.GetConnection("SAOHana"));
            }
            else
            {
                gvEmployment.DataSource = hana.GetData($@"SELECT DISTINCT ""Id"" ""Code"", ""SalesPerson"" ""Name"" FROM ""OSLA""", hana.GetConnection("SAOHana"));
            }
            gvEmployment.PageIndex = e.NewPageIndex;
            gvEmployment.DataBind();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string Code = GetID.ID;
            if (FileUpload1.HasFile) //If the used Uploaded a file  
            {
                string code = ViewState["Guid"].ToString();

                //Get FileName and Extension seperately
                string fileNameOnly = Path.GetFileNameWithoutExtension(FileUpload1.FileName);
                string extension = Path.GetExtension(FileUpload1.FileName);
                string uniqueCode = code;

                string FileName = fileNameOnly + "_" + code + extension; //Name of the file is stored in local Variable   
                lblFileName.Text = FileName;
                FileUpload1.PostedFile.SaveAs(Server.MapPath("~/BUYER_AGENT/") + FileName); //File is saved in the Physical folder  

                visibleDocumentButtons(true, btnPreview, btnRemove);
            }
            if (FileUpload2.HasFile) //If the used Uploaded a file  
            {
                string code = ViewState["Guid"].ToString();

                //Get FileName and Extension seperately
                string fileNameOnly = Path.GetFileNameWithoutExtension(FileUpload2.FileName);
                string extension = Path.GetExtension(FileUpload2.FileName);
                string uniqueCode = code;

                string FileName = fileNameOnly + "_" + code + extension; //Name of the file is stored in local Variable   
                lblFileName2.Text = FileName;
                FileUpload2.PostedFile.SaveAs(Server.MapPath("~/BUYER_APPROVED/") + FileName); //File is saved in the Physical folder  

                visibleDocumentButtons(true, btnPreview2, btnRemove2);
            }
            if (Code == "btnUpload2")
                ScriptManager.RegisterStartupScript(this, GetType(), "approve", "showApprove();", true);
        }

        void visibleDocumentButtons(bool visible, LinkButton prev, LinkButton del)
        {
            prev.Visible = visible;
            del.Visible = visible;
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string Code = GetID.ID;
            if (File.Exists(Server.MapPath("~/BUYER_AGENT/") + lblFileName.Text))
            {
                // If file found, delete it    
                if (lblSave.InnerText == "Update")
                {
                    string CardCode = (string)Session["CardCode"];
                    hana.Execute($@"UPDATE ""OCRD"" SET ""SalesAgentDocument"" = '' WHERE ""CardCode"" = '{CardCode}'", hana.GetConnection("SAOHana"));
                }
                File.Delete(Server.MapPath("~/BUYER_AGENT/") + lblFileName.Text);
                lblFileName.Text = "";
                visibleDocumentButtons(false, btnPreview, btnRemove);
            }
            if (File.Exists(Server.MapPath("~/BUYER_APPROVED/") + lblFileName2.Text))
            {
                // If file found, delete it    
                if (lblSave.InnerText == "Update")
                {
                    string CardCode = (string)Session["CardCode"];
                    hana.Execute($@"UPDATE ""OCRD"" SET ""ApprovedDocument"" = '' WHERE ""CardCode"" = '{CardCode}'", hana.GetConnection("SAOHana"));
                }
                File.Delete(Server.MapPath("~/BUYER_APPROVED/") + lblFileName2.Text);
                lblFileName2.Text = "";
                visibleDocumentButtons(false, btnPreview2, btnRemove2);
            }
            if (Code == "btnRemove2")
                ScriptManager.RegisterStartupScript(this, GetType(), "approve", "showApprove();", true);
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            string Filepath = Server.MapPath("~/BUYER_AGENT/" + lblFileName.Text);
            //lblFileName.Text = Filepath;
            //System.Diagnostics.Process.Start(Filepath);
            var host = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.LocalPath, "");
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + host + "/BUYER_AGENT/" + lblFileName.Text + "');", true);

        }

        protected void btnPreview2_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string Code = GetID.ID;
            string Filepath = Server.MapPath("~/BUYER_APPROVED/" + lblFileName2.Text);
            //lblFileName.Text = Filepath;
            //System.Diagnostics.Process.Start(Filepath);
            var host = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.LocalPath, "");
            if (Code == "btnPreview2")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "approve", "showApprove();", true);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + host + "/BUYER_APPROVED/" + lblFileName2.Text + "');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + host + "/BUYER_APPROVED/" + lblAprvAttachment.Text + "');", true);
            }

        }

        protected void btnShowApprove_ServerClick(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "approve", "showApprove();", true);
        }

        protected void btnApprove_ServerClick(object sender, EventArgs e)
        {
            string CardCode = (string)Session["CardCode"];
            string approveddocument = lblFileName2.Text;
            hana.Execute($@"UPDATE ""OCRD"" SET ""ApprovedDocument"" = '{approveddocument}', ""Approved"" = 'Y' WHERE ""CardCode"" = '{CardCode}'", hana.GetConnection("SAOHana"));

            DataTable dtapprove = hana.GetData($@"SELECT 
		                          ""Approved"" 
                                 ,""ApprovedDocument"" 
                                FROM 
                                    ""OCRD"" 
                                WHERE 
                                    ""CardCode"" = '{CardCode}'", hana.GetConnection("SAOHana"));
            //AND   ""IsArchive"" = FALSE; 

            string approved = (string)DataAccess.GetData(dtapprove, 0, "Approved", "N");
            lblAprvAttachment.Text = (string)DataAccess.GetData(dtapprove, 0, "ApprovedDocument", ""); ;
            DataTable dtaccess = (DataTable)Session["UserAccess"];
            var access = dtaccess.Select($"CodeEncrypt= 'SALES01'");
            //if (Session["UserName"].ToString() != "Sales01" && approved == "Y")
            if (!access.Any())
            {
                btnApprove.Visible = false;
                divAprv.Visible = true;
                divAttch.Visible = String.IsNullOrEmpty(lblAprvAttachment.Text) ? false : true;
            }
            else
            {
                if (approved != "Y")
                {
                    btnApprove.Visible = true;
                    divAprv.Visible = false;
                    lblFileName.Text = (string)DataAccess.GetData(dtapprove, 0, "ApprovedDocument", "");
                }
                else
                {
                    btnApprove.Visible = false;
                    divAprv.Visible = true;
                    divAttch.Visible = String.IsNullOrEmpty(lblAprvAttachment.Text) ? false : true;
                }
            }
            access = dtaccess.Select($"CodeEncrypt= 'SALES' or CodeEncrypt= 'SALES01'");
            //if (access != "IT" && approved == "Y")
            if (!access.Any())
            {
                btnSave.Visible = false;
                mtxtUniqueId.Text = "Operation completed successfully.";
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "ShowSuccessAlert();", true);
                //alertMsg("Operation completed successfully.", "success");
                ScriptManager.RegisterStartupScript(this, GetType(), "approve", "hideApproveIT();", true);
            }
            else
            {
                btnSave.Visible = true;
                mtxtUniqueId.Text = "Operation completed successfully.";
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "ShowSuccessAlert();", true);
                //alertMsg("Operation completed successfully.", "success");
                ScriptManager.RegisterStartupScript(this, GetType(), "approve", "hideApprove();", true);
            }
            //alertMsg("Operation completed successfully.", "success");
        }
        protected void ddlBusinessType_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadDivisionsForNames(ddlBusinessType.Text);
        }

        void loadDivisionsForNames(string type)
        {
            DataTable taxclass = new DataTable();
            taxclass = (DataTable)ViewState["TaxClass"];
            taxclass.Rows.Clear();

            rbGuardian.Checked = false;
            rbGuardee.Checked = false;
            if (tCivilStatus.Text == "CS2" && ddlBusinessType.Text != "Corporation")
            {
                spouseBtn.Visible = true;
                sBuyerType.Visible = false;
                RequiredFieldValidator11.Enabled = true;
                RequiredFieldValidator9.Enabled = true;
                RequiredFieldValidator10.Enabled = true;
                RequiredFieldValidator51.Enabled = false;
                //txtSpecifyBusiness.Text = "";
            }
            if (type == "Corporation")
            {
                tMiddleName.Text = "";
                tFirstName.Text = "";
                tLastName.Text = "";

                tMiddleName2.Text = "";
                tFirstName2.Text = "";
                tLastName2.Text = "";
                tTINCorp.Text = "";

                suppTitle.InnerText = "Authorized Person Details";

                //taxclass.Rows.Add("Corporation", "Corporation");
                taxclass.Rows.Add("Engaged in Business", "Engaged in Business");
                taxclass.Rows.Add("Not engaged in Business", "Not engaged in Business");
                RequiredFieldValidator8.Enabled = true;
                empDetails.Visible = false;
                spouseBtn.Visible = false;
                RequiredFieldValidator11.Enabled = false;
                RequiredFieldValidator9.Enabled = false;
                RequiredFieldValidator10.Enabled = false;
                empDependents.Visible = false;

                divCompanyName.Visible = true;
                RequiredFieldValidator8.Enabled = true;

                divIndividual.Visible = false;
                RequiredFieldValidator1.Enabled = false;
                RequiredFieldValidator2.Enabled = false;
                RequiredFieldValidator3.Enabled = false;

                RequiredFieldValidator34.Enabled = true;
                RequiredFieldValidator35.Enabled = true;
                RequiredFieldValidator36.Enabled = true;

                //RequiredFieldValidator42.Enabled = true;
                //RequiredFieldValidator43.Enabled = true;
                //RequiredFieldValidator44.Enabled = true;
                //RequiredFieldValidator45.Enabled = true;
                //RequiredFieldValidator46.Enabled = true;
                //RequiredFieldValidator47.Enabled = true;

                SpecialBuyersValidation(false);

                compDetails.Visible = true;

                coborrowerGrid.Visible = false;
                contactDetails.Visible = false;

                othersGrid.Visible = false;

                RequiredFieldValidator7.Enabled = false;
                CustomValidator1.Enabled = false;

                RequiredFieldValidator41.Enabled = true;
                CustomValidator7.Enabled = true;

                tinhide.Visible = false;

                sBuyerType.Visible = false;
                RequiredFieldValidator51.Enabled = false;
                txtSpecifyBusiness.Text = "";

                divspecbuyers.Visible = false;

                ConformeCorp.Visible = true;

                RequiredFieldValidator63.Enabled = true;

                divnoncorp.Visible = false;
                divnoncorp2.Visible = false;
                divnoncorp3.Visible = false;
                RequiredFieldValidator33.Enabled = false;

                divTrustGuard.Visible = false;
                CustomValidator9.Enabled = false;

                HidePermAddress(false);


                //2023-06-14 : MAKE VALID ID 1 DEFAULT TO TIN FOR CORPORATION   
                tTypeOfId.SelectedValue = "ID1";
            }
            else
            {
                suppTitle.InnerText = "Supplementary Details";

                txtCompanyName.Text = "";

                taxclass.Rows.Add("Engaged in Business", "Engaged in Business");
                taxclass.Rows.Add("Not engaged in Business", "Not engaged in Business");
                RequiredFieldValidator8.Enabled = false;
                empDetails.Visible = true;
                empDependents.Visible = true;

                divCompanyName.Visible = false;
                RequiredFieldValidator8.Enabled = false;

                divIndividual.Visible = true;
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;

                RequiredFieldValidator34.Enabled = false;
                RequiredFieldValidator35.Enabled = false;
                RequiredFieldValidator36.Enabled = false;

                txtSpecifyBusiness.Text = "";
                //RequiredFieldValidator42.Enabled = false;
                //RequiredFieldValidator43.Enabled = false;
                //RequiredFieldValidator44.Enabled = false;
                //RequiredFieldValidator45.Enabled = false;
                //RequiredFieldValidator46.Enabled = false;
                //RequiredFieldValidator47.Enabled = false;

                compDetails.Visible = false;

                //divspecbuyers.Attributes.Clear();
                //divspecbuyers.Attributes.Add("class", "col-md-4");
                divbuyertype.Attributes.Clear();
                divbuyertype.Attributes.Add("class", "col-md-4");
                divtaxclass.Attributes.Clear();
                divtaxclass.Attributes.Add("class", "col-md-4");
                txtspecbuyer.Value = "";

                if (type == "Guardianship")
                {
                    txtspecbuyer.Value = "";
                    lblspecbuyers.InnerText = "Link Guardianship";
                    divspecbuyers.Visible = true;
                    //SpecialBuyersValidation(true);
                    //coborrowerGrid.Visible = false;
                    //contactDetails.Visible = true;
                    //contactTitle.InnerText = "Guardian Details";
                    //othersGrid.Visible = false;
                    sBuyerType.Visible = false;
                    RequiredFieldValidator51.Enabled = false;
                    txtSpecifyBusiness.Text = "";
                    lblSpecBuyersHeader.InnerText = "Choose Guardianship";

                    CustomValidator4.Enabled = false;
                    CustomValidator5.Enabled = false;
                    CustomValidator6.Enabled = false;

                    divTrustGuard.Visible = true;
                    CustomValidator9.Enabled = true;
                    rbGuardian.Text = "Guardian";
                    rbGuardee.Text = "Guardee";

                    divbuyertype.Attributes.Clear();
                    divbuyertype.Attributes.Add("class", "col-md-3");
                    divtaxclass.Attributes.Clear();
                    divtaxclass.Attributes.Add("class", "col-md-3");
                    divspecbuyers.Attributes.Clear();
                    divspecbuyers.Attributes.Add("class", "col-md-3");

                }
                else if (type == "Trusteeship")
                {
                    txtspecbuyer.Value = "";
                    lblspecbuyers.InnerText = "Link Trusteeship";
                    divspecbuyers.Visible = true;
                    //SpecialBuyersValidation(true);
                    //coborrowerGrid.Visible = false;
                    //contactDetails.Visible = true;
                    //contactTitle.InnerText = "Trustee Details";
                    //othersGrid.Visible = false;
                    sBuyerType.Visible = false;
                    RequiredFieldValidator51.Enabled = false;
                    txtSpecifyBusiness.Text = "";
                    lblSpecBuyersHeader.InnerText = "Choose Trusteeship";

                    CustomValidator4.Enabled = false;
                    CustomValidator5.Enabled = false;
                    CustomValidator6.Enabled = false;

                    divTrustGuard.Visible = true;
                    CustomValidator9.Enabled = true;
                    rbGuardian.Text = "Trustor";
                    rbGuardee.Text = "Trustee";

                    divbuyertype.Attributes.Clear();
                    divbuyertype.Attributes.Add("class", "col-md-3");
                    divtaxclass.Attributes.Clear();
                    divtaxclass.Attributes.Add("class", "col-md-3");
                    divspecbuyers.Attributes.Clear();
                    divspecbuyers.Attributes.Add("class", "col-md-3");
                }
                else if (type == "Co-ownership")
                {
                    txtspecbuyer.Value = "Co-owner/s";
                    lblspecbuyers.InnerText = "Link Co-ownership";
                    divspecbuyers.Visible = true;
                    //SpecialBuyersValidation(true);
                    //coborrowerGrid.Visible = true;
                    //contactDetails.Visible = false;
                    //othersGrid.Visible = false;
                    sBuyerType.Visible = false;
                    RequiredFieldValidator51.Enabled = false;
                    txtSpecifyBusiness.Text = "";
                    coother.InnerText = "Add Co-Owner";
                    lblSpecBuyersHeader.InnerText = "Choose Co-ownership";

                    CustomValidator4.Enabled = true;
                    CustomValidator5.Enabled = true;
                    CustomValidator6.Enabled = true;

                    divTrustGuard.Visible = false;
                    CustomValidator9.Enabled = false;

                    divspecbuyers.Attributes.Clear();
                    divspecbuyers.Attributes.Add("class", "col-md-4");
                }
                else if (ddlBusinessType.Text == "Others")
                {
                    lblspecbuyers.InnerText = "Link Co-buyer";
                    txtspecbuyer.Value = "Co-buyer/s";
                    divspecbuyers.Attributes.Clear();
                    divspecbuyers.Attributes.Add("class", "col-md-3");
                    divbuyertype.Attributes.Clear();
                    divbuyertype.Attributes.Add("class", "col-md-3");
                    divtaxclass.Attributes.Clear();
                    divtaxclass.Attributes.Add("class", "col-md-3");
                    divspecbuyers.Visible = true;
                    //SpecialBuyersValidation(true);
                    //coborrowerGrid.Visible = false;
                    //contactDetails.Visible = false;
                    //othersGrid.Visible = true;
                    sBuyerType.Visible = true;
                    RequiredFieldValidator51.Enabled = true;
                    coother.InnerText = "Add Co-Buyer";
                    lblSpecBuyersHeader.InnerText = "Choose Co-buyer";

                    CustomValidator4.Enabled = true;
                    CustomValidator5.Enabled = true;
                    CustomValidator6.Enabled = true;

                    divTrustGuard.Visible = false;
                    CustomValidator9.Enabled = false;
                }
                else
                {
                    divspecbuyers.Visible = false;
                    sBuyerType.Visible = false;
                    RequiredFieldValidator51.Enabled = false;
                    txtSpecifyBusiness.Text = "";
                    SpecialBuyersValidation(false);
                    coborrowerGrid.Visible = false;
                    contactDetails.Visible = false;
                    othersGrid.Visible = false;

                    CustomValidator4.Enabled = true;
                    CustomValidator5.Enabled = true;
                    CustomValidator6.Enabled = true;

                    divTrustGuard.Visible = false;
                    CustomValidator9.Enabled = false;
                }

                RequiredFieldValidator41.Enabled = false;
                CustomValidator7.Enabled = false;

                if (rbGuardian.Checked)
                {
                    RequiredFieldValidator7.Enabled = true;
                    CustomValidator1.Enabled = true;
                }
                else
                {
                    if (ddlBusinessType.Text != "Individual" && ddlBusinessType.Text != "Others")
                    {
                        RequiredFieldValidator7.Enabled = false;
                        CustomValidator1.Enabled = false;
                    }
                    else
                    {
                        RequiredFieldValidator7.Enabled = true;
                        CustomValidator1.Enabled = true;
                    }
                }



                tinhide.Visible = true;

                ConformeCorp.Visible = false;

                RequiredFieldValidator63.Enabled = false;

                divnoncorp.Visible = true;
                divnoncorp2.Visible = true;
                divnoncorp3.Visible = true;
                RequiredFieldValidator33.Enabled = true;

                HidePermAddress(true);
            }


            ddTaxClass.DataSource = taxclass;
            ddTaxClass.DataBind();

            taxClassChanged();
        }

        protected void ddSourceFunds_SelectedIndexChanged(object sender, EventArgs e)
        {
            otherFields();
        }

        void otherFields()
        {
            if (ddSourceFunds.Text == "OTH")
            {
                otherSourceOfFund.Visible = true;
            }
            else
            {
                otherSourceOfFund.Visible = false;
            }
        }

        //FOR TIN
        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //Check if TIN is in correct format(###-###-###-###)
            //bool isOK = Regex.IsMatch(tTIN.Value, @"[0-9][0-9][0-9]\-[0-9][0-9][0-9]\-[0-9][0-9][0-9]\-[0-9][0-9][0-9]");
            bool isOK = Regex.IsMatch(tTIN.Text, @"[0-9][0-9][0-9]\-[0-9][0-9][0-9]\-[0-9][0-9][0-9]\-[0-9][0-9][0-9]");
            if (!isOK)
            {
                RequiredFieldValidator7.Visible = false;
                args.IsValid = false;
            }
            else
            {
                RequiredFieldValidator7.Visible = true;
                args.IsValid = true;
            }
        }

        protected void CustomValidator3_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //if (tSPA.Checked == false && tCoBorrower.Checked == false)
            //{
            //    args.IsValid = false;
            //}
            //else
            //{
            //    args.IsValid = true;
            //}
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            //REPORT
            Session["RptConn"] = "Addon";
            Session["PrintDocEntry"] = lblCustomerCode.Text;

            //2023-06-05 : Add Project ; additional parameter on form
            Session["reportParameter"] = $@"-";

            //2023-06-06 : Change from Project to DocEntry instead
            Session["reportParameter"] = "0";



            Session["ReportPath"] = ConfigurationManager.AppSettings["ReportPathForms"].ToString();
            Session["ReportName"] = ConfigSettings.BuyersInfoForm;
            Session["ReportType"] = "Buyer";
            Session["RptConn"] = "SAP";

            //open new tab
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openpage", "window.open('ReportViewer.aspx');", true);
        }

        protected void btnNext2_ServerClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                //step_1.Visible = false;
                step_2.Visible = false;
                step_3.Visible = true;
                //done_step.Visible = false;
                //step1.Attributes.Add("class", "btn btn-success btn-circle");
                //step2.Attributes.Add("class", "btn btn-success btn-circle");
                //step3.Attributes.Add("class", "btn btn-info btn-circle");


                //2023-04-26 : ILAGAY NALANG SA NEXT BUTTON ANG PAG LAGAY NG LAMAN SA CERTIFIED NAME
                if (ddlBusinessType.Text != "Corporation")
                {
                    txtCertifyCompleteName.Value = ($@"{tFirstName.Text} {tMiddleName.Text} {tLastName.Text}").Trim();
                }
                else
                {
                    txtCertifyCompleteName.Value = ($@"{tFirstName2.Text} {tMiddleName2.Text} {tLastName2.Text}").Trim();
                    txtConformeCorp.Value = txtCompanyName.Text;
                }


                if (string.IsNullOrWhiteSpace(lblCustomerCode.Text))
                {
                    GenerateGuid();
                }

                if (string.IsNullOrWhiteSpace(ViewState["Guid"].ToString()))
                {
                    GenerateGuid();
                }


                //2023-06-15: LOADING OF DOCUMENTS



                //2023-06-15 : CHECK IF INDIVIDUAL AND IS MARRIED
                if (ddlBusinessType.Text == "Individual" && tCivilStatus.Text == "CS2")
                {
                    RequiredFieldValidator32.Enabled = true;
                }
                else
                {
                    RequiredFieldValidator32.Enabled = false;
                }
            }
        }


        void loadDocuments(string Code)
        {
            DataTable dt = hana.GetData($"CALL sp_BPEditOCRD ('{Code}')", hana.GetConnection("SAOHana"));

            foreach (GridViewRow row in gvStandardDocumentRequirements.Rows)
            {
                if (row.Cells[0].Text == "Proof of billing")
                {
                    LoadStandardDocuments("ProofOfBillingAttachment", dt, row);
                }
                else if (row.Cells[0].Text == "Valid ID 1")
                {
                    LoadStandardDocuments("ValidId1Attachment", dt, row);
                }
                else if (row.Cells[0].Text == "Valid ID 2")
                {
                    LoadStandardDocuments("ValidId2Attachment", dt, row);
                }
                else if (row.Cells[0].Text == "Proof of Income")
                {
                    LoadStandardDocuments("ProofOfIncomeAttachment", dt, row);
                }
            }

            foreach (GridViewRow row in gvStandardDocumentRequirements2.Rows)
            {
                if (row.Cells[0].Text == "Proof of billing")
                {
                    LoadStandardDocuments2("ProofOfBillingAttachment", dt, row);
                }
                else if (row.Cells[0].Text == "Valid ID 1")
                {
                    LoadStandardDocuments2("ValidId1Attachment", dt, row);
                }
                else if (row.Cells[0].Text == "Valid ID 2")
                {
                    LoadStandardDocuments2("ValidId2Attachment", dt, row);
                }
                else if (row.Cells[0].Text == "Proof of Income")
                {
                    LoadStandardDocuments2("ProofOfIncomeAttachment", dt, row);
                }
            }

        }

        protected void ddCivilStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tCivilStatus.Text == "CS2" && ddlBusinessType.Text != "Corporation")
            {
                spouseBtn.Visible = true;
                RequiredFieldValidator11.Enabled = true;
                RequiredFieldValidator9.Enabled = true;
                RequiredFieldValidator10.Enabled = true;
            }
            else
            {
                spouseBtn.Visible = false;
                RequiredFieldValidator11.Enabled = false;
                RequiredFieldValidator9.Enabled = false;
                RequiredFieldValidator10.Enabled = false;
            }

            tCivilStatus.Focus();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            //step_1.Visible = false;
            step_2.Visible = true;
            step_3.Visible = false;
            step1.Attributes.Add("class", "btn btn-success btn-circle");
            step2.Attributes.Add("class", "btn btn-info btn-circle");
            //step3.Attributes.Add("class", "btn btn-default btn-circle");
        }

        protected void btnCopySPA_Click(object sender, EventArgs e)
        {
            try
            {
                //STAY IN CURRENT TAB
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);

                //Spouse Details
                tSPALastName.Value = tSpouseLastName.Value;
                tSPAFirstName.Value = tSpouseFirstName.Value;
                tSPAMiddleName.Value = tSpouseMiddleName.Value;
                tSPAPresentAddress.Value = txtSPOAddress.Text;
                tSPABirthDate.Value = tSpouseBirthDate.Value;
                tSPABirthPlace.Value = tSpouseBirthPlace.Value;
                tSPAGender.SelectedIndex = tSpouseGender.SelectedIndex;
                tSPACitizenship.SelectedIndex = tSpouseCitizenship.SelectedIndex;
                tSPAEmailAdd.Value = tSpouseEmailAdd.Value;
                //tSPAHomeTelNo.Value = txtSPOTelNo.Text;
                tSPACellphoneNo.Value = tSpouseCellphoneNo.Value;
                tSPAFBAccount.Value = tSpouseFBAccount.Value;

                //Spouse Business Details
                tSPAEmploymentStatus.SelectedValue = tSpouseEmpStatus.SelectedValue;
                tSPANatureEmployment.SelectedValue = tSpouseNatureEmp.SelectedValue;
                tSPAEmpBusinessName.Value = tSpouseEmpBusinessName.Value;
                tSPAEmpBusinessAddress.Value = tSpouseEmpBusinessAddress.Value;
                //ddSPABusCountry.SelectedValue = ddSPOBusCountry.SelectedValue;
                tSPAPosition.Value = tSpousePosition.Value;
                tSPAYearsofService.Value = tSpouseYearsofService.Value;
                tSPAOfficeTelNo.Value = tSpouseOfficeTelNo.Value;
                tSPAFAXNo.Value = tSpouseFAXNo.Value;

                //COMMENTED 06 - 13 - 2023 : CHANGE REQUEST  https://direcbti.monday.com/boards/4462005457/pulses/4576973637?asset_id=908456598 --%>
                //tSPATIN.Value = tSpouseTIN.Value;
                tSPATIN.Value = tSpouseTIN2.Text;


                tSPASSSNo.Value = tSpouseSSSNo.Value;
                tSPAGSISNo.Value = tSpouseGSISNo.Value;
                tSPAPagibigNo.Value = tSpousePagibigNo.Value;

                //alertMsg("Data from Spouse tab copied to SPA/Co-borrower tab.", "success");
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }
        }
        protected void bTypeofAccount_ServerClick(object sender, EventArgs e)
        {
            DataTable dtBPDependents = hana.GetData($@"select 'Savings' as ""Account"" from ""DUMMY""
                                                        union all 
                                                        select 'Checking' as ""Account"" from ""DUMMY""
                                                        ", hana.GetConnection("SAOHana"));
            gvList.DataSource = dtBPDependents;
            gvList.DataBind();
        }

        protected void bSelect_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string Code = GetID.CommandArgument;

            tBAAcctType.Value = Code;

            ScriptManager.RegisterStartupScript(this, GetType(), "MsgAccount", "hideMsgAccount();", true);
        }

        protected void tLastName2_TextChanged(object sender, EventArgs e)
        {
            tLastName.Text = tLastName2.Text;
            txtCertifyCompleteName.Value = ($@"{tFirstName2.Text} {tMiddleName2.Text} {tLastName2.Text}").Trim();
            tLastName.Focus();
        }

        protected void tFirstName2_TextChanged(object sender, EventArgs e)
        {
            tFirstName.Text = tFirstName2.Text;
            txtCertifyCompleteName.Value = ($@"{tFirstName2.Text} {tMiddleName2.Text} {tLastName2.Text}").Trim();
            tFirstName.Focus();
        }

        protected void tMiddleName2_TextChanged(object sender, EventArgs e)
        {
            tMiddleName.Text = tMiddleName2.Text;
            txtCertifyCompleteName.Value = ($@"{tFirstName2.Text} {tMiddleName2.Text} {tLastName2.Text}").Trim();
            tMiddleName.Focus();
        }

        protected void btnAddCoOwner_ServerClick(object sender, EventArgs e)
        {
            string datatable = "";
            if (ddlBusinessType.SelectedValue == "Co-ownership")
            {
                datatable = "dtCoOwner";
            }
            else if (ddlBusinessType.SelectedValue == "Others")
            {
                datatable = "dtOthers";
            }
            DataTable dt = new DataTable();
            dt = (DataTable)Session[datatable];
            DataRow dr = dt.NewRow();

            int count = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (row.RowState.ToString() != "Deleted")
                    count++;
            }

            dr[0] = Convert.ToInt32(count + 1);
            dr[1] = ddlBusinessType.SelectedValue;
            dr[2] = tCoFName.Value;
            dr[3] = tCoMName.Value;
            dr[4] = tCoLName.Value;
            dr[5] = tCoRelationship.Value;
            dr[6] = tCoEmail.Value;
            dr[7] = tCoMobileNo.Value;
            dr[8] = tCoAddress.Value;
            dr[9] = tCoResidence.Value;
            dr[10] = tCoValidID.SelectedItem;
            dr[11] = tCoValidIDNo.Value;
            dt.Rows.Add(dr);

            Session[datatable] = dt;

            if (datatable == "dtCoOwner")
            {
                LoadData(gvCoOwner, datatable);
            }
            else if (datatable == "dtOthers")
            {
                LoadData(gvOthers, datatable);
            }
            ClearCoOwner();

            ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "MsgCoowner_Hide();", true);
        }

        void SpecialBuyersValidation(bool val)
        {
            RequiredFieldValidator13.Enabled = val;
            RequiredFieldValidator14.Enabled = val;
            RequiredFieldValidator15.Enabled = val;
            RequiredFieldValidator21.Enabled = val;
            RequiredFieldValidator23.Enabled = val;
            RequiredFieldValidator24.Enabled = val;
        }
        public void ClearCoOwner()
        {
            tCoFName.Value = "";
            tCoMName.Value = "";
            tCoLName.Value = "";
            tCoRelationship.Value = "";
            tCoEmail.Value = "";
            tCoMobileNo.Value = "";
            tCoAddress.Value = "";
            tCoResidence.Value = "";
            tCoValidID.SelectedValue = "---Select Valid ID---";
            tCoValidIDNo.Value = "";
        }

        protected void gvCoOwnerDelete_Click(object sender, EventArgs e)
        {
            string action = "";
            string type = "";
            if (ddlBusinessType.SelectedValue == "Co-ownership")
            {
                action = "DelCoOwner";
                type = "co-owner";
            }
            else if (ddlBusinessType.SelectedValue == "Others")
            {
                action = "DelOthers";
                type = "detail";
            }

            LinkButton GetID = (LinkButton)sender;
            Control btn = (Control)sender;
            Session["Buyers_Control"] = btn.ID;
            Session["BuyersCo_ID"] = Convert.ToString(GetID.CommandArgument);
            confirmation($@"Are you sure you want to delete the selected {type}?", action);
        }

        protected void gvCoOwner_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCoOwner.PageIndex = e.NewPageIndex;
            string type = ddlBusinessType.SelectedValue;
            if (ddlBusinessType.SelectedValue != "Co-ownership" && ddlBusinessType.SelectedValue != "Individual" && ddlBusinessType.SelectedValue != "Corporation" && ddlBusinessType.SelectedValue != "Others")
            {
                string trustguard = "";
                trustguard = rbGuardian.Checked ? rbGuardee.Text : rbGuardian.Text;

                string qrytrustguard = $@"Select B.""CardCode"",B.""LastName"",B.""FirstName"",B.""MiddleName"",A.""BusinessType"" as ""SpecifiedBusinessType"" from ""OCRD"" A
                            INNER JOIN CRD1 B ON A.""CardCode"" = B.""CardCode""
                            where A.""BusinessType"" = '{type}' AND IFNULL(A.""Approved"", '') <> '' AND A.""CardCode"" NOT IN('{Session["CardCode"].ToString()}','{ViewState["BuyerLink"].ToString()}') AND B.""CardType"" = 'Buyer' AND IFNULL(A.""SpecialBuyerRole"",'{rbGuardian.Text}') = '{trustguard}'";

                DataTable dt = hana.GetData(qrytrustguard, hana.GetConnection("SAOHana"));
                gvCoOwner.DataSource = dt;
                gvCoOwner.Columns[4].Visible = false;
                gvCoOwner.DataBind();
            }
            else if (ddlBusinessType.SelectedValue == "Co-ownership" || ddlBusinessType.SelectedValue == "Others")
            {
                string Coowners = "";
                DataTable dt = (DataTable)Session["dtCoOwner"];
                foreach (DataRow row in dt.Rows)
                {
                    Coowners += $@",'{row["CardCode"]}'";
                }
                string qry = $@"Select B.""CardCode"",B.""LastName"",B.""FirstName"",B.""MiddleName"",A.""BusinessType"" as ""SpecifiedBusinessType"" from ""OCRD"" A
                            INNER JOIN CRD1 B ON A.""CardCode"" = B.""CardCode""
                            where A.""BusinessType"" = '{type}' AND IFNULL(A.""Approved"", '') <> '' AND A.""CardCode"" NOT IN('{Session["CardCode"].ToString()}'{Coowners}) AND B.""CardType"" = 'Buyer'";
                dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
                gvCoOwner.DataSource = dt;
                gvCoOwner.Columns[4].Visible = true;
                gvCoOwner.DataBind();
            }
            //LoadData(gvCoOwner, "dtCoOwner");

            //gvCoOwner.DataBind();
        }
        protected void CustomValidator2_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (gvCoOwner.Rows.Count <= 0)
            {
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }
        }
        protected void Reload_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/pages/Buyers.aspx");
        }
        protected void gvOthers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCoOwner.PageIndex = e.NewPageIndex;
            LoadData(gvOthers, "dtOthers");
        }

        protected void btnSPACoBorrowerModal_ServerClick(object sender, EventArgs e)
        {

            //2023-06-18 : ADDED
            btnSPACoBorrower.Visible = true;
            btnSPAUpdate.Visible = false;

            spaTitle.InnerText = "New SPA";

            lblSPAFileName.Text = string.Empty;
            visibleDocumentButtons(false, btnSPAPreview, btnSPARemove);

            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);
            ClearSPACoBorrower();
        }







        void ChangeOthers(bool enabledBlocker, bool showOthers, int num)
        {
            if (enabledBlocker == true)
            {
                if (num == 1)
                {
                    others1.Visible = showOthers;
                    if (showOthers)
                    {
                        ddothers1.Attributes.Remove("class");
                        ddothers1.Attributes.Add("class", "col-md-3");

                    }
                    else
                    {
                        ddothers1.Attributes.Remove("class");
                        ddothers1.Attributes.Add("class", "col-md-6");
                    }
                }
                if (num == 2)
                {
                    others2.Visible = showOthers;
                    //--COMMENTED 04-04-2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                    //RequiredFieldValidator12.Enabled = enabledBlocker;
                    if (showOthers)
                    {
                        ddothers2.Attributes.Remove("class");
                        ddothers2.Attributes.Add("class", "col-md-3");
                    }
                    else
                    {
                        ddothers2.Attributes.Remove("class");
                        ddothers2.Attributes.Add("class", "col-md-6");
                    }

                }
                if (num == 3)
                {
                    others3.Visible = showOthers;
                    RequiredFieldValidator39.Enabled = enabledBlocker;
                    if (showOthers)
                    {
                        ddothers3.Attributes.Remove("class");
                        ddothers3.Attributes.Add("class", "col-md-3");
                    }
                    else
                    {
                        ddothers3.Attributes.Remove("class");
                        ddothers3.Attributes.Add("class", "col-md-6");
                    }
                }
                if (num == 4)
                {
                    others4.Visible = showOthers;
                    RequiredFieldValidator40.Enabled = enabledBlocker;
                    if (showOthers)
                    {
                        ddothers4.Attributes.Remove("class");
                        ddothers4.Attributes.Add("class", "col-md-3");
                    }
                    else
                    {
                        ddothers4.Attributes.Remove("class");
                        ddothers4.Attributes.Add("class", "col-md-6");
                    }
                }
            }
            else
            {
                if (num == 1)
                {
                    others1.Visible = showOthers;
                    //ddothers1.Style.Remove("col-md-4");
                    ddothers1.Attributes.Remove("class");
                    ddothers1.Attributes.Add("class", "col-md-6");
                }
                if (num == 2)
                {
                    others2.Visible = showOthers;
                    ddothers2.Attributes.Remove("class");
                    ddothers2.Attributes.Add("class", "col-md-6");
                    //--COMMENTED 04-04-2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                    //RequiredFieldValidator12.Enabled = enabledBlocker;
                }
                if (num == 3)
                {
                    others3.Visible = showOthers;
                    ddothers3.Attributes.Remove("class");
                    ddothers3.Attributes.Add("class", "col-md-6");
                    RequiredFieldValidator39.Enabled = enabledBlocker;
                }
                if (num == 4)
                {
                    others4.Visible = showOthers;
                    ddothers4.Attributes.Remove("class");
                    ddothers4.Attributes.Add("class", "col-md-6");
                    RequiredFieldValidator40.Enabled = enabledBlocker;
                }
            }
        }



        void selectIndexChange1(DropDownList typeOfID, int tag, TextBox idno)
        {
            DataTable dt = (DataTable)ViewState["ValidId"];
            DataTable dt2 = (DataTable)ViewState["ValidIdPrev"];

            string checker = typeOfID.SelectedValue;
            idno.Text = "";
            if (checker == "---Select Valid ID---")
            {
                if (dt2.Rows[tag - 1]["Code"].ToString() != "---Select Type of ID---" && dt2.Rows[tag - 1]["Code"].ToString() != "OTH" && dt2.Rows[tag - 1]["Code"].ToString() != checker)
                {
                    dt.Rows.Add(
                    dt2.Rows[tag - 1]["Code"].ToString(),
                    dt2.Rows[tag - 1]["Name"].ToString()
                    );
                }
                dt2.Rows[tag - 1]["Code"] = typeOfID.SelectedValue;
                dt2.Rows[tag - 1]["Name"] = typeOfID.SelectedItem;
                ChangeOthers(false, false, tag);
            }
            if (checker == "OTH")
            {
                if (dt2.Rows[tag - 1]["Code"].ToString() != "---Select Type of ID---" && dt2.Rows[tag - 1]["Code"].ToString() != "OTH" && dt2.Rows[tag - 1]["Code"].ToString() != checker)
                {
                    dt.Rows.Add(
                    dt2.Rows[tag - 1]["Code"].ToString(),
                    dt2.Rows[tag - 1]["Name"].ToString()
                    );
                }
                dt2.Rows[tag - 1]["Code"] = typeOfID.SelectedValue;
                dt2.Rows[tag - 1]["Name"] = typeOfID.SelectedItem;
                ChangeOthers(true, true, tag);
            }
            else
            {
                if (dt2.Rows[tag - 1]["Code"].ToString() != "---Select Type of ID---" && dt2.Rows[tag - 1]["Code"].ToString() != "OTH" && dt2.Rows[tag - 1]["Code"].ToString() != checker)
                {
                    dt.Rows.Add(
                    dt2.Rows[tag - 1]["Code"].ToString(),
                    dt2.Rows[tag - 1]["Name"].ToString()
                    );
                }
                dt2.Rows[tag - 1]["Code"] = typeOfID.SelectedValue;
                dt2.Rows[tag - 1]["Name"] = typeOfID.SelectedItem;
                if (typeOfID.SelectedValue == "ID1")
                {
                    //2023-05-03 : CHECK BUSINESS TYPE: IF CORPORATION = DONT AUTOMATE TIN NUMBER
                    if (ddlBusinessType.Text.ToUpper() != "CORPORATION")
                    {
                        idno.Text = tTIN.Text;
                    }
                }
                if (typeOfID.SelectedValue == "ID2")
                {
                    idno.Text = tSSSNo.Text;
                }
                if (typeOfID.SelectedValue == "ID3")
                {
                    idno.Text = tPagibigNo.Text;
                }
                foreach (DataRow row in dt.Rows)
                {
                    if (row[0].ToString() == checker && row[0].ToString() != "OTH" && row[0].ToString() != "---Select Type of ID---")
                    {
                        dt.Rows.Remove(row);
                        break;
                    }
                }
                ChangeOthers(true, false, tag);
            }
            ChangeValidIDDd(dt, dt2, typeOfID.ID, tag);
        }

        void ChangeValidIDDd(DataTable dt, DataTable dt2, string dropdown, int tag)
        {

            string prev;
            ViewState["ValidIdPrev"] = dt2;
            ViewState["ValidId"] = dt;

            if (dropdown != "tTypeOfId")
            {
                if (tTypeOfId.SelectedValue == "---Select Type of ID---")
                {
                    tTypeOfId.DataSource = dt;
                    tTypeOfId.DataBind();
                    tTypeOfId.SelectedValue = "---Select Type of ID---";
                }
                else
                {
                    prev = tTypeOfId.SelectedValue;
                    tTypeOfId.SelectedValue = "---Select Type of ID---";
                    tTypeOfId.DataSource = dt;
                    tTypeOfId.DataBind();
                    if (!dt.Columns["Code"].ToString().Contains(prev))
                    {
                        tTypeOfId.Items.Add(new ListItem(dt2.Rows[0]["Name"].ToString(), dt2.Rows[0]["Code"].ToString()));
                    }
                    tTypeOfId.SelectedValue = prev;
                }
            }
            if (dropdown != "tTypeOfId2")
            {
                if (tTypeOfId2.SelectedValue == "---Select Type of ID---")
                {
                    tTypeOfId2.DataSource = dt;
                    tTypeOfId2.DataBind();
                    tTypeOfId2.SelectedValue = "---Select Type of ID---";
                }
                else if (tTypeOfId2.SelectedValue == "OTH")
                {
                    tTypeOfId2.DataSource = dt;
                    tTypeOfId2.DataBind();
                    tTypeOfId2.SelectedValue = "OTH";
                }
                else
                {
                    prev = tTypeOfId2.SelectedValue;
                    tTypeOfId2.SelectedValue = "---Select Type of ID---";
                    tTypeOfId2.DataSource = dt;
                    tTypeOfId2.DataBind();
                    if (!dt.Columns["Code"].ToString().Contains(prev))
                    {
                        tTypeOfId2.Items.Add(new ListItem(dt2.Rows[1]["Name"].ToString(), dt2.Rows[1]["Code"].ToString()));
                    }
                    tTypeOfId2.SelectedValue = prev;
                }
            }
            if (dropdown != "tTypeOfId3")
            {
                if (tTypeOfId3.SelectedValue == "---Select Type of ID---")
                {
                    tTypeOfId3.DataSource = dt;
                    tTypeOfId3.DataBind();
                    tTypeOfId3.SelectedValue = "---Select Type of ID---";
                }
                else if (tTypeOfId3.SelectedValue == "OTH")
                {
                    tTypeOfId3.DataSource = dt;
                    tTypeOfId3.DataBind();
                    tTypeOfId3.SelectedValue = "OTH";
                }
                else
                {
                    prev = tTypeOfId3.SelectedValue;
                    tTypeOfId3.SelectedValue = "---Select Type of ID---";
                    tTypeOfId3.DataSource = dt;
                    tTypeOfId3.DataBind();
                    if (!dt.Columns["Code"].ToString().Contains(prev))
                    {
                        tTypeOfId3.Items.Add(new ListItem(dt2.Rows[2]["Name"].ToString(), dt2.Rows[2]["Code"].ToString()));
                    }
                    tTypeOfId3.SelectedValue = prev;
                }
            }
            if (dropdown != "tTypeOfId4")
            {
                if (tTypeOfId4.SelectedValue == "---Select Type of ID---")
                {
                    tTypeOfId4.DataSource = dt;
                    tTypeOfId4.DataBind();
                    tTypeOfId4.SelectedValue = "---Select Type of ID---";
                }
                else if (tTypeOfId4.SelectedValue == "OTH")
                {
                    tTypeOfId4.DataSource = dt;
                    tTypeOfId4.DataBind();
                    tTypeOfId4.SelectedValue = "OTH";
                }
                else
                {
                    prev = tTypeOfId4.SelectedValue;
                    tTypeOfId4.SelectedValue = "---Select Type of ID---";
                    tTypeOfId4.DataSource = dt;
                    tTypeOfId4.DataBind();
                    if (!dt.Columns["Code"].ToString().Contains(prev))
                    {
                        tTypeOfId4.Items.Add(new ListItem(dt2.Rows[3]["Name"].ToString(), dt2.Rows[3]["Code"].ToString()));
                    }
                    tTypeOfId4.SelectedValue = prev;
                }
            }
        }

        void ValidIdDropDownEdit(DataTable dt, DataTable dt2, string value, DropDownList dropdown, int tag, TextBox idNumber, TextBox others)
        {
            //Prevent other Valid ID Dropdown to choose same ID
            if (dropdown.SelectedValue != "---Select Type of ID---")
            {
                dt2.Rows[tag - 1]["Code"] = dropdown.SelectedValue;
                dt2.Rows[tag - 1]["Name"] = dropdown.SelectedItem;
                if (dropdown.SelectedValue != "OTH")
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row[0].ToString() == dropdown.SelectedValue && row[0].ToString() != "OTH" && row[0].ToString() != "---Select Valid ID---")
                        {
                            dt.Rows.Remove(row);
                            break;
                        }
                    }
                }
                else if (dropdown.SelectedValue == "OTH")
                {
                    idNumber.Text = value;
                    ChangeOthers(true, true, tag);
                }
                else
                {
                    idNumber.Text = value;
                }
                ChangeValidIDDd(dt, dt2, dropdown.ID, tag);
            }
            else
            {
                idNumber.Text = "";
                others.Text = "";
            }
        }

        protected void tTypeOfId_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectIndexChange1(tTypeOfId, 1, tIDNo);
            tIDNo.Focus();
        }

        protected void tTypeOfId2_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectIndexChange1(tTypeOfId2, 2, tIDNo2);
            tIDNo2.Focus();
        }

        protected void tTypeOfId3_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectIndexChange1(tTypeOfId3, 3, tIDNo3);

        }

        protected void tTypeOfId4_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectIndexChange1(tTypeOfId4, 4, tIDNo4);

        }

        protected void CustomValidator4_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                DateTime checker = Convert.ToDateTime(args.Value);
                TimeSpan totaldays = DateTime.Now.Subtract(checker);
                int age = totaldays.Days / 365;
                if (age < 17)
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

        protected void CustomValidator5_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                DateTime checker = Convert.ToDateTime(args.Value);
                TimeSpan totaldays = DateTime.Now.Subtract(checker);
                int age = totaldays.Days / 365;
                if (age < 17)
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

        protected void CustomValidator6_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                DateTime checker = Convert.ToDateTime(args.Value);
                TimeSpan totaldays = DateTime.Now.Subtract(checker);
                int age = totaldays.Days / 365;
                if (age < 17)
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

        protected void tTIN_TextChanged(object sender, EventArgs e)
        {
            if (tTypeOfId.SelectedValue == "ID1")
            {
                tIDNo.Text = tTIN.Text;
            }
            else if (tTypeOfId2.SelectedValue == "ID1")
            {
                tIDNo2.Text = tTIN.Text;
            }
            else if (tTypeOfId3.SelectedValue == "ID1")
            {
                tIDNo3.Text = tTIN.Text;
            }
            else if (tTypeOfId4.SelectedValue == "ID1")
            {
                tIDNo4.Text = tTIN.Text;
            }
            //tTINCorp.Text = tTIN.Text;
            tTIN.Focus();
        }

        protected void tIDNo_TextChanged(object sender, EventArgs e)
        {
            if (tTypeOfId.SelectedValue == "ID1")
            {
                tTIN.Text = tIDNo.Text;
            }
            if (tTypeOfId.SelectedValue == "ID2")
            {
                tSSSNo.Text = tIDNo.Text;
            }
            if (tTypeOfId.SelectedValue == "ID3")
            {
                tPagibigNo.Text = tIDNo.Text;
            }

            tTypeOfId2.Focus();
        }

        protected void tIDNo2_TextChanged(object sender, EventArgs e)
        {
            if (tTypeOfId2.SelectedValue == "ID1")
            {
                tTIN.Text = tIDNo2.Text;
            }
            if (tTypeOfId2.SelectedValue == "ID2")
            {
                tSSSNo.Text = tIDNo2.Text;
            }
            if (tTypeOfId2.SelectedValue == "ID3")
            {
                tPagibigNo.Text = tIDNo2.Text;
            }
        }

        protected void tIDNo3_TextChanged(object sender, EventArgs e)
        {
            if (tTypeOfId3.SelectedValue == "ID1")
            {
                tTIN.Text = tIDNo3.Text;
            }
        }

        protected void tIDNo4_TextChanged(object sender, EventArgs e)
        {
            if (tTypeOfId4.SelectedValue == "ID1")
            {
                tTIN.Text = tIDNo4.Text;
            }
        }

        private void taxClassChanged()
        {
            //KARL - SET SOURCE OF FUNDS AND POSITION VALIDATOR DEPENDING ON TAX CLASSIFICATION - 2023/05/04
            if (ddTaxClass.SelectedValue != "Not engaged in Business")
            {
                sourcefunds.Visible = false;
                ddSourceFunds_RequiredFieldValidator.Enabled = false;
                tPosition_RequiredFieldValidator.Enabled = false;
                empstat.Visible = false;
                empDetails.Visible = false;
                RequiredFieldValidator4.Enabled = false;
            }
            else
            {
                sourcefunds.Visible = true;
                ddSourceFunds_RequiredFieldValidator.Enabled = true;
                tPosition_RequiredFieldValidator.Enabled = true;
                empstat.Visible = true;
                empDetails.Visible = true;
                RequiredFieldValidator4.Enabled = true;

            }
        }

        protected void ddTaxClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            taxClassChanged();
        }

        protected void CustomValidator7_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //Check if TIN is in correct format(###-###-###-###)
            //bool isOK = Regex.IsMatch(tTIN.Value, @"[0-9][0-9][0-9]\-[0-9][0-9][0-9]\-[0-9][0-9][0-9]\-[0-9][0-9][0-9]");
            bool isOK = Regex.IsMatch(tTINCorp.Text, @"[0-9][0-9][0-9]\-[0-9][0-9][0-9]\-[0-9][0-9][0-9]\-[0-9][0-9][0-9]");
            if (!isOK)
            {
                RequiredFieldValidator41.Visible = false;
                args.IsValid = false;
            }
            else
            {
                RequiredFieldValidator41.Visible = true;
                args.IsValid = true;
            }
        }

        protected void tTINCorp_TextChanged(object sender, EventArgs e)
        {
            //tTIN.Text = tTINCorp.Text;
            //if (tTypeOfId.SelectedValue == "ID1")
            //    tIDNo.Text = tTIN.Text;
            //if (tTypeOfId2.SelectedValue == "ID1")
            //    tIDNo.Text = tTIN.Text;
            tTINCorp.Focus();
        }
        protected void gvStandardDocumentRequirements_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            attachmentFileUploadBuyer(gvStandardDocumentRequirements, e, "FileUpload3", "lblFileName1", "btnPreview3", "btnRemove3", "RequiredFieldValidatorAttachments");
            //attachmentFileUploadBuyer(gvStandardDocumentRequirements, e, "FileUpload3", "lblFileName1", "btnPreview3", "btnRemove3", "customValidatorDocuments");
        }
        void attachmentFileUploadBuyer(GridView gv, GridViewCommandEventArgs e, string FileUpload, string lblfilename, string preview, string remove, string rv)
        {
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int index = row.RowIndex;

            FileUpload file = (FileUpload)gv.Rows[index].FindControl(FileUpload);
            Label lblFileName = (Label)gv.Rows[index].FindControl(lblfilename);
            LinkButton btnPreview = (LinkButton)gv.Rows[index].FindControl(preview);
            LinkButton btnDelete = (LinkButton)gv.Rows[index].FindControl(remove);
            RequiredFieldValidator rvReq = (RequiredFieldValidator)gv.Rows[index].FindControl(rv);
            //CustomValidator rvReq = (CustomValidator)gv.Rows[index].FindControl(rv);

            string buyerFullName = (tFirstName.Text + "_" + tMiddleName.Text + "_" + tLastName.Text).Trim().ToString().ToUpper();
            string code = ViewState["Guid"].ToString();
            string uniqueCode = code + "_" + tTIN.Text.Replace("-", "");
            //string FolderName = uniqueCode + "_" + buyerFullName; //Folder Name for buyer
            if (e.CommandName.ToLower().Contains("upload"))
            {

                if (file.HasFile)
                {

                    //Get FileName and Extension seperately
                    string fileNameOnly = Path.GetFileNameWithoutExtension(file.FileName);
                    string extension = Path.GetExtension(file.FileName);
                    string FileName = fileNameOnly + "_" + code + extension; //File Name of buyer  
                    //string FileName = ""; //File Name of buyer  
                    //string path = HttpContext.Current.Server.MapPath("~/BUYER_ATTACHMENTS/");

                    //if (index == 0)
                    //    FileName = "pob" + "_" + fileNameOnly + "_" + code + extension;

                    //if (index == 1)
                    //    FileName = "id1" + "_" + fileNameOnly + "_" + code + extension;

                    //if (index == 2)
                    //    FileName = "id2" + "_" + fileNameOnly + "_" + code + extension;

                    //string tempPath = HttpContext.Current.Server.MapPath("~/TEMP_DOCS/");

                    //if (!System.IO.Directory.Exists(tempPath + FolderName))
                    //{
                    //    System.IO.Directory.CreateDirectory(tempPath + FolderName); //Create buyers directory if it doesn't exist
                    //}

                    //if (File.Exists(HttpContext.Current.Server.MapPath($"~/TEMP_DOCS/{FolderName}/") + FileName))
                    if (File.Exists(HttpContext.Current.Server.MapPath($"~/TEMP_DOCS/") + FileName) || File.Exists(HttpContext.Current.Server.MapPath($"~/BUYER_ATTACHMENTS/") + FileName))
                    {
                        alertMsg("File already exists!", "error");
                        rvReq.Enabled = true;
                    }
                    else
                    {
                        lblFileName.Text = FileName;
                        file.PostedFile.SaveAs(HttpContext.Current.Server.MapPath($"~/TEMP_DOCS/") + FileName); //File is saved in the Physical folder 
                        //file.PostedFile.SaveAs(HttpContext.Current.Server.MapPath($"~/TEMP_DOCS/{FolderName}/") + FileName); //File is saved in the Physical folder 
                        btnPreview.Visible = true;
                        btnDelete.Visible = true;
                        rvReq.Enabled = false;
                    }
                }
                else
                {
                    rvReq.Enabled = false;
                    alertMsg("Please choose file!", "warning");
                }
            }
            else if (e.CommandName.ToLower().Contains("preview"))
            {
                //if (File.Exists(Server.MapPath($"~/TEMP_DOCS/{FolderName}/") + lblFileName.Text))
                if (File.Exists(Server.MapPath($"~/TEMP_DOCS/") + lblFileName.Text))
                {
                    string Filepath = Server.MapPath($"~/TEMP_DOCS/" + lblFileName.Text);
                    //string Filepath = Server.MapPath($"~/TEMP_DOCS/{FolderName}/" + lblFileName.Text);
                    var host = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.LocalPath, "");
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + host + $"/TEMP_DOCS/" + lblFileName.Text + "');", true);
                    //ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + host + $"/TEMP_DOCS/{FolderName}/" + lblFileName.Text + "');", true);
                }
                else
                {
                    //string Filepath = Server.MapPath($"~/BUYER_ATTACHMENTS/{FolderName}/" + lblFileName.Text);
                    string Filepath = Server.MapPath($"~/BUYER_ATTACHMENTS/" + lblFileName.Text);
                    var host = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.LocalPath, "");
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + host + $"/BUYER_ATTACHMENTS/" + lblFileName.Text + "');", true);
                    //ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + host + $"/BUYER_ATTACHMENTS/{FolderName}/" + lblFileName.Text + "');", true);
                }

            }
            else if (e.CommandName.ToLower().Contains("remove"))
            {

                //if (File.Exists(Server.MapPath($"~/TEMP_DOCS/{FolderName}/") + lblFileName.Text))
                if (File.Exists(Server.MapPath($"~/TEMP_DOCS/") + lblFileName.Text))
                {
                    // If file found, delete it 
                    File.Delete(Server.MapPath($"~/TEMP_DOCS/") + lblFileName.Text);
                    //File.Delete(Server.MapPath($"~/TEMP_DOCS/{FolderName}/") + lblFileName.Text);
                    lblFileName.Text = "";
                }
                else
                {
                    lblFileName.Text = "";
                }
                btnPreview.Visible = false;
                btnDelete.Visible = false;
                rvReq.Enabled = true;
                //rvReq.Enabled = false;
            }

        }

        void GetBuyerAttachments(GridView gv, string FileUpload, string lblfilename, string preview, string remove, string rv)
        {
            foreach (GridViewRow row in gv.Rows)
            {
                int index = row.RowIndex;

                //FileUpload fileUpload = (FileUpload)gv.Rows[index].FindControl(FileUpload);
                Label lblFileName = (Label)gv.Rows[index].FindControl(lblfilename);
                LinkButton btnPreview = (LinkButton)gv.Rows[index].FindControl(preview);
                LinkButton btnDelete = (LinkButton)gv.Rows[index].FindControl(remove);
                RequiredFieldValidator rvReq = (RequiredFieldValidator)gv.Rows[index].FindControl(rv);

                string tin = tTIN.Text.Replace("-", "");
                string path = Server.MapPath($"~/BUYER_ATTACHMENTS/");
                string dirPattern = $@"*_{tin}_*";

                var findFile = Directory.GetDirectories(path, dirPattern, SearchOption.AllDirectories);

                if (findFile.Length > 0)
                {
                    string filePattern = "";


                    //2023-11-29 : CHANGED CONDITION; CHANGED REQUIRED DOCUMENTS
                    //if (index == 0)
                    //    filePattern = $@"*pob*";
                    //if (index == 1)
                    //    filePattern = $@"*id1*";
                    //if (index == 2)
                    //    filePattern = $@"*id2*";
                    if (index == 0)
                        filePattern = $@"*id1*";
                    if (index == 1)
                        filePattern = $@"*id2*";

                    string dirPath = findFile.FirstOrDefault();
                    //var dirName = new DirectoryInfo(dirPath).Name;

                    var dirBuyerFiles = Directory.GetFiles(dirPath, filePattern, SearchOption.TopDirectoryOnly);

                    if (dirBuyerFiles.Length > 0)
                    {
                        lblFileName.Text = Path.GetFileName(dirBuyerFiles.FirstOrDefault());
                        if (!string.IsNullOrEmpty(lblFileName.Text))
                        {
                            btnPreview.Visible = true;
                            btnDelete.Visible = true;
                            rvReq.Enabled = false;
                        }
                    }
                }

            }
        }

        void LoadBuyerDocumentsStandard()
        {
            gvStandardDocumentRequirements.DataSource = ws.GetBuyerAttachments();
            gvStandardDocumentRequirements.DataBind();
        }

        protected string GetFormattedDocumentName(object dataItem)
        {
            DataRowView row = (DataRowView)dataItem;
            string documentName = row["DocumentName"].ToString();
            bool isRequired = Convert.ToBoolean(row["Required"]);

            if (isRequired)
            {
                return $"{documentName}<span class=\"color-red fsize-16\"> *</span>";
            }
            else
            {
                return documentName;
            }
        }



        void LoadBuyerDocumentsStandard_NotRequired()
        {
            //<% --2023 - 04 - 19 : REQUESTED BY DATA TO REMOVE BLOCKING OF 2ND ID--%>
            //     <% --CHANG REQUEST: NO BLOCKINGS FOR THIS GRIDVIEW-- %>
            gvStandardDocumentRequirements2.DataSource = ws.GetBuyerAttachments_NotRequired();
            gvStandardDocumentRequirements2.DataBind();
        }

        protected void btnSpecialBuyers_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string type = ddlBusinessType.SelectedValue;
                //string qry = $@"Select * from ""CRD7"" where ""BuyerType"" = '{type}'";

                //string qrycoowner = $@"Select B.*,A.""SpecifiedBusinessType"" from ""OCRD"" A
                //            INNER JOIN CRD7 B ON A.""CardCode"" = B.""CardCode""
                //            where B.""BuyerType"" = '{type}' AND IFNULL(A.""Approved"", '') <> '' AND A.""CardCode"" = '{Session["CardCode"].ToString()}'";
                gvCoOwner.DataSource = null;
                gvcoownerlist.DataSource = null;
                if (ddlBusinessType.SelectedValue != "Co-ownership" && ddlBusinessType.SelectedValue != "Individual" && ddlBusinessType.SelectedValue != "Corporation" && ddlBusinessType.SelectedValue != "Others")
                {
                    string trustguard = "";
                    trustguard = rbGuardian.Checked ? rbGuardee.Text : rbGuardian.Text;

                    string qrytrustguard = $@"Select B.""CardCode"",B.""LastName"",B.""FirstName"",B.""MiddleName"",A.""BusinessType"" as ""SpecifiedBusinessType"" from ""OCRD"" A
                            INNER JOIN CRD1 B ON A.""CardCode"" = B.""CardCode""
                            where A.""BusinessType"" = '{type}' AND IFNULL(A.""Approved"", '') <> '' AND A.""CardCode"" NOT IN('{Session["CardCode"].ToString()}','{ViewState["BuyerLink"].ToString()}') AND B.""CardType"" = 'Buyer' AND IFNULL(A.""SpecialBuyerRole"",'{rbGuardian.Text}') = '{trustguard}'";

                    DataTable dt = hana.GetData(qrytrustguard, hana.GetConnection("SAOHana"));
                    gvCoOwner.DataSource = dt;
                    gvCoOwner.Columns[4].Visible = false;
                    Div2.Visible = true;
                    gvCoOwner.DataBind();
                    ScriptManager.RegisterStartupScript(this, GetType(), "ModelHideBuyerModal", "ShowLinkBuyerModal();", true);
                }
                else if (ddlBusinessType.SelectedValue == "Co-ownership" || ddlBusinessType.SelectedValue == "Others")
                {
                    string Coowners = "";
                    DataTable dt = (DataTable)Session["dtCoOwner"];
                    foreach (DataRow row in dt.Rows)
                    {
                        Coowners += $@",'{row["CardCode"]}'";
                    }
                    string qry = $@"Select 
                                        B.""CardCode"",
                                        B.""LastName"",
                                        B.""FirstName"",
                                        B.""MiddleName"",
                                        A.""BusinessType"" as ""SpecifiedBusinessType"" 
                                    from 
                                        ""OCRD"" A INNER JOIN 
                                    CRD1 B ON A.""CardCode"" = B.""CardCode""
                            where A.""BusinessType"" = '{type}' AND IFNULL(A.""Approved"", '') <> '' AND A.""CardCode"" NOT IN('{Session["CardCode"].ToString()}'{Coowners}) AND B.""CardType"" = 'Buyer'";
                    dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
                    //Session["dtCoOwner"] = dt;
                    //gvCoOwner.Columns[11].Visible = true;
                    gvCoOwner.DataSource = dt;
                    gvCoOwner.Columns[4].Visible = true;
                    gvCoOwner.DataBind();
                    gvcoownerlist.Columns[5].Visible = false;
                    //dt = hana.GetData(qrycoowner, hana.GetConnection("SAOHana"));
                    LoadData(gvcoownerlist, "dtCoOwner");
                    //gvcoownerlist.Columns[4].Visible = ddlBusinessType.SelectedValue == "Others" ? true : false;
                    //gvcoownerlist.DataBind();
                    //string tHint = ddlBusinessType.SelectedValue == "Co-ownership" ? "Co-owner/s" : "Others";
                    //txtspecbuyer.Attributes.Add("placeholder", $"{tHint}");
                    Div2.Visible = true;
                    lblcoownerlisttitle.InnerText = ddlBusinessType.SelectedValue == "Co-ownership" ? "Linked Co-Owners" : "Linked Co-Buyers";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ModelHideBuyerModal", "ShowCoownerlist();", true);
                }
            }
            catch (Exception ex)
            {

                alertMsg(ex.Message, "error");
            }
        }

        protected void btnSelectLinkBuyer_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton GetID = (LinkButton)sender;
                string Code = GetID.CommandArgument;

                if (ddlBusinessType.SelectedValue != "Co-ownership" && ddlBusinessType.SelectedValue != "Individual" && ddlBusinessType.SelectedValue != "Corporation" && ddlBusinessType.SelectedValue != "Others")
                {
                    string qry = $@"Select A.""CardCode"",A.""LastName"",A.""FirstName"",A.""MiddleName"" from ""CRD1"" A
                            where A.""CardCode"" = '{Code}' AND A.""CardType"" = 'Buyer'";

                    DataTable dt = hana.GetData(qry, hana.GetConnection("SAOHana"));

                    ViewState["BuyerLink"] = Code;
                    txtspecbuyer.Value = ($@"{(string)DataAccess.GetData(dt, 0, "FirstName", "")} {(string)DataAccess.GetData(dt, 0, "MiddleName", "")} {(string)DataAccess.GetData(dt, 0, "LastName", "")}").Trim();
                }
                else if (ddlBusinessType.SelectedValue == "Co-ownership" || ddlBusinessType.SelectedValue == "Others")
                {
                    string qrycoowner = $@"Select B.""CardCode"",B.""LastName"",B.""FirstName"",B.""MiddleName"",A.""BusinessType"" as ""SpecifiedBusinessType"" from ""OCRD"" A
                            INNER JOIN CRD1 B ON A.""CardCode"" = B.""CardCode""
                            where IFNULL(A.""Approved"", '') <> '' AND A.""CardCode"" = '{Code}' AND B.""CardType"" = 'Buyer'";

                    DataTable dtownerlist = hana.GetData(qrycoowner, hana.GetConnection("SAOHana"));

                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["dtCoOwner"];
                    var rows = dt.Select($"CardCode = '{Code}'");
                    if (rows.Length <= 0)
                    {
                        dt.Rows.Add(
                            Code,
                            dtownerlist.Rows[0].ItemArray[2],
                            dtownerlist.Rows[0].ItemArray[3],
                            dtownerlist.Rows[0].ItemArray[1],
                            ddlBusinessType.SelectedValue
                            );
                        Session["dtCoOwner"] = dt;
                        LoadData(gvcoownerlist, "dtCoOwner");
                    }
                    else
                    {
                        alertMsg("The Buyers is already on the list.", "info");
                    }
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "ModelHideBuyerModal", "HideLinkBuyerModal();", true);
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }

            //string qry = $@"SELECT A.""CardCode"" FROM OCRD A WHERE A.""CardCode"" = '{Code}'";
            //DataTable dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
        }

        protected void txtName_TextChanged(object sender, EventArgs e)
        {
            if (ddlBusinessType.Text != "Corporation")
            {
                txtCertifyCompleteName.Value = ($@"{tFirstName.Text} {tMiddleName.Text} {tLastName.Text}").Trim();
            }
            else
            {
                txtCertifyCompleteName.Value = ($@"{tFirstName2.Text} {tMiddleName2.Text} {tLastName2.Text}").Trim();
            }
            tLastName.Focus();
        }

        protected void CustomValidator8_ServerValidate(object source, ServerValidateEventArgs args)
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

        void DeleteTemporaryFIles()
        {
            string Filepath = Server.MapPath("~/TEMP_DOCS/");
            string code = $@"*{ViewState["Guid"].ToString()}*.*";
            string[] fileList = Directory.GetFiles(Filepath, code);
            foreach (string file in fileList)
            {
                System.Diagnostics.Debug.WriteLine(file + "will be deleted");
                File.Delete(file);
            }
        }

        void LoadStandardDocuments(string document, DataTable dt, GridViewRow row)
        {
            if (!string.IsNullOrWhiteSpace(DataAccess.GetData(dt, 0, $"{document}", "").ToString()))
            {
                ((Label)row.FindControl("lblFileName1")).Text = DataAccess.GetData(dt, 0, document, "").ToString();
                ((RequiredFieldValidator)row.FindControl("RequiredFieldValidatorAttachments")).Enabled = false;
                //((CustomValidator)row.FindControl("customValidatorDocuments")).Enabled = false;
                visibleDocumentButtons(true, (LinkButton)row.FindControl("btnPreview3"), (LinkButton)row.FindControl("btnRemove3"));
            }
        }

        void LoadStandardDocuments2(string document, DataTable dt, GridViewRow row)
        {
            if (!string.IsNullOrWhiteSpace(DataAccess.GetData(dt, 0, $"{document}", "").ToString()))
            {
                ((Label)row.FindControl("lblFileName3")).Text = DataAccess.GetData(dt, 0, document, "").ToString();
                visibleDocumentButtons(true, (LinkButton)row.FindControl("btnPreview4"), (LinkButton)row.FindControl("btnRemove4"));
            }
        }

        protected void tSSSNo_TextChanged(object sender, EventArgs e)
        {
            if (tTypeOfId.SelectedValue == "ID2")
            {
                tIDNo.Text = tSSSNo.Text;
            }
            else if (tTypeOfId2.SelectedValue == "ID2")
            {
                tIDNo2.Text = tSSSNo.Text;
            }
            tGSISNo.Focus();
        }

        protected void tPagibigNo_TextChanged(object sender, EventArgs e)
        {
            if (tTypeOfId.SelectedValue == "ID3")
            {
                tIDNo.Text = tPagibigNo.Text;
            }
            else if (tTypeOfId2.SelectedValue == "ID3")
            {
                tIDNo2.Text = tPagibigNo.Text;
            }
            txtBusinessPhoneNo.Focus();
        }

        void HidePermAddress(bool val)
        {
            addtitle.InnerText = val ? "Complete Present Address" : "Registered Address.";

            nonCorpAddress.Visible = val;
            RequiredFieldValidator50.Enabled = val;
            RequiredFieldValidator52.Enabled = val;
            RequiredFieldValidator53.Enabled = val;
            RequiredFieldValidator54.Enabled = val;
            RequiredFieldValidator55.Enabled = val;
            RequiredFieldValidator56.Enabled = val;
            RequiredFieldValidator57.Enabled = val;
            RequiredFieldValidator58.Enabled = val;
        }

        protected void txtCompanyName_TextChanged(object sender, EventArgs e)
        {
            txtConformeCorp.Value = txtCompanyName.Text;
        }
        protected void tSPA_CheckedChanged(object sender, EventArgs e)
        {

            //divUploadSPADocs.Visible = tSPA.Checked;

            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);
        }
        protected void btnSPAUpload_Click(object sender, EventArgs e)
        {
            try
            {


                LinkButton btn = sender as LinkButton;
                string text = btn.Text;
                if (text == "Upload")
                {
                    if (SPAFileUpload.HasFile) //If the used Uploaded a file  
                    {
                        string code = ViewState["Guid"].ToString();

                        //Get FileName and Extension seperately
                        string fileNameOnly = Path.GetFileNameWithoutExtension(SPAFileUpload.FileName);
                        string extension = Path.GetExtension(SPAFileUpload.FileName);
                        string uniqueCode = code;

                        string FileName = fileNameOnly + "_" + code + extension; //Name of the file is stored in local Variable   
                        if (File.Exists(Server.MapPath("~/BUYER_SPA/") + FileName) || File.Exists(Server.MapPath("~/TEMP_DOCS/") + FileName))
                        {
                            alertMsg("File already exists!", "error");
                            //ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);
                            ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "MsgSPACoBorrower_Show();", true);
                        }
                        else
                        {
                            lblSPAFileName.Text = FileName;
                            SPAFileUpload.PostedFile.SaveAs(Server.MapPath("~/TEMP_DOCS/") + FileName); //File is saved in the Physical folder  

                            visibleDocumentButtons(true, btnSPAPreview, btnSPARemove);
                        }

                    }
                }
                else if (text == "Preview")
                {
                    if (File.Exists(Server.MapPath("~/TEMP_DOCS/") + lblSPAFileName.Text))
                    {
                        string Filepath = Server.MapPath("~/TEMP_DOCS/" + lblSPAFileName.Text);
                        var host = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.LocalPath, "");
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + host + "/TEMP_DOCS/" + lblSPAFileName.Text + "');", true);
                    }
                    else
                    {
                        string Filepath = Server.MapPath("~/BUYER_SPA/" + lblSPAFileName.Text);
                        var host = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.LocalPath, "");
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + host + "/BUYER_SPA/" + lblSPAFileName.Text + "');", true);
                    }
                }
                else if (text == "Remove")
                {
                    if (File.Exists(Server.MapPath("~/TEMP_DOCS/") + lblSPAFileName.Text))
                    {
                        // If file found, delete it    
                        File.Delete(Server.MapPath("~/TEMP_DOCS/") + lblSPAFileName.Text);
                        lblSPAFileName.Text = "";
                        visibleDocumentButtons(false, btnSPAPreview, btnSPARemove);
                    }
                    else if (File.Exists(Server.MapPath("~/BUYER_SPA/") + lblSPAFileName.Text))
                    {
                        // If file found, delete it    
                        File.Delete(Server.MapPath("~/BUYER_SPA/") + lblSPAFileName.Text);
                        lblSPAFileName.Text = "";
                        visibleDocumentButtons(false, btnSPAPreview, btnSPARemove);
                    }
                    else
                    {
                        // If not found, remove filename    
                        lblSPAFileName.Text = "";
                        visibleDocumentButtons(false, btnSPAPreview, btnSPARemove);
                    }
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "MsgSPACoBorrower_Show();", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "info");
            }
        }

        protected void CustomValidator9_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (rbGuardian.Checked == false && rbGuardee.Checked == false)
            {
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }
        }
        protected void rbGuardian_CheckedChanged(object sender, EventArgs e)
        {
            ChangeRole();
        }
        void ChangeRole()
        {
            if (rbGuardian.Checked)
            {
                if (ddlBusinessType.SelectedValue == "Trusteeship")
                {
                    RequiredFieldValidator7.Enabled = false;
                    spanTinReq.Visible = false;
                }
                else
                {
                    RequiredFieldValidator7.Enabled = true;
                    spanTinReq.Visible = true;
                }
                if (rbGuardian.Text == "Guardian")
                {
                    CustomValidator4.Enabled = true;
                    CustomValidator5.Enabled = true;
                    CustomValidator6.Enabled = true;
                }
                else
                {
                    CustomValidator4.Enabled = false;
                    CustomValidator5.Enabled = false;
                    CustomValidator6.Enabled = false;
                }
            }
            else if (rbGuardee.Checked)
            {
                if (ddlBusinessType.SelectedValue == "Guardianship")
                {
                    RequiredFieldValidator7.Enabled = false;
                    spanTinReq.Visible = false;
                }
                else
                {
                    RequiredFieldValidator7.Enabled = true;
                    spanTinReq.Visible = true;
                }
                CustomValidator4.Enabled = false;
                CustomValidator5.Enabled = false;
                CustomValidator6.Enabled = false;
            }
        }

        protected void btnAddtolist_Click(object sender, EventArgs e)
        {
            try
            {
                string type = ddlBusinessType.SelectedValue;
                string Coowners = "";
                DataTable dt = (DataTable)Session["dtCoOwner"];
                foreach (DataRow row in dt.Rows)
                {
                    Coowners += $@",'{row["CardCode"]}'";
                }
                string qry = $@"Select B.""CardCode"",B.""LastName"",B.""FirstName"",B.""MiddleName"",A.""BusinessType"" as ""SpecifiedBusinessType"" from ""OCRD"" A
                            INNER JOIN CRD1 B ON A.""CardCode"" = B.""CardCode""
                            where A.""BusinessType"" = '{type}' AND IFNULL(A.""Approved"", '') <> '' AND A.""CardCode"" NOT IN('{Session["CardCode"].ToString()}'{Coowners}) AND B.""CardType"" = 'Buyer'";
                dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
                //Session["dtCoOwner"] = dt;
                //gvCoOwner.Columns[11].Visible = true;
                gvCoOwner.DataSource = dt;
                gvCoOwner.Columns[4].Visible = true;
                gvCoOwner.DataBind();
            }
            catch (Exception ex)
            {

                alertMsg(ex.Message, "error");
            }
        }

        protected void gvcoownerlist_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvcoownerlist.PageIndex = e.NewPageIndex;
            LoadData(gvcoownerlist, "dtCoOwner");
        }

        protected void gvDeleteLinkBuyer_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            Control btn = (Control)sender;
            string type = ddlBusinessType.SelectedValue == "Co-ownership" ? "co-owner" : "co-Buyer";
            Session["BuyersCo_ID"] = GetID.CommandArgument;
            confirmation($@"Are you sure you want to delete the selected {type}?", "DelCoOwner");

        }
        //void DeleteTemporaryFIles()
        //{
        //    string buyerFullName = (tFirstName.Text + "_" + tMiddleName.Text + "_" + tLastName.Text).Trim().ToString().ToUpper();
        //    string code = ViewState["Guid"];
        //    string uniqueCode = code + "_" + tTIN.Text.Replace("-", "");
        //    string FolderName = uniqueCode + "_" + buyerFullName;

        //    string Filepath = Server.MapPath($"~/TEMP_DOCS/{FolderName}/");

        //    string filePattern = $@"*{ViewState["Guid"]}*.*";
        //    if (Directory.Exists(Filepath))
        //    {
        //        string[] fileList = Directory.GetFiles(Filepath, filePattern);
        //        foreach (string file in fileList)
        //        {
        //            System.Diagnostics.Debug.WriteLine(file + "will be deleted");
        //            File.Delete(file);
        //        }
        //    }

        //    if (System.IO.Directory.Exists(Server.MapPath($"~/TEMP_DOCS/{FolderName}")))
        //    {
        //        System.IO.Directory.Delete(Server.MapPath($"~/TEMP_DOCS/{FolderName}"));
        //    }
        //}
        void loadbuyer(string Code)
        {
            try
            {


                divCustomerCode.Visible = true;


                FirstLoad();
                DataTable ValidId = (DataTable)ViewState["ValidId"];
                DataTable ValidIdPrev = (DataTable)ViewState["ValidIdPrev"];
                lblSave.InnerText = "Update";
                DateTime date;
                string bdate;
                //LinkButton GetID = (LinkButton)sender;
                //string Code = GetID.CommandArgument;

                lblCustomerCode.Text = Code;

                Session["CardCode"] = Code;
                string tCode;
                ClearAll();
                DataTable dt = new DataTable();
                DataTable dtapprove = new DataTable();
                dtapprove = hana.GetData($@"SELECT 
		                          ""Approved""
                                 ,""ApprovedDocument""
                                FROM
                                    ""OCRD""
                                WHERE
                                    ""CardCode"" = '{Code}' ", hana.GetConnection("SAOHana"));
                //AND ""IsArchive"" = FALSE;
                string approved = (string)DataAccess.GetData(dtapprove, 0, "Approved", "N");
                lblAprvAttachment.Text = (string)DataAccess.GetData(dtapprove, 0, "ApprovedDocument", ""); ;
                DataTable dtaccess = (DataTable)Session["UserAccess"];
                var access = dtaccess.Select($"CodeEncrypt= 'SALES01'");


                //if (Session["UserName"].ToString() != "Sales01" && approved == "Y")
                if (!access.Any())
                {
                    btnApprove.Visible = false;
                    divAprv.Visible = true;
                    divAttch.Visible = String.IsNullOrEmpty(lblAprvAttachment.Text) ? false : true;
                }
                else
                {
                    if (approved != "Y")
                    {
                        btnApprove.Visible = true;
                        divAprv.Visible = false;
                        lblFileName.Text = (string)DataAccess.GetData(dtapprove, 0, "ApprovedDocument", "");
                    }
                    else
                    {
                        btnApprove.Visible = false;
                        divAprv.Visible = true;
                        divAttch.Visible = String.IsNullOrEmpty(lblAprvAttachment.Text) ? false : true;
                    }
                }











                //############################# OCRD ################################
                dt = hana.GetData($"CALL sp_BPEditOCRD ('{Code}')", hana.GetConnection("SAOHana"));

                Session["SQDocEntry"] = Code;

                //btnPrint.Visible = true;

                Session["SalesAgent"] = (string)DataAccess.GetData(dt, 0, "SalesAgent", "0");
                DataTable dtSA = hana.GetData($@"SELECT ""SalesPerson"" ""Name"" FROM ""OSLA"" WHERE ""Id"" = '{Session["SalesAgent"].ToString()}'", hana.GetConnection("SAOHana"));
                txtSalesAgent.Value = DataAccess.GetData(dtSA, 0, "Name", "").ToString();

                //SalesAgentDocument
                string SADocs = (string)DataAccess.GetData(dt, 0, "SalesAgentDocument", "");
                if (SADocs == "?" || SADocs == "")
                {
                    lblFileName.Text = "";
                    visibleDocumentButtons(false, btnPreview, btnRemove);
                }
                else
                {
                    lblFileName.Text = SADocs;
                    visibleDocumentButtons(true, btnPreview, btnRemove);
                }
                //tCode = (string)DataAccess.GetData(dt, 0, "NatureEmp", "N/A");
                //Session["tNatureofEmp"] = tCode;
                //tNatureofEmp.Value = ws.GetOLSTName(tCode);




                textAuthorizedPersonAddress.Value = DataAccess.GetData(dt, 0, "AuthorizedPersonAddress", "").ToString();
                txtAuthorizedPersonStreet.Value = DataAccess.GetData(dt, 0, "AuthorizedPersonStreet", "").ToString();
                txtAuthorizedPersonSubdivision.Value = DataAccess.GetData(dt, 0, "AuthorizedPersonSubdivision", "").ToString();
                txtAuthorizedPersonBarangay.Value = DataAccess.GetData(dt, 0, "AuthorizedPersonBarangay", "").ToString();
                txtAuthorizedPersonCity.Value = DataAccess.GetData(dt, 0, "AuthorizedPersonCity", "").ToString();
                txtAuthorizedPersonProvince.Value = DataAccess.GetData(dt, 0, "AuthorizedPersonProvince", "").ToString();




                tCode = (string)DataAccess.GetData(dt, 0, "IDType", "---Select Type of ID---");
                Session["tTypeOfId"] = tCode;
                //tTypeOfId.SelectedValue = ws.GetOLSTName(tCode);
                tTypeOfId.SelectedValue = tCode;

                tIDNo.Enabled = true;
                tIDNo.Text = (string)DataAccess.GetData(dt, 0, "IDNo", "N/A");
                txtOthers1.Text = (string)DataAccess.GetData(dt, 0, "IDOthers", "N/A");




                tCode = (string)DataAccess.GetData(dt, 0, "IDType2", "---Select Type of ID---");
                Session["tTypeOfId2"] = tCode;
                //tTypeOfId2.SelectedValue = ws.GetOLSTName(tCode);
                tTypeOfId2.SelectedValue = tCode;

                tIDNo2.Enabled = true;
                tIDNo2.Text = (string)DataAccess.GetData(dt, 0, "IDNo2", "N/A");
                txtOthers2.Text = (string)DataAccess.GetData(dt, 0, "IDOthers2", "N/A");





                tCode = (string)DataAccess.GetData(dt, 0, "IDType3", "---Select Type of ID---");
                Session["tTypeOfId3"] = tCode;
                //tTypeOfId3.SelectedValue = ws.GetOLSTName(tCode);
                tTypeOfId3.SelectedValue = tCode;

                tIDNo3.Enabled = true;
                tIDNo3.Text = (string)DataAccess.GetData(dt, 0, "IDNo3", "N/A");
                txtOthers3.Text = (string)DataAccess.GetData(dt, 0, "IDOthers3", "N/A");






                tCode = (string)DataAccess.GetData(dt, 0, "IDType4", "---Select Type of ID---");
                Session["tTypeOfId4"] = tCode;
                //tTypeOfId4.SelectedValue = ws.GetOLSTName(tCode);
                tTypeOfId4.SelectedValue = tCode;

                tIDNo4.Enabled = true;
                tIDNo4.Text = (string)DataAccess.GetData(dt, 0, "IDNo4", "N/A");
                txtOthers4.Text = (string)DataAccess.GetData(dt, 0, "IDOthers4", "N/A");





                //selectIndexChange1(tTypeOfId, 1);
                //selectIndexChange1(tTypeOfId2, 2);
                //selectIndexChange1(tTypeOfId3, 3);
                //selectIndexChange1(tTypeOfId4, 4);

                ValidIdDropDownEdit(ValidId, ValidIdPrev, tIDNo.Text, tTypeOfId, 1, tIDNo, txtOthers1);
                ValidIdDropDownEdit(ValidId, ValidIdPrev, tIDNo2.Text, tTypeOfId2, 2, tIDNo2, txtOthers2);
                ValidIdDropDownEdit(ValidId, ValidIdPrev, tIDNo3.Text, tTypeOfId3, 3, tIDNo3, txtOthers3);
                ValidIdDropDownEdit(ValidId, ValidIdPrev, tIDNo4.Text, tTypeOfId4, 4, tIDNo4, txtOthers4);








                tCode = (string)DataAccess.GetData(dt, 0, "SpecialInstructions", "");
                Session["SpecialInstructions"] = tCode;
                tSpecialInstructions.Value = tCode;

                tHomeTelNo.Value = (string)DataAccess.GetData(dt, 0, "HomeTelNo", "N/A");
                tPresentAddress.Text = (string)DataAccess.GetData(dt, 0, "PresentAddress", "N/A");
                tPermanent.Text = (string)DataAccess.GetData(dt, 0, "PermanentAddress", "N/A");
                //txtComaker.Value = (string)DataAccess.GetData(dt, 0, "Comaker", "N/A");

                string HomeOwnership = (string)DataAccess.GetData(dt, 0, "HomeOwnership", "N/A");

                //tPerMonth.Disabled = true;
                //if (HomeOwnership == "Owned")
                //{ tRented_CheckedChanged(tOwned, EventArgs.Empty); }
                //else if (HomeOwnership == "Mortgaged")
                //{ tMortgaged.Checked = true; }
                //else if (HomeOwnership == "LivingwRelatives")
                //{ tLivingwRelatives.Checked = true; }
                //else
                //{
                //    tRented.Checked = true;
                //    tPerMonth.Disabled = false;
                //    tPerMonth.Value = HomeOwnership;
                //}

                //tYearsOfStay.Value = (string)DataAccess.GetData(dt, 0, "YearsStay", "");

                tCode = (string)DataAccess.GetData(dt, 0, "CivilStatus", "---Select Civil Status---");

                if (ws.OLSTExist(tCode) == true)
                { Session["tCivilStatus"] = tCode; /*tCivilStatus.SelectedValue = ws.GetOLSTName(tCode);*/ tCivilStatus.SelectedValue = tCode; }
                else { Session["tCivilStatus"] = "OTH"; tCivilStatus.SelectedValue = tCode; }


                tRemarks.Value = (string)DataAccess.GetData(dt, 0, "Remarks", "");
                ddlBusinessType.Text = (string)DataAccess.GetData(dt, 0, "BusinessType", "Individual");
                loadDivisionsForNames(ddlBusinessType.Text);
                string taxclass = (string)DataAccess.GetData(dt, 0, "TaxClassification", "");
                if (taxclass.ToLower() == ConfigSettings.TaxClassification1.ToLower() || taxclass.ToLower() == ConfigSettings.TaxClassification2.ToLower() || taxclass.ToLower() == "corporation")
                {
                    ddTaxClass.SelectedValue = taxclass;
                }
                ddSourceFunds.SelectedValue = (string)DataAccess.GetData(dt, 0, "SourceOfFunds", "---Select Source of Funds---");
                txtOtherSourceOfFund.Text = (string)DataAccess.GetData(dt, 0, "OtherSourceOfFund", "N/A");
                otherFields();
                txtPresPostal.Text = (string)DataAccess.GetData(dt, 0, "PresentPostalCode", "N/A");
                txtPermPostal.Text = (string)DataAccess.GetData(dt, 0, "PermanentPostalCode", "N/A");
                ddPreCountry.SelectedValue = (string)DataAccess.GetData(dt, 0, "PresentCountry", "PH");
                ddPermCountry.SelectedValue = (string)DataAccess.GetData(dt, 0, "PermanentCountry", "PH");
                txtPermYrStay.Text = (string)DataAccess.GetData(dt, 0, "PermanentYrStay", "N/A");
                txtPresYrStay.Text = (string)DataAccess.GetData(dt, 0, "PresentYrStay", "N/A");
                txtProfession.Text = (string)DataAccess.GetData(dt, 0, "Profession", "N/A");
                ddOccupation.SelectedValue = (string)DataAccess.GetData(dt, 0, "Occupation", "N/A");
                ddMonthlyIncome.SelectedValue = (string)DataAccess.GetData(dt, 0, "MonthlyIncome", "") == "0" ? "" : (string)DataAccess.GetData(dt, 0, "MonthlyIncome", "");
                txtBusinessPhoneNo.Value = (string)DataAccess.GetData(dt, 0, "BusinessPhoneNo", "N/A");
                txtCertifyCompleteName.Value = (string)DataAccess.GetData(dt, 0, "CertifyCompleteName", "N/A");
                txtPresentStreet.Text = (string)DataAccess.GetData(dt, 0, "PresentStreet", "N/A");
                txtPresentSubdivision.Text = (string)DataAccess.GetData(dt, 0, "PresentSubdivision", "N/A");
                txtPresentBarangay.Text = (string)DataAccess.GetData(dt, 0, "PresentBarangay", "N/A");
                txtPresentCity.Text = (string)DataAccess.GetData(dt, 0, "PresentCity", "N/A");
                txtPresentProvince.Text = (string)DataAccess.GetData(dt, 0, "PresentProvince", "N/A");
                txtPermanentStreet.Text = (string)DataAccess.GetData(dt, 0, "PermanentStreet", "N/A");
                txtPermanentSubdivision.Text = (string)DataAccess.GetData(dt, 0, "PermanentSubdivision", "N/A");
                txtPermanentBarangay.Text = (string)DataAccess.GetData(dt, 0, "PermanentBarangay", "N/A");
                txtPermanentCity.Text = (string)DataAccess.GetData(dt, 0, "PermanentCity", "N/A");
                txtPermanentProvince.Text = (string)DataAccess.GetData(dt, 0, "PermanentProvince", "N/A");
                txtSpecifyBusiness.Text = (string)DataAccess.GetData(dt, 0, "SpecifiedBusinessType", "N/A");
                txtReligion.Text = (string)DataAccess.GetData(dt, 0, "Religion", "N/A");
                txtSECCORIDNo.Text = (string)DataAccess.GetData(dt, 0, "SECCORIDNo", "N/A");
                string conformecheck = (string)DataAccess.GetData(dt, 0, "Conforme", "false");
                //if (conformecheck.ToLower() == "true" || conformecheck == "1")
                //{
                //    CBconforme.Checked = true;
                //}
                //else if (conformecheck.ToLower() == "true" || conformecheck == "0")
                //{
                //    CBconforme.Checked = false;
                //}
                string CertifyDate = (string)DataAccess.GetData(dt, 0, "CertifyDate", "");
                if (!string.IsNullOrEmpty(CertifyDate))
                {
                    date = DateTime.Parse(CertifyDate);
                    CertifyDate = date.ToString("yyyy-MM-dd");
                }
                txtCertifyDate.Value = CertifyDate;

                string SpecialBuyerRole = (string)DataAccess.GetData(dt, 0, "SpecialBuyerRole", "");
                if (SpecialBuyerRole != "")
                {
                    if (SpecialBuyerRole == "Guardian" || SpecialBuyerRole == "Trustor")
                    {
                        rbGuardian.Checked = true;
                    }
                    else
                    {
                        rbGuardee.Checked = true;
                    }
                }
                else
                {
                    if (ddlBusinessType.SelectedValue == "Guardianship" || ddlBusinessType.SelectedValue == "Trusteeship")
                    {
                        rbGuardian.Checked = true;
                    }
                }
                ChangeRole();

                foreach (GridViewRow row in gvStandardDocumentRequirements.Rows)
                {
                    if (row.Cells[0].Text == "Proof of billing")
                    {
                        LoadStandardDocuments("ProofOfBillingAttachment", dt, row);
                    }
                    else if (row.Cells[0].Text == "Valid ID 1")
                    {
                        LoadStandardDocuments("ValidId1Attachment", dt, row);
                    }
                    else if (row.Cells[0].Text == "Valid ID 2")
                    {
                        LoadStandardDocuments("ValidId2Attachment", dt, row);
                    }
                    else if (row.Cells[0].Text == "Proof of Income")
                    {
                        LoadStandardDocuments("ProofOfIncomeAttachment", dt, row);
                    }
                }

                foreach (GridViewRow row in gvStandardDocumentRequirements2.Rows)
                {
                    if (row.Cells[0].Text == "Proof of billing")
                    {
                        LoadStandardDocuments2("ProofOfBillingAttachment", dt, row);
                    }
                    else if (row.Cells[0].Text == "Valid ID 1")
                    {
                        LoadStandardDocuments2("ValidId1Attachment", dt, row);
                    }
                    else if (row.Cells[0].Text == "Valid ID 2")
                    {
                        LoadStandardDocuments2("ValidId2Attachment", dt, row);
                    }
                    else if (row.Cells[0].Text == "Proof of Income")
                    {
                        LoadStandardDocuments2("ProofOfIncomeAttachment", dt, row);
                    }
                }

                //############################## END OCRD ################################

                //############################# CRD1 ################################

                dt = hana.GetData($"CALL sp_BPEditCRD1 ('{Code}','Buyer')", hana.GetConnection("SAOHana"));

                tLastName.Text = (string)DataAccess.GetData(dt, 0, "LastName", "N/A") == " " ? "N/A" : (string)DataAccess.GetData(dt, 0, "LastName", "N/A");
                tFirstName.Text = (string)DataAccess.GetData(dt, 0, "FirstName", "N/A") == " " ? "N/A" : (string)DataAccess.GetData(dt, 0, "FirstName", "N/A");
                tMiddleName.Text = (string)DataAccess.GetData(dt, 0, "MiddleName", "N/A") == " " ? "N/A" : (string)DataAccess.GetData(dt, 0, "MiddleName", "N/A");
                tLastName2.Text = (string)DataAccess.GetData(dt, 0, "LastName", "N/A") == " " ? "N/A" : (string)DataAccess.GetData(dt, 0, "LastName", "N/A");
                tFirstName2.Text = (string)DataAccess.GetData(dt, 0, "FirstName", "N/A") == " " ? "N/A" : (string)DataAccess.GetData(dt, 0, "FirstName", "N/A");
                tMiddleName2.Text = (string)DataAccess.GetData(dt, 0, "MiddleName", "N/A") == " " ? "N/A" : (string)DataAccess.GetData(dt, 0, "MiddleName", "N/A");


                txtCompanyName.Text = (string)DataAccess.GetData(dt, 0, "CompanyName", "N/A");
                txtConformeCorp.Value = (string)DataAccess.GetData(dt, 0, "CompanyName", "N/A");

                loadDivisionsPerBusinessType();


                //2023-06-27 : GET FROM SPOUSE BUSINESS COUNTRY
                //ddSPOBusCountry.SelectedValue = (string)DataAccess.GetData(dt, 0, "EmpBusCountry", "PH");
                ddSPOBusCountry.SelectedValue = (string)DataAccess.GetData(dt, 0, "SpouseBusCountry", "PH");


                tGender.Value = (string)DataAccess.GetData(dt, 0, "Gender", "N/A");
                tCitizenship.Value = (string)DataAccess.GetData(dt, 0, "Citizenship", "N/A");

                bdate = (string)DataAccess.GetData(dt, 0, "BirthDay", "");
                if (!string.IsNullOrEmpty(bdate))
                {
                    date = DateTime.Parse(bdate);
                    bdate = date.ToString("yyyy-MM-dd");
                }

                tBirthDate.Value = bdate;
                tBirthPlace.Value = (string)DataAccess.GetData(dt, 0, "BirthPlace", "N/A");
                tCellphoneNo.Value = (string)DataAccess.GetData(dt, 0, "CellNo", "N/A");
                tEmpEmailAddress.Value = (string)DataAccess.GetData(dt, 0, "EmailAddress", "N/A");
                tFBAccount.Value = (string)DataAccess.GetData(dt, 0, "FBAccount", "N/A");
                //tTIN.Value = (string)DataAccess.GetData(dt, 0, "TIN", "N/A");
                tTIN.Text = (string)DataAccess.GetData(dt, 0, "TIN", "N/A");
                tTINCorp.Text = (string)DataAccess.GetData(dt, 0, "TIN", "N/A");
                tSSSNo.Text = (string)DataAccess.GetData(dt, 0, "SSSNo", "N/A");
                tGSISNo.Value = (string)DataAccess.GetData(dt, 0, "GSISNo", "N/A");
                tPagibigNo.Text = (string)DataAccess.GetData(dt, 0, "PagibiNo", "N/A");
                tPosition.Value = (string)DataAccess.GetData(dt, 0, "Position", "N/A");
                tYearsofService.Value = (string)DataAccess.GetData(dt, 0, "YearsService", "0");
                tOfficeTelNo.Value = (string)DataAccess.GetData(dt, 0, "OfficeTelNo", "N/A");
                tFAXNo.Value = (string)DataAccess.GetData(dt, 0, "FaxNo", "N/A");
                tEmpBusinessName.Value = (string)DataAccess.GetData(dt, 0, "EmpBusName", "N/A");
                tEmpBusinessAddress.Value = (string)DataAccess.GetData(dt, 0, "EmpBusAdd", "N/A");

                //txtProfession.Text = (string)DataAccess.GetData(dt, 0, "SourceOfFunds", "N/A");       

                tCode = (string)DataAccess.GetData(dt, 0, "EmpStatus", "---Select Employment Status---");
                if (tCode == "ES1" || tCode == "ES2" || tCode == "ES3" || tCode == "ES4" || tCode == "ES5")
                {
                    //tEmpStatus.SelectedValue = ws.GetOLSTName(tCode);
                    tEmpStatus.SelectedValue = tCode;
                    Session["tEmpStatus"] = tCode;
                }
                else
                {
                    Session["tEmpStatus"] = "OTH";
                    tEmpStatus.SelectedValue = tCode;
                }

                taxClassChanged();

                tCode = (string)DataAccess.GetData(dt, 0, "NatureEmp", "---Select Nature of Employment---");

                if (ws.OLSTExist(tCode) == true)
                { Session["tNatureEmployment"] = tCode; /*tNatureEmployment.SelectedValue = ws.GetOLSTName(tCode);*/ tNatureEmployment.SelectedValue = tCode; }
                else { Session["tNatureEmployment"] = "---Select Nature of Employment---"; tNatureEmployment.SelectedValue = "---Select Nature of Employment---"; }

                dt = hana.GetData($"CALL sp_BPEditCRD1 ('{Code}','Spouse')", hana.GetConnection("SAOHana"));

                tSpouseLastName.Value = (string)DataAccess.GetData(dt, 0, "LastName", "N/A");
                tSpouseFirstName.Value = (string)DataAccess.GetData(dt, 0, "FirstName", "N/A");
                tSpouseMiddleName.Value = (string)DataAccess.GetData(dt, 0, "MiddleName", "N/A");
                tSpouseGender.Value = (string)DataAccess.GetData(dt, 0, "Gender", "N/A");
                tSpouseCitizenship.Value = (string)DataAccess.GetData(dt, 0, "Citizenship", "N/A");

                bdate = (string)DataAccess.GetData(dt, 0, "BirthDay", "");
                if (!string.IsNullOrEmpty(bdate))
                {
                    date = DateTime.Parse(bdate);
                    bdate = date.ToString("yyyy-MM-dd");
                }

                tSpouseBirthDate.Value = bdate;
                tSpouseBirthPlace.Value = (string)DataAccess.GetData(dt, 0, "BirthPlace", "N/A");
                tSpouseCellphoneNo.Value = (string)DataAccess.GetData(dt, 0, "CellNo", "N/A");
                tSpouseEmailAdd.Value = (string)DataAccess.GetData(dt, 0, "EmailAddress", "N/A");
                tSpouseFBAccount.Value = (string)DataAccess.GetData(dt, 0, "FBAccount", "N/A");

                //COMMENTED 06 - 13 - 2023 : CHANGE REQUEST  https://direcbti.monday.com/boards/4462005457/pulses/4576973637?asset_id=908456598 --%>
                //tSpouseTIN.Value = (string)DataAccess.GetData(dt, 0, "TIN", "N/A");
                tSpouseTIN2.Text = (string)DataAccess.GetData(dt, 0, "TIN", "N/A");

                tSpouseSSSNo.Value = (string)DataAccess.GetData(dt, 0, "SSSNo", "N/A");
                tSpouseGSISNo.Value = (string)DataAccess.GetData(dt, 0, "GSISNo", "N/A");
                tSpousePagibigNo.Value = (string)DataAccess.GetData(dt, 0, "PagibiNo", "N/A");
                tSpousePosition.Value = (string)DataAccess.GetData(dt, 0, "Position", "N/A");
                tSpouseYearsofService.Value = (string)DataAccess.GetData(dt, 0, "YearsService", "0");
                tSpouseOfficeTelNo.Value = (string)DataAccess.GetData(dt, 0, "OfficeTelNo", "N/A");
                tSpouseFAXNo.Value = (string)DataAccess.GetData(dt, 0, "FaxNo", "N/A");
                tSpouseEmpBusinessName.Value = (string)DataAccess.GetData(dt, 0, "EmpBusName", "N/A");
                tSpouseEmpBusinessAddress.Value = (string)DataAccess.GetData(dt, 0, "EmpBusAdd", "N/A");
                txtSPOAddress.Text = (string)DataAccess.GetData(dt, 0, "PresentAddress", "N/A");

                tCode = (string)DataAccess.GetData(dt, 0, "EmpStatus", "");

                if (ws.OLSTExist(tCode) == true)
                {
                    Session["tSpouseEmpStatus"] = tCode;
                    /*tSpouseEmpStatus.SelectedValue = ws.GetOLSTName(tCode);*/
                    tSpouseEmpStatus.SelectedValue = tCode;
                }
                else
                {
                    Session["tSpouseEmpStatus"] = "OTH";

                    tSpouseEmpStatus.SelectedValue = (tCode == "N/A" ? "" : tCode);
                }

                tCode = (string)DataAccess.GetData(dt, 0, "NatureEmp", "");

                if (ws.OLSTExist(tCode) == true)
                {
                    Session["tSpouseNatureEmp"] = tCode;
                    /*tSpouseNatureEmp.SelectedValue = ws.GetOLSTName(tCode);*/
                    tSpouseNatureEmp.SelectedValue = tCode;
                }
                else
                {
                    Session["tSpouseNatureEmp"] = "";

                    tSpouseNatureEmp.SelectedValue = "";
                }

                //############################## END CRD1 ################################


                //############################# temp_TABLE ################################

                //dt = ws.Select($"sp_EditSPACBDependent '{Code}'", "dtSPACBDependent", "Addon").Tables["dtSPACBDependent"];
                //Session["dtSPACBDependent"] = dt;

                //dt = ws.Select($"sp_EditSPACBList '{Code}'", "dtListSPACoBorrower", "Addon").Tables["dtListSPACoBorrower"];
                //Session["dtListSPACoBorrower"] = dt;

                hana.Execute($"CALL sp_EditSPACBDependent ('{Code}',{(int)Session["UserID"]})", hana.GetConnection("SAOHana"));

                //foreach (DataRow dr in ws.Select($"sp_EditSPACBList '{Code}',{(int)Session["UserID"]}", "dtListSPACoBorrower", "Addon").Tables["dtListSPACoBorrower"].Rows)
                //{
                //    DataRow dr1 = dt1.NewRow();
                //    dr1[0] = int.Parse(dr["ID"].ToString());
                //    dr1[1] = dr["Relationship"].ToString();
                //    dr1[2] = $"{dr["LastName"].ToString()}, {dr["FirstName"].ToString()} {dr["MiddleName"].ToString()[0]}";
                //    dr1[3] = dr["Gender"].ToString();
                //    dr1[4] = dr["Email"].ToString();
                //    dt1.Rows.Add(dr1);
                //}
                dt = hana.GetData($"cALL sp_EditSPACBList ('{Code}',{(int)Session["UserID"]})", hana.GetConnection("SAOHana"));

                //2023-06-18 : CHANGE TO ViewState
                //Session["gvSPACoBorrower"] = dt;
                //LoadData(gvSPACoBorrower, "gvSPACoBorrower");
                ViewState["gvSPACoBorrower"] = dt;
                LoadDataViewState(gvSPACoBorrower, "gvSPACoBorrower");


                //2023-06-18 : CHANGE TO ViewState
                //dt = (DataTable)Session["dtDependent"];
                dt = (DataTable)ViewState["dtDependent"];

                foreach (DataRow dr in hana.GetData($@"SELECT ""Name"",""Age"",""Relationship"" FROM ""CRD2"" WHERE ""DependentType"" = 'B' AND ""CardCode"" = '{Code}'", hana.GetConnection("SAOHana")).Rows)
                {
                    DataRow adr = dt.NewRow();

                    adr[0] = Convert.ToInt32(dt.Rows.Count + 1);
                    adr[1] = dr["Name"].ToString();
                    adr[2] = Convert.ToUInt64(dr["Age"]);
                    adr[3] = dr["Relationship"].ToString();
                    dt.Rows.Add(adr);
                }

                //2023-06-18 : CHANGE TO ViewState
                //Session["dtDependent"] = dt;
                //LoadData(gvDependent, "dtDependent");
                ViewState["dtDependent"] = dt;
                LoadDataViewState(gvDependent, "dtDependent");

                dt = (DataTable)Session["dtSPADependent"];

                foreach (DataRow dr in hana.GetData($@"SELECT ""Name"",""Age"",""Relationship"" FROM ""CRD2"" WHERE ""DependentType"" = 'S' AND ""CardCode"" = '{Code}'", hana.GetConnection("SAOHana")).Rows)
                {
                    DataRow adr = dt.NewRow();

                    adr[0] = Convert.ToInt32(dt.Rows.Count + 1);
                    adr[1] = dr["Name"].ToString();
                    adr[2] = Convert.ToUInt64(dr["Age"]);
                    adr[3] = dr["Relationship"].ToString();
                    dt.Rows.Add(adr);
                }
                Session["dtSPADependent"] = dt;
                LoadData(gvSPADependent, "dtSPADependent");

                //2023-06-18 : CHANGE TO ViewState
                //dt = (DataTable)Session["dtBankAccount"];
                dt = (DataTable)ViewState["dtBankAccount"];

                foreach (DataRow dr in hana.GetData($@"SELECT ""Bank"",""Branch"",""AcctType"",""AcctNo"",IFNULL(""AvgDailyBal"",0) ""AvgDailyBal"",IFNULL(""PresentBal"",0) ""PresentBal"" FROM ""CRD3"" WHERE ""CardCode"" = '{Code}'", hana.GetConnection("SAOHana")).Rows)
                {
                    DataRow adr = dt.NewRow();

                    adr[0] = Convert.ToInt32(dt.Rows.Count + 1);
                    adr[1] = dr["Bank"].ToString();
                    adr[2] = dr["Branch"].ToString();
                    adr[3] = dr["AcctType"].ToString();
                    adr[4] = dr["AcctNo"].ToString();
                    adr[5] = Convert.ToUInt64(dr["AvgDailyBal"]);
                    adr[6] = Convert.ToUInt64(dr["PresentBal"]);
                    dt.Rows.Add(adr);
                }
                //2023-06-18 : CHANGE TO ViewState
                //Session["dtBankAccount"] = dt;
                //LoadData(gvBankAccount, "dtBankAccount");                
                ViewState["dtBankAccount"] = dt;
                LoadDataViewState(gvBankAccount, "dtBankAccount");

                //2023-06-18 : CHANGE TO ViewState
                //dt = (DataTable)Session["dtCharacterRef"];
                dt = (DataTable)ViewState["dtCharacterRef"];

                foreach (DataRow dr in hana.GetData($@"SELECT ""Name"",""Address"",""Email"",""TelNo"" FROM ""CRD4"" WHERE ""CardCode"" = '{Code}'", hana.GetConnection("SAOHana")).Rows)
                {
                    DataRow adr = dt.NewRow();

                    adr[0] = Convert.ToInt32(dt.Rows.Count + 1);
                    adr[1] = dr["Name"].ToString();
                    adr[2] = dr["Address"].ToString().Replace("amp;", "");
                    adr[3] = dr["Email"].ToString();
                    adr[4] = dr["TelNo"].ToString();
                    dt.Rows.Add(adr);
                }

                //2023-06-18 : CHANGE TO ViewState
                //Session["dtCharacterRef"] = dt;
                //LoadData(gvCharacterRef, "dtCharacterRef");
                ViewState["dtCharacterRef"] = dt;
                LoadDataViewState(gvCharacterRef, "dtCharacterRef");

                //############################## END temp_TABLE ################################

                //############################# CRD7 ################################
                dt = hana.GetData($@"SELECT * FROM CRD7 WHERE ""CardCode"" = '{Code}' AND ""BuyerType"" = '{ddlBusinessType.Text}'", hana.GetConnection("SAOHana"));
                if (dt.Rows.Count > 0)
                {
                    string qrycoowner = $@"Select A.""SpecialBuyerLink"" as ""CardCode"",B.""LastName"",B.""FirstName"",B.""MiddleName"",A.""BuyerType"" as ""SpecifiedBusinessType"" from ""CRD7"" A
                            INNER JOIN CRD1 B ON A.""SpecialBuyerLink"" = B.""CardCode""
                            where A.""CardCode"" = '{Session["CardCode"].ToString()}' AND B.""CardType"" = 'Buyer'";

                    dt = hana.GetData(qrycoowner, hana.GetConnection("SAOHana"));
                    //####GUARDIANSHIP/TRUSTEESHIP####
                    if (ddlBusinessType.Text == "Guardianship" || ddlBusinessType.Text == "Trusteeship")
                    {
                        ViewState["BuyerLink"] = (string)DataAccess.GetData(dt, 0, "SpecialBuyerLink", "");
                        txtspecbuyer.Value = ($@"{(string)DataAccess.GetData(dt, 0, "FirstName", "")} {(string)DataAccess.GetData(dt, 0, "MiddleName", "")} {(string)DataAccess.GetData(dt, 0, "LastName", "")}").Trim();
                    }
                    else if (ddlBusinessType.Text == "Co-ownership" || ddlBusinessType.Text == "Others")
                    {
                        Session["dtCoOwner"] = dt;
                        LoadData(gvcoownerlist, "dtCoOwner");
                    }
                }
                //############################## END CRD7 ################################

                DeleteTemporaryFIles();

                access = dtaccess.Select($"CodeEncrypt= 'SALES' or CodeEncrypt= 'SALES01'");
                if (!access.Any())
                {
                    btnSave.Visible = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "MsgBuyers_Hide();", true);
                }
                else
                {
                    btnSave.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "MsgBuyers_HideIT();", true);
                }

                GetBuyerAttachments(gvStandardDocumentRequirements, "FileUpload3", "lblFileName1", "btnPreview3", "btnRemove3", "RequiredFieldValidatorAttachments");

            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "warning");
            }
        }

        protected void tSpouseEmpStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowSpouseBusiness();
        }

        void ShowSpouseBusiness()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);

            if (tSpouseEmpStatus.SelectedValue == "")
            {
                dvSpouseDetails.Attributes.Clear();
                dvSpouseDetails.Attributes.Add("class", "col-md-12");
                divSpouseBusiness.Visible = false;
                tSpouseNatureEmp.SelectedValue = "";
                tSpouseEmpBusinessName.Value = string.Empty;
                tSpouseEmpBusinessAddress.Value = string.Empty;
                tSpousePosition.Value = string.Empty;
                tSpouseYearsofService.Value = string.Empty;
                tSpouseOfficeTelNo.Value = string.Empty;
                tSpouseFAXNo.Value = string.Empty;

                //COMMENTED 06 - 13 - 2023 : CHANGE REQUEST  https://direcbti.monday.com/boards/4462005457/pulses/4576973637?asset_id=908456598 --%>
                //tSpouseTIN.Value = string.Empty;
                tSpouseTIN2.Text = string.Empty;
                //COMMENTED 06 - 13 - 2023 : CHANGE REQUEST  https://direcbti.monday.com/boards/4462005457/pulses/4576973637?asset_id=908456598 --%>
                RequiredFieldValidator12.Enabled = false;

                tSpouseSSSNo.Value = string.Empty;
                tSpouseGSISNo.Value = string.Empty;
                tSpousePagibigNo.Value = string.Empty;
            }
            else
            {
                dvSpouseDetails.Attributes.Clear();
                dvSpouseDetails.Attributes.Add("class", "col-md-6");
                divSpouseBusiness.Visible = true;


                //COMMENTED 06 - 13 - 2023 : CHANGE REQUEST  https://direcbti.monday.com/boards/4462005457/pulses/4576973637?asset_id=908456598 --%>
                RequiredFieldValidator12.Enabled = true;

            }


        }

        protected void btnCopyPrincipalAddress_Click(object sender, EventArgs e)
        {
            txtSPOAddress.Text = (tPresentAddress.Text.ToUpper() == "N/A" ? "" : $@"{tPresentAddress.Text} ")
                + (txtPresentStreet.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentStreet.Text} ")
                + (txtPresentSubdivision.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentSubdivision.Text} ")
                + (txtPresentBarangay.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentBarangay.Text} ")
                + (txtPresentCity.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentCity.Text} ")
                + (txtPresentProvince.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentProvince.Text} ");

            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);
        }

        protected void btnCopyPrincipalAddressSPA_Click(object sender, EventArgs e)
        {
            tSPAPresentAddress.Value = (tPresentAddress.Text.ToUpper() == "N/A" ? "" : $@"{tPresentAddress.Text} ")
                + (txtPresentStreet.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentStreet.Text} ")
                + (txtPresentSubdivision.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentSubdivision.Text} ")
                + (txtPresentBarangay.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentBarangay.Text} ")
                + (txtPresentCity.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentCity.Text} ")
                + (txtPresentProvince.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentProvince.Text} ");

            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);
        }

        protected void btnSameasPresent_Click(object sender, EventArgs e)
        {
            tPermanent.Text = tPresentAddress.Text;
            txtPermanentStreet.Text = txtPresentStreet.Text;
            txtPermanentSubdivision.Text = txtPresentSubdivision.Text;
            txtPermanentBarangay.Text = txtPresentBarangay.Text;
            txtPermanentCity.Text = txtPresentCity.Text;
            txtPermanentProvince.Text = txtPresentProvince.Text;
            txtPermPostal.Text = txtPresPostal.Text;
            ddPermCountry.SelectedValue = ddPreCountry.SelectedValue;
            txtPermYrStay.Text = txtPresYrStay.Text;


            //2023-04-25 : Commented to prevent Postback when inputting data on textboxes
            //hidebtnSameasPresent();
        }
        void hidebtnSameasPresent()
        {
            string present = ((tPresentAddress.Text.ToUpper() == "N/A" ? "" : $@"{tPresentAddress.Text} ")
                + (txtPresentStreet.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentStreet.Text} ")
                + (txtPresentSubdivision.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentSubdivision.Text} ")
                + (txtPresentBarangay.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentBarangay.Text} ")
                + (txtPresentCity.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentCity.Text} ")
                + (txtPresentProvince.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentProvince.Text} ")
                + (txtPresPostal.Text.ToUpper() == "N/A" ? "" : $@"{txtPresPostal.Text} ")
                + (txtPresYrStay.Text.ToUpper() == "N/A" ? "" : $@"{txtPresYrStay.Text} ")).Trim().ToUpper();

            string permanent = ((tPermanent.Text.ToUpper() == "N/A" ? "" : $@"{tPermanent.Text} ")
                + (txtPermanentStreet.Text.ToUpper() == "N/A" ? "" : $@"{txtPermanentStreet.Text} ")
                + (txtPermanentSubdivision.Text.ToUpper() == "N/A" ? "" : $@"{txtPermanentSubdivision.Text} ")
                + (txtPermanentBarangay.Text.ToUpper() == "N/A" ? "" : $@"{txtPermanentBarangay.Text} ")
                + (txtPermanentCity.Text.ToUpper() == "N/A" ? "" : $@"{txtPermanentCity.Text} ")
                + (txtPermanentProvince.Text.ToUpper() == "N/A" ? "" : $@"{txtPermanentProvince.Text} ")
                + (txtPermPostal.Text.ToUpper() == "N/A" ? "" : $@"{txtPermPostal.Text} ")
                + (txtPermYrStay.Text.ToUpper() == "N/A" ? "" : $@"{txtPermYrStay.Text} ")).Trim().ToUpper();

            if (present == permanent && (present != "" && permanent != ""))
            {
                if (ddPermCountry.SelectedValue == ddPermCountry.SelectedValue)
                {
                    btnSameasPresent.Visible = false;
                }
                else
                {
                    btnSameasPresent.Visible = true;
                }
            }
            else
            {
                btnSameasPresent.Visible = true;
            }

            //txtPresentBarangay.Focus();
        }
        protected void txtAddress_TextChanged(object sender, EventArgs e)
        {
            //hidebtnSameasPresent();
        }
        protected void ddPreCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            //hidebtnSameasPresent();
        }
        protected void btnUpdatetoSAP_Click(object sender, EventArgs e)
        {
            try
            {
                int bpSeries = int.Parse(ConfigSettings.BPSeries);
                string clearingAccount = ConfigSettings.ClearingAccount;
                int bpListNum = int.Parse(ConfigSettings.BPPriceList);
                string PayTermsGrpCode = ConfigSettings.PayTermsGrpCode;

                string SapCardCode, ErrMsg;
                string CardName = ddlBusinessType.SelectedValue != "Corporation" ? $@"{tLastName.Text}, {tFirstName.Text}" : txtCompanyName.Text;
                string LastName = tLastName.Text;
                string FirstName = tFirstName.Text;
                string MiddleName = tMiddleName.Text;
                string TIN = tTIN.Text;
                string TaxClassification = ddTaxClass.SelectedValue;

                SapHanaLayer company = new SapHanaLayer();
                company.Connect();

                DataTable dt = hana.GetData($@"SELECT TOP 1 ""DocEntry"",""CardCode"",""CardName"",""CardFName"" FROM ""OCRD"" WHERE ""U_DreamsCustCode"" = '{lblCustomerCode.Text}'", hana.GetConnection("SAPHana"));

                IDictionary<string, object> BusinessPartners = new Dictionary<string, object>()
                    {
                    { "CardName" , CardName },
                    { "CardForeignName" , CardName },
                    { "U_LName" , LastName},
                    { "U_FName" , FirstName},
                    { "U_MName" , MiddleName},
                    { "FederalTaxID" , TIN },
                    { "U_TaxClass" , TaxClassification }
                    };

                var json = DataHelper.JsonBuilder(BusinessPartners, new StringBuilder());

                if (company.PATCH($@"BusinessPartners('{DataAccess.GetData(dt, 0, "CardCode", "").ToString()}')", json))
                {
                    //SapCardCode = company.ResultDescription;
                    SapCardCode = DataAccess.GetData(dt, 0, "CardCode", "").ToString();
                    ErrMsg = "Business Partner successfully updated.";
                    hana.Execute($@"UPDATE ""OCRD"" SET ""SAPCardCode"" = '{SapCardCode}' WHERE ""CardCode"" = '{lblCustomerCode.Text}'", hana.GetConnection("SAOHana"));
                    alertMsg(ErrMsg, "success");
                }
                else
                {
                    SapCardCode = "";
                    ErrMsg = $"({company.ResultCode}) {company.ResultDescription}";
                    alertMsg(ErrMsg, "error");
                }
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "error");
            }
        }

        protected void customValidatorDocuments_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //ONLY BLOCK IF IT IS NOT VALID ID 2

            foreach (DataRow row in gvStandardDocumentRequirements.Rows)
            {
                //CHECK IF REEQUIRED FROM OCRA
                int required = int.Parse(row[5].ToString());
                //CHECK IF DOCUMENT IS UPLOADED
                string document = row[1].ToString();

                if (required > 0 && string.IsNullOrEmpty(document))
                {
                    RequiredFieldValidator7.Visible = true;
                    args.IsValid = true;
                }
                else
                {
                    RequiredFieldValidator7.Visible = false;
                    args.IsValid = false;

                }
            }

        }

        protected void gvStandardDocumentRequirements2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            attachmentFileUploadBuyer2(gvStandardDocumentRequirements2, e, "FileUpload4", "lblFileName3", "btnPreview4", "btnRemove4", "RequiredFieldValidatorAttachments");

        }



        void attachmentFileUploadBuyer2(GridView gv, GridViewCommandEventArgs e, string FileUpload, string lblfilename, string preview, string remove, string rv)
        {
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int index = row.RowIndex;

            FileUpload file = (FileUpload)gv.Rows[index].FindControl(FileUpload);
            Label lblFileName = (Label)gv.Rows[index].FindControl(lblfilename);
            LinkButton btnPreview = (LinkButton)gv.Rows[index].FindControl(preview);
            LinkButton btnDelete = (LinkButton)gv.Rows[index].FindControl(remove);
            RequiredFieldValidator rvReq = (RequiredFieldValidator)gv.Rows[index].FindControl(rv);
            //CustomValidator rvReq = (CustomValidator)gv.Rows[index].FindControl(rv);

            string buyerFullName = (tFirstName.Text + "_" + tMiddleName.Text + "_" + tLastName.Text).Trim().ToString().ToUpper();
            string code = ViewState["Guid"].ToString();
            string uniqueCode = code + "_" + tTIN.Text.Replace("-", "");
            //string FolderName = uniqueCode + "_" + buyerFullName; //Folder Name for buyer
            if (e.CommandName.ToLower().Contains("upload"))
            {
                if (file.HasFile)
                {
                    //Get FileName and Extension seperately
                    string fileNameOnly = Path.GetFileNameWithoutExtension(file.FileName);
                    string extension = Path.GetExtension(file.FileName);
                    string FileName = fileNameOnly + "_" + code + extension; //File Name of buyer  


                    //if (File.Exists(HttpContext.Current.Server.MapPath($"~/TEMP_DOCS/{FolderName}/") + FileName))
                    if (File.Exists(HttpContext.Current.Server.MapPath($"~/TEMP_DOCS/") + FileName) || File.Exists(HttpContext.Current.Server.MapPath($"~/BUYER_ATTACHMENTS/") + FileName))
                    {
                        alertMsg("File already exists!", "info");
                    }
                    else
                    {
                        lblFileName.Text = FileName;
                        file.PostedFile.SaveAs(HttpContext.Current.Server.MapPath($"~/TEMP_DOCS/") + FileName); //File is saved in the Physical folder 
                                                                                                                //file.PostedFile.SaveAs(HttpContext.Current.Server.MapPath($"~/TEMP_DOCS/{FolderName}/") + FileName); //File is saved in the Physical folder 
                        btnPreview.Visible = true;
                        btnDelete.Visible = true;


                        ////UPDATE OCRD
                        //if (string.IsNullOrWhiteSpace(ViewState["Guid"].ToString()))
                        //{
                        //    hana.Execute($@"UPDATE OCRD SET ""Guid"" WHERE ""CardCode"" = '{}' ", hana.GetConnection("SAOHana"));
                        //}


                    }
                }
                else
                {
                    //rvReq.Enabled = false;
                    alertMsg("Please choose file!", "info");
                }
            }
            else if (e.CommandName.ToLower().Contains("preview"))
            {
                //if (File.Exists(Server.MapPath($"~/TEMP_DOCS/{FolderName}/") + lblFileName.Text))
                if (File.Exists(Server.MapPath($"~/TEMP_DOCS/") + lblFileName.Text))
                {
                    string Filepath = Server.MapPath($"~/TEMP_DOCS/" + lblFileName.Text);
                    //string Filepath = Server.MapPath($"~/TEMP_DOCS/{FolderName}/" + lblFileName.Text);
                    var host = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.LocalPath, "");
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + host + $"/TEMP_DOCS/" + lblFileName.Text + "');", true);
                    //ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + host + $"/TEMP_DOCS/{FolderName}/" + lblFileName.Text + "');", true);
                }
                else
                {
                    //string Filepath = Server.MapPath($"~/BUYER_ATTACHMENTS/{FolderName}/" + lblFileName.Text);
                    string Filepath = Server.MapPath($"~/BUYER_ATTACHMENTS/" + lblFileName.Text);
                    var host = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.LocalPath, "");
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + host + $"/BUYER_ATTACHMENTS/" + lblFileName.Text + "');", true);
                    //ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + host + $"/BUYER_ATTACHMENTS/{FolderName}/" + lblFileName.Text + "');", true);
                }

            }
            else if (e.CommandName.ToLower().Contains("remove"))
            {

                //if (File.Exists(Server.MapPath($"~/TEMP_DOCS/{FolderName}/") + lblFileName.Text))
                if (File.Exists(Server.MapPath($"~/TEMP_DOCS/") + lblFileName.Text))
                {
                    // If file found, delete it 
                    File.Delete(Server.MapPath($"~/TEMP_DOCS/") + lblFileName.Text);
                    //File.Delete(Server.MapPath($"~/TEMP_DOCS/{FolderName}/") + lblFileName.Text);
                    lblFileName.Text = "";
                }
                else
                {
                    lblFileName.Text = "";
                }
                btnPreview.Visible = false;
                btnDelete.Visible = false;
            }

        }




        void moveTemporaryFilesToPermanent(GridView gv, string lblfileName)
        {
            //string buyerFullName = (txtFirstName.Text + "_" + txtMiddleName.Text + "_" + txtLastName.Text).Trim().ToString().ToUpper();
            //string code = ViewState["Guid"].ToString();
            //string uniqueCode = code + "_" + txtTIN.Text.Replace("-", "");
            //string FolderName = uniqueCode + "_" + buyerFullName;

            foreach (GridViewRow row in gv.Rows)
            {
                int index = row.RowIndex;

                string FileName = ((Label)row.FindControl(lblfileName)).Text;

                if (!string.IsNullOrWhiteSpace(FileName)) //If the used Uploaded a file  
                {
                    string sourceFilePath = Server.MapPath($"~/TEMP_DOCS/") + FileName;
                    //2023-06-19 : ADDED PARAMETER FOR SAVING OF SPA
                    //string destinationFilePath = Server.MapPath($"~/BUYER_ATTACHMENTS/");
                    string destinationFilePath = Server.MapPath($"~/BUYER_ATTACHMENTS/");

                    // Check if the file exists at the source path
                    if (File.Exists(sourceFilePath))
                    {
                        File.Copy(sourceFilePath, destinationFilePath + FileName, true);
                    }
                }
            }
        }


        void moveTemporaryFilesToPermanentSPA(GridView gv)
        {

            foreach (GridViewRow row in gv.Rows)
            {
                int index = row.RowIndex;

                //GET FILE NAME FROM gvSPACoBorrower, WHICH IS "SPAFormDocument"
                string FileName = row.Cells[16].Text;

                if (!string.IsNullOrWhiteSpace(FileName)) //If the used Uploaded a file  
                {
                    string sourceFilePath = Server.MapPath($"~/TEMP_DOCS/") + FileName;
                    //2023-06-19 : ADDED PARAMETER FOR SAVING OF SPA
                    //string destinationFilePath = Server.MapPath($"~/BUYER_ATTACHMENTS/");
                    string destinationFilePath = Server.MapPath($"~/BUYER_SPA/");

                    // Check if the file exists at the source path
                    if (File.Exists(sourceFilePath))
                    {
                        File.Copy(sourceFilePath, destinationFilePath + FileName, true);
                    }
                }
            }
        }



        public void GenerateGuid()
        {
            //GENERATED NEW GUID FOR UNIQUE ID PER CONNECTIONS/WORKSTATIONS
            string id = "";
            var ticks = DateTime.Now.Ticks;
            var guid = Guid.NewGuid().ToString();
            var uniqueSessionId = ticks.ToString() + guid.ToString();
            string[] agentCode = uniqueSessionId.Split('-');
            ViewState["Guid"] = agentCode.Last();
        }

        protected void CustomValidator14_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //2023-06-15 : ADD VALIDATOR FOR SPOUSE TIN FORMAT--%>
            //Check if TIN is in correct format(###-###-###-###)
            bool isOK = Regex.IsMatch(tSpouseTIN2.Text, @"[0-9][0-9][0-9]\-[0-9][0-9][0-9]\-[0-9][0-9][0-9]\-[0-9][0-9][0-9]");
            if (!isOK)
            {
                RequiredFieldValidator12.Visible = false;
                args.IsValid = false;
            }
            else
            {
                RequiredFieldValidator12.Visible = true;
                args.IsValid = true;
            }
            tSpouseTIN2.Focus();
        }













        // ###############################################
        // ADD UPDATING OF TABLES - DEPENDENTS AND REFERENCES
        // ###############################################

        protected void ClearAllModalFields()
        {
            tDependentName.Value = "";
            tDependentAge.Value = "";
            tDependentRelationship.Value = "";

            tBABank.Value = "";
            tBABranch.Value = "";
            tBAAcctType.Value = "";
            tBAAcctNo.Value = "";

            tCRName.Value = "";
            tCRAddress.Value = "";
            reftxtEmail.Value = "";
            tCRTelNo.Value = "";
        }


        protected void btnDepEdit_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            int row = int.Parse(GetID.CommandArgument.ToString());
            tDependentName.Value = gvDependent.Rows[row].Cells[1].Text;
            tDependentAge.Value = gvDependent.Rows[row].Cells[2].Text;
            tDependentRelationship.Value = gvDependent.Rows[row].Cells[3].Text;

            //mtxtRow.Text = row.ToString();
            mtxtRow.Text = gvDependent.Rows[row].Cells[0].Text;

            btnAdd.Visible = false;
            btnUpdate.Visible = true;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowAddDependent();", true);
        }


        protected void btnUpdate_ServerClick(object sender, EventArgs e)
        {
            try
            {
                int row = int.Parse(mtxtRow.Text);
                //gvDependent.Rows[row].Cells[1].Text = tDependentName.Value.ToUpper();
                //gvDependent.Rows[row].Cells[2].Text = tDependentAge.Value;
                //gvDependent.Rows[row].Cells[3].Text = tDependentRelationship.Value.ToUpper();

                //UPDATE ViewState for Updating

                //2023-06-18 : CHANGE TO ViewState
                //DataTable dt = (DataTable)Session["dtDependent"];
                DataTable dt = (DataTable)ViewState["dtDependent"];

                //clear Session
                //2023-06-18 : CHANGE TO ViewState
                //Session["dtDependent"] = null;
                ViewState["dtDependent"] = null;

                foreach (DataRow dr in dt.Rows)
                {
                    //update row of datatable
                    if (int.Parse(dr["Id"].ToString()) == row)
                    {
                        dr["Name"] = tDependentName.Value.Trim().ToUpper();
                        dr["Age"] = Convert.ToInt32(tDependentAge.Value);
                        dr["Relationship"] = tDependentRelationship.Value;
                    }

                }

                //2023-06-18 : CHANGE TO ViewState
                //Session["dtDependent"] = dt;
                ViewState["dtDependent"] = dt;

                //REFRESH GRIDVIEW
                gvDependent.DataSource = dt;
                gvDependent.DataBind();

                ClearAllModalFields();


                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideAddDependent();", true);
            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "info");
            }
        }

        protected void btnBankUpdate_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;

            int row = int.Parse(GetID.CommandArgument.ToString());

            tBABank.Value = gvBankAccount.Rows[row].Cells[1].Text;
            tBABranch.Value = gvBankAccount.Rows[row].Cells[2].Text;
            tBAAcctType.Value = gvBankAccount.Rows[row].Cells[3].Text;
            tBAAcctNo.Value = gvBankAccount.Rows[row].Cells[4].Text;

            //2023-06-17 : FIX FOR UPDATING OF BANK
            //mtxtRow2.Text = row.ToString();
            mtxtRow2.Text = gvBankAccount.Rows[row].Cells[0].Text;



            banktitle.InnerText = "Update Bank Account";

            btnBAAdd.Visible = false;
            btnUpdateBank.Visible = true;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowAddBank();", true);
        }

        protected void btnUpdateBank_ServerClick(object sender, EventArgs e)
        {
            int row = int.Parse(mtxtRow2.Text);

            //2023-06-18 : CHANGE TO ViewState
            //DataTable dt = (DataTable)Session["dtBankAccount"];
            DataTable dt = (DataTable)ViewState["dtBankAccount"];

            //clear Session
            //2023-06-18 : CHANGE TO ViewState
            //Session["dtBankAccount"] = null;
            ViewState["dtBankAccount"] = null;


            foreach (DataRow dr in dt.Rows)
            {
                //update row of datatable
                if (int.Parse(dr["Id"].ToString()) == row)
                {
                    dr["Bank"] = tBABank.Value.Trim().ToUpper();
                    dr["Branch"] = tBABranch.Value.Trim().ToUpper();
                    dr["AcctType"] = tBAAcctType.Value;
                    dr["AcctNo"] = tBAAcctNo.Value;
                }
            }

            //2023-06-18 : CHANGE TO ViewState
            //Session["dtBankAccount"] = dt;
            ViewState["dtBankAccount"] = dt;

            //REFRESH GRIDVIEW
            gvBankAccount.DataSource = dt;
            gvBankAccount.DataBind();

            ClearAllModalFields();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideAddBank();", true);
        }

        protected void btnShowBank_Click(object sender, EventArgs e)
        {
            banktitle.InnerText = "Add Bank Account";
            btnBAAdd.Visible = true;
            btnUpdateBank.Visible = false;
            ClearAllModalFields();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowAddBank();", true);
        }

        protected void btnRefUpdate_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            int row = int.Parse(GetID.CommandArgument.ToString());

            tCRName.Value = gvCharacterRef.Rows[row].Cells[1].Text.Trim().ToUpper();
            tCRAddress.Value = System.Net.WebUtility.HtmlDecode(gvCharacterRef.Rows[row].Cells[2].Text.Trim().ToUpper());
            reftxtEmail.Value = System.Net.WebUtility.HtmlDecode(gvCharacterRef.Rows[row].Cells[3].Text);
            tCRTelNo.Value = gvCharacterRef.Rows[row].Cells[4].Text;


            //2023-06-17: FIX FOR UPDATING CHARACTER REF
            //mtxtRow3.Value = row.ToString();
            mtxtRow3.Value = gvCharacterRef.Rows[row].Cells[0].Text;

            reftitle.InnerText = "Update Character Reference";

            btnCRAdd.Visible = false;
            btnRefUpdate.Visible = true;



            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowCharacterReference();", true);
        }

        protected void btnRefUpdate_ServerClick(object sender, EventArgs e)
        {
            if (tCRName.Value == "" || tCRAddress.Value == "" || reftxtEmail.Value == "")
            { alertMsg("Please fill up all forms before adding!", "info"); }
            else
            {
                int row = int.Parse(mtxtRow3.Value);

                //2023-06-18 : CHANGE TO ViewState
                //DataTable dt = (DataTable)Session["dtCharacterRef"];
                DataTable dt = (DataTable)ViewState["dtCharacterRef"];

                //clear Session
                //2023-06-18 : CHANGE TO ViewState
                //Session["dtCharacterRef"] = null;
                ViewState["dtCharacterRef"] = null;


                foreach (DataRow dr in dt.Rows)
                {
                    //update row of datatable
                    if (int.Parse(dr["Id"].ToString()) == row)
                    {
                        dr["Name"] = tCRName.Value;
                        dr["Address"] = tCRAddress.Value;
                        dr["Email"] = reftxtEmail.Value;
                        dr["TelNo"] = tCRTelNo.Value;
                    }
                }

                //2023-06-18 : CHANGE TO ViewState
                //Session["dtCharacterRef"] = dt;
                ViewState["dtCharacterRef"] = dt;

                //REFRESH GRIDVIEW
                gvCharacterRef.DataSource = dt;
                gvCharacterRef.DataBind();




                ClearAllModalFields();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideCharacterReference();", true);
            }
        }


        protected void btnRefShow_Click(object sender, EventArgs e)
        {
            ClearAllModalFields();
            reftitle.InnerText = "New Character Reference";
            btnCRAdd.Visible = true;
            btnRefUpdate.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowCharacterReference();", true);
        }

        protected void btnSPAUpdate_ServerClick(object sender, EventArgs e)
        {
            int row = int.Parse(txtSPAID.Value);

            //2023-06-18 : CHANGE TO ViewState
            //DataTable dt = (DataTable)Session["gvSPACoBorrower"];
            DataTable dt = (DataTable)ViewState["gvSPACoBorrower"];

            //clear Session
            //2023-06-18 : CHANGE TO ViewState
            //Session["gvSPACoBorrower"] = null;
            ViewState["gvSPACoBorrower"] = null;

            foreach (DataRow dr in dt.Rows)
            {
                //update row of datatable
                if (int.Parse(dr["Id"].ToString()) == row)
                {
                    dr["Relationship"] = tRelationship.Value;
                    dr["LastName"] = tSPALastName.Value.Trim().ToUpper();
                    dr["FirstName"] = tSPAFirstName.Value.Trim().ToUpper();
                    dr["MiddleName"] = tSPAMiddleName.Value.Trim().ToUpper();
                    dr["CivilStatus"] = tSPACivilStatus.SelectedValue;
                    dr["YearsOfStay"] = string.IsNullOrEmpty(tSPAYearsOfStay.Value) ? "0" : tSPAYearsOfStay.Value;
                    dr["Address"] = string.IsNullOrEmpty(tSPAPresentAddress.Value) ? string.Empty : tSPAPresentAddress.Value.Trim().ToUpper();
                    dr["BirthDate"] = string.IsNullOrEmpty(tSPABirthDate.Value) ? string.Empty : tSPABirthDate.Value;
                    dr["BirthPlace"] = string.IsNullOrEmpty(tSPABirthPlace.Value) ? string.Empty : tSPABirthPlace.Value.Trim().ToUpper();
                    dr["Gender"] = tSPAGender.Value;
                    dr["Citizenship"] = tSPACitizenship.Value;
                    dr["Email"] = string.IsNullOrEmpty(tSPAEmailAdd.Value) ? string.Empty : tSPAEmailAdd.Value;
                    dr["TelNo"] = string.IsNullOrEmpty(tSPAHomeTelNo.Value) ? string.Empty : tSPAHomeTelNo.Value;
                    dr["MobileNo"] = string.IsNullOrEmpty(tSPACellphoneNo.Value) ? string.Empty : tSPACellphoneNo.Value;
                    dr["FB"] = string.IsNullOrEmpty(tSPAFBAccount.Value) ? string.Empty : tSPAFBAccount.Value;
                    dr["SPAFormDocument"] = lblSPAFileName.Text;
                }
            }

            //2023-06-18 : CHANGE TO ViewState
            //Session["gvSPACoBorrower"] = dt;
            ViewState["gvSPACoBorrower"] = dt;

            //REFRESH GRIDVIEW
            gvSPACoBorrower.DataSource = dt;
            gvSPACoBorrower.DataBind();

            ClearAllModalFields();
            hidTAB.Value = "#tab_3";

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "MsgSPACoBorrower_Hide();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);
        }






    }
}
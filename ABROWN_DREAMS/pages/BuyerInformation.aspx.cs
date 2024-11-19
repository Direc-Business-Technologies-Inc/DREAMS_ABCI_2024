using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABROWN_DREAMS.pages
{
    public partial class Broker : System.Web.UI.Page
    {
        SAPHanaAccess hana = new SAPHanaAccess();
        DirecWebService ws = new DirecWebService();


        protected void Page_Load(object sender, EventArgs e)
        {
            //2023-06-14: ALWAYS ADD 0 ON USERID WHEN EXPIRED

            Session["UserID"] = 0;

            Session["ReportType"] = "";
            if (this.IsPostBack)
            {
                //STAY IN CURRENT TAB
                TabName.Value = Request.Form[TabName.UniqueID];
                //ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);


            }
            if (!this.IsPostBack)
            {
                //GENERATED NEW GUID FOR UNIQUE ID PER CONNECTIONS/WORKSTATIONS
                string id = "";
                var ticks = DateTime.Now.Ticks;
                var guid = Guid.NewGuid().ToString();
                var uniqueSessionId = ticks.ToString() + guid.ToString();
                string[] agentCode = uniqueSessionId.Split('-');
                ViewState["Guid"] = agentCode.Last();



                Session["BrkStatus"] = "";
                Session["BrkReason"] = "";
                Session["Brkcreds"] = "";
                Session["UserAccess"] = null;

                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);

                step_1.Visible = true;
                step_2.Visible = false;
                step_3.Visible = false;
                step1.Attributes.Add("class", "btn btn-info btn-circle");
                step2.Attributes.Add("class", "btn btn-default btn-circle");
                //step3.Attributes.Add("class", "btn btn-default btn-circle");

                spouseBtn.Visible = false;
                btnCopySPA.Visible = false;
                RequiredFieldValidator8.Enabled = false;
                RequiredFieldValidator9.Enabled = false;
                RequiredFieldValidator10.Enabled = false;
                compDetails.Visible = false;
                contactDetails.Visible = false;
                coborrowerGrid.Visible = false;
                othersGrid.Visible = false;

                Session["SPACoBorrowerCount"] = 1;

                if (Session["UserID"] == null)
                {
                    Session["UserID"] = 0;
                }
                ws.InitializeSPA((int)Session["UserID"]);

                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[4]
                {
                        new DataColumn("Id"),
                        new DataColumn("Name"),
                        new DataColumn("Age"),
                        new DataColumn("RelationShip")
                });
                ViewState["Dependent"] = dt;
                ViewState["dtDependent"] = dt;
                this.BindGrid();


                DataTable dt2 = new DataTable();
                dt2.Columns.AddRange(new DataColumn[6]
                {
                        new DataColumn("Id"),
                        new DataColumn("Bank"),
                        new DataColumn("Branch"),
                        new DataColumn("AcctType"),
                         new DataColumn("AcctNo"),
                         new DataColumn("AcctNoOrig")
                });
                ViewState["BankAccount"] = dt2;
                this.BindGrid2();

                DataTable dt3 = new DataTable();
                dt3.Columns.AddRange(new DataColumn[5]
                {

                        new DataColumn("Id"),
                        new DataColumn("Name"),
                        new DataColumn("Address"),
                        new DataColumn("Email"),
                        new DataColumn("TelNo")
                });
                ViewState["CharacterRef"] = dt3;
                this.BindGrid3();



                //2023-06-17 : CHANGE SAVING OF SPA
                DataTable dtSPA = new DataTable();
                dtSPA.Columns.AddRange(new DataColumn[17]
                {

                        new DataColumn("Id"),
                        new DataColumn("Relationship"),
                        new DataColumn("LastName"),
                        new DataColumn("FirstName"),
                        new DataColumn("MiddleName"),
                        new DataColumn("CivilStatus"),
                        new DataColumn("YearsOfStay"),
                        new DataColumn("Address"),
                        new DataColumn("BirthDate"),
                        new DataColumn("BirthPlace"),
                        new DataColumn("Gender"),
                        new DataColumn("Citizenship"),
                        new DataColumn("Email"),
                        new DataColumn("TelNo"),
                        new DataColumn("MobileNo"),
                        new DataColumn("FB"),
                        new DataColumn("SPAFormDocument")
                });
                ViewState["SPA"] = dtSPA;
                this.BindSPA();





                DataTable taxclass = new DataTable();
                taxclass.Columns.AddRange(new DataColumn[2]
                        {
                        new DataColumn("Code"),
                        new DataColumn("Name")
                        });
                ViewState["TaxClass"] = taxclass;

                DataTable dtCoOwner = new DataTable();
                string session = "dtCoOwner";
                dtCoOwner.Columns.Add("ID", typeof(int));
                dtCoOwner.Columns.Add("FirstName", typeof(string));
                dtCoOwner.Columns.Add("MiddleName", typeof(string));
                dtCoOwner.Columns.Add("LastName", typeof(string));
                dtCoOwner.Columns.Add("Relationship", typeof(string));
                dtCoOwner.Columns.Add("Email", typeof(string));
                dtCoOwner.Columns.Add("MobileNo", typeof(string));
                dtCoOwner.Columns.Add("Address", typeof(string));
                dtCoOwner.Columns.Add("Residence", typeof(string));
                dtCoOwner.Columns.Add("ValidID", typeof(string));
                dtCoOwner.Columns.Add("ValidIDNo", typeof(string));
                Session[session] = dtCoOwner;

                LoadData(gvCoOwner, session);

                DataTable dtOthers = new DataTable();
                session = "dtOthers";
                dtOthers.Columns.Add("ID", typeof(int));
                dtOthers.Columns.Add("FirstName", typeof(string));
                dtOthers.Columns.Add("MiddleName", typeof(string));
                dtOthers.Columns.Add("LastName", typeof(string));
                dtOthers.Columns.Add("Relationship", typeof(string));
                dtOthers.Columns.Add("Email", typeof(string));
                dtOthers.Columns.Add("MobileNo", typeof(string));
                dtOthers.Columns.Add("Address", typeof(string));
                dtOthers.Columns.Add("Residence", typeof(string));
                dtOthers.Columns.Add("ValidID", typeof(string));
                dtOthers.Columns.Add("ValidIDNo", typeof(string));
                Session[session] = dtOthers;

                LoadData(gvOthers, session);

                DataTable SPACoBorrower = new DataTable();
                session = "gvSPACoBorrower";
                SPACoBorrower.Columns.Add("ID", typeof(int));
                SPACoBorrower.Columns.Add("Relationship", typeof(string));
                SPACoBorrower.Columns.Add("Name", typeof(string));
                SPACoBorrower.Columns.Add("Gender", typeof(string));
                SPACoBorrower.Columns.Add("Email", typeof(string));
                SPACoBorrower.Columns.Add("SPAFormDocument", typeof(string));
                Session[session] = SPACoBorrower;

                LoadData(gvSPACoBorrower, session);










                SPAAddUpdate.InnerText = "Add";

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
                ddTypeOfID.DataSource = dt;
                ddTypeOfID.DataBind();
                ddTypeOfID.SelectedValue = "---Select Type of ID---";
                ddTypeOfID2.DataSource = dt;
                ddTypeOfID2.DataBind();
                ddTypeOfID2.SelectedValue = "---Select Type of ID---";
                ddTypeOfID3.DataSource = dt;
                ddTypeOfID3.DataBind();
                ddTypeOfID3.SelectedValue = "---Select Type of ID---";
                ddTypeOfID4.DataSource = dt;
                ddTypeOfID4.DataBind();
                ddTypeOfID4.SelectedValue = "---Select Type of ID---";
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
                ViewState["ValidIdPrev"] = dt4;

                txtCertifyDate.Value = DateTime.Now.Date.ToString("yyyy-MM-dd");

                taxClassChanged();


                otherFields();
                LoadBuyerDocumentsStandard();
                LoadBuyerDocumentsStandard_NotRequired();

                DeleteTemporaryFIles();

                hidebtnSameasPresent();


                //2023-06-15 : ADD VALIDATOR FOR SPOUSE TIN FORMAT--%>
                CustomValidator14.Enabled = true;
            }
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
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


        protected void BindGrid()
        {
            gvDependent.DataSource = (DataTable)ViewState["dtDependent"];
            gvDependent.DataBind();
        }

        protected void BindGrid2()
        {
            gvBankAccount.DataSource = (DataTable)ViewState["BankAccount"];
            gvBankAccount.DataBind();
        }
        protected void BindGrid3()
        {
            gvCharacter.DataSource = (DataTable)ViewState["CharacterRef"];
            gvCharacter.DataBind();
        }

        protected void BindSPA()
        {
            gvSPACoBorrower.DataSource = (DataTable)ViewState["SPA"];
            gvSPACoBorrower.DataBind();
        }

        protected void btnNext_ServerClick(object sender, EventArgs e)
        {
            step_1.Visible = false;
            step_2.Visible = true;
            step_3.Visible = false;
            step1.Attributes.Add("class", "btn btn-success btn-circle");
            step2.Attributes.Add("class", "btn btn-info btn-circle");
            //step3.Attributes.Add("class", "btn btn-default btn-circle");

            //if (!string.IsNullOrWhiteSpace(txtUniqueID.Text))
            //{
            //    DataTable dtBPHeader = hana.GetData($@"SELECT T0.""PresentAddress"", T0.""PermanentAddress"" , T0.""YearsStay"", T0.""PermanentYrStay"", T0.""PresentPostalCode"", T0.""PermanentPostalCode"" 
            //                                            , T0.""PresentCountry"", T0.""PermanentCountry"" , T0.""Profession"" , T0.""SourceOfFunds"" , T0.""Occupation"", T0.""MonthlyIncome"", ""Comaker""
            //                                            , T1.""LastName"" , T1.""FirstName"", T1.""MiddleName"" , T1.""CompanyName""
            //                                            , T1.""BirthDay"" , T1.""BirthPlace"", T1.""Gender"", T1.""Citizenship""
            //                                            , T1.""EmailAddress"" , T0.""HomeTelNo"" , T1.""CellNo"" , T1.""FBAccount""
            //                                            , IFNULL(T0.""IDType"",'---Select Type of ID---') ""IDType"", T0.""IDNo"" 
            //                                            , T1.""TIN"", T1.""SSSNo"", T1.""GSISNo"", T1.""PagibiNo""
            //                                            , S1.""Code"" as ""EmpStatus"" , S2.""Code"" as ""NatureEmp""
            //                                            , IFNULL(S3.""Code"",'---Select Civil Status---') as ""CivilStatus""
            //                                            , T0.""HomeOwnership"", T0.""HomeOwnerValue""
            //                                            /**Employer Info**/
            //                                            , T1.""EmpBusName"", T1.""EmpBusAdd"", T1.""Position"", T1.""YearsService""
            //                                            , T1.""OfficeTelNo"" , T1.""FaxNo"" , T1.""EmpBusCountry""
            //                                            , T0.""BusinessType"", T0.""TaxClassification""
            //                                           FROM OCRD T0 
            //                                            INNER JOIN CRD1 T1 ON T0.""CardCode"" = T1.""CardCode""
            //                                            LEFT JOIN OLST S1 ON T1.""EmpStatus"" = S1.""Code""
            //                                            LEFT JOIN OLST S2 ON T1.""NatureEmp"" = S2.""Code""
            //                                            LEFT JOIN OLST S3 ON T0.""CivilStatus"" = S3.""Code""                                                        
            //                                           WHERE UPPER(T0.""CardCode"") = UPPER('{txtUniqueID.Text.ToLower()}') AND T1.""CardType"" = 'Buyer' ; 
            //                                        ", hana.GetConnection("SAOHana"));
            //    if (dtBPHeader != null)
            //    {
            //        if (dtBPHeader.Rows.Count > 0)
            //        {
            //            foreach (DataRow bp in dtBPHeader.Rows)
            //            {
            //                //CHECK IF BUSINESS TYPE IS INDIVIDUAL OR CORP
            //                if (bp["BusinessType"].ToString() == "Individual")
            //                {
            //                    gensolo.Visible = true;
            //                    gencorp.Visible = false;
            //                }
            //                //else
            //                //{
            //                //    gensolo.Visible = false;
            //                //    gencorp.Visible = true;
            //                //}

            //                ddlBusinessType.SelectedValue = bp["BusinessType"].ToString();
            //                loadDivisionsForNames(ddlBusinessType.Text);
            //                if (bp["TaxClassification"].ToString() != "")
            //                {
            //                    ddTaxClass.SelectedValue = bp["TaxClassification"].ToString();
            //                }

            //                ////--BUYER GENERAL INFO --////
            //                txtPermanentAdd.Text = bp["PermanentAddress"].ToString();
            //                txtAddress.Text = bp["PresentAddress"].ToString();
            //                txtLastName.Text = bp["LastName"].ToString();
            //                txtFirstName.Text = bp["FirstName"].ToString();
            //                txtMiddleName.Text = bp["MiddleName"].ToString();
            //                txtCorpName.Text = bp["CompanyName"].ToString();
            //                txtComaker.Text = bp["Comaker"].ToString();
            //                txtPresPostal.Text = bp["PresentPostalCode"].ToString();
            //                txtPermPostal.Text = bp["PermanentPostalCode"].ToString();
            //                //txtPresYrStay.Text = bp["YearsStay"].ToString();
            //                txtPermYrStay.Text = bp["PermanentYrStay"].ToString();
            //                if (!String.IsNullOrEmpty(bp["PresentCountry"].ToString())) ddPreCountry.SelectedValue = bp["PresentCountry"].ToString();
            //                if (!String.IsNullOrEmpty(bp["PermanentCountry"].ToString())) ddPermCountry.SelectedValue = bp["PermanentCountry"].ToString();
            //                txtProfession.Text = bp["Profession"].ToString();
            //                if (!String.IsNullOrEmpty(bp["SourceOfFunds"].ToString())) ddSourceFunds.SelectedValue = bp["SourceOfFunds"].ToString();
            //                if (!String.IsNullOrEmpty(bp["Occupation"].ToString())) ddOccupation.SelectedValue = bp["Occupation"].ToString();
            //                if (!String.IsNullOrEmpty(bp["MonthlyIncome"].ToString())) ddMonthlyIncome.SelectedValue = bp["MonthlyIncome"].ToString() == "" || bp["MonthlyIncome"].ToString() == "0" ? "---Select Source of Funds---" : bp["MonthlyIncome"].ToString();

            //                ////--BUYER Address and Business Info--////
            //                txtTIN.Text = bp["TIN"].ToString();
            //                txtSSS.Text = bp["SSSNo"].ToString();
            //                txtGSIS.Text = bp["GSISNo"].ToString();
            //                txtPagIbig.Text = bp["PagibiNo"].ToString();

            //                if (!String.IsNullOrEmpty(bp["EmpStatus"].ToString())) ddEmploymentStatus.SelectedValue = bp["EmpStatus"].ToString();
            //                if (!String.IsNullOrEmpty(bp["NatureEmp"].ToString())) ddNatureOfEmployement.SelectedValue = bp["NatureEmp"].ToString();

            //                ////--BUYER ADDRESS--////
            //                dtBirthday.Text = Convert.ToDateTime(bp["BirthDay"].ToString()).ToString("yyyy-MM-dd");
            //                txtPlaceOfBirth.Text = bp["BirthPlace"].ToString();
            //                if (!String.IsNullOrEmpty(bp["Gender"].ToString())) ddGender.SelectedValue = bp["Gender"].ToString();
            //                txtCitizenship.Value = bp["Citizenship"].ToString();
            //                txtEmail.Text = bp["EmailAddress"].ToString();
            //                txtTelNo.Text = bp["HomeTelNo"].ToString();
            //                txtMobileNo.Text = bp["CellNo"].ToString();
            //                txtFacebook.Text = bp["FBAccount"].ToString();
            //                if (!String.IsNullOrEmpty(bp["IDType"].ToString())) ddTypeOfID.SelectedItem.Value = bp["IDType"].ToString();
            //                txtIDNumber.Text = bp["IDNo"].ToString();
            //                if (!String.IsNullOrEmpty(bp["CivilStatus"].ToString())) ddCivilStatus.SelectedValue = bp["CivilStatus"].ToString();

            //                ////--BUYER DEPENDENTS--////
            //                //if (bp["HomeOwnership"].ToString() == "Owned")
            //                //{
            //                //    rb1.Checked = true;
            //                //}
            //                //else if (bp["HomeOwnership"].ToString() == "Mortgaged")
            //                //{
            //                //    rb2.Checked = true;
            //                //}
            //                //else if (bp["HomeOwnership"].ToString() == "Living w/ Relatives")
            //                //{
            //                //    rb3.Checked = true;
            //                //}
            //                //else if (bp["HomeOwnership"].ToString() == "Rented")
            //                //{
            //                //    rb4.Checked = true;
            //                //    txtRented.Enabled = true;
            //                //    txtRented.Text = SystemClass.ToCurrency(bp["HomeOwnerValue"].ToString());
            //                //}


            //                DataTable dtBPDependents = hana.GetData($@"SELECT T0.""ID"", T0.""Name"", T0.""Age"" , T0.""Relationship""
            //                                                       FROM CRD2 T0                                                        
            //                                                       WHERE LCASE(T0.""CardCode"") = '{txtUniqueID.Text.ToLower()}' ; 
            //                                                    ", hana.GetConnection("SAOHana"));
            //                ViewState["Dependent"] = dtBPDependents;
            //                this.BindGrid();

            //                ////--BUYER REFERENCE--////    
            //                ////--#1 BUYER'S BANK INFO
            //                DataTable dtBPBankList = hana.GetData($@"SELECT T0.""Bank"", T0.""Branch"", T0.""AcctType"", T0.""AcctNo""
            //                                                        , T0.""AvgDailyBal"", T0.""PresentBal"" ,T0.""AcctNo"" as ""AcctNoOrig""
            //                                                        FROM CRD3 T0 WHERE LCASE(T0.""CardCode"")= '{txtUniqueID.Text.ToLower()}'; ", hana.GetConnection("SAOHana"));
            //                ViewState["BankAccount"] = dtBPBankList;
            //                this.BindGrid2();

            //                ////--BUYER REFERENCE--////    
            //                ////--#2 BUYER'S CHARACTER REFERENCE
            //                DataTable dtBPCharRefList = hana.GetData($@"SELECT T0.""FullName"", T0.""Address"", T0.""Email"", T0.""ContactNo""
            //                                                        , T0.""CreateDate"" 
            //                                                        FROM CRD6 T0 WHERE LCASE(T0.""CardCode"")= '{txtUniqueID.Text.ToLower()}'; ", hana.GetConnection("SAOHana"));
            //                ViewState["CharacterRef"] = dtBPCharRefList;
            //                this.BindGrid3();

            //                txtEmpName.Text = bp["EmpBusName"].ToString();
            //                txtEmpAdd.Text = bp["EmpBusAdd"].ToString();
            //                txtMyPosition.Text = bp["Position"].ToString();
            //                txtEmpYrService.Text = bp["YearsService"].ToString();
            //                txtEmpTelNo.Text = bp["OfficeTelNo"].ToString();
            //                txtEmpFaxNo.Text = bp["FaxNo"].ToString();
            //                if (!String.IsNullOrEmpty(bp["EmpBusCountry"].ToString())) ddEmployCountry.SelectedValue = bp["EmpBusCountry"].ToString();

            //                ////--#2 BUYER'S SPOUSE
            //                DataTable dtBPSPO = hana.GetData($@"SELECT  T0.* FROM CRD1 T0 Where LCASE(T0.""CardType"")='spouse' AND LCASE(T0.""CardCode"") = '{txtUniqueID.Text.ToLower()}' ", hana.GetConnection("SAOHana"));
            //                foreach (DataRow bpspo in dtBPSPO.Rows)
            //                {
            //                    txtSPOLastName.Text = bpspo["LastName"].ToString();
            //                    txtSPOFirstName.Text = bpspo["FirstName"].ToString();
            //                    txtSPOMiddleName.Text = bpspo["MiddleName"].ToString();
            //                    txtSPOAddress.Text = bpspo["PresentAddress"].ToString();
            //                    dtSPOBirthDate.Text = String.IsNullOrEmpty(bpspo["BirthDay"].ToString()) ? bpspo["BirthDay"].ToString() : Convert.ToDateTime(bpspo["BirthDay"].ToString()).ToString("yyyy-MM-dd");
            //                    txtSPOBirthPlace.Text = bpspo["BirthPlace"].ToString();
            //                    txtSPOCitizenShip.Value = bpspo["Citizenship"].ToString();
            //                    txtSPOEmail.Text = bpspo["EmailAddress"].ToString();
            //                    txtSPOMobile.Text = bpspo["CellNo"].ToString();
            //                    txtSPOTelNo.Text = bpspo["OfficeTelNo"].ToString();
            //                    txtSPOFB.Text = bpspo["FBAccount"].ToString();
            //                    if (!String.IsNullOrEmpty(bpspo["Gender"].ToString())) ddSPOGender.SelectedValue = bpspo["Gender"].ToString();
            //                    if (!String.IsNullOrEmpty(bpspo["EmpStatus"].ToString())) ddSPOEmpStat.SelectedValue = bpspo["EmpStatus"].ToString() == "" || bpspo["EmpStatus"].ToString() == "N/A" ? "---Select Employment Status---" : bpspo["EmpStatus"].ToString();
            //                    if (!String.IsNullOrEmpty(bpspo["NatureEmp"].ToString())) ddSPONatureEmp.SelectedValue = bpspo["NatureEmp"].ToString() == "" || bpspo["NatureEmp"].ToString() == "N/A" ? "---Select Nature of Employment---" : bpspo["NatureEmp"].ToString();
            //                    txtSPOBusName.Text = bpspo["EmpBusName"].ToString();
            //                    txtSPOBusAdd.Text = bpspo["EmpBusAdd"].ToString();
            //                    if (!String.IsNullOrEmpty(bpspo["EmpBusCountry"].ToString())) ddSPOBusCountry.SelectedValue = bpspo["EmpBusCountry"].ToString();
            //                    txtSPOPosition.Text = bpspo["Position"].ToString();
            //                    txtSPOYearsService.Text = bpspo["YearsService"].ToString();
            //                    txtSPOOfcTelNo.Text = bpspo["OfficeTelNo"].ToString();
            //                    txtSPOFaxNo.Text = bpspo["FaxNo"].ToString();
            //                    txtSPOTinNo.Text = bpspo["TIN"].ToString();
            //                    txtSPOSSSNo.Text = bpspo["SSSNo"].ToString();
            //                    txtSPOGSIS.Text = bpspo["GSISNo"].ToString();
            //                    txtSPOPagibi.Text = bpspo["PagibiNo"].ToString();
            //                }

            //                ////--BUYER'S SPA &/or Co-Borrower--////
            //                DataTable dtBPSPA = hana.GetData($@"SELECT  T0.* FROM CRD5 T0 Where LCASE(T0.""SPA"")='true' AND LCASE(T0.""CardCode"") = '{txtUniqueID.Text.ToLower()}' ", hana.GetConnection("SAOHana"));
            //                foreach (DataRow bpspa in dtBPSPA.Rows)
            //                {
            //                    txtSPALastName.Text = bpspa["LastName"].ToString();
            //                    txtSPAFirstName.Text = bpspa["FirstName"].ToString();
            //                    txtSPAMiddleName.Text = bpspa["MiddleName"].ToString();
            //                    txtSPAAddress.Text = bpspa["SPAPresentAddress"].ToString();
            //                    dtSPABirthDate.Text = String.IsNullOrEmpty(bpspa["BirthDay"].ToString()) ? bpspa["BirthDay"].ToString() : Convert.ToDateTime(bpspa["BirthDay"].ToString()).ToString("yyyy-MM-dd");
            //                    txtSPABirthPlace.Text = bpspa["BirthPlace"].ToString();
            //                    txtSPACitizenship.Value = bpspa["Citizenship"].ToString();
            //                    txtSPAEmail.Text = bpspa["EmailAddress"].ToString();
            //                    txtSPAMobile.Text = bpspa["CellNo"].ToString();
            //                    txtSPATelNo.Text = bpspa["OfficeTelNo"].ToString();
            //                    txtSPAFB.Text = bpspa["FBAccount"].ToString();
            //                    if (!String.IsNullOrEmpty(bpspa["Gender"].ToString())) ddSPAGender.SelectedValue = bpspa["Gender"].ToString();
            //                    if (!String.IsNullOrEmpty(bpspa["EmpStatus"].ToString())) ddSPAEmpStat.SelectedValue = bpspa["EmpStatus"].ToString();
            //                    if (!String.IsNullOrEmpty(bpspa["NatureEmp"].ToString())) ddSPANatureEmp.SelectedValue = bpspa["NatureEmp"].ToString();
            //                    txtSPABusName.Text = bpspa["EmpBusName"].ToString();
            //                    txtSPABusAdd.Text = bpspa["EmpBusAdd"].ToString();
            //                    if (!String.IsNullOrEmpty(bpspa["SPABusCountry"].ToString())) ddSPABusCountry.SelectedValue = bpspa["SPABusCountry"].ToString();
            //                    txtSPAPosition.Text = bpspa["Position"].ToString();
            //                    txtSPAYearsService.Text = bpspa["YearsService"].ToString();
            //                    txtSPAOfcTelNo.Text = bpspa["OfficeTelNo"].ToString();
            //                    txtSPAFaxNo.Text = bpspa["FaxNo"].ToString();
            //                    txtSPATinNo.Text = bpspa["TIN"].ToString();
            //                    txtSPASSSNo.Text = bpspa["SSSNo"].ToString();
            //                    txtSPAGSIS.Text = bpspa["GSISNo"].ToString();
            //                    txtSPAPagibi.Text = bpspa["PagibiNo"].ToString();
            //                }
            //            }

            //            step_1.Visible = false;
            //            step_2.Visible = true;
            //            step_3.Visible = false;
            //            step1.Attributes.Add("class", "btn btn-success btn-circle");
            //            step2.Attributes.Add("class", "btn btn-info btn-circle");
            //            step3.Attributes.Add("class", "btn btn-default btn-circle");
            //        }
            //        else
            //        {
            //            alertMsg("No record found!", "info");
            //        }
            //    }
            //    else
            //    {
            //        alertMsg("No record found!", "error");
            //    }
            //}
            //else
            //{
            //    alertMsg("No code specified to proceed.", "error");
            //}
        }
        protected void btnNext2_ServerClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (ddlBusinessType.Text != "Corporation")
                {
                    txtCertifyCompleteName.Value = ($@"{txtFirstName.Text} {txtMiddleName.Text} {txtLastName.Text}").Trim();
                }
                else
                {
                    txtCertifyCompleteName.Value = ($@"{txtFirstName2.Text} {txtMiddleName2.Text} {txtLastName2.Text}").Trim();
                    txtConformeCorp.Value = txtCorpName.Text;

                }


                step_1.Visible = false;
                step_2.Visible = false;
                step_3.Visible = true;
                done_step.Visible = false;
                step1.Attributes.Add("class", "btn btn-success btn-circle");
                step2.Attributes.Add("class", "btn btn-success btn-circle");


                //2023-06-15 : CHECK IF INDIVIDUAL AND IS MARRIED
                if (ddlBusinessType.Text == "Individual" && ddCivilStatus.Text == "CS2")
                {
                    RequiredFieldValidator11.Enabled = true;
                }
                else
                {
                    RequiredFieldValidator11.Enabled = false;
                }
            }
        }
        protected void btnNext3_ServerClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {

                    //BLOCK ADDING WHEN TIN ALREADY EXIST
                    string qry = "";
                    DataTable dt = null;
                    if (ddlBusinessType.Text == "Individual")
                    {
                        qry = $@"SELECT ""CardCode"" FROM ""CRD1"" where ""TIN"" = IFNULL('{txtTIN.Text}','--') AND  ""CardType"" = 'Buyer'";
                        dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
                    }
                    else if (ddlBusinessType.Text == "Corporation")
                    {
                        qry = $@"SELECT ""CardCode"" FROM ""CRD1"" where ""TIN"" = IFNULL('{tTINCorp.Text}','--') AND  ""CardType"" = 'Buyer'";
                        dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
                    }
                    else
                    {
                        qry = $@"SELECT ""CardCode"" FROM ""CRD1"" where ""TIN"" = IFNULL('{txtTIN.Text}','--') AND  ""CardType"" = 'Buyer'";
                        dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
                        dt.Rows.Clear();
                    }


                    //dt = hana.GetData(qry, hana.GetConnection("SAOHana"));

                    if (dt.Rows.Count <= 0)
                    {


                        bool IsUpdate;
                        string CardCode;
                        IsUpdate = false;
                        CardCode = (int.Parse(ws.GetAutoKey(1, "C")) + (int)Session["UserID"]).ToString();

                        int UserID = (int)Session["UserID"];
                        string _ddNatureOfEmp = ddNatureOfEmployement.SelectedValue.Contains("Select") ? "" : ddNatureOfEmployement.SelectedValue;
                        string _ddEmpCountry = ddEmployCountry.SelectedValue.Contains("Select") ? "" : ddEmployCountry.SelectedValue;
                        string _ddEmpStatus = ddEmploymentStatus.SelectedValue.Contains("Select") ? "" : ddEmploymentStatus.SelectedValue;
                        string _ddCivilStat = ddCivilStatus.SelectedValue.Contains("Select") ? "" : ddCivilStatus.SelectedValue;
                        string _ddIDType = ddTypeOfID.SelectedValue.Contains("Select") ? "" : ddTypeOfID.SelectedValue;
                        string _ddIDType2 = ddTypeOfID2.SelectedValue.Contains("Select") ? "" : ddTypeOfID2.SelectedValue;
                        string _ddIDType3 = ddTypeOfID3.SelectedValue.Contains("Select") ? "" : ddTypeOfID3.SelectedValue;
                        string _ddIDType4 = ddTypeOfID4.SelectedValue.Contains("Select") ? "" : ddTypeOfID4.SelectedValue;
                        string _ddGender = ddGender.SelectedValue.Contains("Select") ? "" : ddGender.SelectedValue;
                        string _ddSourceFunds = ddSourceFunds.SelectedValue.Contains("Select") ? "" : ddSourceFunds.SelectedValue;
                        string _ddOccupation = ddOccupation.SelectedValue.Contains("Select") ? "" : ddOccupation.SelectedValue;
                        string _ddMonthlyIncome = ddMonthlyIncome.SelectedValue.Contains("Select") ? "" : ddMonthlyIncome.SelectedValue;

                        string _yrAddrStayPre = String.IsNullOrEmpty(txtPresYrStay.Text) ? "1" : txtPresYrStay.Text; ////
                        string _yrservice = String.IsNullOrEmpty(txtEmpYrService.Text) ? "1" : txtEmpYrService.Text;
                        string _yrAddrStayPerm = string.IsNullOrEmpty(txtPermYrStay.Text) ? "1" : txtPermYrStay.Text; ////

                        string _ddPresCountry = ddPreCountry.SelectedValue.Contains("Select") ? "" : ddPreCountry.SelectedValue;
                        string _ddPermCountry = ddPermCountry.SelectedValue.Contains("Select") ? "" : ddPermCountry.SelectedValue;

                        string _ddSPOBusCountry = ddSPOBusCountry.SelectedValue.Contains("Select") ? "" : ddSPOBusCountry.SelectedValue;
                        string _ddSPOEmpStat = ddSPOEmpStat.SelectedValue.Contains("Select") ? "" : ddSPOEmpStat.SelectedValue;
                        string _ddSPOEmpNatu = ddSPONatureEmp.SelectedValue.Contains("Select") ? "" : ddSPONatureEmp.SelectedValue;
                        string _ddSPOGender = ddSPOGender.SelectedValue.Contains("Select") ? "" : ddSPOGender.SelectedValue;
                        string _spoyrserv = String.IsNullOrEmpty(txtSPOYearsService.Text) ? "1" : txtSPOYearsService.Text;

                        string _ddSPABusCountry = ddSPABusCountry.SelectedValue.Contains("Select") ? "" : ddSPABusCountry.SelectedValue;
                        string _ddSPAEmpStat = ddSPAEmpStat.SelectedValue.Contains("Select") ? "" : ddSPAEmpStat.SelectedValue;
                        string _ddSPAEmpNatu = ddSPANatureEmp.SelectedValue.Contains("Select") ? "" : ddSPANatureEmp.SelectedValue;
                        string _ddSPAGender = ddSPAGender.SelectedValue.Contains("Select") ? "" : ddSPAGender.SelectedValue;
                        string _spayrserv = string.IsNullOrEmpty(txtSPAYearsService.Text) ? "1" : txtSPAYearsService.Text;

                        string comaker = string.IsNullOrEmpty(txtComaker.Text) ? "N/A" : txtComaker.Text;
                        string _ownership;

                        string _others = string.IsNullOrEmpty(txtOthers1.Text) ? "N/A" : txtOthers1.Text;
                        string _others2 = string.IsNullOrEmpty(txtOthers2.Text) ? "N/A" : txtOthers2.Text;
                        string _others3 = string.IsNullOrEmpty(txtComaker.Text) ? "N/A" : txtComaker.Text;
                        string _others4 = string.IsNullOrEmpty(txtComaker.Text) ? "N/A" : txtComaker.Text;
                        string TINNo = ddlBusinessType.SelectedValue != "Corporation" ? txtTIN.Text : tTINCorp.Text;

                        string AuthorizedPersonAddress = textAuthorizedPersonAddress.Value;
                        string AuthorizedPersonStreet = txtAuthorizedPersonStreet.Value;
                        string AuthorizedPersonSubdivision = txtAuthorizedPersonSubdivision.Value;
                        string AuthorizedPersonBarangay = txtAuthorizedPersonBarangay.Value;
                        string AuthorizedPersonCity = txtAuthorizedPersonCity.Value;
                        string AuthorizedPersonProvince = txtAuthorizedPersonProvince.Value;
                        string ProofOfBillingAttachment = "";
                        string ValidId1Attachment = "";
                        string ValidId2Attachment = "";
                        string ProofOfIncomeAttachment = "";

                        string SECCORIDNo = txtSECCORIDNo.Text;

                        bool Conforme = CBconforme.Checked;

                        string Religion = txtReligion.Text;
                        string SpecialBuyerRole = "";
                        if (ddlBusinessType.SelectedValue == "Guardianship" || ddlBusinessType.SelectedValue == "Trusteeship")
                        {
                            SpecialBuyerRole = rbGuardian.Checked ? rbGuardian.Text : rbGuardee.Text;
                        }

                        foreach (GridViewRow row in gvStandardDocumentRequirements.Rows)
                        {
                            if (row.Cells[0].Text == "Proof of billing")
                            {
                                ProofOfBillingAttachment = ((Label)row.FindControl("lblFileName")).Text;
                            }
                            else if (row.Cells[0].Text == "Valid ID 1")
                            {
                                ValidId1Attachment = ((Label)row.FindControl("lblFileName")).Text;
                            }
                            else if (row.Cells[0].Text == "Valid ID 2")
                            {
                                ValidId2Attachment = ((Label)row.FindControl("lblFileName")).Text;
                            }
                            else if (row.Cells[0].Text == "Proof of Income")
                            {
                                ProofOfIncomeAttachment = ((Label)row.FindControl("lblFileName")).Text;
                            }
                        }


                        foreach (GridViewRow row in gvStandardDocumentRequirements2.Rows)
                        {
                            if (row.Cells[0].Text == "Proof of billing")
                            {
                                ProofOfBillingAttachment = ((Label)row.FindControl("lblFileName3")).Text;
                            }
                            else if (row.Cells[0].Text == "Valid ID 1")
                            {
                                ValidId1Attachment = ((Label)row.FindControl("lblFileName3")).Text;
                            }
                            else if (row.Cells[0].Text == "Valid ID 2")
                            {
                                ValidId2Attachment = ((Label)row.FindControl("lblFileName3")).Text;
                            }
                            else if (row.Cells[0].Text == "Proof of Income")
                            {
                                ProofOfIncomeAttachment = ((Label)row.FindControl("lblFileName3")).Text;
                            }
                        }

                        if ((string)Session["SpecialInstructions"] == "OTH")
                        {
                            Session["SpecialInstructions"] = tSpecialInstructions.Value;
                        }


                        //2023-06-08 : REMOVE CONDITION; ALWAYS SPA
                        //int SPA = Convert.ToInt32(tSPA.Checked);
                        //int CoBorrower = Convert.ToInt32(tCoBorrower.Checked);


                        //2023-06-07 : SAVING AUTHORIZED PERSON'S NAME
                        string LastName = "";
                        string FirstName = "";
                        string MiddleName = "";

                        if (ddlBusinessType.Text.ToUpper() == "CORPORATION")
                        {
                            LastName = txtLastName2.Text;
                            FirstName = txtFirstName2.Text;
                            MiddleName = txtMiddleName2.Text;
                        }
                        else
                        {
                            LastName = txtLastName.Text;
                            FirstName = txtFirstName.Text;
                            MiddleName = txtMiddleName.Text;
                        }
                        


                        //2024-03-21 : IF TAX CLASSIFICATION IS CORPORATION, CONVERT TO

                        string ret = ws.BusinessPartner(CardCode, _ddNatureOfEmp, //2
                                                    _ddIDType, txtIDNumber.Text, //4
                                                    (string)Session["SalesAgent"] == null ? "" : (string)Session["SalesAgent"], LastName, //6
                                                    FirstName, MiddleName, //8
                                                    _ddGender, txtCitizenship.Value, //10
                                                    (string.IsNullOrEmpty(dtBirthday.Text) ? "1000-01-01" : dtBirthday.Text), txtPlaceOfBirth.Text, //12
                                                    txtTelNo.Text, txtMobileNo.Text, //14
                                                    txtEmail.Text, txtFacebook.Text, //16
                                                    /*tTIN.Value,*/ TINNo, txtSSS.Text, //18
                                                    txtGSIS.Text, txtPagIbig.Text, //20
                                                    txtAddress.Text, txtPermanentAdd.Text, //22
                                                                                           //HomeOwnership, SystemClass.TextIsZero(tYearsOfStay.Value),
                                                    txtEmpName.Text, txtEmpAdd.Text, //24
                                                    txtMyPosition.Text, SystemClass.TextIsZero(txtEmpYrService.Text), //26
                                                    txtEmpTelNo.Text, txtEmpFaxNo.Text, //28
                                                    _ddEmpStatus, _ddNatureOfEmp, //30
                                                    _ddCivilStat, txtSPOLastName.Text, //32
                                                    txtSPOFirstName.Text, txtSPOMiddleName.Text, //34
                                                    _ddSPOGender, txtSPOCitizenShip.Value, //36
                                                    (string.IsNullOrEmpty(dtSPOBirthDate.Text) ? "1000-01-01" : dtSPOBirthDate.Text), txtSPOBirthPlace.Text, ///38
                                                    txtSPOMobile.Text, txtSPOEmail.Text, //40
                                                    txtSPOFB.Text, txtSPOTinNo.Text, //42
                                                    txtSPOSSSNo.Text, txtSPOGSIS.Text, //44
                                                    txtSPOPagibi.Text, txtSPOPosition.Text, //46
                                                    SystemClass.TextIsZero(txtSPOYearsService.Text), txtSPOOfcTelNo.Text, //48
                                                    txtSPOFaxNo.Text, txtSPOBusName.Text, //50
                                                    txtSPOBusAdd.Text, _ddSPOEmpStat, //52
                                                    _ddSPOEmpNatu, tRemarks.Value, //54
                                                    (DataTable)ViewState["dtDependent"], (DataTable)ViewState["BankAccount"], //56
                                                    (DataTable)ViewState["CharacterRef"], IsUpdate, (int)Session["UserID"], //58
                                                    ddlBusinessType.Text, "", //60


                                                    //NEW FIELDS
                                                    txtPresPostal.Text, txtPermPostal.Text, //62
                                                    _ddPresCountry, _ddPermCountry, //64
                                                    _yrAddrStayPerm, txtProfession.Text, //66
                                                    _ddSourceFunds, _ddOccupation, //68
                                                    _ddMonthlyIncome, //69
                                                                      //_ddEmpCountry,
                                                    txtSPOAddress.Text, //70
                                                                        //SPAHomeTelNo,

                                                    //2023-06-27 : CHANGED TO EMP BUSINESS COUNTRY
                                                    //_ddSPOBusCountry, _yrAddrStayPre, //72
                                                    _ddEmpCountry, _yrAddrStayPre, //72


                                                    txtCorpName.Text, "", //74
                                                    ddTaxClass.SelectedValue, _ddIDType2, //76
                                                    txtIDNumber2.Text, _ddIDType3, //78
                                                    txtIDNumber3.Text, _ddIDType4, //80
                                                    txtIDNumber4.Text, (string)Session["SpecialInstructions"] == null ? "" : (string)Session["SpecialInstructions"], //82
                                                    txtBusinessPhoneNo.Text, txtOtherSourceOfFund.Text //84
                                                    , txtCertifyCompleteName.Value, txtCertifyDate.Value //86
                                                    , txtOthers1.Text, txtOthers2.Text //88
                                                    , txtOthers3.Text, txtOthers4.Text //90
                                                    , AuthorizedPersonAddress, AuthorizedPersonStreet //92
                                                    , AuthorizedPersonSubdivision, AuthorizedPersonBarangay //94
                                                    , AuthorizedPersonCity, AuthorizedPersonProvince //96

                                                    , txtPresentStreet.Text, txtPresentSubdivision.Text //98
                                                    , txtPresentBarangay.Text, txtPresentCity.Text //100
                                                    , txtPresentProvince.Text, txtPermanentStreet.Text //102
                                                    , txtPermanentSubdivision.Text, txtPermanentBarangay.Text //104
                                                    , txtPermanentCity.Text, txtPermanentProvince.Text //108
                                                    , txtSpecifyBusiness.Text, Conforme //110
                                                    , ProofOfBillingAttachment, ValidId1Attachment //112
                                                    , ValidId2Attachment, Religion //114
                                                    , SECCORIDNo, SpecialBuyerRole //116
                                                    , ViewState["Guid"].ToString(), ProofOfIncomeAttachment //118
                                                    , (DataTable)ViewState["SPA"], _ddSPOBusCountry
                                        );

                        if (ret == "Operation completed successfully.")
                        {
                            moveTemporaryFilesToPermanent(gvStandardDocumentRequirements, "lblFileName");
                            moveTemporaryFilesToPermanent(gvStandardDocumentRequirements2, "lblFileName3");

                            moveTemporaryFilesToPermanentSPA(gvSPACoBorrower);

                            DeleteTemporaryFIles();

                            step_1.Visible = false;
                            step_2.Visible = false;
                            done_step.Visible = true;
                            step1.Attributes.Add("class", "btn btn-success btn-circle");
                            step2.Attributes.Add("class", "btn btn-success btn-circle");

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowResultSuccess();", true);
                            alertIcon.ImageUrl = "~/assets/img/success.png";
                            lblMessageAlert.Text = "Adding Successful!";
                        }
                        else
                        {
                            //DELETE SAVED DATA FROM SUB TABLES TO PREVENT ADDING EXTRA DATA ON THE NEXT SUCCESSFUL SAVE
                            hana.Execute($@"DELETE FROM CRD2 WHERE LCASE(""CardCode"") = '{CardCode.ToLower()}';
                                            DELETE FROM CRD3 WHERE LCASE(""CardCode"") = '{CardCode.ToLower()}';
                                            DELETE FROM CRD4 WHERE LCASE(""CardCode"") = '{CardCode.ToLower()}';
                                            DELETE FROM CRD5 WHERE LCASE(""CardCode"") = '{CardCode.ToLower()}';", hana.GetConnection("SAOHana"));
                            alertMsg2(ret, "error");
                        }

                    }
                    else
                    {
                        //alertMsg("Cannot add buyer. TIN already exists!", "error");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowResultFailed2();", true);
                        alert2img.ImageUrl = "~/assets/img/error.png";
                        alert2lbl.Text = "Cannot add buyer. TIN already exists!";
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowResultFailed2();", true);
                    alert2img.ImageUrl = "~/assets/img/error.png";
                    alert2lbl.Text = ex.Message;
                }

            }
        }
        protected bool updateinfo(string CardCode)
        {
            try
            {
                string _ddNatureOfEmp = ddNatureOfEmployement.SelectedValue.Contains("Select") ? "" : ddNatureOfEmployement.SelectedValue;
                string _ddEmpCountry = ddEmployCountry.SelectedValue.Contains("Select") ? "" : ddEmployCountry.SelectedValue;
                string _ddEmpStatus = ddEmploymentStatus.SelectedValue.Contains("Select") ? "" : ddEmploymentStatus.SelectedValue;
                string _ddCivilStat = ddCivilStatus.SelectedValue.Contains("Select") ? "" : ddCivilStatus.SelectedValue;
                string _ddIDType = ddTypeOfID.SelectedValue.Contains("Select") ? "" : ddTypeOfID.SelectedValue;
                string _ddGender = ddGender.SelectedValue.Contains("Select") ? "" : ddGender.SelectedValue;
                string _ddSourceFunds = ddSourceFunds.SelectedValue.Contains("Select") ? "" : ddSourceFunds.SelectedValue;
                string _ddOccupation = ddOccupation.SelectedValue.Contains("Select") ? "" : ddOccupation.SelectedValue;
                string _ddMonthlyIncome = ddMonthlyIncome.SelectedValue.Contains("Select") ? "" : ddMonthlyIncome.SelectedValue;

                string _yrAddrStayPre = String.IsNullOrEmpty(txtPresYrStay.Text) ? "1" : txtPresYrStay.Text; ////
                string _yrservice = String.IsNullOrEmpty(txtEmpYrService.Text) ? "1" : txtEmpYrService.Text;
                string _yrAddrStayPerm = string.IsNullOrEmpty(txtPermYrStay.Text) ? "1" : txtPermYrStay.Text; ////

                string _ddPresCountry = ddPreCountry.SelectedValue.Contains("Select") ? "" : ddPreCountry.SelectedValue;
                string _ddPermCountry = ddPermCountry.SelectedValue.Contains("Select") ? "" : ddPermCountry.SelectedValue;

                string _ddSPOBusCountry = ddSPOBusCountry.SelectedValue.Contains("Select") ? "" : ddSPOBusCountry.SelectedValue;
                string _ddSPOEmpStat = ddSPOEmpStat.SelectedValue.Contains("Select") ? "" : ddSPOEmpStat.SelectedValue;
                string _ddSPOEmpNatu = ddSPONatureEmp.SelectedValue.Contains("Select") ? "" : ddSPONatureEmp.SelectedValue;
                string _ddSPOGender = ddSPOGender.SelectedValue.Contains("Select") ? "" : ddSPOGender.SelectedValue;
                string _spoyrserv = String.IsNullOrEmpty(txtSPOYearsService.Text) ? "1" : txtSPOYearsService.Text;

                string _ddSPABusCountry = ddSPABusCountry.SelectedValue.Contains("Select") ? "" : ddSPABusCountry.SelectedValue;
                string _ddSPAEmpStat = ddSPAEmpStat.SelectedValue.Contains("Select") ? "" : ddSPAEmpStat.SelectedValue;
                string _ddSPAEmpNatu = ddSPANatureEmp.SelectedValue.Contains("Select") ? "" : ddSPANatureEmp.SelectedValue;
                string _ddSPAGender = ddSPAGender.SelectedValue.Contains("Select") ? "" : ddSPAGender.SelectedValue;
                string _spayrserv = string.IsNullOrEmpty(txtSPAYearsService.Text) ? "1" : txtSPAYearsService.Text;

                //string _ownval = string.IsNullOrEmpty(txtRented.Text) ? "0" : txtRented.Text;
                string comaker = string.IsNullOrEmpty(txtComaker.Text) ? "N/A" : txtComaker.Text;
                string _ownership;
                //if (rb1.Checked == true)
                //{
                //    _ownership = "Owned";
                //}
                //else if (rb2.Checked == true)
                //{
                //    _ownership = "Mortgaged";
                //}
                //else if (rb3.Checked == true)
                //{
                //    _ownership = "Living w/ Relatives";
                //}
                //else
                //{
                //    _ownership = "Rented";
                //}

                int countfail = 0;

                //--""HomeOwnership"" = '{_ownership}',
                //--""YearsStay"" = '{_yrAddrStayPre}',
                //--""HomeOwnerValue"" = '{_ownval.Replace(",", "")}',
                string qryOCRD = $@"UPDATE OCRD SET 
                                    ""NatureEmp"" = '{_ddNatureOfEmp}',
                                    ""IDType"" = '{_ddIDType}',
                                    ""IDNo"" = '{txtIDNumber.Text}',
                                    ""HomeTelNo"" = '{txtTelNo.Text}',
                                    ""PresentAddress"" = '{txtAddress.Text}',
                                    ""PermanentAddress"" = '{txtPermanentAdd.Text}',
                                    ""CivilStatus"" = '{_ddCivilStat}',
                                    ""Remarks"" = '',
                                    --""UpdateDate"" = '{DateTime.Now.ToString("yyyy-MM-dd")}',
                                    ""SalesAgent"" = '',
                                    ""PresentPostalCode"" = '{txtPresPostal.Text}',
                                    ""PermanentPostalCode"" = '{txtPermPostal.Text}',
                                    ""PresentCountry"" = '{_ddPresCountry}',
                                    ""PermanentCountry"" = '{_ddPermCountry}',
                                    ""Profession"" = '{txtProfession.Text}',
                                    ""PermanentYrStay"" = '{txtPermYrStay.Text}',
                                    ""SourceOfFunds"" = '{_ddSourceFunds}',
                                    ""OtherSourceOfFund"" = '{txtOtherSourceOfFund.Text}',
                                    ""Occupation"" = '{_ddOccupation}',
                                    ""MonthlyIncome"" = '{_ddMonthlyIncome}'
                                    ""BusinessType"" = '{ddlBusinessType.SelectedValue}',
                                    ""TaxClassification"" = '{ddTaxClass.SelectedValue}',""Comaker"" = 'N/A'                                    WHERE LCASE(""CardCode"") = '{CardCode.ToLower()}'; ";
                if (hana.Execute(qryOCRD, hana.GetConnection("SAOHana")) == false) countfail += 1;

                string qryCRD1 = $@"UPDATE CRD1 SET
                                    ""LastName"" = '{txtLastName.Text}',
                                    ""FirstName"" = '{txtFirstName.Text}',
                                    ""MiddleName"" = '{txtMiddleName.Text}',
                                    ""Gender"" = '{_ddGender}',
                                    ""Citizenship"" = '{txtCitizenship.Value}',
                                    ""BirthDay"" = '{dtBirthday.Text}',
                                    ""BirthPlace"" = '{txtPlaceOfBirth.Text}',
                                    ""CellNo"" = '{txtMobileNo.Text}',
                                    ""EmailAddress"" = '{txtEmail.Text}',
                                    ""FBAccount"" = '{txtFacebook.Text}',
                                    ""TIN"" = '{txtTIN.Text}',
                                    ""SSSNo"" = '{txtSSS.Text}',
                                    ""GSISNo"" = '{txtGSIS.Text}',
                                    ""PagibiNo"" = '{txtPagIbig.Text}',
                                    ""Position"" = '{txtMyPosition.Text}',
                                    ""YearsService"" = '{_yrservice}',
                                    ""OfficeTelNo"" = '{txtEmpTelNo.Text}',
                                    ""FaxNo"" = '{txtEmpFaxNo.Text}',
                                    ""EmpBusName"" = '{txtEmpName.Text}',
                                    ""EmpBusAdd"" = '{txtEmpAdd.Text}',
                                    ""EmpStatus"" = '{_ddEmpStatus}',
                                    ""NatureEmp"" = '{_ddNatureOfEmp}',                                         
                                    ""EmpBusCountry"" = '{_ddEmpCountry}',
                                    ""PresentAddress"" = '{txtAddress.Text}'
                                    WHERE LCASE(""CardCode"") = '{CardCode.ToLower()}' AND ""CardType"" = 'Buyer'; ";

                if (hana.Execute(qryCRD1, hana.GetConnection("SAOHana")) == false) countfail += 1;

                string qryCRD1_ = $@"INSERT INTO CRD1 
                                    (
                                    ""CardCode"" ,
                                    ""CardType"" ,
                                    ""LastName"" ,
                                    ""FirstName"" ,
                                    ""MiddleName"" ,
                                    ""Gender"" ,
                                    ""Citizenship"" ,
                                    ""BirthDay"" ,
                                    ""BirthPlace"" ,
                                    ""CellNo"" ,
                                    ""EmailAddress"" ,
                                    ""FBAccount"" ,
                                    ""TIN"" ,
                                    ""SSSNo"" ,
                                    ""GSISNo"" ,
                                    ""PagibiNo"" ,
                                    ""Position"" ,
                                    ""YearsService"" ,
                                    ""OfficeTelNo"" ,
                                    ""FaxNo"" ,
                                    ""EmpBusName"" ,
                                    ""EmpBusAdd"" ,
                                    ""EmpStatus"" ,
                                    ""NatureEmp"" ,
                                    ""EmpBusCountry"" ,
                                    ""PresentAddress"")
                                    VALUES (
                                     '{CardCode}'
                                    , 'Spouse'
                                    , '{txtSPOLastName.Text}'
                                    , '{txtSPOFirstName.Text}'
                                    , '{txtSPOMiddleName.Text}'
                                    , '{_ddSPOGender}'
                                    , '{txtSPOCitizenShip.Value}'
                                    , '{dtSPOBirthDate.Text}'
                                    , '{txtSPOBirthPlace.Text}'
                                    , '{txtSPOMobile.Text}'
                                    , '{txtSPOEmail.Text}'
                                    , '{txtSPOFB.Text}'
                                    , '{txtSPOTinNo.Text}'
                                    , '{txtSPOSSSNo.Text}'
                                    , '{txtSPOGSIS.Text}'
                                    , '{txtSPOPagibi.Text}'
                                    , '{txtSPOPosition.Text}'
                                    , '{_spoyrserv}'
                                    , '{txtEmpTelNo.Text}'
                                    , '{txtEmpFaxNo.Text}'
                                    , '{txtSPOBusName.Text}'
                                    , '{txtSPOBusAdd.Text}'
                                    , '{_ddSPOEmpStat}'
                                    , '{_ddSPOEmpNatu}'
                                    , '{_ddSPOBusCountry}'
                                    , '{txtSPOAddress.Text}')                                    
                                    ";
                ////--Remove first before save--////
                hana.Execute($@"DELETE FROM CRD1 WHERE LCASE(""CardCode"") = '{CardCode.ToLower()}' AND ""CardType""='Spouse' ; ", hana.GetConnection("SAOHana"));
                if (hana.Execute(qryCRD1_, hana.GetConnection("SAOHana")) == false) countfail += 1;

                string qryCRD5 = $@"INSERT INTO CRD5                                    
                                    (
                                    ""CardCode"" ,
                                    ""SPA"" ,
                                    ""CoBorrower"" ,
                                    ""LastName"" ,
                                    ""FirstName"" ,
                                    ""MiddleName"" ,
                                    ""Gender"" ,
                                    ""Citizenship"" ,
                                    ""BirthDay"" ,
                                    ""BirthPlace"" ,
                                    ""CellNo"" ,
                                    ""EmailAddress"" ,
                                    ""FBAccount"" ,
                                    ""TIN"" ,
                                    ""SSSNo"" , 
                                    ""GSISNo"" ,
                                    ""PagibiNo"" ,
                                    ""Position"" ,
                                    ""YearsService"" , 
                                    ""OfficeTelNo"" ,
                                    ""FaxNo"" ,
                                    ""EmpBusName"" ,
                                    ""EmpBusAdd"" ,
                                    ""EmpStatus"" ,
                                    ""NatureEmp"" ,
                                    ""SPAHomeTelNo"" ,     
                                    ""SPAYearsStay"",
                                    ""SPABusCountry"",
                                    ""SPAPresentAddress""                                   
                                    )
                                    VALUES ( 
                                     '{CardCode}'
                                    , true
                                    , false
                                    , '{txtSPALastName.Text}'
                                    , '{txtSPAFirstName.Text}'
                                    , '{txtSPAMiddleName.Text}'
                                    , '{_ddSPAGender}'
                                    , '{txtSPACitizenship.Value}'
                                    , '{dtSPABirthDate.Text}'
                                    , '{txtSPABirthPlace.Text}'
                                    , '{txtSPAMobile.Text}'
                                    , '{txtSPAEmail.Text}'
                                    , '{txtSPAFB.Text}'
                                    , '{txtSPATinNo.Text}'
                                    , '{txtSPASSSNo.Text}'
                                    , '{txtSPAGSIS.Text}'
                                    , '{txtSPAPagibi.Text}'
                                    , '{txtSPAPosition.Text}'
                                    , '{_spayrserv}'
                                    , '{txtSPAOfcTelNo.Text}'
                                    , '{txtSPAFaxNo.Text}'
                                    , '{txtSPABusName.Text}'
                                    , '{txtSPABusAdd.Text}'
                                    , '{_ddSPAEmpStat}'
                                    , '{_ddSPAEmpNatu}'
                                    , '{txtSPATelNo.Text}'
                                    , '{_spayrserv}'
                                    , '{_ddSPABusCountry}'
                                    , '{txtSPAAddress.Text}')
                                    ";
                ////--Remove first before save--////
                hana.Execute($@"DELETE FROM CRD5 WHERE LCASE(""CardCode"") = '{CardCode.ToLower()}' AND ""SPA""='true' ; ", hana.GetConnection("SAOHana"));
                if (hana.Execute(qryCRD5, hana.GetConnection("SAOHana")) == false) countfail += 1;

                ////--Save Dependent Reference--////
                hana.Execute($@"DELETE FROM CRD2 WHERE ""CardCode""='{CardCode}'; ", hana.GetConnection("SAOHana"));
                foreach (GridViewRow row in gvDependent.Rows)
                {
                    string qryCRD4 = $@"INSERT INTO CRD2 (""CardCode"",""ID"", ""Name"", ""Age"", ""Relationship"", ""DependentType"", ""DocEntry"", ""LineNum""  ) 
                                        VALUES ('{CardCode}','{row.Cells[0].Text}','{row.Cells[1].Text}','{row.Cells[2].Text}','{row.Cells[3].Text}','B','0','0')";
                    if (hana.Execute(qryCRD4, hana.GetConnection("SAOHana")) == false) countfail += 1;
                }

                ////--Save Bank Reference--////
                hana.Execute($@"DELETE FROM CRD3 WHERE ""CardCode""='{CardCode}'; ", hana.GetConnection("SAOHana"));
                foreach (GridViewRow row in gvBankAccount.Rows)
                {
                    string qryCRD4 = $@"INSERT INTO CRD3 (""CardCode"",""Bank"", ""Branch"", ""AcctType"", ""AcctNo"",""AvgDailyBal"", ""PresentBal"") VALUES ('{CardCode}','{row.Cells[0].Text}','{row.Cells[1].Text}','{row.Cells[2].Text}','{row.Cells[3].Text}','0','0')";
                    if (hana.Execute(qryCRD4, hana.GetConnection("SAOHana")) == false) countfail += 1;
                }

                ////--Save Character Reference--////
                hana.Execute($@"DELETE FROM CRD6 WHERE ""CardCode""='{CardCode}'; ", hana.GetConnection("SAOHana"));
                foreach (GridViewRow row in gvCharacter.Rows)
                {
                    string qryCRD4 = $@"INSERT INTO CRD6 (""CardCode"",""FullName"", ""Address"",""Email"", ""ContactNo"", ""CreateDate"") VALUES ('{CardCode}','{row.Cells[0].Text}','{row.Cells[1].Text}','{row.Cells[2].Text}','{row.Cells[3].Text}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}')";
                    if (hana.Execute(qryCRD4, hana.GetConnection("SAOHana")) == false) countfail += 1;
                }

                if (countfail > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                string x = ex.Message;
                return false;
            }

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            step_1.Visible = false;
            step_2.Visible = true;
            step_3.Visible = false;
            step1.Attributes.Add("class", "btn btn-success btn-circle");
            step2.Attributes.Add("class", "btn btn-info btn-circle");
            //step3.Attributes.Add("class", "btn btn-default btn-circle");
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

        void alertMsg2(string Message, string type)
        {
            alert2lbl.Text = Message;
            if (type == "success")
                alert2img.ImageUrl = "~/assets/img/success.png";
            else if (type == "warning")
                alert2img.ImageUrl = "~/assets/img/warning.png";
            else if (type == "error")
                alert2img.ImageUrl = "~/assets/img/error.png";
            else if (type == "info")
                alert2img.ImageUrl = "~/assets/img/info.png";

            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "ShowAlert2();", true);
        }

        protected void btnAdd_ServerClick(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string session = (string)Session["bDependent"];

            if (session == "bDependent")
            {
                dt = (DataTable)ViewState["dtDependent"];

                DataRow dr = dt.NewRow();

                dr[0] = Convert.ToInt32(dt.Rows.Count + 1);
                dr[1] = mtxtName.Text.Trim().ToUpper();
                dr[2] = Convert.ToUInt64(mtxtAge.Text);
                dr[3] = mtxtRelationship.Value;
                //dr[1] = tDependentName.Value;
                //dr[2] = Convert.ToUInt64(tDependentAge.Value);
                //dr[3] = tDependentRelationship.Value;

                dt.Rows.Add(dr);

                ViewState["dtDependent"] = dt;

                //LoadData(gvDependent, "dtDependent");
                gvDependent.DataSource = dt;
                gvDependent.DataBind();

                ClearDependent();
            }
            else if (session == "bSPADependent")
            {
                int count = GetCountID("LineNum");

                if (ws.AddSPADependent((int)Session["UserID"], (int)Session["SPACoBorrowerCount"], count, tDependentName.Value, int.Parse(tDependentAge.Value), tDependentRelationship.Value) == true)
                {
                    dt = ws.select_temp_crd2((int)Session["UserID"], (int)Session["SPACoBorrowerCount"]).Tables["select_temp_crd2"];
                    ViewState["dtSPADependent"] = dt;
                    gvSPADependent.DataSource = dt;
                    gvSPADependent.DataBind();
                    //LoadData(gvSPADependent, "dtSPADependent");
                    ClearDependent();
                }
            }
            //else
            //{
            //    DataTable dt = (DataTable)ViewState["Dependent"];
            //    dt.Rows.Add(
            //        Convert.ToInt32(dt.Rows.Count + 1),
            //        mtxtName.Text.Trim().ToUpper(),
            //         mtxtAge.Text.Trim(),
            //          mtxtRelationship.Value.Trim().ToUpper());
            //    ViewState["Dependent"] = dt;
            //    this.BindGrid();

            //    ClearAllModalFields();
            //}
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "MsgDependent_Hide();", true);

            //step_1.Visible = false;
            //step_2.Visible = true;
            //step_3.Visible = false;
            //step1.Attributes.Add("class", "btn btn-success btn-circle");
            //step2.Attributes.Add("class", "btn btn-info btn-circle");
            //step3.Attributes.Add("class", "btn btn-default btn-circle");
        }

        protected void ClearAllModalFields()
        {
            mtxtRelationship.Value = "";
            mtxtName.Text = "";
            mtxtAge.Text = "";
            mtxtBankName.Text = "";
            mtxtBranch.Text = "";
            mtxtAccount.Text = "";
            mtxtAccountNo.Text = "";
            mtxtAverage.Text = "";
            mtxtBalance.Text = "";
            reftxtName.Value = "";
            reftxtAddress.Value = "";
            reftxtEmail.Value = "";
            reftxtContact.Value = "";
        }

        protected void btnAddBank_ServerClick(object sender, EventArgs e)
        {
            try
            {
                bool existrec = false;
                DataTable dt = (DataTable)ViewState["BankAccount"];


                foreach (DataRow row in dt.Rows)
                {
                    if (mtxtAccountNo.Text.Trim() == row["AcctNo"].ToString())
                    {
                        existrec = true;
                    }
                }

                //CHECK IF ACCOUNT NUMBER ALREADY EXISTS
                if (existrec == true)
                {
                    //lblMessageAlert.Text = "Bank Account already exist!";
                    //alertIcon.ImageUrl = "~/assets/img/info.png";

                    //ScriptManager.RegisterStartupScript(this, GetType(), "alert", "ShowAlert();", true);
                    alertMsg2("Bank Account already exist!", "info");

                }
                else
                {

                    dt.Rows.Add(
                            Convert.ToInt32(dt.Rows.Count + 1),
                            mtxtBankName.Text.Trim().ToUpper(),
                            mtxtBranch.Text.Trim().ToUpper(),
                            mtxtAccount.Text.Trim().ToUpper(),
                            mtxtAccountNo.Text.Trim(),
                            mtxtAccountNo.Text.Trim());

                    ViewState["BankAccount"] = dt;
                    this.BindGrid2();
                    ClearAllModalFields();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideAddDependent2();", true);




                    step_1.Visible = false;
                    step_2.Visible = false;
                    step_3.Visible = true;
                    step1.Attributes.Add("class", "btn btn-success btn-circle");
                    step2.Attributes.Add("class", "btn btn-success btn-circle");
                    //step3.Attributes.Add("class", "btn btn-info btn-circle");
                }
            }
            catch (Exception ex)
            {
                alertMsg2(ex.Message, "error");
            }

        }

        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ CUSTOMIZED METHOD @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@


        void LoadBuyerDetails()
        {
            //try
            //{
            //    // POPULAT BROKER DETAILS -- HEADER
            //    DataTable dtBuyerInformation = hana.GetData($"CALL sp_BPEditOCRD ('{txtUniqueID.Text}')", hana.GetConnection("SAOHana"));

            //    if (dtBuyerInformation.Rows.Count != 0)
            //    {
            //        //ddNatureOfEmployement.SelectedValue = ws.GetOLSTName(tCode);
            //        //ddTypeOfID.SelectedValue = ws.GetOLSTName(tCode);
            //        //tIDNo.Value = (string)DataAccess.GetData(dt, 0, "IDNo", "");

            //        //tHomeTelNo.Value = (string)DataAccess.GetData(dt, 0, "HomeTelNo", "");
            //        //tPresentAddress.Value = (string)DataAccess.GetData(dt, 0, "PresentAddress", "");
            //        //tPermanent.Value = (string)DataAccess.GetData(dt, 0, "PermanentAddress", "");
            //        //string HomeOwnership = (string)DataAccess.GetData(dt, 0, "HomeOwnership", "");

            //        //tPerMonth.Disabled = true;
            //        //if (HomeOwnership == "Owned")
            //        //{ tRented_CheckedChanged(tOwned, EventArgs.Empty); }
            //        //else if (HomeOwnership == "Mortgaged")
            //        //{ tMortgaged.Checked = true; }
            //        //else if (HomeOwnership == "LivingwRelatives")
            //        //{ tLivingwRelatives.Checked = true; }
            //        //else
            //        //{
            //        //    tRented.Checked = true;
            //        //    tPerMonth.Disabled = false;
            //        //    tPerMonth.Value = HomeOwnership;
            //        //}

            //        //tYearsOfStay.Value = (string)DataAccess.GetData(dt, 0, "YearsStay", "");

            //        //tCode = (string)DataAccess.GetData(dt, 0, "CivilStatus", "");

            //        //if (ws.OLSTExist(tCode) == true)
            //        //{ Session["tCivilStatus"] = tCode; tCivilStatus.Value = ws.GetOLSTName(tCode); }
            //        //else { Session["tCivilStatus"] = "OTH"; tCivilStatus.Value = tCode; }


            //        //tRemarks.Value = (string)DataAccess.GetData(dt, 0, "Remarks", "");
            //    }



            //}
            //catch (Exception ex)
            //{
            //    alertMsg(ex.Message, "error");
            //}
        }
        protected void btnStartPage_ServerClick(object sender, EventArgs e)
        {
            this.Response.Redirect("~/pages/BuyerInformation.aspx");
        }

        protected void btnAddCharRef_ServerClick(object sender, EventArgs e)
        {
            if (reftxtName.Value == "" || reftxtAddress.Value == "" || reftxtEmail.Value == "")
            { alertMsg2("Please fill up all forms before adding!", "info"); }
            else
            {
                DataTable dt = (DataTable)ViewState["CharacterRef"];
                dt.Rows.Add(
                    Convert.ToInt32(dt.Rows.Count + 1),
                    reftxtName.Value.Trim().ToUpper(),
                     reftxtAddress.Value.Trim().ToUpper(),
                     reftxtEmail.Value.Trim(),
                      reftxtContact.Value.Trim());
                ViewState["CharacterRef"] = dt;
                this.BindGrid3();

                ClearAllModalFields();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideCharacterReference();", true);
            }

        }

        protected void btnAddDep_Click(object sender, EventArgs e)
        {
            btnAdd.Visible = true;
            btnUpdate.Visible = false;
            Control btn = (Control)sender;
            Session["bDependent"] = btn.ID;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowAddDependent();", true);
        }
        protected void btnDepEdit_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            int row = int.Parse(GetID.CommandArgument.ToString());
            mtxtName.Text = gvDependent.Rows[row].Cells[1].Text;
            mtxtAge.Text = gvDependent.Rows[row].Cells[2].Text;
            mtxtRelationship.Value = gvDependent.Rows[row].Cells[3].Text;

            //2023-06-17 : GET ROW # FROM GRIDVIEW INSTEAD OF ACTUAL INDEX
            //mtxtRow.Text = row.ToString();
            mtxtRow.Text = gvDependent.Rows[row].Cells[0].Text;


            btnAdd.Visible = false;
            btnUpdate.Visible = true;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowAddDependent();", true);
        }
        protected void btnDeleteDependent_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            int row = int.Parse(GetID.CommandArgument.ToString());
            Session["ExtBuyerRow"] = row;
            Session["ExtGv"] = "gvDependent";
            confirmation($"Are you sure you want to remove dependent?");
        }
        protected void btnDeleteBank_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            int row = int.Parse(GetID.CommandArgument.ToString());
            Session["ExtBuyerRow"] = row;
            Session["ExtGv"] = "gvBankAccount";
            confirmation($"Are you sure you want to remove bank account?");
        }
        protected void btnDeleteRef_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            int row = int.Parse(GetID.CommandArgument.ToString());
            Session["ExtBuyerRow"] = row;
            Session["ExtGv"] = "gvCharacter";
            confirmation($"Are you sure you want to remove character reference?");
        }
        void confirmation(string body)
        {
            Session["ConfirmType"] = "";
            lblConfirmationInfo.Text = body;
            ScriptManager.RegisterStartupScript(this, GetType(), "confirmation", "showMsgConfirm();", true);
        }

        //Deletes row in GridView
        protected void btnYes_Click(object sender, EventArgs e)
        {
            try
            {

                int row = int.Parse(Session["ExtBuyerRow"].ToString());
                string gv = Session["ExtGv"].ToString();
                DataTable dt = new DataTable();
                if ((string)Session["ConfirmType"] == "DelDependent")
                {
                    if ((string)Session["Buyers_Control"] == "gvDelete")
                    {
                        dt = (DataTable)ViewState["dtDependent"];

                        for (int i = dt.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = dt.Rows[i];

                            if (Convert.ToInt32(dr["ID"]) == (int)Session["Buyers_ID"])
                            { dr.Delete(); }
                        }

                        LoadData(gvDependent, "dtDependent");
                    }
                    else if ((string)Session["Buyers_Control"] == "gvSPADelete")
                    {
                        //dt = (DataTable)ViewState["dtSPADependent"];

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
                            ViewState["dtSPADependent"] = dt;
                            LoadData(gvSPADependent, "dtSPADependent");
                        }

                    }
                    else if ((string)Session["Buyers_Control"] == "gvSPADelete")
                    {
                        dt = (DataTable)ViewState["dtSPADependent"];

                        for (int i = dt.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = dt.Rows[i];

                            if (Convert.ToInt32(dr["ID"]) == (int)Session["Buyers_ID"])
                            { dr.Delete(); }
                        }

                        LoadData(gvSPADependent, "dtSPADependent");
                    }
                }
                else if ((string)Session["ConfirmType"] == "DelSPACoBorrower")
                {
                    //2023-06-18 : COMMENTED FOR CHANGING OF SPA PROCESS
                    ////int ID = (int)Session["SPACoBorrowerCount"];
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



                }
                else if ((string)Session["ConfirmType"] == "DelCoOwner")
                {
                    dt = (DataTable)Session["dtCoOwner"];

                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = dt.Rows[i];

                        if (dr.RowState.ToString() != "Deleted")
                            if (dr["ID"].ToString() == row.ToString())
                            { dr.Delete(); }
                    }

                    LoadData(gvCoOwner, "dtCoOwner");
                }
                else if ((string)Session["ConfirmType"] == "DelOthers")
                {
                    dt = (DataTable)Session["dtOthers"];

                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = dt.Rows[i];

                        if (dr.RowState.ToString() != "Deleted")
                            if (dr["ID"].ToString() == row.ToString())
                            { dr.Delete(); }
                    }

                    LoadData(gvOthers, "dtOthers");
                }
                else if ((string)Session["ConfirmType"] == "AddSPACoBorrower")
                {
                    if (!string.IsNullOrEmpty(tRelationship.Value))
                    {

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

                        if (!string.IsNullOrEmpty(dtSPABirthDate.Text))
                        { bdate = Convert.ToDateTime(dtSPABirthDate.Text).ToString("yyyy-MM-dd"); }


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
                                                txtSPALastName.Text,  //6
                                                txtSPAFirstName.Text, //7
                                                txtSPAMiddleName.Text, //8
                                                ddSPAGender.SelectedValue,  //9
                                                txtSPACitizenship.Value, //10
                                                bdate, //11
                                                txtSPABirthPlace.Text, //12
                                                txtSPAMobile.Text,  //13
                                                txtSPATelNo.Text, //14
                                                txtSPAEmail.Text, //15
                                                txtSPAFB.Text,  //16
                                                txtSPATinNo.Text,  //17
                                                txtSPASSSNo.Text, //18
                                                txtSPAGSIS.Text, //19
                                                txtSPAPagibi.Text, //20
                                                txtSPAAddress.Text, //21 
                                                txtSPAAddress.Text, //22
                                                txtSPAPosition.Text, //23
                                                int.Parse(txtSPAYearsService.Text == "" ? "0" : txtSPAYearsService.Text), //24
                                                txtSPAOfcTelNo.Text,  //25
                                                txtSPAFaxNo.Text,  //26
                                                SPAHomeOwnership,  //27

                                                //2023-06-15: UPDATE TO SPA'S YEARS OF STAY 
                                                //int.Parse(txtSPAYearsService.Text == "" ? "0" : txtSPAYearsService.Text), //24
                                                double.Parse(tSPAYearsOfStay.Value == "" ? "0" : tSPAYearsOfStay.Value), //28

                                                txtSPABusName.Text, //29
                                                txtSPABusAdd.Text, //30
                                                ddSPAEmpStat.SelectedValue,  //31
                                                ddSPANatureEmp.SelectedValue, //32
                                                ddSPACivilStatus.SelectedValue,   //33   
                                                lblSPAFileName.Text) == true)

                            {

                                Session["SPACoBorrowerCount"] = GetCountID("ID");

                                dt = ws.select_temp_crd5(UserID).Tables["select_temp_crd5"];
                                ClearSPACoBorrower();
                                Session["gvSPACoBorrower"] = dt;
                                LoadData(gvSPACoBorrower, "gvSPACoBorrower");
                            }
                        }
                    }
                    else
                    {
                        alertMsg2("Please fill up SPA &/or Co-Borrower details.", "warning");
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);
                    }
                    ScriptManager.RegisterStartupScript(this, GetType(), "hide", "MsgSPACoBorrower_Hide();", true);
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);
                }

                if (gv == "gvDependent")
                {
                    dt = (DataTable)ViewState["dtDependent"];
                    dt.Rows.RemoveAt(row);
                    gvDependent.DataSource = dt;
                    ViewState["dtDependent"] = dt;
                    gvDependent.DataBind();
                }
                //else if (gv == "gvSPADependent")
                //{
                //    dt = (DataTable)ViewState["dtSPADependent"];
                //    dt.Rows.RemoveAt(row);
                //    gvSPADependent.DataSource = dt;
                //    ViewState["dtSPADependent"] = dt;
                //    gvSPADependent.DataBind();
                //}
                else if (gv == "gvBankAccount")
                {
                    dt = (DataTable)ViewState["BankAccount"];
                    dt.Rows.RemoveAt(row);
                    gvBankAccount.DataSource = dt;
                    ViewState["BankAccount"] = dt;
                    gvBankAccount.DataBind();
                }
                else if (gv == "gvCharacter")
                {
                    dt = (DataTable)ViewState["CharacterRef"];
                    dt.Rows.RemoveAt(row);
                    gvCharacter.DataSource = dt;
                    ViewState["CharacterRef"] = dt;
                    gvCharacter.DataBind();
                }

                //2023-06-18 : COMMENTED FOR CHANGING OF SPA PROCESS
                else if (gv == "gvSPACoBorrower")
                {
                    dt = (DataTable)ViewState["SPA"];
                    dt.Rows.RemoveAt(row);
                    gvSPACoBorrower.DataSource = dt;
                    ViewState["SPA"] = dt;
                    gvSPACoBorrower.DataBind();
                }

                closeconfirm();

            }
            catch (Exception ex)
            {
                alertMsg(ex.Message, "info");
            }
        }
        void closeconfirm()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "closeConfirmation", "hideMsgConfirm();", true);
        }
        protected void gvDependent_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        protected void btnUpdate_ServerClick(object sender, EventArgs e)
        {
            try
            {

                int row = int.Parse(mtxtRow.Text);

                //2023-06-17 : COMMENTED TO FIX UPDATING
                //gvDependent.Rows[row].Cells[1].Text = mtxtName.Text.ToUpper();
                //gvDependent.Rows[row].Cells[2].Text = mtxtAge.Text;
                //gvDependent.Rows[row].Cells[3].Text = mtxtRelationship.Value.ToUpper();

                //UPDATE ViewState for Updating

                DataTable dt = (DataTable)ViewState["dtDependent"];

                //2023-06-17 : COMMENTED TO FIX UPDATING
                //ViewState["dtDependent"] = null;
                //dt.Rows.Clear();

                //foreach (GridViewRow gvRows in gvDependent.Rows)
                //{
                //    DataRow dr = dt.NewRow();

                //    dr[0] = Convert.ToInt32(dt.Rows.Count + 1);
                //    dr[1] = mtxtName.Text;
                //    dr[2] = Convert.ToInt32(mtxtAge.Text);
                //    dr[3] = mtxtRelationship.Value;
                //    //dr[1] = tDependentName.Value;
                //    //dr[2] = Convert.ToUInt64(tDependentAge.Value);
                //    //dr[3] = tDependentRelationship.Value;

                //    dt.Rows.Add(dr);

                //    ViewState["dtDependent"] = dt;
                //}


                //2023-06-17 : NEW SAVING OF UPDATE
                //clear Session
                ViewState["dtDependent"] = null;

                foreach (DataRow dr in dt.Rows)
                {
                    //update row of datatable
                    if (int.Parse(dr["Id"].ToString()) == row)
                    {
                        dr["Name"] = mtxtName.Text.Trim().ToUpper();
                        dr["Age"] = Convert.ToInt32(mtxtAge.Text);
                        dr["Relationship"] = mtxtRelationship.Value;
                    }

                }
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

        protected void btnShowBank_Click(object sender, EventArgs e)
        {
            banktitle.InnerText = "Add Bank Account";
            btnAddBank.Visible = true;
            btnUpdateBank.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowAddDependent2();", true);
        }
        protected void btnBankUpdate_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;

            int row = int.Parse(GetID.CommandArgument.ToString());

            mtxtBankName.Text = gvBankAccount.Rows[row].Cells[1].Text;
            mtxtBranch.Text = gvBankAccount.Rows[row].Cells[2].Text;
            mtxtAccount.Text = gvBankAccount.Rows[row].Cells[3].Text;
            mtxtAccountNo.Text = gvBankAccount.Rows[row].Cells[4].Text;

            //2023-06-17 : FIX FOR UPDATING OF BANK
            //mtxtRow2.Text = row.ToString();
            mtxtRow2.Text = gvBankAccount.Rows[row].Cells[0].Text;



            banktitle.InnerText = "Update Bank Account";
            btnAddBank.Visible = false;
            btnUpdateBank.Visible = true;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowAddDependent2();", true);
        }
        protected void btnUpdateBank_ServerClick(object sender, EventArgs e)
        {
            int row = int.Parse(mtxtRow2.Text);
            //gvBankAccount.Rows[row].Cells[0].Text = mtxtBankName.Text.ToUpper();
            //gvBankAccount.Rows[row].Cells[1].Text = mtxtBranch.Text.ToUpper();
            //gvBankAccount.Rows[row].Cells[2].Text = mtxtAccount.Text.ToUpper();
            //gvBankAccount.Rows[row].Cells[3].Text = mtxtAccountNo.Text;
            //gvBankAccount.Rows[row].Cells[4].Text = mtxtRow2.Text;

            DataTable dt = (DataTable)ViewState["BankAccount"];

            //clear Session
            ViewState["BankAccount"] = null;


            foreach (DataRow dr in dt.Rows)
            {
                //update row of datatable
                if (int.Parse(dr["Id"].ToString()) == row)
                {
                    dr["Bank"] = mtxtBankName.Text.Trim().ToUpper();
                    dr["Branch"] = mtxtBranch.Text.Trim().ToUpper();
                    dr["AcctType"] = mtxtAccount.Text;
                    dr["AcctNo"] = mtxtAccountNo.Text;
                }
            }

            ViewState["BankAccount"] = dt;

            //REFRESH GRIDVIEW
            gvBankAccount.DataSource = dt;
            gvBankAccount.DataBind();

            ClearAllModalFields();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideAddDependent2();", true);
        }
        protected void btnRefShow_Click(object sender, EventArgs e)
        {
            reftitle.InnerText = "New Character Reference";
            btnAddCharRef.Visible = true;
            btnRefUpdate.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowCharacterReference();", true);
        }
        protected void btnRefUpdateShow_Click(object sender, EventArgs e)
        {

            LinkButton GetID = (LinkButton)sender;
            int row = int.Parse(GetID.CommandArgument.ToString());

            reftxtName.Value = gvCharacter.Rows[row].Cells[1].Text;
            reftxtAddress.Value = System.Net.WebUtility.HtmlDecode(gvCharacter.Rows[row].Cells[2].Text);
            reftxtEmail.Value = System.Net.WebUtility.HtmlDecode(gvCharacter.Rows[row].Cells[3].Text);
            reftxtContact.Value = gvCharacter.Rows[row].Cells[4].Text;


            //2023-06-17: FIX FOR UPDATING CHARACTER REF
            //mtxtRow3.Value = row.ToString();
            mtxtRow3.Value = gvCharacter.Rows[row].Cells[0].Text;

            reftitle.InnerText = "Update Character Reference";

            btnAddCharRef.Visible = false;
            btnRefUpdate.Visible = true;



            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "ShowCharacterReference();", true);

        }
        protected void btnRefUpdate_ServerClick(object sender, EventArgs e)
        {

            if (reftxtName.Value == "" || reftxtAddress.Value == "" || reftxtEmail.Value == "")
            { alertMsg2("Please fill up all forms before adding!", "info"); }
            else
            {
                int row = int.Parse(mtxtRow3.Value);

                //2023-06-17: FIX UPDATING OF CHAR REF
                //gvCharacter.Rows[row].Cells[0].Text = reftxtName.Value.ToUpper();
                //gvCharacter.Rows[row].Cells[1].Text = reftxtAddress.Value.ToUpper();
                //gvCharacter.Rows[row].Cells[2].Text = reftxtEmail.Value;
                //gvCharacter.Rows[row].Cells[3].Text = reftxtContact.Value;


                DataTable dt = (DataTable)ViewState["CharacterRef"];

                //clear Session
                ViewState["CharacterRef"] = null;


                foreach (DataRow dr in dt.Rows)
                {
                    //update row of datatable
                    if (int.Parse(dr["Id"].ToString()) == row)
                    {
                        dr["Name"] = reftxtName.Value.Trim().ToUpper();
                        dr["Address"] = reftxtAddress.Value.Trim().ToUpper();
                        dr["Email"] = reftxtEmail.Value;
                        dr["TelNo"] = reftxtContact.Value;
                    }
                }

                ViewState["CharacterRef"] = dt;

                //REFRESH GRIDVIEW
                gvCharacter.DataSource = dt;
                gvCharacter.DataBind();




                ClearAllModalFields();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "HideCharacterReference();", true);
            }
        }

        protected void Reload_ServerClick(object sender, EventArgs e)
        {
            if (lblMessageAlert.Text != "Data from Spouse tab copied to SPA/Co-borrower tab.")
            {
                Response.Redirect("~/pages/BuyerInformation.aspx");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "clickTab", "clickTab();", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "HideAlert();", true);
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

            ScriptManager.RegisterStartupScript(this, GetType(), "Show", "showMsgAccount();", true);
        }

        protected void bSelect_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string Code = GetID.CommandArgument;

            mtxtAccount.Text = Code;

            ScriptManager.RegisterStartupScript(this, GetType(), "MsgAccount", "hideMsgAccount();", true);
        }
        //protected void txtRented_OnKeyup(object sender, EventArgs e)
        //{
        //    txtRented.Text = SystemClass.ToCurrency(txtRented.Text);
        //    txtRented.Enabled = true;
        //}

        //protected void rbpsolo_Click(object sender, EventArgs e)
        //{
        //    gensolo.Visible = true;
        //    gencorp.Visible = false;
        //}

        //protected void rbpcorp_Click(object sender, EventArgs e)
        //{
        //    gensolo.Visible = false;
        //    gencorp.Visible = true;
        //}

        void loadDivisionsForNames(string type)
        {
            DataTable taxclass = new DataTable();
            taxclass = (DataTable)ViewState["TaxClass"];
            taxclass.Rows.Clear();
            if (type == "Corporation")
            {
                taxclass.Rows.Add("Corporation", "Corporation");

            }
            else
            {
                taxclass.Rows.Add("Engaged in business", "Engaged in business");
                taxclass.Rows.Add("Not engaged in business", "Not engaged in business");
            }
            ddTaxClass.DataSource = taxclass;
            ddTaxClass.DataBind();
        }
        protected void ddlBusinessType_SelectedIndexChanged(object sender, EventArgs e)
        {
            rbGuardian.Checked = false;
            rbGuardee.Checked = false;
            if (ddCivilStatus.Text == "CS2" && ddlBusinessType.Text != "Corporation")
            {
                spouseBtn.Visible = true;
                btnCopySPA.Visible = true;
                sBuyerType.Visible = false;
                RequiredFieldValidator8.Enabled = true;
                RequiredFieldValidator9.Enabled = true;
                RequiredFieldValidator10.Enabled = true;
                RequiredFieldValidator51.Enabled = false;
                //txtSpecifyBusiness.Text = "";
            }
            if (ddlBusinessType.Text == "Corporation")
            {
                suppTitle.InnerText = "Authorized Person Details";

                txtLastName.Text = "";
                txtFirstName.Text = "";
                txtMiddleName.Text = "";
                txtLastName2.Text = "";
                txtFirstName2.Text = "";
                txtMiddleName2.Text = "";

                gensolo.Visible = false;
                gencorp.Visible = true;
                empDetails.Visible = false;
                RequiredFieldValidator1.Enabled = false;
                RequiredFieldValidator28.Enabled = true;
                spouseBtn.Visible = false;
                btnCopySPA.Visible = false;
                RequiredFieldValidator8.Enabled = false;
                RequiredFieldValidator9.Enabled = false;
                RequiredFieldValidator10.Enabled = false;
                empDependents.Visible = false;
                compDetails.Visible = true;
                sBuyerType.Visible = false;
                RequiredFieldValidator51.Enabled = false;
                txtSpecifyBusiness.Text = "";
                //RequiredFieldValidator45.Enabled = true;
                //RequiredFieldValidator46.Enabled = true;
                //RequiredFieldValidator47.Enabled = true;
                //RequiredFieldValidator48.Enabled = true;
                //RequiredFieldValidator49.Enabled = true;
                //RequiredFieldValidator50.Enabled = true;

                coborrowerGrid.Visible = false;
                contactDetails.Visible = false;
                othersGrid.Visible = false;

                RequiredFieldValidator35.Enabled = true;
                RequiredFieldValidator36.Enabled = true;
                RequiredFieldValidator37.Enabled = true;

                RequiredFieldValidator7.Enabled = false;
                CustomValidator1.Enabled = false;

                RequiredFieldValidator43.Enabled = true;
                CustomValidator7.Enabled = true;

                tinhide.Visible = false;

                ConformeCorp.Visible = true;

                RequiredFieldValidator63.Enabled = true;

                divnoncorp.Visible = false;
                divnoncorp2.Visible = false;
                divnoncorp3.Visible = false;
                RequiredFieldValidator34.Enabled = false;

                divTrustGuard.Visible = false;
                CustomValidator9.Enabled = false;

                HidePermAddress(false);

                SpecialBuyersValidation(false);


                //2023-06-14 : MAKE VALID ID 1 DEFAULT TO TIN FOR CORPORATION   
                ddTypeOfID.SelectedValue = "ID1";
                txtIDNumber.Text = "";


            }
            else
            {
                suppTitle.InnerText = "Supplementary Details";

                txtCorpName.Text = "";
                txtSpecifyBusiness.Text = "";
                gensolo.Visible = true;
                gencorp.Visible = false;
                empDetails.Visible = true;
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator28.Enabled = false;
                empDependents.Visible = true;
                compDetails.Visible = false;

                //RequiredFieldValidator45.Enabled = false;
                //RequiredFieldValidator46.Enabled = false;
                //RequiredFieldValidator47.Enabled = false;
                //RequiredFieldValidator48.Enabled = false;
                //RequiredFieldValidator49.Enabled = false;
                //RequiredFieldValidator50.Enabled = false;

                RequiredFieldValidator35.Enabled = false;
                RequiredFieldValidator36.Enabled = false;
                RequiredFieldValidator37.Enabled = false;

                if (ddlBusinessType.Text == "Guardianship")
                {
                    //SpecialBuyersValidation(true);
                    //coborrowerGrid.Visible = false;
                    //contactDetails.Visible = true;
                    //contactTitle.InnerText = "Guardian Details";
                    sBuyerType.Visible = false;
                    RequiredFieldValidator51.Enabled = false;
                    txtSpecifyBusiness.Text = "";
                    //othersGrid.Visible = false;

                    CustomValidator4.Enabled = false;
                    CustomValidator5.Enabled = false;
                    CustomValidator6.Enabled = false;

                    divTrustGuard.Visible = true;
                    CustomValidator9.Enabled = true;
                    rbGuardian.Text = "Guardian";
                    rbGuardee.Text = "Guardee";
                }
                else if (ddlBusinessType.Text == "Trusteeship")
                {
                    //SpecialBuyersValidation(true);
                    //coborrowerGrid.Visible = false;
                    //contactDetails.Visible = true;
                    //contactTitle.InnerText = "Trustee Details";
                    sBuyerType.Visible = false;
                    RequiredFieldValidator51.Enabled = false;
                    txtSpecifyBusiness.Text = "";
                    //othersGrid.Visible = false;

                    CustomValidator4.Enabled = false;
                    CustomValidator5.Enabled = false;
                    CustomValidator6.Enabled = false;

                    divTrustGuard.Visible = true;
                    CustomValidator9.Enabled = true;
                    rbGuardian.Text = "Trustor";
                    rbGuardee.Text = "Trustee";
                }
                else if (ddlBusinessType.Text == "Co-ownership")
                {
                    //SpecialBuyersValidation(true);
                    //coborrowerGrid.Visible = true;
                    //contactDetails.Visible = false;
                    //othersGrid.Visible = false;
                    sBuyerType.Visible = false;
                    RequiredFieldValidator51.Enabled = false;
                    txtSpecifyBusiness.Text = "";
                    coother.InnerText = "Add Co-Owner";

                    CustomValidator4.Enabled = true;
                    CustomValidator5.Enabled = true;
                    CustomValidator6.Enabled = true;

                    divTrustGuard.Visible = false;
                    CustomValidator9.Enabled = false;
                }
                else if (ddlBusinessType.Text == "Others")
                {
                    //SpecialBuyersValidation(true);
                    //coborrowerGrid.Visible = false;
                    //contactDetails.Visible = false;
                    //othersGrid.Visible = true;
                    sBuyerType.Visible = true;
                    RequiredFieldValidator51.Enabled = true;
                    coother.InnerText = "Add Co-Buyer";

                    CustomValidator4.Enabled = true;
                    CustomValidator5.Enabled = true;
                    CustomValidator6.Enabled = true;

                    divTrustGuard.Visible = false;
                    CustomValidator9.Enabled = false;
                }
                else
                {
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

                RequiredFieldValidator7.Enabled = true;
                CustomValidator1.Enabled = true;

                RequiredFieldValidator43.Enabled = false;
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
                RequiredFieldValidator34.Enabled = true;

                HidePermAddress(true);
            }
            taxClassChanged();
            loadDivisionsForNames(ddlBusinessType.Text);

        }

        //protected bool addinfo()
        //{
        //    try
        //    {


        //        //if (rb1.Checked == true)
        //        //{
        //        //    _ownership = "Owned";
        //        //}
        //        //else if (rb2.Checked == true)
        //        //{
        //        //    _ownership = "Mortgaged";
        //        //}
        //        //else if (rb3.Checked == true)
        //        //{
        //        //    _ownership = "Living w/ Relatives";
        //        //}
        //        //else
        //        //{
        //        //    _ownership = "Rented";
        //        //}

        //        int countfail = 0;


        //        //--""HomeOwnership"" = '{_ownership}',
        //        //--""YearsStay"" = '{_yrAddrStayPre}',
        //        //--""HomeOwnerValue"" = '{_ownval.Replace(",", "")}',

        //        if () == false)
        //        {
        //            countfail += 1;
        //        }
        //        else
        //        {


        //            if ( == false) countfail += 1;


        //            ////--Remove first before save--////
        //            if ( == false) countfail += 1;

        //            //string qryCRD5 = $@"INSERT INTO CRD5                                    
        //            //                (
        //            //                ""CardCode"" ,
        //            //                ""SPA"" ,
        //            //                ""CoBorrower"" ,
        //            //                ""LastName"" ,
        //            //                ""FirstName"" ,
        //            //                ""MiddleName"" ,
        //            //                ""Gender"" ,
        //            //                ""Citizenship"" ,
        //            //                ""BirthDay"" ,
        //            //                ""BirthPlace"" ,
        //            //                ""CellNo"" ,
        //            //                ""EmailAddress"" ,
        //            //                ""FBAccount"" ,
        //            //                ""TIN"" ,
        //            //                ""SSSNo"" , 
        //            //                ""GSISNo"" ,
        //            //                ""PagibiNo"" ,
        //            //                ""Position"" ,
        //            //                ""YearsService"" , 
        //            //                ""OfficeTelNo"" ,
        //            //                ""FaxNo"" ,
        //            //                ""EmpBusName"" ,
        //            //                ""EmpBusAdd"" ,
        //            //                ""EmpStatus"" ,
        //            //                ""NatureEmp"" ,
        //            //                ""SPAHomeTelNo"" ,     
        //            //                ""SPAYearsStay"",
        //            //                ""SPABusCountry"",
        //            //                ""SPAPresentAddress"",
        //            //                ""Relationship""
        //            //                )
        //            //                VALUES ( 
        //            //                 '{CardCode}'
        //            //                , {SPA}
        //            //                , {CoBorrower}
        //            //                , '{txtSPALastName.Text}'
        //            //                , '{txtSPAFirstName.Text}'
        //            //                , '{txtSPAMiddleName.Text}'
        //            //                , '{_ddSPAGender}'
        //            //                , '{txtSPACitizenship.Value}'
        //            //                , '{(dtSPABirthDate.Text == "" ? DateTime.MinValue.ToString("yyyy-MM-dd") : Convert.ToDateTime(dtSPABirthDate.Text).ToString("yyyy-MM-dd"))}'
        //            //                , '{txtSPABirthPlace.Text}'
        //            //                , '{txtSPAMobile.Text}'
        //            //                , '{txtSPAEmail.Text}'
        //            //                , '{txtSPAFB.Text}'
        //            //                , '{txtSPATinNo.Text}'
        //            //                , '{txtSPASSSNo.Text}'
        //            //                , '{txtSPAGSIS.Text}'
        //            //                , '{txtSPAPagibi.Text}'
        //            //                , '{txtSPAPosition.Text}'
        //            //                , '{(_spayrserv == "" ? 0.00 : Convert.ToDouble(_spayrserv))}'
        //            //                , '{txtSPAOfcTelNo.Text}'
        //            //                , '{txtSPAFaxNo.Text}'
        //            //                , '{txtSPABusName.Text}'
        //            //                , '{txtSPABusAdd.Text}'
        //            //                , '{_ddSPAEmpStat}'
        //            //                , '{_ddSPAEmpNatu}'
        //            //                , '{txtSPATelNo.Text}'
        //            //                , '{(_spayrserv == "" ? 0.00 : Convert.ToDouble(_spayrserv))}'
        //            //                , '{_ddSPABusCountry}'
        //            //                , '{txtSPAAddress.Text}'
        //            //                , '{tRelationship}')
        //            //                ";
        //            ////--Remove first before save--////
        //            //hana.Execute($@"DELETE FROM CRD5 WHERE LCASE(""CardCode"") = '{CardCode.ToLower()}' AND ""SPA""='true' ; ", hana.GetConnection("SAOHana"));
        //            //if (hana.Execute(qryCRD5, hana.GetConnection("SAOHana")) == false) countfail += 1;










        //            //if (ddlBusinessType.Text == "Guardianship" || ddlBusinessType.Text == "Trusteeship")
        //            //{
        //            //    string qryCRD7 = $@"INSERT INTO CRD7
        //            //                (
        //            //                ""CardCode"",
        //            //                ""BuyerType"",
        //            //                ""FirstName"",
        //            //                ""MiddleName"",
        //            //                ""LastName"",
        //            //                ""Relationship"",
        //            //                ""Email"",
        //            //                ""MobileNo"",
        //            //                ""Address"",
        //            //                ""Residence"",
        //            //                ""ValidID"",
        //            //                ""ValidIDNo"",
        //            //                ""CreateDate"")
        //            //                VALUES(
        //            //                '{CardCode}'
        //            //                , '{ddlBusinessType.Text}'
        //            //                , '{txtContactFName.Text}'
        //            //                , '{txtContactMName.Text}'
        //            //                , '{txtContactLName.Text}'
        //            //                , '{txtContactPersonPosition.Text}'
        //            //                , '{txtContactEmail.Text}'
        //            //                , '{txtContactMobile.Text}'
        //            //                , '{txtContactAddress.Text}'
        //            //                , '{txtContactResidence.Text}'
        //            //                , '{ddContactValidID.SelectedValue}'
        //            //                , '{txtContactValidIDNo.Text}'
        //            //                , '{DateTime.Now.ToString("yyyy-MM-dd")}'
        //            //                )";
        //            //    if (hana.Execute(qryCRD7, hana.GetConnection("SAOHana")) == false)
        //            //    {
        //            //        countfail += 1;
        //            //    }
        //            //}
        //            //else if (ddlBusinessType.Text == "Co-ownership" || ddlBusinessType.Text == "Others")
        //            //{
        //            //    dt = (DataTable)Session["dtCoOwner"];
        //            //    foreach (DataRow row in dt.Rows)
        //            //    {
        //            //        if (row.RowState != DataRowState.Deleted)
        //            //        {
        //            //            string qryCRD7 = $@"INSERT INTO CRD7
        //            //                (
        //            //                ""CardCode"",
        //            //                ""BuyerType"",
        //            //                ""FirstName"",
        //            //                ""MiddleName"",
        //            //                ""LastName"",
        //            //                ""Relationship"",
        //            //                ""Email"",
        //            //                ""MobileNo"",
        //            //                ""Address"",
        //            //                ""Residence"",
        //            //                ""ValidID"",
        //            //                ""ValidIDNo"",
        //            //                ""CreateDate"")
        //            //                VALUES(
        //            //                '{CardCode}'
        //            //                , '{ddlBusinessType.Text}'
        //            //                , '{row["FirstName"].ToString()}'
        //            //                , '{row["MiddleName"].ToString()}'
        //            //                , '{row["LastName"].ToString()}'
        //            //                , '{row["Relationship"].ToString()}'
        //            //                , '{row["Email"].ToString()}'
        //            //                , '{row["MobileNo"].ToString()}'
        //            //                , '{row["Address"].ToString()}'
        //            //                , '{row["Residence"].ToString()}'
        //            //                , '{(row["ValidID"].ToString() == "---Select Valid ID---" ? "" : row["ValidID"].ToString())}'
        //            //                , '{row["ValidIDNo"].ToString()}'
        //            //                , '{DateTime.Now.ToString("yyyy-MM-dd")}'
        //            //                )";
        //            //            if (hana.Execute(qryCRD7, hana.GetConnection("SAOHana")) == false)
        //            //            {
        //            //                countfail += 1;
        //            //                break;
        //            //            }
        //            //        }
        //            //    }
        //            //}
        //        }
        //        if (countfail > 0)
        //        {
        //            return false;
        //        }
        //        else
        //        {

        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //ViewState["errmsg"] = ex.Message;
        //        return false;
        //    }

        //}

        protected void ddCivilStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddCivilStatus.Text == "CS2" && ddlBusinessType.Text != "Corporation")
            {
                spouseBtn.Visible = true;
                btnCopySPA.Visible = true;
                RequiredFieldValidator8.Enabled = true;
                RequiredFieldValidator9.Enabled = true;
                RequiredFieldValidator10.Enabled = true;
            }
            else
            {
                spouseBtn.Visible = false;
                btnCopySPA.Visible = false;
                RequiredFieldValidator8.Enabled = false;
                RequiredFieldValidator9.Enabled = false;
                RequiredFieldValidator10.Enabled = false;
            }
        }

        protected void ddSourceFunds_SelectedIndexChanged(object sender, EventArgs e)
        {
            otherFields();
        }

        protected void btnCopySPA_Click(object sender, EventArgs e)
        {
            try
            {

                //Spouse Details
                txtSPALastName.Text = txtSPOLastName.Text;
                txtSPAFirstName.Text = txtSPOFirstName.Text;
                txtSPAMiddleName.Text = txtSPOMiddleName.Text;
                txtSPAAddress.Text = txtSPOAddress.Text;
                dtSPABirthDate.Text = dtSPOBirthDate.Text;
                txtSPABirthPlace.Text = txtSPOBirthPlace.Text;
                ddSPAGender.Text = ddSPOGender.Text;
                txtSPACitizenship.SelectedIndex = txtSPOCitizenShip.SelectedIndex;
                txtSPAEmail.Text = txtSPOEmail.Text;
                //txtSPATelNo.Text = txtSPOTelNo.Text;
                txtSPAMobile.Text = txtSPOMobile.Text;
                txtSPAFB.Text = txtSPOFB.Text;

                //Spouse Business Details
                ddSPAEmpStat.SelectedValue = ddSPOEmpStat.SelectedValue;
                ddSPANatureEmp.SelectedValue = ddSPONatureEmp.SelectedValue;
                txtSPABusName.Text = txtSPOBusName.Text;
                txtSPABusAdd.Text = txtSPOBusAdd.Text;
                ddSPABusCountry.SelectedValue = ddSPOBusCountry.SelectedValue;
                txtSPAPosition.Text = txtSPOPosition.Text;
                txtSPAYearsService.Text = txtSPOYearsService.Text;
                txtSPAOfcTelNo.Text = txtSPOOfcTelNo.Text;
                txtSPAFaxNo.Text = txtSPOFaxNo.Text;
                txtSPATinNo.Text = txtSPOTinNo.Text;
                txtSPASSSNo.Text = txtSPOSSSNo.Text;
                txtSPAGSIS.Text = txtSPOGSIS.Text;
                txtSPAPagibi.Text = txtSPOPagibi.Text;

                alertMsg2("Data from Spouse tab copied to SPA/Co-borrower tab.", "success");
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);
            }
            catch (Exception ex)
            {
                alertMsg2(ex.Message, "error");
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);
            }



        }

        protected void btnEmployment_ServerClick(object sender, EventArgs e)
        {
            Control btn = (Control)sender;
            string id = btn.ID;
            string txt = "Choose ";
            string GrpCode = "";

            if (id == "bSpecialInstructions")
            { txt += "Special Instructions"; }
            else if (id == "bCoBorrower")
            { txt += "Relationship"; GrpCode = "RE"; }
            else if (id == "bDependentRelationship")
            { txt += "Relationship"; GrpCode = "RE"; }
            else if (id == "bDependentRelationship2")
            { txt += "Relationship"; GrpCode = "RE"; }

            ChooseEmployment.InnerText = txt;

            if (txt == "Choose Special Instructions")
            {
                gvEmployment.DataSource = hana.GetData($@"Select 'Home Address' as ""Code"", 'Home Address' as ""Name"" from ""DUMMY"" union all select 'Business Address' as ""Code"", 'Business Address' as ""Name"" from ""DUMMY"" union all Select 'OTH' as ""Code"", 'Others' as ""Name"" from ""DUMMY""", hana.GetConnection("SAOHana"));
                gvEmployment.DataBind();
            }
            else
            {
                //gvEmployment.DataSource = hana.GetData($@"SELECT ""Code"", ""Name"" FROM ""OLST"" WHERE ""GrpCode"" = '{GrpCode}'", hana.GetConnection("SAOHana"));
                gvEmployment.DataSource = hana.GetData($@"(SELECT ""Code"", ""Name"" FROM ""OLST"" WHERE ""GrpCode"" = '{GrpCode}' AND ""Code"" <> 'OTH' ORDER BY ""Code"") UNION ALL SELECT ""Code"", ""Name"" FROM ""OLST"" WHERE ""GrpCode"" = '{GrpCode}' AND ""Code"" = 'OTH'", hana.GetConnection("SAOHana"));
                gvEmployment.DataBind();
            }


            Session["btnID"] = id;

            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "Show", "MsgEmployment_Show();", true);
        }

        protected void btnSelectEmployment_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string Code = GetID.CommandArgument;

            string GrpCode = (string)Session["btnID"];

            if (GrpCode == "bSpecialInstructions")
            {
                Session["SpecialInstructions"] = Code;
                if (Code == "OTH")
                { tSpecialInstructions.Disabled = false; tSpecialInstructions.Value = ""; }
                else { tSpecialInstructions.Disabled = true; tSpecialInstructions.Value = Code; }

            }
            else if (GrpCode == "bCoBorrower")
            {
                tRelationship.Value = ws.GetOLSTName(Code);
                Session["tRelationship"] = Code;
                if (Code == "OTH")
                { tRelationship.Disabled = false; tRelationship.Value = ""; }
                else { tRelationship.Disabled = true; }
            }
            else if (GrpCode == "bDependentRelationship")
            {
                tDependentRelationship.Value = ws.GetOLSTName(Code);
                Session["tDependentRelationship"] = Code;
                if (Code == "OTH")
                { tDependentRelationship.Disabled = false; tDependentRelationship.Value = ""; }
                else { tDependentRelationship.Disabled = true; }
            }
            else if (GrpCode == "bDependentRelationship2")
            {
                mtxtRelationship.Value = ws.GetOLSTName(Code);
                Session["tDependentRelationship2"] = Code;
                if (Code == "OTH")
                { mtxtRelationship.Disabled = false; mtxtRelationship.Value = ""; }
                else { mtxtRelationship.Disabled = true; }
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "Hide", "MsgEmployment_Hide();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);
        }

        //FOR TIN
        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //BLOCK ADDING WHEN TIN ALREADY EXIST
            //2023-10-25 : FILTER ONLY PER BUYER, EXCLUDE SPOUSE
            //string qry = $@"SELECT ""CardCode"" FROM ""CRD1"" where ""TIN"" = '{txtTIN.Text}'";
            string qry = $@"SELECT ""CardCode"" FROM ""CRD1"" where ""TIN"" = '{txtTIN.Text}' AND ""CardType"" = 'Buyer'";

            DataTable dt = hana.GetData(qry, hana.GetConnection("SAOHana"));

            if (dt.Rows.Count == 0)
            {
                //Check if TIN is in correct format(###-###-###-###)
                bool isOK = Regex.IsMatch(txtTIN.Text, @"[0-9][0-9][0-9]\-[0-9][0-9][0-9]\-[0-9][0-9][0-9]\-[0-9][0-9][0-9]");
                if (!isOK)
                {
                    CustomValidator1.Text = "Incorrect TIN format. Must be xxx-xxx-xxx-xxx.";
                    RequiredFieldValidator7.Visible = false;
                    args.IsValid = false;
                }
                else
                {
                    RequiredFieldValidator7.Visible = true;
                    args.IsValid = true;
                }
            }
            else
            {
                CustomValidator1.Text = "TIN already exists!";
                RequiredFieldValidator7.Visible = false;
                args.IsValid = false;
            }
        }

        protected void btnSPACoBorrower_ServerClick(object sender, EventArgs e)
        {

            if (Page.IsValid)
            {
                DataTable dt = (DataTable)ViewState["SPA"];
                dt.Rows.Add(
                    Convert.ToInt32(dt.Rows.Count + 1),
                    tRelationship.Value.Trim().ToUpper(),
                    txtSPALastName.Text.Trim().ToUpper(),
                    txtSPAFirstName.Text.Trim().ToUpper(),
                    txtSPAMiddleName.Text.Trim().ToUpper(),
                    ddSPACivilStatus.Text.Trim().ToUpper(),
                    string.IsNullOrEmpty(tSPAYearsOfStay.Value) ? string.Empty : tSPAYearsOfStay.Value.Trim(),
                    string.IsNullOrEmpty(txtSPAAddress.Text) ? string.Empty : txtSPAAddress.Text.Trim().ToUpper(),
                    string.IsNullOrEmpty(dtSPABirthDate.Text) ? string.Empty : dtSPABirthDate.Text.Trim(),
                    string.IsNullOrEmpty(txtSPABirthPlace.Text) ? string.Empty : txtSPABirthPlace.Text.Trim(),
                    ddSPAGender.SelectedValue,
                    txtSPACitizenship.Value,
                    string.IsNullOrEmpty(txtSPAEmail.Text) ? string.Empty : txtSPAEmail.Text.Trim(),
                    string.IsNullOrEmpty(txtSPATelNo.Text) ? string.Empty : txtSPATelNo.Text.Trim(),
                    string.IsNullOrEmpty(txtSPAMobile.Text) ? string.Empty : txtSPAMobile.Text.Trim(),
                    string.IsNullOrEmpty(txtSPAFB.Text) ? string.Empty : txtSPAFB.Text.Trim(),
                    lblSPAFileName.Text.Trim()
                    );
                ViewState["SPA"] = dt;
                this.BindSPA();

                clearSPADetails();


                //Session["ExtBuyerRow"] = "1";
                //Session["ExtGv"] = "";

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "MsgSPACoBorrower_Hide();", true);

                //2023-06-16 : COMMENTED FOR SPA CHANGES
                //confirmation("Are you sure you want to add SPA / Co Borrower?", "AddSPACoBorrower");
                //ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);
            }



            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);

        }



        void clearSPADetails()
        {
            txtSPAID.Value = string.Empty;
            tRelationship.Value = string.Empty;
            txtSPALastName.Text = string.Empty;
            txtSPAFirstName.Text = string.Empty;
            txtSPAMiddleName.Text = string.Empty;
            ddSPACivilStatus.Text = string.Empty;
            tSPAYearsOfStay.Value = string.Empty;
            txtSPAAddress.Text = string.Empty;
            dtSPABirthDate.Text = string.Empty;
            txtSPABirthPlace.Text = string.Empty;
            ddSPAGender.SelectedIndex = 0;
            txtSPACitizenship.SelectedIndex = 0;
            txtSPAEmail.Text = string.Empty;
            txtSPATelNo.Text = string.Empty;
            txtSPAMobile.Text = string.Empty;
            txtSPAFB.Text = string.Empty;
            lblSPAFileName.Text = string.Empty;
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

        void LoadData(GridView gv, string session)
        {
            gv.DataSource = (DataTable)Session[session];
            gv.DataBind();
        }

        protected void gvSPACoBorrower_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadData(gvSPACoBorrower, "gvSPACoBorrower");
            gvSPACoBorrower.PageIndex = e.NewPageIndex;
            gvSPACoBorrower.PageIndex = e.NewPageIndex;
            gvSPACoBorrower.DataBind();
        }

        protected void gvSPACBEdit_Click(object sender, EventArgs e)
        {

            try
            {


                LinkButton GetID = (LinkButton)sender;
                int row = int.Parse(GetID.CommandArgument.ToString());

                txtSPAID.Value = gvSPACoBorrower.Rows[row].Cells[0].Text;

                tRelationship.Value = gvSPACoBorrower.Rows[row].Cells[1].Text;
                txtSPALastName.Text = gvSPACoBorrower.Rows[row].Cells[2].Text;
                txtSPAFirstName.Text = gvSPACoBorrower.Rows[row].Cells[3].Text;
                txtSPAMiddleName.Text = gvSPACoBorrower.Rows[row].Cells[4].Text;
                ddSPACivilStatus.Text = gvSPACoBorrower.Rows[row].Cells[5].Text;
                tSPAYearsOfStay.Value = string.IsNullOrEmpty(gvSPACoBorrower.Rows[row].Cells[6].Text) ? "0" : gvSPACoBorrower.Rows[row].Cells[6].Text;
                txtSPAAddress.Text = string.IsNullOrEmpty(gvSPACoBorrower.Rows[row].Cells[7].Text) ? string.Empty : gvSPACoBorrower.Rows[row].Cells[7].Text;
                dtSPABirthDate.Text = string.IsNullOrEmpty(gvSPACoBorrower.Rows[row].Cells[8].Text) ? string.Empty : Convert.ToDateTime(gvSPACoBorrower.Rows[row].Cells[8].Text).ToString("yyyy-MM-dd");
                txtSPABirthPlace.Text = string.IsNullOrEmpty(gvSPACoBorrower.Rows[row].Cells[9].Text) ? string.Empty : gvSPACoBorrower.Rows[row].Cells[9].Text;
                ddSPAGender.SelectedValue = gvSPACoBorrower.Rows[row].Cells[10].Text;
                txtSPACitizenship.Value = gvSPACoBorrower.Rows[row].Cells[11].Text;
                txtSPAEmail.Text = string.IsNullOrEmpty(gvSPACoBorrower.Rows[row].Cells[12].Text) ? string.Empty : gvSPACoBorrower.Rows[row].Cells[12].Text;
                txtSPATelNo.Text = string.IsNullOrEmpty(gvSPACoBorrower.Rows[row].Cells[13].Text) ? string.Empty : gvSPACoBorrower.Rows[row].Cells[13].Text;
                txtSPAMobile.Text = string.IsNullOrEmpty(gvSPACoBorrower.Rows[row].Cells[14].Text) ? string.Empty : gvSPACoBorrower.Rows[row].Cells[14].Text;
                txtSPAFB.Text = string.IsNullOrEmpty(gvSPACoBorrower.Rows[row].Cells[15].Text) ? string.Empty : gvSPACoBorrower.Rows[row].Cells[15].Text;

                lblSPAFileName.Text = string.Empty;
                visibleDocumentButtons(false, btnSPAPreview, btnSPARemove);

                lblSPAFileName.Text = string.IsNullOrEmpty(gvSPACoBorrower.Rows[row].Cells[16].Text) ? string.Empty : gvSPACoBorrower.Rows[row].Cells[16].Text;

                spaTitle.InnerText = "Update Character Reference";

                btnSPACoBorrower.Visible = false;
                btnSPAUpdate.Visible = true;

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "MsgSPACoBorrower_Show();", true);



                //2023-06-18 : VISIBLE FIELDS
                if (!string.IsNullOrEmpty(lblSPAFileName.Text))
                {
                    visibleDocumentButtons(true, btnSPAPreview, btnSPARemove);
                }
                else
                {
                    visibleDocumentButtons(false, btnSPAPreview, btnSPARemove);
                }


                //2023-06-18 : COMMENTED FOR NEW SPA PROCESS 

                #region OLD PROCESS
                //DataTable dt = new DataTable();

                //LinkButton GetID = (LinkButton)sender;
                //int ID = Convert.ToInt32(GetID.CommandArgument);
                //Session["SPACoBorrowerCount"] = ID;
                //SPAAddUpdate.InnerText = "Update";

                //dt = hana.GetDataDS($@"SELECT * FROM ""temp_CRD5"" WHERE ""UserID"" = {(int)Session["UserID"]} AND ""ID"" = '{ID}'", hana.GetConnection("SAOHana")).Tables[0];

                //for (int i = dt.Rows.Count - 1; i >= 0; i--)
                //{
                //    DataRow dr = dt.Rows[i];
                //    if (Convert.ToInt32(dr["ID"]) == ID)
                //    {


                //        divUploadSPADocs.Visible = true;


                //        tRelationship.Value = dr["Relationship"].ToString();
                //        txtSPALastName.Text = dr["LastName"].ToString();
                //        txtSPAFirstName.Text = dr["FirstName"].ToString();
                //        txtSPAMiddleName.Text = dr["MiddleName"].ToString();
                //        ddSPAGender.SelectedValue = dr["Gender"].ToString();
                //        txtSPACitizenship.Value = dr["Citizenship"].ToString();

                //        string bdate = dr["BirthDate"].ToString();
                //        if (!string.IsNullOrEmpty(bdate))
                //        {
                //            DateTime oBDAY;
                //            oBDAY = DateTime.Parse(bdate);
                //            bdate = oBDAY.ToString("yyyy-MM-dd");
                //        }

                //        dtSPABirthDate.Text = bdate;
                //        txtSPABirthPlace.Text = dr["BirthPlace"].ToString();
                //        txtSPAMobile.Text = dr["CellNo"].ToString();
                //        txtSPATelNo.Text = dr["HomeTelNo"].ToString();
                //        txtSPAEmail.Text = dr["Email"].ToString();
                //        txtSPAFB.Text = dr["FB"].ToString();
                //        txtSPATinNo.Text = dr["TIN"].ToString();
                //        txtSPASSSNo.Text = dr["SSSNo"].ToString();
                //        txtSPAGSIS.Text = dr["GSISNo"].ToString();
                //        txtSPAPagibi.Text = dr["PagIbigNo"].ToString();
                //        //ddPreCountry.SelectedValue = string.IsNullOrWhiteSpace(dr["PresentAddress"].ToString()) ? "" : dr["PresentAddress"].ToString();
                //        //ddPermCountry.SelectedValue = dr["PermanentAddress"].ToString();
                //        txtSPAPosition.Text = dr["Position"].ToString();
                //        txtSPAYearsService.Text = Math.Round(decimal.Parse(dr["YearsOfService"].ToString())).ToString();
                //        txtSPAOfcTelNo.Text = dr["OfficeTelNo"].ToString();
                //        txtSPAFaxNo.Text = dr["FaxNo"].ToString();

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

                //        txtSPAYearsService.Text = Math.Round(decimal.Parse(dr["YearsOfStay"].ToString())).ToString();

                //        txtSPABusName.Text = dr["EmpBusinessName"].ToString();
                //        txtSPABusAdd.Text = dr["EmpBusinessAddress"].ToString();
                //        ddSPAEmpStat.SelectedValue = dr["EmploymentStatus"].ToString();
                //        ddSPANatureEmp.SelectedValue = dr["NatureOfEmp"].ToString();
                //        ddSPACivilStatus.SelectedValue = dr["CivilStatus"].ToString();
                //        lblSPAFileName.Text = dr["SPAFormDocument"].ToString();

                //        if (!String.IsNullOrEmpty(lblSPAFileName.Text))
                //        {
                //            visibleDocumentButtons(true, btnSPAPreview, btnSPARemove);
                //        }
                //        else
                //        {
                //            visibleDocumentButtons(false, btnSPAPreview, btnSPARemove);
                //        }

                //        DataTable dt1 = new DataTable();

                //        //int UserID = (int)Session["UserID"];
                //        //if (ws.DeleteListSPA(UserID, ID) == true)
                //        //{

                //        //    dt1 = ws.select_temp_crd5(UserID).Tables["select_temp_crd5"];
                //        //    Session["gvSPACoBorrower"] = dt1;
                //        //    LoadData(gvSPACoBorrower, "gvSPACoBorrower");
                //        //}
                //    }
                //}


                #endregion


                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);

            }
            catch (Exception ex)
            {
                alertMsg2(ex.Message, "error");
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);
            }

        }

        protected void gvSPACBDelete_Click(object sender, EventArgs e)
        {
            //2023-06-18: COMMENTED FOR SPA PROCESS
            //LinkButton GetID = (LinkButton)sender; 
            //Session["SPACB_ID"] = Convert.ToInt32(GetID.CommandArgument);
            //confirmation("Are you sure you want to delete the selected SPA/Co-Borrower?", "DelSPACoBorrower");

            LinkButton GetID = (LinkButton)sender;
            int row = int.Parse(GetID.CommandArgument.ToString());
            Session["ExtBuyerRow"] = row;
            Session["ExtGv"] = "gvSPACoBorrower";
            confirmation($"Are you sure you want to remove SPA?");
        }

        void confirmation(string body, string type)
        {
            Session["ConfirmType"] = type;
            lblConfirmationInfo.Text = body;
            ScriptManager.RegisterStartupScript(this, GetType(), "confirmation", "showMsgConfirm();", true);
        }

        void ClearSPACoBorrower()
        {
            //2023-06-08 : REMOVE CONDITION; ALWAYS SPA
            //tSPA.Checked = false; 
            //tCoBorrower.Checked = false;

            tRelationship.Value = string.Empty;
            txtSPALastName.Text = string.Empty;
            txtSPAFirstName.Text = string.Empty;
            txtSPAMiddleName.Text = string.Empty;
            ddSPAGender.SelectedValue = string.Empty;
            txtSPACitizenship.Value = string.Empty;
            dtSPABirthDate.Text = string.Empty;
            txtSPABirthPlace.Text = string.Empty;
            txtSPAMobile.Text = string.Empty;
            txtSPATelNo.Text = string.Empty;
            txtSPAEmail.Text = string.Empty;
            txtSPAFB.Text = string.Empty;
            txtSPATinNo.Text = string.Empty;
            txtSPASSSNo.Text = string.Empty;
            txtSPAGSIS.Text = string.Empty;
            txtSPAPagibi.Text = string.Empty;

            //2023-06-15 : DO NOT PUT PH WHEN ADDING SPA
            //ddPreCountry.SelectedValue = "PH";
            //ddPermCountry.SelectedValue = "PH";


            txtSPAPosition.Text = string.Empty;
            txtSPAYearsService.Text = "0";
            txtSPAOfcTelNo.Text = string.Empty;
            txtSPAFaxNo.Text = string.Empty;
            txtSPAYearsService.Text = "0";
            txtSPABusName.Text = string.Empty;
            txtSPABusAdd.Text = string.Empty;
            ddSPAEmpStat.SelectedValue = string.Empty;
            ddSPANatureEmp.SelectedValue = string.Empty;
            ddSPACivilStatus.SelectedValue = string.Empty;
            lblSPAFileName.Text = string.Empty;
            //tRented_CheckedChanged(tOwned, EventArgs.Empty);
            LoadData(gvSPACoBorrower, "gvSPACoBorrower");
            SPAAddUpdate.InnerText = "Add";

            visibleDocumentButtons(false, btnSPAPreview, btnSPARemove);
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

        }

        protected void btnBuyerPrint_ServerClick(object sender, EventArgs e)
        {
            //REPORT
            Session["PrintDocEntry"] = Session["SQDocEntry"].ToString();
            Session["ReportPath"] = ConfigurationManager.AppSettings["ReportPathForms"].ToString();
            Session["ReportName"] = ConfigSettings.BuyersInfoForm;

            //open new tab
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openpage", "window.open('ReportViewer.aspx');", true);
        }

        protected void bDependent_ServerClick(object sender, EventArgs e)
        {
            Control btn = (Control)sender;
            Session["bDependent"] = btn.ID;
        }

        protected void gvDelete_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            Control btn = (Control)sender;
            Session["Buyers_Control"] = btn.ID;
            Session["ExtGv"] = "gvSPADependent";
            Session["Buyers_ID"] = Convert.ToInt32(GetID.CommandArgument);
            Session["ExtBuyerRow"] = Convert.ToInt32(GetID.CommandArgument);
            confirmation("Are you sure you want to delete the selected dependent?", "DelDependent");
        }

        protected void gvSPADependent_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadData(gvSPADependent, "dtSPADependent");
            gvSPADependent.PageIndex = e.NewPageIndex;
            gvSPADependent.DataBind();
        }
        void ClearDependent()
        {
            mtxtRow.Text = "";
            mtxtName.Text = "";
            mtxtAge.Text = "";
            tDependentName.Value = "";
            mtxtRelationship.Value = "";
            tDependentAge.Value = "";
            tDependentRelationship.Value = "";
        }

        protected void txtLastName2_TextChanged(object sender, EventArgs e)
        {
            txtLastName.Text = txtLastName2.Text;
            txtCertifyCompleteName.Value = ($@"{txtFirstName2.Text} {txtMiddleName2.Text} {txtLastName2.Text}").Trim();
        }

        protected void txtFirstName2_TextChanged(object sender, EventArgs e)
        {
            txtFirstName.Text = txtFirstName2.Text;
            txtCertifyCompleteName.Value = ($@"{txtFirstName2.Text} {txtMiddleName2.Text} {txtLastName2.Text}").Trim();
        }

        protected void txtMiddleName2_TextChanged(object sender, EventArgs e)
        {
            txtMiddleName.Text = txtMiddleName2.Text;
            txtCertifyCompleteName.Value = ($@"{txtFirstName2.Text} {txtMiddleName2.Text} {txtLastName2.Text}").Trim();
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

            dr[0] = Convert.ToInt32(dt.Rows.Count + 1);
            dr[1] = tCoFName.Value;
            dr[2] = tCoMName.Value;
            dr[3] = tCoLName.Value;
            dr[4] = tCoRelationship.Value;
            dr[5] = tCoEmail.Value;
            dr[6] = tCoMobileNo.Value;
            dr[7] = tCoAddress.Value;
            dr[8] = tCoResidence.Value;
            dr[9] = tCoValidID.SelectedItem;
            dr[10] = tCoValidIDNo.Value;
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

        void ClearCoOwner()
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
            Session["ExtGv"] = "";
            Session["ExtBuyerRow"] = GetID.CommandArgument;
            Session["Buyers_Control"] = btn.ID;
            Session["Buyers_ID"] = Convert.ToInt32(GetID.CommandArgument);
            confirmation($@"Are you sure you want to delete the selected {type}?", action);
        }
        void SpecialBuyersValidation(bool val)
        {
            RequiredFieldValidator21.Enabled = val;
            RequiredFieldValidator23.Enabled = val;
            RequiredFieldValidator24.Enabled = val;
            RequiredFieldValidator25.Enabled = val;
            RequiredFieldValidator26.Enabled = val;
            RequiredFieldValidator27.Enabled = val;
        }

        void HidePermAddress(bool val)
        {
            addtitle.InnerText = val ? "Complete Present Address" : "Registered Address.";

            nonCorpAddress.Visible = val;
            RequiredFieldValidator54.Enabled = val;
            RequiredFieldValidator55.Enabled = val;
            RequiredFieldValidator56.Enabled = val;
            RequiredFieldValidator57.Enabled = val;
            RequiredFieldValidator58.Enabled = val;
            RequiredFieldValidator60.Enabled = val;
            RequiredFieldValidator61.Enabled = val;
            RequiredFieldValidator62.Enabled = val;
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

        protected void gvCoOwner_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCoOwner.PageIndex = e.NewPageIndex;
            LoadData(gvCoOwner, "dtCoOwner");
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


            clearSPADetails();
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);
        }

        protected void ddTypeOfID_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["ValidId"];
            DataTable dt2 = (DataTable)ViewState["ValidIdPrev"];

            txtIDNumber.Text = "";

            string checker = ddTypeOfID.SelectedValue;
            if (checker == "---Select Valid ID---")
            {
                if (dt2.Rows[0]["Code"].ToString() != "---Select Type of ID---" && dt2.Rows[0]["Code"].ToString() != "OTH" && dt2.Rows[0]["Code"].ToString() != checker)
                {
                    dt.Rows.Add(
                    dt2.Rows[0]["Code"].ToString(),
                    dt2.Rows[0]["Name"].ToString()
                    );
                }
                dt2.Rows[0]["Code"] = ddTypeOfID.SelectedValue;
                dt2.Rows[0]["Name"] = ddTypeOfID.SelectedItem;
                ChangeOthers(false, false, 1);
            }
            if (checker == "OTH")
            {
                if (dt2.Rows[0]["Code"].ToString() != "---Select Type of ID---" && dt2.Rows[0]["Code"].ToString() != "OTH" && dt2.Rows[0]["Code"].ToString() != checker)
                {
                    dt.Rows.Add(
                    dt2.Rows[0]["Code"].ToString(),
                    dt2.Rows[0]["Name"].ToString()
                    );
                }
                dt2.Rows[0]["Code"] = ddTypeOfID.SelectedValue;
                dt2.Rows[0]["Name"] = ddTypeOfID.SelectedItem;
                ChangeOthers(true, true, 1);
            }
            else
            {
                if (dt2.Rows[0]["Code"].ToString() != "---Select Type of ID---" && dt2.Rows[0]["Code"].ToString() != "OTH" && dt2.Rows[0]["Code"].ToString() != checker)
                {
                    dt.Rows.Add(
                    dt2.Rows[0]["Code"].ToString(),
                    dt2.Rows[0]["Name"].ToString()
                    );
                }
                dt2.Rows[0]["Code"] = ddTypeOfID.SelectedValue;
                dt2.Rows[0]["Name"] = ddTypeOfID.SelectedItem;
                if (ddTypeOfID.SelectedValue == "ID1")
                {
                    txtIDNumber.Text = txtTIN.Text;
                }
                if (ddTypeOfID.SelectedValue == "ID2")
                {
                    txtIDNumber.Text = txtSSS.Text;
                }
                if (ddTypeOfID.SelectedValue == "ID3")
                {
                    txtIDNumber.Text = txtPagIbig.Text;
                }
                foreach (DataRow row in dt.Rows)
                {
                    if (row[0].ToString() == checker && row[0].ToString() != "OTH" && row[0].ToString() != "---Select Type of ID---")
                    {
                        dt.Rows.Remove(row);
                        break;
                    }
                }
                ChangeOthers(true, false, 1);
            }
            ChangeValidIDDd(dt, dt2, "ddTypeOfID", 1);
            txtIDNumber.Focus();



        }

        protected void ddTypeOfID2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["ValidId"];
            DataTable dt2 = (DataTable)ViewState["ValidIdPrev"];

            string checker = ddTypeOfID2.SelectedValue;
            txtIDNumber2.Text = "";
            if (checker == "---Select Valid ID---")
            {
                if (dt2.Rows[1]["Code"].ToString() != "---Select Type of ID---" && dt2.Rows[1]["Code"].ToString() != "OTH" && dt2.Rows[1]["Code"].ToString() != checker)
                {
                    dt.Rows.Add(
                    dt2.Rows[1]["Code"].ToString(),
                    dt2.Rows[1]["Name"].ToString()
                    );
                }
                dt2.Rows[1]["Code"] = ddTypeOfID2.SelectedValue;
                dt2.Rows[1]["Name"] = ddTypeOfID2.SelectedItem;
                ChangeOthers(false, false, 2);
            }
            if (checker == "OTH")
            {
                if (dt2.Rows[1]["Code"].ToString() != "---Select Type of ID---" && dt2.Rows[1]["Code"].ToString() != "OTH" && dt2.Rows[1]["Code"].ToString() != checker)
                {
                    dt.Rows.Add(
                    dt2.Rows[1]["Code"].ToString(),
                    dt2.Rows[1]["Name"].ToString()
                    );
                }
                dt2.Rows[1]["Code"] = ddTypeOfID2.SelectedValue;
                dt2.Rows[1]["Name"] = ddTypeOfID2.SelectedItem;
                ChangeOthers(true, true, 2);
            }
            else
            {
                if (dt2.Rows[1]["Code"].ToString() != "---Select Type of ID---" && dt2.Rows[1]["Code"].ToString() != "OTH" && dt2.Rows[1]["Code"].ToString() != checker)
                {
                    dt.Rows.Add(
                    dt2.Rows[1]["Code"].ToString(),
                    dt2.Rows[1]["Name"].ToString()
                    );
                }
                dt2.Rows[1]["Code"] = ddTypeOfID2.SelectedValue;
                dt2.Rows[1]["Name"] = ddTypeOfID2.SelectedItem;
                if (ddTypeOfID2.SelectedValue == "ID1")
                {
                    txtIDNumber2.Text = txtTIN.Text;
                }
                if (ddTypeOfID2.SelectedValue == "ID2")
                {
                    txtIDNumber2.Text = txtSSS.Text;
                }
                if (ddTypeOfID2.SelectedValue == "ID3")
                {
                    txtIDNumber2.Text = txtPagIbig.Text;
                }
                foreach (DataRow row in dt.Rows)
                {
                    if (row[0].ToString() == checker && row[0].ToString() != "OTH" && row[0].ToString() != "---Select Type of ID---")
                    {
                        dt.Rows.Remove(row);
                        break;
                    }
                }
                ChangeOthers(true, false, 2);
            }
            ChangeValidIDDd(dt, dt2, "ddTypeOfID2", 2);

            txtIDNumber2.Focus();

        }

        protected void ddTypeOfID3_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["ValidId"];
            DataTable dt2 = (DataTable)ViewState["ValidIdPrev"];

            string checker = ddTypeOfID3.SelectedValue;
            if (checker == "---Select Valid ID---")
            {
                if (dt2.Rows[2]["Code"].ToString() != "---Select Type of ID---" && dt2.Rows[2]["Code"].ToString() != "OTH" && dt2.Rows[2]["Code"].ToString() != checker)
                {
                    dt.Rows.Add(
                    dt2.Rows[2]["Code"].ToString(),
                    dt2.Rows[2]["Name"].ToString()
                    );
                }
                dt2.Rows[2]["Code"] = ddTypeOfID3.SelectedValue;
                dt2.Rows[2]["Name"] = ddTypeOfID3.SelectedItem;
                ChangeOthers(false, false, 3);
            }
            if (checker == "OTH")
            {
                if (dt2.Rows[2]["Code"].ToString() != "---Select Type of ID---" && dt2.Rows[2]["Code"].ToString() != "OTH" && dt2.Rows[2]["Code"].ToString() != checker)
                {
                    dt.Rows.Add(
                    dt2.Rows[2]["Code"].ToString(),
                    dt2.Rows[2]["Name"].ToString()
                    );
                }
                dt2.Rows[2]["Code"] = ddTypeOfID3.SelectedValue;
                dt2.Rows[2]["Name"] = ddTypeOfID3.SelectedItem;
                ChangeOthers(true, true, 3);
            }
            else
            {
                if (dt2.Rows[2]["Code"].ToString() != "---Select Type of ID---" && dt2.Rows[2]["Code"].ToString() != "OTH" && dt2.Rows[2]["Code"].ToString() != checker)
                {
                    dt.Rows.Add(
                    dt2.Rows[2]["Code"].ToString(),
                    dt2.Rows[2]["Name"].ToString()
                    );
                }
                dt2.Rows[2]["Code"] = ddTypeOfID3.SelectedValue;
                dt2.Rows[2]["Name"] = ddTypeOfID3.SelectedItem;
                if (ddTypeOfID3.SelectedValue == "ID1")
                {
                    txtIDNumber3.Text = txtTIN.Text;
                }
                foreach (DataRow row in dt.Rows)
                {
                    if (row[0].ToString() == checker && row[0].ToString() != "OTH" && row[0].ToString() != "---Select Type of ID---")
                    {
                        dt.Rows.Remove(row);
                        break;
                    }
                }
                ChangeOthers(true, false, 3);
            }
            ChangeValidIDDd(dt, dt2, "ddTypeOfID3", 3);
        }

        protected void ddTypeOfID4_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["ValidId"];
            DataTable dt2 = (DataTable)ViewState["ValidIdPrev"];

            string checker = ddTypeOfID4.SelectedValue;
            if (checker == "---Select Valid ID---")
            {
                if (dt2.Rows[3]["Code"].ToString() != "---Select Type of ID---" && dt2.Rows[3]["Code"].ToString() != "OTH" && dt2.Rows[3]["Code"].ToString() != checker)
                {
                    dt.Rows.Add(
                    dt2.Rows[3]["Code"].ToString(),
                    dt2.Rows[3]["Name"].ToString()
                    );
                }
                dt2.Rows[3]["Code"] = ddTypeOfID4.SelectedValue;
                dt2.Rows[3]["Name"] = ddTypeOfID4.SelectedItem;
                if (ddTypeOfID4.SelectedValue == "ID1")
                {
                    txtIDNumber4.Text = txtTIN.Text;
                }
                ChangeOthers(false, false, 4);
            }
            if (checker == "OTH")
            {
                if (dt2.Rows[3]["Code"].ToString() != "---Select Type of ID---" && dt2.Rows[3]["Code"].ToString() != "OTH" && dt2.Rows[3]["Code"].ToString() != checker)
                {
                    dt.Rows.Add(
                    dt2.Rows[3]["Code"].ToString(),
                    dt2.Rows[3]["Name"].ToString()
                    );
                }
                dt2.Rows[3]["Code"] = ddTypeOfID4.SelectedValue;
                dt2.Rows[3]["Name"] = ddTypeOfID4.SelectedItem;
                ChangeOthers(true, true, 4);
            }
            else
            {
                if (dt2.Rows[3]["Code"].ToString() != "---Select Type of ID---" && dt2.Rows[3]["Code"].ToString() != "OTH" && dt2.Rows[3]["Code"].ToString() != checker)
                {
                    dt.Rows.Add(
                    dt2.Rows[3]["Code"].ToString(),
                    dt2.Rows[3]["Name"].ToString()
                    );
                }
                dt2.Rows[3]["Code"] = ddTypeOfID4.SelectedValue;
                dt2.Rows[3]["Name"] = ddTypeOfID4.SelectedItem;
                foreach (DataRow row in dt.Rows)
                {
                    if (row[0].ToString() == checker && row[0].ToString() != "OTH" && row[0].ToString() != "---Select Type of ID---")
                    {
                        dt.Rows.Remove(row);
                        break;
                    }
                }
                ChangeOthers(true, false, 4);
            }
            ChangeValidIDDd(dt, dt2, "ddTypeOfID4", 4);
        }

        void ChangeValidIDDd(DataTable dt, DataTable dt2, string dropdown, int tag)
        {

            string prev;
            ViewState["ValidIdPrev"] = dt2;
            ViewState["ValidId"] = dt;

            if (dropdown != "ddTypeOfID")
            {
                if (ddTypeOfID.SelectedValue == "---Select Type of ID---")
                {
                    ddTypeOfID.DataSource = dt;
                    ddTypeOfID.DataBind();
                    ddTypeOfID.SelectedValue = "---Select Type of ID---";
                }
                else
                {
                    prev = ddTypeOfID.SelectedValue;
                    ddTypeOfID.SelectedValue = "---Select Type of ID---";
                    ddTypeOfID.DataSource = dt;
                    ddTypeOfID.DataBind();
                    if (!dt.Columns["Code"].ToString().Contains(prev))
                    {
                        ddTypeOfID.Items.Add(new ListItem(dt2.Rows[0]["Name"].ToString(), dt2.Rows[0]["Code"].ToString()));
                    }
                    ddTypeOfID.SelectedValue = prev;
                }
            }
            if (dropdown != "ddTypeOfID2")
            {
                if (ddTypeOfID2.SelectedValue == "---Select Type of ID---")
                {
                    ddTypeOfID2.DataSource = dt;
                    ddTypeOfID2.DataBind();
                    ddTypeOfID2.SelectedValue = "---Select Type of ID---";
                }
                else if (ddTypeOfID2.SelectedValue == "OTH")
                {
                    ddTypeOfID2.DataSource = dt;
                    ddTypeOfID2.DataBind();
                    ddTypeOfID2.SelectedValue = "OTH";
                }
                else
                {
                    prev = ddTypeOfID2.SelectedValue;
                    ddTypeOfID2.SelectedValue = "---Select Type of ID---";
                    ddTypeOfID2.DataSource = dt;
                    ddTypeOfID2.DataBind();
                    if (!dt.Columns["Code"].ToString().Contains(prev))
                    {
                        ddTypeOfID2.Items.Add(new ListItem(dt2.Rows[1]["Name"].ToString(), dt2.Rows[1]["Code"].ToString()));
                    }
                    ddTypeOfID2.SelectedValue = prev;
                }
            }
            if (dropdown != "ddTypeOfID3")
            {
                if (ddTypeOfID3.SelectedValue == "---Select Type of ID---")
                {
                    ddTypeOfID3.DataSource = dt;
                    ddTypeOfID3.DataBind();
                    ddTypeOfID3.SelectedValue = "---Select Type of ID---";
                }
                else if (ddTypeOfID3.SelectedValue == "OTH")
                {
                    ddTypeOfID3.DataSource = dt;
                    ddTypeOfID3.DataBind();
                    ddTypeOfID3.SelectedValue = "OTH";
                }
                else
                {
                    prev = ddTypeOfID3.SelectedValue;
                    ddTypeOfID3.SelectedValue = "---Select Type of ID---";
                    ddTypeOfID3.DataSource = dt;
                    ddTypeOfID3.DataBind();
                    if (!dt.Columns["Code"].ToString().Contains(prev))
                    {
                        ddTypeOfID3.Items.Add(new ListItem(dt2.Rows[2]["Name"].ToString(), dt2.Rows[2]["Code"].ToString()));
                    }
                    ddTypeOfID3.SelectedValue = prev;
                }
            }
            if (dropdown != "ddTypeOfID4")
            {
                if (ddTypeOfID4.SelectedValue == "---Select Type of ID---")
                {
                    ddTypeOfID4.DataSource = dt;
                    ddTypeOfID4.DataBind();
                    ddTypeOfID4.SelectedValue = "---Select Type of ID---";
                }
                else if (ddTypeOfID4.SelectedValue == "OTH")
                {
                    ddTypeOfID4.DataSource = dt;
                    ddTypeOfID4.DataBind();
                    ddTypeOfID4.SelectedValue = "OTH";
                }
                else
                {
                    prev = ddTypeOfID4.SelectedValue;
                    ddTypeOfID4.SelectedValue = "---Select Type of ID---";
                    ddTypeOfID4.DataSource = dt;
                    ddTypeOfID4.DataBind();
                    if (!dt.Columns["Code"].ToString().Contains(prev))
                    {
                        ddTypeOfID4.Items.Add(new ListItem(dt2.Rows[3]["Name"].ToString(), dt2.Rows[3]["Code"].ToString()));
                    }
                    ddTypeOfID4.SelectedValue = prev;
                }
            }
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
                    //RequiredFieldValidator11.Enabled = enabledBlocker;
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
                    //RequiredFieldValidator45.Enabled = enabledBlocker;
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
                    //RequiredFieldValidator46.Enabled = enabledBlocker;
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
                    //RequiredFieldValidator11.Enabled = enabledBlocker;
                }
                if (num == 3)
                {
                    others3.Visible = showOthers;
                    ddothers3.Attributes.Remove("class");
                    ddothers3.Attributes.Add("class", "col-md-6");
                    //RequiredFieldValidator45.Enabled = enabledBlocker;
                }
                if (num == 4)
                {
                    others4.Visible = showOthers;
                    ddothers4.Attributes.Remove("class");
                    ddothers4.Attributes.Add("class", "col-md-6");
                    //RequiredFieldValidator46.Enabled = enabledBlocker;
                }
            }
        }

        protected void CustomValidator4_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                DateTime checker = Convert.ToDateTime(args.Value);
                TimeSpan totaldays = DateTime.Now.Subtract(checker);
                int age = totaldays.Days / 365;
                if (age < 18)
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
                alertMsg2(ex.Message, "error");
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
                alertMsg2(ex.Message, "error");
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
                alertMsg2(ex.Message, "error");
            }
        }

        protected void txtTIN_TextChanged(object sender, EventArgs e)
        {
            if (ddTypeOfID.SelectedValue == "ID1")
            {
                txtIDNumber.Text = txtTIN.Text;
            }
            else if (ddTypeOfID2.SelectedValue == "ID1")
            {
                txtIDNumber2.Text = txtTIN.Text;
            }
            else if (ddTypeOfID3.SelectedValue == "ID1")
            {
                txtIDNumber3.Text = txtTIN.Text;
            }
            else if (ddTypeOfID4.SelectedValue == "ID1")
            {
                txtIDNumber4.Text = txtTIN.Text;
            }
            //tTINCorp.Text = txtTIN.Text;
        }

        protected void txtIDNumber_TextChanged(object sender, EventArgs e)
        {
            if (ddTypeOfID.SelectedValue == "ID1")
            {
                txtTIN.Text = txtIDNumber.Text;
            }
            if (ddTypeOfID.SelectedValue == "ID2")
            {
                txtSSS.Text = txtIDNumber.Text;
            }
            if (ddTypeOfID.SelectedValue == "ID3")
            {
                txtPagIbig.Text = txtIDNumber.Text;
            }
            ddTypeOfID2.Focus();
        }

        protected void txtIDNumber2_TextChanged(object sender, EventArgs e)
        {
            if (ddTypeOfID2.SelectedValue == "ID1")
            {
                txtTIN.Text = txtIDNumber2.Text;
            }
            if (ddTypeOfID2.SelectedValue == "ID2")
            {
                txtSSS.Text = txtIDNumber2.Text;
            }
            if (ddTypeOfID2.SelectedValue == "ID3")
            {
                txtPagIbig.Text = txtIDNumber2.Text;
            }
        }

        protected void txtIDNumber3_TextChanged(object sender, EventArgs e)
        {
            if (ddTypeOfID3.SelectedValue == "ID1")
            {
                txtTIN.Text = txtIDNumber3.Text;
            }
        }

        protected void txtIDNumber4_TextChanged(object sender, EventArgs e)
        {
            if (ddTypeOfID4.SelectedValue == "ID1")
            {
                txtTIN.Text = txtIDNumber4.Text;
            }
        }

        private void taxClassChanged()
        {
            //KARL - SET SOURCE OF FUNDS AND POSITION VALIDATOR DEPENDING ON TAX CLASSIFICATION - 2023/05/04
            if (ddTaxClass.SelectedValue != "Not engaged in business")
            {
                sourceoffund.Visible = false;
                ddSourceFunds_RequiredFieldValidator.Enabled = false;
                txtMyPosition_RequiredFieldValidator.Enabled = false;
                empstatus.Visible = false;
                empDetails.Visible = false;
                RequiredFieldValidator1.Enabled = false;
            }
            else
            {
                sourceoffund.Visible = true;
                ddSourceFunds_RequiredFieldValidator.Enabled = true;
                txtMyPosition_RequiredFieldValidator.Enabled = true;
                empstatus.Visible = true;
                empDetails.Visible = true;
                RequiredFieldValidator1.Enabled = true;

            }
        }

        protected void ddTaxClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            taxClassChanged();
        }

        protected void CustomValidator7_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //BLOCK ADDING WHEN TIN ALREADY EXIST
            string qry = $@"SELECT ""CardCode"" FROM ""CRD1"" where ""TIN"" = '{tTINCorp.Text}'";
            DataTable dt = hana.GetData(qry, hana.GetConnection("SAOHana"));
            if (dt.Rows.Count == 0)
            {
                //Check if TIN is in correct format(###-###-###-###)
                //bool isOK = Regex.IsMatch(tTIN.Value, @"[0-9][0-9][0-9]\-[0-9][0-9][0-9]\-[0-9][0-9][0-9]\-[0-9][0-9][0-9]");
                bool isOK = Regex.IsMatch(tTINCorp.Text, @"[0-9][0-9][0-9]\-[0-9][0-9][0-9]\-[0-9][0-9][0-9]\-[0-9][0-9][0-9]");
                if (!isOK)
                {
                    CustomValidator7.Text = "Incorrect TIN format. Must be xxx-xxx-xxx-xxx.";
                    RequiredFieldValidator41.Visible = false;
                    args.IsValid = false;
                }
                else
                {
                    RequiredFieldValidator41.Visible = true;
                    args.IsValid = true;
                }
            }
            else
            {
                CustomValidator7.Text = "TIN already exists!";
                RequiredFieldValidator41.Visible = false;
                args.IsValid = false;
            }

        }

        protected void tTINCorp_TextChanged(object sender, EventArgs e)
        {
            //txtTIN.Text = tTINCorp.Text;
            //if (ddTypeOfID.SelectedValue == "ID1")
            //    txtIDNumber.Text = txtTIN.Text;
            //if (ddTypeOfID2.SelectedValue == "ID1")
            //    txtIDNumber2.Text = txtTIN.Text;
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


        //void LoadBuyerDocumentsStandard()
        //{
        //    DataTable buyerAttachments = ws.GetBuyerAttachments().Tables[0];

        //    gvStandardDocumentRequirements.DataSource = buyerAttachments;
        //    gvStandardDocumentRequirements.DataBind();

        //    // Modify the GridView row to add the span to the Document Name
        //    foreach (GridViewRow row in gvStandardDocumentRequirements.Rows)
        //    {
        //        if (row.RowType == DataControlRowType.DataRow)
        //        {
        //            Label lblFileName = (Label)row.FindControl("lblFileName");
        //            string documentName = lblFileName.Text;
        //            lblFileName.Text = $"<span class=\"color-red fsize-16\"> *</span>{documentName}";
        //        }
        //    }
        //}


        void LoadBuyerDocumentsStandard_NotRequired()
        {
            //<% --2023 - 04 - 19 : REQUESTED BY DATA TO REMOVE BLOCKING OF 2ND ID--%>
            //     <% --CHANG REQUEST: NO BLOCKINGS FOR THIS GRIDVIEW-- %>
            gvStandardDocumentRequirements2.DataSource = ws.GetBuyerAttachments_NotRequired();
            gvStandardDocumentRequirements2.DataBind();
        }


        //protected void btnUpload_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        attachmentFileUploadBuyer(FileUploadBilling,"Upload");
        //    }
        //    catch (Exception ex)
        //    {
        //        alertMsg(ex.Message, "error");
        //    }
        //}

        protected void gvStandardDocumentRequirements_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            attachmentFileUploadBuyer(gvStandardDocumentRequirements, e, "FileUpload1", "lblFileName", "btnPreview", "btnRemove", "RequiredFieldValidatorAttachments");
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

            string buyerFullName = (txtFirstName.Text + "_" + txtMiddleName.Text + "_" + txtLastName.Text).Trim().ToString().ToUpper();
            string code = ViewState["Guid"].ToString();
            string uniqueCode = code + "_" + txtTIN.Text.Replace("-", "");
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

                    //if(index == 0)
                    //    FileName = "pob" + "_" + fileNameOnly + "_" + code + extension;

                    //if(index == 1)
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
                        alertMsg2("File already exists!", "error");
                        rvReq.Enabled = true;
                        //alert2lbl.Text = "File already exists!";
                        //alert2img.ImageUrl = "~/assets/img/error.png";
                        //ScriptManager.RegisterStartupScript(this, GetType(), "alert", "ShowAlert2();", true);
                    }
                    else
                    {
                        lblFileName.Text = FileName;
                        //file.PostedFile.SaveAs(HttpContext.Current.Server.MapPath($"~/TEMP_DOCS/{FolderName}/") + FileName); //File is saved in the Physical folder 
                        file.PostedFile.SaveAs(HttpContext.Current.Server.MapPath($"~/TEMP_DOCS/") + FileName); //File is saved in the Physical folder 
                        btnPreview.Visible = true;
                        btnDelete.Visible = true;
                        rvReq.Enabled = false;
                    }
                }
                else
                {
                    rvReq.Enabled = false;
                    alertMsg2("Please choose file!", "warning");
                    //alert2lbl.Text = "Please choose file!";
                    //alert2img.ImageUrl = "~/assets/img/warning.png";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "alert", "ShowAlert2();", true);
                }
            }
            else if (e.CommandName.ToLower().Contains("preview"))
            {
                //if (File.Exists(Server.MapPath($"~/TEMP_DOCS/{FolderName}/") + lblFileName.Text))
                if (File.Exists(Server.MapPath($"~/TEMP_DOCS/") + lblFileName.Text))
                {
                    //string Filepath = Server.MapPath($"~/TEMP_DOCS/{FolderName}/" + lblFileName.Text);
                    string Filepath = Server.MapPath($"~/TEMP_DOCS/" + lblFileName.Text);
                    var host = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.LocalPath, "");
                    //ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + host + $"/TEMP_DOCS/{FolderName}/" + lblFileName.Text + "');", true);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + host + $"/TEMP_DOCS/" + lblFileName.Text + "');", true);
                }
                else
                {
                    string Filepath = Server.MapPath("~/BUYER_ATTACHMENTS/" + lblFileName.Text);
                    var host = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.LocalPath, "");
                    //ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + host + $"/BUYER_ATTACHMENTS/{FolderName}/" + lblFileName.Text + "');", true);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + host + $"/BUYER_ATTACHMENTS/" + lblFileName.Text + "');", true);
                }

            }
            else if (e.CommandName.ToLower().Contains("remove"))
            {

                //if (File.Exists(Server.MapPath($"~/TEMP_DOCS/{FolderName}/") + lblFileName.Text))
                if (File.Exists(Server.MapPath($"~/TEMP_DOCS/") + lblFileName.Text))
                {
                    // If file found, delete it 
                    //File.Delete(Server.MapPath($"~/TEMP_DOCS/{FolderName}/") + lblFileName.Text);
                    File.Delete(Server.MapPath($"~/TEMP_DOCS/") + lblFileName.Text);
                    lblFileName.Text = "";
                }
                else
                {
                    lblFileName.Text = "";
                }
                btnPreview.Visible = false;
                btnDelete.Visible = false;
                rvReq.Enabled = true;
            }

        }

        void moveTemporaryFilesToPermanent(GridView gv, string lblfileName)
        {
            string buyerFullName = (txtFirstName.Text + "_" + txtMiddleName.Text + "_" + txtLastName.Text).Trim().ToString().ToUpper();
            string code = ViewState["Guid"].ToString();
            string uniqueCode = code + "_" + txtTIN.Text.Replace("-", "");
            //string FolderName = uniqueCode + "_" + buyerFullName;

            foreach (GridViewRow row in gv.Rows)
            {
                int index = row.RowIndex;

                string FileName = ((Label)row.FindControl(lblfileName)).Text;

                if (!string.IsNullOrWhiteSpace(FileName)) //If the used Uploaded a file  
                {
                    string sourceFilePath = Server.MapPath($"~/TEMP_DOCS/") + FileName;
                    //string sourceFilePath = Server.MapPath($"~/TEMP_DOCS/{FolderName}/") + FileName;
                    //string destinationFilePath = Server.MapPath($"~/BUYER_ATTACHMENTS/{FolderName}/");
                    string destinationFilePath = Server.MapPath($"~/BUYER_ATTACHMENTS/");
                    //if (!System.IO.Directory.Exists(destinationFilePath))
                    //{
                    //    System.IO.Directory.CreateDirectory(destinationFilePath); //Create directory if it doesn't exist
                    //}
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




        void DeleteTemporaryFIles()
        {
            string Filepath = Server.MapPath("~/TEMP_DOCS/");
            string code = $@"*{ViewState["Guid"]}*.*";
            string[] fileList = Directory.GetFiles(Filepath, code);
            foreach (string file in fileList)
            {
                System.Diagnostics.Debug.WriteLine(file + "will be deleted");
                File.Delete(file);
            }
        }
        //void DeleteTemporaryFIles()
        //{
        //    string buyerFullName = (txtFirstName.Text + "_" + txtMiddleName.Text + "_" + txtLastName.Text).Trim().ToString().ToUpper();
        //    string code = ViewState["Guid"];
        //    string uniqueCode = code + "_" + txtTIN.Text.Replace("-", "");
        //    string FolderName = uniqueCode + "_" + buyerFullName;

        //    string Filepath = Server.MapPath($"~/TEMP_DOCS/{FolderName}/");

        //    string filePattern = $@"*{ViewState["Guid"]}*.*";
        //    string[] fileList = Directory.GetFiles(Filepath, filePattern);
        //    foreach (string file in fileList)
        //    {
        //        System.Diagnostics.Debug.WriteLine(file + "will be deleted");
        //        File.Delete(file);
        //    }

        //    if (System.IO.Directory.Exists(Server.MapPath($"~/TEMP_DOCS/{FolderName}")))
        //    {
        //        System.IO.Directory.Delete(Server.MapPath($"~/TEMP_DOCS/{FolderName}"));
        //    }
        //}

        protected void txtName_TextChanged(object sender, EventArgs e)
        {
            if (ddlBusinessType.Text != "Corporation")
            {
                txtCertifyCompleteName.Value = ($@"{txtFirstName.Text} {txtMiddleName.Text} {txtLastName.Text}").Trim();
            }
            else
            {
                txtCertifyCompleteName.Value = ($@"{txtFirstName2.Text} {txtMiddleName2.Text} {txtLastName2.Text}").Trim();
            }
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

        protected void txtSSS_TextChanged(object sender, EventArgs e)
        {
            if (ddTypeOfID.SelectedValue == "ID2")
            {
                txtIDNumber.Text = txtSSS.Text;
            }
            else if (ddTypeOfID2.SelectedValue == "ID2")
            {
                txtIDNumber2.Text = txtSSS.Text;
            }
        }

        protected void txtPagIbig_TextChanged(object sender, EventArgs e)
        {
            if (ddTypeOfID.SelectedValue == "ID3")
            {
                txtIDNumber.Text = txtPagIbig.Text;
            }
            else if (ddTypeOfID2.SelectedValue == "ID3")
            {
                txtIDNumber2.Text = txtPagIbig.Text;
            }
        }

        protected void txtCorpName_TextChanged(object sender, EventArgs e)
        {
            txtConformeCorp.Value = txtCorpName.Text;
        }

        protected void tSPA_CheckedChanged(object sender, EventArgs e)
        {

            //divUploadSPADocs.Visible = tSPA.Checked;

            //ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);
        }

        protected void btnSPAUpload_Click(object sender, EventArgs e)
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
                        alertMsg2("File already exists!", "error");
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
            //ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "MsgSPACoBorrower_Show();", true);
        }
        void visibleDocumentButtons(bool visible, LinkButton prev, LinkButton del)
        {
            prev.Visible = visible;
            del.Visible = visible;
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
        protected void ddSPOEmpStat_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);

            if (ddSPOEmpStat.SelectedValue == "")
            {
                dvSpouseDetails.Attributes.Clear();
                dvSpouseDetails.Attributes.Add("class", "col-md-12");
                divSpouseBusiness.Visible = false;

                ddSPONatureEmp.SelectedValue = "";
                txtSPOBusName.Text = string.Empty;
                txtSPOBusAdd.Text = string.Empty;
                txtSPOPosition.Text = string.Empty;
                txtSPOYearsService.Text = string.Empty;
                txtSPOOfcTelNo.Text = string.Empty;
                txtSPOFaxNo.Text = string.Empty;
                txtSPOTinNo.Text = string.Empty;
                txtSPOSSSNo.Text = string.Empty;
                txtSPOGSIS.Text = string.Empty;
                txtSPOPagibi.Text = string.Empty;


                //COMMENTED 06 - 13 - 2023 : CHANGE REQUEST  https://direcbti.monday.com/boards/4462005457/pulses/4576973637?asset_id=908456598 --%>
                RequiredFieldValidator13.Enabled = false;
            }
            else
            {
                dvSpouseDetails.Attributes.Clear();
                dvSpouseDetails.Attributes.Add("class", "col-md-6");
                divSpouseBusiness.Visible = true;

                //COMMENTED 06 - 13 - 2023 : CHANGE REQUEST  https://direcbti.monday.com/boards/4462005457/pulses/4576973637?asset_id=908456598 --%>
                RequiredFieldValidator13.Enabled = true;

            }
        }

        protected void btnCopyPrincipalAddress_Click(object sender, EventArgs e)
        {
            txtSPOAddress.Text = (txtAddress.Text.ToUpper() == "N/A" ? "" : $@"{txtAddress.Text} ")
                + (txtPresentStreet.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentStreet.Text} ")
                + (txtPresentSubdivision.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentSubdivision.Text} ")
                + (txtPresentBarangay.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentBarangay.Text} ")
                + (txtPresentCity.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentCity.Text} ")
                + (txtPresentProvince.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentProvince.Text} ");

            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);
        }

        protected void btnCopyPrincipalAddressSPA_Click(object sender, EventArgs e)
        {
            txtSPAAddress.Text = (txtAddress.Text.ToUpper() == "N/A" ? "" : $@"{txtAddress.Text} ")
                + (txtPresentStreet.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentStreet.Text} ")
                + (txtPresentSubdivision.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentSubdivision.Text} ")
                + (txtPresentBarangay.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentBarangay.Text} ")
                + (txtPresentCity.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentCity.Text} ")
                + (txtPresentProvince.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentProvince.Text} ");

            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);
        }

        protected void btnSameasPresent_Click(object sender, EventArgs e)
        {
            txtPermanentAdd.Text = txtAddress.Text;
            txtPermanentStreet.Text = txtPresentStreet.Text;
            txtPermanentSubdivision.Text = txtPresentSubdivision.Text;
            txtPermanentBarangay.Text = txtPresentBarangay.Text;
            txtPermanentCity.Text = txtPresentCity.Text;
            txtPermanentProvince.Text = txtPresentProvince.Text;
            txtPermPostal.Text = txtPresPostal.Text;
            ddPermCountry.SelectedValue = ddPreCountry.SelectedValue;
            txtPermYrStay.Text = txtPresYrStay.Text;

            hidebtnSameasPresent();
        }

        void hidebtnSameasPresent()
        {
            string present = ((txtAddress.Text.ToUpper() == "N/A" ? "" : $@"{txtAddress.Text} ")
                + (txtPresentStreet.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentStreet.Text} ")
                + (txtPresentSubdivision.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentSubdivision.Text} ")
                + (txtPresentBarangay.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentBarangay.Text} ")
                + (txtPresentCity.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentCity.Text} ")
                + (txtPresentProvince.Text.ToUpper() == "N/A" ? "" : $@"{txtPresentProvince.Text} ")
                + (txtPresPostal.Text.ToUpper() == "N/A" ? "" : $@"{txtPresPostal.Text} ")
                + (txtPresYrStay.Text.ToUpper() == "N/A" ? "" : $@"{txtPresYrStay.Text} ")).Trim().ToUpper();

            string permanent = ((txtPermanentAdd.Text.ToUpper() == "N/A" ? "" : $@"{txtPermanentAdd.Text} ")
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
        }

        protected void txtAddress_TextChanged(object sender, EventArgs e)
        {
            hidebtnSameasPresent();
        }

        protected void ddPreCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            hidebtnSameasPresent();
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

            string buyerFullName = (txtFirstName.Text + "_" + txtMiddleName.Text + "_" + txtLastName.Text).Trim().ToString().ToUpper();
            string code = ViewState["Guid"].ToString();
            string uniqueCode = code + "_" + txtTIN.Text.Replace("-", "");
            //string FolderName = uniqueCode + "_" + buyerFullName; //Folder Name for buyer
            if (e.CommandName.ToLower().Contains("upload"))
            {

                if (file.HasFile)
                {

                    //Get FileName and Extension seperately
                    string fileNameOnly = Path.GetFileNameWithoutExtension(file.FileName);
                    string extension = Path.GetExtension(file.FileName);
                    string FileName = fileNameOnly + "_" + code + extension; //File Name of buyer  
                    bool fileChecking = false;

                    //if (File.Exists(HttpContext.Current.Server.MapPath($"~/TEMP_DOCS/{FolderName}/") + FileName))
                    if (File.Exists(HttpContext.Current.Server.MapPath($"~/TEMP_DOCS/") + FileName) || File.Exists(HttpContext.Current.Server.MapPath($"~/BUYER_ATTACHMENTS/") + FileName))
                    {
                        alertMsg2("File already exists!", "info");
                    }
                    else
                    {
                        lblFileName.Text = FileName;
                        file.PostedFile.SaveAs(HttpContext.Current.Server.MapPath($"~/TEMP_DOCS/") + FileName); //File is saved in the Physical folder 
                                                                                                                //file.PostedFile.SaveAs(HttpContext.Current.Server.MapPath($"~/TEMP_DOCS/{FolderName}/") + FileName); //File is saved in the Physical folder 
                        btnPreview.Visible = true;
                        btnDelete.Visible = true;
                    }
                }
                else
                {
                    //rvReq.Enabled = false;
                    alertMsg2("Please choose file!", "info");
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

        protected void CustomValidator14_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //2023-06-15 : ADD VALIDATOR FOR SPOUSE TIN FORMAT--%>
            //Check if TIN is in correct format(###-###-###-###)
            bool isOK = Regex.IsMatch(txtSPOTinNo.Text, @"[0-9][0-9][0-9]\-[0-9][0-9][0-9]\-[0-9][0-9][0-9]\-[0-9][0-9][0-9]");
            if (!isOK)
            {
                RequiredFieldValidator13.Visible = false;
                args.IsValid = false;
            }
            else
            {
                RequiredFieldValidator13.Visible = true;
                args.IsValid = true;
            }
        }

        protected void btnSPAUpdate_ServerClick(object sender, EventArgs e)
        {

            int row = int.Parse(txtSPAID.Value);

            DataTable dt = (DataTable)ViewState["SPA"];

            //clear Session
            ViewState["SPA"] = null;


            foreach (DataRow dr in dt.Rows)
            {
                //update row of datatable
                if (int.Parse(dr["Id"].ToString()) == row)
                {
                    dr["Relationship"] = tRelationship.Value;
                    dr["LastName"] = txtSPALastName.Text.Trim().ToUpper();
                    dr["FirstName"] = txtSPAFirstName.Text.Trim().ToUpper();
                    dr["MiddleName"] = txtSPAMiddleName.Text.Trim().ToUpper();
                    dr["CivilStatus"] = ddSPACivilStatus.SelectedValue;
                    dr["YearsOfStay"] = string.IsNullOrEmpty(tSPAYearsOfStay.Value) ? "0" : tSPAYearsOfStay.Value;
                    dr["Address"] = string.IsNullOrEmpty(txtSPAAddress.Text) ? string.Empty : txtSPAAddress.Text.Trim().ToUpper();
                    dr["BirthDate"] = string.IsNullOrEmpty(dtSPABirthDate.Text) ? string.Empty : dtSPABirthDate.Text;
                    dr["BirthPlace"] = string.IsNullOrEmpty(txtSPABirthPlace.Text) ? string.Empty : txtSPABirthPlace.Text.Trim().ToUpper();
                    dr["Gender"] = ddSPAGender.SelectedValue;
                    dr["Citizenship"] = txtSPACitizenship.Value;
                    dr["Email"] = string.IsNullOrEmpty(txtSPAEmail.Text) ? string.Empty : txtSPAEmail.Text;
                    dr["TelNo"] = string.IsNullOrEmpty(txtSPATelNo.Text) ? string.Empty : txtSPATelNo.Text;
                    dr["MobileNo"] = string.IsNullOrEmpty(txtSPAMobile.Text) ? string.Empty : txtSPAMobile.Text;
                    dr["FB"] = string.IsNullOrEmpty(txtSPAFB.Text) ? string.Empty : txtSPAFB.Text;
                    dr["SPAFormDocument"] = lblSPAFileName.Text;
                }
            }

            ViewState["SPA"] = dt;

            //REFRESH GRIDVIEW
            gvSPACoBorrower.DataSource = dt;
            gvSPACoBorrower.DataBind();



            ClearAllModalFields();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "MsgSPACoBorrower_Hide();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "fixTab();", true);
        }

        protected void btnAdd_ServerClick1(object sender, EventArgs e)
        {

        }
    }
}
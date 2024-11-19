using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataCipher;
using System.Drawing;
using System.Collections;

namespace ABROWN_DREAMS
{
    public partial class UserManagement : Page
    {

        DirecWebService ws = new DirecWebService();
        SAPHanaAccess hana = new SAPHanaAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    LoadRole();
                    LoadUserList();
                    //LoadModuleList();
                    //Session["UserID"] = 1;
                }
                else
                {
                    tPassword.Attributes["value"] = tPassword.Text;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Log(Session["UserId"]?.ToString()
                    ,"UserManagement"
                    , "Critical"
                    , ex.Message
                    , ex.StackTrace);
            }            
        }

        void LoadUserList()
        {
            try
            {
                DataTable dt = ws.GetUsers().Tables["GetUsers"];
                gvUserList.DataSource = dt;
                gvUserList.DataBind();
                foreach (GridViewRow row in gvUserList.Rows)
                {
                    CheckBox chk = (row.Cells[0].FindControl("chkLicense") as CheckBox);
                    chk.Checked = Convert.ToBoolean(dt.Rows[row.RowIndex]["IsActive"].ToString() == "" ? false : true);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Log(Session["UserId"]?.ToString()
                    , "UserManagement"
                    , "Critical"
                    , ex.Message
                    , ex.StackTrace);
            }
        }

        void LoadModuleList()
        {
            gvModule.DataSource = hana.GetData(@"SELECT * FROM ""OMOD""", hana.GetConnection("SAOHana"));
            gvModule.DataBind();
        }

        void LoadRole()
        {
            try
            {
                gvRole.DataSource = null;
                gvRole.DataBind();
                ViewState["Roles"] = ws.GetUserRole().Tables["GetUserRole"];
                gvRole.DataSource = (DataTable)ViewState["Roles"];
                gvRole.DataBind();
            }
            catch (Exception ex)
            {
                ErrorHandler.Log(Session["UserId"]?.ToString()
                    , "UserManagement"
                    , "Critical"
                    , ex.Message
                    , ex.StackTrace);
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
                    , "UserManagement"
                   , "Low"
                   , Message
                   , $"User Management : Type of {type}.");
            }
           
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "showAlert();", true);
        }
        protected void btnSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                if (!string.IsNullOrWhiteSpace(txtSearch.Value))
                {
                    dt = hana.GetData($@"SELECT
	                         A.""ID""
	                         ,A.""Username""
	                         , CONCAT(B.""LastName"",CONCAT(', ',B.""FirstName"")) Name
	                         ,  B.""Email""
	                         , A.""IsLock""
	                         , C.""IsActive"" 
                        FROM 
	                        ""OUSR"" A 
                        INNER JOIN 
	                        ""OMBR"" B ON A.""ID"" = B.""UserID"" 
                        LEFT JOIN
	                        ""USR3"" C ON A.""ID"" = C.""UserID""
                        WHERE 
	                        A.""IsShow"" = TRUE 
	                        AND
	                        ((LOWER(B.""LastName"") LIKE '%{txtSearch.Value.ToLower()}%') OR (LOWER(B.""FirstName"") LIKE '%{txtSearch.Value.ToLower()}%') OR (LOWER(A.""Username"") LIKE '%{txtSearch.Value.ToLower()}%'))
                        ORDER BY 
	                        A.""IsLock"",  B.""LastName""", hana.GetConnection("SAOHana"));
                    gvUserList.DataSource = dt;
                    gvUserList.DataBind();
                }
                else
                {
                    LoadUserList();
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Log(Session["UserId"]?.ToString()
                    , "UserManagement"
                    , "Critical"
                    , ex.Message
                    , ex.StackTrace);
            }
        }

        protected void gvUserList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvUserList.PageIndex = e.NewPageIndex;
                DataTable dt = new DataTable();
                if (!string.IsNullOrWhiteSpace(txtSearch.Value))
                {
                    dt = hana.GetData($@"SELECT
	                         A.""ID""
	                         ,A.""Username""
	                         , CONCAT(B.""LastName"",CONCAT(', ',B.""FirstName"")) Name
	                         ,  B.""Email""
	                         , A.""IsLock""
	                         , C.""IsActive"" 
                        FROM 
	                        ""OUSR"" A 
                        INNER JOIN 
	                        ""OMBR"" B ON A.""ID"" = B.""UserID"" 
                        LEFT JOIN
	                        ""USR3"" C ON A.""ID"" = C.""UserID""
                        WHERE 
	                        A.""IsShow"" = TRUE 
	                        AND
	                        ((LOWER(B.""LastName"") LIKE '%{txtSearch.Value.ToLower()}%') OR (LOWER(B.""FirstName"") LIKE '%{txtSearch.Value.ToLower()}%') OR (LOWER(A.""Username"") LIKE '%{txtSearch.Value.ToLower()}%'))
                        ORDER BY 
	                        A.""IsLock"",  B.""LastName""", hana.GetConnection("SAOHana"));
                    gvUserList.DataSource = dt;
                    gvUserList.DataBind();
                }
                else
                {
                    LoadUserList();
                }

                LoadRole();
            }
            catch (Exception ex)
            {
                ErrorHandler.Log(Session["UserId"]?.ToString()
                    , "UserManagement"
                    , "Critical"
                    , ex.Message
                    , ex.StackTrace);
            }            
        }

        protected void btnUserAdd_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string Msg = string.Empty;
                if (string.IsNullOrEmpty(tLastName.Value) || string.IsNullOrEmpty(tFirstName.Value))
                {
                    //ScriptManager.RegisterStartupScript(this, GetType(), "hide", "hideCreateUser()", true);
                    alertMsg("Please dont leave first name or last name blank.", "warning");
                    return;
                }
                if (!string.IsNullOrEmpty(tPassword.Text) || !string.IsNullOrEmpty(tUserID.Value))
                {
                    DataTable dt = new DataTable();

                    if ((bool)Session["IsUpdate"] == true)
                    {
                        string UserID = (string)Session["GetUserID"];
                        dt = hana.GetData($@"SELECT 'True' FROM ""OUSR"" WHERE ""Username"" = '{tUserID.Value}' AND ""ID"" <> {UserID}", hana.GetConnection("SAOHana"));
                        if (DataAccess.Exist(dt) == false)
                        {
                            if (ws.SetUpOUSR(int.Parse(UserID), tLastName.Value, tFirstName.Value, tMiddleInitial.Value, tEmail.Value, tUserID.Value, Cryption.Encrypt($"{tUserID.Value}{tPassword.Text}")) == true && ws.DeleteRole(int.Parse(UserID)) == true)
                            {
                                string oRole = "";
                                foreach (GridViewRow row in gvRole.Rows)
                                {
                                    oRole = gvRole.Rows[row.RowIndex].Cells[2].Text;
                                    if (row.RowType == DataControlRowType.DataRow)
                                    {
                                        CheckBox chk = (row.Cells[0].FindControl("chkRow") as CheckBox);
                                        if (chk.Checked)
                                        { ws.SetRole(int.Parse(UserID), Cryption.Encrypt($"{UserID}{oRole}"), (int)Session["UserID"]); }
                                    }
                                }

                                //FOR OTHER PAGE GRIDVIEW
                                DataTable roles = (DataTable)ViewState["Roles"];
                                if (ViewState["CHECKED_ITEMS"] != null)
                                {
                                    ArrayList checkotherpage = (ArrayList)ViewState["CHECKED_ITEMS"];
                                    int sequence;
                                    int chksequence;
                                    foreach (DataRow row in roles.Rows)
                                    {
                                        sequence = Convert.ToInt32(row["Sequence"]);
                                        foreach (object i in checkotherpage)
                                        {
                                            chksequence = (int)i;
                                            if (sequence == chksequence)
                                            {
                                                oRole = row["Code"].ToString();
                                                ws.SetRole(int.Parse(UserID), Cryption.Encrypt($"{UserID}{oRole}"), (int)Session["UserID"]);
                                            }
                                        }
                                    }
                                }


                                alertMsg("Operation completed successfully.", "success");
                                LoadRole(); LoadUserList();
                                ScriptManager.RegisterStartupScript(this, GetType(), "hide", "hideCreateUser()", true);
                                ScriptManager.RegisterStartupScript(this, GetType(), "clear", "resetValueAddUser()", true);
                                tPassword.Attributes["value"] = string.Empty;
                                tEmail.Value = string.Empty;
                            }
                        }
                        else
                        {
                            //ScriptManager.RegisterStartupScript(this, GetType(), "hide", "hideCreateUser()", true);
                            alertMsg("Username name already exist.", "warning");
                        }
                    }
                    else
                    {
                        dt = hana.GetData($@"SELECT 'True' FROM ""OUSR"" WHERE ""Username"" = '{tUserID.Value}'", hana.GetConnection("SAOHana"));
                        if (DataAccess.Exist(dt) == false)
                        {
                            int UserID = ws.SetOUSR(tLastName.Value, tFirstName.Value, tMiddleInitial.Value, tEmail.Value, tUserID.Value, Cryption.Encrypt(tUserID.Value + tPassword.Text), out Msg);
                            if (UserID != 0)
                            {
                                UserID = Convert.ToInt32(DataAccess.GetData(hana.GetData(@"SELECT ""AutoKey"" FROM ""AKEY"" WHERE ""ObjectCode"" = 4;", hana.GetConnection("SAOHana")), 0, "AutoKey", "0").ToString());

                                if (ws.DeleteRole(UserID) == true || ws.DeleteMode(UserID) == true)
                                {
                                    string oRole = "";
                                    foreach (GridViewRow row in gvRole.Rows)
                                    {
                                        oRole = gvRole.Rows[row.RowIndex].Cells[2].Text;
                                        if (row.RowType == DataControlRowType.DataRow)
                                        {
                                            CheckBox chk = (row.Cells[0].FindControl("chkRow") as CheckBox);
                                            if (chk.Checked)
                                            {
                                                if (ws.SetRole(UserID, Cryption.Encrypt($"{UserID}{oRole}"), (int)Session["UserID"]) == true)
                                                {
                                                    dt = hana.GetData($@"SELECT ""FeatID"" FROM ""OMOD"" WHERE ""FeatRole"" = '{oRole}'", hana.GetConnection("SAOHana"));
                                                    foreach (DataRow dr in dt.Rows)
                                                    { ws.SetMode(UserID, Cryption.Encrypt($"{UserID}{dr["FeatID"].ToString()}"), (int)Session["UserID"]); }
                                                }
                                            }
                                        }
                                    }

                                    //FOR OTHER PAGE GRIDVIEW
                                    DataTable roles = (DataTable)ViewState["Roles"];
                                    if (ViewState["CHECKED_ITEMS"] != null)
                                    {
                                        ArrayList checkotherpage = (ArrayList)ViewState["CHECKED_ITEMS"];
                                        int sequence;
                                        int chksequence;
                                        foreach (DataRow row in roles.Rows)
                                        {
                                            sequence = Convert.ToInt32(row["Sequence"]);
                                            foreach (object i in checkotherpage)
                                            {
                                                chksequence = (int)i;
                                                if (sequence == chksequence)
                                                {
                                                    oRole = row["Code"].ToString();
                                                    if (ws.SetRole(UserID, Cryption.Encrypt($"{UserID}{oRole}"), (int)Session["UserID"]) == true)
                                                    {
                                                        dt = hana.GetData($@"SELECT ""FeatID"" FROM ""OMOD"" WHERE ""FeatRole"" = '{oRole}'", hana.GetConnection("SAOHana"));
                                                        foreach (DataRow dr in dt.Rows)
                                                        { ws.SetMode(UserID, Cryption.Encrypt($"{UserID}{dr["FeatID"].ToString()}"), (int)Session["UserID"]); }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                ScriptManager.RegisterStartupScript(this, GetType(), "hide", "hideCreateUser()", true);
                                ViewState["CHECKED_ITEMS"] = null;
                                ViewState["Name"] = null;
                                alertMsg("Operation completed successfully.", "success");
                                LoadRole(); LoadUserList();
                                ScriptManager.RegisterStartupScript(this, GetType(), "clear", "resetValueAddUser()", true);
                                tPassword.Attributes["value"] = string.Empty;
                                tEmail.Value = string.Empty;
                            }
                            else
                            {
                                //ScriptManager.RegisterStartupScript(this, GetType(), "hide", "hideCreateUser()", true);
                                alertMsg(Msg, "warning");
                            }
                        }
                        else
                        {
                            //ScriptManager.RegisterStartupScript(this, GetType(), "hide", "hideCreateUser()", true);
                            alertMsg("Username name already exist.", "warning");
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "hide", "hideCreateUser()", true);
                    alertMsg("Please dont leave username/password blank.", "warning");
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Log(Session["UserId"]?.ToString()
                    , "UserManagement"
                    , "Critical"
                    , ex.Message
                    , ex.StackTrace);
            }
        }

        protected void SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox ChkBoxHeader = (CheckBox)gvRole.HeaderRow.FindControl("SelectAll");
                foreach (GridViewRow row in gvRole.Rows)
                {
                    CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkRow");
                    if (ChkBoxHeader.Checked == true)
                    { ChkBoxRows.Checked = true; }
                    else
                    { ChkBoxRows.Checked = false; }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Log(Session["UserId"]?.ToString()
                    , "UserManagement"
                    , "Critical"
                    , ex.Message
                    , ex.StackTrace);
            }            
        }

        protected void SelectMod_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox ChkBoxHeader = (CheckBox)gvModule.HeaderRow.FindControl("SelectMod");
                foreach (GridViewRow row in gvModule.Rows)
                {
                    CheckBox ChkBoxRows = (CheckBox)row.FindControl("chk");
                    if (ChkBoxHeader.Checked == true)
                    { ChkBoxRows.Checked = true; }
                    else
                    { ChkBoxRows.Checked = false; }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Log(Session["UserId"]?.ToString()
                    , "UserManagement"
                    , "Critical"
                    , ex.Message
                    , ex.StackTrace);
            }            
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                string Name = btn.CommandArgument;
                Session["GetUserID"] = Name;
                ViewState["Name"] = Name;

                lblAddUser.InnerText = "Update user";
                lblAddUpdateUser.InnerText = "Update";
                Session["IsUpdate"] = true;

                DataTable dt = new DataTable();
                dt = ws.GetUserByID(int.Parse(Name)).Tables["GetUserByID"];

                if (DataAccess.Exist(dt) == true)
                {
                    string oPass = Cryption.Decrypt((string)DataAccess.GetData(dt, 0, "Password", ""));
                    string oUser = (string)DataAccess.GetData(dt, 0, "UserName", "");
                    int cnt = oPass.Length - oUser.Length;
                    oPass = oPass.Substring(oUser.Length, cnt);
                    tLastName.Value = (string)DataAccess.GetData(dt, 0, "LastName", "");
                    tFirstName.Value = (string)DataAccess.GetData(dt, 0, "FirstName", "");
                    tMiddleInitial.Value = (string)DataAccess.GetData(dt, 0, "MiddleName", "");
                    tEmail.Value = (string)DataAccess.GetData(dt, 0, "Email", "");
                    tUserID.Value = oUser;
                    tPassword.Attributes["value"] = oPass;
                }

                LoadRole();
                dt = ws.GetUserRolesByID(int.Parse(Name)).Tables["GetUserRolesByID"];

                foreach (GridViewRow rows in gvRole.Rows)
                {
                    CheckBox chk = (rows.Cells[0].FindControl("chkRow") as CheckBox);
                    foreach (DataRow dr in dt.Rows)
                    {
                        string Mode = Cryption.Decrypt(dr["CodeEncrypt"].ToString());
                        int cnt = Mode.Length - Name.Length;
                        Mode = Mode.Substring(Name.Length, cnt);
                        if (Mode == gvRole.Rows[rows.RowIndex].Cells[2].Text)
                        { chk.Checked = true; }
                    }

                }
                ScriptManager.RegisterStartupScript(this, GetType(), "show", "showCreateUser()", true);
                btnSelect_Click(sender, e);
            }
            catch (Exception ex)
            {
                ErrorHandler.Log(Session["UserId"]?.ToString()
                    , "UserManagement"
                    , "Critical"
                    , ex.Message
                    , ex.StackTrace);
            }
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                string Name = btn.CommandArgument;
                Session["GetUserID"] = Name;
                string UserName = ws.WebUsername(int.Parse(Name));
                foreach (GridViewRow row in gvUserList.Rows)
                {
                    if (gvUserList.Rows[row.RowIndex].Cells[0].Text == UserName)
                    { row.BackColor = ColorTranslator.FromHtml("#A1DCF2"); }
                    else
                    { row.BackColor = ColorTranslator.FromHtml("#FFFFFF"); }
                }

                LoadModuleList();
                foreach (GridViewRow rows in gvModule.Rows)
                {
                    CheckBox chk = (rows.Cells[0].FindControl("chk") as CheckBox);
                    DataTable dt = new DataTable();
                    dt = hana.GetData($@"SELECT * FROM ""USR1"" WHERE ""UserID"" = {Name}", hana.GetConnection("SAOHana"));
                    foreach (DataRow dr in dt.Rows)
                    {
                        string Mode = Cryption.Decrypt(dr["FeatEncrypt"].ToString());
                        int cnt = Mode.Length - Name.Length;
                        Mode = Mode.Substring(Name.Length, cnt);
                        if (Mode == gvModule.Rows[rows.RowIndex].Cells[1].Text)
                        { chk.Checked = true; }
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Log(Session["UserId"]?.ToString()
                    , "UserManagement"
                    , "Critical"
                    , ex.Message
                    , ex.StackTrace);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string GetUserID = (string)Session["GetUserID"];
                bool ret = true;
                if (ws.DeleteMode(int.Parse(GetUserID)) == true)
                {
                    foreach (GridViewRow rows in gvModule.Rows)
                    {
                        CheckBox chk = (rows.Cells[0].FindControl("chk") as CheckBox);
                        if (chk.Checked)
                        {
                            ret = ws.SetMode(int.Parse(GetUserID), Cryption.Encrypt($"{int.Parse(GetUserID)}{gvModule.Rows[rows.RowIndex].Cells[1].Text}"), (int)Session["UserID"]);
                            if (ret == false)
                            { break; }
                        }
                    }

                    if (ret == true)
                    { alertMsg("Operation completed successfully.", "success"); }
                    else { alertMsg("Error has been encountered.", "warning"); }
                }
                else { alertMsg("Error has been encountered.", "warning"); }
            }
            catch (Exception ex)
            {
                ErrorHandler.Log(Session["UserId"]?.ToString()
                    , "UserManagement"
                    , "Critical"
                    , ex.Message
                    , ex.StackTrace);
            }            
        }

        protected void btnAddUser_ServerClick(object sender, EventArgs e)
        { lblAddUser.InnerText = "Add user"; lblAddUpdateUser.InnerText = "Add"; Session["IsUpdate"] = false; ScriptManager.RegisterStartupScript(this, GetType(), "clear", "resetValueAddUser()", true); LoadRole(); ViewState["CHECKED_ITEMS"] = null; ViewState["Name"] = null; }

        protected void gvUserList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int i = int.Parse(e.CommandArgument.ToString());

                if (e.CommandName.Equals("Sel"))
                {
                    ViewState["CHECKED_ITEMS"] = null;

                    int id = int.Parse(gvUserList.Rows[i].Cells[1].Text);
                    Session["GetUserID"] = id.ToString();
                    string Name = gvUserList.Rows[i].Cells[2].Text;
                    LoadModuleList();
                    foreach (GridViewRow rows in gvModule.Rows)
                    {
                        CheckBox chk = (rows.Cells[0].FindControl("chk") as CheckBox);
                        DataTable dt = new DataTable();
                        dt = hana.GetData($@"SELECT * FROM ""USR1"" WHERE ""UserID"" = {id}", hana.GetConnection("SAOHana"));
                        foreach (DataRow dr in dt.Rows)
                        {
                            string Mode = Cryption.Decrypt(dr["FeatEncrypt"].ToString());
                            int cnt = Mode.Length - id.ToString().Length;
                            Mode = Mode.Substring(id.ToString().Length, cnt);
                            if (Mode == gvModule.Rows[rows.RowIndex].Cells[1].Text)
                            { chk.Checked = true; }
                        }
                    }

                    //** highlight selected row **//
                    foreach (GridViewRow selectedrow in gvUserList.Rows)
                    {
                        if (selectedrow.RowIndex == i)
                        { selectedrow.BackColor = System.Drawing.ColorTranslator.FromHtml("#A1DCF2"); }
                        else
                        { selectedrow.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF"); }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Log(Session["UserId"]?.ToString()
                    , "UserManagement"
                    , "Critical"
                    , ex.Message
                    , ex.StackTrace);
            }            
        }

        protected void gvRole_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                RememberOldValues();
                gvRole.DataSource = ws.GetUserRole().Tables["GetUserRole"]; gvRole.PageIndex = e.NewPageIndex;
                LoadRole();
                if (ViewState["Name"] != null)
                {
                    string Name = ViewState["Name"].ToString();
                    DataTable dt = ws.GetUserRolesByID(int.Parse(Name)).Tables["GetUserRolesByID"];
                    foreach (GridViewRow rows in gvRole.Rows)
                    {
                        CheckBox chk = (rows.Cells[0].FindControl("chkRow") as CheckBox);
                        foreach (DataRow dr in dt.Rows)
                        {
                            string Mode = Cryption.Decrypt(dr["CodeEncrypt"].ToString());
                            int cnt = Mode.Length - Name.Length;
                            Mode = Mode.Substring(Name.Length, cnt);
                            if (Mode == gvRole.Rows[rows.RowIndex].Cells[2].Text)
                            { chk.Checked = true; }
                        }
                    }
                }
                RePopulateValues();
            }
            catch (Exception ex)
            {
                ErrorHandler.Log(Session["UserId"]?.ToString()
                    , "UserManagement"
                    , "Critical"
                    , ex.Message
                    , ex.StackTrace);
            }            
        }

        private void RememberOldValues()
        {
            try
            {
                ArrayList categoryIDList = new ArrayList();
                int index = -1;
                int count;
                foreach (GridViewRow row in gvRole.Rows)
                {
                    count = gvRole.DataKeys.Count;
                    index = (int)gvRole.DataKeys[row.RowIndex].Value;
                    bool result = ((CheckBox)row.FindControl("chkRow")).Checked;

                    // Check in the Session
                    if (ViewState["CHECKED_ITEMS"] != null)
                        categoryIDList = (ArrayList)ViewState["CHECKED_ITEMS"];
                    if (result)
                    {
                        if (!categoryIDList.Contains(index))
                            categoryIDList.Add(index);
                    }
                    else
                        categoryIDList.Remove(index);
                }
                if (categoryIDList != null && categoryIDList.Count > 0)
                    ViewState["CHECKED_ITEMS"] = categoryIDList;
            }
            catch (Exception ex)
            {
                ErrorHandler.Log(Session["UserId"]?.ToString()
                    , "UserManagement"
                    , "Critical"
                    , ex.Message
                    , ex.StackTrace);
            }            
        }

        private void RePopulateValues()
        {
            try
            {
                ArrayList categoryIDList = (ArrayList)ViewState["CHECKED_ITEMS"];
                if (categoryIDList != null)
                {
                    if (categoryIDList.Count > 0)
                    {
                        foreach (GridViewRow row in gvRole.Rows)
                        {
                            int index = (int)gvRole.DataKeys[row.RowIndex].Value;
                            if (categoryIDList.Contains(index))
                            {
                                CheckBox myCheckBox = (CheckBox)row.FindControl("chkRow");
                                myCheckBox.Checked = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Log(Session["UserId"]?.ToString()
                    , "UserManagement"
                    , "Critical"
                    , ex.Message
                    , ex.StackTrace);
            }            
        }
    }
}
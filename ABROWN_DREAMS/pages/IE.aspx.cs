using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSXML2;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Collections.Specialized;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serialization.Json;
using System.Linq;
using RestSharp.Authenticators;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;

namespace MDC_REALITY
{
    public partial class IE : Page
    {
        DirecWebService ws = new DirecWebService();
        //private static XMLHTTP60 ServiceLayer { get; set; }
        //private static readonly HttpClient client = new HttpClient();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtProjId.Value = ConfigurationManager.AppSettings["PRJCode"].ToString();
                //txtProjName.Value = ConfigurationManager.AppSettings["PRJName"].ToString();

                DataTable dtProj = new DataTable();
                dtProj = DataAccess.Select("Addon", $"SELECT PrjImage,ImgWidth,ImgHeight FROM OPRJ Where PrjCode = '{txtProjId.Value}' AND PrjImage IS NOT NULL");

                if (dtProj.Rows.Count > 0)
                {
                    string imgwidth = dtProj.Rows[0][1].ToString();
                    string imgheight = dtProj.Rows[0][2].ToString();
                    //load block
                    imgProjectWidth.Value = imgwidth;
                    imgProjectHeight.Value = imgheight;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "imgProject", "drawProjectMap();", true);
                //close modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "closeProjectList();", true);
            }
        }
        void LoadStatus()
        {
            Session["dtDocumentStatus"] = ws.GetDocumentStatus().Tables["DocumentStatus"];
            LoadData(gvBlockColor, "dtDocumentStatus");
        }
        void LoadData(GridView gv, string session)
        {
            gv.DataSource = (DataTable)Session[session];
            gv.DataBind();
        }

        protected void btnExport_ServerClick(object sender, EventArgs e)
        {

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
        }

        protected void btnShowProj_ServerClick(object sender, EventArgs e)
        {
            LoadSAPProjects();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "projList", "showProjList();", true);
        }
        void LoadSAPProjects()
        {
            if (gvProjectList.Rows.Count == 0)
            {
                gvProjectList.DataSource = ws.GetSAPProjects();
                gvProjectList.DataBind();
            }
        }

        protected void bSearch_Click(object sender, EventArgs e)
        {
            gvProjectList.DataSource = ws.SearchSAPProjects(txtSearch.Value);
            gvProjectList.DataBind();
        }

        protected void gvProjectList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int row = int.Parse(e.CommandArgument.ToString());
            if (e.CommandName.Equals("Sel"))
            {
                string projCode = string.IsNullOrEmpty(gvProjectList.Rows[row].Cells[0].Text) ? "PARK INFINA" : gvProjectList.Rows[row].Cells[0].Text;
                string projName = string.IsNullOrEmpty(gvProjectList.Rows[row].Cells[1].Text) ? "PARK INFINA" : gvProjectList.Rows[row].Cells[1].Text;

                txtProjId.Value = projCode;
                //txtProjName.Value = projName;

                DataTable dtProj = new DataTable();
                dtProj = DataAccess.Select("Addon", $"SELECT PrjImage,ImgWidth,ImgHeight FROM OPRJ Where PrjCode = '{txtProjId.Value}' AND PrjImage IS NOT NULL");

                if (dtProj.Rows.Count > 0)
                {
                    string imgwidth = dtProj.Rows[0][1].ToString();
                    string imgheight = dtProj.Rows[0][2].ToString();
                    //load block
                    imgProjectWidth.Value = imgwidth;
                    imgProjectHeight.Value = imgheight;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "imgProject", "drawProjectMap();", true);
                //close modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "closeProjectList();", true);
            }
        }

        protected void btnLotPreview_ServerClick(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = ws.Select($"SELECT ImgWidth,ImgHeight FROM PRJ1 WHERE PrjCode = '{txtProjId.Value}' AND Block = '{txtSelectedBlock.Value}'", "PRJ1", "Addon").Tables["PRJ1"];
            txtLotWidthPreview.Value = (string)DataAccess.GetData(dt, 0, "ImgWidth", "0");
            txtLotHeightPreview.Value = (string)DataAccess.GetData(dt, 0, "ImgHeight", "0");

            ScriptManager.RegisterStartupScript(this, this.GetType(), "imgProject", "drawProjectMap();", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "drawLot", "drawLotPreview();", true);
            //show modal
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "show", "showLotPreview();", true);


            //DataTable dt = new DataTable();
            //dt = ws.Select($"SELECT ImgWidth,ImgHeight FROM PRJ1 WHERE PrjCode = '{hPrjCode.Value}' AND Block = '{tBlock.Value}'", "PRJ1", "Addon").Tables["PRJ1"];
            //hLotWidth.Value = (string)DataAccess.GetData(dt, 0, "ImgWidth", "0");
            //hLotHeight.Value = (string)DataAccess.GetData(dt, 0, "ImgHeight", "0");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "drawLot", "drawLotMap();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "MsgLotList_Show", "MsgLotList_Show();", true);
            LoadStatus();

        }

        protected void gvProjectList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProjectList.DataSource = ws.GetSAPProjects();
            gvProjectList.PageIndex = e.NewPageIndex;
            gvProjectList.DataBind();
        }

        protected void bGenerate_ServerClick(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Gen", "MsgLotList_Hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "imgProject", "drawProjectMap();", true);

            DataTable dt = new DataTable();
            dt = ws.GetHouseStatus(txtProjId.Value, txtSelectedBlock.Value, txtSelectedLot.Value, "", "").Tables["GetHouseStatus"];
            if (DataAccess.Exist(dt) == true)
            {
                string oHouseCode = (string)DataAccess.GetData(dt, 0, "Code", "");
                Session["HouseStatusCode"] = oHouseCode;
                //tModel.Value = (string)DataAccess.GetData(dt, 0, "Name", "");

                if (oHouseCode == "")
                { Session["IsGetHouse"] = true; }

                Session["ModelName"] = (string)DataAccess.GetData(dt, 0, "U_prModelName", "");
                string oCode = (string)DataAccess.GetData(dt, 0, "U_prHoPropType", "");
                Session["IRate"] = double.Parse(DataAccess.GetData(dt, 0, "IRate", "0").ToString());

                Session["HouseStatus"] = oCode;
                DataTable dt1 = new DataTable();
                dt1 = ws.Select($"SELECT Name,U_ThresholdAmt FROM [@HOUSE_PROP] WHERE Code = '{oCode}'", "HOUSE_PROP", "SAP").Tables["HOUSE_PROP"];
                //tHouseStatus.Value = (string)DataAccess.GetData(dt1, 0, "Name", "");

                Session["ThresholdAmt"] = (string)DataAccess.GetData(dt1, 0, "U_ThresholdAmt", "0");

                oCode = (string)DataAccess.GetData(dt, 0, "U_prSize", "");

                if (oCode == "")
                { Session["IsGetSize"] = true; }
                else
                {
                    Session["tSize"] = oCode;
                    //tSize.Value = ws.GetUFD1Name("OITT", oCode, 4);
                }
                oCode = (string)DataAccess.GetData(dt, 0, "U_prHoFeat", "");
                if (oCode == "")
                { Session["IsGetFeat"] = true; }
                else
                {
                    Session["tFeature"] = oCode;
                    //tFeature.Value = (string)DataAccess.GetData(ws.Select($"SELECT Name FROM [@HOUSE_FEAT] WHERE Code = '{oCode}'", "HOUSE_FEAT", "SAP").Tables["HOUSE_FEAT"], 0, "Name", "");
                }
                //tResrvFee.Text = SystemClass.ToNumeric((string)DataAccess.GetData(dt, 0, "ResFee", "0"));
            }
            else
            {
                Session["IsGetHouse"] = true;
                Session["IsGetSize"] = true;
                Session["IsGetFeat"] = true;
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "MsgHouseList", "MsgHouseList_Show();", true);
            gvHouseList.DataSource = ws.GetHouseModel(txtProjId.Value, (string)Session["ModelName"]).Tables["GetHouseModel"];
            gvHouseList.DataBind();
        }

        protected void bPicPreview_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string Code = GetID.CommandArgument;
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgHouseList", "MsgHouseList_Hide();", true);
            gvPicPreview.DataSource = ws.GetHousePicture(Code).Tables["GetHousePicture"];
            gvPicPreview.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgPicPreview", "MsgPicPreview_Show();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "imgProject", "drawProjectMap();", true);
        }

        protected void bChooseHouse_Click(object sender, EventArgs e)
        {
            LinkButton GetID = (LinkButton)sender;
            string Code = GetID.CommandArgument;
            DataTable dt = new DataTable();
            dt = ws.GetHousePicture(Code).Tables["GetHousePicture"];
            Session["HouseStatusCode"] = (string)DataAccess.GetData(dt, 0, "Code", "");
            //tModel.Value = (string)DataAccess.GetData(dt, 0, "Name", "");

            //tResrvFee.Text = SystemClass.ToNumeric((string)DataAccess.GetData(dt, 0, "U_ResFee", "0"));
            ScriptManager.RegisterStartupScript(this, GetType(), "MsgHouseList", "MsgHouseList_Hide();", true);
        }
        protected void btnIE_ServerClick(object sender, EventArgs e)
        {
            try
            {

                //ID AND PASSWORD FOR API
                string clientID = "sb-61520a89-44c5-4d1d-a5bb-05a70113c0d6!b15828|foundation-std-mlftrial!b3410";
                string clientsecret = "oiyfTze9GQgK4S+uX2FwBJmpHXs=";
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls11;


                //GET TOKEN TOKEN
                var client = new RestClient("https://p2000552537trial.authentication.eu10.hana.ondemand.com/oauth/token?grant_type=client_credentials");
                client.Authenticator = new HttpBasicAuthenticator(clientID, clientsecret);
                var request = new RestRequest(Method.GET);

                request.AddParameter("grant_type", "client_credentials");
                IRestResponse response = client.Execute(request);

                string access_token = response.Content.ToString();
                var token = JsonHelper.GetJsonValue(access_token, "access_token");




                //POST IMAGE WITH BEARER TOKEN
                client = new RestClient("https://mlftrial-scene-text-recognition.cfapps.eu10.hana.ondemand.com/api/v2/image/scene-text-recognition");
                client.AddDefaultHeader("Authorization", "Bearer " + token);
                var request2 = new RestRequest(Method.POST);
                request2.AddFile("files", @"C:\\Users\\DIREC0046\\Desktop\\IE PRESENTATION\\SDP2.png");
                IRestResponse response2 = client.Execute(request2);

                //Final Result in JSON
                string result2 = response2.Content.ToString();

                //parse JSON to JObject
                JObject rss = JObject.Parse(result2);
                JObject rss2 = JObject.Parse(rss["predictions"][0].ToString());
                int ctr = 0;

                DataTable dt = new DataTable();
                //dt = ws.Select($"SELECT Code, Name FROM OLST WHERE GrpCode = 'FS' AND Name = '{tFinancing.Value}'", "OLST", "Addon").Tables["OLST"];



                foreach (var x in rss2)
                {
                    

                    string name = x.Key;
                    JToken value = x.Value;

                    ctr = ctr + 1;
                }





            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('" + ex.Message + "');", true);
            }
        }

    }
}
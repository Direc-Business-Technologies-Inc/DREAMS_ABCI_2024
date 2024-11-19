using System;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABROWN_DREAMS.pages
{
    public partial class ProjectPerLot : System.Web.UI.Page
    {
        DirecWebService ws = new DirecWebService();
        SAPHanaAccess hana = new SAPHanaAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Page.Form.Attributes.Add("enctype", "multipart/form-data");
            if (!IsPostBack)
            {
                Session["LotRadius"] = 15;
                Session["LotMode"] = "Add";
                Session["BlockWidth"] = 0;
                Session["BlockHeight"] = 0;
                Session["SelectedLot"] = "";
                Session["AddTempLot"] = false;
                //hide adding of block and lot
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "showBlockListDiv();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "showLotListDiv();", true);
            }
        }
        void LoadProjectList()
        {
            gvProjectList.DataSource = ws.GetSAPProjects();
            gvProjectList.DataBind();
        }
        void LoadSAPProjects()
        {
            if (gvProjectList.Rows.Count == 0)
            {
                gvProjectList.DataSource = ws.GetSAPProjects();
                gvProjectList.DataBind();
            }
        }
        void LoadBlockList()
        {
            //gvProjectBlocks.DataSource = ws.GetBlockList(txtProjId.Value);
            //gvProjectBlocks.DataBind();
            //FooterTotalBlocks();

            //gvSAPBlocks.DataSource = ws.GetBlockLotsList(txtProjId.Value);
            //gvSAPBlocks.DataBind();

            gvBLockList.DataSource = ws.GetBlockList(txtProjId.Value);
            gvBLockList.DataBind();
        }

        void FooterTotalBlocks()
        {
            //gvProjectBlocks.FooterRow.Cells[0].Text = "Count:";
            //gvProjectBlocks.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;

            //gvProjectBlocks.FooterRow.Cells[1].Text = gvProjectBlocks.Rows.Count.ToString();
            //gvProjectBlocks.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;
        }
        void closeconfirm()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "close", "closeConfirmation();", true);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            confirmation("Are you sure you want to cancel?", "cancel");
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
        //void alert(string message, string type)
        //{
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", $"swal('','{message}','{type}')", true);
        //}
        void confirmation(string body, string type)
        {
            Session["ConfirmType"] = type;
            lblConfirmationInfo.Text = body;
            //Show Modal
            ScriptManager.RegisterStartupScript(this, this.GetType(), "confirmation", "showConfirmation();", true);

            ReloadCanvas();
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtProjId.Value))
            {
                alertMsg("Please choose project", "warning");
            }
            else
            {
                //** check if project has image **//
                //check if existing
                DataTable dtProj = new DataTable();
                dtProj = hana.GetData($@"SELECT ""PrjImage"",""ImgWidth"",""ImgHeight"" FROM ""OPRJ"" Where ""PrjCode"" = '{txtProjId.Value}' AND ""PrjImage"" IS NOT NULL", hana.GetConnection("SAOHana"));

                if (dtProj.Rows.Count > 0)
                {
                    string imgwidth = dtProj.Rows[0][1].ToString();
                    string imgheight = dtProj.Rows[0][2].ToString();
                    //load block
                    LoadBlockList();
                    imgProjectWidth.Value = imgwidth;
                    imgProjectHeight.Value = imgheight;
                    NextTab("project_block");
                    //draw project map to canvas
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "imgProject", "drawProjectMap();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "imgPrvw", "showImgPrvw();", true);
                }
                else
                {
                    alertMsg("Please upload project image", "warning");
                }
            }
        }
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            PrevTab("project");
        }
        protected void imgBlockMap_Click(object sender, ImageClickEventArgs e)
        {
            Session["X"] = e.X.ToString();
            Session["Y"] = e.Y.ToString();

            int x = e.X;
            int y = e.Y;

            txtPixelX.Text = e.X.ToString();
            txtPixelY.Text = e.Y.ToString();
            //Show Modal
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showBlockMap();", true);
        }
        protected void btnSaveBlock_Click(object sender, EventArgs e)
        {
            //check if block per project is existing
            DataTable dt = hana.GetData($@"SELECT ""name"" FROM ""PRJ1"" Where ""PrjCode"" = '{txtProjId.Value}' and ""Block"" = '{txtBlock.Text}'", hana.GetConnection("SAOHana"));
            if (dt.Rows.Count > 0)
            {
                if (FileUpload2.HasFile)
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(FileUpload2.PostedFile.InputStream);
                    int width = img.Width;
                    int height = img.Height;
                    //Convert Img to Byte
                    byte[] img_byte = DataAccess.ConvertImageToByteArray(img, ImageFormat.Jpeg);
                    //UPDATE Block
                    if (ws.UpdateProjectBlock("Addon", txtProjId.Value, txtBlock.Text, img_byte, width, height, Convert.ToInt32(txtPixelX.Text), Convert.ToInt32(txtPixelY.Text)) == true)
                    {
                        alertMsg("Block updated successfully", "success");
                        DeleteBlockTemp(txtProjId.Value, txtBlock.Text);
                    }
                    else
                    {
                        alertMsg("Updating of block failed", "error");
                    }
                }
                else
                {
                    if (ws.UpdateProjectBlockNoImage("Addon", txtProjId.Value, txtBlock.Text, Convert.ToInt32(txtPixelX.Text), Convert.ToInt32(txtPixelY.Text)) == true)
                    {
                        alertMsg("Block updated successfully", "success");
                        DeleteBlockTemp(txtProjId.Value, txtBlock.Text);
                    }
                    else
                    {
                        alertMsg("Updating of block failed", "error");
                    }
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(txtBlock.Text))
                {
                    //if location not selected
                    if (!String.IsNullOrEmpty(txtPixelX.Text) && !String.IsNullOrEmpty(txtPixelY.Text))
                    {
                        //if image is empty
                        if (FileUpload2.HasFile)
                        {
                            System.Drawing.Image img = System.Drawing.Image.FromStream(FileUpload2.PostedFile.InputStream);
                            //Convert Img to Byte
                            byte[] img_byte = DataAccess.ConvertImageToByteArray(img, ImageFormat.Jpeg);
                            int width = img.Width;
                            int height = img.Height;
                            ws.AddNewProjectBlock(txtProjId.Value, txtBlock.Text, txtBlockDescription.Text, img_byte, width, height, Convert.ToInt32(txtPixelX.Text), Convert.ToInt32(txtPixelY.Text));
                            //load block list
                            LoadBlockList();
                            alertMsg("Block updated successfully", "success");
                            DeleteBlockTemp(txtProjId.Value, txtBlock.Text);
                        }
                        else
                        {
                            alertMsg("Please select image for selected block", "warning");
                        }
                    }
                    else
                    {
                        alertMsg("Please select block location", "warning");
                    }
                }
                else
                {
                    alertMsg("Block No. should not be empty", "warning");
                }
            }

            LoadBlockList();
            //show block list div
            ScriptManager.RegisterStartupScript(this, this.GetType(), "blocklist", "showBlockListDiv();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "imgPrvw1", "drawProjectMap();", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "imgPrvw2", "drawBlockMap();", true);

        }
        protected void ImagePreview_Click(object sender, ImageClickEventArgs e)
        {
            Session["X"] = e.X.ToString();
            Session["Y"] = e.Y.ToString();
            int x = e.X;
            int y = e.Y;
            txtPixelX.Text = e.X.ToString();
            txtPixelY.Text = e.Y.ToString();
        }
        protected void btnYes_Click(object sender, EventArgs e)
        {
            //cancel trans
            if ((string)Session["ConfirmType"] == "cancel")
            {

            }
            //remove block
            else if ((string)Session["ConfirmType"] == "removeBlock")
            {
                if (ws.DeleteBlock(txtProjId.Value, (string)Session["SelectedBlock"]) == true)
                {
                    closeconfirm();
                    //successful
                    alertMsg("Block removed successfully", "success");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "NextTab();", true);

                    LoadBlockList();
                }
                else
                {
                    closeconfirm();
                    alertMsg("Error in removing block", "error");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "NextTab();", true);
                }
            }

            else if ((string)Session["ConfirmType"] == "removeblockimage")
            {
                if (hana.Execute($@"UPDATE ""PRJ1"" SET ""BlockImage"" = NULL  Where ""Block"" ='{(string)Session["selectedblockrow"]}' and ""PrjCode"" = '{txtProjId.Value}'", hana.GetConnection("SAOHana")))
                {
                    LoadBlockList();
                    closeconfirm();
                    alertMsg("Block image successfully removed", "success");
                }
                else
                {
                    closeconfirm();
                    alertMsg("Error in removing block image", "error");
                }
            }
            else if ((string)Session["ConfirmType"] == "removeblocklocation")
            {
                //** delete block location
                if (hana.Execute($@"UPDATE ""PRJ2"" SET ""type"" = NULL,""left"" = NULL,""top"" = NULL Where ""PrjCode"" = '{txtProjId.Value}' and""Block"" ='{txtSelectedProjectBlock.Value}' and ""PrjCode"" = '{txtProjId.Value}'  and ""Lot""='{txtSelectedProjectLot.Value}'", hana.GetConnection("SAOHana")))
                {
                    LoadBlockList();
                    closeconfirm();
                    txtSelectedProjectBlock.Value = string.Empty;
                }
            }

            ReloadCanvas();
        }
        void ReloadCanvas()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "imgPrvw1", "drawProjectMap();", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "imgPrvw2", "drawBlockMap();", true);
        }
        protected void btnPrev2_Click(object sender, EventArgs e)
        {
            PrevTab("project_block");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "imgPrvw", "drawProjectMap();", true);
        }
        protected void btnNext2_Click(object sender, EventArgs e)
        {
            if ((bool)Session["AddTempLot"] == true)
            {
                ReloadCanvas();
                alertMsg("Please save lot location!", "warning");
            }
            else
            {
                ReloadCanvas();
                NextTab("project_complete");
            }
        }
        protected void gvBLockList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int row = int.Parse(e.CommandArgument.ToString());
            if (e.CommandName.Equals("Select"))
            {
                string projCode = gvBLockList.Rows[row].Cells[1].Text;
                string Block = gvBLockList.Rows[row].Cells[2].Text;
                string BlockDesc = gvBLockList.Rows[row].Cells[3].Text;
                DataTable dt = new DataTable();
                dt = hana.GetData($@"SELECT ""Block"",""PrjCode"",""BlockImage"",""ImgWidth"",""ImgHeight"" FROM ""PRJ1"" Where ""PrjCode"" = '{projCode}' and ""Block""= '{Block}' and ""BlockImage"" is not null", hana.GetConnection("SAOHana"));



                //if (dt.Rows.Count > 0)
                //{
                //    Session["Mode"] = "Add";

                //    txtBlockLot.Value = Block;
                //    //Load Image File
                //    string imageUrl = "~/Handler/BlockPreview.ashx?id=" + projCode + "&Block=" + Block;
                //    //canvas1.ImageUrl = Page.ResolveUrl(imageUrl);

                //    string width = dt.Rows[0][3].ToString();
                //    string height = dt.Rows[0][4].ToString();


                //    //delete tmp lot
                //    //DeleteLotTemp(projCode, Block);
                //    //bind lot
                //    //GetBlocksLots(projCode, Block);
                //    //close modal
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "closeBlockList();", true);

                //}
                //else
                //{
                //    //close modal
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "closeBlockList();", true);
                //    alertMsg("Invalid Block", "error");
                //}
                txtBlockLot.Value = Block;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "closeBlockList();", true);
                GetBlocksLots(projCode, Block);
            }
           
            ReloadCanvas();
        }
        //protected void btnNext3_Click(object sender, EventArgs e)
        //{
        //    if ((bool)Session["AddTempLot"] == true)
        //    {
        //        ReloadCanvas();
        //        alertMsg("Please save lot location!", "warning");
        //    }
        //    else
        //    {
        //        ReloadCanvas();
        //        NextTab("project_complete");
        //    }
        //}
        //protected void btnPrev3_Click(object sender, EventArgs e)
        //{
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "imgPrvw", "drawProjectMap();", true);
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "imgPrvw", "drawBlockMap();", true);
        //    PrevTab("project_block");
        //}
        protected void btnPrev4_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "imgPrvw", "drawProjectMap();", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "imgPrvw", "drawBlockMap();", true);
            PrevTab("project_block");
        }
        //protected void imgLot_Click(object sender, ImageClickEventArgs e)
        //{
        //    Session["LotX"] = e.X.ToString();
        //    Session["LotY"] = e.Y.ToString();

        //    int x = e.X;
        //    int y = e.Y;

        //    txtLotX.Text = e.X.ToString();
        //    txtLotY.Text = e.Y.ToString();
        //}

        //protected void btnAddLot_Click(object sender, EventArgs e)
        //{
        //}

        //protected void btnNewLot_Click(object sender, EventArgs e)
        //{
        //    newLot();
        //}
        //void newLot()
        //{
        //    Session["LotMode"] = "Add";
        //    txtLotX.Text = "";
        //    txtLotY.Text = "";
        //    txtLot.Text = "";

        //    txtLot.Enabled = true;
        //}

        //protected void gvLotList_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //}

        protected void btnAddBlockYes_ServerClick(object sender, EventArgs e)
        {
            //show block adding div
            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "addBlock();", true);

            ReloadCanvas();

            txtBlock.Text = "";
            txtBlockDescription.Text = "";

            txtBlock.Focus();

            BlockPreview.ImageUrl = "~/assets/img/no_image.png";

            txtBlock.Enabled = true;
            txtBlockDescription.Enabled = true;
        }

        protected void btnCancelAddBlock_Click(object sender, EventArgs e)
        {
            //delete block temp
            DeleteBlockTemp(txtProjId.Value, txtBlock.Text);

            //show block list div
            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "showBlockListDiv();", true);
            ReloadCanvas();
        }
        protected void btnDeleteBlock_Click(object sender, EventArgs e)
        {
            Session["SelectedBlock"] = txtBlock.Text;
            confirmation("Are you sure you want to remove block?", "removeBlock");
            ReloadCanvas();
        }
        //protected void gvSAPLot_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName.Equals("Select"))
        //    {
        //        int row = int.Parse(e.CommandArgument.ToString());
        //        string projCode = gvSAPLot.Rows[row].Cells[1].Text;
        //        string Block = gvSAPLot.Rows[row].Cells[2].Text;
        //        string Lot = gvSAPLot.Rows[row].Cells[3].Text;
        //        string x = gvSAPLot.Rows[row].Cells[4].Text;
        //        string y = gvSAPLot.Rows[row].Cells[5].Text;

        //        txtBlockLotEdit.Text = Block;
        //        txtLot.Text = Lot;

        //        DataTable dt = new DataTable();
        //        dt = hana.GetData($@"SELECT ""Block"",""Lot"",""left"",""top"" FROM ""PRJ2"" Where ""PrjCode"" = '{projCode}' and ""Block""= '{Block}' and ""Lot""= '{Lot}'", hana.GetConnection("SAOHana"));
        //        //check if exist
        //        if (DataAccess.Exist(dt))
        //        {
        //            txtLotX.Text = dt.Rows[0][2].ToString();
        //            txtLotY.Text = dt.Rows[0][3].ToString();
        //        }
        //        else
        //        {
        //            txtLotX.Text = "";
        //            txtLotY.Text = "";
        //        }

        //        Session["SelectedLot"] = Lot;
        //        //add lot to temp
        //        //ws.AddNewLotTemp(projCode, Block, Lot,(int) Session["UserID"]);
        //        //load canvas
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "imgPrvw", "drawBlockMap();", true);
        //    }
        //    else if (e.CommandName.Equals("Remove"))
        //    {
        //        int row = int.Parse(e.CommandArgument.ToString());
        //        string projCode = gvSAPLot.Rows[row].Cells[1].Text;
        //        string Block = gvSAPLot.Rows[row].Cells[2].Text;
        //        string Lot = gvSAPLot.Rows[row].Cells[3].Text;
        //        txtLot.Text = Lot;

        //        confirmation($"Are you sure you want to remove location for LOT:{Lot}?", "removelotlocation");
        //    }
        //}

        protected void gvSAPBlocks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int row = int.Parse(e.CommandArgument.ToString());
                string projCode = gvSAPBlocks.Rows[row].Cells[1].Text;
                string Block = gvSAPBlocks.Rows[row].Cells[2].Text;
                string Lot = gvSAPBlocks.Rows[row].Cells[3].Text;

                if (e.CommandName.Equals("Select"))
                {
                    //alert($"Please click image to set block location for BLOCK: {Block}", "info");
                    Session["SelectedProjectBlock"] = Block;
                    txtSelectedProjectBlock.Value = Block;
                    Session["SelectedProjectLot"] = Lot;
                    txtSelectedProjectLot.Value = Lot;

                    //DataTable dt = hana.GetData($@"SELECT ""BlockImage"" FROM ""PRJ1"" Where ""PrjCode"" = '{txtProjId.Value}' and ""Block"" = '{Block}'", hana.GetConnection("SAOHana"));
                    //if (dt.Rows.Count == 0)
                    //{
                    //    Session["SelectedProjectBlock"] = string.Empty;
                    //    Session["SelectedProjectLot"] = string.Empty;
                    //    txtSelectedProjectBlock.Value = string.Empty;
                    //    txtSelectedProjectLot.Value = string.Empty;
                    //    alertMsg($"Please upload image file for Block: {Block}", "warning");
                    //}

                    //** highlight selected row **//
                    //foreach (GridViewRow selectedrow in gvSAPBlocks.Rows)
                    //{
                    //    if (gvSAPBlocks.Rows[selectedrow.RowIndex].Cells[1].Text == Block)
                    //    { selectedrow.BackColor = System.Drawing.ColorTranslator.FromHtml("#A1DCF2"); }
                    //    else
                    //    { selectedrow.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF"); }
                    //}

                    //GetBlocksLots(projCode, Block);
                }
                else if (e.CommandName.Equals("Del"))
                {
                    txtSelectedProjectBlock.Value = Block;
                    txtSelectedProjectLot.Value = Lot;
                    confirmation($"Are you sure you want to remove location for Block: {Block}?", "removeblocklocation");
                }

            }
            catch { }
            ReloadCanvas();
        }

        //protected void btnSaveLot_Click(object sender, EventArgs e)
        //{
        //    //check if block per project is existing
        //    DataTable dt = new DataTable();
        //    dt = hana.GetData($@"SELECT 'true' FROM ""PRJ2"" Where ""PrjCode"" = '{txtProjId.Value}' and ""Block"" = '{txtBlockLot.Value}' and ""Lot"" = '{txtLot.Text}'", hana.GetConnection("SAOHana"));
        //    if (dt.Rows.Count > 0)
        //    {
        //        //UPDATE LOT
        //        if (ws.UpdateProjectLot(txtProjId.Value, txtBlockLot.Value, txtLot.Text, Convert.ToInt32(txtLotX.Text), Convert.ToInt32(txtLotY.Text)) == true)
        //        {
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "LotAllocationTab();", true);
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "showLotListDiv();", true);
        //            alertMsg("Lot updated successfully", "success");
        //        }
        //        else
        //        {
        //            alertMsg("Error in updating lot", "error");
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "LotAllocationTab();", true);
        //        }
        //    }
        //    else
        //    {
        //        //if location not selected
        //        if (!String.IsNullOrEmpty(txtLotX.Text) && !String.IsNullOrEmpty(txtLotY.Text))
        //        {
        //            if (ws.AddNewProjectLot(txtProjId.Value, txtBlockLot.Value, txtLot.Text, Convert.ToInt32(txtLotX.Text), Convert.ToInt32(txtLotY.Text)) == true)
        //            {
        //                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "LotAllocationTab();", true);
        //                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "showLotListDiv();", true);
        //                alertMsg("Lot saved successfully", "success");
        //            }
        //            else
        //            {
        //                alertMsg("Error in saving lot", "error");
        //                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "LotAllocationTab();", true);
        //            }
        //        }
        //        else
        //        {
        //            alertMsg("Please select lot location", "warning");
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "LotAllocationTab();", true);
        //        }
        //    }
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "imgPrvw", "drawBlockMap();", true);
        //    //load lot
        //    GetLot(txtProjId.Value, txtBlockLot.Value);
        //}

        protected void btnCancelAddLot_Click(object sender, EventArgs e)
        {
            //show block list div
            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "showLotListDiv();", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "imgPrvw", "drawBlockMap();", true);
            ReloadCanvas();
        }
        void NextTab(string tab)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", $"NextTab('{tab}');", true);
        }

        void PrevTab(string tab)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", $"PrevTab('{tab}');", true);
        }

        protected void btnEditBlock_ServerClick(object sender, EventArgs e)
        {
            FileUpload2.Enabled = true;

            txtBlock.Enabled = false;
            txtBlockDescription.Enabled = false;
            //Load Image File
            string imageUrl = "~/Handler/BlockPreview.ashx?id=" + txtProjId.Value + "&Block=" + txtBlock.Text;
            BlockPreview.ImageUrl = Page.ResolveUrl(imageUrl);
            //show block adding div
            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "addBlock();", true);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "$('.btnDeleteBlock').removeClass('hide');", true);

            ReloadCanvas();
        }
        protected void gvBLockList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //LoadBlockList();
            gvBLockList.PageIndex = e.NewPageIndex;
            LoadBlockList();
            //gvBLockList.DataSource = ws.GetBlockList(txtProjId.Value);
            //gvBLockList.DataBind();
        }

        protected void gvProjectList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            //gvProjectList.PageIndex = e.NewPageIndex;
            //gvProjectList.DataSource = ws.GetBlockList(txtProjId.Value);
            //gvProjectList.DataBind();

            gvProjectList.DataSource = ws.GetSAPProjects();
            gvProjectList.PageIndex = e.NewPageIndex;
            gvProjectList.DataBind();

        }

        //protected void gvSAPLot_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    //draw block
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "imgPrvw", "drawBlockMap();", true);

        //    GetLot(txtProjId.Value, txtBlockLot.Value);

        //    gvSAPLot.PageIndex = e.NewPageIndex;
        //    gvSAPLot.DataBind();

        //    gvSAPLot.SelectedIndex = -1;
        //}

        protected void gvProjectList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int row = int.Parse(e.CommandArgument.ToString());
            if (e.CommandName.Equals("Sel"))
            {
                string projCode = gvProjectList.Rows[row].Cells[0].Text;
                string projName = gvProjectList.Rows[row].Cells[1].Text;

                txtProjId.Value = projCode;
                txtProjName.Value = projName;

                //clear image
                ImagePreview.ImageUrl = "~/assets/img/no_image.png";

                DataTable dt = new DataTable();
                dt = hana.GetData($@"SELECT ""PrjImage"" FROM ""OPRJ"" Where ""PrjCode"" = '{projCode}'", hana.GetConnection("SAOHana"));
                if (dt.Rows.Count > 0)
                {
                    //check if ProjImage is not null
                    if (!String.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                    {
                        //Load Image File
                        string imageUrl = "~/Handler/ImageViewer.ashx?id=" + projCode;
                        ImagePreview.ImageUrl = Page.ResolveUrl(imageUrl);
                    }
                    else
                    {
                        ImagePreview.ImageUrl = "~/assets/img/no_image.png";
                    }
                }
                FileUpload1.Enabled = true;
                btnUpload.Disabled = false;
                //Load Block
                LoadBlockList();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "imgProject", "drawProjectMap();", true);
                //close modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "closeProjectList();", true);
            }
        }



        //protected void btnSaveLot_ServerClick(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(txtBlockLot.Value))
        //    {
        //        DataTable dt = new DataTable();
        //        dt = hana.GetData($@"SELECT IFNULL(""Lot"",'') ""Lot"",""left"",""top"" FROM ""TMP1"" Where ""PrjCode"" = '{txtProjId.Value}' And ""Block"" = '{txtBlockLot.Value}' and ""UserId"" = '{(int)Session["UserID"]}'", hana.GetConnection("SAOHana"));
        //        if (DataAccess.Exist(dt))
        //        {
        //            foreach (DataRow _row in dt.Rows)
        //            {
        //                string lot = _row[0].ToString();
        //                string x = _row[1].ToString();
        //                string y = _row[2].ToString();
        //                if (!string.IsNullOrEmpty(x))
        //                {
        //                    //** update lot if existing or add new lot
        //                    if (ws.UpdateProjectLot(txtProjId.Value, txtBlockLot.Value, lot, Convert.ToInt32(x), Convert.ToInt32(y)))
        //                    {
        //                        GetLot(txtProjId.Value, txtBlockLot.Value);
        //                        DeleteLotTemp(txtProjId.Value, txtBlockLot.Value);
        //                        Session["AddTempLot"] = false;
        //                        alertMsg("Saving of Lot location successfull", "success");
        //                    }
        //                    else
        //                    {
        //                        alertMsg("Error in saving Lot", "error");
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        alertMsg("Please select block", "warning");
        //    }
        //    GetLot(txtProjId.Value, txtBlockLot.Value);
        //    ReloadCanvas();
        //}

        protected void btnFinish_Click(object sender, EventArgs e)
        {
            Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
        }

        protected void btnLotPreview_ServerClick(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = hana.GetData($@"SELECT ""ImgWidth"",""ImgHeight"" FROM ""PRJ1"" WHERE ""PrjCode"" = '{txtProjId.Value}' AND ""Block"" = '{txtSelectedBlock.Value}'", hana.GetConnection("SAOHana"));
            txtLotWidthPreview.Value = (string)DataAccess.GetData(dt, 0, "ImgWidth", "0");
            txtLotHeightPreview.Value = (string)DataAccess.GetData(dt, 0, "ImgHeight", "0");

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "drawLot", "drawLotPreview();", true);
            ReloadCanvas();
            //show modal
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "show", "showLotPreview();", true);
        }

        protected void btnUpdateBlockTemp_ServerClick(object sender, EventArgs e)
        {
            if (ws.UpdateBlockTemp(txtProjId.Value, txtBlock.Text, Convert.ToInt32(txtPixelX.Text), Convert.ToInt32(txtPixelY.Text), (int)Session["UserID"]))
            {
                //success
                //LoadBlockList();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "imgPrvw", "drawProjectMap();", true);
            }
        }
        void GetBlocksLots(string proj, string block)
        {
            gvSAPBlocks.DataSource = ws.GetBlockLotsList(proj, block);
            gvSAPBlocks.DataBind();
        }

        void DeleteLotTemp(string projCode, string Block)
        {
            hana.Execute($@"DELETE FROM ""TMP1"" Where ""PrjCode"" = '{projCode}' and ""Block""='{Block}' and ""UserId""='{Session["UserID"]}'", hana.GetConnection("SAOHana"));
        }
        void DeleteBlockTemp(string projCode, string Block)
        {
            hana.Execute($@"DELETE FROM ""TMP1"" Where ""PrjCode"" = '{projCode}' and ""Block""='{Block}' and IFNULL(""Lot"",'') = '' and ""UserId""='{Session["UserID"]}'", hana.GetConnection("SAOHana"));
        }
        void DeleteTemp()
        {
            hana.Execute($@"DELETE FROM ""TMP1"" WHERE ""UserId""='{(int)Session["UserID"]}'", hana.GetConnection("SAOHana")); //delete temp
        }

        //protected void gvProjectBlocks_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    try
        //    {
        //        int index = Convert.ToInt32(e.CommandArgument);
        //        string block = gvProjectBlocks.Rows[index].Cells[2].Text;
        //        Session["selectedblockrow"] = block;

        //        if (e.CommandName.Equals("Sel"))
        //        {
        //            // ** image preview ** //
        //            //Check if Block has image
        //            DataTable dt = ws.GetProjectBlocks(txtProjId.Value, block).Tables["GetProjectBlocks"];
        //            if (dt.Rows.Count > 0)
        //            {
        //                string imageUrl = "~/Handler/BlockPreview.ashx?id=" + txtProjId.Value + "&Block=" + block;
        //                imgPreview.ImageUrl = Page.ResolveUrl(imageUrl);
        //                //show image preview
        //                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "showPreview();", true);
        //            }
        //        }
        //        else if (e.CommandName.Equals("Upload"))
        //        {
        //            FileUpload fu = (FileUpload)gvProjectBlocks.Rows[index].FindControl("blockImage");

        //            //if image is empty
        //            if (fu.HasFile)
        //            {
        //                System.Drawing.Image img = System.Drawing.Image.FromStream(fu.PostedFile.InputStream);
        //                //Convert Img to Byte
        //                byte[] img_byte = DataAccess.ConvertImageToByteArray(img, ImageFormat.Jpeg);
        //                int width = img.Width;
        //                int height = img.Height;
        //                //update image 
        //                if (ws.UpdateImageProjectBlock(txtProjId.Value, block, img_byte, width, height))
        //                {
        //                    LoadBlockList();
        //                    alertMsg("Image uploaded successfully", "success");
        //                }
        //                else
        //                {
        //                    alertMsg("Error in saving image!", "error");
        //                }
        //            }
        //            else
        //            {
        //                alertMsg("Please choose image!", "warning");
        //            }
        //        }
        //        else if (e.CommandName.Equals("Del"))
        //        {
        //            confirmation($"Are you sure you want to remove image from block {(string)Session["selectedblockrow"]}?", "removeblockimage");
        //        }
        //    }
        //    catch { }

        //}

        protected void gvProjectBlocks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Checking the RowType of the Row  
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Image img = e.Row.FindControl("blockStatus") as Image;

                //Check if Block has image
                DataTable dt = ws.GetProjectBlocks(txtProjId.Value, e.Row.Cells[2].Text).Tables["GetProjectBlocks"];
                if (dt.Rows.Count > 0)
                {
                    img.ImageUrl = "~/assets/img/checked.png";
                }
                else
                {
                    img.ImageUrl = "~/assets/img/cancel.png";
                }
            }
        }
        //protected void gvProjectBlocks_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    gvProjectBlocks.PageIndex = e.NewPageIndex;
        //    gvProjectBlocks.DataSource = ws.GetBlockList(txtProjId.Value);
        //    gvProjectBlocks.DataBind();

        //}
        protected void btnUpload_ServerClick(object sender, EventArgs e)
        {
            //check if existing
            DataTable dtProj = new DataTable();
            dtProj = hana.GetData($@"SELECT ""PrjImage"",""ImgWidth"",""ImgHeight"" FROM ""OPRJ"" Where ""PrjCode"" = '{txtProjId.Value}'", hana.GetConnection("SAOHana"));

            if (dtProj.Rows.Count > 0)
            {
                string imgwidth = dtProj.Rows[0][1].ToString();
                string imgheight = dtProj.Rows[0][2].ToString();
                //if image is empty
                if (FileUpload1.HasFile)
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(FileUpload1.PostedFile.InputStream);
                    //Convert Img to Byte
                    byte[] img_byte = DataAccess.ConvertImageToByteArray(img, ImageFormat.Jpeg);

                    int width = img.Width;
                    int height = img.Height;

                    Session["ProjectWidth"] = img.Width;
                    Session["ProjectHeight"] = img.Height;

                    //update image 
                    if (ws.UpdateProjectImage(txtProjId.Value, img_byte, width, height, (int)Session["UserID"]))
                    {
                        string imageUrl = "~/Handler/ImageViewer.ashx?id=" + txtProjId.Value;

                        imgProjectWidth.Value = img.Width.ToString();
                        imgProjectHeight.Value = img.Height.ToString();
                        ImagePreview.ImageUrl = Page.ResolveUrl(imageUrl);
                        alertMsg("Project image updated successfully", "success");
                    }
                    else
                    {
                        //alert("Error in updating Project Image.", "error");
                        alertMsg("Error in updating Project Image!", "error");
                    }
                }
                else
                {
                    alertMsg("Please choose image!", "warning");
                }
            }
            else
            {
                //add new project
                //if image is empty
                if (FileUpload1.HasFile)
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(FileUpload1.PostedFile.InputStream);
                    //Convert Img to Byte
                    byte[] img_byte = DataAccess.ConvertImageToByteArray(img, ImageFormat.Jpeg);

                    int width = img.Width;
                    int height = img.Height;
                    //update image 
                    if (ws.AddNewProject("Addon", txtProjId.Value, txtProjName.Value, img_byte, width, height, "User"))
                    {
                        string imageUrl = "~/Handler/ImageViewer.ashx?id=" + txtProjId.Value;

                        imgProjectWidth.Value = img.Width.ToString();
                        imgProjectHeight.Value = img.Height.ToString();
                        ImagePreview.ImageUrl = Page.ResolveUrl(imageUrl);
                        alertMsg("Project image added successfully", "success");
                    }
                    else
                    {
                        alertMsg("Error in saving project!", "error");
                    }
                }
                else
                {
                    alertMsg("Please choose image!", "warning");
                }
            }
        }
        protected void gvSAPBlocks_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GetBlocksLots(txtProjId.Value, txtBlockLot.Value);

            gvSAPBlocks.PageIndex = e.NewPageIndex;
            gvSAPBlocks.DataBind();

            gvSAPBlocks.SelectedIndex = -1;
        }
        protected void btnUpdateBlockLocation_ServerClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSelectedProjectBlock.Value) && !string.IsNullOrEmpty(txtSelectedProjectLot.Value))
            {
                DataTable dt = hana.GetData($@"SELECT * FROM ""PRJ2"" Where ""PrjCode"" = '{txtProjId.Value}' and ""Block"" = '{txtSelectedProjectBlock.Value}' and ""Lot"" = '{txtSelectedProjectLot.Value}'", hana.GetConnection("SAOHana"));
                if (dt.Rows.Count == 0 || dt.Rows.Count > 0)
                {
                    Session["LotRadius"] = txtPinActualSize.Value;
                    if (ws.UpdateProjectLot(txtProjId.Value, txtSelectedProjectBlock.Value, txtSelectedProjectLot.Value, Convert.ToInt32(Math.Round(Convert.ToDouble(txtPixelX.Text))), Convert.ToInt32(Math.Round(Convert.ToDouble(txtPixelY.Text)))))
                    {  /// (ws.UpdateProjectLot(txtProjId.Value, txtBlockLot.Value, txtLot.Text, Convert.ToInt32(txtLotX.Text), Convert.ToInt32(txtLotY.Text)) == true)//
                        LoadBlockList();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "imgProjectMap", "drawProjectMap();", true);
                        //alertMsg("Lot updated successfully", "success");
                    }
                }
                else
                {
                    alertMsg($"Invalid Block: {txtSelectedProjectBlock.Value} and Lot: {txtSelectedProjectLot.Value}", "warning");
                    LoadBlockList();
                    txtSelectedProjectBlock.Value = string.Empty;
                }
            }
            ReloadCanvas();
        }
        //protected void btnUpdateLotTemp_ServerClick(object sender, EventArgs e)
        //{
        //    if (ws.AddNewProjectLot(txtProjId.Value, txtBlockLot.Value, (string)Session["SelectedLot"], Convert.ToInt32(txtLotX.Text), Convert.ToInt32(txtLotY.Text)))
        //    {
        //        GetLot(txtProjId.Value, txtBlockLot.Value);
        //    }
        //    ReloadCanvas();
        //}

        protected void btnShowProj_ServerClick(object sender, EventArgs e)
        {
            LoadSAPProjects();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "projList", "showProjList();", true);
        }
        protected void bSearch_ServerClick(object sender, EventArgs e)
        {
            gvProjectList.DataSource = ws.SearchSAPProjects(txtSearch.Value);
            gvProjectList.DataBind();
        }
        protected void gvSAPBlocks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Checking the RowType of the Row  
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Image img = e.Row.FindControl("blockStatus") as Image;

                //Check if Block has image
                if (!string.IsNullOrEmpty(e.Row.Cells[4].Text.Trim()))
                {
                    img.ImageUrl = "~/assets/img/checked.png";
                }
                else
                {
                    img.ImageUrl = "~/assets/img/cancel.png";
                }
            }
        }
        protected void gvSAPLot_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Checking the RowType of the Row  
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Image img = e.Row.FindControl("blockStatus") as Image;

                //Check if Block has image
                if (!string.IsNullOrEmpty(e.Row.Cells[4].Text.Trim()))
                {
                    img.ImageUrl = "~/assets/img/checked.png";
                }
                else
                {
                    img.ImageUrl = "~/assets/img/cancel.png";
                }
            }
        }

        protected void bSearch_Click(object sender, EventArgs e)
        {
            gvProjectList.DataSource = ws.SearchSAPProjects(txtSearch.Value);
            gvProjectList.DataBind();
        }
    }
}
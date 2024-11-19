<%@ Page Title="DREAMS | Dashboard" Language="C#" MasterPageFile="~/master/Main.Master" AutoEventWireup="true" 
    CodeBehind="IE.aspx.cs" Inherits="MDC_REALITY.IE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function showModal() {
            $('#myModal').modal('show');
        }
        function showBlockList() {
            $('#modalBlockList').modal('show');
        }
        function showProjList() {
            $('#modalProjList').modal('show');
        }
        function showBlock() {
            $('#modalBlock').modal('show');
        }
        function showLotPreview() {
            $('#modalLotPreview').modal('show');
        }
        function closeProjectList() {
            $('#modalProjList').modal('hide');
        }
        function closeBlockList() {
            $('#modalBlockList').modal('hide');
        }
        function showPreview() {
            $('#modalPreview').modal('show');
        }
        function showBlockMap() {
            $('#modalBlockMap').modal({
                backdrop: 'static',
                keyboard: false,
                show: true
            });
        }
        function MsgNoti_Show() {
            $('#MsgNoti').modal('show');
        }
        function MsgBox_Show() {
            $('#MsgBox').modal('show');
        }
        function MsgBox_Hide() {
            $('#MsgBox').modal('hide');
        }
        function MsgQuotation_Hide() {
            $('#MsgQuotation').modal('hide');
        };
        function MsgQuotation_Show() {
            $('#MsgQuotation').modal('show');
        };
        function MsgEmpList_Hide() {
            $('#MsgEmpList').modal('hide');
        };
        function MsgEmpList_Show() {
            $('#MsgEmpList').modal('show');
        };
        function MsgBuyers_Show() {
            $('#MsgBuyers').modal('show');
        }
        function MsgBuyers_Hide() {
            $('#MsgBuyers').modal('hide');
        }
        function MsgProjList_Show() {
            $('#MsgProjList').modal('show');
        }
        function MsgProjList_Hide() {
            $('#MsgProjList').modal('hide');
        }
        function MsgBlockList_Show() {
            $('#MsgBlockList').modal('show');
        }
        function MsgLotList_Show() {
            $('#MsgLotList').modal('show');
        }
        function MsgBlockList_Hide() {
            $('#MsgBlockList').modal('hide');
        }
        function MsgLotList_Hide() {
            $('#MsgLotList').modal('hide');
        }
        function MsgHouseList_Show() {
            $('#MsgHouseList').modal('show');
        }
        function MsgHouseList_Hide() {
            $('#MsgHouseList').modal('hide');
        }
        function MsgPicPreview_Show() {
            $('#MsgPicPreview').modal('show');
        }
        function MsgPicPreview_Hide() {
            $('#MsgPicPreview').modal('hide');
        }

        function MsgNewQuotation_Show() {
            $('#MsgNewQuotation').modal('show');
        }
        function MsgNewQuotation_Hide() {
            $('#MsgNewQuotation').modal('hide');
        }

        function MsgBPList_Show() {
            $('#MsgBPList').modal('show');
        }
        function MsgBPList_Hide() {
            $('#MsgBPList').modal('hide');
        }
    </script>
    <script type="text/javascript">
        function drawProjectMap() {
            var width = $('.imgProjectWidth').val();
            var height = $('.imgProjectHeight').val();
            //plot map markers base on json
            var proj = $('.txtProjId').val();
            var jsonProject = '<%=ConfigurationManager.AppSettings["jsonProject"].ToString() %>'; //get json for project lot from web config
            var imgProject = '<%=ConfigurationManager.AppSettings["imgProject"].ToString() %>';//get img handler for blocks from web config
            var jsonLot = jsonProject + 'PrjCode=' + proj;
            var imgBlockUrl = imgProject + 'id=' + proj;

            //for summary preview
            var canvasPreview = this.__canvas = new fabric.Canvas('imgBlockPreview', {
                hoverCursor: 'pointer',
                selection: false,
                perPixelTargetFind: true,
                targetFindTolerance: 5,
                width: width,
                height: height
            });

            $.getJSON(jsonLot, function (data) {
                var circle = canvasPreview.loadFromJSON(data, canvasPreview.renderAll.bind(canvasPreview), function (o, object) {
                    fabric.log(o, object);
                });


                //set background image
                canvasPreview.setBackgroundImage(imgBlockUrl, canvasPreview.renderAll.bind(canvasPreview), {
                    backgroundImageOpacity: 0.5,
                    backgroundImageStretch: true
                });

                //loop all opbject
                canvasPreview.forEachObject(function (o) {
                    //Render the text
                    var y = 0;
                    if (o.description.length != 1) {
                        y = 10; //one digit number
                    }
                    else {
                        y = 15; //two digit number
                    }
                    var text = canvasPreview.add(new fabric.Text(o.description, {
                        left: o.left + y,
                        top: o.top + 10,
                        fill: 'white',
                        fontSize: '<%=ConfigurationManager.AppSettings["MapFontSize"].ToString() %>',
                        fontWeight: 'bold',
                        name: o.name,
                        description: o.description,
                        selectable: false
                    }));
                });
                canvasPreview.forEachObject(function (x) { x.hasBorders = x.hasControls = false; x.lockMovementX = true; x.lockMovementY = true; });

                //set background image
                canvasPreview.setBackgroundImage(imgBlockUrl, canvasPreview.renderAll.bind(canvasPreview), {
                    backgroundImageOpacity: 0.5,
                    backgroundImageStretch: true
                });
                //****mouse events****
                //mouse down
                canvasPreview.on('mouse:down', function (e) {
                    if (e.target != null) {
                        //show lot preview modal
                        $('#<%= txtSelectedBlock.ClientID %>').val(e.target.name);
                        document.getElementById('<%= btnLotPreview.ClientID %>').click();
                    }
                });
                //mouse over
                canvasPreview.on('mouse:over', function (e) {
                    if (e.target != null) {
                        if (e.target.type == 'text') {
                            e.target.setFill('white');
                        }
                        else {
                            e.target.setFill('red');
                        }
                        canvasPreview.renderAll();
                    }
                });
                //mouse out
                canvasPreview.on('mouse:out', function (e) {
                    if (e.target != null) {
                        if (e.target.type == 'text') {
                            e.target.setFill('white');
                        }
                        else {
                            e.target.setFill('green');
                        }
                        canvasPreview.renderAll();
                    }
                });
            });
        }
    </script>
    <script type="text/javascript">
        //**** LOT PREVIEW ****//
        function drawLotPreview() {
            var width = $('.txtLotWidthPreview').val();
            var height = $('.txtLotHeightPreview').val();

            var proj = $('.txtProjId').val();
            var block = $('.txtSelectedBlock').val();

            var jsonLotURL = '<%=ConfigurationManager.AppSettings["jsonProjectLot"].ToString() %>';
            var jsonLotPreview = jsonLotURL + 'PrjCode=' + proj + '&Block=' + block;
            var imgBlockURL = '<%=ConfigurationManager.AppSettings["imgBlock"].ToString() %>';//get img handler for blocks from web config
            var imgBlock = imgBlockURL + 'id=' + proj + '&Block=' + block;

            var canvasPreview = this.__canvas = new fabric.Canvas('imgLotPreview', {
                hoverCursor: 'pointer',
                selection: false,
                perPixelTargetFind: true,
                targetFindTolerance: 5,
                width: width,
                height: height
            });

            //set background image
            canvasPreview.setBackgroundImage(imgBlock, canvasPreview.renderAll.bind(canvasPreview), {
                backgroundImageOpacity: 0.5,
                backgroundImageStretch: true
            });

            $.getJSON(jsonLotPreview, function (data) {
                var circle = canvasPreview.loadFromJSON(data, canvasPreview.renderAll.bind(canvasPreview), function (o, object) {
                    fabric.log(o, object);
                });
                //loop all opbject
                canvasPreview.forEachObject(function (o) {
                    //Render the text
                    var y = 0;
                    if (o.description.length != 1) {
                        y = 10; //one digit number
                    }
                    else {
                        y = 15; //two digit number
                    }
                    var text = canvasPreview.add(new fabric.Text(o.description, {
                        left: o.left + y,
                        top: o.top + 10,
                        fill: 'white',
                        fontSize: '<%=ConfigurationManager.AppSettings["MapFontSize"].ToString() %>',
                        fontWeight: 'bold',
                        name: o.name,
                        description: o.description,
                        selectable: false
                    }));

                    //set background image
                    canvasPreview.setBackgroundImage(imgBlock, canvasPreview.renderAll.bind(canvasPreview), {
                        backgroundImageOpacity: 0.5,
                        backgroundImageStretch: true
                    });
                });
                canvasPreview.forEachObject(function (x) { x.hasBorders = x.hasControls = false; x.lockMovementX = true; x.lockMovementY = true; });
            });
        }
    </script>
    <%-- LOT LOCATION --%>
    <script type="text/javascript">
        function drawLotMap() {
            var width = $('.txtLotWidthPreview').val();
            var height = $('.txtLotHeightPreview').val();

            // initialize fabric canvas and assign to global windows object for debug
            var canvas = this.__canvas = new fabric.Canvas('imgProjectLot', {
                hoverCursor: 'pointer',
                selection: false,
                perPixelTargetFind: true,
                targetFindTolerance: 5
            });
            //plot map markers base on json
            var proj = $('.txtProjId').val();
            var block = $('.txtSelectedBlock').val();
            var jsonProjectLot = '<%=ConfigurationManager.AppSettings["jsonPrjwithColor"].ToString() %>'; //get json for project lot from web config
            var imgBlock = '<%=ConfigurationManager.AppSettings["imgBlock"].ToString() %>';//get img handler for blocks from web config
            var jsonLot = jsonProjectLot + 'PrjCode=' + proj + '&Block=' + block;
            var imgBlockUrl = imgBlock + 'id=' + proj + '&Block=' + block;

            $.getJSON(jsonLot, function (data) {
                canvas.loadFromJSON(data, canvas.renderAll.bind(canvas), function (o, object) {
                    fabric.log(o, object);
                });
                //loop all opbject
                canvas.forEachObject(function (o) {
                    //Render the text
                    var y = 0;
                    if (o.description.length != 1) {
                        y = 10; //one digit number
                    }
                    else {
                        y = 15;
                    }
                    var text = canvas.add(new fabric.Text(o.description, {
                        left: o.left + y,
                        top: o.top + 10,
                        fill: 'white',
                        fontSize: '<%=ConfigurationManager.AppSettings["LotRadius"].ToString() %>',
                        fontWeight: 'bold',
                        name: o.name,
                        description: o.description,
                        status: o.status,
                        selectable: false
                    }));
                });
                canvas.forEachObject(function (o) { o.hasBorders = o.hasControls = false; o.lockMovementX = true; o.lockMovementY = true; });

                canvas.setBackgroundImage(imgBlockUrl, canvas.renderAll.bind(canvas), {
                    backgroundImageOpacity: 0.5,
                    backgroundImageStretch: true
                });
                //****mouse events****
                //mouse down
                canvas.on('mouse:down', function (e) {
                    if (e.target != null) {
                        canvas.renderAll();
                        $('#<%= txtSelectedLot.ClientID%>').val(e.target.name);
                        $('.bGenerate').click();
                    }
                });

                canvas.setWidth(width);
                canvas.setHeight(height);
            });
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="sitemap" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div class="col-lg-12">
        <div class="panel panel-default">
            <div class="panel-heading">
                Project Map
            </div>
            <div class="panel-body">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-lg-3">
                                <div class="form-group">
                                    <div class="input-group">
                                        <input type="text" placeholder="Project Code" style="display:none; z-index: auto;" class="form-control txtProjId" runat="server" id="txtProjId" disabled />
                                        <span class="input-group-btn">
                                            <%--<button runat="server" id="btnShowProj" onserverclick="btnShowProj_ServerClick" style="width: 50px; z-index: auto;" class="btn btn-secondary btn-dropbox" type="button"><i class="fa fa-bars"></i></button>--%>
                                            <button runat="server" id="btnIE" onserverclick="btnIE_ServerClick" style="width: 50px; z-index: auto;" class="btn btn-secondary btn-dropbox" type="button"><i class="fa fa-bars"></i></button>
                                        </span>
                                    </div>
                                </div>
                            </div> 
                        </div>
                        <div class="row" style="overflow: auto; display:none; margin: 0 auto; width: 100%;">
                            <div class="col-lg-12">
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <p align="center">
                                            <canvas id="imgBlockPreview" style="border: 1px solid black; margin: 0 auto; display: block; width: 100%;">Your browser does not support the Canvas Element
                                            </canvas>
                                        </p>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <input type="text" runat="server" id="txtSelectedLot" class="txtSelectedLot hide" />
                        <input type="text" runat="server" id="txtSelectedBlock" class="txtSelectedBlock hide" />
                        <input type="text" runat="server" id="txtLotWidthPreview" class="txtLotWidthPreview hide" />
                        <input type="text" runat="server" id="txtLotHeightPreview" class="txtLotHeightPreview hide" />
                        <input id="imgProjectWidth" runat="server" class="imgProjectWidth form-control hide" disabled />
                        <input id="imgProjectHeight" runat="server" class="imgProjectHeight form-control hide" disabled />
                        <input type="text" runat="server" id="hLotWidth" class="hLotWidth hide" />
                        <input type="text" runat="server" id="hLotHeight" class="hLotHeight hide" />
                        <button runat="server" id="bGenerate" onserverclick="bGenerate_ServerClick" class="bGenerate hide" />
                        <button type="button" class="btn btn-info btnLotPreview hide" id="btnLotPreview" runat="server" onserverclick="btnLotPreview_ServerClick"></button>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="modals" runat="server">
    <div id="modalProjList" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header" style="background-color: dodgerblue">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" style="color: white">Project List</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-lg-2 col-md-2 col-sm-6 col-xs-12">
                                    <h5>Search: </h5>
                                </div>
                                <div class="col-lg-10 col-md-10 col-sm-6 col-xs-12">
                                    <asp:Panel runat="server" DefaultButton="bSearch">
                                        <div class="input-group">
                                            <input runat="server" id="txtSearch" placeholder="Search here" class="form-control" type="text" autofocus /><span class="input-group-btn">
                                                <asp:LinkButton runat="server" ID="bSearch" Style="width: 50px;" CssClass="btn btn-secondary btn-default btn-facebook" OnClick="bSearch_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                            </span>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-lg-12">
                                    <asp:GridView runat="server" ID="gvProjectList"
                                        AllowPaging="true"
                                        PageSize="10"
                                        CssClass="table table-hover table-responsive"
                                        AutoGenerateColumns="false"
                                        GridLines="None"
                                        OnRowCommand="gvProjectList_RowCommand"
                                        OnPageIndexChanging="gvProjectList_PageIndexChanging">
                                        <HeaderStyle BackColor="#333333" ForeColor="White" Font-Bold="true" />
                                        <Columns>
                                            <asp:BoundField DataField="PrjCode" HeaderText="Project Code" />
                                            <asp:BoundField DataField="PrjName" HeaderText="Project Name" />
                                            <asp:ButtonField ButtonType="Link" ControlStyle-CssClass="btn btn-default btn-success fa fa-arrow-right" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" CommandName="Sel" />
                                        </Columns>
                                        <PagerStyle CssClass="pagination-ys" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <div id="modalLotPreview" class="modal fade">
        <div class="modal-dialog modal-default modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    Lot Preview
                </div>
                <div class="modal-body">
                    <div class="row" style="overflow: auto; width: 100%; margin: 0 auto;">
                        <div class="col-lg-12">
                            <asp:UpdatePanel runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <input type="text" runat="server" id="Text1" class="txtSelectedBlock hide" />
                                    <input type="text" runat="server" id="Text2" class="txtLotWidthPreview hide" />
                                    <input type="text" runat="server" id="Text3" class="txtLotHeightPreview hide" />
                                    <p align="center">
                                        <canvas id="imgLotPreview" style="border: 1px solid black; display: block; margin: 0 auto; overflow: auto;">Your browser does not support the Canvas Element
                                        </canvas>
                                    </p>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="MsgLotList" class="modal fade" role="dialog">
        <div class="modal-dialog modal-lg" role="dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Lot List</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-3 col-md-3 col-sm-12 col-xs-12">
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                    <h4>Legend</h4>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="gvBlockColor" runat="server"
                                                AutoGenerateColumns="false"
                                                EmptyDataText="No Records Found"
                                                CssClass="table table-bordered table-hover "
                                                Width="100%"
                                                ShowHeader="True">
                                                <HeaderStyle BackColor="#66ccff" />
                                                <Columns>
                                                    <asp:BoundField DataField="Name" HeaderText="Name" />
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderText="Color" ItemStyle-Width="50">
                                                        <ItemTemplate>
                                                            <asp:TextBox runat="server" ID="txtColor" TextMode="Color" CssClass="form-control nomargin" BorderStyle="None" Width="50px" Text='<%# Bind("Color") %>' Enabled="false"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerStyle CssClass="pagination-ys" />
                                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12">
                            <div style="overflow: auto; height: 500px;">
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <p align="center">
                                            <canvas id="imgProjectLot" style="border: 1px solid black; display: block; margin: 0 auto;">Your browser does not support the Canvas Element
                                            </canvas>
                                        </p>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <div id="MsgHouseList" class="modal fade" role="dialog">
        <div class="modal-dialog modal-lg" role="dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">House List</h4>
                </div>
                <div class="modal-body" style="overflow: auto; height: 500px;">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:GridView runat="server"
                                        ID="gvHouseList"
                                        CssClass="table table-responsive"
                                        AutoGenerateColumns="false"
                                        GridLines="None"
                                        ShowHeader="false">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <div class="row">
                                                        <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                                            <div class="row">
                                                                <div class="col-lg-12">
                                                                    <asp:Image runat="server" ID="imgHouse" ImageUrl='<%# "~/Handler/HouseModel.ashx?ID=" + Eval("U_Picture")%>' Width="100%" Height="180px" />
                                                                </div>
                                                                <div class="col-lg-12">
                                                                    <asp:LinkButton runat="server" ID="bPicPreview" CssClass="btn btn-primary" Width="100%" Height="100%" OnClick="bPicPreview_Click" CommandArgument='<%# Bind("DocEntry")%>'><i class="fa fa-image"> Preview</i></asp:LinkButton>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                            <div class="row">
                                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-3">
                                                                    Model :
                                                                </div>
                                                                <div class="col-lg-9 col-md-9 col-sm-9 col-xs-9">
                                                                    <asp:Label runat="server" ID="lblModel" Text='<%# Bind("Name") %>'></asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-3">
                                                                    Feature :
                                                                </div>
                                                                <div class="col-lg-9 col-md-9 col-sm-9 col-xs-9">
                                                                    <asp:Label runat="server" ID="Label2" Text='<%# Bind("U_Feat") %>'></asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-3">
                                                                    Structure :
                                                                </div>
                                                                <div class="col-lg-9 col-md-9 col-sm-9 col-xs-9">
                                                                    <asp:Label runat="server" ID="Label4" Text='<%# Bind("U_Structure") %>'></asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-3">
                                                                    Floor Area :
                                                                </div>
                                                                <div class="col-lg-5 col-md-5 col-sm-5 col-xs-5">
                                                                    <asp:Label runat="server" ID="Label1" Text='<%# string.Format("{0:#,##0.00}", Eval("U_FloorArea")) %>'></asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="row" style="display:none;">
                                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-3">
                                                                    TCP :
                                                                </div>
                                                                <div class="col-lg-5 col-md-5 col-sm-5 col-xs-5">
                                                                    <asp:Label runat="server" CssClass="text-primary" Font-Size="10" ID="Label6" Text='<%# string.Format("PHP {0:#,##0.00}",Eval("U_ResFee")) %>'></asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-3">
                                                                    Reservation Fee :
                                                                </div>
                                                                <div class="col-lg-5 col-md-5 col-sm-5 col-xs-5">
                                                                    <asp:Label runat="server" CssClass="text-info" Font-Size="10" ID="Label3" Text='<%# string.Format("PHP {0:#,##0.00}",Eval("U_ResFee"))  %>'></asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="pagination-ys" />
                                        <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <div id="MsgPicPreview" class="modal fade" role="dialog" tabindex="-1">
        <div class="modal-dialog modal-lg" role="dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">House Preview</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:GridView runat="server"
                                        ID="gvPicPreview"
                                        CssClass="table table-responsive"
                                        AutoGenerateColumns="false"
                                        GridLines="None"
                                        ShowHeader="false">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <div class="row">
                                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                            <asp:Image runat="server" ID="imgHouse" ImageUrl='<%# "~/Handler/HouseModel.ashx?ID=" + Eval("U_Picture")%>' Width="100%" Height="400px" />
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="pagination-ys" />
                                        <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal" data-toggle="modal" data-target="#MsgHouseList">Close</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

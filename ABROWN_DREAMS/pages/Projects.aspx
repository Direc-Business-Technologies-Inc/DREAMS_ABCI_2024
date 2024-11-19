<%@ Page Title="DREAMS | Project Maintenance" Language="C#" MasterPageFile="~/master/Main.Master" AutoEventWireup="true" CodeBehind="Projects.aspx.cs" Inherits="ABROWN_DREAMS.Projects" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--MODALS--%>
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
    </script>
    <%--IMAGE PREVIEW--%>
    <script type="text/javascript">
        function previewFile() {
            var preview = document.querySelector('#imgPrvw');
            var file = document.querySelector('#imageUpload').files[0];
            var reader = new FileReader();

            reader.onloadend = function () {
                preview.src = reader.result;
            }

            if (file) {
                reader.readAsDataURL(file);
            } else {
                preview.src = "";
                $('#divImg').hide();
            }
            $('#divImg').show(); //SHOW
        }
        function preview() {
            var preview = document.querySelector('#<%=ImagePreview.ClientID %>');
            var file = document.querySelector('#projectFile').files[0];
            var reader = new FileReader();

            reader.onloadend = function () {
                preview.src = reader.result;
            }
            if (file) {
                reader.readAsDataURL(file);
                $('#divImg').show(); //SHOW
            } else {
                preview.src = "";
            }
        }
        function previewFile1() {
            var preview = document.querySelector('#<%=ImagePreview.ClientID %>');
            var file = document.querySelector('#<%=FileUpload1.ClientID %>').files[0];
            var reader = new FileReader();

            reader.onloadend = function () {
                preview.src = reader.result;
            }
            if (file) {
                reader.readAsDataURL(file);
                $('#divImg').show(); //SHOW
            } else {
                preview.src = "";
            }
        }

        function previewFile2() {
            var preview = document.querySelector('#<%=BlockPreview.ClientID %>');
            var file = document.querySelector('#<%=FileUpload2.ClientID %>').files[0];
            var reader = new FileReader();

            reader.onloadend = function () {
                preview.src = reader.result;
            }

            if (file) {
                reader.readAsDataURL(file);
            } else {
                preview.src = "";
            }
        }
    </script>
    <%--WIZARD--%>
    <script>
        $(document).ready(function () {
            //Initialize tooltips
            $('.nav-tabs > li a[title]').tooltip();
            //Wizard
            $('a[data-toggle="tab"]').on('show.bs.tab', function (e) {
                var $target = $(e.target);
                if ($target.parent().hasClass('disabled')) {
                    return false;
                }
            });
            $(".next-step").click(function (e) {
                var $active = $('.wizard .nav-tabs li.active');
                $active.next().removeClass('disabled');
                nextTab($active);
            });
            $(".prev-step").click(function (e) {
                var $active = $('.wizard .nav-tabs li.active');
                prevTab($active);
            });
        });
        function nextTab(elem) {
            $(elem).next().find('a[data-toggle="tab"]').click();
        }
        function prevTab(elem) {
            $(elem).prev().find('a[data-toggle="tab"]').click();
        }
    </script>
    <%--SAVE IMAGE TO LOCAL STORAGE--%>
    <script>
        function Prev() {
            var $active = $('.wizard .nav-tabs li.active');
            prevTab($active);
        }
        function Next() {
            var $active = $('.wizard .nav-tabs li.active');
            $active.next().removeClass('disabled');
            nextTab($active);
        }
        function NextTab() {
            $('.nav-tabs a[href="#project_block"]').tab('show');
            $('.nav-tabs a[href="#project_block"]').removeClass('disabled');
        }
        function LotAllocationTab() {
            $('.nav-tabs a[href="#project_lot"]').tab('show');
            $('.nav-tabs a[href="#project_block"]').removeClass('disabled');
        }
        function showImgPrvw() {
            $('#divImg').show();
        }
    </script>
    <%--LOT--%>
    <script type="text/javascript">
        function drawBlockMap() {
            var width = $('.imgBlockWidth').val();
            var height = $('.imgBlockHeight').val();

            // initialize fabric canvas and assign to global windows object for debug
            var canvas = this.__canvas = new fabric.Canvas('imgProjectLot', {
                hoverCursor: 'pointer',
                selection: false,
                perPixelTargetFind: true,
                targetFindTolerance: 5,
                width: width,
                height: height
            });

            //plot map markers base on json
            var proj = $('.txtProjId').val();
            var block = $('.txtBlockLot').val();

            var jsonProjectLot = '<%=ConfigSettings.jsonProjectLot"].ToString() %>'; //get json for project lot from web config
            var jsonLot = jsonProjectLot + 'PrjCode=' + proj + '&Block=' + block;
            var imgBlock = '<%=ConfigSettings.imgBlock"].ToString() %>';//get img handler for blocks from web config
            var imgBlockUrl = imgBlock + 'id=' + proj + '&Block=' + block;

            canvas.setBackgroundImage(imgBlockUrl, canvas.renderAll.bind(canvas), {
                backgroundImageOpacity: 0.5,
                backgroundImageStretch: true
            });
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
                        fontSize: '<%=ConfigSettings.LotRadius"].ToString() %>',
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
            });

            //****mouse events****
            //mouse down
            canvas.on('mouse:down', function (e) {
                if (e.target != null) {
                    if (e.target.type == 'text') {
                        e.target.setFill('white');
                    }
                    else {
                        e.target.setFill('red');
                    }
                    //set value
                    $('#<%= txtLot.ClientID%>').val(e.target.name);
                    $('#<%= txtLotX.ClientID%>').val(e.target.left);
                    $('#<%= txtLotY.ClientID%>').val(e.target.top);

                        //document.getElementById('<%= btnUpdateLotTemp.ClientID %>').click();
                }
            });

            //mouse event
            canvas.on('mouse:down', function (evt) {
                var x = evt.e.offsetX;
                var y = evt.e.offsetY;

                $('#<%= txtLotX.ClientID%>').val(x);
                $('#<%= txtLotY.ClientID%>').val(y);

                var message = 'Mouse position: ' + x + ',' + y;
                console.log(message);
                document.getElementById('<%= btnUpdateLotTemp.ClientID %>').click();
            });
        };
    </script>
    <script type="text/javascript">
        //**** LOT PREVIEW ****//
        function drawLotPreview() {
            var width = $('.txtLotWidthPreview').val();
            var height = $('.txtLotHeightPreview').val();

            var proj = $('.txtProjId').val();
            var block = $('.txtSelectedBlock').val();

            var jsonLotURL = '<%=ConfigSettings.jsonProjectLot"].ToString() %>';
            var jsonLotPreview = jsonLotURL + 'PrjCode=' + proj + '&Block=' + block;
            var imgBlockURL = '<%=ConfigSettings.imgBlock"].ToString() %>';//get img handler for blocks from web config
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
                        fontSize: '<%=ConfigSettings.MapFontSize"].ToString() %>',
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
    <%--BLOCK--%>
    <script type="text/javascript">
        function drawProjectMap() {
            var width = $('.imgProjectWidth').val();
            var height = $('.imgProjectHeight').val();

            // initialize fabric canvas and assign to global windows object for debug
            var canvas = this.__canvas = new fabric.Canvas('imgProjectBlock', {
                hoverCursor: 'pointer',
                selection: false,
                perPixelTargetFind: true,
                targetFindTolerance: 5,
                width: innerWidth,
                height: height
            });

            //plot map markers base on json
            var proj = $('.txtProjId').val();
            var block = $("#<%=txtBlock.ClientID%>").val();
            var jsonProject = '<%=ConfigSettings.jsonProject"].ToString() %>'; //get json for project lot from web config
            var imgProject = '<%=ConfigSettings.imgProject"].ToString() %>';//get img handler for blocks from web config
            var jsonLot = jsonProject + 'PrjCode=' + proj;
            var imgBlockUrl = imgProject + 'id=' + proj;

            $.getJSON(jsonLot, function (data) {
                var circle = canvas.loadFromJSON(data, canvas.renderAll.bind(canvas), function (o, object) {
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
                        left: o.left + y, //+10 to center text
                        top: o.top + 10,
                        fill: 'white',
                        fontSize: '<%=ConfigSettings.MapFontSize"].ToString() %>',
                        fontWeight: 'bold',
                        name: o.name,
                        description: o.description,
                        selectable: false
                    }));
                });
                canvas.forEachObject(function (x) { x.hasBorders = x.hasControls = false; x.lockMovementX = true; x.lockMovementY = true; });

                //set background image
                canvas.setBackgroundImage(imgBlockUrl, canvas.renderAll.bind(canvas), {
                    backgroundImageOpacity: 0.5,
                    backgroundImageStretch: true,
                    scaleX: canvasPreview.width / width,
                    scaleY: canvasPreview.height / height
                });
                //****mouse events****
                //mouse down
                canvas.on('mouse:down', function (e) {
                    if (e.target != null) {
                        if (e.target.type == 'text') {
                            e.target.setFill('white');
                        }
                        else {
                            e.target.setFill('red');
                        }

                        $('#<%= txtPixelX.ClientID%>').val(e.target.left);
                        $('#<%= txtPixelY.ClientID%>').val(e.target.top);

                        <%--//view block
                        //set value
                        $('#<%= txtBlock.ClientID%>').val(e.target.name);
                        $('#<%= txtBlockDescription.ClientID%>').val(e.target.description);
                       

                        //showBlock();
                        $('.btnEditBlock').click();
                        //hide div block list
                        addBlock();--%>
                    }
                    else {
                        //add new block
                        var x = e.e.offsetX;
                        var y = e.e.offsetY;
                        $('.txtPixelX').val(x);
                        $('.txtPixelY').val(y);
                        //showBlockMap();
                    }
                });
                //mouse over
                canvas.on('mouse:over', function (e) {
                    if (e.target != null && e.target.fill != 'orange') {
                        if (e.target.type == 'text') {
                            e.target.setFill('white');
                        }
                        else {
                            e.target.setFill('red');
                        }
                        canvas.renderAll();
                    }
                });
                //mouse out
                canvas.on('mouse:out', function (e) {
                    if (e.target != null && e.target.fill != 'orange') {
                        if (e.target.type == 'text') {
                            e.target.setFill('white');
                        }
                        else {
                            e.target.setFill('green');
                        }
                        canvas.renderAll();
                    }
                });
                //mouse event
                canvas.on('mouse:down', function (evt) {
                    document.getElementById('<%= btnUpdateBlockLocation.ClientID %>').click();
                });
            });

            //for summary preview
            var canvasPreview = this.__canvas = new fabric.Canvas('imgBlockPreview', {
                hoverCursor: 'pointer',
                selection: false,
                perPixelTargetFind: true,
                targetFindTolerance: 5,
                width: innerWidth,
                height: height
            });

            $.getJSON(jsonLot, function (data) {
                var circle = canvasPreview.loadFromJSON(data, canvasPreview.renderAll.bind(canvasPreview), function (o, object) {
                    fabric.log(o, object);
                });


                //set background image
                canvasPreview.setBackgroundImage(imgBlockUrl, canvasPreview.renderAll.bind(canvasPreview), {
                    backgroundImageOpacity: 0.5,
                    backgroundImageStretch: true,
                    scaleX: canvasPreview.width / width,
                    scaleY: canvasPreview.height / height
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
                        fontSize: '<%=ConfigSettings.MapFontSize"].ToString() %>',
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
                    backgroundImageStretch: true,
                    scaleX: canvasPreview.width / width,
                    scaleY: canvasPreview.height / height
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
    <script>
        function mousePos(event) {
            var x = event.e.offsetX;
            var y = event.e.offsetY;

            $('#<%= txtLotX.ClientID%>').val(x);
            $('#<%= txtLotY.ClientID%>').val(y);
        }
    </script>
    <script>
        //ADDING OF BLOCK
        function addBlock() {
            $('#addBlock').show();
            $('#blockList').hide();
        };
        function showBlockListDiv() {
            $('#blockList').show();
            $('#addBlock').hide();
        };
        $(document).ready(function () {
            $('#addBlock').hide();
        });
    </script>
    <script>
        //ADDING OF LOT
        function addLot() {
            $('#addLot').show();
            $('#lotList').hide();
        };
        function showLotListDiv() {
            $('#lotList').show();
            $('#addLot').hide();
        };
        $(document).ready(function () {
            $('#addLot').hide();
        });
    </script>
    <%--navigation--%>
    <script type="text/javascript">
        function Prev() {
            var $active = $('.wizard .nav-tabs li.active');
            $active.prev().removeClass('disabled');
            $active.addClass('disabled');
            prevTab($active);
        }
        function Next() {
            var $active = $('.wizard .nav-tabs li.active');
            $active.next().removeClass('disabled');
            $active.addClass('disabled');
            nextTab($active);
        }
        function NextTab(tab) {
            $('.nav-tabs a[href="#' + tab + ']').removeClass('disabled');
            Next();
        }

        function PrevTab(tab) {
            $('.nav-tabs a[href="#' + tab + ']').removeClass('disabled');
            Prev();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="sitemap" runat="server">
    <%--<li><a><span>Project Maps</span></a></li>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div class="panel panel-default panel-trans" id="form_buyer">
        <div class="panel-heading">
            <div class="row">
                <div class="col-md-7 col-sm-5 col-xs-12">
                    <h4 class="trans-title">Project Master Data</h4>
                </div>
                <div class="col-md-5 col-sm-7 col-xs-12">
                </div>
            </div>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-sm-12">
                    <div class="wizard">
                        <div class="wizard-inner">
                            <div class="connecting-line"></div>
                            <ul class="nav nav-tabs" role="tablist">
                                <li role="presentation" class="active">
                                    <a href="#project" data-toggle="tab" aria-controls="project" role="tab" title="Select Project">
                                        <span class="round-tabs one">
                                            <i class="glyphicon glyphicon-list"></i>
                                        </span>
                                    </a>
                                </li>
                                <li role="presentation" class="disabled">
                                    <a href="#project_block" data-toggle="tab" aria-controls="project_block" role="tab" title="Assign Block">
                                        <span class="round-tabs two">
                                            <i class="fa fa-map-o"></i>
                                        </span>
                                    </a>
                                </li>
                                <li role="presentation" class="disabled">
                                    <a href="#project_lot" data-toggle="tab" aria-controls="project_lot" role="tab" title="Assign Lot">
                                        <span class="round-tabs three">
                                            <i class="fa fa-home"></i>
                                        </span>
                                    </a>
                                </li>
                                <li role="presentation" class="disabled">
                                    <a href="#project_complete" data-toggle="tab" aria-controls="project_complete" role="tab" title="Summary">
                                        <span class="round-tabs four">
                                            <i class="glyphicon glyphicon-ok"></i>
                                        </span>
                                    </a>
                                </li>
                            </ul>
                        </div>
                        <div class="tab-content">
                            <div class="tab-pane active" role="tabpanel" id="project">
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <div class="container">
                                            <%--    <input id="projectFile" type="file" name="projectFile" onchange="preview();" />
                                            <asp:Button runat="server" ID="btnUpload" OnClick="btnUpload_Click" Text="Upload" CssClass="btn btn-primary" />--%>
                                            <div class="row">
                                                <div class="panel panel-primary">
                                                    <div class="panel-heading clearfix">
                                                        Project Info
                                                    <i class="fa fa-info-circle pull-right"></i>
                                                    </div>
                                                    <div class="panel-body">
                                                        <%--PROJECT DETAILS--%>
                                                        <div class="col-lg-6">
                                                            <div class="row">
                                                                <div class="col-lg-12">
                                                                    <div class="form-group">
                                                                        <div class="input-group">
                                                                            <input type="text" placeholder="Project Code" style="z-index: auto;" class="form-control txtProjId" runat="server" id="txtProjId" disabled />
                                                                            <span class="input-group-btn">
                                                                                <button runat="server" id="btnShowProj" onserverclick="btnShowProj_ServerClick" style="width: 50px; z-index: auto;" class="btn btn-secondary btn-dropbox" type="button"><i class="fa fa-bars"></i></button>
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-lg-12">
                                                                    <div class="form-group">
                                                                        <input type="text" placeholder="Project Name" class="form-control" id="txtProjName" runat="server" disabled />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-lg-12">
                                                                    <div class="form-group">
                                                                        <div class="input-group">
                                                                            <asp:FileUpload runat="server" ID="FileUpload1" onchange="previewFile1();" accept="image/*" CssClass="form-control" Enabled="false" ClientIDMode="Static" />
                                                                            <span class="input-group-btn">
                                                                                <button runat="server" id="btnUpload" onserverclick="btnUpload_ServerClick" disabled style="width: 50px; z-index: auto;" class="btn btn-secondary btn-dropbox" type="button"><i class="fa fa-upload"></i></button>
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                    <div style="margin: auto;" id="divImg">
                                                                        <asp:Image ID="ImagePreview" runat="server" Width="100%" ImageUrl="~/assets/img/no_image.png" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <%--BLOCK LIST--%>
                                                        <div class="col-lg-6">
                                                            <div class="row">
                                                                <div class="col-lg-12">
                                                                    <asp:UpdatePanel runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:GridView runat="server" ID="gvProjectBlocks"
                                                                                AllowSorting="true"
                                                                                CssClass="table table-hover table-responsive"
                                                                                AutoGenerateColumns="false"
                                                                                GridLines="None"
                                                                                ShowFooter="true"
                                                                                PageSize="10"
                                                                                AllowPaging="true"
                                                                                SelectedRowStyle-BackColor="#A1DCF2"
                                                                                OnRowCommand="gvProjectBlocks_RowCommand"
                                                                                OnRowDataBound="gvProjectBlocks_RowDataBound"
                                                                                OnPageIndexChanging="gvProjectBlocks_PageIndexChanging"
                                                                                EmptyDataText="No Blocks found">
                                                                                <Columns>
                                                                                    <asp:TemplateField>
                                                                                        <ItemTemplate>
                                                                                            <asp:Image runat="server" ID="blockStatus" ImageUrl="~/assets/img/cancel.png" Width="20" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField DataField="PrjCode" HeaderText="Project" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                                                                    <asp:BoundField DataField="Block" HeaderText="Block" ItemStyle-HorizontalAlign="Center" />
                                                                                    <asp:ButtonField ControlStyle-CssClass="glyphicon glyphicon-picture" HeaderStyle-Width="10" CommandName="Sel" ItemStyle-HorizontalAlign="Center" HeaderText="Preview" />
                                                                                    <asp:TemplateField>
                                                                                        <ItemTemplate>
                                                                                            <asp:FileUpload runat="server" ID="blockImage" Width="100%" accept="image/*" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:ButtonField ControlStyle-CssClass="glyphicon glyphicon-upload" HeaderStyle-Width="10" CommandName="Upload" ItemStyle-HorizontalAlign="Center" HeaderText="Upload" />
                                                                                    <asp:ButtonField ControlStyle-CssClass="glyphicon glyphicon-trash" HeaderStyle-Width="10" CommandName="Del" ItemStyle-HorizontalAlign="Center" HeaderText="Remove" ItemStyle-ForeColor="#e2584d" />
                                                                                </Columns>
                                                                                <PagerStyle CssClass="pagination-ys" />
                                                                                <EmptyDataRowStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                                                                            </asp:GridView>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:PostBackTrigger ControlID="gvProjectBlocks" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row pull-right">
                                                <div class="col-lg-12">
                                                    <asp:UpdatePanel runat="server">
                                                        <ContentTemplate>
                                                            <asp:Button runat="server" ID="btnCancel" CssClass="btn btn-danger hide" Style="width: 100px;" Text="Cancel" OnClick="btnCancel_Click" />
                                                            <asp:Button runat="server" ID="btnNext" CssClass="btn btn-primary" Style="width: 100px;" Text="Next" OnClick="btnNext_Click" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnUpload" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <div class="tab-pane" role="tabpanel" id="project_block">
                                <div class="panel panel-default">
                                    <%--<div class="panel-heading clearfix">
                                        Block Allocation
                                                    <i class="fa fa-info-circle pull-right"></i>
                                    </div>--%>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-lg-3">
                                                <div class="panel panel-default">
                                                    <div class="panel-body">
                                                        <div class="row">
                                                            <div class="col-lg-12">
                                                                <div id="blockList">
                                                                    <asp:UpdatePanel runat="server" UpdateMode="Always">
                                                                        <ContentTemplate>
                                                                            <asp:GridView runat="server" ID="gvSAPBlocks"
                                                                                AllowSorting="true"
                                                                                CssClass="table table-hover table-responsive"
                                                                                AutoGenerateColumns="false"
                                                                                GridLines="None"
                                                                                AllowPaging="true"
                                                                                PageSize="10"
                                                                                SelectedRowStyle-BackColor="#A1DCF2"
                                                                                OnPageIndexChanging="gvSAPBlocks_PageIndexChanging"
                                                                                OnRowCommand="gvSAPBlocks_RowCommand"
                                                                                OnRowDataBound="gvSAPBlocks_RowDataBound">
                                                                                <Columns>
                                                                                    <asp:TemplateField>
                                                                                        <ItemTemplate>
                                                                                            <asp:Image runat="server" ID="blockStatus" ImageUrl="~/assets/img/cancel.png" Width="20" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField DataField="PrjCode" HeaderText="Project" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                                                                    <asp:BoundField DataField="Block" HeaderText="Block" />
                                                                                    <asp:BoundField DataField="left" HeaderText="X" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText=" " />
                                                                                    <asp:BoundField DataField="top" HeaderText="Y" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText=" " />
                                                                                    <asp:ButtonField ControlStyle-CssClass="glyphicon glyphicon-map-marker" HeaderStyle-Width="10" CommandName="Select" ItemStyle-HorizontalAlign="Center" HeaderText="Location" />
                                                                                    <asp:ButtonField ControlStyle-CssClass="glyphicon glyphicon-trash" HeaderStyle-Width="10" CommandName="Del" ControlStyle-ForeColor="Red" />
                                                                                </Columns>
                                                                                <PagerStyle CssClass="pagination-ys" />
                                                                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" Font-Bold="true" />
                                                                            </asp:GridView>
                                                                            <div class="row" style="visibility: hidden;">
                                                                                <div class="col-lg-6">
                                                                                    <h5>Selected Block:</h5>
                                                                                </div>
                                                                                <div class="col-lg-6">
                                                                                    <input runat="server" type="text" class="form-control small" id="txtSelectedProjectBlock" disabled>
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                                <div id="addBlock" style="display: none;">
                                                                    <div class="row pull-right">
                                                                        <div class="col-lg-12">
                                                                            <asp:UpdatePanel runat="server">
                                                                                <ContentTemplate>
                                                                                    <asp:LinkButton runat="server" ID="btnSaveBlock" CssClass="btn btn-info" Text="Save" OnClick="btnSaveBlock_Click" Width="100">Save <i class="fa fa-save"></i></asp:LinkButton>
                                                                                    <asp:LinkButton runat="server" ID="btnCancelAddBlock" CssClass="btn btn-danger" Text="Save" OnClick="btnCancelAddBlock_Click" Width="100">Cancel <i class="fa fa-remove"></i></asp:LinkButton>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </div>
                                                                    </div>
                                                                    <asp:UpdatePanel runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-lg-12">
                                                                                    <div class="form-group">
                                                                                        <label>Block:</label>
                                                                                        <asp:TextBox runat="server" ID="txtBlock" CssClass="form-control txtBlock" placeholder="Block No." Enabled="false"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row">
                                                                                <div class="col-lg-12">
                                                                                    <div class="form-group">
                                                                                        <label>Please select block location</label>
                                                                                        <asp:TextBox runat="server" ID="txtBlockDescription" CssClass="form-control" placeholder="Block Description" Visible="false"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row">
                                                                                <div class="col-lg-6">
                                                                                    <div class="form-group">
                                                                                        <label>X:</label>
                                                                                        <asp:TextBox runat="server" ID="txtPixelX" CssClass="form-control txtPixelX"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-lg-6">
                                                                                    <div class="form-group">
                                                                                        <label>Y:</label>
                                                                                        <asp:TextBox runat="server" ID="txtPixelY" CssClass="form-control txtPixelY"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row">
                                                                                <div class="col-lg-12">
                                                                                    <asp:UpdatePanel runat="server">
                                                                                        <ContentTemplate>
                                                                                            <asp:FileUpload runat="server" ID="FileUpload2" onchange="previewFile2();" CssClass="form-control" />
                                                                                        </ContentTemplate>
                                                                                        <Triggers>
                                                                                            <asp:PostBackTrigger ControlID="btnSaveBlock" />
                                                                                        </Triggers>
                                                                                    </asp:UpdatePanel>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row">
                                                                                <div class="col-lg-12">
                                                                                    <asp:UpdatePanel runat="server">
                                                                                        <ContentTemplate>
                                                                                            <asp:Image ID="BlockPreview" runat="server" Width="100%" ImageUrl="~/assets/img/no_image.png" />
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-9">
                                                <div class="row">
                                                    <div class="col-lg-12">
                                                        <div runat="server" id="divBlockLocation">
                                                            <div style="overflow: auto; width: 100%;">
                                                                <asp:UpdatePanel runat="server">
                                                                    <ContentTemplate>
                                                                        <%--<asp:ImageButton runat="server" ID="imgBlockMap" BorderWidth="1" OnClick="imgBlockMap_Click" />--%>
                                                                        <p align="center">
                                                                            <canvas id="imgProjectBlock" style="border: 1px solid black; display: block; margin: 0 auto;">Your browser does not support the Canvas Element
                                                                            </canvas>
                                                                        </p>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-12">
                                                        <asp:UpdatePanel runat="server">
                                                            <ContentTemplate>
                                                                <input id="imgProjectWidth" runat="server" class="imgProjectWidth form-control hide" disabled />
                                                                <input id="imgProjectHeight" runat="server" class="imgProjectHeight form-control hide" disabled />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row pull-right">
                                    <div class="col-lg-12">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <asp:Button runat="server" ID="btnPrev2" CssClass="btn btn-default prev-step" Style="width: 100px;" Text="Previous" OnClick="btnPrev2_Click" />
                                                <asp:Button runat="server" ID="btnNext2" CssClass="btn btn-primary" Style="width: 100px;" Text="Next" OnClick="btnNext2_Click" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                            <div class="tab-pane" role="tabpanel" id="project_lot">
                                <div class="panel panel-default">
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-lg-4">
                                                <div class="panel panel-default">
                                                    <%-- <div class="panel-heading clearfix">
                                                        <h4 class="panel-title pull-left" style="padding-top: 7.5px;">Lot List</h4>
                                                        <asp:UpdatePanel runat="server">
                                                            <ContentTemplate>
                                                                <button class="btn btn-success pull-right" id="btnSaveLot" onserverclick="btnSaveLot_ServerClick" runat="server" style="visibility: hidden;">Save</button>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>--%>
                                                    <div class="panel-body">
                                                        <div id="addLot">
                                                            <div class="row pull-right">
                                                                <div class="col-lg-12">
                                                                    <asp:UpdatePanel runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:LinkButton runat="server" ID="btnSaveLot2" CssClass="btn btn-info" Text="Save" OnClick="btnSaveLot_Click" Width="100">Save <i class="fa fa-save" hidden="hidden"></i></asp:LinkButton>
                                                                            <asp:LinkButton runat="server" ID="btnCancelAddLot" CssClass="btn btn-danger" Text="Save" OnClick="btnCancelAddLot_Click" Width="100">Cancel <i class="fa fa-remove"></i></asp:LinkButton>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                            </div>
                                                            <asp:UpdatePanel runat="server">
                                                                <ContentTemplate>
                                                                    <div class="row">
                                                                        <div class="col-lg-12">
                                                                            <div class="form-group">
                                                                                <label>Block:</label>
                                                                                <asp:TextBox runat="server" ID="txtBlockLotEdit" CssClass="form-control txtBlockLotEdit" placeholder="Block No." ReadOnly="true"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-lg-6">
                                                                            <h5>Lot:</h5>
                                                                            <div class="form-group">
                                                                                <label>Lot:</label>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-lg-6">
                                                                            <asp:TextBox runat="server" ID="txtLot" CssClass="form-control txtLot" placeholder="Lot" Enabled="false"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-lg-12">
                                                                            <div class="form-group">
                                                                                <label>Please select lot location</label>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-lg-6">
                                                                            <div class="form-group">
                                                                                <label>X:</label>
                                                                                <asp:TextBox runat="server" ID="txtLotX" CssClass="form-control txtLotX"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-lg-6">
                                                                            <div class="form-group">
                                                                                <label>Y:</label>
                                                                                <asp:TextBox runat="server" ID="txtLotY" CssClass="form-control txtLotY"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <div id="lotList">
                                                            <asp:UpdatePanel runat="server">
                                                                <ContentTemplate>
                                                                    <div class="row">
                                                                        <div class="col-lg-4">
                                                                            <h5>Block:</h5>
                                                                        </div>
                                                                        <div class="col-lg-8">
                                                                            <div class="input-group">
                                                                                <input type="text" class="form-control txtBlockLot" id="txtBlockLot" runat="server" placeholder="Please select Block" disabled />
                                                                                <span class="input-group-btn">
                                                                                    <button style="width: 50px; z-index: auto;" onclick="showBlockList();" class="btn btn-secondary btn-dropbox" type="button"><i class="fa fa-bars"></i></button>
                                                                                </span>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                            <div class="row">
                                                                <div class="col-lg-12">
                                                                    <asp:UpdatePanel runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:GridView runat="server" ID="gvSAPLot"
                                                                                AllowSorting="true"
                                                                                AllowPaging="true"
                                                                                CssClass="table table-hover table-responsive table-condensed"
                                                                                AutoGenerateColumns="false"
                                                                                GridLines="None"
                                                                                SelectedRowStyle-BackColor="#A1DCF2"
                                                                                OnRowCommand="gvSAPLot_RowCommand"
                                                                                OnPageIndexChanging="gvSAPLot_PageIndexChanging"
                                                                                OnRowDataBound="gvSAPLot_RowDataBound">
                                                                                <Columns>
                                                                                    <asp:TemplateField>
                                                                                        <ItemTemplate>
                                                                                            <asp:Image runat="server" ID="blockStatus" ImageUrl="~/assets/img/cancel.png" Width="20" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField DataField="PrjCode" HeaderText="Project" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                                                                    <asp:BoundField DataField="Block" HeaderText="Block No." ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                                                                    <asp:BoundField DataField="Lot" HeaderText="Lot No." />
                                                                                    <asp:BoundField DataField="left" HeaderText="X" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText=" " />
                                                                                    <asp:BoundField DataField="top" HeaderText="Y" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText=" " />
                                                                                    <asp:ButtonField ControlStyle-CssClass="glyphicon glyphicon-map-marker" CommandName="Select" ItemStyle-HorizontalAlign="Right" HeaderText="Location" />
                                                                                    <asp:ButtonField ControlStyle-CssClass="glyphicon glyphicon-trash" CommandName="Remove" ControlStyle-ForeColor="Red" ItemStyle-HorizontalAlign="Right" />
                                                                                </Columns>
                                                                                <PagerStyle CssClass="pagination-ys" />
                                                                            </asp:GridView>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-8">
                                                <div style="overflow: auto; width: 100%;">
                                                    <asp:UpdatePanel runat="server">
                                                        <ContentTemplate>
                                                            <input id="imgBlockWidth" runat="server" class="imgBlockWidth form-control hide" disabled />
                                                            <input id="imgBlockHeight" runat="server" class="imgBlockHeight form-control hide" disabled />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                    <asp:UpdatePanel runat="server">
                                                        <ContentTemplate>
                                                            <p align="center">
                                                                <canvas id="imgProjectLot" style="border: 1px solid black; display: block;">Your browser does not support the Canvas Element
                                                                </canvas>
                                                            </p>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row pull-right">
                                    <div class="col-lg-12">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <asp:Button runat="server" ID="btnPrev3" CssClass="btn btn-default prev-step" Style="width: 100px;" Text="Previous" OnClick="btnPrev3_Click" />
                                                <asp:Button runat="server" ID="btnNext3" CssClass="btn btn-primary" Style="width: 100px;" Text="Next" OnClick="btnNext3_Click" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                            <div class="tab-pane" role="tabpanel" id="project_complete">
                                <div class="container">
                                    <div class="panel panel-primary">
                                        <div class="panel-heading clearfix">
                                            Map Preview
                                                    <i class="fa fa-map pull-right"></i>
                                        </div>
                                        <div class="panel-body">
                                            <div class="row" style="overflow: auto; margin: 0 auto; width: 100%;">
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
                                        </div>
                                    </div>
                                    <div class="row pull-right">
                                        <div class="col-lg-12">
                                            <asp:UpdatePanel runat="server">
                                                <ContentTemplate>
                                                    <asp:Button runat="server" ID="btnPrev4" CssClass="btn btn-default prev-step" Style="width: 100px;" Text="Previous" OnClick="btnPrev4_Click" />
                                                    <asp:Button runat="server" ID="btnFinish" CssClass="btn btn-primary" Style="width: 100px;" Text="Finish" OnClick="btnFinish_Click" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
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
                                        <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
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
    <div id="modalBlockList" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Block List</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-lg-12">
                                    <asp:GridView runat="server" ID="gvBLockList"
                                        AllowPaging="true"
                                        AllowSorting="true"
                                        CssClass="table table-hover table-responsive"
                                        AutoGenerateColumns="false"
                                        GridLines="None"
                                        OnRowCommand="gvBLockList_RowCommand"
                                        OnPageIndexChanging="gvBLockList_PageIndexChanging">
                                        <HeaderStyle BackColor="#66ccff" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblID" Text='<%# Eval("Id")%>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PrjCode" HeaderText="Project Code" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                            <asp:BoundField DataField="Block" HeaderText="Block Number" />
                                            <asp:BoundField DataField="description" HeaderText="Block Description" />
                                            <asp:BoundField DataField="left" HeaderText="X" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                            <asp:BoundField DataField="top" HeaderText="Y" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                            <asp:ButtonField ButtonType="Link" ControlStyle-CssClass="btn btn-default btn-success fa fa-hand-o-up" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" CommandName="Select" />
                                        </Columns>
                                        <PagerStyle CssClass="pagination-ys" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
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
                                    <input type="text" runat="server" id="txtSelectedBlock" class="txtSelectedBlock hide" />
                                    <input type="text" runat="server" id="txtLotWidthPreview" class="txtLotWidthPreview hide" />
                                    <input type="text" runat="server" id="txtLotHeightPreview" class="txtLotHeightPreview hide" />
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
    <div id="modalAlert" class="modal fade">
        <div class="modal-dialog" id="alertModal" runat="server">
            <div class="modal-content">
                <div class="modal-body">
                    <div class="row" style="text-align: center;">
                        <div class="col-lg-12">
                            <asp:UpdatePanel runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <asp:Image ImageUrl="~/assets/img/success.png" runat="server" ID="alertIcon" Width="90" Height="90" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="row" style="text-align: center;">
                        <div class="col-lg-12">
                            <asp:UpdatePanel runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <h4>
                                        <asp:Label runat="server" ID="lblMessageAlert"></asp:Label></h4>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="row" style="text-align: center;">
                        <div class="col-lg-12">
                            <button type="button" class="btn btn-primary" data-dismiss="modal" style="width: 90px;">OK</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="modalConfirmation" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="row" style="text-align: center;">
                                <div class="col-lg-12">
                                    <asp:Image ImageUrl="~/assets/img/question.png" runat="server" ID="imgQuestion" Width="90" Height="90" />
                                </div>
                            </div>
                            <div class="row" style="text-align: center">
                                <div class="col-lg-12">
                                    <h4>
                                        <asp:Label runat="server" ID="lblConfirmationInfo"></asp:Label></h4>
                                </div>
                            </div>
                            <div class="row" style="text-align: center">
                                <div class="col-lg-12">
                                    <asp:Button runat="server" ID="btnYes" CssClass="btn btn-info btn-sm" Text="Yes" OnClick="btnYes_Click" Style="width: 90px;" />
                                    <button type="button" class="btn btn-default btn-sm" data-dismiss="modal" style="width: 90px;">No</button>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div id="modalBlockMap" class="modal fade">
        <asp:UpdatePanel runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="modal-dialog modal-info modal-sm">
                    <div class="modal-content">
                        <div class="modal-header" runat="server">
                        </div>
                        <div class="modal-body">
                            <h4>Add new block?</h4>
                        </div>
                        <div class="modal-footer">
                            <div class="row">
                                <div class="col-lg-6">
                                    <button type="button" class="btn btn-info" id="btnAddBlockYes" data-dismiss="modal" style="width: 100%;" runat="server" onserverclick="btnAddBlockYes_ServerClick">Yes</button>
                                </div>
                                <div class="col-lg-6">
                                    <button type="button" class="btn btn-default" data-dismiss="modal" style="width: 100%;" onclick="blockList();">Cancel</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="modalPreview" class="modal fade">
        <div class="modal-dialog modal-default modal-lg" runat="server">
            <div class="modal-content">
                <div class="modal-header">
                    Image Preview
                </div>
                <div class="modal-body">
                    <asp:Image runat="server" ID="imgPreview" Width="100%" />
                </div>
                <div class="modal-footer">
                    <button type="button" style="width: 100px;" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <button type="button" class="btn btn-info btnLotPreview hide" id="btnLotPreview" runat="server" onserverclick="btnLotPreview_ServerClick"></button>
                <button type="button" class="btn btn-info btnEditBlock hide" id="btnEditBlock" runat="server" onserverclick="btnEditBlock_ServerClick"></button>
                <button type="button" runat="server" id="btnUpdateLotTemp" class="btn btn-info btnUpdateLotTemp hide" onserverclick="btnUpdateLotTemp_ServerClick"></button>
                <button type="button" runat="server" id="btnUpdateBlockTemp" class="btn btn-info btnUpdateBlockTemp hide" onserverclick="btnUpdateBlockTemp_ServerClick"></button>
                <button type="button" runat="server" id="btnUpdateBlockLocation" class="btn btn-info btnUpdateBlockLocation hide" onserverclick="btnUpdateBlockLocation_ServerClick"></button>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

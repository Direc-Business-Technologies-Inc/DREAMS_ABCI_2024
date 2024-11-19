<%@ Page Title="DREAMS | Dashboard" Language="C#" MasterPageFile="~/master/Main.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="ABROWN_DREAMS.Dashboard" %>

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


        //ALERTS
        function ShowAlert() {
            $('#modalAlert').modal('show');
        }
        function HideAlert() {
            $('#modalAlert').modal('hide');
        }
        function ShowSuccessAlert() {
            $('#modalSuccess').modal('show');
        }

        function download1test() {
            console.log('test');

            $('#updateProgress').show();

            var convertedDataURL = [];

            var json;

            var issuccess = false;

            var canvasArray = [];
            //console.log(canvasArray);

            const names = document.getElementById('canvasForm').children;
            //console.log(names);

            const canvaspush = async () => {
                for (var i = 0; i < names.length; i++) {
                    const result = await canvasArray.push(names[i].childNodes[0].id);
                }
            }

            canvaspush();



            const canvaspost = async () => {
                for (var x = 0; x < canvasArray.length; x++) {


                    var canvasElementID = canvasArray[x];
                    var splitID = canvasArray[x].split("s");
                    var canvas = document.getElementById(canvasElementID);
                    var imgData = canvas.toDataURL("image/jpeg", 1.0);
                    json = "{'items':[{ 'canvassImage': '" + imgData + "','prjCode': '" + splitID[1] + "'}]}";
                    convertedDataURL.push({ canvassImage: imgData, prjCode: splitID[1] });

                    //var items = convertedDataURL[x];

                    const post = await $.ajax({
                        type: "POST",
                        url: "/pages/Dashboard.aspx/ReportCreation",
                        data: json,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (result) {

                            //** Clear all the canvas after downloading **//
                            var canvas = document.getElementById('' + canvasArray[x] + '');
                            const context = canvas.getContext("2d");
                            context.clearRect(0, 0, canvas.width, canvas.height);
                            issuccess = true;
                        },
                        complete: function () {
                            //$('#updateProgress').hide();
                            issuccess = true;
                        },
                        error: function (xmlhttprequest, textstatus, errorthrown) {
                            //$('#updateProgress').hide();
                            console.log("error: " + errorthrown);
                            issuccess = false;
                        }
                    });
                }
                if (issuccess) {
                    $('#updateProgress').hide();
                    alert("Canvas downloaded successfully");
                    $.ajax({
                        type: "POST",
                        url: 'Dashboard.aspx/GenerateMapForm',
                        data: "{}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            window.open('ReportViewer.aspx');
                        }
                    });
                }
                else {
                    $('#updateProgress').hide();
                    alert(" conection to the server failed ");
                }
            }


            canvaspost();


            //var sample = [{ compId: "1", formId: "531" },
            //{ compId: "2", formId: "77" },
            //{ compId: "3", formId: "99" },
            //{ status: "2", statusId: "8" },
            //{ name: "Value", value: "myValue" }];
            // console.log(sample);

            //console.log(items);
            //console.log(JSON.stringify(items));
            //$('#updateProgress').css("display", "block");
            //$('#updateProgress').css("aria-hidden", "false");


        }

        //Function for the Generation of Multiple Project Map//
        function download1() {
            console.log('test');

            $('#updateProgress').show();

            var convertedDataURL = [];

            var json;

            var issuccess = false;

            var canvasArray = [];
            //console.log(canvasArray);

            const names = document.getElementById('canvasForm').children;
            //console.log(names);

            const canvaspush = async () => {
                for (var i = 0; i < names.length; i++) {
                    const result = await canvasArray.push(names[i].childNodes[0].id);
                }
            }

            canvaspush();



            const canvaspost = async () => {
                for (var x = 0; x < canvasArray.length; x++) {


                    var canvasElementID = canvasArray[x];
                    var splitID = canvasArray[x].split("s");
                    var canvas = document.getElementById(canvasElementID);
                    var imgData = canvas.toDataURL("image/jpeg", 1.0);
                    json = "{'items':[{ 'canvassImage': '" + imgData + "','prjCode': '" + splitID[1] + "'}]}";
                    convertedDataURL.push({ canvassImage: imgData, prjCode: splitID[1] });

                    //var items = convertedDataURL[x];

                    const post = await $.ajax({
                        type: "POST",
                        url: "/pages/Dashboard.aspx/ReportCreation",
                        data: json,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (result) {

                            //** Clear all the canvas after downloading **//
                            var canvas = document.getElementById('' + canvasArray[x] + '');
                            const context = canvas.getContext("2d");
                            context.clearRect(0, 0, canvas.width, canvas.height);
                            issuccess = true;
                        },
                        complete: function () {
                            //$('#updateProgress').hide();
                            issuccess = true;
                        },
                        error: function (xmlhttprequest, textstatus, errorthrown) {
                            //$('#updateProgress').hide();
                            console.log("error: " + errorthrown);
                            issuccess = false;
                        }
                    });
                }
                if (issuccess) {
                    $('#updateProgress').hide();
                    alert("Canvas downloaded successfully");
                    $.ajax({
                        type: "POST",
                        url: 'Dashboard.aspx/GenerateMapForm',
                        data: "{}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            window.open('ReportViewer.aspx');
                        }
                    });
                }
                else {
                    $('#updateProgress').hide();
                    alert(" conection to the server failed ");
                }
            }


            canvaspost();


            //var sample = [{ compId: "1", formId: "531" },
            //{ compId: "2", formId: "77" },
            //{ compId: "3", formId: "99" },
            //{ status: "2", statusId: "8" },
            //{ name: "Value", value: "myValue" }];
            // console.log(sample);

            //console.log(items);
            //console.log(JSON.stringify(items));
            //$('#updateProgress').css("display", "block");
            //$('#updateProgress').css("aria-hidden", "false");


        }
        //function download1() { 
        //    console.log('test');

        //    var convertedDataURL = [];

        //    var canvasArray = [];
        //    //console.log(canvasArray);

        //    const names = document.getElementById('canvasForm').children;
        //    //console.log(names);

        //    for (var i = 0; i < names.length; i++) {
        //        canvasArray.push(names[i].childNodes[0].id);
        //    }

        //    for (var x = 0; x < canvasArray.length; x++) {

        //        var canvasElementID = canvasArray[x];
        //        var splitID = canvasArray[x].split("s");
        //        var canvas = document.getElementById(canvasElementID);
        //        var imgData = canvas.toDataURL("image/jpeg", 1.0);
        //        convertedDataURL.push({ canvassImage: imgData, prjCode: splitID[1] });
        //    }

        //    var sample = [{ compId: "1", formId: "531" },
        //    { compId: "2", formId: "77" },
        //    { compId: "3", formId: "99" },
        //    { status: "2", statusId: "8" },
        //    { name: "Value", value: "myValue" }];
        //    // console.log(sample);

        //    var items = convertedDataURL;
        //    //console.log(items);
        //    console.log(JSON.stringify(items));
        //    //$('#updateProgress').css("display", "block");
        //    //$('#updateProgress').css("aria-hidden", "false");
        //    $('#updateProgress').show();
        //    $.ajax({
        //        type: "POST",
        //        url: "/pages/Dashboard.aspx/ReportCreation",
        //        data: "{'items':" + JSON.stringify(items) + "}",
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (result) {

        //            //** Clear all the canvas after downloading **//
        //            for (var x in canvasArray) {
        //                var canvas = document.getElementById('' + canvasArray[x] + '');
        //                const context = canvas.getContext("2d");
        //                context.clearRect(0, 0, canvas.width, canvas.height);
        //            }
        //            alert("Canvas downloaded successfully");
        //        },
        //        complete: function () {
        //            $('#updateProgress').hide();
        //        },
        //        error: function (xmlhttprequest, textstatus, errorthrown) {
        //            $('#updateProgress').hide();
        //            alert(" conection to the server failed ");
        //            console.log("error: " + errorthrown);
        //        }
        //    });

        //}




        //$(document).ready(function () {
        //console.log('PageReady');

        //** load all project once page is ready **//
        function loadCanvas() {

            $("#download").addClass('hide');
            console.log('PageReady');

            $.get('/webservice/DirecWebService.asmx/GetProjectForCanvas', {}).done(result => {

                var projectList = [];
                var canvasID = [];

                const names = result.getElementsByTagName("ImageDatas")[0].children;

                //Get Details for the canvas
                for (var i = 0; i < names.length; i++) {
                    projectList.push({
                        projectCode: names[i].getElementsByTagName("projectCode")[0].innerHTML,
                        projectName: names[i].getElementsByTagName("projectName")[0].innerHTML,
                        imgWidth: names[i].getElementsByTagName("imgWidth")[0].innerHTML,
                        imgHeight: names[i].getElementsByTagName("imgHeight")[0].innerHTML,
                    });
                }

                //Draw dynamic canvas//
                for (var y = 0; y < projectList.length; y++) {

                    var projCode = projectList[y].projectCode;
                    var canid = 'canvas' + projCode;
                    canvasID.push(canid);

                    document.getElementById('canvasForm').innerHTML += '<canvas id="' + canid + '" style="border: 1px solid black; margin: 0 auto; width: 100%">Your browser does not support the Canvas Element </canvas>';

                }

                //** Put Images and Pins to Each Canvas --Div is hidden**//
                const loadprojectlist = async () => {
                    //$.each(canvasID, async (_, canID) {
                    canvasID.forEach(async (canID, _) => {

                        var splitCanvasPrj = canID.split("s");
                        var canvasPrj = splitCanvasPrj[1];

                        const loadproj = async () => {
                            for (var a in projectList) {

                                var projCode = projectList[a].projectCode;
                                var imageW = projectList[a].imgWidth;
                                var imageH = projectList[a].imgHeight;

                                if (projCode == canvasPrj) {

                                    var width = imageW;
                                    var height = imageH;
                                    //New Dashboard plot map markers base on json lot
                                    var jsonLotURL = '<%=ConfigurationManager.AppSettings["jsonNewPrjwithColor"].ToString() %>'; //get json for project lot from web config
                                    var imgProject = '<%=ConfigurationManager.AppSettings["imgProject"].ToString() %>';//get img handler for blocks from web config
                                    var jsonLotPreview = jsonLotURL + 'PrjCode=' + canvasPrj;
                                    var imgBlockUrl = imgProject + 'id=' + canvasPrj;

                                    //get width and height of image
                                    var imgwidth = $('.imgProjectWidth').val();
                                    var imgheight = $('.imgProjectHeight').val();

                                    //get canvass width
                                    var Cwidth = document.getElementById('imgBlockPreview').offsetWidth;

                                    //get new canvass height
                                    var Cheight = Math.round((Cwidth * height) / width);

                                    //get percentage
                                    var percent = 0;
                                    var diff = Cwidth - width;
                                    percent = diff / width;


                                    //for summary preview
                                    var canvasPreview = this.__canvas = new fabric.Canvas('' + canID + '', {
                                        hoverCursor: 'pointer',
                                        selection: false,
                                        perPixelTargetFind: true,
                                        targetFindTolerance: 5,
                                        width: Cwidth,
                                        height: Cheight
                                    });




                                    const loadeachproj = await $.getJSON(jsonLotPreview, function (data) {
                                        var circle = canvasPreview.loadFromJSON(data, canvasPreview.renderAll.bind(canvasPreview), function (o, object) {
                                            object.fill = 'rgb(' + (hexToRgb(object.fill).r) + ', ' + (hexToRgb(object.fill).g) + ', ' + (hexToRgb(object.fill).b) + ',<%=ConfigurationManager.AppSettings["LotOpacity"].ToString() %>)';
                                            var rad = object.radius;
                                            object.radius = (object.radius * percent) + object.radius;
                                            object.left = ((percent * object.left) + object.left) + (object.radius - rad);
                                            object.top = ((percent * object.top) + object.top) + (object.radius - rad);
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
                                                y = 7; //one digit number
                                            }
                                            else {
                                                y = 12; //two digit number
                                            }
                                            //o.left = ((percent * o.left) + o.left);
                                            //o.top = ((percent * o.top) + o.top);
                                            var text = new fabric.Text(o.description, {
                                                //left: ((percent * o.left) + o.left) + y, //+7 to center text
                                                //top: ((percent * o.top) + o.top) + 5,
                                                left: o.left + y, //+7 to center text
                                                top: o.top + 5,
                                                //fill: 'black',
                                                fill: o.fill.replace(/[^,]+(?=\))/, '0.01'),
                                                fontSize: '<%=ConfigurationManager.AppSettings["MapFontSize"].ToString() %>',
                                                fontWeight: 'bold',
                                                name: o.name,
                                                Block: o.Block,
                                                description: o.description,
                                                selectable: false,
                                            });
                                            canvasPreview.add(text);
                                        });


                                        //set background image
                                        canvasPreview.setBackgroundImage(imgBlockUrl, canvasPreview.renderAll.bind(canvasPreview), {
                                            backgroundImageOpacity: 0.5,
                                            backgroundImageStretch: true,
                                            scaleX: canvasPreview.width / width,
                                            scaleY: canvasPreview.height / height

                                        });

                                        canvasPreview.forEachObject(function (x) {
                                            console.log(x);
                                            x.hasBorders = x.hasControls = false;
                                            x.lockMovementX = true;
                                            x.lockMovementY = true;
                                            //x.left = ((percent * x.left) + x.left);
                                            //x.top = ((percent * x.top) + x.top);
                                        });
                                        //var canvas = document.getElementById('' + canID + '');
                                        //var imgdata = canvas.toDataURL("image/jpg", 1.0);
                                        //cavasData.push(imgdata);
                                    });

                                    break;

                                }
                            }

                        }

                        let doneloop = await loadproj();

                    })
                    //UNCOMMENT IF WE ARE ALREADY INTEGRATING
                    //$("#download").removeClass('hide');
                }
                loadprojectlist();



            });

        };

        //});




        //** Generate Per Project **//
        function download2() {
            console.log('Clicked GENERATOR!');

            // ****** Sigle Porject Map generation ********** //
            var canvas = document.getElementById('imgBlockPreview');
            var imgData = canvas.toDataURL("image/jpeg", 1.0);
            var imge = $("#sketch_data").val(imgData);
            //console.log(imgData);
            //var projCode = $('.txtProjId').val();
            //var projName = $('.txtProjName').val();
            // ****** Sigle Porject Map generation  **********  for canvas//
        }



        //CLIENT SIDE
        function download() {
            console.log('Test');
            var canvas = document.getElementById('imgBlockPreview');
            // only jpeg is supported by jsPDF
            var imgData = canvas.toDataURL("image/jpeg", 1.0);
            console.log(imgData);

            var pdf = new jsPDF('', 'mm', [canvas.width, canvas.height]);
            pdf.addImage(imgData, 'JPEG', 0, 0, canvas.width, canvas.height);
            pdf.save("download.pdf");

        }


        function msg() {
            alert('test');
        }

        //Convert color from hex to rgb
        function hexToRgb(hex) {
            var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
            return result ? {
                r: parseInt(result[1], 16),
                g: parseInt(result[2], 16),
                b: parseInt(result[3], 16)
            } : null;
        }
    </script>


    <script type="text/javascript"> 
        function drawProjectMap() {

            //get width and height of image
            var width = $('.imgProjectWidth').val();
            var height = $('.imgProjectHeight').val();

            //get canvass width
            var Cwidth = document.getElementById('imgBlockPreview').offsetWidth;

            //get new canvass height
            var Cheight = Math.round((Cwidth * height) / width);

            //get percentage
            var percent = 0;
            var diff = Cwidth - width;
            percent = diff / width;

            //New Dashboard plot map markers base on json lot
            var proj = $('.txtProjId').val();
            var jsonLotURL = '<%=ConfigurationManager.AppSettings["jsonNewPrjwithColor"].ToString() %>'; //get json for project lot from web config
            var imgProject = '<%=ConfigurationManager.AppSettings["imgProject"].ToString() %>';//get img handler for blocks from web config
            var jsonLotPreview = jsonLotURL + 'PrjCode=' + proj;
            var imgBlockUrl = imgProject + 'id=' + proj;



            //for summary preview
            var canvasPreview = this.__canvas = new fabric.Canvas('imgBlockPreview', {
                hoverCursor: 'pointer',
                selection: false,
                perPixelTargetFind: true,
                targetFindTolerance: 5,
                width: Cwidth,
                height: Cheight
            });

            $.getJSON(jsonLotPreview, function (data) {
                var circle = canvasPreview.loadFromJSON(data, canvasPreview.renderAll.bind(canvasPreview), function (o, object) {
                    object.fill = 'rgb(' + (hexToRgb(object.fill).r) + ', ' + (hexToRgb(object.fill).g) + ', ' + (hexToRgb(object.fill).b) + ',<%=ConfigurationManager.AppSettings["LotOpacity"].ToString() %>)';
                    var rad = object.radius;
                    object.radius = (object.radius * percent) + object.radius;
                    object.left = ((percent * object.left) + object.left) + (object.radius - rad);
                    object.top = ((percent * object.top) + object.top) + (object.radius - rad);
                    fabric.log(o, object);
                });
                //object.left = ((percent * object.left) + object.left);
                //object.top = ((percent * object.top) + object.top);
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
                        y = 7; //one digit number
                    }
                    else {
                        y = 12; //two digit number
                    }
                    //o.left = ((percent * o.left) + o.left);
                    //o.top = ((percent * o.top) + o.top);
                    var text = new fabric.Text(o.description, {
                        //left: ((percent * o.left) + o.left) + y, //+7 to center text
                        //top: ((percent * o.top) + o.top) + 5,
                        left: o.left + y, //+7 to center text
                        top: o.top + 5,
                        //fill: 'black',
                        fill: o.fill.replace(/[^,]+(?=\))/, '0.01'),
                        fontSize: '<%=ConfigurationManager.AppSettings["MapFontSize"].ToString() %>',
                        fontWeight: 'bold',
                        name: o.name,
                        Block: o.Block,
                        description: o.description,
                        selectable: false,
                    });
                    canvasPreview.add(text);
                });

                //set background image
                canvasPreview.setBackgroundImage(imgBlockUrl, canvasPreview.renderAll.bind(canvasPreview), {
                    backgroundImageOpacity: 0.5,
                    backgroundImageStretch: true,
                    scaleX: canvasPreview.width / width,
                    scaleY: canvasPreview.height / height

                });

                canvasPreview.forEachObject(function (x) {
                    console.log(x);
                    x.hasBorders = x.hasControls = false;
                    x.lockMovementX = true;
                    x.lockMovementY = true;
                    //x.left = ((percent * x.left) + x.left);
                    //x.top = ((percent * x.top) + x.top);
                });

                //****mouse events****
                //mouse down
                canvasPreview.on('mouse:down', function (e) {
                    if (e.target != null) {
                        canvasPreview.renderAll();
                        $('#<%= txtSelectedBlock.ClientID%>').val(e.target.Block);

                        $('#<%= txtSelectedLot.ClientID%>').val(e.target.name);
                        $('.bGenerate').click();
                    }
                });


<%--                canvasPreview.on('mouse:up', function (e) {
                    if (e.target != null) {
                        canvasPreview.renderAll();
                        $('#<%= txtSelectedLot.ClientID%>').val(e.target.name);
                        console.log(e.target.name);
                        $('.bGenerate').click();

                    }
                });--%>

                //pdf.addImage(imgData, 'JPEG', 0, 0);
                //pdf.save("download.pdf");

                //****mouse events****
                //mouse down
                <%--canvasPreview.on('mouse:down', function (e) {
                    if (e.target != null) {
                        //show lot preview modal
                        $('#<%= txtSelectedBlock.ClientID %>').val(e.target.name);
                        document.getElementById('<%= btnLotPreview.ClientID %>').click();
                    }
                });--%>
                //mouse over
                //canvasPreview.on('mouse:over', function (e) {
                //    if (e.target != null) {
                //        if (e.target.type == 'text') {
                //            e.target.setFill('white');
                //        }
                //        else {
                //            e.target.setFill('red');
                //        }
                //        canvasPreview.renderAll();
                //    }
                //});
                //mouse out
                //canvasPreview.on('mouse:out', function (e) {
                //    if (e.target != null) {
                //        if (e.target.type == 'text') {
                //            e.target.setFill('white');
                //        }
                //        else {
                //            e.target.setFill('green');
                //        }
                //        canvasPreview.renderAll();
                //    }
                //});
            });


        }







    </script>
    <%--<script type="text/javascript">
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
                        y = 7; //one digit number
                    }
                    else {
                        y = 12; //two digit number
                    }
                    var text = canvasPreview.add(new fabric.Text(o.description, {
                        left: o.left + y, //+7 to center text
                        top: o.top + 5,
                        fill: 'white',
                        fontSize: '<%=ConfigSettings.LotRadius"].ToString() %>',
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

            var imgData = canvasPreview.toDataURL("image/jpeg", 1.0);
            var pdf = new jsPDF();

            pdf.addImage(imgData, 'JPEG', 0, 0);
            pdf.save("download.pdf");

        }
    </script>--%>
    <%-- LOT LOCATION --%>
    <%--    <script type="text/javascript">
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
            var jsonProjectLot = '<%=ConfigSettings.jsonPrjwithColor"].ToString() %>'; //get json for project lot from web config

            var imgBlock = '<%=ConfigSettings.imgBlock"].ToString() %>';//get img handler for blocks from web config
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
                        y = 5; //one digit number
                    }
                    else {
                        y = 15;
                    }
                    var text = canvas.add(new fabric.Text(o.description, {
                        left: o.left + y,
                        top: o.top + 5,
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
            var imgData = canvasPreview.toDataURL("image/jpeg", 1.0);
            var pdf = new jsPDF();

            pdf.addImage(imgData, 'JPEG', 0, 0);
            pdf.save("download.pdf");

        };


    </script>--%>
    <script>
        //document.getElementById("download").addEventListener("click", function () {
        //    console.log('test 12345');

        //    // only jpeg is supported by jsPDF
        //    var imgData = $('#imgBlockPreview').toDataURL("image/jpeg", 1.0);
        //    var pdf = new jsPDF();

        //    pdf.addImage(imgData, 'JPEG', 0, 0);
        //    pdf.save("download.pdf");
        //}, false);

    </script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/1.3.2/jspdf.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="sitemap" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div class="panel panel-trans">
        <div class="panel-heading">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    Project Map 
                        <%--<button id="btnExtractCanvas" onclick="extractCanvas();">Test</button>--%>
                    <%-- <button id="download" type="button" onclick="download1();">download</button>--%>
                    <button id="btnGenerateRep" runat="server" onclick="download2();" onserverclick="btnGenerateRep_ServerClick" type="button" visible="false">Generate Single Project</button>
                    <input type="hidden" id="sketch_data" name="sketch_data" />

                    <button id="download" class="hide" type="button" onclick="download1()" visible="false">Generate Multiple Proj Rep</button>

                    <%--// ETO YUNG UNANG NAKALABAS--%>
                    <button id="btnGenerateTest" runat="server" onclick="download2();" onserverclick="btnGenerateTest_ServerClick" type="button">Generate Test Per Project</button>

                    <%--// ETO YUNG PANGALAWANG NAKALABAS--%>
                    <button id="btnGenerateTestAll" runat="server" onclick="download1test();" type="button">Generate Test All Projects</button>
                    <%--<button id="btnGenerateTestAll" runat="server" onclick="download1test();" onserverclick="btnGenerateTestAll_ServerClick" type="button">Generate Test All Projects</button>--%>


                    <div class="row">
                        <div class="col-lg-3">
                            <div class="form-group">
                                <div class="input-group">
                                    <input type="text" placeholder="Project Code" style="z-index: auto;" class="form-control txtProjId" runat="server" id="txtProjId" disabled />
                                    <span class="input-group-btn">
                                        <button runat="server" id="btnShowProj" onserverclick="btnShowProj_ServerClick" style="width: 50px; z-index: auto;" class="btn btn-secondary btn-dropbox" type="button"><i class="fa fa-bars"></i></button>
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3">
                            <div class="form-group">
                                <input type="text" placeholder="Project Name" class="form-control txtProjName" id="txtProjName" runat="server" disabled />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-3">
                            Color Details
                            <button class="btn btn-default pull-right" type="button" style="width: auto; height: auto;" data-toggle="collapse" data-target="#profile"><i class="fa fa-list"></i></button>
                        </div>
                    </div>
                    <div class="row">
                        <div class="collapse" id="profile">
                            <div class="table-responsive">
                                <div class="col-lg-3">
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
                    </div>
                    <%--<div class="row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                   
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>--%>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <%-- DASHBOARD panel body -EPI--%>
    <%--    <div class="col-lg-12">
        <div class="panel panel-default">
            <div class="panel-body">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="row" style="overflow: auto; margin: 0 auto;">
                            <div class="col-lg-12">
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <p align="center">
                                            <canvas id="imgBlockPreview" style="border: 1px solid black; margin: 0 auto;" width="600" height="400">Your browser does not support the Canvas Element
                                            </canvas>
                                            <canvas id="imgBlockReport" style="border: 1px solid black; margin: 0 auto; display: none;">Your browser does not support the Canvas Element
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
    </div>--%>

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="row" style="overflow: auto; margin: 0 auto;">
                <div class="col-lg-12">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <p align="center">
                                <canvas id="imgBlockPreview" style="border: 1px solid black; margin: 0 auto; width: 100%">Your browser does not support the Canvas Element 
                                </canvas>
                            </p>
                            <p align="center" id="canvasForm" class="hidden">
                            </p>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="col-lg-12">
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
                                        EmptyDataText="No Records Found"
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
        <div class="modal-dialog modal-lg" role="dialog" style="width: 90%">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Lot List</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-2 col-md-2 col-sm-12 col-xs-12">
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                    <h4>Legend</h4>
                                </div>
                            </div>
                            <%--<div class="row">
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
                            </div>--%>
                        </div>
                        <div class="col-lg-10 col-md-10 col-sm-12 col-xs-12">
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
        <div class="modal-dialog modal-lg" role="dialog" style="width: 90%;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Lot Information</h4>
                </div>
                <div class="modal-body" style="overflow: auto; height: auto; background-color: lightgray">

                    <%--       <div class="col-lg-12" style="background-color: cornflowerblue; height: 5px;">
                                <asp:Label runat="server" Text=" "></asp:Label>
                            </div>
                            <br />--%>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <%--//LOAD IMAGE--%>
                            <div class="col-lg-9">
                                <div class="panel panel-primary">
                                    <div class="row">
                                        <div class="col-lg-12">
                                            <p align="center">
                                                <asp:Image runat="server" ID="imgHouse" Width="100%" Height="100%" />
                                            </p>
                                            <%--<asp:Image runat="server" ID="imgHouse" ImageUrl='<%# "~/Handler/HouseModel.ashx?ID=" + Eval("U_Picture")%>' Width="1050px" Height="85%" />--%>
                                        </div>
                                        <%--   <div class="col-lg-12">
                                                <asp:LinkButton runat="server" ID="bPicPreview" CssClass="btn btn-primary" Width="100%" Height="100%" OnClick="bPicPreview_Click" CommandArgument='<%# Bind("DocEntry")%>'><i class="fa fa-image"> Preview</i></asp:LinkButton>
                                            </div>--%>
                                    </div>
                                </div>
                            </div>

                            <%--//LOAD INFORMATION--%>
                            <div class="col-lg-3">
                                <div class="panel panel-success" style="background-color: white;">
                                    <div class="row">
                                        <div class="col-lg-12">
                                            <div class="col-lg-6">
                                                Model :
                                            </div>
                                            <div class="col-lg-6">
                                                <asp:Label runat="server" ID="lblModel"></asp:Label>
                                            </div>
                                            <div class="col-lg-6">
                                                Floor Area :
                                            </div>
                                            <div class="col-lg-6">
                                                <asp:Label runat="server" ID="lblFloorArea"></asp:Label>
                                            </div>
                                            <div class="col-lg-6">
                                                Reservation Fee :
                                            </div>
                                            <div class="col-lg-6">
                                                <asp:Label runat="server" CssClass="text-info" Font-Size="10" ID="lblResFee"></asp:Label>
                                            </div>
                                            <div class="col-lg-6">
                                                No. of Buyers : 
                                            </div>
                                            <div class="col-lg-6">
                                                <asp:Label runat="server" CssClass="text-info" Font-Size="10" ID="lblNoOfBuyers"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
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

    <%-- <div id="MsgPicPreview" class="modal fade" role="dialog" tabindex="-1">
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

    --%>

    <%--MODAL ALERT--%>
    <div id="modalAlert" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="row" style="text-align: center;">
                                <div class="col-lg-12">
                                    <asp:Image ImageUrl="~/assets/img/success.png" runat="server" ID="alertIcon" Width="90" Height="90" />
                                </div>
                            </div>
                            <div class="row" style="text-align: center;">
                                <div class="col-lg-12">
                                    <h4>
                                        <asp:Label runat="server" ID="lblMessageAlert"></asp:Label></h4>
                                </div>
                            </div>
                            <div class="row" style="text-align: center;">
                                <div class="col-lg-12">
                                    <button type="button" class="btn btn-primary btn-sm" data-dismiss="modal" style="width: 90px;" onclick="hideAlert()">Ok</button>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

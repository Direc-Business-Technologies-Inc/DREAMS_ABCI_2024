<%@ Page Title="DREAMS | Quotation" Language="C#" MasterPageFile="~/master/Main.Master" AutoEventWireup="true" CodeBehind="Quotation.aspx.cs" Inherits="ABROWN_DREAMS.Quotation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--############[ ENABLE/DISABLE WIZARD ]############--%>
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

            $("#MsgBlockLotList").on("shown.bs.modal", function () {
                drawBlockMap();
            });
        });
        function nextTab(elem) {
            $(elem).next().find('a[data-toggle="tab"]').click();
        }
        function prevTab(elem) {
            $(elem).prev().find('a[data-toggle="tab"]').click();
        }


        /* END EXTERNAL SOURCE */

        /* BEGIN EXTERNAL SOURCE */


        function ResetTextBox() {
            $('.ResetTextBox').find('input:text').val('');
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
            //$('#MsgBuyers').modal('show');
            $('#MsgBuyers').modal({
                backdrop: 'static',
                keyboard: false
            });
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
        function MsgBlockLotList_Show() {
            $('#MsgBlockLotList').modal('show');
        }
        function MsgLotList_Show() {
            $('#MsgLotList').modal('show');
        }
        function MsgBlockLotList_Hide() {
            $('#MsgBlockLotList').modal('hide');
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
        function MsgSampleActual_Show() {
            $('#MsgSampleActual').modal('show');
        }
        function MsgSampleActual_Hide() {
            $('#MsgSampleActual').modal('hide');
        }
        function MsgHouseModel_Show() {
            $('#MsgHouseModel').modal('show');
        }
        function MsgHouseModel_Hide() {
            $('#MsgHouseModel').modal('hide');
        }

        function MsgLoanType_Show() {
            $('#MsgLoanType').modal('show');
        }
        function MsgLoanType_Hide() {
            $('#MsgLoanType').modal('hide');
        }

        function MsgAccreditedBanks_Show() {
            $('#MsgAccreditedBanks').modal('show');
        }
        function MsgAccreditedBanks_Hide() {
            $('#MsgAccreditedBanks').modal('hide');
        }

        function MsgRetitlingType_Show() {
            $('#MsgRetitlingType').modal('show');
        }
        function MsgRetitlingType_Hide() {
            $('#MsgRetitlingType').modal('hide');
        }

        function MsgAdjLot_Show() {
            $('#MsgAdjLot').modal('show');
        }
        function MsgAdjLot_Hide() {
            $('#MsgAdjLot').modal('hide');
        }

        function MsgLotNo_Show() {
            $('#MsgLotNo').modal('show');
        }
        function MsgLotNo_Hide() {
            $('#MsgLotNo').modal('hide');
        }

        function MsgIncentive_Show() {
            $('#MsgIncentive').modal('show');
        }
        function MsgIncentive_Hide() {
            $('#MsgIncentive').modal('hide');
        }

        function MsgCoBorrower_Show() {
            $('#modal-coborrower').modal('show');
        }
        function MsgCoBorrower_Hide() {
            $('#modal-coborrower').modal('hide');
        }
        function MsgCoOwnermodal_Show() {
            $('#modal-coowner').modal('show');
        }
        function MsgCoOwnermodal_Hide() {
            $('#modal-coowner').modal('hide');
        }
        function disableModelBtn() {
            document.getElementById("<%=bModel.ClientID%>").disabled = true;
        }
        function enableModelBtn() {
            document.getElementById("<%=bModel.ClientID%>").disabled = false;
        }
        function MsgCoMaker_Hide() {
            $('#MsgCoMaker').modal('hide');
        }
        function MsgCoOwner_Hide() {
            $('#MsgCoOwner').modal('hide');
        }
        function MsgOthers_Hide() {
            $('#MsgOthers').modal('hide');
        }
        function showMsgConfirm() {
            $('#modalConfirmation').modal('show');
        }
        function hideMsgConfirm() {
            $('#modalConfirmation').modal('hide');
            hidemodalOTP();
        }
        function showmodalOTP() {
            $('#modalOTP').modal('show');
        }
        function hidemodalOTP() {
            $('#modalOTP').modal('hide');
        }

        function showConfirmation() {
            $('#modalConfirmation1').modal({
                backdrop: 'static',
                keyboard: false
            });
        }

        function hideConfirm() {
            $('#modalConfirmation1').modal('hide');
        }

        function fnAllowNumeric(evt) {
            // Allow numeric characters and dashes
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            var charStr = String.fromCharCode(charCode);
            if ((charCode < 48 || charCode > 57) && charCode != 8 && charStr != "-")
                return false;
            return true;
        }
        function TIN_keyup(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode == 8 || charCode == 46) { }
            else {
                var elem = document.getElementById(evt.target.id);
                if (elem.value.length < 15) {
                    if (elem.value.length == 4) {
                        elem.value = [elem.value.slice(0, 3), "-", elem.value.slice(3)].join('');
                    }
                    if (elem.value.length == 8) {
                        elem.value = [elem.value.slice(0, 7), "-", elem.value.slice(7)].join('');
                    }
                    if (elem.value.length == 12) {
                        elem.value = [elem.value.slice(0, 11), "-", elem.value.slice(11)].join('');
                    }
                }
            }
        }







        //** Bootstrap Card Tab **//
        $(document).ready(function () {
            $(".btn-pref .btn").click(function () {
                $(".btn-pref .btn").removeClass("btn-primary").addClass("btn-default");
                $(this).removeClass("btn-default").addClass("btn-primary");
            });
        });
        //

        /* END EXTERNAL SOURCE */

        /* BEGIN EXTERNAL SOURCE */

        function formValid() {
            $('#form_quotation').find('input[required]').each(function () {
                if ($(this).val() == '') {
                    $('#isValid').val('false');
                    $(this).addClass("required-input");
                    return false;
                }
                else {
                    $('#isValid').val('true');
                }
            });

            $('#form_quotation').find('input[required]:enabled').each(function () {
                if ($(this).val() == '') {
                    $(this).addClass("required-input");

                    //loop all input required with null values
                }
                else {
                    $(this).removeClass("required-input");
                }
            });

            //remove required class in disabled input
            $('#form_quotation').find('input[required]:disabled').each(function () {
                $(this).removeClass("required-input");
            });
        }

        /* END EXTERNAL SOURCE */

        /* BEGIN EXTERNAL SOURCE */

        /*********************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************/

        function AddOption(ddl, text, value) {
            var opt = document.createElement("OPTION");
            opt.text = text;
            opt.value = value;
            ddl.options.add(opt);
        }

        /*******************************************************************************************************************************************************************************************************************************************************************************************************************************************/

        /* END EXTERNAL SOURCE */

        /* BEGIN EXTERNAL SOURCE */

        function clientSampleClick() {
            $("#tabSalesPerson").hide();
        }
        function clientCreateAmortClick() {

            $("#tabSalesPerson").show();
        }


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
        //function tNextTab() {
        //    $('#btnSummaryComp').prop("disabled", false);
        //    var $active = $('.wizard .nav-tabs li.active');
        //    nextTab($active);
        //    //$('#btnSummaryComp').attr("disabled", true); 
        //}
        function PrevTab(tab) {
            $('.nav-tabs a[href="#' + tab + ']').removeClass('disabled');
            Prev();
        }
    </script>
    <%--############[ END ENABLE/DISABLE WIZARD ]#############--%>
    <%-- Modals --%>
    <script type="text/javascript"></script>

    <%-- Valid Values --%>
    <script type="text/javascript"></script>

    <%-- Date Populate --%>
    <script type="text/javascript"><%--   function PopulateDays() {
            var ddlMonth = document.getElementById("<%=ddlDPMonth.ClientID%>");
            var ddlYear = document.getElementById("<%=ddlDPYear.ClientID%>");
            var ddlDay = document.getElementById("<%=ddlDPDay.ClientID%>");
            var y = ddlYear.options[ddlYear.selectedIndex].value;
            var m = ddlMonth.options[ddlMonth.selectedIndex].value != 0;
            if (ddlMonth.options[ddlMonth.selectedIndex].value != 0 && ddlYear.options[ddlYear.selectedIndex].value != 0) {
                var dayCount = 32 - new Date(ddlYear.options[ddlYear.selectedIndex].value, ddlMonth.options[ddlMonth.selectedIndex].value - 1, 32).getDate();
                ddlDay.options.length = 0;
                AddOption(ddlDay, "DD", "0");
                for (var i = 1; i <= dayCount; i++) {
                    AddOption(ddlDay, i, i);
                }
            }
        }--%><%--    function Validate(sender, args) {
            var ddlMonth = document.getElementById("<%=ddlDPMonth.ClientID%>");
            var ddlYear = document.getElementById("<%=ddlDPYear.ClientID%>");
            args.IsValid = (ddlDay.selectedIndex != 0 && ddlMonth.selectedIndex != 0 && ddlYear.selectedIndex != 0)
        }--%></script>

    <%-- Navigation for Wizard --%>
    <script type="text/javascript"></script>
    <%-- BLOCK LOCATION --%>
    <script type="text/javascript">
        <%--Convert color from hex to rgb--%>
        function hexToRgb(hex) {
            var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
            return result ? {
                r: parseInt(result[1], 16),
                g: parseInt(result[2], 16),
                b: parseInt(result[3], 16)
            } : null;
        }

        function drawBlockMap() {
            var width = $('.hBlockWidth').val();
            console.log(width);
            var height = $('.hBlockHeight').val();
            console.log(height);
            console.log('test');

            var Cwidth = $("#Divblocklot").width();

            //get new canvass height
            var Cheight = Math.round((Cwidth * height) / width);
            console.log(Cwidth);
            console.log(Cheight);

            //get percentage
            var percent = 0;
            var diff = Cwidth - width;
            percent = diff / width;

            // initialize fabric canvas and assign to global windows object for debug
            var canvas = this.__canvas = new fabric.Canvas('imgProjectBlock', {
                hoverCursor: 'pointer',
                selection: false,
                perPixelTargetFind: true,
                targetFindTolerance: 5,
                width: Cwidth,
                height: Cheight
            });

            //plot map markers base on json
            var proj = $('.hPrjCode').val();
            var jsonLotURL = '<%=ConfigurationManager.AppSettings["jsonNewPrjwithColor"].ToString() %>'; //get json for project lot from web config
            var imgProject = '<%=ConfigurationManager.AppSettings["imgProject"].ToString() %>';//get img handler for blocks from web config
            var jsonLot = jsonLotURL + 'PrjCode=' + proj;
            var imgBlockUrl = imgProject + 'id=' + proj;

            $.getJSON(jsonLot, function (data) {
                var circle = canvas.loadFromJSON(data, canvas.renderAll.bind(canvas), function (o, object) {
                    object.fill = 'rgb(' + (hexToRgb(object.fill).r) + ', ' + (hexToRgb(object.fill).g) + ', ' + (hexToRgb(object.fill).b) + ',<%=ConfigurationManager.AppSettings["LotOpacity"].ToString() %>)';
                    var rad = object.radius;
                    object.radius = (object.radius * percent) + object.radius;
                    object.left = ((percent * object.left) + object.left) + (object.radius - rad);
                    object.top = ((percent * object.top) + object.top) + (object.radius - rad);
                    fabric.log(o, object);
                });
                //loop all opbject
                canvas.forEachObject(function (o) {
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
                    var text = canvas.add(new fabric.Text(o.description, {
                        left: o.left + y, //+7 to center text
                        top: o.top + 5,
                        //fill: 'black',
                        fill: o.fill.replace(/[^,]+(?=\))/, '0.01'),
                        fontSize: '<%=ConfigurationManager.AppSettings["MapFontSize"].ToString() %>',
                        fontWeight: 'bold',
                        name: o.name,
                        status: o.status,
                        Block: o.Block,
                        description: o.description,
                        selectable: false
                    }));
                });
                canvas.forEachObject(function (x) { x.hasBorders = x.hasControls = false; x.lockMovementX = true; x.lockMovementY = true; });

                //set background image
                canvas.setBackgroundImage(imgBlockUrl, canvas.renderAll.bind(canvas), {
                    backgroundImageOpacity: 0.5,
                    backgroundImageStretch: true,
                    scaleX: canvas.width / width,
                    scaleY: canvas.height / height
                });
                //****mouse events****
                //mouse down 
                canvas.on('mouse:down', function (e) {
      <%--              if (e.target != null) {
                        $('#<%= tBlock.ClientID %>').val(e.target.Block);
                        $('#<%= tBlock2.ClientID %>').val(e.target.Block);
                        console.log(e.target.Block);
                        document.getElementById('<%= bLot.ClientID %>').click();
                    }--%>
                    console.log(e.target.status);
                    console.log(e.target.Block);
                    console.log(e.target.name);

                    if (e.target.status == "S01") {
                        //set value
                        console.log(e.target.status);
                        $('#<%= tBlock.ClientID %>').val(e.target.Block);
                        $('#<%= tBlock2.ClientID %>').val(e.target.Block);
                        $('#<%= tLot.ClientID%>').val(e.target.name);
                        $('#<%= tLot2.ClientID%>').val(e.target.name);
                        document.getElementById('<%= bLot.ClientID %>').click();
                    }
                    else if (e.target.status == "S02") {
                        $('#<%= lblMsg.ClientID %>').val("The selected lot is not available for sale");
                        MsgNoti_Show();
                    }
                    if (e.target.status == "S03") {
                        $('#<%= lblMsg.ClientID %>').val("The selected lot is already sold. Block: " + e.target.Block + " Lot: " + e.target.name);
                        MsgNoti_Show();
                    }
                });
            });
        }
    </script>

    <%-- LOT LOCATION --%>
    <script type="text/javascript">
        function drawLotMap() {
            var width = $('.hLotWidth').val();
            var height = $('.hLotHeight').val();

            // initialize fabric canvas and assign to global windows object for debug
            var canvas = this.__canvas = new fabric.Canvas('imgProjectLot', {
                hoverCursor: 'pointer',
                selection: false,
                perPixelTargetFind: true,
                targetFindTolerance: 5
            });
            //plot map markers base on json
            var proj = $('.hPrjCode').val();
            var block = $('.tBlock').val();
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
                        fill: 'black',
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
                        //e.target.setFill('red');
                        canvas.renderAll();//INSERT CODE HERE
                        if (e.target.status != "S06" || e.target.status != "S33" || e.target.status != "Hold" || e.target.status != "Sold") {
                            //set value
                            $('#<%= tLot.ClientID%>').val(e.target.name);
                            $('#<%= tLot2.ClientID%>').val(e.target.name);
                            $('.bGenerate').click();
                        }
                        else {
                            $('#<%= lblMsg.ClientID %>').val("The selected lot is not available");
                            MsgNoti_Show();
                        }
                    }
                });

                canvas.setWidth(width);
                canvas.setHeight(height);
            });
        };
    </script>

    <script type="text/javascript">
        $("a.SRF").on("click", function () {
            window.open('../pages/ReportViewer.aspx', '_blank');
        });
        function newtab() {
            window.open('www.google.com', '_blank');
        }
        function SetTarget() {
            document.forms[0].target = "_blank";
        }



        //2023-06-30 : TIN FORMAT
        $(document).ready(function () {
            $('#<%= tTIN1.ClientID %>').mask('000-000-000-000');
            $('#<%= txtTinNumber.ClientID %>').mask('000-000-000-000');
        });


    </script>
</asp:Content>










<asp:Content ID="Content2" ContentPlaceHolderID="sitemap" runat="server">
    <li><a href="#"><span>Quotation</span></a></li>
</asp:Content>





<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <input type="text" runat="server" id="hPrjCode" class="hPrjCode hide" />
            <input type="text" runat="server" id="hBlockWidth" class="hBlockWidth hide" />
            <input type="text" runat="server" id="hBlockHeight" class="hBlockHeight hide" />
            <input type="text" runat="server" id="hLotWidth" class="hLotWidth hide" />
            <input type="text" runat="server" id="hLotHeight" class="hLotHeight hide" />
            <button runat="server" id="bLot" onserverclick="bLot_ServerClick" class="bLot hide" />
            <button runat="server" id="bGenerate" onserverclick="bGenerate_ServerClick" class="bGenerate hide" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="panel panel-primary panel-trans">
        <div class="panel-heading">
            <h5>QUOTATION</h5>
        </div>
    </div>
    <div class="col-lg-4">
        <div class="panel panel-success" style="background-color: white;">
            <div class="panel-body ">
                <div class="row">

                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="col-lg-10">
                                <div class="customer-badge">
                                    <%--<asp:LinkButton runat="server" ID="btnFind" OnClick="btnFind_Click" CssClass="btn btn-info btn-circle pull-right"><span class="fa fa-search pull-right" style="color: #337ab7;"></span></asp:LinkButton>--%>
                                    <div class="avatar">
                                        <img src="../assets/img/user.png" alt="" width="50" height="50">
                                    </div>
                                    <div class="details">
                                        <a href="#">
                                            <asp:Label runat="server" ID="lblName" CssClass="text-uppercase" Style="font-size: 16px;" Text="" /></a>
                                        <br />
                                        <div class="text-success" />
                                        <asp:Label runat="server" ID="lblDocEntry" CssClass="small text-upper hidden" Text="" /><br />
                                        Buyer Code:
                                            <asp:Label runat="server" ID="lblID" CssClass="small text-upper" Text="" /><br />
                                        Document No.:
                                            <asp:Label runat="server" ID="lblDocNum" CssClass="small text-upper" Text="" /><br />
                                    </div>
                                </div>
                            </div>
                            </div>
                            <div class="col-lg-2">
                                <asp:LinkButton runat="server" ID="btnFind" OnClick="btnFind_Click" CssClass="btn btn-info btn-circle pull-right"><span class="fa fa-search" style="color: #337ab7;"></span></asp:LinkButton>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>

                <div class="row">
                    <div class="col-lg-12 col-sm-12">
                        <div class="btn-pref btn-group btn-group-justified btn-group-lg" role="group" aria-label="...">
                            <div class="btn-group" role="group">
                                <button type="button" id="profile" class="btn btn-primary" href="#tab1" data-toggle="tab">
                                    <span class="glyphicon glyphicon-user" aria-hidden="true"></span>
                                    <div class="hidden-xs">Profile</div>
                                </button>
                            </div>
                            <div class="btn-group" id="tabSalesPerson" role="group">
                                <button type="button" id="documents" class="btn btn-default" href="#tab2" data-toggle="tab">
                                    <span class="glyphicon glyphicon-list" aria-hidden="true"></span>
                                    <div class="hidden-xs">Sales Persons</div>
                                </button>
                            </div>
                            <div class="btn-group" role="group">
                                <button type="button" id="btnSummaryComp" class="btn btn-default" href="#tab3" data-toggle="tab">
                                    <span class="glyphicon glyphicon-list-alt" aria-hidden="true"></span>
                                    <div class="hidden-xs">Computation</div>
                                </button>
                            </div>
                        </div>




                        <div class="well" style="background-color: white;">
                            <div class="tab-content">

                                <%--CREATION OF BUYERS--%>
                                <div class="tab-pane fade in active" id="tab1">
                                    <div class="table-responsive" style="border: 0px;">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <button runat="server" id="btnEditProfile" class="btn btn-success pull-right">Edit &nbsp;<i class="fa fa-edit"></i></button>
                                                <table class="table">
                                                    <caption>Buyer's Details</caption>

                                                    <tr>
                                                        <th>Buyer Type:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblBusinessType" CssClass="text-uppercase" /></td>
                                                    </tr>
                                                    <tr runat="server" id="trComaker" visible="false">
                                                        <th>Co-maker:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblComaker" CssClass="text-uppercase" /></td>
                                                    </tr>

                                                    <tr runat="server" id="trLastName">
                                                        <th style="width: 50%">Last Name:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblLastName" CssClass="text-uppercase" /></td>
                                                    </tr>

                                                    <tr runat="server" id="trFirstName">
                                                        <th>First Name:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblFirstName" CssClass="text-uppercase" /></td>
                                                    </tr>
                                                    <tr runat="server" id="trMiddleName">
                                                        <th>Middle Name:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblMiddleName" CssClass="text-uppercase" /></td>
                                                    </tr>
                                                    <tr runat="server" id="trCompanyName">
                                                        <th>Company Name:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblCompanyName" CssClass="text-uppercase" /></td>
                                                    </tr>
                                                    <tr runat="server" id="trBirthday">
                                                        <th>Birthday:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblBirthday" CssClass="text-uppercase" /></td>
                                                    </tr>
                                                    <tr runat="server" id="trNatureEmployment">
                                                        <th>Nature of Employment:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblNatureofEmployment" CssClass="text-uppercase" /></td>
                                                    </tr>
                                                    <%--2023-05-05 : REQUESTED BY DHEZA--%>
                                                    <tr runat="server" id="trTaxClassification">
                                                        <th>Tax Classification:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblTaxClassification" CssClass="text-uppercase" /></td>
                                                    </tr>
                                                    <tr runat="server" id="trIdType">
                                                        <th>Type of ID:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblTypeofID" CssClass="text-uppercase" /></td>
                                                    </tr>
                                                    <tr runat="server" id="trIdNumber">
                                                        <th>ID Number:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblIDNo" CssClass="text-uppercase" /></td>
                                                    </tr>
                                                    <tr runat="server" id="trTin">
                                                        <th>TIN:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblTIN" CssClass="text-uppercase" /></td>
                                                    </tr>
                                                </table>

                                                <hr>

                                                <div class="row">
                                                    <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                                        <h5>Co-Borrower:</h5>
                                                    </div>
                                                    <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                        <div class="input-group">
                                                            <input type="text" style="z-index: auto;" class="form-control text-uppercase" runat="server" id="txtComaker" readonly /><span class="input-group-btn">
                                                                <button id="btnCoMaker" runat="server" style="width: 50px; z-index: auto;" data-toggle="modal" data-target="#MsgCoMaker" class="btn btn-secondary" type="button" onserverclick="btnCoMaker_ServerClick"><i class="fa fa-bars"></i></button>
                                                            </span>
                                                        </div>
                                                        <%--         <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator26"
                                                ControlToValidate="txtComaker"
                                                Text="Please fill Co-maker field."
                                                ValidationGroup="newQuotation"
                                                runat="server" Style="color: red" />--%>
                                                    </div>
                                                </div>

                                                <hr>


                                                <div runat="server" id="divcoowner">
                                                    <div class="row hidden">
                                                        <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                                            <h5>Co-Owner: <span class="color-red fsize-16">*</span></h5>
                                                        </div>
                                                        <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                            <div class="input-group">
                                                                <input type="text" style="z-index: auto;" class="form-control text-uppercase" runat="server" id="txtCoowner" readonly /><span class="input-group-btn">
                                                                    <button id="btnCoOwner" runat="server" style="width: 50px; z-index: auto;" data-toggle="modal" data-target="#MsgCoOwner" class="btn btn-secondary" type="button" onserverclick="btnCoOwner_ServerClick"><i class="fa fa-bars"></i></button>
                                                                </span>
                                                            </div>
                                                        </div>
                                                        <asp:CustomValidator
                                                            Style="color: red"
                                                            runat="server"
                                                            ID="cvCoOwner" CssClass="col-md-12"
                                                            ValidationGroup="newQuotation"
                                                            Text="Please add Co-Owner."
                                                            OnServerValidate="cvCoOwner_ServerValidate" />
                                                    </div>


                                                    <h4 runat="server" id="H1">Co-ownership </h4>

                                                    <div class="row">
                                                        <div class="col-lg-12">
                                                            <button runat="server" id="Button2" class="btn btn-default btn-primary" type="button" onserverclick="bCreateNewCoOwner_ServerClick">Select Co-owner/s</button>
                                                            <br />
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                            <fieldset>
                                                                <asp:GridView ID="gvCoOwner"
                                                                    runat="server"
                                                                    AllowPaging="true"
                                                                    AutoGenerateColumns="false"
                                                                    EmptyDataText="No Records Found"
                                                                    CssClass="table table-bordered table-hover"
                                                                    Width="100%"
                                                                    ShowHeader="True"
                                                                    PageSize="5"
                                                                    OnPageIndexChanging="gvCoOwner_PageIndexChanging">
                                                                    <HeaderStyle BackColor="#66ccff" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="Code" HeaderText="Code" SortExpression="Name" ItemStyle-Font-Size="Medium" ItemStyle-CssClass="text-uppercase" />
                                                                        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-Font-Size="Medium" ItemStyle-CssClass="text-uppercase" />
                                                                        <%--    <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="bSelectCoOwner" CssClass="btn btn-default btn-success" Width="100%" Height="100%" OnClick="bSelectCoOwner_Click" CommandArgument='<%# Bind("Name")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>

                                                                        <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center" HeaderText="Delete">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton runat="server" ID="bDeleteOwner" CssClass="btn btn-default btn-danger" Width="100%" Height="100%" OnClick="bDeleteOwner_Click" CommandArgument='<%# Bind("Code")%>'><i class="fa fa-trash"></i></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>


                                                                    </Columns>
                                                                    <PagerStyle CssClass="pagination-ys" />
                                                                    <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                                                </asp:GridView>
                                                            </fieldset>
                                                        </div>
                                                    </div>


                                                </div>



                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>


                                <%--BUYERS INFORMATION SHEET--%>
                                <div class="tab-pane fade in" id="tab2">
                                    <div class="table-responsive" style="border: 0px;">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <table class="table">
                                                    <caption></caption>

                                                    <%--<tr>
                                                        <td>
                                                            <asp:UpdatePanel runat="server">
                                                                <ContentTemplate>
                                                                    <asp:GridView runat="server" ID="gvPosList"
                                                                        CssClass="table table-hover table-responsive"
                                                                        AutoGenerateColumns="false"
                                                                        GridLines="None"
                                                                        ShowHeader="false">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="Code" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                                                            <asp:BoundField DataField="Name" ItemStyle-Width="20%" />
                                                                            <asp:TemplateField ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide">
                                                                                <ItemTemplate>
                                                                                    <asp:Label runat="server" ID="lblSalesCode" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:Label runat="server" ID="lblSalesName" ItemStyle-Width="80%" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <span class="input-group-btn">
                                                                                        <button id="bPosList" runat="server" style="width: 50px; z-index: auto;" data-toggle="modal" data-target="#MsgEmpList" class="btn btn-secondary" type="button" onserverclick="bPosList_ServerClick"><i class="fa fa-bars"></i></button>
                                                                                    </span>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <PagerStyle CssClass="pagination-ys" />
                                                                        <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" Font-Bold="true" />
                                                                    </asp:GridView>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>--%>
                                                    <tr>
                                                        <asp:LinkButton runat="server" CssClass="btn btn-primary" ID="btnSelectAgent" OnClick="btnSelectAgent_Click">
                                                                                 Select Agent <i class="glyphicon glyphicon-user"></i> 
                                                        </asp:LinkButton>
                                                    </tr>

                                                    <tr>
                                                        <th>ID:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblEmployeeID" CssClass="text-uppercase" /></td>
                                                    </tr>
                                                    <tr>
                                                        <th>Name:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblEmployeeName" CssClass="text-uppercase" /></td>
                                                    </tr>
                                                    <tr>
                                                        <th>Position:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblEmployeePosition" CssClass="text-uppercase" /></td>
                                                    </tr>
                                                    <tr class="hidden">
                                                        <th>BrokerID:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblEmployeeBrokerID" CssClass="text-uppercase" /></td>
                                                    </tr>

                                                    <%--2023-11-07 : ADDED FIELD FOR BROKER NAME OF SELECTED SALES AGENT --%>
                                                    <tr>
                                                        <th>Broker:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblEmployeeBrokerName" CssClass="text-uppercase" /></td>
                                                    </tr>

                                                </table>






                                                <div class="row">
                                                    <asp:GridView ID="gvSharingDetails" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                                        CssClass="table table-bordered table-hover" Width="100%" ShowHeader="True" PageSize="10" OnPageIndexChanging="gvSharingDetails_PageIndexChanging">
                                                        <HeaderStyle BackColor="#66ccff" />
                                                        <Columns>
                                                            <asp:BoundField DataField="TransID" HeaderText="ID" SortExpression="Name" ItemStyle-Font-Size="Small" ItemStyle-CssClass="hidden" />
                                                            <asp:BoundField DataField="SalesPersonNameSharedDetails" HeaderText="Name" SortExpression="Name" ItemStyle-Font-Size="Small" />
                                                            <asp:BoundField DataField="PositionSharedDetails" HeaderText="Position" SortExpression="Position" ItemStyle-Font-Size="Small" />
                                                        </Columns>
                                                        <PagerStyle CssClass="pagination-ys" />
                                                        <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" Font-Bold="true" />
                                                    </asp:GridView>
                                                </div>



                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>


                                <%--COMPUTATION SHEET--%>
                                <div class="tab-pane fade in" id="tab3">
                                    <div class="table-responsive" style="border: 0px;">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <div class="table-responsive">
                                                    <%--JOSES1--%>


                                                    <h4>Computation Sheet  </h4>

                                                    <div class="col-lg-12">
                                                        <div class="col-lg-6 text-left nomargin">
                                                            TCP Amount
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin">
                                                            <label runat="server" style="font-weight: normal;">PHP </label>
                                                            <asp:Label runat="server" Style="font-weight: normal;" ID="lblDASAmt">00.00 </asp:Label>
                                                        </div>
                                                    </div>

                                                    <div class=" col-lg-12 hidden">
                                                        <div class="col-lg-6 text-left nomargin">
                                                            Less: Promo Discount
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin" style="font-weight: normal;">
                                                            <label runat="server" style="font-weight: normal;">PHP </label>
                                                            <asp:Label runat="server" Style="font-weight: normal;" ID="lblPromoDisc">0.00 </asp:Label>
                                                            <%--<label runat="server" style="font-weight: normal;" id="lblPromoDisc">00.00 </label>--%>
                                                        </div>
                                                    </div>


                                                    <div class="col-lg-12">
                                                        <div class="col-lg-6 text-left nomargin" style="padding-left: 2em">
                                                            Less Discount
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin">
                                                            <label runat="server" style="font-weight: normal;">PHP </label>
                                                            <asp:Label runat="server" Style="font-weight: normal;" ID="lblDiscount"> 0.00 </asp:Label>
                                                            <%--<label runat="server" style="font-weight: normal;" id="lblDiscount">(175,000.00) </label>--%>
                                                        </div>
                                                    </div>

                                                    <div class="col-lg-12 nomargin" style="padding-top: 0;">
                                                        <div class="col-lg-6 text-right nomargin"></div>
                                                        <div class="col-lg-6 text-right nomargin">
                                                            <hr style="border-top: 1px solid black; margin-top: 0; margin-bottom: 0;">
                                                        </div>
                                                    </div>


                                                    <div class="col-lg-12">
                                                        <div class="col-lg-6 text-left nomargin">
                                                            <label runat="server">NET TCP </label>
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin">
                                                            <label runat="server">PHP </label>
                                                            <asp:Label runat="server" Style="font-weight: bold;" ID="lblNetDAS">00.00 </asp:Label>
                                                            <%--<label runat="server" id="lblNetDAS">3,315,000.00 </label>--%>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-12">
                                                        <div class="col-lg-6 text-left nomargin">
                                                            Add Miscellaneous Fees
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin">
                                                            <label runat="server" style="font-weight: normal;">PHP </label>
                                                            <asp:Label runat="server" Style="font-weight: normal;" ID="lblAddMiscFees">00.00 </asp:Label>
                                                        </div>
                                                        <div class="col-lg-12 nomargin" style="padding-top: 0;">
                                                            <div class="col-lg-6 text-right nomargin"></div>
                                                            <div class="col-lg-6 text-right nomargin">
                                                                <hr style="border-top: 1px solid black; margin-top: 0; margin-bottom: 0;">
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-12">
                                                        <div class="col-lg-6 text-left nomargin" style="font-weight: bold;">
                                                            Total
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin">
                                                            <label runat="server" style="font-weight: bold;">PHP </label>
                                                            <asp:Label runat="server" Style="font-weight: bold;" ID="lblCompTotal">00.00 </asp:Label>
                                                        </div>
                                                    </div>

                                                    <div class=" col-lg-12" style="padding-top: 2em; padding-bottom: 2em;">
                                                        <div class="col-lg-6 text-left nomargin" style="font-weight: bold;">
                                                            Reservation Fee
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin" style="font-weight: normal;">
                                                            <label runat="server" style="font-weight: bold;">PHP </label>
                                                            <asp:Label runat="server" Style="font-weight: bold;" ID="lblReserveFee">00.00 </asp:Label>
                                                            <%--<label runat="server" style="font-weight: normal;" id="lblNetDAS2">3,199,000.00 </label>--%>
                                                        </div>
                                                    </div>

                                                    <div class=" col-lg-12 hidden" style="padding-top: 2em;">
                                                        <div class="col-lg-6 text-left nomargin">
                                                            Gross TCP
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin" style="font-weight: normal;">
                                                            <label runat="server" style="font-weight: normal;">PHP </label>
                                                            <asp:Label runat="server" Style="font-weight: normal;" ID="lblNetDAS2">00.00 </asp:Label>
                                                            <%--<label runat="server" style="font-weight: normal;" id="lblNetDAS2">3,199,000.00 </label>--%>
                                                        </div>
                                                    </div>


                                                    <div class="col-lg-12 hidden">
                                                        <div class="col-lg-6 text-left nomargin" style="padding-left: 2em">
                                                            VAT
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin">
                                                            <label runat="server" style="font-weight: normal;">PHP </label>
                                                            <asp:Label runat="server" Style="font-weight: normal;" ID="lblVAT"> 0.00 </asp:Label>
                                                            <%--<label runat="server" style="font-weight: normal;" id="lblVAT">1,920.00 </label>--%>
                                                        </div>
                                                    </div>


                                                    <div class="col-lg-12 nomargin hidden" style="padding-top: 0;">
                                                        <div class="col-lg-6 text-right nomargin"></div>
                                                        <div class="col-lg-6 text-right nomargin">
                                                            <hr style="border-top: 1px solid black; margin-top: 0; margin-bottom: 0;">
                                                        </div>
                                                    </div>

                                                    <div class="col-lg-12 hidden" style="padding-bottom: 2em;">
                                                        <div class="col-lg-6 text-left nomargin">
                                                            <label runat="server">Net TCP </label>
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin">
                                                            <label runat="server">PHP </label>
                                                            <asp:Label runat="server" Style="font-weight: bold;" ID="lblNetTCP"> 0.00 </asp:Label>
                                                            <%--<label runat="server" id="lblNetTCP">3,315,000.00 </label>--%>
                                                        </div>
                                                    </div>

                                                    <div class=" col-lg-12 hidden" style="padding-bottom: 1em;">
                                                        <div class="col-lg-6 text-left nomargin" style="font-weight: bold;">
                                                            Misc Charges
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin" style="font-weight: bold;">
                                                            <label runat="server" style="font-weight: normal;">PHP </label>
                                                            <asp:Label runat="server" Style="font-weight: bold;" ID="lblAddMiscCharges">00.00 </asp:Label>
                                                            <%--<label runat="server" style="font-weight: normal;" id="lblAddMiscCharges">223,930.00 </label>--%>
                                                        </div>
                                                    </div>

                                                    <br />
                                                    <h4>TCP Payment Breakdown </h4>

                                                    <div class=" col-lg-12">
                                                        <div class="col-lg-6 text-left nomargin">
                                                            Financing Scheme
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin" style="font-weight: normal;">
                                                            <asp:Label runat="server" ID="lblTCPFinScheme"> </asp:Label>
                                                            <%--<label runat="server" id="lblDownPayment">3,199,000.00 </label>--%>
                                                        </div>
                                                    </div>

                                                    <div class="col-lg-12">
                                                        <div class="col-lg-6 text-left nomargin">
                                                            DP Due Date
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin">
                                                            <asp:Label runat="server" ID="lblDPDueDate"> - </asp:Label>
                                                            <%--<label runat="server" style="font-weight: normal;" id="Label6">- </label>--%>
                                                        </div>
                                                    </div>

                                                    <div class=" col-lg-12">
                                                        <div class="col-lg-6 text-left nomargin">
                                                            Downpayment
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin" style="font-weight: normal;">
                                                            <label runat="server" style="font-weight: normal;">PHP </label>
                                                            <asp:Label runat="server" ID="lblDownPayment"> 0.00 </asp:Label>
                                                            <%--<label runat="server" id="lblDownPayment">3,199,000.00 </label>--%>
                                                        </div>
                                                    </div>






                                                    <div class=" col-lg-12">
                                                        <div class="col-lg-6 text-left nomargin">
                                                            Less Reservation Fee
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin" style="font-weight: normal;">
                                                            <label runat="server" style="font-weight: normal;">PHP </label>
                                                            <asp:Label runat="server" ID="lblReserveFee2"> 0.00 </asp:Label>
                                                            <%--<label runat="server" id="lblDownPayment">3,199,000.00 </label>--%>
                                                        </div>
                                                    </div>

                                                    <div class="col-lg-12 nomargin" style="padding-top: 0;">
                                                        <div class="col-lg-6 text-right nomargin"></div>
                                                        <div class="col-lg-6 text-right nomargin">
                                                            <hr style="border-top: 1px solid black; margin-top: 0; margin-bottom: 0;">
                                                        </div>
                                                    </div>

                                                    <div class=" col-lg-12">
                                                        <div class="col-lg-6 text-left nomargin" style="font-weight: bold;">
                                                            Balance on Equity
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin" style="font-weight: bold;">
                                                            <label runat="server" style="font-weight: bold;">PHP </label>
                                                            <asp:Label runat="server" ID="lblBalance"> 0.00 </asp:Label>
                                                            <%--<label runat="server" id="lblDownPayment">3,199,000.00 </label>--%>
                                                        </div>
                                                    </div>

                                                    <div class=" col-lg-12">
                                                        <div class="col-lg-6 text-left nomargin">
                                                            DP Terms
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin" style="font-weight: normal;">
                                                            <asp:Label runat="server" ID="lblDPTerms"> 0.00 </asp:Label>
                                                            <%--<label runat="server" id="lblDownPayment">3,199,000.00 </label>--%>
                                                        </div>
                                                    </div>

                                                    <div class="col-lg-12 nomargin" style="padding-top: 0;">
                                                        <div class="col-lg-6 text-right nomargin"></div>
                                                        <div class="col-lg-6 text-right nomargin">
                                                            <hr style="border-top: 1px solid black; margin-top: 0; margin-bottom: 0;">
                                                        </div>
                                                    </div>

                                                    <div class=" col-lg-12">
                                                        <div class="col-lg-6 text-left nomargin" style="font-weight: bold;">
                                                            Monthly DP
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin" style="font-weight: normal;">
                                                            <label runat="server" style="font-weight: bold;">PHP </label>
                                                            <asp:Label runat="server" ID="lblTCPMonthly" Style="font-weight: bold;"> 0.00 </asp:Label>
                                                            <%--<label runat="server" id="lblDownPayment">3,199,000.00 </label>--%>
                                                        </div>
                                                    </div>

                                                    <div class="col-lg-12 nomargin" style="padding-top: 0;">
                                                        <div class="col-lg-6 text-right nomargin"></div>
                                                        <div class="col-lg-6 text-right nomargin">
                                                            <hr style="border-top: 1px solid black; margin-top: 0; margin-bottom: 0;">
                                                        </div>
                                                    </div>

                                                    <div class=" col-lg-12">
                                                        <div class="col-lg-6 text-left nomargin" style="font-weight: bold;">
                                                            Loanable Balance
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin" style="font-weight: normal;">
                                                            <label runat="server" style="font-weight: bold;">PHP </label>
                                                            <asp:Label runat="server" ID="lblLoanableBalance" Style="font-weight: bold;"> 0.00 </asp:Label>
                                                            <%--<label runat="server" id="lblDownPayment">3,199,000.00 </label>--%>
                                                        </div>
                                                    </div>
                                                    <div class=" col-lg-12">
                                                        <div class="col-lg-6 text-left nomargin">
                                                            LB Terms
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin" style="font-weight: normal;">
                                                            <asp:Label runat="server" ID="lblLBTerms"> 0 </asp:Label>
                                                            <%--<label runat="server" id="lblDownPayment">3,199,000.00 </label>--%>
                                                        </div>
                                                    </div>



                                                    <%-- <div class="col-lg-12" style="padding-bottom: 2em;" runat="server" id="divMonthly2">--%>
                                                    <div class="col-lg-12" style="padding-bottom: 2em;">
                                                        <div class="col-lg-6 text-left nomargin">
                                                            Monthly LB
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin" style="font-weight: normal;">
                                                            <label runat="server" style="font-weight: normal;">PHP</label>
                                                            <asp:Label runat="server" ID="lblLBMonthly"> 0.00 </asp:Label>
                                                            <%--<label runat="server" style="font-weight: normal;" id="lblMonthly2">(175,000.00) </label>--%>
                                                        </div>
                                                    </div>



                                                    <div class="col-lg-12 hidden">
                                                        <div class="col-lg-6 text-left nomargin">
                                                            LB Due Date
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin">
                                                            <asp:Label runat="server" ID="lblLBDueDate"> - </asp:Label>
                                                            <%--<label runat="server" style="font-weight: normal;" id="Label7">- </label>--%>
                                                        </div>
                                                    </div>


                                                    <h4>Miscellaneous Fees Breakdown </h4>

                                                    <div class=" col-lg-12">
                                                        <div class="col-lg-6 text-left nomargin">
                                                            Financing Scheme
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin" style="font-weight: normal;">
                                                            <asp:Label runat="server" ID="lblMiscFinancingScheme"> </asp:Label>
                                                            <%--<label runat="server" id="lblDownPayment">3,199,000.00 </label>--%>
                                                        </div>
                                                    </div>

                                                    <div class="col-lg-12">
                                                        <div class="col-lg-6 text-left nomargin">
                                                            Misc Due Date   
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin">
                                                            <asp:Label runat="server" ID="lblMiscDueDate"> - </asp:Label>
                                                            <%--<label runat="server" style="font-weight: normal;" id="lblDueDate3">(175,000.00) </label>--%>
                                                        </div>
                                                    </div>


                                                    <div class="col-lg-12">
                                                        <div class="col-lg-6 text-left nomargin">
                                                            Misc Fees
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin">
                                                            <label runat="server" style="font-weight: normal;">PHP </label>
                                                            <asp:Label runat="server" Style="font-weight: normal;" ID="lblMiscFees">00.00 </asp:Label>
                                                        </div>
                                                    </div>



                                                    <div class="col-lg-12">
                                                        <div class="col-lg-6 text-left nomargin">
                                                            Misc Downpayment
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin">
                                                            <label runat="server" style="font-weight: normal;">PHP </label>
                                                            <asp:Label runat="server" Style="font-weight: normal;" ID="lblMiscDPAmount">00.00 </asp:Label>
                                                        </div>
                                                    </div>



                                                    <div class=" col-lg-12 hidden">
                                                        <div class="col-lg-6 text-left nomargin">
                                                            Less: Promo Discount
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin" style="font-weight: normal;">
                                                            <label runat="server" style="font-weight: normal;">PHP </label>
                                                            <asp:Label runat="server" Style="font-weight: normal;" ID="Label14">0.00 </asp:Label>
                                                            <%--<label runat="server" style="font-weight: normal;" id="lblPromoDisc">00.00 </label>--%>
                                                        </div>
                                                    </div>


                                                    <%--MISCELLANEOUS TERMS--%>
                                                    <div class="col-lg-12">
                                                        <div class="col-lg-6 text-left nomargin">
                                                            Misc DP Terms
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin">
                                                            <asp:Label runat="server" Style="font-weight: normal;" ID="lblMiscDPTerms"> 0.00 </asp:Label>
                                                            <%--<label runat="server" style="font-weight: normal;" id="lblDiscount">(175,000.00) </label>--%>
                                                        </div>
                                                    </div>

                                                    <div class="col-lg-12 nomargin" style="padding-top: 0;">
                                                        <div class="col-lg-6 text-right nomargin"></div>
                                                        <div class="col-lg-6 text-right nomargin">
                                                            <hr style="border-top: 1px solid black; margin-top: 0; margin-bottom: 0;">
                                                        </div>
                                                    </div>


                                                    <div class="col-lg-12">
                                                        <div class="col-lg-6 text-left nomargin">
                                                            <label runat="server">Misc DP Monthly </label>
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin">
                                                            <label runat="server">PHP </label>
                                                            <asp:Label runat="server" Style="font-weight: bold;" ID="lblMiscDPMonthly">00.00 </asp:Label>
                                                            <%--<label runat="server" id="lblNetDAS">3,315,000.00 </label>--%>
                                                        </div>
                                                    </div>


                                                    <div class="col-lg-12 nomargin" style="padding-top: 0;">
                                                        <div class="col-lg-6 text-right nomargin"></div>
                                                        <div class="col-lg-6 text-right nomargin">
                                                            <hr style="border-top: 1px solid black; margin-top: 0; margin-bottom: 0;">
                                                        </div>
                                                    </div>


                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <div class="col-lg-12"></div>
                                                    <hr />

                                                    <div class="col-lg-12">
                                                        <div class="col-lg-6 text-left nomargin">
                                                            Misc Loanable Balance
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin">
                                                            <label runat="server" style="font-weight: normal;">PHP </label>
                                                            <asp:Label runat="server" Style="font-weight: normal;" ID="lblMiscLBAmount">00.00 </asp:Label>
                                                        </div>
                                                    </div>

                                                    <%--MISCELLANEOUS TERMS--%>
                                                    <div class="col-lg-12">
                                                        <div class="col-lg-6 text-left nomargin">
                                                            Misc LB Terms
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin">
                                                            <asp:Label runat="server" Style="font-weight: normal;" ID="lblMiscLBTerms"> 0 </asp:Label>
                                                            <%--<label runat="server" style="font-weight: normal;" id="lblDiscount">(175,000.00) </label>--%>
                                                        </div>
                                                    </div>

                                                    <div class="col-lg-12 nomargin" style="padding-top: 0;">
                                                        <div class="col-lg-6 text-right nomargin"></div>
                                                        <div class="col-lg-6 text-right nomargin">
                                                            <hr style="border-top: 1px solid black; margin-top: 0; margin-bottom: 0;">
                                                        </div>
                                                    </div>


                                                    <div class="col-lg-12">
                                                        <div class="col-lg-6 text-left nomargin">
                                                            <label runat="server">Misc LB Monthly </label>
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin">
                                                            <label runat="server">PHP </label>
                                                            <asp:Label runat="server" Style="font-weight: bold;" ID="lblMiscLBMonthly">00.00 </asp:Label>
                                                            <%--<label runat="server" id="lblNetDAS">3,315,000.00 </label>--%>
                                                        </div>
                                                    </div>


                                                    <div class="col-lg-12 nomargin" style="padding-top: 0;">
                                                        <div class="col-lg-6 text-right nomargin"></div>
                                                        <div class="col-lg-6 text-right nomargin">
                                                            <hr style="border-top: 1px solid black; margin-top: 0; margin-bottom: 0;">
                                                        </div>
                                                    </div>



                                                    <div class=" col-lg-12 hidden">
                                                        <div class="col-lg-6 text-left nomargin" style="font-weight: bold;">
                                                            Misc Charges
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin" style="font-weight: normal;">
                                                            <label runat="server" style="font-weight: bold;">PHP </label>
                                                            <asp:Label runat="server" ID="lblAddMiscCharges2" Style="font-weight: bold;"> 0.00 </asp:Label>
                                                            <%--<label runat="server" id="lblDownPayment">3,199,000.00 </label>--%>
                                                        </div>
                                                    </div>


                                                    <div class="col-lg-12 hidden">
                                                        <div class="col-lg-6 text-left nomargin" style="padding-left: 2em">
                                                            Monthly
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin">
                                                            <label runat="server" style="font-weight: normal;">PHP </label>
                                                            <asp:Label runat="server" ID="lblDPMonthly"> 0.00 </asp:Label>
                                                            <%--<label runat="server" style="font-weight: normal;" id="Label5">(175,000.00) </label>--%>
                                                        </div>
                                                    </div>



                                                    <div class=" col-lg-12 hidden">
                                                        <div class="col-lg-6 text-left nomargin">
                                                            Additional Charges
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin" style="font-weight: normal;">
                                                            <label runat="server" style="font-weight: normal;">PHP </label>
                                                            <asp:Label runat="server" ID="lblAddCharges"> - </asp:Label>
                                                            <%--<label runat="server" id="lblAddCharges">3,199,000.00 </label>--%>
                                                        </div>
                                                    </div>




                                                    <div class=" col-lg-12 hidden">
                                                        <div class="col-lg-6 text-left nomargin">
                                                            Loanable Amount  
                                                        </div>
                                                        <div class="col-lg-6 text-right nomargin" style="font-weight: normal;">
                                                            <%--<label runat="server" id="lblAmountDue">3,199,000.00 </label>--%>
                                                            <asp:Label runat="server" ID="lblAmountDue"> - </asp:Label>
                                                        </div>
                                                    </div>






                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>

                                <%----ENDDD--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="col-lg-8">
        <div class="panel panel-default">
            <div class="panel-body">
                <div class="row">
                    <div class="wizard">

                        <%--HOUSE DETAILS--%>
                        <div class="wizard-inner">
                            <ul class="nav nav-tabs" role="tablist">
                                <li role="presentation" class="active" style="margin-left: 25%;">
                                    <a href="#HouseDetails" data-toggle="tab" aria-controls="HouseDetails" role="tab" title="Contract Details">
                                        <span class="round-tabs one">
                                            <i class="fa fa-map-signs"></i>
                                        </span>
                                    </a>
                                </li>
                                <%--       <li role="presentation" class="disabled">
                                    <a href="#PaymentTerms" data-toggle="tab" aria-controls="PaymentTerms" role="tab" title="Payment Terms">
                                        <span class="round-tabs two">
                                            <i class="fa fa-money"></i>
                                        </span>
                                    </a>
                                </li>--%>
                                <%--                                <li role="presentation" class="disabled hidden">
                                    <a href="#QuickEval" data-toggle="tab" aria-controls="QuickEval" role="tab" title="Quick Evaluation">
                                        <span class="round-tabs two">
                                            <i class="fa fa-calculator"></i>
                                        </span>
                                    </a>
                                </li>--%>
                                <li role="presentation" class="disabled">
                                    <a href="#QuotationSummary" data-toggle="tab" aria-controls="QuotationSummary" role="tab" title="Quotation Summary">
                                        <span class="round-tabs three">
                                            <i class="fa fa-check"></i>
                                        </span>
                                    </a>
                                </li>
                            </ul>

                        </div>


                        <div class="tab-content">
                            <%-- ############################ HouseDetails ########################## --%>
                            <div class="tab-pane active HouseDetails" role="tabpanel" id="HouseDetails">
                                <asp:Panel ID="Panel1" runat="server" DefaultButton="tNextHouseDetails">
                                    <div style="margin-top: -25px; margin-left: 10px; margin-right: 10px;">


                                        <h3 style="font-weight: 300;">TCP</h3>
                                        <hr />

                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>



                                                <div class="row">
                                                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                <h5>Document Date</h5>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">



                                                                <asp:TextBox TextMode="Date" runat="server" CssClass="form-control" ID="tDocDate"></asp:TextBox>
                                                                <%--<input type="date" class="form-control" id="tDocDate" runat="server" />--%>
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator8"
                                                                    ControlToValidate="tDocDate"
                                                                    Text="Please fill up Date."
                                                                    ValidationGroup="Next"
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>
                                                    </div>


                                                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                <h5>DP Due Date</h5>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                <asp:TextBox TextMode="Date" runat="server" CssClass="form-control" ID="txtDPDueDate"></asp:TextBox>

                                                                <%--<input type="date" class="form-control" id="tDocDate" runat="server" />--%>
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator33"
                                                                    ControlToValidate="txtDPDueDate"
                                                                    Text="Please fill up Date."
                                                                    ValidationGroup="Next"
                                                                    runat="server" Style="color: red" />


                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>






                                                <div class="row">

                                                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                <h5>Project Name
                                                                </h5>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                <div class="input-group">
                                                                    <input type="text" style="z-index: auto;" class="form-control tProjName" runat="server" id="tProjName" disabled /><span class="input-group-btn">
                                                                        <button id="bProjName" runat="server" style="width: 50px; z-index: auto;" class="btn btn-secondary" onserverclick="bProjName_ServerClick" type="button"><i class="fa fa-bars"></i></button>
                                                                    </span>
                                                                </div>
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator2"
                                                                    ControlToValidate="tProjName"
                                                                    Text="Please fill up Project."
                                                                    ValidationGroup="Next"
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>
                                                    </div>



                                                </div>




                                                <div class="row">


                                                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                <h5>Block</h5>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-8 col-md-8 col-sm-11 col-xs-12">
                                                                <input type="text" class="form-control tBlock" runat="server" id="tBlock" disabled />
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                <h5>Lot</h5>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                <div class="input-group">
                                                                    <input type="text" style="z-index: auto;" class="form-control tLot" runat="server" id="tLot" readonly />
                                                                    <span class="input-group-btn">
                                                                        <button id="bLotList" runat="server" style="width: 50px; z-index: auto;" onserverclick="bLotList_ServerClick" class="btn btn-secondary" type="button"><i class="fa fa-bars"></i></button>
                                                                    </span>
                                                                </div>
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator4"
                                                                    ControlToValidate="tLot"
                                                                    Text="Please fill up Lot."
                                                                    ValidationGroup="Next"
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>
                                                    </div>




                                                </div>








                                                <div class="row">


                                                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                <h5>House Model</h5>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                <input type="text" style="z-index: auto;" class="form-control" id="tModel" runat="server" readonly />
                                                                <span class="input-group-btn hidden">
                                                                    <button id="bModel" runat="server" style="width: 50px; z-index: auto;" onserverclick="bModel_ServerClick" class="btn btn-secondary" type="button" visible="false" disabled><i class="fa fa-bars"></i></button>
                                                                </span>
                                                                <%--     <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator5"
                                                                    ControlToValidate="tModel"
                                                                    Text="Please fill up Model."
                                                                    ValidationGroup="Next"
                                                                    runat="server" Style="color: red" />--%>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                <h5>Financing Scheme</h5>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                <div class="input-group">
                                                                    <input runat="server" style="z-index: auto;" id="tFinancing" class="form-control" type="text" disabled /><span class="input-group-btn">
                                                                        <button id="bFinancing" runat="server" style="width: 50px; z-index: auto;" data-toggle="modal" data-target="#MsgQuotation" class="btn btn-secondary" onserverclick="bNatureofEmp_ServerClick" type="button"><i class="fa fa-bars"></i></button>
                                                                    </span>
                                                                </div>
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator6"
                                                                    ControlToValidate="tFinancing"
                                                                    Text="Please fill up Financing Scheme."
                                                                    ValidationGroup="Next"
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>
                                                    </div>





                                                </div>









                                                <div class="row">


                                                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                <h5>Lot Area</h5>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-8 col-md-8 col-sm-11 col-xs-12">
                                                                <input type="text" class="form-control" id="tLotArea" runat="server" disabled />
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                <h5>Floor Area</h5>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                <input type="text" class="form-control" id="tFloorArea" runat="server" disabled />
                                                            </div>
                                                        </div>
                                                    </div>



                                                </div>




                                                <div class="row">

                                                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                <h5>Product Status</h5>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                <input type="text" class="form-control" id="tHouseStatus" runat="server" disabled />
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                <h5>Phase</h5>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                <input type="text" class="form-control" id="tPhase" runat="server" disabled />
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>







                                                <div class="row">


                                                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                <h5>Lot Classification</h5>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                <input type="text" class="form-control" id="txtLotClassification" runat="server" disabled />
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                <h5>Product Type</h5>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                <input type="text" class="form-control" id="txtProductType" runat="server" disabled />
                                                            </div>
                                                        </div>
                                                    </div>


                                                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                <h5>Loan Type</h5>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                <div class="input-group">
                                                                    <input type="text" style="z-index: auto;" class="form-control" id="txtLoanType" runat="server" readonly /><span class="input-group-btn">
                                                                        <button id="btnLoanType" runat="server" style="width: 50px; z-index: auto;" onserverclick="btnLoanType_ServerClick" class="btn btn-secondary" type="button"><i class="fa fa-bars"></i></button>
                                                                    </span>
                                                                </div>
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator7"
                                                                    ControlToValidate="txtLoanType"
                                                                    Text="Please fill up Loan Type."
                                                                    ValidationGroup="Next"
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>
                                                    </div>




                                                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12" runat="server" id="divBank">
                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                <h5>Bank</h5>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                <div class="input-group">
                                                                    <input type="text" style="z-index: auto;" class="form-control" id="tBank" runat="server" readonly /><span class="input-group-btn">
                                                                        <button id="btnBank" runat="server" style="width: 50px; z-index: auto;" onserverclick="btnBank_ServerClick" class="btn btn-secondary" type="button"><i class="fa fa-bars"></i></button>
                                                                    </span>
                                                                </div>
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator9"
                                                                    ControlToValidate="tBank"
                                                                    Text="Please fill up Bank."
                                                                    ValidationGroup="Next"
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>
                                                    </div>



                                                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                <h5>Retitling Type</h5>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                <div class="input-group">
                                                                    <input type="text" style="z-index: auto;" class="form-control" id="tRetType" runat="server" readonly /><span class="input-group-btn">
                                                                        <button id="btnRetType" runat="server" style="width: 50px; z-index: auto;" onserverclick="btnRetType_ServerClick" class="btn btn-secondary" type="button"><i class="fa fa-bars"></i></button>
                                                                    </span>
                                                                </div>
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator18"
                                                                    ControlToValidate="tRetType"
                                                                    Text="Please fill up Retitling Type."
                                                                    ValidationGroup="Next"
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>
                                                    </div>



                                                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12 hidden">
                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                <h5>Sold w/ Adjacent Lot</h5>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                <div class="input-group">
                                                                    <input type="text" style="z-index: auto;" class="form-control" id="txtAdjLot" runat="server" readonly value="No" />
                                                                    <span class="input-group-btn">
                                                                        <button id="btnAdjLot" runat="server" style="width: 50px; z-index: auto;" onserverclick="btnAdjLot_ServerClick" class="btn btn-secondary" type="button"><i class="fa fa-bars"></i></button>
                                                                    </span>
                                                                </div>
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator24"
                                                                    ControlToValidate="txtAdjLot"
                                                                    Text="Please fill up Sold w/ Adjacent Lot."
                                                                    ValidationGroup="Next"
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12 hidden">
                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                <h5>Adjacent Lot Quotation No.</h5>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                <div class="input-group">
                                                                    <input type="text" style="z-index: auto;" class="form-control" id="txtLotNo" runat="server" readonly /><span class="input-group-btn">
                                                                        <button id="bLotNo" runat="server" style="width: 50px; z-index: auto;" onserverclick="bLotNo_ServerClick" class="btn btn-secondary" type="button"><i class="fa fa-bars"></i></button>
                                                                    </span>
                                                                </div>
                                                                <%--    <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator25"
                                                                    ControlToValidate="txtLotNo"
                                                                    Text="Please fill up Adjacent Lot Quotation No."
                                                                    ValidationGroup="Next"
                                                                    runat="server" Style="color: red" />--%>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12" runat="server" id="divIncentive">
                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                <h5>Incentive Option</h5>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                <div class="input-group">
                                                                    <input type="text" style="z-index: auto;" class="form-control" id="txtIncentiveOption" runat="server" readonly /><span class="input-group-btn">
                                                                        <button id="btnIncentiveOption" runat="server" style="width: 50px; z-index: auto;" onserverclick="btnIncentiveOption_ServerClick" class="btn btn-secondary" type="button"><i class="fa fa-bars"></i></button>
                                                                    </span>
                                                                </div>
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator21"
                                                                    ControlToValidate="txtIncentiveOption"
                                                                    Text="Please fill up Incentive Option"
                                                                    ValidationGroup="Next"
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>


                                                <br />
                                                <br />
                                                <h3 style="font-weight: 300;">MISC</h3>
                                                <hr />

                                                <div class="row">
                                                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                <h5>Document Date</h5>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                <asp:TextBox TextMode="Date" runat="server" CssClass="form-control" ID="tMiscDocDate"></asp:TextBox>
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator22"
                                                                    ControlToValidate="tMiscDocDate"
                                                                    Text="Please fill up Date."
                                                                    ValidationGroup="Next"
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                <h5>Financing Scheme</h5>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                <div class="input-group">
                                                                    <input runat="server" style="z-index: auto;" id="txtFinancingMisc" class="form-control" type="text" disabled /><span class="input-group-btn">
                                                                        <button id="bFinancingMisc" runat="server" style="width: 50px; z-index: auto;" data-toggle="modal" data-target="#MsgQuotation" class="btn btn-secondary" onserverclick="bNatureofEmp_ServerClick" type="button"><i class="fa fa-bars"></i></button>
                                                                    </span>
                                                                </div>
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator32"
                                                                    ControlToValidate="txtFinancingMisc"
                                                                    Text="Please fill up Financing Scheme."
                                                                    ValidationGroup="Next"
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>












                                                <%--//UNUSED FIELDS--%>
                                                <div class="row hidden">
                                                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                <h5>Size</h5>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                <div class="input-group">
                                                                    <input runat="server" style="z-index: auto;" id="tSize" class="form-control" type="text" disabled /><span class="input-group-btn">
                                                                        <button id="bSize" runat="server" style="width: 50px; z-index: auto;" class="btn btn-secondary" onserverclick="bSize_ServerClick" type="button"><i class="fa fa-bars"></i></button>
                                                                    </span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                <h5>Feature</h5>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                <div class="input-group">
                                                                    <input runat="server" style="z-index: auto;" id="tFeature" class="form-control" type="text" disabled /><span class="input-group-btn">
                                                                        <button id="bFeature" runat="server" style="width: 50px; z-index: auto;" class="btn btn-secondary" onserverclick="bSize_ServerClick" type="button"><i class="fa fa-bars"></i></button>
                                                                    </span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row hidden">
                                                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                <h5>Account Type</h5>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                <div class="input-group">
                                                                    <input runat="server" style="z-index: auto;" id="tAccountType" class="form-control" type="text" disabled /><span class="input-group-btn">
                                                                        <button id="bAccountType" runat="server" style="width: 50px; z-index: auto;" data-toggle="modal" data-target="#MsgQuotation" class="btn btn-secondary" onserverclick="bNatureofEmp_ServerClick" type="button"><i class="fa fa-bars"></i></button>
                                                                    </span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                <h5>Misc Charge Type</h5>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                <div class="input-group">
                                                                    <input runat="server" style="z-index: auto;" id="tSalesType" class="form-control" type="text" value="Inclusive" disabled /><span class="input-group-btn">
                                                                        <button id="bSalesType" runat="server" style="width: 50px; z-index: auto;" data-toggle="modal" data-target="#MsgQuotation" class="btn btn-secondary" onserverclick="bNatureofEmp_ServerClick" type="button"><i class="fa fa-bars"></i></button>
                                                                    </span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>


                                                <br />
                                                <br />
                                                <br />
                                                <br />


                                                <hr />


                                                <%--PAYMENT DETAILS--%>
                                                <div class="table-responsive">
                                                    <table class="table">
                                                        <caption>PAYMENT DETAILS</caption>

                                                        <div class="row"></div>

                                                        <tr>
                                                            <th>TCP Amount:</th>
                                                            <td></td>
                                                            <td>
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">PHP</span>
                                                                    <input id="tODas" style="z-index: auto;" runat="server" type="text" class="form-control text-right" disabled>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th>Discount Amount :</th>
                                                            <td style="width: 20%;">
                                                                <div class="input-group" style="z-index: auto;">
                                                                    <asp:TextBox ID="txtDiscPercent" runat="server" Style="z-index: auto;" MaxLength="6" AutoPostBack="true" CssClass="form-control text-right" Disabled />
                                                                    <span class="input-group-addon">%
                                                                    </span>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">PHP</span>
                                                                    <asp:TextBox ID="tSpotDPDiscAmt" runat="server" Style="z-index: auto;" AutoPostBack="true" CssClass="form-control text-right" OnTextChanged="TextAmount" Disabled />
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th>NET TCP Amount:</th>
                                                            <td></td>
                                                            <td>
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">PHP</span>
                                                                    <input id="tNetDas" style="z-index: auto;" runat="server" type="text" class="form-control text-right" disabled>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <%--joses2--%>
                                                        <tr>
                                                            <th style="font-size: large;">Down Payment :</th>

                                                        </tr>
                                                        <tr>
                                                            <th style="border: none; padding-left: 2em;">Down Payment :</th>
                                                            <td style="width: 20%;">
                                                                <div class="input-group" style="z-index: auto;">
                                                                    <asp:TextBox ID="tDPPercent" runat="server" Style="z-index: auto;" MaxLength="6" AutoPostBack="true" CssClass="form-control text-right" Disabled />
                                                                    <span class="input-group-addon">%
                                                                    </span>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">PHP</span>
                                                                    <asp:TextBox ID="tDPAmount" runat="server" Style="z-index: auto;" MaxLength="6" AutoPostBack="true" CssClass="form-control text-right" OnTextChanged="tDPAmount_TextChanged" Disabled />
                                                                    <%--<input id="tDPAmount" style="z-index: auto;" runat="server" type="text" class="form-control text-right">--%>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th style="border: none; padding-left: 2em;">Reservation Fee :</th>
                                                            <td></td>
                                                            <td>
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">PHP</span>
                                                                    <asp:TextBox ID="tResrvFee" runat="server" Style="z-index: auto;" AutoPostBack="true" CssClass="form-control text-right" />
                                                                    <span class="input-group-btn">
                                                                        <button id="btnLoadResFee" runat="server" style="width: 50px; z-index: auto;" onserverclick="btnLoadResFee_ServerClick" class="btn btn-secondary" type="button"><i class="fa fa-refresh"></i></button>
                                                                    </span>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th style="border: none; padding-left: 2em;">Balance on Equity :</th>
                                                            <td></td>
                                                            <td>
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">PHP</span>
                                                                    <asp:TextBox ID="tPDBalance" runat="server" Style="z-index: auto;" AutoPostBack="true" CssClass="form-control text-right" OnTextChanged="TextAmount" Disabled />
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th style="border: none; padding-left: 2em;">Terms (Months) :</th>
                                                            <td></td>
                                                            <td>
                                                                <asp:TextBox ID="txtDPTerms" runat="server" Style="z-index: auto;" MaxLength="6" AutoPostBack="true" CssClass="form-control text-right" OnTextChanged="tDPAmount_TextChanged" Disabled />
                                                                <%--    <asp:DropDownList runat="server" ID="ddlDPTerms" CssClass="form-control"
                                                                    AutoPostBack="true" DataTextField="Code" DataValueField="Code" Style="text-align: right;" OnSelectedIndexChanged="Terms_SelectedIndexChanged">
                                                                </asp:DropDownList>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th style="border: none; padding-left: 2em;">Monthly DP :</th>
                                                            <td></td>
                                                            <td>
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">PHP</span>
                                                                    <asp:TextBox ID="txtDPMonthly" runat="server" Style="z-index: auto;" AutoPostBack="true" CssClass="form-control text-right" OnTextChanged="TextAmount" Disabled />
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr class="hidden">
                                                            <th>Promo Discount :</th>
                                                            <td></td>
                                                            <td>
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">PHP</span>
                                                                    <asp:TextBox ID="tPromoDisc" runat="server" Style="z-index: auto;" AutoPostBack="true" CssClass="form-control text-right" OnTextChanged="TextAmount" />
                                                                </div>
                                                            </td>
                                                        </tr>


                                                        <%--HIDE MISC TEXTBOXES : TERM AND MONTHLY--%>

                                                        <tr>
                                                            <th style="font-size: large;">Misc Fees :</th>
                                                            <td></td>
                                                            <td style="opacity: 0;">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">PHP</span>
                                                                    <input id="t" style="z-index: auto;" runat="server" type="text" class="form-control text-right" disabled>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th style="border: none; padding-left: 2em;">Misc Fees :</th>
                                                            <td></td>
                                                            <td>
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">PHP</span>
                                                                    <asp:TextBox ID="txtMiscFees" runat="server" Style="z-index: auto;" AutoPostBack="true" CssClass="form-control text-right" />
                                                                    <span class="input-group-btn">
                                                                        <button id="btnLoadMiscFee" runat="server" style="width: 50px; z-index: auto;" onserverclick="btnLoadMiscFee_ServerClick" class="btn btn-secondary" type="button">
                                                                            <i class="fa fa-refresh"></i>
                                                                        </button>
                                                                    </span>
                                                                </div>


                                                            </td>
                                                        </tr>


                                                        <div runat="server" visible="false">
                                                            <tr>
                                                                <th style="border: none; padding-left: 2em;">Terms (Months) :</th>
                                                                <td></td>
                                                                <td>
                                                                    <asp:TextBox ID="txtMiscTerms" runat="server" Style="z-index: auto;" MaxLength="6" AutoPostBack="true" CssClass="form-control text-right" OnTextChanged="tDPAmount_TextChanged" Disabled />
                                                                    <%--    <asp:DropDownList runat="server" ID="ddlDPTerms" CssClass="form-control"
                                                                    AutoPostBack="true" DataTextField="Code" DataValueField="Code" Style="text-align: right;" OnSelectedIndexChanged="Terms_SelectedIndexChanged">
                                                                </asp:DropDownList>--%>`    
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <th style="border: none; padding-left: 2em;">Monthly :</th>
                                                                <td></td>
                                                                <td>
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">PHP</span>
                                                                        <asp:TextBox ID="txtMiscMonthly" runat="server" Style="z-index: auto;" AutoPostBack="true" CssClass="form-control text-right" OnTextChanged="TextAmount" Disabled />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </div>

                                                        <tr>
                                                            <th style="font-size: large;">Loanable Balance :</th>
                                                            <td></td>
                                                            <td style="opacity: 0;">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">PHP</span>
                                                                    <input id="Text1" style="z-index: auto;" runat="server" type="text" class="form-control text-right" disabled>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th style="border: none; padding-left: 2em;">Loanable Balance :</th>
                                                            <td></td>
                                                            <td>
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">PHP</span>
                                                                    <asp:TextBox ID="txtLoanableBalance" runat="server" Style="z-index: auto;" AutoPostBack="true" CssClass="form-control text-right" OnTextChanged="TextAmount" Disabled />
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr class="hidden">
                                                            <th style="border: none; padding-left: 2em;">Discount Base :</th>
                                                            <td style="border: none;"></td>
                                                            <td>
                                                                <asp:DropDownList runat="server" ID="ddlDiscBased" CssClass="form-control"
                                                                    AutoPostBack="true" Style="text-align: right;" OnSelectedIndexChanged="Terms_SelectedIndexChanged">
                                                                    <asp:ListItem Text=" "></asp:ListItem>
                                                                    <asp:ListItem Text="Spot DP"></asp:ListItem>
                                                                    <asp:ListItem Text="Spot Cash"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr style="display: none;">
                                                            <th>Financing Scheme :</th>
                                                            <td></td>
                                                            <td>
                                                                <div class="col-lg-12 nomargin">
                                                                    <input runat="server" style="z-index: auto;" id="Text2" class="form-control" type="text" disabled />
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr id="terms" runat="server">
                                                            <th>LB Terms (Months) :</th>
                                                            <td></td>
                                                            <td>
                                                                <div class="col-lg-12 nomargin">
                                                                    <%--                                                               <asp:DropDownList runat="server" ID="ddlLTerms" CssClass="form-control"
                                                                        AutoPostBack="true" DataTextField="Code" DataValueField="Code" Style="text-align: right;" OnSelectedIndexChanged="Terms_SelectedIndexChanged">
                                                                    </asp:DropDownList>--%>
                                                                    <asp:TextBox ID="txtLTerms" runat="server" Style="z-index: auto;" Enabled="false" AutoPostBack="true" CssClass="form-control text-right" Text="0" OnTextChanged="txtLTerms_TextChanged" />
                                                                    <%--<asp:RequiredFieldValidator
                                                                        ID="RequiredFieldValidator19"
                                                                        ControlToValidate="txtLTerms"
                                                                        InitialValue="0"
                                                                        Text="Please fill up Terms (Years)."
                                                                        ValidationGroup="Next"
                                                                        runat="server" Style="color: red" />
                                                                    <asp:RequiredFieldValidator
                                                                        ID="RequiredFieldValidator20"
                                                                        ControlToValidate="txtLTerms"
                                                                        Text="Please fill up Terms (Years)."
                                                                        ValidationGroup="Next"
                                                                        runat="server" Style="color: red" />--%>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr id="interestRate" runat="server">
                                                            <th>Interest Rate :</th>
                                                            <td></td>
                                                            <td>
                                                                <div class="col-lg-12 nomargin">
                                                                    <asp:TextBox ID="txtFactorRate1" runat="server" Style="z-index: auto;" Enabled="false" AutoPostBack="true" CssClass="form-control text-right" Text="0" OnTextChanged="txtLTerms_TextChanged" />
                                                                    <%--<input runat="server" style="z-index: auto;" id="txtFactorRate"  value="0" class="form-control text-right" type="text" />--%>
                                                                    <%--  <asp:RequiredFieldValidator
                                                                        ID="RequiredFieldValidator21"
                                                                        ControlToValidate="txtFactorRate1"
                                                                        InitialValue="0"
                                                                        Text="Please fill up Interest Rate."
                                                                        ValidationGroup="Next"
                                                                        runat="server" Style="color: red" />--%>
                                                                    <%--  <asp:RequiredFieldValidator
                                                                        ID="RequiredFieldValidator22"
                                                                        ControlToValidate="txtFactorRate1"
                                                                        Text="Please fill up Interest Rate."
                                                                        ValidationGroup="Next"
                                                                        runat="server" Style="color: red" />--%>
                                                                </div>
                                                            </td>
                                                        </tr>


                                                        <%--                                                        <tr id="dueDate" runat="server">
                                                            <th>Due Date :</th>
                                                            <td></td>
                                                            <td>
                                                                <div class="col-lg-12 nomargin">
                                                                   
                                                                    <asp:TextBox runat="server" TextMode="Date" ID="dtpDueDate" CssClass="form-control" AutoPostBack="true"></asp:TextBox>
                                                                </div>

                                                            </td>
                                                        </tr>--%>
                                                        <%--     <asp:DropDownList runat="server" ID="ddlDPDay" Style="text-align: right;" CssClass="form-control"
                                                                        OnSelectedIndexChanged="ddlDPDay_TextChanged" AutoPostBack="true" DataValueField="Code">
                                                                    </asp:DropDownList>--%>
                                                        <%--<input runat="server" style="z-index: auto;" id="txtDueDate" class="form-control" type="text" />--%>


                                                        <%--   <tr>

                                                            <th style="border: none; padding-left: 2em;"></th>
                                                            <td style="border: none;"></td>
                                                            <td>
                                                                <%--<input type="date" class="form-control" id="dtpDPDueDate" runat="server" />--%>
                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <th>Remarks :</th>
                                                            <td></td>
                                                            <td>
                                                                <div class="col-lg-12 nomargin">
                                                                    <input runat="server" style="z-index: auto;" id="tRemarks" class="form-control text-right" type="text" />
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr class="hidden">
                                                            <th>Gross Disposable Income :</th>
                                                            <td></td>
                                                            <td>
                                                                <div class="col-lg-12 nomargin">
                                                                    <input runat="server" style="z-index: auto;" id="txtGDI" class="form-control text-right" type="text" />
                                                                </div>
                                                            </td>
                                                        </tr>

                                                        <tr class="hidden">
                                                            <th style="font-size: large;">Additional Charges :</th>
                                                            <td></td>
                                                            <td style="opacity: 0;">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">PHP</span>
                                                                    <input id="Text6" style="z-index: auto;" runat="server" type="text" class="form-control text-right" disabled>
                                                                </div>

                                                            </td>

                                                        </tr>

                                                        <tr class="hidden">
                                                            <th style="border: none; padding-left: 2em;">Amount :</th>
                                                            <td style="border: none;"></td>
                                                            <td>
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">PHP</span>
                                                                    <asp:TextBox ID="txtAddChargeAmount" runat="server" Style="z-index: auto;" AutoPostBack="true" CssClass="form-control text-right" Enabled="false" OnTextChanged="TextAmount" />
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr class="hidden">
                                                            <th style="border: none; padding-left: 2em;">Allowed :</th>
                                                            <td style="border: none;"></td>
                                                            <td>
                                                                <div class="col-lg-12 nomargin">
                                                                    <asp:DropDownList runat="server" ID="ddlAllowed" CssClass="form-control"
                                                                        AutoPostBack="true" Style="text-align: right;" OnSelectedIndexChanged="Terms_SelectedIndexChanged">
                                                                        <asp:ListItem Text="Not Allowed"></asp:ListItem>
                                                                        <asp:ListItem Text="Allowed"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr class="hidden">
                                                            <th style="border: none; padding-left: 2em;">Terms (Months) :</th>
                                                            <td style="border: none;"></td>
                                                            <td>
                                                                <div class="col-lg-12 nomargin">
                                                                    <%--<input runat="server" style="z-index: auto;" id="tTerms" class="form-control" type="text" Tex/>--%>

                                                                    <asp:TextBox ID="tTerms" runat="server" Style="z-index: auto;" AutoPostBack="true" CssClass="form-control text-right" OnTextChanged="tTerms_TextChanged" />
                                                                </div>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </div>

                                                <br />
                                                <div class="row">
                                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                        <div class="trans-command">
                                                            <div class="btn-group" role="group">
                                                                <asp:UpdatePanel runat="server">
                                                                    <ContentTemplate>
                                                                        <%--FIRST NEXT BUTTON--%>
                                                                        <%--                                                                        <asp:LinkButton runat="server" CssClass="btn btn-success btn-width" ValidationGroup="Next" ID="tNextHouseDetails" OnClientClick="tNextTab();" OnClick="tNextHouseDetails_Click">
                                        Next <i class="glyphicon glyphicon-arrow-right"></i>
                                                                        </asp:LinkButton>--%>
                                                                        <asp:LinkButton runat="server" CssClass="btn btn-success btn-width" ValidationGroup="Next" ID="tNextHouseDetails" OnClick="tNextHouseDetails_Click">
                                        Next <i class="glyphicon glyphicon-arrow-right"></i>
                                                                        </asp:LinkButton>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </asp:Panel>
                            </div>
                            <%-- ############################ END HouseDetails ########################## --%>



















                            <%-- ############################ PaymentTerms ########################## --%>
                            <div class="tab-pane PaymentTerms" role="tabpanel" id="PaymentTerms">
                                <asp:Panel ID="Panel2" runat="server" DefaultButton="tNextHouseSummary">
                                    <div style="margin-top: -25px; margin-left: 10px; margin-right: 10px;">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>

                                                <%--FOR DELETE--%>


                                                <%--      <div class="table-responsive" style="display: none;">
                                                    <table class="table">
                                                        <caption>TOTAL CONTRACT PRICE</caption>
                                                        <tr>
                                                            <th>TOTAL CONTRACT PRICE :</th>
                                                            <td style="width: 48%;">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">PHP</span>
                                                                    <input id="tTcp" style="z-index: auto;" runat="server" type="text" class="form-control text-right" disabled>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th>DISCOUNTS :</th>
                                                            <td>
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">PHP</span>
                                                                    <input id="tTotalDisc" style="z-index: auto;" runat="server" type="text" class="form-control text-right" disabled>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th>NET OF DISCOUNT :</th>
                                                            <td>
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">PHP</span>
                                                                    <input id="tNetDisc" style="z-index: auto;" runat="server" type="text" class="form-control text-right" disabled>
                                                                </div>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </div>--%>

                                                <%--     <div class="table-responsive" style="margin-top: -500; display: none;">
                                                    <table class="table">
                                                        <caption>BREAKDOWN</caption>
                                                        <tr>
                                                            <th>NET DEED OF ABSOLUTE SALES :</th>
                                                            <td>
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">PHP</span>
                                                                    <input id="tDas" style="z-index: auto;" runat="server" type="text" class="form-control text-right" disabled>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th>MISCELLANEOUS :</th>
                                                            <td>
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">PHP</span>
                                                                    <input id="tMisc" style="z-index: auto;" runat="server" type="text" class="form-control text-right" disabled>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th>ADDT'L CHARGES :</th>
                                                            <td>
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">PHP</span>
                                                                    <input id="tAdditionalCharges" style="z-index: auto;" runat="server" type="text" class="form-control text-right" disabled>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th>VAT :</th>
                                                            <td>
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">PHP</span>
                                                                    <input id="tVat" style="z-index: auto;" runat="server" type="text" class="form-control text-right" disabled>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th>DISCOUNT :</th>
                                                            <td>
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">PHP</span>
                                                                    <input id="tDiscount" style="z-index: auto;" runat="server" type="text" class="form-control text-right" disabled>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th>TOTAL CONTRACT PRICE (Net of Discount):</th>
                                                            <td>
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">PHP</span>
                                                                    <input id="tNetTcp" style="z-index: auto; font-weight: bolder;" runat="server" type="text" class="form-control text-right" disabled>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>--%>

                                                <%--                                                <div class="row" style="display: none;">
                                                    <div class="col-lg-12 col-sm-12">
                                                        <div class="btn-pref btn-group btn-group-justified btn-group-lg" role="group" aria-label="...">
                                                            <div class="btn-group" role="group">
                                                                <button type="button" runat="server" id="bDiscount" onserverclick="bTab_ServerClick" class="btn btn-success" href="#tabDiscount" data-toggle="tab">
                                                                    <span class="fa fa-dollar" aria-hidden="true"></span>
                                                                    <div class="hidden-xs">Discount Scheme</div>
                                                                </button>
                                                            </div>
                                                            <div class="btn-group" role="group">
                                                                <button type="button" runat="server" id="bEquity" onserverclick="bTab_ServerClick" class="btn btn-default" href="#tabEquity" data-toggle="tab">
                                                                    <span class="fa fa-calendar" aria-hidden="true"></span>
                                                                    <div class="hidden-xs">Equity</div>
                                                                </button>
                                                            </div>
                                                            <div class="btn-group" role="group">
                                                                <button type="button" runat="server" id="bLoanable" onserverclick="bTab_ServerClick" class="btn btn-default" href="#tabLoanable" data-toggle="tab">
                                                                    <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                                                    <div class="hidden-xs">Loanable</div>
                                                                </button>
                                                            </div>
                                                            <div class="btn-group" role="group">
                                                                <button type="button" runat="server" id="bOtherCharges" onserverclick="bTab_ServerClick" class="btn btn-default" href="#tabOtherCharges" data-toggle="tab">
                                                                    <span class="fa fa-tags" aria-hidden="true"></span>
                                                                    <div class="hidden-xs">Miscellaneous</div>
                                                                </button>
                                                            </div>
                                                        </div>
                                                        <div class="well" style="background-color: white;">
                                                            <div class="tab-content">
                                                                <div class="tab-pane fade in active" runat="server" id="tabDiscount">
                                                                    <div class="table-responsive" style="border: 0px; margin-top: -50px;">
                                                                        <asp:UpdatePanel runat="server">
                                                                            <ContentTemplate>
                                                                                <table class="table">
                                                                                    <tr>
                                                                                        <th>PROMO</th>
                                                                                        <td> 
                                                                                        </td>

                                                                                    </tr>
                                                                                    <tr>
                                                                                        <th>SPOT DP PERCENT</th>
                                                                                        <td>
                                                                                            <div class="input-group" style="z-index: auto;">
                                                                                                <asp:TextBox ID="tSpotDPPercent" runat="server" Style="z-index: auto;" MaxLength="6" AutoPostBack="true" CssClass="form-control text-right" Enabled="false" />
                                                                                                <span class="input-group-addon">%
                                                                                                </span>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td>
                                                                                            <div class="input-group">
                                                                                                <span class="input-group-addon">PHP</span>
                                                                                                <asp:TextBox ID="tSpotDPPercentDiscAmt" runat="server" Style="z-index: auto;" AutoPostBack="true" CssClass="form-control text-right" Enabled="false" />
                                                                                            </div>
                                                                                        </td>

                                                                                    </tr>
                                                                                    <tr>
                                                                                        <th>SPOT DP AMOUNT</th>
                                                                                        <td>
                                                                                            <div class="input-group" style="z-index: auto;">
                                                                                                <span class="input-group-addon">PHP</span>
                                                                                                <asp:TextBox ID="tSpotDPAmt" runat="server" Style="z-index: auto;" AutoPostBack="true" CssClass="form-control text-right" OnTextChanged="TextAmount" />
                                                                                            </div>
                                                                                        </td>
                                                                                        <td>
                                                                                            <div class="input-group">
                                                                                                <span class="input-group-addon">PHP</span>
                                                                                            </div>
                                                                                        </td>

                                                                                    </tr>
                                                                                    <tr>
                                                                                        <th>SPOT CASH</th>
                                                                                        <td>
                                                                                            <div class="input-group" style="z-index: auto;">
                                                                                                <asp:TextBox ID="tSpotCash" runat="server" Style="z-index: auto;" MaxLength="6" AutoPostBack="true" CssClass="form-control text-right" OnTextChanged="TextPercentage" Enabled="false" />
                                                                                                <span class="input-group-addon">%
                                                                                                </span>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td>
                                                                                            <div class="input-group">
                                                                                                <span class="input-group-addon">PHP</span>
                                                                                                <asp:TextBox ID="tSpotCashDiscAmt" runat="server" Style="z-index: auto;" AutoPostBack="true" Enabled="false" CssClass="form-control text-right" OnTextChanged="TextAmount" />
                                                                                            </div>
                                                                                        </td>

                                                                                    </tr>
                                                                                </table>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </div>
                                                                </div>
                                                                <div class="tab-pane fade in" runat="server" id="tabEquity">
                                                                    <div class="table-responsive" style="border: 0px; margin-top: -50px;">
                                                                        <asp:UpdatePanel runat="server">
                                                                            <ContentTemplate>
                                                                                <table class="table">
                                                                                    <tr>
                                                                                        <th>DOWN PAYMENT :</th>
                                                                                        <td style="width: 20%;">
                                                                                            <div class="input-group" style="z-index: auto;">
                                                                                                <span class="input-group-addon">%
                                                                                                </span>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td style="width: 45%;">
                                                                                            <div class="input-group">
                                                                                                <span class="input-group-addon">PHP</span>
                                                                                                <input id="tOTcp" style="z-index: auto; opacity: 0;" runat="server" type="text" class="form-control text-right" disabled>
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr style="display: none;">
                                                                                        <th colspan="2">FIRST DOWN PAYMENT :</th>
                                                                                        <td>
                                                                                            <div class="input-group">
                                                                                                <span class="input-group-addon">PHP</span>
                                                                                                <asp:TextBox ID="tFirstDP" runat="server" Style="z-index: auto;" AutoPostBack="true" CssClass="form-control text-right" OnTextChanged="TextAmount" />
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <th colspan="2">RESERVATION FEE :</th>
                                                                                        <td>
                                                                                            <div class="input-group">
                                                                                                <span class="input-group-addon">PHP</span>
                                                                                                <input id="tOMisc" style="z-index: auto; opacity: 0;" runat="server" type="text" class="form-control text-right" disabled>
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <th colspan="2">NET DOWN PAYMENT :</th>
                                                                                        <td>
                                                                                            <div class="input-group">
                                                                                                <span class="input-group-addon">PHP</span>
                                                                                                <input id="tNetDP" style="z-index: auto;" runat="server" type="text" class="form-control text-right" disabled>
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <th colspan="2">TERMS :</th>
                                                                                        <td> 

                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <th colspan="2">DUE DATE :</th>
                                                                                        <td>
                                                                                            <div class="col-lg-4 col-md-4 col-sm-4 col-xs-4">
                                                                                            </div>
                                                                                            <div class="col-lg-4 col-md-4 col-sm-4 col-xs-4">
                                                                                                <asp:DropDownList ID="ddlDPMonth" runat="server" CssClass="form-control" onchange="PopulateDays()" />
                                                                                            </div>
                                                                                            <div class="col-lg-4 col-md-4 col-sm-4 col-xs-4">
                                                                                                <asp:DropDownList ID="ddlDPYear" runat="server" Style="text-align: right;" CssClass="form-control" onchange="PopulateDays()" />
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <th colspan="2">MONTHLY DOWN PAYMENT :</th>
                                                                                        <td>
                                                                                            <div class="input-group">
                                                                                                <span class="input-group-addon">PHP</span>
                                                                                                <input id="tMonthlyDP" style="z-index: auto;" runat="server" type="text" class="form-control text-right" disabled>
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </div>
                                                                </div>
                                                                <div class="tab-pane fade in" runat="server" id="tabLoanable">
                                                                    <div class="table-responsive" style="border: 0px; margin-top: -50px;">
                                                                        <asp:UpdatePanel runat="server">
                                                                            <ContentTemplate>
                                                                                <table class="table">
                                                                                    <tr>
                                                                                        <th colspan="2">LOANABLE MATURITY AGE :</th>
                                                                                        <td>
                                                                                            <div class="input-group" style="z-index: auto;">
                                                                                                <asp:TextBox ID="tLMaturityAge" runat="server" Style="z-index: auto;" MaxLength="3" AutoPostBack="true" CssClass="form-control text-right" Enabled="false" />
                                                                                                <span class="input-group-addon">y/o
                                                                                                </span>
                                                                                            </div>

                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <th>LOANABLE AMOUNT :</th>
                                                                                        <td style="width: 20%;">
                                                                                            <div class="input-group" style="z-index: auto;">
                                                                                                <asp:TextBox ID="tLPercent" runat="server" Style="z-index: auto;" Enabled="false" MaxLength="6" AutoPostBack="true" CssClass="form-control text-right" OnTextChanged="TextPercentage" />
                                                                                                <span class="input-group-addon">%
                                                                                                </span>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td style="width: 45%;">
                                                                                            <div class="input-group">
                                                                                                <span class="input-group-addon">PHP</span>
                                                                                                <input id="tLAmount" style="z-index: auto;" runat="server" type="text" class="form-control text-right" disabled>
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <th colspan="2">Terms :</th>
                                                                                        <td> 

                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <th colspan="2">INTEREST RATE :</th>
                                                                                        <td>
                                                                                            <div class="input-group" style="z-index: auto;">
                                                                                                <asp:TextBox ID="tInterestRate" runat="server" Style="z-index: auto;" Enabled="false" AutoPostBack="true" CssClass="form-control text-right" OnTextChanged="TextPercentage" />
                                                                                                <span class="input-group-addon">%
                                                                                                </span>
                                                                                            </div>

                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <th colspan="2">MONTHLY AMORTIZATION :</th>
                                                                                        <td>
                                                                                            <div class="input-group">
                                                                                                <span class="input-group-addon">PHP</span>
                                                                                                <input id="tMonthlyAmort" style="z-index: auto;" runat="server" type="text" class="form-control text-right" disabled>
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </div>
                                                                </div>
                                                                <div class="tab-pane disabled" runat="server" id="tabOtherCharges">
                                                                    <div class="table-responsive" style="border: 0px; margin-top: -50px;">
                                                                        <asp:UpdatePanel runat="server">
                                                                            <ContentTemplate>
                                                                                <asp:GridView runat="server"
                                                                                    ID="gvOtherCharges"
                                                                                    CssClass="table table-hover table-responsive"
                                                                                    AutoGenerateColumns="false"
                                                                                    GridLines="None"
                                                                                    AllowPaging="true"
                                                                                    PageSize="5"
                                                                                    OnPageIndexChanging="gvOtherCharges_PageIndexChanging"
                                                                                    ShowHeader="true">
                                                                                    <HeaderStyle BackColor="#66ccff" />
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="ItemName" HeaderText="Description" />
                                                                                        <asp:BoundField DataField="Price" HeaderText="Unit Price" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="25%" DataFormatString="{0:#,##0.00}" />
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
                                                    </div>
                                                </div>



                                                --%>




                                                <%--FOR DELETE--%>

                                                <%-- ############################ END Selling Price ########################## --%>
                                                <br />
                                                <div class="row">
                                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                        <div class="trans-command">
                                                            <div class="btn-group" role="group">
                                                                <asp:UpdatePanel runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:LinkButton runat="server" ID="btnPrevTab" CssClass="btn btn-info btn-width" OnClick="btnPrevTab_Click" OnClientClick="PrevTab('HouseDetails');">
                                                                                                                                                       
                                       <i class="glyphicon glyphicon-arrow-left"></i> Prev
                                                                        </asp:LinkButton>
                                                                        <%--SECOND NEXT BUTTON--%>
                                                                        <asp:LinkButton runat="server" CssClass="btn btn-success btn-width" ID="tNextHouseSummary" OnClick="tNextHouseSummary_Click">
                                        Next <i class="glyphicon glyphicon-arrow-right"></i>
                                                                        </asp:LinkButton>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </asp:Panel>
                            </div>
                            <%-- ############################ END PaymentTerms ########################## --%>

                            <%-- ############################ Quick Eval ########################## --%>
                            <div class="tab-pane QuickEval" role="tabpanel" id="QuickEval">
                                <asp:Panel ID="Panel4" runat="server" DefaultButton="tNextHouseSummary">
                                    <div style="margin-top: -25px; margin-left: 10px; margin-right: 10px;">
                                        <div class="row">
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" id="emp-evaluation">
                                                <div class="row">
                                                    <%--MONTHLY INCOME COL--%>
                                                    <div class="col-lg-7 col-md-7 col-sm-12 col-xs-12">
                                                        <div class="row">
                                                            <asp:UpdatePanel runat="server">
                                                                <ContentTemplate>
                                                                    <div class="col-md-12">
                                                                        <div class="panel panel-info">
                                                                            <div class="panel-heading">
                                                                                <h3 class="panel-title" style="text-align: center; width: 100%;">MONTHLY INCOME</h3>
                                                                            </div>
                                                                            <div class="panel-body">
                                                                                <div class="row">
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                                                    </div>
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                                                        <h5 style="text-align: center; font-weight: bold;">PRINCIPAL</h5>
                                                                                    </div>
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                                                        <h5 style="text-align: center; font-weight: bold;">SPOUSE</h5>
                                                                                    </div>
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                                                        <h5 style="text-align: center; font-weight: bold;">TOTAL</h5>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="row">
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                                                        <h5 style="margin-bottom: 0px; margin-top: 0px;">BASIC SALARY</h5>
                                                                                    </div>
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12 pad-r-4 pad-l-1">
                                                                                        <%--  <input runat="server" class="BasicSalary PTotal form-control" type="number" value="0" id="tPBasicSalary" />--%>

                                                                                        <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control text-right" Text="0.00" ID="tPBasicSalary" OnTextChanged="Text_TextChanged" />
                                                                                    </div>
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12 pad-l-1 pad-r-1">
                                                                                        <%--<input runat="server" class="BasicSalary STotal form-control" type="number" value="0" id="tSBasicSalary" />--%>

                                                                                        <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control text-right" Text="0.00" ID="tSBasicSalary" OnTextChanged="Text_TextChanged" />
                                                                                    </div>
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12 pad-l-2">
                                                                                        <%--<input runat="server" type="number" class="tTBasicSalary form-control" value="0" id="tTBasicSalary" disabled />--%>
                                                                                        <input runat="server" class="form-control text-right" id="tTBasicSalary" disabled />
                                                                                    </div>
                                                                                </div>
                                                                                <div class="row">
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                                                        <h5 style="margin-bottom: 0px;">ALLOWANCES</h5>
                                                                                    </div>
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12 pad-r-4 pad-l-1">
                                                                                        <%--<input runat="server" type="number" class="Allowances PTotal form-control" id="tPAllowances" value="0" />--%>

                                                                                        <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control text-right" Text="0.00" ID="tPAllowances" OnTextChanged="Text_TextChanged" />
                                                                                    </div>
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12 pad-l-1 pad-r-1">
                                                                                        <%--<input runat="server" type="number" class="Allowances STotal form-control" id="tSAllowances" value="0" />--%>

                                                                                        <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control text-right" Text="0.00" ID="tSAllowances" OnTextChanged="Text_TextChanged" />
                                                                                    </div>
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12 pad-l-2">
                                                                                        <%--<input runat="server" type="number" class="tTAllowances form-control" id="tTAllowances" value="0" disabled />--%>
                                                                                        <input runat="server" class="form-control text-right" id="tTAllowances" disabled />
                                                                                    </div>
                                                                                </div>
                                                                                <div class="row">
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                                                        <h5 style="margin-bottom: 0px;">COMISSIONS</h5>
                                                                                    </div>
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12 pad-r-4 pad-l-1">
                                                                                        <%--<input runat="server" type="number" class="Commissions PTotal form-control" id="tPCommissions" value="0" />--%>

                                                                                        <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control text-right" Text="0.00" ID="tPCommissions" OnTextChanged="Text_TextChanged" />
                                                                                    </div>
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12 pad-l-1 pad-r-1">
                                                                                        <%--<input runat="server" type="number" class="Commissions STotal form-control" id="tSCommissions" value="0" />--%>

                                                                                        <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control text-right" Text="0.00" ID="tSCommissions" OnTextChanged="Text_TextChanged" />
                                                                                    </div>
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12 pad-l-2">
                                                                                        <%--<input runat="server" type="number" class="tTCommissions form-control" id="tTCommissions" value="0" disabled />--%>
                                                                                        <input runat="server" class="form-control text-right" id="tTCommissions" disabled />
                                                                                    </div>
                                                                                </div>
                                                                                <div class="row">
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                                                        <h5 style="margin-bottom: 0px; margin-top: 0px;">RENTAL INCOME</h5>
                                                                                    </div>
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12 pad-r-4 pad-l-1">
                                                                                        <%--<input runat="server" type="number" class="RentalIncome PTotal form-control" id="tPRentalIncome" value="0" />--%>

                                                                                        <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control text-right" Text="0.00" ID="tPRentalIncome" OnTextChanged="Text_TextChanged" />
                                                                                    </div>
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12 pad-l-1 pad-r-1">
                                                                                        <%--<input runat="server" type="number" class="RentalIncome STotal form-control" id="tSRentalIncome" value="0" />--%>

                                                                                        <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control text-right" Text="0.00" ID="tSRentalIncome" OnTextChanged="Text_TextChanged" />
                                                                                    </div>
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12 pad-l-2">
                                                                                        <%--<input runat="server" type="number" class="tTRentalIncome form-control" id="tTRentalIncome" value="0" disabled />--%>
                                                                                        <input runat="server" class="form-control text-right" id="tTRentalIncome" disabled />
                                                                                    </div>
                                                                                </div>
                                                                                <div class="row">
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                                                        <h5 style="margin-bottom: 0px; margin-top: 0px;">RETAINER'S FEE</h5>
                                                                                    </div>
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12 pad-r-4 pad-l-1">
                                                                                        <%--<input runat="server" type="number" class="Retainer PTotal form-control" id="tPRetainer" value="0" />--%>

                                                                                        <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control text-right" Text="0.00" ID="tPRetainer" OnTextChanged="Text_TextChanged" />
                                                                                    </div>
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12 pad-l-1 pad-r-1">
                                                                                        <%--<input runat="server" type="number" class="Retainer STotal form-control" id="tSRetainer" value="0" />--%>

                                                                                        <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control text-right" Text="0.00" ID="tSRetainer" OnTextChanged="Text_TextChanged" />
                                                                                    </div>
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12 pad-l-2">
                                                                                        <%--<input runat="server" type="number" class="tTRetainer form-control" id="tTRetainer" value="0" disabled />--%>
                                                                                        <input runat="server" class="form-control text-right" id="tTRetainer" disabled />
                                                                                    </div>
                                                                                </div>
                                                                                <div class="row">
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                                                        <h5 style="margin-bottom: 0px;">OTHERS</h5>
                                                                                    </div>
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12 pad-r-4 pad-l-1">
                                                                                        <%--<input runat="server" type="number" class="MIOthers PTotal form-control" id="tPOthers" value="0" />--%>

                                                                                        <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control text-right" Text="0.00" ID="tPOthers" OnTextChanged="Text_TextChanged" />
                                                                                    </div>
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12 pad-l-1 pad-r-1">
                                                                                        <%--<input runat="server" type="number" class="MIOthers STotal form-control" id="tSOthers" value="0" />--%>

                                                                                        <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control text-right" Text="0.00" ID="tSOthers" OnTextChanged="Text_TextChanged" />
                                                                                    </div>
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12 pad-l-2">
                                                                                        <%--<input runat="server" type="number" class="tTOthers form-control" id="tTOthers" value="0" disabled />--%>
                                                                                        <input runat="server" class="form-control text-right" id="tTOthers" disabled />
                                                                                    </div>
                                                                                </div>
                                                                                <div class="row">
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                                                    </div>
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12 pad-r-4 pad-l-1">
                                                                                        <%--<input runat="server" type="number" name="MITotal" class="tPTotal MITotal form-control" id="tPTotal" value="0" disabled />--%>
                                                                                        <input runat="server" class="form-control text-right" id="tPTotal" disabled />
                                                                                    </div>
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12 pad-l-1 pad-r-1">
                                                                                        <%--<input runat="server" type="number" name="MITotal" class="tSTotal MITotal form-control" id="tSTotal" value="0" disabled />--%>
                                                                                        <input runat="server" class="form-control text-right" id="tSTotal" disabled />
                                                                                    </div>
                                                                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12 pad-l-2">
                                                                                        <%--  <input runat="server" type="number" class="tMITotal form-control" id="tMITotal" value="0" disabled />--%>
                                                                                        <input runat="server" class="form-control text-right" id="tMITotal" disabled />
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-12">
                                                                        <div class="panel panel-info">
                                                                            <div class="panel-body strong" style="padding-top: 10px; padding-bottom: 10px;">
                                                                                <div class="row" style="margin-top: 0px;">
                                                                                    <div class="col-lg-6 col-md-6 col-sm-6 col-md-12">
                                                                                        <h5>Monthly Net Income</h5>
                                                                                    </div>
                                                                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12 pad-l-1">
                                                                                        <input runat="server" class="form-control text-right" id="tMNetIncome" value="0.00" readonly />
                                                                                    </div>
                                                                                </div>
                                                                                <div class="row">
                                                                                    <div class="col-lg-6 col-md-6 col-sm-6 col-md-12">
                                                                                        <h5>Gross Disposable Income</h5>
                                                                                    </div>
                                                                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12 pad-l-1">
                                                                                        <%--<input type="number" class="form-control" id="tRequiredMonthly" value="0" disabled />--%>

                                                                                        <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control text-right" Text="0.00" ID="tRequiredMonthly" OnTextChanged="Text_TextChanged" Enabled="false" />
                                                                                    </div>
                                                                                </div>
                                                                                <div class="row">
                                                                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                                                        <h5>Monthly Amortization</h5>
                                                                                    </div>
                                                                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12 pad-l-1">
                                                                                        <%--<input runat="server" type="number" class="form-control" id="tMonthlyAmort" value="0" disabled />--%>
                                                                                        <%--<input runat="server" class="form-control" id="Text1" disabled />--%>
                                                                                        <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control text-right" Text="0.00" ID="tMonthlyAmort2" OnTextChanged="Text_TextChanged" Enabled="false" />
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                    <%--MONTHLY EXPENSES COL--%>
                                                    <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                                        <div class="row">
                                                            <asp:UpdatePanel runat="server">
                                                                <ContentTemplate>
                                                                    <div class="col-md-12">
                                                                        <div class="panel panel-info">
                                                                            <div class="panel-heading">
                                                                                <h3 class="panel-title" style="text-align: center; width: 100%;">MONTHLY EXPENSES</h3>
                                                                            </div>
                                                                            <div class="panel-body">
                                                                                <div class="row">
                                                                                    <div class="col-lg-7 col-md-7 col-sm-7 col-xs-12">
                                                                                        <h5 style="font-weight: bold;">LIVING UTILITIES</h5>
                                                                                    </div>
                                                                                    <div class="col-lg-5 col-md-5 col-sm-5 col-xs-12">
                                                                                    </div>
                                                                                </div>
                                                                                <div class="row">
                                                                                    <div class="col-lg-7 col-md-7 col-sm-7 col-xs-12">
                                                                                        <h5 style="margin-bottom: 0px;">FOOD</h5>
                                                                                    </div>
                                                                                    <div class="col-lg-5 col-md-5 col-sm-5 col-xs-12">
                                                                                        <%--<input runat="server" type="number" class="METotal form-control" id="tFood" value="0" />--%>

                                                                                        <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control text-right" Text="0.00" ID="tFood" OnTextChanged="Text_TextChanged" />
                                                                                    </div>
                                                                                </div>
                                                                                <div class="row">
                                                                                    <div class="col-lg-7 col-md-7 col-sm-7 col-xs-12">
                                                                                        <h5 style="margin-bottom: 0px;">LIGHT & WATER</h5>
                                                                                    </div>
                                                                                    <div class="col-lg-5 col-md-5 col-sm-5 col-xs-12">
                                                                                        <%--<input runat="server" type="number" class="METotal form-control" id="tLightWater" value="0" />--%>

                                                                                        <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control text-right" Text="0.00" ID="tLightWater" OnTextChanged="Text_TextChanged" />
                                                                                    </div>
                                                                                </div>
                                                                                <div class="row">
                                                                                    <div class="col-lg-7 col-md-7 col-sm-7 col-xs-12">
                                                                                        <h5 style="margin-bottom: 0px;">TELEPHONE BILL</h5>
                                                                                    </div>
                                                                                    <div class="col-lg-5 col-md-5 col-sm-5 col-xs-12">
                                                                                        <%--<input runat="server" type="number" class="METotal form-control" id="tTelephoneBill" value="0" />--%>

                                                                                        <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control text-right" Text="0.00" ID="tTelephoneBill" OnTextChanged="Text_TextChanged" />
                                                                                    </div>
                                                                                </div>
                                                                                <div class="row">
                                                                                    <div class="col-lg-7 col-md-7 col-sm-7 col-xs-12">
                                                                                        <h5 style="margin-bottom: 0px;">TRANSPORTATION</h5>
                                                                                    </div>
                                                                                    <div class="col-lg-5 col-md-5 col-sm-5 col-xs-12">
                                                                                        <%--<input runat="server" type="number" class="METotal form-control" id="tTransportation" value="0" />--%>

                                                                                        <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control text-right" Text="0.00" ID="tTransportation" OnTextChanged="Text_TextChanged" />
                                                                                    </div>
                                                                                </div>
                                                                                <div class="row">
                                                                                    <div class="col-lg-7 col-md-7 col-sm-7 col-xs-12">
                                                                                        <h5 style="margin-bottom: 0px;">RENT</h5>
                                                                                    </div>
                                                                                    <div class="col-lg-5 col-md-5 col-sm-5 col-xs-12">
                                                                                        <%--<input runat="server" type="number" class="METotal form-control" id="tRent" value="0" />--%>

                                                                                        <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control text-right" Text="0.00" ID="tRent" OnTextChanged="Text_TextChanged" />
                                                                                    </div>
                                                                                </div>
                                                                                <div class="row">
                                                                                    <div class="col-lg-7 col-md-7 col-sm-7 col-xs-12">
                                                                                        <h5 style="margin-bottom: 0px;">EDUCATION</h5>
                                                                                    </div>
                                                                                    <div class="col-lg-5 col-md-5 col-sm-5 col-xs-12">
                                                                                        <%--<input runat="server" type="number" class="METotal form-control" id="tEducation" value="0" />--%>

                                                                                        <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control text-right" Text="0.00" ID="tEducation" OnTextChanged="Text_TextChanged" />
                                                                                    </div>
                                                                                </div>
                                                                                <div class="row">
                                                                                    <div class="col-lg-7 col-md-7 col-sm-7 col-xs-12">
                                                                                        <h5 style="margin-bottom: 0px;">LOAN AMORT</h5>
                                                                                    </div>
                                                                                    <div class="col-lg-5 col-md-5 col-sm-5 col-xs-12">
                                                                                        <%-- <input runat="server" type="number" class="METotal form-control" id="tLoanAmort" value="0" />--%>

                                                                                        <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control text-right" Text="0.00" ID="tLoanAmort" OnTextChanged="Text_TextChanged" />
                                                                                    </div>
                                                                                </div>
                                                                                <div class="row">
                                                                                    <div class="col-lg-7 col-md-7 col-sm-7 col-xs-12">
                                                                                        <h5 style="margin-bottom: 0px;">OTHERS</h5>
                                                                                    </div>
                                                                                    <div class="col-lg-5 col-md-5 col-sm-5 col-xs-12">
                                                                                        <%--<input runat="server" type="number" class="METotal form-control" id="tMEOthers" value="0" />--%>

                                                                                        <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control text-right" Text="0.00" ID="tMEOthers" OnTextChanged="Text_TextChanged" />
                                                                                    </div>
                                                                                </div>
                                                                                <div class="row">
                                                                                    <div class="col-lg-7 col-md-7 col-sm-7 col-xs-12">
                                                                                    </div>
                                                                                    <div class="col-lg-5 col-md-5 col-sm-5 col-xs-12">
                                                                                        <%--<input runat="server" type="number" class="tMETotal form-control" id="tMETotal" value="0" disabled />--%>

                                                                                        <input runat="server" class="form-control text-right" id="tMETotal" disabled />
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-12">
                                                                        <div class="panel panel-info">
                                                                            <div class="panel-body">
                                                                                <div class="col-lg-7 col-md-7 col-sm-7 col-xs-12">
                                                                                    <label>Remarks</label>
                                                                                </div>
                                                                                <div class="col-lg-5 col-md-5 col-sm-5 col-xs-12">
                                                                                    <h4 runat="server" id="lblRemarks" style="color: green; margin-top: 0px; margin-bottom: 0px;">PASSED</h4>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>

                                        </div>
                                        <div class="row">
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <div class="trans-command">
                                                    <div class="btn-group" role="group">
                                                        <asp:UpdatePanel runat="server">
                                                            <ContentTemplate>
                                                                <asp:LinkButton runat="server" CssClass="btn btn-info btn-width" ID="btnPrevQuick" OnClientClick="PrevTab('PaymentTerms');">
                                       <i class="glyphicon glyphicon-arrow-left"></i> Prev
                                                                </asp:LinkButton>
                                                                <%--THIRD NEXT BUTTON--%>
                                                                <asp:LinkButton runat="server" CssClass="btn btn-success btn-width" ID="btnNextQuick" OnClick="btnNextQuick_Click">
                                        Next <i class="glyphicon glyphicon-arrow-right"></i>
                                                                </asp:LinkButton>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                            <%-- ############################ END Quick Eval ########################## --%>

                            <%-- ############################ QuotationSummary ########################## --%>
                            <div class="tab-pane" role="tabpanel" id="QuotationSummary">
                                <asp:Panel ID="Panel3" runat="server" DefaultButton="bFinish">
                                    <div style="margin-top: -25px; margin-left: 10px; margin-right: 10px;">
                                        <div class="row">
                                            <div class="col-lg-12">
                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                        <div class="row">
                                                            <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                                <div class="row">
                                                                    <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                        <h5>Reservation Date</h5>
                                                                    </div>
                                                                    <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                        <h5 style="text-align: right;">:</h5>
                                                                    </div>
                                                                    <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                        <input type="date" class="form-control" id="tDocDate2" runat="server" disabled />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                                <div class="row">
                                                                    <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                        <h5>Project Name</h5>
                                                                    </div>
                                                                    <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                        <h5 style="text-align: right;">:</h5>
                                                                    </div>
                                                                    <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                        <input type="text" style="z-index: auto;" class="form-control tProjName2" runat="server" id="tProjName2" disabled />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                                <div class="row">
                                                                    <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                        <h5>Block</h5>
                                                                    </div>
                                                                    <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                        <h5 style="text-align: right;">:</h5>
                                                                    </div>
                                                                    <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                        <input type="text" style="z-index: auto;" class="form-control tBlock2" runat="server" id="tBlock2" readonly />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                                <div class="row">
                                                                    <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                        <h5>Lot</h5>
                                                                    </div>
                                                                    <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                        <h5 style="text-align: right;">:</h5>
                                                                    </div>
                                                                    <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                        <input type="text" style="z-index: auto;" class="form-control tLot2" runat="server" id="tLot2" readonly />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                                <div class="row">
                                                                    <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                        <h5>House Model</h5>
                                                                    </div>
                                                                    <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                        <h5 style="text-align: right;">:</h5>
                                                                    </div>
                                                                    <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                        <input type="text" style="z-index: auto;" class="form-control tModel2" id="tModel2" runat="server" readonly />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                                <div class="row">
                                                                    <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                        <h5>Financing Scheme</h5>
                                                                    </div>
                                                                    <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                        <h5 style="text-align: right;">:</h5>
                                                                    </div>
                                                                    <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                        <input runat="server" style="z-index: auto;" id="tFinancing2" class="form-control" type="text" disabled />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                                <div class="row">
                                                                    <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                        <h5>Lot Area</h5>
                                                                    </div>
                                                                    <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                        <h5 style="text-align: right;">:</h5>
                                                                    </div>
                                                                    <div class="col-lg-8 col-md-8 col-sm-11 col-xs-12">
                                                                        <input type="text" class="form-control" id="txtLotArea2" runat="server" disabled />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                                <div class="row">
                                                                    <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                        <h5>Floor Area</h5>
                                                                    </div>
                                                                    <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                        <h5 style="text-align: right;">:</h5>
                                                                    </div>
                                                                    <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                        <input type="text" class="form-control" id="txtFloorArea2" runat="server" disabled />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                                <div class="row">
                                                                    <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                        <h5>Product Status</h5>
                                                                    </div>
                                                                    <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                        <h5 style="text-align: right;">:</h5>
                                                                    </div>
                                                                    <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                        <input type="text" class="form-control" id="tHouseStatus2" runat="server" disabled />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                                <div class="row">
                                                                    <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                        <h5>Phase</h5>
                                                                    </div>
                                                                    <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                        <h5 style="text-align: right;">:</h5>
                                                                    </div>
                                                                    <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                        <input type="text" class="form-control" id="txtPhase2" runat="server" disabled />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="row">
                                                            <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                                <div class="row">
                                                                    <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                        <h5>Lot Classification</h5>
                                                                    </div>
                                                                    <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                        <h5 style="text-align: right;">:</h5>
                                                                    </div>
                                                                    <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                        <input runat="server" style="z-index: auto;" id="txtLotClass2" class="form-control" type="text" disabled />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                                <div class="row">
                                                                    <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                        <h5>Product Type</h5>
                                                                    </div>
                                                                    <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                        <h5 style="text-align: right;">:</h5>
                                                                    </div>
                                                                    <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                        <input runat="server" style="z-index: auto;" id="txtProductType2" class="form-control" type="text" disabled />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="row">
                                                            <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                                <div class="row">
                                                                    <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                        <h5>Loan Type</h5>
                                                                    </div>
                                                                    <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                        <h5 style="text-align: right;">:</h5>
                                                                    </div>
                                                                    <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                        <input type="text" style="z-index: auto;" class="form-control" id="txtLoanType2" runat="server" disabled />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12" runat="server" id="divBank2">
                                                                <div class="row">
                                                                    <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                        <h5>Bank</h5>
                                                                    </div>
                                                                    <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                        <h5 style="text-align: right;">:</h5>
                                                                    </div>
                                                                    <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                        <input type="text" style="z-index: auto;" class="form-control" id="tBank2" runat="server" readonly /><span class="input-group-btn">
                                                                        </span>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12" runat="server" id="divIncentive2">
                                                                <div class="row">
                                                                    <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                        <h5>Incentive Option</h5>
                                                                    </div>
                                                                    <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                        <h5 style="text-align: right;">:</h5>
                                                                    </div>
                                                                    <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                        <input type="text" style="z-index: auto;" class="form-control" id="txtIncentiveOption2" runat="server" readonly /><span class="input-group-btn">
                                                                        </span>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>





                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <hr />


                                        <%--         <div class="row">
                                            <asp:UpdatePanel runat="server">
                                                <ContentTemplate>
                                                    <div class="col-lg-12">
                                                        <div class="row">
                                                            <div class="col-lg-6 col-md-6 col-sm-11 col-xs-11">
                                                                <h5>TOTAL CONTRACT PRICE</h5>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">PHP</span>
                                                                    <input id="tTcp2" style="z-index: auto;" runat="server" type="text" class="form-control text-right" disabled>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-lg-6 col-md-6 col-sm-11 col-xs-11">
                                                                <h5>DISCOUNT</h5>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">PHP</span>
                                                                    <asp:TextBox runat="server" ID="tTotalDisc2" CssClass="form-control text-right" Style="z-index: auto;" Enabled="false" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-lg-6 col-md-6 col-sm-11 col-xs-11">
                                                                <h5>NET OF DISCOUNT</h5>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">PHP</span>
                                                                    <asp:TextBox runat="server" ID="tNetDisc2" CssClass="form-control text-right" Style="z-index: auto;" Enabled="false" />
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-3 col-sm-12 col-xs-12">
                                                                <h5>DOWN PAYMENT</h5>
                                                            </div>
                                                            <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                <div class="input-group" style="z-index: auto;">
                                                                    <asp:TextBox runat="server" ID="tDPPercent2" Style="z-index: auto;" CssClass="form-control text-right" Enabled="false" />
                                                                    <span class="input-group-addon">%
                                                                    </span>
                                                                </div>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">PHP</span>
                                                                    <input id="tDPAmount2" style="z-index: auto;" runat="server" type="text" class="form-control text-right" disabled>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row" style="display: none;">
                                                            <div class="col-lg-6 col-md-6 col-sm-11 col-xs-11">
                                                                <h5>FIRST DOWN PAYMENT</h5>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">PHP</span>
                                                                    <asp:TextBox runat="server" ID="tFirstDP2" CssClass="form-control text-right" Style="z-index: auto;" Enabled="false" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-lg-6 col-md-6 col-sm-11 col-xs-11">
                                                                <h5>RESERVATION FEE</h5>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">PHP</span>
                                                                    <input id="tResrvFee2" style="z-index: auto;" runat="server" type="text" class="form-control text-right" disabled>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-lg-6 col-md-6 col-sm-11 col-xs-11">
                                                                <h5>NET DOWN PAYMENT</h5>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">PHP</span>
                                                                    <input id="tNetDP2" style="z-index: auto;" runat="server" type="text" class="form-control text-right" disabled>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row" style="display: none;">
                                                            <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                <div class="input-group" style="z-index: auto;">
                                                                    <asp:TextBox ID="tLMaturityAge" runat="server" Style="z-index: auto;" MaxLength="3" AutoPostBack="true" CssClass="form-control text-right" Enabled="false" />
                                                                    <asp:TextBox ID="tInterestRate" runat="server" Style="z-index: auto;" Enabled="false" AutoPostBack="true" CssClass="form-control text-right" OnTextChanged="TextPercentage" />
                                                                    <span class="input-group-addon">%
                                                                    </span>
                                                                </div>
                                                            </div>
                                                            <div class="col-lg-3 col-md-3 col-sm-12 col-xs-12">
                                                                <h5>LOANABLE AMOUNT</h5>
                                                            </div>
                                                            <div class="col-lg-3 col-md-3 col-sm-11 col-xs-11">
                                                                <div class="input-group" style="z-index: auto;">
                                                                    <input id="tLPercent2" style="z-index: auto;" runat="server" type="text" class="form-control text-right" disabled>
                                                                    <span class="input-group-addon">%
                                                                    </span>
                                                                </div>
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                                                <h5 style="text-align: right;">:</h5>
                                                            </div>
                                                            <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">PHP</span>
                                                                    <input id="tLAmount2" style="z-index: auto;" runat="server" type="text" class="form-control text-right" disabled>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>--%>


                                        <hr />
                                        <div class="row">
                                            <div class="col-lg-12">
                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                        <%--PANEL NA GAWA NI JOSES--%>
                                                        <asp:Panel ID="divAddCharges" runat="server">
                                                            <div class="row">
                                                                <h4 style="text-align: center;" runat="server" id="hAddCharges">ADDITIONAL CHARGES</h4>
                                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                    <asp:UpdatePanel runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:GridView runat="server" ID="gvAddCharges"
                                                                                CssClass="table table-hover table-responsive"
                                                                                AutoGenerateColumns="false"
                                                                                AllowPaging="true"
                                                                                PageSize="12"
                                                                                OnPageIndexChanging="gvAddCharges_PageIndexChanging"
                                                                                GridLines="None"
                                                                                ShowHeader="true">
                                                                                <HeaderStyle BackColor="#66ccff" />
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="Terms" HeaderText="Terms" />
                                                                                    <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                                                                    <asp:BoundField DataField="PaymentAmount" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Payment Amount" />
                                                                                    <asp:BoundField DataField="Balance" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Balance" />
                                                                                </Columns>
                                                                                <PagerStyle CssClass="pagination-ys" />
                                                                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                                                            </asp:GridView>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                            </div>
                                                        </asp:Panel>

                                                        <asp:Panel ID="divMonthlyDP" runat="server" BackColor="">
                                                            <div class="row">
                                                                <h4 style="text-align: center;">MONTHLY DOWNPAYMENT SCHEDULE</h4>
                                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                    <asp:UpdatePanel runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:GridView runat="server" ID="gvDownPayment"
                                                                                CssClass="table table-hover table-responsive"
                                                                                AutoGenerateColumns="false"
                                                                                AllowPaging="true"
                                                                                PageSize="12"
                                                                                OnPageIndexChanging="gvDownPayment_PageIndexChanging"
                                                                                GridLines="None"
                                                                                ShowHeader="true">
                                                                                <HeaderStyle BackColor="#66ccff" />
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="Terms" HeaderText="Line Item No." />
                                                                                    <asp:BoundField DataField="PaymentType" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" HeaderText="Payment Type" />
                                                                                    <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd-MMM-yyyy}" />

                                                                                    <asp:BoundField DataField="PaymentAmount" ItemStyle-CssClass="text-right" DataFormatString="PHP {0:###,###,##0.##}" HeaderStyle-CssClass="text-center" HeaderText="Payment Amount (PHP)" />
                                                                                    <asp:BoundField DataField="Principal" ItemStyle-CssClass="text-right" DataFormatString="PHP {0:###,###,##0.##}" HeaderStyle-CssClass="text-center" HeaderText="Principal" />

                                                                                    <asp:BoundField DataField="InterestAmount" Visible="false" HeaderText="Interest" DataFormatString="PHP {0:###,###,##0.##}" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" />
                                                                                    <asp:BoundField DataField="Balance" DataFormatString="PHP {0:###,###,##0.00}" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-right" HeaderText="Balance (PHP)" />


                                                                                    <%--   <asp:BoundField DataField="Penalty" ControlStyle-CssClass="hidden" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Penalty" />
                                                                                    <asp:BoundField DataField="Misc" ControlStyle-CssClass="hidden" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Misc" />
                                                                                    <asp:BoundField DataField="UnAll" DataFormatString="PHP {0:###,###,##0.##}"
                                                                                        HeaderText="UnAllocated" Visible="false" />--%>
                                                                                </Columns>
                                                                                <PagerStyle CssClass="pagination-ys" />
                                                                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                                                            </asp:GridView>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                            </div>
                                                        </asp:Panel>

                                                        <asp:Panel ID="divMonthlyAmort" runat="server">
                                                            <div class="row">
                                                                <h4 style="text-align: center;">MONTHLY AMORTIZATION SCHEDULE (LOANABLE BALANCE)</h4>
                                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                    <asp:UpdatePanel runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:GridView runat="server" ID="gvAmortization"
                                                                                CssClass="table table-hover table-responsive"
                                                                                AutoGenerateColumns="false"
                                                                                AllowPaging="true"
                                                                                PageSize="12"
                                                                                OnPageIndexChanging="gvAmortization_PageIndexChanging"
                                                                                GridLines="None"
                                                                                ShowHeader="true">
                                                                                <HeaderStyle BackColor="#66ccff" />
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="Terms" HeaderText="Line Item No." />
                                                                                    <asp:BoundField DataField="PaymentType" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" HeaderText="Payment Type" />
                                                                                    <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                                                                    <asp:BoundField DataField="PaymentAmount" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" DataFormatString="PHP {0:###,###,##0.00}" HeaderText="Payment Amount (PHP)" />
                                                                                    <asp:BoundField DataField="Principal" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-right" DataFormatString="PHP {0:###,###,##0.00}" HeaderText="Principal (PHP)" />
                                                                                    <asp:BoundField DataField="InterestAmount" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-right" DataFormatString="PHP {0:###,###,##0.00}" HeaderText="Interest (PHP)" />
                                                                                    <asp:BoundField DataField="Balance" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-right" DataFormatString="PHP {0:###,###,##0.00}" HeaderText="Balance (PHP)" />

                                                                                    <%--<asp:BoundField DataField="Penalty" Visible="false" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" DataFormatString="{0:###,###,##0.00}" HeaderText="Penalty (PHP)" />
                                                                                    <asp:BoundField DataField="Misc" Visible="false" HeaderStyle-CssClass="text-center hidden" ItemStyle-CssClass="text-center hidden" DataFormatString="{0:###,###,##0.00}" HeaderText="Misc (PHP)" />
                                                                                    <asp:BoundField DataField="UnAll" ControlStyle-CssClass="hidden" DataFormatString="{0:###,###,##0.##}" HeaderText="UnAllocated" Visible="false" />--%>
                                                                                </Columns>
                                                                                <PagerStyle CssClass="pagination-ys" />
                                                                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                                                            </asp:GridView>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                            </div>
                                                        </asp:Panel>


                                                        <asp:Panel ID="divMonthlyDPMisc" runat="server">
                                                            <div class="row">
                                                                <h4 style="text-align: center;">MONTHLY MISCELLANEOUS DOWNPAYMENT SCHEDULE</h4>
                                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                    <asp:UpdatePanel runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:GridView runat="server" ID="gvMiscellaneous"
                                                                                CssClass="table table-hover table-responsive"
                                                                                AutoGenerateColumns="false"
                                                                                AllowPaging="true"
                                                                                PageSize="12"
                                                                                OnPageIndexChanging="gvMiscellaneous_PageIndexChanging"
                                                                                GridLines="None"
                                                                                ShowHeader="true">
                                                                                <HeaderStyle BackColor="#00cc66" />
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="Terms" HeaderText="Line Item No." />
                                                                                    <asp:BoundField DataField="PaymentType" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" HeaderText="Payment Type" />
                                                                                    <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd-MMM-yyyy}" />

                                                                                    <asp:BoundField DataField="PaymentAmount" ItemStyle-CssClass="text-right" DataFormatString="PHP {0:###,###,##0.##}" HeaderStyle-CssClass="text-center" HeaderText="Payment Amount (PHP)" />
                                                                                    <asp:BoundField DataField="Principal" ItemStyle-CssClass="text-right" DataFormatString="PHP {0:###,###,##0.##}" HeaderStyle-CssClass="text-center" HeaderText="Principal" />

                                                                                    <asp:BoundField DataField="InterestAmount" Visible="false" HeaderText="Interest" DataFormatString="PHP {0:###,###,##0.##}" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" />
                                                                                    <asp:BoundField DataField="Balance" DataFormatString="PHP {0:###,###,##0.00}" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-right" HeaderText="Balance (PHP)" />


                                                                                    <%--   <asp:BoundField DataField="Penalty" ControlStyle-CssClass="hidden" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Penalty" />
                                                                                    <asp:BoundField DataField="Misc" ControlStyle-CssClass="hidden" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Misc" />
                                                                                    <asp:BoundField DataField="UnAll" DataFormatString="PHP {0:###,###,##0.##}"
                                                                                        HeaderText="UnAllocated" Visible="false" />--%>
                                                                                </Columns>
                                                                                <PagerStyle CssClass="pagination-ys" />
                                                                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                                                            </asp:GridView>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                            </div>
                                                        </asp:Panel>




                                                        <asp:Panel ID="divMonthlyAmortMisc" runat="server">
                                                            <div class="row">
                                                                <h4 style="text-align: center;">MONTHLY MISCELLANEOUS AMORTIZATION SCHEDULE (LOANABLE BALANCE)</h4>
                                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                    <asp:UpdatePanel runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:GridView runat="server" ID="gvMiscellaneousAmort"
                                                                                CssClass="table table-hover table-responsive"
                                                                                AutoGenerateColumns="false"
                                                                                AllowPaging="true"
                                                                                PageSize="12"
                                                                                OnPageIndexChanging="gvMiscellaneousAmort_PageIndexChanging"
                                                                                GridLines="None"
                                                                                ShowHeader="true">
                                                                                <HeaderStyle BackColor="#00cc66" />
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="Terms" HeaderText="Line Item No." />
                                                                                    <asp:BoundField DataField="PaymentType" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" HeaderText="Payment Type" />
                                                                                    <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd-MMM-yyyy}" />

                                                                                    <asp:BoundField DataField="PaymentAmount" ItemStyle-CssClass="text-right" DataFormatString="PHP {0:###,###,##0.##}" HeaderStyle-CssClass="text-center" HeaderText="Payment Amount (PHP)" />
                                                                                    <asp:BoundField DataField="Principal" ItemStyle-CssClass="text-right" DataFormatString="PHP {0:###,###,##0.##}" HeaderStyle-CssClass="text-center" HeaderText="Principal" />

                                                                                    <asp:BoundField DataField="InterestAmount" Visible="false" HeaderText="Interest" DataFormatString="PHP {0:###,###,##0.##}" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" />
                                                                                    <asp:BoundField DataField="Balance" DataFormatString="PHP {0:###,###,##0.00}" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-right" HeaderText="Balance (PHP)" />


                                                                                    <%--   <asp:BoundField DataField="Penalty" ControlStyle-CssClass="hidden" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Penalty" />
                                                                                    <asp:BoundField DataField="Misc" ControlStyle-CssClass="hidden" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Misc" />
                                                                                    <asp:BoundField DataField="UnAll" DataFormatString="PHP {0:###,###,##0.##}"
                                                                                        HeaderText="UnAllocated" Visible="false" />--%>
                                                                                </Columns>
                                                                                <PagerStyle CssClass="pagination-ys" />
                                                                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                                                            </asp:GridView>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                            </div>
                                                        </asp:Panel>






                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <div class="trans-command">
                                                    <div class="btn-group" role="group">
                                                        <asp:UpdatePanel runat="server">
                                                            <ContentTemplate>
                                                                <asp:LinkButton runat="server" CssClass="btn btn-info btn-width" ID="PrevQuoatationSummary" OnClick="PrevQuoatationSummary_Click">
                                       <i class="glyphicon glyphicon-arrow-left"></i> Prev
                                                                </asp:LinkButton>
                                                                <asp:LinkButton runat="server" CssClass="btn btn-success btn-width" ID="bFinish" OnClick="bFinish_Click">
                                                                    <label runat="server" id="lblFinish" />
                                                                    <i class="glyphicon glyphicon-arrow-right"></i>
                                                                </asp:LinkButton>
                                                                <%-- <asp:LinkButton runat="server" CssClass="btn btn-success btn-width" ID="LinkButton1" OnClick="bPrint_Click" OnClientClick="SetTarget();" Visible="false">--%>
                                                                <asp:LinkButton runat="server" CssClass="btn btn-success btn-width" ID="bPrint" OnClick="bPrint_Click" Visible="false">
                                                                  Print
                                                                    <i class="glyphicon glyphicon-print"></i>
                                                                </asp:LinkButton>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:PostBackTrigger ControlID="bPrint" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                            <%-- ############################ END QuotationSummary ########################## --%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>








<asp:Content ID="Content4" ContentPlaceHolderID="modals" runat="server">
    <div class="modal fade" id="MsgNewQuotation" style="overflow-y: auto">
        <div class="modal-dialog" role="document">
            <div class="modal-content" id="form_BankAccount">
                <div class="modal-header bg-color">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                    <h4 class="modal-title">New Quotation</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                    <div class="row">
                                        <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                            <h5>Buyer Code:</h5>
                                        </div>
                                        <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                            <div class="input-group">
                                                <input type="text" style="z-index: auto;" class="form-control" runat="server" id="txtCardCode" readonly /><span class="input-group-btn">
                                                    <button id="btnCardCode" runat="server" style="width: 50px; z-index: auto;" class="btn btn-secondary" onserverclick="btnCardCode_ServerClick" type="button"><i class="fa fa-bars"></i></button>
                                                </span>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                            <h5>Buyer Type: <span class="color-red fsize-16">*</span></h5>
                                        </div>
                                        <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                            <asp:DropDownList runat="server" ID="ddlBusinessType" CssClass="form-control"
                                                AutoPostBack="true" Style="text-align: right;" OnSelectedIndexChanged="ddlBusinessType_SelectedIndexChanged">
                                                <asp:ListItem Text="Individual" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Corporation"></asp:ListItem>
                                                <asp:ListItem Text="Co-ownership"></asp:ListItem>
                                                <asp:ListItem Text="Guardianship"></asp:ListItem>
                                                <asp:ListItem Text="Trusteeship"></asp:ListItem>
                                                <asp:ListItem Text="Others"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                            <h5>Tax classification: <span class="color-red fsize-16">*</span></h5>
                                        </div>
                                        <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                            <asp:DropDownList runat="server" OnSelectedIndexChanged="ddTaxClass_SelectedIndexChanged" ID="ddTaxClass" DataTextField="Name" DataValueField="Code" CssClass="form-control"
                                                AutoPostBack="true" Style="text-align: right;">
                                                <asp:ListItem Text=" " Selected="True"></asp:ListItem>
                                            </asp:DropDownList>

                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator19"
                                                ControlToValidate="ddTaxClass"
                                                Text="Please select Tax Classification."
                                                ValidationGroup="newQuotation"
                                                runat="server" Style="color: red" />
                                        </div>
                                    </div>
                                    <div runat="server" id="divIndividual">



                                        <%-- <div class="row">
                                            <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                                <h5>Co-maker: <span class="color-red fsize-16">*</span></h5>
                                            </div>
                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                <input runat="server" maxlength="50" type="text" class="form-control text-uppercase" id="txtComaker" />
                                                <asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator23"
                                                    ControlToValidate="txtComaker"
                                                    Text="Please fill Co-maker field."
                                                    ValidationGroup="newQuotation"
                                                    runat="server" Style="color: red" />
                                            </div>
                                        </div>--%>

                                        <div class="row">
                                            <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                                <h5>Last Name: <span class="color-red fsize-16">*</span></h5>
                                            </div>
                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                <input runat="server" maxlength="50" type="text" class="form-control text-uppercase" id="txtLastName" />
                                                <asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator1"
                                                    ControlToValidate="txtLastName"
                                                    Text="Please fill Last Name field."
                                                    ValidationGroup="newQuotation"
                                                    runat="server" Style="color: red" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                                <h5>First Name: <span class="color-red fsize-16">*</span></h5>
                                            </div>
                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                <input runat="server" maxlength="50" type="text" class="form-control text-uppercase" id="txtFirstName" />
                                                <asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator10"
                                                    ControlToValidate="txtFirstName"
                                                    Text="Please fill First Name field."
                                                    ValidationGroup="newQuotation"
                                                    runat="server" Style="color: red" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                                <h5>Middle Name: <span class="color-red fsize-16">*</span></h5>
                                            </div>
                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                <input runat="server" maxlength="50" type="text" class="form-control text-uppercase" id="txtMiddleName" />
                                                <asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator11"
                                                    ControlToValidate="txtMiddleName"
                                                    Text="Please fill Middle Name field."
                                                    ValidationGroup="newQuotation"
                                                    runat="server" Style="color: red" />
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                                <h5>Birthday: <span class="color-red fsize-16">*</span></h5>
                                            </div>
                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                <input runat="server" type="date" class="form-control" id="txtBirthday" />
                                                <asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator12"
                                                    ControlToValidate="txtBirthday"
                                                    Text="Please fill Birthday field."
                                                    ValidationGroup="newQuotation"
                                                    runat="server" Style="color: red" />
                                            </div>
                                        </div>
                                        <div class="row" runat="server" id="divEmp">
                                            <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                                <h5>Nature of Employment: <span class="color-red fsize-16">*</span></h5>
                                            </div>
                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                <div class="input-group">
                                                    <input type="text" style="z-index: auto;" class="form-control text-uppercase" runat="server" id="tNatureofEmp" disabled required /><span class="input-group-btn">
                                                        <button id="bNatureofEmp" runat="server" style="width: 50px; z-index: auto;" data-toggle="modal" data-target="#MsgQuotation" class="btn btn-secondary" type="button" onserverclick="bNatureofEmp_ServerClick"><i class="fa fa-bars"></i></button>
                                                    </span>
                                                </div>
                                                <asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator13"
                                                    ControlToValidate="tNatureofEmp"
                                                    Text="Please fill Nature of Emp field."
                                                    ValidationGroup="newQuotation"
                                                    runat="server" Style="color: red" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                                <h5>Type of ID: <span class="color-red fsize-16">*</span></h5>
                                            </div>
                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                <div class="input-group">
                                                    <input runat="server" style="z-index: auto;" id="tTypeOfId" class="form-control text-uppercase" type="text" disabled required /><span class="input-group-btn">
                                                        <button id="bTypeofID" runat="server" style="width: 50px; z-index: auto;" data-toggle="modal" data-target="#MsgQuotation" class="btn btn-secondary" type="button" onserverclick="bNatureofEmp_ServerClick"><i class="fa fa-bars"></i></button>

                                                    </span>
                                                </div>
                                                <asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator14"
                                                    ControlToValidate="tTypeOfId"
                                                    Text="Please fill Nature of Type of ID."
                                                    ValidationGroup="newQuotation"
                                                    runat="server" Style="color: red" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                                <h5>ID Number: <span class="color-red fsize-16">*</span></h5>
                                            </div>
                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                <input type="text" style="z-index: auto;" class="form-control text-uppercase" runat="server" id="tIDNo" disabled />
                                                <asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator15"
                                                    ControlToValidate="tIDNo"
                                                    Text="Please fill ID No. field"
                                                    ValidationGroup="newQuotation"
                                                    runat="server" Style="color: red" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                                <h5>TIN: <span class="color-red fsize-16">*</span></h5>
                                            </div>
                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                <%--<input type="text" style="z-index: auto;" onkeypress="return fnAllowNumeric(event)" maxlength="15" onkeyup="TIN_keyup(event);" class="form-control text-uppercase" runat="server" id="tTIN1" />--%>
                                                <%--<input type="text" style="z-index: auto;" onkeypress="return fnAllowNumeric(event)" maxlength="15" onkeyup="TIN_keyup(event);" class="form-control text-uppercase" runat="server" id="tTIN1" />--%>
                                                <asp:TextBox runat="server" AutoPostBack="true" type="text" class="form-control" ID="tTIN1" onkeypress="return fnAllowNumeric(event)" onkeyup="TIN_keyup(event);"
                                                    placeholder="xxx-xxx-xxx-xxx" MaxLength="15" ValidationGroup="newQuotation"></asp:TextBox>


                                                <asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator3"
                                                    ControlToValidate="tTIN1"
                                                    Text="Please fill TIN field"
                                                    ValidationGroup="newQuotation"
                                                    runat="server" Style="color: red" />
                                                <asp:CustomValidator
                                                    Style="color: red"
                                                    runat="server"
                                                    ID="CustomValidator14"
                                                    ValidationGroup="newQuotation"
                                                    ControlToValidate="tTIN1"
                                                    Text="Invalid TIN format. Must be 000-000-000-000"
                                                    OnServerValidate="CustomValidator14_ServerValidate1" />


                                            </div>
                                        </div>
                                    </div>

                                    <div runat="server" id="divCorp">
                                        <div class="row">
                                            <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                                <h5>Company Name: <span class="color-red fsize-16">*</span> </h5>
                                            </div>
                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                <input runat="server" maxlength="50" type="text" class="form-control text-uppercase" id="txtCompanyName" />
                                                <asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator17"
                                                    ControlToValidate="txtCompanyName"
                                                    Text="Please fill Company Name field."
                                                    ValidationGroup="newQuotation"
                                                    runat="server" Style="color: red" />
                                            </div>
                                        </div>
                                        <%--                                        <div class="row">
                                            <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                                <h5>Co-maker: <span class="color-red fsize-16">*</span></h5>
                                            </div>
                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                <input runat="server" maxlength="50" type="text" class="form-control text-uppercase" id="txtComakerCorp" />
                                                <asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator3"
                                                    ControlToValidate="txtComakerCorp"
                                                    Text="Please fill Co-maker field."
                                                    ValidationGroup="newQuotation"
                                                    runat="server" Style="color: red" />
                                            </div>
                                        </div>--%>
                                        <div class="row">
                                            <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                                <h5>TIN: <span class="color-red fsize-16">*</span> </h5>
                                            </div>
                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                <input runat="server" maxlength="15" onkeypress="return fnAllowNumeric(event)" onkeyup="TIN_keyup(event);" type="text" class="form-control text-uppercase" id="txtTinNumber" />
                                                <asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator16"
                                                    ControlToValidate="txtTinNumber"
                                                    Text="Please enter TIN"
                                                    ValidationGroup="newQuotation"
                                                    runat="server" Style="color: red" />
                                            </div>
                                        </div>
                                    </div>


                                    <hr />
                                    <hr />
                                    <h4 runat="server" id="specHeader">Guardianship Details </h4>
                                    <div runat="server" id="divSpec">
                                        <div class="row">
                                            <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                                <h5>Last Name:</h5>
                                            </div>
                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                <input type="text" style="z-index: auto;" class="form-control text-uppercase" runat="server" id="tspecLName" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                                <h5>First Name:</h5>
                                            </div>
                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                <input type="text" style="z-index: auto;" class="form-control text-uppercase" runat="server" id="tspecFName" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                                <h5>Middle Name:</h5>
                                            </div>
                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                <input type="text" style="z-index: auto;" class="form-control text-uppercase" runat="server" id="tspecMName" />
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                                <h5>Relationship to Principal Buyer:</h5>
                                            </div>
                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                <input type="text" style="z-index: auto;" class="form-control text-uppercase" runat="server" id="tspecRelationship" />
                                            </div>
                                        </div>
                                    </div>








                                    <div runat="server" id="divothers">
                                        <div class="row">
                                            <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                                <h5>Others: <span class="color-red fsize-16">*</span></h5>
                                            </div>
                                            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                <div class="input-group">
                                                    <input type="text" style="z-index: auto;" class="form-control text-uppercase" runat="server" id="txtOthers" readonly /><span class="input-group-btn">
                                                        <button id="btnOthers" runat="server" style="width: 50px; z-index: auto;" data-toggle="modal" data-target="#MsgOthers" class="btn btn-secondary" type="button" onserverclick="btnCoOwner_ServerClick"><i class="fa fa-bars"></i></button>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <button runat="server" id="btnAcceptNewBuyer" validationgroup="newQuotation" onserverclick="btnAcceptNewBuyer_ServerClick" style="width: 100px;" class="btn btn-primary" type="button">OK </button>
                            <button runat="server" style="width: 100px;" class="btn btn-default" type="button" data-dismiss="modal">Cancel </button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>





    <div class="modal fade" id="MsgSampleActual" role="dialog" tabindex="-1" style="z-index: 1051;">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header bg-color">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                    <h4 class="modal-title">Create Quotation</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 text-center">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <div class="col-lg-5">
                                        <%--<button runat="server" id="bSample" class="btn btn-default btn-warning" onserverclick="tSample_Click" type="button">Sample Computation</button>--%>
                                        <asp:LinkButton CssClass="btn btn-default btn-warning" OnClientClick="clientSampleClick();" runat="server" Text="Sample Computation" OnClick="tSample_Click" />
                                    </div>
                                    <div class="col-lg-2">
                                    </div>
                                    <div class="col-lg-5">
                                        <%--<button runat="server" id="btnBAAdd" class="btn btn-primary" onserverclick="btnBAAdd_ServerClick" type="button">Create Amortization </button>--%>
                                        <asp:LinkButton CssClass="btn btn-primary" OnClientClick="clientCreateAmortClick();" runat="server" Text="Create Amortization" ID="btnBAAdd" OnClick="btnBAAdd_ServerClick" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <%--<button style="width: 100px;" class="btn btn-primary" type="button" data-dismiss="modal" aria-label="Close">OK</button>--%>
                </div>
            </div>
        </div>
    </div>



    <div class="modal fade" id="MsgNoti" role="dialog" tabindex="-1" style="z-index: 1051;">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header bg-color">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                    <h4 class="modal-title">Quotation</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <input type="text" id="lblMsg" runat="server" style="border: none; width: 100%;" readonly />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button style="width: 100px;" class="btn btn-primary" type="button" data-dismiss="modal" aria-label="Close">OK</button>
                </div>
            </div>
        </div>
    </div>




    <div class="modal fade" id="MsgQuotation" role="dialog" tabindex="-1">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header bg-color">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                            <h4 class="modal-title">
                                <label runat="server" id="ChooseText" />
                            </h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                    <fieldset>
                                        <asp:GridView ID="gvList" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                            CssClass="table table-bordered table-hover" Width="100%" ShowHeader="True" PageSize="10" OnPageIndexChanging="gvList_PageIndexChanging">
                                            <HeaderStyle BackColor="#66ccff" />
                                            <Columns>
                                                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-Font-Size="Medium" />
                                                <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" ID="bSelect" CssClass="btn btn-default btn-success" Width="100%" Height="100%" OnClick="bSelect_Click" CommandArgument='<%# Bind("Code")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle CssClass="pagination-ys" />
                                            <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" Font-Bold="true" />
                                        </asp:GridView>
                                    </fieldset>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button class="btn btn-default btn-facebook" type="button" style="width: 100px;" data-dismiss="modal">OK</button>
                        </div>
                    </div>
                </div>
                5c
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <%--<div class="modal fade" id="MsgBox" role="dialog" tabindex="-1" style="z-index: 1051;">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header bg-color">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                    <h4 class="modal-title">Quotation</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <p>
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <label runat="server" id="lblMsgBox" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </p>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <button runat="server" id="btnYes" style="width: 100px;" class="btn btn-primary" type="button" data-dismiss="modal">Yes</button>
                            <button runat="server" id="btnNo" style="width: 100px;" class="btn btn-default" type="button" data-dismiss="modal">
                                <label runat="server" id="lblNo" style="height: 5px; font-weight: normal;" />
                            </button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>--%>






    <div id="modalConfirmation1" class="modal" data-focus-on="input:first">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="row" style="text-align: center;">
                                <div class="col-lg-12">
                                    <asp:Image ImageUrl="~/assets/img/question.png" runat="server" ID="Image2" Width="90" Height="90" />
                                </div>
                            </div>
                            <div class="row" style="text-align: center">
                                <div class="col-lg-12">
                                    <h4>
                                        <asp:Label runat="server" ID="lblConfirmationInfo1"></asp:Label></h4>
                                </div>
                            </div>
                            <div class="row" style="text-align: center">
                                <div class="col-lg-12">
                                    <asp:Button runat="server" ID="btnYes" CssClass="btn btn-info btn-sm" Text="Yes" TabIndex="0" AutoFocus="true" OnClick="btnYes_Click" Style="width: 90px;" />
                                    <button type="button" class="btn btn-default btn-sm" data-dismiss="modal" style="width: 90px;" onclick="hideConfirm()">No</button>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>










    <div id="modalAlert" class="modal fade" style="z-index: 9999">
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



    <div class="modal fade" id="MsgCoMaker" role="dialog" tabindex="-1">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog modal-lg" role="document" style="width: 75%">
                    <div class="modal-content">
                        <div class="modal-header bg-color">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                            <h4 class="modal-title">List of Co-Borrowers
                            </h4>
                        </div>
                        <div class="modal-body">
                            <asp:Panel runat="server" DefaultButton="bSearch">
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <div class="row">
                                            <div class="col-lg-2 col-md-2 col-sm-6 col-xs-12">
                                                <h5>Search: </h5>
                                            </div>
                                            <div class="col-lg-10 col-md-10 col-sm-6 col-xs-12">
                                                <div class="input-group">
                                                    <input runat="server" id="txtCoMakerSearch" placeholder="Search here" class="form-control" type="text" autofocus /><span class="input-group-btn">
                                                        <asp:LinkButton runat="server" ID="btnCoMakerSearch" Style="width: 50px;" CssClass="btn btn-secondary btn-default btn-facebook" OnClick="btnCoMakerSearch_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <fieldset>
                                                    <asp:GridView ID="gvCoMaker" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                                        CssClass="table table-bordered table-hover" Width="100%" ShowHeader="True" PageSize="5"
                                                        OnPageIndexChanging="gvCoMaker_PageIndexChanging">
                                                        <HeaderStyle BackColor="#66ccff" />
                                                        <Columns>
                                                            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-Font-Size="Medium" ItemStyle-CssClass="text-uppercase" />
                                                            <asp:TemplateField HeaderStyle-Width="10px" HeaderText="Select" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="bSelectCoMaker" CssClass="btn btn-default btn-success" Width="100%" Height="100%" OnClick="bSelectCoMaker_Click" CommandArgument='<%# Bind("Name")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderStyle-Width="10px" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="bDeleteCoMaker" CssClass="btn btn-default btn-danger" Width="100%" Height="100%" OnClick="bDeleteCoMaker_Click1" CommandArgument='<%# Bind("Name")%>'><i class="fa fa-trash"></i></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                        </Columns>
                                                        <PagerStyle CssClass="pagination-ys" />
                                                        <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                                    </asp:GridView>
                                                </fieldset>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                        </div>
                        <div class="modal-footer">
                            <button runat="server" id="Button1" class="btn btn-default btn-primary" type="button" style="width: 100px;" onserverclick="bCreateNewCoborrower_ServerClick">Create New</button>
                            <button runat="server" style="width: 100px;" class="btn btn-default" type="button" data-dismiss="modal">Cancel </button>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div class="modal fade" id="MsgCoOwner" role="dialog" tabindex="-1">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog modal-lg" role="document" style="width: 75%">
                    <div class="modal-content">
                        <div class="modal-header bg-color">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                            <h4 class="modal-title">List of Co-Owners
                            </h4>
                        </div>
                        <div class="modal-body">
                            <asp:Panel runat="server" DefaultButton="bSearch">
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <%--                                        <div class="row">
                                            <div class="col-lg-2 col-md-2 col-sm-6 col-xs-12">
                                                <h5>Search: </h5>
                                            </div>
                                            <div class="col-lg-10 col-md-10 col-sm-6 col-xs-12">
                                                <div class="input-group">
                                                    <input runat="server" id="txtCoOwnerSearch" placeholder="Search here" class="form-control" type="text" autofocus /><span class="input-group-btn">
                                                        <asp:LinkButton runat="server" ID="btnCoOwnerSearch" Style="width: 50px;" CssClass="btn btn-secondary btn-default btn-facebook" OnClick="btnCoMakerSearch_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>--%>
                                    </div>
                                </div>
                            </asp:Panel>

                        </div>
                        <div class="modal-footer">
                            <button runat="server" style="width: 100px;" class="btn btn-default" type="button" data-dismiss="modal">Cancel </button>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div class="modal fade" id="MsgOthers" role="dialog" tabindex="-1">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog modal-lg" role="document" style="width: 75%">
                    <div class="modal-content">
                        <div class="modal-header bg-color">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                            <h4 class="modal-title">List of Other buyers
                            </h4>
                        </div>
                        <div class="modal-body">
                            <asp:Panel runat="server" DefaultButton="bSearch">
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <%--                                        <div class="row">
                                            <div class="col-lg-2 col-md-2 col-sm-6 col-xs-12">
                                                <h5>Search: </h5>
                                            </div>
                                            <div class="col-lg-10 col-md-10 col-sm-6 col-xs-12">
                                                <div class="input-group">
                                                    <input runat="server" id="txtCoOwnerSearch" placeholder="Search here" class="form-control" type="text" autofocus /><span class="input-group-btn">
                                                        <asp:LinkButton runat="server" ID="btnCoOwnerSearch" Style="width: 50px;" CssClass="btn btn-secondary btn-default btn-facebook" OnClick="btnCoMakerSearch_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>--%>
                                        <br />
                                        <div class="row">
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <fieldset>
                                                    <asp:GridView ID="gvOthers" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                                        CssClass="table table-bordered table-hover" Width="100%" ShowHeader="True" PageSize="5"
                                                        OnPageIndexChanging="gvOthers_PageIndexChanging">
                                                        <HeaderStyle BackColor="#66ccff" />
                                                        <Columns>
                                                            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-Font-Size="Medium" ItemStyle-CssClass="text-uppercase" />
                                                            <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="bSelectOthers" CssClass="btn btn-default btn-success" Width="100%" Height="100%" OnClick="bSelectOthers_Click" CommandArgument='<%# Bind("Name")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerStyle CssClass="pagination-ys" />
                                                        <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                                    </asp:GridView>
                                                </fieldset>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                        </div>
                        <div class="modal-footer">
                            <button runat="server" id="Button3" class="btn btn-default btn-primary" type="button" style="width: 100px;" onserverclick="bCreateNewCoOwner_ServerClick">Create New</button>
                            <button runat="server" style="width: 100px;" class="btn btn-default" type="button" data-dismiss="modal">Cancel </button>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="modal fade" id="MsgBuyers" role="dialog" tabindex="-1">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog modal-lg" role="document" style="width: 75%">
                    <div class="modal-content">
                        <div class="modal-header bg-color">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close" onclick="location.href = 'Dashboard.aspx'"><span aria-hidden="true">×</span></button>
                            <h4 class="modal-title">List of Quotations
                            </h4>
                        </div>
                        <div class="modal-body">




                            <asp:Panel runat="server" DefaultButton="bSearch">
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <div class="row">
                                            <div class="col-lg-2 col-md-2 col-sm-6 col-xs-12">
                                                <h5>Search: </h5>
                                            </div>
                                            <div class="col-lg-10 col-md-10 col-sm-6 col-xs-12">
                                                <div class="input-group">
                                                    <input runat="server" id="txtSearch" placeholder="Search here" class="form-control" type="text" autofocus /><span class="input-group-btn">
                                                        <asp:LinkButton runat="server" ID="bSearch" Style="width: 50px;" CssClass="btn btn-secondary btn-default btn-facebook" OnClick="bSearch_ServerClick"><i class="fa fa-search"></i></asp:LinkButton>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <fieldset>
                                                    <asp:GridView ID="gvBuyers" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                                        CssClass="table table-bordered table-hover" Width="100%" ShowHeader="True" PageSize="5" OnPageIndexChanging="gvBuyers_PageIndexChanging">
                                                        <HeaderStyle BackColor="#66ccff" />
                                                        <Columns>
                                                            <asp:BoundField DataField="DocNum" HeaderText="Doc No." SortExpression="DocNum" ItemStyle-Font-Size="Medium" />
                                                            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-Font-Size="Medium" />
                                                            <asp:BoundField DataField="ProjCode" HeaderText="Project" SortExpression="Name" ItemStyle-Font-Size="Medium" />
                                                            <asp:BoundField DataField="Block" HeaderText="Block" SortExpression="Name" ItemStyle-Font-Size="Medium" />
                                                            <asp:BoundField DataField="Lot" HeaderText="Lot" SortExpression="Name" ItemStyle-Font-Size="Medium" />
                                                            <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="bSelectBuyer" CssClass="btn btn-default btn-success" Width="100%" Height="100%" OnClick="bSelectBuyer_Click" CommandArgument='<%# Bind("DocEntry")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerStyle CssClass="pagination-ys" />
                                                        <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                                    </asp:GridView>
                                                </fieldset>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                        </div>
                        <div class="modal-footer">
                            <button runat="server" id="bCreateNew" class="btn btn-default btn-primary" type="button" style="width: 100px;" onserverclick="bCreateNew_ServerClick">Create New</button>
                            <button runat="server" onclick="location.href = 'Dashboard.aspx'" style="width: 100px;" class="btn btn-default" type="button" data-dismiss="modal">Cancel </button>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div class="modal fade" id="MsgEmpList" role="dialog" tabindex="-1">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header bg-color">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                            <h4 class="modal-title">Accredited Realty and Sellers 
                            </h4>
                        </div>



                        <div class="modal-body">

                            <asp:Panel runat="server" DefaultButton="btnSearchEmpList">

                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <div class="row">
                                            <div class="col-lg-2 col-md-2 col-sm-6 col-xs-12">
                                                <h5>Search: </h5>
                                            </div>
                                            <div class="col-lg-10 col-md-10 col-sm-6 col-xs-12">
                                                <div class="input-group">
                                                    <input runat="server" id="txtSearchEmpList" placeholder="Search here" class="form-control" type="text" autofocus /><span class="input-group-btn">
                                                        <%--<button runat="server" id="btnSearchEmpList" style="width: 50px;" class="btn btn-secondary btn-default btn-facebook" type="button" onserverclick="btnSearchEmpList_ServerClick"><i class="fa fa-search"></i></button>--%>
                                                        <asp:LinkButton runat="server" ID="btnSearchEmpList" Style="width: 50px;" CssClass="btn btn-secondary btn-default btn-facebook" OnClick="btnSearchEmpList_ServerClick"><i class="fa fa-search"></i></asp:LinkButton>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <fieldset>
                                                    <asp:GridView ID="gvEmpList" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                                        CssClass="table table-bordered table-hover" Width="100%" ShowHeader="True" PageSize="10" OnPageIndexChanging="gvEmpList_PageIndexChanging">
                                                        <HeaderStyle BackColor="#66ccff" />
                                                        <Columns>
                                                            <asp:BoundField DataField="CardName" HeaderText="Name" SortExpression="Name" ItemStyle-Font-Size="Medium" />
                                                            <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="btnSelectEmpList" CssClass="btn btn-default btn-success" Width="100%" Height="100%" OnClick="btnSelectEmpList_Click" CommandArgument='<%# Bind("CardCode")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerStyle CssClass="pagination-ys" />
                                                        <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" Font-Bold="true" />
                                                    </asp:GridView>
                                                </fieldset>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                        <div class="modal-footer">
                            <button runat="server" style="width: 100px;" class="btn btn-default" type="button" data-dismiss="modal">Cancel </button>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div id="MsgProjList" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Project List</h4>
                </div>
                <div class="modal-body">


                    <asp:Panel runat="server" DefaultButton="btnSearchProject">

                        <%-- //2023-06-05 : ADD SEARCH FUNCTION FOR PROJECT--%>
                        <div class="row">
                            <asp:UpdatePanel runat="server" DefaultButton="btnSearchProject">
                                <ContentTemplate>
                                    <div class="col-lg-2 col-md-2 col-sm-6 col-xs-12">
                                        <h5>Search: </h5>
                                    </div>
                                    <div class="col-lg-10 col-md-10 col-sm-6 col-xs-12">
                                        <div class="input-group">
                                            <input runat="server" id="txtSearchProject" placeholder="Search here" class="form-control" type="text" autofocus /><span class="input-group-btn">
                                                <asp:LinkButton runat="server" ID="btnSearchProject" Style="width: 50px;" CssClass="btn btn-secondary btn-default btn-facebook" OnClick="btnSearchProject_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                            </span>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>



                        <div class="row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>



                                        <asp:GridView ID="gvProjectList" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                            CssClass="table table-bordered table-hover " Width="100%" ShowHeader="True" PageSize="5" OnPageIndexChanging="gvProjectList_PageIndexChanging">
                                            <HeaderStyle BackColor="#66ccff" />
                                            <Columns>
                                                <asp:BoundField DataField="PrjCode" HeaderText="Project Code" />
                                                <asp:BoundField DataField="PrjName" HeaderText="Project Name" />
                                                <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" ID="bSelectProject" CssClass="btn btn-default btn-success" Width="100%" Height="100%" OnClick="bSelectProject_Click" CommandArgument='<%# Bind("PrjCode")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
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
                    </asp:Panel>


                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <div id="MsgBPList" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Buyers List</h4>
                </div>
                <div class="modal-body">
                    <asp:Panel runat="server" DefaultButton="bSearchBuyer">
                        <div class="row">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <div class="col-lg-2 col-md-2 col-sm-6 col-xs-12">
                                        <h5>Search: </h5>
                                    </div>
                                    <div class="col-lg-10 col-md-10 col-sm-6 col-xs-12">
                                        <div class="input-group">
                                            <input runat="server" id="txtSearchbuyer" placeholder="Search here" class="form-control" type="text" autofocus /><span class="input-group-btn">
                                                <asp:LinkButton runat="server" ID="bSearchBuyer" Style="width: 50px;" CssClass="btn btn-secondary btn-default btn-facebook" OnClick="bSearchBuyer_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                            </span>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvBPList" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                            CssClass="table table-bordered table-hover " Width="100%" ShowHeader="True" PageSize="8" OnPageIndexChanging="gvBPList_PageIndexChanging">
                                            <HeaderStyle BackColor="#66ccff" />
                                            <Columns>
                                                <asp:BoundField DataField="CardCode" HeaderText="Code" />
                                                <asp:BoundField DataField="Name" HeaderText="Name" />
                                                <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" ID="bSelectBP" CssClass="btn btn-default btn-success" Width="100%" Height="100%" OnClick="bSelectBP_Click" CommandArgument='<%# Bind("CardCode")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
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
                    </asp:Panel>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>


    <div id="MsgHouseModel" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">House Model List</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="gvHouseModelList" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                        CssClass="table table-bordered table-hover " Width="100%" ShowHeader="True" PageSize="5" OnPageIndexChanging="gvHouseModelList_PageIndexChanging">
                                        <HeaderStyle BackColor="#66ccff" />
                                        <Columns>
                                            <asp:BoundField DataField="U_Code" HeaderText="House Model Code" />
                                            <asp:BoundField DataField="U_Name" HeaderText="House Model Name" />
                                            <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="bSelectHouseModel" OnClick="bSelectHouseModel_Click" CssClass="btn btn-default btn-success" Width="100%" Height="100%" CommandArgument='<%# Bind("U_Code")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
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



    <div id="MsgLoanType" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Loan Type List</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="gvLoanType" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                        CssClass="table table-bordered table-hover " Width="100%" ShowHeader="True" PageSize="5" OnPageIndexChanging="gvLoanType_PageIndexChanging">
                                        <HeaderStyle BackColor="#66ccff" />
                                        <Columns>
                                            <asp:BoundField DataField="Code" HeaderText="Loan Type Code" />
                                            <asp:BoundField DataField="Name" HeaderText="Loan Type Name" />
                                            <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="bLoanType" OnClick="bLoanType_Click" CssClass="btn btn-default btn-success" Width="100%" Height="100%" CommandArgument='<%# Bind("Code")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
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

    <div id="MsgRetitlingType" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Retitling Type List</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="gvRetType" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                        CssClass="table table-bordered table-hover " Width="100%" ShowHeader="True" PageSize="5">
                                        <HeaderStyle BackColor="#66ccff" />
                                        <Columns>
                                            <asp:BoundField DataField="Name" HeaderText="Retitling Type Name" />
                                            <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="bRetitlingType" OnClick="bRetitlingType_Click" CssClass="btn btn-default btn-success" Width="100%" Height="100%" CommandArgument='<%# Bind("Name")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
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

    <div id="MsgAdjLot" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Sold w/ Adjacent Lot</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="gvAdjLot" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                        CssClass="table table-bordered table-hover " Width="100%" ShowHeader="True" PageSize="5">
                                        <HeaderStyle BackColor="#66ccff" />
                                        <Columns>
                                            <asp:BoundField DataField="Name" />
                                            <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="bAdjLot" OnClick="bAdjLot_Click" CssClass="btn btn-default btn-success" Width="100%" Height="100%" CommandArgument='<%# Bind("Name")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
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

    <div id="MsgLotNo" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Adjacent Lot Quotation No.</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="gvLotNo" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                        CssClass="table table-bordered table-hover " Width="100%" ShowHeader="True" PageSize="5" OnPageIndexChanging="gvLotNo_PageIndexChanging">
                                        <HeaderStyle BackColor="#66ccff" />
                                        <Columns>
                                            <asp:BoundField DataField="Block" HeaderText="Block" />
                                            <asp:BoundField DataField="Lot" HeaderText="Lot" />
                                            <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="bLotNo" OnClick="bLotNo_Click" CssClass="btn btn-default btn-success" Width="100%" Height="100%" CommandArgument='<%# Bind("Lot")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
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

    <div id="MsgIncentive" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Incentive Option</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="gvIncentiveOption" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                        CssClass="table table-bordered table-hover " Width="100%" ShowHeader="True" PageSize="5">
                                        <HeaderStyle BackColor="#66ccff" />
                                        <Columns>
                                            <asp:BoundField DataField="Name" />
                                            <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="bIncentive" OnClick="bIncentive_Click" CssClass="btn btn-default btn-success" Width="100%" Height="100%" CommandArgument='<%# Bind("Name")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
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





    <div id="MsgAccreditedBanks" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">List of Accredited Banks</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="gvBanks" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                        CssClass="table table-bordered table-hover " Width="100%" ShowHeader="True" PageSize="5" OnPageIndexChanging="gvBanks_PageIndexChanging">
                                        <HeaderStyle BackColor="#66ccff" />
                                        <Columns>
                                            <asp:BoundField DataField="Code" HeaderText="Bank Code" />
                                            <asp:BoundField DataField="Name" HeaderText="Bank Name" />
                                            <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="bBanks" OnClick="bBanks_Click" CssClass="btn btn-default btn-success" Width="100%" Height="100%" CommandArgument='<%# Bind("Code")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
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



    <div id="MsgBlockLotList" class="modal fade" role="dialog">
        <div class="modal-dialog modal-lg">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Lot List</h4>
                </div>
                <div class="modal-body">
                    <div class="row" style="overflow: auto; margin: 0 auto; height: 100%; width: 100%">
                        <div class="col-lg-12">
                            <div style="width: inherit">
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <p align="center" id="Divblocklot">
                                            <canvas id="imgProjectBlock" style="border: 1px solid black; margin: 0px; height: 100%; width: 100%">Your browser does not support the Canvas Element
                                            </canvas>
                                        </p>
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
                                            <asp:GridView ID="gvBlockColor" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                                CssClass="table table-bordered table-hover " Width="100%" ShowHeader="True" PageSize="5" OnPageIndexChanging="gvBlockColor_PageIndexChanging">
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
                                        <p align="center" runat="server">
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
                                                            <asp:Image runat="server" ID="imgHouse" ImageUrl='<%# "~/Handler/HouseModel.ashx?ID=" + Eval("U_Picture")%>' Width="100%" Height="180px" />
                                                        </div>
                                                        <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                            <div class="row">
                                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-3">
                                                                    Model :
                                                                </div>
                                                                <div class="col-lg-9 col-md-9 col-sm-9 col-xs-9">
                                                                    <asp:Label runat="server" ID="lblModel" Text='<%# Bind("U_Model") %>'></asp:Label>
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
                                                                    Reservation Fee :
                                                                </div>
                                                                <div class="col-lg-5 col-md-5 col-sm-5 col-xs-5">
                                                                    <asp:Label runat="server" ID="Label3" Text='<%# Bind("U_ResFee") %>'></asp:Label>
                                                                </div>
                                                                <div class="col-lg-2 col-md-2 col-sm-2 col-xs-2">
                                                                    <asp:LinkButton runat="server" ID="bPicPreview" CssClass="btn btn-default" Width="85px" Height="100%" OnClick="bPicPreview_ServerClick" CommandArgument='<%# Bind("DocEntry")%>'><i class="fa fa-image"> Preview</i></asp:LinkButton>
                                                                </div>
                                                                <div class="col-lg-2 col-md-2 col-sm-2 col-xs-2">
                                                                    <asp:LinkButton runat="server" ID="bChooseHouse" CssClass="btn btn-default btn-success" Width="85px" Height="100%" OnClick="bChooseHouse_Click" CommandArgument='<%# Bind("DocEntry")%>'><i class="fa fa-hand-o-up"> Choose</i></asp:LinkButton>
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

    <%--//Modal for Co-Borrower--%>
    <div class="modal fade" id="modal-coborrower">
        <div class="modal-dialog">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span></button>
                            <h3 class="modal-title">Co-Borrower</h3>
                        </div>
                        <div class="modal-body">
                            <div class="form-group">
                                <asp:TextBox runat="server" ID="mtxtRow" class="form-control text-uppercase hidden" type="text" />
                            </div>
                            <div class="form-group">
                                <label>Last Name <span class="color-red fsize-16">*</span></label>
                                <asp:TextBox runat="server" ID="mtxtLName" class="form-control text-uppercase" type="text" placeholder="Dela Cruz" />
                                <asp:RequiredFieldValidator
                                    ID="RequiredFieldValidator23"
                                    ControlToValidate="mtxtLName"
                                    Text="Please fill up Last Name."
                                    ValidationGroup="saveborrower"
                                    runat="server" Style="color: red" />
                            </div>
                            <div class="form-group">
                                <label>First Name <span class="color-red fsize-16">*</span></label>
                                <asp:TextBox runat="server" ID="mtxtFName" class="form-control text-uppercase" type="text" placeholder="Juan" />
                                <asp:RequiredFieldValidator
                                    ID="RequiredFieldValidator26"
                                    ControlToValidate="mtxtFName"
                                    Text="Please fill up First Name."
                                    ValidationGroup="saveborrower"
                                    runat="server" Style="color: red" />
                            </div>
                            <div class="form-group">
                                <label>Middle Name <span class="color-red fsize-16">*</span></label>
                                <asp:TextBox runat="server" ID="mtxtMName" class="form-control text-uppercase" type="text" placeholder="D" />
                                <asp:RequiredFieldValidator
                                    ID="RequiredFieldValidator27"
                                    ControlToValidate="mtxtMName"
                                    Text="Please fill up Middle Name."
                                    ValidationGroup="saveborrower"
                                    runat="server" Style="color: red" />
                            </div>
                            <div class="form-group">
                                <label>Relationship <span class="color-red fsize-16">*</span></label>
                                <asp:TextBox runat="server" ID="mtxtRelationship" class="form-control text-uppercase" type="text" placeholder="Son" />
                                <asp:RequiredFieldValidator
                                    ID="RequiredFieldValidator28"
                                    ControlToValidate="mtxtRelationship"
                                    Text="Please fill up Relationship."
                                    ValidationGroup="saveborrower"
                                    runat="server" Style="color: red" />
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-danger pull-left" data-dismiss="modal">Close</button>
                            <button id="btnAddCoborrower" onserverclick="btnAddCoborrower_ServerClick" runat="server" type="button" validationgroup="saveborrower" class="btn btn-success">Add Co-Borrower</button>
                            <button id="btnUpdate" onserverclick="btnAddCoborrower_ServerClick" runat="server" type="button" validationgroup="saveborrower" class="btn btn-success" visible="false">Update Co-Borrower</button>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>





    <%--//modal confirmation--%>
    <div id="modalConfirmation" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="row" style="text-align: center;">
                                <div class="col-lg-12">
                                    <asp:Image ImageUrl="~/assets/img/info.png" runat="server" ID="imgQuestion" Width="90" Height="90" />
                                </div>
                            </div>
                            <div class="row" style="text-align: center">
                                <div class="col-lg-12">
                                    <h4>
                                        <asp:Label runat="server" ID="lblConfirmationInfo">Loanable amount is more than 6M. Co-maker is required.</asp:Label></h4>
                                </div>
                            </div>
                            <div class="row" style="text-align: center">
                                <div class="col-lg-12">
                                    <asp:Button runat="server" ID="btnWaive" CssClass="btn btn-info pull-left" OnClick="btnWaive_Click" Text="Send waive request" />
                                    <button type="button" class="btn btn-default pull-right" data-dismiss="modal">Cancel Contract</button>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <div id="modalOTP" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="row" style="text-align: center;">
                                <div class="col-lg-12">
                                    <asp:Image ImageUrl="~/assets/img/info.png" runat="server" ID="Image1" Width="90" Height="90" />
                                </div>
                            </div>
                            <div class="row" style="text-align: center">
                                <div class="col-lg-12">
                                    <h4>
                                        <asp:Label runat="server" ID="Label1">Please provide OTP sent via Email.</asp:Label></h4>
                                </div>
                                <div class="col-lg-3">&nbsp;</div>
                                <div class="col-lg-6">
                                    <asp:TextBox runat="server" ID="txtOTP" CssClass="form-control" onkeypress="return fnAllowNumeric(event)"></asp:TextBox>
                                </div>
                                <div class="col-lg-3">&nbsp;</div>
                            </div>
                            <div class="row" style="text-align: center">
                                <div class="col-lg-12" style="align-items: center;">
                                    <asp:Button runat="server" ID="btnConfirmOTP" CssClass="btn btn-info" OnClick="btnConfirmOTP_Click" Text="OK" />
                                    <%--<button type="button" class="btn btn-default pull-right" data-dismiss="modal">di ako maka isip</button>--%>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>






    <%--//Modal for Co-Owner--%>
    <div class="modal fade" id="modal-coowner">
        <div class="modal-dialog">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span></button>
                                <h4 class="modal-title" runat="server" id="coothers">Co-Owner</h4>
                            </div>
                            <div class="modal-body">
                                <div class="form-group">
                                    <asp:TextBox runat="server" ID="tCoOwnerRow" class="form-control text-uppercase hidden" type="text" />
                                </div>
                                <div class="form-group">
                                    <label>Last Name <span class="color-red fsize-16">*</span></label>
                                    <asp:TextBox runat="server" ID="tOwnerLName" class="form-control text-uppercase" type="text" placeholder="Dela Cruz" />
                                    <asp:RequiredFieldValidator
                                        ID="RequiredFieldValidator5"
                                        ControlToValidate="tOwnerLName"
                                        Text="Please fill up Last Name."
                                        ValidationGroup="saveowner"
                                        runat="server" Style="color: red" />
                                </div>
                                <div class="form-group">
                                    <label>First Name <span class="color-red fsize-16">*</span></label>
                                    <asp:TextBox runat="server" ID="tOwnerFName" class="form-control text-uppercase" type="text" placeholder="Juan" />
                                    <asp:RequiredFieldValidator
                                        ID="RequiredFieldValidator29"
                                        ControlToValidate="tOwnerFName"
                                        Text="Please fill up First Name."
                                        ValidationGroup="saveowner"
                                        runat="server" Style="color: red" />
                                </div>
                                <div class="form-group">
                                    <label>Middle Name <span class="color-red fsize-16">*</span></label>
                                    <asp:TextBox runat="server" ID="tOwnerMName" class="form-control text-uppercase" type="text" placeholder="D" />
                                    <asp:RequiredFieldValidator
                                        ID="RequiredFieldValidator30"
                                        ControlToValidate="tOwnerMName"
                                        Text="Please fill up Middle Name."
                                        ValidationGroup="saveowner"
                                        runat="server" Style="color: red" />
                                </div>
                                <div class="form-group">
                                    <label>Relationship <span class="color-red fsize-16">*</span></label>
                                    <asp:TextBox runat="server" ID="tOwnerRelationship" class="form-control text-uppercase" type="text" placeholder="Son" />
                                    <asp:RequiredFieldValidator
                                        ID="RequiredFieldValidator31"
                                        ControlToValidate="tOwnerRelationship"
                                        Text="Please fill up Relationship."
                                        ValidationGroup="saveowner"
                                        runat="server" Style="color: red" />
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-danger pull-left" data-dismiss="modal">Close</button>
                                <button id="btnAddCoOwner" onserverclick="btnAddCoOwner_ServerClick" runat="server" type="button" validationgroup="saveowner" class="btn btn-success">Add Co-Owner</button>
                                <button id="Button4" onserverclick="btnAddCoborrower_ServerClick" runat="server" type="button" validationgroup="saveborrower" class="btn btn-success" visible="false">Update Co-Borrower</button>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
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
    <a href="#0" class="cd-top"></a>

    <%--Date year format checker--%>
    <script>
        $(document).on('change', '#modals_txtBirthday', function () {
            var getDate = $('#modals_txtBirthday').val().split('-');
            var yearLen = getDate[0].length;

            if (yearLen > 4) {
                console.log('1990-' + getDate[1] + '-' + getDate[2]);
                $('#modals_txtBirthday').val('2000-' + getDate[1] + '-' + getDate[2]);
            }
        });
    </script>

    <%--Remarks label color generator--%>
    <script>
        $(document).on('change', '#content_lblRemarks', function () {
            var getText = $(this);

            switch (getText.html()) {
                case "PASSED":
                    $('#content_lblRemarks').attr('style', 'color: green; margin-top: 0px; margin-bottom: 0px;');
                    break;
                case "FAILED":
                    $('#content_lblRemarks').attr('style', 'color: red; margin-top: 0px; margin-bottom: 0px;');
                    break;
            }
        });
    </script>
</asp:Content>

<%@ Page Title="DREAMS | Buyer's Info" Language="C#" MasterPageFile="~/master/Main.Master" AutoEventWireup="true" CodeBehind="Buyers.aspx.cs" Inherits="ABROWN_DREAMS.Buyers" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--Nav-Tab--%>
    <script type="text/javascript">
        $(window).load(function () {
            //$('.nav-tabs a[href="#TabSpouse"]').tab('show');
            //$('.nav-tabs a[href="#TabEmployment"]').tab('show');
            if ($('#emptablist').hasClass("active")) {
                $('.nav-tabs a[href="#TabEmployment"]').tab('show');
            }
            else if ($('#spousetablist').hasClass("active")) {
                $('.nav-tabs a[href="#TabSpouse"]').tab('show');
            }
            else if ($('#spatablist').hasClass("active")) {
                $('.nav-tabs a[href="#TabSPA"]').tab('show');
            }
            else if ($('#otherstablist').hasClass("active")) {
                $('.nav-tabs a[href="#TabOthers"]').tab('show');
            }
        });
<%--        $(document).ready(function () {
            $('#<%=btnAddtolist.ClientID%>').click(function () {
                ShowCoownerlist();
            });
        });--%>


        //ADD DATA TO TIN
        function insertText() {

            console.log('test');
            if (document.getElementById("tTypeOfId").value = "TIN") {
                document.getElementById("tIDNo").value = document.getElementById("tTin").value
            }
            if (document.getElementById("tTypeOfId2").value = "TIN") {
                document.getElementById("tIDNo2 ").value = document.getElementById("tTin").value
            }
        }







    </script>

    <%-- Refresh --%>
    <%--<script type="text/javascript">
        window.onbeforeunload = function () {
            return "Changes you made may not be saved.";
        }

        document.onkeydown = function () {
            if (event.keyCode == 116) {
                event.returnValue = false;
                event.keyCode = 0;
                return false;
            }
        };
    </script>--%>

    <%-- Clear All Textbox --%>
    <script type="text/javascript">
        //Prevents Tab UI bug upon loading
        function EmpTab() {
            //$('.nav-tabs a[href="#TabSpouse"]').tab('show');
            //$('.nav-tabs a[href="#TabEmployment"]').tab('show');
            if ($('#emptablist').hasClass("active")) {
                $('.nav-tabs a[href="#TabSpouse"]').tab('show');
                $('.nav-tabs a[href="#TabEmployment"]').tab('show');
            }
            else if ($('#spousetablist').hasClass("active")) {
                $('.nav-tabs a[href="#TabSpouse"]').tab('show');
                $('.nav-tabs a[href="#TabSpouse"]').tab('show');
            }
            else if ($('#spatablist').hasClass("active")) {
                $('.nav-tabs a[href="#TabSpouse"]').tab('show');
                $('.nav-tabs a[href="#TabSPA"]').tab('show');
            }
            else if ($('#otherstablist').hasClass("active")) {
                $('.nav-tabs a[href="#TabSpouse"]').tab('show');
                $('.nav-tabs a[href="#TabOthers"]').tab('show');
            }
        }
        function hideMsgAccount() {
            $('#MsgAccount').modal('hide');
        }
        function ClearAll() {
            $('#form_buyer').find('input[type=text]').each(function () {
                $(this).val('');
            });

            $('#form_buyer').find('input[type=email]').each(function () {
                $(this).val('');
            });
            $('#form_buyer').find('input[type=number]').each(function () {
                $(this).val('0');
            });
            $('#form_buyer').find('input[type=date]').each(function () {
                $(this).val('');
            });
            $('#form_buyer').find('select').each(function () {
                $(this).val('').change();
            });
            $('#form_buyer').find('input[type=checkbox]').each(function () {
                $(this).attr('checked', false);
            });
        }
        function DisableAll() {
            $('#form_buyer').find('input[type=text]').each(function () {
                $(this).prop("disabled", true);
            });

            $('#form_buyer').find('input[type=email]').each(function () {
                $(this).prop("disabled", true);
            });
            $('#form_buyer').find('input[type=number]').each(function () {
                $(this).prop("disabled", true);
            });
            $('#form_buyer').find('input[type=date]').each(function () {
                $(this).prop("disabled", true);
            });
            $('#form_buyer').find('select').each(function () {
                $(this).prop("disabled", true);
            });
            $('#form_buyer').find('input[type=checkbox]').each(function () {
                $(this).prop("disabled", true);
            });
        }
        function EnableAll() {
            $('#form_buyer').find('input[type=text]').each(function () {
                $(this).prop("disabled", false);
            });

            $('#form_buyer').find('input[type=email]').each(function () {
                $(this).prop("disabled", false);
            });
            $('#form_buyer').find('input[type=number]').each(function () {
                $(this).prop("disabled", false);
            });
            $('#form_buyer').find('input[type=date]').each(function () {
                $(this).prop("disabled", false);
            });
            $('#form_buyer').find('select').each(function () {
                $(this).prop("disabled", false);
            });
            $('#form_buyer').find('input[type=checkbox]').each(function () {
                $(this).prop("disabled", false);
            });
        }
        function ShowSuccessAlert() {
            $('#modalSuccess').modal('show');
        }
    </script>

    <!-- ############ For tabs ############### -->
    <script type="text/javascript">
        //function fixTab() {
        //    var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "referenceBtn";
        //    $('#myTab1 a[href="#' + tabName + '"]').tab('show');
        //    $("#myTab1 a").click(function () {
        //        $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
        //    });
        //    var navName = $('#myTab1 a[href="#' + tabName + '"]').attr("data-target");
        //    if (navName != undefined) {
        //        tabNav(event, navName);
        //    }
        //}
        function fixTab() {
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "#tab_1";
            $('#myTab1 a[href="' + tabName + '"]').tab('show');
            $("#myTab1 a").click(function () {
                $("[id*=TabName]").val($(this).attr("href"));
            });
        }
        function setCurrentTab(evt) {
            //var href = evt.target.id;
            const anchor = evt.target.closest("a");   // Find closest Anchor (or self)
            if (!anchor) return;                        // Not found. Exit here.
            //console.log(anchor.getAttribute('href'));
            $("[id*=TabName]").val(anchor.getAttribute('href'));
            console.log('it worked');
        }

        $(document).ready(function () {
            $('#<%= tTIN.ClientID %>').mask('000-000-000-000');
            $('#<%= tTINCorp.ClientID %>').mask('000-000-000-000');

            //2023-06-16 : TIN FORMAT FOR SPOUSE
            $('#<%= tSpouseTIN2.ClientID %>').mask('000-000-000-000');
        });


        $(document).ready(function () {
            var tab = document.getElementById('<%= hidTAB.ClientID%>').value;
            $('#myTab1 a[href="' + tab + '"]').tab('show');
        });

    </script>

    <%-- Valid Values --%>
    <script type="text/javascript">
        function formValid() {
            $('#form_buyer').find('input[required]').each(function () {
                if ($(this).val() == '') {
                    $('#isValid').val('false');
                    $(this).addClass("required-input");
                    return false;
                }
                else {
                    $('#isValid').val('true');
                }
            });

            $('#form_buyer').find('input[required]:enabled').each(function () {
                if ($(this).val() == '') {
                    $(this).addClass("required-input");

                    //loop all input required with null values
                }
                else {
                    $(this).removeClass("required-input");
                }
            });

            //remove required class in disabled input
            $('#form_buyer').find('input[required]:disabled').each(function () {
                $(this).removeClass("required-input");
            });
        }

        function tabNav(evt, navName) {
            console.log((navName));
            $("[id*=TabName]").val(navName);

            var i, tabcontent, tablinks, name;
            navName = navName != "" ? navName : "tab_1";
            name = document.getElementById(navName);
            tabcontent = document.getElementsByClassName("tabcontent");
            for (i = 0; i < tabcontent.length; i++) {
                tabcontent[i].style.display = "none";
            }
            tablinks = document.getElementsByClassName("tablinks");
            for (i = 0; i < tablinks.length; i++) {
                tablinks[i].className = tablinks[i].className.replace(" active", "");
            }
            name.style.display = "block";
            evt.currentTarget.className += " active";
        }
    </script>

    <%-- Autcompute --%>
    <%--<script type="text/javascript">
        $(document).ready(function () {
            var total = 0;
            $('.BasicSalary').keyup(function () {
                $('.BasicSalary').each(function () {
                    var txtBoxVal = $(this).val();
                    total = total + Number(txtBoxVal);
                });
                $('.tTBasicSalary').val(total);
                total = 0;
            });
        });

        $(document).ready(function () {
            var total = 0;
            $('.Allowances').keyup(function () {
                $('.Allowances').each(function () {
                    var txtBoxVal = $(this).val();
                    total = total + Number(txtBoxVal);
                });
                $('.tTAllowances').val(total);
                total = 0;
            });
        });

        $(document).ready(function () {
            var total = 0;
            $('.Commissions').keyup(function () {
                $('.Commissions').each(function () {
                    var txtBoxVal = $(this).val();
                    total = total + Number(txtBoxVal);
                });
                $('.tTCommissions').val(total);
                total = 0;
            });
        });

        $(document).ready(function () {
            var total = 0;
            $('.RentalIncome').keyup(function () {
                $('.RentalIncome').each(function () {
                    var txtBoxVal = $(this).val();
                    total = total + Number(txtBoxVal);
                });
                $('.tTRentalIncome').val(total);
                total = 0;
            });
        });

        $(document).ready(function () {
            var total = 0;
            $('.Retainer').keyup(function () {
                $('.Retainer').each(function () {
                    var txtBoxVal = $(this).val();
                    total = total + Number(txtBoxVal);
                });
                $('.tTRetainer').val(total);
                total = 0;
            });
        });

        $(document).ready(function () {
            var total = 0;
            $('.MIOthers').keyup(function () {
                $('.MIOthers').each(function () {
                    var txtBoxVal = $(this).val();
                    total = total + Number(txtBoxVal);
                });
                $('.tTOthers').val(total);
                total = 0;
            });
        });

        $(document).ready(function () {
            var total = 0;
            $('.STotal').keyup(function () {
                $('.STotal').each(function () {
                    var txtBoxVal = $(this).val();
                    total = total + Number(txtBoxVal);
                });
                $('.tSTotal').val(total);
                total = 0;
                calculateSum();
            });
        });

        $(document).ready(function () {
            var total = 0;
            $('.PTotal').keyup(function () {
                $('.PTotal').each(function () {
                    var txtBoxVal = $(this).val();
                    total = total + Number(txtBoxVal);
                });
                $('.tPTotal').val(total);
                total = 0;
                calculateSum();
            });
        });

        function calculateSum() {
            var total = 0;
            $('.MITotal').each(function () {
                var txtBoxVal = $(this).val();
                total = total + Number(txtBoxVal);
            });
            $('.tMITotal').val(total);

            $('.tRequiredMonthly').val(total * '0.35');

            total = 0;
        }

        $(document).ready(function () {
            var total = 0;
            $('.METotal').keyup(function () {
                $('.METotal').each(function () {
                    var txtBoxVal = $(this).val();
                    total = total + Number(txtBoxVal);
                });
                $('.tMETotal').val(total);
                total = 0;
            });
        });

    </script>--%>

    <%-- Modals --%>
    <script type="text/javascript">
        function MsgDependent_Hide() {
            $('#MsgDependent').modal('hide');
        };
        function MsgCoowner_Hide() {
            $('#MsgCoowner').modal('hide');
        };
        function MsgEmployment_Hide() {
            $('#MsgEmployment').modal('hide');
        };
        function MsgSPACoBorrower_Hide() {
            $('#MsgSPACoBorrower').modal('hide');
        };
        function MsgSPACoBorrower_Show() {
            $('#MsgSPACoBorrower').modal('show');
        };
        function MsgBankAccount_Hide() {
            $('#MsgBankAccount').modal('hide');
        };
        function MsgCharacterRef_Hide() {
            $('#MsgCharacterRef').modal('hide');
        };
        function MsgBuyers_Hide() {
            $('#MsgBuyers').modal('hide');
            EnableAll()
        };
        function MsgBuyers_HideIT() {
            $('#MsgBuyers').modal('hide');
            DisableAll()
        };
        function modalConfirmation_Hide() {
            $('#modalConfirmation').modal('hide');
            MsgSPACoBorrower_Hide();
        };
        function showApprove() {
            $('#MsgApprove').modal('show');
        };
        function hideApprove() {
            $('#MsgApprove').modal('hide');
            EnableAll()
        };
        function hideApproveIT() {
            $('#MsgApprove').modal('hide');
            DisableAll()
        };
        function ShowAlert() {
            $('#modalAlert').modal('show');
        };
        function HideAlert() {
            $('#modalAlert').modal('hide');
        };
        function ShowLinkBuyerModal() {
            $('#MsgSpecialBuyers').modal('show');
        }
        function HideLinkBuyerModal() {
            $('#MsgSpecialBuyers').modal('hide');
        }
        function ShowCoownerlist() {
            $('#Msgcoownerlist').modal('show');
        }
        function HideCoownerlist() {
            $('#Msgcoownerlist').modal('hide');
        }


        //2023-06-16 : UPDATING OF DEPENDENT RELATED
        function ShowAddDependent() {
            $('#MsgDependent').modal('show');
        }
        function HideAddDependent() {
            $('#MsgDependent').modal('hide');
        }

        //2023-06-17 : UPDATING OF BANK 
        function ShowAddBank() {
            $('#MsgBankAccount').modal('show');
        }
        function HideAddBank() {
            $('#MsgBankAccount').modal('hide');
        }

        //2023-06-17  : UPDATING OF CHARACTER REF
        function ShowCharacterReference() {
            $('#MsgCharacterRef').modal('show');
        }
        function HideCharacterReference() {
            $('#MsgCharacterRef').modal('hide');
        }

    </script>

    <!-- ############ Number Only ############### -->
    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>
    <script type="text/javascript">
        function fnAllowNumeric(evt) {
            // Allow numeric characters and dashes
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            var charStr = String.fromCharCode(charCode);
            if ((charCode < 48 || charCode > 57) && charCode != 8 && charStr != "-")
                return false;
            return true;
        }

        //2023-06-15
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
            document.getElementById("tSpouseTIN2").focus();
        }



        function alpha(e) {
            var k;
            document.all ? k = e.keyCode : k = e.which;
            return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || (k >= 48 && k <= 57));
        }
        <%--function ValidTIN_keyup(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            var elemid;
            if (charCode == 8 || charCode == 46) { }
            else {
                var istin = false;
                var elemidno = evt.target.id;
                if (elemidno == "content_tIDNo") {
                    elemid = document.getElementById("<%=tTypeOfId.ClientID%>");
                    if (elemid.value == "ID1") {
                        var istin = true;
                    }
                }
                if (elemidno == "content_tIDNo2") {
                    elemid = document.getElementById("<%=tTypeOfId2.ClientID%>");
                    if (elemid.value == "ID1") {
                        var istin = true;
                    }
                }
                if (elemidno == "content_tIDNo3") {
                    elemid = document.getElementById("<%=tTypeOfId3.ClientID%>");
                    if (elemid.value == "ID1") {
                        var istin = true;
                    }
                }
                if (elemidno == "content_tIDNo4") {
                    elemid = document.getElementById("<%=tTypeOfId4.ClientID%>");
                    if (elemid.value == "ID1") {
                        var istin = true;
                    }
                }
                var elem = document.getElementById(evt.target.id);
                if (istin) {
                    $("#" + evt.target.id).attr('maxlength', '15');
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
                else {
                    $("#" + evt.target.id).removeAttr("maxlength");
                }
            }
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
        }--%>
    </script>

    <style>
        .stepwizard-step p {
            margin-top: 0px;
            color: #666;
        }

        .stepwizard-row {
            display: table-row;
        }

        .stepwizard {
            display: table;
            width: 100%;
            position: relative;
        }

        .stepwizard-step button[disabled] {
            /*opacity: 1 !important;
          filter: alpha(opacity=100) !important;*/
        }

        .stepwizard .btn.disabled,
        .stepwizard .btn[disabled],
        .stepwizard fieldset[disabled] .btn {
            opacity: 1 !important;
            color: #bbb;
        }

        .stepwizard-row:before {
            top: 14px;
            bottom: 0;
            position: absolute;
            content: " ";
            width: 100%;
            height: 1px;
            background-color: #ccc;
            z-index: 0;
        }

        .stepwizard-step {
            display: table-cell;
            text-align: center;
            position: relative;
        }

        .btn-circle {
            width: 30px;
            height: 30px;
            text-align: center;
            padding: 6px 0;
            font-size: 12px;
            line-height: 1.428571429;
            border-radius: 15px;
        }

        /*        .tabcontent {
            display: none;
            border-top: none;
        }*/

        .titlemargin {
            margin: 0px 10px 0px 10px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="sitemap" runat="server" style="overflow-x: hidden;">
    <li><a href="#"><span>Buyer's Information</span></a></li>
</asp:Content>




<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server" style="overflow-x: hidden;">
    <!-- Horizontal Steppers -->
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12">
                <!-- Stepers Wrapper -->
                <div class="stepwizard">

                    <div class="stepwizard-row setup-panel">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <div class="stepwizard-step col-xs-6">
                                    <a runat="server" id="step1" href="#step-1" type="button" class="btn btn-default btn-circle">1</a>
                                    <p><small>Overview</small></p>
                                </div>
                                <div class="stepwizard-step col-xs-6">
                                    <a runat="server" id="step2" href="#step-2" type="button" class="btn btn-success btn-circle">2</a>
                                    <p><small>Buyer Details</small></p>
                                </div>
                                <%--  <div class="stepwizard-step col-xs-4">
                                    <a runat="server" id="step3" href="#step-3" type="button" class="btn btn-default btn-circle">3</a>
                                    <p><small>Financials</small></p>
                                </div>--%>
                                <%-- <div class="stepwizard-step col-xs-3">
                                 <a runat="server" id="step4" href="#step-4" type="button" class="btn btn-default btn-circle">4</a>
                                 <p><small>Attachments</small></p>
                                 </div>--%>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                </div>
            </div>
        </div>
    </div>

    <div class="container-fluid">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:Panel class="panel panel-primary" runat="server" ID="step_2" Visible="true">
                    <div class="panel-heading" style="background-color: #5caceb">
                        <div class="row">
                            <div class="col-md-7 col-sm-6 col-xs-12">
                                <h3 class="panel-title">Buyer Details</h3>
                            </div>
                            <div class="col-md-5 col-sm-6 col-xs-12">
                                <div class="trans-command">
                                    <div class="btn-group" role="group">
                                        <asp:UpdatePanel runat="server" ID="UpdatePanelButton">
                                            <ContentTemplate>
                                                <asp:LinkButton runat="server" CssClass="btn btn-success btn-width" ID="btnPrint" OnClick="btnPrint_Click">
                                                    <label runat="server" id="Label1" style="height: 5px; font-weight: normal;">Print</label>
                                                    <i class="glyphicon glyphicon-print"></i>
                                                </asp:LinkButton>
                                                <asp:LinkButton runat="server" CssClass="btn btn-success btn-width" ID="btnApprove" OnClick="btnShowApprove_ServerClick" Visible="false">
                                                    <label runat="server" id="lblApprove" style="height: 5px; font-weight: normal;">Approve</label>
                                                    <i class="glyphicon glyphicon-check"></i>
                                                </asp:LinkButton>
                                                <button runat="server" id="btnFind" class="btn btn-info btn-width" type="button" data-toggle="modal" data-target="#MsgBuyers" onserverclick="btnFind_ServerClick">Find <i class="glyphicon glyphicon-search"></i></button>
                                                <button class="btn btn-danger btn-width" type="button" runat="server" id="btnCancel" onserverclick="btnCancel_ServerClick">Delete <i class="fa fa-trash-o"></i></button>
                                                <asp:LinkButton runat="server" Visible="false" CssClass="btn btn-info btn-width" ID="btnUpdatetoSAP" OnClick="btnUpdatetoSAP_Click">
                                                    <label runat="server" id="Label2" style="height: 5px; font-weight: normal;">Update BP</label>
                                                </asp:LinkButton>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    </br>
                                        <%-- ###############################  CUSTOMER CODE ###############################--%>

                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div style="background-color: #D2D6DE; height: 3px;"></div>
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <h4>Please fill in the information below.</h4>
                                <h4>Fields marked with <span class="color-red fsize-16">*</span>
                                are mandatory                                
                            </div>
                            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="col-lg-12" runat="server" id="divAprv" visible="false">
                                        <div class="col-lg-9">&nbsp;</div>
                                        <div class="col-lg-3 panel-heading" style="background-color: #5caceb">
                                            <h3 class="panel-title" style="color: white; margin: 0 auto; text-align: center;">Proof of Approval</h3>
                                        </div>
                                        <div class="col-lg-9">&nbsp;</div>
                                        <div class="col-lg-3 panel panel-success" style="margin: 0 auto; text-align: center;" runat="server">
                                            <br />
                                            <label>Status: APPROVED</label>
                                            <%--<label style="color: white; background-color: #5caceb; border-radius: 10%">Proof of Approval</label>--%>
                                            <div runat="server" id="divAttch">
                                                <div class="col-lg-12 center-block center-block">
                                                    <asp:Label runat="server" ID="lblAprvAttachment" Style="overflow-wrap: break-word;" Text=""></asp:Label>
                                                    <%--<asp:FileUpload ID="FileUpload4" CssClass="btn btn-default pull-right" CommandName="FileUpload1" runat="server" EnableViewState="true" />--%>
                                                </div>
                                                <div class="col-lg-12 center-block center-block" style="margin-bottom: 5px;">
                                                    <%--<asp:LinkButton ID="LinkButton3" CssClass="btn btn-info" Visible="true" runat="server" Text="Preview" />--%>
                                                    <asp:LinkButton ID="btnPreviewAprv" OnClick="btnPreview2_Click" CssClass="btn btn-info" Visible="true" runat="server" Text="Preview" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnPreviewAprv" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <br />
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <div class="col-lg-6 col-md-12 col-sm-12 col-xs-12" runat="server" id="divCustomerCode">
                                    <h4 class="trans-title">Customer Code: 
                                    <asp:Label runat="server" ID="lblCustomerCode" CssClass="strong" Text=""></asp:Label>
                                    </h4>
                                </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <%-- ###############################  TAG THE SALES PERSON  ###############################--%>
                    </br>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 hidden">
                                <div class="col-lg-6 col-md-12 col-sm-12 col-xs-12 col-sm-offset-3">
                                    <div class="panel panel-info">
                                        <div class="panel-heading">
                                            <h3 class="panel-title text-center" style="text-align: center;">SALES AGENT</h3>
                                        </div>
                                        <div class="panel-body text-center">
                                            <div class="row text-center">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-center ">
                                                    <div class="input-group ">
                                                        <input type="text" style="z-index: auto;" placeholder="SALES AGENT" class="form-control text-uppercase" runat="server" id="txtSalesAgent" disabled required /><span class="input-group-btn">
                                                            <button id="bSalesAgent" runat="server" style="width: 50px; z-index: auto;" data-toggle="modal" data-target="#MsgEmployment" class="btn btn-secondary btn-dropbox" type="button" onserverclick="btnEmployment_ServerClick"><i class="fa fa-bars"></i></button>
                                                        </span>
                                                    </div>
                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <div class="input-group col-sm-offset-3">
                                                                <asp:Label runat="server" ID="lblFileName" Text=""></asp:Label>
                                                                <asp:FileUpload ID="FileUpload1" CssClass="btn btn-default" CommandName="FileUpload1" runat="server" EnableViewState="true" />
                                                                <asp:LinkButton ID="btnUpload" CssClass="btn btn-primary" OnClick="btnUpload_Click" runat="server" Text="Upload" />
                                                                <asp:LinkButton ID="btnPreview" CssClass="btn btn-info" OnClick="btnPreview_Click" runat="server" Text="Preview" />
                                                                <asp:LinkButton ID="btnRemove" CssClass="btn btn-danger" OnClick="btnRemove_Click" runat="server" Text="Remove" />
                                                            </div>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="btnUpload" />
                                                            <asp:PostBackTrigger ControlID="btnPreview" />
                                                            <asp:PostBackTrigger ControlID="btnRemove" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <%--<hr />--%>

                    <div class="panel-body">
                        <div class="box box-default">
                            <div class="box-header with-border">
                                <h3 class="box-title">General Information</h3>
                            </div>
                            <div class="box-body">

                                <div class="row" id="Div1" runat="server" visible="true">

                                    <div class="col-md-4" runat="server" id="divbuyertype">
                                        <div class="form-group">
                                            <label>Buyer Type</label>
                                            <asp:DropDownList runat="server" ID="ddlBusinessType" CssClass="form-control"
                                                AutoPostBack="true" Style="" OnSelectedIndexChanged="ddlBusinessType_SelectedIndexChanged">
                                                <asp:ListItem Text="Individual" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Corporation"></asp:ListItem>
                                                <%--<asp:ListItem Text="Co-ownership"></asp:ListItem>--%>
                                                <asp:ListItem Text="Guardianship"></asp:ListItem>
                                                <asp:ListItem Text="Trusteeship"></asp:ListItem>
                                                <asp:ListItem Text="Others"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3" runat="server" id="divTrustGuard" visible="false">
                                        <div class="form-group">
                                            <label>Role</label>
                                            <div class="form-control">
                                                <asp:RadioButton AutoPostBack="true" Text="Guardian" runat="server" type="radio" OnCheckedChanged="rbGuardian_CheckedChanged" GroupName="trustguard" value="false" ID="rbGuardian" />
                                                <asp:RadioButton AutoPostBack="true" CssClass="pull-right" Text="Guardee" OnCheckedChanged="rbGuardian_CheckedChanged" runat="server" type="radio" GroupName="trustguard" value="true" ID="rbGuardee" />
                                            </div>
                                            <asp:CustomValidator
                                                Style="color: red"
                                                runat="server"
                                                ID="CustomValidator9"
                                                ValidationGroup="Save"
                                                Text="Please choose role."
                                                OnServerValidate="CustomValidator9_ServerValidate" />
                                        </div>
                                    </div>
                                    <div class="col-md-3" runat="server" id="sBuyerType" visible="false">
                                        <div class="form-group">
                                            <label>Specify Buyer Type <span class="color-red fsize-16">*</span></label>
                                            <asp:TextBox ID="txtSpecifyBusiness" runat="server" class="form-control text-uppercase" type="text" placeholder="Others" />
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator51"
                                                ControlToValidate="txtSpecifyBusiness"
                                                Text="Please specify buyer type."
                                                ValidationGroup="Save"
                                                runat="server" Style="color: red" />
                                        </div>
                                    </div>
                                    <div class="col-md-3" runat="server" id="divspecbuyers" visible="false">
                                        <label runat="server" id="lblspecbuyers">Link #:</label>
                                        <div class="input-group">
                                            <input runat="server" style="z-index: auto;" id="txtspecbuyer" class="form-control text-uppercase" type="text" disabled /><span class="input-group-btn">
                                                <button id="btnSpecialBuyers" runat="server" style="width: 50px; z-index: auto;" class="btn btn-primary btn-dropbox" type="button" onserverclick="btnSpecialBuyers_ServerClick"><i class="fa fa-bars"></i></button>
                                            </span>
                                        </div>
                                    </div>

                                    <div class="col-md-4" runat="server" id="divtaxclass">
                                        <div class="form-group">
                                            <label>Tax Classification</label>
                                            <asp:DropDownList runat="server" ID="ddTaxClass" OnSelectedIndexChanged="ddTaxClass_SelectedIndexChanged" DataTextField="Name" DataValueField="Code" CssClass="form-control"
                                                AutoPostBack="true" Style="">
                                                <asp:ListItem Text="Engaged in Business" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Not engaged in Business"></asp:ListItem>
                                                <%--<asp:ListItem Text="Regular Individual"></asp:ListItem>--%>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="col-md-4" hidden>
                                        <div class="form-group">
                                            <label>Co-maker</label>
                                            <input runat="server" type="text" class="form-control text-uppercase" id="txtComaker" placeholder="Applicant Co-maker" required />
                                        </div>
                                    </div>
                                </div>

                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <div class="row" id="divIndividual" runat="server" visible="true">

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Last Name <span class="color-red fsize-16">*</span></label>
                                                    <%--<input runat="server" type="text" class="form-control text-uppercase" id="tLastName" placeholder="Applicant Last Name" required />--%>
                                                    <asp:TextBox runat="server" OnTextChanged="txtName_TextChanged" type="text" class="form-control text-uppercase" ID="tLastName" placeholder="Applicant Last Name" required />
                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator1"
                                                        ControlToValidate="tLastName"
                                                        Text="Please fill up Last Name."
                                                        ErrorMessage="Please fill up Spouse TIN."
                                                        SetFocusOnError="true"
                                                        ValidationGroup="Save"
                                                        runat="server" Style="color: red" />
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>First Name <span class="color-red fsize-16">*</span></label>
                                                    <%--<input runat="server" type="text" class="form-control text-uppercase" id="tFirstName" placeholder="Applicant First Name" required />--%>
                                                    <asp:TextBox runat="server" OnTextChanged="txtName_TextChanged" type="text" class="form-control text-uppercase" ID="tFirstName" placeholder="Applicant First Name" required />
                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator2"
                                                        ControlToValidate="tFirstName"
                                                        Text="Please fill up First Name."
                                                        ErrorMessage="Please fill up First Name."
                                                        SetFocusOnError="true"
                                                        ValidationGroup="Save"
                                                        runat="server" Style="color: red" />
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Middle Name <span class="color-red fsize-16">*</span></label>
                                                    <%--<input runat="server" type="text" class="form-control text-uppercase" id="tMiddleName" placeholder="Applicant Middle Name" required />--%>
                                                    <asp:TextBox runat="server" OnTextChanged="txtName_TextChanged" type="text" class="form-control text-uppercase" ID="tMiddleName" placeholder="Applicant Middle Name" required />
                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator3"
                                                        ControlToValidate="tMiddleName"
                                                        Text="Please fill up Middle Name."
                                                        ErrorMessage="Please fill up Middle Name."
                                                        SetFocusOnError="true"
                                                        ValidationGroup="Save"
                                                        runat="server" Style="color: red" />
                                                </div>
                                            </div>

                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                                <div class="row" id="divCompanyName" runat="server" visible="false">
                                    <div class="col-md-9">
                                        <div class="form-group">
                                            <label>Corporate Name <span class="color-red fsize-16">*</span></label>
                                            <%--<asp:TextBox runat="server" AutoPostBack="true" OnTextChanged="txtCompanyName_TextChanged" class="form-control text-uppercase" ID="TextBox1" placeholder="Company Name" required></asp:TextBox>--%>
                                            <asp:TextBox runat="server" class="form-control text-uppercase" ID="txtCompanyName" placeholder="Company Name" required></asp:TextBox>
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator8"
                                                ControlToValidate="txtCompanyName"
                                                Text="Please fill up Company Name."
                                                ValidationGroup="Save"
                                                runat="server" Style="color: red" />
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>TIN No. <span class="color-red fsize-16">*</span></label>
                                            <%--<input runat="server" type="text" class="form-control" id="tTIN" onkeyup="TIN_keyup(event);" onkeypress="return fnAllowNumeric(event)" placeholder="xxx-xxx-xxx-xxx" maxlength="15" />--%>
                                            <%--<asp:TextBox runat="server" AutoPostBack="true" type="text" class="form-control" OnTextChanged="tTINCorp_TextChanged" ID="tTINCorp" onkeypress="return fnAllowNumeric(event)" placeholder="xxx-xxx-xxx-xxx" MaxLength="15"></asp:TextBox>--%>
                                            <%--<asp:TextBox runat="server" AutoPostBack="true" type="text" class="form-control" OnTextChanged="tTINCorp_TextChanged" ID="TextBox1" onkeypress="return fnAllowNumeric(event)" placeholder="xxx-xxx-xxx-xxx" MaxLength="15" ValidationGroup="Save"></asp:TextBox>--%>
                                            <asp:TextBox runat="server" ID="tTINCorp" type="text" class="form-control" onkeypress="return fnAllowNumeric(event)" placeholder="xxx-xxx-xxx-xxx" MaxLength="15" ValidationGroup="Save"></asp:TextBox>
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator41"
                                                ControlToValidate="tTINCorp"
                                                Text="Please fill up TIN."
                                                ValidationGroup="Save"
                                                runat="server" Style="color: red" />
                                            <asp:CustomValidator
                                                Style="color: red"
                                                runat="server"
                                                ID="CustomValidator7"
                                                ValidationGroup="Save"
                                                ControlToValidate="tTINCorp"
                                                Text="Incorrect TIN format. Must be xxx-xxx-xxx-xxx."
                                                OnServerValidate="CustomValidator7_ServerValidate" />

                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label>SEC COR ID Number <span class="color-red fsize-16">*</span></label>
                                            <%--<asp:TextBox ID="TextBox1" AutoPostBack="true" runat="server" onkeypress="return alpha(event)" class="form-control" type="text" placeholder="Your SEC COR ID Number" />--%>
                                            <asp:TextBox ID="txtSECCORIDNo" runat="server" onkeypress="return alpha(event)" class="form-control" type="text" placeholder="Your SEC COR ID Number" />
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator63"
                                                ControlToValidate="txtSECCORIDNo"
                                                Text="Please fill up SEC COR ID Number."
                                                ValidationGroup="Save"
                                                runat="server" Style="color: red" />
                                        </div>
                                    </div>
                                </div>


                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <hr />

                                <div class="col-md-12">
                                    <div class="box box-danger">
                                        <div style="background-color: #DD4B39; height: 3px;"></div>
                                        <div class="box-header titlemargin">
                                            <h3 class="box-title">Address and Business Information</h3>
                                        </div>



                                        <div class="box-body">
                                            <h4 class="col-md-12"><b runat="server" id="addtitle">Complete Present Address</b></h4>


                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label>No. <span class="color-red fsize-16">*</span></label>
                                                    <%--<asp:TextBox runat="server" type="text" AutoPostBack="true" OnTextChanged="txtAddress_TextChanged"  class="form-control text-uppercase" ID="TextBox1" placeholder="No." />--%>
                                                    <asp:TextBox runat="server" type="text" class="form-control text-uppercase" ID="tPresentAddress" placeholder="No." />
                                                    <%--<input runat="server" type="text" class="form-control text-uppercase" id="tPresentAddress" placeholder="No." />--%>
                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator42"
                                                        ControlToValidate="tPresentAddress"
                                                        Text="Please fill up Present Address No."
                                                        ValidationGroup="Save"
                                                        runat="server" Style="color: red" />
                                                </div>
                                            </div>


                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label>Street <span class="color-red fsize-16">*</span></label>
                                                    <asp:TextBox runat="server" type="text" class="form-control text-uppercase" ID="txtPresentStreet" placeholder="Street" />
                                                    <%--<input runat="server" type="text" class="form-control text-uppercase" id="txtPresentStreet" placeholder="Street" />--%>
                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator43"
                                                        ControlToValidate="txtPresentStreet"
                                                        Text="Please fill up Present Street."
                                                        ValidationGroup="Save"
                                                        runat="server" Style="color: red" />
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label>Subdivision <span class="color-red fsize-16">*</span></label>
                                                    <asp:TextBox runat="server" type="text" class="form-control text-uppercase" ID="txtPresentSubdivision" placeholder="Subdivision" />
                                                    <%--<input runat="server" type="text" class="form-control text-uppercase" id="txtPresentSubdivision" placeholder="Subdivision" />--%>
                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator44"
                                                        ControlToValidate="txtPresentSubdivision"
                                                        Text="Please fill up Present Subdivision."
                                                        ValidationGroup="Save"
                                                        runat="server" Style="color: red" />
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label>Barangay/District <span class="color-red fsize-16">*</span></label>
                                                    <asp:TextBox runat="server" type="text" class="form-control text-uppercase" ID="txtPresentBarangay" placeholder="Barangay/District" />
                                                    <%--<input runat="server" type="text" class="form-control text-uppercase" id="txtPresentBarangay" placeholder="Barangay/District" />--%>
                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator45"
                                                        ControlToValidate="txtPresentBarangay"
                                                        Text="Please fill up Present Barangay/District."
                                                        ValidationGroup="Save"
                                                        runat="server" Style="color: red" />
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label>Municipality/City <span class="color-red fsize-16">*</span></label>
                                                    <asp:TextBox runat="server" type="text" class="form-control text-uppercase" ID="txtPresentCity" placeholder="Municipality/City" />
                                                    <%--<input runat="server" type="text" class="form-control text-uppercase" id="txtPresentCity" placeholder="Municipality/City" />--%>
                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator46"
                                                        ControlToValidate="txtPresentCity"
                                                        Text="Please fill up Present Municipality/City."
                                                        ValidationGroup="Save"
                                                        runat="server" Style="color: red" />
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label>Province <span class="color-red fsize-16">*</span></label>
                                                    <asp:TextBox runat="server" type="text" class="form-control text-uppercase" ID="txtPresentProvince" placeholder="Address" />
                                                    <%--<input runat="server" type="text" class="form-control text-uppercase" id="txtPresentProvince" placeholder="Address" />--%>
                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator47"
                                                        ControlToValidate="txtPresentProvince"
                                                        Text="Please fill up Present Province."
                                                        ValidationGroup="Save"
                                                        runat="server" Style="color: red" />
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Present Postal Code <span class="color-red fsize-16">*</span></label>
                                                    <%--<asp:TextBox ID="TextBox1" runat="server" AutoPostBack="true" OnTextChanged="txtAddress_TextChanged" class="form-control" onkeypress="return alpha(event)" type="text" placeholder="000" />--%>
                                                    <asp:TextBox ID="txtPresPostal" runat="server" class="form-control" onkeypress="return alpha(event)" type="text" placeholder="000" />
                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator48"
                                                        ControlToValidate="txtPresPostal"
                                                        Text="Please fill up Present Postal Code."
                                                        ValidationGroup="Save"
                                                        runat="server" Style="color: red" />
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Present Country</label>
                                                    <%-- <asp:DropDownList ID="DropDownList1" OnSelectedIndexChanged="ddPreCountry_SelectedIndexChanged" runat="server" CssClass="form-control text-uppercase">--%>
                                                    <asp:DropDownList ID="ddPreCountry" runat="server" CssClass="form-control text-uppercase">
                                                        <asp:ListItem Selected="true" Value="PH">Philippines</asp:ListItem>
                                                        <asp:ListItem Value="1">--- SELECT COUNTRY ---</asp:ListItem>
                                                        <asp:ListItem Value="AF">Afghanistan</asp:ListItem>
                                                        <asp:ListItem Value="AX">Aland Islands</asp:ListItem>
                                                        <asp:ListItem Value="AL">Albania</asp:ListItem>
                                                        <asp:ListItem Value="DZ">Algeria</asp:ListItem>
                                                        <asp:ListItem Value="AS">American Samoa</asp:ListItem>
                                                        <asp:ListItem Value="AD">Andorra</asp:ListItem>
                                                        <asp:ListItem Value="AO">Angola</asp:ListItem>
                                                        <asp:ListItem Value="AI">Anguilla</asp:ListItem>
                                                        <asp:ListItem Value="AQ">Antarctica</asp:ListItem>
                                                        <asp:ListItem Value="AG">Antigua and Barbuda</asp:ListItem>
                                                        <asp:ListItem Value="AR">Argentina</asp:ListItem>
                                                        <asp:ListItem Value="AM">Armenia</asp:ListItem>
                                                        <asp:ListItem Value="AW">Aruba</asp:ListItem>
                                                        <asp:ListItem Value="AU">Australia</asp:ListItem>
                                                        <asp:ListItem Value="AT">Austria</asp:ListItem>
                                                        <asp:ListItem Value="AZ">Azerbaijan</asp:ListItem>
                                                        <asp:ListItem Value="BS">Bahamas</asp:ListItem>
                                                        <asp:ListItem Value="BH">Bahrain</asp:ListItem>
                                                        <asp:ListItem Value="BD">Bangladesh</asp:ListItem>
                                                        <asp:ListItem Value="BB">Barbados</asp:ListItem>
                                                        <asp:ListItem Value="BY">Belarus</asp:ListItem>
                                                        <asp:ListItem Value="BE">Belgium</asp:ListItem>
                                                        <asp:ListItem Value="BZ">Belize</asp:ListItem>
                                                        <asp:ListItem Value="BJ">Benin</asp:ListItem>
                                                        <asp:ListItem Value="BM">Bermuda</asp:ListItem>
                                                        <asp:ListItem Value="BT">Bhutan</asp:ListItem>
                                                        <asp:ListItem Value="BO">Bolivia</asp:ListItem>
                                                        <asp:ListItem Value="BA">Bosnia and Herzegovina</asp:ListItem>
                                                        <asp:ListItem Value="BW">Botswana</asp:ListItem>
                                                        <asp:ListItem Value="BV">Bouvet Island</asp:ListItem>
                                                        <asp:ListItem Value="BR">Brazil</asp:ListItem>
                                                        <asp:ListItem Value="VG">British Virgin Islands</asp:ListItem>
                                                        <asp:ListItem Value="IO">British Indian Ocean Territory</asp:ListItem>
                                                        <asp:ListItem Value="BN">Brunei Darussalam</asp:ListItem>
                                                        <asp:ListItem Value="BG">Bulgaria</asp:ListItem>
                                                        <asp:ListItem Value="BF">Burkina Faso</asp:ListItem>
                                                        <asp:ListItem Value="BI">Burundi</asp:ListItem>
                                                        <asp:ListItem Value="KH">Cambodia</asp:ListItem>
                                                        <asp:ListItem Value="CM">Cameroon</asp:ListItem>
                                                        <asp:ListItem Value="CA">Canada</asp:ListItem>
                                                        <asp:ListItem Value="CV">Cape Verde</asp:ListItem>
                                                        <asp:ListItem Value="KY">Cayman Islands</asp:ListItem>
                                                        <asp:ListItem Value="CF">Central African Republic</asp:ListItem>
                                                        <asp:ListItem Value="TD">Chad</asp:ListItem>
                                                        <asp:ListItem Value="CL">Chile</asp:ListItem>
                                                        <asp:ListItem Value="CN">China</asp:ListItem>
                                                        <asp:ListItem Value="HK">Hong Kong, SAR China</asp:ListItem>
                                                        <asp:ListItem Value="MO">Macao, SAR China</asp:ListItem>
                                                        <asp:ListItem Value="CX">Christmas Island</asp:ListItem>
                                                        <asp:ListItem Value="CC">Cocos (Keeling) Islands</asp:ListItem>
                                                        <asp:ListItem Value="CO">Colombia</asp:ListItem>
                                                        <asp:ListItem Value="KM">Comoros</asp:ListItem>
                                                        <asp:ListItem Value="CG">Congo (Brazzaville)</asp:ListItem>
                                                        <asp:ListItem Value="CD">Congo, (Kinshasa)</asp:ListItem>
                                                        <asp:ListItem Value="CK">Cook Islands</asp:ListItem>
                                                        <asp:ListItem Value="CR">Costa Rica</asp:ListItem>
                                                        <asp:ListItem Value="CI">Côte d'Ivoire</asp:ListItem>
                                                        <asp:ListItem Value="HR">Croatia</asp:ListItem>
                                                        <asp:ListItem Value="CU">Cuba</asp:ListItem>
                                                        <asp:ListItem Value="CY">Cyprus</asp:ListItem>
                                                        <asp:ListItem Value="CZ">Czech Republic</asp:ListItem>
                                                        <asp:ListItem Value="DK">Denmark</asp:ListItem>
                                                        <asp:ListItem Value="DJ">Djibouti</asp:ListItem>
                                                        <asp:ListItem Value="DM">Dominica</asp:ListItem>
                                                        <asp:ListItem Value="DO">Dominican Republic</asp:ListItem>
                                                        <asp:ListItem Value="EC">Ecuador</asp:ListItem>
                                                        <asp:ListItem Value="EG">Egypt</asp:ListItem>
                                                        <asp:ListItem Value="SV">El Salvador</asp:ListItem>
                                                        <asp:ListItem Value="GQ">Equatorial Guinea</asp:ListItem>
                                                        <asp:ListItem Value="ER">Eritrea</asp:ListItem>
                                                        <asp:ListItem Value="EE">Estonia</asp:ListItem>
                                                        <asp:ListItem Value="ET">Ethiopia</asp:ListItem>
                                                        <asp:ListItem Value="FK">Falkland Islands (Malvinas)</asp:ListItem>
                                                        <asp:ListItem Value="FO">Faroe Islands</asp:ListItem>
                                                        <asp:ListItem Value="FJ">Fiji</asp:ListItem>
                                                        <asp:ListItem Value="FI">Finland</asp:ListItem>
                                                        <asp:ListItem Value="FR">France</asp:ListItem>
                                                        <asp:ListItem Value="GF">French Guiana</asp:ListItem>
                                                        <asp:ListItem Value="PF">French Polynesia</asp:ListItem>
                                                        <asp:ListItem Value="TF">French Southern Territories</asp:ListItem>
                                                        <asp:ListItem Value="GA">Gabon</asp:ListItem>
                                                        <asp:ListItem Value="GM">Gambia</asp:ListItem>
                                                        <asp:ListItem Value="GE">Georgia</asp:ListItem>
                                                        <asp:ListItem Value="DE">Germany</asp:ListItem>
                                                        <asp:ListItem Value="GH">Ghana</asp:ListItem>
                                                        <asp:ListItem Value="GI">Gibraltar</asp:ListItem>
                                                        <asp:ListItem Value="GR">Greece</asp:ListItem>
                                                        <asp:ListItem Value="GL">Greenland</asp:ListItem>
                                                        <asp:ListItem Value="GD">Grenada</asp:ListItem>
                                                        <asp:ListItem Value="GP">Guadeloupe</asp:ListItem>
                                                        <asp:ListItem Value="GU">Guam</asp:ListItem>
                                                        <asp:ListItem Value="GT">Guatemala</asp:ListItem>
                                                        <asp:ListItem Value="GG">Guernsey</asp:ListItem>
                                                        <asp:ListItem Value="GN">Guinea</asp:ListItem>
                                                        <asp:ListItem Value="GW">Guinea-Bissau</asp:ListItem>
                                                        <asp:ListItem Value="GY">Guyana</asp:ListItem>
                                                        <asp:ListItem Value="HT">Haiti</asp:ListItem>
                                                        <asp:ListItem Value="HM">Heard and Mcdonald Islands</asp:ListItem>
                                                        <asp:ListItem Value="VA">Holy See (Vatican City State)</asp:ListItem>
                                                        <asp:ListItem Value="HN">Honduras</asp:ListItem>
                                                        <asp:ListItem Value="HU">Hungary</asp:ListItem>
                                                        <asp:ListItem Value="IS">Iceland</asp:ListItem>
                                                        <asp:ListItem Value="IN">India</asp:ListItem>
                                                        <asp:ListItem Value="ID">Indonesia</asp:ListItem>
                                                        <asp:ListItem Value="IR">Iran, Islamic Republic of</asp:ListItem>
                                                        <asp:ListItem Value="IQ">Iraq</asp:ListItem>
                                                        <asp:ListItem Value="IE">Ireland</asp:ListItem>
                                                        <asp:ListItem Value="IM">Isle of Man</asp:ListItem>
                                                        <asp:ListItem Value="IL">Israel</asp:ListItem>
                                                        <asp:ListItem Value="IT">Italy</asp:ListItem>
                                                        <asp:ListItem Value="JM">Jamaica</asp:ListItem>
                                                        <asp:ListItem Value="JP">Japan</asp:ListItem>
                                                        <asp:ListItem Value="JE">Jersey</asp:ListItem>
                                                        <asp:ListItem Value="JO">Jordan</asp:ListItem>
                                                        <asp:ListItem Value="KZ">Kazakhstan</asp:ListItem>
                                                        <asp:ListItem Value="KE">Kenya</asp:ListItem>
                                                        <asp:ListItem Value="KI">Kiribati</asp:ListItem>
                                                        <asp:ListItem Value="KP">Korea (North)</asp:ListItem>
                                                        <asp:ListItem Value="KR">Korea (South)</asp:ListItem>
                                                        <asp:ListItem Value="KW">Kuwait</asp:ListItem>
                                                        <asp:ListItem Value="KG">Kyrgyzstan</asp:ListItem>
                                                        <asp:ListItem Value="LA">Lao PDR</asp:ListItem>
                                                        <asp:ListItem Value="LV">Latvia</asp:ListItem>
                                                        <asp:ListItem Value="LB">Lebanon</asp:ListItem>
                                                        <asp:ListItem Value="LS">Lesotho</asp:ListItem>
                                                        <asp:ListItem Value="LR">Liberia</asp:ListItem>
                                                        <asp:ListItem Value="LY">Libya</asp:ListItem>
                                                        <asp:ListItem Value="LI">Liechtenstein</asp:ListItem>
                                                        <asp:ListItem Value="LT">Lithuania</asp:ListItem>
                                                        <asp:ListItem Value="LU">Luxembourg</asp:ListItem>
                                                        <asp:ListItem Value="MK">Macedonia, Republic of</asp:ListItem>
                                                        <asp:ListItem Value="MG">Madagascar</asp:ListItem>
                                                        <asp:ListItem Value="MW">Malawi</asp:ListItem>
                                                        <asp:ListItem Value="MY">Malaysia</asp:ListItem>
                                                        <asp:ListItem Value="MV">Maldives</asp:ListItem>
                                                        <asp:ListItem Value="ML">Mali</asp:ListItem>
                                                        <asp:ListItem Value="MT">Malta</asp:ListItem>
                                                        <asp:ListItem Value="MH">Marshall Islands</asp:ListItem>
                                                        <asp:ListItem Value="MQ">Martinique</asp:ListItem>
                                                        <asp:ListItem Value="MR">Mauritania</asp:ListItem>
                                                        <asp:ListItem Value="MU">Mauritius</asp:ListItem>
                                                        <asp:ListItem Value="YT">Mayotte</asp:ListItem>
                                                        <asp:ListItem Value="MX">Mexico</asp:ListItem>
                                                        <asp:ListItem Value="FM">Micronesia, Federated States of</asp:ListItem>
                                                        <asp:ListItem Value="MD">Moldova</asp:ListItem>
                                                        <asp:ListItem Value="MC">Monaco</asp:ListItem>
                                                        <asp:ListItem Value="MN">Mongolia</asp:ListItem>
                                                        <asp:ListItem Value="ME">Montenegro</asp:ListItem>
                                                        <asp:ListItem Value="MS">Montserrat</asp:ListItem>
                                                        <asp:ListItem Value="MA">Morocco</asp:ListItem>
                                                        <asp:ListItem Value="MZ">Mozambique</asp:ListItem>
                                                        <asp:ListItem Value="MM">Myanmar</asp:ListItem>
                                                        <asp:ListItem Value="NA">Namibia</asp:ListItem>
                                                        <asp:ListItem Value="NR">Nauru</asp:ListItem>
                                                        <asp:ListItem Value="NP">Nepal</asp:ListItem>
                                                        <asp:ListItem Value="NL">Netherlands</asp:ListItem>
                                                        <asp:ListItem Value="AN">Netherlands Antilles</asp:ListItem>
                                                        <asp:ListItem Value="NC">New Caledonia</asp:ListItem>
                                                        <asp:ListItem Value="NZ">New Zealand</asp:ListItem>
                                                        <asp:ListItem Value="NI">Nicaragua</asp:ListItem>
                                                        <asp:ListItem Value="NE">Niger</asp:ListItem>
                                                        <asp:ListItem Value="NG">Nigeria</asp:ListItem>
                                                        <asp:ListItem Value="NU">Niue</asp:ListItem>
                                                        <asp:ListItem Value="NF">Norfolk Island</asp:ListItem>
                                                        <asp:ListItem Value="MP">Northern Mariana Islands</asp:ListItem>
                                                        <asp:ListItem Value="NO">Norway</asp:ListItem>
                                                        <asp:ListItem Value="OM">Oman</asp:ListItem>
                                                        <asp:ListItem Value="PK">Pakistan</asp:ListItem>
                                                        <asp:ListItem Value="PW">Palau</asp:ListItem>
                                                        <asp:ListItem Value="PS">Palestinian Territory</asp:ListItem>
                                                        <asp:ListItem Value="PA">Panama</asp:ListItem>
                                                        <asp:ListItem Value="PG">Papua New Guinea</asp:ListItem>
                                                        <asp:ListItem Value="PY">Paraguay</asp:ListItem>
                                                        <asp:ListItem Value="PE">Peru</asp:ListItem>
                                                        <asp:ListItem Value="PN">Pitcairn</asp:ListItem>
                                                        <asp:ListItem Value="PL">Poland</asp:ListItem>
                                                        <asp:ListItem Value="PT">Portugal</asp:ListItem>
                                                        <asp:ListItem Value="PR">Puerto Rico</asp:ListItem>
                                                        <asp:ListItem Value="QA">Qatar</asp:ListItem>
                                                        <asp:ListItem Value="RE">Réunion</asp:ListItem>
                                                        <asp:ListItem Value="RO">Romania</asp:ListItem>
                                                        <asp:ListItem Value="RU">Russian Federation</asp:ListItem>
                                                        <asp:ListItem Value="RW">Rwanda</asp:ListItem>
                                                        <asp:ListItem Value="BL">Saint-Barthélemy</asp:ListItem>
                                                        <asp:ListItem Value="SH">Saint Helena</asp:ListItem>
                                                        <asp:ListItem Value="KN">Saint Kitts and Nevis</asp:ListItem>
                                                        <asp:ListItem Value="LC">Saint Lucia</asp:ListItem>
                                                        <asp:ListItem Value="MF">Saint-Martin (French part)</asp:ListItem>
                                                        <asp:ListItem Value="PM">Saint Pierre and Miquelon</asp:ListItem>
                                                        <asp:ListItem Value="VC">Saint Vincent and Grenadines</asp:ListItem>
                                                        <asp:ListItem Value="WS">Samoa</asp:ListItem>
                                                        <asp:ListItem Value="SM">San Marino</asp:ListItem>
                                                        <asp:ListItem Value="ST">Sao Tome and Principe</asp:ListItem>
                                                        <asp:ListItem Value="SA">Saudi Arabia</asp:ListItem>
                                                        <asp:ListItem Value="SN">Senegal</asp:ListItem>
                                                        <asp:ListItem Value="RS">Serbia</asp:ListItem>
                                                        <asp:ListItem Value="SC">Seychelles</asp:ListItem>
                                                        <asp:ListItem Value="SL">Sierra Leone</asp:ListItem>
                                                        <asp:ListItem Value="SG">Singapore</asp:ListItem>
                                                        <asp:ListItem Value="SK">Slovakia</asp:ListItem>
                                                        <asp:ListItem Value="SI">Slovenia</asp:ListItem>
                                                        <asp:ListItem Value="SB">Solomon Islands</asp:ListItem>
                                                        <asp:ListItem Value="SO">Somalia</asp:ListItem>
                                                        <asp:ListItem Value="ZA">South Africa</asp:ListItem>
                                                        <asp:ListItem Value="GS">South Georgia and the South Sandwich Islands</asp:ListItem>
                                                        <asp:ListItem Value="SS">South Sudan</asp:ListItem>
                                                        <asp:ListItem Value="ES">Spain</asp:ListItem>
                                                        <asp:ListItem Value="LK">Sri Lanka</asp:ListItem>
                                                        <asp:ListItem Value="SD">Sudan</asp:ListItem>
                                                        <asp:ListItem Value="SR">Suriname</asp:ListItem>
                                                        <asp:ListItem Value="SJ">Svalbard and Jan Mayen Islands</asp:ListItem>
                                                        <asp:ListItem Value="SZ">Swaziland</asp:ListItem>
                                                        <asp:ListItem Value="SE">Sweden</asp:ListItem>
                                                        <asp:ListItem Value="CH">Switzerland</asp:ListItem>
                                                        <asp:ListItem Value="SY">Syrian Arab Republic (Syria)</asp:ListItem>
                                                        <asp:ListItem Value="TW">Taiwan, Republic of China</asp:ListItem>
                                                        <asp:ListItem Value="TJ">Tajikistan</asp:ListItem>
                                                        <asp:ListItem Value="TZ">Tanzania, United Republic of</asp:ListItem>
                                                        <asp:ListItem Value="TH">Thailand</asp:ListItem>
                                                        <asp:ListItem Value="TL">Timor-Leste</asp:ListItem>
                                                        <asp:ListItem Value="TG">Togo</asp:ListItem>
                                                        <asp:ListItem Value="TK">Tokelau</asp:ListItem>
                                                        <asp:ListItem Value="TO">Tonga</asp:ListItem>
                                                        <asp:ListItem Value="TT">Trinidad and Tobago</asp:ListItem>
                                                        <asp:ListItem Value="TN">Tunisia</asp:ListItem>
                                                        <asp:ListItem Value="TR">Turkey</asp:ListItem>
                                                        <asp:ListItem Value="TM">Turkmenistan</asp:ListItem>
                                                        <asp:ListItem Value="TC">Turks and Caicos Islands</asp:ListItem>
                                                        <asp:ListItem Value="TV">Tuvalu</asp:ListItem>
                                                        <asp:ListItem Value="UG">Uganda</asp:ListItem>
                                                        <asp:ListItem Value="UA">Ukraine</asp:ListItem>
                                                        <asp:ListItem Value="AE">United Arab Emirates</asp:ListItem>
                                                        <asp:ListItem Value="GB">United Kingdom</asp:ListItem>
                                                        <asp:ListItem Value="US">United States of America</asp:ListItem>
                                                        <asp:ListItem Value="UM">US Minor Outlying Islands</asp:ListItem>
                                                        <asp:ListItem Value="UY">Uruguay</asp:ListItem>
                                                        <asp:ListItem Value="UZ">Uzbekistan</asp:ListItem>
                                                        <asp:ListItem Value="VU">Vanuatu</asp:ListItem>
                                                        <asp:ListItem Value="VE">Venezuela (Bolivarian Republic)</asp:ListItem>
                                                        <asp:ListItem Value="VN">Vietnam</asp:ListItem>
                                                        <asp:ListItem Value="VI">Virgin Islands, US</asp:ListItem>
                                                        <asp:ListItem Value="WF">Wallis and Futuna Islands</asp:ListItem>
                                                        <asp:ListItem Value="EH">Western Sahara</asp:ListItem>
                                                        <asp:ListItem Value="YE">Yemen</asp:ListItem>
                                                        <asp:ListItem Value="ZM">Zambia</asp:ListItem>
                                                        <asp:ListItem Value="ZW">Zimbabwe</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Present Address Years of Stay <span class="color-red fsize-16">*</span></label>
                                                    <asp:TextBox ID="txtPresYrStay" runat="server" class="form-control" type="text" onkeypress="return isNumberKey(event)" placeholder="10" />
                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator49"
                                                        ControlToValidate="txtPresYrStay"
                                                        Text="Please fill up Present Address Years of Stay."
                                                        ValidationGroup="Save"
                                                        runat="server" Style="color: red" />
                                                </div>
                                            </div>

























                                            <div runat="server" id="nonCorpAddress">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label>
                                                            <h4><b>Permanent Address</b></h4>
                                                        </label>
                                                        &nbsp; <span>
                                                            <asp:LinkButton runat="server" ID="btnSameasPresent" OnClick="btnSameasPresent_Click" class="btn btn-info btn-sm"><b>Same as Present Address</b></asp:LinkButton></span>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <label>No. <span class="color-red fsize-16">*</span></label>
                                                        <%--<asp:TextBox runat="server" type="text" AutoPostBack="true" OnTextChanged="txtAddress_TextChanged" class="form-control text-uppercase" ID="TextBox1" placeholder="No." />--%>
                                                        <asp:TextBox runat="server" type="text" class="form-control text-uppercase" ID="tPermanent" placeholder="No." />
                                                        <%--<input runat="server" type="text" class="form-control text-uppercase" id="tPermanent" placeholder="No." />--%>
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator50"
                                                            ControlToValidate="tPermanent"
                                                            Text="Please fill up Permanent Address No."
                                                            ValidationGroup="Save"
                                                            runat="server" Style="color: red" />
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <label>Street <span class="color-red fsize-16">*</span></label>
                                                        <%--<asp:TextBox runat="server" type="text" AutoPostBack="true" OnTextChanged="txtAddress_TextChanged" class="form-control text-uppercase" ID="TextBox1" placeholder="Street" />--%>
                                                        <asp:TextBox runat="server" type="text" class="form-control text-uppercase" ID="txtPermanentStreet" placeholder="Street" />
                                                        <%--<input runat="server" type="text" class="form-control text-uppercase" id="txtPermanentStreet" placeholder="Street" />--%>
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator52"
                                                            ControlToValidate="txtPermanentStreet"
                                                            Text="Please fill up Permanent Street."
                                                            ValidationGroup="Save"
                                                            runat="server" Style="color: red" />
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <label>Subdivision <span class="color-red fsize-16">*</span></label>
                                                        <%--<asp:TextBox runat="server" type="text" AutoPostBack="true" OnTextChanged="txtAddress_TextChanged" class="form-control text-uppercase" ID="TextBox1" placeholder="Subdivision" />--%>
                                                        <asp:TextBox runat="server" type="text" class="form-control text-uppercase" ID="txtPermanentSubdivision" placeholder="Subdivision" />
                                                        <%--<input runat="server" type="text" class="form-control text-uppercase" id="txtPermanentSubdivision" placeholder="Subdivision" />--%>
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator53"
                                                            ControlToValidate="txtPermanentSubdivision"
                                                            Text="Please fill up Permanent Subdivision."
                                                            ValidationGroup="Save"
                                                            runat="server" Style="color: red" />
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <label>Barangay/District <span class="color-red fsize-16">*</span></label>
                                                        <%--<asp:TextBox runat="server" type="text" AutoPostBack="true" OnTextChanged="txtAddress_TextChanged" class="form-control text-uppercase" ID="TextBox1" placeholder="Barangay/District" />--%>
                                                        <asp:TextBox runat="server" type="text" class="form-control text-uppercase" ID="txtPermanentBarangay" placeholder="Barangay/District" />
                                                        <%--<input runat="server" type="text" class="form-control text-uppercase" id="txtPermanentBarangay" placeholder="Barangay/District" />--%>
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator54"
                                                            ControlToValidate="txtPermanentBarangay"
                                                            Text="Please fill up Permanent Barangay/District."
                                                            ValidationGroup="Save"
                                                            runat="server" Style="color: red" />
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <label>Municipality/City <span class="color-red fsize-16">*</span></label>
                                                        <%--<asp:TextBox runat="server" type="text" AutoPostBack="true" OnTextChanged="txtAddress_TextChanged" class="form-control text-uppercase" ID="TextBox1" placeholder="Municipality/City" />--%>
                                                        <asp:TextBox runat="server" type="text" class="form-control text-uppercase" ID="txtPermanentCity" placeholder="Municipality/City" />
                                                        <%--<input runat="server" type="text" class="form-control text-uppercase" id="txtPermanentCity" placeholder="Municipality/City" />--%>
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator55"
                                                            ControlToValidate="txtPermanentCity"
                                                            Text="Please fill up Permanent Municipality/City."
                                                            ValidationGroup="Save"
                                                            runat="server" Style="color: red" />
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <label>Province <span class="color-red fsize-16">*</span></label>
                                                        <%--<asp:TextBox runat="server" type="text" AutoPostBack="true" OnTextChanged="txtAddress_TextChanged" class="form-control text-uppercase" ID="TextBox1" placeholder="Province" />--%>
                                                        <asp:TextBox runat="server" type="text" class="form-control text-uppercase" ID="txtPermanentProvince" placeholder="Province" />
                                                        <%--<input runat="server" type="text" class="form-control text-uppercase" id="txtPermanentProvince" placeholder="Province" />--%>
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator56"
                                                            ControlToValidate="txtPermanentProvince"
                                                            Text="Please fill up Permanent Province."
                                                            ValidationGroup="Save"
                                                            runat="server" Style="color: red" />
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label>Permanent Postal Code <span class="color-red fsize-16">*</span></label>
                                                        <%--<asp:TextBox ID="TextBox1" AutoPostBack="true" OnTextChanged="txtAddress_TextChanged" runat="server" onkeypress="return alpha(event)" class="form-control" type="text" placeholder="000" />--%>
                                                        <asp:TextBox ID="txtPermPostal" runat="server" onkeypress="return alpha(event)" class="form-control" type="text" placeholder="000" />
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator57"
                                                            ControlToValidate="txtPermPostal"
                                                            Text="Please fill up Permanent Postal Code."
                                                            ValidationGroup="Save"
                                                            runat="server" Style="color: red" />
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label>Permanent Country</label>
                                                        <%-- <asp:DropDownList ID="DropDownList1" OnSelectedIndexChanged="ddPreCountry_SelectedIndexChanged" runat="server" CssClass="form-control text-uppercase">--%>
                                                        <asp:DropDownList ID="ddPermCountry" runat="server" CssClass="form-control text-uppercase">
                                                            <asp:ListItem Selected="true" Value="PH">Philippines</asp:ListItem>
                                                            <asp:ListItem Value="1">--- SELECT COUNTRY ---</asp:ListItem>
                                                            <asp:ListItem Value="AF">Afghanistan</asp:ListItem>
                                                            <asp:ListItem Value="AX">Aland Islands</asp:ListItem>
                                                            <asp:ListItem Value="AL">Albania</asp:ListItem>
                                                            <asp:ListItem Value="DZ">Algeria</asp:ListItem>
                                                            <asp:ListItem Value="AS">American Samoa</asp:ListItem>
                                                            <asp:ListItem Value="AD">Andorra</asp:ListItem>
                                                            <asp:ListItem Value="AO">Angola</asp:ListItem>
                                                            <asp:ListItem Value="AI">Anguilla</asp:ListItem>
                                                            <asp:ListItem Value="AQ">Antarctica</asp:ListItem>
                                                            <asp:ListItem Value="AG">Antigua and Barbuda</asp:ListItem>
                                                            <asp:ListItem Value="AR">Argentina</asp:ListItem>
                                                            <asp:ListItem Value="AM">Armenia</asp:ListItem>
                                                            <asp:ListItem Value="AW">Aruba</asp:ListItem>
                                                            <asp:ListItem Value="AU">Australia</asp:ListItem>
                                                            <asp:ListItem Value="AT">Austria</asp:ListItem>
                                                            <asp:ListItem Value="AZ">Azerbaijan</asp:ListItem>
                                                            <asp:ListItem Value="BS">Bahamas</asp:ListItem>
                                                            <asp:ListItem Value="BH">Bahrain</asp:ListItem>
                                                            <asp:ListItem Value="BD">Bangladesh</asp:ListItem>
                                                            <asp:ListItem Value="BB">Barbados</asp:ListItem>
                                                            <asp:ListItem Value="BY">Belarus</asp:ListItem>
                                                            <asp:ListItem Value="BE">Belgium</asp:ListItem>
                                                            <asp:ListItem Value="BZ">Belize</asp:ListItem>
                                                            <asp:ListItem Value="BJ">Benin</asp:ListItem>
                                                            <asp:ListItem Value="BM">Bermuda</asp:ListItem>
                                                            <asp:ListItem Value="BT">Bhutan</asp:ListItem>
                                                            <asp:ListItem Value="BO">Bolivia</asp:ListItem>
                                                            <asp:ListItem Value="BA">Bosnia and Herzegovina</asp:ListItem>
                                                            <asp:ListItem Value="BW">Botswana</asp:ListItem>
                                                            <asp:ListItem Value="BV">Bouvet Island</asp:ListItem>
                                                            <asp:ListItem Value="BR">Brazil</asp:ListItem>
                                                            <asp:ListItem Value="VG">British Virgin Islands</asp:ListItem>
                                                            <asp:ListItem Value="IO">British Indian Ocean Territory</asp:ListItem>
                                                            <asp:ListItem Value="BN">Brunei Darussalam</asp:ListItem>
                                                            <asp:ListItem Value="BG">Bulgaria</asp:ListItem>
                                                            <asp:ListItem Value="BF">Burkina Faso</asp:ListItem>
                                                            <asp:ListItem Value="BI">Burundi</asp:ListItem>
                                                            <asp:ListItem Value="KH">Cambodia</asp:ListItem>
                                                            <asp:ListItem Value="CM">Cameroon</asp:ListItem>
                                                            <asp:ListItem Value="CA">Canada</asp:ListItem>
                                                            <asp:ListItem Value="CV">Cape Verde</asp:ListItem>
                                                            <asp:ListItem Value="KY">Cayman Islands</asp:ListItem>
                                                            <asp:ListItem Value="CF">Central African Republic</asp:ListItem>
                                                            <asp:ListItem Value="TD">Chad</asp:ListItem>
                                                            <asp:ListItem Value="CL">Chile</asp:ListItem>
                                                            <asp:ListItem Value="CN">China</asp:ListItem>
                                                            <asp:ListItem Value="HK">Hong Kong, SAR China</asp:ListItem>
                                                            <asp:ListItem Value="MO">Macao, SAR China</asp:ListItem>
                                                            <asp:ListItem Value="CX">Christmas Island</asp:ListItem>
                                                            <asp:ListItem Value="CC">Cocos (Keeling) Islands</asp:ListItem>
                                                            <asp:ListItem Value="CO">Colombia</asp:ListItem>
                                                            <asp:ListItem Value="KM">Comoros</asp:ListItem>
                                                            <asp:ListItem Value="CG">Congo (Brazzaville)</asp:ListItem>
                                                            <asp:ListItem Value="CD">Congo, (Kinshasa)</asp:ListItem>
                                                            <asp:ListItem Value="CK">Cook Islands</asp:ListItem>
                                                            <asp:ListItem Value="CR">Costa Rica</asp:ListItem>
                                                            <asp:ListItem Value="CI">Côte d'Ivoire</asp:ListItem>
                                                            <asp:ListItem Value="HR">Croatia</asp:ListItem>
                                                            <asp:ListItem Value="CU">Cuba</asp:ListItem>
                                                            <asp:ListItem Value="CY">Cyprus</asp:ListItem>
                                                            <asp:ListItem Value="CZ">Czech Republic</asp:ListItem>
                                                            <asp:ListItem Value="DK">Denmark</asp:ListItem>
                                                            <asp:ListItem Value="DJ">Djibouti</asp:ListItem>
                                                            <asp:ListItem Value="DM">Dominica</asp:ListItem>
                                                            <asp:ListItem Value="DO">Dominican Republic</asp:ListItem>
                                                            <asp:ListItem Value="EC">Ecuador</asp:ListItem>
                                                            <asp:ListItem Value="EG">Egypt</asp:ListItem>
                                                            <asp:ListItem Value="SV">El Salvador</asp:ListItem>
                                                            <asp:ListItem Value="GQ">Equatorial Guinea</asp:ListItem>
                                                            <asp:ListItem Value="ER">Eritrea</asp:ListItem>
                                                            <asp:ListItem Value="EE">Estonia</asp:ListItem>
                                                            <asp:ListItem Value="ET">Ethiopia</asp:ListItem>
                                                            <asp:ListItem Value="FK">Falkland Islands (Malvinas)</asp:ListItem>
                                                            <asp:ListItem Value="FO">Faroe Islands</asp:ListItem>
                                                            <asp:ListItem Value="FJ">Fiji</asp:ListItem>
                                                            <asp:ListItem Value="FI">Finland</asp:ListItem>
                                                            <asp:ListItem Value="FR">France</asp:ListItem>
                                                            <asp:ListItem Value="GF">French Guiana</asp:ListItem>
                                                            <asp:ListItem Value="PF">French Polynesia</asp:ListItem>
                                                            <asp:ListItem Value="TF">French Southern Territories</asp:ListItem>
                                                            <asp:ListItem Value="GA">Gabon</asp:ListItem>
                                                            <asp:ListItem Value="GM">Gambia</asp:ListItem>
                                                            <asp:ListItem Value="GE">Georgia</asp:ListItem>
                                                            <asp:ListItem Value="DE">Germany</asp:ListItem>
                                                            <asp:ListItem Value="GH">Ghana</asp:ListItem>
                                                            <asp:ListItem Value="GI">Gibraltar</asp:ListItem>
                                                            <asp:ListItem Value="GR">Greece</asp:ListItem>
                                                            <asp:ListItem Value="GL">Greenland</asp:ListItem>
                                                            <asp:ListItem Value="GD">Grenada</asp:ListItem>
                                                            <asp:ListItem Value="GP">Guadeloupe</asp:ListItem>
                                                            <asp:ListItem Value="GU">Guam</asp:ListItem>
                                                            <asp:ListItem Value="GT">Guatemala</asp:ListItem>
                                                            <asp:ListItem Value="GG">Guernsey</asp:ListItem>
                                                            <asp:ListItem Value="GN">Guinea</asp:ListItem>
                                                            <asp:ListItem Value="GW">Guinea-Bissau</asp:ListItem>
                                                            <asp:ListItem Value="GY">Guyana</asp:ListItem>
                                                            <asp:ListItem Value="HT">Haiti</asp:ListItem>
                                                            <asp:ListItem Value="HM">Heard and Mcdonald Islands</asp:ListItem>
                                                            <asp:ListItem Value="VA">Holy See (Vatican City State)</asp:ListItem>
                                                            <asp:ListItem Value="HN">Honduras</asp:ListItem>
                                                            <asp:ListItem Value="HU">Hungary</asp:ListItem>
                                                            <asp:ListItem Value="IS">Iceland</asp:ListItem>
                                                            <asp:ListItem Value="IN">India</asp:ListItem>
                                                            <asp:ListItem Value="ID">Indonesia</asp:ListItem>
                                                            <asp:ListItem Value="IR">Iran, Islamic Republic of</asp:ListItem>
                                                            <asp:ListItem Value="IQ">Iraq</asp:ListItem>
                                                            <asp:ListItem Value="IE">Ireland</asp:ListItem>
                                                            <asp:ListItem Value="IM">Isle of Man</asp:ListItem>
                                                            <asp:ListItem Value="IL">Israel</asp:ListItem>
                                                            <asp:ListItem Value="IT">Italy</asp:ListItem>
                                                            <asp:ListItem Value="JM">Jamaica</asp:ListItem>
                                                            <asp:ListItem Value="JP">Japan</asp:ListItem>
                                                            <asp:ListItem Value="JE">Jersey</asp:ListItem>
                                                            <asp:ListItem Value="JO">Jordan</asp:ListItem>
                                                            <asp:ListItem Value="KZ">Kazakhstan</asp:ListItem>
                                                            <asp:ListItem Value="KE">Kenya</asp:ListItem>
                                                            <asp:ListItem Value="KI">Kiribati</asp:ListItem>
                                                            <asp:ListItem Value="KP">Korea (North)</asp:ListItem>
                                                            <asp:ListItem Value="KR">Korea (South)</asp:ListItem>
                                                            <asp:ListItem Value="KW">Kuwait</asp:ListItem>
                                                            <asp:ListItem Value="KG">Kyrgyzstan</asp:ListItem>
                                                            <asp:ListItem Value="LA">Lao PDR</asp:ListItem>
                                                            <asp:ListItem Value="LV">Latvia</asp:ListItem>
                                                            <asp:ListItem Value="LB">Lebanon</asp:ListItem>
                                                            <asp:ListItem Value="LS">Lesotho</asp:ListItem>
                                                            <asp:ListItem Value="LR">Liberia</asp:ListItem>
                                                            <asp:ListItem Value="LY">Libya</asp:ListItem>
                                                            <asp:ListItem Value="LI">Liechtenstein</asp:ListItem>
                                                            <asp:ListItem Value="LT">Lithuania</asp:ListItem>
                                                            <asp:ListItem Value="LU">Luxembourg</asp:ListItem>
                                                            <asp:ListItem Value="MK">Macedonia, Republic of</asp:ListItem>
                                                            <asp:ListItem Value="MG">Madagascar</asp:ListItem>
                                                            <asp:ListItem Value="MW">Malawi</asp:ListItem>
                                                            <asp:ListItem Value="MY">Malaysia</asp:ListItem>
                                                            <asp:ListItem Value="MV">Maldives</asp:ListItem>
                                                            <asp:ListItem Value="ML">Mali</asp:ListItem>
                                                            <asp:ListItem Value="MT">Malta</asp:ListItem>
                                                            <asp:ListItem Value="MH">Marshall Islands</asp:ListItem>
                                                            <asp:ListItem Value="MQ">Martinique</asp:ListItem>
                                                            <asp:ListItem Value="MR">Mauritania</asp:ListItem>
                                                            <asp:ListItem Value="MU">Mauritius</asp:ListItem>
                                                            <asp:ListItem Value="YT">Mayotte</asp:ListItem>
                                                            <asp:ListItem Value="MX">Mexico</asp:ListItem>
                                                            <asp:ListItem Value="FM">Micronesia, Federated States of</asp:ListItem>
                                                            <asp:ListItem Value="MD">Moldova</asp:ListItem>
                                                            <asp:ListItem Value="MC">Monaco</asp:ListItem>
                                                            <asp:ListItem Value="MN">Mongolia</asp:ListItem>
                                                            <asp:ListItem Value="ME">Montenegro</asp:ListItem>
                                                            <asp:ListItem Value="MS">Montserrat</asp:ListItem>
                                                            <asp:ListItem Value="MA">Morocco</asp:ListItem>
                                                            <asp:ListItem Value="MZ">Mozambique</asp:ListItem>
                                                            <asp:ListItem Value="MM">Myanmar</asp:ListItem>
                                                            <asp:ListItem Value="NA">Namibia</asp:ListItem>
                                                            <asp:ListItem Value="NR">Nauru</asp:ListItem>
                                                            <asp:ListItem Value="NP">Nepal</asp:ListItem>
                                                            <asp:ListItem Value="NL">Netherlands</asp:ListItem>
                                                            <asp:ListItem Value="AN">Netherlands Antilles</asp:ListItem>
                                                            <asp:ListItem Value="NC">New Caledonia</asp:ListItem>
                                                            <asp:ListItem Value="NZ">New Zealand</asp:ListItem>
                                                            <asp:ListItem Value="NI">Nicaragua</asp:ListItem>
                                                            <asp:ListItem Value="NE">Niger</asp:ListItem>
                                                            <asp:ListItem Value="NG">Nigeria</asp:ListItem>
                                                            <asp:ListItem Value="NU">Niue</asp:ListItem>
                                                            <asp:ListItem Value="NF">Norfolk Island</asp:ListItem>
                                                            <asp:ListItem Value="MP">Northern Mariana Islands</asp:ListItem>
                                                            <asp:ListItem Value="NO">Norway</asp:ListItem>
                                                            <asp:ListItem Value="OM">Oman</asp:ListItem>
                                                            <asp:ListItem Value="PK">Pakistan</asp:ListItem>
                                                            <asp:ListItem Value="PW">Palau</asp:ListItem>
                                                            <asp:ListItem Value="PS">Palestinian Territory</asp:ListItem>
                                                            <asp:ListItem Value="PA">Panama</asp:ListItem>
                                                            <asp:ListItem Value="PG">Papua New Guinea</asp:ListItem>
                                                            <asp:ListItem Value="PY">Paraguay</asp:ListItem>
                                                            <asp:ListItem Value="PE">Peru</asp:ListItem>
                                                            <asp:ListItem Value="PN">Pitcairn</asp:ListItem>
                                                            <asp:ListItem Value="PL">Poland</asp:ListItem>
                                                            <asp:ListItem Value="PT">Portugal</asp:ListItem>
                                                            <asp:ListItem Value="PR">Puerto Rico</asp:ListItem>
                                                            <asp:ListItem Value="QA">Qatar</asp:ListItem>
                                                            <asp:ListItem Value="RE">Réunion</asp:ListItem>
                                                            <asp:ListItem Value="RO">Romania</asp:ListItem>
                                                            <asp:ListItem Value="RU">Russian Federation</asp:ListItem>
                                                            <asp:ListItem Value="RW">Rwanda</asp:ListItem>
                                                            <asp:ListItem Value="BL">Saint-Barthélemy</asp:ListItem>
                                                            <asp:ListItem Value="SH">Saint Helena</asp:ListItem>
                                                            <asp:ListItem Value="KN">Saint Kitts and Nevis</asp:ListItem>
                                                            <asp:ListItem Value="LC">Saint Lucia</asp:ListItem>
                                                            <asp:ListItem Value="MF">Saint-Martin (French part)</asp:ListItem>
                                                            <asp:ListItem Value="PM">Saint Pierre and Miquelon</asp:ListItem>
                                                            <asp:ListItem Value="VC">Saint Vincent and Grenadines</asp:ListItem>
                                                            <asp:ListItem Value="WS">Samoa</asp:ListItem>
                                                            <asp:ListItem Value="SM">San Marino</asp:ListItem>
                                                            <asp:ListItem Value="ST">Sao Tome and Principe</asp:ListItem>
                                                            <asp:ListItem Value="SA">Saudi Arabia</asp:ListItem>
                                                            <asp:ListItem Value="SN">Senegal</asp:ListItem>
                                                            <asp:ListItem Value="RS">Serbia</asp:ListItem>
                                                            <asp:ListItem Value="SC">Seychelles</asp:ListItem>
                                                            <asp:ListItem Value="SL">Sierra Leone</asp:ListItem>
                                                            <asp:ListItem Value="SG">Singapore</asp:ListItem>
                                                            <asp:ListItem Value="SK">Slovakia</asp:ListItem>
                                                            <asp:ListItem Value="SI">Slovenia</asp:ListItem>
                                                            <asp:ListItem Value="SB">Solomon Islands</asp:ListItem>
                                                            <asp:ListItem Value="SO">Somalia</asp:ListItem>
                                                            <asp:ListItem Value="ZA">South Africa</asp:ListItem>
                                                            <asp:ListItem Value="GS">South Georgia and the South Sandwich Islands</asp:ListItem>
                                                            <asp:ListItem Value="SS">South Sudan</asp:ListItem>
                                                            <asp:ListItem Value="ES">Spain</asp:ListItem>
                                                            <asp:ListItem Value="LK">Sri Lanka</asp:ListItem>
                                                            <asp:ListItem Value="SD">Sudan</asp:ListItem>
                                                            <asp:ListItem Value="SR">Suriname</asp:ListItem>
                                                            <asp:ListItem Value="SJ">Svalbard and Jan Mayen Islands</asp:ListItem>
                                                            <asp:ListItem Value="SZ">Swaziland</asp:ListItem>
                                                            <asp:ListItem Value="SE">Sweden</asp:ListItem>
                                                            <asp:ListItem Value="CH">Switzerland</asp:ListItem>
                                                            <asp:ListItem Value="SY">Syrian Arab Republic (Syria)</asp:ListItem>
                                                            <asp:ListItem Value="TW">Taiwan, Republic of China</asp:ListItem>
                                                            <asp:ListItem Value="TJ">Tajikistan</asp:ListItem>
                                                            <asp:ListItem Value="TZ">Tanzania, United Republic of</asp:ListItem>
                                                            <asp:ListItem Value="TH">Thailand</asp:ListItem>
                                                            <asp:ListItem Value="TL">Timor-Leste</asp:ListItem>
                                                            <asp:ListItem Value="TG">Togo</asp:ListItem>
                                                            <asp:ListItem Value="TK">Tokelau</asp:ListItem>
                                                            <asp:ListItem Value="TO">Tonga</asp:ListItem>
                                                            <asp:ListItem Value="TT">Trinidad and Tobago</asp:ListItem>
                                                            <asp:ListItem Value="TN">Tunisia</asp:ListItem>
                                                            <asp:ListItem Value="TR">Turkey</asp:ListItem>
                                                            <asp:ListItem Value="TM">Turkmenistan</asp:ListItem>
                                                            <asp:ListItem Value="TC">Turks and Caicos Islands</asp:ListItem>
                                                            <asp:ListItem Value="TV">Tuvalu</asp:ListItem>
                                                            <asp:ListItem Value="UG">Uganda</asp:ListItem>
                                                            <asp:ListItem Value="UA">Ukraine</asp:ListItem>
                                                            <asp:ListItem Value="AE">United Arab Emirates</asp:ListItem>
                                                            <asp:ListItem Value="GB">United Kingdom</asp:ListItem>
                                                            <asp:ListItem Value="US">United States of America</asp:ListItem>
                                                            <asp:ListItem Value="UM">US Minor Outlying Islands</asp:ListItem>
                                                            <asp:ListItem Value="UY">Uruguay</asp:ListItem>
                                                            <asp:ListItem Value="UZ">Uzbekistan</asp:ListItem>
                                                            <asp:ListItem Value="VU">Vanuatu</asp:ListItem>
                                                            <asp:ListItem Value="VE">Venezuela (Bolivarian Republic)</asp:ListItem>
                                                            <asp:ListItem Value="VN">Vietnam</asp:ListItem>
                                                            <asp:ListItem Value="VI">Virgin Islands, US</asp:ListItem>
                                                            <asp:ListItem Value="WF">Wallis and Futuna Islands</asp:ListItem>
                                                            <asp:ListItem Value="EH">Western Sahara</asp:ListItem>
                                                            <asp:ListItem Value="YE">Yemen</asp:ListItem>
                                                            <asp:ListItem Value="ZM">Zambia</asp:ListItem>
                                                            <asp:ListItem Value="ZW">Zimbabwe</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label>Permanent Address Years of Stay <span class="color-red fsize-16">*</span></label>
                                                        <%--<asp:TextBox ID="TextBox1" runat="server" AutoPostBack="true" OnTextChanged="txtAddress_TextChanged" class="form-control" type="text" onkeypress="return isNumberKey(event)" placeholder="10" />--%>
                                                        <asp:TextBox ID="txtPermYrStay" runat="server" class="form-control" type="text" onkeypress="return isNumberKey(event)" placeholder="10" />
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator58"
                                                            ControlToValidate="txtPermYrStay"
                                                            Text="Please fill up Permanent Address Years of Stay."
                                                            ValidationGroup="Save"
                                                            runat="server" Style="color: red" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4" runat="server" id="tinhide">
                                                <div class="form-group">
                                                    <label>TIN No. <span runat="server" id="spanTinReq" class="color-red fsize-16">*</span></label>
                                                    <%--<input runat="server" type="text" class="form-control" id="tTIN" onkeyup="TIN_keyup(event);" onkeypress="return fnAllowNumeric(event)" placeholder="xxx-xxx-xxx-xxx" maxlength="15" />--%>
                                                    <%--<asp:TextBox runat="server" AutoPostBack="true" type="text" class="form-control" OnTextChanged="tTIN_TextChanged" ID="tTIN" onkeypress="return fnAllowNumeric(event)" placeholder="xxx-xxx-xxx-xxx" MaxLength="15"></asp:TextBox>--%>
                                                    <asp:TextBox runat="server" ID="tTIN" onchange="javascript: insertText();" ValidationGroup="Save" class="form-control" type="text" placeholder="xxx-xxx-xxx-xxx" MaxLength="15"></asp:TextBox>
                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator7"
                                                        ControlToValidate="tTIN"
                                                        Text="Please fill up TIN."
                                                        ValidationGroup="Save"
                                                        runat="server" Style="color: red" />
                                                    <asp:CustomValidator
                                                        Style="color: red"
                                                        runat="server"
                                                        ID="CustomValidator1"
                                                        ValidationGroup="Save"
                                                        ControlToValidate="tTIN"
                                                        Text="Incorrect TIN format. Must be xxx-xxx-xxx-xxx."
                                                        OnServerValidate="CustomValidator1_ServerValidate" />



                                                </div>
                                            </div>
                                            <%--                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>SSS No.</label>
                                                    <input runat="server" type="text" class="form-control" id="tSSSNo" maxlength="12" onkeypress="return fnAllowNumeric(event)" placeholder="xx xxxxxxx x" />
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>GSIS No.</label>
                                                    <input runat="server" type="text" class="form-control" id="tGSISNo" maxlength="14" onkeypress="return fnAllowNumeric(event)" placeholder="xxxx xxxxxxx x" />
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>PAG-IBIG No.</label>
                                                    <input runat="server" type="text" class="form-control" id="tPagibigNo" maxlength="14" onkeypress="return fnAllowNumeric(event)" placeholder="xxxx xxxxxxx x" />
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Business Phone No.</label>
                                                    <input runat="server" type="text" class="form-control" id="txtBusinessPhoneNo" onkeypress="return fnAllowNumeric(event)" maxlength="16" placeholder="xxxx xxx xxxx" />
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Profession</label>
                                                    <asp:TextBox ID="txtProfession" runat="server" class="form-control text-uppercase" type="text" placeholder="Engineer/Lawyer/Architect" />
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Employment Status</label>
                                                    <asp:DropDownList ID="tEmpStatus" runat="server" CssClass="form-control text-uppercase">
                                                        <asp:ListItem></asp:ListItem>
                                                        <asp:ListItem Value="---Select Employment Status---" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Value="ES1" Text="Casual"> </asp:ListItem>
                                                        <asp:ListItem Value="ES2" Text="Contractual"> </asp:ListItem>
                                                        <asp:ListItem Value="ES3" Text="Probationary"> </asp:ListItem>
                                                        <asp:ListItem Value="ES4" Text="Permanent"> </asp:ListItem>
                                                        <asp:ListItem Value="ES5" Text="Consultant"> </asp:ListItem>
                                                        <asp:ListItem Value="OTH" Text="Others"> </asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Source of Funds</label>
                                                    <asp:DropDownList ID="ddSourceFunds" AutoPostBack="true" OnSelectedIndexChanged="ddSourceFunds_SelectedIndexChanged" runat="server" CssClass="form-control text-uppercase">
                                                        <asp:ListItem></asp:ListItem>
                                                        <asp:ListItem Value="---Select Source of Funds---" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Value="SF1" Text="SalaryHonorarium"> </asp:ListItem>
                                                        <asp:ListItem Value="SF2" Text="Interest/Commission"> </asp:ListItem>
                                                        <asp:ListItem Value="SF3" Text="Business"> </asp:ListItem>
                                                        <asp:ListItem Value="SF4" Text="Pension"> </asp:ListItem>
                                                        <asp:ListItem Value="SF5" Text="OFW Remittance"> </asp:ListItem>
                                                        <asp:ListItem Value="SF6" Text="Other Remittance"> </asp:ListItem>
                                                        <asp:ListItem Value="OTH" Text="Others"> </asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-6" runat="server" id="otherSourceOfFund">
                                                <div class="form-group">
                                                    <label>Specify source of fund: </label>
                                                    <asp:TextBox ID="txtOtherSourceOfFund" runat="server" class="form-control" type="number" placeholder="OTHERS" />
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Occupation</label>
                                                    <asp:DropDownList ID="ddOccupation" runat="server" CssClass="form-control text-uppercase">
                                                        <asp:ListItem></asp:ListItem>
                                                        <asp:ListItem Value="---Select Occupation---" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Value="OP1" Text="Employed"> </asp:ListItem>
                                                        <asp:ListItem Value="OP2" Text="Self-Employed"> </asp:ListItem>
                                                        <asp:ListItem Value="OP3" Text="OFW"> </asp:ListItem>
                                                        <asp:ListItem Value="OP4" Text="Retired"> </asp:ListItem>
                                                        <asp:ListItem Value="OP5" Text="N/A"> </asp:ListItem>
                                                        <asp:ListItem Value="OTH" Text="Others"> </asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Monthly Gross Income</label>
                                                    <asp:DropDownList ID="ddMonthlyIncome" runat="server" CssClass="form-control text-uppercase">
                                                        <asp:ListItem></asp:ListItem>
                                                        <asp:ListItem Value="MI1" Text="Under Php 10,000"> </asp:ListItem>
                                                        <asp:ListItem Value="MI2" Text="Php 10,001 - 20,000"> </asp:ListItem>
                                                        <asp:ListItem Value="MI3" Text="Php 20,001 - 30,000"> </asp:ListItem>
                                                        <asp:ListItem Value="MI4" Text="Php 30,001 - 40,000"> </asp:ListItem>
                                                        <asp:ListItem Value="MI5" Text="Php 40,001 - 50,000"> </asp:ListItem>
                                                        <asp:ListItem Value="MI5" Text="Above Php 50,000"> </asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>--%>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div style="background-color: #468DBC; height: 3px;"></div>
                                        <div class="box-header">
                                            <h3 class="box-title titlemargin" runat="server" id="suppTitle">Supplementary Details</h3>
                                        </div>
                                        <div class="box-body">

                                            <div runat="server" id="compDetails">
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Last Name <span class="color-red fsize-16">*</span></label>
                                                        <%--<asp:TextBox runat="server" AutoPostBack="true" ID="TextBox1" OnTextChanged="tLastName2_TextChanged" class="form-control text-uppercase" type="text" placeholder="DELA CRUZ" />--%>
                                                        <asp:TextBox runat="server" ID="tLastName2" class="form-control text-uppercase" type="text" placeholder="DELA CRUZ" />
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator34"
                                                            ControlToValidate="tLastName2"
                                                            Text="Please fill up Last Name."
                                                            ValidationGroup="Save"
                                                            runat="server" Style="color: red" />
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>First Name <span class="color-red fsize-16">*</span></label>
                                                        <%--<asp:TextBox runat="server" AutoPostBack="true" ID="TextBox1" OnTextChanged="tFirstName2_TextChanged" class="form-control text-uppercase" type="text" placeholder="JUAN" />--%>
                                                        <asp:TextBox runat="server" ID="tFirstName2" class="form-control text-uppercase" type="text" placeholder="JUAN" />
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator35"
                                                            ControlToValidate="tFirstName2"
                                                            Text="Please fill up First Name."
                                                            ValidationGroup="Save"
                                                            runat="server" Style="color: red" />
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="">
                                                        <label>Middle Name <span class="color-red fsize-16">*</span></label>
                                                        <%--<asp:TextBox runat="server" AutoPostBack="true" ID="TextBox1" OnTextChanged="tMiddleName2_TextChanged" class="form-control text-uppercase" type="text" placeholder="D" />--%>
                                                        <asp:TextBox runat="server" ID="tMiddleName2" class="form-control text-uppercase" type="text" placeholder="D" />
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator36"
                                                            ControlToValidate="tMiddleName2"
                                                            Text="Please fill up Middle Name."
                                                            ValidationGroup="Save"
                                                            runat="server" Style="color: red" />
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>No.<%-- <span class="color-red fsize-16">*</span>--%></label>
                                                        <input runat="server" type="text" class="form-control text-uppercase" id="textAuthorizedPersonAddress" placeholder="Address" />
                                                        <%--                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator42"
                                                            ControlToValidate="textAuthorizedPersonAddress"
                                                            Text="Please fill up No."
                                                            ValidationGroup="Save"
                                                            runat="server" Style="color: red" />--%>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Street<%-- <span class="color-red fsize-16">*</span>--%></label>
                                                        <input runat="server" type="text" class="form-control text-uppercase" id="txtAuthorizedPersonStreet" placeholder="Address" />
                                                        <%--<asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator43"
                                                            ControlToValidate="txtAuthorizedPersonStreet"
                                                            Text="Please fill up Street."
                                                            ValidationGroup="Save"
                                                            runat="server" Style="color: red" />--%>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Subdivision<%-- <span class="color-red fsize-16">*</span>--%></label>
                                                        <input runat="server" type="text" class="form-control text-uppercase" id="txtAuthorizedPersonSubdivision" placeholder="Address" />
                                                        <%--<asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator44"
                                                            ControlToValidate="txtAuthorizedPersonSubdivision"
                                                            Text="Please fill up Subdivision."
                                                            ValidationGroup="Save"
                                                            runat="server" Style="color: red" />--%>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Barangay/District<%-- <span class="color-red fsize-16">*</span>--%></label>
                                                        <input runat="server" type="text" class="form-control text-uppercase" id="txtAuthorizedPersonBarangay" placeholder="Address" />
                                                        <%--<asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator45"
                                                            ControlToValidate="txtAuthorizedPersonBarangay"
                                                            Text="Please fill up Barangay/District."
                                                            ValidationGroup="Save"
                                                            runat="server" Style="color: red" />--%>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Municipality/City<%-- <span class="color-red fsize-16">*</span>--%></label>
                                                        <input runat="server" type="text" class="form-control text-uppercase" id="txtAuthorizedPersonCity" placeholder="Address" />
                                                        <%--<asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator46"
                                                            ControlToValidate="txtAuthorizedPersonCity"
                                                            Text="Please fill up Municipality/City."
                                                            ValidationGroup="Save"
                                                            runat="server" Style="color: red" />--%>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Province<%-- <span class="color-red fsize-16">*</span>--%></label>
                                                        <input runat="server" type="text" class="form-control text-uppercase" id="txtAuthorizedPersonProvince" placeholder="Address" />
                                                        <%--<asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator47"
                                                            ControlToValidate="txtAuthorizedPersonProvince"
                                                            Text="Please fill up Province."
                                                            ValidationGroup="Save"
                                                            runat="server" Style="color: red" />--%>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-12">
                                                <div class="row">
                                                    <div runat="server" id="divnoncorp">
                                                        <div class="col-md-6">
                                                            <label>Date of Birth <span class="color-red fsize-16">*</span></label>
                                                            <div class="input-group">
                                                                <div class="input-group-addon">
                                                                    <i class="fa fa-calendar"></i>
                                                                </div>
                                                                <input runat="server" type="date" class="form-control" id="tBirthDate" />
                                                            </div>
                                                            <asp:RequiredFieldValidator
                                                                ID="RequiredFieldValidator33"
                                                                ControlToValidate="tBirthDate"
                                                                Text="Please fill up Date of Birth."
                                                                CssClass="col-md-12"
                                                                ValidationGroup="Save"
                                                                runat="server" Style="color: red" />
                                                            <asp:CustomValidator
                                                                Style="color: red"
                                                                runat="server"
                                                                ID="CustomValidator4"
                                                                CssClass="col-md-12"
                                                                ValidationGroup="Save"
                                                                ControlToValidate="tBirthDate"
                                                                Text="Below legal age."
                                                                OnServerValidate="CustomValidator4_ServerValidate" />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label>Place of Birth</label>
                                                                <input runat="server" type="text" class="form-control text-uppercase" id="tBirthPlace" placeholder="Applicant Birth Place" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-12">
                                                <div class="row">
                                                    <div runat="server" id="divnoncorp2">
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label>Religion</label>
                                                                <asp:TextBox ID="txtReligion" runat="server" class="form-control text-uppercase" type="text" placeholder="Religion" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label>Gender</label>
                                                                <select runat="server" id="tGender" class="form-control">
                                                                    <option value="">-- Select One --</option>
                                                                    <option value="M">Male</option>
                                                                    <option value="F">Female</option>
                                                                </select>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label>Citizenship</label>
                                                                <select runat="server" id="tCitizenship" name="nationality" class="form-control">
                                                                    <option value="">-- Select One --</option>
                                                                    <option value="afghan">Afghan</option>
                                                                    <option value="albanian">Albanian</option>
                                                                    <option value="algerian">Algerian</option>
                                                                    <option value="american">American</option>
                                                                    <option value="andorran">Andorran</option>
                                                                    <option value="angolan">Angolan</option>
                                                                    <option value="antiguans">Antiguans</option>
                                                                    <option value="argentinean">Argentinean</option>
                                                                    <option value="armenian">Armenian</option>
                                                                    <option value="australian">Australian</option>
                                                                    <option value="austrian">Austrian</option>
                                                                    <option value="azerbaijani">Azerbaijani</option>
                                                                    <option value="bahamian">Bahamian</option>
                                                                    <option value="bahraini">Bahraini</option>
                                                                    <option value="bangladeshi">Bangladeshi</option>
                                                                    <option value="barbadian">Barbadian</option>
                                                                    <option value="barbudans">Barbudans</option>
                                                                    <option value="batswana">Batswana</option>
                                                                    <option value="belarusian">Belarusian</option>
                                                                    <option value="belgian">Belgian</option>
                                                                    <option value="belizean">Belizean</option>
                                                                    <option value="beninese">Beninese</option>
                                                                    <option value="bhutanese">Bhutanese</option>
                                                                    <option value="bolivian">Bolivian</option>
                                                                    <option value="bosnian">Bosnian</option>
                                                                    <option value="brazilian">Brazilian</option>
                                                                    <option value="british">British</option>
                                                                    <option value="bruneian">Bruneian</option>
                                                                    <option value="bulgarian">Bulgarian</option>
                                                                    <option value="burkinabe">Burkinabe</option>
                                                                    <option value="burmese">Burmese</option>
                                                                    <option value="burundian">Burundian</option>
                                                                    <option value="cambodian">Cambodian</option>
                                                                    <option value="cameroonian">Cameroonian</option>
                                                                    <option value="canadian">Canadian</option>
                                                                    <option value="cape verdean">Cape Verdean</option>
                                                                    <option value="central african">Central African</option>
                                                                    <option value="chadian">Chadian</option>
                                                                    <option value="chilean">Chilean</option>
                                                                    <option value="chinese">Chinese</option>
                                                                    <option value="colombian">Colombian</option>
                                                                    <option value="comoran">Comoran</option>
                                                                    <option value="congolese">Congolese</option>
                                                                    <option value="costa rican">Costa Rican</option>
                                                                    <option value="croatian">Croatian</option>
                                                                    <option value="cuban">Cuban</option>
                                                                    <option value="cypriot">Cypriot</option>
                                                                    <option value="czech">Czech</option>
                                                                    <option value="danish">Danish</option>
                                                                    <option value="djibouti">Djibouti</option>
                                                                    <option value="dominican">Dominican</option>
                                                                    <option value="dutch">Dutch</option>
                                                                    <option value="east timorese">East Timorese</option>
                                                                    <option value="ecuadorean">Ecuadorean</option>
                                                                    <option value="egyptian">Egyptian</option>
                                                                    <option value="emirian">Emirian</option>
                                                                    <option value="equatorial guinean">Equatorial Guinean</option>
                                                                    <option value="eritrean">Eritrean</option>
                                                                    <option value="estonian">Estonian</option>
                                                                    <option value="ethiopian">Ethiopian</option>
                                                                    <option value="fijian">Fijian</option>
                                                                    <option value="filipino">Filipino</option>
                                                                    <option value="finnish">Finnish</option>
                                                                    <option value="french">French</option>
                                                                    <option value="gabonese">Gabonese</option>
                                                                    <option value="gambian">Gambian</option>
                                                                    <option value="georgian">Georgian</option>
                                                                    <option value="german">German</option>
                                                                    <option value="ghanaian">Ghanaian</option>
                                                                    <option value="greek">Greek</option>
                                                                    <option value="grenadian">Grenadian</option>
                                                                    <option value="guatemalan">Guatemalan</option>
                                                                    <option value="guinea-bissauan">Guinea-Bissauan</option>
                                                                    <option value="guinean">Guinean</option>
                                                                    <option value="guyanese">Guyanese</option>
                                                                    <option value="haitian">Haitian</option>
                                                                    <option value="herzegovinian">Herzegovinian</option>
                                                                    <option value="honduran">Honduran</option>
                                                                    <option value="hungarian">Hungarian</option>
                                                                    <option value="icelander">Icelander</option>
                                                                    <option value="indian">Indian</option>
                                                                    <option value="indonesian">Indonesian</option>
                                                                    <option value="iranian">Iranian</option>
                                                                    <option value="iraqi">Iraqi</option>
                                                                    <option value="irish">Irish</option>
                                                                    <option value="israeli">Israeli</option>
                                                                    <option value="italian">Italian</option>
                                                                    <option value="ivorian">Ivorian</option>
                                                                    <option value="jamaican">Jamaican</option>
                                                                    <option value="japanese">Japanese</option>
                                                                    <option value="jordanian">Jordanian</option>
                                                                    <option value="kazakhstani">Kazakhstani</option>
                                                                    <option value="kenyan">Kenyan</option>
                                                                    <option value="kittian and nevisian">Kittian and Nevisian</option>
                                                                    <option value="kuwaiti">Kuwaiti</option>
                                                                    <option value="kyrgyz">Kyrgyz</option>
                                                                    <option value="laotian">Laotian</option>
                                                                    <option value="latvian">Latvian</option>
                                                                    <option value="lebanese">Lebanese</option>
                                                                    <option value="liberian">Liberian</option>
                                                                    <option value="libyan">Libyan</option>
                                                                    <option value="liechtensteiner">Liechtensteiner</option>
                                                                    <option value="lithuanian">Lithuanian</option>
                                                                    <option value="luxembourger">Luxembourger</option>
                                                                    <option value="macedonian">Macedonian</option>
                                                                    <option value="malagasy">Malagasy</option>
                                                                    <option value="malawian">Malawian</option>
                                                                    <option value="malaysian">Malaysian</option>
                                                                    <option value="maldivan">Maldivan</option>
                                                                    <option value="malian">Malian</option>
                                                                    <option value="maltese">Maltese</option>
                                                                    <option value="marshallese">Marshallese</option>
                                                                    <option value="mauritanian">Mauritanian</option>
                                                                    <option value="mauritian">Mauritian</option>
                                                                    <option value="mexican">Mexican</option>
                                                                    <option value="micronesian">Micronesian</option>
                                                                    <option value="moldovan">Moldovan</option>
                                                                    <option value="monacan">Monacan</option>
                                                                    <option value="mongolian">Mongolian</option>
                                                                    <option value="moroccan">Moroccan</option>
                                                                    <option value="mosotho">Mosotho</option>
                                                                    <option value="motswana">Motswana</option>
                                                                    <option value="mozambican">Mozambican</option>
                                                                    <option value="namibian">Namibian</option>
                                                                    <option value="nauruan">Nauruan</option>
                                                                    <option value="nepalese">Nepalese</option>
                                                                    <option value="new zealander">New Zealander</option>
                                                                    <option value="ni-vanuatu">Ni-Vanuatu</option>
                                                                    <option value="nicaraguan">Nicaraguan</option>
                                                                    <option value="nigerien">Nigerien</option>
                                                                    <option value="north korean">North Korean</option>
                                                                    <option value="northern irish">Northern Irish</option>
                                                                    <option value="norwegian">Norwegian</option>
                                                                    <option value="omani">Omani</option>
                                                                    <option value="pakistani">Pakistani</option>
                                                                    <option value="palauan">Palauan</option>
                                                                    <option value="panamanian">Panamanian</option>
                                                                    <option value="papua new guinean">Papua New Guinean</option>
                                                                    <option value="paraguayan">Paraguayan</option>
                                                                    <option value="peruvian">Peruvian</option>
                                                                    <option value="polish">Polish</option>
                                                                    <option value="portuguese">Portuguese</option>
                                                                    <option value="qatari">Qatari</option>
                                                                    <option value="romanian">Romanian</option>
                                                                    <option value="russian">Russian</option>
                                                                    <option value="rwandan">Rwandan</option>
                                                                    <option value="saint lucian">Saint Lucian</option>
                                                                    <option value="salvadoran">Salvadoran</option>
                                                                    <option value="samoan">Samoan</option>
                                                                    <option value="san marinese">San Marinese</option>
                                                                    <option value="sao tomean">Sao Tomean</option>
                                                                    <option value="saudi">Saudi</option>
                                                                    <option value="scottish">Scottish</option>
                                                                    <option value="senegalese">Senegalese</option>
                                                                    <option value="serbian">Serbian</option>
                                                                    <option value="seychellois">Seychellois</option>
                                                                    <option value="sierra leonean">Sierra Leonean</option>
                                                                    <option value="singaporean">Singaporean</option>
                                                                    <option value="slovakian">Slovakian</option>
                                                                    <option value="slovenian">Slovenian</option>
                                                                    <option value="solomon islander">Solomon Islander</option>
                                                                    <option value="somali">Somali</option>
                                                                    <option value="south african">South African</option>
                                                                    <option value="south korean">South Korean</option>
                                                                    <option value="spanish">Spanish</option>
                                                                    <option value="sri lankan">Sri Lankan</option>
                                                                    <option value="sudanese">Sudanese</option>
                                                                    <option value="surinamer">Surinamer</option>
                                                                    <option value="swazi">Swazi</option>
                                                                    <option value="swedish">Swedish</option>
                                                                    <option value="swiss">Swiss</option>
                                                                    <option value="syrian">Syrian</option>
                                                                    <option value="taiwanese">Taiwanese</option>
                                                                    <option value="tajik">Tajik</option>
                                                                    <option value="tanzanian">Tanzanian</option>
                                                                    <option value="thai">Thai</option>
                                                                    <option value="togolese">Togolese</option>
                                                                    <option value="tongan">Tongan</option>
                                                                    <option value="trinidadian or tobagonian">Trinidadian or Tobagonian</option>
                                                                    <option value="tunisian">Tunisian</option>
                                                                    <option value="turkish">Turkish</option>
                                                                    <option value="tuvaluan">Tuvaluan</option>
                                                                    <option value="ugandan">Ugandan</option>
                                                                    <option value="ukrainian">Ukrainian</option>
                                                                    <option value="uruguayan">Uruguayan</option>
                                                                    <option value="uzbekistani">Uzbekistani</option>
                                                                    <option value="venezuelan">Venezuelan</option>
                                                                    <option value="vietnamese">Vietnamese</option>
                                                                    <option value="welsh">Welsh</option>
                                                                    <option value="yemenite">Yemenite</option>
                                                                    <option value="zambian">Zambian</option>
                                                                    <option value="zimbabwean">Zimbabwean</option>
                                                                </select>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>Email Address</label>
                                                            <input runat="server" type="email" class="form-control text-lowercase" id="tEmpEmailAddress" placeholder="xxxx@hotmail.com" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>Home Tel No.</label>
                                                            <input runat="server" type="text" class="form-control" onkeypress="return fnAllowNumeric(event)" id="tHomeTelNo" maxlength="13" placeholder="xx xxx xxxx" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>Cellphone No.</label>
                                                            <input runat="server" type="text" class="form-control" id="tCellphoneNo" onkeypress="return fnAllowNumeric(event)" maxlength="16" placeholder="xxxx xxx xxxx" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>Facebook Account</label>
                                                            <input runat="server" type="email" class="form-control text-lowercase" id="tFBAccount" placeholder="xxxx@hotmail.com" />
                                                        </div>
                                                    </div>






                                                </div>















                                                <%--VALID IDS--%>




                                                <div class="col-lg-12">
                                                    <div class="row">

                                                        <div class="col-md-6" runat="server" id="ddothers1">
                                                            <div class="form-group">
                                                                <label>Type of ID <span class="color-red fsize-16">*</span></label>
                                                                <asp:DropDownList ID="tTypeOfId" runat="server" CssClass="form-control" DataTextField="Name" DataValueField="Code"
                                                                    OnSelectedIndexChanged="tTypeOfId_SelectedIndexChanged" AutoPostBack="true">
                                                                    <asp:ListItem Value="---Select Type of ID---" Selected="True"></asp:ListItem>
                                                                    <%--                                                            <asp:ListItem Value="ID1" Text="TIN"> </asp:ListItem>
                                                            <asp:ListItem Value="ID2" Text="SSS"> </asp:ListItem>
                                                            <asp:ListItem Value="ID3" Text="Pagibig"> </asp:ListItem>
                                                            <asp:ListItem Value="ID4" Text="Passport"> </asp:ListItem>
                                                            <asp:ListItem Value="OTH" Text="Others"> </asp:ListItem>--%>
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator5"
                                                                    ControlToValidate="tTypeOfId"
                                                                    Text="Please select Type of ID."
                                                                    InitialValue="---Select Type of ID---"
                                                                    ValidationGroup="Save"
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>



                                                        <div class="col-md-3" runat="server" id="others1" visible="false">
                                                            <div class="form-group">
                                                                <%-- <asp:Label runat="server" ID="Label2" Text="ID No."></asp:Label>--%>
                                                                <label>Specify ID</label>
                                                                <asp:TextBox runat="server" ID="txtOthers1" class="form-control text-uppercase" type="text" placeholder="ID" />
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator59"
                                                                    ControlToValidate="txtOthers1"
                                                                    ValidationGroup="Next"
                                                                    Text="Please specify ID."
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>


                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label>ID Number <span class="color-red fsize-16">*</span></label>
                                                                <%--<input type="text" style="z-index: auto;" onkeyup="ValidTIN_keyup(event)" class="form-control text-uppercase" runat="server" id="tIDNo" required />--%>
                                                                <%--<asp:TextBox Style="z-index: auto;" AutoPostBack="true" OnTextChanged="tIDNo_TextChanged" onkeyup="Valid
                                                                    (event)" class="form-control text-uppercase" runat="server" ID="TextBox1"></asp:TextBox>--%>
                                                                <asp:TextBox Style="z-index: auto;" onkeyup="ValidTIN_keyup(event)" AutoPostBack="true" OnTextChanged="tIDNo_TextChanged" class="form-control text-uppercase" runat="server" ID="tIDNo"></asp:TextBox>
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator6"
                                                                    ControlToValidate="tIDNo"
                                                                    Text="Please fill up Type of ID."
                                                                    ValidationGroup="Save"
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>










                                                <div class="col-lg-12">
                                                    <div class="row">

                                                        <div class="col-md-6" runat="server" id="ddothers2">
                                                            <div class="form-group">
                                                                <label>
                                                                    Type of ID 2 <%--<span class="color-red fsize-16">*</span>--%>
                                                                </label>
                                                                <asp:DropDownList ID="tTypeOfId2" runat="server" CssClass="form-control" DataTextField="Name" DataValueField="Code"
                                                                    OnSelectedIndexChanged="tTypeOfId2_SelectedIndexChanged" AutoPostBack="true">
                                                                    <asp:ListItem Value="---Select Type of ID---" Selected="True"></asp:ListItem>
                                                                    <%--                                                            <asp:ListItem Value="ID1" Text="TIN"> </asp:ListItem>
                                                            <asp:ListItem Value="ID2" Text="SSS"> </asp:ListItem>
                                                            <asp:ListItem Value="ID3" Text="Pagibig"> </asp:ListItem>
                                                            <asp:ListItem Value="ID4" Text="Passport"> </asp:ListItem>
                                                            <asp:ListItem Value="OTH" Text="Others"> </asp:ListItem>--%>
                                                                </asp:DropDownList>
                                                                <%--COMMENTED 04-04-2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                                                                <%--    <asp:RequiredFieldValidator
                                                                    ID="rvTypeofId2"
                                                                    ControlToValidate="tTypeOfId2"
                                                                    Text="Please fill up Type of ID."
                                                                    InitialValue="---Select Type of ID---"
                                                                    ValidationGroup="Save"
                                                                    runat="server" Style="color: red" />--%>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-3" runat="server" id="others2" visible="false">
                                                            <div class="form-group">
                                                                <%-- <asp:Label runat="server" ID="Label2" Text="ID No."></asp:Label>--%>
                                                                <label>Specify ID</label>
                                                                <asp:TextBox runat="server" ID="txtOthers2" class="form-control text-uppercase" type="text" placeholder="ID" />
                                                                <%--COMMENTED 04-04-2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                                                                <%--   <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator12"
                                                                    ControlToValidate="txtOthers2"
                                                                    ValidationGroup="Next"
                                                                    Text="Please specify ID."
                                                                    runat="server" Style="color: red" />--%>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label>
                                                                    ID Number 2 
                                                                    <%--<span class="color-red fsize-16">*</span>--%>
                                                                </label>
                                                                <%--<input type="text" style="z-index: auto;" onkeyup="ValidTIN_keyup(event)" class="form-control text-uppercase" runat="server" id="tIDNo2" required />--%>
                                                                <%--<asp:TextBox Style="z-index: auto;" AutoPostBack="true" OnTextChanged=" tIDNo2_TextChanged" onkeyup="ValidTIN_keyup(event)" class="form-control text-uppercase" runat="server" ID="TextBox1"></asp:TextBox>--%>
                                                                <asp:TextBox Style="z-index: auto;" onkeyup="ValidTIN_keyup(event)" AutoPostBack="true" OnTextChanged="tIDNo2_TextChanged" class="form-control text-uppercase" runat="server" ID="tIDNo2"></asp:TextBox>
                                                                <%--COMMENTED 04-04-2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                                                                <%-- <asp:RequiredFieldValidator
                                                                    ID="rvIDNo2"
                                                                    ControlToValidate="tIDNo2"
                                                                    Text="Please fill up Type of ID."
                                                                    ValidationGroup="Save"
                                                                    runat="server" Style="color: red" />--%>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>








                                                <div class="col-lg-12 hidden">
                                                    <div class="row">

                                                        <div class="col-md-6" runat="server" id="ddothers3">
                                                            <div class="form-group">
                                                                <label>Type of ID 3</label>
                                                                <asp:DropDownList ID="tTypeOfId3" runat="server" CssClass="form-control" DataTextField="Name" DataValueField="Code"
                                                                    OnSelectedIndexChanged="tTypeOfId3_SelectedIndexChanged" AutoPostBack="true">
                                                                    <asp:ListItem Value="---Select Type of ID---" Selected="True"></asp:ListItem>
                                                                    <%--                                                            <asp:ListItem Value="ID1" Text="TIN"> </asp:ListItem>
                                                            <asp:ListItem Value="ID2" Text="SSS"> </asp:ListItem>
                                                            <asp:ListItem Value="ID3" Text="Pagibig"> </asp:ListItem>
                                                            <asp:ListItem Value="ID4" Text="Passport"> </asp:ListItem>
                                                            <asp:ListItem Value="OTH" Text="Others"> </asp:ListItem>--%>
                                                                </asp:DropDownList>
                                                                <%--<asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator11"
                                                            ControlToValidate="tTypeOfId3"
                                                            Text="Please fill up Type of ID."
                                                            InitialValue="---Select Type of ID---"
                                                            ValidationGroup="Save"
                                                            runat="server" Style="color: red" />--%>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-3" runat="server" id="others3" visible="false">
                                                            <div class="form-group">
                                                                <%-- <asp:Label runat="server" ID="Label2" Text="ID No."></asp:Label>--%>
                                                                <label>Specify ID</label>
                                                                <asp:TextBox runat="server" ID="txtOthers3" class="form-control text-uppercase" type="text" placeholder="ID" />
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator39"
                                                                    ControlToValidate="txtOthers3"
                                                                    ValidationGroup="Next"
                                                                    Text="Please specify ID."
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>

                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label>ID Number 3</label>
                                                                <%--<input type="text" style="z-index: auto;" onkeyup="ValidTIN_keyup(event)" class="form-control text-uppercase" runat="server" id="tIDNo3" required />--%>
                                                                <asp:TextBox Style="z-index: auto;" AutoPostBack="true" OnTextChanged="tIDNo3_TextChanged" onkeyup="ValidTIN_keyup(event)" class="form-control text-uppercase" runat="server" ID="tIDNo3"></asp:TextBox>
                                                                <%--<asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator12"
                                                            ControlToValidate="tIDNo3"
                                                            Text="Please fill up Type of ID."
                                                            ValidationGroup="Save"
                                                            runat="server" Style="color: red" />--%>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>










                                                <div class="col-lg-12 hidden">
                                                    <div class="row">
                                                        <div class="col-md-6" runat="server" id="ddothers4">
                                                            <div class="form-group">
                                                                <label>Type of ID 4</label>
                                                                <asp:DropDownList ID="tTypeOfId4" runat="server" CssClass="form-control" DataTextField="Name" DataValueField="Code"
                                                                    OnSelectedIndexChanged="tTypeOfId4_SelectedIndexChanged" AutoPostBack="true">
                                                                    <asp:ListItem Value="---Select Type of ID---" Selected="True"></asp:ListItem>
                                                                    <%--                                                            <asp:ListItem Value="ID1" Text="TIN"> </asp:ListItem>
                                                            <asp:ListItem Value="ID2" Text="SSS"> </asp:ListItem>
                                                            <asp:ListItem Value="ID3" Text="Pagibig"> </asp:ListItem>
                                                            <asp:ListItem Value="ID4" Text="Passport"> </asp:ListItem>
                                                            <asp:ListItem Value="OTH" Text="Others"> </asp:ListItem>--%>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-3" runat="server" id="others4" visible="false">
                                                            <div class="form-group">
                                                                <%-- <asp:Label runat="server" ID="Label2" Text="ID No."></asp:Label>--%>
                                                                <label>Specify ID</label>
                                                                <asp:TextBox runat="server" ID="txtOthers4" class="form-control text-uppercase" type="text" placeholder="ID" />
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator40"
                                                                    ControlToValidate="txtOthers4"
                                                                    ValidationGroup="Next"
                                                                    Text="Please specify ID."
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>

                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label>ID Number 4</label>
                                                                <%--<input type="text" style="z-index: auto;" onkeyup="ValidTIN_keyup(event)" class="form-control text-uppercase" runat="server" id="tIDNo4" required />--%>
                                                                <asp:TextBox Style="z-index: auto;" AutoPostBack="true" OnTextChanged="tIDNo4_TextChanged" onkeyup="ValidTIN_keyup(event)" class="form-control text-uppercase" runat="server" ID="tIDNo4"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>



                                                <%--END OF VALID IDS--%>





























                                                <div runat="server" id="divnoncorp3">
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>Civil Status</label>
                                                            <asp:DropDownList ID="tCivilStatus" AutoPostBack="true" OnSelectedIndexChanged="ddCivilStatus_SelectedIndexChanged" runat="server" CssClass="form-control">
                                                                <asp:ListItem Value="---Select Civil Status---" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Value="CS1" Text="Single"> </asp:ListItem>
                                                                <asp:ListItem Value="CS2" Text="Married"> </asp:ListItem>

                                                                <%-- 2023-06-14 : REMOVED WIDOW AND OTHERS; RETAIN SINGLE AND MARRIED --%>
                                                                <%--<asp:ListItem Value="CS3" Text="Widow"> </asp:ListItem>--%>
                                                                <%--<asp:ListItem Value="CS4" Text="Legally Separated"> </asp:ListItem>--%>
                                                                <%--<asp:ListItem Value="OTH" Text="Others"> </asp:ListItem>--%>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>SSS No.</label>
                                                        <%--<asp:TextBox runat="server" AutoPostBack="true" OnTextChanged="tSSSNo_TextChanged" class="form-control" ID="TextBox1" MaxLength="12" onkeypress="return fnAllowNumeric(event)" placeholder="xx xxxxxxx x"></asp:TextBox>--%>
                                                        <asp:TextBox runat="server" class="form-control" ID="tSSSNo" MaxLength="12" onkeypress="return fnAllowNumeric(event)" placeholder="xx xxxxxxx x"></asp:TextBox>
                                                        <%--<input runat="server" type="text" class="form-control" id="tSSSNo" maxlength="12" onkeypress="return fnAllowNumeric(event)" placeholder="xx xxxxxxx x" />--%>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>GSIS No.</label>
                                                        <input runat="server" type="text" class="form-control" id="tGSISNo" maxlength="14" onkeypress="return fnAllowNumeric(event)" placeholder="xxxx xxxxxxx x" />
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>PAG-IBIG No.</label>
                                                        <%--<asp:TextBox runat="server" AutoPostBack="true" OnTextChanged="tPagibigNo_TextChanged" class="form-control" ID="TextBox1" MaxLength="14" onkeypress="return fnAllowNumeric(event)" placeholder="xxxx xxxxxxx x"></asp:TextBox>--%>
                                                        <asp:TextBox runat="server" class="form-control" ID="tPagibigNo" MaxLength="14" onkeypress="return fnAllowNumeric(event)" placeholder="xxxx xxxxxxx x"></asp:TextBox>
                                                        <%--<input runat="server" type="text" class="form-control" id="tPagibigNo" maxlength="14" onkeypress="return fnAllowNumeric(event)" placeholder="xxxx xxxxxxx x" />--%>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Business Phone No.</label>
                                                        <input runat="server" type="text" class="form-control" id="txtBusinessPhoneNo" onkeypress="return fnAllowNumeric(event)" maxlength="50" placeholder="xxxx xxx xxxx" />
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Profession</label>
                                                        <asp:TextBox ID="txtProfession" runat="server" class="form-control text-uppercase" type="text" placeholder="Engineer/Lawyer/Architect" />
                                                    </div>
                                                </div>
                                                <%--omit the “Occupation” and the “Employment Status” fields.--%>
                                                <div class="col-md-6 hidden">
                                                    <div class="form-group">
                                                        <label>Occupation</label>
                                                        <asp:DropDownList ID="ddOccupation" runat="server" CssClass="form-control text-uppercase">
                                                            <asp:ListItem></asp:ListItem>
                                                            <asp:ListItem Value="---Select Occupation---" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Value="OP1" Text="Employed"> </asp:ListItem>
                                                            <asp:ListItem Value="OP2" Text="Self-Employed"> </asp:ListItem>
                                                            <asp:ListItem Value="OP3" Text="OFW"> </asp:ListItem>
                                                            <asp:ListItem Value="OP4" Text="Retired"> </asp:ListItem>
                                                            <asp:ListItem Value="OP5" Text="N/A"> </asp:ListItem>
                                                            <asp:ListItem Value="OTH" Text="Others"> </asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-6 hidden" runat="server" id="empstat">
                                                    <div class="form-group">
                                                        <label>Employment Status</label>
                                                        <asp:DropDownList ID="tEmpStatus" runat="server" CssClass="form-control text-uppercase">
                                                            <asp:ListItem></asp:ListItem>
                                                            <asp:ListItem Value="---Select Employment Status---" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Value="ES1" Text="Casual"> </asp:ListItem>
                                                            <asp:ListItem Value="ES2" Text="Contractual"> </asp:ListItem>
                                                            <asp:ListItem Value="ES3" Text="Probationary"> </asp:ListItem>
                                                            <asp:ListItem Value="ES4" Text="Permanent"> </asp:ListItem>
                                                            <asp:ListItem Value="ES5" Text="Consultant"> </asp:ListItem>
                                                            <asp:ListItem Value="OTH" Text="Others"> </asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <%--omit the “Occupation” and the “Employment Status” fields.--%>
                                                <div runat="server" id="sourcefunds">
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <%--KARL - Added Validator to source of funds 2023/05/04--%>
                                                            <label>Source of Funds <span class="color-red fsize-16">*</span></label>
                                                            <asp:DropDownList ID="ddSourceFunds" AutoPostBack="true" DataTextField="Name" DataValueField="Code" OnSelectedIndexChanged="ddSourceFunds_SelectedIndexChanged" runat="server" CssClass="form-control text-uppercase">
                                                                <%--                                   <asp:ListItem></asp:ListItem>
                                                                <asp:ListItem Value="---Select Source of Funds---" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Value="SF1" Text="SalaryHonorarium"> </asp:ListItem>
                                                                <asp:ListItem Value="SF2" Text="Interest/Commission"> </asp:ListItem>
                                                                <asp:ListItem Value="SF3" Text="Business"> </asp:ListItem>
                                                                <asp:ListItem Value="SF4" Text="Pension"> </asp:ListItem>
                                                                <asp:ListItem Value="SF5" Text="OFW Remittance"> </asp:ListItem>
                                                                <asp:ListItem Value="SF6" Text="Other Remittance"> </asp:ListItem>
                                                                <asp:ListItem Value="SF7" Text="Self Employed"> </asp:ListItem>
                                                                <asp:ListItem Value="OTH" Text="Others"> </asp:ListItem>--%>
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator
                                                                ID="ddSourceFunds_RequiredFieldValidator"
                                                                ControlToValidate="ddSourceFunds" CssClass="col-md-12"
                                                                Text="Please select Source of Funds."
                                                                Enabled="false"
                                                                InitialValue=""
                                                                ValidationGroup="Save"
                                                                runat="server" Style="color: red" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6" runat="server" id="otherSourceOfFund">
                                                        <div class="form-group">
                                                            <label>Specify source of fund: </label>
                                                            <asp:TextBox ID="txtOtherSourceOfFund" runat="server" class="form-control" type="text" placeholder="OTHERS" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Monthly Gross Income</label>
                                                        <asp:DropDownList ID="ddMonthlyIncome" runat="server" CssClass="form-control text-uppercase">
                                                            <asp:ListItem></asp:ListItem>
                                                            <%--<asp:ListItem Value="---Select Source of Funds---" Selected="True"></asp:ListItem>--%>
                                                            <asp:ListItem Value="MI1" Text="Under Php 10,000"> </asp:ListItem>
                                                            <asp:ListItem Value="MI2" Text="Php 10,001 - 20,000"> </asp:ListItem>
                                                            <asp:ListItem Value="MI3" Text="Php 20,001 - 30,000"> </asp:ListItem>
                                                            <asp:ListItem Value="MI4" Text="Php 30,001 - 40,000"> </asp:ListItem>
                                                            <asp:ListItem Value="MI5" Text="Php 40,001 - 50,000"> </asp:ListItem>
                                                            <asp:ListItem Value="MI6" Text="Above Php 50,000"> </asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6" runat="server" id="contactDetails">
                                        <div class="box box-primary">
                                            <div class="box-header">
                                                <h3 class="box-title" runat="server" id="contactTitle">Contact Person Details</h3>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-md-12">
                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label>First Name <span class="color-red fsize-16">*</span></label>
                                                                <asp:TextBox ID="txtContactFName" type="text" ValidationGroup="Next" class="form-control text-uppercase" runat="server" placeholder="Juan" />
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator13"
                                                                    ControlToValidate="txtContactFName" CssClass="col-md-12"
                                                                    Text="Please fill up First Name."
                                                                    ValidationGroup="Save"
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label>Middle Name <span class="color-red fsize-16">*</span></label>
                                                                <asp:TextBox runat="server" ID="txtContactMName" class="form-control text-uppercase" type="text" ValidationGroup="Next" placeholder="D" />
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator14"
                                                                    ControlToValidate="txtContactMName" CssClass="col-md-12"
                                                                    Text="Please fill up Middle Name."
                                                                    ValidationGroup="Save"
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label>Last Name <span class="color-red fsize-16">*</span></label>
                                                                <asp:TextBox runat="server" ID="txtContactLName" ValidationGroup="Next" class="form-control text-uppercase" type="text" placeholder="Dela Cruz" />
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator15"
                                                                    ControlToValidate="txtContactLName" CssClass="col-md-12"
                                                                    Text="Please fill up Last Name."
                                                                    ValidationGroup="Save"
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label>Relationship to Principal Buyer <span class="color-red fsize-16">*</span></label>
                                                                <asp:TextBox runat="server" ID="txtContactPersonPosition" type="text" placeholder="" class="form-control" />
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator21"
                                                                    ControlToValidate="txtContactPersonPosition" CssClass="col-md-12"
                                                                    Text="Please fill up Relationship to Principal Buyer."
                                                                    ValidationGroup="Save"
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label>Email</label>
                                                                <asp:TextBox runat="server" ID="txtContactEmail" type="text" placeholder="Juan@gmail.com" class="form-control" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label>Mobile No</label>
                                                                <asp:TextBox runat="server" ID="txtContactMobile" ValidationGroup="Next" class="form-control text-uppercase" oninput="this.value=this.value.slice(0,this.maxLength)" MaxLength="11" type="number" placeholder="09000000000" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="form-group">
                                                                <label>Address</label>
                                                                <asp:TextBox runat="server" ID="txtContactAddress" class="form-control text-uppercase" type="text" ValidationGroup="Next"
                                                                    placeholder="Ermita, Manila,1000 Metro Manila" />
                                                            </div>
                                                        </div>

                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="form-group">
                                                                <label>Residence</label>
                                                                <asp:TextBox runat="server" ID="txtContactResidence" class="form-control text-uppercase" ValidationGroup="Next" type="text" placeholder="" />
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label>Valid ID <span class="color-red fsize-16">*</span></label>
                                                                <asp:DropDownList ID="ddContactValidID" runat="server" DataTextField="Name" DataValueField="Code" CssClass="form-control">
                                                                    <asp:ListItem Value="---Select Valid ID---" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Value="ID1" Text="TIN"> </asp:ListItem>
                                                                    <asp:ListItem Value="ID2" Text="SSS"> </asp:ListItem>
                                                                    <asp:ListItem Value="ID3" Text="Pagibig"> </asp:ListItem>
                                                                    <asp:ListItem Value="ID4" Text="Passport"> </asp:ListItem>
                                                                    <asp:ListItem Value="OTH" Text="Others"> </asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator24"
                                                                    ControlToValidate="ddContactValidID" CssClass="col-md-12"
                                                                    Text="Please select Valid ID."
                                                                    InitialValue="---Select Valid ID---"
                                                                    ValidationGroup="Save"
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label>Valid ID No <span class="color-red fsize-16">*</span></label>
                                                                <asp:TextBox runat="server" ID="txtContactValidIDNo" ValidationGroup="Next" class="form-control" type="text" placeholder="12345678" />
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator23"
                                                                    ControlToValidate="txtContactValidIDNo" CssClass="col-md-12"
                                                                    Text="Please fill up Valid ID No."
                                                                    ValidationGroup="Save"
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="box box-primary col-md-12" runat="server" id="empDetails">
                                        <div class="box-header titlemargin">
                                            <h3 class="box-title">Employer Details</h3>
                                        </div>
                                        <div class="box-body">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Employer Business Name</label>
                                                    <input runat="server" type="text" class="form-control text-uppercase" id="tEmpBusinessName" placeholder="Applicant Employer's Business Name" />
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Employer Business Address</label>
                                                    <input runat="server" type="text" class="form-control text-uppercase" id="tEmpBusinessAddress" placeholder="Applicant Employer's Business Address" />
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <%--KARL - Added Validator to position 2023/05/04--%>
                                                    <label>Position <span class="color-red fsize-16">*</span></label>
                                                    <input runat="server" type="text" class="form-control text-uppercase" id="tPosition" placeholder="Applicant Position" />
                                                    <asp:RequiredFieldValidator
                                                        ID="tPosition_RequiredFieldValidator"
                                                        ControlToValidate="tPosition" CssClass="col-md-12"
                                                        Text="Please fill up Position."
                                                        ValidationGroup="Save"
                                                        Enabled="false"
                                                        runat="server" Style="color: red" />
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Years of Service</label>
                                                    <input runat="server" type="text" onkeypress="return isNumberKey(event)" maxlength="5" class="form-control" id="tYearsofService" value="0" />
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Office Telephone No.</label>
                                                    <input runat="server" type="text" class="form-control" id="tOfficeTelNo" onkeypress="return fnAllowNumeric(event)" placeholder="xx xxx xxxx" />
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Fax No.</label>
                                                    <input runat="server" type="text" class="form-control" id="tFAXNo" maxlength="14" onkeypress="return fnAllowNumeric(event)" placeholder="x xxx xxx xxxx" />
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Nature of Employment <span class="color-red fsize-16">*</span></label>
                                                    <asp:DropDownList ID="tNatureEmployment" runat="server" CssClass="form-control">
                                                        <%--<asp:ListItem></asp:ListItem>--%>
                                                        <asp:ListItem Value="---Select Nature of Employment---" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Value="NE1" Text="Private Sector"> </asp:ListItem>
                                                        <%--<asp:ListItem Value="NE2" Text="Self Employed"> </asp:ListItem>--%>
                                                        <asp:ListItem Value="NE3" Text="Government"> </asp:ListItem>
                                                        <asp:ListItem Value="NE4" Text="Retired"> </asp:ListItem>
                                                        <asp:ListItem Value="NE5" Text="Overseas"> </asp:ListItem>
                                                        <asp:ListItem Value="OTH" Text="Others"> </asp:ListItem>
                                                    </asp:DropDownList>
                                                    <%--KARL - Adust Validator of Nature of employment 
                                                        (must block "" and "---Select Nature of Employment---" values) 
                                                        2023/05/04--%>
                                                    <%--                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator4"
                                                        ControlToValidate="tNatureEmployment"
                                                        Text="Please select Nature of Employment."
                                                        ValidationExpression="^$|^---Select Nature of Employment---$"
                                                        ValidationGroup="Save"
                                                        runat="server" Style="color: red" />--%>
                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator4"
                                                        ControlToValidate="tNatureEmployment"
                                                        Text="Please select Nature of Employment."
                                                        InitialValue="---Select Nature of Employment---"
                                                        ValidationGroup="Save"
                                                        runat="server" Style="color: red" />

                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Business Country</label>
                                                    <asp:DropDownList ID="ddEmployCountry" runat="server" CssClass="form-control text-uppercase">
                                                        <asp:ListItem Selected="true" Value="PH">Philippines</asp:ListItem>
                                                        <asp:ListItem Value="AF">Afghanistan</asp:ListItem>
                                                        <asp:ListItem Value="AX">Aland Islands</asp:ListItem>
                                                        <asp:ListItem Value="AL">Albania</asp:ListItem>
                                                        <asp:ListItem Value="DZ">Algeria</asp:ListItem>
                                                        <asp:ListItem Value="AS">American Samoa</asp:ListItem>
                                                        <asp:ListItem Value="AD">Andorra</asp:ListItem>
                                                        <asp:ListItem Value="AO">Angola</asp:ListItem>
                                                        <asp:ListItem Value="AI">Anguilla</asp:ListItem>
                                                        <asp:ListItem Value="AQ">Antarctica</asp:ListItem>
                                                        <asp:ListItem Value="AG">Antigua and Barbuda</asp:ListItem>
                                                        <asp:ListItem Value="AR">Argentina</asp:ListItem>
                                                        <asp:ListItem Value="AM">Armenia</asp:ListItem>
                                                        <asp:ListItem Value="AW">Aruba</asp:ListItem>
                                                        <asp:ListItem Value="AU">Australia</asp:ListItem>
                                                        <asp:ListItem Value="AT">Austria</asp:ListItem>
                                                        <asp:ListItem Value="AZ">Azerbaijan</asp:ListItem>
                                                        <asp:ListItem Value="BS">Bahamas</asp:ListItem>
                                                        <asp:ListItem Value="BH">Bahrain</asp:ListItem>
                                                        <asp:ListItem Value="BD">Bangladesh</asp:ListItem>
                                                        <asp:ListItem Value="BB">Barbados</asp:ListItem>
                                                        <asp:ListItem Value="BY">Belarus</asp:ListItem>
                                                        <asp:ListItem Value="BE">Belgium</asp:ListItem>
                                                        <asp:ListItem Value="BZ">Belize</asp:ListItem>
                                                        <asp:ListItem Value="BJ">Benin</asp:ListItem>
                                                        <asp:ListItem Value="BM">Bermuda</asp:ListItem>
                                                        <asp:ListItem Value="BT">Bhutan</asp:ListItem>
                                                        <asp:ListItem Value="BO">Bolivia</asp:ListItem>
                                                        <asp:ListItem Value="BA">Bosnia and Herzegovina</asp:ListItem>
                                                        <asp:ListItem Value="BW">Botswana</asp:ListItem>
                                                        <asp:ListItem Value="BV">Bouvet Island</asp:ListItem>
                                                        <asp:ListItem Value="BR">Brazil</asp:ListItem>
                                                        <asp:ListItem Value="VG">British Virgin Islands</asp:ListItem>
                                                        <asp:ListItem Value="IO">British Indian Ocean Territory</asp:ListItem>
                                                        <asp:ListItem Value="BN">Brunei Darussalam</asp:ListItem>
                                                        <asp:ListItem Value="BG">Bulgaria</asp:ListItem>
                                                        <asp:ListItem Value="BF">Burkina Faso</asp:ListItem>
                                                        <asp:ListItem Value="BI">Burundi</asp:ListItem>
                                                        <asp:ListItem Value="KH">Cambodia</asp:ListItem>
                                                        <asp:ListItem Value="CM">Cameroon</asp:ListItem>
                                                        <asp:ListItem Value="CA">Canada</asp:ListItem>
                                                        <asp:ListItem Value="CV">Cape Verde</asp:ListItem>
                                                        <asp:ListItem Value="KY">Cayman Islands</asp:ListItem>
                                                        <asp:ListItem Value="CF">Central African Republic</asp:ListItem>
                                                        <asp:ListItem Value="TD">Chad</asp:ListItem>
                                                        <asp:ListItem Value="CL">Chile</asp:ListItem>
                                                        <asp:ListItem Value="CN">China</asp:ListItem>
                                                        <asp:ListItem Value="HK">Hong Kong, SAR China</asp:ListItem>
                                                        <asp:ListItem Value="MO">Macao, SAR China</asp:ListItem>
                                                        <asp:ListItem Value="CX">Christmas Island</asp:ListItem>
                                                        <asp:ListItem Value="CC">Cocos (Keeling) Islands</asp:ListItem>
                                                        <asp:ListItem Value="CO">Colombia</asp:ListItem>
                                                        <asp:ListItem Value="KM">Comoros</asp:ListItem>
                                                        <asp:ListItem Value="CG">Congo (Brazzaville)</asp:ListItem>
                                                        <asp:ListItem Value="CD">Congo, (Kinshasa)</asp:ListItem>
                                                        <asp:ListItem Value="CK">Cook Islands</asp:ListItem>
                                                        <asp:ListItem Value="CR">Costa Rica</asp:ListItem>
                                                        <asp:ListItem Value="CI">Côte d'Ivoire</asp:ListItem>
                                                        <asp:ListItem Value="HR">Croatia</asp:ListItem>
                                                        <asp:ListItem Value="CU">Cuba</asp:ListItem>
                                                        <asp:ListItem Value="CY">Cyprus</asp:ListItem>
                                                        <asp:ListItem Value="CZ">Czech Republic</asp:ListItem>
                                                        <asp:ListItem Value="DK">Denmark</asp:ListItem>
                                                        <asp:ListItem Value="DJ">Djibouti</asp:ListItem>
                                                        <asp:ListItem Value="DM">Dominica</asp:ListItem>
                                                        <asp:ListItem Value="DO">Dominican Republic</asp:ListItem>
                                                        <asp:ListItem Value="EC">Ecuador</asp:ListItem>
                                                        <asp:ListItem Value="EG">Egypt</asp:ListItem>
                                                        <asp:ListItem Value="SV">El Salvador</asp:ListItem>
                                                        <asp:ListItem Value="GQ">Equatorial Guinea</asp:ListItem>
                                                        <asp:ListItem Value="ER">Eritrea</asp:ListItem>
                                                        <asp:ListItem Value="EE">Estonia</asp:ListItem>
                                                        <asp:ListItem Value="ET">Ethiopia</asp:ListItem>
                                                        <asp:ListItem Value="FK">Falkland Islands (Malvinas)</asp:ListItem>
                                                        <asp:ListItem Value="FO">Faroe Islands</asp:ListItem>
                                                        <asp:ListItem Value="FJ">Fiji</asp:ListItem>
                                                        <asp:ListItem Value="FI">Finland</asp:ListItem>
                                                        <asp:ListItem Value="FR">France</asp:ListItem>
                                                        <asp:ListItem Value="GF">French Guiana</asp:ListItem>
                                                        <asp:ListItem Value="PF">French Polynesia</asp:ListItem>
                                                        <asp:ListItem Value="TF">French Southern Territories</asp:ListItem>
                                                        <asp:ListItem Value="GA">Gabon</asp:ListItem>
                                                        <asp:ListItem Value="GM">Gambia</asp:ListItem>
                                                        <asp:ListItem Value="GE">Georgia</asp:ListItem>
                                                        <asp:ListItem Value="DE">Germany</asp:ListItem>
                                                        <asp:ListItem Value="GH">Ghana</asp:ListItem>
                                                        <asp:ListItem Value="GI">Gibraltar</asp:ListItem>
                                                        <asp:ListItem Value="GR">Greece</asp:ListItem>
                                                        <asp:ListItem Value="GL">Greenland</asp:ListItem>
                                                        <asp:ListItem Value="GD">Grenada</asp:ListItem>
                                                        <asp:ListItem Value="GP">Guadeloupe</asp:ListItem>
                                                        <asp:ListItem Value="GU">Guam</asp:ListItem>
                                                        <asp:ListItem Value="GT">Guatemala</asp:ListItem>
                                                        <asp:ListItem Value="GG">Guernsey</asp:ListItem>
                                                        <asp:ListItem Value="GN">Guinea</asp:ListItem>
                                                        <asp:ListItem Value="GW">Guinea-Bissau</asp:ListItem>
                                                        <asp:ListItem Value="GY">Guyana</asp:ListItem>
                                                        <asp:ListItem Value="HT">Haiti</asp:ListItem>
                                                        <asp:ListItem Value="HM">Heard and Mcdonald Islands</asp:ListItem>
                                                        <asp:ListItem Value="VA">Holy See (Vatican City State)</asp:ListItem>
                                                        <asp:ListItem Value="HN">Honduras</asp:ListItem>
                                                        <asp:ListItem Value="HU">Hungary</asp:ListItem>
                                                        <asp:ListItem Value="IS">Iceland</asp:ListItem>
                                                        <asp:ListItem Value="IN">India</asp:ListItem>
                                                        <asp:ListItem Value="ID">Indonesia</asp:ListItem>
                                                        <asp:ListItem Value="IR">Iran, Islamic Republic of</asp:ListItem>
                                                        <asp:ListItem Value="IQ">Iraq</asp:ListItem>
                                                        <asp:ListItem Value="IE">Ireland</asp:ListItem>
                                                        <asp:ListItem Value="IM">Isle of Man</asp:ListItem>
                                                        <asp:ListItem Value="IL">Israel</asp:ListItem>
                                                        <asp:ListItem Value="IT">Italy</asp:ListItem>
                                                        <asp:ListItem Value="JM">Jamaica</asp:ListItem>
                                                        <asp:ListItem Value="JP">Japan</asp:ListItem>
                                                        <asp:ListItem Value="JE">Jersey</asp:ListItem>
                                                        <asp:ListItem Value="JO">Jordan</asp:ListItem>
                                                        <asp:ListItem Value="KZ">Kazakhstan</asp:ListItem>
                                                        <asp:ListItem Value="KE">Kenya</asp:ListItem>
                                                        <asp:ListItem Value="KI">Kiribati</asp:ListItem>
                                                        <asp:ListItem Value="KP">Korea (North)</asp:ListItem>
                                                        <asp:ListItem Value="KR">Korea (South)</asp:ListItem>
                                                        <asp:ListItem Value="KW">Kuwait</asp:ListItem>
                                                        <asp:ListItem Value="KG">Kyrgyzstan</asp:ListItem>
                                                        <asp:ListItem Value="LA">Lao PDR</asp:ListItem>
                                                        <asp:ListItem Value="LV">Latvia</asp:ListItem>
                                                        <asp:ListItem Value="LB">Lebanon</asp:ListItem>
                                                        <asp:ListItem Value="LS">Lesotho</asp:ListItem>
                                                        <asp:ListItem Value="LR">Liberia</asp:ListItem>
                                                        <asp:ListItem Value="LY">Libya</asp:ListItem>
                                                        <asp:ListItem Value="LI">Liechtenstein</asp:ListItem>
                                                        <asp:ListItem Value="LT">Lithuania</asp:ListItem>
                                                        <asp:ListItem Value="LU">Luxembourg</asp:ListItem>
                                                        <asp:ListItem Value="MK">Macedonia, Republic of</asp:ListItem>
                                                        <asp:ListItem Value="MG">Madagascar</asp:ListItem>
                                                        <asp:ListItem Value="MW">Malawi</asp:ListItem>
                                                        <asp:ListItem Value="MY">Malaysia</asp:ListItem>
                                                        <asp:ListItem Value="MV">Maldives</asp:ListItem>
                                                        <asp:ListItem Value="ML">Mali</asp:ListItem>
                                                        <asp:ListItem Value="MT">Malta</asp:ListItem>
                                                        <asp:ListItem Value="MH">Marshall Islands</asp:ListItem>
                                                        <asp:ListItem Value="MQ">Martinique</asp:ListItem>
                                                        <asp:ListItem Value="MR">Mauritania</asp:ListItem>
                                                        <asp:ListItem Value="MU">Mauritius</asp:ListItem>
                                                        <asp:ListItem Value="YT">Mayotte</asp:ListItem>
                                                        <asp:ListItem Value="MX">Mexico</asp:ListItem>
                                                        <asp:ListItem Value="FM">Micronesia, Federated States of</asp:ListItem>
                                                        <asp:ListItem Value="MD">Moldova</asp:ListItem>
                                                        <asp:ListItem Value="MC">Monaco</asp:ListItem>
                                                        <asp:ListItem Value="MN">Mongolia</asp:ListItem>
                                                        <asp:ListItem Value="ME">Montenegro</asp:ListItem>
                                                        <asp:ListItem Value="MS">Montserrat</asp:ListItem>
                                                        <asp:ListItem Value="MA">Morocco</asp:ListItem>
                                                        <asp:ListItem Value="MZ">Mozambique</asp:ListItem>
                                                        <asp:ListItem Value="MM">Myanmar</asp:ListItem>
                                                        <asp:ListItem Value="NA">Namibia</asp:ListItem>
                                                        <asp:ListItem Value="NR">Nauru</asp:ListItem>
                                                        <asp:ListItem Value="NP">Nepal</asp:ListItem>
                                                        <asp:ListItem Value="NL">Netherlands</asp:ListItem>
                                                        <asp:ListItem Value="AN">Netherlands Antilles</asp:ListItem>
                                                        <asp:ListItem Value="NC">New Caledonia</asp:ListItem>
                                                        <asp:ListItem Value="NZ">New Zealand</asp:ListItem>
                                                        <asp:ListItem Value="NI">Nicaragua</asp:ListItem>
                                                        <asp:ListItem Value="NE">Niger</asp:ListItem>
                                                        <asp:ListItem Value="NG">Nigeria</asp:ListItem>
                                                        <asp:ListItem Value="NU">Niue</asp:ListItem>
                                                        <asp:ListItem Value="NF">Norfolk Island</asp:ListItem>
                                                        <asp:ListItem Value="MP">Northern Mariana Islands</asp:ListItem>
                                                        <asp:ListItem Value="NO">Norway</asp:ListItem>
                                                        <asp:ListItem Value="OM">Oman</asp:ListItem>
                                                        <asp:ListItem Value="PK">Pakistan</asp:ListItem>
                                                        <asp:ListItem Value="PW">Palau</asp:ListItem>
                                                        <asp:ListItem Value="PS">Palestinian Territory</asp:ListItem>
                                                        <asp:ListItem Value="PA">Panama</asp:ListItem>
                                                        <asp:ListItem Value="PG">Papua New Guinea</asp:ListItem>
                                                        <asp:ListItem Value="PY">Paraguay</asp:ListItem>
                                                        <asp:ListItem Value="PE">Peru</asp:ListItem>
                                                        <asp:ListItem Value="PN">Pitcairn</asp:ListItem>
                                                        <asp:ListItem Value="PL">Poland</asp:ListItem>
                                                        <asp:ListItem Value="PT">Portugal</asp:ListItem>
                                                        <asp:ListItem Value="PR">Puerto Rico</asp:ListItem>
                                                        <asp:ListItem Value="QA">Qatar</asp:ListItem>
                                                        <asp:ListItem Value="RE">Réunion</asp:ListItem>
                                                        <asp:ListItem Value="RO">Romania</asp:ListItem>
                                                        <asp:ListItem Value="RU">Russian Federation</asp:ListItem>
                                                        <asp:ListItem Value="RW">Rwanda</asp:ListItem>
                                                        <asp:ListItem Value="BL">Saint-Barthélemy</asp:ListItem>
                                                        <asp:ListItem Value="SH">Saint Helena</asp:ListItem>
                                                        <asp:ListItem Value="KN">Saint Kitts and Nevis</asp:ListItem>
                                                        <asp:ListItem Value="LC">Saint Lucia</asp:ListItem>
                                                        <asp:ListItem Value="MF">Saint-Martin (French part)</asp:ListItem>
                                                        <asp:ListItem Value="PM">Saint Pierre and Miquelon</asp:ListItem>
                                                        <asp:ListItem Value="VC">Saint Vincent and Grenadines</asp:ListItem>
                                                        <asp:ListItem Value="WS">Samoa</asp:ListItem>
                                                        <asp:ListItem Value="SM">San Marino</asp:ListItem>
                                                        <asp:ListItem Value="ST">Sao Tome and Principe</asp:ListItem>
                                                        <asp:ListItem Value="SA">Saudi Arabia</asp:ListItem>
                                                        <asp:ListItem Value="SN">Senegal</asp:ListItem>
                                                        <asp:ListItem Value="RS">Serbia</asp:ListItem>
                                                        <asp:ListItem Value="SC">Seychelles</asp:ListItem>
                                                        <asp:ListItem Value="SL">Sierra Leone</asp:ListItem>
                                                        <asp:ListItem Value="SG">Singapore</asp:ListItem>
                                                        <asp:ListItem Value="SK">Slovakia</asp:ListItem>
                                                        <asp:ListItem Value="SI">Slovenia</asp:ListItem>
                                                        <asp:ListItem Value="SB">Solomon Islands</asp:ListItem>
                                                        <asp:ListItem Value="SO">Somalia</asp:ListItem>
                                                        <asp:ListItem Value="ZA">South Africa</asp:ListItem>
                                                        <asp:ListItem Value="GS">South Georgia and the South Sandwich Islands</asp:ListItem>
                                                        <asp:ListItem Value="SS">South Sudan</asp:ListItem>
                                                        <asp:ListItem Value="ES">Spain</asp:ListItem>
                                                        <asp:ListItem Value="LK">Sri Lanka</asp:ListItem>
                                                        <asp:ListItem Value="SD">Sudan</asp:ListItem>
                                                        <asp:ListItem Value="SR">Suriname</asp:ListItem>
                                                        <asp:ListItem Value="SJ">Svalbard and Jan Mayen Islands</asp:ListItem>
                                                        <asp:ListItem Value="SZ">Swaziland</asp:ListItem>
                                                        <asp:ListItem Value="SE">Sweden</asp:ListItem>
                                                        <asp:ListItem Value="CH">Switzerland</asp:ListItem>
                                                        <asp:ListItem Value="SY">Syrian Arab Republic (Syria)</asp:ListItem>
                                                        <asp:ListItem Value="TW">Taiwan, Republic of China</asp:ListItem>
                                                        <asp:ListItem Value="TJ">Tajikistan</asp:ListItem>
                                                        <asp:ListItem Value="TZ">Tanzania, United Republic of</asp:ListItem>
                                                        <asp:ListItem Value="TH">Thailand</asp:ListItem>
                                                        <asp:ListItem Value="TL">Timor-Leste</asp:ListItem>
                                                        <asp:ListItem Value="TG">Togo</asp:ListItem>
                                                        <asp:ListItem Value="TK">Tokelau</asp:ListItem>
                                                        <asp:ListItem Value="TO">Tonga</asp:ListItem>
                                                        <asp:ListItem Value="TT">Trinidad and Tobago</asp:ListItem>
                                                        <asp:ListItem Value="TN">Tunisia</asp:ListItem>
                                                        <asp:ListItem Value="TR">Turkey</asp:ListItem>
                                                        <asp:ListItem Value="TM">Turkmenistan</asp:ListItem>
                                                        <asp:ListItem Value="TC">Turks and Caicos Islands</asp:ListItem>
                                                        <asp:ListItem Value="TV">Tuvalu</asp:ListItem>
                                                        <asp:ListItem Value="UG">Uganda</asp:ListItem>
                                                        <asp:ListItem Value="UA">Ukraine</asp:ListItem>
                                                        <asp:ListItem Value="AE">United Arab Emirates</asp:ListItem>
                                                        <asp:ListItem Value="GB">United Kingdom</asp:ListItem>
                                                        <asp:ListItem Value="US">United States of America</asp:ListItem>
                                                        <asp:ListItem Value="UM">US Minor Outlying Islands</asp:ListItem>
                                                        <asp:ListItem Value="UY">Uruguay</asp:ListItem>
                                                        <asp:ListItem Value="UZ">Uzbekistan</asp:ListItem>
                                                        <asp:ListItem Value="VU">Vanuatu</asp:ListItem>
                                                        <asp:ListItem Value="VE">Venezuela (Bolivarian Republic)</asp:ListItem>
                                                        <asp:ListItem Value="VN">Vietnam</asp:ListItem>
                                                        <asp:ListItem Value="VI">Virgin Islands, US</asp:ListItem>
                                                        <asp:ListItem Value="WF">Wallis and Futuna Islands</asp:ListItem>
                                                        <asp:ListItem Value="EH">Western Sahara</asp:ListItem>
                                                        <asp:ListItem Value="YE">Yemen</asp:ListItem>
                                                        <asp:ListItem Value="ZM">Zambia</asp:ListItem>
                                                        <asp:ListItem Value="ZW">Zimbabwe</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="box box-success col-md-12" runat="server" id="coborrowerGrid">
                                        <div class="box-header with-border">
                                            <h3 class="box-title titlemargin">Co-owner Details</h3>
                                        </div>
                                        <div class="box-body">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="box box-success" style="overflow-x: auto">
                                                        <div class="box-header with-border">
                                                            <button class="btn btn-default btn-primary pull-left titlemargin" type="button" data-toggle="modal" data-target="#MsgCoowner" runat="server" id="Button4" onserverclick="bDependent_ServerClick">Add Co-Owner <i class="fa fa-plus-square"></i></button>
                                                            <asp:CustomValidator
                                                                Style="color: red"
                                                                runat="server"
                                                                ID="CustomValidator2"
                                                                ValidationGroup="Save"
                                                                CssClass="col-md-12"
                                                                Text="Please add Co-Owner."
                                                                Enabled="false"
                                                                OnServerValidate="CustomValidator2_ServerValidate" />
                                                        </div>
                                                        <div class="box-body titlemargin" style="overflow-x: auto">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="box box-success col-md-12" runat="server" id="othersGrid">
                                        <div class="box-header with-border">
                                            <h3 class="box-title titlemargin">Co-Buyer Details</h3>
                                        </div>
                                        <div class="box-body">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="box box-success">
                                                        <div class="box-header with-border">
                                                            <button class="btn btn-default btn-primary pull-left titlemargin" type="button" data-toggle="modal" data-target="#MsgCoowner" runat="server" id="Button7" onserverclick="bDependent_ServerClick" style="color: white;">Add Co-Buyer <i class="fa fa-plus-square"></i></button>
                                                        </div>
                                                        <div class="box-body titlemargin" style="overflow-x: auto">
                                                            <asp:GridView ID="gvOthers" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                                                CssClass="table table-bordered table-hover" Width="100%" ShowHeader="True" PageSize="3" OnPageIndexChanging="gvOthers_PageIndexChanging">
                                                                <HeaderStyle BackColor="#66ccff" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" ItemStyle-Font-Size="Medium" />
                                                                    <asp:BoundField DataField="FirstName" HeaderText="First Name" SortExpression="First Name" ItemStyle-Font-Size="Medium" />
                                                                    <asp:BoundField DataField="MiddleName" HeaderText="Middle Name" SortExpression="Middle Name" ItemStyle-Font-Size="Medium" />
                                                                    <asp:BoundField DataField="LastName" HeaderText="Last Name" SortExpression="Last Name" ItemStyle-Font-Size="Medium" />
                                                                    <asp:BoundField DataField="Relationship" HeaderText="Relationship" SortExpression="Relationship" ItemStyle-Font-Size="Medium" />
                                                                    <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" ItemStyle-Font-Size="Medium" />
                                                                    <asp:BoundField DataField="MobileNo" HeaderText="Mobile No" SortExpression="Mobile No" ItemStyle-Font-Size="Medium" />
                                                                    <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" ItemStyle-Font-Size="Medium" />
                                                                    <asp:BoundField DataField="Residence" HeaderText="Residence" SortExpression="Residence" ItemStyle-Font-Size="Medium" />
                                                                    <asp:BoundField DataField="ValidID" HeaderText="ValidID" SortExpression="ValidID" ItemStyle-Font-Size="Medium" />
                                                                    <asp:BoundField DataField="ValidIDNo" HeaderText="ValidIDNo" SortExpression="ValidIDNo" ItemStyle-Font-Size="Medium" />
                                                                    <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton runat="server" ID="gvCoOwnerDelete" CssClass="btn btn-default btn-danger" Width="100%" Height="100%" CommandArgument='<%# Bind("ID")%>' OnClick="gvCoOwnerDelete_Click"><i class="fa fa-trash-o"></i></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <PagerStyle CssClass="pagination-ys" />
                                                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="box box-success col-md-12" runat="server" id="empDependents">
                                        <div style="background-color: #4FA75B; height: 3px;"></div>
                                        <div class="box-header with-border">
                                            <h3 class="box-title titlemargin">Dependents</h3>
                                        </div>
                                        <div class="box-body">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="box">
                                                        <div class="box-header">
                                                            <button class="btn btn-default btn-primary pull-left titlemargin" type="button" data-toggle="modal" data-target="#MsgDependent" runat="server" id="bDependent" onserverclick="bDependent_ServerClick">Add Dependent <i class="fa fa-plus-square"></i></button>
                                                        </div>
                                                        <div class="box-body titlemargin">
                                                            <asp:GridView ID="gvDependent" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                                                CssClass="table table-bordered table-hover" Width="100%" ShowHeader="True" PageSize="3" OnPageIndexChanging="gvDependent_PageIndexChanging">
                                                                <HeaderStyle BackColor="#66ccff" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" ItemStyle-Font-Size="Medium" />
                                                                    <asp:BoundField DataField="Name" HeaderText="Name of Dependents" SortExpression="Name" ItemStyle-Font-Size="Medium" />
                                                                    <asp:BoundField DataField="Age" HeaderText="Ages" SortExpression="Age" ItemStyle-Font-Size="Medium" />
                                                                    <asp:BoundField DataField="Relationship" HeaderText="Relationship" SortExpression="Relationship" ItemStyle-Font-Size="Medium" />


                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton runat="server"
                                                                                ID="btnDepEdit"
                                                                                type="button"
                                                                                class="btn btn-default btn-success"
                                                                                OnClick="btnDepEdit_Click"
                                                                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>">
                                                                                    <i class="fa fa-edit"></i>
                                                                            </asp:LinkButton>

                                                                            <asp:LinkButton runat="server" ID="gvDelete" CssClass="btn btn-default btn-danger" CommandArgument='<%# Bind("ID")%>' OnClick="gvDelete_Click"><i class="fa fa-trash-o"></i></asp:LinkButton>


                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <PagerStyle CssClass="pagination-ys" />
                                                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B"
                                                                    ForeColor="Blue"
                                                                    Font-Bold="true" />
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <%--                                    <div class="col-md-12">
                                        <asp:ValidationSummary DisplayMode="BulletList" Style="color: red" ID="ValidationSummary1" runat="server" CssClass="pull-right" ValidationGroup="Save" />
                                    </div>--%>
                                    <div class="col-md-12" style="margin-bottom: 5px">
                                        <button class="btn btn-primary nextBtn pull-right titlemargin" type="button" id="Button2" validationgroup="Save" runat="server" onserverclick="btnNext2_ServerClick">Next</button>
                                    </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div class="container-fluid">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:Panel runat="server" class="panel panel-primary" ID="step_3" Visible="false">
                    <div class="panel-heading" style="background-color: #5caceb">
                        <h3 class="panel-title">Buyer Details</h3>
                    </div>
                    <div class="panel-body">
                        <div class="nav-tabs-custom">
                            <ul class="nav nav-tabs" id="myTab1">
                                <%--                                <li class="active"><a href="#tab_1" class="tablinks" onclick="tabNav(event, 'tab_1')" data-target="tab_1" data-toggle="tab" id="referenceBtn">References</a></li>
                                <li class=""><a href="#tab_2" class="tablinks" onclick="tabNav(event, 'tab_2')" data-target="tab_2" data-toggle="tab" runat="server" id="spouseBtn">Spouse</a></li>
                                <li class=""><a href="#tab_3" class="tablinks" onclick="tabNav(event, 'tab_3')" data-target="tab_3" data-toggle="tab" id="SPABtn">SPA &/or Co-Borrower</a></li>--%>

                                <li class="active"><a href="#tab_1" class="tablinks" onclick="setCurrentTab(event);" data-toggle="tab" id="referenceBtn">References</a></li>
                                <li class=""><a href="#tab_2" class="tablinks" onclick="setCurrentTab(event);" data-toggle="tab" runat="server" id="spouseBtn">Spouse</a></li>
                                <li class=""><a href="#tab_3" class="tablinks" onclick="setCurrentTab(event);" data-toggle="tab" id="SPABtn">SPA  </a></li>

                            </ul>
                            <asp:HiddenField ID="TabName" runat="server" />
                            <div class="tab-content">
                                <div class="tab-pane active" id="tab_1">
                                    <div class="content">
                                        <%--                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>--%>
                                        <div class="box box-primary">
                                            <div style="background-color: #468DBC; height: 3px;"></div>
                                            <div class="box-header">
                                                <h3 class="box-title">
                                                    <%--<button class="btn btn-default btn-primary" type="button" data-toggle="modal" data-target="#MsgBankAccount">Add Bank Account <i class="fa fa-plus-square"></i></button>--%>

                                                    <asp:LinkButton class="btn btn-primary" type="button" runat="server"
                                                        ID="btnShowBank" OnClick="btnShowBank_Click">
                                                                <i class="fa fa-plus-circle"></i>Add Bank Account</asp:LinkButton>
                                                </h3>
                                            </div>
                                            <div class="box-body">
                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="gvBankAccount" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                                            CssClass="table table-bordered table-hover" Width="100%" ShowHeader="True" PageSize="3" OnPageIndexChanging="gvBankAccount_PageIndexChanging">
                                                            <HeaderStyle BackColor="#66ccff" />
                                                            <Columns>
                                                                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" ItemStyle-Font-Size="Medium" />
                                                                <asp:BoundField DataField="Bank" HeaderText="BANK" SortExpression="Bank" ItemStyle-Font-Size="Medium" />
                                                                <asp:BoundField DataField="Branch" HeaderText="BRANCH" SortExpression="Branch" ItemStyle-Font-Size="Medium" />
                                                                <asp:BoundField DataField="AcctType" HeaderText="TYPE OF ACCOUNT" SortExpression="AcctType" ItemStyle-Font-Size="Medium" />
                                                                <asp:BoundField DataField="AcctNo" HeaderText="ACCT. NO." SortExpression="AcctNo" ItemStyle-Font-Size="Medium" />
                                                                <%--<asp:BoundField HeaderStyle-Width="15%" DataField="AvgDailyBal" HeaderText="AVERAGE DAILY BALANCE" SortExpression="AvgDailyBal" ItemStyle-Font-Size="Medium" />--%>
                                                                <%--<asp:BoundField HeaderStyle-Width="15%" DataField="PresBal" HeaderText="PRESENT BALANCE" SortExpression="PresBal" ItemStyle-Font-Size="Medium" />--%>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton runat="server" ID="btnBankUpdate" type="button"
                                                                            class="btn btn-default btn-success" OnClick="btnBankUpdate_Click" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>">
                                                                                    <i class="fa fa-edit"></i>
                                                                        </asp:LinkButton>
                                                                        <asp:LinkButton runat="server" ID="btnBankAccount" CssClass="btn btn-default btn-danger" CommandArgument='<%# Bind("ID")%>' OnClick="btnBankAccount_Click"><i class="fa fa-trash-o"></i></asp:LinkButton>
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
                                        <div class="box box-primary">
                                            <div style="background-color: #468DBC; height: 3px;"></div>
                                            <div class="box-header">
                                                <h3 class="box-title">
                                                    <asp:LinkButton class="btn btn-primary" type="button" runat="server" data-toggle="modal" data-target="#MsgCharacterRef" ID="btnRefShow" OnClick="btnRefShow_Click"><i class="fa fa-plus-circle"></i>Add Character Ref</asp:LinkButton>
                                                    <%-- 2023-06-17 : CHANGE BUTTON TO LINKBUTTON--%>
                                                    <%--<button class="btn btn-default btn-primary" type="button"  >Add Character Ref <i class="fa fa-plus-square"></i></button>--%>
                                                </h3>
                                            </div>
                                            <div class="box-body">
                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="gvCharacterRef" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                                            CssClass="table table-bordered table-hover" ShowHeader="True" PageSize="3" OnPageIndexChanging="gvCharacterRef_PageIndexChanging">
                                                            <HeaderStyle BackColor="#66ccff" />
                                                            <Columns>
                                                                <asp:BoundField DataField="Id" HeaderText="ID" SortExpression="ID" ItemStyle-Font-Size="Medium" />
                                                                <asp:BoundField DataField="Name" HeaderText="NAME" SortExpression="Name" ItemStyle-Font-Size="Medium" />
                                                                <asp:BoundField DataField="Address" HeaderText="ADDRESS" SortExpression="Address" ItemStyle-Font-Size="Medium" />
                                                                <asp:BoundField DataField="Email" HeaderText="Email"
                                                                    SortExpression="Email"
                                                                    ItemStyle-Font-Size="Medium" />
                                                                <asp:BoundField DataField="TelNo" HeaderText="TELEPHONE NO." SortExpression="TelNo" ItemStyle-Font-Size="Medium" />
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton runat="server" ID="btnRefUpdate" type="button"
                                                                            class="btn btn-default btn-success" OnClick="btnRefUpdate_Click" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>">
                                                                                    <i class="fa fa-edit"></i>
                                                                        </asp:LinkButton>

                                                                        <asp:LinkButton runat="server" ID="btnCharacterRef" CssClass="btn btn-default btn-danger"
                                                                            CommandArgument='<%# Bind("ID")%>' OnClick="btnCharacterRef_Click"><i class="fa fa-trash-o"></i></asp:LinkButton>
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











                                        <div class="box box-primary">
                                            <div class="box-header">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <h3 style="text-align: center;"><b>Attachments</b></h3>
                                                </div>
                                            </div>





                                            <div class="box-body">




                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="border-right-color: lightgray; border-right-width: thin; border-right-style: solid;">
                                                    <div class="row">
                                                        <asp:GridView ID="gvStandardDocumentRequirements" runat="server" AllowPaging="false" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                                            CssClass="table table-hover table-responsive" Width="100%" ShowHeader="True" PageSize="10"
                                                            OnRowCommand="gvStandardDocumentRequirements_RowCommand"
                                                            GridLines="None">
                                                            <HeaderStyle BackColor="#ff9999" />
                                                            <Columns>
                                                                <asp:BoundField DataField="DocumentName" HeaderText="Required Document Name *" SortExpression="Document Name" ItemStyle-Font-Size="Medium" HeaderStyle-Width="30%" />
                                                                <%--                                                                <asp:TemplateField HeaderText="Document Name" SortExpression="Document Name" ItemStyle-Font-Size="Medium" HeaderStyle-Width="65%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDocumentName" runat="server" Text='<%# GetFormattedDocu   mentName(Container.DataItem) %>'
                                                                            Font-Size="Medium" Width="30%" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>--%>

                                                                <asp:TemplateField HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center"  HeaderText="Preview">
                                                                    <ItemTemplate>
                                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <div class="form-group  center-block text-center ">
                                                                                    <%--<asp:FileUpload ID="FileUpload2" runat="server" onChange="UploadFile(this);" />--%>

                                                                                    <div class="col-lg-12 center-block center-block">
                                                                                        <asp:Label runat="server" ID="lblFileName1" Text=""> </asp:Label>
                                                                                        <asp:FileUpload ID="FileUpload3" CssClass="btn btn-default" CommandName="FileUpload3" runat="server" EnableViewState="true" />

                                                                                        <%--2023-04-28 : 1ST AND 2ND ID ARE REQUIRED--%>
                                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorAttachments" runat="server"
                                                                                            ControlToValidate="FileUpload3"
                                                                                            CssClass="col-md-12"
                                                                                            ValidationGroup="SaveBuyer2" Style="color: red" ErrorMessage="Please upload your required attachment.">
                                                                                        </asp:RequiredFieldValidator>

                                                                                        <%--2023-04-19 : REQUESTED BY DATA TO REMOVE BLOCKING OF 2ND ID--%>
                                                                                        <%--                                                                                        <asp:CustomValidator runat="server"
                                                                                            Style="color: red"
                                                                                            ID="customValidatorDocuments"
                                                                                            CssClass="col-md-12"
                                                                                            ValidationGroup="SaveBuyer2"
                                                                                            OnServerValidate="customValidatorDocuments_ServerValidate" />--%>
                                                                                    </div>

                                                                                    <div class="col-lg-12 center-block center-block">
                                                                                        <asp:LinkButton ID="btnUpload3" CssClass="btn btn-primary" runat="server" CommandName="Upload" Text="Upload" />
                                                                                        <asp:LinkButton ID="btnPreview3" CssClass="btn btn-info" Visible="false" runat="server" CommandName="Preview" Text="Preview" />
                                                                                        <asp:LinkButton ID="btnRemove3" CssClass="btn btn-danger" Visible="false" runat="server" CommandName="Remove" Text="Remove" />
                                                                                    </div>
                                                                                </div>
                                                                            </ContentTemplate>
                                                                            <Triggers>
                                                                                <asp:PostBackTrigger ControlID="btnUpload3" />
                                                                                <asp:PostBackTrigger ControlID="btnPreview3" />
                                                                                <asp:PostBackTrigger ControlID="btnRemove3" />
                                                                            </Triggers>
                                                                        </asp:UpdatePanel>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="Required" Visible="false" />
                                                            </Columns>
                                                            <PagerStyle CssClass="pagination-ys" />
                                                            <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                                        </asp:GridView>
                                                    </div>
                                                </div>


                                                <%--2023-04-19 : REQUESTED BY DATA TO REMOVE BLOCKING OF 2ND ID--%>
                                                <%--CHANG REQUEST : NO BLOCKINGS FOR THIS GRIDVIEW--%>
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="border-right-color: lightgray; border-right-width: thin; border-right-style: solid;">
                                                    <div class="row">
                                                        <asp:GridView ID="gvStandardDocumentRequirements2" runat="server" AllowPaging="false" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                                            CssClass="table table-hover table-responsive" Width="100%" ShowHeader="true" PageSize="10"
                                                            OnRowCommand="gvStandardDocumentRequirements2_RowCommand"
                                                            GridLines="None">
                                                            <HeaderStyle BackColor="#66ccff" />
                                                            <Columns>
                                                                <asp:BoundField DataField="DocumentName" HeaderText="Document Name " SortExpression="Document Name" ItemStyle-Font-Size="Medium" HeaderStyle-Width="30%" />
                                                                <asp:TemplateField HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center"  HeaderText="Preview">
                                                                    <ItemTemplate>
                                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <div class="form-group  center-block text-center ">
                                                                                    <%--<asp:FileUpload ID="FileUpload2" runat="server" onChange="UploadFile(this);" />--%>

                                                                                    <div class="col-lg-12 center-block center-block">
                                                                                        <asp:Label runat="server" ID="lblFileName3" Text=""></asp:Label>
                                                                                        <asp:FileUpload ID="FileUpload4" CssClass="btn btn-default" CommandName="FileUpload3" runat="server" EnableViewState="true" />


                                                                                    </div>

                                                                                    <div class="col-lg-12 center-block center-block">
                                                                                        <asp:LinkButton ID="btnUpload4" CssClass="btn btn-primary" runat="server" CommandName="Upload" Text="Upload" />
                                                                                        <asp:LinkButton ID="btnPreview4" CssClass="btn btn-info" Visible="false" runat="server" CommandName="Preview" Text="Preview" />
                                                                                        <asp:LinkButton ID="btnRemove4" CssClass="btn btn-danger" Visible="false" runat="server" CommandName="Remove" Text="Remove" />
                                                                                    </div>
                                                                                </div>
                                                                            </ContentTemplate>
                                                                            <Triggers>
                                                                                <asp:PostBackTrigger ControlID="btnUpload4" />
                                                                                <asp:PostBackTrigger ControlID="btnPreview4" />
                                                                                <asp:PostBackTrigger ControlID="btnRemove4" />
                                                                            </Triggers>
                                                                        </asp:UpdatePanel>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="Required" Visible="false" />
                                                            </Columns>
                                                            <PagerStyle CssClass="pagination-ys" />
                                                            <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                                        </asp:GridView>
                                                    </div>
                                                </div>



                                            </div>







                                        </div>
                                        <div class="box box-primary">
                                            <div style="background-color: #468DBC; height: 3px;"></div>
                                        </div>
                                        <%--                                            </ContentTemplate>
                                        </asp:UpdatePanel>--%>
                                    </div>
                                </div>
                                <%--<div class="tab-pane tabcontent" id="tab_2" style="display: none">--%>
                                <div class="tab-pane" id="tab_2">
                                    <div class="content">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <div class="row">
                                                    <div class="col-md-12" runat="server" id="dvSpouseDetails">
                                                        <div class="box box-primary">
                                                            <div class="box-header">
                                                                <h3 class="box-title">Spouse Details</h3>
                                                            </div>
                                                            <div class="box-body">
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <label>Last Name <span class="color-red fsize-16">*</span></label>
                                                                        <input runat="server" type="text" class="form-control text-uppercase" maxlength="50" id="tSpouseLastName" placeholder="Spouse Last Name" />
                                                                    </div>
                                                                    <asp:RequiredFieldValidator
                                                                        ID="RequiredFieldValidator9"
                                                                        ControlToValidate="tSpouseLastName" CssClass="col-md-12"
                                                                        ErrorMessage="Please fill Spouse Last Name."
                                                                        ValidationGroup="SaveBuyer2"
                                                                        runat="server" Style="color: red" />
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <label>First Name <span class="color-red fsize-16">*</span></label>
                                                                        <input runat="server" type="text" class="form-control text-uppercase" maxlength="50" id="tSpouseFirstName" placeholder="Spouse First Name" />
                                                                    </div>
                                                                    <asp:RequiredFieldValidator
                                                                        ID="RequiredFieldValidator10"
                                                                        ControlToValidate="tSpouseFirstName" CssClass="col-md-12"
                                                                        ErrorMessage="Please fill Spouse First Name."
                                                                        ValidationGroup="SaveBuyer2"
                                                                        runat="server" Style="color: red" />
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <label>Middle Name <span class="color-red fsize-16">*</span></label>
                                                                        <input runat="server" type="text" class="form-control text-uppercase" maxlength="50" id="tSpouseMiddleName" placeholder="Spouse Middle Name" />
                                                                    </div>
                                                                    <asp:RequiredFieldValidator
                                                                        ID="RequiredFieldValidator11"
                                                                        ControlToValidate="tSpouseMiddleName" CssClass="col-md-12"
                                                                        ErrorMessage="Please fill Spouse Middle Name."
                                                                        ValidationGroup="SaveBuyer2"
                                                                        runat="server" Style="color: red" />
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <label>Employment Status <span class="color-red fsize-16">*</span></label>
                                                                        <asp:DropDownList ID="tSpouseEmpStatus" AutoPostBack="true" DataTextField="Name" OnSelectedIndexChanged="tSpouseEmpStatus_SelectedIndexChanged" DataValueField="Code" runat="server" CssClass="form-control">
                                                                            <%--<asp:ListItem Value="---Select Civil Status---" Selected="True"></asp:ListItem>--%>
                                                                        </asp:DropDownList>
                                                                        <%--COMMENTED 06 - 13 - 2023 : CHANGE REQUEST  https://direcbti.monday.com/boards/4462005457/pulses/4576973637?asset_id=908456598  --%>
                                                                        <asp:RequiredFieldValidator
                                                                            ID="RequiredFieldValidator32"
                                                                            ErrorMessage="Please select Employment Status."
                                                                            SetFocusOnError="true"
                                                                            ControlToValidate="tSpouseEmpStatus"
                                                                            Text="Please select Employment Status."
                                                                            ValidationGroup="SaveBuyer2"
                                                                            runat="server" Style="color: red" />
                                                                    </div>
                                                                </div>
                                                                <div class="row"></div>
                                                                <div class="col-md-12">
                                                                    <div class="form-group">
                                                                        <label>Residential No</label>&nbsp; <span>
                                                                            <asp:LinkButton runat="server" ID="btnCopyPrincipalAddress" OnClick="btnCopyPrincipalAddress_Click" class="btn btn-info btn-sm"> Copy principal buyer's present address</asp:LinkButton></span>
                                                                        <asp:TextBox runat="server" ID="txtSPOAddress" MaxLength="250" placeholder="4th Floor Victoria One Bldg, South Triangle, Quezon City" CssClass="form-control text-uppercase"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <%--                                                                <div class="col-md-12">
                                                                    <div class="form-group">
                                                                        <label>Permanent/Provincial Address</label>
                                                                        <input runat="server" type="text" class="form-control text-uppercase" id="tSPAPermanent" placeholder="Permanent/Provincial Address" />
                                                                    </div>
                                                                </div>--%>
                                                                <div class="col-md-6">
                                                                    <label>Date of Birth</label>
                                                                    <div class="input-group">
                                                                        <div class="input-group-addon">
                                                                            <i class="fa fa-calendar"></i>
                                                                        </div>
                                                                        <input runat="server" type="date" class="form-control" id="tSpouseBirthDate" />
                                                                    </div>
                                                                    <asp:CustomValidator
                                                                        Style="color: red"
                                                                        runat="server"
                                                                        ID="CustomValidator6"
                                                                        CssClass="col-md-12"
                                                                        ValidationGroup="SaveBuyer"
                                                                        ControlToValidate="tSpouseBirthDate"
                                                                        ErrorMessage="Below legal age."
                                                                        OnServerValidate="CustomValidator6_ServerValidate" />
                                                                </div>
                                                                <div class="col-md-12">
                                                                    <div class="form-group">
                                                                        <label>Place of Birth</label>
                                                                        <input runat="server" type="text" maxlength="50" class="form-control text-uppercase" id="tSpouseBirthPlace" placeholder="Spouse Birth Place" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <label>Gender</label>
                                                                        <select runat="server" id="tSpouseGender" class="form-control">
                                                                            <option value="">-- Select One --</option>
                                                                            <option value="M">Male</option>
                                                                            <option value="F">Female</option>
                                                                        </select>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <label>Citizenship</label>
                                                                        <select runat="server" id="tSpouseCitizenship" name="nationality" class="form-control">
                                                                            <option value="">-- Select One --</option>
                                                                            <option value="afghan">Afghan</option>
                                                                            <option value="albanian">Albanian</option>
                                                                            <option value="algerian">Algerian</option>
                                                                            <option value="american">American</option>
                                                                            <option value="andorran">Andorran</option>
                                                                            <option value="angolan">Angolan</option>
                                                                            <option value="antiguans">Antiguans</option>
                                                                            <option value="argentinean">Argentinean</option>
                                                                            <option value="armenian">Armenian</option>
                                                                            <option value="australian">Australian</option>
                                                                            <option value="austrian">Austrian</option>
                                                                            <option value="azerbaijani">Azerbaijani</option>
                                                                            <option value="bahamian">Bahamian</option>
                                                                            <option value="bahraini">Bahraini</option>
                                                                            <option value="bangladeshi">Bangladeshi</option>
                                                                            <option value="barbadian">Barbadian</option>
                                                                            <option value="barbudans">Barbudans</option>
                                                                            <option value="batswana">Batswana</option>
                                                                            <option value="belarusian">Belarusian</option>
                                                                            <option value="belgian">Belgian</option>
                                                                            <option value="belizean">Belizean</option>
                                                                            <option value="beninese">Beninese</option>
                                                                            <option value="bhutanese">Bhutanese</option>
                                                                            <option value="bolivian">Bolivian</option>
                                                                            <option value="bosnian">Bosnian</option>
                                                                            <option value="brazilian">Brazilian</option>
                                                                            <option value="british">British</option>
                                                                            <option value="bruneian">Bruneian</option>
                                                                            <option value="bulgarian">Bulgarian</option>
                                                                            <option value="burkinabe">Burkinabe</option>
                                                                            <option value="burmese">Burmese</option>
                                                                            <option value="burundian">Burundian</option>
                                                                            <option value="cambodian">Cambodian</option>
                                                                            <option value="cameroonian">Cameroonian</option>
                                                                            <option value="canadian">Canadian</option>
                                                                            <option value="cape verdean">Cape Verdean</option>
                                                                            <option value="central african">Central African</option>
                                                                            <option value="chadian">Chadian</option>
                                                                            <option value="chilean">Chilean</option>
                                                                            <option value="chinese">Chinese</option>
                                                                            <option value="colombian">Colombian</option>
                                                                            <option value="comoran">Comoran</option>
                                                                            <option value="congolese">Congolese</option>
                                                                            <option value="costa rican">Costa Rican</option>
                                                                            <option value="croatian">Croatian</option>
                                                                            <option value="cuban">Cuban</option>
                                                                            <option value="cypriot">Cypriot</option>
                                                                            <option value="czech">Czech</option>
                                                                            <option value="danish">Danish</option>
                                                                            <option value="djibouti">Djibouti</option>
                                                                            <option value="dominican">Dominican</option>
                                                                            <option value="dutch">Dutch</option>
                                                                            <option value="east timorese">East Timorese</option>
                                                                            <option value="ecuadorean">Ecuadorean</option>
                                                                            <option value="egyptian">Egyptian</option>
                                                                            <option value="emirian">Emirian</option>
                                                                            <option value="equatorial guinean">Equatorial Guinean</option>
                                                                            <option value="eritrean">Eritrean</option>
                                                                            <option value="estonian">Estonian</option>
                                                                            <option value="ethiopian">Ethiopian</option>
                                                                            <option value="fijian">Fijian</option>
                                                                            <option value="filipino">Filipino</option>
                                                                            <option value="finnish">Finnish</option>
                                                                            <option value="french">French</option>
                                                                            <option value="gabonese">Gabonese</option>
                                                                            <option value="gambian">Gambian</option>
                                                                            <option value="georgian">Georgian</option>
                                                                            <option value="german">German</option>
                                                                            <option value="ghanaian">Ghanaian</option>
                                                                            <option value="greek">Greek</option>
                                                                            <option value="grenadian">Grenadian</option>
                                                                            <option value="guatemalan">Guatemalan</option>
                                                                            <option value="guinea-bissauan">Guinea-Bissauan</option>
                                                                            <option value="guinean">Guinean</option>
                                                                            <option value="guyanese">Guyanese</option>
                                                                            <option value="haitian">Haitian</option>
                                                                            <option value="herzegovinian">Herzegovinian</option>
                                                                            <option value="honduran">Honduran</option>
                                                                            <option value="hungarian">Hungarian</option>
                                                                            <option value="icelander">Icelander</option>
                                                                            <option value="indian">Indian</option>
                                                                            <option value="indonesian">Indonesian</option>
                                                                            <option value="iranian">Iranian</option>
                                                                            <option value="iraqi">Iraqi</option>
                                                                            <option value="irish">Irish</option>
                                                                            <option value="israeli">Israeli</option>
                                                                            <option value="italian">Italian</option>
                                                                            <option value="ivorian">Ivorian</option>
                                                                            <option value="jamaican">Jamaican</option>
                                                                            <option value="japanese">Japanese</option>
                                                                            <option value="jordanian">Jordanian</option>
                                                                            <option value="kazakhstani">Kazakhstani</option>
                                                                            <option value="kenyan">Kenyan</option>
                                                                            <option value="kittian and nevisian">Kittian and Nevisian</option>
                                                                            <option value="kuwaiti">Kuwaiti</option>
                                                                            <option value="kyrgyz">Kyrgyz</option>
                                                                            <option value="laotian">Laotian</option>
                                                                            <option value="latvian">Latvian</option>
                                                                            <option value="lebanese">Lebanese</option>
                                                                            <option value="liberian">Liberian</option>
                                                                            <option value="libyan">Libyan</option>
                                                                            <option value="liechtensteiner">Liechtensteiner</option>
                                                                            <option value="lithuanian">Lithuanian</option>
                                                                            <option value="luxembourger">Luxembourger</option>
                                                                            <option value="macedonian">Macedonian</option>
                                                                            <option value="malagasy">Malagasy</option>
                                                                            <option value="malawian">Malawian</option>
                                                                            <option value="malaysian">Malaysian</option>
                                                                            <option value="maldivan">Maldivan</option>
                                                                            <option value="malian">Malian</option>
                                                                            <option value="maltese">Maltese</option>
                                                                            <option value="marshallese">Marshallese</option>
                                                                            <option value="mauritanian">Mauritanian</option>
                                                                            <option value="mauritian">Mauritian</option>
                                                                            <option value="mexican">Mexican</option>
                                                                            <option value="micronesian">Micronesian</option>
                                                                            <option value="moldovan">Moldovan</option>
                                                                            <option value="monacan">Monacan</option>
                                                                            <option value="mongolian">Mongolian</option>
                                                                            <option value="moroccan">Moroccan</option>
                                                                            <option value="mosotho">Mosotho</option>
                                                                            <option value="motswana">Motswana</option>
                                                                            <option value="mozambican">Mozambican</option>
                                                                            <option value="namibian">Namibian</option>
                                                                            <option value="nauruan">Nauruan</option>
                                                                            <option value="nepalese">Nepalese</option>
                                                                            <option value="new zealander">New Zealander</option>
                                                                            <option value="ni-vanuatu">Ni-Vanuatu</option>
                                                                            <option value="nicaraguan">Nicaraguan</option>
                                                                            <option value="nigerien">Nigerien</option>
                                                                            <option value="north korean">North Korean</option>
                                                                            <option value="northern irish">Northern Irish</option>
                                                                            <option value="norwegian">Norwegian</option>
                                                                            <option value="omani">Omani</option>
                                                                            <option value="pakistani">Pakistani</option>
                                                                            <option value="palauan">Palauan</option>
                                                                            <option value="panamanian">Panamanian</option>
                                                                            <option value="papua new guinean">Papua New Guinean</option>
                                                                            <option value="paraguayan">Paraguayan</option>
                                                                            <option value="peruvian">Peruvian</option>
                                                                            <option value="polish">Polish</option>
                                                                            <option value="portuguese">Portuguese</option>
                                                                            <option value="qatari">Qatari</option>
                                                                            <option value="romanian">Romanian</option>
                                                                            <option value="russian">Russian</option>
                                                                            <option value="rwandan">Rwandan</option>
                                                                            <option value="saint lucian">Saint Lucian</option>
                                                                            <option value="salvadoran">Salvadoran</option>
                                                                            <option value="samoan">Samoan</option>
                                                                            <option value="san marinese">San Marinese</option>
                                                                            <option value="sao tomean">Sao Tomean</option>
                                                                            <option value="saudi">Saudi</option>
                                                                            <option value="scottish">Scottish</option>
                                                                            <option value="senegalese">Senegalese</option>
                                                                            <option value="serbian">Serbian</option>
                                                                            <option value="seychellois">Seychellois</option>
                                                                            <option value="sierra leonean">Sierra Leonean</option>
                                                                            <option value="singaporean">Singaporean</option>
                                                                            <option value="slovakian">Slovakian</option>
                                                                            <option value="slovenian">Slovenian</option>
                                                                            <option value="solomon islander">Solomon Islander</option>
                                                                            <option value="somali">Somali</option>
                                                                            <option value="south african">South African</option>
                                                                            <option value="south korean">South Korean</option>
                                                                            <option value="spanish">Spanish</option>
                                                                            <option value="sri lankan">Sri Lankan</option>
                                                                            <option value="sudanese">Sudanese</option>
                                                                            <option value="surinamer">Surinamer</option>
                                                                            <option value="swazi">Swazi</option>
                                                                            <option value="swedish">Swedish</option>
                                                                            <option value="swiss">Swiss</option>
                                                                            <option value="syrian">Syrian</option>
                                                                            <option value="taiwanese">Taiwanese</option>
                                                                            <option value="tajik">Tajik</option>
                                                                            <option value="tanzanian">Tanzanian</option>
                                                                            <option value="thai">Thai</option>
                                                                            <option value="togolese">Togolese</option>
                                                                            <option value="tongan">Tongan</option>
                                                                            <option value="trinidadian or tobagonian">Trinidadian or Tobagonian</option>
                                                                            <option value="tunisian">Tunisian</option>
                                                                            <option value="turkish">Turkish</option>
                                                                            <option value="tuvaluan">Tuvaluan</option>
                                                                            <option value="ugandan">Ugandan</option>
                                                                            <option value="ukrainian">Ukrainian</option>
                                                                            <option value="uruguayan">Uruguayan</option>
                                                                            <option value="uzbekistani">Uzbekistani</option>
                                                                            <option value="venezuelan">Venezuelan</option>
                                                                            <option value="vietnamese">Vietnamese</option>
                                                                            <option value="welsh">Welsh</option>
                                                                            <option value="yemenite">Yemenite</option>
                                                                            <option value="zambian">Zambian</option>
                                                                            <option value="zimbabwean">Zimbabwean</option>
                                                                        </select>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <label>Email Address</label>
                                                                        <input runat="server" maxlength="50" type="email" class="form-control text-lowercase" id="tSpouseEmailAdd" placeholder="xxxx@email.com" />
                                                                    </div>
                                                                </div>
                                                                <%--                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <label>Home Tel No.</label>
                                                                        <asp:TextBox runat="server" ID="txtSPOTelNo" placeholder="123-45-6789" CssClass="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>--%>
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <label>Cellphone No.</label>
                                                                        <input runat="server" type="text" class="form-control" id="tSpouseCellphoneNo" maxlength="13" onkeypress="return fnAllowNumeric(event)" placeholder="xxxx xxx xxxx" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <label>Facebook Account</label>
                                                                        <input runat="server" type="email" maxlength="50" class="form-control text-lowercase" id="tSpouseFBAccount" placeholder="xxxx@email.com" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div runat="server" id="divSpouseBusiness" visible="false">
                                                        <div class="col-md-6">
                                                            <div class="box box-primary">
                                                                <div class="box-header">
                                                                    <h3 class="box-title">Spouse Employment Details</h3>
                                                                </div>
                                                                <div class="box-body">
                                                                    <div class="col-md-6">
                                                                        <div class="form-group">
                                                                            <label>Nature of Employment</label>
                                                                            <asp:DropDownList ID="tSpouseNatureEmp" DataTextField="Name" DataValueField="Code" runat="server" CssClass="form-control">
                                                                                <asp:ListItem Value="---Select Nature of Employment---" Selected="True"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <div class="form-group">
                                                                            <label>Business Name</label>
                                                                            <%--//2023-06-18 REMOVED WORD SPA--%>
                                                                            <%--<input runat="server" type="text" maxlength="100" class="form-control text-uppercase" id="Text1" placeholder="SPA Employer's Business Name" />--%>
                                                                            <input runat="server" type="text" maxlength="100" class="form-control text-uppercase" id="tSpouseEmpBusinessName" placeholder="Employer's Business Name" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <div class="form-group">
                                                                            <label>Business Address</label>
                                                                            <%--//2023-06-18 REMOVED WORD SPA--%>
                                                                            <%--<input runat="server" type="text" maxlength="150" class="form-control text-uppercase" id="Text2" placeholder="SPA Employer's Business Address" />--%>
                                                                            <input runat="server" type="text" maxlength="150" class="form-control text-uppercase" id="tSpouseEmpBusinessAddress" placeholder="Employer's Business Address" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <div class="form-group">
                                                                            <label>Business Country</label>
                                                                            <asp:DropDownList ID="ddSPOBusCountry" runat="server" CssClass="form-control text-uppercase">
                                                                                <asp:ListItem Selected="true" Value="PH">Philippines</asp:ListItem>
                                                                                <asp:ListItem Value="AF">Afghanistan</asp:ListItem>
                                                                                <asp:ListItem Value="AX">Aland Islands</asp:ListItem>
                                                                                <asp:ListItem Value="AL">Albania</asp:ListItem>
                                                                                <asp:ListItem Value="DZ">Algeria</asp:ListItem>
                                                                                <asp:ListItem Value="AS">American Samoa</asp:ListItem>
                                                                                <asp:ListItem Value="AD">Andorra</asp:ListItem>
                                                                                <asp:ListItem Value="AO">Angola</asp:ListItem>
                                                                                <asp:ListItem Value="AI">Anguilla</asp:ListItem>
                                                                                <asp:ListItem Value="AQ">Antarctica</asp:ListItem>
                                                                                <asp:ListItem Value="AG">Antigua and Barbuda</asp:ListItem>
                                                                                <asp:ListItem Value="AR">Argentina</asp:ListItem>
                                                                                <asp:ListItem Value="AM">Armenia</asp:ListItem>
                                                                                <asp:ListItem Value="AW">Aruba</asp:ListItem>
                                                                                <asp:ListItem Value="AU">Australia</asp:ListItem>
                                                                                <asp:ListItem Value="AT">Austria</asp:ListItem>
                                                                                <asp:ListItem Value="AZ">Azerbaijan</asp:ListItem>
                                                                                <asp:ListItem Value="BS">Bahamas</asp:ListItem>
                                                                                <asp:ListItem Value="BH">Bahrain</asp:ListItem>
                                                                                <asp:ListItem Value="BD">Bangladesh</asp:ListItem>
                                                                                <asp:ListItem Value="BB">Barbados</asp:ListItem>
                                                                                <asp:ListItem Value="BY">Belarus</asp:ListItem>
                                                                                <asp:ListItem Value="BE">Belgium</asp:ListItem>
                                                                                <asp:ListItem Value="BZ">Belize</asp:ListItem>
                                                                                <asp:ListItem Value="BJ">Benin</asp:ListItem>
                                                                                <asp:ListItem Value="BM">Bermuda</asp:ListItem>
                                                                                <asp:ListItem Value="BT">Bhutan</asp:ListItem>
                                                                                <asp:ListItem Value="BO">Bolivia</asp:ListItem>
                                                                                <asp:ListItem Value="BA">Bosnia and Herzegovina</asp:ListItem>
                                                                                <asp:ListItem Value="BW">Botswana</asp:ListItem>
                                                                                <asp:ListItem Value="BV">Bouvet Island</asp:ListItem>
                                                                                <asp:ListItem Value="BR">Brazil</asp:ListItem>
                                                                                <asp:ListItem Value="VG">British Virgin Islands</asp:ListItem>
                                                                                <asp:ListItem Value="IO">British Indian Ocean Territory</asp:ListItem>
                                                                                <asp:ListItem Value="BN">Brunei Darussalam</asp:ListItem>
                                                                                <asp:ListItem Value="BG">Bulgaria</asp:ListItem>
                                                                                <asp:ListItem Value="BF">Burkina Faso</asp:ListItem>
                                                                                <asp:ListItem Value="BI">Burundi</asp:ListItem>
                                                                                <asp:ListItem Value="KH">Cambodia</asp:ListItem>
                                                                                <asp:ListItem Value="CM">Cameroon</asp:ListItem>
                                                                                <asp:ListItem Value="CA">Canada</asp:ListItem>
                                                                                <asp:ListItem Value="CV">Cape Verde</asp:ListItem>
                                                                                <asp:ListItem Value="KY">Cayman Islands</asp:ListItem>
                                                                                <asp:ListItem Value="CF">Central African Republic</asp:ListItem>
                                                                                <asp:ListItem Value="TD">Chad</asp:ListItem>
                                                                                <asp:ListItem Value="CL">Chile</asp:ListItem>
                                                                                <asp:ListItem Value="CN">China</asp:ListItem>
                                                                                <asp:ListItem Value="HK">Hong Kong, SAR China</asp:ListItem>
                                                                                <asp:ListItem Value="MO">Macao, SAR China</asp:ListItem>
                                                                                <asp:ListItem Value="CX">Christmas Island</asp:ListItem>
                                                                                <asp:ListItem Value="CC">Cocos (Keeling) Islands</asp:ListItem>
                                                                                <asp:ListItem Value="CO">Colombia</asp:ListItem>
                                                                                <asp:ListItem Value="KM">Comoros</asp:ListItem>
                                                                                <asp:ListItem Value="CG">Congo (Brazzaville)</asp:ListItem>
                                                                                <asp:ListItem Value="CD">Congo, (Kinshasa)</asp:ListItem>
                                                                                <asp:ListItem Value="CK">Cook Islands</asp:ListItem>
                                                                                <asp:ListItem Value="CR">Costa Rica</asp:ListItem>
                                                                                <asp:ListItem Value="CI">Côte d'Ivoire</asp:ListItem>
                                                                                <asp:ListItem Value="HR">Croatia</asp:ListItem>
                                                                                <asp:ListItem Value="CU">Cuba</asp:ListItem>
                                                                                <asp:ListItem Value="CY">Cyprus</asp:ListItem>
                                                                                <asp:ListItem Value="CZ">Czech Republic</asp:ListItem>
                                                                                <asp:ListItem Value="DK">Denmark</asp:ListItem>
                                                                                <asp:ListItem Value="DJ">Djibouti</asp:ListItem>
                                                                                <asp:ListItem Value="DM">Dominica</asp:ListItem>
                                                                                <asp:ListItem Value="DO">Dominican Republic</asp:ListItem>
                                                                                <asp:ListItem Value="EC">Ecuador</asp:ListItem>
                                                                                <asp:ListItem Value="EG">Egypt</asp:ListItem>
                                                                                <asp:ListItem Value="SV">El Salvador</asp:ListItem>
                                                                                <asp:ListItem Value="GQ">Equatorial Guinea</asp:ListItem>
                                                                                <asp:ListItem Value="ER">Eritrea</asp:ListItem>
                                                                                <asp:ListItem Value="EE">Estonia</asp:ListItem>
                                                                                <asp:ListItem Value="ET">Ethiopia</asp:ListItem>
                                                                                <asp:ListItem Value="FK">Falkland Islands (Malvinas)</asp:ListItem>
                                                                                <asp:ListItem Value="FO">Faroe Islands</asp:ListItem>
                                                                                <asp:ListItem Value="FJ">Fiji</asp:ListItem>
                                                                                <asp:ListItem Value="FI">Finland</asp:ListItem>
                                                                                <asp:ListItem Value="FR">France</asp:ListItem>
                                                                                <asp:ListItem Value="GF">French Guiana</asp:ListItem>
                                                                                <asp:ListItem Value="PF">French Polynesia</asp:ListItem>
                                                                                <asp:ListItem Value="TF">French Southern Territories</asp:ListItem>
                                                                                <asp:ListItem Value="GA">Gabon</asp:ListItem>
                                                                                <asp:ListItem Value="GM">Gambia</asp:ListItem>
                                                                                <asp:ListItem Value="GE">Georgia</asp:ListItem>
                                                                                <asp:ListItem Value="DE">Germany</asp:ListItem>
                                                                                <asp:ListItem Value="GH">Ghana</asp:ListItem>
                                                                                <asp:ListItem Value="GI">Gibraltar</asp:ListItem>
                                                                                <asp:ListItem Value="GR">Greece</asp:ListItem>
                                                                                <asp:ListItem Value="GL">Greenland</asp:ListItem>
                                                                                <asp:ListItem Value="GD">Grenada</asp:ListItem>
                                                                                <asp:ListItem Value="GP">Guadeloupe</asp:ListItem>
                                                                                <asp:ListItem Value="GU">Guam</asp:ListItem>
                                                                                <asp:ListItem Value="GT">Guatemala</asp:ListItem>
                                                                                <asp:ListItem Value="GG">Guernsey</asp:ListItem>
                                                                                <asp:ListItem Value="GN">Guinea</asp:ListItem>
                                                                                <asp:ListItem Value="GW">Guinea-Bissau</asp:ListItem>
                                                                                <asp:ListItem Value="GY">Guyana</asp:ListItem>
                                                                                <asp:ListItem Value="HT">Haiti</asp:ListItem>
                                                                                <asp:ListItem Value="HM">Heard and Mcdonald Islands</asp:ListItem>
                                                                                <asp:ListItem Value="VA">Holy See (Vatican City State)</asp:ListItem>
                                                                                <asp:ListItem Value="HN">Honduras</asp:ListItem>
                                                                                <asp:ListItem Value="HU">Hungary</asp:ListItem>
                                                                                <asp:ListItem Value="IS">Iceland</asp:ListItem>
                                                                                <asp:ListItem Value="IN">India</asp:ListItem>
                                                                                <asp:ListItem Value="ID">Indonesia</asp:ListItem>
                                                                                <asp:ListItem Value="IR">Iran, Islamic Republic of</asp:ListItem>
                                                                                <asp:ListItem Value="IQ">Iraq</asp:ListItem>
                                                                                <asp:ListItem Value="IE">Ireland</asp:ListItem>
                                                                                <asp:ListItem Value="IM">Isle of Man</asp:ListItem>
                                                                                <asp:ListItem Value="IL">Israel</asp:ListItem>
                                                                                <asp:ListItem Value="IT">Italy</asp:ListItem>
                                                                                <asp:ListItem Value="JM">Jamaica</asp:ListItem>
                                                                                <asp:ListItem Value="JP">Japan</asp:ListItem>
                                                                                <asp:ListItem Value="JE">Jersey</asp:ListItem>
                                                                                <asp:ListItem Value="JO">Jordan</asp:ListItem>
                                                                                <asp:ListItem Value="KZ">Kazakhstan</asp:ListItem>
                                                                                <asp:ListItem Value="KE">Kenya</asp:ListItem>
                                                                                <asp:ListItem Value="KI">Kiribati</asp:ListItem>
                                                                                <asp:ListItem Value="KP">Korea (North)</asp:ListItem>
                                                                                <asp:ListItem Value="KR">Korea (South)</asp:ListItem>
                                                                                <asp:ListItem Value="KW">Kuwait</asp:ListItem>
                                                                                <asp:ListItem Value="KG">Kyrgyzstan</asp:ListItem>
                                                                                <asp:ListItem Value="LA">Lao PDR</asp:ListItem>
                                                                                <asp:ListItem Value="LV">Latvia</asp:ListItem>
                                                                                <asp:ListItem Value="LB">Lebanon</asp:ListItem>
                                                                                <asp:ListItem Value="LS">Lesotho</asp:ListItem>
                                                                                <asp:ListItem Value="LR">Liberia</asp:ListItem>
                                                                                <asp:ListItem Value="LY">Libya</asp:ListItem>
                                                                                <asp:ListItem Value="LI">Liechtenstein</asp:ListItem>
                                                                                <asp:ListItem Value="LT">Lithuania</asp:ListItem>
                                                                                <asp:ListItem Value="LU">Luxembourg</asp:ListItem>
                                                                                <asp:ListItem Value="MK">Macedonia, Republic of</asp:ListItem>
                                                                                <asp:ListItem Value="MG">Madagascar</asp:ListItem>
                                                                                <asp:ListItem Value="MW">Malawi</asp:ListItem>
                                                                                <asp:ListItem Value="MY">Malaysia</asp:ListItem>
                                                                                <asp:ListItem Value="MV">Maldives</asp:ListItem>
                                                                                <asp:ListItem Value="ML">Mali</asp:ListItem>
                                                                                <asp:ListItem Value="MT">Malta</asp:ListItem>
                                                                                <asp:ListItem Value="MH">Marshall Islands</asp:ListItem>
                                                                                <asp:ListItem Value="MQ">Martinique</asp:ListItem>
                                                                                <asp:ListItem Value="MR">Mauritania</asp:ListItem>
                                                                                <asp:ListItem Value="MU">Mauritius</asp:ListItem>
                                                                                <asp:ListItem Value="YT">Mayotte</asp:ListItem>
                                                                                <asp:ListItem Value="MX">Mexico</asp:ListItem>
                                                                                <asp:ListItem Value="FM">Micronesia, Federated States of</asp:ListItem>
                                                                                <asp:ListItem Value="MD">Moldova</asp:ListItem>
                                                                                <asp:ListItem Value="MC">Monaco</asp:ListItem>
                                                                                <asp:ListItem Value="MN">Mongolia</asp:ListItem>
                                                                                <asp:ListItem Value="ME">Montenegro</asp:ListItem>
                                                                                <asp:ListItem Value="MS">Montserrat</asp:ListItem>
                                                                                <asp:ListItem Value="MA">Morocco</asp:ListItem>
                                                                                <asp:ListItem Value="MZ">Mozambique</asp:ListItem>
                                                                                <asp:ListItem Value="MM">Myanmar</asp:ListItem>
                                                                                <asp:ListItem Value="NA">Namibia</asp:ListItem>
                                                                                <asp:ListItem Value="NR">Nauru</asp:ListItem>
                                                                                <asp:ListItem Value="NP">Nepal</asp:ListItem>
                                                                                <asp:ListItem Value="NL">Netherlands</asp:ListItem>
                                                                                <asp:ListItem Value="AN">Netherlands Antilles</asp:ListItem>
                                                                                <asp:ListItem Value="NC">New Caledonia</asp:ListItem>
                                                                                <asp:ListItem Value="NZ">New Zealand</asp:ListItem>
                                                                                <asp:ListItem Value="NI">Nicaragua</asp:ListItem>
                                                                                <asp:ListItem Value="NE">Niger</asp:ListItem>
                                                                                <asp:ListItem Value="NG">Nigeria</asp:ListItem>
                                                                                <asp:ListItem Value="NU">Niue</asp:ListItem>
                                                                                <asp:ListItem Value="NF">Norfolk Island</asp:ListItem>
                                                                                <asp:ListItem Value="MP">Northern Mariana Islands</asp:ListItem>
                                                                                <asp:ListItem Value="NO">Norway</asp:ListItem>
                                                                                <asp:ListItem Value="OM">Oman</asp:ListItem>
                                                                                <asp:ListItem Value="PK">Pakistan</asp:ListItem>
                                                                                <asp:ListItem Value="PW">Palau</asp:ListItem>
                                                                                <asp:ListItem Value="PS">Palestinian Territory</asp:ListItem>
                                                                                <asp:ListItem Value="PA">Panama</asp:ListItem>
                                                                                <asp:ListItem Value="PG">Papua New Guinea</asp:ListItem>
                                                                                <asp:ListItem Value="PY">Paraguay</asp:ListItem>
                                                                                <asp:ListItem Value="PE">Peru</asp:ListItem>
                                                                                <asp:ListItem Value="PN">Pitcairn</asp:ListItem>
                                                                                <asp:ListItem Value="PL">Poland</asp:ListItem>
                                                                                <asp:ListItem Value="PT">Portugal</asp:ListItem>
                                                                                <asp:ListItem Value="PR">Puerto Rico</asp:ListItem>
                                                                                <asp:ListItem Value="QA">Qatar</asp:ListItem>
                                                                                <asp:ListItem Value="RE">Réunion</asp:ListItem>
                                                                                <asp:ListItem Value="RO">Romania</asp:ListItem>
                                                                                <asp:ListItem Value="RU">Russian Federation</asp:ListItem>
                                                                                <asp:ListItem Value="RW">Rwanda</asp:ListItem>
                                                                                <asp:ListItem Value="BL">Saint-Barthélemy</asp:ListItem>
                                                                                <asp:ListItem Value="SH">Saint Helena</asp:ListItem>
                                                                                <asp:ListItem Value="KN">Saint Kitts and Nevis</asp:ListItem>
                                                                                <asp:ListItem Value="LC">Saint Lucia</asp:ListItem>
                                                                                <asp:ListItem Value="MF">Saint-Martin (French part)</asp:ListItem>
                                                                                <asp:ListItem Value="PM">Saint Pierre and Miquelon</asp:ListItem>
                                                                                <asp:ListItem Value="VC">Saint Vincent and Grenadines</asp:ListItem>
                                                                                <asp:ListItem Value="WS">Samoa</asp:ListItem>
                                                                                <asp:ListItem Value="SM">San Marino</asp:ListItem>
                                                                                <asp:ListItem Value="ST">Sao Tome and Principe</asp:ListItem>
                                                                                <asp:ListItem Value="SA">Saudi Arabia</asp:ListItem>
                                                                                <asp:ListItem Value="SN">Senegal</asp:ListItem>
                                                                                <asp:ListItem Value="RS">Serbia</asp:ListItem>
                                                                                <asp:ListItem Value="SC">Seychelles</asp:ListItem>
                                                                                <asp:ListItem Value="SL">Sierra Leone</asp:ListItem>
                                                                                <asp:ListItem Value="SG">Singapore</asp:ListItem>
                                                                                <asp:ListItem Value="SK">Slovakia</asp:ListItem>
                                                                                <asp:ListItem Value="SI">Slovenia</asp:ListItem>
                                                                                <asp:ListItem Value="SB">Solomon Islands</asp:ListItem>
                                                                                <asp:ListItem Value="SO">Somalia</asp:ListItem>
                                                                                <asp:ListItem Value="ZA">South Africa</asp:ListItem>
                                                                                <asp:ListItem Value="GS">South Georgia and the South Sandwich Islands</asp:ListItem>
                                                                                <asp:ListItem Value="SS">South Sudan</asp:ListItem>
                                                                                <asp:ListItem Value="ES">Spain</asp:ListItem>
                                                                                <asp:ListItem Value="LK">Sri Lanka</asp:ListItem>
                                                                                <asp:ListItem Value="SD">Sudan</asp:ListItem>
                                                                                <asp:ListItem Value="SR">Suriname</asp:ListItem>
                                                                                <asp:ListItem Value="SJ">Svalbard and Jan Mayen Islands</asp:ListItem>
                                                                                <asp:ListItem Value="SZ">Swaziland</asp:ListItem>
                                                                                <asp:ListItem Value="SE">Sweden</asp:ListItem>
                                                                                <asp:ListItem Value="CH">Switzerland</asp:ListItem>
                                                                                <asp:ListItem Value="SY">Syrian Arab Republic (Syria)</asp:ListItem>
                                                                                <asp:ListItem Value="TW">Taiwan, Republic of China</asp:ListItem>
                                                                                <asp:ListItem Value="TJ">Tajikistan</asp:ListItem>
                                                                                <asp:ListItem Value="TZ">Tanzania, United Republic of</asp:ListItem>
                                                                                <asp:ListItem Value="TH">Thailand</asp:ListItem>
                                                                                <asp:ListItem Value="TL">Timor-Leste</asp:ListItem>
                                                                                <asp:ListItem Value="TG">Togo</asp:ListItem>
                                                                                <asp:ListItem Value="TK">Tokelau</asp:ListItem>
                                                                                <asp:ListItem Value="TO">Tonga</asp:ListItem>
                                                                                <asp:ListItem Value="TT">Trinidad and Tobago</asp:ListItem>
                                                                                <asp:ListItem Value="TN">Tunisia</asp:ListItem>
                                                                                <asp:ListItem Value="TR">Turkey</asp:ListItem>
                                                                                <asp:ListItem Value="TM">Turkmenistan</asp:ListItem>
                                                                                <asp:ListItem Value="TC">Turks and Caicos Islands</asp:ListItem>
                                                                                <asp:ListItem Value="TV">Tuvalu</asp:ListItem>
                                                                                <asp:ListItem Value="UG">Uganda</asp:ListItem>
                                                                                <asp:ListItem Value="UA">Ukraine</asp:ListItem>
                                                                                <asp:ListItem Value="AE">United Arab Emirates</asp:ListItem>
                                                                                <asp:ListItem Value="GB">United Kingdom</asp:ListItem>
                                                                                <asp:ListItem Value="US">United States of America</asp:ListItem>
                                                                                <asp:ListItem Value="UM">US Minor Outlying Islands</asp:ListItem>
                                                                                <asp:ListItem Value="UY">Uruguay</asp:ListItem>
                                                                                <asp:ListItem Value="UZ">Uzbekistan</asp:ListItem>
                                                                                <asp:ListItem Value="VU">Vanuatu</asp:ListItem>
                                                                                <asp:ListItem Value="VE">Venezuela (Bolivarian Republic)</asp:ListItem>
                                                                                <asp:ListItem Value="VN">Vietnam</asp:ListItem>
                                                                                <asp:ListItem Value="VI">Virgin Islands, US</asp:ListItem>
                                                                                <asp:ListItem Value="WF">Wallis and Futuna Islands</asp:ListItem>
                                                                                <asp:ListItem Value="EH">Western Sahara</asp:ListItem>
                                                                                <asp:ListItem Value="YE">Yemen</asp:ListItem>
                                                                                <asp:ListItem Value="ZM">Zambia</asp:ListItem>
                                                                                <asp:ListItem Value="ZW">Zimbabwe</asp:ListItem>

                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <div class="form-group">
                                                                            <label>Position</label>
                                                                            <input runat="server" maxlength="150" type="text" class="form-control text-uppercase" id="tSpousePosition" placeholder="Position" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <div class="form-group">
                                                                            <label>Years in Service</label>
                                                                            <input runat="server" type="text" maxlength="5" onkeypress="return isNumberKey(event)" class="form-control" id="tSpouseYearsofService" value="0" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <div class="form-group">
                                                                            <label>Office Tel No.</label>
                                                                            <input runat="server" type="text" class="form-control" id="tSpouseOfficeTelNo" maxlength="13" onkeypress="return fnAllowNumeric(event)" placeholder="xx xxx xxxx" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <div class="form-group">
                                                                            <label>Fax No.</label>
                                                                            <input runat="server" type="text" class="form-control" id="tSpouseFAXNo" maxlength="14" onkeypress="return fnAllowNumeric(event)" placeholder="x xxx xxx xxxx" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <div class="form-group">
                                                                            <label>TIN No. <span class="color-red fsize-16">*</span></label>
                                                                            <%--<input runat="server" type="text" onkeyup="TIN_keyup(event);" class="form-control" id="tSpouseTIN" onkeypress="return fnAllowNumeric(event)" placeholder="xxx xxx xxx xxx" maxlength="15" />--%>
                                                                            <%--<input runat="server" type="text" class="form-control" id="tSpouseTIN" onkeypress="return fnAllowNumeric(event)" placeholder="xxx xxx xxx xxx" maxlength="15" />--%>

                                                                            <%--COMMENTED 06 - 13 - 2023 : CHANGE REQUEST  https://direcbti.monday.com/boards/4462005457/pulses/4576973637?asset_id=908456598  --%>
                                                                            <asp:TextBox runat="server" ID="tSpouseTIN2" ValidationGroup="SaveBuyer2" class="form-control" type="text"
                                                                                onkeypress="return fnAllowNumeric(event)" placeholder="xxx-xxx-xxx-xxx" MaxLength="15"></asp:TextBox>


                                                                            <asp:RequiredFieldValidator
                                                                                ID="RequiredFieldValidator12"
                                                                                ErrorMessage="Please fill up Spouse TIN."
                                                                                SetFocusOnError="true"
                                                                                ControlToValidate="tSpouseTIN2"
                                                                                Text="Please fill up TIN."
                                                                                ValidationGroup="SaveBuyer2"
                                                                                runat="server" Style="color: red" />

                                                                            <%--  //2023-06-15 : ADD VALIDATOR FOR SPOUSE TIN FORMAT--%>
                                                                            <asp:CustomValidator
                                                                                ValidateRequestMode="Enabled"
                                                                                Style="color: red"
                                                                                runat="server"
                                                                                ID="CustomValidator14"
                                                                                ValidationGroup="SaveBuyer2"
                                                                                ControlToValidate="tSpouseTIN2"
                                                                                ErrorMessage="Invalid Spouse TIN format. Must be 000-000-000-000"
                                                                                SetFocusOnError="true"
                                                                                Text="Invalid TIN format. Must be 000-000-000-000"
                                                                                OnServerValidate="CustomValidator14_ServerValidate" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <div class="form-group">
                                                                            <label>SSS No.</label>
                                                                            <input runat="server" type="text" class="form-control" id="tSpouseSSSNo" maxlength="12" onkeypress="return fnAllowNumeric(event)" placeholder="xx xxxxxxx x" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <div class="form-group">
                                                                            <label>GSIS No.</label>
                                                                            <input runat="server" type="text" class="form-control" id="tSpouseGSISNo" maxlength="14" onkeypress="return fnAllowNumeric(event)" placeholder="xxxx xxxxxxx x" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <div class="form-group">
                                                                            <label>PAG-IBIG No.</label>
                                                                            <input runat="server" type="text" class="form-control" id="tSpousePagibigNo" maxlength="14" onkeypress="return fnAllowNumeric(event)" placeholder="xxxx xxxxxxx x" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <%--<div class="tab-pane tabcontent" id="tab_3" style="display: none">--%>
                                <div class="tab-pane" id="tab_3">
                                    <div class="content">
                                        <%--                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>--%>
                                        <div class="row">
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 hidden">
                                                <div class="row">
                                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                        <div class="panel panel-info">
                                                            <div class="panel-heading">
                                                                <h3 class="panel-title" style="text-align: center;">Home Ownership</h3>
                                                            </div>
                                                            <div class="panel-body">
                                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                    <div class="row">
                                                                        <div class="col-lg-2 col-md-2 col-sm-6 col-xs-12">
                                                                            <div class="row">
                                                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                                    <%--   <input runat="server" type="radio" class="SPAHomeOwnership" name="SPAHomeOwnership" value="Owned" id="tSPAOwned" checked />
                                                                                            Owned--%>
                                                                                    <asp:RadioButton runat="server" CssClass="radio" Text="Owned" ID="tSPAOwned" OnCheckedChanged="tRented_CheckedChanged" AutoPostBack="True" GroupName="SPAHomeOwnership" Checked="true" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-lg-2 col-md-2 col-sm-6 col-xs-12">
                                                                            <div class="row">
                                                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                                    <%--<input runat="server" type="radio" class="SPAHomeOwnership" name="SPAHomeOwnership" value="Mortgaged" id="tSPAMortgaged" />
                                                                                            Mortgaged--%>
                                                                                    <asp:RadioButton runat="server" CssClass="radio" Text="Mortgaged" ID="tSPAMortgaged" OnCheckedChanged="tRented_CheckedChanged" AutoPostBack="True" GroupName="SPAHomeOwnership" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                                                                            <div class="row">
                                                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                                    <%-- <input runat="server" type="radio" class="SPAHomeOwnership" name="SPAHomeOwnership" value="LivingwRelatives" id="tSPALivingwRelatives" />
                                                                                            Living w/ Relatives--%>
                                                                                    <asp:RadioButton runat="server" CssClass="radio" Text="Living w/ Relatives" ID="tSPALivingwRelatives" OnCheckedChanged="tRented_CheckedChanged" AutoPostBack="True" GroupName="SPAHomeOwnership" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-lg-5 col-md-5 col-sm-6 col-xs-12">
                                                                            <div class="row">
                                                                                <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                                                                    <%--<input runat="server" type="radio" class="SPAHomeOwnership" name="SPAHomeOwnership" value="Rented" id="tSPARented" />
                                                                                            Rented--%>
                                                                                    <asp:RadioButton runat="server" CssClass="radio" Text="Rented" ID="tSPARented" OnCheckedChanged="tRented_CheckedChanged" AutoPostBack="True" GroupName="SPAHomeOwnership" />
                                                                                </div>
                                                                                <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                                                                    <input type="text" placeholder="Php. per Month" runat="server" class="tSPAPerMonth form-control" id="tSPAPerMonth" disabled />
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
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <%--                                                        <asp:UpdatePanel runat="server">
                                                            <ContentTemplate>--%>
                                                <div class="panel panel-info">
                                                    <div class="panel-heading">
                                                        <h3 class="panel-title" style="text-align: center;">List of SPA</h3>
                                                    </div>
                                                    <div class="panel-body">
                                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                            <div class="row">
                                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                    <%--                 <asp:UpdatePanel runat="server">
                                                                                        <ContentTemplate>--%>
                                                                    <button class="btn btn-default btn-primary pull-left" type="button" data-toggle="modal" data-target="#MsgSPACoBorrower" onserverclick="btnSPACoBorrowerModal_ServerClick" runat="server" id="btnSPACoBorrowerModal">Add SPA <i class="fa fa-plus-square"></i></button>
                                                                    <%--                              </ContentTemplate>
                                                                                    </asp:UpdatePanel>--%>
                                                                </div>
                                                            </div>
                                                            <br />
                                                            <div class="row">
                                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                    <%--                                                                                    <asp:UpdatePanel runat="server">
                                                                                        <ContentTemplate>--%>
                                                                    <fieldset>
                                                                        <asp:GridView ID="gvSPACoBorrower" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                                                            CssClass="table table-bordered table-hover" Width="100%" ShowHeader="True" OnPageIndexChanging="gvSPACoBorrower_PageIndexChanging">
                                                                            <HeaderStyle BackColor="#66ccff" />
                                                                            <Columns>
                                                                                <asp:BoundField DataField="Id" HeaderText="ID" SortExpression="ID" ItemStyle-Font-Size="Medium" />
                                                                                <asp:BoundField DataField="Relationship" HeaderText="Relationship" SortExpression="Relationship" ItemStyle-Font-Size="Medium" />
                                                                                <%--COMMENTED FOR NEW SPA PROCESS--%>
                                                                                <%--<asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-Font-Size="Medium" />--%>
                                                                                <asp:BoundField DataField="LastName" HeaderText="LastName" SortExpression="LastName" ItemStyle-Font-Size="Medium" />
                                                                                <asp:BoundField DataField="FirstName" HeaderText="FirstName" SortExpression="FirstName" ItemStyle-Font-Size="Medium" />
                                                                                <asp:BoundField DataField="MiddleName" HeaderText="MiddleName" SortExpression="MiddleName" ItemStyle-Font-Size="Medium" />
                                                                                <asp:BoundField DataField="CivilStatus" HeaderText="CivilStatus" SortExpression="CivilStatus" ItemStyle-Font-Size="Medium" />
                                                                                <%--5--%>
                                                                                <asp:BoundField DataField="YearsOfStay" HeaderText="YearsOfStay" SortExpression="YearsOfStay" ItemStyle-Font-Size="Medium" />
                                                                                <asp:BoundField DataField="Address" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" HeaderText="Address" SortExpression="Address" ItemStyle-Font-Size="Medium" />
                                                                                <asp:BoundField DataField="BirthDate" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" HeaderText="BirthDate" SortExpression="BirthDate" ItemStyle-Font-Size="Medium" />
                                                                                <asp:BoundField DataField="BirthPlace" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" HeaderText="BirthPlace" SortExpression="BirthPlace" ItemStyle-Font-Size="Medium" />
                                                                                <asp:BoundField DataField="Gender" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" HeaderText="Gender" SortExpression="Gender" ItemStyle-Font-Size="Medium" />
                                                                                <%--10--%>
                                                                                <asp:BoundField DataField="Citizenship" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" HeaderText="Citizenship" SortExpression="Citizenship" ItemStyle-Font-Size="Medium" />
                                                                                <asp:BoundField DataField="Email" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" HeaderText="Email" SortExpression="Email" ItemStyle-Font-Size="Medium" />
                                                                                <asp:BoundField DataField="TelNo" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" HeaderText="TelNo" SortExpression="TelNo" ItemStyle-Font-Size="Medium" />
                                                                                <asp:BoundField DataField="MobileNo" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" HeaderText="MobileNo" SortExpression="MobileNo" ItemStyle-Font-Size="Medium" />
                                                                                <asp:BoundField DataField="FB" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" HeaderText="FB" SortExpression="FB" ItemStyle-Font-Size="Medium" />
                                                                                <%--15--%>
                                                                                <asp:BoundField DataField="SPAFormDocument" HeaderText="SPAFormDocument" SortExpression="SPAFormDocument" ItemStyle-Font-Size="Medium" />
                                                                                <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>
                                                                                        <%-- //2023-06-16 : CHANGE SEQUENCE OF LOADING DATA + OPENING MODAL--%>
                                                                                        <%--<asp:LinkButton runat="server" ID="gvSPACBEdit" CssClass="btn btn-default btn-warning" OnClientClick="MsgSPACoBorrower_Show();" Width="100%" Height="100%" CommandArgument='<%# Bind("ID")%>' OnClick="gvSPACBEdit_Click"><i class="fa fa-edit"></i></asp:LinkButton>--%>

                                                                                        <%-- //2023-06-18 COMMENTED FOR NEW SPA PROCESS--%>
                                                                                        <%--<asp:LinkButton runat="server" ID="LinkButton2" CssClass="btn btn-default btn-warning" Width="100%" Height="100%" CommandArgument='<%# Bind("ID")%>' OnClick="gvSPACBEdit_Click"><i class="fa fa-edit"></i></asp:LinkButton>--%>
                                                                                        <asp:LinkButton runat="server" ID="gvSPACBEdit" CssClass="btn btn-default btn-warning" Width="100%" Height="100%" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" OnClick="gvSPACBEdit_Click"><i class="fa fa-edit"></i></asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>
                                                                                        <%-- //2023-06-18 COMMENTED FOR NEW SPA PROCESS--%>
                                                                                        <%--<asp:LinkButton runat="server" ID="LinkButton2" CssClass="btn btn-default btn-danger" Width="100%" Height="100%" CommandArgument='<%# Bind("ID")%>' OnClick="gvSPACBDelete_Click"><i class="fa fa-trash-o"></i></asp:LinkButton>--%>
                                                                                        <asp:LinkButton runat="server" ID="gvSPACBDelete" CssClass="btn btn-default btn-danger" Width="100%" Height="100%" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" OnClick="gvSPACBDelete_Click"><i class="fa fa-trash-o"></i></asp:LinkButton>

                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            <PagerStyle CssClass="pagination-ys" />
                                                                            <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                                                        </asp:GridView>
                                                                    </fieldset>
                                                                    <%--                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>--%>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <%--                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>--%>
                                                    </div>
                                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 hidden">
                                                        <div class="row">
                                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                <div class="panel panel-info">
                                                                    <div class="panel-heading">
                                                                        <h3 class="panel-title" style="text-align: center;">Dependents</h3>
                                                                    </div>
                                                                    <div class="panel-body">
                                                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                            <div class="row">
                                                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                                    <%--                                                                                    <asp:UpdatePanel runat="server">
                                                                                        <ContentTemplate>--%>
                                                                                    <button class="btn btn-default btn-primary pull-right" type="button" data-toggle="modal" data-target="#MsgDependent" runat="server" id="bSPADependent" onserverclick="bDependent_ServerClick">Dependent <i class="fa fa-plus-square"></i></button>
                                                                                    <%--                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>--%>
                                                                                </div>
                                                                            </div>
                                                                            <br />
                                                                            <div class="row">
                                                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                                    <%--                                                                                    <asp:UpdatePanel runat="server">
                                                                                        <ContentTemplate>--%>
                                                                                    <fieldset>
                                                                                        <asp:GridView ID="gvSPADependent" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                                                                            CssClass="table table-bordered table-hover" Width="100%" ShowHeader="True" PageSize="3" OnPageIndexChanging="gvSPADependent_PageIndexChanging">
                                                                                            <HeaderStyle BackColor="#66ccff" />
                                                                                            <Columns>
                                                                                                <asp:BoundField DataField="Name" HeaderText="Name of Dependents" SortExpression="Name" ItemStyle-Font-Size="Medium" />
                                                                                                <asp:BoundField DataField="Age" HeaderText="Ages" SortExpression="Age" ItemStyle-Font-Size="Medium" />
                                                                                                <asp:BoundField DataField="Relationship" HeaderText="Relationship" SortExpression="Relationship" ItemStyle-Font-Size="Medium" />
                                                                                                <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:LinkButton runat="server" ID="gvSPADelete" CssClass="btn btn-default btn-danger" Width="100%" Height="100%" CommandArgument='<%# Bind("ID")%>' OnClick="gvDelete_Click"><i class="fa fa-trash-o"></i></asp:LinkButton>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                            </Columns>
                                                                                            <PagerStyle CssClass="pagination-ys" />
                                                                                            <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                                                                        </asp:GridView>
                                                                                    </fieldset>
                                                                                    <%--                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>--%>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <%--                                            </ContentTemplate>
                                        </asp:UpdatePanel>--%>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6" style="border-right-color: lightgray; border-right-width: thin; border-right-style: solid;">
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <h5 style="text-align: center;">SPECIAL INSTRUCTIONS</h5>
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <h5>For written communications please send to:</h5>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <div class="input-group">
                                                <input runat="server" style="z-index: auto;" id="tSpecialInstructions" class="form-control text-uppercase" type="text" disabled required /><span class="input-group-btn">
                                                    <button id="bSpecialInstructions" runat="server" style="width: 50px; z-index: auto;" data-toggle="modal" data-target="#MsgEmployment" class="btn btn-primary btn-dropbox" type="button" onserverclick="btnEmployment_ServerClick"><i class="fa fa-bars"></i></button>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <%--                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>--%>
                                        <div class="col-lg-12  col-md-12 col-sm-12 col-xs-12">
                                            <div class="form-group">
                                                <h5>Remarks/Recommendations</h5>
                                                <textarea runat="server" id="tRemarks" rows="5" class="text-uppercase form-control" style="width: 100%; max-width: 100%; min-width: 100%; height: 50px; max-height: 150px; min-height: 50px;" />
                                            </div>
                                        </div>
                                        <%--                                    </ContentTemplate>
                                </asp:UpdatePanel>--%>
                                    </div>
                                </div>
                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="text-align: center;">
                                        <asp:CheckBox runat="server" ID="CBconforme" Font-Bold="false" onclick="return false;" Checked="true" Text="&nbsp; I/We certify that all information furnished herein are true and correct &nbsp;" /><span class="color-red fsize-16">*</span>
                                        <asp:CustomValidator
                                            Style="color: red"
                                            runat="server"
                                            ID="CustomValidator8" CssClass="col-md-12"
                                            ValidationGroup="SaveBuyer2"
                                            Text="Please confirm all your information as true and correct before proceeding."
                                            OnServerValidate="CustomValidator8_ServerValidate" />
                                        <%--<h5 style="text-align: center;">I/We certify that all information furnished herein are true and correct</h5>--%>
                                    </div>
                                    <div class="row" runat="server" id="ConformeCorp" visible="false">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <div class="form-group">
                                                <label>Corporation Name</label>
                                                <input runat="server" readonly type="text" class="form-control text-uppercase" id="txtConformeCorp" placeholder="Corporation Name" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <div class="form-group">
                                                <label>Buyer's Full Name</label>
                                                <input runat="server" readonly type="text" class="form-control text-uppercase" id="txtCertifyCompleteName" placeholder="Buyer's Full Name" />
                                            </div>
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator37"
                                                ControlToValidate="txtCertifyCompleteName" CssClass="col-md-12"
                                                Text="Please fill up Buyer's Full Name."
                                                ValidationGroup="SaveBuyer2"
                                                runat="server" Style="color: red" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
                                            <div class="form-group">
                                                <label>Date</label>
                                                <input runat="server" type="date" class="form-control text-uppercase" id="txtCertifyDate" />
                                            </div>
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator38"
                                                ControlToValidate="txtCertifyDate" CssClass="col-md-12"
                                                Text="Please fill up Date."
                                                ValidationGroup="SaveBuyer2"
                                                runat="server" Style="color: red" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <asp:ValidationSummary DisplayMode="BulletList" Style="color: red" ID="validation_sum" runat="server" CssClass="pull-right" ValidationGroup="SaveBuyer2" />
                                </div>
                                <div class="col-md-12">
                                    <button class="btn btn-primary nextBtn pull-left" type="button" id="Button3" runat="server" onserverclick="btnBack_Click">Back</button>
                                    <asp:LinkButton runat="server" CssClass="btn btn-success btn-width pull-right" ID="btnSave" ValidationGroup="SaveBuyer2" OnClientClick="formValid();" OnClick="btnSave_ServerClick">
                                        <label runat="server" id="lblSave" style="height: 5px; font-weight: normal;" />
                                        <i class="glyphicon glyphicon-check"></i>
                                    </asp:LinkButton>
                                    <%--<button class="btn btn-primary nextBtn pull-right" type="button" id="Button4" runat="server" onserverclick="btnNext3_ServerClick">Save All Information</button>--%>
                                </div>
                            </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <br />
    <div class="container-fluid">
        <%-- ######################## Accounting ######################## --%>
        <div class="tab-pane fade in active tab-bg hidden" role="tabpanel" id="TabAccounting">
            <div class="container tab-container">
                <div class="br">
                    <br />
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <div class="row">
                                <div class="col-lg-7 col-md-7 col-sm-12 col-xs-12">
                                    <div class="panel panel-info">
                                        <div class="panel-heading">
                                            <h3 class="panel-title" style="text-align: center;">Monthly Income</h3>
                                        </div>
                                        <%--                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>--%>
                                        <div class="panel-body">

                                            <div class="row">
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <h5 style="text-align: center; font-weight: bold;">Principal</h5>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <h5 style="text-align: center; font-weight: bold;">Spouse</h5>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <h5 style="text-align: center; font-weight: bold;">Total</h5>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <h5>Basic Salary</h5>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <%--  <input runat="server" class="BasicSalary PTotal form-control" type="number" value="0" id="tPBasicSalary" />--%>

                                                    <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control" Text="0.00" ID="tPBasicSalary" TextMode="Number" OnTextChanged="Text_TextChanged" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <%--<input runat="server" class="BasicSalary STotal form-control" type="number" value="0" id="tSBasicSalary" />--%>

                                                    <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control" Text="0.00" ID="tSBasicSalary" TextMode="Number" OnTextChanged="Text_TextChanged" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <%--<input runat="server" type="number" class="tTBasicSalary form-control" value="0" id="tTBasicSalary" disabled />--%>
                                                    <input runat="server" class="form-control" id="tTBasicSalary" disabled />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <h5>Allowances</h5>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <%--<input runat="server" type="number" class="Allowances PTotal form-control" id="tPAllowances" value="0" />--%>

                                                    <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control" Text="0.00" ID="tPAllowances" TextMode="Number" OnTextChanged="Text_TextChanged" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <%--<input runat="server" type="number" class="Allowances STotal form-control" id="tSAllowances" value="0" />--%>

                                                    <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control" Text="0.00" ID="tSAllowances" TextMode="Number" OnTextChanged="Text_TextChanged" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <%--<input runat="server" type="number" class="tTAllowances form-control" id="tTAllowances" value="0" disabled />--%>
                                                    <input runat="server" class="form-control" id="tTAllowances" disabled />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <h5>Comissions</h5>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <%--<input runat="server" type="number" class="Commissions PTotal form-control" id="tPCommissions" value="0" />--%>

                                                    <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control" Text="0.00" ID="tPCommissions" TextMode="Number" OnTextChanged="Text_TextChanged" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <%--<input runat="server" type="number" class="Commissions STotal form-control" id="tSCommissions" value="0" />--%>

                                                    <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control" Text="0.00" ID="tSCommissions" TextMode="Number" OnTextChanged="Text_TextChanged" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <%--<input runat="server" type="number" class="tTCommissions form-control" id="tTCommissions" value="0" disabled />--%>
                                                    <input runat="server" class="form-control" id="tTCommissions" disabled />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <h5>Rental Income</h5>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <%--<input runat="server" type="number" class="RentalIncome PTotal form-control" id="tPRentalIncome" value="0" />--%>

                                                    <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control" Text="0.00" ID="tPRentalIncome" TextMode="Number" OnTextChanged="Text_TextChanged" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <%--<input runat="server" type="number" class="RentalIncome STotal form-control" id="tSRentalIncome" value="0" />--%>

                                                    <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control" Text="0.00" ID="tSRentalIncome" TextMode="Number" OnTextChanged="Text_TextChanged" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <%--<input runat="server" type="number" class="tTRentalIncome form-control" id="tTRentalIncome" value="0" disabled />--%>
                                                    <input runat="server" class="form-control" id="tTRentalIncome" disabled />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <h5>Retainer's Fee</h5>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <%--<input runat="server" type="number" class="Retainer PTotal form-control" id="tPRetainer" value="0" />--%>

                                                    <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control" Text="0.00" ID="tPRetainer" TextMode="Number" OnTextChanged="Text_TextChanged" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <%--<input runat="server" type="number" class="Retainer STotal form-control" id="tSRetainer" value="0" />--%>

                                                    <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control" Text="0.00" ID="tSRetainer" TextMode="Number" OnTextChanged="Text_TextChanged" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <%--<input runat="server" type="number" class="tTRetainer form-control" id="tTRetainer" value="0" disabled />--%>
                                                    <input runat="server" class="form-control" id="tTRetainer" disabled />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <h5>Others</h5>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <%--<input runat="server" type="number" class="MIOthers PTotal form-control" id="tPOthers" value="0" />--%>

                                                    <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control" Text="0.00" ID="tPOthers" TextMode="Number" OnTextChanged="Text_TextChanged" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <%--<input runat="server" type="number" class="MIOthers STotal form-control" id="tSOthers" value="0" />--%>

                                                    <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control" Text="0.00" ID="tSOthers" TextMode="Number" OnTextChanged="Text_TextChanged" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <%--<input runat="server" type="number" class="tTOthers form-control" id="tTOthers" value="0" disabled />--%>
                                                    <input runat="server" class="form-control" id="tTOthers" disabled />
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <%--<input runat="server" type="number" name="MITotal" class="tPTotal MITotal form-control" id="tPTotal" value="0" disabled />--%>
                                                    <input runat="server" class="form-control" id="tPTotal" disabled />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <%--<input runat="server" type="number" name="MITotal" class="tSTotal MITotal form-control" id="tSTotal" value="0" disabled />--%>
                                                    <input runat="server" class="form-control" id="tSTotal" disabled />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <%--  <input runat="server" type="number" class="tMITotal form-control" id="tMITotal" value="0" disabled />--%>
                                                    <input runat="server" class="form-control" id="tMITotal" disabled />
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-lg-9 col-md-9 col-sm-9 col-xs-12">
                                                    <h5>Required Monthly Income</h5>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <%--<input type="number" class="form-control" id="tRequiredMonthly" value="0" disabled />--%>

                                                    <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control" Text="0.00" ID="tRequiredMonthly" TextMode="Number" OnTextChanged="Text_TextChanged" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-9 col-md-9 col-sm-9 col-xs-12">
                                                    <h5>Monthly Amortization</h5>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                    <%--<input runat="server" type="number" class="form-control" id="tMonthlyAmort" value="0" disabled />--%>
                                                    <input runat="server" class="form-control" id="tMonthlyAmort" disabled />
                                                </div>
                                            </div>
                                        </div>
                                        <%--                                            </ContentTemplate>
                                        </asp:UpdatePanel>--%>
                                    </div>
                                </div>
                                <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                    <%--                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>--%>
                                    <div class="panel panel-info">
                                        <div class="panel-heading">
                                            <h3 class="panel-title" style="text-align: center;">Monthly Expenses</h3>
                                        </div>
                                        <div class="panel-body">
                                            <div class="row">
                                                <div class="col-lg-7 col-md-7 col-sm-7 col-xs-12">
                                                    <h5 style="font-weight: bold;">Living Ttilities</h5>
                                                </div>
                                                <div class="col-lg-5 col-md-5 col-sm-5 col-xs-12">
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-7 col-md-7 col-sm-7 col-xs-12">
                                                    <h5>Food</h5>
                                                </div>
                                                <div class="col-lg-5 col-md-5 col-sm-5 col-xs-12">
                                                    <%--<input runat="server" type="number" class="METotal form-control" id="tFood" value="0" />--%>

                                                    <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control" Text="0.00" ID="tFood" TextMode="Number" OnTextChanged="Text_TextChanged" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-7 col-md-7 col-sm-7 col-xs-12">
                                                    <h5>Light & Water</h5>
                                                </div>
                                                <div class="col-lg-5 col-md-5 col-sm-5 col-xs-12">
                                                    <%--<input runat="server" type="number" class="METotal form-control" id="tLightWater" value="0" />--%>

                                                    <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control" Text="0.00" ID="tLightWater" TextMode="Number" OnTextChanged="Text_TextChanged" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-7 col-md-7 col-sm-7 col-xs-12">
                                                    <h5>Tel. Bill</h5>
                                                </div>
                                                <div class="col-lg-5 col-md-5 col-sm-5 col-xs-12">
                                                    <%--<input runat="server" type="number" class="METotal form-control" id="tTelephoneBill" value="0" />--%>

                                                    <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control" Text="0.00" ID="tTelephoneBill" TextMode="Number" OnTextChanged="Text_TextChanged" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-7 col-md-7 col-sm-7 col-xs-12">
                                                    <h5>Transportation</h5>
                                                </div>
                                                <div class="col-lg-5 col-md-5 col-sm-5 col-xs-12">
                                                    <%--<input runat="server" type="number" class="METotal form-control" id="tTransportation" value="0" />--%>

                                                    <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control" Text="0.00" ID="tTransportation" TextMode="Number" OnTextChanged="Text_TextChanged" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-7 col-md-7 col-sm-7 col-xs-12">
                                                    <h5>Rent</h5>
                                                </div>
                                                <div class="col-lg-5 col-md-5 col-sm-5 col-xs-12">
                                                    <%--<input runat="server" type="number" class="METotal form-control" id="tRent" value="0" />--%>

                                                    <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control" Text="0.00" ID="tRent" TextMode="Number" OnTextChanged="Text_TextChanged" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-7 col-md-7 col-sm-7 col-xs-12">
                                                    <h5>Education</h5>
                                                </div>
                                                <div class="col-lg-5 col-md-5 col-sm-5 col-xs-12">
                                                    <%--<input runat="server" type="number" class="METotal form-control" id="tEducation" value="0" />--%>

                                                    <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control" Text="0.00" ID="tEducation" TextMode="Number" OnTextChanged="Text_TextChanged" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-7 col-md-7 col-sm-7 col-xs-12">
                                                    <h5>Loan Amort</h5>
                                                </div>
                                                <div class="col-lg-5 col-md-5 col-sm-5 col-xs-12">
                                                    <%-- <input runat="server" type="number" class="METotal form-control" id="tLoanAmort" value="0" />--%>

                                                    <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control" Text="0.00" ID="tLoanAmort" TextMode="Number" OnTextChanged="Text_TextChanged" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-7 col-md-7 col-sm-7 col-xs-12">
                                                    <h5>Others</h5>
                                                </div>
                                                <div class="col-lg-5 col-md-5 col-sm-5 col-xs-12">
                                                    <%--<input runat="server" type="number" class="METotal form-control" id="tMEOthers" value="0" />--%>

                                                    <asp:TextBox runat="server" AutoPostBack="true" CssClass="form-control" Text="0.00" ID="tMEOthers" TextMode="Number" OnTextChanged="Text_TextChanged" />
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-lg-7 col-md-7 col-sm-7 col-xs-12">
                                                    <h5 style="font-weight: bold;">TOTAL</h5>
                                                </div>
                                                <div class="col-lg-5 col-md-5 col-sm-5 col-xs-12">
                                                    <%--<input runat="server" type="number" class="tMETotal form-control" id="tMETotal" value="0" disabled />--%>
                                                    <input runat="server" class="form-control" id="tMETotal" disabled />
                                                </div>
                                            </div>
                                            &nbsp
                                        </div>
                                    </div>
                                    <%--                                        </ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                </div>
                            </div>

                        </div>

                    </div>
                </div>

            </div>
        </div>
        <%-- ######################## Accounting ######################## --%>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="modals" runat="server">

    <%--ADD MODAL SUCCESS--%>
    <div id="modalSuccess" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="row" style="text-align: center;">
                                <div class="col-lg-12">
                                    <asp:Image ImageUrl="~/assets/img/success.png" runat="server" ID="Image1" Width="90" Height="90" />
                                </div>
                            </div>
                            <div class="row" style="text-align: center;">
                                <div class="col-lg-12">
                                    <asp:Label runat="server" ID="mtxtUniqueId"></asp:Label>
                                </div>
                            </div>
                            <div class="row" style="text-align: center;">
                                <div class="col-lg-12">
                                    <asp:LinkButton type="button" runat="server" class="btn btn-primary btn-sm" Style="width: 90px;" OnClick="Reload_ServerClick" Text="Ok" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--MODAL ALERT--%>
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

    <div class="modal fade" id="MsgCoowner" role="dialog" tabindex="-1">
        <div class="modal-dialog" role="document">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="modal-content" id="">
                        <div class="modal-header bg-color">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                            <h4 class="modal-title" runat="server" id="coother">Add Co-Owner</h4>
                        </div>
                        <div class="modal-body">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <h5>First Name: <span class="color-red fsize-16">*</span></h5>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <input runat="server" type="text" class="form-control text-uppercase" id="tCoFName" placeholder="Your First Name Here" />
                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator22"
                                                        ControlToValidate="tCoFName" CssClass="col-md-12"
                                                        Text="Please fill up First Name."
                                                        ValidationGroup="addcoowner"
                                                        runat="server" Style="color: red" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <h5>Middle Name: <span class="color-red fsize-16">*</span></h5>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <input runat="server" type="text" class="form-control text-uppercase" id="tCoMName" placeholder="Your Middle Name Here" />
                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator16"
                                                        ControlToValidate="tCoMName" CssClass="col-md-12"
                                                        Text="Please fill up Middle Name."
                                                        ValidationGroup="addcoowner"
                                                        runat="server" Style="color: red" />
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <h5>Last Name: <span class="color-red fsize-16">*</span></h5>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <input runat="server" type="text" class="form-control text-uppercase" id="tCoLName" placeholder="Your Last Name Here" />
                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator17"
                                                        ControlToValidate="tCoLName" CssClass="col-md-12"
                                                        Text="Please fill up Last Name."
                                                        ValidationGroup="addcoowner"
                                                        runat="server" Style="color: red" />
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <h5>Relationship to Principal Buyer: <span class="color-red fsize-16">*</span></h5>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <input runat="server" type="text" class="form-control text-uppercase" id="tCoRelationship" placeholder="Your Relationship to Principal Buyer Here" />
                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator18"
                                                        ControlToValidate="tCoRelationship" CssClass="col-md-12"
                                                        Text="Please fill up Relationship."
                                                        ValidationGroup="addcoowner"
                                                        runat="server" Style="color: red" />
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <h5>Email:</h5>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <input runat="server" type="text" class="form-control text-uppercase" id="tCoEmail" placeholder="Your Email Here" />
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <h5>Mobile No:</h5>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <input runat="server" type="text" class="form-control text-uppercase" id="tCoMobileNo" placeholder="Your Mobile No Here" />
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <h5>Address:</h5>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <input runat="server" type="text" class="form-control text-uppercase" id="tCoAddress" placeholder="Your Address Here" />
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <h5>Residence:</h5>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <input runat="server" type="text" class="form-control text-uppercase" id="tCoResidence" placeholder="Your Residence Here" />
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <h5>Valid ID: <span class="color-red fsize-16">*</span></h5>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <asp:DropDownList ID="tCoValidID" runat="server" DataTextField="Name" DataValueField="Code" CssClass="form-control">
                                                        <asp:ListItem Value="---Select Valid ID---" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Value="ID1" Text="TIN"> </asp:ListItem>
                                                        <asp:ListItem Value="ID2" Text="SSS"> </asp:ListItem>
                                                        <asp:ListItem Value="ID3" Text="Pagibig"> </asp:ListItem>
                                                        <asp:ListItem Value="ID4" Text="Passport"> </asp:ListItem>
                                                        <asp:ListItem Value="OTH" Text="Others"> </asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator20"
                                                        ControlToValidate="tCoValidID" CssClass="col-md-12"
                                                        Text="Please select Valid ID."
                                                        InitialValue="---Select Valid ID---"
                                                        ValidationGroup="addcoowner"
                                                        runat="server" Style="color: red" />
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <h5>Valid ID No: <span class="color-red fsize-16">*</span></h5>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <input runat="server" type="text" class="form-control text-uppercase" id="tCoValidIDNo" placeholder="Your Valid ID No Here" />
                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator19"
                                                        ControlToValidate="tCoValidIDNo" CssClass="col-md-12"
                                                        Text="Please fill up Valid ID No."
                                                        ValidationGroup="addcoowner"
                                                        runat="server" Style="color: red" />
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
                                    <button runat="server" id="btnAddCoOwner" style="width: 100px;" validationgroup="addcoowner" class="btn btn-primary" type="button" onserverclick="btnAddCoOwner_ServerClick">Add</button>
                                    <button runat="server" style="width: 100px;" class="btn btn-default" type="button" data-dismiss="modal">Cancel </button>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <div class="modal" id="MsgSPACoBorrower" role="dialog" tabindex="-1" style="overflow-y: auto;">
        <div class="modal-dialog modal-lg" role="document">

            <div class="modal-content">


                <div class="modal-header bg-color">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                    <h4 class="modal-title">Add SPA</h4>
                </div>


                <div class="modal-body">

                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="form-group">
                                    <asp:Button runat="server" ID="btnCopySPA" CssClass="btn btn-info btn-sm titlemargin" Text="Copy Spouse Details" OnClick="btnCopySPA_Click" />
                                </div>
                            </div>
                            <div class="row">


                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-header">
                                            <%--<h3 class="box-title">SPA Details</h3>--%>
                                            <h3 class="modal-title" id="spaTitle" runat="server">Add SPA</h3>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <asp:CustomValidator
                                                    Style="color: red"
                                                    runat="server"
                                                    ID="CustomValidator3"
                                                    ValidationGroup="spacoSave"
                                                    Text="Please tick SPA check box."
                                                    OnServerValidate="CustomValidator3_ServerValidate" />
                                            </div>
                                        </div>


                                        <div class="row hidden">

                                            <div class="col-lg-2 col-md-2 col-sm-6 col-xs-12">
                                                <div class="row">
                                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                        <%--<input runat="server" type="radio" name="SPA" value="SPA" id="tSPA" />--%>
                                                        <asp:RadioButton AutoPostBack="true" runat="server" type="radio" Checked="true" GroupName="SPA" value="SPA" ID="tSPA" OnCheckedChanged="tSPA_CheckedChanged" />
                                                        SPA
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-2 col-md-2 col-sm-6 col-xs-12">
                                                <div class="row">
                                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                        <%--<input runat="server" type="radio" name="SPA" value="CoBorrower" id="tCoBorrower" />--%>
                                                        <asp:RadioButton AutoPostBack="true" OnCheckedChanged="tSPA_CheckedChanged" runat="server" type="radio" GroupName="SPA" value="CoBorrower" ID="tCoBorrower" />
                                                        Co-Borrower
                                                    </div>
                                                </div>
                                            </div>

                                        </div>

                                        <input runat="server" id="txtSPAID" class="form-control text-uppercase hidden" type="text" />




                                        <div class="row">
                                            <div class="box-body">
                                                <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                    <div class="row">
                                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                            Relationship: <span class="color-red fsize-16">*</span>
                                                            <div class="input-group">
                                                                <input runat="server" style="z-index: auto;" id="tRelationship" class="form-control text-uppercase" type="text" disabled /><span class="input-group-btn">
                                                                    <button id="bCoBorrower" runat="server" style="width: 50px; z-index: auto;" data-toggle="modal" data-target="#MsgEmployment" class="btn btn-primary btn-dropbox" type="button" onserverclick="btnEmployment_ServerClick"><i class="fa fa-bars"></i></button>
                                                                </span>
                                                            </div>
                                                            <asp:RequiredFieldValidator
                                                                ID="RequiredFieldValidator28"
                                                                ControlToValidate="tRelationship"
                                                                Text="Please choose Relationship."
                                                                ValidationGroup="spacoSave"
                                                                runat="server" Style="color: red" />
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>



                                        <div class="box-body">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Last Name <span class="color-red fsize-16">*</span></label>
                                                    <input runat="server" type="text" class="form-control text-uppercase" id="tSPALastName" placeholder="SPA/Co-Borrowers Last Name" />
                                                </div>
                                                <asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator29"
                                                    ControlToValidate="tSPALastName"
                                                    Text="Please choose Last Name."
                                                    ValidationGroup="spacoSave"
                                                    runat="server" Style="color: red" />
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>First Name <span class="color-red fsize-16">*</span></label>
                                                    <input runat="server" type="text" class="form-control text-uppercase" id="tSPAFirstName" placeholder="SPA/Co-Borrowers First Name" />
                                                </div>
                                                <asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator30"
                                                    ControlToValidate="tSPAFirstName"
                                                    Text="Please choose First Name."
                                                    ValidationGroup="spacoSave"
                                                    runat="server" Style="color: red" />
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Middle Name <span class="color-red fsize-16">*</span></label>
                                                    <input runat="server" type="text" class="form-control text-uppercase" id="tSPAMiddleName" placeholder="SPA/Co-Borrowers Middle Name" />
                                                </div>
                                                <asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator31"
                                                    ControlToValidate="tSPAMiddleName"
                                                    Text="Please choose Middle Name."
                                                    ValidationGroup="spacoSave"
                                                    runat="server" Style="color: red" />
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Civil Status</label>
                                                    <asp:DropDownList ID="tSPACivilStatus" runat="server" CssClass="form-control text-uppercase">
                                                        <asp:ListItem Value="Single" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Value="Married" Text="Married"> </asp:ListItem>

                                                        <%-- 2023-06-14 : REMOVED WIDOW AND OTHERS; RETAIN SINGLE AND MARRIED --%>
                                                        <%--   <asp:ListItem Value="Widow" Text="Widow"> </asp:ListItem>
                                                        <asp:ListItem Value="Legally Separated" Text="Legally Separated"> </asp:ListItem>--%>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Years of Stay</label>
                                                    <input runat="server" type="text" maxlength="5" class="form-control" onkeypress="return isNumberKey(event)" id="tSPAYearsOfStay" value="0" />
                                                </div>
                                            </div>
                                            <div class="row"></div>
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label>Residential No</label>&nbsp; <span>
                                                        <asp:LinkButton runat="server" ID="btnCopyPrincipalAddressSPA" OnClick="btnCopyPrincipalAddressSPA_Click" class="btn btn-info btn-sm"> Copy principal buyer's present address</asp:LinkButton></span>
                                                    <input runat="server" type="text" class="form-control text-uppercase" id="tSPAPresentAddress" placeholder="Present Address" />
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <label>Date of Birth</label>
                                                <div class="input-group">
                                                    <div class="input-group-addon">
                                                        <i class="fa fa-calendar"></i>
                                                    </div>
                                                    <input runat="server" type="date" class="form-control" id="tSPABirthDate" />
                                                </div>
                                                <asp:CustomValidator
                                                    Style="color: red"
                                                    runat="server"
                                                    ID="CustomValidator5"
                                                    CssClass="col-md-12"
                                                    ValidationGroup="spacoSave"
                                                    ControlToValidate="tSPABirthDate"
                                                    Text="Below legal age."
                                                    OnServerValidate="CustomValidator5_ServerValidate" />
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Place of Birth</label>
                                                    <input runat="server" type="text" class="form-control text-uppercase" id="tSPABirthPlace" placeholder="Birth Place" />
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Gender</label>
                                                    <select runat="server" id="tSPAGender" class="form-control">
                                                        <option value="">-- Select One --</option>
                                                        <option value="M">Male</option>
                                                        <option value="F">Female</option>
                                                    </select>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Citizenship</label>
                                                    <select runat="server" id="tSPACitizenship" name="nationality" class="form-control">
                                                        <option value="">-- Select One --</option>
                                                        <option value="afghan">Afghan</option>
                                                        <option value="albanian">Albanian</option>
                                                        <option value="algerian">Algerian</option>
                                                        <option value="american">American</option>
                                                        <option value="andorran">Andorran</option>
                                                        <option value="angolan">Angolan</option>
                                                        <option value="antiguans">Antiguans</option>
                                                        <option value="argentinean">Argentinean</option>
                                                        <option value="armenian">Armenian</option>
                                                        <option value="australian">Australian</option>
                                                        <option value="austrian">Austrian</option>
                                                        <option value="azerbaijani">Azerbaijani</option>
                                                        <option value="bahamian">Bahamian</option>
                                                        <option value="bahraini">Bahraini</option>
                                                        <option value="bangladeshi">Bangladeshi</option>
                                                        <option value="barbadian">Barbadian</option>
                                                        <option value="barbudans">Barbudans</option>
                                                        <option value="batswana">Batswana</option>
                                                        <option value="belarusian">Belarusian</option>
                                                        <option value="belgian">Belgian</option>
                                                        <option value="belizean">Belizean</option>
                                                        <option value="beninese">Beninese</option>
                                                        <option value="bhutanese">Bhutanese</option>
                                                        <option value="bolivian">Bolivian</option>
                                                        <option value="bosnian">Bosnian</option>
                                                        <option value="brazilian">Brazilian</option>
                                                        <option value="british">British</option>
                                                        <option value="bruneian">Bruneian</option>
                                                        <option value="bulgarian">Bulgarian</option>
                                                        <option value="burkinabe">Burkinabe</option>
                                                        <option value="burmese">Burmese</option>
                                                        <option value="burundian">Burundian</option>
                                                        <option value="cambodian">Cambodian</option>
                                                        <option value="cameroonian">Cameroonian</option>
                                                        <option value="canadian">Canadian</option>
                                                        <option value="cape verdean">Cape Verdean</option>
                                                        <option value="central african">Central African</option>
                                                        <option value="chadian">Chadian</option>
                                                        <option value="chilean">Chilean</option>
                                                        <option value="chinese">Chinese</option>
                                                        <option value="colombian">Colombian</option>
                                                        <option value="comoran">Comoran</option>
                                                        <option value="congolese">Congolese</option>
                                                        <option value="costa rican">Costa Rican</option>
                                                        <option value="croatian">Croatian</option>
                                                        <option value="cuban">Cuban</option>
                                                        <option value="cypriot">Cypriot</option>
                                                        <option value="czech">Czech</option>
                                                        <option value="danish">Danish</option>
                                                        <option value="djibouti">Djibouti</option>
                                                        <option value="dominican">Dominican</option>
                                                        <option value="dutch">Dutch</option>
                                                        <option value="east timorese">East Timorese</option>
                                                        <option value="ecuadorean">Ecuadorean</option>
                                                        <option value="egyptian">Egyptian</option>
                                                        <option value="emirian">Emirian</option>
                                                        <option value="equatorial guinean">Equatorial Guinean</option>
                                                        <option value="eritrean">Eritrean</option>
                                                        <option value="estonian">Estonian</option>
                                                        <option value="ethiopian">Ethiopian</option>
                                                        <option value="fijian">Fijian</option>
                                                        <option value="filipino">Filipino</option>
                                                        <option value="finnish">Finnish</option>
                                                        <option value="french">French</option>
                                                        <option value="gabonese">Gabonese</option>
                                                        <option value="gambian">Gambian</option>
                                                        <option value="georgian">Georgian</option>
                                                        <option value="german">German</option>
                                                        <option value="ghanaian">Ghanaian</option>
                                                        <option value="greek">Greek</option>
                                                        <option value="grenadian">Grenadian</option>
                                                        <option value="guatemalan">Guatemalan</option>
                                                        <option value="guinea-bissauan">Guinea-Bissauan</option>
                                                        <option value="guinean">Guinean</option>
                                                        <option value="guyanese">Guyanese</option>
                                                        <option value="haitian">Haitian</option>
                                                        <option value="herzegovinian">Herzegovinian</option>
                                                        <option value="honduran">Honduran</option>
                                                        <option value="hungarian">Hungarian</option>
                                                        <option value="icelander">Icelander</option>
                                                        <option value="indian">Indian</option>
                                                        <option value="indonesian">Indonesian</option>
                                                        <option value="iranian">Iranian</option>
                                                        <option value="iraqi">Iraqi</option>
                                                        <option value="irish">Irish</option>
                                                        <option value="israeli">Israeli</option>
                                                        <option value="italian">Italian</option>
                                                        <option value="ivorian">Ivorian</option>
                                                        <option value="jamaican">Jamaican</option>
                                                        <option value="japanese">Japanese</option>
                                                        <option value="jordanian">Jordanian</option>
                                                        <option value="kazakhstani">Kazakhstani</option>
                                                        <option value="kenyan">Kenyan</option>
                                                        <option value="kittian and nevisian">Kittian and Nevisian</option>
                                                        <option value="kuwaiti">Kuwaiti</option>
                                                        <option value="kyrgyz">Kyrgyz</option>
                                                        <option value="laotian">Laotian</option>
                                                        <option value="latvian">Latvian</option>
                                                        <option value="lebanese">Lebanese</option>
                                                        <option value="liberian">Liberian</option>
                                                        <option value="libyan">Libyan</option>
                                                        <option value="liechtensteiner">Liechtensteiner</option>
                                                        <option value="lithuanian">Lithuanian</option>
                                                        <option value="luxembourger">Luxembourger</option>
                                                        <option value="macedonian">Macedonian</option>
                                                        <option value="malagasy">Malagasy</option>
                                                        <option value="malawian">Malawian</option>
                                                        <option value="malaysian">Malaysian</option>
                                                        <option value="maldivan">Maldivan</option>
                                                        <option value="malian">Malian</option>
                                                        <option value="maltese">Maltese</option>
                                                        <option value="marshallese">Marshallese</option>
                                                        <option value="mauritanian">Mauritanian</option>
                                                        <option value="mauritian">Mauritian</option>
                                                        <option value="mexican">Mexican</option>
                                                        <option value="micronesian">Micronesian</option>
                                                        <option value="moldovan">Moldovan</option>
                                                        <option value="monacan">Monacan</option>
                                                        <option value="mongolian">Mongolian</option>
                                                        <option value="moroccan">Moroccan</option>
                                                        <option value="mosotho">Mosotho</option>
                                                        <option value="motswana">Motswana</option>
                                                        <option value="mozambican">Mozambican</option>
                                                        <option value="namibian">Namibian</option>
                                                        <option value="nauruan">Nauruan</option>
                                                        <option value="nepalese">Nepalese</option>
                                                        <option value="new zealander">New Zealander</option>
                                                        <option value="ni-vanuatu">Ni-Vanuatu</option>
                                                        <option value="nicaraguan">Nicaraguan</option>
                                                        <option value="nigerien">Nigerien</option>
                                                        <option value="north korean">North Korean</option>
                                                        <option value="northern irish">Northern Irish</option>
                                                        <option value="norwegian">Norwegian</option>
                                                        <option value="omani">Omani</option>
                                                        <option value="pakistani">Pakistani</option>
                                                        <option value="palauan">Palauan</option>
                                                        <option value="panamanian">Panamanian</option>
                                                        <option value="papua new guinean">Papua New Guinean</option>
                                                        <option value="paraguayan">Paraguayan</option>
                                                        <option value="peruvian">Peruvian</option>
                                                        <option value="polish">Polish</option>
                                                        <option value="portuguese">Portuguese</option>
                                                        <option value="qatari">Qatari</option>
                                                        <option value="romanian">Romanian</option>
                                                        <option value="russian">Russian</option>
                                                        <option value="rwandan">Rwandan</option>
                                                        <option value="saint lucian">Saint Lucian</option>
                                                        <option value="salvadoran">Salvadoran</option>
                                                        <option value="samoan">Samoan</option>
                                                        <option value="san marinese">San Marinese</option>
                                                        <option value="sao tomean">Sao Tomean</option>
                                                        <option value="saudi">Saudi</option>
                                                        <option value="scottish">Scottish</option>
                                                        <option value="senegalese">Senegalese</option>
                                                        <option value="serbian">Serbian</option>
                                                        <option value="seychellois">Seychellois</option>
                                                        <option value="sierra leonean">Sierra Leonean</option>
                                                        <option value="singaporean">Singaporean</option>
                                                        <option value="slovakian">Slovakian</option>
                                                        <option value="slovenian">Slovenian</option>
                                                        <option value="solomon islander">Solomon Islander</option>
                                                        <option value="somali">Somali</option>
                                                        <option value="south african">South African</option>
                                                        <option value="south korean">South Korean</option>
                                                        <option value="spanish">Spanish</option>
                                                        <option value="sri lankan">Sri Lankan</option>
                                                        <option value="sudanese">Sudanese</option>
                                                        <option value="surinamer">Surinamer</option>
                                                        <option value="swazi">Swazi</option>
                                                        <option value="swedish">Swedish</option>
                                                        <option value="swiss">Swiss</option>
                                                        <option value="syrian">Syrian</option>
                                                        <option value="taiwanese">Taiwanese</option>
                                                        <option value="tajik">Tajik</option>
                                                        <option value="tanzanian">Tanzanian</option>
                                                        <option value="thai">Thai</option>
                                                        <option value="togolese">Togolese</option>
                                                        <option value="tongan">Tongan</option>
                                                        <option value="trinidadian or tobagonian">Trinidadian or Tobagonian</option>
                                                        <option value="tunisian">Tunisian</option>
                                                        <option value="turkish">Turkish</option>
                                                        <option value="tuvaluan">Tuvaluan</option>
                                                        <option value="ugandan">Ugandan</option>
                                                        <option value="ukrainian">Ukrainian</option>
                                                        <option value="uruguayan">Uruguayan</option>
                                                        <option value="uzbekistani">Uzbekistani</option>
                                                        <option value="venezuelan">Venezuelan</option>
                                                        <option value="vietnamese">Vietnamese</option>
                                                        <option value="welsh">Welsh</option>
                                                        <option value="yemenite">Yemenite</option>
                                                        <option value="zambian">Zambian</option>
                                                        <option value="zimbabwean">Zimbabwean</option>
                                                    </select>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Email Address</label>
                                                    <input runat="server" type="email" class="form-control text-lowercase" id="tSPAEmailAdd" placeholder="xxxx@email.com" />
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Home Tel No.</label>
                                                    <input runat="server" type="text" class="form-control" id="tSPAHomeTelNo" maxlength="50" onkeypress="return fnAllowNumeric(event)" placeholder="xx xxx xxxx" />
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Cellphone No.</label>
                                                    <input runat="server" type="text" class="form-control" id="tSPACellphoneNo" maxlength="50" onkeypress="return fnAllowNumeric(event)" placeholder="xxxx xxx xxxx" />
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Facebook Account</label>
                                                    <input runat="server" type="email" class="form-control text-lowercase" id="tSPAFBAccount" placeholder="xxxx@email.com" />
                                                </div>
                                            </div>


                                            <div class="col-md-6">
                                                <div class="col-md-12">
                                                    <br>
                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <div class="col-lg-12" runat="server" id="divUploadSPADocs">
                                                                <label>SPA Form document</label>
                                                                <div class="col-lg-12 center-block center-block">
                                                                    <asp:Label runat="server" ID="lblSPAFileName" Text=""></asp:Label>
                                                                    <asp:FileUpload ID="SPAFileUpload" CssClass="btn btn-default" CommandName="FileUpload1" runat="server" EnableViewState="true" />
                                                                </div>

                                                                <div class="col-lg-12 center-block center-block">
                                                                    <asp:LinkButton ID="btnSPAUpload" OnClick="btnSPAUpload_Click" CssClass="btn btn-primary" runat="server" Text="Upload" />
                                                                    <asp:LinkButton ID="btnSPAPreview" OnClick="btnSPAUpload_Click" CssClass="btn btn-info" Visible="false" runat="server" Text="Preview" />
                                                                    <asp:LinkButton ID="btnSPARemove" OnClick="btnSPAUpload_Click" CssClass="btn btn-danger" Visible="false" runat="server" Text="Remove" />
                                                                </div>
                                                            </div>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="btnSPAUpload" />
                                                            <asp:PostBackTrigger ControlID="btnSPAPreview" />
                                                            <asp:PostBackTrigger ControlID="btnSPARemove" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>

                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>



                            <div class="col-md-6 hidden">
                                <div class="box box-primary">
                                    <div class="box-header">
                                        <h3 class="box-title">Co Borrower Business Details</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Employment Status</label>
                                                <asp:DropDownList ID="tSPAEmploymentStatus" runat="server" CssClass="form-control">
                                                    <asp:ListItem Selected="True"></asp:ListItem>
                                                    <%--<asp:ListItem Value="---Select Employment Status---" Selected="True"></asp:ListItem>--%>
                                                    <asp:ListItem Value="ES1" Text="Casual"> </asp:ListItem>
                                                    <asp:ListItem Value="ES2" Text="Contractual"> </asp:ListItem>
                                                    <asp:ListItem Value="ES3" Text="Probationary"> </asp:ListItem>
                                                    <asp:ListItem Value="ES4" Text="Permanent"> </asp:ListItem>
                                                    <asp:ListItem Value="ES5" Text="Consultant"> </asp:ListItem>
                                                    <asp:ListItem Value="OTH" Text="Others"> </asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Nature of Employment</label>
                                                <asp:DropDownList ID="tSPANatureEmployment" runat="server" CssClass="form-control">
                                                    <asp:ListItem Selected="True"></asp:ListItem>
                                                    <%--<asp:ListItem Value="---Select Nature of Employment---" Selected="True"></asp:ListItem>--%>
                                                    <asp:ListItem Value="NE1" Text="Private Sector"> </asp:ListItem>
                                                    <%--<asp:ListItem Value="NE2" Text="Self Employed"> </asp:ListItem>--%>
                                                    <asp:ListItem Value="NE3" Text="Government"> </asp:ListItem>
                                                    <asp:ListItem Value="NE4" Text="Retired"> </asp:ListItem>
                                                    <asp:ListItem Value="NE5" Text="Overseas"> </asp:ListItem>
                                                    <asp:ListItem Value="OTH" Text="Others"> </asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Business Name</label>
                                                <input runat="server" type="text" class="form-control text-uppercase" id="tSPAEmpBusinessName" placeholder="Spouse Employer's Business Name" />
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Business Address</label>
                                                <input runat="server" type="text" class="form-control text-uppercase" id="tSPAEmpBusinessAddress" placeholder="Spouse Employer's Business Address" />
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Business Country</label>
                                                <asp:DropDownList ID="ddSPABusCountry" runat="server" CssClass="form-control text-uppercase">
                                                    <asp:ListItem Selected="true" Value="PH">Philippines</asp:ListItem>
                                                    <asp:ListItem Value="AF">Afghanistan</asp:ListItem>
                                                    <asp:ListItem Value="AX">Aland Islands</asp:ListItem>
                                                    <asp:ListItem Value="AL">Albania</asp:ListItem>
                                                    <asp:ListItem Value="DZ">Algeria</asp:ListItem>
                                                    <asp:ListItem Value="AS">American Samoa</asp:ListItem>
                                                    <asp:ListItem Value="AD">Andorra</asp:ListItem>
                                                    <asp:ListItem Value="AO">Angola</asp:ListItem>
                                                    <asp:ListItem Value="AI">Anguilla</asp:ListItem>
                                                    <asp:ListItem Value="AQ">Antarctica</asp:ListItem>
                                                    <asp:ListItem Value="AG">Antigua and Barbuda</asp:ListItem>
                                                    <asp:ListItem Value="AR">Argentina</asp:ListItem>
                                                    <asp:ListItem Value="AM">Armenia</asp:ListItem>
                                                    <asp:ListItem Value="AW">Aruba</asp:ListItem>
                                                    <asp:ListItem Value="AU">Australia</asp:ListItem>
                                                    <asp:ListItem Value="AT">Austria</asp:ListItem>
                                                    <asp:ListItem Value="AZ">Azerbaijan</asp:ListItem>
                                                    <asp:ListItem Value="BS">Bahamas</asp:ListItem>
                                                    <asp:ListItem Value="BH">Bahrain</asp:ListItem>
                                                    <asp:ListItem Value="BD">Bangladesh</asp:ListItem>
                                                    <asp:ListItem Value="BB">Barbados</asp:ListItem>
                                                    <asp:ListItem Value="BY">Belarus</asp:ListItem>
                                                    <asp:ListItem Value="BE">Belgium</asp:ListItem>
                                                    <asp:ListItem Value="BZ">Belize</asp:ListItem>
                                                    <asp:ListItem Value="BJ">Benin</asp:ListItem>
                                                    <asp:ListItem Value="BM">Bermuda</asp:ListItem>
                                                    <asp:ListItem Value="BT">Bhutan</asp:ListItem>
                                                    <asp:ListItem Value="BO">Bolivia</asp:ListItem>
                                                    <asp:ListItem Value="BA">Bosnia and Herzegovina</asp:ListItem>
                                                    <asp:ListItem Value="BW">Botswana</asp:ListItem>
                                                    <asp:ListItem Value="BV">Bouvet Island</asp:ListItem>
                                                    <asp:ListItem Value="BR">Brazil</asp:ListItem>
                                                    <asp:ListItem Value="VG">British Virgin Islands</asp:ListItem>
                                                    <asp:ListItem Value="IO">British Indian Ocean Territory</asp:ListItem>
                                                    <asp:ListItem Value="BN">Brunei Darussalam</asp:ListItem>
                                                    <asp:ListItem Value="BG">Bulgaria</asp:ListItem>
                                                    <asp:ListItem Value="BF">Burkina Faso</asp:ListItem>
                                                    <asp:ListItem Value="BI">Burundi</asp:ListItem>
                                                    <asp:ListItem Value="KH">Cambodia</asp:ListItem>
                                                    <asp:ListItem Value="CM">Cameroon</asp:ListItem>
                                                    <asp:ListItem Value="CA">Canada</asp:ListItem>
                                                    <asp:ListItem Value="CV">Cape Verde</asp:ListItem>
                                                    <asp:ListItem Value="KY">Cayman Islands</asp:ListItem>
                                                    <asp:ListItem Value="CF">Central African Republic</asp:ListItem>
                                                    <asp:ListItem Value="TD">Chad</asp:ListItem>
                                                    <asp:ListItem Value="CL">Chile</asp:ListItem>
                                                    <asp:ListItem Value="CN">China</asp:ListItem>
                                                    <asp:ListItem Value="HK">Hong Kong, SAR China</asp:ListItem>
                                                    <asp:ListItem Value="MO">Macao, SAR China</asp:ListItem>
                                                    <asp:ListItem Value="CX">Christmas Island</asp:ListItem>
                                                    <asp:ListItem Value="CC">Cocos (Keeling) Islands</asp:ListItem>
                                                    <asp:ListItem Value="CO">Colombia</asp:ListItem>
                                                    <asp:ListItem Value="KM">Comoros</asp:ListItem>
                                                    <asp:ListItem Value="CG">Congo (Brazzaville)</asp:ListItem>
                                                    <asp:ListItem Value="CD">Congo, (Kinshasa)</asp:ListItem>
                                                    <asp:ListItem Value="CK">Cook Islands</asp:ListItem>
                                                    <asp:ListItem Value="CR">Costa Rica</asp:ListItem>
                                                    <asp:ListItem Value="CI">Côte d'Ivoire</asp:ListItem>
                                                    <asp:ListItem Value="HR">Croatia</asp:ListItem>
                                                    <asp:ListItem Value="CU">Cuba</asp:ListItem>
                                                    <asp:ListItem Value="CY">Cyprus</asp:ListItem>
                                                    <asp:ListItem Value="CZ">Czech Republic</asp:ListItem>
                                                    <asp:ListItem Value="DK">Denmark</asp:ListItem>
                                                    <asp:ListItem Value="DJ">Djibouti</asp:ListItem>
                                                    <asp:ListItem Value="DM">Dominica</asp:ListItem>
                                                    <asp:ListItem Value="DO">Dominican Republic</asp:ListItem>
                                                    <asp:ListItem Value="EC">Ecuador</asp:ListItem>
                                                    <asp:ListItem Value="EG">Egypt</asp:ListItem>
                                                    <asp:ListItem Value="SV">El Salvador</asp:ListItem>
                                                    <asp:ListItem Value="GQ">Equatorial Guinea</asp:ListItem>
                                                    <asp:ListItem Value="ER">Eritrea</asp:ListItem>
                                                    <asp:ListItem Value="EE">Estonia</asp:ListItem>
                                                    <asp:ListItem Value="ET">Ethiopia</asp:ListItem>
                                                    <asp:ListItem Value="FK">Falkland Islands (Malvinas)</asp:ListItem>
                                                    <asp:ListItem Value="FO">Faroe Islands</asp:ListItem>
                                                    <asp:ListItem Value="FJ">Fiji</asp:ListItem>
                                                    <asp:ListItem Value="FI">Finland</asp:ListItem>
                                                    <asp:ListItem Value="FR">France</asp:ListItem>
                                                    <asp:ListItem Value="GF">French Guiana</asp:ListItem>
                                                    <asp:ListItem Value="PF">French Polynesia</asp:ListItem>
                                                    <asp:ListItem Value="TF">French Southern Territories</asp:ListItem>
                                                    <asp:ListItem Value="GA">Gabon</asp:ListItem>
                                                    <asp:ListItem Value="GM">Gambia</asp:ListItem>
                                                    <asp:ListItem Value="GE">Georgia</asp:ListItem>
                                                    <asp:ListItem Value="DE">Germany</asp:ListItem>
                                                    <asp:ListItem Value="GH">Ghana</asp:ListItem>
                                                    <asp:ListItem Value="GI">Gibraltar</asp:ListItem>
                                                    <asp:ListItem Value="GR">Greece</asp:ListItem>
                                                    <asp:ListItem Value="GL">Greenland</asp:ListItem>
                                                    <asp:ListItem Value="GD">Grenada</asp:ListItem>
                                                    <asp:ListItem Value="GP">Guadeloupe</asp:ListItem>
                                                    <asp:ListItem Value="GU">Guam</asp:ListItem>
                                                    <asp:ListItem Value="GT">Guatemala</asp:ListItem>
                                                    <asp:ListItem Value="GG">Guernsey</asp:ListItem>
                                                    <asp:ListItem Value="GN">Guinea</asp:ListItem>
                                                    <asp:ListItem Value="GW">Guinea-Bissau</asp:ListItem>
                                                    <asp:ListItem Value="GY">Guyana</asp:ListItem>
                                                    <asp:ListItem Value="HT">Haiti</asp:ListItem>
                                                    <asp:ListItem Value="HM">Heard and Mcdonald Islands</asp:ListItem>
                                                    <asp:ListItem Value="VA">Holy See (Vatican City State)</asp:ListItem>
                                                    <asp:ListItem Value="HN">Honduras</asp:ListItem>
                                                    <asp:ListItem Value="HU">Hungary</asp:ListItem>
                                                    <asp:ListItem Value="IS">Iceland</asp:ListItem>
                                                    <asp:ListItem Value="IN">India</asp:ListItem>
                                                    <asp:ListItem Value="ID">Indonesia</asp:ListItem>
                                                    <asp:ListItem Value="IR">Iran, Islamic Republic of</asp:ListItem>
                                                    <asp:ListItem Value="IQ">Iraq</asp:ListItem>
                                                    <asp:ListItem Value="IE">Ireland</asp:ListItem>
                                                    <asp:ListItem Value="IM">Isle of Man</asp:ListItem>
                                                    <asp:ListItem Value="IL">Israel</asp:ListItem>
                                                    <asp:ListItem Value="IT">Italy</asp:ListItem>
                                                    <asp:ListItem Value="JM">Jamaica</asp:ListItem>
                                                    <asp:ListItem Value="JP">Japan</asp:ListItem>
                                                    <asp:ListItem Value="JE">Jersey</asp:ListItem>
                                                    <asp:ListItem Value="JO">Jordan</asp:ListItem>
                                                    <asp:ListItem Value="KZ">Kazakhstan</asp:ListItem>
                                                    <asp:ListItem Value="KE">Kenya</asp:ListItem>
                                                    <asp:ListItem Value="KI">Kiribati</asp:ListItem>
                                                    <asp:ListItem Value="KP">Korea (North)</asp:ListItem>
                                                    <asp:ListItem Value="KR">Korea (South)</asp:ListItem>
                                                    <asp:ListItem Value="KW">Kuwait</asp:ListItem>
                                                    <asp:ListItem Value="KG">Kyrgyzstan</asp:ListItem>
                                                    <asp:ListItem Value="LA">Lao PDR</asp:ListItem>
                                                    <asp:ListItem Value="LV">Latvia</asp:ListItem>
                                                    <asp:ListItem Value="LB">Lebanon</asp:ListItem>
                                                    <asp:ListItem Value="LS">Lesotho</asp:ListItem>
                                                    <asp:ListItem Value="LR">Liberia</asp:ListItem>
                                                    <asp:ListItem Value="LY">Libya</asp:ListItem>
                                                    <asp:ListItem Value="LI">Liechtenstein</asp:ListItem>
                                                    <asp:ListItem Value="LT">Lithuania</asp:ListItem>
                                                    <asp:ListItem Value="LU">Luxembourg</asp:ListItem>
                                                    <asp:ListItem Value="MK">Macedonia, Republic of</asp:ListItem>
                                                    <asp:ListItem Value="MG">Madagascar</asp:ListItem>
                                                    <asp:ListItem Value="MW">Malawi</asp:ListItem>
                                                    <asp:ListItem Value="MY">Malaysia</asp:ListItem>
                                                    <asp:ListItem Value="MV">Maldives</asp:ListItem>
                                                    <asp:ListItem Value="ML">Mali</asp:ListItem>
                                                    <asp:ListItem Value="MT">Malta</asp:ListItem>
                                                    <asp:ListItem Value="MH">Marshall Islands</asp:ListItem>
                                                    <asp:ListItem Value="MQ">Martinique</asp:ListItem>
                                                    <asp:ListItem Value="MR">Mauritania</asp:ListItem>
                                                    <asp:ListItem Value="MU">Mauritius</asp:ListItem>
                                                    <asp:ListItem Value="YT">Mayotte</asp:ListItem>
                                                    <asp:ListItem Value="MX">Mexico</asp:ListItem>
                                                    <asp:ListItem Value="FM">Micronesia, Federated States of</asp:ListItem>
                                                    <asp:ListItem Value="MD">Moldova</asp:ListItem>
                                                    <asp:ListItem Value="MC">Monaco</asp:ListItem>
                                                    <asp:ListItem Value="MN">Mongolia</asp:ListItem>
                                                    <asp:ListItem Value="ME">Montenegro</asp:ListItem>
                                                    <asp:ListItem Value="MS">Montserrat</asp:ListItem>
                                                    <asp:ListItem Value="MA">Morocco</asp:ListItem>
                                                    <asp:ListItem Value="MZ">Mozambique</asp:ListItem>
                                                    <asp:ListItem Value="MM">Myanmar</asp:ListItem>
                                                    <asp:ListItem Value="NA">Namibia</asp:ListItem>
                                                    <asp:ListItem Value="NR">Nauru</asp:ListItem>
                                                    <asp:ListItem Value="NP">Nepal</asp:ListItem>
                                                    <asp:ListItem Value="NL">Netherlands</asp:ListItem>
                                                    <asp:ListItem Value="AN">Netherlands Antilles</asp:ListItem>
                                                    <asp:ListItem Value="NC">New Caledonia</asp:ListItem>
                                                    <asp:ListItem Value="NZ">New Zealand</asp:ListItem>
                                                    <asp:ListItem Value="NI">Nicaragua</asp:ListItem>
                                                    <asp:ListItem Value="NE">Niger</asp:ListItem>
                                                    <asp:ListItem Value="NG">Nigeria</asp:ListItem>
                                                    <asp:ListItem Value="NU">Niue</asp:ListItem>
                                                    <asp:ListItem Value="NF">Norfolk Island</asp:ListItem>
                                                    <asp:ListItem Value="MP">Northern Mariana Islands</asp:ListItem>
                                                    <asp:ListItem Value="NO">Norway</asp:ListItem>
                                                    <asp:ListItem Value="OM">Oman</asp:ListItem>
                                                    <asp:ListItem Value="PK">Pakistan</asp:ListItem>
                                                    <asp:ListItem Value="PW">Palau</asp:ListItem>
                                                    <asp:ListItem Value="PS">Palestinian Territory</asp:ListItem>
                                                    <asp:ListItem Value="PA">Panama</asp:ListItem>
                                                    <asp:ListItem Value="PG">Papua New Guinea</asp:ListItem>
                                                    <asp:ListItem Value="PY">Paraguay</asp:ListItem>
                                                    <asp:ListItem Value="PE">Peru</asp:ListItem>
                                                    <asp:ListItem Value="PN">Pitcairn</asp:ListItem>
                                                    <asp:ListItem Value="PL">Poland</asp:ListItem>
                                                    <asp:ListItem Value="PT">Portugal</asp:ListItem>
                                                    <asp:ListItem Value="PR">Puerto Rico</asp:ListItem>
                                                    <asp:ListItem Value="QA">Qatar</asp:ListItem>
                                                    <asp:ListItem Value="RE">Réunion</asp:ListItem>
                                                    <asp:ListItem Value="RO">Romania</asp:ListItem>
                                                    <asp:ListItem Value="RU">Russian Federation</asp:ListItem>
                                                    <asp:ListItem Value="RW">Rwanda</asp:ListItem>
                                                    <asp:ListItem Value="BL">Saint-Barthélemy</asp:ListItem>
                                                    <asp:ListItem Value="SH">Saint Helena</asp:ListItem>
                                                    <asp:ListItem Value="KN">Saint Kitts and Nevis</asp:ListItem>
                                                    <asp:ListItem Value="LC">Saint Lucia</asp:ListItem>
                                                    <asp:ListItem Value="MF">Saint-Martin (French part)</asp:ListItem>
                                                    <asp:ListItem Value="PM">Saint Pierre and Miquelon</asp:ListItem>
                                                    <asp:ListItem Value="VC">Saint Vincent and Grenadines</asp:ListItem>
                                                    <asp:ListItem Value="WS">Samoa</asp:ListItem>
                                                    <asp:ListItem Value="SM">San Marino</asp:ListItem>
                                                    <asp:ListItem Value="ST">Sao Tome and Principe</asp:ListItem>
                                                    <asp:ListItem Value="SA">Saudi Arabia</asp:ListItem>
                                                    <asp:ListItem Value="SN">Senegal</asp:ListItem>
                                                    <asp:ListItem Value="RS">Serbia</asp:ListItem>
                                                    <asp:ListItem Value="SC">Seychelles</asp:ListItem>
                                                    <asp:ListItem Value="SL">Sierra Leone</asp:ListItem>
                                                    <asp:ListItem Value="SG">Singapore</asp:ListItem>
                                                    <asp:ListItem Value="SK">Slovakia</asp:ListItem>
                                                    <asp:ListItem Value="SI">Slovenia</asp:ListItem>
                                                    <asp:ListItem Value="SB">Solomon Islands</asp:ListItem>
                                                    <asp:ListItem Value="SO">Somalia</asp:ListItem>
                                                    <asp:ListItem Value="ZA">South Africa</asp:ListItem>
                                                    <asp:ListItem Value="GS">South Georgia and the South Sandwich Islands</asp:ListItem>
                                                    <asp:ListItem Value="SS">South Sudan</asp:ListItem>
                                                    <asp:ListItem Value="ES">Spain</asp:ListItem>
                                                    <asp:ListItem Value="LK">Sri Lanka</asp:ListItem>
                                                    <asp:ListItem Value="SD">Sudan</asp:ListItem>
                                                    <asp:ListItem Value="SR">Suriname</asp:ListItem>
                                                    <asp:ListItem Value="SJ">Svalbard and Jan Mayen Islands</asp:ListItem>
                                                    <asp:ListItem Value="SZ">Swaziland</asp:ListItem>
                                                    <asp:ListItem Value="SE">Sweden</asp:ListItem>
                                                    <asp:ListItem Value="CH">Switzerland</asp:ListItem>
                                                    <asp:ListItem Value="SY">Syrian Arab Republic (Syria)</asp:ListItem>
                                                    <asp:ListItem Value="TW">Taiwan, Republic of China</asp:ListItem>
                                                    <asp:ListItem Value="TJ">Tajikistan</asp:ListItem>
                                                    <asp:ListItem Value="TZ">Tanzania, United Republic of</asp:ListItem>
                                                    <asp:ListItem Value="TH">Thailand</asp:ListItem>
                                                    <asp:ListItem Value="TL">Timor-Leste</asp:ListItem>
                                                    <asp:ListItem Value="TG">Togo</asp:ListItem>
                                                    <asp:ListItem Value="TK">Tokelau</asp:ListItem>
                                                    <asp:ListItem Value="TO">Tonga</asp:ListItem>
                                                    <asp:ListItem Value="TT">Trinidad and Tobago</asp:ListItem>
                                                    <asp:ListItem Value="TN">Tunisia</asp:ListItem>
                                                    <asp:ListItem Value="TR">Turkey</asp:ListItem>
                                                    <asp:ListItem Value="TM">Turkmenistan</asp:ListItem>
                                                    <asp:ListItem Value="TC">Turks and Caicos Islands</asp:ListItem>
                                                    <asp:ListItem Value="TV">Tuvalu</asp:ListItem>
                                                    <asp:ListItem Value="UG">Uganda</asp:ListItem>
                                                    <asp:ListItem Value="UA">Ukraine</asp:ListItem>
                                                    <asp:ListItem Value="AE">United Arab Emirates</asp:ListItem>
                                                    <asp:ListItem Value="GB">United Kingdom</asp:ListItem>
                                                    <asp:ListItem Value="US">United States of America</asp:ListItem>
                                                    <asp:ListItem Value="UM">US Minor Outlying Islands</asp:ListItem>
                                                    <asp:ListItem Value="UY">Uruguay</asp:ListItem>
                                                    <asp:ListItem Value="UZ">Uzbekistan</asp:ListItem>
                                                    <asp:ListItem Value="VU">Vanuatu</asp:ListItem>
                                                    <asp:ListItem Value="VE">Venezuela (Bolivarian Republic)</asp:ListItem>
                                                    <asp:ListItem Value="VN">Vietnam</asp:ListItem>
                                                    <asp:ListItem Value="VI">Virgin Islands, US</asp:ListItem>
                                                    <asp:ListItem Value="WF">Wallis and Futuna Islands</asp:ListItem>
                                                    <asp:ListItem Value="EH">Western Sahara</asp:ListItem>
                                                    <asp:ListItem Value="YE">Yemen</asp:ListItem>
                                                    <asp:ListItem Value="ZM">Zambia</asp:ListItem>
                                                    <asp:ListItem Value="ZW">Zimbabwe</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Position</label>
                                                <input runat="server" type="text" class="form-control text-uppercase" id="tSPAPosition" placeholder="Position" />
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Years in Service</label>
                                                <input runat="server" type="text" onkeypress="return isNumberKey(event)" maxlength="5" class="form-control" id="tSPAYearsofService" value="0" />
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Office Tel No.</label>
                                                <input runat="server" type="text" class="form-control" id="tSPAOfficeTelNo" maxlength="13" onkeypress="return fnAllowNumeric(event)" placeholder="xx xxx xxxx" />
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Fax No.</label>
                                                <input runat="server" type="text" class="form-control" id="tSPAFAXNo" maxlength="14" onkeypress="return fnAllowNumeric(event)" placeholder="x xxx xxx xxxx" />
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>TIN No. <span class="color-red fsize-16">*</span></label>
                                                <input runat="server" type="text" class="form-control" id="tSPATIN" onkeypress="return fnAllowNumeric(event)" placeholder="xxx xxx xxx xxx" maxlength="15" />
                                                <%--<input runat="server" type="text" class="form-control" onkeyup="TIN_keyup(event);" id="tSPATIN" onkeypress="return fnAllowNumeric(event)" placeholder="xxx xxx xxx xxx" maxlength="15" />--%>
                                            </div>
                                            <%--<asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator32"
                                                ControlToValidate="tSPATIN"
                                                Text="Please choose TIN."
                                                ValidationGroup="spacoSave"
                                                runat="server" Style="color: red" />--%>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>SSS No.</label>
                                                <input runat="server" type="text" class="form-control" id="tSPASSSNo" maxlength="12" onkeypress="return fnAllowNumeric(event)" placeholder="xx xxxxxxx x" />
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>GSIS No.</label>
                                                <input runat="server" type="text" class="form-control" id="tSPAGSISNo" maxlength="14" onkeypress="return fnAllowNumeric(event)" placeholder="xxxx xxxxxxx x" />
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>PAG-IBIG No.</label>
                                                <input runat="server" type="text" class="form-control" id="tSPAPagibigNo" maxlength="14" onkeypress="return fnAllowNumeric(event)" placeholder="xxxx xxxxxxx x" />
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>


                    <div class="modal-footer">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <button class="btn btn-default btn-primary pull-right" type="button" validationgroup="spacoSave" runat="server" id="btnSPACoBorrower" onserverclick="btnSPACoBorrower_ServerClick">
                                    <label runat="server" id="SPAAddUpdate" />
                                    SPA <i class="fa fa-plus-square"></i>
                                </button>

                                <button id="btnSPAUpdate" runat="server" onserverclick="btnSPAUpdate_ServerClick" type="button" class="btn btn-primary" visible="false">Update changes</button>
                                <asp:HiddenField ID="hidTAB" runat="server" Value="image" />
                                <button runat="server" style="width: 100px;" class="btn btn-default pull-left" type="button" data-dismiss="modal">Cancel </button>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>





            </div>

        </div>
    </div>

    <div class="modal fade" id="MsgDependent" role="dialog" tabindex="-1">
        <div class="modal-dialog" role="document">
            <div class="modal-content" id="form_Dependent">
                <div class="modal-header bg-color">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                    <h4 class="modal-title">Add Dependent</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <h5>Name of Dependent: <span class="color-red fsize-16">*</span></h5>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:TextBox runat="server" ID="mtxtRow" class="form-control text-uppercase hidden" type="text" />
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <input runat="server" type="text" class="form-control text-uppercase" id="tDependentName" placeholder="Your Dependent Name Here" />
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator25"
                                                ControlToValidate="tDependentName"
                                                Text="Please fill up Dependent Name."
                                                ValidationGroup="DependentSave"
                                                runat="server" Style="color: red" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <h5>Age: <span class="color-red fsize-16">*</span></h5>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <input runat="server" type="text" onkeypress="return isNumberKey(event)" maxlength="3" class="form-control" id="tDependentAge" value="0" />
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator26"
                                                ControlToValidate="tDependentAge"
                                                Text="Please fill up Age."
                                                ValidationGroup="DependentSave"
                                                runat="server" Style="color: red" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <h5>Relationship: <span class="color-red fsize-16">*</span></h5>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <div class="input-group">
                                                <input runat="server" style="z-index: auto;" id="tDependentRelationship" class="form-control text-uppercase" type="text" disabled /><span class="input-group-btn">
                                                    <button id="bDependentRelationship" runat="server" style="width: 50px; z-index: auto;" data-toggle="modal" data-target="#MsgEmployment" class="btn btn-primary btn-dropbox" type="button" onserverclick="btnEmployment_ServerClick"><i class="fa fa-bars"></i></button>
                                                </span>
                                            </div>
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator27"
                                                ControlToValidate="tDependentRelationship"
                                                Text="Please choose Relationship."
                                                ValidationGroup="DependentSave"
                                                runat="server" Style="color: red" />
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
                            <button runat="server" id="btnAdd" style="width: 100px;" class="btn btn-primary" type="button" validationgroup="DependentSave" onserverclick="btnAdd_ServerClick">Add</button>
                            <button id="btnUpdate" runat="server" onserverclick="btnUpdate_ServerClick" type="button" class="btn btn-primary" visible="false">Update</button>
                            <button runat="server" style="width: 100px;" class="btn btn-default" type="button" data-dismiss="modal">Cancel </button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>


    <div class="modal fade" id="MsgBankAccount" role="dialog" tabindex="-1">
        <div class="modal-dialog" role="document">
            <div class="modal-content" id="form_BankAccount">
                <div class="modal-header bg-color">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                    <h4 class="modal-title" runat="server" id="banktitle">Add Bank Account</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                    <asp:TextBox runat="server" ID="mtxtRow2" class="form-control text-uppercase hidden" type="text" />
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <h5>Bank Name:</h5>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <input runat="server" maxlength="150" type="text" class="form-control text-uppercase" id="tBABank" placeholder="Bank" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <h5>Branch:</h5>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <input runat="server" maxlength="500" type="text" class="form-control text-uppercase" id="tBABranch" placeholder="Branch" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <h5>Type of Account:</h5>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <div class="input-group">
                                                <input runat="server" maxlength="25" type="text" class="form-control text-uppercase" id="tBAAcctType" placeholder="Type of Account" readonly /><span class="input-group-btn">
                                                    <button id="bAccount" runat="server" onserverclick="bTypeofAccount_ServerClick" style="width: 50px; z-index: auto;" data-toggle="modal" data-target="#MsgAccount" class="btn btn-secondary" type="button"><i class="fa fa-bars"></i></button>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <h5>Account No.:</h5>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <input runat="server" maxlength="25" type="text" class="form-control text-uppercase" id="tBAAcctNo" placeholder="Account Number" />
                                        </div>
                                    </div>
                                    <%--<div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <h5>Average Daily Balance:</h5>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <input runat="server" type="text" class="form-control" onkeypress="return isNumberKey(event)" id="tBAAvgDailyBal" value="0" />
                                        </div>
                                    </div>--%>

                                    <%--<div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <h5>Present Balance:</h5>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <input runat="server" type="text" class="form-control" onkeypress="return isNumberKey(event)" id="tBAPresBal" value="0" />
                                        </div>
                                    </div>--%>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <button runat="server" id="btnBAAdd" style="width: 100px;" class="btn btn-primary" type="button" onserverclick="btnBAAdd_ServerClick">Add</button>
                            <button type="button" class="btn btn-primary" runat="server" id="btnUpdateBank" onserverclick="btnUpdateBank_ServerClick" visible="false">Update</button>
                            <button runat="server" style="width: 100px;" class="btn btn-default" type="button" data-dismiss="modal">Cancel </button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--//modal accounts--%>
    <div class="modal fade" id="MsgAccount" role="dialog" tabindex="-1">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header bg-color">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                            <h4 class="modal-title">
                                <label runat="server" id="ChooseText" text="Type of Account" />
                            </h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                    <fieldset>
                                        <asp:GridView ID="gvList" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                            CssClass="table table-bordered table-hover" Width="100%" ShowHeader="True" PageSize="10">
                                            <HeaderStyle BackColor="#66ccff" />
                                            <Columns>
                                                <asp:BoundField DataField="Account" HeaderText="Type of Account" SortExpression="Name" ItemStyle-Font-Size="Medium" />
                                                <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" ID="bSelect" CssClass="btn btn-default btn-success" Width="100%" Height="100%" OnClick="bSelect_Click" CommandArgument='<%# Bind("Account")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div class="modal fade" id="MsgEmployment" role="dialog" tabindex="-1">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header bg-color">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                            <h4 class="modal-title">
                                <label runat="server" id="ChooseEmployment" />
                            </h4>
                        </div>
                        <div class="modal-body">
                            <div class="row" runat="server" id="employmentSearch">
                                <div class="col-lg-2 col-md-2 col-sm-6 col-xs-12">
                                    <h5>Search: </h5>
                                </div>
                                <div class="col-lg-10 col-md-10 col-sm-6 col-xs-12">
                                    <div class="input-group">
                                        <input runat="server" id="txtSalesSearch" placeholder="Search here" class="form-control" type="text" autofocus /><span class="input-group-btn">
                                            <asp:LinkButton runat="server" ID="btnSalesSearch" Style="width: 50px;" CssClass="btn btn-default" OnClick="btnSalesSearch_ServerClick"><i class="fa fa-search"></i></asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">

                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                    <fieldset>
                                        <asp:GridView ID="gvEmployment" runat="server"
                                            AllowPaging="true"
                                            AutoGenerateColumns="false"
                                            PageSize="10"
                                            GridLines="None"
                                            EmptyDataText="No Records Found"
                                            CssClass="table table-hover table-responsive"
                                            Width="100%"
                                            OnPageIndexChanging="gvEmployment_PageIndexChanging">
                                            <HeaderStyle BackColor="#66ccff" />
                                            <Columns>
                                                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-Font-Size="Medium" />
                                                <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" ID="btnSelectEmployment" CssClass="btn btn-default btn-success" Width="100%" Height="100%" OnClick="btnSelectEmployment_Click" CommandArgument='<%# Bind("Code")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
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
                        <div class="modal-footer">
                            <button class="btn btn-facebook btn-primary" type="button" style="width: 100px;" data-dismiss="modal">OK</button>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div class="modal fade" id="MsgSpecialBuyers" role="dialog" tabindex="-1" style="z-index: 9999;">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog modal-lg" role="document">
                    <div class="modal-content">
                        <div class="modal-header bg-color">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                            <h4 class="modal-title">
                                <label runat="server" id="lblSpecBuyersHeader" />
                            </h4>
                        </div>
                        <div class="modal-body">
                            <div class="row" runat="server" id="Div2">
                                <div class="col-lg-2 col-md-2 col-sm-6 col-xs-12">
                                    <h5>Search: </h5>
                                </div>
                                <div class="col-lg-10 col-md-10 col-sm-6 col-xs-12">
                                    <div class="input-group">
                                        <input runat="server" id="txtSearchLinkBuyer" placeholder="Search here" class="form-control" type="text" autofocus /><span class="input-group-btn">
                                            <asp:LinkButton runat="server" ID="LinkButton1" Style="width: 50px;" CssClass="btn btn-default" OnClick="btnSalesSearch_ServerClick"><i class="fa fa-search"></i></asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">

                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                    <fieldset>
                                        <asp:GridView ID="gvCoOwner" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                            CssClass="table table-bordered table-hover" Width="100%" ShowHeader="True" PageSize="3" OnPageIndexChanging="gvCoOwner_PageIndexChanging">
                                            <HeaderStyle BackColor="#66ccff" />
                                            <Columns>
                                                <asp:BoundField DataField="CardCode" HeaderText="ID" SortExpression="CardCode" ItemStyle-Font-Size="Medium" />
                                                <asp:BoundField DataField="FirstName" HeaderText="First Name" SortExpression="FirstName" ItemStyle-Font-Size="Medium" ItemStyle-CssClass="text-uppercase" />
                                                <asp:BoundField DataField="MiddleName" HeaderText="Middle Name" SortExpression="MiddleName" ItemStyle-Font-Size="Medium" ItemStyle-CssClass="text-uppercase" />
                                                <asp:BoundField DataField="LastName" HeaderText="Last Name" SortExpression="LastName" ItemStyle-Font-Size="Medium" ItemStyle-CssClass="text-uppercase" />
                                                <asp:BoundField DataField="SpecifiedBusinessType" HeaderText="Specified BusinessType" SortExpression="Specified Business Type" ItemStyle-Font-Size="Medium" />
                                                <%-- <asp:BoundField DataField="Relationship" HeaderText="Relationship" SortExpression="Relationship" ItemStyle-Font-Size="Medium" />--%>
                                                <%--                                                <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" ItemStyle-Font-Size="Medium" Visible="false" />
                                                <asp:BoundField DataField="MobileNo" HeaderText="Mobile No" SortExpression="Mobile No" ItemStyle-Font-Size="Medium" Visible="false" />
                                                <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" ItemStyle-Font-Size="Medium" Visible="false" />
                                                <asp:BoundField DataField="Residence" HeaderText="Residence" SortExpression="Residence" ItemStyle-Font-Size="Medium" Visible="false" />
                                                <asp:BoundField DataField="ValidID" HeaderText="ValidID" SortExpression="ValidID" ItemStyle-Font-Size="Medium" Visible="false" />
                                                <asp:BoundField DataField="ValidIDNo" HeaderText="ValidIDNo" SortExpression="ValidIDNo" ItemStyle-Font-Size="Medium" Visible="false" />--%>
                                                <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" ID="gvSelectLinkBuyer" CssClass="btn btn-default btn-success" Width="100%" Height="100%" CommandArgument='<%# Bind("CardCode")%>' OnClick="btnSelectLinkBuyer_Click"><i class="fa fa-hand-o-up"></i></asp:LinkButton>
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
                        <div class="modal-footer">
                            <button class="btn btn-facebook btn-primary" type="button" style="width: 100px;" data-dismiss="modal">OK</button>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div class="modal fade" id="Msgcoownerlist" role="dialog" tabindex="-1" style="z-index: 9997;">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog modal-lg" role="document">
                    <div class="modal-content">
                        <div class="modal-header bg-color">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                            <h4 class="modal-title">
                                <label runat="server" id="lblcoownerlisttitle" />
                            </h4>
                        </div>
                        <div class="modal-body">
                            <asp:LinkButton runat="server" ID="btnAddtolist" OnClick="btnAddtolist_Click" CssClass="btn btn-primary" OnClientClick="ShowLinkBuyerModal()">Add to list <i class="fa fa-plus-square"></i></asp:LinkButton>
                            <br />
                            <div class="row">

                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                    <fieldset>
                                        <asp:GridView ID="gvcoownerlist" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                            CssClass="table table-bordered table-hover" Width="100%" ShowHeader="True" PageSize="3" OnPageIndexChanging="gvcoownerlist_PageIndexChanging">
                                            <HeaderStyle BackColor="#66ccff" />
                                            <Columns>
                                                <asp:BoundField DataField="CardCode" HeaderText="ID" SortExpression="CardCode" ItemStyle-Font-Size="Medium" />
                                                <asp:BoundField DataField="FirstName" HeaderText="First Name" SortExpression="FirstName" ItemStyle-Font-Size="Medium" ItemStyle-CssClass="text-uppercase" />
                                                <asp:BoundField DataField="MiddleName" HeaderText="Middle Name" SortExpression="MiddleName" ItemStyle-Font-Size="Medium" ItemStyle-CssClass="text-uppercase" />
                                                <asp:BoundField DataField="LastName" HeaderText="Last Name" SortExpression="LastName" ItemStyle-Font-Size="Medium" ItemStyle-CssClass="text-uppercase" />
                                                <asp:BoundField DataField="SpecifiedBusinessType" HeaderText="SpecifiedBusinessType" SortExpression="Specified Business Type" ItemStyle-Font-Size="Medium" />
                                                <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" ID="gvSelectLinkBuyer" CssClass="btn btn-default btn-success" Width="100%" Height="100%" CommandArgument='<%# Bind("CardCode")%>' OnClick="btnSelectLinkBuyer_Click"><i class="fa fa-hand-o-up"></i></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" ID="gvDeleteLinkBuyer" CssClass="btn btn-default btn-danger" Width="100%" Height="100%" CommandArgument='<%# Bind("CardCode")%>' OnClick="gvDeleteLinkBuyer_Click"><i class="fa fa-trash-o"></i></asp:LinkButton>
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
                        <div class="modal-footer">
                            <button class="btn btn-facebook btn-primary" type="button" style="width: 100px;" data-dismiss="modal">OK</button>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div class="modal fade" id="MsgCharacterRef" role="dialog" tabindex="-1">
        <div class="modal-dialog" role="document">
            <div class="modal-content" id="form_CharacterRef">
                <div class="modal-header bg-color">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                    <h3 class="modal-title" runat="server" id="reftitle">New Character Reference</h3>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">

                                    <input runat="server" id="mtxtRow3" class="form-control text-uppercase hidden" type="text" />

                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <h5>Name:</h5>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <input runat="server" type="text" class="form-control text-uppercase" id="tCRName" placeholder="Name" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <h5>Address:</h5>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <input runat="server" type="text" class="form-control text-uppercase" id="tCRAddress" placeholder="Address" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <h5>Email:</h5>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <input runat="server" id="reftxtEmail" class="form-control" type="text" placeholder="juandelacruz@mail.com" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <h5>Telephone/Cellphone No.:</h5>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <input runat="server" type="text" class="form-control text-uppercase" id="tCRTelNo" maxlength="16" onkeypress="return fnAllowNumeric(event)" placeholder="xx xxx xxxx/xxxx xxx xxxx" />

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
                            <button runat="server" id="btnCRAdd" class="btn btn-primary" type="button" onserverclick="btnCRAdd_ServerClick">Save Changes</button>
                            <button id="btnRefUpdate" runat="server" onserverclick="btnRefUpdate_ServerClick" type="button" class="btn btn-primary" visible="false">Update changes</button>
                            <button runat="server" style="width: 100px;" class="btn btn-default pull-left" type="button" data-dismiss="modal">Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <div id="modalConfirmation" class="modal fade" style="z-index: 9998">
        <div class="modal-dialog" style="width: 30%;">
            <asp:UpdatePanel runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <div class="modal-content">
                        <div class="modal-body">
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
                                    <asp:Button runat="server" ID="btnYes" CssClass="btn btn-info btn-sm" Text="Yes" OnClientClick="modalConfirmation_Hide()" OnClick="btnYes_Click" Style="width: 90px;" />
                                    <button type="button" class="btn btn-default btn-sm" data-dismiss="modal" style="width: 90px;">No</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <div class="modal fade" id="MsgBuyers" role="dialog" tabindex="-1">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog modal-lg" role="document">
                    <div class="modal-content">
                        <div class="modal-header bg-color">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                            <h4 class="modal-title">Choose Buyer
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
                                                        <asp:LinkButton runat="server" ID="bSearch" Style="width: 50px;" CssClass="btn btn-default" OnClick="bSearch_ServerClick"><i class="fa fa-search"></i></asp:LinkButton>
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
                                                            <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="bSelectBuyer" CssClass="btn btn-default btn-success" Width="100%" Height="100%" OnClick="bSelectBuyer_Click" CommandArgument='<%# Bind("CardCode")%>'><i class="fa fa-arrow-right"></i></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="CardCode" HeaderText="BP Code" SortExpression="CardCode" ItemStyle-Font-Size="Medium" />
                                                            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-Font-Size="Medium" />
                                                            <asp:BoundField DataField="TIN" HeaderText="TIN" SortExpression="TIN" ItemStyle-Font-Size="Medium" />

                                                            <%-- --2023-06-16 : ADD APPROVED COLUMN--%>
                                                            <asp:BoundField DataField="Approved" HeaderText="Approved" SortExpression="Approved" ItemStyle-Font-Size="Medium" />

                                                            <%--<asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:LinkButton runat="server" ID="bDeleteBuyer" CssClass="btn btn-default btn-danger" Width="100%" Height="100%" OnClick="bDeleteBuyer_Click" CommandArgument='<%# Bind("CardCode")%>'><i class="fa fa-trash-o"></i></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
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
                            <button class="btn btn-default btn-facebook" type="button" style="width: 100px;" data-dismiss="modal">OK</button>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div class="modal" id="MsgApprove" role="dialog" tabindex="-1">
        <div class="modal-dialog" role="document">
            <div class="modal-content" id="form_Approve">
                <div class="modal-header bg-color">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                    <h4 class="modal-title">Approve Buyer</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="text-align: center;">
                                    <h4>Proof of Approval</h4>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="input-group col-sm-offset-3">
                                                <asp:Label runat="server" ID="lblFileName2" Text=""></asp:Label>
                                                <asp:FileUpload ID="FileUpload2" CssClass="btn btn-default" CommandName="FileUpload2" runat="server" EnableViewState="true" />
                                                <asp:LinkButton ID="btnUpload2" CssClass="btn btn-primary" OnClick="btnUpload_Click" runat="server" Text="Upload" />
                                                <asp:LinkButton ID="btnPreview2" CssClass="btn btn-info" OnClick="btnPreview2_Click" runat="server" Text="Preview" />
                                                <asp:LinkButton ID="btnRemove2" CssClass="btn btn-danger" OnClick="btnRemove_Click" runat="server" Text="Remove" />
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnUpload2" />
                                            <asp:PostBackTrigger ControlID="btnPreview2" />
                                            <asp:PostBackTrigger ControlID="btnRemove2" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <button runat="server" id="Button1" style="width: 100px;" class="btn btn-primary" type="button" onserverclick="btnApprove_ServerClick">Approve</button>
                            <button runat="server" style="width: 100px;" class="btn btn-default" type="button" data-dismiss="modal">Cancel </button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <a href="#0" class="cd-top"></a>
</asp:Content>

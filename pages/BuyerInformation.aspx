<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BuyerInformation.aspx.cs" Inherits="ABROWN_DREAMS.pages.Broker" MaintainScrollPositionOnPostback="true" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>DREAMS Buyers Information</title>
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
    <link rel="shortcut icon" href="../assets/img/Icon.ico" />
    <link rel="stylesheet" href="../assets/css/jquery-ui.css" />
    <link rel="stylesheet" href="../assets/css/bootstrap3-wysihtml5.min.css" />
    <link rel="stylesheet" href="../assets/css/bootstrap.min.css" />
    <link rel="stylesheet" href="../assets/css/styles.css" />
    <link rel="stylesheet" href="../assets/fonts/font-awesome.min.css" />
    <link rel="stylesheet" href="../assets/css/pagination.css" />
    <link rel="stylesheet" href="../assets/css/custom.css" />
    <link rel="stylesheet" href="../assets/css/wizard.css" />
    <link rel="stylesheet" href="../assets/css/sweetalert.css" />
    <link rel="stylesheet" href="../assets/css/ionicons.min.css" />
    <link rel="stylesheet" href="../assets/css/daterangepicker.css" />
    <link rel="stylesheet" href="../assets/css/smart_wizard.css" />
    <link rel="stylesheet" href="../assets/css/font-awesome.css" />
    <link rel="stylesheet" href="../assets/css/smart_wizard_all.css" />
    <link rel="stylesheet" href="../assets/css/smart_wizard_arrows.css" />
    <link rel="stylesheet" href="../assets/css/smart_wizard_dark.css" />
    <link rel="stylesheet" href="../assets/css/smart_wizard_dots.css" />
    <link rel="stylesheet" href="../assets/css/select2.min.css" />
    <link rel="stylesheet" href="../assets/css/AdminLTE.min.css" />
    <link rel="stylesheet" href="../assets/css/_all-skins.min.css" />

    <%-- JavaScript --%>
    <%--    <script src="../assets/js/jquery.min.js"></script>
    <script src="../assets/js/bootstrap.min.js"></script>
    <script src="../assets/js/bootstrap-datetimepicker.min.js"></script>
    <script src="//cdnjs.cloudflare.com/ajax/libs/jspdf/1.3.3/jspdf.min.js"></script>--%>
    <%--<script src="//ajax.googleapis.com/ajax/libs/jqueryui/1.10.2/jquery-ui.min.js"></script>--%>
    <%--fabric js for html5 canvas--%>
    <%--    <script src="../assets/js/fabric.js"></script>
    <script src="../assets/js/bootstrap3-wysihtml5.all.min.js"></script>--%>
    <%--jquery number--%>
    <%--<script src="../assets/js/jquery.number.min.js"></script>--%>

    <script type="text/javascript"> 



        function ResetTextBox() {
            $('.ResetTextBox').find('input:text').val('');
        }

        //ALERTS
        function ShowAlert() {
            $('#modalAlert').modal('show');
        }
        function ShowAlert2() {
            $('#modalAlert2').modal('show');
        }
        function HideAlert() {
            $('#modalAlert').modal('hide');
        }
        function ShowAddDependent() {
            $('#modal-default').modal('show');
        }
        function HideAddDependent() {
            $('#modal-default').modal('hide');
        }
        function ShowAddDependent2() {
            $('#modal-default2').modal('show');
        }
        function HideAddDependent2() {
            $('#modal-default2').modal('hide');
        }
        function MsgCoowner_Hide() {
            $('#MsgCoowner').modal('hide');
        };
        function ShowResultSuccess() {
            $('#title-success').removeAttr('hidden');
            $('#title-failed').attr('hidden', 'hidden');
            $('#button-errback').attr('hidden', 'hidden');
            $('#button-print').removeAttr('hidden');
            ShowAlert()
        }
        function ShowResultFailed() {
            $('#title-success').attr('hidden', 'hidden');
            $('#title-failed').removeAttr('hidden');
            $('#button-errback').removeAttr('hidden');
            $('#button-print').attr('hidden', 'hidden');
            ShowAlert()
        }
        function ShowResultFailed2() {
            $('#title-success').attr('hidden', 'hidden');
            $('#title-failed').removeAttr('hidden');
            $('#button-errback').removeAttr('hidden');
            $('#button-print').attr('hidden', 'hidden');
            ShowAlert2()
        }
        function ShowCharacterReference() {
            $('#modal-default3').modal('show');
        }
        function HideCharacterReference() {
            $('#modal-default3').modal('hide');
        }
        function MsgEmployment_Hide() {
            $('#MsgEmployment').modal('hide');
        };
        function MsgEmployment_Show() {
            $('#MsgEmployment').modal('show');
        };
        function showMsgConfirm() {
            $('#modalConfirmation').modal({
                backdrop: 'static',
                keyboard: false
            })
        }


        function hideMsgConfirm() {
            $('#modalConfirmation').modal('hide');
        }
        function hideMsgAccount() {
            $('#MsgAccount').modal('hide');
        }
        function showMsgAccount() {
            $('#MsgAccount').modal('show');
        }
        function clickTab() {
            $('#tab_3').click();
        }
        function MsgDependent_Hide() {
            $('#MsgDependent').modal('hide');
            HideAddDependent();
        };
        function MsgSPACoBorrower_Hide() {
            $('#MsgSPACoBorrower').modal('hide');
        };
        function MsgSPACoBorrower_Show() {
            $('#MsgSPACoBorrower').modal('show');
        };
    </script>

    <!-- ############ For tabs ############### -->
    <script type="text/javascript">
        function fixTab() {
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "#tab_1";
            //console.log(tabName);
            $('#myTab1 a[href="' + tabName + '"]').tab('show');
            $("#myTab1 a").click(function () {
                $("[id*=TabName]").val($(this).attr("href"));
                //$("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        }
        //$(document).ready(function () {
        //    $("#myTab1 a").click(function () {
        //        console.log($(this).attr("href").replace("#", ""));
        //        $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
        //    });
        //});
        function setCurrentTab(evt) {
            //var href = evt.target.id;
            const anchor = evt.target.closest("a");   // Find closest Anchor (or self)
            if (!anchor) return;                        // Not found. Exit here.
            //console.log(anchor.getAttribute('href'));
            $("[id*=TabName]").val(anchor.getAttribute('href'));
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
            document.getElementById("txtSPOTinNo").focus();
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
                if (elemidno == "txtIDNumber") {
                    elemid = document.getElementById("<%=ddTypeOfID.ClientID%>");
                    if (elemid.value == "ID1") {
                        var istin = true;
                    }
                }
                if (elemidno == "txtIDNumber2") {
                    elemid = document.getElementById("<%=ddTypeOfID2.ClientID%>");
                    if (elemid.value == "ID1") {
                        var istin = true;
                    }
                }
                if (elemidno == "txtIDNumber3") {
                    elemid = document.getElementById("<%=ddTypeOfID3.ClientID%>");
                    if (elemid.value == "ID1") {
                        var istin = true;
                    }
                }
                if (elemidno == "txtIDNumber4") {
                    elemid = document.getElementById("<%=ddTypeOfID4.ClientID%>");
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
        }--%>

        //function TIN_keyup(evt) {
        //    var charCode = (evt.which) ? evt.which : evt.keyCode;
        //    if (charCode == 8 || charCode == 46) { }
        //    else {
        //        var elem = document.getElementById(evt.target.id);
        //        if (elem.value.length < 15) {
        //            if (elem.value.length == 4) {
        //                elem.value = [elem.value.slice(0, 3), "-", elem.value.slice(3)].join('');
        //            }
        //            if (elem.value.length == 8) {
        //                elem.value = [elem.value.slice(0, 7), "-", elem.value.slice(7)].join('');
        //            }
        //            if (elem.value.length == 12) {
        //                elem.value = [elem.value.slice(0, 11), "-", elem.value.slice(11)].join('');
        //            }
        //        }
        //    }
        //}
    </script>
</head>


<body class="hold-transition layout-top-nav nav-color" runat="server">
    <form runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <header class="main-header">
            <div class="navbar-header" style="align-items: center;">
                <a class="navbar-brand navbar-link" href="#">
                    <img src="../assets/img/dreams logo.png" runat="server" class="iconHeader" /></a>
                <button class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navcol-1"><span class="sr-only">Toggle navigation</span><span class="icon-bar"></span><span class="icon-bar"></span><span class="icon-bar"></span></button>
            </div>
            <!-- Header Navbar: style can be found in header.less -->
            <nav class="navbar navbar-static-top">
            </nav>
        </header>

        <!-- Main content -->
        <div class="content-wrapper">
            <br />
            <!-- Horizontal Steppers -->
            <div class="row">
                <div class="col-md-12">
                    <!-- Stepers Wrapper -->
                    <div class="stepwizard">

                        <div class="stepwizard-row setup-panel">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <div class="stepwizard-step col-xs-6">
                                        <a runat="server" id="step1" href="#step-1" type="button" class="btn btn-success btn-circle">1</a>
                                        <p><small>Overview</small></p>
                                    </div>
                                    <div class="stepwizard-step col-xs-6">
                                        <a runat="server" id="step2" href="#step-2" type="button" class="btn btn-default btn-circle">2</a>
                                        <p><small>Buyer Details</small></p>
                                    </div>
                                    <%--                                    <div class="stepwizard-step col-xs-4 hidden">
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

            <div class="container-fluid">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:Panel class="panel panel-primary active" ID="step_1" runat="server" Visible="false">
                            <div class="panel-heading" style="background-color: #5caceb">
                                <h3 class="panel-title">Overview Details</h3>
                            </div>
                            <div class="panel-body">
                                <p style="text-align: center;">
                                    <h2>About this Form</h2>
                                    <br>
                                    This form will require you to encode your personal information as well as upload supplementary documents
                                  for
                                  evaluation.
                                  Information needed will include the following:
                            <br>
                                    <br>
                                    <li>Business Information</li>
                                    <li>Social Security System</li>
                                    <li>Tax Identification Number</li>
                                    <li>Professional Regulation Commission</li>
                                    <li>Passport</li>
                                </p>

                                <div class="input-group col-md-4">
                                    <span
                                        class="input-group-btn">
                                        <button class="btn btn-primary nextBtn pull-right" type="button" id="Button2" runat="server" onserverclick="btnNext_ServerClick">Add New Buyer</button>
                                    </span>
                                    <div>
                                        <%--<label>Edit Code (For Existing Buyer Applications - Ignore if new application)</label>--%>
                                        <%--<div class="row">
                                            <div class="col-md-3">
                                                <div class="radio">
                                                    <label>
                                                        <asp:RadioButton runat="server" GroupName="optionsRadios" ID="rbpsolo" OnCheckedChanged="rbpsolo_Click" />
                                                        <input type="radio" name="optionsRadios" runat="server" id="rbp1" onserverclick="rbpsolo_Click" value="option1" />
                                                        Individual
                          
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="radio">
                                                    <label>
                                                        <asp:RadioButton runat="server" GroupName="optionsRadios" ID="rbpcorp" OnCheckedChanged="rbpcorp_Click" />
                                                        Corporate
                           
                                                    </label>
                                                </div>
                                            </div>
                                        </div>--%>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="input-group">
                                                    <%--                                                    <asp:TextBox runat="server" ID="txtUniqueID" placeholder="Unique ID - 10102302" CssClass="form-control"></asp:TextBox>
                                                    <span
                                                        class="input-group-btn">
                                                        <button class="btn btn-primary nextBtn pull-right" type="button" id="btnNext" runat="server" onserverclick="btnNext_ServerClick">Next</button>
                                                    </span>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="container-fluid">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:Panel class="panel panel-primary" runat="server" ID="step_2" Visible="false">
                            <div class="panel-heading" style="background-color: #5caceb">
                                <h3 class="panel-title">Buyer Details</h3>
                            </div>
                            <div class="panel-body">
                                <div class="box box-default">
                                    <div class="box-header with-border">
                                        <h4>Please fill in the information below.</h4>
                                        <h4>Fields marked with <span class="color-red fsize-16">*</span> are mandatory.</h4>
                                        <br />
                                        <h3 class="box-title">General Information</h3>

                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                        </div>
                                    </div>
                                    <div class="box-body">

                                        <div class="row" id="Div1" runat="server" visible="true">
                                            <div class="col-md-4">
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
                                            <div class="col-md-4" runat="server" id="divTrustGuard" visible="false">
                                                <div class="form-group">
                                                    <label>Role</label>
                                                    <div class="form-control">
                                                        <asp:RadioButton AutoPostBack="true" Text="Guardian" runat="server" type="radio" GroupName="trustguard" OnCheckedChanged="rbGuardian_CheckedChanged" value="false" ID="rbGuardian" />
                                                        <asp:RadioButton AutoPostBack="true" CssClass="pull-right" Text="Guardee" runat="server" type="radio" OnCheckedChanged="rbGuardian_CheckedChanged" GroupName="trustguard" value="true" ID="rbGuardee" />
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
                                            <div class="col-md-4" runat="server" id="sBuyerType" visible="false">
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

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Tax Classification</label>
                                                    <asp:DropDownList runat="server" ID="ddTaxClass" OnSelectedIndexChanged="ddTaxClass_SelectedIndexChanged" DataTextField="Name" DataValueField="Code" CssClass="form-control"
                                                        AutoPostBack="true" Style="">
                                                        <asp:ListItem Text="Engaged in business" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Not engaged in business"></asp:ListItem>
                                                        <%--<asp:ListItem Text="Regular Individual"></asp:ListItem>--%>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="col-md-4" hidden>
                                                <div class="form-group">
                                                    <label>Co-maker</label>
                                                    <asp:TextBox ID="txtComaker" runat="server" class="form-control text-uppercase" type="text" placeholder="Full name" />
                                                </div>
                                            </div>
                                        </div>






                                        <div class="row" id="gensolo" runat="server" visible="true">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Last Name <span class="color-red fsize-16">*</span></label>
                                                    <asp:TextBox ID="txtLastName" OnTextChanged="txtName_TextChanged" runat="server" class="form-control text-uppercase" type="text" placeholder="Dela Cruz" />
                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator4"
                                                        ControlToValidate="txtLastName"
                                                        Text="Please fill up Last Name."
                                                        ValidationGroup="Save"
                                                        runat="server" Style="color: red" />
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>First Name <span class="color-red fsize-16">*</span></label>
                                                    <asp:TextBox ID="txtFirstName" OnTextChanged="txtName_TextChanged" runat="server" class="form-control text-uppercase" type="text" placeholder="Juan" />
                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator5"
                                                        ControlToValidate="txtFirstName"
                                                        Text="Please fill up First Name."
                                                        ValidationGroup="Save"
                                                        runat="server" Style="color: red" />
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Middle Name <span class="color-red fsize-16">*</span></label>
                                                    <asp:TextBox ID="txtMiddleName" OnTextChanged="txtName_TextChanged" runat="server" class="form-control text-uppercase" type="text" placeholder="D" />
                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator6"
                                                        ControlToValidate="txtMiddleName"
                                                        Text="Please fill up Middle Name."
                                                        ValidationGroup="Save"
                                                        runat="server" Style="color: red" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row" id="gencorp" runat="server" visible="false">
                                            <div class="col-md-9">
                                                <div class="form-group">
                                                    <label>Corporate Name <span class="color-red fsize-16">*</span></label>
                                                    <%--<asp:TextBox ID="TextBox1" AutoPostBack="true" OnTextChanged="txtCorpName_TextChanged" runat="server" class="form-control text-uppercase" type="text" placeholder="Your Corporate Name" />--%>
                                                    <asp:TextBox ID="txtCorpName" runat="server" class="form-control text-uppercase" type="text" placeholder="Your Corporate Name" />
                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator28"
                                                        ControlToValidate="txtCorpName"
                                                        Text="Please fill up Corporate Name."
                                                        ValidationGroup="Save"
                                                        runat="server" Style="color: red" />
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <label>TIN No. <span class="color-red fsize-16">*</span></label>
                                                    <%--<asp:TextBox runat="server" AutoPostBack="true" type="text" class="form-control" OnTextChanged="tTINCorp_TextChanged" ID="TextBox1" onkeypress="return fnAllowNumeric(event)" placeholder="xxx-xxx-xxx-xxx" MaxLength="15"></asp:TextBox>--%>
                                                    <asp:TextBox runat="server" type="text" class="form-control" ID="tTINCorp" onkeypress="return fnAllowNumeric(event)" placeholder="xxx-xxx-xxx-xxx" MaxLength="15"></asp:TextBox>
                                                    <%--<asp:TextBox runat="server" AutoPostBack="true" type="text" class="form-control" OnTextChanged="tTINCorp_TextChanged" ID="tTINCorp" onkeyup="TIN_keyup(event);" onkeypress="return fnAllowNumeric(event)" placeholder="xxx-xxx-xxx-xxx" MaxLength="15"></asp:TextBox>--%>
                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator43"
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
                                        <div class="col-md-12">
                                            <div class="box box-danger">
                                                <div class="box-header">
                                                    <h3 class="box-title">Address and Business Information</h3>
                                                </div>
                                                <div class="box-body">
                                                    <h4 class="col-md-12"><b runat="server" id="addtitle">Complete Present Address</b></h4>
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <label>No. <span class="color-red fsize-16">*</span></label>
                                                            <%--<asp:TextBox ID="TextBox1" runat="server" AutoPostBack="true" OnTextChanged="txtAddress_TextChanged" class="form-control text-uppercase" type="text" placeholder="Ermita, Manila, 1000 Metro Manila" />--%>
                                                            <asp:TextBox ID="txtAddress" runat="server" class="form-control text-uppercase" type="text" placeholder="Ermita, Manila, 1000 Metro Manila" />
                                                            <asp:RequiredFieldValidator
                                                                ID="RequiredFieldValidator45"
                                                                ControlToValidate="txtAddress"
                                                                Text="Please fill Present Address No."
                                                                ValidationGroup="Save"
                                                                runat="server" Style="color: red" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <label>Street <span class="color-red fsize-16">*</span></label>
                                                            <%--<asp:TextBox runat="server" type="text" AutoPostBack="true" OnTextChanged="txtAddress_TextChanged" class="form-control text-uppercase" ID="TextBox1" placeholder="Address" />--%>
                                                            <asp:TextBox runat="server" type="text" class="form-control text-uppercase" ID="txtPresentStreet" placeholder="Address" />
                                                            <%--<input runat="server" type="text" class="form-control text-uppercase" id="txtPresentStreet" placeholder="Address" />--%>
                                                            <asp:RequiredFieldValidator
                                                                ID="RequiredFieldValidator46"
                                                                ControlToValidate="txtPresentStreet"
                                                                Text="Please fill Present Street."
                                                                ValidationGroup="Save"
                                                                runat="server" Style="color: red" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <label>Subdivision <span class="color-red fsize-16">*</span></label>
                                                            <%--<asp:TextBox runat="server" type="text" AutoPostBack="true" OnTextChanged="txtAddress_TextChanged" class="form-control text-uppercase" ID="TextBox1" placeholder="Address" />--%>
                                                            <asp:TextBox runat="server" type="text" class="form-control text-uppercase" ID="txtPresentSubdivision" placeholder="Address" />
                                                            <%--<input runat="server" type="text" class="form-control text-uppercase" id="txtPresentSubdivision" placeholder="Address" />--%>
                                                            <asp:RequiredFieldValidator
                                                                ID="RequiredFieldValidator47"
                                                                ControlToValidate="txtPresentSubdivision"
                                                                Text="Please fill Present Subdivision."
                                                                ValidationGroup="Save"
                                                                runat="server" Style="color: red" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <label>Barangay/District <span class="color-red fsize-16">*</span></label>
                                                            <%--<asp:TextBox runat="server" type="text" AutoPostBack="true" OnTextChanged="txtAddress_TextChanged" class="form-control text-uppercase" ID="TextBox1" placeholder="Address" />--%>
                                                            <asp:TextBox runat="server" type="text" class="form-control text-uppercase" ID="txtPresentBarangay" placeholder="Address" />
                                                            <%--<input runat="server" type="text" class="form-control text-uppercase" id="txtPresentBarangay" placeholder="Address" />--%>
                                                            <asp:RequiredFieldValidator
                                                                ID="RequiredFieldValidator48"
                                                                ControlToValidate="txtPresentBarangay"
                                                                Text="Please fill Present Barangay/District."
                                                                ValidationGroup="Save"
                                                                runat="server" Style="color: red" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <label>Municipality/City <span class="color-red fsize-16">*</span></label>
                                                            <%--<asp:TextBox runat="server" type="text" AutoPostBack="true" OnTextChanged="txtAddress_TextChanged" class="form-control text-uppercase" ID="TextBox1" placeholder="Address" />--%>
                                                            <asp:TextBox runat="server" type="text" class="form-control text-uppercase" ID="txtPresentCity" placeholder="Address" />
                                                            <%--<input runat="server" type="text" class="form-control text-uppercase" id="txtPresentCity" placeholder="Address" />--%>
                                                            <asp:RequiredFieldValidator
                                                                ID="RequiredFieldValidator49"
                                                                ControlToValidate="txtPresentCity"
                                                                Text="Please fill Present Municipality/City."
                                                                ValidationGroup="Save"
                                                                runat="server" Style="color: red" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <label>Province <span class="color-red fsize-16">*</span></label>
                                                            <%--<asp:TextBox runat="server" type="text" AutoPostBack="true" OnTextChanged="txtAddress_TextChanged" class="form-control text-uppercase" ID="TextBox1" placeholder="Address" />--%>
                                                            <asp:TextBox runat="server" type="text" class="form-control text-uppercase" ID="txtPresentProvince" placeholder="Address" />
                                                            <%--<input runat="server" type="text" class="form-control text-uppercase" id="txtPresentProvince" placeholder="Address" />--%>
                                                            <asp:RequiredFieldValidator
                                                                ID="RequiredFieldValidator50"
                                                                ControlToValidate="txtPresentProvince"
                                                                Text="Please fill Present Province."
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
                                                                ID="RequiredFieldValidator52"
                                                                ControlToValidate="txtPresPostal"
                                                                Text="Please fill Present Postal Code."
                                                                ValidationGroup="Save"
                                                                runat="server" Style="color: red" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <div class="form-group">
                                                            <label>Present Country</label>
                                                            <%--  <asp:DropDownList ID="DropDownList1" OnSelectedIndexChanged="ddPreCountry_SelectedIndexChanged" runat="server" CssClass="form-control text-uppercase">--%>
                                                            <asp:DropDownList ID="ddPreCountry" OnSelectedIndexChanged="ddPreCountry_SelectedIndexChanged" runat="server" CssClass="form-control text-uppercase">
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
                                                    <div class="col-md-4">
                                                        <div class="form-group">
                                                            <label>Present Address Years of Stay <span class="color-red fsize-16">*</span></label>
                                                            <%--<asp:TextBox ID="TextBox1" runat="server" AutoPostBack="true" OnTextChanged="txtAddress_TextChanged" class="form-control" type="text" placeholder="10" onkeypress="return isNumberKey(event)" />--%>
                                                            <asp:TextBox ID="txtPresYrStay" runat="server" class="form-control" type="text" placeholder="10" onkeypress="return isNumberKey(event)" />
                                                            <asp:RequiredFieldValidator
                                                                ID="RequiredFieldValidator53"
                                                                ControlToValidate="txtPresYrStay"
                                                                Text="Please fill Present Address Years of Stay."
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
                                                                <%--<asp:TextBox ID="TextBox1" runat="server" AutoPostBack="true" OnTextChanged="txtAddress_TextChanged" class="form-control text-uppercase" type="text" placeholder="Quirino Grandstand, 666 Behind, Ermita, Manila, 1000 Metro Manila" />--%>
                                                                <asp:TextBox ID="txtPermanentAdd" runat="server" class="form-control text-uppercase" type="text" placeholder="Quirino Grandstand, 666 Behind, Ermita, Manila, 1000 Metro Manila" />
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator54"
                                                                    ControlToValidate="txtPermanentAdd"
                                                                    Text="Please fill Permanent Address No."
                                                                    ValidationGroup="Save"
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <div class="form-group">
                                                                <label>Street <span class="color-red fsize-16">*</span></label>
                                                                <%--<asp:TextBox runat="server" type="text" AutoPostBack="true" OnTextChanged="txtAddress_TextChanged" class="form-control text-uppercase" ID="TextBox1" placeholder="Address" />--%>
                                                                <asp:TextBox runat="server" type="text" class="form-control text-uppercase" ID="txtPermanentStreet" placeholder="Address" />
                                                                <%--<input runat="server" type="text" class="form-control text-uppercase" id="txtPermanentStreet" placeholder="Address" />--%>
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator55"
                                                                    ControlToValidate="txtPermanentStreet"
                                                                    Text="Please fill Permanent Street."
                                                                    ValidationGroup="Save"
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <div class="form-group">
                                                                <label>Subdivision <span class="color-red fsize-16">*</span></label>
                                                                <%--<asp:TextBox runat="server" type="text" AutoPostBack="true" OnTextChanged="txtAddress_TextChanged" class="form-control text-uppercase" ID="TextBox1" placeholder="Address" />--%>
                                                                <asp:TextBox runat="server" type="text" class="form-control text-uppercase" ID="txtPermanentSubdivision" placeholder="Address" />
                                                                <%--<input runat="server" type="text" class="form-control text-uppercase" id="txtPermanentSubdivision" placeholder="Address" />--%>
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator56"
                                                                    ControlToValidate="txtPermanentSubdivision"
                                                                    Text="Please fill Permanent Subdivision."
                                                                    ValidationGroup="Save"
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <div class="form-group">
                                                                <label>Barangay/District <span class="color-red fsize-16">*</span></label>
                                                                <%--<asp:TextBox runat="server" type="text" AutoPostBack="true" OnTextChanged="txtAddress_TextChanged" class="form-control text-uppercase" ID="TextBox1" placeholder="Address" />--%>
                                                                <asp:TextBox runat="server" type="text" class="form-control text-uppercase" ID="txtPermanentBarangay" placeholder="Address" />
                                                                <%--<input runat="server" type="text" class="form-control text-uppercase" id="txtPermanentBarangay" placeholder="Address" />--%>
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator57"
                                                                    ControlToValidate="txtPermanentBarangay"
                                                                    Text="Please fill Permanent Barangay/District."
                                                                    ValidationGroup="Save"
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <div class="form-group">
                                                                <label>Municipality/City <span class="color-red fsize-16">*</span></label>
                                                                <%--<asp:TextBox runat="server" type="text" AutoPostBack="true" OnTextChanged="txtAddress_TextChanged" class="form-control text-uppercase" ID="TextBox1" placeholder="Address" />--%>
                                                                <asp:TextBox runat="server" type="text" class="form-control text-uppercase" ID="txtPermanentCity" placeholder="Address" />
                                                                <%--<input runat="server" type="text" class="form-control text-uppercase" id="txtPermanentCity" placeholder="Address" />--%>
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator58"
                                                                    ControlToValidate="txtPermanentCity"
                                                                    Text="Please fill Permanent Municipality/City."
                                                                    ValidationGroup="Save"
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <div class="form-group">
                                                                <label>Province <span class="color-red fsize-16">*</span></label>
                                                                <%--<asp:TextBox runat="server" type="text" AutoPostBack="true" OnTextChanged="txtAddress_TextChanged" class="form-control text-uppercase" ID="TextBox1" placeholder="Address" />--%>
                                                                <asp:TextBox runat="server" type="text" class="form-control text-uppercase" ID="txtPermanentProvince" placeholder="Address" />
                                                                <%--<input runat="server" type="text" class="form-control text-uppercase" id="txtPermanentProvince" placeholder="Address" />--%>
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator60"
                                                                    ControlToValidate="txtPermanentProvince"
                                                                    Text="Please fill Permanent Province."
                                                                    ValidationGroup="Save"
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <div class="form-group">
                                                                <label>Permanent Postal Code <span class="color-red fsize-16">*</span></label>
                                                                <%--<asp:TextBox ID="TextBox1" runat="server" AutoPostBack="true" OnTextChanged="txtAddress_TextChanged" class="form-control" onkeypress="return alpha(event)" type="text" placeholder="000" />--%>
                                                                <asp:TextBox ID="txtPermPostal" runat="server" class="form-control" onkeypress="return alpha(event)" type="text" placeholder="000" />
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator61"
                                                                    ControlToValidate="txtPermPostal"
                                                                    Text="Please fill Permanent Postal Code."
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
                                                                    ID="RequiredFieldValidator62"
                                                                    ControlToValidate="txtPermYrStay"
                                                                    Text="Please fill Permanent Address Years of Stay."
                                                                    ValidationGroup="Save"
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4" runat="server" id="tinhide">
                                                        <div class="form-group">
                                                            <label>TIN No. <span runat="server" id="spanTinReq" class="color-red fsize-16">*</span></label>
                                                            <%--<asp:TextBox ID="TextBox1" AutoPostBack="true" OnTextChanged="txtTIN_TextChanged" MaxLength="15" onkeypress="return fnAllowNumeric(event)" runat="server" class="form-control" type="text" placeholder="xxx-xxx-xxx-xxx" />--%>
                                                            <asp:TextBox ID="txtTIN" MaxLength="15" onkeypress="return fnAllowNumeric(event)" runat="server" class="form-control" type="text" placeholder="xxx-xxx-xxx-xxx" />
                                                            <%--<asp:TextBox ID="txtTIN" AutoPostBack="true" OnTextChanged="txtTIN_TextChanged" MaxLength="15" onkeyup="TIN_keyup(event);" onkeypress="return fnAllowNumeric(event)" runat="server" class="form-control" type="text" placeholder="xxx-xxx-xxx-xxx" />--%>
                                                        </div>
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator7"
                                                            ControlToValidate="txtTIN"
                                                            Text="Please fill up TIN."
                                                            ValidationGroup="Save"
                                                            runat="server" Style="color: red" />
                                                        <asp:CustomValidator
                                                            Style="color: red"
                                                            runat="server"
                                                            ID="CustomValidator1"
                                                            ValidationGroup="Save"
                                                            ControlToValidate="txtTIN"
                                                            Text="Incorrect TIN format. Must be xxx-xxx-xxx-xxx."
                                                            OnServerValidate="CustomValidator1_ServerValidate" />
                                                    </div>
                                                    <%--<div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>SSS No.</label>
                                                            <asp:TextBox ID="txtSSS" runat="server" class="form-control" type="text" placeholder="12-123456-7" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>GSIS No.</label>
                                                            <asp:TextBox ID="txtGSIS" runat="server" class="form-control" type="text" placeholder="12-123456-7" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>PAG-IBIG No.</label>
                                                            <asp:TextBox ID="txtPagIbig" runat="server" class="form-control" type="text" placeholder="12-123456-7" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>Business Phone No.</label>
                                                            <asp:TextBox ID="txtBusinessPhoneNo" runat="server" class="form-control" type="text" placeholder="09123456789" />
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
                                                            <asp:DropDownList ID="ddEmploymentStatus" runat="server" CssClass="form-control text-uppercase">
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
                                                            <labelsource of fund: </label>
                                                            <asp:TextBox ID="txtOtherSourceOfFund" runat="server" class="form-control text-uppercase" type="text" placeholder="Others" />
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
                                                                <asp:ListItem Value="---Select Source of Funds---" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Value="MI1" Text="Under Php 10,000"> </asp:ListItem>
                                                                <asp:ListItem Value="MI2" Text="Php 10,001 - 20,000"> </asp:ListItem>
                                                                <asp:ListItem Value="MI3" Text="Php 20,001 - 30,000"> </asp:ListItem>
                                                                <asp:ListItem Value="MI4" Text="Php 30,001 - 40,000"> </asp:ListItem>
                                                                <asp:ListItem Value="MI5" Text="Php 40,001 - 50,000"> </asp:ListItem>
                                                                <asp:ListItem Value="MI5" Text="Above Php 50,000"> </asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>--%>
                                                    <%--<div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Nature of Employment</label>
                                                        <asp:DropDownList ID="ddNatureOfEmployement" runat="server" CssClass="form-control">
                                                            <asp:ListItem></asp:ListItem>
                                                            <asp:ListItem Value="---Select Nature of Employment---" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Value="NE1" Text="Private Sector"> </asp:ListItem>
                                                            <asp:ListItem Value="NE2" Text="Self Employed"> </asp:ListItem>
                                                            <asp:ListItem Value="NE3" Text="Government"> </asp:ListItem>
                                                            <asp:ListItem Value="NE4" Text="Retired"> </asp:ListItem>
                                                            <asp:ListItem Value="NE5" Text="Overseas"> </asp:ListItem>
                                                            <asp:ListItem Value="OTH" Text="Others"> </asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>--%>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="box box-primary">
                                                <div class="box-header">
                                                    <h3 class="box-title" runat="server" id="suppTitle">Supplementary Details</h3>
                                                </div>
                                                <div class="box-body">
                                                    <div class="col-lg-12">
                                                        <div class="row">
                                                            <div runat="server" id="compDetails">
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <label>Last Name <span class="color-red fsize-16">*</span></label>
                                                                        <%--<asp:TextBox runat="server" ID="TextBox1" OnTextChanged="txtLastName2_TextChanged" class="form-control text-uppercase" type="text" placeholder="DELA CRUZ" />--%>
                                                                        <asp:TextBox runat="server" ID="txtLastName2" class="form-control text-uppercase" type="text" placeholder="DELA CRUZ" />
                                                                        <asp:RequiredFieldValidator
                                                                            ID="RequiredFieldValidator35"
                                                                            ControlToValidate="txtLastName2"
                                                                            Text="Please fill up Last Name."
                                                                            ValidationGroup="Save"
                                                                            runat="server" Style="color: red" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <label>First Name <span class="color-red fsize-16">*</span></label>
                                                                        <%--<asp:TextBox runat="server" ID="TextBox1" OnTextChanged="txtFirstName2_TextChanged" class="form-control text-uppercase" type="text" placeholder="JUAN" />--%>
                                                                        <asp:TextBox runat="server" ID="txtFirstName2" class="form-control text-uppercase" type="text" placeholder="JUAN" />
                                                                        <asp:RequiredFieldValidator
                                                                            ID="RequiredFieldValidator36"
                                                                            ControlToValidate="txtFirstName2"
                                                                            Text="Please fill up First Name."
                                                                            ValidationGroup="Save"
                                                                            runat="server" Style="color: red" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <div class="">
                                                                        <label>Middle Name <span class="color-red fsize-16">*</span></label>
                                                                        <%--<asp:TextBox runat="server" ID="TextBox1" OnTextChanged="txtMiddleName2_TextChanged" class="form-control text-uppercase" type="text" placeholder="D" />--%>
                                                                        <asp:TextBox runat="server" ID="txtMiddleName2" class="form-control text-uppercase" type="text" placeholder="D" />
                                                                        <asp:RequiredFieldValidator
                                                                            ID="RequiredFieldValidator37"
                                                                            ControlToValidate="txtMiddleName2"
                                                                            Text="Please fill up Middle Name."
                                                                            ValidationGroup="Save"
                                                                            runat="server" Style="color: red" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <label>No.<%-- <span class="color-red fsize-16">*</span>--%></label>
                                                                        <input runat="server" type="text" class="form-control text-uppercase" id="textAuthorizedPersonAddress" placeholder="Address" />
                                                                        <%--<asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator45"
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
                                                                    ID="RequiredFieldValidator46"
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
                                                                    ID="RequiredFieldValidator47"
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
                                                                    ID="RequiredFieldValidator48"
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
                                                                    ID="RequiredFieldValidator49"
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
                                                                    ID="RequiredFieldValidator50"
                                                                    ControlToValidate="txtAuthorizedPersonProvince"
                                                                    Text="Please fill up Province."
                                                                    ValidationGroup="Save"
                                                                    runat="server" Style="color: red" />--%>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div runat="server" id="divnoncorp">
                                                                <div class="col-md-6">
                                                                    <label for="datepicker">Date of Birth <span class="color-red fsize-16">*</span></label>
                                                                    <div class="input-group">

                                                                        <div class="input-group-addon">
                                                                            <i class="fa fa-calendar"></i>
                                                                        </div>
                                                                        <asp:TextBox runat="server" class="form-control" type="date" ID="dtBirthday" />
                                                                    </div>
                                                                    <asp:RequiredFieldValidator
                                                                        ID="RequiredFieldValidator34"
                                                                        ControlToValidate="dtBirthday"
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
                                                                        ControlToValidate="dtBirthday"
                                                                        Text="Below legal age."
                                                                        OnServerValidate="CustomValidator4_ServerValidate" />
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <label>Place of Birth</label>
                                                                        <asp:TextBox runat="server" ID="txtPlaceOfBirth" class="form-control text-uppercase" type="text" placeholder="Manila" />
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
                                                                        <asp:DropDownList ID="ddGender" runat="server" CssClass="form-control text-uppercase">
                                                                            <asp:ListItem></asp:ListItem>
                                                                            <asp:ListItem Value="---Select Gender---" Selected="True"></asp:ListItem>
                                                                            <asp:ListItem Value="M" Text="Male"> </asp:ListItem>
                                                                            <asp:ListItem Value="F" Text="Female"> </asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <label>Citizenship</label>
                                                                        <%--<asp:TextBox ID="txtCitizenship" runat="server" class="form-control text-uppercase" type="text" placeholder="Fil" />--%>
                                                                        <select runat="server" id="txtCitizenship" name="nationality" class="form-control text-uppercase">
                                                                            <option value="filipino">Filipino</option>
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
                                                                            <%--<option value="filipino">Filipino</option>--%>
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
                                                                    <asp:TextBox ID="txtEmail" runat="server" class="form-control" type="text" placeholder="Juan@gmail.com" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group">

                                                                    <label>Home Tel No.</label>
                                                                    <asp:TextBox ID="txtTelNo" runat="server" class="form-control" type="text" placeholder="123-45-6789" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label>Cellphone No.</label>
                                                                    <asp:TextBox ID="txtMobileNo" runat="server" class="form-control" type="text" placeholder="09171231234" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label>Facebook Account</label>
                                                                    <asp:TextBox ID="txtFacebook" runat="server" class="form-control" type="text" placeholder="Juan@gmail.com" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>







                                                    <div class="col-lg-12">
                                                        <div class="row">

                                                            <div class="col-md-6" runat="server" id="ddothers1">
                                                                <div class="form-group">
                                                                    <label>Type of ID <span class="color-red fsize-16">*</span></label>
                                                                    <asp:DropDownList ID="ddTypeOfID" runat="server" CssClass="form-control"
                                                                        DataTextField="Name" DataValueField="Code"
                                                                        OnSelectedIndexChanged="ddTypeOfID_SelectedIndexChanged" AutoPostBack="true">
                                                                        <asp:ListItem Value="---Select Type of ID---" Selected="True"></asp:ListItem>
                                                                        <%--    <asp:ListItem Value="ID1" Text="TIN"> </asp:ListItem>
                                                                        <asp:ListItem Value="ID2" Text="SSS"> </asp:ListItem>
                                                                        <asp:ListItem Value="ID3" Text="Pagibig"> </asp:ListItem>
                                                                        <asp:ListItem Value="ID4" Text="Passport"> </asp:ListItem>
                                                                        <asp:ListItem Value="OTH" Text="Others"> </asp:ListItem>--%>
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator
                                                                        ID="RequiredFieldValidator2"
                                                                        ControlToValidate="ddTypeOfID"
                                                                        Text="Please select Type of ID."
                                                                        ValidationGroup="Save"
                                                                        InitialValue="---Select Type of ID---"
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
                                                                    <%--<input id="txtIDNumber" runat="server" class="form-control" type="text" placeholder="12345678" />--%>
                                                                    <%--<asp:TextBox ID="TextBox1" AutoPostBack="true" OnTextChanged="txtIDNumber_TextChanged" runat="server" class="form-control" type="text" placeholder="12345678"></asp:TextBox>--%>
                                                                    <asp:TextBox ID="txtIDNumber" runat="server" AutoPostBack="true" OnTextChanged="txtIDNumber_TextChanged" class="form-control" type="text" placeholder="12345678"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator
                                                                        ID="RequiredFieldValidator3"
                                                                        ControlToValidate="txtIDNumber"
                                                                        Text="Please fill up ID Number."
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
                                                                        Type of ID 2 
                                                                        <%--<span class="color-red fsize-16">*</span>--%>
                                                                    </label>
                                                                    <asp:DropDownList ID="ddTypeOfID2" runat="server" CssClass="form-control"
                                                                        DataTextField="Name" DataValueField="Code" OnSelectedIndexChanged="ddTypeOfID2_SelectedIndexChanged"
                                                                        AutoPostBack="true">
                                                                        <asp:ListItem Value="---Select Type of ID---" Selected="True"></asp:ListItem>
                                                                        <%--   <asp:ListItem Value="ID1" Text="TIN"> </asp:ListItem>
                                                                        <asp:ListItem Value="ID2" Text="SSS"> </asp:ListItem>
                                                                        <asp:ListItem Value="ID3" Text="Pagibig"> </asp:ListItem>
                                                                        <asp:ListItem Value="ID4" Text="Passport"> </asp:ListItem>
                                                                        <asp:ListItem Value="OTH" Text="Others"> </asp:ListItem>--%>
                                                                    </asp:DropDownList>
                                                                    <%--COMMENTED 04-04-2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                                                                    <%--       <asp:RequiredFieldValidator
                                                                        ID="RequiredFieldValidator42"
                                                                        ControlToValidate="ddTypeOfID2"
                                                                        Text="Please select Type of ID."
                                                                        ValidationGroup="Save"
                                                                        InitialValue="---Select Type of ID---"
                                                                        runat="server" Style="color: red" />--%>
                                                                </div>
                                                            </div>


                                                            <div class="col-md-3" runat="server" id="others2" visible="false">
                                                                <div class="form-group">
                                                                    <%-- <asp:Label runat="server" ID="Label2" Text="ID No."></asp:Label>--%>
                                                                    <label>Specify ID</label>
                                                                    <asp:TextBox runat="server" ID="txtOthers2" class="form-control text-uppercase" type="text" placeholder="ID" />
                                                                    <%--COMMENTED 04-04-2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                                                                    <%--  <asp:RequiredFieldValidator
                                                                        ID="RequiredFieldValidator11"
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
                                                                    <%--<input id="txtIDNumber2" runat="server" class="form-control" type="text" placeholder="12345678" />--%>
                                                                    <%--<asp:TextBox ID="TextBox1" AutoPostBack="true" OnTextChanged="txtIDNumber2_TextChanged" runat="server" class="form-control" type="text" placeholder="12345678"></asp:TextBox>--%>
                                                                    <asp:TextBox ID="txtIDNumber2" runat="server" AutoPostBack="true" OnTextChanged="txtIDNumber2_TextChanged" class="form-control" type="text" placeholder="12345678"></asp:TextBox>
                                                                    <%--COMMENTED 04-04-2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                                                                    <%--   <asp:RequiredFieldValidator
                                                                        ID="RequiredFieldValidator44"
                                                                        ControlToValidate="txtIDNumber2"
                                                                        Text="Please fill up ID Number."
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
                                                                    <asp:DropDownList ID="ddTypeOfID3" runat="server" CssClass="form-control"
                                                                        DataTextField="Name" DataValueField="Code" OnSelectedIndexChanged="ddTypeOfID3_SelectedIndexChanged"
                                                                        AutoPostBack="true">
                                                                        <asp:ListItem Value="---Select Type of ID---" Selected="True"></asp:ListItem>
                                                                        <%--  <asp:ListItem Value="ID1" Text="TIN"> </asp:ListItem>
                                                                        <asp:ListItem Value="ID2" Text="SSS"> </asp:ListItem>
                                                                        <asp:ListItem Value="ID3" Text="Pagibig"> </asp:ListItem>
                                                                        <asp:ListItem Value="ID4" Text="Passport"> </asp:ListItem>
                                                                        <asp:ListItem Value="OTH" Text="Others"> </asp:ListItem>--%>
                                                                    </asp:DropDownList>
                                                                    <%--<asp:RequiredFieldValidator
                                                                        ID="RequiredFieldValidator43"
                                                                        ControlToValidate="ddTypeOfID3"
                                                                        Text="Please select Type of ID."
                                                                        ValidationGroup="Save"
                                                                        InitialValue="---Select Type of ID---"
                                                                        runat="server" Style="color: red" />--%>
                                                                </div>
                                                            </div>

                                                            <div class="col-md-3" runat="server" id="others3" visible="false">
                                                                <div class="form-group">
                                                                    <%-- <asp:Label runat="server" ID="Label2" Text="ID No."></asp:Label>--%>
                                                                    <label>Specify ID</label>
                                                                    <asp:TextBox runat="server" ID="txtOthers3" class="form-control text-uppercase" type="text" placeholder="ID" />
                                                                    <asp:RequiredFieldValidator
                                                                        ID="RequiredFieldValidator40"
                                                                        ControlToValidate="txtOthers3"
                                                                        ValidationGroup="Next"
                                                                        Text="Please specify ID."
                                                                        runat="server" Style="color: red" />
                                                                </div>
                                                            </div>


                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label>ID Number 3</label>
                                                                    <%--<input id="txtIDNumber3" runat="server" class="form-control" type="text" placeholder="12345678" />--%>
                                                                    <asp:TextBox ID="txtIDNumber3" AutoPostBack="true" OnTextChanged="txtIDNumber3_TextChanged" runat="server" class="form-control" type="text" placeholder="12345678"></asp:TextBox>
                                                                    <%--<asp:RequiredFieldValidator
                                                                        ID="RequiredFieldValidator45"
                                                                        ControlToValidate="txtIDNumber3"
                                                                        Text="Please fill up ID Number."
                                                                        ValidationGroup="Save"
                                                                        Enabled="false"
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
                                                                    <asp:DropDownList ID="ddTypeOfID4" runat="server" CssClass="form-control"
                                                                        DataTextField="Name" DataValueField="Code" OnSelectedIndexChanged="ddTypeOfID4_SelectedIndexChanged"
                                                                        AutoPostBack="true">
                                                                        <asp:ListItem Value="---Select Type of ID---" Selected="True"></asp:ListItem>
                                                                        <%--    <asp:ListItem Value="ID1" Text="TIN"> </asp:ListItem>
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
                                                                        ID="RequiredFieldValidator41"
                                                                        ControlToValidate="txtOthers4"
                                                                        ValidationGroup="Next"
                                                                        Text="Please specify ID."
                                                                        runat="server" Style="color: red" />
                                                                </div>
                                                            </div>


                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label>ID Number 4</label>
                                                                    <%--<input id="txtIDNumber4" runat="server" class="form-control" type="text" placeholder="12345678" />--%>
                                                                    <asp:TextBox ID="txtIDNumber4" AutoPostBack="true" OnTextChanged="txtIDNumber4_TextChanged" runat="server" class="form-control" type="text" placeholder="12345678"></asp:TextBox>
                                                                    <%--<asp:RequiredFieldValidator
                                                                        ID="RequiredFieldValidator46"
                                                                        ControlToValidate="txtIDNumber3"
                                                                        Text="Please fill up ID Number."
                                                                        ValidationGroup="Save"
                                                                        Enabled="false"
                                                                        runat="server" Style="color: red" />--%>
                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>




















                                                    <div class="col-md-6" runat="server" id="divnoncorp3">
                                                        <div class="form-group">
                                                            <label>Civil Status</label>
                                                            <asp:DropDownList ID="ddCivilStatus" AutoPostBack="true" OnSelectedIndexChanged="ddCivilStatus_SelectedIndexChanged" runat="server" CssClass="form-control">
                                                                <asp:ListItem></asp:ListItem>
                                                                <asp:ListItem Value="---Select Civil Status---" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Value="CS1" Text="Single"> </asp:ListItem>
                                                                <asp:ListItem Value="CS2" Text="Married"> </asp:ListItem>

                                                                <%-- 2023-06-14 : REMOVED WIDOW AND OTHERS; RETAIN SINGLE AND MARRIED --%>
                                                                <%--     <asp:ListItem Value="CS3" Text="Widow"> </asp:ListItem>
                                                                <asp:ListItem Value="CS4" Text="Legally Separated"> </asp:ListItem>
                                                                <asp:ListItem Value="OTH" Text="Others"> </asp:ListItem>--%>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>SSS No.</label>
                                                            <%--<asp:TextBox ID="TextBox1" AutoPostBack="true" OnTextChanged="txtSSS_TextChanged" runat="server" class="form-control" type="text" placeholder="12-123456-7" />--%>
                                                            <asp:TextBox ID="txtSSS" runat="server" class="form-control" type="text" placeholder="12-123456-7" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>GSIS No.</label>
                                                            <asp:TextBox ID="txtGSIS" runat="server" class="form-control" type="text" placeholder="12-123456-7" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>PAG-IBIG No.</label>
                                                            <%--<asp:TextBox ID="TextBox1" AutoPostBack="true" OnTextChanged="txtPagIbig_TextChanged" runat="server" class="form-control" type="text" placeholder="12-123456-7" />--%>
                                                            <asp:TextBox ID="txtPagIbig" runat="server" class="form-control" type="text" placeholder="12-123456-7" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>Business Phone No.</label>
                                                            <asp:TextBox ID="txtBusinessPhoneNo" runat="server" class="form-control" type="text" MaxLength="50" placeholder="09123456789" />
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
                                                    <div class="col-md-6 hidden" runat="server" id="empstatus">
                                                        <div class="form-group">
                                                            <label>Employment Status</label>
                                                            <asp:DropDownList ID="ddEmploymentStatus" runat="server" CssClass="form-control text-uppercase">
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
                                                    <div runat="server" id="sourceoffund">
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <%--KARL - Added Validator to source of funds 2023/05/04--%>
                                                                <label>Source of Funds <span class="color-red fsize-16">*</span></label>
                                                                <asp:DropDownList ID="ddSourceFunds" AutoPostBack="true" DataTextField="Name" DataValueField="Code" OnSelectedIndexChanged="ddSourceFunds_SelectedIndexChanged" runat="server" CssClass="form-control text-uppercase">
                                                                    <%--                                                                    <asp:ListItem></asp:ListItem>
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
                                                                <asp:TextBox ID="txtOtherSourceOfFund" runat="server" class="form-control text-uppercase" type="text" placeholder="Others" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>Monthly Gross Income</label>
                                                            <asp:DropDownList ID="ddMonthlyIncome" runat="server" CssClass="form-control text-uppercase">
                                                                <asp:ListItem></asp:ListItem>
                                                                <asp:ListItem Value="---Select Source of Funds---" Selected="True"></asp:ListItem>
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
                                                                        ID="RequiredFieldValidator21"
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
                                                                        ID="RequiredFieldValidator23"
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
                                                                    <label>Last Name</label>
                                                                    <asp:TextBox runat="server" ID="txtContactLName" ValidationGroup="Next" class="form-control text-uppercase" type="text" placeholder="Dela Cruz" />
                                                                    <asp:RequiredFieldValidator
                                                                        ID="RequiredFieldValidator24"
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
                                                                        ID="RequiredFieldValidator25"
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
                                                                        ID="RequiredFieldValidator26"
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
                                                                        ID="RequiredFieldValidator27"
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
                                        <div class="col-md-12">
                                            <div class="box box-primary" runat="server" id="empDetails">
                                                <div class="box-header">
                                                    <h3 class="box-title">Employer Details</h3>
                                                </div>
                                                <div class="box-body">
                                                    <div class="col-md-4">
                                                        <div class="form-group">
                                                            <label>Employer Business Name</label>
                                                            <asp:TextBox runat="server" ID="txtEmpName" placeholder="Ayala Land" CssClass="form-control text-uppercase"></asp:TextBox>
                                                            <%--<input class="form-control" type="text" placeholder="Ayala Land" />--%>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <div class="form-group">
                                                            <label>Employer Business Address</label>
                                                            <asp:TextBox runat="server" ID="txtEmpAdd" placeholder="Ermita, Manila, 1000 Metro Manila" CssClass="form-control text-uppercase"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <div class="form-group">
                                                            <%--KARL - Added Validator to position 2023/05/04--%>
                                                            <label>Position <span class="color-red fsize-16">*</span></label>
                                                            <asp:TextBox runat="server" ID="txtMyPosition" placeholder="Sales Agent" CssClass="form-control text-uppercase"></asp:TextBox>
                                                            <asp:RequiredFieldValidator
                                                                ID="txtMyPosition_RequiredFieldValidator"
                                                                ControlToValidate="txtMyPosition" CssClass="col-md-12"
                                                                Text="Please fill up Position."
                                                                ValidationGroup="Save"
                                                                Enabled="false"
                                                                runat="server" Style="color: red" />
                                                        </div>
                                                    </div>

                                                    <div class="col-md-4">
                                                        <div class="form-group">
                                                            <label>Years of Service</label>
                                                            <asp:TextBox runat="server" onkeypress="return isNumberKey(event)" ID="txtEmpYrService" placeholder="2" CssClass="form-control" type="text"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <div class="form-group">
                                                            <label>Office Telephone No.</label>
                                                            <asp:TextBox runat="server" ID="txtEmpTelNo" placeholder="123 1234" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <div class="form-group">
                                                            <label>Fax No.</label>
                                                            <asp:TextBox runat="server" ID="txtEmpFaxNo" placeholder="123 1234" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <%--<div class="col-md-4">
                                                            <div class="form-group">
                                                                <label>Nature of Employer</label>
                                                                <input class="form-control" type="text" placeholder="" />
                                                            </div>
                                                        </div>--%>
                                                    <div class="col-md-4">
                                                        <div class="form-group">
                                                            <label>Nature of Employment <span class="color-red fsize-16">*</span></label>
                                                            <asp:DropDownList ID="ddNatureOfEmployement" runat="server" CssClass="form-control">
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
                                                            <%--         <asp:RequiredFieldValidator
                                                                ID="RequiredFieldValidator1"
                                                                ControlToValidate="ddNatureOfEmployement"
                                                                Text="Please select Nature of Employment."
                                                                ValidationGroup="Save"
                                                                ValidationExpression="^$|^---Select Nature of Employment---$"
                                                                runat="server" Style="color: red" />--%>
                                                            <asp:RequiredFieldValidator
                                                                ID="RequiredFieldValidator1"
                                                                ControlToValidate="ddNatureOfEmployement"
                                                                Text="Please select Nature of Employment."
                                                                ValidationGroup="Save"
                                                                InitialValue="---Select Nature of Employment---"
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
                                            <div class="box box-success" runat="server" id="coborrowerGrid">
                                                <div class="box-header with-border">
                                                    <h3 class="box-title titlemargin">Co-owner Details</h3>
                                                </div>
                                                <div class="box-body">
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="box box-success">
                                                                <div class="box-header with-border">
                                                                    <button class="btn btn-default btn-primary pull-left titlemargin" type="button" data-toggle="modal" data-target="#MsgCoowner" runat="server" id="Button6" onserverclick="bDependent_ServerClick" style="color: white;">Add Co-owner <i class="fa fa-plus-square"></i></button>
                                                                    <asp:CustomValidator
                                                                        Style="color: red"
                                                                        runat="server"
                                                                        ID="CustomValidator2"
                                                                        ValidationGroup="Save"
                                                                        CssClass="col-md-12"
                                                                        Text="Please add Co-Owner."
                                                                        OnServerValidate="CustomValidator2_ServerValidate" />
                                                                </div>
                                                                <div class="box-body titlemargin" style="overflow-x: auto">
                                                                    <asp:GridView ID="gvCoOwner" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                                                        CssClass="table table-bordered table-hover" Width="100%" ShowHeader="True" PageSize="3" OnPageIndexChanging="gvCoOwner_PageIndexChanging">
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
                                            <div class="box box-success" runat="server" id="othersGrid">
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
                                            <div class="box box-success" runat="server" id="empDependents">
                                                <div class="box-header with-border">
                                                    <h3 class="box-title">Dependents</h3>

                                                    <div class="box-tools pull-right">
                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                                    </div>
                                                </div>
                                                <div class="box-body">
                                                    <div class="row">
                                                        <%--<div class="col-md-12">

                                                        <div class="form-group">
                                                            <div class="col-md-3">
                                                                <div class="radio">
                                                                    <label>
                                                                        <input type="radio" name="optionsRadios" runat="server" id="rb1" class="rb1" value="option1" />
                                                                        Owned
                          
                                                                    </label>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <div class="radio">
                                                                    <label>
                                                                        <input type="radio" name="optionsRadios" runat="server" id="rb2" class="rb2" value="option2" />
                                                                        Mortgaged
                           
                                                                    </label>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <div class="radio">
                                                                    <label>
                                                                        <input type="radio" name="optionsRadios" runat="server" id="rb3" class="rb3" value="option3" />
                                                                        Living w/ Relatives
                           
                                                                    </label>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <div class="radio">
                                                                    <label>
                                                                        <input type="radio" name="optionsRadios" runat="server" id="rb4" class="rb4" value="option4" />
                                                                        Rented

                                                                    <asp:UpdatePanel runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:TextBox type="text" placeholder="25,000" CssClass="form-control rented" Enabled="false" runat="server" AutoPostBack="true" ID="txtRented" OnTextChanged="txtRented_OnKeyup" />
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    </label>
                                                                </div>
                                                            </div>
                                                        </div>


                                                    </div>--%>
                                                        <div class="col-md-12">
                                                            <div class="">
                                                                <div class="">
                                                                    <%--<button class="btn btn-primary" data-toggle="modal" data-target="#modal-default" type="button">
                                                                    <i class="fa fa-plus-circle"></i>Add Dependent</button>--%>
                                                                    <asp:LinkButton class="btn btn-primary" runat="server" ID="bDependent" OnClick="btnAddDep_Click" type="button">
                                                                    <i class="fa fa-plus-circle"></i>Add Dependent</asp:LinkButton>
                                                                </div>
                                                                <div class="">
                                                                    <asp:UpdatePanel runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:GridView ID="gvDependent" runat="server" AllowPaging="true" AutoGenerateColumns="false"
                                                                                EmptyDataText="No Records Found"
                                                                                CssClass="table table-bordered table-hover" OnRowCommand="gvDependent_RowCommand" Width="100%" ShowHeader="True" PageSize="3">
                                                                                <HeaderStyle BackColor="#66ccff" />
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" ItemStyle-Font-Size="Medium" />
                                                                                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-Font-Size="Medium" />
                                                                                    <asp:BoundField DataField="Age" HeaderText="Age" SortExpression="Age" ItemStyle-Font-Size="Medium" />
                                                                                    <asp:BoundField DataField="RelationShip" HeaderText="RelationShip" SortExpression="RelationShip" ItemStyle-Font-Size="Medium" />

                                                                                    <asp:TemplateField
                                                                                        HeaderStyle-HorizontalAlign="Center"
                                                                                        HeaderText="Action">
                                                                                        <ItemTemplate>
                                                                                            <%--<button runat="server" id="gvShare" type="button"
                                                                                    class="btn btn-default btn-success" data-toggle="modal"
                                                                                    data-target="#modal-AddSharing">
                                                                                    <i class="fa fa-edit"></i>
                                                                                </button>--%>
                                                                                            <asp:LinkButton runat="server" ID="btnDepEdit" type="button"
                                                                                                class="btn btn-default btn-success" OnClick="btnDepEdit_Click" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>">
                                                                                    <i class="fa fa-edit"></i>
                                                                                            </asp:LinkButton>
                                                                                            <asp:LinkButton runat="server" ID="btnDeleteDependent" type="button"
                                                                                                class="btn btn-default btn-danger"
                                                                                                OnClick="btnDeleteDependent_Click"
                                                                                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>">
                                                                                    <i class="fa fa-trash"></i>
                                                                                            </asp:LinkButton>
                                                                                            <%--<button runat="server" id="Button2" type="button"
                                                                                    class="btn btn-default btn-danger"
                                                                                    data-toggle="modal"
                                                                                    data-target="#modal-AddSharing">
                                                                                    <i class="fa fa-trash"></i>
                                                                                </button>--%>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                                <PagerStyle CssClass="pagination-ys" />
                                                                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B"
                                                                                    ForeColor="Blue"
                                                                                    Font-Bold="true" />
                                                                            </asp:GridView>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12" style="margin-bottom: 5px">
                                                <button class="btn btn-primary nextBtn pull-left" type="button" id="btnStartPage" runat="server" onserverclick="btnStartPage_ServerClick">Back</button>
                                                <button class="btn btn-primary nextBtn pull-right" type="button" id="Button1" validationgroup="Save" runat="server" onserverclick="btnNext2_ServerClick">Next</button>
                                            </div>
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
                                        <li class="active"><a href="#tab_1" onclick="setCurrentTab(event);" data-toggle="tab" aria-expanded="true" id="referenceBtn">References</a></li>
                                        <li class=""><a href="#tab_2" onclick="setCurrentTab(event);" data-toggle="tab" aria-expanded="false" runat="server" id="spouseBtn">Spouse</a></li>
                                        <li class=""><a href="#tab_3" onclick="setCurrentTab(event);" data-toggle="tab" aria-expanded="false" runat="server" id="SPABtn">SPA</a></li>
                                        <%-- <li class="pull-right"><a href="#" class="text-muted"><i class="fa fa-gear"></i></a></li>--%>
                                    </ul>
                                    <asp:HiddenField ID="TabName" runat="server" />
                                    <div class="tab-content">
                                        <div class="tab-pane active" id="tab_1">
                                            <div class="content">
                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                        <div class="box box-primary">
                                                            <div class="box-header">
                                                                <h3 class="box-title">
                                                                    <%--<button class="btn btn-primary" type="button" data-toggle="modal"
                                                                data-target="#modal-default2">
                                                                <i class="fa fa-plus-circle"></i>Add Bank Account</button>--%>
                                                                    <asp:LinkButton class="btn btn-primary" type="button" runat="server"
                                                                        OnClick="btnShowBank_Click">
                                                                <i class="fa fa-plus-circle"></i>Add Bank Account</asp:LinkButton>
                                                                </h3>

                                                                <div class="box-tools pull-right">
                                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                        <i
                                                                            class="fa fa-minus"></i>
                                                                    </button>
                                                                </div>
                                                            </div>
                                                            <div class="box-body">
                                                                <asp:GridView ID="gvBankAccount" runat="server" AllowPaging="true" AutoGenerateColumns="false"
                                                                    EmptyDataText="No Records Found"
                                                                    CssClass="table table-bordered table-hover" Width="100%" ShowHeader="True" PageSize="3">
                                                                    <HeaderStyle BackColor="#66ccff" />
                                                                    <Columns>
                                                                        <%--  <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id"
                                                                    ItemStyle-Font-Size="Medium" />--%>

                                                                        <asp:BoundField DataField="ID" HeaderText="ID"
                                                                            SortExpression="ID"
                                                                            ItemStyle-Font-Size="Medium" />
                                                                        <asp:BoundField DataField="Bank" HeaderText="Bank Name"
                                                                            SortExpression="Bank"
                                                                            ItemStyle-Font-Size="Medium" />
                                                                        <asp:BoundField DataField="Branch" HeaderText="Branch"
                                                                            SortExpression="Branch"
                                                                            ItemStyle-Font-Size="Medium" />
                                                                        <asp:BoundField DataField="AcctType" HeaderText="Type of Account"
                                                                            SortExpression="TypeOfAccount"
                                                                            ItemStyle-Font-Size="Medium" />
                                                                        <asp:BoundField DataField="AcctNo" HeaderText="Account No"
                                                                            SortExpression="AccountNo"
                                                                            ItemStyle-Font-Size="Medium" />
                                                                        <%--                         <asp:BoundField DataField="AvgDailyBal" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"
                                                                    HeaderText="Average Daily Balance"
                                                                    SortExpression="AverageDailyBalance"
                                                                    ItemStyle-Font-Size="Medium" />
                                                                <asp:BoundField DataField="PresentBal" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"
                                                                    HeaderText="Present Balance"
                                                                    SortExpression="PresentBalance"
                                                                    ItemStyle-Font-Size="Medium" />--%>
                                                                        <asp:BoundField DataField="AcctNoOrig" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"
                                                                            HeaderText="Account No"
                                                                            SortExpression="AccountNo"
                                                                            ItemStyle-Font-Size="Medium" />
                                                                        <asp:TemplateField
                                                                            HeaderStyle-HorizontalAlign="Center"
                                                                            HeaderText="Action">
                                                                            <ItemTemplate>
                                                                                <%--<button runat="server" id="gvShare" type="button"
                                                                            class="btn btn-default btn-success" data-toggle="modal"
                                                                            data-target="#modal-AddSharing">
                                                                            <i class="fa fa-edit"></i>
                                                                        </button>--%>
                                                                                <asp:LinkButton runat="server" ID="btnBankUpdate" type="button"
                                                                                    class="btn btn-default btn-success" OnClick="btnBankUpdate_Click" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>">
                                                                                    <i class="fa fa-edit"></i>
                                                                                </asp:LinkButton>
                                                                                <asp:LinkButton runat="server" ID="btnDeleteBank" type="button"
                                                                                    class="btn btn-default btn-danger"
                                                                                    OnClick="btnDeleteBank_Click"
                                                                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>">
                                                                                    <i class="fa fa-trash"></i>
                                                                                </asp:LinkButton>
                                                                                <%--<button runat="server" id="Button2" type="button"
                                                                            class="btn btn-default btn-danger"
                                                                            data-toggle="modal"
                                                                            data-target="#modal-AddSharing">
                                                                            <i class="fa fa-trash"></i>
                                                                        </button>--%>
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
                                                        <div class="box box-primary">
                                                            <div class="box-header">
                                                                <h3 class="box-title">
                                                                    <%--<button class="btn btn-primary" type="button" data-toggle="modal" data-target="#modal-default3">
                                                                <i class="fa fa-plus-circle"></i>Add Character Ref</button></h3>--%>
                                                                    <asp:LinkButton class="btn btn-primary" type="button" runat="server" OnClick="btnRefShow_Click">
                                                                <i class="fa fa-plus-circle"></i>Add Character Ref</asp:LinkButton></h3>

                                                                <div class="box-tools pull-right">
                                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                                                </div>
                                                            </div>
                                                            <div class="box-body">
                                                                <asp:GridView ID="gvCharacter" runat="server" AllowPaging="true" AutoGenerateColumns="false"
                                                                    EmptyDataText="No Records Found"
                                                                    CssClass="table table-bordered table-hover" Width="100%" ShowHeader="True" PageSize="3">
                                                                    <HeaderStyle BackColor="#66ccff" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="Id" HeaderText="ID"
                                                                            SortExpression="ID"
                                                                            ItemStyle-Font-Size="Medium" />
                                                                        <asp:BoundField DataField="Name" HeaderText="Full Name"
                                                                            SortExpression="Name"
                                                                            ItemStyle-Font-Size="Medium" />
                                                                        <asp:BoundField DataField="Address" HeaderText="Address"
                                                                            SortExpression="Address"
                                                                            ItemStyle-Font-Size="Medium" />
                                                                        <asp:BoundField DataField="Email" HeaderText="Email"
                                                                            SortExpression="Email"
                                                                            ItemStyle-Font-Size="Medium" />
                                                                        <asp:BoundField DataField="TelNo" HeaderText="Contact No"
                                                                            SortExpression="TelNo"
                                                                            ItemStyle-Font-Size="Medium" />
                                                                        <asp:TemplateField
                                                                            HeaderStyle-HorizontalAlign="Center"
                                                                            HeaderText="Action">
                                                                            <ItemTemplate>
                                                                                <%--<button runat="server" id="gvShare" type="button"
                                                                            class="btn btn-default btn-success" data-toggle="modal"
                                                                            data-target="#modal-AddSharing">
                                                                            <i class="fa fa-edit"></i>
                                                                        </button>--%>
                                                                                <asp:LinkButton runat="server" ID="btnRefUpdate" type="button"
                                                                                    class="btn btn-default btn-success" OnClick="btnRefUpdateShow_Click" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>">
                                                                                    <i class="fa fa-edit"></i>
                                                                                </asp:LinkButton>
                                                                                <asp:LinkButton runat="server" ID="btnDeleteBank" type="button"
                                                                                    class="btn btn-default btn-danger"
                                                                                    OnClick="btnDeleteRef_Click"
                                                                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>">
                                                                                    <i class="fa fa-trash"></i>
                                                                                </asp:LinkButton>
                                                                                <%--<button runat="server" id="Button2" type="button"
                                                                            class="btn btn-default btn-danger"
                                                                            data-toggle="modal"
                                                                            data-target="#modal-AddSharing">
                                                                            <i class="fa fa-trash"></i>
                                                                        </button>--%>
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


                                                                            <%--2023-06-20 : TAGGING FOR REQUIRED DOCUMENTS--%>
                                                                            <%--<HeaderStyle BackColor="#66ccff" />--%>
                                                                            <HeaderStyle BackColor="#ff9999" />
                                                                            <Columns>


                                                                                <%--2023-06-20 : TAGGING FOR REQUIRED DOCUMENTS--%>
                                                                                <%--<asp:BoundField DataField="DocumentName" HeaderText="Document Name" SortExpression="Document Name" ItemStyle-Font-Size="Medium" HeaderStyle-Width="30%" />--%>
                                                                                <asp:BoundField DataField="DocumentName" HeaderText="Required Document Name *" SortExpression="Document Name" ItemStyle-Font-Size="Medium" HeaderStyle-Width="30%" />

                                                                                <%--  <asp:TemplateField HeaderText="Document Name" SortExpression="DocumentName">
                                                                                    <ItemTemplate>
                                                                                        <%# GetFormattedDocumentName(Container.DataItem) %>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle Font-Size="Medium" Width="30%" />
                                                                                </asp:TemplateField>--%>

                                                                                <asp:TemplateField HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" HeaderText="Preview">
                                                                                    <ItemTemplate>
                                                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                                                            <ContentTemplate>
                                                                                                <div class="form-group  center-block text-center ">
                                                                                                    <%--<asp:FileUpload ID="FileUpload2" runat="server" onChange="UploadFile(this);" />--%>

                                                                                                    <div class="col-lg-12 center-block center-block">
                                                                                                        <asp:Label runat="server" ID="lblFileName" Text=""></asp:Label>
                                                                                                        <asp:FileUpload ID="FileUpload1" CssClass="btn btn-default" CommandName="FileUpload1" runat="server" EnableViewState="true" />

                                                                                                        <%--2023-04-28 : 1ST AND 2ND ID ARE REQUIRED--%>
                                                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorAttachments" runat="server"
                                                                                                            ControlToValidate="FileUpload1"
                                                                                                            CssClass="col-md-12"
                                                                                                            ValidationGroup="SaveBuyer2" Style="color: red" ErrorMessage="Please upload your required attachment.">
                                                                                                        </asp:RequiredFieldValidator>
                                                                                                    </div>

                                                                                                    <div class="col-lg-12 center-block center-block">
                                                                                                        <asp:LinkButton ID="btnUpload" CssClass="btn btn-primary" runat="server" CommandName="Upload" Text="Upload" />
                                                                                                        <asp:LinkButton ID="btnPreview" CssClass="btn btn-info" Visible="false" runat="server" CommandName="Preview" Text="Preview" />
                                                                                                        <asp:LinkButton ID="btnRemove" CssClass="btn btn-danger" Visible="false" runat="server" CommandName="Remove" Text="Remove" />
                                                                                                    </div>
                                                                                                </div>
                                                                                            </ContentTemplate>
                                                                                            <Triggers>
                                                                                                <asp:PostBackTrigger ControlID="btnUpload" />
                                                                                                <asp:PostBackTrigger ControlID="btnPreview" />
                                                                                                <asp:PostBackTrigger ControlID="btnRemove" />
                                                                                            </Triggers>
                                                                                        </asp:UpdatePanel>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
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

                                                                            <%--2023-06-20 : TAGGING FOR REQUIRED DOCUMENTS--%>
                                                                            <%--<HeaderStyle />--%>
                                                                            <HeaderStyle BackColor="#66ccff" />

                                                                            <Columns>

                                                                                <%--2023-06-20 : TAGGING FOR REQUIRED DOCUMENTS--%>
                                                                                <%--<asp:BoundField DataField="DocumentName" SortExpression="Document Name" ItemStyle-Font-Size="Medium" HeaderStyle-Width="30%" />--%>
                                                                                <asp:BoundField DataField="DocumentName" HeaderText="Document Name" SortExpression="Document Name" ItemStyle-Font-Size="Medium" HeaderStyle-Width="30%" />

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














                                                                <%--  <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
                                                                    <div class="row">
                                                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 center-block">
                                                                            <h5 style="text-align: center;"><b>Valid ID 1</b></h5>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-lg-4 col-md-4 col-sm-4 col-xs-4">
                                                                                &nbsp;
                                                                            </div>
                                                                            <div class="col-lg-4 col-md-4 col-sm-4 col-xs-4">
                                                                                <div class="input-group">
                                                                                    <asp:FileUpload ID="FileUploadValid1" CssClass="btn btn-default" CommandName="FileUploadValid1" runat="server" EnableViewState="true" />
                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator46" runat="server"
                                                                                        ControlToValidate="FileUploadValid1"
                                                                                        CssClass="col-md-12"
                                                                                        ValidationGroup="SaveBuyer2" Style="color: red" ErrorMessage="Please uploading your Valid ID 1.">
                                                                                    </asp:RequiredFieldValidator>
                                                                                </div>
                                                                                <div class="form-group  center-block text-center">
                                                                                    <asp:LinkButton ID="LinkButton1" CssClass="btn btn-primary" runat="server" CommandName="Upload" Text="Upload" />
                                                                                    <asp:LinkButton ID="LinkButton2" CssClass="btn btn-info" Visible="false" runat="server" CommandName="Preview" Text="Preview" />
                                                                                    <asp:LinkButton ID="LinkButton3" CssClass="btn btn-danger" Visible="false" runat="server" CommandName="Remove" Text="Remove" />
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-lg-4 col-md-4 col-sm-4 col-xs-4">
                                                                                &nbsp;
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 center-block">
                                                                            <h5 style="text-align: center;"><b>Valid ID 1</b></h5>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-lg-4 col-md-4 col-sm-4 col-xs-4">
                                                                                &nbsp;
                                                                            </div>
                                                                            <div class="col-lg-4 col-md-4 col-sm-4 col-xs-4">
                                                                                <div class="input-group">
                                                                                    <asp:FileUpload ID="FileUploadValid2" CssClass="btn btn-default" CommandName="FileUploadValid2" runat="server" EnableViewState="true" />
                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator47" runat="server"
                                                                                        ControlToValidate="FileUploadValid2"
                                                                                        CssClass="col-md-12"
                                                                                        ValidationGroup="SaveBuyer2" Style="color: red" ErrorMessage="Please uploading your Valid ID 2.">
                                                                                    </asp:RequiredFieldValidator>
                                                                                </div>
                                                                                <div class="form-group  center-block text-center">
                                                                                    <asp:LinkButton ID="LinkButton4" CssClass="btn btn-primary" runat="server" CommandName="Upload" Text="Upload" />
                                                                                    <asp:LinkButton ID="LinkButton5" CssClass="btn btn-info" Visible="false" runat="server" CommandName="Preview" Text="Preview" />
                                                                                    <asp:LinkButton ID="LinkButton6" CssClass="btn btn-danger" Visible="false" runat="server" CommandName="Remove" Text="Remove" />
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-lg-4 col-md-4 col-sm-4 col-xs-4">
                                                                                &nbsp;
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>--%>
                                                            </div>
                                                        </div>
                                                        <div class="box box-primary">
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <div runat="server" class="tab-pane" id="tab_2">
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
                                                                                <asp:TextBox runat="server" ID="txtSPOLastName" MaxLength="50" placeholder="Dela Cruz" CssClass="form-control text-uppercase"></asp:TextBox>
                                                                            </div>
                                                                            <asp:RequiredFieldValidator
                                                                                ID="RequiredFieldValidator10"
                                                                                ControlToValidate="txtSPOLastName" CssClass="col-md-12"
                                                                                ErrorMessage="Please fill up Spouse Last Name."
                                                                                SetFocusOnError="true"
                                                                                ValidationGroup="SaveBuyer2"
                                                                                runat="server" Style="color: red" />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <div class="form-group">
                                                                                <label>First Name <span class="color-red fsize-16">*</span></label>
                                                                                <asp:TextBox runat="server" MaxLength="50" ID="txtSPOFirstName" placeholder="Juan" CssClass="form-control text-uppercase"></asp:TextBox>
                                                                            </div>
                                                                            <asp:RequiredFieldValidator
                                                                                ID="RequiredFieldValidator8"
                                                                                ControlToValidate="txtSPOFirstName" CssClass="col-md-12"
                                                                                ErrorMessage="Please fill Spouse First Name."
                                                                                ValidationGroup="SaveBuyer2"
                                                                                runat="server" Style="color: red" />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <div class="form-group">
                                                                                <label>Middle Name <span class="color-red fsize-16">*</span></label>
                                                                                <asp:TextBox runat="server" MaxLength="50" ID="txtSPOMiddleName" placeholder="Quintos" CssClass="form-control text-uppercase"></asp:TextBox>
                                                                            </div>
                                                                            <asp:RequiredFieldValidator
                                                                                ID="RequiredFieldValidator9"
                                                                                ControlToValidate="txtSPOMiddleName" CssClass="col-md-12"
                                                                                ErrorMessage="Please fill Spouse Middle Name."
                                                                                ValidationGroup="SaveBuyer2"
                                                                                runat="server" Style="color: red" />
                                                                        </div>





                                                                        <div class="col-md-6">
                                                                            <div class="form-group">
                                                                                <label>Employment Status <span class="color-red fsize-16">*</span> </label>
                                                                                <asp:DropDownList ID="ddSPOEmpStat" AutoPostBack="true" OnSelectedIndexChanged="ddSPOEmpStat_SelectedIndexChanged" runat="server" CssClass="form-control">
                                                                                    <asp:ListItem Selected="True"></asp:ListItem>
                                                                                    <%--<asp:ListItem Value="---Select Employment Status---" ></asp:ListItem>--%>
                                                                                    <asp:ListItem Value="ES1" Text="Casual"> </asp:ListItem>
                                                                                    <asp:ListItem Value="ES2" Text="Contractual"> </asp:ListItem>
                                                                                    <asp:ListItem Value="ES3" Text="Probationary"> </asp:ListItem>
                                                                                    <asp:ListItem Value="ES4" Text="Permanent"> </asp:ListItem>
                                                                                    <asp:ListItem Value="ES5" Text="Consultant"> </asp:ListItem>
                                                                                    <%-- 2023-06-14 : ADDED SELF EMPLOYED AS REQUESTED  --%>
                                                                                    <asp:ListItem Value="ES6" Text="Self-Employed"> </asp:ListItem>
                                                                                    <asp:ListItem Value="OTH" Text="Others"> </asp:ListItem>
                                                                                </asp:DropDownList>

                                                                                <%--COMMENTED 06 - 13 - 2023 : CHANGE REQUEST  https://direcbti.monday.com/boards/4462005457/pulses/4576973637?asset_id=908456598  --%>
                                                                                <asp:RequiredFieldValidator
                                                                                    ID="RequiredFieldValidator11"
                                                                                    ControlToValidate="ddSPOEmpStat"
                                                                                    ErrorMessage="Please select Employment Status."
                                                                                    SetFocusOnError="true"
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
                                                                                <asp:TextBox runat="server" MaxLength="250" ID="txtSPOAddress" placeholder="4th Floor Victoria One Bldg, South Triangle, Quezon City" CssClass="form-control text-uppercase"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <%--                                                                        <div class="col-md-12">
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
                                                                                <asp:TextBox runat="server" class="form-control" type="date" ID="dtSPOBirthDate" />
                                                                            </div>
                                                                            <asp:CustomValidator
                                                                                Style="color: red"
                                                                                runat="server"
                                                                                ID="CustomValidator5"
                                                                                CssClass="col-md-12"
                                                                                ValidationGroup="SaveBuyer2"
                                                                                ControlToValidate="dtSPOBirthDate"
                                                                                ErrorMessage="Below legal age."
                                                                                OnServerValidate="CustomValidator5_ServerValidate" />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <div class="form-group">
                                                                                <label>
                                                                                    Place of Birth
                                                                                </label>
                                                                                <asp:TextBox runat="server" MaxLength="50" ID="txtSPOBirthPlace" placeholder="Manila" CssClass="form-control text-uppercase"></asp:TextBox>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-md-6">
                                                                            <div class="form-group">
                                                                                <label>Gender</label>
                                                                                <asp:DropDownList ID="ddSPOGender" runat="server" CssClass="form-control">
                                                                                    <asp:ListItem></asp:ListItem>
                                                                                    <asp:ListItem Value="---Select Gender---" Selected="True"></asp:ListItem>
                                                                                    <asp:ListItem Value="M" Text="Male"> </asp:ListItem>
                                                                                    <asp:ListItem Value="F" Text="Female"> </asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <div class="form-group">

                                                                                <label>Citizenship</label>
                                                                                <%--<asp:TextBox runat="server" ID="txtSPOCitizenShip" placeholder="Fil" CssClass="form-control text-uppercase"></asp:TextBox>--%>
                                                                                <select runat="server" id="txtSPOCitizenShip" name="nationality" class="form-control text-uppercase">
                                                                                    <option value="filipino">Filipino</option>
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
                                                                                    <%--<option value="filipino">Filipino</option>--%>
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
                                                                                <asp:TextBox MaxLength="50" runat="server" ID="txtSPOEmail" placeholder="Juan@gmail.com" CssClass="form-control"></asp:TextBox>
                                                                            </div>
                                                                        </div>

                                                                        <%--                                                                        <div class="col-md-6">
                                                                            <div class="form-group">
                                                                                <label>Home Tel No.</label>
                                                                                <asp:TextBox runat="server" ID="txtSPOTelNo" placeholder="123-45-6789" CssClass="form-control"></asp:TextBox>
                                                                            </div>
                                                                        </div>--%>
                                                                        <div class="col-md-6">
                                                                            <div class="form-group">
                                                                                <label>Cellphone No.</label>
                                                                                <asp:TextBox runat="server" ID="txtSPOMobile" MaxLength="13" placeholder="09171231234" CssClass="form-control"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <div class="form-group">
                                                                                <label>Facebook Account</label>
                                                                                <asp:TextBox runat="server" MaxLength="50" ID="txtSPOFB" placeholder="Juan@gmail.com" CssClass="form-control"></asp:TextBox>
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
                                                                                    <asp:DropDownList ID="ddSPONatureEmp" runat="server" CssClass="form-control">
                                                                                        <asp:ListItem Selected="True"></asp:ListItem>
                                                                                        <%--<asp:ListItem Value="---Select Nature of Employment---" Selected="True"></asp:ListItem>--%>
                                                                                        <asp:ListItem Value="NE1" Text="Private Sector"> </asp:ListItem>
                                                                                        <%--                                                                                    <asp:ListItem Value="NE2" Text="Self Employed"> </asp:ListItem>--%>
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
                                                                                    <asp:TextBox ID="txtSPOBusName" MaxLength="100" runat="server" class="form-control text-uppercase" type="text" placeholder="Ayala Land" />
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <div class="form-group">
                                                                                    <label>Business Address</label>
                                                                                    <asp:TextBox ID="txtSPOBusAdd" MaxLength="150" runat="server" class="form-control text-uppercase" type="text" placeholder="101 Tri-State Area" />
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
                                                                                    <asp:TextBox ID="txtSPOPosition" MaxLength="150" runat="server" class="form-control text-uppercase" type="text" placeholder="Broker" />
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <div class="form-group">
                                                                                    <label>Years in Service</label>
                                                                                    <asp:TextBox ID="txtSPOYearsService" MaxLength="5" runat="server" onkeypress="return isNumberKey(event)" class="form-control" type="text" placeholder="2" />
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <div class="form-group">
                                                                                    <label>Office Tel No.</label>
                                                                                    <asp:TextBox ID="txtSPOOfcTelNo" runat="server" MaxLength="13" class="form-control" type="text" placeholder="123-45-6789" />
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <div class="form-group">
                                                                                    <label>Fax No.</label>
                                                                                    <asp:TextBox ID="txtSPOFaxNo" runat="server" MaxLength="14" class="form-control" type="text" placeholder="09171231234" />
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <div class="form-group">
                                                                                    <label>TIN No. <span class="color-red fsize-16">*</span></label>
                                                                                    <asp:TextBox ID="txtSPOTinNo" MaxLength="15" onkeypress="return fnAllowNumeric(event)" onkeyup="TIN_keyup(event);"
                                                                                        runat="server" class="form-control" type="text" placeholder="xxx-xxx-xxx-xxx" ValidationGroup="SaveBuyer2" />
                                                                                    <%--<asp:TextBox ID="txtSPOTinNo" MaxLength="15" onkeyup="TIN_keyup(event);" onkeypress="return fnAllowNumeric(event)" runat="server" class="form-control" type="text" placeholder="xxx-xxx-xxx-xxx" />--%>

                                                                                    <%--COMMENTED 06 - 13 - 2023 : CHANGE REQUEST  https://direcbti.monday.com/boards/4462005457/pulses/4576973637?asset_id=908456598  --%>
                                                                                    <asp:RequiredFieldValidator
                                                                                        ID="RequiredFieldValidator13"
                                                                                        ControlToValidate="txtSPOTinNo"
                                                                                        ErrorMessage="Please fill up Spouse TIN."
                                                                                        SetFocusOnError="true"
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
                                                                                        ControlToValidate="txtSPOTinNo"
                                                                                        ErrorMessage="Invalid Spouse TIN format. Must be 000-000-000-000"
                                                                                        SetFocusOnError="true"
                                                                                        Text="Invalid Spouse TIN format. Must be 000-000-000-000"
                                                                                        OnServerValidate="CustomValidator14_ServerValidate" />

                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <div class="form-group">
                                                                                    <label>SSS No.</label>
                                                                                    <asp:TextBox ID="txtSPOSSSNo" runat="server" MaxLength="12" class="form-control" type="text" placeholder="12-123456-7" />
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <div class="form-group">
                                                                                    <label>GSIS No.</label>
                                                                                    <asp:TextBox ID="txtSPOGSIS" runat="server" MaxLength="14" class="form-control" type="text" placeholder="12-123456-7" />
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <div class="form-group">
                                                                                    <label>PAG-IBIG No.</label>
                                                                                    <asp:TextBox ID="txtSPOPagibi" runat="server" MaxLength="14" class="form-control" type="text" placeholder="12-123456-7" />
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
                                        <div class="tab-pane" id="tab_3">
                                            <div class="content">
                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>




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
                                                                                                    <asp:RadioButton runat="server" CssClass="radio" Text="Owned" ID="tSPAOwned" OnCheckedChanged="tRented_CheckedChanged" AutoPostBack="true" GroupName="SPAHomeOwnership" Checked="true" />
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col-lg-2 col-md-2 col-sm-6 col-xs-12">
                                                                                            <div class="row">
                                                                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                                                    <%--<input runat="server" type="radio" class="SPAHomeOwnership" name="SPAHomeOwnership" value="Mortgaged" id="tSPAMortgaged" />
                                                                                            Mortgaged--%>
                                                                                                    <asp:RadioButton runat="server" CssClass="radio" Text="Mortgaged" ID="tSPAMortgaged" OnCheckedChanged="tRented_CheckedChanged" AutoPostBack="true" GroupName="SPAHomeOwnership" />
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                                                                                            <div class="row">
                                                                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                                                    <%-- <input runat="server" type="radio" class="SPAHomeOwnership" name="SPAHomeOwnership" value="LivingwRelatives" id="tSPALivingwRelatives" />
                                                                                            Living w/ Relatives--%>
                                                                                                    <asp:RadioButton runat="server" CssClass="radio" Text="Living w/ Relatives" ID="tSPALivingwRelatives" OnCheckedChanged="tRented_CheckedChanged" AutoPostBack="true" GroupName="SPAHomeOwnership" />
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col-lg-5 col-md-5 col-sm-6 col-xs-12">
                                                                                            <div class="row">
                                                                                                <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                                                                                    <%--<input runat="server" type="radio" class="SPAHomeOwnership" name="SPAHomeOwnership" value="Rented" id="tSPARented" />
                                                                                            Rented--%>
                                                                                                    <asp:RadioButton runat="server" CssClass="radio" Text="Rented" ID="tSPARented" OnCheckedChanged="tRented_CheckedChanged" AutoPostBack="true" GroupName="SPAHomeOwnership" />
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
                                                                <asp:UpdatePanel runat="server">
                                                                    <ContentTemplate>
                                                                        <div class="panel panel-info">
                                                                            <div class="panel-heading">
                                                                                <h3 class="panel-title" style="text-align: center;">List of SPA</h3>
                                                                            </div>
                                                                            <div class="panel-body">
                                                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                                    <div class="row">
                                                                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                                            <asp:UpdatePanel runat="server">
                                                                                                <ContentTemplate>
                                                                                                    <button class="btn btn-default btn-primary pull-left" type="button" style="color: white;" data-toggle="modal" data-target="#MsgSPACoBorrower" onserverclick="btnSPACoBorrowerModal_ServerClick" runat="server" id="btnSPACoBorrowerModal">Add SPA <i class="fa fa-plus-square"></i></button>
                                                                                                </ContentTemplate>
                                                                                            </asp:UpdatePanel>
                                                                                        </div>
                                                                                    </div>
                                                                                    <br />
                                                                                    <div class="row">
                                                                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                                            <asp:UpdatePanel runat="server">
                                                                                                <ContentTemplate>
                                                                                                    <fieldset>
                                                                                                        <asp:GridView ID="gvSPACoBorrower" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                                                                                            CssClass="table table-bordered table-hover" Width="100%" ShowHeader="True" OnPageIndexChanging="gvSPACoBorrower_PageIndexChanging">
                                                                                                            <HeaderStyle BackColor="#66ccff" />
                                                                                                            <Columns>
                                                                                                                <asp:BoundField DataField="Id" HeaderText="ID" SortExpression="ID" ItemStyle-Font-Size="Medium" />
                                                                                                                <asp:BoundField DataField="Relationship" HeaderText="Relationship" SortExpression="Relationship" ItemStyle-Font-Size="Medium" />
                                                                                                                <asp:BoundField DataField="LastName" HeaderText="LastName" SortExpression="LastName" ItemStyle-Font-Size="Medium" />
                                                                                                                <asp:BoundField DataField="FirstName" HeaderText="FirstName" SortExpression="FirstName" ItemStyle-Font-Size="Medium" />
                                                                                                                <asp:BoundField DataField="MiddleName" HeaderText="MiddleName" SortExpression="MiddleName" ItemStyle-Font-Size="Medium" />
                                                                                                                <asp:BoundField DataField="CivilStatus" HeaderText="CivilStatus" SortExpression="CivilStatus" ItemStyle-Font-Size="Medium" />
                                                                                                                <asp:BoundField DataField="YearsOfStay" HeaderText="YearsOfStay" SortExpression="YearsOfStay" ItemStyle-Font-Size="Medium" />
                                                                                                                <asp:BoundField DataField="Address" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" HeaderText="Address" SortExpression="Address" ItemStyle-Font-Size="Medium" />
                                                                                                                <asp:BoundField DataField="BirthDate" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" HeaderText="BirthDate" SortExpression="BirthDate" ItemStyle-Font-Size="Medium" />
                                                                                                                <asp:BoundField DataField="BirthPlace" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" HeaderText="BirthPlace" SortExpression="BirthPlace" ItemStyle-Font-Size="Medium" />
                                                                                                                <asp:BoundField DataField="Gender" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" HeaderText="Gender" SortExpression="Gender" ItemStyle-Font-Size="Medium" />
                                                                                                                <asp:BoundField DataField="Citizenship" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" HeaderText="Citizenship" SortExpression="Citizenship" ItemStyle-Font-Size="Medium" />
                                                                                                                <asp:BoundField DataField="Email" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" HeaderText="Email" SortExpression="Email" ItemStyle-Font-Size="Medium" />
                                                                                                                <asp:BoundField DataField="TelNo" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" HeaderText="TelNo" SortExpression="TelNo" ItemStyle-Font-Size="Medium" />
                                                                                                                <asp:BoundField DataField="MobileNo" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" HeaderText="MobileNo" SortExpression="MobileNo" ItemStyle-Font-Size="Medium" />
                                                                                                                <asp:BoundField DataField="FB" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" HeaderText="FB" SortExpression="FB" ItemStyle-Font-Size="Medium" />
                                                                                                                <asp:BoundField DataField="SPAFormDocument" HeaderText="SPAFormDocument" SortExpression="SPAFormDocument" ItemStyle-Font-Size="Medium" />
                                                                                                                <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%--  <asp:LinkButton runat="server" ID="gvSPACBEdit" CssClass="btn btn-default btn-warning" OnClientClick="MsgSPACoBorrower_Show();"
                                                                                                                            CommandArgument='<%# Bind("Id")%>' OnClick="gvSPACBEdit_Click"><i class="fa fa-edit"></i></asp:LinkButton>--%>
                                                                                                                        <asp:LinkButton runat="server" ID="gvSPACBEdit" CssClass="btn btn-default btn-warning" OnClientClick="MsgSPACoBorrower_Show();"
                                                                                                                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" OnClick="gvSPACBEdit_Click"><i class="fa fa-edit"></i></asp:LinkButton>
                                                                                                                    </ItemTemplate>



                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                                                                                    <ItemTemplate>

                                                                                                                        <%--<asp:LinkButton runat="server" ID="LinkButton1" CssClass="btn btn-default btn-danger" CommandArgument='<%# Bind("ID")%>' OnClick="gvSPACBDelete_Click"><i class="fa fa-trash-o"></i></asp:LinkButton>--%>
                                                                                                                        <asp:LinkButton runat="server" ID="gvSPACBDelete" CssClass="btn btn-default btn-danger" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" OnClick="gvSPACBDelete_Click"><i class="fa fa-trash-o"></i></asp:LinkButton>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                            </Columns>
                                                                                                            <PagerStyle CssClass="pagination-ys" />
                                                                                                            <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                                                                                        </asp:GridView>
                                                                                                    </fieldset>
                                                                                                </ContentTemplate>
                                                                                            </asp:UpdatePanel>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
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
                                                                                            <asp:UpdatePanel runat="server">
                                                                                                <ContentTemplate>
                                                                                                    <button class="btn btn-default btn-primary pull-right" type="button" data-toggle="modal" data-target="#MsgDependent" runat="server" id="bSPADependent" onserverclick="bDependent_ServerClick" style="color: white;">Dependent <i class="fa fa-plus-square"></i></button>
                                                                                                </ContentTemplate>
                                                                                            </asp:UpdatePanel>
                                                                                        </div>
                                                                                    </div>
                                                                                    <br />
                                                                                    <div class="row">
                                                                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                                            <asp:UpdatePanel runat="server">
                                                                                                <ContentTemplate>
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
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <div class="col-lg-6 col-md-12 col-sm-12 col-xs-12" style="border-right-color: lightgray; border-right-width: thin; border-right-style: solid;">
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
                                                <input runat="server" style="z-index: auto;" id="tSpecialInstructions" class="form-control text-uppercase" type="text" disabled /><span class="input-group-btn">
                                                    <button id="bSpecialInstructions" runat="server" style="width: 50px; z-index: auto;" class="btn btn-secondary btn-dropbox" type="button" onserverclick="btnEmployment_ServerClick"><i class="fa fa-bars"></i></button>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <div class="col-lg-12  col-md-12 col-sm-12 col-xs-12">
                                                    <div class="form-group">
                                                        <h5>Remarks/Recommendations</h5>
                                                        <textarea runat="server" id="tRemarks" rows="5" class="text-uppercase form-control" style="width: 100%; max-width: 100%; min-width: 100%; height: 50px; max-height: 150px; min-height: 50px;" />
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="text-align: center;">
                                        <asp:CheckBox runat="server" ID="CBconforme" Font-Bold="false" Text="&nbsp; I/We certify that all information furnished herein are true and correct &nbsp;" /><span class="color-red fsize-16">*</span>
                                        <%--<h5 style="text-align: center;">I/We certify that all information furnished herein are true and correct</h5>--%>
                                        <asp:CustomValidator
                                            Style="color: red"
                                            runat="server"
                                            ID="CustomValidator8" CssClass="col-md-12"
                                            ValidationGroup="SaveBuyer2"
                                            Text="Please confirm all your information as true and correct before proceeding."
                                            OnServerValidate="CustomValidator8_ServerValidate" />
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
                                                ID="RequiredFieldValidator38"
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
                                                <input runat="server" type="date" class="form-control text-uppercase" id="txtCertifyDate" readonly />
                                            </div>
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator39"
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
                                    <button class="btn btn-primary nextBtn pull-right" validationgroup="SaveBuyer2" type="button" id="Button4" runat="server" onserverclick="btnNext3_ServerClick">Save All Information</button>
                                </div>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="container-fluid">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:Panel runat="server" class="panel panel-primary" ID="done_step" Visible="false">
                            <div class="panel-heading" style="background-color: #5caceb">
                                <center>
                                    <h3 id="title-success" hidden="hidden" class="panel-title">Adding Successful!</h3>
                                    <h3 id="title-failed" hidden="hidden" class="panel-title">Adding Failed! Please contact your support.</h3>
                                </center>
                            </div>
                            <div class="panel-body">
                                <div class="col-md-12" id="button-print">
                                    <center>
                                        <button type="button" runat="server" id="btnBuyerPrint" class="btn btn-primary btn-sm" onserverclick="btnBuyerPrint_ServerClick">Print</button>
                                    </center>
                                </div>
                                <div class="col-md-12" id="button-errback">
                                    <center>
                                        <button type="button" runat="server" id="btnErrBack" class="btn btn-primary btn-sm" onserverclick="btnNext2_ServerClick">Back</button>
                                    </center>
                                </div>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
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
                                                <h5>Name of Dependent:</h5>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <input runat="server" type="text" class="form-control text-uppercase" id="tDependentName" placeholder="Your Dependent Name Here" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <h5>Age:</h5>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <input runat="server" type="text" onkeypress="return isNumberKey(event)" maxlength="3" class="form-control" id="tDependentAge" value="0" />
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <h5>Relationship:</h5>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <div class="input-group">
                                                    <input runat="server" style="z-index: auto;" id="tDependentRelationship" class="form-control text-uppercase" type="text" disabled /><span class="input-group-btn">
                                                        <button id="bDependentRelationship" runat="server" style="width: 50px; z-index: auto;" class="btn btn-secondary btn-dropbox" type="button" onserverclick="btnEmployment_ServerClick"><i class="fa fa-bars"></i></button>
                                                    </span>
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
                                <button runat="server" id="Button5" style="width: 100px;" class="btn btn-primary" type="button" onserverclick="btnAdd_ServerClick">Add</button>
                                <button runat="server" style="width: 100px;" class="btn btn-default" type="button" data-dismiss="modal">Cancel </button>
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
                                                        <input runat="server" type="text" class="form-control" id="tCoEmail" placeholder="Your Email Here" />
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
                                        <button runat="server" id="btnAddCoOwner" style="width: 100px;" class="btn btn-primary" type="button" validationgroup="addcoowner" onserverclick="btnAddCoOwner_ServerClick">Add</button>
                                        <button runat="server" style="width: 100px;" class="btn btn-default" type="button" data-dismiss="modal">Cancel </button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <div class="modal fade" id="MsgEmployment" role="dialog" tabindex="-1" style="z-index: 9999;">
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
                                                Width="100%">
                                                <HeaderStyle BackColor="#66ccff" />
                                                <Columns>
                                                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-Font-Size="Medium" />
                                                    <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:LinkButton runat="server" ID="btnSelectEmployment" CssClass="btn btn-default btn-success" Style="color: white;" Width="100%" Height="100%" OnClick="btnSelectEmployment_Click" CommandArgument='<%# Bind("Code")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
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
                                <button class="btn btn-default btn-facebook" type="button" style="width: 100px;" data-dismiss="modal">OK</button>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="modal" id="MsgSPACoBorrower" role="dialog" tabindex="-1" style="overflow-y: auto;">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header bg-color">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                        <%--<h4 class="modal-title">Add SPA/Co-Borrower</h4>--%>
                        <h4 class="modal-title" id="spaTitle" runat="server">Add SPA</h4>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-lg-2 col-md-2 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <asp:Button runat="server" ID="btnCopySPA" Visible="false" CssClass="btn btn-info btn-sm titlemargin" Text="Copy Spouse Details" OnClick="btnCopySPA_Click" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">

                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-header">
                                                <h3 class="box-title">SPA Details</h3>
                                            </div>
                                            <div class="row hidden">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <asp:CustomValidator
                                                        Style="color: red"
                                                        runat="server"
                                                        ID="CustomValidator3"
                                                        ValidationGroup="addspa"
                                                        enable="false"
                                                        Text="Please tick SPA &/or Co-Borrower check box."
                                                        OnServerValidate="CustomValidator3_ServerValidate" />
                                                </div>
                                            </div>


                                            <div class="hidden">
                                                <div class="col-lg-2 col-md-2 col-sm-6 col-xs-12">
                                                    <div class="row">
                                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                            <asp:RadioButton AutoPostBack="true" runat="server" type="radio" Checked="true" GroupName="SPA" value="SPA" ID="tSPA" OnCheckedChanged="tSPA_CheckedChanged" />
                                                            SPA
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-lg-2 col-md-2 col-sm-6 col-xs-12">
                                                    <div class="row">
                                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                            <asp:RadioButton AutoPostBack="true" OnCheckedChanged="tSPA_CheckedChanged" runat="server" type="radio" GroupName="SPA" value="CoBorrower" ID="tCoBorrower" />
                                                            Co-Borrower
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>



                                            <%--<div class="col-lg-2 col-md-2 col-sm-6 col-xs-12">--%>

                                            <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                <div class="row">
                                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" id="vCoBorrower">
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
                                                                        <button id="bCoBorrower" runat="server" style="width: 50px; z-index: auto;" onserverclick="btnEmployment_ServerClick" class="btn btn-secondary btn-dropbox" type="button"><i class="fa fa-bars"></i></button>
                                                                    </span>
                                                                </div>
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator29"
                                                                    ControlToValidate="tRelationship"
                                                                    Text="Please choose Relationship."
                                                                    ValidationGroup="addspa"
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
                                                        <asp:TextBox runat="server" ID="txtSPALastName" placeholder="Dela Cruz" CssClass="form-control text-uppercase"></asp:TextBox>
                                                    </div>
                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator30"
                                                        ControlToValidate="txtSPALastName"
                                                        Text="Please choose Last Name."
                                                        ValidationGroup="addspa"
                                                        runat="server" Style="color: red" />
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>First Name <span class="color-red fsize-16">*</span></label>
                                                        <asp:TextBox runat="server" ID="txtSPAFirstName" placeholder="Juan" CssClass="form-control text-uppercase"></asp:TextBox>
                                                    </div>
                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator31"
                                                        ControlToValidate="txtSPAFirstName"
                                                        Text="Please choose First Name."
                                                        ValidationGroup="addspa"
                                                        runat="server" Style="color: red" />
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Middle Name <span class="color-red fsize-16">*</span></label>
                                                        <asp:TextBox runat="server" ID="txtSPAMiddleName" placeholder="Quintos" CssClass="form-control text-uppercase"></asp:TextBox>
                                                    </div>
                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator32"
                                                        ControlToValidate="txtSPAMiddleName"
                                                        Text="Please choose Middle Name."
                                                        ValidationGroup="addspa"
                                                        runat="server" Style="color: red" />
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Civil Status</label>
                                                        <asp:DropDownList ID="ddSPACivilStatus" runat="server" CssClass="form-control text-uppercase">
                                                            <asp:ListItem Value="Single" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Value="Married" Text="Married"> </asp:ListItem>

                                                            <%-- 2023-06-14 : REMOVED WIDOW AND OTHERS; RETAIN SINGLE AND MARRIED --%>
                                                            <%--     <asp:ListItem Value="Widow" Text="Widow"> </asp:ListItem>
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
                                                        <asp:TextBox runat="server" ID="txtSPAAddress" placeholder="4th Floor Victoria One Bldg, South Triangle, Quezon City" CssClass="form-control text-uppercase"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <label>Date of Birth</label>
                                                    <div class="input-group">
                                                        <div class="input-group-addon">
                                                            <i class="fa fa-calendar"></i>
                                                        </div>
                                                        <asp:TextBox runat="server" class="form-control" type="date" ID="dtSPABirthDate" />
                                                    </div>
                                                    <asp:CustomValidator
                                                        Style="color: red"
                                                        runat="server"
                                                        ID="CustomValidator6"
                                                        CssClass="col-md-12"
                                                        ValidationGroup="addspa"
                                                        ControlToValidate="dtSPABirthDate"
                                                        Text="Below legal age."
                                                        OnServerValidate="CustomValidator6_ServerValidate" />
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Place of Birth</label>
                                                        <asp:TextBox runat="server" ID="txtSPABirthPlace" placeholder="Manila" CssClass="form-control text-uppercase"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">

                                                    <div class="form-group">
                                                        <label>Gender <span class="color-red fsize-16">*</span></label>
                                                        <asp:DropDownList ID="ddSPAGender" runat="server" CssClass="form-control">
                                                            <asp:ListItem></asp:ListItem>
                                                            <asp:ListItem Value="---Select Gender---" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Value="M" Text="Male"> </asp:ListItem>
                                                            <asp:ListItem Value="F" Text="Female"> </asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator12"
                                                            ControlToValidate="ddSPAGender"
                                                            InitialValue="---Select Gender---"
                                                            Text="Please select gender."
                                                            ValidationGroup="addspa"
                                                            runat="server" Style="color: red" />
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Citizenship <span class="color-red fsize-16">*</span></label>
                                                        <%--<asp:TextBox runat="server" ID="txtSPACitizenship" placeholder="Fil" CssClass="form-control text-uppercase"></asp:TextBox>--%>
                                                        <select runat="server" id="txtSPACitizenship" name="nationality" class="form-control text-uppercase">
                                                            <option value="filipino">Filipino</option>
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
                                                            <%--<option value="filipino">Filipino</option>--%>
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
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator15"
                                                            ControlToValidate="txtSPACitizenship"
                                                            Text="Please select citizenship."
                                                            ValidationGroup="addspa"
                                                            runat="server" Style="color: red" />
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Email Address</label>
                                                        <asp:TextBox runat="server" ID="txtSPAEmail" placeholder="Juan@gmail.com" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Home Tel No.</label>
                                                        <asp:TextBox runat="server" ID="txtSPATelNo" placeholder="123-45-6789" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Cellphone No.</label>
                                                        <asp:TextBox runat="server" ID="txtSPAMobile" placeholder="09171231234" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Facebook Account</label>
                                                        <asp:TextBox runat="server" ID="txtSPAFB" placeholder="Juan@gmail.com" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="col-md-6">
                                                    <div class="col-md-12">

                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <div class="col-md-12" runat="server" id="divUploadSPADocs">
                                                                    <label>SPA Form document.</label>
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








































































































































































































































































































































                                    <div class="col-md-6 hidden">
                                        <div class="box box-primary">
                                            <div class="box-header">
                                                <h3 class="box-title">Co Borrower Business Details</h3>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Employment Status <span class="color-red fsize-16">*</span></label>
                                                        <asp:DropDownList ID="ddSPAEmpStat" runat="server" CssClass="form-control">
                                                            <asp:ListItem></asp:ListItem>
                                                            <asp:ListItem Value="---Select Employment Status---" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Value="ES1" Text="Casual"> </asp:ListItem>
                                                            <asp:ListItem Value="ES2" Text="Contractual"> </asp:ListItem>
                                                            <asp:ListItem Value="ES3" Text="Probationary"> </asp:ListItem>
                                                            <asp:ListItem Value="ES4" Text="Permanent"> </asp:ListItem>
                                                            <asp:ListItem Value="ES5" Text="Consultant"> </asp:ListItem>
                                                            <asp:ListItem Value="OTH" Text="Others"> </asp:ListItem>
                                                        </asp:DropDownList>
                                                        <%--<asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator13"
                                                            ControlToValidate="ddSPAEmpStat"
                                                            InitialValue="---Select Employment Status---"
                                                            Text="Please select Employment Status."
                                                            ValidationGroup="addspa"
                                                            runat="server" Style="color: red" />--%>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Nature of Employment <span class="color-red fsize-16">*</span></label>
                                                        <asp:DropDownList ID="ddSPANatureEmp" runat="server" CssClass="form-control">
                                                            <asp:ListItem></asp:ListItem>
                                                            <asp:ListItem Value="---Select Nature of Employment---" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Value="NE1" Text="Private Sector"> </asp:ListItem>
                                                            <%--<asp:ListItem Value="NE2" Text="Self Employed"> </asp:ListItem>--%>
                                                            <asp:ListItem Value="NE3" Text="Government"> </asp:ListItem>
                                                            <asp:ListItem Value="NE4" Text="Retired"> </asp:ListItem>
                                                            <asp:ListItem Value="NE5" Text="Overseas"> </asp:ListItem>
                                                            <asp:ListItem Value="OTH" Text="Others"> </asp:ListItem>
                                                        </asp:DropDownList>
                                                        <%--<asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator14"
                                                            ControlToValidate="ddSPANatureEmp"
                                                            InitialValue="---Select Nature of Employment---"
                                                            Text="Please select Nature of Employment."
                                                            ValidationGroup="addspa"
                                                            runat="server" Style="color: red" />--%>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Business Name</label>
                                                        <asp:TextBox ID="txtSPABusName" runat="server" class="form-control text-uppercase" type="text" placeholder="Ayala Land" />
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Business Address</label>
                                                        <asp:TextBox ID="txtSPABusAdd" runat="server" class="form-control text-uppercase" type="text" placeholder="101 Tri-State Area" />
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
                                                        <asp:TextBox ID="txtSPAPosition" runat="server" class="form-control text-uppercase" type="text" placeholder="Broker" />
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Years in Service</label>
                                                        <asp:TextBox ID="txtSPAYearsService" runat="server" class="form-control text-uppercase" onkeypress="return isNumberKey(event)" type="text" placeholder="2" />
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Office Tel No.</label>
                                                        <asp:TextBox ID="txtSPAOfcTelNo" runat="server" class="form-control text-uppercase" type="text" placeholder="123-45-6789" />
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Fax No.</label>
                                                        <asp:TextBox ID="txtSPAFaxNo" runat="server" class="form-control text-uppercase" type="text" placeholder="09171231234" />
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>TIN No. <span class="color-red fsize-16">*</span></label>
                                                        <asp:TextBox ID="txtSPATinNo" MaxLength="15" runat="server" onkeypress="return fnAllowNumeric(event)" class="form-control text-uppercase" type="text" placeholder="xxx-xxx-xxx-xxx" />
                                                        <%--<asp:TextBox ID="txtSPATinNo" MaxLength="15" runat="server" onkeyup="TIN_keyup(event);" onkeypress="return fnAllowNumeric(event)" class="form-control text-uppercase" type="text" placeholder="xxx-xxx-xxx-xxx" />--%>
                                                    </div>
                                                    <%--<asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator33"
                                                        ControlToValidate="txtSPATinNo"
                                                        Text="Please choose TIN."
                                                        ValidationGroup="addspa"
                                                        runat="server" Style="color: red" />--%>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>SSS No.</label>
                                                        <asp:TextBox ID="txtSPASSSNo" runat="server" class="form-control text-uppercase" type="text" placeholder="12-123456-7" />
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>GSIS No.</label>
                                                        <asp:TextBox ID="txtSPAGSIS" runat="server" class="form-control text-uppercase" type="text" placeholder="12-123456-7" />
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>PAG-IBIG No.</label>
                                                        <asp:TextBox ID="txtSPAPagibi" runat="server" class="form-control text-uppercase" type="text" placeholder="12-123456-7" />
                                                    </div>
                                                </div>
                                                <br>
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
                                <button validationgroup="addspa" style="color: white;" class="btn btn-default btn-primary pull-right" type="button" runat="server" id="btnSPACoBorrower" onserverclick="btnSPACoBorrower_ServerClick">
                                    <label runat="server" id="SPAAddUpdate" />
                                    SPA <i class="fa fa-plus-square"></i>
                                </button>

                                <button id="btnSPAUpdate" runat="server" onserverclick="btnSPAUpdate_ServerClick" type="button" class="btn btn-primary" visible="false">Update changes</button>

                                <button runat="server" style="width: 100px;" class="btn btn-default pull-left" type="button" data-dismiss="modal">Cancel </button>
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
                                        <button type="button" runat="server" class="btn btn-primary btn-sm" data-dismiss="modal" style="width: 90px;" onserverclick="Reload_ServerClick">Ok</button>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalAlert2" class="modal fade" style="z-index: 9999">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-body">
                        <asp:UpdatePanel runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <div class="row" style="text-align: center;">
                                    <div class="col-lg-12">
                                        <asp:Image ImageUrl="~/assets/img/success.png" runat="server" ID="alert2img" Width="90" Height="90" />
                                    </div>
                                </div>
                                <div class="row" style="text-align: center;">
                                    <div class="col-lg-12">
                                        <h4>
                                            <asp:Label runat="server" ID="alert2lbl"></asp:Label></h4>
                                    </div>
                                </div>
                                <div class="row" style="text-align: center;">
                                    <div class="col-lg-12">
                                        <button type="button" runat="server" class="btn btn-primary btn-sm" data-dismiss="modal" style="width: 90px;">Ok</button>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>

        <%--//Modal for Advance Dependent--%>
        <div class="modal fade" id="modal-default">
            <div class="modal-dialog">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span></button>
                                <h3 class="modal-title">Add Dependent</h3>
                            </div>
                            <div class="modal-body">
                                <div class="form-group">
                                    <asp:TextBox runat="server" ID="mtxtRow" class="form-control text-uppercase hidden" type="text" />
                                </div>
                                <div class="form-group">
                                    <label>Name of Dependent:</label>
                                    <asp:TextBox runat="server" ID="mtxtName" class="form-control text-uppercase" type="text" placeholder="Juan Dela Cruz" />
                                </div>
                                <div class="form-group">
                                    <label>Age:</label>
                                    <asp:TextBox runat="server" ID="mtxtAge" class="form-control text-uppercase" type="number" placeholder="18" />
                                </div>
                                <label>Relationship:</label>
                                <div class="input-group">
                                    <input style="z-index: auto;" runat="server" id="mtxtRelationship" class="form-control text-uppercase" type="text" disabled /><span class="input-group-btn">
                                        <button id="bDependentRelationship2" runat="server" style="width: 50px; z-index: auto;" class="btn btn-primary btn-dropbox" type="button" onserverclick="btnEmployment_ServerClick"><i class="fa fa-bars"></i></button>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button id="btnAdd" runat="server" onserverclick="btnAdd_ServerClick" type="button" class="btn btn-primary">Add</button>
                                <button id="btnUpdate" runat="server" onserverclick="btnUpdate_ServerClick" type="button" class="btn btn-primary" visible="false">Update</button>
                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <%--//modal--%>
        <div class="modal fade" id="modal-default2">
            <div class="modal-dialog">
                <div class="modal-content">
                    <asp:UpdatePanel runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="modal-header">
                                <h4 class="modal-title" runat="server" id="banktitle">Add Bank Account</h4>
                            </div>
                            <div class="modal-body">
                                <asp:TextBox runat="server" ID="mtxtRow2" class="form-control text-uppercase hidden" type="text" />
                                <div class="form-group">
                                    <label>Bank Name:</label>
                                    <asp:TextBox runat="server" ID="mtxtBankName" MaxLength="150" class="form-control text-uppercase" type="text" placeholder="BANCO DE ORO" />
                                </div>
                                <div class="form-group">
                                    <label>Branch:</label>
                                    <asp:TextBox runat="server" ID="mtxtBranch" MaxLength="500" class="form-control text-uppercase" type="text" placeholder="Quezon Ave" />
                                </div>
                                <div class="form-group">
                                    <label>Type of Account:</label>
                                    <div class="input-group">
                                        <asp:TextBox runat="server" Style="z-index: auto" ID="mtxtAccount" class="form-control text-uppercase" type="text" placeholder="TYPE OF ACCOUNT" ReadOnly="true" /><span class="input-group-btn">
                                            <button id="bAccount" runat="server" onserverclick="bTypeofAccount_ServerClick" style="width: 50px; z-index: auto;" class="btn btn-secondary" type="button"><i class="fa fa-bars"></i></button>
                                        </span>
                                    </div>
                                    <%--<asp:TextBox runat="server" ID="mtxtAccount" class="form-control text-uppercase" type="text" placeholder="Savings/Checking"/>--%>
                                </div>
                                <div class="form-group">
                                    <label>Account No.:</label>
                                    <asp:TextBox runat="server" ID="mtxtAccountNo" class="form-control text-uppercase" type="text" placeholder="0000-0000-0000" />
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" CssClass="hidden">Average Daily Balance</asp:Label>
                                    <asp:TextBox runat="server" ID="mtxtAverage" class="form-control hidden" type="number" placeholder="12000" />
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" CssClass="hidden">Present Balance</asp:Label>
                                    <asp:TextBox runat="server" ID="mtxtBalance" class="form-control hidden" type="number" placeholder="25000" />
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" CssClass="hidden">Action</asp:Label>
                                    <asp:TextBox runat="server" ID="mtxtAction" class="form-control hidden" type="text" placeholder="Action" />
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-primary" runat="server" id="btnAddBank" onserverclick="btnAddBank_ServerClick">Add</button>
                                <button type="button" class="btn btn-primary" runat="server" id="btnUpdateBank" onserverclick="btnUpdateBank_ServerClick" visible="false">Update</button>
                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>

        <%--//modal--%>
        <div class="modal fade" id="modal-default3">
            <div class="modal-dialog">
                <div class="modal-content">
                    <asp:UpdatePanel runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="modal-header">
                                <h3 class="modal-title" runat="server" id="reftitle">New Character Reference</h3>
                            </div>
                            <div class="modal-body">
                                <input runat="server" id="mtxtRow3" class="form-control text-uppercase hidden" type="text" />

                                <div class="form-group">
                                    <asp:TextBox runat="server" ID="TextBox1" class="form-control text-uppercase hidden" type="text" />
                                </div>

                                <div class="form-group">
                                    <label>Name:</label>
                                    <input runat="server" id="reftxtName" class="form-control text-uppercase" type="text" placeholder="Juan Dela Cruz" />
                                </div>
                                <div class="form-group">
                                    <label>Address:</label>
                                    <input runat="server" id="reftxtAddress" class="form-control text-uppercase" type="text" placeholder="Ermita, Manila, 1000 Metro Manila" />
                                </div>
                                <div class="form-group">
                                    <label>Email:</label>
                                    <input runat="server" id="reftxtEmail" class="form-control" type="text" placeholder="juandelacruz@mail.com" />
                                </div>
                                <div class="form-group">
                                    <label>Telephone/Cellphone No.:</label>
                                    <input runat="server" id="reftxtContact" class="form-control text-uppercase" type="text" placeholder="0917 123 1234" />
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                                <button type="button" style="width: 100px;" data-dismiss="modal" class="btn btn-primary" runat="server" id="btnAddCharRef" onserverclick="btnAddCharRef_ServerClick">Save changes</button>
                                <button id="btnRefUpdate" runat="server" onserverclick="btnRefUpdate_ServerClick" type="button" class="btn btn-primary" visible="false">Update changes</button>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
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

        <asp:UpdateProgress ID="updateProgress" runat="server">
            <ProgressTemplate>
                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #FFFFFF; opacity: 0.7;">
                    <asp:Image runat="server" ID="loading" ImageUrl="~/assets/img/loader.gif" CssClass="CenterPB" AlternateText="Loading..." ToolTip="Loading ..." Height="254px" Width="254px" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

        <script src="../assets/js/jquery.min.js"></script>
        <script src="../assets/js/bootstrap.min.js"></script>
        <script src="../assets/js/select2.full.min.js"></script>
        <script src="../assets/js/jquery.inputmask.js"></script>
        <script src="../assets/js/jquery.inputmask.date.extensions.js"></script>
        <script src="../assets/js/jquery.inputmask.extensions.js"></script>
        <script src="../assets/js/moment.min.js"></script>
        <script src="../assets/js/daterangepicker.js"></script>
        <script src="../assets/js/bootstrap-datetimepicker.min.js"></script>
        <script src="../assets/js/bootstrap-colorpicker.min.js"></script>
        <script src="../assets/js/bootstrap-timepicker.min.js"></script>
        <script src="../assets/js/jquery.slimscroll.min.js"></script>
        <script src="../assets/js/icheck.min.js"></script>
        <script src="../assets/js/fastclick.js"></script>
        <script src="../assets/js/adminlte.min.js"></script>
        <script src="../assets/js/demo.js"></script>
        <script src="../assets/js/daterangepicker.js"></script>
        <script src="../assets/js/jquery.mask.min.js"></script>


        <script>
            $(function () {
                //Initialize Select2 Elements
                $('.select2').select2()
                //Datemask dd/mm/yyyy
                $('#datemask').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/yyyy' })
                //Datemask2 mm/dd/yyyy
                $('#datemask2').inputmask('mm/dd/yyyy', { 'placeholder': 'mm/dd/yyyy' })
                //Money Euro
                $('[data-mask]').inputmask()
                //Date range picker
                $('#reservation').daterangepicker()
                $('#reservation2').daterangepicker()
                //Date range picker with time picker
                $('#reservationtime').daterangepicker({ timePicker: true, timePickerIncrement: 30, locale: { format: 'MM/DD/YYYY hh:mm A' } })

                //GAB 06/09/2023 TIN Masking
                $('#txtTIN').mask('000-000-000-000');

                //2023-06-16: TIN MASKING
                $('#tTINCorp').mask('000-000-000-000');
                $('#txtSPOTinNo').mask('000-000-000-000');


                //Date range as a button
                $('#daterange-btn').daterangepicker(
                    {
                        ranges: {
                            'Today': [moment(), moment()],
                            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                            'This Month': [moment().startOf('month'), moment().endOf('month')],
                            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
                        },
                        startDate: moment().subtract(29, 'days'),
                        endDate: moment()
                    },
                    function (start, end) {
                        $('#daterange-btn span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'))
                    }
                )

                ////Date picker
                //$('#datepicker').datepicker({
                //    autoclose: false
                //})

                //iCheck for checkbox and radio inputs
                $('input[type="checkbox"].minimal, input[type="radio"].minimal').iCheck({
                    checkboxClass: 'icheckbox_minimal-blue',
                    radioClass: 'iradio_minimal-blue'
                })
                //Red color scheme for iCheck
                $('input[type="checkbox"].minimal-red, input[type="radio"].minimal-red').iCheck({
                    checkboxClass: 'icheckbox_minimal-red',
                    radioClass: 'iradio_minimal-red'
                })
                //Flat red color scheme for iCheck
                $('input[type="checkbox"].flat-red, input[type="radio"].flat-red').iCheck({
                    checkboxClass: 'icheckbox_flat-green',
                    radioClass: 'iradio_flat-green'
                })

                //Colorpicker
                $('.my-colorpicker1').colorpicker()
                //color picker with addon
                $('.my-colorpicker2').colorpicker()

                //Timepicker
                $('.timepicker').timepicker({
                    showInputs: false
                })
                $(document).ready(function () {
                    //$('.stepper').mdbStepper();
                })
            })
        </script>
        <script>
            $(document).ready(function () {

                $(document).on('click', '.rb1', function () {
                    $('.rb1').attr('Checked', true); $('.rb1').prop('Checked', true);
                    $('.rb2').attr('Checked', false); $('.rb2').prop('Checked', false);
                    $('.rb3').attr('Checked', false); $('.rb3').prop('Checked', false);
                    $('.rb4').attr('Checked', false); $('.rb4').prop('Checked', false);
                    $('.rented').attr('enabled', false); $('.rented').prop('enabled', false);
                    $('.rented').attr('disabled', true); $('.rented').prop('disabled', true);
                    console.log('1');
                });

                $(document).on('click', '.rb2', function () {
                    $('.rb1').attr('Checked', false); $('.rb1').prop('Checked', false);
                    $('.rb2').attr('Checked', true); $('.rb2').prop('Checked', true);
                    $('.rb3').attr('Checked', false); $('.rb3').prop('Checked', false);
                    $('.rb4').attr('Checked', false); $('.rb4').prop('Checked', false);
                    $('.rented').attr('enabled', false); $('.rented').prop('enabled', false);
                    $('.rented').attr('disabled', true); $('.rented').prop('disabled', true);
                    console.log('2');
                });

                $(document).on('click', '.rb3', function () {
                    $('.rb1').attr('Checked', false); $('.rb1').prop('Checked', false);
                    $('.rb2').attr('Checked', false); $('.rb2').prop('Checked', false);
                    $('.rb3').attr('Checked', true); $('.rb3').prop('Checked', true);
                    $('.rb4').attr('Checked', false); $('.rb4').prop('Checked', false);
                    $('.rented').attr('enabled', false); $('.rented').prop('enabled', false);
                    $('.rented').attr('disabled', true); $('.rented').prop('disabled', true);
                    console.log('3');
                });

                $(document).on('click', '.rb4', function () {
                    $('.rb1').attr('Checked', false); $('.rb1').prop('Checked', false);
                    $('.rb2').attr('Checked', false); $('.rb2').prop('Checked', false);
                    $('.rb3').attr('Checked', false); $('.rb3').prop('Checked', false);
                    $('.rb4').attr('Checked', true); $('.rb4').prop('Checked', true);
                    $('.rented').attr('enabled', true); $('.rented').prop('enabled', true);
                    $('.rented').attr('disabled', false); $('.rented').prop('disabled', false);
                    console.log('4');
                });



                var navListItems = $('div.setup-panel div a'),
                    allWells = $('.setup-content'),
                    allNextBtn = $('.nextBtn');

                allWells.hide();

                navListItems.click(function (e) {
                    e.preventDefault();
                    var $target = $($(this).attr('href')),
                        $item = $(this);

                    if (!$item.hasClass('disabled')) {
                        navListItems.removeClass('btn-success').addClass('btn-default');
                        $item.addClass('btn-success');
                        allWells.hide();
                        $target.show();
                        $target.find('input:eq(0)').focus();
                    }
                });

                allNextBtn.click(function () {
                    var curStep = $(this).closest(".setup-content"),
                        curStepBtn = curStep.attr("id"),
                        nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().next().children("a"),
                        curInputs = curStep.find("input[type='text'],input[type='url']"),
                        isValid = true;

                    $(".form-group").removeClass("has-error");
                    for (var i = 0; i < curInputs.length; i++) {
                        if (!curInputs[i].validity.valid) {
                            isValid = false;
                            $(curInputs[i]).closest(".form-group").addClass("has-error");
                        }
                    }

                    if (isValid) nextStepWizard.removeAttr('disabled').trigger('click');
                });

                $('div.setup-panel div a.btn-success').trigger('click');
            });
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
        </style>
    </form>
</body>
</html>

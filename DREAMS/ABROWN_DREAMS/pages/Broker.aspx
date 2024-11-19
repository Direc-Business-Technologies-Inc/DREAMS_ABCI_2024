<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Broker.aspx.cs" Inherits="ABROWN_DREAMS.pages.Broker1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>DREAMS Broker Accreditation</title>
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
    <link rel="shortcut icon" href="../assets/img/Icon.ico" />
    <link rel="stylesheet" href="../assets/css/bootstrap3-wysihtml5.min.css" />
    <link rel="stylesheet" href="../assets/css/bootstrap.min.css" />
    <link rel="stylesheet" href="../assets/css/styles.css" />
    <link rel="stylesheet" href="../assets/fonts/font-awesome.min.css" />
    <link rel="stylesheet" href="../assets/css/pagination.css" />
    <link rel="stylesheet" href="../assets/css/custom.css" />
    <link rel="stylesheet" href="../assets/css/wizard.css" />
    <link rel="stylesheet" href="../assets/css/sweetalert.css" />
    <%--<link rel="stylesheet" href="../assets/css/bootstrap-datetimepicker.min.css" />--%>
    <%--<link href="../lib/fontawesome-free-5.3.1-web/css/fontawesome.min.css" rel="stylesheet" />--%>
    <link href="../assets/css/jquery-ui.css" rel="stylesheet" />
    <link rel="stylesheet" href="../assets/css/ionicons.min.css" />
    <link rel="stylesheet" href="../assets/css/daterangepicker.css" />
    <%--<link rel="stylesheet" href="../assets/css/all.css" />--%>
    <link rel="stylesheet" href="../assets/css/smart_wizard.css" />
    <link rel="stylesheet" href="../assets/css/font-awesome.css" />
    <link rel="stylesheet" href="../assets/css/smart_wizard_all.css" />
    <link rel="stylesheet" href="../assets/css/smart_wizard_arrows.css" />
    <link rel="stylesheet" href="../assets/css/smart_wizard_dark.css" />
    <link rel="stylesheet" href="../assets/css/smart_wizard_dots.css" />

    <%--<link rel="stylesheet" href="../assets/css/bootstrap-colorpicker.min.css" />--%>
    <%--<link rel="stylesheet" href="../assets/css/bootstrap-timepicker.min.css" />--%>
    <link rel="stylesheet" href="../assets/css/select2.min.css" />
    <link rel="stylesheet" href="../assets/css/AdminLTE.min.css" />
    <link rel="stylesheet" href="../assets/css/_all-skins.min.css" />
    <%--  <link rel="stylesheet"
        href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,600,700,300italic,400italic,600italic" />
    <script type="text/javascript" src="https://code.jquery.com/jquery-3.3.1.min.js"></script>--%>
    <script type="text/javascript"> 
        //function ResetTextBox() {
        //    $('.ResetTextBox').find('input:text').val('');
        //}
        function ShowAlert() {
            $('#modalAlert').modal('show');
        }
        function ShowSuccessAlert() {
            $('#modalSuccess').modal('show');
        }
        function ShowSharingModal() {
            $('#modal-AddSharing').modal('show');
        }
        function HideSharingModal() {
            $('#modal-AddSharing').modal('hide');
        }
        function ShowAddSharing() {
            $('#modal-default3').modal('show');
        }
        function HideAddSharingModal() {
            $('#modal-default3').modal('hide');
        }
        function ShowEditSharing() {
            $('#modal-defaultEdit').modal('show');
        }
        function HideEditSharingModal() {
            $('#modal-defaultEdit').modal('hide');
        }
    </script>
</head>
<body class="hold-transition layout-top-nav nav-color">
    <form runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <header class="main-header">
                    <div class="navbar-header" style="align-items: center;">
                        <a class="navbar-brand navbar-link" href="#">
                            <img src="../assets/img/dreams logo.png" runat="server" class="iconHeader" /></a>
                        <button class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navcol-1"><span class="sr-only">Toggle navigation</span><span class="icon-bar"></span><span class="icon-bar"></span><span class="icon-bar"></span></button>
                    </div>
                    <nav class="navbar navbar-static-top">
                    </nav>
                </header>
                <div class="content-wrapper">
                    <br />
                    <div class="row">
                        <div class="col-md-12">
                            <div class="stepwizard">
                                <div class="stepwizard-row setup-panel">
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <div class="stepwizard-step col-xs-3">
                                                <a runat="server" id="step1" href="#step-1" type="button" class="btn btn-success btn-circle">1</a>
                                                <p><small>Overview</small></p>
                                            </div>
                                            <div class="stepwizard-step col-xs-3">
                                                <a runat="server" id="step2" href="#step-2" type="button" class="btn btn-default btn-circle">2</a>
                                                <p><small>Broker Details</small></p>
                                            </div>
                                            <div class="stepwizard-step col-xs-3">
                                                <a runat="server" id="step3" href="#step-3" type="button" class="btn btn-default btn-circle">3</a>
                                                <p><small>Sales Persons</small></p>
                                            </div>
                                            <div class="stepwizard-step col-xs-3">
                                                <a runat="server" id="step4" href="#step-4" type="button" class="btn btn-default btn-circle">4</a>
                                                <p><small>Attachments</small></p>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="container-fluid">
                                <asp:Panel class="panel panel-primary  active" ID="step_1" runat="server" Visible="false">
                                    <div class="panel-heading" style="background-color: #5caceb">
                                        <h3 class="panel-title">Overview Details</h3>
                                    </div>
                                    <div class="panel-body">
                                        <div class="input-group col-md-4">
                                            <div>
                                                <label>Edit Code (For Existing Broker Applications - Ignore if new application)</label>
                                                <div class="input-group">
                                                    <asp:TextBox type="text" ID="txtUniqueIdSet" CssClass="form-control" placeholder="Unique ID - 10102302" runat="server"></asp:TextBox>
                                                    <asp:TextBox ID="txtUniqueId" runat="server" Visible="false" AutoPostBack="true"></asp:TextBox>
                                                    <span class="input-group-btn">
                                                        <button class="btn btn-primary" type="button" id="btnSubmitID" runat="server"
                                                            onserverclick="btnSubmitID_ServerClick">
                                                            Submit</button>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                        <h2>About this Form</h2>
                                        <br />
                                        This form will require you to encode your personal information as well as upload supplimentary documents for
                                        evaluation.
                                        Information needed will include the following:
                                        <br />
                                        <br />
                                        <li>Business Information</li>
                                        <li>Sales Personnel Information</li>
                                        <li>Social Security System</li>
                                        <li>Tax Identification Number</li>
                                        <li>Professional Regulation Comission</li>
                                        <li>Passport</li>
                                        <li>Broker Application Form </li>
                                        <li>List of Accredited Real Estate Sales Person</li>
                                        <li>Accreditation Agreement</li>
                                        <li>Broker Accreditation General Policies</li>
                                        <button class="btn btn-primary  pull-right" id="btnNext" runat="server" onserverclick="btnNext_ServerClick">Next <i class="fa fa-arrow-right"></i></button>
                                        <%--<asp:Button ID="btnNextStep" runat="server" Text="Next" OnClick="btnNextStep_Click" CssClass="form-control"/>--%> 
                                    </div>
                                </asp:Panel>
                      
                    </div>
                    <div class="container-fluid">
                                <asp:Panel class="panel panel-primary " runat="server" ID="step_2" Visible="false">
                                    <div class="panel-heading" style="background-color: #5caceb">
                                        <h3 class="panel-title">Broker Details</h3>
                                    </div>
                                    <div class="panel-body">
                                        <div class="box box-default">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">General Information</h3>
                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i class="fa fa-minus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-3">
                                                        <div class="form-group">
                                                            <label>Last Name *</label>
                                                            <asp:TextBox runat="server" ID="txtLastName" class="form-control" type="text" placeholder="Dela Cruz" />
                                                            <asp:RequiredFieldValidator
                                                                ID="reqFirstName"
                                                                ControlToValidate="txtLastName"
                                                                Text="Please fill up Last Name."
                                                                runat="server" Style="color: red" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <div class="form-group">
                                                            <label>First Name *</label>
                                                            <asp:TextBox runat="server" ID="txtFirstName" class="form-control" type="text" placeholder="Juan" />
                                                            <asp:RequiredFieldValidator
                                                                ID="RequiredFieldValidator1"
                                                                ControlToValidate="txtFirstName"
                                                                Text="Please fill up First Name."
                                                                runat="server" Style="color: red" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <div class="form-group">
                                                            <label>Middle Name *</label>
                                                            <asp:TextBox runat="server" ID="txtMiddleName" class="form-control" type="text" placeholder="D" />
                                                            <asp:RequiredFieldValidator
                                                                ID="RequiredFieldValidator2"
                                                                ControlToValidate="txtMiddleName"
                                                                Text="Please fill up Middle Name."
                                                                runat="server" Style="color: red" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <label>NickName</label>
                                                            <asp:TextBox runat="server" ID="txtNickName" class="form-control" type="text" placeholder="Juan" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="box box-danger">
                                                    <div class="box-header">
                                                        <h3 class="box-title">Address and Business Information</h3>
                                                    </div>
                                                    <div class="box-body">
                                                        <div class="col-md-12">
                                                            <div class="form-group">
                                                                <label>Complete Present Address *</label>
                                                                <asp:TextBox runat="server" ID="txtAddress" class="form-control" type="text" placeholder="Ermita, Manila, 1000 Metro Manila" />
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator3"
                                                                    ControlToValidate="txtAddress"
                                                                    Text="Please fill up Address."
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label>City *</label>
                                                                <asp:TextBox runat="server" ID="txtCity" class="form-control" type="text" placeholder="Manila" />
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator4"
                                                                    ControlToValidate="txtCity"
                                                                    Text="Please fill up City."
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label>ZIP Code *</label>
                                                                <asp:TextBox runat="server" ID="txtZipCode" class="form-control" type="text" placeholder="xxxx" />
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator5"
                                                                    ControlToValidate="txtZipCode"
                                                                    Text="Please fill up Zip Code."
                                                                    runat="server" Style="color: red" />
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                                                                    ControlToValidate="txtZipCode" runat="server"
                                                                    ErrorMessage="Only Numbers allowed!" Style="color: red"
                                                                    ValidationExpression="\d+">
                                                                </asp:RegularExpressionValidator>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label>Nature of Business *</label>
                                                                <asp:TextBox runat="server" ID="txtNatureOfBusiness" class="form-control" type="text"                                       placeholder="Retail" />
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator6"
                                                                    ControlToValidate="txtNatureOfBusiness"
                                                                    Text="Please fill up Nature Of Business."
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label>Business Name *</label>
                                                                <asp:TextBox runat="server" ID="txtBusinessName" class="form-control" type="text" placeholder="Juan Retailing" />
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator7"
                                                                    ControlToValidate="txtBusinessName"
                                                                    Text="Please fill up Business Name."
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12">
                                                            <div class="form-group">
                                                                <label>Complete Business Address *</label>
                                                                <asp:TextBox runat="server" ID="txtBusinessAddress" class="form-control" type="text"
                                                                    placeholder="Quirino Grandstand, 666 Behind, Ermita, Manila, 1000 Metro Manila" />
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator8"
                                                                    ControlToValidate="txtBusinessAddress"
                                                                    Text="Please fill up Business Address."
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label>Business ZIP Code *</label>
                                                                <asp:TextBox runat="server" ID="txtBusinessZipCode" class="form-control" type="text" placeholder="xxxx" />
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator9"
                                                                    ControlToValidate="txtBusinessZipCode"
                                                                    Text="Please fill up Business Zip Code."
                                                                    runat="server" Style="color: red" />
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2"
                                                                    ControlToValidate="txtBusinessZipCode" runat="server"
                                                                    ErrorMessage="Only Numbers allowed!" Style="color: red"
                                                                    ValidationExpression="\d+">
                                                                </asp:RegularExpressionValidator>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label>Business Phone No. *</label>
                                                                <asp:TextBox runat="server" ID="txtBusinessPhoneNo" class="form-control" type="text" placeholder="xxxxxxxxxxx" />
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator10"
                                                                    ControlToValidate="txtBusinessPhoneNo"
                                                                    Text="Please fill up Business Phone No."
                                                                    runat="server" Style="color: red" />
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3"
                                                                    ControlToValidate="txtBusinessPhoneNo" runat="server"
                                                                    ErrorMessage="Only Numbers allowed!" Style="color: red"
                                                                    ValidationExpression="\d+">
                                                                </asp:RegularExpressionValidator>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label>Fax No.</label>
                                                                <asp:TextBox runat="server" ID="txtFaxNo" class="form-control" type="text" placeholder="123456789" />
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4"
                                                                    ControlToValidate="txtFaxNo" runat="server"
                                                                    ErrorMessage="Only Numbers allowed!" Style="color: red"
                                                                    ValidationExpression="\d+">
                                                                </asp:RegularExpressionValidator>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label>Type of Business *</label>
                                                                <asp:RadioButtonList ID="rbTypeOfBusiness" runat="server" RepeatDirection="Horizontal">
                                                                    <asp:ListItem>Sole Proprietor&nbsp;&nbsp;&nbsp;</asp:ListItem>
                                                                    <asp:ListItem>Partnership&nbsp;&nbsp;&nbsp;</asp:ListItem>
                                                                    <asp:ListItem>Corporation&nbsp;&nbsp;&nbsp;</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator11"
                                                                    ControlToValidate="rbTypeOfBusiness"
                                                                    Text="Please select type of business."
                                                                    runat="server" Style="color: red" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="box box-primary">
                                                    <div class="box-header">
                                                        <h3 class="box-title">Supplimentary Details</h3>
                                                    </div>
                                                    <div class="box-body">
                                                        <div class="col-md-12">

                                                      <%--      <div class="col-md-6">
                                                                <label>Date of Birth *</label>
                                                                <div class="input-group">
                                                                    <div class="input-group-addon">
                                                                        <i class="fa fa-calendar"></i>
                                                                    </div>
                                                                    <asp:TextBox runat="server" ID="dtDateOfBirth" class="form-control" data-inputmask="'alias': 'dd/mm/yyyy'" data-mask="" />
                                                                </div>
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator12"
                                                                    ControlToValidate="dtDateOfBirth"
                                                                    Text="Please fill up Date Of Birth."
                                                                    runat="server" Style="color: red" />
                                                            </div>--%>
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label>Place of Birth *</label>
                                                                    <asp:TextBox runat="server" ID="txtPlaceOfBirth" class="form-control" type="text" placeholder="Manila" />
                                                                    <asp:RequiredFieldValidator
                                                                        ID="RequiredFieldValidator13"
                                                                        ControlToValidate="txtPlaceOfBirth"
                                                                        Text="Please fill up Place Of Birth."
                                                                        runat="server" Style="color: red" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label>Religion</label>
                                                                    <asp:TextBox runat="server" ID="txtReligion" class="form-control" type="text" placeholder="Default input" />
                                                                    <%-- <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator14"
                                                        ControlToValidate="txtReligion"
                                                        Text="Please fill up Religion."
                                                        runat="server" Style="color:red" />--%>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group">

                                                                    <label>Citizenship *</label>
                                                                    <asp:TextBox runat="server" ID="txtCitizenship" class="form-control" type="text" placeholder="Fil" />
                                                                    <asp:RequiredFieldValidator
                                                                        ID="RequiredFieldValidator15"
                                                                        ControlToValidate="txtCitizenship"
                                                                        Text="Please fill up Citizenship."
                                                                        runat="server" Style="color: red" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label>Tax Identification Number (TIN) *</label>
                                                                    <asp:TextBox runat="server" ID="txtTIN" class="form-control" type="text" placeholder="-xx-xxxx" />
                                                                    <asp:RequiredFieldValidator
                                                                        ID="RequiredFieldValidator16"
                                                                        ControlToValidate="txtTIN"
                                                                        Text="Please fill up TIN."
                                                                        runat="server" Style="color: red" />
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator5"
                                                                        ControlToValidate="txtTIN" runat="server"
                                                                        ErrorMessage="Only Numbers allowed!" Style="color: red"
                                                                        ValidationExpression="\d+">
                                                                    </asp:RegularExpressionValidator>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label>Social Security Number (SSS) *</label>
                                                                    <asp:TextBox runat="server" ID="txtSSS" class="form-control" type="text" placeholder="xx-xxxxxx-x" />
                                                                    <asp:RequiredFieldValidator
                                                                        ID="RequiredFieldValidator17"
                                                                        ControlToValidate="txtSSS"
                                                                        Text="Please fill up SSS."
                                                                        runat="server" Style="color: red" />
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator6"
                                                                        ControlToValidate="txtSSS" runat="server"
                                                                        ErrorMessage="Only Numbers allowed!" Style="color: red"
                                                                        ValidationExpression="\d+">
                                                                    </asp:RegularExpressionValidator>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label>Passport No. *</label>
                                                                    <asp:TextBox runat="server" ID="txtPassport" class="form-control" type="text" placeholder="12345678" />
                                                                    <asp:RequiredFieldValidator
                                                                        ID="rfvPass"
                                                                        ControlToValidate="txtPassport"
                                                                        Text="Please fill up Pass No."
                                                                        runat="server" Style="color: red" />
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator7"
                                                                        ControlToValidate="txtPassport" runat="server"
                                                                        ErrorMessage="Only Numbers allowed!" Style="color: red"
                                                                        ValidationExpression="\d+">
                                                                    </asp:RegularExpressionValidator>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label>Passport Valid From *</label>

                                                                    <asp:TextBox runat="server" ID="txtPassportValid" class="form-control" />

                                                                    <asp:RequiredFieldValidator
                                                                        ID="rfvPassValid"
                                                                        ControlToValidate="txtPassportValid"
                                                                        Text="Please fill up Passport Valid From."
                                                                        runat="server" Style="color: red" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group">

                                                                    <label>Issued by (Name of Gov't Agency) *</label>
                                                                    <select class="form-control" runat="server" id="dpIssuedBy">
                                                                        <option value="1">Department of Foreign Affairs</option>
                                                                    </select>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label>Place Issued</label>
                                                                    <asp:TextBox runat="server" ID="txtPlaceIssued" class="form-control" type="text" placeholder="Manila Philippines" />
                                                                    <asp:RequiredFieldValidator
                                                                        ID="rfvPlaceIssued"
                                                                        ControlToValidate="txtPlaceIssued"
                                                                        Text="Please fill up Place Issued."
                                                                        runat="server" Style="color: red" />
                                                                </div>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="box box-success">
                                                    <div class="box-header with-border">
                                                        <h3 class="box-title">PRC License Information</h3>

                                                        <div class="box-tools pull-right">
                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse" runat="server">
                                                                <i class="fa fa-minus"></i>
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="box-body">
                                                        <div class="row">
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label>PRC Registration Number *</label>
                                                                    <asp:TextBox runat="server" ID="txtPRCRegis" class="form-control" type="text" placeholder="12345678" />
                                                                    <asp:RequiredFieldValidator
                                                                        ID="RequiredFieldValidator21"
                                                                        ControlToValidate="txtPRCRegis"
                                                                        Text="Please fill up PRC Registration Number."
                                                                        runat="server" Style="color: red" />
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator8"
                                                                        ControlToValidate="txtPRCRegis" runat="server"
                                                                        ErrorMessage="Only Numbers allowed!" Style="color: red"
                                                                        ValidationExpression="\d+">
                                                                    </asp:RegularExpressionValidator>
                                                                </div>
                                                            </div>
                                                        <%--    <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label>PRC License Validity *</label>

                                                                    <asp:TextBox runat="server" ID="txtPRCLicense" class="form-control" placeholder="01/01/2022" />
                                                                    <asp:RequiredFieldValidator
                                                                        ID="RequiredFieldValidator22"
                                                                        ControlToValidate="txtPRCLicense"
                                                                        Text="Please fill up PRC License Validity."
                                                                        runat="server" Style="color: red" />
                                                                </div>
                                                            </div>--%>
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label>PTR Number *</label>
                                                                    <asp:TextBox runat="server" ID="txtPTRNumber" class="form-control" type="text" placeholder="12345678" />
                                                                    <asp:RequiredFieldValidator
                                                                        ID="RequiredFieldValidator23"
                                                                        ControlToValidate="txtPTRNumber"
                                                                        Text="Please fill up PTR Number."
                                                                        runat="server" Style="color: red" />
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator10"
                                                                        ControlToValidate="txtPTRNumber" runat="server"
                                                                        ErrorMessage="Only Numbers allowed!" Style="color: red"
                                                                        ValidationExpression="\d+">
                                                                    </asp:RegularExpressionValidator>
                                                                </div>
                                                            </div>
                                                      <%--      <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label>Valid Date range: *</label>
                                                                    <div class="input-group">
                                                                        <div class="input-group-addon">
                                                                            <i class="fa fa-calendar"></i>
                                                                        </div>
                                                                        <input id="reservation2" class="form-control pull-right" />
                                                                    </div>
                                                                    <asp:RequiredFieldValidator
                                                                        ID="RequiredFieldValidator24"
                                                                        ControlToValidate="reservation2"
                                                                        Text="Please fill up Valid Date range."
                                                                        runat="server" Style="color: red" />
                                                                </div>
                                                            </div>--%>
                                                        </div>

                                                        <div class="col-md-4 pull-right">
                                                            <button class="btn btn-primary nextBtn pull-right" type="button" id="btnNext2" runat="server" onserverclick="btnNext2_ServerClick">Next <i class="fa fa-arrow-right"></i></button>
                                                            <%--<button class="btn btn-danger pull-right" type="button" id="btnPrevious" runat="server" onserverclick="btnPrevious_ServerClick"><i class="fa fa-arrow-left"></i> Previous</button>--%>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                    </div>
                    <div class="container-fluid">
                        <asp:Panel runat="server" class="panel panel-primary " ID="step_3" Visible="false">
                            <div class="panel-heading" style="background-color: #5caceb">
                                <h3 class="panel-title">Sales Persons (include yourself)</h3>
                            </div>
                            <div class="panel-body">
                                <div class="col-md-12">
                                    <!-- /.box-header -->
                                    <button class="btn btn-primary" type="button" data-toggle="modal" data-target="#modal-addRow"><i class="fa fa-plus"></i>Add Sales Person</button>
                                    <asp:GridView ID="gvSalesPerson" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                        CssClass="table table-bordered table-hover" Width="100%" ShowHeader="True" PageSize="3">
                                        <HeaderStyle BackColor="#66ccff" />

                                        <Columns>
                                            <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" ItemStyle-Font-Size="Medium" />
                                            <asp:BoundField DataField="SalesPersonName" HeaderText="Sales Person Name" SortExpression="Sales Person Name" ItemStyle-Font-Size="Medium" />
                                            <asp:BoundField DataField="PRCLicense/AccreditationNo" HeaderText="PRC License/Accreditation No" SortExpression="PRC License/Accreditation No" ItemStyle-Font-Size="Medium" />
                                            <asp:BoundField DataField="MobileNumber" HeaderText="Mobile Number" SortExpression="Mobile Number" ItemStyle-Font-Size="Medium" />
                                            <asp:BoundField DataField="EmailAddress" HeaderText="Email Address" SortExpression="Email Address" ItemStyle-Font-Size="Medium" />
                                            <asp:BoundField DataField="Position" HeaderText="Position" SortExpression="Position" ItemStyle-Font-Size="Medium" />
                                            <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center" HeaderText="Sharing">
                                                <ItemTemplate>
                                                    <button runat="server" id="gvShare" type="button" class="btn btn-default btn-success" data-toggle="modal" data-target="#modal-AddSharing"><i class="fa fa-edit"></i>Open</button>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="pagination-ys" />
                                        <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                    </asp:GridView>

                                    <div class="col-md-4 pull-right">

                                        <button class="btn btn-primary nextBtn pull-right" type="button" id="btnNext3" runat="server" onserverclick="btnNext3_ServerClick">Next <i class="fa fa-arrow-right"></i></button>
                                        <button class="btn btn-danger nextBtn pull-right" type="button" id="btnPrevious2" runat="server" onserverclick="btnPrevious2_ServerClick"><i class="fa fa-arrow-left"></i>Previous</button>
                                    </div>

                                    <!-- /.box-body -->

                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="container-fluid">

                        <asp:Panel runat="server" class="panel panel-primary " ID="step_4" Visible="false">
                            <div class="panel-heading" style="background-color: #5caceb">
                                <h3 class="panel-title">Attachments</h3>
                            </div>
                            <div class="panel-body">
                                <div class="col-md-12">
                                    <asp:Panel runat="server" class="box box-success" ID="Standard">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">Standard Attachments</h3>

                                            <div class="box-tools pull-right">
                                                <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                            </div>
                                        </div>
                                        <div class="box-body">
                                            <table class="table table-bordered">
                                                <tbody>
                                                    <tr>

                                                        <th>Document Name</th>
                                                        <th>Action</th>

                                                    </tr>
                                                    <tr>

                                                        <td>Broker Application Form</td>
                                                        <td>
                                                            <div class="form-group">
                                                                <label for="exampleInputFile">File input</label>
                                                                <input type="file" id="exampleInputFile" />

                                                                <p class="help-block">PDF, XLS, .Doc</p>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr>

                                                        <td>List of Accredited Real Estate Sales Person</td>
                                                        <td>
                                                            <div class="form-group">
                                                                <label for="exampleInputFile">File input</label>
                                                                <input type="file" id="exampleInputFile1" />

                                                                <p class="help-block">PDF, XLS, .Doc</p>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr>

                                                        <td>Accreditation Agreement</td>
                                                        <td>
                                                            <div class="form-group">
                                                                <label for="exampleInputFile">File input</label>
                                                                <input type="file" id="exampleInputFile2" />

                                                                <p class="help-block">PDF, XLS, .Doc</p>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr>

                                                        <td>Broker Accreditation General Policies</td>
                                                        <td>
                                                            <div class="form-group">
                                                                <label for="exampleInputFile">File input</label>
                                                                <input type="file" id="exampleInputFile3" />

                                                                <p class="help-block">PDF, XLS, .Doc</p>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </asp:Panel>

                                    <asp:Panel runat="server" class="box box-success" ID="SoleProp" Visible="false">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">Individual (Sole Proprietor)</h3>

                                            <div class="box-tools pull-right">
                                                <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                            </div>
                                        </div>
                                        <div class="box-body">
                                            <table class="table table-bordered">
                                                <tbody>
                                                    <tr>

                                                        <th>Document Name</th>
                                                        <th>Action</th>

                                                    </tr>
                                                    <tr>

                                                        <td>Real Estate Brokers License</td>
                                                        <td>
                                                            <div class="form-group">
                                                                <label for="exampleInputFile">File input</label>
                                                                <input type="file" id="exampleInputFile4" />

                                                                <p class="help-block">PDF, XLS, .Doc</p>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr>

                                                        <td>HLURB Certificate of Registration</td>
                                                        <td>
                                                            <div class="form-group">
                                                                <label for="exampleInputFile">File input</label>
                                                                <input type="file" id="exampleInputFile5" />

                                                                <p class="help-block">PDF, XLS, .Doc</p>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr>

                                                        <td>VAT Registration/Proof of VAT</td>
                                                        <td>
                                                            <div class="form-group">
                                                                <label for="exampleInputFile">File input</label>
                                                                <input type="file" id="exampleInputFile6" />

                                                                <p class="help-block">PDF, XLS, .Doc</p>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr>

                                                        <td>Any Valid Gov't Issued ID</td>
                                                        <td>
                                                            <div class="form-group">
                                                                <label for="exampleInputFile">File input</label>
                                                                <input type="file" id="exampleInputFile7" />

                                                                <p class="help-block">PDF, XLS, .Doc</p>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr>

                                                        <td>DTI Registration</td>
                                                        <td>
                                                            <div class="form-group">
                                                                <label for="exampleInputFile">File input</label>
                                                                <input type="file" id="exampleInputFile8" />

                                                                <p class="help-block">PDF, XLS, .Doc</p>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr>

                                                        <td>Official Receipt, Sample/Photocopy</td>
                                                        <td>
                                                            <div class="form-group">
                                                                <label for="exampleInputFile">File input</label>
                                                                <input type="file" id="exampleInputFile9" />

                                                                <p class="help-block">PDF, XLS, .Doc</p>
                                                            </div>
                                                        </td>

                                                    </tr>


                                                </tbody>
                                            </table>
                                        </div>
                                    </asp:Panel>

                                    <asp:Panel runat="server" class="box box-success" ID="Partnership" Visible="false">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">Individual (Partnership)</h3>

                                            <div class="box-tools pull-right">
                                                <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                            </div>
                                        </div>
                                        <div class="box-body">
                                            <table class="table table-bordered">
                                                <tbody>
                                                    <tr>

                                                        <th>Document Name</th>
                                                        <th>Action</th>

                                                    </tr>
                                                    <tr>

                                                        <td>Real Estate Brokers License</td>
                                                        <td>
                                                            <div class="form-group">
                                                                <label for="exampleInputFile">File input</label>
                                                                <input type="file" id="exampleInputFile10" />

                                                                <p class="help-block">PDF, XLS, .Doc</p>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr>

                                                        <td>HLURB Certificate of Registration</td>
                                                        <td>
                                                            <div class="form-group">
                                                                <label for="exampleInputFile">File input</label>
                                                                <input type="file" id="exampleInputFile11" />

                                                                <p class="help-block">PDF, XLS, .Doc</p>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr>

                                                        <td>VAT Registration/Proof of VAT</td>
                                                        <td>
                                                            <div class="form-group">
                                                                <label for="exampleInputFile">File input</label>
                                                                <input type="file" id="exampleInputFile12" />

                                                                <p class="help-block">PDF, XLS, .Doc</p>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr>

                                                        <td>Any Valid Gov't Issued ID</td>
                                                        <td>
                                                            <div class="form-group">
                                                                <label for="exampleInputFile">File input</label>
                                                                <input type="file" id="exampleInputFile13" />

                                                                <p class="help-block">PDF, XLS, .Doc</p>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr>

                                                        <td>SEC Registration</td>
                                                        <td>
                                                            <div class="form-group">
                                                                <label for="exampleInputFile">File input</label>
                                                                <input type="file" id="exampleInputFile14" />

                                                                <p class="help-block">PDF, XLS, .Doc</p>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr>

                                                        <td>Articles of Partnership</td>
                                                        <td>
                                                            <div class="form-group">
                                                                <label for="exampleInputFile">File input</label>
                                                                <input type="file" id="exampleInputFile15" />

                                                                <p class="help-block">PDF, XLS, .Doc</p>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr>

                                                        <td>Official Receipt, Sample/Photocopy</td>
                                                        <td>
                                                            <div class="form-group">
                                                                <label for="exampleInputFile">File input</label>
                                                                <input type="file" id="exampleInputFile16" />

                                                                <p class="help-block">PDF, XLS, .Doc</p>
                                                            </div>
                                                        </td>

                                                    </tr>


                                                </tbody>
                                            </table>
                                        </div>
                                    </asp:Panel>

                                    <asp:Panel runat="server" class="box box-success" ID="Corporation" Visible="false">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">Corporation</h3>

                                            <div class="box-tools pull-right">
                                                <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                            </div>
                                        </div>
                                        <div class="box-body">
                                            <table class="table table-bordered">
                                                <tbody>
                                                    <tr>

                                                        <th>Document Name</th>
                                                        <th>Action</th>

                                                    </tr>
                                                    <tr>

                                                        <td>VAT Registration/ Proof of TIN</td>
                                                        <td>
                                                            <div class="form-group">
                                                                <label for="exampleInputFile">File input</label>
                                                                <input type="file" id="exampleInputFile17" />

                                                                <p class="help-block">PDF, XLS, .Doc</p>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr>

                                                        <td>Articles of Incorporation & By Laws</td>
                                                        <td>
                                                            <div class="form-group">
                                                                <label for="exampleInputFile">File input</label>
                                                                <input type="file" id="exampleInputFile18">

                                                                <p class="help-block">PDF, XLS, .Doc</p>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr>

                                                        <td>Secretary's Certificate/ Board of Resolution</td>
                                                        <td>
                                                            <div class="form-group">
                                                                <label for="exampleInputFile">File input</label>
                                                                <input type="file" id="exampleInputFile19">

                                                                <p class="help-block">PDF, XLS, .Doc</p>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr>

                                                        <td>Official Receipt, Sample/ Photocoy</td>
                                                        <td>
                                                            <div class="form-group">
                                                                <label for="exampleInputFile">File input</label>
                                                                <input type="file" id="exampleInputFile20" />

                                                                <p class="help-block">PDF, XLS, .Doc</p>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr>

                                                        <td>Real Estate Brokers License (PRC)</td>
                                                        <td>
                                                            <div class="form-group">
                                                                <label for="exampleInputFile">File input</label>
                                                                <input type="file" id="exampleInputFile21" />

                                                                <p class="help-block">PDF, XLS, .Doc</p>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr>

                                                        <td>VAT Registration/ Proof of TIN</td>
                                                        <td>
                                                            <div class="form-group">
                                                                <label for="exampleInputFile">File input</label>
                                                                <input type="file" id="exampleInputFile22" />

                                                                <p class="help-block">PDF, XLS, .Doc</p>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr>

                                                        <td>Any Valid Gov't Issued ID w/ Photo & Signature</td>
                                                        <td>
                                                            <div class="form-group">
                                                                <label for="exampleInputFile">File input</label>
                                                                <input type="file" id="exampleInputFile23" />

                                                                <p class="help-block">PDF, XLS, .Doc</p>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </asp:Panel>

                                    <div class="col-md-4 pull-right">
                                        <button class="btn btn-primary nextBtn pull-right" type="button" data-toggle="modal"
                                            data-target="#modal-default2">
                                            Submit Documents <i class="fa fa-arrow-right"></i>
                                        </button>
                                        <button class="btn btn-danger nextBtn pull-right" type="button" id="btnPrevious3" runat="server" onserverclick="btnPrevious3_ServerClick"><i class="fa fa-arrow-left"></i>Previous</button>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
      


        <%--//Modal Alert--%>
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
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>

        <%--//Modal Alert--%>
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
                                        <asp:Label runat="server" ID="Label2" Text="Your Unique ID is:"></asp:Label>
                                        <asp:Label runat="server" ID="mtxtUniqueId"></asp:Label>
                                    </div>
                                </div>
                                <div class="row" style="text-align: center;">
                                    <div class="col-lg-12">
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal-addRow">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Add Sales Person</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label>Sales Person Name</label>
                                        <asp:TextBox runat="server" ID="mtxtSalesPerson" type="text" placeholder="Juan Dela Cruz" class="form-control" />
                                        <asp:RequiredFieldValidator
                                            ID="RequiredFieldValidator20"
                                            ControlToValidate="mtxtSalesPerson"
                                            Text="Please fill up Sales Person Name."
                                            runat="server" Style="color: red" />
                                    </div>

                                    <div class="form-group">
                                        <label>Email Address</label>
                                        <asp:TextBox runat="server" ID="mtxtEmail" type="text" placeholder="Juan@gmail.com" class="form-control" />
                                        <asp:RequiredFieldValidator
                                            ID="RequiredFieldValidator18"
                                            ControlToValidate="mtxtEmail"
                                            Text="Please fill up Sales Person Name."
                                            runat="server" Style="color: red" />
                                    </div>
                                    <div class="form-group">
                                        <label>Position</label>
                                        <asp:DropDownList runat="server" ID="ddPosition" class="form-control">
                                            <asp:ListItem Value="---Select Position---" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Sales Agent"> </asp:ListItem>
                                            <asp:ListItem Value="2" Text="Broker"> </asp:ListItem>
                                            <asp:ListItem Value="3" Text="Manager"> </asp:ListItem>
                                            <asp:ListItem Value="4" Text="Area Head"> </asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label>PRC License/Accreditation No. </label>
                                        <asp:TextBox runat="server" ID="mtxtPRCLicense" type="text" placeholder="12345678" class="form-control" />
                                        <asp:RequiredFieldValidator
                                            ID="RequiredFieldValidator14"
                                            ControlToValidate="mtxtPRCLicense"
                                            Text="Please fill up PRC License/Accreditation No.."
                                            runat="server" Style="color: red" />
                                    </div>

                                    <div class="form-group">
                                        <label>Mobile Number </label>
                                        <asp:TextBox runat="server" ID="mtxtMobile" type="text" class="form-control" placeholder="090000000" />
                                        <asp:RequiredFieldValidator
                                            ID="RequiredFieldValidator19"
                                            ControlToValidate="mtxtMobile"
                                            Text="Please fill up PRC License/Accreditation No.."
                                            runat="server" Style="color: red" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator9"
                                            ControlToValidate="mtxtMobile" runat="server"
                                            ErrorMessage="Only Numbers allowed!" Style="color: red"
                                            ValidationExpression="\d+">
                                        </asp:RegularExpressionValidator>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger pull-left" data-dismiss="modal">Close</button>
                        <button runat="server" type="button" class="btn btn-primary" id="btnAddSalesPerson" onserverclick="btnAddSalesPerson_ServerClick"><i class="fa fa-plus-circle"></i>Add</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>

        <div class="modal fade" id="modal-AddSharing">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Sharing Details</h4>
                    </div>
                    <div class="modal-body">
                        <button class="btn btn-primary" type="button" data-toggle="modal" data-target="#modal-default3">Add Row</button><br />
                        <asp:GridView ID="gvShareDetails" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                            CssClass="table table-bordered table-hover" Width="100%" ShowHeader="True" PageSize="3">
                            <HeaderStyle BackColor="#66ccff" />
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" ItemStyle-Font-Size="Medium" />
                                <asp:BoundField DataField="Position" HeaderText="Position" SortExpression="Position" ItemStyle-Font-Size="Medium" />
                                <asp:BoundField DataField="SalesPersonName" HeaderText="Sales Person Name" SortExpression="Sales Person Name No" ItemStyle-Font-Size="Medium" />
                                <asp:BoundField DataField="Percentage" HeaderText="Percentage" SortExpression="Percentage" ItemStyle-Font-Size="Medium" />
                                <asp:BoundField DataField="Amount?" HeaderText="Amount?" SortExpression="Amount?" ItemStyle-Font-Size="Medium" />

                                <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center" HeaderText="Action">
                                    <ItemTemplate>
                                        <%--<button runat="server" type="button" class="btn btn-default btn-success" id="btnEdit"
                                            onserverclick="btnEdit_ServerClick"><i class="fa fa-eye"></i>View</button>--%>
                                        <button data-id="<%#Eval("Id") %>" id="btnView" type="button" title="View" class="openDialog"></button>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="pagination-ys" />
                            <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                        </asp:GridView>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger pull-left" data-dismiss="modal">Close</button>
                        <button type="button" class="btn btn-primary"><i class="fa fa-plus"></i>Add</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal-default3">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Add Sharing Details</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label>Position</label>
                                        <asp:DropDownList runat="server" ID="mddPositionShare" class="form-control">
                                            <asp:ListItem Value="---Select Position---" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Sales Agent"> </asp:ListItem>
                                            <asp:ListItem Text="Broker"> </asp:ListItem>
                                            <asp:ListItem Text="Manager"> </asp:ListItem>
                                            <asp:ListItem Text="Area Head"> </asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <label>Percentage</label>
                                        <asp:TextBox runat="server" ID="mtxtPecent" type="text" placeholder="10%" class="form-control" />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label>Sales Person Name</label>
                                        <asp:TextBox runat="server" ID="mtxtSalesPersonShare" type="text" placeholder="Juan Dela Cruz" class="form-control" />
                                    </div>

                                    <div class="form-group">
                                        <label>Amount? </label>
                                        <asp:TextBox runat="server" ID="mtxtAmount" type="text" class="form-control" placeholder="10,000" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger pull-left" data-dismiss="modal">Close</button>
                        <button class="btn btn-primary" type="button" id="btnAdd" runat="server" onserverclick="btnAdd_ServerClick">
                            <i class="fa fa-save"></i>Save</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal-defaultEdit">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Add Sharing Details</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label>Position</label>
                                        <asp:DropDownList runat="server" ID="mddPositionEdit" class="form-control">
                                            <asp:ListItem Value="---Select Position---" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Sales Agent"> </asp:ListItem>
                                            <asp:ListItem Text="Broker"> </asp:ListItem>
                                            <asp:ListItem Text="Manager"> </asp:ListItem>
                                            <asp:ListItem Text="Area Head"> </asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <label>Percentage</label>
                                        <asp:TextBox runat="server" ID="mtxtPercentageEdit" type="text" class="form-control" />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label>Sales Person Name</label>
                                        <asp:TextBox runat="server" ID="mtxtSalesPersonEdit" type="text" class="form-control" />
                                    </div>

                                    <div class="form-group">
                                        <label>Amount? </label>
                                        <asp:TextBox runat="server" ID="mtxtAmountEdit" type="text" class="form-control" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger pull-left" data-dismiss="modal">Close</button>
                        <button class="btn btn-primary" type="button" id="btnUpdate" runat="server" onserverclick="btnUpdate_ServerClick">
                            <i class="fa fa-save"></i>Save</button>
                    </div>
                </div>
            </div>
        </div>
          </form>
        <%--<script src="../assets/js/jquery.smartWizard.js"></script>--%>
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

        <%-- <script type="text/javascript" src="js/addons-pro/steppers.js"></script>
        <script type="text/javascript" src="js/addons-pro/steppers.min.js"></script>--%>

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
                $('#reservation').daterangepicker();
                $('#reservation2').daterangepicker();

                $('#drValidDate').daterangepicker();
                //Date range picker with time picker
                $('#reservationtime').daterangepicker({ timePicker: true, timePickerIncrement: 30, locale: { format: 'MM/DD/YYYY hh:mm A' } })
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

                //Date picker
             

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
                });

                ////Colorpicker
                //$('.my-colorpicker1').colorpicker()
                //color picker with addon
               
            })
  </script>
        <script>
            $(document).ready(function () {

               // $('#datepicker').daterangepicker({
               //     autoclose: true
               // });
               //// $('.my-colorpicker2')()

               // ////Timepicker
               // $('.timepicker').timepicker({
               //     showInputs: false
               // });
                //$(document).ready(function () {
                //    $('.stepper').mdbStepper();
                //});
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

                      
  
</body>
</html>
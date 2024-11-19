<%@ Page Title="DREAMS | Broker Approval" Language="C#" MasterPageFile="~/master/Main.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="BrokerApprove.aspx.cs" Inherits="ABROWN_DREAMS.pages.BrokerApprove" %>

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

            $("#modalSuccess").on("hidden", function () {
                window.location.href = '/pages/BrokerApprove.aspx';
            });

            //GAB 07/01/2023 TIN Masking
            $('#<%=txtTIN.ClientID%>').mask('000-000-000-000');
            $('#<%=mtxtTIN.ClientID%>').mask('000-000-000-000');

        });
        function nextTab(elem) {
            $(elem).next().find('a[data-toggle="tab"]').click();
        }
        function prevTab(elem) {
            $(elem).prev().find('a[data-toggle="tab"]').click();
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>
    <%--############[ END ENABLE/DISABLE WIZARD ]#############--%>

    <%--//MODALS--%>
    <script type="text/javascript"> 

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

        //CONFIRMATION
        function hideConfirm() {
            $('#modalConfirmation').modal('hide');
        }
        function showConfirmation() {
            $('#modalConfirmation').modal('show');
        }

        //ADD SALES PERSON
        function ShowAddSalesPersonModal() {
            $('#modalAddSalesPerson').modal('show');
        }
        function HideAddSalesPersonModal() {
            $('#modalAddSalesPerson').modal('hide');
        }

        //ADD SHARING MODAL
        function ShowAddSharingModal() {
            $('#modalAddSharing').modal('show');
        }
        function HideAddSharingModal() {
            $('#modalAddSharing').modal('hide');
        }

        //GAB 05/13/2023 For displaying error messages
        function ShowErrorMessageModal(errorMessage) {
            $('#modalErrorMessage').modal('show');
            $('#errorMessageId').text(errorMessage);
        }
        function HideErrorMessageModal() {
            $('#modalErrorMessage').modal('hide');
        }

        //ADD SHARING DETIALS
        function ShowAddSharingDetailsModal() {
            $('#modalAddSharingDetails').modal('show');
        }
        function HideAddSharingDetailsModal() {
            $('#modalAddSharingDetails').modal('hide');
        }

        //BUSINESS TYPE
        function HideBusinessTypeModal() {
            $('#modalBusinessType').modal('hide');
        }
        function ShowBusinessTypeModal() {
            $('#modalBusinessType').modal('show');
        }

        function ShowEditSharing() {
            $("#modaldefaultEdit").modal("show");
        }

        //ADD SALES AGENT 
        function ShowRegisterSalesAgentModal() {
            $('#modalRegisterSalesAgent').modal('show');
        }
        function HideRegisterSalesAgentModal() {
            $('#modalRegisterSalesAgent').modal('hide');
        }

        function HideContactDetailsWithHide() {
            $('#contactdetails :input').prop('disabled', true);
            $('#contactdetails').addClass('hidden');
            ExpandAddressAndBusinessInformation();
            HideBusinessTypeModal();
        }

        function ShowContactDetails() {
            $('#contactdetails :input').prop('disabled', true);
            $('#contactdetails').removeClass('hidden');
            HideBusinessTypeModal();
            RevertAddressAndBusinessInformation();
        }

        function HideDetailsModal() {
            $('#modalAddSharingDetails').modal('hide');
            $('#modalAddSharing').modal('hide');
        }

        //BROKER MODAL
        function ModalBrokerList_Show() {
            $('#ModalBrokerList').modal({
                backdrop: 'static',
                keyboard: false
            });
        }
        function ModalBrokerList_Hide() {
            $('#ModalBrokerList').modal('hide');
        }


        //APPROVAL TYPE MODAL 
        function ModalApprovalType_Show() {
            $('#modalApprovalType').modal({
                backdrop: 'static',
                keyboard: false
            });
        }
        function ModalApprovalType_Hide() {
            $('#modalApprovalType').modal('hide');
        }

        //LOAD SALES AGENT
        function ShowListOfSalesPersons() {
            $('#MsgListOfSalesPersons').modal('show');
        }
        function HideListOfSalesPersons() {
            $('#MsgListOfSalesPersons').modal('hide');
        }

        function CloseModalsAfterUpdateSalesPerson() {
            $('#MsgListOfSalesPersons').modal('hide');
            $('#modalAddSalesPerson').modal('hide');
        }

        //GAB 07/04/2023 DELETING CONFIRMATION
        function hideConfirmDelete() {
            $('#modalConfirmDelete').modal('hide');
        }
        function showConfirmDelete() {
            $('#modalConfirmDelete').modal({
                backdrop: 'static',
                keyboard: false
            });
        }

        $(document).ready(function () {
            $(".btn-pref .btn").click(function () {
                $(".btn-pref .btn").removeClass("btn-primary").addClass("btn-default");
                $(this).removeClass("btn-default").addClass("btn-primary");
            });

            $("#modalSuccess").on("hidden.bs.modal", function () {
                window.location.href = '/pages/BrokerApprove.aspx';
            });

        });
    </script>

    <%-- Navigation for Wizard --%>
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

        //Supplementary Details
        function ShowSupplementaryDetails() {
            $('#supplementarydetails').removeClass('hidden');
            $('#solefields').removeClass('hidden');
            $('#solefields2').removeClass('hidden');
            $('#<%=civilstat1.ClientID%>').removeClass('hidden');
            RevertAddressAndBusinessInformation();
        }
        function HideSupplementaryDetails() {
            $('#supplementarydetails').addClass('hidden');
            $('#solefields').addClass('hidden');
            $('#solefields2').addClass('hidden');
            $('#<%=civilstat1.ClientID%>').addClass('hidden');
            RevertAddressAndBusinessInformation();
        }

        //Address and Business Information
        function ExpandAddressAndBusinessInformation() {
            $('#addbusinessinfo').removeClass();
            $('#addbusinessinfo').addClass('col-md-12');
        }
        function RevertAddressAndBusinessInformation() {
            $('#addbusinessinfo').removeClass();
            $('#addbusinessinfo').addClass('col-md-6');
        }
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




</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="sitemap" runat="server">
    <li><a href="#"><span>Broker Approval</span></a></li>
</asp:Content>





<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">


    <div class="row">
        <div class="col-md-12">
            <div class="stepwizard">
                <div class="stepwizard-row setup-panel">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="stepwizard-step col-xs-4">
                                <a runat="server" id="step1" href="#step-2" type="button" class="btn btn-default btn-circle">1</a>
                                <p><small>Broker Details</small></p>
                            </div>
                            <div class="stepwizard-step col-xs-4">
                                <a runat="server" id="step2" href="#step-3" type="button" class="btn btn-default btn-circle">2</a>
                                <p><small>Sales Persons</small></p>
                            </div>
                            <div class="stepwizard-step col-xs-4">
                                <a runat="server" id="step3" href="#step-4" type="button" class="btn btn-default btn-circle">3</a>
                                <p><small>Attachments</small></p>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>


    <div class="container-fluid">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:Panel class="panel panel-primary " runat="server" ID="step_2" Visible="false">
                    <div class="panel-heading" style="background-color: #5caceb">
                        <h3 class="panel-title">Broker Details</h3>


                    </div>
                    <div class="panel-body">
                        <h4 class="box-title" style="color: royalblue; font-weight: bolder;">Broker ID:
                            <asp:Label runat="server" ID="lblBrokerID"></asp:Label>
                            <asp:LinkButton runat="server" ID="btnSearchBroker" Style="width: 90px;" CssClass="btn btn-info btn-default btn-facebook" OnClick="btnSearchBroker_Click">Find   <i class="fa fa-search"></i></asp:LinkButton>
                        </h4>
                        <div runat="server" id="approvalDate">
                            <h4 class="box-title" style="color: royalblue; font-weight: bolder;">Approval Date:
                                    <asp:Label runat="server" ID="lblApprovalDate"></asp:Label>
                            </h4>
                        </div>
                        <span>
                            <div class="box-tools pull-right">
                                <button class="btn btn-info pull-right" type="button" id="btnCreateusers" onserverclick="btnCreateusers_ServerClick" runat="server">Create Users <i class="fa fa-user-plus"></i></button>
                            </div>
                        </span>
                        <div runat="server" id="divReports" visible="false">
                            <span>
                                <div class="box-tools pull-right">
                                    <%--2023-07-12 : SPECIFIED WIDTH TO HAVE UNIFORM SIZE--%>
                                    <button class="btn btn-success pull-right" style="margin-right: 10px; width: 278px;" type="button" id="btnBrokerApplication" onserverclick="btnPrintApproved_ServerClick" runat="server">Print Broker Application <i class="fa fa-male"></i></button>
                                    <button class="btn btn-success pull-right" style="margin-right: 10px; width: 278px;" type="button" id="btnAccreditationAgreement" onserverclick="btnPrintApproved_ServerClick" runat="server">Print Accreditation Agreement <i class="fa fa-newspaper-o"></i></button>
                                    <button class="btn btn-success pull-right" style="margin-right: 10px; width: 278px;" type="button" id="btnGeneralPolicies" onserverclick="btnPrintApproved_ServerClick" runat="server">Print General Policies <i class="fa fa-map-o"></i></button>
                                    <button class="btn btn-success pull-right" style="margin-right: 10px; width: 278px;" type="button" id="btnListOfAccredited" onserverclick="btnPrintApproved_ServerClick" runat="server">Print List of Accredited Sales Persons <i class="fa fa-list-alt"></i></button>
                                </div>
                            </span>
                        </div>
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="form-group">
                                    <label>Comments: </label>
                                    <asp:TextBox ID="txtGeneralInfoComments" Style="resize: none;" TextMode="MultiLine" Rows="3" class="form-control" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="box box-primary">
                            <div style="background-color: #468DBC; height: 3px;"></div>
                            <div class="box-header">
                                <h3 class="box-title">Personal Profile
                                </h3>
                            </div>
                            <div class="box-body">
                                <%--PARTNERSHIP--%>
                                <div class="row hidden" id="divPartnership" runat="server">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="lblPartnership" Text="Partnership"></asp:Label><span class="color-red fsize-16"> *</span>
                                            <asp:TextBox runat="server" ID="txtPartnership" MaxLength="50" OnTextChanged="txtPartnership_TextChanged" AutoPostBack="true" class="form-control text-uppercase" ReadOnly="true" type="text" placeholder="CORPORATION NAME" />
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator12"
                                                ControlToValidate="txtPartnership"
                                                Text="Please fill up Partnership Name."
                                                ValidationGroup="Next"
                                                runat="server" Style="color: red" />
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="lblSECRegNo" Text="SEC Reg No."></asp:Label><span class="color-red fsize-16"> *</span>
                                            <asp:TextBox runat="server" ID="txtSECRegNo" class="form-control text-uppercase" ReadOnly="true" type="text" placeholder="SEC REG NO." />
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator25"
                                                ControlToValidate="txtSECRegNo"
                                                Text="Please fill up SEC Registration No."
                                                ValidationGroup="Next"
                                                runat="server" Style="color: red" />
                                        </div>
                                    </div>
                                </div>

                                <%--SOLE PROPRIETORSHIP--%>
                                <div class="row" id="divSoleProp" runat="server">
                                    <div class="row col-md-12">

                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <asp:Label runat="server" ID="lblFirstName" Text="First Name"></asp:Label><span class="color-red fsize-16">*</span>
                                                <asp:TextBox runat="server" ID="txtFirstName" ValidationGroup="Next" class="form-control text-uppercase" ReadOnly="true" type="text" placeholder="Juan" />
                                                <asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator1"
                                                    ControlToValidate="txtFirstName"
                                                    Text="Please fill up First Name."
                                                    ValidationGroup="Next"
                                                    runat="server" Style="color: red" />
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:Label runat="server" ID="lblMiddleName" Text="Middle Name"></asp:Label><span class="color-red fsize-16">*</span>
                                                <asp:TextBox runat="server" ID="txtMiddleName" ValidationGroup="Next" class="form-control text-uppercase" ReadOnly="true" type="text" placeholder="D" />
                                                <asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator2"
                                                    ControlToValidate="txtMiddleName"
                                                    ValidationGroup="Next"
                                                    Text="Please fill up Middle Name."
                                                    runat="server" Style="color: red" />
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:Label runat="server" ID="lblLastName" Text="Last Name"></asp:Label><span class="color-red fsize-16">*</span>
                                                <asp:TextBox runat="server" ID="txtLastName" ValidationGroup="Next" class="form-control text-uppercase" ReadOnly="true" type="text" placeholder="Dela Cruz" />
                                                <asp:RequiredFieldValidator
                                                    ID="reqFirstName"
                                                    ControlToValidate="txtLastName"
                                                    Text="Please fill up Last Name."
                                                    ValidationGroup="Next"
                                                    runat="server" Style="color: red" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <asp:Label runat="server" ID="lblNickName" Text="NickName"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtNickName" class="form-control text-uppercase" ReadOnly="true" type="text" placeholder="Juan" />
                                            </div>
                                        </div>


                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Complete Present Address<span class="color-red fsize-16"> *</span></label>
                                            <asp:TextBox runat="server" AutoPostBack="true" ID="txtAddress" MaxLength="250" OnTextChanged="txtAddress_TextChanged" class="form-control text-uppercase" ReadOnly="true" type="text" ValidationGroup="Next"
                                                placeholder="Ermita, Manila,1000 Metro Manila" />
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator3"
                                                ValidationGroup="Next"
                                                ControlToValidate="txtAddress"
                                                Text="Please fill up Address."
                                                runat="server" Style="color: red" />
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <div class="form-group">
                                            <label>City/Municipality<%--<span class="color-red fsize-16"> *</span>--%></label>
                                            <asp:TextBox runat="server" ID="txtCity" class="form-control text-uppercase" type="text" ReadOnly="true" placeholder="Manila" />
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <div class="form-group">
                                            <label>Province<%--<span class="color-red fsize-16"> *</span>--%></label>
                                            <asp:TextBox runat="server" ID="txtProvince" class="form-control text-uppercase" type="text" placeholder="" />
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <div class="form-group">
                                            <label>ZIP Code<span class="color-red fsize-16"> *</span></label>
                                            <asp:TextBox runat="server" AutoPostBack="true" onkeypress="return isNumberKey(event)" ID="txtZipCode" OnTextChanged="txtZipCode_TextChanged" class="form-control" ValidationGroup="Next" ReadOnly="true" type="text" placeholder="0000" />
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator5"
                                                ValidationGroup="Next"
                                                ControlToValidate="txtZipCode"
                                                Text="Please fill up Zip Code."
                                                runat="server" Style="color: red" />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2"
                                                ControlToValidate="txtZipCode" runat="server"
                                                ValidationGroup="Next"
                                                CssClass="col-md-12"
                                                ErrorMessage="Only Numbers allowed"
                                                ForeColor="Red"
                                                ValidationExpression="\d+">
                                            </asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class="col-md-5">
                                        <div class="form-group">
                                            <label>Residence Address</label>
                                            <asp:TextBox runat="server" ID="txtResidenceNo" ValidationGroup="Next" class="form-control text-uppercase" type="text" placeholder="" />
                                            <%--<asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator42"
                                                        ValidationGroup="Next"
                                                        ControlToValidate="txtResidenceNo"
                                                        Text="Please fill up Residence No."
                                                        runat="server" Style="color: red" />--%>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <div class="form-group">

                                            <label>Mobile No<span class="color-red fsize-16"> *</span></label>
                                            <asp:TextBox runat="server" ID="txtMobileNo" ValidationGroup="Next" MaxLength="11" oninput="this.value=this.value.slice(0,this.maxLength)" class="form-control text-uppercase" type="number" placeholder="09000000000" />
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator43"
                                                ValidationGroup="Next"
                                                ControlToValidate="txtMobileNo"
                                                Text="Please fill up Mobile No."
                                                runat="server" Style="color: red" />
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Email Address<span class="color-red fsize-16"> *</span></label>
                                            <asp:TextBox runat="server" ID="txtEmailAddress" class="form-control" type="text" MaxLength="50" ValidationGroup="Next" placeholder="google@gmail.com" />
                                            <asp:RegularExpressionValidator
                                                ID="txtEmailAddressValidator"
                                                runat="server"
                                                ControlToValidate="txtEmailAddress"
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                Text="Invalid email address."
                                                Style="color: red">
                                            </asp:RegularExpressionValidator>
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator20"
                                                ValidationGroup="Next"
                                                ControlToValidate="txtEmailAddress"
                                                Text="Please fill up Email Address."
                                                runat="server" Style="color: red" />
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <div class="form-group">
                                            <label>Location<span class="color-red fsize-16"> *</span></label>
                                            <asp:DropDownList ID="ddlLocation" DataTextField="Name" DataValueField="Code" runat="server" CssClass="form-control" />
                                            <%--GAB 6/16/2023 REQUIRED LOCATION--%>
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidatorLocation"
                                                Enabled="true"
                                                ControlToValidate="ddlLocation"
                                                Text="Please select Location"
                                                InitialValue="-1"
                                                ValidationGroup="Next"
                                                runat="server" Style="color: red" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row" id="solefields"  runat="server">
                                    <div class="col-md-1" style="width: 19.999999992%">

                                        <div class="form-group">
                                            <label>Date of Birth<span class="color-red fsize-16"> *</span></label>
                                            <div class="input-group">
                                                <%--  <div class="input-group-addon">
                                                                        <i class="fa fa-calendar"></i>
                                                                    </div>--%>
                                                <asp:TextBox ID="dtDateOfBirth" type="date" Max="9999-12-31" ReadOnly="true" class="form-control" runat="server" />
                                                <asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator24"
                                                    ControlToValidate="dtDateOfBirth"
                                                    ValidationGroup="Next" CssClass="col-md-12"
                                                    Text="Please fill up Date of Birth."
                                                    runat="server" Style="color: red" />
                                                <asp:CompareValidator ID="CompareValidator3" runat="server"
                                                    ControlToValidate="dtDateOfBirth" ErrorMessage="Please Enter a valid date <br>"
                                                    Operator="DataTypeCheck" Type="Date" ValidationGroup="Next" Style="color: red" />
                                                <asp:CustomValidator
                                                    Style="color: red"
                                                    runat="server"
                                                    ID="CustomValidator3"
                                                    ValidationGroup="Next" CssClass="col-md-12"
                                                    ControlToValidate="dtDateOfBirth"
                                                    Text="Date of Birth cannot be earlier than today."
                                                    OnServerValidate="CustomValidator3_ServerValidate" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-1" style="width: 19.999999992%">
                                        <div class="form-group">
                                            <label>Place of Birth<span class="color-red fsize-16"> *</span></label>
                                            <asp:TextBox runat="server" ID="txtPlaceOfBirth" class="form-control text-uppercase" ReadOnly="true" type="text" placeholder="Manila" />
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator13"
                                                ControlToValidate="txtPlaceOfBirth"
                                                ValidationGroup="Next"
                                                Text="Please fill up Place Of Birth."
                                                runat="server" Style="color: red" />
                                        </div>
                                    </div>
                                    <div class="col-md-1" style="width: 19.999999992%">
                                        <div class="form-group">
                                            <label>Religion</label>
                                            <asp:TextBox runat="server" ID="txtReligion" ReadOnly="true" class="form-control text-uppercase" type="text" />
                                        </div>
                                    </div>
                                    <div class="col-md-1" style="width: 19.999999992%">
                                        <div class="form-group">

                                            <label>Citizenship<span class="color-red fsize-16"> *</span></label>
                                            <%--<asp:TextBox runat="server" ID="txtCitizenship" ValidationGroup="Next" ReadOnly="true" class="form-control text-uppercase" type="text" placeholder="Fil" />--%><select runat="server" id="txtCitizenship" name="nationality" class="form-control text-uppercase">
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
                                            <%--<asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator15"
                                                            ValidationGroup="Next"
                                                            ControlToValidate="txtCitizenship"
                                                            Text="Please fill up Citizenship."
                                                            runat="server" Style="color: red" />--%>
                                        </div>
                                    </div>
                                    <div class="col-md-8" style="width: 19.999999992%">
                                        <div class="form-group">
                                            <label>Sex<span class="color-red fsize-16"> *</span></label>
                                            <asp:DropDownList ID="ddSex" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="---SELECT SEX---" Text="---Select Gender---" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="FEMALE" Text="Female"> </asp:ListItem>
                                                <asp:ListItem Value="MALE" Text="Male"> </asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator44"
                                                Enabled="true"
                                                ControlToValidate="ddSex"
                                                Text="Please select Sex"
                                                InitialValue="---SELECT SEX---"
                                                ValidationGroup="Next"
                                                runat="server" Style="color: red" />
                                        </div>
                                    </div>
                                </div>



                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label>Tax Identification Number (TIN)<span class="color-red fsize-16"> *</span></label>
                                            <asp:TextBox runat="server" ID="txtTIN" AutoPostBack="true" OnTextChanged="txtTIN_TextChanged" ValidationGroup="Next" ReadOnly="true" class="form-control" type="text" placeholder="00-0000" />
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator16"
                                                ValidationGroup="Next"
                                                ControlToValidate="txtTIN"
                                                Text="Please fill up TIN."
                                                runat="server" Style="color: red" />
                                            <asp:CustomValidator
                                                Style="color: red"
                                                runat="server"
                                                ID="CustomValidator14"
                                                ValidationGroup="Next"
                                                ControlToValidate="txtTIN"
                                                Text="Invalid TIN format. Must be 000-000-000-000"
                                                OnServerValidate="CustomValidator14_ServerValidate" />
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label>Nature of Business<span class="color-red fsize-16"> *</span></label>
                                            <asp:TextBox runat="server" ID="txtNatureOfBusiness" ReadOnly="true" class="form-control text-uppercase" type="text" placeholder="REAL EASTATE" />
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator6"
                                                ValidationGroup="Next"
                                                ControlToValidate="txtNatureOfBusiness"
                                                Text="Please fill up Nature Of Business."
                                                runat="server" Style="color: red" />
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label>Business Name<span class="color-red fsize-16"> *</span></label>
                                            <asp:TextBox runat="server" ID="txtBusinessName" OnTextChanged="txtBusinessName_TextChanged" MaxLength="50" class="form-control text-uppercase" ReadOnly="true" type="text" ValidationGroup="Next"
                                                placeholder="Juan Retailing" />
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator7"
                                                ValidationGroup="Next"
                                                ControlToValidate="txtBusinessName"
                                                Text="Please fill up Business Name."
                                                runat="server" Style="color: red" />
                                        </div>
                                    </div>

                                    <div class="col-md-10">
                                        <div class="form-group">
                                            <label>Complete Business Address<span class="color-red fsize-16"> *</span></label>
                                            <asp:LinkButton runat="server" ID="btnCopyPrincipalAddress" OnClick="btnCopyPrincipalAddress_Click" class="btn btn-info btn-sm" Style="margin-left: 10px"> Copy principal buyer's present address</asp:LinkButton></span>
                                            <asp:TextBox runat="server" ID="txtBusinessAddress" class="form-control text-uppercase" ReadOnly="true" type="text" ValidationGroup="Next"
                                                placeholder="Quirino Grandstand, 666 Behind, Ermita, Manila, 1000 Metro Manila" Style="margin-top: 10px" />
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator8"
                                                ValidationGroup="Next"
                                                ControlToValidate="txtBusinessAddress"
                                                Text="Please fill up Business Address."
                                                runat="server" Style="color: red" />
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <div class="form-group">
                                            <label>Business ZIP Code<span class="color-red fsize-16"> *</span></label>
                                            <asp:TextBox runat="server" ID="txtBusinessZipCode" onkeypress="return isNumberKey(event)" class="form-control" type="text" ReadOnly="true" ValidationGroup="Next" placeholder="0000" />
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator9"
                                                ValidationGroup="Next"
                                                ControlToValidate="txtBusinessZipCode"
                                                Text="Please fill up Business Zip Code."
                                                runat="server" Style="color: red" />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                                                ControlToValidate="txtBusinessZipCode" runat="server"
                                                ValidationGroup="Next"
                                                CssClass="col-md-12"
                                                ErrorMessage="Only Numbers allowed"
                                                ForeColor="Red"
                                                ValidationExpression="\d+">
                                            </asp:RegularExpressionValidator>

                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label>Business No.<span class="color-red fsize-16"> *</span></label>
                                            <asp:TextBox runat="server" ID="txtBusinessPhoneNo" ValidationGroup="Next" class="form-control" ReadOnly="true" type="text" />
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator10"
                                                ValidationGroup="Next"
                                                ControlToValidate="txtBusinessPhoneNo"
                                                Text="Please fill up Business No."
                                                runat="server" Style="color: red" />

                                        </div>
                                    </div>
                                    <div class="col-md-2 hidden">
                                        <div class="form-group">
                                            <label>Fax No.</label>
                                            <asp:TextBox runat="server" ID="txtFaxNo" class="form-control" type="number" ReadOnly="true" placeholder="123456789" />

                                        </div>
                                    </div>

                                    <div class="col-md-4" id="solefields2" runat="server">
                                        <div class="form-group">
                                            <label>Civil Status</label><span class="color-red fsize-16"> *</span>
                                            <asp:DropDownList ID="ddCivilStatus" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddCivilStatus_SelectedIndexChanged">
                                                <asp:ListItem Value="---SELECT CIVIL STATUS---" Text="---Select Civil Status---" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="SINGLE" Text="Single"> </asp:ListItem>
                                                <asp:ListItem Value="MARRIED" Text="Married"> </asp:ListItem>
                                                <%--GAB 06/19/2023--%>
                                                <%--<asp:ListItem Value="WIDOWED" Text="Widowed"> </asp:ListItem>
                                                <asp:ListItem Value="SEPARATED" Text="Separated"> </asp:ListItem>
                                                <asp:ListItem Value="DIVORCED" Text="Divorced"> </asp:ListItem>--%>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator47"
                                                Enabled="true"
                                                ControlToValidate="ddCivilStatus"
                                                Text="Please select Civil Status"
                                                InitialValue="---SELECT CIVIL STATUS---"
                                                ValidationGroup="Next"
                                                runat="server" Style="color: red" />
                                        </div>
                                    </div>
                                    <div class="col-md-4" runat="server" id="civilstat1">
                                        <div class="form-group">
                                            <label>Name of Spouse</label><span class="color-red fsize-16"> *</span>
                                            <asp:TextBox runat="server" ID="txtSpouse" class="form-control text-uppercase" type="text" placeholder="Juan" ReadOnly="true" autocomplete="new-password" />
                                            <!--GAB 06/07/2023 Spouse Validation-->
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator50"
                                                ControlToValidate="txtSpouse"
                                                ValidationGroup="Next"
                                                Text="Please fill up Spouse."
                                                runat="server" Style="color: red" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="box box-default">
                            <div style="background-color: lightgrey; height: 3px;"></div>
                            <div class="box-header with-border">
                                <h3 class="box-title">General Information</h3>

                            </div>
                            <div class="box-body">



                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label>ATP Expiry Date<span class="color-red fsize-16"> *</span></label>
                                            <%--  <div class="input-group-addon">
                                                        <i class="fa fa-calendar"></i>
                                                    </div>--%>

                                            <div class="input-group" style="width: 100%;">
                                                <asp:TextBox ID="txtATPExpiryDate" type="date" Max="9999-12-31" ValidationGroup="Next" class="form-control" ReadOnly="true" runat="server" />
                                                <%--COMMENTED 04-04-2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                                                <%-- <asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator22"
                                                    ControlToValidate="txtATPExpiryDate" CssClass="col-md-12"
                                                    Text="Please fill up ATP Expiry Date."
                                                    ValidationGroup="Next"
                                                    runat="server" Style="color: red" />--%>
                                                <asp:CompareValidator ID="CompareValidator1" runat="server"
                                                    ControlToValidate="txtATPExpiryDate" ErrorMessage="Please Enter a valid date <br>"
                                                    Operator="DataTypeCheck" Type="Date" ValidationGroup="Next" Style="color: red" />
                                                <asp:CustomValidator
                                                    Style="color: red"
                                                    runat="server"
                                                    ID="CustomValidator2"
                                                    ValidationGroup="Next"
                                                    ControlToValidate="txtATPExpiryDate" CssClass="col-md-12"
                                                    Text="ATP Expiry Date cannot be earlier than today."
                                                    OnServerValidate="CustomValidator2_ServerValidate" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label>DHSUD Accreditation License Valid from:<span class="color-red fsize-16"> *</span></label>
                                            <asp:TextBox ID="txtHlurb" runat="server" type="date" Max="9999-12-31"
                                                class="form-control" />
                                            <asp:CompareValidator ID="CompareValidator2" runat="server"
                                                ControlToValidate="txtHlurb" ErrorMessage="Please Enter a valid date <br>"
                                                Operator="DataTypeCheck" Type="Date" ValidationGroup="Next" Style="color: red" />
                                            <asp:CustomValidator
                                                Style="color: red"
                                                runat="server" CssClass="col-md-12"
                                                ID="CustomValidator21"
                                                ValidationGroup="Next"
                                                ControlToValidate="txtHlurb"
                                                Text="Invalid Date."
                                                OnServerValidate="CustomValidator21_ServerValidate" />
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label>DHSUD Accreditation License No.:<span class="color-red fsize-16"> *</span></label>
                                            <asp:TextBox ID="txtHlurbNo" runat="server" type="text"
                                                class="form-control" />
                                        </div>
                                    </div>
                                </div>



                                <div class="row" runat="server" id="validids">
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <div class="row col-md-12">
                                                <div class="col-md-4" runat="server" id="ddothers1">
                                                    <div class="form-group">
                                                        <label>Valid ID 1<span class="color-red fsize-16"> *</span></label>
                                                        <asp:DropDownList ID="ddValidID" AutoPostBack="true" OnSelectedIndexChanged="ddValidID_SelectedIndexChanged" runat="server" DataTextField="Name" DataValueField="Code" CssClass="form-control">
                                                            <asp:ListItem Value="---Select Valid ID---" Selected="True"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator46"
                                                            Enabled="true"
                                                            ControlToValidate="ddValidID"
                                                            Text="Please select Valid ID"
                                                            InitialValue="---Select Valid ID---"
                                                            ValidationGroup="Next"
                                                            runat="server" Style="color: red" />
                                                    </div>
                                                </div>
                                                <div class="col-md-2" runat="server" id="others1" visible="false">
                                                    <div class="form-group">
                                                        <%-- <asp:Label runat="server" ID="Label2" Text="ID No."></asp:Label>--%>
                                                        <label>Specify ID<span class="color-red fsize-16"> *</span></label>
                                                        <asp:TextBox runat="server" AutoPostBack="true" OnTextChanged="txtOthers_TextChanged" ID="txtOthers1" class="form-control text-uppercase" type="text" placeholder="ID" />
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator59"
                                                            ControlToValidate="txtOthers1"
                                                            ValidationGroup="Next"
                                                            Text="Please specify ID."
                                                            runat="server" Style="color: red" />
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <%-- <asp:Label runat="server" ID="Label2" Text="ID No."></asp:Label>--%>
                                                        <label>ID Number 1<span class="color-red fsize-16"> *</span></label>
                                                        <asp:TextBox runat="server" ID="txtIDNo" AutoPostBack="true" OnTextChanged="txtIDNo_TextChanged" ValidationGroup="Next" MaxLength="25" class="form-control text-uppercase" type="text" placeholder="12345678" />
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator54"
                                                            ControlToValidate="txtIDNo"
                                                            ValidationGroup="Next"
                                                            Text="Please fill up ID Number."
                                                            runat="server" Style="color: red" />
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label runat="server" id="lblId1">ID Expiry Date 1<span class="color-red fsize-16"> *</span></label>
                                                        <%--<div class="input-group">--%>
                                                        <asp:TextBox ID="txtIDExpirationDate" runat="server" type="date" Max="9999-12-31"
                                                            class="form-control" />
                                                        <%--</div>--%>
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator55" CssClass="col-md-12"
                                                            ValidationGroup="Next"
                                                            ControlToValidate="txtIDExpirationDate"
                                                            Text="Please fill up ID Expiration Date"
                                                            runat="server" Style="color: red" />
                                                        <asp:CompareValidator ID="CompareValidator17" runat="server"
                                                            ControlToValidate="txtATPExpiryDate" ErrorMessage="Please Enter a valid date <br>"
                                                            Operator="DataTypeCheck" Type="Date" ValidationGroup="Next" Style="color: red" />
                                                        <asp:CustomValidator
                                                            Style="color: red"
                                                            runat="server"
                                                            ID="CustomValidator10" CssClass="col-md-12"
                                                            ValidationGroup="Next"
                                                            ControlToValidate="txtIDExpirationDate"
                                                            Text="Passport Valid To cannot be lower than today."
                                                            OnServerValidate="CustomValidator10_ServerValidate" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row col-md-12">
                                                <div class="col-md-4" id="ddothers2" runat="server">
                                                    <div class="form-group">


                                                        <label>
                                                            Valid ID 2
                                                            <%--<span class="color-red fsize-16">*</span>--%>
                                                        </label>
                                                        <asp:DropDownList ID="ddValidID2" runat="server" OnSelectedIndexChanged="ddValidID2_SelectedIndexChanged" AutoPostBack="true" DataTextField="Name" DataValueField="Code" CssClass="form-control">
                                                            <asp:ListItem Value="---Select Valid ID---" Selected="True"></asp:ListItem>
                                                        </asp:DropDownList>

                                                        <%--COMMENTED 04-04-2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                                                        <%--                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator56"
                                                            Enabled="true"
                                                            ControlToValidate="ddValidID2"
                                                            Text="Please select Valid ID"
                                                            InitialValue="---Select Valid ID---"
                                                            ValidationGroup="Next"
                                                            runat="server" Style="color: red" />--%>
                                                    </div>
                                                </div>
                                                <div class="col-md-2" runat="server" id="others2" visible="false">
                                                    <div class="form-group">
                                                        <%-- <asp:Label runat="server" ID="Label2" Text="ID No."></asp:Label>--%>
                                                        <label>Specify ID<span class="color-red fsize-16"> *</span></label>
                                                        <asp:TextBox runat="server" ID="txtOthers2" AutoPostBack="true" OnTextChanged="txtOthers_TextChanged" class="form-control text-uppercase" type="text" placeholder="ID" />
                                                        <%--COMMENTED 04-04-2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                                                        <%--                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator60"
                                                            ControlToValidate="txtOthers2"
                                                            ValidationGroup="Next"
                                                            Text="Please specify ID."
                                                            runat="server" Style="color: red" />--%>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <%-- <asp:Label runat="server" ID="Label2" Text="ID No."></asp:Label>--%>
                                                        <label>ID Number 2<span class="color-red fsize-16"> *</span></label>
                                                        <asp:TextBox runat="server" ID="txtIDNo2" AutoPostBack="true" OnTextChanged="txtIDNo2_TextChanged" MaxLength="25" class="form-control text-uppercase" type="text" placeholder="12345678" />
                                                        <%--COMMENTED 04-04-2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                                                        <%--                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator57"
                                                            ControlToValidate="txtIDNo2"
                                                            ValidationGroup="Next"
                                                            Text="Please fill up ID Number."
                                                            runat="server" Style="color: red" />--%>
                                                    </div>
                                                </div>

                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label runat="server" id="lblId2">ID Expiry Date 2<span class="color-red fsize-16"> *</span></label>

                                                        <asp:TextBox ID="txtIDExpirationDate2" runat="server" type="date" Max="9999-12-31"
                                                            class="form-control" />

                                                        <%--COMMENTED 04-04-2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                                                        <%--       <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator58"
                                                            ValidationGroup="Next"
                                                            ControlToValidate="txtIDExpirationDate2"
                                                            Text="Please fill up ID Expiration Date" CssClass="col-md-12"
                                                            runat="server" Style="color: red" />
                                                        <asp:CompareValidator ID="CompareValidator18" runat="server"
                                                            ControlToValidate="txtIDExpirationDate2" ErrorMessage="Please Enter a valid date <br>"
                                                            Operator="DataTypeCheck" Type="Date" ValidationGroup="Next" Style="color: red" />
                                                        <asp:CustomValidator
                                                            Style="color: red"
                                                            runat="server"
                                                            ID="CustomValidator18" CssClass="col-md-12"
                                                            ValidationGroup="Next"
                                                            ControlToValidate="txtIDExpirationDate2"
                                                            Text="Passport Valid To cannot be lower than today."
                                                            OnServerValidate="CustomValidator18_ServerValidate" />--%>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row col-md-12" id="idvalid3" runat="server" visible="false">
                                                <div class="col-md-4" runat="server" id="ddothers3">
                                                    <div class="form-group">
                                                        <label>Valid ID 3</label>
                                                        <asp:DropDownList ID="ddValidID3" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddValidID3_SelectedIndexChanged" DataTextField="Name" DataValueField="Code" CssClass="form-control">
                                                            <asp:ListItem Value="---Select Valid ID---" Selected="True"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-2" runat="server" id="others3" visible="false">
                                                    <div class="form-group">
                                                        <%-- <asp:Label runat="server" ID="Label2" Text="ID No."></asp:Label>--%>
                                                        <label>Specify ID</label>
                                                        <asp:TextBox runat="server" ID="txtOthers3" class="form-control text-uppercase" type="text" placeholder="ID" />
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator61"
                                                            ControlToValidate="txtOthers3"
                                                            ValidationGroup="Next"
                                                            Text="Please specify ID."
                                                            runat="server" Style="color: red" />
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <%-- <asp:Label runat="server" ID="Label2" Text="ID No."></asp:Label>--%>
                                                        <label>ID Number 3</label>
                                                        <asp:TextBox runat="server" ID="txtIDNo3" AutoPostBack="true" OnTextChanged="txtIDNo3_TextChanged" ReadOnly="true" MaxLength="25" class="form-control text-uppercase" type="text" placeholder="12345678" />
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator63"
                                                            ControlToValidate="txtIDNo3"
                                                            ValidationGroup="Next"
                                                            Enabled="false"
                                                            Text="Please fill up ID Number."
                                                            runat="server" Style="color: red" />
                                                    </div>
                                                </div>

                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label runat="server" id="lblId3">ID Expiry Date 3</label>

                                                        <asp:TextBox ID="txtIDExpirationDate3" ReadOnly="true" runat="server" type="date" Max="9999-12-31"
                                                            class="form-control" />
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator65"
                                                            ValidationGroup="Next"
                                                            ControlToValidate="txtIDExpirationDate3" CssClass="col-md-12"
                                                            Enabled="false"
                                                            Text="Please fill up ID Expiration Date"
                                                            runat="server" Style="color: red" />
                                                        <asp:CompareValidator ID="CompareValidator19" runat="server"
                                                            ControlToValidate="txtIDExpirationDate3" ErrorMessage="Please Enter a valid date <br>"
                                                            Operator="DataTypeCheck" Type="Date" ValidationGroup="Next" Style="color: red" />
                                                        <asp:CustomValidator
                                                            Style="color: red"
                                                            runat="server"
                                                            ID="CustomValidator19" CssClass="col-md-12"
                                                            ValidationGroup="Next"
                                                            ControlToValidate="txtIDExpirationDate3"
                                                            Text="Passport Valid To cannot be lower than today."
                                                            OnServerValidate="CustomValidator19_ServerValidate" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row col-md-12" id="idvalid4" runat="server" visible="false">
                                                <div class="col-md-4" runat="server" id="ddothers4">
                                                    <div class="form-group">
                                                        <label>Valid ID 4</label>
                                                        <asp:DropDownList ID="ddValidID4" AutoPostBack="true" OnSelectedIndexChanged="ddValidID4_SelectedIndexChanged" runat="server" DataTextField="Name" DataValueField="Code" CssClass="form-control">
                                                            <asp:ListItem Value="---Select Valid ID---" Selected="True"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-2" runat="server" id="others4" visible="false">
                                                    <div class="form-group">
                                                        <%-- <asp:Label runat="server" ID="Label2" Text="ID No."></asp:Label>--%>
                                                        <label>Specify ID</label>
                                                        <asp:TextBox runat="server" ID="txtOthers4" class="form-control text-uppercase" type="text" placeholder="ID" />
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator62"
                                                            ControlToValidate="txtOthers4"
                                                            ValidationGroup="Next"
                                                            Text="Please specify ID."
                                                            runat="server" Style="color: red" />
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <%-- <asp:Label runat="server" ID="Label2" Text="ID No."></asp:Label>--%>
                                                        <label>ID Number 4</label>
                                                        <asp:TextBox runat="server" ID="txtIDNo4" AutoPostBack="true" OnTextChanged="txtIDNo4_TextChanged" ReadOnly="true" MaxLength="25" class="form-control text-uppercase" type="text" placeholder="12345678" />
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator64"
                                                            ControlToValidate="txtIDNo4"
                                                            Enabled="false"
                                                            ValidationGroup="Next"
                                                            Text="Please fill up ID Number."
                                                            runat="server" Style="color: red" />
                                                    </div>
                                                </div>

                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label runat="server" id="lblId4">ID Expiry Date 4</label>

                                                        <asp:TextBox ID="txtIDExpirationDate4" ReadOnly="true" runat="server" type="date" Max="9999-12-31"
                                                            class="form-control" />
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator66"
                                                            ValidationGroup="Next"
                                                            ControlToValidate="txtIDExpirationDate4"
                                                            Text="Please fill up ID Expiration Date" CssClass="col-md-12"
                                                            Enabled="false"
                                                            runat="server" Style="color: red" />
                                                        <asp:CompareValidator ID="CompareValidator20" runat="server"
                                                            ControlToValidate="txtIDExpirationDate4" ErrorMessage="Please Enter a valid date <br>"
                                                            Operator="DataTypeCheck" Type="Date" ValidationGroup="Next" Style="color: red" />
                                                        <asp:CustomValidator
                                                            Style="color: red"
                                                            runat="server"
                                                            ID="CustomValidator20"
                                                            ValidationGroup="Next" CssClass="col-md-12"
                                                            ControlToValidate="txtIDExpirationDate"
                                                            Text="Passport Valid To cannot be lower than today."
                                                            OnServerValidate="CustomValidator20_ServerValidate" />
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6" id="addbusinessinfo">
                                <div class="box box-danger">
                                    <div style="background-color: darkorange; height: 3px;"></div>

                                    <div class="box-header">
                                        <%--<h3 class="box-title">Address and Business Information</h3>--%>
                                        <h3 class="box-title">Tax Details</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="row" id="divTaxDetails">
                                            <div class="col-lg-12">
                                                <div class="form-group">
                                                    <label>Comments: </label>
                                                    <asp:TextBox ID="txtAddressBusinessComments" TextMode="MultiLine" Style="resize: none;" Rows="3" class="form-control" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row hidden">

                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label>Trade Name<span class="color-red fsize-16"> *</span></label>
                                                    <asp:TextBox runat="server" ID="txtTradeName" class="form-control text-uppercase" type="text" ValidationGroup="Next"
                                                        placeholder="Trade Name" />
                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator4"
                                                        Enabled="false"
                                                        ValidationGroup="Next"
                                                        ControlToValidate="txtTradeName"
                                                        Text="Please fill up Trade Name."
                                                        runat="server" Style="color: red" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>VAT Code<span class="color-red fsize-16"> *</span></label>
                                                    <asp:DropDownList ID="ddlVATCode" DataTextField="Name" DataValueField="Code" runat="server" CssClass="form-control" Style="text-transform: capitalize;" />
                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidatorVAT"
                                                        Enabled="true"
                                                        ControlToValidate="ddlVATCode"
                                                        Text="Please select VAT Code"
                                                        InitialValue="-1"
                                                        ValidationGroup="Next"
                                                        runat="server" Style="color: red" />
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Withholding Tax Code<span class="color-red fsize-16"> *</span></label>
                                                    <asp:DropDownList ID="ddlWTAXCode" DataTextField="WTName" DataValueField="WTCode" runat="server" CssClass="form-control" Style="text-transform: capitalize;" />
                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidatorWTax"
                                                        Enabled="true"
                                                        ControlToValidate="ddlWTAXCode"
                                                        Text="Please select Withholding Tax Code"
                                                        InitialValue="-1"
                                                        ValidationGroup="Next"
                                                        runat="server" Style="color: red" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 hidden" id="supplementarydetails">
                                <div class="box box-primary">
                                    <div style="background-color: cornflowerblue; height: 3px;"></div>

                                    <div class="box-header">
                                        <h3 class="box-title">Supplementary Details</h3>
                                    </div>
                                    <div class="box-body">

                                        <div class="row">
                                            <div class="col-lg-12">
                                                <div class="form-group">
                                                    <label>Comments: </label>
                                                    <asp:TextBox ID="txtSupplementaryDetailsComments" Style="resize: none;" TextMode="MultiLine" Rows="3" class="form-control" runat="server" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-12">
                                            <div class="row">
                                            </div>
                                            <div class="row">
                                            </div>
                                            <div class="row">

                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label>&nbsp;</label>
                                                    </div>
                                                </div>

                                                <%--<div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Social Security Number (SSS)</label>
                                                        <asp:TextBox runat="server" ID="txtSSS" class="form-control" ReadOnly="true" ValidationGroup="Next" type="text" placeholder="00-0000-0" />
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator17"
                                                            ControlToValidate="txtSSS"
                                                            ValidationGroup="Next"
                                                            Text="Please fill up SSS."
                                                            runat="server" Style="color: red" />
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Passport No.</label>
                                                        <asp:TextBox runat="server" ID="txtPassport" ValidationGroup="Next" ReadOnly="true" class="form-control" type="text" placeholder="12345678" />
                                                        <asp:RequiredFieldValidator
                                                            ID="rfvPass"
                                                            ValidationGroup="Next"
                                                            ControlToValidate="txtPassport"
                                                            Text="Please fill up Pass No."
                                                            runat="server" Style="color: red" />

                                                    </div>
                                                </div>--%>
                                            </div>
                                            <div class="row" runat="server" id="civilstat2">
                                                <%--<div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Passport Valid From</label>--%>
                                                <%--<div class="input-group">--%>
                                                <%--<div class="input-group-addon">
                                                                        <i class="fa fa-calendar"></i>
                                                                    </div>--%>
                                                <%--<asp:TextBox ID="txtPassportValid" runat="server" type="date" Max="9999-12-31" ReadOnly="true"
                                                            class="form-control" />
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator41"
                                                            ValidationGroup="Next"
                                                            ControlToValidate="txtPassportValid"
                                                            Text="Please fill up Passport Valid From"
                                                            runat="server" Style="color: red" />
                                                        <asp:CustomValidator
                                                            Style="color: red"
                                                            runat="server"
                                                            ID="CustomValidator4"
                                                            ValidationGroup="Next"
                                                            ControlToValidate="txtPassportValid"
                                                            Text="Passport Valid From cannot be lower than today."
                                                            OnServerValidate="CustomValidator4_ServerValidate" />--%>
                                                <%--</div>--%>
                                                <%-- </div>
                                                </div>--%>
                                                <%--<div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Passport Valid To</label>--%>
                                                <%--<div class="input-group">--%>
                                                <%--<div class="input-group-addon">
                                                                        <i class="fa fa-calendar"></i>
                                                                    </div>--%>
                                                <%--<asp:TextBox ID="txtPassportValidTo" runat="server" type="date" Max="9999-12-31"
                                                            class="form-control" ReadOnly="true" />
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator45"
                                                            ValidationGroup="Next"
                                                            ControlToValidate="txtPassportValidTo"
                                                            Text="Please fill up Passport Valid To"
                                                            runat="server" Style="color: red" />--%>
                                                <%--<asp:CustomValidator
                                                            Style="color: red"
                                                            runat="server"
                                                            ID="CustomValidator9"
                                                            ValidationGroup="Next"
                                                            ControlToValidate="txtPassportValidTo"
                                                            Text="Passport Valid To cannot be lower than today."
                                                            OnServerValidate="CustomValidator9_ServerValidate" />--%>
                                                <%--</div>--%>
                                                <%--</div>
                                                </div>--%>
                                            </div>
                                            <%--<div class="row" runat="server" id="civilstat3">
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Issued by (Name of Gov't Agency)</label>
                                                        <asp:DropDownList ID="dpIssuedBy" AutoPostBack="true" runat="server" OnSelectedIndexChanged="dpIssuedBy_SelectedIndexChanged" CssClass="form-control">
                                                            <asp:ListItem></asp:ListItem>
                                                            <asp:ListItem Value="---Select Issued By---" Text="---Select Issued By---" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Value="1" Text="DEPARTMENT OF FOREIGN AFFAIRS"> </asp:ListItem>
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Place Issued</label>
                                                        <asp:TextBox runat="server" ID="txtPlaceIssued" ValidationGroup="Next" ReadOnly="true" class="form-control text-uppercase" type="text"
                                                            placeholder="Manila Philippines" />
                                                        <asp:RequiredFieldValidator
                                                            ID="rfvPlaceIssued"
                                                            ValidationGroup="Next"
                                                            ControlToValidate="txtPlaceIssued"
                                                            Text="Please fill up Place Issued."
                                                            runat="server" Style="color: red" />
                                                    </div>
                                                </div>
                                            </div>--%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <%--FOR PARTNERTSHIP AND CORPORATION ONLY--%>
                            <div class="col-md-6" id="contactdetails">
                                <div class="box box-primary">
                                    <div style="background-color: #468DBC; height: 3px;"></div>
                                    <div class="box-header">
                                        <h3 class="box-title">Contact Person Details</h3>
                                    </div>
                                    <div class="box-body">

                                        <div class="col-md-12">




                                            <div class="row">
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>First Name<span class="color-red fsize-16"> *</span></label>
                                                        <div class="input-group" style="width: 100%;">
                                                            <asp:TextBox ID="txtContactFName" type="text" ValidationGroup="Next" class="form-control text-uppercase" runat="server" ReadOnly="true" placeholder="Juan" />
                                                            <asp:RequiredFieldValidator
                                                                ID="RequiredFieldValidator67"
                                                                ControlToValidate="txtContactFName"
                                                                ValidationGroup="Next"
                                                                Text="Please fill up First Name."
                                                                runat="server" Style="color: red" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Middle Name<span class="color-red fsize-16"> *</span></label>
                                                        <asp:TextBox runat="server" ID="txtContactMName" class="form-control text-uppercase" type="text" ValidationGroup="Next" ReadOnly="true" placeholder="D" />
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator68"
                                                            ControlToValidate="txtContactMName"
                                                            ValidationGroup="Next"
                                                            Text="Please fill up Middle Name"
                                                            runat="server" Style="color: red" />
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Last Name<span class="color-red fsize-16"> *</span></label>
                                                        <asp:TextBox runat="server" ID="txtContactLName" ValidationGroup="Next" class="form-control text-uppercase" type="text" ReadOnly="true" placeholder="Dela Cruz" />
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator69"
                                                            ValidationGroup="Next"
                                                            ControlToValidate="txtContactLName"
                                                            Text="Please fill up Last Name."
                                                            runat="server" Style="color: red" />
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Contact Person Position<span class="color-red fsize-16"> *</span></label>
                                                        <asp:TextBox runat="server" ID="txtContactPersonPosition" type="text" placeholder="" class="form-control" />
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator79"
                                                            ControlToValidate="txtContactPersonPosition"
                                                            Text="Please select Position"
                                                            ValidationGroup="Next"
                                                            runat="server" Style="color: red" />
                                                    </div>
                                                </div>
                                                <div class="col-md-6" runat="server" id="contactpersonposition" visible="false" hidden="hidden">
                                                    <div class="form-group">
                                                        <label>Position</label>
                                                        <asp:DropDownList runat="server" ID="ddContactPosition" ReadOnly="true" class="form-control">
                                                            <asp:ListItem Value="---Select Position---" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Value="1" Text="Sales Agent"> </asp:ListItem>
                                                            <asp:ListItem Value="2" Text="Broker"> </asp:ListItem>
                                                            <asp:ListItem Value="3" Text="Manager"> </asp:ListItem>
                                                            <asp:ListItem Value="4" Text="Area Head"> </asp:ListItem>
                                                            <asp:ListItem Value="5" Text="Broker-Agent"> </asp:ListItem>
                                                        </asp:DropDownList>
                                                        <%--                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator70"
                                                            ControlToValidate="ddContactPosition"
                                                            Text="Please select Position"
                                                            InitialValue="---Select Position---"
                                                            ValidationGroup="Next"
                                                            runat="server" Style="color: red" />--%>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Email<span class="color-red fsize-16"> *</span></label>
                                                        <asp:TextBox runat="server" ReadOnly="true" ID="txtContactEmail" type="text" MaxLength="50" placeholder="Juan@gmail.com" class="form-control" />
                                                        <asp:RegularExpressionValidator
                                                            ID="txtContactEmailValidator"
                                                            runat="server"
                                                            ControlToValidate="txtContactEmail"
                                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                            Text="Invalid email address."
                                                            Style="color: red">
                                                        </asp:RegularExpressionValidator>
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator71"
                                                            ControlToValidate="txtContactEmail"
                                                            Text="Please input Email Address"
                                                            ValidationGroup="Next"
                                                            runat="server" Style="color: red" />
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Mobile No<span class="color-red fsize-16"> *</span></label>
                                                        <asp:TextBox runat="server" ReadOnly="true" ID="txtContactMobile" ValidationGroup="Next" class="form-control text-uppercase" oninput="this.value=this.value.slice(0,this.maxLength)" MaxLength="11" type="number" placeholder="09000000000" />
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator72"
                                                            ValidationGroup="Next"
                                                            ControlToValidate="txtContactMobile"
                                                            Text="Please fill up Mobile No."
                                                            runat="server" Style="color: red" />
                                                    </div>
                                                </div>
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label>Address<span class="color-red fsize-16"> *</span></label>
                                                        <asp:TextBox runat="server" ReadOnly="true" ID="txtContactAddress" class="form-control text-uppercase" type="text" ValidationGroup="Next"
                                                            placeholder="Ermita, Manila,1000 Metro Manila" />
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator76"
                                                            ValidationGroup="Next"
                                                            ControlToValidate="txtContactAddress"
                                                            Text="Please fill up Address."
                                                            runat="server" Style="color: red" />
                                                    </div>
                                                </div>




                                                <div class="col-md-6 hidden">
                                                    <div class="form-group">
                                                        <label>Religion</label>
                                                        <asp:TextBox runat="server" AutoPostBack="true" ID="txtCorpReligion" OnTextChanged="txtCorpReligion_TextChanged" ValidationGroup="Next" class="form-control text-uppercase" type="text" />
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">

                                                        <label>Citizenship</label>
                                                        <%--<asp:TextBox runat="server" ID="txtCitizenship" ValidationGroup="Next" class="form-control text-uppercase" type="text" placeholder="Fil" />--%>
                                                        <asp:DropDownList runat="server" AutoPostBack="true" OnSelectedIndexChanged="txtCorpCitizenship_SelectedIndexChanged" ID="txtCorpCitizenship" name="nationality" class="form-control text-uppercase">
                                                            <asp:ListItem Value="filipino">Filipino</asp:ListItem>
                                                            <asp:ListItem Value="afghan">Afghan</asp:ListItem>
                                                            <asp:ListItem Value="albanian">Albanian</asp:ListItem>
                                                            <asp:ListItem Value="algerian">Algerian</asp:ListItem>
                                                            <asp:ListItem Value="american">American</asp:ListItem>
                                                            <asp:ListItem Value="andorran">Andorran</asp:ListItem>
                                                            <asp:ListItem Value="angolan">Angolan</asp:ListItem>
                                                            <asp:ListItem Value="antiguans">Antiguans</asp:ListItem>
                                                            <asp:ListItem Value="argentinean">Argentinean</asp:ListItem>
                                                            <asp:ListItem Value="armenian">Armenian</asp:ListItem>
                                                            <asp:ListItem Value="australian">Australian</asp:ListItem>
                                                            <asp:ListItem Value="austrian">Austrian</asp:ListItem>
                                                            <asp:ListItem Value="azerbaijani">Azerbaijani</asp:ListItem>
                                                            <asp:ListItem Value="bahamian">Bahamian</asp:ListItem>
                                                            <asp:ListItem Value="bahraini">Bahraini</asp:ListItem>
                                                            <asp:ListItem Value="bangladeshi">Bangladeshi</asp:ListItem>
                                                            <asp:ListItem Value="barbadian">Barbadian</asp:ListItem>
                                                            <asp:ListItem Value="barbudans">Barbudans</asp:ListItem>
                                                            <asp:ListItem Value="batswana">Batswana</asp:ListItem>
                                                            <asp:ListItem Value="belarusian">Belarusian</asp:ListItem>
                                                            <asp:ListItem Value="belgian">Belgian</asp:ListItem>
                                                            <asp:ListItem Value="belizean">Belizean</asp:ListItem>
                                                            <asp:ListItem Value="beninese">Beninese</asp:ListItem>
                                                            <asp:ListItem Value="bhutanese">Bhutanese</asp:ListItem>
                                                            <asp:ListItem Value="bolivian">Bolivian</asp:ListItem>
                                                            <asp:ListItem Value="bosnian">Bosnian</asp:ListItem>
                                                            <asp:ListItem Value="brazilian">Brazilian</asp:ListItem>
                                                            <asp:ListItem Value="british">British</asp:ListItem>
                                                            <asp:ListItem Value="bruneian">Bruneian</asp:ListItem>
                                                            <asp:ListItem Value="bulgarian">Bulgarian</asp:ListItem>
                                                            <asp:ListItem Value="burkinabe">Burkinabe</asp:ListItem>
                                                            <asp:ListItem Value="burmese">Burmese</asp:ListItem>
                                                            <asp:ListItem Value="burundian">Burundian</asp:ListItem>
                                                            <asp:ListItem Value="cambodian">Cambodian</asp:ListItem>
                                                            <asp:ListItem Value="cameroonian">Cameroonian</asp:ListItem>
                                                            <asp:ListItem Value="canadian">Canadian</asp:ListItem>
                                                            <asp:ListItem Value="cape verdean">Cape Verdean</asp:ListItem>
                                                            <asp:ListItem Value="central african">Central African</asp:ListItem>
                                                            <asp:ListItem Value="chadian">Chadian</asp:ListItem>
                                                            <asp:ListItem Value="chilean">Chilean</asp:ListItem>
                                                            <asp:ListItem Value="chinese">Chinese</asp:ListItem>
                                                            <asp:ListItem Value="colombian">Colombian</asp:ListItem>
                                                            <asp:ListItem Value="comoran">Comoran</asp:ListItem>
                                                            <asp:ListItem Value="congolese">Congolese</asp:ListItem>
                                                            <asp:ListItem Value="costa rican">Costa Rican</asp:ListItem>
                                                            <asp:ListItem Value="croatian">Croatian</asp:ListItem>
                                                            <asp:ListItem Value="cuban">Cuban</asp:ListItem>
                                                            <asp:ListItem Value="cypriot">Cypriot</asp:ListItem>
                                                            <asp:ListItem Value="czech">Czech</asp:ListItem>
                                                            <asp:ListItem Value="danish">Danish</asp:ListItem>
                                                            <asp:ListItem Value="djibouti">Djibouti</asp:ListItem>
                                                            <asp:ListItem Value="dominican">Dominican</asp:ListItem>
                                                            <asp:ListItem Value="dutch">Dutch</asp:ListItem>
                                                            <asp:ListItem Value="east timorese">East Timorese</asp:ListItem>
                                                            <asp:ListItem Value="ecuadorean">Ecuadorean</asp:ListItem>
                                                            <asp:ListItem Value="egyptian">Egyptian</asp:ListItem>
                                                            <asp:ListItem Value="emirian">Emirian</asp:ListItem>
                                                            <asp:ListItem Value="equatorial guinean">Equatorial Guinean</asp:ListItem>
                                                            <asp:ListItem Value="eritrean">Eritrean</asp:ListItem>
                                                            <asp:ListItem Value="estonian">Estonian</asp:ListItem>
                                                            <asp:ListItem Value="ethiopian">Ethiopian</asp:ListItem>
                                                            <asp:ListItem Value="fijian">Fijian</asp:ListItem>
                                                            <%--<asp:ListItem Value="filipino">Filipino</asp:ListItem>--%>
                                                            <asp:ListItem Value="finnish">Finnish</asp:ListItem>
                                                            <asp:ListItem Value="french">French</asp:ListItem>
                                                            <asp:ListItem Value="gabonese">Gabonese</asp:ListItem>
                                                            <asp:ListItem Value="gambian">Gambian</asp:ListItem>
                                                            <asp:ListItem Value="georgian">Georgian</asp:ListItem>
                                                            <asp:ListItem Value="german">German</asp:ListItem>
                                                            <asp:ListItem Value="ghanaian">Ghanaian</asp:ListItem>
                                                            <asp:ListItem Value="greek">Greek</asp:ListItem>
                                                            <asp:ListItem Value="grenadian">Grenadian</asp:ListItem>
                                                            <asp:ListItem Value="guatemalan">Guatemalan</asp:ListItem>
                                                            <asp:ListItem Value="guinea-bissauan">Guinea-Bissauan</asp:ListItem>
                                                            <asp:ListItem Value="guinean">Guinean</asp:ListItem>
                                                            <asp:ListItem Value="guyanese">Guyanese</asp:ListItem>
                                                            <asp:ListItem Value="haitian">Haitian</asp:ListItem>
                                                            <asp:ListItem Value="herzegovinian">Herzegovinian</asp:ListItem>
                                                            <asp:ListItem Value="honduran">Honduran</asp:ListItem>
                                                            <asp:ListItem Value="hungarian">Hungarian</asp:ListItem>
                                                            <asp:ListItem Value="icelander">Icelander</asp:ListItem>
                                                            <asp:ListItem Value="indian">Indian</asp:ListItem>
                                                            <asp:ListItem Value="indonesian">Indonesian</asp:ListItem>
                                                            <asp:ListItem Value="iranian">Iranian</asp:ListItem>
                                                            <asp:ListItem Value="iraqi">Iraqi</asp:ListItem>
                                                            <asp:ListItem Value="irish">Irish</asp:ListItem>
                                                            <asp:ListItem Value="israeli">Israeli</asp:ListItem>
                                                            <asp:ListItem Value="italian">Italian</asp:ListItem>
                                                            <asp:ListItem Value="ivorian">Ivorian</asp:ListItem>
                                                            <asp:ListItem Value="jamaican">Jamaican</asp:ListItem>
                                                            <asp:ListItem Value="japanese">Japanese</asp:ListItem>
                                                            <asp:ListItem Value="jordanian">Jordanian</asp:ListItem>
                                                            <asp:ListItem Value="kazakhstani">Kazakhstani</asp:ListItem>
                                                            <asp:ListItem Value="kenyan">Kenyan</asp:ListItem>
                                                            <asp:ListItem Value="kittian and nevisian">Kittian and Nevisian</asp:ListItem>
                                                            <asp:ListItem Value="kuwaiti">Kuwaiti</asp:ListItem>
                                                            <asp:ListItem Value="kyrgyz">Kyrgyz</asp:ListItem>
                                                            <asp:ListItem Value="laotian">Laotian</asp:ListItem>
                                                            <asp:ListItem Value="latvian">Latvian</asp:ListItem>
                                                            <asp:ListItem Value="lebanese">Lebanese</asp:ListItem>
                                                            <asp:ListItem Value="liberian">Liberian</asp:ListItem>
                                                            <asp:ListItem Value="libyan">Libyan</asp:ListItem>
                                                            <asp:ListItem Value="liechtensteiner">Liechtensteiner</asp:ListItem>
                                                            <asp:ListItem Value="lithuanian">Lithuanian</asp:ListItem>
                                                            <asp:ListItem Value="luxembourger">Luxembourger</asp:ListItem>
                                                            <asp:ListItem Value="macedonian">Macedonian</asp:ListItem>
                                                            <asp:ListItem Value="malagasy">Malagasy</asp:ListItem>
                                                            <asp:ListItem Value="malawian">Malawian</asp:ListItem>
                                                            <asp:ListItem Value="malaysian">Malaysian</asp:ListItem>
                                                            <asp:ListItem Value="maldivan">Maldivan</asp:ListItem>
                                                            <asp:ListItem Value="malian">Malian</asp:ListItem>
                                                            <asp:ListItem Value="maltese">Maltese</asp:ListItem>
                                                            <asp:ListItem Value="marshallese">Marshallese</asp:ListItem>
                                                            <asp:ListItem Value="mauritanian">Mauritanian</asp:ListItem>
                                                            <asp:ListItem Value="mauritian">Mauritian</asp:ListItem>
                                                            <asp:ListItem Value="mexican">Mexican</asp:ListItem>
                                                            <asp:ListItem Value="micronesian">Micronesian</asp:ListItem>
                                                            <asp:ListItem Value="moldovan">Moldovan</asp:ListItem>
                                                            <asp:ListItem Value="monacan">Monacan</asp:ListItem>
                                                            <asp:ListItem Value="mongolian">Mongolian</asp:ListItem>
                                                            <asp:ListItem Value="moroccan">Moroccan</asp:ListItem>
                                                            <asp:ListItem Value="mosotho">Mosotho</asp:ListItem>
                                                            <asp:ListItem Value="motswana">Motswana</asp:ListItem>
                                                            <asp:ListItem Value="mozambican">Mozambican</asp:ListItem>
                                                            <asp:ListItem Value="namibian">Namibian</asp:ListItem>
                                                            <asp:ListItem Value="nauruan">Nauruan</asp:ListItem>
                                                            <asp:ListItem Value="nepalese">Nepalese</asp:ListItem>
                                                            <asp:ListItem Value="new zealander">New Zealander</asp:ListItem>
                                                            <asp:ListItem Value="ni-vanuatu">Ni-Vanuatu</asp:ListItem>
                                                            <asp:ListItem Value="nicaraguan">Nicaraguan</asp:ListItem>
                                                            <asp:ListItem Value="nigerien">Nigerien</asp:ListItem>
                                                            <asp:ListItem Value="north korean">North Korean</asp:ListItem>
                                                            <asp:ListItem Value="northern irish">Northern Irish</asp:ListItem>
                                                            <asp:ListItem Value="norwegian">Norwegian</asp:ListItem>
                                                            <asp:ListItem Value="omani">Omani</asp:ListItem>
                                                            <asp:ListItem Value="pakistani">Pakistani</asp:ListItem>
                                                            <asp:ListItem Value="palauan">Palauan</asp:ListItem>
                                                            <asp:ListItem Value="panamanian">Panamanian</asp:ListItem>
                                                            <asp:ListItem Value="papua new guinean">Papua New Guinean</asp:ListItem>
                                                            <asp:ListItem Value="paraguayan">Paraguayan</asp:ListItem>
                                                            <asp:ListItem Value="peruvian">Peruvian</asp:ListItem>
                                                            <asp:ListItem Value="polish">Polish</asp:ListItem>
                                                            <asp:ListItem Value="portuguese">Portuguese</asp:ListItem>
                                                            <asp:ListItem Value="qatari">Qatari</asp:ListItem>
                                                            <asp:ListItem Value="romanian">Romanian</asp:ListItem>
                                                            <asp:ListItem Value="russian">Russian</asp:ListItem>
                                                            <asp:ListItem Value="rwandan">Rwandan</asp:ListItem>
                                                            <asp:ListItem Value="saint lucian">Saint Lucian</asp:ListItem>
                                                            <asp:ListItem Value="salvadoran">Salvadoran</asp:ListItem>
                                                            <asp:ListItem Value="samoan">Samoan</asp:ListItem>
                                                            <asp:ListItem Value="san marinese">San Marinese</asp:ListItem>
                                                            <asp:ListItem Value="sao tomean">Sao Tomean</asp:ListItem>
                                                            <asp:ListItem Value="saudi">Saudi</asp:ListItem>
                                                            <asp:ListItem Value="scottish">Scottish</asp:ListItem>
                                                            <asp:ListItem Value="senegalese">Senegalese</asp:ListItem>
                                                            <asp:ListItem Value="serbian">Serbian</asp:ListItem>
                                                            <asp:ListItem Value="seychellois">Seychellois</asp:ListItem>
                                                            <asp:ListItem Value="sierra leonean">Sierra Leonean</asp:ListItem>
                                                            <asp:ListItem Value="singaporean">Singaporean</asp:ListItem>
                                                            <asp:ListItem Value="slovakian">Slovakian</asp:ListItem>
                                                            <asp:ListItem Value="slovenian">Slovenian</asp:ListItem>
                                                            <asp:ListItem Value="solomon islander">Solomon Islander</asp:ListItem>
                                                            <asp:ListItem Value="somali">Somali</asp:ListItem>
                                                            <asp:ListItem Value="south african">South African</asp:ListItem>
                                                            <asp:ListItem Value="south korean">South Korean</asp:ListItem>
                                                            <asp:ListItem Value="spanish">Spanish</asp:ListItem>
                                                            <asp:ListItem Value="sri lankan">Sri Lankan</asp:ListItem>
                                                            <asp:ListItem Value="sudanese">Sudanese</asp:ListItem>
                                                            <asp:ListItem Value="surinamer">Surinamer</asp:ListItem>
                                                            <asp:ListItem Value="swazi">Swazi</asp:ListItem>
                                                            <asp:ListItem Value="swedish">Swedish</asp:ListItem>
                                                            <asp:ListItem Value="swiss">Swiss</asp:ListItem>
                                                            <asp:ListItem Value="syrian">Syrian</asp:ListItem>
                                                            <asp:ListItem Value="taiwanese">Taiwanese</asp:ListItem>
                                                            <asp:ListItem Value="tajik">Tajik</asp:ListItem>
                                                            <asp:ListItem Value="tanzanian">Tanzanian</asp:ListItem>
                                                            <asp:ListItem Value="thai">Thai</asp:ListItem>
                                                            <asp:ListItem Value="togolese">Togolese</asp:ListItem>
                                                            <asp:ListItem Value="tongan">Tongan</asp:ListItem>
                                                            <asp:ListItem Value="trinidadian or tobagonian">Trinidadian or Tobagonian</asp:ListItem>
                                                            <asp:ListItem Value="tunisian">Tunisian</asp:ListItem>
                                                            <asp:ListItem Value="turkish">Turkish</asp:ListItem>
                                                            <asp:ListItem Value="tuvaluan">Tuvaluan</asp:ListItem>
                                                            <asp:ListItem Value="ugandan">Ugandan</asp:ListItem>
                                                            <asp:ListItem Value="ukrainian">Ukrainian</asp:ListItem>
                                                            <asp:ListItem Value="uruguayan">Uruguayan</asp:ListItem>
                                                            <asp:ListItem Value="uzbekistani">Uzbekistani</asp:ListItem>
                                                            <asp:ListItem Value="venezuelan">Venezuelan</asp:ListItem>
                                                            <asp:ListItem Value="vietnamese">Vietnamese</asp:ListItem>
                                                            <asp:ListItem Value="welsh">Welsh</asp:ListItem>
                                                            <asp:ListItem Value="yemenite">Yemenite</asp:ListItem>
                                                            <asp:ListItem Value="zambian">Zambian</asp:ListItem>
                                                            <asp:ListItem Value="zimbabwean">Zimbabwean</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-6 hidden">
                                                    <div class="form-group">
                                                        <%--Issue #114 under Accounting -  Residence No to be adjusted to "Residence Address"--%>
                                                        <label>Residence Address</label>
                                                        <asp:TextBox runat="server" ReadOnly="true" ID="txtContactResidence" class="form-control" ValidationGroup="Next" type="text" placeholder="" />
                                                        <%--<asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator73"
                                                            ValidationGroup="Next"
                                                            ControlToValidate="txtContactResidence"
                                                            Text="Please fill up Residence No."
                                                            runat="server" Style="color: red" />--%>
                                                    </div>
                                                </div>

                                                <div class="col-md-6" runat="server" id="ValidIDContactInfo">
                                                    <div class="form-group">
                                                        <label>Valid ID</label>
                                                        <asp:DropDownList ID="ddContactValidID" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddContactValidID_SelectedIndexChanged" DataTextField="Name" DataValueField="Code" CssClass="form-control">
                                                            <asp:ListItem Value="---Select Valid ID---" Selected="True"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <%--                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator74"
                                                            Enabled="true"
                                                            ControlToValidate="ddContactValidID"
                                                            Text="Please select Valid ID"
                                                            InitialValue="---Select Valid ID---"
                                                            ValidationGroup="Next"
                                                            runat="server" Style="color: red" />--%>
                                                    </div>
                                                </div>

                                                <div class="col-md-3" runat="server" id="OthersContactInfo" visible="false">
                                                    <div class="form-group">
                                                        <label>Specify ID</label>
                                                        <asp:TextBox runat="server" ReadOnly="true" ID="txtOthersContactInfo" ValidationGroup="Next" class="form-control" type="text" placeholder="12345678" />
                                                        <%--                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator14"
                                                            Enabled="False"
                                                            ControlToValidate="txtOthersContactInfo"
                                                            Text="Please Specify ID"
                                                            ValidationGroup="Next"
                                                            runat="server" Style="color: red" />--%>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Valid ID No</label>
                                                        <asp:TextBox runat="server" ReadOnly="true" ID="txtContactValidIDNo" ValidationGroup="Next" class="form-control" type="text" placeholder="12345678" />
                                                        <%--                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator75"
                                                            ValidationGroup="Next"
                                                            ControlToValidate="txtContactValidIDNo"
                                                            Text="Please fill up Valid ID No."
                                                            runat="server" Style="color: red" />--%>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Place Issued<span class="color-red fsize-16"> *</span></label>
                                                        <asp:TextBox runat="server" ReadOnly="true" ID="txtContactPlaceIssued" ValidationGroup="Next" class="form-control text-uppercase" type="text"
                                                            placeholder="Manila Philippines" />
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator77"
                                                            ValidationGroup="Next"
                                                            ControlToValidate="txtContactPlaceIssued"
                                                            Text="Please fill up Place Issued."
                                                            runat="server" Style="color: red" />
                                                    </div>
                                                </div>


                                                <%--2023-07-07 : ADJUSTED FOR BETTER LINING--%>
                                                <%--  <div class="col-md-12">
                                                    <div class="form-group">--%>
                                                <%--<label>&nbsp;</label>--%>
                                                <%--  </div>
                                                </div>--%>
                                                <div class="col-lg-12">
                                                    <hr />
                                                </div>

                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Authorized Representative</label>
                                                        <asp:TextBox runat="server" AutoPostBack="true" OnTextChanged="txtAuthorizedRepresentative2_TextChanged" ID="txtAuthorizedRepresentative2" class="form-control text-uppercase" type="text" placeholder="Juan" />
                                                    </div>
                                                </div>


                                                <%--2023-07-07 : ADDED DESIGNATION FIELD TO AUTOMATE--%>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Designation</label>
                                                        <asp:TextBox runat="server" AutoPostBack="true" ID="txtDesignation2" class="form-control text-uppercase" type="text" placeholder="--" />
                                                    </div>
                                                </div>



                                            </div>



                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="box box-success">
                                    <div class="box-header with-border">
                                        <div style="background-color: darkseagreen; height: 3px;"></div>
                                        <h3 class="box-title">PRC License Information</h3>
                                        <div class="box-tools pull-right">
                                        </div>
                                    </div>
                                    <div class="box-body">

                                        <div class="row">
                                            <div class="col-lg-12">
                                                <div class="form-group">
                                                    <label>Comments: </label>
                                                    <asp:TextBox ID="txtPRCLicenseInformationComments" Style="resize: none;" TextMode="MultiLine" Rows="3" class="form-control" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6 hidden">
                                                <div class="form-group">
                                                    <label>PRC License Registration</label>
                                                    <asp:TextBox runat="server" ID="txtPRCLicenseRegistration" ValidationGroup="Next" class="form-control text-uppercase" type="text" placeholder="REAL ESTATE BROKER" ReadOnly="true" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>PRC Registration Number<%--<span class="color-red fsize-16"> *</span>--%></label>
                                                    <asp:TextBox runat="server" ID="txtPRCRegis" ValidationGroup="Next" AutoPostBack="true" OnTextChanged="txtPRCRegis_TextChanged" class="form-control text-uppercase" ReadOnly="true" type="text" placeholder="1234567" MaxLength="7" onkeypress="return isNumberKey(event)" />
                                                    <%--GAB 06/26/2023 COMMENTED--%>
                                                    <%--<asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator21"
                                                        ValidationGroup="Next"
                                                        ControlToValidate="txtPRCRegis"
                                                        Text="Please fill up PRC Registration Number."
                                                        runat="server" Style="color: red" />--%>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>PRC Registration Date<%--<span class="color-red fsize-16"> *</span>--%></label>
                                                    <div class="input-group" style="width: 100%;">
                                                        <asp:TextBox ID="txtRegistrationDate" class="form-control pull-right"
                                                            type="date" Max="9999-12-31" runat="server" />
                                                        <%--GAB 06/26/2023 COMMENTED--%>
                                                        <%--<asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator14"
                                                            ControlToValidate="txtRegistrationDate"
                                                            ValidationGroup="Next"
                                                            Text="Please provide Registration Date." CssClass="col-md-12"
                                                            runat="server" Style="color: red" />--%>

                                                        <%--GAB 06/30/2023 UNCOMMENTED REASON: ISSUE LOGS--%>
                                                        <asp:CompareValidator ID="CompareValidator21" runat="server"
                                                            ControlToValidate="txtRegistrationDate" ErrorMessage="Please Enter a valid date <br>"
                                                            Operator="DataTypeCheck" Type="Date" ValidationGroup="Next" Style="color: red" />
                                                        <asp:CustomValidator
                                                            Style="color: red"
                                                            runat="server"
                                                            ID="CustomValidator9"
                                                            ValidationGroup="Next"
                                                            ControlToValidate="txtRegistrationDate"
                                                            Text="Registration Date cannot be later than today."
                                                            OnServerValidate="CustomValidator9_ServerValidate" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>PRC Valid Until<%--<span class="color-red fsize-16"> *</span>--%></label>
                                                    <div class="input-group" style="width: 100%;">
                                                        <asp:TextBox ID="txtPRCLicenseExpirationDate" class="form-control pull-right" ReadOnly="true"
                                                            type="date" Max="9999-12-31" runat="server" />
                                                        <%--GAB 06/26/2023 COMMENTED--%>
                                                        <%--<asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator19" CssClass="col-md-12"
                                                            ValidationGroup="Next"
                                                            ControlToValidate="txtPRCLicenseExpirationDate"
                                                            Text="Please fill up Valid Until."
                                                            runat="server" Style="color: red" />--%>
                                                        <asp:CompareValidator ID="CompareValidator4" runat="server"
                                                            ControlToValidate="txtPRCLicenseExpirationDate" ErrorMessage="Please Enter a valid date <br>"
                                                            Operator="DataTypeCheck" Type="Date" ValidationGroup="Next" Style="color: red" />
                                                        <asp:CustomValidator
                                                            Style="color: red"
                                                            runat="server"
                                                            ID="CustomValidator5"
                                                            ValidationGroup="Next"
                                                            ControlToValidate="txtPRCLicenseExpirationDate"
                                                            Text="PRC Expiration Date cannot be earlier than Registration Date."
                                                            OnServerValidate="CustomValidator5_ServerValidate" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>


                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="box box-warning">
                                    <div style="background-color: #EE9B1F; height: 3px;"></div>
                                    <div class="box-header with-border">
                                        <h3 class="box-title">PTR</h3>
                                        <div class="box-tools pull-right">
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>PTR Number<%--<span class="color-red fsize-16"> *</span>--%></label>
                                                    <asp:TextBox runat="server" ID="txtPTRNumber" ValidationGroup="Next" class="form-control pull-left" ReadOnly="true" type="text" placeholder="12345678" />
                                                    <%--COMMENTED 04-04-2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                                                    <%--                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator23"
                                                        ValidationGroup="Next"
                                                        ControlToValidate="txtPTRNumber"
                                                        Text="Please fill up PTR Number."
                                                        runat="server" Style="color: red" />--%>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>PTR From<%--<span class="color-red fsize-16"> *</span>--%></label>
                                                    <div class="input-group" style="width: 100%;">
                                                        <asp:TextBox ID="txtPTRValidFrom" type="date" Max="9999-12-31" runat="server" ReadOnly="true"
                                                            class="form-control pull-right" />
                                                        <%--COMMENTED 04-04-2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                                                        <%--<asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator18"
                                                            ValidationGroup="Next"
                                                            ControlToValidate="txtPTRValidFrom"
                                                            Text="Please fill up PTR Valid From Date." CssClass="col-md-12"
                                                            runat="server" Style="color: red" />--%>
                                                        <asp:CompareValidator ID="CompareValidator5" runat="server"
                                                            ControlToValidate="txtPTRValidFrom" ErrorMessage="Please Enter a valid date <br>"
                                                            Operator="DataTypeCheck" Type="Date" ValidationGroup="Next" Style="color: red" />
                                                        <asp:CustomValidator
                                                            Style="color: red"
                                                            runat="server"
                                                            ID="CustomValidator6"
                                                            ValidationGroup="Next"
                                                            ControlToValidate="txtPTRValidFrom"
                                                            Text="PTR Valid From cannot be later than today."
                                                            OnServerValidate="CustomValidator6_ServerValidate" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>PTR To<%--<span class="color-red fsize-16"> *</span>--%></label>
                                                    <div class="input-group" style="width: 100%;">
                                                        <asp:TextBox ID="txtPTRValidTo" type="date" Max="9999-12-31" runat="server" ReadOnly="true"
                                                            class="form-control" Style="width: 100%;" />
                                                        <%--COMMENTED 04-04-2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                                                        <%--<asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator26"
                                                            ValidationGroup="Next"
                                                            ControlToValidate="txtPTRValidTo" CssClass="col-md-12"
                                                            Text="Please fill up PTR Expiry Date Date."
                                                            runat="server" Style="color: red" />--%>
                                                        <asp:CompareValidator ID="CompareValidator6" runat="server"
                                                            ControlToValidate="txtPTRValidTo" ErrorMessage="Please Enter a valid date <br>"
                                                            Operator="DataTypeCheck" Type="Date" ValidationGroup="Next" Style="color: red" />
                                                        <asp:CustomValidator
                                                            Style="color: red"
                                                            runat="server"
                                                            ID="PTRValidToValidator"
                                                            ValidationGroup="Next"
                                                            ControlToValidate="txtPTRValidTo"
                                                            Text="PTR To cannot be earlier than PTR From."
                                                            OnServerValidate="PTRValidToValidator_ServerValidate" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="box box-primary">
                                    <div style="background-color: #468DBC; height: 3px;"></div>
                                    <div class="box-header with-border">
                                        <h3 class="box-title">AIPO Membership</h3>
                                        <div class="box-tools pull-right">
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>AIPO Organization</label>
                                                    <asp:TextBox runat="server" ID="txtAIPOOrganization" class="form-control text-uppercase" type="text" placeholder="" ReadOnly="true" />
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label>AIPO Receipt No</label>
                                                    <asp:TextBox runat="server" ID="txtAIPOReceiptNo" class="form-control pull-left" type="text" placeholder="12345678" ReadOnly="true" />
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label>AIPO Valid From</label>
                                                    <asp:TextBox ID="txtAIPOValidFrom" type="date" Max="9999-12-31" runat="server"
                                                        class="form-control pull-right" ReadOnly="true" />
                                                    <asp:CompareValidator ID="CompareValidator7" runat="server"
                                                        ControlToValidate="txtAIPOValidFrom" ErrorMessage="Please Enter a valid date <br>"
                                                        Operator="DataTypeCheck" Type="Date" ValidationGroup="Next" Style="color: red" />
                                                    <asp:CustomValidator
                                                        Style="color: red"
                                                        runat="server"
                                                        ID="CustomValidator8"
                                                        ValidationGroup="Next"
                                                        ControlToValidate="txtAIPOValidFrom"
                                                        Text="AIPO Valid From cannot be earlier than today."
                                                        OnServerValidate="CustomValidator8_ServerValidate" />
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label>AIPO Valid To</label>
                                                    <asp:TextBox ID="txtAIPOValidTo" class="form-control pull-right"
                                                        type="date" Max="9999-12-31" runat="server" ReadOnly="true" />
                                                    <asp:CompareValidator ID="CompareValidator8" runat="server"
                                                        ControlToValidate="txtAIPOValidTo" ErrorMessage="Please Enter a valid date <br>"
                                                        Operator="DataTypeCheck" Type="Date" ValidationGroup="Next" Style="color: red" />
                                                    <%--GAB 07/01/2023 Validator for lower AIPO Valid To Date than Valid From--%>
                                                    <asp:CustomValidator
                                                        Style="color: red"
                                                        runat="server"
                                                        ID="AIPOValidToValidator"
                                                        ValidationGroup="Next"
                                                        ControlToValidate="txtAIPOValidTo"
                                                        Text="AIPO Valid To cannot be earlier than AIPO Valid From."
                                                        OnServerValidate="AIPOValidToValidator_ServerValidate" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 pull-right">
                                <%-- validationgroup="Next"--%>
                                <%--<button class="btn btn-primary pull-right" validationgroup="Next" onserverclick="btnNext2_ServerClick" type="button" id="btnNext2" runat="server">Next <i class="fa fa-arrow-right"></i></button>--%>

                                <%--2023-07-12 : SPECIFIED WIDTH TO HAVE UNIFORM SIZE--%>
                                <button class="btn btn-primary pull-right" style="width: 71px;" validationgroup="Next" type="button" id="btnNext2" runat="server"><i class="fa fa-arrow-right"></i></button>
                                <asp:Button class="btn btn-primary" type="button" ID="btnNext2Hidden" Style="display: none;" runat="server" Text="Next Hidden" OnClick="btnNext2_ServerClick" ValidationGroup="Next"></asp:Button>
                                <asp:TextBox ID="txtNext2Hidden" Style="display: none;" runat="server" />
                                <%--<button class="btn btn-primary pull-right" onserverclick="btnNext2_ServerClick" type="button" id="btnNext2" runat="server">Next <i class="fa fa-arrow-right"></i></button>--%>
                                &nbsp;
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
                <asp:Panel runat="server" class="panel panel-primary " ID="step_3" Visible="false">
                    <div class="panel-heading" style="background-color: #5caceb">
                        <h3 class="panel-title">Sales Persons (include yourself)</h3>
                    </div>
                    <div class="panel-body" style="overflow-x: auto">
                        <div class="col-md-12">
                            <button class="btn btn-primary" type="button" visible="true" runat="server" id="btnNewSalesPerson" onserverclick="btnNewSalesPerson_ServerClick"><i class="fa fa-plus"></i>Select Sales Person</button>
                            <asp:GridView ID="gvSalesPerson" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                CssClass="table table-bordered table-hover" Width="100%" ShowHeader="True" PageSize="3" OnPageIndexChanging="gvSalesPerson_PageIndexChanging">
                                <HeaderStyle BackColor="#66ccff" />
                                <Columns>
                                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" ItemStyle-Font-Size="Medium" />
                                    <asp:BoundField DataField="SalesPersonName" HeaderText="Sales Person Name" SortExpression="Sales Person Name" ControlStyle-CssClass="text-uppercase" ItemStyle-Font-Size="Medium" />
                                    <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username" ControlStyle-CssClass="text-uppercase" ItemStyle-Font-Size="Medium" />
                                    <asp:BoundField DataField="EmailAddress" HeaderText="Email Address" SortExpression="Email Address" ItemStyle-Font-Size="Medium" />
                                    <asp:BoundField DataField="Position" HeaderText="Position" SortExpression="Position" ItemStyle-Font-Size="Medium" />
                                    <asp:BoundField DataField="PRCLicense" HeaderText="PRC License/Accreditation No" SortExpression="PRC License/Accreditation No" ItemStyle-Font-Size="Medium" />
                                    <asp:BoundField DataField="PRCLicenseExpirationDate" HeaderText="PRC License Expiry Date" SortExpression="PRC License Expiration Date" ItemStyle-Font-Size="Medium" DataFormatString="{0:MM/dd/yyyy}" />
                                    <asp:BoundField DataField="ATPDate" HeaderText="ATP Date" SortExpression="ATP Date" ItemStyle-Font-Size="Medium" DataFormatString="{0:MM/dd/yyyy}" />
                                    <asp:BoundField DataField="TIN" HeaderText="TIN" SortExpression="TIN" ItemStyle-Font-Size="Medium" />
                                    <asp:BoundField DataField="VATCode" HeaderText="VAT Code" SortExpression="PRC License/Accreditation No" ItemStyle-Font-Size="Medium" ControlStyle-CssClass="text-uppercase" />
                                    <asp:BoundField DataField="WTaxCode" HeaderText="WTAX Code" SortExpression="WTAX Code" ItemStyle-Font-Size="Medium" ControlStyle-CssClass="text-uppercase" />
                                    <asp:BoundField DataField="MobileNumber" HeaderText="Mobile Number" SortExpression="Mobile Number" ItemStyle-Font-Size="Medium" />
                                    <asp:BoundField DataField="ValidFrom" HeaderText="Valid From" SortExpression="Valid From" ItemStyle-Font-Size="Medium" />
                                    <asp:BoundField DataField="ValidTo" HeaderText="Valid To" SortExpression="Valid To" ItemStyle-Font-Size="Medium" />
                                    <asp:BoundField DataField="HLURBLicenseNo" HeaderText="DHSUD License No" SortExpression="HLURB License No" ItemStyle-Font-Size="Medium" />
                                    <asp:BoundField DataField="PTRNo" HeaderText="PTR No" SortExpression="PTR No" ItemStyle-Font-Size="Medium" />
                                    <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center" HeaderText="Lot Only Sharing Details">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="btnAddSharing" class="form-control btn-success"
                                                OnClick="btnAddSharing_Click" Style="color: white;" Text=" Open"
                                                CommandArgument='<%# Eval("Id")%>' CommandName="AddSharing" />
                                            <%-- <button runat="server" id="btnAddSharing" type="button" class="btn btn-default btn-success fa fa-edit" data-toggle="modal"
                                                        data-target="#modal-AddSharing" commandargument='<%# Eval("Id")%>' commandname="addSharing">
                                                        <i class="fa fa-edit"></i>Open</button>--%>
                                            <asp:LinkButton ID="btnSalesPersonView" type="button" runat="server" OnClick="btnSalesPersonView_Click" title="View" class="form-control btn-primary" Text="View" />
                                            <%--<asp:LinkButton ID="btnSalesPersonDelete" type="button" runat="server" OnClick="btnSalesPersonDelete_Click" title="View" class="form-control btn-danger" Text="Delete" />--%>
                                            <asp:LinkButton ID="btnSalesPersonDelete" type="button" runat="server" title="View" class="form-control btn-danger text-center" Text="Delete" CommandArgument='<%# Eval("Id")%>' CommandName="btnSalesPersonDelete_Command" OnCommand="btnSalesPersonDelete_Command" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center" HeaderText="Sharing House And Lot Sharing Details">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="btnAddSharingHouseLot" class="form-control btn-success"
                                                OnClick="btnAddSharingHouseLot_Click" Style="color: white;" Text=" Open"
                                                CommandArgument='<%# Eval("Id")%>' CommandName="AddSharing" />
                                            <%-- <button runat="server" id="btnAddSharing" type="button" class="btn btn-default btn-success fa fa-edit" data-toggle="modal"
                                                        data-target="#modal-AddSharing" commandargument='<%# Eval("Id")%>' commandname="addSharing">
                                                        <i class="fa fa-edit"></i>Open</button>--%>
                                            <asp:LinkButton ID="btnSalesPersonHouseLotView" type="button" runat="server" OnClick="btnSalesPersonView_Click" title="View" class="form-control btn-primary" Text="View" />
                                            <%--<asp:LinkButton ID="btnSalesPersonHouseLotDelete" type="button" runat="server" OnClick="btnSalesPersonDelete_Click" title="View" class="form-control btn-danger" Text="Delete" />--%>
                                            <asp:LinkButton ID="btnSalesPersonHouseLotDelete" type="button" runat="server" title="View" class="form-control btn-danger text-center" Text="Delete" CommandArgument='<%# Eval("Id")%>' CommandName="btnSalesPersonDelete_Command" OnCommand="btnSalesPersonDelete_Command" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="pagination-ys" />
                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                            </asp:GridView>

                        </div>
                    </div>
                    <div class="col-md-4 pull-right">
                        <br />
                        <%--<button class="btn btn-primary nextBtn pull-right" type="button" id="btnNext3" onserverclick="btnNext3_ServerClick1" runat="server"> Next <i class="fa fa-arrow-right"></i></button>--%>
                        <button class="btn btn-primary nextBtn pull-right" type="button" style="width: 71px;" id="btnNext3" runat="server"><i class="fa fa-arrow-right"></i></button>
                        <asp:Button class="btn btn-primary" type="button" ID="btnNext3Hidden" Style="display: none;" runat="server" Text="Next Hidden" OnClick="btnNext3_ServerClick1"></asp:Button>
                        <asp:TextBox ID="txtNext3Hidden" Style="display: none;" runat="server" />

                        <button class="btn btn-danger nextBtn pull-right" style="margin-right: 10px; width: 71px;" type="button" id="btnPrevious2"
                            onserverclick="btnPrevious2_ServerClick" runat="server">
                            <i class="fa fa-arrow-left"></i>
                        </button>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>















    <div class="container-fluid">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:Panel runat="server" class="panel panel-primary " ID="step_4" Visible="false">
                    <div class="panel-heading" style="background-color: #5caceb">
                        <h3 class="panel-title">Attachments</h3>
                    </div>
                    <div class="panel-body">
                        <div class="col-md-12">

                            <asp:Panel runat="server" class="box box-success" ID="Standard">
                                <div class="box-header with-border">
                                    <h3 class="box-title">Standard Attachments</h3>

                                </div>
                                <div class="box-body">

                                    <asp:GridView ID="gvStandardDocumentRequirements" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                        CssClass="table table-hover table-responsive" Width="100%" ShowHeader="True" PageSize="10"
                                        OnRowCommand="gvStandardDocumentRequirements_RowCommand"
                                        GridLines="None">
                                        <HeaderStyle BackColor="#66ccff" />
                                        <Columns>
                                            <%--<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" ItemStyle-Font-Size="Medium" />--%>

                                            <asp:BoundField DataField="DocumentName" HeaderText="Document Name" SortExpression="Document Name" ItemStyle-Font-Size="Medium" HeaderStyle-Width="30%" />


                                            <asp:TemplateField HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" HeaderText="Preview">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <div class="form-group  center-block text-center ">
                                                                <%--<asp:FileUpload ID="FileUpload2" runat="server" onChange="UploadFile(this);" />--%>
                                                                <asp:Label runat="server" ID="lblFileName" Text=""></asp:Label>

                                                                <div class="col-lg-12 center-block center-block" style="margin-left: 5px;">
                                                                    <asp:FileUpload ID="FileUpload1" CssClass="btn btn-default" CommandName="FileUpload1" runat="server" EnableViewState="true" />
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

                                            <asp:TemplateField HeaderStyle-Width="2px" HeaderStyle-HorizontalAlign="Center" HeaderText="Date Issued">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <div class="row">
                                                                <div class="col-lg-12">
                                                                    <div class="form-group">
                                                                        <asp:TextBox ID="txtStandardAttachmentDateIssued" TextMode="Date" Enabled="false" Rows="3" class="form-control" runat="server" />
                                                                        <asp:RequiredFieldValidator
                                                                            ID="rvStandardAttachmentDateIssued"
                                                                            ValidationGroup="NextSave" CssClass="col-md-12"
                                                                            Enabled="false"
                                                                            ControlToValidate="txtStandardAttachmentDateIssued"
                                                                            Text="Please input date issued."
                                                                            runat="server" Style="color: red" />
                                                                        <asp:CompareValidator ID="CompareValidator13" runat="server"
                                                                            ControlToValidate="txtStandardAttachmentDateIssued" ErrorMessage="Please Enter a valid date <br>"
                                                                            Operator="DataTypeCheck" Type="Date" ValidationGroup="NextSave" Style="color: red" />
                                                                        <asp:CustomValidator
                                                                            Style="color: red"
                                                                            runat="server"
                                                                            ID="cvStandardAttachmentDateIssued" CssClass="col-md-12"
                                                                            ValidationGroup="NextSave"
                                                                            ControlToValidate="txtStandardAttachmentDateIssued"
                                                                            Text="Date Issued cannot be later than today."
                                                                            OnServerValidate="cvStandardAttachmentDateIssued_ServerValidate" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-Width="5px" HeaderStyle-HorizontalAlign="Center" HeaderText="Checked">
                                                <HeaderTemplate>
                                                    <div class="form-group text-center">
                                                        <asp:CheckBox ID="chkAllDocs" runat="server" onclick="checkAllDocs(this);" />
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:UpdatePanel runat="server">
                                                        <ContentTemplate>
                                                            <div class="form-group text-center">
                                                                <asp:CheckBox ID="chkStandard" runat="server" CssClass="text-center" />
                                                            </div>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="btnPreview" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center" HeaderText="Comments">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <div class="row">
                                                                <div class="col-lg-12">
                                                                    <div class="form-group">
                                                                        <label>Approver Comments: </label>
                                                                        <asp:TextBox ID="txtStandardAttachmentComments" Style="resize: none;" TextMode="MultiLine" Rows="3" class="form-control" runat="server" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="pagination-ys" />
                                        <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                    </asp:GridView>

                                </div>
                            </asp:Panel>


                            <asp:Panel runat="server" class="box box-success" ID="SoleProp" Visible="false">
                                <div class="box-header with-border">
                                    <h3 class="box-title">Individual (Sole Proprietor)</h3>

                                </div>
                                <div class="box-body">
                                    <asp:GridView ID="gvSolePropDocumentRequirements" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                        CssClass="table table-hover table-responsive" Width="100%" ShowHeader="True" PageSize="10"
                                        OnRowCommand="gvSolePropDocumentRequirements_RowCommand"
                                        GridLines="None">
                                        <HeaderStyle BackColor="#66ccff" />
                                        <Columns>
                                            <%--<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" ItemStyle-Font-Size="Medium" />--%>

                                            <asp:BoundField DataField="DocumentName" HeaderText="Document Name" SortExpression="Document Name" ItemStyle-Font-Size="Medium" HeaderStyle-Width="30%" />



                                            <asp:TemplateField HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" HeaderText="Preview">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <div class="form-group  center-block text-center ">
                                                                <%--<asp:FileUpload ID="FileUpload2" runat="server" onChange="UploadFile(this);" />--%>
                                                                <asp:Label runat="server" ID="lblFileName2" Text=""></asp:Label>
                                                                <div class="col-lg-12 center-block center-block" style="margin-left: 5px;">
                                                                    <asp:FileUpload ID="FileUpload2" CssClass="btn btn-default" CommandName="FileUpload1" runat="server" EnableViewState="true" />
                                                                </div>
                                                                <div class="col-lg-12 center-block center-block">

                                                                    <asp:LinkButton ID="btnUpload2" CssClass="btn btn-primary" runat="server" CommandName="Upload" Text="Upload" />
                                                                    <asp:LinkButton ID="btnPreview2" CssClass="btn btn-info" Visible="false" runat="server" CommandName="Preview" Text="Preview" />
                                                                    <asp:LinkButton ID="btnRemove2" CssClass="btn btn-danger" Visible="false" runat="server" CommandName="Remove" Text="Remove" />
                                                                </div>
                                                            </div>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="btnUpload2" />
                                                            <asp:PostBackTrigger ControlID="btnPreview2" />
                                                            <asp:PostBackTrigger ControlID="btnRemove2" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-Width="2px" HeaderStyle-HorizontalAlign="Center" HeaderText="Date Issued">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <div class="row">
                                                                <div class="col-lg-12">
                                                                    <div class="form-group">
                                                                        <asp:TextBox ID="txtSolePropDocumentRequirementsDateIssued" TextMode="Date" Enabled="false" Rows="3" class="form-control" runat="server" />
                                                                        <asp:RequiredFieldValidator
                                                                            ID="rvSolePropDocumentRequirementsDateIssued"
                                                                            ValidationGroup="NextSave" CssClass="col-md-12"
                                                                            Enabled="false"
                                                                            ControlToValidate="txtSolePropDocumentRequirementsDateIssued"
                                                                            Text="Please input date issued."
                                                                            runat="server" Style="color: red" />
                                                                        <asp:CompareValidator ID="CompareValidator14" runat="server"
                                                                            ControlToValidate="txtSolePropDocumentRequirementsDateIssued" ErrorMessage="Please Enter a valid date <br>"
                                                                            Operator="DataTypeCheck" Type="Date" ValidationGroup="NextSave" Style="color: red" />
                                                                        <asp:CustomValidator
                                                                            Style="color: red"
                                                                            runat="server"
                                                                            ID="cvSolePropDocumentRequirementsDateIssued" CssClass="col-md-12"
                                                                            ValidationGroup="NextSave"
                                                                            ControlToValidate="txtSolePropDocumentRequirementsDateIssued"
                                                                            Text="Date Issued cannot be later than today."
                                                                            OnServerValidate="cvSolePropDocumentRequirementsDateIssued_ServerValidate" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-Width="5px" HeaderStyle-HorizontalAlign="Center" HeaderText="Checked">
                                                <HeaderTemplate>
                                                    <div class="form-group text-center">
                                                        <asp:CheckBox ID="chkAllSole" runat="server" onclick="checkAllSolePartCorp(this);" />
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:UpdatePanel runat="server">
                                                        <ContentTemplate>
                                                            <div class="form-group text-center">
                                                                <asp:CheckBox ID="chkSole" runat="server" CssClass="text-center" />
                                                            </div>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="btnPreview2" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center" HeaderText="Comments">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <div class="row">
                                                                <div class="col-lg-12">
                                                                    <div class="form-group">
                                                                        <label>Approver Comments: </label>
                                                                        <asp:TextBox ID="txtSolePropDocumentRequirementsComments" Style="resize: none;" TextMode="MultiLine" Rows="3" class="form-control" runat="server" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                        </Columns>
                                        <PagerStyle CssClass="pagination-ys" />
                                        <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                    </asp:GridView>

                                </div>
                            </asp:Panel>





















                            <asp:Panel runat="server" class="box box-success" ID="Partnership" Visible="false">
                                <div class="box-header with-border">
                                    <h3 class="box-title">Individual (Partnership)</h3>


                                </div>
                                <div class="box-body">
                                    <asp:GridView ID="gvPartnershipDocumentRequirements" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                        CssClass="table table-hover table-responsive" Width="100%" ShowHeader="True" PageSize="10"
                                        OnRowCommand="gvPartnershipDocumentRequirements_RowCommand"
                                        GridLines="None">
                                        <HeaderStyle BackColor="#66ccff" />
                                        <Columns>
                                            <%--<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" ItemStyle-Font-Size="Medium" />--%>


                                            <asp:BoundField DataField="DocumentName" HeaderText="Document Name" SortExpression="Document Name" ItemStyle-Font-Size="Medium" HeaderStyle-Width="30%" />

                                            <asp:TemplateField HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" HeaderText="Preview">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <div class="form-group  center-block text-center ">
                                                                <%--<asp:FileUpload ID="FileUpload2" runat="server" onChange="UploadFile(this);" />--%>
                                                                <asp:Label runat="server" ID="lblFileName3" Text=""></asp:Label>

                                                                <div class="col-lg-12 center-block center-block" style="margin-left: 5px;">
                                                                    <asp:FileUpload ID="FileUpload3" CssClass="btn btn-default" CommandName="FileUpload1" runat="server" EnableViewState="true" />
                                                                </div>

                                                                <div class="col-lg-12 center-block center-block">
                                                                    <asp:LinkButton ID="btnUpload3" CssClass="btn btn-primary" runat="server" CommandName="Upload" Text="Upload" />
                                                                    <asp:LinkButton ID="btnPreview3" CssClass="btn btn-info" Visible="false" runat="server" CommandName="Preview" Text="Preview" />
                                                                    <asp:LinkButton ID="btnRemove3" CssClass="btn btn-danger" Visible="false" runat="server" CommandName="Remove" Text="Remove" />
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

                                            <asp:TemplateField HeaderStyle-Width="2px" HeaderStyle-HorizontalAlign="Center" HeaderText="Date Issued">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <div class="row">
                                                                <div class="col-lg-12">
                                                                    <div class="form-group">
                                                                        <asp:TextBox ID="txtPartnershipDocumentRequirementsDateIssued" TextMode="Date" Enabled="false" Rows="3" class="form-control" runat="server" />
                                                                        <asp:RequiredFieldValidator
                                                                            ID="rvPartnershipDocumentRequirementsDateIssued"
                                                                            ValidationGroup="NextSave"
                                                                            Enabled="false" CssClass="col-md-12"
                                                                            ControlToValidate="txtPartnershipDocumentRequirementsDateIssued"
                                                                            Text="Please input date issued."
                                                                            runat="server" Style="color: red" />
                                                                        <asp:CompareValidator ID="CompareValidator15" runat="server"
                                                                            ControlToValidate="txtPartnershipDocumentRequirementsDateIssued" ErrorMessage="Please Enter a valid date <br>"
                                                                            Operator="DataTypeCheck" Type="Date" ValidationGroup="NextSave" Style="color: red" />
                                                                        <asp:CustomValidator
                                                                            Style="color: red"
                                                                            runat="server"
                                                                            ID="cvPartnershipDocumentRequirementsDateIssued" CssClass="col-md-12"
                                                                            ValidationGroup="NextSave"
                                                                            ControlToValidate="txtPartnershipDocumentRequirementsDateIssued"
                                                                            Text="Date Issued cannot be later than today."
                                                                            OnServerValidate="cvPartnershipDocumentRequirementsDateIssued_ServerValidate" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-Width="5px" HeaderStyle-HorizontalAlign="Center" HeaderText="Checked">
                                                <HeaderTemplate>
                                                    <div class="form-group text-center">
                                                        <asp:CheckBox ID="chkAllParntner" runat="server" onclick="checkAllSolePartCorp(this);" />
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:UpdatePanel runat="server">
                                                        <ContentTemplate>
                                                            <div class="form-group text-center">
                                                                <asp:CheckBox ID="chkPartner" runat="server" CssClass="text-center" />
                                                            </div>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="btnPreview3" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-Width="10" HeaderStyle-HorizontalAlign="Center" HeaderText="Comments">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <div class="row">
                                                                <div class="col-lg-12">
                                                                    <div class="form-group">
                                                                        <label>Approver Comments: </label>
                                                                        <asp:TextBox ID="txtPartnershipDocumentRequirementsComments" Style="resize: none;" TextMode="MultiLine" Rows="3" class="form-control" runat="server" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                        </Columns>
                                        <PagerStyle CssClass="pagination-ys" />
                                        <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                    </asp:GridView>
                                </div>
                            </asp:Panel>





                            <asp:Panel runat="server" class="box box-success" ID="Corporation" Visible="false">
                                <div class="box-header with-border">
                                    <h3 class="box-title">Corporation</h3>


                                </div>
                                <div class="box-body">
                                    <asp:GridView ID="gvCorporationDocumentRequirements" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                        CssClass="table table-hover table-responsive" Width="100%" ShowHeader="True" PageSize="10"
                                        OnRowCommand="gvCorporationDocumentRequirements_RowCommand" GridLines="None">
                                        <HeaderStyle BackColor="#66ccff" />
                                        <Columns>
                                            <%--<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" ItemStyle-Font-Size="Medium" />--%>


                                            <asp:BoundField DataField="DocumentName" HeaderText="Document Name" SortExpression="Document Name" ItemStyle-Font-Size="Medium" HeaderStyle-Width="30%" />

                                            <asp:TemplateField HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" HeaderText="Preview">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <div class="form-group  center-block text-center ">
                                                                <%--<asp:FileUpload ID="FileUpload2" runat="server" onChange="UploadFile(this);" />--%>
                                                                <asp:Label runat="server" ID="lblFileName4" Text=""></asp:Label>

                                                                <div class="col-lg-12 center-block center-block" style="margin-left: 5px;">
                                                                    <asp:FileUpload ID="FileUpload4" CssClass="btn btn-default" CommandName="FileUpload1" runat="server" EnableViewState="true" />
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

                                            <asp:TemplateField HeaderStyle-Width="2px" HeaderStyle-HorizontalAlign="Center" HeaderText="Date Issued">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <div class="row">
                                                                <div class="col-lg-12">
                                                                    <div class="form-group">
                                                                        <asp:TextBox ID="txtCorporationDocumentRequirementsDateIssued" TextMode="Date" Enabled="false" Rows="3" class="form-control" runat="server" />
                                                                        <asp:RequiredFieldValidator
                                                                            ID="rvCorporationDocumentRequirementsDateIssued"
                                                                            ValidationGroup="NextSave" CssClass="col-md-12"
                                                                            Enabled="false"
                                                                            ControlToValidate="txtCorporationDocumentRequirementsDateIssued"
                                                                            Text="Please input date issued."
                                                                            runat="server" Style="color: red" />
                                                                        <asp:CompareValidator ID="CompareValidator16" runat="server"
                                                                            ControlToValidate="txtCorporationDocumentRequirementsDateIssued" ErrorMessage="Please Enter a valid date <br>"
                                                                            Operator="DataTypeCheck" Type="Date" ValidationGroup="NextSave" Style="color: red" />
                                                                        <asp:CustomValidator
                                                                            Style="color: red"
                                                                            runat="server"
                                                                            ID="cvCorporationDocumentRequirementsDateIssued" CssClass="col-md-12"
                                                                            ValidationGroup="NextSave"
                                                                            ControlToValidate="txtCorporationDocumentRequirementsDateIssued"
                                                                            Text="Date Issued cannot be later than today."
                                                                            OnServerValidate="cvCorporationDocumentRequirementsDateIssued_ServerValidate" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-Width="5px" HeaderStyle-HorizontalAlign="Center" HeaderText="Checked">
                                                <HeaderTemplate>
                                                    <div class="form-group text-center">
                                                        <asp:CheckBox ID="chkAllCorp" runat="server" onclick="checkAllSolePartCorp(this);" />
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:UpdatePanel runat="server">
                                                        <ContentTemplate>
                                                            <div class="form-group text-center">
                                                                <asp:CheckBox ID="chkCorp" runat="server" CssClass="text-center" />
                                                            </div>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="btnPreview4" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center" HeaderText="Comments">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <div class="row">
                                                                <div class="col-lg-12">
                                                                    <div class="form-group">
                                                                        <label>Approver Comments: </label>
                                                                        <asp:TextBox ID="txtCorporationDocumentRequirementsComments" Style="resize: none;" TextMode="MultiLine" Rows="3" class="form-control" runat="server" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                        </Columns>
                                        <PagerStyle CssClass="pagination-ys" />
                                        <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                    </asp:GridView>
                                </div>
                            </asp:Panel>
                            <%--  --%>
                            <%--validationgroup="NextSave"--%>
                            <div class="box box-primary">
                                <div style="background-color: #468DBC; height: 3px;"></div>
                                <div class="box-header">
                                    <h3 class="box-title">Commitment</h3>
                                </div>
                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            I hereby commit to abide by, and/or achieve the following as the basis of my accreditation:
                                                    <br />
                                            <br />
                                            <li>Abide by the Accreditation Agreement and Code of Ethics governing accredited Brokers of ABCI and its assigns;</li>
                                            <li>Attain the required sales production set by the management of ABCI and its assign;</li>
                                            <li>Actively participate in all sales and marketing activities of ABCI and it's assigns</li>
                                            <br />
                                        </div>
                                        <div class="col-md-12" style="display: inline-block; vertical-align: middle;">
                                            <label>
                                                <asp:CheckBox runat="server" AutoPostBack="true" ID="CBconforme" onclick="return false;" Checked="true" />
                                                I understand that failure to satisfy any of the aforementioned condition and any false statements/information herein may be grounds for ABCI and its assins to disapprove my application for accreditation.
                                            </label>
                                            <asp:CustomValidator
                                                Style="color: red"
                                                runat="server"
                                                ID="CustomValidator11" CssClass="col-md-12"
                                                ValidationGroup="SaveBuyer2"
                                                Text="Please confirm all your information as true and correct before proceeding."
                                                OnServerValidate="CustomValidator11_ServerValidate" />
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <asp:TextBox runat="server" ID="txtCommitName" ReadOnly BorderColor="Black" class="form-control text-uppercase" Style="border-style: none none solid none; text-align: center;" type="text" placeholder="Juan" />
                                                <label style="display: block; text-align: center;">SIGNATURE OVER PRINTED NAME</label>
                                            </div>
                                            <asp:RequiredFieldValidator
                                                ID="rvCommitName"
                                                ValidationGroup="NextSave"
                                                ControlToValidate="txtCommitName"
                                                Text="Please input name."
                                                runat="server" Style="color: red" />
                                        </div>
                                        <div class="col-md-1" style="width: 12.499999995%">
                                            <div class="form-group">
                                                <label>&nbsp;</label>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:TextBox runat="server" ID="txtCommitDate" BorderColor="Black" ReadOnly="true" class="form-control text-uppercase" Style="border-style: none none solid none; text-align: center;" type="date" Max="9999-12-31" placeholder="Juan" />
                                                <label style="display: block; text-align: center;">DATE</label>
                                            </div>
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator42"
                                                ControlToValidate="txtCommitDate"
                                                ValidationGroup="NextSave"
                                                Text="Please fill up Date."
                                                runat="server" Style="color: red" />
                                        </div>
                                        <div class="col-md-1" style="width: 12.499999995%">
                                            <div class="form-group">
                                                <label>&nbsp;</label>
                                            </div>
                                        </div>
                                    </div>
                                    <hr style="border: 1px solid black" />
                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Realty Name</label>
                                                <asp:TextBox runat="server" ID="txtRealtyName" class="form-control text-uppercase" type="text" ReadOnly />
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Authorized Representative</label>
                                                <asp:TextBox runat="server" ID="txtAuthorizedRepresentative" class="form-control text-uppercase" type="text" placeholder="Juan" ReadOnly="true" />
                                                <asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator11"
                                                    ControlToValidate="txtAuthorizedRepresentative"
                                                    ValidationGroup="NextSave"
                                                    Text="Please provide Authorized Representative." CssClass="col-md-12"
                                                    runat="server" Style="color: red" />
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Designation</label>
                                                <asp:TextBox runat="server" ID="txtDesignation" class="form-control text-uppercase" type="text" placeholder="--" ReadOnly="true" />
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Affiliation Date</label>
                                                <asp:TextBox ID="txtCommitAffiliationDate" ReadOnly="true" class="form-control pull-right"
                                                    type="date" Max="9999-12-31" runat="server" />
                                                <asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator48"
                                                    ControlToValidate="txtCommitAffiliationDate"
                                                    ValidationGroup="NextSave"
                                                    Text="Please provide Affiliation Date." CssClass="col-md-12"
                                                    runat="server" Style="color: red" />
                                                <asp:CompareValidator ID="CompareValidator22" runat="server"
                                                    ControlToValidate="txtCommitAffiliationDate" ErrorMessage="Please Enter a valid date <br>"
                                                    Operator="DataTypeCheck" Type="Date" ValidationGroup="NextSave" Style="color: red" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <hr />
                            <div class="col-md-4 pull-right">
                                <%--<asp:LinkButton class="btn btn-primary nextBtn pull-right" type="button" ValidationGroup="NextSave" ID="btnSubmitDocument" runat="server" Text="Submit Documents"
                                    OnClick="btnSubmitDocument_Click">Decision
                                    <i class="fa fa-arrow-right"></i></asp:LinkButton>--%>
                                <%--                                <button class="btn btn-primary nextBtn pull-right" type="button" id="btnSubmitDocument" runat="server" text="Submit Documents"
                                    onserverclick="btnSubmitDocument_ServerClick">
                                    Decision
                                    <i class="fa fa-arrow-right"></i>
                                </button>--%>

                                <asp:LinkButton class="btn btn-primary nextBtn pull-right" Style="width: 80px" type="button" ID="btnSubmitDocument" runat="server" Text="Submit Documents" Visible="false" ValidationGroup="NextSave" OnClientClick="getPouchData()"> Decision   <i class="fa fa-arrow-right"></i></asp:LinkButton>
                                <asp:Button class="btn btn-primary nextBtn pull-right" type="button" ID="btnSubmitDocumentHidden" Style="display: none;" runat="server" Text="Submit Documents" OnClick="btnSubmitDocument_Click"></asp:Button>
                                <asp:HiddenField ID="hiddenLabel" runat="server" />
                                <button class="btn btn-danger nextBtn pull-right" style="margin-right: 10px; width: 71px;" type="button" id="btnPrevious3" runat="server" onserverclick="btnPrevious3_ServerClick"><i class="fa fa-arrow-left"></i></button>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
            <%--                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnUpload" />
                    </Triggers>--%>
        </asp:UpdatePanel>
    </div>
</asp:Content>











<asp:Content ID="Content4" ContentPlaceHolderID="modals" runat="server">

    <%--MODAL ALERT--%>
    <div id="modalAlert" class="modal fade" style="z-index: 9999">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="row" style="text-align: center;">
                                <div class="col-lg-12">
                                    <asp:Image ImageUrl="~/assets/img/success.png" runat="server" ID="alertIcon" Width="60" Height="60" />
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
                                    <button type="button" class="btn btn-primary btn-sm" data-dismiss="modal" style="width: 90px;" onclick="HideAlert()">Ok</button>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>



    <%--//TYPE OF BUSINESS--%>
    <div class="modal fade" id="modalBusinessType" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-md">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title">Type of Business</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label>Type of Business</label>
                                            <asp:RadioButtonList ID="rbTypeOfBusiness" runat="server" ValidationGroup="Next" RepeatDirection="Horizontal">
                                                <asp:ListItem Selected="True"> &nbsp; &nbsp; Sole Proprietor&nbsp;&nbsp;&nbsp;</asp:ListItem>
                                                <asp:ListItem>&nbsp;&nbsp;Partnership&nbsp;&nbsp;&nbsp;</asp:ListItem>
                                                <asp:ListItem>&nbsp;&nbsp;Corporation&nbsp;&nbsp;&nbsp;</asp:ListItem>
                                            </asp:RadioButtonList>
                                            <%--<asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator11"
                                                ValidationGroup="Next"
                                                ControlToValidate="rbTypeOfBusiness"
                                                Text="Please select type of business."
                                                runat="server" Style="color: red" />--%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-danger pull-left" data-dismiss="modal">Close</button>
                            <button runat="server" type="button" class="btn btn-primary" id="btnNext" onserverclick="btnNext_ServerClick"><i class="fa fa-plus-circle"></i>Next</button>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>


    <%--ADD SALES PERSON--%>
    <div class="modal fade" id="modalAddSalesPerson" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-sm">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title">Add Sales Person</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-lg-6">
                                    <button class="btn btn-primary" type="button" runat="server" id="btnRegisterSalesAgent" onserverclick="btnRegisterSalesAgent_ServerClick">
                                        <i class="fa fa-plus"></i>
                                        Register Sales Person</button>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12">
                                    <div class="form-group">
                                        <label>Sales Person Name</label><span class="color-red fsize-16"> *</span>
                                        <asp:TextBox runat="server" ID="mtxtID" type="text" Visible="false" />
                                        <div class="input-group">
                                            <asp:TextBox runat="server" ID="mtxtSalesPersonPosition" type="text" ReadOnly="true" placeholder="Juan Dela Cruz" class="form-control text-uppercase" Visible="false" />
                                            <asp:TextBox runat="server" ID="mtxtSalesPersonID" type="text" ReadOnly="true" placeholder="Juan Dela Cruz" class="form-control text-uppercase" Visible="false" />
                                            <asp:TextBox runat="server" ID="mtxtSalesPerson" type="text" ReadOnly="true" Enabled="false" CssClass="form-control text-uppercase disabled" placeholder="Juan Dela Cruz" class="" /><span class="input-group-btn">
                                                <button id="btnListOfSalesPerson" runat="server" style="width: 50px; z-index: auto;" onserverclick="btnListOfSalesPerson_ServerClick" class="btn btn-secondary" type="button"><i class="fa fa-bars"></i></button>
                                            </span>
                                        </div>
                                        <asp:RequiredFieldValidator
                                            ID="RequiredFieldValidator28"
                                            ControlToValidate="mtxtSalesPerson"
                                            ValidationGroup="AddSalesPerson"
                                            Text="Please select Sales Person."
                                            runat="server" Style="color: red" />
                                        <%--<asp:TextBox runat="server" ID="mtxtSalesPerson" type="text" ReadOnly="true" placeholder="Juan Dela Cruz" class="form-control text-uppercase" />--%>
                                    </div>

                                    <div class="form-group">
                                        <label>Valid From</label><span class="color-red fsize-16"> *</span>
                                        <asp:TextBox ID="mtxtValidFrom" type="date" Max="9999-12-31" class="form-control" runat="server" />
                                        <asp:RequiredFieldValidator
                                            ID="RequiredFieldValidator29"
                                            ControlToValidate="mtxtValidFrom"
                                            ValidationGroup="AddSalesPerson"
                                            Text="Please provide Valid From date."
                                            runat="server" Style="color: red" />
                                        <asp:CompareValidator ID="CompareValidator9" runat="server"
                                            ControlToValidate="mtxtValidFrom" ErrorMessage="Please Enter a valid date <br>"
                                            Operator="DataTypeCheck" Type="Date" ValidationGroup="AddSalesPerson" Style="color: red" />
                                        <asp:CustomValidator
                                            Style="color: red"
                                            runat="server"
                                            ID="CustomValidator4"
                                            ValidationGroup="AddSalesPerson"
                                            ControlToValidate="mtxtValidFrom"
                                            Text="Valid From cannot be later than today."
                                            OnServerValidate="CustomValidator4_ServerValidate" />
                                    </div>
                                    <div class="form-group">
                                        <label>Valid To</label><span class="color-red fsize-16"> *</span>
                                        <asp:TextBox ID="mtxtValidTo" type="date" Max="9999-12-31" class="form-control" runat="server" />
                                        <asp:RequiredFieldValidator
                                            ID="RequiredFieldValidator33"
                                            ControlToValidate="mtxtValidTo"
                                            ValidationGroup="AddSalesPerson"
                                            Text="Please provide Valid To date."
                                            runat="server" Style="color: red" />
                                        <asp:CompareValidator ID="CompareValidator10" runat="server"
                                            ControlToValidate="mtxtValidTo" ErrorMessage="Please Enter a valid date <br>"
                                            Operator="DataTypeCheck" Type="Date" ValidationGroup="AddSalesPerson" Style="color: red" />
                                        <%--GAB 07/03/2023 CUSTOM VALIDATION FOR VALID TO--%>
                                        <asp:CustomValidator
                                            Style="color: red"
                                            runat="server"
                                            ID="ValidToValidator"
                                            ValidationGroup="AddSalesPerson"
                                            ControlToValidate="mtxtValidTo"
                                            Text="Valid To cannot be earlier than Valid From."
                                            OnServerValidate="ValidToValidator_ServerValidate" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-danger pull-left" data-dismiss="modal">Close</button>
                            <button runat="server" type="button" validationgroup="AddSalesPerson" class="btn btn-primary" id="btnAddSalesPerson" onserverclick="btnAddSalesPerson_ServerClick">
                                <i class="fa fa-plus-circle"></i>
                                <asp:Label runat="server" ID="lblSaveSalesAgent" Text="Add"></asp:Label>
                            </button>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>




    <%--REGISTER SALES AGENT--%>
    <div class="modal fade" id="modalRegisterSalesAgent" role="dialog" style="z-index: 9999;" aria-labelledby="myModalLabel1" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title">Register Sales Agent</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Sales Person Name</label><span class="color-red fsize-16"> *</span>
                                            <asp:TextBox runat="server" ID="mtxtRegisterSalesPersonName" type="text" placeholder="Juan Dela Cruz" class="form-control text-uppercase" />
                                            <%--<asp:TextBox runat="server" ID="mtxtSalesPerson" type="text" ReadOnly="true" placeholder="Juan Dela Cruz" class="form-control text-uppercase" />--%>
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator17"
                                                ControlToValidate="mtxtRegisterSalesPersonName"
                                                Text="Please input Sales Person Name"
                                                ValidationGroup="RegisterSalesAgent"
                                                runat="server" Style="color: red" />
                                        </div>
                                        <div class="form-group">
                                            <label>Email Address</label><span class="color-red fsize-16"> *</span>
                                            <asp:TextBox runat="server" ID="mtxtEmail" type="text" MaxLength="50" placeholder="Juan@gmail.com" class="form-control" />
                                            <asp:RegularExpressionValidator
                                                ID="mtxtEmailValidator"
                                                runat="server"
                                                ControlToValidate="mtxtEmail"
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                Text="Invalid email address."
                                                Style="color: red">
                                            </asp:RegularExpressionValidator>
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator34"
                                                ControlToValidate="mtxtEmail"
                                                Text="Please input Email Address"
                                                ValidationGroup="RegisterSalesAgent"
                                                runat="server" Style="color: red" />
                                        </div>
                                        <div class="form-group">
                                            <label>Position</label><span class="color-red fsize-16"> *</span>

                                            <asp:DropDownList runat="server" ID="ddPosition" class="form-control">
                                                <asp:ListItem Value="---Select Position---" Text="---Select Position---" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Sales Agent"> </asp:ListItem>
                                                <asp:ListItem Value="2" Text="Broker"> </asp:ListItem>
                                                <asp:ListItem Value="3" Text="Manager"> </asp:ListItem>
                                                <asp:ListItem Value="4" Text="Area Head"> </asp:ListItem>
                                                <asp:ListItem Value="5" Text="Broker-Agent"> </asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator35"
                                                ControlToValidate="ddPosition"
                                                Text="Please select Position"
                                                InitialValue="---Select Position---"
                                                ValidationGroup="RegisterSalesAgent"
                                                runat="server" Style="color: red" />
                                        </div>

                                        <div class="form-group">
                                            <label>VAT Code<span class="color-red fsize-16"> *</span></label>
                                            <asp:DropDownList ID="ddlVATCode2" AutoPostBack="true" OnSelectedIndexChanged="ddlVATCode2_SelectedIndexChanged" DataTextField="Name" DataValueField="Code" runat="server" CssClass="form-control" onchange="setFocusToNextField();" />
                                            <%--GAB COMMENTED 06/29/2023--%>
                                            <%--<asp:DropDownList ID="ddlVATCode2" AutoPostBack="true" DataTextField="Name" DataValueField="Code" runat="server" CssClass="form-control"/>--%>
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator26"
                                                Enabled="true"
                                                ControlToValidate="ddlVATCode2"
                                                Text="Please select VAT Code"
                                                InitialValue="-1"
                                                ValidationGroup="RegisterSalesAgent"
                                                runat="server" Style="color: red" />
                                        </div>
                                        <%--<div class="form-group">
                                                <label>VAT Code</label>
                                                <asp:DropDownList ID="ddlVATCode2" DataTextField="Name" DataValueField="Code" runat="server" CssClass="form-control" />
                                                <%--<asp:TextBox runat="server" ID="mtxtVATCode" type="text" class="form-control text-uppercase" />
                                            </div>--%>
                                        <%--PANG AYOS LANG NG ALIGNMENT SA UI--%>
                                        <%--<asp:CustomValidator
                                                Style="color: red"
                                                runat="server"
                                                ID="CustomValidator17"
                                                ValidationGroup="RegisterSalesAgent"
                                                ControlToValidate="mtxtTIN"
                                                Enabled="false"
                                                Text="Invalid TIN format. Must be 000-000-000-000"
                                                OnServerValidate="CustomValidator15_ServerValidate" />--%>

                                        <div class="form-group">
                                            <label>Withholding Tax Code<span class="color-red fsize-10"> *</span></label>
                                            <asp:DropDownList ID="ddlWTAXCode2" DataTextField="WTName" DataValueField="WTCode" runat="server" CssClass="form-control" />
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator38"
                                                Enabled="true"
                                                ControlToValidate="ddlWTAXCode2"
                                                Text="Please select Withholding Tax Code"
                                                InitialValue="-1"
                                                ValidationGroup="RegisterSalesAgent"
                                                runat="server" Style="color: red" />
                                        </div>

                                        <%--<div class="form-group">
                                                <label>WTax Code</label>
                                                <asp:DropDownList ID="ddlWTAXCode2" DataTextField="WTName" DataValueField="WTCode" runat="server" CssClass="form-control" />
                                                <%--<asp:TextBox runat="server" ID="mtxtWTaxCode" type="text" class="form-control text-uppercase" />--%>
                                        <%--PANG AYOS LANG NG ALIGNMENT SA UI--%>
                                        <%--<asp:CustomValidator
                                                    Style="color: red"
                                                    runat="server"
                                                    ID="CustomValidator22"
                                                    ValidationGroup="RegisterSalesAgent"
                                                    ControlToValidate="mtxtTIN"
                                                    Enabled="false"
                                                    Text="Invalid TIN format. Must be 000-000-000-000"
                                                    OnServerValidate="CustomValidator15_ServerValidate" />
                                            </div>--%>
                                        <div class="form-group">
                                            <label>ATP Expiry Date</label><%--<span class="color-red fsize-16"> *</span>--%>
                                            <asp:TextBox ID="mtxtATPDate" type="date" Max="9999-12-31" class="form-control" runat="server" />
                                            <%--COMMENTED 04-04-2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                                            <%--  <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator40"
                                                ControlToValidate="mtxtATPDate"
                                                Text="Please input ATP Expiry Date"
                                                ValidationGroup="RegisterSalesAgent"
                                                runat="server" Style="color: red" />--%>
                                            <asp:CustomValidator
                                                Style="color: red"
                                                runat="server"
                                                ID="CustomValidator13"
                                                ValidationGroup="RegisterSalesAgent"
                                                ControlToValidate="mtxtATPDate"
                                                Text="ATP Date cannot be earlier than today."
                                                OnServerValidate="CustomValidator13_ServerValidate" />
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Mobile Number </label>
                                            <span class="color-red fsize-16">*</span>
                                            <asp:TextBox runat="server" ID="mtxtMobile" type="number" oninput="this.value=this.value.slice(0,this.maxLength)" MaxLength="11" class="form-control" placeholder="090000000" />
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator37"
                                                ControlToValidate="mtxtMobile"
                                                Text="Please input Mobile Number"
                                                ValidationGroup="RegisterSalesAgent"
                                                runat="server" Style="color: red" />
                                        </div>
                                        <div class="form-group">
                                            <label>TIN</label><span class="color-red fsize-16"> *</span>
                                            <asp:TextBox ID="mtxtTIN" type="text" class="form-control text-uppercase" runat="server" placeholder="000-000-000-000" />
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator36"
                                                ControlToValidate="mtxtTIN"
                                                Text="Please input TIN"
                                                ValidationGroup="RegisterSalesAgent"
                                                runat="server" Style="color: red" />
                                            <asp:CustomValidator
                                                Style="color: red"
                                                runat="server"
                                                ID="CustomValidator15"
                                                ValidationGroup="RegisterSalesAgent"
                                                ControlToValidate="mtxtTIN"
                                                Text="Invalid TIN format. Must be 000-000-000-000"
                                                OnServerValidate="CustomValidator15_ServerValidate" />
                                        </div>
                                        <div class="form-group">
                                            <label>DHSUD License no.</label><span class="color-red fsize-16"> *</span>
                                            <asp:TextBox ID="txtSalesHLURBNo" type="text" class="form-control" runat="server" />
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator45"
                                                ControlToValidate="txtSalesHLURBNo"
                                                Text="Please input DHSUD No"
                                                ValidationGroup="RegisterSalesAgent"
                                                runat="server" Style="color: red" />
                                        </div>
                                        <div class="form-group">
                                            <label>PTR no.</label><%--<span class="color-red fsize-16"> *</span>--%>
                                            <asp:TextBox ID="txtSalesPTRNo" type="text" class="form-control" runat="server" />
                                            <%--COMMENTED 04-04-2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                                            <%--                                                <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator41"
                                                ControlToValidate="txtSalesPTRNo"
                                                Text="Please input PTR no."
                                                ValidationGroup="RegisterSalesAgent"
                                                runat="server" Style="color: red" />--%>

                                            <%--PANG AYOS LANG NG ALIGNMENT SA UI--%>
                                            <asp:CustomValidator
                                                Style="color: red"
                                                runat="server"
                                                ID="CustomValidator22"
                                                ValidationGroup="RegisterSalesAgent"
                                                ControlToValidate="mtxtTIN"
                                                Enabled="false"
                                                Text="Invalid TIN format. Must be 000-000-000-000"
                                                OnServerValidate="CustomValidator15_ServerValidate" />
                                        </div>
                                        <div class="form-group">
                                            <label>PRC License/Accreditation No. </label>
                                            <%--<span class="color-red fsize-16">*</span>--%>
                                            <asp:TextBox runat="server" ID="mtxtPRCLicense" MaxLength="7" onkeypress="return isNumberKey(event)" type="text" placeholder="12345678" class="form-control" />
                                            <%--COMMENTED 04-04-2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                                            <%--       <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator38"
                                                ControlToValidate="mtxtPRCLicense"
                                                Text="Please input PRC License/Accreditation No."
                                                ValidationGroup="RegisterSalesAgent"
                                                runat="server" Style="color: red" />--%>

                                            <%--PANG AYOS LANG NG ALIGNMENT SA UI--%>
                                            <asp:CustomValidator
                                                Style="color: red"
                                                runat="server"
                                                ID="CustomValidator23"
                                                ValidationGroup="RegisterSalesAgent"
                                                ControlToValidate="mtxtTIN"
                                                Enabled="false"
                                                Text="Invalid TIN format. Must be 000-000-000-000"
                                                OnServerValidate="CustomValidator15_ServerValidate" />
                                        </div>
                                        <div class="form-group">
                                            <label>PRC/ Accreditation Expiry Date </label>
                                            <%--<span class="color-red fsize-16">*</span>--%>
                                            <asp:TextBox ID="mtxtPRCLicenseExpirationDate" type="date" Max="9999-12-31" class="form-control" runat="server" />
                                            <%--COMMENTED 04-04-2023 : CHANGE REQUEST  https://docs.google.com/spreadsheets/d/1RV6bJiLx9xIdnBVTYVX4B3B8VHLhGni2/edit?pli=1#gid=379127910 --%>
                                            <%--  <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator39"
                                                ControlToValidate="mtxtPRCLicenseExpirationDate"
                                                Text="Please input PRC/ Accreditation Expiration Date" CssClass="col-md-12"
                                                ValidationGroup="RegisterSalesAgent"
                                                runat="server" Style="color: red" />--%>
                                            <asp:CompareValidator ID="CompareValidator12" runat="server"
                                                ControlToValidate="mtxtPRCLicenseExpirationDate" ErrorMessage="Please Enter a valid date <br>"
                                                Operator="DataTypeCheck" Type="Date" ValidationGroup="RegisterSalesAgent" Style="color: red" />
                                            <asp:CustomValidator
                                                Style="color: red"
                                                runat="server" CssClass="col-md-12"
                                                ID="CustomValidator12"
                                                ValidationGroup="RegisterSalesAgent"
                                                ControlToValidate="mtxtPRCLicenseExpirationDate"
                                                Text="PRC/ Accreditation Expiration Date cannot be earlier than today."
                                                OnServerValidate="CustomValidator12_ServerValidate" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <%--<button type="button" class="btn btn-danger pull-left" data-dismiss="modal">Close</button>
                            <button runat="server" type="button" class="btn btn-primary" id="btnRegisterdSalesAgent" validationgroup="RegisterSalesAgent" onserverclick="btnRegisterdSalesAgent_ServerClick">
                                <i class="fa fa-plus-circle"></i>
                                <asp:Label runat="server" ID="Label1" Text="Add"></asp:Label>
                            </button>--%>

                            <button runat="server" type="button" class="btn btn-danger pull-left" data-dismiss="modal" onserverclick="btnCloseSalesAgent_ServerClick">Close</button>
                            <button runat="server" type="button" validationgroup="RegisterSalesAgent" class="btn btn-primary" id="Button1" onserverclick="btnRegisterdSalesAgent_ServerClick">
                                <i class="fa fa-plus-circle"></i>
                                <asp:Label runat="server" ID="Label1" Text="Add"></asp:Label>
                            </button>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>















    <%--ADD SHARING VIEW--%>
    <div class="modal fade" id="modalAddSharing" data-backdrop="static" data-keyboard="false" style="overflow-y: scroll;">
        <div class="modal-dialog modal-lg">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="modal-content">
                        <div class="modal-header">
                            <%--                            <button type="button" class="close" runat="server" onserverclick="btnCloseSharingDetails_ServerClick" validationgroup="CloseSharing">
                                <span>&times;</span></button>--%>
                            <%--                            <button type="button" class="close" data-dismiss="modal" aria-label="Close" validationgroup="CloseSharing">
                                <span aria-hidden="true">&times;</span></button>--%>
                            <h4 class="modal-title">Sharing Details</h4>
                        </div>
                        <div class="modal-body">
                            <button class="btn btn-primary" type="button" runat="server" id="btnAddSharingDetails" onserverclick="btnAddSharingDetails_ServerClick">
                                <i class="fa fa-plus"></i>Add Row</button><br />
                            <asp:CustomValidator
                                Style="color: red"
                                runat="server"
                                Enabled="true"
                                ID="CustomValidator7"
                                ValidationGroup="CloseSharing"
                                CssClass="col-md-12 text-right"
                                Text="Sum should be 7% for Lot Only and 5% for House and Lot."
                                OnServerValidate="CustomValidator7_ServerValidate" />
                            <%--GAB 5/13/2023 Commission % input--%>
                            <div class="form-group row">
                                <label for="CommPercent" class="col-sm-9 col-form-label text-right">Commission Percentage</label>
                                <div class="col-sm-3">
                                    <%--<input type="text" class="form-control-plaintext" id="CommPercent" autocomplete="off" value="0">--%>
                                    <asp:TextBox runat="server" ID="CommPercent" CssClass="form-control-plaintext" ClientIDMode="Static"></asp:TextBox>
                                </div>
                            </div>

                            <asp:GridView ID="gvShareDetails" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                CssClass="table table-bordered table-hover" Width="100%" ShowHeader="True" PageSize="100" OnRowCommand="gvShareDetails_RowCommand"
                                OnPageIndexChanging="gvShareDetails_PageIndexChanging">
                                <HeaderStyle BackColor="#66ccff" />
                                <Columns>
                                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" ItemStyle-Font-Size="Medium" />
                                    <asp:BoundField DataField="SalesPersonId" HeaderText="Sales Person Id" SortExpression="Sales Person Id" ItemStyle-Font-Size="Medium" />
                                    <asp:BoundField DataField="Position" HeaderText="Position" SortExpression="Position" ItemStyle-Font-Size="Medium" />
                                    <asp:BoundField DataField="SalesPersonName" HeaderText="Sales Person Name" SortExpression="Sales Person Name No" ItemStyle-Font-Size="Medium" />
                                    <asp:BoundField DataField="LotOnlyPercentage" HeaderText="Lot Only Percentage" SortExpression="Percentage" ItemStyle-Font-Size="Medium" />
                                    <asp:BoundField DataField="HouseandLotPercentage" HeaderText="House And Lot Percentage" SortExpression="Percentage" ItemStyle-Font-Size="Medium" />
                                    <asp:BoundField DataField="OslaID" HeaderText="OslaID" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" SortExpression="OslaID" ItemStyle-Font-Size="Medium" />
                                    <asp:BoundField DataField="TransID" HeaderText="TransID" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" SortExpression="TransID" ItemStyle-Font-Size="Medium" />
                                    <%--<asp:BoundField DataField="ValidFrom" HeaderText="Valid From" SortExpression="Valid From" ItemStyle-Font-Size="Medium" />
                                    <asp:BoundField DataField="ValidTo" HeaderText="Valid To" SortExpression="Valid To" ItemStyle-Font-Size="Medium" />--%>
                                    <%--<asp:BoundField DataField="Amount?" HeaderText="Amount?" SortExpression="Amount?" ItemStyle-Font-Size="Medium" />--%>

                                    <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center" HeaderText="Action">
                                        <ItemTemplate>
                                            <%-- <button runat="server" type="button" class="btn btn-default btn-success" id="btnEdit"><i class="fa fa-eye"></i></button>--%>
                                            <asp:LinkButton ID="btnView" type="button" runat="server" OnClick="btnView_Click" title="View" class="form-control btn-primary text-center" Text="Edit" CommandName="View" />
                                            <asp:LinkButton ID="btnDelete" type="button" runat="server" OnClick="btnDelete_Click" title="View" class="form-control btn-danger text-center" Text="Delete" CommandName="Delete" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="pagination-ys" />
                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                            </asp:GridView>
                            <%-- 05/13/2023 Added Project Table--%>
                            <asp:GridView ID="gvProjectList" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                CssClass="table table-bordered table-hover" Width="100%" ShowHeader="True" PageSize="10" OnPageIndexChanging="gvProjectList_PageIndexChanging">
                                <HeaderStyle BackColor="#66ccff" />
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkAll" runat="server" onclick="checkAll(this);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" Enabled="false" data-projcode='<%# Eval("ProjectCode") %>' Value='<%# Eval("ProjectCode") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ProjectCode" HeaderText="Project Code" SortExpression="Project Code" ItemStyle-Font-Size="Medium" />
                                    <asp:BoundField DataField="ProjectName" HeaderText="Project Name" SortExpression="Project Name" ItemStyle-Font-Size="Large" />
                                    <asp:BoundField DataField="Commission" HeaderText="Commission" SortExpression="Commission" ItemStyle-Font-Size="Medium" DataFormatString="{0:#,##0.00}" />
                                </Columns>
                                <PagerStyle CssClass="pagination-ys" />
                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                            </asp:GridView>





                            <%-- 05/13/2023 Added Selected Sharing Details--%>
                            <div class="text-center" style="padding-top: 10px; padding-bottom: 20px;">
                                <%--<button type="button" runat="server" id="btnAddToSharing" class="btn btn-success text-center" style="margin-right: 20px;" onserverclick="btnAddToSharing_ServerClick" disabled="disabled"><i class="fa fa-arrow-down"></i>  Add</button>--%>
                                <%--<button type="button" runat="server" id="btnBackToProjList" class="btn btn-info text-center" onserverclick="btnBackToProjList_ServerClick"><i class="fa fa-arrow-up"></i>  Back</button>--%>

                                <button type="button" runat="server" id="btnAddToSharing" class="btn btn-success text-center" style="margin-right: 20px;" disabled="disabled"><i class="fa fa-arrow-down"></i>Add</button>
                                <asp:Button class="btn btn-primary text-center" type="button" ID="BtnPouchDbDataViewHidden" Style="display: none;" runat="server" Text="PouchDbData View" OnClick="BtnPouchDbDataViewHidden_ServerClick"></asp:Button>
                                <asp:HiddenField ID="txtPouchDbDataViewHidden" runat="server" />

                                <button type="button" runat="server" id="btnBackToProjList" class="btn btn-info text-center"><i class="fa fa-arrow-up"></i>Back</button>
                                <asp:Button class="btn btn-primary text-center" type="button" ID="BtnPouchDbDataDeleteHidden" Style="display: none;" runat="server" Text="PouchDbData Delete" OnClick="btnBackToProjList_ServerClick"></asp:Button>
                                <asp:HiddenField ID="txtPouchDbDataDeleteHidden" runat="server" />
                            </div>






                            <div style="height: 400px; overflow-y: scroll;">
                                <asp:GridView ID="gvSelectedSharingDetails" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                    CssClass="table table-bordered table-hover" Width="100%" ShowHeader="True" PageSize="100" OnPageIndexChanging="gvSelectedSharingDetails_PageIndexChanging"
                                    DataKeyNames="OslaID, SalesPersonId, PositionSharedDetails, _id">
                                    <HeaderStyle BackColor="#66ccff" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkAll" runat="server" onclick="checkAllBack(this);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox1" runat="server" onclick='<%# "checkSame(\"" + Eval("ProjectCode") + "\",\"" + Eval("CommissionPercentage") + "\"); storeCheckedItems(this);" %>' data-projcode='<%# Eval("ProjectCode") %>' Value='<%# Eval("ProjectCode") %>' />
                                                <%--<asp:CheckBox ID="CheckBox1" runat="server" onclick='<%# "checkSame(\"" + Eval("ProjectCode") + "\",\"" + Eval("CommissionPercentage") + "\");" %>' data-projcode='<%# Eval("ProjectCode") %>' Value='<%# Eval("ProjectCode") %>' />--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ProjectCode" HeaderText="Project Code" SortExpression="ProjectCode" ItemStyle-Font-Size="Medium" />
                                        <asp:BoundField DataField="ProjectName" HeaderText="Project Name" SortExpression="ProjectName" ItemStyle-Font-Size="Large" />
                                        <asp:BoundField DataField="PercentageSharedDetails" HeaderText="Lot Commission" SortExpression="LotCommission" ItemStyle-Font-Size="Medium" />
                                        <asp:BoundField DataField="HouseandLotSharingDetails" HeaderText="House & Lot Commission" SortExpression="HouseandLotSharingDetails" ItemStyle-Font-Size="Medium" />
                                        <asp:BoundField DataField="SalesPersonNameSharedDetails" HeaderText="Sales Person" SortExpression="SalesPerson" ItemStyle-Font-Size="Medium" />
                                        <asp:BoundField DataField="CommissionPercentage" HeaderText="Share" SortExpression="Share" ItemStyle-Font-Size="Medium" />
                                        <asp:BoundField DataField="OslaID" HeaderText="OslaID" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" SortExpression="OslaID" ItemStyle-Font-Size="Medium" />
                                        <asp:BoundField DataField="SalesPersonId" HeaderText="SalesPersonId" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" SortExpression="SalesPersonId" ItemStyle-Font-Size="Medium" />
                                        <asp:BoundField DataField="PositionSharedDetails" HeaderText="Position" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" SortExpression="Position" ItemStyle-Font-Size="Medium" />
                                        <asp:BoundField DataField="_id" HeaderText="_id" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" SortExpression="_id" ItemStyle-Font-Size="Medium" />
                                    </Columns>
                                    <PagerStyle CssClass="pagination-ys" />
                                    <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                </asp:GridView>
                            </div>
                            <asp:HiddenField ID="hfCheckedItems" runat="server" Value="" />
                        </div>
                        <div class="modal-footer">
                            <%--<button type="button" runat="server" id="btnCloseSharingDetails" class="btn btn-danger pull-right text-center" onserverclick="btnCloseSharingDetails_ServerClick" validationgroup="CloseSharing">Close</button>--%>
                            <button type="button" runat="server" id="btnCloseSharingDetails" class="btn btn-danger pull-right text-center" onserverclick="btnCloseSharingDetails_ServerClick">Close</button>
                            <%--<button type="button" class="btn btn-danger pull-right" data-dismiss="modal">Close</button>--%>
                            <%--<button type="button" class="btn btn-primary"><i class="fa fa-plus"></i> Add</button>--%>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>


    <%--ADD SHARING DETAILS--%>
    <div class="modal fade" id="modalAddSharingDetails" style="overflow-y: scroll;">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog modal-sm">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title">Add Sharing Details</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-lg-6">
                                    <button class="btn btn-primary" type="button" runat="server" id="btnRegisterSalesPerson2" onserverclick="btnRegisterSalesAgent_ServerClick">
                                        <%--onclick="HideAddSharingDetailsModal(); HideAddSharingModal();"--%>
                                        <i class="fa fa-plus"></i>
                                        Register Sales Person</button>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <asp:TextBox runat="server" ID="mtxtID2" type="text" Visible="false" />
                                            <label>Sales Person Name</label>
                                            <div class="input-group">
                                                <asp:TextBox runat="server" ID="mtxtSalesPersonShareID" type="text" ReadOnly="true" placeholder="Juan Dela Cruz" class="form-control text-uppercase" Visible="false" />
                                                <asp:TextBox runat="server" ID="mtxtSalesPersonShare" type="text" ReadOnly="true" placeholder="Juan Dela Cruz" class="form-control text-uppercase" />
                                                <span class="input-group-btn">
                                                    <button id="btnListOfSalesPersonSharingDetails" runat="server" style="width: 50px; z-index: auto;" onserverclick="btnListOfSalesPerson_ServerClick" class="btn btn-secondary" type="button"><i class="fa fa-bars"></i></button>
                                                </span>
                                            </div>
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator30"
                                                ControlToValidate="mtxtSalesPersonShare"
                                                Text="Please select Sales Person."
                                                ValidationGroup="SharedDetails"
                                                runat="server" Style="color: red" />
                                        </div>
                                        <div class="form-group">
                                            <label>Position</label>
                                            <asp:DropDownList runat="server" ID="mddPositionShare" class="form-control" disabled>
                                                <asp:ListItem Value="---Select Position---" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Sales Agent"> </asp:ListItem>
                                                <asp:ListItem Text="Broker"> </asp:ListItem>
                                                <asp:ListItem Text="Manager"> </asp:ListItem>
                                                <asp:ListItem Text="Area Head"> </asp:ListItem>
                                                <asp:ListItem Text="Broker-Agent"> </asp:ListItem>

                                            </asp:DropDownList>
                                        </div>
                                        <asp:RequiredFieldValidator
                                            ID="RequiredFieldValidator31"
                                            ControlToValidate="mddPositionShare"
                                            Text="Please select Position."
                                            ValidationGroup="SharedDetails"
                                            runat="server" Style="color: red" />
                                        <%--<div class="form-group">
                                                <label>Sales Person Name</label>
                                                <asp:TextBox runat="server" ID="mtxtSalesPersonShare" type="text" placeholder="Juan Dela Cruz" class="form-control text-uppercase" />
                                            </div>--%>
                                        <div class="form-group" id="sharingLotOnly" runat="server">
                                            <label>Lot Only Percentage</label>
                                            <%--GAB 07/02/2023 COMMENTED Reason: Google Chrome autocompletes both mtxtPecent and mtxtHouseAndLotPecent textboxes when accidentally clicked which results into an error--%>
                                            <%--<asp:TextBox runat="server" ID="mtxtPecent" type="text" placeholder="7%" class="form-control" />--%>
                                            <asp:TextBox runat="server" ID="mtxtPecent" type="number" placeholder="7%" class="form-control" />
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator32"
                                                ControlToValidate="mtxtPecent"
                                                Text="Please select Lot Only Percentage."
                                                ValidationGroup="SharedDetails"
                                                runat="server" Style="color: red" />
                                        </div>
                                        <div class="form-group" id="sharingHouseAndLot" runat="server">
                                            <label>House and Lot Percentage</label>
                                            <%--GAB 07/02/2023 COMMENTED Reason: Google Chrome autocompletes both mtxtPecent and mtxtHouseAndLotPecent textboxes when accidentally clicked which results into an error--%>
                                            <%--<asp:TextBox runat="server" ID="mtxtHouseAndLotPecent" type="text" placeholder="5%" class="form-control" />--%>
                                            <asp:TextBox runat="server" ID="mtxtHouseAndLotPecent" type="number" placeholder="5%" class="form-control" />
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator15"
                                                ControlToValidate="mtxtHouseAndLotPecent"
                                                Text="Please select House and Lot Percentage."
                                                ValidationGroup="SharedDetails"
                                                runat="server" Style="color: red" />
                                        </div>
                                    </div>
                                    <%--<div class="col-md-12">--%>

                                    <%--     <div class="form-group">
                                                <label>Valid From</label>
                                                <asp:TextBox ID="mtxtValidFrom" type="date" Max="9999-12-31" class="form-control" runat="server" />
                                            </div>
                                            <div class="form-group">
                                                <label>Valid To</label>
                                                <asp:TextBox ID="mtxtValidTo" type="date" Max="9999-12-31" class="form-control" runat="server" />
                                            </div>--%>
                                    <%--  <div class="form-group">
                                        <label>Amount? </label>
                                        <asp:TextBox runat="server" ID="mtxtAmount" type="text" class="form-control" placeholder="10,000" />
                                    </div>--%>
                                    <%--</div>--%>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-danger pull-left" data-dismiss="modal">
                                Close</button>
                            <button class="btn btn-primary" type="button" id="btnSaveSharingDetails" validationgroup="SharedDetails" runat="server" onserverclick="btnSaveSharingDetails_ServerClick">
                                <i class="fa fa-save"></i>Save</button>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

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
                                    <button type="button" class="btn btn-primary btn-sm" data-dismiss="modal" style="width: 90px;" onclick="window.location.href='/pages/BrokerApprove.aspx'">Ok</button>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>




    <%--MODAL CONFIRMATION--%>
    <div id="modalConfirmation" class="modal fade" tabindex="-1" data-focus-on="input:first">
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
                                    <asp:LinkButton runat="server" ID="btnYes" CssClass="btn btn-info btn-sm" Text="Yes" Style="width: 90px;"
                                        OnClick="btnYes_Click" />
                                    <button type="button" class="btn btn-default btn-sm" data-dismiss="modal" style="width: 90px;" onclick="hideConfirm()">No</button>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>


    <%--MODAL LIST OF BROKERS--%>
    <div class="modal fade" id="ModalBrokerList" role="dialog" tabindex="-1">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog modal-lg" role="document" style="width: 75%">
                    <div class="modal-content">
                        <div class="modal-header bg-color">
                            <button type="button" class="close" runat="server" onserverclick="btnClose_ServerClick" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                            <h4 class="modal-title">Choose Broker
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
                                                    <input runat="server" id="txtSearch" placeholder="Search here" class="form-control autofocus" type="text" /><span class="input-group-btn">
                                                        <asp:LinkButton runat="server" ID="bSearch" Style="width: 90px;" CssClass="btn btn-info btn-default btn-facebook" OnClick="bSearch_Click">Find<i class="fa fa-search"></i></asp:LinkButton>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <fieldset>
                                                    <asp:GridView ID="gvBrokerList" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                                        CssClass="table table-bordered table-hover" Width="100%" ShowHeader="True" PageSize="5" OnPageIndexChanging="gvBrokerList_PageIndexChanging">
                                                        <HeaderStyle BackColor="#66ccff" />
                                                        <Columns>
                                                            <asp:BoundField DataField="BrokerId" HeaderText="Broker ID" SortExpression="BrokerId" ItemStyle-Font-Size="Medium" />
                                                            <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-CssClass="text-uppercase" SortExpression="Name" ItemStyle-Font-Size="Medium" />
                                                            <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-CssClass="text-uppercase" SortExpression="Status" ItemStyle-Font-Size="Medium" />
                                                            <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="bSelectBuyer" CssClass="btn btn-default btn-success" Width="100%" Height="100%" OnClick="bSelectBuyer_Click" CommandArgument='<%# Bind("BrokerId")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
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
                            <button runat="server" style="width: 100px;" onserverclick="btnClose_ServerClick" class="btn btn-danger" type="button" data-dismiss="modal">Close  </button>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>


    <%--// TYPE OF APPROVAL --%>
    <div class="modal fade" id="modalApprovalType" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-md">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title">Type of Approval</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label>Type of Approval</label>
                                            <asp:RadioButtonList ID="rbApprovalType" runat="server" ValidationGroup="Next" RepeatDirection="Horizontal">
                                                <asp:ListItem Selected="True"> &nbsp; &nbsp; Rejected &nbsp;&nbsp;&nbsp;</asp:ListItem>
                                                <asp:ListItem>&nbsp;&nbsp;For Revision&nbsp;&nbsp;&nbsp;</asp:ListItem>
                                                <asp:ListItem>&nbsp;&nbsp;Accepted&nbsp;&nbsp;&nbsp;</asp:ListItem>
                                            </asp:RadioButtonList>
                                            <%--<asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator27"
                                                ValidationGroup="Next"
                                                ControlToValidate="rbApprovalType"
                                                Text="Please select type of approval."
                                                runat="server" Style="color: red" />--%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-danger pull-left" data-dismiss="modal">Close</button>
                            <button runat="server" type="button" class="btn btn-primary" id="btnApprovalType" onserverclick="btnApprovalType_ServerClick">Submit   <i class="fa fa-plus-circle"></i></button>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>



    <div id="MsgListOfSalesPersons" class="modal fade" role="dialog">
        <div class="modal-dialog modal-lg">
            <!-- Modal content-->
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Sales Persons List</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-lg-2 col-md-2 col-sm-6 col-xs-12">
                                    <h5>Search: </h5>
                                </div>
                                <div class="col-lg-10 col-md-10 col-sm-6 col-xs-12">
                                    <div class="input-group">
                                        <input runat="server" id="txtSalesSearch" placeholder="Search here" class="form-control autofocus" type="text" /><span class="input-group-btn">
                                            <%--<asp:LinkButton runat="server" ID="bSalesSearch" Style="width: 50px;" CssClass="btn btn-secondary btn-default btn-facebook" OnClick="bSalesSearch_Click"><i class="fa fa-search"></i></asp:LinkButton>--%>
                                            <button type="submit" runat="server" id="bSalesSearch" style="width: 90px;" class="btn btn-info btn-default btn-facebook" onserverclick="bSalesSearch_Click">Find   <i class="fa fa-search"></i></button>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="overflow: auto;">
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="gvSalesPersons" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                                CssClass="table table-bordered table-hover" Width="100%" ShowHeader="True" PageSize="5" OnPageIndexChanging="gvSalesPersons_PageIndexChanging">
                                                <HeaderStyle BackColor="#66ccff" />
                                                <Columns>
                                                    <asp:BoundField DataField="Id" HeaderText="Id" />
                                                    <asp:BoundField DataField="SalesPerson" HeaderText="SalesPerson Name" ItemStyle-CssClass="text-uppercase" />
                                                    <asp:BoundField DataField="Position" HeaderText="Position" />
                                                    <asp:BoundField DataField="PRCLicense" HeaderText="PRC License" />
                                                    <asp:BoundField DataField="PRCLicenseExpirationDate" HeaderText="PRC License Expiry Date" DataFormatString="{0:MM/dd/yyyy}" />
                                                    <asp:BoundField DataField="ATPDateSalesPerson" HeaderText="ATP Date" DataFormatString="{0:MM/dd/yyyy}" />
                                                    <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center" HeaderText="Select">
                                                        <ItemTemplate>
                                                            <asp:LinkButton runat="server" ID="bSelectsalesPersons" CssClass="btn btn-default btn-success" Width="100%" Height="100%" OnClick="bSelectsalesPersons_Click" CommandArgument='<%# Bind("Id")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center" HeaderText="Update">
                                                        <ItemTemplate>
                                                            <asp:LinkButton runat="server" ID="bUpdatesalesPersons" CssClass="btn btn-default btn-primary" Width="100%" Height="100%" OnCommand="bUpdatesalesPersons_Command" CommandArgument='<%# Bind("Id")%>'><i class="fa fa-edit"></i></asp:LinkButton>
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
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <%--GAB 07/04/2023 ERROR MESSAGE--%>
    <div id="modalErrorMessage" class="modal fade" style="z-index: 9999">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="row" style="text-align: center;">
                                <div class="col-lg-12">
                                    <asp:Image ImageUrl="~/assets/img/info.png" runat="server" ID="infoIcon" Width="90" Height="90" />
                                </div>
                            </div>
                            <div class="row" style="text-align: center;">
                                <div class="col-lg-12">
                                    <h4>
                                        <asp:Label runat="server" ID="errorMessageId" ClientIDMode="Static"></asp:Label></h4>
                                </div>
                            </div>
                            <div class="row" style="text-align: center;">
                                <div class="col-lg-12">
                                    <button type="button" class="btn btn-primary btn-sm" data-dismiss="modal" style="width: 90px;" onclick="HideAlert()">Ok</button>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <%--GAB 07/04/2023 DELETE CONFIRMATION BUTTON --%>
    <div id="modalConfirmDelete" class="modal fade" tabindex="-1" data-focus-on="input:first">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="row" style="text-align: center;">
                                <div class="col-lg-12">
                                    <asp:Image ImageUrl="~/assets/img/question.png" runat="server" ID="DeleteConfirm" Width="90" Height="90" />
                                </div>
                            </div>
                            <div class="row" style="text-align: center">
                                <div class="col-lg-12">
                                    <h4>
                                        <asp:Label runat="server" ID="lblDeleteConfirmation"></asp:Label></h4>
                                </div>
                            </div>
                            <div class="row" style="text-align: center">
                                <div class="col-lg-12">

                                    <%--2023-07-11 : CHANGED BUTTON TEXTS --%>
                                    <%--                                    <asp:LinkButton runat="server" ID="btnConfirmDelete" CssClass="btn btn-danger btn-sm" Text="Delete" Style="width: 90px;" OnClick="btnConfirmDelete_Click" />
                                    <button type="button" class="btn btn-default btn-sm" data-dismiss="modal" style="width: 90px;" onclick="hideConfirmDelete()">Cancel</button>--%>
                                    <asp:LinkButton runat="server" ID="btnConfirmDelete" CssClass="btn btn-danger btn-sm" Text="Yes" Style="width: 90px;" OnClick="btnConfirmDelete_Click" />
                                    <button type="button" class="btn btn-default btn-sm" data-dismiss="modal" style="width: 90px;" onclick="hideConfirmDelete()">No</button>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <script>
        var shareDb = new PouchDB('sharingDetails');
        var commission = new PouchDB('commission');

        // GAB 05/11/2023 initialization of pouchdb from database //Revision 05/13/2023
        $(document).on('click', '#<%= btnNext2.ClientID%>', async function () {
            console.log('test1');
            let pouchDb = $("#<%= txtNext2Hidden.ClientID%>").val();
            let json = JSON.parse(pouchDb).map(x => ({
                ...x,
                _rev: null
            }));
            console.log('test2');

            // get all documents in the database
            await shareDb.allDocs({ include_docs: true })
                .then(result => {
                    // remove each document one by one
                    return Promise.all(result.rows.map(row => {
                        return shareDb.remove(row.doc);
                    }));
                })
                .then(() => {
                    console.log('All documents removed from the database.');
                })
                .catch(error => {
                    console.error('Error removing documents from the database:', error);
                });

            //Add the document to the database
            await shareDb.bulkDocs(json).then(function (response) {
                console.log('Document loaded successfully!');
            }).catch(function (error) {
                console.log('Error loading document: ', error);
            });

            console.log('test3');

            //Check pouchDb items
            await shareDb.allDocs({ include_docs: true }).then(function (docs) {
                var pouchDBData = docs.rows.map(function (row) {
                    return row.doc;
                });
                $("#<%= txtNext2Hidden.ClientID%>").val(JSON.stringify(pouchDBData));
            });
            console.log('test4');

            var myButton = document.getElementById('<%= btnNext2Hidden.ClientID%>');

            if (myButton !== null) {
                console.log('test5');
                console.log('test7');
                myButton.click();
                console.log('test8');
            } else {
                console.log('test6');
                console.error('Could not find button element');
            }
        });

        //GAB 06/08/2023
        function ChangeCommPercent() {
            $("#CommPercent").trigger("change");
        }

        //GAB 05/13/2023 Comparing Table to disable rows that are existing 
        function ComparingTables() {
            if ($('#<%= gvProjectList.ClientID%> tr:not(:first-child):not(:last-child)').length == 10) {
                $('#<%= gvSelectedSharingDetails.ClientID%> > tbody > tr:not(:first-child)').each(function () {
                    //Get project code and selected commission per row
                    var selectedProjCode = $(this).find('td:eq(1)').text();
                    var selectedCommission = +$(this).find('td:eq(6)').text();
                    //Filter and highlight rows of gvProjectListtable
                    $('#<%= gvProjectList.ClientID%> > tbody > tr:not(:first-child):not(:last-child)').filter(function () {
                        var projCode = $(this).find('td:eq(1)').text();
                        var commission = +$(this).find('td:eq(3)').text();
                        return (selectedProjCode === projCode && selectedCommission === commission);
                    }).css('background-color', 'yellow').find('td:eq(0) input').prop('disabled', true);
                });
            } else {
                $('#<%= gvSelectedSharingDetails.ClientID%> > tbody > tr:not(:first-child)').each(function () {
                    //Get project code and selected commission per row
                    var selectedProjCode = $(this).find('td:eq(1)').text();
                    var selectedCommission = +$(this).find('td:eq(6)').text();

                    //Filter and highlight rows of gvProjectListtable
                    $('#<%= gvProjectList.ClientID%> > tbody > tr:not(:first-child)').filter(function () {
                        var projCode = $(this).find('td:eq(1)').text();
                        var commission = +$(this).find('td:eq(3)').text();
                        return (selectedProjCode === projCode && selectedCommission === commission);
                    }).css('background-color', 'yellow').find('td:eq(0) input').prop('disabled', true);
                });
            }
        }

        //GAB 05/13/2023 Disabling rows depending on commission percentage
        $('#modalAddSharing').on('change', '#CommPercent', function () {
            let salesShare = this.value;
            let lotShare = 0;
            let sumShare = 0;

            //2023-09-08 : EXCLUDE DISABLING WHEN PROJECT LIST HAS LESS THAN 10 ROWS
            if ($('#<%= gvProjectList.ClientID%> tr:not(:first-child):not(:last-child)').length == 10) {
                console.log('joses-test1');

                $('#<%= gvProjectList.ClientID%> > tbody > tr:not(:first-child):not(:last-child)').each(function () {
                    var comms = $(this).closest('tr').find('td:eq(3)').text();
                    if (+comms == +salesShare) {
                        $(this).closest('tr').find('td:eq(0) input[type="checkbox"]').prop('disabled', false);
                        $(this).closest('tr').css("background-color", "white");
                    } else {
                        $(this).closest('tr').find('td:eq(0) input[type="checkbox"]').prop('disabled', true);
                        $(this).closest('tr').css("background-color", "gray");
                    }
                });
            } else {
                //2023-09-08 : EXCLUDE DISABLING WHEN PROJECT LIST HAS LESS THAN 10 ROWS
                console.log('joses-test2');
                $('#<%= gvProjectList.ClientID%> > tbody > tr:not(:first-child)').each(function () {
                    var comms = $(this).closest('tr').find('td:eq(3)').text();
                    if (+comms == +salesShare) {
                        console.log('joses-test3');
                        $(this).closest('tr').find('td:eq(0) input[type="checkbox"]').prop('disabled', false);
                        $(this).closest('tr').css("background-color", "white");
                    } else {
                        console.log('joses-test4');
                        $(this).closest('tr').find('td:eq(0) input[type="checkbox"]').prop('disabled', true);
                        $(this).closest('tr').css("background-color", "gray");
                    }
                });

            }



            $('#<%= gvShareDetails.ClientID%> > tbody > tr').each(function () {
                lotShare = +$(this).closest('tr').find('td:visible:eq(4)').text();
                sumShare += lotShare;
            });

            if (salesShare == sumShare) {
                $('#<%= btnAddToSharing.ClientID %>').prop('disabled', false);
            } else {
                $('#<%= btnAddToSharing.ClientID %>').prop('disabled', true);
                $('#<%= gvProjectList.ClientID%> > tbody > tr:not(:last-child)').each(function () {
                    $(this).closest('tr').find('td:eq(0) input[type="checkbox"]').prop('disabled', true);
                });
                ShowErrorMessageModal("Commission Percentage is not equal to the total defined sharing");
            }

            $('table input[type="checkbox"]').prop('checked', false);
            ComparingTables();
        });

        //GAB 05/13/2023 Check all checkboxes in project list
        function checkAll(chk) {
            var gridView = document.getElementById('<%= gvProjectList.ClientID %>');
            var inputs = gridView.getElementsByTagName("input");
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox" && !inputs[i].disabled) {
                    inputs[i].checked = chk.checked;
                }
            }
        }

        function uncheckAll() {
            var gridView = document.getElementById('<%= gvProjectList.ClientID %>');
            var inputs = gridView.getElementsByTagName("input");
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type === "checkbox") {
                    inputs[i].checked = false;
                }
            }
        }

        //GAB 05/13/2023 Check all checkboxes in selected sharing details list
        function checkAllBack(chk) {
            var gridView = document.getElementById('<%= gvSelectedSharingDetails.ClientID %>');
            var inputs = gridView.getElementsByTagName("input");
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox" && !inputs[i].disabled) {
                    inputs[i].checked = chk.checked;
                }
            }
        }

        //GAB 05/27/2023 Check all checkboxes in Standard Document Requirements list
        function checkAllDocs(chk) {
            var gridView = document.getElementById('<%= gvStandardDocumentRequirements.ClientID %>');
            var inputs = gridView.querySelectorAll("input[type='checkbox']");
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox" && !inputs[i].disabled) {
                    inputs[i].checked = chk.checked;
                }
            }
        }

        //GAB 05/27/2023 Check all checkboxes in Sole Document Requirements list
        function checkAllSolePartCorp(chk) {
            var gridView = chk.closest('table');
            var inputs = gridView.querySelectorAll("input[type='checkbox']");
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox" && !inputs[i].disabled) {
                    inputs[i].checked = chk.checked;
                }
            }
        }

        //////////GAB 05/27/2023 Check all checkboxes in Partnership Document Requirements list
        ////////function chkAllParntner(chk) {
        ////////////////    var gridView = document.getElementById('<%= gvPartnershipDocumentRequirements.ClientID %>');
        ////////    var inputs = gridView.getElementsByTagName("input");
        ////////    for (var i = 0; i < inputs.length; i++) {
        ////////        if (inputs[i].type == "checkbox" && !inputs[i].disabled) {
        ////////            inputs[i].checked = chk.checked;
        ////////        }
        ////////    }
        ////////}

        //////////GAB 05/27/2023 Check all checkboxes in Corporation Document Requirements list
        ////////function checkAllCorp(chk) {
        ////////////////    var gridView = document.getElementById('<%= gvCorporationDocumentRequirements.ClientID %>');
        ////////    var inputs = gridView.getElementsByTagName("input");
        ////////    for (var i = 0; i < inputs.length; i++) {
        ////////        if (inputs[i].type == "checkbox" && !inputs[i].disabled) {
        ////////            inputs[i].checked = chk.checked;
        ////////        }
        ////////    }
        ////////}

        //GAB 5/15/2023 Checking all checkboxes with same project code and commission
        function checkSame(projectCode, commission) {
            debugger;
            var matchingRows = $("#<%= gvSelectedSharingDetails.ClientID %> tr:not(:first-child)").filter(function () {
                var rowProjectCode = $(this).find("td:nth-child(2)").text().trim();
                var rowCommission = $(this).find("td:nth-child(7)").text().trim();
                return rowProjectCode === projectCode && rowCommission === commission;
            });

            var isChecked = matchingRows.first().find("input[type='checkbox']").prop("checked");

            matchingRows.find("input[type='checkbox']").prop("checked", isChecked);
        }

        //GAB 05/13/2023 PouchDb
        $('#modalAddSharing').on('click', '#<%= btnAddToSharing.ClientID%>', async function () {
            let obj = {};
            let type;

            for (const salesAgent of $('#<%= gvShareDetails.ClientID%> > tbody > tr:not(:first-child)').toArray()) {
                let salesPersonId = $(salesAgent).closest('tr').find('td:eq(1)').text();
                let position = $(salesAgent).closest('tr').find('td:eq(2)').text();
                let salesPersonName = $(salesAgent).closest('tr').find('td:eq(3)').text();

                let lotShare = 0;
                let houseLotShare = 0;
                let $tr = $(salesAgent).closest('tr');
                if ($('th:contains("Lot Only Percentage")').is(':visible')) {
                    lotShare = +$tr.find('td:eq(4)').text() || 0;
                    type = "Lot";
                }
                if ($('th:contains("House And Lot Percentage")').is(':visible')) {
                    houseLotShare = +$tr.find('td:eq(5)').text() || 0;
                    type = "HouseNLot";
                }

                let salesAgentId = $(salesAgent).closest('tr').find('td:eq(6)').text();

                for (const salesPerson of $('#<%= gvProjectList.ClientID%> > tbody > tr:not(:first-child) input[type="checkbox"]:checked').toArray()) {
                    let projCode = $(salesPerson).closest('tr').find('td:eq(1)').text();
                    let projName = $(salesPerson).closest('tr').find('td:eq(2)').text();
                    let commission = +$(salesPerson).closest('tr').find('td:eq(3)').text();

                    let maxId = 0;
                    await shareDb.allDocs({ include_docs: true, attachments: false }).then(function (result) {
                        result.rows.forEach(function (row) {
                            const doc = row.doc;
                            if (doc && doc.Id && doc.Id > maxId) {
                                maxId = doc.Id;
                            }
                        });
                    }).catch(function (error) {
                        console.log('Error retrieving documents: ', error);
                    });

                    const newId = maxId + 1;

                    var currentDate = new Date().toISOString().slice(0, 10);

                    obj = {
                        Id: newId,
                        SalesPersonId: salesPersonId,
                        PositionSharedDetails: position,
                        SalesPersonNameSharedDetails: salesPersonName,
                        PercentageSharedDetails: lotShare,
                        CreateDate: currentDate,
                        OslaId: salesAgentId,
                        HouseandLotSharingDetails: houseLotShare,
                        ProjectCode: projCode,
                        ProjectName: projName,
                        CommissionPercentage: commission,
                        Type: type,
                    }

                    // Add the document to the database
                    await shareDb.post(obj).then(function (response) {
                        console.log('Document added successfully!');
                    }).catch(function (error) {
                        console.log('Error adding document: ', error);
                    });
                }

            }

            $('table input[type="checkbox"]').prop('checked', false);
            shareDb.allDocs({ include_docs: true }).then(function (docs) {
                var pouchDBData = docs.rows.map(function (row) {
                    return row.doc;
                });
                let stringifyData = JSON.stringify(pouchDBData);
                document.getElementById('<%= txtPouchDbDataViewHidden.ClientID %>').value = stringifyData;
                document.getElementById('<%= txtNext3Hidden.ClientID %>').value = stringifyData;
                var myButton = document.getElementById('<%= BtnPouchDbDataViewHidden.ClientID%>');

                if (myButton !== null) {
                    myButton.click();
                } else {
                    console.error('Could not find button element');
                }

            });
        })

        //GAB 05/13/2023 Delete in PouchDb
        $('#modalAddSharing').on('click', '#<%= btnBackToProjList.ClientID%>', async function () {
            for (const salesPerson of $('#<%= gvSelectedSharingDetails.ClientID%> > tbody > tr:not(:first-child) input[type="checkbox"]:checked').toArray()) {

                let pouchId = $(salesPerson).closest('tr').find('td:eq(10)').text();

                // First, use the get() method to retrieve the document by _id
                await shareDb.get(pouchId).then(
                    function (doc) {
                        // Delete the document using its _id and _rev
                        return shareDb.remove(doc);
                    }).then(
                        function (result) {
                            console.log('Document deleted successfully');
                        }).catch(


                            function (err) {
                                console.log(err);
                            });

            }

            //Check pouchDb items
            await shareDb.allDocs({ include_docs: true }).then(function (docs) {
                var pouchDBData = docs.rows.map(function (row) {
                    return row.doc;
                });
                $("#<%= txtNext3Hidden.ClientID%>").val(JSON.stringify(pouchDBData));
            });

            $('table input[type="checkbox"]').prop('checked', false);

            await shareDb.allDocs({ include_docs: true }).then(function (docs) {
                var pouchDBData = docs.rows.map(function (row) {
                    return row.doc;
                });
                let stringifyData = JSON.stringify(pouchDBData);
                document.getElementById('<%= txtPouchDbDataDeleteHidden.ClientID %>').value = stringifyData;

                var myButton = document.getElementById('<%= BtnPouchDbDataDeleteHidden.ClientID%>');

                if (myButton !== null) {
                    myButton.click();
                } else {
                    console.error('Could not find button element');
                }

            });
        });

        //GAB 05/13/2023 putting data inside PouchDB then triggering backend code.
        function getPouchData() {
            shareDb.allDocs({ include_docs: true }).then(function (docs) {
                var pouchDBData = docs.rows.map(function (row) {
                    return row.doc;
                });
                let stringifyData = JSON.stringify(pouchDBData);
                document.getElementById('<%= hiddenLabel.ClientID %>').value = stringifyData;

                var myButton = document.getElementById('<%= btnSubmitDocumentHidden.ClientID%>');

                if (myButton !== null) {
                    myButton.click();
                } else {
                    console.error('Could not find button element');
                }
            });
        }

        // GAB 05/13/2023 initialization of pouchdb from database
        $(document).on('click', '#<%= btnNext3.ClientID%>', async function () {
            var myButton = document.getElementById('<%= btnNext3Hidden.ClientID%>');

            if (myButton !== null) {
                myButton.click();
            } else {
                console.error('Could not find button element');
            }
        });

        //GAB 05/17/2023 Store Checked and Unchecked rows in gvSelectedSharedDetails version 1
        function storeCheckedItems(checkbox) {
            debugger;
            var hfCheckedItems = $('#<%= hfCheckedItems.ClientID %>');
            var row = $(checkbox).closest('tr');
            console.log("row", row);
            if (row && row.find('td').length > 1) {
                var itemID = row.find('td:eq(1)').text(); // Replace 1 with the index of the ProjectCode column in your GridView
                console.log("itemID", itemID);
                if (checkbox.checked) {
                    if (hfCheckedItems.val().indexOf(itemID) === -1) {
                        hfCheckedItems.val(hfCheckedItems.val() + itemID + ",");
                    }
                } else {
                    hfCheckedItems.val(hfCheckedItems.val().replace(itemID + ",", ""));
                }
            } else {
                console.error('Checkbox parent node or cell is undefined or null');
            }
        }


        //GAB 05/16/2023 Restore Checked and Unchecked rows in gvSelectedSharedDetails
        function restoreCheckedItems() {
            debugger;
            var hfCheckedItems = $('#<%= hfCheckedItems.ClientID %>');
            var gridView = $('#<%= gvSelectedSharingDetails.ClientID %>')[0];
            console.log("hfCheckedItems", hfCheckedItems);
            console.log("gridView", gridView);

            for (var i = 1; i < gridView.rows.length; i++) {
                var row = $(gridView.rows[i]);
                var itemID = row.find('td:eq(1)').text(); // Replace 1 with the index of the ProjectCode column in your GridView
                var checkbox = row.find('td:eq(0) input[type="checkbox"]');

                if (hfCheckedItems.val().indexOf(itemID) !== -1) {
                    checkbox.prop('checked', true);
                }
            }
        }

        //GAB 07/05/2023 COMMENTED REASON: WENT TO BACKEND
        <%--//GAB 05/24/2023 
        function ComparingSalesPersonTables() {
            $("#<%= gvSalesPerson.ClientID %> > tbody > tr:not(:first-child)").each(function () {
                var selectedSalesPersonId = $(this).find('td:eq(0)').text();
                console.log("Basis", selectedSalesPersonId);
                $('#<%= gvSalesPersons.ClientID %> > tbody > tr:not(:first-child)').filter(function () {
                    var SalesPersonId = $(this).find('td:eq(0)').text();
                    console.log("Exists?", SalesPersonId);
                    return (selectedSalesPersonId === SalesPersonId);
                }).find('td:eq(-2) a').addClass('disabled').on('click', function (e) {
                    e.preventDefault();
                });
            });
        }

        function ComparingSharingDetailsTables() {
            $("#<%= gvShareDetails.ClientID %> > tbody > tr:not(:first-child)").each(function () {
                var selectedSalesPersonId = $(this).find('td:eq(6)').text();
                console.log("Basis", selectedSalesPersonId);
                $('#<%= gvSalesPersons.ClientID %> > tbody > tr:not(:first-child)').filter(function () {
                    var SalesPersonId = $(this).find('td:eq(0)').text();
                    console.log("Exists?", SalesPersonId);
                    return (selectedSalesPersonId === SalesPersonId);
                }).find('td:eq(-2) a').addClass('disabled').on('click', function (e) {
                    e.preventDefault();
                });
            });
        }--%>

        //GAB 06/27/2023 Setting focus
        function setFocusToNextField() {
            // Get the next field after the DropDownList
            var nextField = document.getElementById('<%= ddlVATCode2.ClientID %>').nextElementSibling;

            // Set focus to the next field
            if (nextField) {
                nextField.focus();
            }
        }

        ////GAB 06/22/2023 Hiding Tax Comment textbox
        //function hideTaxDetails() {
        //    $("#divTaxDetails").hide();
        //}

        ////GAB 06/22/2023 Showing Tax Comment textbox
        //function showTaxDetails() {
        //    $("#divTaxDetails").show();
        //}

    </script>

</asp:Content>

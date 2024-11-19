<%@ Page Title="DREAMS | Restructuring History" Language="C#" MasterPageFile="~/master/Main.Master" AutoEventWireup="true" CodeBehind="RestructuringHistory.aspx.cs" Inherits="ABROWN_DREAMS.RestructuringHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%-- Refresh --%>
    <script type="text/javascript">
        document.onkeydown = function () {
            if (event.keyCode == 116) {
                event.returnValue = false;
                event.keyCode = 0;
                return false;
            }
        };
    </script>
    <%--STRING FORMAT--%>
    <script>
        $('#validuntil').keyup(function () {
            alert('test');
            //var i = 0,
            //    v = this.value;
            //var pattern = '##/##'
            //return pattern.replace(/#/g, _ => v[i++]);
        });
    </script>
    <%--MODALS--%>
    <script type="text/javascript">
        function showProjList() {
            $('#modalProjList').modal('show');
        }
        function showQuotation() {
            $('#modalQuotation').modal({
                backdrop: 'static',
                keyboard: false
            });
        }
        function closeQuotation() {
            $('#modalQuotation').modal('hide');
        }
        function showAlert() {
            $('#modalAlert').modal('show');
        }
        function showAccounts() {
            $('#modalAccounts').modal('show');
        }
        function closeAccounts() {
            $('#modalAccounts').modal('hide');
        }
        function showCredit() {
            $('#modalCreditCard').modal({
                backdrop: 'static',
                keyboard: false
            });
        }
        function closeCredit() {
            $('#modalCreditCard').modal('hide');
        }
        function showCreditPayment() {
            $('#modalCredit').modal({
                backdrop: 'static',
                keyboard: false
            });
        }
        function closeCreditPayment() {
            $('#modalCredit').modal('hide');
        }
        function showPymtMethod() {
            $('#modalCreditPaymentMethod').modal('show');
        }
        function closePymtMethod() {
            $('#modalCreditPaymentMethod').modal('hide');
        }
        function showBank() {
            $('#modalBank').modal('show');
        }
        function hideBank() {
            $('#modalBank').modal('hide');
        }
        function showBranch() {
            $('#modalBranch').modal('show');
        }
        function hideBranch() {
            $('#modalBranch').modal('hide');
        }
        function showCash() {
            $('#modalCash').modal({
                backdrop: 'static',
                keyboard: false
            });
        }
        function closeCash() {
            $('#modalCash').modal('hide');
        }
        function showCheck() {
            $('#modalCheck').modal('show');
        }
        function closeCheck() {
            $('#modalCheck').modal('hide');
        }
        function showPaymentMeans() {
            $('#modalPaymentMeans').modal('show');
        }
        function showPayments() {
            $('#modalPayment').modal('show');
        }
        //Custom Functions that you can call
        function resetAllValues() {
            $('.quotation').find('input:text').val('');
        }

        function MsgRestructuringDocReq_Hide() {
            $('#MsgRestructuringDocuments').modal('hide');
        };
        function MsgRestructuringDocReq_Show() {
            $('#MsgRestructuringDocuments').modal('show');
        };



    </script>
    <%--Compute--%>
    <script type="text/javascript">
        function compute_downpayment() {
            $(".discountpercent").change(function () {
                $('.downpayment').val($('.sellingprice').val() * ($('.discountpercent').val() / 100)).number(true, 2);
            });
        }
        $(document).ready(function () {
            $('.panellist').height = $('.panelamount').height;
        });
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="sitemap" runat="server">
    <li><a href="#"><span>Reservation</span></a></li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div class="col-lg-4">
        <div class="panel panel-default">
            <div class="panel-body">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:Label runat="server" ID="lblDocEntry" CssClass="hide" />
                        <asp:Panel runat="server" ID="pnlBuyerInfo">
                            <div class="row">
                                <div class="col-lg-10">
                                    <div class="customer-badge" style="background-color: white;">
                                        <%--#f1ffec--%>
                                        <%--<asp:LinkButton runat="server" ID="removebuyer" OnClick="removebuyer_Click" CssClass="pull-right"><span class="fa fa-times-circle pull-right" style="color: #E2584D;"></span></asp:LinkButton>--%>
                                        <div class="avatar">
                                            <img src="../assets/img/user.png" alt="" width="50" height="50">
                                        </div>
                                        <div class="details">
                                            <asp:Label runat="server" ID="lblName" CssClass="text-uppercase" Style="font-size: 16px;" Text="" />
                                            <div class="text-success balance">
                                                <asp:Label runat="server" ID="lblID" CssClass="small text-uppercase" Text="" /><br />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <asp:LinkButton runat="server" ID="btnFind" CssClass="btn btn-info btn-circle pull-right" OnClick="btnFind_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                </div>
                            </div>




                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:GridView runat="server"
                                        ID="gvHouseLot"
                                        CssClass="table table-responsive table-hover"
                                        AutoGenerateColumns="false"
                                        GridLines="None"
                                        ShowHeader="false"
                                        UseAccessibleHeader="true"
                                        SelectedRowStyle-BackColor="#A1DCF2"
                                        EmptyDataText="No records found"
                                        OnRowCommand="gvHouseLot_RowCommand">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblDocEntry" CssClass="text-uppercase" Style="font-size: 16px;" Visible="false" Text='<%# Eval("DocEntry") %>' />
                                                    <asp:Label runat="server" ID="lblProject" CssClass="text-uppercase" Style="font-size: 16px;" Text='<%# Eval("PrjName") %>' />
                                                    <br />
                                                    <div class="text-success balance">
                                                        <asp:Label runat="server" ID="lblBlock" CssClass="small text-uppercase" Text='<%# "Block:" + Eval("Block") %>' />
                                                        <asp:Label runat="server" ID="lblLot" CssClass="small text-uppercase" Text='<%# "Lot:" + Eval("Lot") %>' />
                                                        <asp:Label runat="server" ID="lblModel" CssClass="small text-uppercase" Text='<%# "Model:" + Eval("Model") %>' />
                                                        <asp:Label runat="server" ID="lblStatus" CssClass="small text-uppercase" Text='<%# Eval("Status") %>' Visible="false" />
                                                        <asp:Label runat="server" ID="lblPhase" CssClass="small text-uppercase" Text='<%# Eval("Phase") %>' Visible="false" />
                                                        <asp:Label runat="server" ID="lblDocNum" CssClass="small text-uppercase" Text='<%# Eval("DocNum") %>' Visible="false" />
                                                        <asp:Label runat="server" ID="lblFinscheme" CssClass="small text-uppercase" Text='<%# Eval("FinancingScheme") %>' Visible="false" />
                                                        <asp:Label runat="server" ID="lblBlock1" CssClass="small text-uppercase" Text='<%# Eval("Block") %>' Visible="false" />
                                                        <asp:Label runat="server" ID="lblLot1" CssClass="small text-uppercase" Text='<%# Eval("Lot") %>' Visible="false" />
                                                        <asp:Label runat="server" ID="lblModel1" CssClass="small text-uppercase" Text='<%# Eval("Model") %>' Visible="false" />
                                                        <asp:Label runat="server" ID="lblFinschemeCode" CssClass="small text-uppercase" Text='<%# Eval("FinCode") %>' Visible="false" />
                                                        <asp:Label runat="server" ID="lblProjCode" CssClass="small text-uppercase" Text='<%#  Eval("ProjCode") %>' Visible="false" />
                                                        <asp:Label runat="server" ID="lblRSTDocEntry" CssClass="text-uppercase" Style="font-size: 16px;" Visible="false" Text='<%# Eval("RSTDocEntry") %>' />
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:ButtonField ButtonType="Link" ControlStyle-CssClass="btn btn-default btn-success btn-sm fa fa-arrow-right pull-right" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" CommandName="Sel" />
                                        </Columns>
                                        <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                        <PagerStyle CssClass="pagination-ys" />
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>






                            <div class="row">
                                <div class="col-lg-12 col-sm-12 box-container" style="height: 50px; background-color: #f1ffec;">
                                    <div class="row">
                                        <div class="col-lg-4">
                                            <h5 style="color: #67676C;">Quotation No.:</h5>
                                        </div>
                                        <div class="col-lg-8" style="text-align: right;">
                                            <asp:Label runat="server" ID="lblQuotationNum" CssClass="" Text="" Font-Size="14" ForeColor="#3333cc" Font-Bold="true" />
                                        </div>
                                    </div>
                                </div>
                            </div>





                            <div class="comment-block">
                                <div class="side-heading">
                                    <div class="table-responsive">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <table class="table">
                                                    <caption>Buyer's Details</caption>

                                                    <tr>
                                                        <th style="width: 50%">Document Date:</th>
                                                        <td>
                                                            <%--2023-05-16 : REQUESTED TO BE ADDED BY DHEZA--%>
                                                            <asp:Label runat="server" ID="lblDocDate" Text="" /></td>
                                                    </tr>
                                                    <tr runat="server" id="trLastName">
                                                        <th style="width: 50%">Last Name:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="txtLName" Text="" /></td>
                                                    </tr>
                                                    <tr runat="server" id="trFirstName">
                                                        <th>First Name:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="txtFName" Text="" /></td>
                                                    </tr>
                                                    <%--                                                    <tr runat="server" id="trMiddleName">
                                                        <th>Middle Name:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="txtMName" Text="" /></td>
                                                    </tr>--%>

                                                    <tr>
                                                        <th>Project:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="txtProjId" Text="" /></td>
                                                    </tr>
                                                    <tr>
                                                        <th>Block:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="txtBlock" Text="" /></td>
                                                    </tr>
                                                    <tr>
                                                        <th>Lot:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="txtLot" /></td>
                                                    </tr>
                                                    <tr>
                                                        <th>House Model:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblModel" Text="" /></td>
                                                    </tr>
                                                    <tr>
                                                        <th>Lot Area:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="txtLotArea" Text="" /></td>
                                                    </tr>
                                                    <tr>
                                                        <th>Floor Area:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="txtFloorArea" /></td>
                                                    </tr>
                                                    <tr>
                                                        <th>Financing Scheme:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="txtFinScheme" Text="" /></td>
                                                    </tr>

                                                    <tr>
                                                        <th>Book Status:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="txtBookStatus" Text="" /></td>
                                                    </tr>


                                                    <tr>
                                                        <th>Loan Type:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblLoanType" /></td>
                                                    </tr>

                                                    <tr id="trBank" runat="server">
                                                        <th>Bank:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblBank" /></td>
                                                    </tr>
                                                    <tr>
                                                        <th>Retitling Type:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblRetitlingType" /></td>
                                                    </tr>
                                                    <tr>
                                                        <th>Tax Classification:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblTaxClassification" Text="" /></td>
                                                    </tr>

                                                    <tr>
                                                        <th>LOI:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblLoi" /></td>
                                                    </tr>
                                                    <tr>
                                                        <th>LTS No:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblLTSNo" /></td>
                                                    </tr>

                                                    <tr>
                                                        <th>Co-Borrower:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblCoBorrower" /></td>
                                                    </tr>

                                                    <%--2023-05-16 : REQUESTED TO BE ADDED BY DHEZA--%>
                                                    <tr>
                                                        <th>Sales Agent:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblSalesAgent" /></td>
                                                    </tr>













                                                    <%--                                                    <tr>
                                                        <th>Business Type:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblBusinessType" Text="" /></td>
                                                    </tr>--%>
                                                    <%--   <tr>
                                                        <th>Co-maker:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblComaker" Text="" /></td>
                                                    </tr>
                                                    <tr runat="server" id="trCompanyName">
                                                        <th>Company Name:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblCompanyName" Text="" /></td>
                                                    </tr>
                                                    <tr runat="server" id="trBirthday">
                                                        <th>Birthday:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblBirthday" Text="" /></td>
                                                    </tr>
                                                    <tr runat="server" id="trNatureEmployment">
                                                        <th>Nature of Employment:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblNatureofEmployment" Text="" /></td>
                                                    </tr>
                                                    <tr runat="server" id="trIdType">
                                                        <th>Type of ID:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblTypeofID" Text="" /></td>
                                                    </tr>
                                                    <tr runat="server" id="trIdNumber">
                                                        <th>ID Number:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblIDNo" Text="" /></td>
                                                    </tr>--%>

                                                    <hr />



                                                    <div class="row">
                                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                            <fieldset>
                                                                <asp:GridView ID="gvCoOwner" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                                                    CssClass="table table-bordered table-hover" Width="100%" ShowHeader="True" PageSize="5"
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

                                                                        <%--         <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center" HeaderText="Delete">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton runat="server" ID="bDeleteOwner" CssClass="btn btn-default btn-danger" Width="100%" Height="100%" CommandArgument='<%# Bind("Code")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>--%>
                                                                    </Columns>
                                                                    <PagerStyle CssClass="pagination-ys" />
                                                                    <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                                                </asp:GridView>
                                                            </fieldset>
                                                        </div>
                                                    </div>











                                                    <%--  <tr>
                                                        <th>Account Type:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="  " /></td>
                                                    </tr>
                                                    <tr>
                                                        <th>Sales Type:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="txtSalesType" Text="" /></td>
                                                    </tr>--%>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>





                            <div class="comment-block">
                                <div class="side-heading">
                                    <div class="table-responsive">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <table class="table">
                                                    <caption>Restructuring Details</caption>

                                                    <tr runat="server" id="tr1">
                                                        <th style="width: 50%">Restructuring Date:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblRestructuringDate" Text="" /></td>
                                                    </tr>
                                                    <tr runat="server" id="tr2">
                                                        <th>Restructuring Type:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblRestructuringType" Text="" /></td>
                                                    </tr>
                                                    <tr runat="server" id="tr3">
                                                        <th>Other Restructuring Type:</th>
                                                        <td>
                                                            <asp:Button runat="server" CssClass="btn btn-info" Text="Restructuring Document Requirements" ID="btnRequirementDocumentRequirements" OnClick="btnRequirementDocumentRequirements_Click" data-backdrop="static" />
                                                        </td>
                                                    </tr>

                                                    <tr runat="server" id="tr4">
                                                        <th>Restructuring Request Letter:</th>
                                                        <td>
                                                            <asp:Label Visible="false" runat="server" ID="lblRestructuringLetter" Text="" />
                                                            <asp:LinkButton ID="btnRequestLetter" CssClass="btn btn-info" OnClick="btnRequestLetter_Click" runat="server" Text="Open Document" />

                                                        </td>
                                                    </tr>
                                                    <tr runat="server" id="tr5">
                                                        <th>Request Letter Approval Date:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblLetterApprovalDate" Text="" /></td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>











                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>



        <div class="panel panel-default hidden">
            <div class="panel-body">
                <asp:GridView runat="server"
                    ID="gvReports"
                    CssClass="table table-responsive"
                    AutoGenerateColumns="false"
                    GridLines="None"
                    BorderStyle="None"
                    ShowHeader="false"
                    OnRowCommand="gvReports_RowCommand"
                    OnRowDataBound="gvReports_RowDataBound">
                    <HeaderStyle BackColor="#66ccff" />
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="LineNum" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                        <asp:BoundField DataField="Name" ItemStyle-Width="100%" />
                        <asp:BoundField DataField="RptName" ItemStyle-Width="100%" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                        <asp:BoundField DataField="RptPath" ItemStyle-Width="100%" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                        <asp:BoundField DataField="RptGroup" ItemStyle-Width="100%" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                        <asp:BoundField DataField="ObjCode" ItemStyle-Width="100%" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText="0" />
                        <%--<asp:ButtonField ControlStyle-CssClass="fa fa-print" ItemStyle-Width="10" CommandName="Print" ItemStyle-HorizontalAlign="Center" />--%>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <%--<asp:Image runat="server" ID="isPrinted" ImageUrl="~/assets/img/cancel.png" Width="20" />--%>
                                <asp:LinkButton CssClass="btn btn-primary btn-sm" ID="btnPrint" Text="Print" runat="server" OnClick="btnPrint_Click" CommandArgument='<%# Container.DataItemIndex %>'>
                                    Print <i class="fa fa-print"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                    <PagerStyle CssClass="pagination-ys" />
                </asp:GridView>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>

                        <div class="finish-sale">
                            <div class="row">
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <div class="col-lg-12">
                                            <button runat="server" id="btnForward" class="btn btn-success btn-large btn-block" onserverclick="btnForward_ServerClick">Forward to Cashier <i class="fa fa-check"></i></button>
                                        </div>
                                        <%--<div class="col-lg-6">
                                            <button runat="server" id="btnCancel" class="btn btn-danger btn-large btn-block" onserverclick="btnCancel_ServerClick">Cancel <i class="fa fa-close"></i></button>
                                        </div>--%>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>


        <div class="panel panel-default" style="display: none;">
            <div class="panel-body">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:GridView runat="server"
                            ID="gvPayments"
                            CssClass="table table-responsive"
                            AutoGenerateColumns="false"
                            GridLines="None"
                            BorderStyle="None"
                            ShowHeader="false"
                            OnRowCommand="gvPayments_RowCommand"
                            OnRowDataBound="gvPayments_RowDataBound">
                            <HeaderStyle BackColor="#66ccff" />
                            <Columns>
                                <asp:BoundField DataField="LineNum" HeaderText="LineNum" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="Type" ItemStyle-Width="100%" />
                                <asp:BoundField DataField="Amount" ItemStyle-Width="100%" ItemStyle-HorizontalAlign="right" DataFormatString="{0:#,##0.00}" />
                                <%--CHECK--%>
                                <asp:BoundField DataField="CheckNo" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="BankCode" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="Bank" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="Branch" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="DueDate" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="AccountNum" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <%--CREDIT--%>
                                <asp:BoundField DataField="CreditCard" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="CreditAcctCode" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="CreditAcct" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="CreditCardNumber" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="ValidUntil" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="IdNum" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="TelNum" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="PymtTypeCode" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="PymtType" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="NumOfPymts" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="VoucherNum" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="Id" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText="0" />
                                <asp:BoundField DataField="ORNumber" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText=" " />

                                <asp:ButtonField ControlStyle-CssClass="glyphicon glyphicon-edit" ItemStyle-Width="10" CommandName="Edt" ItemStyle-HorizontalAlign="Center" />
                                <asp:ButtonField ControlStyle-CssClass="fa fa-times-circle" ItemStyle-Width="10" CommandName="Del" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="#e2584d" />
                            </Columns>
                            <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                            <PagerStyle CssClass="pagination-ys" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%--<hr style="border-top: dashed 1px #D0D3D8;" />--%>
                <h5 style="color: #67676C;">Add Payment</h5>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <button runat="server" id="btnCash" class="btn btn-primary" onserverclick="btnCash_ServerClick">Cash</button>
                        <button runat="server" id="btnCheck" class="btn btn-primary" onserverclick="btnCheck_ServerClick">Check</button>
                        <button runat="server" id="btnCredit" class="btn btn-primary" onserverclick="btnCredit_ServerClick">Credit</button>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-lg-12 col-sm-12 box-container" style="height: 50px; background-color: #f1ffec;">
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5 style="color: #67676C;">Balance:</h5>
                                    </div>
                                    <div class="col-lg-8" style="text-align: right;">
                                        <asp:Label runat="server" ID="reservationbalance" CssClass="" Text="0.00" Font-Size="14" ForeColor="#d43f3a" Font-Bold="true" /></h4>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-6 col-sm-12 box-container-left">
                                <div class="col-lg-12">
                                    <h4 style="color: #67676C;">Total</h4>
                                </div>
                                <div class="col-lg-12">
                                    <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                        <asp:Label runat="server" ID="lblAmount" CssClass="" Text="0.00" Font-Size="16" ForeColor="#33cc33" Font-Bold="true"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-6 col-sm-12 box-container-right">
                                <div class="col-lg-12">
                                    <h4 style="color: #67676C;">Payment</h4>
                                </div>
                                <div class="col-lg-12">
                                    <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                        <asp:Label runat="server" ID="lblAmountDue" CssClass="" Text="0.00" Font-Size="16" ForeColor="#ff9933" Font-Bold="true"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="comment-block">
                    <div class="side-heading">Commission:</div>
                </div>
                <div class="finish-sale">
                    <div class="row">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <div class="col-lg-6">
                                    <button runat="server" id="btnPayment" class="btn btn-success btn-large btn-block" onserverclick="btnPayment_ServerClick">Save <i class="fa fa-check"></i></button>
                                </div>
                                <div class="col-lg-6">
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="col-lg-8">
        <div class="panel panel-primary clearfix" style="margin-left: 10px;">
            <div class="panel-body">
                <div class="row">
                    <div class="col-lg-12">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-lg-12">


                                        <%--2023-06-22 : ADD BUTTONS FROM QUOTATION SUMMARY--%>
                                        <div class="col-lg-3">
                                            <button id="btnSchedule" runat="server" data-toggle="modal" data-target="#modalSchedule" class="btn btn-secondary btn-dropbox pull-right" type="button">Payment Schedule</button>
                                        </div>
                                        <div class="col-lg-3">
                                            <button id="btnnCommissionScheme" runat="server" style="padding-right: 15px;" data-toggle="modal" data-target="#modalCommissionScheme" class="btn btn-secondary btn-dropbox pull-right" type="button">Commission Scheme</button>
                                        </div>
                                        <div class="col-lg-3">
                                            <button id="btnIncentiveScheme" runat="server" style="padding-right: 15px;" data-toggle="modal" data-target="#modalIncentiveScheme" class="btn btn-secondary btn-dropbox pull-right" type="button">Incentive Scheme</button>
                                        </div>
                                        <div class="col-lg-3">
                                            <button id="btnSharingDetails" runat="server" style="padding-right: 15px;" data-toggle="modal" data-target="#modalSharingDetails" class="btn btn-secondary btn-dropbox pull-right" type="button">Sharing Details</button>
                                        </div>



                                    </div>
                                </div>
                                <div class="table-responsive">
                                    <table class="table">
                                        <caption>Summary</caption>


                                        <tr>
                                            <th style="width: 50%">TCP Amount:  </th>
                                            <td>
                                                <asp:Label runat="server" ID="txtDAS" Text="0.00" CssClass="pull-right" /></td>
                                        </tr>

                                        <tr>
                                            <th>Net Total Contract Price:</th>
                                            <td>
                                                <asp:Label runat="server" ID="txtNetTCP" Text="0.00" CssClass="pull-right" /></td>
                                        </tr>

                                        <tr runat="server" visible="false">
                                            <th>Miscellaneous:</th>
                                            <td>
                                                <asp:Label runat="server" ID="txtMisc" Text="0.00" CssClass="pull-right" /></td>
                                        </tr>

                                        <tr>
                                            <th>Total Discount Amount:</th>
                                            <td>
                                                <asp:Label runat="server" ID="txtDiscount" Text="0.00" CssClass="pull-right" /></td>
                                        </tr>

                                        <tr>
                                            <th>Total Discount Percent:</th>
                                            <td>
                                                <asp:Label runat="server" ID="txtDiscountPercent" Text="0.00" CssClass="pull-right" /></td>
                                        </tr>

                                        <tr class="hidden">
                                            <th>VAT:</th>
                                            <td>
                                                <asp:Label runat="server" ID="txtVat" Text="0.00" CssClass="pull-right" /></td>
                                        </tr>

                                    </table>










                                    <table class="table">
                                        <caption>Down Payment</caption>
                                        <tr>
                                            <th style="width: 50%">Down Payment Percent:</th>
                                            <td>
                                                <asp:Label runat="server" ID="txtDPPercent" Text="0" CssClass="pull-right" /></td>
                                        </tr>
                                        <tr>
                                            <th>Down Payment Amount:</th>
                                            <td>
                                                <asp:Label runat="server" ID="txtDPAmount" Text="0.00" CssClass="pull-right" /></td>
                                        </tr>

                                        <tr>
                                            <th>Reservation Fee:</th>
                                            <td>
                                                <asp:Label runat="server" ID="txtRsvFee" Text="0.00" CssClass="pull-right" /></td>
                                        </tr>
                                        <%--                                        <tr>
                                            <th>First Down Payment:</th>
                                            <td>
                                                <asp:Label runat="server" ID="txtFrstDP" Text="0.00" /></td>
                                        </tr>--%>
                                        <tr>
                                            <th>Net Down Payment:</th>
                                            <td>
                                                <asp:Label runat="server" ID="txtNetDP" Text="0.00" CssClass="pull-right" /></td>
                                        </tr>
                                        <tr>
                                            <th>Terms(mos):</th>
                                            <td>
                                                <asp:Label runat="server" ID="txtDPTerms" Text="0.00" CssClass="pull-right" /></td>
                                        </tr>
                                        <tr>
                                            <th>Monthly DP:</th>
                                            <td>
                                                <asp:Label runat="server" ID="txtMonthlyDP" Text="0.00" CssClass="pull-right" /></td>
                                        </tr>
                                        <tr>
                                            <th>Due Date:</th>
                                            <td>
                                                <asp:Label runat="server" ID="txtDPDueDate" Text="0.00" CssClass="pull-right" /></td>
                                        </tr>
                                    </table>








                                    <table class="table">
                                        <caption>Loanable</caption>
                                        <tr style="display: none;">
                                            <th style="width: 50%">Loanable Percent:</th>
                                            <td>
                                                <asp:Label runat="server" ID="txtLPercent" Text="0" CssClass="pull-right" /></td>
                                        </tr>
                                        <tr>
                                            <th style="width: 50%">Loanable Amount:</th>
                                            <td>
                                                <asp:Label runat="server" ID="txtLAmount" Text="0.00" CssClass="pull-right" /></td>
                                        </tr>
                                        <tr>
                                            <th>Terms(mos):</th>
                                            <td>
                                                <asp:Label runat="server" ID="txtLTerms" Text="" CssClass="pull-right" /></td>
                                        </tr>
                                        <tr>
                                            <th>Interest Rate:</th>
                                            <td>
                                                <asp:Label runat="server" ID="txtRate" Text="0.00" CssClass="pull-right" /></td>
                                        </tr>
                                        <tr>
                                            <th>Monthly Amortization:</th>
                                            <td>
                                                <asp:Label runat="server" ID="txtMonthlyAmort" Text="0.00" CssClass="pull-right" /></td>
                                        </tr>














                                        <table class="table">
                                            <caption>Miscellaneous</caption>
                                            <tr>
                                                <th>Misc Financing Scheme:</th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblMiscFinancingScheme" Text="-" CssClass="pull-right" /></td>
                                            </tr>
                                            <tr>
                                                <th>Due Date:</th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblMiscDueDate" Text="0.00" CssClass="pull-right" /></td>
                                            </tr>
                                            <tr>
                                                <th style="width: 50%">Miscellaneous Fees:</th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblMiscFees" Text="0.00" CssClass="pull-right" /></td>
                                            </tr>
                                            <tr>
                                                <th style="width: 50%">Misc DP Amount:</th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblMiscDPAmount" Text="0.00" CssClass="pull-right" /></td>
                                            </tr>
                                            <tr>
                                                <th style="width: 50%">Misc Monthly DP:</th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblMiscDPMonthly" Text="0.00" CssClass="pull-right" /></td>
                                            </tr>
                                            <tr>
                                                <th>Misc DP Terms(mos):</th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblMiscDPTerms" Text="" CssClass="pull-right" /></td>
                                            </tr>

                                            <tr>
                                                <th style="width: 50%">Misc LB Amount:</th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblMiscLBAmount" Text="0.00" CssClass="pull-right" /></td>
                                            </tr>
                                            <tr>
                                                <th style="width: 50%">Misc Monthly LB:</th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblMiscLBMonthly" Text="0.00" CssClass="pull-right" /></td>
                                            </tr>
                                            <tr>
                                                <th>Misc LB Terms(mos):</th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblMiscLBTerms" Text="" CssClass="pull-right" /></td>
                                            </tr>


                                        </table>

                                    </table>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="modals" runat="server">
    <div id="modalQuotation" class="modal fade" role="dialog">
        <div class="modal-dialog modal-lg" style="width: 80%;">
            <div class="modal-content">
                <div class="modal-header" style="background-color: dodgerblue">
                    <%--<button type="button" class="close" data-dismiss="modal">&times;</button>--%>
                    <h4 class="modal-title" style="color: white;">Restructured Contracts List</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-lg-12">
                                    <asp:Panel runat="server" DefaultButton="bSearch">
                                        <div class="input-group">
                                            <input runat="server" id="txtSearch" placeholder="Search..." class="form-control" type="text" autofocus /><span class="input-group-btn">
                                                <asp:LinkButton runat="server" ID="bSearch" Style="width: 50px;" CssClass="btn btn-secondary btn-default btn-facebook" OnClick="bSearch_ServerClick"><i class="fa fa-search"></i></asp:LinkButton>
                                            </span>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-lg-12">
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:GridView runat="server"
                                                ID="gvQuotationList"
                                                CssClass="table table-responsive table-hover"
                                                AutoGenerateColumns="false"
                                                GridLines="None"
                                                ShowHeader="true"
                                                AllowPaging="true"
                                                PageSize="8"
                                                SelectedRowStyle-BackColor="#A1DCF2"
                                                OnRowCommand="gvQuotationList_RowCommand"
                                                OnRowDataBound="gvQuotationList_RowDataBound"
                                                OnPageIndexChanging="gvQuotationList_PageIndexChanging"
                                                EmptyDataText="No records found">
                                                <HeaderStyle BackColor="#333333" ForeColor="White" Font-Bold="true" />
                                                <Columns>
                                                    <asp:BoundField DataField="DocEntry" HeaderText="Quo No." ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                                    <asp:BoundField DataField="DocNum" HeaderText="Quo No." />
                                                    <asp:BoundField DataField="CardCode" HeaderText="BP" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                                    <asp:BoundField DataField="Name" HeaderText="Name" />
                                                    <asp:BoundField DataField="DocDate" HeaderText="Doc Date" DataFormatString="{0:MM/dd/yyyy}" />
                                                    <asp:BoundField DataField="ProjCode" HeaderText="Project Code" />
                                                    <asp:BoundField DataField="Block" HeaderText="Block" />
                                                    <asp:BoundField DataField="Lot" HeaderText="Lot" />
                                                    <asp:BoundField DataField="Model" HeaderText="Model" />
                                                    <asp:BoundField DataField="RSTDocEntry" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                                    <%-- <asp:BoundField DataField="Broker" HeaderText="Broker" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                                    <asp:BoundField DataField="Agent" HeaderText="SalesAgent" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />--%>
                                                    <%--                                                  <asp:BoundField DataField="FinScheme" HeaderText="FinScheme" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                                    <asp:BoundField DataField="AcctType" HeaderText="AcctType" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                                    <asp:BoundField DataField="SalesType" HeaderText="SalesType" ItemStyle-CssClzass="Hide" HeaderStyle-CssClass="Hide" />
                                                    <asp:BoundField DataField="SellingPrice" HeaderText="Selling Price" DataFormatString="{0:N}" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                                    <asp:BoundField DataField="ForwardedStatus" HeaderText="ForwardedStatus" DataFormatString="{0:N}" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText=" " />--%>
                                                    <asp:BoundField DataField="RestructuringType" HeaderText="Restructuring Type" />
                                                    <asp:ButtonField ButtonType="Link" ControlStyle-CssClass="btn btn-default btn-success btn-sm fa fa-arrow-right" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" CommandName="Sel" />
                                                </Columns>
                                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                                <PagerStyle CssClass="pagination-ys" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button runat="server" id="btnClose" type="button" class="btn btn-danger" onserverclick="btnClose_ServerClick">Cancel</button>
                </div>
            </div>
        </div>
    </div>
    <div id="modalPayment" class="modal fade" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Payment Means</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <button data-toggle="modal" data-target="#modalCheck" class="btn btn-primary" type="button">Add Check <i class="fa fa-plus"></i></button>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="panel panel-default">
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <asp:GridView runat="server"
                                            ID="gvChecks"
                                            CssClass="table table-responsive"
                                            AutoGenerateColumns="false"
                                            GridLines="None"
                                            ShowHeader="true"
                                            OnRowCommand="gvChecks_RowCommand"
                                            EmptyDataText="No Checks Added">
                                            <HeaderStyle BackColor="#66ccff" />
                                            <Columns>
                                                <asp:BoundField DataField="LineNum" HeaderText="LineNum" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                                <asp:BoundField DataField="CheckNo" HeaderText="Check No." />
                                                <asp:BoundField DataField="CheckAmt" HeaderText="Amount" />
                                                <asp:BoundField DataField="Bank" HeaderText="Bank" />
                                                <asp:BoundField DataField="Branch" HeaderText="Branch" />
                                                <asp:BoundField DataField="DueDate" HeaderText="DueDate" />
                                                <asp:BoundField DataField="AccountNum" HeaderText="AccountNum" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                                <asp:ButtonField ButtonType="Link" ControlStyle-CssClass="btn btn-danger fa fa-trash" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" CommandName="Del" />
                                            </Columns>
                                            <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                            <PagerStyle CssClass="pagination-ys" />
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-lg-8">
                                    <h5 class="pull-right">Total Check Amount:</h5>
                                </div>
                                <div class="col-lg-4">
                                    <div class="input-group">
                                        <span class="input-group-addon">PHP</span>
                                        <asp:TextBox runat="server" CssClass="form-control" ID="txtCheckTotal" Style="text-align: right;" Enabled="false" Text="0.00" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-8">
                                    <h5 class="pull-right">Cash Amount:</h5>
                                </div>
                                <div class="col-lg-4">
                                    <div class="input-group">
                                        <span class="input-group-addon">PHP</span>
                                        <asp:TextBox runat="server" CssClass="form-control txtCashAmount" AutoPostBack="true" ID="txtCashAmount" TextMode="Number" Style="text-align: right;" OnTextChanged="txtCashAmount_TextChanged" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-8">
                                    <h5 class="pull-right">Total Amount:</h5>
                                </div>
                                <div class="col-lg-4">
                                    <div class="input-group">
                                        <span class="input-group-addon">PHP</span>
                                        <asp:TextBox runat="server" CssClass="form-control" AutoPostBack="true" ID="txtTotalAmount" Style="text-align: right;" Enabled="false" Text="0.00" />
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
    <div id="modalCash" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Enter Cash Amount</h4>
                </div>
                <div class="modal-body">
                    <asp:Panel runat="server" DefaultButton="bAddCash">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <div class="input-group">
                                    <asp:TextBox runat="server" CssClass="form-control" placeholder="0" ID="txtCashTotal"></asp:TextBox>
                                    <span class="input-group-btn">
                                        <asp:LinkButton runat="server" ID="bAddCash" Style="width: 100px;" Text="Add" CssClass="btn btn-secondary btn-primary btn-facebook" OnClick="bAddCash_Click"></asp:LinkButton>
                                    </span>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
    <div id="modalCheck" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Enter Check Details</h4>
                </div>
                <div class="modal-body">
                    <asp:Panel runat="server" DefaultButton="btnAddCheck">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Check No.:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <input type="number" style="z-index: auto;" class="form-control txtCheckNo" runat="server" id="txtCheckNo" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Check Amount:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <div class="input-group">
                                            <span class="input-group-addon">PHP</span>
                                            <asp:TextBox runat="server" CssClass="form-control txtCheckAmount" AutoPostBack="true" ID="txtCheckAmount" TextMode="Number" Style="text-align: right;" OnTextChanged="txtCheckAmount_TextChanged" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>DueDate:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <asp:TextBox runat="server" CssClass="form-control" ID="txtCheckDate" TextMode="Date" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Bank Code:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <input type="text" style="z-index: auto;" class="form-control txtBankCode" runat="server" id="txtBankCode" disabled /><span class="input-group-btn">
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Bank:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <div class="input-group">
                                            <input type="text" style="z-index: auto;" class="form-control txtBank" runat="server" id="txtBank" disabled /><span class="input-group-btn">
                                                <button style="width: 50px; z-index: auto;" onclick="showBank();" class="btn btn-secondary btn-dropbox" type="button"><i class="fa fa-bars"></i></button>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Branch:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <div class="input-group">
                                            <input type="text" style="z-index: auto;" class="form-control txtBranch" runat="server" id="txtBranch" disabled /><span class="input-group-btn">
                                                <button id="branch" runat="server" style="width: 50px; z-index: auto;" class="btn btn-secondary btn-dropbox" type="button" onserverclick="branch_ServerClick"><i class="fa fa-bars"></i></button>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Account:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <input type="text" style="z-index: auto;" class="form-control txtAccount" runat="server" id="txtAccount" disabled />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-8 col-lg-offset-4">
                                        <asp:Button runat="server" ID="btnAddCheck" CssClass="btn btn-info" Text="Add Check" Width="100%" OnClick="btnAddCheck_Click" />
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
    <div id="modalCredit" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Enter Credit Details</h4>
                </div>
                <div class="modal-body">
                    <asp:Panel runat="server" DefaultButton="btnAddCredit">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <input type="text" style="z-index: auto;" class="form-control hide" runat="server" id="txtCreditCardCode" disabled />
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Credit Card:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <div class="input-group">
                                            <input type="text" style="z-index: auto;" class="form-control" runat="server" id="txtCreditCard" disabled />
                                            <span class="input-group-btn">
                                                <button runat="server" id="btnShowCredit" style="width: 50px; z-index: auto;" onserverclick="btnShowCredit_ServerClick" class="btn btn-secondary btn-dropbox" type="button"><i class="fa fa-bars"></i></button>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Credit Card No.:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <input type="text" style="z-index: auto;" class="form-control" runat="server" id="txtCreditCardNum" maxlength="4" placeholder="Enter last 4 digit only" /><span class="input-group-btn">
                                    </div>
                                </div>
                                <input type="text" style="z-index: auto; display: none;" class="form-control" runat="server" id="txtCreditAccountCode" disabled />
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Credit Account:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <div class="input-group">
                                            <input type="text" style="z-index: auto;" class="form-control" runat="server" id="txtCreditAccount" disabled /><span class="input-group-btn">
                                                <button runat="server" id="btnAccounts" style="width: 50px; z-index: auto;" onserverclick="btnAccounts_ServerClick" class="btn btn-secondary btn-dropbox"><i class="fa fa-bars"></i></button>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Credit Amount:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <div class="input-group">
                                            <span class="input-group-addon">PHP</span>
                                            <asp:TextBox runat="server" CssClass="form-control" AutoPostBack="true" ID="txtCreditAmount" MaxLength="10" TextMode="Number" Style="text-align: right;" OnTextChanged="txtCheckAmount_TextChanged" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Expiry Date: (MM/YY)</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <asp:TextBox runat="server" CssClass="form-control " ID="txtValidUntil" placeholder="MM/YY" MaxLength="5" />
                                    </div>
                                </div>
                                <input type="text" style="z-index: auto;" class="form-control hide" runat="server" id="txtCreditMethodCode" disabled />
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Payment Method:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <div class="input-group">
                                            <input type="text" style="z-index: auto;" class="form-control" runat="server" id="txtCreditMethod" disabled /><span class="input-group-btn">
                                                <button style="width: 50px; z-index: auto;" onclick="showPymtMethod();" class="btn btn-secondary btn-dropbox" type="button"><i class="fa fa-bars"></i></button>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>No. of Payments:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <input type="number" style="z-index: auto;" class="form-control" runat="server" id="txtNoOfPayments" value="1" maxlength="2" /><span class="input-group-btn">
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Voucher Number:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <input type="text" style="z-index: auto;" class="form-control" runat="server" maxlength="20" id="txtVoucherNum" /><span class="input-group-btn">
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>ID Number:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <input type="text" style="z-index: auto;" class="form-control" maxlength="15" runat="server" id="txtIDNum" /><span class="input-group-btn">
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Tel. No:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <input type="text" style="z-index: auto;" class="form-control" maxlength="20" runat="server" id="txtTelNo" /><span class="input-group-btn">
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-8 col-lg-offset-4">
                                        <asp:Button runat="server" ID="btnAddCredit" CssClass="btn btn-info" Text="Add Credit" Width="100%" OnClick="btnAddCredit_Click" />
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
    <div id="modalBank" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header modal-header-primary">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Bank</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:GridView runat="server"
                                ID="gvBanks"
                                CssClass="table table-responsive"
                                AutoGenerateColumns="false"
                                GridLines="None"
                                ShowHeader="false"
                                AllowPaging="true"
                                PageSize="10"
                                OnRowCommand="gvBanks_RowCommand"
                                OnPageIndexChanging="gvBanks_PageIndexChanging"
                                EmptyDataText="No records found">
                                <HeaderStyle BackColor="#66ccff" />
                                <Columns>
                                    <asp:BoundField DataField="BankCode" HeaderText="Code" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                    <asp:BoundField DataField="BankName" HeaderText="Bank" />
                                    <asp:ButtonField ButtonType="Link" ControlStyle-CssClass="btn btn-default btn-success fa fa-hand-o-up" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" CommandName="Sel" />
                                </Columns>
                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                <PagerStyle CssClass="pagination-ys" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div id="modalBranch" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header modal-header-primary">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Branch</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:GridView runat="server"
                                ID="gvBranch"
                                CssClass="table table-responsive"
                                AutoGenerateColumns="false"
                                GridLines="None"
                                ShowHeader="false"
                                AllowPaging="false"
                                OnRowCommand="gvBranch_RowCommand"
                                EmptyDataText="No records found">
                                <HeaderStyle BackColor="#66ccff" />
                                <Columns>
                                    <asp:BoundField DataField="Account" HeaderText="Code" />
                                    <asp:BoundField DataField="Branch" HeaderText="Bank" />
                                    <asp:ButtonField ButtonType="Link" ControlStyle-CssClass="btn btn-default btn-success fa fa-hand-o-up" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" CommandName="Sel" />
                                </Columns>
                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                <PagerStyle CssClass="pagination-ys" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div id="modalAccounts" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header modal-header-primary">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">G/L Accounts</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-lg-12">
                                    <asp:Panel runat="server" DefaultButton="btnSearchAccounts">
                                        <div class="input-group">
                                            <input runat="server" id="txtSearchAccounts" placeholder="Search..." class="form-control" type="text" autofocus /><span class="input-group-btn">
                                                <asp:LinkButton runat="server" ID="btnSearchAccounts" Style="width: 50px;" CssClass="btn btn-secondary btn-default btn-facebook" OnClick="btnSearchAccounts_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                            </span>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                            <br />
                            <asp:GridView runat="server"
                                ID="gvAccounts"
                                CssClass="table table-hover table-responsive"
                                AutoGenerateColumns="false"
                                GridLines="None"
                                ShowHeader="true"
                                AllowPaging="true"
                                PageSize="10"
                                OnRowCommand="gvAccounts_RowCommand"
                                OnPageIndexChanging="gvAccounts_PageIndexChanging"
                                EmptyDataText="No records found">
                                <HeaderStyle BackColor="#66ccff" />
                                <Columns>
                                    <asp:BoundField DataField="AcctCode" HeaderText="Code" />
                                    <asp:BoundField DataField="AcctName" HeaderText="Name" />
                                    <asp:ButtonField ButtonType="Link" ControlStyle-CssClass="btn btn-default btn-success fa fa-arrow-right" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" CommandName="Sel" />
                                </Columns>
                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                <PagerStyle CssClass="pagination-ys" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div id="modalCreditCard" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header modal-header-primary">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Credit Cards</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:GridView runat="server"
                                ID="gvCreditCard"
                                CssClass="table table-hover table-responsive"
                                AutoGenerateColumns="false"
                                GridLines="None"
                                ShowHeader="true"
                                AllowPaging="false"
                                OnRowCommand="gvCreditCard_RowCommand"
                                EmptyDataText="No records found">
                                <HeaderStyle BackColor="#66ccff" />
                                <Columns>
                                    <asp:BoundField DataField="CreditCard" HeaderText="Code" />
                                    <asp:BoundField DataField="CardName" HeaderText="Name" />
                                    <asp:BoundField DataField="CompanyID" HeaderText="Company" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                                    <asp:BoundField DataField="AcctCode" HeaderText="Account Code" />
                                    <asp:BoundField DataField="AcctName" HeaderText="Account Name" />
                                    <asp:ButtonField ButtonType="Link" ControlStyle-CssClass="btn btn-default btn-success fa fa-arrow-right" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" CommandName="Sel" />
                                </Columns>
                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                <PagerStyle CssClass="pagination-ys" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div id="modalCreditPaymentMethod" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header modal-header-primary">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Credit Card Payment Method</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:GridView runat="server"
                                ID="gvPymtMethod"
                                CssClass="table table-hover table-responsive"
                                AutoGenerateColumns="false"
                                GridLines="None"
                                ShowHeader="true"
                                AllowPaging="false"
                                OnRowCommand="gvPymtMethod_RowCommand"
                                EmptyDataText="No records found">
                                <HeaderStyle BackColor="#66ccff" />
                                <Columns>
                                    <asp:BoundField DataField="CrTypeCode" HeaderText="Code" />
                                    <asp:BoundField DataField="CrTypeName" HeaderText="Name" />
                                    <asp:ButtonField ButtonType="Link" ControlStyle-CssClass="btn btn-default btn-success fa fa-arrow-right" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" CommandName="Sel" />
                                </Columns>
                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                <PagerStyle CssClass="pagination-ys" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>





























    <div id="modalSchedule" class="modal fade" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header modal-header-primary hidden">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Payment Schedule</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>


                            <asp:Panel ID="divAddCharge" runat="server" Visible="false">
                                <div class="row">
                                    <h4 style="text-align: center;" id="dAddCharge" runat="server">ADDITIONAL CHARGES</h4>
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <asp:GridView runat="server" ID="gvAdditionalCharges"
                                                    CssClass="table table-hover table-responsive"
                                                    AutoGenerateColumns="false"
                                                    AllowPaging="true"
                                                    PageSize="12"
                                                    OnPageIndexChanging="gvAdditionalCharges_PageIndexChanging"
                                                    GridLines="None"
                                                    ShowHeader="true">
                                                    <HeaderStyle BackColor="#66ccff" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Terms" HeaderText="Terms" />
                                                        <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd MMM yyyy}" />
                                                        <asp:BoundField DataField="PaymentAmount" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Payment Amount" />
                                                        <%--<asp:BoundField DataField="Penalty" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Penalty" />--%>
                                                        <%--<asp:BoundField DataField="Misc" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Misc" />--%>
                                                        <%--<asp:BoundField DataField="InterestRate" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Interest" />--%>
                                                        <%--<asp:BoundField DataField="Principal" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Principal" />--%>
                                                        <%--<asp:BoundField DataField="UnAll" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="UnAllocated" />--%>
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




                            <asp:Panel ID="divMonthlyDP" runat="server">
                                <div class="row">
                                    <h4 style="text-align: center;" id="dDown" runat="server">MONTHLY DOWNPAYMENT SCHEDULE</h4>
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
                                                        <asp:BoundField DataField="Terms" HeaderText="Terms" />
                                                        <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd MMM yyyy}" />
                                                        <asp:BoundField DataField="PaymentAmount" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Payment Amount" />
                                                        <asp:BoundField DataField="Penalty" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Penalty" />
                                                        <%--<asp:BoundField DataField="Misc" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Misc" />--%>
                                                        <asp:BoundField DataField="InterestAmount" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Interest" />
                                                        <asp:BoundField DataField="Principal" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Principal" />
                                                        <%--<asp:BoundField DataField="UnAll" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="UnAllocated" />--%>
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



                            <asp:Panel ID="divMonthlyAmort" runat="server">
                                <div class="row">
                                    <h4 style="text-align: center;" id="dAmort" runat="server">MONTHLY AMORTIZATION SCHEDULE</h4>
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
                                                        <asp:BoundField DataField="Terms" HeaderText="Terms" />
                                                        <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd MMM yyyy}" />
                                                        <asp:BoundField DataField="PaymentAmount" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Payment Amount" />
                                                        <asp:BoundField DataField="Penalty" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Penalty" />
                                                        <%--<asp:BoundField DataField="Misc" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Misc" />--%>
                                                        <asp:BoundField DataField="InterestAmount" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Interest" />
                                                        <asp:BoundField DataField="Principal" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Principal" />
                                                        <%--<asp:BoundField DataField="UnAll" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="UnAllocated" />--%>
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



                            <asp:Panel ID="divMonthlyDPMiscMiscDP" runat="server">
                                <div class="row">
                                    <h4 style="text-align: center;" id="dMiscDP" runat="server">MONTHLY MISCELLANEOUS DOWNPAYMENT SCHEDULE</h4>
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <asp:GridView runat="server" ID="gvMiscellaneousDP"
                                                    CssClass="table table-hover table-responsive"
                                                    AutoGenerateColumns="false"
                                                    AllowPaging="true"
                                                    PageSize="12"
                                                    OnPageIndexChanging="gvMiscellaneousDP_PageIndexChanging"
                                                    GridLines="None"
                                                    ShowHeader="true">
                                                    <HeaderStyle BackColor="#00cc66" />
                                                    <Columns>
                                                        <%--<asp:BoundField DataField="Terms" HeaderText="Terms" />
                                                        <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd MMM yyyy}" />
                                                        <asp:BoundField DataField="PaymentAmount" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Payment Amount" />--%>
                                                        <%--<asp:BoundField DataField="Penalty" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Penalty" />--%>
                                                        <%--<asp:BoundField DataField="Misc" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Misc" />--%>
                                                        <%--<asp:BoundField DataField="InterestRate" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Interest" />--%>
                                                        <%--<asp:BoundField DataField="Principal" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Principal" />--%>
                                                        <%--<asp:BoundField DataField="UnAll" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="UnAllocated" />--%>
                                                        <%--<asp:BoundField DataField="Balance" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Balance" />--%>
                                                        <asp:BoundField DataField="Terms" HeaderText="Line Item No." />
                                                        <asp:BoundField DataField="PaymentType" HeaderText="Payment Type" />
                                                        <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd MMM yyyy}" />
                                                        <asp:BoundField DataField="PaymentAmount" DataFormatString="{0:###,###,##0.00}" HeaderText="Payment Amount" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" />
                                                        <asp:BoundField DataField="Principal" DataFormatString="{0:###,###,##0.00}" HeaderText="Principal" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" />
                                                        <asp:BoundField DataField="InterestAmount" DataFormatString="{0:###,###,##0.00}" HeaderText="Interest" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" />
                                                        <asp:BoundField DataField="Balance" DataFormatString="{0:###,###,##0.00}" HeaderText="Balance" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" />
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
        </div>
    </div>






    <div id="modalCommissionScheme" class="modal fade" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header modal-header-primary">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Commission Scheme</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>

                            <asp:Panel ID="Panel1" runat="server">
                                <div class="row">
                                    <h4 style="text-align: center;" id="H2" runat="server">COMMISSION SCHEME</h4>
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <asp:GridView runat="server" ID="gvCommissionScheme"
                                                    CssClass="table table-hover table-responsive"
                                                    AutoGenerateColumns="false"
                                                    AllowPaging="true"
                                                    PageSize="12"
                                                    OnPageIndexChanging="gvCommissionScheme_PageIndexChanging"
                                                    GridLines="None"
                                                    ShowHeader="true">
                                                    <HeaderStyle BackColor="#66ccff" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Release" HeaderText="Release" ItemStyle-CssClass="text-left" HeaderStyle-CssClass="text-center" />
                                                        <asp:BoundField DataField="Type" HeaderText="Type" />
                                                        <asp:BoundField DataField="ProjCode" HeaderText="Project Code" />
                                                        <asp:BoundField DataField="CollectedTCP" HeaderText="Collected TCP" />
                                                        <%--<asp:BoundField DataField="Release" DataFormatString="{0:###,###,##0.00}" HeaderText="Payment Amount" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" />--%>
                                                        <asp:BoundField DataField="CommissionRelease" DataFormatString="{0:###,###,##0.00}" HeaderText="Commission Release" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" />
                                                        <asp:BoundField DataField="CommissionPercent" DataFormatString="{0:###,###,##0.00}" HeaderText="Commission Percent" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" />
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
        </div>
    </div>




    <div id="modalIncentiveScheme" class="modal fade" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header modal-header-primary">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Incentive Scheme</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>

                            <asp:Panel ID="Panel2" runat="server">
                                <div class="row">
                                    <h4 style="text-align: center;" id="H3" runat="server">INCENTIVE SCHEME</h4>
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <asp:GridView runat="server" ID="gvIncentiveScheme"
                                                    CssClass="table table-hover table-responsive"
                                                    AutoGenerateColumns="false"
                                                    AllowPaging="true"
                                                    PageSize="12"
                                                    OnPageIndexChanging="gvIncentiveScheme_PageIndexChanging"
                                                    GridLines="None"
                                                    ShowHeader="true">
                                                    <HeaderStyle BackColor="#66ccff" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Release" HeaderText="Release" ItemStyle-CssClass="text-left" HeaderStyle-CssClass="text-center" />
                                                        <asp:BoundField DataField="Type" HeaderText="Type" />
                                                        <asp:BoundField DataField="ProjCode" HeaderText="Project Code" />
                                                        <asp:BoundField DataField="IncentiveAmount" HeaderText="Incentive Amount" />
                                                        <asp:BoundField DataField="Position" HeaderText="Position" />

                                                        <asp:BoundField DataField="Amount" DataFormatString="{0:###,###,##0.00}" HeaderText="Amount" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" />
                                                        <asp:BoundField DataField="EffectivityDateFrom" DataFormatString="{0:###,###,##0.00}" HeaderText="Effectivity Date From" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" />
                                                        <asp:BoundField DataField="EffectivityDateTo" DataFormatString="{0:###,###,##0.00}" HeaderText="Effectivity Date To" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" />
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
        </div>
    </div>




    <div id="modalSharingDetails" class="modal fade" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header modal-header-primary">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Sharing Details</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>

                            <asp:Panel ID="Panel3" runat="server">
                                <div class="row">
                                    <h4 style="text-align: center;" id="H4" runat="server">SHARING DETAILS</h4>
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <asp:GridView runat="server" ID="gvSharingDetails"
                                                    CssClass="table table-hover table-responsive"
                                                    AutoGenerateColumns="false"
                                                    AllowPaging="true"
                                                    PageSize="12"
                                                    OnPageIndexChanging="gvSharingDetails_PageIndexChanging"
                                                    GridLines="None"
                                                    ShowHeader="true">
                                                    <HeaderStyle BackColor="#66ccff" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Position" HeaderText="Position" />
                                                        <asp:BoundField DataField="SalesPerson" HeaderText="Sales Person" />
                                                        <asp:BoundField DataField="Status" HeaderText="Status" />
                                                        <asp:BoundField DataField="OSLAID" HeaderText="OSLA ID" />

                                                        <asp:BoundField DataField="PercentageSharedDetails" HeaderText="Lot %" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" />
                                                        <asp:BoundField DataField="HouseAndLotPercentageSharedDetails" HeaderText="House & Lot %" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" />
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
        </div>
    </div>

























    <div class="modal fade" id="MsgRestructuringDocuments" data-backdrop="static" role="dialog" tabindex="-1">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog modal-lg" role="document">
                    <div class="modal-content">
                        <div class="modal-header bg-color">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                            <h4 class="modal-title">Restructuring Document Requirements
                            </h4>
                        </div>

                        <div class="modal-body">
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                    <div class="row">
                                        <div class="col-lg-12">

                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <fieldset>

                                                    <div class="row">
                                                        <asp:GridView
                                                            ID="gvRestructuringDocumentRequirement"
                                                            runat="server"
                                                            AllowPaging="true"
                                                            AutoGenerateColumns="false"
                                                            EmptyDataText="No Records Found"
                                                            CssClass="table table-bordered table-hover"
                                                            Width="100%"
                                                            ShowHeader="True"
                                                            PageSize="10"
                                                            OnRowCommand="gvRestructuringDocumentRequirement_RowCommand"
                                                            OnPageIndexChanging="gvRestructuringDocumentRequirement_PageIndexChanging">
                                                            <HeaderStyle BackColor="#66ccff" />


                                                            <Columns>

                                                                <asp:BoundField DataField="Code" HeaderText="Code" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />

                                                                <asp:BoundField DataField="U_Document" HeaderText="Document" ItemStyle-Width="40%" />

                                                                <asp:TemplateField HeaderText="Expiration Date" ItemStyle-Width="20%">
                                                                    <ItemTemplate>
                                                                        <div style="overflow: hidden; white-space: nowrap;">
                                                                            <asp:TextBox ID="lblExpirationDate" class="form-control" type="date" runat="server" />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderStyle-Width="5px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" HeaderText="Attachment">
                                                                    <ItemTemplate>
                                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <div class="form-group  center-block text-center ">
                                                                                    <asp:Label runat="server" ID="lblFileName1" Text=""></asp:Label>
                                                                                    <div class="row">
                                                                                        <asp:LinkButton ID="btnPreview1" CssClass="btn btn-info" Visible="false" runat="server" CommandName="Preview1" Text="Preview" OnClientClick="$('#MsgRestructuringDocuments').modal({'backdrop': 'static'});" />
                                                                                    </div>
                                                                                </div>
                                                                            </ContentTemplate>
                                                                            <Triggers>
                                                                                <asp:PostBackTrigger ControlID="btnPreview1" />
                                                                            </Triggers>
                                                                        </asp:UpdatePanel>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>



                                                            </Columns>


                                                            <PagerStyle CssClass="pagination-ys" />
                                                            <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" Font-Bold="true" />
                                                        </asp:GridView>
                                                    </div>
                                            </div>

                                            </fieldset>
                                        </div>



                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="modal-footer">
                            <button runat="server" style="width: 100px;" class="btn btn-default btn-danger" type="button" data-dismiss="modal">Close </button>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
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



</asp:Content>

<%@ Page Title="Downpayment" Language="C#" MasterPageFile="~/master/Main.Master" AutoEventWireup="true" CodeBehind="Downpayment.aspx.cs" Inherits="ABROWN_DREAMS.Downpayment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--MODALS--%>
    <script type="text/javascript">
        function showDPList() {
            $('#modalFind').modal('show');
        }
        function hideDPList() {
            $('#modalFind').modal('hide');
        }
        function showMsgPaymentmeans() {
            $('#MsgPaymentmeans').modal('show');
        }
        function hideMsgPaymentmeans() {
            $('#MsgPaymentmeans').modal('hide');
        }
        function showBranch() {
            $('#modalBranch').modal('show');
        }
        function hideBranch() {
            $('#modalBranch').modal('hide');
        }
        //modalPaymentMeans
        function showBank() {
            $('#modalBank').modal('show');
        }
        function hideBank() {
            $('#modalBank').modal('hide');
        }
        function showPaymentMeans() {
            $('#modalPaymentMeans').modal('show');
        }
        function hidePaymentMeans() {
            $('#modalPaymentMeans').modal('hide');
        }
        function showMsgBox() {
            $('#MsgBox').modal('show');
        }
        function hideMsgBox() {
            $('#MsgBox').modal('hide');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="sitemap" runat="server">
    <li><a href="#"><span>Downpayment</span></a></li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div class="panel panel-default panel-trans" id="form_cont">
        <div class="panel-heading">
            <div class="row">
                <div class="col-md-7 col-sm-6 col-xs-12">
                    <h4 class="trans-title">Downpayment</h4>
                </div>
                <div class="col-md-5 col-sm-6 col-xs-12">
                    <div class="trans-command">
                        <div class="btn-group" role="group">
                            <asp:UpdatePanel runat="server" ID="UpdatePanelButton">
                                <ContentTemplate>
                                    <button class="btn btn-success btn-width" type="button" runat="server" id="btnSave" onserverclick="btnSave_ServerClick">Save <i class="glyphicon glyphicon-check"></i></button>
                                    <button class="btn btn-danger btn-width" type="button" runat="server" id="btnCancel">Cancel <i class="glyphicon glyphicon-remove"></i></button>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-body">
            <div class="row">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                            <div class="row">
                                <div class="col-lg-5 col-md-5 col-sm-11 col-xs-11">
                                    <h5>Customer Code</h5>
                                </div>
                                <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                    <h5>:</h5>
                                </div>
                                <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                    <div class="input-group">
                                        <input type="text" style="z-index: auto;" class="form-control txtCardCode" runat="server" id="txtCardCode" disabled /><span class="input-group-btn">
                                            <button style="width: 50px; z-index: auto;" runat="server" id="bCardCode" class="btn btn-secondary btn-dropbox" onserverclick="btnFind_ServerClick" type="button"><i class="fa fa-bars"></i></button>
                                        </span>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-5 col-md-5 col-sm-11 col-xs-11">
                                    <h5>Customer Name</h5>
                                </div>
                                <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                    <h5>:</h5>
                                </div>
                                <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                    <input type="text" style="z-index: auto;" class="form-control txtCardName" runat="server" id="txtCardName" disabled />
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                            <div class="row">
                                <div class="col-lg-5 col-md-5 col-sm-11 col-xs-11">
                                    <h5>Document Number</h5>
                                </div>
                                <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                    <h5>:</h5>
                                </div>
                                <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                    <div class="input-group">
                                        <input type="text" style="z-index: auto;" class="form-control txtDocNum" runat="server" id="txtDocNum" disabled /><span class="input-group-btn">
                                            <button style="width: 50px; z-index: auto;" runat="server" id="bDocNum" onserverclick="btnFind_ServerClick" class="btn btn-secondary btn-dropbox" type="button"><i class="fa fa-bars"></i></button>
                                        </span>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-5 col-md-5 col-sm-11 col-xs-11">
                                    <h5>Document Date</h5>
                                </div>
                                <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                                    <h5>:</h5>
                                </div>
                                <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                    <input type="date" class="form-control" runat="server" id="txtDocDate" />
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <div class="row">
                            </div>
                        </div>
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <div class="row">
                            </div>
                        </div>
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <div class="row">
                            </div>
                        </div>
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <ul class="nav nav-tabs" id="myTab1">
                                <li class="active"><a href="#TabDownpayment" role="tab" data-toggle="tab" style="width: 120px; text-align: center;">Downpayment</a></li>
                                <button class="btn btn-primary pull-right" runat="server" id="btnPaymentMeans" onserverclick="btnPaymentMeans_ServerClick">Payment Means</button>
                            </ul>

                            <div class="tab-content">
                                <div class="tab-pane fade in active tab-bg" role="tabpanel" id="TabDownpayment">
                                    <div class="container tab-container">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <div class="row">
                                            </div>
                                            <div class="row">
                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView runat="server" ID="gvDownpayment"
                                                            CssClass="table table-hover table-responsive"
                                                            AutoGenerateColumns="false"
                                                            AllowPaging="true"
                                                            PageSize="12"
                                                            OnPageIndexChanging="gvDownpayment_PageIndexChanging"
                                                            GridLines="None"
                                                            EmptyDataText="No Records Found"
                                                            ShowHeader="true">
                                                            <HeaderStyle BackColor="#66ccff" />
                                                            <Columns>
<%--                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="SelectAll" runat="server"  OnCheckedChanged="SelectAll_CheckedChanged"
                                                                            AutoPostBack="true" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkRow" runat="server" AutoPostBack="true"/>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>--%>
                                                                <asp:BoundField DataField="Terms" HeaderText="Terms" />
                                                                <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:MM-dd-yyyy}" />
                                                                <asp:BoundField DataField="MonthlyAmort" DataFormatString="{0:###,###,##0.##}" HeaderText="Monthly Amortization" />
                                                                <asp:BoundField DataField="Principal" DataFormatString="{0:###,###,##0.##}" HeaderText="Principal" />
                                                                <asp:BoundField DataField="Interest" DataFormatString="{0:#0.##}" HeaderText="Interest" />
                                                                <asp:BoundField DataField="ActualPay" DataFormatString="{0:###,###,##0.##}" HeaderText="Actual Payment" />
                                                                <asp:BoundField DataField="Balance" DataFormatString="{0:###,###,##0.##}" HeaderText="Balance" />
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
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="modals" runat="server">
    <div class="modal fade" id="modalFind" role="dialog" tabindex="-1">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog modal-lg" role="document">
                    <div class="modal-content">
                        <div class="modal-header bg-color">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                            <h4 class="modal-title">Downpayment List</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <fieldset>
                                                <asp:GridView runat="server" ID="gvDPList"
                                                    AllowPaging="true"
                                                    AllowSorting="true"
                                                    EmptyDataText="No Records Found"
                                                    Width="100%"
                                                    CssClass="table table-hover table-responsive"
                                                    AutoGenerateColumns="false"
                                                    GridLines="None"
                                                    PageSize="8" OnPageIndexChanging="gvDPList_PageIndexChanging">
                                                    <HeaderStyle BackColor="#66ccff" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Name" HeaderText="Buyers Name" SortExpression="Name" ItemStyle-Font-Size="Medium" />
                                                        <asp:BoundField DataField="DocNum" HeaderText="Document Number" SortExpression="DocNum" ItemStyle-Font-Size="Medium" />
                                                        <asp:BoundField DataField="DocTotal" HeaderText="Total DP" SortExpression="DocTotal" DataFormatString="PHP {0:###,###,###.00}" ItemStyle-Font-Size="Medium" />
                                                        <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:LinkButton runat="server" ID="bSelectDP" CssClass="btn btn-default btn-success" Width="100%" Height="100%" OnClick="bSelectDP_Click" CommandArgument='<%# Bind("DocEntry")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
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
                        </div>
                        <div class="modal-footer">
                            <button class="btn btn-default btn-facebook" type="button" style="width: 100px;" data-dismiss="modal">OK</button>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

   <%-- <div class="modal fade" id="MsgPaymentmeans" role="dialog" tabindex="-1">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header bg-color">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                            <h4 class="modal-title">Payment Means
                            </h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <h5>Cash on hand:</h5>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <input runat="server" type="number" class="form-control" id="tPayment" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button runat="server" id="btnOk" style="width: 100px;" class="btn btn-primary" type="button" onserverclick="btnOk_ServerClick">OK </button>
                            <button runat="server" style="width: 100px;" class="btn btn-default" type="button" data-dismiss="modal">Cancel </button>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>--%>

    <div id="modalPaymentMeans" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header modal-header-primary">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Payment Means</h4>
                </div>
                <div class="modal-body">
                    <div class="row" style="padding: 5px;">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <div class="col-lg-12">
                                    <div class="row">
                                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                            <div class="row">
                                                <button runat="server" id="bTabCash" class="btn btn-success" type="button" style="width: 100%;" onserverclick="bTab_ServerClick">Cash</button>
                                            </div>
                                        </div>
                                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                            <div class="row">
                                                <button runat="server" id="bTabCheck" class="btn btn-default" type="button" style="width: 100%;" onserverclick="bTab_ServerClick">Check</button>
                                            </div>
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="tab-content" style="overflow: hidden;">
                                        <div class="tab-pane active" role="tabpanel" id="TabCash" runat="server">
                                            <div class="row">
                                                <div class="col-lg-4">
                                                    <h5>Cash Amount:</h5>
                                                </div>
                                                <div class="col-lg-8">
                                                    <div class="input-group">
                                                        <span class="input-group-addon">PHP</span>
                                                        <asp:TextBox runat="server" CssClass="form-control txtCashAmount" AutoPostBack="true" ID="txtCashAmount" TextMode="Number" Style="text-align: right;" OnTextChanged="txtCashAmount_TextChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="tab-pane" role="tabpanel" id="TabCheck" runat="server">
                                            <div class="row">
                                                <div class="col-lg-4">
                                                    <h5>Check Amount:</h5>
                                                </div>
                                                <div class="col-lg-8">
                                                    <div class="input-group">
                                                        <span class="input-group-addon">PHP</span>
                                                        <asp:TextBox runat="server" CssClass="form-control txtCheckAmount" AutoPostBack="true" ID="txtCheckAmount" TextMode="Number" Style="text-align: right;" OnTextChanged="txtCashAmount_TextChanged" />
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
                                                    <h5>Bank Name:</h5>
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
                                                <div class="col-lg-4">
                                                    <h5>Check No.:</h5>
                                                </div>
                                                <div class="col-lg-8">
                                                    <input type="number" style="z-index: auto;" class="form-control txtCheckNo" runat="server" id="txtCheckNo" size="9" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <hr />
                    <div class="row" style="padding: 5px;">
                        <div class="col-lg-4">
                            <h5>Total:</h5>
                        </div>
                        <div class="col-lg-8">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <div class="input-group">
                                        <span class="input-group-addon">PHP</span>
                                        <asp:TextBox runat="server" CssClass="form-control txtTotalAmount" AutoPostBack="true" ID="txtTotalAmount" Style="text-align: right;" Enabled="false" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-success btn-width" runat="server" id="bPaymntmeans" onserverclick="bPaymntmeans_ServerClick"> OK </button>
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
                                AllowPaging="true"
                                PageSize="10"
                                OnRowCommand="gvBranch_RowCommand"
                                OnPageIndexChanging="gvBranch_PageIndexChanging"
                                EmptyDataText="No records found">
                                <HeaderStyle BackColor="#66ccff" />
                                <Columns>
                                    <asp:BoundField DataField="Account" HeaderText="Code" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                    <asp:BoundField DataField="Branch" HeaderText="Bank" />
                                    <asp:ButtonField ButtonType="Link" ControlStyle-CssClass="btn btn-default btn-success fa fa-hand-o-up" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" CommandName="Sel" />
                                </Columns>
                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                <PagerStyle CssClass="pagination-ys" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
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
                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                <PagerStyle CssClass="pagination-ys" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div id="modalAlert" class="modal fade">
        <div class="modal-dialog" style="width: 30%;" id="alertModal" runat="server">
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
        <div class="modal-dialog" style="width: 30%;">
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
                                    <h4><asp:Label runat="server" ID="lblConfirmationInfo"></asp:Label></h4>
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

<%@ Page Title="DREAMS | Waiver of Surcharge" Language="C#" MasterPageFile="~/master/Main.Master" AutoEventWireup="true" CodeBehind="Assessment.aspx.cs" Inherits="ABROWN_DREAMS.Assessment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function showBuyer() {
            $('#modalBuyers').modal({
                backdrop: 'static',
                keyboard: false
            });
        }
        function closeBuyer() {
            $('#modalBuyers').modal('hide');
        }
        function hideAlert() {
            $('#modalAlert').modal('hide');
        }
        function hideConfirm() {
            $('#modalConfirmation').modal('hide');
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            var charStr = String.fromCharCode(charCode);

            // Allow digits (0-9), backspace (8), and dot (.)
            if ((charCode >= 48 && charCode <= 57) || charCode == 8 || charStr == ".") {
                return true;
            } else {
                return false;
            }
        }

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
    <li><span>AMD</span></li>
    <li><span>Assessment</span></li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div class="col-lg-3">
        <div class="panel panel-default">
            <div class="panel-body">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="pnlBuyerInfo">
                            <div class="row">
                                <div class="col-lg-10">
                                    <div class="customer-badge" style="background-color: white;">
                                        <div class="avatar">
                                            <img src="../assets/img/user.png" alt="" width="50" height="50">
                                        </div>
                                        <div class="details">
                                            <a href="#">
                                                <asp:Label runat="server" ID="lblName" CssClass="text-uppercase" Style="font-size: 16px;" Text="" /></a>
                                            <br />
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
                            <br />
                            <img src="../assets/img/house.png" alt="" width="30" height="30" class="pull-left">
                            <h5 style="color: #67676C;">&nbsp;House/Lot:</h5>
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
                                        OnRowCommand="gvHouseLot_RowCommand"
                                        EmptyDataText="No records found">
                                        <Columns>
                                            <asp:BoundField DataField="SellingPrice" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                            <asp:BoundField DataField="DPEntry" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
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
                                                        <asp:Label runat="server" ID="lblFinscheme" CssClass="small text-uppercase" Text='<%# Eval("FinCode") %>' Visible="false" />
                                                        <asp:Label runat="server" ID="lblBlock1" CssClass="small text-uppercase" Text='<%# Eval("Block") %>' Visible="false" />
                                                        <asp:Label runat="server" ID="lblLot1" CssClass="small text-uppercase" Text='<%# Eval("Lot") %>' Visible="false" />
                                                        <asp:Label runat="server" ID="lblModel1" CssClass="small text-uppercase" Text='<%# Eval("Model") %>' Visible="false" />
                                                        <asp:Label runat="server" ID="lblFinschemeCode" CssClass="small text-uppercase" Text='<%# Eval("FinCode") %>' Visible="false" />
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
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <%--<button runat="server" id="btnEditProfile" class="btn btn-success pull-right">Edit &nbsp;<i class="fa fa-edit"></i></button>--%>
                                    <table class="table">
                                        <caption class="pull-left">Account Details</caption>
                                        <tr style="display: none;">
                                            <th style="width: 50%">First Name:</th>
                                            <td>
                                                <asp:Label runat="server" ID="lblFName" CssClass=" " Text="" />
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <th style="width: 50%">Last Name:</th>
                                            <td>
                                                <asp:Label runat="server" ID="lblLName" CssClass=" " Text="" />
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <th style="width: 50%">Middle Name:</th>
                                            <td>
                                                <asp:Label runat="server" ID="lblMName" CssClass=" " Text="" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <th style="width: 50%">Project:</th>
                                            <td>
                                                <asp:Label runat="server" ID="tProj" CssClass=" " Text="" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <th style="width: 50%">Phase:</th>
                                            <td>
                                                <asp:Label runat="server" ID="tPhase" CssClass=" " Text="" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>Model:</th>
                                            <td>
                                                <asp:Label runat="server" CssClass=" " ID="tModel" Text="" /></td>
                                        </tr>
                                        <tr>
                                            <th>Block:</th>
                                            <td>
                                                <asp:Label runat="server" CssClass=" " ID="tBlock" Text="" /></td>
                                        </tr>
                                        <tr>
                                            <th>Lot:</th>
                                            <td>
                                                <asp:Label runat="server" CssClass=" " ID="tLot" Text="" /></td>
                                        </tr>
                                        <tr>
                                            <th>Net TCP (PHP):</th>
                                            <td>
                                                <asp:Label runat="server" CssClass="label label-success" ID="tTcp" Text="" Font-Size="10" /></td>
                                        </tr>
                                        <tr>
                                            <th style="width: 50%">Fin. Scheme:</th>
                                            <td>
                                                <asp:Label runat="server" ID="tFinScheme" CssClass="" Text="" />
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <th>Fin. Scheme Code:</th>
                                            <td>
                                                <asp:Label runat="server" ID="tFinCode" CssClass="" Text="" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>


    <div class="col-lg-9">

        <div class="panel panel-default clearfix" style="margin-left: 10px;">
            <div class="panel-body">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>


                        <div class="row">


                            <div class="col-sm-3" style="margin-right: 20px; margin-left: 20px">
                                <div class="row">
                                    <h5>Total Penalties:</h5>
                                </div>

                                <div class="row">
                                    <div class="input-group">
                                        <span class="input-group-addon">PHP</span>
                                        <asp:TextBox ID="txtTotalPenalty" runat="server" Style="z-index: auto;" AutoPostBack="true" CssClass="form-control text-right" Enabled="false" />
                                    </div>
                                </div>
                            </div>


                            <div class="col-sm-3">
                                <div class="row">
                                    <h5>Document Date:</h5>
                                </div>

                                <div class="row">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtDocumentDate" type="date" runat="server" Style="z-index: auto;" AutoPostBack="true" CssClass="form-control text-right" Enabled="false" />
                                    </div>

                                </div>
                            </div>


                            <div class="col-sm-3">
                                <div class="row">
                                    <h5>Surcharge Date:</h5>
                                </div>

                                <div class="row">
                                    <div class="input-group">
                                        <asp:TextBox runat="server" ID="tSurChargeDate" CssClass="form-control" type="date"></asp:TextBox>
                                        <span class="input-group-btn">
                                            <asp:LinkButton runat="server" ID="btnSurchargeDate1" OnClick="btnSurchargeDate1_Click" CssClass="btn btn-secondary btn-circle"><i class="fa fa-search"></i></asp:LinkButton>
                                        </span>

                                    </div>
                                </div>
                            </div>



                            <div class="col-sm-2">

                                <div class="row pull-right">
                                    <div style="float: left;">
                                        <asp:Button runat="server" ID="btnUpdate" CssClass="btn btn-info" Text="Save" OnClick="btnUpdate_Click" Width="100%" />
                                    </div>
                                </div>
                                <div class="row">
                                </div>

                            </div>
                        </div>




                        <div class="row" style="height: 800px; overflow: auto;">
                            <div class="col-lg-12">
                                <asp:GridView runat="server"
                                    ID="gvDownPayments"
                                    CssClass="table table-responsive"
                                    AutoGenerateColumns="false"
                                    GridLines="None"
                                    ShowHeader="true"
                                    SelectedRowStyle-BackColor="#A1DCF2"
                                    UseAccessibleHeader="true"
                                    OnRowDataBound="gvDownPayments_RowDataBound"
                                    EmptyDataText="No records found">
                                    <HeaderStyle BackColor="#d7dce5" />
                                    <Columns>
                                        <asp:BoundField DataField="DocEntry" HeaderText="DocEntry" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                        <asp:BoundField DataField="Terms" HeaderText="Terms" />
                                        <asp:BoundField DataField="PaymentType" HeaderText="Type" />
                                        <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                        <asp:BoundField DataField="PaymentAmount" HeaderText="Amount Due" DataFormatString="{0:#,##0.00}" />
                                        <asp:BoundField DataField="Principal" HeaderText="Principal" DataFormatString="{0:#,##0.00}" />
                                        <%--5--%>

                                        <asp:BoundField DataField="InterestRate" HeaderText="Interest" DataFormatString="{0:#,##0.00}" />
                                        <asp:BoundField DataField="Penalty" HeaderText="SurCharge" DataFormatString="{0:#,##0.00}" ItemStyle-Font-Bold="true" ItemStyle-CssClass="text-danger" />
                                        <asp:BoundField DataField="Payment" HeaderText="Payment" DataFormatString="{0:#,##0.00}" ItemStyle-Font-Bold="true" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                        <asp:BoundField DataField="Balance" HeaderText="Balance" DataFormatString="{0:#,##0.00}" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />

                                        <asp:BoundField DataField="DPEntry" HeaderText="DPEntry" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                        <%--10--%>

                                        <asp:TemplateField HeaderText="Approved Surcharge Amount" ItemStyle-Width="150" HeaderStyle-Width="150" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtPenalty" onkeypress="return isNumberKey(event)" CssClass="form-control text-danger text-right"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="AmountPaid" HeaderText="Paid" DataFormatString="{0:#,##0.00}" ItemStyle-Font-Bold="true" />


                                        <asp:BoundField DataField="LineStatus" HeaderText="LineStatus" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                        <asp:TemplateField HeaderText="Cash Discount" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" AutoPostBack="true" ID="txtCashDiscount" OnTextChanged="txtCashDiscount_TextChanged"
                                                    CssClass="form-control text-danger text-right"
                                                    onkeypress="return isNumberKey(event)" Text='<%# Eval("CashDiscount", "{0:#,0.##}")%>'></asp:TextBox>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Discount %" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" AutoPostBack="true" ID="txtCashDiscountPercent" OnTextChanged="txtCashDiscountPercent_TextChanged"
                                                    onkeypress="return isNumberKey(event)" CssClass="form-control text-danger text-right"
                                                    Text='<%# Eval("CashDiscountPercent", "{0:#,0.##}")%>'></asp:TextBox>


                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Discount Valid Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide">
                                            <ItemTemplate>
                                                <%--                                                <asp:TextBox runat="server" type="date" AutoPostBack="true" ID="txtCashDiscountValidDate" class="form-control"
                                                    Text='<%# string.IsNullOrWhiteSpace(Eval("CashDiscountValidDate").ToString()) ? "" : 
                                                        Convert.ToDateTime( Eval("CashDiscountValidDate")).ToString("yyyy-MM-dd") %>'></asp:TextBox>--%>

                                                <%--#184 under CNC - Discount Valid Date fields unstable. Removed AutopostBack to prevent losing focus on textbox--%>
                                                <asp:TextBox runat="server" type="date" ID="txtCashDiscountValidDate" class="form-control"
                                                    Text='<%# string.IsNullOrWhiteSpace(Eval("CashDiscountValidDate").ToString()) ? "" : 
                                                        Convert.ToDateTime( Eval("CashDiscountValidDate")).ToString("yyyy-MM-dd") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                    <PagerStyle CssClass="pagination-ys" />
                                </asp:GridView>
                            </div>
                        </div>



                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="modals" runat="server">
    <div id="modalBuyers" class="modal fade" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header modal-header-primary">
                    <h4 class="modal-title">Buyer's List</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <%--     <div class="col-lg-2 col-md-2 col-sm-6 col-xs-12">
                                    <h5>Search: </h5>
                                </div>--%>
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                    <asp:Panel runat="server" DefaultButton="bSearchBuyer">
                                        <div class="input-group">
                                            <input runat="server" id="txtSearchBuyer" placeholder="Search..." class="form-control" type="text" autofocus autocomplete="off" /><span class="input-group-btn">
                                                <asp:LinkButton runat="server" ID="bSearchBuyer" Style="width: 50px;" CssClass="btn btn-secondary btn-default btn-facebook" OnClick="bSearchBuyer_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                            </span>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                            <br />
                            <asp:GridView runat="server"
                                ID="gvBuyers"
                                CssClass="table table-responsive"
                                AutoGenerateColumns="false"
                                GridLines="None"
                                ShowHeader="true"
                                AllowPaging="true"
                                PageSize="10"
                                UseAccessibleHeader="true"
                                OnRowCommand="gvBuyers_RowCommand"
                                OnPageIndexChanging="gvBuyers_PageIndexChanging"
                                EmptyDataText="No records found">
                                <HeaderStyle BackColor="#66ccff" />
                                <Columns>
                                    <asp:BoundField DataField="CardCode" HeaderText="Code" />
                                    <asp:BoundField DataField="LastName" HeaderText="Last Name" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                    <asp:BoundField DataField="FirstName" HeaderText="First Name" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                    <asp:BoundField DataField="MiddleName" HeaderText="Middle Name" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                    <asp:BoundField DataField="Name" HeaderText="Name" />
                                    <asp:BoundField DataField="BirthDay" HeaderText="BirthDay" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                    <asp:ButtonField ButtonType="Link" ControlStyle-CssClass="btn btn-default btn-success fa fa-arrow-right" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" CommandName="Sel" />
                                </Columns>
                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                <PagerStyle CssClass="pagination-ys" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button runat="server" id="btnClose" type="button" class="btn btn-danger" onserverclick="btnClose_ServerClick">Cancel</button>
                </div>
            </div>
        </div>
    </div>
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
                                    <asp:Button runat="server" ID="btnYes" CssClass="btn btn-info btn-sm" Text="Yes" OnClick="btnYes_Click" Style="width: 90px;" />
                                    <button type="button" class="btn btn-default btn-sm" data-dismiss="modal" style="width: 90px;" onclick="hideConfirm()">No</button>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div id="modalAlert" class="modal fade" tabindex="-1" data-focus-on="input:first">
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

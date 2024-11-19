<%@ Page Title="DREAMS | Document Status" Language="C#" MasterPageFile="~/master/Main.Master" AutoEventWireup="true" CodeBehind="DocumentStatus.aspx.cs" Inherits="ABROWN_DREAMS.pages.DocumentStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #gvStatusBody {
            overflow: auto;
        }
    </style>
    <script>
        function showBuyer() {
            $('#modalBuyers').modal({
                backdrop: 'static',
                keyboard: false
            });
        }
        function showStatus() {
            $('#modalStatus').modal('show');
        }
        function closeBuyer() {
            $('#modalBuyers').modal('hide');
        }
        function hideAlert() {
            $('#modalAlert').modal('hide');
        }
        function hideStatus() {
            $('#modalStatus').modal('hide');
        }
        function showStatusList() {
            $('#MsgStatusList').modal('show');
        }
        function hideStatusList() {
            $('#MsgStatusList').modal('hide');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="sitemap" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div class="panel panel-primary panel-trans">
        <div class="panel-heading">
            <h5>Document Status</h5>
        </div>
    </div>
    <div class="col-lg-4">
        <div class="panel panel-default">
            <div class="panel-body ">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-lg-10">
                                <div class="customer-badge" style="background-color: white;">
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
                                <asp:LinkButton runat="server" ID="LinkButton1" CssClass="btn btn-info btn-circle pull-right" OnClick="btnFind_Click"><i class="fa fa-search"></i></asp:LinkButton>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
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
                                <asp:BoundField DataField="DocEntry" HeaderText="DocEntry" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="ProjCode" HeaderText="Project" ItemStyle-VerticalAlign="Middle" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="Model" HeaderText="Model" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="Block" HeaderText="Block" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="Lot" HeaderText="Block" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="LotArea" HeaderText="Lot Area" DataFormatString="{0:#,##0.##}" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="FloorArea" HeaderText="Floor Area" DataFormatString="{0:#,##0.##}" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="Size" HeaderText="Size" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="FinCode" HeaderText="FinancingScheme" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="AcctType" HeaderText="AcctType" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="SalesType" HeaderText="SalesType" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="DPTerms" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="LTerms" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="DPDueDate" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="DPEntry" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText="0" />
                                <asp:BoundField DataField="AmountPaid" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="FinCode" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="NetTcp" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="TotalDisc" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="Stage" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="SapCardCode" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="FirstName" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="LastName" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="MiddleName" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="SAPDocEntry" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="ProductType" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="LoanType" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="Bank" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
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
            </div>
        </div>
        <div class="panel panel-default" style="background-color: white;">
            <div class="panel-body ">
                <div class="row">
                    <div class="col-lg-12 col-sm-12">
                        <div class="btn-pref btn-group btn-group-justified btn-group-lg" role="group" aria-label="...">
                            <div class="btn-group" role="group" style="display: none;">
                                <button type="button" id="profile" class="btn btn-primary" href="#tab1" data-toggle="tab">
                                    <span class="glyphicon glyphicon-file" aria-hidden="true"></span>
                                    <div class="hidden-xs">Account</div>
                                </button>
                            </div>
                            <div class="btn-group" role="group" style="display: none;">
                                <button type="button" id="documents" class="btn btn-default" href="#tab2" data-toggle="tab">
                                    <span class="glyphicon glyphicon-list" aria-hidden="true"></span>
                                    <div class="hidden-xs">Documents</div>
                                </button>
                            </div>
                            <div class="btn-group" role="group" style="display: none;">
                                <button type="button" id="project" class="btn btn-default" href="#tab3" data-toggle="tab">
                                    <span class="glyphicon glyphicon-home" aria-hidden="true"></span>
                                    <div class="hidden-xs">House/Lot</div>
                                </button>
                            </div>
                        </div>
                        <div class="well" style="background-color: white;">
                            <div class="tab-content">
                                <div class="tab-pane fade in active" id="tab1">
                                    <div class="table-responsive" style="border: 0px;">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <%--<button runat="server" id="btnEditProfile" class="btn btn-success pull-right">Edit &nbsp;<i class="fa fa-edit"></i></button>--%>
                                                <table class="table">
                                                    <caption class="pull-left">Account Details</caption>
                                                    <tr style="display: none;">
                                                        <th style="width: 50%">DocEntry:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblDocEntry" CssClass=" " Text="" />
                                                        </td>
                                                    </tr>
                                                    <tr style="display: none;">
                                                        <th style="width: 50%">SQ DocEntry:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblSQEntry" CssClass=" " Text="" />
                                                        </td>
                                                    </tr>
                                                    <tr style="display: none;">
                                                        <th style="width: 50%">Sap CardCode:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblSapCardCode" CssClass=" " Text="" />
                                                        </td>
                                                    </tr>
                                                    <tr style="display: none;">
                                                        <th style="width: 50%">Buyers Code:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblBuyerCode" CssClass=" " Text="" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 50%">First Name:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblFName" CssClass=" " Text="" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 50%">Last Name:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblLName" CssClass=" " Text="" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 50%">Middle Name:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblMName" CssClass=" " Text="" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 50%">Project:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblProj" CssClass=" " Text="" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th>Model:</th>
                                                        <td>
                                                            <asp:Label runat="server" CssClass=" " ID="lblModel" Text="" /></td>
                                                    </tr>
                                                    <tr>
                                                        <th>Block:</th>
                                                        <td>
                                                            <asp:Label runat="server" CssClass=" " ID="lblBlock" Text="" /></td>
                                                    </tr>
                                                    <tr>
                                                        <th>Lot:</th>
                                                        <td>
                                                            <asp:Label runat="server" CssClass=" " ID="lblLot" Text="" /></td>
                                                    </tr>
                                                    <tr>
                                                        <th>Net TCP (PHP):</th>
                                                        <td>
                                                            <asp:Label runat="server" CssClass="label label-success" ID="lblTCP" Text="" Font-Size="10" /></td>
                                                    </tr>
<%--                                                    <tr>
                                                        <th>Discount (PHP):</th>
                                                        <td>
                                                            <asp:Label runat="server" CssClass="label label-success" ID="lblDiscount" Text="" Font-Size="10" /></td>
                                                    </tr>
                                                    <tr>
                                                        <th>Amount Paid (PHP):</th>
                                                        <td>
                                                            <asp:Label runat="server" CssClass="label label-danger" ID="lblAmountPaid" Text="" Font-Size="10" /></td>
                                                    </tr>--%>
                                                    <tr style="display: none;">
                                                        <th>Interest (PHP):</th>
                                                        <td>
                                                            <asp:Label runat="server" CssClass="label label-danger" ID="lblInterest" Text="" Font-Size="10" /></td>
                                                    </tr>
                                                    <tr style="display: none;">
                                                        <th>Penalty (PHP):</th>
                                                        <td>
                                                            <asp:Label runat="server" CssClass="label label-danger" ID="lblPenalty" Text="" Font-Size="10" /></td>
                                                    </tr>
                                                    <tr style="display: none;">
                                                        <th>Stage:</th>
                                                        <td>
                                                            <asp:Label runat="server" CssClass="" ID="lblStage" Text="" /></td>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 50%">Fin. Scheme:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblFinScheme" CssClass="" Text="" />
                                                        </td>
                                                    </tr>
                                                    <tr style="display: none;">
                                                        <th>Fin. Scheme Code:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblFinCode" CssClass="" Text="" />
                                                        </td>
                                                    </tr>
                                                    <tr >
                                                        <th>Product Type:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblProdType" CssClass="" Text="" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th>Loan Type:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblLoanType" CssClass="" Text="" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th>Bank:</th>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblBank" CssClass="" Text="" />
                                                        </td>
                                                    </tr>
<%--                                                    <tr>
                                                        <th>Acct. Type:</th>
                                                        <td>
                                                            <asp:Label runat="server" CssClass=" " ID="lblAcctType" Text="" /></td>
                                                    </tr>
                                                    <tr>
                                                        <th>Sales Type:</th>
                                                        <td>
                                                            <asp:Label runat="server" CssClass=" " ID="lblSalesType" Text="" /></td>
                                                    </tr>--%>
                                                    <tr style="display: none;">
                                                        <th>Equity Terms (mos):</th>
                                                        <td>
                                                            <asp:Label runat="server" CssClass=" " ID="lblDPTerms" Text="" /></td>
                                                    </tr>
                                                    <tr style="display: none;">
                                                        <th>Loanable Terms (yrs):</th>
                                                        <td>
                                                            <asp:Label runat="server" CssClass=" " ID="lblLBTerms" Text="" /></td>
                                                    </tr>
                                                    <tr style="display: none;">
                                                        <th>Due Date:</th>
                                                        <td>
                                                            <asp:Label runat="server" CssClass=" " ID="lblDueDate" Text="" /></td>
                                                    </tr>
                                                </table>
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
    <div class="col-lg-8">
        <%--        <div class="row">
            <div class="col-lg-12">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="btn-pref btn-group btn-group-justified btn-group-lg" role="group" aria-label="...">
                            <div class="btn-group" role="group">
                                <button type="button" runat="server" id="bConstruction" onserverclick="bTab_ServerClick" class="btn btn-success" href="#tabConstruction" data-toggle="tab">--%>
        <%--<span class="fa fa-dollar" aria-hidden="true"></span>--%>
        <%--                                    <div class="hidden-xs">Construction</div>
                                </button>
                            </div>
                            <div class="btn-group" role="group">
                                <button type="button" runat="server" id="bAMD" onserverclick="bTab_ServerClick" class="btn btn-default" href="#tabAMD" data-toggle="tab">--%>
        <%--<span class="fa fa-calendar" aria-hidden="true"></span>--%>
        <%--                                    <div class="hidden-xs">AMD</div>
                                </button>
                            </div>
                            <div class="btn-group" role="group">
                                <button type="button" runat="server" id="bLDD" onserverclick="bTab_ServerClick" class="btn btn-default" href="#tabLDD" data-toggle="tab">--%>
        <%--<span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>--%>
        <%--                                    <div class="hidden-xs">LDD</div>
                                </button>
                            </div>
                            <div class="btn-group" role="group">
                                <button type="button" runat="server" id="bRSD" onserverclick="bTab_ServerClick" class="btn btn-default" href="#tabRSD" data-toggle="tab">--%>
        <%--<span class="fa fa-tags" aria-hidden="true"></span>--%>
        <%--                                    <div class="hidden-xs">RSD</div>
                                </button>
                            </div>
                            <div class="btn-group" role="group">
                                <button type="button" runat="server" id="bMoveIn" onserverclick="bTab_ServerClick" class="btn btn-default" href="#tabMovein" data-toggle="tab">--%>
        <%--<span class="fa fa-tags" aria-hidden="true"></span>--%>
        <%--                                    <div class="hidden-xs">Move-in</div>
                                </button>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>--%>
        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-default clearfix" style="margin-left: 10px;">
                    <div class="panel-body">
                        <%--                        <div class="row">
                            <div class="col-lg-12">
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <asp:Button runat="server" ID="btnShowStatus" CssClass="btn btn-primary" Text="Add Status" OnClick="btnShowStatus_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>--%>
                        <div class="row">
                            <div class="col-lg-12" id="gvStatusBody">
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <%--<asp:GridView runat="server"
                                            ID="gvStatus"
                                            CssClass="table table-responsive"
                                            AutoGenerateColumns="false"
                                            GridLines="None"
                                            ShowHeader="true"
                                            SelectedRowStyle-BackColor="#A1DCF2"
                                            UseAccessibleHeader="true"
                                            OnRowCommand="gvStatus_RowCommand"
                                            EmptyDataText="No records found">
                                            <HeaderStyle BackColor="#d7dce5" />
                                            <Columns>66ccff
                                                <asp:BoundField DataField="Id" HeaderText="DocEntry" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                                <asp:BoundField DataField="CreateDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                                <asp:BoundField DataField="StatusId" HeaderText="StatusId" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                                <asp:BoundField DataField="Status" HeaderText="Status" />
                                                <asp:BoundField DataField="SubStatusId" HeaderText="SubStatusId" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                                <asp:BoundField DataField="SubStatus" HeaderText="Sub Status" />
                                                <asp:BoundField DataField="Remarks" HeaderText="Remarks" NullDisplayText=" " />
                                                <asp:ButtonField ControlStyle-CssClass="glyphicon glyphicon-edit" ItemStyle-Width="10" CommandName="Edt" ItemStyle-HorizontalAlign="Center" />
                                                <asp:ButtonField ControlStyle-CssClass="fa fa-times-circle" ItemStyle-Width="10" CommandName="Del" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="#e2584d" />
                                            </Columns>
                                            <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                            <PagerStyle CssClass="pagination-ys" />
                                        </asp:GridView>--%>

                                        <asp:GridView ID="gvStatus" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                            CssClass="table table-hover table-responsive" Width="100%" ShowHeader="True" PageSize="5"
                                            GridLines="None" OnPageIndexChanging="gvStatus_PageIndexChanging"
                                            OnRowCommand="gvStatus_RowCommand">
                                            <HeaderStyle BackColor="#66ccff" />
                                            <Columns>
                                                <asp:BoundField DataField="Code" HeaderText="Code" SortExpression="Code" ItemStyle-Font-Size="Medium" HeaderStyle-Width="30%" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"/>
                                                <asp:BoundField DataField="inputdate" HeaderText="Input Date" SortExpression="InputDate" ItemStyle-Font-Size="Medium" HeaderStyle-Width="30%" />
                                                <asp:BoundField DataField="U_Type" HeaderText="Loan Type" SortExpression="Scheme" ItemStyle-Font-Size="Medium" HeaderStyle-Width="30%" />
                                                <asp:BoundField DataField="U_Document" HeaderText="Document Name" SortExpression="Document Name" ItemStyle-Font-Size="Medium" />
                                                <asp:TemplateField HeaderStyle-Width="5px" HeaderStyle-HorizontalAlign="Center" HeaderText="Date Required">
                                                    <ItemTemplate>
                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <div class="row">
                                                                    <div class="col-lg-12">
                                                                        <div class="form-group">
                                                                            <asp:TextBox runat="server" ID="txtDate1" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="5px" HeaderStyle-HorizontalAlign="Center" HeaderText="Issue Date">
                                                    <ItemTemplate>
                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <div class="row">
                                                                    <div class="col-lg-12">
                                                                        <div class="form-group">
                                                                            <asp:TextBox runat="server" ID="txtDate2" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="5px" HeaderStyle-HorizontalAlign="Center" HeaderText="Document Date">
                                                    <ItemTemplate>
                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <div class="row">
                                                                    <div class="col-lg-12">
                                                                        <div class="form-group">
                                                                            <asp:TextBox runat="server" ID="txtDate3" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="5px" HeaderStyle-HorizontalAlign="Center" HeaderText="Received Date">
                                                    <ItemTemplate>
                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <div class="row">
                                                                    <div class="col-lg-12">
                                                                        <div class="form-group">
                                                                            <asp:TextBox runat="server" ID="txtDate4" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="5px" HeaderStyle-HorizontalAlign="Center" HeaderText="Expiry Date">
                                                    <ItemTemplate>
                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <div class="row">
                                                                    <div class="col-lg-12">
                                                                        <div class="form-group">
                                                                            <asp:TextBox runat="server" ID="txtDate5" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="5px" HeaderStyle-HorizontalAlign="Center" HeaderText="Status">
                                                    <ItemTemplate>
                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <div class="row">
                                                                    <div class="col-lg-12">
                                                                        <%--   <input type="text" style="z-index: auto; width: 150px;" class="form-control tProjName" runat="server" ID="tStatusName" disabled />                                                                     <div class="form-group">
                                                                            <asp:TextBox ID="txtStandardStatus" class="form-control" runat="server" />
                                                                        </div>--%>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="tStatusName" Style="z-index: auto; width: 150px;" class="form-control tProjName" runat="server" disabled="True" /><span class="input-group-btn">
                                                                                <%--<button id="bStatusName" runat="server" style="width: 50px; z-index: auto;" class="btn btn-secondary" type="button" onserverclick="bStatusName_ServerClick" CommandArgument='<%# Bind("Code")%>'><i class="fa fa-bars"></i></button>--%>
                                                                                <asp:LinkButton runat="server" ID="bStatusName" CssClass="btn btn-default btn-secondary" Width="100%" Height="100%" OnClick="bStatusName_ServerClick" CommandArgument='<%# Bind("Code")%>'><i class="fa fa-bars"></i></asp:LinkButton>
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="5px" HeaderStyle-HorizontalAlign="Center" HeaderText="Reference No">
                                                    <ItemTemplate>
                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <div class="row">
                                                                    <div class="col-lg-12">
                                                                        <div class="form-group">
                                                                            <asp:TextBox ID="txtStandardReferenceNo" class="form-control" Style="width: 150px;" runat="server" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="5px" HeaderStyle-HorizontalAlign="Center" HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <div class="row">
                                                                    <div class="col-lg-12">
                                                                        <div class="form-group">
                                                                            <asp:TextBox ID="txtStandardAttachmentComments" TextMode="MultiLine" Rows="3" class="form-control" Style="width: 200px;" runat="server" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="5px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" HeaderText="Attachment">
                                                    <ItemTemplate>
                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <div class="form-group  center-block text-center ">
                                                                    <asp:Label runat="server" ID="lblFileName" Text=""></asp:Label>
                                                                    <asp:FileUpload ID="FileUpload1" CssClass="btn btn-default" CommandName="FileUpload1" runat="server" EnableViewState="true" />
                                                                    <asp:LinkButton ID="btnUpload" CssClass="btn btn-primary" runat="server" CommandName="Upload" Text="Upload" />
                                                                    <div class="col-lg-12">
                                                                        <asp:LinkButton ID="btnPreview" CssClass="btn btn-info" Visible="false" runat="server" CommandName="Preview" Text="Preview" />
                                                                    </div>
                                                                    <asp:LinkButton ID="btnRemove" CssClass="btn btn-danger" Visible="false" runat="server" CommandName="Remove" Text="Remove" />
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
                                                <asp:TemplateField HeaderStyle-Width="5px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <div class="form-group  center-block text-center ">
                                                                    <asp:LinkButton class="btn btn-info" runat="server" ID="btnAddDocStatus" Text="Add Status" onclick="btnAddDocStatus_Click" CommandName="Uploading" CommandArgument='<%# Bind("Code")%>'/>
                                                                    <asp:LinkButton class="btn btn-info" runat="server" Visible="false" ID="btnUpdateDocStatus" Text="Update Status" onclick="btnUpdateDocStatus_Click" CommandName="Updating" CommandArgument='<%# Bind("Code")%>'/>
                                                                </div>
                                                            </ContentTemplate>
                                                            <%--<Triggers>
                                                                <asp:PostBackTrigger ControlID="btnAddDocStatus" />
                                                            </Triggers>--%>
                                                        </asp:UpdatePanel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <%--<asp:UpdatePanel runat="server">
                                            </as--%>
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
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="modals" runat="server">
    <div id="modalBuyers" class="modal fade" role="dialog">
        <div class="modal-dialog" style="width: 70%;">
            <div class="modal-content">
                <div class="modal-header modal-header-primary">
                    <h4 class="modal-title">Buyer's List</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                    <asp:Panel runat="server" DefaultButton="bSearchBuyer">
                                        <div class="input-group">
                                            <input runat="server" id="txtSearchBuyer" placeholder="Search..." class="form-control" type="text" autofocus /><span class="input-group-btn">
                                                <asp:LinkButton runat="server" ID="bSearchBuyer" Style="width: 50px;" CssClass="btn btn-secondary btn-default btn-facebook" OnClick="bSearchBuyer_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                            </span>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                            <br />
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
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
                                            <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                                            <asp:BoundField DataField="FirstName" HeaderText="First Name" />
                                            <asp:BoundField DataField="MiddleName" HeaderText="Middle Name" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                            <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                            <%--<asp:BoundField DataField="DocEntry" HeaderText="DocEntry" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />--%>
                                            <asp:BoundField DataField="BirthDay" HeaderText="BirthDay" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                            <asp:ButtonField ButtonType="Link" ControlStyle-CssClass="btn btn-default btn-success fa fa-arrow-right" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" CommandName="Sel" />
                                        </Columns>
                                        <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                        <PagerStyle CssClass="pagination-ys" />
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button runat="server" id="btnClose" type="button" class="btn btn-danger" onserverclick="btnClose_ServerClick">Cancel</button>
                </div>
            </div>
        </div>
    </div>
    <div id="modalStatus" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Document Status</h4>
                </div>
                <div class="modal-body">
                    <asp:Panel runat="server" DefaultButton="btnAddStatus">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <%--<input type="text" style="z-index: auto;" class="form-control hide" runat="server" id="txtDiaryId" title="0" disabled/>--%>
                                <asp:TextBox runat="server" ID="txtDiaryId" CssClass="hide" Text="0" Enabled="false"></asp:TextBox>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Date:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <asp:TextBox runat="server" ID="txtDate" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Type:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <%--                                        <asp:DropDownList runat="server" ID="ddType" CssClass="form-control"
                                            DataTextField="Type" DataValueField="Type" AutoPostBack="true">
                                        </asp:DropDownList>--%>
                                        <div class="input-group">
                                            <input type="text" style="z-index: auto;" class="form-control tType" runat="server" id="tType" disabled /><span class="input-group-btn">
                                                <button id="bTypeList" runat="server" style="width: 50px; z-index: auto;" class="btn btn-secondary" type="button"><i class="fa fa-bars"></i></button>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Scheme:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <%--<asp:TextBox runat="server" ID="txtScheme" CssClass="form-control" Width="100%"></asp:TextBox>--%>
                                        <div class="input-group">
                                            <input type="text" style="z-index: auto;" class="form-control tScheme" runat="server" id="tScheme" disabled /><span class="input-group-btn">
                                                <button id="bSchemeList" runat="server" style="width: 50px; z-index: auto;" class="btn btn-secondary" type="button"><i class="fa fa-bars"></i></button>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Document:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <%--<asp:DropDownList runat="server" ID="ddName" CssClass="form-control"
                                            DataTextField="U_Document" DataValueField="U_Document" AutoPostBack="true">
                                        </asp:DropDownList>--%>
                                        <div class="input-group">
                                            <input type="text" style="z-index: auto;" class="form-control tDocument" runat="server" id="tDocument" disabled /><span class="input-group-btn">
                                                <button id="bDocumentList" runat="server" style="width: 50px; z-index: auto;" class="btn btn-secondary" type="button"><i class="fa fa-bars"></i></button>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Status:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <asp:DropDownList runat="server" ID="ddStatus" CssClass="form-control"
                                            DataTextField="Name" DataValueField="Id" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddStatus_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Sub Status:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <asp:DropDownList runat="server" ID="ddSubStatus" CssClass="form-control"
                                            DataTextField="Name" DataValueField="Id">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <input type="text" style="z-index: auto; display: none;" class="form-control" runat="server" id="txtCreditAccountCode" disabled />
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Remarks:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <asp:TextBox runat="server" ID="txtStatus" TextMode="MultiLine" CssClass="form-control" Width="100%" Height="300"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-8 col-lg-offset-4">
                                        <asp:Button runat="server" ID="btnAddStatus" CssClass="btn btn-info" Text="Add Status" Width="100%" OnClick="btnAddStatus_Click" />
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
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
                                        <asp:Label runat="server" ID="lblMessageAlert" CssClass="lblMessageAlert"></asp:Label></h4>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="row" style="text-align: center;">
                        <div class="col-lg-12">
                            <button type="button" class="btn btn-primary" style="width: 90px;" onclick="hideAlert()">OK</button>
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
    <div id="MsgStatusList" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Status List</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="gvStatusList" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                        CssClass="table table-bordered table-hover " Width="100%" ShowHeader="True" PageSize="5">
                                        <HeaderStyle BackColor="#66ccff" />
                                        <Columns>
                                            <asp:BoundField DataField="Statuses" HeaderText="" />
                                            <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="bSelectStatus" CssClass="btn btn-default btn-success" Width="100%" Height="100%" OnClick="bSelectStatus_Click" CommandArgument='<%# Bind("Statuses")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="bSelectProject" CssClass="btn btn-default btn-success" Width="100%" Height="100%" OnClick="bSelectProject_Click" CommandArgument='<%# Bind("PrjCode")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
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
</asp:Content>

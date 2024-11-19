<%@ Page Title="DREAMS | Incentives" Language="C#" MasterPageFile="~/master/Main.Master" AutoEventWireup="true" CodeBehind="Incentives.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="ABROWN_DREAMS.pages.Incentives" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        //ALERTS
        function ShowAlert() {
            $('#modalAlert').modal('show');
        }
        function HideAlert() {
            $('#modalAlert').modal('hide');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="sitemap" runat="server">
    <li><span>AMD</span></li>
    <li><span>Incentives</span></li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">

    <div class="panel panel-primary panel-trans">
        <div class="panel-heading">
            <h5>List of Incentives</h5>
        </div>
    </div>

    <div class="col-lg-12">
        <div class="panel panel-default clearfix" style="margin-left: 10px;">
            <div class="panel-body">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>





                        <div class="col-lg-12">

                            <%--2023-11-16 : ADD NEW FIELD: UPLOAD DATE --%>
                            <div class="row">
                                <div class="col-lg-1 col-md-1 col-sm-3 col-xs-6">
                                    <h5>Upload Date: </h5>
                                </div>

                                <div class="col-lg-5 col-md-5 col-sm-3 col-xs-6">
                                    <div class="input-group" style="width: 250px;">
                                        <asp:TextBox TextMode="Date" runat="server" CssClass="form-control" ID="txtDocDate"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <asp:Panel runat="server" DefaultButton="btnDocSearch">
                                    <div class="col-lg-1 col-md-1 col-sm-3 col-xs-6">
                                        <h5>Search: </h5>
                                    </div>
                                    <div class="col-lg-5 col-md-5 col-sm-3 col-xs-6">
                                        <div class="input-group">
                                            <input runat="server" id="txDocSearch" placeholder="Search here" class="form-control" type="text" autofocus /><span class="input-group-btn">
                                                <asp:LinkButton runat="server" ID="btnDocSearch" Style="width: 50px;" CssClass="btn btn-secondary btn-default btn-facebook" OnClick="btnDocSearch_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                            </span>
                                        </div>
                                    </div>
                                    <div style="float: right;">
                                        <asp:Button ID="btnUploadIncent" runat="server" CssClass="btn btn-info btn-sm" Text="Upload" OnClick="btnUploadIncent_Click" />
                                    </div>
                                </asp:Panel>
                            </div>


                            <div class="row">
                                <div class="col-lg-1 col-md-1 col-sm-3 col-xs-6">
                                    <h5>Release Type: </h5>
                                </div>
                                <div class="col-lg-5 col-md-5 col-sm-3 col-xs-6">
                                    <asp:DropDownList runat="server" ID="ddlReplenishment" CssClass="form-control">
                                        <asp:ListItem Selected="True" Text="Check"></asp:ListItem>
                                        <asp:ListItem Text="Cash"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>


                            <div class="row" style="height: 800px; overflow: auto;">
                                <div class="col-lg-12">
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:GridView runat="server"
                                                ID="gvDocList"
                                                CssClass="table table-responsive"
                                                AutoGenerateColumns="false"
                                                GridLines="None"
                                                ShowHeader="true"
                                                SelectedRowStyle-BackColor="#A1DCF2"
                                                UseAccessibleHeader="true"
                                                EmptyDataText="No records found">
                                                <HeaderStyle BackColor="#d7dce5" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cbRow" runat="server" AutoPostBack="true" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="DocEntry" HeaderText="DocEntry" ItemStyle-CssClass="" HeaderStyle-CssClass="" />
                                                    <asp:BoundField DataField="DocNum" HeaderText="Doc No." ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                                                    <asp:BoundField DataField="AcctType" HeaderText="Product Type" ItemStyle-CssClass="" />
                                                    <asp:BoundField DataField="Position" HeaderText="Position" ItemStyle-CssClass="" HeaderStyle-CssClass="" />
                                                    <asp:BoundField DataField="SAPCardCode" HeaderText="SAP BP" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                                                    <asp:BoundField DataField="SalesPerson" HeaderText="Sales Person" ItemStyle-CssClass="" />
                                                    <asp:BoundField DataField="Release" HeaderText="Release" ItemStyle-CssClass="" />
                                                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:#,##0.00}" />
                                                    <asp:BoundField DataField="CardCode" HeaderText="Buyer Code" ItemStyle-CssClass="" />
                                                    <asp:BoundField DataField="CardName" HeaderText="Buyer Name" ItemStyle-CssClass="" />
                                                    <asp:BoundField DataField="ProjCode" HeaderText="Project" ItemStyle-CssClass="" />
                                                    <asp:BoundField DataField="Block" HeaderText="Block" ItemStyle-CssClass="" />
                                                    <asp:BoundField DataField="Lot" HeaderText="Lot" ItemStyle-CssClass="" />
                                                    <asp:BoundField DataField="TransType" HeaderText="TransType" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="hide" />
                                                </Columns>
                                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                                <PagerStyle CssClass="pagination-ys" />
                                            </asp:GridView>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>

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
                                        <asp:Label runat="server" ID="lblMessageAlert"></asp:Label>
                                    </h4>
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

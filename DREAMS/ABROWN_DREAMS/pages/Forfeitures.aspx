<%@ Page Title="" Language="C#" MasterPageFile="~/master/Main.Master" AutoEventWireup="true" CodeBehind="Forfeitures.aspx.cs" Inherits="ABROWN_DREAMS.pages.Forfeitures" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script>
        //with SetTimeout
        function showAlertSuccess() {
            $('#modalAlertSuccess').modal('show');
            setTimeout(function hideAlertSuccess() {
                $('#modalAlertSuccess').modal('hide');
            }, 2500);
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="sitemap" runat="server">
    <li><span>Forfeitures</span></li>
    <li><span>Forfeitures</span></li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div class="panel panel-primary panel-trans">
        <div class="panel-heading">
            <h5>List of Forfeited Accounts</h5>
        </div>
    </div>

    <div class="col-lg-12">
        <div class="panel panel-default clearfix" style="margin-left: 10px;">
            <div class="panel-body">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>


                        <%-- 2023-06-28 : ADD SEARCHING FOR LIST OF FORFEITURES --%>
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Panel runat="server" DefaultButton="bSearchBuyer">
                                <div class="input-group">
                                    <input runat="server" id="txtSearchBuyer" placeholder="Search..." class="form-control" type="text" autofocus /><span class="input-group-btn">
                                        <asp:LinkButton runat="server" ID="bSearchBuyer" OnClick="bSearchBuyer_Click" Style="width: 50px;" CssClass="btn btn-secondary btn-default btn-facebook">
                                            <i class="fa fa-search"></i>
                                        </asp:LinkButton>
                                    </span>
                                </div>
                            </asp:Panel>
                        </div>

                        <div class="col-lg-12">
                            <div class="row" style="height: 800px; overflow: auto;">
                                <div class="col-lg-12">
                                    <asp:Label runat="server" ID="pymntTitle" CssClass="" Text="Equity / Down Payments" Visible="false"></asp:Label>
                                    <asp:GridView runat="server"
                                        ID="gvDocList"
                                        CssClass="table table-responsive"
                                        AutoGenerateColumns="false"
                                        GridLines="None"
                                        ShowHeader="true"
                                        SelectedRowStyle-BackColor="#A1DCF2"
                                        UseAccessibleHeader="true"
                                        EmptyDataText="No records found"
                                        AllowPaging="true"
                                        PageSize="20"
                                        OnPageIndexChanging="gvDocList_PageIndexChanging">
                                        <HeaderStyle BackColor="#d7dce5" />
                                        <Columns>
                                            <asp:BoundField DataField="DocEntry" HeaderText="DocEntry" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                            <asp:BoundField DataField="DocNum" HeaderText="Doc No." ItemStyle-CssClass="" />
                                            <asp:BoundField DataField="CardCode" HeaderText="CardCode" ItemStyle-CssClass="" HeaderStyle-CssClass="" />
                                            <asp:BoundField DataField="CardName" HeaderText="CardName" ItemStyle-CssClass="" HeaderStyle-CssClass="" />
                                            <asp:BoundField DataField="LoanType" HeaderText="LoanType" ItemStyle-CssClass="" />
                                            <asp:BoundField DataField="DocDate" HeaderText="Quotation Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                            <asp:BoundField DataField="DateForfeit" HeaderText="Date Forfeited" DataFormatString="{0:dd-MMM-yyyy}" />
                                            <%--<asp:BoundField DataField="DocStatus" HeaderText="DocStatus" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />--%>

                                            <%-- 2023-06-28 : CHANGED FROM OTCP TO Das --%>
                                            <%--<asp:BoundField DataField="OTcp" HeaderText="TCP Amount" DataFormatString="{0:#,##0.00}" />--%>
                                            <asp:BoundField DataField="Das" HeaderText="TCP Amount" DataFormatString="{0:#,##0.00}" />

                                            <asp:BoundField DataField="TotalPayment" HeaderText="TCP Paid" DataFormatString="{0:#,##0.00}" />
                                            <asp:BoundField DataField="DocStatus" HeaderText="DocStatus" />
                                            <asp:BoundField DataField="Phase" HeaderText="Phase" ItemStyle-CssClass="" />
                                            <asp:BoundField DataField="ProjCode" HeaderText="Project" ItemStyle-CssClass="" />
                                            <asp:BoundField DataField="Block" HeaderText="Block" ItemStyle-CssClass="" />
                                            <asp:BoundField DataField="Lot" HeaderText="Lot" ItemStyle-CssClass="" />
                                            <asp:BoundField DataField="Model" HeaderText="Model" ItemStyle-CssClass="" />

                                        </Columns>
                                        <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                        <PagerStyle CssClass="pagination-ys" />
                                    </asp:GridView>

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

    <%--    Hide/Show automatically --%>
    <div id="modalAlertSuccess" class="modal fade" tabindex="-1" data-focus-on="input:first">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="row" style="text-align: center;">
                                <div class="col-lg-12">
                                    <asp:Image ImageUrl="~/assets/img/success.png" runat="server" Width="90" Height="90" />
                                </div>
                            </div>
                            <div class="row" style="text-align: center;">
                                <div class="col-lg-12">
                                    <h4>
                                        <asp:Label runat="server" ID="txtAlert"></asp:Label>
                                    </h4>
                                </div>
                            </div>
                            <br />
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
                        <contenttemplate>
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
                        </contenttemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

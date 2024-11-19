<%@ Page Title="" Language="C#" MasterPageFile="~/master/Main.Master" AutoEventWireup="true" CodeBehind="HouseCon.aspx.cs" Inherits="ABROWN_DREAMS.HouseCon" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="sitemap" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div class="col-lg-12">
        <div class="panel panel-default">
            <div class="panel-heading">
                For House Construction
            </div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-lg-12">
                        <button runat="server" id="btnExport" class="btn btn-success btn-sm" style="float: right;" onserverclick="btnExport_ServerClick">Export to Excel</button>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-12">
                        <asp:GridView runat="server" ID="gvForConstruction"
                            CssClass="table table-hover table-responsive"
                            AutoGenerateColumns="false"
                            GridLines="None"
                            ShowHeader="true"
                            OnRowCommand="gvForConstruction_RowCommand">
                            <HeaderStyle BackColor="#66ccff" />
                            <Columns>
                                <asp:BoundField DataField="DocEntry" HeaderText="DocEntry" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                <asp:BoundField DataField="Name" HeaderText="Buyer's Name" />
                                <asp:BoundField DataField="ProjCode" HeaderText="Project" />
                                <asp:BoundField DataField="Phase" HeaderText="Phase" />
                                 <asp:BoundField DataField="Model" HeaderText="Model" />
                                <asp:BoundField DataField="Block" HeaderText="Block" />
                                <asp:BoundField DataField="Lot" HeaderText="Lot" />
                                <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn btn-default btn-info btn-sm" Text="Update Status" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" CommandName="Sel" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="modals" runat="server">
</asp:Content>

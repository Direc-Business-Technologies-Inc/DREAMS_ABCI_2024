<%@ Page Title="DREAMS | Reports" Language="C#" MasterPageFile="~/master/Main.Master" AutoEventWireup="true" CodeBehind="ReportList.aspx.cs" Inherits="ABROWN_DREAMS.pages.ReportList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    <li><span>Reports</span></li>
    <li><span>Report List</span></li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">

    <div class="row">

        <div class="col-lg-1"></div>
        <div class="col-lg-10">
            <h3>List of Reports </h3>
            <asp:GridView runat="server"
                ID="gvReports"
                AutoGenerateColumns="false"
                CssClass="table table-hover table-responsive"
                OnRowCommand="gvReports_RowCommand"
                OnRowDataBound="gvReports_RowDataBound"
                OnSelectedIndexChanged="gvReports_SelectedIndexChanged"
                CellPadding="2"
                GridLines="None"
                ShowHeader="false">
                <HeaderStyle BackColor="#66ccff" />
                <Columns>
                    <%--                <asp:BoundField DataField="Id" HeaderText="LineNum" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                <asp:BoundField DataField="Name" ItemStyle-Width="100%" />
                <asp:BoundField DataField="RptName" ItemStyle-Width="100%" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                <asp:BoundField DataField="RptPath" ItemStyle-Width="100%" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                <asp:BoundField DataField="RptGroup" ItemStyle-Width="100%" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                <asp:BoundField DataField="ObjCode" ItemStyle-Width="100%" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText="0" />--%>
                    <%--<asp:ButtonField ControlStyle-CssClass="fa fa-print" ItemStyle-Width="10" CommandName="Print" ItemStyle-HorizontalAlign="Center" />--%>
                    <%--<asp:TemplateField>
                    <ItemTemplate>
                        <%--<asp:Image runat="server" ID="isPrinted" ImageUrl="~/assets/img/cancel.png" Width="20" />--%>
                    <%--           <asp:LinkButton CssClass="btn btn-primary btn-sm" ID="btnPrint" Text="Print" runat="server" OnClick="btnPrint_Click" OnClientClick="SetTarget();" CommandArgument='<%# Container.DataItemIndex %>'>
                                    Print <i class="fa fa-print"></i>
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                    <asp:BoundField DataField="Text" HeaderText="File Name" />
                    <asp:TemplateField ControlStyle-Width="100">
                        <ItemTemplate>
                            <asp:LinkButton CssClass="btn btn-primary btn-sm" ID="btnPrint" Text="Print" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                CommandName="report">
                            <i class="fa fa-print"></i> 
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                <PagerStyle CssClass="pagination-ys" />
            </asp:GridView>
        </div>
        <div class="col-lg-1"></div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="modals" runat="server">
</asp:Content>

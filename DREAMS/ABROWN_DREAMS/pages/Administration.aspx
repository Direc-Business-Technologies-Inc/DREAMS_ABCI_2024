<%@ Page Title="DREAMS | Administration" Language="C#" MasterPageFile="~/master/Main.Master" AutoEventWireup="true" CodeBehind="Administration.aspx.cs" Inherits="ABROWN_DREAMS.Administration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--MODALS--%>
    <%--<link rel="stylesheet" href="../assets/css/VerticalTabs.css">--%>
    <script src="../assets/js/VerticalTabs.js"></script>
    <script type="text/javascript">
        function showValidValues() {
            $('#modalValidValues').modal('show');
        }
        function closeValidValues() {
            $('#modalValidValues').modal('hide');
        }
        function closeTerms() {
            $('#modalTerms').modal('hide');
        }
        function closeDoc() {
            $('#modalDocuments').modal('hide');
        }
        function closeBrokerDoc() {
            $('#modalBrokerDocuments').modal('hide');
        }
        function showRole() {
            $('#modalRole').modal('show');
        }
        function closeRole() {
            $('#modalRole').modal('hide');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="sitemap" runat="server">
    <li><a href="#"><span>General Setup</span></a></li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-sm-3 col-md-3">
                <div class="panel-group" id="accordion">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseOne"><span class="fa fa-gears"></span>Configuration</a>
                            </h4>
                        </div>
                        <div id="collapseOne" class="panel-collapse collapse in">
                            <div class="panel-body">
                                <table class="table">
                                    <tr>
                                        <td hidden="hidden">
                                            <span class="glyphicon glyphicon-bookmark text-primary"></span><a data-toggle="tab" href="#lA">&nbsp;Status</a>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <span class="glyphicon glyphicon-list text-danger"></span><a data-toggle="tab" href="#lC">&nbsp;Valid Values</a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td hidden="hidden">
                                            <span class="fa fa-bank text-success"></span><a data-toggle="tab" href="#lB">&nbsp;Financing Scheme</a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span class="glyphicon glyphicon-hand-up text-primary"></span><a data-toggle="tab" href="#lF">&nbsp;Broker Renewal Date</a>
                                        </td>
                                    </tr>
                                    <tr hidden="hidden">
                                        <td>
                                            <span class="glyphicon glyphicon-book text-primary"></span><a data-toggle="tab" href="#lD">&nbsp;Buyer Document Requirements</a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span class="glyphicon glyphicon-folder-close text-primary"></span><a data-toggle="tab" href="#lD2">&nbsp;Broker Document Requirements</a>
                                        </td>
                                    </tr>
                                    <tr hidden="hidden">
                                        <td>
                                            <span class="glyphicon glyphicon-user text-primary"></span><a data-toggle="tab" href="#lE">&nbsp;User Roles</a>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm-9 col-md-9">
                <div class="well">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="tab-content">
                                <div class="tab-pane active" id="lA">
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <div class="panel panel-default">
                                                <div class="panel-body">
                                                    <div class="row">
                                                        <div class="col-lg-12">
                                                            <asp:GridView runat="server" ID="gvDocumentStatus"
                                                                AllowPaging="true"
                                                                AllowSorting="true"
                                                                CssClass="table table-hover table-responsive"
                                                                AutoGenerateColumns="false"
                                                                GridLines="None"
                                                                PageSize="5"
                                                                OnPageIndexChanging="gvDocumentStatus_PageIndexChanging">
                                                                <HeaderStyle BackColor="#333333" ForeColor="White" Font-Bold="true" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="Code" HeaderText="Code" />
                                                                    <asp:BoundField DataField="Name" HeaderText="Name" />
                                                                    <asp:BoundField DataField="Color" HeaderText="Color" Visible="false" />
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderText="Color" ItemStyle-Width="50">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox runat="server" ID="txtColor" TextMode="Color" CssClass="form-control" Width="50" Text='<%# Bind("Color") %>'></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <PagerStyle CssClass="pagination-ys" />
                                                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <div class="trans-command">
                                                        <div class="btn-group" role="group">
                                                            <asp:UpdatePanel runat="server">
                                                                <ContentTemplate>
                                                                    <asp:LinkButton runat="server" CssClass="btn btn-success btn-width" ID="btnSave" OnClick="btnSave_Click">
                                                                        <label runat="server" id="lblSave" style="height: 5px; font-weight: normal;" />
                                                                        Save
                                            <i class="fa fa-save"></i>
                                                                    </asp:LinkButton>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="tab-pane" id="lB">
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <div class="col-lg-12">
                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                        <div class="panel panel-default">
                                                            <div class="panel-body">
                                                                <asp:GridView ID="gvFinancingScheme" runat="server"
                                                                    AutoGenerateColumns="false"
                                                                    EmptyDataText="No Records Found"
                                                                    CssClass="table table-hover table-responsive"
                                                                    Width="100%"
                                                                    ShowHeader="True"
                                                                    GridLines="None"
                                                                    SelectedRowStyle-BackColor="#A1DCF2">
                                                                    <HeaderStyle BackColor="#333333" ForeColor="White" Font-Bold="true" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="Name" HeaderText="Financing Scheme" ItemStyle-Width="100%" />
                                                                        <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton runat="server" ID="bSelectBuyer" CssClass="btn btn-default btn-success" Width="100%" Height="100%" OnClick="bSelectBuyer_Click" CommandArgument='<%# Bind("Code")%>'><i class="fa fa-arrow-right"></i></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <PagerStyle CssClass="pagination-ys" />
                                                                    <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-lg-12">
                                                <div class="panel panel-primary" runat="server" id="pnlTerms">
                                                    <div class="panel-heading">
                                                        <asp:Label runat="server" ID="lblTermsTitle" Text=""></asp:Label>
                                                    </div>
                                                    <div class="panel-body">
                                                        <asp:UpdatePanel runat="server">
                                                            <ContentTemplate>
                                                                <div class="row">
                                                                    <div class="col-lg-12">
                                                                        <asp:GridView runat="server" ID="gvTerms"
                                                                            AllowPaging="false"
                                                                            AllowSorting="false"
                                                                            CssClass="table table-hover table-responsive"
                                                                            AutoGenerateColumns="false"
                                                                            EmptyDataText="No Records Found"
                                                                            OnRowCommand="gvTerms_RowCommand"
                                                                            GridLines="None">
                                                                            <HeaderStyle BackColor="#333333" ForeColor="White" Font-Bold="true" />
                                                                            <Columns>
                                                                                <asp:BoundField DataField="Name" HeaderText="Terms" />
                                                                                <asp:ButtonField ControlStyle-CssClass="fa fa-times-circle" ItemStyle-Width="10" CommandName="Del" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="#e2584d" />
                                                                                <%--<asp:ButtonField runat="server" ControlStyle-CssClass="btn btn-danger btn-sm" Text="Delete" CommandName="Del" ItemStyle-HorizontalAlign="Right" />--%>
                                                                            </Columns>
                                                                            <PagerStyle CssClass="pagination-ys" />
                                                                            <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                                                        </asp:GridView>
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                        <div class="trans-command">
                                                                            <div class="btn-group" role="group">
                                                                                <button class="btn btn-success btn-width" data-toggle="modal" data-target="#modalTerms">
                                                                                    <label runat="server" id="Label1" style="height: 5px; font-weight: normal;" />
                                                                                    Add
                                                        <i class="fa fa-plus"></i>
                                                                                </button>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="tab-pane" id="lC">
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <div class="panel panel-default">
                                                <div class="panel-body">
                                                    <div class="row">
                                                        <div class="col-lg-12">
                                                            <asp:GridView runat="server" ID="gvValidValues"
                                                                AllowPaging="true"
                                                                AllowSorting="true"
                                                                CssClass="table table-hover table-responsive"
                                                                AutoGenerateColumns="false"
                                                                GridLines="None"
                                                                OnRowCommand="gvValidValues_RowCommand"
                                                                OnPageIndexChanging="gvValidValues_PageIndexChanging">
                                                                <HeaderStyle BackColor="#333333" ForeColor="White" Font-Bold="true" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="Code" HeaderText="Code" />
                                                                    <asp:BoundField DataField="Name" HeaderText="Name" />
                                                                    <asp:BoundField DataField="GrpCode" HeaderText="Group" />
                                                                    <asp:ButtonField ControlStyle-CssClass="glyphicon glyphicon-edit" ItemStyle-Width="10" CommandName="Edt" ItemStyle-HorizontalAlign="Center" />
                                                                    <%--<asp:ButtonField runat="server" ControlStyle-CssClass="btn btn-info btn-sm" Text="Edit" CommandName="Edt" ItemStyle-HorizontalAlign="Right" />--%>
                                                                </Columns>
                                                                <PagerStyle CssClass="pagination-ys" />
                                                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="tab-pane" id="lD">
                                    <div class="col-lg-12">

                                        <div class="row">
                                            <div class="panel panel-default">
                                                <div class="panel-body">
                                                    <div class="row">
                                                        <div class="col-lg-12">
                                                            <asp:UpdatePanel runat="server">
                                                                <ContentTemplate>
                                                                    <asp:GridView runat="server" ID="gvDocuments"
                                                                        AllowPaging="false"
                                                                        AllowSorting="false"
                                                                        CssClass="table table-hover table-responsive"
                                                                        AutoGenerateColumns="false"
                                                                        OnRowCommand="gvDocuments_RowCommand"
                                                                        GridLines="None">
                                                                        <HeaderStyle BackColor="#333333" ForeColor="White" Font-Bold="true" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="DocId" HeaderText="Id" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                                                                            <asp:BoundField DataField="Document" HeaderText="Document Title" />
                                                                            <asp:BoundField DataField="Code" HeaderText="Business Type" />
                                                                            <asp:ButtonField ControlStyle-CssClass="fa fa-times-circle" ItemStyle-Width="10" CommandName="Del" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="#e2584d" />
                                                                            <%--<asp:ButtonField runat="server" ControlStyle-CssClass="btn btn-danger btn-sm" Text="Delete" CommandName="Del" ItemStyle-HorizontalAlign="Right" />--%>
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
                                        <div class="row">
                                            <div class="trans-command">
                                                <div class="btn-group" role="group">
                                                    <asp:UpdatePanel runat="server">
                                                        <ContentTemplate>
                                                            <button class="btn btn-success" data-toggle="modal" data-target="#modalDocuments">
                                                                Add
                                                    <i class="fa fa-plus"></i>
                                                            </button>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="tab-pane" id="lD2">
                                    <div class="col-lg-12">
                                        <div class="row">
                                            <div class="panel panel-default">
                                                <div class="panel-body">
                                                    <div class="row">
                                                        <div class="col-lg-12">
                                                            <asp:UpdatePanel runat="server">
                                                                <ContentTemplate>
                                                                    <asp:GridView runat="server" ID="gvBrokerDocuments"
                                                                        AllowPaging="false"
                                                                        AllowSorting="false"
                                                                        CssClass="table table-hover table-responsive"
                                                                        AutoGenerateColumns="false"
                                                                        OnRowCommand="gvBrokerDocuments_RowCommand"
                                                                        GridLines="None">
                                                                        <HeaderStyle BackColor="#333333" ForeColor="White" Font-Bold="true" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="DocId" HeaderText="Id" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                                                                            <asp:BoundField DataField="DocumentName" HeaderText="Document Title" />
                                                                            <asp:BoundField DataField="Section" HeaderText="Section" />
                                                                            <asp:ButtonField ControlStyle-CssClass="fa fa-times-circle" ItemStyle-Width="10" CommandName="Del" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="#e2584d" />
                                                                            <%--<asp:ButtonField runat="server" ControlStyle-CssClass="btn btn-danger btn-sm" Text="Delete" CommandName="Del" ItemStyle-HorizontalAlign="Right" />--%>
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
                                        <div class="row">
                                            <div class="trans-command">
                                                <div class="btn-group" role="group">
                                                    <asp:UpdatePanel runat="server">
                                                        <ContentTemplate>
                                                            <button class="btn btn-success" data-toggle="modal" data-target="#modalBrokerDocuments">
                                                                Add
                                                    <i class="fa fa-plus"></i>
                                                            </button>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="tab-pane" id="lE">
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <div class="panel panel-default">
                                                <div class="panel-body">
                                                    <div class="row">
                                                        <div class="col-lg-12">
                                                            <asp:GridView runat="server" ID="gvRoles"
                                                                AllowSorting="true"
                                                                CssClass="table table-hover table-responsive"
                                                                AutoGenerateColumns="false"
                                                                GridLines="None"
                                                                OnRowCommand="gvRoles_RowCommand">
                                                                <HeaderStyle BackColor="#333333" ForeColor="White" Font-Bold="true" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="Id" HeaderText="Id" />
                                                                    <asp:BoundField DataField="Sequence" HeaderText="Sequence" />
                                                                    <asp:BoundField DataField="Code" HeaderText="Code" />
                                                                    <asp:BoundField DataField="Name" HeaderText="Name" />
                                                                    <asp:ButtonField ControlStyle-CssClass="glyphicon glyphicon-edit" ItemStyle-Width="10" CommandName="Edt" ItemStyle-HorizontalAlign="Center" />
                                                                    <asp:ButtonField ControlStyle-CssClass="fa fa-times-circle" ItemStyle-Width="10" CommandName="Del" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="#e2584d" />
                                                                </Columns>
                                                                <PagerStyle CssClass="pagination-ys" />
                                                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <div class="trans-command">
                                                        <div class="btn-group" role="group">
                                                            <asp:UpdatePanel runat="server">
                                                                <ContentTemplate>
                                                                    <asp:LinkButton runat="server" CssClass="btn btn-success btn-width" ID="btnAddRole" OnClick="btnAddRole_Click">
                                                                        <i class="fa fa-plus"></i>
                                                                        <label runat="server" id="Label2" style="height: 5px; font-weight: normal;" />
                                                                        Add Role
                                                                    </asp:LinkButton>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>


                                <div class="tab-pane" id="lF">
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <div class="panel panel-default">

                                                <div class="panel-body">

                                                    <div class="col-lg-12" style="background-color: black;">
                                                        <asp:Label runat="server" CssClass="navbar-text label-white"> Broker Renewal Date </asp:Label>
                                                    </div>

                                                    <div class="row"></div>

                                                    <div class="row">
                                                        <div class="col-lg-12">
                                                            <div class="col-lg-1">
                                                                <asp:Label Text="From: " runat="server" />
                                                            </div>
                                                            <div class="col-lg-3">
                                                                <asp:TextBox ID="txtRenewalDateFrom" type="date" class="form-control" runat="server" />
                                                            </div>
                                                            <div class="col-lg-1">
                                                                <asp:Label Text="To: " runat="server" />
                                                            </div>
                                                            <div class="col-lg-3">
                                                                <asp:TextBox ID="txtRenewalDate" type="date" class="form-control" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <hr />
                                                    <div class="row"></div>

                                                    <div class="row">
                                                        <div class="col-lg-12">
                                                            <div class="col-lg-1">
                                                                <asp:Label Text="Grace Period: " runat="server" />
                                                            </div>
                                                            <div class="col-lg-3">
                                                                <asp:TextBox ID="txtGracePeriod" class="form-control text-right" runat="server" TextMode="Number" />
                                                            </div>
                                                            <div class="col-lg-1">
                                                                <asp:Label Text="Minimum Days: " runat="server" />
                                                            </div>
                                                            <div class="col-lg-3">
                                                                <asp:TextBox ID="txtMinimumDay" class="form-control text-right" runat="server" TextMode="Number" />
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>

                                                <div class="row"></div>

                                                <div class="row">
                                                    <div class="col-lg-12">
                                                        <div class="trans-command">
                                                            <div class="btn-group" role="group">
                                                                <asp:UpdatePanel runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:Button runat="server" ID="btnSaveRenewalDate" OnClick="btnSaveRenewalDate_Click" Text="Save" class="btn btn-success" />

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
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="modals" runat="server">
    <div id="modalValidValues" class="modal fade">
        <div class="modal-dialog modal-info" runat="server">
            <div class="modal-content">
                <div class="modal-header">
                    Valid Values [Data Entry]
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-lg-6">
                                    Group
                                </div>
                                <div class="col-lg-6">
                                    <asp:TextBox runat="server" ID="txtGroup" CssClass="form-control disabled" Enabled="false" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-6">
                                    Code
                                </div>
                                <div class="col-lg-6">
                                    <asp:TextBox runat="server" ID="txtCode" CssClass="form-control" Enabled="false" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-6">
                                    Name
                                </div>
                                <div class="col-lg-6">
                                    <asp:TextBox runat="server" ID="txtName" CssClass="form-control" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <button runat="server" id="btnEdit" style="width: 100px;" class="btn btn-primary" type="button" onserverclick="btnEdit_ServerClick">Edit</button>
                            <button runat="server" style="width: 100px;" class="btn btn-danger" type="button" data-dismiss="modal">Cancel </button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div id="modalTerms" class="modal fade">
        <div class="modal-dialog modal-info" runat="server">
            <div class="modal-content">
                <div class="modal-header">
                    Add Payment Terms
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-lg-6">
                                    Terms
                                </div>
                                <div class="col-lg-6">
                                    <asp:TextBox runat="server" ID="txtTerms" CssClass="form-control" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <button runat="server" id="btnAddTerms" class="btn btn-primary btn-width" type="button" onserverclick="btnAddTerms_ServerClick">Add</button>
                            <button runat="server" class="btn btn-danger btn-width" type="button" data-dismiss="modal">Cancel </button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div id="modalDocuments" class="modal fade">
        <div class="modal-dialog modal-info" runat="server">
            <div class="modal-content">
                <div class="modal-header">
                    Add Buyer Documents
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-lg-6">
                                    Documents
                                </div>
                                <div class="col-lg-6">
                                    <asp:TextBox runat="server" AutoPostBack="true" ID="txtDocuments" OnTextChanged="txtDocuments_TextChanged" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-6">
                                    Business Type
                                </div>
                                <div class="col-lg-6">
                                    <asp:DropDownList runat="server" ID="ddlBusinessType" class="form-control">
                                        <asp:ListItem Value="Individual" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="Corporation"> </asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <button runat="server" id="btnAddDocuments" style="width: 100px;" class="btn btn-primary" type="button" onserverclick="btnAddDocuments_ServerClick">Add</button>
                            <button runat="server" style="width: 100px;" class="btn btn-danger" type="button" data-dismiss="modal">Cancel </button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div id="modalBrokerDocuments" class="modal fade">
        <div class="modal-dialog modal-info" runat="server">
            <div class="modal-content">
                <div class="modal-header">
                    Add Broker Documents
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-lg-6">
                                    Documents
                                </div>
                                <div class="col-lg-6">
                                    <asp:TextBox runat="server" ID="txtBrokerDocument" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-6">
                                    Section
                                </div>
                                <div class="col-lg-6">
                                    <label>Section</label>
                                    <asp:DropDownList runat="server" ID="ddlBrokerSection" class="form-control">
                                        <asp:ListItem Value="---Select Position---" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Standard"> </asp:ListItem>
                                        <asp:ListItem Value="2" Text="Sole Proprietorship"> </asp:ListItem>
                                        <asp:ListItem Value="3" Text="Partnership"> </asp:ListItem>
                                        <asp:ListItem Value="4" Text="Corporation"> </asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <button runat="server" id="btnAddBrokerDocuments" style="width: 100px;" class="btn btn-primary" type="button" onserverclick="btnAddBrokerDocuments_ServerClick">Add</button>
                            <button runat="server" style="width: 100px;" class="btn btn-danger" type="button" data-dismiss="modal">Cancel </button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div id="modalRole" class="modal fade">
        <div class="modal-dialog modal-info" runat="server">
            <div class="modal-content">
                <div class="modal-header">
                    User Role
                </div>
                <div class="modal-body">
                    <asp:TextBox runat="server" ID="txtRoleId" CssClass="form-control hide" />
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-lg-6">
                                    Sequence
                                </div>
                                <div class="col-lg-6">
                                    <asp:TextBox runat="server" ID="txtSequence" CssClass="form-control" TextMode="Number" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-6">
                                    Code
                                </div>
                                <div class="col-lg-6">
                                    <asp:TextBox runat="server" ID="txtRoleCode" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-6">
                                    Name
                                </div>
                                <div class="col-lg-6">
                                    <asp:TextBox runat="server" ID="txtRoleName" CssClass="form-control" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <button runat="server" id="btnAddUserRole" style="width: 100px;" class="btn btn-primary" type="button" onserverclick="btnAddUserRole_ServerClick">Add</button>
                            <button runat="server" style="width: 100px;" class="btn btn-danger" type="button" data-dismiss="modal">Cancel </button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
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
    <div id="modalAlert" class="modal fade">
        <div class="modal-dialog" style="width: 30%;" id="Div1" runat="server">
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
</asp:Content>

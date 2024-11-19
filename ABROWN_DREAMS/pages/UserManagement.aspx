<%@ Page Title="DREAMS | User Management" Language="C#" MasterPageFile="~/master/Main.Master" AutoEventWireup="true" CodeBehind="UserManagement.aspx.cs" Inherits="ABROWN_DREAMS.UserManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function hideCreateUser() {
            $('#CreateUser').modal('hide');
        }
        function showCreateUser() {
            $('#CreateUser').modal('show');
        }
        function resetValueAddUser() {
            $('.AddUserModal').find('input:text').val('');
            $('.AddUserModal').find('input:password').val('');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="sitemap" runat="server">
    <li><a href="#"><span>User Management</span></a></li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                        </div>
                        <div class="col-lg-10 col-md-10 col-sm-10 col-xs-10">
                            <asp:Panel runat="server" DefaultButton="btnSearch">
                                <div class="input-group">
                                    <input runat="server" id="txtSearch" placeholder="Search name here" class="form-control" type="text" autofocus /><span class="input-group-btn">
                                        <asp:LinkButton runat="server" ID="btnSearch" Style="width: 50px;" CssClass="btn btn-secondary btn-default btn-facebook" OnClick="btnSearch_ServerClick"><i class="fa fa-search"></i></asp:LinkButton>
                                    </span>
                                </div>
                            </asp:Panel>
                        </div>
                        <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1">
                        </div>
                    </div>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                            <fieldset>
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvUserList" runat="server"
                                            AllowPaging="true"
                                            AutoGenerateColumns="false"
                                            EmptyDataText="No Records Found"
                                            CssClass="table table-responsive table-hover"
                                            Width="100%" ShowHeader="True"
                                            PageSize="10"
                                            OnRowCommand="gvUserList_RowCommand"
                                            OnPageIndexChanging="gvUserList_PageIndexChanging">
                                            <HeaderStyle BackColor="#333333" ForeColor="White" Font-Bold="true" />
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="License">
                                                    <ItemTemplate>
                                                        <div class="col-md-12">
                                                            <asp:CheckBox runat="server" ID="chkLicense" />
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ID" HeaderText="ID" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                                                <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username" />
                                                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-Width="100%" />
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Edit User Details">
                                                    <ItemTemplate>
                                                        <div class="col-md-12">
                                                            <asp:LinkButton runat="server" ID="btnEdit" CssClass="btn btn-default btn-primary btn-sm" OnClick="btnEdit_Click" CommandArgument='<%# Bind("ID")%>'><i class="fa fa-edit"></i></asp:LinkButton>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:ButtonField ButtonType="Link" HeaderText="Authorization" ControlStyle-CssClass="btn btn-default btn-success fa fa-arrow-right" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" CommandName="Sel" />
                                                <%-- <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="1%">
                                                    <ItemTemplate>
                                                        <div class="col-md-12">
                                                            <asp:LinkButton runat="server" ID="btnSelect" CssClass="btn btn-default btn-success btn-sm fa fa-arrow-right" OnClick="btnSelect_Click" CommandArgument='<%# Bind("ID")%>' />
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                            </Columns>
                                            <PagerStyle CssClass="pagination-ys" />
                                            <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </fieldset>
                        </div>
                        <div class="col-lg-7 col-md-7 col-sm-12 col-xs-12">
                            <fieldset>
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvModule" runat="server" AllowPaging="false" AutoGenerateColumns="false" EmptyDataText="Select user to view modules"
                                            CssClass="table table-bordered table-hover" Width="100%" ShowHeader="True" OnPageIndexChanging="gvUserList_PageIndexChanging">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="1px">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="SelectMod" runat="server" OnCheckedChanged="SelectMod_CheckedChanged" AutoPostBack="true" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chk" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="FeatID" HeaderText="Code" SortExpression="FeatID" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="hide" />
                                                <asp:BoundField DataField="FeatDesc" HeaderText="Module" SortExpression="FeatDesc" />
                                            </Columns>
                                            <PagerStyle CssClass="pagination-ys" />
                                            <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </fieldset>
                        </div>
                    </div>
                    <div class="row">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                    <button runat="server" id="btnAddUser" class="btn btn-info btn-width" type="button" data-toggle="modal" data-target="#CreateUser" onserverclick="btnAddUser_ServerClick">Add user <i class="glyphicon glyphicon-plus"></i></button>
                                    <asp:LinkButton runat="server" CssClass="btn btn-success" ID="btnSave" OnClick="btnSave_Click">
                            Save Changes
                                <i class="glyphicon glyphicon-save"></i>
                                    </asp:LinkButton>
                                    <%--<button class="btn btn-danger btn-width pull-right" type="button" runat="server" id="btnCancel">Cancel <i class="glyphicon glyphicon-remove"></i></button>--%>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="modals" runat="server" >
    <div class="modal fade" id="CreateUser"  data-backdrop="static" data-keyboard="false" style="overflow-y: scroll;">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header bg-color">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                            <h4 class="modal-title">
                                <label runat="server" id="lblAddUser" />
                            </h4>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 AddUserModal">
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <h5>Name :</h5>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-4 col-md-4 col-sm-4 col-xs-4">
                                            <input runat="server" type="text" class="form-control" id="tLastName" placeholder="Last Name" />
                                        </div>
                                        <div class="col-lg-4 col-md-4 col-sm-4 col-xs-4">
                                            <input runat="server" type="text" class="form-control" id="tFirstName" placeholder="First Name" />
                                        </div>
                                        <div class="col-lg-4 col-md-4 col-sm-4 col-xs-4">
                                            <input runat="server" type="text" class="form-control" id="tMiddleInitial" placeholder="Middle Name" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <h5>User Details :</h5>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-4 col-md-4 col-sm-4 col-xs-4">
                                            <input runat="server" type="email" class="form-control" id="tEmail" placeholder="email@hotmail.com" />
                                        </div>
                                        <div class="col-lg-4 col-md-4 col-sm-4 col-xs-4">
                                            <input runat="server" type="text" class="form-control" maxlength="15" id="tUserID" placeholder="User ID" />
                                        </div>
                                        <div class="col-lg-4 col-md-4 col-sm-4 col-xs-4">
                                            <asp:TextBox ID="tPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Password" />

                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <h5>Role :</h5>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <asp:GridView runat="server" ID="gvRole"
                                                AllowPaging="true"
                                                AllowSorting="true"
                                                EmptyDataText="No Records Found"
                                                Width="100%"
                                                CssClass="table table-hover table-responsive"
                                                AutoGenerateColumns="false"
                                                OnPageIndexChanging="gvRole_PageIndexChanging"
                                                DataKeyNames="Sequence" 
                                                GridLines="None">
                                                <HeaderStyle BackColor="#66ccff" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="SelectAll" runat="server" OnCheckedChanged="SelectAll_CheckedChanged" AutoPostBack="true" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkRow" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Sequence" HeaderText="Sequence" ItemStyle-Width="0px" />
                                                    <asp:BoundField DataField="Code" HeaderText="Role Code" />
                                                    <asp:BoundField DataField="Name" HeaderText="Role Name" />
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
                <div class="modal-footer">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <button runat="server" id="btnUserAdd" style="width: 100px;" class="btn btn-width btn-primary" type="button" onserverclick="btnUserAdd_ServerClick">
                                <label runat="server" id="lblAddUpdateUser" />
                            </button>
                            <button runat="server" style="width: 100px;" class="btn btn-default" type="button" data-dismiss="modal">Cancel </button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div id="modalAlert" class="modal fade" role="dialog" style="overflow-y: scroll;">
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
</asp:Content>

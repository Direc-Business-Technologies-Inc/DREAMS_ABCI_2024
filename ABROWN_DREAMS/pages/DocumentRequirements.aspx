<%@ Page Title="DREAMS | Document Requirements" Language="C#" MasterPageFile="~/master/Main.Master" AutoEventWireup="true" CodeBehind="DocumentRequirements.aspx.cs" Inherits="ABROWN_DREAMS.DocumentRequirements" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function showBuyersList() {
            $('#modalBuyers').modal('show');
        }
        function hideBuyersList() {
            $('#modalBuyers').modal('hide');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="sitemap" runat="server">
    <li><a href="#"><span>Document Requirements List</span></a></li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div class="panel panel-primary panel-trans">
        <div class="panel-heading">
            <h5>DOCUMENT REQUIREMENTS</h5>
        </div>
    </div>
    <div class="col-lg-6">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="panel panel-default">
                    <div class="panel-body">
                        <asp:GridView ID="gvRoles" runat="server"
                            AutoGenerateColumns="false"
                            EmptyDataText="No Records Found"
                            CssClass="table table-hover table-responsive"
                            Width="100%"
                            ShowHeader="True"
                            GridLines="None"
                            SelectedRowStyle-BackColor="#A1DCF2"
                            OnRowCommand="gvRoles_RowCommand">
                            <HeaderStyle BackColor="#333333" ForeColor="White" Font-Bold="true" />
                            <Columns>
                                <asp:BoundField DataField="Code" HeaderText="Code" ItemStyle-Width="20%" />
                                <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="100%" />
                                <asp:ButtonField ButtonType="Link" ControlStyle-CssClass="btn btn-default btn-success fa fa-arrow-right" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" CommandName="Sel" />
                            </Columns>
                            <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black"/>
                        </asp:GridView>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="col-lg-6">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:Panel runat="server" Enabled="false" ID="pnlDoclist">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <asp:Label runat="server" ID="lblTitle" Text="Choose department" Font-Size="16"></asp:Label>
                            <asp:LinkButton runat="server" ID="btnCancel" OnClick="btnCancel_Click" CssClass="btn btn-danger pull-right" Width="80">Cancel &nbsp;<i class="fa fa-close"></i></asp:LinkButton>
                            <asp:LinkButton runat="server" ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-success pull-right" Width="80">Save &nbsp;<i class="fa fa-save"></i></asp:LinkButton>
                            <div class="clearfix"></div>
                        </div>
                        <div class="panel-body">
                            <asp:GridView ID="gvDocs" runat="server"
                                AutoGenerateColumns="false"
                                EmptyDataText="No Records Found"
                                CssClass="table table-hover table-responsive"
                                Width="100%"
                                ShowHeader="True"
                                GridLines="None"
                                UseAccessibleHeader="true">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="1%">
                                        <ItemTemplate>
                                            <div class="col-md-12">
                                                <asp:CheckBox runat="server" ID="chk" />
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DocId" HeaderText="Doc Id" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                                    <asp:BoundField DataField="Document" HeaderText="Document List" ItemStyle-Width="100%" />
                                    <asp:BoundField DataField="Code" HeaderText="Code" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                                </Columns>
                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black"/>
                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="modals" runat="server">
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
                                        <asp:Label runat="server" ID="lblMessageAlert" CssClass="lblMessageAlert"></asp:Label></h4>
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

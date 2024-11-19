<%@ Page Title="" Language="C#" MasterPageFile="~/master/Main.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="Cleanup.aspx.cs" Inherits="ABROWN_DREAMS.Cleanup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function hideAlert() {
            $('#modalAlert').modal('hide');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="sitemap" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div class="col-lg-12">
        <div class="row">
            <div class="col-lg-12">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="btn-pref btn-group btn-group-justified btn-group-lg" role="group" aria-label="...">
                            <div class="btn-group" role="group">
                                <button type="button" runat="server" id="bQuotation" onserverclick="bTab_ServerClick" class="btn btn-success" href="#tabQuotation" data-toggle="tab">
                                    <%--<span class="fa fa-dollar" aria-hidden="true"></span>--%>
                                    <div class="hidden-xs">Quotation</div>
                                </button>
                            </div>

                            <div class="btn-group" role="group">
                                <button type="button" runat="server" id="bBuyers" onserverclick="bTab_ServerClick" class="btn btn-default" href="#tabBuyers" data-toggle="tab">
                                    <%--<span class="fa fa-calendar" aria-hidden="true"></span>--%>
                                    <div class="hidden-xs">Buyer's Info</div>
                                </button>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <div class="well" style="background-color: white;">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="tab-content">
                                <div class="tab-pane fade in active" runat="server" id="tabQuotation">
                                    <div class="table-responsive" style="border: 0px;">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <asp:Panel ID="pnlQuotation" runat="server">
                                                    <div class="row">
                                                        <div class="col-lg-1">
                                                            <h5>Date From</h5>
                                                        </div>
                                                        <div class="col-lg-2">
                                                            <asp:TextBox runat="server" ID="txtDateFrom" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-1">
                                                            <h5>Date To</h5>
                                                        </div>
                                                        <div class="col-lg-2">
                                                            <asp:TextBox runat="server" ID="txtDateTo" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-1">
                                                            <asp:Button runat="server" ID="btnGenerate" CssClass="btn btn-primary" Text="Generate" OnClick="btnGenerate_Click"></asp:Button>
                                                        </div>
                                                        <div class="col-lg-1">
                                                            <asp:Button runat="server" ID="btnClean" CssClass="btn btn-danger" Text="Cleanup" OnClick="btnClean_Click"></asp:Button>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-lg-12">
                                                            <asp:GridView runat="server"
                                                                ID="gvQuotation"
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
                                                                            <asp:UpdatePanel runat="server">
                                                                                <ContentTemplate>
                                                                                    <asp:CheckBox runat="server" AutoPostBack="true" ID="chkSel"
                                                                                        CommandName="chk" CausesValidation="false" />
                                                                                </ContentTemplate>
                                                                                <Triggers>
                                                                                    <asp:PostBackTrigger ControlID="chkSel" />
                                                                                </Triggers>
                                                                            </asp:UpdatePanel>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>


                                                                    <asp:BoundField DataField="DocEntry" HeaderText="DocEntry" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                                                    <asp:BoundField DataField="DocNum" HeaderText="Doc No." />
                                                                    <asp:BoundField DataField="DocDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                                                    <asp:BoundField DataField="Name" HeaderText="Status" />
                                                                    <asp:BoundField DataField="ProjCode" HeaderText="Project" />
                                                                    <asp:BoundField DataField="Block" HeaderText="Block" />
                                                                    <asp:BoundField DataField="Lot" HeaderText="Lot" NullDisplayText=" " />
                                                                    <asp:BoundField DataField="Model" HeaderText="Model" />
                                                                </Columns>
                                                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                                                <PagerStyle CssClass="pagination-ys" />
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div class="tab-pane fade in" runat="server" id="tabReservation">
                                    <div class="table-responsive" style="border: 0px;">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <asp:Panel runat="server" ID="pnlReservation">
                                                    <div class="row">
                                                        <div class="col-lg-1">
                                                            <h5>Date From</h5>
                                                        </div>
                                                        <div class="col-lg-2">
                                                            <asp:TextBox runat="server" ID="txtDateFrom2" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-1">
                                                            <h5>Date To</h5>
                                                        </div>
                                                        <div class="col-lg-2">
                                                            <asp:TextBox runat="server" ID="txtDateTo2" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-1">
                                                            <asp:Button runat="server" ID="btnGenerate2" CssClass="btn btn-primary" Text="Generate" OnClick="btnGenerate2_Click"></asp:Button>
                                                        </div>
                                                        <div class="col-lg-1">
                                                            <asp:Button runat="server" ID="btnClean2" CssClass="btn btn-danger" Text="Cleanup" OnClick="btnClean2_Click"></asp:Button>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-lg-12">
                                                            <asp:GridView runat="server"
                                                                ID="gvReservation"
                                                                CssClass="table table-responsive"
                                                                AutoGenerateColumns="false"
                                                                GridLines="None"
                                                                ShowHeader="true"
                                                                SelectedRowStyle-BackColor="#A1DCF2"
                                                                UseAccessibleHeader="true">
                                                                <HeaderStyle BackColor="#d7dce5" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="DocEntry" HeaderText="DocEntry" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                                                    <asp:BoundField DataField="DocNum" HeaderText="Doc No." />
                                                                    <asp:BoundField DataField="DocDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                                                    <asp:BoundField DataField="Name" HeaderText="Status" />
                                                                    <asp:BoundField DataField="ProjCode" HeaderText="Project" />
                                                                    <asp:BoundField DataField="Block" HeaderText="Block" />
                                                                    <asp:BoundField DataField="Lot" HeaderText="Lot" NullDisplayText=" " />
                                                                    <asp:BoundField DataField="Model" HeaderText="Model" />
                                                                </Columns>
                                                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                                                <PagerStyle CssClass="pagination-ys" />
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>




                                <div class="tab-pane fade in" runat="server" id="tabBuyers">
                                    <div class="table-responsive" style="border: 0px;">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <asp:Panel runat="server" ID="pnlBuyer">
                                                    <div class="row">


                                                        <div class="col-lg-1">
                                                            <asp:Button runat="server" ID="btnGenerate3" CssClass="btn btn-primary" Text="Generate" OnClick="btnGenerate3_Click"></asp:Button>
                                                        </div>
                                                        <div class="col-lg-1">
                                                            <asp:Button runat="server" ID="btnCleanup3" CssClass="btn btn-danger" Text="Cleanup" OnClick="btnCleanup3_Click"></asp:Button>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-lg-12">
                                                            <asp:GridView runat="server"
                                                                ID="gvBuyers"
                                                                CssClass="table table-responsive"
                                                                AutoGenerateColumns="false"
                                                                GridLines="None"
                                                                ShowHeader="true"
                                                                SelectedRowStyle-BackColor="#A1DCF2"
                                                                UseAccessibleHeader="true"
                                                                EmptyDataText="No records found">
                                                                <HeaderStyle BackColor="#d7dce5" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="CardCode" HeaderText="Code" /> 
                                                                    <asp:BoundField DataField="FirstName" HeaderText="Buyer Name" />
                                                                    <asp:BoundField DataField="Gender" HeaderText="Gender" />
                                                                    <asp:BoundField DataField="BirthDay" HeaderText="Birthday" DataFormatString="{0:dd-MMM-yyyy}" />
                                                                </Columns>
                                                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                                                <PagerStyle CssClass="pagination-ys" />
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
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
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="modals" runat="server">
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
</asp:Content>

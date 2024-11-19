<%@ Page Title="" Language="C#" MasterPageFile="~/master/Main.Master" AutoEventWireup="true" CodeBehind="Licensing.aspx.cs" Inherits="ABROWN_DREAMS.pages.Licensing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        $(document).ready(function () {

            var textFile = "";

            function extractstring() {

                const matched = extractFromString(textFile);

                $.post('/Licensing.aspx/ReadLicense',
                    {
                        DateMM: matched[0],
                        DateDD: matched[1],
                        DateYYYY: matched[2],
                        ComputerName: matched[3],
                        HardwareKey: matched[4],
                        ProductId: matched[5],
                        NumberOfUsers: matched[6]
                    }).done(result => {
                        alert(result);
                    });
            }

            document.getElementById('<%=lblFileName.ClientID%>').addEventListener('change', readFile, false);

            function extractFromString(text) {

                const matched = [];
                var cnt = 0;

                var strStart = "3Qg1";
                var strEnd = "3Qg2";

                var indexStart = 0;
                var indexEnd = 0;

                var exit = false;

                while (!exit) {
                    indexStart = text.indexOf(strStart);
                    indexEnd = text.indexOf(strEnd);

                    if (indexStart != -1 && indexEnd != -1) {

                        matched[cnt] = text.substring(indexEnd,
                            indexStart + strStart.length);

                        text = text.substring(indexEnd + strEnd.length);

                        cnt++;
                    }
                    else
                        exit = true;
                }

                return matched;

            }

            function readFile(evt) {
                var files = evt.target.files;
                var file = files[0];
                var reader = new FileReader();
                reader.onload = function (event) {
                    textFile = event.target.result;
                }
                reader.readAsText(file)
            }
        });
    </script>

    <%--ALERTS--%>
    <script type="text/javascript">
        function ShowAlert() {
            $('#modalAlert').modal('show');
        }
        function HideAlert() {
            $('#modalAlert').modal('hide');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="sitemap" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="panel panel-default">
                    <div class="panel-body">
                        <div class="box box-success col-md-12">
                            <div class="box-header with-border">
                                <h3 class="box-title titlemargin">General Information</h3>
                            </div>
                            <div class="box-body">
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="box box-success" style="overflow-x: auto;">
                                            <div class="box-header with-border">
                                                <asp:LinkButton runat="server" ID="btnRequestLicense" Text="Request License" OnClick="btnRequestLicense_Click" CssClass="btn btn-primary" />
                                            </div>
                                            <br />
                                            <div class="box-body titlemargin">
                                                <asp:GridView ID="gvLicense" runat="server" AllowPaging="false" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                                    CssClass="table table-bordered table-hover" Width="100%" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" ShowHeader="True">
                                                    <Columns>
                                                        <asp:BoundField DataField="ProductName" HeaderText="Product Name" SortExpression="ProductName" />
                                                        <asp:BoundField DataField="ProductVersion" HeaderText="Product Version" SortExpression="ProductVersion" />
                                                        <asp:BoundField DataField="ComputerName" HeaderText="Computer Name" SortExpression="ComputerName" />
                                                        <asp:BoundField DataField="HardwareKey" HeaderText="Hardware Key" SortExpression="HardwareKey" />
                                                        <asp:BoundField DataField="CompanyName" HeaderText="Company Name" SortExpression="CompanyName" />
                                                        <%--<asp:BoundField DataField="ContactPerson" HeaderText="Contact Person" SortExpression="ContactPerson" />
                                                                <asp:BoundField DataField="EmailAddress" HeaderText="Email Address" SortExpression="EmailAddress" />--%>
                                                        <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center" ItemStyle-BackColor="Yellow" HeaderText="Contact Person">
                                                            <ItemTemplate>
                                                                <asp:TextBox runat="server" ID="gvContactPerson" BorderStyle="None" BackColor="Yellow" placeholder="Input Contact Person"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center" ItemStyle-BackColor="Yellow" HeaderText="Company Name">
                                                            <ItemTemplate>
                                                                <asp:TextBox runat="server" ID="gvCompanyName" BorderStyle="None" BackColor="Yellow" placeholder="Input Company Name"></asp:TextBox>
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

                            </div>
                        </div>

                        <div class="col-md-12">
                            <hr style="margin-left: 0px; border-color: black;" class="col-md-12">
                        </div>

                        <div class="box box-success col-md-12">
                            <div class="box-header with-border">
                                <h3 class="box-title titlemargin">Import License File</h3>
                            </div>
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-success" style="overflow-x: auto">
                                            <div class="box-body titlemargin">
                                                <div class="col-md-4">
                                                    <asp:Label runat="server" Text="Import File: "></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <div class="input-group col-sm-offset-3">
                                                                <asp:Label runat="server" ID="lblFileName" Text=""></asp:Label>
                                                                <asp:FileUpload ID="FileUpload" CssClass="btn btn-default" CommandName="FileUpload" runat="server" EnableViewState="true" />
                                                                <asp:LinkButton ID="btnUpload" CssClass="btn btn-primary" OnClick="btnUpload_Click" runat="server" Text="Upload" />
                                                                <asp:LinkButton ID="btnPreview" CssClass="btn btn-info" OnClick="btnPreview_Click" runat="server" Text="Preview" />
                                                                <asp:LinkButton ID="btnRemove" CssClass="btn btn-danger" OnClick="btnRemove_Click" runat="server" Text="Remove" />
                                                            </div>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="btnUpload" />
                                                            <asp:PostBackTrigger ControlID="btnPreview" />
                                                            <asp:PostBackTrigger ControlID="btnRemove" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:LinkButton runat="server" ID="btnImportLicense" OnClientClick="extractstring();" Text="Import License File" CssClass="btn btn-primary pull-right" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
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

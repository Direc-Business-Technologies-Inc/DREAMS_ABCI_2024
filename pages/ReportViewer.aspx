<%@ Page Title="DREAMS | Report Viewer" Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.aspx.cs" Inherits="ABROWN_DREAMS.ReportViewer" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <title></title>
    <%-- Meta --%>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="Realty">
    <%-- /Meta --%>
    <%-- Links --%>
    <link rel="shortcut icon" href="../assets/img/Icon.ico" />
    <link rel="stylesheet" href="../assets/css/bootstrap3-wysihtml5.min.css" />
    <link rel="stylesheet" href="../assets/css/bootstrap.min.css" />
    <link rel="stylesheet" href="../assets/css/styles.css" />
    <link rel="stylesheet" href="../assets/fonts/font-awesome.min.css" />
    <link rel="stylesheet" href="../assets/css/pagination.css" />
    <link rel="stylesheet" href="../assets/css/custom.css" />
    <link rel="stylesheet" href="../assets/css/wizard.css" />
    <link rel="stylesheet" href="../assets/css/sweetalert.css" />
    <link rel="stylesheet" href="../assets/css/bootstrap-datetimepicker.min.css" />
    <%--draggable panel--%>
    <link href="../lib/fontawesome-free-5.3.1-web/css/fontawesome.min.css" rel="stylesheet" />
    <link href="../assets/css/jquery-ui.css" rel="stylesheet" />
    <%--<link href="//maxcdn.bootstrapcdn.com/font-awesome/4.2.0/css/font-awesome.min.css" rel="stylesheet">--%>
    <%--<link rel="stylesheet" href="//ajax.googleapis.com/ajax/libs/jqueryui/1.10.2/themes/smoothness/jquery-ui.css" />--%>
    <%-- /Links --%>
    <%-- JavaScript --%>
    <script src="../assets/js/jquery.min.js"></script>
    <script src="../assets/js/bootstrap.min.js"></script>
    <script src="../assets/js/bootstrap-datetimepicker.min.js"></script>
    <%--<script src="//ajax.googleapis.com/ajax/libs/jqueryui/1.10.2/jquery-ui.min.js"></script>--%>
    <%--fabric js for html5 canvas--%>
    <script src="../assets/js/fabric.js"></script>
    <script type="text/javascript">



        function Print() {
            var dvReport = document.getElementById("dvReport");

            var frame1 = dvReport.getElementsByTagName("iframe")[0];
            if (navigator.appName.indexOf("Internet Explorer") != -1) {
                frame1.name = frame1.id;
                window.frames[frame1.id].focus();
                window.frames[frame1.id].print();
            }
            else {
                var frameDoc = frame1.contentWindow ? frame1.contentWindow : frame1.contentDocument.document ? frame1.contentDocument.document : frame1.contentDocument;
                frameDoc.print();
            }
        }



        function shwwindow(myurl) {
            window.open(myurl, '_blank');
        }
    </script>
    <script>
        function sweetAlert(title, content, type) {
            swal(title, content, type);
        }
        function showAlert() {
            $('#modalAlert').modal('show');
        }
        function showLogout() {
            $('#modalLogout').modal('show');
        }
        function showConfirmation() {
            $('#modalConfirmation').modal('show');
        }
        function closeConfirmation() {
            $('#modalConfirmation').modal('hide');
        }
        $(function () {
            $('a[title]').tooltip();
        });
    </script>
</head>

<body>
    <form runat="server">
        <asp:ScriptManager ID="ScriptManager" runat="server" />
        <div style="text-align: center;">
            <asp:LinkButton ID="btnPrint" runat="server" CssClass="btn btn-primary" OnClientClick="Print();" AccessKey="P">Print &nbsp; <i class="fa fa-print"></i> </asp:LinkButton>
        </div>
        <center>
             <div id="dvReport">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        
<%--                                                <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" OnInit="crvPage_Init" OnUnload="Page_Unload" AutoDataBind="true" EnableDatabaseLogonPrompt="False" HasCrystalLogo="True" HasExportButton="False" 
                            HasPrintButton="true" HasToggleGroupTreeButton="false" EnableDrillDown="False" HasDrillUpButton="False" 
                            ToolPanelView="None" HasRefreshButton="False" HasSearchButton ="False" ShowAllPageIds="true" SeparatePages="False"/>--%>
                         

  <%--                                                                      <CR:CrystalReportViewer ID="crstalTest" runat="server" 
                                                                            OnUnload="Page_Unload" AutoDataBind="true" 
                                                                           EnableDatabaseLogonPrompt="False" HasCrystalLogo="True" 
                                                                           HasExportButton="False"  HasPrintButton="true" 
                                                                           HasToggleGroupTreeButton="false" EnableDrillDown="False" 
                                                                           HasDrillUpButton="False" ToolPanelView="None" 
                                                                           HasRefreshButton="False" HasSearchButton ="False" 
                                                                           ShowAllPageIds="true" SeparatePages="False"/>--%>

<%--                                                                        <CR:CrystalReportViewer runat="server"  ID="crstalTest2"
                                                                              OnUnload="Page_Unload" AutoDataBind="true" 
                                                                           EnableDatabaseLogonPrompt="False" HasCrystalLogo="True" 
                                                                           HasExportButton="False"  HasPrintButton="true" 
                                                                           HasToggleGroupTreeButton="false" EnableDrillDown="False" 
                                                                           HasDrillUpButton="False" ToolPanelView="None" 
                                                                           HasRefreshButton="False" HasSearchButton ="False" 
                                                                           ShowAllPageIds="true" SeparatePages="False" />--%>

                                               <%--                            <CR:CrystalReportViewer runat="server" ID="crstalTest2"
                                                                              OnUnload="Page_Unload" AutoDataBind="true" 
                                                                           EnableDatabaseLogonPrompt="False" HasCrystalLogo="True" 
                                                                           HasExportButton="False"  HasPrintButton="true" 
                                                                           HasToggleGroupTreeButton="false" EnableDrillDown="False" 
                                                                           HasDrillUpButton="False" ToolPanelView="None" 
                                                                           HasRefreshButton="False" HasSearchButton ="False" 
                                                                           ShowAllPageIds="true" SeparatePages="False"  />--%>
                                                                            

<%--                                                                            <CR:CrystalReportViewer runat="server" ID="crystalVIewer2"
                                                                                 OnUnload="Page_Unload" AutoDataBind="true" 
                                                                           EnableDatabaseLogonPrompt="False" HasCrystalLogo="True" 
                                                                           HasExportButton="False"  HasPrintButton="true" 
                                                                           HasToggleGroupTreeButton="false" EnableDrillDown="False" 
                                                                           HasDrillUpButton="False" ToolPanelView="None" 
                                                                           HasRefreshButton="False" HasSearchButton ="False" 
                                                                           ShowAllPageIds="true" SeparatePages="False"
                                                                                />--%>

                                                                                <CR:CrystalReportViewer runat="server"  ID="crystal" AutoDataBind="true" />

                    </ContentTemplate>
                </asp:UpdatePanel>
             </div>
        </center>
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

    </form>
</body>
</html>

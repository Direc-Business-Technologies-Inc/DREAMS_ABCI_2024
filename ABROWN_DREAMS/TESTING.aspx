<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TESTING.aspx.cs" Inherits="ABROWN_DREAMS.TESTING" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>
<html>
<head>
    <title></title>
    <link rel="stylesheet" href="../assets/css/custom.css" />
    <link rel="stylesheet" href="../assets/css/wizard.css" />
    <link rel="shortcut icon" href="../assets/img/Icon.ico" />
    <link rel="stylesheet" href="../assets/css/bootstrap3-wysihtml5.min.css" />
    <link rel="stylesheet" href="../assets/css/bootstrap.min.css" />
    <link rel="stylesheet" href="../assets/css/styles.css" />
    <link rel="stylesheet" href="../assets/fonts/font-awesome.min.css" />
    <link rel="stylesheet" href="../assets/css/pagination.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.9/jquery-ui.min.js"></script>

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
    </script>
</head>
<body>
    <form runat="server">
        <asp:ScriptManager ID="ScriptManager" runat="server" />

        <div id="dvReport">
            <%--<CR:CrystalReportViewer ID="crViewer" runat="server" OnInit="crvPage_Init" AutoDataBind="true" EnableDatabaseLogonPrompt="False" HasCrystalLogo="True" EnableDrillDown="False" HasDrillUpButton="False" ToolPanelView="None" HasRefreshButton="True" />--%>
        </div>

        <asp:Button ID="btnPrint" runat="server" Text="Button" OnClientClick="Print();" />
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="~/pages/Login.aspx.cs" Inherits="ABROWN_DREAMS.Login" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <title>DREAMS</title>
    <%-- Meta --%>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="Backend integration">
    <meta name="author" content="Danilo A. Veroy II">
    <%-- /Meta --%>
    <%-- Links --%>
    <link rel="stylesheet" href="../assets/css/sweetalert.css" />
    <link rel="shortcut icon" href="../assets/img/Icon.ico">
    <link rel="stylesheet" href="../assets/css/style-login.css">
    <link rel="stylesheet" href="../assets/css/bootstrap-theme-login.css">
    <%-- /Links --%>


    <script type="text/javascript">
        $(document).ready(function () {
            alert('test');

            var SAOHana = '<%=ConfigurationManager.AppSettings["SAODatabase"].ToString() %>';
            var SAPHana = '<%=ConfigurationManager.AppSettings["HANADatabase"].ToString() %>';

            $("#AddonDBName").text(SAOHana);
            $("#SapDBName").text(SAPHana);
            alert('test');
        });
    </script>
</head>



<body class="login-img-body">
    <form id="frmLogin" runat="server" class="login-form">
        <asp:ScriptManager ID="ScriptManager" runat="server" />
        <div class="container">
            <div class="login-wrap">
                <p class="login-img">
                    <img alt="DirecBTI" src="../assets/img/dreams logo.png" class="img-responsive" />
                    <%--<img alt="DirecBSI" src="../assets/img/DirecBSI.png" class="img-responsive" />--%>
                </p>
                <hr />

                <b>Database Connection:
                </b>

                <div class="text-primary    " style="font-weight: bolder;">
                    <%--2023-09-06 :   ADD DATABASE NAME --%>
                    <span>SAP: </span>
                    <asp:Label runat="server" ID="SapDBName"></asp:Label>
                    <br />
                    <span>ADDON: </span>
                    <asp:Label runat="server" ID="AddonDBName"></asp:Label>
                </div>
                <hr />

                <div class="input-group">
                    <span class="input-group-addon">
                        <asp:Image runat="server" ImageUrl="~/assets/img/iconUser.png" Width="15px" /></span>
                    <input id="txtUser" runat="server" type="text" class="form-control" placeholder="Username" autofocus>
                </div>
                <div class="input-group">
                    <span class="input-group-addon">
                        <asp:Image runat="server" ImageUrl="~/assets/img/iconPass.png" Width="15px" /></span>
                    <input id="txtPass" runat="server" type="password" class="form-control" placeholder="Password">
                </div>
                <button id="btnSubmit" runat="server" class="btn btn-primary btn-lg btn-block" type="submit" onserverclick="btnSubmit_ServerClick">Login</button>

                <p />
                <footer>
                    <div class="row"></div>
                    <div>
                        <div style="float: right;">
                            <img alt="Trademark" src="../assets/img/Trademark.png" class="img-responsive" />
                            <span>Version:</span><span id="DREAMSVersion" runat="server" autopostback="true"></span>
                        </div>

                    </div>


                    <%--                    <div class="left">
                        <span>SAP:</span><span id="AddonDBName"></span> || <span>Sap:</span><span id="SapDBName"></span>
                    </div>--%>
                </footer>
            </div>
        </div>



    </form>







    <div runat="server" id="MsgNoti" visible="false">
        <a href="#" class="close" data-dismiss="alert">&times;</a>
        <label runat="server" id="lblHeader" style="font-weight: bold;" />
        <label runat="server" id="lblMsg" />
    </div>


    <%-- JavaScript --%>
    <script src="../assets/js/jquery-2.2.3.min.js"></script>
    <script src="../assets/js/bootstrap.min.js"></script>
    <script src="../assets/js/custom.js"></script>
    <%-- /JavaScript --%>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="webError.aspx.cs" Inherits="ABROWN_DREAMS.webError" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">

    <title>404</title>

    <%-- Meta --%>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="author" content="Danilo A. Veroy II">
    <%-- /Meta --%>

    <%-- Links --%>
    <link rel="shortcut icon" href="../assets/img/Icon.ico" />
    <link rel="stylesheet" href="../assets/css/bootstrap-theme.css" />
    <link rel="stylesheet" href="../assets/css/styles.css" />
    <%-- /Links --%>
</head>

<body style="background-color: White;">
    <div class="container">
        <form id="frmError" runat="server">
            <br />
            <br />
            <br />
            <br />
            <br />
            <div>
                <section id="error" class="container" style="text-align: center;">
                    <p class="text-404">404</p>
                    <h1>Oops! Something went wrong.</h1>
                    We will work on fixing that right away. Meanwhile, you may return to dashboard.<br>
                    <br />
                    <a class="btn btn-danger" href="../pages/Dashboard.aspx">GO BACK TO DASHBOARD</a>
                    <a class="btn btn-success" href="http://www.direcbsi.com/main/?page=homepage&view=contact">CONTACT SUPPORT</a>
                </section>
            </div>
        </form>
    </div>
</body>
</html>


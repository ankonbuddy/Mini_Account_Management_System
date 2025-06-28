<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CodeBehind/Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Login</title>
    <!-- Bootstrap CDN -->
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
</head>
<body class="bg-light">
    <form id="form1" runat="server" class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-4">
                <h3 class="text-center mb-4">Login</h3>
                <div class="form-group">
                    <label for="txtUsername">Username</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label for="txtPassword">Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" />
                </div>
                <div class="text-center">
                    <asp:Button ID="btnLogin" runat="server" CssClass="btn btn-primary btn-block" Text="Login" OnClick="btnLogin_Click" />
                    <asp:Label ID="lblMessage" runat="server" CssClass="text-danger d-block mt-3" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>

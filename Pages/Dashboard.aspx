<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CodeBehind/Dashboard.aspx.cs" Inherits="Dashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dashboard - Employee Management System</title>
    <!-- Bootstrap 5 CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        .dashboard-card {
            max-width: 600px;
            margin: 2rem auto;
            box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15);
            border: none;
            border-radius: 10px;
        }
        .welcome-message {
            font-size: 1.5rem;
            margin-bottom: 1.5rem;
        }
        .btn-employee {
            padding: 0.5rem 2rem;
            font-size: 1.1rem;
        }
    </style>
</head>
<body class="bg-light">
    <form id="form1" runat="server">
        <div class="container py-5">
            <div class="row justify-content-center">
                <div class="col-12">
                    <div class="card dashboard-card">
                        <div class="card-header bg-primary text-white">
                            <h2 class="h4 mb-0">Dashboard</h2>
                        </div>
                        <div class="row justify-content-center">
                            <div class="col-md-8">
                                <div class="card p-4 shadow-sm">
                                    <h2 class="mb-4">Welcome, <asp:Label ID="lblUsername" runat="server" />!</h2>
                                    <hr />
                                    <asp:Panel ID="pnlAdmin" runat="server" Visible="false">
                                        <div class="d-flex flex-column gap-3 align-items-center">
                                            <asp:Button ID="btnEmployeeCRUD" runat="server" CssClass="btn btn-primary btn-lg w-100" Text="Go to User Management" OnClick="btnEmployeeCRUD_Click" />
                                            <asp:Button ID="btnVoucherEntry" runat="server" CssClass="btn btn-secondary btn-lg w-100" Text="Go to Voucher Entry" OnClick="btnVoucherEntry_Click" />
                                            <asp:Button ID="btnLogout" runat="server" CssClass="btn btn-danger btn-lg w-100" Text="Logout" OnClick="btnLogout_Click" />
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlUser" runat="server" Visible="false">
                                        <table class="table table-bordered">
                                            <tr><th>Account Name</th><td><asp:Label ID="lblAccountName" runat="server" /></td></tr>
                                            <tr><th>Password</th><td><asp:Label ID="lblPassword" runat="server" /></td></tr>
                                            <tr><th>Account Type</th><td><asp:Label ID="lblAccountType" runat="server" /></td></tr>
                                            <tr><th>Amount</th><td><asp:Label ID="lblAmount" runat="server" /></td></tr>
                                        </table>
                                        <asp:Panel ID="pnlVoucherInfo" runat="server" Visible="false">
                                            <h4>Voucher Available!</h4>
                                            <p><b>Voucher Type:</b> <asp:Label ID="lblVoucherType" runat="server" /></p>
                                            <p><b>Voucher Amount:</b> <asp:Label ID="lblVoucherAmount" runat="server" /></p>
                                            <asp:Button ID="btnUseVoucher" runat="server" CssClass="btn btn-success" Text="Use Voucher" OnClick="btnUseVoucher_Click" />
                                        </asp:Panel>
                                        <asp:Button ID="btnLogoutUser" runat="server" CssClass="btn btn-danger mt-3" Text="Logout" OnClick="btnLogout_Click" />
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    
    <!-- Bootstrap 5 Bundle with Popper -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        // Optionally: JS to auto-refresh or show voucher info dynamically
    </script>
</body>
</html>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CodeBehind/UserCRUD.aspx.cs" Inherits="UserCRUD" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>User Management</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-5">
            <div class="card">
                <div class="card-header bg-primary text-white">
                    <h4 class="mb-0">User Management</h4>
                </div>
                <div class="card-body">
                    <div class="row mb-3">
                        <div class="col-md-3">
                            <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control" placeholder="Account Name" />
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" placeholder="Password" TextMode="Password" />
                        </div>
                        <div class="col-md-3">
                            <asp:DropDownList ID="ddlAccountType" runat="server" CssClass="form-select">
                                <asp:ListItem Text="Select Type" Value="" />
                                <asp:ListItem Text="Savings" Value="Savings" />
                                <asp:ListItem Text="Current" Value="Current" />
                                <asp:ListItem Text="Admin" Value="Admin" />
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" placeholder="Amount" TextMode="Number" />
                        </div>
                        <div class="col-md-1">
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-success w-100" OnClick="btnSave_Click" />
                        </div>
                    </div>
                    <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="AccountId" OnRowCommand="gvUsers_RowCommand" OnRowEditing="gvUsers_RowEditing" OnRowUpdating="gvUsers_RowUpdating" OnRowCancelingEdit="gvUsers_RowCancelingEdit" OnRowDeleting="gvUsers_RowDeleting">
                        <Columns>
                            <asp:BoundField DataField="AccountId" HeaderText="ID" ReadOnly="true" />
                            <asp:BoundField DataField="AccountName" HeaderText="Account Name" />
                            <asp:BoundField DataField="Password" HeaderText="Password" />
                            <asp:BoundField DataField="AccountType" HeaderText="Account Type" />
                            <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:C}" />
                            <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CodeBehind/VoucherEntry.aspx.cs" Inherits="VoucherEntry" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Voucher Entry</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        .voucher-table th, .voucher-table td { vertical-align: middle; }
    </style>
</head>
<body>
    <form id="form1" runat="server" class="container mt-4">
        <h2>Voucher Entry</h2>
        <div class="row mb-3">
            <div class="col-md-3">
                <label for="ddlVoucherType" class="form-label">Voucher Type</label>
                <asp:DropDownList ID="ddlVoucherType" runat="server" CssClass="form-select" required="true">
                    <asp:ListItem Value="">--Select--</asp:ListItem>
                    <asp:ListItem Value="JV">Journal Voucher</asp:ListItem>
                    <asp:ListItem Value="PV">Payment Voucher</asp:ListItem>
                    <asp:ListItem Value="RV">Receipt Voucher</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-md-3">
                <label for="txtDate" class="form-label">Date</label>
                <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" TextMode="Date" required="true"></asp:TextBox>
            </div>
            <div class="col-md-3">
                <label for="txtReferenceNo" class="form-label">Reference No.</label>
                <asp:TextBox ID="txtReferenceNo" runat="server" CssClass="form-control" placeholder="Auto/Manual"></asp:TextBox>
            </div>
        </div>
        <div class="row mb-3">
            <div class="col-md-4">
                <label for="ddlAssignAccount" class="form-label">Assign Voucher To (Account)</label>
                <asp:DropDownList ID="ddlAssignAccount" runat="server" CssClass="form-select" />
            </div>
        </div>
        <h5>Voucher Entries</h5>
        <asp:HiddenField ID="hfVoucherEntries" runat="server" />
        <table class="table table-bordered voucher-table" id="entriesTable">
            <thead class="table-light">
                <tr>
                    <th>Account</th>
                    <th>Debit</th>
                    <th>Credit</th>
                    <th>Action</th>
                </tr>
            </thead>
            <tbody id="entriesBody">
                <!-- Dynamic rows will be added here by JS -->
            </tbody>
        </table>
        <button type="button" class="btn btn-outline-primary mb-3" onclick="addRow()">Add Entry</button>
        <asp:Button ID="btnSave" runat="server" Text="Save Voucher" CssClass="btn btn-success mb-3" OnClick="btnSave_Click" />
        <asp:Label ID="lblMessage" runat="server" CssClass="text-danger" />
    </form>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        var accounts = [];
        // This will be replaced with actual account data from server
        // Use AJAX in code-behind to inject the account list as JSON

        function renderAccountDropdown(selectedId) {
            var html = '<select class="form-select account-select">';
            html += '<option value="">--Select--</option>';
            for (var i = 0; i < accounts.length; i++) {
                var selected = accounts[i].Id == selectedId ? 'selected' : '';
                html += '<option value="' + accounts[i].Id + '" ' + selected + '>' + accounts[i].Name + '</option>';
            }
            html += '</select>';
            return html;
        }
        function addRow(accountId, debit, credit) {
            var table = document.getElementById('entriesBody');
            var row = document.createElement('tr');
            row.innerHTML =
                '<td>' + renderAccountDropdown(accountId) + '</td>' +
                '<td><input type="number" class="form-control debit-input" value="' + (debit || '') + '" min="0" step="0.01"></td>' +
                '<td><input type="number" class="form-control credit-input" value="' + (credit || '') + '" min="0" step="0.01"></td>' +
                '<td><button type="button" class="btn btn-danger btn-sm" onclick="removeRow(this)">Remove</button></td>';
            table.appendChild(row);
        }
        function removeRow(btn) {
            btn.closest('tr').remove();
        }
        function getVoucherEntries() {
            var rows = document.querySelectorAll('#entriesBody tr');
            var entries = [];
            rows.forEach(function(row) {
                var accountId = row.querySelector('.account-select').value;
                var debit = row.querySelector('.debit-input').value;
                var credit = row.querySelector('.credit-input').value;
                entries.push({ AccountId: accountId, Debit: debit, Credit: credit });
            });
            return entries;
        }
        document.getElementById('<%= btnSave.ClientID %>').onclick = function() {
            document.getElementById('<%= hfVoucherEntries.ClientID %>').value = JSON.stringify(getVoucherEntries());
        };
    </script>
</body>
</html>

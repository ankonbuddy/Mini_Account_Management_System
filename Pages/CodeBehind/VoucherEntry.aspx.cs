using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Web.Script.Serialization;
using System.Web.UI;

public partial class VoucherEntry : System.Web.UI.Page
{
    // Updated Account class for new schema
    public class Account
    {
        public int AccountId { get; set; }
        public string Password { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public decimal Amount { get; set; }
    }

    protected List<Account> Accounts = new List<Account>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadAccounts();
            InjectAccountsToJs();
            GenerateReferenceNo();
        }
    }

    private void LoadAccounts()
    {
        // Only ChartOfAccounts should be selectable for vouchers (not Users or Employees)
        string connStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
        using (SQLiteConnection conn = new SQLiteConnection(connStr))
        using (SQLiteCommand cmd = new SQLiteCommand("SELECT AccountId, Password, AccountName, AccountType, Amount FROM ChartOfAccounts", conn))
        {
            conn.Open();
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader["AccountId"] != DBNull.Value && reader["AccountName"] != DBNull.Value)
                    {
                        Accounts.Add(new Account
                        {
                            AccountId = Convert.ToInt32(reader["AccountId"]),
                            Password = reader["Password"].ToString(),
                            AccountName = reader["AccountName"].ToString(),
                            AccountType = reader["AccountType"].ToString(),
                            Amount = Convert.ToDecimal(reader["Amount"])
                        });
                    }
                }
            }
        }
    }

    private void InjectAccountsToJs()
    {
        var serializer = new JavaScriptSerializer();
        string json = serializer.Serialize(Accounts);
        string script = string.Format("var accounts = {0};", json);
        ClientScript.RegisterStartupScript(this.GetType(), "accountsJs", script, true);
    }

    private void GenerateReferenceNo()
    {
        // Example: Auto-generate based on voucher type and date
        string prefix = ddlVoucherType.SelectedValue;
        if (string.IsNullOrEmpty(prefix)) prefix = "JV";
        txtReferenceNo.Text = prefix + "-" + DateTime.Now.ToString("yyyyMMddHHmmss");
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        if (string.IsNullOrEmpty(ddlVoucherType.SelectedValue) || string.IsNullOrEmpty(txtDate.Text))
        {
            lblMessage.Text = "Voucher Type and Date are required.";
            return;
        }
        var entries = new JavaScriptSerializer().Deserialize<List<VoucherEntryRow>>(hfVoucherEntries.Value);
        if (entries == null || entries.Count == 0)
        {
            lblMessage.Text = "Please add at least one voucher entry.";
            return;
        }
        string connStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
        using (SQLiteConnection conn = new SQLiteConnection(connStr))
        {
            conn.Open();
            using (var transaction = conn.BeginTransaction())
            {
                // Insert into Vouchers
                string insertVoucherSql = "INSERT INTO Vouchers (VoucherType, Date, ReferenceNo, CreatedBy, CreatedAt) VALUES (@VoucherType, @Date, @ReferenceNo, @CreatedBy, @CreatedAt); SELECT last_insert_rowid();";
                long voucherId;
                using (SQLiteCommand cmd = new SQLiteCommand(insertVoucherSql, conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@VoucherType", ddlVoucherType.SelectedValue);
                    cmd.Parameters.AddWithValue("@Date", DateTime.Parse(txtDate.Text));
                    cmd.Parameters.AddWithValue("@ReferenceNo", txtReferenceNo.Text);
                    cmd.Parameters.AddWithValue("@CreatedBy", User.Identity.Name ?? "system");
                    cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                    voucherId = (long)cmd.ExecuteScalar();
                }
                // Insert into VoucherDetails
                string insertDetailSql = "INSERT INTO VoucherDetails (VoucherId, AccountId, Amount, IsDebit) VALUES (@VoucherId, @AccountId, @Amount, @IsDebit);";
                foreach (var entry in entries)
                {
                    if (!string.IsNullOrEmpty(entry.AccountId) && (entry.Debit != "" || entry.Credit != ""))
                    {
                        decimal amount = 0;
                        bool isDebit = false;
                        if (!string.IsNullOrEmpty(entry.Debit) && entry.Debit != "0")
                        {
                            amount = Convert.ToDecimal(entry.Debit);
                            isDebit = true;
                        }
                        else if (!string.IsNullOrEmpty(entry.Credit) && entry.Credit != "0")
                        {
                            amount = Convert.ToDecimal(entry.Credit);
                            isDebit = false;
                        }
                        using (SQLiteCommand detailCmd = new SQLiteCommand(insertDetailSql, conn, transaction))
                        {
                            detailCmd.Parameters.AddWithValue("@VoucherId", voucherId);
                            detailCmd.Parameters.AddWithValue("@AccountId", Convert.ToInt32(entry.AccountId));
                            detailCmd.Parameters.AddWithValue("@Amount", amount);
                            detailCmd.Parameters.AddWithValue("@IsDebit", isDebit ? 1 : 0);
                            detailCmd.ExecuteNonQuery();
                        }
                    }
                }
                transaction.Commit();
            }
        }
        lblMessage.CssClass = "text-success";
        lblMessage.Text = "Voucher saved successfully.";
        // Optionally clear form or redirect
    }


    public class VoucherEntryRow
    {
        public string AccountId { get; set; }
        public string Debit { get; set; }
        public string Credit { get; set; }
    }
}

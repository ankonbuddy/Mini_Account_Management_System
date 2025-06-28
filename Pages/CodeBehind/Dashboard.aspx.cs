using System;
using System.Web.UI;

public partial class Dashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsUserLoggedIn())
        {
            Response.Redirect("Login.aspx");
            return;
        }
        if (!IsPostBack)
        {
            bool isAdmin = Session["IsAdmin"] != null && (bool)Session["IsAdmin"];
            pnlAdmin.Visible = isAdmin;
            pnlUser.Visible = !isAdmin;
            lblUsername.Text = Session["Username"].ToString();

            if (!isAdmin)
            {
                // Load user info
                string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
                using (var conn = new System.Data.SQLite.SQLiteConnection(connStr))
                {
                    conn.Open();
                    string q = "SELECT Password, AccountName, AccountType, Amount FROM ChartOfAccounts WHERE AccountId=@id";
                    using (var cmd = new System.Data.SQLite.SQLiteCommand(q, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", Session["AccountId"]);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lblAccountName.Text = reader["AccountName"].ToString();
                                lblPassword.Text = reader["Password"].ToString();
                                lblAccountType.Text = reader["AccountType"].ToString();
                                lblAmount.Text = reader["Amount"].ToString();
                            }
                        }
                    }
                    // Check for voucher assigned to this user
                    string vq = "SELECT v.VoucherType, d.Amount FROM Vouchers v JOIN VoucherDetails d ON v.Id = d.VoucherId WHERE d.AccountId=@id LIMIT 1";
                    using (var vcmd = new System.Data.SQLite.SQLiteCommand(vq, conn))
                    {
                        vcmd.Parameters.AddWithValue("@id", Session["AccountId"]);
                        using (var vreader = vcmd.ExecuteReader())
                        {
                            if (vreader.Read())
                            {
                                pnlVoucherInfo.Visible = true;
                                lblVoucherType.Text = vreader["VoucherType"].ToString();
                                lblVoucherAmount.Text = vreader["Amount"].ToString();
                            }
                            else
                            {
                                pnlVoucherInfo.Visible = false;
                            }
                        }
                    }
                }
            }
        }
    }

    protected void btnEmployeeCRUD_Click(object sender, EventArgs e)
    {
        if (!IsUserLoggedIn())
        {
            Response.Redirect("Login.aspx");
            return;
        }
        Response.Redirect("userCRUD.aspx");
    }

    // protected void btnVoucherEntry_Click(object sender, EventArgs e)
    // {
    //     if (!IsUserLoggedIn())
    //     {
    //         Response.Redirect("Login.aspx");
    //         return;
    //     }
    //     Response.Redirect("VoucherEntry.aspx");
    // }

    // protected void btnLogout_Click(object sender, EventArgs e)
    // {
    //     if (!IsUserLoggedIn())
    //     {
    //         Response.Redirect("Login.aspx");
    //         return;
    //     }
    //     Response.Redirect("VoucherEntry.aspx");
    // }

    protected void btnVoucherEntry_Click(object sender, EventArgs e)
    {
        if (!IsUserLoggedIn())
        {
            Response.Redirect("Login.aspx");
            return;
        }
        Response.Redirect("VoucherEntry.aspx");
    }

    protected void btnUseVoucher_Click(object sender, EventArgs e)
    {
        // TODO: Implement logic to apply voucher to user account, update balance, and remove voucher.
        // For now, just refresh the page as a placeholder.
        Response.Redirect(Request.RawUrl);
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {

        Session.Clear();  

        Session.Abandon();
        if (Response.Cookies["ASP.NET_SessionId"] != null)
        {
            Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
            Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
        }
        Response.Redirect("Login.aspx");
    }
    
    private bool IsUserLoggedIn()
    {
        return Session["Username"] != null && !string.IsNullOrEmpty(Session["Username"].ToString());
    }
}
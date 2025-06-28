using System;
using System.Configuration;
using System.Data.SQLite;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMessage.Text = "";
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string username = txtUsername.Text.Trim();
        string password = txtPassword.Text.Trim();
        string connStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;

        using (SQLiteConnection conn = new SQLiteConnection(connStr))
        {
            try
            {
                conn.Open();
                // Try admin login first
                string adminQuery = "SELECT COUNT(*) FROM Admin WHERE Username = @username AND Password = @password";
                using (SQLiteCommand cmd = new SQLiteCommand(adminQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count == 1)
                    {
                        Session["Username"] = username;
                        Session["IsAdmin"] = true;
                        Response.Redirect("Dashboard.aspx");
                        return;
                    }
                }
                // Try user (ChartOfAccount) login
                string userQuery = "SELECT AccountId, AccountName, AccountType FROM ChartOfAccounts WHERE AccountName = @username AND Password = @password";
                using (SQLiteCommand userCmd = new SQLiteCommand(userQuery, conn))
                {
                    userCmd.Parameters.AddWithValue("@username", username);
                    userCmd.Parameters.AddWithValue("@password", password);
                    using (var reader = userCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Session["Username"] = reader["AccountName"].ToString();
                            Session["AccountId"] = reader["AccountId"].ToString();
                            Session["AccountType"] = reader["AccountType"].ToString();
                            Session["IsAdmin"] = false;
                            Response.Redirect("Dashboard.aspx");
                            return;
                        }
                    }
                }
                lblMessage.Text = "Invalid credentials.";
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
            }
        }
    }
}

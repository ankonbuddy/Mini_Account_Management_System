using System;
using System.Data;
using System.Configuration;
using System.Data.SQLite;
using System.Web.UI.WebControls;

public partial class UserCRUD : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindUsers();
        }
    }

    protected void BindUsers()
    {
        string connStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
        DataTable dt = new DataTable();
        
        try
        {
            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                conn.Open();
                string query = "SELECT AccountId, AccountName, Password, AccountType, Amount FROM ChartOfAccounts";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                using (SQLiteDataAdapter da = new SQLiteDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
        }
        catch (Exception ex)
        {
            // Log the error details for debugging
            System.Diagnostics.Debug.WriteLine(string.Format("Database connection error: {0}", ex.Message));
            System.Diagnostics.Debug.WriteLine(string.Format("Connection string: {0}", connStr));
            System.Diagnostics.Debug.WriteLine(string.Format("Stack trace: {0}", ex.StackTrace));
            throw; // Re-throw the exception to maintain original behavior
        }
        
        gvUsers.DataSource = dt;
        gvUsers.DataBind();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string name = txtAccountName.Text.Trim();
        string password = txtPassword.Text.Trim();
        string type = ddlAccountType.SelectedValue;
        decimal amount = 0;
        decimal.TryParse(txtAmount.Text, out amount);
        string connStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
        // Always use using blocks for connections and commands to avoid DB lock issues
        using (SQLiteConnection conn = new SQLiteConnection(connStr))
        {
            conn.Open();
            string query = "INSERT INTO ChartOfAccounts (AccountName, Password, AccountType, Amount) VALUES (@name, @password, @type, @amount)";
            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.ExecuteNonQuery();
            }
        }
        ClearForm();
        BindUsers();
    }

    protected void gvUsers_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvUsers.EditIndex = e.NewEditIndex;
        BindUsers();
    }

    protected void gvUsers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvUsers.EditIndex = -1;
        BindUsers();
    }

    protected void gvUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int accountId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);
        GridViewRow row = gvUsers.Rows[e.RowIndex];
        string name = ((TextBox)row.Cells[1].Controls[0]).Text.Trim();
        string password = ((TextBox)row.Cells[2].Controls[0]).Text.Trim();
        string type = ((TextBox)row.Cells[3].Controls[0]).Text.Trim();
        decimal amount = Convert.ToDecimal(((TextBox)row.Cells[4].Controls[0]).Text.Trim());
        string connStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
        using (SQLiteConnection conn = new SQLiteConnection(connStr))
        {
            conn.Open();
            string query = "UPDATE ChartOfAccounts SET AccountName=@name, Password=@password, AccountType=@type, Amount=@amount WHERE AccountId=@id";
            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@id", accountId);
                cmd.ExecuteNonQuery();
            }
        }
        gvUsers.EditIndex = -1;
        BindUsers();
    }

    protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int accountId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);
        string connStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
        using (SQLiteConnection conn = new SQLiteConnection(connStr))
        {
            conn.Open();
            string query = "DELETE FROM ChartOfAccounts WHERE AccountId=@id";
            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", accountId);
                cmd.ExecuteNonQuery();
            }
        }
        BindUsers();
    }

    protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        // Not used but required for compatibility
    }

    private void ClearForm()
    {
        txtAccountName.Text = "";
        txtPassword.Text = "";
        ddlAccountType.SelectedIndex = 0;
        txtAmount.Text = "";
    }
}

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Pass_Storer
{
    public partial class frmRegister : Form
    {
        public frmRegister()
        {
            InitializeComponent();
        }

        //link database (change AttachDbFilename to path of pwds.mdf on system after download)
        private static string cstring = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\myf11\source\repos\Pass Storer\Pass Storer\Database\pwds.mdf;Integrated Security=True";


        //On clicking register button
        private void button1_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "" || txtMasterPassword.Text == "" || txtConPassword.Text == "")
            {
                MessageBox.Show("Username or Password field is empty.", "Registration failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtMasterPassword.Text == txtConPassword.Text)
            {
                using (SqlConnection cs = new SqlConnection(cstring))
                {
                    try
                    {
                        cs.Open();
                        SqlCommand cmd = new SqlCommand("UserAdd", cs);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                        string cypher = Base64.Base64Encode(txtMasterPassword.Text.Trim());
                        cmd.Parameters.AddWithValue("@masterpass", cypher);
                        cmd.ExecuteNonQuery();
                        cs.Close();

                        txtUsername.Text = "";
                        txtMasterPassword.Text = "";
                        txtConPassword.Text = "";

                        MessageBox.Show("Your account has been successfully created.", "Registration Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        new frmLogin().Show();
                        this.Hide();
                    }
                    catch (System.Data.SqlClient.SqlException)
                    {
                        MessageBox.Show("Account with this Username already exists.", "Registration failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtMasterPassword.Text = "";
                        txtConPassword.Text = "";
                        txtUsername.Focus();
                    }
                }
            }
            else
            {
                MessageBox.Show("Passwords do not match. Please re-enter.", "Registration failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMasterPassword.Text = "";
                txtConPassword.Text = "";
                txtMasterPassword.Focus();
            }
        }

        //On clicking show password checkbox
        private void checkBxShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBxShowPass.Checked)
            {
                txtMasterPassword.PasswordChar = '\0';
                txtConPassword.PasswordChar = '\0';
            }
            else
            {
                txtMasterPassword.PasswordChar = '*';
                txtConPassword.PasswordChar = '*';
            }
        }

        //On clicking already have an account
        private void label6_Click(object sender, EventArgs e)
        {
            new frmLogin().Show();
            this.Hide();
        }
    }
}
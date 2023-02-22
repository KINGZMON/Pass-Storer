using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace Pass_Storer
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        //link database (change AttachDbFilename to path of pwds.mdf on your system after download)
        private static string cstring = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\myf11\source\repos\Pass Storer\Pass Storer\Database\pwds.mdf;Integrated Security=True";

        //On clicking Login
        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection cs = new SqlConnection(cstring))
            {
                cs.Open();
                string cypher = Base64.Base64Encode(txtMasterPassword.Text.Trim());
                string login = "SELECT * FROM masterPass WHERE username= '" + txtUsername.Text + "' and masterpass= '" + cypher + "'";
                SqlCommand cmd = new SqlCommand(login, cs);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read() == true)
                {
                    new dashboard().Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid Username or Password. Please Try Again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtUsername.Text = "";
                    txtMasterPassword.Text = "";
                    txtUsername.Focus();
                }
                cs.Close();
            }
        }

        //On clicking show password checkbox
        private void checkBxShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBxShowPass.Checked)
            {
                txtMasterPassword.PasswordChar = '\0';
            }
            else
            {
                txtMasterPassword.PasswordChar = '*';
            }
        }

        //On clicking create account
        private void label6_Click(object sender, EventArgs e)
        {
            new frmRegister().Show();
            this.Hide();
        }
    }
}
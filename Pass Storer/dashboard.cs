using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace Pass_Storer
{
    public partial class dashboard : Form
    {
        public dashboard()
        {
            InitializeComponent();
        }

        //link database (change AttachDbFilename to path of pwds.mdf on system after download)
        private static string cstring = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\myf11\source\repos\Pass Storer\Pass Storer\Database\pwds.mdf;Integrated Security=True";

        //On clicking store button
        private void button1_Click(object sender, EventArgs e)
        {
            if(txtServiceName.Text =="" || txtServicePassword.Text=="" || txtConSerPassword.Text == "")
            {
                MessageBox.Show("All fields must be filled", "Storing failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtServicePassword.Text == txtConSerPassword.Text)
            {
                using (SqlConnection cs = new SqlConnection(cstring))
                {
                    try
                    {
                        cs.Open();
                        SqlCommand cmd = new SqlCommand("ServiceAdd", cs);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@service", txtServiceName.Text.Trim());
                        string cypher = Base64.Base64Encode(txtServicePassword.Text.Trim());
                        cmd.Parameters.AddWithValue("@pass", cypher);
                        cmd.ExecuteNonQuery();
                        cs.Close();

                        txtServiceName.Text = "";
                        txtServicePassword.Text = "";
                        txtConSerPassword.Text = "";

                        MessageBox.Show("Password for service successfully stored", "Storing Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (System.Data.SqlClient.SqlException)
                    {
                        MessageBox.Show("Service already exists. Delete from retriever to add service with new password.", "Storing failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtServicePassword.Text = "";
                        txtConSerPassword.Text = "";
                        txtServiceName.Focus();
                    }
                }
            }
            else
            {
                MessageBox.Show("Passwords do not match. Please re-enter", "Storing failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtServicePassword.Text = "";
                txtConSerPassword.Text = "";
                txtServicePassword.Focus();
            }
        }

        //On clicking retrieve password
        private void label6_Click(object sender, EventArgs e)
        {
            new retriever().Show();
            this.Hide();
        }

        //On clicking logout icon
        private void button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        //On clicking show password checkbox
        private void checkBxShowPass_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBxShowPass.Checked)
            {
                txtServicePassword.PasswordChar = '\0';
                txtConSerPassword.PasswordChar = '\0';
            }
            else
            {
                txtServicePassword.PasswordChar = '*';
                txtConSerPassword.PasswordChar = '*';
            }
        }
    }
}

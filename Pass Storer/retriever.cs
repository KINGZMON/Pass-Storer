using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pass_Storer
{
    public partial class retriever : Form
    {
        public retriever()
        {
            InitializeComponent();
            RefreshData();
        }

        //link database (change AttachDbFilename to path of pwds.mdf on system after download)
        private static string cstring = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\myf11\source\repos\Pass Storer\Pass Storer\Database\pwds.mdf;Integrated Security=True";

        //function to bind data to combo box
        public void RefreshData()
        {
            DataRow dr;
            using (SqlConnection cs = new SqlConnection(cstring))
            {
                cs.Open();
                SqlCommand cmd = new SqlCommand("select * from pwStorer", cs);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dr = dt.NewRow();
                dr.ItemArray = new object[] { "--Select Service--" };
                dt.Rows.InsertAt(dr, 0);

                //Bind the values
                comboBox1.DisplayMember = "service";
                comboBox1.DataSource = dt;

                cs.Close();
            }
        }

        //On clicking get password
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                txtDisplayPass.Text = "Select a Service!";
            }
            else
            {
                string chosenService = comboBox1.Text;
                using (SqlConnection cs = new SqlConnection(cstring))
                {
                    cs.Open();
                    SqlCommand cmd = new SqlCommand("select pass from pwStorer where service='" + chosenService + "'", cs);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        string encText = (string)dt.Rows[0][0];
                        txtDisplayPass.Text = Base64.Base64Decode(encText);
                    }
                }
            }
        }

        //On clicking delete entry
        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                txtDisplayPass.Text = "Select a Service!";
            }
            else
            {
                txtDisplayPass.Text = "";
                string chosenService = comboBox1.Text;
                using (SqlConnection cs = new SqlConnection(cstring))
                {
                    cs.Open();
                    SqlCommand cmd = new SqlCommand("delete from pwStorer where service='" + chosenService + "'", cs);
                    cmd.ExecuteNonQuery();
                    cs.Close();

                    MessageBox.Show("Record deleted successfully", "Deletion Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtDisplayPass.Text = "";

                    //Call RefreshData Function to bind current data after deleting record in database
                    RefreshData();
                }
            }
        }

        //on clicking back to pass storer
        private void label6_Click(object sender, EventArgs e)
        {
            new dashboard().Show();
            this.Hide();
        }
    }
}

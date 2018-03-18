using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;  

namespace Stock
{
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();
        }
        private void Products_Load(object sender, EventArgs e)
        {
            
            comboBox1.SelectedIndex = 0;
            LoadData();

            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=Stock;Integrated Security=True");
            con.Open();
            int status = 0;
            if (comboBox1.SelectedIndex == 0)
            {
                status = 1;
            }
            else 
            {
                status = 0;
            }
            var sqlQuery = "";
            if(IfProductExists(con,textBox1.Text))
            {
                sqlQuery = @"UPDATE [Stock].[dbo].[Products] SET [ProductName] = '" + textBox2.Text + "',[ProductStatus] = '" + status + "' WHERE [ProductCode]='"+textBox1.Text+"'";
            }else 
            {
                sqlQuery = @"INSERT INTO [Stock].[dbo].[Products] ([ProductCode],[ProductName],[ProductStatus]) VALUES
                             ('" + textBox1.Text + "','" + textBox2.Text + "','" + status + "')";
            }

            SqlCommand cmd = new SqlCommand(sqlQuery, con);
            cmd.ExecuteNonQuery();
            LoadData();


            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text=="")
            {
                MessageBox.Show("Record not exist...!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else { 
            SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=Stock;Integrated Security=True");
            con.Open();
            SqlCommand cmd = new SqlCommand(@"DELETE FROM [Stock].[dbo].[Products] WHERE ProductCode ='"+textBox1.Text+"'",con);
            cmd.ExecuteNonQuery();
            con.Close();
            LoadData();
            }
        }

        private bool IfProductExists(SqlConnection con, String code)
        {
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM [Stock].[dbo].[Products] WHERE [ProductCode]='"+code+"'", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                return false;
            }
            return true;
        }

        public void LoadData() 
        {
            SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=Stock;Integrated Security=True");
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM [Stock].[dbo].[Products]", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.Rows.Clear();


            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = item["ProductCode"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["ProductName"].ToString();
                if (item["ProductStatus"].ToString().Equals("1"))
                {
                    dataGridView1.Rows[n].Cells[2].Value = "Active";
                }
                else
                {
                    dataGridView1.Rows[n].Cells[2].Value = "Desactive";
                }


            }
            con.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            
            if (dataGridView1.SelectedRows[0].Cells[2].Value.ToString() == "Active")
            {
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                comboBox1.SelectedIndex = 1;
            }
        }
    }
}

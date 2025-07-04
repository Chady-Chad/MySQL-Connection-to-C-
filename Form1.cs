using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SampleMysql_C_Connection
{
    public partial class Form1 : Form
    {
        string connectionString = "server=localhost;user=root;password=Cameng02atgmaledotcom;database=project_3";

        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }

        // INSERT
        private void button1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO employee (EMP_NAME, SUR_NAME, Address) VALUES (@name, @surname, @address)";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@name", textBox1.Text);
                    command.Parameters.AddWithValue("@surname", textBox2.Text);
                    command.Parameters.AddWithValue("@address", textBox3.Text);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Data Inserted Successfully");
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Insert Error: " + ex.Message);
                }
            }
        }

        // UPDATE
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int empId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["empId"].Value); // Make sure "empId" matches the column name in your database
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string query = "UPDATE employee SET EMP_NAME = @name, SUR_NAME = @surname, Address = @address WHERE empId = @empId";
                        MySqlCommand command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@name", textBox1.Text);
                        command.Parameters.AddWithValue("@surname", textBox2.Text);
                        command.Parameters.AddWithValue("@address", textBox3.Text);
                        command.Parameters.AddWithValue("@empId", empId);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Data Updated Successfully");
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Update Error: " + ex.Message);
                    }
                }
            }
        }

        // DELETE
        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int empId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["empId"].Value);
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string query = "DELETE FROM employee WHERE empId = @empId";
                        MySqlCommand command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@empId", empId);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Data Deleted Successfully");
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Delete Error: " + ex.Message);
                    }
                }
            }
        }

        // LOAD DATA INTO DATAGRIDVIEW
        private void button3_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM employee";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                    DataTable Table = new DataTable();
                    adapter.Fill(Table);
                    dataGridView1.DataSource = Table;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Load Data Error: " + ex.Message);
                }
            }
        }

        // SELECT ROW (POPULATE TEXTBOXES)
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                textBox1.Text = row.Cells["EMP_NAME"].Value.ToString();
                textBox2.Text = row.Cells["SUR_NAME"].Value.ToString();
                textBox3.Text = row.Cells["Address"].Value.ToString();
            }
        }

        // Optional: Remove if unused
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete all data and reset IDs?",
                "Confirm Reset",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        // Delete all data
                        string deleteQuery = "DELETE FROM employee";
                        MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, connection);
                        deleteCmd.ExecuteNonQuery();

                        // Reset auto-increment to 1
                        string resetQuery = "ALTER TABLE employee AUTO_INCREMENT = 1";
                        MySqlCommand resetCmd = new MySqlCommand(resetQuery, connection);
                        resetCmd.ExecuteNonQuery();

                        MessageBox.Show("All data cleared and empId reset to 1.");
                        LoadData();

                        // Clear textboxes too (optional)
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Reset Error: " + ex.Message);
                    }
                }
            }
        }

    }
}

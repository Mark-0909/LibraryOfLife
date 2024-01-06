using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp1
{
    public partial class LoginPage : Form
    {
        private Thread th;

        public LoginPage()
        {
            InitializeComponent();
            button1.Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
            label1.BackColor = System.Drawing.Color.Transparent;
            label2.BackColor = System.Drawing.Color.Transparent;
            label3.BackColor = System.Drawing.Color.Transparent;
            pictureBox1.BackColor = System.Drawing.Color.Transparent;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            // Replace the connection string with your actual database connection string
            string connectionString = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Replace "admininfo" with your actual table name
                    string query = $"SELECT * FROM admininfo WHERE Admin_Username = '{username}' AND Admin_Password = '{password}'";
                    MySqlCommand command = new MySqlCommand(query, connection);

                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        MessageBox.Show("Login Successful!");
                        this.Hide();
                        Library form2 = new Library();
                        form2.Show();
                        th = new Thread(openNewForm);
                        th.SetApartmentState(ApartmentState.STA);
                        th.Start();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void openNewForm()
        {
            Application.Run(new Library());
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // Additional actions for the "Forgot Password" link
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Additional actions when the username textbox changes
        }
        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}
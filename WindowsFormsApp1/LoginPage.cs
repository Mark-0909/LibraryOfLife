using System;

using System.Windows.Forms;

using MySql.Data.MySqlClient;



namespace WindowsFormsApp1
{
    public partial class LoginPage : Form
    {
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
            login();
        }
        public void login()
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            string connectionString = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT * FROM admininfo WHERE Admin_Username = @Username AND Admin_Password = @Password";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        MessageBox.Show("Login Successful!");
                        this.Hide();
                        Library form2 = new Library();
                        form2.FormClosed += (s, args) => this.Close(); // Close LoginPage when Library form is closed
                        form2.Show();
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
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
            // Additional actions on form load
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            // Additional actions when the username textbox changes
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                textBox2.Focus();
            } else if (e.KeyCode == Keys.Enter)
            {
                login();
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                login();
            }
        }
    }
}

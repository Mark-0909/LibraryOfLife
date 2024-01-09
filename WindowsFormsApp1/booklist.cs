using System;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp1
{
    public partial class booklist : UserControl
    {
        private bool isPopUpFormOpen = false;
        public booklist()
        {
            InitializeComponent();
            DisplayBooks();

        }
        public FlowLayoutPanel FlowLayoutPanel1
        {
            get { return flowLayoutPanel1; }
            set { flowLayoutPanel1 = value; }
        }

        public void DisplayBooks()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();
                    string query = "select * from books";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    MySqlDataReader reader = cmdDatabase.ExecuteReader();

                    while (reader.Read())
                    {

                        books bookControl = new books(
                            reader["Book_Name"].ToString(),
                            reader["Book_Id"].ToString(),
                            reader["Book_Author"].ToString(),
                            reader["Book_Location"].ToString(),
                            reader["Book_Stocks"].ToString(),
                            (byte[])reader["Book_Image"]
                        );


                        flowLayoutPanel1.Controls.Add(bookControl);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }

            }
        }
        private void OpenAddingForm(object sender, EventArgs e)
        {

            if (!isPopUpFormOpen)
            {
                // Open your pop-up form here
                editBook popUpForm = new editBook();

                // Disable the main form
                this.FindForm().Enabled = false;

                isPopUpFormOpen = true;
                popUpForm.TextBox1.Enabled = true;
                popUpForm.TextBox2.Enabled = true;
                popUpForm.TextBox3.Enabled = true;
                popUpForm.TextBox4.Enabled = true;
                popUpForm.ComboBox1.Enabled = true;
                popUpForm.PictureBox1.Enabled = true;
                popUpForm.Label1.Text = "ADD BOOK";
                popUpForm.Button1.Visible = false;
                popUpForm.Button5.BringToFront();
                popUpForm.Button3.Visible = false;




                // Subscribe to the FormClosed event of the pop-up form
                popUpForm.FormClosed += (s, args) =>
                {
                    // Enable the main form when the pop-up form is closed
                    this.FindForm().Enabled = true;
                    isPopUpFormOpen = false;
                    refreshBooks();
                    //flowLayoutPanel1.Controls.Add(bookControl);
                };

                popUpForm.ShowDialog();
            }
        }



        private void button3_Click(object sender, EventArgs e)
        {
            OpenAddingForm(sender, e);
        }
        public void refreshBooks()
        {
            flowLayoutPanel1.Controls.Clear();
            DisplayBooks();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            searchBooks();
        }

        public void searchBooks()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";
            string searchTerm = textBox1.Text.Trim();
            
            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();

                    // Move the parameter addition before executing the query
                    string query = "SELECT * FROM books WHERE Book_Name LIKE @SearchTerm OR Book_Id LIKE @SearchTerm OR Book_Author LIKE @SearchTerm";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    cmdDatabase.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");

                    // Execute the query after adding parameters
                    MySqlDataReader reader = cmdDatabase.ExecuteReader();

                    // Clear controls before adding new ones
                    flowLayoutPanel1.Controls.Clear();

                    while (reader.Read())
                    {
                        books bookControl = new books(
                            reader["Book_Name"].ToString(),
                            reader["Book_Id"].ToString(),
                            reader["Book_Author"].ToString(),
                            reader["Book_Location"].ToString(),
                            reader["Book_Stocks"].ToString(),
                            (byte[])reader["Book_Image"]
                        );

                        flowLayoutPanel1.Controls.Add(bookControl);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            searchBookGenre();
        }
        public void searchBookGenre()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";
            string searchTerm = comboBox1.Text.Trim();
            
            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();

                    // Move the parameter addition before executing the query
                    string query = "SELECT * FROM books WHERE Book_Genre LIKE @SearchTerm";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    cmdDatabase.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");

                    // Execute the query after adding parameters
                    MySqlDataReader reader = cmdDatabase.ExecuteReader();

                    // Clear controls before adding new ones
                    flowLayoutPanel1.Controls.Clear();

                    while (reader.Read())
                    {
                        books bookControl = new books(
                            reader["Book_Name"].ToString(),
                            reader["Book_Id"].ToString(),
                            reader["Book_Author"].ToString(),
                            reader["Book_Location"].ToString(),
                            reader["Book_Stocks"].ToString(),
                            (byte[])reader["Book_Image"]
                        );

                        flowLayoutPanel1.Controls.Add(bookControl);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

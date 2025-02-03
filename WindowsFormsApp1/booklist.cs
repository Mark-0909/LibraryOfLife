using System;
using System.Data;
using System.Drawing;
using System.Reflection.Emit;
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
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            SetItemsGenreComboBox();

            
        }
        

        

        

        

        


        private void booklist_Load(object sender, EventArgs e)
        {
            
        }
        public void SetItemsGenreComboBox()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();

                    // Clear existing items before adding new ones
                    comboBox1.Items.Clear();

                    // Add a placeholder item
                    comboBox1.Items.Add("Select a genre");

                    string query = "SELECT Genre FROM genre_list";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    MySqlDataReader reader = cmdDatabase.ExecuteReader();

                    while (reader.Read())
                    {
                        string genreName = reader["Genre"].ToString();
                        comboBox1.Items.Add(genreName);
                    }

                    // Set the default selection to the placeholder
                    comboBox1.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
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
                            reader["Image_Path"].ToString(),
                            reader["Status"].ToString()

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
                popUpForm.ComboBox2.Enabled = true;
                popUpForm.TextBox4.Enabled = true;
                popUpForm.ComboBox1.Enabled = true;
                popUpForm.PictureBox1.Enabled = true;
                popUpForm.Label1.Text = "ADD BOOK";
                
                popUpForm.Button5.BringToFront();
                




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

        public void clearBookIfNotneeded()
        {
            flowLayoutPanel1.Controls.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenAddingForm(sender, e);
        }
        public void refreshBooks()
        {
            flowLayoutPanel1.Controls.Clear();
            DisplayBooks();
            SetItemsGenreComboBox();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (textBox1.Text == "Search Book")
            {
                MessageBox.Show("Please enter a valid search term.", "Empty Search", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                searchBooks();
                
                label1.Text = $"Search for: {textBox1.Text}";
                textBox1.Text = "Search Book";
                textBox1.ForeColor = Color.Silver;
            }
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
                    string query = "SELECT * FROM books WHERE Book_Name LIKE @SearchTerm OR Book_Id LIKE @SearchTerm OR Book_Author LIKE @SearchTerm OR Status LIKE @SearchTerm OR Book_Stocks LIKE @SearchTerm";
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
                            reader["Image_Path"].ToString(),
                            reader["Status"].ToString()
                        ) ;

                        flowLayoutPanel1.Controls.Add(bookControl);
                    }
                    reader.Close();
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
            comboBox1.SelectedIndex = 0;
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
                            reader["Image_Path"].ToString(),
                            reader["Status"].ToString()
                        );
                        label1.Text = $"Search for: {comboBox1.Text}";
                        flowLayoutPanel1.Controls.Add(bookControl);
                        
                    }
                    reader.Close();
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

        private void button4_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            DisplayBooks();
            label1.Text = "All Books:";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Search Book")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Search Book";
                textBox1.ForeColor = Color.Silver;
            }
        }

        private void textBox1_KeyDown_1(object sender, KeyEventArgs e)
        {
            // Check if Enter key is pressed
            if (e.KeyCode == Keys.Enter)
            {
                searchBooks();
                
                label1.Text = $"Search for: {textBox1.Text}";
                textBox1.Text = "Search Book";
                textBox1.ForeColor = Color.Silver;
                button2.Focus();

            }
        }
    }
}

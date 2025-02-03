using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class historyBook : UserControl
    {
        public historyBook()
        {
            InitializeComponent();
            DisplayHistoryBooks();
            label5.Hide();
        }
        public void DisplayHistoryBooks()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";
            string query = "SELECT * FROM book_history ORDER BY ID DESC";

            using (MySqlConnection conDatabase = new MySqlConnection(constring))
            {
                try
                {
                    conDatabase.Open();

                    using (MySqlCommand cmdDatabase = new MySqlCommand(query, conDatabase))
                    {
                        using (MySqlDataReader reader = cmdDatabase.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string changeId = reader["ID"].ToString();
                                string bookId = reader["Book_ID"].ToString();
                                string changeDate = reader["Change_Date"].ToString();
                                string remarks = reader["Remarks"].ToString();

                                // Create a historyMemberLayout for each row in the result set
                                historyBooksLayout layout = new historyBooksLayout(changeId, bookId, changeDate, remarks);

                                // Assuming you have a container (e.g., a panel) to add the layouts to
                                flowLayoutPanel1.Controls.Add(layout);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}\n\nDetails:\n{ex.ToString()}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public void refreshControl()
        {
            flowLayoutPanel1.Controls.Clear();
            DisplayHistoryBooks();
        }





        public void matchBookName(string trymatch)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";
            string query = "SELECT Book_Id, Book_Name FROM books WHERE Book_Name LIKE @SearchTerm";

            using (MySqlConnection conDatabase = new MySqlConnection(constring))
            {
                try
                {
                    conDatabase.Open();

                    // Clear existing controls before the search
                    flowLayoutPanel1.Controls.Clear();

                    using (MySqlCommand cmdDatabase = new MySqlCommand(query, conDatabase))
                    {
                        cmdDatabase.Parameters.AddWithValue("@SearchTerm", $"%{trymatch}%");

                        using (MySqlDataReader reader = cmdDatabase.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    // Retrieve Book_ID and Book_Name from the reader for each matching book
                                    string bookID = reader["Book_Id"].ToString();
                                    string bookName = reader["Book_Name"].ToString();

                                    // Call the searchDisplayHistoryBooks method directly
                                    searchDisplayHistoryBooks(bookID);
                                }
                            }
                            else
                            {
                                searchDisplayHistoryBooks(trymatch);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}\n\nDetails:\n{ex.ToString()}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void searchDisplayHistoryBooks(string searchTerm)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";
            string query = "SELECT * FROM book_history WHERE Book_ID = @SearchTerm OR Change_Date LIKE @SearchTerm OR Remarks LIKE @SearchTerm ORDER BY ID DESC";

            using (MySqlConnection conDatabase = new MySqlConnection(constring))
            {
                try
                {
                    conDatabase.Open();

                    using (MySqlCommand cmdDatabase = new MySqlCommand(query, conDatabase))
                    {
                        // Check if searchTerm is a valid integer
                        if (int.TryParse(searchTerm, out int bookIdInt))
                        {
                            // If it's an integer, search directly by Book_Id
                            cmdDatabase.Parameters.AddWithValue("@SearchTerm", bookIdInt);
                        }
                        else
                        {
                            // If it's not an integer, search using the LIKE pattern for Change_Date or Remarks
                            cmdDatabase.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");
                        }

                        using (MySqlDataReader reader = cmdDatabase.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string changeId = reader["ID"].ToString();
                                string bookId = reader["Book_ID"].ToString(); // It will be an integer
                                string changeDate = reader["Change_Date"].ToString();
                                string remarks = reader["Remarks"].ToString();

                                // Create a historyBooksLayout for each row in the result set
                                historyBooksLayout layout = new historyBooksLayout(changeId, bookId, changeDate, remarks);

                                // Add the layout to the flowLayoutPanel
                                flowLayoutPanel1.Controls.Add(layout);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}\n\nDetails:\n{ex.ToString()}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }






        private void button4_Click(object sender, EventArgs e)
        {
            refreshControl();
            label5.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "Search here")
            {
                MessageBox.Show("Please enter a valid search term.", "Empty Search", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                matchBookName(textBox1.Text);
                label5.Show();
                label5.Text = $"Search for: {textBox1.Text}";
                textBox1.Text = "Search here";
                textBox1.ForeColor = Color.Silver;
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Search here")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Search here";
                textBox1.ForeColor = Color.Silver;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                matchBookName(textBox1.Text);
                label5.Show();
                label5.Text = $"Search for: {textBox1.Text}";
                textBox1.Text = "Search here";
                textBox1.ForeColor = Color.Silver;
                button2.Focus();

            }
        }
    }
}

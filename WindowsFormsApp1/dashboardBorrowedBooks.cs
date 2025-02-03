using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class dashboardBorrowedBooks : UserControl
    {
        public dashboardBorrowedBooks()
        {
            InitializeComponent();
        }
        public dashboardBorrowedBooks(string labelID, string LabelName) : this()
        {
            label1.Text = labelID;
            label2.Text = LabelName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            getBookDetails(label1.Text);
        }



        public void getBookDetails(string bookId)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";
            string query = "SELECT * FROM books WHERE Book_Id = @BookId";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(constring))
                {
                    connection.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@BookId", bookId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string imagepath = reader["Image_Path"].ToString();
                                string bookName = reader["Book_Name"].ToString();
                                string author = reader["Book_Author"].ToString();
                                string location = reader["Book_Location"].ToString();
                                int stocks = Convert.ToInt32(reader["Book_Stocks"]);
                                string genre = reader["Book_Genre"].ToString();
                                books book = new books();
                                editBook editForm = new editBook(book, imagepath, bookName, author, location, stocks, genre, bookId);
                                editForm.Label7.Show();

                                // Disable the main form
                                this.FindForm().Enabled = false;
                                
                                editForm.Button2.Visible = false;
                                editForm.TextBox1.Enabled = false;
                                editForm.TextBox2.Enabled = false;
                                editForm.ComboBox2.Enabled = false;
                                editForm.TextBox4.Enabled = false;
                                editForm.ComboBox1.Enabled = false;
                                editForm.PictureBox1.Enabled = false;
                                editForm.Button5.Visible = false;
                                
                                // Subscribe to the FormClosed event of the pop-up form
                                editForm.FormClosed += (s, args) =>
                                {
                                    // Enable the main form when the pop-up form is closed
                                    this.FindForm().Enabled = true;
                                    

                                };

                                // Show the pop-up form
                                editForm.ShowDialog();
                            }
                            else
                            {
                                MessageBox.Show("Book not found.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}

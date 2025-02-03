using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class returnBook : Form
    {
        public string updateRecordID;
        public string Violation;
        private borrowedBookList parentBorrowedBookList;
        private returnDisplayLayout parentreturn;
        public returnBook()
        {
            InitializeComponent();
            textBox1.Enabled = false;
            label11.Hide();
            
        }
        public returnBook(borrowedBookList parentBorrowedBookList, string bookName, string bookAuthor, string BookId, string borrowedDate, string returnDate, string remaarks, string bookBorrowRecordID, string imageData) : this()
        {
            this.parentBorrowedBookList = parentBorrowedBookList;
            displayReturn(bookName, bookAuthor, BookId, borrowedDate, returnDate, remaarks, bookBorrowRecordID, imageData);
            DateTime date = DateTime.Now;
            DateTime returndate = DateTime.Parse(returnDate);
            if (date > returndate)
            {
                checkBox1.Checked = true;
            }
        }
        public returnBook(returnDisplayLayout parentreturn, string bookName, string bookAuthor, string BookId, string borrowedDate, string returnDate, string remaarks, string bookBorrowRecordID, string imageData) : this()
        {
            this.parentreturn = parentreturn;
            displayReturn(bookName, bookAuthor, BookId, borrowedDate, returnDate, remaarks, bookBorrowRecordID, imageData);
            DateTime date = DateTime.Now;
            DateTime returndate = DateTime.Parse(returnDate);
            if (date > returndate)
            {
                checkBox1.Checked = true;
            }
        }
        
        public void displayReturn(string bookName, string bookAuthor, string BookId, string borrowedDate, string returnDate, string remaarks, string bookBorrowRecordID, string imageData)
        {
            label2.Text = bookName;
            label3.Text = BookId;
            label10.Text = bookAuthor;
            label6.Text = borrowedDate;
            label7.Text = returnDate;
            label9.Text = remaarks;

            pictureBox1.Image = Image.FromFile(imageData);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            

            //updateRecordID = bookBorrowRecordID;
            label11.Text = bookBorrowRecordID;
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox2.Checked == true)
            {
                textBox1.Enabled = true;
            }
            else
            {
                textBox1.Enabled = false;
                textBox1.Clear();
            }
        }

        
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";
                string updateQuery = "UPDATE library_of_life.borrowlist " +
                     "SET Status = @status, " +
                     "    New_Remarks = @newRemarks, " +
                     "    Returned_Date = @returnedDate, " +
                     "    Returned_Time = @returnedTime, " +
                     "    Violation = @violation " +  // Add SET here
                     "WHERE ID = @Booklist";


                using (MySqlConnection conn = new MySqlConnection(constring))
                using (MySqlCommand cmd = new MySqlCommand(updateQuery, conn))
                {
                    if (checkBox1.Checked == true && checkBox2.Checked == true)
                    {
                        Violation = "LATE & DAMAGED";
                    }
                    else if (checkBox2.Checked == true && checkBox1.Checked == false)
                    {
                        Violation = "DAMAGED";
                    }
                    else if (checkBox1.Checked == true && checkBox2.Checked == false)
                    {
                        Violation = "LATE";
                    }
                    else if (checkBox3.Checked) 
                    {
                        Violation = "MISSING";
                    }
                    else
                    {
                        Violation = " ";
                    }

                    cmd.Parameters.AddWithValue("@status", "Returned"); // Correct parameter name
                    cmd.Parameters.AddWithValue("@newRemarks", textBox1.Text);
                    cmd.Parameters.AddWithValue("@returnedDate", DateTime.Now.ToString("MM-dd-yyyy"));
                    cmd.Parameters.AddWithValue("@returnedTime", DateTime.Now.ToString("HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@violation", Violation.ToString());
                    // Assuming you have a BookList variable, replace it with the correct one
                    cmd.Parameters.AddWithValue("@Booklist", Convert.ToInt32(label11.Text));

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Changes saved successfully!");
                        conn.Close();
                        this.Close();
                        if (int.TryParse(label3.Text, out int bookId))
                        {
                            if (checkBox3.Checked) 
                            {
                                MessageBox.Show("DONE");
                            } else
                            {
                                IncrementBookStocks(bookId);
                            }
                            
                        }
                        else
                        {
                            MessageBox.Show("Invalid book ID format.");
                        }

                        // Access the parentBorrowedBookList reference and hide button1
                        if (parentBorrowedBookList != null)
                        {
                            parentBorrowedBookList.Button1.Visible = false;
                            parentBorrowedBookList.Label4.Text = textBox1.Text;
                            parentBorrowedBookList.Label8.Text = "Returned";
                            parentBorrowedBookList.Label6.Text = $"{DateTime.Now.ToString("MM-dd-yyyy")}";
                            parentBorrowedBookList.Label7.Text = $"{Violation}";
                        }
                    }
                    else
                    {
                        MessageBox.Show("No changes made or failed to save changes. Please check your input.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }  
        }




        public void IncrementBookStocks(int bookId)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";
            string updateQuery = "UPDATE books SET Book_Stocks = Book_Stocks + 1 WHERE Book_Id = @BookId";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(constring))
                {
                    connection.Open();

                    using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, connection))
                    {
                        updateCmd.Parameters.AddWithValue("@BookId", Convert.ToInt32(bookId));

                        int rowsAffected = updateCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Rows were affected, meaning the update was successful
                            MessageBox.Show("Book stocks updated successfully.");
                        }
                        else
                        {
                            // No rows were affected, meaning the book with the given ID was not found
                            MessageBox.Show("Book not found or stocks could not be updated.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
            } else
            {
                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
            }
        }
    }
}


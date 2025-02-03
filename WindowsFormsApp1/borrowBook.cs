using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class borrowBook : Form
    {
        public List<string> remarkList = new List<string>();
        public List<int> bookList = new List<int>();
        private int displayedBooksCount = 0;
        borrowed borrowed = new borrowed();
        memberInformation memberInformation = new memberInformation();
        private string MemberID;
        booklist booklist = new booklist();

        
        public FlowLayoutPanel FlowLayoutPanel1
        {
            get { return flowLayoutPanel1; }
            set { flowLayoutPanel1 = value; }
        }
        public Label Label1
        {
            get { return label1; }
            set { label1 = value; }
        }
        public borrowBook()
        {
            InitializeComponent();
            textBox2.Text = "7";
            radioButton1.Checked = true;
        }

        private void borrowBook_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (flowLayoutPanel1.Controls.Count == 0)
            {
                MessageBox.Show("The list of books is empty.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {

                proceedToBorrowusingID();
            }
        }

        public void proceedToBorrowusingID()
        {
            // Clear the remarkList before populating it
            remarkList.Clear();

            // Loop through each borrowed control and get the remarks
            foreach (borrowed bookControl in flowLayoutPanel1.Controls.OfType<borrowed>())
            {
                string remarks = bookControl.TextBox1.Text;
                remarkList.Add(remarks);
            }

            // Show the book list with remarks
            ShowBookList();
            saveBorrowedBook();
        }
        private void ShowBookList()
        {
            StringBuilder bookListText = new StringBuilder("Book List with Remarks:\n");

            for (int i = 0; i < bookList.Count; i++)
            {
                int bookID = bookList[i];
                string remarks = i < remarkList.Count ? remarkList[i] : "No remarks";

                bookListText.AppendLine($"{bookID} - Remarks: {remarks}");
            }

            
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                countStocks();
            } else if (radioButton1.Checked)
            {
                countStocks2();
            }
            
        }
        public void countStocks()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();

                    int BOOKID = Convert.ToInt32(textBox1.Text);
                    string query = "SELECT Book_Stocks, Status FROM books WHERE Book_Id = @Bookid";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    cmdDatabase.Parameters.AddWithValue("@Bookid", BOOKID);

                    using (MySqlDataReader reader = cmdDatabase.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            
                            if (reader["Book_Stocks"].ToString() == "0")
                            {
                                MessageBox.Show("The book is out of stock.", "Out of Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            } else if (reader["Status"].ToString() == "Phase Out")
                            {
                                MessageBox.Show("The book was Phase Out.", "Phase Out", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                DisplayBorrowBooks(BOOKID);
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
        public void countStocks2()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();

                    string bookName = textBox1.Text;
                    string query = "SELECT Book_Stocks, Book_Id FROM books WHERE Book_Name = @Bookname";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    cmdDatabase.Parameters.AddWithValue("@Bookname", bookName);

                    using (MySqlDataReader reader = cmdDatabase.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int bookID = Convert.ToInt32(reader["Book_Id"]);
                            if (reader["Book_Stocks"].ToString() == "0")
                            {
                                MessageBox.Show("The book is out of stock.", "Out of Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                DisplayBorrowBooks(bookID);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Book not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        public void DisplayBorrowBooks(int bookId)
        {
            if (displayedBooksCount >= 5)
            {
                MessageBox.Show("You can only borrow up to 5 books.");
                return;
            }

            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();

                    // Use the bookId parameter instead of converting textBox1.Text
                    string query = "select * from books Where Book_Id = @Bookid";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    cmdDatabase.Parameters.AddWithValue("@Bookid", bookId);

                    using (MySqlDataReader reader = cmdDatabase.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("Book_Name")) && !reader.IsDBNull(reader.GetOrdinal("Image_Path")))
                            {
                                // Pass the bookId when creating an instance of borrowed
                                borrowed bookControl = new borrowed(
                                    this,  // Pass the borrowBook instance
                                    bookId,
                                    reader["Book_Name"].ToString(),
                                    reader["Image_Path"].ToString()
                                );

                                // Add the bookId to the bookList
                                bookList.Add(bookId);

                                // Add an empty remark to the remarkList
                                remarkList.Add(" ");

                                // Add the 'borrowed' control to the flowLayoutPanel1
                                flowLayoutPanel1.Controls.Add(bookControl);
                                textBox1.Clear();
                                // Increment the displayedBooksCount
                                displayedBooksCount++;

                                // Disable the button if the maximum limit is reached
                                if (displayedBooksCount >= 5)
                                {
                                    button1.Enabled = false;
                                }
                            }
                        }
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }




    


    public void DecrementDisplayedBooksCount()
        {
            displayedBooksCount--;

            if (displayedBooksCount < 5)
            {
                button1.Enabled = true;
            }
        }


        public void SetmemberID(string memberID)
        {
            MemberID = memberID;
            
        }

        public void saveBorrowedBook()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();
                    string referenceID = GenerateUniqueReferenceID(connection);
                    

                    // Insert into borrowedbook table
                    string borrowBookQuery = "INSERT INTO borrowedbook (member_ID, `Reference_ID`, Borrowed_Date, Return_Date) VALUES (@MemberID, @ReferenceID, @Borrowed, @Return)";
                    using (MySqlCommand cmdBorrowBook = new MySqlCommand(borrowBookQuery, connection))
                    {
                        // Assuming MemberID is a property or variable in your borrowBook class
                        cmdBorrowBook.Parameters.AddWithValue("@MemberID", MemberID);

                        // Assuming ReferenceID is a property or variable in your borrowBook class
                        cmdBorrowBook.Parameters.AddWithValue("@ReferenceID", referenceID);
                        cmdBorrowBook.Parameters.AddWithValue("@Borrowed", DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss"));

                        cmdBorrowBook.Parameters.AddWithValue("@Return", DateTime.Now.AddDays(Convert.ToUInt32(textBox2.Text)).ToString("MM-dd-yyyy HH:mm:ss"));

                        cmdBorrowBook.ExecuteNonQuery();
                    }

                    // Insert into borrowlist table
                    InsertBookList(connection, referenceID);

                    MessageBox.Show("Borrowed book successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void InsertBookList(MySqlConnection connection, string referenceID)
        {
            string insertQuery = "INSERT INTO borrowlist (Reference_ID, Book_List, Book_Remarks, Borrowed_Date, Return_Date, Status, Initial_Status) VALUES (@ReferenceID, @BookList, @BookRemarks, @BorrowedDate, @ReturnDate, @Status, @Initial_Status)";

            using (MySqlCommand cmdDatabase = new MySqlCommand(insertQuery, connection))
            {
                for (int i = 0; i < bookList.Count; i++)
                {
                    cmdDatabase.Parameters.Clear();
                    cmdDatabase.Parameters.AddWithValue("@ReferenceID", referenceID);
                    cmdDatabase.Parameters.AddWithValue("@BookList", bookList[i]);
                    cmdDatabase.Parameters.AddWithValue("@BookRemarks", i < remarkList.Count ? remarkList[i] : "No remarks");

                    // Set Borrowed_Date to the current date
                    cmdDatabase.Parameters.AddWithValue("@BorrowedDate", DateTime.Now.ToString("MM-dd-yyyy"));
                    int retdate = Convert.ToInt32(textBox2.Text);

                    // Set Return_Date to 7 days from the current date
                    DateTime returnDate = DateTime.Now.AddDays(retdate);
                    cmdDatabase.Parameters.AddWithValue("@ReturnDate", returnDate.ToString("MM-dd-yyyy"));

                    // Set Status to "Borrowed"
                    cmdDatabase.Parameters.AddWithValue("@Status", "Borrowed");
                    cmdDatabase.Parameters.AddWithValue("@Initial_Status", "Borrowed");

                    cmdDatabase.ExecuteNonQuery();

                    // Decrement Book_Stocks in the books table
                    DecrementBookStocks(connection, bookList[i]);
                }
            }
        }

        private void DecrementBookStocks(MySqlConnection connection, int bookID)
        {
            string decrementQuery = "UPDATE books SET Book_Stocks = Book_Stocks - 1 WHERE Book_Id = @BookID";

            using (MySqlCommand cmdDecrement = new MySqlCommand(decrementQuery, connection))
            {
                cmdDecrement.Parameters.AddWithValue("@BookID", bookID);
                cmdDecrement.ExecuteNonQuery();
                this.Close();
            }
        }




        private string GenerateUniqueReferenceID(MySqlConnection connection)
        {
            Random random = new Random();
            string referenceID;

            // Generate a unique 9-digit reference ID
            do
            {
                referenceID = random.Next(100000000, 1000000000).ToString();
            }
            while (IsReferenceIDDuplicate(referenceID, connection));

            return referenceID;
        }

        private bool IsReferenceIDDuplicate(string referenceID, MySqlConnection connection)
        {
            string query = "SELECT Reference_ID FROM borrowedbook WHERE Reference_ID = @ReferenceID";
            using (MySqlCommand cmdDatabase = new MySqlCommand(query, connection))
            {
                cmdDatabase.Parameters.AddWithValue("@ReferenceID", referenceID);

                using (MySqlDataReader reader = cmdDatabase.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                radioButton2.Checked = false;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                radioButton1.Checked = false;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (radioButton2.Checked)
                {
                    countStocks();
                }
                else if (radioButton1.Checked)
                {
                    countStocks2();
                }
            }
        }
    }
}


using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class borrowedBookList : UserControl
    {
        private bool isPopUpFormOpen = false;
        public System.Windows.Forms.Label Label1
        {
            get { return label1; }
            set { label1 = value; }
        }
        public System.Windows.Forms.Label Label2
        {
            get { return label2; }
            set { label2 = value; }
        }
        public System.Windows.Forms.Label Label3
        {
            get { return label3; }
            set { label3 = value; }
        }
        public System.Windows.Forms.Label Label4
        {
            get { return label4; }
            set { label4 = value; }
        }
        public Button Button1
        {
            get { return button1; }
            set { button1 = value; }
        }
        public System.Windows.Forms.Label Label6
        {
            get { return label6; }
            set { label6 = value; }
        }
        public System.Windows.Forms.Label Label7
        {
            get { return label7; }
            set { label7 = value; }
        }
        public System.Windows.Forms.Label Label8
        {
            get { return label8; }
            set { label8 = value; }
        }
        public borrowedBookList()
        {
            InitializeComponent();
            label3.Hide();
        }
        
        public borrowedBookList(string imageData, string BookID, string id, string BookName, string Remarks):this()
        {
            displayallborrowedBooks(imageData, BookID, id, BookName, Remarks);
        }
        private Image ByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream memoryStream = new MemoryStream(byteArray))
            {
                return Image.FromStream(memoryStream);
            }
        }
        public void displayallborrowedBooks(string imageData, string BookID, string id, string BookName , string Remarks)
        {
            label3.Text = BookID;
            label2.Text = BookName;
            label1.Text = id;
            pictureBox1.Image = Image.FromFile(imageData);
            label4.Text = Remarks;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            
        }

        

        public void fetchForReturn()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM borrowlist WHERE ID = @bookborrowid";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    cmdDatabase.Parameters.AddWithValue("@bookborrowid", Convert.ToInt32(label3.Text));

                    using (MySqlDataReader reader = cmdDatabase.ExecuteReader())
                    { 
                        if (reader.Read())
                        {
                            realFetcher(reader);
                        }                           
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.ToString()}");
                }
            }
        }
        public void realFetcher(MySqlDataReader reader)
        {
            using (MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password=;database=library_of_life"))
            {
                connection.Open();

                string borrowBookId = label3.Text;
                string bookId = reader["Book_List"].ToString();
                string remarks = reader["Book_Remarks"].ToString();
                string borrowedDate = reader["Borrowed_Date"].ToString();
                string returnDate = reader["Return_Date"].ToString();

                string bookDetailsQuery = "SELECT Book_Id, Book_Name, Image_Path, Book_Author FROM books WHERE Book_Id = @BookId";
                MySqlCommand bookDetailsCmd = new MySqlCommand(bookDetailsQuery, connection);
                bookDetailsCmd.Parameters.AddWithValue("@BookId", Convert.ToInt32(label1.Text));  // Use the bookId obtained from borrowlist reader

                using (MySqlDataReader bookDetailsReader = bookDetailsCmd.ExecuteReader())
                {
                    if (bookDetailsReader.Read())
                    {
                        string IDBook = label1.Text;  // This line might need adjustment based on your application logic
                        string bookName = bookDetailsReader["Book_Name"].ToString();
                        string imageData = bookDetailsReader["Image_Path"].ToString();
                        string author = bookDetailsReader["Book_Author"].ToString();

                        this.OpenAddingForm(bookName, author, IDBook, borrowedDate, returnDate, remarks, borrowBookId, imageData);
                       
                        
                    }
                }

            }
        }
        private void button1_Click(object sender, EventArgs e)
        {

            fetchForReturn();
        }
        public void OpenAddingForm(string bookName, string bookAuthor, string BookId, string borrowedDate, string returnDate, string remaarks, string bookBorrowRecordID, string imageData)
        {
            if (!isPopUpFormOpen)
            {
                // Disable the main form
                this.FindForm().Enabled = false;

                // Open your pop-up form here
                returnBook popUpForm = new returnBook(this, bookName, bookAuthor, BookId, borrowedDate, returnDate, remaarks, bookBorrowRecordID, imageData);

                isPopUpFormOpen = true;

                // Subscribe to the FormClosed event of the pop-up form
                popUpForm.FormClosed += (s, args) =>
                {
                    // Enable the main form when the pop-up form is closed
                    this.FindForm().Enabled = true;
                    isPopUpFormOpen = false;
                };

                popUpForm.ShowDialog();
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}

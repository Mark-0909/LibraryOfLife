using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Windows.Forms;
using iText.IO.Image;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class books : UserControl
    {
        private bool isPopUpFormOpen = false;
        System.Windows.Forms.Label labelTitle, labelBookId, labelAuthor, labelLocation, labelStocks;
        PictureBox pictureBox;
        
        public string bookID;
        
        
        public System.Windows.Forms.Label Label1
        {
            get { return label1; }
            set { label1 = value; }
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
        
        public System.Windows.Forms.Label Label6
        {
            get { return label6; }
            set { label6 = value; }
        }
        public System.Windows.Forms.PictureBox PictureBox1
        {
            get { return pictureBox1; }
            set { pictureBox1 = value; }
        }

        public books()
        {
            InitializeComponent();
            InitializeComponentState();
            labelTitle = label1;
            labelBookId = label2;
            labelAuthor = label3;
            labelLocation = label4;
            labelStocks = label6;
            pictureBox = pictureBox1;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            button3.Hide();


            bookID = labelBookId.Text;

            


        }
        private void InitializeComponentState()
        {
            
        }

        public books(string bookName, string bookId, string author, string location, string stocks, string imageData, string status)
            : this()
        {
            SetBookData(bookName, bookId, author, location, stocks, imageData, status);

            //pwede diresto na like label3.text = bookName; etc
        }
        


        


        public void SetBookData(string bookName, string bookId, string author, string location, string stocks, string imageData, string status)
        {

            if(status == "Available")
            {
                labelTitle.Text = bookName;
                labelBookId.Text = bookId;
                labelAuthor.Text = author;
                labelLocation.Text = location;
                labelStocks.Text = stocks;

                pictureBox1.Image = Image.FromFile(imageData);

                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                
            } else if (status == "Phase Out")
            {
                labelTitle.Text = bookName;
                labelBookId.Text = bookId;
                labelAuthor.Text = author;
                labelLocation.Text = location;
                labelStocks.Text = stocks;

                pictureBox1.Image = Image.FromFile(imageData);

                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                button3.Show();
                button1.Enabled = false;
                button2.Enabled = false;
            }
            
            
        }

        public void label1_Click(object sender, EventArgs e)
        {

        }

        private void books_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

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
                                
                                editBook editForm = new editBook(this, imagepath, bookName, author, location, stocks, genre, bookId);
                                editForm.Label7.Show();
                                editForm.Button2.BringToFront();
                                // Disable the main form
                                this.FindForm().Enabled = false;
                                
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



        

        private void button1_Click(object sender, EventArgs e)
        {
            getBookDetails(label2.Text);
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to set this book as Available?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);


            if (result == DialogResult.Yes)
            {
                
                getdetailsbeforedelete1();
            }
            else
            {

            }
        }

        public void getdetailsbeforedelete1()
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
                        cmd.Parameters.AddWithValue("@BookId", label2.Text);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                saveBookChangesHistory1(reader);


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

        public void saveBookChangesHistory1(MySqlDataReader initialDetails)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            string insertQuery = "INSERT INTO library_of_life.book_history (Book_ID, Change_Name, Change_Author, Change_Location, Change_Stocks, Change_Genre, Change_Date, Remarks, Image_Path) " +
                     "VALUES(@ID, @Name, @Author, @Location, @Stocks, @Genre, @ChangeDate, @Remarks, @Image); SELECT LAST_INSERT_ID();";


            using (MySqlConnection conDatabase = new MySqlConnection(constring))
            using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, conDatabase))
            {
                insertCommand.Parameters.AddWithValue("@ID", label2.Text);
                insertCommand.Parameters.AddWithValue("@Name", $"{initialDetails["Book_Name"]}");
                insertCommand.Parameters.AddWithValue("@Author", $"{initialDetails["Book_Author"]}");
                insertCommand.Parameters.AddWithValue("@Location", $"{initialDetails["Book_Location"]}");
                insertCommand.Parameters.AddWithValue("@Stocks", $"{initialDetails["Book_Stocks"]}");
                insertCommand.Parameters.AddWithValue("@Genre", $"{initialDetails["Book_Genre"]}");
                insertCommand.Parameters.AddWithValue("@ChangeDate", DateTime.Now.ToString("MM-dd-yyyy"));
                insertCommand.Parameters.AddWithValue("@Remarks", "AVAILABLE");
                insertCommand.Parameters.AddWithValue("@Image", $"{initialDetails["Image_Path"]}");

                try
                {
                    conDatabase.Open();
                    int bookId = Convert.ToInt32(insertCommand.ExecuteScalar()); // Retrieve the newly inserted Book_Id

                    if (bookId > 0)
                    {
                        MessageBox.Show("Changes saved successfully!");
                        deleteBook1(label2.Text);
                    }
                    else
                    {
                        MessageBox.Show("Failed to save changes. Please check your input.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        public void deleteBook1(string bookId)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";
            string query = "UPDATE books SET Status = 'Available' WHERE Book_Id = @BookId";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(constring))
                {
                    connection.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@BookId", bookId);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Book set as Available successfully.");
                            button3.Hide();
                            button1.Enabled = true;
                            button2.Enabled = true;
                        }
                        else
                        {
                            MessageBox.Show("Book not found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            

            
            DialogResult result = MessageBox.Show("Are you sure you want to set this book as Phase out?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            
            if (result == DialogResult.Yes)
            {
                // Get details before delete
                getdetailsbeforedelete();
            }
            else
            {
               
            }
        }


        public void getdetailsbeforedelete()
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
                        cmd.Parameters.AddWithValue("@BookId", label2.Text);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                saveBookChangesHistory(reader);


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

        public void saveBookChangesHistory(MySqlDataReader initialDetails)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            string insertQuery = "INSERT INTO library_of_life.book_history (Book_ID, Change_Name, Change_Author, Change_Location, Change_Stocks, Change_Genre, Change_Date, Remarks, Image_Path) " +
                     "VALUES(@ID, @Name, @Author, @Location, @Stocks, @Genre, @ChangeDate, @Remarks, @Image); SELECT LAST_INSERT_ID();";


            using (MySqlConnection conDatabase = new MySqlConnection(constring))
            using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, conDatabase))
            {
                insertCommand.Parameters.AddWithValue("@ID", label2.Text);
                insertCommand.Parameters.AddWithValue("@Name", $"{initialDetails["Book_Name"]}");
                insertCommand.Parameters.AddWithValue("@Author", $"{initialDetails["Book_Author"]}");
                insertCommand.Parameters.AddWithValue("@Location", $"{initialDetails["Book_Location"]}");
                insertCommand.Parameters.AddWithValue("@Stocks", $"{initialDetails["Book_Stocks"]}");
                insertCommand.Parameters.AddWithValue("@Genre", $"{initialDetails["Book_Genre"]}");
                insertCommand.Parameters.AddWithValue("@ChangeDate", DateTime.Now.ToString("MM-dd-yyyy"));
                insertCommand.Parameters.AddWithValue("@Remarks", "PHASE OUT");
                insertCommand.Parameters.AddWithValue("@Image", $"{initialDetails["Image_Path"]}");

                try
                {
                    conDatabase.Open();
                    int bookId = Convert.ToInt32(insertCommand.ExecuteScalar()); // Retrieve the newly inserted Book_Id

                    if (bookId > 0)
                    {
                        MessageBox.Show("Changes saved successfully!");
                        deleteBook(label2.Text);
                    }
                    else
                    {
                        MessageBox.Show("Failed to save changes. Please check your input.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }
        
        public void deleteBook(string bookId)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";
            string query = "UPDATE books SET Status = 'Phase Out' WHERE Book_Id = @BookId";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(constring))
                {
                    connection.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@BookId", bookId);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Book phase out successfully.");
                            button3.Show();
                            button1.Enabled = false;
                            button2.Enabled = false;
                        }
                        else
                        {
                            MessageBox.Show("Book not found.");
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
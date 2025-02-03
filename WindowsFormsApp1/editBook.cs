
using MySql.Data.MySqlClient;
using System;


using System.Drawing;
using System.IO;

using System.Windows.Forms;

namespace WindowsFormsApp1
{

    public partial class editBook : Form
    {
        
        public booklist bookdisplay;
        public TextBox TextBox1
        {
            get { return textBox1; }
            set { textBox1 = value; }
        }

        public TextBox TextBox2
        {
            get { return textBox2; }
            set { textBox2 = value; }
        }

        public ComboBox ComboBox2
        {
            get { return comboBox2; }
            set { comboBox2 = value; }
        }

        public TextBox TextBox4
        {
            get { return textBox4; }
            set { textBox4 = value; }
        }

       

        public PictureBox PictureBox1
        {
            get { return pictureBox1; }
            set { pictureBox1 = value; }
        }

        public ComboBox ComboBox1
        {
            get { return comboBox1; }
            set { comboBox1 = value; }
        }
        public Label Label7
        {
            get { return label7; }
            set { label7 = value; }
        }
        
        public Button Button2
        {
            get { return button2; }
            set { button2 = value; }
        }
        public Button Button5
        {
            get { return button5; }
            set { button5 = value; }
        }
        public Button Button4
        {
            get { return button4; }
            set { button4 = value; }
        }
        
        public Label Label1
        {
            get { return label1; }
            set { label1 = value; }
        }

        private books bookControl;
        
        public editBook()
        {
            
            InitializeComponent();
            
            this.label7.Hide();
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            SetItemsLocationComboBox();
            SetItemsGenreComboBox();
        }
        
        public editBook(books bookControl, string imageData, string BookName, string author, string location, int stocks, string genre, string ID) : this()
        {
            DisplayBookDetails(imageData, BookName, author, location, stocks, genre, ID);
            this.bookControl = bookControl;
        }
        
        public void DisplayBookDetails(string imageData, string BookName, string author, string location, int stocks, string genre, string ID)
        {
            textBox1.Text = BookName;
            textBox2.Text = author;
            comboBox2.Text = location;
            textBox4.Text = stocks.ToString();
            comboBox1.Text = genre;
            label7.Text = ID;

            // Set the image using the imageData
            // Assuming pictureBox1 is the PictureBox control on your editBook form
            pictureBox1.Image = Image.FromFile(imageData);
            SetItemsLocationComboBox2(location);
            SetItemsGenreComboBox2(genre);





        }

        public void SetItemsLocationComboBox2(string location)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();

                    comboBox2.Items.Clear();

                    // Add a placeholder item
                    comboBox2.Items.Add(location);



                    string query = "SELECT Location_Name FROM location_list";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    MySqlDataReader reader = cmdDatabase.ExecuteReader();

                    while (reader.Read())
                    {
                        string locationName = reader["Location_Name"].ToString();
                        comboBox2.Items.Add(locationName);
                    }

                    // Set the default selection to the placeholder
                    comboBox2.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }
        public void SetItemsLocationComboBox()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();

                    // Clear existing items before adding new ones
                    comboBox2.Items.Clear();

                    // Add a placeholder item
                    comboBox2.Items.Add("Select here");

                    string query = "SELECT Location_Name FROM location_list";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    MySqlDataReader reader = cmdDatabase.ExecuteReader();

                    while (reader.Read())
                    {
                        string locationName = reader["Location_Name"].ToString();
                        comboBox2.Items.Add(locationName);
                    }

                    // Set the default selection to the placeholder
                    comboBox2.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
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
                    comboBox1.Items.Add("Select here");

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

        public void SetItemsGenreComboBox2(string genre)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();

                    // Clear existing items before adding new ones
                    comboBox1.Items.Clear();

                    comboBox1.Items.Add(genre);

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

        
        private void button1_Click(object sender, EventArgs e)
        {

        }

        

        private void button1_Click_1(object sender, EventArgs e)
        {
            
                   
        }

        private string imagePath = "";
        private void button2_Click(object sender, EventArgs e)
        {
            getBookDetails();
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog open = new OpenFileDialog())
            {
                // Image filters
                open.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.gif; *.bmp; *.tiff; *.ico)|*.jpg; *.jpeg; *.png; *.gif; *.bmp; *.tiff; *.ico";

                if (open.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Controls.Clear();
                    pictureBox1.Image = new Bitmap(open.FileName);

                    // Update imagePath based on the OpenFileDialog result
                    imagePath = open.FileName;
                }
            }
        }

        public void getBookDetails()
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
                        cmd.Parameters.AddWithValue("@BookId", label7.Text);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                
                                
                                changeImage(reader);
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

        public void changeImage(MySqlDataReader reader)
        {
            try
            {
                // Get the image from the PictureBox
                Image pictureBoxImage = pictureBox1.Image;

                if (pictureBoxImage != null)
                {
                    // Set the destination folder path
                    string destinationFolderPath = Path.Combine(Application.StartupPath, "Resources");

                    // Create the destination folder if it doesn't exist
                    if (!Directory.Exists(destinationFolderPath))
                    {
                        Directory.CreateDirectory(destinationFolderPath);
                    }
                    string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    // Specify the desired file name (you can modify this as needed)
                    string newFileName = $"{label7.Text}{timestamp}.png"; // Change the extension based on the actual image format

                    // Combine the destination folder path with the new file name
                    string destinationFilePath = Path.Combine(destinationFolderPath, newFileName);

                    // Save the image to the destination folder with the specified name
                    pictureBoxImage.Save(destinationFilePath);

                    

                    saveBookChangesHistory(reader, newFileName);
                }
                else
                {
                    MessageBox.Show("No image in the PictureBox to save.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        













        public void saveBookChangesHistory(MySqlDataReader initialDetails, string newfilename)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            string insertQuery = "INSERT INTO library_of_life.book_history (Book_ID, Change_Name, Change_Author, Change_Location, Change_Stocks, Change_Genre, Change_Date, Remarks, Image_Path, Initial_Name, Initial_Author, Initial_Location, Initial_Stocks, Initial_Genre) " +
                     "VALUES(@ID, @cName, @cAuthor, @cLocation, @cStocks, @cGenre, @ChangeDate, @Remarks, @Image, @iName, @iAuthor, @iLocation, @iStocks, @iGenre); SELECT LAST_INSERT_ID();";
            

            using (MySqlConnection conDatabase = new MySqlConnection(constring))
            using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, conDatabase))
            {
                insertCommand.Parameters.AddWithValue("@ID", label7.Text);
                insertCommand.Parameters.AddWithValue("@cName", textBox1.Text);
                insertCommand.Parameters.AddWithValue("@cAuthor", textBox2.Text);
                insertCommand.Parameters.AddWithValue("@cLocation", comboBox2.Text);
                insertCommand.Parameters.AddWithValue("@cStocks", textBox4.Text);
                insertCommand.Parameters.AddWithValue("@cGenre", comboBox1.Text);
                insertCommand.Parameters.AddWithValue("@ChangeDate", DateTime.Now.ToString("MM-dd-yyyy"));
                insertCommand.Parameters.AddWithValue("@Remarks", "EDIT");
                insertCommand.Parameters.AddWithValue("@Image", $"Resources\\{newfilename}"); // Corrected parameter name
                insertCommand.Parameters.AddWithValue("@iName", initialDetails["Book_Name"]);
                insertCommand.Parameters.AddWithValue("@iAuthor", initialDetails["Book_Author"]);
                insertCommand.Parameters.AddWithValue("@iLocation", initialDetails["Book_Location"]);
                insertCommand.Parameters.AddWithValue("@iStocks", initialDetails["Book_Stocks"]);
                insertCommand.Parameters.AddWithValue("@iGenre", initialDetails["Book_Genre"]);


                try
                {
                    conDatabase.Open();
                    int bookId = Convert.ToInt32(insertCommand.ExecuteScalar()); // Retrieve the newly inserted Book_Id

                    if (bookId > 0)
                    {
                        

                        SaveChangesToDatabase(newfilename);
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

        private void SaveChangesToDatabase(string newFilename)
        {
            try
            {
                string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

                // Use parameterized query to prevent SQL injection
                string updateQuery = "UPDATE library_of_life.books " +
                     "SET Book_Name = @BookName, " +
                     "    Book_Author = @BookAuthor, " +
                     "    Book_Location = @BookLocation, " +
                     "    Book_Stocks = @BookStocks, " +
                     "    Book_Genre = @BookGenre, " +
                     "Image_Path = @Imagepath " +
                     "WHERE Book_Id = @BookId"; // Assuming Book_Id is the primary key


                using (MySqlConnection conDatabase = new MySqlConnection(constring))
                using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, conDatabase))
                {
                    // Add parameters
                    updateCommand.Parameters.AddWithValue("@BookId", label7.Text); // Ensure Book_Id is included in the WHERE clause
                    updateCommand.Parameters.AddWithValue("@BookName", this.textBox1.Text);
                    updateCommand.Parameters.AddWithValue("@BookAuthor", this.textBox2.Text);
                    updateCommand.Parameters.AddWithValue("@BookLocation", this.comboBox2.Text);
                    updateCommand.Parameters.AddWithValue("@BookStocks", this.textBox4.Text);
                    updateCommand.Parameters.AddWithValue("@BookGenre", this.comboBox1.Text);
                    updateCommand.Parameters.AddWithValue("@Imagepath", $"Resources\\{newFilename}"); // Corrected parameter name


                    conDatabase.Open();
                    int rowsAffected = updateCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                       
                        this.Close();
                        if (bookControl != null)
                        {
                            bookControl.Label1.Text = textBox1.Text;
                            bookControl.Label3.Text = textBox2.Text;
                            bookControl.Label4.Text = comboBox2.Text;
                            bookControl.Label6.Text = textBox4.Text;
                            bookControl.PictureBox1.Image = pictureBox1.Image;
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
                // Log the exception for debugging purposes
            }
        }







        private void button5_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null ||
                textBox1.Text == string.Empty ||
                textBox2.Text == string.Empty ||
                textBox4.Text == string.Empty ||
                comboBox1.Text == "Select here" ||
                comboBox2.Text == "Select here")
            {
                MessageBox.Show("Please fill in all the required fields and select valid options.", "Incomplete Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                CheckIfBookExists(textBox1.Text);
            }
        }

        public bool CheckIfBookExists(string bookName)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();

                    // Check if the book name already exists
                    string checkQuery = "SELECT Book_Name FROM books WHERE Book_Name = @BookName";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, connection);
                    checkCmd.Parameters.AddWithValue("@BookName", bookName);

                    using (MySqlDataReader reader = checkCmd.ExecuteReader())
                    {
                        // If reader has rows, then the book name already exists
                        if (reader.HasRows)
                        {
                            MessageBox.Show("Book with the same name already exists.", "Duplicate Book", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            addBooktodatabase();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }





        public void addBooktodatabase()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            string insertQuery = "INSERT INTO library_of_life.books (Book_Name, Book_Author, Book_Location, Book_Stocks, Book_Genre, Date_Added, Image_Path, Status) " +
                     "VALUES(@BookName, @BookAuthor, @BookLocation, @BookStocks, @BookGenre, @Date, @ImagePath, @Status); SELECT LAST_INSERT_ID();";


            using (MySqlConnection conDatabase = new MySqlConnection(constring))
            using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, conDatabase))
            {
                insertCommand.Parameters.AddWithValue("@BookName", this.textBox1.Text);
                insertCommand.Parameters.AddWithValue("@BookAuthor", this.textBox2.Text);
                insertCommand.Parameters.AddWithValue("@BookLocation", this.comboBox2.Text);
                insertCommand.Parameters.AddWithValue("@BookStocks", this.textBox4.Text);
                insertCommand.Parameters.AddWithValue("@BookGenre", this.comboBox1.Text);
                insertCommand.Parameters.AddWithValue("@BookImage", "NONE");  // Add this line for @BookImage
                insertCommand.Parameters.AddWithValue("@Date", DateTime.Now.ToString("MM-dd-yyyy"));
                insertCommand.Parameters.AddWithValue("@ImagePath", "NONE");
                insertCommand.Parameters.AddWithValue("@Status", "Available");
                try
                {
                    conDatabase.Open();

                    using (MySqlTransaction transaction = conDatabase.BeginTransaction())
                    {
                        try
                        {
                            int bookId = Convert.ToInt32(insertCommand.ExecuteScalar()); // Retrieve the newly inserted Book_Id

                            if (bookId > 0)
                            {
                                string imagePath = Path.Combine("Resources", $"{bookId}.png"); // Construct the image path

                                // Update the image path in the database with the relative path
                                string updateImagePathQuery = "UPDATE library_of_life.books SET Image_Path = @ImagePath WHERE Book_Id = @BookId";

                                using (MySqlCommand updateImagePathCommand = new MySqlCommand(updateImagePathQuery, conDatabase))
                                {
                                    updateImagePathCommand.Parameters.AddWithValue("@BookId", bookId);
                                    updateImagePathCommand.Parameters.AddWithValue("@ImagePath", imagePath);
                                    updateImagePathCommand.ExecuteNonQuery();
                                }
                                transaction.Commit();
                                
                                saveImagetoFolder(bookId, Convert.ToInt32(textBox4.Text));


                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Failed to add book. Please check your input.");
                            }
                        }
                        catch (Exception ex)
                        {
                            
                            MessageBox.Show($"Error: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }


        public void saveImagetoFolder(int bookid, int initialStock)
        {
            try
            {
                // Get the image from the PictureBox
                Image pictureBoxImage = pictureBox1.Image;

                if (pictureBoxImage != null)
                {
                    // Set the destination folder path
                    string destinationFolderPath = Path.Combine(Application.StartupPath, "Resources");

                    // Create the destination folder if it doesn't exist
                    if (!Directory.Exists(destinationFolderPath))
                    {
                        Directory.CreateDirectory(destinationFolderPath);
                    }

                    // Specify the desired file name (you can modify this as needed)
                    string newFileName = $"{bookid}.png"; // Change the extension based on the actual image format

                    // Combine the destination folder path with the new file name
                    string destinationFilePath = Path.Combine(destinationFolderPath, newFileName);

                    // Save the image to the destination folder with the specified name
                    pictureBoxImage.Save(destinationFilePath);

                    

                    // Save book history
                    saveBookaddHistory(bookid, initialStock);
                }
                else
                {
                    MessageBox.Show("No image in the PictureBox to save.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        public void saveBookaddHistory(int bookId, int initialStock)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            string insertQuery = "INSERT INTO library_of_life.book_history (Book_ID, Change_Name, Change_Author, Change_Location, Change_Genre, Change_Stocks, Change_Date, Remarks, Image_Path) " +
                                 "VALUES(@BookId, @name, @author, @Location, @genre, @stock, @date, @Remarks, @Image)";

            using (MySqlConnection conDatabase = new MySqlConnection(constring))
            using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, conDatabase))
            {
                insertCommand.Parameters.AddWithValue("@BookId", bookId); // Use the retrieved Book_Id from the books table
                insertCommand.Parameters.AddWithValue("@stock", initialStock);
                insertCommand.Parameters.AddWithValue("@date", DateTime.Now.ToString("MM-dd-yyyy"));
                insertCommand.Parameters.AddWithValue("@name", textBox1.Text);
                insertCommand.Parameters.AddWithValue("@author", textBox2.Text);
                insertCommand.Parameters.AddWithValue("@Location", comboBox2.Text);
                insertCommand.Parameters.AddWithValue("@genre", comboBox1.Text);
                insertCommand.Parameters.AddWithValue("@Remarks", "ADD");
                insertCommand.Parameters.AddWithValue("@Image", $"Resources\\{bookId}.png");

                try
                {
                    conDatabase.Open();
                    int rowsAffected = insertCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        

                    }
                    else
                    {
                        MessageBox.Show("Failed to add book history. Please check your input.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }




        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void button3_Click(object sender, EventArgs e)
        {

            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                textBox2.Focus();
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                comboBox2.Focus();
            }
        }

        private void comboBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                textBox4.Focus();
            }
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                comboBox1.Focus();
            }
        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the pressed key is not a digit and not the backspace key
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                // Consume the key press
                e.Handled = true;
            }
        }


    }
}

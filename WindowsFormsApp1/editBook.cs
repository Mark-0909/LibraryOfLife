using iText.IO.Image;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{

    public partial class editBook : Form
    {
        private string imagePath;
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

        public TextBox TextBox3
        {
            get { return textBox3; }
            set { textBox3 = value; }
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
        public Button Button1
        {
            get { return button1; }
            set { button1 = value; }
        }
        public Button Button3
        {
            get { return button3; }
            set { button3 = value; }
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
        

        public editBook()
        {
            InitializeComponent();
            this.textBox1.Enabled = false;
            this.textBox2.Enabled = false;
            this.textBox3.Enabled = false;
            this.textBox4.Enabled = false;
            this.comboBox1.Enabled = false;
            this.pictureBox1.Enabled = false;


        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private bool isEditMode = false;

        private void button1_Click_1(object sender, EventArgs e)
        {
            isEditMode = !isEditMode; // Toggle the edit mode

            // Set the enabled property based on the current edit mode
            this.textBox1.Enabled = isEditMode;
            this.textBox2.Enabled = isEditMode;
            this.textBox3.Enabled = isEditMode;
            this.textBox4.Enabled = isEditMode;
            this.comboBox1.Enabled = isEditMode;
            this.pictureBox1.Enabled = isEditMode;  
                   
        }


        private void button2_Click(object sender, EventArgs e)
        {
            SaveChangesToDatabase();
        }


        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            // Open file dialog
            OpenFileDialog open = new OpenFileDialog();
            // Image filters
            open.Filter = "Image Files(.jpg; *.jpeg; *.gif; *.bmp)|.jpg; *.jpeg; *.gif; *.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                // Display image in picture box
                pictureBox1.Image = new Bitmap(open.FileName);
                // Update imagePath based on the OpenFileDialog result
                imagePath = open.FileName;
                
            }
        }

        private void SaveChangesToDatabase()
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
                                     "    Book_Image = @BookImage " +
                                     "WHERE Book_Id = @BookId"; // Assuming Book_Id is the primary key

                using (MySqlConnection conDatabase = new MySqlConnection(constring))
                using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, conDatabase))
                {

                    // Add parameters
                    updateCommand.Parameters.AddWithValue("@BookId", label7.Text); // Ensure Book_Id is included in the WHERE clause
                    updateCommand.Parameters.AddWithValue("@BookName", this.textBox1.Text);
                    updateCommand.Parameters.AddWithValue("@BookAuthor", this.textBox2.Text);
                    updateCommand.Parameters.AddWithValue("@BookLocation", this.textBox3.Text);
                    updateCommand.Parameters.AddWithValue("@BookStocks", this.textBox4.Text);
                    updateCommand.Parameters.AddWithValue("@BookGenre", this.comboBox1.Text);
                    updateCommand.Parameters.AddWithValue("@BookImage", ConvertImageToByteArray(pictureBox1.Image)); // Convert image to byte array

                    conDatabase.Open();
                    int rowsAffected = updateCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Changes saved successfully!");
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

        private byte[] ConvertImageToByteArray(Image image)
        {
            if (image == null)
                return null;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png); // You can change the format based on your requirement
                return memoryStream.ToArray();
            }
        }












        private void button5_Click(object sender, EventArgs e)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            // Use parameterized query to prevent SQL injection
            string insertQuery = "INSERT INTO library_of_life.books (Book_Name, Book_Author, Book_Location, Book_Stocks, Book_Genre, Book_Image) " +
                                 "VALUES(@BookName, @BookAuthor, @BookLocation, @BookStocks, @BookGenre, @BookImage)";

            using (MySqlConnection conDatabase = new MySqlConnection(constring))
            using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, conDatabase))
            {
                // Add parameters
                insertCommand.Parameters.AddWithValue("@BookName", this.textBox1.Text);
                insertCommand.Parameters.AddWithValue("@BookAuthor", this.textBox2.Text);
                insertCommand.Parameters.AddWithValue("@BookLocation", this.textBox3.Text);
                insertCommand.Parameters.AddWithValue("@BookStocks", this.textBox4.Text);
                insertCommand.Parameters.AddWithValue("@BookGenre", this.comboBox1.Text);
                insertCommand.Parameters.AddWithValue("@BookImage", ConvertImageToByteArray(pictureBox1.Image)); // Convert image to byte array
                

                try
                {
                    conDatabase.Open();
                    int rowsAffected = insertCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Book added successfully!");

                        // Clear the form or perform any other necessary actions
                        ClearForm();
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
        private void ClearForm()
        {
            // Clear your form fields or perform any other necessary actions
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            comboBox1.SelectedIndex = -1;
            pictureBox1.Image = null;
            label7.Text = string.Empty;
            imagePath = null;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class books : UserControl
    {
        private bool isPopUpFormOpen = false;
        Label labelTitle, labelBookId, labelAuthor, labelLocation, labelStocks;
        PictureBox pictureBox;
        
        public string bookID;
        
        public Label Label1
        {
            get { return label1; }
            set { label1 = value; }
        }
        public books()
        {
            InitializeComponent();
            labelTitle = label1;
            labelBookId = label2;
            labelAuthor = label3;
            labelLocation = label4;
            labelStocks = label6;
            pictureBox = pictureBox1;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            this.Click += YourUserControl_Click;
            this.pictureBox1.Click += YourUserControl_Click;
            this.label1.Click += YourUserControl_Click;
            this.label2.Click += YourUserControl_Click;
            this.label3.Click += YourUserControl_Click;
            this.label4.Click += YourUserControl_Click;
            this.label5.Click += YourUserControl_Click;
            this.label6.Click += YourUserControl_Click;


            bookID = labelBookId.Text;




        }


        public books(string bookName, string bookId, string author, string location, string stocks, byte[] imageData)
            : this()
        {
            SetBookData(bookName, bookId, author, location, stocks, imageData);
        }
        


        private Image ByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream memoryStream = new MemoryStream(byteArray))
            {
                return Image.FromStream(memoryStream);
            }
        }


        public void SetBookData(string bookName, string bookId, string author, string location, string stocks, byte[] imageData)
        {
            labelTitle.Text = bookName;
            labelBookId.Text = bookId;
            labelAuthor.Text = author;
            labelLocation.Text = location;
            labelStocks.Text = stocks;
        
            Image loadedImage = ByteArrayToImage(imageData);

            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Image = loadedImage;
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

        private void YourUserControl_Click(object sender, EventArgs e)
        {
            OpenEditForm(sender, e);
        }
        private void PictureBox1_Click(object sender, EventArgs e)
        {
            OpenEditForm(sender, e);
        }

        private void OpenEditForm(object sender, EventArgs e)
        {
            if (!isPopUpFormOpen)
            {
                // Open your pop-up form here
                editBook popUpForm = new editBook();

                // Disable the main form
                this.FindForm().Enabled = false;

                isPopUpFormOpen = true;

                // Set values in the editBook form based on the selected book
                popUpForm.TextBox1.Text = labelTitle.Text;
                popUpForm.TextBox2.Text = labelAuthor.Text;
                popUpForm.TextBox3.Text = labelLocation.Text;
                popUpForm.TextBox4.Text = labelStocks.Text;
                popUpForm.PictureBox1.Image = pictureBox.Image;
                popUpForm.Label7.Text = labelBookId.Text;
                popUpForm.Button2.BringToFront();

                // Set the genre in the pop-up form
                // Make sure GetGenre returns a valid value

                // Subscribe to the FormClosed event of the pop-up form
                popUpForm.FormClosed += (s, args) =>
                {
                    // Enable the main form when the pop-up form is closed
                    this.FindForm().Enabled = true;
                    isPopUpFormOpen = false;

                    // Optionally, update the label1 text based on editBook form data
                    label1.Text = popUpForm.TextBox1.Text;
                    label3.Text = popUpForm.TextBox2.Text;
                    label4.Text = popUpForm.TextBox3.Text;
                    label6.Text = popUpForm.TextBox4.Text;
                    pictureBox1.Image = popUpForm.PictureBox1.Image;
                };

                popUpForm.ShowDialog();
            }
        }


        // Inside the 'books' class
        public string GetGenre()
        {
            // Assume bookId is known, replace it with the actual bookId value
            string bookId = labelBookId.Text;

            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            // Use parameterized query to prevent SQL injection
            string selectQuery = "SELECT Book_Genre FROM library_of_life.books WHERE Book_Id = @BookId";

            using (MySqlConnection conDatabase = new MySqlConnection(constring))
            using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, conDatabase))
            {
                // Add parameter
                selectCommand.Parameters.AddWithValue("@BookId", bookId);

                try
                {
                    conDatabase.Open();
                    // ExecuteScalar is used for fetching a single value
                    object result = selectCommand.ExecuteScalar();

                    if (result != null)
                    {
                        // Convert the result to string (assuming Book_Genre is a string)
                        return result.ToString();
                    }
                    else
                    {
                        // Handle the case when the genre is not found
                        return "Genre not found";
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception
                    return $"Error: {ex.Message}";
                }
            }
        }
        



       





    }
}
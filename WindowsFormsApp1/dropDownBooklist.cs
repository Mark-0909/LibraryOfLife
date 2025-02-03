using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Windows.Forms;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Borders;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Font;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Utils;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Globalization;

namespace WindowsFormsApp1
{
    public partial class dropDownBooklist : UserControl
    {
        private string ReferenceID;

        public string memberID;

        public string BorrowedDate;
        public string ReturnDate;

        public string returned;

        public string Vio;

        public string ReturnedDate;

        public dropDownBooklist()
        {
            InitializeComponent();
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
        }

        public dropDownBooklist(string referenceID) : this()
        {
            ReferenceID = referenceID;
            FetchBookData();
            GetDates(referenceID);
        }

        public void GetDates(string referenceID)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();

                    // Select Borrowed Date and Return Date based on Reference_ID
                    string query = "SELECT Borrowed_Date, Return_Date, member_ID FROM borrowedBook WHERE Reference_ID = @ReferenceID";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    cmdDatabase.Parameters.AddWithValue("@ReferenceID", referenceID);

                    using (MySqlDataReader reader = cmdDatabase.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Correcting the parsing and formatting of Borrowed_Date
                            DateTime bor = DateTime.Parse(reader["Borrowed_Date"].ToString());
                            BorrowedDate = bor.ToString("MM-dd-yyyy");

                            // Correcting the parsing and formatting of Return_Date
                            DateTime ret = DateTime.Parse(reader["Return_Date"].ToString());
                            ReturnDate = ret.ToString("MM-dd-yyyy");

                            memberID = reader["member_ID"].ToString();
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching dates: {ex.Message}");
                }
            }
        }

        public void FetchBookData()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();

                    if (!int.TryParse(ReferenceID, out int referenceID))
                    {
                        MessageBox.Show("Invalid reference ID format. Please enter a valid integer value.");
                        return;
                    }

                    string query = "SELECT * FROM borrowlist WHERE Reference_ID = @ReferenceID";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    cmdDatabase.Parameters.AddWithValue("@ReferenceID", referenceID);

                    using (MySqlDataReader reader = cmdDatabase.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FetchBookDetailsAndAddToFlowLayoutPanel(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void FetchBookDetailsAndAddToFlowLayoutPanel(MySqlDataReader reader)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                connection.Open();

                int bookIDFromList = Convert.ToInt32(reader["Book_List"]);
                string bookRemark = Convert.ToString(reader["Book_Remarks"]);
                string borrowListID = Convert.ToString(reader["ID"]);

                string bookDetailsQuery = "SELECT Book_Id, Book_Name, Image_Path FROM books WHERE Book_Id = @BookId";
                MySqlCommand bookDetailsCmd = new MySqlCommand(bookDetailsQuery, connection);
                bookDetailsCmd.Parameters.AddWithValue("@BookId", bookIDFromList);

                using (MySqlDataReader bookDetailsReader = bookDetailsCmd.ExecuteReader())
                {
                    if (reader["Status"].ToString() == "Borrowed")
                    {
                        while (bookDetailsReader.Read())
                        {
                            string bookId = bookDetailsReader["Book_Id"].ToString();
                            string bookName = bookDetailsReader["Book_Name"].ToString();
                            string imageData = bookDetailsReader["Image_Path"].ToString();

                            borrowedBookList borrowedBook = new borrowedBookList(imageData, borrowListID, bookIDFromList.ToString(), bookName, bookRemark);
                            flowLayoutPanel1.Controls.Add(borrowedBook);
                        }
                    }
                    else
                    {
                        while (bookDetailsReader.Read())
                        {
                            string newRemark = reader["New_Remarks"].ToString();
                            string status = reader["Status"].ToString();
                            string violation = reader["Violation"].ToString();
                            
                            string returned = reader["Returned_Date"].ToString();
                            

                            string completeString = $"{status}\n{returned}\n{violation}";

                            string bookId = bookDetailsReader["Book_Id"].ToString();
                            string bookName = bookDetailsReader["Book_Name"].ToString();
                            string imageData = bookDetailsReader["Image_Path"].ToString();

                            returnDisplayLayout returnedBook = new returnDisplayLayout(imageData, borrowListID, bookIDFromList.ToString(), bookName, newRemark, completeString);
                            flowLayoutPanel1.Controls.Add(returnedBook);
                        }
                    }
                }
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            GenerateReceipt();
            
        }

        

        private void GenerateReceipt()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                FileName = $"{ReferenceID}.pdf"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string outputFilePath = saveFileDialog.FileName;
                string backgroundImagePath = @"Resources\receipt.png";

                using (FileStream fs = new FileStream(outputFilePath, FileMode.Create))
                {
                    using (PdfWriter writer = new PdfWriter(fs))
                    {
                        using (PdfDocument pdf = new PdfDocument(writer))
                        {
                            // Set the page size
                            PageSize pageSize = new PageSize(223f, 472f); //original size 130f, 250f
                            pdf.SetDefaultPageSize(pageSize);

                            // Set document margins to zero
                            Document document = new Document(pdf);
                            document.SetMargins(0, 0, 0, 0);

                            // Load the background image
                            iText.Layout.Element.Image backgroundImage = new iText.Layout.Element.Image(ImageDataFactory.Create(backgroundImagePath));

                            // Set the background image size to match the page size
                            backgroundImage.ScaleToFit(pageSize.GetWidth(), pageSize.GetHeight());

                            // Set the background image for the entire document
                            document.Add(backgroundImage);

                            // Use PdfCanvas to draw the "Hello, World!" content on top of the background image
                            PdfCanvas canvas = new PdfCanvas(pdf.GetFirstPage());

                            // Add header to the document
                            AddHeaderToPdfDocument(document, ReferenceID, BorrowedDate, ReturnDate, memberID);

                            // Add text directly to the document with PdfCanvas
                            AddTextToPdfCanvas(canvas, $"{ReferenceID}", 5, 395);
                            AddTextToPdfCanvas(canvas, $"{memberID}", 59, 395);
                            AddTextToPdfCanvas(canvas, $"{BorrowedDate}", 121, 395);
                            AddTextToPdfCanvas(canvas, $"{ReturnDate}", 173, 395);
                            

                            float initialY = 350; // Initial y coordinate

                            foreach (Control control in flowLayoutPanel1.Controls)
                            {
                                if (control is borrowedBookList borrowedBook)
                                {
                                    string bookId = borrowedBook.Label1.Text;
                                    string bookName = borrowedBook.Label2.Text;
                                    string remarks = borrowedBook.Label4.Text;
                                    string remarks1 = borrowedBook.Label8.Text;
                                    string remarks2 = borrowedBook.Label6.Text;
                                    string remarks3 = borrowedBook.Label7.Text;
                                    if (borrowedBook.Button1.Visible == false) 
                                    {
                                        // Add returned book details to the document
                                        AddTextToPdfCanvas1(canvas, $"{bookId}", 17, initialY + 6, 0);
                                        AddTextToPdfCanvas1(canvas, $"{bookName}", 76, initialY, 60); // Adjusted spacing
                                        AddTextToPdfCanvas1(canvas, $"{remarks1}", 152, initialY, 60);
                                        AddTextToPdfCanvas1(canvas, $"{remarks2}", 152, initialY - 7, 60);
                                        AddTextToPdfCanvas1(canvas, $"{remarks3}", 152, initialY - 15, 50);
                                    } else
                                    {
                                        // Add book details to the document
                                        AddTextToPdfCanvas1(canvas, $"{bookId}", 17, initialY + 6, 0);
                                        AddTextToPdfCanvas1(canvas, $"{bookName}", 76, initialY, 60); // Adjusted spacing
                                        AddTextToPdfCanvas1(canvas, $"{remarks}", 152, initialY, 60); // Adjusted spacing
                                    }
                                    

                                    

                                    initialY -= 40; // Increment y coordinate
                                }
                                else if (control is returnDisplayLayout returnedBook)
                                {
                                    getViolations(returnedBook.Label3.Text);
                                    string bookId = returnedBook.Label1.Text;
                                    string bookName = returnedBook.Label2.Text;
                                    string remarks1 = "Returned";
                                    string remarks2 = ReturnedDate.ToString();
                                    string remarks3 = Vio.ToString();

                                    // Add returned book details to the document
                                    AddTextToPdfCanvas1(canvas, $"{bookId}", 22, initialY + 6, 0);
                                    AddTextToPdfCanvas1(canvas, $"{bookName}", 76, initialY, 60); // Adjusted spacing
                                    AddTextToPdfCanvas1(canvas, $"{remarks1}", 152, initialY, 60);
                                    AddTextToPdfCanvas1(canvas, $"{remarks2}", 152, initialY-7, 60);
                                    AddTextToPdfCanvas1(canvas, $"{remarks3}", 152, initialY-15, 50);

                                    initialY -= 40; // Increment y coordinate
                                }
                            }


                        }
                    }
                }

                OpenPdfFile(outputFilePath);

                Console.WriteLine($"PDF file generated successfully at: {outputFilePath}");
            }
            else
            {
                Console.WriteLine("PDF generation canceled.");
            }
        }

        

public void getViolations(string borrowListID)
    {
        // Connection string for your MySQL database
        string connectionString = "server=localhost;port=3306;user=root;password=;database=library_of_life";

        // SQL query to retrieve Returned_Date and Violation for the given borrowListID
        string query = "SELECT Returned_Date, Violation FROM borrowlist WHERE ID = @BorrowListID";

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Add a parameter for the borrowListID
                    command.Parameters.AddWithValue("@BorrowListID", borrowListID);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                                
                                ReturnedDate = reader["Returned_Date"].ToString();

                                Vio = reader["Violation"].ToString();

                            // Now you can use the values as needed
                        }
                        else
                        {
                            Console.WriteLine("No data found for the specified borrowListID.");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }



    private void AddTextToPdfCanvas1(PdfCanvas canvas, string text, float x, float y, float maxWidth)
        {
            // Define the font and size
            PdfFont font = PdfFontFactory.CreateFont();
            float fontSize = 8;

            

            // Split the text into lines based on the width and handle word wrapping
            IList<String> lines = new List<String>();
            string[] words = text.Split(' ');
            StringBuilder currentLine = new StringBuilder();
            foreach (string word in words)
            {
                if (font.GetWidth(currentLine.ToString() + " " + word, fontSize) < maxWidth)
                {
                    currentLine.Append(word).Append(" ");
                }
                else
                {
                    lines.Add(currentLine.ToString().Trim());
                    currentLine = new StringBuilder(word + " ");
                }
            }
            lines.Add(currentLine.ToString().Trim());

            // Add the text lines to the canvas
            float lineHeight = 9; // Adjust this value based on your preference
            foreach (string line in lines)
            {
                canvas.BeginText()
                    .SetFontAndSize(font, fontSize)
                    .MoveText(x, y)
                    .ShowText(line)
                    .EndText();
                y -= lineHeight; // Move to the next line
            }
        }









        private void AddTextToPdfCanvas(PdfCanvas canvas, string text, float x, float y)
        {
            canvas.BeginText()
                .SetFontAndSize(PdfFontFactory.CreateFont(), 8)
                .MoveText(x, y)
                .ShowText(text)
                .EndText();
        }

        private void AddHeaderToPdfDocument(Document document, string referenceID, string borrowedDate, string returnDate, string memID)
        {
            // ... (your existing code for adding the header)
        }

        



        



        private static void OpenPdfFile(string filePath)
        {
            try
            {
                System.Diagnostics.Process.Start(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening the file: {ex.Message}");
            }
        }


        


    }
}

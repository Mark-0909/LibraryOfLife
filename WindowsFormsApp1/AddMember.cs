using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;
using static WindowsFormsApp1.memberlist;
namespace WindowsFormsApp1
{
    public partial class AddMember : Form
    {

        int lastInsertedId;
        string pdfFilePath;
        public memberlist memberlistControl; // Make sure this is set before calling FetchDataFromAddMember
        string pdfDirectoryPath = @"C:\Users\orcul\source\repos\WindowsFormsApp1\WindowsFormsApp1\memberID";

        

        public string TextBox1Text => textBox1.Text;
        public string TextBox2Text => textBox2.Text;
        public string TextBox3Text => textBox3.Text;
        public int LastInsertedId { get; private set; }
        public memberlist MemberListForm { get; set; }


        public string Strmode { get; private set; }


        public AddMember()
        {
            InitializeComponent();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            string query = "INSERT INTO library_of_life.members (First_Name, Last_Name, MI, Age, Address, Contact_Number, Email_Address, PDF_FilePath, Registration_Year, Registration_Date) " +
                            "VALUES(@FirstName, @LastName, @MI, @Age, @Address, @ContactNumber, @EmailAddress, @PDFFilePath, @RegistrationYear, @RegistrationDate); SELECT LAST_INSERT_ID();";
                      using (MySqlConnection conDatabase = new MySqlConnection(constring))
            {
                using (MySqlCommand cmdDatabase = new MySqlCommand(query, conDatabase))
                {
                    try
                    {
                        conDatabase.Open();

                        int year = DateTime.Now.Year;
                        DateTime currentDate = DateTime.Now;

                        cmdDatabase.Parameters.AddWithValue("@RegistrationYear", year);
                        cmdDatabase.Parameters.AddWithValue("@RegistrationDate", currentDate.ToString("MM/dd/yyyy")); // Format the date as "MM/dd/yyyy"
                        cmdDatabase.Parameters.AddWithValue("@PDFFilePath", ""); // Set a default value

                        cmdDatabase.Parameters.AddWithValue("@FirstName", textBox1.Text);
                        cmdDatabase.Parameters.AddWithValue("@LastName", textBox2.Text);
                        cmdDatabase.Parameters.AddWithValue("@MI", textBox3.Text);
                        cmdDatabase.Parameters.AddWithValue("@Age", textBox4.Text);
                        cmdDatabase.Parameters.AddWithValue("@Address", textBox5.Text);
                        cmdDatabase.Parameters.AddWithValue("@ContactNumber", textBox6.Text);
                        cmdDatabase.Parameters.AddWithValue("@EmailAddress", textBox7.Text);
            
                        object result = cmdDatabase.ExecuteScalar();

                        if (result != null)
                        {
                            lastInsertedId = Convert.ToInt32(result);
                             
                            pdfFilePath = Path.Combine(pdfDirectoryPath, $"{year}00{lastInsertedId}.pdf"); // Update the file path with the correct lastInsertedId

                            using (FileStream fs = new FileStream(pdfFilePath, FileMode.Create))
                            {
                                using (Document document = new Document())
                                {
                                    PdfWriter writer = PdfWriter.GetInstance(document, fs);
                                    document.Open();

                                    // Apply PDF design for the first page
                                    ApplyFirstPageDesign(document);

                                    document.NewPage();  // Add a new page

                                    // Apply PDF design for the second page
                                    ApplySecondPageDesign(document, lastInsertedId, writer);

                                    document.Close();
                                }
                            }



                            MessageBox.Show($"Member added successfully! Member ID: {lastInsertedId}");
                            Process.Start("msedge.exe", pdfFilePath);
                            
                        }
                        else
                        {
                            MessageBox.Show("Failed to add a member. Please check your input.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}\n\nDetails:\n{ex.ToString()}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }









        
        private void button4_Click(object sender, EventArgs e)
        {
            // Close the current form
            this.Close();
            

        }












        

        // Method to apply PDF design for the first page
        // Method to apply PDF design for the first page
        private void ApplyFirstPageDesign(Document document)
        {
            // Set page size to ID size and landscape orientation
            document.SetPageSize(new iTextSharp.text.Rectangle(2.125f * 72, 3.375f * 72));
            document.SetPageSize(document.PageSize.Rotate());

            // Set page margins to zero
            document.SetMargins(0, 0, 0, 0);

            // Create a new page with the specified size and orientation
            document.NewPage();

            // Resize the background image with a specific height
            string backgroundImagePath = @"C:\Users\orcul\source\repos\WindowsFormsApp1\WindowsFormsApp1\Resources\Slide1.JPG";
            iTextSharp.text.Image backgroundImage = iTextSharp.text.Image.GetInstance(backgroundImagePath);
            backgroundImage.SetAbsolutePosition(0, 0);
            backgroundImage.ScaleAbsolute(document.PageSize.Width, document.PageSize.Height);  // Set the desired height here

            // Set the background image
            document.Add(backgroundImage);

            // Add all information to page 1

        }





        // Method to apply PDF design for the second page
        private void ApplySecondPageDesign(Document document, int memberId, object writerObject)
        {
            // Cast the object to PdfWriter
            PdfWriter writer = (PdfWriter)writerObject;

            // Set page size to ID size and landscape orientation
            document.SetPageSize(new iTextSharp.text.Rectangle(2.125f * 72, 3.375f * 72));
            document.SetPageSize(document.PageSize.Rotate());

            // Set page margins to zero
            document.SetMargins(0, 0, 0, 0);

            // Resize the background image with a specific height
            string backgroundImagePath = @"C:\Users\orcul\source\repos\WindowsFormsApp1\WindowsFormsApp1\Resources\Slide2.JPG";
            iTextSharp.text.Image backgroundImage = iTextSharp.text.Image.GetInstance(backgroundImagePath);
            backgroundImage.SetAbsolutePosition(0, 0);
            backgroundImage.ScaleAbsolute(document.PageSize.Width, document.PageSize.Height);  // Set the desired height here

            // Set the background image
            document.Add(backgroundImage);

            // Add all information to page 1


            // Add additional information with different fonts and positions
            Font font1 = FontFactory.GetFont(FontFactory.HELVETICA, 7, BaseColor.BLACK);
            Font font2 = FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.WHITE);


            PdfContentByte canvas = writer.DirectContent;
            DateTime currentDate = DateTime.Now;
            string yearString = currentDate.Year.ToString();
            string result = yearString + "00";
            canvas.BeginText();
            canvas.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 21);
            canvas.SetColorFill(BaseColor.WHITE);
            canvas.SetTextMatrix(20, 125);  // Adjust the coordinates as needed
            canvas.ShowText($"{result}{memberId}");
            canvas.EndText();

            // Adjust the coordinates as needed
            canvas.BeginText();
            canvas.SetFontAndSize(font2.BaseFont, font2.Size);
            canvas.SetColorFill(BaseColor.WHITE);
            canvas.SetTextMatrix(22, 110);
            canvas.ShowText($"{textBox2.Text}, {textBox1.Text} {textBox3.Text}");
            canvas.EndText();




            canvas.BeginText();
            canvas.SetFontAndSize(font1.BaseFont, font1.Size);
            canvas.SetColorFill(BaseColor.BLACK);
            canvas.SetTextMatrix(35, 30);
            canvas.ShowText(textBox5.Text);
            canvas.EndText();

            canvas.BeginText();
            canvas.SetFontAndSize(font1.BaseFont, font1.Size);
            canvas.SetColorFill(BaseColor.BLACK);
            canvas.SetTextMatrix(155, 55);
            canvas.ShowText(textBox6.Text);
            canvas.EndText();

            canvas.BeginText();
            canvas.SetFontAndSize(font1.BaseFont, font1.Size);
            canvas.SetColorFill(BaseColor.BLACK);
            canvas.SetTextMatrix(35, 55);
            canvas.ShowText(textBox7.Text);
            canvas.EndText();
        }


    }
}

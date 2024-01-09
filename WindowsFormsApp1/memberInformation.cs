using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MySqlX.XDevAPI.Common;
using System.Diagnostics;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class memberInformation : UserControl
    {
        string pdfFilePath;
        borrowBook BorrowBook;
        memberBorrow memberBorrow;
        public Label Label1
        {
            get { return label9; }
            set { label9 = value; }
        }

        public Label Label2
        {
            get { return label10; }
            set { label10 = value; }
        }

        public Label Label3
        {
            get { return label11; }
            set { label11 = value; }
        }

        public Label Label4
        {
            get { return label12; }
            set { label12 = value; }
        }

        public Label Label5
        {
            get { return label13; }
            set { label13 = value; }
        }

        public Label Label6
        {
            get { return label14; }
            set { label14 = value; }
        }
        public Label labelId, labelName, labelAge, labelAddress, labelPhone, labelEmail;
        private bool isPopUpFormOpen = false;
        public string FN;
        public string LN;
        public string MI;

       

        public memberInformation()
        {
            InitializeComponent();
            


        }

        public memberInformation(string memberID, string firstName, string lastName, string mi, int age, string address, string contactNumber, string emailAddress)
            : this()
        {
            DisplayMemberDetails(memberID, firstName, lastName, mi, age, address, contactNumber, emailAddress);
        }
        
        public void DisplayMemberDetails(string memberID, string firstName, string lastName, string mi, int age, string address, string contactNumber, string emailAddress)
        {
            label9.Text = $"{memberID}";
            label10.Text = $"{lastName}, {firstName} {mi}";
            label11.Text = $"{age}";
            label12.Text = $"{address}";
            label13.Text = $"{contactNumber}";
            label14.Text = $"{emailAddress}";
            FN = $"{firstName}";
            LN = $"{lastName}";
            MI = $"{mi}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenBorrowForm(sender, e);

            
        }


        private void button2_Click(object sender, EventArgs e)
        {
            OpenEditForm(sender, e);
        }


        private void OpenBorrowForm(object sender, EventArgs e)
        {
            if (!isPopUpFormOpen)
            {
                // Create a reference to the existing instance of borrowBook
                BorrowBook = new borrowBook();
                
                // Set the memberID on the existing instance
                BorrowBook.SetmemberID(label9.Text);
                


                this.FindForm().Enabled = false;

                isPopUpFormOpen = true;

                BorrowBook.FormClosed += (s, args) =>
                {
                    this.FindForm().Enabled = true;
                    isPopUpFormOpen = false;
                };

                BorrowBook.ShowDialog();
            }
        }


        private void OpenEditForm(object sender, EventArgs e)
        {
            if (!isPopUpFormOpen)
            {
                EditMember editMember = new EditMember();

                editMember.SetMemberInformationForm(this);

                this.FindForm().Enabled = false;

                isPopUpFormOpen = true;
                editMember.TextBox1.Text = FN;
                editMember.TextBox2.Text = LN;
                editMember.TextBox3.Text = MI;
                editMember.TextBox4.Text = labelAddress.Text;
                editMember.TextBox5.Text = labelPhone.Text;
                editMember.TextBox6.Text = labelEmail.Text;
                editMember.TextBox7.Text = labelAge.Text;
                editMember.Label10.Text = labelId.Text;

                editMember.FormClosed += (s, args) =>
                {
                    this.FindForm().Enabled = true;
                    isPopUpFormOpen = false;
                };

                editMember.ShowDialog();
            }
        }
        

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string pdfDirectoryPath = @"C:\Users\orcul\source\repos\WindowsFormsApp1\WindowsFormsApp1\memberID";
                string pdfFileName = $"{labelId.Text}.pdf";
                pdfFilePath = Path.Combine(pdfDirectoryPath, pdfFileName);

                if (File.Exists(pdfFilePath))
                {
                    // If the PDF file already exists, ask the user if they want to replace it
                    DialogResult result = MessageBox.Show("The PDF file already exists. Do you want to replace it?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
                    if (result == DialogResult.No)
                    {
                        return; // User chose not to replace the existing file
                    }
                }

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
                    saveFileDialog.FileName = pdfFileName;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        pdfFilePath = saveFileDialog.FileName;

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
                                ApplySecondPageDesign(document, labelId.Text, writer);

                                document.Close();
                            }
                        }

                        MessageBox.Show($"PDF saved successfully to: {pdfFilePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Open the saved PDF file using Microsoft Edge
                        Process.Start("msedge.exe", pdfFilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ... (your existing code)



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
        private void ApplySecondPageDesign(Document document, string memberId, object writerObject)
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
            
            
            canvas.BeginText();
            canvas.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 21);
            canvas.SetColorFill(BaseColor.WHITE);
            canvas.SetTextMatrix(20, 125);  // Adjust the coordinates as needed
            canvas.ShowText($"{labelId.Text}");
            canvas.EndText();

            // Adjust the coordinates as needed
            canvas.BeginText();
            canvas.SetFontAndSize(font2.BaseFont, font2.Size);
            canvas.SetColorFill(BaseColor.WHITE);
            canvas.SetTextMatrix(22, 110);
            canvas.ShowText($"{LN}, {FN} {MI}");
            canvas.EndText();




            canvas.BeginText();
            canvas.SetFontAndSize(font1.BaseFont, font1.Size);
            canvas.SetColorFill(BaseColor.BLACK);
            canvas.SetTextMatrix(35, 27);
            canvas.ShowText(labelAddress.Text);
            canvas.EndText();

            canvas.BeginText();
            canvas.SetFontAndSize(font1.BaseFont, font1.Size);
            canvas.SetColorFill(BaseColor.BLACK);
            canvas.SetTextMatrix(155, 55);
            canvas.ShowText(labelPhone.Text);
            canvas.EndText();

            canvas.BeginText();
            canvas.SetFontAndSize(font1.BaseFont, font1.Size);
            canvas.SetColorFill(BaseColor.BLACK);
            canvas.SetTextMatrix(35, 55);
            canvas.ShowText(labelEmail.Text);
            canvas.EndText();
        }



    }
}

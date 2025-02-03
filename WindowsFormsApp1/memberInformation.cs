using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MySqlX.XDevAPI.Common;
using System.Diagnostics;
using System.IO;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using iText.Kernel.Colors;


namespace WindowsFormsApp1
{
    public partial class memberInformation : UserControl
    {
        string pdfFilePath;
        borrowBook BorrowBook;
        
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
        public int displayedViolation = 0;
       

        public memberInformation()
        {
            InitializeComponent();
            
        }

        
        public memberInformation(string memberID, string firstName, string lastName, string mi, int age, string address, string contactNumber, string emailAddress, string presentedID)
            : this()
        {
            DisplayMemberDetails(memberID, firstName, lastName, mi, age, address, contactNumber, emailAddress, presentedID);
        }
        
        public void DisplayMemberDetails(string memberID, string firstName, string lastName, string mi, int age, string address, string contactNumber, string emailAddress, string presentedID)
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

            label20.Text = presentedID;
             


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
                editMember.TextBox4.Text = label12.Text;
                editMember.TextBox5.Text = label13.Text;
                editMember.TextBox6.Text = label14.Text;
                editMember.TextBox7.Text = label11.Text;
                editMember.Label10.Text = label9.Text;

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
            FileStream fs = null;  // Declare FileStream outside the try block

            try
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
                    saveFileDialog.FileName = $"{label9.Text}.pdf";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string pdfFilePath = saveFileDialog.FileName;

                        fs = new FileStream(pdfFilePath, FileMode.Create);  // Initialize FileStream

                        using (Document document = new Document())
                        {
                            PdfWriter writer = PdfWriter.GetInstance(document, fs);
                            document.Open();

                            // Apply PDF design for the first page
                            ApplyFirstPageDesign(document);

                            document.NewPage();  // Add a new page

                            // Apply PDF design for the second page
                            ApplySecondPageDesign(document, writer);

                            document.Close();
                        }

                        // Close the file stream before copying or opening the file
                        fs.Close();

                        // Open the saved file
                        openFile(pdfFilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Ensure that the file stream is closed in case of an exception
                fs?.Close();
            }
        }

        public void openFile(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                // Open the saved file in Microsoft Edge
                Process.Start("msedge.exe", filePath);
            }
        }


        private void ApplyFirstPageDesign(Document document)
        {
            try
            {
                // Set page size to ID size and landscape orientation
                document.SetPageSize(new iTextSharp.text.Rectangle(2.125f * 72, 3.375f * 72));
                document.SetPageSize(document.PageSize.Rotate());

                // Set page margins to zero
                document.SetMargins(0, 0, 0, 0);

                // Create a new page with the specified size and orientation
                document.NewPage();

                // Resize the background image with a specific height
                string backgroundImagePath = @"Resources\Slide1.JPG";
                string fullPath = Path.Combine(Application.StartupPath, backgroundImagePath);

                iTextSharp.text.Image backgroundImage = iTextSharp.text.Image.GetInstance(fullPath);
                backgroundImage.SetAbsolutePosition(0, 0);
                backgroundImage.ScaleAbsolute(document.PageSize.Width, document.PageSize.Height);

                // Set the background image
                document.Add(backgroundImage);

                // Add all information to page 1
                // Add content to the first page if needed
                // ...
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ApplyFirstPageDesign: {ex.Message}");
            }
        }

        private void ApplySecondPageDesign(Document document, object writerObject)
        {
            try
            {
                // Cast the object to PdfWriter
                PdfWriter writer = (PdfWriter)writerObject;

                // Set page size to ID size and landscape orientation
                document.SetPageSize(new iTextSharp.text.Rectangle(2.125f * 72, 3.375f * 72));
                document.SetPageSize(document.PageSize.Rotate());

                // Set page margins to zero
                document.SetMargins(0, 0, 0, 0);

                // Resize the background image with a specific height
                string backgroundImagePath = @"Resources\Slide2.JPG";
                string fullPath = Path.Combine(Application.StartupPath, backgroundImagePath);

                iTextSharp.text.Image backgroundImage = iTextSharp.text.Image.GetInstance(fullPath);
                backgroundImage.SetAbsolutePosition(0, 0);
                backgroundImage.ScaleAbsolute(document.PageSize.Width, document.PageSize.Height);

                // Set the background image
                document.Add(backgroundImage);

                // Add additional information with different fonts and positions
                Font font1 = FontFactory.GetFont(FontFactory.HELVETICA, 7, BaseColor.BLACK);
                Font font2 = FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.WHITE);

                PdfContentByte canvas = writer.DirectContent;
                DateTime currentDate = DateTime.Now;
                string yearString = currentDate.Year.ToString();
                string result = yearString + "00";

                // Member ID
                canvas.BeginText();
                canvas.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 21);
                canvas.SetColorFill(BaseColor.WHITE);
                canvas.SetTextMatrix(20, 125);  // Adjust the coordinates as needed
                canvas.ShowText($"{label9.Text}");
                canvas.EndText();

                // Name
                canvas.BeginText();
                canvas.SetFontAndSize(font2.BaseFont, font2.Size);
                canvas.SetColorFill(BaseColor.WHITE);
                canvas.SetTextMatrix(22, 110);
                canvas.ShowText($"{LN}, {FN} {MI}");
                canvas.EndText();

                // Address
                canvas.BeginText();
                canvas.SetFontAndSize(font1.BaseFont, font1.Size);
                canvas.SetColorFill(BaseColor.BLACK);
                canvas.SetTextMatrix(35, 30);
                canvas.ShowText(label12.Text);
                canvas.EndText();

                // Contact Number
                canvas.BeginText();
                canvas.SetFontAndSize(font1.BaseFont, font1.Size);
                canvas.SetColorFill(BaseColor.BLACK);
                canvas.SetTextMatrix(155, 55);
                canvas.ShowText(label13.Text);
                canvas.EndText();

                // Email Address
                canvas.BeginText();
                canvas.SetFontAndSize(font1.BaseFont, font1.Size);
                canvas.SetColorFill(BaseColor.BLACK);
                canvas.SetTextMatrix(35, 55);
                canvas.ShowText(label14.Text);
                canvas.EndText();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ApplySecondPageDesign: {ex.Message}");
            }
        }



        private string memID;

        public void setmemberID(string memberID)
        {
            memID = memberID;
            IsMemberBanned(memID);
        }

        public bool IsMemberBanned(string memID)
{
    // Remove the first 4 letters from memID and convert the rest to an integer
    if (memID.Length > 4 && int.TryParse(memID.Substring(4), out int numericMemID))
    {
        string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

        using (MySqlConnection connection = new MySqlConnection(constring))
        {
            try
            {
                connection.Open();

                string query = "SELECT Status FROM members WHERE ID = @NumericMemID";

                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@NumericMemID", numericMemID);

                object result = cmd.ExecuteScalar();

                // If the result is not null and is equal to 'Banned', return true
                if (result != null && result.ToString() == "Banned")
                {
                    fetchBannedMember(memID);

                    label18.Text = "10";
                    button1.Enabled = false;
                    button2.Enabled = false;
                    panel1.BackColor = System.Drawing.Color.LightCoral;
                    

                    // Disable other controls on the main form if needed
                    // For example: this.textBox1.Enabled = false;

                    bannedMemberPopup banpopup = new bannedMemberPopup();
                    banpopup.FormClosed += (s, e) =>
                    {
                        this.Enabled = true; // Re-enable the main form
                    };

                    this.Enabled = false; // Disable the main form
                    banpopup.ShowDialog();

                    return true;
                }
                else
                {
                    refreshViolation();
                            
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }
    }
    else
    {
        MessageBox.Show("Invalid member ID format. Unable to check ban status.");
        return false;
    }
}

        


        public void fetchBannedMember(string memberID)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();

                    // Assuming 'member_ID' in 'banned_members' table is the same as 'ID' in 'members' table
                    string query = "SELECT Reference_ID, Book_ID, Violations FROM banned_members WHERE member_ID = @MemberID";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@MemberID", memberID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Clear existing controls in the flowLayoutPanel1
                        flowLayoutPanel1.Controls.Clear();

                        // Iterate through the results and create violationLayout controls for each row
                        while (reader.Read())
                        {
                            string referenceID = reader["Reference_ID"].ToString();
                            string bookID = reader["Book_ID"].ToString();
                            string violations = reader["Violations"].ToString();

                            // Create a violationLayout control for each row
                            violationLayout violationLayout = new violationLayout(referenceID, bookID, violations);

                            // Add the violationLayout control to the flowLayoutPanel1
                            flowLayoutPanel1.Controls.Add(violationLayout);
                        }

                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        public void refreshViolation()
        {
            flowLayoutPanel1.Controls.Clear();
            displayedViolation = 0; // Clear the counter
            List<string> refIds = new List<string>();
            List<string> bookIds = new List<string>();
            List<string> violations = new List<string>();

            // Retrieve violations and populate lists
            Violations(refIds, bookIds, violations);

            // Add controls based on the refreshed data
            for (int i = 0; i < refIds.Count; i++)
            {
                violationLayout violationLayout = new violationLayout(refIds[i], bookIds[i], violations[i]);
                flowLayoutPanel1.Controls.Add(violationLayout);

                // Increment displayedViolation only when a violation is displayed
                displayedViolation++;
                label18.Text = displayedViolation.ToString();

                if (label18.Text == "10")
                {
                    
                    button1.Enabled = false;
                    button2.Enabled = false;

                    string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

                    using (MySqlConnection connection = new MySqlConnection(constring))
                    {
                        try
                        {
                            connection.Open();

                            string bannedDate = DateTime.Now.ToString("yyyy-MM-dd"); // Assuming you want to store the current date

                            string query = "INSERT INTO banned_members (member_ID, Banned_Date, Reference_ID, Book_ID, Violations) " +
                                           "VALUES (@MemberID, @BannedDate, @ReferenceID, @BookID, @Violations)";

                            for (int j = 0; j < 10; j++)
                            {
                                MySqlCommand cmd = new MySqlCommand(query, connection);

                                // Add parameters
                                cmd.Parameters.AddWithValue("@MemberID", memID); // Use memID here
                                cmd.Parameters.AddWithValue("@BannedDate", bannedDate);

                                // Use specific index for each list
                                cmd.Parameters.AddWithValue("@ReferenceID", refIds[j]);
                                cmd.Parameters.AddWithValue("@BookID", bookIds[j]);
                                cmd.Parameters.AddWithValue("@Violations", violations[j]);

                                // Execute the query
                                cmd.ExecuteNonQuery();
                            }

                            // Change the status to 'Banned' in the 'members' table
                            ChangeMemberStatusToBanned(memID);
                            panel1.BackColor = System.Drawing.Color.LightCoral;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error: {ex.Message}");
                        }
                    }
                } else if (label18.Text == "5" || label18.Text == "6" || label18.Text == "7"){
                    panel1.BackColor = System.Drawing.Color.PaleGoldenrod; // Pastel Yellow
                } else if (label18.Text == "8" || label18.Text == "9")
                {
                    panel1.BackColor = System.Drawing.Color.LightCoral;
                }
            }
        }


        public void ChangeMemberStatusToBanned(string memID)
        {
            // Remove the first 4 letters from memID and convert the rest to an integer
            if (memID.Length > 4 && int.TryParse(memID.Substring(4), out int numericMemID))
            {
                string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

                using (MySqlConnection connection = new MySqlConnection(constring))
                {
                    try
                    {
                        connection.Open();

                        string query = "UPDATE members SET Status = 'Banned' WHERE ID = @NumericMemID";

                        MySqlCommand cmd = new MySqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@NumericMemID", numericMemID);

                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Invalid member ID format. Unable to change status to 'Banned'.");
            }
        }

        

        public void Violations(List<string> refIds, List<string> bookIds, List<string> violations)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();

                    if (!int.TryParse(memID, out int member_ID))
                    {
                        MessageBox.Show("Invalid member ID format. Please enter a valid integer value.");
                        return;
                    }

                    string query = "SELECT * FROM borrowedbook WHERE member_ID = @MemberID";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    cmdDatabase.Parameters.AddWithValue("@MemberID", member_ID);

                    using (MySqlDataReader reader = cmdDatabase.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            getViolations(reader, refIds, bookIds, violations);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        public void getViolations(MySqlDataReader filterReader, List<string> refIds, List<string> bookIds, List<string> violations)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";
            string refid = filterReader["Reference_ID"].ToString();

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                connection.Open();

                string query = "SELECT Book_List, Violation, Returned_Date FROM borrowlist WHERE Reference_ID = @refID";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@refID", refid);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            // Check if Returned_Date is present and not null
                            if (!reader.IsDBNull(reader.GetOrdinal("Returned_Date")))
                            {
                                // Try parsing the returned date
                                if (DateTime.TryParse(reader["Returned_Date"].ToString(), out DateTime returnedDate))
                                {
                                    DateTime removingtime = returnedDate.AddDays(30);
                                    int daydiffernce = (removingtime - DateTime.Now).Days;
                                    if (reader["Violation"].ToString() != " " && daydiffernce >= 0)
                                    {
                                        // Use filterReader for Reference_ID and Book_List, and reader for member_ID and Violation
                                        refIds.Add(filterReader["Reference_ID"].ToString());
                                        bookIds.Add(reader["Book_List"].ToString());
                                        violations.Add(reader["Violation"].ToString());
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
            }
        }




        







    }
}

using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;
using static WindowsFormsApp1.memberlist;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class AddMember : Form
    {

        int lastInsertedId;
        string pdfFilePath;
        public memberlist memberlistControl; // Make sure this is set before calling FetchDataFromAddMember
        

        

        public string TextBox1Text => textBox1.Text;
        public string TextBox2Text => textBox2.Text;
        public string TextBox3Text => textBox3.Text;
        public int LastInsertedId { get; private set; }
        public memberlist MemberListForm { get; set; }


        public string Strmode { get; private set; }

        

       
        
        public AddMember()
        {
            InitializeComponent();

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.SelectedIndex = 0;

        }



        private void button1_Click(object sender, EventArgs e)
        {
            checkAllTextBoxes();
        }

        public void checkAllTextBoxes()
        {
            if (textBox1.Text == string.Empty || textBox2.Text == string.Empty || textBox4.Text == string.Empty || textBox5.Text == string.Empty || textBox6.Text == string.Empty || textBox7.Text == string.Empty || comboBox1.Text == "Select here")
            {
                MessageBox.Show("Please fill in all the information.", "INCOMPLETE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                
                if (!IsTextNumeric(textBox4.Text))
                {
                    MessageBox.Show("Textbox 4 should not contain a letter.", "INCORRECT FORMAT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (!IsTextNumeric(textBox6.Text))
                {
                    MessageBox.Show("Textbox 6 should only contain numeric characters.", "INCORRECT FORMAT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (textBox6.TextLength != 11 || !textBox7.Text.Contains("@"))
                {
                    MessageBox.Show("Incorrect format of Contact number (11 Digits) or Email (Must contain '@').", "INCORRECT FORMAT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    checkIfMemberAlreadyExists();
                }
            }
        }

        

        private bool IsTextNumeric(string text)
        {
            return !text.Any(char.IsLetter);
        }

        public void checkIfMemberAlreadyExists()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT * FROM members WHERE First_Name = @Firstname AND Last_Name = @Lastname AND MI = @MI";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    cmdDatabase.Parameters.AddWithValue("@Firstname", textBox1.Text);
                    cmdDatabase.Parameters.AddWithValue("@Lastname", textBox2.Text);
                    cmdDatabase.Parameters.AddWithValue("@MI", textBox3.Text);

                    using (MySqlDataReader reader = cmdDatabase.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            MessageBox.Show("Member already exists in the database.", "Duplicate Member", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            addMember();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }


        public void addMember()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            string query = "INSERT INTO library_of_life.members (First_Name, Last_Name, MI, Actual_ID, Age, Address, Contact_Number, Email_Address, Registration_Year, Registration_Date, Status, Presented_ID) " +
                            "VALUES (@FirstName, @LastName, @MI, @ID, @Age, @Address, @ContactNumber, @EmailAddress, @RegistrationYear, @RegistrationDate, @status, @presentedID); SELECT LAST_INSERT_ID();";

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
                        cmdDatabase.Parameters.AddWithValue("@FirstName", textBox1.Text);
                        cmdDatabase.Parameters.AddWithValue("@LastName", textBox2.Text);
                        cmdDatabase.Parameters.AddWithValue("@MI", textBox3.Text);
                        cmdDatabase.Parameters.AddWithValue("@Age", textBox4.Text);
                        cmdDatabase.Parameters.AddWithValue("@Address", textBox5.Text);
                        cmdDatabase.Parameters.AddWithValue("@ContactNumber", textBox6.Text);
                        cmdDatabase.Parameters.AddWithValue("@EmailAddress", textBox7.Text);
                        cmdDatabase.Parameters.AddWithValue("@status", "Regular");
                        cmdDatabase.Parameters.AddWithValue("@presentedID", comboBox1.Text);
                        cmdDatabase.Parameters.AddWithValue("@ID", "");


                        object result = cmdDatabase.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            int lastInsertedId = Convert.ToInt32(result);
                            string formattedId = lastInsertedId.ToString().PadLeft(5, '0');
                            string pdfFilePath = $"{DateTime.Now.ToString("yyyy")}{formattedId}.pdf";
                            string forHistoryID = $"{DateTime.Now.ToString("yyyy")}{formattedId}";

                            string updateQuery = "UPDATE members SET Actual_ID = @actualID WHERE ID = @ID;";
                            using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conDatabase))
                            {
                                updateCmd.Parameters.AddWithValue("@actualID", forHistoryID);
                                updateCmd.Parameters.AddWithValue("@ID", lastInsertedId);
                                updateCmd.ExecuteNonQuery();
                            }






                            // Make pdfFilePath consistent with formattedId


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
                                    ApplySecondPageDesign(document, forHistoryID, writer);

                                    document.Close();
                                }
                            }
                            string name = $"{textBox2.Text}, {textBox1.Text} {textBox3.Text}";
                            


                            insertMemHistory(forHistoryID, name, DateTime.Now.ToString("MM-dd-yyyy"));


                            

                            
                            // Open SaveFileDialog after saving the member
                            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                            {
                                saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
                                saveFileDialog.FileName = $"{DateTime.Now.ToString("yyyy")}{formattedId}.pdf";

                                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                                {
                                    // Optionally, you can keep this block if you want to copy the file to the user-selected location
                                    if (saveFileDialog.FileName != pdfFilePath)
                                    {
                                        File.Copy(pdfFilePath, saveFileDialog.FileName);
                                    }

                                    // Open the saved file in Microsoft Edge
                                    Process.Start("msedge.exe", saveFileDialog.FileName);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Failed to add a member. Please check your input.");
                        }

                        // ...


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}\n\nDetails:\n{ex.ToString()}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public void insertMemHistory(string memid, string Name, string regDate)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            string query = "INSERT INTO history_member (member_ID, Initial_Name, Edited_Name, Initial_Age, Edited_Age, Initial_Address, Edited_Address, Initial_Contact, Edited_Contact, Initial_Email, Edited_Email, Date, Remarks) " +
                           "VALUES (@ID, @iname, @ename, @iage, @eage, @iaddress, @eaddress, @icontact, @econtact, @iemail, @eemail, @Date, @Remarks); SELECT LAST_INSERT_ID();";

            using (MySqlConnection conDatabase = new MySqlConnection(constring))
            {
                using (MySqlCommand cmdDatabase = new MySqlCommand(query, conDatabase))
                {
                    try
                    {
                        conDatabase.Open();

                        cmdDatabase.Parameters.AddWithValue("@ID", memid);
                        cmdDatabase.Parameters.AddWithValue("@iname", Name);
                        cmdDatabase.Parameters.AddWithValue("@ename", "NONE");
                        cmdDatabase.Parameters.AddWithValue("@iage", textBox4.Text);
                        cmdDatabase.Parameters.AddWithValue("@eage", "NONE");
                        cmdDatabase.Parameters.AddWithValue("@iaddress", textBox5.Text);
                        cmdDatabase.Parameters.AddWithValue("@eaddress", "NONE");
                        cmdDatabase.Parameters.AddWithValue("@icontact", textBox6.Text);
                        cmdDatabase.Parameters.AddWithValue("@econtact", "NONE");
                        cmdDatabase.Parameters.AddWithValue("@iemail", textBox7.Text);
                        cmdDatabase.Parameters.AddWithValue("@eemail", "NONE");
                        cmdDatabase.Parameters.AddWithValue("@Date", regDate);
                        cmdDatabase.Parameters.AddWithValue("@Remarks", "ADD");

                        int rowsAffected = cmdDatabase.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Member added successfully.");
                            this.Close();
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

        private void ApplySecondPageDesign(Document document, string memberId, object writerObject)
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
                

                // Member ID
                canvas.BeginText();
                canvas.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 21);
                canvas.SetColorFill(BaseColor.WHITE);
                canvas.SetTextMatrix(20, 125);  // Adjust the coordinates as needed
                canvas.ShowText($"{memberId}");
                canvas.EndText();

                // Name
                canvas.BeginText();
                canvas.SetFontAndSize(font2.BaseFont, font2.Size);
                canvas.SetColorFill(BaseColor.WHITE);
                canvas.SetTextMatrix(22, 110);
                canvas.ShowText($"{textBox2.Text}, {textBox1.Text} {textBox3.Text}");
                canvas.EndText();

                // Address
                canvas.BeginText();
                canvas.SetFontAndSize(font1.BaseFont, font1.Size);
                canvas.SetColorFill(BaseColor.BLACK);
                canvas.SetTextMatrix(35, 30);
                canvas.ShowText(textBox5.Text);
                canvas.EndText();

                // Contact Number
                canvas.BeginText();
                canvas.SetFontAndSize(font1.BaseFont, font1.Size);
                canvas.SetColorFill(BaseColor.BLACK);
                canvas.SetTextMatrix(155, 55);
                canvas.ShowText(textBox6.Text);
                canvas.EndText();

                // Email Address
                canvas.BeginText();
                canvas.SetFontAndSize(font1.BaseFont, font1.Size);
                canvas.SetColorFill(BaseColor.BLACK);
                canvas.SetTextMatrix(35, 55);
                canvas.ShowText(textBox7.Text);
                canvas.EndText();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ApplySecondPageDesign: {ex.Message}");
            }
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
                textBox3.Focus();
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                textBox3.Focus();
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
            if (e.KeyCode == Keys.Tab)
            {
                textBox5.Focus();
            }
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                textBox6.Focus();
            }
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                textBox7.Focus();
            }
        }

        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                checkAllTextBoxes();
                button1.Focus();
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the pressed key is not a digit and not the backspace key
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                // Consume the key press
                e.Handled = true;
            }
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

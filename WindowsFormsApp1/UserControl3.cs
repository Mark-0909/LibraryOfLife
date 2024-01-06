using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class UserControl3 : UserControl
    {
        public member memberInformControl;
        public memberInformation memberInfoControl = new memberInformation();  // Initialize memberInfoControl
        public memberBorrow memberBorrow = new memberBorrow();
        public memberHistory memberHistory = new memberHistory();

        borrowBook borrowBook = new borrowBook();   

        public Panel Panel2
        {
            get { return panel1; }
            set { panel1 = value; }
        }
        public TextBox TextBox1
        {
            get { return textBox1; }
            set { textBox1 = value; }
        }
        public UserControl3()
        {
            InitializeComponent();
            this.panel1.BringToFront();
            
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            clickedButton(sender, e);
            memberInfoControl.BringToFront();
            button5.Visible = true;
            button5.BringToFront();
            memberBorrow.Visible = false;
            memberHistory.Visible = false;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            clickedButton(sender, e);
            memberBorrow.Visible = true;
            memberBorrow.BringToFront();
            button5.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            clickedButton(sender, e);
            memberHistory.Visible = true;
            memberHistory.BringToFront();
            button5.Visible = false;
        }

        // Button style when clicked
        private void ApplyButtonStyle(System.Windows.Forms.Button button)
        {
            button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            button.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            button.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            button.ForeColor = System.Drawing.Color.White;
            button.UseVisualStyleBackColor = false;
        }

        // Button style when not clicked
        private void ResetButtonStyle(System.Windows.Forms.Button button)
        {
            button.BackColor = System.Drawing.Color.Green;
            button.FlatAppearance.BorderColor = System.Drawing.Color.Green;
            button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            button.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            button.ForeColor = System.Drawing.Color.White;
            button.UseVisualStyleBackColor = false;
        }

        private void clickedButton(object sender, EventArgs e)
        {
            System.Windows.Forms.Button clickedButton = (System.Windows.Forms.Button)sender;

            ApplyButtonStyle(clickedButton);

            for (int i = 0; i <= 3; i++)
            {
                if (i != int.Parse(clickedButton.Name.Substring("button".Length)))
                {
                    System.Windows.Forms.Button otherButton = Controls.Find("button" + i, true).FirstOrDefault() as System.Windows.Forms.Button;
                    if (otherButton != null)
                    {
                        ResetButtonStyle(otherButton);
                    }
                }
            }
        }

        public void refreshData()
        {
            this.panel1.BringToFront();
        }

        public void FetchMemberData(string memberId)
        {
            // Add a debug statement to check if the method is being called
            Console.WriteLine($"Fetching data for member ID: {memberId}");
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();

                    // Extract the input ID from the TextBox
                    string inputID = memberId.Trim();

                    // Check if the input ID has exactly 9 digits
                    if (inputID.Length != 9 || !int.TryParse(inputID, out int _))
                    {
                        MessageBox.Show("Invalid input ID. Please enter a valid 9-digit ID.");
                        return;
                    }

                    // Extract the first 4 digits from the input ID
                    int registrationYear;
                    if (!int.TryParse(inputID.Substring(0, 4), out registrationYear))
                    {
                        MessageBox.Show("Invalid input ID format. Please enter a valid 9-digit ID with numeric first 4 digits.");
                        return;
                    }

                    // Extract the numbers after the first 4 digits and convert to int
                    int actualMemberID;
                    if (!int.TryParse(inputID.Substring(4), out actualMemberID))
                    {
                        MessageBox.Show("Invalid input ID format. Please enter a valid 9-digit ID with numeric characters after the first 4 digits.");
                        return;
                    }

                    // Query the database based on both conditions
                    string query = "SELECT * FROM members WHERE Registration_Year = @registrationYear AND ID = @actualMemberID";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    cmdDatabase.Parameters.AddWithValue("@registrationYear", registrationYear);
                    cmdDatabase.Parameters.AddWithValue("@actualMemberID", actualMemberID);

                    using (MySqlDataReader reader = cmdDatabase.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string memberID = inputID;
                            string firstName = reader["First_Name"].ToString();
                            string lastName = reader["Last_Name"].ToString();
                            string mi = reader["MI"].ToString();
                            int age = Convert.ToInt32(reader["Age"]);
                            string address = reader["Address"].ToString();
                            string contactNumber = reader["Contact_Number"].ToString();
                            string emailAddress = reader["Email_Address"].ToString();

                            MessageBox.Show($"Record found for Registration Year: {registrationYear}, Member ID: {memberID}");

                            // Dispose old controls
                            memberInfoControl.Dispose();
                            memberBorrow.Dispose();
                            memberHistory.Dispose();

                            // Recreate controls
                            memberInfoControl = new memberInformation(memberID, firstName, lastName, mi, age, address, contactNumber, emailAddress);
                            
                            memberBorrow = new memberBorrow();
                            memberHistory = new memberHistory();

                            // Add controls to the form
                            this.Controls.Add(memberInfoControl);
                            this.Controls.Add(memberBorrow);
                            this.Controls.Add(memberHistory);

                            // Set properties for controls
                            memberInfoControl.BackColor = System.Drawing.Color.White;
                            memberInfoControl.Location = new System.Drawing.Point(3, 100);
                            memberInfoControl.Name = "memberInformation1";
                            memberInfoControl.Size = new System.Drawing.Size(2080, 1029);
                            memberInfoControl.TabIndex = 5;
                            panel1.Visible = false;

                            memberBorrow.BackColor = System.Drawing.Color.White;
                            memberBorrow.Location = new System.Drawing.Point(3, 85);
                            memberBorrow.Name = "memberInformation1";
                            memberBorrow.Size = new System.Drawing.Size(2080, 1029);
                            memberBorrow.TabIndex = 5;

                            memberHistory.BackColor = System.Drawing.Color.White;
                            memberHistory.Location = new System.Drawing.Point(3, 85);
                            memberHistory.Name = "memberInformation1";
                            memberHistory.Size = new System.Drawing.Size(2080, 1029);
                            memberHistory.TabIndex = 5;

                            // Set visibility
                            memberInfoControl.BringToFront();
                            memberBorrow.Visible = false;
                            memberHistory.Visible = false;
                            button5.BringToFront();
                        }
                        else
                        {
                            MessageBox.Show($"No record found for Registration Year: {registrationYear} and Member ID: {actualMemberID}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.ToString()}");
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FetchMemberData(textBox1.Text);
        }





        private void button5_Click(object sender, EventArgs e)
        {
            // Dispose old controls
            memberInfoControl.Dispose();
            memberBorrow.Dispose();
            memberHistory.Dispose();

            // Recreate controls
            memberInfoControl = new memberInformation();
            memberBorrow = new memberBorrow();
            memberHistory = new memberHistory();

            // Add controls to the form
            this.Controls.Add(memberInfoControl);
            this.Controls.Add(memberBorrow);
            this.Controls.Add(memberHistory);

            // Set properties for controls
            // ... Existing code ...

            panel1.Visible = true;
            panel1.BringToFront();
            this.textBox1.Clear();
        }

        

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}


using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp1
{
    public partial class EditMember : Form
    {
        memberInformation memberInfoDisplay;

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

        public TextBox TextBox5
        {
            get { return textBox5; }
            set { textBox5 = value; }
        }

        public TextBox TextBox6
        {
            get { return textBox6; }
            set { textBox6 = value; }
        }

        public TextBox TextBox7
        {
            get { return textBox7; }
            set { textBox7 = value; }
        }

        public Label Label10
        {
            get { return label10; }
            set { label10 = value; }
        }

        public EditMember()
        {
            InitializeComponent();
        }

        public void SetMemberInformationForm(memberInformation memberInfoForm)
        {
            memberInfoDisplay = memberInfoForm;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
            SetMemberInformationLabels();
        }

        private void SetMemberInformationLabels()
        {
            if (memberInfoDisplay != null)
            {
                // Update the labels in the UI
                memberInfoDisplay.Label2.Text = $"{TextBox2.Text}, {TextBox1.Text} {TextBox3.Text}";
                memberInfoDisplay.Label3.Text = TextBox7.Text;
                memberInfoDisplay.Label4.Text = TextBox4.Text;
                memberInfoDisplay.Label5.Text = TextBox5.Text;
                memberInfoDisplay.Label6.Text = TextBox6.Text;

                // Update the variables in the memberInformation class
                memberInfoDisplay.FN = TextBox1.Text;
                memberInfoDisplay.LN = TextBox2.Text;
                memberInfoDisplay.MI = TextBox3.Text;

                // Update the labels in the memberInformation class
                

                memberInfoDisplay.labelName.Text = $"{memberInfoDisplay.LN}, {memberInfoDisplay.FN} {memberInfoDisplay.MI}";
                memberInfoDisplay.labelAge.Text = memberInfoDisplay.Label3.Text;
                memberInfoDisplay.labelAddress.Text = memberInfoDisplay.Label4.Text;
                memberInfoDisplay.labelPhone.Text = memberInfoDisplay.Label5.Text;
                memberInfoDisplay.labelEmail.Text = memberInfoDisplay.Label6.Text;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveChangesToDatabase();
        }

        private void SaveChangesToDatabase()
        {
            // Get the values from textboxes
            string firstName = TextBox1.Text;
            string lastName = TextBox2.Text;
            string middleInitial = TextBox3.Text;
            string age = TextBox7.Text;
            string address = TextBox4.Text;
            string contactNumber = TextBox5.Text;
            string emailAddress = TextBox6.Text;
            string memberId = Label10.Text;

            try
            {
                // Extract registration year and unique number from memberId
                string registrationYear = memberId.Substring(0, 4);
                string uniqueNumber = memberId.Substring(4);

                // Connection string
                string connectionString = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Find the ID of the member based on registration year and unique number
                    string findIdQuery = "SELECT ID FROM members WHERE Registration_Year=@RegistrationYear AND ID=@UniqueNumber";

                    using (MySqlCommand findIdCommand = new MySqlCommand(findIdQuery, connection))
                    {
                        // Add parameters to the query
                        findIdCommand.Parameters.AddWithValue("@RegistrationYear", registrationYear);
                        findIdCommand.Parameters.AddWithValue("@UniqueNumber", int.Parse(uniqueNumber)); // Assuming it's an integer

                        // Execute the findIdQuery
                        object result = findIdCommand.ExecuteScalar();

                        if (result != null)  // If a matching record is found
                        {
                            // The actual ID in the database
                            int actualId = Convert.ToInt32(result);

                            // SQL query to update the member in the database
                            string updateQuery = "UPDATE members SET First_Name=@FirstName, Last_Name=@LastName, MI=@MiddleInitial, Age=@Age, Address=@Address, Contact_Number=@ContactNumber, Email_Address=@EmailAddress WHERE ID=@ActualId";

                            using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection))
                            {
                                // Add parameters to the query
                                updateCommand.Parameters.AddWithValue("@FirstName", firstName);
                                updateCommand.Parameters.AddWithValue("@LastName", lastName);
                                updateCommand.Parameters.AddWithValue("@MiddleInitial", middleInitial);
                                updateCommand.Parameters.AddWithValue("@Age", age);
                                updateCommand.Parameters.AddWithValue("@Address", address);
                                updateCommand.Parameters.AddWithValue("@ContactNumber", contactNumber);
                                updateCommand.Parameters.AddWithValue("@EmailAddress", emailAddress);
                                updateCommand.Parameters.AddWithValue("@ActualId", actualId);

                                // For debugging purposes, print the query and the number of rows affected
                                Console.WriteLine("Query: " + updateCommand.CommandText);

                                // Execute the update query
                                int rowsAffected = updateCommand.ExecuteNonQuery();

                                Console.WriteLine("Rows affected: " + rowsAffected);

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Database updated successfully!");
                                }
                                else
                                {
                                    MessageBox.Show("No records were updated.");
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("No matching record found based on Registration Year and Unique Number.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
    }
}

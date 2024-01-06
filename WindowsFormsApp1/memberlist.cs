using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp1
{
    public partial class memberlist : UserControl
    {
        public FlowLayoutPanel MemberFlowLayoutPanel => flowLayoutPanel1;
        bool isPopUpFormOpen = false;
        public memberlist()
        {
            InitializeComponent();
            DisplayMembers();

            
        }




        
        public void refreshMember()
        {
            flowLayoutPanel1.Controls.Clear();
            DisplayMembers();
        }

        public void DisplayMembers()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();
                    string query = "select * from members";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    MySqlDataReader reader = cmdDatabase.ExecuteReader();

                    while (reader.Read())
                    {
                        string ID = reader["ID"].ToString();
                        string memberLName = reader["Last_Name"].ToString();
                        string memberFName = reader["First_Name"].ToString();
                        string memberMI = reader["MI"].ToString();
                        string year = reader["Registration_Year"].ToString();

                        string memberID; // Declare memberID outside the if-else blocks

                        if (ID.Length == 3)
                        {
                            memberID = $"{year}00{ID}";
                        }
                        else if (ID.Length == 4)
                        {
                            memberID = $"{year}0{ID}";
                        }
                        else
                        {
                            memberID = $"{year}{ID}";
                        }

                        string memFullName = $"{memberLName}, {memberFName} {memberMI}";

                        member memberControl = new member(memberID, memFullName);

                        MemberFlowLayoutPanel.Controls.Add(memberControl);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            OpenAddingForm(sender, e);
        }
        private void OpenAddingForm(object sender, EventArgs e)
        {



            if (!isPopUpFormOpen)
            {
                // Open your pop-up form here
                AddMember popUpForm = new AddMember();

                // Disable the main form
                this.FindForm().Enabled = false;
                
                isPopUpFormOpen = true;




                // Subscribe to the FormClosed event of the pop-up form
                popUpForm.FormClosed += (s, args) =>
                {
                    // Enable the main form when the pop-up form is closed
                    this.FindForm().Enabled = true;
                    isPopUpFormOpen = false;
                    refreshControl(sender, args);
                    
                };

                popUpForm.ShowDialog();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            refreshControl(sender, e);
        }
        public void refreshControl(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            DisplayMembers();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        public void searchBooks()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";
            string searchTerm = textBox1.Text.Trim();

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();

                    // Move the parameter addition before executing the query
                    string query = "SELECT * FROM members WHERE First_Name LIKE @SearchTerm OR Last_Name LIKE @SearchTerm OR MI LIKE @SearchTerm OR ID LIKE @SearchTerm";

                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    cmdDatabase.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");

                    // Execute the query after adding parameters
                    MySqlDataReader reader = cmdDatabase.ExecuteReader();

                    // Clear controls before adding new ones
                    flowLayoutPanel1.Controls.Clear();

                    while (reader.Read())
                    {
                        string ID = reader["ID"].ToString();
                        string memberLName = reader["Last_Name"].ToString();
                        string memberFName = reader["First_Name"].ToString();
                        string memberMI = reader["MI"].ToString();
                        string year = reader["Registration_Year"].ToString();

                        string memberID; // Declare memberID outside the if-else blocks

                        if (ID.Length == 3)
                        {
                            memberID = $"{year}00{ID}";
                        }
                        else if (ID.Length == 4)
                        {
                            memberID = $"{year}0{ID}";
                        }
                        else
                        {
                            memberID = $"{year}{ID}";
                        }

                        string memFullName = $"{memberLName}, {memberFName} {memberMI}";

                        member memberControl = new member(memberID, memFullName);

                        MemberFlowLayoutPanel.Controls.Add(memberControl);
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
            searchBooks();
        }
    }

}

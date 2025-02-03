using System;
using System.Drawing;
using System.Linq;
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
            label5.Hide();
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
                    string query = "select * from members WHERE Status = 'Regular'";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    MySqlDataReader reader = cmdDatabase.ExecuteReader();

                    while (reader.Read())
                    {
                        string id = reader["Actual_ID"].ToString();  // Use Actual_ID as the member ID
                        string memberLName = reader["Last_Name"].ToString();
                        string memberFName = reader["First_Name"].ToString();
                        string memberMI = reader["MI"].ToString();
                        string year = reader["Registration_Year"].ToString();

                        string memFullName = $"{memberLName}, {memberFName} {memberMI}";

                        member memberControl = new member(id, memFullName);

                        MemberFlowLayoutPanel.Controls.Add(memberControl);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
                finally
                {
                    connection.Close();
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
                AddMember popUpForm = new AddMember();
                this.FindForm().Enabled = false;

                isPopUpFormOpen = true;

                popUpForm.FormClosed += (s, args) =>
                {
                    this.FindForm().Enabled = true;
                    isPopUpFormOpen = false;
                    flowLayoutPanel1.Controls.Clear();
                    refreshMember();
                };

                popUpForm.ShowDialog();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            refreshMember();
        }

        

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Handle Paint event
        }

        public void searchmember()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";
            string searchTerm = textBox1.Text.Trim();

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT * FROM members WHERE First_Name LIKE @SearchTerm OR Last_Name LIKE @SearchTerm OR MI LIKE @SearchTerm OR Actual_ID LIKE @SearchTerm OR ID LIKE @SearchTerm AND Status = 'Regular'";

                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);

                    // Use parameters to prevent SQL injection
                    cmdDatabase.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");

                    using (MySqlDataReader reader = cmdDatabase.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string ID = reader["ID"].ToString();
                            string memberLName = reader["Last_Name"].ToString();
                            string memberFName = reader["First_Name"].ToString();
                            string memberMI = reader["MI"].ToString();
                            string year = reader["Registration_Year"].ToString();

                            string id = reader["Actual_ID"].ToString();
                            string memFullName = $"{memberLName}, {memberFName} {memberMI}";

                            member memberControl = new member(id, memFullName);

                            MemberFlowLayoutPanel.Controls.Add(memberControl);
                        }
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
            flowLayoutPanel1.Controls.Clear();
            searchmember();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "Search here")
            {
                MessageBox.Show("Please enter a valid search term.", "Empty Search", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                flowLayoutPanel1.Controls.Clear();
                searchmember();
                label5.Show();
                label5.Text = $"Search for: {textBox1.Text}";
                textBox1.Text = "Search here";
                textBox1.ForeColor = Color.Silver;
            }

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            label5.Hide();
            DisplayMembers();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Search here")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Search here";
                textBox1.ForeColor = Color.Silver;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                flowLayoutPanel1.Controls.Clear();
                searchmember();
                label5.Show();
                label5.Text = $"Search for: {textBox1.Text}";
                textBox1.Text = "Search here";
                textBox1.ForeColor = Color.Silver;
                button2.Focus();
            }
        }
    }
}

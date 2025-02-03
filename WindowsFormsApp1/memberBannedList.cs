using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp1
{
    public partial class memberBannedList : UserControl
    {
        public FlowLayoutPanel MemberFlowLayoutPanel => flowLayoutPanel1;
        bool isPopUpFormOpen = false;

        public memberBannedList()
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
                    string query = "select * from members WHERE Status = 'Banned'";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    MySqlDataReader reader = cmdDatabase.ExecuteReader();

                    while (reader.Read())
                    {
                        string ID = reader["ID"].ToString();
                        string memberLName = reader["Last_Name"].ToString();
                        string memberFName = reader["First_Name"].ToString();
                        string memberMI = reader["MI"].ToString();
                        string year = reader["Registration_Year"].ToString();

                        string memberID;

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
                    reader.Close();
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
                AddMember popUpForm = new AddMember();
                this.FindForm().Enabled = false;

                isPopUpFormOpen = true;

                popUpForm.FormClosed += (s, args) =>
                {
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
            // Handle Paint event
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

                    string query = "SELECT * FROM members WHERE First_Name LIKE @SearchTerm OR Last_Name LIKE @SearchTerm OR MI LIKE @SearchTerm OR ID LIKE @SearchTerm AND Status = 'Banned'";

                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    cmdDatabase.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");

                    MySqlDataReader reader = cmdDatabase.ExecuteReader();

                    flowLayoutPanel1.Controls.Clear();

                    while (reader.Read())
                    {
                        string ID = reader["ID"].ToString();
                        string memberLName = reader["Last_Name"].ToString();
                        string memberFName = reader["First_Name"].ToString();
                        string memberMI = reader["MI"].ToString();
                        string year = reader["Registration_Year"].ToString();

                        string memberID;

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

        

        private void button4_Click_1(object sender, EventArgs e)
        {
            refreshControl(sender, e);
            label5.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "Search here")
            {
                MessageBox.Show("Please enter a valid search term.", "Empty Search", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                searchBooks();
                label5.Show();
                label5.Text = $"Search for: {textBox1.Text}";
                textBox1.Text = "Search here";
                textBox1.ForeColor = Color.Silver;
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

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
                searchBooks();
                label5.Show();
                label5.Text = $"Search for: {textBox1.Text}";
                textBox1.Text = "Search here";
                textBox1.ForeColor = Color.Silver;
                button2.Focus();

            }
        }
    }
}

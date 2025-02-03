using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class historyMember : UserControl
    {
        public historyMember()
        {
            InitializeComponent();
            displayHistoryMember();
            label5.Hide();
        }


        public void displayHistoryMember()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";
            string query = "SELECT * FROM history_member ORDER BY ID DESC";

            using (MySqlConnection conDatabase = new MySqlConnection(constring))
            {
                try
                {
                    conDatabase.Open();

                    using (MySqlCommand cmdDatabase = new MySqlCommand(query, conDatabase))
                    {
                        using (MySqlDataReader reader = cmdDatabase.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string memberId = reader["member_ID"].ToString();
                                string memberName = reader["Initial_Name"].ToString();
                                string registrationDate = reader["Date"].ToString();
                                string remarks = reader["Remarks"].ToString();
                                string changeID = reader["ID"].ToString();

                                // Create a historyMemberLayout for each row in the result set
                                historyMemberLayout layout = new historyMemberLayout(memberId, memberName, registrationDate, remarks, changeID);

                                // Assuming you have a container (e.g., a panel) to add the layouts to
                                flowLayoutPanel1.Controls.Add(layout);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}\n\nDetails:\n{ex.ToString()}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void refreshControl()
        {
            flowLayoutPanel1.Controls.Clear();
            displayHistoryMember();
        }


        public void displayHistorysearchmember(string searchTerm)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";
            string query = "SELECT * FROM history_member WHERE member_ID LIKE @SearchTerm OR Initial_Name LIKE @SearchTerm OR Date LIKE @SearchTerm OR Remarks LIKE @SearchTerm ORDER BY ID DESC";

            using (MySqlConnection conDatabase = new MySqlConnection(constring))
            {
                try
                {
                    conDatabase.Open();

                    using (MySqlCommand cmdDatabase = new MySqlCommand(query, conDatabase))
                    {
                        cmdDatabase.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");

                        using (MySqlDataReader reader = cmdDatabase.ExecuteReader())
                        {
                            // Clear existing controls before adding new ones
                            flowLayoutPanel1.Controls.Clear();

                            while (reader.Read())
                            {
                                string memberId = reader["member_ID"].ToString();
                                string memberName = reader["Initial_Name"].ToString();
                                string registrationDate = reader["Date"].ToString();
                                string remarks = reader["Remarks"].ToString();
                                string changeID = reader["ID"].ToString();

                                // Create a historyMemberLayout for each row in the result set
                                historyMemberLayout layout = new historyMemberLayout(memberId, memberName, registrationDate, remarks, changeID);

                                // Add the layout to the flowLayoutPanel
                                flowLayoutPanel1.Controls.Add(layout);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}\n\nDetails:\n{ex.ToString()}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            displayHistorysearchmember(textBox1.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            refreshControl();
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
                displayHistorysearchmember(textBox1.Text);
                label5.Show();
                label5.Text = $"Search for: {textBox1.Text}";
                textBox1.Text = "Search here";
                textBox1.ForeColor = Color.Silver;
            }
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
            if(e.KeyCode == Keys.Enter)
            {
                displayHistorysearchmember(textBox1.Text);
                label5.Show();
                label5.Text = $"Search for: {textBox1.Text}";
                textBox1.Text = "Search here";
                textBox1.ForeColor = Color.Silver;
                button2.Focus();

            }
        }
    }
}

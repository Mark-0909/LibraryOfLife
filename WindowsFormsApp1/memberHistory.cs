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
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace WindowsFormsApp1
{
    public partial class memberHistory : UserControl
    {
        public memberHistory()
        {
            InitializeComponent();
            label5.Hide();
        }
        public string memID;
        public memberHistory(string memberID):this() 
        {
            memID = memberID;
            getmemberHistory(memberID);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void memberHistory_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }







        


        public void getmemberHistory(string memberID)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();

                    if (!int.TryParse(memberID, out int memID))
                    {
                        MessageBox.Show("Invalid member ID format. Please enter a valid integer value.");
                        return;
                    }

                    // Step 1: Retrieve all Reference-IDs with the same member_ID from the borrowedbook table
                    string borrowedBookQuery = "SELECT Reference_ID FROM borrowedbook WHERE member_ID = @MemberID";
                    MySqlCommand borrowedBookCmd = new MySqlCommand(borrowedBookQuery, connection);
                    borrowedBookCmd.Parameters.AddWithValue("@MemberID", memID);

                    List<string> refIds = new List<string>();

                    using (MySqlDataReader borrowedBookReader = borrowedBookCmd.ExecuteReader())
                    {
                        while (borrowedBookReader.Read())
                        {
                            string refId = borrowedBookReader["Reference_ID"].ToString();
                            refIds.Add(refId);
                        }
                    }

                    // Step 2: Retrieve and sort the corresponding records from the borrowlist table
                    List<string> sortedReferenceIds = new List<string>();
                    List<string> seenRefIds = new List<string>();

                    foreach (string refId in refIds)
                    {
                        string borrowListQuery = "SELECT COUNT(*) as Count FROM borrowlist WHERE Reference_ID = @RefID AND Status <> 'Returned'";
                        MySqlCommand countCmd = new MySqlCommand(borrowListQuery, connection);
                        countCmd.Parameters.AddWithValue("@RefID", refId);

                        int nonReturnedCount = Convert.ToInt32(countCmd.ExecuteScalar());

                        if (nonReturnedCount == 0)
                        {
                            // If all rows have Status 'Returned', add the RefID to both lists
                            if (!seenRefIds.Contains(refId))
                            {
                                seenRefIds.Add(refId);
                                sortedReferenceIds.Add(refId);
                            }
                        }
                    }

                    // Reverse the list to have the first occurrence at the beginning
                    sortedReferenceIds.Reverse();

                    // Pass the reversed list to DisplayLatestItem
                    DisplayLatestItem(sortedReferenceIds);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void DisplayLatestItem(List<string> sortedReferenceIds)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();

                    foreach (string refId in sortedReferenceIds)
                    {
                        string borrowListQuery = "SELECT Reference_ID, Return_Date, Borrowed_Date, member_ID FROM borrowedbook WHERE Reference_ID = @RefID";
                        MySqlCommand borrowListCmd = new MySqlCommand(borrowListQuery, connection);
                        borrowListCmd.Parameters.AddWithValue("@RefID", refId);

                        using (MySqlDataReader borrowListReader = borrowListCmd.ExecuteReader())
                        {
                            if (borrowListReader.Read())
                            {
                                string referenceId = borrowListReader["Reference_ID"].ToString();
                                DateTime returnedDate = Convert.ToDateTime(borrowListReader["Return_Date"]);
                                DateTime borrowedDate = Convert.ToDateTime(borrowListReader["Borrowed_Date"]);
                                string memberId = borrowListReader["member_ID"].ToString();

                                memberHistoryLayout historyLayout = new memberHistoryLayout(
                                    referenceId,
                                    memberId,
                                    borrowedDate.ToString("MM-dd-yyyy"),
                                    returnedDate.ToString("MM-dd-yyyy")
                                );

                                flowLayoutPanel1.Controls.Add(historyLayout);
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




















        public void refreshControl()
        {
            flowLayoutPanel1.Controls.Clear();
            getmemberHistory(memID);
            label5.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "Search here")
            {
                MessageBox.Show("Please enter a valid search term.", "Empty Search", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } else
            {
                flowLayoutPanel1.Controls.Clear();
                matchBookName(textBox1.Text, memID);
                label5.Show();
                label5.Text = $"Search for: {textBox1.Text}";
                textBox1.Text = "Search here";
                textBox1.ForeColor = Color.Silver;
            }
            
            
        }























        public void matchBookName(string searchTerm, string id)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";
            flowLayoutPanel1.Controls.Clear();

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT bb.Reference_ID, MAX(bb.Borrowed_Date) AS Borrowed_Date, " +
                                   "MAX(bb.Return_Date) AS Return_Date, MAX(bb.member_ID) AS member_ID, " +
                                   "MAX(bl.Status) AS Status, " +
                                   "MAX(bl.Violation) AS Violation, MAX(bl.Returned_Date) AS Returned_Date " +
                                   "FROM borrowedbook bb " +
                                   "INNER JOIN borrowlist bl ON bb.Reference_ID = bl.Reference_ID " +
                                   "INNER JOIN books b ON bl.Book_List = b.Book_Id " +
                                   "WHERE " +
                                   "bb.member_ID = @memID AND " + // Add AND here
                                   "(b.Book_Name LIKE @SearchTerm OR " +
                                   "bb.Reference_ID LIKE @SearchTerm OR " +
                                   "bb.Borrowed_Date LIKE @SearchTerm OR " +
                                   "bb.member_ID LIKE @SearchTerm OR " +
                                   "bb.Return_Date LIKE @SearchTerm OR " +
                                   "bl.Book_List LIKE @SearchTerm OR " +
                                   "bl.Violation LIKE @SearchTerm OR " +
                                   "bl.Returned_Date LIKE @SearchTerm) " +
                                   "AND bl.Status = 'Returned' " +
                                   "AND NOT EXISTS (" +
                                   "    SELECT 1 FROM borrowlist bl_sub " +
                                   "    WHERE bl_sub.Reference_ID = bb.Reference_ID " +
                                   "    AND bl_sub.Status <> 'Returned'" +
                                   ") " +
                                   "GROUP BY bb.Reference_ID " +
                                   "ORDER BY MAX(bl.Returned_Date) DESC, MAX(bl.Returned_Time) DESC";

                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    cmdDatabase.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");
                    cmdDatabase.Parameters.AddWithValue("@memID", id);

                    using (MySqlDataReader reader = cmdDatabase.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string refId = reader["Reference_ID"].ToString();
                            int memberId = int.Parse(reader["member_ID"].ToString());
                            DateTime borrowedDate = Convert.ToDateTime(reader["Borrowed_Date"]);
                            DateTime returnDate = Convert.ToDateTime(reader["Return_Date"]);

                            // Create your display control (e.g., historyreturndisplay) and add it to the flowLayoutPanel
                            // Adjust this line according to your actual implementation
                            var borrowLayout = new memberHistoryLayout(
                                refId,
                                memberId.ToString(),
                                borrowedDate.ToString("MM-dd-yyyy"),
                                returnDate.ToString("MM-dd-yyyy")
                            );

                            flowLayoutPanel1.Controls.Add(borrowLayout);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }
























































        private void textBox1_Enter(object sender, EventArgs e)
        {
            if(textBox1.Text == "Search here")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if(textBox1.Text == "")
            {
                textBox1.Text = "Search here";
                textBox1.ForeColor = Color.Silver;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                flowLayoutPanel1.Controls.Clear();
                matchBookName(textBox1.Text, memID);
                label5.Show();
                label5.Text = $"Search for: {textBox1.Text}";
                textBox1.Text = "Search here";
                textBox1.ForeColor = Color.Silver;
                button1.Focus();
                
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            getmemberHistory(memID);
            label5.Hide();
        }
    }
}

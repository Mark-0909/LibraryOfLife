using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class historyReturn : UserControl
    {
        public historyReturn()
        {
            InitializeComponent();
            getmemberHistory();
            label5.Hide();
        }
        public string memberID;
        public void getmemberHistory()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();

                    // Step 1: Retrieve all Reference-IDs from the borrowedbook table
                    string borrowedBookQuery = "SELECT DISTINCT Reference_ID FROM borrowedbook";
                    MySqlCommand borrowedBookCmd = new MySqlCommand(borrowedBookQuery, connection);

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

                                historyreturndisplay historyLayout = new historyreturndisplay(
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

        public void filterBorrowedBook(MySqlDataReader filterReader, List<string> refIds, List<DateTime> returnDates, List<DateTime> borrowedDates, List<string> memberIDs)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";
            string refid = filterReader["Reference_ID"].ToString();

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                connection.Open();

                string query = "SELECT Status, Returned_Date FROM borrowlist WHERE Reference_ID = @refID ORDER BY COALESCE(Returned_Date, '9999-12-31') DESC LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@refID", refid);

                string status = string.Empty;

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            status = reader["Status"].ToString();

                            // If any book is still "Borrowed", exit the loop
                            if (status == "Borrowed")
                            {
                                return;
                            }

                            // Get the latest Return_Date for each Ref_ID
                            DateTime returnDate = reader["Returned_Date"] == DBNull.Value ? DateTime.MaxValue : Convert.ToDateTime(reader["Returned_Date"]);
                            refIds.Add(refid);
                            returnDates.Add(returnDate);
                            borrowedDates.Add(Convert.ToDateTime(filterReader["Borrowed_Date"]));
                            memberIDs.Add(filterReader["member_ID"].ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}");
                    }
                }
            }
        }



        public void refreshControl()
        {
            flowLayoutPanel1.Controls.Clear();
            getmemberHistory();
        }


        


        public void matchBookName(string searchTerm)
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
                            var borrowLayout = new historyreturndisplay(
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




        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "Search here")
            {
                MessageBox.Show("Please enter a valid search term.", "Empty Search", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                flowLayoutPanel1.Controls.Clear();
                matchBookName(textBox1.Text);
                label5.Show();
                label5.Text = $"Search for: {textBox1.Text}";
                textBox1.Text = "Search here";
                textBox1.ForeColor = Color.Silver;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label5.Hide();
            getmemberHistory();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            refreshControl();
            label5.Hide();
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
                matchBookName(textBox1.Text);
                label5.Show();
                label5.Text = $"Search for: {textBox1.Text}";
                textBox1.Text = "Search here";
                textBox1.ForeColor = Color.Silver;
                button1.Focus();

            }
        }
    }
}

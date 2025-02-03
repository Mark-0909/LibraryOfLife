// memberBorrow
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class memberBorrow : UserControl
    {
        private string MemberID;

        public FlowLayoutPanel FlowLayoutPanel
        {
            get { return flowLayoutPanel1; }
            set { flowLayoutPanel1 = value; }
        }

        // Make sure to initialize flowLayoutPanel1 in the constructor or designer.
        // For example, in the constructor of memberBorrow:
        public memberBorrow()
        {
            InitializeComponent();
            label5.Hide();
        }

        public memberBorrow(string memberID) : this()
        {
            getMemberID(memberID);
        }

        public void getMemberID(string memberID)
        {
            MemberID = memberID;
        }

        public void UpdateMemberID(string memberID)
        {
            MemberID = memberID;
            displayBorrowHistory();
        }

        public void displayBorrowHistory()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            List<string> refIds = new List<string>();
            List<DateTime> returnDates = new List<DateTime>();
            List<DateTime> borrowedDates = new List<DateTime>();

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();

                    if (!int.TryParse(MemberID, out int memberID))
                    {
                        MessageBox.Show("Invalid member ID format. Please enter a valid integer value.");
                        return;
                    }

                    string query = "SELECT * FROM borrowedbook WHERE member_ID = @MemberID ORDER BY Return_Date DESC";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    cmdDatabase.Parameters.AddWithValue("@MemberID", memberID);

                    using (MySqlDataReader reader = cmdDatabase.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            filterBorrowedBook(reader, refIds, returnDates, borrowedDates);
                        }
                    }

                    // Sort lists based on the return date (ascending order)
                    var sortedDates = returnDates.OrderBy(date => date).ToList();
                    var sortedRefIds = refIds.OrderBy(id => returnDates[refIds.IndexOf(id)]).ToList();

                    // Display layouts based on the sorted lists
                    for (int i = 0; i < sortedRefIds.Count; i++)
                    {
                        string refId = sortedRefIds[i];
                        int index = refIds.IndexOf(refId);
                        DateTime returnDate = returnDates[index];
                        DateTime borrowedDate = borrowedDates[index];

                        memberBorrowLayout borrowLayout = new memberBorrowLayout(
                            refId,
                            MemberID,
                            borrowedDate.ToString("MM-dd-yyyy"),
                            returnDate.ToString("MM-dd-yyyy")
                        );

                        flowLayoutPanel1.Controls.Add(borrowLayout);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        public void filterBorrowedBook(MySqlDataReader filterReader, List<string> refIds, List<DateTime> returnDates, List<DateTime> borrowedDates)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";
            string refid = filterReader["Reference_ID"].ToString();

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                connection.Open();

                string query = "SELECT Status FROM borrowlist WHERE Reference_ID = @refID";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@refID", refid);

                bool hasBorrowedStatus = false;

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            if (reader["Status"].ToString() == "Borrowed")
                            {
                                hasBorrowedStatus = true;
                                break; // No need to continue checking once "Borrowed" status is found
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}");
                    }
                }

                // Create the memberBorrowLayout only if there is at least one "Borrowed" status
                if (hasBorrowedStatus)
                {
                    DateTime borrowedDate = Convert.ToDateTime(filterReader["Borrowed_Date"]);
                    DateTime returnDate = Convert.ToDateTime(filterReader["Return_Date"]);

                    // Add data to the lists
                    refIds.Add(refid);
                    returnDates.Add(returnDate);
                    borrowedDates.Add(borrowedDate);
                }
            }
        }

        public void refreshBorrow()
        {
            flowLayoutPanel1.Controls.Clear();
            UpdateMemberID(MemberID);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            matchBookName(textBox1.Text, MemberID);
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
                                   "MAX(bl.Status) AS Status " +
                                   "FROM borrowedbook bb " +
                                   "INNER JOIN borrowlist bl ON bb.Reference_ID = bl.Reference_ID " +
                                   "INNER JOIN books b ON bl.Book_List = b.Book_Id " +
                                   "WHERE " + 
                                   "member_ID = @ID AND" +
                                   "(b.Book_Name LIKE @SearchTerm OR " +
                                   "bb.Reference_ID LIKE @SearchTerm OR " +
                                   "bb.Borrowed_Date LIKE @SearchTerm OR " +
                                   "bb.member_ID LIKE @SearchTerm OR " +
                                   "bb.Return_Date LIKE @SearchTerm OR " +
                                   "bl.Book_List LIKE @SearchTerm) " +
                                   "AND bl.Status = 'Borrowed' " +
                                   "AND NOT EXISTS (" +
                                   "    SELECT 1 FROM borrowlist bl_sub " +
                                   "    WHERE bl_sub.Reference_ID = bb.Reference_ID " +
                                   "    AND bl_sub.Status <> 'Borrowed'" +
                                   ") " +
                                   "GROUP BY bb.Reference_ID " +
                                   "ORDER BY MIN(bl.Return_Date) ASC";

                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    cmdDatabase.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");
                    cmdDatabase.Parameters.AddWithValue("@ID", id);
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
                            var borrowLayout = new memberBorrowLayout(
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


        private void button4_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            refreshBorrow();
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
                flowLayoutPanel1.Controls.Clear();
                matchBookName(textBox1.Text, MemberID);
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
            if (e.KeyCode == Keys.Enter)
            {
                flowLayoutPanel1.Controls.Clear();
                matchBookName(textBox1.Text, MemberID);
                label5.Show();
                label5.Text = $"Search for: {textBox1.Text}";
                textBox1.Text = "Search here";
                textBox1.ForeColor = Color.Silver;
                button2.Focus();

            }
        }

        private void memberBorrow_Load(object sender, EventArgs e)
        {

        }
    }
}

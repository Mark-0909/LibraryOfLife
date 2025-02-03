using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class allBorrowedBook : UserControl
    {
        public allBorrowedBook()
        {
            InitializeComponent();
            displayBorrowHistory();
            label6.Hide();
        }

        public class BorrowedBookInfo
        {
            public string RefId { get; set; }
            public DateTime ReturnDate { get; set; }
            public DateTime BorrowedDate { get; set; }
            public int MemberId { get; set; }
        }

        public void displayBorrowHistory()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            List<BorrowedBookInfo> borrowedBooks = new List<BorrowedBookInfo>();

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT * FROM borrowedbook";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);

                    using (MySqlDataReader reader = cmdDatabase.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            filterBorrowedBook(reader, borrowedBooks);
                        }
                    }

                    // Sort borrowedBooks list based on the return date (ascending order)
                    borrowedBooks = borrowedBooks.OrderBy(book => book.ReturnDate).ToList();

                    // Display layouts based on the sorted list
                    foreach (var book in borrowedBooks)
                    {
                        allBorrowLayout borrowLayout = new allBorrowLayout(
                            book.RefId,
                            book.MemberId.ToString(),
                            book.BorrowedDate.ToString("MM-dd-yyyy"),
                            book.ReturnDate.ToString("MM-dd-yyyy")
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

        public void filterBorrowedBook(MySqlDataReader filterReader, List<BorrowedBookInfo> borrowedBooks)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";
            string refid = filterReader["Reference_ID"].ToString();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(constring))
                {
                    connection.Open();

                    string query = "SELECT Status FROM borrowlist WHERE Reference_ID = @refID";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@refID", refid);

                        bool hasBorrowedStatus = false;

                        using (MySqlDataReader reader = cmd.ExecuteReader())
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

                        // Create the BorrowedBookInfo only if there is at least one "Borrowed" status
                        if (hasBorrowedStatus)
                        {
                            DateTime borrowedDate = Convert.ToDateTime(filterReader["Borrowed_Date"]);
                            DateTime returnDate = Convert.ToDateTime(filterReader["Return_Date"]);

                            // Add data to the borrowedBooks list
                            borrowedBooks.Add(new BorrowedBookInfo
                            {
                                RefId = refid,
                                ReturnDate = returnDate,
                                BorrowedDate = borrowedDate,
                                MemberId = int.Parse(filterReader["member_ID"].ToString())
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private string connectionString = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";












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
                                   "MAX(bl.Status) AS Status " +
                                   "FROM borrowedbook bb " +
                                   "INNER JOIN borrowlist bl ON bb.Reference_ID = bl.Reference_ID " +
                                   "INNER JOIN books b ON bl.Book_List = b.Book_Id " +
                                   "WHERE " +
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
                            var borrowLayout = new allBorrowLayout(
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
            matchBookName(textBox1.Text);
        }
        public void refreshcontrol()
        {
            flowLayoutPanel1.Controls.Clear();
            displayBorrowHistory();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            refreshcontrol();
            label6.Hide();
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
                matchBookName(textBox1.Text);
                label6.Show();
                label6.Text = $"Search for: {textBox1.Text}";
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
                matchBookName(textBox1.Text);
                label6.Show();
                label6.Text = $"Search for: {textBox1.Text}";
                textBox1.Text = "Search here";
                textBox1.ForeColor = Color.Silver;
                button2.Focus();

            }
        }
    }
}

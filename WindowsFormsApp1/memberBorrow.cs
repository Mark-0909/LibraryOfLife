// memberBorrow
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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

                    string query = "SELECT * FROM borrowedbook WHERE member_ID = @MemberID";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    cmdDatabase.Parameters.AddWithValue("@MemberID", memberID);

                    MySqlDataReader reader = cmdDatabase.ExecuteReader();

                    while (reader.Read())
                    {
                        memberBorrowLayout borrowLayout = new memberBorrowLayout(reader["Reference_ID"].ToString(), reader["member_ID"].ToString());
                        flowLayoutPanel1.Controls.Add(borrowLayout);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }
    }
}

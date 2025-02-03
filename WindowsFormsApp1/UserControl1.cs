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
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
            getBookBorrowed();
            getNumberOfNewMember();
            getNumberOfBorrow();
            getNumberNewBooks();
            getReturnNumber();
            string date = DateTime.Now.ToString("MM/dd/yyyy");
            label17.Text = date;


            chart charts = new chart("Today");
            panel16.Controls.Add(charts);


            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;


        }
        public void refreshAllControls(object sender, EventArgs e)
        {
            getNumberNewBooks();
            getReturnNumber();
            getNumberOfNewMember();
            getNumberOfBorrow();
            flowLayoutPanel1.Controls.Clear();
            getBookBorrowed();
            refreshGraph();

        }
        public void refreshGraph()
        {
            panel16.Controls.Clear();
            chart charts = new chart("Today");
            panel16.Controls.Add(charts);
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedPeriod = comboBox1.SelectedItem.ToString();

            if (selectedPeriod == "Today")
            {
                panel16.Controls.Clear();
                chart charts = new chart("Today");
                panel16.Controls.Add(charts);

            }
            else if (selectedPeriod == "Last week")
            {
                panel16.Controls.Clear();
                chart charts = new chart("Last week");
                panel16.Controls.Add(charts);

            }
            else if (selectedPeriod == "Last month")
            {
                panel16.Controls.Clear();
                chart charts = new chart("Last month");
                panel16.Controls.Add(charts);
            }
        }

        

        public void getNumberNewBooks()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(constring))
                {
                    connection.Open();

                    string todayDateString = DateTime.Now.ToString("MM-dd-yyyy");

                    // Get the sum of 'ADD' records
                    string addQuery = $"SELECT SUM(Change_Stocks) FROM book_history WHERE Change_Date = '{todayDateString}' AND Remarks = 'ADD'";

                    using (MySqlCommand addCmd = new MySqlCommand(addQuery, connection))
                    {
                        object addResult = addCmd.ExecuteScalar();

                        int totalSum = 0;

                        if (addResult != null && addResult != DBNull.Value)
                        {
                            int addSum = Convert.ToInt32(addResult);
                            // Update label16.Text with the sum from 'ADD'
                            totalSum += addSum;
                        }

                        // Get the sum of 'EDIT' records
                        string editQuery = $"SELECT Change_Stocks, Initial_Stocks FROM book_history WHERE Change_Date = '{todayDateString}' AND Remarks = 'EDIT'";

                        using (MySqlCommand editCmd = new MySqlCommand(editQuery, connection))
                        {
                            using (MySqlDataReader reader = editCmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string changeStocksString = reader["Change_Stocks"].ToString();
                                    string initialstocks = reader["Initial_Stocks"].ToString();
                                    string needsum = $"{initialstocks} - {changeStocksString}";
                                   

                                    int editSum = CalculateSum(needsum);

                                    // Add the sum from 'EDIT' to the existing value
                                    totalSum += editSum;
                                }
                            }
                        }

                        // Update label16.Text with the total sum
                        label16.Text = totalSum.ToString();
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }





        private int CalculateSum(string changeStocksString)
        {
            Console.WriteLine($"Debug: Calculating sum for {changeStocksString}");

            // Check if the string contains a '-'
            if (changeStocksString.Contains("-"))
            {
                // Split the string into individual values
                string[] values = changeStocksString.Split('-');

                // Parse each value to int
                int value1, value2;
                if (values.Length == 2 && int.TryParse(values[0].Trim(), out value1) && int.TryParse(values[1].Trim(), out value2))
                {
                    Console.WriteLine($"Debug: Parsed values - value1: {value1}, value2: {value2}");

                    
                    if (value2 > value1)
                    {
                        
                        return value2 - value1;
                    }
                    else
                    {
                        
                        return 0;
                    }
                }
            }

           
            return 0;
        }





        public void getReturnNumber()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM borrowlist WHERE Returned_Date = @ReturnedDate";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    cmdDatabase.Parameters.AddWithValue("@ReturnedDate", DateTime.Now.ToString("MM-dd-yyyy"));

                    int count = Convert.ToInt32(cmdDatabase.ExecuteScalar());
                    string dis = count.ToString();
                    label15.Text = dis;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.ToString()}");
                }
            }
        }



        private void label1_Click(object sender, EventArgs e)
        {
           
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }
        
        

        public void getNumberOfNewMember()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM history_member WHERE Date = @Date AND Remarks = 'ADD'";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    cmdDatabase.Parameters.AddWithValue("@Date", DateTime.Now.ToString("MM-dd-yyyy"));

                    int count = Convert.ToInt32(cmdDatabase.ExecuteScalar());
                    string dis = count.ToString();
                    label13.Text = dis;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.ToString()}");
                }
            }
        }

        public void getNumberOfBorrow()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM borrowlist WHERE Borrowed_Date = @BorrowedDate and Initial_Status = @InitialStat";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);

                    // Parameterize the date and use DateTime.Now directly
                    cmdDatabase.Parameters.AddWithValue("@BorrowedDate", DateTime.Now.ToString("MM-dd-yyyy"));
                    cmdDatabase.Parameters.AddWithValue("@InitialStat", "Borrowed");

                    int count = Convert.ToInt32(cmdDatabase.ExecuteScalar());
                    label14.Text = count.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.ToString()}");
                }
                finally
                {
                    // Ensure the connection is closed in all cases
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }


        
        






        public void getBookBorrowed()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Book_List, Borrowed_Date, Initial_Status FROM borrowlist WHERE Borrowed_Date = @Borroweddate AND Initial_Status = 'Borrowed'";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    cmdDatabase.Parameters.AddWithValue("@Borroweddate", DateTime.Now.ToString("MM-dd-yyyy"));

                    using (MySqlDataReader reader = cmdDatabase.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                getBookDetails(reader["Book_List"].ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.ToString()}");
                }
            }
        }

        public void getBookDetails(string bookID)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Book_Name FROM books WHERE Book_Id = @ID";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    cmdDatabase.Parameters.AddWithValue("@ID", bookID);

                    using (MySqlDataReader reader = cmdDatabase.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                

                                        dashboardBorrowedBooks booklist = new dashboardBorrowedBooks(
                                        bookID,
                                        reader["Book_Name"].ToString()
                                );
                                flowLayoutPanel1.Controls.Add(booklist);    
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.ToString()}");
                }
            }
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }
    }
}

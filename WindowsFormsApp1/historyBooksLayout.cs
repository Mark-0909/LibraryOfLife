using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class historyBooksLayout : UserControl
    {
        public historyBooksLayout()
        {
            InitializeComponent();
            label4.Visible = false;
        }
        public historyBooksLayout(string changeID, string BookID, string date, string remarks) : this()
        {
            label1.Text = BookID;
            label2.Text = date;
            label3.Text = remarks;
            label4.Text = changeID;

        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private historyBookChangesLayout historyLayout;

        private void button1_Click(object sender, EventArgs e)
        {
            // Check if the historyLayout is null or disposed
            if (historyLayout == null || historyLayout.IsDisposed)
            {
                if (label3.Text == "ADD" || label3.Text == "PHASE OUT" || label3.Text == "AVAILABLE")
                {
                    historyLayout = getChangeDetailAdd();
                    flowLayoutPanel1.Controls.Add(historyLayout);
                }
                else
                {
                    historyLayout = getDetailsEdit();
                    flowLayoutPanel1.Controls.Add(historyLayout);
                }
                    
            }
            else
            {
                // Remove the existing historyLayout from flowLayoutPanel1
                flowLayoutPanel1.Controls.Remove(historyLayout);

                // Dispose the historyLayout to free resources
                historyLayout.Dispose();

                // Set historyLayout to null to indicate it's not added anymore
                historyLayout = null;
            }
        }

        public historyBookChangesLayout getChangeDetailAdd()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";
            string query = "SELECT * FROM book_history WHERE ID = @ChangeID";

            using (MySqlConnection conDatabase = new MySqlConnection(constring))
            {
                try
                {
                    conDatabase.Open();

                    using (MySqlCommand cmdDatabase = new MySqlCommand(query, conDatabase))
                    {
                        // Set the value for the @ChangeID parameter
                        cmdDatabase.Parameters.AddWithValue("@ChangeID", int.Parse(label4.Text));

                        using (MySqlDataReader reader = cmdDatabase.ExecuteReader())
                        {
                            if (reader.Read())  // Check if there is a row
                            {
                                

                                string name = reader["Change_Name"].ToString();
                                string author = reader["Change_Author"].ToString();
                                string location = reader["Change_Location"].ToString();
                                string stocks = reader["Change_Stocks"].ToString();
                                string genre = reader["Change_Genre"].ToString();

                                // Convert the Image to byte[] if required by the constructor
                                string image = reader["Image_Path"].ToString();

                                // Create a historyBookChangesLayout for the row
                                return new historyBookChangesLayout(image, name, author, location, stocks, genre, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                            }
                            else
                            {
                                // Handle the case where no rows were returned
                                MessageBox.Show("No records found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return null; // Return null if no records found
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}\n\nDetails:\n{ex.ToString()}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null; // Return null in case of an error
                }
            }
        }

        
        

        private Image ByteArrayToImage(byte[] byteArray)
        {
            if (byteArray == null)
                return null;

            using (MemoryStream memoryStream = new MemoryStream(byteArray))
            {
                return Image.FromStream(memoryStream);
            }
        }

        private byte[] ImageToByteArray(Image image)
        {
            if (image == null)
                return null;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Save the image to the memory stream with its original format
                image.Save(memoryStream, image.RawFormat);

                // Get the byte array from the memory stream
                return memoryStream.ToArray();
            }
        }



        public historyBookChangesLayout getDetailsEdit()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";
            string query = "SELECT * FROM book_history WHERE ID = @ChangeID";

            using (MySqlConnection conDatabase = new MySqlConnection(constring))
            {
                try
                {
                    conDatabase.Open();

                    using (MySqlCommand cmdDatabase = new MySqlCommand(query, conDatabase))
                    {
                        cmdDatabase.Parameters.AddWithValue("@ChangeID", int.Parse(label4.Text));

                        using (MySqlDataReader reader = cmdDatabase.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                string iname = reader["Initial_Name"].ToString();
                                string iauthor = reader["Initial_Author"].ToString();
                                string ilocation = reader["Initial_Location"].ToString();
                                string istocks = reader["Initial_Stocks"].ToString();
                                string igenre = reader["Initial_Genre"].ToString();
                                
                                string cname = reader["Change_Name"].ToString();
                                string cauthor = reader["Change_Author"].ToString();
                                string clocation = reader["Change_Location"].ToString();
                                string cstocks = reader["Change_Stocks"].ToString();
                                string cgenre = reader["Change_Genre"].ToString();
                                string image = reader["Image_Path"].ToString();

                                return new historyBookChangesLayout(image, iname, iauthor, ilocation, istocks, igenre, cname, cauthor, clocation, cstocks, cgenre);
                            }
                            else
                            {
                                MessageBox.Show("No records found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return null;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}\n\nDetails:\n{ex.ToString()}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }




        // Updated SplitField method with explicit types for Tuple.Create



    }
}


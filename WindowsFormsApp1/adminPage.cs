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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace WindowsFormsApp1
{
    public partial class adminPage : Form
    {
        public adminPage()
        {
            InitializeComponent();
            
            FetchGenre();
            FetchLocation();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        public void FetchGenre()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM genre_list";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    MySqlDataReader reader = cmdDatabase.ExecuteReader();

                    

                    while (reader.Read())
                    {
                        // Assuming genreorlocation is a class that can display genre information
                        string genreName = reader["Genre"].ToString();
                        string id = reader["ID"].ToString();
                        genreorlocation genrelayout = new genreorlocation(genreName, "Genre", id);
                        flowLayoutPanel1.Controls.Add(genrelayout);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }


        public void FetchLocation()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM location_list";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    MySqlDataReader reader = cmdDatabase.ExecuteReader();

                    while (reader.Read())
                    {
                        // Assuming genreorlocation is a class that can display genre information
                        string locationName = reader["Location_Name"].ToString();
                        string id = reader["ID"].ToString();
                        genreorlocation locationlayout = new genreorlocation(locationName, "Location", id);
                        flowLayoutPanel2.Controls.Add(locationlayout);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            change();
        }

        
        public void change()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();
                    string query = "select * from admininfo";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    MySqlDataReader reader = cmdDatabase.ExecuteReader();

                    while (reader.Read())
                    {


                        if (textBox1.Text == string.Empty || textBox2.Text == string.Empty || textBox5.Text == string.Empty || textBox6.Text == string.Empty)
                        {
                            MessageBox.Show("Incomplete input. ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                        else
                        {
                            if (reader["Admin_Username"].ToString() == textBox6.Text || reader["Admin_password"].ToString() == textBox5.Text)
                            {
                                ChangeCredentials(textBox1.Text, textBox2.Text);
                            }
                            else
                            {
                                MessageBox.Show("Incorrect username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        
        
        public void ChangeCredentials(string newUsername, string newPassword)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();

                    // Use proper UPDATE syntax to update specific records
                    string query = "UPDATE admininfo SET Admin_Username = @NewUsername, Admin_password = @NewPassword";

                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    cmdDatabase.Parameters.AddWithValue("@NewUsername", newUsername);
                    cmdDatabase.Parameters.AddWithValue("@NewPassword", newPassword);

                    int rowsAffected = cmdDatabase.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Credentials updated successfully");
                        textBox5.Clear();
                        textBox6.Clear();
                        textBox1.Clear();
                        textBox2.Clear();
                    }
                    else
                    {
                        MessageBox.Show("No records updated. Make sure the record exists.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (textBox3.Text == string.Empty)
            {
                MessageBox.Show("Empty input. ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else
            {
                AddGenre(textBox3.Text);
            }
            
        }

        public void AddGenre(string genre)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();

                    // Use proper INSERT syntax to insert a new record into the genre_list table
                    string query = "INSERT INTO genre_list (Genre) VALUES (@Genre)";

                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    cmdDatabase.Parameters.AddWithValue("@Genre", genre);

                    int rowsAffected = cmdDatabase.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Genre added successfully");
                        textBox3.Clear(); 
                        flowLayoutPanel1.Controls.Clear();
                        FetchGenre();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add genre. Check for errors and try again.");
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
            if(textBox4.Text == string.Empty)
            {
                MessageBox.Show("Empty input. ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                AddLocation(textBox4.Text);
            }
        }
        public void AddLocation(string location)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    connection.Open();

                    // Use proper INSERT syntax to insert a new record into the genre_list table
                    string query = "INSERT INTO location_list (Location_Name) VALUES (@location)";

                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    cmdDatabase.Parameters.AddWithValue("@location", location);

                    int rowsAffected = cmdDatabase.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Location added successfully");
                        textBox4.Clear(); 
                        flowLayoutPanel2.Controls.Clear();
                        FetchLocation();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add genre. Check for errors and try again.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                change();
            }
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab) 
            {
                textBox5.Focus();
            }
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                textBox1.Focus();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                textBox2.Focus();
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddGenre(textBox3.Text);
            }
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddLocation(textBox4.Text);
            }
        }
    }
}


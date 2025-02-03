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
    public partial class historyMemberLayout : UserControl
    {
        public historyMemberLayout()
        {
            InitializeComponent();
            label5.Hide();
        }
        public historyMemberLayout(string ID, string Name, string regDate, string remarks, string changeID) : this()
        {
            displayHistory(ID, Name, regDate, remarks, changeID);
        }
        public void displayHistory(string ID, string Name, string regDate, string remarks, string changeID)
        {
            label1.Text = ID;
            label2.Text = Name;
            label3.Text = regDate;
            label4.Text = remarks;
            label5.Text = changeID;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }


        private historymemberchangeslayout historyLayout;

        private void button1_Click(object sender, EventArgs e)
        {

            if (historyLayout == null || historyLayout.IsDisposed)
            {
                if (label4.Text == "ADD")
                {
                    historyLayout = getAddDetails();
                    flowLayoutPanel1.Controls.Add(historyLayout);
                }
                else if (label4.Text == "EDIT")
                {
                    historyLayout = getEditDetails();
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

        public historymemberchangeslayout getAddDetails()
        {
            historymemberchangeslayout layout = null;  // Initialize the layout variable outside the try-catch block

            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";
            string query = "SELECT * FROM history_member WHERE ID = @changeId";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(constring))
                {
                    connection.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@changeId", label5.Text);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string name = reader["Initial_Name"].ToString().Trim();
                                string age = reader["Initial_Age"].ToString();
                                string address = reader["Initial_Address"].ToString();
                                string contact = reader["Initial_Contact"].ToString();
                                string email = reader["Initial_Email"].ToString();

                                layout = new historymemberchangeslayout(name, age, address, contact, email, "", "", "", "", "");
                                layout.Label11.Hide();
                                layout.Label12.Hide();
                                layout.Label13.Hide();
                                layout.Label14.Hide();
                                layout.Label15.Hide();
                                flowLayoutPanel1.Controls.Add(layout);
                            }
                            else
                            {
                                MessageBox.Show("Records not found.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

            // Return the layout, whether it was created or not
            return layout;
        }

        public historymemberchangeslayout getEditDetails()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";
            string query = "SELECT * FROM history_member WHERE ID = @changeId";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(constring))
                {
                    connection.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@changeId", label5.Text);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                
                                string initialName;
                                string editedName;
                                if (reader["Initial_Name"].ToString().Trim() == reader["Edited_Name"].ToString().Trim())
                                {
                                    initialName = reader["Initial_Name"].ToString();
                                    editedName = "";
                                } else
                                {
                                    initialName = reader["Initial_Name"].ToString();
                                    editedName = reader["Edited_Name"].ToString();
                                }



                                string initialAge;
                                string editedAge;
                                if (reader["Initial_Age"].ToString().Trim() == reader["Edited_Age"].ToString().Trim())
                                {
                                    initialAge = reader["Initial_Age"].ToString();
                                    editedAge = "";
                                }
                                else
                                {
                                    initialAge = reader["Initial_Age"].ToString();
                                    editedAge = reader["Edited_Age"].ToString();
                                }



                                string initialAddress;
                                string editedAddress;

                                if (reader["Initial_Address"].ToString().Trim() == reader["Edited_Address"].ToString().Trim())
                                {
                                    initialAddress = reader["Initial_Address"].ToString();
                                    editedAddress = "";
                                }
                                else
                                {
                                    initialAddress = reader["Initial_Address"].ToString();
                                    editedAddress = reader["Edited_Address"].ToString();
                                }



                                string initialContact;
                                string editedContact;
                                if (reader["Initial_Contact"].ToString().Trim() == reader["Edited_Contact"].ToString().Trim())
                                {
                                    initialContact = reader["Initial_Contact"].ToString();
                                    editedContact = "";
                                }
                                else
                                {
                                    initialContact = reader["Initial_Contact"].ToString();
                                    editedContact = reader["Edited_Contact"].ToString();
                                }




                                string initialEmail;
                                string editedEmail;
                                if (reader["Initial_Email"].ToString().Trim() == reader["Edited_Email"].ToString().Trim())
                                {
                                    initialEmail = reader["Initial_Email"].ToString();
                                    editedEmail = "";
                                }
                                else
                                {
                                    initialEmail = reader["Initial_Email"].ToString();
                                    editedEmail = reader["Edited_Email"].ToString();
                                }
                                historymemberchangeslayout layout = new historymemberchangeslayout(initialName, initialAge, initialAddress, initialContact, initialEmail, editedName, editedAge, editedAddress, editedContact, editedEmail);

                                if (editedName == "")
                                {
                                    layout.Label11.Hide();
                                }

                                if(editedAge == "")
                                {
                                    layout.Label12.Hide();
                                }

                                if (editedAddress == "")
                                {
                                    layout.Label13.Hide();
                                }
                                if(editedContact == "")
                                {
                                    layout.Label14.Hide();
                                }
                                if(editedEmail == "")
                                {
                                    layout.Label15.Hide();
                                }

                                return layout;  // Add this line to return the created layout
                            }
                            else
                            {
                                MessageBox.Show("Records not found.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

            // If no layout was created or an exception occurred, return null (or create a default layout)
            return null;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
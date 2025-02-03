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
    public partial class genreorlocation : UserControl
    {
        public genreorlocation()
        {
            InitializeComponent();
            label2.Hide();
            label3.Hide();
        }
        public genreorlocation(string name, string type, string id):this()
        {
            label1.Text = name;
            label2.Text = type;
            label3.Text = id;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (label2.Text == "Genre")
            {
                deleteGenre(label3.Text);
            }
            else if (label2.Text == "Location")
            {
                DeleteLocation(label3.Text);
            }
        }
        public void deleteGenre(string id)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    // Display a confirmation dialog
                    DialogResult result = MessageBox.Show("Are you sure you want to delete this genre?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        connection.Open();
                        string query = "DELETE FROM genre_list WHERE ID = @genreId";
                        MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                        cmdDatabase.Parameters.AddWithValue("@genreId", id);

                        int rowsAffected = cmdDatabase.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Genre deleted successfully");
                            this.Dispose();
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete genre. Check for errors and try again.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }
        public void DeleteLocation(string id)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=;database=library_of_life";

            using (MySqlConnection connection = new MySqlConnection(constring))
            {
                try
                {
                    // Display a confirmation dialog
                    DialogResult result = MessageBox.Show("Are you sure you want to delete this location?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        connection.Open();
                        string query = "DELETE FROM location_list WHERE ID = @LocationId";
                        MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                        cmdDatabase.Parameters.AddWithValue("@LocationId", id);

                        int rowsAffected = cmdDatabase.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Location deleted successfully");
                            this.Dispose();
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete location. Check for errors and try again.");
                        }
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

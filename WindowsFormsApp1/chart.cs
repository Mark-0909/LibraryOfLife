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
    public partial class chart : UserControl
    {
        public chart()
        {
            InitializeComponent();
        }
        public chart(string combobox) : this()
        {
            if (combobox == "Today")
            {
                defaultChartToday();
            } else if (combobox == "Last week")
            {
                defaultChartLastWeek();
            } else if (combobox == "Last month")
            {
                defaultChartLastMonth();
            }
        }

        public void defaultChartToday()
        {
            chart1.Series["s1"].IsValueShownAsLabel = true;
            chart2.Series["s1"].IsValueShownAsLabel = true;
            SetChartValuesToday(chart1.Series["s1"], "Return", "Returned_Date", "Status = 'Returned'");
            SetChartValuesToday(chart1.Series["s1"], "Borrow", "Borrowed_Date", "Initial_Status = 'Borrowed'");
            SetChartValuesToday(chart2.Series["s1"], "Damaged", "Returned_Date", "Violation LIKE '%DAMAGED%'");
            SetChartValuesToday(chart2.Series["s1"], "Late", "Returned_Date", "Violation LIKE '%LATE%'");
            SetChartValuesToday(chart2.Series["s1"], "Missing", "Returned_Date", "Violation LIKE '%MISSING%'");
            SetChartValuesToday(chart2.Series["s1"], "Normal", "Returned_Date", "Violation = ''");
        }

        public void defaultChartLastWeek()
        {
            chart1.Series["s1"].IsValueShownAsLabel = true;
            chart2.Series["s1"].IsValueShownAsLabel = true;
            SetChartValuesLastWeek(chart1.Series["s1"], "Return", "Returned_Date", "Status = 'Returned'");
            SetChartValuesLastWeek(chart1.Series["s1"], "Borrow", "Borrowed_Date", "Initial_Status = 'Borrowed'");
            SetChartValuesLastWeek(chart2.Series["s1"], "Damaged", "Returned_Date", "Violation LIKE '%DAMAGED%'");
            SetChartValuesLastWeek(chart2.Series["s1"], "Late", "Returned_Date", "Violation LIKE '%LATE%'");
            SetChartValuesToday(chart2.Series["s1"], "Missing", "Returned_Date", "Violation LIKE '%MISSING%'");
            SetChartValuesLastWeek(chart2.Series["s1"], "Normal", "Returned_Date", "Violation = ''");
        }
        public void defaultChartLastMonth()
        {
            chart1.Series["s1"].IsValueShownAsLabel = true;
            chart2.Series["s1"].IsValueShownAsLabel = true;
            SetChartValuesLastMonth(chart1.Series["s1"], "Return", "Returned_Date", "Status = 'Returned'");
            SetChartValuesLastMonth(chart1.Series["s1"], "Borrow", "Borrowed_Date", "Initial_Status = 'Borrowed'");
            SetChartValuesLastMonth(chart2.Series["s1"], "Damaged", "Returned_Date", "Violation LIKE '%DAMAGED%'");
            SetChartValuesLastMonth(chart2.Series["s1"], "Late", "Returned_Date", "Violation LIKE '%LATE%'");
            SetChartValuesToday(chart2.Series["s1"], "Missing", "Returned_Date", "Violation LIKE '%MISSING%'");
            SetChartValuesLastMonth(chart2.Series["s1"], "Normal", "Returned_Date", "Violation = ''");
        }
        private void SetChartValuesToday(Series series, string dataPointName, string dateColumn, string violationFilter)
        {
            using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=library_of_life;User=root;Password=;"))
            {
                try
                {
                    connection.Open();

                    string query = $"SELECT COUNT(*) FROM borrowlist WHERE {dateColumn} = @Date AND {violationFilter}";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    cmdDatabase.Parameters.AddWithValue("@Date", DateTime.Now.ToString("MM-dd-yyyy"));

                    int count = Convert.ToInt32(cmdDatabase.ExecuteScalar());

                    // Always set IsVisibleInLegend to true
                    series.IsVisibleInLegend = true;

                    if (count == 0)
                    {
                        // Add a point with null Y value to hide the label
                        series.Points.AddXY(dataPointName, (object)null);
                    }
                    else
                    {
                        // Add new point
                        series.Points.AddXY(dataPointName, count.ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.ToString()}");
                }
            }
        }




        private void SetChartValuesLastWeek(Series series, string dataPointName, string dateColumn, string violationFilter)
        {
            using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=library_of_life;User=root;Password=;"))
            {
                try
                {
                    connection.Open();

                    DateTime today = DateTime.Now;
                    DayOfWeek dayOfWeek = today.DayOfWeek;
                    string day = dayOfWeek.ToString();
                    DateTime date;
                    string startDate = string.Empty;
                    string endDate = string.Empty;

                    if (day == "Sunday")
                    {
                        date = today.AddDays(-7);
                        endDate = date.ToString("MM-dd-yyyy");
                        DateTime end = date.AddDays(-6);
                        startDate = end.ToString("MM-dd-yyyy");
                    }
                    else if (day == "Saturday")
                    {
                        date = today.AddDays(-6);
                        endDate = date.ToString("MM-dd-yyyy");
                        DateTime end = date.AddDays(-6);
                        startDate = end.ToString("MM-dd-yyyy");
                    }
                    else if (day == "Friday")
                    {
                        date = today.AddDays(-5);
                        endDate = date.ToString("MM-dd-yyyy");
                        DateTime end = date.AddDays(-6);
                        startDate = end.ToString("MM-dd-yyyy");
                    }
                    else if (day == "Thursday")
                    {
                        date = today.AddDays(-4);
                        endDate = date.ToString("MM-dd-yyyy");
                        DateTime end = date.AddDays(-6);
                        startDate = end.ToString("MM-dd-yyyy");
                    }
                    else if (day == "Wednesday")
                    {
                        date = today.AddDays(-3);
                        endDate = date.ToString("MM-dd-yyyy");
                        DateTime end = date.AddDays(-6);
                        startDate = end.ToString("MM-dd-yyyy");
                    }
                    else if (day == "Tuesday")
                    {
                        date = today.AddDays(-2);
                        endDate = date.ToString("MM-dd-yyyy");
                        DateTime end = date.AddDays(-6);
                        startDate = end.ToString("MM-dd-yyyy");
                    }
                    else if (day == "Monday")
                    {
                        date = today.AddDays(-1);
                        endDate = date.ToString("MM-dd-yyyy");
                        DateTime end = date.AddDays(-6);
                        startDate = end.ToString("MM-dd-yyyy");
                    }

                    

                    string query = $"SELECT COUNT(*) FROM borrowlist WHERE {dateColumn} >= @StartDate AND {dateColumn} <= @EndDate AND {violationFilter}";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    cmdDatabase.Parameters.AddWithValue("@StartDate", startDate);
                    cmdDatabase.Parameters.AddWithValue("@EndDate", endDate);

                    int count = Convert.ToInt32(cmdDatabase.ExecuteScalar());
                    label11.Text = $"Start - {startDate}";
                    label12.Text = $"End - {endDate}";

                    if (count == 0)
                    {
                        // Add a point with null Y value to hide the label
                        series.Points.AddXY(dataPointName, (object)null);
                    }
                    else
                    {
                        // Add new point
                        series.Points.AddXY(dataPointName, count.ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.ToString()}");
                }
            }
        }










        private void SetChartValuesLastMonth(Series series, string dataPointName, string dateColumn, string violationFilter)
        {
            using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=library_of_life;User=root;Password=;"))
            {
                try
                {
                    connection.Open();

                    DateTime date = DateTime.Now;
                    string datenumber = date.ToString("dd");
                    int subnumber = Convert.ToInt32(datenumber);
                    DateTime subdate = date.AddDays(-subnumber);
                    string subDateEnd = subdate.ToString("dd");
                    string endate = subdate.ToString("MM-dd-yyyy");
                    string startDate = string.Empty;

                    // Check different end-of-month scenarios
                    if (subDateEnd == "31" || subDateEnd == "30" || subDateEnd == "28")
                    {
                        // Assuming the endate is the last day of the month
                        DateTime start = subdate.AddDays(1 - subdate.Day); // Start of the month
                        startDate = start.ToString("MM-dd-yyyy");
                    }
                    // Add more conditions as needed for specific cases

                    string query = $"SELECT COUNT(*) FROM borrowlist WHERE {dateColumn} >= @StartDate AND {dateColumn} <= @EndDate AND {violationFilter}";
                    MySqlCommand cmdDatabase = new MySqlCommand(query, connection);
                    cmdDatabase.Parameters.AddWithValue("@StartDate", startDate);
                    cmdDatabase.Parameters.AddWithValue("@EndDate", endate);

                    int count = Convert.ToInt32(cmdDatabase.ExecuteScalar());

                    label11.Text = $"Start - {startDate}";
                    label12.Text = $"End - {endate}";
                    if (count == 0)
                    {
                        // Add a point with null Y value to hide the label
                        series.Points.AddXY(dataPointName, (object)null);
                    }
                    else
                    {
                        // Add new point
                        series.Points.AddXY(dataPointName, count.ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.ToString()}");
                }
            }
        }




    }
}

using System;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsFormsApp1
{
    public partial class memberBorrowLayout : UserControl
    {
        dropDownBooklist dropDownBooklist;
        private bool isDropDownAdded = false;

        public DateTime ReturnDate { get; set; }
        public memberBorrowLayout()
        {
            InitializeComponent();
            flowLayoutPanel1.BackColor = Color.White;

        }

        // Add properties for MemberID and ReferenceID
        public memberBorrowLayout(string referenceID, string memberID, string borrowdate, string returndate) : this()
        {
            displayLayouts(referenceID, memberID, borrowdate, returndate);
        }
        public void displayLayouts(string referenceID, string memberID, string borrowdate, string returndate)
        {
            
            label1.Text = referenceID;
            label2.Text = borrowdate;
            label3.Text = returndate;
            this.setUrgency(returndate);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Check if dropDownBooklist is already added
            if (isDropDownAdded)
            {
                // Remove existing dropDownBooklist and dispose
                flowLayoutPanel1.Controls.Remove(dropDownBooklist);
                dropDownBooklist.Dispose();
                isDropDownAdded = false;
                
            }
            else
            {
                // Create a new instance of dropDownBooklist
                dropDownBooklist = new dropDownBooklist(label1.Text);
                flowLayoutPanel1.Controls.Add(dropDownBooklist);
                isDropDownAdded = true;
            }
        }


        public void setUrgency(string initialdate)
        {
            DateTime retDate = DateTime.Parse(initialdate);
            DateTime MidUrgency = retDate.AddDays(-5);
            DateTime highUrgency = retDate.AddDays(-2);
            DateTime dateToday = DateTime.Today;

            if (dateToday >= highUrgency)
            {
                label4.Text = "HIGH";
                label4.ForeColor = Color.Red;
                this.BackColor = Color.Red;
                panel1.BackColor = Color.White;
            }
            else if (dateToday >= MidUrgency)
            {
                label4.Text = "MID";
                label4.ForeColor = Color.FromArgb(184, 134, 11);
                this.BackColor = Color.FromArgb(184, 134, 11);
                panel1.BackColor = Color.White;
            }
            else
            {
                label4.Text = "LOW";
                label4.ForeColor = Color.Green;
                this.BackColor = Color.Green;
                panel1.BackColor = Color.White;
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}

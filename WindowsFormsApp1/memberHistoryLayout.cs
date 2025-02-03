using System;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsFormsApp1
{
    public partial class memberHistoryLayout : UserControl
    {
        dropDownBooklist dropDownBooklist;
        private bool isDropDownAdded = false;

        public DateTime ReturnDate { get; set; }
        public memberHistoryLayout()
        {
            InitializeComponent();
            flowLayoutPanel1.BackColor = Color.White;

        }

        // Add properties for MemberID and ReferenceID
        public memberHistoryLayout(string referenceID, string memberID, string borrowdate, string returndate) : this()
        {
            displayLayouts(referenceID, memberID, borrowdate, returndate);
        }
        public void displayLayouts(string referenceID, string memberID, string borrowdate, string returndate)
        {
            
            label1.Text = referenceID;
            label2.Text = borrowdate;
            label3.Text = returndate;
            this.BackColor = Color.Black;
            panel1.BackColor = Color.White;
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

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class memberBorrowLayout : UserControl
    {
        public memberBorrowLayout()
        {
            InitializeComponent();
            
        }

        // Add properties for MemberID and ReferenceID
        public memberBorrowLayout(string referenceID, string memberID) : this()
        {
            displayLayouts(referenceID, memberID);
        }
        public void displayLayouts(string referenceID, string memberID)
        {
            MessageBox.Show($"Reference ID: {referenceID}, Member ID: {memberID}");
            label1.Text = referenceID;
            label2.Text = memberID;
        }

    }
}

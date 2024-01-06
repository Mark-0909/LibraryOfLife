using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp1
{
    public partial class member : UserControl
    {
        public UserControl3 memberInfoControl;

        public member()
        {
            InitializeComponent();
            memberInfoControl = new UserControl3();
        }

        public member(string memberID, string memberFName) : this()
        {
            SetMemberData(memberID, memberFName);
        }

        private void SetMemberData(string memberID, string memberFName)
        {
            label1.Text = memberID;
            label2.Text = memberFName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Library library = new Library();
            library.memberShow();
        }
    }
}

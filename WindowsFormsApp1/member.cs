using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp1
{
    
    public partial class member : UserControl
    {   
        public UserControl3 userControl3;
        public memberBorrow memberBorrow;
        public memberHistory memberHistory;
        public memberInformation memberInfoControl;

        public member()
        {
            InitializeComponent();
            userControl3 = new UserControl3();
            memberBorrow = new memberBorrow();
            memberHistory = new memberHistory();
            memberInfoControl = new memberInformation();
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
            string memberID = label1.Text; 
            
            if (ParentForm is Library libraryForm)
            {
                libraryForm.ShowUserControl1(memberID);
            }
        }

        private void member_Load(object sender, EventArgs e)
        {

        }
    }
}

using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class member : UserControl
    {
        
        public UserControl3 memberInfoControl;

        Library library = new Library();
        public memberInformation memberInfoTransfer;




        public member()
        {
            InitializeComponent();


            memberInfoControl = new UserControl3();

        }

        public member(string memberID, string memberFName)
    : this()
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
            
        }

        
    }
}

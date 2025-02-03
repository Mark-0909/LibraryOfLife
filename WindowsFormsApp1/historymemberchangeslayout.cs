using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class historymemberchangeslayout : UserControl
    {
        public Label Label11
        {
            get { return label11; }
            set { label11 = value; }
        }
        public Label Label12
        {
            get { return label12; }
            set { label12 = value; }
        }
        public Label Label13
        {
            get { return label13; }
            set { label13 = value; }
        }
        public Label Label14
        {
            get { return label14; }
            set { label14 = value; }
        }
        public Label Label15
        {
            get { return label15; }
            set { label15 = value; }
        }
        public historymemberchangeslayout()
        {
            InitializeComponent();
        }
        public historymemberchangeslayout(string iname, string iage, string iaddress, string icontact, string iemail, string ename, string eage, string eaddress, string econtact, string eemail) : this()
        {
            label6.Text = iname;
            label7.Text = iage;
            label8.Text = iaddress;
            label9.Text = icontact;
            label10.Text = iemail;

            label16.Text = ename;
            label17.Text = eage;
            label18.Text = eaddress;
            label19.Text = econtact;
            label20.Text = eemail;
        }
    }
}

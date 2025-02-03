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
    public partial class violationLayout : UserControl
    {
        public violationLayout()
        {
            InitializeComponent();
        }
        public violationLayout(string refID, string BookID, string violation): this()
        {
            displayViolation(refID, BookID, violation);
        }
        public void displayViolation(string refID, string BookID, string violation)
        {
            label1.Text = refID;
            label2.Text = BookID;
            label3.Text = violation;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void violationLayout_Load(object sender, EventArgs e)
        {

        }
    }
}

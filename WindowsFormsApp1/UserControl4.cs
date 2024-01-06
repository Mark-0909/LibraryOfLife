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
    public partial class UserControl4 : UserControl
    {
        bool isPopUpFormOpen = false;

        private memberlist memberlistControl;
        private AddMember addMemberForm;
        public UserControl4()
        {
            InitializeComponent();
            memberlistControl = new memberlist(); // Create an instance of your memberlist control
            addMemberForm = new AddMember();
            addMemberForm.MemberListForm = memberlistControl;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void UserControl4_Load(object sender, EventArgs e)
        {

        }

        

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            clickedButton(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clickedButton(sender, e);
        }
        private void ApplyButtonStyle(Button button)
        {
            button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            button.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            button.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            button.ForeColor = System.Drawing.Color.White;
            button.UseVisualStyleBackColor = false;
        }

        //Button style when not clicked
        private void ResetButtonStyle(Button button)
        {
            button.BackColor = System.Drawing.Color.Green;
            button.FlatAppearance.BorderColor = System.Drawing.Color.Green;
            button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            button.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            button.ForeColor = System.Drawing.Color.White;
            button.UseVisualStyleBackColor = false;
        }
        private void clickedButton(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            ApplyButtonStyle(clickedButton);

            for (int i = 0; i <= 2; i++)
            {
                if (i != int.Parse(clickedButton.Name.Substring("button".Length)))
                {
                    Button otherButton = Controls.Find("button" + i, true).FirstOrDefault() as Button;
                    if (otherButton != null)
                    {
                        ResetButtonStyle(otherButton);
                    }
                }
            }
        }
        

        private void button5_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void memberlist1_Load(object sender, EventArgs e)
        {

        }
        // Assuming you have an instance of memberlist in your MainForm


    }
}

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
    public partial class UserControl6 : UserControl
    {
        public UserControl6()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            clickedButton(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clickedButton(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
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

            for (int i = 0; i <= 3; i++)
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
    }
}

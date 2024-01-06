using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Library : Form
    {
        private UserControl currentActiveControl;
        member memberControl;
        public Library()
        {
            InitializeComponent();
        }

        private void trial_Load(object sender, EventArgs e)
        {

        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            clickedButton(sender, e);
            userControl11.BringToFront();
           
        }
        private void button2_Click(object sender, EventArgs e)
        {
            clickedButton(sender, e);
            userControl21.BringToFront();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            clickedButton(sender, e);

            if (currentActiveControl != userControl31)
            {
                userControl31.refreshData();
                currentActiveControl = userControl31;
            }

            userControl31.BringToFront();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            clickedButton(sender, e);
           userControl41.BringToFront();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            clickedButton(sender, e);
            userControl51.BringToFront();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            clickedButton(sender, e);
           userControl61.BringToFront();
        }
        
        //Button style when clicked
        private void ApplyButtonStyle(Button button)
        {
            button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            button.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            button.Font = new System.Drawing.Font("Calibri", 14.14286F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            button.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
        }

        //Button style when not clicked
        private void ResetButtonStyle(Button button)
        {
            button.BackColor = System.Drawing.Color.White;
            button.FlatAppearance.BorderColor = System.Drawing.Color.White;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            button.Font = new System.Drawing.Font("Calibri", 14.14286F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            button.ForeColor = System.Drawing.Color.Black;
        }
        private void clickedButton(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            ApplyButtonStyle(clickedButton);

            for (int i = 0; i <= 6; i++)
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

        private void button7_Click(object sender, EventArgs e)
        {
           
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Application.Restart();
            Environment.Exit(0);

        }
        


       

    }
}


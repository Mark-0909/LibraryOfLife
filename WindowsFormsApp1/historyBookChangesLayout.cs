
using MySql.Data.MySqlClient;
using System;


using System.Drawing;
using System.IO;

using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class historyBookChangesLayout : UserControl
    {
        public historyBookChangesLayout()
        {
            InitializeComponent();
        }

        

        public historyBookChangesLayout(string image, string iBookName, string iBookAuthor, string iBookLocation, string iBookStocks, string iBookGenre, string cname, string cauthor, string clocation, string cstocks, string cgenre) : this()
        {
            

            pictureBox1.Image = Image.FromFile(image);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            label6.Text = iBookName;
            label7.Text = iBookAuthor;
            label8.Text = iBookLocation;
            label9.Text = iBookStocks;
            label10.Text = iBookGenre;

            if(iBookName == cname || cname == string.Empty)
            {
                label20.Hide();
                label11.Hide();
            } else
            {
                label20.Text = cname;
            }

            if (iBookAuthor == cauthor || cauthor == string.Empty)
            {
                label19.Hide();
                label12.Hide();
            }
            else
            {
                label19.Text = cauthor;
            }

            if (iBookLocation == clocation || clocation == string.Empty)
            {
                label18.Hide();
                label13.Hide();
            }
            else
            {
                label18.Text = clocation;
            }

            if (iBookStocks == cstocks || cstocks == string.Empty)
            {
                label17.Hide();
                label14.Hide();
            }
            else
            {
                label17.Text = cstocks;
            }

            if (iBookGenre == cgenre || cgenre == string.Empty)
            {
                label16.Hide();
                label15.Hide();
            }
            else
            {
                label16.Text = cgenre;
            }
        }

        
    }
}

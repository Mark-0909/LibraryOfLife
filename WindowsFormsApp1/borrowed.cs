using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class borrowed : UserControl
    {   

        borrowBook borrowBook;

        public TextBox  TextBox1
        {
            get { return textBox1; }
            set { textBox1 = value; }
        }
        public borrowed()
        {
            InitializeComponent();
             
            string bookName = label1.Text;
            textBox1.Visible = false;
            
        }
        public string BOOKID;
        // Modify the constructor to accept the BOOKID
        public borrowed(borrowBook borrowBook, int bookID, string bookName, byte[] imageData) : this()
        {
            this.borrowBook = borrowBook;  // Initialize borrowBook
            SetBookData(bookName, imageData);
        
            // Set the BOOKID to the textBox1
            BOOKID = bookID.ToString();

        }


        private Image ByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream memoryStream = new MemoryStream(byteArray))
            {
                return Image.FromStream(memoryStream);
            }
        }


        public void SetBookData(string bookName, byte[] imageData)
        {
            label1.Text = bookName;
       


            Image loadedImage = ByteArrayToImage(imageData);


            pictureBox1.Image = loadedImage;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            borrowBook.DecrementDisplayedBooksCount();
            int removedBookID;
            if (int.TryParse(BOOKID, out removedBookID))
            {
                int indexToRemove = borrowBook.bookList.IndexOf(removedBookID);
        
                if (indexToRemove >= 0)
                {
                    // Remove the corresponding book ID and remark
                    borrowBook.bookList.RemoveAt(indexToRemove);
                    borrowBook.remarkList.RemoveAt(indexToRemove);
                }
            }

            // Remove the control from flowLayoutPanel1
            borrowBook.FlowLayoutPanel1.Controls.Remove(this);

            // Reposition the remaining controls in the list
            RepositionControls();

            this.Dispose();
        }





        private void RepositionControls()
        {
            int index = 0;
            foreach (borrowed control in borrowBook.FlowLayoutPanel1.Controls.OfType<borrowed>())
            {
                control.Location = new Point(5, index * (control.Height + 5));
                index++;
            }
        }




        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBox1.Visible = true;
            }
            else
            {
                textBox1.Visible = false;
                textBox1.Clear();
            }
        }
    }
}

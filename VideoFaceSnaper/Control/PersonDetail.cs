using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VideoFaceSnaper.Control
{
    public partial class PersonDetail : UserControl
    {
        public PersonDetail()
        {
            InitializeComponent(); 
        }

        public Image Pic
        {
            set { this.pic.Image = value; }
        }
        public string Rank 
        {
            set { this.lblRank.Text = value; }
        }
        public string Score
        {
            set { this.lblScore.Text = value; }
        }
        public string userName
        {
            set { this.lblName.Text = value; }
        }
        public string Sex
        {
            set { this.lblSex.Text = value; }
        }
        public string Idcard
        {
            set { this.lblCard.Text = value; }
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Forms;

namespace VideoFaceSnaper.Control
{
    public partial class PictureDisplay : MetroForm
    {
        public PictureDisplay()
        {
            InitializeComponent();

            this.BorderStyle = MetroFormBorderStyle.None;
            this.ShadowType = MetroFormShadowType.AeroShadow;
            this.metroStyleManager.Theme = MetroThemeStyle.Dark;
        }

        public PictureDisplay(string strFilePath, string strTitle): this()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            if (!string.IsNullOrEmpty(strFilePath))
            {
                this.picDisplay.Image = new Bitmap(strFilePath);
            }
            if (!string.IsNullOrEmpty(strTitle))
            {
                this.Text += ":" + strTitle;
            }

            int formwidth = this.Width - this.picDisplay.Width + this.picDisplay.Left;
            int formheight = this.Height - this.picDisplay.Height+ this.picDisplay.Top;

            this.Width = this.picDisplay.Image.Width*2 + formwidth + 2;
            this.Height = this.picDisplay.Image.Height*2 + formheight + 2;

            Closing += delegate
            {
                if (this.picDisplay.Image != null)
                {
                    this.picDisplay.Image.Dispose();
                    this.picDisplay.Image = null;
                }
            };
        }

        public PictureDisplay(Bitmap bitmapDisplay, string strTitle) : this()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            if (bitmapDisplay != null)
            {
                this.picDisplay.Image = bitmapDisplay;
            }
            if (!string.IsNullOrEmpty(strTitle))
            {
                this.Text += ":" + strTitle;
            }

            int formwidth = this.Width - this.picDisplay.Width;
            int formheight = this.Height - this.picDisplay.Height;

            this.Width = this.picDisplay.Image.Width * 2 + formwidth + 2;
            this.Height = this.picDisplay.Image.Height * 2 + formheight + 2;

            Closing += delegate
            {
                if (this.picDisplay.Image != null)
                {
                    this.picDisplay.Image.Dispose();
                    this.picDisplay.Image = null;
                }
            };
        }

    }
}

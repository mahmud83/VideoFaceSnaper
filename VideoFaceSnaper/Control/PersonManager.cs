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
using System.Globalization;
using System.Windows.Forms.VisualStyles;
using MsFaceSDK;
using VideoFace.Common.Lib;

namespace VideoFaceSnaper.Control
{
    public partial class PersonManager : MetroForm
    {
        private FaceManagerSDK _faceManager = new FaceManagerSDK();

        public PersonManager()
        {
            InitializeComponent();
            this.BorderStyle = MetroFormBorderStyle.None;
            this.ShadowType = MetroFormShadowType.AeroShadow;
            this.metroStyleManager.Theme = MetroThemeStyle.Dark;

            this.gbUserData.ForeColor = Color.White;
            this.cobGender.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cobGender.SelectedIndex = 0;

            this.FormClosing += delegate
            {
                if (this.pbImage.Image != null)
                {
                    this.pbImage.Dispose();
                }
            };
        }

        public PersonManager(byte[] faceimgdata, string strPassport) : this()
        {
            this.pbImage.Image = ImageHelper.BytesToBitmap(faceimgdata);
            this.txtUpdateUserPassport.Text = strPassport;
        }

        private void btnUserCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUserUpdate_Click(object sender, EventArgs e)
        {
            // txtUpdateUserName.Text;
            // txtUpdateUserPassport.Text;
            // cobGender.SelectedIndex.ToString();

            string passport = txtUpdateUserPassport.Text.Trim();
            if (string.IsNullOrEmpty(passport))
            {
                MessageBox.Show("请输入唯一编号!");
                return;
            }

            if (!string.IsNullOrEmpty(txtUpdateUserName.Text.Trim()))
            {
                passport += "_"+ txtUpdateUserName.Text.Trim();
            }

            bool lbRet = _faceManager.AddLibrary(this.pbImage.Image, passport);

            if (lbRet)
            {
                MessageBox.Show("加入人像库成功!");
                this.Close();
            }
        }

    }
}

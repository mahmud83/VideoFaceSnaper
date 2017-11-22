using System.Windows.Forms;
using MetroFramework.Controls;

namespace VideoFaceSnaper.Control
{
    partial class PersonManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.btnUserUpdate = new MetroFramework.Controls.MetroButton();
            this.gbUserData = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblUpdateUserPassport = new MetroFramework.Controls.MetroLabel();
            this.txtUpdateUserPassport = new MetroFramework.Controls.MetroTextBox();
            this.lblUpdateUserName = new MetroFramework.Controls.MetroLabel();
            this.txtUpdateUserName = new MetroFramework.Controls.MetroTextBox();
            this.lblGender = new MetroFramework.Controls.MetroLabel();
            this.cobGender = new MetroFramework.Controls.MetroComboBox();
            this.btnUserCancel = new MetroFramework.Controls.MetroButton();
            this.metroStyleManager = new MetroFramework.Components.MetroStyleManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            this.gbUserData.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).BeginInit();
            this.SuspendLayout();
            // 
            // pbImage
            // 
            this.pbImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbImage.Location = new System.Drawing.Point(16, 78);
            this.pbImage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new System.Drawing.Size(373, 379);
            this.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbImage.TabIndex = 64;
            this.pbImage.TabStop = false;
            // 
            // btnUserUpdate
            // 
            this.btnUserUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUserUpdate.Location = new System.Drawing.Point(143, 490);
            this.btnUserUpdate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnUserUpdate.Name = "btnUserUpdate";
            this.btnUserUpdate.Size = new System.Drawing.Size(227, 39);
            this.btnUserUpdate.TabIndex = 66;
            this.btnUserUpdate.Text = "添加人像库";
            this.btnUserUpdate.UseSelectable = true;
            this.btnUserUpdate.Click += new System.EventHandler(this.btnUserUpdate_Click);
            // 
            // gbUserData
            // 
            this.gbUserData.Controls.Add(this.tableLayoutPanel1);
            this.gbUserData.Location = new System.Drawing.Point(413, 69);
            this.gbUserData.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gbUserData.Name = "gbUserData";
            this.gbUserData.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gbUserData.Size = new System.Drawing.Size(445, 389);
            this.gbUserData.TabIndex = 65;
            this.gbUserData.TabStop = false;
            this.gbUserData.Text = "人员信息";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.77922F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 59.22078F));
            this.tableLayoutPanel1.Controls.Add(this.lblUpdateUserPassport, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtUpdateUserPassport, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblUpdateUserName, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtUpdateUserName, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblGender, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.cobGender, 1, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(21, 47);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(411, 313);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // lblUpdateUserPassport
            // 
            this.lblUpdateUserPassport.AutoSize = true;
            this.lblUpdateUserPassport.Location = new System.Drawing.Point(4, 0);
            this.lblUpdateUserPassport.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUpdateUserPassport.Name = "lblUpdateUserPassport";
            this.lblUpdateUserPassport.Size = new System.Drawing.Size(87, 20);
            this.lblUpdateUserPassport.TabIndex = 11;
            this.lblUpdateUserPassport.Text = "唯一编号(*)";
            // 
            // txtUpdateUserPassport
            // 
            this.txtUpdateUserPassport.CustomButton.Image = null;
            this.txtUpdateUserPassport.CustomButton.Location = new System.Drawing.Point(200, 1);
            this.txtUpdateUserPassport.CustomButton.Name = "";
            this.txtUpdateUserPassport.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtUpdateUserPassport.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtUpdateUserPassport.CustomButton.TabIndex = 1;
            this.txtUpdateUserPassport.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtUpdateUserPassport.CustomButton.UseSelectable = true;
            this.txtUpdateUserPassport.CustomButton.Visible = false;
            this.txtUpdateUserPassport.Lines = new string[0];
            this.txtUpdateUserPassport.Location = new System.Drawing.Point(171, 3);
            this.txtUpdateUserPassport.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtUpdateUserPassport.MaxLength = 90;
            this.txtUpdateUserPassport.Name = "txtUpdateUserPassport";
            this.txtUpdateUserPassport.PasswordChar = '\0';
            this.txtUpdateUserPassport.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtUpdateUserPassport.SelectedText = "";
            this.txtUpdateUserPassport.SelectionLength = 0;
            this.txtUpdateUserPassport.SelectionStart = 0;
            this.txtUpdateUserPassport.ShortcutsEnabled = true;
            this.txtUpdateUserPassport.Size = new System.Drawing.Size(224, 25);
            this.txtUpdateUserPassport.TabIndex = 65;
            this.txtUpdateUserPassport.UseSelectable = true;
            this.txtUpdateUserPassport.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtUpdateUserPassport.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // lblUpdateUserName
            // 
            this.lblUpdateUserName.AutoSize = true;
            this.lblUpdateUserName.Location = new System.Drawing.Point(4, 38);
            this.lblUpdateUserName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUpdateUserName.Name = "lblUpdateUserName";
            this.lblUpdateUserName.Size = new System.Drawing.Size(41, 20);
            this.lblUpdateUserName.TabIndex = 8;
            this.lblUpdateUserName.Text = "姓名";
            // 
            // txtUpdateUserName
            // 
            this.txtUpdateUserName.CustomButton.Image = null;
            this.txtUpdateUserName.CustomButton.Location = new System.Drawing.Point(200, 1);
            this.txtUpdateUserName.CustomButton.Name = "";
            this.txtUpdateUserName.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtUpdateUserName.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtUpdateUserName.CustomButton.TabIndex = 1;
            this.txtUpdateUserName.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtUpdateUserName.CustomButton.UseSelectable = true;
            this.txtUpdateUserName.CustomButton.Visible = false;
            this.txtUpdateUserName.Lines = new string[0];
            this.txtUpdateUserName.Location = new System.Drawing.Point(171, 41);
            this.txtUpdateUserName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtUpdateUserName.MaxLength = 90;
            this.txtUpdateUserName.Name = "txtUpdateUserName";
            this.txtUpdateUserName.PasswordChar = '\0';
            this.txtUpdateUserName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtUpdateUserName.SelectedText = "";
            this.txtUpdateUserName.SelectionLength = 0;
            this.txtUpdateUserName.SelectionStart = 0;
            this.txtUpdateUserName.ShortcutsEnabled = true;
            this.txtUpdateUserName.Size = new System.Drawing.Size(224, 25);
            this.txtUpdateUserName.TabIndex = 63;
            this.txtUpdateUserName.UseSelectable = true;
            this.txtUpdateUserName.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtUpdateUserName.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // lblGender
            // 
            this.lblGender.AutoSize = true;
            this.lblGender.Location = new System.Drawing.Point(4, 80);
            this.lblGender.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(41, 20);
            this.lblGender.TabIndex = 9;
            this.lblGender.Text = "性别";
            // 
            // cobGender
            // 
            this.cobGender.FormattingEnabled = true;
            this.cobGender.ItemHeight = 24;
            this.cobGender.Items.AddRange(new object[] {
            "未知",
            "男",
            "女"});
            this.cobGender.Location = new System.Drawing.Point(170, 83);
            this.cobGender.Name = "cobGender";
            this.cobGender.Size = new System.Drawing.Size(224, 30);
            this.cobGender.TabIndex = 64;
            this.cobGender.UseSelectable = true;
            // 
            // btnUserCancel
            // 
            this.btnUserCancel.Location = new System.Drawing.Point(481, 490);
            this.btnUserCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnUserCancel.Name = "btnUserCancel";
            this.btnUserCancel.Size = new System.Drawing.Size(227, 39);
            this.btnUserCancel.TabIndex = 67;
            this.btnUserCancel.Text = "关闭";
            this.btnUserCancel.UseSelectable = true;
            this.btnUserCancel.Click += new System.EventHandler(this.btnUserCancel_Click);
            // 
            // metroStyleManager
            // 
            this.metroStyleManager.Owner = this;
            // 
            // PersonManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 549);
            this.Controls.Add(this.btnUserCancel);
            this.Controls.Add(this.btnUserUpdate);
            this.Controls.Add(this.gbUserData);
            this.Controls.Add(this.pbImage);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PersonManager";
            this.StyleManager = this.metroStyleManager;
            this.Text = "详细信息";
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            this.gbUserData.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbImage;
        private MetroFramework.Controls.MetroButton btnUserUpdate;
        private System.Windows.Forms.GroupBox gbUserData;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private MetroFramework.Controls.MetroLabel lblUpdateUserName;
        private MetroFramework.Controls.MetroLabel lblGender;
        private MetroFramework.Controls.MetroTextBox txtUpdateUserName;
        private MetroFramework.Controls.MetroComboBox cobGender;
        private MetroFramework.Controls.MetroLabel lblUpdateUserPassport;
        private MetroFramework.Controls.MetroTextBox txtUpdateUserPassport;
        private MetroFramework.Controls.MetroButton btnUserCancel;
        private MetroFramework.Components.MetroStyleManager metroStyleManager;
    }
}
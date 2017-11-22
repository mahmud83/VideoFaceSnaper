using System.Windows.Forms;

namespace VideoFaceSnaper
{
    partial class VideoImgSearch
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
            this.panel1 = new MetroFramework.Controls.MetroPanel();
            this.txtScore = new MetroFramework.Controls.MetroTextBox();
            this.label2 = new MetroFramework.Controls.MetroLabel();
            this.label1 = new MetroFramework.Controls.MetroLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnAddLibrary = new MetroFramework.Controls.MetroButton();
            this.btnSearch = new MetroFramework.Controls.MetroButton();
            this.btnSelect = new MetroFramework.Controls.MetroButton();
            this.lstSearch = new Manina.Windows.Forms.ImageListView();
            this.panel2 = new MetroFramework.Controls.MetroPanel();
            this.metroStyleManager = new MetroFramework.Components.MetroStyleManager(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtScore);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.btnAddLibrary);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.btnSelect);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(253, 747);
            this.panel1.TabIndex = 0;
            // 
            // txtScore
            // 
            this.txtScore.Location = new System.Drawing.Point(62, 539);
            this.txtScore.Name = "txtScore";
            this.txtScore.Size = new System.Drawing.Size(109, 25);
            this.txtScore.TabIndex = 6;
            this.txtScore.Text = "0.8";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(59, 509);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "搜索阈值设置：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "请选择人脸图像：";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(3, 41);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(240, 250);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.TabStop = false;
            // 
            // btnAddLibrary
            // 
            this.btnAddLibrary.Location = new System.Drawing.Point(59, 433);
            this.btnAddLibrary.Name = "btnAddLibrary";
            this.btnAddLibrary.Size = new System.Drawing.Size(108, 23);
            this.btnAddLibrary.TabIndex = 2;
            this.btnAddLibrary.Text = "加入人像库";
            this.btnAddLibrary.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(59, 383);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(108, 23);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "搜索";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(59, 326);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(108, 23);
            this.btnSelect.TabIndex = 0;
            this.btnSelect.Text = "选择图像";
            this.btnSelect.UseVisualStyleBackColor = true;
            // 
            // lstSearch
            // 
            this.lstSearch.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lstSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstSearch.Dock = DockStyle.Fill;
            this.lstSearch.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lstSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lstSearch.Location = new System.Drawing.Point(0, 0);
            this.lstSearch.Margin = new Padding(3, 3, 3, 3);
            this.lstSearch.Name = "lstSearch";
            this.lstSearch.Size = new System.Drawing.Size(910, 745);
            this.lstSearch.TabIndex = 2;
            this.lstSearch.ItemDoubleClick += new Manina.Windows.Forms.ItemDoubleClickEventHandler(this.lstSearch_ItemDoubleClick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lstSearch);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(253, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(914, 747);
            this.panel2.TabIndex = 1;
            // 
            // metroStyleManager
            // 
            this.metroStyleManager.Owner = this;
            // 
            // VideoImgSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 768);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MinimizeBox = false;
            this.Padding = new System.Windows.Forms.Padding(0, 60, 0, 0);
            this.ShowIcon = false;
            this.StyleManager = this.metroStyleManager;
            this.Name = "VideoImgSearch";
            this.Text = "人脸图像搜索";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VideoImgSearch_FormClosing);
            this.Load += new System.EventHandler(this.VideoImgSearch_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroPanel panel1;
        private MetroFramework.Controls.MetroPanel panel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private MetroFramework.Controls.MetroButton btnAddLibrary;
        private MetroFramework.Controls.MetroButton btnSearch;
        private MetroFramework.Controls.MetroButton btnSelect;
        private MetroFramework.Controls.MetroTextBox txtScore;
        private MetroFramework.Controls.MetroLabel label2;
        private MetroFramework.Controls.MetroLabel label1;
        private Manina.Windows.Forms.ImageListView lstSearch;
        private MetroFramework.Components.MetroStyleManager metroStyleManager;
    }
}
using System.Windows.Forms;

namespace VideoFaceSnaper
{
    partial class VideoSurveilance
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
            this.imageBox1 = new System.Windows.Forms.PictureBox();
            this.splitpanel1 = new MetroFramework.Controls.MetroPanel();
            this.splitpanel2 = new MetroFramework.Controls.MetroPanel();
            this.panel1 = new MetroFramework.Controls.MetroPanel();
            this.label1 = new MetroFramework.Controls.MetroLabel();
            this.listBox1 = new System.Windows.Forms.Panel();
            this.panel2 = new MetroFramework.Controls.MetroPanel();
            this.label2 = new MetroFramework.Controls.MetroLabel();
            this.metroStyleManager = new MetroFramework.Components.MetroStyleManager(this.components);
            this.splitpanel1.SuspendLayout();
            this.splitpanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).BeginInit();
            this.SuspendLayout();

            // 
            // splitpanel1
            // 
            this.splitpanel1.Controls.Add(this.imageBox1);
            this.splitpanel1.Controls.Add(this.panel1);
            this.splitpanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitpanel1.Location = new System.Drawing.Point(0, 0);
            this.splitpanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.splitpanel1.Name = "splitpanel1";
            this.splitpanel1.Size = new System.Drawing.Size(956, 749);
            this.splitpanel1.TabIndex = 0;
            // 
            // splitpanel2
            // 
            this.splitpanel2.Controls.Add(this.listBox1);
            this.splitpanel2.Controls.Add(this.panel2);
            this.splitpanel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitpanel2.Location = new System.Drawing.Point(0, 0);
            this.splitpanel2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.splitpanel2.Name = "splitpanel2";
            this.splitpanel2.Size = new System.Drawing.Size(397, 749);
            this.splitpanel2.TabIndex = 0;
            // 
            // imageBox1
            // 
            this.imageBox1.BackColor = System.Drawing.SystemColors.WindowText;
            this.imageBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBox1.Location = new System.Drawing.Point(0, 40);
            this.imageBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.Size = new System.Drawing.Size(956, 709);
            this.imageBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBox1.TabIndex = 2;
            this.imageBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(956, 40);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "实时视频图像";
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.listBox1.Location = new System.Drawing.Point(-3, 40);
            this.listBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(397, 709);
            this.listBox1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(394, 40);
            this.panel2.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 15);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "抓拍人脸图像";
            // 
            // metroStyleManager
            // 
            this.metroStyleManager.Owner = this;
            // 
            // VideoSurveilance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1355, 809);
            this.Controls.Add(this.splitpanel1);
            this.Controls.Add(this.splitpanel2);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimizeBox = false;
            this.Name = "VideoSurveilance";
            this.Padding = new System.Windows.Forms.Padding(0, 60, 0, 0);
            this.ShowIcon = false;
            this.StyleManager = this.metroStyleManager;
            this.Text = "视频人脸采集";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VideoSurveilance_FormClosing);
            this.Load += new System.EventHandler(this.VideoSurveilance_Load);
            this.SizeChanged += new System.EventHandler(this.VideoSurveilance_SizeChanged);
            this.splitpanel1.ResumeLayout(false);
            this.splitpanel1.PerformLayout();
            this.splitpanel2.ResumeLayout(false);
            this.splitpanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroPanel splitpanel1;
        private MetroFramework.Controls.MetroPanel splitpanel2;
        private MetroFramework.Controls.MetroPanel panel1;
        private MetroFramework.Controls.MetroPanel panel2;
        private System.Windows.Forms.PictureBox imageBox1;
        private MetroFramework.Controls.MetroLabel label1;
        private MetroFramework.Controls.MetroLabel label2;
        private System.Windows.Forms.Panel listBox1;
        private MetroFramework.Components.MetroStyleManager metroStyleManager;

    }
}
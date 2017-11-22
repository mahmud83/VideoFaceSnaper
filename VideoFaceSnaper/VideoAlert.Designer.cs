using MetroFramework.Properties;

namespace VideoFaceSnaper
{
    partial class VideoAlert
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
            this.plNewSnap = new MetroFramework.Controls.MetroPanel();
            this.plRight = new MetroFramework.Controls.MetroPanel();
            this.lblAlarmHistory = new MetroFramework.Controls.MetroLabel();
            this.plAlarmHistory = new MetroFramework.Controls.MetroPanel();
            this.tsRight = new System.Windows.Forms.ToolStrip();
            this.tsbtnClear = new System.Windows.Forms.ToolStripButton();
            this.pbVideo = new System.Windows.Forms.PictureBox();
            this.plVideo = new MetroFramework.Controls.MetroPanel();
            this.metroStyleManager = new MetroFramework.Components.MetroStyleManager(this.components);
            this.plRight.SuspendLayout();
            this.tsRight.SuspendLayout();
            this.plNewSnap.SuspendLayout();
            this.plVideo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbVideo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).BeginInit();
            this.SuspendLayout();
            // 
            // plNewSnap
            // 
            this.plNewSnap.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.plNewSnap.Location = new System.Drawing.Point(0, 500);
            this.plNewSnap.Margin = new System.Windows.Forms.Padding(4);
            this.plNewSnap.Name = "plNewSnap";
            this.plNewSnap.Size = new System.Drawing.Size(1178, 144);
            this.plNewSnap.TabIndex = 38;
            this.plNewSnap.TabStop = true;
            // 
            // plRight
            // 
            this.plRight.Controls.Add(this.lblAlarmHistory);
            this.plRight.Controls.Add(this.plAlarmHistory);
            this.plRight.Controls.Add(this.tsRight);
            this.plRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.plRight.HorizontalScrollbarBarColor = true;
            this.plRight.HorizontalScrollbarHighlightOnWheel = false;
            this.plRight.HorizontalScrollbarSize = 10;
            this.plRight.Location = new System.Drawing.Point(1178, 60);
            this.plRight.Margin = new System.Windows.Forms.Padding(4);
            this.plRight.Name = "plRight";
            this.plRight.Size = new System.Drawing.Size(346, 804);
            this.plRight.TabIndex = 11;
            this.plRight.VerticalScrollbarBarColor = true;
            this.plRight.VerticalScrollbarHighlightOnWheel = false;
            this.plRight.VerticalScrollbarSize = 10;
            // 
            // lblAlarmHistory
            // 
            this.lblAlarmHistory.AutoSize = true;
            this.lblAlarmHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(79)))), ((int)(((byte)(79)))));
            this.lblAlarmHistory.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblAlarmHistory.ForeColor = System.Drawing.Color.White;
            this.lblAlarmHistory.Location = new System.Drawing.Point(104, 9);
            this.lblAlarmHistory.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAlarmHistory.Name = "lblAlarmHistory";
            this.lblAlarmHistory.Size = new System.Drawing.Size(73, 20);
            this.lblAlarmHistory.TabIndex = 15;
            this.lblAlarmHistory.Text = "报警历史";
            // 
            // plAlarmHistory
            // 
            this.plAlarmHistory.AutoScroll = true;
            this.plAlarmHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plAlarmHistory.Location = new System.Drawing.Point(0, 29);
            this.plAlarmHistory.Margin = new System.Windows.Forms.Padding(2);
            this.plAlarmHistory.Name = "plAlarmHistory";
            this.plAlarmHistory.Size = new System.Drawing.Size(346, 775);
            this.plAlarmHistory.TabIndex = 39;
            // 
            // tsRight
            // 
            this.tsRight.AutoSize = false;
            this.tsRight.BackColor = System.Drawing.Color.Black;
            this.tsRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tsRight.Font = new System.Drawing.Font("Arial", 8F);
            this.tsRight.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsRight.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.tsRight.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnClear});
            this.tsRight.Location = new System.Drawing.Point(0, 0);
            this.tsRight.Name = "tsRight";
            this.tsRight.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tsRight.Size = new System.Drawing.Size(346, 29);
            this.tsRight.TabIndex = 38;
            // 
            // tsbtnClear
            // 
            this.tsbtnClear.AutoSize = false;
            this.tsbtnClear.BackColor = System.Drawing.Color.Transparent;
            this.tsbtnClear.BackgroundImage = global::VideoFaceSnaper.Properties.Resources.Clear2;
            this.tsbtnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tsbtnClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnClear.Name = "tsbtnClear";
            this.tsbtnClear.Size = new System.Drawing.Size(20, 20);
            this.tsbtnClear.Text = "清空列表";
            this.tsbtnClear.ToolTipText = "清空列表";
            this.tsbtnClear.Click += new System.EventHandler(this.TsbtnClear_Click);
            // 
            // plVideo
            // 
            this.plVideo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plVideo.Controls.Add(this.pbVideo);
            this.plVideo.Enabled = false;
            this.plVideo.Location = new System.Drawing.Point(0, 60);
            this.plVideo.Size = new System.Drawing.Size(1178, 656);
            this.plVideo.Name = "plVideo";
            // 
            // pbVideo
            // 
            this.pbVideo.BackColor = System.Drawing.Color.Black;
            this.pbVideo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbVideo.Location = new System.Drawing.Point(0, 0);
            this.pbVideo.Margin = new System.Windows.Forms.Padding(4);
            this.pbVideo.Name = "pbVideo";
            this.pbVideo.Size = new System.Drawing.Size(1178, 656);
            this.pbVideo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbVideo.TabIndex = 14;
            this.pbVideo.TabStop = false;
            // 
            // metroStyleManager
            // 
            this.metroStyleManager.Owner = this;
            // 
            // VideoAlert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1524, 864);
            this.Controls.Add(this.plVideo);
            this.Controls.Add(this.plNewSnap);
            this.Controls.Add(this.plRight);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "VideoAlert";
            this.Padding = new System.Windows.Forms.Padding(0, 60, 0, 0);
            this.StyleManager = this.metroStyleManager;
            this.Text = "布控报警";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VideoAlert_FormClosing);
            this.Load += new System.EventHandler(this.VideoAlert_Load);
            this.SizeChanged += new System.EventHandler(this.VideoAlert_SizeChanged);
            this.plRight.ResumeLayout(false);
            this.plRight.PerformLayout();
            this.tsRight.ResumeLayout(false);
            this.tsRight.PerformLayout();
            this.plNewSnap.ResumeLayout(false);
            this.plNewSnap.PerformLayout();
            this.plVideo.ResumeLayout(false);
            this.plVideo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbVideo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).EndInit();
            this.ResumeLayout(false);

        }

        private void TsbtnClear_Click1(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        private MetroFramework.Controls.MetroPanel plNewSnap;
        private MetroFramework.Controls.MetroPanel plRight;
        private MetroFramework.Controls.MetroPanel plAlarmHistory;
        private System.Windows.Forms.ToolStrip tsRight;
        private MetroFramework.Controls.MetroLabel lblAlarmHistory;
        private System.Windows.Forms.ToolStripButton tsbtnClear;
        private MetroFramework.Controls.MetroPanel plVideo;
        private System.Windows.Forms.PictureBox pbVideo;
        private MetroFramework.Components.MetroStyleManager metroStyleManager;
    }
}
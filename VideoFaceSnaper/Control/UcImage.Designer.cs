using System.Drawing;

namespace VideoFaceSnaper.Control
{
    partial class UcImage
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.cms = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiAddModel = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.lblAlarmMsg = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            this.cms.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbImage
            // 
            this.pbImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbImage.ContextMenuStrip = this.cms;
            this.pbImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbImage.Location = new System.Drawing.Point(0, 0);
            this.pbImage.Margin = new System.Windows.Forms.Padding(4);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new System.Drawing.Size(267, 334);
            this.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbImage.TabIndex = 0;
            this.pbImage.TabStop = false;
            // 
            // cms
            // 
            this.cms.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSearch,
            this.tsmiAddModel});
            this.cms.Name = "contextMenuStrip1";
            this.cms.Size = new System.Drawing.Size(109, 28);
            // 
            // tsmiSearch
            // 
            this.tsmiSearch.Name = "tsmiSearch";
            this.tsmiSearch.Size = new System.Drawing.Size(108, 24);
            this.tsmiSearch.Text = "人像搜索";
            this.tsmiSearch.Click += new System.EventHandler(this.tsmiSearch_Click);
            // 
            // tsmiAddModel
            // 
            this.tsmiAddModel.Name = "tsmiAddModel";
            this.tsmiAddModel.Size = new System.Drawing.Size(108, 24);
            this.tsmiAddModel.Text = "添加人像库";
            this.tsmiAddModel.Click += new System.EventHandler(this.tsmiAddModel_Click);
            // 
            // lblAlarmMsg
            // 
            this.lblAlarmMsg.AutoSize = true;
            this.lblAlarmMsg.BackColor = System.Drawing.Color.Transparent;
            this.lblAlarmMsg.Location = new System.Drawing.Point(56, 289);
            this.lblAlarmMsg.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAlarmMsg.Name = "lblAlarmMsg";
            this.lblAlarmMsg.Size = new System.Drawing.Size(159, 15);
            this.lblAlarmMsg.TabIndex = 3;
            this.lblAlarmMsg.Text = "2017-01-01 00:00:00";
            // 
            // UcImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblAlarmMsg);
            this.Controls.Add(this.pbImage);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UcImage";
            this.Size = new System.Drawing.Size(267, 334);
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            this.cms.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.PictureBox pbImage;
        private System.Windows.Forms.Label lblAlarmMsg;
        public System.Windows.Forms.ContextMenuStrip cms;
        private System.Windows.Forms.ToolStripMenuItem tsmiAddModel;
        private System.Windows.Forms.ToolStripMenuItem tsmiSearch;
    }
}

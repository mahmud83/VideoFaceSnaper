namespace VideoFace.CoreNetApiTest
{
    partial class Form1
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStar = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.picVideo = new System.Windows.Forms.PictureBox();
            this.picFace = new System.Windows.Forms.PictureBox();
            this.labFaceID = new System.Windows.Forms.Label();
            this.labScore = new System.Windows.Forms.Label();
            this.labNumber = new System.Windows.Forms.Label();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.minTxt = new System.Windows.Forms.TextBox();
            this.maxTxt = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picVideo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFace)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStar
            // 
            this.btnStar.Location = new System.Drawing.Point(873, 696);
            this.btnStar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStar.Name = "btnStar";
            this.btnStar.Size = new System.Drawing.Size(100, 29);
            this.btnStar.TabIndex = 0;
            this.btnStar.Text = "开始";
            this.btnStar.UseVisualStyleBackColor = true;
            this.btnStar.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(1197, 699);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(100, 29);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "结束";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // picVideo
            // 
            this.picVideo.Location = new System.Drawing.Point(16, 15);
            this.picVideo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.picVideo.Name = "picVideo";
            this.picVideo.Size = new System.Drawing.Size(957, 612);
            this.picVideo.TabIndex = 2;
            this.picVideo.TabStop = false;
            // 
            // picFace
            // 
            this.picFace.Location = new System.Drawing.Point(981, 15);
            this.picFace.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.picFace.Name = "picFace";
            this.picFace.Size = new System.Drawing.Size(653, 612);
            this.picFace.TabIndex = 3;
            this.picFace.TabStop = false;
            // 
            // labFaceID
            // 
            this.labFaceID.AutoSize = true;
            this.labFaceID.Location = new System.Drawing.Point(1123, 658);
            this.labFaceID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labFaceID.Name = "labFaceID";
            this.labFaceID.Size = new System.Drawing.Size(55, 15);
            this.labFaceID.TabIndex = 4;
            this.labFaceID.Text = "label1";
            // 
            // labScore
            // 
            this.labScore.AutoSize = true;
            this.labScore.Location = new System.Drawing.Point(1235, 658);
            this.labScore.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labScore.Name = "labScore";
            this.labScore.Size = new System.Drawing.Size(55, 15);
            this.labScore.TabIndex = 5;
            this.labScore.Text = "label1";
            // 
            // labNumber
            // 
            this.labNumber.AutoSize = true;
            this.labNumber.Location = new System.Drawing.Point(1353, 658);
            this.labNumber.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labNumber.Name = "labNumber";
            this.labNumber.Size = new System.Drawing.Size(55, 15);
            this.labNumber.TabIndex = 6;
            this.labNumber.Text = "label1";
            // 
            // txtIp
            // 
            this.txtIp.Location = new System.Drawing.Point(581, 654);
            this.txtIp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(391, 25);
            this.txtIp.TabIndex = 7;
            this.txtIp.Text = "rtsp://admin:12345@172.31.108.249:554/h264/ch1/main/av_stream";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(39, 640);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(463, 119);
            this.richTextBox1.TabIndex = 8;
            this.richTextBox1.Text = "";
            // 
            // minTxt
            // 
            this.minTxt.Location = new System.Drawing.Point(581, 699);
            this.minTxt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.minTxt.Name = "minTxt";
            this.minTxt.Size = new System.Drawing.Size(132, 25);
            this.minTxt.TabIndex = 9;
            this.minTxt.Text = "0.05";
            // 
            // maxTxt
            // 
            this.maxTxt.Location = new System.Drawing.Point(724, 698);
            this.maxTxt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.maxTxt.Name = "maxTxt";
            this.maxTxt.Size = new System.Drawing.Size(132, 25);
            this.maxTxt.TabIndex = 10;
            this.maxTxt.Text = "0.5";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1651, 775);
            this.Controls.Add(this.maxTxt);
            this.Controls.Add(this.minTxt);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.txtIp);
            this.Controls.Add(this.labNumber);
            this.Controls.Add(this.labScore);
            this.Controls.Add(this.labFaceID);
            this.Controls.Add(this.picFace);
            this.Controls.Add(this.picVideo);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStar);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picVideo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFace)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStar;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.PictureBox picVideo;
        private System.Windows.Forms.PictureBox picFace;
        private System.Windows.Forms.Label labFaceID;
        private System.Windows.Forms.Label labScore;
        private System.Windows.Forms.Label labNumber;
        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox minTxt;
        private System.Windows.Forms.TextBox maxTxt;
    }
}


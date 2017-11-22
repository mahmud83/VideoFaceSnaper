using MetroFramework;
using MetroFramework.Forms;
using MsFaceSDK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using VideoFace.Common;
using VideoFaceSnaper.Model;

namespace VideoFaceSnaper
{
    public partial class VideoSurveilance : MetroForm
    {
        private VideoFaceProxy _proxy = new VideoFaceProxy();
        private int _pictureboxWidth, _pictureboxHeight;
        private const int _listboxcontrolCnt = 10;
        private int _changecontrolCnt;

        public VideoSurveilance()
        {
            InitializeComponent();
            this.BorderStyle = MetroFormBorderStyle.None;
            this.ShadowType = MetroFormShadowType.AeroShadow;
            this.metroStyleManager.Theme = MetroThemeStyle.Dark;
        }

        private void NoticeFrameEvent(FaceDetectInfo detInfo)
        {
            Invoke((MethodInvoker) delegate()
            {
                Image oldImage = imageBox1.Image;

                if ((detInfo.Rects != null) && (detInfo.Rects.Count > 0))
                {
                    DrawRectangle(detInfo.Img, detInfo.Rects);
                }
                Bitmap newImage = new Bitmap(detInfo.Img, imageBox1.Size);
                imageBox1.Image = newImage;
                detInfo.Img.Dispose();

                if (oldImage != null)
                {
                    oldImage.Dispose();
                }
           });
        }

        /// <summary>
        /// 在Image基础上画矩形
        /// </summary>
        /// <param name="img">照片</param>
        /// <param name="listRect">矩形列表</param>
        /// <returns>加上矩形的照片</returns>
        private void DrawRectangle(Image img, List<Rectangle> listRect)
        {
            try
            {
                using (Graphics g = Graphics.FromImage(img))
                {
                    Pen pen = new Pen(Brushes.Blue, 2);
                    foreach (Rectangle rect in listRect)
                    {
                        g.DrawRectangle(pen, rect);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.Info("DrawRectangle绘制图像出现异常：" + ex.Message);
            }
        }

        private void NoticeFaceEvent(SnapVideoImage videoImg)
        {
            BeginInvoke((MethodInvoker) delegate()
            {
                if (videoImg != null && videoImg.Img != null)
                {
                    for (int ie = _listboxcontrolCnt + _changecontrolCnt - 1; ie >= 1; ie--)
                    {
                        PictureBox pbctrl = (PictureBox) this.listBox1.Controls[ie - 1];
                        PictureBox pbnextctrl = (PictureBox) this.listBox1.Controls[ie];
                        if ((pbctrl != null) && (pbnextctrl != null) && (pbctrl.Image != null))
                        {
                            if (ie == _listboxcontrolCnt+ _changecontrolCnt - 1 && pbnextctrl.Image != null)
                            {
                                pbnextctrl.Image.Dispose();
                            }
                            pbnextctrl.Image = pbctrl.Image;
                        }
                    }

                    Bitmap newImage = new Bitmap(videoImg.Img, new Size(_pictureboxWidth, _pictureboxHeight));
                    PictureBox firstctrl = (PictureBox) this.listBox1.Controls[0];
                    if (firstctrl != null)
                    {
                        firstctrl.Image = newImage;
                    }
                    videoImg.Img.Dispose();
                }
            });
        }

        private void VideoSurveilance_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("你确定要关闭程序？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) ==
                DialogResult.No)
            {
                e.Cancel = true;
                return;
            }

            _proxy.NoticeFrameEvent -= NoticeFrameEvent;
            _proxy.NoticeFaceEvent -= NoticeFaceEvent;
            _proxy.UnLoad();
            Log4NetHelper.Instance.Info("客户端应用退出");
        }

        private void VideoSurveilance_Load(object sender, EventArgs e)
        {
            _pictureboxWidth = (int)(this.listBox1.Width/2D);
            _pictureboxHeight = (int) (_pictureboxWidth * 3D/4D);

            for (int index = 1; index <= _listboxcontrolCnt; index++)
            {
                PictureBox picturebox = new PictureBox();
                picturebox.Name = "no"+ index.ToString();
                picturebox.Width = _pictureboxWidth;
                picturebox.Height = _pictureboxHeight;
                picturebox.SizeMode = PictureBoxSizeMode.Zoom;

                // 偶数
                if (index % 2 == 1)
                {
                    picturebox.Left = 0;
                    picturebox.Top = (index - 1)/2 * _pictureboxHeight;
                }
                else
                {
                    picturebox.Left = _pictureboxWidth;
                    picturebox.Top = (index-1)/2 * _pictureboxHeight;
                }
                this.listBox1.Controls.Add(picturebox);
            }

            try
            {
                _proxy.Initial();
                _proxy.NoticeFrameEvent += NoticeFrameEvent;
                _proxy.NoticeFaceEvent += NoticeFaceEvent;
                Log4NetHelper.Instance.Info("客户端应用启动");
            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.Info("VideoSurveilance加载摄像机数据出现异常：" + ex.Message);
                MessageBox.Show(ex.Message);
            }
        }

        private void VideoSurveilance_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal && this.Created)
            {
                if (this.listBox1.Controls.Count == _listboxcontrolCnt + _changecontrolCnt)
                {
                    int backchanegelistboxcontrolCnt = _changecontrolCnt;
                    _changecontrolCnt = 0; // 优先调整
                    for (int index = _listboxcontrolCnt + backchanegelistboxcontrolCnt; index >= _listboxcontrolCnt + 1; index--)
                    {
                        string pictureboxKey = "no" + index.ToString();
                        PictureBox pbox = (PictureBox)this.listBox1.Controls[pictureboxKey];
                        if (pbox != null)
                        {
                            if (pbox.Image != null)
                            {
                                pbox.Image.Dispose();
                            }
                            this.listBox1.Controls.RemoveByKey(pictureboxKey);
                        }
                    }
                }
            }
            else if (WindowState == FormWindowState.Maximized && this.Created)
            {
                if (this.listBox1.Controls.Count == _listboxcontrolCnt)
                {
                    int newlistboxcontrolCnt = (int)Math.Round(this.listBox1.Height * 1.0D / _pictureboxHeight)* 2;
                    for (int index = _listboxcontrolCnt + 1; index <= newlistboxcontrolCnt; index++)
                    {
                        PictureBox picturebox = new PictureBox();
                        picturebox.Name = "no" + index.ToString();
                        picturebox.Width = _pictureboxWidth;
                        picturebox.Height = _pictureboxHeight;
                        picturebox.SizeMode = PictureBoxSizeMode.Zoom;

                        // 偶数
                        if (index % 2 == 1)
                        {
                            picturebox.Left = 0;
                            picturebox.Top = (index - 1) / 2 * _pictureboxHeight;
                        }
                        else
                        {
                            picturebox.Left = _pictureboxWidth;
                            picturebox.Top = (index - 1) / 2 * _pictureboxHeight;
                        }
                        this.listBox1.Controls.Add(picturebox);
                    }
                    _changecontrolCnt = newlistboxcontrolCnt - _listboxcontrolCnt; // 最后调整
                }
            }

        }
    }
}
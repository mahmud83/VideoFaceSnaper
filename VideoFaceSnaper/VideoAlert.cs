using MsFaceSDK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Forms;
using VideoFace.Common;
using VideoFaceSnaper.Model;
using System.Media;
using VideoFace.Common.Lib;
using VideoFaceSnaper.Control;

namespace VideoFaceSnaper
{
    public partial class VideoAlert : MetroForm
    {
        private VideoFaceProxy _proxy = new VideoFaceProxy();
        private FaceVerifyTask _verifyTask = new FaceVerifyTask();
        /// <summary>
        /// 报警历史列表
        /// </summary>
        private List<HitAlertInfo> _listAlertHistory = new List<HitAlertInfo>();
        /// <summary>
        /// 报警历史显示的数量
        /// </summary>
        private int _iAlertHistoryNum = 10;

        private int _pictureboxHeight;
        private int _listboxcontrolCnt, _chanegelistboxcontrolCnt;
        private const int _pictureboxWidth = 115;

        public VideoAlert()
        {
            InitializeComponent();

            this.BorderStyle = MetroFormBorderStyle.FixedSingle;
            this.ShadowType = MetroFormShadowType.AeroShadow;
            this.metroStyleManager.Theme = MetroThemeStyle.Dark;
        }

        private void NoticeFrameEvent(FaceDetectInfo detInfo)
        {
            Invoke((MethodInvoker)delegate ()
            {
                Image oldImage = pbVideo.Image;

                if ((detInfo.Rects != null) && (detInfo.Rects.Count > 0))
                {
                    DrawRectangle(detInfo.Img, detInfo.Rects);
                }
                Bitmap newImage = new Bitmap(detInfo.Img, pbVideo.Size);
                pbVideo.Image = newImage;
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
            BeginInvoke((MethodInvoker)delegate ()
            {
                if (videoImg != null && videoImg.Img != null)
                {
                    for (int ie = _listboxcontrolCnt + _chanegelistboxcontrolCnt - 1; ie >= 1; ie--)
                    {
                        UcSnapImage pbctrl = null;
                        UcSnapImage pbnextctrl = null;
                        if (this.plNewSnap.Controls[ie - 1] is UcSnapImage)
                        {
                            pbctrl = (UcSnapImage) this.plNewSnap.Controls[ie -1];
                        }
                        if (this.plNewSnap.Controls[ie] is UcSnapImage)
                        {
                            pbnextctrl = (UcSnapImage) this.plNewSnap.Controls[ie];
                        }
                        
                        if ((pbctrl != null) && (pbnextctrl != null))
                        {
                            if (ie == _listboxcontrolCnt + _chanegelistboxcontrolCnt + 1  && pbnextctrl.pbImage.Image != null)
                            {
                                pbnextctrl.Dispose();
                            }

                            if (pbctrl.pbImage.Image != null)
                            {
                                pbnextctrl.pbImage.Image = pbctrl.pbImage.Image;
                            }
                        }
                    }

                    Bitmap newImage = new Bitmap(videoImg.Img, new Size(_pictureboxWidth, _pictureboxHeight));
                    if (this.plNewSnap.Controls[0] is UcSnapImage)
                    {
                        UcSnapImage firstctrl = (UcSnapImage) this.plNewSnap.Controls[0];
                        if (firstctrl != null)
                        {
                            firstctrl.pbImage.Image = newImage;
                        }
                    }

                    // log 
                    var currentCount = _verifyTask.GetCount();
                    Log4NetHelper.Instance.Info("等待进行比对的数量:" + currentCount.ToString());
                    
                    _verifyTask.AddSnapImage(videoImg);
                }
            });
        }

        private void NoticeAlarmEvent(HitAlertInfo hitalert)
        {
            if (hitalert == null) return;

            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            //双缓冲绘制，避免闪烁
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

            // 报警播报
            var player = new SoundPlayer(Properties.Resources.Beep);
            player.Play();

            this.Invoke((MethodInvoker)delegate
            {
                AddAlarmHistory(hitalert);

                Application.DoEvents();
            });
        }

        private void AddAlarmHistory(HitAlertInfo hitalert)
        {
            // 报警历史
            int iUCWidth = 120;
            int iUCHeight = (int)(4 * iUCWidth / 3);
            int iInterval = 5;
            int iLabelHeight = 15;

            plAlarmHistory.Controls.Clear();
            InitialalarmHistory();

            if (_listAlertHistory == null || _listAlertHistory.Count <= 0)
            {
                UcImage ucSnapImg = new UcImage(hitalert.ImgData, null, null, null);
                ucSnapImg.SetControlWidth(iUCWidth);
                ucSnapImg.Location = new Point(plAlarmHistory.Location.X, iLabelHeight + iInterval);
                ucSnapImg.SearchEvent += UcSnapImg_SearchEvent;
                ucSnapImg.AddModelEvent += UcSnapImg_AddModelEvent;

                UcImage ucStandImg = new UcImage(hitalert.Detail[0].Face, hitalert.Detail[0].PersonId, hitalert.Detail[0].Score.ToString("N2"), hitalert.CreateTime.ToString("yy-MM-dd HH:mm:ss"));
                ucStandImg.SetControlWidth(iUCWidth);
                ucStandImg.pbImage.ContextMenuStrip = null;
                ucStandImg.Location = new Point(ucSnapImg.Right, iLabelHeight + iInterval);

                _listAlertHistory.Add(hitalert);
                plAlarmHistory.Controls.Add(ucSnapImg);
                plAlarmHistory.Controls.Add(ucStandImg);
            }
            else
            {
                int iListNum = _listAlertHistory.Count;
                if (iListNum >= _iAlertHistoryNum)
                {
                    _listAlertHistory.RemoveAt(_iAlertHistoryNum - 1);
                }
                _listAlertHistory.Insert(0, hitalert);
                // 循环临时报警信息集合
                for (int i = 0; i < _listAlertHistory.Count; i++)
                {
                    if (i > 0)
                    {
                        iInterval += iUCHeight + 2;
                    }
                    HitAlertInfo alarmHistory = _listAlertHistory[i];

                    UcImage ucHSnapImg = new UcImage(alarmHistory.ImgData, null, null, null);
                    ucHSnapImg.SetControlWidth(iUCWidth);
                    ucHSnapImg.Location = new Point(plAlarmHistory.Location.X, iLabelHeight + iInterval);
                    ucHSnapImg.SearchEvent += UcSnapImg_SearchEvent;
                    ucHSnapImg.AddModelEvent += UcSnapImg_AddModelEvent;
                    UcImage ucStandImg = new UcImage(alarmHistory.Detail[0].Face, alarmHistory.Detail[0].PersonId, alarmHistory.Detail[0].Score.ToString("N2"), alarmHistory.CreateTime.ToString("yy-MM-dd HH:mm:ss"));
                    ucStandImg.SetControlWidth(iUCWidth);
                    ucStandImg.pbImage.ContextMenuStrip = null;
                    ucStandImg.Location = new Point(ucHSnapImg.Right, iLabelHeight + iInterval);

                    plAlarmHistory.Controls.Add(ucHSnapImg);
                    plAlarmHistory.Controls.Add(ucStandImg);
                }
            }

        }

        private void InitialalarmHistory()
        {
            int iLabelHeight = 15;

            MetroFramework.Controls.MetroLabel lblLeftTitle = new MetroFramework.Controls.MetroLabel();
            lblLeftTitle.Font = new Font("宋体", 8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblLeftTitle.Height = iLabelHeight;
            lblLeftTitle.Text = "现场照";
            lblLeftTitle.StyleManager = metroStyleManager;
            lblLeftTitle.Style = this.Style;
            lblLeftTitle.Location = new Point(plAlarmHistory.Location.X, 0);

            MetroFramework.Controls.MetroLabel lblRightTitle = new MetroFramework.Controls.MetroLabel();
            lblRightTitle.Font = new Font("宋体", 8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRightTitle.Height = iLabelHeight;
            lblRightTitle.Text = "标准照";
            lblRightTitle.StyleManager = metroStyleManager;
            lblRightTitle.Style = this.Style;
            lblRightTitle.Location = new Point(plAlarmHistory.Location.X + plAlarmHistory.Width / 2, 0);

            plAlarmHistory.Controls.Add(lblLeftTitle);
            plAlarmHistory.Controls.Add(lblRightTitle);
        }

        private void InitialSnapControl()
        {
            _listboxcontrolCnt =(int) (Math.Round(this.plNewSnap.Width *1.0D/ _pictureboxWidth));

            this.plNewSnap.Controls.Clear();
            for (int index = 1; index <= _listboxcontrolCnt; index++)
            {
                UcSnapImage picturebox = new UcSnapImage();
                picturebox.Name = "no" + index.ToString();
                picturebox.Width = _pictureboxWidth;
                picturebox.Height = _pictureboxHeight;
                picturebox.SearchEvent += UcSnapImg_SearchEvent;
                picturebox.AddModelEvent += UcSnapImg_AddModelEvent;

                picturebox.Left = (index - 1) * _pictureboxWidth;
                picturebox.Top = 1;
                this.plNewSnap.Controls.Add(picturebox);
            }
        }

        /// <summary>
        /// 双击显示图像详细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Picturebox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (sender is PictureBox)
            {
                PictureBox pb = (PictureBox) sender;
                if (pb != null && pb.Image != null)
                {
                    // Detail
                }
            }
        }

        private void TsbtnClear_Click(object sender, EventArgs e)
        {
            // 清空当前报警列表
            MessageBox.Show("Clear ok!");
        }

        /// <summary>
        /// 人脸搜索
        /// </summary>
        /// <param name="img"></param>
        private void UcSnapImg_SearchEvent(Image img)
        {
            if (img != null)
            {
                byte[] imgdisplay = ImageHelper.BitmapToByteArray(img);

                VideoImgSearch frm = new VideoImgSearch(imgdisplay, string.Empty);
                frm.ShowDialog();
            }
        }

        /// <summary>
        /// 添加人像库
        /// </summary>
        /// <param name="img"></param>
        private void UcSnapImg_AddModelEvent(Image img)
        {
            if (img != null)
            {
                string number = ConvertHelper.DateTimeToStamp().ToString();
                byte[] imgdisplay = ImageHelper.BitmapToByteArray(img);

                PersonManager frm = new PersonManager(imgdisplay, number);
                frm.ShowDialog();
            }
        }

        private void VideoAlert_FormClosing(object sender, FormClosingEventArgs e)
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

            _verifyTask.Stop();
            _verifyTask.NoticeAlertEvent -= NoticeAlarmEvent;

            Log4NetHelper.Instance.Info("客户端应用退出");
        }

        private void VideoAlert_Load(object sender, EventArgs e)
        {
            _pictureboxHeight = this.plNewSnap.Height;

            InitialSnapControl();
            try
            {
                _proxy.Initial();
                _proxy.NoticeFrameEvent += NoticeFrameEvent;
                _proxy.NoticeFaceEvent += NoticeFaceEvent;

                _verifyTask.Start();
                _verifyTask.NoticeAlertEvent += NoticeAlarmEvent;
                Log4NetHelper.Instance.Info("客户端应用启动");
            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.Info("VideoAlert加载摄像机数据出现异常：" + ex.Message);
                MessageBox.Show(ex.Message);
            }
        }

        private void VideoAlert_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal && this.Created)
            {
                if (this.plNewSnap.Controls.Count == _listboxcontrolCnt + _chanegelistboxcontrolCnt)
                {
                    int backchanegelistboxcontrolCnt = _chanegelistboxcontrolCnt;
                    _chanegelistboxcontrolCnt = 0; // 优先调整
                    for (int index = _listboxcontrolCnt + backchanegelistboxcontrolCnt; index >= _listboxcontrolCnt + 1;index--)
                    {
                        string pictureboxKey = "no" + index.ToString();
                        UcSnapImage pbox = (UcSnapImage) this.plNewSnap.Controls[pictureboxKey];
                        if (pbox != null)
                        {
                            if (pbox.pbImage != null)
                            {
                                pbox.pbImage.Dispose();
                            }
                            this.plNewSnap.Controls.RemoveByKey(pictureboxKey);
                        }
                    }
                }
            }
            else if (WindowState == FormWindowState.Maximized && this.Created)
            {
                if (this.plNewSnap.Controls.Count == _listboxcontrolCnt)
                {
                    int newlistboxcontrolCnt = (int) Math.Round(this.plNewSnap.Width *1.0D/_pictureboxWidth);
                    for (int index = _listboxcontrolCnt + 1; index <= newlistboxcontrolCnt; index++)
                    {
                        UcSnapImage picturebox = new UcSnapImage();
                        picturebox.Name = "no" + index.ToString();
                        picturebox.Width = _pictureboxWidth;
                        picturebox.Height = _pictureboxHeight;
                        picturebox.SearchEvent += UcSnapImg_SearchEvent;
                        picturebox.AddModelEvent += UcSnapImg_AddModelEvent;

                        picturebox.Left = (index - 1)*_pictureboxWidth;
                        picturebox.Top = 1;
                        this.plNewSnap.Controls.Add(picturebox);
                    }
                    _chanegelistboxcontrolCnt = newlistboxcontrolCnt - _listboxcontrolCnt; // 最后调整
                }
            }
            
        }
    }
}

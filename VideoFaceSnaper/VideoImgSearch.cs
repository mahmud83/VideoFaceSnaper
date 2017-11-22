using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Manina.Windows.Forms;

using MetroFramework;
using MsFaceSDK;
using VideoFace.Common;
using VideoFaceSnaper.Model;
using VideoFaceSnaper.Control;
using VideoFace.Common.Lib;
using VideoFace.Common.Util;

namespace VideoFaceSnaper
{
    public partial class VideoImgSearch : MetroForm
    {
        private FaceVerifySDK _faceVerify = new FaceVerifySDK();
        private FaceManagerSDK _faceManager = new FaceManagerSDK();
        private double _AlertScore = Convert.ToDouble(ConfigurationHelper.GetValue("AlertScore", "0.7"));
        private SearchCondition m_searchCondition;
        private int m_iSearchCurrPage = 1;
        private int m_iSearchTotalPages = 5;
        private static int m_iPageSize = 12;
        public VideoImgSearch()
        {
            InitializeComponent();

            this.BorderStyle = MetroFormBorderStyle.None;
            this.ShadowType = MetroFormShadowType.AeroShadow;
            this.metroStyleManager.Theme = MetroThemeStyle.Dark;
        }

        public VideoImgSearch(byte[] faceimgdata, string strsnaptime) : this()
        {
            this.pictureBox1.Image = ImageHelper.BytesToBitmap(faceimgdata);
            // 抓拍时间 -暂没使用
        }

        private void lstSearch_ItemDoubleClick(object sender, ItemClickEventArgs e)
        {
            ImageListViewItem selectItem = this.lstSearch.SelectedItems[0];
            if (selectItem == null) return;

            SearcPersonDetail selectObj = (SearcPersonDetail)selectItem.VirtualItemKey;
            if (selectObj != null)
            {
                PictureDisplay frDisplay = new PictureDisplay(new Bitmap(selectObj.Photo), null);
                frDisplay.ShowDialog(this);
            }
        }

        private int SearchImage()
        {
            List<Rectangle> lstRect;
            bool lbcheck = _faceManager.GetFaceRect(m_searchCondition.QueryImage, out lstRect);
            if (!lbcheck)
            {
                return -2;
            }
            if (lstRect == null || lstRect.Count == 0)
            {
                return -3;
            }

            m_searchCondition.Start = (m_iSearchCurrPage - 1) * m_iPageSize;
            m_searchCondition.Limit = m_iPageSize;

            List<HitAlertInfoDetail> searchHitAlertInfo;
            bool lbRet = _faceVerify.CompareByFace(m_searchCondition.QueryImage, out searchHitAlertInfo);
            if (lbRet)
            {
                if (searchHitAlertInfo != null && searchHitAlertInfo.Count > 0)
                {
                    double mustScore = double.Parse(txtScore.Text);
                    List<SearcPersonDetail> lstDetail = new List<SearcPersonDetail>();
                    foreach (var info in searchHitAlertInfo)
                    {
                        if (info.Score >= mustScore)
                        {
                            SearcPersonDetail detail = new SearcPersonDetail(info.Score, info.PersonId);
                            detail.Photo = ImageHelper.ByteArrayToBitmap(info.Face);

                            lstDetail.Add(detail);
                        }
                    }
                    updateSearchUI(lstDetail);
                    return 0;
                }
                else
                {
                    return -4;
                }
            }
            else
            {
                return -1;
            }
        }

        private void updateSearchUI(List<SearcPersonDetail> searchResult)
        {

            if (lstSearch.InvokeRequired)
            {
                lstSearch.BeginInvoke(new MethodInvoker(delegate () { updateSearchUI(searchResult); }));
            }
            else
            {
                this.lstSearch.SuspendLayout();
                this.lstSearch.Items.Clear();
                // Resume layout logic.
                this.lstSearch.ResumeLayout(true);

                if (searchResult != null && searchResult.Count > 0)
                {
                    foreach (var searchDet in searchResult)
                    {
                        this.lstSearch.Items.Add(searchDet, searchDet.Score.ToString() + "-" + searchDet.PersonId, searchDet.Photo);
                    }
                }
                else
                {
                    MessageBox.Show("没有符合结果!","提示");
                }
            }
        }

        private void VideoImgSearch_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("你确定要关闭程序？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) ==
                DialogResult.No)
            {
                e.Cancel = true;
                return;
            }


            Log4NetHelper.Instance.Info("客户端应用退出");
        }

        private void VideoImgSearch_Load(object sender, EventArgs e)
        {
            this.lstSearch.ThumbnailSize = new Size(130, 138);
            this.lstSearch.SetRenderer(new ImageListViewRenderers.XPRenderer());

            this.txtScore.Text = _AlertScore.ToString();
            this.btnSearch.Click += delegate
            {
                if (m_searchCondition != null)
                {
                    m_searchCondition.Dispose();
                }

                Image searchImg = (Image) this.pictureBox1.Image.Clone();
                m_searchCondition = new SearchCondition("FaceTest", searchImg);

                this.lstSearch.SuspendLayout();
                this.lstSearch.Items.Clear();
                // Resume layout logic.
                this.lstSearch.ResumeLayout(true);

                int lret = SearchImage();
                if (lret == -1)
                {
                    MessageBox.Show("人脸图像搜索出现异常!", "提示");
                }
                else if (lret == -2)
                {
                    MessageBox.Show("人脸图像检测出现异常!", "提示");
                }
                else if (lret == -3)
                {
                    MessageBox.Show("人脸图像没有人脸!", "提示");
                }
                else if (lret == -4)
                {
                    MessageBox.Show("没有符合结果!", "提示");
                }
            };

            this.btnSelect.Click += delegate
            {
                string selFile = BaseCommon.SelectFile();
                if (string.IsNullOrEmpty(selFile)) return;

                if (this.pictureBox1.Image != null)
                {
                    this.pictureBox1.Image.Dispose();
                }

                Image img = Image.FromFile(selFile);
                this.pictureBox1.Image = img;
            };

            this.btnAddLibrary.Click += delegate
            {
                string number = ConvertHelper.DateTimeToStamp().ToString();
                byte[] imgdisplay = ImageHelper.BitmapToByteArray(this.pictureBox1.Image);

                PersonManager frm = new PersonManager(imgdisplay, number);
                frm.ShowDialog();

            };
        }

    }
}

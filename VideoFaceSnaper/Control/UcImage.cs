using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MetroFramework;
using MetroFramework.Controls;

namespace VideoFaceSnaper.Control
{

    ///<summary>
    /// UcImage：图像的用户控件
    ///</summary>
    public partial class UcImage : MetroUserControl, IDisposable
    {
       
        /// <summary>
        /// 姓名
        /// </summary>
        private string strName = null;

        /// <summary>
        /// 相似度(分值)
        /// </summary>
        private string strScore = null;

        /// <summary>
        /// 报警时间
        /// </summary>
        private string strAlarmTime = null;

        /// <summary>
        /// 是否成功显示图像
        /// </summary>
        private bool bShowSucessImage = true;

        /// <summary>
        /// 错误信息
        /// </summary>
        private string strErrorMsg = null;

        /// <summary>
        /// 内在流
        /// </summary>
        private MemoryStream ms=null;

        /// <summary>
        /// 文本格式
        /// </summary>
        private readonly Font fontText = new Font("宋体", 7F, FontStyle.Bold, GraphicsUnit.Point, 0);

        /// <summary>
        /// 搜索事件
        /// </summary>
        public Action<Image> SearchEvent;

        /// <summary>
        /// 添加模板事件
        /// </summary>
        public Action<Image> AddModelEvent;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcImage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bImage">相片字节</param>
        /// <param name="strName">姓名</param>
        /// <param name="strScore">分值</param>
        /// <param name="strAlarmTime">报警时间</param>
        public UcImage(byte[] bImage, string strName, string strScore, string strAlarmTime)
        {
            InitializeComponent();

            this.strName = strName;
            this.strScore = strScore;
            this.strAlarmTime = strAlarmTime;
            if (string.IsNullOrEmpty(strAlarmTime))
            {
                lblAlarmMsg.Visible = false;
            }
            else
            {
                lblAlarmMsg.Visible = true;
            }

            if (bImage == null || bImage.Length <= 0)
            {
                strErrorMsg = "人像为空";
                bShowSucessImage = false;
                pbImage.Image = null;
                return;
            }

            try
            {
                ms = new MemoryStream(bImage);
                pbImage.Image =Image.FromStream(ms);
            }
            catch (Exception ex)
            {
                bShowSucessImage = false;
                strErrorMsg = ex.Message;
                pbImage.Image = null;
            }
            finally
            {
                if(ms!=null)
                {
                   ms.Dispose();
                }
            }
            
        }

        /// <summary>
        /// 设置控件宽和高,宽:高=3:4的比例
        /// </summary>
        /// <param name="iWidth">宽</param>
        public void SetControlWidth(int iWidth)
        {
            this.Width = iWidth;
            this.Height = (int)(4 * iWidth / 3);
            StringBuilder sbMsg = new StringBuilder();
            sbMsg.AppendFormat("姓名:{0}\n\r", this.strName);
            sbMsg.AppendFormat("相似度:{0}\n\r", this.strScore);
            sbMsg.AppendFormat("{0}", this.strAlarmTime);
            lblAlarmMsg.Text = sbMsg.ToString();
            int iLocationX = (int)(pbImage.Width / 2 - lblAlarmMsg.Width / 2);
            int iLocationY=pbImage.Height;
            if (bShowSucessImage)
            {
                
                lblAlarmMsg.Font = fontText;
                this.lblAlarmMsg.Location = new Point(iLocationX, iLocationY - 35);
            }
            else
            {
                lblAlarmMsg.Visible = true;
                lblAlarmMsg.Text = strErrorMsg;
                lblAlarmMsg.Font = fontText;
                this.lblAlarmMsg.Location = new Point(pbImage.Location.X+5,pbImage.Location.Y+5);

            }
            //DrawWatermark();
        }

        /// <summary>
        /// 画水印
        /// </summary>
        private void DrawWatermark()
        {
            StringBuilder sbMsg = new StringBuilder();
            sbMsg.AppendFormat("姓名:{0}\n\r",this.strName);
            sbMsg.AppendFormat("相似度:{0}\n\r", this.strScore);
            sbMsg.AppendFormat("{0}", this.strAlarmTime);
            int iLocationX = (int)(pbImage.Width / 2 - lblAlarmMsg.Width / 2);
            int iLocationY = pbImage.Height;
            using (Graphics g = Graphics.FromImage(pbImage.Image))
            {
                g.DrawString(sbMsg.ToString(), fontText, Brushes.Red, iLocationX, iLocationY);
            }
            pbImage.Refresh();
        }

        /// <summary>
        /// 人像搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiSearch_Click(object sender, EventArgs e)
        {
            if (this.SearchEvent != null)
            {
                this.SearchEvent(this.pbImage.Image);
            }
        }

        /// <summary>
        /// 添加模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiAddModel_Click(object sender, EventArgs e)
        {
            if (this.AddModelEvent != null)
            {
                this.AddModelEvent(this.pbImage.Image);
            }
        }

        #region IDisposable 成员

        public void Dispose()
        {
            if (pbImage.Image  != null)
            {
                pbImage.Image.Dispose();
            }
        }

        #endregion
    }
}

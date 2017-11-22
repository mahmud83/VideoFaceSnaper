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
    public partial class UcSnapImage : MetroUserControl, IDisposable
    {
        /// <summary>
        /// 文本格式
        /// </summary>
        private readonly Font fontText = new Font("宋体", 7F, FontStyle.Bold, GraphicsUnit.Point, 0);

        /// <summary>
        /// 添加模板事件
        /// </summary>
        public Action<Image> AddModelEvent;
        /// <summary>
        /// 搜索事件
        /// </summary>
        public Action<Image> SearchEvent;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcSnapImage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bImage">相片图像</param>
        public UcSnapImage(Image bImage)
        {
            InitializeComponent();

            if (bImage == null)
            {
                pbImage.Image = null;
                return;
            }
            pbImage.Image = bImage;
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageSplicer.Common;
using MetroFramework.Forms;

namespace ImageChecker
{
    public partial class Form1 : MetroForm
    {
        ProcessImager2 _processImager = new ProcessImager2();
        private Image _imageOr = null;
        private bool _alive = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _alive = false;
            string strRect = this.textBox1.Text.Trim();
            if (_imageOr == null || string.IsNullOrEmpty(strRect)) return;

            Rectangle cropRect = _processImager.GetRectangle(strRect);
            string errormsg;
            string outputfile =  Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
            string saveFileName = AppDomain.CurrentDomain.BaseDirectory + outputfile;
            if (cropRect.Width > 0 && cropRect.Height > 0)
            {
                // 根据rect进行截图处理,改为当前图像的1/2区域
                var changeWidth = (int)((_imageOr.Width / 2F - cropRect.Width) / 2F);
                var changeHeight = (int)((_imageOr.Height / 2F - cropRect.Height) / 2F);
                int x1 = 0, y1 = 0;
                if (cropRect.X > changeWidth)
                {
                    x1 = cropRect.X - changeWidth;
                }
                if (cropRect.Y > changeHeight)
                {
                    y1 = cropRect.Y - changeHeight;
                }

                // 判断是否越界，进行边界分析
                if (x1 > (int)(_imageOr.Width / 2F))
                {
                    x1 = (int)(_imageOr.Width / 2F);
                }
                if (y1 > (int)(_imageOr.Height / 2F))
                {
                    y1 = (int)(_imageOr.Height / 2F);
                }

                Rectangle newcropRect = new Rectangle(x1, y1, (int)(_imageOr.Width / 2F), (int)(_imageOr.Height / 2F));
                bool lbCrop = _processImager.CmdImage(new Bitmap(_imageOr), newcropRect, saveFileName, out errormsg, 1);
                if (lbCrop)
                {
                    Log4NetHelper.Instance.Info("图像存储文件：" + saveFileName);
                    System.Diagnostics.Process.Start("mspaint.exe", saveFileName);
                }
                else
                {
                    MessageBox.Show("图像截取出现错误:"  + errormsg, "提示");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 将图片显示到控件
            string filepath = BaseCommon.SelectFile();
            if (String.IsNullOrEmpty(filepath)) return;

            if (_imageOr != null)
            {
                _imageOr.Dispose();
            }
            _imageOr = new Bitmap(filepath);
            pictureBox1.Image = new Bitmap(_imageOr, pictureBox1.Width, pictureBox1.Height);
        }

        public void DataSenderTask()
        {
            Thread.Sleep(500);
            while (_alive)
            {
                Log4NetHelper.Instance.Info("正常运行!");
                Thread.Sleep(1000);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*
            _alive = true;
            Task[] tasksDeal = new Task[5];
            for (int counter = 0; counter < 5; counter++)
            {
                tasksDeal[counter] = new Task(DataSenderTask, TaskCreationOptions.LongRunning);
            }

            foreach (Task t in tasksDeal)
            {
                t.Start();
            }
             */
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ImageSplicer.Common
{
    public class DisplayPicture
    {
        public string FileName;
        public double Simularity;
    }

    public static class BaseCommon
    {
        public static string TempPictureDir = System.Environment.CurrentDirectory + "\\temp\\";
        public static string FeaturePictureDir = System.Environment.CurrentDirectory + "\\picture\\";
        public static string FeatureDataDir = System.Environment.CurrentDirectory + "\\data\\";
        public static string HistoryDir = System.Environment.CurrentDirectory + "\\history\\";

        /// <summary>  
        ///C#计算时间间隔  
        /// </summary>  
        /// <param name="dateTime1">起始日期和时间</param>  
        /// <returns></returns>  
        public static string DateDiff(DateTime dateTime1)
        {
            string dateDiff = null;
            TimeSpan ts1 = new TimeSpan(dateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();

            dateDiff = ts.Hours.ToString() + "小时"
            + ts.Minutes.ToString() + "分"
            + ts.Seconds.ToString() + "秒"
            + ts.Milliseconds.ToString() + "毫秒";

            return dateDiff;
        }

        public static double Max(double x, double y)
        {
            return (x > y) ? x : y;
        }

        public static string SelectFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择文件";
            openFileDialog.Filter = "jpg文件|*.jpg|png文件|*.png|bmp文件|*.bmp|所有文件|*.*";
            openFileDialog.FileName = string.Empty;
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.DefaultExt = "jpg";
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return null;
            }
            return openFileDialog.FileName;
        }

        public static string SelectDirectory()
        {
            FolderBrowserDialog df = new FolderBrowserDialog();

            //设置文件浏览对话框上的描述内容   
            df.Description = "选择人脸图像文件所在目录";
            //不显示对话框下方的创建新文件夹按钮   
            df.ShowNewFolderButton = false;

            df.RootFolder = Environment.SpecialFolder.MyComputer;

            //显示文件夹对话框，并返回对话框处理结果数值   
            DialogResult result = df.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderPath = df.SelectedPath;
                return folderPath;
            }
            return null;
        }

    }
}

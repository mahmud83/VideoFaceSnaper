using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace VideoFace.Common.Lib
{
    public class DisplayPicture
    {
        public string FileName;
        public double Simularity;
    }

    public static class BaseCommon
    {
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

        ///<summary>
        /// 标准人像库ID
        ///</summary>
        public static int PersonRepositoryId = 1;

        ///<summary>
        /// 默认抓拍图像库ID
        ///</summary>
        public static int SnapRepositoryId = 2;

        ///<summary>
        /// 默认抓拍摄像机编号
        ///</summary>
        public static string SnapSourceId = "1001";

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

        public static List<string> GetDirectory(string imageDir)
        {
            var files = Directory.GetFiles(imageDir, "*.*", SearchOption.AllDirectories)
                    .Where(s => s.EndsWith(".jpg") || s.EndsWith(".png") || s.EndsWith(".bmp"));
            int fileProcessAll = files.Count();

            List<string> lstRes = new List<string>();
            foreach (var filepath in files)
            {
                lstRes.Add(filepath);
            }

            return lstRes;
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

        public static byte[] StructToBytes(object structObj)
        {
            int size = Marshal.SizeOf(structObj);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(structObj, buffer, false);
                byte[] bytes = new byte[size];
                Marshal.Copy(buffer, bytes, 0, size);
                return bytes;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        public static object BytesToStruct(byte[] bytes, Type strcutType)
        {
            int size = Marshal.SizeOf(strcutType);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, buffer, size);
                return Marshal.PtrToStructure(buffer, strcutType);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

    }
}

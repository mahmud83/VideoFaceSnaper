using System;
using System.Configuration;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using ImageSplicer.Common;
using ImageSplicer.Common.Util;
using ImageSplicer.Data;
using log4net.Appender;
using System.IO;

namespace ImageSplicer
{
    /// <summary>
    ///     通知分析图像
    /// </summary>
    public class NotifyAnalyImager
    {
        private bool _alive = false;
        ProcessImager _processImager = new ProcessImager();
        private int _imageSpliceSort = int.Parse(ConfigurationHelper.GetValue("ImageSpliceSort", "0"));
        private int _QueueTaskCount = int.Parse(ConfigurationHelper.GetValue("QueueTaskCount", "5"));
        private static readonly string _RecordFilePath = AppDomain.CurrentDomain.BaseDirectory + "VEHICLEDATA\\";
        private static string _RecordFileUrl = ConfigurationHelper.GetValue("RecordFileUrl", "http://100.11.41.222:80/");

        /// <summary>
        ///     与程序启动一起开始
        /// </summary>
        public void StartListen()
        {
            if (_alive) return;

            _alive = true;
            Log4NetHelper.Instance.Info("接收结果线程开启");

            Task[] tasksDeal = new Task[_QueueTaskCount];
            for (int counter = 0; counter < _QueueTaskCount; counter++)
            {
                tasksDeal[counter] = new Task(DataSenderTask, TaskCreationOptions.LongRunning);
            }

            foreach (Task t in tasksDeal)
            {
                t.Start();
            }
        }

        /// <summary>
        ///     在程序结束时停止
        /// </summary>
        public void StopListen()
        {
            Log4NetHelper.Instance.Info("接收结果线程结束");
            _alive = false;
        }

        public void DataSenderTask()
        {
            Thread.Sleep(500);
            while (_alive)
            {
                try
                {
                    Img4kafka resultkafka = ResultImageQueue.GetFromQueue();
                    if (resultkafka == null)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }
                    else
                    {
                        if (resultkafka.Picturepath != null && !string.IsNullOrEmpty(resultkafka.Tx1) && !string.IsNullOrEmpty(resultkafka.Rect))
                        {
                            string outputstr = "从队列接收数据-Tx1:" + resultkafka.Tx1 + ", Rect:"+ resultkafka.Rect;
                            if (resultkafka.Picturepath.Length == 1)
                            {
                                outputstr += ", Picturepath0:" + resultkafka.Picturepath[0];
                            }
                            else if (resultkafka.Picturepath.Length == 2)
                            {
                                outputstr += ", Picturepath0:" + resultkafka.Picturepath[0] + ", Picturepath1:" + resultkafka.Picturepath[1];
                            }
                            Log4NetHelper.Instance.Debug(outputstr);

                            Bitmap bitmap = null;
                            string errormsg = null;
                            bool lbRet = _processImager.GetImageByUrl(resultkafka.Tx1, out bitmap, out errormsg);
                            if (!lbRet)
                            {
                                Log4NetHelper.Instance.Info("图像数据不完整：" + resultkafka.Tx1 + ","+ errormsg);
                                continue;
                            }

                            // 图片合成存储路径
                            string outputfile =  Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
                            string filedir = _RecordFilePath + DateTime.Today.ToString("yyyy-M-d") + "\\";
                            if (!Directory.Exists(filedir))
                            {
                                Directory.CreateDirectory(filedir);
                            }

                            Rectangle cropRect = _processImager.GetRectangle(resultkafka.Rect);
                            if (cropRect.Width > 0 && cropRect.Height > 0)
                            {
                                // 根据rect进行截图处理,改为当前图像的1/2区域
                                var changeWidth = (int)((bitmap.Width / 2F - cropRect.Width) / 2F);
                                var changeHeight = (int)((bitmap.Height / 2F - cropRect.Height) / 2F);
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
                                if (x1 > (int)(bitmap.Width / 2F))
                                {
                                    x1 = (int)(bitmap.Width / 2F);
                                }
                                if (y1 > (int)(bitmap.Height / 2F))
                                {
                                    y1 = (int)(bitmap.Height / 2F);
                                }

                                Rectangle newcropRect = new Rectangle(x1, y1,(int)(bitmap.Width / 2F), (int)(bitmap.Height / 2F));
                                bool lbCrop = _processImager.CmdImage(bitmap, newcropRect, filedir + outputfile, out errormsg, _imageSpliceSort);
                                if (lbCrop)
                                {
                                    Log4NetHelper.Instance.Info("图像存储文件：" + outputfile);

                                    // 将数据进行组合后重新发送
                                    string outputPath =GetResultImageServerPath(filedir + outputfile);
                                    resultkafka.Tx1 = outputPath;
                                    if (resultkafka.PictureHttpPath != null && resultkafka.PictureHttpPath.Length > 0)
                                    {
                                        resultkafka.PictureHttpPath[0] = outputPath;
                                    }

                                    string resultjson = Newtonsoft.Json.JsonConvert.SerializeObject(resultkafka);
                                    ResultSpliceQueue.AddToQueue(resultjson);
                                }
                                else
                                {
                                    Log4NetHelper.Instance.Error("图像截取出现错误:"+ resultkafka.Tx1 + ","+ errormsg);
                                }
                            }
                            bitmap.Dispose();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4NetHelper.Instance.Error("从队列接收数据错误:" + (ex.InnerException != null? ex.InnerException.Message : ex.Message));
                }
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        ///     获取对外的http或ftp文件路径
        /// </summary>
        /// <param name="localPath"></param>
        /// <returns></returns>
        public static string GetResultImageServerPath(string localPath)
        {
            if (string.IsNullOrEmpty(localPath)) return null;

            // 保护异常的参数传递
            if (localPath.StartsWith("http") || localPath.StartsWith("ftp"))
            {
                return localPath;
            }

            if (!string.IsNullOrEmpty(_RecordFilePath))
            {
                localPath = localPath.Replace(_RecordFilePath, _RecordFileUrl);
                localPath = localPath.Replace("\\", "/");

                // 暂时不用
                //var sUrlPath = HttpUtility.UrlEncode(localPath);
                return localPath;
            }
            return null;
        }
    }
}
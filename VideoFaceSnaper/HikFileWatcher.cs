using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VideoFace.Common;
using VideoFace.Common.Lib;
using VideoFaceSnaper.Model;

namespace VideoFaceSnaper
{
    /// <summary>
    /// 海康人脸抓拍摄像机数据监控
    /// </summary>
    public class HikFileWatcher
    {
        private static FileSystemWatcher m_Watcher;
        private static bool m_bIsWatching;
        public Action<FileFaceImage> ReceiveFaceEvent;
        private CancellationTokenSource _tokenDeleteOld = new CancellationTokenSource();
        private Task _taskDeleteOld = null;

        public HikFileWatcher()
        {
        }

        public void StartWatch(string directoryPath)
        {
            if (m_bIsWatching) return;

            StartFileTask();

            m_bIsWatching = true;
            m_Watcher = new System.IO.FileSystemWatcher();
            m_Watcher.Filter = "*.*";
            m_Watcher.IncludeSubdirectories = true;
            if (!directoryPath.EndsWith("\\"))
            {
                m_Watcher.Path = directoryPath + "\\";
            }
            else
            {
                m_Watcher.Path = directoryPath;
            }

            m_Watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size
                                    | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            m_Watcher.Changed += new FileSystemEventHandler(WatcherOnChanged);
            m_Watcher.Created += new FileSystemEventHandler(WatcherOnChanged);
            m_Watcher.Deleted += new FileSystemEventHandler(WatcherOnChanged);
            m_Watcher.Renamed += new RenamedEventHandler(WatcherOnRenamed);
            m_Watcher.EnableRaisingEvents = true;


            _taskDeleteOld = Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        // 取消任务判断
                        if (_tokenDeleteOld.IsCancellationRequested)
                        {
                            break;
                        }

                        // select all file
                        List<string> waitfordel = new List<string>();
                        List<string>  allfiles = BaseCommon.GetDirectory(directoryPath);
                        if (allfiles != null && allfiles.Count > 0)
                        {
                            foreach (string sFile in allfiles)
                            {
                                if (File.Exists(sFile))
                                {
                                    FileInfo fi = new FileInfo(sFile);
                                    if (fi.CreationTime < DateTime.Today.AddDays(-2))
                                    {
                                        waitfordel.Add(sFile);
                                    }
                                }
                            }

                            // delete file
                            if (waitfordel != null && waitfordel.Count > 0)
                            {
                                foreach (string delfile in waitfordel)
                                {
                                    File.Delete(delfile);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log4NetHelper.Instance.Error("删除FTP目录旧的文件数据错误:" + (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
                    }

                    Thread.Sleep(2* 60 * 60* 1000);
                }

            }, _tokenDeleteOld.Token);
        }

        public void StopWatch()
        {
            if (m_bIsWatching)
            {
                m_bIsWatching = false;
                m_Watcher.EnableRaisingEvents = false;
                m_Watcher.Dispose();

                StopFileTask();

                try
                {
                    _tokenDeleteOld.Cancel();

                    Console.WriteLine("TaskDeleteFile Exit...");
                    _taskDeleteOld.Wait();
                }
                catch (AggregateException e)
                {
                    Console.WriteLine("Exception messages:");
                    foreach (var ie in e.InnerExceptions)
                        Console.WriteLine("   {0}: {1}", ie.GetType().Name, ie.Message);

                    Console.WriteLine("\nTaskDeleteFile status: {0}", _taskDeleteOld.Status);
                }
                finally
                {
                    _tokenDeleteOld.Dispose();
                }
            }
        }

        private void WatcherOnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                // 文件格式范例 172.31.108.248_01_20170905143220480_FACE_ALARM.jpg
                string[] vals = e.Name.Split('_');
                if (vals.Length >= 3)
                {
                    if (e.FullPath.EndsWith(".jpg") || e.FullPath.EndsWith(".bmp") ||
                        e.FullPath.EndsWith(".png"))
                    {
                        string cameraIp = vals[0];
                        if (cameraIp.Contains("\\"))
                        {
                            cameraIp = cameraIp.Substring(0, cameraIp.IndexOf("\\"));
                        }
                        string fileTime = DateTime.Now.ToString("yyyyMMddHHmmss");

                        Thread.Sleep(50);
                        if (e.FullPath.ToLower().Contains("face"))
                        {
                            var fileFace = new FileFaceImage(fileTime, DateTime.Now, cameraIp, e.FullPath);
                            Log4NetHelper.Instance.Info("抓拍人脸图像文件:" + cameraIp + "," + Path.GetFileName(e.FullPath));
                            ResultFileQueue.AddToQueue(fileFace);
                        }
                        else
                        {
                            Log4NetHelper.Instance.Debug("抓拍人脸图像文件忽略:" + e.FullPath);
                        }
                    }
                }
            }
        }

        private void WatcherOnRenamed(object sender, RenamedEventArgs e)
        {
            string old = e.OldFullPath;
            old += e.ChangeType.ToString();
            old +=" to " + e.Name;
            old += DateTime.Now.ToString();
        }

        private CancellationTokenSource _tokenFileWatch = new CancellationTokenSource();
        private static Task _taskFile = null;

        private void StartFileTask()
        {
            if (_taskFile != null) return;

            _taskFile = Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        // 取消任务判断
                        if (_tokenFileWatch.IsCancellationRequested)
                        {
                            break;
                        }

                        FileFaceImage message = ResultFileQueue.GetFromQueue();
                        if (message == null)
                        {
                            Thread.Sleep(1000);
                            continue;
                        }
                        else
                        {
                            if (ReceiveFaceEvent != null)
                            {
                                // 该处理确保图像文件已经完成，可以进行读取操作
                                Thread.Sleep(500);
                                ReceiveFaceEvent(message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log4NetHelper.Instance.Error("从队列发送Kafka数据错误:" + (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
                    }
                    Thread.Sleep(1000);
                }

            }, _tokenFileWatch.Token);

        }

        private void StopFileTask()
        {
            try
            {
                _tokenFileWatch.Cancel();

                Console.WriteLine("ImgFileTask Exit...");
                _taskFile.Wait();
            }
            catch (AggregateException e)
            {
                Console.WriteLine("Exception messages:");
                foreach (var ie in e.InnerExceptions)
                    Console.WriteLine("   {0}: {1}", ie.GetType().Name, ie.Message);

                Console.WriteLine("\nImgFileTask status: {0}", _taskFile.Status);
            }
            finally
            {
                _tokenFileWatch.Dispose();
            }
        }
    }
}

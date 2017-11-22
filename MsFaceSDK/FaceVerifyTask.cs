using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VideoFace.Common;
using VideoFace.Common.Cache;
using VideoFace.Common.Data;
using VideoFace.Common.Lib;
using VideoFace.Common.Util;

namespace MsFaceSDK
{
    /// <summary>
    /// 人脸抓拍图像分析任务
    /// </summary>
    public class FaceVerifyTask
    {
        private bool _alive = false;
        private int _QueueTaskCount = Convert.ToInt32(ConfigurationHelper.GetValue("QueueTaskCount", "5"));
        private int _FaceMinPeriod = Convert.ToInt32(ConfigurationHelper.GetValue("FaceMinPeriod", "5"));
        private double _AlertScore = Convert.ToDouble(ConfigurationHelper.GetValue("AlertScore", "0.7"));
        private FaceVerifySDK _faceVerify = new FaceVerifySDK();
        private readonly static CachingService _cache = new CachingService();
        public Action<HitAlertInfo> NoticeAlertEvent;

        /// <summary>
        ///     与程序启动一起
        /// </summary>
        public void Start()
        {
            if (_alive) return;

            _alive = true;
            Log4NetHelper.Instance.Info("布控任务处理线程开启");

            _cache.DefaultCacheDuration = _FaceMinPeriod * 2; // default 10 seconds
            Task[] tasksDeal = new Task[_QueueTaskCount];
            for (int counter = 0; counter < _QueueTaskCount; counter++)
            {
                tasksDeal[counter] = new Task(DataSenderTask, TaskCreationOptions.LongRunning);
            }

            foreach (Task t in tasksDeal)
            {
                t.Start();
            }

            if (_AlertScore > 0.9 || _AlertScore < 0.4)
            {
                _AlertScore = 0.7;
            }
        }

        /// <summary>
        ///     在程序结束时停止
        /// </summary>
        public void Stop()
        {
            Log4NetHelper.Instance.Info("布控任务处理线程结束");
            _alive = false;
        }

        /// <summary>
        ///     将抓拍图像提交分析
        /// </summary>
        /// <param name="data"></param>
        public void AddSnapImage(SnapVideoImage data)
        {
            if (_alive)
            {
                if (data != null && data.Img != null)
                {
                    SnapImageQueue.AddToQueue(data);
                }
            }
            else
            {
                if (data != null && data.Img != null)
                {
                    data.Img.Dispose();
                    data = null;
                }
            }
        }

        /// <summary>
        ///     分析抓拍任务数量
        /// </summary>
        /// <param name="data"></param>
        public bool IsFull(int data)
        {
            return SnapImageQueue.IsFull(data);
        }

        public int GetCount()
        {
            return SnapImageQueue.GetCount();
        }

        private void DataSenderTask()
        {
            Thread.Sleep(500);
            while (_alive)
            {
                try
                {
                    SnapVideoImage snapimg = SnapImageQueue.GetFromQueue();
                    if (snapimg == null)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }
                    else
                    {
                        if (snapimg.Img == null)
                            continue;

                        List<HitAlertInfoDetail> compresult;
                        byte[] bytSrc = ImageHelper.ImageToBytes(snapimg.Img);

                        bool lbRet = _faceVerify.CompareByFace(bytSrc, out compresult);
                        if (lbRet && (compresult!= null)&& (compresult.Count > 0) && (NoticeAlertEvent != null))
                        {
                            byte[] srcImgData = ObjectCopier.Clone(bytSrc);

                            HitAlertInfo alert = new HitAlertInfo(snapimg.SourceId, snapimg.CreateTime, srcImgData);
                            alert.Detail = new List<HitAlertInfoDetail>();
                            foreach (HitAlertInfoDetail detail in compresult)
                            {
                                if (detail.Score >= _AlertScore)
                                {
                                    alert.Detail.Add(detail);
                                }
                            }

                            // 存在符合分值的数据
                            if (alert.Detail.Count > 0)
                            {
                                // 判断是否报警
                                bool lbCheck = CheckExistAlarm(snapimg.CreateTime, alert.Detail[0].PersonId);
                                if (!lbCheck)
                                {
                                    long timestamp = ConvertHelper.DateTimeToStamp();
                                    HitPersonInfo hitpersoninfo = new HitPersonInfo(snapimg.CreateTime, alert.Detail[0].PersonId);

                                    _cache.Add<HitPersonInfo>(timestamp.ToString(), hitpersoninfo);

                                    NoticeAlertEvent(alert);
                                }
                                else
                                {
                                    alert = null;
                                }
                            }
                            else
                            {
                                alert = null;
                            }
                        }

                        // 销毁旧对象
                        if (snapimg !=null && snapimg.Img != null)
                        {
                            snapimg.Dispose();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4NetHelper.Instance.Error("从队列接收数据错误:" + (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
                }
                Thread.Sleep(1000);
            }
        }

        private bool CheckExistAlarm(DateTime currentTime, string personId)
        {
            IList<HitPersonInfo> lstcache = _cache.GetAll<HitPersonInfo>();
            if (lstcache != null && lstcache.Count > 0)
            {
                foreach (HitPersonInfo info in lstcache)
                {
                    // 如果周期内出现，则默认不出现
                    if (info.HitTime.AddSeconds(_FaceMinPeriod) >= currentTime && info.PersonId == personId)
                    {
                        Log4NetHelper.Instance.Info("人脸出现时间:" + currentTime.ToString() + ","+ personId + " 重复出现，将过滤");
                        return true;
                    }
                }
            }
            return false;
        }
    }
}

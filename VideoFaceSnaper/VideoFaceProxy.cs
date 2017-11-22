using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsFaceSDK;
using VideoFace.Common;
using VideoFace.Common.Data;
using VideoFace.Common.Util;
using VideoFaceSnaper.Model;
using System.IO;
using VideoFace.Common.Lib;
using VideoFaceSnaper.Data;
using System.Xml.Serialization;
using VideoFaceSnaper.Common.Lib;

namespace VideoFaceSnaper
{
    public class VideoFaceProxy
    {
        private static VideoFace.CoreNetApi.CoreNetApi _api = new VideoFace.CoreNetApi.CoreNetApi();
        private double _traceMinArea = Convert.ToDouble(ConfigurationHelper.GetValue("TraceMinArea", "0.05"));
        private double _traceMaxArea = Convert.ToDouble(ConfigurationHelper.GetValue("TraceMaxArea", "0.5"));
        private double _traceFaceScore = Convert.ToDouble(ConfigurationHelper.GetValue("TraceFaceScore", "0.2"));

        private static string _videoSrcUrl = ConfigurationHelper.GetValue("VideoSource", "");
        private static readonly string _faceOutDir = ConfigurationHelper.GetValue("FaceOutDir", "");

        private static string _faceFilePath = AppDomain.CurrentDomain.BaseDirectory + "FACEDATA\\";
        private static string _faceFileUrl = ConfigurationHelper.GetValue("FaceFileUrl", "http://127.0.0.1:80/");
        private ImgkafkaActor _imgkafkaActor = new ImgkafkaActor();

        private static HikFileWatcher _watcher = null;
        private static HikFilePlayer _hikPlayer = new HikFilePlayer();
        public Action<FaceDetectInfo> NoticeFrameEvent;
        public Action<SnapVideoImage> NoticeFaceEvent;

        public void Initial()
        {
            // 判断是rtsp或文件，则启动opencv，如果是文件目录则跟踪文件目录变动
            if (string.IsNullOrEmpty(_faceOutDir))
            {
                _api.InitFaceDll();

                _api.FaceEvent += api_FaceEvent;
                _api.FrameEvent += api_FrameEvent;
                if (_videoSrcUrl.StartsWith("http://"))
                {
                    // 播放http录像
                }
                else
                {
                    string strdir = Path.GetDirectoryName(_videoSrcUrl);
                    if (string.IsNullOrEmpty(strdir))
                    {
                        strdir = AppDomain.CurrentDomain.BaseDirectory + "测试录像\\";
                        _videoSrcUrl = strdir + _videoSrcUrl;
                    }
                }
                int result = _api.StartPlay(_videoSrcUrl, _traceMinArea, _traceMaxArea);
            }
            else
            {
                //_detectAlarm = new FaceDetectAlarm();
                ////1、初始化
                //_detectAlarm.Initial(_faceFilePath, _traceFaceScore);
                //List<CameraInfo> lstCamera = DeSerializeConfig();
                //if (lstCamera == null || lstCamera.Count == 0)
                //{
                //    Log4NetHelper.Instance.Error("摄像机配置数据为空,检查配置文件[CameraList.xml]");
                //    return;
                //}

                ////2、登录摄像机
                //foreach (CameraInfo camera in lstCamera)
                //{
                //    int irc = _detectAlarm.LoginCamera(camera);
                //    if (irc <= 0)
                //    {
                //        Log4NetHelper.Instance.Error("登录摄像机错误:" + camera.CameraName + ","+ camera.CameraIp);
                //    }
                //}

                ////3、启动布防
                //_detectAlarm.StartAlarm();
                //_detectAlarm.NoticeFaceSnapEvent += hikvison_FaceEvent;
                ////4、启动监听
                //_detectAlarm.StartListen(PortHelper.GetFirstAvailablePort());

                _watcher = new HikFileWatcher();
                _watcher.ReceiveFaceEvent += watcher_FaceEvent;
                _watcher.StartWatch(_faceOutDir);
              
                _hikPlayer.ReceiveFrameEvent += hikPlayer_FrameEventt;
                _hikPlayer.Start(_videoSrcUrl);
            }

            _imgkafkaActor.Initial();
        }

        public void UnLoad()
        {
            if (string.IsNullOrEmpty(_faceOutDir))
            {
                _api.EndPlay();
                _api.FaceEvent -= api_FaceEvent;
                _api.FrameEvent -= api_FrameEvent;
            }
            else
            {
                _hikPlayer.Stop();
                _hikPlayer.ReceiveFrameEvent -= hikPlayer_FrameEventt;

                // 暂没使用
                //if (_detectAlarm != null)
                //{
                //    _detectAlarm.NoticeFaceSnapEvent -= hikvison_FaceEvent;
                //    _detectAlarm.UnInitial();
                //}

                if (_watcher != null)
                {
                    _watcher.StopWatch();
                    _watcher.ReceiveFaceEvent -= watcher_FaceEvent;
                }
            }

            _imgkafkaActor.Unload();
        }

        private void api_FrameEvent(int frameId, Bitmap image, System.Drawing.Rectangle[] facerectArray)
        {
            if (image == null) return;

            if (NoticeFrameEvent != null)
            {
                List<Rectangle> rects = null;
                if ((facerectArray != null) && (facerectArray.Length > 0))
                {
                    rects = new List<Rectangle>(facerectArray);
                }
                FaceDetectInfo info = new FaceDetectInfo(image, frameId, rects);
                NoticeFrameEvent(info);
            }
            else
            {
                image.Dispose();
            }
        }

        private void api_FaceEvent(int frameId, Bitmap image, int faceId, int faceSerial, double score)
        {
            if (image == null) return;

            if (score > _traceFaceScore && faceSerial == 3)
            {
                ///////////////////////////////////
                // 增加发送到Kafka
                try
                {
                    string outputfile = score.ToString() + "_" + faceId.ToString() + ".jpg";

                    string filedir = _faceFilePath + DateTime.Today.ToString("yyyy-M-d") + "\\";
                    if (!Directory.Exists(filedir))
                    {
                        Directory.CreateDirectory(filedir);
                    }
                    image.Save(filedir + outputfile, ImageFormat.Jpeg);

                    Log4NetHelper.Instance.Info("图像存储文件：" + outputfile);
                    // 将数据进行组合后重新发送
                    string serveroutputPath = GetResultImageServerPath(filedir + outputfile);

                    var resultkafka = new FaceImg4kafka();
                    resultkafka.ImgNum = frameId.ToString();
                    resultkafka.PassTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    resultkafka.CameraIp = "127.0.0.1";
                    resultkafka.FaceUrl = serveroutputPath;

                    string resultjson = Newtonsoft.Json.JsonConvert.SerializeObject(resultkafka);
                    _imgkafkaActor.Send2Quere(resultjson);
                }
                catch (Exception ex)
                {
                    Log4NetHelper.Instance.Error("FaceEvent生成Kakfa数据出现错误：" + ex.Message);
                }
                ///////////////////////////////////

                if (NoticeFaceEvent != null)
                {
                    Image faceImg = ObjectCopier.Clone(image);
                    SnapVideoImage videoImg = new SnapVideoImage(_videoSrcUrl, frameId.ToString(), faceId, DateTime.Now, faceImg);
                    NoticeFaceEvent(videoImg);
                }
                image.Dispose();
            }
            else
            {
                image.Dispose();
            }
        }

        private void watcher_FaceEvent(FileFaceImage faceimage)
        {
            if (faceimage == null || string.IsNullOrEmpty(faceimage.ImgFilePath)) return;

            ///////////////////////////////////
            // 增加发送到Kafka
            try
            {
                string outputfile = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";

                string filedir = _faceFilePath + DateTime.Today.ToString("yyyy-M-d") +"\\";
                if (!Directory.Exists(filedir))
                {
                    Directory.CreateDirectory(filedir);
                }
                File.Copy(faceimage.ImgFilePath, filedir + outputfile);

                Log4NetHelper.Instance.Info("图像存储文件：" + outputfile);
                // 将数据进行组合后重新发送
                string serveroutputPath = GetResultImageServerPath(filedir + outputfile);

                var resultkafka = new FaceImg4kafka();
                resultkafka.ImgNum = ConvertHelper.DateTimeToStamp().ToString();
                resultkafka.PassTime = faceimage.TimeStamp;
                resultkafka.CameraIp = faceimage.CameraIp;
                resultkafka.FaceUrl = serveroutputPath;

                string resultjson = Newtonsoft.Json.JsonConvert.SerializeObject(resultkafka);
                _imgkafkaActor.Send2Quere(resultjson);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.Error("FaceEvent生成Kakfa数据出现错误：" + ex.Message);
            }
            ///////////////////////////////////

            if (NoticeFaceEvent != null)
            {
                Image faceImg = new Bitmap(faceimage.ImgFilePath);
                string frameid = ObjectCopier.Clone(faceimage.TimeStamp);
                DateTime createDate = faceimage.CreateTime;
                SnapVideoImage videoImg = new SnapVideoImage(_videoSrcUrl, frameid, 1, createDate, faceImg);
                NoticeFaceEvent(videoImg);
            }
        }

        private void hikvison_FaceEvent(string strSnapTime, string strDevIP, string strFilePath, Rectangle rectFace)
        {
            if (string.IsNullOrEmpty(strFilePath)) return;

            ///////////////////////////////////
            // 增加发送到Kafka
            try
            {
                // 将数据进行组合后重新发送
                string serveroutputPath = GetResultImageServerPath(strFilePath);

                var resultkafka = new FaceImg4kafka();
                resultkafka.ImgNum = ConvertHelper.DateTimeToStamp().ToString();
                resultkafka.PassTime = strSnapTime;
                resultkafka.CameraIp = strDevIP;
                resultkafka.FaceUrl = serveroutputPath;

                string resultjson = Newtonsoft.Json.JsonConvert.SerializeObject(resultkafka);
                _imgkafkaActor.Send2Quere(resultjson);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.Error("FaceEvent生成Kakfa数据出现错误：" + ex.Message);
            }
            ///////////////////////////////////

            if (NoticeFaceEvent != null)
            {
                Image faceImg = new Bitmap(strFilePath);
                string frameid = Path.GetFileNameWithoutExtension(strFilePath);
                DateTime createDate = Convert.ToDateTime(strSnapTime);
                SnapVideoImage videoImg = new SnapVideoImage(_videoSrcUrl, frameid, 1, createDate, faceImg);
                NoticeFaceEvent(videoImg);
            }
        }

        private void hikPlayer_FrameEventt(FaceDetectInfo faceimage)
        {
            if (faceimage == null || faceimage.Img == null) return;

            if (NoticeFrameEvent != null)
            {
                NoticeFrameEvent(faceimage);
            }
            else
            {
                faceimage.Dispose();
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

            if (!string.IsNullOrEmpty(_faceFilePath))
            {
                localPath = localPath.Replace(_faceFilePath, _faceFileUrl);
                localPath = localPath.Replace("\\", "/");

                // 暂时不用
                //var sUrlPath = HttpUtility.UrlEncode(localPath);
                return localPath;
            }
            return null;
        }

        /*
        /// <summary>
        /// 加载本地配置文件数据
        /// </summary>
        /// <returns></returns>
        private static List<CameraInfo> DeSerializeConfig()
        {
            if (false == File.Exists(AppDomain.CurrentDomain.BaseDirectory + "CameraList.xml")) return null;

            List<CameraInfo> lstCamera = new List<CameraInfo>();
            XmlSerializer deserializer = new XmlSerializer(typeof(List<CameraInfo>));
            using (TextReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "CameraList.xml"))
            {
                object obj = deserializer.Deserialize(reader);
                List<CameraInfo> xmlData = (List<CameraInfo>)obj;
                reader.Close();

                // 根据CameraIp来确定唯一性
                foreach (var camera in xmlData)
                {
                    bool lbExist = lstCamera.Exists(x => x.CameraIp == camera.CameraIp);
                    if (lbExist == false)
                    {
                        lstCamera.Add(camera);
                    }
                }
            }

            return lstCamera;
        }
        */
    }
}

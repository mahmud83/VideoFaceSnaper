using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using nVlc.Declarations;
using nVlc.Declarations.Events;
using nVlc.Declarations.Media;
using nVlc.Declarations.Players;
using nVlc.Implementation;
using VideoFace.Common;
using VideoFaceSnaper.Model;
using System.Drawing;

namespace VideoFaceSnaper
{
    /// <summary>
    /// 海康人脸抓拍摄像机播放
    /// </summary>
    public class HikFilePlayer
    {
        public Action<FaceDetectInfo> ReceiveFrameEvent;
        private static bool _bIsInitial;
        private static readonly IMediaPlayerFactory Vlcfactory = new MediaPlayerFactory();
        private IMedia _vlcmedia;
        private IDiskPlayer _vlcplayer;
        private IMemoryRenderer _memRender;
        private long _currentTime; // 当前播放的位置信息，精确到毫秒
        private int _currentPosition; // 当前播放的进度
        private bool _videoPlayState;// 当前播放的状态

        public HikFilePlayer()
        {
            _vlcplayer = Vlcfactory.CreatePlayer<IDiskPlayer>();
            _vlcplayer.Events.MediaEnded += Events_MediaEnded;
            _vlcplayer.Events.PlayerStopped += Events_PlayerStopped;
            _vlcplayer.Events.TimeChanged += Events_TimeChanged;
            _vlcplayer.Events.PlayerPositionChanged += Events_PlayerPositionChanged;

            //// 调整为1.5倍速播放
            //_vlcplayer.PlaybackRate = 1.5F * _vlcplayer.PlaybackRate;
        }

        public bool Start(string videoSrcUrl)
        {
            if (_bIsInitial) return true;

            _bIsInitial = true;
            try
            {
                if (_vlcmedia != null)
                {
                    _vlcmedia.Events.StateChanged -= Events_StateChanged;
                    _vlcmedia.Events.DurationChanged -= Events_DurationChanged;
                    _vlcmedia.Dispose();
                }
                _vlcmedia = Vlcfactory.CreateMedia<IMedia>(videoSrcUrl);
                _vlcmedia.Events.StateChanged += Events_StateChanged;
                _vlcmedia.Events.DurationChanged += Events_DurationChanged;
                // 自定义显示
                _memRender = _vlcplayer.CustomRenderer;
                _memRender.SetFormat(new BitmapFormat(704, 576, ChromaType.RV24));
                _memRender.SetCallback(delegate (Bitmap frame)
                {
                    if ((frame != null)&& (ReceiveFrameEvent!= null))
                    {
                        if (_videoPlayState)
                        {
                            long currentDateTime = _currentTime;

                            Image imgd = (Bitmap)frame.Clone();
                            FaceDetectInfo facedet = new FaceDetectInfo(imgd, currentDateTime);

                            //ReceiveFrameEvent(facedet);
                            Task.Factory.StartNew(() => ReceiveFrameEvent(facedet));
                        }
                        frame.Dispose();
                        frame = null;
                    }
                });

                _vlcplayer.Open(_vlcmedia);
                _vlcplayer.Play();
                _videoPlayState = true;

                return true;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.Error("启动视频播放Capture方法出现错误:" + ex.Message);
                return false;
            }
        }

        public void Stop()
        {
            if (!_bIsInitial) return;

            if (_vlcmedia != null)
            {
                _vlcmedia.Events.StateChanged -= Events_StateChanged;
                _vlcmedia.Events.DurationChanged -= Events_DurationChanged;
                _vlcmedia.Dispose();
            }
            if ((_vlcplayer != null) && (_vlcplayer.Events != null))
            {
                _vlcplayer.Events.MediaEnded -= Events_MediaEnded;
                _vlcplayer.Events.PlayerStopped -= Events_PlayerStopped;
                _vlcplayer.Events.TimeChanged -= Events_TimeChanged;
                _vlcplayer.Events.PlayerPositionChanged -= Events_PlayerPositionChanged;
            }
            _vlcplayer = null;
        }

        #region VLC播放事件
        private void Events_PlayerStopped(object sender, EventArgs e)
        {
            _videoPlayState = false; // 结束播放
            // PlayerStoped
        }

        private void Events_MediaEnded(object sender, EventArgs e)
        {
            _videoPlayState = false;// 结束播放
            // MediaEnded
        }

        private void Events_TimeChanged(object sender, MediaPlayerTimeChanged e)
        {
            // 播放时间：当前时间
            _currentTime = e.NewTime;
            // TimeSpan.FromMilliseconds(e.NewTime).ToString().Substring(0, 8)
        }

        private void Events_PlayerPositionChanged(object sender, MediaPlayerPositionChanged e)
        {
            _currentPosition = (int)(e.NewPosition * 100);
            if (_currentPosition > 100) _currentPosition = 100; // vlc进度数据保护

            if (_currentPosition == 100)
            {
                _videoPlayState = false; // 结束播放
            }
        }

        private void Events_DurationChanged(object sender, MediaDurationChange e)
        {
            // 播放时间：总长时间
            // allframeCount,allTimes
            //(int)e.NewDuration / 1000,TimeSpan.FromMilliseconds(e.NewDuration).ToString().Substring(0, 8));
        }

        private void Events_StateChanged(object sender, MediaStateChange e)
        {
            if ((e.NewState.ToString() == "Error") || ((e.NewState.ToString() == "Stopped")))
            {
                _videoPlayState = false; // 结束播放
                Log4NetHelper.Instance.Info("执行从VLC控件播放视频:" + e.NewState.ToString());
            }
        }

        #endregion

    }
}

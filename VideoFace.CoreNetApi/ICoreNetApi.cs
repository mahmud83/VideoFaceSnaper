using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;

namespace VideoFace.CoreNetApi
{
    public struct FaceRECT
    {
        public int x;
        public int y;
        public int width;
        public int height;
    }
    /// <summary>
    /// 视频回调函数
    /// </summary>
    /// <param name="frameId">帧ID号</param>
    /// <param name="buffer">帧图片字节数组</param>
    /// <param name="width">图片宽度</param>
    /// <param name="height">图片高度</param>
    /// <param name="pitch"></param>
    /// <param name="facerectArray">人脸位置矩形</param>
    public delegate void FrameCallBack(int frameId, Bitmap image, Rectangle[] facerectArray);

    /// <summary>
    /// 人脸图像回调
    /// </summary>
    /// <param name="frameId">帧ID号</param>
    /// <param name="buffer">图片字节数组</param>
    /// <param name="width">图片宽度</param>
    /// <param name="height">图片高度</param>
    /// <param name="pitch"></param>
    /// <param name="FaceId">人脸编号</param>
    /// <param name="FaceSerial">该人脸编号的第几次图片【-1标识人员消失】</param>
    /// <param name="Score">图片质量分值</param>
    public delegate void FaceCallBack(int frameId, Bitmap image, int FaceId, int FaceSerial, double Score);

    public interface ICoreNetApi
    {
        /// <summary>
        /// 播放事件
        /// </summary>
        event FrameCallBack FrameEvent;

        /// <summary>
        /// 人脸事件
        /// </summary>
        event FaceCallBack FaceEvent;

        /// <summary>
        /// 初始化DLL
        /// </summary>
        /// <returns></returns>
        int InitFaceDll();

        /// <summary>
        /// 播放摄像枪rtsp码流视频
        /// </summary>
        /// <param name="IPC">摄像枪地址</param>
        /// <param name="fMinfaceSize">截取最小人脸，为视频宽度倍数， 如设置0.1</param>
        /// <param name="fMaxFaceSize">最大人脸范围，为视频宽度倍数， 如设置0.3</param>
        /// <returns></returns>
        int StartPlay(string IPC, double fMinfaceSize, double fMaxFaceSize);

        /// <summary>
        /// 停止播放
        /// </summary>
        /// <returns></returns>
        int EndPlay();
    }
}

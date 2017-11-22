using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace VideoFace.CoreNetApi
{

    /// <summary>
    /// 解码回调函数
    /// </summary>
    /// <param name="frameId">帧ID号</param>
    /// <param name="pImageBuf">图片字节数组</param>
    /// <param name="width">图片宽度</param>
    /// <param name="height">图片高度</param>
    /// <param name="pitch"></param>
    /// <param name="nSize">图片大小</param>
    /// <param name="facerect">人脸位置数组</param>
    /// <param name="facerectCount">人脸个数</param>
    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
    public delegate void FrameCBFun(int frameId, IntPtr pImageBuf, int width, int height, int pitch, int nSize, IntPtr facerect, int facerectCount);


    /// <summary>
    /// 人脸回调函数
    /// </summary>
    /// <param name="frameId">帧ID号</param>
    /// <param name="pImageBuf">图像字节数组</param>
    /// <param name="width">图片宽度</param>
    /// <param name="height">图片高度</param>
    /// <param name="pitch"></param>
    /// <param name="nSize">图片大小</param>
    /// <param name="FaceId">人脸标签号</param>
    /// <param name="FaceSerial">人脸贞系列号</param>
    /// <param name="Score">图片质量分值</param>
    [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
    public delegate void FaceCBFun(int frameId, IntPtr pImageBuf, int width, int height, int pitch, int nSize, int FaceId, int FaceSerial, double Score);
    public class ExtNetApi
    {

        [DllImport("VideoFace.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int InitFaceDll();

        [DllImport("VideoFace.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int StartPlay(string IPC, double fMinfaceSize, double fMaxFaceSize);

        [DllImport("VideoFace.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int EndPlay();

        [DllImport("VideoFace.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int SetFrameCallBack(FrameCBFun FrameCBFun);

        [DllImport("VideoFace.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int SetFaceCallBack(FaceCBFun FrameCBFun);
    }
}

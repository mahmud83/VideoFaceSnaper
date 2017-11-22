using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using VideoFace.Common;

namespace VideoFace.CoreNetApi
{

    public class CoreNetApi : ICoreNetApi
    {
        FrameCBFun _frameCall;
        FaceCBFun _faceCall;
        public event FrameCallBack FrameEvent;
        public event FaceCallBack FaceEvent;

        /// <summary>
        /// 结构体数组指针转换为结构体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<T> MarshalPtrToStructArray<T>(IntPtr p, int count)
        {
            List<T> array = new List<T>();
            for (int i = 0; i < count; i++, p = new IntPtr(p.ToInt32() + Marshal.SizeOf(typeof(T))))
            {
                T t = (T)Marshal.PtrToStructure(p, typeof(T));
                array.Add(t);
            }
            return array;
        }
        /// <summary>
        /// 内存buffer转换成Bitmap
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="buffer">内存数据</param>
        /// <returns>Bitmap</returns>
        public static Bitmap BufferToBitmap(int width, int height, byte[] buffer)
        {
            try
            {
                Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                int h = bmp.Height;
                int w = bmp.Width;
                BitmapData dataIn = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                if (width % 4 != 0)
                {
                    unsafe
                    {
                        byte* pIn = (byte*)(dataIn.Scan0.ToPointer());
                        fixed (byte* souce = &buffer[0])
                        {
                            //int index = 0;
                            int widthFixedLength = dataIn.Width * 3;
                            for (int y = 0; y < dataIn.Height; y++)
                            {

                                Marshal.Copy(buffer, y * widthFixedLength, (IntPtr)pIn, widthFixedLength);
                                pIn += widthFixedLength + dataIn.Stride - widthFixedLength;
                                //for (int x = 0; x < dataIn.Width; x++)
                                //{
                                //    pIn[0] = souce[index++];
                                //    pIn[1] = souce[index++];
                                //    pIn[2] = souce[index++];
                                //    pIn += 3;
                                //}

                            }
                        }
                    }
                }
                else
                {
                    Marshal.Copy(buffer, 0, dataIn.Scan0, buffer.Length);
                }
                bmp.UnlockBits(dataIn);

                return bmp;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.Warn("CoreNetApi.BufferToBitmap出现异常" + ex.Message);
                throw ex;
            }

        }

        private void FrameCBFunCallBack(int frameId, IntPtr pImageBuf, int width, int height, int pitch, int nSize, IntPtr facerect, int facerectCount)
        {
            if (FrameEvent != null)
            {
                try
                {
                    byte[] buffer = new byte[nSize];
                    Marshal.Copy(pImageBuf, buffer, 0, buffer.Length);
                    List<FaceRECT> list = MarshalPtrToStructArray<FaceRECT>(facerect, facerectCount);
                    List<System.Drawing.Rectangle> rList = new List<Rectangle>();
                    foreach (var item in list)
                    {
                        Rectangle tmp = new Rectangle(item.x, item.y, item.width, item.height);
                        rList.Add(tmp);
                    }
                    FrameEvent(frameId, BufferToBitmap(width, height, buffer), rList.ToArray());
                }
                catch (Exception ex)
                {
                    Log4NetHelper.Instance.Warn("CoreNetApi.FrameCBFunCallBack出现异常" + ex.Message);
                    throw ex;
                }

            }
        }

        private void FaceCBFunCallBack(int frameId, IntPtr pImageBuf, int width, int height, int pitch, int nSize, int FaceId, int FaceSerial, double Score)
        {
            if (FaceEvent != null)
            {
                try
                {
                    byte[] buffer = new byte[nSize];
                    Marshal.Copy(pImageBuf, buffer, 0, buffer.Length);
                    FaceEvent(frameId, BufferToBitmap(width, height, buffer), FaceId, FaceSerial, Score);
                }
                catch (Exception ex)
                {
                    Log4NetHelper.Instance.Warn("CoreNetApi.FaceCBFunCallBack出现异常" + ex.Message);
                    throw ex;
                }

            }
        }

        public int InitFaceDll()
        {
            _frameCall = new FrameCBFun(FrameCBFunCallBack);
            _faceCall = new FaceCBFun(FaceCBFunCallBack);
            int result = ExtNetApi.InitFaceDll();
            ExtNetApi.SetFrameCallBack(_frameCall);
            ExtNetApi.SetFaceCallBack(_faceCall);
            return result;
        }

        public int StartPlay(string IPC, double fMinfaceSize, double fMaxFaceSize)
        {
            return ExtNetApi.StartPlay(IPC, fMinfaceSize, fMaxFaceSize);
        }

        public int EndPlay()
        {
            return ExtNetApi.EndPlay();
        }


    }
}

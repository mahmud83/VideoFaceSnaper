using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace VideoFaceSnaper.Model
{
    /// <summary>
    /// 检测人脸图像对象
    /// </summary>
    public class FaceDetectInfo : IDisposable
    {
        public Image Img
        {
            get;
            private set;
        }

        public long FrameId
        {
            get;
            private set;
        }

        public List<Rectangle> Rects
        {
            get;
            private set;
        }

        public FaceDetectInfo(Image img, long frameId)
        {
            Img = img;
            FrameId = frameId;
            Rects = null;
        }

        public FaceDetectInfo(Image img, long frameId, List<Rectangle> facerects)
        {
            Img = img;
            FrameId = frameId;
            Rects = facerects;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            if (Img != null)
            {
                Img.Dispose();
            }
        }

        #endregion
    }
}

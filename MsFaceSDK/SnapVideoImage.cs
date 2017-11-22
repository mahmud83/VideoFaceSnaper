using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoFace.Common.Lib;

namespace MsFaceSDK
{
    /// <summary>
    /// 原抓拍对象
    /// </summary>
    public class SnapVideoImage : IDisposable
    {
        public string SourceId { get; set; }

        public string FrameId { get; set; }

        public int TraceId { get; set; }

        public DateTime CreateTime { get; set; }

        public Image Img { get; set; }

        public SnapVideoImage()
        {

        }

        public SnapVideoImage(string sourceId, string frameId, int traceId, DateTime createTime, Image snapImg)
        {
            this.SourceId = sourceId;
            this.FrameId = frameId;
            this.TraceId = traceId;
            this.CreateTime = createTime;
            this.Img = snapImg;
        }

        public SnapVideoImage(string sourceId, string frameId, int traceId, DateTime createTime, byte[] snapBte)
        {
            this.SourceId = sourceId;
            this.FrameId = frameId;
            this.TraceId = traceId;
            this.CreateTime = createTime;
            if (snapBte != null)
            {
                this.Img = ImageHelper.BytesToBitmap(snapBte);
            }
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

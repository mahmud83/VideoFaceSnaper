using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using VideoFace.Common.Lib;

namespace MsFaceSDK
{
    /// <summary>
    /// 命中原抓拍对象
    /// </summary>
    public class HitAlertInfo
    {
        public string SourceId { get; private set; }

        public DateTime CreateTime { get; private set; }

        public byte[] ImgData { get; private set; }

        public List<HitAlertInfoDetail> Detail { get; set; }

        public HitAlertInfo()
        {
            
        }

        public HitAlertInfo(string sourceId, DateTime createTime, Image queryImg)
        {
            this.SourceId = sourceId;
            this.CreateTime = createTime;
            if (queryImg != null)
            {
                this.ImgData = ImageHelper.ImageToBytes(queryImg);
                queryImg.Dispose();
            }
        }

        public HitAlertInfo(string sourceId, DateTime createTime, byte[] queryBte)
        {
            this.SourceId = sourceId;
            this.CreateTime = createTime;
            if (queryBte != null)
            {
                this.ImgData = queryBte;
            }
        }

    }

    /// <summary>
    /// 命中目标对象
    /// </summary>
    public class HitAlertInfoDetail : IComparable
    {
        public int Rank { get; set; }
        public double Score { get; private set; }
        public string PersonId { get; private set; }
        public byte[] Face { get; private set; }
        public HitAlertInfoDetail() : this(0d, string.Empty)
        {
        }

        public HitAlertInfoDetail(double fScore, string spersonid)
        {
            this.Score = Math.Round(fScore, 3);
            this.PersonId = spersonid;
        }

        public HitAlertInfoDetail(double fScore, string spersonid, byte[] faceBte)
        {
            this.Score = Math.Round(fScore, 3);
            this.PersonId = spersonid;
            if (faceBte != null)
            {
                this.Face = faceBte;
            }
        }

        public int CompareTo(object obj)
        {
            if (obj is HitAlertInfoDetail)
            {
                HitAlertInfoDetail stu = (HitAlertInfoDetail) obj;
                return (int) (this.Score - stu.Score);
            }
            else
            {
                throw new Exception("类型不兼容!");
            }
        }

    }
}

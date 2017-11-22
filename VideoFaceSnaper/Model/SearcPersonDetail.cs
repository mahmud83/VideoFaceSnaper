using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace VideoFaceSnaper.Model
{
    [Serializable]
    public class SearchCondition : IDisposable
    {
        /// <summary>
        /// 摄像机源，对应不同人像库
        /// </summary>
        [DataMember]
        public string SourceId
        {
            get;
            private set;
        }

        /// <summary>
        /// 查询图像
        /// </summary>
        public Image QueryImage
        {
            get;
            private set;
        }

        /// <summary>
        /// 从第几个结果开始返回
        /// </summary>
        [DataMember]
        public int Start { get; set; }

        /// <summary>
        /// 返回至多多少个结果
        /// </summary>
        [DataMember]
        public int Limit { get; set; }

        #region IDisposable 成员

        public void Dispose()
        {
            if (QueryImage != null)
            {
                QueryImage.Dispose();
            }
        }

        #endregion

        public SearchCondition(string sourceId, Image queryImg)
        {
            SourceId = sourceId;
            QueryImage = queryImg;
            Limit = 20;
        }
    }

    [Serializable]
    public class SearcPersonDetail : IComparable, IDisposable
    {
        /// <summary>
        /// 相似度, 0-100之间
        /// </summary>
        [DataMember]
        public double Score { get; set; }

        [DataMember]
        public string PersonId { get; set; }

        public Image Photo { get; set; }

        public SearcPersonDetail() : this(0d, "0")
        {
        }

        public SearcPersonDetail(double dScore, string personid)
        {
            this.Score = dScore;
            this.PersonId = personid;
        }

        public int CompareTo(object obj)
        {
            if (obj is SearcPersonDetail)
            {
                SearcPersonDetail stu = (SearcPersonDetail)obj;
                return (int)(this.Score - stu.Score);
            }
            else
            {
                throw new Exception("类型不兼容!");
            }
        }

        #region IDisposable 成员

        public void Dispose()
        {
            if (Photo != null)
            {
                Photo.Dispose();
            }
        }

        #endregion
    }
}

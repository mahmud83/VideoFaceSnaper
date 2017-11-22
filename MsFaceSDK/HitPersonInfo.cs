using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsFaceSDK
{
    /// <summary>
    /// 内存存储比中人员信息
    /// </summary>
    public class HitPersonInfo
    {
        /// <summary>
        /// 唯一性标识
        /// </summary>
        public DateTime HitTime { get; private set; }

        /// <summary>
        /// 人员证件号或姓名
        /// </summary>
        public string PersonId { get; private set; }

        public HitPersonInfo() : this(DateTime.Now, string.Empty)
        {
        }

        public HitPersonInfo(DateTime dateTime, string spersonid)
        {
            this.HitTime = dateTime;
            this.PersonId = spersonid;
        }
    }
}

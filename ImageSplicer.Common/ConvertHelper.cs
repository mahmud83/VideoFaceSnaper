using System;
using System.Collections.Generic;
using System.Text;

namespace ImageSplicer.Common
{
    /// <summary>
    /// 处理数据类型转换，数制转换、编码转换相关的类
    /// </summary>
    public sealed class ConvertHelper
    {

        #region 将数据转换为DateTime

        /// <summary>
        /// 将数据转换为DateTime  转换失败返回默认值
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static DateTime ToDateTime<T>(T data, DateTime defValue)
        {
            //如果为空则返回默认值
            if (data == null || Convert.IsDBNull(data))
            {
                return defValue;
            }

            try
            {
                return Convert.ToDateTime(data);
            }
            catch
            {
                return defValue;
            }
        }


        /// <summary>
        /// 将数据转换为DateTime  转换失败返回 默认值
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static DateTime ToDateTime(object data, DateTime defValue)
        {
            //如果为空则返回默认值
            if (data == null || Convert.IsDBNull(data))
            {
                return defValue;
            }

            try
            {
                return Convert.ToDateTime(data);
            }
            catch
            {
                return defValue;
            }
        }

        /// <summary>
        /// 将数据转换为DateTime  转换失败返回 默认值
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static DateTime ToDateTime(string data, DateTime defValue)
        {
            //如果为空则返回默认值
            if (string.IsNullOrEmpty(data))
            {
                return defValue;
            }

            DateTime temp = DateTime.Now;

            if (DateTime.TryParse(data, out temp))
            {
                return temp;
            }
            else
            {
                return defValue;
            }
        }

        #endregion

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime StampToDateTime(long timeStamp)
        {
            return StampToDateTime(timeStamp.ToString());
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime StampToDateTime(string timeStamp)
        {
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);

            return dateTimeStart.Add(toNow);
        }

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long DateTimeToStamp(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (long)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// 时间戳的流水号
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int DateTimeToNumber(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// TimeSpan时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="timespan"></param>
        /// <returns></returns>
        public static int TimeStampToInt(TimeSpan timespan)
        {
            TimeSpan ts1 = new TimeSpan(DateTime.Now.Date.Ticks);
            TimeSpan nowtime = timespan.Subtract(ts1);
            string stime = nowtime.Hours.ToString("00")+ nowtime.Minutes.ToString("00")+ nowtime.Seconds.ToString("00")+ nowtime.Milliseconds.ToString("000");
            return int.Parse(stime);
        }

        /// <summary>
        /// 当前时间格式转换为Unix时间戳格式
        /// </summary>
        /// <returns></returns>
        public static int TimeStampToInt()
        {
            TimeSpan ts1 = new TimeSpan(DateTime.Now.Date.Ticks);
            TimeSpan timespan = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan nowtime = timespan.Subtract(ts1);
            string stime = nowtime.Hours.ToString("00") + nowtime.Minutes.ToString("00") + nowtime.Seconds.ToString("00") + nowtime.Milliseconds.ToString("000");
            return int.Parse(stime);
        }

        /// <summary>
        /// Unix时间戳格式转换为TimeSpan时间格式
        /// </summary>
        /// <param name="timesecond"></param>
        /// <returns></returns>
        public static TimeSpan IntToTimeStamp(int timesecond)
        {
            string stime = timesecond.ToString("000000000");
            stime = DateTime.Now.Date.ToString("yyyyMMdd") + stime;
            DateTime ldatetime = DateTime.ParseExact(stime, "yyyyMMddHHmmssfff", null);

            return new TimeSpan(ldatetime.Ticks);
        }

        /// <summary>
        /// 获取唯一性的编号
        /// </summary>
        /// <returns></returns>
        public static long GenerateIntId()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }
    }
}

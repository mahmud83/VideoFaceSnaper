using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using VideoFaceSnaper.Data;

namespace VideoFaceSnaper
{
    /// <summary>
    /// 图像发送队列
    /// </summary>
    public class ResultImgQueue
    {
        /// <summary>
        /// 唯一的队列，内存存储
        /// </summary>
        private static ConcurrentQueue<string> _datequeue =
            new ConcurrentQueue<string>();

        public static void AddToQueue(string date)
        {
            lock (_datequeue)
            {
                _datequeue.Enqueue(date);
            }
        }

        public static bool IsFull(int num)
        {
            bool isFull = false;
            lock (_datequeue)
            {
                if (_datequeue.Count >= num)
                    isFull = true;
            }
            return isFull;
        }

        public static string GetFromQueue()
        {
            string data = null;
            lock (_datequeue)
            {
                if (_datequeue.Count >= 0)
                {
                    _datequeue.TryDequeue(out data); //从队列中取数据
                }
            }
            return data;
        }

        public static int GetCount()
        {
            return _datequeue.Count;
        }

    }


}

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using ImageSplicer.Data;

namespace ImageSplicer
{
    /// <summary>
    /// 分析图像接收队列
    /// </summary>
    public class ResultImageQueue
    {
        /// <summary>
        /// 唯一的队列，内存存储
        /// </summary>
        private static ConcurrentQueue<Img4kafka> _datequeue =
            new ConcurrentQueue<Img4kafka>();

        public static void AddToQueue(Img4kafka date)
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

        public static Img4kafka GetFromQueue()
        {
            Img4kafka date = null;
            lock (_datequeue)
            {
                if (_datequeue.Count >= 0)
                {
                    _datequeue.TryDequeue(out date); //从队列中取数据
                }
            }
            return date;
        }

        public static int GetCount()
        {
            return _datequeue.Count;
        }

    }


}

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace MsFaceSDK
{
    /// <summary>
    /// 分析图像队列
    /// </summary>
    class SnapImageQueue
    {
        /// <summary>
        /// 唯一的队列，内存存储
        /// </summary>
        private static ConcurrentQueue<SnapVideoImage> _datequeue =
            new ConcurrentQueue<SnapVideoImage>();

        public static void AddToQueue(SnapVideoImage data)
        {
            lock (_datequeue)
            {
                _datequeue.Enqueue(data);
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

        public static SnapVideoImage GetFromQueue()
        {
            SnapVideoImage date = null;
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

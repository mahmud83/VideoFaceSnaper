using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using VideoFaceSnaper.Data;
using VideoFaceSnaper.Model;

namespace VideoFaceSnaper
{
    /// <summary>
    /// 图像文件处理队列
    /// </summary>
    public class ResultFileQueue
    {
        /// <summary>
        /// 唯一的队列，内存存储
        /// </summary>
        private static ConcurrentQueue<FileFaceImage> _datequeue =
            new ConcurrentQueue<FileFaceImage>();

        public static void AddToQueue(FileFaceImage date)
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

        public static FileFaceImage GetFromQueue()
        {
            FileFaceImage data = null;
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

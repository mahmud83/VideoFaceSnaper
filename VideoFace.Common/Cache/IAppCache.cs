using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace VideoFace.Common.Cache
{
    public interface IAppCache
    {
        ObjectCache ObjectCache { get; }
        void Add<T>(string key, T item);
        void Add<T>(string key, T item, DateTimeOffset absoluteExpiration);
        void Add<T>(string key, T item, TimeSpan slidingExpiration);
        void Add<T>(string key, T item, CacheItemPolicy policy);

        T Get<T>(string key);

        IList<string> GetKeys();

        IList<T> GetAll<T>();

        void Remove(string key);

    }
}
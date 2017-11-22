using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace VideoFace.Common.Cache
{
    public class CachingService : IAppCache
    {
        public CachingService() : this(MemoryCache.Default)
        {
        }

        public CachingService(ObjectCache cache)
        {
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

            ObjectCache = cache;
            DefaultCacheDuration = 60 * 20; // 20分钟
        }

        /// <summary>
        /// Seconds to cache objects for by default
        /// </summary>
        public int DefaultCacheDuration { get; set; }

        private DateTimeOffset DefaultExpiryDateTime => DateTimeOffset.Now.AddSeconds(DefaultCacheDuration);

        public void Add<T>(string key, T item)
        {
            Add(key, item, DefaultExpiryDateTime);
        }

        public void Add<T>(string key, T item, DateTimeOffset expires)
        {
            Add(key, item, new CacheItemPolicy {AbsoluteExpiration = expires});
        }

        public void Add<T>(string key, T item, TimeSpan slidingExpiration)
        {
            Add(key, item, new CacheItemPolicy {SlidingExpiration = slidingExpiration});
        }

        public void Add<T>(string key, T item, CacheItemPolicy policy)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            ValidateKey(key);

            ObjectCache.Set(key, item, policy);
        }

        public T Get<T>(string key)
        {
            ValidateKey(key);

            var item = ObjectCache[key];

            return (T)item;
        }

        public IList<string> GetKeys()
        {
            List<string> lstKey= new List<string>();
            foreach (var item in ObjectCache)
            {
                lstKey.Add(item.Key);
            }
            return lstKey;
        }

        public IList<T> GetAll<T>()
        {
            List<T> lstObj = new List<T>();
            foreach (var item in ObjectCache)
            {
                lstObj.Add((T)(item.Value));
            }
            return lstObj;
        }

        public void Remove(string key)
        {
            ValidateKey(key);
            ObjectCache.Remove(key);
        }

        public ObjectCache ObjectCache { get; }

        private void ValidateKey(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentOutOfRangeException(nameof(key), "Cache keys cannot be empty or whitespace");
        }
    }
}
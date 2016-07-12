using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace CodeProjectDemo.Services.Cache
{
    public class CacheService : ICacheService
    {
        public void Set<T>(string key, T objectToCache, int minutes = 15)
        {
            CacheItemPolicy cachingPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = new DateTimeOffset(new DateTime().AddMinutes(minutes))
            };

            MemoryCache.Default.Set(key, objectToCache, cachingPolicy);
        }

        public T Get<T>(string key)
        {
            return (T)MemoryCache.Default.Get(key);
        }

        public T Remove<T>(string key)
        {
            return (T)MemoryCache.Default.Remove(key);
        }
    }
}

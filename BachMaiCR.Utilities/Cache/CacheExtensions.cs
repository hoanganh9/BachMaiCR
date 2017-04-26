using System;

namespace BachMaiCR.Utilities.Cache
{
    public static class CacheExtensions
    {
        public static T Get<T>(this ICacheProvider cacheProvider, string key, Func<T> acquire)
        {
            return cacheProvider.Get<T>(key, 60, acquire);
        }

        public static T Get<T>(this ICacheProvider cacheProvider, string key, int cacheTime, Func<T> acquire)
        {
            if (cacheProvider.IsStored(key))
                return cacheProvider.Get<T>(key);
            T obj = acquire();
            cacheProvider.Set(key, obj, cacheTime);
            return obj;
        }
    }
}
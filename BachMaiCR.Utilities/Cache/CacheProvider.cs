using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace BachMaiCR.Utilities.Cache
{
  public class CacheProvider : ICacheProvider
  {
    private static readonly object _lockCache = new object();

    private ObjectCache Cache
    {
      get
      {
        return (ObjectCache) MemoryCache.Default;
      }
    }

    public void Set(string key, object data, int cacheTime = 60)
    {
      if (data == null)
        return;
      lock (CacheProvider._lockCache)
        this.Cache.Add(new CacheItem(key, data), new CacheItemPolicy()
        {
          AbsoluteExpiration = (DateTimeOffset) (DateTime.Now + TimeSpan.FromMinutes((double) cacheTime))
        });
    }

    public T Get<T>(string key)
    {
      try
      {
        return (T) this.Cache[key];
      }
      catch (Exception ex)
      {
        return default (T);
      }
    }

    public bool IsStored(string key)
    {
      return this.Cache.Contains(key, (string) null);
    }

    public void RemoveByTerm(string term)
    {
      foreach (KeyValuePair<string, object> keyValuePair in (IEnumerable<KeyValuePair<string, object>>) this.Cache)
      {
        if (keyValuePair.Key.Contains(term))
          this.Cache.Remove(keyValuePair.Key, (string) null);
      }
    }

    public void Remove(string key)
    {
      this.Cache.Remove(key, (string) null);
    }

    public void Clear()
    {
      foreach (KeyValuePair<string, object> keyValuePair in (IEnumerable<KeyValuePair<string, object>>) this.Cache)
        this.Cache.Remove(keyValuePair.Key, (string) null);
    }
  }
}

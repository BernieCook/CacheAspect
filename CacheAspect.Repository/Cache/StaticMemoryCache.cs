using System;
using System.Collections.Generic;
using System.Linq;

namespace CacheAspect.Repository
{
    /// <summary>
    /// Static memory cache. Useful for caching on a single instance.
    /// </summary>
    /// <remarks>
    /// This class was adapted quite heavily from Matthew Groves' original StaticMemoryCache class.
    /// https://github.com/mgroves/PostSharp5/blob/master/4-Caching/StaticMemoryCache.cs
    /// </remarks>
    public class StaticMemoryCache : ICache
    {
        private readonly TimeSpan _cacheLife;
        private static readonly IList<CachedObject> Cache = new List<CachedObject>();

        public StaticMemoryCache()
        {
            _cacheLife = new TimeSpan(0, 15, 0);
        }

        public object this[string key]
        {
            get
            {
                var cacheHit = Cache.FirstOrDefault(c => c.Key == key);

                if (cacheHit != null)
                {
                    if ((DateTime.Now - cacheHit.CachedDate) <= _cacheLife)
                    {
                        return cacheHit.Value;
                    }
                    Cache.Remove(cacheHit);
                }

                return null;
            }
            set
            {
                var cacheHit = Cache.FirstOrDefault(c => c.Key == key);

                if (cacheHit != null)
                {
                    Cache.Remove(cacheHit);
                }  

                Cache.Add(new CachedObject
                    { 
                        Key = key, 
                        Value = value, 
                        CachedDate = DateTime.Now
                    });
            }
        }

        public void Remove(string keyPrefix)
        {
            var cacheHits = Cache.Where(c => c.Key.StartsWith(keyPrefix)).ToList();

            foreach (var cacheHit in cacheHits)
            {
                Cache.Remove(cacheHit);
            }
        }

        private class CachedObject
        {
            public string Key { get; set; }
            public object Value { get; set; }
            public DateTime CachedDate { get; set; }
        }
    }
}
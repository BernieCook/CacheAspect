using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ApplicationServer.Caching;

namespace CacheAspect.Repository
{
    public class AzureCache : ICache
    {
        private const string CacheKeys = "CacheKeys";
        private const string DataCacheName = "default";

        private readonly TimeSpan _defaultTimeout;
        private readonly DataCache _dataCache;

        /// <summary>
        /// Constructor.
        /// </summary>
        public AzureCache()
        {
            _defaultTimeout = new TimeSpan(1, 0, 0);

            _dataCache = new DataCache(DataCacheName);
        }

        /// <summary>
        /// Cache indexer.
        /// </summary>
        /// <param name="key">Cache key.</param>
        /// <returns>Cache value.</returns>
        public object this[string key]
        {
            get
            {
                return _dataCache.Get(key);
            }
            set
            {
                UpdateCacheKeys(key);

                _dataCache.Put(key, value, _defaultTimeout);
            }
        }

        /// <summary>
        /// Update the cache keys list.
        /// </summary>
        /// <param name="key">New key to add to the cache key list, if it doesn't already exist.</param>
        private void UpdateCacheKeys(string key)
        {
            if (key.Equals(CacheKeys))
            {
                throw new ArgumentException("The current cache key cannot be used as it's already employed by the system.", "key");
            }

            IList<string> cacheKeys;
            var cacheKeysObject = _dataCache.Get(CacheKeys);

            if (cacheKeysObject == null)
            {
                cacheKeys = new List<string>();
            }
            else
            {
                cacheKeys = (List<string>)cacheKeysObject;

                if (cacheKeys.Contains(key))
                {
                    return;
                }
            }

            cacheKeys.Add(key);
            _dataCache.Put(CacheKeys, cacheKeys, _defaultTimeout);
        }

        /// <summary>
        /// Remove the cache key, and any related keys, from cache.
        /// </summary>
        /// <param name="key">Cache key.</param>
        public void Remove(string key)
        {
            var cacheKeysObject = _dataCache.Get(CacheKeys);

            if (cacheKeysObject == null)
            {
                return;
            }

            var cacheKeys = (List<string>)cacheKeysObject;

            foreach (var cacheKey in cacheKeys.Where(c => c.StartsWith(key)).ToList())
            {
                _dataCache.Remove(cacheKey);
                cacheKeys.Remove(cacheKey);
            }

            _dataCache.Put(CacheKeys, cacheKeys, _defaultTimeout);
        }
    }
}
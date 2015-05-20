// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace Pokefans.SystemCache
{
    /// <summary>
    /// Represents a cache object based on a local cache.
    /// </summary>
    public class NativeCache : Cache
    {
        /// <summary>
        /// cache instance
        /// </summary>
        private static MemoryCache _cacheInstance = null;

        /// <summary>
        /// mutex object for synchronization
        /// </summary>
        private readonly object _cacheMutex = new object();

        /// <summary>
        /// Creates a new instance of the NativeCache-class.
        /// </summary>
        public NativeCache()
        {
            if (_cacheInstance == null)
            {
                _cacheInstance = MemoryCache.Default;
            }
        }

        /// <summary>
        /// Stores an element in the cache
        /// </summary>
        /// <exception cref="CacheEntryNotAddedException">Fired when a cache-entry cannot be added.</exception>
        /// <typeparam name="T">Type of the element to be stored</typeparam>
        /// <param name="key">Key identifying the object</param>
        /// <param name="value">Object</param>
        /// <param name="timeOffset">Time until the object shall be cached</param>
        public override void Add<T>(string key, T value, DateTimeOffset timeOffset)
        {
            _cacheInstance.Set(key, value, timeOffset);
        }

        /// <summary>
        /// Query an object from the cache.
        /// </summary>
        /// <exception cref="CacheEntryNotAddedException">Fired when a cache-entry cannot be found.</exception>
        /// <typeparam name="T">Type of the element to be retrieved</typeparam>
        /// <param name="key">Key</param>
        /// <returns>Cached object</returns>
        public override T Get<T>(string key)
        {
            if (_cacheInstance.Contains(key))
            {
                return (T)_cacheInstance.Get(key);
            }

            throw new CacheEntryNotFoundException(key);
        }

        /// <summary>
        /// Query an object from the cache.
        /// </summary>
        /// <exception cref="CacheEntryNotAddedException">Fired when a cache-entry cannot be found.</exception>
        /// <typeparam name="T">Type of the element to be retrieved</typeparam>
        /// <param name="key">Key</param>
        /// <param name="default_">Default value, if the key could not be found</param>
        /// <returns>Cached object</returns>
        public override T Get<T>(string key, T default_)
        {
            if (_cacheInstance.Contains(key))
            {
                return (T)_cacheInstance.Get(key);
            }

            return default_;
        }

        /// <summary>
        /// Removes one entry from the cache.
        /// </summary>
        /// <exception cref="CacheEntryNotAddedException">Fired when a cache-entry cannot be found.</exception>
        /// <param name="key">Key</param>
        /// <param name="silent">If false an exception will be fired in case the key cannot be found.</param>
        public override void Remove(string key, bool silent)
        {
            lock (_cacheMutex)
            {
                if (_cacheInstance.Contains(key))
                {
                    _cacheInstance.Remove(key);
                }
                else if (!silent)
                {
                    throw new CacheEntryNotFoundException(key);
                }
            }
        }

        /// <summary>
        /// Removes all data from the cache.
        /// </summary>
        public override void Purge()
        {
            lock (_cacheMutex)
            {
                // Note:
                //      Even though removing every single item is a lot slower than just creating a new MemoryCache object
                //      it is thread-safe and we won't run into some weird race conditions.
                //      Also the NativeCache class should only be used for local testing and not for live production, so
                //      the speed shouldn't be that much of an issue.

                var cacheItems = (from n in _cacheInstance.AsParallel() select n);
                foreach (KeyValuePair<String, Object> a in cacheItems)
                {
                    _cacheInstance.Remove(a.Key);
                }
            }
        }

        /// <summary>
        /// Check for an item's existence.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>true, if the object exists.</returns>
        public override bool Contains(string key)
        {
            return _cacheInstance.Contains(key);
        }
    }
}

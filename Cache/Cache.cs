// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

using System;
using System.Collections;
using System.Linq;
using System.Runtime.Caching;

namespace Pokefans.SystemCache
{
    /// <summary>
    /// Represents an abstract cache object.
    /// </summary>
    public abstract class Cache : IDisposable
    {
        /// <summary>
        /// Stores an element in the cache
        /// </summary>
        /// <exception cref="CacheEntryNotAddedException">Fired when a cache-entry cannot be found.</exception>
        /// <typeparam name="T">Type of the element to be stored</typeparam>
        /// <param name="key">Key identifying the object</param>
        /// <param name="value">Object</param>
        public void Add<T>(string key, T value)
        {
            Add(key, value, ObjectCache.InfiniteAbsoluteExpiration);
        }

        /// <summary>
        /// Stores an element in the cache
        /// </summary>
        /// <exception cref="CacheEntryNotAddedException">Fired when a cache-entry cannot be found.</exception>
        /// <typeparam name="T">Type of the element to be stored</typeparam>
        /// <param name="key">Key identifying the object</param>
        /// <param name="value">Object</param>
        /// <param name="duration">Duration the object shall be cached</param>
        public void Add<T>(string key, T value, TimeSpan duration)
        {
            DateTimeOffset releaseTime = DateTimeOffset.Now + duration;
            Add(key, value, releaseTime);
        }

        /// <summary>
        /// Stores an element in the cache
        /// </summary>
        /// <exception cref="CacheEntryNotAddedException">Fired when a cache-entry cannot be added.</exception>
        /// <typeparam name="T">Type of the element to be stored</typeparam>
        /// <param name="key">Key identifying the object</param>
        /// <param name="value">Object</param>
        /// <param name="timeOffset">Time until the object shall be cached</param>
        public abstract void Add<T>(string key, T value, DateTimeOffset timeOffset);

        /// <summary>
        /// Check for an item's existence.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>true, if the object exists.</returns>
        public abstract bool Contains(string key);

        /// <summary>
        /// Query an object from the cache.
        /// </summary>
        /// <exception cref="CacheEntryNotFoundException">Fired when a cache-entry cannot be found.</exception>
        /// <typeparam name="T">Type of the element to be retrieved</typeparam>
        /// <param name="key">Key</param>
        /// <returns>Cached object</returns>
        public abstract T Get<T>(string key);

        /// <summary>
        /// Query an object from the cache.
        /// </summary>
        /// <exception cref="CacheEntryNotFoundException">Fired when a cache-entry cannot be found.</exception>
        /// <typeparam name="T">Type of the element to be retrieved</typeparam>
        /// <param name="key">Key</param>
        /// <param name="default_">Default value, if the key could not be found</param>
        /// <returns>Cached object</returns>
        public abstract T Get<T>(string key, T default_);

        /// <summary>
        /// Query an object from the cache.
        /// </summary>
        /// <exception cref="CacheEntryNotFoundException">Fired when a cache-entry cannot be found.</exception>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public object this[string key]
        {
            get { return Get<object>(key); }

            set
            {
                Add(key, value);
            }
        }

        /// <summary>
        /// Query an object from the cache.
        /// </summary>
        /// <typeparam name="T">Type of the element to be retrieved</typeparam>
        /// <param name="key">Key</param>
        /// <param name="result">If the key exists the cached object will be stored in <paramref name="result"/></param>
        /// <returns>Cached object</returns>
        public bool TryGet<T>(string key, out T result)
        {
            if (this.Contains(key))
            {
                try
                {
                    result = this.Get<T>(key);
                    return true;
                }
                catch (Exception)
                {
                    
                }
            }

            result = default(T);
            return false;
        }

        /// <summary>
        /// Removes one entry from the cache.
        /// </summary>
        /// <exception cref="CacheEntryNotFoundException">Fired when a cache-entry cannot be found.</exception>
        /// <param name="key">Key</param>
        public void Remove(string key)
        {
            Remove(key, false);
        }

        /// <summary>
        /// Removes one entry from the cache.
        /// </summary>
        /// <exception cref="CacheEntryNotFoundException">Fired when a cache-entry cannot be found.</exception>
        /// <param name="key">Key</param>
        /// <param name="silent">If false an exception will be fired in case the key cannot be found.</param>
        public abstract void Remove(string key, bool silent);

        /// <summary>
        /// Removes all data from the cache.
        /// </summary>
        public abstract void Purge();

        /// <summary>
        /// Dispose the cache object
        /// </summary>
        public virtual void Dispose() { }
    }
}

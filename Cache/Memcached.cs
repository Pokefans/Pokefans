// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

using System;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;

namespace Pokefans.SystemCache
{
    /// <summary>
    /// Represents a cache object based on memcached.
    /// </summary>
    public class Memcached : Cache
    {
        /// <summary>
        /// memcached server
        /// </summary>
        public string Server { get; private set; }

        /// <summary>
        /// memcached port
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// true if the client is connected
        /// </summary>
        public bool Connected { get; private set; }

        /// <summary>
        /// memcached client instance
        /// </summary>
        private MemcachedClient _memcachedClient;

        /// <summary>
        /// Creates a new instance of the Memcached-class.
        /// </summary>
        /// <param name="server">memcached server</param>
        /// <param name="port">memcached port</param>
        public Memcached(string server, int port)
        {
            Server = server;
            Port = port;
            Connected = false;
            _memcachedClient = null;
        }

        /// <summary>
        /// Connects to the memcached server.
        /// </summary>
        public void Connect()
        {
            if (Connected && _memcachedClient != null)
            {
                return;
            }

            MemcachedClientConfiguration config = new MemcachedClientConfiguration();
            config.AddServer(Server, Port);
            _memcachedClient = new MemcachedClient(config);

            Connected = true;
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
            if (!Connected)
            {
                throw new CacheNotConnectedException();
            }

            if (!_memcachedClient.Store(StoreMode.Set, key, value, timeOffset.DateTime))
            {
                throw new CacheEntryNotAddedException(key);
            }
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
            if (!Connected)
            {
                throw new CacheNotConnectedException();
            }

            object value;
            if (_memcachedClient.TryGet(key, out value))
            {
                return (T)value;
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
            if (!Connected)
            {
                throw new CacheNotConnectedException();
            }

            object value;
            if (_memcachedClient.TryGet(key, out value))
            {
                return (T) value;
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
            if (!Connected)
            {
                throw new CacheNotConnectedException();
            }

            if (!silent && !Contains(key))
            {
                throw new CacheEntryNotFoundException(key);
            }

            _memcachedClient.Remove(key);
        }

        /// <summary>
        /// Removes all data from the cache.
        /// </summary>
        public override void Purge()
        {
            if (!Connected)
            {
                throw new CacheNotConnectedException();
            }

            _memcachedClient.FlushAll();
        }

        /// <summary>
        /// Check for an item's existence.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>true, if the object exists.</returns>
        public override bool Contains(string key)
        {
            if (!Connected)
            {
                throw new CacheNotConnectedException();
            }

            object value;
            return _memcachedClient.TryGet(key, out value);
        }

        /// <summary>
        /// Dispose the cache object
        /// </summary>
        public override void Dispose()
        {
            _memcachedClient.Dispose();
            _memcachedClient = null;
            Connected = false;
        }
    }
}

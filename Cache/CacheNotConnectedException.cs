// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

namespace Pokefans.SystemCache
{
    /// <summary>
    /// Fired when performing actions on a not connected cache.
    /// </summary>
    public class CacheNotConnectedException : CacheException
    {
        public CacheNotConnectedException() : base("") { }

        public CacheNotConnectedException(string message) : base(message) { }
    }
}

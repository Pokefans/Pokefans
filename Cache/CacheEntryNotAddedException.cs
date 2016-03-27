// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

namespace Pokefans.Caching
{
    /// <summary>
    /// Fired when a cache-entry cannot be added.
    /// </summary>
    public class CacheEntryNotAddedException : CacheException
    {
        public CacheEntryNotAddedException(string message) : base(message) { }
    }
}

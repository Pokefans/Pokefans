// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

namespace Pokefans.SystemCache
{
    /// <summary>
    /// Fired when a cache-entry cannot be found.
    /// </summary>
    public class CacheEntryNotFoundException : CacheException
    {
        public CacheEntryNotFoundException(string message) : base(message) { }
    }
}

// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

using System;

namespace Pokefans.Caching
{
    /// <summary>
    /// Basic exception class for caching operations.
    /// </summary>
    public class CacheException : Exception
    {
        public CacheException(string message) : base(message) { }
    }
}

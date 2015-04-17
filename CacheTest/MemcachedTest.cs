// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pokefans.SystemCache;

namespace Pokefans.CacheTest
{
    [TestClass]
    public class MemcachedTest
    {
        /// <summary>
        /// memcached instance the tests are performed on
        /// </summary>
        private Memcached _cache;

        [TestInitialize]
        public void TestInitialize()
        {
            _cache = new Memcached("87.106.30.51", 11211);
            _cache.Connect();
        }

        [TestMethod]
        public void TestCachingAddGet()
        {
            _cache.Add("key1", 123456);
            _cache.Add("key2", "Hallo Welt");
            _cache.Add("key3", 123.456);

            Assert.IsTrue(_cache.Contains("key1"));
            Assert.IsTrue(_cache.Contains("key2"));
            Assert.IsTrue(_cache.Contains("key3"));
            Assert.IsFalse(_cache.Contains("non-existing"));
            Assert.AreEqual(_cache.Get<int>("key1"), 123456);
            Assert.AreEqual(_cache.Get<string>("key2"), "Hallo Welt");
            Assert.AreEqual(_cache.Get<double>("key3"), 123.456);
            Assert.AreEqual(_cache.Get<int>("non-existing", -1), -1);
        }

        [TestMethod]
        [ExpectedException(typeof(CacheEntryNotFoundException))]
        public void TestCachingExpiration()
        {
            _cache.Add("entry", 123, TimeSpan.FromTicks(1));

            Thread.Sleep(TimeSpan.FromSeconds(1));

            _cache.Get<int>("entry");
        }

        [TestMethod]
        public void TestCachingRemoveSilent()
        {
            _cache.Add("key1", 123456);
            _cache.Remove("key1");
            Assert.AreEqual(_cache.Get<int>("key1", 0), 0);
        }

        [TestMethod]
        [ExpectedException(typeof(CacheEntryNotFoundException))]
        public void TestCachingRemove()
        {
            _cache.Add("key1", 123456);

            _cache.Remove("key1");
            _cache.Get<int>("key1");
        }

        [TestMethod]
        [ExpectedException(typeof(CacheEntryNotFoundException))]
        public void TestCachingPurge()
        {
            _cache.Add("key1", 123456);
            _cache.Add("key2", 678910);

            _cache.Purge();

            _cache.Get<int>("key1");
            _cache.Get<int>("key2");
        }

        [TestMethod]
        public void TestComplexObject()
        {
            ComplexObject obj = new ComplexObject(5, 7);
            _cache.Add("object", obj);

            ComplexObject obj2 = _cache.Get<ComplexObject>("object");

            Assert.AreEqual<int>(obj.Foo(), obj2.Foo());
        }
    }
}

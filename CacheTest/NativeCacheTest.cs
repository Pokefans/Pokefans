// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pokefans.SystemCache;

namespace Pokefans.CacheTest
{
    [Serializable]
    class ComplexObject
    {
        public int A;
        public int B;

        public ComplexObject(int a, int b)
        {
            A = a;
            B = b;
        }

        public int Foo()
        {
            return A + B;
        }
    }

    [TestClass]
    public class NativeCacheTest
    {
        [TestMethod]
        public void TestCreateCache()
        {
            Cache cache = new NativeCache();
        }

        [TestMethod]
        public void TestCachingAddGet()
        {
            Cache cache = new NativeCache();
            cache.Add("key1", 123456);
            cache.Add("key2", "Hallo Welt");
            cache.Add("key3", 123.456);

            Assert.IsTrue(cache.Contains("key1"));
            Assert.IsTrue(cache.Contains("key2"));
            Assert.IsTrue(cache.Contains("key3"));
            Assert.IsFalse(cache.Contains("non-existing"));
            Assert.AreEqual(cache.Get<int>("key1"), 123456);
            Assert.AreEqual(cache.Get<string>("key2"), "Hallo Welt");
            Assert.AreEqual(cache.Get<double>("key3"), 123.456);
            Assert.AreEqual(cache.Get<int>("non-existing", -1), -1);
        }

        [TestMethod]
        [ExpectedException(typeof(CacheEntryNotFoundException))]
        public void TestCachingExpiration()
        {
            Cache cache = new NativeCache();
            cache.Add("entry", 123, TimeSpan.FromTicks(1));

            Thread.Sleep(TimeSpan.FromSeconds(1));

            cache.Get<int>("entry");
        }

        [TestMethod]
        public void TestCachingRemoveSilent()
        {
            Cache cache = new NativeCache();
            cache.Add("key1", 123456);
            cache.Remove("key1");
            Assert.AreEqual(cache.Get<int>("key1", 0), 0);
        }

        [TestMethod]
        [ExpectedException(typeof(CacheEntryNotFoundException))]
        public void TestCachingRemove()
        {
            Cache cache = new NativeCache();
            cache.Add("key1", 123456);

            cache.Remove("key1");
            cache.Get<int>("key1");
        }

        [TestMethod]
        [ExpectedException(typeof(CacheEntryNotFoundException))]
        public void TestCachingPurge()
        {
            Cache cache = new NativeCache();
            cache.Add("key1", 123456);
            cache.Add("key2", 678910);

            cache.Purge();

            cache.Get<int>("key1");
            cache.Get<int>("key2");
        }

        [TestMethod]
        [ExpectedException(typeof(CacheEntryNotFoundException))]
        public void TestDifferentCaches()
        {
            Cache cache1 = new NativeCache("cache1");
            Cache cache2 = new NativeCache("cache2");

            cache1.Add("A", 5);
            cache2.Get<int>("A");
        }

        [TestMethod]
        public void TestComplexObject()
        {
            Cache cache = new NativeCache();

            ComplexObject obj = new ComplexObject(5, 7);
            cache.Add("object", obj);

            ComplexObject obj2 = cache.Get<ComplexObject>("object");

            Assert.AreEqual<int>(obj.Foo(), obj2.Foo());
        }
    }
}

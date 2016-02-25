// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

using NUnit.Framework;
using Pokefans.Caching;
using System;
using System.Threading;

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

    [TestFixture]
    public class NativeCacheTest
    {
        [Test]
        public void TestCreateCache()
        {
            Cache cache = new NativeCache();
        }

        [Test]
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

            int val;
            Assert.IsTrue(cache.TryGet("key1", out val));
            Assert.AreEqual(val, 123456);
        }

        [Test]
        [ExpectedException(typeof(CacheEntryNotFoundException))]
        public void TestCachingExpiration()
        {
            Cache cache = new NativeCache();
            cache.Add("entry", 123, TimeSpan.FromTicks(1));

            Thread.Sleep(TimeSpan.FromSeconds(1));

            cache.Get<int>("entry");
        }

        [Test]
        public void TestCachingRemoveSilent()
        {
            Cache cache = new NativeCache();
            cache.Add("key1", 123456);
            cache.Remove("key1");
            Assert.AreEqual(cache.Get<int>("key1", 0), 0);
        }

        [Test]
        [ExpectedException(typeof(CacheEntryNotFoundException))]
        public void TestCachingRemove()
        {
            Cache cache = new NativeCache();
            cache.Add("key1", 123456);

            cache.Remove("key1");
            cache.Get<int>("key1");
        }

        [Test]
        public void TestCachingPurge()
        {
            Cache cache = new NativeCache();
            cache.Add("key1", 123456);
            cache.Add("key2", 678910);

            cache.Purge();


            Assert.Catch<CacheEntryNotFoundException>(() => cache.Get<int>("key1"));
            Assert.Catch<CacheEntryNotFoundException>(() => cache.Get<int>("key2"));
        }

        [Test]
        public void TestDifferentCaches()
        {
            Cache cache1 = new NativeCache(/*"cache1"*/);
            Cache cache2 = new NativeCache(/*"cache2"*/);

            cache1.Add("A", 5);
            Assert.AreEqual(cache2.Get<int>("A"), 5);
        }

        [Test]
        public void TestComplexObject()
        {
            Cache cache = new NativeCache();

            ComplexObject obj = new ComplexObject(5, 7);
            cache.Add("object", obj);

            ComplexObject obj2 = cache.Get<ComplexObject>("object");

            Assert.AreEqual(obj.Foo(), obj2.Foo());
        }
    }
}

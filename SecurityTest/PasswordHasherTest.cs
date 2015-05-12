// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using NUnit.Framework;
using Pokefans.Security;

namespace Pokefans.SecurityTest
{
    [TestFixture]
    class PasswordHasherTest
    {
        [Test]
        public void HashTest()
        {
            Pbkdf2PasswordHasher hasher = new Pbkdf2PasswordHasher();

            string hash = hasher.HashPassword("abcd123");

            Assert.AreEqual(89, hash.Length);
        }

        [Test]
        public void CheckHashTest()
        {
            Pbkdf2PasswordHasher hasher = new Pbkdf2PasswordHasher();

            var result = hasher.VerifyHashedPassword("vGawPyJwb3h4gpYiyxptJdUS31lLLiVhJk2yzlsskx0=:doTdEmoK8ACsuIrLrp4riGYCqi6DEn4CyitpOqoVrFA=", "abcd123");

            Assert.AreEqual(PasswordVerificationResult.Success, result);
        }
    }
}

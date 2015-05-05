// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Pokefans.Data;
using Pokefans.Security;
using Pokefans.Security.Exceptions;
using Pokefans.SystemCache;

namespace Pokefans.SecurityTest
{
    [TestFixture]
    class UserExtensionsTest
    {
        [Test]
        public void GenerateUrlTest()
        {
            User u = new User();
            u.Name = "Legomeister 1337";

            string Url = u.GenerateUrl();

            Assert.AreEqual("legomeister+1337", Url);
        }

        [Test]
        public void UserHasPermissionTest()
        {
            User u = new User()
            {
                id = 0,
                Name = "Testuser",
                Email = "test@example.com",
                Salt = "yAuUlCrmGWnxI/9gPA8MMM5p3eSOkTgKAgEOS7+RsKk=",
                Password = "1cuYOl6iGwC9nJnPmI6sHfzNEww2ZesPY0ljsoxK5S4=",
                Activationkey = "12345678abcd"
            };
            WebSecurity.CurrentUser = u;

            var data = new List<Permission>()
            {
                new Permission() { Name = "testperm", FriendlyName="Test-Permisson", Id = 0 }
            }.AsQueryable();

            // Jointable
            var data2 = new List<UserPermission>()
            {
                new UserPermission() { Id = 0, PermissionId = 0, UserId = 0 }
            }.AsQueryable();

            var emock = new Mock<Entities>();
            var mockSet = new Mock<DbSet<Permission>>();
            var jmockSet = new Mock<DbSet<UserPermission>>();

            mockSet.As<IQueryable<Permission>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Permission>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Permission>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Permission>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            jmockSet.As<IQueryable<UserPermission>>().Setup(m => m.Provider).Returns(data2.Provider);
            jmockSet.As<IQueryable<UserPermission>>().Setup(m => m.Expression).Returns(data2.Expression);
            jmockSet.As<IQueryable<UserPermission>>().Setup(m => m.ElementType).Returns(data2.ElementType);
            jmockSet.As<IQueryable<UserPermission>>().Setup(m => m.GetEnumerator()).Returns(data2.GetEnumerator());

            emock.Setup(x => x.Permissions).Returns(mockSet.Object);
            emock.Setup(x => x.UserPermissions).Returns(jmockSet.Object);

            bool permResult = u.HasPermission("testperm", new Mock<Cache>().Object, emock.Object);

            Assert.IsTrue(permResult);
        }

        [Test]
        public void UserHasNotPermissionTest()
        {
            User u = new User()
            {
                id = 0,
                Name = "Testuser",
                Email = "test@example.com",
                Salt = "yAuUlCrmGWnxI/9gPA8MMM5p3eSOkTgKAgEOS7+RsKk=",
                Password = "1cuYOl6iGwC9nJnPmI6sHfzNEww2ZesPY0ljsoxK5S4=",
                Activationkey = "12345678abcd"
            };
            WebSecurity.CurrentUser = u;

            var data = new List<Permission>()
            {
                new Permission() { Name = "testperm2", FriendlyName="Test-Permisson", Id = 0 }
            }.AsQueryable();

            // Jointable
            var data2 = new List<UserPermission>()
            {
                
            }.AsQueryable();

            var emock = new Mock<Entities>();
            var mockSet = new Mock<DbSet<Permission>>();
            var jmockSet = new Mock<DbSet<UserPermission>>();

            mockSet.As<IQueryable<Permission>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Permission>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Permission>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Permission>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            jmockSet.As<IQueryable<UserPermission>>().Setup(m => m.Provider).Returns(data2.Provider);
            jmockSet.As<IQueryable<UserPermission>>().Setup(m => m.Expression).Returns(data2.Expression);
            jmockSet.As<IQueryable<UserPermission>>().Setup(m => m.ElementType).Returns(data2.ElementType);
            jmockSet.As<IQueryable<UserPermission>>().Setup(m => m.GetEnumerator()).Returns(data2.GetEnumerator());

            emock.Setup(x => x.Permissions).Returns(mockSet.Object);
            emock.Setup(x => x.UserPermissions).Returns(jmockSet.Object);

            bool permResult = u.HasPermission("testperm2", new Mock<Cache>().Object, emock.Object);

            Assert.IsFalse(permResult);
        }

        [Test]
        [ExpectedException(typeof(PermissionNotFoundException))]
        public void UserNotPermissionPermissionNonexistantTest()
        {
            User u = new User()
            {
                id = 0,
                Name = "Testuser",
                Email = "test@example.com",
                Salt = "yAuUlCrmGWnxI/9gPA8MMM5p3eSOkTgKAgEOS7+RsKk=",
                Password = "1cuYOl6iGwC9nJnPmI6sHfzNEww2ZesPY0ljsoxK5S4=",
                Activationkey = "12345678abcd"
            };
            WebSecurity.CurrentUser = u;

            var data = new List<Permission>()
            {
                new Permission() { Name = "testperm2", FriendlyName="Test-Permisson", Id = 0 }
            }.AsQueryable();

            // Jointable
            var data2 = new List<UserPermission>()
            {

            }.AsQueryable();

            var emock = new Mock<Entities>();
            var mockSet = new Mock<DbSet<Permission>>();
            var jmockSet = new Mock<DbSet<UserPermission>>();

            mockSet.As<IQueryable<Permission>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Permission>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Permission>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Permission>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            jmockSet.As<IQueryable<UserPermission>>().Setup(m => m.Provider).Returns(data2.Provider);
            jmockSet.As<IQueryable<UserPermission>>().Setup(m => m.Expression).Returns(data2.Expression);
            jmockSet.As<IQueryable<UserPermission>>().Setup(m => m.ElementType).Returns(data2.ElementType);
            jmockSet.As<IQueryable<UserPermission>>().Setup(m => m.GetEnumerator()).Returns(data2.GetEnumerator());

            emock.Setup(x => x.Permissions).Returns(mockSet.Object);
            emock.Setup(x => x.UserPermissions).Returns(jmockSet.Object);

            bool permResult = u.HasPermission("testperm", new Mock<Cache>().Object, emock.Object);

            Assert.IsFalse(permResult);
        }
    }
}

// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using Moq;
using Pokefans.Data;
using Pokefans.Security;
using System.Web.Security;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.IO;
using System.Collections.Specialized;
using Pokefans.Security.Exceptions;
using NUnit.Framework;

namespace Pokefans.SecurityTest
{
    [TestFixture]
    public class MembershipProviderTest
    {
        [Test]
        public void AddUserTest()
        {
            var data = new List<User>
            {
            }.AsQueryable();

            var emock = new Mock<Entities>();
            var mockSet = new Mock<DbSet<User>>();

            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());


            emock.Setup(x => x.Users).Returns(mockSet.Object);

            var ctxmock = new Mock<HttpContextBase>();
            var reqmock = new Mock<HttpRequestBase>();

            var srvvars = new NameValueCollection()
            {
                { "REMOTE_ADDR", "10.13.37.42" }
            };

            reqmock.Setup(x => x.ServerVariables).Returns(srvvars);

            ctxmock.Setup(x => x.Request).Returns(reqmock.Object);

            PokefansMembershipProvider pmp = new PokefansMembershipProvider(emock.Object, ctxmock.Object);

            MembershipCreateStatus status;
            pmp.CreateUser("Testuser", "12345", "test@example.com", null, null, true, null, out status);

            Assert.AreEqual(MembershipCreateStatus.Success, status);

            mockSet.Verify(m => m.Add(It.IsAny<User>()), Times.Once());
            emock.Verify(m => m.SaveChanges(), Times.Once());

        }

        [Test]
        public void AddUserFailDuplicatEmailTest()
        {
            var data = new List<User>
            {
                new User() { Name = "Testuser", Email = "test@example.com" }
            }.AsQueryable();

            var emock = new Mock<Entities>();
            var mockSet = new Mock<DbSet<User>>();

            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());


            emock.Setup(x => x.Users).Returns(mockSet.Object);

            var ctxmock = new Mock<HttpContextBase>();
            var reqmock = new Mock<HttpRequestBase>();

            var srvvars = new NameValueCollection()
            {
                { "REMOTE_ADDR", "10.13.37.42" }
            };

            reqmock.Setup(x => x.ServerVariables).Returns(srvvars);

            ctxmock.Setup(x => x.Request).Returns(reqmock.Object);

            PokefansMembershipProvider pmp = new PokefansMembershipProvider(emock.Object, ctxmock.Object);

            MembershipCreateStatus status;
            pmp.CreateUser("nichtdertestuser", "12345", "test@example.com", null, null, true, null, out status);

            Assert.AreEqual(MembershipCreateStatus.DuplicateEmail, status);

            mockSet.Verify(m => m.Add(It.IsAny<User>()), Times.Never());
            emock.Verify(m => m.SaveChanges(), Times.Never());
        }

        [Test]
        public void AddUserFailInvalidUsernameTest()
        {
            var data = new List<User>
            {
            }.AsQueryable();

            var emock = new Mock<Entities>();
            var mockSet = new Mock<DbSet<User>>();

            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());


            emock.Setup(x => x.Users).Returns(mockSet.Object);

            var ctxmock = new Mock<HttpContextBase>();
            var reqmock = new Mock<HttpRequestBase>();

            var srvvars = new NameValueCollection()
            {
                { "REMOTE_ADDR", "10.13.37.42" }
            };

            reqmock.Setup(x => x.ServerVariables).Returns(srvvars);

            ctxmock.Setup(x => x.Request).Returns(reqmock.Object);

            PokefansMembershipProvider pmp = new PokefansMembershipProvider(emock.Object, ctxmock.Object);

            MembershipCreateStatus status;
            pmp.CreateUser("ForbiddenUsername", "12345", "test@example.com", null, null, true, null, out status);

            Assert.AreEqual(MembershipCreateStatus.InvalidUserName, status);

            mockSet.Verify(m => m.Add(It.IsAny<User>()), Times.Never());
            emock.Verify(m => m.SaveChanges(), Times.Never());
        }

        [Test]
        public void AddUserFailDuplicateUsername()
        {
            var data = new List<User>
            {
                new User() { id = 0, Name = "Testuser", Email = "test@example.com", Status = 1, Registered = DateTime.Now  }
            }.AsQueryable();

            var emock = new Mock<Entities>();
            var mockSet = new Mock<DbSet<User>>();

            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());


            emock.Setup(x => x.Users).Returns(mockSet.Object);

            var ctxmock = new Mock<HttpContextBase>();
            var reqmock = new Mock<HttpRequestBase>();

            var srvvars = new NameValueCollection()
            {
                { "REMOTE_ADDR", "10.13.37.42" }
            };

            reqmock.Setup(x => x.ServerVariables).Returns(srvvars);

            ctxmock.Setup(x => x.Request).Returns(reqmock.Object);

            PokefansMembershipProvider pmp = new PokefansMembershipProvider(emock.Object, ctxmock.Object);

            MembershipCreateStatus status;
            pmp.CreateUser("Testuser", "12345", "test2@example.com", null, null, true, null, out status);

            Assert.AreEqual(MembershipCreateStatus.DuplicateUserName, status);

            mockSet.Verify(m => m.Add(It.IsAny<User>()), Times.Never());
            emock.Verify(m => m.SaveChanges(), Times.Never());
        }

        [Test]
        public void DeleteUserTest()
        {
            var data = new List<User>
            {
                new User() { id = 0, Name = "Testuser", Email = "test@example.com", Status = 1, Registered = DateTime.Now  }
            }.AsQueryable();

            var emock = new Mock<Entities>();
            var mockSet = new Mock<DbSet<User>>();

            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());


            emock.Setup(x => x.Users).Returns(mockSet.Object);

            var ctxmock = new Mock<HttpContextBase>();

            PokefansMembershipProvider pmp = new PokefansMembershipProvider(emock.Object, ctxmock.Object);

            pmp.DeleteUser("test@example.com", false);

            mockSet.Verify(m => m.Remove(It.IsAny<User>()), Times.Once());
            emock.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Test]
        [ExpectedException(typeof(UserNotFoundException))]
        public void DeleteUserNotFoundTest()
        {
            var data = new List<User>
            {
                new User() { id = 0, Name = "Testuser", Email = "test@example.com", Status = 1, Registered = DateTime.Now  }
            }.AsQueryable();

            var emock = new Mock<Entities>();
            var mockSet = new Mock<DbSet<User>>();

            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());


            emock.Setup(x => x.Users).Returns(mockSet.Object);

            var ctxmock = new Mock<HttpContextBase>();

            PokefansMembershipProvider pmp = new PokefansMembershipProvider(emock.Object, ctxmock.Object);

            pmp.DeleteUser("test2@example.com", false);
        }

        [Test]
        public void ChangePasswordTest()
        {
            var data = new List<User>
            {
                new User() { id = 0, Name = "Testuser", Salt = "yAuUlCrmGWnxI/9gPA8MMM5p3eSOkTgKAgEOS7+RsKk=", Password = "1cuYOl6iGwC9nJnPmI6sHfzNEww2ZesPY0ljsoxK5S4="}
            }.AsQueryable();

            var emock = new Mock<Entities>();
            var mockSet = new Mock<DbSet<User>>();

            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());


            emock.Setup(x => x.Users).Returns(mockSet.Object);
            emock.Setup(x => x.SetModified(It.IsAny<User>())).Callback<object>((object o) =>
            {
                Assert.AreEqual("Testuser", ((User)o).Name);
            });

            var ctxmock = new Mock<HttpContextBase>();
            var reqmock = new Mock<HttpRequestBase>();

            var srvvars = new NameValueCollection()
            {
                { "REMOTE_ADDR", "10.13.37.42" }
            };

            reqmock.Setup(x => x.ServerVariables).Returns(srvvars);

            ctxmock.Setup(x => x.Request).Returns(reqmock.Object);

            PokefansMembershipProvider pmp = new PokefansMembershipProvider(emock.Object, ctxmock.Object);

            var result = pmp.ChangePassword("Testuser", "12345", "123456");

            Assert.AreEqual(true, result);

            emock.Verify(m => m.SetModified(It.IsAny<User>()), Times.Once());
            emock.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Test]
        [ExpectedException(typeof(PasswordMismatchException))]
        public void ChangePasswordMismatchTest()
        {
            var data = new List<User>
            {
                new User() { id = 0, Name = "Testuser", Salt = "yAuUlCrmGWnxI/9gPA8MMM5p3eSOkTgKAgEOS7+RsKk=", Password = "1cuYOl6iGwC9nJnPmI6sHfzNEww2ZesPY0ljsoxK5S4="}
            }.AsQueryable();

            var emock = new Mock<Entities>();
            var mockSet = new Mock<DbSet<User>>();

            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());


            emock.Setup(x => x.Users).Returns(mockSet.Object);

            var ctxmock = new Mock<HttpContextBase>();
            var reqmock = new Mock<HttpRequestBase>();

            var srvvars = new NameValueCollection()
            {
                { "REMOTE_ADDR", "10.13.37.42" }
            };

            reqmock.Setup(x => x.ServerVariables).Returns(srvvars);

            ctxmock.Setup(x => x.Request).Returns(reqmock.Object);

            PokefansMembershipProvider pmp = new PokefansMembershipProvider(emock.Object, ctxmock.Object);

            var result = pmp.ChangePassword("Testuser", "123457", "123456");
        }

        [Test]
        [ExpectedException(typeof(UserNotFoundException))]
        public void ChangePasswordUserNotFoundTest()
        {
            var data = new List<User>
            {
                new User() { id = 0, Name = "Testuser", Salt = "yAuUlCrmGWnxI/9gPA8MMM5p3eSOkTgKAgEOS7+RsKk=", Password = "1cuYOl6iGwC9nJnPmI6sHfzNEww2ZesPY0ljsoxK5S4="}
            }.AsQueryable();

            var emock = new Mock<Entities>();
            var mockSet = new Mock<DbSet<User>>();

            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());


            emock.Setup(x => x.Users).Returns(mockSet.Object);

            var ctxmock = new Mock<HttpContextBase>();
            var reqmock = new Mock<HttpRequestBase>();

            var srvvars = new NameValueCollection()
            {
                { "REMOTE_ADDR", "10.13.37.42" }
            };

            reqmock.Setup(x => x.ServerVariables).Returns(srvvars);

            ctxmock.Setup(x => x.Request).Returns(reqmock.Object);

            PokefansMembershipProvider pmp = new PokefansMembershipProvider(emock.Object, ctxmock.Object);

            var result = pmp.ChangePassword("Testluser", "12345", "123456");
        }

        [Test]
        public void ResetPasswordTest()
        {
            var data = new List<User>
            {
                new User() { id = 0, Name = "Testuser", Email="test@example.com", Salt = "yAuUlCrmGWnxI/9gPA8MMM5p3eSOkTgKAgEOS7+RsKk=", Password = "1cuYOl6iGwC9nJnPmI6sHfzNEww2ZesPY0ljsoxK5S4="}
            }.AsQueryable();

            var emock = new Mock<Entities>();
            var mockSet = new Mock<DbSet<User>>();

            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            emock.Setup(x => x.SetModified(It.IsAny<User>())).Callback<object>((object o) =>
            {
                Assert.AreEqual("Testuser", ((User)o).Name);
            });


            emock.Setup(x => x.Users).Returns(mockSet.Object);

            var ctxmock = new Mock<HttpContextBase>();
            var reqmock = new Mock<HttpRequestBase>();

            var srvvars = new NameValueCollection()
            {
                { "REMOTE_ADDR", "10.13.37.42" }
            };

            reqmock.Setup(x => x.ServerVariables).Returns(srvvars);

            ctxmock.Setup(x => x.Request).Returns(reqmock.Object);

            PokefansMembershipProvider pmp = new PokefansMembershipProvider(emock.Object, ctxmock.Object);

            var newPass = pmp.ResetPassword("test@example.com", "irrelevant");

            Assert.IsNotNull(newPass);

            var ok = pmp.ValidateUser("test@example.com", newPass);

            Assert.IsTrue(ok);

            emock.Verify(m => m.SetModified(It.IsAny<User>()), Times.Once());
            emock.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Test]
        [ExpectedException(typeof(UserNotFoundException))]
        public void ResetPasswordUserNotFoundTest()
        {
            var data = new List<User>
            {
                new User() { id = 0, Name = "Testuser", Email="test@example.com", Salt = "yAuUlCrmGWnxI/9gPA8MMM5p3eSOkTgKAgEOS7+RsKk=", Password = "1cuYOl6iGwC9nJnPmI6sHfzNEww2ZesPY0ljsoxK5S4="}
            }.AsQueryable();

            var emock = new Mock<Entities>();
            var mockSet = new Mock<DbSet<User>>();

            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            emock.Setup(x => x.SetModified(It.IsAny<User>())).Callback<object>((object o) =>
            {
                Assert.AreEqual("Testuser", ((User)o).Name);
            });


            emock.Setup(x => x.Users).Returns(mockSet.Object);

            var ctxmock = new Mock<HttpContextBase>();
            var reqmock = new Mock<HttpRequestBase>();

            var srvvars = new NameValueCollection()
            {
                { "REMOTE_ADDR", "10.13.37.42" }
            };

            reqmock.Setup(x => x.ServerVariables).Returns(srvvars);

            ctxmock.Setup(x => x.Request).Returns(reqmock.Object);

            PokefansMembershipProvider pmp = new PokefansMembershipProvider(emock.Object, ctxmock.Object);

            var newPass = pmp.ResetPassword("test2@example.com", "irrelevant");
        }

        [Test]
        public void ActivateUserTest()
        {
            var data = new List<User>
            {
                new User() { id = 0, Name = "Testuser", Email="test@example.com", Salt = "yAuUlCrmGWnxI/9gPA8MMM5p3eSOkTgKAgEOS7+RsKk=", Password = "1cuYOl6iGwC9nJnPmI6sHfzNEww2ZesPY0ljsoxK5S4=", Activationkey="12345678abcd"}
            }.AsQueryable();

            var emock = new Mock<Entities>();
            var mockSet = new Mock<DbSet<User>>();

            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            emock.Setup(x => x.SetModified(It.IsAny<User>())).Callback<object>((object o) =>
            {
                Assert.AreEqual("Testuser", ((User)o).Name);
            });


            emock.Setup(x => x.Users).Returns(mockSet.Object);

            var ctxmock = new Mock<HttpContextBase>();
            var reqmock = new Mock<HttpRequestBase>();

            var srvvars = new NameValueCollection()
            {
                { "REMOTE_ADDR", "10.13.37.42" }
            };

            reqmock.Setup(x => x.ServerVariables).Returns(srvvars);

            ctxmock.Setup(x => x.Request).Returns(reqmock.Object);

            PokefansMembershipProvider pmp = new PokefansMembershipProvider(emock.Object, ctxmock.Object);

            var result = pmp.ActivateUser("test@example.com", "12345678abcd");

            Assert.IsTrue(result);

            emock.Verify(m => m.SetModified(It.IsAny<User>()), Times.Once());
            emock.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Test]
        [ExpectedException(typeof(UserNotFoundException))]
        public void ActivateUserNotFoundTest()
        {
            var data = new List<User>
            {
                new User() { id = 0, Name = "Testuser", Email="test@example.com", Salt = "yAuUlCrmGWnxI/9gPA8MMM5p3eSOkTgKAgEOS7+RsKk=", Password = "1cuYOl6iGwC9nJnPmI6sHfzNEww2ZesPY0ljsoxK5S4=", Activationkey="12345678abcd"}
            }.AsQueryable();

            var emock = new Mock<Entities>();
            var mockSet = new Mock<DbSet<User>>();

            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            emock.Setup(x => x.SetModified(It.IsAny<User>())).Callback<object>((object o) =>
            {
                Assert.AreEqual("Testuser", ((User)o).Name);
            });


            emock.Setup(x => x.Users).Returns(mockSet.Object);

            var ctxmock = new Mock<HttpContextBase>();
            var reqmock = new Mock<HttpRequestBase>();

            var srvvars = new NameValueCollection()
            {
                { "REMOTE_ADDR", "10.13.37.42" }
            };

            reqmock.Setup(x => x.ServerVariables).Returns(srvvars);

            ctxmock.Setup(x => x.Request).Returns(reqmock.Object);

            PokefansMembershipProvider pmp = new PokefansMembershipProvider(emock.Object, ctxmock.Object);

            var result = pmp.ActivateUser("test1@example.com", "12345678abcd");

            Assert.IsTrue(result);

            emock.Verify(m => m.SetModified(It.IsAny<User>()), Times.Once());
            emock.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Test]
        public void ActivateUserInvalidKeyTest()
        {
            var data = new List<User>
            {
                new User() { id = 0, Name = "Testuser", Email="test@example.com", Salt = "yAuUlCrmGWnxI/9gPA8MMM5p3eSOkTgKAgEOS7+RsKk=", Password = "1cuYOl6iGwC9nJnPmI6sHfzNEww2ZesPY0ljsoxK5S4=", Activationkey="12345678abcd"}
            }.AsQueryable();

            var emock = new Mock<Entities>();
            var mockSet = new Mock<DbSet<User>>();

            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            emock.Setup(x => x.SetModified(It.IsAny<User>())).Callback<object>((object o) =>
            {
                Assert.AreEqual("Testuser", ((User)o).Name);
            });


            emock.Setup(x => x.Users).Returns(mockSet.Object);

            var ctxmock = new Mock<HttpContextBase>();
            var reqmock = new Mock<HttpRequestBase>();

            var srvvars = new NameValueCollection()
            {
                { "REMOTE_ADDR", "10.13.37.42" }
            };

            reqmock.Setup(x => x.ServerVariables).Returns(srvvars);

            ctxmock.Setup(x => x.Request).Returns(reqmock.Object);

            PokefansMembershipProvider pmp = new PokefansMembershipProvider(emock.Object, ctxmock.Object);

            var result = pmp.ActivateUser("test@example.com", "nichtderkey");

            Assert.IsFalse(result);

        }

        [Test]
        public void ValidateUserTest()
        {
            var data = new List<User>
            {
                new User() { id = 0, Name = "Testuser", Email = "test@example.com", Salt = "yAuUlCrmGWnxI/9gPA8MMM5p3eSOkTgKAgEOS7+RsKk=", Password = "1cuYOl6iGwC9nJnPmI6sHfzNEww2ZesPY0ljsoxK5S4="}
            }.AsQueryable();

            var emock = new Mock<Entities>();
            var mockSet = new Mock<DbSet<User>>();

            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());


            emock.Setup(x => x.Users).Returns(mockSet.Object);
            emock.Setup(x => x.SetModified(It.IsAny<User>())).Callback<object>((object o) =>
            {
                Assert.AreEqual("Testuser", ((User)o).Name);
            });

            var ctxmock = new Mock<HttpContextBase>();
            var reqmock = new Mock<HttpRequestBase>();

            var srvvars = new NameValueCollection()
            {
                { "REMOTE_ADDR", "10.13.37.42" }
            };

            reqmock.Setup(x => x.ServerVariables).Returns(srvvars);

            ctxmock.Setup(x => x.Request).Returns(reqmock.Object);

            PokefansMembershipProvider pmp = new PokefansMembershipProvider(emock.Object, ctxmock.Object);

            var result = pmp.ValidateUser("test@example.com", "12345");

            Assert.IsTrue(result);
        }

        [Test]
        public void ValidateUserInvalidPasswordTest()
        {
            var data = new List<User>
            {
                new User() { id = 0, Name = "Testuser", Email = "test@example.com", Salt = "yAuUlCrmGWnxI/9gPA8MMM5p3eSOkTgKAgEOS7+RsKk=", Password = "1cuYOl6iGwC9nJnPmI6sHfzNEww2ZesPY0ljsoxK5S4="}
            }.AsQueryable();

            var emock = new Mock<Entities>();
            var mockSet = new Mock<DbSet<User>>();

            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());


            emock.Setup(x => x.Users).Returns(mockSet.Object);
            emock.Setup(x => x.SetModified(It.IsAny<User>())).Callback<object>((object o) =>
            {
                Assert.AreEqual("Testuser", ((User)o).Name);
            });

            var ctxmock = new Mock<HttpContextBase>();
            var reqmock = new Mock<HttpRequestBase>();

            var srvvars = new NameValueCollection()
            {
                { "REMOTE_ADDR", "10.13.37.42" }
            };

            reqmock.Setup(x => x.ServerVariables).Returns(srvvars);

            ctxmock.Setup(x => x.Request).Returns(reqmock.Object);

            PokefansMembershipProvider pmp = new PokefansMembershipProvider(emock.Object, ctxmock.Object);

            var result = pmp.ValidateUser("test@example.com", "123456");

            Assert.IsFalse(result);
        }

        [Test]
        public void ValidateUserInvalidUserEmailTest()
        {
            var data = new List<User>
            {
                new User() { id = 0, Name = "Testuser", Email = "test@example.com", Salt = "yAuUlCrmGWnxI/9gPA8MMM5p3eSOkTgKAgEOS7+RsKk=", Password = "1cuYOl6iGwC9nJnPmI6sHfzNEww2ZesPY0ljsoxK5S4="}
            }.AsQueryable();

            var emock = new Mock<Entities>();
            var mockSet = new Mock<DbSet<User>>();

            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());


            emock.Setup(x => x.Users).Returns(mockSet.Object);
            emock.Setup(x => x.SetModified(It.IsAny<User>())).Callback<object>((object o) =>
            {
                Assert.AreEqual("Testuser", ((User)o).Name);
            });

            var ctxmock = new Mock<HttpContextBase>();
            var reqmock = new Mock<HttpRequestBase>();

            var srvvars = new NameValueCollection()
            {
                { "REMOTE_ADDR", "10.13.37.42" }
            };

            reqmock.Setup(x => x.ServerVariables).Returns(srvvars);

            ctxmock.Setup(x => x.Request).Returns(reqmock.Object);

            PokefansMembershipProvider pmp = new PokefansMembershipProvider(emock.Object, ctxmock.Object);

            var result = pmp.ValidateUser("test2@example.com", "123456");

            Assert.IsFalse(result);
        }

        // Warning: The following Tests need a database connection... maybe. 
        // Depends on how "funny" the developers at microsoft were back then when they implemented the first MembershipProvider.
        // The Problem is that the Creation of a MembershipUser features an implicit creation of the bare MembershipProvider, which
        // needs (of course!) Database Access and an HTTP context. First is already mocked away and the second is completely irrelevant
        // for these tests. But whatever.
        [Test]
        public void GetUserByEmailTest()
        {
            var data = new List<User>
            {
                new User() { id = 0, Name = "Testuser", Email = "test@example.com", Status = 1, Registered = DateTime.Now  }
            }.AsQueryable();

            var emock = new Mock<Entities>();
            var mockSet = new Mock<DbSet<User>>();

            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());


            emock.Setup(x => x.Users).Returns(mockSet.Object);

            var ctxmock = new Mock<HttpContextBase>();
            var reqmock = new Mock<HttpRequestBase>();

            var srvvars = new NameValueCollection()
            {
                { "REMOTE_ADDR", "10.13.37.42" }
            };

            reqmock.Setup(x => x.ServerVariables).Returns(srvvars);

            ctxmock.Setup(x => x.Request).Returns(reqmock.Object);

            HttpContext.Current = new HttpContext(new HttpRequest("", "http://tempuri.com", ""), new HttpResponse(StreamWriter.Null));

            PokefansMembershipProvider pmp = new PokefansMembershipProvider(emock.Object, ctxmock.Object);

            var user = pmp.GetUser("test@example.com", false);
            Assert.IsNotNull(user);
        }

        [Test]
        public void GetUserByIdTest()
        {
            var data = new List<User>
            {
                new User() { id = 0, Name = "Testuser", Email = "test@example.com", Status = 1, Registered = DateTime.Now  }
            }.AsQueryable();

            var emock = new Mock<Entities>();
            var mockSet = new Mock<DbSet<User>>();

            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());


            emock.Setup(x => x.Users).Returns(mockSet.Object);

            var ctxmock = new Mock<HttpContextBase>();
            var reqmock = new Mock<HttpRequestBase>();

            var srvvars = new NameValueCollection()
            {
                { "REMOTE_ADDR", "10.13.37.42" }
            };

            reqmock.Setup(x => x.ServerVariables).Returns(srvvars);

            ctxmock.Setup(x => x.Request).Returns(reqmock.Object);

            HttpContext.Current = new HttpContext(new HttpRequest("", "http://tempuri.com", ""), new HttpResponse(StreamWriter.Null));

            PokefansMembershipProvider pmp = new PokefansMembershipProvider(emock.Object, ctxmock.Object);

            var user = pmp.GetUser(0, false);
            Assert.IsNotNull(user);
        }

    }
}

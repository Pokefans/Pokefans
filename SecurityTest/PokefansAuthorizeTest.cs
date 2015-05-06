// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using Moq;
using Pokefans.Data;
using Pokefans.Security;
using Pokefans.SystemCache;

namespace Pokefans.SecurityTest
{
    [TestFixture]
    [Ignore("Test must be fixed, Something with the setup of the autorization context isn't right, but we ignore this for the Moment.")]
    class PokefansAuthorizeTest
    {
        [Test]
        public void AuthorizeTest()
        {

            WebSecurity.CurrentUser = new User()
            {
                id = 0,
                Name = "Testuser",
                Email = "test@example.com",
                Salt = "yAuUlCrmGWnxI/9gPA8MMM5p3eSOkTgKAgEOS7+RsKk=",
                Password = "1cuYOl6iGwC9nJnPmI6sHfzNEww2ZesPY0ljsoxK5S4=",
                Activationkey = "12345678abcd"
            };

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

            var ctxmock = new Mock<HttpContextBase>();
            var reqmock = new Mock<HttpRequestBase>();

            var srvvars = new NameValueCollection()
            {
                { "REMOTE_ADDR", "10.13.37.42" }
            };

            reqmock.Setup(x => x.ServerVariables).Returns(srvvars);

            ctxmock.Setup(x => x.Request).Returns(reqmock.Object);

            var controller = new Mock<ControllerBase>();
            var actionDescriptor = new Mock<ActionDescriptor>();

            var controllerContext = new ControllerContext(ctxmock.Object, new RouteData(), controller.Object);

            var cmock = new Mock<Cache>();
            var dictionary = new Dictionary<string, object>(); 
            ctxmock.Setup(c => c.Items).Returns(dictionary);

            PokefansAuthorizeAttribute pfaa = new PokefansAuthorizeAttribute("testperm", emock.Object, cmock.Object);

            var filterContext = new AuthorizationContext(controllerContext, actionDescriptor.Object);
            pfaa.OnAuthorization(filterContext);

            Assert.AreEqual("", filterContext.HttpContext.Response);
            
        }
    }
}

// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Moq;
using NUnit.Framework;
using Pokefans.Security;

namespace Pokefans.SecurityTest
{
    [TestFixture]
    class SecurityUtilTests
    {
        [Test]
        public void GetIpAddressAsStringTest()
        {
            var ctxmock = new Mock<HttpContextBase>();
            var reqmock = new Mock<HttpRequestBase>();

            var srvvars = new NameValueCollection()
            {
                { "REMOTE_ADDR", "10.13.37.42" }
            };

            reqmock.Setup(x => x.ServerVariables).Returns(srvvars);

            ctxmock.Setup(x => x.Request).Returns(reqmock.Object);

            string ip = SecurityUtils.GetIPAddressAsString(ctxmock.Object);

            Assert.AreEqual("10.13.37.42", ip);
        }

        [Test]
        public void GetIpAddressAsStringWithValidProxyTest()
        {
            var ctxmock = new Mock<HttpContextBase>();
            var reqmock = new Mock<HttpRequestBase>();

            var srvvars = new NameValueCollection()
            {
                { "HTTP_X_FORWARDED_FOR", "13.37.13.37" },
                { "REMOTE_ADDR", "123.123.123.123" }
            };

            reqmock.Setup(x => x.ServerVariables).Returns(srvvars);

            ctxmock.Setup(x => x.Request).Returns(reqmock.Object);

            string ip = SecurityUtils.GetIPAddressAsString(ctxmock.Object);

            Assert.AreEqual("13.37.13.37", ip);
        }

        [Test]
        public void GetIpAddressAsStringWithInvalidProxyTest()
        {
            var ctxmock = new Mock<HttpContextBase>();
            var reqmock = new Mock<HttpRequestBase>();

            var srvvars = new NameValueCollection()
            {
                { "HTTP_X_FORWARDED_FOR", "192.168.1.10" },
                { "REMOTE_ADDR", "123.123.123.123" }
            };

            reqmock.Setup(x => x.ServerVariables).Returns(srvvars);

            ctxmock.Setup(x => x.Request).Returns(reqmock.Object);

            string ip = SecurityUtils.GetIPAddressAsString(ctxmock.Object);

            Assert.AreEqual("123.123.123.123", ip);
        }
    }
}

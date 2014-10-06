using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsQuery.Utility;
using Cuttler.Api.Modules;
using Cuttler.DataAccess;
using Cuttler.Entities;
using Moq;
using Nancy;
using Nancy.Responses.Negotiation;
using Nancy.Testing;
using NUnit.Framework;
using Browser = Nancy.Testing.Browser;

namespace Cuttler.Api.Tests.Modules
{
    [TestFixture]
    class UsersTests
    {
        [Test]
        public void Should_return_the_user_when_login_with_credentials()
        {
            var mock = GetUserServiceLoginMock();

            var browser = new Browser(cfg =>
            {
                cfg.Module<UsersModule>();
                cfg.Dependency(mock.Object);
            });

            // When
            var result = browser.Post("/users/login", with =>
            {
                with.HttpRequest();
                with.Accept(new MediaRange("application/json"));
                with.FormValue("username", "user");
                with.FormValue("password", "pass");
            });

            // Then
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [Test]
        public void Should_return_error_when_login_with_wrong_credentials()
        {
            var mock = GetUserServiceLoginMock();

            var browser = new Browser(cfg =>
            {
                cfg.Module<UsersModule>();
                cfg.Dependency(mock.Object);
            });

            // When
            var result = browser.Post("/users/login", with =>
            {
                with.HttpRequest();
                with.Accept(new MediaRange("application/json"));
                with.FormValue("username", "wrong");
                with.FormValue("password", "wrong");
            });

            // Then
            Assert.AreEqual(HttpStatusCode.Forbidden, result.StatusCode);
        }

        private static Mock<IUserService> GetUserServiceLoginMock()
        {
            var mock = new Mock<IUserService>();
            mock.Setup(svc => svc.Login(It.IsAny<string>(), It.IsAny<string>())).
                Returns<User>(null);
            mock.Setup(svc => svc.Login("user", "pass"))
                .Returns(new User() {UserName = "user"});
            return mock;
        }
    }
}

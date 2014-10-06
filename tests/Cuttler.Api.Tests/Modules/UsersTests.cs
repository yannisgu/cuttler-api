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
using Nancy.Routing.Constraints;
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

            var boot = new TestBootstrapper(cfg =>
            {
                cfg.Module<UsersModule>();
                cfg.Dependency(mock.Object);
            });
            var browser = new Browser(boot);

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
            Assert.AreEqual(result.Context.CurrentUser.UserName, "user");
        }

        [Test]
        public void Should_return_error_when_login_with_wrong_credentials()
        {
            var mock = GetUserServiceLoginMock();
            var bootstraper = new TestBootstrapper(cfg =>
            {
                cfg.Module<UsersModule>();
                cfg.Dependency(mock.Object);
            });
            var browser = new Browser(bootstraper);

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

        [Test]
        public void Should_return_current_user_when_loggedin()
        {
            var mock = GetUserServiceLoginMock();
            var bootstraper = new TestBootstrapper(cfg =>
            {
                cfg.Module<UsersModule>();
                cfg.Dependency(mock.Object);
            });
            var browser = new Browser(bootstraper);

            Login(browser);
            var response = browser.Get("/users/current", with =>
            {
                with.HttpRequest();
                with.Accept(new MediaRange("application/json"));
            });

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

            var user = response.Get<User>();
            Assert.NotNull(user);
            Assert.AreEqual(user.UserName, "user");
        }

        private BrowserResponse Login(Browser browser)
        {
            return browser.Post("/users/login", with =>
            {
                with.HttpRequest();
                with.Accept(new MediaRange("application/json"));
                with.FormValue("username", "user");
                with.FormValue("password", "pass");
            });
        }

        private static Mock<IUserService> GetUserServiceLoginMock()
        {
            var testUser = new User() {UserName = "user"};

            var mock = new Mock<IUserService>();
            mock.Setup(svc => svc.Login(It.IsAny<string>(), It.IsAny<string>())).
                ReturnsAsync(default(User));
            mock.Setup(svc => svc.Login("user", "pass"))
                .ReturnsAsync(testUser);

            mock.Setup(svc => svc.GetUser("user")).
                ReturnsAsync(testUser);
            return mock;
        }
    }
}

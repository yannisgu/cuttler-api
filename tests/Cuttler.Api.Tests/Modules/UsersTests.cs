﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsQuery.Utility;
using Cuttler.Api.Modules;
using Cuttler.Api.ViewModels;
using Cuttler.DataAccess;
using Cuttler.Entities;
using Cuttler.TestFramework;
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
    class UsersTests : BaseModuleTests
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
            var result = Login(browser);

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
            var result = Login(browser, "wrong", "wrong");

            // Then
            Assert.AreEqual(HttpStatusCode.Forbidden, result.StatusCode);
        }

        [Test]
        public void Should_add_user_login_when_register()
        {
            var mock = GetUserServiceLoginMock();
            mock.Setup(svc => svc.AddUser(It.IsAny<User>())).Returns(Task.Run(() => { }));
            var bootstraper = new TestBootstrapper(cfg =>
            {
                cfg.Module<UsersModule>();
                cfg.Dependency(mock.Object);
            });
            var browser = new Browser(bootstraper);

            var email = "yannis@yannis.org";
            
            //when 
            var newUser = new NewUserViewModel()
            {
                UserName = "yannisgu",
                Email = email,
                Country = "Switzerland",
                Location = "Courtaman",
                Password = "Pass@word1",
                Street = "Schulweg 28",
                Zip = "1791"
            };
            var result = browser.Post("/users/register", with =>
            {
                with.HttpRequest();
                with.Accept(new MediaRange("application/json"));
                with.JsonBody(newUser);
            });

            //Should
            mock.Verify(svc => svc.AddUser(It.Is<User>(_ =>
                _.UserName == newUser.UserName &&
                _.Country == newUser.Country &&
                _.Location == newUser.Location &&
                _.Street == newUser.Street &&
                _.Zip == newUser.Zip
                )));

            mock.Verify(_ => _.AddEmail(It.Is<User>(u => u.UserName == newUser.UserName), email));

            mock.Verify(svc => svc.AddLogin(It.Is<User>(_ => 
                _.UserName == newUser.UserName), newUser.Password, false));

            var user = result.Get<User>();
            Assert.AreEqual(user.UserName, newUser.UserName);
            Assert.AreEqual(user.Country, newUser.Country);
            Assert.AreEqual(user.Location, newUser.Location);
            Assert.AreEqual(user.Street, newUser.Street);
            Assert.AreEqual(user.Zip, newUser.Zip);

        }

        [Test]
        public void Should_update_user_when_post_user()
        {
            var mock = GetUserServiceLoginMock();

            var bootstraper = new TestBootstrapper(cfg =>
            {
                cfg.Module<UsersModule>();
                cfg.Dependency(mock.Object);
            });
            var browser = new Browser(bootstraper);

            Login(browser);

            var newUser = new User() {UserName = "newUser"};

            browser.Post("/users/current", with =>
            {
                with.HttpRequest();
                with.Accept(new MediaRange("application/json"));
                with.JsonBody(newUser);
            });

            //Should
            mock.Verify(svc => svc.UpdateUser(It.Is<User>(_ => _.UserName == "newUser" && _.Id==new Guid("{DD0BAA48-9D5F-4167-8672-632D4EE1D27F}"))));
        }

        [Test]
        public void Should_block_when_anonymous_update_user()
        {
            var mock = GetUserServiceLoginMock();

            var bootstraper = new TestBootstrapper(cfg =>
            {
                cfg.Module<UsersModule>();
                cfg.Dependency(mock.Object);
            });
            var browser = new Browser(bootstraper);
           var result =  browser.Post("/users/current", with =>
            {
                with.HttpRequest();
                with.Accept(new MediaRange("application/json"));
                with.JsonBody(new User() {UserName = "dummy"});
            });

            Assert.AreEqual(result.StatusCode, HttpStatusCode.Forbidden);

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

    }

    
}

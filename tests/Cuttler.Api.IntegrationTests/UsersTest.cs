using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Cuttler.Api.ViewModels;
using Cuttler.DataAccess;
using Cuttler.DataAccess.Implementation;
using Cuttler.Entities;
using Cuttler.TestFramework;
using Nancy;
using Nancy.Responses.Negotiation;
using Nancy.Testing;
using Ninject;
using NUnit.Framework;

namespace Cuttler.Api.IntegrationTests
{
    [TestFixture]
    public class UsersTest
    {
        private Bootstrapper bootstrapper;
        private Browser browser;

        [TestFixtureSetUp]
        public void SetUp()
        {

            bootstrapper = new Bootstrapper();
            browser = new Browser(bootstrapper);

            Common.Cleanup(bootstrapper.Kernel);   
        }

        [Test]
        public async Task Should_add_user_when_register()
        {
            var userSvc = bootstrapper.Kernel.Get<IUserService>();
            var ctx = bootstrapper.Kernel.Get<IDataContext>();
            var user = new NewUserViewModel()
            {
                UserName = "demouser",
                Password = "demopassword",
                Email = "demo@demo.org"
            };
            var response = browser.Post("/users/register", Common.With(with => with.JsonBody(user)));

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.NotNull(await userSvc.GetUser("demouser"));
            // Email not yet verfied
            Assert.Null(await userSvc.Login("demouser", "demopassword")); 
            var email = ctx.Emails.FirstOrDefault(e => e.Mail == user.Email);
            Assert.NotNull(email);

            browser.Post(string.Format("/users/verifyEmail/{0}", email.VerifyCode));

            
            var loginUser = browser.Post("/users/login",Common.With(with =>
            {
                with.FormValue("username", "demouser");
                with.FormValue("password", "demopassword");
            })).Get<UserViewModel>();
            Assert.NotNull(loginUser);
            Assert.AreEqual("demouser", loginUser.UserName);

            var currentUser = browser.Get("/users/current", Common.With()).
                Get<UserViewModel>();
            Assert.NotNull(currentUser);
            Assert.AreEqual("demouser", currentUser.UserName);

            currentUser.Street = "update street";
            var updatedUser = browser.Post("/users/current", Common.With(with =>
            {
                with.JsonBody(currentUser);
            })).Get<UserViewModel>();
            Assert.AreEqual("demouser" , updatedUser.UserName);
            Assert.AreEqual("update street", updatedUser.Street);


        }

        public void Should_update_address_when_update_current_user()
        {
            var userResponse = browser.Get("/users/current", Common.With());
            var user = userResponse.Get<User>();
            //var user = new UserViewModel()
            //{
            //    Password = "demopass",

            //}
            //browser.Post("/users/register")   
        }
    }
}

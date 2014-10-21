using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cuttler.Api.ViewModels;
using Cuttler.DataAccess;
using Cuttler.DataAccess.Implementation;
using Cuttler.Entities;
using Cuttler.TestFramework;
using Moq;
using Nancy;
using Nancy.Testing;
using Ninject;
using NUnit.Framework;

namespace Cuttler.Api.IntegrationTests
{
    [TestFixture]
    public class OctopusTenantTests
    {
        [Test]
        public async Task BigOctopusTest()
        {
            var bootstrapper = new Bootstrapper();
            var browser = new Browser(bootstrapper);
            Common.Cleanup(bootstrapper.Kernel);
            var messageServiceMock = new Mock<IMessageService>();
            bootstrapper.Kernel.Rebind<IMessageService>().ToConstant(messageServiceMock.Object);
            var ctx = bootstrapper.Kernel.Get<DataContext>();
            var userSvc = bootstrapper.Kernel.Get<IUserService>();
            var user = new NewUserViewModel()
            {
                UserName = "demouser",
                Password = "demopassword",
                Email = "bla@bla.ch"
            };
            var response = browser.Post("/users/register", Common.With(with => with.JsonBody(user)));
            var email = ctx.Emails.First(e => e.Mail == user.Email);
            browser.Post(string.Format("/users/verifyEmail/{0}", email.VerifyCode));

            var loginUser = browser.Post("/users/login", Common.With(with =>
            {
                with.FormValue("username", "demouser");
                with.FormValue("password", "demopassword");
            })).Get<UserViewModel>();

            var tenant = new OctopusTenantViewModel()
            {
                Name = "demo tenant"
            };
            browser.Post("/tenants/octopus", Common.With(with =>
            {
                with.JsonBody(tenant);
            }));

            var tenants = browser.Get("/tenants/octopus", Common.With()).
                Get<IEnumerable<OctopusTenantViewModel>>();

            var tenantsCollection = tenants as OctopusTenantViewModel[] ?? tenants.ToArray();
            Assert.AreEqual(1, tenantsCollection.Count());
            Assert.AreEqual("demo tenant", tenantsCollection.First().Name);
            Assert.NotNull(tenantsCollection.First().Id);
            messageServiceMock.Verify(_ => 
                _.Publish(It.Is<Message<Tenant<OctopusTenant>>>(
                    i => i.Action == "Create" && i.Pack.Name == "demo tenant")));

            var updateTenant = new OctopusTenantViewModel()
            {
                Name = "changed tenant"
            };
            var updateResponse = browser.Post("/tenants/octopus/" + tenantsCollection.First().Id,
                Common.With(context => context.JsonBody(updateTenant)));

            Assert.AreEqual(HttpStatusCode.OK, updateResponse.StatusCode);

        }
    }
}

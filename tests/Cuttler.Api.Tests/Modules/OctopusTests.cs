using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Cuttler.Api.Modules;
using Cuttler.DataAccess;
using Cuttler.Entities;
using Moq;
using Nancy;
using Nancy.Responses.Negotiation;
using Nancy.Testing;
using NUnit.Framework;

namespace Cuttler.Api.Tests.Modules
{
    [TestFixture]
    public class OctopusTests  : BaseModuleTests
    {
        private const string TEST_BACKUP_ID = "371DE0C0-817B-47E6-9A59-6C6090071853";
        private const string TEST_TENANT_ID = "99A66035-CDDA-454E-BD76-504BE158F962";

        private static readonly Guid TestBackupGuid = new Guid(TEST_BACKUP_ID);
        private static readonly Guid TestTenantGuid = new Guid(TEST_TENANT_ID);




        [Test]
        public void Should_return_all_my_tenants()
        {
            var tenantMock = new Mock<ITenantService<OctopusTenant>>();
            var browser = GetLoggedInBrowser(tenantMock);

            var response = browser.Get("/tenants/octopus", With());

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            tenantMock.Verify(svc => svc.GetByUser(testUser.Id));
        }


        [Test]
        public void Should_filter_for_backup_and_tenant()
        {
            var tenantMock = new Mock<ITenantService<OctopusTenant>>();

            tenantMock.Setup(_ => _.GetBackup(new Guid(TEST_BACKUP_ID))).ReturnsAsync(new Backup() { TenantId = new Guid(TEST_TENANT_ID) });
            
            tenantMock.Setup(svc => svc.GetBackupStream(new Guid(TEST_BACKUP_ID))).ReturnsAsync(new MemoryStream());
            var browser = GetLoggedInBrowser(tenantMock);
            

            var response = browser.Get("/tenants/octopus/"+ TEST_TENANT_ID + "/backups/" + TEST_BACKUP_ID, With());
            // Read stream
            var body = response.Body.AsString();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            tenantMock.Verify(svc => svc.GetBackupStream(new Guid(TEST_BACKUP_ID)));
        }


        [Test]
        public void Should_forbidd_when_get_not_matching_backup_tenant()
        {
            var tenantMock = new Mock<ITenantService<OctopusTenant>>();
            Guid backupGuid = Guid.NewGuid();
            tenantMock.Setup(_ => _.GetBackup(backupGuid)).ReturnsAsync(new Backup() { TenantId = Guid.NewGuid() });
            var browser = GetLoggedInBrowser(tenantMock);


            var response = browser.Get("/tenants/octopus/" + TEST_TENANT_ID + "/backups/" + backupGuid, With());

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }


        [Test]
        [TestCase("/tenants/octopus")]
        [TestCase("/tenants/octopus/"+TEST_TENANT_ID+"/backups")]
        public void Should_return_forbidden_when_calling_tenant_service(string url)
        {
            var tenantMock = new Mock<ITenantService<OctopusTenant>>();
            var browser = GetBrowser(tenantMock);

            var response = browser.Get(url, With());

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        private Browser GetLoggedInBrowser(Mock<ITenantService<OctopusTenant>> tenantMock)
        {
            tenantMock.Setup(_ => _.Get(TestTenantGuid)).ReturnsAsync(new OctopusTenant() { Id = TestTenantGuid });
            tenantMock.Setup(_ => _.IsAdmin(It.Is<Tenant<OctopusTenant>>(t => t.Id == TestTenantGuid), TestIserGuid)).Returns(true);
            var browser = GetBrowser(tenantMock);
            Login(browser);
            return browser;
        }

        private Browser GetBrowser(Mock<ITenantService<OctopusTenant>> tenantMock)
        {
            var userService = GetUserServiceLoginMock();
            tenantMock.Setup(svc => svc.GetByUser(It.IsAny<Guid>())).ReturnsAsync(new List<OctopusTenant>());
            var bootstraper = new TestBootstrapper(cfg =>
            {
                cfg.Module<UsersModule>();
                cfg.Module<OctopusModule>();
                cfg.Dependency(tenantMock.Object);
                cfg.Dependency(userService.Object);
            });
            var browser = new Browser(bootstraper);
            return browser;
        }

        private Action<BrowserContext> With(Action<BrowserContext> otherWiths = null)
        {
            return with =>
            {
                with.HttpRequest();
                with.Accept(new MediaRange("application/json"));
                if (otherWiths != null) otherWiths(with);
            };
        }
    }

}

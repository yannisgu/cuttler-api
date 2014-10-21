using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cuttler.DataAccess.Implementation;
using Cuttler.Entities;
using Moq;
using NUnit.Framework;

namespace Cuttler.DataAccess.Tests.Services
{
    [TestFixture]
    public class OctopusTenantTests
    {
        [Test]
        public async Task Should_publish_and_add_tenant_when_add_through_api()
        {
            var messageServiceMock = new Mock<IMessageService>();
            var dataContextMock = new Mock<DataContext>();
            var octopusDbSetMock = new Mock<DbSet<OctopusTenant>>();
            dataContextMock.SetupGet(_ => _.OctopusTenants).Returns(octopusDbSetMock.Object);
            var tenantService = new OctopusTenantService(dataContextMock.Object, messageServiceMock.Object, null);

            var tenant = new OctopusTenant()
            {
                MainUrl = "https://bla.mycuttler.com",
                Name = "bla"
            };

            // When
            await tenantService.Add(tenant);

            // Should
            octopusDbSetMock.Verify(_ => _.Add(It.Is<OctopusTenant>(t => t.Name == "bla")));
            dataContextMock.Verify(_ => _.SaveChangesAsync());
            messageServiceMock.Verify(_ => _.Publish(It.Is<Message<Tenant<OctopusTenant>>>
                (m => m.Action == "Create" && m.Pack == tenant)));
        }
    }
}

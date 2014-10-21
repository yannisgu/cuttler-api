using System;
using Cuttler.DataAccess;
using Cuttler.DataAccess.Implementation;
using Nancy.Responses.Negotiation;
using Nancy.Testing;
using Ninject;

namespace Cuttler.Api.IntegrationTests
{
    public class Common
    {
        public static void Cleanup(IKernel kernel)
        {
            using (var ctx = (DataContext) kernel.Get<IDataContext>())
            {
                ctx.Database.ExecuteSqlCommand("delete  from  dbo.Emails");
                ctx.Database.ExecuteSqlCommand("delete  from  dbo.Logins");
                ctx.Database.ExecuteSqlCommand("delete  from  dbo.Users");
                ctx.Database.ExecuteSqlCommand("delete  from  dbo.Backups");
                ctx.Database.ExecuteSqlCommand("delete  from  dbo.OctopusTenants");
            }
        }

        public static Action<BrowserContext> With( Action<BrowserContext> otherWiths = null)
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
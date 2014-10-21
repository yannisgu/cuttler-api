using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Cuttler.DataAccess;
using Nancy;
using Nancy.Bootstrapper;
using Ninject.Activation;

namespace Cuttler.Api
{
    public static class BootsrapperExtension
    {
        public static void WithSessionAuthentification(this IPipelines _this, IUserService userService)
        {
            _this.BeforeRequest.AddItemToStartOfPipeline(async (ctx, cancel) =>
            {
                var userName = ctx.Request.Session["user"] as string;
                if (userName != null)
                {
                    var user = await userService.GetUser(userName);
                    if (user != null)
                    {
                        ctx.CurrentUser = new NancyUser(user);
                    }
                }
                return null;
            });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Cuttler.DataAccess;
using Cuttler.Entities;
using Nancy;
using Nancy.Responses.Negotiation;

namespace Cuttler.Api.Modules
{
    public class UsersModule : NancyModule
    {
        private readonly IUserService userService;

        public UsersModule(IUserService userService)
            : base("/users")
        {
            this.userService = userService;
            Post["/login", true] = Login;
            Get["/current"] = ctx =>
            {
                var user = Context.CurrentUser as NancyUser;
                if (user != null)
                {
                    return user.User;
                }
                else
                {
                    throw new Exception("User should be logged.");
                }
            };
        }

        private async Task<dynamic> Login(dynamic paramters, CancellationToken cancel)
        {
            User user;
            object returnObject = user= await userService.Login(Context.Request.Form["username"], Context.Request.Form["password"]);
            var statusCode = HttpStatusCode.OK;
            if (returnObject == null)
            {
                statusCode = HttpStatusCode.Forbidden;
                returnObject = new Error() {Message = "The provided username or password is wrong"};
            }
            else
            {
                Context.Request.Session["user"] = user.UserName;
                this.Context.CurrentUser = new NancyUser(user);
            }


            return Negotiate.WithModel(returnObject)
                .WithStatusCode(statusCode);
        }
    }

    internal class Error
    {
        public string Message { get; set; }
    }
}
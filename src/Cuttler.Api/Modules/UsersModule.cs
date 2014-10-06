using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Cuttler.DataAccess;
using Cuttler.Entities;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using Nancy.Security;

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
            Post["/register", true] = Register;
            Get["/current"] = ctx =>
            {
                var user = Context.CurrentUser as NancyUser;
                if (user != null)
                {
                    return user.User;
                }
                else
                {
                    return Negotiate.WithModel(new Error("Not logged in.")).
                        WithStatusCode(HttpStatusCode.Forbidden);
                }
            };

        }

        private async Task<object> Register(dynamic parameters, CancellationToken cancel)
        {
            var newUser = this.Bind<User>();
            await userService.AddUser(newUser);
            await userService.AddLogin(newUser, Request.Form["password"], enabled: false);
            return newUser;
        }
    
        private async Task<dynamic> Login(dynamic paramters, CancellationToken cancel)
        {
            User user;
            object returnObject = user= await userService.Login(Context.Request.Form["username"], Context.Request.Form["password"]);
            var statusCode = HttpStatusCode.OK;
            if (returnObject == null)
            {
                statusCode = HttpStatusCode.Forbidden;
                returnObject = new Error("The provided username or password is wrong");
            }
            else
            {
                Context.Request.Session["user"] = user.UserName;
                Context.CurrentUser = new NancyUser(user);
            }


            return Negotiate.WithModel(returnObject)
                .WithStatusCode(statusCode);
        }
    }

    internal class Error
    {
        public Error(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Cuttler.Api.ViewModels;
using Cuttler.DataAccess;
using Cuttler.Entities;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using Nancy.Security;

namespace Cuttler.Api.Modules
{
    public class UsersModule : BaseModule
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
                    return AccesDenied();
                }
            };
            Post["/current", true] = Update;

        }

        private async Task<object> Register(dynamic parameters, CancellationToken cancel)
        {
            var newUser = this.Bind<UserViewModel>();
            var userObject = newUser.AsUser();
            await userService.AddUser(userObject);
            await userService.AddEmail(userObject, newUser.Email);
            await userService.AddLogin(userObject, newUser.Password, enabled: false);
            return newUser;
        }

        private async Task<object> Update(dynamic paramters, CancellationToken canel)
        {
            var newUser = this.Bind<UserViewModel>().AsUser();
            var user = Context.CurrentUser as NancyUser;
            if (user != null)
            {
                newUser.Id = user.User.Id;
                await userService.UpdateUser(newUser);
                return newUser;
            }
            else
            {
                return AccesDenied();
            }
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cuttler.DataAccess;
using Cuttler.Entities;
using Nancy;

namespace Cuttler.Api.Modules
{
    public class UsersModule : NancyModule
    {
        private readonly IUserService userService;

        public UsersModule(IUserService userService)
            : base("/users")
        {
            this.userService = userService;
            Post["/login"] = Login;
        }

        private dynamic Login(dynamic paramters)
        {
            User user;
            object returnObject = user= userService.Login(Context.Request.Form.username.Value, Context.Request.Form.password.Value);
            var statusCode = HttpStatusCode.OK;
            if (returnObject == null)
            {
                statusCode = HttpStatusCode.Forbidden;
                returnObject = new Error() {Message = "The provided username or password is wrong"};
            }
            else
            {
                //this.Context.CurrentUser = user
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
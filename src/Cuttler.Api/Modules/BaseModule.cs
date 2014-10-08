using Cuttler.Entities;
using Nancy;
using Nancy.Responses.Negotiation;

namespace Cuttler.Api.Modules
{
    public class BaseModule :NancyModule
    {
        public BaseModule() : base()
        {
            
        }

        public BaseModule(string url) : base(url)
        {
            
        }

        public User User
        {
            get
            {
                var user = this.Context.CurrentUser as NancyUser;
                return user != null ? user.User : null;
            }
        }

        protected Negotiator AccesDenied()
        {
            return Negotiate.WithModel(new Error("Acces denied.")).
                WithStatusCode(HttpStatusCode.Forbidden);
        }
    }
}
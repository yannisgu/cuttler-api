using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using Cuttler.Entities;
using Nancy.Security;

namespace Cuttler.Api
{
    public class NancyUser : IUserIdentity
    {
        public NancyUser(User user)
        {
            User = user;
        }

        public string UserName
        {
            get { return User.UserName; }
        }

        public IEnumerable<string> Claims
        {
            get { return new List<string>(); }
        }

        public User User { get; private set; }
    }
}
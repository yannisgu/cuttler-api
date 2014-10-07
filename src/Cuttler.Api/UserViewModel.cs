using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cuttler.Entities
{
    public class UserViewModel : User
    {
        public string Password { get; set; }

        [JsonIgnore]
        public override Guid Id { get; set; }

        [JsonIgnore]
        public override IEnumerable<Login> Logins { get; set; }
    }
}
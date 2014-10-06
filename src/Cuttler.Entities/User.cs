using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace Cuttler.Entities
{
    public class User
    {
        public string UserName { get; set; }
        public IEnumerable<Login> Logins { get; set; }
    }
}

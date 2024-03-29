﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace Cuttler.Entities
{
    public class User
    {
        public virtual Guid Id { get; set; }
               
        public virtual string UserName { get; set; }
               
        public virtual string Street { get; set; }
        public virtual string Zip { get; set; }
        public virtual string Location { get; set; }
               
        public virtual string Country { get; set; }

        public virtual List<Login> Logins { get; set; }
        public virtual ICollection<string> Emails { get; set; }

        public virtual List<OctopusTenant> OctopusTenants { get; set; } 
    }
}

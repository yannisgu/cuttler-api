using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Cuttler.Entities;

namespace Cuttler.DataAccess
{
    public interface IDataContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Email> Emails { get; set; }
        DbSet<Login> Logins { get; set; }
        DbSet<OctopusTenant> OctopusTenants { get; set; }
    }
}

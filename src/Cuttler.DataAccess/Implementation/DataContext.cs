using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cuttler.Entities;

namespace Cuttler.DataAccess.Implementation
{
    [DbConfigurationType(typeof(DbConfiguration))] 
    public class DataContext : DbContext ,IDataContext
    {
        public DataContext() : base("data")
        {
           
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Login> Logins { get; set; }
        public virtual DbSet<OctopusTenant> OctopusTenants { get; set; }
        public virtual  DbSet<Email> Emails { get; set; }
        public virtual DbSet<Backup> Backups { get; set; }
    }
}

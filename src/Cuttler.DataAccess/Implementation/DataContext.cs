using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cuttler.Entities;

namespace Cuttler.DataAccess.Implementation
{
    public class DataContext : DbContext ,IDataContext
    {
        public virtual DbSet<User> Users { get; set; }
    }
}

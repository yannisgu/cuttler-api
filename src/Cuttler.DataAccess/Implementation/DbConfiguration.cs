using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuttler.DataAccess.Implementation
{
    public class DbConfiguration : System.Data.Entity.DbConfiguration
    {
        public DbConfiguration()
            {
                SetExecutionStrategy("System.Data.SqlClient", () => new DefaultExecutionStrategy());
                SetDefaultConnectionFactory(new LocalDbConnectionFactory("v11.0"));
            }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cuttler.Entities;

namespace Cuttler.DataAccess.Implementation
{
    public class OctopusTenantService : TenantService<OctopusTenant>
    {
        public override Task Add(OctopusTenant viewModel)
        {
            throw new NotImplementedException();
        }

        public override Task Update(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> IsAdmin(Guid tenant, Guid user)
        {
            throw new NotImplementedException();
        }

        public override Task Delete(Guid guid)
        {
            throw new NotImplementedException();
        }

        public override Task<Stream> GetBackupStream(Guid backupId)
        {
            throw new NotImplementedException();
        }

        public override Task<Backup> GetBackups(Guid guid)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> MatchTenantBackup(Guid tenantId, Guid backupId)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<OctopusTenant>> Get(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}

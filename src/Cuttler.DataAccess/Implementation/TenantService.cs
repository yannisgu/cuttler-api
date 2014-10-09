using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cuttler.Entities;

namespace Cuttler.DataAccess.Implementation
{
    public abstract class TenantService<T> : ITenantService<T> where T : Tenant<T>
    {
        public abstract Task Add(T viewModel);
        public abstract Task Update(Guid id);
        public abstract Task<bool> IsAdmin(Guid tenant, Guid user);
        public abstract Task Delete(Guid guid);
        public abstract Task<Stream> GetBackupStream(Guid backupId);
        public abstract Task<Backup> GetBackups(Guid guid);
        public abstract Task<bool> MatchTenantBackup(Guid tenantId, Guid backupId);
        public abstract Task<IEnumerable<T>> Get(Guid id);
    }
}

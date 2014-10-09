using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cuttler.Entities;

namespace Cuttler.DataAccess
{
    public interface ITenantService<T> where T : Tenant
    {
        Task Add(T viewModel);
        Task Update(Guid id);
        Task<bool> IsAdmin(Guid tenant, Guid user);
        Task Delete(Guid guid);
        Task<Stream> GetBackupStream(Guid backupId);
        Task<Backup> GetBackups(Guid guid);
        Task<bool> MatchTenantBackup(Guid tenantId, Guid backupId);
        Task<IEnumerable<T>> Get(Guid id);
    }
}
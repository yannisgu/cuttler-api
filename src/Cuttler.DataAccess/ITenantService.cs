using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cuttler.Entities;

namespace Cuttler.DataAccess
{
    public interface ITenantService<T> where T : Tenant<T>
    {
        Task Add(T viewModel);
        Task Update(Guid id, T tenant);
        //Task<bool> IsAdmin(Guid tenant, Guid user);
        Task Delete(Guid guid);
        Task<Stream> GetBackupStream(Guid backupId);
        Task<Stream> GetBackupStream(Backup backup);
        Task<IEnumerable<Backup>> GetBackups(Guid tenantId);
        
        Task<T> Get(Guid id);
        Task<Backup> GetBackup(Guid backupId);

        Task<IEnumerable<T>> GetByUser(Guid userId);
        Task AddAdmin(Tenant<T> tenant, Guid id);
        bool IsAdmin(Tenant<T> tenant, Guid userId);
    }
}
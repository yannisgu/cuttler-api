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
        private readonly DataContext dataContext;
        private readonly IMessageService messageService;

        protected TenantService(DataContext dataContext,  IMessageService messageService)
        {
            this.dataContext = dataContext;
            this.messageService = messageService;
        }

        public async virtual Task Add(T tenant)
        {
            tenant.Id = Guid.NewGuid();
            tenant.Enabled = true;
            await AddPersistent(tenant);
            var createMessage = new Message<Tenant<T>>()
            {
                Action = "Create",
                Pack = tenant,
                Name = "Create tenant " + tenant.Name
            };
            await messageService.Publish(createMessage);
        }

        public async Task Update(Guid id, T tenant)
        {
            tenant.Id = id;
            await Update(tenant);
        }

        protected abstract Task Update(T tenant);

        public async virtual Task Delete(Guid tenantId)
        {
            var tenant = await Get(tenantId);
            await DeletePersistent(tenant);
            var createMessage = new Message<Tenant<T>>()
            {
                Action = "Delete",
                Pack = tenant,
                Name = "Delete tenant " + tenant.Name
            };
            await messageService.Publish(createMessage);
        }

        protected abstract Task AddPersistent(T tenant);
        
        public abstract Task<bool> IsAdmin(Guid tenant, Guid user);
        public abstract Task DeletePersistent(T tenant);

        public async Task<Stream> GetBackupStream(Guid backupId)
        {
            return await GetBackupStream(await GetBackup(backupId));
        }
        public abstract Task<Stream> GetBackupStream(Backup backup);

        public abstract Task<IEnumerable<Backup>> GetBackups(Guid guid);
        public abstract Task<T> Get(Guid id);
        public virtual Task<Backup> GetBackup(Guid backupId)
        {
            return dataContext.Backups.FindAsync(backupId);
        }

        public abstract Task<IEnumerable<T>> GetByUser(Guid userId);
        public async Task AddAdmin(Tenant<T> tenant, Guid id)
        {
            var user = dataContext.Users.FirstOrDefault(_ => _.Id == id);
            tenant.Admins = new List<User>() {user};
            await dataContext.SaveChangesAsync();
        }

        public bool IsAdmin(Tenant<T> tenant, Guid userId)
        {
            return tenant.Admins.Any(_ => _.Id == userId);
        }
    }
}

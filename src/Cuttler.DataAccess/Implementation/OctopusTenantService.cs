using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cuttler.Entities;

namespace Cuttler.DataAccess.Implementation
{
    public class OctopusTenantService : TenantService<OctopusTenant>
    {
        private readonly DataContext dataContext;
        private readonly IFileService fileService;

        public OctopusTenantService(DataContext dataContext, IMessageService messageService, IFileService fileService) : base(dataContext, messageService)
        {
            this.dataContext = dataContext;
            this.fileService = fileService;
        }

        protected async override  Task Update(OctopusTenant tenant)
        {
            dataContext.Entry(tenant).State = EntityState.Modified;
            await dataContext.SaveChangesAsync();
        }

        protected async override Task AddPersistent(OctopusTenant tenant)
        {
            dataContext.OctopusTenants.Add(tenant);
            await dataContext.SaveChangesAsync();
        }


        public async override Task<bool> IsAdmin(Guid tenant, Guid user)
        {
            return (await dataContext.OctopusTenants.FindAsync(tenant)).
                Admins.Any(_ => _.Id == user);
        }

        public override async Task DeletePersistent(OctopusTenant tenant)
        {
            tenant.Enabled = false;
            await dataContext.SaveChangesAsync();
        }

        public override Task<Stream> GetBackupStream(Backup backup)
        {
            return fileService.GetStreamAsync(backup.FilePath);
        }

        public override async Task<IEnumerable<Backup>> GetBackups(Guid guid)
        {
            return await dataContext.Backups.Where(_ => _.TenantId == guid).ToListAsync();
        }

        public async override Task<OctopusTenant> Get(Guid id)
        {
            return await dataContext.OctopusTenants.FindAsync(id);
        }

        public async override Task<IEnumerable<OctopusTenant>> GetByUser(Guid userId)
        {
            return await dataContext.OctopusTenants.
                Where(_ => _.Admins.Any(a => a.Id == userId)).ToListAsync();
        }
    }
}

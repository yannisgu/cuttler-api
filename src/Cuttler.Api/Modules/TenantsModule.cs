using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Cuttler.Entities;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;

namespace Cuttler.Api.Modules
{
    public abstract class TenantsModule<T> :BaseModule where T : Tenant
    {
        protected abstract ITenantService<T> TenantService { get; }

        protected TenantsModule(string type) :base("/tenants/" + type)
        {
            Post["/", true] = async (_, cancel) =>
            {
                var viewModel = this.Bind<T>();
                await TenantService.Add(viewModel);
                return viewModel;
            };

            Post["/{id}", true] = async (_, cancel) =>
            {
                var guid = new Guid(_.id);
                if (! await TenantService.IsAdmin(guid, User.Id))
                {
                    return AccesDenied();
                }
                var viewModel = this.Bind<T>();
                viewModel.Id = guid;
                await TenantService.Update(viewModel.Id);
                return viewModel;
            };

            Delete["/{id}", true] = async (_, cancel) =>
            {
                var guid = new Guid(_.id);
                if (!await TenantService.IsAdmin(guid, User.Id))
                {
                    return AccesDenied();
                }
                await TenantService.Delete(guid);
                return "";
            };

            Get["/{id}/backups", true] = async (_, cancel) =>
            {
                var guid = new Guid(_.id);

                if (!await TenantService.IsAdmin(guid, User.Id))
                {
                    return AccesDenied();
                }
                return await TenantService.GetBackups(guid);
                
            };

            Get["/{id}/backups/{backupId}"] = (_) =>
            {
                return new Nancy.Responses.StreamResponse(() =>
                {
                    var back=  TenantService.GetBackupStream(new Guid(_.backupId));
                    back.Wait();
                    return back.Result;
                }, "application/zip");
            };
        }
    }

    public class Tenant
    {
        public virtual Guid Id { get; set; }
    }

    public interface ITenantService

    {
    }

    public interface ITenantService<T> : ITenantService where T : Tenant
    {
        Task Add(T viewModel);
        Task Update(Guid id);
        Task<bool> IsAdmin(Guid tenant, Guid user);
        Task Delete(Guid guid);
        Task<Stream> GetBackupStream(Guid backupId);
        Task<Backup> GetBackups(Guid guid);
    }

    public class Backup
    {
    }
}
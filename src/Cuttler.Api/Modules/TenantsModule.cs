using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cuttler.DataAccess;
using Cuttler.Entities;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using Nancy.Security;

namespace Cuttler.Api.Modules
{
    public abstract class TenantsModule<T> :BaseModule where T : Tenant
    {
        protected abstract ITenantService<T> TenantService { get; }

        protected TenantsModule(string type) :base("/tenants/" + type)
        {
            this.RequiresAuthentication();

            Get["/", true] = async (_, cancel) => await TenantService.Get(User.Id);

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

            Get["/{id}/backups/{backupId}", true] = async (_, cancel) =>
            {
                var tenantId = new Guid(_.id);
                var backupId = new Guid(_.backupId);
                if (!await TenantService.IsAdmin(tenantId, User.Id) || !await TenantService.MatchTenantBackup(tenantId, backupId))
                {
                    return AccesDenied();
                }

                return new Nancy.Responses.StreamResponse(() =>
                {
                    var back = TenantService.GetBackupStream(backupId);
                    back.Wait();
                    return back.Result;
                }, "application/zip");
            };
        }
    }
}
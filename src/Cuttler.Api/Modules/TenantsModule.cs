using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cuttler.Api.ViewModels;
using Cuttler.DataAccess;
using Cuttler.Entities;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using Nancy.Security;

namespace Cuttler.Api.Modules
{
    public abstract class TenantsModule<TTeanant, TViewModel> :BaseModule where TTeanant : Tenant<TTeanant> where TViewModel : TenantViewModel<TTeanant>
    {
        protected abstract ITenantService<TTeanant> TenantService { get; }

        protected TenantsModule(string type) :base("/tenants/" + type)
        {
            this.RequiresAuthentication();

            Get["/", true] = async (_, cancel) =>
                (await TenantService.GetByUser(User.Id)).Select(GetViewModel);

            Post["/", true] = async (_, cancel) =>
            {
                var viewModel = this.Bind<TViewModel>();
                var model = viewModel.AsTenant();
                await TenantService.Add(model);
                await TenantService.AddAdmin(model, this.User.Id);
                return GetViewModel(model);
            };

            Post["/{id}", true] = async (_, cancel) =>
            {
                var guid = new Guid(_.id);
                var tenant = await TenantService.Get(guid);
                if (!(tenant.Admins.Any(u => u.Id  == User.Id)))
                {
                    return AccesDenied();
                }
                var viewModel = this.Bind<TViewModel>();
                viewModel.UpdateTenant(tenant);
                await TenantService.Update(guid, tenant);
                return GetViewModel(tenant);
            };

            Delete["/{id}", true] = async (_, cancel) =>
            {
                var guid = new Guid(_.id);
                var tenant = await TenantService.Get(guid);
                if (!(TenantService.IsAdmin(tenant, User.Id)))
                {
                    return AccesDenied();
                }
                await TenantService.Delete(guid);
                return "";
            };

            Get["/{id}/backups", true] = async (_, cancel) =>
            {
                var guid = new Guid(_.id);
                var tenant = await TenantService.Get(guid);
                if (!TenantService.IsAdmin(tenant, User.Id))
                {
                    return AccesDenied();
                }
                return await TenantService.GetBackups(guid);
                
            };

            Get["/{id}/backups/{backupId}", true] = async (_, cancel) =>
            {
                var tenantId = new Guid(_.id);
                var backupId = new Guid(_.backupId);
                var backup = await TenantService.GetBackup(backupId);
                var tenant = await TenantService.Get(tenantId);
                if (! TenantService.IsAdmin(tenant, User.Id) || backup.TenantId != tenantId)
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

        protected abstract TViewModel GetViewModel(TTeanant model);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cuttler.Entities;

namespace Cuttler.Api.ViewModels
{
    public class OctopusTenantViewModel : TenantViewModel<OctopusTenant>
    {
        public OctopusTenantViewModel() 
        { }

        public OctopusTenantViewModel(OctopusTenant model) : base(model)
        {
        }

        public override OctopusTenant AsTenant()
        {
            var tenant = new OctopusTenant();
            UpdateTenant(tenant);
            return tenant;
        }

        public override void UpdateTenant(OctopusTenant model)
        {
            base.UpdateTenant(model);
            model.Urls = Urls;
        }

        public IEnumerable<string> Urls { get; set; }
    }
}
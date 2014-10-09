using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cuttler.DataAccess;
using Cuttler.Entities;

namespace Cuttler.Api.Modules
{
    public class OctopusModule : TenantsModule<OctopusTenant>
    {
        private readonly ITenantService<OctopusTenant> tenantService;

        public OctopusModule(ITenantService<OctopusTenant> tenantService) : base("octopus")
        {
            this.tenantService = tenantService;
        }

        protected override ITenantService<OctopusTenant> TenantService
        {
            get { return tenantService; }
        }
    }
}
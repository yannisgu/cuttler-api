using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using Cuttler.Entities;

namespace Cuttler.Api.ViewModels
{
    public abstract class TenantViewModel<T> where T : Tenant<T>
    {
        protected TenantViewModel()
        { } 

        protected TenantViewModel(T model)
        {
            Name = model.Name;
            MainUrl = model.MainUrl;
            SubscriptionId = model.SubscriptionId;
            Id = model.Id;
        }


        public abstract T AsTenant();

        public virtual void UpdateTenant(T model)
        {
            model.Name = Name;
            model.MainUrl = MainUrl;
            model.SubscriptionId = SubscriptionId;
            model.Id = Id;
        }

        public Guid SubscriptionId { get; set; }

        public string MainUrl { get; set; }

        public string Name { get; set; }
        public Guid Id { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace Cuttler.Entities
{
    public abstract class Tenant<T> where T : Tenant<T>
    {
        public virtual Guid Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string MainUrl { get; set; }

        public virtual Guid SubscriptionId { get; set; }

        public virtual Subscription<T> Subscription { get; set; }

        public abstract string Type { get; }

        public virtual List<Backup> Backups { get; set; }
        public virtual bool Enabled { get; set; }
        
        public virtual List<User> Admins { get; set; }
    }
}
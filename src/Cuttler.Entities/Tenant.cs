using System;

namespace Cuttler.Entities
{
    public class Tenant
    {
        public virtual Guid Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string MainUrl { get; set; }
    }
}
using System;

namespace Cuttler.Entities
{
    public class Subscription<T> where T : Tenant<T>
    {
        public virtual Guid Id { get; set; }

        public virtual string Name { get; set; }

    }
}
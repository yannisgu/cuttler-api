using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuttler.Entities
{
    public class Email
    {
        public virtual Guid Guid { get; set; }
        public virtual string Mail { get; set; }
        public virtual bool Verified { get; set; }

        public virtual Guid UserId { get; set; }
    }
}

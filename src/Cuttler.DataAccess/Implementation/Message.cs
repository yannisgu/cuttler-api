using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cuttler.DataAccess.Implementation
{
    public class Message<T>
    {


        public string Name { get; set; }
        public Guid Id { get; set; }
        public string Action { get; set; }
        public T Pack { get; set; }
    }
}

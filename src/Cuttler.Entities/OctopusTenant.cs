using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuttler.Entities
{
    public class OctopusTenant : Tenant<OctopusTenant>
    {
        public IEnumerable<string> Urls { get; set; } 
    }
}

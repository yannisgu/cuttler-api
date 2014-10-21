using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Cuttler.DataAccess.Implementation
{
    public interface IMessageService
    {
        Task Publish<T>(Message<T> message);
    }
}

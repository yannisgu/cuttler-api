using System;
using System.Threading.Tasks;
using Cuttler.DataAccess.Implementation;

namespace Cuttler.DataAccess
{
    public class MessageService :IMessageService
    {
        public async Task Publish<T>(Message<T> message)
        {
        }
    }
}
using System.Threading.Tasks;
using Cuttler.Entities;

namespace Cuttler.DataAccess
{
    public interface IUserService
    {
        Task<User> Login(string username, string password);
        Task<User> GetUser(string userName);
    }
}
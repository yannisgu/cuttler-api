using System.Threading.Tasks;
using Cuttler.Entities;

namespace Cuttler.DataAccess
{
    public interface IUserService
    {
        Task<User> Login(string username, string password);
        Task<User> GetUser(string userName);
        Task AddUser(User user);
        Task AddLogin(User user, string password, bool enabled = true);
        Task UpdateUser(User newUser);
        Task AddEmail(User user, string email);
    }
}
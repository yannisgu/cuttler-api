using Cuttler.Entities;

namespace Cuttler.DataAccess
{
    public interface IUserService
    {
        User Login(string username, string password);
    }
}
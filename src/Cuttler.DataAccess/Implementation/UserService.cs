using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cuttler.Entities;

namespace Cuttler.DataAccess.Implementation
{
    public class UserService : IUserService
    {
        private readonly DataContext dataContext;
        private readonly PasswordService passwordService;

        public UserService(DataContext dataContext, PasswordService passwordService)
        {
            this.dataContext = dataContext;
            this.passwordService = passwordService;
        }

        public async Task<User> Login(string username, string password)
        {
            var user = await dataContext.Users.Where(u => u.UserName == username).Include(u => u.Logins).FirstOrDefaultAsync();
            if (user.Logins.Any(l => passwordService.ValidatePassword(password, l.PasswordHash)))
            {
                return user;
            }
            return null;
        }

        public async Task<User> GetUser(string userName)
        {
            return await dataContext.Users.Where(u => u.UserName == userName).SingleOrDefaultAsync();
        }

        public async Task AddUser(User user)
        {
            user.Id = Guid.NewGuid();
            user.Logins = null;
            if (await GetUser(user.UserName) != null)
            {
                throw new ArgumentException(string.Format("A user with the username {0} allready exists.", user.UserName));
            }
            dataContext.Users.Add(user);
            await dataContext.SaveChangesAsync();
        }

        public async Task AddLogin(User user, string password, bool enabled = true)
        {
            var login = new Login()
            {
                UserId = user.Id,
                PasswordHash = passwordService.CreateHash(password),
                Enabled = enabled
            };
            dataContext.Logins.Add(login);
            await dataContext.SaveChangesAsync();
        }

        public async Task UpdateUser(User user)
        {
            user.Logins = null;
            dataContext.Users.Attach(user);
            await dataContext.SaveChangesAsync();
        }

        public async Task AddEmail(User user, string email)
        {
            Email mail = new Email()
            {
                Guid = Guid.NewGuid(),
                Mail = email,
                UserId = user.Id,
            };
            dataContext.Emails.Add(mail);
            await dataContext.SaveChangesAsync();
        }
    }
}

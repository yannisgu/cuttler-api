using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
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
            if (user.Logins.Any(l => l.Enabled && passwordService.ValidatePassword(password, l.PasswordHash)))
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
            dataContext.Entry(user).State = EntityState.Modified;
            await dataContext.SaveChangesAsync();
        }

        public async Task AddEmail(User user, string email)
        {
            Email mail = new Email()
            {
                Id = Guid.NewGuid(),
                Mail = email,
                UserId = user.Id,
                Verified = false,
                VerifyCode = GetNewVerifyCode()
            };
            dataContext.Emails.Add(mail);
            await dataContext.SaveChangesAsync();
        }

        private string GetNewVerifyCode()
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                // ANSI (varchar)
                var valueBytes = Encoding.Default.GetBytes(Guid.NewGuid().ToString());
                var md5HashBytes = md5.ComputeHash(valueBytes);
                var builder = new StringBuilder(md5HashBytes.Length * 2);
                foreach (var md5Byte in md5HashBytes)
                    builder.Append(md5Byte.ToString("X2"));
                return builder.ToString();
            }
        }

        public async Task<User> VerifyEmail(string verifyCode)
        {
            var email = await dataContext.Emails
                .Where(_ => !_.Verified && _.VerifyCode == verifyCode)
                .FirstOrDefaultAsync();
            if (email != null)
            {
                email.Verified = true;
                var logins = dataContext.Logins.Where(_ => _.UserId == email.UserId && !_.Enabled);
                logins.ToList().ForEach(_ => _.Enabled = true);
                dataContext.SaveChanges();
                return await dataContext.Users.FindAsync(email.UserId);
            }
            return null;
        }

        public async Task<User> GetUser(Guid guid)
        {
            return await dataContext.Users.FindAsync(guid);
        }
    }
}

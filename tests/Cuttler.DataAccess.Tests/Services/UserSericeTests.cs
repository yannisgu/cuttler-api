using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Cuttler.DataAccess.Implementation;
using Cuttler.DataAccess.Tests.Mocks;
using Cuttler.Entities;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Cuttler.DataAccess.Tests.Services
{
    [TestFixture]
    public class UserSericeTests
    {
        [Test]
        public async void Should_return_user_when_login_with_correct_credentials()
        {
            var usersMock = new UsersDbSetMock();
    
            var pwdMock = new Mock<PasswordService>();
            pwdMock.Setup(_ => _.ValidatePassword("pass", "mockedTrueHash")).Returns(true);
            var dataContextMock = new Mock<DataContext>();
            dataContextMock.SetupGet(ctx => ctx.Users).Returns(usersMock.Object);

            var service = new UserService(dataContextMock.Object, pwdMock.Object);

            var user = await service.Login("yannisgu", "pass");

            Assert.NotNull(user);
            Assert.AreEqual(user.UserName,"yannisgu");
        }

        [Test]
        public async void Should_return_null_when_login_is_incorrect()
        {
            var usersMock = new UsersDbSetMock();
    
            var pwdMock = new Mock<PasswordService>();
            pwdMock.Setup(_ => _.ValidatePassword("pass", "mockedTrueHash")).Returns(true);
            var dataContextMock = new Mock<DataContext>();
            dataContextMock.SetupGet(ctx => ctx.Users).Returns(usersMock.Object);

            var service = new UserService(dataContextMock.Object, pwdMock.Object);

            var user = await service.Login("yannisgu", "wrongPass");

            Assert.Null(user);
        }
        [Test]
        public async void Should_add_user_when_addUser()
        {
            var usersMock = new UsersDbSetMock();
            var dataContextMock = new Mock<DataContext>();
            dataContextMock.SetupGet(ctx => ctx.Users).Returns(usersMock.Object);

            var service = new UserService(dataContextMock.Object, null);

            var user = new User()
            {
                UserName = "nick",
                Country = "United States",
                Location = "New York",
                Street = "Manhatten 60",
                Zip = "70000"
            };
            await service.AddUser(user);
            usersMock.Verify(_ => _.Add(user));
            dataContextMock.Verify(_ => _.SaveChangesAsync());
        }

        [Test]
        public async void Should_add_login_when_calling()
        {
            var usersMock = new UsersDbSetMock();
            var dataContextMock = new Mock<DataContext>();
            dataContextMock.SetupGet(ctx => ctx.Users).Returns(usersMock.Object);
            var loginMock = new Mock<DbSet<Login>>();
            dataContextMock.SetupGet(_ => _.Logins).Returns(loginMock.Object);
            var passwordService = new PasswordService();
            var service = new UserService(dataContextMock.Object, passwordService);
            var user = usersMock.Data.First();
            await service.AddLogin(user, "pass", false);
            loginMock.Verify(_ => _.Add(It.Is<Login>(l =>
                l.Enabled == false && 
                l.UserId == user.Id &&
                passwordService.ValidatePassword("pass", l.PasswordHash))));
            dataContextMock.Verify(_ => _.SaveChangesAsync());
        
            
        }
    }
}

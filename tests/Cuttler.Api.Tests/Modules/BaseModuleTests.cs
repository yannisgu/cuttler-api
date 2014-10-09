using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cuttler.DataAccess;
using Cuttler.Entities;
using Moq;
using Nancy.Responses.Negotiation;
using Nancy.Testing;

namespace Cuttler.Api.Tests.Modules
{
     public class BaseModuleTests
    {
         protected static readonly Guid TestIserGuid = new Guid("{DD0BAA48-9D5F-4167-8672-632D4EE1D27F}");
         protected readonly User testUser = new User() { UserName = "user", Id = TestIserGuid };

         protected virtual Mock<IUserService> GetUserServiceLoginMock()
         {
             var mock = new Mock<IUserService>();
             mock.Setup(svc => svc.Login(It.IsAny<string>(), It.IsAny<string>())).
                 ReturnsAsync(default(User));
             mock.Setup(svc => svc.Login("user", "pass"))
                 .ReturnsAsync(testUser);
             mock.Setup(svc => svc.GetUser("user")).
                 ReturnsAsync(testUser);
             return mock;
         }

         protected BrowserResponse Login(Browser browser, string userName = "user", string password = "pass")
         {
             return browser.Post("/users/login", with =>
             {
                 with.HttpRequest();
                 with.Accept(new MediaRange("application/json"));
                 with.FormValue("username", userName);
                 with.FormValue("password", password);
             });
         }
    }
}

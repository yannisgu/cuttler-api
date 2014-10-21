using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cuttler.DataAccess.Implementation;
using Cuttler.Entities;
using Ninject;
using Ninject.Modules;

namespace Cuttler.DataAccess
{
     public class DataAccessModule : NinjectModule
    {

         public override void Load()
         {
             Bind<IDataContext>().To<DataContext>();
             Bind<IUserService>().To<UserService>();
             Bind<IMessageService>().To<MessageService>();
             Bind<ITenantService<OctopusTenant>>().To<OctopusTenantService>();
             Bind<IFileService>().To<FileService>();

         }
    }
}

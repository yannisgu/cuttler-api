using System.Configuration;
using Cuttler.Api.Modules;
using Cuttler.DataAccess;
using Cuttler.DataAccess.Implementation;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Ninject;
using Nancy.Responses;
using Nancy.Responses.Negotiation;
using Nancy.Session;
using Ninject;
using Ninject.Modules;
using NinjectModule = Cuttler.DataAccess.DataAccessModule;

namespace Cuttler.Api
{
    using Nancy;

    public class Bootstrapper : NinjectNancyBootstrapper
    {
        protected override void ConfigureRequestContainer(IKernel container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);
            
        }

        protected override void ApplicationStartup(IKernel container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            container.Load(new INinjectModule[] { new NinjectModule() });
            pipelines.WithSessionAuthentification(container.Get<IUserService>());
            CookieBasedSessions.Enable(pipelines);
            pipelines.OnError.AddItemToEndOfPipeline((_, err) =>
            {
               
                return new NegotiatedResponse(new Error(err.Message))
                {
                    StatusCode = HttpStatusCode.InternalServerError
                };  
            });
        
        }

        public IKernel Kernel
        {
            get { return this.ApplicationContainer; }
        }

    }
}
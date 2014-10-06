using Nancy.Bootstrappers.Ninject;
using Ninject;

namespace Cuttler.Api
{
    using Nancy;

    public class Bootstrapper : NinjectNancyBootstrapper
    {
        protected override void ConfigureRequestContainer(IKernel container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);
            
        }
    }
}
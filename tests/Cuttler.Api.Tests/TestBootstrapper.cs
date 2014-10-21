using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Cuttler.DataAccess;
using Cuttler.TestFramework;
using Nancy.Bootstrapper;
using Nancy.Session;
using Nancy.Testing;
using Nancy.TinyIoc;
using Newtonsoft.Json;

namespace Cuttler.Api.Tests.Modules
{
    public class TestBootstrapper : ConfigurableBootstrapper
    {
        public TestBootstrapper(Action<ConfigurableBootstrapperConfigurator> config) : base(config)
        {
            
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            this.WithSessionAuthentification(container.Resolve<IUserService>());
            this.WithSession(new Dictionary<string, object>());
            base.ApplicationStartup(container, pipelines);
           
        }
    }

}

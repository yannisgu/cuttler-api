using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Bootstrapper;
using Nancy.Session;
using Nancy.Testing;
using Newtonsoft.Json;

namespace Cuttler.TestFramework
{
    public static class BootstrapperExtensions
    {
        public static void WithSession(this IPipelines pipeline, IDictionary<string, object> session)
        {
            pipeline.BeforeRequest.AddItemToStartOfPipeline(ctx =>
            {
                ctx.Request.Session = new Session(session);
                return null;
            });
        }

        public static T Get<T>(this BrowserResponse resp)
        {
            var content = resp.Body.AsString();
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}

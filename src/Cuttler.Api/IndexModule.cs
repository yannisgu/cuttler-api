namespace Cuttler.Api
{
    using Nancy;

    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = parameters => "asdf";
        }
    }
}
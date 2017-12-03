using Snowflake.Services;
using Unosquare.Labs.EmbedIO;

namespace Snowflake.Support.Remoting.GraphQl.Servers
{
    public class GraphQlServerWrapper : ILocalWebService
    {
        private GraphQlServer remote;
        public GraphQlServerWrapper(GraphQlServer remote)
        {
            this.remote = remote;

        }

        public int Port => 9797;

        public string Protocol => "http";

        public string Name => "GraphQL Root";

        public void Start()
        {
            var webServer = new WebServer($"http://localhost:{this.Port}/");
            webServer.RegisterModule(remote);
            webServer.RunAsync();
        }
    }
}

using Snowflake.Services;
using Unosquare.Labs.EmbedIO;

namespace Snowflake.Support.Remoting.GraphQl.Servers
{
    internal class GraphQlServerWrapper : ILocalWebService
    {
        private GraphQlServer remote;

        public GraphQlServerWrapper(GraphQlServer remote)
        {
            this.remote = remote;
        }

        /// <inheritdoc/>
        public int Port => 9797;

        /// <inheritdoc/>
        public string Protocol => "http";

        /// <inheritdoc/>
        public string Name => "GraphQL Root";

        /// <inheritdoc/>
        public void Start()
        {
            var webServer = new WebServer($"http://localhost:{this.Port}/");
            webServer.RegisterModule(remote);
            webServer.RunAsync();
        }
    }
}

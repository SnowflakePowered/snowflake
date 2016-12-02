using System;
using Snowflake.Extensibility;
using Snowflake.Services;
using Unosquare.Labs.EmbedIO;
using Snowflake.Support.Remoting.Servers;

namespace Snowflake.Support.Remoting
{
    public class RemotingContainer : IPluginContainer
    {
        public void Compose(ICoreService coreInstance)
        {
            var webServer = new WebServer("http://localhost:9696/");
            webServer.RegisterModule(new RestRemotingServer(coreInstance));
            coreInstance.RegisterService(webServer);
            webServer.RunAsync();
        }
    }
}

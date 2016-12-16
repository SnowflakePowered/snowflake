using System;
using Snowflake.Extensibility;
using Snowflake.Services;
using Unosquare.Labs.EmbedIO;
using Snowflake.Support.Remoting.Servers;
using Snowflake.Support.Remoting.Resources;
using Snowflake.Support.Remoting.Framework;

namespace Snowflake.Support.Remoting
{
    public class RemotingContainer : IPluginContainer
    {
        public void Compose(ICoreService coreInstance)
        {
            var pluginEndpoint = new Plugins(coreInstance);
            var stoneEndpoint = new Stone(coreInstance.Get<IStoneProvider>());
            var gameEndpoint = new Games(coreInstance);
            var endpointCollection = new EndpointCollection();
            endpointCollection.Add("~:plugins", (p) => pluginEndpoint.ListPlugins());
            endpointCollection.Add("~:plugins:{echo}", (p) => pluginEndpoint.Echo(p.GetParameters["echo"]));
            endpointCollection.Add("~:stone:platforms", p => stoneEndpoint.ListPlatforms());
            endpointCollection.Add("~:stone:platforms:{platformId}", p => stoneEndpoint.GetPlatform(p.GetParameters["platformId"]));
            endpointCollection.Add("~:stone:controllers", p => stoneEndpoint.ListControllers());
            endpointCollection.Add("~:stone:controllers:{controllerId}", p => stoneEndpoint.GetController(p.GetParameters["controllerId"]));
            endpointCollection.Add("~:games", p => gameEndpoint.ListGames());
            endpointCollection.Add("~:games:{guid}", p => gameEndpoint.GetGame(Guid.Parse(p.GetParameters["guid"])));
            var webServer = new WebServer("http://localhost:9696/");
            webServer.RegisterModule(new RestRemotingServer(endpointCollection));
            coreInstance.RegisterService(webServer);
            webServer.RunAsync();
        }
    }
}

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
            endpointCollection.Add(RequestVerb.Read, "~:plugins", (p) => pluginEndpoint.ListPlugins());
            endpointCollection.Add(RequestVerb.Read, "~:plugins:{echo}", (p) => pluginEndpoint.Echo(p.Get["echo"]));
            endpointCollection.Add(RequestVerb.Read, "~:stone:platforms", p => stoneEndpoint.ListPlatforms());
            endpointCollection.Add(RequestVerb.Read, "~:stone:platforms:{platformId}", p => stoneEndpoint.GetPlatform(p.Get["platformId"]));
            endpointCollection.Add(RequestVerb.Read, "~:stone:controllers", p => stoneEndpoint.ListControllers());
            endpointCollection.Add(RequestVerb.Read, "~:stone:controllers:{controllerId}", p => stoneEndpoint.GetController(p.Get["controllerId"]));



            endpointCollection.Add(RequestVerb.Create, "~:games:{guid}:files",
                p => gameEndpoint.CreateFile(Guid.Parse(p.Get["guid"]), (string)p.Post["path"], (string)p.Post["mimetype"]));
            endpointCollection.Add(RequestVerb.Delete, "~:games:{guid}:files:{fileid}",
                p => gameEndpoint.DeleteFile(Guid.Parse(p.Get["guid"]), Guid.Parse(p.Get["fileid"])));
            endpointCollection.Add(RequestVerb.Read, "~:games", p => gameEndpoint.ListGames());
            endpointCollection.Add(RequestVerb.Read, "~:games:{guid}", p => gameEndpoint.GetGame(Guid.Parse(p.Get["guid"])));
            endpointCollection.Add(RequestVerb.Update, "~:games:{guid}:metadata:{metadata_key}", 
                p => gameEndpoint.SetMetadata(Guid.Parse(p.Get["guid"]), p.Get["metadata_key"], (string)p.Post["value"]));
            endpointCollection.Add(RequestVerb.Delete, "~:games:{guid}:metadata:{metadata_key}", 
                p => gameEndpoint.DeleteMetadata(Guid.Parse(p.Get["guid"]), p.Get["metadata_key"]));
            endpointCollection.Add(RequestVerb.Create, "~:games", p => 
                    gameEndpoint.CreateGame((string)p.Post["title"], (string)p.Post["platform"]));

            endpointCollection.Add(RequestVerb.Delete, "~:games:{guid}", p =>
                   gameEndpoint.DeleteGame(Guid.Parse(p.Get["guid"])));



            var webServer = new WebServer("http://localhost:9696/");
            webServer.RegisterModule(new RestRemotingServer(endpointCollection));
            coreInstance.RegisterService(webServer);
            webServer.RunAsync();
        }
    }
}

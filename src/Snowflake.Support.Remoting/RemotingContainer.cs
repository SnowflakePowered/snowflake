using System;
using Snowflake.Extensibility;
using Snowflake.Services;
using Unosquare.Labs.EmbedIO;
using Snowflake.Support.Remoting.Servers;
using Snowflake.Support.Remoting.Resources;
using Snowflake.Support.Remoting.Framework;
using Snowflake.Loader;
using Snowflake.Records.Game;
using Snowflake.Servers;
using Snowflake.Remoting.Requests;
using Snowflake.Resources.Games;
using Snowflake.Resources.Stone;

namespace Snowflake.Support.Remoting
{

    public class RemotingContainer : IComposable
    {
        [ImportService(typeof(IStoneProvider))]
        [ImportService(typeof(IGameLibrary))]
        [ImportService(typeof(IServiceRegistrationProvider))]
        public void Compose(IModule module, Loader.IServiceProvider coreInstance)
        {
            var stone = coreInstance.Get<IStoneProvider>();
            var gameLib = coreInstance.Get<IGameLibrary>();
            var register = coreInstance.Get<IServiceRegistrationProvider>();
            IResourceContainer container = new ResourceContainer()
            {
                { new GameLibraryRoot(stone, gameLib) },
                { new GameRoot(gameLib) },
                { new GamesFilesRoot(gameLib) },
                { new GameFilesLibraryRoot(gameLib) },
                { new GameMetadataRoot(gameLib) },
                { new GameFilesMetadataRoot(gameLib) },
                { new ControllerRoot(stone) },
                { new ControllersRoot(stone) },
                { new PlatformRoot(stone) },
                { new PlatformsRoot(stone) }
            };
            




            //var plugMan = coreInstance.Get<IPluginManager>();

            //var pluginEndpoint = new Plugins(plugMan);
           // var stoneEndpoint = new Stone(stone);
            var gameEndpoint = new GameLibraryRoot(stone, gameLib);

            /* endpointCollection.Add(RequestVerb.Read, "~:plugins", p => pluginEndpoint.ListPlugins());
            endpointCollection.Add(RequestVerb.Read, "~:plugins:{echo}", p => pluginEndpoint.Echo(p.Url["echo"]));
            */

         /*   endpointCollection.Add(RequestVerb.Read, "~:stone:platforms", p => stoneEndpoint.ListPlatforms());
            endpointCollection.Add(RequestVerb.Read, "~:stone:platforms:{platformId}", p => stoneEndpoint.GetPlatform(p.Url["platformId"]));
            endpointCollection.Add(RequestVerb.Read, "~:stone:controllers", p => stoneEndpoint.ListControllers());
            endpointCollection.Add(RequestVerb.Read, "~:stone:controllers:{controllerId}", p => stoneEndpoint.GetController(p.Url["controllerId"]));
            */

            /*
            endpointCollection.Add(RequestVerb.Create, "~:games:{guid}:files",
                p => gameEndpoint.CreateFile(Guid.Parse(p.Url["guid"]), (string)p.Body["path"], (string)p.Body["mimetype"]));
            endpointCollection.Add(RequestVerb.Delete, "~:games:{guid}:files:{fileid}",
                p => gameEndpoint.DeleteFile(Guid.Parse(p.Url["guid"]), Guid.Parse(p.Url["fileid"])));
*/

            /*
            endpointCollection.Add(RequestVerb.Read, "~:games", 
                p => gameEndpoint.ListGames());
            endpointCollection.Add(RequestVerb.Create, "~:games",
           p => gameEndpoint.CreateGame((string)p.Body["title"], (string)p.Body["platform"]));

            */
            /*
            endpointCollection.Add(RequestVerb.Read, "~:games:{guid}", 
                p => gameEndpoint.GetGame(Guid.Parse(p.Url["guid"])));
            endpointCollection.Add(RequestVerb.Delete, "~:games:{guid}",
              p => gameEndpoint.DeleteGame(Guid.Parse(p.Url["guid"])));
              */


            /*endpointCollection.Add(RequestVerb.Update, "~:games:{guid}:metadata:{metadata_key}", 
                p => gameEndpoint.SetMetadata(Guid.Parse(p.Url["guid"]), p.Url["metadata_key"], (string)p.Body["value"]));
            endpointCollection.Add(RequestVerb.Delete, "~:games:{guid}:metadata:{metadata_key}", 
                p => gameEndpoint.DeleteMetadata(Guid.Parse(p.Url["guid"]), p.Url["metadata_key"]));*/
           
          

            /*
            endpointCollection.Add(RequestVerb.Delete, "~:games:{guid}:files:{fileid}:metadata:{metadata_key}",
                p => gameEndpoint.DeleteFileMetadata(Guid.Parse(p.Url["guid"]), Guid.Parse(p.Url["fileid"]), p.Url["metadata_key"]));
            endpointCollection.Add(RequestVerb.Update, "~:games:{guid}:files:{fileid}:metadata:{metadata_key}",
               p => gameEndpoint.SetFileMetadata(Guid.Parse(p.Url["guid"]), Guid.Parse(p.Url["fileid"]), p.Url["metadata_key"], 
               (string)p.Body["value"]));*/
          
            //  endpointCollection.Add(RequestVerb.Create, "~:scrape:file", p => gameEndpoint.Scrape((string)p.Body["path"]));


            var webServer = new WebServerWrapper(new RestRemotingServer(container));
            webServer.Start();
            //register.RegisterService<ILocalWebService>(webServer); //todo should be plugin.

        }
    }
}

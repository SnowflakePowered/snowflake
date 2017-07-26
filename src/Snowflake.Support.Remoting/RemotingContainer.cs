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
using Snowflake.Resources.Emulators;
using Snowflake.Emulator;
using Snowflake.Mappers;
using Snowflake.Configuration;

namespace Snowflake.Support.Remoting
{

    public class RemotingContainer : IComposable
    {
        [ImportService(typeof(IPluginManager))]
        [ImportService(typeof(IStoneProvider))]
        [ImportService(typeof(IGameLibrary))]
        [ImportService(typeof(IServiceRegistrationProvider))]
        [ImportService(typeof(IConfigurationCollectionStore))]
        public void Compose(IModule module, Loader.IServiceRepository coreInstance)
        {
            var manager = coreInstance.Get<IPluginManager>();
            var stone = coreInstance.Get<IStoneProvider>();
            var gameLib = coreInstance.Get<IGameLibrary>();
            var register = coreInstance.Get<IServiceRegistrationProvider>();
            var store = coreInstance.Get<IConfigurationCollectionStore>();
            IResourceContainer container = new ResourceContainer()
            {
                { new GameLibraryRoot(gameLib) },
                { new GameRoot(gameLib) },
                { new GamesFilesRoot(gameLib) },
                { new GameFilesLibraryRoot(gameLib) },
                { new GameMetadataRoot(gameLib) },
                { new GameFilesMetadataRoot(gameLib) },
                { new ControllerRoot() },
                { new ControllersRoot(stone) },
                { new PlatformRoot() },
                { new PlatformsRoot(stone) },
                { new EmulatorsRoot(manager.Get<IEmulatorAdapter>()) },
                { new EmulatorsGameConfigRoot(manager.Get<IEmulatorAdapter>()) },
                { new GameConfigurationCollectionsRoot(manager.Get<IEmulatorAdapter>()) },
                { new GameConfigurationCollectionsValueRoot(manager.Get<IEmulatorAdapter>(), store) }
            };

            container.AddTypeMapping(new GameRecordMapping(gameLib));
            container.AddTypeMapping(new ControllerLayoutMapping(stone));
            container.AddTypeMapping(new PlatformInfoMapping(stone));
            
            var webServer = new WebServerWrapper(new RestRemotingServer(container));
            webServer.Start();
            //register.RegisterService<ILocalWebService>(webServer); //todo should be plugin.

        }
    }
}

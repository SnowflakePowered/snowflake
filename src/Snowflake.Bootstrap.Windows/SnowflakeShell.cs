using System;
using Snowflake.Bootstrap.Windows.Utility;
using Snowflake.Loader;
using Snowflake.Services;
using Snowflake.Services.AssemblyLoader;


namespace Snowflake.Shell.Windows
{
    internal class SnowflakeShell
    {
        private readonly string appDataDirectory = PathUtility.GetApplicationDataPath().CreateSubdirectory("snowflake").FullName;
        private IServiceContainer loadedCore;

        internal SnowflakeShell()
        {
            this.StartCore();
        }

        public void StartCore()
        {
            this.loadedCore = new ServiceContainer(this.appDataDirectory);
            var loader = this.loadedCore.Get<IModuleEnumerator>();
            var composer = new AssemblyComposer(this.loadedCore, loader);
            composer.Compose();
            // this.loadedCore.Get<IEmulatorAssembliesManager>()?.LoadEmulatorAssemblies();
            // this.loadedCore.Get<IPluginManager>()?.Initialize();
           /* this.loadedCore.Get<IServerManager>().RegisterServer("ThemeServer", new ThemeServer(Path.Combine(this.loadedCore.AppDataDirectory, "themes")));
            foreach (string serverName in this.loadedCore.Get<IServerManager>().RegisteredServers)
            {
                this.loadedCore.Get<IServerManager>()?.StartServer(serverName);
                var serverStartEvent = new ServerStartEventArgs(this.loadedCore, serverName);
                SnowflakeEventManager.EventSource.RaiseEvent(serverStartEvent); //todo Move event registration to SnowflakeEVentManager
            }*/
            /*var im = this.loadedCore.Get<IPluginManager>().Get<IInputEnumerator>()["InputEnumerator-Keyboard"];
            var ep = new EmulatedPort(1, im.ControllerLayout,
                im.GetConnectedDevices().First(), MappedControllerElementCollection.GetDefaultMappings(im.ControllerLayout, this.loadedCore.Get<IStoneProvider>().Controllers["NES_CONTROLLER"]));
            var gr = new GameRecord(this.loadedCore.Get<IStoneProvider>().Platforms["NINTENDO_SNES"], "test",
                @"E:\Super Mario World (USA).sfc", "application/x-romfile-snes-sfc");
            var raadapter = this.loadedCore.Get<IPluginManager>().Get<BsnesRetroArchAdapter>().First().Value;

            var lmfao = raadapter.Instantiate(gr, gr.Files[0], 0, new List<IEmulatedPort> { ep});
            lmfao.Create();
            lmfao.Start();*/
        }

        public void StartShell()
        {
            var electronUi = this.loadedCore.Get<IUserInterface>();

            // electronUi.StartUserInterface();
        }

        public void RestartCore()
        {
            this.ShutdownCore();
            this.StartCore();
        }

        public void ShutdownCore()
        {
            this.loadedCore.Dispose();
            GC.WaitForPendingFinalizers();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Linq;
using System.Reflection;
using Snowflake.Emulator;
using Snowflake.Events;
using Snowflake.Events.ServiceEvents;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;
using Snowflake.Platform;
using Snowflake.Plugin.EmulatorAdapter.RetroArch.Adapters;
using Snowflake.Scraper;
using Snowflake.Service;
using Snowflake.Service.Manager;
using Snowflake.Utility;

namespace Snowflake.Shell.Windows
{
    internal class SnowflakeShell
    {
        private ICoreService loadedCore;
        private readonly string appDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Snowflake");
        internal SnowflakeShell()
        {
            this.StartCore();
        }
       
        public void StartCore()
        {

            this.loadedCore = new CoreService(this.appDataDirectory);
           // this.loadedCore.Get<IEmulatorAssembliesManager>()?.LoadEmulatorAssemblies();
            this.loadedCore.Get<IPluginManager>()?.Initialize();
            this.loadedCore.Get<IServerManager>().RegisterServer("ThemeServer", new ThemeServer(Path.Combine(this.loadedCore.AppDataDirectory, "themes")));
            foreach (string serverName in this.loadedCore.Get<IServerManager>().RegisteredServers)
            {
                this.loadedCore.Get<IServerManager>()?.StartServer(serverName);
                var serverStartEvent = new ServerStartEventArgs(this.loadedCore, serverName);
                SnowflakeEventManager.EventSource.RaiseEvent(serverStartEvent); //todo Move event registration to SnowflakeEVentManager
            }

            var store = new SqliteMappedControllerElementCollectionStore(new SqliteDatabase("controllermappings.db"));
            var devices = this.loadedCore.Get<IPluginManager>()?.Get<IInputEnumerator>()["InputEnumerator-Keyboard"].GetConnectedDevices().First();
            var contrl = this.loadedCore.Get<IStoneProvider>().Controllers["DUALSHOCK_CONTROLLER"];
            var profile = MappedControllerElementCollection.GetDefaultMappings(devices.DeviceLayout, contrl);
            var port = new EmulatedPort(0, contrl, devices,
               profile);
            


        }

        public void StartShell() {
            var electronUi = this.loadedCore.Get<IUserInterface>();
            electronUi.StartUserInterface();
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

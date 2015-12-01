using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Snowflake.Controller;
using Snowflake.Events;
using Snowflake.Events.ServiceEvents;
using Snowflake.Platform;
using Snowflake.Scraper;
using Snowflake.Service;
using Snowflake.Service.Manager;

namespace Snowflake.Shell.Windows
{
    internal class SnowflakeShell
    {
        string ShellRoot => Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "electron");
        private ICoreService loadedCore;
        Process currentShellInstance;
        internal SnowflakeShell()
        {
            this.StartCore();
        }

        public void StartCore()
        {

            this.loadedCore = new CoreService(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Snowflake"));
            this.loadedCore.Get<IEmulatorAssembliesManager>()?.LoadEmulatorAssemblies();
            this.loadedCore.Get<IPluginManager>()?.Initialize();
            this.loadedCore.Get<IAjaxManager>()?.Initialize(this.loadedCore.Get<IPluginManager>());
            foreach (IPlatformInfo platform in this.loadedCore.Platforms.Values)
            {
                this.loadedCore.Get<IControllerPortsDatabase>()?.AddPlatform(platform);
                this.loadedCore.Get<IPlatformPreferenceDatabase>()?.AddPlatform(platform);
            }
            this.loadedCore.Get<IServerManager>().RegisterServer("ThemeServer", new ThemeServer(Path.Combine(this.loadedCore.AppDataDirectory, "theme")));
            foreach (string serverName in this.loadedCore.Get<IServerManager>().RegisteredServers)
            {
                this.loadedCore.Get<IServerManager>()?.StartServer(serverName);
                var serverStartEvent = new ServerStartEventArgs(this.loadedCore, serverName);
                SnowflakeEventManager.EventSource.RaiseEvent(serverStartEvent); //todo Move event registration to SnowflakeEVentManager
            }
            var electronUi = this.loadedCore.Get<IUserInterface>();
            
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

       
        public Process StartShell(params string[] args)
        {
            if (this.ShellAvailable())
            {
                IList<string> arguments = new List<string>(args);
                arguments.Insert(0, this.ShellRoot);
                var electronShell = this.GetShell();
                electronShell.Arguments = string.Join(" ", arguments);
                if (this.currentShellInstance != null) this.currentShellInstance.Close();
                this.currentShellInstance = Process.Start(electronShell);
                return this.currentShellInstance;
            }
            return null;
        }

        public bool ShellAvailable()
        {
            return File.Exists(Path.Combine(this.ShellRoot, "node_modules", "electron-prebuilt", "dist", "electron.exe"));
        }

        public ProcessStartInfo GetShell()
        {
            return new ProcessStartInfo(Path.Combine(this.ShellRoot, "node_modules", "electron-prebuilt", "dist", "electron.exe"));
        }
  
    }
}

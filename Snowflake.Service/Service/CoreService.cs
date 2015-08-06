using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Platform;
using Snowflake.Service.HttpServer;
using Snowflake.Service.JSWebSocketServer;
using System.IO;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Newtonsoft.Json;
using Snowflake.Events.ServiceEvents;
using System.Threading;
using Snowflake.Information;
using System.Reflection;
using Snowflake.Service.Manager;
using Snowflake.Controller;
using Snowflake.Game;
using Snowflake.Emulator.Configuration;
using Snowflake.Emulator.Input.InputManager;
using Snowflake.Events;
namespace Snowflake.Service
{
    [Export(typeof(ICoreService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class CoreService : ICoreService 
    {
        #region Loaded Objects
        public IDictionary<string, IPlatformInfo> LoadedPlatforms { get; private set; }
        public IDictionary<string, IControllerDefinition> LoadedControllers { get; private set; }

        public IPluginManager PluginManager { get; private set; }
        public IAjaxManager AjaxManager { get; }
        public IGameDatabase GameDatabase { get; }
        public IGamepadAbstractionDatabase GamepadAbstractionDatabase { get; }
        public IInputManager InputManager => new InputManager.InputManager();
        public IControllerPortsDatabase ControllerPortsDatabase { get; }
        public IPlatformPreferenceDatabase PlatformPreferenceDatabase { get; private set; }
        public IEmulatorAssembliesManager EmulatorManager { get; private set; }
        #endregion

        public string AppDataDirectory { get; }
        public static ICoreService LoadedCore { get; private set; }
        public IServerManager ServerManager { get; private set; }
        // Flag: Has Dispose already been called? 
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        public static void InitCore()
        {
            CoreService.InitCore(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Snowflake"));
        }
        public EventHandler<ServerStartEventArgs> ServerStartEvent; 
        public static void InitCore(string dataDirectory)
        {
            var core = new CoreService(dataDirectory);
            CoreService.LoadedCore = core;
            SnowflakeEventManager.EventSource.RegisterEvent<ServerStartEventArgs>(core.ServerStartEvent);
            SnowflakeEventManager.EventSource.Subscribe<ServerStartEventArgs>((s, e) =>
            {
                Console.WriteLine(e.ServerName);
            });
            foreach (string serverName in CoreService.LoadedCore.ServerManager.RegisteredServers)
            {
                CoreService.LoadedCore.ServerManager.StartServer(serverName);
                var serverStartEvent = new ServerStartEventArgs(core, serverName);
                SnowflakeEventManager.EventSource.RaiseEvent<ServerStartEventArgs>(serverStartEvent); //todo Move event registration to SnowflakeEVentManager

            }

        }
      
        public async static Task InitPluginManagerAsync()
        {
            await Task.Run(() => CoreService.InitPluginManager());
        }

        public static void InitPluginManager()
        {
            CoreService.LoadedCore.EmulatorManager.LoadEmulatorAssemblies();
            CoreService.LoadedCore.PluginManager.LoadAll();
            CoreService.LoadedCore.AjaxManager.LoadAll();
            foreach (PlatformInfo platform in CoreService.LoadedCore.LoadedPlatforms.Values)
            {
                CoreService.LoadedCore.ControllerPortsDatabase.AddPlatform(platform);
                CoreService.LoadedCore.PlatformPreferenceDatabase.AddPlatform(platform);
            }
        }

        internal CoreService(string appDataDirectory)
        {
            this.AppDataDirectory = appDataDirectory;
            this.LoadedPlatforms = this.LoadPlatforms(Path.Combine(this.AppDataDirectory, "platforms"));
            this.LoadedControllers = this.LoadControllers(Path.Combine(this.AppDataDirectory, "controllers"));
            this.ServerManager = new ServerManager();
            this.GameDatabase = new GameDatabase(Path.Combine(this.AppDataDirectory, "games.db"));
            this.GamepadAbstractionDatabase = new GamepadAbstractionDatabase(Path.Combine(this.AppDataDirectory, "gamepads.db"));
            this.ControllerPortsDatabase = new ControllerPortsDatabase(Path.Combine(this.AppDataDirectory, "ports.db"));
            this.PluginManager = new PluginManager(this.AppDataDirectory);
            this.AjaxManager = new AjaxManager(this.AppDataDirectory);
            this.EmulatorManager = new EmulatorAssembliesManager(Path.Combine(this.AppDataDirectory, "emulators"));
            this.PlatformPreferenceDatabase = new PlatformPreferencesDatabase(Path.Combine(this.AppDataDirectory, "platformprefs.db"), this.PluginManager);
            this.ServerManager.RegisterServer("AjaxApiServer", new ApiServer());
            this.ServerManager.RegisterServer("WebSocketApiServer", new JsonApiWebSocketServer(30003));
            this.ServerManager.RegisterServer("GameCacheServer", new GameCacheServer());
            
        }
        private IDictionary<string, IPlatformInfo> LoadPlatforms(string platformDirectory)
        {
            var loadedPlatforms = new Dictionary<string, IPlatformInfo>();
            if (!Directory.Exists(platformDirectory)) Directory.CreateDirectory(platformDirectory);
            foreach (string fileName in Directory.GetFiles(platformDirectory).Where(fileName => Path.GetExtension(fileName) == ".platform"))
            {
                try
                {
                    var _platform = JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(File.ReadAllText(fileName));
                    var platform = PlatformInfo.FromJsonProtoTemplate(_platform); //Convert MediaStoreKey reference to full MediaStore object
                    loadedPlatforms.Add(platform.PlatformID, platform);
                }
                catch (Exception)
                {
                    //log
                    Console.WriteLine($"Exception occured when importing platform {fileName}");
                    continue;
                }
            }
            return loadedPlatforms;
        }
        private IDictionary<string, IControllerDefinition> LoadControllers(string controllerDirectory)
        {
            var loadedControllers = new Dictionary<string, IControllerDefinition>();
            if (!Directory.Exists(controllerDirectory)) Directory.CreateDirectory(controllerDirectory);
            foreach (string fileName in Directory.GetFiles(controllerDirectory).Where(fileName => Path.GetExtension(fileName) == ".controller"))
            {
                try
                {
                    var _controller = JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(File.ReadAllText(fileName));
                    var controller = ControllerDefinition.FromJsonProtoTemplate(_controller);
                    loadedControllers.Add(controller.ControllerID, controller);
                }
                catch (Exception)
                {
                    //log
                    Console.WriteLine($"Exception occured when importing controller {fileName}");
                    continue;
                }
            }
            return loadedControllers;
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
                return;

            if (disposing)
            {
                this.LoadedPlatforms = null;
                this.LoadedControllers = null;
                this.PlatformPreferenceDatabase = null;
                this.PluginManager.Dispose();
                this.ServerManager.Dispose();
                this.ServerManager = null;
                this.PluginManager = null;
                this.EmulatorManager = null;
            }

            // Free any unmanaged objects here. 
            //
            this.disposed = true;
        }

        public static void DisposeLoadedCore()
        {
            
           CoreService.LoadedCore?.Dispose();
           CoreService.LoadedCore = null;
        }
    }
}

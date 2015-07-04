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
using Snowflake.Events.ServiceEvents;
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
        public IAjaxManager AjaxManager { get; private set; }
        public IGameDatabase GameDatabase { get; private set; }
        public IGamepadAbstractionDatabase GamepadAbstractionDatabase { get; private set; }
        public IInputManager InputManager { get { return new Snowflake.InputManager.InputManager(); } }
        public IControllerPortsDatabase ControllerPortsDatabase { get; private set; }
        public IPlatformPreferenceDatabase PlatformPreferenceDatabase { get; private set; }
        public IEmulatorAssembliesManager EmulatorManager { get; private set; }
        #endregion

        public string AppDataDirectory { get; private set; }
        public static ICoreService LoadedCore { get; private set; }
        public IServerManager ServerManager { get; private set; }

        public static void InitCore()
        {
            SnowflakeEventSource.InitEventSource();
            var core = new CoreService();
            CoreService.LoadedCore = core;
            foreach (string serverName in CoreService.LoadedCore.ServerManager.RegisteredServers)
            {
                CoreService.LoadedCore.ServerManager.StartServer(serverName);
                SnowflakeEventSource.EventSource.OnServerStart(new ServerStartEventArgs(core, serverName));
            }
          
        }
      
        public async static Task InitPluginManagerAsync()
        {
            await Task.Run(() => InitPluginManager());
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

        public CoreService() : this(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Snowflake")) { }
        public CoreService(string appDataDirectory)
        {
            this.AppDataDirectory = appDataDirectory;
#if DEBUG
            this.AppDataDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
#endif
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
            this.ServerManager.RegisterServer("ThemeServer", new ThemeServer(Path.Combine(this.AppDataDirectory, "theme")));
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
                    Console.WriteLine("Exception occured when importing platform " + fileName);
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
                    Console.WriteLine("Exception occured when importing controller " + fileName);
                    continue;
                }
            }
            return loadedControllers;
        }
    }
}

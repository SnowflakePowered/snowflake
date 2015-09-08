using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snowflake.Ajax;
using Snowflake.Controller;
using Snowflake.Emulator;
using Snowflake.Emulator.Input.InputManager;
using Snowflake.Events;
using Snowflake.Events.ServiceEvents;
using Snowflake.Game;
using Snowflake.Platform;
using Snowflake.Plugin;
using Snowflake.Scraper;
using Snowflake.Service.HttpServer;
using Snowflake.Service.JSWebSocketServer;
using Snowflake.Service.Manager;

namespace Snowflake.Service
{
   
    [Export(typeof(ICoreService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CoreService : ICoreService
    {
        #region Loaded Objects
        public IDictionary<string, IPlatformInfo> Platforms { get; }
        public IDictionary<string, IControllerDefinition> Controllers { get; }
        public string AppDataDirectory { get; }
        private IDictionary<Type, dynamic> serviceContainer;


        #endregion

        // Flag: Has Dispose already been called? 
        bool disposed;

        // Instantiate a SafeHandle instance.
      /*  public static void InitCore()
        {
            CoreService.InitCore(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Snowflake"));
        }*/
        public EventHandler<ServerStartEventArgs> ServerStartEvent; 
     /*   public static void InitCore(string dataDirectory)
        {
            var core = new CoreService(dataDirectory);
            CoreService.LoadedCore = core;
            SnowflakeEventManager.EventSource.RegisterEvent(core.ServerStartEvent);
            SnowflakeEventManager.EventSource.Subscribe<ServerStartEventArgs>((s, e) =>
            {
                Console.WriteLine(e.ServerName);
            });
            foreach (string serverName in CoreService.LoadedCore.ServerManager.RegisteredServers)
            {
                CoreService.LoadedCore.ServerManager.StartServer(serverName);
                var serverStartEvent = new ServerStartEventArgs(core, serverName);
                SnowflakeEventManager.EventSource.RaiseEvent(serverStartEvent); //todo Move event registration to SnowflakeEVentManager

            }

        }*/
      
    /*    public async static Task InitPluginManagerAsync()
        {
            await Task.Run(() => CoreService.InitPluginManager());
        }*/

      /*  public static void InitPluginManager()
        {
            CoreService.LoadedCore.EmulatorManager.LoadEmulatorAssemblies();
            CoreService.LoadedCore.PluginManager.Initialize();
            CoreService.LoadedCore.AjaxManager.Initialize(CoreService.LoadedCore.PluginManager);
            foreach (PlatformInfo platform in CoreService.LoadedCore.LoadedPlatforms.Values)
            {
                CoreService.LoadedCore.ControllerPortsDatabase.AddPlatform(platform);
                CoreService.LoadedCore.PlatformPreferenceDatabase.AddPlatform(platform);
            }
        }*/

        public CoreService(string appDataDirectory)
        {
            this.serviceContainer = new Dictionary<Type, dynamic>();
            this.AppDataDirectory = appDataDirectory;
            this.Platforms = this.LoadPlatforms(Path.Combine(this.AppDataDirectory, "platforms"));
            this.Controllers = this.LoadControllers(Path.Combine(this.AppDataDirectory, "controllers"));

            this.RegisterService<IServerManager>(new ServerManager());
            this.RegisterService<IGameDatabase>(new GameDatabase(Path.Combine(this.AppDataDirectory, "games.db")));
            this.RegisterService<IGamepadAbstractionDatabase>(new GamepadAbstractionDatabase(Path.Combine(this.AppDataDirectory, "gamepads.db")));
            this.RegisterService<IControllerPortsDatabase>(new ControllerPortsDatabase(Path.Combine(this.AppDataDirectory, "ports.db")));
            this.RegisterService<IPluginManager>(new PluginManager(this.AppDataDirectory, typeof(IEmulatorBridge), typeof(IScraper), typeof(IGeneralPlugin), typeof(IBaseAjaxNamespace)));
            this.RegisterService<IAjaxManager>(new AjaxManager(this));
            this.RegisterService<IEmulatorAssembliesManager>(new EmulatorAssembliesManager(Path.Combine(this.AppDataDirectory, "emulators")));
            this.RegisterService<IPlatformPreferenceDatabase>(new PlatformPreferencesDatabase(Path.Combine(this.AppDataDirectory, "platformprefs.db"), this.PluginManager));
            this.RegisterService<IInputManager>(new InputManager.InputManager());
            var serverManager = this.Get<IServerManager>();
            serverManager.RegisterServer("AjaxApiServer", new ApiServer());
            serverManager.RegisterServer("WebSocketApiServer", new JsonApiWebSocketServer(30003));
            serverManager.RegisterServer("GameCacheServer", new GameCacheServer());
            
        }

        public void RegisterService<T>(T serviceObject)
        {
            if (this.serviceContainer.ContainsKey(typeof (T))) return;
            this.serviceContainer.Add(typeof(T), serviceObject);
        }

        public IEnumerable<string> AvailableServices()
        {
            return this.serviceContainer.Keys.Select(service => service.Name);
        } 

        public T Get<T>()
        {
            return this.serviceContainer[typeof (T)];
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

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Snowflake.Romfile;
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
        private readonly IDictionary<Type, dynamic> serviceContainer;


        #endregion

        // Flag: Has Dispose already been called? 
        bool disposed;

        // Instantiate a SafeHandle instance.
    
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
            this.RegisterService<IEmulatorAssembliesManager>(new EmulatorAssembliesManager(Path.Combine(this.AppDataDirectory, "emulators")));
            this.RegisterService<IInputManager>(new InputManager.InputManager());
            this.RegisterService<IPluginManager>(new PluginManager(this.AppDataDirectory, this, typeof(IEmulatorBridge), typeof(IScraper), typeof(IFileSignature), typeof(IGeneralPlugin), typeof(IBaseAjaxNamespace)));
            this.RegisterService<IAjaxManager>(new AjaxManager(this));
            this.RegisterService<IPlatformPreferenceDatabase>(new PlatformPreferencesDatabase(Path.Combine(this.AppDataDirectory, "platformprefs.db"), this.Get<IPluginManager>()));
            this.RegisterService<IScrapeEngine>(new ScrapeEngine(this));
            this.RegisterService<IEmulatorInstanceManager>(new EmulatorInstanceManager(this));
            var serverManager = this.Get<IServerManager>();
            serverManager.RegisterServer("AjaxApiServer", new ApiServer(this));
            serverManager.RegisterServer("WebSocketApiServer", new JsonApiWebSocketServer(30003, this));
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
              this.Get<IPluginManager>().Dispose();
              this.Get<IServerManager>().Dispose();
              GC.Collect();

            }

            // Free any unmanaged objects here. 
            //
            this.disposed = true;
        }
    }
}

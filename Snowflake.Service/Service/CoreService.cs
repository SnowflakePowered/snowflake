using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Snowflake.Ajax;
using Snowflake.Controller;
using Snowflake.Emulator;
using Snowflake.Emulator.Input.InputManager;
using Snowflake.Events;
using Snowflake.Events.ServiceEvents;
using Snowflake.Game;
using Snowflake.Platform;
using Snowflake.Extensibility;
using Snowflake.Romfile;
using Snowflake.Scraper;
using Snowflake.Service.HttpServer;
using Snowflake.Service.JSWebSocketServer;
using Snowflake.Service.Manager;
using NLog;

namespace Snowflake.Service
{
   
    [Export(typeof(ICoreService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CoreService : ICoreService
    {
        #region Loaded Objects
        public IStoneProvider StoneProvider { get; }
        public IDictionary<string, IPlatformInfo> Platforms { get; }
        public IDictionary<string, IControllerDefinition> Controllers { get; }
        public string AppDataDirectory { get; }
        public dynamic InfoBlob { get; }
        private readonly IDictionary<Type, dynamic> serviceContainer;
        private ILogger logger;

        #endregion

        // Flag: Has Dispose already been called? 
        bool disposed;

        // Instantiate a SafeHandle instance.
    
        public CoreService(string appDataDirectory)
        {
            this.logger = LogManager.GetLogger("~CORESERVICE");
            this.serviceContainer = new Dictionary<Type, dynamic>();
            this.AppDataDirectory = appDataDirectory;
            this.InfoBlob = JsonConvert.DeserializeObject(File.ReadAllText(Path.Combine(this.AppDataDirectory, "info.json")));
            this.Platforms = this.LoadPlatforms();
            this.Controllers = this.LoadControllers();

            this.RegisterService<IServerManager>(new ServerManager());
            this.RegisterService<IGameLibrary>(new GameLibrary(Path.Combine(this.AppDataDirectory, "games.db")));
            this.RegisterService<IGamepadAbstractionStore>(new GamepadAbstractionStore(Path.Combine(this.AppDataDirectory, "gamepads.db")));
            this.RegisterService<IControllerPortStore>(new ControllerPortStore(Path.Combine(this.AppDataDirectory, "ports.db")));
            this.RegisterService<IEmulatorAssembliesManager>(new EmulatorAssembliesManager(Path.Combine(this.AppDataDirectory, "emulators")));
            this.RegisterService<IInputManager>(new InputManager.InputManager());
            this.RegisterService<IPluginManager>(new PluginManager(this.AppDataDirectory, this));
            this.RegisterService<IAjaxManager>(new AjaxManager(this));
            this.RegisterService<IPlatformPreferenceStore>(new PlatformPreferencesStore(Path.Combine(this.AppDataDirectory, "platformprefs.db"), this.Get<IPluginManager>()));
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
            return this.serviceContainer.ContainsKey(typeof (T)) ? this.serviceContainer[typeof (T)] : default(T);
        }

        private IDictionary<string, IPlatformInfo> LoadPlatforms()
        {
            var loadedPlatforms = new Dictionary<string, IPlatformInfo>();
            foreach (var _platform in this.InfoBlob["platforms"])
            {
                try
                {
                    var platform = PlatformInfo.FromJsonProtoTemplate(_platform); //Convert MediaStoreKey reference to full MediaStore object
                    loadedPlatforms.Add(platform.PlatformID, platform);
                }
                catch (Exception ex)
                {
                  logger.Error(ex, "Something went wrong when loading a platform from the info blob. Regenerate it by deleting info.json");
                }
            }
            return loadedPlatforms;
        }
        private IDictionary<string, IControllerDefinition> LoadControllers()
        {
            var loadedControllers = new Dictionary<string, IControllerDefinition>();
            foreach (var _controller in this.InfoBlob["controllers"])
            {
                try
                {
                    var controller = ControllerDefinition.FromJsonProtoTemplate(_controller);
                    loadedControllers.Add(controller.ControllerID, controller);
                }
                catch (Exception ex)
                {
                    //log
                    logger.Error(ex, "Something went wrong when loading a controller from the info blob. Regenerate it by deleting info.json");
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

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using Snowflake.Service.HttpServer;
using Snowflake.Service.JSWebSocketServer;
using Snowflake.Service.Manager;
using NLog;
using Snowflake.Configuration;
using Snowflake.Input;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Records.Game;
using Snowflake.Utility;

namespace Snowflake.Service
{
   
    [Export(typeof(ICoreService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CoreService : ICoreService
    {
        #region Loaded Objects
        public string AppDataDirectory { get; }
        private readonly IDictionary<Type, object> serviceContainer;
        private ILogger logger;

        #endregion

        // Flag: Has Dispose already been called? 
        bool disposed;

        // Instantiate a SafeHandle instance.
    
        public CoreService(string appDataDirectory)
        {
            this.logger = LogManager.GetLogger("~CORESERVICE");
            this.serviceContainer = new ConcurrentDictionary<Type, object>();
            this.RegisterService<IStoneProvider>(new StoneProvider());
            this.AppDataDirectory = appDataDirectory;
            this.RegisterService<IServerManager>(new ServerManager());
            this.RegisterService<IGameLibrary>(new SqliteGameLibrary(new SqliteDatabase(Path.Combine(this.AppDataDirectory, "games.db"))));
            this.RegisterService<IMappedControllerElementCollectionStore>
                (new SqliteMappedControllerElementCollectionStore(new SqliteDatabase(Path.Combine(this.AppDataDirectory, "controllermappings.db"))));
            this.RegisterService<IConfigurationCollectionStore>(new SqliteConfigurationCollectionStore(new SqliteDatabase(Path.Combine(this.AppDataDirectory, "configurations.db"))));
            //this.RegisterService<IEmulatorAssembliesManager>(new EmulatorAssembliesManager(Path.Combine(this.AppDataDirectory, "emulators")));
            this.RegisterService<IPluginManager>(new PluginManager(this.AppDataDirectory, this)); 
            this.RegisterService<IAjaxManager>(new AjaxManager(this)); //todo deprecate with michi-based ipc
           // this.RegisterService<IScrapeEngine>(new ScrapeEngine(this.Get<IStoneProvider>(), this.Get<IPluginManager>()));
            var serverManager = this.Get<IServerManager>();
            serverManager.RegisterServer("AjaxApiServer", new ApiServer(this)); //todo deprecate with michi-based ipc
            serverManager.RegisterServer("WebSocketApiServer", new JsonApiWebSocketServer(30003, this)); //todo deprecate with michi-based ipc
            
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
            return this.serviceContainer.ContainsKey(typeof (T)) ? (T)this.serviceContainer[typeof (T)] : default(T);
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

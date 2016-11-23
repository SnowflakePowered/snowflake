using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NLog;
using Snowflake.Configuration;
using Snowflake.Input;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Records.Game;
using Snowflake.Utility;

namespace Snowflake.Services
{
  
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
            this.RegisterService<IGameLibrary>(new SqliteGameLibrary(new SqliteDatabase(Path.Combine(this.AppDataDirectory, "games.db"))));
            this.RegisterService<IMappedControllerElementCollectionStore>
                (new SqliteMappedControllerElementCollectionStore(new SqliteDatabase(Path.Combine(this.AppDataDirectory, "controllermappings.db"))));
            this.RegisterService<IConfigurationCollectionStore>(new SqliteConfigurationCollectionStore(new SqliteDatabase(Path.Combine(this.AppDataDirectory, "configurations.db"))));
          
            this.RegisterService<IPluginManager>(new PluginManager(this.AppDataDirectory, this)); 
           
            
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
            //todo throw?
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
              GC.Collect();

            }

            // Free any unmanaged objects here. 
            //
            this.disposed = true;
        }
    }
}

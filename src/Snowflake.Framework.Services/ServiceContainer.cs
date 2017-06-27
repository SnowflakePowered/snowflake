using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Snowflake.Configuration;
using Snowflake.Input;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Records.Game;
using Snowflake.Utility;
<<<<<<< HEAD:src/Snowflake.Framework.Services/CoreService.cs
using Snowflake.Romfile;
using Snowflake.Scraper.Shiragame;
=======
using Snowflake.Loader;
using Snowflake.Services.Logging;
using Snowflake.Services.Persistence;
>>>>>>> cd39263... Modules: Robust loader and plugin system (#249):src/Snowflake.Framework.Services/ServiceContainer.cs

namespace Snowflake.Services
{
  
    public class ServiceContainer : IServiceContainer
    {
        #region Loaded Objects
        public string AppDataDirectory { get; }
        private readonly IDictionary<Type, object> serviceContainer;
        #endregion

        // Flag: Has Dispose already been called? 
        bool disposed;

        // Instantiate a SafeHandle instance.
    
        public ServiceContainer(string appDataDirectory)
        {
            this.AppDataDirectory = appDataDirectory;
<<<<<<< HEAD:src/Snowflake.Framework.Services/CoreService.cs
            this.RegisterService<IGameLibrary>(new SqliteGameLibrary(new SqliteDatabase(Path.Combine(this.AppDataDirectory, "games.db"))));
            this.RegisterService<IMappedControllerElementCollectionStore>
                (new SqliteMappedControllerElementCollectionStore(new SqliteDatabase(Path.Combine(this.AppDataDirectory, "controllermappings.db"))));
            this.RegisterService<IConfigurationCollectionStore>(new SqliteConfigurationCollectionStore(new SqliteDatabase(Path.Combine(this.AppDataDirectory, "configurations.db"))));
            this.RegisterService<IFileSignatureMatcher>(new FileSignatureMatcher(this.Get<IStoneProvider>()));
            this.RegisterService<IShiragameProvider>(new ShiragameProvider("shiragame.db"));
            this.RegisterService<IPluginManager>(new PluginManager(this.AppDataDirectory, this)); 
           
=======
            var directoryProvider = new ContentDirectoryProvider(this.AppDataDirectory);
            this.serviceContainer = new ConcurrentDictionary<Type, object>();
            this.RegisterService<ILogProvider>(new LogProvider());
            this.RegisterService<IModuleEnumerator>(new ModuleEnumerator(appDataDirectory));
            this.RegisterService<IContentDirectoryProvider>(directoryProvider);
            this.RegisterService<IServiceRegistrationProvider>(new ServiceRegistrationProvider(this));
            this.RegisterService<ISqliteDatabaseProvider>(new SqliteDatabaseProvider(directoryProvider.ApplicationData.CreateSubdirectory("libraries")));
>>>>>>> cd39263... Modules: Robust loader and plugin system (#249):src/Snowflake.Framework.Services/ServiceContainer.cs
            
            //this.RegisterService<IPluginManager>(new PluginManager(this.AppDataDirectory, this)); 
        }

        public void RegisterService<T>(T serviceObject)
        {
            //todo: check for same name
            if (this.serviceContainer.ContainsKey(typeof (T))) return; //todo throw exception
            this.serviceContainer.Add(typeof(T), serviceObject);
        }

        public IEnumerable<string> AvailableServices()
        {
            return this.serviceContainer.Keys.Select(service => service.FullName);
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

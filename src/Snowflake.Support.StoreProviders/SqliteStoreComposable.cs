using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Snowflake.Extensibility.Configuration;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Loader;
using Snowflake.Model.Database;
using Snowflake.Model.Database.Models;
using Snowflake.Model.Game;
using Snowflake.Model.Game.LibraryExtensions;
using Snowflake.Services;
using Zio;
using Zio.FileSystems;

namespace Snowflake.Support.StoreProviders
{
    public class SqliteStoreComposable : IComposable
    {
        [ImportService(typeof(IServiceRegistrationProvider))]
        [ImportService(typeof(IContentDirectoryProvider))]

        public void Compose(IModule module, Loader.IServiceRepository serviceContainer)
        {
            var contentDirectory = serviceContainer.Get<IContentDirectoryProvider>();
            string sqlitePath = Path.Combine(contentDirectory.ApplicationData.FullName, "library.db");
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder
                .UseSqlite($"Data Source={sqlitePath}");

            using (var context = new DatabaseContext(optionsBuilder.Options))
            {
                var connection = context.Database.GetDbConnection();
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "PRAGMA journal_mode=WAL;";
                    command.ExecuteNonQuery();
                }
            }

            // game library dependency tree

            var gameRecordLibrary = new GameRecordLibrary(optionsBuilder);
            var gameLibrary = new GameLibrary(gameRecordLibrary);
            var configStore = new ConfigurationCollectionStore(optionsBuilder);

            var fileLibrary = new FileRecordLibrary(optionsBuilder);
            var physicalFs = new PhysicalFileSystem();

            var appDataPath = physicalFs.ConvertPathFromInternal(contentDirectory.ApplicationData.FullName);
            var gameFs = physicalFs.GetOrCreateSubFileSystem(appDataPath / "games");

            // Add default extensions
            gameLibrary.AddExtension<IGameFileExtensionProvider, 
                IGameFileExtension>(new GameFileExtensionProvider(fileLibrary, gameFs));

            gameLibrary.AddExtension<IGameConfigurationExtensionProvider,
              IGameConfigurationExtension>(new GameConfigurationExtensionProvider(configStore));

            // register game library.
            serviceContainer.Get<IServiceRegistrationProvider>()
                .RegisterService<IGameLibrary>(gameLibrary);

            var pluginLibrary = new PluginConfigurationStore(optionsBuilder);

            // plugin config store

            serviceContainer.Get<IServiceRegistrationProvider>()
                .RegisterService<IPluginConfigurationStore>(pluginLibrary);

            // controller elements

            var inputStore = new ControllerElementMappingsStore(optionsBuilder);
            serviceContainer.Get<IServiceRegistrationProvider>()
               .RegisterService<IControllerElementMappingsStore>(inputStore);

        }
    }
}

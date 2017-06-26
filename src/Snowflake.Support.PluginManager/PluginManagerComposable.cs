using Snowflake.Loader;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.PluginManager
{
    public class PluginManagerComposable : IComposable
    {
        [ImportService(typeof(ILogProvider))]
        [ImportService(typeof(IContentDirectoryProvider))]
        [ImportService(typeof(IServiceRegistrationProvider))]
        [ImportService(typeof(ISqliteDatabaseProvider))]
        public void Compose(IModule composableModule, Loader.IServiceProvider serviceContainer)
        {
            var logProvider = serviceContainer.Get<ILogProvider>();
            var appdataProvider = serviceContainer.Get<IContentDirectoryProvider>();
            var registrationProvider = serviceContainer.Get<IServiceRegistrationProvider>();
            var sqliteDbProvider = serviceContainer.Get<ISqliteDatabaseProvider>();
            var pluginManager = new PluginManager(logProvider, appdataProvider, sqliteDbProvider);
            registrationProvider.RegisterService<IPluginManager>(pluginManager);
        }
    }
}

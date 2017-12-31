using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Extensibility.Configuration;
using Snowflake.Loader;
using Snowflake.Services;

namespace Snowflake.Support.PluginManager
{
    public class PluginManagerComposable : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(ILogProvider))]
        [ImportService(typeof(IContentDirectoryProvider))]
        [ImportService(typeof(IServiceRegistrationProvider))]
        [ImportService(typeof(IPluginConfigurationStore))]
        public void Compose(IModule composableModule, Loader.IServiceRepository serviceContainer)
        {
            var logProvider = serviceContainer.Get<ILogProvider>();
            var appdataProvider = serviceContainer.Get<IContentDirectoryProvider>();
            var registrationProvider = serviceContainer.Get<IServiceRegistrationProvider>();
            var configStoreProvider = serviceContainer.Get<IPluginConfigurationStore>();
            var pluginManager = new PluginManager(logProvider, appdataProvider, configStoreProvider);
            registrationProvider.RegisterService<IPluginManager>(pluginManager);
        }
    }
}

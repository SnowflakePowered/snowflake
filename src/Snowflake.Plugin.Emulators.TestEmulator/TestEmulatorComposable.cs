using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Configuration;
using Snowflake.EmulatorOld;
using Snowflake.Loader;
using Snowflake.Services;

namespace Snowflake.Plugin.Emulators.TestEmulator
{
    public class TestEmulatorComposable : IComposable
    {
        // Import our services

        /// <inheritdoc/>
        [ImportService(typeof(IPluginManager))]
        [ImportService(typeof(IContentDirectoryProvider))]
        [ImportService(typeof(IStoneProvider))]
        [ImportService(typeof(ILogProvider))]
        [ImportService(typeof(IConfigurationCollectionStore))]
        public void Compose(IModule composableModule,
            IServiceRepository serviceContainer)
        {
            // Get the plugin manager
            IPluginManager pluginManager = serviceContainer.Get<IPluginManager>();

            // Get the directory provider
            IContentDirectoryProvider contentDirectoryProvider
                = serviceContainer.Get<IContentDirectoryProvider>();

            string appDataDirectory = contentDirectoryProvider.ApplicationData.FullName;
       
        }
    }
}

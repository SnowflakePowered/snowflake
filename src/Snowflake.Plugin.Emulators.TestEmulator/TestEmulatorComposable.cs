﻿using Snowflake.Configuration;
using Snowflake.Emulator;
using Snowflake.Loader;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Plugin.Emulators.TestEmulator
{
    public class TestEmulatorComposable : IComposable
    {
        // Import our services
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
            var provision = pluginManager.GetProvision<TestEmulatorAdapter>(composableModule);

            pluginManager.Register<IEmulatorAdapter>(new TestEmulatorAdapter(
                provision,
                serviceContainer.Get<IStoneProvider>(),
                serviceContainer.Get<IConfigurationCollectionStore>(),
                new BiosManager(appDataDirectory),
                new SaveManager(appDataDirectory)));
        }
    }
}

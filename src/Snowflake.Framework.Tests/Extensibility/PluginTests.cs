using Snowflake.Extensibility.Provisioned;
using Snowflake.Loader;
using Snowflake.Services;
using Snowflake.Services.Logging;
using Snowflake.Services.Persistence;
using Snowflake.Support.PluginManager;
using Snowflake.Tests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;
namespace Snowflake.Extensibility.Tests
{
    [Plugin("TestPlugin", Author = "TestAuthor", Description = "TestDescription")]
    class StandalonePluginImpl : StandalonePlugin
    {
    }

    [Plugin("TestPluginProvisioned", Author = "TestAuthor", Description = "TestDescription")]
    class ProvisionedPluginImpl : ProvisionedPlugin
    {
        public ProvisionedPluginImpl(IPluginProvision provision) : base(provision)
        {
        }
    }

    public class PluginTests
    {
        [Fact]
        public void StandalonePluginImpl_Test()
        {
            var plugin = new StandalonePluginImpl();
            Assert.Equal("TestPlugin", plugin.Name);
            Assert.Equal("TestAuthor", plugin.Author);
            Assert.Equal("TestDescription", plugin.Description);
        }

        [Fact]
        public void PluginManagerRegister_Test()
        {
            var appDataDirectory = new DirectoryInfo(Path.GetTempPath())
                .CreateSubdirectory(Guid.NewGuid().ToString());
            var directoryProvider = new ContentDirectoryProvider(appDataDirectory.FullName);
            var sqliteProvider = new SqliteDatabaseProvider(appDataDirectory);
            var logProvider = new LogProvider();
            var pluginManager = new PluginManager(logProvider, directoryProvider, sqliteProvider);
            pluginManager.Register<StandalonePlugin>(new StandalonePluginImpl());
            Assert.NotEmpty(pluginManager.Get<StandalonePlugin>());
            Assert.NotNull(pluginManager.Get<StandalonePlugin>("TestPlugin"));
        }

        [Fact]
        public void PluginManagerRegisterProvisioned_Test()
        {
            var appDataDirectory = new DirectoryInfo(Path.GetTempPath())
                .CreateSubdirectory(Guid.NewGuid().ToString());
            var module = new Module("","","","",appDataDirectory);
            var resourceDir = module.ContentsDirectory.CreateSubdirectory("resource").CreateSubdirectory("TestPluginProvisioned");
            string pluginJson = TestUtilities.GetStringResource("Loader.plugin.json");
            File.WriteAllText(Path.Combine(resourceDir.FullName, "plugin.json"), pluginJson);
            var directoryProvider = new ContentDirectoryProvider(appDataDirectory.FullName);
            var sqliteProvider = new SqliteDatabaseProvider(appDataDirectory);
            var logProvider = new LogProvider();
            var pluginManager = new PluginManager(logProvider, directoryProvider, sqliteProvider);
            var provision = pluginManager.GetProvision<ProvisionedPluginImpl>(module);
            var plugin = new ProvisionedPluginImpl(provision);

            pluginManager.Register<ProvisionedPlugin>(plugin);
            Assert.NotEmpty(pluginManager.Get<ProvisionedPlugin>());
            Assert.NotNull(pluginManager.Get<ProvisionedPlugin>("TestPluginProvisioned")); 
        }
    }
}

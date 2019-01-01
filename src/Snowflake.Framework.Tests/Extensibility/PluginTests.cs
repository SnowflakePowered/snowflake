using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Extensibility.Provisioning.Standalone;
using Snowflake.Loader;
using Snowflake.Services;
using Snowflake.Services.Logging;
using Snowflake.Support.PluginManager;
using Snowflake.Tests;
using Xunit;
using Snowflake.Configuration;
using Snowflake.Extensibility.Configuration;
using Microsoft.EntityFrameworkCore;
using Snowflake.Model.Database;
using Snowflake.Model.Database.Models;

namespace Snowflake.Extensibility.Tests
{
    [Plugin("TestPlugin", "1.0.0.0", Author = "TestAuthor", Description = "TestDescription")]
    public class StandalonePluginImpl : StandalonePlugin
    {
        public StandalonePluginImpl()
            : base(typeof(StandalonePluginImpl))
        {
        }
    }

    public class NonAttributedStandalonePluginImpl : StandalonePlugin
    {
        public NonAttributedStandalonePluginImpl()
            : base(typeof(NonAttributedStandalonePluginImpl))
        {
        }
    }

    [Plugin("TestPluginProvisioned", Author = "TestAuthor", Description = "TestDescription")]
    public class ProvisionedPluginImpl : ProvisionedPlugin
    {
        public ProvisionedPluginImpl(IPluginProvision provision)
            : base(provision)
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
            Assert.Equal(Version.Parse("1.0.0.0"), plugin.Version);
            Assert.Throws<NotImplementedException>(() => plugin.Provision.CommonResourceDirectory);
            Assert.Throws<NotImplementedException>(() => plugin.Provision.ContentDirectory);
            Assert.Throws<NotImplementedException>(() => plugin.Provision.ResourceDirectory);
            Assert.Throws<NotImplementedException>(() => plugin.Provision.Logger);
            Assert.Throws<NotImplementedException>(() => plugin.Provision.ResourceDirectory);
            Assert.Equal(EmptyPluginConfigurationStore.EmptyConfigurationStore, plugin.Provision.ConfigurationStore);
            Assert.Equal(EmptyPluginProperties.EmptyProperties, plugin.Provision.Properties);
        }

        [Fact]
        public void NonAttributedStandalonePluginImpl_Test()
        {
           Assert.Throws<InvalidOperationException>(() => new NonAttributedStandalonePluginImpl());
        }

        [Fact]
        public void EmptyPluginProps_Test()
        {
            var props = EmptyPluginProperties.EmptyProperties;
            Assert.Null(props.Get(string.Empty));
            Assert.Empty(props.PropertyKeys);
            Assert.Empty(props.GetEnumerable(string.Empty));
            Assert.Empty(props.GetDictionary(string.Empty));
        }

        [Fact]
        public void EmptyConfigurationStore_Test()
        {
            var config = EmptyPluginConfigurationStore.EmptyConfigurationStore;
            config.Set(new ConfigurationSection<ExampleConfigurationSection>());
            config.Get<ExampleConfigurationSection>();
        }

        [Fact]
        public void PluginManagerRegister_Test()
        {
            var appDataDirectory = new DirectoryInfo(Path.GetTempPath())
                .CreateSubdirectory(Guid.NewGuid().ToString());
            var directoryProvider = new ContentDirectoryProvider(appDataDirectory.FullName);
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var sqliteProvider = new PluginConfigurationStore(optionsBuilder);
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
            var module = new Module(string.Empty, string.Empty, string.Empty, string.Empty, appDataDirectory, Version.Parse("1.0.0"));
            var resourceDir = module.ContentsDirectory.CreateSubdirectory("resource").CreateSubdirectory("TestPluginProvisioned");
            string pluginJson = TestUtilities.GetStringResource("Loader.plugin.json");
            File.WriteAllText(Path.Combine(resourceDir.FullName, "plugin.json"), pluginJson);
            var directoryProvider = new ContentDirectoryProvider(appDataDirectory.FullName);
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var sqliteProvider= new PluginConfigurationStore(optionsBuilder);

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

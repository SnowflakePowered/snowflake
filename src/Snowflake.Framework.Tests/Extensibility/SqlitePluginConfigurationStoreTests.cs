using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Snowflake.Configuration;
using Snowflake.Extensibility.Configuration;
using Snowflake.Persistence;
using Xunit;

namespace Snowflake.Extensibility.Tests
{
    public class SqlitePluginConfigurationStoreTests
    {
        [Fact]
        public void ConfigurationStoreSet_Test()
        {
            var store = new SqlitePluginConfigurationStore(new SqliteDatabase(Path.GetTempFileName()));
            var configSection = new ConfigurationSection<ExampleConfigurationSection>();
            store.Set(configSection);
        }

        [Fact]
        public void ConfigurationStoreGet_Test()
        {
            var store = new SqlitePluginConfigurationStore(new SqliteDatabase(Path.GetTempFileName()));
            var configSection = new ConfigurationSection<ExampleConfigurationSection>();
            store.Set(configSection);

            var retrievedConfig = store.Get<ExampleConfigurationSection>();
            Assert.NotNull(retrievedConfig);
            Assert.Equal(configSection.Configuration.ISOPath0, retrievedConfig.Configuration.ISOPath0);
            Assert.Equal(configSection.Configuration.FullscreenResolution, retrievedConfig.Configuration.FullscreenResolution);
            Assert.Equal(configSection.Configuration.Fullscreen, retrievedConfig.Configuration.Fullscreen);
        }

        [Fact]
        public void ConfigurationStoreSetIndividual_Test()
        {
            var store = new SqlitePluginConfigurationStore(new SqliteDatabase(Path.GetTempFileName()));
            var configSection = new ConfigurationSection<ExampleConfigurationSection>();
            store.Set(configSection);
            configSection.Configuration.ISOPath0 = "TestEqual";
            configSection.Configuration.FullscreenResolution = FullscreenResolution.Resolution1152X648;
            configSection.Configuration.Fullscreen = false;

            store.Set(configSection);
            configSection.Configuration.FullscreenResolution = FullscreenResolution.Resolution1280X768;
            store.Set(configSection.Configuration.Values["FullscreenResolution"]);

            var retrievedConfig = store.Get<ExampleConfigurationSection>();
            Assert.NotNull(retrievedConfig);
            Assert.NotNull(retrievedConfig);
            Assert.Equal(configSection.Configuration.ISOPath0, retrievedConfig.Configuration.ISOPath0);
            Assert.Equal(configSection.Configuration.FullscreenResolution, retrievedConfig.Configuration.FullscreenResolution);
            Assert.Equal(configSection.Configuration.Fullscreen, retrievedConfig.Configuration.Fullscreen);
        }

        [Fact]
        public void ConfigurationStoreSetIndividualEnumerable_Test()
        {
            var store = new SqlitePluginConfigurationStore(new SqliteDatabase(Path.GetTempFileName()));
            var configSection = new ConfigurationSection<ExampleConfigurationSection>();
            store.Set(configSection);
            configSection.Configuration.ISOPath0 = "TestEqual";
            configSection.Configuration.FullscreenResolution = FullscreenResolution.Resolution1152X648;
            configSection.Configuration.Fullscreen = false;

            store.Set(configSection);
            configSection.Configuration.FullscreenResolution = FullscreenResolution.Resolution1280X768;
            configSection.Configuration.Fullscreen = true;
            store.Set(new[] { configSection.Configuration.Values["FullscreenResolution"],
                              configSection.Configuration.Values["Fullscreen"],
            });

            var retrievedConfig = store.Get<ExampleConfigurationSection>();
            Assert.NotNull(retrievedConfig);
            Assert.NotNull(retrievedConfig);
            Assert.Equal(configSection.Configuration.ISOPath0, retrievedConfig.Configuration.ISOPath0);
            Assert.Equal(configSection.Configuration.FullscreenResolution, retrievedConfig.Configuration.FullscreenResolution);
            Assert.Equal(configSection.Configuration.Fullscreen, retrievedConfig.Configuration.Fullscreen);
        }
    }
}

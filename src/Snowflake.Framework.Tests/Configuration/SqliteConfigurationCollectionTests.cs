using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Persistence;

using Xunit;

namespace Snowflake.Configuration.Tests
{
    public class ConfigurationCollectionStoreTests
    {
        [Fact]
        public void ConfigurationStoreSet_Test()
        {
            var store = new SqliteConfigurationCollectionStore(new SqliteDatabase(Path.GetTempFileName()));
            var configCollection = new ConfigurationCollection<ExampleConfigurationCollection>();
            store.Set(configCollection, Guid.Empty, "test", "test");
        }

        [Fact]
        public void ConfigurationStoreGet_Test()
        {
            var store = new SqliteConfigurationCollectionStore(new SqliteDatabase(Path.GetTempFileName()));
            var configCollection = new ConfigurationCollection<ExampleConfigurationCollection>();
            configCollection.Configuration.ExampleConfiguration.ISOPath0 = "TestEqual";
            configCollection.Configuration.ExampleConfiguration.FullscreenResolution = FullscreenResolution.Resolution1152X648;
            configCollection.Configuration.ExampleConfiguration.Fullscreen = false;

            store.Set(configCollection, Guid.Empty, "test", "test");

            var retrievedConfig = store.Get<ExampleConfigurationCollection>(Guid.Empty, "test", "test");
            Assert.NotNull(retrievedConfig);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.ISOPath0, retrievedConfig.Configuration.ExampleConfiguration.ISOPath0);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.FullscreenResolution, retrievedConfig.Configuration.ExampleConfiguration.FullscreenResolution);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.Fullscreen, retrievedConfig.Configuration.ExampleConfiguration.Fullscreen);
        }

        [Fact]
        public void ConfigurationStoreSetIndividual_Test()
        {
            var store = new SqliteConfigurationCollectionStore(new SqliteDatabase(Path.GetTempFileName()));
            var configCollection = new ConfigurationCollection<ExampleConfigurationCollection>();
            configCollection.Configuration.ExampleConfiguration.ISOPath0 = "TestEqual";
            configCollection.Configuration.ExampleConfiguration.FullscreenResolution = FullscreenResolution.Resolution1152X648;
            configCollection.Configuration.ExampleConfiguration.Fullscreen = false;

            store.Set(configCollection, Guid.Empty, "test", "test");
            configCollection.Configuration.ExampleConfiguration.FullscreenResolution = FullscreenResolution.Resolution1280X768;
            store.Set(configCollection.Configuration.ExampleConfiguration.Values["FullscreenResolution"]);

            var retrievedConfig = store.Get<ExampleConfigurationCollection>(Guid.Empty, "test", "test");
            Assert.NotNull(retrievedConfig);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.ISOPath0, retrievedConfig.Configuration.ExampleConfiguration.ISOPath0);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.FullscreenResolution, retrievedConfig.Configuration.ExampleConfiguration.FullscreenResolution);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.Fullscreen, retrievedConfig.Configuration.ExampleConfiguration.Fullscreen);
        }

        [Fact]
        public void ConfigurationStoreMultipleProfileIndividual_Test()
        {
            var store = new SqliteConfigurationCollectionStore(new SqliteDatabase(Path.GetTempFileName()));
            var configCollection = new ConfigurationCollection<ExampleConfigurationCollection>();
            configCollection.Configuration.ExampleConfiguration.ISOPath0 = "TestEqual";
            configCollection.Configuration.ExampleConfiguration.FullscreenResolution = FullscreenResolution.Resolution1152X648;
            configCollection.Configuration.ExampleConfiguration.Fullscreen = false;

            var otherConfigCollection = new ConfigurationCollection<ExampleConfigurationCollection>();
            otherConfigCollection.Configuration.ExampleConfiguration.ISOPath0 = "TestNotEqual";
            otherConfigCollection.Configuration.ExampleConfiguration.FullscreenResolution = FullscreenResolution.Resolution1280X1024;
            otherConfigCollection.Configuration.ExampleConfiguration.Fullscreen = true;

            store.Set(configCollection, Guid.Empty, "test", "test");
            store.Set(otherConfigCollection, Guid.Empty, "some other", "some other");

            var retrievedConfig = store.Get<ExampleConfigurationCollection>(Guid.Empty, "test", "test");
            var newRetrievedConfig = store.Get<ExampleConfigurationCollection>(Guid.Empty, "some other", "some other");

            Assert.NotNull(retrievedConfig);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.ISOPath0, retrievedConfig.Configuration.ExampleConfiguration.ISOPath0);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.FullscreenResolution, retrievedConfig.Configuration.ExampleConfiguration.FullscreenResolution);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.Fullscreen, retrievedConfig.Configuration.ExampleConfiguration.Fullscreen);

            Assert.NotEqual(retrievedConfig.Configuration.ExampleConfiguration.FullscreenResolution, newRetrievedConfig.Configuration.ExampleConfiguration.FullscreenResolution);
            Assert.NotEqual(retrievedConfig.Configuration.ExampleConfiguration.ISOPath0, newRetrievedConfig.Configuration.ExampleConfiguration.ISOPath0);
            Assert.NotEqual(retrievedConfig.Configuration.ExampleConfiguration.Fullscreen, newRetrievedConfig.Configuration.ExampleConfiguration.Fullscreen);
        }

        [Fact]
        public void ConfigurationStoreGetEmpty_Test()
        {
            var store = new SqliteConfigurationCollectionStore(new SqliteDatabase(Path.GetTempFileName()));
            var configCollection = new ConfigurationCollection<ExampleConfigurationCollection>();

            var retrievedConfig = store.Get<ExampleConfigurationCollection>(Guid.Empty, "test", "test");
            Assert.NotNull(retrievedConfig);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.ISOPath0, retrievedConfig.Configuration.ExampleConfiguration.ISOPath0);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.FullscreenResolution, retrievedConfig.Configuration.ExampleConfiguration.FullscreenResolution);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.Fullscreen, retrievedConfig.Configuration.ExampleConfiguration.Fullscreen);
        }
    }
}
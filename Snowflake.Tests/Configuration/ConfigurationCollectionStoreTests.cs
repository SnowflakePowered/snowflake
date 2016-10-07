using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Records;
using Snowflake.Utility;
using Xunit;
namespace Snowflake.Configuration.Tests
{
    public class ConfigurationCollectionStoreTests
    {
        [Fact]
        public void ConfigurationStoreSet_Test()
        {
            IConfigurationCollectionStore store = new SqliteConfigurationCollectionStore(new SqliteDatabase(Path.GetTempFileName()));
            var configCollection = ConfigurationCollection.MakeDefault<ExampleConfigurationCollection>();
            store.Set(configCollection, Guid.Empty);
        }

        [Fact]
        public void ConfigurationStoreGet_Test()
        {
            IConfigurationCollectionStore store = new SqliteConfigurationCollectionStore(new SqliteDatabase(Path.GetTempFileName()));
            var configCollection = ConfigurationCollection.MakeDefault<ExampleConfigurationCollection>();
            configCollection.ExampleConfiguration.ISOPath0 = "TestEqual";
            configCollection.ExampleConfiguration.FullscreenResolution = FullscreenResolution.Resolution1152X648;
            configCollection.ExampleConfiguration.Fullscreen = false;
            
            store.Set(configCollection, Guid.Empty);
            var retrievedConfig = store.Get<ExampleConfigurationCollection>(Guid.Empty);
            Assert.NotNull(retrievedConfig);
            Assert.Equal(configCollection.ExampleConfiguration.ISOPath0, retrievedConfig.ExampleConfiguration.ISOPath0);
            Assert.Equal(configCollection.ExampleConfiguration.FullscreenResolution, retrievedConfig.ExampleConfiguration.FullscreenResolution);
            Assert.Equal(configCollection.ExampleConfiguration.Fullscreen, retrievedConfig.ExampleConfiguration.Fullscreen);
        }


        [Fact]
        public void ConfigurationStoreGetEmpty_Test()
        {
            IConfigurationCollectionStore store = new SqliteConfigurationCollectionStore(new SqliteDatabase(Path.GetTempFileName()));
            var configCollection = ConfigurationCollection.MakeDefault<ExampleConfigurationCollection>();
          
            var retrievedConfig = store.Get<ExampleConfigurationCollection>(Guid.Empty);
            Assert.NotNull(retrievedConfig);
            Assert.Equal(configCollection.ExampleConfiguration.ISOPath0, retrievedConfig.ExampleConfiguration.ISOPath0);
            Assert.Equal(configCollection.ExampleConfiguration.FullscreenResolution, retrievedConfig.ExampleConfiguration.FullscreenResolution);
            Assert.Equal(configCollection.ExampleConfiguration.Fullscreen, retrievedConfig.ExampleConfiguration.Fullscreen);

        }

        [Fact]
        public void SetSingleConfigRecord_Test()
        {
            IConfigurationCollectionStore store = new SqliteConfigurationCollectionStore(new SqliteDatabase(Path.GetTempFileName()));

            var configCollection = ConfigurationCollection.MakeDefault<ExampleConfigurationCollection>();
            var setting = configCollection.First().Options.Values.First().GetValue(Guid.Empty);
            store.Set(setting);
        }

        [Fact]
        public void RemoveConfigRecord_Test()
        {
            IConfigurationCollectionStore store = new SqliteConfigurationCollectionStore(new SqliteDatabase(Path.GetTempFileName()));
            var configCollection = ConfigurationCollection.MakeDefault<ExampleConfigurationCollection>();
            configCollection.ExampleConfiguration.ISOPath0 = "TEST";
            store.Set(configCollection, Guid.Empty);
            var preDeleteConfig = store.Get<ExampleConfigurationCollection>(Guid.Empty);
            Assert.Equal("TEST", preDeleteConfig.ExampleConfiguration.ISOPath0);
            store.Remove(preDeleteConfig.ExampleConfiguration.Options["ISOPath0"].GetValue(Guid.Empty));
            Assert.NotEqual("TEST", store.Get<ExampleConfigurationCollection>(Guid.Empty).ExampleConfiguration.ISOPath0);
        }

        [Fact]
        public void RemoveMultiConfigRecord_Test()
        {
            IConfigurationCollectionStore store = new SqliteConfigurationCollectionStore(new SqliteDatabase(Path.GetTempFileName()));
            var configCollection = ConfigurationCollection.MakeDefault<ExampleConfigurationCollection>();
            configCollection.ExampleConfiguration.ISOPath0 = "TEST";
            store.Set(configCollection, Guid.Empty);
            var preDeleteConfig = store.Get<ExampleConfigurationCollection>(Guid.Empty);
            Assert.Equal("TEST", preDeleteConfig.ExampleConfiguration.ISOPath0);
            store.Remove(new List<IConfigurationValue> { preDeleteConfig.ExampleConfiguration.Options["ISOPath0"].GetValue(Guid.Empty)});
            //this uses the ienumerable branch for remove
            Assert.NotEqual("TEST", store.Get<ExampleConfigurationCollection>(Guid.Empty).ExampleConfiguration.ISOPath0);
        }
    }
}

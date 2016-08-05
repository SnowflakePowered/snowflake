using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Utility;
using Xunit;
namespace Snowflake.Configuration.Tests
{
    public class ConfigurationCollectionStoreTests
    {
        [Fact]
        public void ConfigurationStoreSet_Test()
        {
            IConfigurationCollectionStore store = new ConfigurationCollectionStore(new SqliteDatabase(Path.GetTempFileName()));
            var configCollection = ConfigurationCollection.MakeDefault<ExampleConfigurationCollection>();
            store.SetConfiguration(configCollection, Guid.Empty);
        }
        [Fact]
        public void ConfigurationStoreGet_Test()
        {
            IConfigurationCollectionStore store = new ConfigurationCollectionStore(new SqliteDatabase(Path.GetTempFileName()));
            var configCollection = ConfigurationCollection.MakeDefault<ExampleConfigurationCollection>();
            configCollection.ExampleConfiguration.ISOPath0 = "TestEqual";
            configCollection.ExampleConfiguration.FullscreenResolution = FullscreenResolution.Resolution1152X648;
            configCollection.ExampleConfiguration.Fullscreen = false;
            
            store.SetConfiguration(configCollection, Guid.Empty);

            var retrievedConfig = store.GetConfiguration<ExampleConfigurationCollection>(Guid.Empty);
            Assert.NotNull(retrievedConfig);
            Assert.Equal(configCollection.ExampleConfiguration.ISOPath0, retrievedConfig.ExampleConfiguration.ISOPath0);
            Assert.Equal(configCollection.ExampleConfiguration.FullscreenResolution, retrievedConfig.ExampleConfiguration.FullscreenResolution);
            Assert.Equal(configCollection.ExampleConfiguration.Fullscreen, retrievedConfig.ExampleConfiguration.Fullscreen);

        }



    }
}

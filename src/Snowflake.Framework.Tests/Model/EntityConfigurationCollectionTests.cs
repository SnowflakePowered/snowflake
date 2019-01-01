using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Snowflake.Model.Database;
using Snowflake.Model.Database.Models;
using Snowflake.Persistence;

using Xunit;

namespace Snowflake.Configuration.Tests
{
    public class ConfigurationCollectionStoreTests
    {
        [Fact]
        public void ConfigurationStoreSet_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var store = new ConfigurationCollectionStore(optionsBuilder);
            store.CreateConfiguration<ExampleConfigurationCollection>("Test");
           

        }

        [Fact]
        public void ConfigurationStoreGet_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var store = new ConfigurationCollectionStore(optionsBuilder);
            var configCollection = store.CreateConfiguration<ExampleConfigurationCollection>("Test");

            configCollection.Configuration.ExampleConfiguration.ISOPath0 = "TestEqual";
            configCollection.Configuration.ExampleConfiguration.FullscreenResolution = FullscreenResolution.Resolution1152X648;
            configCollection.Configuration.ExampleConfiguration.Fullscreen = false;

            store.UpdateConfiguration(configCollection);

            var retrievedConfig = store.GetConfiguration<ExampleConfigurationCollection>(configCollection.ValueCollection.Guid);
            Assert.NotNull(retrievedConfig);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.ISOPath0, retrievedConfig.Configuration.ExampleConfiguration.ISOPath0);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.FullscreenResolution, retrievedConfig.Configuration.ExampleConfiguration.FullscreenResolution);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.Fullscreen, retrievedConfig.Configuration.ExampleConfiguration.Fullscreen);
        }

        [Fact]
        public void ConfigurationStoreSetIndividual_Test()
        {

            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var store = new ConfigurationCollectionStore(optionsBuilder);
            var configCollection = store.CreateConfiguration<ExampleConfigurationCollection>("Test");


            configCollection.Configuration.ExampleConfiguration.ISOPath0 = "TestEqual";
            configCollection.Configuration.ExampleConfiguration.FullscreenResolution = FullscreenResolution.Resolution1152X648;
            configCollection.Configuration.ExampleConfiguration.Fullscreen = false;

            store.UpdateConfiguration(configCollection);
            configCollection.Configuration.ExampleConfiguration.FullscreenResolution = FullscreenResolution.Resolution1280X768;
            store.UpdateValue(configCollection.Configuration.ExampleConfiguration.Values["FullscreenResolution"]);

            var retrievedConfig = store.GetConfiguration<ExampleConfigurationCollection>(configCollection.ValueCollection.Guid);
            Assert.NotNull(retrievedConfig);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.ISOPath0, retrievedConfig.Configuration.ExampleConfiguration.ISOPath0);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.FullscreenResolution, retrievedConfig.Configuration.ExampleConfiguration.FullscreenResolution);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.Fullscreen, retrievedConfig.Configuration.ExampleConfiguration.Fullscreen);
        }


        [Fact]
        public void ConfigurationStoreMultipleProfileIndividual_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var store = new ConfigurationCollectionStore(optionsBuilder);
            var configCollection = store.CreateConfiguration<ExampleConfigurationCollection>("Test");

            configCollection.Configuration.ExampleConfiguration.ISOPath0 = "TestEqual";
            configCollection.Configuration.ExampleConfiguration.FullscreenResolution = FullscreenResolution.Resolution1152X648;
            configCollection.Configuration.ExampleConfiguration.Fullscreen = false;

            var otherConfigCollection = store.CreateConfiguration<ExampleConfigurationCollection>("Test");
            otherConfigCollection.Configuration.ExampleConfiguration.ISOPath0 = "TestNotEqual";
            otherConfigCollection.Configuration.ExampleConfiguration.FullscreenResolution = FullscreenResolution.Resolution1280X1024;
            otherConfigCollection.Configuration.ExampleConfiguration.Fullscreen = true;

            store.UpdateConfiguration(configCollection);
            store.UpdateConfiguration(otherConfigCollection);

            var retrievedConfig = store.GetConfiguration<ExampleConfigurationCollection>(configCollection.ValueCollection.Guid);
            var newRetrievedConfig = store.GetConfiguration<ExampleConfigurationCollection>(otherConfigCollection.ValueCollection.Guid);

            Assert.NotNull(retrievedConfig);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.ISOPath0, retrievedConfig.Configuration.ExampleConfiguration.ISOPath0);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.FullscreenResolution, retrievedConfig.Configuration.ExampleConfiguration.FullscreenResolution);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.Fullscreen, retrievedConfig.Configuration.ExampleConfiguration.Fullscreen);

            Assert.NotEqual(retrievedConfig.Configuration.ExampleConfiguration.FullscreenResolution, newRetrievedConfig.Configuration.ExampleConfiguration.FullscreenResolution);
            Assert.NotEqual(retrievedConfig.Configuration.ExampleConfiguration.ISOPath0, newRetrievedConfig.Configuration.ExampleConfiguration.ISOPath0);
            Assert.NotEqual(retrievedConfig.Configuration.ExampleConfiguration.Fullscreen, newRetrievedConfig.Configuration.ExampleConfiguration.Fullscreen);
        }

        [Fact]
        public void ConfigurationStoreGetDefault_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var store = new ConfigurationCollectionStore(optionsBuilder);
            var configCollection = store.CreateConfiguration<ExampleConfigurationCollection>("Test");
            var retrievedConfig = store.GetConfiguration<ExampleConfigurationCollection>(configCollection.ValueCollection.Guid);

            Assert.NotNull(retrievedConfig);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.ISOPath0, retrievedConfig.Configuration.ExampleConfiguration.ISOPath0);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.FullscreenResolution, retrievedConfig.Configuration.ExampleConfiguration.FullscreenResolution);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.Fullscreen, retrievedConfig.Configuration.ExampleConfiguration.Fullscreen);
        }
    }
}
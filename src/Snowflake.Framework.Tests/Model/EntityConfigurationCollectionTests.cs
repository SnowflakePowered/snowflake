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
            configCollection.Configuration.ExampleConfiguration.FullscreenResolution =
                FullscreenResolution.Resolution1152X648;
            configCollection.Configuration.ExampleConfiguration.Fullscreen = false;

            store.UpdateConfiguration(configCollection);

            var retrievedConfig =
                store.GetConfiguration<ExampleConfigurationCollection>(configCollection.ValueCollection.Guid);
            Assert.NotNull(retrievedConfig);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.ISOPath0,
                retrievedConfig.Configuration.ExampleConfiguration.ISOPath0);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.FullscreenResolution,
                retrievedConfig.Configuration.ExampleConfiguration.FullscreenResolution);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.Fullscreen,
                retrievedConfig.Configuration.ExampleConfiguration.Fullscreen);
        }

        [Fact]
        public void ConfigurationStoreSetIndividual_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var store = new ConfigurationCollectionStore(optionsBuilder);
            var configCollection = store.CreateConfiguration<ExampleConfigurationCollection>("Test");

            configCollection.Configuration.ExampleConfiguration.ISOPath0 = "TestEqual";
            configCollection.Configuration.ExampleConfiguration.FullscreenResolution =
                FullscreenResolution.Resolution1152X648;
            configCollection.Configuration.ExampleConfiguration.Fullscreen = false;

            store.UpdateConfiguration(configCollection);
            configCollection.Configuration.ExampleConfiguration.FullscreenResolution =
                FullscreenResolution.Resolution1280X768;
            store.UpdateValue(configCollection.GetSection(e => e.ExampleConfiguration).Values["FullscreenResolution"]);

            var retrievedConfig =
                store.GetConfiguration<ExampleConfigurationCollection>(configCollection.ValueCollection.Guid);
            Assert.NotNull(retrievedConfig);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.ISOPath0,
                retrievedConfig.Configuration.ExampleConfiguration.ISOPath0);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.FullscreenResolution,
                retrievedConfig.Configuration.ExampleConfiguration.FullscreenResolution);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.Fullscreen,
                retrievedConfig.Configuration.ExampleConfiguration.Fullscreen);
        }

        [Fact]
        public void ConfigurationStoreMultipleProfileIndividual_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var store = new ConfigurationCollectionStore(optionsBuilder);
            var configCollection = store.CreateConfiguration<ExampleConfigurationCollection>("Test");

            configCollection.Configuration.ExampleConfiguration.ISOPath0 = "TestEqual";
            configCollection.Configuration.ExampleConfiguration.FullscreenResolution =
                FullscreenResolution.Resolution1152X648;
            configCollection.Configuration.ExampleConfiguration.Fullscreen = false;

            var otherConfigCollection = store.CreateConfiguration<ExampleConfigurationCollection>("Test");
            otherConfigCollection.Configuration.ExampleConfiguration.ISOPath0 = "TestNotEqual";
            otherConfigCollection.Configuration.ExampleConfiguration.FullscreenResolution =
                FullscreenResolution.Resolution1280X1024;
            otherConfigCollection.Configuration.ExampleConfiguration.Fullscreen = true;

            store.UpdateConfiguration(configCollection);
            store.UpdateConfiguration(otherConfigCollection);

            var retrievedConfig =
                store.GetConfiguration<ExampleConfigurationCollection>(configCollection.ValueCollection.Guid);
            var newRetrievedConfig =
                store.GetConfiguration<ExampleConfigurationCollection>(otherConfigCollection.ValueCollection.Guid);

            Assert.NotNull(retrievedConfig);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.ISOPath0,
                retrievedConfig.Configuration.ExampleConfiguration.ISOPath0);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.FullscreenResolution,
                retrievedConfig.Configuration.ExampleConfiguration.FullscreenResolution);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.Fullscreen,
                retrievedConfig.Configuration.ExampleConfiguration.Fullscreen);

            Assert.NotEqual(retrievedConfig.Configuration.ExampleConfiguration.FullscreenResolution,
                newRetrievedConfig.Configuration.ExampleConfiguration.FullscreenResolution);
            Assert.NotEqual(retrievedConfig.Configuration.ExampleConfiguration.ISOPath0,
                newRetrievedConfig.Configuration.ExampleConfiguration.ISOPath0);
            Assert.NotEqual(retrievedConfig.Configuration.ExampleConfiguration.Fullscreen,
                newRetrievedConfig.Configuration.ExampleConfiguration.Fullscreen);
        }

        [Fact]
        public void ConfigurationStoreGetDefault_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var store = new ConfigurationCollectionStore(optionsBuilder);
            var configCollection = store.CreateConfiguration<ExampleConfigurationCollection>("Test");
            var retrievedConfig =
                store.GetConfiguration<ExampleConfigurationCollection>(configCollection.ValueCollection.Guid);

            Assert.NotNull(retrievedConfig);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.ISOPath0,
                retrievedConfig.Configuration.ExampleConfiguration.ISOPath0);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.FullscreenResolution,
                retrievedConfig.Configuration.ExampleConfiguration.FullscreenResolution);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.Fullscreen,
                retrievedConfig.Configuration.ExampleConfiguration.Fullscreen);
        }


        [Fact]
        public async Task ConfigurationStoreSetAsync_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var store = new ConfigurationCollectionStore(optionsBuilder);
            await store.CreateConfigurationAsync<ExampleConfigurationCollection>("Test");
        }

        [Fact]
        public async Task ConfigurationStoreGetAsync_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var store = new ConfigurationCollectionStore(optionsBuilder);
            var configCollection = await store.CreateConfigurationAsync<ExampleConfigurationCollection>("Test");

            var resGuid = Guid.NewGuid();
            configCollection.Configuration.ExampleConfiguration.ISOPath0 = "TestEqual";
            configCollection.Configuration.ExampleConfiguration.FullscreenResolution =
                FullscreenResolution.Resolution1152X648;
            configCollection.Configuration.ExampleConfiguration.Fullscreen = false;
            configCollection.Configuration.ExampleConfiguration.SomeResource = resGuid;
            await store.UpdateConfigurationAsync(configCollection);

            var retrievedConfig =
                await store.GetConfigurationAsync<ExampleConfigurationCollection>(configCollection.ValueCollection.Guid);
            Assert.NotNull(retrievedConfig);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.ISOPath0,
                retrievedConfig.Configuration.ExampleConfiguration.ISOPath0);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.FullscreenResolution,
                retrievedConfig.Configuration.ExampleConfiguration.FullscreenResolution);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.Fullscreen,
                retrievedConfig.Configuration.ExampleConfiguration.Fullscreen);
            Assert.Equal(resGuid, configCollection.Configuration.ExampleConfiguration.SomeResource);

        }

        [Fact]
        public async Task ConfigurationStoreSetIndividualAsync_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var store = new ConfigurationCollectionStore(optionsBuilder);
            var configCollection = await store.CreateConfigurationAsync<ExampleConfigurationCollection>("Test");
            var resGuid = Guid.NewGuid();

            configCollection.Configuration.ExampleConfiguration.ISOPath0 = "TestEqual";
            configCollection.Configuration.ExampleConfiguration.FullscreenResolution =
                FullscreenResolution.Resolution1152X648;
            configCollection.Configuration.ExampleConfiguration.Fullscreen = false;
            configCollection.Configuration.ExampleConfiguration.SomeResource = resGuid;

            await store.UpdateConfigurationAsync(configCollection);
            configCollection.Configuration.ExampleConfiguration.FullscreenResolution =
                FullscreenResolution.Resolution1280X768;
            await store.UpdateValueAsync(configCollection.GetSection(e => e.ExampleConfiguration).Values["FullscreenResolution"]);

            var retrievedConfig =
                await store.GetConfigurationAsync<ExampleConfigurationCollection>(configCollection.ValueCollection.Guid);
            Assert.NotNull(retrievedConfig);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.ISOPath0,
                retrievedConfig.Configuration.ExampleConfiguration.ISOPath0);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.FullscreenResolution,
                retrievedConfig.Configuration.ExampleConfiguration.FullscreenResolution);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.Fullscreen,
                retrievedConfig.Configuration.ExampleConfiguration.Fullscreen);
            Assert.Equal(resGuid, configCollection.Configuration.ExampleConfiguration.SomeResource);

        }

        [Fact]
        public async Task ConfigurationStoreMultipleProfileIndividualAsync_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var store = new ConfigurationCollectionStore(optionsBuilder);
            var configCollection = await store.CreateConfigurationAsync<ExampleConfigurationCollection>("Test");

            configCollection.Configuration.ExampleConfiguration.ISOPath0 = "TestEqual";
            configCollection.Configuration.ExampleConfiguration.FullscreenResolution =
                FullscreenResolution.Resolution1152X648;
            configCollection.Configuration.ExampleConfiguration.Fullscreen = false;

            var otherConfigCollection = store.CreateConfiguration<ExampleConfigurationCollection>("Test");
            otherConfigCollection.Configuration.ExampleConfiguration.ISOPath0 = "TestNotEqual";
            otherConfigCollection.Configuration.ExampleConfiguration.FullscreenResolution =
                FullscreenResolution.Resolution1280X1024;
            otherConfigCollection.Configuration.ExampleConfiguration.Fullscreen = true;

            await store.UpdateConfigurationAsync(configCollection);
            await store.UpdateConfigurationAsync(otherConfigCollection);

            var retrievedConfig =
                await store.GetConfigurationAsync<ExampleConfigurationCollection>(configCollection.ValueCollection.Guid);
            var newRetrievedConfig =
                await store.GetConfigurationAsync<ExampleConfigurationCollection>(otherConfigCollection.ValueCollection.Guid);

            Assert.NotNull(retrievedConfig);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.ISOPath0,
                retrievedConfig.Configuration.ExampleConfiguration.ISOPath0);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.FullscreenResolution,
                retrievedConfig.Configuration.ExampleConfiguration.FullscreenResolution);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.Fullscreen,
                retrievedConfig.Configuration.ExampleConfiguration.Fullscreen);

            Assert.NotEqual(retrievedConfig.Configuration.ExampleConfiguration.FullscreenResolution,
                newRetrievedConfig.Configuration.ExampleConfiguration.FullscreenResolution);
            Assert.NotEqual(retrievedConfig.Configuration.ExampleConfiguration.ISOPath0,
                newRetrievedConfig.Configuration.ExampleConfiguration.ISOPath0);
            Assert.NotEqual(retrievedConfig.Configuration.ExampleConfiguration.Fullscreen,
                newRetrievedConfig.Configuration.ExampleConfiguration.Fullscreen);
        }

        [Fact]
        public async Task ConfigurationStoreGetDefaultAsync_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var store = new ConfigurationCollectionStore(optionsBuilder);
            var configCollection = await store.CreateConfigurationAsync<ExampleConfigurationCollection>("Test");
            var retrievedConfig = await
                store.GetConfigurationAsync<ExampleConfigurationCollection>(configCollection.ValueCollection.Guid);

            Assert.NotNull(retrievedConfig);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.ISOPath0,
                retrievedConfig.Configuration.ExampleConfiguration.ISOPath0);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.FullscreenResolution,
                retrievedConfig.Configuration.ExampleConfiguration.FullscreenResolution);
            Assert.Equal(configCollection.Configuration.ExampleConfiguration.Fullscreen,
                retrievedConfig.Configuration.ExampleConfiguration.Fullscreen);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Snowflake.Configuration;
using Snowflake.Extensibility.Configuration;
using Snowflake.Model.Database;
using Snowflake.Model.Database.Models;
using Xunit;

namespace Snowflake.Extensibility.Tests
{
    public class EntityPluginConfigurationStoreTests
    {
        [Fact]
        public void ConfigurationStoreSet_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var store = new PluginConfigurationStore(optionsBuilder);

            var configSection = store.Get<ExampleConfigurationSection>();
            store.Set(configSection);
        }

        [Fact]
        public void ConfigurationStoreGet_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var store = new PluginConfigurationStore(optionsBuilder);

            var configSection = store.Get<ExampleConfigurationSection>();
            store.Set(configSection);

            var retrievedConfig = store.Get<ExampleConfigurationSection>();
            Assert.NotNull(retrievedConfig);
            Assert.Equal(configSection.Configuration.ISOPath0, retrievedConfig.Configuration.ISOPath0);
            Assert.Equal(configSection.Configuration.FullscreenResolution,
                retrievedConfig.Configuration.FullscreenResolution);
            Assert.Equal(configSection.Configuration.Fullscreen, retrievedConfig.Configuration.Fullscreen);
        }

        [Fact]
        public void ConfigurationStoreSetIndividual_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var store = new PluginConfigurationStore(optionsBuilder);

            var configSection = store.Get<ExampleConfigurationSection>();
            store.Set(configSection);
            configSection.Configuration.ISOPath0 = "TestEqual";
            configSection.Configuration.FullscreenResolution = FullscreenResolution.Resolution1152X648;
            configSection.Configuration.Fullscreen = false;

            store.Set(configSection);
            configSection.Configuration.FullscreenResolution = FullscreenResolution.Resolution1280X768;
            store.Set(configSection.Values["FullscreenResolution"].Guid, configSection.Values["FullscreenResolution"].Value);

            var retrievedConfig = store.Get<ExampleConfigurationSection>();
            Assert.NotNull(retrievedConfig);
            Assert.NotNull(retrievedConfig);
            Assert.Equal(configSection.Configuration.ISOPath0, retrievedConfig.Configuration.ISOPath0);
            Assert.Equal(configSection.Configuration.FullscreenResolution,
                retrievedConfig.Configuration.FullscreenResolution);
            Assert.Equal(configSection.Configuration.Fullscreen, retrievedConfig.Configuration.Fullscreen);
        }

        [Fact]
        public void ConfigurationStoreSetIndividualEnumerable_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var store = new PluginConfigurationStore(optionsBuilder);

            var configSection = store.Get<ExampleConfigurationSection>();
            store.Set(configSection);
            configSection.Configuration.ISOPath0 = "TestEqual";
            configSection.Configuration.FullscreenResolution = FullscreenResolution.Resolution1152X648;
            configSection.Configuration.Fullscreen = false;

            store.Set(configSection);
            configSection.Configuration.FullscreenResolution = FullscreenResolution.Resolution1280X768;
            configSection.Configuration.Fullscreen = true;
            store.Set(new[]
            {
                (configSection.Values["FullscreenResolution"].Guid, configSection.Values["FullscreenResolution"].Value),
                (configSection.Values["Fullscreen"].Guid, configSection.Values["Fullscreen"].Value),
            });

            var retrievedConfig = store.Get<ExampleConfigurationSection>();
            Assert.NotNull(retrievedConfig);
            Assert.NotNull(retrievedConfig);
            Assert.Equal(configSection.Configuration.ISOPath0, retrievedConfig.Configuration.ISOPath0);
            Assert.Equal(configSection.Configuration.FullscreenResolution,
                retrievedConfig.Configuration.FullscreenResolution);
            Assert.Equal(configSection.Configuration.Fullscreen, retrievedConfig.Configuration.Fullscreen);
        }

        [Fact]
        public async Task ConfigurationStoreSetAsync_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var store = new PluginConfigurationStore(optionsBuilder);

            var configSection = await store.GetAsync<ExampleConfigurationSection>();
            await store.SetAsync(configSection);
        }

        [Fact]
        public async Task ConfigurationStoreGetAsync_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var store = new PluginConfigurationStore(optionsBuilder);

            var configSection = await store.GetAsync<ExampleConfigurationSection>();
            await store.SetAsync(configSection);

            var retrievedConfig = await store.GetAsync<ExampleConfigurationSection>();
            Assert.NotNull(retrievedConfig);
            Assert.Equal(configSection.Configuration.ISOPath0, retrievedConfig.Configuration.ISOPath0);
            Assert.Equal(configSection.Configuration.FullscreenResolution,
                retrievedConfig.Configuration.FullscreenResolution);
            Assert.Equal(configSection.Configuration.Fullscreen, retrievedConfig.Configuration.Fullscreen);
        }

        [Fact]
        public async Task ConfigurationStoreSetIndividualAsync_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var store = new PluginConfigurationStore(optionsBuilder);

            var configSection = await store.GetAsync<ExampleConfigurationSection>();
            await store.SetAsync(configSection);
            configSection.Configuration.ISOPath0 = "TestEqual";
            configSection.Configuration.FullscreenResolution = FullscreenResolution.Resolution1152X648;
            configSection.Configuration.Fullscreen = false;

            await store.SetAsync(configSection);
            configSection.Configuration.FullscreenResolution = FullscreenResolution.Resolution1280X768;
            await store.SetAsync(configSection.Values["FullscreenResolution"].Guid, configSection.Values["FullscreenResolution"].Value);

            var retrievedConfig =await store.GetAsync<ExampleConfigurationSection>();
            Assert.NotNull(retrievedConfig);
            Assert.NotNull(retrievedConfig);
            Assert.Equal(configSection.Configuration.ISOPath0, retrievedConfig.Configuration.ISOPath0);
            Assert.Equal(configSection.Configuration.FullscreenResolution,
                retrievedConfig.Configuration.FullscreenResolution);
            Assert.Equal(configSection.Configuration.Fullscreen, retrievedConfig.Configuration.Fullscreen);
        }

        [Fact]
        public async Task ConfigurationStoreSetIndividualEnumerableAsync_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var store = new PluginConfigurationStore(optionsBuilder);

            var configSection = await store.GetAsync<ExampleConfigurationSection>();
            await store.SetAsync(configSection);
            configSection.Configuration.ISOPath0 = "TestEqual";
            configSection.Configuration.FullscreenResolution = FullscreenResolution.Resolution1152X648;
            configSection.Configuration.Fullscreen = false;

            await store.SetAsync(configSection);
            configSection.Configuration.FullscreenResolution = FullscreenResolution.Resolution1280X768;
            configSection.Configuration.Fullscreen = true;
            await store.SetAsync(new[]
            {
                (configSection.Values["FullscreenResolution"].Guid, configSection.Values["FullscreenResolution"].Value),
                (configSection.Values["Fullscreen"].Guid, configSection.Values["Fullscreen"].Value),
            });

            var retrievedConfig = await store.GetAsync<ExampleConfigurationSection>();
            Assert.NotNull(retrievedConfig);
            Assert.NotNull(retrievedConfig);
            Assert.Equal(configSection.Configuration.ISOPath0, retrievedConfig.Configuration.ISOPath0);
            Assert.Equal(configSection.Configuration.FullscreenResolution,
                retrievedConfig.Configuration.FullscreenResolution);
            Assert.Equal(configSection.Configuration.Fullscreen, retrievedConfig.Configuration.Fullscreen);
        }
    }
}

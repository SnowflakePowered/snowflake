using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Snowflake.Configuration.Tests;
using Snowflake.Model.Database;
using Snowflake.Model.Database.Models;
using Xunit;

namespace Snowflake.Model.Tests
{
    public class ConfigurationStoreTests
    {
        [Fact]
        public void ConfigurationStore_CreateAndRetrieve_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var configStore = new ConfigurationCollectionStore(optionsBuilder);
            var gameGuid = Guid.NewGuid();
            var config = configStore
                .CreateConfiguration<ExampleConfigurationCollection>();
            var retrieved = configStore.GetConfiguration<ExampleConfigurationCollection>
                (config.ValueCollection.Guid);
        }

        [Fact]
        public void ConfigurationStore_CreateAndRetrieveEnsure_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var configStore = new ConfigurationCollectionStore(optionsBuilder);
            var gameGuid = Guid.NewGuid();
            var config = configStore
                .CreateConfiguration<ExampleConfigurationCollection>();
            // trigger an ensure of the ExampleConfiguration
            var res = config.Configuration.ExampleConfiguration.FullscreenResolution;
            configStore.UpdateConfiguration(config);
            var retrieved = configStore.GetConfiguration<ExampleConfigurationCollection>
                (config.ValueCollection.Guid);
        }
    }
}

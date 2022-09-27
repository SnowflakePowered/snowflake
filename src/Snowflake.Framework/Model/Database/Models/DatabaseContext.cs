using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Snowflake.Model.Game;

#nullable disable
namespace Snowflake.Model.Database.Models
{
    internal class DatabaseContext : DbContext
    {
        public DbSet<GameRecordModel> GameRecords { get; set; }
        public DbSet<FileRecordModel> FileRecords { get; set; }

        public DbSet<RecordModel> Records { get; set; }
        public DbSet<RecordMetadataModel> Metadata { get; set; }

        public DbSet<ConfigurationProfileModel> ConfigurationProfiles { get; set; }
        public DbSet<ConfigurationValueModel> ConfigurationValues { get; set; }
        public DbSet<GameRecordConfigurationProfileModel> GameRecordsConfigurationProfiles { get; set; }

        public DbSet<ControllerElementMappingCollectionModel> ControllerElementMappings { get; set; }
        public DbSet<ControllerElementMappingModel> MappedControllerElements { get; set; }

        public DbSet<PortDeviceEntryModel> PortDeviceEntries { get; set; }

        public DbSet<ContentLibraryModel> ContentLibraries { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ControllerElementMappingModel.SetupModel(modelBuilder);
            ControllerElementMappingCollectionModel.SetupModel(modelBuilder);

            ConfigurationValueModel.SetupModel(modelBuilder);
            ConfigurationProfileModel.SetupModel(modelBuilder);

            RecordModel.SetupModel(modelBuilder);
            RecordMetadataModel.SetupModel(modelBuilder);

            GameRecordModel.SetupModel(modelBuilder);
            FileRecordModel.SetupModel(modelBuilder);

            GameRecordConfigurationProfileModel.SetupModel(modelBuilder);

            PortDeviceEntryModel.SetupModel(modelBuilder);

            ContentLibraryModel.SetupModel(modelBuilder);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Snowflake.Model.Game;

namespace Snowflake.Model.Database.Models
{
    internal class DatabaseContext : DbContext
    {
        public DbSet<GameRecordModel> GameRecords { get; set; }
        public DbSet<FileRecordModel> FileRecords { get; set; }

        public DbSet<RecordModel> Records { get; set; }
        public DbSet<RecordMetadataModel>? Metadata { get; set; }

        public DbSet<ConfigurationProfileModel> ConfigurationProfiles { get; set; }
        public DbSet<ConfigurationValueModel> ConfigurationValues { get; set; }
        public DbSet<GameRecordConfigurationProfileModel> GameRecordsConfigurationProfiles { get; set; }

        public DbSet<ControllerElementMappingsModel> ControllerElementMappings { get; set; }
        public DbSet<MappedControllerElementModel> MappedControllerElements { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MappedControllerElementModel.SetupModel(modelBuilder);
            ControllerElementMappingsModel.SetupModel(modelBuilder);

            ConfigurationValueModel.SetupModel(modelBuilder);
            ConfigurationProfileModel.SetupModel(modelBuilder);

            RecordModel.SetupModel(modelBuilder);
            RecordMetadataModel.SetupModel(modelBuilder);

            GameRecordModel.SetupModel(modelBuilder);
            FileRecordModel.SetupModel(modelBuilder);

            GameRecordConfigurationProfileModel.SetupModel(modelBuilder);
        }
    }
}

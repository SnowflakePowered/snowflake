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

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<GameRecordModel>()
                .Property(r => r.Platform)
                .HasConversion(p => p.PlatformIdString,
                    p => new PlatformId(p))
                .IsRequired();

            modelBuilder.Entity<FileRecordModel>()
                .Property(r => r.MimeType)
                .IsRequired();

            modelBuilder.Entity<RecordModel>()
                .HasKey(r => r.RecordID);

        
            modelBuilder.Entity<RecordModel>()
                .Property(r => r.RecordID)
                .IsRequired();

            modelBuilder.Entity<RecordModel>()
                .Property(r => r.RecordType)
                .IsRequired();

            modelBuilder.Entity<RecordMetadataModel>()
                .HasOne(r => r.Record)
                .WithMany(r => r.Metadata);


            modelBuilder.Entity<RecordMetadataModel>()
                .HasKey(r => r.RecordMetadataID);

            modelBuilder.Entity<RecordMetadataModel>()
               .Property(r => r.MetadataKey)
               .IsRequired();

            modelBuilder.Entity<RecordMetadataModel>()
                .Property(r => r.RecordMetadataID)
                .IsRequired();

            modelBuilder.Entity<RecordMetadataModel>()
                  .Property(r => r.RecordID)
                  .IsRequired();
            modelBuilder.Entity<RecordMetadataModel>()
                  .Property(r => r.MetadataValue)
                  .IsRequired();
            

        }
    }
}
